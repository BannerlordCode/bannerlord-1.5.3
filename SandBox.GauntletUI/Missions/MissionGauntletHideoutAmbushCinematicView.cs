using System;
using SandBox.View.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000020 RID: 32
	[OverrideView(typeof(MissionHideoutAmbushCinematicView))]
	public class MissionGauntletHideoutAmbushCinematicView : MissionHideoutAmbushCinematicView
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0000BCBA File Offset: 0x00009EBA
		public MissionGauntletHideoutAmbushCinematicView()
		{
			this._gauntletLayer = new MissionGauntletHideoutAmbushCinematicView.HideoutAmbushCutsceneGauntletLayer(10, false);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000BCE9 File Offset: 0x00009EE9
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000BD04 File Offset: 0x00009F04
		protected override void SetPlayerMovementEnabled(bool isPlayerMovementEnabled)
		{
			base.SetPlayerMovementEnabled(isPlayerMovementEnabled);
			for (int i = 0; i < base.Mission.MissionBehaviors.Count; i++)
			{
				MissionBattleUIBaseView missionBattleUIBaseView;
				if ((missionBattleUIBaseView = base.Mission.MissionBehaviors[i] as MissionBattleUIBaseView) != null)
				{
					if (!isPlayerMovementEnabled)
					{
						missionBattleUIBaseView.SuspendView();
					}
					else
					{
						missionBattleUIBaseView.ResumeView();
					}
				}
			}
			if (isPlayerMovementEnabled)
			{
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				return;
			}
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
		}

		// Token: 0x0400008E RID: 142
		private MissionGauntletHideoutAmbushCinematicView.HideoutAmbushCutsceneGauntletLayer _gauntletLayer;

		// Token: 0x02000080 RID: 128
		private class HideoutAmbushCutsceneGauntletLayer : GauntletLayer
		{
			// Token: 0x06000461 RID: 1121 RVA: 0x00018D37 File Offset: 0x00016F37
			public HideoutAmbushCutsceneGauntletLayer(int localOrder, bool shouldClear = false)
				: base("MissionHideoutAmbushCutscene", localOrder, shouldClear)
			{
			}

			// Token: 0x06000462 RID: 1122 RVA: 0x00018D46 File Offset: 0x00016F46
			public override bool HitTest()
			{
				return true;
			}
		}
	}
}
