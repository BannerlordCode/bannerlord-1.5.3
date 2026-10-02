using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000202 RID: 514
	public class DefaultDamageParticleModel : DamageParticleModel
	{
		// Token: 0x06001E1D RID: 7709 RVA: 0x00066928 File Offset: 0x00064B28
		public DefaultDamageParticleModel()
		{
			this._bloodStartHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_blood_sword_enter");
			this._bloodContinueHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_blood_sword_inside");
			this._bloodEndHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_blood_sword_exit");
			this._sweatStartHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_sweat_sword_enter");
			this._sweatContinueHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_sweat_sword_enter");
			this._sweatEndHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_sweat_sword_enter");
			this._missileHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_blood_sword_enter");
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000669DC File Offset: 0x00064BDC
		public override void GetMeleeAttackBloodParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, out HitParticleResultData particleResultData)
		{
			particleResultData.StartHitParticleIndex = this._bloodStartHitParticleIndex;
			particleResultData.ContinueHitParticleIndex = this._bloodContinueHitParticleIndex;
			particleResultData.EndHitParticleIndex = this._bloodEndHitParticleIndex;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x00066A05 File Offset: 0x00064C05
		public override void GetMeleeAttackSweatParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, out HitParticleResultData particleResultData)
		{
			particleResultData.StartHitParticleIndex = this._sweatStartHitParticleIndex;
			particleResultData.ContinueHitParticleIndex = this._sweatContinueHitParticleIndex;
			particleResultData.EndHitParticleIndex = this._sweatEndHitParticleIndex;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x00066A2E File Offset: 0x00064C2E
		public override int GetMissileAttackParticle(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData)
		{
			return this._missileHitParticleIndex;
		}

		// Token: 0x04000A5D RID: 2653
		private int _bloodStartHitParticleIndex = -1;

		// Token: 0x04000A5E RID: 2654
		private int _bloodContinueHitParticleIndex = -1;

		// Token: 0x04000A5F RID: 2655
		private int _bloodEndHitParticleIndex = -1;

		// Token: 0x04000A60 RID: 2656
		private int _sweatStartHitParticleIndex = -1;

		// Token: 0x04000A61 RID: 2657
		private int _sweatContinueHitParticleIndex = -1;

		// Token: 0x04000A62 RID: 2658
		private int _sweatEndHitParticleIndex = -1;

		// Token: 0x04000A63 RID: 2659
		private int _missileHitParticleIndex = -1;
	}
}
