using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000062 RID: 98
	public class BattleAgentLogic : MissionLogic
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0001618B File Offset: 0x0001438B
		private TroopUpgradeTracker _troopUpgradeTracker
		{
			get
			{
				return MapEvent.PlayerMapEvent.TroopUpgradeTracker;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00016197 File Offset: 0x00014397
		public override void AfterStart()
		{
			this._battleObserverMissionLogic = Mission.Current.GetMissionBehavior<BattleObserverMissionLogic>();
			this.CheckPerkEffectsOnTeams();
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000161B0 File Offset: 0x000143B0
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (this._battleObserverMissionLogic != null && agent.Character != null && agent.Origin != null)
			{
				PartyBase partyBase = (PartyBase)agent.Origin.BattleCombatant;
				CharacterObject characterObject = (CharacterObject)agent.Character;
				if (partyBase != null)
				{
					TroopUpgradeTracker troopUpgradeTracker = this._troopUpgradeTracker;
					if (troopUpgradeTracker == null)
					{
						return;
					}
					troopUpgradeTracker.AddTrackedTroop(partyBase, characterObject);
				}
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00016208 File Offset: 0x00014408
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			if (affectedAgent.Character != null && affectorAgent != null && affectorAgent.Character != null && affectedAgent.State == AgentState.Active && affectorAgent != null)
			{
				bool flag = affectedAgent.Health - (float)blow.InflictedDamage < 1f;
				bool flag2 = false;
				if (affectedAgent.Team != null && affectorAgent.Team != null)
				{
					flag2 = affectedAgent.Team.Side == affectorAgent.Team.Side;
				}
				IAgentOriginBase origin = affectorAgent.Origin;
				BasicCharacterObject character = affectedAgent.Character;
				Formation formation = affectorAgent.Formation;
				BasicCharacterObject basicCharacterObject;
				if (formation == null)
				{
					basicCharacterObject = null;
				}
				else
				{
					Agent captain = formation.Captain;
					basicCharacterObject = ((captain != null) ? captain.Character : null);
				}
				int inflictedDamage = blow.InflictedDamage;
				bool flag3 = flag;
				bool flag4 = flag2;
				MissionWeapon missionWeapon = attackerWeapon;
				origin.OnScoreHit(character, basicCharacterObject, inflictedDamage, flag3, flag4, missionWeapon.CurrentUsageItem);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000162CC File Offset: 0x000144CC
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			if (prevTeam != null && prevTeam != Team.Invalid && newTeam != null && prevTeam != newTeam)
			{
				BattleObserverMissionLogic battleObserverMissionLogic = this._battleObserverMissionLogic;
				if (battleObserverMissionLogic == null)
				{
					return;
				}
				IBattleObserver battleObserver = battleObserverMissionLogic.BattleObserver;
				if (battleObserver == null)
				{
					return;
				}
				battleObserver.TroopSideChanged((prevTeam != null) ? prevTeam.Side : BattleSideEnum.None, (newTeam != null) ? newTeam.Side : BattleSideEnum.None, (PartyBase)agent.Origin.BattleCombatant, agent.Character);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00016334 File Offset: 0x00014534
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent == null)
			{
				return;
			}
			if (affectorAgent.IsMount && affectorAgent.RiderAgent != null)
			{
				affectorAgent = affectorAgent.RiderAgent;
			}
			if (affectorAgent.Character == null || affectedAgent.Character == null)
			{
				return;
			}
			float num = (float)blow.InflictedDamage;
			if (num > affectedAgent.HealthLimit)
			{
				num = affectedAgent.HealthLimit;
			}
			float num2 = num / affectedAgent.HealthLimit;
			this.EnemyHitReward(affectedAgent, affectorAgent, blow.MovementSpeedDamageModifier, shotDifficulty, isSiegeEngineHit, attackerWeapon, blow.AttackType, 0.5f * num2, num, collisionData.IsSneakAttack);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000163BC File Offset: 0x000145BC
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent == null && affectedAgent.IsMount && agentState == AgentState.Routed)
			{
				return;
			}
			CharacterObject characterObject = (CharacterObject)affectedAgent.Character;
			CharacterObject characterObject2 = (CharacterObject)((affectorAgent != null) ? affectorAgent.Character : null);
			if (affectedAgent.Origin != null)
			{
				PartyBase partyBase = (PartyBase)affectedAgent.Origin.BattleCombatant;
				if (agentState == AgentState.Unconscious)
				{
					affectedAgent.Origin.SetWounded();
					return;
				}
				if (agentState == AgentState.Killed)
				{
					affectedAgent.Origin.SetKilled();
					Hero hero = (affectedAgent.IsHuman ? characterObject.HeroObject : null);
					Hero hero2 = ((affectorAgent == null) ? null : (affectorAgent.IsHuman ? characterObject2.HeroObject : null));
					if (hero != null && hero2 != null)
					{
						CampaignEventDispatcher.Instance.OnCharacterDefeated(hero2, hero);
					}
					if (partyBase != null)
					{
						this.CheckUpgrade(affectedAgent.Team.Side, partyBase, characterObject);
						return;
					}
				}
				else
				{
					bool flag = affectedAgent.GetMorale() < 0.01f;
					affectedAgent.Origin.SetRouted(!flag);
				}
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000164A7 File Offset: 0x000146A7
		public override void OnAgentFleeing(Agent affectedAgent)
		{
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000164A9 File Offset: 0x000146A9
		public override void OnMissionTick(float dt)
		{
			this.UpdateMorale();
			if (this._nextMoraleCheckTime.IsPast)
			{
				this._nextMoraleCheckTime = MissionTime.SecondsFromNow(10f);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000164CE File Offset: 0x000146CE
		private void CheckPerkEffectsOnTeams()
		{
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000164D0 File Offset: 0x000146D0
		private void UpdateMorale()
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000164D4 File Offset: 0x000146D4
		private void EnemyHitReward(Agent affectedAgent, Agent affectorAgent, float lastSpeedBonus, float lastShotDifficulty, bool isSiegeEngineHit, WeaponComponentData lastAttackerWeapon, AgentAttackType attackType, float hitpointRatio, float damageAmount, bool isSneakAttack)
		{
			CharacterObject characterObject = (CharacterObject)affectedAgent.Character;
			CharacterObject characterObject2 = (CharacterObject)affectorAgent.Character;
			if (affectedAgent.Origin != null && affectorAgent != null && affectorAgent.Origin != null && affectorAgent.Team != null && affectorAgent.Team.IsValid && affectedAgent.Team != null && affectedAgent.Team.IsValid)
			{
				PartyBase partyBase = (PartyBase)affectorAgent.Origin.BattleCombatant;
				Hero captain = BattleAgentLogic.GetCaptain(affectorAgent);
				Hero hero = ((affectorAgent.Team.Leader != null && affectorAgent.Team.Leader.Character.IsHero) ? ((CharacterObject)affectorAgent.Team.Leader.Character).HeroObject : null);
				bool flag = affectorAgent.Team.Side == affectedAgent.Team.Side;
				bool flag2 = affectorAgent.MountAgent != null;
				bool flag3 = flag2 && attackType == AgentAttackType.Collision;
				SkillLevelingManager.OnCombatHit(characterObject2, characterObject, (captain != null) ? captain.CharacterObject : null, hero, lastSpeedBonus, lastShotDifficulty, lastAttackerWeapon, hitpointRatio, CombatXpModel.MissionTypeEnum.Battle, flag2, flag, hero != null && affectorAgent.Character != hero.CharacterObject && (hero != Hero.MainHero || affectorAgent.Formation == null || !affectorAgent.Formation.IsAIControlled), damageAmount, affectedAgent.Health < 1f, isSiegeEngineHit, flag3, isSneakAttack);
				BattleObserverMissionLogic battleObserverMissionLogic = this._battleObserverMissionLogic;
				if (((battleObserverMissionLogic != null) ? battleObserverMissionLogic.BattleObserver : null) != null && affectorAgent.Character != null)
				{
					if (affectorAgent.Character.IsHero)
					{
						Hero heroObject = characterObject2.HeroObject;
						using (IEnumerator<SkillObject> enumerator = this._troopUpgradeTracker.CheckSkillUpgrades(heroObject).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								SkillObject skillObject = enumerator.Current;
								this._battleObserverMissionLogic.BattleObserver.HeroSkillIncreased(affectorAgent.Team.Side, partyBase, characterObject2, skillObject);
							}
							return;
						}
					}
					this.CheckUpgrade(affectorAgent.Team.Side, partyBase, characterObject2);
				}
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000166F8 File Offset: 0x000148F8
		private static Hero GetCaptain(Agent affectorAgent)
		{
			Hero hero = null;
			if (affectorAgent.Formation != null)
			{
				Agent captain = affectorAgent.Formation.Captain;
				if (captain != null)
				{
					float captainRadius = Campaign.Current.Models.CombatXpModel.CaptainRadius;
					if (captain.Position.Distance(affectorAgent.Position) < captainRadius)
					{
						hero = ((CharacterObject)captain.Character).HeroObject;
					}
				}
			}
			return hero;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001675C File Offset: 0x0001495C
		private void CheckUpgrade(BattleSideEnum side, PartyBase party, CharacterObject character)
		{
			BattleObserverMissionLogic battleObserverMissionLogic = this._battleObserverMissionLogic;
			if (((battleObserverMissionLogic != null) ? battleObserverMissionLogic.BattleObserver : null) != null)
			{
				int num = this._troopUpgradeTracker.CheckUpgradedCount(party, character);
				if (num != 0)
				{
					this._battleObserverMissionLogic.BattleObserver.TroopNumberChanged(side, party, character, 0, 0, 0, 0, 0, num);
				}
			}
		}

		// Token: 0x04000203 RID: 515
		private BattleObserverMissionLogic _battleObserverMissionLogic;

		// Token: 0x04000204 RID: 516
		private const float XpShareForKill = 0.5f;

		// Token: 0x04000205 RID: 517
		private const float XpShareForDamage = 0.5f;

		// Token: 0x04000206 RID: 518
		private MissionTime _nextMoraleCheckTime;
	}
}
