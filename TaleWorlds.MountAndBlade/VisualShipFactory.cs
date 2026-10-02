using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200039A RID: 922
	public class VisualShipFactory
	{
		// Token: 0x06003544 RID: 13636 RVA: 0x000DC520 File Offset: 0x000DA720
		public static void InitializeShipEntityCache(Scene scene)
		{
			scene.SetDoNotAddEntitiesToTickList(true);
			VisualShipFactory._shipEntityCache.Clear();
			foreach (MissionShipObject missionShipObject in MBObjectManager.Instance.GetObjectTypeList<MissionShipObject>())
			{
				if (!VisualShipFactory._shipEntityCache.ContainsKey(missionShipObject.Prefab))
				{
					GameEntity gameEntity = VisualShipFactory.CreateShipEntityForCache(scene, missionShipObject.Prefab);
					VisualShipFactory._shipEntityCache.Add(missionShipObject.Prefab, gameEntity);
				}
			}
			scene.SetDoNotAddEntitiesToTickList(false);
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000DC5B8 File Offset: 0x000DA7B8
		public static void DeregisterVisualShipCache()
		{
			VisualShipFactory._shipEntityCache.Clear();
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000DC5C4 File Offset: 0x000DA7C4
		public static GameEntity CreateVisualShip(string shipPrefab, Scene scene, List<ShipVisualSlotInfo> upgrades, int shipSeed, float hitPointRatio, uint sailColor1 = 4294967295U, uint sailColor2 = 4294967295U, bool createPhysics = false, float floatingForceMultiplier = 1f, bool keepFireEntities = true)
		{
			Debug.Print("VisualShipFactory.CreateVisualShip: " + shipPrefab, 0, Debug.DebugColor.White, 17592186044416UL);
			GameEntity gameEntity = GameEntity.InstantiateWithRestOffset(scene, shipPrefab, createPhysics, MatrixFrame.Identity, -0.1f, false, "ship_visual_only");
			if (!keepFireEntities)
			{
				List<GameEntity> list = new List<GameEntity>();
				gameEntity.GetChildrenRecursive(ref list);
				list.RemoveAll((GameEntity e) => !e.HasTag("sail_mesh_free_entity"));
				foreach (GameEntity gameEntity2 in list)
				{
					gameEntity2.Parent.RemoveChild(gameEntity2, false, false, false, 32);
				}
			}
			if (!createPhysics)
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in gameEntity.GetScriptComponents().ToList<ScriptComponentBehavior>())
				{
					if (scriptComponentBehavior.ScriptComponent.GetName() == "NavalPhysics")
					{
						gameEntity.RemoveScriptComponent(scriptComponentBehavior.ScriptComponent.Pointer, 32);
					}
				}
			}
			if (gameEntity != null)
			{
				VisualShipFactory.RefreshUpgrades(gameEntity.WeakEntity, upgrades);
				gameEntity.CreateAndAddScriptComponent(typeof(ShipVisual).Name, false);
				gameEntity.GetFirstScriptOfType<ShipVisual>().Initialize(shipSeed, "", new float?(hitPointRatio), new ValueTuple<uint, uint>?(new ValueTuple<uint, uint>(sailColor1, sailColor2)), new float?(floatingForceMultiplier));
				gameEntity.CallScriptCallbacks(true);
			}
			foreach (Mesh mesh in gameEntity.WeakEntity.GetAllMeshesWithTag("faction_color"))
			{
				mesh.Color = sailColor1;
				mesh.Color2 = sailColor2;
			}
			return gameEntity;
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000DC7B4 File Offset: 0x000DA9B4
		public static GameEntity CreateVisualShipForCampaign(string shipPrefab, Scene scene, List<ShipVisualSlotInfo> upgrades, int shipSeed, string shipCustomSailPatternId, uint sailColor1 = 4294967295U, uint sailColor2 = 4294967295U)
		{
			Debug.Print("VisualShipFactory.CreateVisualShip: " + shipPrefab, 0, Debug.DebugColor.White, 17592186044416UL);
			GameEntity gameEntity;
			if (!VisualShipFactory._shipEntityCache.TryGetValue(shipPrefab, out gameEntity))
			{
				gameEntity = VisualShipFactory.CreateShipEntityForCache(scene, shipPrefab);
				VisualShipFactory._shipEntityCache.Add(shipPrefab, gameEntity);
			}
			GameEntity gameEntity2 = GameEntity.CopyFrom(scene, gameEntity, false, false);
			if (gameEntity2 != null)
			{
				VisualShipFactory.RefreshUpgrades(gameEntity2.WeakEntity, upgrades);
			}
			gameEntity2.GetFirstScriptOfType<ShipVisual>().Initialize(shipSeed, shipCustomSailPatternId, null, new ValueTuple<uint, uint>?(new ValueTuple<uint, uint>(sailColor1, sailColor2)), null);
			foreach (Mesh mesh in gameEntity2.WeakEntity.GetAllMeshesWithTag("faction_color"))
			{
				mesh.Color = sailColor1;
				mesh.Color2 = sailColor2;
			}
			gameEntity2.CallScriptCallbacks(true);
			return gameEntity2;
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000DC8B0 File Offset: 0x000DAAB0
		public static void RefreshUpgrades(WeakGameEntity shipEntity, List<ShipVisualSlotInfo> upgrades)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			using (IEnumerator<WeakGameEntity> enumerator = shipEntity.CollectChildrenEntitiesWithTagAsEnumarable("upgrade_slot").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeakGameEntity slot = enumerator.Current;
					bool flag = false;
					bool flag2 = false;
					int num = upgrades.FindIndex((ShipVisualSlotInfo u) => slot.HasTag(u.VisualSlotTag));
					IEnumerable<WeakGameEntity> children = slot.GetChildren();
					if (num != -1)
					{
						foreach (WeakGameEntity weakGameEntity in children)
						{
							if (!weakGameEntity.HasTag(upgrades[num].VisualPieceId))
							{
								if (!weakGameEntity.HasTag("base"))
								{
									if (weakGameEntity.HasTag("platform"))
									{
										list.Add(weakGameEntity);
									}
									else
									{
										weakGameEntity.SetVisibilityExcludeParents(false);
									}
								}
							}
							else
							{
								weakGameEntity.SetVisibilityExcludeParents(true);
								flag = true;
								slot.SetVisibilityExcludeParents(true);
							}
						}
					}
					foreach (WeakGameEntity weakGameEntity2 in children)
					{
						if (weakGameEntity2.HasTag("base"))
						{
							weakGameEntity2.SetVisibilityExcludeParents(!flag);
							flag2 = true;
						}
					}
					if (!flag)
					{
						foreach (WeakGameEntity weakGameEntity3 in list)
						{
							weakGameEntity3.SetVisibilityExcludeParents(false);
						}
						if (flag2)
						{
							for (int i = slot.ChildCount - 1; i >= 0; i--)
							{
								WeakGameEntity child = slot.GetChild(i);
								if (!child.HasTag("base"))
								{
									child.SetVisibilityExcludeParents(false);
								}
							}
						}
						else
						{
							slot.SetVisibilityExcludeParents(false);
						}
					}
					else
					{
						foreach (WeakGameEntity weakGameEntity4 in list)
						{
							weakGameEntity4.SetVisibilityExcludeParents(true);
						}
					}
					list.Clear();
				}
			}
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000DCB44 File Offset: 0x000DAD44
		private static GameEntity CreateShipEntityForCache(Scene scene, string missionShipObjectPrefab)
		{
			GameEntity gameEntity = GameEntity.Instantiate(scene, missionShipObjectPrefab, false, false, "ship_visual_only");
			List<GameEntity> list = new List<GameEntity>();
			foreach (GameEntity gameEntity2 in gameEntity.GetChildren())
			{
				if (gameEntity2.Name.Equals("attachment_machine_holder") || gameEntity2.Name.Equals("spawn_point_holder") || gameEntity2.Name.Equals("barrier_holder") || gameEntity2.Name.Equals("control_point_holder") || gameEntity2.Name.Equals("knobs_holder") || gameEntity2.Name.Equals("floater_volume_holder") || gameEntity2.Name.Equals("torch_holder") || gameEntity2.HasTag("wetness_decals") || gameEntity2.Name.Equals("deck_upgrade_holder"))
				{
					list.Add(gameEntity2);
				}
				else if (gameEntity2.Name.Equals("oars_holder"))
				{
					foreach (GameEntity gameEntity3 in gameEntity2.CollectChildrenEntitiesWithTag("Pilot"))
					{
						list.Add(gameEntity3);
					}
				}
			}
			foreach (GameEntity gameEntity4 in list)
			{
				if (gameEntity4.Parent != null)
				{
					gameEntity4.Parent.RemoveChild(gameEntity4, false, false, false, 32);
				}
			}
			list.Clear();
			List<GameEntity> list2 = new List<GameEntity>();
			gameEntity.GetChildrenRecursive(ref list2);
			foreach (GameEntity gameEntity5 in list2)
			{
				if (gameEntity5.Name.Equals("state1") || gameEntity5.Name.Equals("state2") || gameEntity5.Name.Equals("state3") || gameEntity5.Name.Equals("destroyed") || gameEntity5.Name.Contains("knot_point") || gameEntity5.HasTag("sail_mesh_free_entity"))
				{
					list.Add(gameEntity5);
				}
			}
			foreach (GameEntity gameEntity6 in list)
			{
				gameEntity6.Parent.RemoveChild(gameEntity6, false, false, false, 32);
			}
			List<GameEntity> list3 = new List<GameEntity>();
			gameEntity.GetChildrenRecursive(ref list3);
			foreach (GameEntity gameEntity7 in list3)
			{
				gameEntity7.RemoveAllParticleSystems();
			}
			gameEntity.SetVisibilityExcludeParents(false);
			gameEntity.CreateAndAddScriptComponent(typeof(ShipVisual).Name, false);
			foreach (ScriptComponentBehavior scriptComponentBehavior in gameEntity.GetScriptComponents().ToList<ScriptComponentBehavior>())
			{
				if (scriptComponentBehavior.ScriptComponent.GetName() == "NavalPhysics")
				{
					gameEntity.RemoveScriptComponent(scriptComponentBehavior.ScriptComponent.Pointer, 32);
				}
			}
			return gameEntity;
		}

		// Token: 0x0400168B RID: 5771
		private static readonly Dictionary<string, GameEntity> _shipEntityCache = new Dictionary<string, GameEntity>();
	}
}
