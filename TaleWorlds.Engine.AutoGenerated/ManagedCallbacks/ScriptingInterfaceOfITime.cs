using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002F RID: 47
	internal class ScriptingInterfaceOfITime : ITime
	{
		// Token: 0x0600063D RID: 1597 RVA: 0x0001A5C7 File Offset: 0x000187C7
		public float GetApplicationTime()
		{
			return ScriptingInterfaceOfITime.call_GetApplicationTimeDelegate();
		}

		// Token: 0x0400058B RID: 1419
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400058C RID: 1420
		public static ScriptingInterfaceOfITime.GetApplicationTimeDelegate call_GetApplicationTimeDelegate;

		// Token: 0x020005EC RID: 1516
		// (Invoke) Token: 0x06001DDB RID: 7643
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetApplicationTimeDelegate();
	}
}
