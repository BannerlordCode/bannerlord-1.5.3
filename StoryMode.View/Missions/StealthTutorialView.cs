using System;
using SandBox.ViewModelCollection.Missions.NameMarker;
using StoryMode.View.MarkerProviders;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace StoryMode.View.Missions
{
	// Token: 0x02000006 RID: 6
	public class StealthTutorialView : MissionView
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000027D3 File Offset: 0x000009D3
		public override void AfterStart()
		{
			MissionNameMarkerFactory.PushContext("StealthTutorialContext", false).AddProvider<StealthTutorialMarkerProvider>();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027E5 File Offset: 0x000009E5
		protected override void OnEndMission()
		{
			MissionNameMarkerFactory.PopContext("StealthTutorialContext");
		}
	}
}
