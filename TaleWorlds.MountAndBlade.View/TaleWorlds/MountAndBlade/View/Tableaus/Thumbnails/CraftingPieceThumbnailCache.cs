using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000045 RID: 69
	public class CraftingPieceThumbnailCache : ThumbnailCache<CraftingPieceCreationData>
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		public CraftingPieceThumbnailCache(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000FBA5 File Offset: 0x0000DDA5
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._itemTableauGPUAllocationIndex = Utilities.RegisterGPUAllocationGroup("CraftingPieceThumbnailCache");
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000FBBD File Offset: 0x0000DDBD
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		protected override TextureCreationInfo OnCreateTexture(CraftingPieceCreationData thumbnailCreationData)
		{
			CraftingPiece craftingPiece = thumbnailCreationData.CraftingPiece;
			string type = thumbnailCreationData.Type;
			Action<Texture> setAction = thumbnailCreationData.SetAction;
			Action cancelAction = thumbnailCreationData.CancelAction;
			string text = craftingPiece.StringId + "$" + type;
			Texture texture;
			if (((IThumbnailCache)this).GetValue(text, out texture))
			{
				if (this._renderCallbacks.ContainsKey(text))
				{
					this._renderCallbacks[text].SetActions.Add(setAction);
					this._renderCallbacks[text].CancelActions.Add(cancelAction);
				}
				else if (setAction != null)
				{
					setAction(texture);
				}
				((IThumbnailCache)this).AddReference(text);
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			Camera camera = null;
			int num = 2;
			int num2 = 256;
			int num3 = 180;
			GameEntity gameEntity = this.CreateCraftingPieceBaseEntity(craftingPiece, type, BannerlordTableauManager.TableauCharacterScenes[num], ref camera);
			string text2 = ThumbnailCache<CraftingPieceCreationData>.CreateDebugIdFrom(text, "crf", "");
			ThumbnailRenderRequest thumbnailRenderRequest = ThumbnailRenderRequest.CreateWithoutTexture(BannerlordTableauManager.TableauCharacterScenes[num], camera, gameEntity, text, num2, num3, text2, this._itemTableauGPUAllocationIndex);
			this._thumbnailCreatorView.RegisterRenderRequest(ref thumbnailRenderRequest);
			gameEntity.ManualInvalidate();
			((IThumbnailCache)this).Add(text, null);
			((IThumbnailCache)this).AddReference(text);
			if (!this._renderCallbacks.ContainsKey(text))
			{
				this._renderCallbacks.Add(text, RenderCallbackCollection.CreateEmpty());
			}
			this._renderCallbacks[text].SetActions.Add(setAction);
			this._renderCallbacks[text].CancelActions.Add(cancelAction);
			return TextureCreationInfo.WithNewTexture(null);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000FD58 File Offset: 0x0000DF58
		protected override bool OnReleaseTexture(CraftingPieceCreationData thumbnailCreationData)
		{
			string renderId = thumbnailCreationData.RenderId;
			return ((IThumbnailCache)this).RemoveReference(renderId);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000FD74 File Offset: 0x0000DF74
		private GameEntity CreateCraftingPieceBaseEntity(CraftingPiece craftingPiece, string ItemType, Scene scene, ref Camera camera)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			bool flag = false;
			string text = "craftingPiece_cam";
			string text2 = "craftingPiece_frame";
			if (craftingPiece.PieceType == CraftingPiece.PieceTypes.Blade)
			{
				if (ItemType == "OneHandedAxe" || ItemType == "ThrowingAxe")
				{
					text = "craft_axe_camera";
					text2 = "craft_axe";
				}
				else if (ItemType == "TwoHandedAxe")
				{
					text = "craft_big_axe_camera";
					text2 = "craft_big_axe";
				}
				else if (ItemType == "Dagger" || ItemType == "ThrowingKnife" || ItemType == "TwoHandedPolearm" || ItemType == "Pike" || ItemType == "Javelin")
				{
					text = "craft_spear_blade_camera";
					text2 = "craft_spear_blade";
				}
				else if (ItemType == "Mace" || ItemType == "TwoHandedMace")
				{
					text = "craft_mace_camera";
					text2 = "craft_mace";
				}
				else
				{
					text = "craft_blade_camera";
					text2 = "craft_blade";
				}
				flag = true;
			}
			else if (craftingPiece.PieceType == CraftingPiece.PieceTypes.Pommel)
			{
				text = "craft_pommel_camera";
				text2 = "craft_pommel";
				flag = true;
			}
			else if (craftingPiece.PieceType == CraftingPiece.PieceTypes.Guard)
			{
				text = "craft_guard_camera";
				text2 = "craft_guard";
				flag = true;
			}
			else if (craftingPiece.PieceType == CraftingPiece.PieceTypes.Handle)
			{
				text = "craft_handle_camera";
				text2 = "craft_handle";
				flag = true;
			}
			bool flag2 = false;
			if (flag)
			{
				GameEntity gameEntity = scene.FindEntityWithTag(text);
				if (gameEntity != null)
				{
					camera = Camera.CreateCamera();
					Vec3 vec = default(Vec3);
					gameEntity.GetCameraParamsFromCameraScript(camera, ref vec);
				}
				GameEntity gameEntity2 = scene.FindEntityWithTag(text2);
				if (gameEntity2 != null)
				{
					matrixFrame = gameEntity2.GetGlobalFrame();
					gameEntity2.SetVisibilityExcludeParents(false);
					flag2 = true;
				}
			}
			else
			{
				GameEntity gameEntity3 = scene.FindEntityWithTag("old_system_item_frame");
				if (gameEntity3 != null)
				{
					matrixFrame = gameEntity3.GetGlobalFrame();
					gameEntity3.SetVisibilityExcludeParents(false);
				}
			}
			if (camera == null)
			{
				camera = Camera.CreateCamera();
				camera.SetViewVolume(false, -1f, 1f, -0.5f, 0.5f, 0.01f, 100f);
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin -= identity.rotation.u * 7f;
				identity.rotation.u = identity.rotation.u * -1f;
				camera.Frame = identity;
			}
			if (!flag2)
			{
				matrixFrame = craftingPiece.GetCraftingPieceFrameForInventory();
			}
			MetaMesh copy = MetaMesh.GetCopy(craftingPiece.MeshName, true, false);
			GameEntity gameEntity4 = null;
			if (copy != null)
			{
				gameEntity4 = scene.AddItemEntity(ref matrixFrame, copy);
			}
			else
			{
				MBDebug.ShowWarning("[DEBUG]craftingPiece with " + craftingPiece.StringId + "[DEBUG] string id cannot be found");
			}
			gameEntity4.SetVisibilityExcludeParents(false);
			return gameEntity4;
		}

		// Token: 0x04000144 RID: 324
		private int _itemTableauGPUAllocationIndex;
	}
}
