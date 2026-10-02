using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200045A RID: 1114
	public class PrisonerReleaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060047CB RID: 18379 RVA: 0x00160DFC File Offset: 0x0015EFFC
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyHeroTick));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyPartyTick));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeaceEvent));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.ClanChangedKingdom));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x00160E94 File Offset: 0x0015F094
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 0))
			{
				foreach (Hero hero in Hero.AllAliveHeroes)
				{
					if (hero != Hero.MainHero)
					{
						if (hero.IsPrisoner)
						{
							bool flag = hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.IsMilitia;
							bool flag2 = hero.PartyBelongedToAsPrisoner != null && !hero.PartyBelongedToAsPrisoner.MapFaction.IsAtWarWith(hero.MapFaction);
							if (hero.PartyBelongedToAsPrisoner == null)
							{
								if (hero.CurrentSettlement == null)
								{
									MakeHeroFugitiveAction.Apply(hero, false);
								}
							}
							else if (flag || flag2)
							{
								EndCaptivityAction.ApplyByEscape(hero, null, true);
								MakeHeroFugitiveAction.Apply(hero, false);
							}
						}
						else if (hero.PartyBelongedToAsPrisoner != null)
						{
							hero.PartyBelongedToAsPrisoner.PrisonRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
							MakeHeroFugitiveAction.Apply(hero, false);
						}
					}
				}
			}
		}

		// Token: 0x060047CD RID: 18381 RVA: 0x00160FCC File Offset: 0x0015F1CC
		private void ClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (detail != ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom)
			{
				PrisonerReleaseCampaignBehavior.ReleasePrisonersInternal(clan);
				if (oldKingdom != null)
				{
					PrisonerReleaseCampaignBehavior.ReleasePrisonersInternal(oldKingdom);
					foreach (IFaction faction in oldKingdom.FactionsAtWarWith)
					{
						PrisonerReleaseCampaignBehavior.ReleasePrisonersInternal(faction);
					}
				}
				if (newKingdom != null)
				{
					this.OnAfterClanJoinedKingdom(clan, newKingdom);
					PrisonerReleaseCampaignBehavior.ReleasePrisonersInternal(newKingdom);
				}
			}
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x00161044 File Offset: 0x0015F244
		private void OnAfterClanJoinedKingdom(Clan clan, Kingdom newKingdom)
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (kingdom != newKingdom && kingdom.IsAtWarWith(clan) && !kingdom.IsAtWarWith(newKingdom))
				{
					this.OnMakePeace(clan, kingdom);
				}
			}
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x001610B0 File Offset: 0x0015F2B0
		private void OnMakePeaceEvent(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			this.OnMakePeace(side1Faction, side2Faction);
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x001610BA File Offset: 0x0015F2BA
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction)
		{
			PrisonerReleaseCampaignBehavior.ReleasePrisonersInternal(side1Faction);
			PrisonerReleaseCampaignBehavior.ReleasePrisonersInternal(side2Faction);
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x001610C8 File Offset: 0x0015F2C8
		private static void ReleasePrisonersInternal(IFaction faction)
		{
			foreach (Settlement settlement in faction.Settlements)
			{
				for (int i = settlement.Party.PrisonRoster.Count - 1; i >= 0; i--)
				{
					if (settlement.Party.PrisonRoster.GetElementNumber(i) > 0)
					{
						TroopRosterElement elementCopyAtIndex = settlement.Party.PrisonRoster.GetElementCopyAtIndex(i);
						if (elementCopyAtIndex.Character.IsHero && elementCopyAtIndex.Character.HeroObject != Hero.MainHero && !elementCopyAtIndex.Character.HeroObject.MapFaction.IsAtWarWith(faction.MapFaction))
						{
							EndCaptivityAction.ApplyByPeace(elementCopyAtIndex.Character.HeroObject, null);
							CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, null, elementCopyAtIndex.Character.HeroObject, true);
						}
					}
				}
			}
			Clan clan = ((faction.IsClan || faction.IsMinorFaction) ? ((Clan)faction) : null);
			Kingdom kingdom = (faction.IsKingdomFaction ? ((Kingdom)faction) : null);
			if (clan != null)
			{
				PrisonerReleaseCampaignBehavior.ReleasePrisonersForClan(clan, faction);
				return;
			}
			if (kingdom != null)
			{
				foreach (Clan clan2 in kingdom.Clans)
				{
					PrisonerReleaseCampaignBehavior.ReleasePrisonersForClan(clan2, faction);
				}
			}
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x00161250 File Offset: 0x0015F450
		private static void ReleasePrisonersForClan(Clan clan, IFaction faction)
		{
			foreach (Hero hero in clan.AliveLords)
			{
				foreach (CaravanPartyComponent caravanPartyComponent in hero.OwnedCaravans)
				{
					PrisonerReleaseCampaignBehavior.ReleasePartyPrisoners(caravanPartyComponent.MobileParty, faction);
				}
			}
			foreach (Hero hero2 in clan.Companions)
			{
				foreach (CaravanPartyComponent caravanPartyComponent2 in hero2.OwnedCaravans)
				{
					PrisonerReleaseCampaignBehavior.ReleasePartyPrisoners(caravanPartyComponent2.MobileParty, faction);
				}
			}
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				PrisonerReleaseCampaignBehavior.ReleasePartyPrisoners(warPartyComponent.MobileParty, faction);
			}
			foreach (Settlement settlement in clan.Settlements)
			{
				if (settlement.IsVillage && settlement.Village.VillagerPartyComponent != null)
				{
					PrisonerReleaseCampaignBehavior.ReleasePartyPrisoners(settlement.Village.VillagerPartyComponent.MobileParty, faction);
				}
				else if ((settlement.IsCastle || settlement.IsTown) && settlement.Town.GarrisonParty != null)
				{
					PrisonerReleaseCampaignBehavior.ReleasePartyPrisoners(settlement.Town.GarrisonParty, faction);
				}
			}
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x00161440 File Offset: 0x0015F640
		private static void ReleasePartyPrisoners(MobileParty mobileParty, IFaction faction)
		{
			for (int i = mobileParty.PrisonRoster.Count - 1; i >= 0; i--)
			{
				if (mobileParty.Party.PrisonRoster.GetElementNumber(i) > 0)
				{
					TroopRosterElement elementCopyAtIndex = mobileParty.Party.PrisonRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsHero && elementCopyAtIndex.Character.HeroObject != Hero.MainHero && !elementCopyAtIndex.Character.HeroObject.MapFaction.IsAtWarWith(faction.MapFaction))
					{
						if (elementCopyAtIndex.Character.HeroObject.PartyBelongedToAsPrisoner == mobileParty.Party)
						{
							EndCaptivityAction.ApplyByPeace(elementCopyAtIndex.Character.HeroObject, null);
						}
						else
						{
							mobileParty.Party.PrisonRoster.RemoveTroop(elementCopyAtIndex.Character, 1, default(UniqueTroopDescriptor), 0);
						}
					}
				}
			}
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0016151C File Offset: 0x0015F71C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x00161520 File Offset: 0x0015F720
		private void DailyHeroTick(Hero hero)
		{
			if (hero.IsPrisoner && hero.PartyBelongedToAsPrisoner != null && hero != Hero.MainHero)
			{
				bool flag = true;
				CampaignEventDispatcher.Instance.CanHeroBeReleased(hero, ref flag);
				if (!flag)
				{
					return;
				}
				float num = 0.04f;
				if (hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement == null)
				{
					num *= 5f - MathF.Pow((float)MathF.Min(81, hero.PartyBelongedToAsPrisoner.NumberOfHealthyMembers), 0.25f);
				}
				if (hero.PartyBelongedToAsPrisoner == PartyBase.MainParty || (hero.PartyBelongedToAsPrisoner.IsSettlement && hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan == Clan.PlayerClan) || (hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement != null && hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement.OwnerClan == Clan.PlayerClan))
				{
					num *= 0.5f;
				}
				ExplainedNumber explainedNumber = new ExplainedNumber(num, false, null);
				if (hero.PartyBelongedToAsPrisoner.IsSettlement && hero.PartyBelongedToAsPrisoner.Settlement.Town != null)
				{
					Town town = hero.PartyBelongedToAsPrisoner.Settlement.Town;
					if (hero.PartyBelongedToAsPrisoner.Settlement.IsTown || hero.PartyBelongedToAsPrisoner.Settlement.IsCastle)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.SweetTalker, town, false, ref explainedNumber);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.DungeonArchitect, town, false, ref explainedNumber);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Riding.MountedPatrols, town, false, ref explainedNumber);
					}
				}
				if (hero.PartyBelongedToAsPrisoner.IsMobile)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.FleetFooted, BattleEnvironment.Any, hero.CharacterObject, false, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.MountedPatrols, hero.PartyBelongedToAsPrisoner.MobileParty, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.RansomBroker, hero.PartyBelongedToAsPrisoner.MobileParty, false, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.KeenSight, hero.PartyBelongedToAsPrisoner.MobileParty, false, ref explainedNumber);
					MobileParty mobileParty = hero.PartyBelongedToAsPrisoner.MobileParty;
					Army army = mobileParty.Army;
					Hero hero2;
					if (army == null)
					{
						hero2 = null;
					}
					else
					{
						MobileParty leaderParty = army.LeaderParty;
						hero2 = ((leaderParty != null) ? leaderParty.LeaderHero : null);
					}
					Hero hero3 = hero2 ?? mobileParty.LeaderHero;
					if (hero3 != null)
					{
						TraitEffectHelper.ApplyTraitEffect(hero3, DefaultPersonalityTraitEffects.ValorPrisonerEscapeEffect, ref explainedNumber);
					}
				}
				if (MBRandom.RandomFloat < explainedNumber.ResultNumber)
				{
					EndCaptivityAction.ApplyByEscape(hero, null, true);
				}
			}
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0016177C File Offset: 0x0015F97C
		private void HourlyPartyTick(MobileParty mobileParty)
		{
			int prisonerSizeLimit = mobileParty.Party.PrisonerSizeLimit;
			if (mobileParty.MapEvent == null && mobileParty.SiegeEvent == null && mobileParty.PrisonRoster.TotalManCount > prisonerSizeLimit)
			{
				int num = mobileParty.PrisonRoster.TotalManCount - prisonerSizeLimit;
				for (int i = 0; i < num; i++)
				{
					bool flag = mobileParty.PrisonRoster.TotalRegulars > 0;
					float randomFloat = MBRandom.RandomFloat;
					int num2 = (flag ? ((int)((float)mobileParty.PrisonRoster.TotalRegulars * randomFloat)) : ((int)((float)mobileParty.PrisonRoster.TotalManCount * randomFloat)));
					CharacterObject characterObject = null;
					foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
					{
						if (!troopRosterElement.Character.IsHero || !flag)
						{
							num2 -= troopRosterElement.Number;
							if (num2 <= 0)
							{
								characterObject = troopRosterElement.Character;
								break;
							}
						}
					}
					this.ApplyEscapeChanceToExceededPrisoners(characterObject, mobileParty);
				}
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0016189C File Offset: 0x0015FA9C
		private void ApplyEscapeChanceToExceededPrisoners(CharacterObject character, MobileParty capturerParty)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0.1f, false, null);
			Hero hero = null;
			if (capturerParty.HasPerk(DefaultPerks.Athletics.Stamina, out hero, true))
			{
				explainedNumber.AddFactor(-0.1f, DefaultPerks.Athletics.Stamina.Name);
			}
			if (capturerParty.IsGarrison || capturerParty.IsMilitia || character.IsPlayerCharacter)
			{
				return;
			}
			Army army = capturerParty.Army;
			bool flag;
			if (army == null)
			{
				flag = null != null;
			}
			else
			{
				MobileParty leaderParty = army.LeaderParty;
				flag = ((leaderParty != null) ? leaderParty.LeaderHero : null) != null;
			}
			if ((flag ?? capturerParty.LeaderHero) != null)
			{
				TraitEffectHelper.ApplyTraitEffect(capturerParty.LeaderHero, DefaultPersonalityTraitEffects.ValorPrisonerEscapeEffect, ref explainedNumber);
			}
			if (MBRandom.RandomFloat < explainedNumber.ResultNumber)
			{
				if (character.IsHero)
				{
					EndCaptivityAction.ApplyByEscape(character.HeroObject, null, true);
					return;
				}
				capturerParty.PrisonRoster.AddToCounts(character, -1, false, 0, 0, true, -1);
			}
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0016196C File Offset: 0x0015FB6C
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			foreach (TroopRosterElement troopRosterElement in settlement.Party.PrisonRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero && troopRosterElement.Character.HeroObject != Hero.MainHero && !troopRosterElement.Character.HeroObject.MapFaction.IsAtWarWith(settlement.MapFaction))
				{
					if (troopRosterElement.Character.HeroObject.PartyBelongedToAsPrisoner == settlement.Party && troopRosterElement.Character.HeroObject.IsPrisoner)
					{
						EndCaptivityAction.ApplyByReleasedAfterBattle(troopRosterElement.Character.HeroObject);
					}
					else
					{
						settlement.Party.PrisonRoster.RemoveTroop(troopRosterElement.Character, troopRosterElement.Number, default(UniqueTroopDescriptor), 0);
					}
				}
			}
		}
	}
}
