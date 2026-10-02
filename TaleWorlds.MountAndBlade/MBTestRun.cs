using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E6 RID: 486
	public class MBTestRun
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x000621F0 File Offset: 0x000603F0
		public static bool EnterEditMode()
		{
			return MBAPI.IMBTestRun.EnterEditMode();
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000621FC File Offset: 0x000603FC
		public static bool NewScene()
		{
			return MBAPI.IMBTestRun.NewScene();
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x00062208 File Offset: 0x00060408
		public static bool LeaveEditMode()
		{
			return MBAPI.IMBTestRun.LeaveEditMode();
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x00062214 File Offset: 0x00060414
		public static bool OpenScene(string sceneName)
		{
			return MBAPI.IMBTestRun.OpenScene(sceneName);
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x00062221 File Offset: 0x00060421
		public static bool CloseScene()
		{
			return MBAPI.IMBTestRun.CloseScene();
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x0006222D File Offset: 0x0006042D
		public static bool SaveScene()
		{
			return false;
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x00062230 File Offset: 0x00060430
		public static bool OpenDefaultScene()
		{
			return false;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x00062233 File Offset: 0x00060433
		public static int GetFPS()
		{
			return MBAPI.IMBTestRun.GetFPS();
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0006223F File Offset: 0x0006043F
		public static void StartMission()
		{
			MBAPI.IMBTestRun.StartMission();
		}
	}
}
