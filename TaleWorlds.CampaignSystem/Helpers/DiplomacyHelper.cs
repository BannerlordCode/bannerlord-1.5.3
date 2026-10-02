using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000005 RID: 5
	public static class DiplomacyHelper
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000038D8 File Offset: 0x00001AD8
		public static bool IsWarCausedByPlayer(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			switch (declareWarDetail)
			{
			case DeclareWarAction.DeclareWarDetail.CausedByPlayerHostility:
				return true;
			case DeclareWarAction.DeclareWarDetail.CausedByKingdomDecision:
				return faction1 == Hero.MainHero.MapFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero;
			case DeclareWarAction.DeclareWarDetail.CausedByCrimeRatingChange:
				return faction2 == Hero.MainHero.MapFaction && faction1.MainHeroCrimeRating > Campaign.Current.Models.CrimeModel.DeclareWarCrimeRatingThreshold;
			case DeclareWarAction.DeclareWarDetail.CausedByKingdomCreation:
				return faction1 == Hero.MainHero.MapFaction;
			}
			return false;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000396B File Offset: 0x00001B6B
		public static bool IsSameFactionAndNotEliminated(IFaction faction1, IFaction faction2)
		{
			return faction1 != null && faction2 != null && faction1 == faction2 && !faction1.IsEliminated && !faction2.IsEliminated;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000398C File Offset: 0x00001B8C
		private static bool IsLogInTimeRange(LogEntry entry, CampaignTime time)
		{
			return entry.GameTime.NumTicks >= time.NumTicks;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000039B4 File Offset: 0x00001BB4
		public static List<ValueTuple<LogEntry, IFaction, IFaction>> GetLogsForWar(StanceLink stance)
		{
			CampaignTime warStartDate = stance.WarStartDate;
			List<ValueTuple<LogEntry, IFaction, IFaction>> list = new List<ValueTuple<LogEntry, IFaction, IFaction>>();
			for (int i = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; i >= 0; i--)
			{
				LogEntry logEntry = Campaign.Current.LogEntryHistory.GameActionLogs[i];
				IWarLog warLog;
				IFaction faction;
				IFaction faction2;
				if (DiplomacyHelper.IsLogInTimeRange(logEntry, warStartDate) && (warLog = logEntry as IWarLog) != null && warLog.IsRelatedToWar(stance, out faction, out faction2))
				{
					list.Add(new ValueTuple<LogEntry, IFaction, IFaction>(logEntry, faction, faction2));
				}
			}
			return list;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003A38 File Offset: 0x00001C38
		public static List<Hero> GetPrisonersOfWarTakenByFaction(IFaction capturerFaction, IFaction prisonerFaction)
		{
			List<Hero> list = new List<Hero>();
			foreach (Hero hero in prisonerFaction.AliveLords)
			{
				if (hero.IsPrisoner)
				{
					PartyBase partyBelongedToAsPrisoner = hero.PartyBelongedToAsPrisoner;
					if (((partyBelongedToAsPrisoner != null) ? partyBelongedToAsPrisoner.MapFaction : null) == capturerFaction)
					{
						list.Add(hero);
					}
				}
			}
			return list;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003AB0 File Offset: 0x00001CB0
		public static bool DidMainHeroSwornNotToAttackFaction(IFaction faction, out TextObject explanation)
		{
			if (faction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				explanation = GameTexts.FindText("str_enemy_not_attackable_tooltip", null);
				return true;
			}
			explanation = null;
			return false;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003AE0 File Offset: 0x00001CE0
		public static bool HasAllianceWithFaction(IFaction faction1, IFaction faction2)
		{
			if (faction1 == null || faction2 == null || faction1 == faction2 || faction1.IsEliminated || faction2.IsEliminated || !faction1.IsKingdomFaction || !faction2.IsKingdomFaction)
			{
				return false;
			}
			Kingdom kingdom = (Kingdom)faction1;
			Kingdom kingdom2 = (Kingdom)faction2;
			return kingdom.IsAllyWith(kingdom2);
		}
	}
}
