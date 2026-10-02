using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000007 RID: 7
	public class DoubleStringBasedVisibilityWidget : Widget
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000288A File Offset: 0x00000A8A
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002892 File Offset: 0x00000A92
		public DoubleStringBasedVisibilityWidget.WatchTypes WatchType
		{
			get
			{
				return this._watchType;
			}
			set
			{
				if (this._watchType != value)
				{
					this._watchType = value;
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000028AA File Offset: 0x00000AAA
		// (set) Token: 0x06000043 RID: 67 RVA: 0x000028B2 File Offset: 0x00000AB2
		public DoubleStringBasedVisibilityWidget.JoinTypes JoinType
		{
			get
			{
				return this._joinType;
			}
			set
			{
				if (this._joinType != value)
				{
					this._joinType = value;
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000028CA File Offset: 0x00000ACA
		public DoubleStringBasedVisibilityWidget(UIContext context)
			: base(context)
		{
			this.UpdateVisibility();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000028DC File Offset: 0x00000ADC
		private void UpdateVisibility()
		{
			bool flag = string.Equals(this.FirstString, this.ComparisonString, StringComparison.OrdinalIgnoreCase);
			bool flag2 = string.Equals(this.SecondString, this.ComparisonString, StringComparison.OrdinalIgnoreCase);
			if (this.WatchType == DoubleStringBasedVisibilityWidget.WatchTypes.NotEqual)
			{
				flag = !flag;
				flag2 = !flag2;
			}
			base.IsVisible = ((this.JoinType == DoubleStringBasedVisibilityWidget.JoinTypes.And) ? (flag && flag2) : (flag || flag2));
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002938 File Offset: 0x00000B38
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002940 File Offset: 0x00000B40
		[Editor(false)]
		public string FirstString
		{
			get
			{
				return this._firstString;
			}
			set
			{
				if (this._firstString != value)
				{
					this._firstString = value;
					base.OnPropertyChanged<string>(value, "FirstString");
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002969 File Offset: 0x00000B69
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002971 File Offset: 0x00000B71
		[Editor(false)]
		public string SecondString
		{
			get
			{
				return this._secondString;
			}
			set
			{
				if (this._secondString != value)
				{
					this._secondString = value;
					base.OnPropertyChanged<string>(value, "SecondString");
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000299A File Offset: 0x00000B9A
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000029A2 File Offset: 0x00000BA2
		[Editor(false)]
		public string ComparisonString
		{
			get
			{
				return this._comparisonString;
			}
			set
			{
				if (this._comparisonString != value)
				{
					this._comparisonString = value;
					base.OnPropertyChanged<string>(value, "ComparisonString");
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x04000022 RID: 34
		private DoubleStringBasedVisibilityWidget.WatchTypes _watchType;

		// Token: 0x04000023 RID: 35
		private DoubleStringBasedVisibilityWidget.JoinTypes _joinType;

		// Token: 0x04000024 RID: 36
		private string _firstString;

		// Token: 0x04000025 RID: 37
		private string _secondString;

		// Token: 0x04000026 RID: 38
		private string _comparisonString;

		// Token: 0x0200001E RID: 30
		public enum WatchTypes
		{
			// Token: 0x040000C5 RID: 197
			Equal,
			// Token: 0x040000C6 RID: 198
			NotEqual
		}

		// Token: 0x0200001F RID: 31
		public enum JoinTypes
		{
			// Token: 0x040000C8 RID: 200
			And,
			// Token: 0x040000C9 RID: 201
			Or
		}
	}
}
