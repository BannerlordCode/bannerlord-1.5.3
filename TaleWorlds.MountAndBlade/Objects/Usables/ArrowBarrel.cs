using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003B0 RID: 944
	public class ArrowBarrel : AmmoBarrelBase
	{
		// Token: 0x060035E9 RID: 13801 RVA: 0x000DE6FB File Offset: 0x000DC8FB
		protected override int GetSoundEvent()
		{
			return SoundEvent.GetEventIdFromString(this._pickupSoundEventString);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x000DE708 File Offset: 0x000DC908
		protected override WeaponClass[] GetRequiredWeaponClasses()
		{
			return new WeaponClass[]
			{
				WeaponClass.Arrow,
				WeaponClass.Bolt
			};
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000DE71A File Offset: 0x000DC91A
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=bWi4aMO9}Arrow Barrel", null);
		}

		// Token: 0x040016FA RID: 5882
		private readonly string _pickupSoundEventString = "event:/mission/combat/pickup_arrows";
	}
}
