using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012C RID: 300
	public class MapBarCustomValueTextWidget : TextWidget
	{
		// Token: 0x06000FD5 RID: 4053 RVA: 0x0002BEFD File Offset: 0x0002A0FD
		public MapBarCustomValueTextWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002BF10 File Offset: 0x0002A110
		private void RefreshTextAnimation(int valueDifference)
		{
			if (valueDifference <= 0)
			{
				if (valueDifference < 0)
				{
					if (base.CurrentState == "Negative")
					{
						base.BrushRenderer.RestartAnimation();
						return;
					}
					this.SetState("Negative");
				}
				return;
			}
			if (base.CurrentState == "Positive")
			{
				base.BrushRenderer.RestartAnimation();
				return;
			}
			this.SetState("Positive");
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002BF78 File Offset: 0x0002A178
		private void RefreshFontColor()
		{
			Color color;
			if (this.IsWarning)
			{
				color = this.WarningColor;
			}
			else
			{
				color = this.NormalColor;
			}
			foreach (Style style in base.Brush.Styles)
			{
				style.FontColor = color;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0002BFE8 File Offset: 0x0002A1E8
		// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		[Editor(false)]
		public int ValueAsInt
		{
			get
			{
				return this._valueAsInt;
			}
			set
			{
				if (value != this._valueAsInt)
				{
					this.RefreshTextAnimation(value - this._valueAsInt);
					this._valueAsInt = value;
					base.OnPropertyChanged(value, "ValueAsInt");
				}
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0002C01C File Offset: 0x0002A21C
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x0002C024 File Offset: 0x0002A224
		[Editor(false)]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChanged(value, "IsWarning");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0002C048 File Offset: 0x0002A248
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x0002C050 File Offset: 0x0002A250
		[Editor(false)]
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					base.OnPropertyChanged(value, "NormalColor");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0002C079 File Offset: 0x0002A279
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x0002C081 File Offset: 0x0002A281
		[Editor(false)]
		public Color WarningColor
		{
			get
			{
				return this._warningColor;
			}
			set
			{
				if (value != this._warningColor)
				{
					this._warningColor = value;
					base.OnPropertyChanged(value, "WarningColor");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x0400073B RID: 1851
		private bool _isWarning;

		// Token: 0x0400073C RID: 1852
		private Color _normalColor;

		// Token: 0x0400073D RID: 1853
		private Color _warningColor;

		// Token: 0x0400073E RID: 1854
		private int _valueAsInt;
	}
}
