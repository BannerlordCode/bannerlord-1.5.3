using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000094 RID: 148
	public interface IShipOrigin
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060008BB RID: 2235
		ShipHull Hull { get; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060008BC RID: 2236
		TextObject Name { get; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060008BD RID: 2237
		string OriginShipId { get; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060008BE RID: 2238
		bool IsPlayerShip { get; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060008BF RID: 2239
		float HitPoints { get; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060008C0 RID: 2240
		float MaxHitPoints { get; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060008C1 RID: 2241
		float MaxFireHitPoints { get; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060008C2 RID: 2242
		float SailHitPoints { get; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060008C3 RID: 2243
		float MaxSailHitPoints { get; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060008C4 RID: 2244
		int TotalCrewCapacity { get; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060008C5 RID: 2245
		int MainDeckCrewCapacity { get; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060008C6 RID: 2246
		int SkeletalCrewCapacity { get; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060008C7 RID: 2247
		int DefaultFormationGroupIndex { get; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060008C8 RID: 2248
		float ForwardDragFactor { get; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060008C9 RID: 2249
		float ShipWeightFactor { get; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060008CA RID: 2250
		float RudderSurfaceAreaFactor { get; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060008CB RID: 2251
		int RandomValue { get; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060008CC RID: 2252
		string CustomSailPatternId { get; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060008CD RID: 2253
		float MaxRudderForceFactor { get; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060008CE RID: 2254
		float MaxOarForceFactor { get; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060008CF RID: 2255
		float SailForceFactor { get; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060008D0 RID: 2256
		float MaxOarPowerFactor { get; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060008D1 RID: 2257
		float SailRotationSpeedFactor { get; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060008D2 RID: 2258
		float FurlUnfurlSpeedFactor { get; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060008D3 RID: 2259
		float CrewShieldHitPointsFactor { get; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060008D4 RID: 2260
		float CrewMeleeDamageFactor { get; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060008D5 RID: 2261
		int AdditionalArcherQuivers { get; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060008D6 RID: 2262
		int AdditionalThrowingWeaponStack { get; }

		// Token: 0x060008D7 RID: 2263
		void OnShipDamaged(float rawDamage, IShipOrigin rammingShip, out float modifiedDamage);

		// Token: 0x060008D8 RID: 2264
		void OnSailDamaged(float rawDamage, float inflictedDamage);

		// Token: 0x060008D9 RID: 2265
		List<ShipVisualSlotInfo> GetShipVisualSlotInfos();

		// Token: 0x060008DA RID: 2266
		List<ShipSlotAndPieceName> GetShipSlotAndPieceNames();
	}
}
