using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000BE RID: 190
	public class MultiplayerDuelKillFeedItemWidget : MultiplayerGeneralKillFeedItemWidget
	{
		// Token: 0x06000A01 RID: 2561 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		public MultiplayerDuelKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0001C2F1 File Offset: 0x0001A4F1
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x0001C2FC File Offset: 0x0001A4FC
		[Editor(false)]
		public bool IsEndOfDuel
		{
			get
			{
				return this._isEndOfDuel;
			}
			set
			{
				if (value != this._isEndOfDuel)
				{
					this._isEndOfDuel = value;
					base.OnPropertyChanged(value, "IsEndOfDuel");
					if (value)
					{
						BrushWidget background = this.Background;
						if (background != null)
						{
							background.SetState("EndOfDuel");
						}
						BrushWidget victimCompassBackground = this.VictimCompassBackground;
						if (victimCompassBackground != null)
						{
							victimCompassBackground.SetState("EndOfDuel");
						}
						BrushWidget murdererCompassBackground = this.MurdererCompassBackground;
						if (murdererCompassBackground != null)
						{
							murdererCompassBackground.SetState("EndOfDuel");
						}
						ScrollingRichTextWidget victimNameText = this.VictimNameText;
						if (victimNameText != null)
						{
							victimNameText.SetState("EndOfDuel");
						}
						ScrollingRichTextWidget murdererNameText = this.MurdererNameText;
						if (murdererNameText != null)
						{
							murdererNameText.SetState("EndOfDuel");
						}
						TextWidget victimScoreText = this.VictimScoreText;
						if (victimScoreText != null)
						{
							victimScoreText.SetState("EndOfDuel");
						}
						TextWidget murdererScoreText = this.MurdererScoreText;
						if (murdererScoreText == null)
						{
							return;
						}
						murdererScoreText.SetState("EndOfDuel");
					}
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0001C3C7 File Offset: 0x0001A5C7
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0001C3CF File Offset: 0x0001A5CF
		[Editor(false)]
		public BrushWidget Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChanged<BrushWidget>(value, "Background");
				}
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0001C3ED File Offset: 0x0001A5ED
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0001C3F5 File Offset: 0x0001A5F5
		[Editor(false)]
		public BrushWidget VictimCompassBackground
		{
			get
			{
				return this._victimCompassBackground;
			}
			set
			{
				if (value != this._victimCompassBackground)
				{
					this._victimCompassBackground = value;
					base.OnPropertyChanged<BrushWidget>(value, "VictimCompassBackground");
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0001C413 File Offset: 0x0001A613
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0001C41B File Offset: 0x0001A61B
		[Editor(false)]
		public BrushWidget MurdererCompassBackground
		{
			get
			{
				return this._murdererCompassBackground;
			}
			set
			{
				if (value != this._murdererCompassBackground)
				{
					this._murdererCompassBackground = value;
					base.OnPropertyChanged<BrushWidget>(value, "MurdererCompassBackground");
				}
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0001C439 File Offset: 0x0001A639
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x0001C441 File Offset: 0x0001A641
		[Editor(false)]
		public ScrollingRichTextWidget VictimNameText
		{
			get
			{
				return this._victimNameText;
			}
			set
			{
				if (value != this._victimNameText)
				{
					this._victimNameText = value;
					base.OnPropertyChanged<ScrollingRichTextWidget>(value, "VictimNameText");
				}
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0001C45F File Offset: 0x0001A65F
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x0001C467 File Offset: 0x0001A667
		[Editor(false)]
		public ScrollingRichTextWidget MurdererNameText
		{
			get
			{
				return this._murdererNameText;
			}
			set
			{
				if (value != this._murdererNameText)
				{
					this._murdererNameText = value;
					base.OnPropertyChanged<ScrollingRichTextWidget>(value, "MurdererNameText");
				}
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0001C485 File Offset: 0x0001A685
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0001C48D File Offset: 0x0001A68D
		[Editor(false)]
		public TextWidget VictimScoreText
		{
			get
			{
				return this._victimScoreText;
			}
			set
			{
				if (value != this._victimScoreText)
				{
					this._victimScoreText = value;
					base.OnPropertyChanged<TextWidget>(value, "VictimScoreText");
				}
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001C4AB File Offset: 0x0001A6AB
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0001C4B3 File Offset: 0x0001A6B3
		[Editor(false)]
		public TextWidget MurdererScoreText
		{
			get
			{
				return this._murdererScoreText;
			}
			set
			{
				if (value != this._murdererScoreText)
				{
					this._murdererScoreText = value;
					base.OnPropertyChanged<TextWidget>(value, "MurdererScoreText");
				}
			}
		}

		// Token: 0x04000482 RID: 1154
		private const string EndOfDuelState = "EndOfDuel";

		// Token: 0x04000483 RID: 1155
		private bool _isEndOfDuel;

		// Token: 0x04000484 RID: 1156
		private BrushWidget _background;

		// Token: 0x04000485 RID: 1157
		private BrushWidget _victimCompassBackground;

		// Token: 0x04000486 RID: 1158
		private BrushWidget _murdererCompassBackground;

		// Token: 0x04000487 RID: 1159
		private ScrollingRichTextWidget _victimNameText;

		// Token: 0x04000488 RID: 1160
		private ScrollingRichTextWidget _murdererNameText;

		// Token: 0x04000489 RID: 1161
		private TextWidget _victimScoreText;

		// Token: 0x0400048A RID: 1162
		private TextWidget _murdererScoreText;
	}
}
