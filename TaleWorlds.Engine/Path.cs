using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000075 RID: 117
	[EngineClass("rglPath")]
	public sealed class Path : NativeObject
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0000AC0F File Offset: 0x00008E0F
		public int NumberOfPoints
		{
			get
			{
				return EngineApplicationInterface.IPath.GetNumberOfPoints(base.Pointer);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0000AC21 File Offset: 0x00008E21
		public float TotalDistance
		{
			get
			{
				return EngineApplicationInterface.IPath.GetTotalLength(base.Pointer);
			}
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0000AC33 File Offset: 0x00008E33
		internal Path(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0000AC44 File Offset: 0x00008E44
		public MatrixFrame GetHermiteFrameForDt(float phase, int first_point)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetHermiteFrameForDt(base.Pointer, ref identity, phase, first_point);
			return identity;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0000AC6C File Offset: 0x00008E6C
		public MatrixFrame GetFrameForDistance(float distance)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetHermiteFrameForDistance(base.Pointer, ref identity, distance);
			return identity;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0000AC94 File Offset: 0x00008E94
		public MatrixFrame GetNearestFrameWithValidAlphaForDistance(float distance, bool searchForward = true, float alphaThreshold = 0.5f)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetNearestHermiteFrameWithValidAlphaForDistance(base.Pointer, ref identity, distance, searchForward, alphaThreshold);
			return identity;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0000ACBD File Offset: 0x00008EBD
		public void GetFrameAndColorForDistance(float distance, out MatrixFrame frame, out Vec3 color)
		{
			frame = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetHermiteFrameAndColorForDistance(base.Pointer, out frame, out color, distance);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0000ACDD File Offset: 0x00008EDD
		public float GetArcLength(int first_point)
		{
			return EngineApplicationInterface.IPath.GetArcLength(base.Pointer, first_point);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0000ACF0 File Offset: 0x00008EF0
		public void GetPoints(MatrixFrame[] points)
		{
			EngineApplicationInterface.IPath.GetPoints(base.Pointer, points);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0000AD03 File Offset: 0x00008F03
		public float GetTotalLength()
		{
			return EngineApplicationInterface.IPath.GetTotalLength(base.Pointer);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0000AD15 File Offset: 0x00008F15
		public int GetVersion()
		{
			return EngineApplicationInterface.IPath.GetVersion(base.Pointer);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0000AD27 File Offset: 0x00008F27
		public void SetFrameOfPoint(int pointIndex, ref MatrixFrame frame)
		{
			EngineApplicationInterface.IPath.SetFrameOfPoint(base.Pointer, pointIndex, ref frame);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0000AD3B File Offset: 0x00008F3B
		public void SetTangentPositionOfPoint(int pointIndex, int tangentIndex, ref Vec3 position)
		{
			EngineApplicationInterface.IPath.SetTangentPositionOfPoint(base.Pointer, pointIndex, tangentIndex, ref position);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0000AD50 File Offset: 0x00008F50
		public int AddPathPoint(int newNodeIndex)
		{
			return EngineApplicationInterface.IPath.AddPathPoint(base.Pointer, newNodeIndex);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0000AD63 File Offset: 0x00008F63
		public void DeletePathPoint(int nodeIndex)
		{
			EngineApplicationInterface.IPath.DeletePathPoint(base.Pointer, nodeIndex);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0000AD76 File Offset: 0x00008F76
		public bool HasValidAlphaAtPathPoint(int nodeIndex, float alphaThreshold = 0.5f)
		{
			return EngineApplicationInterface.IPath.HasValidAlphaAtPathPoint(base.Pointer, nodeIndex, alphaThreshold);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0000AD8A File Offset: 0x00008F8A
		public string GetName()
		{
			return EngineApplicationInterface.IPath.GetName(base.Pointer);
		}
	}
}
