using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews.SiegeWeapon
{
	// Token: 0x020000A1 RID: 161
	public class BallistaView : RangedSiegeWeaponView
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x00028989 File Offset: 0x00026B89
		protected override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			this.UsesMouseForAiming = true;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00028999 File Offset: 0x00026B99
		protected override void StartUsingWeaponCamera()
		{
			base.StartUsingWeaponCamera();
			base.MissionScreen.SetExtraCameraParameters(true, 1.5f);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000289B2 File Offset: 0x00026BB2
		protected override void HandleUserCameraRotation(float dt)
		{
		}
	}
}
