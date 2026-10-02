using System;
using SandBox.Objects.Usables;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000029 RID: 41
	public class StealthMissionUIHandler : MissionView
	{
		// Token: 0x06000113 RID: 275 RVA: 0x0000CAF5 File Offset: 0x0000ACF5
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			base.OnObjectUsed(userAgent, usedObject);
			if (usedObject is StealthAreaUsePoint)
			{
				this.CameraFadeInFadeOut(0.5f, 0.5f, 1f);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		private void CameraFadeInFadeOut(float fadeOutTime, float blackTime, float fadeInTime)
		{
			if (!ScreenFadeController.IsFadeActive)
			{
				ScreenFadeController.BeginFadeOutAndIn(fadeOutTime, blackTime, fadeInTime);
			}
		}
	}
}
