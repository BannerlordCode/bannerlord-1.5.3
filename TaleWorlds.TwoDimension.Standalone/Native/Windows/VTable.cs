using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200002C RID: 44
	internal static class VTable
	{
		// Token: 0x0600010A RID: 266 RVA: 0x000069C3 File Offset: 0x00004BC3
		private static long MakeKey(IntPtr fn, Type t)
		{
			return fn.ToInt64() ^ ((long)t.GetHashCode() << 32);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000069D8 File Offset: 0x00004BD8
		internal static Delegate Get(IntPtr comObj, int slot, Type delegateType)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(Marshal.ReadIntPtr(comObj), slot * IntPtr.Size);
			long num = VTable.MakeKey(intPtr, delegateType);
			Delegate @delegate;
			if (VTable._cache.TryGetValue(num, out @delegate))
			{
				return @delegate;
			}
			Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(intPtr, delegateType);
			VTable._cache[num] = delegateForFunctionPointer;
			return delegateForFunctionPointer;
		}

		// Token: 0x04000117 RID: 279
		private static readonly Dictionary<long, Delegate> _cache = new Dictionary<long, Delegate>();
	}
}
