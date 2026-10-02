using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Information.RundownTooltip;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Options;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000019 RID: 25
	public class GauntletUISubModule : MBSubModuleBase
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00007E48 File Offset: 0x00006048
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00007E4F File Offset: 0x0000604F
		public static GauntletUISubModule Instance { get; private set; }

		// Token: 0x060000F7 RID: 247 RVA: 0x00007E60 File Offset: 0x00006060
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			CustomWidgetManager.TouchAssembly();
			BannerlordCustomWidgetManager.TouchAssembly();
			this.RefreshResources(true);
			ScreenManager.OnControllerDisconnected += this.OnControllerDisconnected;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DisplayMode);
			if (GauntletGamepadNavigationManager.Instance != null)
			{
				GauntletGamepadNavigationManager.Instance.OnFinalize();
			}
			GauntletGamepadNavigationManager.Initialize();
			GauntletGameVersionView.AddModuleVersionInfo("Bannerlord", Utilities.GetApplicationVersionWithBuildNumber().ToString());
			GauntletUISubModule.Instance = this;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007EF8 File Offset: 0x000060F8
		private void RefreshResources(bool initialLoad)
		{
			Dictionary<GauntletLayer, List<GauntletMovieIdentifier>> dictionary = new Dictionary<GauntletLayer, List<GauntletMovieIdentifier>>();
			if (!initialLoad)
			{
				using (List<ScreenLayer>.Enumerator enumerator = ScreenManager.SortedLayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GauntletLayer gauntletLayer;
						if ((gauntletLayer = enumerator.Current as GauntletLayer) != null)
						{
							List<GauntletMovieIdentifier> list;
							gauntletLayer.OnResourceRefreshBegin(out list);
							dictionary.Add(gauntletLayer, list);
						}
					}
				}
			}
			WidgetInfo.Refresh();
			UIResourceManager.Refresh();
			SpriteCategory fullBackgroundCategory = this._fullBackgroundCategory;
			if (fullBackgroundCategory != null)
			{
				fullBackgroundCategory.Unload();
			}
			SpriteCategory fullscreensCategory = this._fullscreensCategory;
			if (fullscreensCategory != null)
			{
				fullscreensCategory.Unload();
			}
			this._fullBackgroundCategory = UIResourceManager.LoadSpriteCategory("ui_fullbackgrounds");
			this._fullscreensCategory = UIResourceManager.LoadSpriteCategory("ui_fullscreens");
			GauntletGameVersionView.Refresh();
			SpriteCategory[] array = UIResourceManager.SpriteData.SpriteCategories.Values.Where<SpriteCategory>((SpriteCategory x) => x.AlwaysLoad).ToArray<SpriteCategory>();
			float num = 0.2f / (float)(array.Length - 1);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Load();
				if (initialLoad)
				{
					Utilities.SetLoadingScreenPercentage(0.4f + (float)i * num);
				}
			}
			if (initialLoad)
			{
				Utilities.SetLoadingScreenPercentage(0.6f);
			}
			if (!initialLoad)
			{
				using (List<ScreenLayer>.Enumerator enumerator = ScreenManager.SortedLayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GauntletLayer gauntletLayer2;
						if ((gauntletLayer2 = enumerator.Current as GauntletLayer) != null)
						{
							gauntletLayer2.OnResourceRefreshEnd(dictionary[gauntletLayer2]);
						}
					}
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00008090 File Offset: 0x00006290
		private void OnControllerDisconnected()
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008092 File Offset: 0x00006292
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.Language)
			{
				UIResourceManager.OnLanguageChange(BannerlordConfig.Language);
				ScreenManager.UpdateLayout();
				return;
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.UIScale)
			{
				ScreenManager.OnScaleChange(BannerlordConfig.UIScale);
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000080B6 File Offset: 0x000062B6
		protected override void OnNewModuleLoad()
		{
			this._areResourcesDirty = true;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000080C0 File Offset: 0x000062C0
		protected override void OnSubModuleUnloaded()
		{
			ScreenManager.OnControllerDisconnected -= this.OnControllerDisconnected;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			UIResourceManager.Clear();
			if (GauntletGamepadNavigationManager.Instance != null)
			{
				GauntletGamepadNavigationManager.Instance.OnFinalize();
			}
			SpriteCategory fullBackgroundCategory = this._fullBackgroundCategory;
			if (fullBackgroundCategory != null)
			{
				fullBackgroundCategory.Unload();
			}
			SpriteCategory fullscreensCategory = this._fullscreensCategory;
			if (fullscreensCategory != null)
			{
				fullscreensCategory.Unload();
			}
			GauntletGameVersionView.RemoveModuleVersionInfo("Bannerlord");
			GauntletUISubModule.Instance = null;
			GauntletInformationView.OnFinalize();
			base.OnSubModuleUnloaded();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008154 File Offset: 0x00006354
		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			if (!this._initialized)
			{
				if (!Utilities.CommandLineArgumentExists("VisualTests"))
				{
					GauntletInformationView.Initialize();
					GauntletSceneNotification.Initialize();
					GauntletSceneNotification.Current.RegisterContextProvider(new NativeSceneNotificationContextProvider());
					GauntletChatLogView.Initialize();
					GauntletGamepadCursor.Initialize();
					GauntletGameVersionView.Initialize();
					GauntletCameraFadeView.Initialize();
					InformationManager.RegisterTooltip<List<TooltipProperty>, PropertyBasedTooltipVM>(new Action<PropertyBasedTooltipVM, object[]>(PropertyBasedTooltipVM.RefreshGenericPropertyBasedTooltip), "PropertyBasedTooltip");
					InformationManager.RegisterTooltip<RundownLineVM, RundownTooltipVM>(new Action<RundownTooltipVM, object[]>(RundownTooltipVM.RefreshGenericRundownTooltip), "RundownTooltip");
					InformationManager.RegisterTooltip<string, HintVM>(new Action<HintVM, object[]>(HintVM.RefreshGenericHintTooltip), "HintTooltip");
					this._queryManager = new GauntletQueryManager();
					this._queryManager.Initialize();
					this._queryManager.InitializeKeyVisuals();
				}
				UIResourceManager.OnLanguageChange(BannerlordConfig.Language);
				ScreenManager.OnScaleChange(BannerlordConfig.UIScale);
				this._initialized = true;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008226 File Offset: 0x00006426
		public override void OnMultiplayerGameStart(Game game, object starterObject)
		{
			base.OnMultiplayerGameStart(game, starterObject);
			if (!this._isMultiplayer)
			{
				ILoadingWindowManager loadingWindowManager = LoadingWindow.LoadingWindowManager;
				if (loadingWindowManager != null)
				{
					loadingWindowManager.SetCurrentModeIsMultiplayer(true);
				}
				this._isMultiplayer = true;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008250 File Offset: 0x00006450
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			if (this._isMultiplayer)
			{
				ILoadingWindowManager loadingWindowManager = LoadingWindow.LoadingWindowManager;
				if (loadingWindowManager != null)
				{
					loadingWindowManager.SetCurrentModeIsMultiplayer(false);
				}
				this._isMultiplayer = false;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000827C File Offset: 0x0000647C
		protected override void OnApplicationTick(float dt)
		{
			if (this._areResourcesDirty)
			{
				this.RefreshResources(false);
				this._areResourcesDirty = false;
			}
			base.OnApplicationTick(dt);
			if (!this._loadingWindowCreated)
			{
				if (LoadingWindow.LoadingWindowManager == null)
				{
					LoadingWindow.InitializeWith<GauntletDefaultLoadingWindowManager>();
				}
				this._loadingWindowCreated = true;
			}
			UIResourceManager.Update();
			if (GauntletGamepadNavigationManager.Instance != null && ScreenManager.GetMouseVisibility())
			{
				GauntletGamepadNavigationManager.Instance.IsTouchpadMouseEnabled = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableTouchpadMouse) != 0f;
				GauntletGamepadNavigationManager.Instance.Update(dt);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000082FA File Offset: 0x000064FA
		[CommandLineFunctionality.CommandLineArgumentFunction("clear", "chatlog")]
		public static string ClearChatLog(List<string> strings)
		{
			InformationManager.ClearAllMessages();
			return "Chatlog cleared!";
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008308 File Offset: 0x00006508
		[CommandLineFunctionality.CommandLineArgumentFunction("can_focus_while_in_mission", "chatlog")]
		public static string SetCanFocusWhileInMission(List<string> strings)
		{
			if (strings[0] == "0" || strings[0] == "1")
			{
				GauntletChatLogView.Current.SetCanFocusWhileInMission(strings[0] == "1");
				return "Chat window will" + ((strings[0] == "1") ? " " : " NOT ") + " be able to gain focus now.";
			}
			return "Wrong input";
		}

		// Token: 0x0400009B RID: 155
		private bool _initialized;

		// Token: 0x0400009C RID: 156
		private bool _isMultiplayer;

		// Token: 0x0400009D RID: 157
		private GauntletQueryManager _queryManager;

		// Token: 0x0400009E RID: 158
		private SpriteCategory _fullBackgroundCategory;

		// Token: 0x0400009F RID: 159
		private SpriteCategory _fullscreensCategory;

		// Token: 0x040000A0 RID: 160
		private bool _areResourcesDirty;

		// Token: 0x040000A1 RID: 161
		private bool _loadingWindowCreated;
	}
}
