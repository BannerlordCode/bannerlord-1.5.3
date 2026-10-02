using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000021 RID: 33
	internal class ScriptingInterfaceOfIMBTestRun : IMBTestRun
	{
		// Token: 0x06000360 RID: 864 RVA: 0x0000D809 File Offset: 0x0000BA09
		public int AutoContinue(int type)
		{
			return ScriptingInterfaceOfIMBTestRun.call_AutoContinueDelegate(type);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D816 File Offset: 0x0000BA16
		public bool CloseScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_CloseSceneDelegate();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000D822 File Offset: 0x0000BA22
		public bool EnterEditMode()
		{
			return ScriptingInterfaceOfIMBTestRun.call_EnterEditModeDelegate();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D82E File Offset: 0x0000BA2E
		public int GetFPS()
		{
			return ScriptingInterfaceOfIMBTestRun.call_GetFPSDelegate();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D83A File Offset: 0x0000BA3A
		public bool LeaveEditMode()
		{
			return ScriptingInterfaceOfIMBTestRun.call_LeaveEditModeDelegate();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D846 File Offset: 0x0000BA46
		public bool NewScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_NewSceneDelegate();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000D852 File Offset: 0x0000BA52
		public bool OpenDefaultScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_OpenDefaultSceneDelegate();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000D860 File Offset: 0x0000BA60
		public bool OpenScene(string sceneName)
		{
			byte[] array = null;
			if (sceneName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBTestRun._utf8.GetByteCount(sceneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBTestRun._utf8.GetBytes(sceneName, 0, sceneName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBTestRun.call_OpenSceneDelegate(array);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000D8BA File Offset: 0x0000BABA
		public bool SaveScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_SaveSceneDelegate();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000D8C6 File Offset: 0x0000BAC6
		public void StartMission()
		{
			ScriptingInterfaceOfIMBTestRun.call_StartMissionDelegate();
		}

		// Token: 0x040002CD RID: 717
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002CE RID: 718
		public static ScriptingInterfaceOfIMBTestRun.AutoContinueDelegate call_AutoContinueDelegate;

		// Token: 0x040002CF RID: 719
		public static ScriptingInterfaceOfIMBTestRun.CloseSceneDelegate call_CloseSceneDelegate;

		// Token: 0x040002D0 RID: 720
		public static ScriptingInterfaceOfIMBTestRun.EnterEditModeDelegate call_EnterEditModeDelegate;

		// Token: 0x040002D1 RID: 721
		public static ScriptingInterfaceOfIMBTestRun.GetFPSDelegate call_GetFPSDelegate;

		// Token: 0x040002D2 RID: 722
		public static ScriptingInterfaceOfIMBTestRun.LeaveEditModeDelegate call_LeaveEditModeDelegate;

		// Token: 0x040002D3 RID: 723
		public static ScriptingInterfaceOfIMBTestRun.NewSceneDelegate call_NewSceneDelegate;

		// Token: 0x040002D4 RID: 724
		public static ScriptingInterfaceOfIMBTestRun.OpenDefaultSceneDelegate call_OpenDefaultSceneDelegate;

		// Token: 0x040002D5 RID: 725
		public static ScriptingInterfaceOfIMBTestRun.OpenSceneDelegate call_OpenSceneDelegate;

		// Token: 0x040002D6 RID: 726
		public static ScriptingInterfaceOfIMBTestRun.SaveSceneDelegate call_SaveSceneDelegate;

		// Token: 0x040002D7 RID: 727
		public static ScriptingInterfaceOfIMBTestRun.StartMissionDelegate call_StartMissionDelegate;

		// Token: 0x02000326 RID: 806
		// (Invoke) Token: 0x06000F88 RID: 3976
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AutoContinueDelegate(int type);

		// Token: 0x02000327 RID: 807
		// (Invoke) Token: 0x06000F8C RID: 3980
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CloseSceneDelegate();

		// Token: 0x02000328 RID: 808
		// (Invoke) Token: 0x06000F90 RID: 3984
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool EnterEditModeDelegate();

		// Token: 0x02000329 RID: 809
		// (Invoke) Token: 0x06000F94 RID: 3988
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFPSDelegate();

		// Token: 0x0200032A RID: 810
		// (Invoke) Token: 0x06000F98 RID: 3992
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool LeaveEditModeDelegate();

		// Token: 0x0200032B RID: 811
		// (Invoke) Token: 0x06000F9C RID: 3996
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool NewSceneDelegate();

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x06000FA0 RID: 4000
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool OpenDefaultSceneDelegate();

		// Token: 0x0200032D RID: 813
		// (Invoke) Token: 0x06000FA4 RID: 4004
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool OpenSceneDelegate(byte[] sceneName);

		// Token: 0x0200032E RID: 814
		// (Invoke) Token: 0x06000FA8 RID: 4008
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool SaveSceneDelegate();

		// Token: 0x0200032F RID: 815
		// (Invoke) Token: 0x06000FAC RID: 4012
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartMissionDelegate();
	}
}
