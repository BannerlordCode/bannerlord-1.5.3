using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000020 RID: 32
	public class GameMenuItemWidget : Widget
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006400 File Offset: 0x00004600
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006408 File Offset: 0x00004608
		public Brush DefaultTextBrush { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006411 File Offset: 0x00004611
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00006419 File Offset: 0x00004619
		public Brush HoveredTextBrush { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006422 File Offset: 0x00004622
		// (set) Token: 0x06000188 RID: 392 RVA: 0x0000642A File Offset: 0x0000462A
		public Brush PressedTextBrush { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006433 File Offset: 0x00004633
		// (set) Token: 0x0600018A RID: 394 RVA: 0x0000643B File Offset: 0x0000463B
		public Brush DisabledTextBrush { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006444 File Offset: 0x00004644
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000644C File Offset: 0x0000464C
		public Brush NormalQuestBrush { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00006455 File Offset: 0x00004655
		// (set) Token: 0x0600018E RID: 398 RVA: 0x0000645D File Offset: 0x0000465D
		public Brush MainStoryQuestBrush { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00006466 File Offset: 0x00004666
		// (set) Token: 0x06000190 RID: 400 RVA: 0x0000646E File Offset: 0x0000466E
		public RichTextWidget ItemRichTextWidget { get; set; }

		// Token: 0x06000191 RID: 401 RVA: 0x00006477 File Offset: 0x00004677
		public GameMenuItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000649C File Offset: 0x0000469C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._latestTextWidgetState != this.ItemRichTextWidget.CurrentState)
			{
				if (this.ItemRichTextWidget.CurrentState == "Default")
				{
					this.ItemRichTextWidget.Brush = this.DefaultTextBrush;
				}
				else if (this.ItemRichTextWidget.CurrentState == "Hovered")
				{
					this.ItemRichTextWidget.Brush = this.HoveredTextBrush;
				}
				else if (this.ItemRichTextWidget.CurrentState == "Pressed")
				{
					this.ItemRichTextWidget.Brush = this.PressedTextBrush;
				}
				else if (this.ItemRichTextWidget.CurrentState == "Disabled")
				{
					this.ItemRichTextWidget.Brush = this.DisabledTextBrush;
				}
				this._latestTextWidgetState = this.ItemRichTextWidget.CurrentState;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006584 File Offset: 0x00004784
		private void UpdateLeaveTypeIcon()
		{
			if (!string.IsNullOrEmpty(this.LeaveType))
			{
				BrushLayer layer = this.LeaveTypeIcon.ReadOnlyBrush.GetLayer(this.LeaveType);
				Sprite sprite = ((layer != null) ? layer.Sprite : null);
				bool flag = sprite != null;
				this.LeaveTypeIcon.IsVisible = flag;
				if (flag)
				{
					this.LeaveTypeIcon.Brush.Sprite = sprite;
					this.LeaveTypeIcon.Brush.DefaultLayer.Sprite = sprite;
				}
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000065FC File Offset: 0x000047FC
		private void UpdateLeaveTypeSound()
		{
			ButtonWidget parentButton = this.ParentButton;
			AudioProperty audioProperty = ((parentButton != null) ? parentButton.Brush.SoundProperties.GetEventAudioProperty("Click") : null);
			if (audioProperty != null)
			{
				audioProperty.AudioName = "default";
				string leaveType = this.LeaveType;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(leaveType);
				if (num <= 2060054327U)
				{
					if (num <= 480945363U)
					{
						if (num != 379087577U)
						{
							if (num != 452811635U)
							{
								if (num != 480945363U)
								{
									return;
								}
								if (!(leaveType == "CallFleet"))
								{
									return;
								}
								audioProperty.AudioName = "panels/panel_call_fleet";
								return;
							}
							else
							{
								if (!(leaveType == "BesiegeTown"))
								{
									return;
								}
								audioProperty.AudioName = "panels/siege/besiege";
								return;
							}
						}
						else
						{
							if (!(leaveType == "Mission"))
							{
								return;
							}
							if (this.GameMenuStringId == "menu_siege_strategies")
							{
								audioProperty.AudioName = "panels/siege/sally_out";
								return;
							}
							return;
						}
					}
					else if (num != 890218248U)
					{
						if (num != 2015531671U)
						{
							if (num != 2060054327U)
							{
								return;
							}
							if (!(leaveType == "HostileAction"))
							{
								return;
							}
							if (!(this.GameMenuStringId == "encounter"))
							{
								return;
							}
							if (this.IsNavalBattle)
							{
								audioProperty.AudioName = "panels/battle/naval_attack_large";
								return;
							}
							if (this.BattleSize < 50)
							{
								audioProperty.AudioName = "panels/battle/attack_small";
								return;
							}
							if (this.BattleSize < 100)
							{
								audioProperty.AudioName = "panels/battle/attack_medium";
								return;
							}
							audioProperty.AudioName = "panels/battle/attack_large";
							return;
						}
						else
						{
							if (!(leaveType == "RepairShips"))
							{
								return;
							}
							audioProperty.AudioName = "repair_all_ships";
							return;
						}
					}
					else if (!(leaveType == "Devastate"))
					{
						return;
					}
				}
				else if (num <= 2995080818U)
				{
					if (num != 2497282626U)
					{
						if (num != 2941774007U)
						{
							if (num != 2995080818U)
							{
								return;
							}
							if (!(leaveType == "ManageFleet"))
							{
								return;
							}
							audioProperty.AudioName = "panels/panel_manage_fleet";
							return;
						}
						else
						{
							if (!(leaveType == "VisitPort"))
							{
								return;
							}
							audioProperty.AudioName = "panels/panel_visit_port";
							return;
						}
					}
					else
					{
						if (!(leaveType == "TakeFerry"))
						{
							return;
						}
						audioProperty.AudioName = "panels/take_ferry";
						return;
					}
				}
				else if (num <= 3859548670U)
				{
					if (num != 3203334290U)
					{
						if (num != 3859548670U)
						{
							return;
						}
						if (!(leaveType == "LeaveTroopsAndFlee"))
						{
							return;
						}
						if (this.GameMenuStringId == "encounter" || this.GameMenuStringId == "encounter_interrupted_siege_preparations" || this.GameMenuStringId == "menu_siege_strategies")
						{
							audioProperty.AudioName = "panels/battle/retreat";
							return;
						}
						return;
					}
					else
					{
						if (!(leaveType == "LeadAssault"))
						{
							return;
						}
						audioProperty.AudioName = "panels/siege/lead_assault";
						return;
					}
				}
				else if (num != 4024512205U)
				{
					if (num != 4158742509U)
					{
						return;
					}
					if (!(leaveType == "Pillage"))
					{
						return;
					}
				}
				else
				{
					if (!(leaveType == "Surrender"))
					{
						return;
					}
					if (this.GameMenuStringId == "encounter")
					{
						audioProperty.AudioName = "panels/battle/retreat";
						return;
					}
					return;
				}
				audioProperty.AudioName = "panels/siege/raid";
				return;
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006914 File Offset: 0x00004B14
		private void SetProgressIconType(int type, Widget progressWidget)
		{
			string text = string.Empty;
			switch (type)
			{
			case 0:
				text = "Default";
				break;
			case 1:
				text = "Available";
				break;
			case 2:
				text = "Active";
				break;
			case 3:
				text = "Completed";
				break;
			default:
				text = "";
				break;
			}
			if (progressWidget == this.QuestIconWidget)
			{
				this.QuestIconWidget.Brush = (this.IsMainStoryQuest ? this.MainStoryQuestBrush : this.NormalQuestBrush);
			}
			if (!string.IsNullOrEmpty(text) && type != 0)
			{
				progressWidget.SetState(text);
				progressWidget.IsVisible = true;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000069A8 File Offset: 0x00004BA8
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000069B0 File Offset: 0x00004BB0
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (this._itemType != value)
				{
					this._itemType = value;
					base.OnPropertyChanged(value, "ItemType");
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000069CE File Offset: 0x00004BCE
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000069D6 File Offset: 0x00004BD6
		public BrushWidget QuestIconWidget
		{
			get
			{
				return this._questIconWidget;
			}
			set
			{
				if (this._questIconWidget != value)
				{
					this._questIconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "QuestIconWidget");
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000069F4 File Offset: 0x00004BF4
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000069FC File Offset: 0x00004BFC
		public BrushWidget IssueIconWidget
		{
			get
			{
				return this._issueIconWidget;
			}
			set
			{
				if (this._issueIconWidget != value)
				{
					this._issueIconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "IssueIconWidget");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006A1A File Offset: 0x00004C1A
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00006A22 File Offset: 0x00004C22
		public string LeaveType
		{
			get
			{
				return this._leaveType;
			}
			set
			{
				if (this._leaveType != value)
				{
					this._leaveType = value;
					base.OnPropertyChanged<string>(value, "LeaveType");
					this.UpdateLeaveTypeIcon();
					this.UpdateLeaveTypeSound();
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00006A51 File Offset: 0x00004C51
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00006A59 File Offset: 0x00004C59
		public bool IsMainStoryQuest
		{
			get
			{
				return this._isMainStoryQuest;
			}
			set
			{
				if (this._isMainStoryQuest != value)
				{
					this._isMainStoryQuest = value;
					base.OnPropertyChanged(value, "IsMainStoryQuest");
					this.SetProgressIconType(this.QuestType, this.QuestIconWidget);
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00006A89 File Offset: 0x00004C89
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00006A91 File Offset: 0x00004C91
		public int QuestType
		{
			get
			{
				return this._questType;
			}
			set
			{
				if (this._questType != value)
				{
					this._questType = value;
					base.OnPropertyChanged(value, "QuestType");
					this.SetProgressIconType(value, this.QuestIconWidget);
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006ABC File Offset: 0x00004CBC
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00006AC4 File Offset: 0x00004CC4
		public int IssueType
		{
			get
			{
				return this._issueType;
			}
			set
			{
				if (this._issueType != value)
				{
					this._issueType = value;
					base.OnPropertyChanged(value, "IssueType");
					this.SetProgressIconType(value, this.IssueIconWidget);
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00006AEF File Offset: 0x00004CEF
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00006AF7 File Offset: 0x00004CF7
		public bool IsWaitActive
		{
			get
			{
				return this._isWaitActive;
			}
			set
			{
				if (this._isWaitActive != value)
				{
					this._isWaitActive = value;
					base.OnPropertyChanged(value, "IsWaitActive");
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006B15 File Offset: 0x00004D15
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00006B1D File Offset: 0x00004D1D
		public BrushWidget LeaveTypeIcon
		{
			get
			{
				return this._leaveTypeIcon;
			}
			set
			{
				if (this._leaveTypeIcon != value)
				{
					this._leaveTypeIcon = value;
					base.OnPropertyChanged<BrushWidget>(value, "LeaveTypeIcon");
					if (value != null)
					{
						this.LeaveTypeIcon.IsVisible = false;
					}
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006B4A File Offset: 0x00004D4A
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006B52 File Offset: 0x00004D52
		public BrushWidget WaitStateWidget
		{
			get
			{
				return this._waitStateWidget;
			}
			set
			{
				if (this._waitStateWidget != value)
				{
					this._waitStateWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "WaitStateWidget");
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006B70 File Offset: 0x00004D70
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00006B78 File Offset: 0x00004D78
		public ButtonWidget ParentButton
		{
			get
			{
				return this._parentButton;
			}
			set
			{
				if (value != this._parentButton)
				{
					this._parentButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ParentButton");
					this._parentButton.boolPropertyChanged += this.ParentButton_PropertyChanged;
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006BAD File Offset: 0x00004DAD
		private void ParentButton_PropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsDisabled" || propertyName == "IsHighlightEnabled")
			{
				Action onOptionStateChanged = this.OnOptionStateChanged;
				if (onOptionStateChanged == null)
				{
					return;
				}
				onOptionStateChanged();
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00006BD9 File Offset: 0x00004DD9
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00006BE1 File Offset: 0x00004DE1
		public string GameMenuStringId
		{
			get
			{
				return this._gameMenuStringId;
			}
			set
			{
				if (value != this._gameMenuStringId)
				{
					this._gameMenuStringId = value;
					base.OnPropertyChanged<string>(value, "GameMenuStringId");
					this.UpdateLeaveTypeSound();
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00006C0A File Offset: 0x00004E0A
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00006C12 File Offset: 0x00004E12
		public int BattleSize
		{
			get
			{
				return this._battleSize;
			}
			set
			{
				if (value != this._battleSize)
				{
					this._battleSize = value;
					base.OnPropertyChanged(value, "BattleSize");
					this.UpdateLeaveTypeSound();
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00006C36 File Offset: 0x00004E36
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00006C3E File Offset: 0x00004E3E
		public bool IsNavalBattle
		{
			get
			{
				return this._isNavalBattle;
			}
			set
			{
				if (value != this._isNavalBattle)
				{
					this._isNavalBattle = value;
					base.OnPropertyChanged(value, "IsNavalBattle");
					this.UpdateLeaveTypeSound();
				}
			}
		}

		// Token: 0x040000B1 RID: 177
		public Action OnOptionStateChanged;

		// Token: 0x040000B9 RID: 185
		private string _latestTextWidgetState = "";

		// Token: 0x040000BA RID: 186
		private int _itemType;

		// Token: 0x040000BB RID: 187
		private bool _isWaitActive;

		// Token: 0x040000BC RID: 188
		private bool _isMainStoryQuest;

		// Token: 0x040000BD RID: 189
		private BrushWidget _waitStateWidget;

		// Token: 0x040000BE RID: 190
		private BrushWidget _leaveTypeIcon;

		// Token: 0x040000BF RID: 191
		private string _leaveType;

		// Token: 0x040000C0 RID: 192
		private int _questType = -1;

		// Token: 0x040000C1 RID: 193
		private int _issueType = -1;

		// Token: 0x040000C2 RID: 194
		private BrushWidget _questIconWidget;

		// Token: 0x040000C3 RID: 195
		private BrushWidget _issueIconWidget;

		// Token: 0x040000C4 RID: 196
		private ButtonWidget _parentButton;

		// Token: 0x040000C5 RID: 197
		private string _gameMenuStringId;

		// Token: 0x040000C6 RID: 198
		private int _battleSize;

		// Token: 0x040000C7 RID: 199
		private bool _isNavalBattle;
	}
}
