using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TroopSelection
{
	// Token: 0x020000A6 RID: 166
	public class TroopSelectionItemVM : ViewModel
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00041A3E File Offset: 0x0003FC3E
		// (set) Token: 0x06000FCA RID: 4042 RVA: 0x00041A46 File Offset: 0x0003FC46
		public TroopRosterElement Troop { get; private set; }

		// Token: 0x06000FCB RID: 4043 RVA: 0x00041A50 File Offset: 0x0003FC50
		public TroopSelectionItemVM(TroopRosterElement troop, Action<TroopSelectionItemVM> onAdd, Action<TroopSelectionItemVM> onRemove)
		{
			this._onAdd = onAdd;
			this._onRemove = onRemove;
			this.Troop = troop;
			this.MaxAmount = this.Troop.Number - this.Troop.WoundedNumber;
			this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(troop.Character, false));
			this.Name = troop.Character.Name.ToString();
			this.TierIconData = CampaignUIHelper.GetCharacterTierData(this.Troop.Character, false);
			this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(this.Troop.Character, false);
			this.IsTroopHero = this.Troop.Character.IsHero;
			this.HeroHealthPercent = (this.Troop.Character.IsHero ? MathF.Ceiling((float)this.Troop.Character.HeroObject.HitPoints / (float)this.Troop.Character.MaxHitPoints() * 100f) : 0);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00041B5A File Offset: 0x0003FD5A
		public void ExecuteAdd()
		{
			Action<TroopSelectionItemVM> onAdd = this._onAdd;
			if (onAdd == null)
			{
				return;
			}
			onAdd.DynamicInvokeWithLog(new object[] { this });
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00041B77 File Offset: 0x0003FD77
		public void ExecuteRemove()
		{
			Action<TroopSelectionItemVM> onRemove = this._onRemove;
			if (onRemove == null)
			{
				return;
			}
			onRemove.DynamicInvokeWithLog(new object[] { this });
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00041B94 File Offset: 0x0003FD94
		private void UpdateAmountText()
		{
			GameTexts.SetVariable("LEFT", this.CurrentAmount);
			GameTexts.SetVariable("RIGHT", this.MaxAmount);
			this.AmountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00041BCC File Offset: 0x0003FDCC
		public void ExecuteLink()
		{
			if (this.Troop.Character != null)
			{
				EncyclopediaManager encyclopediaManager = Campaign.Current.EncyclopediaManager;
				Hero heroObject = this.Troop.Character.HeroObject;
				encyclopediaManager.GoToLink(((heroObject != null) ? heroObject.EncyclopediaLink : null) ?? this.Troop.Character.EncyclopediaLink);
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00041C25 File Offset: 0x0003FE25
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x00041C2D File Offset: 0x0003FE2D
		[DataSourceProperty]
		public int MaxAmount
		{
			get
			{
				return this._maxAmount;
			}
			set
			{
				if (value != this._maxAmount)
				{
					this._maxAmount = value;
					base.OnPropertyChangedWithValue(value, "MaxAmount");
					this.UpdateAmountText();
				}
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x00041C51 File Offset: 0x0003FE51
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x00041C59 File Offset: 0x0003FE59
		[DataSourceProperty]
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
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x00041C77 File Offset: 0x0003FE77
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x00041C7F File Offset: 0x0003FE7F
		[DataSourceProperty]
		public bool IsRosterFull
		{
			get
			{
				return this._isRosterFull;
			}
			set
			{
				if (value != this._isRosterFull)
				{
					this._isRosterFull = value;
					base.OnPropertyChangedWithValue(value, "IsRosterFull");
				}
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00041C9D File Offset: 0x0003FE9D
		// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x00041CA5 File Offset: 0x0003FEA5
		[DataSourceProperty]
		public bool IsTroopHero
		{
			get
			{
				return this._isTroopHero;
			}
			set
			{
				if (value != this._isTroopHero)
				{
					this._isTroopHero = value;
					base.OnPropertyChangedWithValue(value, "IsTroopHero");
				}
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x00041CC3 File Offset: 0x0003FEC3
		// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x00041CCB File Offset: 0x0003FECB
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
				}
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x00041CE9 File Offset: 0x0003FEE9
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x00041CF1 File Offset: 0x0003FEF1
		[DataSourceProperty]
		public int CurrentAmount
		{
			get
			{
				return this._currentAmount;
			}
			set
			{
				if (value != this._currentAmount)
				{
					this._currentAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentAmount");
					this.IsSelected = value > 0;
					this.UpdateAmountText();
				}
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00041D1F File Offset: 0x0003FF1F
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x00041D27 File Offset: 0x0003FF27
		[DataSourceProperty]
		public int HeroHealthPercent
		{
			get
			{
				return this._heroHealthPercent;
			}
			set
			{
				if (value != this._heroHealthPercent)
				{
					this._heroHealthPercent = value;
					base.OnPropertyChangedWithValue(value, "HeroHealthPercent");
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x00041D45 File Offset: 0x0003FF45
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x00041D4D File Offset: 0x0003FF4D
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00041D70 File Offset: 0x0003FF70
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x00041D78 File Offset: 0x0003FF78
		[DataSourceProperty]
		public string AmountText
		{
			get
			{
				return this._amountText;
			}
			set
			{
				if (value != this._amountText)
				{
					this._amountText = value;
					base.OnPropertyChangedWithValue<string>(value, "AmountText");
				}
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x00041D9B File Offset: 0x0003FF9B
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x00041DA3 File Offset: 0x0003FFA3
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00041DC1 File Offset: 0x0003FFC1
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x00041DC9 File Offset: 0x0003FFC9
		[DataSourceProperty]
		public StringItemWithHintVM TierIconData
		{
			get
			{
				return this._tierIconData;
			}
			set
			{
				if (value != this._tierIconData)
				{
					this._tierIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TierIconData");
				}
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00041DE7 File Offset: 0x0003FFE7
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x00041DEF File Offset: 0x0003FFEF
		[DataSourceProperty]
		public StringItemWithHintVM TypeIconData
		{
			get
			{
				return this._typeIconData;
			}
			set
			{
				if (value != this._typeIconData)
				{
					this._typeIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TypeIconData");
				}
			}
		}

		// Token: 0x04000730 RID: 1840
		private readonly Action<TroopSelectionItemVM> _onAdd;

		// Token: 0x04000731 RID: 1841
		private readonly Action<TroopSelectionItemVM> _onRemove;

		// Token: 0x04000732 RID: 1842
		private int _currentAmount;

		// Token: 0x04000733 RID: 1843
		private int _maxAmount;

		// Token: 0x04000734 RID: 1844
		private int _heroHealthPercent;

		// Token: 0x04000735 RID: 1845
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000736 RID: 1846
		private bool _isSelected;

		// Token: 0x04000737 RID: 1847
		private bool _isRosterFull;

		// Token: 0x04000738 RID: 1848
		private bool _isLocked;

		// Token: 0x04000739 RID: 1849
		private bool _isTroopHero;

		// Token: 0x0400073A RID: 1850
		private string _name;

		// Token: 0x0400073B RID: 1851
		private string _amountText;

		// Token: 0x0400073C RID: 1852
		private StringItemWithHintVM _tierIconData;

		// Token: 0x0400073D RID: 1853
		private StringItemWithHintVM _typeIconData;
	}
}
