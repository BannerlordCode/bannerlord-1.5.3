using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000371 RID: 881
	public class TestScript : ScriptComponentBehavior
	{
		// Token: 0x060032C5 RID: 12997 RVA: 0x000CFC34 File Offset: 0x000CDE34
		private void Move(float dt)
		{
			if (MathF.Abs(this.MoveDistance) < 1E-05f)
			{
				return;
			}
			Vec3 vec = new Vec3(this.MoveAxisX, this.MoveAxisY, this.MoveAxisZ, -1f);
			vec.Normalize();
			Vec3 vec2 = vec * this.MoveSpeed * this.MoveDirection;
			float num = vec2.Length * this.MoveDirection;
			if (this.CurrentDistance + num <= -this.MoveDistance)
			{
				this.MoveDirection = 1f;
				num *= -1f;
				vec2 *= -1f;
			}
			else if (this.CurrentDistance + num >= this.MoveDistance)
			{
				this.MoveDirection = -1f;
				num *= -1f;
				vec2 *= -1f;
			}
			this.CurrentDistance += num;
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.origin += vec2;
			base.GameEntity.SetFrame(ref frame, true);
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x000CFD4C File Offset: 0x000CDF4C
		private void Rotate(float dt)
		{
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.rotation.RotateAboutUp(this.rotationSpeed * 0.001f * dt);
			base.GameEntity.SetFrame(ref frame, true);
			this.currentRotation += this.rotationSpeed * 0.001f * dt;
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x000CFDAE File Offset: 0x000CDFAE
		private bool isRotationPhaseInsidePhaseBoundries(float currentPhase, float startPhase, float endPhase)
		{
			if (endPhase <= startPhase)
			{
				return currentPhase > startPhase;
			}
			return currentPhase > startPhase && currentPhase < endPhase;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x000CFDC4 File Offset: 0x000CDFC4
		public static int GetIntegerFromStringEnd(string str)
		{
			string text = "";
			for (int i = str.Length - 1; i > -1; i--)
			{
				char c = str[i];
				if (c < '0' || c > '9')
				{
					break;
				}
				text = c.ToString() + text;
			}
			return Convert.ToInt32(text);
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x000CFE10 File Offset: 0x000CE010
		private void DoWaterMillCalculation()
		{
			float num = (float)base.GameEntity.ChildCount;
			if (num > 0f)
			{
				IEnumerable<WeakGameEntity> children = base.GameEntity.GetChildren();
				float num2 = 6.28f / num;
				foreach (WeakGameEntity weakGameEntity in children)
				{
					int integerFromStringEnd = TestScript.GetIntegerFromStringEnd(weakGameEntity.Name);
					float num3 = this.currentRotation % 6.28f;
					float num4 = (num2 * (float)integerFromStringEnd + this.waterSplashPhaseOffset) % 6.28f;
					float num5 = (num4 + num2 * this.waterSplashIntervalMultiplier) % 6.28f;
					if (this.isRotationPhaseInsidePhaseBoundries(num3, num4, num5))
					{
						weakGameEntity.ResumeParticleSystem(true);
					}
					else
					{
						weakGameEntity.PauseParticleSystem(true);
					}
				}
			}
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000CFEE8 File Offset: 0x000CE0E8
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x000CFEFC File Offset: 0x000CE0FC
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x000CFF06 File Offset: 0x000CE106
		protected internal override void OnTick(float dt)
		{
			this.Rotate(dt);
			this.Move(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x000CFF24 File Offset: 0x000CE124
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.Rotate(dt);
			this.Move(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
			if (this.sideRotatingEntity != null)
			{
				MatrixFrame frame = this.sideRotatingEntity.GetFrame();
				frame.rotation.RotateAboutSide(this.rotationSpeed * 0.01f * dt);
				this.sideRotatingEntity.SetFrame(ref frame, true);
			}
			if (this.forwardRotatingEntity != null)
			{
				MatrixFrame frame2 = this.forwardRotatingEntity.GetFrame();
				frame2.rotation.RotateAboutSide(this.rotationSpeed * 0.005f * dt);
				this.forwardRotatingEntity.SetFrame(ref frame2, true);
			}
		}

		// Token: 0x04001591 RID: 5521
		public string testString;

		// Token: 0x04001592 RID: 5522
		public float rotationSpeed;

		// Token: 0x04001593 RID: 5523
		public float waterSplashPhaseOffset;

		// Token: 0x04001594 RID: 5524
		public float waterSplashIntervalMultiplier = 1f;

		// Token: 0x04001595 RID: 5525
		public bool isWaterMill;

		// Token: 0x04001596 RID: 5526
		private float currentRotation;

		// Token: 0x04001597 RID: 5527
		public float MoveAxisX = 1f;

		// Token: 0x04001598 RID: 5528
		public float MoveAxisY;

		// Token: 0x04001599 RID: 5529
		public float MoveAxisZ;

		// Token: 0x0400159A RID: 5530
		public float MoveSpeed = 0.0001f;

		// Token: 0x0400159B RID: 5531
		public float MoveDistance = 10f;

		// Token: 0x0400159C RID: 5532
		protected float MoveDirection = 1f;

		// Token: 0x0400159D RID: 5533
		protected float CurrentDistance;

		// Token: 0x0400159E RID: 5534
		public GameEntity sideRotatingEntity;

		// Token: 0x0400159F RID: 5535
		public GameEntity forwardRotatingEntity;
	}
}
