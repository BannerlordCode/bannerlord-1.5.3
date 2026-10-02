using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000016 RID: 22
	public struct FogInformation
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002B35 File Offset: 0x00000D35
		public void DeserializeFrom(IReader reader)
		{
			this.Density = reader.ReadFloat();
			this.Color = reader.ReadVec3();
			this.Falloff = reader.ReadFloat();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B5B File Offset: 0x00000D5B
		public void SerializeTo(IWriter writer)
		{
			writer.WriteFloat(this.Density);
			writer.WriteVec3(this.Color);
			writer.WriteFloat(this.Falloff);
		}

		// Token: 0x04000043 RID: 67
		public float Density;

		// Token: 0x04000044 RID: 68
		public Vec3 Color;

		// Token: 0x04000045 RID: 69
		public float Falloff;
	}
}
