using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x0200002B RID: 43
	public abstract class LanguageSpecificTextProcessor
	{
		// Token: 0x06000124 RID: 292
		public abstract void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString);

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000125 RID: 293
		public abstract CultureInfo CultureInfoForLanguage { get; }

		// Token: 0x06000126 RID: 294
		public abstract void ClearTemporaryData();

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000620C File Offset: 0x0000440C
		private static List<int> LowerMarkers
		{
			get
			{
				if (LanguageSpecificTextProcessor._lowerMarkers == null)
				{
					LanguageSpecificTextProcessor._lowerMarkers = new List<int>();
				}
				return LanguageSpecificTextProcessor._lowerMarkers;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006224 File Offset: 0x00004424
		public LanguageSpecificTextProcessor()
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000622C File Offset: 0x0000442C
		public string Process(string text)
		{
			if (text == null)
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '{')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return text;
			}
			List<int> lowerMarkers = LanguageSpecificTextProcessor._lowerMarkers;
			LanguageSpecificTextProcessor._lowerMarkers = null;
			StringBuilder stringBuilder = new StringBuilder();
			int j = 0;
			while (j < text.Length)
			{
				if (text[j] != '{')
				{
					stringBuilder.Append(text[j]);
					j++;
				}
				else
				{
					string text2 = LanguageSpecificTextProcessor.ReadFirstToken(text, ref j);
					if (LanguageSpecificTextProcessor.IsPostProcessToken(text2))
					{
						this.ProcessTokenInternal(text, ref j, text2, stringBuilder);
					}
				}
			}
			this.ProcessLowerCaseMarkers(stringBuilder);
			LanguageSpecificTextProcessor._lowerMarkers = lowerMarkers;
			return stringBuilder.ToString();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000062DC File Offset: 0x000044DC
		private void ProcessTokenInternal(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
			CultureInfo cultureInfoForLanguage = this.CultureInfoForLanguage;
			char c = token[1];
			if (c == '^' && token.Length == 2)
			{
				int num = LanguageSpecificTextProcessor.FindNextLetter(sourceText, cursorPos);
				if (num > cursorPos && num < sourceText.Length)
				{
					outputString.Append(sourceText.Substring(cursorPos, num - cursorPos));
				}
				if (num < sourceText.Length)
				{
					outputString.Append(char.ToUpper(sourceText[num], cultureInfoForLanguage));
					cursorPos = num + 1;
					return;
				}
			}
			else if (c == '_' && token.Length == 2)
			{
				int num2 = LanguageSpecificTextProcessor.FindNextLetter(sourceText, cursorPos);
				if (num2 > cursorPos && num2 < sourceText.Length)
				{
					outputString.Append(sourceText.Substring(cursorPos, num2 - cursorPos));
				}
				if (num2 < sourceText.Length)
				{
					outputString.Append(char.ToLower(sourceText[num2], cultureInfoForLanguage));
					cursorPos = num2 + 1;
					return;
				}
			}
			else
			{
				if (c == '%' && token.Length == 2)
				{
					LanguageSpecificTextProcessor.LowerMarkers.Add(outputString.Length - 1);
					return;
				}
				this.ProcessToken(sourceText, ref cursorPos, token, outputString);
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000063E4 File Offset: 0x000045E4
		private void ProcessLowerCaseMarkers(StringBuilder stringBuilder)
		{
			if (LanguageSpecificTextProcessor.LowerMarkers.Count > 0)
			{
				for (int i = 0; i < LanguageSpecificTextProcessor.LowerMarkers.Count; i += 2)
				{
					int num = LanguageSpecificTextProcessor.LowerMarkers[i];
					if (i + 1 < LanguageSpecificTextProcessor.LowerMarkers.Count)
					{
						int num2 = LanguageSpecificTextProcessor.LowerMarkers[i + 1];
						if (num != num2)
						{
							if (num > stringBuilder.Length)
							{
								num = -1;
							}
							int num3 = Math.Min(num2 - num, stringBuilder.Length - num - 1);
							string text = stringBuilder.ToString(num + 1, num3);
							stringBuilder = stringBuilder.Remove(num + 1, num3).Insert(num + 1, text.ToLower());
						}
					}
					else
					{
						if (num > stringBuilder.Length)
						{
							num = -1;
						}
						if (num + 1 < stringBuilder.Length)
						{
							string text2 = stringBuilder.ToString(num + 1, stringBuilder.Length - num - 1);
							stringBuilder = stringBuilder.Remove(num + 1, stringBuilder.Length - num - 1).Insert(num + 1, text2.ToLower());
						}
					}
				}
				LanguageSpecificTextProcessor.LowerMarkers.Clear();
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000064EC File Offset: 0x000046EC
		private static int FindNextLetter(string sourceText, int cursorPos)
		{
			int i = cursorPos;
			if (sourceText.Length > i + "<a style=\"Link.".Length && sourceText.Substring(i, "<a style=\"Link.".Length).Equals("<a style=\"Link."))
			{
				i += "<a style=\"Link.".Length;
				while (sourceText[i++] != '>')
				{
				}
			}
			while (i < sourceText.Length)
			{
				if (sourceText[i] == '<')
				{
					i += 2;
				}
				if (char.IsLetter(sourceText, i))
				{
					return i;
				}
				i++;
			}
			return i;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006572 File Offset: 0x00004772
		private static bool IsPostProcessToken(string token)
		{
			return token.Length > 1 && token[0] == '.';
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000658C File Offset: 0x0000478C
		private static string ReadFirstToken(string text, ref int i)
		{
			int num = i;
			while (i < text.Length && text[i] != '}')
			{
				i++;
			}
			int num2 = i - num - 1;
			if (i < text.Length)
			{
				i++;
			}
			return text.Substring(num + 1, num2);
		}

		// Token: 0x04000065 RID: 101
		[ThreadStatic]
		private static List<int> _lowerMarkers;
	}
}
