using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200008D RID: 141
	public class FaceGeneratorMissionView : MissionView
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x00026FE5 File Offset: 0x000251E5
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (base.Input.IsGameKeyPressed(37) && !GameNetwork.IsSessionActive)
			{
				LoadingWindow.EnableGlobalLoadingWindow();
				ScreenManager.PushScreen(ViewCreator.CreateMBFaceGeneratorScreen(Game.Current.PlayerTroop, false, null));
			}
		}
	}
}
