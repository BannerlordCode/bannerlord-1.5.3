using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000087 RID: 135
	[EngineClass("rglScript_component")]
	public abstract class ScriptComponent : NativeObject
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x0000D946 File Offset: 0x0000BB46
		protected ScriptComponent()
		{
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0000D94E File Offset: 0x0000BB4E
		internal ScriptComponent(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0000D95D File Offset: 0x0000BB5D
		public string GetName()
		{
			return EngineApplicationInterface.IScriptComponent.GetName(base.Pointer);
		}
	}
}
