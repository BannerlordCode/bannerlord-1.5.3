using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000306 RID: 774
	public static class CompressionMatchmaker
	{
		// Token: 0x0400112E RID: 4398
		public static CompressionInfo.Integer KillDeathAssistCountCompressionInfo = new CompressionInfo.Integer(-1000, 100000, true);

		// Token: 0x0400112F RID: 4399
		public static CompressionInfo.Float MissionTimeCompressionInfo = new CompressionInfo.Float(-5f, 86400f, 20);

		// Token: 0x04001130 RID: 4400
		public static CompressionInfo.Float MissionTimeLowPrecisionCompressionInfo = new CompressionInfo.Float(-5f, 12, 4f);

		// Token: 0x04001131 RID: 4401
		public static CompressionInfo.Integer MissionCurrentStateCompressionInfo = new CompressionInfo.Integer(0, 6);

		// Token: 0x04001132 RID: 4402
		public static CompressionInfo.Integer ScoreCompressionInfo = new CompressionInfo.Integer(-1000000, 21);
	}
}
