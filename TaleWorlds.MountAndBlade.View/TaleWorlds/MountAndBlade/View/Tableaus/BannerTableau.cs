using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x0200002F RID: 47
	public class BannerTableau
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00009206 File Offset: 0x00007406
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000920E File Offset: 0x0000740E
		public Texture Texture { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00009217 File Offset: 0x00007417
		internal Camera CurrentCamera
		{
			get
			{
				if (!this._isNineGrid)
				{
					return this._defaultCamera;
				}
				return this._nineGridCamera;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000922E File Offset: 0x0000742E
		private TableauView View
		{
			get
			{
				if (this.Texture != null)
				{
					return this.Texture.TableauView;
				}
				return null;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000924B File Offset: 0x0000744B
		public BannerTableau()
		{
			this.SetEnabled(true);
			this.FirstTimeInit();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009284 File Offset: 0x00007484
		public void OnTick(float dt)
		{
			if (this._isEnabled && !this._isFinalized)
			{
				this.Refresh();
				TableauView view = this.View;
				if (view == null)
				{
					return;
				}
				view.SetDoNotRenderThisFrame(false);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000092B0 File Offset: 0x000074B0
		private void FirstTimeInit()
		{
			this._scene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
			this._scene.DisableStaticShadows(true);
			this._scene.SetName("BannerTableau.Scene");
			this._scene.SetDefaultLighting();
			this._defaultCamera = BannerTextureCreator.CreateDefaultBannerCamera();
			this._nineGridCamera = BannerTextureCreator.CreateNineGridBannerCamera();
			this._isDirty = true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009314 File Offset: 0x00007514
		private void Refresh()
		{
			if (this._isDirty)
			{
				if (this._currentMeshEntity != null)
				{
					this._scene.RemoveEntity(this._currentMeshEntity, 111);
				}
				if (this._banner != null)
				{
					MatrixFrame identity = MatrixFrame.Identity;
					if (Banner.IsValidBannerCode(this._banner.BannerCode))
					{
						this._currentMultiMesh = this._banner.ConvertToMultiMesh();
						this._currentMeshEntity = this._scene.AddItemEntity(ref identity, this._currentMultiMesh);
						this._currentMeshEntity.ManualInvalidate();
						this._currentMultiMesh.ManualInvalidate();
					}
					else
					{
						Debug.FailedAssert("Banner code is not valid: " + this._banner.BannerCode, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\BannerTableau.cs", "Refresh", 109);
					}
					this._isDirty = false;
				}
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000093DF File Offset: 0x000075DF
		private void SetEnabled(bool enabled)
		{
			this._isEnabled = enabled;
			TableauView view = this.View;
			if (view == null)
			{
				return;
			}
			view.SetEnable(this._isEnabled);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00009400 File Offset: 0x00007600
		public void SetTargetSize(int width, int height)
		{
			this._latestWidth = width;
			this._latestHeight = height;
			if (width <= 0 || height <= 0)
			{
				this._tableauSizeX = 10;
				this._tableauSizeY = 10;
			}
			else
			{
				this.RenderScale = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ResolutionScale) / 100f;
				this._tableauSizeX = (int)((float)width * this._customRenderScale * this.RenderScale);
				this._tableauSizeY = (int)((float)height * this._customRenderScale * this.RenderScale);
			}
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			TableauView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(true);
			}
			Texture texture = this.Texture;
			if (texture != null)
			{
				texture.Release();
			}
			this.Texture = TableauView.AddTableau(string.Format("BannerTableau_{0}", BannerTableau._tableauIndex++), new RenderTargetComponent.TextureUpdateEventHandler(this.BannerTableauContinuousRenderFunction), this._scene, this._tableauSizeX, this._tableauSizeY);
			this.Texture.TableauView.SetSceneUsesContour(false);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009501 File Offset: 0x00007701
		public void SetBannerCode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				this._banner = null;
			}
			else
			{
				this._banner = new Banner(value);
			}
			this._isDirty = true;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00009528 File Offset: 0x00007728
		public void OnFinalize()
		{
			if (!this._isFinalized)
			{
				Scene scene = this._scene;
				if (scene != null)
				{
					scene.ClearDecals();
				}
				Scene scene2 = this._scene;
				if (scene2 != null)
				{
					scene2.ClearAll();
				}
				Scene scene3 = this._scene;
				if (scene3 != null)
				{
					scene3.ManualInvalidate();
				}
				this._scene = null;
				TableauView view = this.View;
				if (view != null)
				{
					view.SetEnable(false);
				}
				Texture texture = this.Texture;
				if (texture != null)
				{
					texture.Release();
				}
				this.Texture = null;
				Camera defaultCamera = this._defaultCamera;
				if (defaultCamera != null)
				{
					defaultCamera.ReleaseCamera();
				}
				this._defaultCamera = null;
				Camera nineGridCamera = this._nineGridCamera;
				if (nineGridCamera != null)
				{
					nineGridCamera.ReleaseCamera();
				}
				this._nineGridCamera = null;
			}
			this._isFinalized = true;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000095DB File Offset: 0x000077DB
		public void SetCustomRenderScale(float value)
		{
			if (!this._customRenderScale.ApproximatelyEqualsTo(value, 1E-05f))
			{
				this._customRenderScale = value;
				if (this._latestWidth != -1 && this._latestHeight != -1)
				{
					this.SetTargetSize(this._latestWidth, this._latestHeight);
				}
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000961C File Offset: 0x0000781C
		internal void BannerTableauContinuousRenderFunction(Texture sender, EventArgs e)
		{
			Scene scene = (Scene)sender.UserData;
			TableauView tableauView = sender.TableauView;
			if (scene == null)
			{
				tableauView.SetContinuousRendering(false);
				tableauView.SetDeleteAfterRendering(true);
				return;
			}
			scene.EnsurePostfxSystem();
			scene.SetDofMode(false);
			scene.SetMotionBlurMode(false);
			scene.SetBloom(false);
			scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.31f);
			tableauView.SetRenderWithPostfx(false);
			tableauView.SetScene(scene);
			tableauView.SetCamera(this.CurrentCamera);
			tableauView.SetSceneUsesSkybox(false);
			tableauView.SetDeleteAfterRendering(false);
			tableauView.SetContinuousRendering(true);
			tableauView.SetDoNotRenderThisFrame(true);
			tableauView.SetClearColor(0U);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000096B7 File Offset: 0x000078B7
		public void SetIsNineGrid(bool value)
		{
			this._isNineGrid = value;
			this._isDirty = true;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000096C7 File Offset: 0x000078C7
		public void SetMeshIndexToUpdate(int value)
		{
			this._meshIndexToUpdate = value;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000096D0 File Offset: 0x000078D0
		public void SetUpdatePositionValueManual(Vec2 value)
		{
			if (this._currentMultiMesh.MeshCount >= 1 && this._meshIndexToUpdate >= 0 && this._meshIndexToUpdate < this._currentMultiMesh.MeshCount)
			{
				Mesh meshAtIndex = this._currentMultiMesh.GetMeshAtIndex(this._meshIndexToUpdate);
				MatrixFrame localFrame = meshAtIndex.GetLocalFrame();
				localFrame.origin.x = 0f;
				localFrame.origin.y = 0f;
				localFrame.origin.x = localFrame.origin.x + value.X / 1528f;
				localFrame.origin.y = localFrame.origin.y - value.Y / 1528f;
				meshAtIndex.SetLocalFrame(localFrame);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009788 File Offset: 0x00007988
		public void SetUpdateSizeValueManual(Vec2 value)
		{
			if (this._currentMultiMesh.MeshCount >= 1 && this._meshIndexToUpdate >= 0 && this._meshIndexToUpdate < this._currentMultiMesh.MeshCount)
			{
				Mesh meshAtIndex = this._currentMultiMesh.GetMeshAtIndex(this._meshIndexToUpdate);
				MatrixFrame localFrame = meshAtIndex.GetLocalFrame();
				float num = value.X / 1528f / meshAtIndex.GetBoundingBoxWidth();
				float num2 = value.Y / 1528f / meshAtIndex.GetBoundingBoxHeight();
				Vec3 eulerAngles = localFrame.rotation.GetEulerAngles();
				localFrame.rotation = Mat3.Identity;
				localFrame.rotation.ApplyEulerAngles(in eulerAngles);
				Vec3 vec = new Vec3(num, num2, 1f, -1f);
				localFrame.rotation.ApplyScaleLocal(in vec);
				meshAtIndex.SetLocalFrame(localFrame);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000985C File Offset: 0x00007A5C
		public void SetUpdateRotationValueManual(ValueTuple<float, bool> value)
		{
			if (this._currentMultiMesh.MeshCount >= 1 && this._meshIndexToUpdate >= 0 && this._meshIndexToUpdate < this._currentMultiMesh.MeshCount)
			{
				Mesh meshAtIndex = this._currentMultiMesh.GetMeshAtIndex(this._meshIndexToUpdate);
				MatrixFrame localFrame = meshAtIndex.GetLocalFrame();
				float num = value.Item1 * 2f * 3.1415927f;
				Vec3 scaleVector = localFrame.rotation.GetScaleVector();
				localFrame.rotation = Mat3.Identity;
				localFrame.rotation.RotateAboutUp(num);
				localFrame.rotation.ApplyScaleLocal(in scaleVector);
				if (value.Item2)
				{
					localFrame.rotation.RotateAboutForward(3.1415927f);
				}
				meshAtIndex.SetLocalFrame(localFrame);
			}
		}

		// Token: 0x0400005E RID: 94
		private static int _tableauIndex;

		// Token: 0x04000060 RID: 96
		private bool _isFinalized;

		// Token: 0x04000061 RID: 97
		private bool _isEnabled;

		// Token: 0x04000062 RID: 98
		private bool _isNineGrid;

		// Token: 0x04000063 RID: 99
		private bool _isDirty;

		// Token: 0x04000064 RID: 100
		private Banner _banner;

		// Token: 0x04000065 RID: 101
		private int _latestWidth = -1;

		// Token: 0x04000066 RID: 102
		private int _latestHeight = -1;

		// Token: 0x04000067 RID: 103
		private int _tableauSizeX;

		// Token: 0x04000068 RID: 104
		private int _tableauSizeY;

		// Token: 0x04000069 RID: 105
		private float RenderScale = 1f;

		// Token: 0x0400006A RID: 106
		private float _customRenderScale = 1f;

		// Token: 0x0400006B RID: 107
		private Scene _scene;

		// Token: 0x0400006C RID: 108
		private Camera _defaultCamera;

		// Token: 0x0400006D RID: 109
		private Camera _nineGridCamera;

		// Token: 0x0400006E RID: 110
		private MetaMesh _currentMultiMesh;

		// Token: 0x0400006F RID: 111
		private GameEntity _currentMeshEntity;

		// Token: 0x04000070 RID: 112
		private int _meshIndexToUpdate;
	}
}
