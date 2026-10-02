using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A0 RID: 160
	public interface ITrackableCampaignObject : ITrackableBase
	{
		// Token: 0x0600134A RID: 4938
		Banner GetBanner();

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600134B RID: 4939
		bool IsReady { get; }
	}
}
