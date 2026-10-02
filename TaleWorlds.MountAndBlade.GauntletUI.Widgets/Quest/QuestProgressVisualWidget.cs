using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x0200005F RID: 95
	public class QuestProgressVisualWidget : Widget
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0000FE09 File Offset: 0x0000E009
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0000FE11 File Offset: 0x0000E011
		public Widget BarWidget { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0000FE1A File Offset: 0x0000E01A
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0000FE22 File Offset: 0x0000E022
		public Widget SliderWidget { get; set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0000FE2B File Offset: 0x0000E02B
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x0000FE33 File Offset: 0x0000E033
		public Widget CheckboxVisualWidget { get; set; }

		// Token: 0x06000538 RID: 1336 RVA: 0x0000FE3C File Offset: 0x0000E03C
		public QuestProgressVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000FE48 File Offset: 0x0000E048
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				bool flag = this.CurrentProgress >= this.TargetProgress;
				base.IsVisible = !flag && this.IsValid;
				this.CheckboxVisualWidget.IsVisible = flag && this.IsValid;
				this.BarWidget.IsVisible = false;
				this.SliderWidget.IsVisible = false;
				if (base.IsVisible)
				{
					if (this.TargetProgress < 20)
					{
						for (int i = 0; i < this.TargetProgress; i++)
						{
							BrushWidget brushWidget = new BrushWidget(base.Context)
							{
								WidthSizePolicy = SizePolicy.Fixed,
								SuggestedWidth = this.ProgressStoneWidth,
								HeightSizePolicy = SizePolicy.Fixed,
								SuggestedHeight = this.ProgressStoneHeight,
								MarginRight = (float)this.HorizontalSpacingBetweenStones / 2f,
								MarginLeft = (float)this.HorizontalSpacingBetweenStones / 2f,
								IsEnabled = false
							};
							if (i < this.CurrentProgress)
							{
								brushWidget.Brush = base.Context.GetBrush("StageTask.ProgressStone");
								brushWidget.Brush.AlphaFactor = 0.8f;
							}
							this.BarWidget.AddChild(brushWidget);
						}
						this.BarWidget.IsVisible = true;
					}
					else if (this.TargetProgress >= 20)
					{
						this.SliderWidget.IsVisible = true;
						this.SliderWidget.IsDisabled = true;
					}
				}
				this._initialized = true;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0000FFBA File Offset: 0x0000E1BA
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x0000FFC2 File Offset: 0x0000E1C2
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (this._isValid != value)
				{
					this._isValid = value;
				}
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x0000FFDC File Offset: 0x0000E1DC
		public float ProgressStoneWidth
		{
			get
			{
				return this._progressStoneWidth;
			}
			set
			{
				if (this._progressStoneWidth != value)
				{
					this._progressStoneWidth = value;
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0000FFEE File Offset: 0x0000E1EE
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0000FFF6 File Offset: 0x0000E1F6
		public float ProgressStoneHeight
		{
			get
			{
				return this._progressStoneHeight;
			}
			set
			{
				if (this._progressStoneHeight != value)
				{
					this._progressStoneHeight = value;
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00010008 File Offset: 0x0000E208
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00010010 File Offset: 0x0000E210
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (this._currentProgress != value)
				{
					this._currentProgress = value;
				}
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00010022 File Offset: 0x0000E222
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001002A File Offset: 0x0000E22A
		public int TargetProgress
		{
			get
			{
				return this._targetProgress;
			}
			set
			{
				if (this._targetProgress != value)
				{
					this._targetProgress = value;
				}
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001003C File Offset: 0x0000E23C
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00010044 File Offset: 0x0000E244
		public int HorizontalSpacingBetweenStones
		{
			get
			{
				return this._horizontalSpacingBetweenStones;
			}
			set
			{
				if (this._horizontalSpacingBetweenStones != value)
				{
					this._horizontalSpacingBetweenStones = value;
				}
			}
		}

		// Token: 0x04000237 RID: 567
		private bool _initialized;

		// Token: 0x0400023B RID: 571
		private int _currentProgress;

		// Token: 0x0400023C RID: 572
		private int _targetProgress;

		// Token: 0x0400023D RID: 573
		private float _progressStoneWidth;

		// Token: 0x0400023E RID: 574
		private float _progressStoneHeight;

		// Token: 0x0400023F RID: 575
		private int _horizontalSpacingBetweenStones;

		// Token: 0x04000240 RID: 576
		private bool _isValid;
	}
}
