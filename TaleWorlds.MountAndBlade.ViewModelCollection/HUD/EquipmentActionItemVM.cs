using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000057 RID: 87
	public class EquipmentActionItemVM : ViewModel
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x00019C2C File Offset: 0x00017E2C
		public EquipmentActionItemVM(string item, string itemTypeAsString, object identifier, Action<EquipmentActionItemVM> onSelection, bool isCurrentlyWielded = false)
		{
			this.Identifier = identifier;
			this.ActionText = item;
			this.TypeAsString = itemTypeAsString;
			this.IsWielded = isCurrentlyWielded;
			this._onSelection = onSelection;
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00019C59 File Offset: 0x00017E59
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x00019C61 File Offset: 0x00017E61
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00019C84 File Offset: 0x00017E84
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x00019C8C File Offset: 0x00017E8C
		[DataSourceProperty]
		public bool IsWielded
		{
			get
			{
				return this._isWielded;
			}
			set
			{
				if (value != this._isWielded)
				{
					this._isWielded = value;
					base.OnPropertyChangedWithValue(value, "IsWielded");
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x00019CAA File Offset: 0x00017EAA
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x00019CB2 File Offset: 0x00017EB2
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
					if (value)
					{
						this._onSelection(this);
					}
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00019CDF File Offset: 0x00017EDF
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x00019CE7 File Offset: 0x00017EE7
		[DataSourceProperty]
		public string TypeAsString
		{
			get
			{
				return this._typeAsString;
			}
			set
			{
				if (value != this._typeAsString)
				{
					this._typeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeAsString");
				}
			}
		}

		// Token: 0x04000326 RID: 806
		private readonly Action<EquipmentActionItemVM> _onSelection;

		// Token: 0x04000327 RID: 807
		public object Identifier;

		// Token: 0x04000328 RID: 808
		private string _actionText;

		// Token: 0x04000329 RID: 809
		private string _typeAsString;

		// Token: 0x0400032A RID: 810
		private bool _isSelected;

		// Token: 0x0400032B RID: 811
		private bool _isWielded;
	}
}
