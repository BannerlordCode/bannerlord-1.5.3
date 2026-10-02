using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TaleWorlds.Localization.TextProcessor.LanguageProcessors
{
	// Token: 0x02000036 RID: 54
	public class SpanishTextProcessor : LanguageSpecificTextProcessor
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00018834 File Offset: 0x00016A34
		public override void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
			if (SpanishTextProcessor.GenderTokens.TokenList.Contains(token))
			{
				this.SetGender(token);
			}
			if (token == ".l" || token == ".L")
			{
				this.HandleDefiniteArticles(sourceText, token, cursorPos, outputString);
				SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.NoDeclination;
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00018881 File Offset: 0x00016A81
		private bool CheckWhiteSpaceAndTextEnd(string sourceText, int cursorPos)
		{
			return cursorPos < sourceText.Length && !char.IsWhiteSpace(sourceText[cursorPos]);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000188A0 File Offset: 0x00016AA0
		private void SetGender(string token)
		{
			if (token == ".MS")
			{
				SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.MasculineSingular;
				return;
			}
			if (token == ".MP")
			{
				SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.MasculinePlural;
				return;
			}
			if (token == ".FS")
			{
				SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.FeminineSingular;
				return;
			}
			if (token == ".FP")
			{
				SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.FemininePlural;
				return;
			}
			if (token == ".NS")
			{
				SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.NeuterSingular;
				return;
			}
			if (!(token == ".NP"))
			{
				return;
			}
			SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.NeuterPlural;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00018928 File Offset: 0x00016B28
		private void HandleDefiniteArticles(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			if (!this.CheckWhiteSpaceAndTextEnd(text, cursorPos))
			{
				return;
			}
			if (SpanishTextProcessor._curGender == SpanishTextProcessor.WordGenderEnum.MasculineSingular || SpanishTextProcessor._curGender == SpanishTextProcessor.WordGenderEnum.MasculinePlural || SpanishTextProcessor._curGender == SpanishTextProcessor.WordGenderEnum.FeminineSingular || SpanishTextProcessor._curGender == SpanishTextProcessor.WordGenderEnum.FemininePlural)
			{
				string text2 = SpanishTextProcessor._genderToDefiniteArticle[SpanishTextProcessor._curGender];
				bool flag = false;
				string text3;
				if (SpanishTextProcessor._curGender == SpanishTextProcessor.WordGenderEnum.MasculineSingular && this.HandleContractions(text, text2, cursorPos, out text3))
				{
					text2 = text3;
					flag = true;
					if (char.IsWhiteSpace(stringBuilder[stringBuilder.Length - 1]))
					{
						stringBuilder.Remove(stringBuilder.Length - 1, 1);
					}
				}
				if (!flag && token == ".L")
				{
					text2 = char.ToUpper(text2[0]).ToString() + text2.Substring(1);
				}
				stringBuilder.Append(text2);
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000189F0 File Offset: 0x00016BF0
		private bool HandleContractions(string text, string article, int cursorPos, out string newVersion)
		{
			string previousWord = this.GetPreviousWord(text, cursorPos);
			Dictionary<string, string> dictionary;
			if (SpanishTextProcessor.Contractions.TryGetValue(previousWord.ToLower(), out dictionary) && dictionary.TryGetValue(article.TrimEnd(Array.Empty<char>()), out newVersion))
			{
				return true;
			}
			newVersion = string.Empty;
			return false;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00018A3C File Offset: 0x00016C3C
		private string GetPreviousWord(string sourceText, int cursorPos)
		{
			string[] array = sourceText.Substring(0, cursorPos).Split(new char[] { ' ' });
			int num = array.Length;
			if (num < 2)
			{
				return "";
			}
			return array[num - 2];
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00018A75 File Offset: 0x00016C75
		public override CultureInfo CultureInfoForLanguage
		{
			get
			{
				return SpanishTextProcessor.CultureInfo;
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00018A7C File Offset: 0x00016C7C
		public override void ClearTemporaryData()
		{
			SpanishTextProcessor._curGender = SpanishTextProcessor.WordGenderEnum.NoDeclination;
		}

		// Token: 0x040000F7 RID: 247
		[ThreadStatic]
		private static SpanishTextProcessor.WordGenderEnum _curGender;

		// Token: 0x040000F8 RID: 248
		private static readonly Dictionary<string, Dictionary<string, string>> Contractions = new Dictionary<string, Dictionary<string, string>>
		{
			{
				"de",
				new Dictionary<string, string> { { "el", "l " } }
			},
			{
				"a",
				new Dictionary<string, string> { { "el", "l " } }
			}
		};

		// Token: 0x040000F9 RID: 249
		private static Dictionary<SpanishTextProcessor.WordGenderEnum, string> _genderToDefiniteArticle = new Dictionary<SpanishTextProcessor.WordGenderEnum, string>
		{
			{
				SpanishTextProcessor.WordGenderEnum.MasculineSingular,
				"el "
			},
			{
				SpanishTextProcessor.WordGenderEnum.MasculinePlural,
				"los "
			},
			{
				SpanishTextProcessor.WordGenderEnum.FeminineSingular,
				"la "
			},
			{
				SpanishTextProcessor.WordGenderEnum.FemininePlural,
				"las "
			},
			{
				SpanishTextProcessor.WordGenderEnum.NeuterSingular,
				""
			},
			{
				SpanishTextProcessor.WordGenderEnum.NeuterPlural,
				""
			}
		};

		// Token: 0x040000FA RID: 250
		private static readonly CultureInfo CultureInfo = new CultureInfo("es-es");

		// Token: 0x0200005C RID: 92
		private enum WordGenderEnum
		{
			// Token: 0x04000234 RID: 564
			MasculineSingular,
			// Token: 0x04000235 RID: 565
			MasculinePlural,
			// Token: 0x04000236 RID: 566
			FeminineSingular,
			// Token: 0x04000237 RID: 567
			FemininePlural,
			// Token: 0x04000238 RID: 568
			NeuterSingular,
			// Token: 0x04000239 RID: 569
			NeuterPlural,
			// Token: 0x0400023A RID: 570
			NoDeclination
		}

		// Token: 0x0200005D RID: 93
		private static class GenderTokens
		{
			// Token: 0x0400023B RID: 571
			public const string MasculineSingular = ".MS";

			// Token: 0x0400023C RID: 572
			public const string MasculinePlural = ".MP";

			// Token: 0x0400023D RID: 573
			public const string FeminineSingular = ".FS";

			// Token: 0x0400023E RID: 574
			public const string FemininePlural = ".FP";

			// Token: 0x0400023F RID: 575
			public const string NeuterSingular = ".NS";

			// Token: 0x04000240 RID: 576
			public const string NeuterPlural = ".NP";

			// Token: 0x04000241 RID: 577
			public static readonly List<string> TokenList = new List<string> { ".MS", ".FS", ".NS", ".MP", ".FP", ".NP" };
		}

		// Token: 0x0200005E RID: 94
		private static class FunctionTokens
		{
			// Token: 0x04000242 RID: 578
			public const string DefiniteArticle = ".l";

			// Token: 0x04000243 RID: 579
			public const string DefiniteArticleInUpperCase = ".L";
		}
	}
}
