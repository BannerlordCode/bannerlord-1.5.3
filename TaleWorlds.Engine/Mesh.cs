using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000069 RID: 105
	[EngineClass("rglMesh")]
	public sealed class Mesh : Resource
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x0000956A File Offset: 0x0000776A
		internal Mesh(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00009573 File Offset: 0x00007773
		public static Mesh CreateMeshWithMaterial(Material material)
		{
			return EngineApplicationInterface.IMesh.CreateMeshWithMaterial(material.Pointer);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00009585 File Offset: 0x00007785
		public static Mesh CreateMesh(bool editable = true)
		{
			return EngineApplicationInterface.IMesh.CreateMesh(editable);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00009592 File Offset: 0x00007792
		public Mesh GetBaseMesh()
		{
			return EngineApplicationInterface.IMesh.GetBaseMesh(base.Pointer);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000095A4 File Offset: 0x000077A4
		public static Mesh GetFromResource(string meshName)
		{
			return EngineApplicationInterface.IMesh.GetMeshFromResource(meshName);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000095B1 File Offset: 0x000077B1
		public static Mesh GetRandomMeshWithVdecl(int inputLayout)
		{
			return EngineApplicationInterface.IMesh.GetRandomMeshWithVdecl(inputLayout);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x000095BE File Offset: 0x000077BE
		public void SetColorAndStroke(uint color, uint strokeColor, bool drawStroke)
		{
			this.Color = color;
			this.Color2 = strokeColor;
			EngineApplicationInterface.IMesh.SetColorAndStroke(base.Pointer, drawStroke);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x000095DF File Offset: 0x000077DF
		public void SetMeshRenderOrder(int renderOrder)
		{
			EngineApplicationInterface.IMesh.SetMeshRenderOrder(base.Pointer, renderOrder);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x000095F2 File Offset: 0x000077F2
		public bool HasTag(string str)
		{
			return EngineApplicationInterface.IMesh.HasTag(base.Pointer, str);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00009605 File Offset: 0x00007805
		public Mesh CreateCopy()
		{
			return EngineApplicationInterface.IMesh.CreateMeshCopy(base.Pointer);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00009617 File Offset: 0x00007817
		public void SetMaterial(string newMaterialName)
		{
			EngineApplicationInterface.IMesh.SetMaterialByName(base.Pointer, newMaterialName);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000962A File Offset: 0x0000782A
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IMesh.SetVectorArgument(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00009641 File Offset: 0x00007841
		public void SetVectorArgument2(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IMesh.SetVectorArgument2(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00009658 File Offset: 0x00007858
		public Vec3 GetVectorArgument()
		{
			return EngineApplicationInterface.IMesh.GetVectorArgument(base.Pointer);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0000966A File Offset: 0x0000786A
		public Vec3 GetVectorArgument2()
		{
			return EngineApplicationInterface.IMesh.GetVectorArgument2(base.Pointer);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0000967C File Offset: 0x0000787C
		public void SetupAdditionalBoneBuffer(int numBones)
		{
			EngineApplicationInterface.IMesh.SetupAdditionalBoneBuffer(base.Pointer, numBones);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0000968F File Offset: 0x0000788F
		public void SetAdditionalBoneFrame(int boneIndex, in MatrixFrame frame)
		{
			EngineApplicationInterface.IMesh.SetAdditionalBoneFrame(base.Pointer, boneIndex, in frame);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000096A3 File Offset: 0x000078A3
		public void SetMaterial(Material material)
		{
			EngineApplicationInterface.IMesh.SetMaterial(base.Pointer, material.Pointer);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000096BB File Offset: 0x000078BB
		public Material GetMaterial()
		{
			return EngineApplicationInterface.IMesh.GetMaterial(base.Pointer);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000096CD File Offset: 0x000078CD
		public Material GetSecondMaterial()
		{
			return EngineApplicationInterface.IMesh.GetSecondMaterial(base.Pointer);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000096DF File Offset: 0x000078DF
		public int AddFaceCorner(Vec3 position, Vec3 normal, Vec2 uvCoord, uint color, UIntPtr lockHandle)
		{
			if (base.IsValid)
			{
				return EngineApplicationInterface.IMesh.AddFaceCorner(base.Pointer, position, normal, uvCoord, color, lockHandle);
			}
			return -1;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00009702 File Offset: 0x00007902
		public int AddFace(int patchNode0, int patchNode1, int patchNode2, UIntPtr lockHandle)
		{
			if (base.IsValid)
			{
				return EngineApplicationInterface.IMesh.AddFace(base.Pointer, patchNode0, patchNode1, patchNode2, lockHandle);
			}
			return -1;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00009723 File Offset: 0x00007923
		public void ClearMesh()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.ClearMesh(base.Pointer);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0000973D File Offset: 0x0000793D
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0000975D File Offset: 0x0000795D
		public string Name
		{
			get
			{
				if (base.IsValid)
				{
					return EngineApplicationInterface.IMesh.GetName(base.Pointer);
				}
				return string.Empty;
			}
			set
			{
				EngineApplicationInterface.IMesh.SetName(base.Pointer, value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00009770 File Offset: 0x00007970
		public MBMeshCullingMode CullingMode
		{
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetCullingMode(base.Pointer, (uint)value);
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0000978B File Offset: 0x0000798B
		public float MorphTime
		{
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetMorphTime(base.Pointer, value);
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x000097DB File Offset: 0x000079DB
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x000097A6 File Offset: 0x000079A6
		public uint Color
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetColor(base.Pointer);
			}
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetColor(base.Pointer, value);
					return;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Mesh.cs", "Color", 331);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x00009822 File Offset: 0x00007A22
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x000097ED File Offset: 0x000079ED
		public uint Color2
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetColor2(base.Pointer);
			}
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetColor2(base.Pointer, value);
					return;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Mesh.cs", "Color2", 354);
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00009834 File Offset: 0x00007A34
		public void SetColorAlpha(uint newAlpha)
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.SetColorAlpha(base.Pointer, newAlpha);
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000984F File Offset: 0x00007A4F
		public uint GetFaceCount()
		{
			if (!base.IsValid)
			{
				return 0U;
			}
			return EngineApplicationInterface.IMesh.GetFaceCount(base.Pointer);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0000986B File Offset: 0x00007A6B
		public uint GetFaceCornerCount()
		{
			if (!base.IsValid)
			{
				return 0U;
			}
			return EngineApplicationInterface.IMesh.GetFaceCornerCount(base.Pointer);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00009887 File Offset: 0x00007A87
		public void ComputeNormals()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.ComputeNormals(base.Pointer);
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000098A1 File Offset: 0x00007AA1
		public void ComputeTangents()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.ComputeTangents(base.Pointer);
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000098BC File Offset: 0x00007ABC
		public void AddMesh(string meshResourceName, MatrixFrame meshFrame)
		{
			if (base.IsValid)
			{
				Mesh fromResource = Mesh.GetFromResource(meshResourceName);
				EngineApplicationInterface.IMesh.AddMeshToMesh(base.Pointer, fromResource.Pointer, ref meshFrame);
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000098F0 File Offset: 0x00007AF0
		public void AddMesh(Mesh mesh, MatrixFrame meshFrame)
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.AddMeshToMesh(base.Pointer, mesh.Pointer, ref meshFrame);
			}
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00009914 File Offset: 0x00007B14
		public MatrixFrame GetLocalFrame()
		{
			if (base.IsValid)
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				EngineApplicationInterface.IMesh.GetLocalFrame(base.Pointer, ref matrixFrame);
				return matrixFrame;
			}
			return default(MatrixFrame);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0000994E File Offset: 0x00007B4E
		public void SetLocalFrame(MatrixFrame meshFrame)
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.SetLocalFrame(base.Pointer, ref meshFrame);
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0000996A File Offset: 0x00007B6A
		public void SetVisibilityMask(VisibilityMaskFlags visibilityMask)
		{
			EngineApplicationInterface.IMesh.SetVisibilityMask(base.Pointer, visibilityMask);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0000997D File Offset: 0x00007B7D
		public void UpdateBoundingBox()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.UpdateBoundingBox(base.Pointer);
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00009997 File Offset: 0x00007B97
		public void SetAsNotEffectedBySeason()
		{
			EngineApplicationInterface.IMesh.SetAsNotEffectedBySeason(base.Pointer);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000099A9 File Offset: 0x00007BA9
		public float GetBoundingBoxWidth()
		{
			if (!base.IsValid)
			{
				return 0f;
			}
			return EngineApplicationInterface.IMesh.GetBoundingBoxWidth(base.Pointer);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000099C9 File Offset: 0x00007BC9
		public float GetBoundingBoxHeight()
		{
			if (!base.IsValid)
			{
				return 0f;
			}
			return EngineApplicationInterface.IMesh.GetBoundingBoxHeight(base.Pointer);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x000099E9 File Offset: 0x00007BE9
		public Vec3 GetBoundingBoxMin()
		{
			return EngineApplicationInterface.IMesh.GetBoundingBoxMin(base.Pointer);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x000099FB File Offset: 0x00007BFB
		public Vec3 GetBoundingBoxMax()
		{
			return EngineApplicationInterface.IMesh.GetBoundingBoxMax(base.Pointer);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00009A10 File Offset: 0x00007C10
		public void AddTriangle(Vec3 p1, Vec3 p2, Vec3 p3, Vec2 uv1, Vec2 uv2, Vec2 uv3, uint color, UIntPtr lockHandle)
		{
			EngineApplicationInterface.IMesh.AddTriangle(base.Pointer, p1, p2, p3, uv1, uv2, uv3, color, lockHandle);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00009A3C File Offset: 0x00007C3C
		public void AddTriangleWithVertexColors(Vec3 p1, Vec3 p2, Vec3 p3, Vec2 uv1, Vec2 uv2, Vec2 uv3, uint c1, uint c2, uint c3, UIntPtr lockHandle)
		{
			EngineApplicationInterface.IMesh.AddTriangleWithVertexColors(base.Pointer, p1, p2, p3, uv1, uv2, uv3, c1, c2, c3, lockHandle);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00009A6A File Offset: 0x00007C6A
		public void HintIndicesDynamic()
		{
			EngineApplicationInterface.IMesh.HintIndicesDynamic(base.Pointer);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00009A7C File Offset: 0x00007C7C
		public void HintVerticesDynamic()
		{
			EngineApplicationInterface.IMesh.HintVerticesDynamic(base.Pointer);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00009A8E File Offset: 0x00007C8E
		public void RecomputeBoundingBox()
		{
			EngineApplicationInterface.IMesh.RecomputeBoundingBox(base.Pointer);
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x00009AA0 File Offset: 0x00007CA0
		// (set) Token: 0x060009E3 RID: 2531 RVA: 0x00009AB2 File Offset: 0x00007CB2
		public BillboardType Billboard
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetBillboard(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMesh.SetBillboard(base.Pointer, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00009AC5 File Offset: 0x00007CC5
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x00009AD7 File Offset: 0x00007CD7
		public VisibilityMaskFlags VisibilityMask
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetVisibilityMask(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMesh.SetVisibilityMask(base.Pointer, value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00009AEA File Offset: 0x00007CEA
		public int EditDataFaceCornerCount
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetEditDataFaceCornerCount(base.Pointer);
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00009AFC File Offset: 0x00007CFC
		public void SetEditDataFaceCornerVertexColor(int index, uint color)
		{
			EngineApplicationInterface.IMesh.SetEditDataFaceCornerVertexColor(base.Pointer, index, color);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00009B10 File Offset: 0x00007D10
		public uint GetEditDataFaceCornerVertexColor(int index)
		{
			return EngineApplicationInterface.IMesh.GetEditDataFaceCornerVertexColor(base.Pointer, index);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00009B23 File Offset: 0x00007D23
		public void PreloadForRendering()
		{
			EngineApplicationInterface.IMesh.PreloadForRendering(base.Pointer);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00009B35 File Offset: 0x00007D35
		public void SetContourColor(Vec3 color, bool alwaysVisible, bool maskMesh)
		{
			EngineApplicationInterface.IMesh.SetContourColor(base.Pointer, color, alwaysVisible, maskMesh);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00009B4A File Offset: 0x00007D4A
		public void DisableContour()
		{
			EngineApplicationInterface.IMesh.DisableContour(base.Pointer);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00009B5C File Offset: 0x00007D5C
		public void SetExternalBoundingBox(BoundingBox bbox)
		{
			EngineApplicationInterface.IMesh.SetExternalBoundingBox(base.Pointer, ref bbox);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00009B70 File Offset: 0x00007D70
		public void AddEditDataUser()
		{
			EngineApplicationInterface.IMesh.AddEditDataUser(base.Pointer);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00009B82 File Offset: 0x00007D82
		public void ReleaseEditDataUser()
		{
			EngineApplicationInterface.IMesh.ReleaseEditDataUser(base.Pointer);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00009B94 File Offset: 0x00007D94
		public void SetEditDataPolicy(EditDataPolicy policy)
		{
			EngineApplicationInterface.IMesh.SetEditDataPolicy(base.Pointer, policy);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00009BA7 File Offset: 0x00007DA7
		public UIntPtr LockEditDataWrite()
		{
			return EngineApplicationInterface.IMesh.LockEditDataWrite(base.Pointer);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00009BB9 File Offset: 0x00007DB9
		public void UnlockEditDataWrite(UIntPtr handle)
		{
			EngineApplicationInterface.IMesh.UnlockEditDataWrite(base.Pointer, handle);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00009BCC File Offset: 0x00007DCC
		public void SetCustomClipPlane(Vec3 clipPlanePosition, Vec3 clipPlaneNormal, int planeIndex)
		{
			EngineApplicationInterface.IMesh.SetCustomClipPlane(base.Pointer, clipPlanePosition, clipPlaneNormal, planeIndex);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00009BE1 File Offset: 0x00007DE1
		public float GetClothLinearVelocityMultiplier()
		{
			return EngineApplicationInterface.IMesh.GetClothLinearVelocityMultiplier(base.Pointer);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00009BF3 File Offset: 0x00007DF3
		public bool HasCloth()
		{
			return EngineApplicationInterface.IMesh.HasCloth(base.Pointer);
		}
	}
}
