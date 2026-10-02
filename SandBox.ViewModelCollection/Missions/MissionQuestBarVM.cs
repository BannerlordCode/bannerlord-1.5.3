using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002E RID: 46
	public class MissionQuestBarVM : ViewModel
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x0001063C File Offset: 0x0000E83C
		public void UpdateQuestValues(float minDetectionLevel, float maxDetectionLevel, float currentDetectionLevel)
		{
			this.MinimumQuestLevel = minDetectionLevel;
			this.MaximumQuestLevel = maxDetectionLevel;
			this.CurrentQuestLevel = currentDetectionLevel;
			this.CurrentQuestLevelRatio = MBMath.InverseLerp(this.MinimumQuestLevel, this.MaximumQuestLevel, this.CurrentQuestLevel);
			this.HasQuestLevel = this.CurrentQuestLevel > 0f;
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001068E File Offset: 0x0000E88E
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x00010696 File Offset: 0x0000E896
		[DataSourceProperty]
		public bool HasQuestLevel
		{
			get
			{
				return this._hasQuestLevel;
			}
			set
			{
				if (value != this._hasQuestLevel)
				{
					this._hasQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "HasQuestLevel");
				}
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x000106B4 File Offset: 0x0000E8B4
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x000106BC File Offset: 0x0000E8BC
		[DataSourceProperty]
		public float MinimumQuestLevel
		{
			get
			{
				return this._minimumQuestLevel;
			}
			set
			{
				if (value != this._minimumQuestLevel)
				{
					this._minimumQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "MinimumQuestLevel");
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000106DA File Offset: 0x0000E8DA
		// (set) Token: 0x060003CA RID: 970 RVA: 0x000106E2 File Offset: 0x0000E8E2
		[DataSourceProperty]
		public float MaximumQuestLevel
		{
			get
			{
				return this._maximumQuestLevel;
			}
			set
			{
				if (value != this._maximumQuestLevel)
				{
					this._maximumQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "MaximumQuestLevel");
				}
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00010700 File Offset: 0x0000E900
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00010708 File Offset: 0x0000E908
		[DataSourceProperty]
		public float CurrentQuestLevel
		{
			get
			{
				return this._currentQuestLevel;
			}
			set
			{
				if (value != this._currentQuestLevel)
				{
					this._currentQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentQuestLevel");
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00010726 File Offset: 0x0000E926
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0001072E File Offset: 0x0000E92E
		[DataSourceProperty]
		public float CurrentQuestLevelRatio
		{
			get
			{
				return this._currentQuestLevelRatio;
			}
			set
			{
				if (value != this._currentQuestLevelRatio)
				{
					this._currentQuestLevelRatio = value;
					base.OnPropertyChangedWithValue(value, "CurrentQuestLevelRatio");
				}
			}
		}

		// Token: 0x040001EF RID: 495
		private bool _hasQuestLevel;

		// Token: 0x040001F0 RID: 496
		private float _minimumQuestLevel;

		// Token: 0x040001F1 RID: 497
		private float _maximumQuestLevel;

		// Token: 0x040001F2 RID: 498
		private float _currentQuestLevel;

		// Token: 0x040001F3 RID: 499
		private float _currentQuestLevelRatio;
	}
}
