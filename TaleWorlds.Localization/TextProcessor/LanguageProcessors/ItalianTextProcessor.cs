using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TaleWorlds.Localization.TextProcessor.LanguageProcessors
{
	// Token: 0x02000033 RID: 51
	public class ItalianTextProcessor : LanguageSpecificTextProcessor
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C1FD File Offset: 0x0000A3FD
		private string LinkTag
		{
			get
			{
				return ".link";
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000C204 File Offset: 0x0000A404
		private int LinkTagLength
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000C207 File Offset: 0x0000A407
		private string LinkStarter
		{
			get
			{
				return "<a style=\"Link.";
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000C20E File Offset: 0x0000A40E
		private string LinkEnding
		{
			get
			{
				return "</b></a>";
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000C215 File Offset: 0x0000A415
		public static Dictionary<string, ValueTuple<string, int>> WordGroups
		{
			get
			{
				if (ItalianTextProcessor._wordGroups == null)
				{
					ItalianTextProcessor._wordGroups = new Dictionary<string, ValueTuple<string, int>>();
				}
				return ItalianTextProcessor._wordGroups;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000C230 File Offset: 0x0000A430
		public override void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
			if (sourceText.Length > this.LinkStarter.Length + cursorPos)
			{
				string text = sourceText.Substring(cursorPos, this.LinkStarter.Length);
				if (token == this.LinkTag || text.Equals(this.LinkStarter))
				{
					cursorPos = this.ProcessLink(sourceText, cursorPos, token, outputString);
				}
			}
			string text2 = token.ToLower();
			if (ItalianTextProcessor.GenderTokens.TokenList.IndexOf(token) >= 0)
			{
				this.SetGenderInfo(token);
				this.ProcessWordGroup(sourceText, token, cursorPos);
				return;
			}
			if (ItalianTextProcessor.FunctionTokens.TokenList.IndexOf(text2) >= 0 && this.CheckWhiteSpaceAndTextEnd(sourceText, cursorPos))
			{
				string text3;
				if (this.IsWordGroup(sourceText, token, cursorPos, out text3))
				{
					this.SetGenderInfo(text3);
				}
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
				if (num <= 1326194706U)
				{
					if (num != 1205941254U)
					{
						if (num != 1209192658U)
						{
							if (num == 1326194706U)
							{
								if (text2 == ".di")
								{
									this.HandleOfPrepositions(sourceText, token, cursorPos, outputString);
								}
							}
						}
						else if (text2 == ".in")
						{
							this.HandleInPrepositions(sourceText, token, cursorPos, outputString);
						}
					}
					else if (text2 == ".un")
					{
						this.HandleIndefiniteArticles(sourceText, token, cursorPos, outputString);
					}
				}
				else if (num <= 1423190704U)
				{
					if (num != 1390200873U)
					{
						if (num == 1423190704U)
						{
							if (text2 == ".a")
							{
								this.HandleToPrepositions(sourceText, token, cursorPos, outputString);
							}
						}
					}
					else if (text2 == ".su")
					{
						this.HandleOnPrepositions(sourceText, token, cursorPos, outputString);
					}
				}
				else if (num != 1460415658U)
				{
					if (num == 1641299751U)
					{
						if (text2 == ".l")
						{
							this.HandleDefiniteArticles(sourceText, token, cursorPos, outputString);
						}
					}
				}
				else if (text2 == ".da")
				{
					this.HandleFromPrepositions(sourceText, token, cursorPos, outputString);
				}
				ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.NoDeclination;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C428 File Offset: 0x0000A628
		private bool IsWordGroup(string sourceText, string token, int cursorPos, out string tag)
		{
			int num = 0;
			string text = string.Empty;
			tag = string.Empty;
			foreach (KeyValuePair<string, ValueTuple<string, int>> keyValuePair in ItalianTextProcessor.WordGroups)
			{
				if (keyValuePair.Key.Length > 0 && sourceText.Length >= cursorPos + keyValuePair.Key.Length && keyValuePair.Key.Length > num && keyValuePair.Key.Equals(sourceText.Substring(cursorPos, keyValuePair.Key.Length)))
				{
					text = keyValuePair.Key;
					num = keyValuePair.Key.Length;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				tag = ItalianTextProcessor.WordGroups[text].Item1;
				return true;
			}
			return false;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000C50C File Offset: 0x0000A70C
		private ItalianTextProcessor.WordType GetNextWordType(string sourceText, int cursorPos)
		{
			if (cursorPos >= sourceText.Length - 1)
			{
				return ItalianTextProcessor.WordType.Other;
			}
			char c = char.ToLower(sourceText[cursorPos]);
			char c2 = char.ToLower(sourceText[cursorPos + 1]);
			string text = sourceText.Substring(cursorPos, 2).ToLowerInvariant();
			if (ItalianTextProcessor.Vowels.Contains(c))
			{
				return ItalianTextProcessor.WordType.Vowel;
			}
			foreach (string text2 in ItalianTextProcessor.SpecialConsonants)
			{
				if (text.StartsWith(text2))
				{
					return ItalianTextProcessor.WordType.SpecialConsonant;
				}
			}
			if (ItalianTextProcessor.SpecialConsonantBeginnings.Contains(c) && ItalianTextProcessor.Consonants.Contains(c2))
			{
				return ItalianTextProcessor.WordType.SpecialConsonant;
			}
			if (char.IsLetter(c))
			{
				return ItalianTextProcessor.WordType.Consonant;
			}
			return ItalianTextProcessor.WordType.Other;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
		private bool CheckWhiteSpaceAndTextEnd(string sourceText, int cursorPos)
		{
			return cursorPos < sourceText.Length && !char.IsWhiteSpace(sourceText[cursorPos]);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		private void HandleOnPrepositions(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.Prepositions prepositions = ItalianTextProcessor.Prepositions.On;
			Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> dictionary = ItalianTextProcessor._prepositionDictionary[prepositions];
			this.HandlePrepositionsInternal(text, token, cursorPos, dictionary, stringBuilder);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		private void HandleInPrepositions(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.Prepositions prepositions = ItalianTextProcessor.Prepositions.In;
			Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> dictionary = ItalianTextProcessor._prepositionDictionary[prepositions];
			this.HandlePrepositionsInternal(text, token, cursorPos, dictionary, stringBuilder);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000C61C File Offset: 0x0000A81C
		private void HandleOfPrepositions(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.Prepositions prepositions = ItalianTextProcessor.Prepositions.Of;
			Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> dictionary = ItalianTextProcessor._prepositionDictionary[prepositions];
			this.HandlePrepositionsInternal(text, token, cursorPos, dictionary, stringBuilder);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000C644 File Offset: 0x0000A844
		private void HandleToPrepositions(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.Prepositions prepositions = ItalianTextProcessor.Prepositions.To;
			Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> dictionary = ItalianTextProcessor._prepositionDictionary[prepositions];
			this.HandlePrepositionsInternal(text, token, cursorPos, dictionary, stringBuilder);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000C66C File Offset: 0x0000A86C
		private void HandleFromPrepositions(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.Prepositions prepositions = ItalianTextProcessor.Prepositions.From;
			Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> dictionary = ItalianTextProcessor._prepositionDictionary[prepositions];
			this.HandlePrepositionsInternal(text, token, cursorPos, dictionary, stringBuilder);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000C694 File Offset: 0x0000A894
		private void HandlePrepositionsInternal(string text, string token, int cursorPos, Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> dictionary, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.WordType nextWordType = this.GetNextWordType(text, cursorPos);
			if (nextWordType != ItalianTextProcessor.WordType.Other && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.NoDeclination)
			{
				string text2 = dictionary[ItalianTextProcessor._curGender][nextWordType];
				if (char.IsUpper(token[1]))
				{
					text2 = char.ToUpper(text2[0]).ToString() + text2.Substring(1);
				}
				stringBuilder.Append(text2);
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C704 File Offset: 0x0000A904
		private void HandleDefiniteArticles(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.WordType nextWordType = this.GetNextWordType(text, cursorPos);
			if (nextWordType != ItalianTextProcessor.WordType.Other && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.MaleNoun && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.FemaleNoun && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.NoDeclination)
			{
				string text2 = ItalianTextProcessor._genderWordTypeDefiniteArticleDictionary[ItalianTextProcessor._curGender][nextWordType];
				if (char.IsUpper(token[1]))
				{
					text2 = char.ToUpper(text2[0]).ToString() + text2.Substring(1);
				}
				stringBuilder.Append(text2);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000C784 File Offset: 0x0000A984
		private void HandleIndefiniteArticles(string text, string token, int cursorPos, StringBuilder stringBuilder)
		{
			ItalianTextProcessor.WordType nextWordType = this.GetNextWordType(text, cursorPos);
			if (nextWordType != ItalianTextProcessor.WordType.Other && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.MaleNoun && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.FemaleNoun && ItalianTextProcessor._curGender != ItalianTextProcessor.WordGenderEnum.NoDeclination)
			{
				string text2 = ItalianTextProcessor._genderWordTypeIndefiniteArticleDictionary[ItalianTextProcessor._curGender][nextWordType];
				if (char.IsUpper(token[1]))
				{
					text2 = char.ToUpper(text2[0]).ToString() + text2.Substring(1);
				}
				stringBuilder.Append(text2);
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000C804 File Offset: 0x0000AA04
		private void SetGenderInfo(string token)
		{
			if (token == ".MS")
			{
				ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.MasculineSingular;
				return;
			}
			if (token == ".MP")
			{
				ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.MasculinePlural;
				return;
			}
			if (token == ".FS")
			{
				ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.FeminineSingular;
				return;
			}
			if (token == ".FP")
			{
				ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.FemininePlural;
				return;
			}
			if (token == ".MN")
			{
				ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.MaleNoun;
				return;
			}
			if (!(token == ".FN"))
			{
				return;
			}
			ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.FemaleNoun;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000C88C File Offset: 0x0000AA8C
		private void ProcessWordGroup(string text, string token, int cursorPos)
		{
			string text2 = text.Substring(text.LastIndexOf('}') + 1);
			ValueTuple<string, int> valueTuple;
			if (ItalianTextProcessor.WordGroups.TryGetValue(text2, out valueTuple))
			{
				ItalianTextProcessor.WordGroups[text2] = new ValueTuple<string, int>(valueTuple.Item1, valueTuple.Item2);
				return;
			}
			ItalianTextProcessor.WordGroups.Add(text2, new ValueTuple<string, int>(token, cursorPos));
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
		private int ProcessLink(string text, int cursorPos, string token, StringBuilder outputString)
		{
			int num = text.IndexOf(this.LinkEnding, cursorPos);
			if (num > cursorPos)
			{
				string text2 = text.Substring(cursorPos, num - cursorPos);
				string text3 = text2.Substring(0, text2.LastIndexOf('>') + 1);
				string text4 = text2.Substring(text3.Length);
				ValueTuple<string, int> valueTuple;
				if (token != this.LinkTag && ItalianTextProcessor.WordGroups.TryGetValue(text4, out valueTuple))
				{
					this.SetGenderInfo(valueTuple.Item1);
				}
				outputString.Append(text3);
				return cursorPos + text3.Length;
			}
			return cursorPos;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000C96E File Offset: 0x0000AB6E
		public override CultureInfo CultureInfoForLanguage
		{
			get
			{
				return ItalianTextProcessor.CultureInfo;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000C975 File Offset: 0x0000AB75
		public override void ClearTemporaryData()
		{
			ItalianTextProcessor.WordGroups.Clear();
			ItalianTextProcessor._curGender = ItalianTextProcessor.WordGenderEnum.NoDeclination;
		}

		// Token: 0x040000CB RID: 203
		private static char[] Vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

		// Token: 0x040000CC RID: 204
		private static char[] SpecialConsonantBeginnings = new char[] { 's' };

		// Token: 0x040000CD RID: 205
		private static string[] SpecialConsonants = new string[] { "x", "y", "gn", "z", "ps", "pn" };

		// Token: 0x040000CE RID: 206
		private static char[] Consonants = new char[]
		{
			'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm',
			'n', 'p', 'q', 'r', 's', 't', 'v', 'z'
		};

		// Token: 0x040000CF RID: 207
		[ThreadStatic]
		private static ItalianTextProcessor.WordGenderEnum _curGender;

		// Token: 0x040000D0 RID: 208
		[ThreadStatic]
		private static Dictionary<string, ValueTuple<string, int>> _wordGroups = new Dictionary<string, ValueTuple<string, int>>();

		// Token: 0x040000D1 RID: 209
		private static Dictionary<ItalianTextProcessor.Prepositions, Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>> _prepositionDictionary = new Dictionary<ItalianTextProcessor.Prepositions, Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>>
		{
			{
				ItalianTextProcessor.Prepositions.Of,
				new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
				{
					{
						ItalianTextProcessor.WordGenderEnum.MasculineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"del "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"dello "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"dell'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MasculinePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"dei "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"degli "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"degli "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FeminineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"della "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"della "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"dell'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemininePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"delle "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"delle "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"delle "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"di "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"di "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"di "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"di "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"di "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"di "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					}
				}
			},
			{
				ItalianTextProcessor.Prepositions.To,
				new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
				{
					{
						ItalianTextProcessor.WordGenderEnum.MasculineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"al "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"allo "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"all'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MasculinePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"ai "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"agli "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"agli "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FeminineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"alla "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"alla "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"all'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemininePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"alle "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"alle "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"alle "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"a "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"a "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"a "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"a "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"a "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"a "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					}
				}
			},
			{
				ItalianTextProcessor.Prepositions.From,
				new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
				{
					{
						ItalianTextProcessor.WordGenderEnum.MasculineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"dal "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"dallo "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"dall'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MasculinePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"dai "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"dagli "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"dagli "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FeminineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"dalla "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"dalla "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"dall'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemininePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"dalle "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"dalle "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"dalle "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"da "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"da "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"da "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"da "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"da "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"da "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					}
				}
			},
			{
				ItalianTextProcessor.Prepositions.On,
				new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
				{
					{
						ItalianTextProcessor.WordGenderEnum.MasculineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"sul "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"sullo "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"sull'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MasculinePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"sui "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"sugli "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"sugli "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FeminineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"sulla "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"sulla "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"sull'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemininePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"sulle "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"sulle "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"sulle "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"su "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"su "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"su "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"su "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"su "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"su "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					}
				}
			},
			{
				ItalianTextProcessor.Prepositions.In,
				new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
				{
					{
						ItalianTextProcessor.WordGenderEnum.MasculineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"nel "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"nello "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"nell'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MasculinePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"nei "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"negli "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"negli "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FeminineSingular,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"nella "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"nella "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"nell'"
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemininePlural,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"nelle "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"nelle "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"nelle "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.MaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"in "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"in "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"in "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					},
					{
						ItalianTextProcessor.WordGenderEnum.FemaleNoun,
						new Dictionary<ItalianTextProcessor.WordType, string>
						{
							{
								ItalianTextProcessor.WordType.Consonant,
								"in "
							},
							{
								ItalianTextProcessor.WordType.SpecialConsonant,
								"in "
							},
							{
								ItalianTextProcessor.WordType.Vowel,
								"in "
							},
							{
								ItalianTextProcessor.WordType.Other,
								""
							}
						}
					}
				}
			}
		};

		// Token: 0x040000D2 RID: 210
		private static Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> _genderWordTypeDefiniteArticleDictionary = new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
		{
			{
				ItalianTextProcessor.WordGenderEnum.MasculineSingular,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"il "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"lo "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"l'"
					}
				}
			},
			{
				ItalianTextProcessor.WordGenderEnum.MasculinePlural,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"i "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"gli "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"gli "
					}
				}
			},
			{
				ItalianTextProcessor.WordGenderEnum.FeminineSingular,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"la "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"la "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"l'"
					}
				}
			},
			{
				ItalianTextProcessor.WordGenderEnum.FemininePlural,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"le "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"le "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"le "
					}
				}
			}
		};

		// Token: 0x040000D3 RID: 211
		private static Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>> _genderWordTypeIndefiniteArticleDictionary = new Dictionary<ItalianTextProcessor.WordGenderEnum, Dictionary<ItalianTextProcessor.WordType, string>>
		{
			{
				ItalianTextProcessor.WordGenderEnum.MasculineSingular,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"un "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"uno "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"un "
					}
				}
			},
			{
				ItalianTextProcessor.WordGenderEnum.MasculinePlural,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"dei "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"degli "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"degli "
					}
				}
			},
			{
				ItalianTextProcessor.WordGenderEnum.FeminineSingular,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"una "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"una "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"un'"
					}
				}
			},
			{
				ItalianTextProcessor.WordGenderEnum.FemininePlural,
				new Dictionary<ItalianTextProcessor.WordType, string>
				{
					{
						ItalianTextProcessor.WordType.Consonant,
						"delle "
					},
					{
						ItalianTextProcessor.WordType.SpecialConsonant,
						"delle "
					},
					{
						ItalianTextProcessor.WordType.Vowel,
						"delle "
					}
				}
			}
		};

		// Token: 0x040000D4 RID: 212
		private static readonly CultureInfo CultureInfo = new CultureInfo("it-IT");

		// Token: 0x0200004B RID: 75
		private enum WordType
		{
			// Token: 0x04000194 RID: 404
			Vowel,
			// Token: 0x04000195 RID: 405
			SpecialConsonant,
			// Token: 0x04000196 RID: 406
			Consonant,
			// Token: 0x04000197 RID: 407
			Other
		}

		// Token: 0x0200004C RID: 76
		private enum WordGenderEnum
		{
			// Token: 0x04000199 RID: 409
			MasculineSingular,
			// Token: 0x0400019A RID: 410
			MasculinePlural,
			// Token: 0x0400019B RID: 411
			FeminineSingular,
			// Token: 0x0400019C RID: 412
			FemininePlural,
			// Token: 0x0400019D RID: 413
			MaleNoun,
			// Token: 0x0400019E RID: 414
			FemaleNoun,
			// Token: 0x0400019F RID: 415
			NoDeclination
		}

		// Token: 0x0200004D RID: 77
		private enum Prepositions
		{
			// Token: 0x040001A1 RID: 417
			To,
			// Token: 0x040001A2 RID: 418
			Of,
			// Token: 0x040001A3 RID: 419
			From,
			// Token: 0x040001A4 RID: 420
			In,
			// Token: 0x040001A5 RID: 421
			On
		}

		// Token: 0x0200004E RID: 78
		private static class GenderTokens
		{
			// Token: 0x040001A6 RID: 422
			public const string MasculineSingular = ".MS";

			// Token: 0x040001A7 RID: 423
			public const string MasculinePlural = ".MP";

			// Token: 0x040001A8 RID: 424
			public const string FeminineSingular = ".FS";

			// Token: 0x040001A9 RID: 425
			public const string FemininePlural = ".FP";

			// Token: 0x040001AA RID: 426
			public const string MaleNoun = ".MN";

			// Token: 0x040001AB RID: 427
			public const string FemaleNoun = ".FN";

			// Token: 0x040001AC RID: 428
			public static readonly List<string> TokenList = new List<string> { ".MS", ".MP", ".FS", ".FP", ".MN", ".FN" };
		}

		// Token: 0x0200004F RID: 79
		private static class FunctionTokens
		{
			// Token: 0x040001AD RID: 429
			public const string DefiniteArticle = ".l";

			// Token: 0x040001AE RID: 430
			public const string IndefiniteArticle = ".un";

			// Token: 0x040001AF RID: 431
			public const string OfPreposition = ".di";

			// Token: 0x040001B0 RID: 432
			public const string ToPreposition = ".a";

			// Token: 0x040001B1 RID: 433
			public const string FromPreposition = ".da";

			// Token: 0x040001B2 RID: 434
			public const string OnPreposition = ".su";

			// Token: 0x040001B3 RID: 435
			public const string InPreposition = ".in";

			// Token: 0x040001B4 RID: 436
			public static readonly List<string> TokenList = new List<string> { ".l", ".un", ".di", ".a", ".da", ".su", ".in" };
		}
	}
}
