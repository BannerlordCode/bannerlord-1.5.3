using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004F RID: 79
	public class TutorialPanelImageWidget : ImageWidget
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0000DF28 File Offset: 0x0000C128
		public TutorialPanelImageWidget(UIContext context)
			: base(context)
		{
			base.UseGlobalTimeForAnimation = true;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000DF38 File Offset: 0x0000C138
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animState == TutorialPanelImageWidget.AnimState.Start)
			{
				this._tickCount++;
				if (this._tickCount > 20)
				{
					this._animState = TutorialPanelImageWidget.AnimState.Starting;
					return;
				}
			}
			else if (this._animState == TutorialPanelImageWidget.AnimState.Starting)
			{
				BrushListPanel tutorialPanel = this.TutorialPanel;
				if (tutorialPanel != null)
				{
					tutorialPanel.BrushRenderer.RestartAnimation();
				}
				this._animState = TutorialPanelImageWidget.AnimState.Playing;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000DF9B File Offset: 0x0000C19B
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this.Initialize();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		private void Initialize()
		{
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				this._animState = TutorialPanelImageWidget.AnimState.Idle;
				this._tickCount = 0;
			}
			else if (this._animState != TutorialPanelImageWidget.AnimState.Start)
			{
				this.SetState("Default");
				this._animState = TutorialPanelImageWidget.AnimState.Start;
				base.Context.TwoDimensionContext.PlaySound("panels/tutorial");
			}
			base.IsVisible = base.IsEnabled;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000E018 File Offset: 0x0000C218
		protected override void RefreshState()
		{
			base.RefreshState();
			this.Initialize();
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000E026 File Offset: 0x0000C226
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0000E02E File Offset: 0x0000C22E
		[Editor(false)]
		public BrushListPanel TutorialPanel
		{
			get
			{
				return this._tutorialPanel;
			}
			set
			{
				if (this._tutorialPanel != value)
				{
					this._tutorialPanel = value;
					base.OnPropertyChanged<BrushListPanel>(value, "TutorialPanel");
					if (this._tutorialPanel != null)
					{
						this._tutorialPanel.UseGlobalTimeForAnimation = true;
					}
				}
			}
		}

		// Token: 0x040001D4 RID: 468
		private TutorialPanelImageWidget.AnimState _animState;

		// Token: 0x040001D5 RID: 469
		private int _tickCount;

		// Token: 0x040001D6 RID: 470
		private BrushListPanel _tutorialPanel;

		// Token: 0x020001B1 RID: 433
		public enum AnimState
		{
			// Token: 0x04000A09 RID: 2569
			Idle,
			// Token: 0x04000A0A RID: 2570
			Start,
			// Token: 0x04000A0B RID: 2571
			Starting,
			// Token: 0x04000A0C RID: 2572
			Playing
		}
	}
}
