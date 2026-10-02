using System;
using System.Numerics;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine.Screens
{
	// Token: 0x020000A4 RID: 164
	public class SceneLayer : ScreenLayer
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x00012120 File Offset: 0x00010320
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x00012128 File Offset: 0x00010328
		public bool ClearSceneOnFinalize { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x00012131 File Offset: 0x00010331
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x00012139 File Offset: 0x00010339
		public bool AutoToggleSceneView { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x00012142 File Offset: 0x00010342
		public SceneView SceneView
		{
			get
			{
				return this._sceneView;
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0001214A File Offset: 0x0001034A
		public SceneLayer(bool clearSceneOnFinalize = true, bool autoToggleSceneView = true)
			: base("SceneLayer", -100)
		{
			this.ClearSceneOnFinalize = clearSceneOnFinalize;
			base.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			this._sceneView = SceneView.CreateSceneView();
			this.AutoToggleSceneView = autoToggleSceneView;
			base.IsFocusLayer = true;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00012186 File Offset: 0x00010386
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this.AutoToggleSceneView)
			{
				this._sceneView.SetEnable(true);
			}
			ScreenManager.TrySetFocus(this);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000121A8 File Offset: 0x000103A8
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (this.AutoToggleSceneView)
			{
				this._sceneView.SetEnable(false);
			}
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000121C4 File Offset: 0x000103C4
		protected override void OnFinalize()
		{
			if (this.ClearSceneOnFinalize)
			{
				this._sceneView.ClearAll(true, true);
			}
			base.OnFinalize();
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000121E1 File Offset: 0x000103E1
		public void SetScene(Scene scene)
		{
			this._sceneView.SetScene(scene);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000121EF File Offset: 0x000103EF
		public void SetRenderWithPostfx(bool value)
		{
			this._sceneView.SetRenderWithPostfx(value);
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x000121FD File Offset: 0x000103FD
		public void SetPostfxConfigParams(int value)
		{
			this._sceneView.SetPostfxConfigParams(value);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0001220B File Offset: 0x0001040B
		public void SetCamera(Camera camera)
		{
			this._sceneView.SetCamera(camera);
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00012219 File Offset: 0x00010419
		public void SetPostfxFromConfig()
		{
			this._sceneView.SetPostfxFromConfig();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00012226 File Offset: 0x00010426
		public Vec2 WorldPointToScreenPoint(Vec3 position)
		{
			return this._sceneView.WorldPointToScreenPoint(position);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00012234 File Offset: 0x00010434
		public Vec2 ScreenPointToViewportPoint(Vec2 position)
		{
			return this._sceneView.ScreenPointToViewportPoint(position);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00012242 File Offset: 0x00010442
		public bool ProjectedMousePositionOnGround(out Vec3 groundPosition, out Vec3 groundNormal, bool mouseVisible, BodyFlags excludeBodyOwnerFlags, bool checkOccludedSurface)
		{
			return this._sceneView.ProjectedMousePositionOnGround(out groundPosition, out groundNormal, mouseVisible, excludeBodyOwnerFlags, checkOccludedSurface);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00012256 File Offset: 0x00010456
		public void TranslateMouse(ref Vec3 worldMouseNear, ref Vec3 worldMouseFar, float maxDistance = -1f)
		{
			this._sceneView.TranslateMouse(ref worldMouseNear, ref worldMouseFar, maxDistance);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00012266 File Offset: 0x00010466
		public void SetSceneUsesSkybox(bool value)
		{
			this._sceneView.SetSceneUsesSkybox(value);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00012274 File Offset: 0x00010474
		public void SetSceneUsesShadows(bool value)
		{
			this._sceneView.SetSceneUsesShadows(value);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00012282 File Offset: 0x00010482
		public void SetSceneUsesContour(bool value)
		{
			this._sceneView.SetSceneUsesContour(value);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00012290 File Offset: 0x00010490
		public void SetShadowmapResolutionMultiplier(float value)
		{
			this._sceneView.SetShadowmapResolutionMultiplier(value);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0001229E File Offset: 0x0001049E
		public void SetFocusedShadowmap(bool enable, ref Vec3 center, float radius)
		{
			this._sceneView.SetFocusedShadowmap(enable, ref center, radius);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x000122AE File Offset: 0x000104AE
		public void DoNotClear(bool value)
		{
			this._sceneView.DoNotClear(value);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x000122BC File Offset: 0x000104BC
		public bool ReadyToRender()
		{
			return this._sceneView.ReadyToRender();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x000122C9 File Offset: 0x000104C9
		public void SetCleanScreenUntilLoadingDone(bool value)
		{
			this._sceneView.SetCleanScreenUntilLoadingDone(value);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x000122D7 File Offset: 0x000104D7
		public void ClearAll()
		{
			this._sceneView.ClearAll(true, true);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x000122E6 File Offset: 0x000104E6
		public void ClearRuntimeGPUMemory(bool remove_terrain)
		{
			this._sceneView.ClearAll(false, remove_terrain);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x000122F5 File Offset: 0x000104F5
		protected override void RefreshGlobalOrder(ref int currentOrder)
		{
			this._sceneView.SetRenderOrder(currentOrder);
			currentOrder++;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0001230C File Offset: 0x0001050C
		public override bool HitTest(Vector2 position)
		{
			bool flag = position.X >= 0f && position.X < Screen.RealScreenResolutionWidth;
			bool flag2 = position.Y >= 0f && position.Y < Screen.RealScreenResolutionHeight;
			return flag && flag2;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00012358 File Offset: 0x00010558
		public override bool HitTest()
		{
			Vector2 vector = (Vector2)base.Input.GetMousePositionPixel();
			bool flag = vector.X >= 0f && vector.X < Screen.RealScreenResolutionWidth;
			bool flag2 = vector.Y >= 0f && vector.Y < Screen.RealScreenResolutionHeight;
			return flag && flag2;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x000123B3 File Offset: 0x000105B3
		public override bool FocusTest()
		{
			return true;
		}

		// Token: 0x0400021C RID: 540
		private SceneView _sceneView;
	}
}
