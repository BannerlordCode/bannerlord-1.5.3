using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000200 RID: 512
	public abstract class DamageParticleModel : MBGameModel<DamageParticleModel>
	{
		// Token: 0x06001E17 RID: 7703
		public abstract void GetMeleeAttackBloodParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, out HitParticleResultData particleResultData);

		// Token: 0x06001E18 RID: 7704
		public abstract void GetMeleeAttackSweatParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, out HitParticleResultData particleResultData);

		// Token: 0x06001E19 RID: 7705
		public abstract int GetMissileAttackParticle(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData);
	}
}
