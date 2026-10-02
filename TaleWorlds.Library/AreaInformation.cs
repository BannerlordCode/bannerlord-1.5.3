using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000018 RID: 24
	public struct AreaInformation
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00002BFD File Offset: 0x00000DFD
		public void DeserializeFrom(IReader reader)
		{
			this.Temperature = reader.ReadFloat();
			this.Humidity = reader.ReadFloat();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C17 File Offset: 0x00000E17
		public void SerializeTo(IWriter writer)
		{
			writer.WriteFloat(this.Temperature);
			writer.WriteFloat(this.Humidity);
		}

		// Token: 0x0400004B RID: 75
		public float Temperature;

		// Token: 0x0400004C RID: 76
		public float Humidity;
	}
}
