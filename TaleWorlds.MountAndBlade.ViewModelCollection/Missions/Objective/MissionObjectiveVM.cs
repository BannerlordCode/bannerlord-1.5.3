using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective
{
	// Token: 0x0200003E RID: 62
	public class MissionObjectiveVM : ViewModel
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x00014A24 File Offset: 0x00012C24
		public MissionObjectiveVM(MissionObjectiveLogic objectiveLogic, Camera missionCamera)
		{
			this.Markers = new MissionObjectiveMarkersVM(objectiveLogic, missionCamera);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00014A3C File Offset: 0x00012C3C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._currentObjective != null)
			{
				this.Title = this._currentObjective.Name.ToString();
				this.Description = this._currentObjective.Description.ToString();
			}
			this.Markers.RefreshValues();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00014A90 File Offset: 0x00012C90
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._currentObjective != null)
			{
				this._currentObjective.OnUpdated -= this.OnObjectiveUpdated;
			}
			CharacterImageIdentifierVM objectiveGiverIdentifier = this.ObjectiveGiverIdentifier;
			if (objectiveGiverIdentifier != null)
			{
				objectiveGiverIdentifier.OnFinalize();
			}
			this.Markers.OnFinalize();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00014AE0 File Offset: 0x00012CE0
		public void UpdateObjective(MissionObjective objective)
		{
			if (this._currentObjective != null)
			{
				this._currentObjective.OnUpdated -= this.OnObjectiveUpdated;
			}
			this._currentObjective = objective;
			if (this._currentObjective != null)
			{
				this._currentObjective.OnUpdated += this.OnObjectiveUpdated;
			}
			this.RefreshCurrentObjectiveData();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00014B38 File Offset: 0x00012D38
		public void Tick(float dt)
		{
			if (this._isCurrentObjectiveDirty)
			{
				this.RefreshCurrentObjectiveData();
				this._isCurrentObjectiveDirty = false;
			}
			this.Markers.Tick(dt);
			if (this._currentObjective != null)
			{
				MissionObjectiveProgressInfo currentProgress = this._currentObjective.GetCurrentProgress();
				if (this.IsProgressDirty(in currentProgress))
				{
					this.HasProgress = currentProgress.HasProgress;
					this.CurrentProgress = currentProgress.CurrentProgressAmount;
					this.RequiredProgress = currentProgress.RequiredProgressAmount;
					this.ProgressText = GameTexts.FindText("str_LEFT_over_RIGHT_no_space", null).SetTextVariable("LEFT", this.CurrentProgress).SetTextVariable("RIGHT", this.RequiredProgress)
						.ToString();
				}
			}
			this.UpdateObjectiveGiver();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00014BE5 File Offset: 0x00012DE5
		private void RefreshCurrentObjectiveData()
		{
			this.Markers.UpdateObjective(this._currentObjective);
			this.IsEnabled = this._currentObjective != null;
			this.RefreshValues();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00014C10 File Offset: 0x00012E10
		private void UpdateObjectiveGiver()
		{
			BasicCharacterObject currentObjectiveGiver = this._currentObjectiveGiver;
			MissionObjective currentObjective = this._currentObjective;
			if (currentObjectiveGiver == ((currentObjective != null) ? currentObjective.ObjectiveGiver : null))
			{
				return;
			}
			MissionObjective currentObjective2 = this._currentObjective;
			this._currentObjectiveGiver = ((currentObjective2 != null) ? currentObjective2.ObjectiveGiver : null);
			this.HasObjectiveGiver = this._currentObjectiveGiver != null;
			CharacterImageIdentifierVM objectiveGiverIdentifier = this.ObjectiveGiverIdentifier;
			if (objectiveGiverIdentifier != null)
			{
				objectiveGiverIdentifier.OnFinalize();
			}
			if (this.HasObjectiveGiver)
			{
				this.ObjectiveGiverIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this._currentObjectiveGiver));
			}
			BasicCharacterObject currentObjectiveGiver2 = this._currentObjectiveGiver;
			string text;
			if (currentObjectiveGiver2 == null)
			{
				text = null;
			}
			else
			{
				TextObject name = currentObjectiveGiver2.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			this.ObjectiveGiverName = text ?? string.Empty;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00014CBC File Offset: 0x00012EBC
		private bool IsProgressDirty(in MissionObjectiveProgressInfo progressInfo)
		{
			MissionObjectiveProgressInfo missionObjectiveProgressInfo = progressInfo;
			return missionObjectiveProgressInfo.HasProgress != this.HasProgress || progressInfo.CurrentProgressAmount != this.CurrentProgress || progressInfo.RequiredProgressAmount != this.RequiredProgress;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00014D00 File Offset: 0x00012F00
		private void OnObjectiveUpdated()
		{
			this._isCurrentObjectiveDirty = true;
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00014D09 File Offset: 0x00012F09
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x00014D11 File Offset: 0x00012F11
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (this._title != value)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
					this.HasTitle = !string.IsNullOrEmpty(value);
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00014D43 File Offset: 0x00012F43
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x00014D4B File Offset: 0x00012F4B
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (this._description != value)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
					this.HasDescription = !string.IsNullOrEmpty(value);
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00014D7D File Offset: 0x00012F7D
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x00014D85 File Offset: 0x00012F85
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (this._progressText != value)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x00014DA8 File Offset: 0x00012FA8
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x00014DB0 File Offset: 0x00012FB0
		[DataSourceProperty]
		public string ObjectiveGiverName
		{
			get
			{
				return this._objectiveGiverName;
			}
			set
			{
				if (this._objectiveGiverName != value)
				{
					this._objectiveGiverName = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveGiverName");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00014DD3 File Offset: 0x00012FD3
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x00014DDB File Offset: 0x00012FDB
		[DataSourceProperty]
		public bool HasObjectiveGiver
		{
			get
			{
				return this._hasObjectiveGiver;
			}
			set
			{
				if (this._hasObjectiveGiver != value)
				{
					this._hasObjectiveGiver = value;
					base.OnPropertyChangedWithValue(value, "HasObjectiveGiver");
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00014DF9 File Offset: 0x00012FF9
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00014E01 File Offset: 0x00013001
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.Markers.IsEnabled = value;
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00014E2B File Offset: 0x0001302B
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00014E33 File Offset: 0x00013033
		[DataSourceProperty]
		public bool HasTitle
		{
			get
			{
				return this._hasTitle;
			}
			set
			{
				if (this._hasTitle != value)
				{
					this._hasTitle = value;
					base.OnPropertyChangedWithValue(value, "HasTitle");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00014E51 File Offset: 0x00013051
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00014E59 File Offset: 0x00013059
		[DataSourceProperty]
		public bool HasDescription
		{
			get
			{
				return this._hasDescription;
			}
			set
			{
				if (this._hasDescription != value)
				{
					this._hasDescription = value;
					base.OnPropertyChangedWithValue(value, "HasDescription");
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00014E77 File Offset: 0x00013077
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00014E7F File Offset: 0x0001307F
		[DataSourceProperty]
		public bool HasProgress
		{
			get
			{
				return this._hasProgress;
			}
			set
			{
				if (this._hasProgress != value)
				{
					this._hasProgress = value;
					base.OnPropertyChangedWithValue(value, "HasProgress");
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00014E9D File Offset: 0x0001309D
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00014EA5 File Offset: 0x000130A5
		[DataSourceProperty]
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (this._currentProgress != value)
				{
					this._currentProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProgress");
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00014EC3 File Offset: 0x000130C3
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00014ECB File Offset: 0x000130CB
		[DataSourceProperty]
		public int RequiredProgress
		{
			get
			{
				return this._requiredProgress;
			}
			set
			{
				if (this._requiredProgress != value)
				{
					this._requiredProgress = value;
					base.OnPropertyChangedWithValue(value, "RequiredProgress");
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00014EE9 File Offset: 0x000130E9
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00014EF1 File Offset: 0x000130F1
		[DataSourceProperty]
		public CharacterImageIdentifierVM ObjectiveGiverIdentifier
		{
			get
			{
				return this._objectiveGiverIdentifier;
			}
			set
			{
				if (this._objectiveGiverIdentifier != value)
				{
					this._objectiveGiverIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ObjectiveGiverIdentifier");
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00014F0F File Offset: 0x0001310F
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00014F17 File Offset: 0x00013117
		[DataSourceProperty]
		public MissionObjectiveMarkersVM Markers
		{
			get
			{
				return this._markers;
			}
			set
			{
				if (this._markers != value)
				{
					this._markers = value;
					base.OnPropertyChangedWithValue<MissionObjectiveMarkersVM>(value, "Markers");
				}
			}
		}

		// Token: 0x04000274 RID: 628
		private MissionObjective _currentObjective;

		// Token: 0x04000275 RID: 629
		private BasicCharacterObject _currentObjectiveGiver;

		// Token: 0x04000276 RID: 630
		private bool _isCurrentObjectiveDirty;

		// Token: 0x04000277 RID: 631
		private string _title;

		// Token: 0x04000278 RID: 632
		private string _description;

		// Token: 0x04000279 RID: 633
		private string _progressText;

		// Token: 0x0400027A RID: 634
		private string _objectiveGiverName;

		// Token: 0x0400027B RID: 635
		private bool _hasObjectiveGiver;

		// Token: 0x0400027C RID: 636
		private bool _isEnabled;

		// Token: 0x0400027D RID: 637
		private bool _hasTitle;

		// Token: 0x0400027E RID: 638
		private bool _hasDescription;

		// Token: 0x0400027F RID: 639
		private bool _hasProgress;

		// Token: 0x04000280 RID: 640
		private int _currentProgress;

		// Token: 0x04000281 RID: 641
		private int _requiredProgress;

		// Token: 0x04000282 RID: 642
		private CharacterImageIdentifierVM _objectiveGiverIdentifier;

		// Token: 0x04000283 RID: 643
		private MissionObjectiveMarkersVM _markers;
	}
}
