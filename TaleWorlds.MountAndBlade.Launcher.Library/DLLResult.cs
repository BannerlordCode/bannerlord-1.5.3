using System;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000013 RID: 19
	public class DLLResult
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004696 File Offset: 0x00002896
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000469E File Offset: 0x0000289E
		public string DLLName { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000046A7 File Offset: 0x000028A7
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000046AF File Offset: 0x000028AF
		public bool IsSafe { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000046B8 File Offset: 0x000028B8
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000046C0 File Offset: 0x000028C0
		public string Information { get; set; }

		// Token: 0x060000AA RID: 170 RVA: 0x000046C9 File Offset: 0x000028C9
		public DLLResult(string dLLName, bool isSafe, string information)
		{
			this.DLLName = dLLName;
			this.IsSafe = isSafe;
			this.Information = information;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000046E6 File Offset: 0x000028E6
		public DLLResult()
		{
			this.DLLName = "";
			this.IsSafe = false;
			this.Information = "";
		}
	}
}
