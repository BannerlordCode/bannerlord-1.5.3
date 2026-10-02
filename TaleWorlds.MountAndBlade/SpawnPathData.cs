using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021F RID: 543
	public class SpawnPathData
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x0006D7DD File Offset: 0x0006B9DD
		public bool IsValid
		{
			get
			{
				return this.Scene != null && this.Path != null && this.Path.NumberOfPoints > 1;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x0006D80B File Offset: 0x0006BA0B
		public int FreeSegmentCount
		{
			get
			{
				return this._freeSegments.Count;
			}
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0006D818 File Offset: 0x0006BA18
		public SpawnPathData Invert()
		{
			float num = this.PathLength - this.PivotOffset;
			return new SpawnPathData(this.Scene, this.Path, MathF.Max(num, 0f), !this.IsInverted, this.SnapType);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0006D860 File Offset: 0x0006BA60
		public void ClampPathOffset(ref float relativePathOffset)
		{
			float num = MathF.Clamp(this.PivotOffset + relativePathOffset, 0f, this.PathLength) - this.PivotOffset;
			relativePathOffset = num;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0006D894 File Offset: 0x0006BA94
		public float ConvertPointToRelativePathOffset(int pointIndex)
		{
			float num = 0f;
			for (int i = 0; i < pointIndex; i++)
			{
				num += this.Path.GetArcLength(i);
			}
			num = MathF.Clamp(num, 0f, this.PathLength);
			num = (this.IsInverted ? (this.PathLength - num) : num);
			return num - this.PivotOffset;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0006D8F0 File Offset: 0x0006BAF0
		public float ConvertRelativePathOffsetToPathDistance(float relativePathOffset)
		{
			float num = MathF.Clamp(this.PivotOffset + relativePathOffset, 0f, this.PathLength);
			return this.IsInverted ? (this.PathLength - num) : num;
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0006D92C File Offset: 0x0006BB2C
		public int GetNodeIndexAtPathDistance(float pathDistance)
		{
			int num = this.Path.NumberOfPoints - 2;
			float num2 = 0f;
			for (int i = 0; i < this.Path.NumberOfPoints - 1; i++)
			{
				float arcLength = this.Path.GetArcLength(i);
				if (pathDistance >= num2 && pathDistance < num2 + arcLength)
				{
					num = i;
					break;
				}
				num2 += arcLength;
			}
			return num;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0006D985 File Offset: 0x0006BB85
		public float GetBaseOffset()
		{
			if (this.IsInverted)
			{
				return this.PivotOffset - this.PathLength + 1f;
			}
			return -this.PivotOffset + 1f;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0006D9B0 File Offset: 0x0006BBB0
		public bool IsPathOffsetValid(float relativePathOffset)
		{
			float num = this.ConvertRelativePathOffsetToPathDistance(relativePathOffset);
			int nodeIndexAtPathDistance = this.GetNodeIndexAtPathDistance(num);
			int num2 = nodeIndexAtPathDistance + 1;
			return this.Path.HasValidAlphaAtPathPoint(nodeIndexAtPathDistance, 0.5f) || this.Path.HasValidAlphaAtPathPoint(num2, 0.5f);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0006D9F8 File Offset: 0x0006BBF8
		public float GetOffsetOverflow(float relativePathOffset)
		{
			float num = this.PivotOffset + relativePathOffset;
			if (num < 0f)
			{
				return num;
			}
			if (num > this.PathLength)
			{
				return num - this.PathLength;
			}
			return 0f;
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0006DA30 File Offset: 0x0006BC30
		public MatrixFrame GetSpawnFrame(float relativePathOffset, bool searchNearestValidFrame = false, SpawnPathData.SearchDirection searchDirection = SpawnPathData.SearchDirection.Backward)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			float num = this.ConvertRelativePathOffsetToPathDistance(relativePathOffset);
			if (searchNearestValidFrame)
			{
				bool flag = searchDirection == SpawnPathData.SearchDirection.Forward;
				flag = (this.IsInverted ? (!flag) : flag);
				matrixFrame = this.Path.GetNearestFrameWithValidAlphaForDistance(num, flag, 0.5f);
			}
			else
			{
				matrixFrame = this.Path.GetFrameForDistance(num);
			}
			matrixFrame.rotation.f = (this.IsInverted ? (-matrixFrame.rotation.f) : matrixFrame.rotation.f);
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			if (this.SnapType != SpawnPathData.SnapMethod.DontSnap)
			{
				if (this.SnapType == SpawnPathData.SnapMethod.SnapToTerrain)
				{
					matrixFrame.origin.z = this.Scene.GetTerrainHeight(matrixFrame.origin.AsVec2, true);
				}
				else if (this.SnapType == SpawnPathData.SnapMethod.SnapToWaterLevel)
				{
					matrixFrame.origin.z = this.Scene.GetWaterLevel();
				}
			}
			return matrixFrame;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0006DB20 File Offset: 0x0006BD20
		public MatrixFrame GetCenterFrame()
		{
			float num = this.PathLength * 0.5f;
			return this.Path.GetFrameForDistance(num);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0006DB48 File Offset: 0x0006BD48
		public void GetSpawnPathFrameFacingTarget(float basePathOffset, float targetPathOffset, bool useTangentDirection, out Vec2 spawnPathPosition, out Vec2 spawnPathDirection, bool decideDirectionDynamically = false, float dynamicDistancePercentage = 0.2f)
		{
			this.ClampPathOffset(ref basePathOffset);
			this.ClampPathOffset(ref targetPathOffset);
			MatrixFrame spawnFrame = this.GetSpawnFrame(basePathOffset, false, SpawnPathData.SearchDirection.Backward);
			float num = 0.01f;
			spawnPathPosition = spawnFrame.origin.AsVec2;
			if (MBMath.ApproximatelyEquals(basePathOffset, targetPathOffset, num))
			{
				if (MBMath.ApproximatelyEquals(basePathOffset, 0f, num))
				{
					spawnPathDirection = this.GetSpawnFrame(0f, false, SpawnPathData.SearchDirection.Backward).rotation.f.AsVec2.Normalized();
					return;
				}
				if (useTangentDirection)
				{
					spawnPathDirection = this.GetSpawnPathTangentDirection(basePathOffset, in spawnFrame, 0f);
					return;
				}
				spawnPathDirection = (this.GetSpawnFrame(0f, false, SpawnPathData.SearchDirection.Backward).origin.AsVec2 - spawnPathPosition).Normalized();
				return;
			}
			else
			{
				MatrixFrame spawnFrame2 = this.GetSpawnFrame(targetPathOffset, false, SpawnPathData.SearchDirection.Backward);
				if (decideDirectionDynamically)
				{
					WorldPosition worldPosition = new WorldPosition(this.Scene, spawnFrame.origin);
					WorldPosition worldPosition2 = new WorldPosition(this.Scene, spawnFrame2.origin);
					float num2;
					if (this.Scene.GetPathDistanceBetweenPositions(ref worldPosition, ref worldPosition2, 0.1f, out num2))
					{
						float length = (spawnFrame2.origin - spawnFrame.origin).Length;
						useTangentDirection = num2 >= length * (1f + dynamicDistancePercentage);
					}
				}
				if (useTangentDirection)
				{
					spawnPathDirection = this.GetSpawnPathTangentDirection(basePathOffset, in spawnFrame, targetPathOffset);
					return;
				}
				spawnPathDirection = (spawnFrame2.origin.AsVec2 - spawnPathPosition).Normalized();
				return;
			}
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0006DCE0 File Offset: 0x0006BEE0
		private Vec2 GetSpawnPathTangentDirection(float onPathOffset, in MatrixFrame onPathFrame, float referenceOffset)
		{
			Vec3 vec = onPathFrame.origin;
			Vec2 asVec = vec.AsVec2;
			if (MBMath.ApproximatelyEquals(onPathOffset, referenceOffset, 1E-05f))
			{
				vec = onPathFrame.rotation.f;
				return vec.AsVec2.Normalized();
			}
			int num = ((onPathOffset > referenceOffset) ? (-1) : 1);
			float num2 = onPathOffset + (float)num * 1f;
			this.ClampPathOffset(ref num2);
			Vec2 vec2 = this.GetSpawnFrame(num2, false, SpawnPathData.SearchDirection.Backward).origin.AsVec2 - asVec;
			Vec2 vec3;
			if (vec2.LengthSquared < 1E-06f)
			{
				float num3 = (float)num;
				vec = onPathFrame.rotation.f;
				vec3 = num3 * vec.AsVec2.Normalized();
			}
			else
			{
				vec3 = vec2.Normalized();
			}
			return vec3;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0006DDA4 File Offset: 0x0006BFA4
		private SpawnPathData(Scene scene, Path path, float pivotOffset, bool isInverted = false, SpawnPathData.SnapMethod snapType = SpawnPathData.SnapMethod.DontSnap)
		{
			this.Scene = scene;
			this.Path = path;
			this.PathLength = this.Path.GetTotalLength();
			this.PivotOffset = MathF.Clamp(pivotOffset, 1f, this.PathLength - 1f);
			this.IsInverted = isInverted;
			this.SnapType = snapType;
			this.BuildFreeSegments();
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0006DE14 File Offset: 0x0006C014
		private void BuildFreeSegments()
		{
			if (this.IsValid)
			{
				int numberOfPoints = this.Path.NumberOfPoints;
				float[] array = new float[numberOfPoints];
				for (int i = 0; i < numberOfPoints; i++)
				{
					array[i] = this.ConvertPointToRelativePathOffset(i);
				}
				bool flag = false;
				float num = 0f;
				float num2 = 0f;
				int num3 = 0;
				int num4 = numberOfPoints - 1;
				int num5 = 1;
				if (this.IsInverted)
				{
					num3 = numberOfPoints - 1;
					num4 = 0;
					num5 = -1;
				}
				for (int num6 = num3; num6 != num4; num6 += num5)
				{
					int num7 = num6 + num5;
					bool flag2 = this.Path.HasValidAlphaAtPathPoint(num6, 0.5f) || this.Path.HasValidAlphaAtPathPoint(num7, 0.5f);
					float num8 = array[num6];
					float num9 = array[num7];
					if (!flag2)
					{
						if (flag)
						{
							float num10 = num + 0.001f;
							float num11 = num2 - 0.001f;
							if (num11 > num10)
							{
								this.ClampPathOffset(ref num10);
								this.ClampPathOffset(ref num11);
								this._freeSegments.Add(new ValueTuple<float, float>(num10, num11));
							}
							flag = false;
						}
					}
					else if (!flag)
					{
						num = num8;
						num2 = num9;
						flag = true;
					}
					else
					{
						num2 = num9;
					}
				}
				if (flag)
				{
					float num12 = num + 0.001f;
					float num13 = num2 - 0.001f;
					if (num13 > num12)
					{
						this.ClampPathOffset(ref num12);
						this.ClampPathOffset(ref num13);
						if (num13 > num12)
						{
							this._freeSegments.Add(new ValueTuple<float, float>(num12, num13));
						}
					}
				}
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0006DF7B File Offset: 0x0006C17B
		public static SpawnPathData Create(Scene scene, Path path, float pivotOffset, bool isInverted = false, SpawnPathData.SnapMethod snapType = SpawnPathData.SnapMethod.DontSnap)
		{
			return new SpawnPathData(scene, path, pivotOffset, isInverted, snapType);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0006DF88 File Offset: 0x0006C188
		[return: TupleElementNames(new string[] { "startOffset", "endOffset" })]
		internal ValueTuple<float, float> GetFreeSegment(int segmentIndex)
		{
			return this._freeSegments[segmentIndex];
		}

		// Token: 0x04000ACA RID: 2762
		public const float MinimumSpawnPathOffset = 1f;

		// Token: 0x04000ACB RID: 2763
		public readonly Scene Scene;

		// Token: 0x04000ACC RID: 2764
		public readonly Path Path;

		// Token: 0x04000ACD RID: 2765
		public readonly bool IsInverted;

		// Token: 0x04000ACE RID: 2766
		public readonly float PivotOffset;

		// Token: 0x04000ACF RID: 2767
		public readonly float PathLength;

		// Token: 0x04000AD0 RID: 2768
		public readonly SpawnPathData.SnapMethod SnapType;

		// Token: 0x04000AD1 RID: 2769
		[TupleElementNames(new string[] { "startOffset", "endOffset" })]
		private readonly MBList<ValueTuple<float, float>> _freeSegments = new MBList<ValueTuple<float, float>>();

		// Token: 0x0200052A RID: 1322
		public enum SnapMethod
		{
			// Token: 0x04001D97 RID: 7575
			DontSnap,
			// Token: 0x04001D98 RID: 7576
			SnapToTerrain,
			// Token: 0x04001D99 RID: 7577
			SnapToWaterLevel
		}

		// Token: 0x0200052B RID: 1323
		public enum SearchDirection
		{
			// Token: 0x04001D9B RID: 7579
			Forward,
			// Token: 0x04001D9C RID: 7580
			Backward
		}
	}
}
