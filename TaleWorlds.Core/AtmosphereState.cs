using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x0200000E RID: 14
	public class AtmosphereState
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public AtmosphereState()
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public AtmosphereState(Vec3 position, float tempAv, float tempVar, float humAv, float humVar, string colorGradeTexture)
		{
			this.Position = position;
			this.TemperatureAverage = tempAv;
			this.TemperatureVariance = tempVar;
			this.HumidityAverage = humAv;
			this.HumidityVariance = humVar;
			this.ColorGradeTexture = colorGradeTexture;
		}

		// Token: 0x040000F4 RID: 244
		public Vec3 Position = Vec3.Zero;

		// Token: 0x040000F5 RID: 245
		public float TemperatureAverage;

		// Token: 0x040000F6 RID: 246
		public float TemperatureVariance;

		// Token: 0x040000F7 RID: 247
		public float HumidityAverage;

		// Token: 0x040000F8 RID: 248
		public float HumidityVariance;

		// Token: 0x040000F9 RID: 249
		public float distanceForMaxWeight = 1f;

		// Token: 0x040000FA RID: 250
		public float distanceForMinWeight = 1f;

		// Token: 0x040000FB RID: 251
		public string ColorGradeTexture = "";
	}
}
