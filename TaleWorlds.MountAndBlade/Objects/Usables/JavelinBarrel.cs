using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003B3 RID: 947
	public class JavelinBarrel : AmmoBarrelBase
	{
		// Token: 0x060035FE RID: 13822 RVA: 0x000DEDEF File Offset: 0x000DCFEF
		protected override int GetSoundEvent()
		{
			return SoundEvent.GetEventIdFromString(this._pickupSoundEventString);
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000DEDFC File Offset: 0x000DCFFC
		protected override WeaponClass[] GetRequiredWeaponClasses()
		{
			return new WeaponClass[]
			{
				WeaponClass.Arrow,
				WeaponClass.Bolt,
				WeaponClass.SlingStone,
				WeaponClass.Cartridge,
				WeaponClass.ThrowingAxe,
				WeaponClass.ThrowingKnife,
				WeaponClass.Javelin
			};
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000DEE0F File Offset: 0x000DD00F
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=ybGIoUvT}Ammunition Barrels", null);
		}

		// Token: 0x0400170A RID: 5898
		private readonly string _pickupSoundEventString = "event:/mission/combat/pickup_arrows";
	}
}
