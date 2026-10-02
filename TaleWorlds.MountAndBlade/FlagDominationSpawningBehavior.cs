using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D8 RID: 728
	public class FlagDominationSpawningBehavior : SpawningBehaviorBase
	{
		// Token: 0x06002A24 RID: 10788 RVA: 0x0009F346 File Offset: 0x0009D546
		public FlagDominationSpawningBehavior()
		{
			this._enforcedSpawnTimers = new List<KeyValuePair<MissionPeer, Timer>>();
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x0009F35C File Offset: 0x0009D55C
		public override void Initialize(SpawnComponent spawnComponent)
		{
			base.Initialize(spawnComponent);
			this._flagDominationMissionController = base.Mission.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this._roundController = base.Mission.GetMissionBehavior<MultiplayerRoundController>();
			this._roundController.OnRoundStarted += this.RequestStartSpawnSession;
			this._roundController.OnRoundEnding += base.RequestStopSpawnSession;
			this._roundController.OnRoundEnding += base.SetRemainingAgentsInvulnerable;
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) == 0)
			{
				this._roundController.EnableEquipmentUpdate();
			}
			base.OnAllAgentsFromPeerSpawnedFromVisuals += this.OnAllAgentsFromPeerSpawnedFromVisuals;
			base.OnPeerSpawnedFromVisuals += this.OnPeerSpawnedFromVisuals;
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x0009F414 File Offset: 0x0009D614
		public override void Clear()
		{
			base.Clear();
			this._roundController.OnRoundStarted -= this.RequestStartSpawnSession;
			this._roundController.OnRoundEnding -= base.SetRemainingAgentsInvulnerable;
			this._roundController.OnRoundEnding -= base.RequestStopSpawnSession;
			base.OnAllAgentsFromPeerSpawnedFromVisuals -= this.OnAllAgentsFromPeerSpawnedFromVisuals;
			base.OnPeerSpawnedFromVisuals -= this.OnPeerSpawnedFromVisuals;
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x0009F494 File Offset: 0x0009D694
		public override void OnTick(float dt)
		{
			if (this._spawningTimerTicking)
			{
				this._spawningTimer += dt;
			}
			if (this.IsSpawningEnabled)
			{
				if (!this._roundInitialSpawnOver && this.IsRoundInProgress())
				{
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						if (((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
						{
							this.SpawnComponent.SetEarlyAgentVisualsDespawning(component, true);
						}
					}
					this._roundInitialSpawnOver = true;
					base.Mission.AllowAiTicking = true;
				}
				this.SpawnAgents();
				if (this._roundInitialSpawnOver && this._flagDominationMissionController.GameModeUsesSingleSpawning && this._spawningTimer > (float)MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					this.IsSpawningEnabled = false;
					this._spawningTimer = 0f;
					this._spawningTimerTicking = false;
				}
			}
			base.OnTick(dt);
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x0009F5A4 File Offset: 0x0009D7A4
		public override void RequestStartSpawnSession()
		{
			if (!this.IsSpawningEnabled)
			{
				Mission.Current.SetBattleAgentCount(-1);
				this.IsSpawningEnabled = true;
				this._spawningTimerTicking = true;
				base.ResetSpawnCounts();
				base.ResetSpawnTimers();
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x0009F5D4 File Offset: 0x0009D7D4
		protected override void SpawnAgents()
		{
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			int intValue = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (networkCommunicator.IsSynchronized && component.Team != null && component.Team.Side != BattleSideEnum.None && (intValue != 0 || !this.CheckIfEnforcedSpawnTimerExpiredForPeer(component)))
				{
					Team team = component.Team;
					bool flag = team == base.Mission.AttackerTeam;
					Team defenderTeam = base.Mission.DefenderTeam;
					BasicCultureObject basicCultureObject = (flag ? @object : object2);
					MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component, false);
					int num = ((this._flagDominationMissionController.GetMissionType() == MultiplayerGameType.Battle) ? mpheroClassForPeer.TroopBattleCost : mpheroClassForPeer.TroopCost);
					if (component.ControlledAgent == null && !component.HasSpawnedAgentVisuals && component.Team != null && !networkCommunicator.IsSpectator && component.Team != base.Mission.SpectatorTeam && component.TeamInitialPerkInfoReady && component.SpawnTimer.Check(base.Mission.CurrentTime))
					{
						int currentGoldForPeer = this._flagDominationMissionController.GetCurrentGoldForPeer(component);
						if (mpheroClassForPeer == null || (this._flagDominationMissionController.UseGold() && num > currentGoldForPeer))
						{
							if (currentGoldForPeer >= MultiplayerClassDivisions.GetMinimumTroopCost(basicCultureObject) && component.SelectedTroopIndex != 0)
							{
								component.SelectedTroopIndex = 0;
								GameNetwork.BeginBroadcastModuleEvent();
								GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(networkCommunicator, 0));
								GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkCommunicator);
							}
						}
						else
						{
							if (intValue == 0)
							{
								this.CreateEnforcedSpawnTimerForPeer(component, 15);
							}
							Formation formation = component.ControlledFormation;
							if (intValue > 0 && formation == null)
							{
								FormationClass formationIndex = component.Team.FormationsIncludingEmpty.First<Formation>((Formation x) => x.PlayerOwner == null && !x.ContainsAgentVisuals && x.CountOfUnits == 0).FormationIndex;
								formation = team.GetFormation(formationIndex);
								formation.ContainsAgentVisuals = true;
								if (string.IsNullOrEmpty(formation.BannerCode))
								{
									formation.BannerCode = component.Peer.BannerCode;
								}
							}
							MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = multiplayerBattleColors.GetPeerColors(component);
							BasicCharacterObject heroCharacter = mpheroClassForPeer.HeroCharacter;
							AgentBuildData agentBuildData = new AgentBuildData(heroCharacter).MissionPeer(component).Team(component.Team).VisualsIndex(0)
								.Formation(formation)
								.MakeUnitStandOutOfFormationDistance(7f)
								.IsFemale(component.Peer.IsFemale)
								.BodyProperties(base.GetBodyProperties(component, (component.Culture == @object) ? @object : object2))
								.ClothingColor1(peerColors.ClothingColor1Uint)
								.ClothingColor2(peerColors.ClothingColor2Uint);
							MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(component);
							Equipment equipment = heroCharacter.Equipment.Clone(false);
							IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
							if (enumerable != null)
							{
								foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
								{
									equipment[valueTuple.Item1] = valueTuple.Item2;
								}
							}
							int amountOfAgentVisualsForPeer = component.GetAmountOfAgentVisualsForPeer();
							bool flag2 = amountOfAgentVisualsForPeer > 0;
							agentBuildData.Equipment(equipment);
							if (intValue == 0)
							{
								if (!flag2)
								{
									MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(component.Team, equipment[EquipmentIndex.ArmorItemEndSlot].Item != null, true);
									agentBuildData.InitialPosition(in spawnFrame.origin);
									AgentBuildData agentBuildData2 = agentBuildData;
									Vec2 vec = spawnFrame.rotation.f.AsVec2;
									vec = vec.Normalized();
									agentBuildData2.InitialDirection(in vec);
								}
								else
								{
									MatrixFrame frame = component.GetAgentVisualForPeer(0).GetFrame();
									agentBuildData.InitialPosition(in frame.origin);
									AgentBuildData agentBuildData3 = agentBuildData;
									Vec2 vec = frame.rotation.f.AsVec2;
									vec = vec.Normalized();
									agentBuildData3.InitialDirection(in vec);
								}
							}
							if (this.GameMode.ShouldSpawnVisualsForServer(networkCommunicator))
							{
								base.AgentVisualSpawnComponent.SpawnAgentVisualsForPeer(component, agentBuildData, component.SelectedTroopIndex, false, 0);
								if (agentBuildData.AgentVisualsIndex == 0)
								{
									component.HasSpawnedAgentVisuals = true;
									component.EquipmentUpdatingExpired = false;
								}
							}
							this.GameMode.HandleAgentVisualSpawning(networkCommunicator, agentBuildData, 0, true);
							component.ControlledFormation = formation;
							if (intValue > 0)
							{
								int troopCount = MPPerkObject.GetTroopCount(mpheroClassForPeer, intValue, onSpawnPerkHandler);
								IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable2 = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(false) : null);
								for (int i = 0; i < troopCount; i++)
								{
									if (i + 1 >= amountOfAgentVisualsForPeer)
									{
										flag2 = false;
									}
									this.SpawnBotVisualsInPlayerFormation(component, i + 1, team, basicCultureObject, mpheroClassForPeer.TroopCharacter.StringId, formation, flag2, troopCount, enumerable2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x0009FAF4 File Offset: 0x0009DCF4
		private new void OnPeerSpawnedFromVisuals(MissionPeer peer)
		{
			if (peer.ControlledFormation != null)
			{
				peer.ControlledAgent.Team.AssignPlayerAsSergeantOfFormation(peer, peer.ControlledFormation.FormationIndex);
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x0009FB1C File Offset: 0x0009DD1C
		private new void OnAllAgentsFromPeerSpawnedFromVisuals(MissionPeer peer)
		{
			if (peer.ControlledFormation != null)
			{
				peer.ControlledFormation.OnFormationDispersed();
				peer.ControlledFormation.SetMovementOrder(MovementOrder.MovementOrderFollow(peer.ControlledAgent));
				NetworkCommunicator networkPeer = peer.GetNetworkPeer();
				if (peer.BotsUnderControlAlive != 0 || peer.BotsUnderControlTotal != 0)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new BotsControlledChange(networkPeer, peer.BotsUnderControlAlive, peer.BotsUnderControlTotal));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					base.Mission.GetMissionBehavior<MissionMultiplayerGameModeFlagDominationClient>().OnBotsControlledChanged(peer, peer.BotsUnderControlAlive, peer.BotsUnderControlTotal);
				}
				if (peer.Team == base.Mission.AttackerTeam)
				{
					base.Mission.NumOfFormationsSpawnedTeamOne++;
				}
				else
				{
					base.Mission.NumOfFormationsSpawnedTeamTwo++;
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetSpawnedFormationCount(base.Mission.NumOfFormationsSpawnedTeamOne, base.Mission.NumOfFormationsSpawnedTeamTwo));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this._flagDominationMissionController.UseGold())
			{
				bool flag = peer.Team == base.Mission.AttackerTeam;
				Team defenderTeam = base.Mission.DefenderTeam;
				MultiplayerClassDivisions.MPHeroClass mpheroClass = MultiplayerClassDivisions.GetMPHeroClasses(MBObjectManager.Instance.GetObject<BasicCultureObject>(flag ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))).ElementAt<MultiplayerClassDivisions.MPHeroClass>(peer.SelectedTroopIndex);
				int num = ((this._flagDominationMissionController.GetMissionType() == MultiplayerGameType.Battle) ? mpheroClass.TroopBattleCost : mpheroClass.TroopCost);
				this._flagDominationMissionController.ChangeCurrentGoldForPeer(peer, this._flagDominationMissionController.GetCurrentGoldForPeer(peer) - num);
			}
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x0009FCAC File Offset: 0x0009DEAC
		private void BotFormationSpawned(Team team)
		{
			if (team == base.Mission.AttackerTeam)
			{
				base.Mission.NumOfFormationsSpawnedTeamOne++;
				return;
			}
			if (team == base.Mission.DefenderTeam)
			{
				base.Mission.NumOfFormationsSpawnedTeamTwo++;
			}
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x0009FCFC File Offset: 0x0009DEFC
		private void AllBotFormationsSpawned()
		{
			if (base.Mission.NumOfFormationsSpawnedTeamOne != 0 || base.Mission.NumOfFormationsSpawnedTeamTwo != 0)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetSpawnedFormationCount(base.Mission.NumOfFormationsSpawnedTeamOne, base.Mission.NumOfFormationsSpawnedTeamTwo));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x0009FD50 File Offset: 0x0009DF50
		public override bool AllowEarlyAgentVisualsDespawning(MissionPeer lobbyPeer)
		{
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) != 0)
			{
				return false;
			}
			if (!this._roundController.IsRoundInProgress)
			{
				return false;
			}
			if (!lobbyPeer.HasSpawnTimerExpired && lobbyPeer.SpawnTimer.Check(Mission.Current.CurrentTime))
			{
				lobbyPeer.HasSpawnTimerExpired = true;
			}
			return lobbyPeer.HasSpawnTimerExpired;
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x0009FDA4 File Offset: 0x0009DFA4
		protected override bool IsRoundInProgress()
		{
			return this._roundController.IsRoundInProgress;
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x0009FDB4 File Offset: 0x0009DFB4
		private void CreateEnforcedSpawnTimerForPeer(MissionPeer peer, int durationInSeconds)
		{
			if (this._enforcedSpawnTimers.Any<KeyValuePair<MissionPeer, Timer>>((KeyValuePair<MissionPeer, Timer> pair) => pair.Key == peer))
			{
				return;
			}
			this._enforcedSpawnTimers.Add(new KeyValuePair<MissionPeer, Timer>(peer, new Timer(base.Mission.CurrentTime, (float)durationInSeconds, true)));
			Debug.Print(string.Concat(new object[] { "EST for ", peer.Name, " set to ", durationInSeconds, " seconds." }), 0, Debug.DebugColor.Yellow, 64UL);
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x0009FE58 File Offset: 0x0009E058
		private bool CheckIfEnforcedSpawnTimerExpiredForPeer(MissionPeer peer)
		{
			KeyValuePair<MissionPeer, Timer> keyValuePair = this._enforcedSpawnTimers.FirstOrDefault<KeyValuePair<MissionPeer, Timer>>((KeyValuePair<MissionPeer, Timer> pr) => pr.Key == peer);
			if (keyValuePair.Key == null)
			{
				return false;
			}
			if (peer.ControlledAgent != null)
			{
				this._enforcedSpawnTimers.RemoveAll((KeyValuePair<MissionPeer, Timer> p) => p.Key == peer);
				Debug.Print("EST for " + peer.Name + " is no longer valid (spawned already).", 0, Debug.DebugColor.Yellow, 64UL);
				return false;
			}
			Timer value = keyValuePair.Value;
			if (peer.HasSpawnedAgentVisuals && value.Check(Mission.Current.CurrentTime))
			{
				this.SpawnComponent.SetEarlyAgentVisualsDespawning(peer, true);
				this._enforcedSpawnTimers.RemoveAll((KeyValuePair<MissionPeer, Timer> p) => p.Key == peer);
				Debug.Print("EST for " + peer.Name + " has expired.", 0, Debug.DebugColor.Yellow, 64UL);
				return true;
			}
			return false;
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x0009FF5A File Offset: 0x0009E15A
		public override void OnClearScene()
		{
			base.OnClearScene();
			this._enforcedSpawnTimers.Clear();
			this._roundInitialSpawnOver = false;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x0009FF74 File Offset: 0x0009E174
		protected void SpawnBotInBotFormation(int visualsIndex, Team agentTeam, BasicCultureObject cultureLimit, BasicCharacterObject character, Formation formation)
		{
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = ((cultureLimit == @object) ? multiplayerBattleColors.AttackerColors : multiplayerBattleColors.DefenderColors);
			AgentBuildData agentBuildData = new AgentBuildData(character).Team(agentTeam).TroopOrigin(new BasicBattleAgentOrigin(character)).VisualsIndex(visualsIndex)
				.EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(character, visualsIndex))
				.Formation(formation)
				.IsFemale(character.IsFemale)
				.ClothingColor1(multiplayerCultureColorInfo.ClothingColor1Uint)
				.ClothingColor2(multiplayerCultureColorInfo.ClothingColor2Uint);
			agentBuildData.Equipment(Equipment.GetRandomEquipmentElements(character, !GameNetwork.IsMultiplayer, Equipment.EquipmentType.Battle, agentBuildData.AgentEquipmentSeed));
			agentBuildData.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData.AgentRace, agentBuildData.AgentIsFemale, character.GetBodyPropertiesMin(false), character.GetBodyPropertiesMax(false), (int)agentBuildData.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData.AgentEquipmentSeed, character.BodyPropertyRange.HairTags, character.BodyPropertyRange.BeardTags, character.BodyPropertyRange.TattooTags, 0f));
			base.Mission.SpawnAgent(agentBuildData, false, null, null).SetAlarmState(Agent.AIStateFlag.Alarmed);
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000A00BC File Offset: 0x0009E2BC
		protected void SpawnBotVisualsInPlayerFormation(MissionPeer missionPeer, int visualsIndex, Team agentTeam, BasicCultureObject cultureLimit, string troopName, Formation formation, bool updateExistingAgentVisuals, int totalCount, IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments)
		{
			BasicCharacterObject @object = MBObjectManager.Instance.GetObject<BasicCharacterObject>(troopName);
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object3 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(object2, object3);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = ((cultureLimit == object2) ? multiplayerBattleColors.AttackerColors : multiplayerBattleColors.DefenderColors);
			AgentBuildData agentBuildData = new AgentBuildData(@object).Team(agentTeam).OwningMissionPeer(missionPeer).VisualsIndex(visualsIndex)
				.TroopOrigin(new BasicBattleAgentOrigin(@object))
				.EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(@object, visualsIndex))
				.Formation(formation)
				.IsFemale(@object.IsFemale)
				.ClothingColor1(multiplayerCultureColorInfo.ClothingColor1Uint)
				.ClothingColor2(multiplayerCultureColorInfo.ClothingColor2Uint);
			Equipment randomEquipmentElements = Equipment.GetRandomEquipmentElements(@object, !GameNetwork.IsMultiplayer, Equipment.EquipmentType.Battle, MBRandom.RandomInt());
			if (alternativeEquipments != null)
			{
				foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in alternativeEquipments)
				{
					randomEquipmentElements[valueTuple.Item1] = valueTuple.Item2;
				}
			}
			agentBuildData.Equipment(randomEquipmentElements);
			agentBuildData.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData.AgentRace, agentBuildData.AgentIsFemale, @object.GetBodyPropertiesMin(false), @object.GetBodyPropertiesMax(false), (int)agentBuildData.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData.AgentEquipmentSeed, @object.BodyPropertyRange.HairTags, @object.BodyPropertyRange.BeardTags, @object.BodyPropertyRange.TattooTags, 0f));
			NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
			if (this.GameMode.ShouldSpawnVisualsForServer(networkPeer))
			{
				base.AgentVisualSpawnComponent.SpawnAgentVisualsForPeer(missionPeer, agentBuildData, -1, true, totalCount);
				if (agentBuildData.AgentVisualsIndex == 0)
				{
					missionPeer.HasSpawnedAgentVisuals = true;
					missionPeer.EquipmentUpdatingExpired = false;
				}
			}
			this.GameMode.HandleAgentVisualSpawning(networkPeer, agentBuildData, totalCount, false);
		}

		// Token: 0x0400102B RID: 4139
		private const int EnforcedSpawnTimeInSeconds = 15;

		// Token: 0x0400102C RID: 4140
		private float _spawningTimer;

		// Token: 0x0400102D RID: 4141
		private bool _spawningTimerTicking;

		// Token: 0x0400102E RID: 4142
		private bool _roundInitialSpawnOver;

		// Token: 0x0400102F RID: 4143
		private MissionMultiplayerFlagDomination _flagDominationMissionController;

		// Token: 0x04001030 RID: 4144
		private MultiplayerRoundController _roundController;

		// Token: 0x04001031 RID: 4145
		private List<KeyValuePair<MissionPeer, Timer>> _enforcedSpawnTimers;
	}
}
