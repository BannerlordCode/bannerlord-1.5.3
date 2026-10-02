using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000005 RID: 5
	public sealed class CallResult
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002428 File Offset: 0x00000628
		public bool Success { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002430 File Offset: 0x00000630
		public FunctionResult Result { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002438 File Offset: 0x00000638
		public string SuccessfulReason { get; }

		// Token: 0x06000011 RID: 17 RVA: 0x00002440 File Offset: 0x00000640
		public CallResult(bool success, FunctionResult result, string successfulReason = null)
		{
			this.Success = success;
			this.Result = result;
			this.SuccessfulReason = successfulReason;
		}
	}
}
