using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x0200009B RID: 155
	public static class TWParallel
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x0001386E File Offset: 0x00011A6E
		public static void InitializeAndSetImplementation(IParallelDriver parallelDriver)
		{
			TWParallel._parallelDriver = parallelDriver;
			TWParallel._mainThreadId = TWParallel.GetMainThreadId();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00013880 File Offset: 0x00011A80
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
		{
			return Parallel.ForEach<TSource>(Partitioner.Create<TSource>(source), Common.ParallelOptions, body);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00013893 File Offset: 0x00011A93
		[Obsolete("Please use For() not ForEach() for better Parallel Performance.", true)]
		public static void ForEach<TSource>(IList<TSource> source, Action<TSource> body)
		{
			Parallel.ForEach<TSource>(Partitioner.Create<TSource>(source), Common.ParallelOptions, body);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000138A7 File Offset: 0x00011AA7
		public static void For(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive);
				return;
			}
			TWParallel._parallelDriver.For(fromInclusive, toExclusive, body, grainSize);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000138C6 File Offset: 0x00011AC6
		public static void ForWithoutRenderThread(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive);
				return;
			}
			TWParallel._parallelDriver.ForWithoutRenderThread(fromInclusive, toExclusive, body, grainSize);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x000138E5 File Offset: 0x00011AE5
		public static void ForWithoutRenderThreadDt(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive, deltaTime);
				return;
			}
			TWParallel._parallelDriver.ForWithoutRenderThreadDt(fromInclusive, toExclusive, deltaTime, body, grainSize);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00013908 File Offset: 0x00011B08
		public static void For(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive, deltaTime);
				return;
			}
			TWParallel._parallelDriver.For(fromInclusive, toExclusive, deltaTime, body, grainSize);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001392B File Offset: 0x00011B2B
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void AssertIsMainThread()
		{
			TWParallel.GetCurrentThreadId();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00013933 File Offset: 0x00011B33
		public static bool IsMainThread()
		{
			return TWParallel._mainThreadId == TWParallel.GetCurrentThreadId();
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00013941 File Offset: 0x00011B41
		private static ulong GetMainThreadId()
		{
			return TWParallel._parallelDriver.GetMainThreadId();
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001394D File Offset: 0x00011B4D
		internal static ulong GetCurrentThreadId()
		{
			return TWParallel._parallelDriver.GetCurrentThreadId();
		}

		// Token: 0x040001B2 RID: 434
		private static IParallelDriver _parallelDriver = new DefaultParallelDriver();

		// Token: 0x040001B3 RID: 435
		private static ulong _mainThreadId;

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x060007C3 RID: 1987
		public delegate void ParallelForAuxPredicate(int localStartIndex, int localEndIndex);

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x060007C7 RID: 1991
		public delegate void ParallelForWithDtAuxPredicate(int localStartIndex, int localEndIndex, float dt);
	}
}
