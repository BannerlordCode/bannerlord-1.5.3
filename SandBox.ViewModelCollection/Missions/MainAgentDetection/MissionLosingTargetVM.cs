using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000044 RID: 68
	public class MissionLosingTargetVM : ViewModel
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x000122B3 File Offset: 0x000104B3
		public MissionLosingTargetVM()
		{
			this.RefreshValues();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000122C1 File Offset: 0x000104C1
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.LosingTargetWarningText = new TextObject("{=kXy4R7ca}You are about to lose the target.", null).ToString();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000122DF File Offset: 0x000104DF
		public void UpdateLosingTargetValues(bool isLosingTarget, float losingTargetTimer, float losingTargetTreshold)
		{
			this.IsLosingTarget = isLosingTarget;
			this.LosingTargetRatio = MathF.Clamp(losingTargetTimer / losingTargetTreshold * 100f, 0f, 100f);
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00012306 File Offset: 0x00010506
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0001230E File Offset: 0x0001050E
		[DataSourceProperty]
		public bool IsLosingTarget
		{
			get
			{
				return this._isLosingTarget;
			}
			set
			{
				if (value != this._isLosingTarget)
				{
					this._isLosingTarget = value;
					base.OnPropertyChangedWithValue(value, "IsLosingTarget");
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001232C File Offset: 0x0001052C
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x00012334 File Offset: 0x00010534
		[DataSourceProperty]
		public float LosingTargetRatio
		{
			get
			{
				return this._losingTargetRatio;
			}
			set
			{
				if (value != this._losingTargetRatio)
				{
					this._losingTargetRatio = value;
					base.OnPropertyChangedWithValue(value, "LosingTargetRatio");
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00012352 File Offset: 0x00010552
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x0001235A File Offset: 0x0001055A
		[DataSourceProperty]
		public string LosingTargetWarningText
		{
			get
			{
				return this._losingTargetWarningText;
			}
			set
			{
				if (value != this._losingTargetWarningText)
				{
					this._losingTargetWarningText = value;
					base.OnPropertyChangedWithValue<string>(value, "LosingTargetWarningText");
				}
			}
		}

		// Token: 0x04000247 RID: 583
		private bool _isLosingTarget;

		// Token: 0x04000248 RID: 584
		private float _losingTargetRatio;

		// Token: 0x04000249 RID: 585
		private string _losingTargetWarningText;
	}
}
