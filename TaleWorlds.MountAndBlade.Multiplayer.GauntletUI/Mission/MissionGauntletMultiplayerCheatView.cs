using System;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000012 RID: 18
	[OverrideView(typeof(MissionCheatView))]
	public class MissionGauntletMultiplayerCheatView : MissionCheatView
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00005CA6 File Offset: 0x00003EA6
		public override bool GetIsCheatsAvailable()
		{
			return false;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005CA9 File Offset: 0x00003EA9
		public override void InitializeScreen()
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005CAB File Offset: 0x00003EAB
		public override void FinalizeScreen()
		{
		}
	}
}
