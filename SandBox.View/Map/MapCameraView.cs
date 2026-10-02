using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map
{
	// Token: 0x02000045 RID: 69
	public class MapCameraView : MapView
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0001415E File Offset: 0x0001235E
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00014166 File Offset: 0x00012366
		protected virtual MapCameraView.CameraFollowMode CurrentCameraFollowMode { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0001416F File Offset: 0x0001236F
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00014177 File Offset: 0x00012377
		public virtual float CameraFastMoveMultiplier { get; protected set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00014180 File Offset: 0x00012380
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00014188 File Offset: 0x00012388
		protected virtual float CameraBearing { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00014191 File Offset: 0x00012391
		protected virtual float MaximumCameraHeight
		{
			get
			{
				return Math.Max(this._customMaximumCameraHeight, Campaign.MapMaximumHeight);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000223 RID: 547 RVA: 0x000141A3 File Offset: 0x000123A3
		// (set) Token: 0x06000224 RID: 548 RVA: 0x000141AB File Offset: 0x000123AB
		protected virtual float CameraBearingVelocity { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000141B4 File Offset: 0x000123B4
		// (set) Token: 0x06000226 RID: 550 RVA: 0x000141BC File Offset: 0x000123BC
		public virtual float CameraDistance { get; protected set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000141C5 File Offset: 0x000123C5
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000141CD File Offset: 0x000123CD
		protected virtual float TargetCameraDistance { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000141D6 File Offset: 0x000123D6
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000141DE File Offset: 0x000123DE
		protected virtual float AdditionalElevation { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000141E7 File Offset: 0x000123E7
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000141EF File Offset: 0x000123EF
		public virtual bool CameraAnimationInProgress { get; protected set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000141F8 File Offset: 0x000123F8
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00014200 File Offset: 0x00012400
		public virtual bool ProcessCameraInput { get; protected set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00014209 File Offset: 0x00012409
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00014211 File Offset: 0x00012411
		public virtual Camera Camera { get; protected set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0001421A File Offset: 0x0001241A
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00014222 File Offset: 0x00012422
		public virtual MatrixFrame CameraFrame
		{
			get
			{
				return this._cameraFrame;
			}
			protected set
			{
				this._cameraFrame = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0001422B File Offset: 0x0001242B
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00014233 File Offset: 0x00012433
		protected virtual Vec3 IdealCameraTarget { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0001423C File Offset: 0x0001243C
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00014243 File Offset: 0x00012443
		private static MapCameraView Instance { get; set; }

		// Token: 0x06000237 RID: 567 RVA: 0x0001424C File Offset: 0x0001244C
		public MapCameraView()
		{
			this.Camera = Camera.CreateCamera();
			this.Camera.SetViewVolume(true, -0.1f, 0.1f, -0.07f, 0.07f, 0.2f, 300f);
			this.Camera.Position = new Vec3(0f, 0f, 10f, -1f);
			this.CameraBearing = 0f;
			this._cameraElevation = 1f;
			this.CameraDistance = 38f;
			this.TargetCameraDistance = 38f;
			this.ProcessCameraInput = true;
			this.CameraFastMoveMultiplier = 4f;
			this._cameraFrame = MatrixFrame.Identity;
			this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.FollowParty;
			this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
			MapCameraView.Instance = this;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00014328 File Offset: 0x00012528
		public virtual void OnActivate(bool leftButtonDraggingMode, Vec3 clickedPosition)
		{
			this.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
			this.CameraBearingVelocity = 0f;
			this.UpdateMapCamera(leftButtonDraggingMode, clickedPosition);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00014344 File Offset: 0x00012544
		public virtual void Initialize()
		{
			if (MobileParty.MainParty != null && PartyBase.MainParty.IsValid)
			{
				float num = 0f;
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				CampaignVec2 campaignVec = MobileParty.MainParty.Position;
				mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num);
				campaignVec = MobileParty.MainParty.Position;
				this.IdealCameraTarget = new Vec3(campaignVec.ToVec2(), num + 1f, -1f);
			}
			this._cameraMoveSfxSoundEventId = SoundEvent.GetEventIdFromString("event:/ui/campaign/focus");
			this._cameraTarget = this.IdealCameraTarget;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x000143CE File Offset: 0x000125CE
		protected internal override void OnFinalize()
		{
			base.OnFinalize();
			MapCameraView.Instance = null;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000143DC File Offset: 0x000125DC
		public virtual void SetCameraMode(MapCameraView.CameraFollowMode cameraMode)
		{
			this.CurrentCameraFollowMode = cameraMode;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000143E5 File Offset: 0x000125E5
		public virtual void ResetCamera(bool resetDistance, bool teleportToMainParty)
		{
			if (teleportToMainParty)
			{
				this.TeleportCameraToMainParty();
			}
			if (resetDistance)
			{
				this.TargetCameraDistance = 15f;
				this.CameraDistance = 15f;
			}
			this.CameraBearing = 0f;
			this._cameraElevation = 1f;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00014420 File Offset: 0x00012620
		public virtual void TeleportCameraToMainParty()
		{
			this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.FollowParty;
			Campaign.Current.CameraFollowParty = MobileParty.MainParty.Party;
			this.IdealCameraTarget = this.GetCameraTargetForParty(Campaign.Current.CameraFollowParty);
			this._lastUsedIdealCameraTarget = new CampaignVec2(this.IdealCameraTarget.AsVec2, !MobileParty.MainParty.IsCurrentlyAtSea);
			this._cameraTarget = this.IdealCameraTarget;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00014490 File Offset: 0x00012690
		public virtual void FastMoveCameraToMainParty()
		{
			this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.FollowParty;
			Campaign.Current.CameraFollowParty = MobileParty.MainParty.Party;
			this.IdealCameraTarget = this.GetCameraTargetForParty(Campaign.Current.CameraFollowParty);
			this._doFastCameraMovementToTarget = true;
			this.TargetCameraDistance = 15f;
			this.OnFastMoveCameraMovementStart();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x000144E6 File Offset: 0x000126E6
		public virtual void FastMoveCameraToPosition(CampaignVec2 target, bool isInMenu)
		{
			if (!isInMenu)
			{
				this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.MoveToPosition;
				this.IdealCameraTarget = this.GetCameraTargetForPosition(target);
				this._doFastCameraMovementToTarget = true;
				this.TargetCameraDistance = 15f;
				this.OnFastMoveCameraMovementStart();
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00014518 File Offset: 0x00012718
		public void OnFastMoveCameraMovementStart()
		{
			this._distanceToIdealCameraTargetToStopCameraSoundEventsSquared = this.IdealCameraTarget.DistanceSquared(this._cameraTarget) * 0.15f;
			if (this._cameraMoveSfxSoundEvent == null || !this._cameraMoveSfxSoundEvent.IsPlaying())
			{
				this._cameraMoveSfxSoundEvent = SoundEvent.CreateEvent(this._cameraMoveSfxSoundEventId, this._mapScene);
				this._cameraMoveSfxSoundEvent.Play();
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0001457D File Offset: 0x0001277D
		public void StopCameraMovementSoundEvents()
		{
			if (this._cameraMoveSfxSoundEvent != null && this._cameraMoveSfxSoundEvent.IsPlaying())
			{
				this._cameraMoveSfxSoundEvent.Release();
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0001459F File Offset: 0x0001279F
		public virtual bool IsCameraLockedToPlayerParty()
		{
			return this.CurrentCameraFollowMode == MapCameraView.CameraFollowMode.FollowParty && Campaign.Current.CameraFollowParty == MobileParty.MainParty.Party;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000145C2 File Offset: 0x000127C2
		public virtual void StartCameraAnimation(CampaignVec2 targetPosition, float animationStopDuration)
		{
			this.CameraAnimationInProgress = true;
			this._cameraAnimationTarget = targetPosition;
			this._cameraAnimationStopDuration = animationStopDuration;
			Campaign.Current.SetTimeSpeed(0);
			Campaign.Current.SetTimeControlModeLock(true);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000145EF File Offset: 0x000127EF
		public virtual void SiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
			if (this.TargetCameraDistance > 18f)
			{
				this.TargetCameraDistance = 18f;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00014609 File Offset: 0x00012809
		public virtual void OnExit()
		{
			this.ProcessCameraInput = true;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00014612 File Offset: 0x00012812
		public virtual void OnEscapeMenuToggled(bool isOpened)
		{
			this.ProcessCameraInput = !isOpened;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00014620 File Offset: 0x00012820
		public virtual void HandleMouse(bool rightMouseButtonPressed, float verticalCameraInput, float mouseMoveY, float dt)
		{
			float num = 0.3f / 700f;
			float num2 = -(700f - MathF.Min(700f, MathF.Max(50f, this.CameraDistance))) * num;
			float num3 = MathF.Max(num2 + 1E-05f, 1.5550884f - this.CalculateCameraElevation(this.CameraDistance));
			if (rightMouseButtonPressed)
			{
				this.AdditionalElevation = MBMath.ClampFloat(this.AdditionalElevation + mouseMoveY * 0.0015f, num2, num3);
			}
			if (verticalCameraInput != 0f)
			{
				this.AdditionalElevation = MBMath.ClampFloat(this.AdditionalElevation - verticalCameraInput * dt, num2, num3);
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000146BA File Offset: 0x000128BA
		public virtual void HandleLeftMouseButtonClick(bool isMouseActive)
		{
			if (isMouseActive && !Hero.MainHero.IsPrisoner)
			{
				this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.FollowParty;
				Campaign.Current.CameraFollowParty = PartyBase.MainParty;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000146E1 File Offset: 0x000128E1
		public virtual void OnSetMapSiegeOverlayState(bool isActive, bool isMapSiegeOverlayViewNull)
		{
			if (isActive && isMapSiegeOverlayViewNull && PlayerSiege.PlayerSiegeEvent != null)
			{
				this.TargetCameraDistance = 13f;
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000146FA File Offset: 0x000128FA
		public virtual void OnRefreshMapSiegeOverlayRequired(bool isMapSiegeOverlayViewNull)
		{
			if (PlayerSiege.PlayerSiegeEvent != null && isMapSiegeOverlayViewNull)
			{
				this.TargetCameraDistance = 13f;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00014714 File Offset: 0x00012914
		public virtual void OnBeforeTick(in MapCameraView.InputInformation inputInformation)
		{
			float num = MathF.Min(1f, MathF.Max(0f, 1f - this.CameraFrame.rotation.f.z)) + 0.15f;
			this._mapScene.SetDepthOfFieldParameters(0.05f, num * 1000f, true);
			this._mapScene.SetDepthOfFieldFocus(0.05f);
			MobileParty mainParty = MobileParty.MainParty;
			if (inputInformation.IsMainPartyValid && this.CameraAnimationInProgress)
			{
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				if (this._cameraAnimationStopDuration > 0f)
				{
					if (this._cameraAnimationTarget.DistanceSquared(this._cameraTarget.AsVec2) < 0.0001f)
					{
						this._cameraAnimationStopDuration = MathF.Max(this._cameraAnimationStopDuration - inputInformation.Dt, 0f);
					}
					else
					{
						this.IdealCameraTarget = this._cameraAnimationTarget.AsVec3() + Vec3.Up;
					}
				}
				else if (MobileParty.MainParty.Position.DistanceSquared(this._cameraTarget.AsVec2) < 0.0001f)
				{
					this.CameraAnimationInProgress = false;
					Campaign.Current.SetTimeControlModeLock(false);
				}
				else
				{
					this.IdealCameraTarget = MobileParty.MainParty.Position.AsVec3() + Vec3.Up;
				}
			}
			bool flag = this.CameraAnimationInProgress;
			if (this.ProcessCameraInput && !this.CameraAnimationInProgress && inputInformation.IsMapReady)
			{
				flag = this.GetMapCameraInput(inputInformation);
			}
			if (flag)
			{
				Vec3 vec = this.IdealCameraTarget - this._cameraTarget;
				Vec3 vec2 = 10f * vec * inputInformation.Dt;
				float num2 = MathF.Sqrt(MathF.Max(this.CameraDistance, 20f)) * 0.15f;
				float num3 = (this._doFastCameraMovementToTarget ? (num2 * 5f) : num2);
				if (vec2.LengthSquared > num3 * num3)
				{
					vec2 = vec2.NormalizedCopy() * num3;
				}
				if (vec2.LengthSquared < num2 * num2)
				{
					this._doFastCameraMovementToTarget = false;
				}
				if (this._distanceToIdealCameraTargetToStopCameraSoundEventsSquared > vec.LengthSquared)
				{
					this.StopCameraMovementSoundEvents();
				}
				this._cameraTarget += vec2;
			}
			else
			{
				this._cameraTarget = this.IdealCameraTarget;
				this._doFastCameraMovementToTarget = false;
				this.StopCameraMovementSoundEvents();
			}
			if (inputInformation.IsMainPartyValid)
			{
				if (inputInformation.CameraFollowModeKeyPressed)
				{
					this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.FollowParty;
				}
				if (!inputInformation.IsInMenu && !inputInformation.MiddleMouseButtonDown && (MobileParty.MainParty == null || MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && (inputInformation.PartyMoveRightKey || inputInformation.PartyMoveLeftKey || inputInformation.PartyMoveUpKey || inputInformation.PartyMoveDownKey))
				{
					float num4 = 0f;
					float num5 = 0f;
					float num6;
					float num7;
					MathF.SinCos(this.CameraBearing, out num6, out num7);
					float num8;
					float num9;
					MathF.SinCos(this.CameraBearing + 1.5707964f, out num8, out num9);
					float num10 = 0.5f;
					if (inputInformation.PartyMoveUpKey)
					{
						num5 += num7 * num10;
						num4 += num6 * num10;
						mainParty.ForceAiNoPathMode = true;
					}
					if (inputInformation.PartyMoveDownKey)
					{
						num5 -= num7 * num10;
						num4 -= num6 * num10;
						mainParty.ForceAiNoPathMode = true;
					}
					if (inputInformation.PartyMoveLeftKey)
					{
						num5 -= num9 * num10;
						num4 -= num8 * num10;
						mainParty.ForceAiNoPathMode = true;
					}
					if (inputInformation.PartyMoveRightKey)
					{
						num5 += num9 * num10;
						num4 += num8 * num10;
						mainParty.ForceAiNoPathMode = true;
					}
					this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.FollowParty;
					CampaignVec2 campaignVec = mainParty.Position + new Vec2(num4, num5);
					MobileParty.NavigationType navigationType;
					if (NavigationHelper.CanPlayerNavigateToPosition(campaignVec, out navigationType))
					{
						mainParty.SetMoveGoToPoint(campaignVec, mainParty.NavigationCapability);
						Campaign.Current.TimeControlMode = CampaignTimeControlMode.StoppablePlay;
					}
				}
				else if (mainParty.ForceAiNoPathMode)
				{
					mainParty.SetMoveGoToPoint(mainParty.Position, mainParty.NavigationCapability);
				}
			}
			this.UpdateMapCamera(inputInformation.LeftButtonDraggingMode, inputInformation.ClickedPosition);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00014B20 File Offset: 0x00012D20
		protected virtual void UpdateMapCamera(bool _leftButtonDraggingMode, Vec3 _clickedPosition)
		{
			this._lastUsedIdealCameraTarget = new CampaignVec2(this.IdealCameraTarget.AsVec2, true);
			MatrixFrame matrixFrame = this.ComputeMapCamera(ref this._cameraTarget, this.CameraBearing, this._cameraElevation, this.CameraDistance, ref this._lastUsedIdealCameraTarget);
			bool flag = !matrixFrame.origin.NearlyEquals(in this._cameraFrame.origin, 1E-05f);
			bool flag2 = !matrixFrame.rotation.NearlyEquals(in this._cameraFrame.rotation, 1E-05f);
			if (flag2 || flag)
			{
				Game.Current.EventManager.TriggerEvent<MapScreen.MainMapCameraMoveEvent>(new MapScreen.MainMapCameraMoveEvent(flag2, flag));
			}
			bool isCurrentlyAtSea = MobileParty.MainParty.IsCurrentlyAtSea;
			this._cameraFrame = matrixFrame;
			float num = 0f;
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			CampaignVec2 campaignVec = new CampaignVec2(this._cameraFrame.origin.AsVec2, !isCurrentlyAtSea);
			mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num);
			num += 0.5f;
			if (this._cameraFrame.origin.z < num)
			{
				if (_leftButtonDraggingMode)
				{
					Vec3 vec = _clickedPosition - Vec3.DotProduct(_clickedPosition - this._cameraFrame.origin, this._cameraFrame.rotation.s) * this._cameraFrame.rotation.s;
					Vec3 vec2 = Vec3.CrossProduct((vec - this._cameraFrame.origin).NormalizedCopy(), (vec - (this._cameraFrame.origin + new Vec3(0f, 0f, num - this._cameraFrame.origin.z, -1f))).NormalizedCopy());
					float num2 = vec2.Normalize();
					this._cameraFrame.origin.z = num;
					this._cameraFrame.rotation.u = this._cameraFrame.rotation.u.RotateAboutAnArbitraryVector(vec2, num2);
					this._cameraFrame.rotation.f = Vec3.CrossProduct(this._cameraFrame.rotation.u, this._cameraFrame.rotation.s).NormalizedCopy();
					this._cameraFrame.rotation.s = Vec3.CrossProduct(this._cameraFrame.rotation.f, this._cameraFrame.rotation.u);
					Vec3 vec3 = -Vec3.Up;
					Vec3 vec4 = -this._cameraFrame.rotation.u;
					Vec3 idealCameraTarget = this.IdealCameraTarget;
					float num3;
					if (MBMath.GetRayPlaneIntersectionPoint(in vec3, in idealCameraTarget, in this._cameraFrame.origin, in vec4, out num3))
					{
						this.IdealCameraTarget = this._cameraFrame.origin + vec4 * num3;
						this._cameraTarget = this.IdealCameraTarget;
					}
					this._cameraElevation = -new Vec2(this._cameraFrame.rotation.f.AsVec2.Length, this._cameraFrame.rotation.f.z).RotationInRadians;
					this.CameraDistance = (this._cameraFrame.origin - this.IdealCameraTarget).Length - 2f;
					this.TargetCameraDistance = this.CameraDistance;
					this.AdditionalElevation = this._cameraElevation - this.CalculateCameraElevation(this.CameraDistance);
					this._lastUsedIdealCameraTarget = new CampaignVec2(this.IdealCameraTarget.AsVec2, true);
					this.ComputeMapCamera(ref this._cameraTarget, this.CameraBearing, this._cameraElevation, this.CameraDistance, ref this._lastUsedIdealCameraTarget);
				}
				else
				{
					float num4 = 0.47123894f;
					int num5 = 0;
					do
					{
						this._cameraElevation += ((this._cameraFrame.origin.z < num) ? num4 : (-num4));
						float num6 = (700f - MathF.Min(700f, MathF.Max(50f, this.CameraDistance))) * -1f * 0.00042857145f;
						float num7 = MathF.Max(num6 + 1E-05f, 1.5550884f - this.CalculateCameraElevation(this.CameraDistance));
						this.AdditionalElevation = this._cameraElevation - this.CalculateCameraElevation(this.CameraDistance);
						this.AdditionalElevation = MBMath.ClampFloat(this.AdditionalElevation, num6, num7);
						this._cameraElevation = this.AdditionalElevation + this.CalculateCameraElevation(this.CameraDistance);
						CampaignVec2 zero = CampaignVec2.Zero;
						this._cameraFrame = this.ComputeMapCamera(ref this._cameraTarget, this.CameraBearing, this._cameraElevation, this.CameraDistance, ref zero);
						IMapScene mapSceneWrapper2 = Campaign.Current.MapSceneWrapper;
						campaignVec = new CampaignVec2(this._cameraFrame.origin.AsVec2, !isCurrentlyAtSea);
						mapSceneWrapper2.GetHeightAtPoint(in campaignVec, ref num);
						num += 0.5f;
						if (num4 > 0.0001f)
						{
							num4 *= 0.5f;
						}
						else
						{
							num5++;
						}
					}
					while (num4 > 0.0001f || (this._cameraFrame.origin.z < num && num5 < 5));
					if (this._cameraFrame.origin.z < num)
					{
						this._cameraFrame.origin.z = num;
						Vec3 vec5 = -Vec3.Up;
						Vec3 vec6 = -this._cameraFrame.rotation.u;
						Vec3 idealCameraTarget2 = this.IdealCameraTarget;
						float num8;
						if (MBMath.GetRayPlaneIntersectionPoint(in vec5, in idealCameraTarget2, in this._cameraFrame.origin, in vec6, out num8) && this.CurrentCameraFollowMode != MapCameraView.CameraFollowMode.MoveToPosition)
						{
							this.IdealCameraTarget = this._cameraFrame.origin + vec6 * num8;
							this._cameraTarget = this.IdealCameraTarget;
							this.CameraDistance = (this._cameraFrame.origin - this.IdealCameraTarget).Length - 2f;
						}
						this._lastUsedIdealCameraTarget = new CampaignVec2(this.IdealCameraTarget.AsVec2, true);
						this.ComputeMapCamera(ref this._cameraTarget, this.CameraBearing, this._cameraElevation, this.CameraDistance, ref this._lastUsedIdealCameraTarget);
						this.TargetCameraDistance = MathF.Max(this.TargetCameraDistance, this.CameraDistance);
					}
				}
			}
			this.Camera.Frame = this._cameraFrame;
			this.Camera.SetFovVertical(0.6981317f, Screen.AspectRatio, 0.01f, this.MaximumCameraHeight * 4f);
			this._mapScene.SetDepthOfFieldFocus(0f);
			this._mapScene.SetDepthOfFieldParameters(0f, 0f, false);
			MatrixFrame identity = MatrixFrame.Identity;
			identity.rotation = this._cameraFrame.rotation;
			identity.origin = this._cameraTarget;
			IMapScene mapSceneWrapper3 = Campaign.Current.MapSceneWrapper;
			campaignVec = new CampaignVec2(identity.origin.AsVec2, true);
			mapSceneWrapper3.GetHeightAtPoint(in campaignVec, ref identity.origin.z);
			identity.origin = MBMath.Lerp(identity.origin, this._cameraFrame.origin, 0.075f, 1E-05f);
			campaignVec = new CampaignVec2(identity.origin.AsVec2, true);
			PathFaceRecord pathFaceRecord = campaignVec.Face;
			if (!pathFaceRecord.IsValid())
			{
				campaignVec = new CampaignVec2(identity.origin.AsVec2, false);
				pathFaceRecord = campaignVec.Face;
			}
			if (pathFaceRecord.IsValid())
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(pathFaceRecord);
				MBMapScene.TickAmbientSounds(this._mapScene, (int)faceTerrainType);
			}
			SoundManager.SetListenerFrame(identity);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000152D1 File Offset: 0x000134D1
		protected virtual Vec3 GetCameraTargetForPosition(CampaignVec2 targetPosition)
		{
			return targetPosition.AsVec3() + Vec3.Up;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000152E4 File Offset: 0x000134E4
		protected virtual Vec3 GetCameraTargetForParty(PartyBase party)
		{
			CampaignVec2 campaignVec = CampaignVec2.Zero;
			if (party.IsMobile && party.MobileParty.CurrentSettlement != null)
			{
				campaignVec = party.MobileParty.CurrentSettlement.Position;
			}
			else if (party.IsMobile && party.MobileParty.BesiegedSettlement != null)
			{
				if (PlayerSiege.PlayerSiegeEvent != null)
				{
					Vec2 asVec = party.MobileParty.BesiegedSettlement.Town.BesiegerCampPositions1.First<MatrixFrame>().origin.AsVec2;
					Vec2 vec = Vec2.Lerp(party.MobileParty.TargetPosition.ToVec2(), asVec, 0.75f);
					campaignVec = new CampaignVec2(vec, campaignVec.IsOnLand);
				}
				else
				{
					campaignVec = party.MobileParty.TargetPosition;
				}
			}
			else
			{
				campaignVec = party.Position;
			}
			return this.GetCameraTargetForPosition(campaignVec);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000153B4 File Offset: 0x000135B4
		protected virtual bool GetMapCameraInput(MapCameraView.InputInformation inputInformation)
		{
			bool flag = false;
			bool flag2 = !inputInformation.LeftButtonDraggingMode;
			if (inputInformation.IsControlDown && inputInformation.CheatModeEnabled)
			{
				flag = true;
				if (inputInformation.DeltaMouseScroll > 0.01f)
				{
					this.CameraFastMoveMultiplier *= 1.25f;
				}
				else if (inputInformation.DeltaMouseScroll < -0.01f)
				{
					this.CameraFastMoveMultiplier *= 0.8f;
				}
				this.CameraFastMoveMultiplier = MBMath.ClampFloat(this.CameraFastMoveMultiplier, 1f, 37.252903f);
			}
			Vec2 vec = Vec2.Zero;
			if (!inputInformation.LeftMouseButtonPressed && inputInformation.LeftMouseButtonDown && !inputInformation.LeftMouseButtonReleased && inputInformation.MousePositionPixel.DistanceSquared(inputInformation.ClickedPositionPixel) > 300f && !inputInformation.IsInMenu)
			{
				Vec3 vec2;
				if (!inputInformation.LeftButtonDraggingMode)
				{
					this.IdealCameraTarget = this._cameraTarget;
					vec2 = this.IdealCameraTarget;
					this._lastUsedIdealCameraTarget = new CampaignVec2(vec2.AsVec2, true);
				}
				vec2 = inputInformation.WorldMouseFar - inputInformation.WorldMouseNear;
				Vec3 vec3 = vec2.NormalizedCopy();
				Vec3 vec4 = -Vec3.Up;
				float num;
				if (MBMath.GetRayPlaneIntersectionPoint(in vec4, in inputInformation.ClickedPosition, in inputInformation.WorldMouseNear, in vec3, out num))
				{
					this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.Free;
					Vec3 vec5 = inputInformation.WorldMouseNear + vec3 * num;
					vec = inputInformation.ClickedPosition.AsVec2 - vec5.AsVec2;
				}
			}
			if (inputInformation.MiddleMouseButtonDown)
			{
				this.TargetCameraDistance += 0.01f * (this.CameraDistance + 20f) * inputInformation.MouseSensitivity * inputInformation.MouseMoveY;
			}
			if (inputInformation.RotateLeftKeyDown)
			{
				this.CameraBearingVelocity = inputInformation.Dt * 2f;
			}
			else if (inputInformation.RotateRightKeyDown)
			{
				this.CameraBearingVelocity = inputInformation.Dt * -2f;
			}
			this.CameraBearingVelocity += inputInformation.HorizontalCameraInput * 1.75f * inputInformation.Dt;
			if (inputInformation.RightMouseButtonDown)
			{
				this.CameraBearingVelocity += 0.01f * inputInformation.MouseSensitivity * inputInformation.MouseMoveX;
			}
			float num2 = 0.1f;
			if (!inputInformation.IsMouseActive)
			{
				num2 *= inputInformation.Dt * 10f;
			}
			if (!flag)
			{
				this.TargetCameraDistance -= inputInformation.MapZoomIn * num2 * (this.CameraDistance + 20f);
				this.TargetCameraDistance += inputInformation.MapZoomOut * num2 * (this.CameraDistance + 20f);
			}
			PartyBase cameraFollowParty = Campaign.Current.CameraFollowParty;
			this.TargetCameraDistance = MBMath.ClampFloat(this.TargetCameraDistance, 2.5f, (cameraFollowParty != null && cameraFollowParty.IsMobile && (cameraFollowParty.MobileParty.BesiegedSettlement != null || (cameraFollowParty.MobileParty.CurrentSettlement != null && cameraFollowParty.MobileParty.CurrentSettlement.IsUnderSiege))) ? 30f : this.MaximumCameraHeight);
			float num3 = this.TargetCameraDistance - this.CameraDistance;
			float num4 = MathF.Abs(num3);
			float num5 = ((num4 > 0.001f) ? (this.CameraDistance + num3 * inputInformation.Dt * 8f) : this.TargetCameraDistance);
			if (this.CurrentCameraFollowMode == MapCameraView.CameraFollowMode.Free && !inputInformation.RightMouseButtonDown && !inputInformation.LeftMouseButtonDown && num4 >= 0.001f)
			{
				Vec3 vec2 = inputInformation.WorldMouseFar - this.CameraFrame.origin;
				if (vec2.NormalizedCopy().z < -0.2f && inputInformation.RayCastForClosestEntityOrTerrainCondition)
				{
					MatrixFrame matrixFrame = this.ComputeMapCamera(ref this._cameraTarget, this.CameraBearing + this.CameraBearingVelocity, MathF.Min(this.CalculateCameraElevation(num5) + this.AdditionalElevation, 1.5550884f), num5, ref this._lastUsedIdealCameraTarget);
					Vec3 vec6 = -Vec3.Up;
					vec2 = inputInformation.WorldMouseFar - this.CameraFrame.origin;
					Vec3 vec7 = vec2.NormalizedCopy();
					vec2 = this.CameraFrame.rotation.TransformToLocal(in vec7);
					Vec3 vec8 = matrixFrame.rotation.TransformToParent(in vec2);
					float num6;
					if (MBMath.GetRayPlaneIntersectionPoint(in vec6, in inputInformation.ProjectedPosition, in matrixFrame.origin, in vec8, out num6))
					{
						Vec2 asVec = inputInformation.ProjectedPosition.AsVec2;
						vec2 = matrixFrame.origin + vec8 * num6;
						vec = asVec - vec2.AsVec2;
						flag2 = false;
					}
				}
			}
			if (inputInformation.RX != 0f || inputInformation.RY != 0f || vec.IsNonZero())
			{
				float num7 = 0.001f * (this.CameraDistance * 0.55f + 15f);
				Vec2 vec9 = Vec2.FromRotation(-this.CameraBearing);
				Vec3 vec2 = this.IdealCameraTarget;
				if ((vec2.AsVec2 - this._lastUsedIdealCameraTarget.ToVec2()).LengthSquared > 0.010000001f)
				{
					this.IdealCameraTarget = this._lastUsedIdealCameraTarget.AsVec3();
					this._cameraTarget = this.IdealCameraTarget;
				}
				if (!vec.IsNonZero())
				{
					this.IdealCameraTarget = this._cameraTarget;
				}
				Vec2 vec10 = inputInformation.Dt * 500f * inputInformation.RX * vec9.RightVec() * num7 + inputInformation.Dt * 500f * inputInformation.RY * vec9 * num7;
				this.IdealCameraTarget = new Vec3(this.IdealCameraTarget.x + vec.x + vec10.x, this.IdealCameraTarget.y + vec.y + vec10.y, this.IdealCameraTarget.z, -1f);
				if (vec.IsNonZero())
				{
					this._cameraTarget = this.IdealCameraTarget;
				}
				this._cameraTarget.AsVec2 = this._cameraTarget.AsVec2 + vec10;
				if (inputInformation.RX != 0f || inputInformation.RY != 0f)
				{
					this.CurrentCameraFollowMode = MapCameraView.CameraFollowMode.Free;
				}
			}
			this.CameraBearing += this.CameraBearingVelocity;
			this.CameraBearingVelocity = 0f;
			this.CameraDistance = num5;
			this._cameraElevation = MathF.Min(this.CalculateCameraElevation(num5) + this.AdditionalElevation, 1.5550884f);
			if (this.CurrentCameraFollowMode == MapCameraView.CameraFollowMode.FollowParty && cameraFollowParty != null && cameraFollowParty.IsValid)
			{
				CampaignVec2 campaignVec;
				Vec2 vec11;
				bool flag3;
				if (cameraFollowParty.IsMobile)
				{
					Settlement settlement;
					if ((settlement = cameraFollowParty.MobileParty.CurrentSettlement) == null && (settlement = cameraFollowParty.MobileParty.BesiegedSettlement) == null)
					{
						MapEvent mapEvent = cameraFollowParty.MapEvent;
						settlement = ((mapEvent != null) ? mapEvent.MapEventSettlement : null);
					}
					Settlement settlement2 = settlement;
					if (settlement2 != null && cameraFollowParty.MobileParty.IsMainParty)
					{
						campaignVec = settlement2.Position;
						vec11 = campaignVec.ToVec2();
						if (settlement2.HasPort)
						{
							CampaignVec2 portPosition = settlement2.PortPosition;
							vec11 += portPosition.ToVec2();
							if (settlement2.IsUnderSiege)
							{
								Vec2 vec12 = vec11;
								campaignVec = settlement2.SiegeEvent.BesiegerCamp.LeaderParty.Position;
								vec11 = vec12 + campaignVec.ToVec2();
								vec11 /= 3f;
							}
							else
							{
								vec11 *= 0.5f;
							}
						}
					}
					else
					{
						Vec2 vec13;
						if (cameraFollowParty.MapEvent == null)
						{
							campaignVec = cameraFollowParty.Position;
							vec13 = campaignVec.ToVec2();
						}
						else
						{
							campaignVec = cameraFollowParty.MapEvent.Position;
							vec13 = campaignVec.ToVec2();
						}
						vec11 = vec13;
					}
					flag3 = !cameraFollowParty.MobileParty.IsCurrentlyAtSea;
				}
				else
				{
					campaignVec = cameraFollowParty.Position;
					vec11 = campaignVec.ToVec2();
					flag3 = true;
				}
				float num8 = 0f;
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				campaignVec = new CampaignVec2(vec11, flag3);
				mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num8);
				this.IdealCameraTarget = new Vec3(vec11.X, vec11.Y, num8 + 1f, -1f);
			}
			return flag2;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00015BC0 File Offset: 0x00013DC0
		protected virtual MatrixFrame ComputeMapCamera(ref Vec3 cameraTarget, float cameraBearing, float cameraElevation, float cameraDistance, ref CampaignVec2 lastUsedIdealCameraTarget)
		{
			Vec2 asVec = cameraTarget.AsVec2;
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = cameraTarget;
			identity.rotation.RotateAboutSide(1.5707964f);
			identity.rotation.RotateAboutForward(-cameraBearing);
			identity.rotation.RotateAboutSide(-cameraElevation);
			identity.origin += identity.rotation.u * (cameraDistance + 2f);
			Vec2 vec = (Campaign.MapMinimumPosition + Campaign.MapMaximumPosition) * 0.5f;
			float num = Campaign.MapMaximumPosition.y - vec.y;
			float num2 = Campaign.MapMaximumPosition.x - vec.x;
			asVec.x = MBMath.ClampFloat(asVec.x, vec.x - num2, vec.x + num2);
			asVec.y = MBMath.ClampFloat(asVec.y, vec.y - num, vec.y + num);
			float num3 = MBMath.ClampFloat(lastUsedIdealCameraTarget.X, vec.x - num2, vec.x + num2);
			float num4 = MBMath.ClampFloat(lastUsedIdealCameraTarget.Y, vec.y - num, vec.y + num);
			lastUsedIdealCameraTarget = new CampaignVec2(new Vec2(num3, num4), lastUsedIdealCameraTarget.IsOnLand);
			identity.origin.x = identity.origin.x + (asVec.x - cameraTarget.x);
			identity.origin.y = identity.origin.y + (asVec.y - cameraTarget.y);
			return identity;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00015D5D File Offset: 0x00013F5D
		protected virtual float CalculateCameraElevation(float cameraDistance)
		{
			return cameraDistance * 0.0075f + 0.35f;
		}

		// Token: 0x0400011F RID: 287
		private const float VerticalHalfViewAngle = 0.34906584f;

		// Token: 0x04000120 RID: 288
		private Vec3 _cameraTarget;

		// Token: 0x04000121 RID: 289
		private float _distanceToIdealCameraTargetToStopCameraSoundEventsSquared;

		// Token: 0x04000122 RID: 290
		private int _cameraMoveSfxSoundEventId;

		// Token: 0x04000123 RID: 291
		private SoundEvent _cameraMoveSfxSoundEvent;

		// Token: 0x04000124 RID: 292
		private bool _doFastCameraMovementToTarget;

		// Token: 0x04000125 RID: 293
		private float _cameraElevation;

		// Token: 0x04000126 RID: 294
		private CampaignVec2 _lastUsedIdealCameraTarget;

		// Token: 0x04000127 RID: 295
		private CampaignVec2 _cameraAnimationTarget;

		// Token: 0x04000128 RID: 296
		private float _cameraAnimationStopDuration;

		// Token: 0x04000129 RID: 297
		private readonly Scene _mapScene;

		// Token: 0x0400012D RID: 301
		protected float _customMaximumCameraHeight;

		// Token: 0x04000135 RID: 309
		private MatrixFrame _cameraFrame;

		// Token: 0x020000A8 RID: 168
		public enum CameraFollowMode
		{
			// Token: 0x0400033B RID: 827
			Free,
			// Token: 0x0400033C RID: 828
			FollowParty,
			// Token: 0x0400033D RID: 829
			MoveToPosition
		}

		// Token: 0x020000A9 RID: 169
		public struct InputInformation
		{
			// Token: 0x0400033E RID: 830
			public bool IsMainPartyValid;

			// Token: 0x0400033F RID: 831
			public bool IsMapReady;

			// Token: 0x04000340 RID: 832
			public bool IsControlDown;

			// Token: 0x04000341 RID: 833
			public bool IsMouseActive;

			// Token: 0x04000342 RID: 834
			public bool CheatModeEnabled;

			// Token: 0x04000343 RID: 835
			public bool LeftMouseButtonPressed;

			// Token: 0x04000344 RID: 836
			public bool LeftMouseButtonDown;

			// Token: 0x04000345 RID: 837
			public bool LeftMouseButtonReleased;

			// Token: 0x04000346 RID: 838
			public bool MiddleMouseButtonDown;

			// Token: 0x04000347 RID: 839
			public bool RightMouseButtonDown;

			// Token: 0x04000348 RID: 840
			public bool RotateLeftKeyDown;

			// Token: 0x04000349 RID: 841
			public bool RotateRightKeyDown;

			// Token: 0x0400034A RID: 842
			public bool PartyMoveUpKey;

			// Token: 0x0400034B RID: 843
			public bool PartyMoveDownKey;

			// Token: 0x0400034C RID: 844
			public bool PartyMoveLeftKey;

			// Token: 0x0400034D RID: 845
			public bool PartyMoveRightKey;

			// Token: 0x0400034E RID: 846
			public bool CameraFollowModeKeyPressed;

			// Token: 0x0400034F RID: 847
			public bool LeftButtonDraggingMode;

			// Token: 0x04000350 RID: 848
			public bool IsInMenu;

			// Token: 0x04000351 RID: 849
			public bool RayCastForClosestEntityOrTerrainCondition;

			// Token: 0x04000352 RID: 850
			public float MapZoomIn;

			// Token: 0x04000353 RID: 851
			public float MapZoomOut;

			// Token: 0x04000354 RID: 852
			public float DeltaMouseScroll;

			// Token: 0x04000355 RID: 853
			public float MouseSensitivity;

			// Token: 0x04000356 RID: 854
			public float MouseMoveX;

			// Token: 0x04000357 RID: 855
			public float MouseMoveY;

			// Token: 0x04000358 RID: 856
			public float HorizontalCameraInput;

			// Token: 0x04000359 RID: 857
			public float RX;

			// Token: 0x0400035A RID: 858
			public float RY;

			// Token: 0x0400035B RID: 859
			public float RS;

			// Token: 0x0400035C RID: 860
			public float Dt;

			// Token: 0x0400035D RID: 861
			public Vec2 MousePositionPixel;

			// Token: 0x0400035E RID: 862
			public Vec2 ClickedPositionPixel;

			// Token: 0x0400035F RID: 863
			public Vec3 ClickedPosition;

			// Token: 0x04000360 RID: 864
			public Vec3 ProjectedPosition;

			// Token: 0x04000361 RID: 865
			public Vec3 WorldMouseNear;

			// Token: 0x04000362 RID: 866
			public Vec3 WorldMouseFar;
		}
	}
}
