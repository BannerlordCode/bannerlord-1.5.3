using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x02000049 RID: 73
	public class MapMobilePartyTrackItemVM : MapTrackerItemVM<MobileParty>
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x00012849 File Offset: 0x00010A49
		public MapMobilePartyTrackItemVM(MobileParty party)
			: base(party)
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00012852 File Offset: 0x00010A52
		protected override void OnShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(MobileParty), new object[] { base.TrackedObject, true, false });
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00012884 File Offset: 0x00010A84
		protected override bool IsVisibleOnMap()
		{
			return base.TrackedObject.AttachedTo == null && !base.TrackedObject.IsVisible;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000128A3 File Offset: 0x00010AA3
		protected override bool GetCanToggleTrack()
		{
			return true;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000128A6 File Offset: 0x00010AA6
		protected override string GetTrackerType()
		{
			return "MobileParty";
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000128AD File Offset: 0x00010AAD
		protected override CampaignUIHelper.IssueQuestFlags GetRelatedQuests()
		{
			return CampaignUIHelper.IssueQuestFlags.None;
		}
	}
}
