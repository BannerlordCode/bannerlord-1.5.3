using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.Source.Objects;
using TaleWorlds.ObjectSystem;

namespace SandBox
{
	// Token: 0x02000024 RID: 36
	public static class SandBoxHelpers
	{
		// Token: 0x0200013C RID: 316
		public static class MissionHelper
		{
			// Token: 0x06000E18 RID: 3608 RVA: 0x00064F28 File Offset: 0x00063128
			public static void FollowAgent(Agent agent, Agent target)
			{
				if (agent != null && target != null && agent.IsActive() && target.IsActive())
				{
					AgentBehaviorGroup activeBehaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetActiveBehaviorGroup();
					if (activeBehaviorGroup != null)
					{
						FollowAgentBehavior followAgentBehavior = activeBehaviorGroup.GetBehavior<FollowAgentBehavior>();
						if (followAgentBehavior == null)
						{
							followAgentBehavior = activeBehaviorGroup.AddBehavior<FollowAgentBehavior>();
						}
						activeBehaviorGroup.SetScriptedBehavior<FollowAgentBehavior>();
						followAgentBehavior.SetTargetAgent(target);
						return;
					}
				}
				else
				{
					Debug.FailedAssert("Cant follow agent", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\SandboxHelpers.cs", "FollowAgent", 47);
				}
			}

			// Token: 0x06000E19 RID: 3609 RVA: 0x00064F94 File Offset: 0x00063194
			public static void UnfollowAgent(Agent agent)
			{
				if (agent != null && agent.IsActive())
				{
					AgentBehaviorGroup activeBehaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetActiveBehaviorGroup();
					if (activeBehaviorGroup != null && activeBehaviorGroup.GetBehavior<FollowAgentBehavior>() != null)
					{
						activeBehaviorGroup.RemoveBehavior<FollowAgentBehavior>();
						return;
					}
				}
				else
				{
					Debug.FailedAssert("Cant unfollow agent", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\SandboxHelpers.cs", "UnfollowAgent", 68);
				}
			}

			// Token: 0x06000E1A RID: 3610 RVA: 0x00064FE8 File Offset: 0x000631E8
			public static void FadeOutAgents(IEnumerable<Agent> agents, bool hideInstantly, bool hideMount)
			{
				if (agents != null)
				{
					Agent[] array = agents.ToArray<Agent>();
					foreach (Agent agent in array)
					{
						if (!agent.IsMount)
						{
							agent.FadeOut(hideInstantly, hideMount);
						}
					}
					foreach (Agent agent2 in array)
					{
						if (agent2.State != AgentState.Routed)
						{
							agent2.FadeOut(hideInstantly, hideMount);
						}
					}
				}
			}

			// Token: 0x06000E1B RID: 3611 RVA: 0x0006504C File Offset: 0x0006324C
			public static void DisableGenericMissionEventScript(string triggeringObjectTag, GenericMissionEvent missionEvent)
			{
				using (IEnumerator<ScriptComponentBehavior> enumerator = Mission.Current.Scene.FindEntityWithTag(triggeringObjectTag).GetScriptComponents().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GenericMissionEventScript genericMissionEventScript;
						if ((genericMissionEventScript = enumerator.Current as GenericMissionEventScript) != null && genericMissionEventScript.EventId.Equals(missionEvent.EventId) && genericMissionEventScript.Parameter.Equals(missionEvent.Parameter))
						{
							genericMissionEventScript.IsDisabled = true;
						}
					}
				}
			}

			// Token: 0x06000E1C RID: 3612 RVA: 0x000650D8 File Offset: 0x000632D8
			public static void SpawnPlayer(bool civilianEquipment = false, bool noHorses = false, bool noWeapon = false, bool wieldInitialWeapons = false, string spawnTag = "")
			{
				GameEntity gameEntity;
				if (!string.IsNullOrEmpty(spawnTag))
				{
					gameEntity = Mission.Current.Scene.FindEntityWithTag(spawnTag);
				}
				else
				{
					gameEntity = Mission.Current.Scene.FindEntityWithTag("spawnpoint_player");
				}
				SandBoxHelpers.MissionHelper.SpawnPlayer(gameEntity, civilianEquipment, noHorses, noWeapon, wieldInitialWeapons);
			}

			// Token: 0x06000E1D RID: 3613 RVA: 0x00065124 File Offset: 0x00063324
			public static void SpawnPlayer(GameEntity spawnPosition, bool civilianEquipment = false, bool noHorses = false, bool noWeapon = false, bool wieldInitialWeapons = false)
			{
				if (Campaign.Current.GameMode != CampaignGameMode.Campaign)
				{
					civilianEquipment = false;
				}
				MatrixFrame matrixFrame = MatrixFrame.Identity;
				if (spawnPosition != null)
				{
					matrixFrame = spawnPosition.GetGlobalFrame();
					matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				}
				CampaignEventDispatcher.Instance.OnBeforePlayerAgentSpawn(ref matrixFrame);
				CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
				AgentBuildData agentBuildData = new AgentBuildData(playerCharacter).Team(Mission.Current.PlayerTeam).InitialPosition(in matrixFrame.origin);
				Vec2 vec = matrixFrame.rotation.f.AsVec2;
				vec = vec.Normalized();
				AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).CivilianEquipment(civilianEquipment).NoHorses(noHorses)
					.NoWeapons(noWeapon)
					.ClothingColor1(Mission.Current.PlayerTeam.Color)
					.ClothingColor2(Mission.Current.PlayerTeam.Color2)
					.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, playerCharacter, -1, default(UniqueTroopDescriptor), false, false))
					.MountKey(MountCreationKey.GetRandomMountKeyString(playerCharacter.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, playerCharacter.GetMountKeySeed()))
					.Controller(AgentControllerType.Player);
				Debug.Print(string.Format("Spawn position: {0}", matrixFrame.origin), 0, Debug.DebugColor.White, 17592186044416UL);
				Hero heroObject = playerCharacter.HeroObject;
				if (((heroObject != null) ? heroObject.ClanBanner : null) != null)
				{
					agentBuildData2.Banner(playerCharacter.HeroObject.ClanBanner);
				}
				if (Campaign.Current.GameMode != CampaignGameMode.Campaign)
				{
					agentBuildData2.TroopOrigin(new SimpleAgentOrigin(CharacterObject.PlayerCharacter, -1, null, default(UniqueTroopDescriptor)));
				}
				if (Campaign.Current.IsMainHeroDisguised)
				{
					MBEquipmentRoster @object = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("npc_disguised_hero_equipment_template");
					agentBuildData2.Equipment(@object.DefaultEquipment);
				}
				Agent agent = Mission.Current.SpawnAgent(agentBuildData2, false, null, null);
				if (wieldInitialWeapons)
				{
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				}
				CampaignEventDispatcher.Instance.OnPlayerAgentSpawned();
				if (spawnPosition != null)
				{
					foreach (string text in spawnPosition.Tags)
					{
						agent.AgentVisuals.GetEntity().AddTag(text);
					}
				}
				for (int j = 0; j < 3; j++)
				{
					Agent.Main.AgentVisuals.GetSkeleton().TickAnimations(0.1f, Agent.Main.AgentVisuals.GetGlobalFrame(), true);
				}
			}

			// Token: 0x06000E1E RID: 3614 RVA: 0x0006537C File Offset: 0x0006357C
			public static List<Agent> SpawnHorses()
			{
				List<Agent> list = new List<Agent>();
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_horse"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					string text = gameEntity.Tags[1];
					ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(text);
					ItemRosterElement itemRosterElement = new ItemRosterElement(@object, 1, null);
					ItemRosterElement itemRosterElement2 = default(ItemRosterElement);
					if (@object.HasHorseComponent)
					{
						globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
						Mission mission = Mission.Current;
						ItemRosterElement itemRosterElement3 = itemRosterElement;
						ItemRosterElement itemRosterElement4 = itemRosterElement2;
						Vec2 asVec = globalFrame.rotation.f.AsVec2;
						Agent agent = mission.SpawnMonster(itemRosterElement3, itemRosterElement4, in globalFrame.origin, in asVec, -1);
						AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
						SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
						list.Add(agent);
					}
				}
				return list;
			}

			// Token: 0x06000E1F RID: 3615 RVA: 0x00065468 File Offset: 0x00063668
			public static void SpawnSheeps()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_sheep"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("sheep"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000E20 RID: 3616 RVA: 0x00065578 File Offset: 0x00063778
			public static void SpawnCows()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_cow"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("cow"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000E21 RID: 3617 RVA: 0x00065688 File Offset: 0x00063888
			public static void SpawnGeese()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_goose"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("goose"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000E22 RID: 3618 RVA: 0x00065798 File Offset: 0x00063998
			public static void SpawnChicken()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_chicken"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("chicken"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000E23 RID: 3619 RVA: 0x000658A8 File Offset: 0x00063AA8
			public static void SpawnHogs()
			{
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sp_hog"))
				{
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("hog"), 0, null);
					globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement2 = itemRosterElement;
					ItemRosterElement itemRosterElement3 = default(ItemRosterElement);
					Vec2 asVec = globalFrame.rotation.f.AsVec2;
					Agent agent = mission.SpawnMonster(itemRosterElement2, itemRosterElement3, in globalFrame.origin, in asVec, -1);
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("navigation_mesh_deactivator");
					if (gameEntity2 != null)
					{
						NavigationMeshDeactivator firstScriptOfType = gameEntity2.GetFirstScriptOfType<NavigationMeshDeactivator>();
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithId, true);
						agent.SetAgentExcludeStateForFaceGroupId(firstScriptOfType.DisableFaceWithIdForAnimals, true);
					}
					AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(gameEntity, agent);
					SandBoxHelpers.MissionHelper.SimulateAnimalAnimations(agent);
				}
			}

			// Token: 0x06000E24 RID: 3620 RVA: 0x000659B8 File Offset: 0x00063BB8
			public static void RemapSpecialTagIfNecessary(LocationCharacter locationCharacter, MissionAgentHandler missionAgentHandler)
			{
				if (locationCharacter.SpecialTargetTag == "sp_throne" && !missionAgentHandler.HasUsablePointWithTag("sp_throne") && missionAgentHandler.HasUsablePointWithTag("sp_king"))
				{
					locationCharacter.SpecialTargetTag = "sp_king";
				}
			}

			// Token: 0x06000E25 RID: 3621 RVA: 0x000659F4 File Offset: 0x00063BF4
			private static void SimulateAnimalAnimations(Agent agent)
			{
				int num = 10 + MBRandom.RandomInt(90);
				for (int i = 0; i < num; i++)
				{
					agent.TickActionChannels(0.1f);
					Vec3 vec = agent.ComputeAnimationDisplacement(0.1f);
					if (vec.LengthSquared > 0f)
					{
						agent.TeleportToPosition(agent.Position + vec);
					}
					agent.AgentVisuals.GetSkeleton().TickAnimations(0.1f, agent.AgentVisuals.GetGlobalFrame(), true);
				}
			}
		}

		// Token: 0x0200013D RID: 317
		public static class MapSceneHelper
		{
			// Token: 0x06000E26 RID: 3622 RVA: 0x00065A70 File Offset: 0x00063C70
			public static bool[] GetRegionMapping(PartyNavigationModel model)
			{
				TerrainType[] array = (TerrainType[])Enum.GetValues(typeof(TerrainType));
				bool[] array2 = new bool[array.Max<TerrainType>((TerrainType v) => (int)v) + 1];
				foreach (TerrainType terrainType in array)
				{
					array2[(int)terrainType] = model.IsTerrainTypeValidForNavigationType(terrainType, MobileParty.NavigationType.Default);
				}
				return array2;
			}
		}
	}
}
