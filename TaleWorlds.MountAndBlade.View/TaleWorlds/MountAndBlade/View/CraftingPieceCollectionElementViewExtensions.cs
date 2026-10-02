using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000015 RID: 21
	public static class CraftingPieceCollectionElementViewExtensions
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00004920 File Offset: 0x00002B20
		public static MatrixFrame GetCraftingPieceFrameForInventory(this CraftingPiece craftingPiece)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			Mat3 identity2 = Mat3.Identity;
			float num = 0.85f;
			Vec3 vec = new Vec3(0f, 0f, 0f, -1f);
			MetaMesh copy = MetaMesh.GetCopy(craftingPiece.MeshName, true, false);
			if (copy != null)
			{
				identity2.RotateAboutSide(-1.5707964f);
				identity2.RotateAboutForward(-0.7853982f);
				Vec3 vec2 = new Vec3(1000000f, 1000000f, 1000000f, -1f);
				Vec3 vec3 = new Vec3(-1000000f, -1000000f, -1000000f, -1f);
				for (int num2 = 0; num2 != copy.MeshCount; num2++)
				{
					Vec3 boundingBoxMin = copy.GetMeshAtIndex(num2).GetBoundingBoxMin();
					Vec3 boundingBoxMax = copy.GetMeshAtIndex(num2).GetBoundingBoxMax();
					Vec3[] array = new Vec3[8];
					Vec3[] array2 = array;
					int num3 = 0;
					Vec3 vec4 = new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMin.z, -1f);
					array2[num3] = identity2.TransformToParent(in vec4);
					Vec3[] array3 = array;
					int num4 = 1;
					vec4 = new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMax.z, -1f);
					array3[num4] = identity2.TransformToParent(in vec4);
					Vec3[] array4 = array;
					int num5 = 2;
					vec4 = new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMin.z, -1f);
					array4[num5] = identity2.TransformToParent(in vec4);
					Vec3[] array5 = array;
					int num6 = 3;
					vec4 = new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMax.z, -1f);
					array5[num6] = identity2.TransformToParent(in vec4);
					Vec3[] array6 = array;
					int num7 = 4;
					vec4 = new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMin.z, -1f);
					array6[num7] = identity2.TransformToParent(in vec4);
					Vec3[] array7 = array;
					int num8 = 5;
					vec4 = new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMax.z, -1f);
					array7[num8] = identity2.TransformToParent(in vec4);
					Vec3[] array8 = array;
					int num9 = 6;
					vec4 = new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMin.z, -1f);
					array8[num9] = identity2.TransformToParent(in vec4);
					Vec3[] array9 = array;
					int num10 = 7;
					vec4 = new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMax.z, -1f);
					array9[num10] = identity2.TransformToParent(in vec4);
					for (int i = 0; i < 8; i++)
					{
						vec2 = Vec3.Vec3Min(vec2, array[i]);
						vec3 = Vec3.Vec3Max(vec3, array[i]);
					}
				}
				float num11 = 1f;
				Vec3 vec5 = (vec2 + vec3) * 0.5f;
				float num12 = MathF.Max(vec3.x - vec2.x, vec3.y - vec2.y);
				float num13 = num * num11 / num12;
				identity.origin -= vec5 * num13;
				identity.origin += vec;
				identity.rotation = identity2;
				identity.rotation.ApplyScaleLocal(num13);
				identity.origin.z = identity.origin.z - 5f;
			}
			return identity;
		}
	}
}
