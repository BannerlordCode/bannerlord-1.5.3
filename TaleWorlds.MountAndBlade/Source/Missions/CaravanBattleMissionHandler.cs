using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003DA RID: 986
	public class CaravanBattleMissionHandler : MissionLogic
	{
		// Token: 0x06003730 RID: 14128 RVA: 0x000E4B7C File Offset: 0x000E2D7C
		public CaravanBattleMissionHandler(IBattleCombatant caravanCombatant, bool isCamelCulture, bool isCaravan, BattleSideEnum playerSide)
		{
			this._isCaravan = isCaravan;
			this._isCamelCulture = isCamelCulture;
			this._caravanCombatant = caravanCombatant;
			this._playerSide = playerSide;
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x000E4C24 File Offset: 0x000E2E24
		public override void OnBattleSideSpawned(BattleSideEnum side)
		{
			if (side == this._caravanCombatant.Side)
			{
				IMissionDeploymentPlan deploymentPlan = base.Mission.DeploymentPlan;
				Vec2 vec;
				MatrixFrame matrixFrame;
				if (side != this._playerSide)
				{
					this._caravanTeam = Mission.GetTeam(TeamSideEnum.EnemyTeam);
					matrixFrame = deploymentPlan.GetFormationsCenterFrameAndExtents(this._caravanTeam, out vec, false);
				}
				else
				{
					IBattleCombatant battleCombatant;
					base.Mission.GetMissionBehavior<MissionCombatantsLogic>().SupportsAllyTeamOnPlayerSide(out battleCombatant);
					this._caravanTeam = Mission.GetTeam((battleCombatant != null && battleCombatant == this._caravanCombatant) ? TeamSideEnum.PlayerAllyTeam : TeamSideEnum.PlayerTeam);
					matrixFrame = deploymentPlan.GetFormationsCenterFrameAndExtents(this._caravanTeam, out vec, false);
				}
				MatrixFrame matrixFrame2 = matrixFrame;
				matrixFrame2.Advance(-(vec.y + 2f));
				matrixFrame2.origin.z = base.Mission.Scene.GetTerrainHeight(matrixFrame2.origin.AsVec2, true);
				this._entity = GameEntity.Instantiate(Mission.Current.Scene, this._isCaravan ? "caravan_scattered_goods_prop" : "villager_scattered_goods_prop", matrixFrame2, true);
				this._entity.SetMobility(GameEntity.Mobility.Dynamic);
				foreach (GameEntity gameEntity in this._entity.GetChildren())
				{
					float num;
					Vec3 vec2;
					base.Mission.Scene.GetTerrainHeightAndNormal(gameEntity.GlobalPosition.AsVec2, out num, out vec2);
					MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
					globalFrame.origin.z = num;
					globalFrame.rotation.u = vec2;
					globalFrame.rotation.Orthonormalize();
					gameEntity.SetGlobalFrame(in globalFrame, true);
				}
				IEnumerable<GameEntity> enumerable = from c in this._entity.GetChildren()
					where c.HasTag("caravan_animal_spawn")
					select c;
				int num2 = (int)((float)enumerable.Count<GameEntity>() * 0.4f);
				foreach (GameEntity gameEntity2 in enumerable)
				{
					MatrixFrame globalFrame2 = gameEntity2.GetGlobalFrame();
					string text;
					if (this._isCamelCulture)
					{
						if (num2 > 0)
						{
							int num3 = MBRandom.RandomInt(this._camelMountableHarnesses.Length);
							text = this._camelMountableHarnesses[num3];
						}
						else
						{
							int num4 = MBRandom.RandomInt(this._camelLoadHarnesses.Length);
							text = this._camelLoadHarnesses[num4];
						}
					}
					else if (num2 > 0)
					{
						int num5 = MBRandom.RandomInt(this._muleMountableHarnesses.Length);
						text = this._muleMountableHarnesses[num5];
					}
					else
					{
						int num6 = MBRandom.RandomInt(this._muleLoadHarnesses.Length);
						text = this._muleLoadHarnesses[num6];
					}
					ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>(text), 0, null);
					ItemRosterElement itemRosterElement2 = (this._isCamelCulture ? ((num2-- > 0) ? new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("pack_camel"), 0, null) : new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("pack_camel_unmountable"), 0, null)) : ((num2-- > 0) ? new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("mule"), 0, null) : new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("mule_unmountable"), 0, null)));
					Mission mission = Mission.Current;
					ItemRosterElement itemRosterElement3 = itemRosterElement2;
					ItemRosterElement itemRosterElement4 = itemRosterElement;
					Vec2 vec3 = globalFrame2.rotation.f.AsVec2;
					vec3 = vec3.Normalized();
					Agent agent = mission.SpawnMonster(itemRosterElement3, itemRosterElement4, in globalFrame2.origin, in vec3, -1);
					agent.SetAgentFlags(agent.GetAgentFlags() & ~AgentFlag.CanWander);
					this._entity.GetFirstScriptInFamilyDescending<TacticalPosition>().UpdateLinkedTacticalPositions();
				}
			}
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000E4FF0 File Offset: 0x000E31F0
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			TacticalPosition firstScriptInFamilyDescending = this._entity.GetFirstScriptInFamilyDescending<TacticalPosition>();
			if (firstScriptInFamilyDescending != null)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					team.TeamAI.TacticalPositions.Add(firstScriptInFamilyDescending);
					if (team == this._caravanTeam && (!team.IsPlayerTeam || !team.IsPlayerGeneral))
					{
						team.TeamAI.ResetTactic(false);
					}
				}
			}
		}

		// Token: 0x040017CF RID: 6095
		private const float CaravanDeploymentOffset = 2f;

		// Token: 0x040017D0 RID: 6096
		private GameEntity _entity;

		// Token: 0x040017D1 RID: 6097
		private bool _isCamelCulture;

		// Token: 0x040017D2 RID: 6098
		private bool _isCaravan;

		// Token: 0x040017D3 RID: 6099
		private Team _caravanTeam;

		// Token: 0x040017D4 RID: 6100
		private BattleSideEnum _playerSide;

		// Token: 0x040017D5 RID: 6101
		private IBattleCombatant _caravanCombatant;

		// Token: 0x040017D6 RID: 6102
		private readonly string[] _camelLoadHarnesses = new string[] { "camel_saddle_a", "camel_saddle_b" };

		// Token: 0x040017D7 RID: 6103
		private readonly string[] _camelMountableHarnesses = new string[] { "camel_saddle" };

		// Token: 0x040017D8 RID: 6104
		private readonly string[] _muleLoadHarnesses = new string[] { "mule_load_a", "mule_load_b", "mule_load_c" };

		// Token: 0x040017D9 RID: 6105
		private readonly string[] _muleMountableHarnesses = new string[] { "aseran_village_harness", "steppe_fur_harness", "steppe_harness" };

		// Token: 0x040017DA RID: 6106
		private const string CaravanPrefabName = "caravan_scattered_goods_prop";

		// Token: 0x040017DB RID: 6107
		private const string VillagerGoodsPrefabName = "villager_scattered_goods_prop";
	}
}
