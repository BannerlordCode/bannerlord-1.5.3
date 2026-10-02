using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CC RID: 204
	public class ClassLoadoutTroopTupleCultureColorBrushWidget : BrushWidget
	{
		// Token: 0x06000AB6 RID: 2742 RVA: 0x0001E19F File Offset: 0x0001C39F
		public ClassLoadoutTroopTupleCultureColorBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0001E1A8 File Offset: 0x0001C3A8
		private void UpdateColor()
		{
			foreach (Style style in base.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = this.CultureColor;
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0001E21C File Offset: 0x0001C41C
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0001E224 File Offset: 0x0001C424
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChanged(value, "CultureColor");
					this.UpdateColor();
				}
			}
		}

		// Token: 0x040004E3 RID: 1251
		private Color _cultureColor;
	}
}
