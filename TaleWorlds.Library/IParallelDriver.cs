using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000099 RID: 153
	public interface IParallelDriver
	{
		// Token: 0x06000577 RID: 1399
		void For(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize);

		// Token: 0x06000578 RID: 1400
		void ForWithoutRenderThread(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize);

		// Token: 0x06000579 RID: 1401
		void ForWithoutRenderThreadDt(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize);

		// Token: 0x0600057A RID: 1402
		void For(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize);

		// Token: 0x0600057B RID: 1403
		ulong GetMainThreadId();

		// Token: 0x0600057C RID: 1404
		ulong GetCurrentThreadId();
	}
}
