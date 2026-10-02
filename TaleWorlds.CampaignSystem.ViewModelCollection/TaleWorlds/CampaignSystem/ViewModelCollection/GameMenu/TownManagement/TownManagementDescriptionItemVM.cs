using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AE RID: 174
	public class TownManagementDescriptionItemVM : ViewModel
	{
		// Token: 0x06001065 RID: 4197 RVA: 0x0004334C File Offset: 0x0004154C
		public TownManagementDescriptionItemVM(TextObject title, int value, int valueChange, TownManagementDescriptionItemVM.DescriptionType type, BasicTooltipViewModel hint = null)
		{
			this._titleObj = title;
			this.Value = value;
			this.ValueChange = valueChange;
			this.Type = (int)type;
			this.Hint = hint ?? new BasicTooltipViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0004339A File Offset: 0x0004159A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = this._titleObj.ToString();
			this.RefreshIsWarning();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x000433BC File Offset: 0x000415BC
		private void RefreshIsWarning()
		{
			int type = this.Type;
			if (type == 1)
			{
				this.IsWarning = this.Value < 1;
				return;
			}
			if (type == 5)
			{
				this.IsWarning = this.Value < Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold;
				return;
			}
			if (type != 7)
			{
				this.IsWarning = false;
				return;
			}
			this.IsWarning = this.Value < 1;
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00043428 File Offset: 0x00041628
		// (set) Token: 0x06001069 RID: 4201 RVA: 0x00043430 File Offset: 0x00041630
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
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0004344E File Offset: 0x0004164E
		// (set) Token: 0x0600106B RID: 4203 RVA: 0x00043456 File Offset: 0x00041656
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x00043479 File Offset: 0x00041679
		// (set) Token: 0x0600106D RID: 4205 RVA: 0x00043481 File Offset: 0x00041681
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
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x0004349F File Offset: 0x0004169F
		// (set) Token: 0x0600106F RID: 4207 RVA: 0x000434A7 File Offset: 0x000416A7
		[DataSourceProperty]
		public int ValueChange
		{
			get
			{
				return this._valueChange;
			}
			set
			{
				if (value != this._valueChange)
				{
					this._valueChange = value;
					base.OnPropertyChangedWithValue(value, "ValueChange");
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x000434C5 File Offset: 0x000416C5
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x000434CD File Offset: 0x000416CD
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint && value != null)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x000434EE File Offset: 0x000416EE
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x000434F6 File Offset: 0x000416F6
		[DataSourceProperty]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChangedWithValue(value, "IsWarning");
				}
			}
		}

		// Token: 0x04000773 RID: 1907
		private readonly TextObject _titleObj;

		// Token: 0x04000774 RID: 1908
		private int _type = -1;

		// Token: 0x04000775 RID: 1909
		private string _title;

		// Token: 0x04000776 RID: 1910
		private int _value;

		// Token: 0x04000777 RID: 1911
		private int _valueChange;

		// Token: 0x04000778 RID: 1912
		private BasicTooltipViewModel _hint;

		// Token: 0x04000779 RID: 1913
		private bool _isWarning;

		// Token: 0x02000220 RID: 544
		public enum DescriptionType
		{
			// Token: 0x04001212 RID: 4626
			Gold,
			// Token: 0x04001213 RID: 4627
			Production,
			// Token: 0x04001214 RID: 4628
			Militia,
			// Token: 0x04001215 RID: 4629
			Prosperity,
			// Token: 0x04001216 RID: 4630
			Food,
			// Token: 0x04001217 RID: 4631
			Loyalty,
			// Token: 0x04001218 RID: 4632
			Security,
			// Token: 0x04001219 RID: 4633
			Garrison
		}
	}
}
