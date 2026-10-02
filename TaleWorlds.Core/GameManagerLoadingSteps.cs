using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006C RID: 108
	public enum GameManagerLoadingSteps
	{
		// Token: 0x04000407 RID: 1031
		None = -1,
		// Token: 0x04000408 RID: 1032
		PreInitializeZerothStep,
		// Token: 0x04000409 RID: 1033
		FirstInitializeFirstStep,
		// Token: 0x0400040A RID: 1034
		WaitSecondStep,
		// Token: 0x0400040B RID: 1035
		SecondInitializeThirdState,
		// Token: 0x0400040C RID: 1036
		PostInitializeFourthState,
		// Token: 0x0400040D RID: 1037
		FinishLoadingFifthStep,
		// Token: 0x0400040E RID: 1038
		LoadingIsOver
	}
}
