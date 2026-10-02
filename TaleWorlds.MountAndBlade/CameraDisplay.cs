using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000331 RID: 817
	public class CameraDisplay : ScriptComponentBehavior
	{
		// Token: 0x06002E7E RID: 11902 RVA: 0x000B41B4 File Offset: 0x000B23B4
		private void BuildView()
		{
			this._sceneView = SceneView.CreateSceneView();
			this._myCamera = Camera.CreateCamera();
			this._sceneView.SetScene(base.GameEntity.Scene);
			this._sceneView.SetPostfxFromConfig();
			this._sceneView.SetRenderOption(View.ViewRenderOptions.ClearColor, false);
			this._sceneView.SetRenderOption(View.ViewRenderOptions.ClearDepth, true);
			this._sceneView.SetScale(new Vec2(0.2f, 0.2f));
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000B4230 File Offset: 0x000B2430
		private void SetCamera()
		{
			Vec2 realScreenResolution = Screen.RealScreenResolution;
			float num = realScreenResolution.x / realScreenResolution.y;
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			this._myCamera.SetFovVertical(0.7853982f, num, 0.2f, 200f);
			this._myCamera.Frame = globalFrame;
			this._sceneView.SetCamera(this._myCamera);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000B4298 File Offset: 0x000B2498
		private void RenderCameraFrustrum()
		{
			this._myCamera.RenderFrustrum();
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000B42A5 File Offset: 0x000B24A5
		protected internal override void OnEditorInit()
		{
			this.BuildView();
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000B42AD File Offset: 0x000B24AD
		protected internal override void OnInit()
		{
			this.BuildView();
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000B42B5 File Offset: 0x000B24B5
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				this.RenderCameraFrustrum();
				this._sceneView.SetEnable(true);
				return;
			}
			this._sceneView.SetEnable(false);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000B42EA File Offset: 0x000B24EA
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this._sceneView = null;
			this._myCamera = null;
		}

		// Token: 0x0400125A RID: 4698
		private Camera _myCamera;

		// Token: 0x0400125B RID: 4699
		private SceneView _sceneView;

		// Token: 0x0400125C RID: 4700
		public int renderOrder;
	}
}
