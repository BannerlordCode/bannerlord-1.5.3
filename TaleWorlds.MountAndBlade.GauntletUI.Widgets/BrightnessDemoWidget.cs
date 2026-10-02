using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000009 RID: 9
	public class BrightnessDemoWidget : TextureWidget
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002879 File Offset: 0x00000A79
		public BrightnessDemoWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "BrightnessDemoTextureProvider";
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002894 File Offset: 0x00000A94
		// (set) Token: 0x0600003E RID: 62 RVA: 0x0000289C File Offset: 0x00000A9C
		[Editor(false)]
		public BrightnessDemoWidget.DemoTypes DemoType
		{
			get
			{
				return this._demoType;
			}
			set
			{
				if (this._demoType != value)
				{
					this._demoType = value;
					base.OnPropertyChanged<string>(Enum.GetName(typeof(BrightnessDemoWidget.DemoTypes), value), "DemoType");
					base.SetTextureProviderProperty("DemoType", (int)value);
				}
			}
		}

		// Token: 0x04000016 RID: 22
		private BrightnessDemoWidget.DemoTypes _demoType = BrightnessDemoWidget.DemoTypes.None;

		// Token: 0x02000197 RID: 407
		public enum DemoTypes
		{
			// Token: 0x040009AC RID: 2476
			None = -1,
			// Token: 0x040009AD RID: 2477
			BrightnessWide,
			// Token: 0x040009AE RID: 2478
			ExposureTexture1,
			// Token: 0x040009AF RID: 2479
			ExposureTexture2,
			// Token: 0x040009B0 RID: 2480
			ExposureTexture3,
			// Token: 0x040009B1 RID: 2481
			ExposureTexture4,
			// Token: 0x040009B2 RID: 2482
			ExposureTexture5,
			// Token: 0x040009B3 RID: 2483
			ExposureTexture6
		}
	}
}
