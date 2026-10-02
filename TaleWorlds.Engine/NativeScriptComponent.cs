using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000071 RID: 113
	[EngineClass("rglNative_script_component")]
	public sealed class NativeScriptComponent : ScriptComponent
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0000A927 File Offset: 0x00008B27
		internal NativeScriptComponent(UIntPtr pointer)
			: base(pointer)
		{
		}
	}
}
