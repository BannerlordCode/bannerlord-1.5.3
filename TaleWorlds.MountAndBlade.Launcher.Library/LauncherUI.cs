using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets;
using TaleWorlds.MountAndBlade.Launcher.Library.UserDatas;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000007 RID: 7
	public class LauncherUI
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000032 RID: 50 RVA: 0x00002614 File Offset: 0x00000814
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x00002648 File Offset: 0x00000848
		public static event Action<string> OnAddHintInformation;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000034 RID: 52 RVA: 0x0000267C File Offset: 0x0000087C
		// (remove) Token: 0x06000035 RID: 53 RVA: 0x000026B0 File Offset: 0x000008B0
		public static event Action OnHideHintInformation;

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000026E3 File Offset: 0x000008E3
		public bool HasUnofficialModulesSelected
		{
			get
			{
				return this._viewModel.ModsData.Modules.Any<LauncherModuleVM>((LauncherModuleVM m) => !m.IsOfficial && m.IsSelected);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000271C File Offset: 0x0000091C
		public LauncherUI(UserDataManager userDataManager, UIContext context, Action onClose, Action onMinimize)
		{
			this._context = context;
			this._twoDimensionContext = this._context.TwoDimensionContext;
			this._userDataManager = userDataManager;
			this._onClose = onClose;
			this._onMinimize = onMinimize;
			this._stopwatch = new Stopwatch();
			this._stopwatch.Start();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002774 File Offset: 0x00000974
		public void Initialize()
		{
			this._spriteData = this._context.SpriteData;
			this._spriteData.SpriteCategories["ui_launcher"].Load(this._twoDimensionContext.ResourceContext, this._twoDimensionContext.ResourceDepot);
			this._spriteData.SpriteCategories["ui_fonts_launcher"].Load(this._twoDimensionContext.ResourceContext, this._twoDimensionContext.ResourceDepot);
			this._material = new PrimitivePolygonMaterial(new Color(0.5f, 0.5f, 0.5f, 1f));
			this._widgetFactory = new WidgetFactory(this._context.ResourceDepot, "Prefabs");
			this._widgetFactory.PrefabExtensionContext.AddExtension(new PrefabDatabindingExtension());
			this._widgetFactory.Initialize(null);
			this._viewModel = new LauncherVM(this._userDataManager, this._onClose, this._onMinimize);
			this._movie = GauntletMovie.Load(this._context, this._widgetFactory, "UILauncher", this._viewModel, false, true);
			GauntletGamepadNavigationContext gauntletGamepadNavigationContext = new GauntletGamepadNavigationContext(new Func<Vector2, bool>(this.GetIsBlockedOnPosition), new Func<int>(this.GetLastScreenOrder), new Func<bool>(this.GetIsAvailableForGamepadNavigation));
			this._context.InitializeGamepadNavigation(gauntletGamepadNavigationContext);
			this._context.EventManager.OnGetIsHitThisFrame = new Func<bool>(this.GetIsHitThisFrame);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028E5 File Offset: 0x00000AE5
		public void OnFinalize()
		{
			this._context.EventManager.OnGetIsHitThisFrame = null;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028F8 File Offset: 0x00000AF8
		private int GetLastScreenOrder()
		{
			return 0;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000028FB File Offset: 0x00000AFB
		private bool GetIsHitThisFrame()
		{
			return true;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000028FE File Offset: 0x00000AFE
		private bool GetIsBlockedOnPosition(Vector2 pos)
		{
			return false;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002901 File Offset: 0x00000B01
		private bool GetIsAvailableForGamepadNavigation()
		{
			return false;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002904 File Offset: 0x00000B04
		public string AdditionalArgs
		{
			get
			{
				if (this._viewModel == null)
				{
					return "";
				}
				return this._viewModel.GameTypeArgument + " " + this._viewModel.ModsData.ModuleListCode + this._viewModel.ContinueGameArgument;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002944 File Offset: 0x00000B44
		public void Update()
		{
			this._movie.Update();
			if (this._stopwatch.IsRunning)
			{
				this._stopwatch.Stop();
				Debug.Print("Total startup time: " + ((float)this._stopwatch.ElapsedMilliseconds / 1000f).ToString("0.0000"), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000029AE File Offset: 0x00000BAE
		public bool CheckMouseOverWindowDragArea()
		{
			return this._context.EventManager.HoveredWidget is LauncherDragWindowAreaWidget;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000029C8 File Offset: 0x00000BC8
		public bool HitTest()
		{
			return this._movie != null && this._context.HitTest(this._movie.RootWidget);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000029EA File Offset: 0x00000BEA
		public static void AddHintInformation(string message)
		{
			Action<string> onAddHintInformation = LauncherUI.OnAddHintInformation;
			if (onAddHintInformation == null)
			{
				return;
			}
			onAddHintInformation(message);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000029FC File Offset: 0x00000BFC
		public static void HideHintInformation()
		{
			Action onHideHintInformation = LauncherUI.OnHideHintInformation;
			if (onHideHintInformation == null)
			{
				return;
			}
			onHideHintInformation();
		}

		// Token: 0x04000016 RID: 22
		private Material _material;

		// Token: 0x04000017 RID: 23
		private TwoDimensionContext _twoDimensionContext;

		// Token: 0x04000018 RID: 24
		private UIContext _context;

		// Token: 0x04000019 RID: 25
		private IGauntletMovie _movie;

		// Token: 0x0400001A RID: 26
		private LauncherVM _viewModel;

		// Token: 0x0400001B RID: 27
		private SpriteData _spriteData;

		// Token: 0x0400001C RID: 28
		private WidgetFactory _widgetFactory;

		// Token: 0x0400001D RID: 29
		private UserDataManager _userDataManager;

		// Token: 0x0400001E RID: 30
		private readonly Action _onClose;

		// Token: 0x0400001F RID: 31
		private readonly Action _onMinimize;

		// Token: 0x04000020 RID: 32
		private Stopwatch _stopwatch;
	}
}
