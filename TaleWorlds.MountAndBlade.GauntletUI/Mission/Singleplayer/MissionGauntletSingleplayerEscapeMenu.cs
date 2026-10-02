using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003E RID: 62
	[OverrideView(typeof(MissionSingleplayerEscapeMenu))]
	public class MissionGauntletSingleplayerEscapeMenu : MissionGauntletEscapeMenuBase
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x00010E40 File Offset: 0x0000F040
		public MissionGauntletSingleplayerEscapeMenu(bool isIronmanMode)
			: base("EscapeMenu")
		{
			this._isIronmanMode = isIronmanMode;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00010E54 File Offset: 0x0000F054
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionOptionsComponent = base.Mission.GetMissionBehavior<MissionOptionsComponent>();
			this.DataSource = new EscapeMenuVM(null, null);
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00010EA5 File Offset: 0x0000F0A5
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00010ECD File Offset: 0x0000F0CD
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.HideBattleUI)
			{
				EscapeMenuVM dataSource = this.DataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.RefreshItems(this.GetEscapeMenuItems());
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00010EEC File Offset: 0x0000F0EC
		public override void OnFocusChangeOnGameWindow(bool focusGained)
		{
			base.OnFocusChangeOnGameWindow(focusGained);
			if (!focusGained && BannerlordConfig.StopGameOnFocusLost && base.MissionScreen.IsOpeningEscapeMenuOnFocusChangeAllowed() && !GameStateManager.Current.ActiveStateDisabledByUser && !LoadingWindow.IsLoadingWindowActive && !base.IsActive)
			{
				this.OnEscape();
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00010F39 File Offset: 0x0000F139
		public override void OnSceneRenderingStarted()
		{
			base.OnSceneRenderingStarted();
			if (base.MissionScreen.IsFocusLost)
			{
				this.OnFocusChangeOnGameWindow(false);
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00010F58 File Offset: 0x0000F158
		protected override List<EscapeMenuItemVM> GetEscapeMenuItems()
		{
			TextObject ironmanDisabledReason = GameTexts.FindText("str_pause_menu_disabled_hint", "IronmanMode");
			List<EscapeMenuItemVM> list = new List<EscapeMenuItemVM>();
			list.Add(new EscapeMenuItemVM(new TextObject("{=e139gKZc}Return to the Game", null), delegate(object o)
			{
				this.OnEscapeMenuToggled(false);
			}, null, () => new Tuple<bool, TextObject>(false, null), true));
			list.Add(new EscapeMenuItemVM(new TextObject("{=NqarFr4P}Options", null), delegate(object o)
			{
				this.OnEscapeMenuToggled(false);
				MissionOptionsComponent missionOptionsComponent = this._missionOptionsComponent;
				if (missionOptionsComponent == null)
				{
					return;
				}
				missionOptionsComponent.OnAddOptionsUIHandler();
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			if (BannerlordConfig.HideBattleUI)
			{
				list.Add(new EscapeMenuItemVM(new TextObject("{=asCeKZXx}Re-enable Battle UI", null), delegate(object o)
				{
					ManagedOptions.SetConfig(ManagedOptions.ManagedOptionsType.HideBattleUI, 0f);
					ManagedOptions.SaveConfig();
					this.DataSource.RefreshItems(this.GetEscapeMenuItems());
				}, null, () => new Tuple<bool, TextObject>(false, null), false));
			}
			if (TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				MissionCheatView missionBehavior = base.Mission.GetMissionBehavior<MissionCheatView>();
				if (missionBehavior != null && missionBehavior.GetIsCheatsAvailable())
				{
					list.Add(new EscapeMenuItemVM(new TextObject("{=WA6Sk6cH}Cheat Menu", null), delegate(object o)
					{
						this.MissionScreen.Mission.GetMissionBehavior<MissionCheatView>().InitializeScreen();
					}, null, () => new Tuple<bool, TextObject>(false, null), false));
				}
			}
			list.Add(new EscapeMenuItemVM(new TextObject("{=VklN5Wm6}Photo Mode", null), delegate(object o)
			{
				this.OnEscapeMenuToggled(false);
				this.MissionScreen.SetPhotoModeEnabled(true);
				this.Mission.IsInPhotoMode = true;
				InformationManager.HideAllMessages();
			}, null, () => this.GetIsPhotoModeDisabled(), false));
			Action <>9__12;
			list.Add(new EscapeMenuItemVM(new TextObject("{=RamV6yLM}Exit to Main Menu", null), delegate(object o)
			{
				Game game = Game.Current;
				if (!(((game != null) ? game.GameType : null) is EditorGame))
				{
					Game game2 = Game.Current;
					if (!(((game2 != null) ? game2.GameType.GetType().Name : null) == "CustomGame"))
					{
						string text = GameTexts.FindText("str_exit", null).ToString();
						string text2 = GameTexts.FindText("str_mission_exit_query", null).ToString();
						bool flag = true;
						bool flag2 = true;
						string text3 = GameTexts.FindText("str_yes", null).ToString();
						string text4 = GameTexts.FindText("str_no", null).ToString();
						Action action = new Action(this.OnExitToMainMenu);
						Action action2;
						if ((action2 = <>9__12) == null)
						{
							action2 = (<>9__12 = delegate
							{
								this.OnEscapeMenuToggled(false);
							});
						}
						InformationManager.ShowInquiry(new InquiryData(text, text2, flag, flag2, text3, text4, action, action2, "", 0f, null, null, null), false, false);
						return;
					}
				}
				this.OnExitToMainMenu();
			}, null, () => new Tuple<bool, TextObject>(this._isIronmanMode, ironmanDisabledReason), false));
			return list;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00011124 File Offset: 0x0000F324
		private Tuple<bool, TextObject> GetIsPhotoModeDisabled()
		{
			if (base.MissionScreen.IsDeploymentActive)
			{
				return new Tuple<bool, TextObject>(true, new TextObject("{=rZSjkCpw}Cannot use photo mode during deployment.", null));
			}
			if (base.MissionScreen.IsConversationActive)
			{
				return new Tuple<bool, TextObject>(true, new TextObject("{=ImQnhIQ5}Cannot use photo mode during conversation.", null));
			}
			if (base.MissionScreen.IsPhotoModeEnabled)
			{
				return new Tuple<bool, TextObject>(true, new TextObject("{=79bODbwZ}Photo mode is already active.", null));
			}
			if (Module.CurrentModule.IsOnlyCoreContentEnabled)
			{
				return new Tuple<bool, TextObject>(true, new TextObject("{=V8BXjyYq}Disabled during installation.", null));
			}
			if (base.MissionScreen.Mission.Mode == MissionMode.CutScene)
			{
				return new Tuple<bool, TextObject>(true, new TextObject("{=WazbgBDJ}Disabled during cutscenes.", null));
			}
			if (base.MissionScreen.CustomCamera != null || !base.MissionScreen.IsPhotoModeAllowed())
			{
				return new Tuple<bool, TextObject>(true, new TextObject("{=7xU2wu7M}Photo mode isn't allowed.", null));
			}
			return new Tuple<bool, TextObject>(false, null);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001120B File Offset: 0x0000F40B
		private void OnExitToMainMenu()
		{
			base.OnEscapeMenuToggled(false);
			InformationManager.HideInquiry();
			MBGameManager.EndGame();
		}

		// Token: 0x0400017D RID: 381
		private MissionOptionsComponent _missionOptionsComponent;

		// Token: 0x0400017E RID: 382
		private bool _isIronmanMode;
	}
}
