using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200002C RID: 44
	public class EngineMethod : Attribute
	{
		// Token: 0x0600015F RID: 351 RVA: 0x00005E45 File Offset: 0x00004045
		public EngineMethod(string engineMethodName, bool activateTelemetryProfiling = false, string[] conditionals = null, bool isMonoInline = false)
		{
			this.EngineMethodName = engineMethodName;
			this.ActivateTelemetryProfiling = activateTelemetryProfiling;
			this.Conditionals = conditionals;
			this.IsMonoInline = isMonoInline;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00005E6A File Offset: 0x0000406A
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00005E72 File Offset: 0x00004072
		public string EngineMethodName { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005E7B File Offset: 0x0000407B
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00005E83 File Offset: 0x00004083
		public bool ActivateTelemetryProfiling { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005E8C File Offset: 0x0000408C
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00005E94 File Offset: 0x00004094
		public string[] Conditionals { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00005E9D File Offset: 0x0000409D
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00005EA5 File Offset: 0x000040A5
		public bool IsMonoInline { get; private set; }
	}
}
