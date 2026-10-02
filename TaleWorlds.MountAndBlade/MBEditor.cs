using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001CB RID: 459
	public class MBEditor
	{
		// Token: 0x06001BB3 RID: 7091 RVA: 0x0006068C File Offset: 0x0005E88C
		[MBCallback(null, false)]
		internal static void SetEditorScene(Scene scene)
		{
			if (MBEditor._editorScene != null)
			{
				if (MBEditor._agentRendererSceneController != null)
				{
					MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
				}
				MBEditor._editorScene.ClearAll();
			}
			MBEditor._editorScene = scene;
			MBEditor._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(MBEditor._editorScene);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000606DC File Offset: 0x0005E8DC
		[MBCallback(null, false)]
		internal static void CloseEditorScene()
		{
			if (MBEditor._agentRendererSceneController != null)
			{
				MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
			}
			MBEditor._agentRendererSceneController = null;
			MBEditor._editorScene = null;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00060701 File Offset: 0x0005E901
		[MBCallback(null, false)]
		internal static void DestroyEditor(Scene scene)
		{
			MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
			MBEditor._editorScene.ClearAll();
			MBEditor._editorScene = null;
			MBEditor._agentRendererSceneController = null;
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x00060729 File Offset: 0x0005E929
		public static bool IsEditModeOn
		{
			get
			{
				return MBAPI.IMBEditor.IsEditMode();
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x00060735 File Offset: 0x0005E935
		public static bool EditModeEnabled
		{
			get
			{
				return MBAPI.IMBEditor.IsEditModeEnabled();
			}
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x00060741 File Offset: 0x0005E941
		public static void UpdateSceneTree(bool doNextFrame)
		{
			MBAPI.IMBEditor.UpdateSceneTree(doNextFrame);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0006074E File Offset: 0x0005E94E
		public static bool IsEntitySelected(GameEntity entity)
		{
			return MBAPI.IMBEditor.IsEntitySelected(entity.Pointer);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00060760 File Offset: 0x0005E960
		public static bool IsEntitySelected(WeakGameEntity entity)
		{
			return MBAPI.IMBEditor.IsEntitySelected(entity.Pointer);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x00060773 File Offset: 0x0005E973
		public static void RenderEditorMesh(MetaMesh mesh, MatrixFrame frame)
		{
			MBAPI.IMBEditor.RenderEditorMesh(mesh.Pointer, ref frame);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x00060787 File Offset: 0x0005E987
		public static void ApplyDeltaToEditorCamera(Vec3 delta)
		{
			MBAPI.IMBEditor.ApplyDeltaToEditorCamera(in delta);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x00060795 File Offset: 0x0005E995
		public static void EnterEditMode(SceneView sceneView, MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing)
		{
			MBAPI.IMBEditor.EnterEditMode(sceneView.Pointer, ref initialCameraFrame, initialCameraElevation, initialCameraBearing);
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000607AB File Offset: 0x0005E9AB
		public static void TickEditMode(float dt)
		{
			MBAPI.IMBEditor.TickEditMode(dt);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000607B8 File Offset: 0x0005E9B8
		public static void LeaveEditMode()
		{
			MBAPI.IMBEditor.LeaveEditMode();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
			MBEditor._agentRendererSceneController = null;
			MBEditor._editorScene = null;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000607E0 File Offset: 0x0005E9E0
		public static void EnterEditMissionMode(Mission mission)
		{
			MBAPI.IMBEditor.EnterEditMissionMode(mission.Pointer);
			MBEditor._isEditorMissionOn = true;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000607F8 File Offset: 0x0005E9F8
		public static void LeaveEditMissionMode()
		{
			MBAPI.IMBEditor.LeaveEditMissionMode();
			MBEditor._isEditorMissionOn = false;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x0006080A File Offset: 0x0005EA0A
		public static bool IsEditorMissionOn()
		{
			return MBEditor._isEditorMissionOn && MBEditor.IsEditModeOn;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0006081C File Offset: 0x0005EA1C
		public static void ActivateSceneEditorPresentation()
		{
			Monster.GetBoneIndexWithId = new Func<string, string, sbyte>(MBActionSet.GetBoneIndexWithId);
			Monster.GetBoneHasParentBone = new Func<string, sbyte, bool>(MBActionSet.GetBoneHasParentBone);
			MBObjectManager.Init();
			MBObjectManager.Instance.RegisterType<Monster>("Monster", "Monsters", 2U, true, false);
			MBObjectManager.Instance.LoadXML("Monsters", true);
			MBAPI.IMBEditor.ActivateSceneEditorPresentation();
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x00060882 File Offset: 0x0005EA82
		public static void DeactivateSceneEditorPresentation()
		{
			MBAPI.IMBEditor.DeactivateSceneEditorPresentation();
			MBObjectManager.Instance.Destroy();
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x00060898 File Offset: 0x0005EA98
		public static void TickSceneEditorPresentation(float dt)
		{
			MBAPI.IMBEditor.TickSceneEditorPresentation(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000608AA File Offset: 0x0005EAAA
		public static SceneView GetEditorSceneView()
		{
			return MBAPI.IMBEditor.GetEditorSceneView();
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000608B6 File Offset: 0x0005EAB6
		public static bool HelpersEnabled()
		{
			return MBAPI.IMBEditor.HelpersEnabled();
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000608C2 File Offset: 0x0005EAC2
		public static bool BorderHelpersEnabled()
		{
			return MBAPI.IMBEditor.BorderHelpersEnabled();
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000608CE File Offset: 0x0005EACE
		public static void ZoomToPosition(Vec3 pos)
		{
			MBAPI.IMBEditor.ZoomToPosition(pos);
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000608DB File Offset: 0x0005EADB
		public static bool IsReplayManagerReplaying()
		{
			return MBAPI.IMBEditor.IsReplayManagerReplaying();
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000608E7 File Offset: 0x0005EAE7
		public static bool IsReplayManagerRendering()
		{
			return MBAPI.IMBEditor.IsReplayManagerRendering();
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000608F3 File Offset: 0x0005EAF3
		public static bool IsReplayManagerRecording()
		{
			return MBAPI.IMBEditor.IsReplayManagerRecording();
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000608FF File Offset: 0x0005EAFF
		public static void AddEditorWarning(string msg)
		{
			MBAPI.IMBEditor.AddEditorWarning(msg);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0006090C File Offset: 0x0005EB0C
		public static void AddEntityWarning(WeakGameEntity entityId, string msg)
		{
			MBAPI.IMBEditor.AddEntityWarning(entityId.Pointer, msg);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00060920 File Offset: 0x0005EB20
		public static void AddNavMeshWarning(Scene scene, PathFaceRecord record, string msg)
		{
			MBAPI.IMBEditor.AddNavMeshWarning(scene.Pointer, in record, msg);
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x00060935 File Offset: 0x0005EB35
		public static string GetAllPrefabsAndChildWithTag(string tag)
		{
			return MBAPI.IMBEditor.GetAllPrefabsAndChildWithTag(tag);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x00060942 File Offset: 0x0005EB42
		public static void ExitEditMode()
		{
			MBAPI.IMBEditor.ExitEditMode();
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x00060950 File Offset: 0x0005EB50
		public static void SetUpgradeLevelVisibility(List<string> levels)
		{
			string text = "";
			for (int i = 0; i < levels.Count - 1; i++)
			{
				text = text + levels[i] + "|";
			}
			text += levels[levels.Count - 1];
			MBAPI.IMBEditor.SetUpgradeLevelVisibility(text);
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000609A9 File Offset: 0x0005EBA9
		public static void SetLevelVisibility(List<string> levels)
		{
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000609AB File Offset: 0x0005EBAB
		public static void ToggleEnableEditorPhysics()
		{
			MBAPI.IMBEditor.ToggleEnableEditorPhysics();
		}

		// Token: 0x0400091E RID: 2334
		public static Scene _editorScene;

		// Token: 0x0400091F RID: 2335
		private static MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x04000920 RID: 2336
		public static bool _isEditorMissionOn;
	}
}
