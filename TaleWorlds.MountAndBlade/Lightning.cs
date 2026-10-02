using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033D RID: 829
	public class Lightning : ScriptComponentBehavior
	{
		// Token: 0x06002ED1 RID: 11985 RVA: 0x000B534A File Offset: 0x000B354A
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000B5354 File Offset: 0x000B3554
		protected internal override void OnInit()
		{
			base.OnInit();
			this._lightningTimer = MBRandom.RandomFloat * this.LightningRate;
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000B537A File Offset: 0x000B357A
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000B537D File Offset: 0x000B357D
		protected internal override void OnEditorTick(float dt)
		{
			this.TickAux(dt);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000B5386 File Offset: 0x000B3586
		protected internal override void OnTick(float dt)
		{
			this.TickAux(dt);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000B5390 File Offset: 0x000B3590
		private void SpawnBolt()
		{
			this._boltIntensity = 1f;
			Random random = new Random();
			float num = (random.NextFloat() * 2f - 1f) * this.LightTravelRadius;
			float num2 = (random.NextFloat() * 2f - 1f) * this.LightTravelRadius;
			Vec3 vec = new Vec3(num, num2, 0f, -1f);
			base.GameEntity.SetLocalPosition(vec);
			string text = "event:/map/ambient/node/thunder";
			Vec3 globalPosition = base.GameEntity.GlobalPosition;
			SoundManager.StartOneShotEvent(text, in globalPosition);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000B5420 File Offset: 0x000B3620
		private void SetLightIntensity(float intensity)
		{
			Light light = base.GameEntity.GetComponentAtIndex(0, TaleWorlds.Engine.GameEntity.ComponentType.Light) as Light;
			Light light2 = base.GameEntity.GetComponentAtIndex(1, TaleWorlds.Engine.GameEntity.ComponentType.Light) as Light;
			float timeOfDay = base.Scene.TimeOfDay;
			if (light != null && light2 != null)
			{
				if (timeOfDay < 3.5f || timeOfDay > 21f)
				{
					float num = ((timeOfDay > 20f) ? MBMath.Map(timeOfDay, 20f, 24f, 4f, 1f) : MBMath.Map(timeOfDay, 0f, 6f, 1f, 4f));
					light.Intensity = this.LightIntensity * intensity * num;
					light2.Intensity = this.LightIntensity * intensity * num / 4f;
					return;
				}
				float num2 = ((timeOfDay < 12f) ? MBMath.Map(timeOfDay, 6f, 12f, 100f, 200f) : MBMath.Map(timeOfDay, 12f, 20f, 200f, 100f));
				light.Intensity = this.LightIntensity * intensity * num2;
				light2.Intensity = this.LightIntensity * intensity * num2 / 2f;
			}
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000B555E File Offset: 0x000B375E
		private float CalculateBoltIntensity(float dt)
		{
			this._boltIntensity -= dt * this.BoltSpeed;
			if (this._boltIntensity < 0f)
			{
				this._boltIntensity = 0f;
			}
			return this._boltIntensity;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000B5594 File Offset: 0x000B3794
		private void TickAux(float dt)
		{
			this._lightningTimer += dt;
			if (this.IsBoltEnabled)
			{
				base.GameEntity.ResumeParticleSystem(true);
			}
			if (this._lightningTimer > this.LightningRate)
			{
				this._lightningTimer = 0f;
				this._spawnLightning = true;
			}
			if (this._spawnLightning)
			{
				this._boltTimer += dt * this.BoltSpeed;
				this.SetLightIntensity(this.CalculateBoltIntensity(dt));
				if (this._boltTimer > this.BoltLife)
				{
					if (this.IsBoltEnabled)
					{
						base.GameEntity.PauseParticleSystem(false);
					}
					this.SpawnBolt();
					if (this.IsBoltEnabled)
					{
						base.GameEntity.BurstEntityParticle(false);
					}
					this.SetLightIntensity(0f);
					this._currentNumberOfLightning++;
					if (this._currentNumberOfLightning == 2)
					{
						this._spawnLightning = false;
						this._currentNumberOfLightning = 0;
					}
					this._boltTimer = 0f;
				}
			}
		}

		// Token: 0x0400128E RID: 4750
		public float LightIntensity = 1000f;

		// Token: 0x0400128F RID: 4751
		public float LightningRate = 5f;

		// Token: 0x04001290 RID: 4752
		public float BoltSpeed = 1f;

		// Token: 0x04001291 RID: 4753
		public float LightTravelRadius = 5f;

		// Token: 0x04001292 RID: 4754
		public float BoltLife = 1f;

		// Token: 0x04001293 RID: 4755
		public bool IsBoltEnabled = true;

		// Token: 0x04001294 RID: 4756
		private float _boltTimer;

		// Token: 0x04001295 RID: 4757
		private float _lightningTimer;

		// Token: 0x04001296 RID: 4758
		private bool _spawnLightning;

		// Token: 0x04001297 RID: 4759
		private float _boltIntensity = 1f;

		// Token: 0x04001298 RID: 4760
		private int _currentNumberOfLightning;
	}
}
