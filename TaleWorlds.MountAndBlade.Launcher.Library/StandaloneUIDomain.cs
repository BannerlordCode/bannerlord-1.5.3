using System;
using System.Threading;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Launcher.Library.UserDatas;
using TaleWorlds.TwoDimension;
using TaleWorlds.TwoDimension.Standalone;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x0200000A RID: 10
	public class StandaloneUIDomain : FrameworkDomain
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002E78 File Offset: 0x00001078
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002E80 File Offset: 0x00001080
		public UserDataManager UserDataManager { get; private set; }

		// Token: 0x06000053 RID: 83 RVA: 0x00002E89 File Offset: 0x00001089
		public StandaloneUIDomain(GraphicsForm graphicsForm, ResourceDepot resourceDepot)
		{
			this._graphicsForm = graphicsForm;
			this._resourceDepot = resourceDepot;
			this.UserDataManager = new UserDataManager();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002EAA File Offset: 0x000010AA
		public override void Update()
		{
			this.UpdateAux();
			this.DestroyIfNeeded();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002EB8 File Offset: 0x000010B8
		private void UpdateAux()
		{
			if (this._synchronizationContext == null)
			{
				this._synchronizationContext = new SingleThreadedSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(this._synchronizationContext);
			}
			if (!this._initialized)
			{
				WidgetInfo.Refresh();
				GauntletGamepadNavigationManager.Initialize();
				this.UserDataManager.LoadUserData();
				Input.Initialize(new StandaloneInputManager(this._graphicsForm), null);
				this._graphicsForm.InitializeGraphicsContext(this._resourceDepot);
				this._graphicsContext = this._graphicsForm.GraphicsContext;
				TwoDimensionPlatform twoDimensionPlatform = new TwoDimensionPlatform(this._graphicsForm, true);
				this._twoDimensionContext = new TwoDimensionContext(twoDimensionPlatform, twoDimensionPlatform, this._resourceDepot);
				InputContext inputContext = new InputContext();
				inputContext.MouseOnMe = true;
				inputContext.IsKeysAllowed = true;
				inputContext.IsMouseButtonAllowed = true;
				inputContext.IsMouseWheelAllowed = true;
				this._gauntletUIContext = new UIContext(this._twoDimensionContext, inputContext);
				this._gauntletUIContext.IsDynamicScaleEnabled = false;
				this._gauntletUIContext.Initialize();
				this._launcherUI = new LauncherUI(this.UserDataManager, this._gauntletUIContext, new Action(this.OnCloseRequest), new Action(this.OnMinimizeRequest));
				this._launcherUI.Initialize();
				this._initialized = true;
				this._graphicsForm.BeginFrame();
				return;
			}
			if (Program.IsShuttingDown)
			{
				return;
			}
			if (this._graphicsForm.IsMinimized)
			{
				Thread.Sleep(this._graphicsContext.MaxTimeToRenderOneFrame);
				return;
			}
			try
			{
				this._resourceDepot.CheckForChanges();
				this._synchronizationContext.Tick();
				bool flag = this._launcherUI.CheckMouseOverWindowDragArea();
				this._graphicsForm.UpdateInput(flag);
				this._graphicsForm.BeginFrame();
				Input.Update();
				this._graphicsForm.Update();
				this._gauntletUIContext.UpdateInput(InputType.MouseButton | InputType.MouseWheel | InputType.Key);
				this._gauntletUIContext.Update(0.016666668f);
				this._launcherUI.Update();
				if (!Program.IsShuttingDown)
				{
					this._gauntletUIContext.LateUpdate(0.016666668f);
					this._gauntletUIContext.RenderTick(0.016666668f);
					if (!Program.IsShuttingDown)
					{
						this._graphicsForm.PostRender();
						this._graphicsContext.SwapBuffers();
						this._consecutiveRenderExceptions = 0;
					}
				}
			}
			catch (Exception ex)
			{
				if (!Program.IsShuttingDown)
				{
					this._consecutiveRenderExceptions++;
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "RenderException", ex.Message);
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "RenderExceptionStack", ex.StackTrace ?? "");
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "RenderExceptionCount", this._consecutiveRenderExceptions.ToString());
					if (this._consecutiveRenderExceptions >= 180)
					{
						Watchdog.LogProperty("crash_tags.txt", "Runtime", "FatalRenderFailure", "Too many consecutive render exceptions");
						Environment.Exit(1);
					}
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003194 File Offset: 0x00001394
		private void DestroyIfNeeded()
		{
			if (this._shouldDestroy)
			{
				this.DestroyAux();
				this._shouldDestroy = false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000031AB File Offset: 0x000013AB
		public string AdditionalArgs
		{
			get
			{
				if (this._launcherUI == null)
				{
					return "";
				}
				return this._launcherUI.AdditionalArgs;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000031C6 File Offset: 0x000013C6
		public bool HasUnofficialModulesSelected
		{
			get
			{
				return this._launcherUI.HasUnofficialModulesSelected;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031D3 File Offset: 0x000013D3
		public override void Destroy()
		{
			this._shouldDestroy = true;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000031DC File Offset: 0x000013DC
		private void DestroyAux()
		{
			GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
			if (instance != null)
			{
				instance.OnFinalize();
			}
			this._synchronizationContext = null;
			this._initialized = false;
			this._launcherUI.OnFinalize();
			this._launcherUI = null;
			this._gauntletUIContext = null;
			GraphicsForm graphicsForm = this._graphicsForm;
			if (graphicsForm != null)
			{
				graphicsForm.Destroy();
			}
			DirectXGraphicsContext graphicsContext = this._graphicsContext;
			if (graphicsContext == null)
			{
				return;
			}
			graphicsContext.DestroyContext();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003241 File Offset: 0x00001441
		private void OnStartGameRequest()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003243 File Offset: 0x00001443
		private void OnCloseRequest()
		{
			Environment.Exit(0);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000324B File Offset: 0x0000144B
		private void OnMinimizeRequest()
		{
			this._graphicsForm.MinimizeWindow();
		}

		// Token: 0x0400002D RID: 45
		private SingleThreadedSynchronizationContext _synchronizationContext;

		// Token: 0x0400002E RID: 46
		private bool _initialized;

		// Token: 0x0400002F RID: 47
		private bool _shouldDestroy;

		// Token: 0x04000030 RID: 48
		private GraphicsForm _graphicsForm;

		// Token: 0x04000031 RID: 49
		private DirectXGraphicsContext _graphicsContext;

		// Token: 0x04000032 RID: 50
		private const int RenderExceptionFatalThreshold = 180;

		// Token: 0x04000033 RID: 51
		private int _consecutiveRenderExceptions;

		// Token: 0x04000034 RID: 52
		private UIContext _gauntletUIContext;

		// Token: 0x04000035 RID: 53
		private TwoDimensionContext _twoDimensionContext;

		// Token: 0x04000036 RID: 54
		private LauncherUI _launcherUI;

		// Token: 0x04000037 RID: 55
		private readonly ResourceDepot _resourceDepot;
	}
}
