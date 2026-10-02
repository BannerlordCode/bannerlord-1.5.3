using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000014 RID: 20
	public struct SkyInformation
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002AB5 File Offset: 0x00000CB5
		public void DeserializeFrom(IReader reader)
		{
			this.Brightness = reader.ReadFloat();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002AC3 File Offset: 0x00000CC3
		public void SerializeTo(IWriter writer)
		{
			writer.WriteFloat(this.Brightness);
		}

		// Token: 0x0400003B RID: 59
		public float Brightness;
	}
}
