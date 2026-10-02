using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Missions.MissionEvents
{
	// Token: 0x020000A0 RID: 160
	public class ShowQuickInformationEventListenerLogic : MissionLogic
	{
		// Token: 0x060006A7 RID: 1703 RVA: 0x0002CDE5 File Offset: 0x0002AFE5
		public ShowQuickInformationEventListenerLogic()
		{
			Game.Current.EventManager.RegisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002CE08 File Offset: 0x0002B008
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002CE28 File Offset: 0x0002B028
		private void OnGenericMissionEventTriggered(GenericMissionEvent missionEvent)
		{
			if (missionEvent.EventId == "show_quick_information_event")
			{
				string[] array = missionEvent.Parameter.Split(new char[] { ' ' });
				SandBoxHelpers.MissionHelper.DisableGenericMissionEventScript(array[0], missionEvent);
				MBInformationManager.AddQuickInformation(GameTexts.FindText(array[1], null), 0, null, null, "");
			}
		}

		// Token: 0x04000391 RID: 913
		private const string ShowQuickInformationEventId = "show_quick_information_event";
	}
}
