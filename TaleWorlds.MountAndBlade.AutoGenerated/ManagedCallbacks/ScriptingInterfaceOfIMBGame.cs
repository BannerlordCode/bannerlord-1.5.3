using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000014 RID: 20
	internal class ScriptingInterfaceOfIMBGame : IMBGame
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		public void LoadModuleData(bool isLoadGame)
		{
			ScriptingInterfaceOfIMBGame.call_LoadModuleDataDelegate(isLoadGame);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B9C5 File Offset: 0x00009BC5
		public void StartNew()
		{
			ScriptingInterfaceOfIMBGame.call_StartNewDelegate();
		}

		// Token: 0x040001CF RID: 463
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001D0 RID: 464
		public static ScriptingInterfaceOfIMBGame.LoadModuleDataDelegate call_LoadModuleDataDelegate;

		// Token: 0x040001D1 RID: 465
		public static ScriptingInterfaceOfIMBGame.StartNewDelegate call_StartNewDelegate;

		// Token: 0x02000235 RID: 565
		// (Invoke) Token: 0x06000BC4 RID: 3012
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadModuleDataDelegate([MarshalAs(UnmanagedType.U1)] bool isLoadGame);

		// Token: 0x02000236 RID: 566
		// (Invoke) Token: 0x06000BC8 RID: 3016
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartNewDelegate();
	}
}
