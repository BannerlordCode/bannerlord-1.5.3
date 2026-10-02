using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000016 RID: 22
	internal class TextOutput
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00006824 File Offset: 0x00004A24
		public float TextHeight
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.LineCount; i++)
				{
					TextLineOutput line = this.GetLine(i);
					num += line.Height;
				}
				return num;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000685C File Offset: 0x00004A5C
		public float TotalLineScale
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.LineCount; i++)
				{
					TextLineOutput line = this.GetLine(i);
					num += line.MaxScale;
				}
				return num;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00006892 File Offset: 0x00004A92
		public float LastLineWidth
		{
			get
			{
				return this._tokensWithLines[this._tokensWithLines.Count - 1].Width;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000068B1 File Offset: 0x00004AB1
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000068B9 File Offset: 0x00004AB9
		public float MaxLineHeight { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000068C2 File Offset: 0x00004AC2
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000068CA File Offset: 0x00004ACA
		public float MaxLineWidth { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000068D3 File Offset: 0x00004AD3
		// (set) Token: 0x060000EC RID: 236 RVA: 0x000068DB File Offset: 0x00004ADB
		public float MaxLineScale { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000068E4 File Offset: 0x00004AE4
		public int LineCount
		{
			get
			{
				return this._tokensWithLines.Count;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000068F4 File Offset: 0x00004AF4
		public TextOutput(float lineHeight)
		{
			this._tokensWithLines = new List<TextLineOutput>();
			this._lineHeight = lineHeight;
			TextLineOutput textLineOutput = new TextLineOutput(this._lineHeight);
			this._tokensWithLines.Add(textLineOutput);
			textLineOutput.LineEnded = true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006938 File Offset: 0x00004B38
		public TextLineOutput AddNewLine(bool currentLineEnded, float newLineBaseHeight = 0f)
		{
			TextLineOutput textLineOutput = this._tokensWithLines[this._tokensWithLines.Count - 1];
			textLineOutput.LineEnded = currentLineEnded;
			TextLineOutput textLineOutput2 = new TextLineOutput(newLineBaseHeight);
			this._tokensWithLines.Add(textLineOutput2);
			textLineOutput2.LineEnded = true;
			if (textLineOutput.Width > this.MaxLineWidth)
			{
				this.MaxLineWidth = textLineOutput.Width;
			}
			if (textLineOutput.MaxScale > this.MaxLineScale)
			{
				this.MaxLineScale = textLineOutput.MaxScale;
			}
			return textLineOutput2;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000069B4 File Offset: 0x00004BB4
		public void AddToken(TextToken textToken, float tokenWidth, float scaleValue, string style = "Default", float tokenHeight = -1f)
		{
			TextLineOutput textLineOutput = this._tokensWithLines[this._tokensWithLines.Count - 1];
			textLineOutput.AddToken(textToken, tokenWidth, tokenHeight, style, scaleValue);
			if (tokenHeight > this.MaxLineHeight)
			{
				this.MaxLineHeight = tokenHeight;
			}
			if (textLineOutput.Width > this.MaxLineWidth)
			{
				this.MaxLineWidth = textLineOutput.Width;
			}
			if (textLineOutput.MaxScale > this.MaxLineScale)
			{
				this.MaxLineScale = textLineOutput.MaxScale;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006A30 File Offset: 0x00004C30
		public List<TextTokenOutput> RemoveTokensFromEnd(int numberOfTokensToRemove)
		{
			List<TextTokenOutput> list = new List<TextTokenOutput>();
			for (int i = 0; i < numberOfTokensToRemove; i++)
			{
				if (this._tokensWithLines[this._tokensWithLines.Count - 1].TokenCount > 0)
				{
					TextLineOutput textLineOutput = this._tokensWithLines[this._tokensWithLines.Count - 1];
					list.Add(textLineOutput.RemoveTokenFromEnd());
				}
				else
				{
					this._tokensWithLines.RemoveAt(this._tokensWithLines.Count - 1);
					TextLineOutput textLineOutput2 = this._tokensWithLines[this._tokensWithLines.Count - 1];
					list.Add(textLineOutput2.RemoveTokenFromEnd());
				}
			}
			return list;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006ADA File Offset: 0x00004CDA
		public TextLineOutput GetLine(int i)
		{
			return this._tokensWithLines[i];
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00006AE8 File Offset: 0x00004CE8
		public IEnumerable<TextTokenOutput> Tokens
		{
			get
			{
				int num;
				for (int i = 0; i < this._tokensWithLines.Count; i = num + 1)
				{
					TextLineOutput tokensWithLine = this._tokensWithLines[i];
					for (int j = 0; j < tokensWithLine.TokenCount; j = num + 1)
					{
						yield return tokensWithLine.GetTokenOutput(j);
						num = j;
					}
					tokensWithLine = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00006AF8 File Offset: 0x00004CF8
		public IEnumerable<TextTokenOutput> TokensWithNewLines
		{
			get
			{
				int num;
				for (int i = 0; i < this._tokensWithLines.Count; i = num + 1)
				{
					TextLineOutput tokensWithLine = this._tokensWithLines[i];
					for (int j = 0; j < tokensWithLine.TokenCount; j = num + 1)
					{
						yield return tokensWithLine.GetTokenOutput(j);
						num = j;
					}
					if (i < this._tokensWithLines.Count - 1)
					{
						yield return new TextTokenOutput(TextToken.CreateNewLine(), 0f, 0f, string.Empty, 0f);
					}
					tokensWithLine = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006B08 File Offset: 0x00004D08
		public void Clear()
		{
			this.MaxLineHeight = 0f;
			this.MaxLineWidth = 0f;
			this.MaxLineScale = 0f;
			this._tokensWithLines.Clear();
			TextLineOutput textLineOutput = new TextLineOutput(this._lineHeight);
			this._tokensWithLines.Add(textLineOutput);
			textLineOutput.LineEnded = true;
		}

		// Token: 0x04000092 RID: 146
		private List<TextLineOutput> _tokensWithLines;

		// Token: 0x04000093 RID: 147
		private readonly float _lineHeight;
	}
}
