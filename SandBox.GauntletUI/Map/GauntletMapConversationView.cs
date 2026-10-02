using System;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Barter;
using TaleWorlds.CampaignSystem.ViewModelCollection.Conversation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapConversation;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000034 RID: 52
	[OverrideView(typeof(MapConversationView))]
	public class GauntletMapConversationView : MapConversationView, IConversationStateHandler
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000F27E File Offset: 0x0000D47E
		public GauntletMapConversationView()
		{
			this._barterManager = Campaign.Current.BarterManager;
			this._conversationCategory = UIResourceManager.GetSpriteCategory("ui_conversation");
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F2A6 File Offset: 0x0000D4A6
		private void OnBarterActiveStateChanged(bool isBarterActive)
		{
			this._dataSource.IsBarterActive = isBarterActive;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		protected override void InitializeConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			base.InitializeConversation(playerCharacterData, conversationPartnerData);
			this._playerCharacterData = playerCharacterData;
			this._conversationPartnerData = conversationPartnerData;
			this.DestroyConversationTableau();
			base.DestroyConversationMission();
			base.CreateConversationMissionIfMissing();
			if (!base.IsConversationActive)
			{
				this.CreateConversationView();
				this.CreateConversationTableau();
			}
			else
			{
				this._minimumAvailableConversationInstallFrame = Utilities.EngineFrameNo + 2;
				this._isSwitchingConversations = true;
			}
			base.IsConversationActive = true;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F31A File Offset: 0x0000D51A
		protected override void FinalizeConversation()
		{
			base.FinalizeConversation();
			this.DestroyConversationTableau();
			this.DestroyConversationView();
			base.DestroyConversationMission();
			this._minimumAvailableConversationInstallFrame = Utilities.EngineFrameNo + 2;
			base.IsConversationActive = false;
			if (!base.MapScreen.IsReady)
			{
				LoadingWindow.EnableGlobalLoadingWindow();
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F35C File Offset: 0x0000D55C
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
			if (base.IsConversationActive)
			{
				this._conversationMovie = this._layerAsGauntletLayer.LoadMovie("MapConversation", this._dataSource);
				if (this._barterView.IsCreated && !this._barterView.IsActive)
				{
					this._barterView.Activate();
				}
				this._conversationCategory.Load();
				this._dataSource.TableauData = this._tableauData;
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000F3E8 File Offset: 0x0000D5E8
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
			if (base.IsConversationActive)
			{
				this._dataSource.TableauData = null;
				this._layerAsGauntletLayer.ReleaseMovie(this._conversationMovie);
				if (this._barterView.IsCreated && this._barterView.IsActive)
				{
					this._barterView.Deactivate();
				}
				this._conversationCategory.Unload();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000F464 File Offset: 0x0000D664
		private void Tick(float dt)
		{
			if (!base.IsConversationActive || this._layerAsGauntletLayer == null)
			{
				return;
			}
			if (this._isSwitchingConversations)
			{
				this._isSwitchingConversations = false;
			}
			if (base.IsConversationActive && ScreenManager.TopScreen == base.MapScreen && ScreenManager.FocusedLayer != base.Layer)
			{
				ScreenManager.TrySetFocus(base.Layer);
			}
			MapConversationVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.Tick(dt);
			}
			MapConversationVM dataSource2 = this._dataSource;
			bool flag;
			if (dataSource2 == null)
			{
				flag = false;
			}
			else
			{
				MissionConversationVM dialogController = dataSource2.DialogController;
				int? num = ((dialogController != null) ? new int?(dialogController.AnswerList.Count) : null);
				int num2 = 0;
				flag = (num.GetValueOrDefault() <= num2) & (num != null);
			}
			if (flag && !this._barterView.IsCreated && base.IsConversationActive && this._layerAsGauntletLayer.Input.IsHotKeyReleased("ContinueKey"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				((IConversationStateHandler)this).ExecuteConversationContinue();
			}
			if (base.IsConversationActive && this._layerAsGauntletLayer != null)
			{
				if (this._barterView.IsCreated)
				{
					this._barterView.TickInput();
				}
				else
				{
					if (base.IsConversationActive && this._tableauData == null && Utilities.EngineFrameNo > this._minimumAvailableConversationInstallFrame)
					{
						this.CreateConversationTableau();
					}
					if (!ScreenFadeController.IsFadeActive && this._layerAsGauntletLayer.Input.IsHotKeyReleased("ToggleEscapeMenu"))
					{
						MapScreen mapScreen = base.MapScreen;
						if (mapScreen != null && mapScreen.IsEscapeMenuOpened)
						{
							base.MapScreen.CloseEscapeMenu();
						}
						else
						{
							MapScreen mapScreen2 = base.MapScreen;
							if (mapScreen2 != null)
							{
								mapScreen2.OpenEscapeMenu();
							}
						}
					}
				}
				BarterItemVM.IsFiveStackModifierActive = this._layerAsGauntletLayer.Input.IsHotKeyDown("FiveStackModifier");
				BarterItemVM.IsEntireStackModifierActive = this._layerAsGauntletLayer.Input.IsHotKeyDown("EntireStackModifier");
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F62D File Offset: 0x0000D82D
		protected override void OnFinalize()
		{
			base.OnFinalize();
			if (base.IsConversationActive)
			{
				this.FinalizeConversation();
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000F644 File Offset: 0x0000D844
		private void CreateConversationView()
		{
			base.Layer = new GauntletLayer("MapConversation", 205, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._barterView = new GauntletMapConversationBarterView(this._layerAsGauntletLayer, new GauntletMapConversationBarterView.OnBarterActiveStateChanged(this.OnBarterActiveStateChanged));
			BarterManager barterManager = this._barterManager;
			barterManager.BarterBegin = (BarterManager.BarterBeginEventDelegate)Delegate.Combine(barterManager.BarterBegin, new BarterManager.BarterBeginEventDelegate(this._barterView.CreateBarterView));
			BarterManager barterManager2 = this._barterManager;
			barterManager2.Closed = (BarterManager.BarterCloseEventDelegate)Delegate.Combine(barterManager2.Closed, new BarterManager.BarterCloseEventDelegate(this._barterView.DestroyBarterView));
			this._dataSource = new MapConversationVM(new Action(this.OnContinue), new Func<string>(GauntletMapConversationView.GetContinueKeyText));
			this._conversationMovie = this._layerAsGauntletLayer.LoadMovie("MapConversation", this._dataSource);
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ConversationHotKeyCategory"));
			base.MapScreen.AddLayer(base.Layer);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			this._conversationCategory.Load();
			Campaign.Current.ConversationManager.Handler = this;
			Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		private void OnContinue()
		{
			if (base.IsConversationActive)
			{
				MapConversationVM dataSource = this._dataSource;
				bool flag;
				if (dataSource == null)
				{
					flag = false;
				}
				else
				{
					MissionConversationVM dialogController = dataSource.DialogController;
					int? num = ((dialogController != null) ? new int?(dialogController.AnswerList.Count) : null);
					int num2 = 0;
					flag = (num.GetValueOrDefault() <= num2) & (num != null);
				}
				if (flag && !this._barterView.IsCreated)
				{
					((IConversationStateHandler)this).ExecuteConversationContinue();
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000F870 File Offset: 0x0000DA70
		private void DestroyConversationView()
		{
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			if (this._barterView.IsCreated)
			{
				this._barterView.DestroyBarterView();
			}
			this._dataSource.OnFinalize();
			base.MapScreen.RemoveLayer(base.Layer);
			SpriteCategory conversationCategory = this._conversationCategory;
			if (conversationCategory != null && conversationCategory.IsLoaded)
			{
				this._conversationCategory.Unload();
			}
			BarterManager barterManager = this._barterManager;
			barterManager.BarterBegin = (BarterManager.BarterBeginEventDelegate)Delegate.Remove(barterManager.BarterBegin, new BarterManager.BarterBeginEventDelegate(this._barterView.CreateBarterView));
			BarterManager barterManager2 = this._barterManager;
			barterManager2.Closed = (BarterManager.BarterCloseEventDelegate)Delegate.Remove(barterManager2.Closed, new BarterManager.BarterCloseEventDelegate(this._barterView.DestroyBarterView));
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			this._dataSource = null;
			Campaign.Current.ConversationManager.Handler = null;
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F974 File Offset: 0x0000DB74
		protected override bool IsEscaped()
		{
			return base.IsConversationActive;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000F97C File Offset: 0x0000DB7C
		protected override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return true;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000F97F File Offset: 0x0000DB7F
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.Tick(dt);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000F98F File Offset: 0x0000DB8F
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			this.Tick(dt);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000F99F File Offset: 0x0000DB9F
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			this.Tick(dt);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		private void CreateConversationTableau()
		{
			float num = CampaignTime.Now.CurrentHourInDay * (float)(24 / CampaignTime.HoursInDay);
			MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(MobileParty.MainParty.Position.ToVec2());
			bool flag = weatherEventInPosition == MapWeatherModel.WeatherEvent.Snowy || weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard;
			string text = null;
			if (this._conversationPartnerData.Character.HeroObject != null)
			{
				LocationComplex locationComplex = LocationComplex.Current;
				string text2;
				if (locationComplex == null)
				{
					text2 = null;
				}
				else
				{
					Location locationOfCharacter = locationComplex.GetLocationOfCharacter(this._conversationPartnerData.Character.HeroObject);
					text2 = ((locationOfCharacter != null) ? locationOfCharacter.StringId : null);
				}
				text = text2;
			}
			this._tableauData = MapConversationTableauData.CreateFrom(this._playerCharacterData, this._conversationPartnerData, Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace), num, flag, Hero.MainHero.CurrentSettlement, text, weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain, weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard);
			this._dataSource.TableauData = this._tableauData;
			this._layerAsGauntletLayer.GamepadNavigationContext.GainNavigationAfterFrames(1, null);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		private void DestroyConversationTableau()
		{
			if (this._dataSource != null)
			{
				this._dataSource.TableauData = null;
			}
			this._tableauData = null;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000FAD1 File Offset: 0x0000DCD1
		void IConversationStateHandler.OnConversationUninstall()
		{
			if (!this._isSwitchingConversations)
			{
				MapState mapState = Game.Current.GameStateManager.LastOrDefault<MapState>();
				if (mapState == null)
				{
					return;
				}
				mapState.OnMapConversationOver();
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000FAF4 File Offset: 0x0000DCF4
		private static string GetContinueKeyText()
		{
			if (Input.IsGamepadActive)
			{
				return GameTexts.FindText("str_click_to_continue_console", null).SetTextVariable("CONSOLE_KEY_NAME", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("ConversationHotKeyCategory", "ContinueClick"), 1f)).ToString();
			}
			return GameTexts.FindText("str_click_to_continue", null).ToString();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000FB4C File Offset: 0x0000DD4C
		void IConversationStateHandler.OnConversationInstall()
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000FB4E File Offset: 0x0000DD4E
		void IConversationStateHandler.OnConversationActivate()
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000FB50 File Offset: 0x0000DD50
		void IConversationStateHandler.OnConversationDeactivate()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000FB57 File Offset: 0x0000DD57
		void IConversationStateHandler.OnConversationContinue()
		{
			this._dataSource.DialogController.OnConversationContinue();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000FB69 File Offset: 0x0000DD69
		void IConversationStateHandler.ExecuteConversationContinue()
		{
			this._dataSource.DialogController.ExecuteContinue();
		}

		// Token: 0x040000DC RID: 220
		private GauntletMovieIdentifier _conversationMovie;

		// Token: 0x040000DD RID: 221
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000DE RID: 222
		private MapConversationVM _dataSource;

		// Token: 0x040000DF RID: 223
		private SpriteCategory _conversationCategory;

		// Token: 0x040000E0 RID: 224
		private MapConversationTableauData _tableauData;

		// Token: 0x040000E1 RID: 225
		private BarterManager _barterManager;

		// Token: 0x040000E2 RID: 226
		private GauntletMapConversationBarterView _barterView;

		// Token: 0x040000E3 RID: 227
		private ConversationCharacterData _playerCharacterData;

		// Token: 0x040000E4 RID: 228
		private ConversationCharacterData _conversationPartnerData;

		// Token: 0x040000E5 RID: 229
		private bool _isSwitchingConversations;

		// Token: 0x040000E6 RID: 230
		private int _minimumAvailableConversationInstallFrame;
	}
}
