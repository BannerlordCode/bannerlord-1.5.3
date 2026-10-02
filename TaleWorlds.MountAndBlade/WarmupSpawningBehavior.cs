using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DB RID: 731
	public class WarmupSpawningBehavior : SpawningBehaviorBase
	{
		// Token: 0x06002A5D RID: 10845 RVA: 0x000A1AB4 File Offset: 0x0009FCB4
		public WarmupSpawningBehavior()
		{
			this.IsSpawningEnabled = true;
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000A1AC3 File Offset: 0x0009FCC3
		public override void OnTick(float dt)
		{
			if (this.IsSpawningEnabled && this.SpawnCheckTimer.Check(base.Mission.CurrentTime))
			{
				this.SpawnAgents();
			}
			base.OnTick(dt);
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000A1AF4 File Offset: 0x0009FCF4
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
						IAgentVisual agentVisualForPeer = component.GetAgentVisualForPeer(0);
						BasicCultureObject basicCultureObject = ((component.Culture == @object) ? @object : object2);
						int num = component.SelectedTroopIndex;
						IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses(basicCultureObject);
						MultiplayerClassDivisions.MPHeroClass mpheroClass = ((num < 0) ? null : mpheroClasses.ElementAt<MultiplayerClassDivisions.MPHeroClass>(num));
						if (mpheroClass == null && num < 0)
						{
							mpheroClass = mpheroClasses.First<MultiplayerClassDivisions.MPHeroClass>();
							num = 0;
						}
						BasicCharacterObject heroCharacter = mpheroClass.HeroCharacter;
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
						MatrixFrame matrixFrame;
						if (agentVisualForPeer == null)
						{
							matrixFrame = this.SpawnComponent.GetSpawnFrame(component.Team, heroCharacter.Equipment.Horse.Item != null, false);
						}
						else
						{
							matrixFrame = agentVisualForPeer.GetFrame();
							matrixFrame.rotation.MakeUnit();
						}
						MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = multiplayerBattleColors.GetPeerColors(component);
						AgentBuildData agentBuildData = new AgentBuildData(heroCharacter).MissionPeer(component).Equipment(equipment).Team(component.Team)
							.TroopOrigin(new BasicBattleAgentOrigin(heroCharacter))
							.InitialPosition(in matrixFrame.origin);
						Vec2 vec = matrixFrame.rotation.f.AsVec2;
						vec = vec.Normalized();
						AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).IsFemale(component.Peer.IsFemale).BodyProperties(base.GetBodyProperties(component, basicCultureObject))
							.VisualsIndex(0)
							.ClothingColor1(peerColors.ClothingColor1Uint)
							.ClothingColor2(peerColors.ClothingColor2Uint);
						if (this.GameMode.ShouldSpawnVisualsForServer(networkCommunicator))
						{
							base.AgentVisualSpawnComponent.SpawnAgentVisualsForPeer(component, agentBuildData2, num, false, 0);
							if (agentBuildData2.AgentVisualsIndex == 0)
							{
								component.HasSpawnedAgentVisuals = true;
								component.EquipmentUpdatingExpired = false;
							}
						}
						this.GameMode.HandleAgentVisualSpawning(networkCommunicator, agentBuildData2, 0, true);
					}
				}
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000A1E28 File Offset: 0x000A0028
		public override bool AllowEarlyAgentVisualsDespawning(MissionPeer lobbyPeer)
		{
			return true;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000A1E2B File Offset: 0x000A002B
		public override int GetMaximumReSpawnPeriodForPeer(MissionPeer peer)
		{
			return 3;
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000A1E2E File Offset: 0x000A002E
		protected override bool IsRoundInProgress()
		{
			return Mission.Current.CurrentState == Mission.State.Continuing;
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000A1E3D File Offset: 0x000A003D
		public override void Clear()
		{
			base.Clear();
			base.RequestStopSpawnSession();
		}
	}
}
