using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Culture
{
	// Token: 0x0200018C RID: 396
	public class CharacterCreationBackgroundGradientBrushWidget : BrushWidget
	{
		// Token: 0x060014AD RID: 5293 RVA: 0x00038745 File Offset: 0x00036945
		public CharacterCreationBackgroundGradientBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00038750 File Offset: 0x00036950
		private void SetCultureBackground(Color cultureColor1)
		{
			foreach (Style style in base.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = cultureColor1;
				}
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x000387C0 File Offset: 0x000369C0
		// (set) Token: 0x060014B0 RID: 5296 RVA: 0x000387C8 File Offset: 0x000369C8
		[Editor(false)]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (this._cultureColor1 != value)
				{
					this._cultureColor1 = value;
					base.OnPropertyChanged(value, "CultureColor1");
					this.SetCultureBackground(value);
				}
			}
		}

		// Token: 0x0400096B RID: 2411
		private Color _cultureColor1;
	}
}
