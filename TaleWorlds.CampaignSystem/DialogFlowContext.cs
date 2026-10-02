using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000088 RID: 136
	internal class DialogFlowContext
	{
		// Token: 0x0600115A RID: 4442 RVA: 0x00053F1B File Offset: 0x0005211B
		public DialogFlowContext(string token, bool byPlayer, DialogFlowContext parent, bool optionsUsedOnlyOnce)
		{
			this.Token = token;
			this.ByPlayer = byPlayer;
			this.Parent = parent;
			this.OptionsUsedOnlyOnce = optionsUsedOnlyOnce;
		}

		// Token: 0x04000566 RID: 1382
		internal readonly string Token;

		// Token: 0x04000567 RID: 1383
		internal readonly bool ByPlayer;

		// Token: 0x04000568 RID: 1384
		internal readonly DialogFlowContext Parent;

		// Token: 0x04000569 RID: 1385
		internal readonly bool OptionsUsedOnlyOnce;
	}
}
