using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000213 RID: 531
	public class BattleSideSpawnPathSelector
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x000699B1 File Offset: 0x00067BB1
		public SpawnPathData InitialSpawnPath
		{
			get
			{
				return this._initialSpawnPath;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x000699B9 File Offset: 0x00067BB9
		[TupleElementNames(new string[] { "pathData", "startOffset" })]
		public MBReadOnlyList<ValueTuple<SpawnPathData, float>> ReinforcementPaths
		{
			[return: TupleElementNames(new string[] { "pathData", "startOffset" })]
			get
			{
				return this._reinforcementSpawnPaths;
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000699C4 File Offset: 0x00067BC4
		public BattleSideSpawnPathSelector(Mission mission, Path initialPath, float initialPivotOffset, bool initialPathIsInverted)
		{
			this._mission = mission;
			this._pathSnapMethod = (mission.IsNavalBattle ? SpawnPathData.SnapMethod.SnapToWaterLevel : (mission.IsFieldBattle ? SpawnPathData.SnapMethod.SnapToTerrain : SpawnPathData.SnapMethod.DontSnap));
			this._initialSpawnPath = SpawnPathData.Create(this._mission.Scene, initialPath, initialPivotOffset, initialPathIsInverted, this._pathSnapMethod);
			this._reinforcementSpawnPaths = new MBList<ValueTuple<SpawnPathData, float>>();
			this._tempPathPoints = new MatrixFrame[200];
			this.FindReinforcementPaths();
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00069A48 File Offset: 0x00067C48
		public bool HasReinforcementPath(Path path)
		{
			return path != null && this._reinforcementSpawnPaths.Exists(([TupleElementNames(new string[] { "pathData", "startOffset" })] ValueTuple<SpawnPathData, float> pdt) => pdt.Item1.Path.Pointer == path.Pointer);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x00069A8C File Offset: 0x00067C8C
		private void FindReinforcementPaths()
		{
			this._reinforcementSpawnPaths.Clear();
			MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate> mblist = new MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate>();
			float pathLength = this._initialSpawnPath.PathLength;
			float num = pathLength * 0.5f;
			float num2 = -pathLength * 0.5f;
			BattleSideSpawnPathSelector.ReinforcementPathCandidate reinforcementPathCandidate = new BattleSideSpawnPathSelector.ReinforcementPathCandidate(0f, this._initialSpawnPath.Path, num, num2, this._initialSpawnPath.IsInverted);
			mblist.Add(reinforcementPathCandidate);
			MBList<Path> allSpawnPaths = MBSceneUtilities.GetAllSpawnPaths(this._mission.Scene);
			if (allSpawnPaths.Count > 1)
			{
				this._initialSpawnPath.Path.GetPoints(this._tempPathPoints);
				Vec2 vec;
				Vec2 vec2;
				this.GetPathBaseFrameData(this._tempPathPoints, this._initialSpawnPath.Path.NumberOfPoints, this._initialSpawnPath.IsInverted, out vec, out vec2);
				Vec2 asVec = this._initialSpawnPath.GetCenterFrame().origin.AsVec2;
				MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate> mblist2 = new MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate>();
				MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate> mblist3 = new MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate>();
				foreach (Path path in allSpawnPaths)
				{
					if (!(path.Pointer == reinforcementPathCandidate.Path.Pointer))
					{
						path.GetPoints(this._tempPathPoints);
						float maxValue = float.MaxValue;
						float num3 = 0f;
						float num4 = 0f;
						float num5 = path.GetTotalLength() * 0.5f;
						if (this.IsValidReinforcementCandidate(in vec, in asVec, path, this._tempPathPoints, num5, false, out maxValue, out num3, out num4))
						{
							if (num3 > 1E-05f)
							{
								mblist2.Add(new BattleSideSpawnPathSelector.ReinforcementPathCandidate(maxValue, path, num5, num4, false));
							}
							else if (num3 < -1E-05f)
							{
								mblist3.Add(new BattleSideSpawnPathSelector.ReinforcementPathCandidate(maxValue, path, num5, num4, false));
							}
						}
						if (this.IsValidReinforcementCandidate(in vec, in asVec, path, this._tempPathPoints, num5, true, out maxValue, out num3, out num4))
						{
							if (num3 > 0.001f)
							{
								mblist2.Add(new BattleSideSpawnPathSelector.ReinforcementPathCandidate(maxValue, path, num5, num4, true));
							}
							else if (num3 < -0.001f)
							{
								mblist3.Add(new BattleSideSpawnPathSelector.ReinforcementPathCandidate(maxValue, path, num5, num4, true));
							}
						}
					}
				}
				if (mblist2.Count > 0 || mblist3.Count > 0)
				{
					mblist2.Sort((BattleSideSpawnPathSelector.ReinforcementPathCandidate left, BattleSideSpawnPathSelector.ReinforcementPathCandidate right) => right.Cost.CompareTo(left.Cost));
					mblist3.Sort((BattleSideSpawnPathSelector.ReinforcementPathCandidate left, BattleSideSpawnPathSelector.ReinforcementPathCandidate right) => right.Cost.CompareTo(left.Cost));
					int num6 = 2;
					MBList<UIntPtr> mblist4 = new MBList<UIntPtr>();
					MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate>[] array = new MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate>[] { mblist2, mblist3 };
					int num7 = 0;
					while (num6 > 0 && (mblist2.Count > 0 || mblist3.Count > 0))
					{
						MBList<BattleSideSpawnPathSelector.ReinforcementPathCandidate> mblist5 = array[num7];
						if (mblist5.Count > 0)
						{
							int num8 = mblist5.Count - 1;
							BattleSideSpawnPathSelector.ReinforcementPathCandidate reinforcementPathCandidate2 = mblist5[num8];
							mblist5.RemoveAt(num8);
							if (!mblist4.Contains(reinforcementPathCandidate2.Path.Pointer))
							{
								mblist.Add(reinforcementPathCandidate2);
								mblist4.Add(reinforcementPathCandidate2.Path.Pointer);
								num6--;
							}
						}
						num7 = (num7 + 1) % array.Length;
					}
				}
			}
			foreach (BattleSideSpawnPathSelector.ReinforcementPathCandidate reinforcementPathCandidate3 in mblist)
			{
				SpawnPathData spawnPathData = SpawnPathData.Create(this._initialSpawnPath.Scene, reinforcementPathCandidate3.Path, reinforcementPathCandidate3.PivotOffset, reinforcementPathCandidate3.IsInverted, this._pathSnapMethod);
				this._reinforcementSpawnPaths.Add(new ValueTuple<SpawnPathData, float>(spawnPathData, reinforcementPathCandidate3.ReinforcementOffset));
			}
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x00069E68 File Offset: 0x00068068
		private void GetPathBaseFrameData(MatrixFrame[] pathPoints, int pathPointCount, bool isInverted, out Vec2 mainPathBasePosition, out Vec2 mainPathForward)
		{
			MatrixFrame matrixFrame;
			MatrixFrame matrixFrame2;
			if (isInverted)
			{
				matrixFrame = pathPoints[pathPointCount - 1];
				matrixFrame2 = pathPoints[pathPointCount - 2];
			}
			else
			{
				matrixFrame = pathPoints[0];
				matrixFrame2 = pathPoints[1];
			}
			mainPathForward = (matrixFrame2.origin - matrixFrame.origin).AsVec2.Normalized();
			mainPathBasePosition = matrixFrame.origin.AsVec2;
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00069EDC File Offset: 0x000680DC
		private bool IsValidReinforcementCandidate(in Vec2 mainPathBasePosition, in Vec2 mainPathCenterPosition, Path candidatePath, MatrixFrame[] candidatePathPoints, float candidatePathPivotOffset, bool isCandidatePathInverted, out float cost, out float arcLength, out float reinforcementOffset)
		{
			cost = float.MaxValue;
			arcLength = 0f;
			reinforcementOffset = 0f;
			int numberOfPoints = candidatePath.NumberOfPoints;
			Vec2 vec = mainPathCenterPosition;
			Vec2 vec2 = mainPathBasePosition - vec;
			float length = vec2.Length;
			float num = MathF.Atan2(vec2.y, vec2.x);
			float totalLength = candidatePath.GetTotalLength();
			float num2 = (isCandidatePathInverted ? (totalLength * 0.5f) : 0f);
			float num3 = (isCandidatePathInverted ? totalLength : (totalLength * 0.5f));
			float num4 = 0f;
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			float num5 = 0f;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < numberOfPoints - 1; i++)
			{
				Vec2 asVec = candidatePathPoints[i].origin.AsVec2;
				Vec2 asVec2 = candidatePathPoints[i + 1].origin.AsVec2;
				float length2 = (asVec2 - asVec).Length;
				if (length2 > 0.001f)
				{
					float num6 = num4;
					float num7 = num4 + length2;
					float num8 = MathF.Max(num6, num2);
					float num9 = MathF.Min(num7, num3);
					if (num8 < num9)
					{
						float num10 = (num8 - num6) / length2;
						float num11 = (num9 - num6) / length2;
						this.EvaluateSegmentAgainstReferenceCircle(in asVec, in asVec2, length2, num6, num10, num11, in vec, length, num, ref maxValue, ref maxValue2, ref num5, ref flag, ref flag2);
					}
					num4 = num7;
				}
			}
			float num12 = MathF.Abs(maxValue2);
			if (flag2 && maxValue <= 50f && num12 >= 40f && num12 <= 200f)
			{
				arcLength = maxValue2;
				cost = maxValue + num12 * 0.25f;
				if (flag)
				{
					cost += 5f;
				}
				reinforcementOffset = (isCandidatePathInverted ? (candidatePathPivotOffset - num5) : (num5 - candidatePathPivotOffset));
				return true;
			}
			return false;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0006A0A8 File Offset: 0x000682A8
		private void EvaluateSegmentAgainstReferenceCircle(in Vec2 segmentStart, in Vec2 segmentEnd, float segmentLength, float segmentStartOffset, float localStartT, float localEndT, in Vec2 circleCenter, float circleRadius, float mainBaseAngle, ref float bestDistanceToCircle, ref float bestArcLength, ref float bestOffset, ref bool bestPointIsInsideCircle, ref bool foundCandidate)
		{
			Vec2 vec = segmentEnd - segmentStart;
			float num = Vec2.DotProduct(vec, vec);
			if (num > 0.001f)
			{
				int num2 = 0;
				this._tempCandidatePointOffsetsOnSegment[num2++] = localStartT;
				this._tempCandidatePointOffsetsOnSegment[num2++] = localEndT;
				Vec2 vec2 = circleCenter - segmentStart;
				float num3 = Vec2.DotProduct(vec2, vec) / num;
				if (num3 >= localStartT && num3 <= localEndT)
				{
					this._tempCandidatePointOffsetsOnSegment[num2++] = num3;
				}
				Vec2 vec3 = -vec2;
				float num4 = num;
				float num5 = 2f * Vec2.DotProduct(vec3, vec);
				float num6 = Vec2.DotProduct(vec3, vec3) - circleRadius * circleRadius;
				float num7 = num5 * num5 - 4f * num4 * num6;
				if (num7 >= 0f)
				{
					float num8 = MathF.Sqrt(num7);
					float num9 = 1f / (2f * num4);
					float num10 = (-num5 - num8) * num9;
					float num11 = (-num5 + num8) * num9;
					if (num10 >= localStartT && num10 <= localEndT)
					{
						this._tempCandidatePointOffsetsOnSegment[num2++] = num10;
					}
					if (num11 >= localStartT && num11 <= localEndT)
					{
						this._tempCandidatePointOffsetsOnSegment[num2++] = num11;
					}
				}
				for (int i = 0; i < num2; i++)
				{
					float num12 = this._tempCandidatePointOffsetsOnSegment[i];
					Vec2 vec4 = segmentStart + vec * num12 - circleCenter;
					float length = vec4.Length;
					if (length > 0.001f)
					{
						float num13 = MathF.Abs(length - circleRadius);
						float num14 = MathF.Atan2(vec4.y, vec4.x);
						float num15 = MBMath.GetSmallestDifferenceBetweenTwoAngles(mainBaseAngle, num14) * circleRadius;
						float num16 = MathF.Abs(num15);
						bool flag = length < circleRadius;
						bool flag2 = false;
						if (!foundCandidate)
						{
							flag2 = true;
						}
						else if (num13 < bestDistanceToCircle - 0.001f)
						{
							flag2 = true;
						}
						else if (MathF.Abs(num13 - bestDistanceToCircle) <= 0.001f)
						{
							float num17 = MathF.Abs(bestArcLength);
							if (num16 < num17 - 0.001f)
							{
								flag2 = true;
							}
							else if (MathF.Abs(num16 - num17) <= 0.001f && bestPointIsInsideCircle && !flag)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							foundCandidate = true;
							bestDistanceToCircle = num13;
							bestArcLength = num15;
							bestOffset = segmentStartOffset + num12 * segmentLength;
							bestPointIsInsideCircle = flag;
						}
					}
				}
			}
		}

		// Token: 0x04000A84 RID: 2692
		public const int MaxNeighborCount = 2;

		// Token: 0x04000A85 RID: 2693
		private const int MaxPointsOnPath = 200;

		// Token: 0x04000A86 RID: 2694
		private readonly Mission _mission;

		// Token: 0x04000A87 RID: 2695
		private readonly SpawnPathData _initialSpawnPath;

		// Token: 0x04000A88 RID: 2696
		private readonly SpawnPathData.SnapMethod _pathSnapMethod;

		// Token: 0x04000A89 RID: 2697
		[TupleElementNames(new string[] { "pathData", "startOffset" })]
		private readonly MBList<ValueTuple<SpawnPathData, float>> _reinforcementSpawnPaths;

		// Token: 0x04000A8A RID: 2698
		private readonly MatrixFrame[] _tempPathPoints;

		// Token: 0x04000A8B RID: 2699
		private float[] _tempCandidatePointOffsetsOnSegment = new float[5];

		// Token: 0x02000522 RID: 1314
		private struct ReinforcementPathCandidate
		{
			// Token: 0x06003CBD RID: 15549 RVA: 0x000F4188 File Offset: 0x000F2388
			public ReinforcementPathCandidate(float cost, Path path, float pivotOffset, float reinforcementOffset, bool isInverted)
			{
				this.Cost = cost;
				this.Path = path;
				this.PivotOffset = pivotOffset;
				this.ReinforcementOffset = reinforcementOffset;
				this.IsInverted = isInverted;
			}

			// Token: 0x04001D88 RID: 7560
			public readonly float Cost;

			// Token: 0x04001D89 RID: 7561
			public readonly Path Path;

			// Token: 0x04001D8A RID: 7562
			public readonly float PivotOffset;

			// Token: 0x04001D8B RID: 7563
			public readonly float ReinforcementOffset;

			// Token: 0x04001D8C RID: 7564
			public readonly bool IsInverted;
		}
	}
}
