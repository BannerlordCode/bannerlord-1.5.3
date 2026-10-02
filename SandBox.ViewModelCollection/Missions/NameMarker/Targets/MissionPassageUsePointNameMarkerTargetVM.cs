using System;
using SandBox.Objects;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x0200003B RID: 59
	public class MissionPassageUsePointNameMarkerTargetVM : MissionNameMarkerTargetVM<PassageUsePoint>
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x00011984 File Offset: 0x0000FB84
		public MissionPassageUsePointNameMarkerTargetVM(PassageUsePoint target)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = ((base.Target.ToLocation == null && base.Target.IsMissionExit) ? "center" : base.Target.ToLocation.StringId);
			base.Quests = new MBBindingList<QuestMarkerVM>();
			this.RefreshValues();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x000119EC File Offset: 0x0000FBEC
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, base.Target.GameEntity.GlobalPosition + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00011A1D File Offset: 0x0000FC1D
		protected override TextObject GetName()
		{
			if (base.Target.ToLocation == null && base.Target.IsMissionExit)
			{
				return GameTexts.FindText("str_mission_exit", null);
			}
			return base.Target.ToLocation.Name;
		}
	}
}
