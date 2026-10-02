using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.InputSystem;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GameKeyCategory;
using TaleWorlds.MountAndBlade.View.CustomBattle;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.MountAndBlade.View.VisualOrders;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000028 RID: 40
	public class ViewSubModule : MBSubModuleBase
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000081BC File Offset: 0x000063BC
		// (set) Token: 0x0600011A RID: 282 RVA: 0x000081C8 File Offset: 0x000063C8
		public static Dictionary<Tuple<Material, Banner>, Material> BannerTexturedMaterialCache
		{
			get
			{
				return ViewSubModule._instance._bannerTexturedMaterialCache;
			}
			set
			{
				ViewSubModule._instance._bannerTexturedMaterialCache = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000081D5 File Offset: 0x000063D5
		public static GameStateScreenManager GameStateScreenManager
		{
			get
			{
				return ViewSubModule._instance._gameStateScreenManager;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000081E4 File Offset: 0x000063E4
		private void InitializeHotKeyManager()
		{
			string text = "BannerlordGameKeys.xml";
			HotKeyManager.Initialize(new PlatformFilePath(EngineFilePaths.ConfigsPath, text), !ScreenManager.IsEnterButtonRDown);
			HotKeyManager.RegisterInitialContexts(new List<GameKeyContext>
			{
				new GenericGameKeyContext(),
				new GenericCampaignPanelsGameKeyCategory("GenericCampaignPanelsGameKeyCategory"),
				new GenericPanelGameKeyCategory("GenericPanelGameKeyCategory"),
				new ArmyManagementHotkeyCategory(),
				new BoardGameHotkeyCategory(),
				new ChatLogHotKeyCategory(),
				new CombatHotKeyCategory(),
				new CraftingHotkeyCategory(),
				new FaceGenHotkeyCategory(),
				new InventoryHotKeyCategory(),
				new PartyHotKeyCategory(),
				new MapHotKeyCategory(),
				new MapNotificationHotKeyCategory(),
				new MissionOrderHotkeyCategory(),
				new OrderOfBattleHotKeyCategory(),
				new MultiplayerHotkeyCategory(),
				new ScoreboardHotKeyCategory(),
				new ConversationHotKeyCategory(),
				new CheatsHotKeyCategory(),
				new PhotoModeHotKeyCategory(),
				new PollHotkeyCategory()
			});
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000830A File Offset: 0x0000650A
		private void InitializeBannerVisualManager()
		{
			if (BannerManager.Instance == null)
			{
				BannerManager.Initialize();
				BannerManager.Instance.LoadBannerIcons();
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008324 File Offset: 0x00006524
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			ViewSubModule._instance = this;
			this.InitializeHotKeyManager();
			this.InitializeBannerVisualManager();
			CraftedDataViewManager.Initialize();
			this._visualOrderProvider = new DefaultVisualOrderProvider();
			VisualOrderFactory.RegisterProvider(this._visualOrderProvider);
			this._gameStateScreenManager = new GameStateScreenManager();
			Module.CurrentModule.GlobalGameStateManager.RegisterListener(this._gameStateScreenManager);
			MBMusicManager.Create();
			TextObject coreContentDisabledReason = new TextObject("{=V8BXjyYq}Disabled during installation.", null);
			if (Utilities.EditModeEnabled)
			{
				Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Editor", new TextObject("{=bUh0x6rA}Editor", null), -1, delegate
				{
					MBInitialScreenBase.OnEditModeEnterPress();
				}, () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason), null, null));
			}
			Module.CurrentModule.AddInitialStateOption(new InitialStateOption("CustomBattle", new TextObject("{=4gOGGbeQ}Custom Battle", null), 5000, new Action(CustomBattleFactory.StartCustomBattle), () => new ValueTuple<bool, TextObject>(false, null), null, () => CustomBattleFactory.GetProviderCount() == 0));
			Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Options", new TextObject("{=NqarFr4P}Options", null), 9998, delegate
			{
				ScreenManager.PushScreen(ViewCreator.CreateOptionsScreen(true));
			}, () => new ValueTuple<bool, TextObject>(false, null), null, null));
			Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Credits", new TextObject("{=ODQmOrIw}Credits", null), 9999, delegate
			{
				ScreenManager.PushScreen(ViewCreator.CreateCreditsScreen());
			}, () => new ValueTuple<bool, TextObject>(false, null), null, null));
			Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Exit", new TextObject("{=YbpzLHzk}Exit Game", null), 10000, delegate
			{
				MBInitialScreenBase.DoExitButtonAction();
			}, () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason), null, null));
			ViewModel.RefreshPropertyAndMethodInfos();
			Module.CurrentModule.ImguiProfilerTick += this.OnImguiProfilerTick;
			ScreenManager.OnPushScreen += this.OnScreenManagerPushScreen;
			EngineController.OnConstrainedStateChanged += this.OnConstrainedStateChange;
			HyperlinkTexts.IsPlayStationGamepadActive = new Func<bool>(this.GetIsPlaystationGamepadActive);
			this._dlcInstallationQueryView = new DLCInstallationQueryView();
			this._dlcInstallationQueryView.Initialize();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000085E1 File Offset: 0x000067E1
		private void OnModuleStructureChanged()
		{
			ViewModel.RefreshPropertyAndMethodInfos();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000085E8 File Offset: 0x000067E8
		private void OnConstrainedStateChange(bool isConstrained)
		{
			ScreenManager.OnConstrainStateChanged(isConstrained);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000085F0 File Offset: 0x000067F0
		private bool GetIsPlaystationGamepadActive()
		{
			bool flag = Input.ControllerType.IsPlaystation();
			return Input.IsGamepadActive && flag;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008610 File Offset: 0x00006810
		protected override void OnSubModuleUnloaded()
		{
			DLCInstallationQueryView dlcInstallationQueryView = this._dlcInstallationQueryView;
			if (dlcInstallationQueryView != null)
			{
				dlcInstallationQueryView.OnFinalize();
			}
			this._dlcInstallationQueryView = null;
			VisualOrderFactory.UnregisterProvider(this._visualOrderProvider);
			ThumbnailCacheManager.ClearManager();
			BannerlordTableauManager.ClearManager();
			CraftedDataViewManager.Clear();
			Module.CurrentModule.ImguiProfilerTick -= this.OnImguiProfilerTick;
			ScreenManager.OnPushScreen -= this.OnScreenManagerPushScreen;
			EngineController.OnConstrainedStateChanged -= this.OnConstrainedStateChange;
			ViewSubModule._instance = null;
			base.OnSubModuleUnloaded();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008694 File Offset: 0x00006894
		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			if (this._initialized)
			{
				BannerPersistentTextureCache bannerPersistentTextureCache = BannerPersistentTextureCache.Current;
				if (bannerPersistentTextureCache != null)
				{
					bannerPersistentTextureCache.FlushCache();
				}
			}
			if (!this._initialized)
			{
				BannerlordTableauManager.InitializeCharacterTableauRenderSystem();
				ThumbnailCacheManager.InitializeManager();
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new AvatarThumbnailCache(75));
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new BannerThumbnailCache(100));
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new BannerPersistentTextureCache());
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new BannerEditorTextureCache(5));
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new CharacterThumbnailCache(75));
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new CraftingPieceThumbnailCache(75));
				ThumbnailCacheManager.Current.RegisterThumbnailCache(new ItemThumbnailCache(75));
				this._initialized = true;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008749 File Offset: 0x00006949
		protected override void OnNewModuleLoad()
		{
			ViewCreatorManager.CollectTypes();
			ViewModel.RefreshPropertyAndMethodInfos();
			this._gameStateScreenManager.CollectTypes();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008760 File Offset: 0x00006960
		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			if (Input.DebugInput.IsHotKeyPressed("ToggleUI"))
			{
				MBDebug.DisableUI(new List<string>());
			}
			HotKeyManager.Tick(dt);
			MBMusicManager mbmusicManager = MBMusicManager.Current;
			if (mbmusicManager != null)
			{
				mbmusicManager.Update(dt);
			}
			ThumbnailCacheManager thumbnailCacheManager = ThumbnailCacheManager.Current;
			if (thumbnailCacheManager == null)
			{
				return;
			}
			thumbnailCacheManager.Tick(dt);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000087B7 File Offset: 0x000069B7
		protected override void AfterAsyncTickTick(float dt)
		{
			MBMusicManager mbmusicManager = MBMusicManager.Current;
			if (mbmusicManager == null)
			{
				return;
			}
			mbmusicManager.Update(dt);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000087C9 File Offset: 0x000069C9
		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			MissionWeapon.OnGetWeaponDataHandler = new MissionWeapon.OnGetWeaponDataDelegate(ItemCollectionElementViewExtensions.OnGetWeaponData);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000087DC File Offset: 0x000069DC
		public override void OnCampaignStart(Game game, object starterObject)
		{
			Game.Current.GameStateManager.RegisterListener(this._gameStateScreenManager);
			this._newGameInitialization = false;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000087FB File Offset: 0x000069FB
		public override void OnMultiplayerGameStart(Game game, object starterObject)
		{
			Game.Current.GameStateManager.RegisterListener(this._gameStateScreenManager);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008813 File Offset: 0x00006A13
		public override void OnGameLoaded(Game game, object initializerObject)
		{
			Game.Current.GameStateManager.RegisterListener(this._gameStateScreenManager);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000882C File Offset: 0x00006A2C
		public override void OnGameInitializationFinished(Game game)
		{
			base.OnGameInitializationFinished(game);
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.MultiMeshName != "")
				{
					MBUnusedResourceManager.SetMeshUsed(itemObject.MultiMeshName);
				}
				HorseComponent horseComponent = itemObject.HorseComponent;
				if (horseComponent != null)
				{
					foreach (KeyValuePair<string, bool> keyValuePair in horseComponent.AdditionalMeshesNameList)
					{
						MBUnusedResourceManager.SetMeshUsed(keyValuePair.Key);
					}
				}
				if (itemObject.PrimaryWeapon != null)
				{
					MBUnusedResourceManager.SetMeshUsed(itemObject.HolsterMeshName);
					MBUnusedResourceManager.SetMeshUsed(itemObject.HolsterWithWeaponMeshName);
					MBUnusedResourceManager.SetMeshUsed(itemObject.FlyingMeshName);
					MBUnusedResourceManager.SetBodyUsed(itemObject.BodyName);
					MBUnusedResourceManager.SetBodyUsed(itemObject.HolsterBodyName);
					MBUnusedResourceManager.SetBodyUsed(itemObject.CollisionBodyName);
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008948 File Offset: 0x00006B48
		public override void BeginGameStart(Game game)
		{
			base.BeginGameStart(game);
			Game.Current.BannerVisualCreator = new BannerVisualCreator();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008960 File Offset: 0x00006B60
		public override bool DoLoading(Game game)
		{
			if (this._newGameInitialization)
			{
				return true;
			}
			this._newGameInitialization = true;
			return this._newGameInitialization;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008979 File Offset: 0x00006B79
		public override void OnGameEnd(Game game)
		{
			MissionWeapon.OnGetWeaponDataHandler = null;
			CraftedDataViewManager.Clear();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008986 File Offset: 0x00006B86
		private void OnImguiProfilerTick()
		{
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008988 File Offset: 0x00006B88
		private void OnScreenManagerPushScreen(ScreenBase pushedScreen)
		{
		}

		// Token: 0x0400004D RID: 77
		private Dictionary<Tuple<Material, Banner>, Material> _bannerTexturedMaterialCache;

		// Token: 0x0400004E RID: 78
		private GameStateScreenManager _gameStateScreenManager;

		// Token: 0x0400004F RID: 79
		private bool _newGameInitialization;

		// Token: 0x04000050 RID: 80
		private VisualOrderProvider _visualOrderProvider;

		// Token: 0x04000051 RID: 81
		private static ViewSubModule _instance;

		// Token: 0x04000052 RID: 82
		private bool _initialized;

		// Token: 0x04000053 RID: 83
		private DLCInstallationQueryView _dlcInstallationQueryView;
	}
}
