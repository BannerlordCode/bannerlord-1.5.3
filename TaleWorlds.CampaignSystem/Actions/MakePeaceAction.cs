using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E5 RID: 1253
	public static class MakePeaceAction
	{
		// Token: 0x06004DC7 RID: 19911 RVA: 0x00189484 File Offset: 0x00187684
		private static void ApplyInternal(IFaction faction1, IFaction faction2, int dailyTributeFrom1To2, int dailyTributeDuration, MakePeaceAction.MakePeaceDetail detail = MakePeaceAction.MakePeaceDetail.Default)
		{
			StanceLink stanceWith = faction1.GetStanceWith(faction2);
			FactionManager.SetNeutral(faction1, faction2);
			stanceWith.SetDailyTributePaid(faction1, dailyTributeFrom1To2, dailyTributeDuration);
			if (faction1 == Hero.MainHero.MapFaction || faction2 == Hero.MainHero.MapFaction)
			{
				IFaction dirtySide = ((faction1 == Hero.MainHero.MapFaction) ? faction2 : faction1);
				IEnumerable<Settlement> all = Settlement.All;
				Func<Settlement, bool> func;
				Func<Settlement, bool> <>9__0;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (Settlement party) => party.IsVisible && party.MapFaction == dirtySide);
				}
				foreach (Settlement settlement in all.Where<Settlement>(func))
				{
					settlement.Party.SetVisualAsDirty();
				}
				IEnumerable<MobileParty> all2 = MobileParty.All;
				Func<MobileParty, bool> func2;
				Func<MobileParty, bool> <>9__1;
				if ((func2 = <>9__1) == null)
				{
					func2 = (<>9__1 = (MobileParty party) => party.IsVisible && party.MapFaction == dirtySide);
				}
				foreach (MobileParty mobileParty in all2.Where<MobileParty>(func2))
				{
					mobileParty.Party.SetVisualAsDirty();
				}
			}
			CampaignEventDispatcher.Instance.OnMakePeace(faction1, faction2, detail);
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x001895C4 File Offset: 0x001877C4
		public static void Apply(IFaction faction1, IFaction faction2)
		{
			MakePeaceAction.ApplyInternal(faction1, faction2, 0, 0, MakePeaceAction.MakePeaceDetail.Default);
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x001895D0 File Offset: 0x001877D0
		public static void ApplyByKingdomDecision(IFaction faction1, IFaction faction2, int dailyTributeFrom1To2, int dailyTributeDuration)
		{
			MakePeaceAction.ApplyInternal(faction1, faction2, dailyTributeFrom1To2, dailyTributeDuration, MakePeaceAction.MakePeaceDetail.ByKingdomDecision);
		}

		// Token: 0x040015A1 RID: 5537
		private const float DefaultValueForBeingLimitedAfterPeace = 100000f;

		// Token: 0x020008EC RID: 2284
		public enum MakePeaceDetail
		{
			// Token: 0x040026AB RID: 9899
			Default,
			// Token: 0x040026AC RID: 9900
			ByKingdomDecision
		}
	}
}
