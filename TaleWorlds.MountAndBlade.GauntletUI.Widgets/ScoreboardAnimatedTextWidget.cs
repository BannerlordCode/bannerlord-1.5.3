using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003C RID: 60
	public class ScoreboardAnimatedTextWidget : TextWidget
	{
		// Token: 0x0600038E RID: 910 RVA: 0x0000B766 File Offset: 0x00009966
		public ScoreboardAnimatedTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000B76F File Offset: 0x0000996F
		private void HandleValueChanged(int value)
		{
			base.Text = ((!this.ShowZero && value == 0) ? "" : value.ToString());
			base.BrushRenderer.RestartAnimation();
			base.RegisterUpdateBrushes();
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000B7A1 File Offset: 0x000099A1
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000B7A9 File Offset: 0x000099A9
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
					this._valueAsInt = value;
					base.OnPropertyChanged(value, "ValueAsInt");
					this.HandleValueChanged(value);
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000B7CE File Offset: 0x000099CE
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000B7D6 File Offset: 0x000099D6
		[Editor(false)]
		public bool ShowZero
		{
			get
			{
				return this._showZero;
			}
			set
			{
				if (this._showZero != value)
				{
					this._showZero = value;
					base.OnPropertyChanged(value, "ShowZero");
					this.HandleValueChanged(this._valueAsInt);
				}
			}
		}

		// Token: 0x04000175 RID: 373
		private bool _showZero;

		// Token: 0x04000176 RID: 374
		private int _valueAsInt;
	}
}
