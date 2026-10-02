using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003B2 RID: 946
	public class EventTriggeringUsableMachine : UsableMachine
	{
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060035F7 RID: 13815 RVA: 0x000DEC55 File Offset: 0x000DCE55
		public TextObject ActionText
		{
			get
			{
				return GameTexts.FindText(this.ActionTextId, null);
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060035F8 RID: 13816 RVA: 0x000DEC63 File Offset: 0x000DCE63
		public TextObject DescriptionText
		{
			get
			{
				return GameTexts.FindText(this.DescriptionTextId, null);
			}
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x000DEC74 File Offset: 0x000DCE74
		protected internal override void OnInit()
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

		// Token: 0x060035FA RID: 13818 RVA: 0x000DECE4 File Offset: 0x000DCEE4
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				if (base.StandingPoints[i].HasUser)
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

		// Token: 0x060035FB RID: 13819 RVA: 0x000DED88 File Offset: 0x000DCF88
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = GameTexts.FindText("str_key_action", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			textObject.SetTextVariable("ACTION", this.ActionText);
			return textObject;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000DEDD4 File Offset: 0x000DCFD4
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return this.DescriptionText;
		}

		// Token: 0x04001706 RID: 5894
		public string ActivatorAgentTags;

		// Token: 0x04001707 RID: 5895
		public string ActionTextId;

		// Token: 0x04001708 RID: 5896
		public string DescriptionTextId;

		// Token: 0x04001709 RID: 5897
		private List<GenericMissionEventScript> _genericMissionEvents = new List<GenericMissionEventScript>();
	}
}
