using System;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;

namespace TaleWorlds.MountAndBlade.View.MissionViews.SiegeWeapon
{
	// Token: 0x020000A4 RID: 164
	public class RangedSiegeWeaponView : UsableMissionObjectComponent
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000289CC File Offset: 0x00026BCC
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x000289D4 File Offset: 0x00026BD4
		public RangedSiegeWeapon RangedSiegeWeapon { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x000289DD File Offset: 0x00026BDD
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x000289E5 File Offset: 0x00026BE5
		public MissionScreen MissionScreen { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x000289EE File Offset: 0x00026BEE
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x000289F6 File Offset: 0x00026BF6
		public Camera Camera { get; private set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x000289FF File Offset: 0x00026BFF
		public GameEntity CameraHolder
		{
			get
			{
				return this.RangedSiegeWeapon.CameraHolder;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00028A0C File Offset: 0x00026C0C
		public Agent PilotAgent
		{
			get
			{
				return this.RangedSiegeWeapon.PilotAgent;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00028A19 File Offset: 0x00026C19
		public void Initialize(RangedSiegeWeapon rangedSiegeWeapon, MissionScreen missionScreen)
		{
			this.RangedSiegeWeapon = rangedSiegeWeapon;
			this.MissionScreen = missionScreen;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00028A29 File Offset: 0x00026C29
		protected override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			if (this.CameraHolder != null)
			{
				this.CreateCamera();
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00028A48 File Offset: 0x00026C48
		protected override void OnMissionReset()
		{
			base.OnMissionReset();
			if (this.CameraHolder != null)
			{
				this._cameraYaw = this._cameraInitialYaw;
				this._cameraPitch = this._cameraInitialPitch;
				this.ApplyCameraRotation();
				this._isInWeaponCameraMode = false;
				this.ResetCamera();
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00028A94 File Offset: 0x00026C94
		public override bool IsOnTickRequired()
		{
			return true;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00028A97 File Offset: 0x00026C97
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!GameNetwork.IsReplay)
			{
				this.HandleUserInput(dt);
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00028AB0 File Offset: 0x00026CB0
		protected virtual void HandleUserInput(float dt)
		{
			if (this.CameraHolder != null && ((this.PilotAgent != null && this.PilotAgent.IsMainAgent) || this.RangedSiegeWeapon.PlayerForceUse))
			{
				if (!this._isInWeaponCameraMode)
				{
					this._isInWeaponCameraMode = true;
					this.StartUsingWeaponCamera();
				}
				if (this.RangedSiegeWeapon.PlayerForceUse)
				{
					this.HandleUserCameraRotation(dt);
				}
			}
			if (this._isInWeaponCameraMode && (this.PilotAgent == null || !this.PilotAgent.IsMainAgent) && !this.RangedSiegeWeapon.PlayerForceUse)
			{
				this._isInWeaponCameraMode = false;
				this.ResetCamera();
			}
			this.HandleUserAiming(dt);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00028B54 File Offset: 0x00026D54
		private void CreateCamera()
		{
			this.Camera = Camera.CreateCamera();
			float aspectRatio = Screen.AspectRatio;
			this.Camera.SetFovVertical(1.0471976f, aspectRatio, 0.1f, 12500f);
			this.Camera.Entity = this.CameraHolder;
			MatrixFrame frame = this.CameraHolder.GetFrame();
			Vec3 eulerAngles = frame.rotation.GetEulerAngles();
			this._cameraYaw = eulerAngles.z;
			this._cameraPitch = eulerAngles.x;
			this._cameraRoll = eulerAngles.y;
			this._cameraPositionOffset = frame.origin;
			this._cameraPositionOffset.RotateAboutZ(-this._cameraYaw);
			this._cameraPositionOffset.RotateAboutX(-this._cameraPitch);
			this._cameraPositionOffset.RotateAboutY(-this._cameraRoll);
			this._cameraInitialYaw = this._cameraYaw;
			this._cameraInitialPitch = this._cameraPitch;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00028C35 File Offset: 0x00026E35
		protected virtual void StartUsingWeaponCamera()
		{
			this.MissionScreen.CustomCamera = this.Camera;
			Agent.Main.IsLookDirectionLocked = true;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00028C54 File Offset: 0x00026E54
		private void ResetCamera()
		{
			if (this.MissionScreen.CustomCamera == this.Camera)
			{
				this.MissionScreen.CustomCamera = null;
				if (Agent.Main != null)
				{
					Agent.Main.IsLookDirectionLocked = false;
					this.MissionScreen.SetExtraCameraParameters(false, 0f);
				}
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00028CA8 File Offset: 0x00026EA8
		protected virtual void HandleUserCameraRotation(float dt)
		{
			float cameraYaw = this._cameraYaw;
			float cameraPitch = this._cameraPitch;
			if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(10))
			{
				this._cameraYaw = this._cameraInitialYaw;
				this._cameraPitch = this._cameraInitialPitch;
			}
			this._cameraYaw += this.MissionScreen.SceneLayer.Input.GetMouseMoveX() * dt * 0.2f;
			this._cameraPitch += this.MissionScreen.SceneLayer.Input.GetMouseMoveY() * dt * 0.2f;
			this._cameraYaw = MBMath.ClampFloat(this._cameraYaw, 1.5707964f, 4.712389f);
			this._cameraPitch = MBMath.ClampFloat(this._cameraPitch, 1.0471976f, 1.7453294f);
			if (cameraPitch != this._cameraPitch || cameraYaw != this._cameraYaw)
			{
				this.ApplyCameraRotation();
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00028D94 File Offset: 0x00026F94
		private void ApplyCameraRotation()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.rotation.RotateAboutUp(this._cameraYaw);
			identity.rotation.RotateAboutSide(this._cameraPitch);
			identity.rotation.RotateAboutForward(this._cameraRoll);
			identity.Strafe(this._cameraPositionOffset.x);
			identity.Advance(this._cameraPositionOffset.y);
			identity.Elevate(this._cameraPositionOffset.z);
			this.CameraHolder.SetFrame(ref identity, true);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00028E24 File Offset: 0x00027024
		private void HandleUserAiming(float dt)
		{
			bool flag = false;
			float num = 0f;
			float num2 = 0f;
			if (this.PilotAgent != null && (this.PilotAgent.IsMainAgent || this.RangedSiegeWeapon.PlayerForceUse))
			{
				if (this.UsesMouseForAiming)
				{
					InputContext input = this.MissionScreen.SceneLayer.Input;
					float num3 = dt * 1666.6666f;
					float num4 = input.GetMouseMoveX() + num3 * input.GetGameKeyAxis("CameraAxisX");
					float num5 = input.GetMouseMoveY() + -num3 * input.GetGameKeyAxis("CameraAxisY");
					if (NativeConfig.InvertMouse)
					{
						num5 *= -1f;
					}
					Vec2 vec = new Vec2(-num4, -num5);
					if (vec.IsNonZero())
					{
						float num6 = vec.Normalize();
						num6 = MathF.Min(5f, MathF.Pow(num6, 1.5f) * 0.025f);
						vec *= num6;
						num = vec.x;
						num2 = vec.y;
					}
				}
				else
				{
					if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(2))
					{
						num = 1f;
					}
					else if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(3))
					{
						num = -1f;
					}
					if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(0))
					{
						num2 = 1f;
					}
					else if (this.MissionScreen.SceneLayer.Input.IsGameKeyDown(1))
					{
						num2 = -1f;
					}
				}
				if (num != 0f)
				{
					flag = true;
				}
				if (num2 != 0f)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.RangedSiegeWeapon.GiveInput(num, num2);
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00028FC9 File Offset: 0x000271C9
		protected override void OnMissionObjectDisabled()
		{
			this.ResetCamera();
		}

		// Token: 0x04000312 RID: 786
		private float _cameraYaw;

		// Token: 0x04000313 RID: 787
		private float _cameraPitch;

		// Token: 0x04000314 RID: 788
		private float _cameraRoll;

		// Token: 0x04000315 RID: 789
		private float _cameraInitialYaw;

		// Token: 0x04000316 RID: 790
		private float _cameraInitialPitch;

		// Token: 0x04000317 RID: 791
		private Vec3 _cameraPositionOffset;

		// Token: 0x04000318 RID: 792
		private bool _isInWeaponCameraMode;

		// Token: 0x04000319 RID: 793
		protected bool UsesMouseForAiming;
	}
}
