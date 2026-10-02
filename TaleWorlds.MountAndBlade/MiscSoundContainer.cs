using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F7 RID: 503
	public static class MiscSoundContainer
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001D9B RID: 7579 RVA: 0x0006417C File Offset: 0x0006237C
		// (set) Token: 0x06001D9C RID: 7580 RVA: 0x00064183 File Offset: 0x00062383
		public static int SoundCodeMovementFoleyDoorOpen { get; private set; }

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x0006418B File Offset: 0x0006238B
		// (set) Token: 0x06001D9E RID: 7582 RVA: 0x00064192 File Offset: 0x00062392
		public static int SoundCodeMovementFoleyDoorClose { get; private set; }

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001D9F RID: 7583 RVA: 0x0006419A File Offset: 0x0006239A
		// (set) Token: 0x06001DA0 RID: 7584 RVA: 0x000641A1 File Offset: 0x000623A1
		public static int SoundCodeAmbientNodeSiegeBallistaFire { get; private set; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x000641A9 File Offset: 0x000623A9
		// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x000641B0 File Offset: 0x000623B0
		public static int SoundCodeAmbientNodeSiegeMangonelFire { get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x000641B8 File Offset: 0x000623B8
		// (set) Token: 0x06001DA4 RID: 7588 RVA: 0x000641BF File Offset: 0x000623BF
		public static int SoundCodeAmbientNodeSiegeTrebuchetFire { get; private set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x000641C7 File Offset: 0x000623C7
		// (set) Token: 0x06001DA6 RID: 7590 RVA: 0x000641CE File Offset: 0x000623CE
		public static int SoundCodeAmbientNodeSiegeBallistaHit { get; private set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x000641D6 File Offset: 0x000623D6
		// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x000641DD File Offset: 0x000623DD
		public static int SoundCodeAmbientNodeSiegeBoulderHit { get; private set; }

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000641E5 File Offset: 0x000623E5
		static MiscSoundContainer()
		{
			MiscSoundContainer.UpdateMiscSoundCodes();
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x000641EC File Offset: 0x000623EC
		private static void UpdateMiscSoundCodes()
		{
			MiscSoundContainer.SoundCodeMovementFoleyDoorOpen = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/door_open");
			MiscSoundContainer.SoundCodeMovementFoleyDoorClose = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/door_close");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeBallistaFire = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/ballista_fire");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeMangonelFire = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/mangonel_fire");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeTrebuchetFire = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/trebuchet_fire");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeBallistaHit = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/ballista_hit");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeBoulderHit = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/boulder_hit");
		}
	}
}
