using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000020 RID: 32
	internal class ScriptingInterfaceOfIMBTeam : IMBTeam
	{
		// Token: 0x0600035C RID: 860 RVA: 0x0000D7D5 File Offset: 0x0000B9D5
		public bool IsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex)
		{
			return ScriptingInterfaceOfIMBTeam.call_IsEnemyDelegate(missionPointer, teamIndex, otherTeamIndex);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
		public void SetIsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex, bool isEnemy)
		{
			ScriptingInterfaceOfIMBTeam.call_SetIsEnemyDelegate(missionPointer, teamIndex, otherTeamIndex, isEnemy);
		}

		// Token: 0x040002CA RID: 714
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002CB RID: 715
		public static ScriptingInterfaceOfIMBTeam.IsEnemyDelegate call_IsEnemyDelegate;

		// Token: 0x040002CC RID: 716
		public static ScriptingInterfaceOfIMBTeam.SetIsEnemyDelegate call_SetIsEnemyDelegate;

		// Token: 0x02000324 RID: 804
		// (Invoke) Token: 0x06000F80 RID: 3968
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEnemyDelegate(UIntPtr missionPointer, int teamIndex, int otherTeamIndex);

		// Token: 0x02000325 RID: 805
		// (Invoke) Token: 0x06000F84 RID: 3972
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetIsEnemyDelegate(UIntPtr missionPointer, int teamIndex, int otherTeamIndex, [MarshalAs(UnmanagedType.U1)] bool isEnemy);
	}
}
