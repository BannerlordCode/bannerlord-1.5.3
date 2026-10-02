using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000010 RID: 16
	public class StyleFontContainer
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00004E6D File Offset: 0x0000306D
		public StyleFontContainer()
		{
			this._styleFonts = new Dictionary<string, StyleFontContainer.FontData>();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004E80 File Offset: 0x00003080
		public void Add(string style, Font font, float fontSize)
		{
			StyleFontContainer.FontData fontData = new StyleFontContainer.FontData(font, fontSize);
			this._styleFonts.Add(style, fontData);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004EA4 File Offset: 0x000030A4
		public StyleFontContainer.FontData GetFontData(string style)
		{
			StyleFontContainer.FontData fontData;
			if (this._styleFonts.TryGetValue(style, out fontData))
			{
				return fontData;
			}
			StyleFontContainer.FontData fontData2;
			this._styleFonts.TryGetValue("Default", out fontData2);
			return fontData2;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004ED7 File Offset: 0x000030D7
		public void ClearFonts()
		{
			this._styleFonts.Clear();
		}

		// Token: 0x0400005A RID: 90
		private readonly Dictionary<string, StyleFontContainer.FontData> _styleFonts;

		// Token: 0x0200003C RID: 60
		public struct FontData
		{
			// Token: 0x060002AA RID: 682 RVA: 0x0000A033 File Offset: 0x00008233
			public FontData(Font font, float fontSize)
			{
				this.Font = font;
				this.FontSize = fontSize;
			}

			// Token: 0x04000142 RID: 322
			public Font Font;

			// Token: 0x04000143 RID: 323
			public float FontSize;
		}
	}
}
