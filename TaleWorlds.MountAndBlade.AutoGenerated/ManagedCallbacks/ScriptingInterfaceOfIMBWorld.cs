using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000024 RID: 36
	internal class ScriptingInterfaceOfIMBWorld : IMBWorld
	{
		// Token: 0x0600037A RID: 890 RVA: 0x0000DAA5 File Offset: 0x0000BCA5
		public void CheckResourceModifications()
		{
			ScriptingInterfaceOfIMBWorld.call_CheckResourceModificationsDelegate();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000DAB1 File Offset: 0x0000BCB1
		public void FixSkeletons()
		{
			ScriptingInterfaceOfIMBWorld.call_FixSkeletonsDelegate();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000DABD File Offset: 0x0000BCBD
		public int GetGameType()
		{
			return ScriptingInterfaceOfIMBWorld.call_GetGameTypeDelegate();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000DAC9 File Offset: 0x0000BCC9
		public float GetGlobalTime(MBCommon.TimeType timeType)
		{
			return ScriptingInterfaceOfIMBWorld.call_GetGlobalTimeDelegate(timeType);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000DAD6 File Offset: 0x0000BCD6
		public string GetLastMessages()
		{
			if (ScriptingInterfaceOfIMBWorld.call_GetLastMessagesDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000DAEC File Offset: 0x0000BCEC
		public void PauseGame()
		{
			ScriptingInterfaceOfIMBWorld.call_PauseGameDelegate();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		public void SetBodyUsed(string bodyName)
		{
			byte[] array = null;
			if (bodyName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBWorld._utf8.GetByteCount(bodyName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBWorld._utf8.GetBytes(bodyName, 0, bodyName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBWorld.call_SetBodyUsedDelegate(array);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000DB52 File Offset: 0x0000BD52
		public void SetGameType(int gameType)
		{
			ScriptingInterfaceOfIMBWorld.call_SetGameTypeDelegate(gameType);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000DB60 File Offset: 0x0000BD60
		public void SetMaterialUsed(string materialName)
		{
			byte[] array = null;
			if (materialName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBWorld._utf8.GetByteCount(materialName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBWorld._utf8.GetBytes(materialName, 0, materialName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBWorld.call_SetMaterialUsedDelegate(array);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000DBBC File Offset: 0x0000BDBC
		public void SetMeshUsed(string meshName)
		{
			byte[] array = null;
			if (meshName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBWorld._utf8.GetByteCount(meshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBWorld._utf8.GetBytes(meshName, 0, meshName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBWorld.call_SetMeshUsedDelegate(array);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000DC16 File Offset: 0x0000BE16
		public void UnpauseGame()
		{
			ScriptingInterfaceOfIMBWorld.call_UnpauseGameDelegate();
		}

		// Token: 0x040002E4 RID: 740
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002E5 RID: 741
		public static ScriptingInterfaceOfIMBWorld.CheckResourceModificationsDelegate call_CheckResourceModificationsDelegate;

		// Token: 0x040002E6 RID: 742
		public static ScriptingInterfaceOfIMBWorld.FixSkeletonsDelegate call_FixSkeletonsDelegate;

		// Token: 0x040002E7 RID: 743
		public static ScriptingInterfaceOfIMBWorld.GetGameTypeDelegate call_GetGameTypeDelegate;

		// Token: 0x040002E8 RID: 744
		public static ScriptingInterfaceOfIMBWorld.GetGlobalTimeDelegate call_GetGlobalTimeDelegate;

		// Token: 0x040002E9 RID: 745
		public static ScriptingInterfaceOfIMBWorld.GetLastMessagesDelegate call_GetLastMessagesDelegate;

		// Token: 0x040002EA RID: 746
		public static ScriptingInterfaceOfIMBWorld.PauseGameDelegate call_PauseGameDelegate;

		// Token: 0x040002EB RID: 747
		public static ScriptingInterfaceOfIMBWorld.SetBodyUsedDelegate call_SetBodyUsedDelegate;

		// Token: 0x040002EC RID: 748
		public static ScriptingInterfaceOfIMBWorld.SetGameTypeDelegate call_SetGameTypeDelegate;

		// Token: 0x040002ED RID: 749
		public static ScriptingInterfaceOfIMBWorld.SetMaterialUsedDelegate call_SetMaterialUsedDelegate;

		// Token: 0x040002EE RID: 750
		public static ScriptingInterfaceOfIMBWorld.SetMeshUsedDelegate call_SetMeshUsedDelegate;

		// Token: 0x040002EF RID: 751
		public static ScriptingInterfaceOfIMBWorld.UnpauseGameDelegate call_UnpauseGameDelegate;

		// Token: 0x0200033A RID: 826
		// (Invoke) Token: 0x06000FD8 RID: 4056
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CheckResourceModificationsDelegate();

		// Token: 0x0200033B RID: 827
		// (Invoke) Token: 0x06000FDC RID: 4060
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FixSkeletonsDelegate();

		// Token: 0x0200033C RID: 828
		// (Invoke) Token: 0x06000FE0 RID: 4064
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetGameTypeDelegate();

		// Token: 0x0200033D RID: 829
		// (Invoke) Token: 0x06000FE4 RID: 4068
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetGlobalTimeDelegate(MBCommon.TimeType timeType);

		// Token: 0x0200033E RID: 830
		// (Invoke) Token: 0x06000FE8 RID: 4072
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetLastMessagesDelegate();

		// Token: 0x0200033F RID: 831
		// (Invoke) Token: 0x06000FEC RID: 4076
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseGameDelegate();

		// Token: 0x02000340 RID: 832
		// (Invoke) Token: 0x06000FF0 RID: 4080
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBodyUsedDelegate(byte[] bodyName);

		// Token: 0x02000341 RID: 833
		// (Invoke) Token: 0x06000FF4 RID: 4084
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGameTypeDelegate(int gameType);

		// Token: 0x02000342 RID: 834
		// (Invoke) Token: 0x06000FF8 RID: 4088
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaterialUsedDelegate(byte[] materialName);

		// Token: 0x02000343 RID: 835
		// (Invoke) Token: 0x06000FFC RID: 4092
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMeshUsedDelegate(byte[] meshName);

		// Token: 0x02000344 RID: 836
		// (Invoke) Token: 0x06001000 RID: 4096
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnpauseGameDelegate();
	}
}
