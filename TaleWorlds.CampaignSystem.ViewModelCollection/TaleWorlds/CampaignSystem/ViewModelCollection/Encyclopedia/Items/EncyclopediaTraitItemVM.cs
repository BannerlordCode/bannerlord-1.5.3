using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000F2 RID: 242
	public class EncyclopediaTraitItemVM : ViewModel
	{
		// Token: 0x060015FB RID: 5627 RVA: 0x0005670B File Offset: 0x0005490B
		public EncyclopediaTraitItemVM(TraitObject traitObj, int value)
		{
			this._traitObj = traitObj;
			this.TraitId = traitObj.StringId;
			this.Value = value;
			this.Hint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTraitEffectTooltip(this._traitObj, this.Value));
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00056744 File Offset: 0x00054944
		public EncyclopediaTraitItemVM(TraitObject traitObj, Hero hero)
			: this(traitObj, hero.GetTraitLevel(traitObj))
		{
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00056754 File Offset: 0x00054954
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x0005675C File Offset: 0x0005495C
		[DataSourceProperty]
		public string TraitId
		{
			get
			{
				return this._traitId;
			}
			set
			{
				if (value != this._traitId)
				{
					this._traitId = value;
					base.OnPropertyChangedWithValue<string>(value, "TraitId");
				}
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x0005677F File Offset: 0x0005497F
		// (set) Token: 0x06001600 RID: 5632 RVA: 0x00056787 File Offset: 0x00054987
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

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x000567A5 File Offset: 0x000549A5
		// (set) Token: 0x06001602 RID: 5634 RVA: 0x000567AD File Offset: 0x000549AD
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

		// Token: 0x040009F2 RID: 2546
		private readonly TraitObject _traitObj;

		// Token: 0x040009F3 RID: 2547
		private string _traitId;

		// Token: 0x040009F4 RID: 2548
		private int _value;

		// Token: 0x040009F5 RID: 2549
		private BasicTooltipViewModel _hint;
	}
}
