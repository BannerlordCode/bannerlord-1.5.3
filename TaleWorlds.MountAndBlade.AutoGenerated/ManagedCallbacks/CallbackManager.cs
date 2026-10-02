using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;

namespace ManagedCallbacks
{
	// Token: 0x02000006 RID: 6
	public class CallbackManager : ICallbackManager
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002566 File Offset: 0x00000766
		public void Initialize()
		{
			CoreCallbacksGenerated.Initialize();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000256D File Offset: 0x0000076D
		public Delegate[] GetDelegates()
		{
			return CoreCallbacksGenerated.Delegates;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002574 File Offset: 0x00000774
		public Dictionary<string, object> GetScriptingInterfaceObjects()
		{
			return ScriptingInterfaceObjects.GetObjects();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000257B File Offset: 0x0000077B
		public void SetFunctionPointer(int id, IntPtr pointer)
		{
			ScriptingInterfaceObjects.SetFunctionPointer(id, pointer);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002584 File Offset: 0x00000784
		public void CheckSharedStructureSizes()
		{
		}
	}
}
