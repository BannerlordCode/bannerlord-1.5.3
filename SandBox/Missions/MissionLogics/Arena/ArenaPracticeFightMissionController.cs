using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x0200009D RID: 157
	public class ArenaPracticeFightMissionController : MissionLogic
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0002B68D File Offset: 0x0002988D
		private int AISpawnIndex
		{
			get
			{
				return this._spawnedOpponentAgentCount;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0002B695 File Offset: 0x00029895
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x0002B69D File Offset: 0x0002989D
		public int RemainingOpponentCountFromLastPractice { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0002B6A6 File Offset: 0x000298A6
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0002B6AE File Offset: 0x000298AE
		public bool IsPlayerPracticing { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0002B6B7 File Offset: 0x000298B7
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x0002B6BF File Offset: 0x000298BF
		public int OpponentCountBeatenByPlayer { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0002B6C8 File Offset: 0x000298C8
		public int RemainingOpponentCount
		{
			get
			{
				return 30 - this._spawnedOpponentAgentCount + this._aliveOpponentCount;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0002B6DA File Offset: 0x000298DA
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x0002B6E2 File Offset: 0x000298E2
		public bool IsPlayerSurvived { get; private set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0002B6EB File Offset: 0x000298EB
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x0002B6F3 File Offset: 0x000298F3
		public bool AfterPractice { get; set; }

		// Token: 0x0600067F RID: 1663 RVA: 0x0002B6FC File Offset: 0x000298FC
		public override void AfterStart()
		{
			this._settlement = PlayerEncounter.LocationEncounter.Settlement;
			this.InitializeTeams();
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("tournament_practice") ?? base.Mission.Scene.FindEntityWithTag("tournament_fight");
			List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag("arena_set").ToList<GameEntity>();
			list.Remove(gameEntity);
			foreach (GameEntity gameEntity2 in list)
			{
				gameEntity2.Remove(88);
			}
			this._initialSpawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena")
				select e.GetGlobalFrame()).ToList<MatrixFrame>();
			this._spawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena_respawn")
				select e.GetGlobalFrame()).ToList<MatrixFrame>();
			for (int i = 0; i < this._initialSpawnFrames.Count; i++)
			{
				MatrixFrame matrixFrame = this._initialSpawnFrames[i];
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				this._initialSpawnFrames[i] = matrixFrame;
			}
			for (int j = 0; j < this._spawnFrames.Count; j++)
			{
				MatrixFrame matrixFrame2 = this._spawnFrames[j];
				matrixFrame2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				this._spawnFrames[j] = matrixFrame2;
			}
			this.IsPlayerPracticing = false;
			this._participantAgents = new List<Agent>();
			this.StartPractice();
			MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			SandBoxHelpers.MissionHelper.SpawnPlayer(true, true, false, false, "");
			missionBehavior.SpawnLocationCharacters(null);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002B8F0 File Offset: 0x00029AF0
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002B900 File Offset: 0x00029B00
		private void SpawnPlayerNearTournamentMaster()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("sp_player_near_arena_master");
			base.Mission.SpawnAgent(new AgentBuildData(CharacterObject.PlayerCharacter).Team(base.Mission.PlayerTeam).InitialFrameFromSpawnPointEntity(gameEntity).NoHorses(true)
				.CivilianEquipment(true)
				.TroopOrigin(new SimpleAgentOrigin(CharacterObject.PlayerCharacter, -1, null, default(UniqueTroopDescriptor)))
				.Controller(AgentControllerType.Player), false, null, null);
			Mission.Current.SetMissionMode(MissionMode.StartUp, false);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002B98C File Offset: 0x00029B8C
		private Agent SpawnArenaAgent(Team team, MatrixFrame frame)
		{
			CharacterObject characterObject;
			int num;
			if (team == base.Mission.PlayerTeam)
			{
				characterObject = CharacterObject.PlayerCharacter;
				num = 0;
			}
			else
			{
				characterObject = this._participantCharacters[this.AISpawnIndex];
				num = this.AISpawnIndex;
			}
			Equipment equipment = new Equipment();
			this.AddRandomWeapons(equipment, num);
			this.AddRandomClothes(characterObject, equipment);
			Mission mission = base.Mission;
			AgentBuildData agentBuildData = new AgentBuildData(characterObject).Team(team).InitialPosition(in frame.origin);
			Vec2 vec = frame.rotation.f.AsVec2;
			vec = vec.Normalized();
			Agent agent = mission.SpawnAgent(agentBuildData.InitialDirection(in vec).NoHorses(true).Equipment(equipment)
				.TroopOrigin(new SimpleAgentOrigin(characterObject, -1, null, default(UniqueTroopDescriptor)))
				.Controller((characterObject == CharacterObject.PlayerCharacter) ? AgentControllerType.Player : AgentControllerType.AI), false, null, null);
			agent.FadeIn();
			if (characterObject != CharacterObject.PlayerCharacter)
			{
				this._aliveOpponentCount++;
				this._spawnedOpponentAgentCount++;
			}
			if (agent.IsAIControlled)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			return agent;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002BA98 File Offset: 0x00029C98
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

		// Token: 0x06000684 RID: 1668 RVA: 0x0002BB1C File Offset: 0x00029D1C
		private void EnemyHitReward(Agent affectedAgent, Agent affectorAgent, float lastSpeedBonus, float lastShotDifficulty, WeaponComponentData attackerWeapon, AgentAttackType attackType, float hitpointRatio, float damageAmount, bool isSneakAttack)
		{
			CharacterObject characterObject = (CharacterObject)affectedAgent.Character;
			CharacterObject characterObject2 = (CharacterObject)affectorAgent.Character;
			if (affectedAgent.Origin != null && affectorAgent != null && affectorAgent.Origin != null)
			{
				bool flag = affectorAgent.MountAgent != null;
				bool flag2 = flag && attackType == AgentAttackType.Collision;
				SkillLevelingManager.OnCombatHit(characterObject2, characterObject, null, null, lastSpeedBonus, lastShotDifficulty, attackerWeapon, hitpointRatio, CombatXpModel.MissionTypeEnum.PracticeFight, flag, affectorAgent.Team == affectedAgent.Team, false, damageAmount, affectedAgent.Health < 1f, false, flag2, isSneakAttack);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0002BBA0 File Offset: 0x00029DA0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._aliveOpponentCount < 6 && this._spawnedOpponentAgentCount < 30 && (this._aliveOpponentCount == 2 || this._nextSpawnTime < base.Mission.CurrentTime))
			{
				Team team = this.SelectRandomAiTeam();
				Agent agent = this.SpawnArenaAgent(team, this.GetSpawnFrame(true, false));
				this._participantAgents.Add(agent);
				this._nextSpawnTime = base.Mission.CurrentTime + 14f - (float)this._spawnedOpponentAgentCount / 3f;
				if (this._spawnedOpponentAgentCount == 30 && !this.IsPlayerPracticing)
				{
					this._spawnedOpponentAgentCount = 0;
				}
			}
			if (this._teleportTimer == null && this.IsPlayerPracticing && this.CheckPracticeEndedForPlayer())
			{
				this._teleportTimer = new BasicMissionTimer();
				this.IsPlayerSurvived = base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive();
				if (this.IsPlayerSurvived)
				{
					MBInformationManager.AddQuickInformation(new TextObject("{=seyti8xR}Victory!", null), 0, null, null, "event:/ui/mission/arena_victory");
				}
				this.AfterPractice = true;
			}
			if (this._teleportTimer != null && this._teleportTimer.ElapsedTime > (float)this.TeleportTime)
			{
				this._teleportTimer = null;
				this.RemainingOpponentCountFromLastPractice = this.RemainingOpponentCount;
				this.IsPlayerPracticing = false;
				this.StartPractice();
				this.SpawnPlayerNearTournamentMaster();
				Agent agent2 = base.Mission.Agents.FirstOrDefault<Agent>((Agent x) => x.Character != null && ((CharacterObject)x.Character).Occupation == Occupation.ArenaMaster);
				MissionConversationLogic.Current.StartConversation(agent2, true, false);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002BD38 File Offset: 0x00029F38
		private Team SelectRandomAiTeam()
		{
			Team team = null;
			foreach (Team team2 in this._AIParticipantTeams)
			{
				if (!team2.HasBots)
				{
					team = team2;
					break;
				}
			}
			if (team == null)
			{
				team = this._AIParticipantTeams[MBRandom.RandomInt(this._AIParticipantTeams.Count - 1) + 1];
			}
			return team;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002BDB8 File Offset: 0x00029FB8
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent != null && affectedAgent.IsHuman)
			{
				if (affectedAgent != Agent.Main)
				{
					this._aliveOpponentCount--;
				}
				if (affectorAgent != null && affectorAgent.IsHuman && affectorAgent == Agent.Main && affectedAgent != Agent.Main)
				{
					int opponentCountBeatenByPlayer = this.OpponentCountBeatenByPlayer;
					this.OpponentCountBeatenByPlayer = opponentCountBeatenByPlayer + 1;
				}
			}
			if (this._participantAgents.Contains(affectedAgent))
			{
				this._participantAgents.Remove(affectedAgent);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0002BE2C File Offset: 0x0002A02C
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return false;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002BE30 File Offset: 0x0002A030
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (!this.IsPlayerPracticing)
			{
				return null;
			}
			return new InquiryData(new TextObject("{=zv49qE35}Practice Fight", null).ToString(), GameTexts.FindText("str_give_up_fight", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0002BEB0 File Offset: 0x0002A0B0
		public void StartPlayerPractice()
		{
			this.IsPlayerPracticing = true;
			this.AfterPractice = false;
			this.StartPractice();
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0002BEC8 File Offset: 0x0002A0C8
		private void StartPractice()
		{
			this.InitializeParticipantCharacters();
			SandBoxHelpers.MissionHelper.FadeOutAgents(base.Mission.Agents.Where<Agent>((Agent agent) => this._participantAgents.Contains(agent) || agent.IsMount || agent.IsPlayerControlled), true, false);
			this._spawnedOpponentAgentCount = 0;
			this._aliveOpponentCount = 0;
			this._participantAgents.Clear();
			Mission.Current.ClearCorpses(false);
			base.Mission.RemoveSpawnedItemsAndMissiles();
			this.ArrangePlayerTeamEnmity();
			if (this.IsPlayerPracticing)
			{
				Agent agent2 = this.SpawnArenaAgent(base.Mission.PlayerTeam, this.GetSpawnFrame(false, true));
				agent2.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				this.OpponentCountBeatenByPlayer = 0;
				this._participantAgents.Add(agent2);
			}
			int count = this._AIParticipantTeams.Count;
			int num = 0;
			while (this._spawnedOpponentAgentCount < 6)
			{
				this._participantAgents.Add(this.SpawnArenaAgent(this._AIParticipantTeams[num % count], this.GetSpawnFrame(false, true)));
				num++;
			}
			this._nextSpawnTime = base.Mission.CurrentTime + 14f;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0002BFCB File Offset: 0x0002A1CB
		private bool CheckPracticeEndedForPlayer()
		{
			return base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive() || this.RemainingOpponentCount == 0;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
		private void AddRandomWeapons(Equipment equipment, int spawnIndex)
		{
			int num = 1 + spawnIndex * 3 / 30;
			List<Equipment> list = (Game.Current.ObjectManager.GetObject<CharacterObject>(string.Concat(new object[]
			{
				"weapon_practice_stage_",
				num,
				"_",
				this._settlement.MapFaction.Culture.StringId
			})) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("weapon_practice_stage_" + num + "_empire")).BattleEquipments.ToList<Equipment>();
			int num2 = MBRandom.RandomInt(list.Count);
			for (int i = 0; i <= 3; i++)
			{
				EquipmentElement equipmentFromSlot = list[num2].GetEquipmentFromSlot((EquipmentIndex)i);
				if (equipmentFromSlot.Item != null)
				{
					equipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
				}
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
		private void AddRandomClothes(CharacterObject troop, Equipment equipment)
		{
			Equipment participantArmor = Campaign.Current.Models.TournamentModel.GetParticipantArmor(troop);
			for (int i = 0; i < 12; i++)
			{
				if (i > 4 && i != 10 && i != 11)
				{
					EquipmentElement equipmentFromSlot = participantArmor.GetEquipmentFromSlot((EquipmentIndex)i);
					if (equipmentFromSlot.Item != null)
					{
						equipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
					}
				}
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002C120 File Offset: 0x0002A320
		private void InitializeTeams()
		{
			this._AIParticipantTeams = new List<Team>();
			base.Mission.Teams.Add(BattleSideEnum.Defender, Hero.MainHero.MapFaction.Color, Hero.MainHero.MapFaction.Color2, null, true, false, true);
			base.Mission.PlayerTeam = base.Mission.DefenderTeam;
			this._tournamentMasterTeam = base.Mission.Teams.Add(BattleSideEnum.None, this._settlement.MapFaction.Color, this._settlement.MapFaction.Color2, null, true, false, true);
			while (this._AIParticipantTeams.Count < 6)
			{
				this._AIParticipantTeams.Add(base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true));
			}
			for (int i = 0; i < this._AIParticipantTeams.Count; i++)
			{
				this._AIParticipantTeams[i].SetIsEnemyOf(this._tournamentMasterTeam, false);
				for (int j = i + 1; j < this._AIParticipantTeams.Count; j++)
				{
					this._AIParticipantTeams[i].SetIsEnemyOf(this._AIParticipantTeams[j], true);
				}
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002C254 File Offset: 0x0002A454
		private void InitializeParticipantCharacters()
		{
			List<CharacterObject> participantCharacters = ArenaPracticeFightMissionController.GetParticipantCharacters(this._settlement);
			this._participantCharacters = participantCharacters.OrderBy<CharacterObject, int>((CharacterObject x) => x.Level).ToList<CharacterObject>();
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002C2A0 File Offset: 0x0002A4A0
		public static List<CharacterObject> GetParticipantCharacters(Settlement settlement)
		{
			int num = 30;
			List<CharacterObject> list = new List<CharacterObject>();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (list.Count < num && settlement.Town.GarrisonParty != null)
			{
				foreach (TroopRosterElement troopRosterElement in settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					int num5 = num - list.Count;
					if (!list.Contains(troopRosterElement.Character) && troopRosterElement.Character.Tier == 3 && (float)num5 * 0.4f > (float)num2)
					{
						list.Add(troopRosterElement.Character);
						num2++;
					}
					else if (!list.Contains(troopRosterElement.Character) && troopRosterElement.Character.Tier == 4 && (float)num5 * 0.4f > (float)num3)
					{
						list.Add(troopRosterElement.Character);
						num3++;
					}
					else if (!list.Contains(troopRosterElement.Character) && troopRosterElement.Character.Tier == 5 && (float)num5 * 0.2f > (float)num4)
					{
						list.Add(troopRosterElement.Character);
						num4++;
					}
					if (list.Count >= num)
					{
						break;
					}
				}
			}
			if (list.Count < num)
			{
				List<CharacterObject> list2 = new List<CharacterObject>();
				ArenaPracticeFightMissionController.GetUpgradeTargets(((settlement != null) ? settlement.Culture : Game.Current.ObjectManager.GetObject<CultureObject>("empire")).BasicTroop, ref list2);
				int num6 = num - list.Count;
				using (List<CharacterObject>.Enumerator enumerator2 = list2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharacterObject characterObject = enumerator2.Current;
						if (!list.Contains(characterObject) && characterObject.Tier == 3 && (float)num6 * 0.4f > (float)num2)
						{
							list.Add(characterObject);
							num2++;
						}
						else if (!list.Contains(characterObject) && characterObject.Tier == 4 && (float)num6 * 0.4f > (float)num3)
						{
							list.Add(characterObject);
							num3++;
						}
						else if (!list.Contains(characterObject) && characterObject.Tier == 5 && (float)num6 * 0.2f > (float)num4)
						{
							list.Add(characterObject);
							num4++;
						}
						if (list.Count >= num)
						{
							break;
						}
					}
					goto IL_0284;
				}
				IL_0256:
				int num7 = 0;
				while (num7 < list2.Count && list.Count < num)
				{
					list.Add(list2[num7]);
					num7++;
				}
				IL_0284:
				if (list.Count < num)
				{
					goto IL_0256;
				}
			}
			return list;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002C558 File Offset: 0x0002A758
		private static void GetUpgradeTargets(CharacterObject troop, ref List<CharacterObject> list)
		{
			if (!list.Contains(troop) && troop.Tier >= 3)
			{
				list.Add(troop);
			}
			CharacterObject[] upgradeTargets = troop.UpgradeTargets;
			for (int i = 0; i < upgradeTargets.Length; i++)
			{
				ArenaPracticeFightMissionController.GetUpgradeTargets(upgradeTargets[i], ref list);
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002C5A0 File Offset: 0x0002A7A0
		private void ArrangePlayerTeamEnmity()
		{
			foreach (Team team in this._AIParticipantTeams)
			{
				team.SetIsEnemyOf(base.Mission.PlayerTeam, this.IsPlayerPracticing);
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002C604 File Offset: 0x0002A804
		private Team GetStrongestTeamExceptPlayerTeam()
		{
			Team team = null;
			int num = -1;
			foreach (Team team2 in this._AIParticipantTeams)
			{
				int num2 = this.CalculateTeamPower(team2);
				if (num2 > num)
				{
					team = team2;
					num = num2;
				}
			}
			return team;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0002C668 File Offset: 0x0002A868
		private int CalculateTeamPower(Team team)
		{
			int num = 0;
			foreach (Agent agent in team.ActiveAgents)
			{
				num += agent.Character.Level * agent.KillCount + (int)MathF.Sqrt(agent.Health);
			}
			return num;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002C6DC File Offset: 0x0002A8DC
		private MatrixFrame GetSpawnFrame(bool considerPlayerDistance, bool isInitialSpawn)
		{
			List<MatrixFrame> list = ((isInitialSpawn || this._spawnFrames.IsEmpty<MatrixFrame>()) ? this._initialSpawnFrames : this._spawnFrames);
			if (list.Count == 1)
			{
				Debug.FailedAssert("Spawn point count is wrong! Arena practice spawn point set should be used in arena scenes.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Arena\\ArenaPracticeFightMissionController.cs", "GetSpawnFrame", 621);
				return list[0];
			}
			MatrixFrame matrixFrame;
			if (considerPlayerDistance && Agent.Main != null && Agent.Main.IsActive())
			{
				int num = MBRandom.RandomInt(list.Count);
				matrixFrame = list[num];
				float num2 = float.MinValue;
				for (int i = num + 1; i < num + list.Count; i++)
				{
					MatrixFrame matrixFrame2 = list[i % list.Count];
					float num3 = this.CalculateLocationScore(matrixFrame2);
					if (num3 >= 100f)
					{
						matrixFrame = matrixFrame2;
						break;
					}
					if (num3 > num2)
					{
						matrixFrame = matrixFrame2;
						num2 = num3;
					}
				}
			}
			else
			{
				int num4 = this._spawnedOpponentAgentCount;
				if (this.IsPlayerPracticing && Agent.Main != null)
				{
					num4++;
				}
				matrixFrame = list[num4 % list.Count];
			}
			return matrixFrame;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002C7E0 File Offset: 0x0002A9E0
		private float CalculateLocationScore(MatrixFrame matrixFrame)
		{
			float num = 100f;
			float num2 = 0.25f;
			float num3 = 0.75f;
			if (matrixFrame.origin.DistanceSquared(Agent.Main.Position) < 144f)
			{
				num *= num2;
			}
			for (int i = 0; i < this._participantAgents.Count; i++)
			{
				if (this._participantAgents[i].Position.DistanceSquared(matrixFrame.origin) < 144f)
				{
					num *= num3;
				}
			}
			return num;
		}

		// Token: 0x04000374 RID: 884
		private const int AIParticipantCount = 30;

		// Token: 0x04000375 RID: 885
		private const int MaxAliveAgentCount = 6;

		// Token: 0x04000376 RID: 886
		private const int MaxSpawnInterval = 14;

		// Token: 0x04000377 RID: 887
		private const int MinSpawnDistanceSquared = 144;

		// Token: 0x04000378 RID: 888
		private const int TotalStageCount = 3;

		// Token: 0x04000379 RID: 889
		private const int PracticeFightTroopTierLimit = 3;

		// Token: 0x0400037A RID: 890
		public int TeleportTime = 5;

		// Token: 0x0400037B RID: 891
		private Settlement _settlement;

		// Token: 0x0400037C RID: 892
		private int _spawnedOpponentAgentCount;

		// Token: 0x0400037D RID: 893
		private int _aliveOpponentCount;

		// Token: 0x0400037E RID: 894
		private float _nextSpawnTime;

		// Token: 0x0400037F RID: 895
		private List<MatrixFrame> _initialSpawnFrames;

		// Token: 0x04000380 RID: 896
		private List<MatrixFrame> _spawnFrames;

		// Token: 0x04000381 RID: 897
		private List<Team> _AIParticipantTeams;

		// Token: 0x04000382 RID: 898
		private List<Agent> _participantAgents;

		// Token: 0x04000383 RID: 899
		private Team _tournamentMasterTeam;

		// Token: 0x04000384 RID: 900
		private BasicMissionTimer _teleportTimer;

		// Token: 0x04000385 RID: 901
		private List<CharacterObject> _participantCharacters;

		// Token: 0x0400038B RID: 907
		private const float XpShareForKill = 0.5f;

		// Token: 0x0400038C RID: 908
		private const float XpShareForDamage = 0.5f;
	}
}
