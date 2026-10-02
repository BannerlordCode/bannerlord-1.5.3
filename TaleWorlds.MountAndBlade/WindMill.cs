using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038C RID: 908
	public class WindMill : ScriptComponentBehavior
	{
		// Token: 0x06003488 RID: 13448 RVA: 0x000D9047 File Offset: 0x000D7247
		protected internal override void OnInit()
		{
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000D9058 File Offset: 0x000D7258
		private void Rotate(float dt)
		{
			float num = this.rotationSpeed * 0.001f * dt;
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.rotation.RotateAboutForward(num);
			base.GameEntity.SetLocalFrame(ref frame, false);
			base.GameEntity.UpdateTriadFrameForEditorForAllChildren();
			this.currentRotation += num;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000D90BD File Offset: 0x000D72BD
		private static bool IsRotationPhaseInsidePhaseBoundaries(float currentPhase, float startPhase, float endPhase)
		{
			if (endPhase <= startPhase)
			{
				return currentPhase > startPhase;
			}
			return currentPhase > startPhase && currentPhase < endPhase;
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000D90D4 File Offset: 0x000D72D4
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

		// Token: 0x0600348C RID: 13452 RVA: 0x000D9120 File Offset: 0x000D7320
		private void DoWaterMillCalculation()
		{
			float num = (float)base.GameEntity.ChildCount;
			if (num > 0f)
			{
				IEnumerable<WeakGameEntity> children = base.GameEntity.GetChildren();
				float num2 = 6.28f / num;
				foreach (WeakGameEntity weakGameEntity in children)
				{
					int integerFromStringEnd = WindMill.GetIntegerFromStringEnd(weakGameEntity.Name);
					float num3 = this.currentRotation % 6.28f;
					float num4 = (num2 * (float)integerFromStringEnd + this.waterSplashPhaseOffset) % 6.28f;
					float num5 = (num4 + num2 * this.waterSplashIntervalMultiplier) % 6.28f;
					if (WindMill.IsRotationPhaseInsidePhaseBoundaries(num3, num4, num5))
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

		// Token: 0x0600348D RID: 13453 RVA: 0x000D91F0 File Offset: 0x000D73F0
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.TickParallel;
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000D91FA File Offset: 0x000D73FA
		protected internal override void OnTickParallel(float dt)
		{
			this.Rotate(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000D9211 File Offset: 0x000D7411
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.Rotate(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000D9230 File Offset: 0x000D7430
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "testMesh")
			{
				if (this.testMesh != null)
				{
					base.GameEntity.AddMultiMesh(this.testMesh, true);
					return;
				}
			}
			else if (variableName == "testTexture")
			{
				if (this.testTexture != null)
				{
					Material material = base.GameEntity.GetFirstMesh().GetMaterial().CreateCopy();
					material.SetTexture(Material.MBTextureType.DiffuseMap, this.testTexture);
					base.GameEntity.SetMaterialForAllMeshes(material);
					return;
				}
			}
			else
			{
				if (variableName == "testEntity")
				{
					this.testEntity != null;
					return;
				}
				if (variableName == "testButton")
				{
					this.rotationSpeed *= 2f;
				}
			}
		}

		// Token: 0x04001651 RID: 5713
		public float rotationSpeed = 100f;

		// Token: 0x04001652 RID: 5714
		public float waterSplashPhaseOffset;

		// Token: 0x04001653 RID: 5715
		public float waterSplashIntervalMultiplier = 1f;

		// Token: 0x04001654 RID: 5716
		public MetaMesh testMesh;

		// Token: 0x04001655 RID: 5717
		public Texture testTexture;

		// Token: 0x04001656 RID: 5718
		public GameEntity testEntity;

		// Token: 0x04001657 RID: 5719
		public bool isWaterMill;

		// Token: 0x04001658 RID: 5720
		private float currentRotation;
	}
}
