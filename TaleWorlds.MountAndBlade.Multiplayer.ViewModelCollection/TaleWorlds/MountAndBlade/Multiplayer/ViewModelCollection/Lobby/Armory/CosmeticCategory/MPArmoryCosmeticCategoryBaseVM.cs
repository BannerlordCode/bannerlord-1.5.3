using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory
{
	// Token: 0x02000083 RID: 131
	public abstract class MPArmoryCosmeticCategoryBaseVM : ViewModel
	{
		// Token: 0x06000D21 RID: 3361 RVA: 0x0002897C File Offset: 0x00026B7C
		public MPArmoryCosmeticCategoryBaseVM(CosmeticsManager.CosmeticType cosmeticType)
		{
			this.AvailableCosmetics = new MBBindingList<MPArmoryCosmeticItemBaseVM>();
			this.CosmeticType = cosmeticType;
			this.CosmeticTypeName = cosmeticType.ToString();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000289A9 File Offset: 0x00026BA9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x000289DB File Offset: 0x00026BDB
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.OnFinalize();
			});
		}

		// Token: 0x06000D24 RID: 3364
		protected abstract void ExecuteSelectCategory();

		// Token: 0x06000D25 RID: 3365 RVA: 0x00028A0D File Offset: 0x00026C0D
		public void Sort(MPArmoryCosmeticsVM.CosmeticItemComparer comparer)
		{
			this.AvailableCosmetics.Sort(comparer);
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00028A1B File Offset: 0x00026C1B
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x00028A23 File Offset: 0x00026C23
		[DataSourceProperty]
		public string CosmeticTypeName
		{
			get
			{
				return this._cosmeticTypeName;
			}
			set
			{
				if (value != this._cosmeticTypeName)
				{
					this._cosmeticTypeName = value;
					base.OnPropertyChangedWithValue<string>(value, "CosmeticTypeName");
				}
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00028A46 File Offset: 0x00026C46
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x00028A4E File Offset: 0x00026C4E
		[DataSourceProperty]
		public string CosmeticCategoryName
		{
			get
			{
				return this._cosmeticCategoryName;
			}
			set
			{
				if (value != this._cosmeticCategoryName)
				{
					this._cosmeticCategoryName = value;
					base.OnPropertyChangedWithValue<string>(value, "CosmeticCategoryName");
				}
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x00028A71 File Offset: 0x00026C71
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x00028A79 File Offset: 0x00026C79
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

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00028A97 File Offset: 0x00026C97
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x00028A9F File Offset: 0x00026C9F
		[DataSourceProperty]
		public MBBindingList<MPArmoryCosmeticItemBaseVM> AvailableCosmetics
		{
			get
			{
				return this._availableCosmetics;
			}
			set
			{
				if (value != this._availableCosmetics)
				{
					this._availableCosmetics = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPArmoryCosmeticItemBaseVM>>(value, "AvailableCosmetics");
				}
			}
		}

		// Token: 0x040005EF RID: 1519
		public readonly CosmeticsManager.CosmeticType CosmeticType;

		// Token: 0x040005F0 RID: 1520
		private string _cosmeticTypeName;

		// Token: 0x040005F1 RID: 1521
		private string _cosmeticCategoryName;

		// Token: 0x040005F2 RID: 1522
		private bool _isSelected;

		// Token: 0x040005F3 RID: 1523
		private MBBindingList<MPArmoryCosmeticItemBaseVM> _availableCosmetics;
	}
}
