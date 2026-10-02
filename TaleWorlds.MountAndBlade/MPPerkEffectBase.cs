using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031A RID: 794
	public abstract class MPPerkEffectBase
	{
		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002D6F RID: 11631 RVA: 0x000AFA40 File Offset: 0x000ADC40
		public virtual bool IsTickRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002D70 RID: 11632 RVA: 0x000AFA43 File Offset: 0x000ADC43
		// (set) Token: 0x06002D71 RID: 11633 RVA: 0x000AFA4B File Offset: 0x000ADC4B
		public bool IsDisabledInWarmup { get; protected set; }

		// Token: 0x06002D72 RID: 11634 RVA: 0x000AFA54 File Offset: 0x000ADC54
		public virtual void OnUpdate(Agent agent, bool newState)
		{
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000AFA58 File Offset: 0x000ADC58
		public virtual void OnTick(MissionPeer peer, int tickCount)
		{
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0)
			{
				MBReadOnlyList<IFormationUnit> mbreadOnlyList;
				if (peer == null)
				{
					mbreadOnlyList = null;
				}
				else
				{
					Formation controlledFormation = peer.ControlledFormation;
					mbreadOnlyList = ((controlledFormation != null) ? controlledFormation.Arrangement.GetAllUnits() : null);
				}
				MBReadOnlyList<IFormationUnit> mbreadOnlyList2 = mbreadOnlyList;
				if (mbreadOnlyList2 == null)
				{
					return;
				}
				using (List<IFormationUnit>.Enumerator enumerator = mbreadOnlyList2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent;
						if ((agent = enumerator.Current as Agent) != null && agent.IsActive())
						{
							this.OnTick(agent, tickCount);
						}
					}
					return;
				}
			}
			if (peer != null)
			{
				Agent controlledAgent = peer.ControlledAgent;
				bool? flag = ((controlledAgent != null) ? new bool?(controlledAgent.IsActive()) : null);
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					this.OnTick(peer.ControlledAgent, tickCount);
				}
			}
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000AFB30 File Offset: 0x000ADD30
		public virtual void OnTick(Agent agent, int tickCount)
		{
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000AFB32 File Offset: 0x000ADD32
		public virtual float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			return 0f;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000AFB39 File Offset: 0x000ADD39
		public virtual float GetMountDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			return 0f;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000AFB40 File Offset: 0x000ADD40
		public virtual float GetDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			return 0f;
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000AFB47 File Offset: 0x000ADD47
		public virtual float GetMountDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			return 0f;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000AFB4E File Offset: 0x000ADD4E
		public virtual float GetSpeedBonusEffectiveness(Agent attacker, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			return 0f;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000AFB55 File Offset: 0x000ADD55
		public virtual float GetShieldDamage(bool isCorrectSideBlock)
		{
			return 0f;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000AFB5C File Offset: 0x000ADD5C
		public virtual float GetShieldDamageTaken(bool isCorrectSideBlock)
		{
			return 0f;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000AFB63 File Offset: 0x000ADD63
		public virtual float GetRangedAccuracy()
		{
			return 0f;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000AFB6A File Offset: 0x000ADD6A
		public virtual float GetThrowingWeaponSpeed(WeaponComponentData attackerWeapon)
		{
			return 0f;
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000AFB71 File Offset: 0x000ADD71
		public virtual float GetDamageInterruptionThreshold()
		{
			return 0f;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000AFB78 File Offset: 0x000ADD78
		public virtual float GetMountManeuver()
		{
			return 0f;
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000AFB7F File Offset: 0x000ADD7F
		public virtual float GetMountSpeed()
		{
			return 0f;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000AFB86 File Offset: 0x000ADD86
		public virtual float GetRangedHeadShotDamage()
		{
			return 0f;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000AFB8D File Offset: 0x000ADD8D
		public virtual int GetGoldOnKill(float attackerValue, float victimValue)
		{
			return 0;
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000AFB90 File Offset: 0x000ADD90
		public virtual int GetGoldOnAssist()
		{
			return 0;
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000AFB93 File Offset: 0x000ADD93
		public virtual int GetRewardedGoldOnAssist()
		{
			return 0;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000AFB96 File Offset: 0x000ADD96
		public virtual bool GetIsTeamRewardedOnDeath()
		{
			return false;
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000AFB99 File Offset: 0x000ADD99
		public virtual void CalculateRewardedGoldOnDeath(Agent agent, List<ValueTuple<MissionPeer, int>> teamMembers)
		{
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000AFB9B File Offset: 0x000ADD9B
		public virtual float GetDrivenPropertyBonus(DrivenProperty drivenProperty, float baseValue)
		{
			return 0f;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000AFBA2 File Offset: 0x000ADDA2
		public virtual float GetEncumbrance(bool isOnBody)
		{
			return 0f;
		}

		// Token: 0x06002D89 RID: 11657
		protected abstract void Deserialize(XmlNode node);
	}
}
