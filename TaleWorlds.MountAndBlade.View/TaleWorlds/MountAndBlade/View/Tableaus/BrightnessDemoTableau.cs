using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000033 RID: 51
	public class BrightnessDemoTableau
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000AE1D File Offset: 0x0000901D
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000AE25 File Offset: 0x00009025
		public Texture Texture { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000AE2E File Offset: 0x0000902E
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

		// Token: 0x06000189 RID: 393 RVA: 0x0000AE65 File Offset: 0x00009065
		private void SetEnabled(bool enabled)
		{
			this._isEnabled = enabled;
			TableauView view = this.View;
			if (!this._initialized)
			{
				this.SetScene();
			}
			if (view == null)
			{
				return;
			}
			view.SetEnable(this._isEnabled);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000AE92 File Offset: 0x00009092
		public void SetDemoType(int demoType)
		{
			this._demoType = demoType;
			this._initialized = false;
			this.RefreshDemoTableau();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public void SetTargetSize(int width, int height)
		{
			int num;
			int num2;
			if (width <= 0 || height <= 0)
			{
				num = 10;
				num2 = 10;
			}
			else
			{
				this.RenderScale = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ResolutionScale) / 100f;
				num = (int)((float)width * this.RenderScale);
				num2 = (int)((float)height * this.RenderScale);
			}
			if (num != this._tableauSizeX || num2 != this._tableauSizeY)
			{
				this._tableauSizeX = num;
				this._tableauSizeY = num2;
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
				this.Texture = TableauView.AddTableau(string.Format("BrightnessDemo_{0}", BrightnessDemoTableau._tableauIndex++), new RenderTargetComponent.TextureUpdateEventHandler(this.SceneTableauContinuousRenderFunction), this._tableauScene, this._tableauSizeX, this._tableauSizeY);
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000AF90 File Offset: 0x00009190
		public void OnFinalize()
		{
			if (this._continuousRenderCamera != null)
			{
				this._continuousRenderCamera.ReleaseCameraEntity();
				this._continuousRenderCamera = null;
			}
			TableauView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			TableauView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(false);
			}
			this.Texture = null;
			Scene tableauScene = this._tableauScene;
			if (tableauScene != null)
			{
				tableauScene.ManualInvalidate();
			}
			this._tableauScene = null;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B000 File Offset: 0x00009200
		public void SetScene()
		{
			this._tableauScene = Scene.CreateNewScene(true, true, DecalAtlasGroup.All, "mono_renderscene");
			switch (this._demoType)
			{
			case 0:
				this._demoTexture = Texture.GetFromResource("brightness_calibration_wide");
				this._tableauScene.SetAtmosphereWithName("brightness_calibration_screen");
				break;
			case 1:
				this._demoTexture = Texture.GetFromResource("calibration_image_1");
				this._tableauScene.SetAtmosphereWithName("TOD_11_00_SemiCloudy");
				break;
			case 2:
				this._demoTexture = Texture.GetFromResource("calibration_image_2");
				this._tableauScene.SetAtmosphereWithName("TOD_05_00_SemiCloudy");
				break;
			case 3:
				this._demoTexture = Texture.GetFromResource("calibration_image_3");
				this._tableauScene.SetAtmosphereWithName("TOD_05_00_SemiCloudy");
				break;
			case 4:
				this._demoTexture = Texture.GetFromResource("naval_calibration_0");
				this._tableauScene.SetAtmosphereWithName("TOD_naval_10_00_SemiCloudy");
				break;
			case 5:
				this._demoTexture = Texture.GetFromResource("naval_calibration_1");
				this._tableauScene.SetAtmosphereWithName("TOD_naval_06_00_sunset");
				break;
			case 6:
				this._demoTexture = Texture.GetFromResource("naval_calibration_2");
				this._tableauScene.SetAtmosphereWithName("TOD_naval_08_00_rain_storm");
				break;
			default:
				Debug.FailedAssert(string.Format("Undefined Brightness demo type({0})", this._demoType), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\BrightnessDemoTableau.cs", "SetScene", 145);
				break;
			}
			this._tableauScene.SetDepthOfFieldParameters(0f, 0f, false);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B187 File Offset: 0x00009387
		private void RefreshDemoTableau()
		{
			if (!this._initialized)
			{
				this.SetEnabled(true);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000B198 File Offset: 0x00009398
		public void OnTick(float dt)
		{
			if (this._continuousRenderCamera == null)
			{
				this._continuousRenderCamera = Camera.CreateCamera();
			}
			TableauView view = this.View;
			if (view == null)
			{
				return;
			}
			view.SetDoNotRenderThisFrame(false);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000B1C4 File Offset: 0x000093C4
		internal void SceneTableauContinuousRenderFunction(Texture sender, EventArgs e)
		{
			Scene scene = (Scene)sender.UserData;
			TableauView tableauView = sender.TableauView;
			tableauView.SetEnable(true);
			if (scene == null)
			{
				tableauView.SetContinuousRendering(false);
				tableauView.SetDeleteAfterRendering(true);
				return;
			}
			scene.SetShadow(false);
			scene.EnsurePostfxSystem();
			scene.SetDofMode(false);
			scene.SetMotionBlurMode(false);
			scene.SetBloom(true);
			scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.31f);
			scene.SetExternalInjectionTexture(this._demoTexture);
			tableauView.SetRenderWithPostfx(true);
			tableauView.SetDoQuickExposure(true);
			if (this._continuousRenderCamera != null)
			{
				Camera continuousRenderCamera = this._continuousRenderCamera;
				tableauView.SetCamera(continuousRenderCamera);
				tableauView.SetScene(scene);
				tableauView.SetSceneUsesSkybox(false);
				tableauView.SetDeleteAfterRendering(false);
				tableauView.SetContinuousRendering(true);
				tableauView.SetDoNotRenderThisFrame(true);
				tableauView.SetClearColor(4278190080U);
				tableauView.SetFocusedShadowmap(true, ref this._frame.origin, 1.55f);
			}
		}

		// Token: 0x040000AC RID: 172
		private static int _tableauIndex;

		// Token: 0x040000AE RID: 174
		private MatrixFrame _frame;

		// Token: 0x040000AF RID: 175
		private Scene _tableauScene;

		// Token: 0x040000B0 RID: 176
		private Texture _demoTexture;

		// Token: 0x040000B1 RID: 177
		private Camera _continuousRenderCamera;

		// Token: 0x040000B2 RID: 178
		private bool _initialized;

		// Token: 0x040000B3 RID: 179
		private int _tableauSizeX;

		// Token: 0x040000B4 RID: 180
		private int _tableauSizeY;

		// Token: 0x040000B5 RID: 181
		private int _demoType = -1;

		// Token: 0x040000B6 RID: 182
		private bool _isEnabled;

		// Token: 0x040000B7 RID: 183
		private float RenderScale = 1f;
	}
}
