using System;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout
{
	// Token: 0x02000040 RID: 64
	public class MissionStealthSentryNameMarkerTargetVM : MissionNameMarkerTargetVM<Agent>
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x00011CB7 File Offset: 0x0000FEB7
		public MissionStealthSentryNameMarkerTargetVM(Agent target)
			: base(target)
		{
			base.IconType = "sentry";
			base.NameType = "Enemy";
			base.IsEnemy = true;
			this.RefreshValues();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00011CE3 File Offset: 0x0000FEE3
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, base.Target.GetEyeGlobalPosition() + MissionNameMarkerHelper.AgentHeightOffset);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00011D01 File Offset: 0x0000FF01
		protected override TextObject GetName()
		{
			return new TextObject("{=KdT0PM8Y}Sentry", null);
		}
	}
}
