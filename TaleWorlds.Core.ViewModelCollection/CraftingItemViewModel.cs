using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection
{
	// Token: 0x0200000A RID: 10
	public class CraftingItemViewModel : ViewModel
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000029DD File Offset: 0x00000BDD
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000029E5 File Offset: 0x00000BE5
		[DataSourceProperty]
		public string UsedPieces
		{
			get
			{
				return this._usedPieces;
			}
			set
			{
				this._usedPieces = value;
				base.OnPropertyChangedWithValue<string>(value, "UsedPieces");
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000029FA File Offset: 0x00000BFA
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002A02 File Offset: 0x00000C02
		[DataSourceProperty]
		public int WeaponClass
		{
			get
			{
				return this._weaponClass;
			}
			set
			{
				if (value != this._weaponClass)
				{
					this._weaponClass = value;
					base.OnPropertyChangedWithValue(value, "WeaponClass");
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002A20 File Offset: 0x00000C20
		public WeaponClass GetWeaponClass()
		{
			return (WeaponClass)this.WeaponClass;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002A28 File Offset: 0x00000C28
		public void SetCraftingData(WeaponClass weaponClass, WeaponDesignElement[] craftingPieces)
		{
			this.WeaponClass = (int)weaponClass;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002A31 File Offset: 0x00000C31
		public CraftingItemViewModel()
		{
			this.WeaponClass = -1;
		}

		// Token: 0x04000022 RID: 34
		private string _usedPieces;

		// Token: 0x04000023 RID: 35
		private int _weaponClass;
	}
}
