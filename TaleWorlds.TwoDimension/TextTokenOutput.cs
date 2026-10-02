using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000019 RID: 25
	internal class TextTokenOutput
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006D8F File Offset: 0x00004F8F
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00006D97 File Offset: 0x00004F97
		public float X { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00006DA0 File Offset: 0x00004FA0
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00006DA8 File Offset: 0x00004FA8
		public float Y { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00006DB1 File Offset: 0x00004FB1
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00006DB9 File Offset: 0x00004FB9
		public float Width { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00006DC2 File Offset: 0x00004FC2
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00006DCA File Offset: 0x00004FCA
		public float Height { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00006DD3 File Offset: 0x00004FD3
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00006DDB File Offset: 0x00004FDB
		public float Scale { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006DE4 File Offset: 0x00004FE4
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00006DEC File Offset: 0x00004FEC
		public SimpleRectangle Rectangle { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006DF5 File Offset: 0x00004FF5
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00006DFD File Offset: 0x00004FFD
		public TextToken Token { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00006E06 File Offset: 0x00005006
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00006E0E File Offset: 0x0000500E
		public string Style { get; private set; }

		// Token: 0x0600011E RID: 286 RVA: 0x00006E18 File Offset: 0x00005018
		public TextTokenOutput(TextToken token, float width, float height, string style, float scaleValue)
		{
			this.Token = token;
			this.Width = width;
			this.Height = height;
			this.Rectangle = new SimpleRectangle(0f, 0f, this.Width, this.Height);
			this.Style = style;
			this.Scale = scaleValue;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006E71 File Offset: 0x00005071
		public void SetPosition(float x, float y)
		{
			this.X = x;
			this.Y = y;
			this.Rectangle = new SimpleRectangle(x, y, this.Width, this.Height);
		}
	}
}
