using System;
using SandBox.View.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001F RID: 31
	[OverrideView(typeof(EavesdroppingMissionCameraView))]
	public class MissionGauntletEavesdroppingCameraView : EavesdroppingMissionCameraView
	{
		// Token: 0x060001BE RID: 446 RVA: 0x0000BBC1 File Offset: 0x00009DC1
		public MissionGauntletEavesdroppingCameraView()
		{
			this._gauntletLayer = new MissionGauntletEavesdroppingCameraView.EavesdroppingGauntletLayer(10, false);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000BBD7 File Offset: 0x00009DD7
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000BC0C File Offset: 0x00009E0C
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

		// Token: 0x0400008D RID: 141
		private MissionGauntletEavesdroppingCameraView.EavesdroppingGauntletLayer _gauntletLayer;

		// Token: 0x0200007F RID: 127
		private class EavesdroppingGauntletLayer : GauntletLayer
		{
			// Token: 0x0600045F RID: 1119 RVA: 0x00018D25 File Offset: 0x00016F25
			public EavesdroppingGauntletLayer(int localOrder, bool shouldClear = false)
				: base("MissionEavesdropping", localOrder, shouldClear)
			{
			}

			// Token: 0x06000460 RID: 1120 RVA: 0x00018D34 File Offset: 0x00016F34
			public override bool HitTest()
			{
				return true;
			}
		}
	}
}
