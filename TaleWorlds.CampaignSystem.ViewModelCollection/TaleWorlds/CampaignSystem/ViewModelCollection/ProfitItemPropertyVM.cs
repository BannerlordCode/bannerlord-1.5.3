using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001C RID: 28
	public class ProfitItemPropertyVM : ViewModel
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000C882 File Offset: 0x0000AA82
		public ProfitItemPropertyVM(string name, int value, ProfitItemPropertyVM.PropertyType type = ProfitItemPropertyVM.PropertyType.None, CharacterImageIdentifierVM governorVisual = null, BasicTooltipViewModel hint = null)
		{
			this.Name = name;
			this.Value = value;
			this.Type = (int)type;
			this.GovernorVisual = governorVisual;
			this.Hint = hint;
			this.RefreshValues();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C8B5 File Offset: 0x0000AAB5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ColonText = GameTexts.FindText("str_colon", null).ToString();
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000C8D3 File Offset: 0x0000AAD3
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000C8DB File Offset: 0x0000AADB
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					this.ShowGovernorPortrait = this._type == 5;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000C908 File Offset: 0x0000AB08
		// (set) Token: 0x060001BA RID: 442 RVA: 0x0000C910 File Offset: 0x0000AB10
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000C933 File Offset: 0x0000AB33
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000C93B File Offset: 0x0000AB3B
		[DataSourceProperty]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					this.ValueString = this._value.ToString("+0;-#");
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000C96F File Offset: 0x0000AB6F
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000C977 File Offset: 0x0000AB77
		[DataSourceProperty]
		public string ValueString
		{
			get
			{
				return this._valueString;
			}
			private set
			{
				if (value != this._valueString)
				{
					this._valueString = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueString");
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000C99A File Offset: 0x0000AB9A
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000C9A2 File Offset: 0x0000ABA2
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		[DataSourceProperty]
		public string ColonText
		{
			get
			{
				return this._colonText;
			}
			set
			{
				if (value != this._colonText)
				{
					this._colonText = value;
					base.OnPropertyChangedWithValue<string>(value, "ColonText");
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C9EB File Offset: 0x0000ABEB
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
		[DataSourceProperty]
		public CharacterImageIdentifierVM GovernorVisual
		{
			get
			{
				return this._governorVisual;
			}
			set
			{
				if (value != this._governorVisual)
				{
					this._governorVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "GovernorVisual");
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000CA11 File Offset: 0x0000AC11
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0000CA19 File Offset: 0x0000AC19
		[DataSourceProperty]
		public bool ShowGovernorPortrait
		{
			get
			{
				return this._showGovernorPortrait;
			}
			private set
			{
				if (value != this._showGovernorPortrait)
				{
					this._showGovernorPortrait = value;
					base.OnPropertyChangedWithValue(value, "ShowGovernorPortrait");
				}
			}
		}

		// Token: 0x040000CC RID: 204
		private int _type;

		// Token: 0x040000CD RID: 205
		private string _name;

		// Token: 0x040000CE RID: 206
		private int _value;

		// Token: 0x040000CF RID: 207
		private string _valueString;

		// Token: 0x040000D0 RID: 208
		private BasicTooltipViewModel _hint;

		// Token: 0x040000D1 RID: 209
		private string _colonText;

		// Token: 0x040000D2 RID: 210
		private CharacterImageIdentifierVM _governorVisual;

		// Token: 0x040000D3 RID: 211
		private bool _showGovernorPortrait;

		// Token: 0x0200017F RID: 383
		public enum PropertyType
		{
			// Token: 0x04001065 RID: 4197
			None,
			// Token: 0x04001066 RID: 4198
			Tax,
			// Token: 0x04001067 RID: 4199
			Tariff,
			// Token: 0x04001068 RID: 4200
			Garrison,
			// Token: 0x04001069 RID: 4201
			Village,
			// Token: 0x0400106A RID: 4202
			Governor
		}
	}
}
