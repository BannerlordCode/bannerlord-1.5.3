using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005C RID: 92
	[NullableContext(2)]
	[Nullable(0)]
	internal static class BufferUtils
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x000163F7 File Offset: 0x000145F7
		[NullableContext(1)]
		public static char[] RentBuffer([Nullable(2)] IArrayPool<char> bufferPool, int minSize)
		{
			if (bufferPool == null)
			{
				return new char[minSize];
			}
			return bufferPool.Rent(minSize);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001640A File Offset: 0x0001460A
		public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer)
		{
			if (bufferPool != null)
			{
				bufferPool.Return(buffer);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00016416 File Offset: 0x00014616
		[return: Nullable(1)]
		public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
		{
			if (bufferPool == null)
			{
				return new char[size];
			}
			if (buffer != null)
			{
				bufferPool.Return(buffer);
			}
			return bufferPool.Rent(size);
		}
	}
}
