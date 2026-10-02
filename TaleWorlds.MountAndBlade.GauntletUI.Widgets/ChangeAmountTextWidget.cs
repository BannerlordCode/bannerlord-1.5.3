using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200000A RID: 10
	public class ChangeAmountTextWidget : TextWidget
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000028EA File Offset: 0x00000AEA
		public ChangeAmountTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028FC File Offset: 0x00000AFC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isVisualsDirty)
			{
				if (!this.ShouldBeVisible)
				{
					base.IsVisible = false;
				}
				else
				{
					base.IsVisible = this.Amount != 0;
					if (base.IsVisible)
					{
						base.Text = ((this.Amount > 0) ? ("+" + this.Amount.ToString()) : this.Amount.ToString());
						if (this.UseParentheses)
						{
							base.Text = "(" + base.Text + ")";
						}
						if (this.Amount > 0)
						{
							base.Brush = this._positiveBrush;
						}
						else if (this.Amount < 0)
						{
							base.Brush = this._negativeBrush;
						}
					}
				}
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000029D6 File Offset: 0x00000BD6
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000029DE File Offset: 0x00000BDE
		[Editor(false)]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (this._amount != value)
				{
					this._amount = value;
					base.OnPropertyChanged(value, "Amount");
					this._isVisualsDirty = false;
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002A03 File Offset: 0x00000C03
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002A0B File Offset: 0x00000C0B
		[Editor(false)]
		public bool UseParentheses
		{
			get
			{
				return this._useParentheses;
			}
			set
			{
				if (this._useParentheses != value)
				{
					this._useParentheses = value;
					base.OnPropertyChanged(value, "UseParentheses");
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002A29 File Offset: 0x00000C29
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002A31 File Offset: 0x00000C31
		[Editor(false)]
		public bool ShouldBeVisible
		{
			get
			{
				return this._shouldBeVisible;
			}
			set
			{
				if (this._shouldBeVisible != value)
				{
					this._shouldBeVisible = value;
					base.OnPropertyChanged(value, "ShouldBeVisible");
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002A4F File Offset: 0x00000C4F
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002A58 File Offset: 0x00000C58
		[Editor(false)]
		public string NegativeBrushName
		{
			get
			{
				return this._negativeBrushName;
			}
			set
			{
				if (this._negativeBrushName != value)
				{
					this._negativeBrushName = value;
					base.OnPropertyChanged<string>(value, "NegativeBrushName");
					this._negativeBrush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == value);
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002AC9 File Offset: 0x00000CC9
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002AD4 File Offset: 0x00000CD4
		[Editor(false)]
		public string PositiveBrushName
		{
			get
			{
				return this._positiveBrushName;
			}
			set
			{
				if (this._positiveBrushName != value)
				{
					this._positiveBrushName = value;
					base.OnPropertyChanged<string>(value, "PositiveBrushName");
					this._positiveBrush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == value);
				}
			}
		}

		// Token: 0x04000017 RID: 23
		private bool _isVisualsDirty;

		// Token: 0x04000018 RID: 24
		private Brush _negativeBrush;

		// Token: 0x04000019 RID: 25
		private Brush _positiveBrush;

		// Token: 0x0400001A RID: 26
		private bool _useParentheses;

		// Token: 0x0400001B RID: 27
		private int _amount;

		// Token: 0x0400001C RID: 28
		private string _negativeBrushName;

		// Token: 0x0400001D RID: 29
		private string _positiveBrushName;

		// Token: 0x0400001E RID: 30
		private bool _shouldBeVisible = true;
	}
}
