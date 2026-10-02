using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews.SiegeWeapon
{
	// Token: 0x020000A5 RID: 165
	[DefaultView]
	public class RangedSiegeWeaponViewController : MissionView
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x00028FDC File Offset: 0x000271DC
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			base.OnObjectUsed(userAgent, usedObject);
			if (userAgent.IsMainAgent && usedObject is StandingPoint)
			{
				UsableMachine usableMachineFromPoint = this.GetUsableMachineFromPoint(usedObject as StandingPoint);
				if (usableMachineFromPoint is RangedSiegeWeapon)
				{
					RangedSiegeWeapon rangedSiegeWeapon = usableMachineFromPoint as RangedSiegeWeapon;
					if (rangedSiegeWeapon.GetComponent<RangedSiegeWeaponView>() == null)
					{
						this.AddRangedSiegeWeaponView(rangedSiegeWeapon);
					}
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0002902C File Offset: 0x0002722C
		private UsableMachine GetUsableMachineFromPoint(StandingPoint standingPoint)
		{
			WeakGameEntity weakGameEntity = standingPoint.GameEntity;
			while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			if (weakGameEntity.IsValid)
			{
				UsableMachine firstScriptOfType = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType != null)
				{
					return firstScriptOfType;
				}
			}
			return null;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00029074 File Offset: 0x00027274
		private void AddRangedSiegeWeaponView(RangedSiegeWeapon rangedSiegeWeapon)
		{
			RangedSiegeWeaponView rangedSiegeWeaponView;
			if (rangedSiegeWeapon is Trebuchet)
			{
				rangedSiegeWeaponView = new TrebuchetView();
			}
			else if (rangedSiegeWeapon is Mangonel)
			{
				rangedSiegeWeaponView = new MangonelView();
			}
			else if (rangedSiegeWeapon is Ballista)
			{
				rangedSiegeWeaponView = new BallistaView();
			}
			else
			{
				rangedSiegeWeaponView = new RangedSiegeWeaponView();
			}
			rangedSiegeWeaponView.Initialize(rangedSiegeWeapon, base.MissionScreen);
			rangedSiegeWeapon.AddComponent(rangedSiegeWeaponView);
		}
	}
}
