using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000019 RID: 25
	public static class ItemObjectViewExtensions
	{
		// Token: 0x060000AA RID: 170 RVA: 0x000058AC File Offset: 0x00003AAC
		public static MetaMesh GetCraftedMultiMesh(this ItemObject itemObject, bool needBatchedVersion)
		{
			CraftedDataView craftedDataView = CraftedDataViewManager.GetCraftedDataView(itemObject.WeaponDesign);
			if (!needBatchedVersion)
			{
				if (craftedDataView == null)
				{
					return null;
				}
				return craftedDataView.NonBatchedWeaponMesh.CreateCopy();
			}
			else
			{
				if (craftedDataView == null)
				{
					return null;
				}
				return craftedDataView.WeaponMesh.CreateCopy();
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000058EC File Offset: 0x00003AEC
		public static MetaMesh GetMultiMeshCopy(this ItemObject itemObject)
		{
			MetaMesh craftedMultiMesh = itemObject.GetCraftedMultiMesh(true);
			if (craftedMultiMesh != null)
			{
				return craftedMultiMesh;
			}
			if (string.IsNullOrEmpty(itemObject.MultiMeshName))
			{
				return null;
			}
			return MetaMesh.GetCopy(itemObject.MultiMeshName, true, false);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005928 File Offset: 0x00003B28
		public static MetaMesh GetMultiMeshCopyWithGenderData(this ItemObject itemObject, bool isFemale, bool useSlimVersion, bool needBatchedVersion)
		{
			MetaMesh craftedMultiMesh = itemObject.GetCraftedMultiMesh(needBatchedVersion);
			if (craftedMultiMesh != null)
			{
				return craftedMultiMesh;
			}
			if (string.IsNullOrEmpty(itemObject.MultiMeshName))
			{
				return null;
			}
			MetaMesh metaMesh = MetaMesh.GetCopy(isFemale ? (itemObject.MultiMeshName + "_female") : (itemObject.MultiMeshName + "_male"), false, true);
			if (metaMesh != null)
			{
				return metaMesh;
			}
			string text = itemObject.MultiMeshName;
			if (isFemale)
			{
				text += (useSlimVersion ? "_converted_slim" : "_converted");
			}
			else
			{
				text += (useSlimVersion ? "_slim" : "");
			}
			metaMesh = MetaMesh.GetCopy(text, false, true);
			if (metaMesh != null)
			{
				return metaMesh;
			}
			metaMesh = MetaMesh.GetCopy(itemObject.MultiMeshName, true, true);
			if (metaMesh != null)
			{
				return metaMesh;
			}
			return null;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000059F8 File Offset: 0x00003BF8
		public static MatrixFrame GetScaledFrame(this ItemObject itemObject, Mat3 rotationMatrix, MetaMesh metaMesh, float scaleFactor, Vec3 positionShift)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			Vec3 vec = new Vec3(1000000f, 1000000f, 1000000f, -1f);
			Vec3 vec2 = new Vec3(-1000000f, -1000000f, -1000000f, -1f);
			for (int num = 0; num != metaMesh.MeshCount; num++)
			{
				Vec3 boundingBoxMin = metaMesh.GetMeshAtIndex(num).GetBoundingBoxMin();
				Vec3 boundingBoxMax = metaMesh.GetMeshAtIndex(num).GetBoundingBoxMax();
				Vec3[] array = new Vec3[8];
				Vec3[] array2 = array;
				int num2 = 0;
				Vec3 vec3 = new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMin.z, -1f);
				array2[num2] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array3 = array;
				int num3 = 1;
				vec3 = new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMax.z, -1f);
				array3[num3] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array4 = array;
				int num4 = 2;
				vec3 = new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMin.z, -1f);
				array4[num4] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array5 = array;
				int num5 = 3;
				vec3 = new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMax.z, -1f);
				array5[num5] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array6 = array;
				int num6 = 4;
				vec3 = new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMin.z, -1f);
				array6[num6] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array7 = array;
				int num7 = 5;
				vec3 = new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMax.z, -1f);
				array7[num7] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array8 = array;
				int num8 = 6;
				vec3 = new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMin.z, -1f);
				array8[num8] = rotationMatrix.TransformToParent(in vec3);
				Vec3[] array9 = array;
				int num9 = 7;
				vec3 = new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMax.z, -1f);
				array9[num9] = rotationMatrix.TransformToParent(in vec3);
				for (int i = 0; i < 8; i++)
				{
					vec = Vec3.Vec3Min(vec, array[i]);
					vec2 = Vec3.Vec3Max(vec2, array[i]);
				}
			}
			float num10 = 1f;
			if (itemObject.PrimaryWeapon != null && itemObject.PrimaryWeapon.IsMeleeWeapon)
			{
				num10 = 0.3f + (float)itemObject.WeaponComponent.PrimaryWeapon.WeaponLength / 1.6f;
				num10 = MBMath.ClampFloat(num10, 0.5f, 1f);
			}
			Vec3 vec4 = (vec + vec2) * 0.5f;
			float num11 = MathF.Max(vec2.x - vec.x, vec2.y - vec.y);
			float num12 = scaleFactor * num10 / num11;
			identity.origin -= vec4 * num12;
			identity.origin += positionShift;
			identity.rotation = rotationMatrix;
			identity.rotation.ApplyScaleLocal(num12);
			return identity;
		}
	}
}
