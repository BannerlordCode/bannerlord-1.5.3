using System;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002F2 RID: 754
	public interface ISiegeEventVisual
	{
		// Token: 0x06002943 RID: 10563
		void Initialize();

		// Token: 0x06002944 RID: 10564
		void OnSiegeEventEnd();

		// Token: 0x06002945 RID: 10565
		void Tick();
	}
}
