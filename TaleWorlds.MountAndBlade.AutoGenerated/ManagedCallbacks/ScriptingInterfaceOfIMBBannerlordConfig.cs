using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200000E RID: 14
	internal class ScriptingInterfaceOfIMBBannerlordConfig : IMBBannerlordConfig
	{
		// Token: 0x060001FE RID: 510 RVA: 0x0000B006 File Offset: 0x00009206
		public void ValidateOptions()
		{
			ScriptingInterfaceOfIMBBannerlordConfig.call_ValidateOptionsDelegate();
		}

		// Token: 0x04000189 RID: 393
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400018A RID: 394
		public static ScriptingInterfaceOfIMBBannerlordConfig.ValidateOptionsDelegate call_ValidateOptionsDelegate;

		// Token: 0x020001F5 RID: 501
		// (Invoke) Token: 0x06000AC4 RID: 2756
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ValidateOptionsDelegate();
	}
}
