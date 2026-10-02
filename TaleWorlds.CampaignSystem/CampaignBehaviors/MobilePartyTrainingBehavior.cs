using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043C RID: 1084
	public class MobilePartyTrainingBehavior : CampaignBehaviorBase
	{
		// Token: 0x060045D7 RID: 17879 RVA: 0x001531B0 File Offset: 0x001513B0
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnDailyTickParty));
			CampaignEvents.PlayerUpgradedTroopsEvent.AddNonSerializedListener(this, new Action<CharacterObject, CharacterObject, int>(this.OnPlayerUpgradedTroops));
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x00153202 File Offset: 0x00151402
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x00153204 File Offset: 0x00151404
		private void OnPlayerUpgradedTroops(CharacterObject troop, CharacterObject upgrade, int number)
		{
			SkillLevelingManager.OnUpgradeTroops(PartyBase.MainParty, troop, upgrade, number);
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x00153214 File Offset: 0x00151414
		private void HourlyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.LeaderHero != null)
			{
				if (mobileParty.BesiegerCamp != null)
				{
					SkillLevelingManager.OnSieging(mobileParty);
				}
				if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty && mobileParty.AttachedParties.Count > 0)
				{
					SkillLevelingManager.OnLeadingArmy(mobileParty);
				}
				if (mobileParty.IsActive)
				{
					this.WorkSkills(mobileParty);
				}
			}
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x00153270 File Offset: 0x00151470
		private void WorkSkills(MobileParty mobileParty)
		{
			if (!mobileParty.IsMoving)
			{
				MobileParty attachedTo = mobileParty.AttachedTo;
				if (attachedTo == null || !attachedTo.IsMoving)
				{
					goto IL_003C;
				}
			}
			this.CheckScouting(mobileParty);
			if (CampaignTime.Now.GetHourOfDay % 4 == 1)
			{
				this.CheckMovementSkills(mobileParty);
			}
			IL_003C:
			if (mobileParty.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue && mobileParty.MemberRoster.TotalRegulars > 0)
			{
				SkillLevelingManager.OnHighMorale(mobileParty);
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x001532EC File Offset: 0x001514EC
		private void OnDailyTickParty(MobileParty mobileParty)
		{
			foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero)
				{
					ExplainedNumber effectiveDailyExperience = Campaign.Current.Models.PartyTrainingModel.GetEffectiveDailyExperience(mobileParty, troopRosterElement);
					mobileParty.Party.MemberRoster.AddXpToTroop(troopRosterElement.Character, MathF.Round(effectiveDailyExperience.ResultNumber * (float)troopRosterElement.Number));
				}
			}
			Hero hero = null;
			if (!mobileParty.IsDisbanding && mobileParty.HasPerk(DefaultPerks.Bow.Trainer, out hero, false))
			{
				Hero hero2 = null;
				int num = int.MaxValue;
				foreach (TroopRosterElement troopRosterElement2 in mobileParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement2.Character.IsHero)
					{
						int skillValue = troopRosterElement2.Character.HeroObject.GetSkillValue(DefaultSkills.Bow);
						if (skillValue < num)
						{
							num = skillValue;
							hero2 = troopRosterElement2.Character.HeroObject;
						}
					}
				}
				if (hero2 != null)
				{
					hero2.AddSkillXp(DefaultSkills.Bow, DefaultPerks.Bow.Trainer.PrimaryBonus);
				}
			}
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x00153450 File Offset: 0x00151650
		private void CheckScouting(MobileParty mobileParty)
		{
			if (mobileParty.EffectiveScout != null && !mobileParty.IsCurrentlyAtSea)
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(mobileParty.CurrentNavigationFace);
				if (mobileParty != MobileParty.MainParty)
				{
					SkillLevelingManager.OnAIPartiesTravel(mobileParty.EffectiveScout, mobileParty.IsCaravan, faceTerrainType);
				}
				SkillLevelingManager.OnTraverseTerrain(mobileParty, faceTerrainType);
			}
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x001534A4 File Offset: 0x001516A4
		private void CheckMovementSkills(MobileParty mobileParty)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				if (mobileParty.IsCurrentlyAtSea)
				{
					if (!mobileParty.IsInNavalAutoTravel)
					{
						SkillLevelingManager.OnTravelOnWater(mobileParty);
						return;
					}
					return;
				}
				else
				{
					using (List<TroopRosterElement>.Enumerator enumerator = mobileParty.MemberRoster.GetTroopRoster().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TroopRosterElement troopRosterElement = enumerator.Current;
							if (troopRosterElement.Character.IsHero)
							{
								if (troopRosterElement.Character.Equipment.Horse.IsEmpty)
								{
									SkillLevelingManager.OnTravelOnFoot(troopRosterElement.Character.HeroObject);
								}
								else
								{
									SkillLevelingManager.OnTravelOnHorse(troopRosterElement.Character.HeroObject);
								}
							}
						}
						return;
					}
				}
			}
			if (mobileParty.LeaderHero != null)
			{
				if (mobileParty.IsCurrentlyAtSea)
				{
					SkillLevelingManager.OnTravelOnWater(mobileParty);
					return;
				}
				if (mobileParty.LeaderHero.CharacterObject.Equipment.Horse.IsEmpty)
				{
					SkillLevelingManager.OnTravelOnFoot(mobileParty.LeaderHero);
					return;
				}
				SkillLevelingManager.OnTravelOnHorse(mobileParty.LeaderHero);
			}
		}
	}
}
