using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000084 RID: 132
	public class SpectatorCameraView : MissionView
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x00025D23 File Offset: 0x00023F23
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MultiplayerHotkeyCategory"));
			ScreenManager.TrySetFocus(base.MissionScreen.SceneLayer);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00025D5C File Offset: 0x00023F5C
		public override void AfterStart()
		{
			for (int i = 0; i < 9; i++)
			{
				this._spectateCameraFrames.Add(MatrixFrame.Identity);
			}
			for (int j = 0; j < 9; j++)
			{
				string text = "spectate_cam_" + j.ToString();
				List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag(text).ToList<GameEntity>();
				if (list.Count > 0)
				{
					this._spectateCameraFrames[j] = list[0].GetGlobalFrame();
					this._spectateCameraFrameIsSet[j] = true;
				}
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00025DE5 File Offset: 0x00023FE5
		public override void OnPreDisplayMissionTick(float dt)
		{
			base.OnPreDisplayMissionTick(dt);
			if (!this.IsSpectatorPeer())
			{
				return;
			}
			this.HandleClickToFollow(base.MissionScreen.SceneLayer.Input);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00025E10 File Offset: 0x00024010
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (!this.IsSpectatorPeer())
			{
				return;
			}
			InputContext input = base.MissionScreen.SceneLayer.Input;
			if (input.IsControlDown())
			{
				this.HandleStoreCameraPosition(input);
			}
			else
			{
				this.HandleRecallCameraPosition(input);
			}
			this.HandlePovToggle(input);
			this.HandleCycleTarget(input);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00025E64 File Offset: 0x00024064
		private void HandleCycleTarget(InputContext input)
		{
			if (input.IsHotKeyPressed("CycleSpectatorTargetPrevious"))
			{
				base.MissionScreen.RequestSpectatorCycle(-1);
				return;
			}
			if (input.IsHotKeyPressed("CycleSpectatorTargetNext"))
			{
				base.MissionScreen.RequestSpectatorCycle(1);
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00025E9C File Offset: 0x0002409C
		private void HandleStoreCameraPosition(InputContext input)
		{
			string[] storeCameraPositionHotKeys = MultiplayerHotkeyCategory.StoreCameraPositionHotKeys;
			int num = 0;
			while (num < 9 && num < storeCameraPositionHotKeys.Length)
			{
				if (input.IsHotKeyPressed(storeCameraPositionHotKeys[num]))
				{
					this._spectateCameraFrames[num] = base.MissionScreen.CombatCamera.Frame;
					this._spectateCameraFrameIsSet[num] = true;
					return;
				}
				num++;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00025EF4 File Offset: 0x000240F4
		private void HandleRecallCameraPosition(InputContext input)
		{
			string[] spectateCameraPositionHotKeys = MultiplayerHotkeyCategory.SpectateCameraPositionHotKeys;
			int num = 0;
			while (num < 9 && num < spectateCameraPositionHotKeys.Length)
			{
				if (input.IsHotKeyPressed(spectateCameraPositionHotKeys[num]))
				{
					if (!this._spectateCameraFrameIsSet[num])
					{
						return;
					}
					base.MissionScreen.UpdateFreeCamera(this._spectateCameraFrames[num]);
					return;
				}
				else
				{
					num++;
				}
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00025F48 File Offset: 0x00024148
		public override void OnMissionScreenFinalize()
		{
			MissionScreen missionScreen = base.MissionScreen;
			if (missionScreen != null)
			{
				missionScreen.SetSpectatorCameraOverride(null);
			}
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00025F78 File Offset: 0x00024178
		private void HandlePovToggle(InputContext input)
		{
			if (!input.IsHotKeyReleased("CycleSpectatorCamera"))
			{
				return;
			}
			if (!MultiplayerOptions.IsSpectatorCameraFreedomAllowed())
			{
				return;
			}
			if (!this._hasCameraCycleMode)
			{
				this._cameraCycleMode = (SpectatorCameraTypes)MultiplayerOptions.OptionType.SpectatorCamera.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				this._hasCameraCycleMode = true;
			}
			SpectatorCameraTypes cameraCycleMode = this._cameraCycleMode;
			if (cameraCycleMode != SpectatorCameraTypes.Free)
			{
				if (cameraCycleMode != SpectatorCameraTypes.LockToAnyPlayer)
				{
					this._cameraCycleMode = SpectatorCameraTypes.Free;
				}
				else
				{
					this._cameraCycleMode = SpectatorCameraTypes.OrbitAroundTarget;
				}
			}
			else
			{
				this._cameraCycleMode = SpectatorCameraTypes.LockToAnyPlayer;
			}
			base.MissionScreen.SetSpectatorCameraOverride(new SpectatorCameraTypes?(this._cameraCycleMode));
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00025FF8 File Offset: 0x000241F8
		private void HandleClickToFollow(InputContext input)
		{
			if (!input.IsKeyReleased(InputKey.LeftMouseButton) || base.MissionScreen.IsRightButtonDragging)
			{
				return;
			}
			Vec2 mousePositionRanged = input.GetMousePositionRanged();
			Agent agent = this.FindAgentNearestToScreenPoint(mousePositionRanged);
			if (agent != null)
			{
				base.MissionScreen.SetAgentToFollow(agent);
				base.MissionScreen.SuppressSpectatorCyclingThisFrame();
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0002604C File Offset: 0x0002424C
		private Agent FindAgentNearestToScreenPoint(Vec2 screenPoint)
		{
			Agent agent = null;
			float num = 0.0016f;
			foreach (Agent agent2 in Mission.Current.Agents)
			{
				if (agent2.IsActive() && agent2.IsCameraAttachable() && agent2.MissionPeer != null)
				{
					Vec3 vec = agent2.VisualPosition + new Vec3(0f, 0f, 1.2f, -1f);
					Vec2 vec2 = base.MissionScreen.SceneLayer.WorldPointToScreenPoint(vec);
					if (vec2.x >= 0f && vec2.x <= 1f && vec2.y >= 0f && vec2.y <= 1f)
					{
						float num2 = vec2.DistanceSquared(screenPoint);
						if (num2 < num)
						{
							num = num2;
							agent = agent2;
						}
					}
				}
			}
			return agent;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00026154 File Offset: 0x00024354
		private bool IsSpectatorPeer()
		{
			return GameNetwork.IsMultiplayer && GameNetwork.IsMyPeerReady && SpectatorHelper.IsLocalPeerSpectator();
		}

		// Token: 0x040002E1 RID: 737
		private const float _clickToFollowMaxScreenDistanceSq = 0.0016f;

		// Token: 0x040002E2 RID: 738
		private const int SpectateCameraSlotCount = 9;

		// Token: 0x040002E3 RID: 739
		private List<MatrixFrame> _spectateCameraFrames = new List<MatrixFrame>();

		// Token: 0x040002E4 RID: 740
		private bool[] _spectateCameraFrameIsSet = new bool[9];

		// Token: 0x040002E5 RID: 741
		private SpectatorCameraTypes _cameraCycleMode;

		// Token: 0x040002E6 RID: 742
		private bool _hasCameraCycleMode;
	}
}
