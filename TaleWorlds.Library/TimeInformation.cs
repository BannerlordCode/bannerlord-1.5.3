using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000017 RID: 23
	public struct TimeInformation
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002B81 File Offset: 0x00000D81
		public void DeserializeFrom(IReader reader)
		{
			this.TimeOfDay = reader.ReadFloat();
			this.NightTimeFactor = reader.ReadFloat();
			this.DrynessFactor = reader.ReadFloat();
			this.WinterTimeFactor = reader.ReadFloat();
			this.Season = reader.ReadInt();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BBF File Offset: 0x00000DBF
		public void SerializeTo(IWriter writer)
		{
			writer.WriteFloat(this.TimeOfDay);
			writer.WriteFloat(this.NightTimeFactor);
			writer.WriteFloat(this.DrynessFactor);
			writer.WriteFloat(this.WinterTimeFactor);
			writer.WriteInt(this.Season);
		}

		// Token: 0x04000046 RID: 70
		public float TimeOfDay;

		// Token: 0x04000047 RID: 71
		public float NightTimeFactor;

		// Token: 0x04000048 RID: 72
		public float DrynessFactor;

		// Token: 0x04000049 RID: 73
		public float WinterTimeFactor;

		// Token: 0x0400004A RID: 74
		public int Season;
	}
}
