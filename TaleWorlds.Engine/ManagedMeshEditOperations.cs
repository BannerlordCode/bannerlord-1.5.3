using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005B RID: 91
	[EngineClass("rglManaged_mesh_edit_operations")]
	public sealed class ManagedMeshEditOperations : NativeObject
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x00007C80 File Offset: 0x00005E80
		internal ManagedMeshEditOperations(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00007C8F File Offset: 0x00005E8F
		public static ManagedMeshEditOperations Create(Mesh meshToEdit)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.Create(meshToEdit.Pointer);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00007CA1 File Offset: 0x00005EA1
		public void Weld()
		{
			EngineApplicationInterface.IManagedMeshEditOperations.Weld(base.Pointer);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00007CB3 File Offset: 0x00005EB3
		public int AddVertex(Vec3 vertexPos)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.AddVertex(base.Pointer, ref vertexPos);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00007CC7 File Offset: 0x00005EC7
		public int AddFaceCorner(int vertexIndex, Vec2 uv0, Vec3 color, Vec3 normal)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.AddFaceCorner1(base.Pointer, vertexIndex, ref uv0, ref color, ref normal);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00007CE0 File Offset: 0x00005EE0
		public int AddFaceCorner(int vertexIndex, Vec2 uv0, Vec2 uv1, Vec3 color, Vec3 normal)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.AddFaceCorner2(base.Pointer, vertexIndex, ref uv0, ref uv1, ref color, ref normal);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00007CFB File Offset: 0x00005EFB
		public int AddFace(int patchNode0, int patchNode1, int patchNode2)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.AddFace(base.Pointer, patchNode0, patchNode1, patchNode2);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00007D10 File Offset: 0x00005F10
		public void AddTriangle(Vec3 p1, Vec3 p2, Vec3 p3, Vec2 uv1, Vec2 uv2, Vec2 uv3, Vec3 color)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddTriangle1(base.Pointer, ref p1, ref p2, ref p3, ref uv1, ref uv2, ref uv3, ref color);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00007D3C File Offset: 0x00005F3C
		public void AddTriangle(Vec3 p1, Vec3 p2, Vec3 p3, Vec3 n1, Vec3 n2, Vec3 n3, Vec2 uv1, Vec2 uv2, Vec2 uv3, Vec3 c1, Vec3 c2, Vec3 c3)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddTriangle2(base.Pointer, ref p1, ref p2, ref p3, ref n1, ref n2, ref n3, ref uv1, ref uv2, ref uv3, ref c1, ref c2, ref c3);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00007D71 File Offset: 0x00005F71
		public void AddRectangle3(Vec3 o, Vec2 size, Vec2 uv_origin, Vec2 uvSize, Vec3 color)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddRectangle3(base.Pointer, ref o, ref size, ref uv_origin, ref uvSize, ref color);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00007D8D File Offset: 0x00005F8D
		public void AddRectangleWithInverseUV(Vec3 o, Vec2 size, Vec2 uv_origin, Vec2 uvSize, Vec3 color)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddRectangleWithInverseUV(base.Pointer, ref o, ref size, ref uv_origin, ref uvSize, ref color);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00007DA9 File Offset: 0x00005FA9
		public void AddRect(Vec3 originBegin, Vec3 originEnd, Vec2 uvBegin, Vec2 uvEnd, Vec3 color)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddRect(base.Pointer, ref originBegin, ref originEnd, ref uvBegin, ref uvEnd, ref color);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00007DC5 File Offset: 0x00005FC5
		public void AddRectWithZUp(Vec3 originBegin, Vec3 originEnd, Vec2 uvBegin, Vec2 uvEnd, Vec3 color)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddRectWithZUp(base.Pointer, ref originBegin, ref originEnd, ref uvBegin, ref uvEnd, ref color);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00007DE1 File Offset: 0x00005FE1
		public void InvertFacesWindingOrder()
		{
			EngineApplicationInterface.IManagedMeshEditOperations.InvertFacesWindingOrder(base.Pointer);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00007DF3 File Offset: 0x00005FF3
		public void ScaleVertices(float newScale)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ScaleVertices1(base.Pointer, newScale);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00007E06 File Offset: 0x00006006
		public void MoveVerticesAlongNormal(float moveAmount)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.MoveVerticesAlongNormal(base.Pointer, moveAmount);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00007E19 File Offset: 0x00006019
		public void ScaleVertices(Vec3 newScale, bool keepUvX = false, float maxUvSize = 1f)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ScaleVertices2(base.Pointer, ref newScale, keepUvX, maxUvSize);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00007E2F File Offset: 0x0000602F
		public void TranslateVertices(Vec3 newOrigin)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.TranslateVertices(base.Pointer, ref newOrigin);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00007E44 File Offset: 0x00006044
		public void AddMeshAux(Mesh mesh, MatrixFrame frame, sbyte boneNo, Vec3 color, bool transformNormal, bool heightGradient, bool addSkinData, bool useDoublePrecision = true)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMeshAux(base.Pointer, mesh.Pointer, ref frame, boneNo, ref color, transformNormal, heightGradient, addSkinData, useDoublePrecision);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00007E74 File Offset: 0x00006074
		public int ComputeTangents(bool checkFixedNormals)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.ComputeTangents(base.Pointer, checkFixedNormals);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00007E87 File Offset: 0x00006087
		public void GenerateGrid(Vec2i numEdges, Vec2 edgeScale)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.GenerateGrid(base.Pointer, ref numEdges, ref edgeScale);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00007E9D File Offset: 0x0000609D
		public void RescaleMesh2d(Vec2 scaleSizeMin, Vec2 scaleSizeMax)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RescaleMesh2d(base.Pointer, ref scaleSizeMin, ref scaleSizeMax);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00007EB3 File Offset: 0x000060B3
		public void RescaleMesh2dRepeatX(Vec2 scaleSizeMin, Vec2 scaleSizeMax, float frameThickness = 0f, int frameSide = 0)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RescaleMesh2dRepeatX(base.Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00007ECC File Offset: 0x000060CC
		public void RescaleMesh2dRepeatY(Vec2 scaleSizeMin, Vec2 scaleSizeMax, float frameThickness = 0f, int frameSide = 0)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RescaleMesh2dRepeatY(base.Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00007EE5 File Offset: 0x000060E5
		public void RescaleMesh2dRepeatXWithTiling(Vec2 scaleSizeMin, Vec2 scaleSizeMax, float frameThickness = 0f, int frameSide = 0, float xyRatio = 0f)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RescaleMesh2dRepeatXWithTiling(base.Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide, xyRatio);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00007F00 File Offset: 0x00006100
		public void RescaleMesh2dRepeatYWithTiling(Vec2 scaleSizeMin, Vec2 scaleSizeMax, float frameThickness = 0f, int frameSide = 0, float xyRatio = 0f)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RescaleMesh2dRepeatYWithTiling(base.Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide, xyRatio);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00007F1B File Offset: 0x0000611B
		public void RescaleMesh2dWithoutChangingUV(Vec2 scaleSizeMin, Vec2 scaleSizeMax, float remaining)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RescaleMesh2dRepeatYWithTiling(base.Pointer, ref scaleSizeMin, ref scaleSizeMax, remaining, 0, 0f);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00007F38 File Offset: 0x00006138
		public void AddLine(Vec3 start, Vec3 end, Vec3 color, float lineWidth = 0.004f)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddLine(base.Pointer, ref start, ref end, ref color, lineWidth);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00007F52 File Offset: 0x00006152
		public void ComputeCornerNormals(bool checkFixedNormals = false, bool smoothCornerNormals = true)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ComputeCornerNormals(base.Pointer, checkFixedNormals, smoothCornerNormals);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00007F66 File Offset: 0x00006166
		public void ComputeCornerNormalsWithSmoothingData()
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ComputeCornerNormalsWithSmoothingData(base.Pointer);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00007F78 File Offset: 0x00006178
		public void AddMesh(Mesh mesh, MatrixFrame frame)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMesh(base.Pointer, mesh.Pointer, ref frame);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00007F92 File Offset: 0x00006192
		public void AddMeshWithSkinData(Mesh mesh, MatrixFrame frame, sbyte boneIndex)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMeshWithSkinData(base.Pointer, mesh.Pointer, ref frame, boneIndex);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00007FAD File Offset: 0x000061AD
		public void AddMeshWithColor(Mesh mesh, MatrixFrame frame, Vec3 vertexColor, bool useDoublePrecision = true)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMeshWithColor(base.Pointer, mesh.Pointer, ref frame, ref vertexColor, useDoublePrecision);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00007FCB File Offset: 0x000061CB
		public void AddMeshToBone(Mesh mesh, MatrixFrame frame, sbyte boneIndex)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMeshToBone(base.Pointer, mesh.Pointer, ref frame, boneIndex);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00007FE6 File Offset: 0x000061E6
		public void AddMeshWithFixedNormals(Mesh mesh, MatrixFrame frame)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMeshWithFixedNormals(base.Pointer, mesh.Pointer, ref frame);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00008000 File Offset: 0x00006200
		public void AddMeshWithFixedNormalsWithHeightGradientColor(Mesh mesh, MatrixFrame frame)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddMeshWithFixedNormalsWithHeightGradientColor(base.Pointer, mesh.Pointer, ref frame);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000801A File Offset: 0x0000621A
		public void AddSkinnedMeshWithColor(Mesh mesh, MatrixFrame frame, Vec3 vertexColor, bool useDoublePrecision = true)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.AddSkinnedMeshWithColor(base.Pointer, mesh.Pointer, ref frame, ref vertexColor, useDoublePrecision);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00008038 File Offset: 0x00006238
		public void SetCornerVertexColor(int cornerNo, Vec3 vertexColor)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.SetCornerVertexColor(base.Pointer, cornerNo, ref vertexColor);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0000804D File Offset: 0x0000624D
		public void SetCornerUV(int cornerNo, Vec2 newUV, int uvNumber = 0)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.SetCornerUV(base.Pointer, cornerNo, ref newUV, uvNumber);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00008063 File Offset: 0x00006263
		public void ReserveVertices(int count)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ReserveVertices(base.Pointer, count);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00008076 File Offset: 0x00006276
		public void ReserveFaceCorners(int count)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ReserveFaceCorners(base.Pointer, count);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00008089 File Offset: 0x00006289
		public void ReserveFaces(int count)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ReserveFaces(base.Pointer, count);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0000809C File Offset: 0x0000629C
		public int RemoveDuplicatedCorners()
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.RemoveDuplicatedCorners(base.Pointer);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000080AE File Offset: 0x000062AE
		public void TransformVerticesToParent(MatrixFrame frame)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.TransformVerticesToParent(base.Pointer, ref frame);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x000080C2 File Offset: 0x000062C2
		public void TransformVerticesToLocal(MatrixFrame frame)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.TransformVerticesToLocal(base.Pointer, ref frame);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000080D6 File Offset: 0x000062D6
		public void SetVertexColor(Vec3 color)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.SetVertexColor(base.Pointer, ref color);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x000080EA File Offset: 0x000062EA
		public Vec3 GetVertexColor(int faceCornerIndex)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.GetVertexColor(base.Pointer, faceCornerIndex);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000080FD File Offset: 0x000062FD
		public void SetVertexColorAlpha(float newAlpha)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.SetVertexColorAlpha(base.Pointer, newAlpha);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00008110 File Offset: 0x00006310
		public float GetVertexColorAlpha()
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.GetVertexColorAlpha(base.Pointer);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00008122 File Offset: 0x00006322
		public void EnsureTransformedVertices()
		{
			EngineApplicationInterface.IManagedMeshEditOperations.EnsureTransformedVertices(base.Pointer);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00008134 File Offset: 0x00006334
		public void ApplyCPUSkinning(Skeleton skeleton)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ApplyCPUSkinning(base.Pointer, skeleton.Pointer);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0000814C File Offset: 0x0000634C
		public void UpdateOverlappedVertexNormals(Mesh attachedToMesh, MatrixFrame attachFrame, float mergeRadiusSQ = 0.0025f)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.UpdateOverlappedVertexNormals(base.Pointer, attachedToMesh.Pointer, ref attachFrame, mergeRadiusSQ);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00008167 File Offset: 0x00006367
		public void ClearAll()
		{
			EngineApplicationInterface.IManagedMeshEditOperations.ClearAll(base.Pointer);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00008179 File Offset: 0x00006379
		public void SetTangentsOfFaceCorner(int faceCornerIndex, Vec3 tangent, Vec3 binormal)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.SetTangentsOfFaceCorner(base.Pointer, faceCornerIndex, ref tangent, ref binormal);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00008190 File Offset: 0x00006390
		public void SetPositionOfVertex(int vertexIndex, Vec3 position)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.SetPositionOfVertex(base.Pointer, vertexIndex, ref position);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x000081A5 File Offset: 0x000063A5
		public Vec3 GetPositionOfVertex(int vertexIndex)
		{
			return EngineApplicationInterface.IManagedMeshEditOperations.GetPositionOfVertex(base.Pointer, vertexIndex);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x000081B8 File Offset: 0x000063B8
		public void RemoveFace(int faceIndex)
		{
			EngineApplicationInterface.IManagedMeshEditOperations.RemoveFace(base.Pointer, faceIndex);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000081CB File Offset: 0x000063CB
		public void FinalizeEditing()
		{
			EngineApplicationInterface.IManagedMeshEditOperations.FinalizeEditing(base.Pointer);
		}
	}
}
