using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000041 RID: 65
	public class MainAgentDetectionVM : ViewModel
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x00011D16 File Offset: 0x0000FF16
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SuspicionFullText = new TextObject("{=KgTFCWG8}You are suspicious", null).ToString();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00011D34 File Offset: 0x0000FF34
		public void UpdateDetectionValues(float minDetectionLevel, float maxDetectionLevel, float currentDetectionLevel)
		{
			this.MinimumDetectionLevel = minDetectionLevel;
			this.MaximumDetectionLevel = maxDetectionLevel;
			this.CurrentDetectionLevel = currentDetectionLevel;
			this.CurrentDetectionLevelRatio = MBMath.InverseLerp(this.MinimumDetectionLevel, this.MaximumDetectionLevel, this.CurrentDetectionLevel);
			this.HasDetection = this.CurrentDetectionLevel > 0f;
			this.HasReachedSuspicionTreshold = this.CurrentDetectionLevel >= this.MaximumDetectionLevel;
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00011D9D File Offset: 0x0000FF9D
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00011DA5 File Offset: 0x0000FFA5
		[DataSourceProperty]
		public bool HasDetection
		{
			get
			{
				return this._hasDetection;
			}
			set
			{
				if (value != this._hasDetection)
				{
					this._hasDetection = value;
					base.OnPropertyChangedWithValue(value, "HasDetection");
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00011DC3 File Offset: 0x0000FFC3
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00011DCB File Offset: 0x0000FFCB
		[DataSourceProperty]
		public bool HasReachedSuspicionTreshold
		{
			get
			{
				return this._hasReachedSuspicionTreshold;
			}
			set
			{
				if (value != this._hasReachedSuspicionTreshold)
				{
					this._hasReachedSuspicionTreshold = value;
					base.OnPropertyChangedWithValue(value, "HasReachedSuspicionTreshold");
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00011DE9 File Offset: 0x0000FFE9
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00011DF1 File Offset: 0x0000FFF1
		[DataSourceProperty]
		public float MinimumDetectionLevel
		{
			get
			{
				return this._minimumDetectionLevel;
			}
			set
			{
				if (value != this._minimumDetectionLevel)
				{
					this._minimumDetectionLevel = value;
					base.OnPropertyChangedWithValue(value, "MinimumDetectionLevel");
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00011E0F File Offset: 0x0001000F
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00011E17 File Offset: 0x00010017
		[DataSourceProperty]
		public float MaximumDetectionLevel
		{
			get
			{
				return this._maximumDetectionLevel;
			}
			set
			{
				if (value != this._maximumDetectionLevel)
				{
					this._maximumDetectionLevel = value;
					base.OnPropertyChangedWithValue(value, "MaximumDetectionLevel");
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00011E35 File Offset: 0x00010035
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00011E3D File Offset: 0x0001003D
		[DataSourceProperty]
		public float CurrentDetectionLevel
		{
			get
			{
				return this._currentDetectionLevel;
			}
			set
			{
				if (value != this._currentDetectionLevel)
				{
					this._currentDetectionLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentDetectionLevel");
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00011E5B File Offset: 0x0001005B
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00011E63 File Offset: 0x00010063
		[DataSourceProperty]
		public float CurrentDetectionLevelRatio
		{
			get
			{
				return this._currentDetectionLevelRatio;
			}
			set
			{
				if (value != this._currentDetectionLevelRatio)
				{
					this._currentDetectionLevelRatio = value;
					base.OnPropertyChangedWithValue(value, "CurrentDetectionLevelRatio");
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00011E81 File Offset: 0x00010081
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00011E89 File Offset: 0x00010089
		[DataSourceProperty]
		public string SuspicionFullText
		{
			get
			{
				return this._suspicionFullText;
			}
			set
			{
				if (value != this._suspicionFullText)
				{
					this._suspicionFullText = value;
					base.OnPropertyChangedWithValue<string>(value, "SuspicionFullText");
				}
			}
		}

		// Token: 0x04000231 RID: 561
		private bool _hasDetection;

		// Token: 0x04000232 RID: 562
		private bool _hasReachedSuspicionTreshold;

		// Token: 0x04000233 RID: 563
		private float _minimumDetectionLevel;

		// Token: 0x04000234 RID: 564
		private float _maximumDetectionLevel;

		// Token: 0x04000235 RID: 565
		private float _currentDetectionLevel;

		// Token: 0x04000236 RID: 566
		private float _currentDetectionLevelRatio;

		// Token: 0x04000237 RID: 567
		private string _suspicionFullText;
	}
}
