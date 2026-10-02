using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame
{
	// Token: 0x02000043 RID: 67
	public class MPMatchmakingItemVM : ViewModel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000650 RID: 1616 RVA: 0x000148F4 File Offset: 0x00012AF4
		// (remove) Token: 0x06000651 RID: 1617 RVA: 0x0001492C File Offset: 0x00012B2C
		public event Action<MPMatchmakingItemVM, bool> OnSelectionChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000652 RID: 1618 RVA: 0x00014964 File Offset: 0x00012B64
		// (remove) Token: 0x06000653 RID: 1619 RVA: 0x0001499C File Offset: 0x00012B9C
		public event Action<MPMatchmakingItemVM> OnSetFocusItem;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000654 RID: 1620 RVA: 0x000149D4 File Offset: 0x00012BD4
		// (remove) Token: 0x06000655 RID: 1621 RVA: 0x00014A0C File Offset: 0x00012C0C
		public event Action OnRemoveFocus;

		// Token: 0x06000656 RID: 1622 RVA: 0x00014A41 File Offset: 0x00012C41
		public MPMatchmakingItemVM(MultiplayerGameType type)
		{
			this.Type = type.ToString();
			this.IsAvailable = true;
			this.IsSelected = this.IsAvailable;
			this.RefreshValues();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00014A75 File Offset: 0x00012C75
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("str_multiplayer_official_game_type_name", this.Type).ToString();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00014A98 File Offset: 0x00012C98
		private void ExecuteSetFocusItem()
		{
			Action<MPMatchmakingItemVM> onSetFocusItem = this.OnSetFocusItem;
			if (onSetFocusItem == null)
			{
				return;
			}
			onSetFocusItem(this);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00014AAB File Offset: 0x00012CAB
		private void ExecuteRemoveFocus()
		{
			Action onRemoveFocus = this.OnRemoveFocus;
			if (onRemoveFocus == null)
			{
				return;
			}
			onRemoveFocus();
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00014ABD File Offset: 0x00012CBD
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00014AC5 File Offset: 0x00012CC5
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00014AE8 File Offset: 0x00012CE8
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00014AF0 File Offset: 0x00012CF0
		[DataSourceProperty]
		public string Type
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
					base.OnPropertyChangedWithValue<string>(value, "Type");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00014B13 File Offset: 0x00012D13
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00014B1B File Offset: 0x00012D1B
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
					Action<MPMatchmakingItemVM, bool> onSelectionChanged = this.OnSelectionChanged;
					if (onSelectionChanged == null)
					{
						return;
					}
					onSelectionChanged(this, this._isSelected);
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00014B50 File Offset: 0x00012D50
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x00014B58 File Offset: 0x00012D58
		[DataSourceProperty]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAvailable");
				}
			}
		}

		// Token: 0x040002FD RID: 765
		private string _name;

		// Token: 0x040002FE RID: 766
		private string _type;

		// Token: 0x040002FF RID: 767
		private bool _isSelected;

		// Token: 0x04000300 RID: 768
		private bool _isAvailable;
	}
}
