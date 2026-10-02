using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000057 RID: 87
	[GameStateScreen(typeof(MissionState))]
	public class MissionScreen : ScreenBase, IMissionSystemHandler, IGameStateListener, IMissionScreen, IMissionListener, IChatLogHandlerScreen
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002CC RID: 716 RVA: 0x000123E1 File Offset: 0x000105E1
		// (set) Token: 0x060002CD RID: 717 RVA: 0x000123E9 File Offset: 0x000105E9
		public bool LockCameraMovement { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002CE RID: 718 RVA: 0x000123F4 File Offset: 0x000105F4
		// (remove) Token: 0x060002CF RID: 719 RVA: 0x0001242C File Offset: 0x0001062C
		public event MissionScreen.OnSpectateAgentDelegate OnSpectateAgentFocusIn;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002D0 RID: 720 RVA: 0x00012464 File Offset: 0x00010664
		// (remove) Token: 0x060002D1 RID: 721 RVA: 0x0001249C File Offset: 0x0001069C
		public event MissionScreen.OnSpectateAgentDelegate OnSpectateAgentFocusOut;

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x000124D1 File Offset: 0x000106D1
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x000124D9 File Offset: 0x000106D9
		public OrderFlag OrderFlag { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x000124E2 File Offset: 0x000106E2
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x000124EA File Offset: 0x000106EA
		public Camera CombatCamera { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x000124F3 File Offset: 0x000106F3
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x000124FB File Offset: 0x000106FB
		public Camera CustomCamera { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00012504 File Offset: 0x00010704
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0001250C File Offset: 0x0001070C
		public float CameraBearing { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00012515 File Offset: 0x00010715
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0001251D File Offset: 0x0001071D
		public float MaxCameraZoom { get; private set; } = 1f;

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00012526 File Offset: 0x00010726
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0001252E File Offset: 0x0001072E
		public float CameraElevation { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00012537 File Offset: 0x00010737
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0001253F File Offset: 0x0001073F
		public float CameraResultDistanceToTarget { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00012548 File Offset: 0x00010748
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00012550 File Offset: 0x00010750
		public float CameraViewAngle { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00012559 File Offset: 0x00010759
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00012561 File Offset: 0x00010761
		public bool IsPhotoModeEnabled { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0001256A File Offset: 0x0001076A
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00012572 File Offset: 0x00010772
		public bool IsConversationMission { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0001257B File Offset: 0x0001077B
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00012583 File Offset: 0x00010783
		public bool IsConversationActive { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0001258C File Offset: 0x0001078C
		public bool IsDeploymentActive
		{
			get
			{
				return this.Mission.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0001259C File Offset: 0x0001079C
		// (set) Token: 0x060002EA RID: 746 RVA: 0x000125A4 File Offset: 0x000107A4
		public SceneLayer SceneLayer { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000125AD File Offset: 0x000107AD
		public SceneView SceneView
		{
			get
			{
				SceneLayer sceneLayer = this.SceneLayer;
				if (sceneLayer == null)
				{
					return null;
				}
				return sceneLayer.SceneView;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002EC RID: 748 RVA: 0x000125C0 File Offset: 0x000107C0
		// (set) Token: 0x060002ED RID: 749 RVA: 0x000125C8 File Offset: 0x000107C8
		public Mission Mission { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002EE RID: 750 RVA: 0x000125D1 File Offset: 0x000107D1
		// (set) Token: 0x060002EF RID: 751 RVA: 0x000125D9 File Offset: 0x000107D9
		public bool IsCheatGhostMode { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x000125E2 File Offset: 0x000107E2
		public bool IsRadialMenuActive
		{
			get
			{
				return this._objectsWithActiveRadialMenu.Count > 0;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000125F2 File Offset: 0x000107F2
		public IInputContext InputManager
		{
			get
			{
				return this.Mission.InputManager;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000125FF File Offset: 0x000107FF
		private bool IsOrderMenuOpen
		{
			get
			{
				return this.Mission.IsOrderMenuOpen;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0001260C File Offset: 0x0001080C
		private bool IsTransferMenuOpen
		{
			get
			{
				return this.Mission.IsTransferMenuOpen;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00012619 File Offset: 0x00010819
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00012624 File Offset: 0x00010824
		public Agent LastFollowedAgent
		{
			get
			{
				return this._lastFollowedAgent;
			}
			private set
			{
				if (this._lastFollowedAgent != value)
				{
					Agent lastFollowedAgent = this._lastFollowedAgent;
					this._lastFollowedAgent = value;
					NetworkCommunicator myPeer = GameNetwork.MyPeer;
					MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
					if (GameNetwork.IsMyPeerReady)
					{
						if (missionPeer != null)
						{
							missionPeer.FollowedAgent = this._lastFollowedAgent;
						}
						else
						{
							Debug.FailedAssert("MyPeer.IsSynchronized but myMissionPeer == null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Screens\\MissionScreen.cs", "LastFollowedAgent", 225);
						}
					}
					this.ResetMaxCameraZoom();
					if (lastFollowedAgent != null)
					{
						MissionScreen.OnSpectateAgentDelegate onSpectateAgentFocusOut = this.OnSpectateAgentFocusOut;
						if (onSpectateAgentFocusOut != null)
						{
							onSpectateAgentFocusOut(lastFollowedAgent);
						}
					}
					if (this._lastFollowedAgent != null)
					{
						MissionScreen.OnSpectateAgentDelegate onSpectateAgentFocusIn = this.OnSpectateAgentFocusIn;
						if (onSpectateAgentFocusIn != null)
						{
							onSpectateAgentFocusIn(this._lastFollowedAgent);
						}
					}
					if (this._lastFollowedAgent == this._agentToFollowOverride)
					{
						this._agentToFollowOverride = null;
					}
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000126DD File Offset: 0x000108DD
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x000126E5 File Offset: 0x000108E5
		public IAgentVisual LastFollowedAgentVisuals { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000126EE File Offset: 0x000108EE
		public override bool MouseVisible
		{
			get
			{
				return ScreenManager.GetMouseVisibility();
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000126F5 File Offset: 0x000108F5
		// (set) Token: 0x060002FA RID: 762 RVA: 0x000126FD File Offset: 0x000108FD
		public bool PhotoModeRequiresMouse { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00012706 File Offset: 0x00010906
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0001270E File Offset: 0x0001090E
		public bool IsFocusLost { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00012717 File Offset: 0x00010917
		public bool IsMissionTickable
		{
			get
			{
				return base.IsActive && this.Mission != null && (this.Mission.CurrentState == Mission.State.Continuing || this.Mission.MissionEnded);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00012748 File Offset: 0x00010948
		public MissionScreen(MissionState missionState)
		{
			missionState.Handler = this;
			((SceneLayer)new SceneLayer(true, true)).SceneView.SetEnable(false);
			this._resetDraggingMode = false;
			this._missionState = missionState;
			this.Mission = missionState.CurrentMission;
			this.CombatCamera = Camera.CreateCamera();
			this._objectsWithActiveRadialMenu = new List<object>();
			this._missionViewsContainer = new MissionViewsContainer();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00012808 File Offset: 0x00010A08
		protected override void OnInitialize()
		{
			MBDebug.Print("-------MissionScreen-OnInitialize", 0, Debug.DebugColor.White, 17592186044416UL);
			base.OnInitialize();
			Module.CurrentModule.SkinsXMLHasChanged += this.OnSkinsXMLChanged;
			this.CameraViewAngle = 65f;
			this._cameraTarget = new Vec3(0f, 0f, 10f, -1f);
			this.CameraBearing = 0f;
			this.CameraElevation = -0.2f;
			this._cameraBearingDelta = 0f;
			this._cameraElevationDelta = 0f;
			this._cameraSpecialTargetAddedBearing = 0f;
			this._cameraSpecialCurrentAddedBearing = 0f;
			this._cameraSpecialTargetAddedElevation = 0f;
			this._cameraSpecialCurrentAddedElevation = 0f;
			this._cameraSpecialTargetPositionToAdd = Vec3.Zero;
			this._cameraSpecialCurrentPositionToAdd = Vec3.Zero;
			this._cameraSpecialTargetDistanceToAdd = 0f;
			this._cameraSpecialCurrentDistanceToAdd = 0f;
			this._cameraSpecialCurrentFOV = 65f;
			this._cameraSpecialTargetFOV = 65f;
			this._cameraAddedElevation = 0f;
			this._cameraTargetAddedHeight = 0f;
			this._cameraDeploymentHeightToAdd = 0f;
			this._lastCameraAddedDistance = 0f;
			this.CameraResultDistanceToTarget = 0f;
			this._cameraSpeed = Vec3.Zero;
			this._cameraSpeedMultiplier = 1f;
			this._cameraHeightLimit = 0f;
			this._cameraAddSpecialMovement = false;
			this._cameraAddSpecialPositionalMovement = false;
			this._cameraApplySpecialMovementsInstantly = false;
			this._currentViewBlockingBodyCoeff = 1f;
			this._targetViewBlockingBodyCoeff = 1f;
			this._cameraSmoothMode = false;
			this.CustomCamera = null;
			this._zoomAmount = 0f;
			this._zoomToggled = false;
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000129B4 File Offset: 0x00010BB4
		protected virtual void InitializeMissionView()
		{
			this._missionState.Paused = false;
			this.SceneLayer = new SceneLayer(true, true);
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ScoreboardHotKeyCategory"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("CombatHotKeyCategory"));
			this.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Cheats"));
			this.Mission.InputManager = this.SceneLayer.Input;
			base.AddLayer(this.SceneLayer);
			this.SceneView.SetScene(this.Mission.Scene);
			this.SceneView.SetSceneUsesShadows(true);
			this.SceneView.SetAcceptGlobalDebugRenderObjects(true);
			this.SceneView.SetResolutionScaling(true);
			this._missionMainAgentController = this.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
			this._missionCameraModeLogic = this.Mission.MissionBehaviors.FirstOrDefault<MissionBehavior>((MissionBehavior b) => b is ICameraModeLogic) as ICameraModeLogic;
			using (List<MissionBehavior>.Enumerator enumerator = this.Mission.MissionBehaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionView missionView;
					if ((missionView = enumerator.Current as MissionView) != null)
					{
						missionView.OnMissionScreenInitialize();
					}
				}
			}
			this.Mission.AgentVisualCreator = new AgentVisualsCreator();
			this.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00012B9C File Offset: 0x00010D9C
		protected override void OnActivate()
		{
			base.OnActivate();
			this.ActivateLoadingScreen();
			MissionScreen missionScreen;
			if (this.Mission != null && (this.Mission.MissionEnded || (this.IsConversationMission && !this.IsConversationActive)) && (missionScreen = ScreenManager.TopScreen as MissionScreen) != null)
			{
				ScreenManager.TopScreen.DeactivateAllLayers();
				missionScreen.SceneView.SetEnable(false);
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00012C00 File Offset: 0x00010E00
		protected override void OnResume()
		{
			base.OnResume();
			MissionScreen missionScreen;
			if (this.Mission != null && (this.Mission.MissionEnded || (this.IsConversationMission && !this.IsConversationActive)) && (missionScreen = ScreenManager.TopScreen as MissionScreen) != null)
			{
				ScreenManager.TopScreen.DeactivateAllLayers();
				missionScreen.SceneView.SetEnable(false);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00012C5C File Offset: 0x00010E5C
		public override void OnFocusChangeOnGameWindow(bool focusGained)
		{
			base.OnFocusChangeOnGameWindow(focusGained);
			if (!LoadingWindow.IsLoadingWindowActive && !InformationManager.IsAnyInquiryActive())
			{
				Mission mission = this.Mission;
				List<MissionBehavior> list;
				if (mission == null)
				{
					list = null;
				}
				else
				{
					List<MissionBehavior> missionBehaviors = mission.MissionBehaviors;
					if (missionBehaviors == null)
					{
						list = null;
					}
					else
					{
						list = (from v in missionBehaviors
							where v is MissionView
							orderby ((MissionView)v).ViewOrderPriority
							select v).ToList<MissionBehavior>();
					}
				}
				List<MissionBehavior> list2 = list;
				if (list2 != null)
				{
					for (int i = 0; i < list2.Count; i++)
					{
						(list2[i] as MissionView).OnFocusChangeOnGameWindow(focusGained);
					}
				}
			}
			this.IsFocusLost = !focusGained;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00012D1B File Offset: 0x00010F1B
		public void SetOrderFlagVisibility(bool value)
		{
			if (this.OrderFlag != null)
			{
				this.OrderFlag.IsVisible = value;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00012D31 File Offset: 0x00010F31
		public string GetFollowText()
		{
			if (this.LastFollowedAgent == null)
			{
				return "";
			}
			return this.LastFollowedAgent.Name;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00012D4C File Offset: 0x00010F4C
		public string GetFollowPartyText()
		{
			if (this.LastFollowedAgent != null)
			{
				TextObject textObject = new TextObject("{=xsC8Ierj}({BATTLE_COMBATANT})", null);
				textObject.SetTextVariable("BATTLE_COMBATANT", this.LastFollowedAgent.Origin.BattleCombatant.Name);
				return textObject.ToString();
			}
			return "";
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00012D98 File Offset: 0x00010F98
		public bool SetDisplayDialog(bool value)
		{
			bool flag = this._displayingDialog != value;
			this._displayingDialog = value;
			return flag;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00012DAD File Offset: 0x00010FAD
		bool IMissionScreen.GetDisplayDialog()
		{
			return this._displayingDialog;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00012DB8 File Offset: 0x00010FB8
		public bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			if (ScreenFadeController.IsFadeActive)
			{
				return false;
			}
			Mission mission = this.Mission;
			List<MissionBehavior> list;
			if (mission == null)
			{
				list = null;
			}
			else
			{
				List<MissionBehavior> missionBehaviors = mission.MissionBehaviors;
				if (missionBehaviors == null)
				{
					list = null;
				}
				else
				{
					list = (from v in missionBehaviors
						where v is MissionView
						orderby ((MissionView)v).ViewOrderPriority
						select v).ToList<MissionBehavior>();
				}
			}
			List<MissionBehavior> list2 = list;
			if (list2 != null)
			{
				using (List<MissionBehavior>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!(enumerator.Current as MissionView).IsOpeningEscapeMenuOnFocusChangeAllowed())
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00012E84 File Offset: 0x00011084
		public bool IsPhotoModeAllowed()
		{
			Mission mission = this.Mission;
			List<MissionBehavior> list;
			if (mission == null)
			{
				list = null;
			}
			else
			{
				List<MissionBehavior> missionBehaviors = mission.MissionBehaviors;
				if (missionBehaviors == null)
				{
					list = null;
				}
				else
				{
					list = missionBehaviors.Where<MissionBehavior>((MissionBehavior v) => v is MissionView).ToList<MissionBehavior>();
				}
			}
			using (List<MissionBehavior>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current as MissionView).IsPhotoModeAllowed())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00012F20 File Offset: 0x00011120
		public void SetExtraCameraParameters(bool newForceCanZoom, float newCameraRayCastStartingPointOffset)
		{
			this._forceCanZoom = newForceCanZoom;
			this._cameraRayCastOffset = newCameraRayCastStartingPointOffset;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00012F30 File Offset: 0x00011130
		public void SetCustomAgentListToSpectateGatherer(MissionScreen.GatherCustomAgentListToSpectateDelegate gatherer)
		{
			this._gatherCustomAgentListToSpectate = gatherer;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00012F3C File Offset: 0x0001113C
		public void UpdateFreeCamera(MatrixFrame frame)
		{
			this.CombatCamera.Frame = frame;
			Vec3 vec = -frame.rotation.u;
			this.CameraBearing = vec.RotationZ;
			Vec3 vec2 = new Vec3(0f, 0f, 1f, -1f);
			this.CameraElevation = MathF.Acos(Vec3.DotProduct(vec2, vec)) - 1.5707964f;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00012FA8 File Offset: 0x000111A8
		protected override void OnFrameTick(float dt)
		{
			this._suppressSpectatorCyclingThisFrame = false;
			if (this.SceneLayer != null)
			{
				bool flag = MBDebug.IsErrorReportModeActive();
				if (flag)
				{
					this._missionState.Paused = MBDebug.IsErrorReportModePauseMission();
				}
				if (base.DebugInput.IsHotKeyPressed("MissionScreenHotkeyFixCamera"))
				{
					this._fixCamera = !this._fixCamera;
				}
				flag = flag || this._fixCamera;
				if (this.IsPhotoModeEnabled)
				{
					flag = flag || this.PhotoModeRequiresMouse;
				}
				flag = flag || (GameNetwork.IsMultiplayer && this.IsLocalPeerSpectator());
				if (this._rightButtonDraggingMode)
				{
					flag = false;
				}
				this.SceneLayer.InputRestrictions.SetMouseVisibility(flag);
			}
			if (this.Mission != null)
			{
				if (this.IsMissionTickable)
				{
					this._missionViewsContainer.ForEach(delegate(MissionView missionView)
					{
						missionView.OnMissionScreenTick(dt);
					});
				}
				this.HandleInputs();
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00013090 File Offset: 0x00011290
		private void ActivateMissionView()
		{
			MBDebug.Print("-------MissionScreen-OnActivate", 0, Debug.DebugColor.White, 17592186044416UL);
			this.Mission.OnMainAgentChanged += this.Mission_OnMainAgentChanged;
			this.Mission.OnBeforeAgentRemoved += this.Mission_OnBeforeAgentRemoved;
			this.Mission.OnCameraShakeTriggered += new Mission.OnCameraShakeTriggeredDelegate(this.MissionOnCameraShakeTriggered);
			this._cameraBearingDelta = 0f;
			this._cameraElevationDelta = 0f;
			this.SetCameraFrameToMapView();
			this.CheckForUpdateCamera(1E-05f);
			this.Mission.ResetFirstThirdPersonView();
			if (MBEditor.EditModeEnabled && MBEditor.IsEditModeOn)
			{
				MBEditor.EnterEditMissionMode(this.Mission);
			}
			this._missionViewsContainer.ForEach(delegate(MissionView missionView)
			{
				missionView.OnMissionScreenActivate();
			});
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00013170 File Offset: 0x00011370
		private void Mission_OnMainAgentChanged(Agent oldAgent)
		{
			if (oldAgent != null)
			{
				oldAgent.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Remove(oldAgent.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChanged));
			}
			if (this.Mission.MainAgent != null)
			{
				Agent mainAgent = this.Mission.MainAgent;
				mainAgent.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Combine(mainAgent.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChanged));
				this._isPlayerAgentAdded = true;
			}
			this.ResetMaxCameraZoom();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000131E8 File Offset: 0x000113E8
		private void Mission_OnBeforeAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent == this._agentToFollowOverride)
			{
				this._agentToFollowOverride = null;
				return;
			}
			if (affectedAgent == this.Mission.MainAgent)
			{
				this._agentToFollowOverride = affectorAgent;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00013210 File Offset: 0x00011410
		private void MissionOnCameraShakeTriggered(in Vec3 position, float radius)
		{
			if (this.CombatCamera.Position.DistanceSquared(position) < radius * radius)
			{
				this._cameraShakeStartTime = Game.Current.ApplicationTime;
				this._cameraShakeIntensity = Math.Min(1.25f - this.CombatCamera.Position.Distance(position) / radius, 1f);
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0001327C File Offset: 0x0001147C
		public void OnMainAgentWeaponChanged()
		{
			this.ResetMaxCameraZoom();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00013284 File Offset: 0x00011484
		private void ResetMaxCameraZoom()
		{
			if (this.LastFollowedAgent == null || this.LastFollowedAgent != this.Mission.MainAgent)
			{
				this.MaxCameraZoom = 1f;
				return;
			}
			this.MaxCameraZoom = ((Mission.Current != null) ? MathF.Max(1f, Mission.Current.GetMainAgentMaxCameraZoom()) : 1f);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000132E0 File Offset: 0x000114E0
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			MBDebug.Print("-------MissionScreen-OnDeactivate", 0, Debug.DebugColor.White, 17592186044416UL);
			if (this.Mission == null)
			{
				return;
			}
			if (this.Mission.MainAgent != null)
			{
				Agent mainAgent = this.Mission.MainAgent;
				mainAgent.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Remove(mainAgent.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChanged));
			}
			this.Mission.OnMainAgentChanged -= this.Mission_OnMainAgentChanged;
			this.Mission.OnBeforeAgentRemoved -= this.Mission_OnBeforeAgentRemoved;
			this.Mission.OnCameraShakeTriggered -= new Mission.OnCameraShakeTriggeredDelegate(this.MissionOnCameraShakeTriggered);
			this._missionViewsContainer.ForEach(delegate(MissionView missionView)
			{
				missionView.OnMissionScreenDeactivate();
			});
			this._isRenderingStarted = false;
			this._loadingScreenFramesLeft = 15;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000133CC File Offset: 0x000115CC
		protected override void OnFinalize()
		{
			MBDebug.Print("-------MissionScreen-OnFinalize", 0, Debug.DebugColor.White, 17592186044416UL);
			Module.CurrentModule.SkinsXMLHasChanged -= this.OnSkinsXMLChanged;
			LoadingWindow.EnableGlobalLoadingWindow();
			if (this.Mission != null)
			{
				this.Mission.InputManager = null;
			}
			SceneLayer sceneLayer = this.SceneLayer;
			if (sceneLayer != null)
			{
				SceneView sceneView = sceneLayer.SceneView;
				if (sceneView != null)
				{
					sceneView.SetEnable(false);
				}
			}
			this.Mission = null;
			this.OrderFlag = null;
			this.SceneLayer = null;
			this._missionMainAgentController = null;
			this.CombatCamera = null;
			this.CustomCamera = null;
			this._missionState = null;
			base.OnFinalize();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00013474 File Offset: 0x00011674
		private IEnumerable<MissionBehavior> AddDefaultMissionBehaviorsTo(Mission mission, IEnumerable<MissionBehavior> behaviors)
		{
			List<MissionBehavior> list = new List<MissionBehavior>();
			IEnumerable<MissionBehavior> enumerable = ViewCreatorManager.CreateDefaultMissionBehaviors(mission);
			list.AddRange(enumerable);
			return behaviors.Concat<MissionBehavior>(list);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0001349C File Offset: 0x0001169C
		private void OnSkinsXMLChanged()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				agent.EquipItemsFromSpawnEquipment(true, false, false, 0);
				agent.UpdateAgentProperties();
				agent.AgentVisuals.UpdateSkeletonScale((int)agent.SpawnEquipment.BodyDeformType);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00013514 File Offset: 0x00011714
		private void OnSceneRenderingStarted()
		{
			LoadingWindow.DisableGlobalLoadingWindow();
			Utilities.SetScreenTextRenderingState(true);
			this._missionViewsContainer.ForEach(delegate(MissionView missionView)
			{
				missionView.OnSceneRenderingStarted();
			});
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001354C File Offset: 0x0001174C
		[CommandLineFunctionality.CommandLineArgumentFunction("fix_camera_toggle", "mission")]
		public static string ToggleFixedMissionCamera(List<string> strings)
		{
			MissionScreen missionScreen;
			if ((missionScreen = ScreenManager.TopScreen as MissionScreen) != null)
			{
				MissionScreen.SetFixedMissionCameraActive(!missionScreen._fixCamera);
			}
			return "Done";
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001357C File Offset: 0x0001177C
		public static void SetFixedMissionCameraActive(bool active)
		{
			MissionScreen missionScreen;
			if ((missionScreen = ScreenManager.TopScreen as MissionScreen) != null)
			{
				missionScreen._fixCamera = active;
				missionScreen.SceneLayer.InputRestrictions.SetMouseVisibility(missionScreen._fixCamera);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000135B4 File Offset: 0x000117B4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_shift_camera_speed", "mission")]
		public static string SetShiftCameraSpeed(List<string> strings)
		{
			MissionScreen missionScreen;
			if ((missionScreen = ScreenManager.TopScreen as MissionScreen) == null)
			{
				return "No Mission Available";
			}
			int num;
			if (strings.Count > 0 && int.TryParse(strings[0], out num))
			{
				missionScreen._shiftSpeedMultiplier = num;
				return "Done";
			}
			return "Current multiplier is " + missionScreen._shiftSpeedMultiplier;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00013610 File Offset: 0x00011810
		[CommandLineFunctionality.CommandLineArgumentFunction("set_camera_position", "mission")]
		public static string SetCameraPosition(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			if (strings.Count < 3)
			{
				return "You need to enter 3 arguments.";
			}
			List<float> list = new List<float>();
			for (int i = 0; i < strings.Count; i++)
			{
				float num;
				if (!float.TryParse(strings[i], out num))
				{
					return "Argument " + (i + 1) + " is not valid.";
				}
				list.Add(num);
			}
			MissionScreen missionScreen;
			if ((missionScreen = ScreenManager.TopScreen as MissionScreen) != null)
			{
				missionScreen.IsCheatGhostMode = true;
				missionScreen.LastFollowedAgent = null;
				missionScreen.CombatCamera.Position = new Vec3(list[0], list[1], list[2], -1f);
				return string.Concat(new string[]
				{
					"Camera position has been set to: ",
					strings[0],
					", ",
					strings[1],
					", ",
					strings[2]
				});
			}
			return "Mission screen not found.";
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00013714 File Offset: 0x00011914
		private void CheckForUpdateCamera(float dt)
		{
			if (this._fixCamera && !this.IsPhotoModeEnabled)
			{
				return;
			}
			if (this.CustomCamera != null)
			{
				this.CombatCamera.FillParametersFrom(this.CustomCamera);
				if (this.CustomCamera.Entity != null)
				{
					MatrixFrame globalFrame = this.CustomCamera.Entity.GetGlobalFrame();
					globalFrame.rotation.MakeUnit();
					this.CombatCamera.Frame = globalFrame;
				}
				this._zoomAmount = MBMath.ClampFloat(this._zoomAmount, 0f, 1f);
				float num = this.CustomCamera.GetFovVertical() * 57.295776f;
				if (num <= 0f)
				{
					num = Mission.GetFirstPersonFov();
				}
				float num2 = 37f / this.MaxCameraZoom;
				this.CameraViewAngle = MBMath.Lerp(num, num2, this._zoomAmount, 0.005f);
				if (this._zoomAmount > 0f)
				{
					this.CombatCamera.SetFovVertical(this.CameraViewAngle * 0.017453292f, Screen.AspectRatio, this.CustomCamera.Near, this.CustomCamera.Far);
				}
				this.SceneView.SetCamera(this.CombatCamera);
				SoundManager.SetListenerFrame(this.CombatCamera.Frame);
				return;
			}
			bool flag = false;
			using (List<MissionBehavior>.Enumerator enumerator = this.Mission.MissionBehaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionView missionView;
					if ((missionView = enumerator.Current as MissionView) != null)
					{
						flag = flag || missionView.UpdateOverridenCamera(dt);
					}
				}
			}
			if (!flag)
			{
				this.UpdateCamera(dt);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000138BC File Offset: 0x00011ABC
		private void UpdateDragData()
		{
			if (this._resetDraggingMode)
			{
				this._rightButtonDraggingMode = false;
				this._resetDraggingMode = false;
				return;
			}
			if (this.SceneLayer.Input.IsKeyReleased(InputKey.RightMouseButton))
			{
				this._resetDraggingMode = true;
				return;
			}
			if (this.SceneLayer.Input.IsKeyPressed(InputKey.RightMouseButton))
			{
				this._clickedPositionPixel = this.SceneLayer.Input.GetMousePositionPixel();
				return;
			}
			if (this.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton) && !this.SceneLayer.Input.IsKeyReleased(InputKey.RightMouseButton) && this.SceneLayer.Input.GetMousePositionPixel().DistanceSquared(this._clickedPositionPixel) > 10f && !this._rightButtonDraggingMode)
			{
				this._rightButtonDraggingMode = true;
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00013990 File Offset: 0x00011B90
		private void UpdateCamera(float dt)
		{
			Scene scene = this.Mission.Scene;
			bool photoModeOrbit = scene.GetPhotoModeOrbit();
			float num = (this.IsPhotoModeEnabled ? scene.GetPhotoModeFov() : 0f);
			bool flag = this._isGamepadActive && this.PhotoModeRequiresMouse;
			this.UpdateDragData();
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			MissionPeer missionPeer = ((GameNetwork.MyPeer != null) ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
			Mission.SpectatorData spectatingData = this.GetSpectatingData(this.CombatCamera.Frame.origin, true);
			Agent agentToFollow = spectatingData.AgentToFollow;
			IAgentVisual agentVisualToFollow = spectatingData.AgentVisualToFollow;
			SpectatorCameraTypes cameraType = spectatingData.CameraType;
			bool flag2 = this.Mission.CameraIsFirstPerson && agentToFollow != null && agentToFollow == this.Mission.MainAgent;
			float num2 = (flag2 ? Mission.GetFirstPersonFov() : 65f);
			if (this.IsPhotoModeEnabled)
			{
				this.CameraViewAngle = num2;
			}
			else
			{
				this._zoomAmount = MBMath.ClampFloat(this._zoomAmount, 0f, 1f);
				float num3 = 37f / this.MaxCameraZoom;
				this.CameraViewAngle = MBMath.Lerp(num2, num3, this._zoomAmount, 0.005f);
			}
			if (this._missionMainAgentController == null)
			{
				this._missionMainAgentController = this.Mission.GetMissionBehavior<MissionMainAgentController>();
			}
			else
			{
				this._missionMainAgentController.IsDisabled = true;
			}
			if (this._missionMainAgentController != null && this.Mission.Mode != MissionMode.Deployment && this.Mission.MainAgent != null && this.Mission.MainAgent.IsCameraAttachable())
			{
				this._missionMainAgentController.IsDisabled = false;
			}
			bool flag3 = this._cameraApplySpecialMovementsInstantly;
			if ((this.IsPhotoModeEnabled && !photoModeOrbit) || (agentToFollow == null && agentVisualToFollow == null))
			{
				float num4 = -scene.GetPhotoModeRoll();
				matrixFrame.rotation.RotateAboutSide(1.5707964f);
				matrixFrame.rotation.RotateAboutForward(this.CameraBearing);
				matrixFrame.rotation.RotateAboutSide(this.CameraElevation);
				matrixFrame.rotation.RotateAboutUp(num4);
				matrixFrame.origin = this.CombatCamera.Frame.origin;
				this._cameraSpeed *= 1f - 5f * dt;
				this._cameraSpeed.x = MBMath.ClampFloat(this._cameraSpeed.x, -20f, 20f);
				this._cameraSpeed.y = MBMath.ClampFloat(this._cameraSpeed.y, -20f, 20f);
				this._cameraSpeed.z = MBMath.ClampFloat(this._cameraSpeed.z, -20f, 20f);
				if (Game.Current.CheatMode)
				{
					if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyIncreaseCameraSpeed"))
					{
						this._cameraSpeedMultiplier *= 1.5f;
					}
					if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyDecreaseCameraSpeed"))
					{
						this._cameraSpeedMultiplier *= 0.6666667f;
					}
					if (this.InputManager.IsHotKeyPressed("ResetCameraSpeed"))
					{
						this._cameraSpeedMultiplier = 1f;
					}
					if (this.InputManager.IsControlDown())
					{
						float num5 = this.SceneLayer.Input.GetDeltaMouseScroll() * 0.008333334f;
						if (num5 > 0.01f)
						{
							this._cameraSpeedMultiplier *= 1.25f;
						}
						else if (num5 < -0.01f)
						{
							this._cameraSpeedMultiplier *= 0.8f;
						}
					}
				}
				float waterLevel = scene.GetWaterLevel();
				float num6 = 10f * this._cameraSpeedMultiplier * (this.IsPhotoModeEnabled ? (flag ? 0f : 0.3f) : 1f);
				if (this.Mission.Mode == MissionMode.Deployment)
				{
					float num7 = MathF.Max(scene.GetGroundHeightAtPosition(matrixFrame.origin, BodyFlags.CommonCollisionExcludeFlags), waterLevel);
					num6 *= MathF.Max(1f, 1f + (matrixFrame.origin.z - num7 - 5f) / 10f);
				}
				if ((!this.IsPhotoModeEnabled && this.SceneLayer.Input.IsGameKeyDown(24)) || (this.IsPhotoModeEnabled && !flag && this.SceneLayer.Input.IsHotKeyDown("FasterCamera")))
				{
					num6 *= (float)this._shiftSpeedMultiplier;
				}
				if (!this._cameraSmoothMode)
				{
					this._cameraSpeed.x = 0f;
					this._cameraSpeed.y = 0f;
					this._cameraSpeed.z = 0f;
				}
				if ((!this.InputManager.IsControlDown() || !this.InputManager.IsAltDown()) && !this.LockCameraMovement)
				{
					bool flag4 = !this._isGamepadActive || this.Mission.Mode != MissionMode.Deployment || Input.IsKeyDown(InputKey.ControllerLTrigger);
					Vec3 vec = Vec3.Zero;
					if (flag4)
					{
						vec.x = this.SceneLayer.Input.GetGameKeyAxis("MovementAxisX");
						vec.y = this.SceneLayer.Input.GetGameKeyAxis("MovementAxisY");
						if (MathF.Abs(vec.x) < 0.2f)
						{
							vec.x = 0f;
						}
						if (MathF.Abs(vec.y) < 0.2f)
						{
							vec.y = 0f;
						}
					}
					if (!this._isGamepadActive || (!this.IsPhotoModeEnabled && this.Mission.Mode != MissionMode.Deployment && !this.IsOrderMenuOpen && !this.IsTransferMenuOpen))
					{
						if (this.SceneLayer.Input.IsGameKeyDown(14))
						{
							vec.z += 1f;
						}
						if (this.SceneLayer.Input.IsGameKeyDown(15))
						{
							vec.z -= 1f;
						}
					}
					else if (this.Mission.Mode == MissionMode.Deployment && this.SceneLayer.IsHitThisFrame)
					{
						if (this.SceneLayer.Input.IsKeyDown(InputKey.ControllerRBumper))
						{
							vec.z += 1f;
						}
						if (this.SceneLayer.Input.IsKeyDown(InputKey.ControllerLBumper))
						{
							vec.z -= 1f;
						}
					}
					if (vec.IsNonZero)
					{
						float num8 = vec.Normalize();
						vec *= num6 * Math.Min(1f, num8);
						this._cameraSpeed += vec;
					}
				}
				if (this.Mission.Mode == MissionMode.Deployment && !this.IsRadialMenuActive)
				{
					Vec3 origin = matrixFrame.origin;
					float x = this._cameraSpeed.x;
					Vec3 vec2 = new Vec3(matrixFrame.rotation.s.AsVec2, 0f, -1f);
					matrixFrame.origin = origin + x * vec2.NormalizedCopy() * dt;
					Vec3 origin2 = matrixFrame.origin;
					float y = this._cameraSpeed.y;
					vec2 = new Vec3(matrixFrame.rotation.u.AsVec2, 0f, -1f);
					matrixFrame.origin = origin2 - y * vec2.NormalizedCopy() * dt;
					matrixFrame.origin.z = matrixFrame.origin.z + this._cameraSpeed.z * dt;
					if (!Game.Current.CheatMode || !this.InputManager.IsControlDown())
					{
						this._cameraDeploymentHeightToAdd += 3f * this.SceneLayer.Input.GetDeltaMouseScroll() / 120f;
						if (this.SceneLayer.Input.IsHotKeyDown("DeploymentCameraIsActive"))
						{
							this._cameraDeploymentHeightToAdd += 0.05f * Input.MouseMoveY;
						}
					}
					if (MathF.Abs(this._cameraDeploymentHeightToAdd) > 0.001f)
					{
						matrixFrame.origin.z = matrixFrame.origin.z + this._cameraDeploymentHeightToAdd * dt * 10f;
						this._cameraDeploymentHeightToAdd = MathF.Lerp(this._cameraDeploymentHeightToAdd, 0f, 1f - MathF.Pow(0.0005f, dt), 1E-05f);
					}
					else
					{
						matrixFrame.origin.z = matrixFrame.origin.z + this._cameraDeploymentHeightToAdd;
						this._cameraDeploymentHeightToAdd = 0f;
					}
				}
				else
				{
					matrixFrame.origin += this._cameraSpeed.x * matrixFrame.rotation.s * dt;
					matrixFrame.origin -= this._cameraSpeed.y * matrixFrame.rotation.u * dt;
					matrixFrame.origin += this._cameraSpeed.z * matrixFrame.rotation.f * dt;
				}
				if (!MBEditor.IsEditModeOn)
				{
					if (!this.Mission.IsPositionInsideBoundaries(matrixFrame.origin.AsVec2))
					{
						matrixFrame.origin.AsVec2 = this.Mission.GetClosestBoundaryPosition(matrixFrame.origin.AsVec2);
					}
					if (!GameNetwork.IsMultiplayer && this.Mission.Mode == MissionMode.Deployment)
					{
						BattleSideEnum side = this.Mission.PlayerTeam.Side;
						IMissionDeploymentPlan deploymentPlan = this.Mission.DeploymentPlan;
						if (deploymentPlan.HasDeploymentBoundaries(this.Mission.PlayerTeam))
						{
							IMissionDeploymentPlan missionDeploymentPlan = deploymentPlan;
							Team playerTeam = this.Mission.PlayerTeam;
							Vec2 vec3 = matrixFrame.origin.AsVec2;
							if (!missionDeploymentPlan.IsPositionInsideDeploymentBoundaries(playerTeam, in vec3))
							{
								IMissionDeploymentPlan missionDeploymentPlan2 = deploymentPlan;
								Team playerTeam2 = this.Mission.PlayerTeam;
								vec3 = matrixFrame.origin.AsVec2;
								matrixFrame.origin.AsVec2 = missionDeploymentPlan2.GetClosestDeploymentBoundaryPosition(playerTeam2, in vec3);
							}
						}
					}
					float num9 = MathF.Max(scene.GetGroundHeightAtPosition((this.Mission.Mode == MissionMode.Deployment) ? (matrixFrame.origin + new Vec3(0f, 0f, 100f, -1f)) : matrixFrame.origin, BodyFlags.CommonCollisionExcludeFlags), waterLevel);
					if (!this.IsCheatGhostMode && num9 < 9999f)
					{
						matrixFrame.origin.z = MathF.Max(matrixFrame.origin.z, num9 + 0.5f);
					}
					if (matrixFrame.origin.z > num9 + 80f)
					{
						matrixFrame.origin.z = num9 + 80f;
					}
					if (this._cameraHeightLimit > 0f && matrixFrame.origin.z > this._cameraHeightLimit)
					{
						matrixFrame.origin.z = this._cameraHeightLimit;
					}
					if (matrixFrame.origin.z < -100f)
					{
						matrixFrame.origin.z = -100f;
					}
				}
			}
			else if (flag2 && !this.IsPhotoModeEnabled)
			{
				Agent agent = agentToFollow;
				if (agentToFollow.AgentVisuals != null)
				{
					if (this._cameraAddSpecialMovement)
					{
						if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
						{
							MissionMainAgentController missionMainAgentController = this._missionMainAgentController;
							if (((missionMainAgentController != null) ? missionMainAgentController.InteractionComponent.CurrentFocusedObject : null) != null && this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent)
							{
								Vec3 vec4 = (this._missionMainAgentController.InteractionComponent.CurrentFocusedObject as Agent).Position - agentToFollow.Position;
								float num11;
								if (65f / this.CameraViewAngle * MathF.Abs(vec4.z) >= 2f)
								{
									float num10 = 160f;
									Vec2 vec3 = vec4.AsVec2;
									num11 = num10 / vec3.Length;
								}
								else
								{
									float num12 = ((this.Mission.Mode == MissionMode.Barter) ? 48.75f : 32.5f);
									float num13 = ((this.Mission.Mode == MissionMode.Barter) ? 75f : 50f);
									Vec2 vec3 = vec4.AsVec2;
									num11 = MathF.Min(num12, num13 / vec3.Length);
								}
								this._cameraSpecialTargetFOV = num11;
								goto IL_0C28;
							}
						}
						this._cameraSpecialTargetFOV = 65f;
						IL_0C28:
						if (flag3)
						{
							this._cameraSpecialCurrentFOV = this._cameraSpecialTargetFOV;
						}
					}
					agentToFollow.AgentVisuals.GetSkeleton().ForceUpdateBoneFrames();
					MatrixFrame boneEntitialFrame = agentToFollow.GetBoneEntitialFrame(agentToFollow.Monster.ThoraxLookDirectionBoneIndex, true);
					MatrixFrame boneEntitialFrame2 = agentToFollow.GetBoneEntitialFrame(agentToFollow.Monster.HeadLookDirectionBoneIndex, true);
					Vec3 vec2 = agent.Monster.FirstPersonCameraOffsetWrtHead;
					boneEntitialFrame2.origin = boneEntitialFrame2.TransformToParent(in vec2);
					MatrixFrame frame = agentToFollow.AgentVisuals.GetFrame();
					Vec3 vec5 = frame.TransformToParentDouble(in boneEntitialFrame2.origin);
					bool flag5;
					if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
					{
						MissionMainAgentController missionMainAgentController2 = this._missionMainAgentController;
						if (((missionMainAgentController2 != null) ? missionMainAgentController2.InteractionComponent.CurrentFocusedObject : null) != null)
						{
							flag5 = this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent;
							goto IL_0D01;
						}
					}
					flag5 = false;
					IL_0D01:
					bool flag6 = flag5;
					if ((agent.GetCurrentAnimationFlag(0) & AnimFlags.anf_lock_camera) != (AnimFlags)0UL || (agent.GetCurrentAnimationFlag(1) & AnimFlags.anf_lock_camera) != (AnimFlags)0UL)
					{
						MatrixFrame matrixFrame2 = frame.TransformToParent(in boneEntitialFrame2);
						matrixFrame2.rotation.MakeUnit();
						this.CameraBearing = matrixFrame2.rotation.f.RotationZ;
						this.CameraElevation = matrixFrame2.rotation.f.RotationX;
					}
					else
					{
						if (!flag6)
						{
							if (agentToFollow.IsMainAgent && this._missionMainAgentController != null)
							{
								vec2 = this._missionMainAgentController.CustomLookDir;
								if (vec2.IsNonZero)
								{
									goto IL_0DA4;
								}
							}
							float num14 = MBMath.WrapAngle(this.CameraBearing);
							float num15 = MBMath.WrapAngle(this.CameraElevation);
							float num16;
							float num17;
							this.CalculateNewBearingAndElevationForFirstPerson(agentToFollow, num14, num15, 0f, 0f, out num16, out num17);
							this.CameraBearing = MBMath.LerpRadians(num14, num16, Math.Min(dt * 12f, 1f), 1E-05f, 0.5f);
							this.CameraElevation = MBMath.LerpRadians(num15, num17, Math.Min(dt * 12f, 1f), 1E-05f, 0.5f);
							goto IL_0F9A;
						}
						IL_0DA4:
						Vec3 vec6;
						if (flag6)
						{
							Agent agent2 = this._missionMainAgentController.InteractionComponent.CurrentFocusedObject as Agent;
							vec2 = agent2.Position;
							vec2 = new Vec3(vec2.AsVec2, agent2.AgentVisuals.GetGlobalStableEyePoint(agent2.IsHuman).z, -1f) - vec5;
							vec6 = vec2.NormalizedCopy();
							vec2 = new Vec3(vec6.y, -vec6.x, 0f, -1f);
							Vec3 vec7 = vec2.NormalizedCopy();
							vec6 = vec6.RotateAboutAnArbitraryVector(vec7, ((this.Mission.Mode == MissionMode.Conversation) ? (-0.003f) : (-0.0045f)) * this._cameraSpecialCurrentFOV);
						}
						else
						{
							vec6 = this._missionMainAgentController.CustomLookDir;
						}
						if (flag3)
						{
							this.CameraBearing = vec6.RotationZ;
							this.CameraElevation = vec6.RotationX;
						}
						else
						{
							Mat3 identity = Mat3.Identity;
							identity.RotateAboutUp(this.CameraBearing);
							identity.RotateAboutSide(this.CameraElevation);
							Vec3 f = identity.f;
							Vec3 vec8 = Vec3.CrossProduct(f, vec6);
							float num18 = vec8.Normalize();
							Vec3 vec9;
							if (num18 < 0.0001f)
							{
								vec9 = vec6;
							}
							else
							{
								vec9 = f;
								vec9 = vec9.RotateAboutAnArbitraryVector(vec8, num18 * dt * 5f);
							}
							this.CameraBearing = vec9.RotationZ;
							this.CameraElevation = vec9.RotationX;
						}
					}
					IL_0F9A:
					if (agentToFollow.IsInWater())
					{
						AgentMovementMode agentMovementMode = agentToFollow.MovementMode & AgentMovementMode.WaterDiving;
						matrixFrame.rotation.RotateAboutSide(1.5707964f);
						matrixFrame.rotation.RotateAboutForward(this.CameraBearing);
						matrixFrame.rotation.RotateAboutSide((agentMovementMode == AgentMovementMode.WaterSurface && agentToFollow.GetCurrentVelocity().y < 0f) ? Math.Max(this.CameraElevation, -0.5f) : this.CameraElevation);
						matrixFrame.origin = vec5;
					}
					else
					{
						matrixFrame.rotation.RotateAboutSide(1.5707964f);
						matrixFrame.rotation.RotateAboutForward(this.CameraBearing);
						matrixFrame.rotation.RotateAboutSide(this.CameraElevation);
						float actionChannelWeight = agentToFollow.GetActionChannelWeight(1);
						float num19 = MBMath.WrapAngle(this.CameraBearing - agentToFollow.MovementDirectionAsAngle);
						float num20 = 1f - (1f - actionChannelWeight) * MBMath.ClampFloat((MathF.Abs(num19) - 1f) * 0.66f, 0f, 1f);
						Vec3 vec10 = frame.rotation.u * 0.25f;
						Vec3 vec11 = frame.rotation.u * 0.15f + Vec3.Forward * 0.15f;
						vec11.RotateAboutX(MBMath.ClampFloat(this.CameraElevation, -0.35f, 0.35f));
						vec11.RotateAboutZ(this.CameraBearing);
						Vec3 vec12 = frame.TransformToParent(in boneEntitialFrame.origin);
						vec12 += vec10;
						vec12 += vec11;
						if (actionChannelWeight > 0f)
						{
							this._currentViewBlockingBodyCoeff = (this._targetViewBlockingBodyCoeff = 1f);
							this._applySmoothTransitionToVirtualEyeCamera = true;
						}
						else
						{
							vec2 = vec5 - vec12;
							Vec3 vec13 = vec2.NormalizedCopy();
							if (Vec3.DotProduct(matrixFrame.rotation.u, vec13) > 0f)
							{
								vec13 = -vec13;
							}
							float num21 = 0.97499996f;
							float num22 = MathF.Lerp(0.55f, 0.7f, MathF.Abs(matrixFrame.rotation.u.z), 1E-05f);
							float num23;
							WeakGameEntity weakGameEntity;
							if (this.Mission.Scene.RayCastForClosestEntityOrTerrain(vec5 - vec13 * (num21 * num22), vec5 + vec13 * (num21 * (1f - num22)), out num23, out vec2, out weakGameEntity, 0.01f, BodyFlags.Disabled | BodyFlags.Dynamic | BodyFlags.Ladder | BodyFlags.OnlyCollideWithRaycast | BodyFlags.AILimiter | BodyFlags.Barrier | BodyFlags.Barrier3D | BodyFlags.Ragdoll | BodyFlags.RagdollLimiter | BodyFlags.DroppedItem | BodyFlags.DoNotCollideWithRaycast | BodyFlags.DontCollideWithCamera | BodyFlags.WaterBody | BodyFlags.AgentOnly | BodyFlags.MissileOnly | BodyFlags.StealthBox))
							{
								float num24 = (num21 - num23) / 0.065f;
								this._targetViewBlockingBodyCoeff = 1f / MathF.Max(1f, num24 * num24 * num24);
							}
							else
							{
								this._targetViewBlockingBodyCoeff = 1f;
							}
							if (this._currentViewBlockingBodyCoeff < this._targetViewBlockingBodyCoeff)
							{
								this._currentViewBlockingBodyCoeff = MathF.Min(this._currentViewBlockingBodyCoeff + dt * 12f, this._targetViewBlockingBodyCoeff);
							}
							else if (this._currentViewBlockingBodyCoeff > this._targetViewBlockingBodyCoeff)
							{
								this._currentViewBlockingBodyCoeff = (this._applySmoothTransitionToVirtualEyeCamera ? MathF.Max(this._currentViewBlockingBodyCoeff - dt * 6f, this._targetViewBlockingBodyCoeff) : this._targetViewBlockingBodyCoeff);
							}
							else
							{
								this._applySmoothTransitionToVirtualEyeCamera = false;
							}
							num20 *= this._currentViewBlockingBodyCoeff;
						}
						matrixFrame.origin.x = MBMath.Lerp(vec12.x, vec5.x, num20, 1E-05f);
						matrixFrame.origin.y = MBMath.Lerp(vec12.y, vec5.y, num20, 1E-05f);
						matrixFrame.origin.z = MBMath.Lerp(vec12.z, vec5.z, actionChannelWeight, 1E-05f);
					}
				}
				else
				{
					matrixFrame = this.CombatCamera.Frame;
				}
			}
			else
			{
				float num25 = 0.6f;
				float num26 = 0f;
				bool flag7 = agentVisualToFollow != null;
				float num27 = 1f;
				bool flag8 = false;
				float num29;
				if (flag7)
				{
					this._cameraSpecialTargetAddedBearing = 0f;
					this._cameraSpecialTargetAddedElevation = 0f;
					this._cameraSpecialTargetPositionToAdd = Vec3.Zero;
					this._cameraSpecialTargetDistanceToAdd = 0f;
					num25 = 1.25f;
					flag3 = flag3 || agentVisualToFollow != this.LastFollowedAgentVisuals;
					if (agentVisualToFollow.GetEquipment().Horse.Item != null)
					{
						float num28 = (float)agentVisualToFollow.GetEquipment().Horse.Item.HorseComponent.BodyLength * 0.01f;
						num25 += 2f;
						num29 = 1f * num28 + 0.9f * num27 - 0.2f;
					}
					else
					{
						num29 = 1f * num27;
					}
					this.CameraBearing = MBMath.WrapAngle(agentVisualToFollow.GetFrame().rotation.f.RotationZ + 3.1415927f);
					this.CameraElevation = 0.15f;
				}
				else
				{
					flag8 = agentToFollow.HasMount;
					flag3 = flag3 || agentToFollow != this.LastFollowedAgent;
					if (this.Mission.CustomCameraFixedDistance == -3.4028235E+38f)
					{
						num27 = agentToFollow.AgentScale;
						if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
						{
							MissionMainAgentController missionMainAgentController3 = this._missionMainAgentController;
							if (((missionMainAgentController3 != null) ? missionMainAgentController3.InteractionComponent.CurrentFocusedObject : null) != null && this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent)
							{
								MissionMainAgentController missionMainAgentController4 = this._missionMainAgentController;
								Agent agent3 = ((missionMainAgentController4 != null) ? missionMainAgentController4.InteractionComponent.CurrentFocusedObject : null) as Agent;
								num29 = (agent3.AgentVisuals.GetGlobalStableEyePoint(true).z + agentToFollow.AgentVisuals.GetGlobalStableEyePoint(true).z) * 0.5f - agentToFollow.Position.z;
								if (agent3.HasMount)
								{
									num25 += 0.1f;
								}
								if (this.Mission.Mode == MissionMode.Barter)
								{
									Vec3 vec2 = agent3.Position;
									Vec2 asVec = vec2.AsVec2;
									vec2 = agentToFollow.Position;
									Vec2 vec14 = asVec - vec2.AsVec2;
									float length = vec14.Length;
									float num30 = MathF.Max(num25 + Mission.CameraAddedDistance, 0.48f) * num27 + length * 0.5f;
									num29 += -0.004f * num30 * this._cameraSpecialCurrentFOV;
									Vec3 globalStableEyePoint = agent3.AgentVisuals.GetGlobalStableEyePoint(agent3.IsHuman);
									Vec3 globalStableEyePoint2 = agentToFollow.AgentVisuals.GetGlobalStableEyePoint(agentToFollow.IsHuman);
									float num31 = vec14.RotationInRadians - MathF.Min(0.47123894f, 0.4f / length);
									this._cameraSpecialTargetAddedBearing = MBMath.WrapAngle(num31 - this.CameraBearing);
									Vec2 vec15 = new Vec2(globalStableEyePoint.z - globalStableEyePoint2.z, MathF.Max(length, 1f));
									float num32 = (flag8 ? (-0.03f) : 0f) - vec15.RotationInRadians;
									this._cameraSpecialTargetAddedElevation = num32 - this.CameraElevation + MathF.Asin(-0.2f * (num30 - length * 0.5f) / num30);
									goto IL_1771;
								}
								goto IL_1771;
							}
						}
						if (flag8)
						{
							num25 += 0.1f;
							Agent mountAgent = agentToFollow.MountAgent;
							Monster monster = mountAgent.Monster;
							num29 = (monster.RiderCameraHeightAdder + monster.BodyCapsulePoint1.z + monster.BodyCapsuleRadius) * mountAgent.AgentScale + agentToFollow.Monster.CrouchEyeHeight * num27;
						}
						else if (agentToFollow.AgentVisuals.GetCurrentRagdollState() == RagdollState.Active)
						{
							num29 = 0.5f;
						}
						else if ((agentToFollow.GetCurrentAnimationFlag(0) & AnimFlags.anf_reset_camera_height) != (AnimFlags)0UL)
						{
							num29 = 0.5f;
						}
						else if (agentToFollow.CrouchMode || agentToFollow.IsSitting())
						{
							num29 = (agentToFollow.Monster.CrouchEyeHeight + 0.2f) * num27;
						}
						else
						{
							num29 = (agentToFollow.Monster.StandingEyeHeight + 0.2f) * num27;
						}
						IL_1771:
						if ((this.IsViewingCharacter() && (cameraType != SpectatorCameraTypes.LockToTeamMembersView || agentToFollow == this.Mission.MainAgent)) || this.IsPhotoModeEnabled)
						{
							num29 *= 0.5f;
							num25 += 0.5f;
						}
						else if (agentToFollow.HasMount && agentToFollow.IsDoingPassiveAttack && (cameraType != SpectatorCameraTypes.LockToTeamMembersView || agentToFollow == this.Mission.MainAgent))
						{
							num29 *= 1.1f;
						}
					}
					else
					{
						num29 = 0f;
					}
					if (this._cameraAddSpecialMovement)
					{
						if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
						{
							MissionMainAgentController missionMainAgentController5 = this._missionMainAgentController;
							if (((missionMainAgentController5 != null) ? missionMainAgentController5.InteractionComponent.CurrentFocusedObject : null) != null && this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent)
							{
								Agent agent4 = this._missionMainAgentController.InteractionComponent.CurrentFocusedObject as Agent;
								Vec3 globalStableEyePoint3 = agent4.AgentVisuals.GetGlobalStableEyePoint(true);
								Vec3 globalStableEyePoint4 = agentToFollow.AgentVisuals.GetGlobalStableEyePoint(true);
								Vec3 vec2 = agent4.Position;
								Vec2 asVec2 = vec2.AsVec2;
								vec2 = agentToFollow.Position;
								Vec2 vec16 = asVec2 - vec2.AsVec2;
								float length2 = vec16.Length;
								this._cameraSpecialTargetPositionToAdd = new Vec3(vec16 * 0.5f, 0f, -1f);
								this._cameraSpecialTargetDistanceToAdd = length2 * (flag8 ? 1.3f : 0.8f) - num25;
								float num33 = vec16.RotationInRadians - MathF.Min(0.47123894f, 0.48f / length2);
								this._cameraSpecialTargetAddedBearing = MBMath.WrapAngle(num33 - this.CameraBearing);
								Vec2 vec17 = new Vec2(globalStableEyePoint3.z - globalStableEyePoint4.z, MathF.Max(length2, 1f));
								float num34 = (flag8 ? (-0.03f) : 0f) - vec17.RotationInRadians;
								this._cameraSpecialTargetAddedElevation = num34 - this.CameraElevation;
								this._cameraSpecialTargetFOV = MathF.Min(32.5f, 50f / length2);
								goto IL_19B6;
							}
						}
						this._cameraSpecialTargetPositionToAdd = Vec3.Zero;
						this._cameraSpecialTargetDistanceToAdd = 0f;
						this._cameraSpecialTargetAddedBearing = 0f;
						this._cameraSpecialTargetAddedElevation = 0f;
						this._cameraSpecialTargetFOV = 65f;
						IL_19B6:
						if (flag3)
						{
							this._cameraSpecialCurrentPositionToAdd = this._cameraSpecialTargetPositionToAdd;
							this._cameraSpecialCurrentDistanceToAdd = this._cameraSpecialTargetDistanceToAdd;
							this._cameraSpecialCurrentAddedBearing = this._cameraSpecialTargetAddedBearing;
							this._cameraSpecialCurrentAddedElevation = this._cameraSpecialTargetAddedElevation;
							this._cameraSpecialCurrentFOV = this._cameraSpecialTargetFOV;
						}
					}
					if (this._cameraSpecialCurrentDistanceToAdd != this._cameraSpecialTargetDistanceToAdd)
					{
						float num35 = this._cameraSpecialTargetDistanceToAdd - this._cameraSpecialCurrentDistanceToAdd;
						if (flag3 || MathF.Abs(num35) < 0.0001f)
						{
							this._cameraSpecialCurrentDistanceToAdd = this._cameraSpecialTargetDistanceToAdd;
						}
						else
						{
							float num36 = num35 * 4f * dt;
							this._cameraSpecialCurrentDistanceToAdd += num36;
						}
					}
					num25 += this._cameraSpecialCurrentDistanceToAdd;
				}
				if (flag3)
				{
					this._cameraTargetAddedHeight = num29;
				}
				else
				{
					this._cameraTargetAddedHeight += (num29 - this._cameraTargetAddedHeight) * dt * 6f * num27;
				}
				if (this._cameraSpecialTargetAddedBearing != this._cameraSpecialCurrentAddedBearing)
				{
					float num37 = this._cameraSpecialTargetAddedBearing - this._cameraSpecialCurrentAddedBearing;
					if (flag3 || MathF.Abs(num37) < 0.0001f)
					{
						this._cameraSpecialCurrentAddedBearing = this._cameraSpecialTargetAddedBearing;
					}
					else
					{
						float num38 = num37 * 10f * dt;
						this._cameraSpecialCurrentAddedBearing += num38;
					}
				}
				if (this._cameraSpecialTargetAddedElevation != this._cameraSpecialCurrentAddedElevation)
				{
					float num39 = this._cameraSpecialTargetAddedElevation - this._cameraSpecialCurrentAddedElevation;
					if (flag3 || MathF.Abs(num39) < 0.0001f)
					{
						this._cameraSpecialCurrentAddedElevation = this._cameraSpecialTargetAddedElevation;
					}
					else
					{
						float num40 = num39 * 8f * dt;
						this._cameraSpecialCurrentAddedElevation += num40;
					}
				}
				matrixFrame.rotation.RotateAboutSide(1.5707964f);
				if (agentToFollow != null && !agentToFollow.IsMine && cameraType == SpectatorCameraTypes.LockToTeamMembersView)
				{
					Vec3 lookDirection = agentToFollow.LookDirection;
					Vec2 vec3 = lookDirection.AsVec2;
					matrixFrame.rotation.RotateAboutForward(vec3.RotationInRadians);
					matrixFrame.rotation.RotateAboutSide(MathF.Asin(lookDirection.z));
				}
				else
				{
					matrixFrame.rotation.RotateAboutForward(this.CameraBearing + this._cameraSpecialCurrentAddedBearing);
					matrixFrame.rotation.RotateAboutSide(this.CameraElevation + this._cameraSpecialCurrentAddedElevation);
					if (this.IsPhotoModeEnabled)
					{
						float num41 = -scene.GetPhotoModeRoll();
						matrixFrame.rotation.RotateAboutUp(num41);
					}
				}
				MatrixFrame matrixFrame3 = matrixFrame;
				float num42 = ((this.Mission.CustomCameraFixedDistance != float.MinValue) ? this.Mission.CustomCameraFixedDistance : (MathF.Max(num25 + Mission.CameraAddedDistance, 0.48f) * num27));
				if (this.Mission.Mode != MissionMode.Conversation && this.Mission.Mode != MissionMode.Barter && agentToFollow != null && agentToFollow.IsActive() && BannerlordConfig.EnableVerticalAimCorrection)
				{
					WeaponComponentData currentUsageItem = agentToFollow.WieldedWeapon.CurrentUsageItem;
					if (currentUsageItem != null && currentUsageItem.IsRangedWeapon)
					{
						MatrixFrame frame2 = this.CombatCamera.Frame;
						frame2.rotation.RotateAboutSide(-this._cameraAddedElevation);
						float num43;
						if (flag8)
						{
							Agent mountAgent2 = agentToFollow.MountAgent;
							Monster monster2 = mountAgent2.Monster;
							num43 = (monster2.RiderCameraHeightAdder + monster2.BodyCapsulePoint1.z + monster2.BodyCapsuleRadius) * mountAgent2.AgentScale + agentToFollow.Monster.CrouchEyeHeight * num27;
						}
						else
						{
							num43 = (agentToFollow.CrouchMode ? agentToFollow.Monster.CrouchEyeHeight : agentToFollow.Monster.StandingEyeHeight) * num27;
						}
						if (currentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.UseHandAsThrowBase))
						{
							num43 *= 1.25f;
						}
						float num45;
						if (flag3)
						{
							Vec3 vec18 = agentToFollow.Position + matrixFrame.rotation.f * num27 * (0.7f * MathF.Pow(MathF.Cos(1f / ((num42 / num27 - 0.2f) * 30f + 20f)), 3500f));
							vec18.z += this._cameraTargetAddedHeight;
							Vec3 vec19 = vec18 + matrixFrame.rotation.u * num42;
							float z = vec19.z;
							float num44 = -matrixFrame3.rotation.u.z;
							Vec2 asVec3 = vec19.AsVec2;
							Vec3 vec2 = agentToFollow.Position;
							Vec2 vec3 = asVec3 - vec2.AsVec2;
							num45 = z + num44 * vec3.Length - (agentToFollow.Position.z + num43);
						}
						else
						{
							float z2 = frame2.origin.z;
							float num46 = -frame2.rotation.u.z;
							Vec2 asVec4 = frame2.origin.AsVec2;
							Vec3 vec2 = agentToFollow.Position;
							Vec2 vec3 = asVec4 - vec2.AsVec2;
							num45 = z2 + num46 * vec3.Length - (agentToFollow.Position.z + num43);
						}
						if (num45 > 0f)
						{
							num26 = MathF.Max(-0.15f, -MathF.Asin(MathF.Min(1f, MathF.Sqrt(19.6f * num45) / (float)agentToFollow.WieldedWeapon.GetModifiedMissileSpeedForCurrentUsage())));
						}
						else
						{
							num26 = 0f;
						}
					}
					else
					{
						num26 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.MeleeAddedElevationForCrosshair);
					}
				}
				if (flag3 || this.IsPhotoModeEnabled)
				{
					this._cameraAddedElevation = num26;
				}
				else
				{
					this._cameraAddedElevation += (num26 - this._cameraAddedElevation) * dt * 3f;
				}
				if (!this.IsPhotoModeEnabled)
				{
					matrixFrame.rotation.RotateAboutSide(this._cameraAddedElevation);
				}
				bool flag9 = this.IsViewingCharacter() && !GameNetwork.IsSessionActive;
				bool flag10 = agentToFollow != null && agentToFollow.AgentVisuals != null && agentToFollow.AgentVisuals.GetCurrentRagdollState() > RagdollState.Disabled;
				bool flag11 = agentToFollow != null && agentToFollow.IsActive() && agentToFollow.GetCurrentActionType(0) == Agent.ActionCodeType.Mount;
				Vec2 vec20 = Vec2.Zero;
				Vec3 vec21;
				Vec3 vec22;
				if (flag7)
				{
					MBAgentVisuals visuals = this.GetPlayerAgentVisuals(missionPeer).GetVisuals();
					vec21 = ((visuals != null) ? visuals.GetGlobalFrame().origin : missionPeer.ControlledAgent.Position);
					vec22 = vec21;
				}
				else
				{
					vec21 = agentToFollow.VisualPosition;
					vec22 = (flag10 ? agentToFollow.AgentVisuals.GetFrame().origin : vec21);
					if (flag8)
					{
						vec20 = agentToFollow.MountAgent.GetMovementDirection() * agentToFollow.MountAgent.Monster.RiderBodyCapsuleForwardAdder;
						vec22 += vec20.ToVec3(0f);
					}
				}
				if (this._cameraAddSpecialPositionalMovement)
				{
					Vec3 vec23 = matrixFrame3.rotation.f * num27 * (0.7f * MathF.Pow(MathF.Cos(1f / ((num42 / num27 - 0.2f) * 30f + 20f)), 3500f));
					if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
					{
						this._cameraSpecialCurrentPositionToAdd += vec23;
					}
					else
					{
						this._cameraSpecialCurrentPositionToAdd -= vec23;
					}
				}
				if (this._cameraSpecialCurrentPositionToAdd != this._cameraSpecialTargetPositionToAdd)
				{
					Vec3 vec24 = this._cameraSpecialTargetPositionToAdd - this._cameraSpecialCurrentPositionToAdd;
					if (flag3 || vec24.LengthSquared < 1.0000001E-06f)
					{
						this._cameraSpecialCurrentPositionToAdd = this._cameraSpecialTargetPositionToAdd;
					}
					else
					{
						this._cameraSpecialCurrentPositionToAdd += vec24 * 4f * dt;
					}
				}
				Vec3 vec25 = this._cameraSpecialCurrentPositionToAdd;
				if (!this.Mission.CameraIsFirstPerson)
				{
					vec25 += this.Mission.CustomCameraTargetLocalOffset;
					Vec3 vec2 = this.Mission.CustomCameraLocalOffset;
					if (!vec2.IsNonZero)
					{
						vec2 = this.Mission.CustomCameraLocalOffset2;
						if (!vec2.IsNonZero)
						{
							goto IL_2230;
						}
					}
					vec25 += matrixFrame.rotation.s * (this.Mission.CustomCameraLocalOffset.x + this.Mission.CustomCameraLocalOffset2.x);
					vec25 += -matrixFrame.rotation.u * (this.Mission.CustomCameraLocalOffset.y + this.Mission.CustomCameraLocalOffset2.y);
					vec25 += matrixFrame.rotation.f * (this.Mission.CustomCameraLocalOffset.z + this.Mission.CustomCameraLocalOffset2.z);
					IL_2230:
					if (this._cameraShakeStartTime > 0f)
					{
						float applicationTime = Game.Current.ApplicationTime;
						if (this._cameraShakeStartTime > applicationTime - 2f)
						{
							float num47 = 1f - MathF.Pow((applicationTime - this._cameraShakeStartTime) / 2f, 0.4f);
							float num48 = num47 * this._cameraShakeIntensity * 0.6f;
							float num49 = num48 * 0.02f;
							this._cameraShakeCurrentTimeWithFrequency += dt * 15f * num47;
							if (num48 > 0f)
							{
								Vec3 vec26 = MBPerlin.NoiseVec3(this._cameraShakeCurrentTimeWithFrequency);
								vec25 += matrixFrame.rotation.s * (vec26.x * num48);
								vec25 += matrixFrame.rotation.f * (vec26.z * num48);
								this.Mission.SetCustomCameraLocalRotationalOffset(new Vec3(vec26.x * num49, vec26.y * num49, 0f, -1f));
							}
						}
						else
						{
							this._cameraShakeStartTime = 0f;
						}
					}
				}
				vec21 += vec25;
				vec22 += vec25;
				vec22.z += this._cameraTargetAddedHeight;
				int num50 = 0;
				bool flag12 = agentToFollow != null;
				Vec3 vec27 = vec25 + (flag12 ? ((flag8 && agentToFollow.MountAgent.AgentVisuals.IsValid()) ? agentToFollow.MountAgent.GetChestGlobalPosition() : agentToFollow.GetChestGlobalPosition()) : Vec3.Invalid);
				Vec3 vec28 = vec22 + matrixFrame3.rotation.u * num42;
				if (!this.Mission.CameraIsFirstPerson)
				{
					Vec3 vec2 = this.Mission.CustomCameraLocalRotationalOffset;
					if (vec2.IsNonZero || this._cameraShakeStartTime > 0f)
					{
						vec2 = this.Mission.CustomCameraLocalRotationalOffset;
						Vec2 vec29 = vec2.AsVec2;
						if (this._cameraShakeStartTime > 0f)
						{
							float currentTime = Mission.Current.CurrentTime;
							float num51 = (1f - MathF.Pow((currentTime - this._cameraShakeStartTime) / 2f, 0.4f)) * this._cameraShakeIntensity * 0.6f * 0.02f;
							if (num51 > 0f)
							{
								Vec3 vec30 = MBPerlin.NoiseVec3(this._cameraShakeCurrentTimeWithFrequency);
								vec29 += new Vec2(vec30.x * num51, vec30.y * num51);
							}
						}
						matrixFrame.rotation.u = matrixFrame.rotation.u.RotateAboutAnArbitraryVector(matrixFrame.rotation.s, vec29.x);
						matrixFrame.rotation.u = matrixFrame.rotation.u.RotateAboutAnArbitraryVector(matrixFrame.rotation.f, vec29.y);
						vec2 = Vec3.CrossProduct(matrixFrame.rotation.u, matrixFrame.rotation.s);
						matrixFrame.rotation.f = vec2.NormalizedCopy();
						matrixFrame.rotation.s = Vec3.CrossProduct(matrixFrame.rotation.f, matrixFrame.rotation.u);
					}
				}
				Vec3 vec31 = vec28 - vec22;
				num42 = vec31.Normalize();
				bool flag13;
				do
				{
					Vec3 vec32 = vec22;
					if (this.Mission.Mode != MissionMode.Conversation && this.Mission.Mode != MissionMode.Barter)
					{
						float num52 = 0f;
						float num53 = 1f;
						Vec3 vec2 = this.Mission.CustomCameraLocalOffset + this.Mission.CustomCameraLocalOffset2;
						float num54 = Math.Max(num52, num53 - vec2.Length);
						if (num54 > 0f)
						{
							vec32 += matrixFrame3.rotation.f * num27 * num54 * (0.7f * MathF.Pow(MathF.Cos(1f / ((num42 / num27 - 0.2f) * 30f + 20f)), 3500f));
						}
					}
					Vec3 vec33 = vec32 + vec31 * num42;
					if (flag10 || flag11)
					{
						float num55 = 0f;
						if (flag11)
						{
							float currentActionProgress = agentToFollow.GetCurrentActionProgress(0);
							num55 = currentActionProgress * currentActionProgress * 20f;
						}
						vec32 = this._cameraTarget + (vec32 - this._cameraTarget) * (5f + num55) * dt;
					}
					flag13 = false;
					MatrixFrame matrixFrame4 = new MatrixFrame(in matrixFrame.rotation, in vec33);
					Camera.GetNearPlanePointsStatic(ref matrixFrame4, this.IsPhotoModeEnabled ? (num * 0.017453292f) : (this.CameraViewAngle * 0.017453292f), Screen.AspectRatio, 0.2f, 1f, this._cameraNearPlanePoints);
					Vec3 vec34 = Vec3.Zero;
					for (int i = 0; i < 4; i++)
					{
						vec34 += this._cameraNearPlanePoints[i];
					}
					vec34 *= 0.25f;
					Vec3 vec35 = new Vec3(vec21.AsVec2 + vec20, vec32.z, -1f);
					Vec3 vec36 = vec35 - vec34;
					for (int j = 0; j < 4; j++)
					{
						this._cameraNearPlanePoints[j] += vec36;
					}
					this._cameraBoxPoints[0] = this._cameraNearPlanePoints[3] + matrixFrame4.rotation.u * 0.01f;
					this._cameraBoxPoints[1] = this._cameraNearPlanePoints[0];
					this._cameraBoxPoints[2] = this._cameraNearPlanePoints[3];
					this._cameraBoxPoints[3] = this._cameraNearPlanePoints[2];
					this._cameraBoxPoints[4] = this._cameraNearPlanePoints[1] + matrixFrame4.rotation.u * 0.01f;
					this._cameraBoxPoints[5] = this._cameraNearPlanePoints[0] + matrixFrame4.rotation.u * 0.01f;
					this._cameraBoxPoints[6] = this._cameraNearPlanePoints[1];
					this._cameraBoxPoints[7] = this._cameraNearPlanePoints[2] + matrixFrame4.rotation.u * 0.01f;
					float num56 = ((this.IsPhotoModeEnabled && !flag && photoModeOrbit) ? this._zoomAmount : 0f);
					num42 += num56;
					Vec3 zero = Vec3.Zero;
					bool flag15;
					if (!this.Mission.CustomCameraIgnoreCollision)
					{
						Scene scene2 = scene;
						Vec3[] cameraBoxPoints = this._cameraBoxPoints;
						bool flag14 = flag12;
						float num57 = num42 + 0.5f;
						GameEntity ignoredEntityForCamera = this.Mission.IgnoredEntityForCamera;
						WeakGameEntity weakGameEntity;
						float num58;
						flag15 = scene2.BoxCastOnlyForCamera(cameraBoxPoints, in vec35, flag14, in vec27, in matrixFrame4.rotation.u, num57, (ignoredEntityForCamera != null) ? ignoredEntityForCamera.WeakEntity : WeakGameEntity.Invalid, out num58, out zero, out weakGameEntity, BodyFlags.Disabled | BodyFlags.Dynamic | BodyFlags.Ladder | BodyFlags.OnlyCollideWithRaycast | BodyFlags.AILimiter | BodyFlags.Barrier | BodyFlags.Barrier3D | BodyFlags.Ragdoll | BodyFlags.RagdollLimiter | BodyFlags.DroppedItem | BodyFlags.DoNotCollideWithRaycast | BodyFlags.DontCollideWithCamera | BodyFlags.WaterBody | BodyFlags.AgentOnly | BodyFlags.MissileOnly | BodyFlags.StealthBox);
					}
					else
					{
						flag15 = false;
					}
					if (flag15)
					{
						float num58 = MathF.Max(Vec3.DotProduct(matrixFrame4.rotation.u, zero - vec32), 0.48f * num27);
						if (num58 < num42)
						{
							flag13 = true;
							num42 = num58;
						}
					}
					num50++;
				}
				while (!flag9 && num50 < 5 && flag13);
				num25 = num42 - Mission.CameraAddedDistance;
				if (flag3 || (this.CameraResultDistanceToTarget > num42 && num50 > 1))
				{
					this.CameraResultDistanceToTarget = num42;
				}
				else
				{
					float num59 = MathF.Max(MathF.Abs(Mission.CameraAddedDistance - this._lastCameraAddedDistance) * num27, MathF.Abs((num25 - (this.CameraResultDistanceToTarget - this._lastCameraAddedDistance)) * dt * 3f * num27));
					this.CameraResultDistanceToTarget += MBMath.ClampFloat(num42 - this.CameraResultDistanceToTarget, -num59, num59);
				}
				this._lastCameraAddedDistance = Mission.CameraAddedDistance;
				this._cameraTarget = vec22;
				if (this.Mission.Mode != MissionMode.Conversation && this.Mission.Mode != MissionMode.Barter)
				{
					float num60 = 0f;
					float num61 = 1f;
					Vec3 vec2 = this.Mission.CustomCameraLocalOffset + this.Mission.CustomCameraLocalOffset2;
					float num62 = Math.Max(num60, num61 - vec2.Length);
					if (num62 > 0f)
					{
						this._cameraTarget += matrixFrame3.rotation.f * num27 * num62 * (0.7f * MathF.Pow(MathF.Cos(1f / ((num42 / num27 - 0.2f) * 30f + 20f)), 3500f));
					}
				}
				matrixFrame.origin = this._cameraTarget + vec31 * this.CameraResultDistanceToTarget;
				if (!this.Mission.CameraIsFirstPerson && agentToFollow != null && agentToFollow.IsPlayerControlled)
				{
					matrixFrame.origin += this.Mission.CustomCameraGlobalOffset;
				}
			}
			if (this._cameraSpecialCurrentFOV != this._cameraSpecialTargetFOV)
			{
				float num63 = this._cameraSpecialTargetFOV - this._cameraSpecialCurrentFOV;
				if (flag3 || MathF.Abs(num63) < 0.001f)
				{
					this._cameraSpecialCurrentFOV = this._cameraSpecialTargetFOV;
				}
				else
				{
					this._cameraSpecialCurrentFOV += num63 * 3f * dt;
				}
			}
			float num64 = (this.Mission.CameraIsFirstPerson ? 0.065f : 0.1f);
			this.CombatCamera.Frame = matrixFrame;
			if (this.IsPhotoModeEnabled)
			{
				float num65 = 0f;
				float num66 = 0f;
				float num67 = 0f;
				float num68 = 0f;
				bool flag16 = false;
				scene.GetPhotoModeFocus(ref num66, ref num67, ref num65, ref num68, ref flag16);
				scene.SetDepthOfFieldFocus(num65);
				scene.SetDepthOfFieldParameters(num66, num67, flag16);
			}
			else
			{
				if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
				{
					MissionMainAgentController missionMainAgentController6 = this._missionMainAgentController;
					if (((missionMainAgentController6 != null) ? missionMainAgentController6.InteractionComponent.CurrentFocusedObject : null) != null && this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent)
					{
						MissionMainAgentController missionMainAgentController7 = this._missionMainAgentController;
						Agent agent5 = ((missionMainAgentController7 != null) ? missionMainAgentController7.InteractionComponent.CurrentFocusedObject : null) as Agent;
						scene.SetDepthOfFieldParameters(5f, 5f, false);
						Scene scene3 = scene;
						Vec3 vec2 = matrixFrame.origin - agent5.AgentVisuals.GetGlobalStableEyePoint(true);
						scene3.SetDepthOfFieldFocus(vec2.Length);
						goto IL_2CE0;
					}
				}
				if (!this._zoomAmount.ApproximatelyEqualsTo(1f, 1E-05f))
				{
					scene.SetDepthOfFieldParameters(0f, 0f, false);
					scene.SetDepthOfFieldFocus(0f);
				}
			}
			IL_2CE0:
			this.CombatCamera.SetFovVertical(this.IsPhotoModeEnabled ? (num * 0.017453292f) : (this._cameraSpecialCurrentFOV * this.Mission.CustomCameraFovMultiplier * (this.CameraViewAngle / 65f) * 0.017453292f), Screen.AspectRatio, num64, 12500f);
			this.SceneView.SetCamera(this.CombatCamera);
			Vec3 vec37 = ((agentToFollow != null) ? agentToFollow.GetEyeGlobalPosition() : matrixFrame.origin);
			if (agentToFollow != null && this.Mission.ListenerAndAttenuationPosBlendFactor > 0f)
			{
				Vec3 vec38 = matrixFrame.origin - vec37;
				vec37 += vec38 * this.Mission.ListenerAndAttenuationPosBlendFactor;
			}
			this.Mission.SetCameraFrame(ref matrixFrame, 65f / this.CameraViewAngle, ref vec37);
			if (this.LastFollowedAgent != null && this.LastFollowedAgent != this.Mission.MainAgent && (agentToFollow == this.Mission.MainAgent || agentToFollow == null))
			{
				MissionScreen.OnSpectateAgentDelegate onSpectateAgentFocusOut = this.OnSpectateAgentFocusOut;
				if (onSpectateAgentFocusOut != null)
				{
					onSpectateAgentFocusOut(this.LastFollowedAgent);
				}
			}
			this.LastFollowedAgent = agentToFollow;
			this.LastFollowedAgentVisuals = agentVisualToFollow;
			this._cameraApplySpecialMovementsInstantly = false;
			this._cameraAddSpecialMovement = false;
			this._cameraAddSpecialPositionalMovement = false;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000167B7 File Offset: 0x000149B7
		protected virtual bool CanToggleCamera()
		{
			return this.Mission.Mode != MissionMode.Deployment && this.Mission.Mode != MissionMode.CutScene;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000167DB File Offset: 0x000149DB
		protected virtual bool CanViewCharacter()
		{
			return true;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000167DE File Offset: 0x000149DE
		public bool IsViewingCharacter()
		{
			return this.CanViewCharacter() && (!this.Mission.CameraIsFirstPerson && !this.IsOrderMenuOpen) && this.SceneLayer.Input.IsGameKeyDown(25);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00016814 File Offset: 0x00014A14
		private void SetCameraFrameToMapView()
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			bool flag = false;
			if (GameNetwork.IsMultiplayer)
			{
				GameEntity gameEntity = this.Mission.Scene.FindEntityWithTag("mp_camera_start_pos");
				if (gameEntity != null)
				{
					matrixFrame = gameEntity.GetGlobalFrame();
					matrixFrame.rotation.Orthonormalize();
					this.CameraBearing = matrixFrame.rotation.f.RotationZ;
					this.CameraElevation = matrixFrame.rotation.f.RotationX - 1.5707964f;
				}
				else
				{
					Debug.FailedAssert("Multiplayer scene does not contain a camera frame", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Screens\\MissionScreen.cs", "SetCameraFrameToMapView", 2261);
					flag = true;
				}
			}
			else if (this.Mission.Mode == MissionMode.Deployment)
			{
				GameEntity gameEntity2;
				if (this.Mission.PlayerTeam.Side == BattleSideEnum.Attacker)
				{
					gameEntity2 = this.Mission.Scene.FindEntityWithTag("strategyCameraAttacker") ?? this.Mission.Scene.FindEntityWithTag("strategyCameraDefender");
				}
				else
				{
					gameEntity2 = this.Mission.Scene.FindEntityWithTag("strategyCameraDefender") ?? this.Mission.Scene.FindEntityWithTag("strategyCameraAttacker");
				}
				if (gameEntity2 != null)
				{
					matrixFrame = gameEntity2.GetGlobalFrame();
					this.CameraBearing = matrixFrame.rotation.f.RotationZ;
					this.CameraElevation = matrixFrame.rotation.f.RotationX - 1.5707964f;
				}
				else if (this.Mission.HasSpawnPath)
				{
					float num = Mission.ComputeSpawnPathDeploymentOffset(100, this.Mission.GetInitialSpawnPath());
					matrixFrame = this.Mission.GetSpawnPathFrame(this.Mission.PlayerTeam.Side, num, 0f).ToGroundMatrixFrame();
					matrixFrame.origin.z = matrixFrame.origin.z + 25f;
					matrixFrame.origin -= 25f * matrixFrame.rotation.f;
					this.CameraBearing = matrixFrame.rotation.f.RotationZ;
					this.CameraElevation = -0.7853982f;
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				Vec3 vec = new Vec3(float.MaxValue, float.MaxValue, 0f, -1f);
				Vec3 vec2 = new Vec3(float.MinValue, float.MinValue, 0f, -1f);
				if (this.Mission.Boundaries.ContainsKey("walk_area"))
				{
					using (IEnumerator<Vec2> enumerator = this.Mission.Boundaries["walk_area"].GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Vec2 vec3 = enumerator.Current;
							vec.x = MathF.Min(vec.x, vec3.x);
							vec.y = MathF.Min(vec.y, vec3.y);
							vec2.x = MathF.Max(vec2.x, vec3.x);
							vec2.y = MathF.Max(vec2.y, vec3.y);
						}
						goto IL_0332;
					}
				}
				this.Mission.Scene.GetBoundingBox(out vec, out vec2);
				IL_0332:
				Vec3 vec4 = (vec + vec2) * 0.5f;
				matrixFrame.origin = vec4;
				matrixFrame.origin.z = matrixFrame.origin.z + 10000f;
				matrixFrame.origin.z = this.Mission.Scene.GetGroundHeightAtPosition(vec4, BodyFlags.CommonCollisionExcludeFlags) + 10f;
			}
			this.CombatCamera.Frame = matrixFrame;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00016BCC File Offset: 0x00014DCC
		private bool HandleUserInputDebug()
		{
			bool flag = false;
			if (base.DebugInput.IsHotKeyPressed("MissionScreenHotkeyResetDebugVariables"))
			{
				GameNetwork.ResetDebugVariables();
			}
			if (base.DebugInput.IsHotKeyPressed("FixSkeletons"))
			{
				MBCommon.FixSkeletons();
				MessageManager.DisplayMessage("Skeleton models are reloaded...", 4294901760U);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00016C1C File Offset: 0x00014E1C
		private void HandleUserInput(float dt)
		{
			bool flag = false;
			bool flag2 = this._isGamepadActive && this.PhotoModeRequiresMouse;
			if (this.Mission == null || this.Mission.CurrentState == Mission.State.EndingNextFrame)
			{
				return;
			}
			if (!flag && Game.Current.CheatMode)
			{
				flag = this.HandleUserInputCheatMode(dt);
			}
			if (!flag && this.SceneLayer.Input.IsGameKeyDown(16) && this.Mission.CanTakeControlOfAgent(this.LastFollowedAgent))
			{
				this.Mission.TakeControlOfAgent(this.LastFollowedAgent);
				flag = true;
			}
			if (flag)
			{
				return;
			}
			float num = this.SceneLayer.Input.GetMouseSensitivity();
			if (!this.MouseVisible && this.Mission.MainAgent != null && this.Mission.MainAgent.State == AgentState.Active && this.Mission.MainAgent.IsLookRotationInSlowMotion)
			{
				num *= ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ReducedMouseSensitivityMultiplier);
			}
			float num2 = dt / 0.0009f;
			float num3 = dt / 0.0009f;
			float num4 = 0f;
			float num5 = 0f;
			if ((!MBCommon.IsPaused || this.IsPhotoModeEnabled) && !this.IsRadialMenuActive && (this.CustomCamera == null || this.AllowInputWithCustomCamera) && this._cameraSpecialTargetFOV > 9f && this.Mission.Mode != MissionMode.Barter)
			{
				if (this.MouseVisible && !this.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton))
				{
					if ((!GameNetwork.IsMultiplayer || !this.IsLocalPeerSpectator()) && this.Mission.Mode != MissionMode.Conversation)
					{
						if (this.Mission.Mode == MissionMode.Deployment)
						{
							num4 = num2 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX");
							num5 = -num3 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY");
						}
						else
						{
							if (this.SceneLayer.Input.GetMousePositionRanged().x <= 0.01f)
							{
								num4 = -400f * dt;
							}
							else if (this.SceneLayer.Input.GetMousePositionRanged().x >= 0.99f)
							{
								num4 = 400f * dt;
							}
							if (this.SceneLayer.Input.GetMousePositionRanged().y <= 0.01f)
							{
								num5 = -400f * dt;
							}
							else if (this.SceneLayer.Input.GetMousePositionRanged().y >= 0.99f)
							{
								num5 = 400f * dt;
							}
						}
					}
				}
				else if (!this.SceneLayer.Input.GetIsMouseActive())
				{
					float gameKeyAxis = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX");
					float gameKeyAxis2 = this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY");
					if (gameKeyAxis > 0.9f || gameKeyAxis < -0.9f)
					{
						num2 = dt / 0.00045f;
					}
					if (gameKeyAxis2 > 0.9f || gameKeyAxis2 < -0.9f)
					{
						num3 = dt / 0.00045f;
					}
					if (this._zoomToggled)
					{
						num2 *= BannerlordConfig.ZoomSensitivityModifier;
						num3 *= BannerlordConfig.ZoomSensitivityModifier;
					}
					num4 = num2 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisX") + this.SceneLayer.Input.GetMouseMoveX();
					num5 = -num3 * this.SceneLayer.Input.GetGameKeyAxis("CameraAxisY") + this.SceneLayer.Input.GetMouseMoveY();
					if (this._missionMainAgentController.IsPlayerAiming && NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableGyroAssistedAim) == 1f)
					{
						float config = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.GyroAimSensitivity);
						float gyroX = Input.GetGyroX();
						Input.GetGyroY();
						float gyroZ = Input.GetGyroZ();
						num4 += config * gyroZ * 12f * -1f;
						num5 += config * gyroX * 12f * -1f;
					}
				}
				else
				{
					num4 = this.SceneLayer.Input.GetMouseMoveX();
					num5 = this.SceneLayer.Input.GetMouseMoveY();
					if (this._zoomAmount > 0.66f)
					{
						num4 *= BannerlordConfig.ZoomSensitivityModifier * this._zoomAmount;
						num5 *= BannerlordConfig.ZoomSensitivityModifier * this._zoomAmount;
					}
				}
			}
			if (NativeConfig.EnableEditMode && base.DebugInput.IsHotKeyPressed("MissionScreenHotkeySwitchCameraSmooth"))
			{
				this._cameraSmoothMode = !this._cameraSmoothMode;
				MessageManager.DisplayMessage(this._cameraSmoothMode ? "Camera smooth mode Enabled." : "Camera smooth mode Disabled.", uint.MaxValue);
			}
			float num6 = 0.0035f;
			float num8;
			if (this._cameraSmoothMode)
			{
				num6 *= 0.02f;
				float num7 = 0.02f + dt - 8f * (dt * dt);
				num8 = MathF.Max(0f, 1f - 2f * num7);
			}
			else
			{
				num8 = 0f;
			}
			this._cameraBearingDelta *= num8;
			this._cameraElevationDelta *= num8;
			bool isSessionActive = GameNetwork.IsSessionActive;
			float num9 = num6 * num;
			float num10 = -num4 * num9;
			float num11 = (NativeConfig.InvertMouse ? num5 : (-num5)) * num9;
			if (isSessionActive)
			{
				float num12 = 0.3f + 10f * dt;
				num10 = MBMath.ClampFloat(num10, -num12, num12);
				num11 = MBMath.ClampFloat(num11, -num12, num12);
			}
			this._cameraBearingDelta += num10;
			this._cameraElevationDelta += num11;
			if (isSessionActive)
			{
				float num13 = 0.3f + 10f * dt;
				this._cameraBearingDelta = MBMath.ClampFloat(this._cameraBearingDelta, -num13, num13);
				this._cameraElevationDelta = MBMath.ClampFloat(this._cameraElevationDelta, -num13, num13);
			}
			Agent agentToFollow = this.GetSpectatingData(this.CombatCamera.Frame.origin).AgentToFollow;
			if (this.Mission.CameraIsFirstPerson && agentToFollow != null && agentToFollow.Controller == AgentControllerType.Player && agentToFollow.HasMount && ((ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson) == 1f && !agentToFollow.WieldedWeapon.IsEmpty && agentToFollow.WieldedWeapon.CurrentUsageItem.IsRangedWeapon) || (ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson) == 2f && (agentToFollow.WieldedWeapon.IsEmpty || agentToFollow.WieldedWeapon.CurrentUsageItem.IsMeleeWeapon)) || ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson) == 3f))
			{
				this._cameraBearingDelta += agentToFollow.MountAgent.GetTurnSpeed() * dt;
			}
			if (this.Mission.CustomCameraFixedDistance == -3.4028235E+38f)
			{
				if (this.InputManager.IsGameKeyDown(28))
				{
					Mission.CameraAddedDistance -= 2.1f * dt;
				}
				if (this.InputManager.IsGameKeyDown(29))
				{
					Mission.CameraAddedDistance += 2.1f * dt;
				}
			}
			Mission.CameraAddedDistance = MBMath.ClampFloat(Mission.CameraAddedDistance, 0.7f, 2.4f);
			this._isGamepadActive = Input.IsGamepadActive;
			if (this._isGamepadActive)
			{
				Agent mainAgent = this.Mission.MainAgent;
				bool flag3;
				if (mainAgent == null)
				{
					flag3 = false;
				}
				else
				{
					WeaponComponentData currentUsageItem = mainAgent.WieldedWeapon.CurrentUsageItem;
					bool? flag4 = ((currentUsageItem != null) ? new bool?(currentUsageItem.IsRangedWeapon) : null);
					bool flag5 = true;
					flag3 = (flag4.GetValueOrDefault() == flag5) & (flag4 != null);
				}
				if (!flag3)
				{
					goto IL_075E;
				}
			}
			bool flag6;
			if (this.CustomCamera == null)
			{
				flag6 = !this.IsRadialMenuActive;
				goto IL_075F;
			}
			IL_075E:
			flag6 = false;
			IL_075F:
			bool flag7 = flag6 || this._forceCanZoom;
			if (flag7)
			{
				if (!Input.IsGamepadActive)
				{
					this._zoomToggled = false;
				}
				else if (this.SceneLayer.Input.IsHotKeyPressed("ToggleZoom"))
				{
					this._zoomToggled = !this._zoomToggled;
				}
			}
			else
			{
				this._zoomToggled = false;
			}
			bool photoModeOrbit = this.Mission.Scene.GetPhotoModeOrbit();
			if (this.IsPhotoModeEnabled)
			{
				if (photoModeOrbit && !flag2)
				{
					this._zoomAmount -= this.SceneLayer.Input.GetDeltaMouseScroll() * 0.002f;
					this._zoomAmount = MBMath.ClampFloat(this._zoomAmount, 0f, 50f);
				}
			}
			else
			{
				if (agentToFollow != null && agentToFollow.IsMine && (this._zoomToggled || (flag7 && this.SceneLayer.Input.IsGameKeyDown(24))))
				{
					this._zoomAmount += 5f * dt;
				}
				else
				{
					this._zoomAmount -= 5f * dt;
				}
				this._zoomAmount = MBMath.ClampFloat(this._zoomAmount, 0f, 1f);
			}
			if (!this.IsPhotoModeEnabled)
			{
				if (this._zoomAmount.ApproximatelyEqualsTo(1f, 1E-05f))
				{
					this.Mission.Scene.SetDepthOfFieldParameters(this._zoomAmount * 160f * 110f, this._zoomAmount * 1500f * 0.3f, false);
				}
				else
				{
					this.Mission.Scene.SetDepthOfFieldParameters(0f, 0f, false);
				}
			}
			float num14;
			this.Mission.Scene.RayCastForClosestEntityOrTerrain(this.CombatCamera.Position + this.CombatCamera.Direction * this._cameraRayCastOffset, this.CombatCamera.Position + this.CombatCamera.Direction * 3000f, out num14, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags);
			this.Mission.Scene.SetDepthOfFieldFocus(num14);
			Agent mainAgent2 = this.Mission.MainAgent;
			if (mainAgent2 != null && !this.IsPhotoModeEnabled)
			{
				if (this._isPlayerAgentAdded)
				{
					this._isPlayerAgentAdded = false;
					if (this.Mission.Mode != MissionMode.Deployment)
					{
						this.CameraBearing = (this.Mission.CameraIsFirstPerson ? mainAgent2.LookDirection.RotationZ : mainAgent2.MovementDirectionAsAngle);
						this.CameraElevation = (this.Mission.CameraIsFirstPerson ? mainAgent2.LookDirection.RotationX : 0f);
						this._cameraSpecialTargetAddedBearing = 0f;
						this._cameraSpecialTargetAddedElevation = 0f;
						this._cameraSpecialCurrentAddedBearing = 0f;
						this._cameraSpecialCurrentAddedElevation = 0f;
					}
				}
				if (this.Mission.ClearSceneTimerElapsedTime >= 0f)
				{
					bool flag8;
					if (!this.IsViewingCharacter() && this.Mission.Mode != MissionMode.Conversation && this.Mission.Mode != MissionMode.Barter && !mainAgent2.IsLookDirectionLocked)
					{
						MissionMainAgentController missionMainAgentController = this._missionMainAgentController;
						flag8 = ((missionMainAgentController != null) ? missionMainAgentController.LockedAgent : null) == null;
					}
					else
					{
						flag8 = false;
					}
					if (!flag8)
					{
						if (this.Mission.Mode != MissionMode.Barter)
						{
							if (this._missionMainAgentController.LockedAgent != null)
							{
								this.CameraBearing = mainAgent2.LookDirection.RotationZ;
								this.CameraElevation = mainAgent2.LookDirection.RotationX;
							}
							else
							{
								this._cameraSpecialTargetAddedBearing = MBMath.WrapAngle(this._cameraSpecialTargetAddedBearing + this._cameraBearingDelta);
								this._cameraSpecialTargetAddedElevation = MBMath.WrapAngle(this._cameraSpecialTargetAddedElevation + this._cameraElevationDelta);
								this._cameraSpecialCurrentAddedBearing = MBMath.WrapAngle(this._cameraSpecialCurrentAddedBearing + this._cameraBearingDelta);
								this._cameraSpecialCurrentAddedElevation = MBMath.WrapAngle(this._cameraSpecialCurrentAddedElevation + this._cameraElevationDelta);
							}
						}
						float num15 = this.CameraElevation + this._cameraSpecialTargetAddedElevation;
						num15 = MBMath.ClampFloat(num15, -1.3659099f, 1.1219975f);
						this._cameraSpecialTargetAddedElevation = num15 - this.CameraElevation;
						num15 = this.CameraElevation + this._cameraSpecialCurrentAddedElevation;
						num15 = MBMath.ClampFloat(num15, -1.3659099f, 1.1219975f);
						this._cameraSpecialCurrentAddedElevation = num15 - this.CameraElevation;
					}
					else
					{
						this._cameraSpecialTargetAddedBearing = 0f;
						this._cameraSpecialTargetAddedElevation = 0f;
						if (this.Mission.CameraIsFirstPerson && agentToFollow != null && agentToFollow == this.Mission.MainAgent && !this.IsPhotoModeEnabled && !agentToFollow.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_lock_camera) && !agentToFollow.GetCurrentAnimationFlag(1).HasAnyFlag(AnimFlags.anf_lock_camera))
						{
							if (this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter)
							{
								MissionMainAgentController missionMainAgentController2 = this._missionMainAgentController;
								if (((missionMainAgentController2 != null) ? missionMainAgentController2.InteractionComponent.CurrentFocusedObject : null) != null && this._missionMainAgentController.InteractionComponent.CurrentFocusedObject.FocusableObjectType == FocusableObjectType.Agent)
								{
									goto IL_0D76;
								}
							}
							if (this._missionMainAgentController == null || !this._missionMainAgentController.CustomLookDir.IsNonZero)
							{
								float num16 = MBMath.WrapAngle(this.CameraBearing + this._cameraBearingDelta);
								float num17 = MBMath.WrapAngle(this.CameraElevation + this._cameraElevationDelta);
								float num18;
								float num19;
								this.CalculateNewBearingAndElevationForFirstPerson(agentToFollow, this.CameraBearing, this.CameraElevation, this._cameraBearingDelta, this._cameraElevationDelta, out num18, out num19);
								if (num18 != num16)
								{
									float num20 = MBMath.WrapAngle(this._cameraBearingDelta);
									ValueTuple<float, float> valueTuple = MathF.MinMax(-num20, num20);
									float item = valueTuple.Item1;
									float item2 = valueTuple.Item2;
									this._cameraBearingDelta = MBMath.ClampFloat(MBMath.WrapAngle(num18 - this.CameraBearing), item, item2);
								}
								if (num19 != num17)
								{
									float num21 = MBMath.WrapAngle(this._cameraElevationDelta);
									ValueTuple<float, float> valueTuple2 = MathF.MinMax(-num21, num21);
									float item3 = valueTuple2.Item1;
									float item4 = valueTuple2.Item2;
									this._cameraElevationDelta = MBMath.ClampFloat(MBMath.WrapAngle(num19 - this.CameraElevation), item3, item4);
								}
							}
						}
						IL_0D76:
						this.CameraBearing += this._cameraBearingDelta;
						this.CameraElevation += this._cameraElevationDelta;
						this.CameraElevation = MBMath.ClampFloat(this.CameraElevation, -1.3659099f, 1.1219975f);
					}
					if (this.LockCameraMovement)
					{
						this._cameraToggleStartTime = float.MaxValue;
						return;
					}
					if (this.CanToggleCamera())
					{
						if (!Input.IsMouseActive)
						{
							float applicationTime = Time.ApplicationTime;
							if (this.SceneLayer.Input.IsGameKeyPressed(27))
							{
								if (this.SceneLayer.Input.GetGameKeyAxis("MovementAxisX") <= 0.1f && this.SceneLayer.Input.GetGameKeyAxis("MovementAxisY") <= 0.1f)
								{
									this._cameraToggleStartTime = applicationTime;
								}
							}
							else if (!this.SceneLayer.Input.IsGameKeyDown(27))
							{
								this._cameraToggleStartTime = float.MaxValue;
							}
							if (this.GetCameraToggleProgress() >= 1f)
							{
								this._cameraToggleStartTime = float.MaxValue;
								this.Mission.CameraIsFirstPerson = !this.Mission.CameraIsFirstPerson;
								this._cameraApplySpecialMovementsInstantly = true;
								return;
							}
						}
						else if (this.SceneLayer.Input.IsGameKeyPressed(27))
						{
							this.Mission.CameraIsFirstPerson = !this.Mission.CameraIsFirstPerson;
							this._cameraApplySpecialMovementsInstantly = true;
							return;
						}
					}
				}
			}
			else
			{
				if (this.IsPhotoModeEnabled && this.Mission.CameraIsFirstPerson)
				{
					this.Mission.CameraIsFirstPerson = false;
				}
				this.CameraBearing += this._cameraBearingDelta;
				this.CameraElevation += this._cameraElevationDelta;
				this.CameraElevation = MBMath.ClampFloat(this.CameraElevation, -1.3659099f, 1.1219975f);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00017B5B File Offset: 0x00015D5B
		public float GetCameraToggleProgress()
		{
			if (this._cameraToggleStartTime != 3.4028235E+38f && this.SceneLayer.Input.IsGameKeyDown(27))
			{
				return (Time.ApplicationTime - this._cameraToggleStartTime) / 0.5f;
			}
			return 0f;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00017B98 File Offset: 0x00015D98
		private bool HandleUserInputCheatMode(float dt)
		{
			bool flag = false;
			if (!GameNetwork.IsMultiplayer)
			{
				if (this.InputManager.IsHotKeyPressed("EnterSlowMotion"))
				{
					float num;
					if (this.Mission.GetRequestedTimeSpeed(1121, out num))
					{
						this.Mission.RemoveTimeSpeedRequest(1121);
					}
					else
					{
						this.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.1f, 1121));
					}
					flag = true;
				}
				float num2;
				if (this.Mission.GetRequestedTimeSpeed(1121, out num2))
				{
					if (this.InputManager.IsHotKeyDown("MissionScreenHotkeyIncreaseSlowMotionFactor"))
					{
						this.Mission.RemoveTimeSpeedRequest(1121);
						num2 = MBMath.ClampFloat(num2 + 0.5f * dt, 0f, 1f);
						this.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(num2, 1121));
					}
					if (this.InputManager.IsHotKeyDown("MissionScreenHotkeyDecreaseSlowMotionFactor"))
					{
						this.Mission.RemoveTimeSpeedRequest(1121);
						num2 = MBMath.ClampFloat(num2 - 0.5f * dt, 0f, 1f);
						this.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(num2, 1121));
					}
				}
				if (this.InputManager.IsHotKeyPressed("Pause"))
				{
					this._missionState.Paused = !this._missionState.Paused;
					flag = true;
				}
				if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyHealYourSelf") && this.Mission.MainAgent != null)
				{
					this.Mission.MainAgent.Health = this.Mission.MainAgent.HealthLimit;
					flag = true;
				}
				if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyHealYourHorse"))
				{
					Agent mainAgent = this.Mission.MainAgent;
					if (((mainAgent != null) ? mainAgent.MountAgent : null) != null)
					{
						this.Mission.MainAgent.MountAgent.Health = this.Mission.MainAgent.MountAgent.HealthLimit;
						flag = true;
					}
				}
				if (!this.InputManager.IsShiftDown())
				{
					if (!this.InputManager.IsAltDown())
					{
						if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillEnemyAgent"))
						{
							return Mission.Current.KillCheats(false, true, false, false);
						}
					}
					else if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillAllEnemyAgents"))
					{
						return Mission.Current.KillCheats(true, true, false, false);
					}
				}
				else if (!this.InputManager.IsAltDown())
				{
					if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillEnemyHorse"))
					{
						return Mission.Current.KillCheats(false, true, true, false);
					}
				}
				else if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillAllEnemyHorses"))
				{
					return Mission.Current.KillCheats(true, true, true, false);
				}
				if (!this.InputManager.IsShiftDown())
				{
					if (!this.InputManager.IsAltDown())
					{
						if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillFriendlyAgent"))
						{
							return Mission.Current.KillCheats(false, false, false, false);
						}
					}
					else if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillAllFriendlyAgents"))
					{
						return Mission.Current.KillCheats(true, false, false, false);
					}
				}
				else if (!this.InputManager.IsAltDown())
				{
					if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillFriendlyHorse"))
					{
						return Mission.Current.KillCheats(false, false, true, false);
					}
				}
				else if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillAllFriendlyHorses"))
				{
					return Mission.Current.KillCheats(true, false, true, false);
				}
				if (!this.InputManager.IsShiftDown())
				{
					if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillYourSelf"))
					{
						return Mission.Current.KillCheats(false, false, false, true);
					}
				}
				else if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyKillYourHorse"))
				{
					return Mission.Current.KillCheats(false, false, true, true);
				}
				if ((GameNetwork.IsServerOrRecorder || !GameNetwork.IsMultiplayer) && this.InputManager.IsHotKeyPressed("MissionScreenHotkeyGhostCam"))
				{
					this.IsCheatGhostMode = !this.IsCheatGhostMode;
				}
			}
			if (!GameNetwork.IsSessionActive)
			{
				if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeySwitchAgentToAi"))
				{
					Debug.Print("Cheat: SwitchAgentToAi", 0, Debug.DebugColor.White, 17592186044416UL);
					if (this.Mission.MainAgent != null && this.Mission.MainAgent.IsActive())
					{
						this.Mission.MainAgent.Controller = ((this.Mission.MainAgent.Controller == AgentControllerType.Player) ? AgentControllerType.AI : AgentControllerType.Player);
						flag = true;
					}
				}
				if (this.InputManager.IsHotKeyPressed("MissionScreenHotkeyControlFollowedAgent"))
				{
					Debug.Print("Cheat: ControlFollowedAgent", 0, Debug.DebugColor.White, 17592186044416UL);
					if (this.Mission.MainAgent != null)
					{
						if (this.Mission.MainAgent.Controller == AgentControllerType.Player)
						{
							this.Mission.MainAgent.Controller = AgentControllerType.AI;
							if (this.LastFollowedAgent != null)
							{
								this.LastFollowedAgent.Controller = AgentControllerType.Player;
							}
						}
						else
						{
							foreach (Agent agent in this.Mission.Agents)
							{
								if (agent.Controller == AgentControllerType.Player)
								{
									agent.Controller = AgentControllerType.AI;
								}
							}
							this.Mission.MainAgent.Controller = AgentControllerType.Player;
						}
						flag = true;
					}
					else
					{
						if (this.LastFollowedAgent != null)
						{
							this.LastFollowedAgent.Controller = AgentControllerType.Player;
						}
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000180CC File Offset: 0x000162CC
		public void AddMissionView(MissionView missionView)
		{
			this.Mission.AddMissionBehavior(missionView);
			this.RegisterView(missionView);
			missionView.OnMissionScreenInitialize();
			Debug.ReportMemoryBookmark("MissionView Initialized: " + missionView.GetType().Name);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00018104 File Offset: 0x00016304
		public void ScreenPointToWorldRay(Vec2 screenPoint, out Vec3 rayBegin, out Vec3 rayEnd)
		{
			rayBegin = Vec3.Invalid;
			rayEnd = Vec3.Invalid;
			Vec2 vec = this.SceneView.ScreenPointToViewportPoint(screenPoint);
			this.CombatCamera.ViewportPointToWorldRay(ref rayBegin, ref rayEnd, vec);
			float num = -1f;
			foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this.Mission.Boundaries)
			{
				float boundaryRadius = this.Mission.Boundaries.GetBoundaryRadius(keyValuePair.Key);
				if (num < boundaryRadius)
				{
					num = boundaryRadius;
				}
			}
			if (num < 0f)
			{
				num = 30f;
			}
			Vec3 vec2 = rayEnd - rayBegin;
			float num2 = vec2.Normalize();
			rayEnd = rayBegin + vec2 * MathF.Min(num2, num);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000181F4 File Offset: 0x000163F4
		public bool GetProjectedMousePositionOnGround(out Vec3 groundPosition, out Vec3 groundNormal, BodyFlags excludeBodyOwnerFlags, bool checkOccludedSurface)
		{
			return this.SceneView.ProjectedMousePositionOnGround(out groundPosition, out groundNormal, this.MouseVisible, excludeBodyOwnerFlags, checkOccludedSurface);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001820C File Offset: 0x0001640C
		public bool GetProjectedMousePositionOnWater(out Vec3 waterPosition)
		{
			return this.SceneView.ProjectedMousePositionOnWater(out waterPosition, this.MouseVisible);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00018220 File Offset: 0x00016420
		public void CancelQuickPositionOrder()
		{
			if (this.OrderFlag != null)
			{
				this.OrderFlag.IsVisible = false;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00018236 File Offset: 0x00016436
		public bool MissionStartedRendering()
		{
			return this.SceneView != null && this.SceneView.ReadyToRender();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00018253 File Offset: 0x00016453
		public bool MissionLoadingWindowDisabled()
		{
			return this._onSceneRenderingStartedCalled;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0001825B File Offset: 0x0001645B
		public Vec3 GetOrderFlagPosition()
		{
			if (this.OrderFlag != null)
			{
				return this.OrderFlag.Position;
			}
			return Vec3.Invalid;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00018276 File Offset: 0x00016476
		public MatrixFrame GetOrderFlagFrame()
		{
			return this.OrderFlag.Frame;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00018284 File Offset: 0x00016484
		private void ActivateLoadingScreen()
		{
			if (this.SceneLayer != null && this.SceneLayer.SceneView != null)
			{
				Scene scene = this.SceneLayer.SceneView.GetScene();
				if (scene != null)
				{
					scene.PreloadForRendering();
				}
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000182CC File Offset: 0x000164CC
		public void RegisterRadialMenuObject<T>(T radialMenuOwnerObject) where T : class
		{
			if (this._objectsWithActiveRadialMenu.Contains(radialMenuOwnerObject))
			{
				return;
			}
			this._objectsWithActiveRadialMenu.Add(radialMenuOwnerObject);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000182F3 File Offset: 0x000164F3
		public void UnregisterRadialMenuObject(object radialMenuOwnerObject)
		{
			if (!this._objectsWithActiveRadialMenu.Contains(radialMenuOwnerObject))
			{
				return;
			}
			this._objectsWithActiveRadialMenu.Remove(radialMenuOwnerObject);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00018311 File Offset: 0x00016511
		public void SetPhotoModeRequiresMouse(bool isRequired)
		{
			this.PhotoModeRequiresMouse = isRequired;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001831C File Offset: 0x0001651C
		public void SetPhotoModeEnabled(bool isEnabled)
		{
			if (this.IsPhotoModeEnabled != isEnabled && !GameNetwork.IsMultiplayer)
			{
				this.IsPhotoModeEnabled = isEnabled;
				if (isEnabled)
				{
					MBCommon.PauseGameEngine();
					this._missionViewsContainer.ForEach(delegate(MissionView missionView)
					{
						missionView.OnPhotoModeActivated();
					});
				}
				else
				{
					MBCommon.UnPauseGameEngine();
					this._missionViewsContainer.ForEach(delegate(MissionView missionView)
					{
						missionView.OnPhotoModeDeactivated();
					});
				}
				this.Mission.Scene.SetPhotoModeOn(this.IsPhotoModeEnabled);
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000183C0 File Offset: 0x000165C0
		public void SetConversationActive(bool isActive)
		{
			if (this.IsConversationActive != isActive && !GameNetwork.IsMultiplayer)
			{
				this.IsConversationActive = isActive;
				this._missionViewsContainer.ForEach(delegate(MissionView missionView)
				{
					if (isActive)
					{
						missionView.OnConversationBegin();
						return;
					}
					missionView.OnConversationEnd();
				});
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00018412 File Offset: 0x00016612
		public void SetAsConversationMission()
		{
			this.IsConversationMission = true;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001841B File Offset: 0x0001661B
		public void SetCameraLockState(bool isLocked)
		{
			this.LockCameraMovement = isLocked;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00018424 File Offset: 0x00016624
		public void RegisterView(MissionView missionView)
		{
			this._missionViewsContainer.Add(missionView);
			missionView.MissionScreen = this;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00018439 File Offset: 0x00016639
		public void UnregisterView(MissionView missionView)
		{
			this._missionViewsContainer.Remove(missionView);
			missionView.MissionScreen = null;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00018450 File Offset: 0x00016650
		public virtual void TeleportMainAgentToCameraFocusForCheat()
		{
			MatrixFrame lastFinalRenderCameraFrame = this.Mission.Scene.LastFinalRenderCameraFrame;
			float num;
			if (this.Mission.Scene.RayCastForClosestEntityOrTerrain(lastFinalRenderCameraFrame.origin, lastFinalRenderCameraFrame.origin + -lastFinalRenderCameraFrame.rotation.u * 100f, out num, 0.01f, BodyFlags.CommonCollisionExcludeFlags))
			{
				Vec3 vec = lastFinalRenderCameraFrame.origin + -lastFinalRenderCameraFrame.rotation.u * num;
				Vec2 vec2 = -lastFinalRenderCameraFrame.rotation.u.AsVec2;
				vec2.Normalize();
				MatrixFrame matrixFrame = default(MatrixFrame);
				matrixFrame.origin = vec;
				matrixFrame.rotation.f = new Vec3(vec2.x, vec2.y, 0f, -1f);
				matrixFrame.rotation.u = new Vec3(0f, 0f, 1f, -1f);
				matrixFrame.rotation.Orthonormalize();
				Agent.Main.TeleportToPosition(matrixFrame.origin);
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00018574 File Offset: 0x00016774
		public IAgentVisual GetPlayerAgentVisuals(MissionPeer lobbyPeer)
		{
			return lobbyPeer.GetAgentVisualForPeer(0);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001857D File Offset: 0x0001677D
		public void SetAgentToFollow(Agent agent)
		{
			this._agentToFollowOverride = agent;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00018586 File Offset: 0x00016786
		public void SetSpectatorCameraOverride(SpectatorCameraTypes? cameraMode)
		{
			this._spectatorCameraOverride = cameraMode;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001858F File Offset: 0x0001678F
		private bool IsLocalPeerSpectator()
		{
			return SpectatorHelper.IsLocalPeerSpectator();
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00018596 File Offset: 0x00016796
		public bool IsRightButtonDragging
		{
			get
			{
				return this._rightButtonDraggingMode;
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001859E File Offset: 0x0001679E
		public void RequestSpectatorCycle(int direction)
		{
			if (direction == 0)
			{
				return;
			}
			this._agentToFollowOverride = null;
			this._requestedSpectatorCycleDirection = ((direction < 0) ? (-1) : 1);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000185B9 File Offset: 0x000167B9
		public void SuppressSpectatorCyclingThisFrame()
		{
			this._suppressSpectatorCyclingThisFrame = true;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000185C2 File Offset: 0x000167C2
		public Mission.SpectatorData GetSpectatingData(Vec3 currentCameraPosition)
		{
			return this.GetSpectatingData(currentCameraPosition, false);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000185CC File Offset: 0x000167CC
		private Mission.SpectatorData GetSpectatingData(Vec3 currentCameraPosition, bool consumeSpectatorCycleRequest)
		{
			bool suppressSpectatorCyclingThisFrame = this._suppressSpectatorCyclingThisFrame;
			Agent agent = null;
			IAgentVisual agentVisual = null;
			SpectatorCameraTypes spectatorCameraTypes = SpectatorCameraTypes.Invalid;
			bool flag = this.Mission.MainAgent != null && this.Mission.MainAgent.IsCameraAttachable() && this.Mission.Mode != MissionMode.Deployment;
			bool flag2 = flag || (this.LastFollowedAgent != null && this.LastFollowedAgent.Controller == AgentControllerType.Player && this.LastFollowedAgent.IsCameraAttachable());
			MissionPeer missionPeer = ((GameNetwork.MyPeer != null) ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
			bool flag3 = missionPeer != null && missionPeer.HasSpawnedAgentVisuals;
			bool flag4 = this.IsLocalPeerSpectator();
			bool flag5 = flag4 || (this._missionLobbyComponent != null && (this._missionLobbyComponent.MissionType == MultiplayerGameType.Siege || this._missionLobbyComponent.MissionType == MultiplayerGameType.TeamDeathmatch)) || this.Mission.Mode == MissionMode.Deployment;
			SpectatorCameraTypes spectatorCameraTypes2;
			if (!this.IsCheatGhostMode && !flag2 && flag5 && this._agentToFollowOverride != null && this._agentToFollowOverride.IsCameraAttachable() && !flag3)
			{
				agent = this._agentToFollowOverride;
				spectatorCameraTypes2 = SpectatorCameraTypes.LockToAnyAgent;
			}
			else
			{
				if (this._missionCameraModeLogic != null)
				{
					spectatorCameraTypes = this._missionCameraModeLogic.GetMissionCameraLockMode(flag2);
				}
				if (this.IsCheatGhostMode)
				{
					spectatorCameraTypes2 = SpectatorCameraTypes.Free;
				}
				else if (flag4 && this._spectatorCameraOverride != null && MultiplayerOptions.IsSpectatorCameraFreedomAllowed())
				{
					spectatorCameraTypes2 = this._spectatorCameraOverride.Value;
				}
				else if (spectatorCameraTypes != SpectatorCameraTypes.Invalid)
				{
					spectatorCameraTypes2 = spectatorCameraTypes;
				}
				else if (this.Mission.Mode == MissionMode.Deployment)
				{
					spectatorCameraTypes2 = SpectatorCameraTypes.Free;
				}
				else if (flag)
				{
					spectatorCameraTypes2 = SpectatorCameraTypes.LockToMainPlayer;
					agent = this.Mission.MainAgent;
				}
				else if (flag2)
				{
					spectatorCameraTypes2 = SpectatorCameraTypes.LockToMainPlayer;
					agent = this.LastFollowedAgent;
				}
				else if (missionPeer != null && this.GetPlayerAgentVisuals(missionPeer) != null && spectatorCameraTypes != SpectatorCameraTypes.Free)
				{
					spectatorCameraTypes2 = SpectatorCameraTypes.LockToPosition;
					agentVisual = this.GetPlayerAgentVisuals(missionPeer);
				}
				else if (!GameNetwork.IsMultiplayer)
				{
					spectatorCameraTypes2 = SpectatorCameraTypes.Free;
				}
				else
				{
					spectatorCameraTypes2 = (SpectatorCameraTypes)MultiplayerOptions.OptionType.SpectatorCamera.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
				if ((spectatorCameraTypes2 != SpectatorCameraTypes.LockToMainPlayer && spectatorCameraTypes2 != SpectatorCameraTypes.LockToPosition && this.Mission.Mode != MissionMode.Deployment) || (this.IsCheatGhostMode && !this.IsOrderMenuOpen && !this.IsTransferMenuOpen))
				{
					if (this.LastFollowedAgent != null && this.LastFollowedAgent.IsCameraAttachable())
					{
						agent = this.LastFollowedAgent;
					}
					else if (spectatorCameraTypes2 != SpectatorCameraTypes.Free || (this._gatherCustomAgentListToSpectate != null && this.LastFollowedAgent != null))
					{
						agent = this.FindNextCameraAttachableAgent(this.LastFollowedAgent, spectatorCameraTypes2, 1, currentCameraPosition);
					}
					bool flag6 = Game.Current.CheatMode && this.InputManager.IsControlDown();
					if (consumeSpectatorCycleRequest && this._requestedSpectatorCycleDirection != 0)
					{
						agent = this.FindNextCameraAttachableAgent(this.LastFollowedAgent, spectatorCameraTypes2, this._requestedSpectatorCycleDirection, currentCameraPosition);
					}
					else if (!flag4 && (this.InputManager.IsGameKeyReleased(9) || this.InputManager.IsGameKeyReleased(12)) && !this._rightButtonDraggingMode && !suppressSpectatorCyclingThisFrame)
					{
						if (!flag6)
						{
							agent = this.FindNextCameraAttachableAgent(this.LastFollowedAgent, spectatorCameraTypes2, 1, currentCameraPosition);
						}
					}
					else if ((this.InputManager.IsGameKeyDown(0) || this.InputManager.IsGameKeyDown(1) || this.InputManager.IsGameKeyDown(2) || this.InputManager.IsGameKeyDown(3) || (this.InputManager.GetIsControllerConnected() && (Input.GetKeyState(InputKey.ControllerLStick).y != 0f || Input.GetKeyState(InputKey.ControllerLStick).x != 0f))) && spectatorCameraTypes2 == SpectatorCameraTypes.Free)
					{
						agent = null;
						agentVisual = null;
					}
				}
			}
			if (consumeSpectatorCycleRequest)
			{
				this._requestedSpectatorCycleDirection = 0;
			}
			return new Mission.SpectatorData(agent, agentVisual, spectatorCameraTypes2);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001894C File Offset: 0x00016B4C
		private Agent FindNextCameraAttachableAgent(Agent currentAgent, SpectatorCameraTypes cameraLockMode, int iterationDirection, Vec3 currentCameraPosition)
		{
			if (this.Mission.AllAgents == null || this.Mission.AllAgents.Count == 0)
			{
				return null;
			}
			if (MBDebug.IsErrorReportModeActive())
			{
				return null;
			}
			MissionPeer missionPeer = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
			List<Agent> list;
			if (this._gatherCustomAgentListToSpectate != null)
			{
				list = this._gatherCustomAgentListToSpectate(currentAgent);
			}
			else
			{
				switch (cameraLockMode)
				{
				case SpectatorCameraTypes.LockToAnyAgent:
					list = this.Mission.AllAgents.Where<Agent>((Agent x) => x.IsCameraAttachable() || x == currentAgent).ToList<Agent>();
					goto IL_0155;
				case SpectatorCameraTypes.LockToAnyPlayer:
				case SpectatorCameraTypes.OrbitAroundTarget:
					list = this.Mission.AllAgents.Where<Agent>((Agent x) => (x.MissionPeer != null && x.IsCameraAttachable()) || x == currentAgent).ToList<Agent>();
					goto IL_0155;
				case SpectatorCameraTypes.LockToPlayerFormation:
					list = this.Mission.AllAgents.Where<Agent>(delegate(Agent x)
					{
						if (x.Formation != null)
						{
							Formation formation = x.Formation;
							MissionPeer missionPeer2 = missionPeer;
							if (formation == ((missionPeer2 != null) ? missionPeer2.ControlledFormation : null) && x.IsCameraAttachable())
							{
								return true;
							}
						}
						return x == currentAgent;
					}).ToList<Agent>();
					goto IL_0155;
				case SpectatorCameraTypes.LockToTeamMembers:
				case SpectatorCameraTypes.LockToTeamMembersView:
					list = this.Mission.AllAgents.Where<Agent>((Agent x) => (x.Team == this.Mission.PlayerTeam && x.MissionPeer != null && x.IsCameraAttachable()) || x == currentAgent).ToList<Agent>();
					goto IL_0155;
				}
				list = this.Mission.AllAgents.Where<Agent>((Agent x) => x.IsCameraAttachable() || x == currentAgent).ToList<Agent>();
			}
			IL_0155:
			Agent agent;
			if (list.Count - ((currentAgent != null && !currentAgent.IsCameraAttachable()) ? 1 : 0) == 0)
			{
				agent = null;
			}
			else if (currentAgent == null)
			{
				Agent agent2 = null;
				float num = float.MaxValue;
				foreach (Agent agent3 in list)
				{
					float lengthSquared = (currentCameraPosition - agent3.Position).LengthSquared;
					if (num > lengthSquared)
					{
						num = lengthSquared;
						agent2 = agent3;
					}
				}
				agent = agent2;
			}
			else
			{
				int num2 = list.IndexOf(currentAgent);
				if (iterationDirection == 1)
				{
					agent = list[(num2 + 1) % list.Count];
				}
				else
				{
					agent = ((num2 < 0) ? list[list.Count - 1] : list[(num2 + list.Count - 1) % list.Count]);
				}
			}
			return agent;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00018BA4 File Offset: 0x00016DA4
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00018BA6 File Offset: 0x00016DA6
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00018BA8 File Offset: 0x00016DA8
		void IGameStateListener.OnActivate()
		{
			if (this._isDeactivated)
			{
				this.ActivateMissionView();
			}
			this._isDeactivated = false;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00018BC0 File Offset: 0x00016DC0
		void IGameStateListener.OnDeactivate()
		{
			this._isDeactivated = true;
			Mission mission = this.Mission;
			if (((mission != null) ? mission.MissionBehaviors : null) != null)
			{
				this._missionViewsContainer.ForEach(delegate(MissionView missionView)
				{
					missionView.OnMissionScreenDeactivate();
				});
			}
			this.OnDeactivate();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00018C18 File Offset: 0x00016E18
		void IMissionSystemHandler.OnMissionAfterStarting(Mission mission)
		{
			this.Mission = mission;
			this.Mission.AddListener(this);
			using (List<MissionBehavior>.Enumerator enumerator = this.Mission.MissionBehaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionView missionView;
					if ((missionView = enumerator.Current as MissionView) != null)
					{
						this.RegisterView(missionView);
					}
				}
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00018C8C File Offset: 0x00016E8C
		void IMissionSystemHandler.OnMissionLoadingFinished(Mission mission)
		{
			this.Mission = mission;
			this.InitializeMissionView();
			this.ActivateMissionView();
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00018CA4 File Offset: 0x00016EA4
		void IMissionSystemHandler.BeforeMissionTick(Mission mission, float realDt)
		{
			if (MBEditor.EditModeEnabled)
			{
				if (base.DebugInput.IsHotKeyReleased("EnterEditMode") && mission == null)
				{
					if (MBEditor.IsEditModeOn)
					{
						MBEditor.LeaveEditMode();
						this._tickEditor = false;
					}
					else
					{
						MBEditor.EnterEditMode(this.SceneView, this.CombatCamera.Frame, this.CameraElevation, this.CameraBearing);
						this._tickEditor = true;
					}
				}
				if (this._tickEditor && MBEditor.IsEditModeOn)
				{
					MBEditor.TickEditMode(realDt);
					return;
				}
			}
			if (mission == null || mission.Scene == null)
			{
				return;
			}
			mission.Scene.SetOwnerThread();
			mission.Scene.SetDynamicShadowmapCascadesRadiusMultiplier(1f);
			if (MBEditor.EditModeEnabled)
			{
				MBCommon.CheckResourceModifications();
			}
			this.HandleUserInput(realDt);
			if (!this._isRenderingStarted && this.MissionStartedRendering())
			{
				Mission.Current.OnRenderingStarted();
				this._isRenderingStarted = true;
				Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.Mission));
			}
			if (this._isRenderingStarted && this._loadingScreenFramesLeft >= 0 && !this._onSceneRenderingStartedCalled)
			{
				if (this._loadingScreenFramesLeft > 0)
				{
					this._loadingScreenFramesLeft--;
					Mission mission2 = Mission.Current;
					Utilities.SetLoadingScreenPercentage((mission2 != null && mission2.HasMissionBehavior<DeploymentMissionController>()) ? ((this._loadingScreenFramesLeft == 0) ? 1f : (0.92f - (float)this._loadingScreenFramesLeft * 0.005f)) : (1f - (float)this._loadingScreenFramesLeft * 0.02f));
				}
				bool flag = this.AreViewsReady();
				if (this._loadingScreenFramesLeft <= 0 && flag && !MBAnimation.IsAnyAnimationLoadingFromDisk())
				{
					this.OnSceneRenderingStarted();
					this._onSceneRenderingStartedCalled = true;
				}
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00018E4C File Offset: 0x0001704C
		private bool AreViewsReady()
		{
			bool isReady = true;
			this._missionViewsContainer.ForEach(delegate(MissionView missionView)
			{
				bool flag = missionView.IsReady();
				isReady = isReady && flag;
			});
			return isReady;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00018E83 File Offset: 0x00017083
		private void CameraTick(Mission mission, float realDt)
		{
			if (mission.CurrentState == Mission.State.Continuing)
			{
				this.CheckForUpdateCamera(realDt);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00018E95 File Offset: 0x00017095
		void IMissionSystemHandler.UpdateCamera(Mission mission, float realDt)
		{
			this.CameraTick(mission, realDt);
			if (mission.CurrentState == Mission.State.Continuing && !mission.MissionEnded)
			{
				MBWindowManager.PreDisplay();
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00018EB8 File Offset: 0x000170B8
		protected virtual void AfterMissionTick(Mission mission, float realDt)
		{
			if ((mission.CurrentState == Mission.State.Continuing || (mission.MissionEnded && mission.CurrentState != Mission.State.Over)) && Game.Current.CheatMode && this.IsCheatGhostMode && Agent.Main != null && this.InputManager.IsHotKeyPressed("MissionScreenHotkeyTeleportMainAgent"))
			{
				this.TeleportMainAgentToCameraFocusForCheat();
			}
			if (this.SceneLayer.Input.IsGameKeyPressed(4) && !base.DebugInput.IsAltDown() && MBEditor.EditModeEnabled && MBEditor.IsEditModeOn)
			{
				MBEditor.LeaveEditMissionMode();
			}
			if (mission.Scene == null)
			{
				MBDebug.Print("Mission is null on MissionScreen::OnFrameTick second phase", 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00018F6A File Offset: 0x0001716A
		void IMissionSystemHandler.AfterMissionTick(Mission mission, float realDt)
		{
			this.AfterMissionTick(mission, realDt);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00018F74 File Offset: 0x00017174
		IEnumerable<MissionBehavior> IMissionSystemHandler.OnAddBehaviors(IEnumerable<MissionBehavior> behaviors, Mission mission, string missionName, bool addDefaultMissionBehaviors)
		{
			if (addDefaultMissionBehaviors)
			{
				behaviors = this.AddDefaultMissionBehaviorsTo(mission, behaviors);
			}
			behaviors = ViewCreatorManager.CollectMissionBehaviors(missionName, mission, behaviors);
			return behaviors;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00018F8F File Offset: 0x0001718F
		private void HandleInputs()
		{
			if (!MBEditor.IsEditorMissionOn() && this.MissionStartedRendering() && this.SceneLayer.Input.IsHotKeyReleased("ToggleEscapeMenu") && !LoadingWindow.IsLoadingWindowActive)
			{
				this.OnEscape();
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00018FC4 File Offset: 0x000171C4
		public void OnEscape()
		{
			if (this.IsMissionTickable && !ScreenFadeController.IsFadeActive)
			{
				foreach (MissionBehavior missionBehavior in (from v in this.Mission.MissionBehaviors
					where v is MissionView
					orderby ((MissionView)v).ViewOrderPriority
					select v).ToList<MissionBehavior>())
				{
					MissionView missionView = missionBehavior as MissionView;
					if (!this.IsMissionTickable)
					{
						break;
					}
					if (missionView.OnEscape())
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00019090 File Offset: 0x00017290
		bool IMissionSystemHandler.RenderIsReady()
		{
			return this.MissionStartedRendering();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00019098 File Offset: 0x00017298
		void IMissionListener.OnEndMission()
		{
			this._agentToFollowOverride = null;
			this.LastFollowedAgent = null;
			this.LastFollowedAgentVisuals = null;
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.None));
			this._missionViewsContainer.ForEach(delegate(MissionView missionView)
			{
				missionView.OnMissionScreenFinalize();
				this.UnregisterView(missionView);
			});
			CraftedDataViewManager.Clear();
			this.Mission.RemoveListener(this);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000190F7 File Offset: 0x000172F7
		void IMissionListener.OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType)
		{
			agent.ClearEquipment();
			agent.AgentVisuals.ClearVisualComponents(false, false);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0001910C File Offset: 0x0001730C
		void IMissionListener.OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
		{
			switch (creationType)
			{
			case Agent.CreationType.FromRoster:
			case Agent.CreationType.FromCharacterObj:
			{
				bool flag = false;
				Random random = null;
				bool randomizeColors = agent.RandomizeColors;
				uint num;
				uint num2;
				if (randomizeColors)
				{
					int bodyPropertiesSeed = agent.BodyPropertiesSeed;
					random = new Random(bodyPropertiesSeed);
					Color color;
					Color color2;
					AgentVisuals.GetRandomClothingColors(bodyPropertiesSeed, Color.FromUint(agent.ClothingColor1), Color.FromUint(agent.ClothingColor2), out color, out color2);
					num = color.ToUnsignedInteger();
					num2 = color2.ToUnsignedInteger();
				}
				else
				{
					num = agent.ClothingColor1;
					num2 = agent.ClothingColor2;
				}
				for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
				{
					if (!agent.SpawnEquipment[equipmentIndex].IsVisualEmpty)
					{
						ItemObject itemObject = agent.SpawnEquipment[equipmentIndex].CosmeticItem ?? agent.SpawnEquipment[equipmentIndex].Item;
						bool flag2 = equipmentIndex == EquipmentIndex.Body && agent.SpawnEquipment[EquipmentIndex.Gloves].Item != null && !agent.SpawnEquipment[EquipmentIndex.Gloves].Item.ArmorComponent.IsNoSlim;
						bool flag3 = agent.Age >= 14f && agent.IsFemale;
						MetaMesh multiMesh = agent.SpawnEquipment[equipmentIndex].GetMultiMesh(flag3, flag2, true);
						if (multiMesh != null)
						{
							if (randomizeColors)
							{
								multiMesh.SetGlossMultiplier(AgentVisuals.GetRandomGlossFactor(random));
							}
							if (!itemObject.IsUsingTableau)
							{
								goto IL_0247;
							}
							bool flag4;
							if (agent == null)
							{
								flag4 = null != null;
							}
							else
							{
								IAgentOriginBase origin = agent.Origin;
								flag4 = ((origin != null) ? origin.Banner : null) != null;
							}
							if (!flag4)
							{
								goto IL_0247;
							}
							for (int i = 0; i < multiMesh.MeshCount; i++)
							{
								Mesh currentMesh = multiMesh.GetMeshAtIndex(i);
								Mesh currentMesh3 = currentMesh;
								if (currentMesh3 != null && !currentMesh3.HasTag("dont_use_tableau"))
								{
									Mesh currentMesh2 = currentMesh;
									if (currentMesh2 != null && currentMesh2.HasTag("banner_replacement_mesh"))
									{
										BannerVisual bannerVisual = (BannerVisual)agent.Origin.Banner.BannerVisual;
										BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
										bannerVisual.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
										{
											MissionScreen.ApplyBannerTextureToMesh(currentMesh, t);
										}, true);
										currentMesh.ManualInvalidate();
										break;
									}
								}
								currentMesh.ManualInvalidate();
							}
							IL_02BC:
							if (itemObject.UsingFacegenScaling)
							{
								multiMesh.UseHeadBoneFaceGenScaling(agent.AgentVisuals.GetSkeleton(), agent.Monster.HeadLookDirectionBoneIndex, agent.AgentVisuals.GetFacegenScalingMatrix());
							}
							Skeleton skeleton = agent.AgentVisuals.GetSkeleton();
							int num3 = ((skeleton != null) ? skeleton.GetComponentCount(GameEntity.ComponentType.ClothSimulator) : (-1));
							agent.AgentVisuals.AddMultiMesh(multiMesh, MBAgentVisuals.GetBodyMeshIndex(equipmentIndex));
							multiMesh.ManualInvalidate();
							int num4 = ((skeleton != null) ? skeleton.GetComponentCount(GameEntity.ComponentType.ClothSimulator) : (-1));
							if (skeleton != null && equipmentIndex == EquipmentIndex.Cape && num4 > num3)
							{
								GameEntityComponent componentAtIndex = skeleton.GetComponentAtIndex(GameEntity.ComponentType.ClothSimulator, num4 - 1);
								agent.SetCapeClothSimulator(componentAtIndex);
								goto IL_0363;
							}
							goto IL_0363;
							IL_0247:
							if (itemObject.IsUsingTeamColor)
							{
								for (int j = 0; j < multiMesh.MeshCount; j++)
								{
									Mesh meshAtIndex = multiMesh.GetMeshAtIndex(j);
									if (!meshAtIndex.HasTag("no_team_color"))
									{
										meshAtIndex.Color = num;
										meshAtIndex.Color2 = num2;
										Material material = meshAtIndex.GetMaterial().CreateCopy();
										material.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
										meshAtIndex.SetMaterial(material);
										flag = true;
									}
									meshAtIndex.ManualInvalidate();
								}
								goto IL_02BC;
							}
							goto IL_02BC;
						}
						IL_0363:
						if (equipmentIndex == EquipmentIndex.Body && !string.IsNullOrEmpty(itemObject.ArmBandMeshName))
						{
							MetaMesh copy = MetaMesh.GetCopy(itemObject.ArmBandMeshName, true, true);
							if (copy != null)
							{
								if (randomizeColors)
								{
									copy.SetGlossMultiplier(AgentVisuals.GetRandomGlossFactor(random));
								}
								if (itemObject.IsUsingTeamColor)
								{
									for (int k = 0; k < copy.MeshCount; k++)
									{
										Mesh meshAtIndex2 = copy.GetMeshAtIndex(k);
										if (!meshAtIndex2.HasTag("no_team_color"))
										{
											meshAtIndex2.Color = num;
											meshAtIndex2.Color2 = num2;
											Material material2 = meshAtIndex2.GetMaterial().CreateCopy();
											material2.AddMaterialShaderFlag("use_double_colormap_with_mask_texture", false);
											meshAtIndex2.SetMaterial(material2);
											flag = true;
										}
										meshAtIndex2.ManualInvalidate();
									}
								}
								agent.AgentVisuals.AddMultiMesh(copy, MBAgentVisuals.GetBodyMeshIndex(equipmentIndex));
								copy.ManualInvalidate();
							}
						}
					}
				}
				ItemObject item = agent.SpawnEquipment[EquipmentIndex.Body].Item;
				if (item != null)
				{
					int lodAtlasIndex = item.LodAtlasIndex;
					if (lodAtlasIndex != -1)
					{
						agent.AgentVisuals.SetLodAtlasShadingIndex(lodAtlasIndex, flag, agent.ClothingColor1, agent.ClothingColor2);
					}
				}
				break;
			}
			case Agent.CreationType.FromHorseObj:
				MountVisualCreator.AddMountMeshToAgentVisual(agent.AgentVisuals, agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot].Item, agent.SpawnEquipment[EquipmentIndex.HorseHarness].Item, agent.HorseCreationKey, agent);
				break;
			}
			ArmorComponent.ArmorMaterialTypes armorMaterialTypes = ArmorComponent.ArmorMaterialTypes.None;
			ItemObject item2 = agent.SpawnEquipment[EquipmentIndex.Body].Item;
			if (item2 != null)
			{
				armorMaterialTypes = item2.ArmorComponent.MaterialType;
			}
			agent.SetBodyArmorMaterialType(armorMaterialTypes);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00019619 File Offset: 0x00017819
		void IMissionListener.OnConversationCharacterChanged()
		{
			this._cameraAddSpecialMovement = true;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00019624 File Offset: 0x00017824
		void IMissionListener.OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			if (this.Mission.Mode == MissionMode.Conversation && oldMissionMode != MissionMode.Conversation)
			{
				this._cameraAddSpecialMovement = true;
				this._cameraApplySpecialMovementsInstantly = atStart;
			}
			else if (this.Mission.Mode == MissionMode.Battle && oldMissionMode == MissionMode.Deployment && this.CombatCamera != null)
			{
				this._cameraAddSpecialMovement = true;
				this._cameraApplySpecialMovementsInstantly = atStart || this._playerDeploymentCancelled;
				Agent agentToFollow = this.GetSpectatingData(this.CombatCamera.Position).AgentToFollow;
				if (!atStart)
				{
					this.LastFollowedAgent = agentToFollow;
				}
				this._cameraSpecialCurrentAddedElevation = this.CameraElevation;
				if (agentToFollow != null)
				{
					this._cameraSpecialCurrentAddedBearing = MBMath.WrapAngle(this.CameraBearing - agentToFollow.LookDirectionAsAngle);
					this._cameraSpecialCurrentPositionToAdd = this.CombatCamera.Position - agentToFollow.VisualPosition;
					this.CameraBearing = agentToFollow.LookDirectionAsAngle;
				}
				else
				{
					this._cameraSpecialCurrentAddedBearing = 0f;
					this._cameraSpecialCurrentPositionToAdd = Vec3.Zero;
					this.CameraBearing = 0f;
				}
				this.CameraElevation = 0f;
			}
			if (((this.Mission.Mode == MissionMode.Conversation || this.Mission.Mode == MissionMode.Barter) && oldMissionMode != MissionMode.Conversation && oldMissionMode != MissionMode.Barter) || ((oldMissionMode == MissionMode.Conversation || oldMissionMode == MissionMode.Barter) && this.Mission.Mode != MissionMode.Conversation && this.Mission.Mode != MissionMode.Barter))
			{
				this._cameraAddSpecialMovement = true;
				this._cameraAddSpecialPositionalMovement = true;
				this._cameraApplySpecialMovementsInstantly = atStart;
			}
			this._cameraHeightLimit = 0f;
			if (this.Mission.Mode == MissionMode.Deployment)
			{
				GameEntity gameEntity;
				if (this.Mission.PlayerTeam.Side == BattleSideEnum.Attacker)
				{
					gameEntity = this.Mission.Scene.FindEntityWithTag("strategyCameraAttacker") ?? this.Mission.Scene.FindEntityWithTag("strategyCameraDefender");
				}
				else
				{
					gameEntity = this.Mission.Scene.FindEntityWithTag("strategyCameraDefender") ?? this.Mission.Scene.FindEntityWithTag("strategyCameraAttacker");
				}
				if (gameEntity != null)
				{
					this._cameraHeightLimit = gameEntity.GetGlobalFrame().origin.z;
					return;
				}
			}
			else
			{
				GameEntity gameEntity2 = this.Mission.Scene.FindEntityWithTag("camera_height_limiter");
				if (gameEntity2 != null)
				{
					this._cameraHeightLimit = gameEntity2.GetGlobalFrame().origin.z;
				}
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0001987C File Offset: 0x00017A7C
		void IMissionListener.OnResetMission()
		{
			this._agentToFollowOverride = null;
			this.LastFollowedAgent = null;
			this.LastFollowedAgentVisuals = null;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00019894 File Offset: 0x00017A94
		void IMissionListener.OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			if ((!GameNetwork.IsMultiplayer && this.Mission.Mode == MissionMode.Deployment) & isFirstPlan)
			{
				Team playerTeam = this.Mission.PlayerTeam;
				if (playerTeam == team)
				{
					DeploymentMissionController missionBehavior = this.Mission.GetMissionBehavior<DeploymentMissionController>();
					bool flag = missionBehavior != null && MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle();
					GameEntity gameEntity;
					if (playerTeam.Side == BattleSideEnum.Attacker)
					{
						gameEntity = this.Mission.Scene.FindEntityWithTag("strategyCameraAttacker") ?? this.Mission.Scene.FindEntityWithTag("strategyCameraDefender");
					}
					else
					{
						gameEntity = this.Mission.Scene.FindEntityWithTag("strategyCameraDefender") ?? this.Mission.Scene.FindEntityWithTag("strategyCameraAttacker");
					}
					if (gameEntity == null && flag)
					{
						MatrixFrame zoomFocusFrame = this.Mission.DeploymentPlan.GetZoomFocusFrame(playerTeam);
						MatrixFrame matrixFrame = zoomFocusFrame;
						float fovHorizontal = this.CombatCamera.GetFovHorizontal();
						float num = Math.Max(this.Mission.DeploymentPlan.GetZoomOffset(playerTeam, fovHorizontal), 32f);
						matrixFrame.rotation.RotateAboutSide(-0.5235988f);
						matrixFrame.origin -= num * matrixFrame.rotation.f;
						bool flag2 = false;
						if (this.Mission.IsPositionInsideBoundaries(matrixFrame.origin.AsVec2))
						{
							flag2 = true;
						}
						else
						{
							ICollection<Vec2> value = this.Mission.Boundaries.Where<KeyValuePair<string, ICollection<Vec2>>>((KeyValuePair<string, ICollection<Vec2>> boundary) => boundary.Key == "walk_area").First<KeyValuePair<string, ICollection<Vec2>>>().Value;
							MBList<Vec2> mblist = value.ToMBList<Vec2>();
							mblist.AddRange(value);
							Vec2 vec = matrixFrame.rotation.f.AsVec2.Normalized();
							Vec2 asVec = matrixFrame.origin.AsVec2;
							Vec2 vec2;
							if (MBMath.IntersectRayWithPolygon(asVec, vec, mblist, out vec2))
							{
								Vec2 asVec2 = zoomFocusFrame.origin.AsVec2;
								float num2 = vec2.Distance(asVec2);
								float num3 = asVec.Distance(asVec2);
								float num4 = num2 / Math.Max(num3, 0.1f) * matrixFrame.origin.z;
								Vec3 vec3 = new Vec3(vec2, num4, -1f);
								matrixFrame.origin = vec3;
								flag2 = true;
							}
						}
						if (!flag2)
						{
							matrixFrame = zoomFocusFrame;
							matrixFrame.origin.z = matrixFrame.origin.z + 20f;
						}
						this.CombatCamera.Frame = matrixFrame;
						this.CameraBearing = matrixFrame.rotation.f.RotationZ;
						this.CameraElevation = matrixFrame.rotation.f.RotationX;
					}
					this._playerDeploymentCancelled = missionBehavior != null && !flag;
				}
			}
			this._missionViewsContainer.ForEach(delegate(MissionView missionView)
			{
				missionView.OnDeploymentPlanMade(team, isFirstPlan);
			});
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00019B90 File Offset: 0x00017D90
		private void CalculateNewBearingAndElevationForFirstPerson(Agent agentToFollow, float oldCameraBearing, float oldCameraElevation, float cameraBearingDelta, float cameraElevationDelta, out float newBearing, out float newElevation)
		{
			newBearing = MBMath.WrapAngle(oldCameraBearing + cameraBearingDelta);
			newElevation = MBMath.WrapAngle(oldCameraElevation + cameraElevationDelta);
			AnimFlags currentAnimationFlag = agentToFollow.GetCurrentAnimationFlag(0);
			AnimFlags currentAnimationFlag2 = agentToFollow.GetCurrentAnimationFlag(1);
			Vec2 bodyRotationConstraint = agentToFollow.GetBodyRotationConstraint(1);
			bool flag = bodyRotationConstraint.IsNonZero();
			if ((agentToFollow.HasMount && flag) || currentAnimationFlag.HasAnyFlag(AnimFlags.anf_lock_movement | AnimFlags.anf_synch_with_ladder_movement) || currentAnimationFlag2.HasAnyFlag(AnimFlags.anf_lock_movement | AnimFlags.anf_synch_with_ladder_movement) || agentToFollow.MovementLockedState == AgentMovementLockedState.FrameLocked)
			{
				MatrixFrame boneEntitialFrame = agentToFollow.GetBoneEntitialFrame(agentToFollow.Monster.ThoraxLookDirectionBoneIndex, true);
				MatrixFrame frame = agentToFollow.AgentVisuals.GetFrame();
				float num = (flag ? 0f : boneEntitialFrame.rotation.f.RotationZ);
				float num2 = num + frame.rotation.f.RotationZ;
				if (flag)
				{
					bodyRotationConstraint.x = Math.Max(-3.1414928f, bodyRotationConstraint.x - 0.9f);
					bodyRotationConstraint.y = Math.Min(3.1414928f, bodyRotationConstraint.y + 0.9f);
				}
				else
				{
					bodyRotationConstraint.y = 50f.ToRadians();
					if (Math.Abs(num) > bodyRotationConstraint.y - 0.0001f)
					{
						float num3 = Math.Abs(num) - (bodyRotationConstraint.y - 0.0001f);
						bodyRotationConstraint.y += num3 * 0.5f;
						num2 += num3 * ((num < 0f) ? 0.25f : (-0.25f));
					}
					bodyRotationConstraint.x = -bodyRotationConstraint.y;
				}
				if (num2 <= -3.1415927f)
				{
					num2 += 6.2831855f;
				}
				else if (num2 > 3.1415927f)
				{
					num2 -= 6.2831855f;
				}
				float num4 = MBMath.WrapAngle(oldCameraBearing - num2);
				num4 += cameraBearingDelta;
				if (num4 > bodyRotationConstraint.y)
				{
					num4 = bodyRotationConstraint.y;
				}
				else if (num4 < bodyRotationConstraint.x)
				{
					num4 = bodyRotationConstraint.x;
				}
				newBearing = MBMath.WrapAngle(num4 + num2);
				float num5 = -25f.ToRadians();
				if (!flag && MBMath.GetSmallestDifferenceBetweenTwoAngles(frame.rotation.f.RotationX, newElevation) < num5)
				{
					newElevation = frame.rotation.f.RotationX + num5;
				}
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00019DC8 File Offset: 0x00017FC8
		private static void ApplyBannerTextureToMesh(Mesh armorMesh, Texture bannerTexture)
		{
			if (armorMesh != null)
			{
				Material material = armorMesh.GetMaterial().CreateCopy();
				material.SetTexture(Material.MBTextureType.DiffuseMap2, bannerTexture);
				uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
				ulong shaderFlags = material.GetShaderFlags();
				material.SetShaderFlags(shaderFlags | (ulong)num);
				armorMesh.SetMaterial(material);
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00019E1D File Offset: 0x0001801D
		void IChatLogHandlerScreen.TryUpdateChatLogLayerParameters(ref bool isTeamChatAvailable, ref bool inputEnabled, ref bool isToggleChatHintAvailable, ref bool isMouseVisible, ref InputContext inputContext)
		{
			if (this.SceneLayer != null)
			{
				inputEnabled = true;
				inputContext = this.SceneLayer.Input;
			}
		}

		// Token: 0x0400016A RID: 362
		public const int LoadingScreenFramesLeftInitial = 15;

		// Token: 0x0400016B RID: 363
		private const float LookUpLimit = 1.1219975f;

		// Token: 0x0400016C RID: 364
		private const float LookDownLimit = -1.3659099f;

		// Token: 0x0400016D RID: 365
		public const float FirstPersonNearClippingDistance = 0.065f;

		// Token: 0x0400016E RID: 366
		public const float ThirdPersonNearClippingDistance = 0.1f;

		// Token: 0x0400016F RID: 367
		public const float FarClippingDistance = 12500f;

		// Token: 0x04000170 RID: 368
		private const float HoldTimeForCameraToggle = 0.5f;

		// Token: 0x04000171 RID: 369
		public const float MinCameraAddedDistance = 0.7f;

		// Token: 0x04000172 RID: 370
		public const float MinCameraDistanceHardLimit = 0.48f;

		// Token: 0x04000173 RID: 371
		public const float DefaultViewAngle = 65f;

		// Token: 0x04000174 RID: 372
		public const float MaxCameraAddedDistance = 2.4f;

		// Token: 0x04000175 RID: 373
		private const int _cheatTimeSpeedRequestId = 1121;

		// Token: 0x04000176 RID: 374
		private const string AttackerCameraEntityTag = "strategyCameraAttacker";

		// Token: 0x04000177 RID: 375
		private const string DefenderCameraEntityTag = "strategyCameraDefender";

		// Token: 0x04000178 RID: 376
		private const string CameraHeightLimiterTag = "camera_height_limiter";

		// Token: 0x0400017A RID: 378
		public Func<BasicCharacterObject> GetSpectatedCharacter;

		// Token: 0x0400017D RID: 381
		private MissionScreen.GatherCustomAgentListToSpectateDelegate _gatherCustomAgentListToSpectate;

		// Token: 0x0400017F RID: 383
		private float _cameraRayCastOffset;

		// Token: 0x04000180 RID: 384
		private bool _forceCanZoom;

		// Token: 0x04000183 RID: 387
		public bool AllowInputWithCustomCamera;

		// Token: 0x04000184 RID: 388
		private readonly Vec3[] _cameraNearPlanePoints = new Vec3[4];

		// Token: 0x04000185 RID: 389
		private readonly Vec3[] _cameraBoxPoints = new Vec3[8];

		// Token: 0x04000186 RID: 390
		private Vec3 _cameraTarget;

		// Token: 0x0400018A RID: 394
		private float _cameraBearingDelta;

		// Token: 0x0400018B RID: 395
		private float _cameraElevationDelta;

		// Token: 0x0400018C RID: 396
		private float _cameraSpecialTargetAddedBearing;

		// Token: 0x0400018D RID: 397
		private float _cameraSpecialCurrentAddedBearing;

		// Token: 0x0400018E RID: 398
		private float _cameraSpecialTargetAddedElevation;

		// Token: 0x0400018F RID: 399
		private float _cameraSpecialCurrentAddedElevation;

		// Token: 0x04000190 RID: 400
		private Vec3 _cameraSpecialTargetPositionToAdd;

		// Token: 0x04000191 RID: 401
		private Vec3 _cameraSpecialCurrentPositionToAdd;

		// Token: 0x04000192 RID: 402
		private float _cameraSpecialTargetDistanceToAdd;

		// Token: 0x04000193 RID: 403
		private float _cameraSpecialCurrentDistanceToAdd;

		// Token: 0x04000194 RID: 404
		private bool _cameraAddSpecialMovement;

		// Token: 0x04000195 RID: 405
		private bool _cameraAddSpecialPositionalMovement;

		// Token: 0x04000196 RID: 406
		private bool _cameraApplySpecialMovementsInstantly;

		// Token: 0x04000197 RID: 407
		private float _cameraSpecialCurrentFOV;

		// Token: 0x04000198 RID: 408
		private float _cameraSpecialTargetFOV;

		// Token: 0x04000199 RID: 409
		private float _cameraTargetAddedHeight;

		// Token: 0x0400019A RID: 410
		private float _cameraDeploymentHeightToAdd;

		// Token: 0x0400019B RID: 411
		private float _lastCameraAddedDistance;

		// Token: 0x0400019D RID: 413
		private float _cameraAddedElevation;

		// Token: 0x0400019E RID: 414
		private float _cameraHeightLimit;

		// Token: 0x0400019F RID: 415
		private float _cameraShakeStartTime;

		// Token: 0x040001A0 RID: 416
		private float _cameraShakeCurrentTimeWithFrequency;

		// Token: 0x040001A1 RID: 417
		private float _cameraShakeIntensity;

		// Token: 0x040001A2 RID: 418
		private float _currentViewBlockingBodyCoeff;

		// Token: 0x040001A3 RID: 419
		private float _targetViewBlockingBodyCoeff;

		// Token: 0x040001A4 RID: 420
		private bool _applySmoothTransitionToVirtualEyeCamera;

		// Token: 0x040001A6 RID: 422
		private Vec3 _cameraSpeed;

		// Token: 0x040001A7 RID: 423
		private float _cameraSpeedMultiplier;

		// Token: 0x040001A8 RID: 424
		private bool _cameraSmoothMode;

		// Token: 0x040001A9 RID: 425
		private bool _fixCamera;

		// Token: 0x040001AA RID: 426
		private int _shiftSpeedMultiplier = 3;

		// Token: 0x040001AB RID: 427
		private bool _tickEditor;

		// Token: 0x040001AC RID: 428
		private bool _playerDeploymentCancelled;

		// Token: 0x040001AD RID: 429
		private bool _isDeactivated;

		// Token: 0x040001B3 RID: 435
		private bool _zoomToggled;

		// Token: 0x040001B4 RID: 436
		private float _zoomAmount;

		// Token: 0x040001B5 RID: 437
		private float _cameraToggleStartTime = float.MaxValue;

		// Token: 0x040001B7 RID: 439
		private bool _displayingDialog;

		// Token: 0x040001B8 RID: 440
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x040001B9 RID: 441
		private ICameraModeLogic _missionCameraModeLogic;

		// Token: 0x040001BA RID: 442
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x040001BB RID: 443
		private readonly List<object> _objectsWithActiveRadialMenu;

		// Token: 0x040001BC RID: 444
		private bool _isPlayerAgentAdded = true;

		// Token: 0x040001BD RID: 445
		private bool _isRenderingStarted;

		// Token: 0x040001BE RID: 446
		private bool _onSceneRenderingStartedCalled;

		// Token: 0x040001BF RID: 447
		private int _loadingScreenFramesLeft = 15;

		// Token: 0x040001C0 RID: 448
		private bool _resetDraggingMode;

		// Token: 0x040001C1 RID: 449
		private bool _rightButtonDraggingMode;

		// Token: 0x040001C2 RID: 450
		private int _requestedSpectatorCycleDirection;

		// Token: 0x040001C3 RID: 451
		private Vec2 _clickedPositionPixel = Vec2.Zero;

		// Token: 0x040001C4 RID: 452
		private Agent _agentToFollowOverride;

		// Token: 0x040001C5 RID: 453
		private SpectatorCameraTypes? _spectatorCameraOverride;

		// Token: 0x040001C6 RID: 454
		private bool _suppressSpectatorCyclingThisFrame;

		// Token: 0x040001C7 RID: 455
		private Agent _lastFollowedAgent;

		// Token: 0x040001C9 RID: 457
		private bool _isGamepadActive;

		// Token: 0x040001CA RID: 458
		private readonly MissionViewsContainer _missionViewsContainer;

		// Token: 0x040001CC RID: 460
		private MissionState _missionState;

		// Token: 0x020000C4 RID: 196
		// (Invoke) Token: 0x0600061E RID: 1566
		public delegate void OnSpectateAgentDelegate(Agent followedAgent);

		// Token: 0x020000C5 RID: 197
		// (Invoke) Token: 0x06000622 RID: 1570
		public delegate List<Agent> GatherCustomAgentListToSpectateDelegate(Agent forcedAgentToInclude);
	}
}
