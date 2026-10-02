using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C9 RID: 457
	public static class DebugExtensions
	{
		// Token: 0x06001B96 RID: 7062 RVA: 0x0006057A File Offset: 0x0005E77A
		public static void RenderDebugCircleOnTerrain(Scene scene, MatrixFrame frame, float radius, uint color, bool depthCheck = true, bool isDotted = false)
		{
			MBAPI.IMBDebugExtensions.RenderDebugCircleOnTerrain(scene.Pointer, ref frame, radius, color, depthCheck, isDotted);
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00060594 File Offset: 0x0005E794
		public static void RenderDebugArcOnTerrain(Scene scene, MatrixFrame frame, float radius, float beginAngle, float endAngle, uint color, bool depthCheck = true, bool isDotted = false)
		{
			MBAPI.IMBDebugExtensions.RenderDebugArcOnTerrain(scene.Pointer, ref frame, radius, beginAngle, endAngle, color, depthCheck, isDotted);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000605C0 File Offset: 0x0005E7C0
		public static void RenderDebugLineOnTerrain(Scene scene, Vec3 position, Vec3 direction, uint color, bool depthCheck = true, float time = 0f, bool isDotted = false, float pointDensity = 1f)
		{
			MBAPI.IMBDebugExtensions.RenderDebugLineOnTerrain(scene.Pointer, position, direction, color, depthCheck, time, isDotted, pointDensity);
		}
	}
}
