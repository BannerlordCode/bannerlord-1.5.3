using System;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory
{
	// Token: 0x02000084 RID: 132
	public class MPArmoryTauntCosmeticCategoryVM : MPArmoryCosmeticCategoryBaseVM
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000D2E RID: 3374 RVA: 0x00028AC0 File Offset: 0x00026CC0
		// (remove) Token: 0x06000D2F RID: 3375 RVA: 0x00028AF4 File Offset: 0x00026CF4
		public static event Action<MPArmoryTauntCosmeticCategoryVM> OnSelected;

		// Token: 0x06000D30 RID: 3376 RVA: 0x00028B27 File Offset: 0x00026D27
		public MPArmoryTauntCosmeticCategoryVM(MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategory)
			: base(CosmeticsManager.CosmeticType.Taunt)
		{
			this.TauntCategory = tauntCategory;
			base.CosmeticCategoryName = tauntCategory.ToString();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00028B4A File Offset: 0x00026D4A
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00028B7C File Offset: 0x00026D7C
		protected override void ExecuteSelectCategory()
		{
			Action<MPArmoryTauntCosmeticCategoryVM> onSelected = MPArmoryTauntCosmeticCategoryVM.OnSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x040005F5 RID: 1525
		public readonly MPArmoryCosmeticsVM.TauntCategoryFlag TauntCategory;
	}
}
