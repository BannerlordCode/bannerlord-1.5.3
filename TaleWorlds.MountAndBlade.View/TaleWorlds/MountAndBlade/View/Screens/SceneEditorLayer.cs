using System;
using TaleWorlds.Engine;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x0200005A RID: 90
	public class SceneEditorLayer : ScreenLayer
	{
		// Token: 0x0600036D RID: 877 RVA: 0x00019F15 File Offset: 0x00018115
		public SceneEditorLayer()
			: base("SceneEditorLayer", -100)
		{
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00019F24 File Offset: 0x00018124
		protected override void OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00019F2C File Offset: 0x0001812C
		protected override void Tick(float dt)
		{
			base.Tick(dt);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00019F35 File Offset: 0x00018135
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00019F40 File Offset: 0x00018140
		protected override void RefreshGlobalOrder(ref int currentOrder)
		{
			SceneView editorSceneView = MBEditor.GetEditorSceneView();
			if (editorSceneView != null)
			{
				editorSceneView.SetRenderOrder(currentOrder);
				currentOrder++;
			}
		}
	}
}
