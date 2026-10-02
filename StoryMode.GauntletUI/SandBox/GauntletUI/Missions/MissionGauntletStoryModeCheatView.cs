using System;
using System.Collections.Generic;
using SandBox.ViewModelCollection.Map.Cheat;
using StoryMode.GameComponents.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000002 RID: 2
	[OverrideView(typeof(MissionCheatView))]
	public class MissionGauntletStoryModeCheatView : MissionCheatView
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this.FinalizeScreen();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public override bool GetIsCheatsAvailable()
		{
			Campaign campaign = Campaign.Current;
			AchievementsCampaignBehavior achievementsCampaignBehavior = ((campaign != null) ? campaign.GetCampaignBehavior<AchievementsCampaignBehavior>() : null);
			TextObject textObject;
			return achievementsCampaignBehavior == null || !achievementsCampaignBehavior.CheckAchievementSystemActivity(out textObject);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002090 File Offset: 0x00000290
		public override void InitializeScreen()
		{
			if (this._isActive)
			{
				return;
			}
			this._isActive = true;
			IEnumerable<GameplayCheatBase> missionCheatList = GameplayCheatsManager.GetMissionCheatList();
			this._dataSource = new GameplayCheatsVM(new Action(this.FinalizeScreen), missionCheatList);
			this.InitializeKeyVisuals();
			this._gauntletLayer = new GauntletLayer("MapCheats", 4500, false);
			this._gauntletLayer.LoadMovie("MapCheats", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002154 File Offset: 0x00000354
		public override void FinalizeScreen()
		{
			if (!this._isActive)
			{
				return;
			}
			this._isActive = false;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			GameplayCheatsVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._gauntletLayer = null;
			this._dataSource = null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021A1 File Offset: 0x000003A1
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isActive)
			{
				this.HandleInput();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021B8 File Offset: 0x000003B8
		private void HandleInput()
		{
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				GameplayCheatsVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.ExecuteClose();
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021EB File Offset: 0x000003EB
		private void InitializeKeyVisuals()
		{
			this._dataSource.SetCloseInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
		}

		// Token: 0x04000001 RID: 1
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000002 RID: 2
		private GameplayCheatsVM _dataSource;

		// Token: 0x04000003 RID: 3
		private bool _isActive;
	}
}
