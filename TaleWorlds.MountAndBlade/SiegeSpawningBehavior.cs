using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D9 RID: 729
	public class SiegeSpawningBehavior : SpawningBehaviorBase
	{
		// Token: 0x06002A35 RID: 10805 RVA: 0x000A02A4 File Offset: 0x0009E4A4
		public override void Initialize(SpawnComponent spawnComponent)
		{
			base.Initialize(spawnComponent);
			base.OnAllAgentsFromPeerSpawnedFromVisuals += this.OnAllAgentsFromPeerSpawnedFromVisuals;
			if (this.GameMode.WarmupComponent == null)
			{
				this.RequestStartSpawnSession();
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000A02D2 File Offset: 0x0009E4D2
		public override void Clear()
		{
			base.Clear();
			base.OnAllAgentsFromPeerSpawnedFromVisuals -= this.OnAllAgentsFromPeerSpawnedFromVisuals;
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000A02EC File Offset: 0x0009E4EC
		public override void OnTick(float dt)
		{
			if (this.IsSpawningEnabled && this.SpawnCheckTimer.Check(base.Mission.CurrentTime))
			{
				this.SpawnAgents();
			}
			base.OnTick(dt);
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000A031C File Offset: 0x0009E51C
		protected override void SpawnAgents()
		{
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.IsSynchronized)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.ControlledAgent == null && !component.HasSpawnedAgentVisuals && component.Team != null && !networkCommunicator.IsSpectator && component.Team != base.Mission.SpectatorTeam && component.TeamInitialPerkInfoReady && component.SpawnTimer.Check(base.Mission.CurrentTime))
					{
						BattleSideEnum side = component.Team.Side;
						MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component, false);
						if (mpheroClassForPeer == null || mpheroClassForPeer.TroopCasualCost > this.GameMode.GetCurrentGoldForPeer(component))
						{
							if (component.SelectedTroopIndex != 0)
							{
								component.SelectedTroopIndex = 0;
								GameNetwork.BeginBroadcastModuleEvent();
								GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(networkCommunicator, 0));
								GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkCommunicator);
							}
						}
						else
						{
							BasicCharacterObject heroCharacter = mpheroClassForPeer.HeroCharacter;
							Equipment equipment = heroCharacter.Equipment.Clone(false);
							MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(component);
							IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
							if (enumerable != null)
							{
								foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
								{
									equipment[valueTuple.Item1] = valueTuple.Item2;
								}
							}
							MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = multiplayerBattleColors.GetPeerColors(component);
							AgentBuildData agentBuildData = new AgentBuildData(heroCharacter).MissionPeer(component).Equipment(equipment).Team(component.Team)
								.TroopOrigin(new BasicBattleAgentOrigin(heroCharacter))
								.IsFemale(component.Peer.IsFemale)
								.BodyProperties(base.GetBodyProperties(component, (component.Culture == @object) ? @object : object2))
								.VisualsIndex(0)
								.ClothingColor1(peerColors.ClothingColor1Uint)
								.ClothingColor2(peerColors.ClothingColor2Uint);
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
						}
					}
				}
			}
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000A05FC File Offset: 0x0009E7FC
		public override bool AllowEarlyAgentVisualsDespawning(MissionPeer lobbyPeer)
		{
			return true;
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000A0600 File Offset: 0x0009E800
		public override int GetMaximumReSpawnPeriodForPeer(MissionPeer peer)
		{
			if (this.GameMode.WarmupComponent != null && this.GameMode.WarmupComponent.IsInWarmup)
			{
				return 3;
			}
			if (peer.Team != null)
			{
				if (peer.Team.Side == BattleSideEnum.Attacker)
				{
					return MultiplayerOptions.OptionType.RespawnPeriodTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
				if (peer.Team.Side == BattleSideEnum.Defender)
				{
					return MultiplayerOptions.OptionType.RespawnPeriodTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
			return -1;
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000A0664 File Offset: 0x0009E864
		protected override bool IsRoundInProgress()
		{
			return Mission.Current.CurrentState == Mission.State.Continuing;
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000A0674 File Offset: 0x0009E874
		private new void OnAllAgentsFromPeerSpawnedFromVisuals(MissionPeer peer)
		{
			bool flag = peer.Team == base.Mission.AttackerTeam;
			Team defenderTeam = base.Mission.DefenderTeam;
			MultiplayerClassDivisions.MPHeroClass mpheroClass = MultiplayerClassDivisions.GetMPHeroClasses(MBObjectManager.Instance.GetObject<BasicCultureObject>(flag ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))).ElementAt<MultiplayerClassDivisions.MPHeroClass>(peer.SelectedTroopIndex);
			this.GameMode.ChangeCurrentGoldForPeer(peer, this.GameMode.GetCurrentGoldForPeer(peer) - mpheroClass.TroopCasualCost);
		}
	}
}
