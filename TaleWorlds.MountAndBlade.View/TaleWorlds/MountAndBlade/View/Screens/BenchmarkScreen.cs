using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000050 RID: 80
	public class BenchmarkScreen : ScreenBase
	{
		// Token: 0x060002AE RID: 686 RVA: 0x00011DDC File Offset: 0x0000FFDC
		protected override void OnActivate()
		{
			base.OnActivate();
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.All, "mono_renderscene");
			this._scene.SetName("BenchmarkScreen");
			this._scene.Read("benchmark");
			this._cameraFrame = this._scene.ReadAndCalculateInitialCamera();
			this._scene.SetUseConstantTime(true);
			this._sceneView = SceneView.CreateSceneView();
			this._sceneView.SetScene(this._scene);
			this._sceneView.SetSceneUsesShadows(true);
			this._camera = Camera.CreateCamera();
			this.UpdateCamera();
			this._cameraTimer = new Timer(MBCommon.GetApplicationTime() - 5f, 5f, true);
			GameEntity gameEntity = this._scene.FindEntityWithName("LocationEntityParent");
			this._cameraLocationEntities = gameEntity.GetChildren().ToList<GameEntity>();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00011EB6 File Offset: 0x000100B6
		public void UpdateCamera()
		{
			this._camera.Frame = this._cameraFrame;
			this._sceneView.SetCamera(this._camera);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00011EDA File Offset: 0x000100DA
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._scene = null;
			this._analyzer = null;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00011EF0 File Offset: 0x000100F0
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._cameraTimer.Check(MBCommon.GetApplicationTime()))
			{
				this._currentEntityIndex++;
				if (this._currentEntityIndex >= this._cameraLocationEntities.Count)
				{
					this._analyzer.FinalizeAndWrite("../../../Tools/TestAutomation/Attachments/benchmark_scene_performance.xml");
					ScreenManager.PopScreen();
					return;
				}
				GameEntity gameEntity = this._cameraLocationEntities[this._currentEntityIndex];
				this._cameraFrame = gameEntity.GetGlobalFrame();
				this.UpdateCamera();
				this._analyzer.Start(gameEntity.Name);
				this._cameraTimer.Reset(MBCommon.GetApplicationTime());
			}
			this._analyzer.Tick(dt);
		}

		// Token: 0x0400015C RID: 348
		private SceneView _sceneView;

		// Token: 0x0400015D RID: 349
		private Scene _scene;

		// Token: 0x0400015E RID: 350
		private Camera _camera;

		// Token: 0x0400015F RID: 351
		private MatrixFrame _cameraFrame;

		// Token: 0x04000160 RID: 352
		private Timer _cameraTimer;

		// Token: 0x04000161 RID: 353
		private const string _parentEntityName = "LocationEntityParent";

		// Token: 0x04000162 RID: 354
		private const string _sceneName = "benchmark";

		// Token: 0x04000163 RID: 355
		private const string _xmlPath = "../../../Tools/TestAutomation/Attachments/benchmark_scene_performance.xml";

		// Token: 0x04000164 RID: 356
		private List<GameEntity> _cameraLocationEntities;

		// Token: 0x04000165 RID: 357
		private int _currentEntityIndex = -1;

		// Token: 0x04000166 RID: 358
		private PerformanceAnalyzer _analyzer = new PerformanceAnalyzer();
	}
}
