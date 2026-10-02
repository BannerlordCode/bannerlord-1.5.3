using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026E RID: 622
	public abstract class MissionBehavior : IMissionBehavior
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x0007B129 File Offset: 0x00079329
		// (set) Token: 0x060022FA RID: 8954 RVA: 0x0007B131 File Offset: 0x00079331
		public Mission Mission { get; internal set; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x0007B13A File Offset: 0x0007933A
		public IInputContext DebugInput
		{
			get
			{
				return Input.DebugInput;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060022FC RID: 8956
		public abstract MissionBehaviorType BehaviorType { get; }

		// Token: 0x060022FD RID: 8957 RVA: 0x0007B141 File Offset: 0x00079341
		public virtual void OnAfterMissionCreated()
		{
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0007B143 File Offset: 0x00079343
		public virtual void OnBehaviorInitialize()
		{
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0007B145 File Offset: 0x00079345
		public virtual void OnCreated()
		{
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0007B147 File Offset: 0x00079347
		public virtual void EarlyStart()
		{
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0007B149 File Offset: 0x00079349
		public virtual void AfterStart()
		{
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0007B14B File Offset: 0x0007934B
		public virtual void OnAfterMissionLoadingFinished()
		{
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0007B14D File Offset: 0x0007934D
		public virtual void OnMissileHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0007B14F File Offset: 0x0007934F
		public virtual void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0007B151 File Offset: 0x00079351
		public virtual void OnMissileCollisionReaction(Mission.MissileCollisionReaction collisionReaction, Agent attackerAgent, Agent attachedAgent, sbyte attachedBoneIndex)
		{
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0007B153 File Offset: 0x00079353
		public virtual void OnMissionScreenPreLoad()
		{
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0007B155 File Offset: 0x00079355
		public virtual void OnAgentCreated(Agent agent)
		{
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0007B157 File Offset: 0x00079357
		public virtual void OnAgentBuild(Agent agent, Banner banner)
		{
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0007B159 File Offset: 0x00079359
		public virtual void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0007B15B File Offset: 0x0007935B
		public virtual void OnAgentControllerSetToPlayer(Agent agent)
		{
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0007B15D File Offset: 0x0007935D
		public virtual void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0007B15F File Offset: 0x0007935F
		public virtual void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0007B161 File Offset: 0x00079361
		public virtual void OnEarlyAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0007B163 File Offset: 0x00079363
		public virtual void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0007B165 File Offset: 0x00079365
		public virtual void OnAgentDeleted(Agent affectedAgent)
		{
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0007B167 File Offset: 0x00079367
		public virtual void OnAgentFleeing(Agent affectedAgent)
		{
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0007B169 File Offset: 0x00079369
		public virtual void OnAgentPanicked(Agent affectedAgent)
		{
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0007B16B File Offset: 0x0007936B
		public virtual void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0007B16D File Offset: 0x0007936D
		public virtual void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0007B16F File Offset: 0x0007936F
		public virtual void OnAddTeam(Team team)
		{
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0007B171 File Offset: 0x00079371
		public virtual void AfterAddTeam(Team team)
		{
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0007B173 File Offset: 0x00079373
		public virtual void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0007B175 File Offset: 0x00079375
		public virtual void OnClearScene()
		{
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0007B177 File Offset: 0x00079377
		public virtual void OnEndMissionInternal()
		{
			this.OnEndMission();
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0007B17F File Offset: 0x0007937F
		protected virtual void OnEndMission()
		{
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0007B181 File Offset: 0x00079381
		public virtual void OnRemoveBehavior()
		{
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0007B183 File Offset: 0x00079383
		public virtual void OnFixedMissionTick(float fixedDt)
		{
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0007B185 File Offset: 0x00079385
		public virtual void OnPreMissionTick(float dt)
		{
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0007B187 File Offset: 0x00079387
		public virtual void OnPreDisplayMissionTick(float dt)
		{
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0007B189 File Offset: 0x00079389
		public virtual void OnMissionTick(float dt)
		{
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0007B18B File Offset: 0x0007938B
		public virtual void OnAgentMount(Agent agent)
		{
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0007B18D File Offset: 0x0007938D
		public virtual void OnAgentDismount(Agent agent)
		{
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0007B18F File Offset: 0x0007938F
		public virtual bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return false;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0007B192 File Offset: 0x00079392
		public virtual void OnEntityRemoved(GameEntity entity)
		{
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0007B194 File Offset: 0x00079394
		public virtual void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0007B196 File Offset: 0x00079396
		public virtual void OnObjectStoppedBeingUsed(Agent userAgent, UsableMissionObject usedObject)
		{
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0007B198 File Offset: 0x00079398
		public virtual void OnRenderingStarted()
		{
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0007B19A File Offset: 0x0007939A
		public virtual void OnMissionStateActivated()
		{
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0007B19C File Offset: 0x0007939C
		public virtual void OnMissionStateFinalized()
		{
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0007B19E File Offset: 0x0007939E
		public virtual void OnMissionStateDeactivated()
		{
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0007B1A0 File Offset: 0x000793A0
		public virtual List<CompassItemUpdateParams> GetCompassTargets()
		{
			return null;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0007B1A3 File Offset: 0x000793A3
		public virtual void OnAssignPlayerAsSergeantOfFormation(Agent agent)
		{
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0007B1A5 File Offset: 0x000793A5
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0007B1A7 File Offset: 0x000793A7
		public virtual void OnAfterDeploymentFinished()
		{
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0007B1A9 File Offset: 0x000793A9
		public virtual void OnBattleSideSpawned(BattleSideEnum side)
		{
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0007B1AB File Offset: 0x000793AB
		protected internal virtual void OnGetAgentState(Agent agent, bool usedSurgery)
		{
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0007B1AD File Offset: 0x000793AD
		public virtual void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0007B1AF File Offset: 0x000793AF
		protected internal virtual void OnObjectDisabled(DestructableComponent destructionComponent)
		{
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0007B1B1 File Offset: 0x000793B1
		public virtual void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0007B1B3 File Offset: 0x000793B3
		protected internal virtual void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0007B1B5 File Offset: 0x000793B5
		public virtual void OnRegisterBlow(Agent attacker, Agent victim, WeakGameEntity realHitEntity, Blow b, ref AttackCollisionData collisionData, in MissionWeapon attackerWeapon)
		{
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0007B1B7 File Offset: 0x000793B7
		public virtual void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
		{
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0007B1B9 File Offset: 0x000793B9
		public virtual void OnMissileRemoved(int MissileIndex)
		{
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0007B1BB File Offset: 0x000793BB
		public virtual void OnTutorialCompleted(string completedTutorialIdentifier)
		{
		}
	}
}
