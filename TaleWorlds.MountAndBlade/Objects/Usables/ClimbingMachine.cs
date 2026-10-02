using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003B1 RID: 945
	public class ClimbingMachine : UsableMachine
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060035ED RID: 13805 RVA: 0x000DE73C File Offset: 0x000DC93C
		public override float SinkingReferenceOffset
		{
			get
			{
				return base.GameEntity.GetGlobalScale().z * -1.5f;
			}
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000DE762 File Offset: 0x000DC962
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=fEQAPJ2e}{KEY} Use", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000DE791 File Offset: 0x000DC991
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=mUIew6a6}Climbing Net", null);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000DE7A0 File Offset: 0x000DC9A0
		protected internal override void OnInit()
		{
			base.OnInit();
			this._climbingLoop = ActionIndexCache.Create("act_climb_net");
			this._climbingEnd = ActionIndexCache.Create("act_climb_net_ending");
			this._climbingEndContinue = ActionIndexCache.Create("act_climb_net_ending_continue");
			this._climbEndingPoint = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("climb_end"));
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AutoEquipWeaponsOnUseStopped = true;
				this._standingPointUsageDurations.Add(0f);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000DE864 File Offset: 0x000DCA64
		private void OnUseAction(Agent userAgent)
		{
			userAgent.SetForceAttachedEntity(base.GameEntity);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000DE872 File Offset: 0x000DCA72
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000DE878 File Offset: 0x000DCA78
		public override void OnDeploymentFinished()
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AddComponent(new ResetGravityExclusionAndEntityAttachmentOnStopUsageComponent(new Action<Agent>(this.OnUseAction)));
				standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
				standingPoint.SetAreUserPositionsUpdatedInTheMachineTick(true);
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000DE8F4 File Offset: 0x000DCAF4
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			bool flag = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = base.StandingPoints[i];
				float num = this._standingPointUsageDurations[i];
				if (standingPoint.HasUser)
				{
					Agent userAgent = standingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
					num += dt;
					if (currentAction == this._climbingLoop || currentAction == this._climbingEnd)
					{
						MatrixFrame globalFrame = standingPoint.GameEntity.GetGlobalFrame();
						Vec3 vec = globalFrame.origin + globalFrame.rotation.u.NormalizedCopy() * num * 1.4f;
						Vec2 vec3;
						if (currentAction == this._climbingEnd)
						{
							Vec3 vec2 = vec;
							vec3 = globalFrame.rotation.f.AsVec2;
							vec = vec2 + new Vec3(vec3.Normalized() * (Math.Min(userAgent.GetCurrentActionProgress(0) * 1f, 1f) * 0.3f), 0f, -1f);
						}
						userAgent.SetTargetZ(vec.z);
						Agent agent = userAgent;
						vec3 = vec.AsVec2;
						Vec3 vec4 = globalFrame.rotation.f.NormalizedCopy();
						agent.SetTargetPositionAndDirection(in vec3, in vec4);
						Vec3 vec5 = globalFrame.rotation.u.NormalizedCopy();
						userAgent.SetTargetUp(in vec5);
						float num2 = Vec3.DotProduct(vec5, this._climbEndingPoint.GlobalPosition - vec);
						if (currentAction == this._climbingLoop && num2 < 1.8f && !userAgent.SetActionChannel(0, in this._climbingEnd, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
						{
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							num = 0f;
						}
						else if (num2 > Vec3.DotProduct(vec5, this._climbEndingPoint.GlobalPosition - globalFrame.origin) - 1.5f)
						{
							flag = true;
						}
					}
					else if (currentAction == this._climbingEndContinue)
					{
						userAgent.ClearTargetFrame();
						userAgent.SetTargetZ(this._climbEndingPoint.GlobalPosition.z);
						userAgent.SetTargetUp(in Vec3.Zero);
						if (userAgent.GetCurrentActionProgress(0) > 0.95f)
						{
							userAgent.SetExcludedFromGravity(false, true);
							userAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							num = 0f;
						}
					}
					else if (userAgent.SetActionChannel(0, in this._climbingLoop, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
					{
						userAgent.SetExcludedFromGravity(true, false);
						flag = true;
					}
					else
					{
						userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						num = 0f;
					}
				}
				else
				{
					num = 0f;
				}
				this._standingPointUsageDurations[i] = num;
			}
			foreach (StandingPoint standingPoint2 in base.StandingPoints)
			{
				if (!standingPoint2.HasUser)
				{
					standingPoint2.IsDeactivated = flag;
					flag = true;
				}
			}
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x000DEC40 File Offset: 0x000DCE40
		public override void OnMissionEnded()
		{
		}

		// Token: 0x040016FB RID: 5883
		private const float ClimbingEndDisplacement = 0.3f;

		// Token: 0x040016FC RID: 5884
		private const float ClimbingSpeed = 1.4f;

		// Token: 0x040016FD RID: 5885
		private const float EndingAnimationTriggerDifference = 1.8f;

		// Token: 0x040016FE RID: 5886
		private const string ClimbingLoopActionName = "act_climb_net";

		// Token: 0x040016FF RID: 5887
		private const string ClimbingEndActionName = "act_climb_net_ending";

		// Token: 0x04001700 RID: 5888
		private const string ClimbingEndContinueActionName = "act_climb_net_ending_continue";

		// Token: 0x04001701 RID: 5889
		private ActionIndexCache _climbingLoop;

		// Token: 0x04001702 RID: 5890
		private ActionIndexCache _climbingEnd;

		// Token: 0x04001703 RID: 5891
		private ActionIndexCache _climbingEndContinue;

		// Token: 0x04001704 RID: 5892
		private GameEntity _climbEndingPoint;

		// Token: 0x04001705 RID: 5893
		private List<float> _standingPointUsageDurations = new List<float>();
	}
}
