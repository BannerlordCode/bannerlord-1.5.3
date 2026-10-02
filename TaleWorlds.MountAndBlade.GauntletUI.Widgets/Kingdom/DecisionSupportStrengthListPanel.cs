using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000134 RID: 308
	public class DecisionSupportStrengthListPanel : ListPanel
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0002CA0E File Offset: 0x0002AC0E
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x0002CA16 File Offset: 0x0002AC16
		public bool IsAbstain { get; set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x0002CA1F File Offset: 0x0002AC1F
		// (set) Token: 0x06001023 RID: 4131 RVA: 0x0002CA27 File Offset: 0x0002AC27
		public bool IsPlayerSupporter { get; set; }

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0002CA30 File Offset: 0x0002AC30
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x0002CA38 File Offset: 0x0002AC38
		public bool IsOptionSelected { get; set; }

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0002CA41 File Offset: 0x0002AC41
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x0002CA49 File Offset: 0x0002AC49
		public bool IsKingsOutcome { get; set; }

		// Token: 0x06001028 RID: 4136 RVA: 0x0002CA52 File Offset: 0x0002AC52
		public DecisionSupportStrengthListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0002CA5C File Offset: 0x0002AC5C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			switch (this.CurrentIndex)
			{
			case 2:
				this.StrengthButton0.IsSelected = true;
				this.StrengthButton1.IsSelected = false;
				this.StrengthButton2.IsSelected = false;
				break;
			case 3:
				this.StrengthButton0.IsSelected = false;
				this.StrengthButton1.IsSelected = true;
				this.StrengthButton2.IsSelected = false;
				break;
			case 4:
				this.StrengthButton0.IsSelected = false;
				this.StrengthButton1.IsSelected = false;
				this.StrengthButton2.IsSelected = true;
				break;
			}
			base.GamepadNavigationIndex = (this.IsOptionSelected ? (-1) : 0);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0002CB0F File Offset: 0x0002AD0F
		private void SetButtonsEnabled(bool isEnabled)
		{
			this.StrengthButton0.IsEnabled = isEnabled;
			this.StrengthButton1.IsEnabled = isEnabled;
			this.StrengthButton2.IsEnabled = isEnabled;
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0002CB35 File Offset: 0x0002AD35
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x0002CB3D File Offset: 0x0002AD3D
		[Editor(false)]
		public int CurrentIndex
		{
			get
			{
				return this._currentIndex;
			}
			set
			{
				if (this._currentIndex != value)
				{
					this._currentIndex = value;
					base.OnPropertyChanged(value, "CurrentIndex");
				}
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0002CB5B File Offset: 0x0002AD5B
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0002CB63 File Offset: 0x0002AD63
		[Editor(false)]
		public ButtonWidget StrengthButton0
		{
			get
			{
				return this._strengthButton0;
			}
			set
			{
				if (this._strengthButton0 != value)
				{
					this._strengthButton0 = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StrengthButton0");
				}
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0002CB81 File Offset: 0x0002AD81
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x0002CB89 File Offset: 0x0002AD89
		[Editor(false)]
		public ButtonWidget StrengthButton1
		{
			get
			{
				return this._strengthButton1;
			}
			set
			{
				if (this._strengthButton1 != value)
				{
					this._strengthButton1 = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StrengthButton1");
				}
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0002CBA7 File Offset: 0x0002ADA7
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x0002CBAF File Offset: 0x0002ADAF
		[Editor(false)]
		public ButtonWidget StrengthButton2
		{
			get
			{
				return this._strengthButton2;
			}
			set
			{
				if (this._strengthButton2 != value)
				{
					this._strengthButton2 = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StrengthButton2");
				}
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0002CBCD File Offset: 0x0002ADCD
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x0002CBD5 File Offset: 0x0002ADD5
		[Editor(false)]
		public RichTextWidget StrengthButton0Text
		{
			get
			{
				return this._strengthButton0Text;
			}
			set
			{
				if (this._strengthButton0Text != value)
				{
					this._strengthButton0Text = value;
					base.OnPropertyChanged<RichTextWidget>(value, "StrengthButton0Text");
				}
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0002CBF3 File Offset: 0x0002ADF3
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x0002CBFB File Offset: 0x0002ADFB
		[Editor(false)]
		public RichTextWidget StrengthButton1Text
		{
			get
			{
				return this._strengthButton1Text;
			}
			set
			{
				if (this._strengthButton1Text != value)
				{
					this._strengthButton1Text = value;
					base.OnPropertyChanged<RichTextWidget>(value, "StrengthButton1Text");
				}
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0002CC19 File Offset: 0x0002AE19
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x0002CC21 File Offset: 0x0002AE21
		[Editor(false)]
		public RichTextWidget StrengthButton2Text
		{
			get
			{
				return this._strengthButton2Text;
			}
			set
			{
				if (this._strengthButton2Text != value)
				{
					this._strengthButton2Text = value;
					base.OnPropertyChanged<RichTextWidget>(value, "StrengthButton2Text");
				}
			}
		}

		// Token: 0x0400075E RID: 1886
		private ButtonWidget _strengthButton0;

		// Token: 0x0400075F RID: 1887
		private RichTextWidget _strengthButton0Text;

		// Token: 0x04000760 RID: 1888
		private ButtonWidget _strengthButton1;

		// Token: 0x04000761 RID: 1889
		private RichTextWidget _strengthButton1Text;

		// Token: 0x04000762 RID: 1890
		private ButtonWidget _strengthButton2;

		// Token: 0x04000763 RID: 1891
		private RichTextWidget _strengthButton2Text;

		// Token: 0x04000764 RID: 1892
		private int _currentIndex;
	}
}
