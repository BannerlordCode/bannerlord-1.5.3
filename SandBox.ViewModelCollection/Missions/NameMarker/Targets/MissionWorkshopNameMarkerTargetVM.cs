using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x0200003C RID: 60
	public class MissionWorkshopNameMarkerTargetVM : MissionNameMarkerTargetVM<Workshop>
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x00011A55 File Offset: 0x0000FC55
		public MissionWorkshopNameMarkerTargetVM(Workshop target, Vec3 signPosition)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = target.WorkshopType.StringId;
			this._signPosition = signPosition;
			this.RefreshValues();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00011A87 File Offset: 0x0000FC87
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._signPosition + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00011AA0 File Offset: 0x0000FCA0
		protected override TextObject GetName()
		{
			return base.Target.WorkshopType.Name;
		}

		// Token: 0x04000229 RID: 553
		private readonly Vec3 _signPosition;
	}
}
