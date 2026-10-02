using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects
{
	// Token: 0x02000037 RID: 55
	public class GenericMissionEventBox : VolumeBox
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x0000CBC8 File Offset: 0x0000ADC8
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(ScriptComponentBehavior.TickRequirement.Tick);
			using (IEnumerator<ScriptComponentBehavior> enumerator = base.GameEntity.GetScriptComponents().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenericMissionEventScript genericMissionEventScript;
					if ((genericMissionEventScript = enumerator.Current as GenericMissionEventScript) != null)
					{
						this._genericMissionEvents.Add(genericMissionEventScript);
					}
				}
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000CC38 File Offset: 0x0000AE38
		protected override void OnTick(float dt)
		{
			bool flag = true;
			using (List<GenericMissionEventScript>.Enumerator enumerator = this._genericMissionEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsDisabled)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				bool flag2 = false;
				foreach (Agent agent in Mission.Current.Agents)
				{
					if (agent.AgentVisuals.IsValid() && agent.AgentVisuals.GetEntity().Tags.Any<string>((string x) => !string.IsNullOrEmpty(x) && this.ActivatorAgentTags.Contains(x)) && base.IsPointIn(agent.Position))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					foreach (GenericMissionEventScript genericMissionEventScript in this._genericMissionEvents)
					{
						if (!genericMissionEventScript.IsDisabled)
						{
							Game.Current.EventManager.TriggerEvent<GenericMissionEvent>(new GenericMissionEvent(genericMissionEventScript.EventId, genericMissionEventScript.Parameter));
						}
					}
				}
			}
		}

		// Token: 0x040000B8 RID: 184
		public string ActivatorAgentTags;

		// Token: 0x040000B9 RID: 185
		private List<GenericMissionEventScript> _genericMissionEvents = new List<GenericMissionEventScript>();
	}
}
