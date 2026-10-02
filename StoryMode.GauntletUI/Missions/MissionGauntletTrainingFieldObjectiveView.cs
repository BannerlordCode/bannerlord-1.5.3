using System;
using System.Collections.Generic;
using StoryMode.Missions;
using StoryMode.View.Missions;
using StoryMode.ViewModelCollection.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace StoryMode.GauntletUI.Missions
{
	// Token: 0x0200004C RID: 76
	[OverrideView(typeof(MissionTrainingFieldObjectiveView))]
	public class MissionGauntletTrainingFieldObjectiveView : MissionView
	{
		// Token: 0x06000169 RID: 361 RVA: 0x0000493C File Offset: 0x00002B3C
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			TrainingFieldMissionController missionBehavior = base.Mission.GetMissionBehavior<TrainingFieldMissionController>();
			this._dataSource = new TrainingFieldObjectivesVM();
			this._dataSource.UpdateCurrentObjectiveExplanationText(missionBehavior.InitialCurrentObjective);
			this._layer = new GauntletLayer("TrainingFieldObjectives", 2, false);
			this._layer.LoadMovie("TrainingFieldObjectives", this._dataSource);
			base.MissionScreen.AddLayer(this._layer);
			missionBehavior.TimerTick = new Action<string>(this._dataSource.UpdateTimerText);
			missionBehavior.CurrentObjectiveTick = new Action<TextObject>(this._dataSource.UpdateCurrentObjectiveExplanationText);
			missionBehavior.AllObjectivesTick = new Action<List<TrainingFieldMissionController.TutorialObjective>>(this._dataSource.UpdateObjectivesWith);
			missionBehavior.UIStartTimer = new Action(this.BeginTimer);
			missionBehavior.UIEndTimer = new Func<float>(this.EndTimer);
			missionBehavior.CurrentMouseObjectiveTick = new Action<TrainingFieldMissionController.MouseObjectives, TrainingFieldMissionController.ObjectivePerformingType>(this._dataSource.UpdateCurrentMouseObjective);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004A34 File Offset: 0x00002C34
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isTimerActive)
			{
				this._dataSource.UpdateTimerText((base.Mission.CurrentTime - this._beginningTime).ToString("0.0"));
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004A7A File Offset: 0x00002C7A
		private void BeginTimer()
		{
			this._isTimerActive = true;
			this._beginningTime = base.Mission.CurrentTime;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004A94 File Offset: 0x00002C94
		private float EndTimer()
		{
			this._isTimerActive = false;
			this._dataSource.UpdateTimerText("");
			return base.Mission.CurrentTime - this._beginningTime;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00004ABF File Offset: 0x00002CBF
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._layer);
			this._dataSource = null;
			this._layer = null;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00004AE6 File Offset: 0x00002CE6
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._layer != null)
			{
				this._layer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004B0B File Offset: 0x00002D0B
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._layer != null)
			{
				this._layer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000060 RID: 96
		private TrainingFieldObjectivesVM _dataSource;

		// Token: 0x04000061 RID: 97
		private GauntletLayer _layer;

		// Token: 0x04000062 RID: 98
		private float _beginningTime;

		// Token: 0x04000063 RID: 99
		private bool _isTimerActive;
	}
}
