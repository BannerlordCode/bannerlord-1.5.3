using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001D RID: 29
	public class SelectableFiefItemPropertyVM : SelectableItemPropertyVM
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x0000CA37 File Offset: 0x0000AC37
		public SelectableFiefItemPropertyVM(string name, string value, int changeAmount, SelectableItemPropertyVM.PropertyType type, BasicTooltipViewModel hint = null, bool isWarning = false)
			: base(name, value, isWarning, hint)
		{
			this.ChangeAmount = changeAmount;
			base.Type = (int)type;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000CA54 File Offset: 0x0000AC54
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		[DataSourceProperty]
		public int ChangeAmount
		{
			get
			{
				return this._changeAmount;
			}
			set
			{
				if (value != this._changeAmount)
				{
					this._changeAmount = value;
					base.OnPropertyChangedWithValue(value, "ChangeAmount");
				}
			}
		}

		// Token: 0x040000D4 RID: 212
		private int _changeAmount;
	}
}
