using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.Objects.Usables;

namespace SandBox.Missions
{
	// Token: 0x0200005F RID: 95
	public class SabotageMissionController : MissionLogic
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x00015CCD File Offset: 0x00013ECD
		public SabotageMissionController()
		{
			Game.Current.EventManager.RegisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00015CFB File Offset: 0x00013EFB
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00015D18 File Offset: 0x00013F18
		private void OnGenericMissionEventTriggered(GenericMissionEvent missionEvent)
		{
			if (missionEvent.EventId == "sabotage_objective_used_event")
			{
				string[] array = missionEvent.Parameter.Split(new char[] { ' ' });
				SandBoxHelpers.MissionHelper.DisableGenericMissionEventScript(array[0], missionEvent);
				EventTriggeringUsableMachine firstScriptOfType = Mission.Current.Scene.FindEntityWithTag(array[0]).GetFirstScriptOfType<EventTriggeringUsableMachine>();
				for (int i = 0; i < firstScriptOfType.StandingPoints.Count; i++)
				{
					if (firstScriptOfType.StandingPoints[i].HasUser)
					{
						firstScriptOfType.StandingPoints[i].UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				this.OnSabotageObjectiveUsed(firstScriptOfType, array[1]);
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00015DC0 File Offset: 0x00013FC0
		public override void AfterStart()
		{
			Mission.Current.SetMissionMode(MissionMode.Stealth, true);
			SandBoxHelpers.MissionHelper.SpawnPlayer(false, true, false, false, "");
			Mission.Current.GetMissionBehavior<MissionAgentHandler>().SpawnLocationCharacters(null);
			foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("sabotage_objective"))
			{
				EventTriggeringUsableMachine firstScriptOfType = gameEntity.GetFirstScriptOfType<EventTriggeringUsableMachine>();
				this._sabotageObjectives.Add(firstScriptOfType);
			}
			this._allSabotageObjectivesCount = this._sabotageObjectives.Count;
			this._missionExitBarrier = Mission.Current.Scene.FindEntityWithTag("sabotage_mission_exit_barrier");
			this._missionExitBarrier.SetVisibilityExcludeParents(false);
			this._missionExitArea = Mission.Current.Scene.FindEntityWithTag("sabotage_mission_exit_area").GetFirstScriptOfType<BasicAreaIndicator>();
			this._missionExitArea.SetIsActive(false);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00015EB4 File Offset: 0x000140B4
		private void OnSabotageObjectiveUsed(EventTriggeringUsableMachine eventTriggeringUsableMachine, string eventDescriptionTextId)
		{
			MBInformationManager.AddQuickInformation(GameTexts.FindText(eventDescriptionTextId, null), 0, null, null, "");
			eventTriggeringUsableMachine.SetDisabled(true);
			this._usedSabotageObjectivesCount++;
			if (this._usedSabotageObjectivesCount >= this._allSabotageObjectivesCount)
			{
				this._missionExitBarrier.SetVisibilityExcludeParents(true);
				this._missionExitArea.SetIsActive(true);
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00015F10 File Offset: 0x00014110
		public override void OnMissionTick(float dt)
		{
		}

		// Token: 0x040001F4 RID: 500
		private const string SabotageObjectiveTag = "sabotage_objective";

		// Token: 0x040001F5 RID: 501
		private const string SabotageMissionExitBarrierTag = "sabotage_mission_exit_barrier";

		// Token: 0x040001F6 RID: 502
		private const string SabotageMissionExitAreaTag = "sabotage_mission_exit_area";

		// Token: 0x040001F7 RID: 503
		private const string SabotageObjectiveUsedEventId = "sabotage_objective_used_event";

		// Token: 0x040001F8 RID: 504
		private readonly List<EventTriggeringUsableMachine> _sabotageObjectives = new List<EventTriggeringUsableMachine>();

		// Token: 0x040001F9 RID: 505
		private GameEntity _missionExitBarrier;

		// Token: 0x040001FA RID: 506
		private BasicAreaIndicator _missionExitArea;

		// Token: 0x040001FB RID: 507
		private int _allSabotageObjectivesCount;

		// Token: 0x040001FC RID: 508
		private int _usedSabotageObjectivesCount;
	}
}
