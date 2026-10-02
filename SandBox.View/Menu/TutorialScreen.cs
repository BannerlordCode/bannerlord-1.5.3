using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Menu
{
	// Token: 0x0200003E RID: 62
	[GameStateScreen(typeof(TutorialState))]
	public class TutorialScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00013AFE File Offset: 0x00011CFE
		public MenuViewContext MenuViewContext { get; }

		// Token: 0x060001FB RID: 507 RVA: 0x00013B06 File Offset: 0x00011D06
		public TutorialScreen(TutorialState tutorialState)
		{
			this.MenuViewContext = new MenuViewContext(this, tutorialState.MenuContext);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00013B20 File Offset: 0x00011D20
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.MenuViewContext.OnFrameTick(dt);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00013B35 File Offset: 0x00011D35
		protected override void OnActivate()
		{
			base.OnActivate();
			this.MenuViewContext.OnActivate();
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00013B4D File Offset: 0x00011D4D
		protected override void OnDeactivate()
		{
			this.MenuViewContext.OnDeactivate();
			base.OnDeactivate();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00013B60 File Offset: 0x00011D60
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.MenuViewContext.OnInitialize();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00013B73 File Offset: 0x00011D73
		protected override void OnFinalize()
		{
			this.MenuViewContext.OnFinalize();
			base.OnFinalize();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00013B86 File Offset: 0x00011D86
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00013B88 File Offset: 0x00011D88
		void IGameStateListener.OnDeactivate()
		{
			this.MenuViewContext.OnGameStateDeactivate();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00013B95 File Offset: 0x00011D95
		void IGameStateListener.OnInitialize()
		{
			this.MenuViewContext.OnGameStateInitialize();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00013BA2 File Offset: 0x00011DA2
		void IGameStateListener.OnFinalize()
		{
			this.MenuViewContext.OnGameStateFinalize();
		}
	}
}
