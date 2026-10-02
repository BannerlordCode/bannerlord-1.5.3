using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B3 RID: 947
	public interface IMapStateHandler
	{
		// Token: 0x0600372B RID: 14123
		void OnRefreshState();

		// Token: 0x0600372C RID: 14124
		void OnMainPartyEncounter();

		// Token: 0x0600372D RID: 14125
		void OnIncidentStarted(Incident incident);

		// Token: 0x0600372E RID: 14126
		void BeforeTick(float dt);

		// Token: 0x0600372F RID: 14127
		void Tick(float dt);

		// Token: 0x06003730 RID: 14128
		void AfterTick(float dt);

		// Token: 0x06003731 RID: 14129
		void AfterWaitTick(float dt);

		// Token: 0x06003732 RID: 14130
		void OnIdleTick(float dt);

		// Token: 0x06003733 RID: 14131
		void OnSignalPeriodicEvents();

		// Token: 0x06003734 RID: 14132
		void OnExit();

		// Token: 0x06003735 RID: 14133
		void ResetCamera(bool resetDistance, bool teleportToMainParty);

		// Token: 0x06003736 RID: 14134
		void TeleportCameraToMainParty();

		// Token: 0x06003737 RID: 14135
		void FastMoveCameraToMainParty();

		// Token: 0x06003738 RID: 14136
		bool IsCameraLockedToPlayerParty();

		// Token: 0x06003739 RID: 14137
		void StartCameraAnimation(CampaignVec2 targetPosition, float animationStopDuration);

		// Token: 0x0600373A RID: 14138
		void OnHourlyTick();

		// Token: 0x0600373B RID: 14139
		void OnMenuModeTick(float dt);

		// Token: 0x0600373C RID: 14140
		void OnEnteringMenuMode(MenuContext menuContext);

		// Token: 0x0600373D RID: 14141
		void OnExitingMenuMode();

		// Token: 0x0600373E RID: 14142
		void OnBattleSimulationStarted(BattleSimulation battleSimulation);

		// Token: 0x0600373F RID: 14143
		void OnBattleSimulationEnded();

		// Token: 0x06003740 RID: 14144
		void OnGameplayCheatsEnabled();

		// Token: 0x06003741 RID: 14145
		void OnMapConversationStarts(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData);

		// Token: 0x06003742 RID: 14146
		void OnMapConversationOver();

		// Token: 0x06003743 RID: 14147
		void OnPlayerSiegeActivated();

		// Token: 0x06003744 RID: 14148
		void OnPlayerSiegeDeactivated();

		// Token: 0x06003745 RID: 14149
		void OnSiegeEngineClick(MatrixFrame siegeEngineFrame);

		// Token: 0x06003746 RID: 14150
		void OnGameLoadFinished();
	}
}
