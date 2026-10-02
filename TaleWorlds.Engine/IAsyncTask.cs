using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200002D RID: 45
	[ApplicationInterfaceBase]
	internal interface IAsyncTask
	{
		// Token: 0x06000502 RID: 1282
		[EngineMethod("create_with_function", false, null, false)]
		AsyncTask CreateWithDelegate(ManagedDelegate function, bool isBackground);

		// Token: 0x06000503 RID: 1283
		[EngineMethod("invoke", false, null, false)]
		void Invoke(UIntPtr Pointer);

		// Token: 0x06000504 RID: 1284
		[EngineMethod("wait", false, null, false)]
		void Wait(UIntPtr Pointer);
	}
}
