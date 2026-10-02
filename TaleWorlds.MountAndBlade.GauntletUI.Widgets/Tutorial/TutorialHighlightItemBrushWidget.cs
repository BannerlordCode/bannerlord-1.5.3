using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004B RID: 75
	public class TutorialHighlightItemBrushWidget : BrushWidget
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000D10F File Offset: 0x0000B30F
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0000D117 File Offset: 0x0000B317
		public Widget CustomSizeSyncTarget { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000D120 File Offset: 0x0000B320
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x0000D128 File Offset: 0x0000B328
		public bool DoNotOverrideWidth { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000D131 File Offset: 0x0000B331
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0000D139 File Offset: 0x0000B339
		public bool DoNotOverrideHeight { get; set; }

		// Token: 0x06000429 RID: 1065 RVA: 0x0000D142 File Offset: 0x0000B342
		public TutorialHighlightItemBrushWidget(UIContext context)
			: base(context)
		{
			base.UseGlobalTimeForAnimation = true;
			base.DoNotAcceptEvents = true;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000D15C File Offset: 0x0000B35C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animState == TutorialHighlightItemBrushWidget.AnimState.Start)
			{
				this._animState = TutorialHighlightItemBrushWidget.AnimState.FirstFrame;
			}
			else if (this._animState == TutorialHighlightItemBrushWidget.AnimState.FirstFrame)
			{
				if (base.BrushRenderer.Brush == null)
				{
					this._animState = TutorialHighlightItemBrushWidget.AnimState.Start;
				}
				else
				{
					this._animState = TutorialHighlightItemBrushWidget.AnimState.Playing;
					base.BrushRenderer.RestartAnimation();
				}
			}
			if (this.IsHighlightEnabled && this._isDisabled)
			{
				this._isDisabled = false;
				this.SetState("Default");
			}
			else if (!this.IsHighlightEnabled && !this._isDisabled)
			{
				this.SetState("Disabled");
				this._isDisabled = true;
			}
			this.UpdateTargetSize();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000D200 File Offset: 0x0000B400
		private void UpdateTargetSize()
		{
			Widget widget = this.CustomSizeSyncTarget ?? base.ParentWidget;
			if (widget == null)
			{
				return;
			}
			bool flag;
			if (widget.HeightSizePolicy == SizePolicy.CoverChildren || widget.WidthSizePolicy == SizePolicy.CoverChildren)
			{
				if (!this.DoNotOverrideWidth)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
				}
				if (!this.DoNotOverrideHeight)
				{
					base.HeightSizePolicy = SizePolicy.Fixed;
				}
				flag = true;
			}
			else
			{
				base.WidthSizePolicy = SizePolicy.StretchToParent;
				base.HeightSizePolicy = SizePolicy.StretchToParent;
				flag = false;
			}
			if (flag && widget.Size.X > 1f && widget.Size.Y > 1f)
			{
				if (!this.DoNotOverrideWidth)
				{
					base.ScaledSuggestedWidth = widget.Size.X - 1f;
				}
				if (!this.DoNotOverrideHeight)
				{
					base.ScaledSuggestedHeight = widget.Size.Y - 1f;
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000D2CB File Offset: 0x0000B4CB
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x0000D2D4 File Offset: 0x0000B4D4
		[Editor(false)]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (this._isHighlightEnabled != value)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChanged(value, "IsHighlightEnabled");
					if (this.IsHighlightEnabled)
					{
						this._animState = TutorialHighlightItemBrushWidget.AnimState.Start;
					}
					base.IsVisible = value;
					TaleWorlds.GauntletUI.EventManager.UIEventManager.TriggerEvent<TutorialHighlightItemBrushWidget.HighlightElementToggledEvent>(new TutorialHighlightItemBrushWidget.HighlightElementToggledEvent(value, value ? this : null));
				}
			}
		}

		// Token: 0x040001BB RID: 443
		private TutorialHighlightItemBrushWidget.AnimState _animState;

		// Token: 0x040001BC RID: 444
		private bool _isDisabled;

		// Token: 0x040001BD RID: 445
		private bool _isHighlightEnabled;

		// Token: 0x020001A7 RID: 423
		public enum AnimState
		{
			// Token: 0x040009E9 RID: 2537
			Idle,
			// Token: 0x040009EA RID: 2538
			Start,
			// Token: 0x040009EB RID: 2539
			FirstFrame,
			// Token: 0x040009EC RID: 2540
			Playing
		}

		// Token: 0x020001A8 RID: 424
		public class HighlightElementToggledEvent : EventBase
		{
			// Token: 0x1700077D RID: 1917
			// (get) Token: 0x06001549 RID: 5449 RVA: 0x0003A5E2 File Offset: 0x000387E2
			// (set) Token: 0x0600154A RID: 5450 RVA: 0x0003A5EA File Offset: 0x000387EA
			public bool IsEnabled { get; private set; }

			// Token: 0x1700077E RID: 1918
			// (get) Token: 0x0600154B RID: 5451 RVA: 0x0003A5F3 File Offset: 0x000387F3
			// (set) Token: 0x0600154C RID: 5452 RVA: 0x0003A5FB File Offset: 0x000387FB
			public TutorialHighlightItemBrushWidget HighlightFrameWidget { get; private set; }

			// Token: 0x0600154D RID: 5453 RVA: 0x0003A604 File Offset: 0x00038804
			public HighlightElementToggledEvent(bool isEnabled, TutorialHighlightItemBrushWidget highlightFrameWidget)
			{
				this.IsEnabled = isEnabled;
				this.HighlightFrameWidget = highlightFrameWidget;
			}
		}
	}
}
