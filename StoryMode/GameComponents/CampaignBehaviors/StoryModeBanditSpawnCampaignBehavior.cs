using System;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000052 RID: 82
	public class StoryModeBanditSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x0001C5CC File Offset: 0x0001A7CC
		public override void RegisterEvents()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				StoryModeEvents.OnStoryModeTutorialEndedEvent.AddNonSerializedListener(this, new Action(this.OnTutorialEnded));
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001C5F1 File Offset: 0x0001A7F1
		private void OnTutorialEnded()
		{
			if (TutorialPhase.Instance.IsSkipped)
			{
				this.SpawnInitialBanditsAndLooters();
			}
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001C610 File Offset: 0x0001A810
		private void SpawnInitialBanditsAndLooters()
		{
			BanditSpawnCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<BanditSpawnCampaignBehavior>();
			if (campaignBehavior != null)
			{
				campaignBehavior.InitializeInitialHideouts();
				campaignBehavior.SpawnBanditsAroundHideoutAtNewGame();
				campaignBehavior.SpawnLootersAtNewGame();
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001C63D File Offset: 0x0001A83D
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
