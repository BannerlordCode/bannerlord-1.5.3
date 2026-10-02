using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006F RID: 111
	public abstract class KeyOptionVM : ViewModel
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0001D377 File Offset: 0x0001B577
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x0001D37F File Offset: 0x0001B57F
		public Key CurrentKey
		{
			get
			{
				return this._currentKey;
			}
			protected set
			{
				this._currentKey = value;
				this.UpdateIsChanged();
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001D38E File Offset: 0x0001B58E
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x0001D396 File Offset: 0x0001B596
		public Key Key
		{
			get
			{
				return this._key;
			}
			protected set
			{
				this._key = value;
				this.UpdateIsChanged();
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001D3A5 File Offset: 0x0001B5A5
		public KeyOptionVM(string groupId, string id, Action<KeyOptionVM> onKeybindRequest)
		{
			this._groupId = groupId;
			this._id = id;
			this._onKeybindRequest = onKeybindRequest;
			this.RevertHint = new HintViewModel(new TextObject("{=ftM2TjQ5}Revert changes", null), null);
		}

		// Token: 0x060008B2 RID: 2226
		public abstract void Set(InputKey newKey);

		// Token: 0x060008B3 RID: 2227
		public abstract void Update();

		// Token: 0x060008B4 RID: 2228
		public abstract void OnDone();

		// Token: 0x060008B5 RID: 2229
		public abstract void ExecuteRevert();

		// Token: 0x060008B6 RID: 2230
		internal abstract void UpdateIsChanged();

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0001D3D9 File Offset: 0x0001B5D9
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0001D3E1 File Offset: 0x0001B5E1
		[DataSourceProperty]
		public string OptionValueText
		{
			get
			{
				return this._optionValueText;
			}
			set
			{
				if (value != this._optionValueText)
				{
					this._optionValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionValueText");
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0001D404 File Offset: 0x0001B604
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x0001D40C File Offset: 0x0001B60C
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

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0001D42F File Offset: 0x0001B62F
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0001D437 File Offset: 0x0001B637
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

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001D45A File Offset: 0x0001B65A
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0001D462 File Offset: 0x0001B662
		[DataSourceProperty]
		public bool IsChanged
		{
			get
			{
				return this._isChanged;
			}
			set
			{
				if (value != this._isChanged)
				{
					this._isChanged = value;
					base.OnPropertyChangedWithValue(value, "IsChanged");
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0001D480 File Offset: 0x0001B680
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0001D488 File Offset: 0x0001B688
		[DataSourceProperty]
		public HintViewModel RevertHint
		{
			get
			{
				return this._revertHint;
			}
			set
			{
				if (value != this._revertHint)
				{
					this._revertHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RevertHint");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0001D4A6 File Offset: 0x0001B6A6
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x0001D4AE File Offset: 0x0001B6AE
		[DataSourceProperty]
		public string ExtraInformationText
		{
			get
			{
				return this._extraInformationText;
			}
			set
			{
				if (value != this._extraInformationText)
				{
					this._extraInformationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExtraInformationText");
				}
			}
		}

		// Token: 0x040003E7 RID: 999
		private Key _currentKey;

		// Token: 0x040003E8 RID: 1000
		private Key _key;

		// Token: 0x040003E9 RID: 1001
		protected readonly string _groupId;

		// Token: 0x040003EA RID: 1002
		protected readonly string _id;

		// Token: 0x040003EB RID: 1003
		protected readonly Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x040003EC RID: 1004
		private string _optionValueText;

		// Token: 0x040003ED RID: 1005
		private string _name;

		// Token: 0x040003EE RID: 1006
		private string _description;

		// Token: 0x040003EF RID: 1007
		private string _extraInformationText;

		// Token: 0x040003F0 RID: 1008
		private bool _isChanged;

		// Token: 0x040003F1 RID: 1009
		private HintViewModel _revertHint;
	}
}
