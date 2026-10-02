using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010B RID: 267
	public class CommonAIComponent : AgentComponent
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x000181F4 File Offset: 0x000163F4
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x000181FC File Offset: 0x000163FC
		public bool IsPanicked { get; private set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00018205 File Offset: 0x00016405
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x0001820D File Offset: 0x0001640D
		public bool IsRetreating { get; private set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00018216 File Offset: 0x00016416
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x0001821E File Offset: 0x0001641E
		public int ReservedRiderAgentIndex { get; private set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00018227 File Offset: 0x00016427
		public float InitialMorale
		{
			get
			{
				return this._initialMorale;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0001822F File Offset: 0x0001642F
		public float RecoveryMorale
		{
			get
			{
				return this._recoveryMorale;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00018237 File Offset: 0x00016437
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0001823F File Offset: 0x0001643F
		public float Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				this._morale = MBMath.ClampFloat(value, 0f, 100f);
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00018258 File Offset: 0x00016458
		public CommonAIComponent(Agent agent)
			: base(agent)
		{
			this._fadeOutTimer = new Timer(Mission.Current.CurrentTime, 0.5f + MBRandom.RandomFloat * 0.1f, true);
			float num = agent.Monster.BodyCapsuleRadius * 2f * 7.5f;
			this._retreatDistanceSquared = num * num;
			this.ReservedRiderAgentIndex = -1;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000182C6 File Offset: 0x000164C6
		public override void Initialize()
		{
			base.Initialize();
			this.InitializeMorale();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000182D4 File Offset: 0x000164D4
		private void InitializeMorale()
		{
			int num = MBRandom.RandomInt(30);
			float num2 = this.Agent.Components.Sum<AgentComponent>((AgentComponent c) => c.GetMoraleAddition());
			float num3 = 35f + (float)num + num2;
			num3 = MissionGameModels.Current.BattleMoraleModel.GetEffectiveInitialMorale(this.Agent, num3);
			num3 = MBMath.ClampFloat(num3, 15f, 100f);
			this._initialMorale = num3;
			this._recoveryMorale = this._initialMorale * 0.5f;
			this.Morale = this._initialMorale;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00018374 File Offset: 0x00016574
		public override void OnTickParallel(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled)
			{
				if (!this.IsRetreating && this._morale < 0.01f)
				{
					if (this.CanPanic())
					{
						this._panicTriggered = true;
					}
					else
					{
						this.Morale = 0.01f;
					}
				}
				if (!this.IsPanicked && !this._panicTriggered && this._morale < this._recoveryMorale)
				{
					this.Morale = Math.Min(this._morale + 0.4f * dt, this._recoveryMorale);
				}
				if (this._fadeOutTimer.Check(Mission.Current.CurrentTime) && Mission.Current.CanAgentRout(this.Agent) && !this.Agent.IsFadingOut())
				{
					Vec3 position = this.Agent.Position;
					WorldPosition retreatPos = this.Agent.GetRetreatPos();
					this._retreatDistanceSquared = this._fadeOutTimer.Duration * this.Agent.Velocity.AsVec2.LengthSquared + 2f * this.Agent.Monster.BodyCapsuleRadius;
					if ((retreatPos.AsVec2.IsValid && retreatPos.AsVec2.DistanceSquared(position.AsVec2) < this._retreatDistanceSquared && retreatPos.GetGroundVec3MT().DistanceSquared(position) < this._retreatDistanceSquared) || !this.Agent.Mission.IsPositionInsideBoundaries(position.AsVec2) || position.DistanceSquared(this.Agent.Mission.GetClosestBoundaryPosition(position.AsVec2).ToVec3(0f)) < this._retreatDistanceSquared)
					{
						this.Agent.StartFadingOut();
					}
				}
				if (this.IsPanicked && this.Agent.Mission.MissionEnded)
				{
					MissionResult missionResult = this.Agent.Mission.MissionResult;
					if (this.Agent.Team != null && missionResult != null && ((missionResult.PlayerVictory && (this.Agent.Team.IsPlayerTeam || this.Agent.Team.IsPlayerAlly)) || (missionResult.PlayerDefeated && !this.Agent.Team.IsPlayerTeam && !this.Agent.Team.IsPlayerAlly)) && this.Agent != Agent.Main && this.Agent.IsActive())
					{
						this.StopRetreating();
					}
				}
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0001860A File Offset: 0x0001680A
		public override void OnTick(float dt)
		{
			if (this._panicTriggered)
			{
				this._panicTriggered = false;
				this.Panic();
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00018621 File Offset: 0x00016821
		public void Panic()
		{
			this.Agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
			if (!this.IsPanicked)
			{
				this.IsPanicked = true;
				this.Agent.Mission.OnAgentPanicked(this.Agent);
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00018658 File Offset: 0x00016858
		public void Retreat(bool useCachingSystem = false)
		{
			if (!this.IsRetreating)
			{
				this.IsRetreating = true;
				this.Agent.EnforceShieldUsage(Agent.UsageDirection.None);
				WorldPosition worldPosition = WorldPosition.Invalid;
				if (useCachingSystem)
				{
					worldPosition = this.Agent.Formation.RetreatPositionCache.GetRetreatPositionFromCache(this.Agent.Position.AsVec2);
				}
				if (!worldPosition.IsValid)
				{
					worldPosition = this.Agent.Mission.GetClosestFleePositionForAgent(this.Agent);
					if (useCachingSystem)
					{
						this.Agent.Formation.RetreatPositionCache.AddNewPositionToCache(this.Agent.Position.AsVec2, worldPosition);
					}
				}
				this.Agent.Retreat(worldPosition);
			}
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00018710 File Offset: 0x00016910
		public void StopRetreating()
		{
			if (!this.IsRetreating)
			{
				return;
			}
			this.IsRetreating = false;
			this.IsPanicked = false;
			float num = MathF.Max(0.02f, this.Morale);
			this.Agent.SetMorale(num);
			this.Agent.StopRetreating();
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0001875C File Offset: 0x0001695C
		public bool CanPanic()
		{
			if (!MissionGameModels.Current.BattleMoraleModel.CanPanicDueToMorale(this.Agent))
			{
				return false;
			}
			TeamAISiegeComponent teamAISiegeComponent;
			if (Mission.Current.IsSiegeBattle && this.Agent.Team.Side == BattleSideEnum.Attacker && (teamAISiegeComponent = this.Agent.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				int currentNavigationFaceId = this.Agent.GetCurrentNavigationFaceId();
				if (currentNavigationFaceId % 10 == 1)
				{
					return false;
				}
				if (teamAISiegeComponent.IsPrimarySiegeWeaponNavmeshFaceId(currentNavigationFaceId))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000187DB File Offset: 0x000169DB
		public override void OnHit(Agent affectorAgent, int damage, in MissionWeapon affectorWeapon, in Blow b, in AttackCollisionData collisionData)
		{
			base.OnHit(affectorAgent, damage, in affectorWeapon, in b, in collisionData);
			if (damage >= 1 && this.Agent.IsMount && this.Agent.IsAIControlled && this.Agent.RiderAgent == null)
			{
				this.Panic();
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0001881C File Offset: 0x00016A1C
		public override void OnAgentRemoved()
		{
			base.OnAgentRemoved();
			if (this.Agent.IsMount && this.Agent.RiderAgent == null)
			{
				Agent agent = this.FindReservingAgent();
				if (agent != null)
				{
					agent.HumanAIComponent.UnreserveMount(this.Agent);
				}
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00018864 File Offset: 0x00016A64
		public override void OnComponentRemoved()
		{
			base.OnComponentRemoved();
			if (this.Agent.IsMount && this.Agent.RiderAgent == null)
			{
				Agent agent = this.FindReservingAgent();
				if (agent != null)
				{
					agent.HumanAIComponent.UnreserveMount(this.Agent);
				}
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000188AC File Offset: 0x00016AAC
		internal void OnMountReserved(int riderAgentIndex)
		{
			this.ReservedRiderAgentIndex = riderAgentIndex;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000188B5 File Offset: 0x00016AB5
		internal void OnMountUnreserved()
		{
			this.ReservedRiderAgentIndex = -1;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000188C0 File Offset: 0x00016AC0
		private Agent FindReservingAgent()
		{
			Agent agent = null;
			if (this.ReservedRiderAgentIndex >= 0)
			{
				foreach (Agent agent2 in Mission.Current.Agents)
				{
					if (agent2.Index == this.ReservedRiderAgentIndex)
					{
						agent = agent2;
						break;
					}
				}
			}
			return agent;
		}

		// Token: 0x040002FE RID: 766
		public const float MoraleThresholdForPanicking = 0.01f;

		// Token: 0x040002FF RID: 767
		private const float MaxRecoverableMoraleMultiplier = 0.5f;

		// Token: 0x04000300 RID: 768
		private const float MoraleRecoveryPerSecond = 0.4f;

		// Token: 0x04000304 RID: 772
		private float _recoveryMorale;

		// Token: 0x04000305 RID: 773
		private float _initialMorale;

		// Token: 0x04000306 RID: 774
		private float _morale = 50f;

		// Token: 0x04000307 RID: 775
		private bool _panicTriggered;

		// Token: 0x04000308 RID: 776
		private readonly Timer _fadeOutTimer;

		// Token: 0x04000309 RID: 777
		private float _retreatDistanceSquared;
	}
}
