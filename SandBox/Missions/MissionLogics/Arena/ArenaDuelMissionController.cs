using System;
using System.Linq;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x0200009C RID: 156
	public class ArenaDuelMissionController : MissionLogic
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x0002B2B4 File Offset: 0x000294B4
		public ArenaDuelMissionController(CharacterObject duelCharacter, bool requireCivilianEquipment, bool spawnBothSideWithHorses, Action<CharacterObject> onDuelEnd, float customAgentHealth)
		{
			this._duelCharacter = duelCharacter;
			this._requireCivilianEquipment = requireCivilianEquipment;
			this._spawnBothSideWithHorses = spawnBothSideWithHorses;
			this._customAgentHealth = customAgentHealth;
			ArenaDuelMissionController._onDuelEnd = onDuelEnd;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002B2E0 File Offset: 0x000294E0
		public override void AfterStart()
		{
			this._duelHasEnded = false;
			this._duelEndTimer = new BasicMissionTimer();
			this.DeactivateOtherTournamentSets();
			this.InitializeMissionTeams();
			this._initialSpawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena")
				select e.GetGlobalFrame()).ToMBList<MatrixFrame>();
			for (int i = 0; i < this._initialSpawnFrames.Count; i++)
			{
				MatrixFrame matrixFrame = this._initialSpawnFrames[i];
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				this._initialSpawnFrames[i] = matrixFrame;
			}
			MatrixFrame randomElement = this._initialSpawnFrames.GetRandomElement<MatrixFrame>();
			this._initialSpawnFrames.Remove(randomElement);
			MatrixFrame randomElement2 = this._initialSpawnFrames.GetRandomElement<MatrixFrame>();
			this.SpawnAgent(CharacterObject.PlayerCharacter, randomElement);
			this._duelAgent = this.SpawnAgent(this._duelCharacter, randomElement2);
			this._duelAgent.Defensiveness = 1f;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002B3E0 File Offset: 0x000295E0
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002B3F0 File Offset: 0x000295F0
		private void InitializeMissionTeams()
		{
			base.Mission.Teams.Add(BattleSideEnum.Defender, Hero.MainHero.MapFaction.Color, Hero.MainHero.MapFaction.Color2, null, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, this._duelCharacter.Culture.Color, this._duelCharacter.Culture.Color2, null, true, false, true);
			base.Mission.PlayerTeam = base.Mission.Teams.Defender;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0002B482 File Offset: 0x00029682
		private void DeactivateOtherTournamentSets()
		{
			TournamentBehavior.DeleteTournamentSetsExcept(base.Mission.Scene.FindEntityWithTag("tournament_fight"));
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0002B4A0 File Offset: 0x000296A0
		private Agent SpawnAgent(CharacterObject character, MatrixFrame spawnFrame)
		{
			AgentBuildData agentBuildData = new AgentBuildData(character);
			agentBuildData.BodyProperties(character.GetBodyPropertiesMax(false));
			Mission mission = base.Mission;
			AgentBuildData agentBuildData2 = agentBuildData.Team((character == CharacterObject.PlayerCharacter) ? base.Mission.PlayerTeam : base.Mission.PlayerEnemyTeam).InitialPosition(in spawnFrame.origin);
			Vec2 vec = spawnFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			Agent agent = mission.SpawnAgent(agentBuildData2.InitialDirection(in vec).NoHorses(!this._spawnBothSideWithHorses).Equipment(this._requireCivilianEquipment ? character.FirstCivilianEquipment : character.FirstBattleEquipment)
				.TroopOrigin(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))), false, null, null);
			agent.FadeIn();
			if (character == CharacterObject.PlayerCharacter)
			{
				agent.Controller = AgentControllerType.Player;
			}
			if (agent.IsAIControlled)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			agent.Health = this._customAgentHealth;
			agent.BaseHealthLimit = this._customAgentHealth;
			agent.HealthLimit = this._customAgentHealth;
			return agent;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002B5AC File Offset: 0x000297AC
		public override void OnMissionTick(float dt)
		{
			if (this._duelHasEnded && this._duelEndTimer.ElapsedTime > 4f)
			{
				GameTexts.SetVariable("leave_key", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f));
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_duel_has_ended", null), 0, null, null, "");
				this._duelEndTimer.Reset();
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002B618 File Offset: 0x00029818
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (ArenaDuelMissionController._onDuelEnd != null)
			{
				ArenaDuelMissionController._onDuelEnd((affectedAgent == this._duelAgent) ? CharacterObject.PlayerCharacter : this._duelCharacter);
				ArenaDuelMissionController._onDuelEnd = null;
				this._duelHasEnded = true;
				this._duelEndTimer.Reset();
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002B664 File Offset: 0x00029864
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (!this._duelHasEnded)
			{
				canPlayerLeave = false;
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_can_not_retreat_duel_ongoing", null), 0, null, null, "");
			}
			return null;
		}

		// Token: 0x0400036B RID: 875
		private CharacterObject _duelCharacter;

		// Token: 0x0400036C RID: 876
		private bool _requireCivilianEquipment;

		// Token: 0x0400036D RID: 877
		private bool _spawnBothSideWithHorses;

		// Token: 0x0400036E RID: 878
		private bool _duelHasEnded;

		// Token: 0x0400036F RID: 879
		private Agent _duelAgent;

		// Token: 0x04000370 RID: 880
		private float _customAgentHealth;

		// Token: 0x04000371 RID: 881
		private BasicMissionTimer _duelEndTimer;

		// Token: 0x04000372 RID: 882
		private MBList<MatrixFrame> _initialSpawnFrames;

		// Token: 0x04000373 RID: 883
		private static Action<CharacterObject> _onDuelEnd;
	}
}
