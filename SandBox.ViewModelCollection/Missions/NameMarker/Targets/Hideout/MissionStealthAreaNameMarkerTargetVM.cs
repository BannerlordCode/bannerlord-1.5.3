using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout
{
	// Token: 0x0200003D RID: 61
	public class MissionStealthAreaNameMarkerTargetVM : MissionNameMarkerTargetVM<StealthAreaMarker>
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x00011AB2 File Offset: 0x0000FCB2
		public MissionStealthAreaNameMarkerTargetVM(StealthAreaMarker target, Vec3 position)
			: base(target)
		{
			this._position = position;
			base.NameType = "Passage";
			base.IconType = "stealth_area";
			this.RefreshValues();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00011ADE File Offset: 0x0000FCDE
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._position + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00011AF7 File Offset: 0x0000FCF7
		protected override TextObject GetName()
		{
			return new TextObject("{=WcSky2KB}Stealth Area", null);
		}

		// Token: 0x0400022A RID: 554
		private readonly Vec3 _position;
	}
}
