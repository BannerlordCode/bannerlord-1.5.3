using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DA RID: 730
	public abstract class SpawningBehaviorBase
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x000A06FA File Offset: 0x0009E8FA
		// (set) Token: 0x06002A3F RID: 10815 RVA: 0x000A0702 File Offset: 0x0009E902
		private protected MultiplayerMissionAgentVisualSpawnComponent AgentVisualSpawnComponent { protected get; private set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x000A070B File Offset: 0x0009E90B
		protected Mission Mission
		{
			get
			{
				return this.SpawnComponent.Mission;
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06002A41 RID: 10817 RVA: 0x000A0718 File Offset: 0x0009E918
		// (remove) Token: 0x06002A42 RID: 10818 RVA: 0x000A0750 File Offset: 0x0009E950
		protected event Action<MissionPeer> OnAllAgentsFromPeerSpawnedFromVisuals;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06002A43 RID: 10819 RVA: 0x000A0788 File Offset: 0x0009E988
		// (remove) Token: 0x06002A44 RID: 10820 RVA: 0x000A07C0 File Offset: 0x0009E9C0
		protected event Action<MissionPeer> OnPeerSpawnedFromVisuals;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06002A45 RID: 10821 RVA: 0x000A07F8 File Offset: 0x0009E9F8
		// (remove) Token: 0x06002A46 RID: 10822 RVA: 0x000A0830 File Offset: 0x0009EA30
		public event SpawningBehaviorBase.OnSpawningEndedEventDelegate OnSpawningEnded;

		// Token: 0x06002A47 RID: 10823 RVA: 0x000A0868 File Offset: 0x0009EA68
		public virtual void Initialize(SpawnComponent spawnComponent)
		{
			this.SpawnComponent = spawnComponent;
			this.AgentVisualSpawnComponent = this.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			this.GameMode = this.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			this.MissionLobbyComponent = this.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this.MissionLobbyEquipmentNetworkComponent = this.Mission.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this.MissionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed += this.OnPeerEquipmentUpdated;
			this.SpawnCheckTimer = new Timer(Mission.Current.CurrentTime, 0.2f, true);
			this._agentsToBeSpawnedCache = new List<AgentBuildData>();
			this._nextTimeToCleanUpMounts = MissionTime.Now;
			this._botsCountForSides = new int[2];
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000A0914 File Offset: 0x0009EB14
		public virtual void Clear()
		{
			this.MissionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed -= this.OnPeerEquipmentUpdated;
			this._agentsToBeSpawnedCache = null;
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000A0934 File Offset: 0x0009EB34
		public virtual void OnTick(float dt)
		{
			int count = Mission.Current.AllAgents.Count;
			int num = 0;
			this._agentsToBeSpawnedCache.Clear();
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.IsSynchronized)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.ControlledAgent == null && component.HasSpawnedAgentVisuals && !this.CanUpdateSpawnEquipment(component))
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component, false);
						MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(component);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SyncPerksForCurrentlySelectedTroop(networkCommunicator, component.Perks[component.SelectedTroopIndex]));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkCommunicator);
						int num2 = 0;
						bool flag = false;
						int intValue = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						if (intValue > 0 && (this.GameMode.WarmupComponent == null || !this.GameMode.WarmupComponent.IsInWarmup))
						{
							num2 = MPPerkObject.GetTroopCount(mpheroClassForPeer, intValue, onSpawnPerkHandler);
							using (List<MPPerkObject>.Enumerator enumerator2 = component.SelectedPerks.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (enumerator2.Current.HasBannerBearer)
									{
										flag = true;
										break;
									}
								}
							}
						}
						if (num2 > 0)
						{
							num2 = (int)((float)num2 * this.GameMode.GetTroopNumberMultiplierForMissingPlayer(component));
						}
						num2 += (flag ? 2 : 1);
						IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(false) : null);
						int i = 0;
						while (i < num2)
						{
							bool flag2 = i == 0;
							BasicCharacterObject basicCharacterObject = (flag2 ? mpheroClassForPeer.HeroCharacter : ((flag && i == 1) ? mpheroClassForPeer.BannerBearerCharacter : mpheroClassForPeer.TroopCharacter));
							MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = multiplayerBattleColors.GetPeerColors(component);
							uint clothingColor1Uint = peerColors.ClothingColor1Uint;
							uint clothingColor2Uint = peerColors.ClothingColor2Uint;
							uint bannerBackgroundColorUint = peerColors.BannerBackgroundColorUint;
							uint bannerForegroundColorUint = peerColors.BannerForegroundColorUint;
							Banner banner = new Banner(component.Peer.BannerCode, bannerBackgroundColorUint, bannerForegroundColorUint);
							AgentBuildData agentBuildData = new AgentBuildData(basicCharacterObject).VisualsIndex(i).Team(component.Team).TroopOrigin(new BasicBattleAgentOrigin(basicCharacterObject))
								.Formation(component.ControlledFormation)
								.IsFemale(flag2 ? component.Peer.IsFemale : basicCharacterObject.IsFemale)
								.ClothingColor1(clothingColor1Uint)
								.ClothingColor2(clothingColor2Uint)
								.Banner(banner);
							if (flag2)
							{
								agentBuildData.MissionPeer(component);
							}
							else
							{
								agentBuildData.OwningMissionPeer(component);
							}
							Equipment equipment = (flag2 ? basicCharacterObject.Equipment.Clone(false) : Equipment.GetRandomEquipmentElements(basicCharacterObject, false, Equipment.EquipmentType.Battle, MBRandom.RandomInt()));
							IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable2 = (flag2 ? ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null) : enumerable);
							if (enumerable2 != null)
							{
								foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable2)
								{
									equipment[valueTuple.Item1] = valueTuple.Item2;
								}
							}
							agentBuildData.Equipment(equipment);
							if (flag2)
							{
								this.GameMode.AddCosmeticItemsToEquipment(equipment, this.GameMode.GetUsedCosmeticsFromPeer(component, basicCharacterObject));
							}
							if (flag2)
							{
								agentBuildData.BodyProperties(this.GetBodyProperties(component, component.Culture));
								agentBuildData.Age((int)agentBuildData.AgentBodyProperties.Age);
							}
							else
							{
								agentBuildData.EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(basicCharacterObject, agentBuildData.AgentVisualsIndex));
								agentBuildData.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData.AgentRace, agentBuildData.AgentIsFemale, basicCharacterObject.GetBodyPropertiesMin(false), basicCharacterObject.GetBodyPropertiesMax(false), (int)agentBuildData.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData.AgentEquipmentSeed, basicCharacterObject.BodyPropertyRange.HairTags, basicCharacterObject.BodyPropertyRange.BeardTags, basicCharacterObject.BodyPropertyRange.TattooTags, 0f));
							}
							if (component.ControlledFormation != null && component.ControlledFormation.Banner == null)
							{
								component.ControlledFormation.Banner = banner;
							}
							MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(component.Team, equipment[EquipmentIndex.ArmorItemEndSlot].Item != null, component.SpawnCountThisRound == 0);
							if (spawnFrame.IsIdentity)
							{
								goto IL_04FF;
							}
							Vec2 vec;
							if (!(spawnFrame.origin != agentBuildData.AgentInitialPosition))
							{
								vec = spawnFrame.rotation.f.AsVec2.Normalized();
								Vec2? agentInitialDirection = agentBuildData.AgentInitialDirection;
								if (!(vec != agentInitialDirection))
								{
									goto IL_04FF;
								}
							}
							agentBuildData.InitialPosition(in spawnFrame.origin);
							AgentBuildData agentBuildData2 = agentBuildData;
							vec = spawnFrame.rotation.f.AsVec2;
							vec = vec.Normalized();
							agentBuildData2.InitialDirection(in vec);
							IL_0518:
							if (component.ControlledAgent != null && !flag2)
							{
								MatrixFrame frame = component.ControlledAgent.Frame;
								frame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
								MatrixFrame matrixFrame = frame;
								matrixFrame.origin -= matrixFrame.rotation.f.NormalizedCopy() * 3.5f;
								Mat3 rotation = matrixFrame.rotation;
								rotation.MakeUnit();
								bool flag3 = !basicCharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty;
								int num3 = MathF.Min(num2, 10);
								MatrixFrame matrixFrame2 = Formation.GetFormationFramesForBeforeFormationCreation((float)num3 * Formation.GetDefaultUnitDiameter(flag3) + (float)(num3 - 1) * Formation.GetDefaultMinimumUnitInterval(flag3), num2, flag3, new WorldPosition(Mission.Current.Scene, matrixFrame.origin), rotation)[i - 1].ToGroundMatrixFrame();
								agentBuildData.InitialPosition(in matrixFrame2.origin);
								AgentBuildData agentBuildData3 = agentBuildData;
								vec = matrixFrame2.rotation.f.AsVec2;
								vec = vec.Normalized();
								agentBuildData3.InitialDirection(in vec);
							}
							this._agentsToBeSpawnedCache.Add(agentBuildData);
							num++;
							if (!agentBuildData.AgentOverridenSpawnEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
							{
								num++;
							}
							i++;
							continue;
							IL_04FF:
							Debug.FailedAssert("Spawn frame could not be found.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\SpawnBehaviors\\SpawningBehaviors\\SpawningBehaviorBase.cs", "OnTick", 213);
							goto IL_0518;
						}
					}
				}
			}
			int num4 = num + count;
			if (num4 > SpawningBehaviorBase._agentCountThreshold && this._nextTimeToCleanUpMounts.IsPast)
			{
				this._nextTimeToCleanUpMounts = MissionTime.SecondsFromNow(5f);
				for (int j = Mission.Current.MountsWithoutRiders.Count - 1; j >= 0; j--)
				{
					KeyValuePair<Agent, MissionTime> keyValuePair = Mission.Current.MountsWithoutRiders[j];
					Agent key = keyValuePair.Key;
					if (keyValuePair.Value.ElapsedSeconds > 30f)
					{
						key.FadeOut(false, false);
					}
				}
			}
			int num5 = SpawningBehaviorBase._maxAgentCount - num4;
			if (num5 >= 0)
			{
				for (int k = this._agentsToBeSpawnedCache.Count - 1; k >= 0; k--)
				{
					AgentBuildData agentBuildData4 = this._agentsToBeSpawnedCache[k];
					bool flag4 = agentBuildData4.AgentMissionPeer != null;
					MissionPeer missionPeer = (flag4 ? agentBuildData4.AgentMissionPeer : agentBuildData4.OwningAgentMissionPeer);
					MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler2 = MPPerkObject.GetOnSpawnPerkHandler(missionPeer);
					Agent agent = this.Mission.SpawnAgent(agentBuildData4, true, null, null);
					agent.AddComponent(new MPPerksAgentComponent(agent));
					Agent mountAgent = agent.MountAgent;
					if (mountAgent != null)
					{
						mountAgent.UpdateAgentProperties();
					}
					agent.HealthLimit += ((onSpawnPerkHandler2 != null) ? onSpawnPerkHandler2.GetHitpoints(flag4) : 0f);
					agent.Health = agent.HealthLimit;
					if (!flag4)
					{
						agent.SetWatchState(Agent.WatchState.Alarmed);
					}
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
					if (flag4)
					{
						MissionPeer missionPeer2 = missionPeer;
						int spawnCountThisRound = missionPeer2.SpawnCountThisRound;
						missionPeer2.SpawnCountThisRound = spawnCountThisRound + 1;
						Action<MissionPeer> onPeerSpawnedFromVisuals = this.OnPeerSpawnedFromVisuals;
						if (onPeerSpawnedFromVisuals != null)
						{
							onPeerSpawnedFromVisuals(missionPeer);
						}
						Action<MissionPeer> onAllAgentsFromPeerSpawnedFromVisuals = this.OnAllAgentsFromPeerSpawnedFromVisuals;
						if (onAllAgentsFromPeerSpawnedFromVisuals != null)
						{
							onAllAgentsFromPeerSpawnedFromVisuals(missionPeer);
						}
						this.AgentVisualSpawnComponent.RemoveAgentVisuals(missionPeer, true);
						if (GameNetwork.IsServerOrRecorder)
						{
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(missionPeer.GetNetworkPeer()));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						}
						missionPeer.HasSpawnedAgentVisuals = false;
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(missionPeer);
						if (perkHandler != null)
						{
							perkHandler.OnEvent(MPPerkCondition.PerkEventFlags.SpawnEnd);
						}
					}
				}
				int intValue2 = MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				int intValue3 = MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				if (this.GameMode.IsGameModeUsingOpposingTeams && (intValue2 > 0 || intValue3 > 0))
				{
					ValueTuple<Team, BasicCultureObject, int>[] array = new ValueTuple<Team, BasicCultureObject, int>[]
					{
						new ValueTuple<Team, BasicCultureObject, int>(this.Mission.DefenderTeam, MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)), intValue3 - this._botsCountForSides[0]),
						new ValueTuple<Team, BasicCultureObject, int>(this.Mission.AttackerTeam, MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)), intValue2 - this._botsCountForSides[1])
					};
					if (num5 >= 4)
					{
						int l = Math.Min(num5 / 2, array[0].Item3 + array[1].Item3);
						BattleSideEnum battleSideEnum = BattleSideEnum.Defender;
						while (l > 0)
						{
							int num6 = (int)battleSideEnum;
							if (array[num6].Item3 > 0)
							{
								this.SpawnBot(array[num6].Item1, array[num6].Item2);
								ValueTuple<Team, BasicCultureObject, int>[] array2 = array;
								int num7 = num6;
								array2[num7].Item3 = array2[num7].Item3 - 1;
								l--;
							}
							battleSideEnum = battleSideEnum.GetOppositeSide();
						}
					}
				}
			}
			if (!this.IsSpawningEnabled && this.IsRoundInProgress())
			{
				if (this.SpawningDelayTimer >= this.SpawningEndDelay && !this._hasCalledSpawningEnded)
				{
					Mission.Current.AllowAiTicking = true;
					if (this.OnSpawningEnded != null)
					{
						this.OnSpawningEnded();
					}
					this._hasCalledSpawningEnded = true;
				}
				this.SpawningDelayTimer += dt;
			}
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000A13A4 File Offset: 0x0009F5A4
		public bool AreAgentsSpawning()
		{
			return this.IsSpawningEnabled;
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000A13AC File Offset: 0x0009F5AC
		protected void ResetSpawnCounts()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.SpawnCountThisRound = 0;
				}
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000A1408 File Offset: 0x0009F608
		protected void ResetSpawnTimers()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.SpawnTimer.Reset(Mission.Current.CurrentTime, 0f);
				}
			}
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000A1478 File Offset: 0x0009F678
		public virtual void RequestStartSpawnSession()
		{
			this.IsSpawningEnabled = true;
			this.SpawningDelayTimer = 0f;
			this._hasCalledSpawningEnded = false;
			this.ResetSpawnCounts();
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000A149C File Offset: 0x0009F69C
		public void RequestStopSpawnSession()
		{
			this.IsSpawningEnabled = false;
			this.SpawningDelayTimer = 0f;
			this._hasCalledSpawningEnded = false;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					this.AgentVisualSpawnComponent.RemoveAgentVisuals(component, true);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(component.GetNetworkPeer()));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					component.HasSpawnedAgentVisuals = false;
				}
			}
			foreach (NetworkCommunicator networkCommunicator2 in GameNetwork.DisconnectedNetworkPeers)
			{
				MissionPeer missionPeer = ((networkCommunicator2 != null) ? networkCommunicator2.GetComponent<MissionPeer>() : null);
				if (missionPeer != null)
				{
					this.AgentVisualSpawnComponent.RemoveAgentVisuals(missionPeer, false);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(missionPeer.GetNetworkPeer()));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					missionPeer.HasSpawnedAgentVisuals = false;
				}
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000A15C0 File Offset: 0x0009F7C0
		public void SetRemainingAgentsInvulnerable()
		{
			foreach (Agent agent in this.Mission.Agents)
			{
				agent.SetMortalityState(Agent.MortalityState.Invulnerable);
			}
		}

		// Token: 0x06002A50 RID: 10832
		protected abstract void SpawnAgents();

		// Token: 0x06002A51 RID: 10833 RVA: 0x000A1618 File Offset: 0x0009F818
		protected BodyProperties GetBodyProperties(MissionPeer missionPeer, BasicCultureObject cultureLimit)
		{
			NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
			if (networkPeer != null)
			{
				return networkPeer.PlayerConnectionInfo.GetParameter<PlayerData>("PlayerData").BodyProperties;
			}
			Debug.FailedAssert("networkCommunicator != null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\SpawnBehaviors\\SpawningBehaviors\\SpawningBehaviorBase.cs", "GetBodyProperties", 518);
			Team team = missionPeer.Team;
			BasicCharacterObject troopCharacter = MultiplayerClassDivisions.GetMPHeroClasses(cultureLimit).ToMBList<MultiplayerClassDivisions.MPHeroClass>().GetRandomElement<MultiplayerClassDivisions.MPHeroClass>()
				.TroopCharacter;
			MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(team, troopCharacter.HasMount(), true);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = ((cultureLimit == @object) ? multiplayerBattleColors.AttackerColors : multiplayerBattleColors.DefenderColors);
			AgentBuildData agentBuildData = new AgentBuildData(troopCharacter).Team(team).InitialPosition(in spawnFrame.origin);
			Vec2 vec = spawnFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).TroopOrigin(new BasicBattleAgentOrigin(troopCharacter)).EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(troopCharacter, 0))
				.ClothingColor1(multiplayerCultureColorInfo.ClothingColor1Uint)
				.ClothingColor2(multiplayerCultureColorInfo.ClothingColor2Uint)
				.IsFemale(troopCharacter.IsFemale);
			agentBuildData2.Equipment(Equipment.GetRandomEquipmentElements(troopCharacter, !GameNetwork.IsMultiplayer, Equipment.EquipmentType.Battle, agentBuildData2.AgentEquipmentSeed));
			agentBuildData2.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData2.AgentRace, agentBuildData2.AgentIsFemale, troopCharacter.GetBodyPropertiesMin(false), troopCharacter.GetBodyPropertiesMax(false), (int)agentBuildData2.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData2.AgentEquipmentSeed, troopCharacter.BodyPropertyRange.HairTags, troopCharacter.BodyPropertyRange.BeardTags, troopCharacter.BodyPropertyRange.TattooTags, 0f));
			return agentBuildData2.AgentBodyProperties;
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000A17E0 File Offset: 0x0009F9E0
		protected void SpawnBot(Team agentTeam, BasicCultureObject cultureLimit)
		{
			BasicCharacterObject troopCharacter = MultiplayerClassDivisions.GetMPHeroClasses(cultureLimit).ToMBList<MultiplayerClassDivisions.MPHeroClass>().GetRandomElement<MultiplayerClassDivisions.MPHeroClass>()
				.TroopCharacter;
			MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(agentTeam, troopCharacter.HasMount(), true);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = ((cultureLimit == @object) ? multiplayerBattleColors.AttackerColors : multiplayerBattleColors.DefenderColors);
			AgentBuildData agentBuildData = new AgentBuildData(troopCharacter).Team(agentTeam).InitialPosition(in spawnFrame.origin);
			Vec2 vec = spawnFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).TroopOrigin(new BasicBattleAgentOrigin(troopCharacter)).EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(troopCharacter, 0))
				.ClothingColor1(multiplayerCultureColorInfo.ClothingColor1Uint)
				.ClothingColor2(multiplayerCultureColorInfo.ClothingColor2Uint)
				.IsFemale(troopCharacter.IsFemale);
			agentBuildData2.Equipment(Equipment.GetRandomEquipmentElements(troopCharacter, !GameNetwork.IsMultiplayer, Equipment.EquipmentType.Battle, agentBuildData2.AgentEquipmentSeed));
			agentBuildData2.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData2.AgentRace, agentBuildData2.AgentIsFemale, troopCharacter.GetBodyPropertiesMin(false), troopCharacter.GetBodyPropertiesMax(false), (int)agentBuildData2.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData2.AgentEquipmentSeed, troopCharacter.BodyPropertyRange.HairTags, troopCharacter.BodyPropertyRange.BeardTags, troopCharacter.BodyPropertyRange.TattooTags, 0f));
			Agent agent = this.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
			this._botsCountForSides[(int)agent.Team.Side]++;
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000A1990 File Offset: 0x0009FB90
		private void OnPeerEquipmentUpdated(MissionPeer peer)
		{
			if (this.IsSpawningEnabled && this.CanUpdateSpawnEquipment(peer))
			{
				peer.HasSpawnedAgentVisuals = false;
				Debug.Print("HasSpawnedAgentVisuals = false for peer: " + peer.Name + " because he just updated his equipment", 0, Debug.DebugColor.White, 17592186044416UL);
				if (peer.ControlledFormation != null)
				{
					peer.ControlledFormation.HasBeenPositioned = false;
					peer.ControlledFormation.SetSpawnIndex(0);
				}
			}
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000A19FB File Offset: 0x0009FBFB
		public virtual bool CanUpdateSpawnEquipment(MissionPeer missionPeer)
		{
			return !missionPeer.EquipmentUpdatingExpired && !this._equipmentUpdatingExpired;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000A1A10 File Offset: 0x0009FC10
		public void ToggleUpdatingSpawnEquipment(bool canUpdate)
		{
			this._equipmentUpdatingExpired = !canUpdate;
		}

		// Token: 0x06002A56 RID: 10838
		public abstract bool AllowEarlyAgentVisualsDespawning(MissionPeer missionPeer);

		// Token: 0x06002A57 RID: 10839 RVA: 0x000A1A1C File Offset: 0x0009FC1C
		public virtual int GetMaximumReSpawnPeriodForPeer(MissionPeer peer)
		{
			return 3;
		}

		// Token: 0x06002A58 RID: 10840
		protected abstract bool IsRoundInProgress();

		// Token: 0x06002A59 RID: 10841 RVA: 0x000A1A20 File Offset: 0x0009FC20
		public virtual void OnClearScene()
		{
			for (int i = 0; i < this._botsCountForSides.Length; i++)
			{
				this._botsCountForSides[i] = 0;
			}
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000A1A49 File Offset: 0x0009FC49
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.IsHuman && affectedAgent.MissionPeer == null && affectedAgent.OwningAgentMissionPeer == null)
			{
				this._botsCountForSides[(int)affectedAgent.Team.Side]--;
			}
		}

		// Token: 0x04001032 RID: 4146
		private const float SecondsToWaitForEachMountBeforeSelectingToFadeOut = 30f;

		// Token: 0x04001033 RID: 4147
		private const float SecondsToWaitBeforeNextMountCleanup = 5f;

		// Token: 0x04001034 RID: 4148
		private static readonly int _maxAgentCount = MBAPI.IMBAgent.GetMaximumNumberOfAgents();

		// Token: 0x04001035 RID: 4149
		private static readonly int _agentCountThreshold = (int)((float)SpawningBehaviorBase._maxAgentCount * 0.9f);

		// Token: 0x04001037 RID: 4151
		protected MissionMultiplayerGameModeBase GameMode;

		// Token: 0x04001038 RID: 4152
		protected SpawnComponent SpawnComponent;

		// Token: 0x04001039 RID: 4153
		private bool _equipmentUpdatingExpired;

		// Token: 0x0400103A RID: 4154
		protected bool IsSpawningEnabled;

		// Token: 0x0400103B RID: 4155
		protected Timer SpawnCheckTimer;

		// Token: 0x0400103C RID: 4156
		protected float SpawningEndDelay = 1f;

		// Token: 0x0400103D RID: 4157
		protected float SpawningDelayTimer;

		// Token: 0x0400103E RID: 4158
		private bool _hasCalledSpawningEnded;

		// Token: 0x0400103F RID: 4159
		protected MissionLobbyComponent MissionLobbyComponent;

		// Token: 0x04001040 RID: 4160
		protected MissionLobbyEquipmentNetworkComponent MissionLobbyEquipmentNetworkComponent;

		// Token: 0x04001043 RID: 4163
		private List<AgentBuildData> _agentsToBeSpawnedCache;

		// Token: 0x04001044 RID: 4164
		private MissionTime _nextTimeToCleanUpMounts;

		// Token: 0x04001046 RID: 4166
		private int[] _botsCountForSides;

		// Token: 0x020005C4 RID: 1476
		// (Invoke) Token: 0x06003F19 RID: 16153
		public delegate void OnSpawningEndedEventDelegate();
	}
}
