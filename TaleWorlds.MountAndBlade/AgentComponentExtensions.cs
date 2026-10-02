using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010C RID: 268
	public static class AgentComponentExtensions
	{
		// Token: 0x06000D85 RID: 3461 RVA: 0x00018930 File Offset: 0x00016B30
		public static float GetMorale(this Agent agent)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent != null)
			{
				return commonAIComponent.Morale;
			}
			return -1f;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00018954 File Offset: 0x00016B54
		public static void SetMorale(this Agent agent, float morale)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent != null)
			{
				commonAIComponent.Morale = morale;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00018974 File Offset: 0x00016B74
		public static void ChangeMorale(this Agent agent, float delta)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent != null)
			{
				commonAIComponent.Morale += delta;
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0001899C File Offset: 0x00016B9C
		public static bool IsRetreating(this Agent agent, bool isComponentAssured = true)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			return commonAIComponent != null && commonAIComponent.IsRetreating;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000189BB File Offset: 0x00016BBB
		public static void Retreat(this Agent agent, bool useCachingSystem = false)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent == null)
			{
				return;
			}
			commonAIComponent.Retreat(useCachingSystem);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000189CE File Offset: 0x00016BCE
		public static void StopRetreatingMoraleComponent(this Agent agent)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent == null)
			{
				return;
			}
			commonAIComponent.StopRetreating();
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000189E0 File Offset: 0x00016BE0
		public static void SetBehaviorValueSet(this Agent agent, HumanAIComponent.BehaviorValueSet behaviorValueSet)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return;
			}
			humanAIComponent.SetBehaviorValueSet(behaviorValueSet);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000189F3 File Offset: 0x00016BF3
		public static void RefreshBehaviorValues(this Agent agent, MovementOrder.MovementOrderEnum movementOrder, ArrangementOrder.ArrangementOrderEnum arrangementOrder)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return;
			}
			humanAIComponent.RefreshBehaviorValues(movementOrder, arrangementOrder);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00018A07 File Offset: 0x00016C07
		public static void SetAIBehaviorValues(this Agent agent, HumanAIComponent.AISimpleBehaviorKind behavior, float y1, float x2, float y2, float x3, float y3)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return;
			}
			humanAIComponent.OverrideBehaviorParams(behavior, y1, x2, y2, x3, y3);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00018A22 File Offset: 0x00016C22
		public static void AIMoveToGameObjectEnable(this Agent agent, UsableMissionObject usedObject, IDetachment detachment, Agent.AIScriptedFrameFlags scriptedFrameFlags = Agent.AIScriptedFrameFlags.NoAttack)
		{
			agent.HumanAIComponent.MoveToUsableGameObject(usedObject, detachment, scriptedFrameFlags);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00018A32 File Offset: 0x00016C32
		public static void AIMoveToGameObjectDisable(this Agent agent)
		{
			agent.HumanAIComponent.MoveToClear();
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00018A3F File Offset: 0x00016C3F
		public static bool AIMoveToGameObjectIsEnabled(this Agent agent)
		{
			return agent.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.UseObjectMoving);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00018A4E File Offset: 0x00016C4E
		public static void AIDefendGameObjectEnable(this Agent agent, UsableMissionObject usedObject, IDetachment detachment)
		{
			agent.HumanAIComponent.StartDefendingGameObject(usedObject, detachment);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00018A5D File Offset: 0x00016C5D
		public static void AIDefendGameObjectDisable(this Agent agent)
		{
			agent.HumanAIComponent.StopDefendingGameObject();
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00018A6A File Offset: 0x00016C6A
		public static bool AIDefendGameObjectIsEnabled(this Agent agent)
		{
			return agent.HumanAIComponent.IsDefending;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00018A77 File Offset: 0x00016C77
		public static bool AIInterestedInAnyGameObject(this Agent agent)
		{
			return agent.HumanAIComponent.IsInterestedInAnyGameObject();
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00018A84 File Offset: 0x00016C84
		public static bool AIInterestedInGameObject(this Agent agent, UsableMissionObject usableMissionObject)
		{
			return agent.HumanAIComponent.IsInterestedInGameObject(usableMissionObject);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00018A92 File Offset: 0x00016C92
		public static void AIUseGameObjectEnable(this Agent agent)
		{
			agent.AIStateFlags |= Agent.AIStateFlag.UseObjectUsing;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00018AA3 File Offset: 0x00016CA3
		public static void AIUseGameObjectDisable(this Agent agent)
		{
			agent.AIStateFlags &= ~Agent.AIStateFlag.UseObjectUsing;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00018AB4 File Offset: 0x00016CB4
		public static bool AIUseGameObjectIsEnabled(this Agent agent)
		{
			return agent.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.UseObjectUsing);
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00018AC3 File Offset: 0x00016CC3
		public static Agent GetFollowedUnit(this Agent agent)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return null;
			}
			return humanAIComponent.FollowedAgent;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00018AD6 File Offset: 0x00016CD6
		public static void SetFollowedUnit(this Agent agent, Agent followedUnit)
		{
			agent.HumanAIComponent.FollowAgent(followedUnit);
		}
	}
}
