using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x02000038 RID: 56
	public class MissionBasicAreaIndicatorMarkerTargetVM : MissionNameMarkerTargetVM<BasicAreaIndicator>
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x00011740 File Offset: 0x0000F940
		public MissionBasicAreaIndicatorMarkerTargetVM(BasicAreaIndicator target, Vec3 position)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = (string.IsNullOrEmpty(base.Target.Type) ? "common_area" : base.Target.Type);
			this._position = position;
			this.RefreshValues();
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00011796 File Offset: 0x0000F996
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._position + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000117AF File Offset: 0x0000F9AF
		protected override TextObject GetName()
		{
			return base.Target.GetName();
		}

		// Token: 0x04000224 RID: 548
		private readonly Vec3 _position;
	}
}
