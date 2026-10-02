using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000018 RID: 24
	public class TextToken
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006C5E File Offset: 0x00004E5E
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00006C66 File Offset: 0x00004E66
		public char Token { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006C6F File Offset: 0x00004E6F
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00006C77 File Offset: 0x00004E77
		public TextToken.TokenType Type { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00006C80 File Offset: 0x00004E80
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00006C88 File Offset: 0x00004E88
		public RichTextTag Tag { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00006C91 File Offset: 0x00004E91
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00006C99 File Offset: 0x00004E99
		public bool CannotStartLineWithCharacter { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00006CA2 File Offset: 0x00004EA2
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00006CAA File Offset: 0x00004EAA
		public bool CannotEndLineWithCharacter { get; set; }

		// Token: 0x06000101 RID: 257 RVA: 0x00006CB3 File Offset: 0x00004EB3
		private TextToken(TextToken.TokenType type, char token)
		{
			this.Type = type;
			this.Token = token;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006CC9 File Offset: 0x00004EC9
		private TextToken(RichTextTag tag)
		{
			this.Type = TextToken.TokenType.Tag;
			this.Tag = tag;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006CDF File Offset: 0x00004EDF
		public static TextToken CreateEmptyCharacter()
		{
			return new TextToken(TextToken.TokenType.EmptyCharacter, ' ');
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006CE9 File Offset: 0x00004EE9
		public static TextToken CreateZeroWidthSpaceCharacter()
		{
			return new TextToken(TextToken.TokenType.ZeroWidthSpace, '\0');
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006CF2 File Offset: 0x00004EF2
		public static TextToken CreateNonBreakingSpaceCharacter()
		{
			return new TextToken(TextToken.TokenType.NonBreakingSpace, ' ');
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006CFC File Offset: 0x00004EFC
		public static TextToken CreateWordJoinerCharacter()
		{
			return new TextToken(TextToken.TokenType.WordJoiner, Convert.ToChar(8288));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006D0E File Offset: 0x00004F0E
		public static TextToken CreateNewLine()
		{
			return new TextToken(TextToken.TokenType.NewLine, '\n');
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006D18 File Offset: 0x00004F18
		public static TextToken CreateTab()
		{
			return new TextToken(TextToken.TokenType.Tab, '\t');
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006D22 File Offset: 0x00004F22
		public static TextToken CreateCharacter(char character)
		{
			return new TextToken(TextToken.TokenType.Character, character);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006D2B File Offset: 0x00004F2B
		public static TextToken CreateTag(RichTextTag tag)
		{
			return new TextToken(tag);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006D33 File Offset: 0x00004F33
		public static TextToken CreateCharacterCannotEndLineWith(char character)
		{
			return new TextToken(TextToken.TokenType.Character, character)
			{
				CannotEndLineWithCharacter = true
			};
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006D43 File Offset: 0x00004F43
		public static TextToken CreateCharacterCannotStartLineWith(char character)
		{
			return new TextToken(TextToken.TokenType.Character, character)
			{
				CannotStartLineWithCharacter = true
			};
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006D54 File Offset: 0x00004F54
		public static List<TextToken> CreateTokenArrayFromWord(string word)
		{
			List<TextToken> list = new List<TextToken>();
			foreach (char c in word)
			{
				list.Add(TextToken.CreateCharacter(c));
			}
			return list;
		}

		// Token: 0x02000041 RID: 65
		public enum TokenType
		{
			// Token: 0x04000157 RID: 343
			EmptyCharacter,
			// Token: 0x04000158 RID: 344
			ZeroWidthSpace,
			// Token: 0x04000159 RID: 345
			NonBreakingSpace,
			// Token: 0x0400015A RID: 346
			WordJoiner,
			// Token: 0x0400015B RID: 347
			NewLine,
			// Token: 0x0400015C RID: 348
			Tab,
			// Token: 0x0400015D RID: 349
			Character,
			// Token: 0x0400015E RID: 350
			Tag
		}
	}
}
