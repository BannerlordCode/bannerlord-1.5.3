using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000013 RID: 19
	public class CraftedDataView
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003F6B File Offset: 0x0000216B
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003F73 File Offset: 0x00002173
		public WeaponDesign CraftedData { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003F7C File Offset: 0x0000217C
		public MetaMesh WeaponMesh
		{
			get
			{
				if (!(this._weaponMesh != null) || !this._weaponMesh.HasVertexBufferOrEditDataOrPackageItem())
				{
					return this._weaponMesh = this.GenerateWeaponMesh(true);
				}
				return this._weaponMesh;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003FBC File Offset: 0x000021BC
		public MetaMesh HolsterMesh
		{
			get
			{
				MetaMesh metaMesh;
				if ((metaMesh = this._holsterMesh) == null)
				{
					metaMesh = (this._holsterMesh = this.GenerateHolsterMesh());
				}
				return metaMesh;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003FE4 File Offset: 0x000021E4
		public MetaMesh HolsterMeshWithWeapon
		{
			get
			{
				if (!(this._holsterMeshWithWeapon != null) || !this._holsterMeshWithWeapon.HasVertexBufferOrEditDataOrPackageItem())
				{
					return this._holsterMeshWithWeapon = this.GenerateHolsterMeshWithWeapon(true);
				}
				return this._holsterMeshWithWeapon;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004024 File Offset: 0x00002224
		public MetaMesh NonBatchedWeaponMesh
		{
			get
			{
				MetaMesh metaMesh;
				if ((metaMesh = this._nonBatchedWeaponMesh) == null)
				{
					metaMesh = (this._nonBatchedWeaponMesh = this.GenerateWeaponMesh(false));
				}
				return metaMesh;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000404C File Offset: 0x0000224C
		public MetaMesh NonBatchedHolsterMesh
		{
			get
			{
				MetaMesh metaMesh;
				if ((metaMesh = this._nonBatchedHolsterMesh) == null)
				{
					metaMesh = (this._nonBatchedHolsterMesh = this.GenerateHolsterMesh());
				}
				return metaMesh;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004074 File Offset: 0x00002274
		public MetaMesh NonBatchedHolsterMeshWithWeapon
		{
			get
			{
				MetaMesh metaMesh;
				if ((metaMesh = this._nonBatchedHolsterMeshWithWeapon) == null)
				{
					metaMesh = (this._nonBatchedHolsterMeshWithWeapon = this.GenerateHolsterMeshWithWeapon(false));
				}
				return metaMesh;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000409B File Offset: 0x0000229B
		public CraftedDataView(WeaponDesign craftedData)
		{
			this.CraftedData = craftedData;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000040AA File Offset: 0x000022AA
		public void Clear()
		{
			this._weaponMesh = null;
			this._holsterMesh = null;
			this._holsterMeshWithWeapon = null;
			this._nonBatchedWeaponMesh = null;
			this._nonBatchedHolsterMesh = null;
			this._nonBatchedHolsterMeshWithWeapon = null;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000040D6 File Offset: 0x000022D6
		private MetaMesh GenerateWeaponMesh(bool batchMeshes)
		{
			if (this.CraftedData.UsedPieces != null)
			{
				return CraftedDataView.BuildWeaponMesh(this.CraftedData, 0f, false, batchMeshes);
			}
			return null;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000040F9 File Offset: 0x000022F9
		private MetaMesh GenerateHolsterMesh()
		{
			if (this.CraftedData.UsedPieces != null)
			{
				return CraftedDataView.BuildHolsterMesh(this.CraftedData);
			}
			return null;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004115 File Offset: 0x00002315
		private MetaMesh GenerateHolsterMeshWithWeapon(bool batchMeshes)
		{
			if (this.CraftedData.UsedPieces != null)
			{
				return CraftedDataView.BuildHolsterMeshWithWeapon(this.CraftedData, 0f, batchMeshes);
			}
			return null;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004138 File Offset: 0x00002338
		public static MetaMesh BuildWeaponMesh(WeaponDesign craftedData, float pivotDiff, bool pieceTypeHidingEnabledForHolster, bool batchAllMeshes)
		{
			CraftingTemplate template = craftedData.Template;
			MetaMesh metaMesh = MetaMesh.CreateMetaMesh(null);
			List<MetaMesh> list = new List<MetaMesh>();
			List<MetaMesh> list2 = new List<MetaMesh>();
			List<MetaMesh> list3 = new List<MetaMesh>();
			foreach (PieceData pieceData in template.BuildOrders)
			{
				if (!pieceTypeHidingEnabledForHolster || !template.IsPieceTypeHiddenOnHolster(pieceData.PieceType))
				{
					WeaponDesignElement weaponDesignElement = craftedData.UsedPieces[(int)pieceData.PieceType];
					float num = craftedData.PiecePivotDistances[(int)pieceData.PieceType];
					if (weaponDesignElement != null && weaponDesignElement.IsValid && !float.IsNaN(num))
					{
						MetaMesh copy = MetaMesh.GetCopy(weaponDesignElement.CraftingPiece.MeshName, true, false);
						if (!batchAllMeshes)
						{
							copy.ClearMeshesForOtherLods(0);
						}
						Mat3 identity = Mat3.Identity;
						Vec3 vec = num * Vec3.Up;
						MatrixFrame matrixFrame = new MatrixFrame(in identity, in vec);
						if (weaponDesignElement.IsPieceScaled)
						{
							Vec3 vec2 = (weaponDesignElement.CraftingPiece.FullScale ? (Vec3.One * weaponDesignElement.ScaleFactor) : new Vec3(1f, 1f, weaponDesignElement.ScaleFactor, -1f));
							matrixFrame.Scale(in vec2);
						}
						copy.Frame = matrixFrame;
						if (copy.HasClothData())
						{
							list3.Add(copy);
						}
						else
						{
							list2.Add(copy);
						}
					}
				}
			}
			foreach (MetaMesh metaMesh2 in list2)
			{
				if (batchAllMeshes)
				{
					list.Add(metaMesh2);
				}
				else
				{
					metaMesh.MergeMultiMeshes(metaMesh2);
				}
			}
			if (batchAllMeshes)
			{
				metaMesh.BatchMultiMeshesMultiple(list);
			}
			foreach (MetaMesh metaMesh3 in list3)
			{
				metaMesh.MergeMultiMeshes(metaMesh3);
				metaMesh.AssignClothBodyFrom(metaMesh3);
			}
			metaMesh.SetEditDataPolicy(EditDataPolicy.KeepUntilFirstRender);
			if (batchAllMeshes)
			{
				metaMesh.SetLodBias(1);
			}
			MatrixFrame frame = metaMesh.Frame;
			frame.Elevate(pivotDiff);
			metaMesh.Frame = frame;
			if (CraftedDataView.OnWeaponMeshBuilt != null)
			{
				CraftedDataView.OnWeaponMeshBuilt(craftedData, ref metaMesh);
			}
			return metaMesh;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004380 File Offset: 0x00002580
		public static MetaMesh BuildHolsterMesh(WeaponDesign craftedData)
		{
			if (craftedData.Template.UseWeaponAsHolsterMesh)
			{
				return null;
			}
			BladeData bladeData = craftedData.UsedPieces[0].CraftingPiece.BladeData;
			if (craftedData.Template.AlwaysShowHolsterWithWeapon || string.IsNullOrEmpty(bladeData.HolsterMeshName))
			{
				return null;
			}
			float num = craftedData.PiecePivotDistances[0];
			MetaMesh copy = MetaMesh.GetCopy(bladeData.HolsterMeshName, false, false);
			MatrixFrame frame = copy.Frame;
			frame.origin += new Vec3(0f, 0f, num, -1f);
			WeaponDesignElement weaponDesignElement = craftedData.UsedPieces[0];
			if (MathF.Abs(weaponDesignElement.ScaledLength - weaponDesignElement.CraftingPiece.Length) > 1E-05f)
			{
				Vec3 vec = (weaponDesignElement.CraftingPiece.FullScale ? (Vec3.One * weaponDesignElement.ScaleFactor) : new Vec3(1f, 1f, weaponDesignElement.ScaleFactor, -1f));
				frame.Scale(in vec);
			}
			copy.Frame = frame;
			MetaMesh metaMesh = MetaMesh.CreateMetaMesh(bladeData.HolsterMeshName);
			metaMesh.MergeMultiMeshes(copy);
			if (CraftedDataView.OnHolsterMeshBuilt != null)
			{
				CraftedDataView.OnHolsterMeshBuilt(craftedData, ref metaMesh);
			}
			return metaMesh;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000044B8 File Offset: 0x000026B8
		public static MetaMesh BuildHolsterMeshWithWeapon(WeaponDesign craftedData, float pivotDiff, bool batchAllMeshes)
		{
			if (craftedData.Template.UseWeaponAsHolsterMesh)
			{
				return null;
			}
			WeaponDesignElement weaponDesignElement = craftedData.UsedPieces[0];
			BladeData bladeData = weaponDesignElement.CraftingPiece.BladeData;
			if (string.IsNullOrEmpty(bladeData.HolsterMeshName))
			{
				return null;
			}
			MetaMesh metaMesh = MetaMesh.CreateMetaMesh(null);
			MetaMesh copy = MetaMesh.GetCopy(bladeData.HolsterMeshName, false, true);
			string text = bladeData.HolsterMeshName + "_skeleton";
			if (Skeleton.SkeletonModelExist(text))
			{
				MetaMesh metaMesh2 = CraftedDataView.BuildWeaponMesh(craftedData, 0f, true, batchAllMeshes);
				float num = craftedData.PiecePivotDistances[0];
				float scaledDistanceToPreviousPiece = craftedData.UsedPieces[0].ScaledDistanceToPreviousPiece;
				float num2 = num - scaledDistanceToPreviousPiece;
				List<MetaMesh> list = new List<MetaMesh>();
				Skeleton skeleton = Skeleton.CreateFromModel(text);
				for (sbyte b = 1; b < skeleton.GetBoneCount(); b += 1)
				{
					MatrixFrame boneEntitialRestFrame = skeleton.GetBoneEntitialRestFrame(b, false);
					if (craftedData.Template.RotateWeaponInHolster)
					{
						boneEntitialRestFrame.rotation.RotateAboutForward(3.1415927f);
					}
					MetaMesh metaMesh3 = metaMesh2.CreateCopy();
					MatrixFrame matrixFrame = new MatrixFrame(in boneEntitialRestFrame.rotation, in boneEntitialRestFrame.origin);
					matrixFrame.Elevate(-num2);
					metaMesh3.Frame = matrixFrame;
					if (batchAllMeshes)
					{
						int num3 = (int)(8 - (b - 1));
						metaMesh3.SetShaderToMaterial("quiver_deformer");
						metaMesh3.SetFactor1Linear((uint)(419430400L * (long)num3));
						list.Add(metaMesh3);
					}
					else
					{
						metaMesh.MergeMultiMeshes(metaMesh3);
					}
				}
				if (list.Count > 0)
				{
					metaMesh.BatchMultiMeshesMultiple(list);
				}
				if (craftedData.Template.PieceTypeToScaleHolsterWith != CraftingPiece.PieceTypes.Invalid)
				{
					WeaponDesignElement weaponDesignElement2 = craftedData.UsedPieces[(int)craftedData.Template.PieceTypeToScaleHolsterWith];
					MatrixFrame frame = copy.Frame;
					int num4 = -MathF.Sign(skeleton.GetBoneEntitialRestFrame(0, false).rotation.u.z);
					float num5 = weaponDesignElement.CraftingPiece.BladeData.HolsterMeshLength * (weaponDesignElement2.ScaleFactor - 1f) * 0.5f * (float)num4;
					WeaponDesignElement weaponDesignElement3 = craftedData.UsedPieces[(int)craftedData.Template.PieceTypeToScaleHolsterWith];
					if (weaponDesignElement3.IsPieceScaled)
					{
						Vec3 vec = (weaponDesignElement3.CraftingPiece.FullScale ? (Vec3.One * weaponDesignElement3.ScaleFactor) : new Vec3(1f, 1f, weaponDesignElement3.ScaleFactor, -1f));
						frame.Scale(in vec);
					}
					frame.origin += new Vec3(0f, 0f, -num5, -1f);
					copy.Frame = frame;
				}
			}
			else
			{
				if (craftedData.Template.PieceTypeToScaleHolsterWith != CraftingPiece.PieceTypes.Invalid)
				{
					MatrixFrame frame2 = copy.Frame;
					frame2.origin += new Vec3(0f, 0f, craftedData.PiecePivotDistances[(int)craftedData.Template.PieceTypeToScaleHolsterWith], -1f);
					WeaponDesignElement weaponDesignElement4 = craftedData.UsedPieces[(int)craftedData.Template.PieceTypeToScaleHolsterWith];
					if (weaponDesignElement4.IsPieceScaled)
					{
						Vec3 vec2 = (weaponDesignElement4.CraftingPiece.FullScale ? (Vec3.One * weaponDesignElement4.ScaleFactor) : new Vec3(1f, 1f, weaponDesignElement4.ScaleFactor, -1f));
						frame2.Scale(in vec2);
					}
					copy.Frame = frame2;
				}
				metaMesh.MergeMultiMeshes(CraftedDataView.BuildWeaponMesh(craftedData, 0f, true, batchAllMeshes));
			}
			metaMesh.MergeMultiMeshes(copy);
			MatrixFrame frame3 = metaMesh.Frame;
			frame3.origin += new Vec3(0f, 0f, pivotDiff, -1f);
			metaMesh.Frame = frame3;
			if (CraftedDataView.OnHolsterMeshWithWeaponBuilt != null)
			{
				CraftedDataView.OnHolsterMeshWithWeaponBuilt(craftedData, ref metaMesh);
			}
			return metaMesh;
		}

		// Token: 0x04000010 RID: 16
		public static CraftedDataView.OnMeshBuiltDelegate OnWeaponMeshBuilt;

		// Token: 0x04000011 RID: 17
		public static CraftedDataView.OnMeshBuiltDelegate OnHolsterMeshBuilt;

		// Token: 0x04000012 RID: 18
		public static CraftedDataView.OnMeshBuiltDelegate OnHolsterMeshWithWeaponBuilt;

		// Token: 0x04000014 RID: 20
		private MetaMesh _weaponMesh;

		// Token: 0x04000015 RID: 21
		private MetaMesh _holsterMesh;

		// Token: 0x04000016 RID: 22
		private MetaMesh _holsterMeshWithWeapon;

		// Token: 0x04000017 RID: 23
		private MetaMesh _nonBatchedWeaponMesh;

		// Token: 0x04000018 RID: 24
		private MetaMesh _nonBatchedHolsterMesh;

		// Token: 0x04000019 RID: 25
		private MetaMesh _nonBatchedHolsterMeshWithWeapon;

		// Token: 0x020000AD RID: 173
		// (Invoke) Token: 0x06000604 RID: 1540
		public delegate void OnMeshBuiltDelegate(WeaponDesign weaponDesign, ref MetaMesh builtMesh);
	}
}
