using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x02000048 RID: 72
	public class MapMarkerTrackerItemVM : MapTrackerItemVM<MapMarker>
	{
		// Token: 0x06000498 RID: 1176 RVA: 0x0001279E File Offset: 0x0001099E
		public MapMarkerTrackerItemVM(MapMarker marker)
			: base(marker)
		{
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000127A7 File Offset: 0x000109A7
		protected override void OnShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(MapMarker), new object[] { base.TrackedObject, true, false });
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000127D9 File Offset: 0x000109D9
		protected override bool IsVisibleOnMap()
		{
			return base.TrackedObject.IsVisibleOnMap;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000127E6 File Offset: 0x000109E6
		protected override bool GetCanToggleTrack()
		{
			return true;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000127E9 File Offset: 0x000109E9
		protected override string GetTrackerType()
		{
			return "Default";
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000127F0 File Offset: 0x000109F0
		protected override CampaignUIHelper.IssueQuestFlags GetRelatedQuests()
		{
			CampaignUIHelper.IssueQuestFlags issueQuestFlags = CampaignUIHelper.IssueQuestFlags.None;
			QuestBase questBase = Campaign.Current.QuestManager.Quests.FirstOrDefault<QuestBase>((QuestBase q) => q.StringId == base.TrackedObject.QuestId);
			if (questBase != null)
			{
				issueQuestFlags = (questBase.IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest : CampaignUIHelper.IssueQuestFlags.ActiveIssue);
			}
			return issueQuestFlags;
		}
	}
}
