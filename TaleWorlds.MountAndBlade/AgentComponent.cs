using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000102 RID: 258
	public abstract class AgentComponent
	{
		// Token: 0x06000C57 RID: 3159 RVA: 0x0001752A File Offset: 0x0001572A
		protected AgentComponent(Agent agent)
		{
			this.Agent = agent;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00017539 File Offset: 0x00015739
		public virtual void Initialize()
		{
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0001753B File Offset: 0x0001573B
		public virtual void OnTick(float dt)
		{
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0001753D File Offset: 0x0001573D
		public virtual void OnTickParallel(float dt)
		{
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0001753F File Offset: 0x0001573F
		public virtual float GetMoraleAddition()
		{
			return 0f;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00017546 File Offset: 0x00015746
		public virtual float GetMoraleDecreaseConstant()
		{
			return 1f;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0001754D File Offset: 0x0001574D
		public virtual void OnItemPickup(SpawnedItemEntity item)
		{
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0001754F File Offset: 0x0001574F
		public virtual void OnWeaponDrop(MissionWeapon droppedWeapon)
		{
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00017551 File Offset: 0x00015751
		public virtual void OnStopUsingGameObject()
		{
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00017553 File Offset: 0x00015753
		public virtual void OnWeaponHPChanged(ItemObject item, int hitPoints)
		{
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00017555 File Offset: 0x00015755
		public virtual void OnRetreating()
		{
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00017557 File Offset: 0x00015757
		public virtual void OnMount(Agent mount)
		{
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00017559 File Offset: 0x00015759
		public virtual void OnDismount(Agent mount)
		{
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0001755B File Offset: 0x0001575B
		public virtual void OnHit(Agent affectorAgent, int damage, in MissionWeapon affectorWeapon, in Blow b, in AttackCollisionData collisionData)
		{
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0001755D File Offset: 0x0001575D
		public virtual void OnDisciplineChanged()
		{
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0001755F File Offset: 0x0001575F
		public virtual void OnAgentRemoved()
		{
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00017561 File Offset: 0x00015761
		public virtual void OnAgentTeleported()
		{
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00017563 File Offset: 0x00015763
		public virtual void OnAIInputSet(ref Agent.EventControlFlag eventFlag, ref Agent.MovementControlFlag movementFlag, ref Vec2 inputVector)
		{
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00017565 File Offset: 0x00015765
		public virtual void OnComponentRemoved()
		{
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00017567 File Offset: 0x00015767
		public virtual void OnFormationSet()
		{
		}

		// Token: 0x040002CD RID: 717
		protected readonly Agent Agent;
	}
}
