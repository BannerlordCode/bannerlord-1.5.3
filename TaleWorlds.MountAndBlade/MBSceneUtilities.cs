using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000258 RID: 600
	public static class MBSceneUtilities
	{
		// Token: 0x06002240 RID: 8768 RVA: 0x00078488 File Offset: 0x00076688
		public static MBList<Path> GetAllSpawnPaths(Scene scene)
		{
			MBList<Path> mblist = new MBList<Path>();
			for (int i = 0; i < 32; i++)
			{
				string text = "spawn_path_" + i.ToString("D2");
				Path pathWithName = scene.GetPathWithName(text);
				if (pathWithName != null && pathWithName.NumberOfPoints > 1)
				{
					mblist.Add(pathWithName);
				}
			}
			return mblist;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000784E4 File Offset: 0x000766E4
		public static MBList<Vec2> GetSoftBoundaryPoints(Scene scene)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			int softBoundaryVertexCount = scene.GetSoftBoundaryVertexCount();
			if (softBoundaryVertexCount > 2)
			{
				for (int i = 0; i < softBoundaryVertexCount; i++)
				{
					Vec2 softBoundaryVertex = scene.GetSoftBoundaryVertex(i);
					mblist.Add(softBoundaryVertex);
				}
			}
			return mblist;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00078520 File Offset: 0x00076720
		public static MBList<Vec2> GetHardBoundaryPoints(Scene scene)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			int hardBoundaryVertexCount = scene.GetHardBoundaryVertexCount();
			for (int i = 0; i < hardBoundaryVertexCount; i++)
			{
				Vec2 hardBoundaryVertex = scene.GetHardBoundaryVertex(i);
				mblist.Add(hardBoundaryVertex);
			}
			return mblist;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00078558 File Offset: 0x00076758
		public static MBList<Vec2> GetSceneLimitPoints(Scene scene, out Vec2 sceneLimitMin, out Vec2 sceneLimitMax)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			Vec3 vec;
			Vec3 vec2;
			scene.GetSceneLimits(out vec, out vec2);
			mblist.Add(new Vec2(vec.x, vec.y));
			mblist.Add(new Vec2(vec2.x, vec.y));
			mblist.Add(new Vec2(vec2.x, vec2.y));
			mblist.Add(new Vec2(vec.x, vec2.y));
			sceneLimitMin = vec.AsVec2;
			sceneLimitMax = vec2.AsVec2;
			return mblist;
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000785EC File Offset: 0x000767EC
		[return: TupleElementNames(new string[] { "tag", "boundaryPoints", "insideAllowance" })]
		public static MBList<ValueTuple<string, MBList<Vec2>, bool>> GetDeploymentBoundaries(BattleSideEnum battleSide)
		{
			IEnumerable<GameEntity> enumerable = Mission.Current.Scene.FindEntitiesWithTagExpression("deployment_castle_boundary(_\\d+)*");
			List<ValueTuple<string, List<GameEntity>>> list = new List<ValueTuple<string, List<GameEntity>>>();
			foreach (GameEntity gameEntity in enumerable)
			{
				if (gameEntity.HasTag(battleSide.ToString()))
				{
					string[] tags = gameEntity.Tags;
					for (int i = 0; i < tags.Length; i++)
					{
						string tag = tags[i];
						if (tag.Contains("deployment_castle_boundary"))
						{
							ValueTuple<string, List<GameEntity>> valueTuple = list.FirstOrDefault<ValueTuple<string, List<GameEntity>>>(([TupleElementNames(new string[] { "tag", "boundaryEntities" })] ValueTuple<string, List<GameEntity>> tuple) => tuple.Item1.Equals(tag));
							if (valueTuple.Item1 == null)
							{
								valueTuple = new ValueTuple<string, List<GameEntity>>(tag, new List<GameEntity>());
								list.Add(valueTuple);
							}
							valueTuple.Item2.Add(gameEntity);
							break;
						}
					}
				}
			}
			MBList<ValueTuple<string, MBList<Vec2>, bool>> mblist = new MBList<ValueTuple<string, MBList<Vec2>, bool>>();
			foreach (ValueTuple<string, List<GameEntity>> valueTuple2 in list)
			{
				string item = valueTuple2.Item1;
				bool flag = !valueTuple2.Item2.Any<GameEntity>((GameEntity e) => e.HasTag("out"));
				MBList<Vec2> mblist2 = valueTuple2.Item2.Select<GameEntity, Vec2>((GameEntity bp) => bp.GlobalPosition.AsVec2).ToMBList<Vec2>();
				MBSceneUtilities.RadialSortBoundary(ref mblist2);
				mblist.Add(new ValueTuple<string, MBList<Vec2>, bool>(item, mblist2, flag));
			}
			return mblist;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000787B0 File Offset: 0x000769B0
		public static void GetAxisAlignedBoundaryRectangle(List<Vec2> boundaryPoints, out Vec2 boundsMin, out Vec2 boundsMax)
		{
			boundsMin = new Vec2(float.MaxValue, float.MaxValue);
			boundsMax = new Vec2(float.MinValue, float.MinValue);
			for (int i = 0; i < boundaryPoints.Count; i++)
			{
				Vec2 vec = boundaryPoints[i];
				if (vec.x < boundsMin.X)
				{
					boundsMin.x = vec.x;
				}
				if (vec.y < boundsMin.Y)
				{
					boundsMin.y = vec.y;
				}
				if (vec.x > boundsMax.X)
				{
					boundsMax.x = vec.x;
				}
				if (vec.y > boundsMax.Y)
				{
					boundsMax.y = vec.y;
				}
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00078868 File Offset: 0x00076A68
		public static void FindConvexHull(ref MBList<Vec2> boundary)
		{
			Vec2[] array = boundary.ToArray();
			int num = 0;
			MBAPI.IMBMission.FindConvexHull(array, boundary.Count, ref num);
			boundary = array.ToMBList<Vec2>();
			boundary.RemoveRange(num, boundary.Count - num);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000788AC File Offset: 0x00076AAC
		public static void RadialSortBoundary(ref MBList<Vec2> boundary)
		{
			MBSceneUtilities.<>c__DisplayClass18_0 CS$<>8__locals1 = new MBSceneUtilities.<>c__DisplayClass18_0();
			if (boundary.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.boundaryCenter = Vec2.Zero;
			foreach (Vec2 vec in boundary)
			{
				CS$<>8__locals1.boundaryCenter += vec;
			}
			MBSceneUtilities.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.boundaryCenter.x = CS$<>8__locals2.boundaryCenter.x / (float)boundary.Count;
			MBSceneUtilities.<>c__DisplayClass18_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.boundaryCenter.y = CS$<>8__locals3.boundaryCenter.y / (float)boundary.Count;
			boundary = boundary.OrderBy<Vec2, float>((Vec2 b) => (b - CS$<>8__locals1.boundaryCenter).RotationInRadians).ToMBList<Vec2>();
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x0007896C File Offset: 0x00076B6C
		public static void RadialSortBoundary(ref MBList<Vec3> boundary)
		{
			MBSceneUtilities.<>c__DisplayClass19_0 CS$<>8__locals1 = new MBSceneUtilities.<>c__DisplayClass19_0();
			if (boundary.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.boundaryCenter = Vec2.Zero;
			foreach (Vec3 vec in boundary)
			{
				CS$<>8__locals1.boundaryCenter += vec.AsVec2;
			}
			MBSceneUtilities.<>c__DisplayClass19_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.boundaryCenter.x = CS$<>8__locals2.boundaryCenter.x / (float)boundary.Count;
			MBSceneUtilities.<>c__DisplayClass19_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.boundaryCenter.y = CS$<>8__locals3.boundaryCenter.y / (float)boundary.Count;
			boundary = boundary.OrderBy<Vec3, float>((Vec3 b) => (b.AsVec2 - CS$<>8__locals1.boundaryCenter).RotationInRadians).ToMBList<Vec3>();
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00078A30 File Offset: 0x00076C30
		public static bool IsConvexAndRadiallySorted(MBList<Vec2> boundary)
		{
			int count = boundary.Count;
			if (count < 3)
			{
				return false;
			}
			Vec2 vec = new Vec2(0f, 0f);
			foreach (Vec2 vec2 in boundary)
			{
				vec += vec2;
			}
			vec /= (float)count;
			Vec2 vec3 = (boundary[0] - vec).Normalized();
			vec3.RotateCCW(-0.001f);
			Vec2 vec4 = vec3;
			for (int i = 0; i < count; i++)
			{
				Vec2 vec5 = boundary[i];
				vec3 = (vec5 - vec).Normalized();
				if (vec4.AngleBetween(vec3) <= 0f)
				{
					return false;
				}
				vec4 = vec3;
				Vec2 vec6 = boundary[(i + 1) % count];
				Vec2 vec7 = boundary[(i + 2) % count];
				Vec2 vec8 = vec6 - vec5;
				Vec2 vec9 = vec7 - vec5;
				if (Vec2.Determinant(in vec8, in vec9) < 0f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x00078B50 File Offset: 0x00076D50
		public static bool IsPointInsideBoundaries(in Vec2 point, MBList<Vec2> boundaries, float acceptanceThreshold = 0.05f)
		{
			if (boundaries.Count <= 2)
			{
				return false;
			}
			acceptanceThreshold = MathF.Max(0f, acceptanceThreshold);
			bool flag = true;
			for (int i = 0; i < boundaries.Count; i++)
			{
				Vec2 vec = boundaries[i];
				Vec2 vec2 = boundaries[(i + 1) % boundaries.Count] - vec;
				Vec2 vec3 = point - vec;
				if (vec2.x * vec3.y - vec2.y * vec3.x < 0f)
				{
					vec2.Normalize();
					Vec2 vec4 = vec3.DotProduct(vec2) * vec2;
					if ((vec3 - vec4).LengthSquared > acceptanceThreshold * acceptanceThreshold)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x00078C14 File Offset: 0x00076E14
		public static float FindClosestPointToBoundaries(in Vec2 position, MBList<Vec2> boundaries, out Vec2 closestPoint)
		{
			closestPoint = position;
			float num = float.MaxValue;
			for (int i = 0; i < boundaries.Count; i++)
			{
				Vec2 vec = boundaries[i];
				Vec2 vec2 = boundaries[(i + 1) % boundaries.Count];
				Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in vec, in vec2, in position);
				Vec2 vec3 = position;
				float num2 = vec3.DistanceSquared(closestPointOnLineSegmentToPoint);
				if (num2 <= num)
				{
					num = num2;
					closestPoint = closestPointOnLineSegmentToPoint;
				}
			}
			return MathF.Sqrt(num);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00078C94 File Offset: 0x00076E94
		public static float FindClosestPointToBoundariesReturnDistanceSquared(in Vec2 position, MBList<Vec2> boundaries, out Vec2 closestPoint, out bool isPositionInsideBoundaries)
		{
			closestPoint = position;
			float num = float.MaxValue;
			for (int i = 0; i < boundaries.Count; i++)
			{
				Vec2 vec = boundaries[i];
				Vec2 vec2 = boundaries[(i + 1) % boundaries.Count];
				Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in vec, in vec2, in position);
				Vec2 vec3 = position;
				float num2 = vec3.DistanceSquared(closestPointOnLineSegmentToPoint);
				if (num2 <= num)
				{
					num = num2;
					closestPoint = closestPointOnLineSegmentToPoint;
				}
			}
			isPositionInsideBoundaries = MBSceneUtilities.IsPointInsideBoundaries(in position, boundaries, 0.05f);
			return MathF.Sqrt(num);
		}

		// Token: 0x04000D33 RID: 3379
		public const int MaxNumberOfSpawnPaths = 32;

		// Token: 0x04000D34 RID: 3380
		public const string SpawnPathPrefix = "spawn_path_";

		// Token: 0x04000D35 RID: 3381
		public const string SoftBorderVertexTag = "walk_area_vertex";

		// Token: 0x04000D36 RID: 3382
		public const string HardBorderVertexTag = "walk_area_vertex_hard";

		// Token: 0x04000D37 RID: 3383
		public const string SoftBoundaryName = "walk_area";

		// Token: 0x04000D38 RID: 3384
		public const string SceneBoundaryName = "scene_boundary";

		// Token: 0x04000D39 RID: 3385
		public const float SceneToHardBoundaryMargin = 100f;

		// Token: 0x04000D3A RID: 3386
		public const string DefenderDeploymentReferencePositionTag = "defender_infantry";

		// Token: 0x04000D3B RID: 3387
		public const string AttackerDeploymentReferencePositionTag = "attacker_infantry";

		// Token: 0x04000D3C RID: 3388
		private const string DeploymentBoundaryTag = "deployment_castle_boundary";

		// Token: 0x04000D3D RID: 3389
		private const string DeploymentBoundaryTagExpression = "deployment_castle_boundary(_\\d+)*";
	}
}
