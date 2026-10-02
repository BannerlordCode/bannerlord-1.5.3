using System;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.CustomBattle.Views
{
	// Token: 0x02000012 RID: 18
	[OverrideView(typeof(MissionCheatView))]
	internal class GauntletCustomBattleMissionCheatView : MissionCheatView
	{
		// Token: 0x06000109 RID: 265 RVA: 0x0000869B File Offset: 0x0000689B
		public override void InitializeScreen()
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000869D File Offset: 0x0000689D
		public override void FinalizeScreen()
		{
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000869F File Offset: 0x0000689F
		public override bool GetIsCheatsAvailable()
		{
			return false;
		}
	}
}
