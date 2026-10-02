using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective
{
	// Token: 0x0200003C RID: 60
	public class MissionObjectiveMarkerVM : ViewModel
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x0001464E File Offset: 0x0001284E
		public MissionObjectiveMarkerVM(MissionObjectiveTarget target)
		{
			this.Target = target;
			this.IsEnabled = true;
			this.IsActive = true;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001466B File Offset: 0x0001286B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ObjectiveName = this.Target.GetName().ToString();
			this.ObjectiveTypeId = "ActiveQuest";
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00014694 File Offset: 0x00012894
		public void UpdateActiveState()
		{
			this.IsActive = this.Target.IsActive();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000146A8 File Offset: 0x000128A8
		public void UpdatePosition(Camera missionCamera)
		{
			Vec3 globalPosition = this.Target.GetGlobalPosition();
			float num = -100f;
			float num2 = -100f;
			float num3 = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, globalPosition, ref num, ref num2, ref num3);
			if (num3 >= 0f)
			{
				this.ScreenPosition = new Vec2(num, num2);
				this.Distance = (int)(globalPosition - missionCamera.Position).Length;
				return;
			}
			this.Distance = -1;
			this.ScreenPosition = new Vec2(-5000f, -5000f);
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001472F File Offset: 0x0001292F
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00014737 File Offset: 0x00012937
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00014755 File Offset: 0x00012955
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001475D File Offset: 0x0001295D
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001477B File Offset: 0x0001297B
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00014783 File Offset: 0x00012983
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000147A1 File Offset: 0x000129A1
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x000147A9 File Offset: 0x000129A9
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value != this._screenPosition)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x000147CC File Offset: 0x000129CC
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x000147D4 File Offset: 0x000129D4
		[DataSourceProperty]
		public string ObjectiveTypeId
		{
			get
			{
				return this._objectiveTypeId;
			}
			set
			{
				if (value != this._objectiveTypeId)
				{
					this._objectiveTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveTypeId");
				}
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000147F7 File Offset: 0x000129F7
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x000147FF File Offset: 0x000129FF
		[DataSourceProperty]
		public string ObjectiveName
		{
			get
			{
				return this._objectiveName;
			}
			set
			{
				if (value != this._objectiveName)
				{
					this._objectiveName = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveName");
				}
			}
		}

		// Token: 0x04000267 RID: 615
		public readonly MissionObjectiveTarget Target;

		// Token: 0x04000268 RID: 616
		private int _distance;

		// Token: 0x04000269 RID: 617
		private bool _isEnabled;

		// Token: 0x0400026A RID: 618
		private bool _isActive;

		// Token: 0x0400026B RID: 619
		private Vec2 _screenPosition;

		// Token: 0x0400026C RID: 620
		private string _objectiveTypeId;

		// Token: 0x0400026D RID: 621
		private string _objectiveName;
	}
}
