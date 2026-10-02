using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C9 RID: 969
	public static class TraitLevelingHelper
	{
		// Token: 0x060038F0 RID: 14576 RVA: 0x000EABEC File Offset: 0x000E8DEC
		public static void UpdateTraitXPAccordingToTraitLevels()
		{
			foreach (TraitObject traitObject in TraitObject.All)
			{
				int traitLevel = Hero.MainHero.GetTraitLevel(traitObject);
				if (traitLevel != 0)
				{
					int traitXpRequiredForTraitLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetTraitXpRequiredForTraitLevel(traitObject, traitLevel);
					Campaign.Current.PlayerTraitDeveloper.SetPropertyValue(traitObject, traitXpRequiredForTraitLevel);
				}
			}
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x000EAC70 File Offset: 0x000E8E70
		public static void OnBattleWon(MapEvent mapEvent, float contribution)
		{
			float strengthRatio = mapEvent.GetMapEventSide(PlayerEncounter.Current.PlayerSide).StrengthRatio;
			if (strengthRatio > 9f)
			{
				int num = (int)(MBMath.Map(strengthRatio, 9f, 10f, 5f, 20f) * contribution);
				if (num > 0)
				{
					TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Valor, num, ActionNotes.BattleValor, null);
				}
			}
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x000EACCB File Offset: 0x000E8ECB
		public static void OnTroopsSacrificed()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Valor, -30, ActionNotes.SacrificedTroops, null);
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x000EACDC File Offset: 0x000E8EDC
		public static void OnBloodFeudStarted(Hero executedHero)
		{
			if (executedHero.GetTraitLevel(DefaultTraits.Honor) >= 0)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, -1000, ActionNotes.SacrificedTroops, null);
			}
			else
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Mercy, -500, ActionNotes.SacrificedTroops, null);
			}
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Mercy, -500, ActionNotes.SacrificedTroops, null);
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x000EAD2F File Offset: 0x000E8F2F
		public static void OnTradeAgreementBroken()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, -1000, ActionNotes.DishonestBusinessQuarrel, null);
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x000EAD42 File Offset: 0x000E8F42
		public static void OnVillageRaided()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Mercy, -30, ActionNotes.VillageRaid, null);
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x000EAD53 File Offset: 0x000E8F53
		public static void OnHostileAction(int amount)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, amount, ActionNotes.HostileAction, null);
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Mercy, amount, ActionNotes.HostileAction, null);
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x000EAD71 File Offset: 0x000E8F71
		public static void OnPartyTreatedWell()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Generosity, 20, ActionNotes.PartyTakenCareOf, null);
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x000EAD82 File Offset: 0x000E8F82
		public static void OnPartyStarved()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Generosity, -20, ActionNotes.PartyHungry, null);
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x000EAD94 File Offset: 0x000E8F94
		public static void OnIssueFailed(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestFailed, targetHero);
			}
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x000EADCC File Offset: 0x000E8FCC
		public static void OnIssueSolvedThroughQuest(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestSuccess, targetHero);
			}
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x000EAE01 File Offset: 0x000E9001
		public static void OnIssueSolvedThroughQuest(Hero targetHero, TraitObject trait, int xp)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(trait, xp, ActionNotes.QuestSuccess, targetHero);
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x000EAE10 File Offset: 0x000E9010
		public static void OnIssueSolvedThroughAlternativeSolution(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestSuccess, targetHero);
			}
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x000EAE48 File Offset: 0x000E9048
		public static void OnIssueSolvedThroughBetrayal(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestBetrayal, targetHero);
			}
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x000EAE7D File Offset: 0x000E907D
		public static void OnLordFreed(Hero targetHero)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Calculating, 20, ActionNotes.NPCFreed, targetHero);
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x000EAE8E File Offset: 0x000E908E
		public static void OnPersuasionDefection(Hero targetHero)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Calculating, 20, ActionNotes.PersuadedToDefect, targetHero);
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x000EAEA0 File Offset: 0x000E90A0
		public static void OnSiegeAftermathApplied(Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, TraitObject[] effectedTraits)
		{
			foreach (TraitObject traitObject in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(traitObject, Campaign.Current.Models.SiegeAftermathModel.GetSiegeAftermathTraitXpChangeForPlayer(traitObject, settlement, aftermathType), ActionNotes.SiegeAftermath, null);
			}
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x000EAEE1 File Offset: 0x000E90E1
		public static void OnIncidentResolved(TraitObject trait, int xpValue)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(trait, xpValue, ActionNotes.DefaultNote, Hero.MainHero);
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x000EAEF0 File Offset: 0x000E90F0
		public static void OnAllianceBrokenThroughHostility()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, -1000, ActionNotes.DishonestBusinessQuarrel, null);
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x000EAF04 File Offset: 0x000E9104
		private static void AddPlayerTraitXPAndLogEntry(TraitObject trait, int xpValue, ActionNotes context, Hero referenceHero)
		{
			int traitLevel = Hero.MainHero.GetTraitLevel(trait);
			TraitLevelingHelper.AddTraitXp(trait, xpValue);
			if (traitLevel != Hero.MainHero.GetTraitLevel(trait))
			{
				CampaignEventDispatcher.Instance.OnPlayerTraitChanged(trait, traitLevel);
			}
			if (MathF.Abs(xpValue) >= 10)
			{
				LogEntry.AddLogEntry(new PlayerReputationChangesLogEntry(trait, referenceHero, context));
			}
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x000EAF58 File Offset: 0x000E9158
		private static void AddTraitXp(TraitObject trait, int xpAmount)
		{
			xpAmount += Campaign.Current.PlayerTraitDeveloper.GetPropertyValue(trait);
			int num;
			int num2;
			Campaign.Current.Models.CharacterDevelopmentModel.GetTraitLevelForTraitXp(Hero.MainHero, trait, xpAmount, out num, out num2);
			Campaign.Current.PlayerTraitDeveloper.SetPropertyValue(trait, num2);
			if (num != Hero.MainHero.GetTraitLevel(trait))
			{
				Hero.MainHero.SetTraitLevel(trait, num);
			}
		}

		// Token: 0x04001179 RID: 4473
		private const int HonorableLordExecutedHonorPenalty = -1000;

		// Token: 0x0400117A RID: 4474
		private const int DishonorableLordExecutedHonorPenalty = -500;

		// Token: 0x0400117B RID: 4475
		private const int LordExecutedMercyPenalty = -500;

		// Token: 0x0400117C RID: 4476
		private const int TradeAgreementBrokenPenalty = -1000;

		// Token: 0x0400117D RID: 4477
		private const int AllianceBrokenHonorPenalty = -1000;

		// Token: 0x0400117E RID: 4478
		private const int TroopsSacrificedValorPenalty = -30;

		// Token: 0x0400117F RID: 4479
		private const int VillageRaidedMercyPenalty = -30;

		// Token: 0x04001180 RID: 4480
		private const int PartyStarvingGenerosityPenalty = -20;

		// Token: 0x04001181 RID: 4481
		private const int PartyTreatedWellGenerosityBonus = 20;

		// Token: 0x04001182 RID: 4482
		private const int LordFreedCalculatingBonus = 20;

		// Token: 0x04001183 RID: 4483
		private const int PersuasionDefectionCalculatingBonus = 20;
	}
}
