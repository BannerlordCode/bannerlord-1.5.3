using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BD RID: 701
	public abstract class MissionMultiplayerGameModeBase : MissionNetwork
	{
		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002824 RID: 10276
		public abstract bool IsGameModeHidingAllAgentVisuals { get; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002825 RID: 10277
		public abstract bool IsGameModeUsingOpposingTeams { get; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x00096EAA File Offset: 0x000950AA
		public virtual bool IsGameModeAllowChargeDamageOnFriendly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x00096EAD File Offset: 0x000950AD
		// (set) Token: 0x06002828 RID: 10280 RVA: 0x00096EB5 File Offset: 0x000950B5
		public SpawnComponent SpawnComponent { get; private set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x00096EBE File Offset: 0x000950BE
		// (set) Token: 0x0600282A RID: 10282 RVA: 0x00096EC6 File Offset: 0x000950C6
		private protected bool CanGameModeSystemsTickThisFrame { protected get; private set; }

		// Token: 0x0600282B RID: 10283
		public abstract MultiplayerGameType GetMissionType();

		// Token: 0x0600282C RID: 10284 RVA: 0x00096ECF File Offset: 0x000950CF
		public virtual bool CheckIfOvertime()
		{
			return false;
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x00096ED4 File Offset: 0x000950D4
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.MultiplayerTeamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			this.MissionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this.GameModeBaseClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.NotificationsComponent = base.Mission.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
			this.RoundController = base.Mission.GetMissionBehavior<MultiplayerRoundController>();
			this.WarmupComponent = base.Mission.GetMissionBehavior<MultiplayerWarmupComponent>();
			this.TimerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
			this.SpawnComponent = Mission.Current.GetMissionBehavior<SpawnComponent>();
			this._agentVisualSpawnComponent = base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			this._lastPerkTickTime = Mission.Current.CurrentTime;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x00096F90 File Offset: 0x00095190
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (Mission.Current.CurrentTime - this._lastPerkTickTime >= 1f)
			{
				this._lastPerkTickTime = Mission.Current.CurrentTime;
				MPPerkObject.TickAllPeerPerks((int)(this._lastPerkTickTime / 1f));
			}
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x00096FDE File Offset: 0x000951DE
		public virtual bool CheckForWarmupEnd()
		{
			return false;
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x00096FE1 File Offset: 0x000951E1
		public virtual bool CheckForRoundEnd()
		{
			return false;
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x00096FE4 File Offset: 0x000951E4
		public virtual bool CheckForMatchEnd()
		{
			return false;
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x00096FE7 File Offset: 0x000951E7
		public virtual bool UseCultureSelection()
		{
			return false;
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x00096FEA File Offset: 0x000951EA
		public virtual bool UseRoundController()
		{
			return false;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x00096FED File Offset: 0x000951ED
		public virtual Team GetWinnerTeam()
		{
			return null;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x00096FF0 File Offset: 0x000951F0
		public virtual void OnPeerChangedTeam(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x00096FF2 File Offset: 0x000951F2
		public override void OnClearScene()
		{
			base.OnClearScene();
			if (this.RoundController == null)
			{
				this.ClearPeerCounts();
			}
			this._lastPerkTickTime = Mission.Current.CurrentTime;
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x00097018 File Offset: 0x00095218
		public void ClearPeerCounts()
		{
			List<MissionPeer> list = VirtualPlayer.Peers<MissionPeer>();
			for (int i = 0; i < list.Count; i++)
			{
				MissionPeer missionPeer = list[i];
				missionPeer.AssistCount = 0;
				missionPeer.DeathCount = 0;
				missionPeer.KillCount = 0;
				missionPeer.Score = 0;
				missionPeer.ResetRequestedKickPollCount();
				missionPeer.ResetKillRegistry();
				missionPeer.ResetSpectatorStats();
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new KillDeathCountChange(missionPeer.GetNetworkPeer(), null, missionPeer.KillCount, missionPeer.AssistCount, missionPeer.DeathCount, missionPeer.Score));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
				if (networkPeer != null)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new PeerLastKillChange(networkPeer, string.Empty));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new PeerMostUsedWeaponChange(networkPeer, WeaponClass.Undefined));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000970EC File Offset: 0x000952EC
		public bool ShouldSpawnVisualsForServer(NetworkCommunicator spawningNetworkPeer)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return false;
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				MissionPeer component = spawningNetworkPeer.GetComponent<MissionPeer>();
				return (!this.IsGameModeHidingAllAgentVisuals && component.Team == missionPeer.Team) || spawningNetworkPeer.IsServerPeer;
			}
			return false;
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x0009714C File Offset: 0x0009534C
		public void HandleAgentVisualSpawning(NetworkCommunicator spawningNetworkPeer, AgentBuildData spawningAgentBuildData, int troopCountInFormation = 0, bool useCosmetics = true)
		{
			MissionPeer component = spawningNetworkPeer.GetComponent<MissionPeer>();
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new SyncPerksForCurrentlySelectedTroop(spawningNetworkPeer, component.Perks[component.SelectedTroopIndex]));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, spawningNetworkPeer);
			component.HasSpawnedAgentVisuals = true;
			component.EquipmentUpdatingExpired = false;
			if (useCosmetics)
			{
				this.AddCosmeticItemsToEquipment(spawningAgentBuildData.AgentOverridenSpawnEquipment, this.GetUsedCosmeticsFromPeer(component, spawningAgentBuildData.AgentCharacter));
			}
			if (!this.IsGameModeHidingAllAgentVisuals)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CreateAgentVisuals(spawningNetworkPeer, spawningAgentBuildData, component.SelectedTroopIndex, troopCountInFormation));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, spawningNetworkPeer);
				return;
			}
			if (!spawningNetworkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(spawningNetworkPeer);
				GameNetwork.WriteMessage(new CreateAgentVisuals(spawningNetworkPeer, spawningAgentBuildData, component.SelectedTroopIndex, troopCountInFormation));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x00097203 File Offset: 0x00095403
		public virtual bool AllowCustomPlayerBanners()
		{
			return true;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x00097206 File Offset: 0x00095406
		public virtual int GetScoreForKill(Agent killedAgent)
		{
			return 20;
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x0009720A File Offset: 0x0009540A
		public virtual float GetTroopNumberMultiplierForMissingPlayer(MissionPeer spawningPeer)
		{
			return 1f;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00097211 File Offset: 0x00095411
		public int GetCurrentGoldForPeer(MissionPeer peer)
		{
			return peer.Representative.Gold;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x00097220 File Offset: 0x00095420
		public void ChangeCurrentGoldForPeer(MissionPeer peer, int newAmount)
		{
			if (newAmount >= 0)
			{
				newAmount = MBMath.ClampInt(newAmount, 0, 2000);
			}
			if (peer.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SyncGoldsForSkirmish(peer.Peer, newAmount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this.GameModeBaseClient != null)
			{
				this.GameModeBaseClient.OnGoldAmountChangedForRepresentative(peer.Representative, newAmount);
			}
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x00097288 File Offset: 0x00095488
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			if (this.GameModeBaseClient.IsGameModeUsingGold)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (networkCommunicator != networkPeer)
					{
						MissionRepresentativeBase component = networkCommunicator.GetComponent<MissionRepresentativeBase>();
						if (component != null)
						{
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SyncGoldsForSkirmish(component.Peer, component.Gold));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
			}
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x00097310 File Offset: 0x00095510
		public virtual bool CheckIfPlayerCanDespawn(MissionPeer missionPeer)
		{
			return false;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x00097313 File Offset: 0x00095513
		public override void OnPreMissionTick(float dt)
		{
			this.CanGameModeSystemsTickThisFrame = false;
			this._gameModeSystemTickTimer += dt;
			if (this._gameModeSystemTickTimer >= 0.25f)
			{
				this._gameModeSystemTickTimer -= 0.25f;
				this.CanGameModeSystemsTickThisFrame = true;
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x00097350 File Offset: 0x00095550
		public Dictionary<string, string> GetUsedCosmeticsFromPeer(MissionPeer missionPeer, BasicCharacterObject selectedTroopCharacter)
		{
			if (missionPeer.Peer.UsedCosmetics != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>();
				int num = -1;
				for (int i = 0; i < objectTypeList.Count; i++)
				{
					if (objectTypeList[i].HeroCharacter == selectedTroopCharacter || objectTypeList[i].TroopCharacter == selectedTroopCharacter)
					{
						num = i;
						break;
					}
				}
				List<int> list;
				missionPeer.Peer.UsedCosmetics.TryGetValue(num, out list);
				if (list != null)
				{
					foreach (int num2 in list)
					{
						ClothingCosmeticElement clothingCosmeticElement;
						if ((clothingCosmeticElement = CosmeticsManager.CosmeticElementsList[num2] as ClothingCosmeticElement) != null)
						{
							foreach (string text in clothingCosmeticElement.ReplaceItemsId)
							{
								dictionary.Add(text, CosmeticsManager.CosmeticElementsList[num2].Id);
							}
							foreach (Tuple<string, string> tuple in clothingCosmeticElement.ReplaceItemless)
							{
								if (tuple.Item1 == objectTypeList[num].StringId)
								{
									dictionary.Add(tuple.Item2, CosmeticsManager.CosmeticElementsList[num2].Id);
									break;
								}
							}
						}
					}
				}
				return dictionary;
			}
			return null;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x00097504 File Offset: 0x00095704
		public void AddCosmeticItemsToEquipment(Equipment equipment, Dictionary<string, string> choosenCosmetics)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
			{
				if (equipment[equipmentIndex].Item == null)
				{
					string text = equipmentIndex.ToString();
					switch (equipmentIndex)
					{
					case EquipmentIndex.NumAllWeaponSlots:
						text = "Head";
						break;
					case EquipmentIndex.Body:
						text = "Body";
						break;
					case EquipmentIndex.Leg:
						text = "Leg";
						break;
					case EquipmentIndex.Gloves:
						text = "Gloves";
						break;
					case EquipmentIndex.Cape:
						text = "Cape";
						break;
					}
					string text2 = null;
					if (choosenCosmetics != null)
					{
						choosenCosmetics.TryGetValue(text, out text2);
					}
					if (text2 != null)
					{
						ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(text2);
						EquipmentElement equipmentElement = equipment[equipmentIndex];
						equipmentElement.CosmeticItem = @object;
						equipment[equipmentIndex] = equipmentElement;
					}
				}
				else
				{
					string stringId = equipment[equipmentIndex].Item.StringId;
					string text3 = null;
					if (choosenCosmetics != null)
					{
						choosenCosmetics.TryGetValue(stringId, out text3);
					}
					if (text3 != null)
					{
						ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>(text3);
						EquipmentElement equipmentElement2 = equipment[equipmentIndex];
						equipmentElement2.CosmeticItem = object2;
						equipment[equipmentIndex] = equipmentElement2;
					}
				}
			}
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x0009761C File Offset: 0x0009581C
		public bool IsClassAvailable(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			FormationClass formationClass;
			if (Enum.TryParse<FormationClass>(heroClass.ClassGroup.StringId, out formationClass))
			{
				return this.MissionLobbyComponent.IsClassAvailable(formationClass);
			}
			Debug.FailedAssert("\"" + heroClass.ClassGroup.StringId + "\" does not match with any FormationClass.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ServerGameModeLogics\\MissionMultiplayerGameModeBase.cs", "IsClassAvailable", 408);
			return false;
		}

		// Token: 0x04000F4A RID: 3914
		public const int GoldCap = 2000;

		// Token: 0x04000F4B RID: 3915
		public const float PerkTickPeriod = 1f;

		// Token: 0x04000F4C RID: 3916
		public const float GameModeSystemTickPeriod = 0.25f;

		// Token: 0x04000F4D RID: 3917
		private float _lastPerkTickTime;

		// Token: 0x04000F4F RID: 3919
		private MultiplayerMissionAgentVisualSpawnComponent _agentVisualSpawnComponent;

		// Token: 0x04000F50 RID: 3920
		public MultiplayerTeamSelectComponent MultiplayerTeamSelectComponent;

		// Token: 0x04000F51 RID: 3921
		protected MissionLobbyComponent MissionLobbyComponent;

		// Token: 0x04000F52 RID: 3922
		protected MultiplayerGameNotificationsComponent NotificationsComponent;

		// Token: 0x04000F53 RID: 3923
		public MultiplayerRoundController RoundController;

		// Token: 0x04000F54 RID: 3924
		public MultiplayerWarmupComponent WarmupComponent;

		// Token: 0x04000F55 RID: 3925
		public MultiplayerTimerComponent TimerComponent;

		// Token: 0x04000F56 RID: 3926
		protected MissionMultiplayerGameModeBaseClient GameModeBaseClient;

		// Token: 0x04000F58 RID: 3928
		private float _gameModeSystemTickTimer;
	}
}
