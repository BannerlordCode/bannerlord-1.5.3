using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x02000050 RID: 80
	public class TutorialScreenWidget : Widget
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0000E060 File Offset: 0x0000C260
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0000E068 File Offset: 0x0000C268
		public TutorialPanelImageWidget LeftItem { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000E071 File Offset: 0x0000C271
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0000E079 File Offset: 0x0000C279
		public TutorialPanelImageWidget RightItem { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000E082 File Offset: 0x0000C282
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0000E08A File Offset: 0x0000C28A
		public TutorialPanelImageWidget BottomItem { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000E093 File Offset: 0x0000C293
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0000E09B File Offset: 0x0000C29B
		public TutorialPanelImageWidget TopItem { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		public TutorialPanelImageWidget LeftTopItem { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000E0B5 File Offset: 0x0000C2B5
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0000E0BD File Offset: 0x0000C2BD
		public TutorialPanelImageWidget RightTopItem { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000E0C6 File Offset: 0x0000C2C6
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x0000E0CE File Offset: 0x0000C2CE
		public TutorialPanelImageWidget LeftBottomItem { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0000E0D7 File Offset: 0x0000C2D7
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x0000E0DF File Offset: 0x0000C2DF
		public TutorialPanelImageWidget RightBottomItem { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0000E0F0 File Offset: 0x0000C2F0
		public TutorialPanelImageWidget CenterItem { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000E0F9 File Offset: 0x0000C2F9
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x0000E101 File Offset: 0x0000C301
		public TutorialArrowWidget ArrowWidget { get; set; }

		// Token: 0x0600046D RID: 1133 RVA: 0x0000E10A File Offset: 0x0000C30A
		public TutorialScreenWidget(UIContext context)
			: base(context)
		{
			EventManager.UIEventManager.RegisterEvent<TutorialHighlightItemBrushWidget.HighlightElementToggledEvent>(new Action<TutorialHighlightItemBrushWidget.HighlightElementToggledEvent>(this.OnHighlightElementToggleEvent));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000E12C File Offset: 0x0000C32C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initalized)
			{
				this.LeftItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.RightItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.BottomItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.TopItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.LeftTopItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.RightTopItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.LeftBottomItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.RightBottomItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.CenterItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this._initalized = true;
			}
			if (this._currentActiveHighligtFrame != null && this._currentActivePanelItem != null)
			{
				Tuple<Widget, Widget> leftAndRightElements = this.GetLeftAndRightElements();
				Tuple<Widget, Widget> topAndBottomElements = this.GetTopAndBottomElements();
				float num = leftAndRightElements.Item1.GlobalPosition.X + leftAndRightElements.Item1.Size.X;
				float x = leftAndRightElements.Item2.GlobalPosition.X;
				float y = topAndBottomElements.Item1.GlobalPosition.Y;
				float y2 = topAndBottomElements.Item2.GlobalPosition.Y;
				float num2 = MathF.Abs(num - x);
				float num3 = MathF.Abs(y - y2);
				this.ArrowWidget.ScaledPositionXOffset = num;
				this.ArrowWidget.ScaledPositionYOffset = y;
				this.ArrowWidget.SetArrowProperties(num2, num3, this.GetIsArrowDirectionIsDownwards(), this.GetIsArrowDirectionIsTowardsRight());
				this.ArrowWidget.IsVisible = true;
				return;
			}
			this.ArrowWidget.IsVisible = false;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		private bool GetIsArrowDirectionIsDownwards()
		{
			if (this._currentActiveHighligtFrame.GlobalPosition.Y < this._currentActivePanelItem.GlobalPosition.Y)
			{
				return this._currentActiveHighligtFrame.GlobalPosition.X < this._currentActivePanelItem.GlobalPosition.X;
			}
			return this._currentActivePanelItem.GlobalPosition.X < this._currentActiveHighligtFrame.GlobalPosition.X;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000E370 File Offset: 0x0000C570
		private bool GetIsArrowDirectionIsTowardsRight()
		{
			return this._currentActiveHighligtFrame.GlobalPosition.X > this._currentActivePanelItem.GlobalPosition.X;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000E394 File Offset: 0x0000C594
		private Tuple<Widget, Widget> GetLeftAndRightElements()
		{
			if (this._currentActiveHighligtFrame.GlobalPosition.X < this._currentActivePanelItem.GlobalPosition.X)
			{
				return new Tuple<Widget, Widget>(this._currentActiveHighligtFrame, this._currentActivePanelItem);
			}
			return new Tuple<Widget, Widget>(this._currentActivePanelItem, this._currentActiveHighligtFrame);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		private Tuple<Widget, Widget> GetTopAndBottomElements()
		{
			if (this._currentActiveHighligtFrame.GlobalPosition.Y < this._currentActivePanelItem.GlobalPosition.Y)
			{
				return new Tuple<Widget, Widget>(this._currentActiveHighligtFrame, this._currentActivePanelItem);
			}
			return new Tuple<Widget, Widget>(this._currentActivePanelItem, this._currentActiveHighligtFrame);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000E43A File Offset: 0x0000C63A
		private void OnTutorialItemPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsDisabled")
			{
				if (propertyValue)
				{
					this._currentActivePanelItem = null;
					this.ArrowWidget.ResetFade();
					return;
				}
				this._currentActivePanelItem = widget as TutorialPanelImageWidget;
				this.ArrowWidget.DisableFade();
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000E476 File Offset: 0x0000C676
		private void OnHighlightElementToggleEvent(TutorialHighlightItemBrushWidget.HighlightElementToggledEvent obj)
		{
			if (obj.IsEnabled)
			{
				this._currentActiveHighligtFrame = obj.HighlightFrameWidget;
				this.ArrowWidget.ResetFade();
				return;
			}
			this.ArrowWidget.DisableFade();
			this._currentActiveHighligtFrame = null;
		}

		// Token: 0x040001E1 RID: 481
		private bool _initalized;

		// Token: 0x040001E2 RID: 482
		private TutorialHighlightItemBrushWidget _currentActiveHighligtFrame;

		// Token: 0x040001E3 RID: 483
		private TutorialPanelImageWidget _currentActivePanelItem;
	}
}
