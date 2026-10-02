using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E0 RID: 736
	public class MBCallback : ManagedFromNativeCallback
	{
		// Token: 0x06002B25 RID: 11045 RVA: 0x000A65C9 File Offset: 0x000A47C9
		public MBCallback(string[] conditionals = null, bool isMultiThreadCallable = false)
			: base(conditionals, isMultiThreadCallable)
		{
		}
	}
}
