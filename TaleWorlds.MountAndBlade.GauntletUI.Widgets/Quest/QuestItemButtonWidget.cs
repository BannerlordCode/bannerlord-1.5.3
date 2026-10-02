using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x0200005D RID: 93
	public class QuestItemButtonWidget : ButtonWidget
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0000FA30 File Offset: 0x0000DC30
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x0000FA38 File Offset: 0x0000DC38
		public Brush MainStoryLineItemBrush { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0000FA41 File Offset: 0x0000DC41
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0000FA49 File Offset: 0x0000DC49
		public Brush NavalStorylineItemBrush { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0000FA52 File Offset: 0x0000DC52
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0000FA5A File Offset: 0x0000DC5A
		public Brush NormalItemBrush { get; set; }

		// Token: 0x06000518 RID: 1304 RVA: 0x0000FA63 File Offset: 0x0000DC63
		public QuestItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				if (this.IsNavalStorylineQuest)
				{
					base.Brush = this.NavalStorylineItemBrush;
				}
				else if (this.IsMainStoryLineQuest)
				{
					base.Brush = this.MainStoryLineItemBrush;
				}
				else
				{
					base.Brush = this.NormalItemBrush;
				}
				this._initialized = true;
			}
			if (this.QuestNameText != null && this.QuestDateText != null)
			{
				if (base.CurrentState == "Pressed")
				{
					this.QuestNameText.PositionYOffset = (float)this.QuestNameYOffset;
					this.QuestNameText.PositionXOffset = (float)this.QuestNameXOffset;
					this.QuestDateText.PositionYOffset = (float)this.QuestDateYOffset;
					this.QuestDateText.PositionXOffset = (float)this.QuestDateXOffset;
				}
				else
				{
					this.QuestNameText.PositionYOffset = 0f;
					this.QuestNameText.PositionXOffset = 0f;
					this.QuestDateText.PositionYOffset = 0f;
					this.QuestDateText.PositionXOffset = 0f;
				}
			}
			if (this.QuestDateText != null)
			{
				if (this.IsCompleted)
				{
					this.QuestDateText.IsVisible = false;
					return;
				}
				this.QuestDateText.IsHidden = this.IsRemainingDaysHidden;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0000FBA7 File Offset: 0x0000DDA7
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x0000FBAF File Offset: 0x0000DDAF
		[Editor(false)]
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
			set
			{
				if (this._isCompleted != value)
				{
					this._isCompleted = value;
					base.OnPropertyChanged(value, "IsCompleted");
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0000FBCD File Offset: 0x0000DDCD
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x0000FBD5 File Offset: 0x0000DDD5
		[Editor(false)]
		public bool IsMainStoryLineQuest
		{
			get
			{
				return this._isMainStoryLineQuest;
			}
			set
			{
				if (this._isMainStoryLineQuest != value)
				{
					this._isMainStoryLineQuest = value;
					base.OnPropertyChanged(value, "IsMainStoryLineQuest");
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0000FBF3 File Offset: 0x0000DDF3
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x0000FBFB File Offset: 0x0000DDFB
		[Editor(false)]
		public bool IsNavalStorylineQuest
		{
			get
			{
				return this._isNavalStorylineQuest;
			}
			set
			{
				if (this._isNavalStorylineQuest != value)
				{
					this._isNavalStorylineQuest = value;
					base.OnPropertyChanged(value, "IsNavalStorylineQuest");
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0000FC19 File Offset: 0x0000DE19
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x0000FC21 File Offset: 0x0000DE21
		[Editor(false)]
		public bool IsRemainingDaysHidden
		{
			get
			{
				return this._isRemainingDaysHidden;
			}
			set
			{
				if (this._isRemainingDaysHidden != value)
				{
					this._isRemainingDaysHidden = value;
					base.OnPropertyChanged(value, "IsRemainingDaysHidden");
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0000FC3F File Offset: 0x0000DE3F
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x0000FC47 File Offset: 0x0000DE47
		[Editor(false)]
		public TextWidget QuestNameText
		{
			get
			{
				return this._questNameText;
			}
			set
			{
				if (this._questNameText != value)
				{
					this._questNameText = value;
					base.OnPropertyChanged<TextWidget>(value, "QuestNameText");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0000FC65 File Offset: 0x0000DE65
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x0000FC6D File Offset: 0x0000DE6D
		[Editor(false)]
		public TextWidget QuestDateText
		{
			get
			{
				return this._questDateText;
			}
			set
			{
				if (this._questDateText != value)
				{
					this._questDateText = value;
					base.OnPropertyChanged<TextWidget>(value, "QuestDateText");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0000FC8B File Offset: 0x0000DE8B
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0000FC93 File Offset: 0x0000DE93
		[Editor(false)]
		public int QuestNameYOffset
		{
			get
			{
				return this._questNameYOffset;
			}
			set
			{
				if (this._questNameYOffset != value)
				{
					this._questNameYOffset = value;
					base.OnPropertyChanged(value, "QuestNameYOffset");
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0000FCB1 File Offset: 0x0000DEB1
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x0000FCB9 File Offset: 0x0000DEB9
		[Editor(false)]
		public int QuestNameXOffset
		{
			get
			{
				return this._questNameXOffset;
			}
			set
			{
				if (this._questNameXOffset != value)
				{
					this._questNameXOffset = value;
					base.OnPropertyChanged(value, "QuestNameXOffset");
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0000FCD7 File Offset: 0x0000DED7
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x0000FCDF File Offset: 0x0000DEDF
		[Editor(false)]
		public int QuestDateYOffset
		{
			get
			{
				return this._questDateYOffset;
			}
			set
			{
				if (this._questDateYOffset != value)
				{
					this._questDateYOffset = value;
					base.OnPropertyChanged(value, "QuestDateYOffset");
				}
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x0000FCFD File Offset: 0x0000DEFD
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0000FD05 File Offset: 0x0000DF05
		[Editor(false)]
		public int QuestDateXOffset
		{
			get
			{
				return this._questDateXOffset;
			}
			set
			{
				if (this._questDateXOffset != value)
				{
					this._questDateXOffset = value;
					base.OnPropertyChanged(value, "QuestDateXOffset");
				}
			}
		}

		// Token: 0x04000228 RID: 552
		private bool _initialized;

		// Token: 0x0400022C RID: 556
		private TextWidget _questNameText;

		// Token: 0x0400022D RID: 557
		private TextWidget _questDateText;

		// Token: 0x0400022E RID: 558
		private int _questNameYOffset;

		// Token: 0x0400022F RID: 559
		private int _questNameXOffset;

		// Token: 0x04000230 RID: 560
		private int _questDateYOffset;

		// Token: 0x04000231 RID: 561
		private int _questDateXOffset;

		// Token: 0x04000232 RID: 562
		private bool _isCompleted;

		// Token: 0x04000233 RID: 563
		private bool _isRemainingDaysHidden;

		// Token: 0x04000234 RID: 564
		private bool _isMainStoryLineQuest;

		// Token: 0x04000235 RID: 565
		private bool _isNavalStorylineQuest;
	}
}
