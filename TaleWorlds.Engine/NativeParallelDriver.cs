using System;
using System.Threading;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000070 RID: 112
	public sealed class NativeParallelDriver : IParallelDriver
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x0000A74C File Offset: 0x0000894C
		public void For(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = loopBody;
				Utilities.ParallelFor(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0000A79C File Offset: 0x0000899C
		public void ForWithoutRenderThread(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = loopBody;
				Utilities.ParallelForWithoutRenderThread(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0000A7EC File Offset: 0x000089EC
		public void ForWithoutRenderThreadDt(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyWithDtHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = loopBody;
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].DeltaTime = deltaTime;
				Utilities.ParallelForWithoutRenderThreadDt(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0000A84C File Offset: 0x00008A4C
		public void For(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyWithDtHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = loopBody;
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].DeltaTime = deltaTime;
				Utilities.ParallelForWithDt(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0000A8AC File Offset: 0x00008AAC
		public ulong GetMainThreadId()
		{
			return Utilities.GetMainThreadId();
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0000A8B3 File Offset: 0x00008AB3
		public ulong GetCurrentThreadId()
		{
			return Utilities.GetCurrentThreadId();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0000A8BA File Offset: 0x00008ABA
		[EngineCallback(null, false)]
		internal static void ParalelForLoopBodyCaller(long loopBodyKey, int localStartIndex, int localEndIndex)
		{
			NativeParallelDriver._loopBodyCache[(int)(checked((IntPtr)loopBodyKey))].LoopBody(localStartIndex, localEndIndex);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0000A8D4 File Offset: 0x00008AD4
		[EngineCallback(null, false)]
		internal static void ParalelForLoopBodyWithDtCaller(long loopBodyKey, int localStartIndex, int localEndIndex)
		{
			checked
			{
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)loopBodyKey)].LoopBody(localStartIndex, localEndIndex, NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)loopBodyKey)].DeltaTime);
			}
		}

		// Token: 0x04000159 RID: 345
		private const int K = 256;

		// Token: 0x0400015A RID: 346
		private static readonly NativeParallelDriver.LoopBodyHolder[] _loopBodyCache = new NativeParallelDriver.LoopBodyHolder[256];

		// Token: 0x0400015B RID: 347
		private static readonly NativeParallelDriver.LoopBodyWithDtHolder[] _loopBodyWithDtCache = new NativeParallelDriver.LoopBodyWithDtHolder[256];

		// Token: 0x020000CD RID: 205
		private struct LoopBodyHolder
		{
			// Token: 0x04000433 RID: 1075
			public static long UniqueLoopBodyKeySeed;

			// Token: 0x04000434 RID: 1076
			public TWParallel.ParallelForAuxPredicate LoopBody;
		}

		// Token: 0x020000CE RID: 206
		private struct LoopBodyWithDtHolder
		{
			// Token: 0x04000435 RID: 1077
			public static long UniqueLoopBodyKeySeed;

			// Token: 0x04000436 RID: 1078
			public TWParallel.ParallelForWithDtAuxPredicate LoopBody;

			// Token: 0x04000437 RID: 1079
			public float DeltaTime;
		}
	}
}
