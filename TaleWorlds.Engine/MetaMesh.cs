using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006C RID: 108
	[EngineClass("rglMeta_mesh")]
	public sealed class MetaMesh : GameEntityComponent
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x00009F06 File Offset: 0x00008106
		internal MetaMesh(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00009F0F File Offset: 0x0000810F
		public static MetaMesh CreateMetaMesh(string name = null)
		{
			return EngineApplicationInterface.IMetaMesh.CreateMetaMesh(name);
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00009F1C File Offset: 0x0000811C
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00009F2E File Offset: 0x0000812E
		public int GetLodMaskForMeshAtIndex(int index)
		{
			return EngineApplicationInterface.IMetaMesh.GetLodMaskForMeshAtIndex(base.Pointer, index);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00009F41 File Offset: 0x00008141
		public int GetTotalGpuSize()
		{
			return EngineApplicationInterface.IMetaMesh.GetTotalGpuSize(base.Pointer);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00009F53 File Offset: 0x00008153
		public int RemoveMeshesWithTag(string tag)
		{
			return EngineApplicationInterface.IMetaMesh.RemoveMeshesWithTag(base.Pointer, tag);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00009F66 File Offset: 0x00008166
		public int RemoveMeshesWithoutTag(string tag)
		{
			return EngineApplicationInterface.IMetaMesh.RemoveMeshesWithoutTag(base.Pointer, tag);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00009F79 File Offset: 0x00008179
		public int GetMeshCountWithTag(string tag)
		{
			return EngineApplicationInterface.IMetaMesh.GetMeshCountWithTag(base.Pointer, tag);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00009F8C File Offset: 0x0000818C
		public bool HasVertexBufferOrEditDataOrPackageItem()
		{
			return EngineApplicationInterface.IMetaMesh.HasVertexBufferOrEditDataOrPackageItem(base.Pointer);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00009F9E File Offset: 0x0000819E
		public bool HasAnyGeneratedLods()
		{
			return EngineApplicationInterface.IMetaMesh.HasAnyGeneratedLods(base.Pointer);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00009FB0 File Offset: 0x000081B0
		public bool HasAnyLods()
		{
			return EngineApplicationInterface.IMetaMesh.HasAnyLods(base.Pointer);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00009FC2 File Offset: 0x000081C2
		public static MetaMesh GetCopy(string metaMeshName, bool showErrors = true, bool mayReturnNull = false)
		{
			return EngineApplicationInterface.IMetaMesh.CreateCopyFromName(metaMeshName, showErrors, mayReturnNull);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00009FD1 File Offset: 0x000081D1
		public void CopyTo(MetaMesh res, bool copyMeshes = true)
		{
			EngineApplicationInterface.IMetaMesh.CopyTo(base.Pointer, res.Pointer, copyMeshes);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00009FEA File Offset: 0x000081EA
		public void ClearMeshesForOtherLods(int lodToKeep)
		{
			EngineApplicationInterface.IMetaMesh.ClearMeshesForOtherLods(base.Pointer, lodToKeep);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00009FFD File Offset: 0x000081FD
		public void ClearMeshesForLod(int lodToClear)
		{
			EngineApplicationInterface.IMetaMesh.ClearMeshesForLod(base.Pointer, lodToClear);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0000A010 File Offset: 0x00008210
		public void ClearMeshesForLowerLods(int lodToClear)
		{
			EngineApplicationInterface.IMetaMesh.ClearMeshesForLowerLods(base.Pointer, lodToClear);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0000A023 File Offset: 0x00008223
		public void ClearMeshes()
		{
			EngineApplicationInterface.IMetaMesh.ClearMeshes(base.Pointer);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0000A035 File Offset: 0x00008235
		public void SetNumLods(int lodToClear)
		{
			EngineApplicationInterface.IMetaMesh.SetNumLods(base.Pointer, lodToClear);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0000A048 File Offset: 0x00008248
		public static void CheckMetaMeshExistence(string metaMeshName, int lod_count_check)
		{
			EngineApplicationInterface.IMetaMesh.CheckMetaMeshExistence(metaMeshName, lod_count_check);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0000A056 File Offset: 0x00008256
		public static MetaMesh GetMorphedCopy(string metaMeshName, float morphTarget, bool showErrors)
		{
			return EngineApplicationInterface.IMetaMesh.GetMorphedCopy(metaMeshName, morphTarget, showErrors);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0000A065 File Offset: 0x00008265
		public MetaMesh CreateCopy()
		{
			return EngineApplicationInterface.IMetaMesh.CreateCopy(base.Pointer);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0000A077 File Offset: 0x00008277
		public void AddMesh(Mesh mesh)
		{
			EngineApplicationInterface.IMetaMesh.AddMesh(base.Pointer, mesh.Pointer, 0U);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0000A090 File Offset: 0x00008290
		public void AddMesh(Mesh mesh, uint lodLevel)
		{
			EngineApplicationInterface.IMetaMesh.AddMesh(base.Pointer, mesh.Pointer, lodLevel);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000A0A9 File Offset: 0x000082A9
		public void AddMetaMesh(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IMetaMesh.AddMetaMesh(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0000A0C1 File Offset: 0x000082C1
		public void SetCullMode(MBMeshCullingMode cullMode)
		{
			EngineApplicationInterface.IMetaMesh.SetCullMode(base.Pointer, cullMode);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000A0D4 File Offset: 0x000082D4
		public void AddMaterialShaderFlag(string materialShaderFlag)
		{
			for (int i = 0; i < this.MeshCount; i++)
			{
				Mesh meshAtIndex = this.GetMeshAtIndex(i);
				Material material = meshAtIndex.GetMaterial();
				material = material.CreateCopy();
				material.AddMaterialShaderFlag(materialShaderFlag, false);
				meshAtIndex.SetMaterial(material);
			}
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0000A115 File Offset: 0x00008315
		public void MergeMultiMeshes(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IMetaMesh.MergeMultiMeshes(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0000A12D File Offset: 0x0000832D
		public void AssignClothBodyFrom(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IMetaMesh.AssignClothBodyFrom(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0000A145 File Offset: 0x00008345
		public void BatchMultiMeshes(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IMetaMesh.BatchMultiMeshes(base.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0000A15D File Offset: 0x0000835D
		public bool HasClothData()
		{
			return EngineApplicationInterface.IMetaMesh.HasClothData(base.Pointer);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0000A170 File Offset: 0x00008370
		public void BatchMultiMeshesMultiple(List<MetaMesh> metaMeshes)
		{
			UIntPtr[] array = new UIntPtr[metaMeshes.Count];
			for (int i = 0; i < metaMeshes.Count; i++)
			{
				array[i] = metaMeshes[i].Pointer;
			}
			EngineApplicationInterface.IMetaMesh.BatchMultiMeshesMultiple(base.Pointer, array, metaMeshes.Count);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0000A1C0 File Offset: 0x000083C0
		public void ClearEditData()
		{
			EngineApplicationInterface.IMetaMesh.ClearEditData(base.Pointer);
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0000A1D2 File Offset: 0x000083D2
		public int MeshCount
		{
			get
			{
				return EngineApplicationInterface.IMetaMesh.GetMeshCount(base.Pointer);
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0000A1E4 File Offset: 0x000083E4
		public Mesh GetMeshAtIndex(int meshIndex)
		{
			return EngineApplicationInterface.IMetaMesh.GetMeshAtIndex(base.Pointer, meshIndex);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0000A1F8 File Offset: 0x000083F8
		public Mesh GetFirstMeshWithTag(string tag)
		{
			for (int i = 0; i < this.MeshCount; i++)
			{
				Mesh meshAtIndex = this.GetMeshAtIndex(i);
				if (meshAtIndex.HasTag(tag))
				{
					return meshAtIndex;
				}
			}
			return null;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0000A22A File Offset: 0x0000842A
		private void Release()
		{
			EngineApplicationInterface.IMetaMesh.Release(base.Pointer);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0000A23C File Offset: 0x0000843C
		public uint GetFactor1()
		{
			return EngineApplicationInterface.IMetaMesh.GetFactor1(base.Pointer);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0000A24E File Offset: 0x0000844E
		public void SetGlossMultiplier(float value)
		{
			EngineApplicationInterface.IMetaMesh.SetGlossMultiplier(base.Pointer, value);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0000A261 File Offset: 0x00008461
		public uint GetFactor2()
		{
			return EngineApplicationInterface.IMetaMesh.GetFactor2(base.Pointer);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000A273 File Offset: 0x00008473
		public void SetFactor1Linear(uint linearFactorColor1)
		{
			EngineApplicationInterface.IMetaMesh.SetFactor1Linear(base.Pointer, linearFactorColor1);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0000A286 File Offset: 0x00008486
		public void SetFactor2Linear(uint linearFactorColor2)
		{
			EngineApplicationInterface.IMetaMesh.SetFactor2Linear(base.Pointer, linearFactorColor2);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0000A299 File Offset: 0x00008499
		public void SetFactor1(uint factorColor1)
		{
			EngineApplicationInterface.IMetaMesh.SetFactor1(base.Pointer, factorColor1);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0000A2AC File Offset: 0x000084AC
		public void SetFactor2(uint factorColor2)
		{
			EngineApplicationInterface.IMetaMesh.SetFactor2(base.Pointer, factorColor2);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0000A2BF File Offset: 0x000084BF
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IMetaMesh.SetVectorArgument(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0000A2D6 File Offset: 0x000084D6
		public void SetVectorArgument2(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IMetaMesh.SetVectorArgument2(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0000A2ED File Offset: 0x000084ED
		public Vec3 GetVectorArgument2()
		{
			return EngineApplicationInterface.IMetaMesh.GetVectorArgument2(base.Pointer);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0000A2FF File Offset: 0x000084FF
		public void SetMaterial(Material material)
		{
			EngineApplicationInterface.IMetaMesh.SetMaterial(base.Pointer, material.Pointer);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0000A317 File Offset: 0x00008517
		public void SetShaderToMaterial(string shaderName)
		{
			EngineApplicationInterface.IMetaMesh.SetShaderToMaterial(base.Pointer, shaderName);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0000A32A File Offset: 0x0000852A
		public void SetLodBias(int lodBias)
		{
			EngineApplicationInterface.IMetaMesh.SetLodBias(base.Pointer, lodBias);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0000A33D File Offset: 0x0000853D
		public void SetBillboarding(BillboardType billboard)
		{
			EngineApplicationInterface.IMetaMesh.SetBillboarding(base.Pointer, billboard);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0000A350 File Offset: 0x00008550
		public void UseHeadBoneFaceGenScaling(Skeleton skeleton, sbyte headLookDirectionBoneIndex, MatrixFrame frame)
		{
			EngineApplicationInterface.IMetaMesh.UseHeadBoneFaceGenScaling(base.Pointer, skeleton.Pointer, headLookDirectionBoneIndex, ref frame);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0000A36B File Offset: 0x0000856B
		public void DrawTextWithDefaultFont(string text, Vec2 textPositionMin, Vec2 textPositionMax, Vec2 size, uint color, TextFlags flags)
		{
			EngineApplicationInterface.IMetaMesh.DrawTextWithDefaultFont(base.Pointer, text, textPositionMin, textPositionMax, size, color, flags);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0000A388 File Offset: 0x00008588
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x0000A3B0 File Offset: 0x000085B0
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				EngineApplicationInterface.IMetaMesh.GetFrame(base.Pointer, ref matrixFrame);
				return matrixFrame;
			}
			set
			{
				EngineApplicationInterface.IMetaMesh.SetFrame(base.Pointer, ref value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0000A3C4 File Offset: 0x000085C4
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x0000A3D6 File Offset: 0x000085D6
		public Vec3 VectorUserData
		{
			get
			{
				return EngineApplicationInterface.IMetaMesh.GetVectorUserData(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMetaMesh.SetVectorUserData(base.Pointer, ref value);
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0000A3EA File Offset: 0x000085EA
		public void PreloadForRendering()
		{
			EngineApplicationInterface.IMetaMesh.PreloadForRendering(base.Pointer);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0000A3FC File Offset: 0x000085FC
		public int CheckResources()
		{
			return EngineApplicationInterface.IMetaMesh.CheckResources(base.Pointer);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0000A40E File Offset: 0x0000860E
		public void PreloadShaders(bool useTableau, bool useTeamColor)
		{
			EngineApplicationInterface.IMetaMesh.PreloadShaders(base.Pointer, useTableau, useTeamColor);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0000A422 File Offset: 0x00008622
		public void RecomputeBoundingBox(bool recomputeMeshes)
		{
			EngineApplicationInterface.IMetaMesh.RecomputeBoundingBox(base.Pointer, recomputeMeshes);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0000A435 File Offset: 0x00008635
		public void AddEditDataUser()
		{
			EngineApplicationInterface.IMetaMesh.AddEditDataUser(base.Pointer);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0000A447 File Offset: 0x00008647
		public void ReleaseEditDataUser()
		{
			EngineApplicationInterface.IMetaMesh.ReleaseEditDataUser(base.Pointer);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0000A459 File Offset: 0x00008659
		public void SetEditDataPolicy(EditDataPolicy policy)
		{
			EngineApplicationInterface.IMetaMesh.SetEditDataPolicy(base.Pointer, policy);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0000A46C File Offset: 0x0000866C
		public MatrixFrame Fit()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			Vec3 vec = new Vec3(1000000f, 1000000f, 1000000f, -1f);
			Vec3 vec2 = new Vec3(-1000000f, -1000000f, -1000000f, -1f);
			for (int num = 0; num != this.MeshCount; num++)
			{
				Vec3 boundingBoxMin = this.GetMeshAtIndex(num).GetBoundingBoxMin();
				Vec3 boundingBoxMax = this.GetMeshAtIndex(num).GetBoundingBoxMax();
				vec = Vec3.Vec3Min(vec, boundingBoxMin);
				vec2 = Vec3.Vec3Max(vec2, boundingBoxMax);
			}
			Vec3 vec3 = (vec + vec2) * 0.5f;
			float num2 = MathF.Max(vec2.x - vec.x, vec2.y - vec.y);
			float num3 = 0.95f / num2;
			identity.origin -= vec3 * num3;
			identity.rotation.ApplyScaleLocal(num3);
			return identity;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000A568 File Offset: 0x00008768
		public BoundingBox GetBoundingBox()
		{
			BoundingBox boundingBox = default(BoundingBox);
			EngineApplicationInterface.IMetaMesh.GetBoundingBox(base.Pointer, ref boundingBox);
			return boundingBox;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0000A590 File Offset: 0x00008790
		public VisibilityMaskFlags GetVisibilityMask()
		{
			return EngineApplicationInterface.IMetaMesh.GetVisibilityMask(base.Pointer);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0000A5A2 File Offset: 0x000087A2
		public void SetVisibilityMask(VisibilityMaskFlags visibilityMask)
		{
			EngineApplicationInterface.IMetaMesh.SetVisibilityMask(base.Pointer, visibilityMask);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0000A5B5 File Offset: 0x000087B5
		public string GetName()
		{
			return EngineApplicationInterface.IMetaMesh.GetName(base.Pointer);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0000A5C8 File Offset: 0x000087C8
		public static void GetAllMultiMeshes(ref List<MetaMesh> multiMeshList)
		{
			int multiMeshCount = EngineApplicationInterface.IMetaMesh.GetMultiMeshCount();
			UIntPtr[] array = new UIntPtr[multiMeshCount];
			EngineApplicationInterface.IMetaMesh.GetAllMultiMeshes(array);
			for (int i = 0; i < multiMeshCount; i++)
			{
				multiMeshList.Add(new MetaMesh(array[i]));
			}
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0000A60E File Offset: 0x0000880E
		public static MetaMesh GetMultiMesh(string name)
		{
			return EngineApplicationInterface.IMetaMesh.GetMultiMesh(name);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0000A61B File Offset: 0x0000881B
		public void SetContourState(bool alwaysVisible)
		{
			EngineApplicationInterface.IMetaMesh.SetContourState(base.Pointer, alwaysVisible);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0000A62E File Offset: 0x0000882E
		public void SetContourColor(uint color)
		{
			EngineApplicationInterface.IMetaMesh.SetContourColor(base.Pointer, color);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0000A641 File Offset: 0x00008841
		public void SetMaterialToSubMeshesWithTag(Material bodyMaterial, string tag)
		{
			EngineApplicationInterface.IMetaMesh.SetMaterialToSubMeshesWithTag(base.Pointer, bodyMaterial.Pointer, tag);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000A65A File Offset: 0x0000885A
		public void SetFactorColorToSubMeshesWithTag(uint color, string tag)
		{
			EngineApplicationInterface.IMetaMesh.SetFactorColorToSubMeshesWithTag(base.Pointer, color, tag);
		}
	}
}
