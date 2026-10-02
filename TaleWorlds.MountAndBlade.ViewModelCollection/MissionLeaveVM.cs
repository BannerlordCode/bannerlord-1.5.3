using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x02000009 RID: 9
	public class MissionLeaveVM : ViewModel
	{
		// Token: 0x06000085 RID: 133 RVA: 0x000036E1 File Offset: 0x000018E1
		public MissionLeaveVM(Func<float> getMissionEndTimer, Func<float> getMissionEndTimeInSeconds)
		{
			this._getMissionEndTimer = getMissionEndTimer;
			this._getMissionEndTimeInSeconds = getMissionEndTimeInSeconds;
			this.RefreshValues();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000036FD File Offset: 0x000018FD
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.LeaveText = GameTexts.FindText("str_leaving", null).ToString();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000371B File Offset: 0x0000191B
		public void Tick(float dt)
		{
			this.CurrentTime = this._getMissionEndTimer();
			this.MaxTime = this._getMissionEndTimeInSeconds();
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000373F File Offset: 0x0000193F
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003747 File Offset: 0x00001947
		[DataSourceProperty]
		public string LeaveText
		{
			get
			{
				return this._leaveText;
			}
			set
			{
				if (value != this._leaveText)
				{
					this._leaveText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaveText");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000376A File Offset: 0x0000196A
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003772 File Offset: 0x00001972
		[DataSourceProperty]
		public float MaxTime
		{
			get
			{
				return this._maxTime;
			}
			set
			{
				if (value != this._maxTime)
				{
					this._maxTime = value;
					base.OnPropertyChangedWithValue(value, "MaxTime");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003790 File Offset: 0x00001990
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00003798 File Offset: 0x00001998
		[DataSourceProperty]
		public float CurrentTime
		{
			get
			{
				return this._currentTime;
			}
			set
			{
				if (value != this._currentTime)
				{
					this._currentTime = value;
					base.OnPropertyChangedWithValue(value, "CurrentTime");
				}
			}
		}

		// Token: 0x04000039 RID: 57
		private Func<float> _getMissionEndTimer;

		// Token: 0x0400003A RID: 58
		private Func<float> _getMissionEndTimeInSeconds;

		// Token: 0x0400003B RID: 59
		private float _maxTime;

		// Token: 0x0400003C RID: 60
		private float _currentTime;

		// Token: 0x0400003D RID: 61
		private string _leaveText;
	}
}
