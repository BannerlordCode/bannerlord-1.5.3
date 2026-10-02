using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x02000049 RID: 73
	public abstract class GenericHostGameOptionDataVM : ViewModel
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x000154E2 File Offset: 0x000136E2
		public MultiplayerOptions.OptionType OptionType { get; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x000154EA File Offset: 0x000136EA
		public int PreferredIndex { get; }

		// Token: 0x0600068F RID: 1679 RVA: 0x000154F2 File Offset: 0x000136F2
		internal GenericHostGameOptionDataVM(OptionsVM.OptionsDataType type, MultiplayerOptions.OptionType optionType, int preferredIndex)
		{
			this.Category = (int)type;
			this.OptionType = optionType;
			this.PreferredIndex = preferredIndex;
			this.Index = preferredIndex;
			this.IsEnabled = true;
			this.RefreshValues();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00015524 File Offset: 0x00013724
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("str_multiplayer_option", this.OptionType.ToString()).ToString();
		}

		// Token: 0x06000691 RID: 1681
		public abstract void RefreshData();

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00015560 File Offset: 0x00013760
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x00015568 File Offset: 0x00013768
		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (value != this._index)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00015586 File Offset: 0x00013786
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0001558E File Offset: 0x0001378E
		[DataSourceProperty]
		public int Category
		{
			get
			{
				return this._category;
			}
			set
			{
				if (value != this._category)
				{
					this._category = value;
					base.OnPropertyChangedWithValue(value, "Category");
				}
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x000155AC File Offset: 0x000137AC
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x000155B4 File Offset: 0x000137B4
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

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000155D7 File Offset: 0x000137D7
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000155DF File Offset: 0x000137DF
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x0400031A RID: 794
		private int _index;

		// Token: 0x0400031B RID: 795
		private int _category;

		// Token: 0x0400031C RID: 796
		private string _name;

		// Token: 0x0400031D RID: 797
		private bool _isEnabled;
	}
}
