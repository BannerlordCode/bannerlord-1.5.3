using System;
using SandBox.Conversation.MissionLogics;
using SandBox.View.Missions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001E RID: 30
	[OverrideView(typeof(MissionConversationView))]
	public class MissionGauntletConversationView : MissionView, IConversationStateHandler
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000B498 File Offset: 0x00009698
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000B4A0 File Offset: 0x000096A0
		public MissionConversationLogic ConversationHandler { get; private set; }

		// Token: 0x060001AB RID: 427 RVA: 0x0000B4A9 File Offset: 0x000096A9
		public MissionGauntletConversationView()
		{
			this.ViewOrderPriority = 49;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000B4BC File Offset: 0x000096BC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			MissionGauntletEscapeMenuBase escapeView = this._escapeView;
			if ((escapeView == null || !escapeView.IsActive) && this._gauntletLayer != null)
			{
				SceneLayer sceneLayer = base.MissionScreen.SceneLayer;
				if (sceneLayer != null && sceneLayer.Input.IsKeyDown(InputKey.RightMouseButton))
				{
					MissionConversationCameraView conversationCameraView = this._conversationCameraView;
					if (conversationCameraView == null || !conversationCameraView.IsCameraOverridden)
					{
						this._gauntletLayer.InputRestrictions.SetMouseVisibility(false);
						goto IL_008A;
					}
				}
				this._gauntletLayer.InputRestrictions.SetMouseVisibility(true);
				IL_008A:
				if (this.IsGameKeyReleasedInAnyLayer("ContinueKey"))
				{
					MissionConversationVM dataSource = this._dataSource;
					if (dataSource != null && dataSource.AnswerList.Count <= 0 && base.Mission.Mode != MissionMode.Barter)
					{
						MissionConversationVM dataSource2 = this._dataSource;
						if (dataSource2 != null && !dataSource2.SelectedAnOptionOrLinkThisFrame)
						{
							MissionConversationVM dataSource3 = this._dataSource;
							if (dataSource3 != null)
							{
								dataSource3.ExecuteContinue();
							}
						}
					}
				}
				if (this._dataSource != null)
				{
					this._dataSource.Tick(dt);
					this._dataSource.SelectedAnOptionOrLinkThisFrame = false;
				}
				if (this._gauntletLayer != null && this.IsGameKeyReleasedInAnyLayer("ToggleEscapeMenu"))
				{
					base.MissionScreen.OnEscape();
				}
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000B5F8 File Offset: 0x000097F8
		public override void OnMissionScreenFinalize()
		{
			Campaign.Current.ConversationManager.ConversationEndOneShot -= this.OnConversationEndOneShot;
			Campaign.Current.ConversationManager.Handler = null;
			if (this._dataSource != null)
			{
				MissionConversationVM dataSource = this._dataSource;
				if (dataSource != null)
				{
					dataSource.OnFinalize();
				}
				this._dataSource = null;
			}
			this._gauntletLayer = null;
			this.ConversationHandler = null;
			Campaign.Current.ConversationManager.ConversationEndOneShot -= this.OnConversationEnded;
			if (this._blankScreenLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._blankScreenLayer);
				this._blankScreenLayer = null;
			}
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000B69F File Offset: 0x0000989F
		public override void EarlyStart()
		{
			base.EarlyStart();
			this.ConversationHandler = base.Mission.GetMissionBehavior<MissionConversationLogic>();
			this._conversationCameraView = base.Mission.GetMissionBehavior<MissionConversationCameraView>();
			Campaign.Current.ConversationManager.Handler = this;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B6D9 File Offset: 0x000098D9
		public override void OnMissionScreenActivate()
		{
			base.OnMissionScreenActivate();
			if (this._dataSource != null)
			{
				base.MissionScreen.SetLayerCategoriesStateAndDeactivateOthers(new string[] { "MissionConversation", "SceneLayer" }, true);
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000B718 File Offset: 0x00009918
		void IConversationStateHandler.OnConversationInstall()
		{
			base.MissionScreen.SetConversationActive(true);
			if (base.Mission.HasMissionBehavior<ConversationMissionLogic>())
			{
				base.MissionScreen.SetAsConversationMission();
			}
			Campaign.Current.ConversationManager.ConversationEndOneShot += this.OnConversationEndOneShot;
			this._conversationCategory = UIResourceManager.LoadSpriteCategory("ui_conversation");
			this._dataSource = new MissionConversationVM(new Func<string>(this.GetContinueKeyText), false);
			this._gauntletLayer = new GauntletLayer("MissionConversation", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("SPConversation", this._dataSource);
			GameKeyContext category = HotKeyManager.GetCategory("ConversationHotKeyCategory");
			this._gauntletLayer.Input.RegisterHotKeyCategory(category);
			if (!base.MissionScreen.SceneLayer.Input.IsCategoryRegistered(category))
			{
				base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			}
			GameKeyContext category2 = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
			this._gauntletLayer.Input.RegisterHotKeyCategory(category2);
			if (!base.MissionScreen.SceneLayer.Input.IsCategoryRegistered(category2))
			{
				base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category2);
			}
			this._gauntletLayer.IsFocusLayer = true;
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._escapeView = base.Mission.GetMissionBehavior<MissionGauntletEscapeMenuBase>();
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.MissionScreen.SetLayerCategoriesStateAndDeactivateOthers(new string[] { "MissionConversation", "SceneLayer" }, true);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			InformationManager.HideAllMessages();
			Campaign.Current.ConversationManager.ConversationEndOneShot += this.OnConversationEnded;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000B8DC File Offset: 0x00009ADC
		private void OnConversationEnded()
		{
			MissionScreen missionScreen = base.MissionScreen;
			if (((missionScreen != null) ? missionScreen.Mission : null) != null && base.MissionScreen.Mission.HasMissionBehavior<ConversationMissionLogic>() && base.MissionScreen.Mission.CurrentState != Mission.State.EndingNextFrame && base.MissionScreen.Mission.CurrentState != Mission.State.Over)
			{
				this._blankScreenLayer = new GauntletLayer("MissionConversationEnd", 10000, false);
				this._blankScreenLayer.LoadMovie("MissionReadyBlocker", new BoolItemWithActionVM(null, true, null));
				base.MissionScreen.AddLayer(this._blankScreenLayer);
			}
			Campaign.Current.ConversationManager.ConversationEndOneShot -= this.OnConversationEnded;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B990 File Offset: 0x00009B90
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			base.OnMissionModeChange(oldMissionMode, atStart);
			if (oldMissionMode == MissionMode.Barter && base.Mission.Mode == MissionMode.Conversation)
			{
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B9B7 File Offset: 0x00009BB7
		private void OnConversationEndOneShot()
		{
			if (base.MissionScreen.IsConversationMission)
			{
				base.MissionScreen.SceneView.SetEnable(false);
				this.DestroyConversationView();
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000B9E0 File Offset: 0x00009BE0
		private void DestroyConversationView()
		{
			if (this._dataSource != null)
			{
				MissionConversationVM dataSource = this._dataSource;
				if (dataSource != null)
				{
					dataSource.OnFinalize();
				}
				this._dataSource = null;
			}
			Campaign.Current.ConversationManager.ConversationEndOneShot -= this.OnConversationEnded;
			this._conversationCategory.Unload();
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			base.MissionScreen.SetLayerCategoriesStateAndToggleOthers(new string[] { "MissionConversation" }, false);
			base.MissionScreen.SetLayerCategoriesState(new string[] { "SceneLayer" }, true);
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._escapeView = null;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000BAAD File Offset: 0x00009CAD
		void IConversationStateHandler.OnConversationUninstall()
		{
			base.MissionScreen.SetConversationActive(false);
			if (this._gauntletLayer != null)
			{
				this.DestroyConversationView();
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000BACC File Offset: 0x00009CCC
		private string GetContinueKeyText()
		{
			if (TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				return GameTexts.FindText("str_click_to_continue_console", null).SetTextVariable("CONSOLE_KEY_NAME", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("ConversationHotKeyCategory", "ContinueClick"), 1f)).ToString();
			}
			return GameTexts.FindText("str_click_to_continue", null).ToString();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000BB24 File Offset: 0x00009D24
		void IConversationStateHandler.OnConversationActivate()
		{
			base.MissionScreen.SetLayerCategoriesStateAndDeactivateOthers(new string[] { "MissionConversation", "SceneLayer" }, true);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000BB48 File Offset: 0x00009D48
		void IConversationStateHandler.OnConversationDeactivate()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BB4F File Offset: 0x00009D4F
		void IConversationStateHandler.OnConversationContinue()
		{
			this._dataSource.OnConversationContinue();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000BB5C File Offset: 0x00009D5C
		void IConversationStateHandler.ExecuteConversationContinue()
		{
			this._dataSource.ExecuteContinue();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000BB6C File Offset: 0x00009D6C
		private bool IsGameKeyReleasedInAnyLayer(string hotKeyID)
		{
			bool flag = this.IsReleasedInSceneLayer(hotKeyID);
			bool flag2 = this.IsReleasedInGauntletLayer(hotKeyID);
			return flag || flag2;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000BB8A File Offset: 0x00009D8A
		private bool IsReleasedInSceneLayer(string hotKeyID)
		{
			SceneLayer sceneLayer = base.MissionScreen.SceneLayer;
			return sceneLayer != null && sceneLayer.Input.IsHotKeyReleased(hotKeyID);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		private bool IsReleasedInGauntletLayer(string hotKeyID)
		{
			GauntletLayer gauntletLayer = this._gauntletLayer;
			return gauntletLayer != null && gauntletLayer.Input.IsHotKeyReleased(hotKeyID);
		}

		// Token: 0x04000086 RID: 134
		private GauntletLayer _blankScreenLayer;

		// Token: 0x04000087 RID: 135
		private MissionConversationVM _dataSource;

		// Token: 0x04000088 RID: 136
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400008A RID: 138
		private MissionConversationCameraView _conversationCameraView;

		// Token: 0x0400008B RID: 139
		private MissionGauntletEscapeMenuBase _escapeView;

		// Token: 0x0400008C RID: 140
		private SpriteCategory _conversationCategory;
	}
}
