using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.BitmapFont;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000011 RID: 17
	public class Text : IText
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004EE4 File Offset: 0x000030E4
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004EEC File Offset: 0x000030EC
		public ILanguage CurrentLanguage { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004EF5 File Offset: 0x000030F5
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00004EFD File Offset: 0x000030FD
		public float ScaleToFitTextInLayout { get; private set; } = 1f;

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004F06 File Offset: 0x00003106
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004F0E File Offset: 0x0000310E
		public int LineCount { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004F17 File Offset: 0x00003117
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00004F1F File Offset: 0x0000311F
		internal TextOutput TextOutput { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004F28 File Offset: 0x00003128
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004F30 File Offset: 0x00003130
		public int Width { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004F39 File Offset: 0x00003139
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004F41 File Offset: 0x00003141
		public int Height { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004F4A File Offset: 0x0000314A
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004F52 File Offset: 0x00003152
		public Font Font
		{
			get
			{
				return this._font;
			}
			set
			{
				if (this._font != value)
				{
					this._font = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004F6A File Offset: 0x0000316A
		private float ExtraPaddingHorizontal
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004F71 File Offset: 0x00003171
		private float ExtraPaddingVertical
		{
			get
			{
				return 5f;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004F78 File Offset: 0x00003178
		private int _textLength
		{
			get
			{
				return this._text.Length + this._numOfAddedSeparators;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004F8C File Offset: 0x0000318C
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00004F94 File Offset: 0x00003194
		public TextHorizontalAlignment HorizontalAlignment
		{
			get
			{
				return this._horizontalAlignment;
			}
			set
			{
				if (this._horizontalAlignment != value)
				{
					this._horizontalAlignment = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004FAC File Offset: 0x000031AC
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004FB4 File Offset: 0x000031B4
		public TextVerticalAlignment VerticalAlignment
		{
			get
			{
				return this._verticalAlignment;
			}
			set
			{
				if (this._verticalAlignment != value)
				{
					this._verticalAlignment = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004FCC File Offset: 0x000031CC
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00004FD5 File Offset: 0x000031D5
		public float FontSize
		{
			get
			{
				return (float)this._fontSize;
			}
			set
			{
				if (this._fontSize != (int)Mathf.Round(value))
				{
					this._fontSize = (int)Mathf.Round(value);
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004FF9 File Offset: 0x000031F9
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00005004 File Offset: 0x00003204
		public string Value
		{
			get
			{
				return this._text;
			}
			set
			{
				string text = value;
				if (text == null)
				{
					text = "";
				}
				if (this._text != text)
				{
					this._text = text;
					this._tokens = TextParser.Parse(text, this.CurrentLanguage);
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005049 File Offset: 0x00003249
		private float EmptyCharacterWidth
		{
			get
			{
				return ((float)this.Font.Characters[32].XAdvance + this.ExtraPaddingHorizontal) * this._scaleValue;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005071 File Offset: 0x00003271
		private float LineHeight
		{
			get
			{
				return ((float)this.Font.Base + this.ExtraPaddingVertical) * this._scaleValue;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000508D File Offset: 0x0000328D
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00005095 File Offset: 0x00003295
		public bool SkipLineOnContainerExceeded
		{
			get
			{
				return this._skipLineOnContainerExceeded;
			}
			set
			{
				if (value != this._skipLineOnContainerExceeded)
				{
					this._skipLineOnContainerExceeded = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000050AD File Offset: 0x000032AD
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x000050B5 File Offset: 0x000032B5
		public bool CanBreakWords
		{
			get
			{
				return this._canBreakWords;
			}
			set
			{
				if (value != this._canBreakWords)
				{
					this._canBreakWords = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000050CD File Offset: 0x000032CD
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x000050D5 File Offset: 0x000032D5
		public bool ResizeTextOnOverflow
		{
			get
			{
				return this._resizeTextOnOverflow;
			}
			set
			{
				if (value != this._resizeTextOnOverflow)
				{
					this._resizeTextOnOverflow = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000050F0 File Offset: 0x000032F0
		public Text(int width, int height, Font bitmapFont, Func<int, Font> getUsableFontForCharacter)
		{
			this.Font = bitmapFont;
			this.Width = width;
			this.Height = height;
			this._getUsableFontForCharacter = getUsableFontForCharacter;
			this._textParts = new List<TextPart>();
			this.SetAllDirty();
			this._text = "";
			this._fontSize = 32;
			this._tokens = null;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000516C File Offset: 0x0000336C
		public Vector2 GetPreferredSize(bool fixedWidth, float widthSize, bool fixedHeight, float heightSize, SpriteData spriteData, float renderScale)
		{
			this._fixedWidth = fixedWidth;
			this._fixedHeight = fixedHeight;
			this._desiredHeight = heightSize;
			this._desiredWidth = widthSize;
			if (this._preferredSizeNeedsUpdate)
			{
				this._preferredSize = new Vector2(0f, 0f);
				if (this._fontSize != 0 && !string.IsNullOrEmpty(this._text))
				{
					this._scaleValue = (float)this._fontSize / (float)this.Font.Size;
					float num = 0f;
					this.LineCount = 1;
					for (int i = 0; i < this._tokens.Count; i++)
					{
						TextToken textToken = this._tokens[i];
						if (textToken.Type != TextToken.TokenType.Tag)
						{
							if (textToken.Type == TextToken.TokenType.NewLine)
							{
								if (num > this._preferredSize.X)
								{
									this._preferredSize.X = num;
								}
								int num2 = this.LineCount;
								this.LineCount = num2 + 1;
								num = 0f;
							}
							else if (textToken.Type == TextToken.TokenType.EmptyCharacter || textToken.Type == TextToken.TokenType.NonBreakingSpace)
							{
								num += this.EmptyCharacterWidth;
							}
							else if (textToken.Type == TextToken.TokenType.Character)
							{
								char token = textToken.Token;
								float num3 = this.Font.GetCharacterWidth(token, this.ExtraPaddingHorizontal) * this._scaleValue;
								if (!this.Font.Characters.ContainsKey((int)token))
								{
									Font font = this._getUsableFontForCharacter((int)token) ?? this.Font;
									float num4 = (float)this._fontSize / (float)font.Size;
									num3 = font.GetCharacterWidth(token, this.ExtraPaddingHorizontal) * num4;
								}
								if (fixedWidth && this._skipLineOnContainerExceeded)
								{
									if (num + num3 > this._desiredWidth && num > 0f)
									{
										int indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex = TextHelper.GetIndexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex(this._tokens, i, this.CurrentLanguage, this.CanBreakWords);
										if (indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex != -1)
										{
											float totalWordWidthBetweenIndices = TextHelper.GetTotalWordWidthBetweenIndices(indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex, i, this._tokens, new Func<TextToken, Font>(this.GetFontForTextToken), this.ExtraPaddingHorizontal, (float)this._fontSize);
											num -= totalWordWidthBetweenIndices;
											if (num > this._preferredSize.X)
											{
												this._preferredSize.X = num;
											}
											int num2 = this.LineCount;
											this.LineCount = num2 + 1;
											num = totalWordWidthBetweenIndices + num3;
										}
										else if (this.CanBreakWords)
										{
											int num5 = Math.Max(0, this._tokens.Count - 2);
											int num6 = Math.Max(0, this._tokens.Count - 1);
											float totalWordWidthBetweenIndices2 = TextHelper.GetTotalWordWidthBetweenIndices(num5, num6, this._tokens, new Func<TextToken, Font>(this.GetFontForTextToken), this.ExtraPaddingHorizontal, (float)this._fontSize);
											num -= totalWordWidthBetweenIndices2;
											if (num > this._preferredSize.X)
											{
												this._preferredSize.X = num;
											}
											int num2 = this.LineCount;
											this.LineCount = num2 + 1;
											num = totalWordWidthBetweenIndices2 + num3;
										}
										else
										{
											num += num3;
										}
									}
									else
									{
										num += num3;
									}
								}
								else
								{
									num += num3;
								}
							}
						}
					}
					if (num > this._preferredSize.X)
					{
						this._preferredSize.X = num;
					}
					this._preferredSize.Y = (float)this.LineCount * this.LineHeight;
				}
				this._preferredSize = new Vector2((float)Math.Ceiling((double)this._preferredSize.X), (float)Math.Ceiling((double)this._preferredSize.Y));
				this._preferredSizeNeedsUpdate = false;
			}
			return this._preferredSize;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000054D0 File Offset: 0x000036D0
		public void UpdateSize(int width, int height)
		{
			if (this.Width != width || this.Height != height)
			{
				this.Width = width;
				this.Height = height;
				this.SetAllDirty();
				this.ScaleToFitTextInLayout = 1f;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005503 File Offset: 0x00003703
		public void SetAllDirty()
		{
			this._meshNeedsUpdate = true;
			this._preferredSizeNeedsUpdate = true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005513 File Offset: 0x00003713
		private Font GetFontForTextToken(TextToken token)
		{
			if (this.Font.Characters.ContainsKey((int)token.Token))
			{
				return this.Font;
			}
			return this._getUsableFontForCharacter((int)token.Token);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005548 File Offset: 0x00003748
		private void UpdateMesh()
		{
			this.RecalculateTextMesh((float)this._fontSize);
			if (!this.ResizeTextOnOverflow)
			{
				return;
			}
			float num = (float)this._fontSize;
			int num2 = 0;
			while (this.ScaleToFitTextInLayout < 0.9f && num2 < 3)
			{
				num2++;
				num *= MathF.Sqrt(this.ScaleToFitTextInLayout);
				this.RecalculateTextMesh(num);
			}
			if (this.ScaleToFitTextInLayout != 1f)
			{
				num *= this.ScaleToFitTextInLayout;
				this.RecalculateTextMesh(num);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000055C0 File Offset: 0x000037C0
		private void RecalculateTextMesh(float desiredFontSize)
		{
			this._textParts.Clear();
			TextOutput textOutput = this.TextOutput;
			if (textOutput != null)
			{
				textOutput.Clear();
			}
			this._numOfAddedSeparators = 0;
			if (desiredFontSize != 0f && !string.IsNullOrEmpty(this._text))
			{
				this._scaleValue = desiredFontSize / (float)this.Font.Size;
				this.TextOutput = new TextOutput(this.LineHeight);
				for (int i = 0; i < this._tokens.Count; i++)
				{
					TextToken textToken = this._tokens[i];
					if (textToken.Type == TextToken.TokenType.NewLine)
					{
						this.TextOutput.AddNewLine(true, 0f);
					}
					else if (textToken.Type == TextToken.TokenType.EmptyCharacter || textToken.Type == TextToken.TokenType.NonBreakingSpace)
					{
						this.TextOutput.AddToken(textToken, this.EmptyCharacterWidth, this._scaleValue, "Default", -1f);
					}
					else if (textToken.Type != TextToken.TokenType.ZeroWidthSpace)
					{
						if (textToken.Type == TextToken.TokenType.WordJoiner)
						{
							this.TextOutput.AddToken(textToken, 0f, this._scaleValue, "Default", -1f);
						}
						else if (textToken.Type == TextToken.TokenType.Character)
						{
							char token = textToken.Token;
							float num = this.Font.GetCharacterWidth(token, this.ExtraPaddingHorizontal) * this._scaleValue;
							if (!this.Font.Characters.ContainsKey((int)token))
							{
								Font font = this._getUsableFontForCharacter((int)token) ?? this.Font;
								float num2 = desiredFontSize / (float)font.Size;
								num = font.GetCharacterWidth(token, this.ExtraPaddingHorizontal) * num2;
							}
							bool flag = this.TextOutput.LastLineWidth + num > (float)this.Width;
							if (this._fixedWidth && flag && this.SkipLineOnContainerExceeded)
							{
								int indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex = TextHelper.GetIndexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex(this._tokens, i, this.CurrentLanguage, this.CanBreakWords);
								int num3 = TextHelper.GetIndexOfFirstAppropriateCharacterToMoveToNextLineForwardsFromIndex(this._tokens, i, this.CurrentLanguage, this.CanBreakWords);
								float num4 = 0f;
								if (indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex != -1)
								{
									if (num3 == -1)
									{
										num3 = this._tokens.Count;
									}
									num4 = TextHelper.GetTotalWordWidthBetweenIndices(indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex, num3, this._tokens, new Func<TextToken, Font>(this.GetFontForTextToken), this.ExtraPaddingHorizontal, desiredFontSize);
								}
								bool flag2 = num <= (float)this.Width;
								bool flag3 = flag2 && (num4 == 0f || (indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex != -1 && num4 <= (float)this.Width));
								if (this.CanBreakWords && (!flag3 || indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex == -1))
								{
									float num5 = this.Font.GetCharacterWidth(this.CurrentLanguage.GetLineSeperatorChar(), this.ExtraPaddingHorizontal) * this._scaleValue;
									if (!flag2)
									{
										this.TextOutput.AddToken(textToken, num, this._scaleValue, "Default", -1f);
										if (i != this._tokens.Count - 1)
										{
											if (this._tokens[i + 1].Type == TextToken.TokenType.Character && !TextHelper.IsTokenEqualToSeparatorChar(textToken, this.CurrentLanguage) && !TextHelper.IsTokenEqualToSeparatorChar(this._tokens[i + 1], this.CurrentLanguage))
											{
												this._numOfAddedSeparators++;
												this.TextOutput.AddToken(TextToken.CreateCharacter(this.CurrentLanguage.GetLineSeperatorChar()), num5, this._scaleValue, "Default", -1f);
											}
											this.TextOutput.AddNewLine(false, 0f);
										}
									}
									else if (this.TextOutput.Tokens.Any<TextTokenOutput>())
									{
										TextTokenOutput textTokenOutput = this.TextOutput.Tokens.LastOrDefault<TextTokenOutput>();
										if (textTokenOutput == null || textTokenOutput.Token.Type != TextToken.TokenType.Character)
										{
											goto IL_03E7;
										}
										TextTokenOutput textTokenOutput2 = this.TextOutput.Tokens.LastOrDefault<TextTokenOutput>();
										if (TextHelper.IsTokenEqualToSeparatorChar((textTokenOutput2 != null) ? textTokenOutput2.Token : null, this.CurrentLanguage))
										{
											goto IL_03E7;
										}
										int num6 = (TextHelper.IsTokenEqualToSeparatorChar(textToken, this.CurrentLanguage) ? 1 : 0);
										IL_03E8:
										bool flag4 = this.TextOutput.LastLineWidth + num5 > (float)this.Width;
										TextTokenOutput textTokenOutput3 = null;
										if (num6 == 0 && flag4)
										{
											textTokenOutput3 = this.TextOutput.RemoveTokensFromEnd(1).First<TextTokenOutput>();
										}
										TextToken textToken2 = ((textTokenOutput3 != null) ? textTokenOutput3.Token : null) ?? textToken;
										TextTokenOutput textTokenOutput4 = this.TextOutput.Tokens.LastOrDefault<TextTokenOutput>();
										if (textTokenOutput4 == null || textTokenOutput4.Token.Type != TextToken.TokenType.Character)
										{
											goto IL_0495;
										}
										TextTokenOutput textTokenOutput5 = this.TextOutput.Tokens.LastOrDefault<TextTokenOutput>();
										if (TextHelper.IsTokenEqualToSeparatorChar((textTokenOutput5 != null) ? textTokenOutput5.Token : null, this.CurrentLanguage))
										{
											goto IL_0495;
										}
										bool flag5 = TextHelper.IsTokenEqualToSeparatorChar(textToken2, this.CurrentLanguage);
										IL_0496:
										if (!flag5)
										{
											this._numOfAddedSeparators++;
											this.TextOutput.AddToken(TextToken.CreateCharacter(this.CurrentLanguage.GetLineSeperatorChar()), num5, this._scaleValue, "Default", -1f);
										}
										this.TextOutput.AddNewLine(false, 0f);
										if (textTokenOutput3 != null)
										{
											this.TextOutput.AddToken(textTokenOutput3.Token, textTokenOutput3.Width, textTokenOutput3.Scale, "Default", -1f);
										}
										this.TextOutput.AddToken(textToken, num, this._scaleValue, "Default", -1f);
										goto IL_0608;
										IL_0495:
										flag5 = true;
										goto IL_0496;
										IL_03E7:
										num6 = 1;
										goto IL_03E8;
									}
								}
								else
								{
									if (indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex != -1)
									{
										List<TextTokenOutput> list = this.TextOutput.RemoveTokensFromEnd(i - indexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex);
										this.TextOutput.AddNewLine(false, 0f);
										for (int j = list.Count - 1; j >= 0; j--)
										{
											TextTokenOutput textTokenOutput6 = list[j];
											if (textTokenOutput6.Token.Type != TextToken.TokenType.EmptyCharacter && textTokenOutput6.Token.Type != TextToken.TokenType.ZeroWidthSpace)
											{
												this.TextOutput.AddToken(textTokenOutput6.Token, textTokenOutput6.Width, this._scaleValue, "Default", -1f);
											}
										}
									}
									this.TextOutput.AddToken(textToken, num, this._scaleValue, "Default", -1f);
								}
							}
							else
							{
								this.TextOutput.AddToken(textToken, num, this._scaleValue, "Default", -1f);
							}
						}
					}
					IL_0608:;
				}
				float num7 = 0f;
				float num8 = 0f;
				for (int k = 0; k < this.TextOutput.LineCount; k++)
				{
					num7 = 0f;
					TextLineOutput line = this.TextOutput.GetLine(k);
					float num9 = this.EmptyCharacterWidth;
					switch (this._horizontalAlignment)
					{
					case TextHorizontalAlignment.Right:
						num7 = (float)this.Width - line.Width;
						break;
					case TextHorizontalAlignment.Center:
					{
						float num10 = 0f;
						if (!line.LineEnded)
						{
							int num11 = 1;
							while (num11 < line.TokenCount && line.GetToken(line.TokenCount - num11).Type == TextToken.TokenType.EmptyCharacter)
							{
								num10 += this.EmptyCharacterWidth;
								num11++;
							}
							num11 = 0;
							while (num11 < line.TokenCount && line.GetToken(num11).Type == TextToken.TokenType.EmptyCharacter)
							{
								num10 += this.EmptyCharacterWidth;
								num11++;
							}
						}
						num7 = ((float)this.Width - (line.Width - num10)) * 0.5f;
						break;
					}
					case TextHorizontalAlignment.Justify:
					{
						float num12 = (float)this.Width - line.TextWidth;
						if (!line.LineEnded)
						{
							int num13 = line.EmptyCharacterCount;
							int num14 = 1;
							while (line.GetToken(line.TokenCount - num14).Type == TextToken.TokenType.EmptyCharacter)
							{
								num13--;
								num14++;
							}
							num14 = 0;
							while (line.GetToken(num14).Type == TextToken.TokenType.EmptyCharacter)
							{
								num13--;
								num14++;
							}
							num9 = num12 / (float)num13;
						}
						break;
					}
					}
					for (int l = 0; l < line.TokenCount; l++)
					{
						Font font2 = this.Font;
						TextToken token2 = line.GetToken(l);
						TextToken.TokenType type = token2.Type;
						if (type != TextToken.TokenType.EmptyCharacter && type != TextToken.TokenType.NonBreakingSpace)
						{
							if (type == TextToken.TokenType.Character)
							{
								int num15 = (int)token2.Token;
								float num16 = this._scaleValue;
								if (!this.Font.Characters.ContainsKey(num15))
								{
									font2 = this._getUsableFontForCharacter(num15);
									if (font2 == null)
									{
										font2 = this.Font;
										num15 = 0;
									}
									else
									{
										num16 = desiredFontSize / (float)font2.Size;
									}
								}
								TextPart orCreateTextPart = this.GetOrCreateTextPart(font2, num7, num8, desiredFontSize);
								BitmapFontCharacter bitmapFontCharacter = font2.Characters[num15];
								float num17 = num7 + (float)bitmapFontCharacter.XOffset * this._scaleValue;
								float num18 = num8 + (float)bitmapFontCharacter.YOffset * this._scaleValue;
								orCreateTextPart.TextMeshGenerator.AddCharacterToMesh(num17, num18, bitmapFontCharacter);
								orCreateTextPart.WordWidth += ((float)bitmapFontCharacter.XAdvance + this.ExtraPaddingHorizontal) * num16;
								num7 += ((float)bitmapFontCharacter.XAdvance + this.ExtraPaddingHorizontal) * num16;
							}
						}
						else
						{
							this.GetOrCreateTextPart(font2, num7, num8, desiredFontSize).WordWidth += num9;
							num7 += num9;
						}
					}
					num8 += this.LineHeight;
				}
				if (this._verticalAlignment == TextVerticalAlignment.Center || this._verticalAlignment == TextVerticalAlignment.Bottom)
				{
					float extraY;
					if (this._verticalAlignment == TextVerticalAlignment.Center)
					{
						extraY = (float)this.Height - num8;
						extraY *= 0.5f;
					}
					else
					{
						extraY = (float)this.Height - num8;
					}
					this._textParts.ForEach(delegate(TextPart x)
					{
						x.TextMeshGenerator.AddValueToY(extraY);
					});
				}
				this.GenerateMeshes();
				this.ScaleToFitTextInLayout = 1f;
				if (this._fixedHeight && num8 > this._desiredHeight && this._desiredHeight > 1f)
				{
					this.ScaleToFitTextInLayout = this._desiredHeight / num8;
				}
				if (this._fixedWidth && num7 > this._desiredWidth && this._desiredWidth > 1f)
				{
					this.ScaleToFitTextInLayout = Math.Min(this.ScaleToFitTextInLayout, this._desiredWidth / num7);
					return;
				}
			}
			else
			{
				this.ScaleToFitTextInLayout = 1f;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005FB4 File Offset: 0x000041B4
		private void GenerateMeshes()
		{
			for (int i = 0; i < this._textParts.Count; i++)
			{
				TextPart textPart = this._textParts[i];
				textPart.DrawObject2D = textPart.TextMeshGenerator.GenerateMesh();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005FF4 File Offset: 0x000041F4
		private TextPart GetOrCreateTextPart(Font font, float x, float y, float fontSize)
		{
			TextPart textPart = this._textParts.LastOrDefault<TextPart>();
			if (textPart != null && textPart.DefaultFont == font)
			{
				return textPart;
			}
			float num = fontSize / (float)font.Size;
			TextMeshGenerator textMeshGenerator = new TextMeshGenerator();
			textMeshGenerator.Refresh(font, this._textLength, num);
			TextPart textPart2 = new TextPart
			{
				TextMeshGenerator = textMeshGenerator,
				WordWidth = 0f,
				PartPosition = new Vector2(x, y),
				DefaultFont = font
			};
			this._textParts.Add(textPart2);
			return textPart2;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006073 File Offset: 0x00004273
		public List<TextPart> GetParts()
		{
			if (this._meshNeedsUpdate)
			{
				this.UpdateMesh();
				this._meshNeedsUpdate = false;
			}
			return this._textParts;
		}

		// Token: 0x0400005E RID: 94
		private TextHorizontalAlignment _horizontalAlignment;

		// Token: 0x0400005F RID: 95
		private TextVerticalAlignment _verticalAlignment;

		// Token: 0x04000060 RID: 96
		private bool _meshNeedsUpdate;

		// Token: 0x04000061 RID: 97
		private bool _preferredSizeNeedsUpdate;

		// Token: 0x04000062 RID: 98
		private bool _fixedHeight;

		// Token: 0x04000063 RID: 99
		private bool _fixedWidth;

		// Token: 0x04000064 RID: 100
		private float _desiredHeight;

		// Token: 0x04000065 RID: 101
		private float _desiredWidth;

		// Token: 0x04000066 RID: 102
		private Vector2 _preferredSize;

		// Token: 0x04000067 RID: 103
		private string _text;

		// Token: 0x04000068 RID: 104
		private List<TextToken> _tokens;

		// Token: 0x04000069 RID: 105
		private List<TextPart> _textParts;

		// Token: 0x0400006B RID: 107
		private int _fontSize;

		// Token: 0x0400006E RID: 110
		private Font _font;

		// Token: 0x0400006F RID: 111
		private float _scaleValue;

		// Token: 0x04000070 RID: 112
		private int _numOfAddedSeparators;

		// Token: 0x04000071 RID: 113
		private readonly Func<int, Font> _getUsableFontForCharacter;

		// Token: 0x04000072 RID: 114
		private bool _skipLineOnContainerExceeded = true;

		// Token: 0x04000073 RID: 115
		private bool _resizeTextOnOverflow = true;

		// Token: 0x04000074 RID: 116
		private bool _canBreakWords = true;
	}
}
