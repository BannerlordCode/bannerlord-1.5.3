using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000008 RID: 8
	public class GauntletChatLogView : GlobalLayer
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003790 File Offset: 0x00001990
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00003797 File Offset: 0x00001997
		public static GauntletChatLogView Current { get; private set; }

		// Token: 0x06000033 RID: 51 RVA: 0x000037A0 File Offset: 0x000019A0
		public GauntletChatLogView()
		{
			this._dataSource = new MPChatVM();
			this._dataSource.SetGetKeyTextFromKeyIDFunc(new Func<TextObject>(this.GetToggleChatKeyText));
			this._dataSource.SetGetCycleChannelKeyTextFunc(new Func<TextObject>(this.GetCycleChannelsKeyText));
			this._dataSource.SetGetSendMessageKeyTextFunc(new Func<TextObject>(this.GetSendMessageKeyText));
			this._dataSource.SetGetCancelSendingKeyTextFunc(new Func<TextObject>(this.GetCancelSendingKeyText));
			this._dataSource.SetChatDisabledStateChangedCallback(new Action<bool>(this.OnChatDisabledStateChanged));
			GauntletLayer gauntletLayer = new GauntletLayer("ChatLog", 15300, false);
			this._movie = gauntletLayer.LoadMovie("SPChatLog", this._dataSource);
			gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ChatLogHotKeyCategory"));
			base.Layer = gauntletLayer;
			this._chatLogMessageManager = new ChatLogMessageManager(this._dataSource);
			MessageManager.SetMessageManager(this._chatLogMessageManager);
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionsChanged));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000038E9 File Offset: 0x00001AE9
		public static void Initialize()
		{
			if (GauntletChatLogView.Current == null)
			{
				GauntletChatLogView.Current = new GauntletChatLogView();
				ScreenManager.AddGlobalLayer(GauntletChatLogView.Current, false);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003908 File Offset: 0x00001B08
		private void OnManagedOptionsChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			bool flag = changedManagedOptionsType == ManagedOptions.ManagedOptionsType.HideBattleUI && Mission.Current != null && BannerlordConfig.HideBattleUI;
			bool flag2 = changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox && !GameNetwork.IsMultiplayer && !BannerlordConfig.EnableSingleplayerChatBox;
			bool flag3 = changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox && GameNetwork.IsMultiplayer && !BannerlordConfig.EnableMultiplayerChatBox;
			if (flag || flag2 || flag3)
			{
				this._dataSource.Clear();
				this.CloseChat();
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003970 File Offset: 0x00001B70
		private void CloseChat()
		{
			if (!this._dataSource.IsTypingText && !this._dataSource.IsInspectingMessages && !base.Layer.IsFocusLayer)
			{
				return;
			}
			if (this._dataSource.IsInspectingMessages)
			{
				this._dataSource.StopInspectingMessages();
			}
			else if (this._dataSource.IsTypingText)
			{
				this._dataSource.StopTyping(true);
			}
			this.UpdateFocusLayer();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000039E0 File Offset: 0x00001BE0
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._dataSource.IsChatAllowedByOptions())
			{
				this._chatLogMessageManager.Update();
			}
			this._dataSource.UpdateObjects(Game.Current, Mission.Current);
			this._dataSource.Tick(dt);
			this._dataSource.ShouldHaveOffset = this.GetShouldHaveOffset();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003A40 File Offset: 0x00001C40
		protected override void OnLateTick(float dt)
		{
			base.OnLateTick(dt);
			bool flag = false;
			bool flag2 = false;
			if (!this._isEnabled || this._dataSource.IsChatDisabled)
			{
				MPChatVM dataSource = this._dataSource;
				if (dataSource != null && dataSource.IsInspectingMessages)
				{
					flag2 = true;
					this._dataSource.StopTyping(this._dataSource.IsChatDisabled);
				}
			}
			if (this._isEnabled)
			{
				MPChatVM dataSource2 = this._dataSource;
				if (dataSource2 != null && dataSource2.IsChatAllowedByOptions())
				{
					this.HandleInput(ref flag, ref flag2);
				}
			}
			MPChatVM dataSource3 = this._dataSource;
			if ((dataSource3 == null || !dataSource3.IsInspectingMessages) && base.Layer.InputRestrictions.MouseVisibility)
			{
				base.Layer.InputRestrictions.SetMouseVisibility(false);
			}
			if (flag || flag2)
			{
				this.OnChatOpenedOrClosed(flag, flag2);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003B08 File Offset: 0x00001D08
		private bool GetShouldHaveOffset()
		{
			if (!this._dataSource.IsTypingText && !this._dataSource.IsInspectingMessages)
			{
				Mission mission = Mission.Current;
				if (mission != null && mission.IsOrderMenuOpen && Mission.Current.Mode != MissionMode.Deployment)
				{
					return !Input.IsGamepadActive;
				}
			}
			return false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003B5C File Offset: 0x00001D5C
		private void HandleInput(ref bool chatOpened, ref bool chatClosed)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = true;
			bool flag4 = true;
			InputContext inputContext = null;
			this._isTeamChatAvailable = true;
			IChatLogHandlerScreen chatLogHandlerScreen;
			if ((chatLogHandlerScreen = ScreenManager.TopScreen as IChatLogHandlerScreen) != null)
			{
				chatLogHandlerScreen.TryUpdateChatLogLayerParameters(ref this._isTeamChatAvailable, ref flag, ref flag2, ref flag4, ref inputContext);
				this._dataSource.ShowHideShowHint = flag2;
			}
			if (flag4 != base.Layer.InputRestrictions.MouseVisibility)
			{
				base.Layer.InputRestrictions.SetMouseVisibility(flag4);
			}
			GauntletLayer gauntletLayer;
			if ((gauntletLayer = ScreenManager.FocusedLayer as GauntletLayer) != null && gauntletLayer != base.Layer && gauntletLayer.UIContext.EventManager.FocusedWidget is EditableTextWidget)
			{
				flag = false;
			}
			if (flag)
			{
				GameKeyContext category = HotKeyManager.GetCategory("ChatLogHotKeyCategory");
				if (inputContext != null && !inputContext.IsCategoryRegistered(category))
				{
					inputContext.RegisterHotKeyCategory(category);
				}
				if (flag3)
				{
					if (this._dataSource.IsInspectingMessages)
					{
						if (base.Layer.Input.IsHotKeyReleased("ToggleEscapeMenu") || base.Layer.Input.IsHotKeyReleased("Exit"))
						{
							bool isGamepadActive = Input.IsGamepadActive;
							this._dataSource.StopTyping(isGamepadActive);
							chatClosed = true;
						}
						else if (base.Layer.Input.IsGameKeyReleased(8) || base.Layer.Input.IsHotKeyReleased("FinalizeChatAlternative") || base.Layer.Input.IsHotKeyReleased("SendMessage"))
						{
							if ((Input.IsGamepadActive && base.Layer.Input.IsHotKeyReleased("SendMessage")) || !Input.IsGamepadActive)
							{
								this._dataSource.SendCurrentlyTypedMessage();
							}
							this._dataSource.StopTyping(false);
							chatClosed = true;
						}
						if (base.Layer.Input.IsHotKeyReleased("CycleChatTypes"))
						{
							if (this._dataSource.ActiveChannelType == ChatChannelType.Team)
							{
								this._dataSource.TypeToChannelAll(false);
								return;
							}
							if (this._dataSource.ActiveChannelType == ChatChannelType.All && this._isTeamChatAvailable)
							{
								this._dataSource.TypeToChannelTeam(false);
								return;
							}
						}
					}
					else if (inputContext != null)
					{
						if (this._canFocusWhileInMission && inputContext.IsGameKeyReleased(6))
						{
							this._dataSource.TypeToChannelAll(true);
							chatOpened = true;
						}
						else if (this._canFocusWhileInMission && this._isTeamChatAvailable && inputContext.IsGameKeyReleased(7))
						{
							this._dataSource.TypeToChannelTeam(true);
							chatOpened = true;
						}
						if (this._canFocusWhileInMission && (inputContext.IsGameKeyReleased(8) || inputContext.IsHotKeyReleased("FinalizeChatAlternative")))
						{
							if (this._dataSource.ActiveChannelType == ChatChannelType.None)
							{
								this._dataSource.TypeToChannelAll(true);
							}
							else
							{
								this._dataSource.StartTyping();
							}
							chatOpened = true;
							return;
						}
					}
				}
				else if (this._canFocusWhileInMission && inputContext != null && (inputContext.IsGameKeyReleased(8) || inputContext.IsHotKeyReleased("FinalizeChatAlternative")))
				{
					if (!this._dataSource.IsInspectingMessages)
					{
						this._dataSource.StartInspectingMessages();
						chatOpened = true;
						return;
					}
					this._dataSource.StopInspectingMessages();
					chatClosed = true;
					return;
				}
			}
			else
			{
				if (this._dataSource.IsTypingText)
				{
					this._dataSource.StopTyping(false);
					chatClosed = true;
					return;
				}
				if (this._dataSource.IsInspectingMessages)
				{
					this._dataSource.StopInspectingMessages();
					chatClosed = true;
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003E98 File Offset: 0x00002098
		private void OnChatOpenedOrClosed(bool chatOpened, bool chatClosed)
		{
			this.UpdateFocusLayer();
			MissionScreen missionScreen = ScreenManager.TopScreen as MissionScreen;
			if (missionScreen != null && missionScreen.SceneLayer != null)
			{
				missionScreen.Mission.GetMissionBehavior<MissionMainAgentController>().IsChatOpen = chatOpened && !chatClosed;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003EDC File Offset: 0x000020DC
		private void UpdateFocusLayer()
		{
			if (this._dataSource.IsTypingText || this._dataSource.IsInspectingMessages)
			{
				if (this._dataSource.IsTypingText && !base.Layer.IsFocusLayer)
				{
					base.Layer.IsFocusLayer = true;
					ScreenManager.TrySetFocus(base.Layer);
				}
				base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				return;
			}
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			base.Layer.InputRestrictions.ResetInputRestrictions();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003F6E File Offset: 0x0000216E
		public void SetCanFocusWhileInMission(bool canFocusInMission)
		{
			this._canFocusWhileInMission = canFocusInMission;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003F77 File Offset: 0x00002177
		public void OnSupportedFeaturesReceived(SupportedFeatures supportedFeatures)
		{
			this.SetEnabled(supportedFeatures.SupportsFeatures(Features.TextChat));
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003F87 File Offset: 0x00002187
		public void SetEnabled(bool isEnabled)
		{
			if (this._isEnabled != isEnabled)
			{
				this._isEnabled = isEnabled;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003F9C File Offset: 0x0000219C
		public void LoadMovie(bool forMultiplayer)
		{
			if (this._movie != null)
			{
				GauntletLayer gauntletLayer = base.Layer as GauntletLayer;
				if (gauntletLayer != null)
				{
					gauntletLayer.ReleaseMovie(this._movie);
				}
			}
			if (forMultiplayer)
			{
				Game game = Game.Current;
				if (game != null)
				{
					ChatBox gameHandler = game.GetGameHandler<ChatBox>();
					if (gameHandler != null)
					{
						gameHandler.InitializeForMultiplayer();
					}
				}
				GauntletLayer gauntletLayer2 = base.Layer as GauntletLayer;
				this._movie = ((gauntletLayer2 != null) ? gauntletLayer2.LoadMovie("MPChatLog", this._dataSource) : null);
				this._dataSource.SetMessageHistoryCapacity(100);
				return;
			}
			this.SetEnabled(true);
			Game game2 = Game.Current;
			if (game2 != null)
			{
				game2.GetGameHandler<ChatBox>().InitializeForSinglePlayer();
			}
			GauntletLayer gauntletLayer3 = base.Layer as GauntletLayer;
			this._movie = ((gauntletLayer3 != null) ? gauntletLayer3.LoadMovie("SPChatLog", this._dataSource) : null);
			this._dataSource.ChatBoxSizeX = BannerlordConfig.ChatBoxSizeX;
			this._dataSource.ChatBoxSizeY = BannerlordConfig.ChatBoxSizeY;
			this._dataSource.SetMessageHistoryCapacity(250);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004098 File Offset: 0x00002298
		private TextObject GetToggleChatKeyText()
		{
			if (Input.IsGamepadActive)
			{
				Game game = Game.Current;
				if (game == null)
				{
					return null;
				}
				GameTextManager gameTextManager = game.GameTextManager;
				if (gameTextManager == null)
				{
					return null;
				}
				return gameTextManager.GetHotKeyGameTextFromKeyID("controllerloption");
			}
			else
			{
				Game game2 = Game.Current;
				if (game2 == null)
				{
					return null;
				}
				GameTextManager gameTextManager2 = game2.GameTextManager;
				if (gameTextManager2 == null)
				{
					return null;
				}
				return gameTextManager2.GetHotKeyGameTextFromKeyID("enter");
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000040ED File Offset: 0x000022ED
		private TextObject GetCycleChannelsKeyText()
		{
			Game game = Game.Current;
			TextObject textObject;
			if (game == null)
			{
				textObject = null;
			}
			else
			{
				GameTextManager gameTextManager = game.GameTextManager;
				textObject = ((gameTextManager != null) ? gameTextManager.GetHotKeyGameText("ChatLogHotKeyCategory", "CycleChatTypes") : null);
			}
			return textObject ?? TextObject.GetEmpty();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000411F File Offset: 0x0000231F
		private TextObject GetSendMessageKeyText()
		{
			Game game = Game.Current;
			TextObject textObject;
			if (game == null)
			{
				textObject = null;
			}
			else
			{
				GameTextManager gameTextManager = game.GameTextManager;
				textObject = ((gameTextManager != null) ? gameTextManager.GetHotKeyGameText("ChatLogHotKeyCategory", "SendMessage") : null);
			}
			return textObject ?? TextObject.GetEmpty();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004151 File Offset: 0x00002351
		private TextObject GetCancelSendingKeyText()
		{
			Game game = Game.Current;
			TextObject textObject;
			if (game == null)
			{
				textObject = null;
			}
			else
			{
				GameTextManager gameTextManager = game.GameTextManager;
				textObject = ((gameTextManager != null) ? gameTextManager.GetHotKeyGameText("GenericPanelGameKeyCategory", "Exit") : null);
			}
			return textObject ?? TextObject.GetEmpty();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004183 File Offset: 0x00002383
		private void OnChatDisabledStateChanged(bool chatDisabled)
		{
			if (!chatDisabled)
			{
				this._dataSource.StopTyping(true);
				this.OnChatOpenedOrClosed(false, true);
			}
		}

		// Token: 0x04000031 RID: 49
		private MPChatVM _dataSource;

		// Token: 0x04000032 RID: 50
		private ChatLogMessageManager _chatLogMessageManager;

		// Token: 0x04000033 RID: 51
		private bool _canFocusWhileInMission = true;

		// Token: 0x04000034 RID: 52
		private bool _isTeamChatAvailable;

		// Token: 0x04000035 RID: 53
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000036 RID: 54
		private bool _isEnabled = true;

		// Token: 0x04000037 RID: 55
		private const int MaxHistoryCountForSingleplayer = 250;

		// Token: 0x04000038 RID: 56
		private const int MaxHistoryCountForMultiplayer = 100;
	}
}
