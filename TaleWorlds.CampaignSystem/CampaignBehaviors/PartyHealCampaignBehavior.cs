using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044A RID: 1098
	public class PartyHealCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600465B RID: 18011 RVA: 0x00156D48 File Offset: 0x00154F48
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanHourlyTick));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnQuarterDailyPartyTick.AddNonSerializedListener(this, new Action<MobileParty>(this.OnQuarterDailyPartyTick));
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnDailyTickSettlement));
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00156DF8 File Offset: 0x00154FF8
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._overflowedHealingForRegulars.ContainsKey(mobileParty.Party))
			{
				this._overflowedHealingForRegulars.Remove(mobileParty.Party);
				if (this._overflowedHealingForHeroes.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForHeroes.Remove(mobileParty.Party);
				}
				if (this._overflowedHealingForPrisonerRegulars.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForPrisonerRegulars.Remove(mobileParty.Party);
				}
				if (this._overflowedHealingForPrisonerHeroes.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForPrisonerHeroes.Remove(mobileParty.Party);
				}
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00156E9C File Offset: 0x0015509C
		public void OnMapEventEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsPlayerMapEvent)
			{
				this.OnBattleEndCheckPerkEffects(mapEvent);
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00156EAD File Offset: 0x001550AD
		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			this.OnBattleEndCheckPerkEffects(mapEvent);
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00156EB8 File Offset: 0x001550B8
		private void OnBattleEndCheckPerkEffects(MapEvent mapEvent)
		{
			if (mapEvent.HasWinner)
			{
				foreach (PartyBase partyBase in mapEvent.InvolvedParties)
				{
					if (partyBase.MemberRoster.TotalHeroes > 0)
					{
						foreach (TroopRosterElement troopRosterElement in partyBase.MemberRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero)
							{
								Hero heroObject = troopRosterElement.Character.HeroObject;
								int roundedResultNumber = Campaign.Current.Models.PartyHealingModel.GetBattleEndHealingAmount(partyBase, heroObject).RoundedResultNumber;
								if (roundedResultNumber > 0)
								{
									heroObject.Heal(roundedResultNumber, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00156FA8 File Offset: 0x001551A8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForRegulars", ref this._overflowedHealingForRegulars);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForHeroes", ref this._overflowedHealingForHeroes);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForPrisonerRegulars", ref this._overflowedHealingForPrisonerRegulars);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForPrisonerHeroes", ref this._overflowedHealingForPrisonerHeroes);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00156FFD File Offset: 0x001551FD
		private void OnHourlyTick()
		{
			this.TryHealOrWoundParty(MobileParty.MainParty.Party, (float)CampaignTime.HoursInDay);
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00157018 File Offset: 0x00155218
		private void OnClanHourlyTick(Clan clan)
		{
			if (!clan.IsBanditFaction)
			{
				foreach (Hero hero in clan.Heroes)
				{
					float num = 0f;
					bool flag = hero.PartyBelongedTo == null && hero.PartyBelongedToAsPrisoner == null;
					bool flag2 = hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.NotSpawned || hero.HeroState == Hero.CharacterStates.Disabled;
					if (flag && !flag2)
					{
						num = Campaign.Current.Models.PartyHealingModel.GetDailyHealingHpForHeroes(null, false, false).ResultNumber / (float)CampaignTime.HoursInDay;
					}
					int num2 = MBRandom.RoundRandomized(num);
					if (!hero.IsHealthFull())
					{
						int num3 = MathF.Min(num2, hero.MaxHitPoints - hero.HitPoints);
						hero.HitPoints += num3;
					}
				}
			}
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x00157118 File Offset: 0x00155318
		private void OnQuarterDailyPartyTick(MobileParty mobileParty)
		{
			if (!mobileParty.IsMainParty)
			{
				this.TryHealOrWoundParty(mobileParty.Party, 4f);
			}
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x00157133 File Offset: 0x00155333
		private void OnDailyTickSettlement(Settlement settlement)
		{
			this.TryHealOrWoundParty(settlement.Party, 1f);
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x00157146 File Offset: 0x00155346
		private void TryHealOrWoundParty(PartyBase partyBase, float healFrequencyPerDay)
		{
			if (partyBase.IsActive && partyBase.MapEvent == null)
			{
				this.TryToHealOrWoundMembers(partyBase, healFrequencyPerDay);
				this.TryToHealOrWoundPrisoners(partyBase, healFrequencyPerDay);
			}
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00157168 File Offset: 0x00155368
		private void TryToHealOrWoundPrisoners(PartyBase partyBase, float healFrequencyPerDay)
		{
			float num;
			if (!this._overflowedHealingForPrisonerHeroes.TryGetValue(partyBase, out num))
			{
				this._overflowedHealingForPrisonerHeroes.Add(partyBase, 0f);
			}
			float num2;
			if (!this._overflowedHealingForPrisonerRegulars.TryGetValue(partyBase, out num2))
			{
				this._overflowedHealingForPrisonerRegulars.Add(partyBase, 0f);
			}
			float num3 = Campaign.Current.Models.PartyHealingModel.GetDailyHealingHpForHeroes(partyBase, true, false).ResultNumber / healFrequencyPerDay;
			float num4 = Campaign.Current.Models.PartyHealingModel.GetDailyHealingForRegulars(partyBase, true, false).ResultNumber / healFrequencyPerDay;
			num += num3;
			num2 += num4;
			if ((int)num != 0)
			{
				this.ManageHealingOfPrisonerHeroes(partyBase, ref num);
			}
			if ((int)num2 != 0)
			{
				this.ManageHealingOfPrisonerRegulars(partyBase, ref num2);
			}
			this._overflowedHealingForPrisonerHeroes[partyBase] = num;
			this._overflowedHealingForPrisonerRegulars[partyBase] = num2;
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0015723C File Offset: 0x0015543C
		private void TryToHealOrWoundMembers(PartyBase partyBase, float healFrequencyPerDay)
		{
			float num;
			if (!this._overflowedHealingForHeroes.TryGetValue(partyBase, out num))
			{
				this._overflowedHealingForHeroes.Add(partyBase, 0f);
			}
			float num2;
			if (!this._overflowedHealingForRegulars.TryGetValue(partyBase, out num2))
			{
				this._overflowedHealingForRegulars.Add(partyBase, 0f);
			}
			float num3 = partyBase.HealingRateForMemberHeroes / healFrequencyPerDay;
			float num4 = partyBase.HealingRateForMemberRegulars / healFrequencyPerDay;
			num += num3;
			num2 += num4;
			if (num >= 1f)
			{
				PartyHealCampaignBehavior.HealMemberHeroes(partyBase, ref num);
			}
			else if (num <= -1f)
			{
				PartyHealCampaignBehavior.ReduceHpMemberHeroes(partyBase, ref num);
			}
			if (num2 >= 1f)
			{
				PartyHealCampaignBehavior.HealMemberRegulars(partyBase, ref num2);
			}
			else if (num2 <= -1f)
			{
				PartyHealCampaignBehavior.ReduceHpMemberRegulars(partyBase, ref num2);
			}
			this._overflowedHealingForHeroes[partyBase] = num;
			this._overflowedHealingForRegulars[partyBase] = num2;
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x00157304 File Offset: 0x00155504
		private void ManageHealingOfPrisonerRegulars(PartyBase partyBase, ref float prisonerRegularsHealingValue)
		{
			TroopRoster prisonRoster = partyBase.PrisonRoster;
			if (prisonRoster.TotalWoundedRegulars == 0)
			{
				prisonerRegularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(prisonerRegularsHealingValue);
			prisonerRegularsHealingValue -= (float)num;
			int num2 = MBRandom.RandomInt(prisonRoster.Count);
			int num3 = 0;
			while (num3 < prisonRoster.Count && num > 0)
			{
				int num4 = (num2 + num3) % prisonRoster.Count;
				if (prisonRoster.GetCharacterAtIndex(num4).IsRegular && prisonRoster.GetElementWoundedNumber(num4) > 0)
				{
					int num5 = MathF.Min(num, prisonRoster.GetElementWoundedNumber(num4));
					if (num5 > 0)
					{
						prisonRoster.AddToCountsAtIndex(num4, 0, -num5, 0, true);
						num -= num5;
					}
				}
				num3++;
			}
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x001573A8 File Offset: 0x001555A8
		private void ManageHealingOfPrisonerHeroes(PartyBase partyBase, ref float prisonerHeroesHealingValue)
		{
			int num = MathF.Floor(prisonerHeroesHealingValue);
			prisonerHeroesHealingValue -= (float)num;
			TroopRoster prisonRoster = partyBase.PrisonRoster;
			if (prisonRoster.TotalHeroes > 0)
			{
				for (int i = 0; i < prisonRoster.Count; i++)
				{
					Hero heroObject = prisonRoster.GetCharacterAtIndex(i).HeroObject;
					if (heroObject != null && heroObject.HitPoints < heroObject.WoundedHealthLimit)
					{
						int num2 = Math.Min(num, heroObject.WoundedHealthLimit - heroObject.HitPoints);
						heroObject.Heal(num2, false);
					}
				}
			}
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x00157424 File Offset: 0x00155624
		private static void HealMemberHeroes(PartyBase partyBase, ref float heroesHealingValue)
		{
			int num = MathF.Floor(heroesHealingValue);
			heroesHealingValue -= (float)num;
			TroopRoster memberRoster = partyBase.MemberRoster;
			if (memberRoster.TotalHeroes > 0)
			{
				for (int i = 0; i < memberRoster.Count; i++)
				{
					Hero heroObject = memberRoster.GetCharacterAtIndex(i).HeroObject;
					if (heroObject != null && !heroObject.IsHealthFull())
					{
						ExplainedNumber explainedNumber = new ExplainedNumber((float)num, false, null);
						BattleEnvironment battleEnvironment = (((heroObject.PartyBelongedTo != null && heroObject.PartyBelongedTo.IsCurrentlyAtSea) || (heroObject.PartyBelongedToAsPrisoner != null && heroObject.PartyBelongedToAsPrisoner.IsMobile && heroObject.PartyBelongedToAsPrisoner.MobileParty.IsCurrentlyAtSea)) ? BattleEnvironment.Naval : BattleEnvironment.Land);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.SelfMedication, battleEnvironment, heroObject.CharacterObject, true, ref explainedNumber);
						TraitEffectHelper.ApplyTraitEffect(heroObject, DefaultPersonalityTraitEffects.ValorInjuryRecoveryEffect, ref explainedNumber);
						heroObject.Heal(MBRandom.RoundRandomized(explainedNumber.ResultNumber), true);
					}
				}
			}
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x00157514 File Offset: 0x00155714
		private static void ReduceHpMemberHeroes(PartyBase partyBase, ref float heroesHealingValue)
		{
			int num = MathF.Ceiling(heroesHealingValue);
			heroesHealingValue = -(-heroesHealingValue % 1f);
			for (int i = 0; i < partyBase.MemberRoster.Count; i++)
			{
				Hero heroObject = partyBase.MemberRoster.GetCharacterAtIndex(i).HeroObject;
				if (heroObject != null && heroObject.HitPoints > 0)
				{
					int num2 = MathF.Min(num, heroObject.HitPoints);
					heroObject.HitPoints += num2;
				}
			}
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x00157584 File Offset: 0x00155784
		private static void HealMemberRegulars(PartyBase partyBase, ref float regularsHealingValue)
		{
			TroopRoster memberRoster = partyBase.MemberRoster;
			if (memberRoster.TotalWoundedRegulars == 0)
			{
				regularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(regularsHealingValue);
			regularsHealingValue -= (float)num;
			int num2 = 0;
			float num3 = 0f;
			int num4 = MBRandom.RandomInt(memberRoster.Count);
			int num5 = 0;
			while (num5 < memberRoster.Count && num > 0)
			{
				int num6 = (num4 + num5) % memberRoster.Count;
				CharacterObject characterAtIndex = memberRoster.GetCharacterAtIndex(num6);
				if (characterAtIndex.IsRegular)
				{
					int num7 = MathF.Min(num, memberRoster.GetElementWoundedNumber(num6));
					if (num7 > 0)
					{
						memberRoster.AddToCountsAtIndex(num6, 0, -num7, 0, true);
						num -= num7;
						num2 += num7;
						num3 += (float)(characterAtIndex.Tier * num7);
					}
				}
				num5++;
			}
			if (num2 > 0)
			{
				SkillLevelingManager.OnRegularTroopHealedWhileWaiting(partyBase.MobileParty, num2, num3 / (float)num2);
			}
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x00157658 File Offset: 0x00155858
		private static void ReduceHpMemberRegulars(PartyBase partyBase, ref float regularsHealingValue)
		{
			TroopRoster memberRoster = partyBase.MemberRoster;
			if (memberRoster.TotalRegulars - memberRoster.TotalWoundedRegulars == 0)
			{
				regularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(-regularsHealingValue);
			regularsHealingValue = -(-regularsHealingValue % 1f);
			int num2 = MBRandom.RandomInt(memberRoster.Count);
			int num3 = 0;
			while (num3 < memberRoster.Count && num > 0)
			{
				int num4 = (num2 + num3) % memberRoster.Count;
				if (memberRoster.GetCharacterAtIndex(num4).IsRegular)
				{
					int num5 = MathF.Min(memberRoster.GetElementNumber(num4) - memberRoster.GetElementWoundedNumber(num4), num);
					if (num5 > 0)
					{
						memberRoster.AddToCountsAtIndex(num4, 0, num5, 0, true);
						num -= num5;
					}
				}
				num3++;
			}
		}

		// Token: 0x04001430 RID: 5168
		private Dictionary<PartyBase, float> _overflowedHealingForRegulars = new Dictionary<PartyBase, float>();

		// Token: 0x04001431 RID: 5169
		private Dictionary<PartyBase, float> _overflowedHealingForHeroes = new Dictionary<PartyBase, float>();

		// Token: 0x04001432 RID: 5170
		private Dictionary<PartyBase, float> _overflowedHealingForPrisonerRegulars = new Dictionary<PartyBase, float>();

		// Token: 0x04001433 RID: 5171
		private Dictionary<PartyBase, float> _overflowedHealingForPrisonerHeroes = new Dictionary<PartyBase, float>();
	}
}
