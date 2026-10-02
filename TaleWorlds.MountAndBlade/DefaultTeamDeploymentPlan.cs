using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000218 RID: 536
	public class DefaultTeamDeploymentPlan : ITeamDeploymentPlan
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x0006C7D1 File Offset: 0x0006A9D1
		// (set) Token: 0x06001F53 RID: 8019 RVA: 0x0006C7D9 File Offset: 0x0006A9D9
		public Team Team { get; private set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x0006C7E2 File Offset: 0x0006A9E2
		// (set) Token: 0x06001F55 RID: 8021 RVA: 0x0006C7EA File Offset: 0x0006A9EA
		public bool SpawnWithHorses { get; private set; }

		// Token: 0x06001F56 RID: 8022 RVA: 0x0006C7F4 File Offset: 0x0006A9F4
		public DefaultTeamDeploymentPlan(Mission mission, Team team)
		{
			this._mission = mission;
			this.Team = team;
			this._deploymentZoneFrame = MatrixFrame.Identity;
			this._deploymentBoundaries = new MBList<ValueTuple<string, MBList<Vec2>>>();
			this.SpawnWithHorses = false;
			this._initialPlan = DefaultDeploymentPlan.CreateInitialPlan(this._mission, this.Team);
			this._reinforcementPlans = new List<DefaultDeploymentPlan>();
			this._currentReinforcementPlan = this._initialPlan;
			if (this._mission.HasSpawnPath)
			{
				foreach (ValueTuple<SpawnPathData, float> valueTuple in this._mission.GetReinforcementPathsDataOfSide(this.Team.Side))
				{
					DefaultDeploymentPlan defaultDeploymentPlan = DefaultDeploymentPlan.CreateReinforcementPlanWithSpawnPath(this._mission, this.Team, valueTuple.Item1, valueTuple.Item2);
					this._reinforcementPlans.Add(defaultDeploymentPlan);
				}
				this._currentReinforcementPlan = this._reinforcementPlans[0];
				return;
			}
			DefaultDeploymentPlan defaultDeploymentPlan2 = DefaultDeploymentPlan.CreateReinforcementPlan(this._mission, this.Team);
			this._reinforcementPlans.Add(defaultDeploymentPlan2);
			this._currentReinforcementPlan = defaultDeploymentPlan2;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0006C920 File Offset: 0x0006AB20
		public void SetSpawnWithHorses(bool value)
		{
			this.SpawnWithHorses = value;
			this._initialPlan.SetSpawnWithHorses(value);
			foreach (DefaultDeploymentPlan defaultDeploymentPlan in this._reinforcementPlans)
			{
				defaultDeploymentPlan.SetSpawnWithHorses(value);
			}
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0006C984 File Offset: 0x0006AB84
		public void MakeDeploymentPlan(float spawnPathOffset = 0f, float targetOffset = 0f, FormationSceneSpawnEntry[,] formationSceneSpawnEntries = null, bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				using (List<DefaultDeploymentPlan>.Enumerator enumerator = this._reinforcementPlans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DefaultDeploymentPlan defaultDeploymentPlan = enumerator.Current;
						defaultDeploymentPlan.MakeDeploymentPlan(formationSceneSpawnEntries);
					}
					return;
				}
			}
			this._initialPlan.SetSpawnPathOffset(spawnPathOffset, targetOffset);
			this._initialPlan.MakeDeploymentPlan(formationSceneSpawnEntries);
			this.PlanDeploymentZone();
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0006C9F8 File Offset: 0x0006ABF8
		public void UpdateReinforcementPlans()
		{
			if (this._reinforcementPlans.Count <= 1)
			{
				return;
			}
			foreach (DefaultDeploymentPlan defaultDeploymentPlan in this._reinforcementPlans)
			{
				defaultDeploymentPlan.UpdateSafetyScore();
			}
			if (!this._currentReinforcementPlan.IsSafeToDeploy)
			{
				this._currentReinforcementPlan = this._reinforcementPlans.MaxBy<DefaultDeploymentPlan, float>((DefaultDeploymentPlan plan) => plan.SafetyScore);
			}
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0006CA94 File Offset: 0x0006AC94
		public void ClearPlan(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				using (List<DefaultDeploymentPlan>.Enumerator enumerator = this._reinforcementPlans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DefaultDeploymentPlan defaultDeploymentPlan = enumerator.Current;
						defaultDeploymentPlan.ClearPlan();
					}
					return;
				}
			}
			this._initialPlan.ClearPlan();
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0006CAF4 File Offset: 0x0006ACF4
		public void ClearAddedTroops(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				using (List<DefaultDeploymentPlan>.Enumerator enumerator = this._reinforcementPlans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DefaultDeploymentPlan defaultDeploymentPlan = enumerator.Current;
						defaultDeploymentPlan.ClearAddedTroops();
					}
					return;
				}
			}
			this._initialPlan.ClearAddedTroops();
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0006CB54 File Offset: 0x0006AD54
		public void AddTroops(FormationClass formationClass, int footTroopCount, int mountedTroopCount, bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				using (List<DefaultDeploymentPlan>.Enumerator enumerator = this._reinforcementPlans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DefaultDeploymentPlan defaultDeploymentPlan = enumerator.Current;
						defaultDeploymentPlan.AddTroops(formationClass, footTroopCount, mountedTroopCount);
					}
					return;
				}
			}
			this._initialPlan.AddTroops(formationClass, footTroopCount, mountedTroopCount);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0006CBBC File Offset: 0x0006ADBC
		public int GetTroopCount(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.TroopCount;
			}
			return this._initialPlan.TroopCount;
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0006CBD8 File Offset: 0x0006ADD8
		public bool IsFirstPlan(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.PlanCount == 1;
			}
			return this._initialPlan.PlanCount == 1;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0006CBFA File Offset: 0x0006ADFA
		public bool IsPlanMade(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.IsPlanMade;
			}
			return this._initialPlan.IsPlanMade;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0006CC16 File Offset: 0x0006AE16
		[return: TupleElementNames(new string[] { "id", "points" })]
		public MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries()
		{
			return this._deploymentBoundaries;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0006CC1E File Offset: 0x0006AE1E
		public float GetSpawnPathOffset(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.SpawnPathReinforcementOffset;
			}
			return this._initialPlan.SpawnPathOffset;
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0006CC3A File Offset: 0x0006AE3A
		public float GetTargetOffset(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.TargetOffset;
			}
			return this._initialPlan.TargetOffset;
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0006CC56 File Offset: 0x0006AE56
		public MatrixFrame GetDeploymentZoneFrame()
		{
			return this._deploymentZoneFrame;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0006CC5E File Offset: 0x0006AE5E
		public MatrixFrame GetFormationsCenterFrameAndExtents(out Vec2 halfExtents, bool ignoreDimensionlessFormations = true)
		{
			return this._initialPlan.ComputeFormationsCenterFrameAndExtents(ignoreDimensionlessFormations, out halfExtents);
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0006CC6D File Offset: 0x0006AE6D
		public bool HasDeploymentBoundaries()
		{
			return !this._deploymentBoundaries.IsEmpty<ValueTuple<string, MBList<Vec2>>>();
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0006CC7D File Offset: 0x0006AE7D
		public IFormationDeploymentPlan GetFormationPlan(FormationClass fClass, bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.GetFormationPlan(fClass);
			}
			return this._initialPlan.GetFormationPlan(fClass);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0006CC9B File Offset: 0x0006AE9B
		public Vec3 GetMeanPosition(bool isReinforcement = false)
		{
			if (isReinforcement)
			{
				return this._currentReinforcementPlan.MeanPosition;
			}
			return this._initialPlan.MeanPosition;
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0006CCB7 File Offset: 0x0006AEB7
		public bool IsInitialPlanSuitableForFormations(ValueTuple<int, int>[] troopDataPerFormationClass)
		{
			return this._initialPlan.IsPlanSuitableForFormations(troopDataPerFormationClass);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0006CCC8 File Offset: 0x0006AEC8
		public bool IsPositionInsideDeploymentBoundaries(in Vec2 position, [TupleElementNames(new string[] { "id", "points" })] out ValueTuple<string, MBList<Vec2>> containingBoundaryTuple)
		{
			bool flag = false;
			containingBoundaryTuple = new ValueTuple<string, MBList<Vec2>>("", null);
			foreach (ValueTuple<string, MBList<Vec2>> valueTuple in this._deploymentBoundaries)
			{
				MBList<Vec2> item = valueTuple.Item2;
				if (MBSceneUtilities.IsPointInsideBoundaries(in position, item, 0.05f))
				{
					containingBoundaryTuple = valueTuple;
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0006CD48 File Offset: 0x0006AF48
		public Vec2 GetClosestDeploymentBoundaryPosition(in Vec2 position)
		{
			Vec2 vec = position;
			float num = float.MaxValue;
			foreach (ValueTuple<string, MBList<Vec2>> valueTuple in this._deploymentBoundaries)
			{
				MBList<Vec2> item = valueTuple.Item2;
				if (item.Count > 2)
				{
					Vec2 vec2;
					float num2 = MBSceneUtilities.FindClosestPointToBoundaries(in position, item, out vec2);
					if (num2 < num)
					{
						num = num2;
						vec = vec2;
					}
				}
			}
			return vec;
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0006CDC8 File Offset: 0x0006AFC8
		public bool GetPathDeploymentBoundaryIntersection(in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection)
		{
			WorldPosition worldPosition = startPosition;
			Vec2 vec = worldPosition.AsVec2;
			ValueTuple<string, MBList<Vec2>> valueTuple;
			this.IsPositionInsideDeploymentBoundaries(in vec, out valueTuple);
			intersection = WorldPosition.Invalid;
			NavigationPath value = DefaultTeamDeploymentPlan._navigationPath.Value;
			Scene scene = Mission.Current.Scene;
			worldPosition = startPosition;
			UIntPtr nearestNavMesh = worldPosition.GetNearestNavMesh();
			worldPosition = endPosition;
			UIntPtr nearestNavMesh2 = worldPosition.GetNearestNavMesh();
			worldPosition = startPosition;
			Vec2 asVec = worldPosition.AsVec2;
			worldPosition = endPosition;
			if (scene.GetPathBetweenAIFaces(nearestNavMesh, nearestNavMesh2, asVec, worldPosition.AsVec2, 0f, value, null) && value.Size > 0)
			{
				worldPosition = startPosition;
				Vec2 vec2 = worldPosition.AsVec2;
				ValueTuple<string, MBList<Vec2>> valueTuple2 = valueTuple;
				Vec2 vec3 = Vec2.Invalid;
				for (int i = 0; i < value.Size; i++)
				{
					Vec2 vec4 = value[i];
					ValueTuple<string, MBList<Vec2>> valueTuple3;
					if (!this.IsPositionInsideDeploymentBoundaries(in vec4, out valueTuple3))
					{
						vec3 = vec4;
						break;
					}
					vec2 = vec4;
					valueTuple2 = valueTuple3;
				}
				if (vec3.IsValid)
				{
					intersection = startPosition;
					intersection.SetVec2(vec2);
					vec = vec3 - vec2;
					Vec2 vec5 = vec.Normalized();
					Vec2 vec6;
					MBMath.IntersectRayWithPolygon(vec2, vec5, valueTuple2.Item2, out vec6);
					intersection.SetVec2(Mission.Current.Scene.GetLastPointOnNavigationMeshFromWorldPositionToDestination(ref intersection, vec6).AsVec2);
				}
				else
				{
					intersection = endPosition;
				}
			}
			else
			{
				intersection = startPosition;
			}
			DefaultTeamDeploymentPlan._navigationPath.Value.Size = 0;
			return intersection.IsValid;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0006CF50 File Offset: 0x0006B150
		public static MBList<Vec2> ComputeDeploymentBoundariesFromMissionBoundaries(ICollection<Vec2> missionBoundaries, in MatrixFrame deploymentFrame, float desiredWidth, float desiredDepth)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			if (missionBoundaries.Count > 2)
			{
				Vec3 vec = deploymentFrame.origin;
				Vec2 asVec = vec.AsVec2;
				vec = deploymentFrame.rotation.s;
				Vec2 vec2 = vec.AsVec2;
				Vec2 vec3 = vec2.Normalized();
				vec = deploymentFrame.rotation.f;
				vec2 = vec.AsVec2;
				Vec2 vec4 = vec2.Normalized();
				MBList<Vec2> mblist2 = missionBoundaries.ToMBList<Vec2>();
				float num = desiredWidth / 2f;
				List<ValueTuple<Vec2, Vec2>> list = new List<ValueTuple<Vec2, Vec2>>();
				Vec2 vec5;
				DefaultTeamDeploymentPlan.ClampRayToMissionBoundaries(mblist2, asVec, vec3, num, out vec5);
				DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec5);
				Vec2 vec6;
				DefaultTeamDeploymentPlan.ClampRayToMissionBoundaries(mblist2, asVec, -vec3, num, out vec6);
				DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec6);
				Vec2 vec7;
				bool flag = DefaultTeamDeploymentPlan.ClampRayToMissionBoundaries(mblist2, vec5, -vec4, desiredDepth, out vec7);
				float num2 = 0f;
				if (flag)
				{
					DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec7);
					num2 = vec5.Distance(vec7);
				}
				Vec2 vec8;
				bool flag2 = DefaultTeamDeploymentPlan.ClampRayToMissionBoundaries(mblist2, vec6, -vec4, desiredDepth, out vec8);
				float num3 = 0f;
				if (flag2)
				{
					DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec8);
					num3 = vec6.Distance(vec8);
				}
				Vec2 vec9;
				if (flag2 && num2 < desiredDepth && DefaultTeamDeploymentPlan.ClampRayToMissionBoundaries(mblist2, vec8, vec3, desiredWidth, out vec9) && vec9.DistanceToLineSegment(vec6, vec5, out vec2) > num2)
				{
					DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec9);
				}
				Vec2 vec10;
				if (flag && num3 < desiredDepth && DefaultTeamDeploymentPlan.ClampRayToMissionBoundaries(mblist2, vec7, -vec3, desiredWidth, out vec10) && vec10.DistanceToLineSegment(vec6, vec5, out vec2) > num3)
				{
					DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec10);
				}
				if (desiredDepth < 3.4028235E+38f)
				{
					Vec2 vec11 = vec5 - vec4 * desiredDepth;
					Vec2 vec12 = vec6 - vec4 * desiredDepth;
					list.Add(new ValueTuple<Vec2, Vec2>(vec11, vec5));
					list.Add(new ValueTuple<Vec2, Vec2>(vec5, vec6));
					list.Add(new ValueTuple<Vec2, Vec2>(vec6, vec12));
					list.Add(new ValueTuple<Vec2, Vec2>(vec12, vec11));
				}
				else
				{
					if (flag)
					{
						list.Add(new ValueTuple<Vec2, Vec2>(vec7, vec5));
					}
					list.Add(new ValueTuple<Vec2, Vec2>(vec5, vec6));
					if (flag2)
					{
						list.Add(new ValueTuple<Vec2, Vec2>(vec6, vec8));
					}
				}
				foreach (Vec2 vec13 in missionBoundaries)
				{
					bool flag3 = true;
					foreach (ValueTuple<Vec2, Vec2> valueTuple in list)
					{
						Vec2 vec14 = vec13 - valueTuple.Item1;
						Vec2 vec15 = valueTuple.Item2 - valueTuple.Item1;
						if (vec15.x * vec14.y - vec15.y * vec14.x <= 1E-06f)
						{
							flag3 = false;
							break;
						}
					}
					if (flag3)
					{
						DefaultTeamDeploymentPlan.AddDeploymentBoundaryPoint(mblist, vec13);
					}
				}
				MBSceneUtilities.RadialSortBoundary(ref mblist);
				MBSceneUtilities.FindConvexHull(ref mblist);
			}
			return mblist;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0006D258 File Offset: 0x0006B458
		private void PlanDeploymentZone()
		{
			if (this._mission.HasSpawnPath || this._mission.IsFieldBattle || this._mission.IsNavalRaidBattle)
			{
				if (this._mission.HasSpawnPath || this.Team.Side == BattleSideEnum.Attacker)
				{
					this.ComputeDeploymentZoneFromFormations(true, true, 0f);
					return;
				}
				this.ComputeDeploymentZoneFromFormations(false, false, 50f);
				return;
			}
			else
			{
				if (this._mission.IsSiegeBattle)
				{
					this.ComputeDeploymentZoneFromSceneDeploymentBoundaries();
					return;
				}
				this._deploymentZoneFrame = MatrixFrame.Identity;
				this._deploymentBoundaries.Clear();
				return;
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0006D2F0 File Offset: 0x0006B4F0
		private void ComputeDeploymentZoneFromFormations(bool addExtraWidthFromTroopCount, bool useMaxDepth, float sideMargin = 0f)
		{
			this._initialPlan.GetFirstValidFormationFrame(out this._deploymentZoneFrame, false);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < 10; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				DefaultFormationDeploymentPlan formationPlan = this._initialPlan.GetFormationPlan(formationClass);
				if (formationPlan.HasFrame() && formationPlan.PlannedTroopCount > 0)
				{
					MatrixFrame frame = formationPlan.GetFrame();
					MatrixFrame matrixFrame = this._deploymentZoneFrame.TransformToLocal(in frame);
					float num5 = formationPlan.PlannedDepth * 0.5f;
					float num6 = formationPlan.PlannedWidth * 0.5f;
					Vec3 s = matrixFrame.rotation.s;
					Vec3 f = matrixFrame.rotation.f;
					float num7 = MathF.Abs(s.x) * num6 + MathF.Abs(f.x) * num5;
					float num8 = MathF.Abs(s.y) * num6 + MathF.Abs(f.y) * num5;
					num = Math.Max(matrixFrame.origin.y + num8, num);
					num2 = Math.Min(matrixFrame.origin.y - num8, num2);
					num3 = Math.Max(matrixFrame.origin.x + num7, num3);
					num4 = Math.Min(matrixFrame.origin.x - num7, num4);
				}
			}
			num += 10f;
			num2 = MathF.Abs(num2) + 20f;
			this._deploymentZoneFrame.Advance(num);
			float num9 = (num3 + num4) / 2f;
			this._deploymentZoneFrame.Strafe(num9);
			float num10 = num3 + MathF.Abs(num4);
			this._deploymentBoundaries.Clear();
			float num11 = num10 + sideMargin;
			if (addExtraWidthFromTroopCount)
			{
				float num12 = 4f * MathF.Sqrt((float)this._initialPlan.TroopCount);
				num11 += num12;
			}
			num11 = Math.Max(num11, 50f);
			num11 = Math.Min(num11, 300f);
			float num13 = num + num2;
			float num14 = (useMaxDepth ? float.MaxValue : num13);
			foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this._mission.Boundaries)
			{
				string key = keyValuePair.Key;
				MBList<Vec2> mblist = DefaultTeamDeploymentPlan.ComputeDeploymentBoundariesFromMissionBoundaries(keyValuePair.Value, in this._deploymentZoneFrame, num11, num14);
				this._deploymentBoundaries.Add(new ValueTuple<string, MBList<Vec2>>(key, mblist));
			}
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0006D56C File Offset: 0x0006B76C
		private void ComputeDeploymentZoneFromSceneDeploymentBoundaries()
		{
			this._deploymentBoundaries.Clear();
			foreach (ValueTuple<string, MBList<Vec2>, bool> valueTuple in MBSceneUtilities.GetDeploymentBoundaries(this.Team.Side))
			{
				MBList<Vec2> mblist = new MBList<Vec2>(valueTuple.Item2);
				MBSceneUtilities.RadialSortBoundary(ref mblist);
				MBSceneUtilities.FindConvexHull(ref mblist);
				this._deploymentBoundaries.Add(new ValueTuple<string, MBList<Vec2>>(valueTuple.Item1, mblist));
			}
			this._deploymentZoneFrame = this._mission.Scene.FindWeakEntityWithTag((this.Team.Side == BattleSideEnum.Attacker) ? "attacker_infantry" : "defender_infantry").GetGlobalFrame();
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0006D638 File Offset: 0x0006B838
		private static void AddDeploymentBoundaryPoint(MBList<Vec2> deploymentBoundaries, Vec2 point)
		{
			if (!deploymentBoundaries.Exists((Vec2 boundaryPoint) => boundaryPoint.Distance(point) <= 0.1f))
			{
				deploymentBoundaries.Add(point);
			}
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0006D674 File Offset: 0x0006B874
		private static bool ClampRayToMissionBoundaries(MBList<Vec2> boundaries, Vec2 origin, Vec2 direction, float maxLength, out Vec2 clampedIntersection)
		{
			if (Mission.Current.IsPositionInsideBoundaries(origin) && maxLength < 3.4028235E+38f)
			{
				Vec2 vec = origin + direction * maxLength;
				if (Mission.Current.IsPositionInsideBoundaries(vec))
				{
					clampedIntersection = vec;
					return true;
				}
			}
			return MBMath.IntersectRayWithPolygon(origin, direction, boundaries, out clampedIntersection);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0006D6E6 File Offset: 0x0006B8E6
		bool ITeamDeploymentPlan.IsPositionInsideDeploymentBoundaries(in Vec2 position, [TupleElementNames(new string[] { "id", "points" })] out ValueTuple<string, MBList<Vec2>> containingBoundaryTuple)
		{
			return this.IsPositionInsideDeploymentBoundaries(in position, out containingBoundaryTuple);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0006D6F0 File Offset: 0x0006B8F0
		Vec2 ITeamDeploymentPlan.GetClosestDeploymentBoundaryPosition(in Vec2 position)
		{
			return this.GetClosestDeploymentBoundaryPosition(in position);
		}

		// Token: 0x04000AAE RID: 2734
		public const float DeployZoneMinimumWidth = 50f;

		// Token: 0x04000AAF RID: 2735
		public const float DeployZoneMaximumWidth = 300f;

		// Token: 0x04000AB0 RID: 2736
		public const float DeployZoneForwardMargin = 10f;

		// Token: 0x04000AB1 RID: 2737
		public const float DeployZoneBackwardsMargin = 20f;

		// Token: 0x04000AB2 RID: 2738
		public const float DeployZoneExtraWidthPerSqrtTroopCount = 4f;

		// Token: 0x04000AB3 RID: 2739
		public const string DefenderDeploymentFrameEntityTag = "defender_infantry";

		// Token: 0x04000AB4 RID: 2740
		public const string AttackerDeploymentFrameEntityTag = "attacker_infantry";

		// Token: 0x04000AB7 RID: 2743
		private Mission _mission;

		// Token: 0x04000AB8 RID: 2744
		private readonly DefaultDeploymentPlan _initialPlan;

		// Token: 0x04000AB9 RID: 2745
		private readonly List<DefaultDeploymentPlan> _reinforcementPlans;

		// Token: 0x04000ABA RID: 2746
		private DefaultDeploymentPlan _currentReinforcementPlan;

		// Token: 0x04000ABB RID: 2747
		private MatrixFrame _deploymentZoneFrame;

		// Token: 0x04000ABC RID: 2748
		[TupleElementNames(new string[] { "id", "points" })]
		private readonly MBList<ValueTuple<string, MBList<Vec2>>> _deploymentBoundaries;

		// Token: 0x04000ABD RID: 2749
		private static ThreadLocal<NavigationPath> _navigationPath = new ThreadLocal<NavigationPath>(() => new NavigationPath());
	}
}
