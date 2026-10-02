using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C1 RID: 705
	public class MultiplayerMissionAgentVisualSpawnComponent : MissionNetwork
	{
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06002890 RID: 10384 RVA: 0x00099CE4 File Offset: 0x00097EE4
		// (remove) Token: 0x06002891 RID: 10385 RVA: 0x00099D1C File Offset: 0x00097F1C
		public event Action OnMyAgentVisualSpawned;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06002892 RID: 10386 RVA: 0x00099D54 File Offset: 0x00097F54
		// (remove) Token: 0x06002893 RID: 10387 RVA: 0x00099D8C File Offset: 0x00097F8C
		public event Action OnMyAgentSpawnedFromVisual;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06002894 RID: 10388 RVA: 0x00099DC4 File Offset: 0x00097FC4
		// (remove) Token: 0x06002895 RID: 10389 RVA: 0x00099DFC File Offset: 0x00097FFC
		public event Action OnMyAgentVisualRemoved;

		// Token: 0x06002896 RID: 10390 RVA: 0x00099E34 File Offset: 0x00098034
		public void SpawnAgentVisualsForPeer(MissionPeer missionPeer, AgentBuildData buildData, int selectedEquipmentSetIndex = -1, bool isBot = false, int totalTroopCount = 0)
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer != null)
			{
				myPeer.GetComponent<MissionPeer>();
			}
			if (buildData.AgentVisualsIndex == 0)
			{
				missionPeer.ClearAllVisuals(false);
			}
			missionPeer.ClearVisuals(buildData.AgentVisualsIndex);
			Equipment equipment = new Equipment(buildData.AgentOverridenSpawnEquipment);
			ItemObject item = equipment[10].Item;
			MatrixFrame spawnPointFrameForPlayer = this._spawnFrameSelectionHelper.GetSpawnPointFrameForPlayer(missionPeer.Peer, missionPeer.Team.Side, buildData.AgentVisualsIndex, totalTroopCount, item != null);
			ActionIndexCache actionIndexCache = ((item == null) ? ActionIndexCache.act_walk_idle_unarmed : ActionIndexCache.act_horse_stand_1);
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(buildData.AgentCharacter);
			MBReadOnlyList<MPPerkObject> selectedPerks = missionPeer.SelectedPerks;
			float num = 0.1f + MBRandom.RandomFloat * 0.8f;
			IAgentVisual agentVisual = null;
			if (item != null)
			{
				Monster monster = item.HorseComponent.Monster;
				AgentVisualsData agentVisualsData = new AgentVisualsData().Equipment(equipment).Scale(item.ScaleFactor).Frame(MatrixFrame.Identity)
					.ActionSet(MBGlobals.GetActionSet(monster.ActionSetCode))
					.Scene(Mission.Current.Scene)
					.Monster(monster)
					.PrepareImmediately(false)
					.MountCreationKey(MountCreationKey.GetRandomMountKeyString(item, MBRandom.RandomInt()));
				agentVisual = Mission.Current.AgentVisualCreator.Create(agentVisualsData, "Agent " + buildData.AgentCharacter.StringId + " mount", true, false);
				MatrixFrame matrixFrame = spawnPointFrameForPlayer;
				matrixFrame.rotation.ApplyScaleLocal(agentVisualsData.ScaleData);
				ActionIndexCache actionIndexCache2 = ActionIndexCache.act_none;
				foreach (MPPerkObject mpperkObject in selectedPerks)
				{
					if (!isBot && mpperkObject.HeroMountIdleAnimOverride != null)
					{
						actionIndexCache2 = ActionIndexCache.Create(mpperkObject.HeroMountIdleAnimOverride);
						break;
					}
					if (isBot && mpperkObject.TroopMountIdleAnimOverride != null)
					{
						actionIndexCache2 = ActionIndexCache.Create(mpperkObject.TroopMountIdleAnimOverride);
						break;
					}
				}
				if (actionIndexCache2 == ActionIndexCache.act_none)
				{
					if (item.StringId == "mp_aserai_camel")
					{
						Debug.Print("Client is spawning a camel for without mountCustomAction from the perk.", 0, Debug.DebugColor.White, 17179869184UL);
						actionIndexCache2 = ((!isBot) ? ActionIndexCache.act_hero_mount_idle_camel : ActionIndexCache.act_camel_idle_1);
					}
					else
					{
						if (!isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.HeroMountIdleAnim))
						{
							actionIndexCache2 = ActionIndexCache.Create(mpheroClassForCharacter.HeroMountIdleAnim);
						}
						if (isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.TroopMountIdleAnim))
						{
							actionIndexCache2 = ActionIndexCache.Create(mpheroClassForCharacter.TroopMountIdleAnim);
						}
					}
				}
				if (actionIndexCache2 != ActionIndexCache.act_none)
				{
					agentVisual.SetAction(in actionIndexCache2, 0f, true);
					agentVisual.GetVisuals().GetSkeleton().SetAnimationParameterAtChannel(0, num);
					agentVisual.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.1f, matrixFrame, true);
				}
				agentVisual.GetVisuals().GetEntity().SetFrame(ref matrixFrame, true);
			}
			ActionIndexCache actionIndexCache3 = actionIndexCache;
			if (agentVisual != null)
			{
				actionIndexCache3 = agentVisual.GetVisuals().GetSkeleton().GetActionAtChannel(0);
			}
			else
			{
				foreach (MPPerkObject mpperkObject2 in selectedPerks)
				{
					if (!isBot && mpperkObject2.HeroIdleAnimOverride != null)
					{
						actionIndexCache3 = ActionIndexCache.Create(mpperkObject2.HeroIdleAnimOverride);
						break;
					}
					if (isBot && mpperkObject2.TroopIdleAnimOverride != null)
					{
						actionIndexCache3 = ActionIndexCache.Create(mpperkObject2.TroopIdleAnimOverride);
						break;
					}
				}
				if (actionIndexCache3 == actionIndexCache)
				{
					if (!isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.HeroIdleAnim))
					{
						actionIndexCache3 = ActionIndexCache.Create(mpheroClassForCharacter.HeroIdleAnim);
					}
					if (isBot && !string.IsNullOrEmpty(mpheroClassForCharacter.TroopIdleAnim))
					{
						actionIndexCache3 = ActionIndexCache.Create(mpheroClassForCharacter.TroopIdleAnim);
					}
				}
			}
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(buildData.AgentCharacter.Race);
			IAgentVisual agentVisual2 = Mission.Current.AgentVisualCreator.Create(new AgentVisualsData().Equipment(equipment).BodyProperties(buildData.AgentBodyProperties).Frame(spawnPointFrameForPlayer)
				.ActionSet(MBActionSet.GetActionSet(baseMonsterFromRace.ActionSetCode))
				.Scene(Mission.Current.Scene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(false)
				.UseMorphAnims(true)
				.SkeletonType(buildData.AgentIsFemale ? SkeletonType.Female : SkeletonType.Male)
				.ClothColor1(buildData.AgentClothingColor1)
				.ClothColor2(buildData.AgentClothingColor2)
				.AddColorRandomness(buildData.AgentVisualsIndex != 0)
				.ActionCode(in actionIndexCache3), "Mission::SpawnAgentVisuals", true, false);
			agentVisual2.SetAction(in actionIndexCache3, 0f, true);
			agentVisual2.GetVisuals().GetSkeleton().SetAnimationParameterAtChannel(0, num);
			agentVisual2.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(0.1f, spawnPointFrameForPlayer, true);
			agentVisual2.GetVisuals().SetFrame(ref spawnPointFrameForPlayer);
			agentVisual2.SetCharacterObjectID(buildData.AgentCharacter.StringId);
			EquipmentIndex equipmentIndex;
			EquipmentIndex equipmentIndex2;
			bool flag;
			equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
			if (flag)
			{
				equipmentIndex2 = EquipmentIndex.None;
			}
			agentVisual2.GetVisuals().SetWieldedWeaponIndices((int)equipmentIndex, (int)equipmentIndex2);
			PeerVisualsHolder peerVisualsHolder = new PeerVisualsHolder(missionPeer, buildData.AgentVisualsIndex, agentVisual2, agentVisual);
			missionPeer.OnVisualsSpawned(peerVisualsHolder, peerVisualsHolder.VisualsIndex);
			if (missionPeer.IsMine && buildData.AgentVisualsIndex == 0)
			{
				Action onMyAgentVisualSpawned = this.OnMyAgentVisualSpawned;
				if (onMyAgentVisualSpawned == null)
				{
					return;
				}
				onMyAgentVisualSpawned();
			}
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x0009A360 File Offset: 0x00098560
		public void RemoveAgentVisuals(MissionPeer missionPeer, bool sync = false)
		{
			missionPeer.ClearAllVisuals(false);
			if (!GameNetwork.IsDedicatedServer && !missionPeer.Peer.IsMine)
			{
				this._spawnFrameSelectionHelper.FreeSpawnPointFromPlayer(missionPeer.Peer);
			}
			if (this.OnMyAgentVisualRemoved != null && missionPeer.IsMine)
			{
				this.OnMyAgentVisualRemoved();
			}
			Debug.Print("Removed visuals for " + missionPeer.Name + ".", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x0009A3DA File Offset: 0x000985DA
		public void OnMyAgentSpawned()
		{
			Action onMyAgentSpawnedFromVisual = this.OnMyAgentSpawnedFromVisual;
			if (onMyAgentSpawnedFromVisual == null)
			{
				return;
			}
			onMyAgentSpawnedFromVisual();
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x0009A3EC File Offset: 0x000985EC
		public override void OnPreMissionTick(float dt)
		{
			if (!GameNetwork.IsDedicatedServer && this._spawnFrameSelectionHelper == null && Mission.Current != null && GameNetwork.MyPeer != null)
			{
				this._spawnFrameSelectionHelper = new MultiplayerMissionAgentVisualSpawnComponent.VisualSpawnFrameSelectionHelper();
			}
		}

		// Token: 0x04000F81 RID: 3969
		private MultiplayerMissionAgentVisualSpawnComponent.VisualSpawnFrameSelectionHelper _spawnFrameSelectionHelper;

		// Token: 0x020005AD RID: 1453
		private class VisualSpawnFrameSelectionHelper
		{
			// Token: 0x06003EB5 RID: 16053 RVA: 0x000F87F0 File Offset: 0x000F69F0
			public VisualSpawnFrameSelectionHelper()
			{
				this._visualSpawnPoints = new GameEntity[6];
				this._visualAttackerSpawnPoints = new GameEntity[6];
				this._visualDefenderSpawnPoints = new GameEntity[6];
				this._visualSpawnPointUsers = new VirtualPlayer[6];
				for (int i = 0; i < 6; i++)
				{
					GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("sp_visual_" + i);
					if (gameEntity != null)
					{
						this._visualSpawnPoints[i] = gameEntity;
					}
					gameEntity = Mission.Current.Scene.FindEntityWithTag("sp_visual_attacker_" + i);
					if (gameEntity != null)
					{
						this._visualAttackerSpawnPoints[i] = gameEntity;
					}
					gameEntity = Mission.Current.Scene.FindEntityWithTag("sp_visual_defender_" + i);
					if (gameEntity != null)
					{
						this._visualDefenderSpawnPoints[i] = gameEntity;
					}
				}
				this._visualSpawnPointUsers[0] = GameNetwork.MyPeer.VirtualPlayer;
			}

			// Token: 0x06003EB6 RID: 16054 RVA: 0x000F88F0 File Offset: 0x000F6AF0
			public MatrixFrame GetSpawnPointFrameForPlayer(VirtualPlayer player, BattleSideEnum side, int agentVisualIndex, int totalTroopCount, bool isMounted = false)
			{
				if (agentVisualIndex == 0)
				{
					int num = -1;
					int num2 = -1;
					for (int i = 0; i < this._visualSpawnPointUsers.Length; i++)
					{
						if (this._visualSpawnPointUsers[i] == player)
						{
							num = i;
							break;
						}
						if (num2 < 0 && this._visualSpawnPointUsers[i] == null)
						{
							num2 = i;
						}
					}
					int num3 = ((num >= 0) ? num : num2);
					if (num3 >= 0)
					{
						this._visualSpawnPointUsers[num3] = player;
						GameEntity gameEntity = null;
						if (side == BattleSideEnum.Attacker)
						{
							gameEntity = this._visualAttackerSpawnPoints[num3];
						}
						else if (side == BattleSideEnum.Defender)
						{
							gameEntity = this._visualDefenderSpawnPoints[num3];
						}
						MatrixFrame matrixFrame = ((gameEntity != null) ? gameEntity.GetGlobalFrame() : this._visualSpawnPoints[num3].GetGlobalFrame());
						matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
						return matrixFrame;
					}
					Debug.FailedAssert("Couldn't find a valid spawn point for player.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerMissionAgentVisualSpawnComponent.cs", "GetSpawnPointFrameForPlayer", 139);
					return MatrixFrame.Identity;
				}
				else
				{
					Vec3 origin = this._visualSpawnPoints[3].GetGlobalFrame().origin;
					Vec3 origin2 = this._visualSpawnPoints[1].GetGlobalFrame().origin;
					Vec3 origin3 = this._visualSpawnPoints[5].GetGlobalFrame().origin;
					Mat3 rotation = this._visualSpawnPoints[0].GetGlobalFrame().rotation;
					rotation.MakeUnit();
					List<WorldFrame> formationFramesForBeforeFormationCreation = Formation.GetFormationFramesForBeforeFormationCreation(origin2.Distance(origin3), totalTroopCount, isMounted, new WorldPosition(Mission.Current.Scene, origin), rotation);
					if (formationFramesForBeforeFormationCreation.Count < agentVisualIndex)
					{
						return new MatrixFrame(in rotation, in origin);
					}
					return formationFramesForBeforeFormationCreation[agentVisualIndex - 1].ToGroundMatrixFrame();
				}
			}

			// Token: 0x06003EB7 RID: 16055 RVA: 0x000F8A64 File Offset: 0x000F6C64
			public void FreeSpawnPointFromPlayer(VirtualPlayer player)
			{
				for (int i = 0; i < this._visualSpawnPointUsers.Length; i++)
				{
					if (this._visualSpawnPointUsers[i] == player)
					{
						this._visualSpawnPointUsers[i] = null;
						return;
					}
				}
			}

			// Token: 0x04001F39 RID: 7993
			private const string SpawnPointTagPrefix = "sp_visual_";

			// Token: 0x04001F3A RID: 7994
			private const string AttackerSpawnPointTagPrefix = "sp_visual_attacker_";

			// Token: 0x04001F3B RID: 7995
			private const string DefenderSpawnPointTagPrefix = "sp_visual_defender_";

			// Token: 0x04001F3C RID: 7996
			private const int NumberOfSpawnPoints = 6;

			// Token: 0x04001F3D RID: 7997
			private const int PlayerSpawnPointIndex = 0;

			// Token: 0x04001F3E RID: 7998
			private GameEntity[] _visualSpawnPoints;

			// Token: 0x04001F3F RID: 7999
			private GameEntity[] _visualAttackerSpawnPoints;

			// Token: 0x04001F40 RID: 8000
			private GameEntity[] _visualDefenderSpawnPoints;

			// Token: 0x04001F41 RID: 8001
			private VirtualPlayer[] _visualSpawnPointUsers;
		}
	}
}
