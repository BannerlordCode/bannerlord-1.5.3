using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x02000047 RID: 71
	public class MapArmyTrackItemVM : MapTrackerItemVM<Army>
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x0001273B File Offset: 0x0001093B
		public MapArmyTrackItemVM(Army trackableObject)
			: base(trackableObject)
		{
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00012744 File Offset: 0x00010944
		protected override void OnShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(Army), new object[] { base.TrackedObject, true, false });
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00012776 File Offset: 0x00010976
		protected override bool IsVisibleOnMap()
		{
			MobileParty leaderParty = base.TrackedObject.LeaderParty;
			return leaderParty != null && !leaderParty.IsVisible;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00012791 File Offset: 0x00010991
		protected override bool GetCanToggleTrack()
		{
			return true;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00012794 File Offset: 0x00010994
		protected override string GetTrackerType()
		{
			return "Army";
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001279B File Offset: 0x0001099B
		protected override CampaignUIHelper.IssueQuestFlags GetRelatedQuests()
		{
			return CampaignUIHelper.IssueQuestFlags.None;
		}
	}
}
