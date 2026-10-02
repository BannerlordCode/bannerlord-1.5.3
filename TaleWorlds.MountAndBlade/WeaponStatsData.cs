using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200039E RID: 926
	[EngineStruct("Weapon_stats_data", false, null)]
	public struct WeaponStatsData
	{
		// Token: 0x040016B2 RID: 5810
		public MatrixFrame WeaponFrame;

		// Token: 0x040016B3 RID: 5811
		public Vec3 RotationSpeed;

		// Token: 0x040016B4 RID: 5812
		public ulong WeaponFlags;

		// Token: 0x040016B5 RID: 5813
		public uint Properties;

		// Token: 0x040016B6 RID: 5814
		public int WeaponClass;

		// Token: 0x040016B7 RID: 5815
		public int AmmoClass;

		// Token: 0x040016B8 RID: 5816
		public int ItemUsageIndex;

		// Token: 0x040016B9 RID: 5817
		public int ThrustSpeed;

		// Token: 0x040016BA RID: 5818
		public int SwingSpeed;

		// Token: 0x040016BB RID: 5819
		public int MissileSpeed;

		// Token: 0x040016BC RID: 5820
		public int ShieldArmor;

		// Token: 0x040016BD RID: 5821
		public int ThrustDamage;

		// Token: 0x040016BE RID: 5822
		public int SwingDamage;

		// Token: 0x040016BF RID: 5823
		public int DefendSpeed;

		// Token: 0x040016C0 RID: 5824
		public int Accuracy;

		// Token: 0x040016C1 RID: 5825
		public int WeaponLength;

		// Token: 0x040016C2 RID: 5826
		public float WeaponBalance;

		// Token: 0x040016C3 RID: 5827
		public float SweetSpot;

		// Token: 0x040016C4 RID: 5828
		public short MaxDataValue;

		// Token: 0x040016C5 RID: 5829
		public short ReloadPhaseCount;

		// Token: 0x040016C6 RID: 5830
		public int ThrustDamageType;

		// Token: 0x040016C7 RID: 5831
		public int SwingDamageType;
	}
}
