using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200002B RID: 43
	[ApplicationInterfaceBase]
	internal interface IScriptComponent
	{
		// Token: 0x060004EE RID: 1262
		[EngineMethod("get_script_component_behavior", false, null, false)]
		ScriptComponentBehavior GetScriptComponentBehavior(UIntPtr pointer);

		// Token: 0x060004EF RID: 1263
		[EngineMethod("set_variable_editor_widget_status", false, null, false)]
		void SetVariableEditorWidgetStatus(UIntPtr pointer, string field, bool enabled);

		// Token: 0x060004F0 RID: 1264
		[EngineMethod("set_variable_editor_widget_value", false, null, false)]
		void SetVariableEditorWidgetValue(UIntPtr pointer, string field, RglScriptFieldType fieldType, double value);

		// Token: 0x060004F1 RID: 1265
		[EngineMethod("get_name", false, null, false)]
		string GetName(UIntPtr pointer);
	}
}
