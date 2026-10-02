using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005D RID: 93
	public interface ICampaignMission
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000915 RID: 2325
		GameState State { get; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000916 RID: 2326
		IMissionTroopSupplier AgentSupplier { get; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000917 RID: 2327
		// (set) Token: 0x06000918 RID: 2328
		Location Location { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000919 RID: 2329
		// (set) Token: 0x0600091A RID: 2330
		Alley LastVisitedAlley { get; set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600091B RID: 2331
		MissionMode Mode { get; }

		// Token: 0x0600091C RID: 2332
		void SetMissionMode(MissionMode newMode, bool atStart);

		// Token: 0x0600091D RID: 2333
		void OnCloseEncounterMenu();

		// Token: 0x0600091E RID: 2334
		bool AgentLookingAtAgent(IAgent agent1, IAgent agent2);

		// Token: 0x0600091F RID: 2335
		void OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation);

		// Token: 0x06000920 RID: 2336
		void OnProcessSentence();

		// Token: 0x06000921 RID: 2337
		void OnConversationContinue();

		// Token: 0x06000922 RID: 2338
		bool CheckIfAgentCanFollow(IAgent agent);

		// Token: 0x06000923 RID: 2339
		void AddAgentFollowing(IAgent agent);

		// Token: 0x06000924 RID: 2340
		bool CheckIfAgentCanUnFollow(IAgent agent);

		// Token: 0x06000925 RID: 2341
		void RemoveAgentFollowing(IAgent agent);

		// Token: 0x06000926 RID: 2342
		void OnConversationPlay(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath);

		// Token: 0x06000927 RID: 2343
		void OnConversationStart(IAgent agent, bool setActionsInstantly);

		// Token: 0x06000928 RID: 2344
		void OnConversationEnd(IAgent agent);

		// Token: 0x06000929 RID: 2345
		void EndMission();

		// Token: 0x0600092A RID: 2346
		void FadeOutCharacter(CharacterObject characterObject);

		// Token: 0x0600092B RID: 2347
		void OnGameStateChanged();
	}
}
