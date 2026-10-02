using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C2 RID: 1218
	public static class ChangeGovernorAction
	{
		// Token: 0x06004D18 RID: 19736 RVA: 0x00185FA4 File Offset: 0x001841A4
		private static void ApplyInternal(Town fortification, Hero governor)
		{
			Hero governor2 = fortification.Governor;
			if (governor == null)
			{
				fortification.Governor = null;
			}
			else if (governor.CurrentSettlement == fortification.Settlement && !governor.IsPrisoner)
			{
				fortification.Governor = governor;
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(governor, fortification.Settlement);
			}
			else
			{
				fortification.Governor = null;
				TeleportHeroAction.ApplyDelayedTeleportToSettlementAsGovernor(governor, fortification.Settlement);
			}
			if (governor2 != null)
			{
				governor2.GovernorOf = null;
			}
			CampaignEventDispatcher.Instance.OnGovernorChanged(fortification, governor2, governor);
			if (governor != null)
			{
				CampaignEventDispatcher.Instance.OnHeroGetsBusy(governor, HeroGetsBusyReasons.BecomeGovernor);
			}
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0018602C File Offset: 0x0018422C
		private static void ApplyGiveUpInternal(Hero governor)
		{
			Town governorOf = governor.GovernorOf;
			governorOf.Governor = null;
			governor.GovernorOf = null;
			CampaignEventDispatcher.Instance.OnGovernorChanged(governorOf, governor, null);
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0018605B File Offset: 0x0018425B
		public static void Apply(Town fortification, Hero governor)
		{
			ChangeGovernorAction.ApplyInternal(fortification, governor);
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x00186064 File Offset: 0x00184264
		public static void RemoveGovernorOf(Hero governor)
		{
			ChangeGovernorAction.ApplyGiveUpInternal(governor);
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0018606C File Offset: 0x0018426C
		public static void RemoveGovernorOfIfExists(Town town)
		{
			if (town.Governor != null)
			{
				ChangeGovernorAction.ApplyGiveUpInternal(town.Governor);
			}
		}
	}
}
