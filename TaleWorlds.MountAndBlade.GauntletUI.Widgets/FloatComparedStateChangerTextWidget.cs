using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001F RID: 31
	public class FloatComparedStateChangerTextWidget : TextWidget
	{
		// Token: 0x06000177 RID: 375 RVA: 0x00006265 File Offset: 0x00004465
		public FloatComparedStateChangerTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006270 File Offset: 0x00004470
		private void UpdateState()
		{
			if (string.IsNullOrEmpty(this.TrueState) || string.IsNullOrEmpty(this.FalseState))
			{
				return;
			}
			bool flag = false;
			if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.Equals)
			{
				flag = this.FirstValue == this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.NotEquals)
			{
				flag = this.FirstValue != this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.LessThan)
			{
				flag = this.FirstValue < this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.GreaterThan)
			{
				flag = this.FirstValue > this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.GreaterThanOrEqual)
			{
				flag = this.FirstValue >= this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.LessThanOrEqual)
			{
				flag = this.FirstValue <= this.SecondValue;
			}
			this.SetState(flag ? this.TrueState : this.FalseState);
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006356 File Offset: 0x00004556
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000635E File Offset: 0x0000455E
		public FloatComparedStateChangerTextWidget.ComparisonTypes ComparisonType
		{
			get
			{
				return this._comparisonType;
			}
			set
			{
				if (value != this._comparisonType)
				{
					this._comparisonType = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006376 File Offset: 0x00004576
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000637E File Offset: 0x0000457E
		public float FirstValue
		{
			get
			{
				return this._firstValue;
			}
			set
			{
				if (value != this._firstValue)
				{
					this._firstValue = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006396 File Offset: 0x00004596
		// (set) Token: 0x0600017E RID: 382 RVA: 0x0000639E File Offset: 0x0000459E
		public float SecondValue
		{
			get
			{
				return this._secondValue;
			}
			set
			{
				if (value != this._secondValue)
				{
					this._secondValue = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000063B6 File Offset: 0x000045B6
		// (set) Token: 0x06000180 RID: 384 RVA: 0x000063BE File Offset: 0x000045BE
		public string TrueState
		{
			get
			{
				return this._trueState;
			}
			set
			{
				if (value != this._trueState)
				{
					this._trueState = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000063DB File Offset: 0x000045DB
		// (set) Token: 0x06000182 RID: 386 RVA: 0x000063E3 File Offset: 0x000045E3
		public string FalseState
		{
			get
			{
				return this._falseState;
			}
			set
			{
				if (value != this._falseState)
				{
					this._falseState = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x040000AC RID: 172
		private FloatComparedStateChangerTextWidget.ComparisonTypes _comparisonType;

		// Token: 0x040000AD RID: 173
		private float _firstValue;

		// Token: 0x040000AE RID: 174
		private float _secondValue;

		// Token: 0x040000AF RID: 175
		private string _trueState;

		// Token: 0x040000B0 RID: 176
		private string _falseState;

		// Token: 0x0200019E RID: 414
		public enum ComparisonTypes
		{
			// Token: 0x040009C3 RID: 2499
			Equals,
			// Token: 0x040009C4 RID: 2500
			NotEquals,
			// Token: 0x040009C5 RID: 2501
			GreaterThan,
			// Token: 0x040009C6 RID: 2502
			LessThan,
			// Token: 0x040009C7 RID: 2503
			GreaterThanOrEqual,
			// Token: 0x040009C8 RID: 2504
			LessThanOrEqual
		}
	}
}
