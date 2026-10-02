using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C1 RID: 449
	[ScriptingInterfaceBase]
	internal interface IMBDebugExtensions
	{
		// Token: 0x0600196D RID: 6509
		[EngineMethod("render_debug_circle_on_terrain", false, null, false)]
		void RenderDebugCircleOnTerrain(UIntPtr scenePointer, ref MatrixFrame frame, float radius, uint color, bool depthCheck, bool isDotted);

		// Token: 0x0600196E RID: 6510
		[EngineMethod("render_debug_arc_on_terrain", false, null, false)]
		void RenderDebugArcOnTerrain(UIntPtr scenePointer, ref MatrixFrame frame, float radius, float beginAngle, float endAngle, uint color, bool depthCheck, bool isDotted);

		// Token: 0x0600196F RID: 6511
		[EngineMethod("render_debug_line_on_terrain", false, null, false)]
		void RenderDebugLineOnTerrain(UIntPtr scenePointer, Vec3 position, Vec3 direction, uint color, bool depthCheck, float time, bool isDotted, float pointDensity);

		// Token: 0x06001970 RID: 6512
		[EngineMethod("override_native_parameter", false, null, false)]
		void OverrideNativeParameter(string paramName, float value);

		// Token: 0x06001971 RID: 6513
		[EngineMethod("reload_native_parameters", false, null, false)]
		void ReloadNativeParameters();
	}
}
