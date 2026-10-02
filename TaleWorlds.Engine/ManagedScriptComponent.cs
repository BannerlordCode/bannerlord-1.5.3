using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005C RID: 92
	[EngineClass("rglManaged_script_component")]
	public sealed class ManagedScriptComponent : ScriptComponent
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x000081DD File Offset: 0x000063DD
		public ScriptComponentBehavior ScriptComponentBehavior
		{
			get
			{
				return EngineApplicationInterface.IScriptComponent.GetScriptComponentBehavior(base.Pointer);
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000081EF File Offset: 0x000063EF
		public void SetVariableEditorWidgetStatus(string field, bool enabled)
		{
			EngineApplicationInterface.IScriptComponent.SetVariableEditorWidgetStatus(base.Pointer, field, enabled);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00008203 File Offset: 0x00006403
		public void SetVariableEditorWidgetValue(string field, RglScriptFieldType fieldType, double value)
		{
			EngineApplicationInterface.IScriptComponent.SetVariableEditorWidgetValue(base.Pointer, field, fieldType, value);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00008218 File Offset: 0x00006418
		private ManagedScriptComponent()
		{
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00008220 File Offset: 0x00006420
		internal ManagedScriptComponent(UIntPtr pointer)
			: base(pointer)
		{
		}
	}
}
