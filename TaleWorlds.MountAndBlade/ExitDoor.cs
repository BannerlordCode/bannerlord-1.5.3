using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000338 RID: 824
	public class ExitDoor : UsableMachine
	{
		// Token: 0x06002EAE RID: 11950 RVA: 0x000B4878 File Offset: 0x000B2A78
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=gqQPSAQZ}{KEY} Leave Area", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000B48A7 File Offset: 0x000B2AA7
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000B48AA File Offset: 0x000B2AAA
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000B48BE File Offset: 0x000B2ABE
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000B48C8 File Offset: 0x000B2AC8
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser)
				{
					Agent userAgent = standingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
					ActionIndexCache currentAction2 = userAgent.GetCurrentAction(1);
					if (!(currentAction2 == ActionIndexCache.act_none) || (!(currentAction == ActionIndexCache.act_pickup_middle_begin) && !(currentAction == ActionIndexCache.act_pickup_middle_begin_left_stance)))
					{
						if (currentAction2 == ActionIndexCache.act_none && (currentAction == ActionIndexCache.act_pickup_middle_end || currentAction == ActionIndexCache.act_pickup_middle_end_left_stance))
						{
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							Mission.Current.EndMission();
						}
						else
						{
							if (!(currentAction2 != ActionIndexCache.act_none))
							{
								Agent agent = userAgent;
								int num = 0;
								ActionIndexCache actionIndexCache = (userAgent.GetIsLeftStance() ? ActionIndexCache.act_pickup_middle_begin_left_stance : ActionIndexCache.act_pickup_middle_begin);
								if (agent.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									continue;
								}
							}
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
			}
		}
	}
}
