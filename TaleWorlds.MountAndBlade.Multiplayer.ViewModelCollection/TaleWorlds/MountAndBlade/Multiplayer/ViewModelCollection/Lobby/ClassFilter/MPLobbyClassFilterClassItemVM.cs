using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter
{
	// Token: 0x02000064 RID: 100
	public class MPLobbyClassFilterClassItemVM : ViewModel
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0001E80F File Offset: 0x0001CA0F
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0001E817 File Offset: 0x0001CA17
		public MultiplayerClassDivisions.MPHeroClass HeroClass { get; private set; }

		// Token: 0x060009C0 RID: 2496 RVA: 0x0001E820 File Offset: 0x0001CA20
		public MPLobbyClassFilterClassItemVM(BasicCultureObject culture, MultiplayerClassDivisions.MPHeroClass heroClass, Action<MPLobbyClassFilterClassItemVM> onSelect)
		{
			this.HeroClass = heroClass;
			this._onSelect = onSelect;
			this.CultureColor = Color.FromUint(culture.Color);
			this.IconType = this.HeroClass.IconType.ToString();
			this.RefreshValues();
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0001E877 File Offset: 0x0001CA77
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroClass.HeroName.ToString();
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0001E895 File Offset: 0x0001CA95
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroClass = null;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0001E8A4 File Offset: 0x0001CAA4
		private void ExecuteSelect()
		{
			if (this._onSelect != null)
			{
				this._onSelect(this);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0001E8BA File Offset: 0x0001CABA
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0001E8C2 File Offset: 0x0001CAC2
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

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0001E8E0 File Offset: 0x0001CAE0
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0001E8E8 File Offset: 0x0001CAE8
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

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0001E906 File Offset: 0x0001CB06
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0001E90E File Offset: 0x0001CB0E
		[DataSourceProperty]
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChangedWithValue(value, "CultureColor");
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0001E931 File Offset: 0x0001CB31
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0001E939 File Offset: 0x0001CB39
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

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0001E95C File Offset: 0x0001CB5C
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0001E964 File Offset: 0x0001CB64
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x0400047A RID: 1146
		private Action<MPLobbyClassFilterClassItemVM> _onSelect;

		// Token: 0x0400047C RID: 1148
		private bool _isEnabled;

		// Token: 0x0400047D RID: 1149
		private bool _isSelected;

		// Token: 0x0400047E RID: 1150
		private Color _cultureColor;

		// Token: 0x0400047F RID: 1151
		private string _name;

		// Token: 0x04000480 RID: 1152
		private string _iconType;
	}
}
