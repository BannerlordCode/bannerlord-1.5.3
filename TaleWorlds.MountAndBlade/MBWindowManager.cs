using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E8 RID: 488
	public class MBWindowManager
	{
		// Token: 0x06001CCC RID: 7372 RVA: 0x00062282 File Offset: 0x00060482
		public static float WorldToScreen(Camera camera, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return MBAPI.IMBWindowManager.WorldToScreen(camera.Pointer, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0006229C File Offset: 0x0006049C
		public static float WorldToScreenInsideUsableArea(Camera camera, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			float num = MBAPI.IMBWindowManager.WorldToScreen(camera.Pointer, worldSpacePosition, ref screenX, ref screenY, ref w);
			screenX -= (Screen.RealScreenResolutionWidth - ScreenManager.UsableArea.X * Screen.RealScreenResolutionWidth) / 2f;
			screenY -= (Screen.RealScreenResolutionHeight - ScreenManager.UsableArea.Y * Screen.RealScreenResolutionHeight) / 2f;
			return num;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00062306 File Offset: 0x00060506
		public static float WorldToScreenWithFixedZ(Camera camera, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return MBAPI.IMBWindowManager.WorldToScreenWithFixedZ(camera.Pointer, cameraPosition, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0006231F File Offset: 0x0006051F
		public static void ScreenToWorld(Camera camera, float screenX, float screenY, float w, ref Vec3 worldSpacePosition)
		{
			MBAPI.IMBWindowManager.ScreenToWorld(camera.Pointer, screenX, screenY, w, ref worldSpacePosition);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x00062336 File Offset: 0x00060536
		public static Vec2 GetScreenResolution()
		{
			return MBAPI.IMBWindowManager.GetScreenResolution();
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x00062342 File Offset: 0x00060542
		public static void PreDisplay()
		{
			MBAPI.IMBWindowManager.PreDisplay();
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0006234E File Offset: 0x0006054E
		public static void DontChangeCursorPos()
		{
			MBAPI.IMBWindowManager.DontChangeCursorPos();
		}
	}
}
