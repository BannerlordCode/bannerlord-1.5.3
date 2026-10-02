using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace SandBox.Tournaments.MissionLogics
{
	// Token: 0x0200002E RID: 46
	public class TournamentFightMissionController : MissionLogic, ITournamentGameBehavior
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00009A64 File Offset: 0x00007C64
		public TournamentFightMissionController(CultureObject culture)
		{
			this._match = null;
			this._culture = culture;
			this._cheerStarted = false;
			this._currentTournamentAgents = new List<Agent>();
			this._currentTournamentMountAgents = new List<Agent>();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009AE1 File Offset: 0x00007CE1
		public override void OnBehaviorInitialize()
		{
			base.Mission.CanAgentRout_AdditionalCondition += this.CanAgentRout;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00009AFC File Offset: 0x00007CFC
		public override void AfterStart()
		{
			TournamentBehavior.DeleteTournamentSetsExcept(base.Mission.Scene.FindEntityWithTag("tournament_fight"));
			this._spawnPoints = new List<GameEntity>();
			for (int i = 0; i < 4; i++)
			{
				GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("sp_arena_" + (i + 1));
				if (gameEntity != null)
				{
					this._spawnPoints.Add(gameEntity);
				}
			}
			if (this._spawnPoints.Count < 4)
			{
				this._spawnPoints = base.Mission.Scene.FindEntitiesWithTag("sp_arena").ToList<GameEntity>();
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009BA0 File Offset: 0x00007DA0
		public void PrepareForMatch()
		{
			List<Equipment> teamWeaponEquipmentList = this.GetTeamWeaponEquipmentList(this._match.Teams.First<TournamentTeam>().Participants.Count<TournamentParticipant>());
			foreach (TournamentTeam tournamentTeam in this._match.Teams)
			{
				int num = 0;
				foreach (TournamentParticipant tournamentParticipant in tournamentTeam.Participants)
				{
					tournamentParticipant.MatchEquipment = teamWeaponEquipmentList[num].Clone(false);
					this.AddRandomClothes(this._culture, tournamentParticipant);
					num++;
				}
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009C68 File Offset: 0x00007E68
		public void StartMatch(TournamentMatch match, bool isLastRound)
		{
			this._cheerStarted = false;
			this._match = match;
			this._isLastRound = isLastRound;
			this.PrepareForMatch();
			base.Mission.SetMissionMode(MissionMode.Battle, true);
			List<Team> list = new List<Team>();
			int count = this._spawnPoints.Count;
			int num = 0;
			foreach (TournamentTeam tournamentTeam in this._match.Teams)
			{
				BattleSideEnum battleSideEnum = (tournamentTeam.IsPlayerTeam ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
				Team team = base.Mission.Teams.Add(battleSideEnum, tournamentTeam.TeamColor, uint.MaxValue, tournamentTeam.TeamBanner, true, false, true);
				GameEntity gameEntity = this._spawnPoints[num % count];
				foreach (TournamentParticipant tournamentParticipant in tournamentTeam.Participants)
				{
					if (tournamentParticipant.Character.IsPlayerCharacter)
					{
						this.SpawnTournamentParticipant(gameEntity, tournamentParticipant, team);
						break;
					}
				}
				foreach (TournamentParticipant tournamentParticipant2 in tournamentTeam.Participants)
				{
					if (!tournamentParticipant2.Character.IsPlayerCharacter)
					{
						this.SpawnTournamentParticipant(gameEntity, tournamentParticipant2, team);
					}
				}
				num++;
				list.Add(team);
			}
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = i + 1; j < list.Count; j++)
				{
					list[i].SetIsEnemyOf(list[j], true);
				}
			}
			this._aliveParticipants = this._match.Participants.ToList<TournamentParticipant>();
			this._aliveTeams = this._match.Teams.ToList<TournamentTeam>();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009E88 File Offset: 0x00008088
		protected override void OnEndMission()
		{
			base.Mission.CanAgentRout_AdditionalCondition -= this.CanAgentRout;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009EA4 File Offset: 0x000080A4
		private void SpawnTournamentParticipant(GameEntity spawnPoint, TournamentParticipant participant, Team team)
		{
			MatrixFrame globalFrame = spawnPoint.GetGlobalFrame();
			globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			this.SpawnAgentWithRandomItems(participant, team, globalFrame);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009ED0 File Offset: 0x000080D0
		private List<Equipment> GetTeamWeaponEquipmentList(int teamSize)
		{
			List<Equipment> list = new List<Equipment>();
			CultureObject culture = PlayerEncounter.EncounterSettlement.Culture;
			MBReadOnlyList<CharacterObject> mbreadOnlyList = ((teamSize == 4) ? culture.TournamentTeamTemplatesForFourParticipant : ((teamSize == 2) ? culture.TournamentTeamTemplatesForTwoParticipant : culture.TournamentTeamTemplatesForOneParticipant));
			CharacterObject characterObject;
			if (mbreadOnlyList.Count > 0)
			{
				characterObject = mbreadOnlyList[MBRandom.RandomInt(mbreadOnlyList.Count)];
			}
			else
			{
				characterObject = ((teamSize == 4) ? this._defaultWeaponTemplatesIdTeamSizeFour : ((teamSize == 2) ? this._defaultWeaponTemplatesIdTeamSizeTwo : this._defaultWeaponTemplatesIdTeamSizeOne));
			}
			foreach (Equipment equipment in characterObject.BattleEquipments)
			{
				Equipment equipment2 = new Equipment();
				equipment2.FillFrom(equipment, true);
				list.Add(equipment2);
			}
			return list;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009FA4 File Offset: 0x000081A4
		public void SkipMatch(TournamentMatch match)
		{
			this._match = match;
			this.PrepareForMatch();
			this.Simulate();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009FBC File Offset: 0x000081BC
		public bool IsMatchEnded()
		{
			if (this._isSimulated || this._match == null)
			{
				return true;
			}
			if ((this._endTimer != null && this._endTimer.ElapsedTime > 6f) || this._forceEndMatch)
			{
				this._forceEndMatch = false;
				this._endTimer = null;
				return true;
			}
			if (this._cheerTimer != null && !this._cheerStarted && this._cheerTimer.ElapsedTime > 1f)
			{
				this.OnMatchResultsReady();
				this._cheerTimer = null;
				this._cheerStarted = true;
				AgentVictoryLogic missionBehavior = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
				foreach (Agent agent in this._currentTournamentAgents)
				{
					if (agent.IsAIControlled)
					{
						missionBehavior.SetTimersOfVictoryReactionsOnTournamentVictoryForAgent(agent, 1f, 3f);
					}
				}
				return false;
			}
			if (this._endTimer == null && !this.CheckIfIsThereAnyEnemies())
			{
				this._endTimer = new BasicMissionTimer();
				if (!this._cheerStarted)
				{
					this._cheerTimer = new BasicMissionTimer();
				}
			}
			return false;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000A0DC File Offset: 0x000082DC
		public void OnMatchResultsReady()
		{
			if (!this._match.IsPlayerParticipating())
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=UBd0dEPp}Match is over", null), 0, null, null, "");
				return;
			}
			if (this._match.IsPlayerWinner())
			{
				if (this._isLastRound)
				{
					if (this._match.QualificationMode == TournamentGame.QualificationMode.IndividualScore)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=Jn0k20c3}Round is over, you survived the final round of the tournament.", null), 0, null, null, "");
						return;
					}
					MBInformationManager.AddQuickInformation(new TextObject("{=wOqOQuJl}Round is over, your team survived the final round of the tournament.", null), 0, null, null, "");
					return;
				}
				else
				{
					if (this._match.QualificationMode == TournamentGame.QualificationMode.IndividualScore)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=uytwdSVH}Round is over, you are qualified for the next stage of the tournament.", null), 0, null, null, "");
						return;
					}
					MBInformationManager.AddQuickInformation(new TextObject("{=fkOYvnVG}Round is over, your team is qualified for the next stage of the tournament.", null), 0, null, null, "");
					return;
				}
			}
			else
			{
				if (this._match.QualificationMode == TournamentGame.QualificationMode.IndividualScore)
				{
					MBInformationManager.AddQuickInformation(new TextObject("{=lcVauEKV}Round is over, you are disqualified from the tournament.", null), 0, null, null, "");
					return;
				}
				MBInformationManager.AddQuickInformation(new TextObject("{=MLyBN51z}Round is over, your team is disqualified from the tournament.", null), 0, null, null, "");
				return;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000A1E8 File Offset: 0x000083E8
		public void OnMatchEnded()
		{
			SandBoxHelpers.MissionHelper.FadeOutAgents(this._currentTournamentAgents.Where<Agent>((Agent x) => x.IsActive()), true, false);
			SandBoxHelpers.MissionHelper.FadeOutAgents(this._currentTournamentMountAgents.Where<Agent>((Agent x) => x.IsActive()), true, false);
			base.Mission.ClearCorpses(false);
			base.Mission.Teams.Clear();
			base.Mission.RemoveSpawnedItemsAndMissiles();
			this._match = null;
			this._endTimer = null;
			this._cheerTimer = null;
			this._isSimulated = false;
			this._currentTournamentAgents.Clear();
			this._currentTournamentMountAgents.Clear();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A2B0 File Offset: 0x000084B0
		private void SpawnAgentWithRandomItems(TournamentParticipant participant, Team team, MatrixFrame frame)
		{
			frame.Strafe((float)MBRandom.RandomInt(-2, 2) * 1f);
			frame.Advance((float)MBRandom.RandomInt(0, 2) * 1f);
			CharacterObject character = participant.Character;
			AgentBuildData agentBuildData = new AgentBuildData(new SimpleAgentOrigin(character, -1, null, participant.Descriptor)).Team(team).InitialPosition(in frame.origin);
			Vec2 vec = frame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).Equipment(participant.MatchEquipment).ClothingColor1(team.Color)
				.Banner(team.Banner)
				.Controller(character.IsPlayerCharacter ? AgentControllerType.Player : AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			if (character.IsPlayerCharacter)
			{
				agent.Health = (float)character.HeroObject.HitPoints;
				base.Mission.PlayerTeam = team;
			}
			else
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			this._currentTournamentAgents.Add(agent);
			if (agent.HasMount)
			{
				this._currentTournamentMountAgents.Add(agent.MountAgent);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000A3D8 File Offset: 0x000085D8
		private void AddRandomClothes(CultureObject culture, TournamentParticipant participant)
		{
			Equipment participantArmor = Campaign.Current.Models.TournamentModel.GetParticipantArmor(participant.Character);
			for (int i = 5; i < 10; i++)
			{
				EquipmentElement equipmentFromSlot = participantArmor.GetEquipmentFromSlot((EquipmentIndex)i);
				if (equipmentFromSlot.Item != null)
				{
					participant.MatchEquipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
				}
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000A42C File Offset: 0x0000862C
		private bool CheckIfTeamIsDead(TournamentTeam affectedParticipantTeam)
		{
			bool flag = true;
			using (List<TournamentParticipant>.Enumerator enumerator = this._aliveParticipants.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Team == affectedParticipantTeam)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000A488 File Offset: 0x00008688
		private void AddScoreToRemainingTeams()
		{
			foreach (TournamentTeam tournamentTeam in this._aliveTeams)
			{
				foreach (TournamentParticipant tournamentParticipant in tournamentTeam.Participants)
				{
					tournamentParticipant.AddScore(1);
				}
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000A510 File Offset: 0x00008710
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (!this.IsMatchEnded() && affectedAgent.IsHuman)
			{
				TournamentParticipant participant = this._match.GetParticipant(affectedAgent.Origin.UniqueSeed);
				this._aliveParticipants.Remove(participant);
				this._currentTournamentAgents.Remove(affectedAgent);
				if (this.CheckIfTeamIsDead(participant.Team))
				{
					this._aliveTeams.Remove(participant.Team);
					this.AddScoreToRemainingTeams();
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000A584 File Offset: 0x00008784
		public bool CanAgentRout(Agent agent)
		{
			return false;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000A588 File Offset: 0x00008788
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent == null)
			{
				return;
			}
			if (affectorAgent.IsMount && affectorAgent.RiderAgent != null)
			{
				affectorAgent = affectorAgent.RiderAgent;
			}
			if (affectorAgent.Character == null || affectedAgent.Character == null)
			{
				return;
			}
			float num = (float)blow.InflictedDamage;
			if (num > affectedAgent.HealthLimit)
			{
				num = affectedAgent.HealthLimit;
			}
			float num2 = num / affectedAgent.HealthLimit;
			this.EnemyHitReward(affectedAgent, affectorAgent, blow.MovementSpeedDamageModifier, shotDifficulty, attackerWeapon, blow.AttackType, 0.5f * num2, num, collisionData.IsSneakAttack);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000A60C File Offset: 0x0000880C
		private void EnemyHitReward(Agent affectedAgent, Agent affectorAgent, float lastSpeedBonus, float lastShotDifficulty, WeaponComponentData lastAttackerWeapon, AgentAttackType attackType, float hitpointRatio, float damageAmount, bool isSneakAttack)
		{
			CharacterObject characterObject = (CharacterObject)affectedAgent.Character;
			CharacterObject characterObject2 = (CharacterObject)affectorAgent.Character;
			if (affectedAgent.Origin != null && affectorAgent != null && affectorAgent.Origin != null)
			{
				bool flag = affectorAgent.MountAgent != null && attackType == AgentAttackType.Collision;
				SkillLevelingManager.OnCombatHit(characterObject2, characterObject, null, null, lastSpeedBonus, lastShotDifficulty, lastAttackerWeapon, hitpointRatio, CombatXpModel.MissionTypeEnum.Tournament, affectorAgent.MountAgent != null, affectorAgent.Team == affectedAgent.Team, false, damageAmount, affectedAgent.Health < 1f, false, flag, isSneakAttack);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000A694 File Offset: 0x00008894
		public bool CheckIfIsThereAnyEnemies()
		{
			Team team = null;
			foreach (Agent agent in this._currentTournamentAgents)
			{
				if (agent.IsHuman && agent.IsActive() && agent.Team != null)
				{
					if (team == null)
					{
						team = agent.Team;
					}
					else if (team != agent.Team)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000A718 File Offset: 0x00008918
		private void Simulate()
		{
			this._isSimulated = false;
			if (this._currentTournamentAgents.Count == 0)
			{
				this._aliveParticipants = this._match.Participants.ToList<TournamentParticipant>();
				this._aliveTeams = this._match.Teams.ToList<TournamentTeam>();
			}
			TournamentParticipant tournamentParticipant = this._aliveParticipants.FirstOrDefault<TournamentParticipant>((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
			if (tournamentParticipant != null)
			{
				TournamentTeam team = tournamentParticipant.Team;
				foreach (TournamentParticipant tournamentParticipant2 in team.Participants)
				{
					tournamentParticipant2.ResetScore();
					this._aliveParticipants.Remove(tournamentParticipant2);
				}
				this._aliveTeams.Remove(team);
				this.AddScoreToRemainingTeams();
			}
			Dictionary<TournamentParticipant, Tuple<float, float>> dictionary = new Dictionary<TournamentParticipant, Tuple<float, float>>();
			foreach (TournamentParticipant tournamentParticipant3 in this._aliveParticipants)
			{
				float num;
				float num2;
				tournamentParticipant3.Character.GetSimulationAttackPower(out num, out num2, tournamentParticipant3.MatchEquipment);
				dictionary.Add(tournamentParticipant3, new Tuple<float, float>(num, num2));
			}
			int num3 = 0;
			while (this._aliveParticipants.Count > 1 && this._aliveTeams.Count > 1)
			{
				num3++;
				num3 %= this._aliveParticipants.Count;
				TournamentParticipant tournamentParticipant4 = this._aliveParticipants[num3];
				int num4;
				TournamentParticipant tournamentParticipant5;
				do
				{
					num4 = MBRandom.RandomInt(this._aliveParticipants.Count);
					tournamentParticipant5 = this._aliveParticipants[num4];
				}
				while (tournamentParticipant4 == tournamentParticipant5 || tournamentParticipant4.Team == tournamentParticipant5.Team);
				if (dictionary[tournamentParticipant5].Item2 - dictionary[tournamentParticipant4].Item1 > 0f)
				{
					dictionary[tournamentParticipant5] = new Tuple<float, float>(dictionary[tournamentParticipant5].Item1, dictionary[tournamentParticipant5].Item2 - dictionary[tournamentParticipant4].Item1);
				}
				else
				{
					dictionary.Remove(tournamentParticipant5);
					this._aliveParticipants.Remove(tournamentParticipant5);
					if (this.CheckIfTeamIsDead(tournamentParticipant5.Team))
					{
						this._aliveTeams.Remove(tournamentParticipant5.Team);
						this.AddScoreToRemainingTeams();
					}
					if (num4 < num3)
					{
						num3--;
					}
				}
			}
			this._isSimulated = true;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000A994 File Offset: 0x00008B94
		private bool IsThereAnyPlayerAgent()
		{
			if (base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive())
			{
				return true;
			}
			return this._currentTournamentAgents.Any<Agent>((Agent agent) => agent.IsPlayerControlled);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000A9EC File Offset: 0x00008BEC
		private void SkipMatch()
		{
			Mission.Current.GetMissionBehavior<TournamentBehavior>().SkipMatch(false);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000AA00 File Offset: 0x00008C00
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			InquiryData inquiryData = null;
			canPlayerLeave = true;
			if (this._match != null)
			{
				if (this._match.IsPlayerParticipating())
				{
					MBTextManager.SetTextVariable("SETTLEMENT_NAME", Hero.MainHero.CurrentSettlement.EncyclopediaLinkWithName, false);
					if (this.IsThereAnyPlayerAgent())
					{
						if (base.Mission.IsPlayerCloseToAnEnemy(5f))
						{
							canPlayerLeave = false;
							MBInformationManager.AddQuickInformation(GameTexts.FindText("str_can_not_retreat", null), 0, null, null, "");
						}
						else if (this.CheckIfIsThereAnyEnemies())
						{
							inquiryData = new InquiryData(GameTexts.FindText("str_tournament", null).ToString(), GameTexts.FindText("str_tournament_forfeit_game", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.SkipMatch), null, "", 0f, null, null, null);
						}
						else
						{
							this._forceEndMatch = true;
							canPlayerLeave = false;
						}
					}
					else if (this.CheckIfIsThereAnyEnemies())
					{
						inquiryData = new InquiryData(GameTexts.FindText("str_tournament", null).ToString(), GameTexts.FindText("str_tournament_skip", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.SkipMatch), null, "", 0f, null, null, null);
					}
					else
					{
						this._forceEndMatch = true;
						canPlayerLeave = false;
					}
				}
				else if (this.CheckIfIsThereAnyEnemies())
				{
					inquiryData = new InquiryData(GameTexts.FindText("str_tournament", null).ToString(), GameTexts.FindText("str_tournament_skip", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.SkipMatch), null, "", 0f, null, null, null);
				}
				else
				{
					this._forceEndMatch = true;
					canPlayerLeave = false;
				}
			}
			return inquiryData;
		}

		// Token: 0x0400007A RID: 122
		private readonly CharacterObject _defaultWeaponTemplatesIdTeamSizeOne = MBObjectManager.Instance.GetObject<CharacterObject>("tournament_template_empire_one_participant_set_v1");

		// Token: 0x0400007B RID: 123
		private readonly CharacterObject _defaultWeaponTemplatesIdTeamSizeTwo = MBObjectManager.Instance.GetObject<CharacterObject>("tournament_template_empire_two_participant_set_v1");

		// Token: 0x0400007C RID: 124
		private readonly CharacterObject _defaultWeaponTemplatesIdTeamSizeFour = MBObjectManager.Instance.GetObject<CharacterObject>("tournament_template_empire_four_participant_set_v1");

		// Token: 0x0400007D RID: 125
		private TournamentMatch _match;

		// Token: 0x0400007E RID: 126
		private bool _isLastRound;

		// Token: 0x0400007F RID: 127
		private BasicMissionTimer _endTimer;

		// Token: 0x04000080 RID: 128
		private BasicMissionTimer _cheerTimer;

		// Token: 0x04000081 RID: 129
		private List<GameEntity> _spawnPoints;

		// Token: 0x04000082 RID: 130
		private bool _isSimulated;

		// Token: 0x04000083 RID: 131
		private bool _forceEndMatch;

		// Token: 0x04000084 RID: 132
		private bool _cheerStarted;

		// Token: 0x04000085 RID: 133
		private CultureObject _culture;

		// Token: 0x04000086 RID: 134
		private List<TournamentParticipant> _aliveParticipants;

		// Token: 0x04000087 RID: 135
		private List<TournamentTeam> _aliveTeams;

		// Token: 0x04000088 RID: 136
		private List<Agent> _currentTournamentAgents;

		// Token: 0x04000089 RID: 137
		private List<Agent> _currentTournamentMountAgents;

		// Token: 0x0400008A RID: 138
		private const float XpShareForKill = 0.5f;

		// Token: 0x0400008B RID: 139
		private const float XpShareForDamage = 0.5f;
	}
}
