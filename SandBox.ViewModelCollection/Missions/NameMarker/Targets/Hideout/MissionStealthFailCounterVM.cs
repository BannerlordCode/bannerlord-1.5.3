using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout
{
	// Token: 0x0200003F RID: 63
	public class MissionStealthFailCounterVM : ViewModel
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x00011B83 File Offset: 0x0000FD83
		public MissionStealthFailCounterVM()
		{
			this._countDownTextObject = new TextObject("{=pY8lnL11}Mission will fail in: {SEC}", null);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00011B9C File Offset: 0x0000FD9C
		public void UpdateFailCounter(float failCounterElapsedTime, float failCounterMaxTime, bool isStealthFailCounterMissionLogicActive)
		{
			this.IsCounterActive = !BannerlordConfig.HideBattleUI && !MBCommon.IsPaused && isStealthFailCounterMissionLogicActive && failCounterElapsedTime > 0f;
			this.FailCounterMaxTime = failCounterMaxTime;
			if (this.IsCounterActive)
			{
				this.FailCounterElapsedTime = this.FailCounterMaxTime - failCounterElapsedTime;
				this._countDownTextObject.SetTextVariable("SEC", MathF.Ceiling(this.FailCounterElapsedTime));
				this.CountDownText = this._countDownTextObject.ToString();
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00011C1A File Offset: 0x0000FE1A
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00011C22 File Offset: 0x0000FE22
		[DataSourceProperty]
		public string CountDownText
		{
			get
			{
				return this._countDownText;
			}
			set
			{
				if (value != this._countDownText)
				{
					this._countDownText = value;
					base.OnPropertyChangedWithValue<string>(value, "CountDownText");
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00011C45 File Offset: 0x0000FE45
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00011C4D File Offset: 0x0000FE4D
		[DataSourceProperty]
		public float FailCounterElapsedTime
		{
			get
			{
				return this._failCounterElapsedTime;
			}
			set
			{
				if (value != this._failCounterElapsedTime)
				{
					this._failCounterElapsedTime = value;
					base.OnPropertyChangedWithValue(value, "FailCounterElapsedTime");
				}
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00011C6B File Offset: 0x0000FE6B
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00011C73 File Offset: 0x0000FE73
		[DataSourceProperty]
		public float FailCounterMaxTime
		{
			get
			{
				return this._failCounterMaxTime;
			}
			set
			{
				if (value != this._failCounterMaxTime)
				{
					this._failCounterMaxTime = value;
					base.OnPropertyChangedWithValue(value, "FailCounterMaxTime");
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00011C91 File Offset: 0x0000FE91
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00011C99 File Offset: 0x0000FE99
		[DataSourceProperty]
		public bool IsCounterActive
		{
			get
			{
				return this._isCounterActive;
			}
			set
			{
				if (value != this._isCounterActive)
				{
					this._isCounterActive = value;
					base.OnPropertyChangedWithValue(value, "IsCounterActive");
				}
			}
		}

		// Token: 0x0400022C RID: 556
		private TextObject _countDownTextObject;

		// Token: 0x0400022D RID: 557
		private float _failCounterElapsedTime;

		// Token: 0x0400022E RID: 558
		private string _countDownText;

		// Token: 0x0400022F RID: 559
		private float _failCounterMaxTime;

		// Token: 0x04000230 RID: 560
		private bool _isCounterActive;
	}
}
