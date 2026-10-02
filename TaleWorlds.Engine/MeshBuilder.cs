using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006A RID: 106
	public class MeshBuilder
	{
		// Token: 0x060009F5 RID: 2549 RVA: 0x00009C05 File Offset: 0x00007E05
		public MeshBuilder()
		{
			this.vertices = new List<Vec3>();
			this.faceCorners = new List<MeshBuilder.FaceCorner>();
			this.faces = new List<MeshBuilder.Face>();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00009C30 File Offset: 0x00007E30
		public int AddFaceCorner(Vec3 position, Vec3 normal, Vec2 uvCoord, uint color)
		{
			this.vertices.Add(new Vec3(position, -1f));
			MeshBuilder.FaceCorner faceCorner;
			faceCorner.vertexIndex = this.vertices.Count - 1;
			faceCorner.color = color;
			faceCorner.uvCoord = uvCoord;
			faceCorner.normal = normal;
			this.faceCorners.Add(faceCorner);
			return this.faceCorners.Count - 1;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00009C9C File Offset: 0x00007E9C
		public int AddFace(int patchNode0, int patchNode1, int patchNode2)
		{
			MeshBuilder.Face face;
			face.fc0 = patchNode0;
			face.fc1 = patchNode1;
			face.fc2 = patchNode2;
			this.faces.Add(face);
			return this.faces.Count - 1;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00009CDA File Offset: 0x00007EDA
		public void Clear()
		{
			this.vertices.Clear();
			this.faceCorners.Clear();
			this.faces.Clear();
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00009D00 File Offset: 0x00007F00
		public new Mesh Finalize()
		{
			Vec3[] array = this.vertices.ToArray();
			MeshBuilder.FaceCorner[] array2 = this.faceCorners.ToArray();
			MeshBuilder.Face[] array3 = this.faces.ToArray();
			Mesh mesh = EngineApplicationInterface.IMeshBuilder.FinalizeMeshBuilder(this.vertices.Count, array, this.faceCorners.Count, array2, this.faces.Count, array3);
			this.vertices.Clear();
			this.faceCorners.Clear();
			this.faces.Clear();
			return mesh;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00009D80 File Offset: 0x00007F80
		public static Mesh CreateUnitMesh()
		{
			Mesh mesh = Mesh.CreateMeshWithMaterial(Material.GetDefaultMaterial());
			Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
			Vec3 vec2 = new Vec3(1f, -1f, 0f, -1f);
			Vec3 vec3 = new Vec3(1f, 0f, 0f, -1f);
			Vec3 vec4 = new Vec3(0f, 0f, 0f, -1f);
			Vec3 vec5 = new Vec3(0f, 0f, 1f, -1f);
			Vec2 vec6 = new Vec2(0f, 0f);
			Vec2 vec7 = new Vec2(1f, 0f);
			Vec2 vec8 = new Vec2(1f, 1f);
			Vec2 vec9 = new Vec2(0f, 1f);
			UIntPtr uintPtr = mesh.LockEditDataWrite();
			int num = mesh.AddFaceCorner(vec, vec5, vec6, uint.MaxValue, uintPtr);
			int num2 = mesh.AddFaceCorner(vec2, vec5, vec7, uint.MaxValue, uintPtr);
			int num3 = mesh.AddFaceCorner(vec3, vec5, vec8, uint.MaxValue, uintPtr);
			int num4 = mesh.AddFaceCorner(vec4, vec5, vec9, uint.MaxValue, uintPtr);
			mesh.AddFace(num, num2, num3, uintPtr);
			mesh.AddFace(num3, num4, num, uintPtr);
			mesh.UpdateBoundingBox();
			mesh.UnlockEditDataWrite(uintPtr);
			return mesh;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00009ED6 File Offset: 0x000080D6
		public static Mesh CreateTilingWindowMesh(string baseMeshName, Vec2 meshSizeMin, Vec2 meshSizeMax, Vec2 borderThickness, Vec2 bgBorderThickness)
		{
			return EngineApplicationInterface.IMeshBuilder.CreateTilingWindowMesh(baseMeshName, ref meshSizeMin, ref meshSizeMax, ref borderThickness, ref bgBorderThickness);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00009EEB File Offset: 0x000080EB
		public static Mesh CreateTilingButtonMesh(string baseMeshName, Vec2 meshSizeMin, Vec2 meshSizeMax, Vec2 borderThickness)
		{
			return EngineApplicationInterface.IMeshBuilder.CreateTilingButtonMesh(baseMeshName, ref meshSizeMin, ref meshSizeMax, ref borderThickness);
		}

		// Token: 0x04000147 RID: 327
		private List<Vec3> vertices;

		// Token: 0x04000148 RID: 328
		private List<MeshBuilder.FaceCorner> faceCorners;

		// Token: 0x04000149 RID: 329
		private List<MeshBuilder.Face> faces;

		// Token: 0x020000CB RID: 203
		[EngineStruct("rglMeshBuilder_face_corner", false, null)]
		public struct FaceCorner
		{
			// Token: 0x0400042C RID: 1068
			public int vertexIndex;

			// Token: 0x0400042D RID: 1069
			public Vec2 uvCoord;

			// Token: 0x0400042E RID: 1070
			public Vec3 normal;

			// Token: 0x0400042F RID: 1071
			public uint color;
		}

		// Token: 0x020000CC RID: 204
		[EngineStruct("rglMeshBuilder_face", false, null)]
		public struct Face
		{
			// Token: 0x04000430 RID: 1072
			public int fc0;

			// Token: 0x04000431 RID: 1073
			public int fc1;

			// Token: 0x04000432 RID: 1074
			public int fc2;
		}
	}
}
