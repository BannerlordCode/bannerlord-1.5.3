using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000016 RID: 22
	public class EngineCallback : ManagedFromNativeCallback
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00003C5F File Offset: 0x00001E5F
		public EngineCallback(string[] conditionals = null, bool isMultiThreadCallable = false)
			: base(conditionals, isMultiThreadCallable)
		{
		}
	}
}
