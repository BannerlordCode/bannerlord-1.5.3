using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001C RID: 28
	internal class ScriptingInterfaceOfIMBPeer : IMBPeer
	{
		// Token: 0x06000329 RID: 809 RVA: 0x0000D1C6 File Offset: 0x0000B3C6
		public void BeginModuleEvent(int index, bool isReliable)
		{
			ScriptingInterfaceOfIMBPeer.call_BeginModuleEventDelegate(index, isReliable);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		public void DebugRefreshDisconnectTimeout(int index)
		{
			ScriptingInterfaceOfIMBPeer.call_DebugRefreshDisconnectTimeoutDelegate(index);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D1E1 File Offset: 0x0000B3E1
		public void EndModuleEvent(bool isReliable)
		{
			ScriptingInterfaceOfIMBPeer.call_EndModuleEventDelegate(isReliable);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000D1EE File Offset: 0x0000B3EE
		public double GetAverageLossPercent(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetAverageLossPercentDelegate(index);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D1FB File Offset: 0x0000B3FB
		public double GetAveragePingInMilliseconds(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetAveragePingInMillisecondsDelegate(index);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D208 File Offset: 0x0000B408
		public uint GetHost(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetHostDelegate(index);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000D215 File Offset: 0x0000B415
		public bool GetIsSynchronized(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetIsSynchronizedDelegate(index);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000D222 File Offset: 0x0000B422
		public ushort GetPort(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetPortDelegate(index);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000D22F File Offset: 0x0000B42F
		public uint GetReversedHost(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetReversedHostDelegate(index);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000D23C File Offset: 0x0000B43C
		public bool IsActive(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_IsActiveDelegate(index);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000D249 File Offset: 0x0000B449
		public void SendExistingObjects(int index, UIntPtr missionPointer)
		{
			ScriptingInterfaceOfIMBPeer.call_SendExistingObjectsDelegate(index, missionPointer);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000D257 File Offset: 0x0000B457
		public void SetControlledAgent(int index, UIntPtr missionPointer, int agentIndex)
		{
			ScriptingInterfaceOfIMBPeer.call_SetControlledAgentDelegate(index, missionPointer, agentIndex);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000D266 File Offset: 0x0000B466
		public void SetIsSynchronized(int index, bool value)
		{
			ScriptingInterfaceOfIMBPeer.call_SetIsSynchronizedDelegate(index, value);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000D274 File Offset: 0x0000B474
		public void SetRelevantGameOptions(int index, bool sendMeBloodEvents, bool sendMeSoundEvents)
		{
			ScriptingInterfaceOfIMBPeer.call_SetRelevantGameOptionsDelegate(index, sendMeBloodEvents, sendMeSoundEvents);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000D283 File Offset: 0x0000B483
		public void SetTeam(int index, int teamIndex)
		{
			ScriptingInterfaceOfIMBPeer.call_SetTeamDelegate(index, teamIndex);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000D291 File Offset: 0x0000B491
		public void SetUserData(int index, MBNetworkPeer data)
		{
			ScriptingInterfaceOfIMBPeer.call_SetUserDataDelegate(index, (data != null) ? data.GetManagedId() : 0);
		}

		// Token: 0x0400029F RID: 671
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002A0 RID: 672
		public static ScriptingInterfaceOfIMBPeer.BeginModuleEventDelegate call_BeginModuleEventDelegate;

		// Token: 0x040002A1 RID: 673
		public static ScriptingInterfaceOfIMBPeer.DebugRefreshDisconnectTimeoutDelegate call_DebugRefreshDisconnectTimeoutDelegate;

		// Token: 0x040002A2 RID: 674
		public static ScriptingInterfaceOfIMBPeer.EndModuleEventDelegate call_EndModuleEventDelegate;

		// Token: 0x040002A3 RID: 675
		public static ScriptingInterfaceOfIMBPeer.GetAverageLossPercentDelegate call_GetAverageLossPercentDelegate;

		// Token: 0x040002A4 RID: 676
		public static ScriptingInterfaceOfIMBPeer.GetAveragePingInMillisecondsDelegate call_GetAveragePingInMillisecondsDelegate;

		// Token: 0x040002A5 RID: 677
		public static ScriptingInterfaceOfIMBPeer.GetHostDelegate call_GetHostDelegate;

		// Token: 0x040002A6 RID: 678
		public static ScriptingInterfaceOfIMBPeer.GetIsSynchronizedDelegate call_GetIsSynchronizedDelegate;

		// Token: 0x040002A7 RID: 679
		public static ScriptingInterfaceOfIMBPeer.GetPortDelegate call_GetPortDelegate;

		// Token: 0x040002A8 RID: 680
		public static ScriptingInterfaceOfIMBPeer.GetReversedHostDelegate call_GetReversedHostDelegate;

		// Token: 0x040002A9 RID: 681
		public static ScriptingInterfaceOfIMBPeer.IsActiveDelegate call_IsActiveDelegate;

		// Token: 0x040002AA RID: 682
		public static ScriptingInterfaceOfIMBPeer.SendExistingObjectsDelegate call_SendExistingObjectsDelegate;

		// Token: 0x040002AB RID: 683
		public static ScriptingInterfaceOfIMBPeer.SetControlledAgentDelegate call_SetControlledAgentDelegate;

		// Token: 0x040002AC RID: 684
		public static ScriptingInterfaceOfIMBPeer.SetIsSynchronizedDelegate call_SetIsSynchronizedDelegate;

		// Token: 0x040002AD RID: 685
		public static ScriptingInterfaceOfIMBPeer.SetRelevantGameOptionsDelegate call_SetRelevantGameOptionsDelegate;

		// Token: 0x040002AE RID: 686
		public static ScriptingInterfaceOfIMBPeer.SetTeamDelegate call_SetTeamDelegate;

		// Token: 0x040002AF RID: 687
		public static ScriptingInterfaceOfIMBPeer.SetUserDataDelegate call_SetUserDataDelegate;

		// Token: 0x020002FD RID: 765
		// (Invoke) Token: 0x06000EE4 RID: 3812
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BeginModuleEventDelegate(int index, [MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x020002FE RID: 766
		// (Invoke) Token: 0x06000EE8 RID: 3816
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DebugRefreshDisconnectTimeoutDelegate(int index);

		// Token: 0x020002FF RID: 767
		// (Invoke) Token: 0x06000EEC RID: 3820
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndModuleEventDelegate([MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x02000300 RID: 768
		// (Invoke) Token: 0x06000EF0 RID: 3824
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate double GetAverageLossPercentDelegate(int index);

		// Token: 0x02000301 RID: 769
		// (Invoke) Token: 0x06000EF4 RID: 3828
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate double GetAveragePingInMillisecondsDelegate(int index);

		// Token: 0x02000302 RID: 770
		// (Invoke) Token: 0x06000EF8 RID: 3832
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetHostDelegate(int index);

		// Token: 0x02000303 RID: 771
		// (Invoke) Token: 0x06000EFC RID: 3836
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetIsSynchronizedDelegate(int index);

		// Token: 0x02000304 RID: 772
		// (Invoke) Token: 0x06000F00 RID: 3840
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ushort GetPortDelegate(int index);

		// Token: 0x02000305 RID: 773
		// (Invoke) Token: 0x06000F04 RID: 3844
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetReversedHostDelegate(int index);

		// Token: 0x02000306 RID: 774
		// (Invoke) Token: 0x06000F08 RID: 3848
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsActiveDelegate(int index);

		// Token: 0x02000307 RID: 775
		// (Invoke) Token: 0x06000F0C RID: 3852
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SendExistingObjectsDelegate(int index, UIntPtr missionPointer);

		// Token: 0x02000308 RID: 776
		// (Invoke) Token: 0x06000F10 RID: 3856
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetControlledAgentDelegate(int index, UIntPtr missionPointer, int agentIndex);

		// Token: 0x02000309 RID: 777
		// (Invoke) Token: 0x06000F14 RID: 3860
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetIsSynchronizedDelegate(int index, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200030A RID: 778
		// (Invoke) Token: 0x06000F18 RID: 3864
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRelevantGameOptionsDelegate(int index, [MarshalAs(UnmanagedType.U1)] bool sendMeBloodEvents, [MarshalAs(UnmanagedType.U1)] bool sendMeSoundEvents);

		// Token: 0x0200030B RID: 779
		// (Invoke) Token: 0x06000F1C RID: 3868
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTeamDelegate(int index, int teamIndex);

		// Token: 0x0200030C RID: 780
		// (Invoke) Token: 0x06000F20 RID: 3872
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUserDataDelegate(int index, int data);
	}
}
