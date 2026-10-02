using System;
using System.Linq;
using SandBox.Objects;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions
{
	// Token: 0x0200005C RID: 92
	public class CoverAnimalAgentComponent : AgentComponent, IFocusable
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00015199 File Offset: 0x00013399
		public bool IsMovementStarted
		{
			get
			{
				return this._agentState > CoverAnimalAgentComponent.NavigationState.WaitingToStart;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000395 RID: 917 RVA: 0x000151A4 File Offset: 0x000133A4
		public bool IsAtFinalPoint
		{
			get
			{
				return this._agentState == CoverAnimalAgentComponent.NavigationState.AtFinalPosition;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000396 RID: 918 RVA: 0x000151AF File Offset: 0x000133AF
		public FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Item;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000397 RID: 919 RVA: 0x000151B2 File Offset: 0x000133B2
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000151B5 File Offset: 0x000133B5
		public CoverAnimalAgentComponent(Agent agent)
			: base(agent)
		{
			this._agentState = CoverAnimalAgentComponent.NavigationState.WaitingToStart;
			this.Agent.SetMaximumSpeedLimit(1f, false);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000151D8 File Offset: 0x000133D8
		public void SetDynamicPatrolArea(GameEntity parentPatrolPoint)
		{
			this._patrolPoints = new PatrolPoint[parentPatrolPoint.ChildCount];
			bool flag = false;
			PatrolPoint[] array = new PatrolPoint[parentPatrolPoint.ChildCount];
			for (int i = 0; i < parentPatrolPoint.ChildCount; i++)
			{
				array[i] = parentPatrolPoint.GetChild(i).GetChild(0).GetFirstScriptOfType<PatrolPoint>();
				if (!flag)
				{
					flag = array[i].IsInfiniteWaitPoint;
				}
			}
			this._patrolPoints = array.OrderBy<PatrolPoint, int>((PatrolPoint x) => x.Index).ToArray<PatrolPoint>();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00015266 File Offset: 0x00013466
		public void StartMovement()
		{
			if (!this.IsMovementStarted)
			{
				this._agentState = CoverAnimalAgentComponent.NavigationState.NoTarget;
				this.Agent.SetMaximumSpeedLimit(1f, false);
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00015288 File Offset: 0x00013488
		public override void OnTick(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled && this._agentState != CoverAnimalAgentComponent.NavigationState.WaitingToStart)
			{
				if (this._waitTimer == null && this.IsTargetReached() && this._agentState != CoverAnimalAgentComponent.NavigationState.NoTarget)
				{
					PatrolPoint patrolPoint = this._patrolPoints[this._currentPatrolAreaIndex];
					float num = (float)patrolPoint.WaitDuration + MBRandom.RandomFloatRanged((float)(-(float)patrolPoint.WaitDeviation), (float)patrolPoint.WaitDeviation);
					this._waitTimer = new Timer(Mission.Current.CurrentTime, num, true);
				}
				if (this._agentState != CoverAnimalAgentComponent.NavigationState.AtFinalPosition)
				{
					if (!this._targetPosition.IsValid)
					{
						this.MoveAnimalToNextPatrolPoint();
					}
					Timer waitTimer = this._waitTimer;
					if (waitTimer != null && waitTimer.Check(Mission.Current.CurrentTime))
					{
						this._waitTimer = null;
						this.Agent.ClearTargetFrame();
						this._targetPosition = WorldPosition.Invalid;
						this._agentState = CoverAnimalAgentComponent.NavigationState.NoTarget;
					}
				}
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00015380 File Offset: 0x00013580
		private void DebugTick()
		{
			int num = this._currentPatrolAreaIndex;
			if (num == -1)
			{
				num = 0;
			}
			if (num + 1 >= this._patrolPoints.Length)
			{
			}
			for (int i = 0; i < this._patrolPoints.Length; i++)
			{
			}
			Timer waitTimer = this._waitTimer;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000153C4 File Offset: 0x000135C4
		public bool IsTargetReached()
		{
			if (this._targetDirection.IsValid && this._targetPosition.IsValid)
			{
				this._targetReached = (this.Agent.Position - this._targetPosition.GetGroundVec3()).LengthSquared < this._rangeThreshold * this._rangeThreshold;
			}
			return this._targetReached;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001542C File Offset: 0x0001362C
		public void SetTargetFrame(WorldPosition position, float rotation, float rangeThreshold = 1f, Agent.AIScriptedFrameFlags flags = Agent.AIScriptedFrameFlags.None)
		{
			if (this._agentState != CoverAnimalAgentComponent.NavigationState.NoTarget)
			{
				this.Agent.ClearTargetFrame();
				this._targetPosition = WorldPosition.Invalid;
				this._agentState = CoverAnimalAgentComponent.NavigationState.NoTarget;
			}
			this._targetPosition = position;
			this._targetDirection = Vec2.FromRotation(rotation);
			this._rangeThreshold = rangeThreshold;
			if (this.IsTargetReached())
			{
				this._targetPosition = WorldPosition.Invalid;
				this._agentState = CoverAnimalAgentComponent.NavigationState.NoTarget;
				return;
			}
			this.Agent.SetScriptedPosition(ref position, false, flags);
			this._agentState = CoverAnimalAgentComponent.NavigationState.GoToTarget;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x000154AC File Offset: 0x000136AC
		private void MoveAnimalToNextPatrolPoint()
		{
			this._waitTimer = null;
			if (this._patrolPoints[this._currentPatrolAreaIndex].IsInfiniteWaitPoint)
			{
				this._agentState = CoverAnimalAgentComponent.NavigationState.AtFinalPosition;
				return;
			}
			this._currentPatrolAreaIndex++;
			if (this._currentPatrolAreaIndex >= this._patrolPoints.Length)
			{
				this._currentPatrolAreaIndex = 0;
			}
			WeakGameEntity gameEntity = this._patrolPoints[this._currentPatrolAreaIndex].GameEntity;
			WorldPosition worldPosition = new WorldPosition(gameEntity.Scene, gameEntity.GlobalPosition);
			this.SetTargetFrame(worldPosition, gameEntity.GetFrame().rotation.f.RotationX, 1f, Agent.AIScriptedFrameFlags.DoNotRun);
			this._agentState = CoverAnimalAgentComponent.NavigationState.GoToTarget;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00015557 File Offset: 0x00013757
		public void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00015559 File Offset: 0x00013759
		public void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001555B File Offset: 0x0001375B
		public TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return null;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00015560 File Offset: 0x00013760
		public TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			TextObject textObject = GameTexts.FindText("str_key_action", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			textObject.SetTextVariable("ACTION", new TextObject("{=F7JGCr9s}Move", null));
			return textObject;
		}

		// Token: 0x040001D6 RID: 470
		private PatrolPoint[] _patrolPoints;

		// Token: 0x040001D7 RID: 471
		private int _currentPatrolAreaIndex;

		// Token: 0x040001D8 RID: 472
		private Timer _waitTimer;

		// Token: 0x040001D9 RID: 473
		private CoverAnimalAgentComponent.NavigationState _agentState;

		// Token: 0x040001DA RID: 474
		private WorldPosition _targetPosition;

		// Token: 0x040001DB RID: 475
		private Vec2 _targetDirection;

		// Token: 0x040001DC RID: 476
		private bool _targetReached;

		// Token: 0x040001DD RID: 477
		private float _rangeThreshold;

		// Token: 0x02000166 RID: 358
		private enum NavigationState
		{
			// Token: 0x040006E6 RID: 1766
			WaitingToStart,
			// Token: 0x040006E7 RID: 1767
			NoTarget,
			// Token: 0x040006E8 RID: 1768
			GoToTarget,
			// Token: 0x040006E9 RID: 1769
			AtFinalPosition
		}
	}
}
