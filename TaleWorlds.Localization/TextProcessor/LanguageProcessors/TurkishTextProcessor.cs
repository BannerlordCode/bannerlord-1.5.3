using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TaleWorlds.Localization.TextProcessor.LanguageProcessors
{
	// Token: 0x02000037 RID: 55
	public class TurkishTextProcessor : LanguageSpecificTextProcessor
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00018B44 File Offset: 0x00016D44
		public static List<string> LinkList
		{
			get
			{
				if (TurkishTextProcessor._linkList == null)
				{
					TurkishTextProcessor._linkList = new List<string>();
				}
				return TurkishTextProcessor._linkList;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00018B5C File Offset: 0x00016D5C
		private bool IsVowel(char c)
		{
			return TurkishTextProcessor.Vowels.Contains(char.ToLower(c, this.CultureInfoForLanguage));
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00018B74 File Offset: 0x00016D74
		private char GetNextVowel(StringBuilder stringBuilder)
		{
			string lastWord = this.GetLastWord(stringBuilder);
			char lastVowel;
			if (lastWord != null && TurkishTextProcessor._exceptions.TryGetValue(lastWord.ToLower(this.CultureInfoForLanguage), out lastVowel))
			{
				return lastVowel;
			}
			int num;
			if (int.TryParse(lastWord, out num))
			{
				return this.GetNextVowel(num);
			}
			lastVowel = this.GetLastVowel(stringBuilder);
			if (!TurkishTextProcessor.BackVowels.Contains(char.ToLower(lastVowel, this.CultureInfoForLanguage)))
			{
				return 'e';
			}
			return 'a';
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00018BE0 File Offset: 0x00016DE0
		private char GetNextVowel(int number)
		{
			int num = Math.Abs(number) % 10;
			int num2 = Math.Abs(number) % 100;
			if (number == 0)
			{
				return 'a';
			}
			if (num != 0)
			{
				if (!TurkishTextProcessor.BackNumbers.Contains(num))
				{
					return 'e';
				}
				return 'a';
			}
			else
			{
				if (num2 == 0)
				{
					return 'e';
				}
				if (!TurkishTextProcessor.BackNumbers.Contains(num2))
				{
					return 'e';
				}
				return 'a';
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00018C35 File Offset: 0x00016E35
		private bool IsFrontVowel(char c)
		{
			return TurkishTextProcessor.FrontVowels.Contains(char.ToLower(c, this.CultureInfoForLanguage));
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00018C4D File Offset: 0x00016E4D
		private bool IsClosedVowel(char c)
		{
			return TurkishTextProcessor.ClosedVowels.Contains(char.ToLower(c, this.CultureInfoForLanguage));
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00018C65 File Offset: 0x00016E65
		private bool IsConsonant(char c)
		{
			return TurkishTextProcessor.Consonants.Contains(char.ToLower(c, this.CultureInfoForLanguage));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00018C7D File Offset: 0x00016E7D
		private bool IsUnvoicedConsonant(char c)
		{
			return TurkishTextProcessor.UnvoicedConsonants.Contains(char.ToLower(c, this.CultureInfoForLanguage));
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00018C95 File Offset: 0x00016E95
		private bool IsHardUnvoicedConsonant(char c)
		{
			return TurkishTextProcessor.HardUnvoicedConsonants.Contains(char.ToLower(c, this.CultureInfoForLanguage));
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00018CAD File Offset: 0x00016EAD
		private char FrontVowelToBackVowel(char c)
		{
			c = char.ToLower(c, this.CultureInfoForLanguage);
			if (c == 'e')
			{
				return 'a';
			}
			if (c == 'i')
			{
				return 'ı';
			}
			if (c == 'ö')
			{
				return 'o';
			}
			if (c != 'ü')
			{
				return '*';
			}
			return 'u';
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00018CE8 File Offset: 0x00016EE8
		private char OpenVowelToClosedVowel(char c)
		{
			c = char.ToLower(c, this.CultureInfoForLanguage);
			if (c == 'a')
			{
				return 'ı';
			}
			if (c == 'e')
			{
				return 'i';
			}
			if (c == 'o')
			{
				return 'u';
			}
			if (c != 'ö')
			{
				return '*';
			}
			return 'ü';
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00018D23 File Offset: 0x00016F23
		private char HardConsonantToSoftConsonant(char c)
		{
			c = char.ToLower(c, this.CultureInfoForLanguage);
			if (c == 'p')
			{
				return 'b';
			}
			if (c == 'ç')
			{
				return 'c';
			}
			if (c == 't')
			{
				return 'd';
			}
			if (c != 'k')
			{
				return '*';
			}
			return 'ğ';
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00018D5C File Offset: 0x00016F5C
		private char GetLastVowel(StringBuilder outputText)
		{
			for (int i = outputText.Length - 1; i >= 0; i--)
			{
				if (this.IsVowel(outputText[i]))
				{
					return outputText[i];
				}
			}
			return 'i';
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00018D98 File Offset: 0x00016F98
		public override void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
			bool flag = false;
			if (token == ".link")
			{
				TurkishTextProcessor.LinkList.Add(sourceText.Substring(7));
			}
			else if (sourceText.Contains("<a style=\"Link."))
			{
				if (sourceText[cursorPos - (token.Length + 3)] == '\'')
				{
					flag = this.IsLink(sourceText, token.Length + 2, cursorPos - 1);
				}
				else
				{
					flag = this.IsLink(sourceText, token.Length + 2, cursorPos);
				}
			}
			if (flag)
			{
				if (sourceText[cursorPos - (token.Length + 3)] == '\'')
				{
					cursorPos -= 8;
					outputString.Remove(outputString.Length - 9, 9);
					outputString.Append('\'');
				}
				else
				{
					cursorPos -= 8;
					outputString.Remove(outputString.Length - 8, 8);
				}
			}
			if (token == ".im")
			{
				this.AddSuffix_im(outputString);
			}
			else if (token == ".sin")
			{
				this.AddSuffix_sin(outputString);
			}
			else if (token == ".dir")
			{
				this.AddSuffix_dir(outputString);
			}
			else if (token == ".iz")
			{
				this.AddSuffix_iz(outputString);
			}
			else if (token == ".siniz")
			{
				this.AddSuffix_siniz(outputString);
			}
			else if (token == ".dirler")
			{
				this.AddSuffix_dirler(outputString);
			}
			else if (token == ".i")
			{
				this.AddSuffix_i(outputString);
			}
			else if (token == ".e")
			{
				this.AddSuffix_e(outputString);
			}
			else if (token == ".de")
			{
				this.AddSuffix_de(outputString);
			}
			else if (token == ".den")
			{
				this.AddSuffix_den(outputString);
			}
			else if (token == ".nin")
			{
				this.AddSuffix_nin(outputString);
			}
			else if (token == ".ler")
			{
				this.AddSuffix_ler(outputString);
			}
			else if (token == ".m")
			{
				this.AddSuffix_m(outputString);
			}
			else if (token == ".n")
			{
				this.AddSuffix_n(outputString);
			}
			else if (token == ".in")
			{
				this.AddSuffix_in(outputString);
			}
			else if (token == ".si")
			{
				this.AddSuffix_si(outputString);
			}
			else if (token == ".miz")
			{
				this.AddSuffix_miz(outputString);
			}
			else if (token == ".niz")
			{
				this.AddSuffix_niz(outputString);
			}
			else if (token == ".leri")
			{
				this.AddSuffix_leri(outputString);
			}
			if (flag)
			{
				cursorPos += 8;
				outputString.Append("</b></a>");
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00019058 File Offset: 0x00017258
		private void AddSuffix_im(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			this.SoftenLastCharacter(outputString);
			this.AddYIfNeeded(outputString);
			outputString.Append(c);
			outputString.Append('m');
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000190A0 File Offset: 0x000172A0
		private void AddSuffix_sin(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			outputString.Append('s');
			outputString.Append(c);
			outputString.Append('n');
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000190E4 File Offset: 0x000172E4
		private void AddSuffix_dir(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char harmonizedD = this.GetHarmonizedD(outputString);
			outputString.Append(harmonizedD);
			outputString.Append(c);
			outputString.Append('r');
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00019130 File Offset: 0x00017330
		private void AddSuffix_iz(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			this.SoftenLastCharacter(outputString);
			this.AddYIfNeeded(outputString);
			outputString.Append(c);
			outputString.Append('z');
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00019178 File Offset: 0x00017378
		private void AddSuffix_siniz(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			outputString.Append('s');
			outputString.Append(c);
			outputString.Append('n');
			outputString.Append(c);
			outputString.Append('z');
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000191CC File Offset: 0x000173CC
		private void AddSuffix_dirler(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char nextVowel = this.GetNextVowel(outputString);
			char harmonizedD = this.GetHarmonizedD(outputString);
			outputString.Append(harmonizedD);
			outputString.Append(c);
			outputString.Append('r');
			outputString.Append('l');
			outputString.Append(nextVowel);
			outputString.Append('r');
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00019238 File Offset: 0x00017438
		private void AddSuffix_i(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			this.SoftenLastCharacter(outputString);
			if (this.GetLastCharacter(outputString) == '\'' && outputString.Length > 6 && outputString.ToString().EndsWith("Kalesi'", true, TurkishTextProcessor._cultureInfo))
			{
				outputString.Append('n');
			}
			else
			{
				this.AddYIfNeeded(outputString);
			}
			outputString.Append(c);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000192B0 File Offset: 0x000174B0
		private void AddSuffix_e(StringBuilder outputString)
		{
			char nextVowel = this.GetNextVowel(outputString);
			this.SoftenLastCharacter(outputString);
			this.AddYIfNeeded(outputString);
			outputString.Append(nextVowel);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000192DC File Offset: 0x000174DC
		private void AddSuffix_de(StringBuilder outputString)
		{
			char nextVowel = this.GetNextVowel(outputString);
			char harmonizedD = this.GetHarmonizedD(outputString);
			outputString.Append(harmonizedD);
			outputString.Append(nextVowel);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0001930C File Offset: 0x0001750C
		private void AddSuffix_den(StringBuilder outputString)
		{
			char nextVowel = this.GetNextVowel(outputString);
			char harmonizedD = this.GetHarmonizedD(outputString);
			outputString.Append(harmonizedD);
			outputString.Append(nextVowel);
			outputString.Append('n');
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00019344 File Offset: 0x00017544
		private void AddSuffix_nin(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char c2 = this.GetLastCharacter(outputString);
			if (c2 == '\'')
			{
				c2 = this.GetSecondLastCharacter(outputString);
			}
			else
			{
				this.SoftenLastCharacter(outputString);
			}
			if (this.IsVowel(c2))
			{
				outputString.Append('n');
			}
			outputString.Append(c);
			outputString.Append('n');
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000193B0 File Offset: 0x000175B0
		private void AddSuffix_ler(StringBuilder outputString)
		{
			char nextVowel = this.GetNextVowel(outputString);
			outputString.Append('l');
			outputString.Append(nextVowel);
			outputString.Append('r');
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000193E0 File Offset: 0x000175E0
		private void AddSuffix_m(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char lastCharacter = this.GetLastCharacter(outputString);
			this.SoftenLastCharacter(outputString);
			if (this.IsConsonant(lastCharacter))
			{
				outputString.Append(c);
			}
			outputString.Append('m');
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00019434 File Offset: 0x00017634
		private void AddSuffix_n(StringBuilder outputString)
		{
			char lastLetter = this.GetLastLetter(outputString);
			char secondLastLetter = this.GetSecondLastLetter(outputString);
			if (this.IsVowel(lastLetter) && !this.IsVowel(secondLastLetter))
			{
				outputString.Append('n');
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0001946C File Offset: 0x0001766C
		private void AddSuffix_in(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char lastLetter = this.GetLastLetter(outputString);
			this.SoftenLastCharacter(outputString);
			if (this.IsConsonant(lastLetter))
			{
				outputString.Append(c);
			}
			outputString.Append('n');
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000194C0 File Offset: 0x000176C0
		private void AddSuffix_si(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char lastCharacter = this.GetLastCharacter(outputString);
			this.SoftenLastCharacter(outputString);
			if (this.IsVowel(lastCharacter))
			{
				outputString.Append('s');
			}
			outputString.Append(c);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00019514 File Offset: 0x00017714
		private void AddSuffix_miz(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char lastCharacter = this.GetLastCharacter(outputString);
			this.SoftenLastCharacter(outputString);
			if (this.IsConsonant(lastCharacter))
			{
				outputString.Append(c);
			}
			outputString.Append('m');
			outputString.Append(c);
			outputString.Append('z');
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00019578 File Offset: 0x00017778
		private void AddSuffix_niz(StringBuilder outputString)
		{
			char lastVowel = this.GetLastVowel(outputString);
			char c = (this.IsClosedVowel(lastVowel) ? lastVowel : this.OpenVowelToClosedVowel(lastVowel));
			char lastCharacter = this.GetLastCharacter(outputString);
			this.SoftenLastCharacter(outputString);
			if (this.IsConsonant(lastCharacter))
			{
				outputString.Append(c);
			}
			outputString.Append('n');
			outputString.Append(c);
			outputString.Append('z');
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000195DC File Offset: 0x000177DC
		private void AddSuffix_leri(StringBuilder outputString)
		{
			this.GetLastVowel(outputString);
			char nextVowel = this.GetNextVowel(outputString);
			char c = ((nextVowel == 'a') ? 'ı' : 'i');
			outputString.Append('l');
			outputString.Append(nextVowel);
			outputString.Append('r');
			outputString.Append(c);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001962C File Offset: 0x0001782C
		private char GetHarmonizedD(StringBuilder outputString)
		{
			char c = this.GetLastCharacter(outputString);
			if (c == '\'')
			{
				c = this.GetSecondLastCharacter(outputString);
			}
			if (!this.IsUnvoicedConsonant(c))
			{
				return 'd';
			}
			return 't';
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0001965C File Offset: 0x0001785C
		private void AddYIfNeeded(StringBuilder outputString)
		{
			char lastCharacter = this.GetLastCharacter(outputString);
			if (this.IsVowel(lastCharacter) || (lastCharacter == '\'' && this.IsVowel(this.GetSecondLastCharacter(outputString))))
			{
				outputString.Append('y');
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00019698 File Offset: 0x00017898
		private void SoftenLastCharacter(StringBuilder outputString)
		{
			char lastCharacter = this.GetLastCharacter(outputString);
			if (this.IsHardUnvoicedConsonant(lastCharacter) && !this.LastWordNonMutating(outputString))
			{
				outputString[outputString.Length - 1] = this.HardConsonantToSoftConsonant(lastCharacter);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000196D4 File Offset: 0x000178D4
		private string GetLastWord(StringBuilder outputString)
		{
			int num = -1;
			int num2 = outputString.Length - 1;
			while (num2 >= 0 && num < 0)
			{
				if (outputString[num2] == ' ')
				{
					num = num2;
				}
				num2--;
			}
			if (num < outputString.Length - 1)
			{
				return outputString.ToString(num + 1, outputString.Length - num - 1).Trim(new char[] { '\n', '\'' });
			}
			return null;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0001973C File Offset: 0x0001793C
		private bool LastWordNonMutating(StringBuilder outputString)
		{
			string lastWord = this.GetLastWord(outputString);
			return lastWord != null && TurkishTextProcessor.NonMutatingWord.Contains(lastWord.ToLower(this.CultureInfoForLanguage));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0001976C File Offset: 0x0001796C
		private char GetLastCharacter(StringBuilder outputString)
		{
			if (outputString.Length <= 0)
			{
				return '*';
			}
			return outputString[outputString.Length - 1];
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00019788 File Offset: 0x00017988
		private char GetLastLetter(StringBuilder outputString)
		{
			for (int i = outputString.Length - 1; i >= 0; i--)
			{
				if (char.IsLetter(outputString[i]))
				{
					return outputString[i];
				}
			}
			return 'x';
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000197C0 File Offset: 0x000179C0
		private char GetSecondLastLetter(StringBuilder outputString)
		{
			bool flag = false;
			for (int i = outputString.Length - 1; i >= 0; i--)
			{
				if (char.IsLetter(outputString[i]))
				{
					if (flag)
					{
						return outputString[i];
					}
					flag = true;
				}
			}
			return 'x';
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000197FF File Offset: 0x000179FF
		private char GetSecondLastCharacter(StringBuilder outputString)
		{
			if (outputString.Length <= 1)
			{
				return '*';
			}
			return outputString[outputString.Length - 2];
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001981C File Offset: 0x00017A1C
		private bool IsLink(string sourceText, int tokenLength, int cursorPos)
		{
			string text = sourceText.Remove(cursorPos - tokenLength);
			for (int i = 0; i < TurkishTextProcessor.LinkList.Count; i++)
			{
				if (sourceText.Length >= TurkishTextProcessor.LinkList[i].Length && text.EndsWith(TurkishTextProcessor.LinkList[i]))
				{
					TurkishTextProcessor.LinkList.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00019881 File Offset: 0x00017A81
		public override CultureInfo CultureInfoForLanguage
		{
			get
			{
				return TurkishTextProcessor._cultureInfo;
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00019888 File Offset: 0x00017A88
		public override void ClearTemporaryData()
		{
			TurkishTextProcessor.LinkList.Clear();
		}

		// Token: 0x040000FB RID: 251
		private static CultureInfo _curCultureInfo = CultureInfo.InvariantCulture;

		// Token: 0x040000FC RID: 252
		private static char[] Vowels = new char[] { 'a', 'ı', 'o', 'u', 'e', 'i', 'ö', 'ü' };

		// Token: 0x040000FD RID: 253
		private static char[] BackVowels = new char[] { 'a', 'ı', 'o', 'u' };

		// Token: 0x040000FE RID: 254
		private static int[] BackNumbers = new int[] { 6, 9, 10, 30, 40, 60, 90 };

		// Token: 0x040000FF RID: 255
		private static char[] FrontVowels = new char[] { 'e', 'i', 'ö', 'ü' };

		// Token: 0x04000100 RID: 256
		private static char[] OpenVowels = new char[] { 'a', 'e', 'o', 'ö' };

		// Token: 0x04000101 RID: 257
		private static char[] ClosedVowels = new char[] { 'ı', 'i', 'u', 'ü' };

		// Token: 0x04000102 RID: 258
		private static char[] Consonants = new char[]
		{
			'b', 'c', 'ç', 'd', 'f', 'g', 'ğ', 'h', 'j', 'k',
			'l', 'm', 'n', 'p', 'r', 's', 'ş', 't', 'v', 'y',
			'z'
		};

		// Token: 0x04000103 RID: 259
		private static char[] UnvoicedConsonants = new char[] { 'ç', 'f', 'h', 'k', 'p', 's', 'ş', 't' };

		// Token: 0x04000104 RID: 260
		private static char[] HardUnvoicedConsonants = new char[] { 'p', 'ç', 't', 'k' };

		// Token: 0x04000105 RID: 261
		private static string[] NonMutatingWord = new string[]
		{
			"ak", "at", "ek", "et", "göç", "ip", "çöp", "ok", "ot", "saç",
			"sap", "süt", "üç", "suç", "top", "ticaret", "kürk", "dük", "kont", "hizmet"
		};

		// Token: 0x04000106 RID: 262
		private static Dictionary<string, char> _exceptions = new Dictionary<string, char> { { "kontrol", 'e' } };

		// Token: 0x04000107 RID: 263
		[ThreadStatic]
		private static List<string> _linkList = new List<string>();

		// Token: 0x04000108 RID: 264
		private static CultureInfo _cultureInfo = new CultureInfo("tr-TR");
	}
}
