using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A0 RID: 416
	public class MBAgentRendererSceneController
	{
		// Token: 0x06001659 RID: 5721 RVA: 0x00052325 File Offset: 0x00050525
		internal MBAgentRendererSceneController(UIntPtr pointer)
		{
			this._pointer = pointer;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00052334 File Offset: 0x00050534
		public void SetEnforcedVisibilityForAllAgents(Scene scene)
		{
			MBAPI.IMBAgentVisuals.SetEnforcedVisibilityForAllAgents(scene.Pointer, this._pointer);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0005234C File Offset: 0x0005054C
		public static MBAgentRendererSceneController CreateNewAgentRendererSceneController(Scene scene)
		{
			return new MBAgentRendererSceneController(MBAPI.IMBAgentVisuals.CreateAgentRendererSceneController(scene.Pointer));
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00052363 File Offset: 0x00050563
		public void SetDoTimerBasedForcedSkeletonUpdates(bool value)
		{
			MBAPI.IMBAgentVisuals.SetDoTimerBasedForcedSkeletonUpdates(this._pointer, value);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00052376 File Offset: 0x00050576
		public static void DestructAgentRendererSceneController(Scene scene, MBAgentRendererSceneController rendererSceneController, bool deleteThisFrame)
		{
			MBAPI.IMBAgentVisuals.DestructAgentRendererSceneController(scene.Pointer, rendererSceneController._pointer, deleteThisFrame);
			rendererSceneController._pointer = UIntPtr.Zero;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0005239A File Offset: 0x0005059A
		public static void ValidateAgentVisualsReseted(Scene scene, MBAgentRendererSceneController rendererSceneController)
		{
			MBAPI.IMBAgentVisuals.ValidateAgentVisualsReseted(scene.Pointer, rendererSceneController._pointer);
		}

		// Token: 0x04000730 RID: 1840
		private UIntPtr _pointer;
	}
}
