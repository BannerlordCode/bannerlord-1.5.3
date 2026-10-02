using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AB RID: 171
	public class MPPerkVM : ViewModel
	{
		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0003349A File Offset: 0x0003169A
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x000334A2 File Offset: 0x000316A2
		public int PerkIndex { get; private set; }

		// Token: 0x0600107C RID: 4220 RVA: 0x000334AB File Offset: 0x000316AB
		public MPPerkVM(Action<MPPerkVM> onSelectPerk, IReadOnlyPerkObject perk, bool isSelectable, int perkIndex)
		{
			this.Perk = perk;
			this.PerkIndex = perkIndex;
			this._onSelectPerk = onSelectPerk;
			this.IconType = perk.IconId;
			this.IsSelectable = isSelectable;
			this.RefreshValues();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x000334E4 File Offset: 0x000316E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Perk.Name.ToString();
			this.Description = this.Perk.Description.ToString();
			GameTexts.SetVariable("newline", "\n");
			this.Hint = new HintViewModel(this.Perk.Description, null);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00033549 File Offset: 0x00031749
		public void ExecuteSelectPerk()
		{
			this._onSelectPerk(this);
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x00033557 File Offset: 0x00031757
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x0003355F File Offset: 0x0003175F
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

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x00033582 File Offset: 0x00031782
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x0003358A File Offset: 0x0003178A
		[DataSourceProperty]
		public HintViewModel Hint
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
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x000335A8 File Offset: 0x000317A8
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x000335B0 File Offset: 0x000317B0
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x000335D3 File Offset: 0x000317D3
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x000335DB File Offset: 0x000317DB
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

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x000335FE File Offset: 0x000317FE
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x00033606 File Offset: 0x00031806
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x040007BC RID: 1980
		public readonly IReadOnlyPerkObject Perk;

		// Token: 0x040007BD RID: 1981
		private readonly Action<MPPerkVM> _onSelectPerk;

		// Token: 0x040007BF RID: 1983
		private string _iconType;

		// Token: 0x040007C0 RID: 1984
		private string _name;

		// Token: 0x040007C1 RID: 1985
		private string _description;

		// Token: 0x040007C2 RID: 1986
		private bool _isSelectable;

		// Token: 0x040007C3 RID: 1987
		private HintViewModel _hint;
	}
}
