using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000074 RID: 116
	public abstract class ItemValueModel : MBGameModel<ItemValueModel>
	{
		// Token: 0x060007F0 RID: 2032
		public abstract float GetEquipmentValueFromTier(float itemTierf);

		// Token: 0x060007F1 RID: 2033
		public abstract float CalculateTier(ItemObject item);

		// Token: 0x060007F2 RID: 2034
		public abstract int CalculateValue(ItemObject item);

		// Token: 0x060007F3 RID: 2035
		public abstract bool GetIsTransferable(ItemObject item);
	}
}
