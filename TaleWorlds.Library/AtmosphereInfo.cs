using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x0200001A RID: 26
	public struct AtmosphereInfo
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002C95 File Offset: 0x00000E95
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.AtmosphereName);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public static AtmosphereInfo GetInvalidAtmosphereInfo()
		{
			return new AtmosphereInfo
			{
				AtmosphereName = ""
			};
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CCC File Offset: 0x00000ECC
		public void DeserializeFrom(IReader reader)
		{
			this.SunInfo.DeserializeFrom(reader);
			this.RainInfo.DeserializeFrom(reader);
			this.SnowInfo.DeserializeFrom(reader);
			this.AmbientInfo.DeserializeFrom(reader);
			this.FogInfo.DeserializeFrom(reader);
			this.SkyInfo.DeserializeFrom(reader);
			this.NauticalInfo.DeserializeFrom(reader);
			this.TimeInfo.DeserializeFrom(reader);
			this.AreaInfo.DeserializeFrom(reader);
			this.PostProInfo.DeserializeFrom(reader);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D54 File Offset: 0x00000F54
		public void SerializeTo(IWriter writer)
		{
			this.SunInfo.SerializeTo(writer);
			this.RainInfo.SerializeTo(writer);
			this.SnowInfo.SerializeTo(writer);
			this.AmbientInfo.SerializeTo(writer);
			this.FogInfo.SerializeTo(writer);
			this.SkyInfo.SerializeTo(writer);
			this.NauticalInfo.SerializeTo(writer);
			this.TimeInfo.SerializeTo(writer);
			this.AreaInfo.SerializeTo(writer);
			this.PostProInfo.SerializeTo(writer);
		}

		// Token: 0x04000051 RID: 81
		public uint Seed;

		// Token: 0x04000052 RID: 82
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string AtmosphereName;

		// Token: 0x04000053 RID: 83
		public SunInformation SunInfo;

		// Token: 0x04000054 RID: 84
		public RainInformation RainInfo;

		// Token: 0x04000055 RID: 85
		public SnowInformation SnowInfo;

		// Token: 0x04000056 RID: 86
		public AmbientInformation AmbientInfo;

		// Token: 0x04000057 RID: 87
		public FogInformation FogInfo;

		// Token: 0x04000058 RID: 88
		public SkyInformation SkyInfo;

		// Token: 0x04000059 RID: 89
		public NauticalInformation NauticalInfo;

		// Token: 0x0400005A RID: 90
		public TimeInformation TimeInfo;

		// Token: 0x0400005B RID: 91
		public AreaInformation AreaInfo;

		// Token: 0x0400005C RID: 92
		public PostProcessInformation PostProInfo;

		// Token: 0x0400005D RID: 93
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string InterpolatedAtmosphereName;
	}
}
