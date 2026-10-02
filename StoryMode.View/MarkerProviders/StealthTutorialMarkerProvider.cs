using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Missions.NameMarker.Targets;
using Storymode.Missions;
using TaleWorlds.MountAndBlade;

namespace StoryMode.View.MarkerProviders
{
	// Token: 0x02000008 RID: 8
	public class StealthTutorialMarkerProvider : MissionNameMarkerProvider
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002A55 File Offset: 0x00000C55
		private SneakIntoTheVillaMissionController Controller
		{
			get
			{
				if (this._controller == null)
				{
					Mission mission = Mission.Current;
					this._controller = ((mission != null) ? mission.GetMissionBehavior<SneakIntoTheVillaMissionController>() : null);
				}
				return this._controller;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A7C File Offset: 0x00000C7C
		public override void CreateMarkers(List<MissionNameMarkerTargetBaseVM> markers)
		{
			foreach (PassageUsePoint passageUsePoint in Mission.Current.ActiveMissionObjects.FindAllWithType<PassageUsePoint>().ToList<PassageUsePoint>())
			{
				if (passageUsePoint.IsMissionExit && !passageUsePoint.IsDeactivated)
				{
					markers.Add(new MissionPassageUsePointNameMarkerTargetVM(passageUsePoint));
				}
			}
			if (this.Controller != null)
			{
				markers.Add(new MissionAgentMarkerTargetVM(this.Controller.HeadmanAgent));
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B10 File Offset: 0x00000D10
		protected override void OnTick(float dt)
		{
			if (this.Controller == null)
			{
				return;
			}
			if (this.Controller.AreVisualsDirty)
			{
				base.SetMarkersDirty();
				this.Controller.AreVisualsDirty = false;
			}
		}

		// Token: 0x04000003 RID: 3
		private SneakIntoTheVillaMissionController _controller;
	}
}
