using System;
using System.Linq;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000016 RID: 22
	[Tutorial("SeeMarkersInMissionTutorial")]
	public class SeeMarkersInMissionTutorial : TutorialItemBase
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00002BF3 File Offset: 0x00000DF3
		public SeeMarkersInMissionTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Left;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = false;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002C14 File Offset: 0x00000E14
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerEnabledNameMarkers;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002C1C File Offset: 0x00000E1C
		public override void OnMissionNameMarkerToggled(MissionNameMarkerToggleEvent obj)
		{
			this._playerEnabledNameMarkers = obj.NewState;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002C2A File Offset: 0x00000E2A
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002C30 File Offset: 0x00000E30
		public override bool IsConditionsMetForActivation()
		{
			string[] array = new string[] { "center", "lordshall", "tavern", "prison", "village_center" };
			return TutorialHelper.PlayerIsInAnySettlement && TutorialHelper.CurrentContext == TutorialContexts.Mission && TutorialHelper.CurrentMissionLocation != null && array.Contains(TutorialHelper.CurrentMissionLocation.StringId) && !TutorialHelper.PlayerIsInAConversation;
		}

		// Token: 0x04000019 RID: 25
		private bool _playerEnabledNameMarkers;
	}
}
