using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CA RID: 1226
	public class ChangeRulingClanAction
	{
		// Token: 0x06004D43 RID: 19779 RVA: 0x00186EF4 File Offset: 0x001850F4
		private static void ApplyInternal(Kingdom kingdom, Clan newRulerClan)
		{
			Clan rulingClan = kingdom.RulingClan;
			kingdom.RulingClan = newRulerClan;
			CampaignEventDispatcher.Instance.OnRulingClanChanged(kingdom, rulingClan);
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x00186F1B File Offset: 0x0018511B
		public static void Apply(Kingdom kingdom, Clan clan)
		{
			ChangeRulingClanAction.ApplyInternal(kingdom, clan);
		}
	}
}
