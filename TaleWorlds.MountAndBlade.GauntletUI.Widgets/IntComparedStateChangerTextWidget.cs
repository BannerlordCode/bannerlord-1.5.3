using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002A RID: 42
	public class IntComparedStateChangerTextWidget : TextWidget
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00007E0B File Offset: 0x0000600B
		public IntComparedStateChangerTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00007E14 File Offset: 0x00006014
		private void UpdateState()
		{
			if (string.IsNullOrEmpty(this.TrueState) || string.IsNullOrEmpty(this.FalseState))
			{
				return;
			}
			bool flag = false;
			if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.Equals)
			{
				flag = this.FirstValue == this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.NotEquals)
			{
				flag = this.FirstValue != this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.LessThan)
			{
				flag = this.FirstValue < this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.GreaterThan)
			{
				flag = this.FirstValue > this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.GreaterThanOrEqual)
			{
				flag = this.FirstValue >= this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.LessThanOrEqual)
			{
				flag = this.FirstValue <= this.SecondValue;
			}
			this.SetState(flag ? this.TrueState : this.FalseState);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00007EFA File Offset: 0x000060FA
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00007F02 File Offset: 0x00006102
		public IntComparedStateChangerTextWidget.ComparisonTypes ComparisonType
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00007F1A File Offset: 0x0000611A
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00007F22 File Offset: 0x00006122
		public int FirstValue
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00007F3A File Offset: 0x0000613A
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00007F42 File Offset: 0x00006142
		public int SecondValue
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00007F5A File Offset: 0x0000615A
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00007F62 File Offset: 0x00006162
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00007F7F File Offset: 0x0000617F
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00007F87 File Offset: 0x00006187
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

		// Token: 0x04000103 RID: 259
		private IntComparedStateChangerTextWidget.ComparisonTypes _comparisonType;

		// Token: 0x04000104 RID: 260
		private int _firstValue;

		// Token: 0x04000105 RID: 261
		private int _secondValue;

		// Token: 0x04000106 RID: 262
		private string _trueState;

		// Token: 0x04000107 RID: 263
		private string _falseState;

		// Token: 0x020001A0 RID: 416
		public enum ComparisonTypes
		{
			// Token: 0x040009CC RID: 2508
			Equals,
			// Token: 0x040009CD RID: 2509
			NotEquals,
			// Token: 0x040009CE RID: 2510
			GreaterThan,
			// Token: 0x040009CF RID: 2511
			LessThan,
			// Token: 0x040009D0 RID: 2512
			GreaterThanOrEqual,
			// Token: 0x040009D1 RID: 2513
			LessThanOrEqual
		}
	}
}
