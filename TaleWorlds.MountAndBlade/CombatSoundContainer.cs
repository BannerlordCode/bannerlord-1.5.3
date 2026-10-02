using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F5 RID: 501
	public static class CombatSoundContainer
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x00063AA2 File Offset: 0x00061CA2
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x00063AA9 File Offset: 0x00061CA9
		public static int SoundCodeMissionCombatBluntHigh { get; private set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x00063AB1 File Offset: 0x00061CB1
		// (set) Token: 0x06001D28 RID: 7464 RVA: 0x00063AB8 File Offset: 0x00061CB8
		public static int SoundCodeMissionCombatBluntLow { get; private set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x00063AC0 File Offset: 0x00061CC0
		// (set) Token: 0x06001D2A RID: 7466 RVA: 0x00063AC7 File Offset: 0x00061CC7
		public static int SoundCodeMissionCombatBluntMed { get; private set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x00063ACF File Offset: 0x00061CCF
		// (set) Token: 0x06001D2C RID: 7468 RVA: 0x00063AD6 File Offset: 0x00061CD6
		public static int SoundCodeMissionCombatBoulderHigh { get; private set; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x00063ADE File Offset: 0x00061CDE
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x00063AE5 File Offset: 0x00061CE5
		public static int SoundCodeMissionCombatBoulderLow { get; private set; }

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x00063AED File Offset: 0x00061CED
		// (set) Token: 0x06001D30 RID: 7472 RVA: 0x00063AF4 File Offset: 0x00061CF4
		public static int SoundCodeMissionCombatBoulderMed { get; private set; }

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x00063AFC File Offset: 0x00061CFC
		// (set) Token: 0x06001D32 RID: 7474 RVA: 0x00063B03 File Offset: 0x00061D03
		public static int SoundCodeMissionCombatCutHigh { get; private set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x00063B0B File Offset: 0x00061D0B
		// (set) Token: 0x06001D34 RID: 7476 RVA: 0x00063B12 File Offset: 0x00061D12
		public static int SoundCodeMissionCombatCutLow { get; private set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x00063B1A File Offset: 0x00061D1A
		// (set) Token: 0x06001D36 RID: 7478 RVA: 0x00063B21 File Offset: 0x00061D21
		public static int SoundCodeMissionCombatCutMed { get; private set; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x00063B29 File Offset: 0x00061D29
		// (set) Token: 0x06001D38 RID: 7480 RVA: 0x00063B30 File Offset: 0x00061D30
		public static int SoundCodeMissionCombatMissileHigh { get; private set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x00063B38 File Offset: 0x00061D38
		// (set) Token: 0x06001D3A RID: 7482 RVA: 0x00063B3F File Offset: 0x00061D3F
		public static int SoundCodeMissionCombatMissileLow { get; private set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001D3B RID: 7483 RVA: 0x00063B47 File Offset: 0x00061D47
		// (set) Token: 0x06001D3C RID: 7484 RVA: 0x00063B4E File Offset: 0x00061D4E
		public static int SoundCodeMissionCombatMissileMed { get; private set; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x00063B56 File Offset: 0x00061D56
		// (set) Token: 0x06001D3E RID: 7486 RVA: 0x00063B5D File Offset: 0x00061D5D
		public static int SoundCodeMissionCombatPierceHigh { get; private set; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001D3F RID: 7487 RVA: 0x00063B65 File Offset: 0x00061D65
		// (set) Token: 0x06001D40 RID: 7488 RVA: 0x00063B6C File Offset: 0x00061D6C
		public static int SoundCodeMissionCombatPierceLow { get; private set; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001D41 RID: 7489 RVA: 0x00063B74 File Offset: 0x00061D74
		// (set) Token: 0x06001D42 RID: 7490 RVA: 0x00063B7B File Offset: 0x00061D7B
		public static int SoundCodeMissionCombatPierceMed { get; private set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001D43 RID: 7491 RVA: 0x00063B83 File Offset: 0x00061D83
		// (set) Token: 0x06001D44 RID: 7492 RVA: 0x00063B8A File Offset: 0x00061D8A
		public static int SoundCodeMissionCombatPunchHigh { get; private set; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x00063B92 File Offset: 0x00061D92
		// (set) Token: 0x06001D46 RID: 7494 RVA: 0x00063B99 File Offset: 0x00061D99
		public static int SoundCodeMissionCombatPunchLow { get; private set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x00063BA1 File Offset: 0x00061DA1
		// (set) Token: 0x06001D48 RID: 7496 RVA: 0x00063BA8 File Offset: 0x00061DA8
		public static int SoundCodeMissionCombatPunchMed { get; private set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00063BB0 File Offset: 0x00061DB0
		// (set) Token: 0x06001D4A RID: 7498 RVA: 0x00063BB7 File Offset: 0x00061DB7
		public static int SoundCodeMissionCombatThrowingAxeHigh { get; private set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x00063BBF File Offset: 0x00061DBF
		// (set) Token: 0x06001D4C RID: 7500 RVA: 0x00063BC6 File Offset: 0x00061DC6
		public static int SoundCodeMissionCombatThrowingAxeLow { get; private set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001D4D RID: 7501 RVA: 0x00063BCE File Offset: 0x00061DCE
		// (set) Token: 0x06001D4E RID: 7502 RVA: 0x00063BD5 File Offset: 0x00061DD5
		public static int SoundCodeMissionCombatThrowingAxeMed { get; private set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x00063BDD File Offset: 0x00061DDD
		// (set) Token: 0x06001D50 RID: 7504 RVA: 0x00063BE4 File Offset: 0x00061DE4
		public static int SoundCodeMissionCombatThrowingDaggerHigh { get; private set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x00063BEC File Offset: 0x00061DEC
		// (set) Token: 0x06001D52 RID: 7506 RVA: 0x00063BF3 File Offset: 0x00061DF3
		public static int SoundCodeMissionCombatThrowingDaggerLow { get; private set; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x00063BFB File Offset: 0x00061DFB
		// (set) Token: 0x06001D54 RID: 7508 RVA: 0x00063C02 File Offset: 0x00061E02
		public static int SoundCodeMissionCombatThrowingDaggerMed { get; private set; }

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x00063C0A File Offset: 0x00061E0A
		// (set) Token: 0x06001D56 RID: 7510 RVA: 0x00063C11 File Offset: 0x00061E11
		public static int SoundCodeMissionCombatThrowingStoneHigh { get; private set; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x00063C19 File Offset: 0x00061E19
		// (set) Token: 0x06001D58 RID: 7512 RVA: 0x00063C20 File Offset: 0x00061E20
		public static int SoundCodeMissionCombatThrowingStoneLow { get; private set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x00063C28 File Offset: 0x00061E28
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x00063C2F File Offset: 0x00061E2F
		public static int SoundCodeMissionCombatThrowingStoneMed { get; private set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x00063C37 File Offset: 0x00061E37
		// (set) Token: 0x06001D5C RID: 7516 RVA: 0x00063C3E File Offset: 0x00061E3E
		public static int SoundCodeMissionCombatChargeDamage { get; private set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x00063C46 File Offset: 0x00061E46
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x00063C4D File Offset: 0x00061E4D
		public static int SoundCodeMissionCombatKick { get; private set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x00063C55 File Offset: 0x00061E55
		// (set) Token: 0x06001D60 RID: 7520 RVA: 0x00063C5C File Offset: 0x00061E5C
		public static int SoundCodeMissionCombatPlayerhit { get; private set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x00063C64 File Offset: 0x00061E64
		// (set) Token: 0x06001D62 RID: 7522 RVA: 0x00063C6B File Offset: 0x00061E6B
		public static int SoundCodeMissionCombatWoodShieldBash { get; private set; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x00063C73 File Offset: 0x00061E73
		// (set) Token: 0x06001D64 RID: 7524 RVA: 0x00063C7A File Offset: 0x00061E7A
		public static int SoundCodeMissionCombatMetalShieldBash { get; private set; }

		// Token: 0x06001D65 RID: 7525 RVA: 0x00063C82 File Offset: 0x00061E82
		static CombatSoundContainer()
		{
			CombatSoundContainer.UpdateMissionCombatSoundCodes();
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00063C8C File Offset: 0x00061E8C
		private static void UpdateMissionCombatSoundCodes()
		{
			CombatSoundContainer.SoundCodeMissionCombatBluntHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/blunt/high");
			CombatSoundContainer.SoundCodeMissionCombatBluntLow = SoundEvent.GetEventIdFromString("event:/mission/combat/blunt/low");
			CombatSoundContainer.SoundCodeMissionCombatBluntMed = SoundEvent.GetEventIdFromString("event:/mission/combat/blunt/med");
			CombatSoundContainer.SoundCodeMissionCombatBoulderHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/boulder/high");
			CombatSoundContainer.SoundCodeMissionCombatBoulderLow = SoundEvent.GetEventIdFromString("event:/mission/combat/boulder/low");
			CombatSoundContainer.SoundCodeMissionCombatBoulderMed = SoundEvent.GetEventIdFromString("event:/mission/combat/boulder/med");
			CombatSoundContainer.SoundCodeMissionCombatCutHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/cut/high");
			CombatSoundContainer.SoundCodeMissionCombatCutLow = SoundEvent.GetEventIdFromString("event:/mission/combat/cut/low");
			CombatSoundContainer.SoundCodeMissionCombatCutMed = SoundEvent.GetEventIdFromString("event:/mission/combat/cut/med");
			CombatSoundContainer.SoundCodeMissionCombatMissileHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/missile/high");
			CombatSoundContainer.SoundCodeMissionCombatMissileLow = SoundEvent.GetEventIdFromString("event:/mission/combat/missile/low");
			CombatSoundContainer.SoundCodeMissionCombatMissileMed = SoundEvent.GetEventIdFromString("event:/mission/combat/missile/med");
			CombatSoundContainer.SoundCodeMissionCombatPierceHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/pierce/high");
			CombatSoundContainer.SoundCodeMissionCombatPierceLow = SoundEvent.GetEventIdFromString("event:/mission/combat/pierce/low");
			CombatSoundContainer.SoundCodeMissionCombatPierceMed = SoundEvent.GetEventIdFromString("event:/mission/combat/pierce/med");
			CombatSoundContainer.SoundCodeMissionCombatPunchHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/punch/high");
			CombatSoundContainer.SoundCodeMissionCombatPunchLow = SoundEvent.GetEventIdFromString("event:/mission/combat/punch/low");
			CombatSoundContainer.SoundCodeMissionCombatPunchMed = SoundEvent.GetEventIdFromString("event:/mission/combat/punch/med");
			CombatSoundContainer.SoundCodeMissionCombatThrowingAxeHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/throwing/high");
			CombatSoundContainer.SoundCodeMissionCombatThrowingAxeLow = SoundEvent.GetEventIdFromString("event:/mission/combat/throwing/low");
			CombatSoundContainer.SoundCodeMissionCombatThrowingAxeMed = SoundEvent.GetEventIdFromString("event:/mission/combat/throwing/med");
			CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/throwing/high");
			CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerLow = SoundEvent.GetEventIdFromString("event:/mission/combat/throwing/low");
			CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerMed = SoundEvent.GetEventIdFromString("event:/mission/combat/throwing/med");
			CombatSoundContainer.SoundCodeMissionCombatThrowingStoneHigh = SoundEvent.GetEventIdFromString("event:/mission/combat/throwingstone/high");
			CombatSoundContainer.SoundCodeMissionCombatThrowingStoneLow = SoundEvent.GetEventIdFromString("event:/mission/combat/throwingstone/low");
			CombatSoundContainer.SoundCodeMissionCombatThrowingStoneMed = SoundEvent.GetEventIdFromString("event:/mission/combat/throwingstone/med");
			CombatSoundContainer.SoundCodeMissionCombatChargeDamage = SoundEvent.GetEventIdFromString("event:/mission/combat/charge/damage");
			CombatSoundContainer.SoundCodeMissionCombatKick = SoundEvent.GetEventIdFromString("event:/mission/combat/kick");
			CombatSoundContainer.SoundCodeMissionCombatPlayerhit = SoundEvent.GetEventIdFromString("event:/mission/combat/playerHit");
			CombatSoundContainer.SoundCodeMissionCombatWoodShieldBash = SoundEvent.GetEventIdFromString("event:/mission/combat/shield/bash");
			CombatSoundContainer.SoundCodeMissionCombatMetalShieldBash = SoundEvent.GetEventIdFromString("event:/mission/combat/shield/metal_bash");
		}
	}
}
