using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000085 RID: 133
	[EngineClass("rglScene_view")]
	public class SceneView : View
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		internal SceneView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0000D5C5 File Offset: 0x0000B7C5
		public static SceneView CreateSceneView()
		{
			return EngineApplicationInterface.ISceneView.CreateSceneView();
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0000D5D1 File Offset: 0x0000B7D1
		public void SetScene(Scene scene)
		{
			EngineApplicationInterface.ISceneView.SetScene(base.Pointer, scene.Pointer);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0000D5E9 File Offset: 0x0000B7E9
		public void SetAcceptGlobalDebugRenderObjects(bool value)
		{
			EngineApplicationInterface.ISceneView.SetAcceptGlobalDebugRenderObjects(base.Pointer, value);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		public void SetRenderWithPostfx(bool value)
		{
			EngineApplicationInterface.ISceneView.SetRenderWithPostfx(base.Pointer, value);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0000D60F File Offset: 0x0000B80F
		public void SetPostfxConfigParams(int value)
		{
			EngineApplicationInterface.ISceneView.SetPostfxConfigParams(base.Pointer, value);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0000D622 File Offset: 0x0000B822
		public void SetForceShaderCompilation(bool value)
		{
			EngineApplicationInterface.ISceneView.SetForceShaderCompilation(base.Pointer, value);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0000D635 File Offset: 0x0000B835
		public bool CheckSceneReadyToRender()
		{
			return EngineApplicationInterface.ISceneView.CheckSceneReadyToRender(base.Pointer);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0000D647 File Offset: 0x0000B847
		public void SetDoQuickExposure(bool value)
		{
			EngineApplicationInterface.ISceneView.SetDoQuickExposure(base.Pointer, value);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0000D65A File Offset: 0x0000B85A
		public void SetCamera(Camera camera)
		{
			EngineApplicationInterface.ISceneView.SetCamera(base.Pointer, camera.Pointer);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0000D672 File Offset: 0x0000B872
		public void SetResolutionScaling(bool value)
		{
			EngineApplicationInterface.ISceneView.SetResolutionScaling(base.Pointer, value);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0000D685 File Offset: 0x0000B885
		public void SetPostfxFromConfig()
		{
			EngineApplicationInterface.ISceneView.SetPostfxFromConfig(base.Pointer);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0000D697 File Offset: 0x0000B897
		public Vec2 WorldPointToScreenPoint(Vec3 position)
		{
			return EngineApplicationInterface.ISceneView.WorldPointToScreenPoint(base.Pointer, position);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0000D6AA File Offset: 0x0000B8AA
		public Vec2 ScreenPointToViewportPoint(Vec2 position)
		{
			return EngineApplicationInterface.ISceneView.ScreenPointToViewportPoint(base.Pointer, position.x, position.y);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public bool ProjectedMousePositionOnGround(out Vec3 groundPosition, out Vec3 groundNormal, bool mouseVisible, BodyFlags excludeBodyOwnerFlags, bool checkOccludedSurface)
		{
			return EngineApplicationInterface.ISceneView.ProjectedMousePositionOnGround(base.Pointer, out groundPosition, out groundNormal, mouseVisible, excludeBodyOwnerFlags, checkOccludedSurface);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0000D6E1 File Offset: 0x0000B8E1
		public bool ProjectedMousePositionOnWater(out Vec3 waterPosition, bool mouseVisible)
		{
			return EngineApplicationInterface.ISceneView.ProjectedMousePositionOnWater(base.Pointer, out waterPosition, mouseVisible);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0000D6F5 File Offset: 0x0000B8F5
		public void TranslateMouse(ref Vec3 worldMouseNear, ref Vec3 worldMouseFar, float maxDistance = -1f)
		{
			EngineApplicationInterface.ISceneView.TranslateMouse(base.Pointer, ref worldMouseNear, ref worldMouseFar, maxDistance);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0000D70A File Offset: 0x0000B90A
		public void SetSceneUsesSkybox(bool value)
		{
			EngineApplicationInterface.ISceneView.SetSceneUsesSkybox(base.Pointer, value);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0000D71D File Offset: 0x0000B91D
		public void SetSceneUsesShadows(bool value)
		{
			EngineApplicationInterface.ISceneView.SetSceneUsesShadows(base.Pointer, value);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0000D730 File Offset: 0x0000B930
		public void SetSceneUsesContour(bool value)
		{
			EngineApplicationInterface.ISceneView.SetSceneUsesContour(base.Pointer, value);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0000D743 File Offset: 0x0000B943
		public void DoNotClear(bool value)
		{
			EngineApplicationInterface.ISceneView.DoNotClear(base.Pointer, value);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0000D756 File Offset: 0x0000B956
		public void AddClearTask(bool clearOnlySceneview = false)
		{
			EngineApplicationInterface.ISceneView.AddClearTask(base.Pointer, clearOnlySceneview);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0000D769 File Offset: 0x0000B969
		public bool ReadyToRender()
		{
			return EngineApplicationInterface.ISceneView.ReadyToRender(base.Pointer);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0000D77B File Offset: 0x0000B97B
		public void SetClearAndDisableAfterSucessfullRender(bool value)
		{
			EngineApplicationInterface.ISceneView.SetClearAndDisableAfterSucessfullRender(base.Pointer, value);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0000D78E File Offset: 0x0000B98E
		public void SetClearGbuffer(bool value)
		{
			EngineApplicationInterface.ISceneView.SetClearGbuffer(base.Pointer, value);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0000D7A1 File Offset: 0x0000B9A1
		public void SetShadowmapResolutionMultiplier(float value)
		{
			EngineApplicationInterface.ISceneView.SetShadowmapResolutionMultiplier(base.Pointer, value);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		public void SetPointlightResolutionMultiplier(float value)
		{
			EngineApplicationInterface.ISceneView.SetPointlightResolutionMultiplier(base.Pointer, value);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0000D7C7 File Offset: 0x0000B9C7
		public void SetCleanScreenUntilLoadingDone(bool value)
		{
			EngineApplicationInterface.ISceneView.SetCleanScreenUntilLoadingDone(base.Pointer, value);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0000D7DA File Offset: 0x0000B9DA
		public void ClearAll(bool clearScene, bool removeTerrain)
		{
			EngineApplicationInterface.ISceneView.ClearAll(base.Pointer, clearScene, removeTerrain);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0000D7EE File Offset: 0x0000B9EE
		public void SetFocusedShadowmap(bool enable, ref Vec3 center, float radius)
		{
			EngineApplicationInterface.ISceneView.SetFocusedShadowmap(base.Pointer, enable, ref center, radius);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0000D803 File Offset: 0x0000BA03
		public Scene GetScene()
		{
			return EngineApplicationInterface.ISceneView.GetScene(base.Pointer);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0000D818 File Offset: 0x0000BA18
		public bool RayCastForClosestEntityOrTerrain(Vec3 sourcePoint, Vec3 targetPoint, out float collisionDistance, out Vec3 closestPoint, float rayThickness = 0.01f, BodyFlags excludeBodyFlags = BodyFlags.CommonFocusRayCastExcludeFlags)
		{
			collisionDistance = float.NaN;
			closestPoint = Vec3.Invalid;
			UIntPtr zero = UIntPtr.Zero;
			return EngineApplicationInterface.ISceneView.RayCastForClosestEntityOrTerrain(base.Pointer, ref sourcePoint, ref targetPoint, rayThickness, ref collisionDistance, ref closestPoint, ref zero, excludeBodyFlags);
		}
	}
}
