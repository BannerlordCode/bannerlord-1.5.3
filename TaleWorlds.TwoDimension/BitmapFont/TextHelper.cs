using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension.BitmapFont
{
	// Token: 0x0200003A RID: 58
	internal static class TextHelper
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x00009E8C File Offset: 0x0000808C
		internal static int GetIndexOfFirstAppropriateCharacterToMoveToNextLineBackwardsFromIndex(List<TextToken> tokens, int startIndex, ILanguage currentLanguage, bool canBreakInZeroWidthSpace = true)
		{
			if (!currentLanguage.DoesLanguageRequireSpaceForNewline())
			{
				for (int i = startIndex; i >= 1; i--)
				{
					if (!currentLanguage.IsCharacterForbiddenAtEndOfLine(tokens[i - 1].Token) && !currentLanguage.IsCharacterForbiddenAtStartOfLine(tokens[i].Token))
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j >= 0; j--)
				{
					if (tokens[j].Type == TextToken.TokenType.EmptyCharacter)
					{
						return j + 1;
					}
					if (canBreakInZeroWidthSpace && tokens[j].Type == TextToken.TokenType.ZeroWidthSpace)
					{
						return j + 1;
					}
				}
			}
			return -1;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009F10 File Offset: 0x00008110
		internal static int GetIndexOfFirstAppropriateCharacterToMoveToNextLineForwardsFromIndex(List<TextToken> tokens, int startIndex, ILanguage currentLanguage, bool canBreakInZeroWidthSpace = true)
		{
			if (!currentLanguage.DoesLanguageRequireSpaceForNewline())
			{
				for (int i = startIndex; i < tokens.Count; i++)
				{
					if (i > 0 && !currentLanguage.IsCharacterForbiddenAtEndOfLine(tokens[i - 1].Token) && !currentLanguage.IsCharacterForbiddenAtStartOfLine(tokens[i].Token))
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j < tokens.Count; j++)
				{
					if (tokens[j].Type == TextToken.TokenType.EmptyCharacter)
					{
						return j + 1;
					}
					if (canBreakInZeroWidthSpace && tokens[j].Type == TextToken.TokenType.ZeroWidthSpace)
					{
						return j + 1;
					}
				}
			}
			return -1;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00009FA4 File Offset: 0x000081A4
		internal static float GetTotalWordWidthBetweenIndices(int startIndex, int endIndex, List<TextToken> tokens, Func<TextToken, Font> getFontForToken, float extraPadding, float requiredFontSize)
		{
			float num = 0f;
			for (int i = startIndex; i < endIndex; i++)
			{
				Font font = getFontForToken(tokens[i]);
				if (font != null)
				{
					float num2 = requiredFontSize / (float)font.Size;
					num += font.GetCharacterWidth(tokens[i].Token, extraPadding) * num2;
				}
			}
			return num;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00009FF9 File Offset: 0x000081F9
		internal static bool IsTokenEqualToSeparatorChar(TextToken token, ILanguage currentLanguage)
		{
			return token != null && token.Type == TextToken.TokenType.Character && token.Token == currentLanguage.GetLineSeperatorChar();
		}
	}
}
