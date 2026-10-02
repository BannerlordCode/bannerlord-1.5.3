using System;
using System.Linq;
using StoryMode.Quests.SecondPhase;
using StoryMode.Quests.SecondPhase.ConspiracyQuests;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000051 RID: 81
	public class SecondPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x0001C1C8 File Offset: 0x0001A3C8
		public SecondPhaseCampaignBehavior()
		{
			this._conspiracyQuestTriggerDayCounter = 0;
			this._isConspiracySetUpStarted = false;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001C1E0 File Offset: 0x0001A3E0
		public override void RegisterEvents()
		{
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(this.OnQuestStarted));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnGameEarlyLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameEarlyLoaded));
			CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomCreated));
			StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001C2A5 File Offset: 0x0001A4A5
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("_conspiracyQuestTriggerDayCounter", ref this._conspiracyQuestTriggerDayCounter);
			dataStore.SyncData<bool>("_isConspiracySetUpStarted", ref this._isConspiracySetUpStarted);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001C2CC File Offset: 0x0001A4CC
		private void WeeklyTick()
		{
			int num = 22;
			SecondPhase instance = SecondPhase.Instance;
			int num2 = num + MBRandom.RandomIntWithSeed((uint)((instance != null) ? instance.LastConspiracyQuestCreationTime.ToMilliseconds : 53.0), 2000U) % 8;
			if (this._isConspiracySetUpStarted && StoryModeManager.Current.MainStoryLine.ThirdPhase == null && SecondPhase.Instance.ConspiracyStrength < 2000f && SecondPhase.Instance.LastConspiracyQuestCreationTime.ElapsedDaysUntilNow >= (float)num2 && !this.IsThereActiveConspiracyQuest())
			{
				SecondPhase.Instance.CreateNextConspiracyQuest();
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001C35D File Offset: 0x0001A55D
		private void OnQuestStarted(QuestBase quest)
		{
			if (quest is AssembleEmpireQuestBehavior.AssembleEmpireQuest || quest is WeakenEmpireQuestBehavior.WeakenEmpireQuest)
			{
				StoryModeManager.Current.MainStoryLine.CompleteFirstPhase();
				this._isConspiracySetUpStarted = true;
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001C385 File Offset: 0x0001A585
		private void DailyTick()
		{
			if (this._isConspiracySetUpStarted && this._conspiracyQuestTriggerDayCounter < 10)
			{
				this._conspiracyQuestTriggerDayCounter++;
				if (this._conspiracyQuestTriggerDayCounter >= 10)
				{
					new ConspiracyProgressQuest().StartQuest();
				}
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001C3BB File Offset: 0x0001A5BB
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			SecondPhase instance = SecondPhase.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnSessionLaunched();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001C3CC File Offset: 0x0001A5CC
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			foreach (MobileParty mobileParty in Campaign.Current.CustomParties.ToList<MobileParty>())
			{
				if (mobileParty.Name.HasSameValue(new TextObject("{=eVzg5Mtl}Conspiracy Caravan", null)))
				{
					bool flag = true;
					foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests)
					{
						if (questBase.GetType() == typeof(DisruptSupplyLinesConspiracyQuest) && ((DisruptSupplyLinesConspiracyQuest)questBase).ConspiracyCaravan == mobileParty)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						DestroyPartyAction.Apply(null, mobileParty);
					}
				}
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001C4BC File Offset: 0x0001A6BC
		private void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
			if (SecondPhase.Instance != null && SecondPhase.Instance.ConspiracyClan == null)
			{
				SecondPhase.Instance.CreateConspiracyClan();
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001C4DC File Offset: 0x0001A6DC
		private void OnKingdomCreated(Kingdom createdKingdom)
		{
			if (StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted && !StoryModeManager.Current.MainStoryLine.IsSecondPhaseCompleted && StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine == StoryModeData.IsKingdomImperial(createdKingdom))
			{
				DeclareWarAction.ApplyByDefault(StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan, createdKingdom);
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001C53C File Offset: 0x0001A73C
		private void OnConspiracyActivated()
		{
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001C54C File Offset: 0x0001A74C
		private bool IsThereActiveConspiracyQuest()
		{
			foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests)
			{
				if (questBase.IsOngoing && typeof(ConspiracyQuestBase) == questBase.GetType().BaseType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040001D3 RID: 467
		private int _conspiracyQuestTriggerDayCounter;

		// Token: 0x040001D4 RID: 468
		private bool _isConspiracySetUpStarted;
	}
}
