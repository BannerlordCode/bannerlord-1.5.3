using System;
using SandBox.Missions.MissionLogics;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000022 RID: 34
	[OverrideView(typeof(MissionQuestBarView))]
	public class MissionGauntletQuestBarView : MissionQuestBarView
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0000C158 File Offset: 0x0000A358
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MissionQuestBarVM();
			this._gauntletLayer = new GauntletLayer("MissionQuestBar", 10, false);
			this._gauntletLayer.LoadMovie("MissionQuestBar", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			foreach (MissionBehavior missionBehavior in base.Mission.MissionBehaviors)
			{
				if (missionBehavior is IMissionProgressTracker)
				{
					this._missionProgressTracker = missionBehavior as IMissionProgressTracker;
					break;
				}
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000C20C File Offset: 0x0000A40C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._dataSource.OnFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource = null;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C23E File Offset: 0x0000A43E
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._missionProgressTracker != null)
			{
				this._dataSource.UpdateQuestValues(0f, 1f, this._missionProgressTracker.CurrentProgress);
			}
		}

		// Token: 0x04000093 RID: 147
		private const float MinProgressValue = 0f;

		// Token: 0x04000094 RID: 148
		private const float MaxProgressValue = 1f;

		// Token: 0x04000095 RID: 149
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000096 RID: 150
		private MissionQuestBarVM _dataSource;

		// Token: 0x04000097 RID: 151
		private IMissionProgressTracker _missionProgressTracker;
	}
}
