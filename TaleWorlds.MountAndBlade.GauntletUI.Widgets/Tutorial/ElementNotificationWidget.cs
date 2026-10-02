using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x02000048 RID: 72
	public class ElementNotificationWidget : Widget
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x0000CA25 File Offset: 0x0000AC25
		public ElementNotificationWidget(UIContext context)
			: base(context)
		{
			base.IsVisible = false;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000CA40 File Offset: 0x0000AC40
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			string elementID = this.ElementID;
			if (elementID != null && elementID.Any<char>() && this.ElementToHighlight == null && !this._doesNotHaveElement)
			{
				this.ElementToHighlight = this.FindElementWithID(base.EventManager.Root, this.ElementID);
				this._doesNotHaveElement = true;
				if (this.ElementToHighlight != null)
				{
					this.TutorialFrameWidget.IsVisible = true;
					this.TutorialFrameWidget.IsHighlightEnabled = true;
					this.TutorialFrameWidget.ParentWidget = this.ElementToHighlight;
					if (this.ElementToHighlight.HeightSizePolicy == SizePolicy.CoverChildren || this.ElementToHighlight.WidthSizePolicy == SizePolicy.CoverChildren)
					{
						this.TutorialFrameWidget.WidthSizePolicy = SizePolicy.Fixed;
						this.TutorialFrameWidget.HeightSizePolicy = SizePolicy.Fixed;
						this._shouldSyncSize = true;
					}
					else
					{
						this.TutorialFrameWidget.WidthSizePolicy = SizePolicy.StretchToParent;
						this.TutorialFrameWidget.HeightSizePolicy = SizePolicy.StretchToParent;
						this._shouldSyncSize = false;
					}
				}
			}
			if (this._shouldSyncSize && this.ElementToHighlight != null && this.ElementToHighlight.Size.X > 1f && this.ElementToHighlight.Size.Y > 1f)
			{
				base.ScaledSuggestedWidth = this.ElementToHighlight.Size.X - 1f;
				base.ScaledSuggestedHeight = this.ElementToHighlight.Size.Y - 1f;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		private Widget FindElementWithID(Widget current, string ID)
		{
			if (current != null)
			{
				for (int i = 0; i < current.ChildCount; i++)
				{
					if (current.GetChild(i).Id == ID)
					{
						return current.GetChild(i);
					}
					Widget widget = this.FindElementWithID(current.GetChild(i), ID);
					if (widget != null)
					{
						return widget;
					}
				}
			}
			return null;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000CBFE File Offset: 0x0000ADFE
		private void ResetHighlight()
		{
			if (this.TutorialFrameWidget != null)
			{
				this.TutorialFrameWidget.ParentWidget = this;
				this._doesNotHaveElement = false;
				this.TutorialFrameWidget.IsVisible = false;
				this.TutorialFrameWidget.IsHighlightEnabled = false;
				this.ElementToHighlight = null;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000CC3A File Offset: 0x0000AE3A
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000CC44 File Offset: 0x0000AE44
		[Editor(false)]
		public string ElementID
		{
			get
			{
				return this._elementID;
			}
			set
			{
				if (this._elementID != value)
				{
					if (this._elementID != string.Empty && value == string.Empty)
					{
						this.ResetHighlight();
					}
					this._elementID = value;
					base.OnPropertyChanged<string>(value, "ElementID");
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000CC97 File Offset: 0x0000AE97
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000CC9F File Offset: 0x0000AE9F
		[Editor(false)]
		public Widget ElementToHighlight
		{
			get
			{
				return this._elementToHighlight;
			}
			set
			{
				if (this._elementToHighlight != value)
				{
					this._elementToHighlight = value;
					base.OnPropertyChanged<Widget>(value, "ElementToHighlight");
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000CCBD File Offset: 0x0000AEBD
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000CCC5 File Offset: 0x0000AEC5
		[Editor(false)]
		public TutorialHighlightItemBrushWidget TutorialFrameWidget
		{
			get
			{
				return this._tutorialFrameWidget;
			}
			set
			{
				if (this._tutorialFrameWidget != value)
				{
					this._tutorialFrameWidget = value;
					base.OnPropertyChanged<TutorialHighlightItemBrushWidget>(value, "TutorialFrameWidget");
					if (this._tutorialFrameWidget != null)
					{
						this._tutorialFrameWidget.IsVisible = false;
					}
				}
			}
		}

		// Token: 0x040001A7 RID: 423
		private bool _doesNotHaveElement;

		// Token: 0x040001A8 RID: 424
		private bool _shouldSyncSize;

		// Token: 0x040001A9 RID: 425
		private string _elementID = string.Empty;

		// Token: 0x040001AA RID: 426
		private Widget _elementToHighlight;

		// Token: 0x040001AB RID: 427
		private TutorialHighlightItemBrushWidget _tutorialFrameWidget;
	}
}
