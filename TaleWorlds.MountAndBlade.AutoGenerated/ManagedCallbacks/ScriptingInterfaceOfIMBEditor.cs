using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000012 RID: 18
	internal class ScriptingInterfaceOfIMBEditor : IMBEditor
	{
		// Token: 0x0600020F RID: 527 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public void ActivateSceneEditorPresentation()
		{
			ScriptingInterfaceOfIMBEditor.call_ActivateSceneEditorPresentationDelegate();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B1B0 File Offset: 0x000093B0
		public void AddEditorWarning(string msg)
		{
			byte[] array = null;
			if (msg != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(msg);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(msg, 0, msg.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_AddEditorWarningDelegate(array);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B20C File Offset: 0x0000940C
		public void AddEntityWarning(UIntPtr entityId, string msg)
		{
			byte[] array = null;
			if (msg != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(msg);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(msg, 0, msg.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_AddEntityWarningDelegate(entityId, array);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B268 File Offset: 0x00009468
		public void AddNavMeshWarning(UIntPtr sceneId, in PathFaceRecord record, string msg)
		{
			byte[] array = null;
			if (msg != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(msg);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(msg, 0, msg.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_AddNavMeshWarningDelegate(sceneId, in record, array);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public void ApplyDeltaToEditorCamera(in Vec3 delta)
		{
			ScriptingInterfaceOfIMBEditor.call_ApplyDeltaToEditorCameraDelegate(in delta);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B2D1 File Offset: 0x000094D1
		public bool BorderHelpersEnabled()
		{
			return ScriptingInterfaceOfIMBEditor.call_BorderHelpersEnabledDelegate();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B2DD File Offset: 0x000094DD
		public void DeactivateSceneEditorPresentation()
		{
			ScriptingInterfaceOfIMBEditor.call_DeactivateSceneEditorPresentationDelegate();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B2E9 File Offset: 0x000094E9
		public void EnterEditMissionMode(UIntPtr missionPointer)
		{
			ScriptingInterfaceOfIMBEditor.call_EnterEditMissionModeDelegate(missionPointer);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B2F6 File Offset: 0x000094F6
		public void EnterEditMode(UIntPtr sceneWidgetPointer, ref MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing)
		{
			ScriptingInterfaceOfIMBEditor.call_EnterEditModeDelegate(sceneWidgetPointer, ref initialCameraFrame, initialCameraElevation, initialCameraBearing);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000B307 File Offset: 0x00009507
		public void ExitEditMode()
		{
			ScriptingInterfaceOfIMBEditor.call_ExitEditModeDelegate();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B314 File Offset: 0x00009514
		public string GetAllPrefabsAndChildWithTag(string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIMBEditor.call_GetAllPrefabsAndChildWithTagDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000B378 File Offset: 0x00009578
		public SceneView GetEditorSceneView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBEditor.call_GetEditorSceneViewDelegate();
			SceneView sceneView = NativeObject.CreateNativeObjectWrapper<SceneView>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return sceneView;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B3B8 File Offset: 0x000095B8
		public bool HelpersEnabled()
		{
			return ScriptingInterfaceOfIMBEditor.call_HelpersEnabledDelegate();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B3C4 File Offset: 0x000095C4
		public bool IsEditMode()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsEditModeDelegate();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B3D0 File Offset: 0x000095D0
		public bool IsEditModeEnabled()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsEditModeEnabledDelegate();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B3DC File Offset: 0x000095DC
		public bool IsEntitySelected(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIMBEditor.call_IsEntitySelectedDelegate(entityId);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B3E9 File Offset: 0x000095E9
		public bool IsReplayManagerRecording()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsReplayManagerRecordingDelegate();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B3F5 File Offset: 0x000095F5
		public bool IsReplayManagerRendering()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsReplayManagerRenderingDelegate();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B401 File Offset: 0x00009601
		public bool IsReplayManagerReplaying()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsReplayManagerReplayingDelegate();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B40D File Offset: 0x0000960D
		public void LeaveEditMissionMode()
		{
			ScriptingInterfaceOfIMBEditor.call_LeaveEditMissionModeDelegate();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B419 File Offset: 0x00009619
		public void LeaveEditMode()
		{
			ScriptingInterfaceOfIMBEditor.call_LeaveEditModeDelegate();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B425 File Offset: 0x00009625
		public void RenderEditorMesh(UIntPtr metaMeshId, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIMBEditor.call_RenderEditorMeshDelegate(metaMeshId, ref frame);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B434 File Offset: 0x00009634
		public void SetLevelVisibility(string cumulated_string)
		{
			byte[] array = null;
			if (cumulated_string != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(cumulated_string);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(cumulated_string, 0, cumulated_string.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_SetLevelVisibilityDelegate(array);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B490 File Offset: 0x00009690
		public void SetUpgradeLevelVisibility(string cumulated_string)
		{
			byte[] array = null;
			if (cumulated_string != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(cumulated_string);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(cumulated_string, 0, cumulated_string.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_SetUpgradeLevelVisibilityDelegate(array);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B4EA File Offset: 0x000096EA
		public void TickEditMode(float dt)
		{
			ScriptingInterfaceOfIMBEditor.call_TickEditModeDelegate(dt);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B4F7 File Offset: 0x000096F7
		public void TickSceneEditorPresentation(float dt)
		{
			ScriptingInterfaceOfIMBEditor.call_TickSceneEditorPresentationDelegate(dt);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B504 File Offset: 0x00009704
		public void ToggleEnableEditorPhysics()
		{
			ScriptingInterfaceOfIMBEditor.call_ToggleEnableEditorPhysicsDelegate();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B510 File Offset: 0x00009710
		public void UpdateSceneTree(bool do_next_frame)
		{
			ScriptingInterfaceOfIMBEditor.call_UpdateSceneTreeDelegate(do_next_frame);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B51D File Offset: 0x0000971D
		public void ZoomToPosition(Vec3 pos)
		{
			ScriptingInterfaceOfIMBEditor.call_ZoomToPositionDelegate(pos);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B53E File Offset: 0x0000973E
		void IMBEditor.ApplyDeltaToEditorCamera(in Vec3 delta)
		{
			this.ApplyDeltaToEditorCamera(in delta);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000B547 File Offset: 0x00009747
		void IMBEditor.AddNavMeshWarning(UIntPtr sceneId, in PathFaceRecord record, string msg)
		{
			this.AddNavMeshWarning(sceneId, in record, msg);
		}

		// Token: 0x04000196 RID: 406
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000197 RID: 407
		public static ScriptingInterfaceOfIMBEditor.ActivateSceneEditorPresentationDelegate call_ActivateSceneEditorPresentationDelegate;

		// Token: 0x04000198 RID: 408
		public static ScriptingInterfaceOfIMBEditor.AddEditorWarningDelegate call_AddEditorWarningDelegate;

		// Token: 0x04000199 RID: 409
		public static ScriptingInterfaceOfIMBEditor.AddEntityWarningDelegate call_AddEntityWarningDelegate;

		// Token: 0x0400019A RID: 410
		public static ScriptingInterfaceOfIMBEditor.AddNavMeshWarningDelegate call_AddNavMeshWarningDelegate;

		// Token: 0x0400019B RID: 411
		public static ScriptingInterfaceOfIMBEditor.ApplyDeltaToEditorCameraDelegate call_ApplyDeltaToEditorCameraDelegate;

		// Token: 0x0400019C RID: 412
		public static ScriptingInterfaceOfIMBEditor.BorderHelpersEnabledDelegate call_BorderHelpersEnabledDelegate;

		// Token: 0x0400019D RID: 413
		public static ScriptingInterfaceOfIMBEditor.DeactivateSceneEditorPresentationDelegate call_DeactivateSceneEditorPresentationDelegate;

		// Token: 0x0400019E RID: 414
		public static ScriptingInterfaceOfIMBEditor.EnterEditMissionModeDelegate call_EnterEditMissionModeDelegate;

		// Token: 0x0400019F RID: 415
		public static ScriptingInterfaceOfIMBEditor.EnterEditModeDelegate call_EnterEditModeDelegate;

		// Token: 0x040001A0 RID: 416
		public static ScriptingInterfaceOfIMBEditor.ExitEditModeDelegate call_ExitEditModeDelegate;

		// Token: 0x040001A1 RID: 417
		public static ScriptingInterfaceOfIMBEditor.GetAllPrefabsAndChildWithTagDelegate call_GetAllPrefabsAndChildWithTagDelegate;

		// Token: 0x040001A2 RID: 418
		public static ScriptingInterfaceOfIMBEditor.GetEditorSceneViewDelegate call_GetEditorSceneViewDelegate;

		// Token: 0x040001A3 RID: 419
		public static ScriptingInterfaceOfIMBEditor.HelpersEnabledDelegate call_HelpersEnabledDelegate;

		// Token: 0x040001A4 RID: 420
		public static ScriptingInterfaceOfIMBEditor.IsEditModeDelegate call_IsEditModeDelegate;

		// Token: 0x040001A5 RID: 421
		public static ScriptingInterfaceOfIMBEditor.IsEditModeEnabledDelegate call_IsEditModeEnabledDelegate;

		// Token: 0x040001A6 RID: 422
		public static ScriptingInterfaceOfIMBEditor.IsEntitySelectedDelegate call_IsEntitySelectedDelegate;

		// Token: 0x040001A7 RID: 423
		public static ScriptingInterfaceOfIMBEditor.IsReplayManagerRecordingDelegate call_IsReplayManagerRecordingDelegate;

		// Token: 0x040001A8 RID: 424
		public static ScriptingInterfaceOfIMBEditor.IsReplayManagerRenderingDelegate call_IsReplayManagerRenderingDelegate;

		// Token: 0x040001A9 RID: 425
		public static ScriptingInterfaceOfIMBEditor.IsReplayManagerReplayingDelegate call_IsReplayManagerReplayingDelegate;

		// Token: 0x040001AA RID: 426
		public static ScriptingInterfaceOfIMBEditor.LeaveEditMissionModeDelegate call_LeaveEditMissionModeDelegate;

		// Token: 0x040001AB RID: 427
		public static ScriptingInterfaceOfIMBEditor.LeaveEditModeDelegate call_LeaveEditModeDelegate;

		// Token: 0x040001AC RID: 428
		public static ScriptingInterfaceOfIMBEditor.RenderEditorMeshDelegate call_RenderEditorMeshDelegate;

		// Token: 0x040001AD RID: 429
		public static ScriptingInterfaceOfIMBEditor.SetLevelVisibilityDelegate call_SetLevelVisibilityDelegate;

		// Token: 0x040001AE RID: 430
		public static ScriptingInterfaceOfIMBEditor.SetUpgradeLevelVisibilityDelegate call_SetUpgradeLevelVisibilityDelegate;

		// Token: 0x040001AF RID: 431
		public static ScriptingInterfaceOfIMBEditor.TickEditModeDelegate call_TickEditModeDelegate;

		// Token: 0x040001B0 RID: 432
		public static ScriptingInterfaceOfIMBEditor.TickSceneEditorPresentationDelegate call_TickSceneEditorPresentationDelegate;

		// Token: 0x040001B1 RID: 433
		public static ScriptingInterfaceOfIMBEditor.ToggleEnableEditorPhysicsDelegate call_ToggleEnableEditorPhysicsDelegate;

		// Token: 0x040001B2 RID: 434
		public static ScriptingInterfaceOfIMBEditor.UpdateSceneTreeDelegate call_UpdateSceneTreeDelegate;

		// Token: 0x040001B3 RID: 435
		public static ScriptingInterfaceOfIMBEditor.ZoomToPositionDelegate call_ZoomToPositionDelegate;

		// Token: 0x020001FE RID: 510
		// (Invoke) Token: 0x06000AE8 RID: 2792
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ActivateSceneEditorPresentationDelegate();

		// Token: 0x020001FF RID: 511
		// (Invoke) Token: 0x06000AEC RID: 2796
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddEditorWarningDelegate(byte[] msg);

		// Token: 0x02000200 RID: 512
		// (Invoke) Token: 0x06000AF0 RID: 2800
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddEntityWarningDelegate(UIntPtr entityId, byte[] msg);

		// Token: 0x02000201 RID: 513
		// (Invoke) Token: 0x06000AF4 RID: 2804
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNavMeshWarningDelegate(UIntPtr sceneId, in PathFaceRecord record, byte[] msg);

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x06000AF8 RID: 2808
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyDeltaToEditorCameraDelegate(in Vec3 delta);

		// Token: 0x02000203 RID: 515
		// (Invoke) Token: 0x06000AFC RID: 2812
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool BorderHelpersEnabledDelegate();

		// Token: 0x02000204 RID: 516
		// (Invoke) Token: 0x06000B00 RID: 2816
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeactivateSceneEditorPresentationDelegate();

		// Token: 0x02000205 RID: 517
		// (Invoke) Token: 0x06000B04 RID: 2820
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnterEditMissionModeDelegate(UIntPtr missionPointer);

		// Token: 0x02000206 RID: 518
		// (Invoke) Token: 0x06000B08 RID: 2824
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnterEditModeDelegate(UIntPtr sceneWidgetPointer, ref MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing);

		// Token: 0x02000207 RID: 519
		// (Invoke) Token: 0x06000B0C RID: 2828
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ExitEditModeDelegate();

		// Token: 0x02000208 RID: 520
		// (Invoke) Token: 0x06000B10 RID: 2832
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAllPrefabsAndChildWithTagDelegate(byte[] tag);

		// Token: 0x02000209 RID: 521
		// (Invoke) Token: 0x06000B14 RID: 2836
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetEditorSceneViewDelegate();

		// Token: 0x0200020A RID: 522
		// (Invoke) Token: 0x06000B18 RID: 2840
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HelpersEnabledDelegate();

		// Token: 0x0200020B RID: 523
		// (Invoke) Token: 0x06000B1C RID: 2844
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEditModeDelegate();

		// Token: 0x0200020C RID: 524
		// (Invoke) Token: 0x06000B20 RID: 2848
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEditModeEnabledDelegate();

		// Token: 0x0200020D RID: 525
		// (Invoke) Token: 0x06000B24 RID: 2852
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEntitySelectedDelegate(UIntPtr entityId);

		// Token: 0x0200020E RID: 526
		// (Invoke) Token: 0x06000B28 RID: 2856
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsReplayManagerRecordingDelegate();

		// Token: 0x0200020F RID: 527
		// (Invoke) Token: 0x06000B2C RID: 2860
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsReplayManagerRenderingDelegate();

		// Token: 0x02000210 RID: 528
		// (Invoke) Token: 0x06000B30 RID: 2864
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsReplayManagerReplayingDelegate();

		// Token: 0x02000211 RID: 529
		// (Invoke) Token: 0x06000B34 RID: 2868
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LeaveEditMissionModeDelegate();

		// Token: 0x02000212 RID: 530
		// (Invoke) Token: 0x06000B38 RID: 2872
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LeaveEditModeDelegate();

		// Token: 0x02000213 RID: 531
		// (Invoke) Token: 0x06000B3C RID: 2876
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderEditorMeshDelegate(UIntPtr metaMeshId, ref MatrixFrame frame);

		// Token: 0x02000214 RID: 532
		// (Invoke) Token: 0x06000B40 RID: 2880
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLevelVisibilityDelegate(byte[] cumulated_string);

		// Token: 0x02000215 RID: 533
		// (Invoke) Token: 0x06000B44 RID: 2884
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUpgradeLevelVisibilityDelegate(byte[] cumulated_string);

		// Token: 0x02000216 RID: 534
		// (Invoke) Token: 0x06000B48 RID: 2888
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickEditModeDelegate(float dt);

		// Token: 0x02000217 RID: 535
		// (Invoke) Token: 0x06000B4C RID: 2892
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickSceneEditorPresentationDelegate(float dt);

		// Token: 0x02000218 RID: 536
		// (Invoke) Token: 0x06000B50 RID: 2896
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ToggleEnableEditorPhysicsDelegate();

		// Token: 0x02000219 RID: 537
		// (Invoke) Token: 0x06000B54 RID: 2900
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateSceneTreeDelegate([MarshalAs(UnmanagedType.U1)] bool do_next_frame);

		// Token: 0x0200021A RID: 538
		// (Invoke) Token: 0x06000B58 RID: 2904
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ZoomToPositionDelegate(Vec3 pos);
	}
}
