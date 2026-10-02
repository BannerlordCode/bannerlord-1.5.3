using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000012 RID: 18
	public struct SnowInformation
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002A35 File Offset: 0x00000C35
		public void DeserializeFrom(IReader reader)
		{
			this.Density = reader.ReadFloat();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A43 File Offset: 0x00000C43
		public void SerializeTo(IWriter writer)
		{
			writer.WriteFloat(this.Density);
		}

		// Token: 0x04000036 RID: 54
		public float Density;
	}
}
