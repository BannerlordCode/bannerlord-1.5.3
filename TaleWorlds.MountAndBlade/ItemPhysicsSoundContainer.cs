using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F6 RID: 502
	public static class ItemPhysicsSoundContainer
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001D67 RID: 7527 RVA: 0x00063E79 File Offset: 0x00062079
		// (set) Token: 0x06001D68 RID: 7528 RVA: 0x00063E80 File Offset: 0x00062080
		public static int SoundCodePhysicsBoulderDefault { get; private set; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x00063E88 File Offset: 0x00062088
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x00063E8F File Offset: 0x0006208F
		public static int SoundCodePhysicsArrowlikeDefault { get; private set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x00063E97 File Offset: 0x00062097
		// (set) Token: 0x06001D6C RID: 7532 RVA: 0x00063E9E File Offset: 0x0006209E
		public static int SoundCodePhysicsBowlikeDefault { get; private set; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x00063EA6 File Offset: 0x000620A6
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x00063EAD File Offset: 0x000620AD
		public static int SoundCodePhysicsDaggerlikeDefault { get; private set; }

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x00063EB5 File Offset: 0x000620B5
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x00063EBC File Offset: 0x000620BC
		public static int SoundCodePhysicsGreatswordlikeDefault { get; private set; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x00063EC4 File Offset: 0x000620C4
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x00063ECB File Offset: 0x000620CB
		public static int SoundCodePhysicsShieldlikeDefault { get; private set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x00063ED3 File Offset: 0x000620D3
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x00063EDA File Offset: 0x000620DA
		public static int SoundCodePhysicsSpearlikeDefault { get; private set; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x00063EE2 File Offset: 0x000620E2
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x00063EE9 File Offset: 0x000620E9
		public static int SoundCodePhysicsSwordlikeDefault { get; private set; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x00063EF1 File Offset: 0x000620F1
		// (set) Token: 0x06001D78 RID: 7544 RVA: 0x00063EF8 File Offset: 0x000620F8
		public static int SoundCodePhysicsBoulderWood { get; private set; }

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x00063F00 File Offset: 0x00062100
		// (set) Token: 0x06001D7A RID: 7546 RVA: 0x00063F07 File Offset: 0x00062107
		public static int SoundCodePhysicsArrowlikeWood { get; private set; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x00063F0F File Offset: 0x0006210F
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x00063F16 File Offset: 0x00062116
		public static int SoundCodePhysicsBowlikeWood { get; private set; }

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x00063F1E File Offset: 0x0006211E
		// (set) Token: 0x06001D7E RID: 7550 RVA: 0x00063F25 File Offset: 0x00062125
		public static int SoundCodePhysicsDaggerlikeWood { get; private set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001D7F RID: 7551 RVA: 0x00063F2D File Offset: 0x0006212D
		// (set) Token: 0x06001D80 RID: 7552 RVA: 0x00063F34 File Offset: 0x00062134
		public static int SoundCodePhysicsGreatswordlikeWood { get; private set; }

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x00063F3C File Offset: 0x0006213C
		// (set) Token: 0x06001D82 RID: 7554 RVA: 0x00063F43 File Offset: 0x00062143
		public static int SoundCodePhysicsShieldlikeWood { get; private set; }

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001D83 RID: 7555 RVA: 0x00063F4B File Offset: 0x0006214B
		// (set) Token: 0x06001D84 RID: 7556 RVA: 0x00063F52 File Offset: 0x00062152
		public static int SoundCodePhysicsSpearlikeWood { get; private set; }

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x00063F5A File Offset: 0x0006215A
		// (set) Token: 0x06001D86 RID: 7558 RVA: 0x00063F61 File Offset: 0x00062161
		public static int SoundCodePhysicsSwordlikeWood { get; private set; }

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x00063F69 File Offset: 0x00062169
		// (set) Token: 0x06001D88 RID: 7560 RVA: 0x00063F70 File Offset: 0x00062170
		public static int SoundCodePhysicsBoulderStone { get; private set; }

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x00063F78 File Offset: 0x00062178
		// (set) Token: 0x06001D8A RID: 7562 RVA: 0x00063F7F File Offset: 0x0006217F
		public static int SoundCodePhysicsArrowlikeStone { get; private set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x00063F87 File Offset: 0x00062187
		// (set) Token: 0x06001D8C RID: 7564 RVA: 0x00063F8E File Offset: 0x0006218E
		public static int SoundCodePhysicsBowlikeStone { get; private set; }

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x00063F96 File Offset: 0x00062196
		// (set) Token: 0x06001D8E RID: 7566 RVA: 0x00063F9D File Offset: 0x0006219D
		public static int SoundCodePhysicsDaggerlikeStone { get; private set; }

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x00063FA5 File Offset: 0x000621A5
		// (set) Token: 0x06001D90 RID: 7568 RVA: 0x00063FAC File Offset: 0x000621AC
		public static int SoundCodePhysicsGreatswordlikeStone { get; private set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x00063FB4 File Offset: 0x000621B4
		// (set) Token: 0x06001D92 RID: 7570 RVA: 0x00063FBB File Offset: 0x000621BB
		public static int SoundCodePhysicsShieldlikeStone { get; private set; }

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x00063FC3 File Offset: 0x000621C3
		// (set) Token: 0x06001D94 RID: 7572 RVA: 0x00063FCA File Offset: 0x000621CA
		public static int SoundCodePhysicsSpearlikeStone { get; private set; }

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x00063FD2 File Offset: 0x000621D2
		// (set) Token: 0x06001D96 RID: 7574 RVA: 0x00063FD9 File Offset: 0x000621D9
		public static int SoundCodePhysicsSwordlikeStone { get; private set; }

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x00063FE1 File Offset: 0x000621E1
		// (set) Token: 0x06001D98 RID: 7576 RVA: 0x00063FE8 File Offset: 0x000621E8
		public static int SoundCodePhysicsWater { get; private set; }

		// Token: 0x06001D99 RID: 7577 RVA: 0x00063FF0 File Offset: 0x000621F0
		static ItemPhysicsSoundContainer()
		{
			ItemPhysicsSoundContainer.UpdateItemPhysicsSoundCodes();
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00063FF8 File Offset: 0x000621F8
		private static void UpdateItemPhysicsSoundCodes()
		{
			ItemPhysicsSoundContainer.SoundCodePhysicsBoulderDefault = SoundEvent.GetEventIdFromString("event:/physics/boulder/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/arrowlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/bowlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/daggerlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/greatswordlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/shieldlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/spearlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/swordlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsBoulderWood = SoundEvent.GetEventIdFromString("event:/physics/boulder/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeWood = SoundEvent.GetEventIdFromString("event:/physics/arrowlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeWood = SoundEvent.GetEventIdFromString("event:/physics/bowlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeWood = SoundEvent.GetEventIdFromString("event:/physics/daggerlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeWood = SoundEvent.GetEventIdFromString("event:/physics/greatswordlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeWood = SoundEvent.GetEventIdFromString("event:/physics/shieldlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood = SoundEvent.GetEventIdFromString("event:/physics/spearlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeWood = SoundEvent.GetEventIdFromString("event:/physics/swordlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsBoulderStone = SoundEvent.GetEventIdFromString("event:/physics/boulder/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeStone = SoundEvent.GetEventIdFromString("event:/physics/arrowlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeStone = SoundEvent.GetEventIdFromString("event:/physics/bowlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeStone = SoundEvent.GetEventIdFromString("event:/physics/daggerlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeStone = SoundEvent.GetEventIdFromString("event:/physics/greatswordlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeStone = SoundEvent.GetEventIdFromString("event:/physics/shieldlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone = SoundEvent.GetEventIdFromString("event:/physics/spearlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeStone = SoundEvent.GetEventIdFromString("event:/physics/swordlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsWater = SoundEvent.GetEventIdFromString("event:/physics/water");
		}
	}
}
