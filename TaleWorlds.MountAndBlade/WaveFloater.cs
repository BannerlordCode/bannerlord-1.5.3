using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038B RID: 907
	public class WaveFloater : ScriptComponentBehavior
	{
		// Token: 0x06003477 RID: 13431 RVA: 0x000D83C5 File Offset: 0x000D65C5
		private float ConvertToRadians(float angle)
		{
			return 0.017453292f * angle;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000D83D0 File Offset: 0x000D65D0
		private void SetMatrix()
		{
			this.resetMF = base.GameEntity.GetGlobalFrame();
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000D83F4 File Offset: 0x000D65F4
		private void ResetMatrix()
		{
			base.GameEntity.SetGlobalFrame(in this.resetMF, true);
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000D8418 File Offset: 0x000D6618
		private void CalculateAxis()
		{
			this.axis = new Vec3(Convert.ToSingle(this.oscillateAtX), Convert.ToSingle(this.oscillateAtY), Convert.ToSingle(this.oscillateAtZ), -1f);
			base.GameEntity.GetGlobalFrame().TransformToParent(in this.axis);
			this.axis.Normalize();
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000D847F File Offset: 0x000D667F
		private float CalculateSpeed(float fq, float maxVal, bool angular)
		{
			if (!angular)
			{
				return maxVal * 2f * fq * 1f;
			}
			return maxVal * 3.1415927f / 90f * fq * 1f;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000D84AC File Offset: 0x000D66AC
		private void CalculateOscilations()
		{
			this.ResetMatrix();
			this.oscillationStart = base.GameEntity.GetGlobalFrame();
			this.oscillationEnd = base.GameEntity.GetGlobalFrame();
			this.oscillationStart.rotation.RotateAboutAnArbitraryVector(in this.axis, -this.ConvertToRadians(this.maxOscillationAngle));
			this.oscillationEnd.rotation.RotateAboutAnArbitraryVector(in this.axis, this.ConvertToRadians(this.maxOscillationAngle));
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000D852C File Offset: 0x000D672C
		private void Oscillate()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			globalFrame.rotation = Mat3.Lerp(in this.oscillationStart.rotation, in this.oscillationEnd.rotation, MathF.Clamp(this.oscillationPercentage, 0f, 1f));
			base.GameEntity.SetGlobalFrame(in globalFrame, true);
			this.oscillationPercentage = (1f + MathF.Cos(this.oscillationSpeed * 1f * this.et)) / 2f;
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000D85BC File Offset: 0x000D67BC
		private void CalculateBounces()
		{
			this.ResetMatrix();
			this.bounceXStart = base.GameEntity.GetGlobalFrame();
			this.bounceXEnd = base.GameEntity.GetGlobalFrame();
			this.bounceYStart = base.GameEntity.GetGlobalFrame();
			this.bounceYEnd = base.GameEntity.GetGlobalFrame();
			this.bounceZStart = base.GameEntity.GetGlobalFrame();
			this.bounceZEnd = base.GameEntity.GetGlobalFrame();
			this.bounceXStart.origin.x = this.bounceXStart.origin.x + this.maxBounceXDistance;
			this.bounceXEnd.origin.x = this.bounceXEnd.origin.x - this.maxBounceXDistance;
			this.bounceYStart.origin.y = this.bounceYStart.origin.y + this.maxBounceYDistance;
			this.bounceYEnd.origin.y = this.bounceYEnd.origin.y - this.maxBounceYDistance;
			this.bounceZStart.origin.z = this.bounceZStart.origin.z + this.maxBounceZDistance;
			this.bounceZEnd.origin.z = this.bounceZEnd.origin.z - this.maxBounceZDistance;
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000D86E4 File Offset: 0x000D68E4
		private void Bounce()
		{
			if (this.bounceX)
			{
				MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
				matrixFrame.origin.x = Vec3.Lerp(this.bounceXStart.origin, this.bounceXEnd.origin, MathF.Clamp(this.bounceXPercentage, 0f, 1f)).x;
				base.GameEntity.SetGlobalFrame(in matrixFrame, true);
				this.bounceXPercentage = (1f + MathF.Sin(this.bounceXSpeed * 1f * this.et)) / 2f;
			}
			if (this.bounceY)
			{
				MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
				matrixFrame.origin.y = Vec3.Lerp(this.bounceYStart.origin, this.bounceYEnd.origin, MathF.Clamp(this.bounceYPercentage, 0f, 1f)).y;
				base.GameEntity.SetGlobalFrame(in matrixFrame, true);
				this.bounceYPercentage = (1f + MathF.Cos(this.bounceYSpeed * 1f * this.et)) / 2f;
			}
			if (this.bounceZ)
			{
				MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
				matrixFrame.origin.z = Vec3.Lerp(this.bounceZStart.origin, this.bounceZEnd.origin, MathF.Clamp(this.bounceZPercentage, 0f, 1f)).z;
				base.GameEntity.SetGlobalFrame(in matrixFrame, true);
				this.bounceZPercentage = (1f + MathF.Cos(this.bounceZSpeed * 1f * this.et)) / 2f;
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000D88B4 File Offset: 0x000D6AB4
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetMatrix();
			this.oscillate = this.oscillateAtX || this.oscillateAtY || this.oscillateAtZ;
			this.bounce = this.bounceX || this.bounceY || this.bounceZ;
			this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
			this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
			this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
			this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
			this.CalculateBounces();
			this.CalculateAxis();
			this.CalculateOscilations();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000D8990 File Offset: 0x000D6B90
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.SetMatrix();
			this.oscillate = this.oscillateAtX || this.oscillateAtY || this.oscillateAtZ;
			this.bounce = this.bounceX || this.bounceY || this.bounceZ;
			this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
			this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
			this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
			this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
			this.CalculateBounces();
			this.CalculateAxis();
			this.CalculateOscilations();
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x000D8A5D File Offset: 0x000D6C5D
		protected internal override void OnSceneSave(string saveFolder)
		{
			base.OnSceneSave(saveFolder);
			this.ResetMatrix();
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x000D8A6C File Offset: 0x000D6C6C
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.et += dt;
			if (this.oscillate)
			{
				this.Oscillate();
			}
			if (this.bounce)
			{
				this.Bounce();
			}
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000D8A9F File Offset: 0x000D6C9F
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000D8AA9 File Offset: 0x000D6CA9
		protected internal override void OnTick(float dt)
		{
			this.et += dt;
			if (this.oscillate)
			{
				this.Oscillate();
			}
			if (this.bounce)
			{
				this.Bounce();
			}
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000D8AD8 File Offset: 0x000D6CD8
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "largeObject")
			{
				this.ResetMatrix();
				this.oscillateAtX = true;
				this.oscillateAtY = true;
				this.oscillationFrequency = 1.5f;
				this.maxOscillationAngle = 7.4f;
				this.bounceX = true;
				this.bounceXFrequency = 2f;
				this.maxBounceXDistance = 0.1f;
				this.bounceY = true;
				this.bounceYFrequency = 0.2f;
				this.maxBounceYDistance = 0.5f;
				this.bounceZ = true;
				this.bounceZFrequency = 0.6f;
				this.maxBounceZDistance = 0.22f;
				this.CalculateAxis();
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				this.CalculateOscilations();
				this.CalculateOscilations();
				this.oscillate = true;
				this.bounce = true;
				return;
			}
			if (variableName == "smallObject")
			{
				this.ResetMatrix();
				this.oscillateAtX = true;
				this.oscillateAtY = true;
				this.oscillateAtZ = true;
				this.oscillationFrequency = 1f;
				this.maxOscillationAngle = 11f;
				this.bounceX = true;
				this.bounceXFrequency = 1.5f;
				this.maxBounceXDistance = 0.3f;
				this.bounceY = true;
				this.bounceYFrequency = 1.5f;
				this.maxBounceYDistance = 0.2f;
				this.bounceZ = true;
				this.bounceZFrequency = 1f;
				this.maxBounceZDistance = 0.1f;
				this.CalculateAxis();
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				this.CalculateOscilations();
				this.CalculateOscilations();
				this.oscillate = true;
				this.bounce = true;
				return;
			}
			if (variableName == "oscillateAtX" || variableName == "oscillateAtY" || variableName == "oscillateAtZ")
			{
				if (this.oscillateAtX || this.oscillateAtY || this.oscillateAtZ)
				{
					if (!this.oscillate)
					{
						if (!this.bounce)
						{
							this.SetMatrix();
						}
						this.oscillate = true;
					}
				}
				else
				{
					this.oscillate = false;
					if (!this.bounce)
					{
						this.ResetMatrix();
					}
				}
				this.CalculateAxis();
				this.CalculateOscilations();
				return;
			}
			if (variableName == "oscillationFrequency")
			{
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				return;
			}
			if (variableName == "maxOscillationAngle")
			{
				this.maxOscillationAngle = MathF.Clamp(this.maxOscillationAngle, 0f, 90f);
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				this.CalculateOscilations();
				return;
			}
			if (variableName == "bounceX" || variableName == "bounceY" || variableName == "bounceZ")
			{
				if (this.bounceX || this.bounceY || this.bounceZ)
				{
					if (!this.bounce)
					{
						if (!this.oscillate)
						{
							this.SetMatrix();
						}
						this.bounce = true;
					}
				}
				else
				{
					this.bounce = false;
					if (!this.oscillate)
					{
						this.ResetMatrix();
					}
				}
				this.CalculateBounces();
				return;
			}
			if (variableName == "bounceXFrequency")
			{
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				return;
			}
			if (variableName == "bounceYFrequency")
			{
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				return;
			}
			if (variableName == "bounceZFrequency")
			{
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				return;
			}
			if (variableName == "maxBounceXDistance")
			{
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				this.CalculateBounces();
				return;
			}
			if (variableName == "maxBounceYDistance")
			{
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				this.CalculateBounces();
				return;
			}
			if (variableName == "maxBounceZDistance")
			{
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				this.CalculateBounces();
			}
		}

		// Token: 0x0400162B RID: 5675
		public SimpleButton largeObject;

		// Token: 0x0400162C RID: 5676
		public SimpleButton smallObject;

		// Token: 0x0400162D RID: 5677
		public bool oscillateAtX;

		// Token: 0x0400162E RID: 5678
		public bool oscillateAtY;

		// Token: 0x0400162F RID: 5679
		public bool oscillateAtZ;

		// Token: 0x04001630 RID: 5680
		public float oscillationFrequency = 1f;

		// Token: 0x04001631 RID: 5681
		public float maxOscillationAngle = 10f;

		// Token: 0x04001632 RID: 5682
		public bool bounceX;

		// Token: 0x04001633 RID: 5683
		public float bounceXFrequency = 14f;

		// Token: 0x04001634 RID: 5684
		public float maxBounceXDistance = 0.3f;

		// Token: 0x04001635 RID: 5685
		public bool bounceY;

		// Token: 0x04001636 RID: 5686
		public float bounceYFrequency = 14f;

		// Token: 0x04001637 RID: 5687
		public float maxBounceYDistance = 0.3f;

		// Token: 0x04001638 RID: 5688
		public bool bounceZ;

		// Token: 0x04001639 RID: 5689
		public float bounceZFrequency = 14f;

		// Token: 0x0400163A RID: 5690
		public float maxBounceZDistance = 0.3f;

		// Token: 0x0400163B RID: 5691
		private Vec3 axis;

		// Token: 0x0400163C RID: 5692
		private float oscillationSpeed = 1f;

		// Token: 0x0400163D RID: 5693
		private float oscillationPercentage = 0.5f;

		// Token: 0x0400163E RID: 5694
		private MatrixFrame resetMF;

		// Token: 0x0400163F RID: 5695
		private MatrixFrame oscillationStart;

		// Token: 0x04001640 RID: 5696
		private MatrixFrame oscillationEnd;

		// Token: 0x04001641 RID: 5697
		private bool oscillate;

		// Token: 0x04001642 RID: 5698
		private float bounceXSpeed = 1f;

		// Token: 0x04001643 RID: 5699
		private float bounceXPercentage = 0.5f;

		// Token: 0x04001644 RID: 5700
		private MatrixFrame bounceXStart;

		// Token: 0x04001645 RID: 5701
		private MatrixFrame bounceXEnd;

		// Token: 0x04001646 RID: 5702
		private float bounceYSpeed = 1f;

		// Token: 0x04001647 RID: 5703
		private float bounceYPercentage = 0.5f;

		// Token: 0x04001648 RID: 5704
		private MatrixFrame bounceYStart;

		// Token: 0x04001649 RID: 5705
		private MatrixFrame bounceYEnd;

		// Token: 0x0400164A RID: 5706
		private float bounceZSpeed = 1f;

		// Token: 0x0400164B RID: 5707
		private float bounceZPercentage = 0.5f;

		// Token: 0x0400164C RID: 5708
		private MatrixFrame bounceZStart;

		// Token: 0x0400164D RID: 5709
		private MatrixFrame bounceZEnd;

		// Token: 0x0400164E RID: 5710
		private bool bounce;

		// Token: 0x0400164F RID: 5711
		private float et;

		// Token: 0x04001650 RID: 5712
		private const float SPEED_MODIFIER = 1f;
	}
}
