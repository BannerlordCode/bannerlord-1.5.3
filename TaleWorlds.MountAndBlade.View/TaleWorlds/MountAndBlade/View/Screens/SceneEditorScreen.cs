using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000059 RID: 89
	[GameStateScreen(typeof(EditorState))]
	public class SceneEditorScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000364 RID: 868 RVA: 0x00019E4F File Offset: 0x0001804F
		public SceneEditorScreen(EditorState editorState)
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00019E58 File Offset: 0x00018058
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._editorLayer = new SceneEditorLayer();
			this._editorLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Invalid);
			base.AddLayer(this._editorLayer);
			ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_core_parameters"));
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00019EAD File Offset: 0x000180AD
		protected override void OnActivate()
		{
			base.OnActivate();
			MouseManager.ActivateMouseCursor(CursorType.System);
			MBEditor.ActivateSceneEditorPresentation();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00019EC0 File Offset: 0x000180C0
		protected override void OnDeactivate()
		{
			MBEditor.DeactivateSceneEditorPresentation();
			MouseManager.ActivateMouseCursor(CursorType.Default);
			base.OnDeactivate();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00019ED4 File Offset: 0x000180D4
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._editorLayer != null)
			{
				bool mouseVisible = Screen.GetMouseVisible();
				this._editorLayer.InputRestrictions.SetMouseVisibility(mouseVisible);
			}
			MBEditor.TickSceneEditorPresentation(dt);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00019F0D File Offset: 0x0001810D
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00019F0F File Offset: 0x0001810F
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00019F11 File Offset: 0x00018111
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00019F13 File Offset: 0x00018113
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x040001CE RID: 462
		private SceneEditorLayer _editorLayer;
	}
}
