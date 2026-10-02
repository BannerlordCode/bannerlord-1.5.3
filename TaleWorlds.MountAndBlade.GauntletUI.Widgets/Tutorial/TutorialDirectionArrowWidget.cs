using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004A RID: 74
	public class TutorialDirectionArrowWidget : Widget
	{
		// Token: 0x0600041B RID: 1051 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		public TutorialDirectionArrowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000CFC8 File Offset: 0x0000B1C8
		private void UpdateArrowState()
		{
			if (this.VerticalArrowWidget != null && this.HorizontalArrowWidget != null && !string.IsNullOrEmpty(this.ArrowState))
			{
				if (this.ArrowState == "Right" || this.ArrowState == "Left")
				{
					this.HorizontalArrowWidget.SetState(this._arrowState);
					this.VerticalArrowWidget.SetState("Default");
					return;
				}
				if (this.ArrowState == "Up" || this.ArrowState == "Down")
				{
					this.HorizontalArrowWidget.SetState("Default");
					this.VerticalArrowWidget.SetState(this._arrowState);
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0000D086 File Offset: 0x0000B286
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0000D08E File Offset: 0x0000B28E
		[Editor(false)]
		public string ArrowState
		{
			get
			{
				return this._arrowState;
			}
			set
			{
				if (value != this._arrowState)
				{
					this._arrowState = value;
					base.OnPropertyChanged<string>(value, "ArrowState");
					this.UpdateArrowState();
				}
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000D0B7 File Offset: 0x0000B2B7
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000D0BF File Offset: 0x0000B2BF
		[Editor(false)]
		public BrushWidget HorizontalArrowWidget
		{
			get
			{
				return this._horizontalArrowWidget;
			}
			set
			{
				if (this._horizontalArrowWidget != value)
				{
					this._horizontalArrowWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "HorizontalArrowWidget");
					this.UpdateArrowState();
				}
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000D0E3 File Offset: 0x0000B2E3
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0000D0EB File Offset: 0x0000B2EB
		[Editor(false)]
		public BrushWidget VerticalArrowWidget
		{
			get
			{
				return this._verticalArrowWidget;
			}
			set
			{
				if (this._verticalArrowWidget != value)
				{
					this._verticalArrowWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "VerticalArrowWidget");
					this.UpdateArrowState();
				}
			}
		}

		// Token: 0x040001B5 RID: 437
		private string _arrowState;

		// Token: 0x040001B6 RID: 438
		private BrushWidget _horizontalArrowWidget;

		// Token: 0x040001B7 RID: 439
		private BrushWidget _verticalArrowWidget;
	}
}
