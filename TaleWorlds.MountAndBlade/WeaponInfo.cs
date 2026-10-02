using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200039C RID: 924
	public struct WeaponInfo
	{
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003550 RID: 13648 RVA: 0x000DD023 File Offset: 0x000DB223
		// (set) Token: 0x06003551 RID: 13649 RVA: 0x000DD02B File Offset: 0x000DB22B
		public bool IsValid { get; private set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003552 RID: 13650 RVA: 0x000DD034 File Offset: 0x000DB234
		// (set) Token: 0x06003553 RID: 13651 RVA: 0x000DD03C File Offset: 0x000DB23C
		public bool IsMeleeWeapon { get; private set; }

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003554 RID: 13652 RVA: 0x000DD045 File Offset: 0x000DB245
		// (set) Token: 0x06003555 RID: 13653 RVA: 0x000DD04D File Offset: 0x000DB24D
		public bool IsRangedWeapon { get; private set; }

		// Token: 0x06003556 RID: 13654 RVA: 0x000DD056 File Offset: 0x000DB256
		public WeaponInfo(bool isValid, bool isMeleeWeapon, bool isRangedWeapon)
		{
			this.IsValid = isValid;
			this.IsMeleeWeapon = isMeleeWeapon;
			this.IsRangedWeapon = isRangedWeapon;
		}
	}
}
