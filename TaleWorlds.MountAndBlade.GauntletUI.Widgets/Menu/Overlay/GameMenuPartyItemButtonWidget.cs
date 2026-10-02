using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000112 RID: 274
	public class GameMenuPartyItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00028863 File Offset: 0x00026A63
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x0002886B File Offset: 0x00026A6B
		public Brush PartyBackgroundBrush { get; set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00028874 File Offset: 0x00026A74
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0002887C File Offset: 0x00026A7C
		public Brush CharacterBackgroundBrush { get; set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00028885 File Offset: 0x00026A85
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x0002888D File Offset: 0x00026A8D
		public ImageWidget BackgroundImageWidget { get; set; }

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00028898 File Offset: 0x00026A98
		public GameMenuPartyItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x000288F1 File Offset: 0x00026AF1
		private string GetRelationBackgroundName(int relation)
		{
			return "";
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x000288F8 File Offset: 0x00026AF8
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._popupWidget == null)
			{
				Widget widget = this;
				while (widget != base.EventManager.Root && this._popupWidget == null && this._parentKnowsPopup)
				{
					if (widget is OverlayBaseWidget)
					{
						OverlayBaseWidget overlayBaseWidget = (OverlayBaseWidget)widget;
						if (overlayBaseWidget.PopupWidget == null)
						{
							this._parentKnowsPopup = false;
							break;
						}
						this._popupWidget = overlayBaseWidget.PopupWidget;
					}
					else
					{
						widget = widget.ParentWidget;
					}
				}
			}
			if (this.CurrentCharacterImageWidget != null)
			{
				this.CurrentCharacterImageWidget.Brush.SaturationFactor = (float)(this.IsMergedWithArmy ? 0 : (-100));
				this.CurrentCharacterImageWidget.Brush.ValueFactor = (float)(this.IsMergedWithArmy ? 0 : (-20));
			}
			if (!this._initialized)
			{
				this.BackgroundImageWidget.Brush = (this.IsPartyItem ? this.PartyBackgroundBrush : this.CharacterBackgroundBrush);
				this._initialized = true;
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x000289DF File Offset: 0x00026BDF
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this._popupWidget != null)
			{
				this._popupWidget.SetCurrentCharacter(this);
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x000289FB File Offset: 0x00026BFB
		// (set) Token: 0x06000EAB RID: 3755 RVA: 0x00028A03 File Offset: 0x00026C03
		[Editor(false)]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (this._relation != value)
				{
					this._relation = value;
					base.OnPropertyChanged(value, "Relation");
				}
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00028A21 File Offset: 0x00026C21
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x00028A29 File Offset: 0x00026C29
		[Editor(false)]
		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				if (this._location != value)
				{
					this._location = value;
					base.OnPropertyChanged<string>(value, "Location");
				}
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x00028A4C File Offset: 0x00026C4C
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x00028A54 File Offset: 0x00026C54
		[Editor(false)]
		public string Power
		{
			get
			{
				return this._power;
			}
			set
			{
				if (this._power != value)
				{
					this._power = value;
					base.OnPropertyChanged<string>(value, "Power");
				}
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00028A77 File Offset: 0x00026C77
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00028A7F File Offset: 0x00026C7F
		[Editor(false)]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (this._description != value)
				{
					this._description = value;
					base.OnPropertyChanged<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00028AA2 File Offset: 0x00026CA2
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x00028AAA File Offset: 0x00026CAA
		[Editor(false)]
		public string Profession
		{
			get
			{
				return this._profession;
			}
			set
			{
				if (this._profession != value)
				{
					this._profession = value;
					base.OnPropertyChanged<string>(value, "Profession");
				}
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00028ACD File Offset: 0x00026CCD
		// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x00028AD5 File Offset: 0x00026CD5
		[Editor(false)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (this._name != value)
				{
					this._name = value;
					base.OnPropertyChanged<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00028AF8 File Offset: 0x00026CF8
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x00028B00 File Offset: 0x00026D00
		[Editor(false)]
		public bool IsMergedWithArmy
		{
			get
			{
				return this._isMergedWithArmy;
			}
			set
			{
				if (this._isMergedWithArmy != value)
				{
					this._isMergedWithArmy = value;
				}
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00028B12 File Offset: 0x00026D12
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00028B1A File Offset: 0x00026D1A
		[Editor(false)]
		public bool IsPartyItem
		{
			get
			{
				return this._isPartyItem;
			}
			set
			{
				if (this._isPartyItem != value)
				{
					this._isPartyItem = value;
				}
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00028B2C File Offset: 0x00026D2C
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00028B34 File Offset: 0x00026D34
		[Editor(false)]
		public Widget ContextMenu
		{
			get
			{
				return this._contextMenu;
			}
			set
			{
				if (this._contextMenu != value)
				{
					this._contextMenu = value;
					base.OnPropertyChanged<Widget>(value, "ContextMenu");
				}
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00028B52 File Offset: 0x00026D52
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x00028B5A File Offset: 0x00026D5A
		[Editor(false)]
		public ImageIdentifierWidget CurrentCharacterImageWidget
		{
			get
			{
				return this._currentCharacterImageWidget;
			}
			set
			{
				if (this._currentCharacterImageWidget != value)
				{
					this._currentCharacterImageWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "CurrentCharacterImageWidget");
				}
			}
		}

		// Token: 0x040006A9 RID: 1705
		private bool _initialized;

		// Token: 0x040006AA RID: 1706
		private int _relation;

		// Token: 0x040006AB RID: 1707
		private string _location = "";

		// Token: 0x040006AC RID: 1708
		private string _description = "";

		// Token: 0x040006AD RID: 1709
		private string _profession = "";

		// Token: 0x040006AE RID: 1710
		private string _power = "";

		// Token: 0x040006AF RID: 1711
		private string _name = "";

		// Token: 0x040006B0 RID: 1712
		private Widget _contextMenu;

		// Token: 0x040006B1 RID: 1713
		private ImageIdentifierWidget _currentCharacterImageWidget;

		// Token: 0x040006B2 RID: 1714
		private OverlayPopupWidget _popupWidget;

		// Token: 0x040006B3 RID: 1715
		private bool _parentKnowsPopup = true;

		// Token: 0x040006B4 RID: 1716
		private bool _isMergedWithArmy = true;

		// Token: 0x040006B5 RID: 1717
		private bool _isPartyItem;
	}
}
