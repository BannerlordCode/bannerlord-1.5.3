using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Credits
{
	// Token: 0x02000164 RID: 356
	public class CreditsTextWidget : RichTextWidget
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x00034332 File Offset: 0x00032532
		public CreditsTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0003433C File Offset: 0x0003253C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.overrideFont != null)
			{
				this._richText.StyleFontContainer.ClearFonts();
				foreach (Style style in base.ReadOnlyBrush.Styles)
				{
					this._richText.StyleFontContainer.Add(style.Name, this.overrideFont, (float)style.FontSize * base._scaleToUse);
				}
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x000343D8 File Offset: 0x000325D8
		// (set) Token: 0x060012ED RID: 4845 RVA: 0x000343E0 File Offset: 0x000325E0
		[Editor(false)]
		public string OverrideFont
		{
			get
			{
				return this._overrideFont;
			}
			set
			{
				if (this._overrideFont != value)
				{
					this._overrideFont = value;
					base.OnPropertyChanged<string>(value, "OverrideFont");
					this.overrideFont = base.Context.FontFactory.GetFont(this.OverrideFont);
				}
			}
		}

		// Token: 0x0400089F RID: 2207
		private Font overrideFont;

		// Token: 0x040008A0 RID: 2208
		private string _overrideFont;
	}
}
