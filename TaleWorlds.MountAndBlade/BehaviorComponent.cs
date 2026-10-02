using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000114 RID: 276
	public abstract class BehaviorComponent
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0001D045 File Offset: 0x0001B245
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x0001D04D File Offset: 0x0001B24D
		public Formation Formation { get; private set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0001D056 File Offset: 0x0001B256
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x0001D05E File Offset: 0x0001B25E
		public float BehaviorCoherence { get; set; }

		// Token: 0x06000DFA RID: 3578 RVA: 0x0001D068 File Offset: 0x0001B268
		protected BehaviorComponent(Formation formation)
		{
			this.Formation = formation;
			this.PreserveExpireTime = 0f;
			this._navmeshlessTargetPenaltyTime = new Timer(Mission.Current.CurrentTime, 50f, true);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0001D0BE File Offset: 0x0001B2BE
		protected BehaviorComponent()
		{
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0001D0DC File Offset: 0x0001B2DC
		private void InformSergeantPlayer()
		{
			if (Mission.Current.MainAgent != null && this.Formation.Team.GeneralAgent != null && !this.Formation.Team.IsPlayerGeneral && this.Formation.Team.IsPlayerSergeant && this.Formation.PlayerOwner == Agent.Main)
			{
				TextObject behaviorString = this.GetBehaviorString();
				MBTextManager.SetTextVariable("BEHAVIOR", behaviorString, false);
				MBTextManager.SetTextVariable("PLAYER_NAME", Mission.Current.MainAgent.NameTextObject, false);
				MBTextManager.SetTextVariable("TEAM_LEADER", this.Formation.Team.GeneralAgent.NameTextObject, false);
				MBInformationManager.AddQuickInformation(new TextObject("{=L91XKoMD}{TEAM_LEADER}: {PLAYER_NAME}, {BEHAVIOR}", null), 4000, this.Formation.Team.GeneralAgent.Character, null, "");
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0001D1C6 File Offset: 0x0001B3C6
		protected virtual void OnBehaviorActivatedAux()
		{
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0001D1C8 File Offset: 0x0001B3C8
		internal void OnBehaviorActivated()
		{
			if (!this.Formation.Team.IsPlayerGeneral && !this.Formation.Team.IsPlayerSergeant && this.Formation.IsPlayerTroopInFormation && Mission.Current.MainAgent != null)
			{
				TextObject textObject = new TextObject(this.ToString().Replace("MBModule.Behavior", ""), null);
				MBTextManager.SetTextVariable("BEHAVIOUR_NAME_BEGIN", textObject, false);
				textObject = GameTexts.FindText("str_formation_ai_soldier_instruction_text", null);
				MBInformationManager.AddQuickInformation(textObject, 2000, Mission.Current.MainAgent.Character, null, "");
			}
			if (!GameNetwork.IsMultiplayer)
			{
				this.InformSergeantPlayer();
				this._lastPlayerInformTime = Mission.Current.CurrentTime;
			}
			if (this.Formation.IsAIControlled)
			{
				this.OnBehaviorActivatedAux();
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0001D296 File Offset: 0x0001B496
		public virtual void OnBehaviorCanceled()
		{
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0001D298 File Offset: 0x0001B498
		public virtual void OnLostAIControl()
		{
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0001D29A File Offset: 0x0001B49A
		public virtual void OnAgentRemoved(Agent agent)
		{
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0001D29C File Offset: 0x0001B49C
		public void RemindSergeantPlayer()
		{
			float currentTime = Mission.Current.CurrentTime;
			if (this == this.Formation.AI.ActiveBehavior && this._lastPlayerInformTime + 60f < currentTime)
			{
				this.InformSergeantPlayer();
				this._lastPlayerInformTime = currentTime;
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0001D2E3 File Offset: 0x0001B4E3
		public virtual void TickOccasionally()
		{
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0001D2E8 File Offset: 0x0001B4E8
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0001D374 File Offset: 0x0001B574
		public virtual float NavmeshlessTargetPositionPenalty
		{
			get
			{
				if (this._navmeshlessTargetPositionPenalty == 1f)
				{
					return 1f;
				}
				this._navmeshlessTargetPenaltyTime.Check(Mission.Current.CurrentTime);
				float num = this._navmeshlessTargetPenaltyTime.ElapsedTime();
				if (num >= 10f)
				{
					this._navmeshlessTargetPositionPenalty = 1f;
					return 1f;
				}
				if (num <= 5f)
				{
					return this._navmeshlessTargetPositionPenalty;
				}
				return MBMath.Lerp(this._navmeshlessTargetPositionPenalty, 1f, (num - 5f) / 5f, 1E-05f);
			}
			set
			{
				this._navmeshlessTargetPenaltyTime.Reset(Mission.Current.CurrentTime);
				this._navmeshlessTargetPositionPenalty = value;
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0001D392 File Offset: 0x0001B592
		public float GetAIWeight()
		{
			return this.GetAiWeight() * this.NavmeshlessTargetPositionPenalty;
		}

		// Token: 0x06000E07 RID: 3591
		protected abstract float GetAiWeight();

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0001D3A1 File Offset: 0x0001B5A1
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0001D3A9 File Offset: 0x0001B5A9
		public MovementOrder CurrentOrder
		{
			get
			{
				return this._currentOrder;
			}
			protected set
			{
				this._currentOrder = value;
				this.IsCurrentOrderChanged = true;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0001D3B9 File Offset: 0x0001B5B9
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0001D3C1 File Offset: 0x0001B5C1
		public float PreserveExpireTime { get; set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0001D3CA File Offset: 0x0001B5CA
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0001D3D2 File Offset: 0x0001B5D2
		public float WeightFactor { get; set; }

		// Token: 0x06000E0E RID: 3598 RVA: 0x0001D3DB File Offset: 0x0001B5DB
		public virtual void ResetBehavior()
		{
			this.WeightFactor = 0f;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0001D3E8 File Offset: 0x0001B5E8
		public virtual TextObject GetBehaviorString()
		{
			string name = base.GetType().Name;
			return GameTexts.FindText("str_formation_ai_sergeant_instruction_behavior_text", name);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0001D40C File Offset: 0x0001B60C
		public virtual void OnValidBehaviorSideChanged()
		{
			this._behaviorSide = this.Formation.AI.Side;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0001D424 File Offset: 0x0001B624
		protected virtual void CalculateCurrentOrder()
		{
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0001D428 File Offset: 0x0001B628
		public void PrecalculateMovementOrder()
		{
			this.CalculateCurrentOrder();
			this.CurrentOrder.GetPosition(this.Formation);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0001D450 File Offset: 0x0001B650
		public override bool Equals(object obj)
		{
			return base.GetType() == obj.GetType();
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0001D463 File Offset: 0x0001B663
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0001D46B File Offset: 0x0001B66B
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x04000341 RID: 833
		protected FormationAI.BehaviorSide _behaviorSide;

		// Token: 0x04000342 RID: 834
		protected const float FormArrangementDistanceToOrderPosition = 10f;

		// Token: 0x04000343 RID: 835
		private const float _playerInformCooldown = 60f;

		// Token: 0x04000344 RID: 836
		protected float _lastPlayerInformTime;

		// Token: 0x04000345 RID: 837
		private Timer _navmeshlessTargetPenaltyTime;

		// Token: 0x04000346 RID: 838
		private float _navmeshlessTargetPositionPenalty = 1f;

		// Token: 0x04000347 RID: 839
		public bool IsCurrentOrderChanged;

		// Token: 0x04000348 RID: 840
		private MovementOrder _currentOrder;

		// Token: 0x04000349 RID: 841
		protected FacingOrder CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
	}
}
