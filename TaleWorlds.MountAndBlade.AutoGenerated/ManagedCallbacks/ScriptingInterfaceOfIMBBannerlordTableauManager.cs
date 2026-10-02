using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200000F RID: 15
	internal class ScriptingInterfaceOfIMBBannerlordTableauManager : IMBBannerlordTableauManager
	{
		// Token: 0x06000201 RID: 513 RVA: 0x0000B026 File Offset: 0x00009226
		public int GetNumberOfPendingTableauRequests()
		{
			return ScriptingInterfaceOfIMBBannerlordTableauManager.call_GetNumberOfPendingTableauRequestsDelegate();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000B032 File Offset: 0x00009232
		public void InitializeCharacterTableauRenderSystem()
		{
			ScriptingInterfaceOfIMBBannerlordTableauManager.call_InitializeCharacterTableauRenderSystemDelegate();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000B040 File Offset: 0x00009240
		public void RequestCharacterTableauRender(int characterCodeId, string path, UIntPtr poseEntity, UIntPtr cameraObject, int tableauType)
		{
			byte[] array = null;
			if (path != null)
			{
				int byteCount = ScriptingInterfaceOfIMBBannerlordTableauManager._utf8.GetByteCount(path);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBBannerlordTableauManager._utf8.GetBytes(path, 0, path.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBBannerlordTableauManager.call_RequestCharacterTableauRenderDelegate(characterCodeId, array, poseEntity, cameraObject, tableauType);
		}

		// Token: 0x0400018B RID: 395
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400018C RID: 396
		public static ScriptingInterfaceOfIMBBannerlordTableauManager.GetNumberOfPendingTableauRequestsDelegate call_GetNumberOfPendingTableauRequestsDelegate;

		// Token: 0x0400018D RID: 397
		public static ScriptingInterfaceOfIMBBannerlordTableauManager.InitializeCharacterTableauRenderSystemDelegate call_InitializeCharacterTableauRenderSystemDelegate;

		// Token: 0x0400018E RID: 398
		public static ScriptingInterfaceOfIMBBannerlordTableauManager.RequestCharacterTableauRenderDelegate call_RequestCharacterTableauRenderDelegate;

		// Token: 0x020001F6 RID: 502
		// (Invoke) Token: 0x06000AC8 RID: 2760
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfPendingTableauRequestsDelegate();

		// Token: 0x020001F7 RID: 503
		// (Invoke) Token: 0x06000ACC RID: 2764
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeCharacterTableauRenderSystemDelegate();

		// Token: 0x020001F8 RID: 504
		// (Invoke) Token: 0x06000AD0 RID: 2768
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RequestCharacterTableauRenderDelegate(int characterCodeId, byte[] path, UIntPtr poseEntity, UIntPtr cameraObject, int tableauType);
	}
}
