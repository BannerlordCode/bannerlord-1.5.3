using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CE RID: 1230
	public static class DeclareWarAction
	{
		// Token: 0x06004D59 RID: 19801 RVA: 0x001872D0 File Offset: 0x001854D0
		private static void ApplyInternal(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			FactionManager.DeclareWar(faction1, faction2);
			if (faction1.IsKingdomFaction && (float)faction2.Fiefs.Count > 1f + (float)faction1.Fiefs.Count * 0.2f)
			{
				Kingdom kingdom = (Kingdom)faction1;
				kingdom.PoliticalStagnation = (int)((float)kingdom.PoliticalStagnation * 0.85f - 3f);
				if (kingdom.PoliticalStagnation < 0)
				{
					kingdom.PoliticalStagnation = 0;
				}
			}
			if (faction2.IsKingdomFaction && (float)faction1.Fiefs.Count > 1f + (float)faction2.Fiefs.Count * 0.2f)
			{
				Kingdom kingdom2 = (Kingdom)faction2;
				kingdom2.PoliticalStagnation = (int)((float)kingdom2.PoliticalStagnation * 0.85f - 3f);
				if (kingdom2.PoliticalStagnation < 0)
				{
					kingdom2.PoliticalStagnation = 0;
				}
			}
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
			CampaignEventDispatcher.Instance.OnWarDeclared(faction1, faction2, declareWarDetail);
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x001874CC File Offset: 0x001856CC
		public static void ApplyByKingdomDecision(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByKingdomDecision);
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x001874D6 File Offset: 0x001856D6
		public static void ApplyByDefault(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.Default);
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x001874E0 File Offset: 0x001856E0
		public static void ApplyByPlayerHostility(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByPlayerHostility);
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x001874EA File Offset: 0x001856EA
		public static void ApplyByRebellion(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByRebellion);
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x001874F4 File Offset: 0x001856F4
		public static void ApplyByCrimeRatingChange(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByCrimeRatingChange);
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x001874FE File Offset: 0x001856FE
		public static void ApplyByKingdomCreation(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByKingdomCreation);
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x00187508 File Offset: 0x00185708
		public static void ApplyByClaimOnThrone(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByClaimOnThrone);
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x00187512 File Offset: 0x00185712
		public static void ApplyByCallToWarAgreement(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByCallToWarAgreement);
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x0018751C File Offset: 0x0018571C
		public static void ApplyByQuest(IFaction faction1, IFaction faction2)
		{
			DeclareWarAction.ApplyInternal(faction1, faction2, DeclareWarAction.DeclareWarDetail.CausedByQuest);
		}

		// Token: 0x020008DF RID: 2271
		public enum DeclareWarDetail
		{
			// Token: 0x0400266D RID: 9837
			Default,
			// Token: 0x0400266E RID: 9838
			CausedByPlayerHostility,
			// Token: 0x0400266F RID: 9839
			CausedByKingdomDecision,
			// Token: 0x04002670 RID: 9840
			CausedByRebellion,
			// Token: 0x04002671 RID: 9841
			CausedByCrimeRatingChange,
			// Token: 0x04002672 RID: 9842
			CausedByKingdomCreation,
			// Token: 0x04002673 RID: 9843
			CausedByClaimOnThrone,
			// Token: 0x04002674 RID: 9844
			CausedByCallToWarAgreement,
			// Token: 0x04002675 RID: 9845
			CausedByQuest
		}
	}
}
