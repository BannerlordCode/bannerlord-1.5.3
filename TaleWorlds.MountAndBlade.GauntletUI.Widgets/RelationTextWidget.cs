using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003A RID: 58
	public class RelationTextWidget : TextWidget
	{
		// Token: 0x06000357 RID: 855 RVA: 0x0000ABD9 File Offset: 0x00008DD9
		public RelationTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000ABEC File Offset: 0x00008DEC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isVisualsDirty)
			{
				base.Text = ((this.Amount > 0) ? ("+" + this.Amount.ToString()) : this.Amount.ToString());
				if (this.Amount > 0)
				{
					base.Brush.FontColor = this.PositiveColor;
				}
				else if (this.Amount < 0)
				{
					base.Brush.FontColor = this.NegativeColor;
				}
				else
				{
					base.Brush.FontColor = this.ZeroColor;
				}
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000AC92 File Offset: 0x00008E92
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000AC9A File Offset: 0x00008E9A
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
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000ACBF File Offset: 0x00008EBF
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000ACC7 File Offset: 0x00008EC7
		[Editor(false)]
		public Color ZeroColor
		{
			get
			{
				return this._zeroColor;
			}
			set
			{
				if (value != this._zeroColor)
				{
					this._zeroColor = value;
					base.OnPropertyChanged(value, "ZeroColor");
				}
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000ACEA File Offset: 0x00008EEA
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000ACF2 File Offset: 0x00008EF2
		[Editor(false)]
		public Color PositiveColor
		{
			get
			{
				return this._positiveColor;
			}
			set
			{
				if (value != this._positiveColor)
				{
					this._positiveColor = value;
					base.OnPropertyChanged(value, "PositiveColor");
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000AD15 File Offset: 0x00008F15
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0000AD1D File Offset: 0x00008F1D
		[Editor(false)]
		public Color NegativeColor
		{
			get
			{
				return this._negativeColor;
			}
			set
			{
				if (value != this._negativeColor)
				{
					this._negativeColor = value;
					base.OnPropertyChanged(value, "NegativeColor");
				}
			}
		}

		// Token: 0x04000157 RID: 343
		private bool _isVisualsDirty = true;

		// Token: 0x04000158 RID: 344
		private int _amount;

		// Token: 0x04000159 RID: 345
		private Color _zeroColor;

		// Token: 0x0400015A RID: 346
		private Color _positiveColor;

		// Token: 0x0400015B RID: 347
		private Color _negativeColor;
	}
}
