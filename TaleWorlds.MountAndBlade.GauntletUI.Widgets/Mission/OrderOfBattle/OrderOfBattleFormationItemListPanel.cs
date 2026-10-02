using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EF RID: 239
	public class OrderOfBattleFormationItemListPanel : ListPanel
	{
		// Token: 0x06000C59 RID: 3161 RVA: 0x00021F25 File Offset: 0x00020125
		public OrderOfBattleFormationItemListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00021F2E File Offset: 0x0002012E
		private void OnStateChanged()
		{
			if (this.IsSelected)
			{
				Widget cardWidget = this.CardWidget;
				if (cardWidget == null)
				{
					return;
				}
				cardWidget.SetState("Selected");
				return;
			}
			else
			{
				Widget cardWidget2 = this.CardWidget;
				if (cardWidget2 == null)
				{
					return;
				}
				cardWidget2.SetState("Default");
				return;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00021F63 File Offset: 0x00020163
		private void OnClassDropdownEnabledStateChanged(DropdownWidget widget)
		{
			this.IsClassDropdownEnabled = widget.IsOpen;
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00021F71 File Offset: 0x00020171
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x00021F79 File Offset: 0x00020179
		[Editor(false)]
		public Widget CardWidget
		{
			get
			{
				return this._cardWidget;
			}
			set
			{
				if (value != this._cardWidget)
				{
					this._cardWidget = value;
					base.OnPropertyChanged<Widget>(value, "CardWidget");
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00021F97 File Offset: 0x00020197
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x00021FA0 File Offset: 0x000201A0
		[Editor(false)]
		public DropdownWidget FormationClassDropdown
		{
			get
			{
				return this._formationClassDropdown;
			}
			set
			{
				if (value != this._formationClassDropdown)
				{
					if (this._formationClassDropdown != null)
					{
						DropdownWidget formationClassDropdown = this._formationClassDropdown;
						formationClassDropdown.OnOpenStateChanged = (Action<DropdownWidget>)Delegate.Remove(formationClassDropdown.OnOpenStateChanged, new Action<DropdownWidget>(this.OnClassDropdownEnabledStateChanged));
					}
					this._formationClassDropdown = value;
					base.OnPropertyChanged<DropdownWidget>(value, "FormationClassDropdown");
					if (this._formationClassDropdown != null)
					{
						DropdownWidget formationClassDropdown2 = this._formationClassDropdown;
						formationClassDropdown2.OnOpenStateChanged = (Action<DropdownWidget>)Delegate.Combine(formationClassDropdown2.OnOpenStateChanged, new Action<DropdownWidget>(this.OnClassDropdownEnabledStateChanged));
						this.OnClassDropdownEnabledStateChanged(this._formationClassDropdown);
					}
				}
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00022033 File Offset: 0x00020233
		// (set) Token: 0x06000C61 RID: 3169 RVA: 0x0002203C File Offset: 0x0002023C
		[Editor(false)]
		public bool IsControlledByPlayer
		{
			get
			{
				return this._isControlledByPlayer;
			}
			set
			{
				if (value != this._isControlledByPlayer)
				{
					this._isControlledByPlayer = value;
					base.OnPropertyChanged(value, "IsControlledByPlayer");
					DropdownWidget formationClassDropdown = this.FormationClassDropdown;
					if (((formationClassDropdown != null) ? formationClassDropdown.Button : null) != null)
					{
						this.FormationClassDropdown.Button.IsEnabled = value;
					}
				}
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0002208A File Offset: 0x0002028A
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x00022092 File Offset: 0x00020292
		[Editor(false)]
		public bool IsClassDropdownEnabled
		{
			get
			{
				return this._isClassDropdownEnabled;
			}
			set
			{
				if (value != this._isClassDropdownEnabled)
				{
					this._isClassDropdownEnabled = value;
					base.OnPropertyChanged(value, "IsClassDropdownEnabled");
					if (this.FormationClassDropdown != null)
					{
						this.FormationClassDropdown.IsOpen = value;
					}
				}
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x000220C4 File Offset: 0x000202C4
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x000220CC File Offset: 0x000202CC
		[Editor(false)]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged(value, "IsSelected");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x000220F0 File Offset: 0x000202F0
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x000220F8 File Offset: 0x000202F8
		[Editor(false)]
		public bool HasFormation
		{
			get
			{
				return this._hasFormation;
			}
			set
			{
				if (value != this._hasFormation)
				{
					this._hasFormation = value;
					base.OnPropertyChanged(value, "HasFormation");
				}
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00022116 File Offset: 0x00020316
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x0002211E File Offset: 0x0002031E
		[Editor(false)]
		public float DefaultFocusYOffsetFromCenter
		{
			get
			{
				return this._defaultFocusYOffsetFromCenter;
			}
			set
			{
				if (value != this._defaultFocusYOffsetFromCenter)
				{
					this._defaultFocusYOffsetFromCenter = value;
					base.OnPropertyChanged(value, "DefaultFocusYOffsetFromCenter");
				}
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0002213C File Offset: 0x0002033C
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x00022144 File Offset: 0x00020344
		[Editor(false)]
		public float NoFormationFocusYOffsetFromCenter
		{
			get
			{
				return this._noFormationFocusYOffsetFromCenter;
			}
			set
			{
				if (value != this._noFormationFocusYOffsetFromCenter)
				{
					this._noFormationFocusYOffsetFromCenter = value;
					base.OnPropertyChanged(value, "NoFormationFocusYOffsetFromCenter");
				}
			}
		}

		// Token: 0x04000598 RID: 1432
		private Widget _cardWidget;

		// Token: 0x04000599 RID: 1433
		private DropdownWidget _formationClassDropdown;

		// Token: 0x0400059A RID: 1434
		private bool _isControlledByPlayer;

		// Token: 0x0400059B RID: 1435
		private bool _isClassDropdownEnabled;

		// Token: 0x0400059C RID: 1436
		private bool _isSelected;

		// Token: 0x0400059D RID: 1437
		private bool _hasFormation;

		// Token: 0x0400059E RID: 1438
		private float _defaultFocusYOffsetFromCenter;

		// Token: 0x0400059F RID: 1439
		private float _noFormationFocusYOffsetFromCenter;
	}
}
