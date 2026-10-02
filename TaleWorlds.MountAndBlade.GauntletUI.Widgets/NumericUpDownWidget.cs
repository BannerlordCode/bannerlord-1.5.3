using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000034 RID: 52
	public class NumericUpDownWidget : Widget
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00009A03 File Offset: 0x00007C03
		public NumericUpDownWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00009A22 File Offset: 0x00007C22
		private void OnUpButtonClicked(Widget widget)
		{
			this.ChangeValue(1);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00009A2B File Offset: 0x00007C2B
		private void OnDownButtonClicked(Widget widget)
		{
			this.ChangeValue(-1);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00009A34 File Offset: 0x00007C34
		private void ChangeValue(int changeAmount)
		{
			int num = this.IntValue + changeAmount;
			if ((float)num <= this.MaxValue && (float)num >= this.MinValue)
			{
				this.IntValue = num;
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00009A68 File Offset: 0x00007C68
		private void UpdateControlButtonsEnabled()
		{
			if (this.UpButton != null)
			{
				this.UpButton.IsEnabled = (float)(this._intValue + 1) <= this.MaxValue;
			}
			if (this.DownButton != null)
			{
				this.DownButton.IsEnabled = (float)(this._intValue - 1) >= this.MinValue;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00009AC3 File Offset: 0x00007CC3
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00009ACB File Offset: 0x00007CCB
		[Editor(false)]
		public bool ShowOneAdded
		{
			get
			{
				return this._showOneAdded;
			}
			set
			{
				if (this._showOneAdded != value)
				{
					this._showOneAdded = value;
					base.OnPropertyChanged(value, "ShowOneAdded");
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00009AE9 File Offset: 0x00007CE9
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00009AF4 File Offset: 0x00007CF4
		[Editor(false)]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (this._intValue != value)
				{
					this._intValue = value;
					this.Value = (float)this._intValue;
					base.OnPropertyChanged(value, "IntValue");
					this._textWidget.IntText = (this.ShowOneAdded ? (this.IntValue + 1) : this.IntValue);
					this.UpdateControlButtonsEnabled();
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00009B53 File Offset: 0x00007D53
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00009B5B File Offset: 0x00007D5B
		[Editor(false)]
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._value != value)
				{
					this._value = value;
					this.IntValue = (int)this._value;
					base.OnPropertyChanged(value, "Value");
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00009B86 File Offset: 0x00007D86
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00009B8E File Offset: 0x00007D8E
		[Editor(false)]
		public float MinValue
		{
			get
			{
				return this._minValue;
			}
			set
			{
				if (value != this._minValue)
				{
					this._minValue = value;
					base.OnPropertyChanged(value, "MinValue");
					this.UpdateControlButtonsEnabled();
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00009BB2 File Offset: 0x00007DB2
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00009BBA File Offset: 0x00007DBA
		[Editor(false)]
		public float MaxValue
		{
			get
			{
				return this._maxValue;
			}
			set
			{
				if (value != this._maxValue)
				{
					this._maxValue = value;
					base.OnPropertyChanged(value, "MaxValue");
					this.UpdateControlButtonsEnabled();
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00009BDE File Offset: 0x00007DDE
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00009BE6 File Offset: 0x00007DE6
		[Editor(false)]
		public TextWidget TextWidget
		{
			get
			{
				return this._textWidget;
			}
			set
			{
				if (this._textWidget != value)
				{
					this._textWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "TextWidget");
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00009C04 File Offset: 0x00007E04
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00009C0C File Offset: 0x00007E0C
		[Editor(false)]
		public ButtonWidget UpButton
		{
			get
			{
				return this._upButton;
			}
			set
			{
				if (this._upButton != value)
				{
					this._upButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "UpButton");
					if (value != null && !this._upButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnUpButtonClicked)))
					{
						this._upButton.ClickEventHandlers.Add(new Action<Widget>(this.OnUpButtonClicked));
					}
				}
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00009C72 File Offset: 0x00007E72
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00009C7C File Offset: 0x00007E7C
		[Editor(false)]
		public ButtonWidget DownButton
		{
			get
			{
				return this._downButton;
			}
			set
			{
				if (this._downButton != value)
				{
					this._downButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DownButton");
					if (value != null && !this._downButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnDownButtonClicked)))
					{
						this._downButton.ClickEventHandlers.Add(new Action<Widget>(this.OnDownButtonClicked));
					}
				}
			}
		}

		// Token: 0x04000139 RID: 313
		private bool _showOneAdded;

		// Token: 0x0400013A RID: 314
		private float _minValue;

		// Token: 0x0400013B RID: 315
		private float _maxValue;

		// Token: 0x0400013C RID: 316
		private int _intValue = int.MinValue;

		// Token: 0x0400013D RID: 317
		private float _value = float.MinValue;

		// Token: 0x0400013E RID: 318
		private TextWidget _textWidget;

		// Token: 0x0400013F RID: 319
		private ButtonWidget _upButton;

		// Token: 0x04000140 RID: 320
		private ButtonWidget _downButton;
	}
}
