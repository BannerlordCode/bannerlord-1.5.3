using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000217 RID: 535
	public class DefaultMissionDeploymentPlan : IMissionDeploymentPlan
	{
		// Token: 0x06001F22 RID: 7970 RVA: 0x0006BBF0 File Offset: 0x00069DF0
		public DefaultMissionDeploymentPlan(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0006BC18 File Offset: 0x00069E18
		public void Initialize()
		{
			foreach (Team team in this._mission.Teams)
			{
				if (team.Side != BattleSideEnum.None)
				{
					DefaultTeamDeploymentPlan defaultTeamDeploymentPlan = new DefaultTeamDeploymentPlan(this._mission, team);
					this._teamDeploymentPlans.Add(new ValueTuple<Team, DefaultTeamDeploymentPlan>(team, defaultTeamDeploymentPlan));
				}
			}
			for (int i = 0; i < 2; i++)
			{
				this._playerSpawnFrames[i] = null;
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0006BCB0 File Offset: 0x00069EB0
		public void ClearDeploymentPlan(Team team)
		{
			this.GetTeamPlan(team).ClearPlan(false);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0006BCBF File Offset: 0x00069EBF
		public void ClearReinforcementPlan(Team team)
		{
			this.GetTeamPlan(team).ClearPlan(true);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0006BCCE File Offset: 0x00069ECE
		public bool HasPlayerSpawnFrame(BattleSideEnum battleSide)
		{
			return this._playerSpawnFrames[(int)battleSide] != null;
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0006BCE4 File Offset: 0x00069EE4
		public bool GetPlayerSpawnFrame(BattleSideEnum battleSide, out WorldPosition position, out Vec2 direction)
		{
			WorldFrame? worldFrame = this._playerSpawnFrames[(int)battleSide];
			if (worldFrame != null)
			{
				Scene scene = Mission.Current.Scene;
				UIntPtr zero = UIntPtr.Zero;
				WorldFrame worldFrame2 = worldFrame.Value;
				position = new WorldPosition(scene, zero, worldFrame2.Origin.GetGroundVec3(), false);
				worldFrame2 = worldFrame.Value;
				direction = worldFrame2.Rotation.f.AsVec2.Normalized();
				return true;
			}
			position = WorldPosition.Invalid;
			direction = Vec2.Invalid;
			return false;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0006BD76 File Offset: 0x00069F76
		public static bool HasSignificantMountedTroops(int footTroopCount, int mountedTroopCount)
		{
			return (float)mountedTroopCount / Math.Max((float)(mountedTroopCount + footTroopCount), 1f) >= 0.1f;
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0006BD93 File Offset: 0x00069F93
		public void ClearAddedTroops(Team team, bool isReinforcement = false)
		{
			this.GetTeamPlan(team).ClearAddedTroops(isReinforcement);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0006BDA4 File Offset: 0x00069FA4
		public void ClearAll()
		{
			foreach (ValueTuple<Team, DefaultTeamDeploymentPlan> valueTuple in this._teamDeploymentPlans)
			{
				DefaultTeamDeploymentPlan item = valueTuple.Item2;
				item.ClearAddedTroops(false);
				item.ClearPlan(false);
				item.ClearAddedTroops(true);
				item.ClearPlan(true);
			}
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0006BE10 File Offset: 0x0006A010
		public void AddTroops(Team team, FormationClass formationClass, int footTroopCount, int mountedTroopCount = 0, bool isReinforcement = false)
		{
			BattleSideEnum side = team.Side;
			this.GetTeamPlan(team).AddTroops(formationClass, footTroopCount, mountedTroopCount, isReinforcement);
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0006BE2B File Offset: 0x0006A02B
		public void SetSpawnWithHorses(Team team, bool spawnWithHorses)
		{
			this.GetTeamPlan(team).SetSpawnWithHorses(spawnWithHorses);
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0006BE3C File Offset: 0x0006A03C
		public void MakeDefaultDeploymentPlans()
		{
			foreach (ValueTuple<Team, DefaultTeamDeploymentPlan> valueTuple in this._teamDeploymentPlans)
			{
				Team item = valueTuple.Item1;
				this.MakeDeploymentPlan(item, 0f, 0f);
				this.MakeReinforcementDeploymentPlan(item);
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0006BEA8 File Offset: 0x0006A0A8
		public void MakeDeploymentPlan(Team team, float spawnPathOffset = 0f, float targetOffset = 0f)
		{
			if (!this.IsPlanMade(team))
			{
				this.MakeDeploymentPlanAux(team, false, spawnPathOffset, targetOffset);
				bool flag;
				if (this.IsPlanMade(team, out flag))
				{
					this._mission.OnDeploymentPlanMade(team, flag);
				}
			}
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0006BEE0 File Offset: 0x0006A0E0
		public void MakeReinforcementDeploymentPlan(Team team)
		{
			if (!this.IsReinforcementPlanMade(team))
			{
				this.MakeDeploymentPlanAux(team, true, 0f, 0f);
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0006BF00 File Offset: 0x0006A100
		public bool RemakeDeploymentPlan(Team team)
		{
			this.IsPlanMade(team);
			float spawnPathOffset = this.GetSpawnPathOffset(team);
			float targetOffset = this.GetTargetOffset(team);
			ValueTuple<int, int>[] array = new ValueTuple<int, int>[11];
			foreach (Agent agent2 in this._mission.AllAgents.Where<Agent>((Agent agent) => agent.IsHuman && agent.Team != null && agent.Team == team && agent.Formation != null))
			{
				int formationIndex = (int)agent2.Formation.FormationIndex;
				ValueTuple<int, int> valueTuple = array[formationIndex];
				array[formationIndex] = (agent2.HasMount ? new ValueTuple<int, int>(valueTuple.Item1, valueTuple.Item2 + 1) : new ValueTuple<int, int>(valueTuple.Item1 + 1, valueTuple.Item2));
			}
			if (!this.IsInitialPlanSuitableForFormations(team, array))
			{
				this.ClearAddedTroops(team, false);
				this.ClearDeploymentPlan(team);
				for (int i = 0; i < 11; i++)
				{
					ValueTuple<int, int> valueTuple2 = array[i];
					int item = valueTuple2.Item1;
					int item2 = valueTuple2.Item2;
					if (item + item2 > 0)
					{
						this.AddTroops(team, (FormationClass)i, item, item2, false);
					}
				}
				this.MakeDeploymentPlan(team, spawnPathOffset, targetOffset);
				return this.IsPlanMade(team);
			}
			return false;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0006C07C File Offset: 0x0006A27C
		public bool IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position)
		{
			ValueTuple<string, MBList<Vec2>> valueTuple;
			return this.GetTeamPlan(team).IsPositionInsideDeploymentBoundaries(in position, out valueTuple);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0006C098 File Offset: 0x0006A298
		public Vec2 GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position)
		{
			return this.GetTeamPlan(team).GetClosestDeploymentBoundaryPosition(in position);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0006C0A7 File Offset: 0x0006A2A7
		public bool SupportsReinforcements()
		{
			return true;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0006C0AA File Offset: 0x0006A2AA
		public bool SupportsNavmesh(Team team)
		{
			return true;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0006C0AD File Offset: 0x0006A2AD
		public bool GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection)
		{
			return this.GetTeamPlan(team).GetPathDeploymentBoundaryIntersection(in startPosition, in endPosition, out intersection);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0006C0C0 File Offset: 0x0006A2C0
		public bool IsPositionInsideSiegeDeploymentBoundaries(in Vec2 position)
		{
			bool flag = false;
			foreach (ICollection<Vec2> collection in this._mission.Boundaries.Values)
			{
				if (MBSceneUtilities.IsPointInsideBoundaries(in position, collection.ToMBList<Vec2>(), 0.05f))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0006C12C File Offset: 0x0006A32C
		public float GetSpawnPathOffset(Team team)
		{
			return this.GetTeamPlan(team).GetSpawnPathOffset(false);
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0006C13B File Offset: 0x0006A33B
		public float GetTargetOffset(Team team)
		{
			return this.GetTeamPlan(team).GetTargetOffset(false);
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0006C14A File Offset: 0x0006A34A
		public int GetTroopCount(Team team, bool isReinforcement = false)
		{
			return this.GetTeamPlan(team).GetTroopCount(isReinforcement);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0006C159 File Offset: 0x0006A359
		public IFormationDeploymentPlan GetFormationPlan(Team team, FormationClass fClass, bool isReinforcement)
		{
			return this.GetTeamPlan(team).GetFormationPlan(fClass, isReinforcement);
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0006C16C File Offset: 0x0006A36C
		public bool IsPlanMade(Team team)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			return teamPlanAux != null && teamPlanAux.IsPlanMade(false);
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0006C190 File Offset: 0x0006A390
		public bool IsPlanMade(Team team, out bool isFirstPlan)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			isFirstPlan = false;
			if (teamPlanAux != null && teamPlanAux.IsPlanMade(false))
			{
				isFirstPlan = teamPlanAux.IsFirstPlan(false);
				return true;
			}
			return false;
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x0006C1C0 File Offset: 0x0006A3C0
		public bool IsReinforcementPlanMade(Team team)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			return teamPlanAux != null && teamPlanAux.IsPlanMade(true);
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0006C1E1 File Offset: 0x0006A3E1
		public bool IsInitialPlanSuitableForFormations(Team team, [TupleElementNames(new string[] { "footTroopCount", "mountedTroopCount" })] ValueTuple<int, int>[] troopDataPerFormationClass)
		{
			return this.GetTeamPlan(team).IsInitialPlanSuitableForFormations(troopDataPerFormationClass);
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0006C1F0 File Offset: 0x0006A3F0
		public bool HasDeploymentBoundaries(Team team)
		{
			DefaultTeamDeploymentPlan teamPlanAux = this.GetTeamPlanAux(team);
			return teamPlanAux != null && teamPlanAux.HasDeploymentBoundaries();
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0006C210 File Offset: 0x0006A410
		public MatrixFrame GetDeploymentZoneFrame(Team team)
		{
			return this.GetTeamPlan(team).GetDeploymentZoneFrame();
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0006C21E File Offset: 0x0006A41E
		public MatrixFrame GetFormationsCenterFrameAndExtents(Team team, out Vec2 halfExtents, bool ignoreDimensionlessFormations = true)
		{
			return this.GetTeamPlan(team).GetFormationsCenterFrameAndExtents(out halfExtents, ignoreDimensionlessFormations);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0006C230 File Offset: 0x0006A430
		public void ProjectPositionToDeploymentBoundaries(Team team, ref WorldPosition endPosition)
		{
			if (this.HasDeploymentBoundaries(team))
			{
				Vec2 asVec = endPosition.AsVec2;
				if (!this.IsPositionInsideDeploymentBoundaries(team, in asVec))
				{
					WorldPosition navmeshValidPositionInDeploymentZone = this.GetNavmeshValidPositionInDeploymentZone(team);
					WorldPosition worldPosition;
					if (this.GetPathDeploymentBoundaryIntersection(team, in navmeshValidPositionInDeploymentZone, in endPosition, out worldPosition))
					{
						endPosition = worldPosition;
					}
				}
			}
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0006C275 File Offset: 0x0006A475
		[return: TupleElementNames(new string[] { "id", "points" })]
		public MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries(Team team)
		{
			return this.GetTeamPlan(team).GetDeploymentBoundaries();
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0006C283 File Offset: 0x0006A483
		public Vec3 GetMeanPosition(Team team, bool isReinforcement = false)
		{
			return this.GetTeamPlan(team).GetMeanPosition(isReinforcement);
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0006C292 File Offset: 0x0006A492
		public void UpdateReinforcementPlan(Team team)
		{
			this.GetTeamPlan(team).UpdateReinforcementPlans();
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0006C2A0 File Offset: 0x0006A4A0
		public MatrixFrame GetZoomFocusFrame(Team team)
		{
			Vec2 vec;
			return this.GetFormationsCenterFrameAndExtents(team, out vec, true);
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0006C2B7 File Offset: 0x0006A4B7
		public float GetZoomOffset(Team team, float fovAngle)
		{
			return 0.2f * (float)this.GetTroopCount(team, false);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0006C2C8 File Offset: 0x0006A4C8
		private void MakeDeploymentPlanAux(Team team, bool isReinforcement = false, float spawnOffset = 0f, float targetOffset = 0f)
		{
			DefaultTeamDeploymentPlan teamPlan = this.GetTeamPlan(team);
			if (teamPlan.IsPlanMade(isReinforcement))
			{
				teamPlan.ClearPlan(isReinforcement);
			}
			if (!this._mission.HasSpawnPath && this._formationSceneSpawnEntries == null)
			{
				this.ReadSpawnEntitiesFromScene(this._mission.IsFieldBattle);
			}
			teamPlan.MakeDeploymentPlan(spawnOffset, targetOffset, this._formationSceneSpawnEntries, isReinforcement);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0006C324 File Offset: 0x0006A524
		private void ReadSpawnEntitiesFromScene(bool isFieldBattle)
		{
			for (int i = 0; i < 2; i++)
			{
				this._playerSpawnFrames[i] = null;
			}
			this._formationSceneSpawnEntries = new FormationSceneSpawnEntry[2, 11];
			Scene scene = this._mission.Scene;
			if (isFieldBattle)
			{
				for (int j = 0; j < 2; j++)
				{
					string text = ((j == 1) ? "attacker_" : "defender_");
					for (int k = 0; k < 11; k++)
					{
						FormationClass formationClass = (FormationClass)k;
						WeakGameEntity weakGameEntity = scene.FindWeakEntityWithTag(text + formationClass.GetName().ToLower());
						if (weakGameEntity == null)
						{
							FormationClass formationClass2 = formationClass.FallbackClass();
							int num = (int)formationClass2;
							GameEntity spawnEntity = this._formationSceneSpawnEntries[j, num].SpawnEntity;
							weakGameEntity = ((spawnEntity != null) ? spawnEntity.WeakEntity : scene.FindWeakEntityWithTag(text + formationClass2.GetName().ToLower()));
							formationClass = ((weakGameEntity != null) ? formationClass2 : FormationClass.NumberOfAllFormations);
						}
						GameEntity gameEntity = null;
						if (weakGameEntity.IsValid)
						{
							gameEntity = GameEntity.CreateFromWeakEntity(weakGameEntity);
						}
						this._formationSceneSpawnEntries[j, k] = new FormationSceneSpawnEntry(formationClass, gameEntity, gameEntity);
					}
				}
				return;
			}
			GameEntity gameEntity2 = null;
			if (this._mission.IsSallyOutBattle)
			{
				gameEntity2 = scene.FindEntityWithTag("sally_out_ambush_battle_set");
			}
			if (gameEntity2 != null)
			{
				this.ReadSallyOutEntitiesFromScene(gameEntity2);
			}
			else
			{
				this.ReadSiegeBattleEntitiesFromScene(scene, BattleSideEnum.Defender);
			}
			this.ReadSiegeBattleEntitiesFromScene(scene, BattleSideEnum.Attacker);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0006C4A4 File Offset: 0x0006A6A4
		private void ReadSallyOutEntitiesFromScene(GameEntity sallyOutSetEntity)
		{
			int num = 0;
			MatrixFrame globalFrame = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_player").GetGlobalFrame();
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalFrame.origin, false);
			this._playerSpawnFrames[num] = new WorldFrame?(new WorldFrame(globalFrame.rotation, worldPosition));
			GameEntity firstChildEntityWithTag = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_infantry");
			GameEntity firstChildEntityWithTag2 = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_archer");
			GameEntity firstChildEntityWithTag3 = sallyOutSetEntity.GetFirstChildEntityWithTag("sally_out_ambush_cavalry");
			for (int i = 0; i < 11; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				FormationClass formationClass2 = formationClass.FallbackClass();
				GameEntity gameEntity = null;
				switch (formationClass2)
				{
				case FormationClass.Infantry:
					gameEntity = firstChildEntityWithTag;
					break;
				case FormationClass.Ranged:
					gameEntity = firstChildEntityWithTag2;
					break;
				case FormationClass.Cavalry:
				case FormationClass.HorseArcher:
					gameEntity = firstChildEntityWithTag3;
					break;
				}
				this._formationSceneSpawnEntries[num, i] = new FormationSceneSpawnEntry(formationClass, gameEntity, gameEntity);
			}
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0006C588 File Offset: 0x0006A788
		private void ReadSiegeBattleEntitiesFromScene(Scene missionScene, BattleSideEnum battleSide)
		{
			int num = (int)battleSide;
			string text = battleSide.ToString().ToLower() + "_";
			for (int i = 0; i < 11; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				string text2 = text + formationClass.GetName().ToLower();
				string text3 = text2 + "_reinforcement";
				GameEntity gameEntity = missionScene.FindEntityWithTag(text2);
				GameEntity gameEntity2;
				if (gameEntity == null)
				{
					FormationClass formationClass2 = formationClass.FallbackClass();
					int num2 = (int)formationClass2;
					FormationSceneSpawnEntry formationSceneSpawnEntry = this._formationSceneSpawnEntries[num, num2];
					if (formationSceneSpawnEntry.SpawnEntity != null)
					{
						gameEntity = formationSceneSpawnEntry.SpawnEntity;
						gameEntity2 = formationSceneSpawnEntry.ReinforcementSpawnEntity;
					}
					else
					{
						text2 = text + formationClass2.GetName().ToLower();
						text3 = text2 + "_reinforcement";
						gameEntity = missionScene.FindEntityWithTag(text2);
						gameEntity2 = missionScene.FindEntityWithTag(text3);
					}
					formationClass = ((gameEntity != null) ? formationClass2 : FormationClass.NumberOfAllFormations);
				}
				else
				{
					gameEntity2 = missionScene.FindEntityWithTag(text3);
				}
				if (gameEntity2 == null)
				{
					gameEntity2 = gameEntity;
				}
				this._formationSceneSpawnEntries[num, i] = new FormationSceneSpawnEntry(formationClass, gameEntity, gameEntity2);
			}
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0006C6BB File Offset: 0x0006A8BB
		private DefaultTeamDeploymentPlan GetTeamPlan(Team team)
		{
			return this.GetTeamPlanAux(team);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0006C6C4 File Offset: 0x0006A8C4
		private DefaultTeamDeploymentPlan GetTeamPlanAux(Team team)
		{
			return this._teamDeploymentPlans.FirstOrDefault<ValueTuple<Team, DefaultTeamDeploymentPlan>>(([TupleElementNames(new string[] { "team", "plan" })] ValueTuple<Team, DefaultTeamDeploymentPlan> t) => t.Item1 == team).Item2;
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0006C6FC File Offset: 0x0006A8FC
		private WorldPosition GetNavmeshValidPositionInDeploymentZone(Team team)
		{
			DefaultTeamDeploymentPlan teamPlan = this.GetTeamPlan(team);
			Scene scene = Mission.Current.Scene;
			Vec3 origin = teamPlan.GetDeploymentZoneFrame().origin;
			UIntPtr navigationMeshForPosition = scene.GetNavigationMeshForPosition(in origin);
			if (navigationMeshForPosition != UIntPtr.Zero)
			{
				return new WorldPosition(scene, navigationMeshForPosition, origin, false);
			}
			for (FormationClass formationClass = FormationClass.Infantry; formationClass < FormationClass.NumberOfAllFormations; formationClass++)
			{
				IFormationDeploymentPlan formationPlan = teamPlan.GetFormationPlan(formationClass, false);
				if (formationPlan.HasFrame())
				{
					Vec3 origin2 = formationPlan.GetFrame().origin;
					return new WorldPosition(scene, UIntPtr.Zero, origin2, false);
				}
			}
			Debug.FailedAssert("Unable to find a formation frame that is on navmesh", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Deployment\\DefaultMissionDeploymentPlan.cs", "GetNavmeshValidPositionInDeploymentZone", 623);
			return new WorldPosition(scene, UIntPtr.Zero, origin, false);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0006C7B0 File Offset: 0x0006A9B0
		bool IMissionDeploymentPlan.IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position)
		{
			return this.IsPositionInsideDeploymentBoundaries(team, in position);
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0006C7BA File Offset: 0x0006A9BA
		Vec2 IMissionDeploymentPlan.GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position)
		{
			return this.GetClosestDeploymentBoundaryPosition(team, in position);
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0006C7C4 File Offset: 0x0006A9C4
		bool IMissionDeploymentPlan.GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection)
		{
			return this.GetPathDeploymentBoundaryIntersection(team, in startPosition, in endPosition, out intersection);
		}

		// Token: 0x04000AAA RID: 2730
		private readonly Mission _mission;

		// Token: 0x04000AAB RID: 2731
		[TupleElementNames(new string[] { "team", "plan" })]
		private readonly MBList<ValueTuple<Team, DefaultTeamDeploymentPlan>> _teamDeploymentPlans = new MBList<ValueTuple<Team, DefaultTeamDeploymentPlan>>();

		// Token: 0x04000AAC RID: 2732
		private readonly WorldFrame?[] _playerSpawnFrames = new WorldFrame?[2];

		// Token: 0x04000AAD RID: 2733
		private FormationSceneSpawnEntry[,] _formationSceneSpawnEntries;
	}
}
