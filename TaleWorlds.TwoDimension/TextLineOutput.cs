using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000014 RID: 20
	internal class TextLineOutput
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000060ED File Offset: 0x000042ED
		// (set) Token: 0x060000CC RID: 204 RVA: 0x000060F5 File Offset: 0x000042F5
		public float Width { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000060FE File Offset: 0x000042FE
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00006106 File Offset: 0x00004306
		public float TextWidth { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000610F File Offset: 0x0000430F
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00006117 File Offset: 0x00004317
		public bool LineEnded { get; internal set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00006120 File Offset: 0x00004320
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00006128 File Offset: 0x00004328
		public int EmptyCharacterCount { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00006131 File Offset: 0x00004331
		public int TokenCount
		{
			get
			{
				return this._tokens.Count;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000613E File Offset: 0x0000433E
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00006146 File Offset: 0x00004346
		public float Height { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000614F File Offset: 0x0000434F
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00006157 File Offset: 0x00004357
		public float MaxScale { get; private set; }

		// Token: 0x060000D8 RID: 216 RVA: 0x00006160 File Offset: 0x00004360
		public TextLineOutput(float lineHeight)
		{
			this._tokens = new List<TextTokenOutput>();
			this.Height = lineHeight;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000617C File Offset: 0x0000437C
		public void AddToken(TextToken textToken, float tokenWidth, float tokenHeight, string style, float scaleValue)
		{
			if (textToken.Type == TextToken.TokenType.EmptyCharacter)
			{
				int emptyCharacterCount = this.EmptyCharacterCount;
				this.EmptyCharacterCount = emptyCharacterCount + 1;
			}
			else
			{
				this.TextWidth += tokenWidth;
			}
			TextTokenOutput textTokenOutput;
			if (tokenHeight > 0f)
			{
				textTokenOutput = new TextTokenOutput(textToken, tokenWidth, tokenHeight, style, scaleValue);
			}
			else
			{
				textTokenOutput = new TextTokenOutput(textToken, tokenWidth, this.Height, style, scaleValue);
			}
			this._tokens.Add(textTokenOutput);
			this.Width += tokenWidth;
			if (tokenHeight > this.Height)
			{
				this.Height = tokenHeight;
			}
			if (scaleValue > this.MaxScale)
			{
				this.MaxScale = scaleValue;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006216 File Offset: 0x00004416
		public TextToken GetToken(int i)
		{
			return this._tokens[i].Token;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006229 File Offset: 0x00004429
		public TextTokenOutput GetTokenOutput(int i)
		{
			return this._tokens[i];
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006238 File Offset: 0x00004438
		public TextTokenOutput RemoveTokenFromEnd()
		{
			TextTokenOutput textTokenOutput = this._tokens[this._tokens.Count - 1];
			this._tokens.Remove(textTokenOutput);
			this.Width -= textTokenOutput.Width;
			return textTokenOutput;
		}

		// Token: 0x04000083 RID: 131
		private List<TextTokenOutput> _tokens;
	}
}
