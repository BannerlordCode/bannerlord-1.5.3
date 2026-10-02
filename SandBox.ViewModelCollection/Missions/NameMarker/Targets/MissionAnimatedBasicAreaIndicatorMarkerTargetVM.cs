using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x02000037 RID: 55
	public class MissionAnimatedBasicAreaIndicatorMarkerTargetVM : MissionNameMarkerTargetVM<AnimatedBasicAreaIndicator>
	{
		// Token: 0x0600041B RID: 1051 RVA: 0x000116C4 File Offset: 0x0000F8C4
		public MissionAnimatedBasicAreaIndicatorMarkerTargetVM(AnimatedBasicAreaIndicator target)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = (string.IsNullOrEmpty(base.Target.Type) ? "common_area" : base.Target.Type);
			this.RefreshValues();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00011713 File Offset: 0x0000F913
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, base.Target.GetPosition() + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00011731 File Offset: 0x0000F931
		protected override TextObject GetName()
		{
			return base.Target.GetName();
		}
	}
}
