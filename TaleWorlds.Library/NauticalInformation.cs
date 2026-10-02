using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000015 RID: 21
	public struct NauticalInformation
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public void DeserializeFrom(IReader reader)
		{
			this.WaveStrength = reader.ReadFloat();
			this.WindVector = reader.ReadVec2();
			this.CanUseLowAltitudeAtmosphere = reader.ReadInt();
			this.UseSceneWindDirection = reader.ReadInt();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B03 File Offset: 0x00000D03
		public void SerializeTo(IWriter writer)
		{
			writer.WriteFloat(this.WaveStrength);
			writer.WriteVec2(this.WindVector);
			writer.WriteInt(this.CanUseLowAltitudeAtmosphere);
			writer.WriteInt(this.UseSceneWindDirection);
		}

		// Token: 0x0400003C RID: 60
		public float WaveStrength;

		// Token: 0x0400003D RID: 61
		public Vec2 WindVector;

		// Token: 0x0400003E RID: 62
		public int CanUseLowAltitudeAtmosphere;

		// Token: 0x0400003F RID: 63
		public int UseSceneWindDirection;

		// Token: 0x04000040 RID: 64
		public int IsRiverBattle;

		// Token: 0x04000041 RID: 65
		public int IsInsideStorm;

		// Token: 0x04000042 RID: 66
		public int UsesNavalSimulatedWater;
	}
}
