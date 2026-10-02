using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000D3 RID: 211
	public class EncyclopediaSettlementPageStatItemVM : ViewModel
	{
		// Token: 0x060013AF RID: 5039 RVA: 0x0004F73D File Offset: 0x0004D93D
		public EncyclopediaSettlementPageStatItemVM(BasicTooltipViewModel basicTooltipViewModel, EncyclopediaSettlementPageStatItemVM.DescriptionType type, string statText)
		{
			this._basicTooltipViewModel = basicTooltipViewModel;
			this._typeString = type.ToString();
			this._statText = statText;
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0004F766 File Offset: 0x0004D966
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x0004F76E File Offset: 0x0004D96E
		[DataSourceProperty]
		public BasicTooltipViewModel BasicTooltipViewModel
		{
			get
			{
				return this._basicTooltipViewModel;
			}
			set
			{
				if (value != this._basicTooltipViewModel)
				{
					this._basicTooltipViewModel = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "BasicTooltipViewModel");
				}
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0004F78C File Offset: 0x0004D98C
		// (set) Token: 0x060013B3 RID: 5043 RVA: 0x0004F794 File Offset: 0x0004D994
		[DataSourceProperty]
		public string TypeString
		{
			get
			{
				return this._typeString;
			}
			set
			{
				if (value != this._typeString)
				{
					this._typeString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeString");
				}
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0004F7B7 File Offset: 0x0004D9B7
		// (set) Token: 0x060013B5 RID: 5045 RVA: 0x0004F7BF File Offset: 0x0004D9BF
		[DataSourceProperty]
		public string StatText
		{
			get
			{
				return this._statText;
			}
			set
			{
				if (value != this._statText)
				{
					this._statText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatText");
				}
			}
		}

		// Token: 0x040008F5 RID: 2293
		private BasicTooltipViewModel _basicTooltipViewModel;

		// Token: 0x040008F6 RID: 2294
		private string _typeString;

		// Token: 0x040008F7 RID: 2295
		private string _statText;

		// Token: 0x02000247 RID: 583
		public enum DescriptionType
		{
			// Token: 0x0400128E RID: 4750
			Wall,
			// Token: 0x0400128F RID: 4751
			Shipyard,
			// Token: 0x04001290 RID: 4752
			Garrison,
			// Token: 0x04001291 RID: 4753
			Militia,
			// Token: 0x04001292 RID: 4754
			Food,
			// Token: 0x04001293 RID: 4755
			Prosperity,
			// Token: 0x04001294 RID: 4756
			Loyalty,
			// Token: 0x04001295 RID: 4757
			Security
		}
	}
}
