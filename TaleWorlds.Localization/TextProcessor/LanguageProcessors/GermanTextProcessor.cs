using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TaleWorlds.Library;

namespace TaleWorlds.Localization.TextProcessor.LanguageProcessors
{
	// Token: 0x02000032 RID: 50
	public class GermanTextProcessor : LanguageSpecificTextProcessor
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00008D03 File Offset: 0x00006F03
		public override CultureInfo CultureInfoForLanguage
		{
			get
			{
				return GermanTextProcessor.CultureInfo;
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008D0A File Offset: 0x00006F0A
		public override void ClearTemporaryData()
		{
			GermanTextProcessor.LinkList.Clear();
			GermanTextProcessor.WordGroups.Clear();
			GermanTextProcessor.WordGroupsNoTags.Clear();
			GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.NoDeclination;
			GermanTextProcessor._doesComeFromWordGroup = false;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00008D36 File Offset: 0x00006F36
		private bool Masculine
		{
			get
			{
				return GermanTextProcessor._curGender == GermanTextProcessor.WordGenderEnum.Masculine;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00008D40 File Offset: 0x00006F40
		private bool Feminine
		{
			get
			{
				return GermanTextProcessor._curGender == GermanTextProcessor.WordGenderEnum.Feminine;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00008D4A File Offset: 0x00006F4A
		private bool Neuter
		{
			get
			{
				return GermanTextProcessor._curGender == GermanTextProcessor.WordGenderEnum.Neuter;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00008D54 File Offset: 0x00006F54
		private bool Plural
		{
			get
			{
				return GermanTextProcessor._curGender == GermanTextProcessor.WordGenderEnum.Plural;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00008D5E File Offset: 0x00006F5E
		[TupleElementNames(new string[] { "wordGroup", "firstMarkerPost" })]
		private static List<ValueTuple<string, int>> WordGroups
		{
			[return: TupleElementNames(new string[] { "wordGroup", "firstMarkerPost" })]
			get
			{
				if (GermanTextProcessor._wordGroups == null)
				{
					GermanTextProcessor._wordGroups = new List<ValueTuple<string, int>>();
				}
				return GermanTextProcessor._wordGroups;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00008D76 File Offset: 0x00006F76
		private static List<string> LinkList
		{
			get
			{
				if (GermanTextProcessor._linkList == null)
				{
					GermanTextProcessor._linkList = new List<string>();
				}
				return GermanTextProcessor._linkList;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008D8E File Offset: 0x00006F8E
		private static List<string> WordGroupsNoTags
		{
			get
			{
				if (GermanTextProcessor._wordGroupsNoTags == null)
				{
					GermanTextProcessor._wordGroupsNoTags = new List<string>();
				}
				return GermanTextProcessor._wordGroupsNoTags;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00008DA6 File Offset: 0x00006FA6
		private string LinkTag
		{
			get
			{
				return ".link";
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00008DAD File Offset: 0x00006FAD
		private int LinkTagLength
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00008DB0 File Offset: 0x00006FB0
		private string LinkStarter
		{
			get
			{
				return "<a style=\"Link.";
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00008DB7 File Offset: 0x00006FB7
		private string LinkEnding
		{
			get
			{
				return "</b></a>";
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00008DBE File Offset: 0x00006FBE
		private int LinkEndingLength
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public override void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
			bool flag = false;
			if (token == this.LinkTag)
			{
				GermanTextProcessor.LinkList.Add(sourceText.Substring(this.LinkTagLength));
			}
			else if (sourceText.Contains(this.LinkStarter))
			{
				flag = this.IsLink(sourceText, token.Length + 2, cursorPos);
			}
			if (flag)
			{
				cursorPos -= this.LinkEndingLength;
				outputString.Remove(outputString.Length - this.LinkEndingLength, this.LinkEndingLength);
			}
			int num2;
			if (token.EndsWith("Creator"))
			{
				outputString.Append("{" + token.Replace("Creator", "") + "}");
			}
			else if (Array.IndexOf<string>(GermanTextProcessor.GenderTokens.TokenList, token) >= 0)
			{
				if (token == ".M")
				{
					this.SetMasculine();
				}
				else if (token == ".F")
				{
					this.SetFeminine();
				}
				else if (token == ".N")
				{
					this.SetNeuter();
				}
				else if (token == ".P")
				{
					this.SetPlural();
				}
				if (cursorPos == token.Length + 2 && sourceText.IndexOf("{.", cursorPos, StringComparison.InvariantCulture) == -1 && sourceText.IndexOf(' ', cursorPos) == -1)
				{
					GermanTextProcessor.WordGroups.Add(new ValueTuple<string, int>(sourceText + "{.nn}", cursorPos));
					GermanTextProcessor.WordGroupsNoTags.Add(sourceText.Substring(cursorPos));
				}
			}
			else if (Array.IndexOf<string>(GermanTextProcessor.WordGroupTokens.TokenList, token) >= 0)
			{
				if (token == ".nnp" || token == ".ajmp" || token == ".ajwp" || token == ".ajsp" || token == ".ajm" || token == ".ajw" || token == ".ajs")
				{
					string text;
					int num;
					if (this.IsIrregularWord(sourceText, cursorPos, token, out text, out num))
					{
						outputString.Remove(outputString.Length - num, num);
						outputString.Append(text);
					}
					else if (token == ".nnp")
					{
						this.AddSuffixNounNominativePlural(outputString);
					}
					else if (token == ".ajmp")
					{
						this.AddSuffixMixedNominativePlural(outputString);
					}
					else if (token == ".ajwp")
					{
						this.AddSuffixWeakNominativePlural(outputString);
					}
					else if (token == ".ajsp")
					{
						this.AddSuffixStrongNominativePlural(outputString);
					}
					else if (token == ".ajm")
					{
						this.AddSuffixMixedNominative(outputString);
					}
					else if (token == ".ajw")
					{
						this.AddSuffixWeakNominative(outputString);
					}
					else if (token == ".ajs")
					{
						this.AddSuffixStrongNominative(outputString);
					}
				}
				else if (token == ".pnpgroup" || token == ".pngroup")
				{
					this.AddPronounArticle(sourceText, cursorPos, token, ref outputString);
				}
				GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.NoDeclination;
				this.WordGroupProcessor(sourceText, cursorPos);
			}
			else if (Array.IndexOf<string>(GermanTextProcessor.NounTokens.TokenList, token) >= 0 && (!GermanTextProcessor._doesComeFromWordGroup || (GermanTextProcessor._doesComeFromWordGroup && GermanTextProcessor._curGender == GermanTextProcessor.WordGenderEnum.NoDeclination)) && this.IsWordGroup(token.Length, sourceText, cursorPos, out num2))
			{
				if (num2 >= 0)
				{
					token = "{" + token + "}";
					GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.NoDeclination;
					this.AddSuffixWordGroup(token, num2, outputString);
				}
			}
			else if (token == ".o")
			{
				this.HandlePossession(outputString, cursorPos);
			}
			else if (GermanTextProcessor._curGender != GermanTextProcessor.WordGenderEnum.NoDeclination)
			{
				if (this.ShouldDeclineWord(sourceText, cursorPos, token))
				{
					string text2;
					int num3;
					if (this.IsIrregularWord(sourceText, cursorPos, token, out text2, out num3))
					{
						outputString.Remove(outputString.Length - num3, num3);
						outputString.Append(text2);
					}
					else
					{
						uint num4 = <PrivateImplementationDetails>.ComputeStringHash(token);
						if (num4 > 1642453538U)
						{
							if (num4 > 1927659500U)
							{
								if (num4 <= 3294919074U)
								{
									if (num4 <= 2295634285U)
									{
										if (num4 != 2093905499U)
										{
											if (num4 != 2295634285U)
											{
												goto IL_0ABA;
											}
											if (!(token == ".wgp"))
											{
												goto IL_0ABA;
											}
											goto IL_0A90;
										}
										else
										{
											if (!(token == ".wap"))
											{
												goto IL_0ABA;
											}
											goto IL_0A90;
										}
									}
									else if (num4 != 3194106265U)
									{
										if (num4 != 3262202479U)
										{
											if (num4 != 3294919074U)
											{
												goto IL_0ABA;
											}
											if (!(token == ".sdp"))
											{
												goto IL_0ABA;
											}
											this.AddSuffixStrongDativePlural(outputString);
											goto IL_0ABA;
										}
										else if (!(token == ".sap"))
										{
											goto IL_0ABA;
										}
									}
									else
									{
										if (!(token == ".sgp"))
										{
											goto IL_0ABA;
										}
										goto IL_0A90;
									}
								}
								else if (num4 <= 3363015288U)
								{
									if (num4 != 3362880480U)
									{
										if (num4 != 3363015288U)
										{
											goto IL_0ABA;
										}
										if (!(token == ".snp"))
										{
											goto IL_0ABA;
										}
									}
									else
									{
										if (!(token == ".mdp"))
										{
											goto IL_0ABA;
										}
										goto IL_0A90;
									}
								}
								else if (num4 != 3665422075U)
								{
									if (num4 != 3834331098U)
									{
										if (num4 != 3867047693U)
										{
											goto IL_0ABA;
										}
										if (!(token == ".map"))
										{
											goto IL_0ABA;
										}
										goto IL_0A90;
									}
									else
									{
										if (!(token == ".mnp"))
										{
											goto IL_0ABA;
										}
										goto IL_0A90;
									}
								}
								else
								{
									if (!(token == ".mgp"))
									{
										goto IL_0ABA;
									}
									goto IL_0A90;
								}
								this.AddSuffixStrongAccusativePlural(outputString);
								goto IL_0ABA;
							}
							if (num4 <= 1743119252U)
							{
								if (num4 <= 1675273301U)
								{
									if (num4 != 1662879919U)
									{
										if (num4 != 1675273301U)
										{
											goto IL_0ABA;
										}
										if (!(token == ".pd"))
										{
											goto IL_0ABA;
										}
										goto IL_0AAE;
									}
									else
									{
										if (!(token == ".ma"))
										{
											goto IL_0ABA;
										}
										this.AddSuffixMixedAccusative(outputString);
										goto IL_0ABA;
									}
								}
								else if (num4 != 1680098823U)
								{
									if (num4 != 1680457860U)
									{
										if (num4 != 1743119252U)
										{
											goto IL_0ABA;
										}
										if (!(token == ".wn"))
										{
											goto IL_0ABA;
										}
										this.AddSuffixWeakNominative(outputString);
										goto IL_0ABA;
									}
									else
									{
										if (!(token == ".pgp"))
										{
											goto IL_0ABA;
										}
										goto IL_0AAE;
									}
								}
								else
								{
									if (!(token == ".np"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixNounNominativePlural(outputString);
									goto IL_0ABA;
								}
							}
							else if (num4 <= 1746768014U)
							{
								if (num4 != 1745635181U)
								{
									if (num4 != 1746768014U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".mn"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixMixedNominative(outputString);
									goto IL_0ABA;
								}
								else
								{
									if (!(token == ".dp"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixNounDativePlural(outputString);
									goto IL_0ABA;
								}
							}
							else if (num4 != 1775939015U)
							{
								if (num4 != 1859563286U)
								{
									if (num4 != 1927659500U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".wnp"))
									{
										goto IL_0ABA;
									}
								}
								else if (!(token == ".wdp"))
								{
									goto IL_0ABA;
								}
							}
							else
							{
								if (!(token == ".pn"))
								{
									goto IL_0ABA;
								}
								goto IL_0AAE;
							}
							IL_0A90:
							this.AddSuffixMixedDativePlural(outputString);
							goto IL_0ABA;
						}
						if (num4 <= 1478729074U)
						{
							if (num4 <= 1205647064U)
							{
								if (num4 <= 1104981350U)
								{
									if (num4 != 1088203731U)
									{
										if (num4 != 1104981350U)
										{
											goto IL_0ABA;
										}
										if (!(token == ".sd"))
										{
											goto IL_0ABA;
										}
										this.AddSuffixStrongDative(outputString);
										goto IL_0ABA;
									}
									else
									{
										if (!(token == ".sg"))
										{
											goto IL_0ABA;
										}
										this.AddSuffixStrongGenitive(outputString);
										goto IL_0ABA;
									}
								}
								else if (num4 != 1188869445U)
								{
									if (num4 != 1205647064U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".sn"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixStrongNominative(outputString);
									goto IL_0ABA;
								}
								else
								{
									if (!(token == ".sa"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixStrongAccusative(outputString);
									goto IL_0ABA;
								}
							}
							else if (num4 <= 1309564182U)
							{
								if (num4 != 1241467968U)
								{
									if (num4 != 1309564182U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".gp"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixNounGenitivePlural(outputString);
									goto IL_0ABA;
								}
								else
								{
									if (!(token == ".ap"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixNounAccusativePlural(outputString);
									goto IL_0ABA;
								}
							}
							else if (num4 != 1423190704U)
							{
								if (num4 != 1446012479U)
								{
									if (num4 != 1478729074U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".pap"))
									{
										goto IL_0ABA;
									}
								}
								else if (!(token == ".pdp"))
								{
									goto IL_0ABA;
								}
							}
							else
							{
								if (!(token == ".a"))
								{
									goto IL_0ABA;
								}
								this.AddSuffixNounAccusative(outputString);
								goto IL_0ABA;
							}
						}
						else if (num4 <= 1591385206U)
						{
							if (num4 <= 1514108693U)
							{
								if (num4 != 1507078799U)
								{
									if (num4 != 1514108693U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".pnp"))
									{
										goto IL_0ABA;
									}
								}
								else
								{
									if (!(token == ".d"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixNounDative(outputString);
									goto IL_0ABA;
								}
							}
							else if (num4 != 1523856418U)
							{
								if (num4 != 1578991824U)
								{
									if (num4 != 1591385206U)
									{
										goto IL_0ABA;
									}
									if (!(token == ".pa"))
									{
										goto IL_0ABA;
									}
								}
								else
								{
									if (!(token == ".md"))
									{
										goto IL_0ABA;
									}
									this.AddSuffixMixedDative(outputString);
									goto IL_0ABA;
								}
							}
							else
							{
								if (!(token == ".g"))
								{
									goto IL_0ABA;
								}
								this.AddSuffixNounGenitive(outputString);
								goto IL_0ABA;
							}
						}
						else if (num4 <= 1624940444U)
						{
							if (num4 != 1592120681U)
							{
								if (num4 != 1624940444U)
								{
									goto IL_0ABA;
								}
								if (!(token == ".pg"))
								{
									goto IL_0ABA;
								}
							}
							else
							{
								if (!(token == ".wa"))
								{
									goto IL_0ABA;
								}
								this.AddSuffixWeakAccusative(outputString);
								goto IL_0ABA;
							}
						}
						else if (num4 != 1625675919U)
						{
							if (num4 != 1629324681U)
							{
								if (num4 != 1642453538U)
								{
									goto IL_0ABA;
								}
								if (!(token == ".wd"))
								{
									goto IL_0ABA;
								}
								this.AddSuffixWeakDative(outputString);
								goto IL_0ABA;
							}
							else
							{
								if (!(token == ".mg"))
								{
									goto IL_0ABA;
								}
								this.AddSuffixMixedGenitive(outputString);
								goto IL_0ABA;
							}
						}
						else
						{
							if (!(token == ".wg"))
							{
								goto IL_0ABA;
							}
							this.AddSuffixWeakGenitive(outputString);
							goto IL_0ABA;
						}
						IL_0AAE:
						this.AddPronounArticle(sourceText, cursorPos, token, ref outputString);
					}
				}
				IL_0ABA:
				GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.NoDeclination;
			}
			if (flag)
			{
				cursorPos += this.LinkEndingLength;
				outputString.Append(this.LinkEnding);
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000098B0 File Offset: 0x00007AB0
		private void HandlePossession(StringBuilder outputString, int cursorPos)
		{
			string text = outputString.ToString();
			bool flag = false;
			if (text.Length < cursorPos)
			{
				cursorPos = text.Length;
			}
			if (text.EndsWith("</b></a>"))
			{
				cursorPos -= 8;
				outputString.Remove(outputString.Length - 8, 8);
				flag = true;
			}
			char lastCharacter = GermanTextProcessor.GetLastCharacter(outputString, cursorPos);
			if (GermanTextProcessor.SSounds.Contains(char.ToLower(lastCharacter, GermanTextProcessor.CultureInfo)))
			{
				outputString.Append('\'');
			}
			else
			{
				outputString.Append('s');
			}
			if (flag)
			{
				outputString.Append("</b></a>");
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009940 File Offset: 0x00007B40
		private void AddSuffixWordGroup(string token, int wordGroupIndex, StringBuilder outputString)
		{
			bool flag = char.IsUpper(outputString[outputString.Length - GermanTextProcessor.WordGroupsNoTags[wordGroupIndex].Length]);
			string text = GermanTextProcessor.WordGroups[wordGroupIndex].Item1;
			outputString.Remove(outputString.Length - GermanTextProcessor.WordGroupsNoTags[wordGroupIndex].Length, GermanTextProcessor.WordGroupsNoTags[wordGroupIndex].Length);
			text = text.Replace("{.nn}", token);
			if (token.Equals("{.n}"))
			{
				text = text.Replace("{.nnp}", "{.np}");
				text = text.Replace("{.pnpgroup}", "{.pn}");
				text = text.Replace("{.ajw}", "{.wn}");
				text = text.Replace("{.ajm}", "{.mn}");
				text = text.Replace("{.ajs}", "{.sn}");
				text = text.Replace("{.ajwp}", "{.wnp}");
				text = text.Replace("{.ajmp}", "{.mnp}");
				text = text.Replace("{.ajsp}", "{.snp}");
			}
			else
			{
				text = text.Replace("{.ajw}", token.Insert(2, "w"));
				text = text.Replace("{.ajm}", token.Insert(2, "m"));
				text = text.Replace("{.ajs}", token.Insert(2, "s"));
				text = text.Replace("{.pngroup}", token.Insert(2, "p"));
				if (token.Contains("p"))
				{
					text = text.Replace("{.nnp}", token);
					text = text.Replace("{.ajwp}", token.Insert(2, "w"));
					text = text.Replace("{.ajmp}", token.Insert(2, "m"));
					text = text.Replace("{.ajsp}", token.Insert(2, "s"));
					text = text.Replace("{.pnpgroup}", token.Insert(2, "p"));
				}
				else
				{
					text = text.Replace("{.nnp}", token.Insert(3, "p"));
					text = text.Replace("{.ajwp}", token.Insert(2, "w").Insert(4, "p"));
					text = text.Replace("{.ajmp}", token.Insert(2, "m").Insert(4, "p"));
					text = text.Replace("{.ajsp}", token.Insert(2, "s").Insert(4, "p"));
					text = text.Replace("{.pnpgroup}", token.Insert(2, "p").Insert(4, "p"));
				}
			}
			GermanTextProcessor._doesComeFromWordGroup = true;
			string text2 = base.Process(text);
			GermanTextProcessor._doesComeFromWordGroup = false;
			if (flag && char.IsLower(text2[0]))
			{
				outputString.Append(char.ToUpperInvariant(text2[0]));
				outputString.Append(text2.Substring(1));
				return;
			}
			if (!flag && char.IsUpper(text2[0]))
			{
				outputString.Append(char.ToLowerInvariant(text2[0]));
				outputString.Append(text2.Substring(1));
				return;
			}
			outputString.Append(text2);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009C60 File Offset: 0x00007E60
		private bool IsWordGroup(int tokenLength, string sourceText, int curPos, out int wordGroupIndex)
		{
			int num = 0;
			wordGroupIndex = -1;
			int num2 = GermanTextProcessor.WordGroupsNoTags.Count - 1;
			while (0 <= num2)
			{
				if (curPos - tokenLength - 2 - GermanTextProcessor.WordGroupsNoTags[num2].Length >= 0 && GermanTextProcessor.WordGroupsNoTags[num2].Length > num && sourceText.Substring(curPos - tokenLength - 2 - GermanTextProcessor.WordGroupsNoTags[num2].Length, GermanTextProcessor.WordGroupsNoTags[num2].Length).Equals(GermanTextProcessor.WordGroupsNoTags[num2]))
				{
					wordGroupIndex = num2;
					num = GermanTextProcessor.WordGroupsNoTags[num2].Length;
				}
				num2--;
			}
			return num > 0;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009D14 File Offset: 0x00007F14
		private void AddSuffixNounNominativePlural(StringBuilder outputString)
		{
			if (!this.Feminine)
			{
				if ((this.Masculine || this.Neuter) && !this.AddSuffixForNDeclension(outputString))
				{
					outputString.Append('e');
				}
				return;
			}
			if (GermanTextProcessor.GetLastCharacter(outputString) == 'e')
			{
				outputString.Append('n');
				return;
			}
			if (GermanTextProcessor.GetEnding(outputString, 2) == "in")
			{
				outputString.Append("nen");
				return;
			}
			outputString.Append("e");
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009D8C File Offset: 0x00007F8C
		private void AddSuffixNounGenitive(StringBuilder outputString)
		{
			if (!this.Feminine && (this.Masculine || this.Neuter) && !this.AddSuffixForNDeclension(outputString))
			{
				char lastCharacter = GermanTextProcessor.GetLastCharacter(outputString);
				if (!this.IsVowel(lastCharacter))
				{
					outputString.Append('e');
				}
				outputString.Append('s');
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009DDC File Offset: 0x00007FDC
		private void AddSuffixNounGenitivePlural(StringBuilder outputString)
		{
			if (!this.Feminine)
			{
				if ((this.Masculine || this.Neuter) && !this.AddSuffixForNDeclension(outputString))
				{
					char lastCharacter = GermanTextProcessor.GetLastCharacter(outputString);
					if (!this.IsVowel(lastCharacter))
					{
						outputString.Append("e");
					}
				}
				return;
			}
			if (GermanTextProcessor.GetLastCharacter(outputString) == 'e')
			{
				outputString.Append('n');
				return;
			}
			if (GermanTextProcessor.GetEnding(outputString, 2) == "in")
			{
				outputString.Append("nen");
				return;
			}
			outputString.Append("e");
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009E67 File Offset: 0x00008067
		private void AddSuffixNounDative(StringBuilder outputString)
		{
			if (this.Masculine || this.Neuter)
			{
				this.AddSuffixForNDeclension(outputString);
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009E84 File Offset: 0x00008084
		private void AddSuffixNounDativePlural(StringBuilder outputString)
		{
			char lastCharacter = GermanTextProcessor.GetLastCharacter(outputString);
			if (!this.IsVowel(lastCharacter) && lastCharacter != 'r')
			{
				outputString.Append('e');
			}
			outputString.Append('n');
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009EB8 File Offset: 0x000080B8
		private void AddSuffixNounAccusative(StringBuilder outputString)
		{
			if (this.Masculine || this.Neuter)
			{
				this.AddSuffixForNDeclension(outputString);
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00009ED2 File Offset: 0x000080D2
		private void AddSuffixNounAccusativePlural(StringBuilder outputString)
		{
			this.AddSuffixNounNominativePlural(outputString);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009EDB File Offset: 0x000080DB
		private void AddSuffixWeakNominative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Plural)
			{
				outputString.Append("en");
				return;
			}
			outputString.Append('e');
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00009F04 File Offset: 0x00008104
		private void AddSuffixMixedNominative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Masculine)
			{
				outputString.Append("er");
				return;
			}
			if (this.Feminine)
			{
				outputString.Append("e");
				return;
			}
			if (this.Neuter)
			{
				outputString.Append("es");
				return;
			}
			if (this.Plural)
			{
				outputString.Append("en");
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00009F6C File Offset: 0x0000816C
		private void AddSuffixStrongNominative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Masculine)
			{
				outputString.Append("er");
				return;
			}
			if (this.Feminine || this.Plural)
			{
				outputString.Append("e");
				return;
			}
			if (this.Neuter)
			{
				outputString.Append("es");
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009FC6 File Offset: 0x000081C6
		private void AddSuffixWeakAccusative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Masculine || this.Plural)
			{
				outputString.Append("en");
				return;
			}
			outputString.Append("e");
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009FF8 File Offset: 0x000081F8
		private void AddSuffixMixedAccusative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Masculine || this.Plural)
			{
				outputString.Append("en");
				return;
			}
			if (this.Feminine)
			{
				outputString.Append("e");
				return;
			}
			outputString.Append("es");
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000A04C File Offset: 0x0000824C
		private void AddSuffixStrongAccusative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Masculine)
			{
				outputString.Append("en");
				return;
			}
			if (this.Feminine || this.Plural)
			{
				outputString.Append("e");
				return;
			}
			outputString.Append("es");
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000A09E File Offset: 0x0000829E
		private void AddSuffixWeakDative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000A0B3 File Offset: 0x000082B3
		private void AddSuffixMixedDative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000A0C8 File Offset: 0x000082C8
		private void AddSuffixStrongDative(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Feminine)
			{
				outputString.Append("er");
				return;
			}
			if (this.Masculine || this.Neuter)
			{
				outputString.Append("em");
				return;
			}
			outputString.Append("en");
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000A11A File Offset: 0x0000831A
		private void AddSuffixWeakGenitive(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000A12F File Offset: 0x0000832F
		private void AddSuffixMixedGenitive(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000A144 File Offset: 0x00008344
		private void AddSuffixStrongGenitive(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			if (this.Feminine || this.Plural)
			{
				outputString.Append("er");
				return;
			}
			if (this.Masculine || this.Neuter)
			{
				outputString.Append("en");
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000A191 File Offset: 0x00008391
		private void AddSuffixWeakNominativePlural(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000A1A6 File Offset: 0x000083A6
		private void AddSuffixMixedNominativePlural(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000A1BB File Offset: 0x000083BB
		private void AddSuffixStrongNominativePlural(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("e");
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000A1D0 File Offset: 0x000083D0
		private void AddSuffixStrongAccusativePlural(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("e");
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000A1E5 File Offset: 0x000083E5
		private void AddSuffixMixedDativePlural(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000A1FA File Offset: 0x000083FA
		private void AddSuffixStrongDativePlural(StringBuilder outputString)
		{
			this.ModifyAdjective(outputString);
			outputString.Append("en");
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000A210 File Offset: 0x00008410
		private bool AddSuffixForNDeclension(StringBuilder outputString)
		{
			bool flag = false;
			string ending = GermanTextProcessor.GetEnding(outputString, 3);
			if (ending == "ent" || ending == "ant" || ending == "ist" || ending.EndsWith("at"))
			{
				flag = true;
				outputString.Append("en");
			}
			else if (ending.EndsWith("e"))
			{
				flag = true;
				outputString.Append("n");
			}
			return flag;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000A288 File Offset: 0x00008488
		private void AddPronounArticle(string sourceText, int cursorPos, string token, ref StringBuilder outputString)
		{
			int num = sourceText.Remove(cursorPos - token.Length - 2).LastIndexOf('}') + 1;
			int num2 = cursorPos - token.Length - 2 - num;
			string text = "";
			if (num2 > 0 && GermanTextProcessor._curGender != GermanTextProcessor.WordGenderEnum.NoDeclination)
			{
				string text2 = sourceText.Substring(num, num2);
				char c = text2[0];
				text2 = text2.ToLowerInvariant();
				Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord> dictionary;
				if (GermanTextProcessor.PronounArticleDictionary.TryGetValue(text2, out dictionary))
				{
					text = GermanTextProcessor.DictionaryWordWithCase(token, dictionary[GermanTextProcessor._curGender]);
				}
				if (text.Length > 0)
				{
					if (char.IsLower(c))
					{
						text = text.ToLowerInvariant();
					}
					outputString.Remove(outputString.Length - num2, num2);
					outputString.Append(text);
					Dictionary<string, string> dictionary2;
					if (GermanTextProcessor.ArticlePronounReplacementDictionary.TryGetValue(text.ToLowerInvariant(), out dictionary2))
					{
						string previousWord = this.GetPreviousWord(sourceText, cursorPos, token, ref outputString);
						string text3;
						if (dictionary2.TryGetValue(previousWord, out text3))
						{
							outputString = outputString.Replace(previousWord + " " + text, text3);
						}
					}
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000A390 File Offset: 0x00008590
		private string GetPreviousWord(string sourceText, int cursorPos, string token, ref StringBuilder outputString)
		{
			string[] array = sourceText.Substring(0, cursorPos).Split(new char[] { ' ' });
			int num = array.Length;
			if (num < 2)
			{
				return "";
			}
			return array[num - 2];
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A3C9 File Offset: 0x000085C9
		private void SetFeminine()
		{
			GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.Feminine;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A3D1 File Offset: 0x000085D1
		private void SetNeuter()
		{
			GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.Neuter;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000A3D9 File Offset: 0x000085D9
		private void SetMasculine()
		{
			GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.Masculine;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000A3E1 File Offset: 0x000085E1
		private void SetPlural()
		{
			GermanTextProcessor._curGender = GermanTextProcessor.WordGenderEnum.Plural;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000A3E9 File Offset: 0x000085E9
		private bool IsVowel(char c)
		{
			return Array.IndexOf<char>(GermanTextProcessor.Vowels, c) >= 0;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000A3FC File Offset: 0x000085FC
		private int FindLastVowel(StringBuilder outputText)
		{
			for (int i = outputText.Length - 1; i >= 0; i--)
			{
				if (this.IsVowel(outputText[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000A42E File Offset: 0x0000862E
		private void RemoveLastCharacter(StringBuilder outputString)
		{
			outputString.Remove(outputString.Length - 1, 1);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000A440 File Offset: 0x00008640
		private bool IsLastCharVowel(StringBuilder outputString)
		{
			char c = outputString[outputString.Length - 1];
			return this.IsVowel(c);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000A464 File Offset: 0x00008664
		private void ModifyAdjective(StringBuilder outputString)
		{
			string ending = GermanTextProcessor.GetEnding(outputString, 2);
			if (this.Neuter && ending == "es")
			{
				outputString.Remove(outputString.Length - 2, 2);
				return;
			}
			if (GermanTextProcessor._curGender != GermanTextProcessor.WordGenderEnum.NoDeclination && ending[1] == 'e')
			{
				outputString.Remove(outputString.Length - 1, 1);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000A4C4 File Offset: 0x000086C4
		private string GetVowelEnding(StringBuilder outputString)
		{
			if (outputString.Length == 0)
			{
				return "";
			}
			char c = outputString[outputString.Length - 1];
			if (!this.IsVowel(c))
			{
				return "";
			}
			if (outputString.Length > 1 && this.IsVowel(outputString[outputString.Length - 2]))
			{
				return outputString[outputString.Length - 2].ToString() + c.ToString();
			}
			return c.ToString();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000A548 File Offset: 0x00008748
		private bool IsLink(string sourceText, int tokenLength, int cursorPos)
		{
			string text = sourceText.Remove(cursorPos - tokenLength);
			for (int i = 0; i < GermanTextProcessor.LinkList.Count; i++)
			{
				if (sourceText.Length >= GermanTextProcessor.LinkList[i].Length && text.EndsWith(GermanTextProcessor.LinkList[i]))
				{
					GermanTextProcessor.LinkList.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000A5B0 File Offset: 0x000087B0
		private bool ShouldDeclineWord(string sourceText, int cursorPos, string token)
		{
			int num = sourceText.Remove(cursorPos - token.Length - 2).LastIndexOf('}') + 1;
			if (cursorPos - token.Length - 2 - num > 0)
			{
				string text = sourceText.Substring(num, cursorPos - token.Length - 2 - num);
				List<string> list;
				if (GermanTextProcessor.DoNotDeclineList.TryGetValue(char.ToUpper(text[0]), out list))
				{
					return !list.Contains(text);
				}
			}
			return true;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000A620 File Offset: 0x00008820
		private bool IsIrregularWord(string sourceText, int cursorPos, string token, out string irregularWord, out int lengthOfWordToReplace)
		{
			int num = sourceText.Remove(cursorPos - token.Length - 2).LastIndexOf('}') + 1;
			lengthOfWordToReplace = cursorPos - token.Length - 2 - num;
			irregularWord = "";
			string text = "";
			if (lengthOfWordToReplace > 0)
			{
				text = sourceText.Substring(num, lengthOfWordToReplace);
				char c = char.ToUpperInvariant(text[0]);
				List<GermanTextProcessor.DictionaryWord> list;
				if (this.Masculine && GermanTextProcessor.IrregularMasculineDictionary.TryGetValue(c, out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Nominative.Equals(text, StringComparison.InvariantCultureIgnoreCase))
						{
							irregularWord = GermanTextProcessor.DictionaryWordWithCase(token, list[i]);
							break;
						}
					}
				}
				else if (this.Feminine && GermanTextProcessor.IrregularFeminineDictionary.TryGetValue(c, out list))
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Nominative.Equals(text, StringComparison.InvariantCultureIgnoreCase))
						{
							irregularWord = GermanTextProcessor.DictionaryWordWithCase(token, list[j]);
							break;
						}
					}
				}
				else if (this.Neuter && GermanTextProcessor.IrregularNeuterDictionary.TryGetValue(c, out list))
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].Nominative.Equals(text, StringComparison.InvariantCultureIgnoreCase))
						{
							irregularWord = GermanTextProcessor.DictionaryWordWithCase(token, list[k]);
							break;
						}
					}
				}
				else if (this.Plural && GermanTextProcessor.IrregularPluralDictionary.TryGetValue(c, out list))
				{
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].Nominative.Equals(text, StringComparison.InvariantCultureIgnoreCase))
						{
							irregularWord = GermanTextProcessor.DictionaryWordWithCase(token, list[l]);
							break;
						}
					}
				}
			}
			if (irregularWord.Length > 0)
			{
				if (char.IsLower(text[0]))
				{
					irregularWord = irregularWord.ToLower();
				}
				return true;
			}
			return false;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000A814 File Offset: 0x00008A14
		private bool IsRecordedWithPreviousTag(string sourceText, int cursorPos)
		{
			for (int i = 0; i < GermanTextProcessor.WordGroups.Count; i++)
			{
				if (GermanTextProcessor.WordGroups[i].Item1 == sourceText && GermanTextProcessor.WordGroups[i].Item2 != cursorPos)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000A864 File Offset: 0x00008A64
		private void WordGroupProcessor(string sourceText, int cursorPos)
		{
			if (!this.IsRecordedWithPreviousTag(sourceText, cursorPos))
			{
				GermanTextProcessor.WordGroups.Add(new ValueTuple<string, int>(sourceText, cursorPos));
				string text = sourceText.Replace("{.nn}", "{.n}");
				text = text.Replace("{.pngroup}", "{.pn}");
				text = text.Replace("{.ajw}", "{.wn}");
				text = text.Replace("{.ajm}", "{.mn}");
				text = text.Replace("{.ajs}", "{.sn}");
				text = text.Replace("{.nnp}", "{.np}");
				text = text.Replace("{.pnpgroup}", "{.pnp}");
				text = text.Replace("{.ajwp}", "{.wnp}");
				text = text.Replace("{.ajmp}", "{.mnp}");
				text = text.Replace("{.ajsp}", "{.snp}");
				GermanTextProcessor._doesComeFromWordGroup = true;
				GermanTextProcessor.WordGroupsNoTags.Add(base.Process(text));
				GermanTextProcessor._doesComeFromWordGroup = false;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000A958 File Offset: 0x00008B58
		private static string DictionaryWordWithCase(string token, GermanTextProcessor.DictionaryWord dictionaryWord)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(token);
			if (num <= 1662879919U)
			{
				if (num <= 1423190704U)
				{
					if (num <= 1104981350U)
					{
						if (num <= 774788651U)
						{
							if (num != 534400075U)
							{
								if (num != 711915317U)
								{
									if (num != 774788651U)
									{
										goto IL_0749;
									}
									if (!(token == ".ajmp"))
									{
										goto IL_0749;
									}
									goto IL_0718;
								}
								else
								{
									if (!(token == ".ajsp"))
									{
										goto IL_0749;
									}
									goto IL_0718;
								}
							}
							else
							{
								if (!(token == ".nnp"))
								{
									goto IL_0749;
								}
								goto IL_0718;
							}
						}
						else if (num != 845547889U)
						{
							if (num != 1088203731U)
							{
								if (num != 1104981350U)
								{
									goto IL_0749;
								}
								if (!(token == ".sd"))
								{
									goto IL_0749;
								}
								goto IL_073B;
							}
							else
							{
								if (!(token == ".sg"))
								{
									goto IL_0749;
								}
								goto IL_072D;
							}
						}
						else
						{
							if (!(token == ".ajwp"))
							{
								goto IL_0749;
							}
							goto IL_0718;
						}
					}
					else if (num <= 1241467968U)
					{
						if (num != 1188869445U)
						{
							if (num != 1205647064U)
							{
								if (num != 1241467968U)
								{
									goto IL_0749;
								}
								if (!(token == ".ap"))
								{
									goto IL_0749;
								}
								goto IL_0726;
							}
							else
							{
								if (!(token == ".sn"))
								{
									goto IL_0749;
								}
								goto IL_0711;
							}
						}
						else if (!(token == ".sa"))
						{
							goto IL_0749;
						}
					}
					else if (num != 1243880729U)
					{
						if (num != 1309564182U)
						{
							if (num != 1423190704U)
							{
								goto IL_0749;
							}
							if (!(token == ".a"))
							{
								goto IL_0749;
							}
						}
						else
						{
							if (!(token == ".gp"))
							{
								goto IL_0749;
							}
							goto IL_0734;
						}
					}
					else
					{
						if (!(token == ".nn"))
						{
							goto IL_0749;
						}
						goto IL_0711;
					}
				}
				else if (num <= 1578991824U)
				{
					if (num <= 1507078799U)
					{
						if (num != 1446012479U)
						{
							if (num != 1478729074U)
							{
								if (num != 1507078799U)
								{
									goto IL_0749;
								}
								if (!(token == ".d"))
								{
									goto IL_0749;
								}
								goto IL_073B;
							}
							else
							{
								if (!(token == ".pap"))
								{
									goto IL_0749;
								}
								goto IL_0726;
							}
						}
						else
						{
							if (!(token == ".pdp"))
							{
								goto IL_0749;
							}
							goto IL_0742;
						}
					}
					else if (num != 1514108693U)
					{
						if (num != 1523856418U)
						{
							if (num != 1578991824U)
							{
								goto IL_0749;
							}
							if (!(token == ".md"))
							{
								goto IL_0749;
							}
							goto IL_073B;
						}
						else
						{
							if (!(token == ".g"))
							{
								goto IL_0749;
							}
							goto IL_072D;
						}
					}
					else
					{
						if (!(token == ".pnp"))
						{
							goto IL_0749;
						}
						goto IL_0718;
					}
				}
				else if (num <= 1624940444U)
				{
					if (num != 1591385206U)
					{
						if (num != 1592120681U)
						{
							if (num != 1624940444U)
							{
								goto IL_0749;
							}
							if (!(token == ".pg"))
							{
								goto IL_0749;
							}
							goto IL_072D;
						}
						else if (!(token == ".wa"))
						{
							goto IL_0749;
						}
					}
					else if (!(token == ".pa"))
					{
						goto IL_0749;
					}
				}
				else if (num <= 1629324681U)
				{
					if (num != 1625675919U)
					{
						if (num != 1629324681U)
						{
							goto IL_0749;
						}
						if (!(token == ".mg"))
						{
							goto IL_0749;
						}
						goto IL_072D;
					}
					else
					{
						if (!(token == ".wg"))
						{
							goto IL_0749;
						}
						goto IL_072D;
					}
				}
				else if (num != 1642453538U)
				{
					if (num != 1662879919U)
					{
						goto IL_0749;
					}
					if (!(token == ".ma"))
					{
						goto IL_0749;
					}
				}
				else
				{
					if (!(token == ".wd"))
					{
						goto IL_0749;
					}
					goto IL_073B;
				}
				return dictionaryWord.Accusative;
				IL_072D:
				return dictionaryWord.Genitive;
			}
			if (num <= 2093905499U)
			{
				if (num <= 1745635181U)
				{
					if (num <= 1680098823U)
					{
						if (num != 1674854989U)
						{
							if (num != 1675273301U)
							{
								if (num != 1680098823U)
								{
									goto IL_0749;
								}
								if (!(token == ".np"))
								{
									goto IL_0749;
								}
								goto IL_0718;
							}
							else
							{
								if (!(token == ".pd"))
								{
									goto IL_0749;
								}
								goto IL_073B;
							}
						}
						else if (!(token == ".n"))
						{
							goto IL_0749;
						}
					}
					else if (num != 1680457860U)
					{
						if (num != 1743119252U)
						{
							if (num != 1745635181U)
							{
								goto IL_0749;
							}
							if (!(token == ".dp"))
							{
								goto IL_0749;
							}
							goto IL_0742;
						}
						else if (!(token == ".wn"))
						{
							goto IL_0749;
						}
					}
					else
					{
						if (!(token == ".pgp"))
						{
							goto IL_0749;
						}
						goto IL_0734;
					}
				}
				else if (num <= 1811819932U)
				{
					if (num != 1746768014U)
					{
						if (num != 1775939015U)
						{
							if (num != 1811819932U)
							{
								goto IL_0749;
							}
							if (!(token == ".pnpgroup"))
							{
								goto IL_0749;
							}
							goto IL_0718;
						}
						else if (!(token == ".pn"))
						{
							goto IL_0749;
						}
					}
					else if (!(token == ".mn"))
					{
						goto IL_0749;
					}
				}
				else if (num != 1859563286U)
				{
					if (num != 1927659500U)
					{
						if (num != 2093905499U)
						{
							goto IL_0749;
						}
						if (!(token == ".wap"))
						{
							goto IL_0749;
						}
						goto IL_0726;
					}
					else
					{
						if (!(token == ".wnp"))
						{
							goto IL_0749;
						}
						goto IL_0718;
					}
				}
				else
				{
					if (!(token == ".wdp"))
					{
						goto IL_0749;
					}
					goto IL_0742;
				}
			}
			else if (num <= 3290807067U)
			{
				if (num <= 2921699449U)
				{
					if (num != 2141918926U)
					{
						if (num != 2295634285U)
						{
							if (num != 2921699449U)
							{
								goto IL_0749;
							}
							if (!(token == ".ajm"))
							{
								goto IL_0749;
							}
						}
						else
						{
							if (!(token == ".wgp"))
							{
								goto IL_0749;
							}
							goto IL_0734;
						}
					}
					else if (!(token == ".pngroup"))
					{
						goto IL_0749;
					}
				}
				else if (num != 3194106265U)
				{
					if (num != 3262202479U)
					{
						if (num != 3290807067U)
						{
							goto IL_0749;
						}
						if (!(token == ".ajw"))
						{
							goto IL_0749;
						}
					}
					else
					{
						if (!(token == ".sap"))
						{
							goto IL_0749;
						}
						goto IL_0726;
					}
				}
				else
				{
					if (!(token == ".sgp"))
					{
						goto IL_0749;
					}
					goto IL_0734;
				}
			}
			else if (num <= 3362880480U)
			{
				if (num != 3294919074U)
				{
					if (num != 3357917543U)
					{
						if (num != 3362880480U)
						{
							goto IL_0749;
						}
						if (!(token == ".mdp"))
						{
							goto IL_0749;
						}
						goto IL_0742;
					}
					else if (!(token == ".ajs"))
					{
						goto IL_0749;
					}
				}
				else
				{
					if (!(token == ".sdp"))
					{
						goto IL_0749;
					}
					goto IL_0742;
				}
			}
			else if (num <= 3665422075U)
			{
				if (num != 3363015288U)
				{
					if (num != 3665422075U)
					{
						goto IL_0749;
					}
					if (!(token == ".mgp"))
					{
						goto IL_0749;
					}
					goto IL_0734;
				}
				else
				{
					if (!(token == ".snp"))
					{
						goto IL_0749;
					}
					goto IL_0718;
				}
			}
			else if (num != 3834331098U)
			{
				if (num != 3867047693U)
				{
					goto IL_0749;
				}
				if (!(token == ".map"))
				{
					goto IL_0749;
				}
				goto IL_0726;
			}
			else
			{
				if (!(token == ".mnp"))
				{
					goto IL_0749;
				}
				goto IL_0718;
			}
			IL_0711:
			return dictionaryWord.Nominative;
			IL_0718:
			return dictionaryWord.NominativePlural;
			IL_0726:
			return dictionaryWord.AccusativePlural;
			IL_0734:
			return dictionaryWord.GenitivePlural;
			IL_073B:
			return dictionaryWord.Dative;
			IL_0742:
			return dictionaryWord.DativePlural;
			IL_0749:
			return "MISSING IRREGULAR WORD IN LIST";
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000B0B4 File Offset: 0x000092B4
		private static char GetLastCharacter(StringBuilder outputText, int cursorPos)
		{
			if (cursorPos < outputText.Length)
			{
				cursorPos = outputText.Length;
			}
			for (int i = cursorPos - 1; i >= 0; i--)
			{
				if (char.IsLetter(outputText[i]))
				{
					return outputText[i];
				}
			}
			return '*';
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B0F8 File Offset: 0x000092F8
		private static char GetLastCharacter(StringBuilder outputString)
		{
			if (outputString.Length <= 0)
			{
				return '*';
			}
			return outputString[outputString.Length - 1];
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B114 File Offset: 0x00009314
		private static char GetSecondLastCharacter(StringBuilder outputString)
		{
			if (outputString.Length <= 1)
			{
				return '*';
			}
			return outputString[outputString.Length - 2];
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B130 File Offset: 0x00009330
		private static string GetEnding(StringBuilder outputString, int numChars)
		{
			numChars = MathF.Min(numChars, outputString.Length);
			return outputString.ToString(outputString.Length - numChars, numChars);
		}

		// Token: 0x040000BB RID: 187
		private static readonly CultureInfo CultureInfo = new CultureInfo("de-DE");

		// Token: 0x040000BC RID: 188
		[ThreadStatic]
		private static GermanTextProcessor.WordGenderEnum _curGender;

		// Token: 0x040000BD RID: 189
		[TupleElementNames(new string[] { "wordGroup", "firstMarkerPost" })]
		[ThreadStatic]
		private static List<ValueTuple<string, int>> _wordGroups = new List<ValueTuple<string, int>>();

		// Token: 0x040000BE RID: 190
		[ThreadStatic]
		private static List<string> _wordGroupsNoTags = new List<string>();

		// Token: 0x040000BF RID: 191
		[ThreadStatic]
		private static List<string> _linkList = new List<string>();

		// Token: 0x040000C0 RID: 192
		[ThreadStatic]
		private static bool _doesComeFromWordGroup = false;

		// Token: 0x040000C1 RID: 193
		private static char[] Vowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'ä', 'ü', 'ö' };

		// Token: 0x040000C2 RID: 194
		private const string Consonants = "BbCcDdFfGgHhJjKkLlMmNnPpRrSsTtWwYyZz";

		// Token: 0x040000C3 RID: 195
		private static char[] SSounds = new char[] { 's', 'ß', 'z', 'x' };

		// Token: 0x040000C4 RID: 196
		private static readonly Dictionary<char, List<GermanTextProcessor.DictionaryWord>> IrregularMasculineDictionary = new Dictionary<char, List<GermanTextProcessor.DictionaryWord>> { 
		{
			'N',
			new List<GermanTextProcessor.DictionaryWord>
			{
				new GermanTextProcessor.DictionaryWord("Name", "Namen", "Namens", "Namen", "Namen", "Namen", "Namen", "Namen")
			}
		} };

		// Token: 0x040000C5 RID: 197
		private static readonly Dictionary<char, List<GermanTextProcessor.DictionaryWord>> IrregularFeminineDictionary = new Dictionary<char, List<GermanTextProcessor.DictionaryWord>>();

		// Token: 0x040000C6 RID: 198
		private static readonly Dictionary<char, List<GermanTextProcessor.DictionaryWord>> IrregularNeuterDictionary = new Dictionary<char, List<GermanTextProcessor.DictionaryWord>> { 
		{
			'I',
			new List<GermanTextProcessor.DictionaryWord>
			{
				new GermanTextProcessor.DictionaryWord("Imperium", "Imperien", "Imperiums", "Imperien", "Imperium", "Imperien", "Imperium", "Imperien")
			}
		} };

		// Token: 0x040000C7 RID: 199
		private static readonly Dictionary<char, List<GermanTextProcessor.DictionaryWord>> IrregularPluralDictionary = new Dictionary<char, List<GermanTextProcessor.DictionaryWord>>();

		// Token: 0x040000C8 RID: 200
		private static readonly Dictionary<string, Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>> PronounArticleDictionary = new Dictionary<string, Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>>
		{
			{
				"der",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Der", "Die", "Des", "Der", "Dem", "Den", "Den", "Die")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Die", "Die", "Der", "Der", "Der", "Den", "Die", "Die")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Das", "Die", "Des", "Der", "Dem", "Den", "Das", "Die")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Die", "Die", "Der", "Der", "Den", "Den", "Die", "Die")
					}
				}
			},
			{
				"ein",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Ein", "", "Eines", "", "Einem", "", "Einen", "")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Eine", "", "Einer", "", "Einer", "", "Eine", "")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Ein", "", "Eines", "", "Einem", "", "Ein", "")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("", "", "", "", "", "", "", "")
					}
				}
			},
			{
				"dieser",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Dieser", "Diese", "Dieses", "Dieser", "Diesem", "Diesen", "Diesen", "Diese")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Diese", "Diese", "Dieser", "Dieser", "Dieser", "Diesen", "Diese", "Diese")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Dieses", "Diese", "Dieses", "Dieser", "Diesem", "Diesen", "Dieses", "Diese")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Diese", "Diese", "Dieser", "Dieser", "Diesen", "Diesen", "Diese", "Diese")
					}
				}
			},
			{
				"jeder",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Jeder", "Alle", "Jedes", "Aller", "Jedem", "Allen", "Jeden", "Alle")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Jede", "Alle", "Jeder", "Aller", "Jeder", "Allen", "Jede", "Alle")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Jedes", "Alle", "Jedes", "Aller", "Jedem", "Allen", "Jedes", "Alle")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Alle", "Alle", "Aller", "Aller", "Allen", "Allen", "Alle", "Alle")
					}
				}
			},
			{
				"kein",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Kein", "Keine", "Keines", "Keiner", "Keinem", "Keinen", "Keinen", "Keine")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Keine", "Keine", "Keiner", "Keiner", "Keiner", "Keinen", "Keine", "Keine")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Kein", "Keine", "Keines", "Keiner", "Keinem", "Keinen", "Kein", "Keine")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Keine", "Keine", "Keiner", "Keiner", "Keinen", "Keinen", "Keine", "Keine")
					}
				}
			},
			{
				"dein",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Dein", "Deine", "Deines", "Deiner", "Deinem", "Deinen", "Deinen", "Deine")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Deine", "Deine", "Deiner", "Deiner", "Deiner", "Deinen", "Deine", "Deine")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Dein", "Deine", "Deines", "Deiner", "Deinem", "Deinen", "Dein", "Deine")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Deine", "Deine", "Deiner", "Deiner", "Deinen", "Deinen", "Deine", "Deine")
					}
				}
			},
			{
				"ihr",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Ihr", "Ihre", "Ihres", "Ihrer", "Ihrem", "Ihren", "Ihren", "Ihre")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Ihre", "Ihre", "Ihrer", "Ihrer", "Ihrer", "Ihren", "Ihre", "Ihre")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Ihr", "Ihre", "Ihres", "Ihrer", "Ihrem", "Ihren", "Ihr", "Ihre")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Ihre", "Ihre", "Ihrer", "Ihrer", "Ihren", "Ihren", "Ihre", "Ihre")
					}
				}
			},
			{
				"euer",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Euer", "Eure", "Eures", "Eurer ", "Eurem", "Euren", "Euren", "Eure")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Eure", "Eure", "Eurer", "Eurer ", "Eurer", "Euren", "Eure", "Eure")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Euer", "Eure", "Eures", "Eurer ", "Eurem", "Euren", "Euer", "Eure")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Eure", "Eure", "Eurer ", "Eurer ", "Euren", "Euren", "Eure", "Eure")
					}
				}
			},
			{
				"welcher",
				new Dictionary<GermanTextProcessor.WordGenderEnum, GermanTextProcessor.DictionaryWord>
				{
					{
						GermanTextProcessor.WordGenderEnum.Masculine,
						new GermanTextProcessor.DictionaryWord("Welcher", "Welche", "Welches", "Welcher", "Welchem", "Welchen", "Welchen", "Welche")
					},
					{
						GermanTextProcessor.WordGenderEnum.Feminine,
						new GermanTextProcessor.DictionaryWord("Welche", "Welche", "Welcher", "Welcher", "Welcher", "Welchen", "Welche", "Welche")
					},
					{
						GermanTextProcessor.WordGenderEnum.Neuter,
						new GermanTextProcessor.DictionaryWord("Welches", "Welche", "Welches", "Welcher", "Welchem", "Welchen", "Welches", "Welche")
					},
					{
						GermanTextProcessor.WordGenderEnum.Plural,
						new GermanTextProcessor.DictionaryWord("Welche", "Welche", "Welcher", "Welcher", "Welchen", "Welchen", "Welche", "Welche")
					}
				}
			}
		};

		// Token: 0x040000C9 RID: 201
		private static readonly Dictionary<string, Dictionary<string, string>> ArticlePronounReplacementDictionary = new Dictionary<string, Dictionary<string, string>> { 
		{
			"dem",
			new Dictionary<string, string>
			{
				{ "von", "vom" },
				{ "vom", "vom" },
				{ "zu", "zum" },
				{ "an", "am" }
			}
		} };

		// Token: 0x040000CA RID: 202
		private static readonly Dictionary<char, List<string>> DoNotDeclineList = new Dictionary<char, List<string>>
		{
			{
				'A',
				new List<string> { "Avlonos", "Argoron", "Arkit", "Airit", "Aldusunit", "Asraloving", "Acapanos", "Angarys" }
			},
			{
				'B',
				new List<string>
				{
					"Banu Hulyan", "Banu Sarmal", "Banu Sarran", "Baltait", "Banu Atij", "Banu Qaraz", "Banu Ruwaid", "Beni Zilal", "Banu Habbab", "Banu Qild",
					"Banu Arbas", "Bochit", "Boranoving", "Bani Aska", "Bani Dhamin", "Bani Fasus", "Bani Julul", "Bani Kinyan", "Bani Laikh", "Bani Mushala",
					"Bani Nir", "Bani Tharuq", "Bani Yatash", "Bani Zus", "Balastisos"
				}
			},
			{
				'C',
				new List<string> { "Chonis", "Corenios", "Comnos", "Charait", "Corstases" }
			},
			{
				'D',
				new List<string>
				{
					"dey Meroc", "Dolentos", "dey Molarn", "dey Gunric", "dey Cortain", "dey Rothad", "dey Jelind", "dey Fortes", "dey Arromanc", "dey Tihr",
					"Dionicos", "dey Valant", "dey Folcun", "Delicos"
				}
			},
			{
				'E',
				new List<string> { "Eleftheroi", "Elaches", "Elysos" }
			},
			{
				'F',
				new List<string>
				{
					"fen Uvain", "fen Caernacht", "fen Gruffendoc", "fen Morcar", "fen Penraic", "fen Giall", "fen Eingal", "fen Derngil", "fen Aertus", "fen Brachar",
					"fen Crusac", "fen Domus", "fen Earach", "fen Fiachan", "fen Loen", "fen Morain", "fen Seanel", "fen Tuil", "Folyoroving"
				}
			},
			{
				'G',
				new List<string> { "Gundaroving", "Ghilman", "Gendiroving", "Gessios" }
			},
			{
				'H',
				new List<string> { "Hongeros", "Harfit" }
			},
			{
				'I',
				new List<string> { "Isyaroving", "Impestores", "Ingchit", "Iskanoving" }
			},
			{
				'J',
				new List<string> { "Julios", "Jawwal", "Jalos" }
			},
			{
				'K',
				new List<string> { "Karakhergit", "Koltit", "Kuloving", "Khergit", "Kostoroving" }
			},
			{
				'L',
				new List<string> { "Lonalion", "Leoniparden", "Lestharos" }
			},
			{
				'M',
				new List<string> { "Mestricaros", "Maneolis", "Maranjit", "Maregoving", "Meones" }
			},
			{
				'N',
				new List<string> { "Neutral", "Neretzes", "Nathanys" }
			},
			{
				'O',
				new List<string> { "Ormidoving", "Oburit", "Osticos", "Oranarit", "Opynates" }
			},
			{
				'P',
				new List<string> { "Prienicos", "Phalentes", "Pethros", "Palladios", "Paltos", "Phenigos" }
			},
			{
				'S',
				new List<string> { "Skolderbroda", "Seeratten", "Sorados", "Serapides", "Seeräuber", "Sunit", "Suratoving", "Stracanasthes", "Sumessos" }
			},
			{
				'T',
				new List<string> { "Tigrit", "Togaroving", "Tokhit", "Therycos" }
			},
			{
				'U',
				new List<string> { "Urkhunait", "Ubroving", "Ubchit" }
			},
			{
				'V',
				new List<string> { "Varros", "Vizartos", "Vagiroving", "Verborgene Hand", "Vatatzes", "Vetranis", "Vezhoving", "Vyshoving" }
			},
			{
				'Y',
				new List<string> { "Yanserit", "Yujit", "Yerchoving" }
			},
			{
				'Z',
				new List<string> { "Zhanoving", "Zebales" }
			}
		};

		// Token: 0x02000043 RID: 67
		private enum WordGenderEnum
		{
			// Token: 0x04000140 RID: 320
			Masculine,
			// Token: 0x04000141 RID: 321
			Feminine,
			// Token: 0x04000142 RID: 322
			Neuter,
			// Token: 0x04000143 RID: 323
			Plural,
			// Token: 0x04000144 RID: 324
			NoDeclination
		}

		// Token: 0x02000044 RID: 68
		private static class NounTokens
		{
			// Token: 0x04000145 RID: 325
			public const string Nominative = ".n";

			// Token: 0x04000146 RID: 326
			public const string Accusative = ".a";

			// Token: 0x04000147 RID: 327
			public const string Genitive = ".g";

			// Token: 0x04000148 RID: 328
			public const string Dative = ".d";

			// Token: 0x04000149 RID: 329
			public const string NominativePlural = ".np";

			// Token: 0x0400014A RID: 330
			public const string AccusativePlural = ".ap";

			// Token: 0x0400014B RID: 331
			public const string GenitivePlural = ".gp";

			// Token: 0x0400014C RID: 332
			public const string DativePlural = ".dp";

			// Token: 0x0400014D RID: 333
			public static readonly string[] TokenList = new string[] { ".n", ".a", ".g", ".d", ".np", ".ap", ".gp", ".dp" };
		}

		// Token: 0x02000045 RID: 69
		private static class AdjectiveTokens
		{
			// Token: 0x0400014E RID: 334
			public const string WeakNominative = ".wn";

			// Token: 0x0400014F RID: 335
			public const string MixedNominative = ".mn";

			// Token: 0x04000150 RID: 336
			public const string StrongNominative = ".sn";

			// Token: 0x04000151 RID: 337
			public const string WeakAccusative = ".wa";

			// Token: 0x04000152 RID: 338
			public const string MixedAccusative = ".ma";

			// Token: 0x04000153 RID: 339
			public const string StrongAccusative = ".sa";

			// Token: 0x04000154 RID: 340
			public const string WeakDative = ".wd";

			// Token: 0x04000155 RID: 341
			public const string MixedDative = ".md";

			// Token: 0x04000156 RID: 342
			public const string StrongDative = ".sd";

			// Token: 0x04000157 RID: 343
			public const string WeakGenitive = ".wg";

			// Token: 0x04000158 RID: 344
			public const string MixedGenitive = ".mg";

			// Token: 0x04000159 RID: 345
			public const string StrongGenitive = ".sg";

			// Token: 0x0400015A RID: 346
			public const string WeakNominativePlural = ".wnp";

			// Token: 0x0400015B RID: 347
			public const string MixedNominativePlural = ".mnp";

			// Token: 0x0400015C RID: 348
			public const string StrongNominativePlural = ".snp";

			// Token: 0x0400015D RID: 349
			public const string WeakAccusativePlural = ".wap";

			// Token: 0x0400015E RID: 350
			public const string MixedAccusativePlural = ".map";

			// Token: 0x0400015F RID: 351
			public const string StrongAccusativePlural = ".sap";

			// Token: 0x04000160 RID: 352
			public const string WeakDativePlural = ".wdp";

			// Token: 0x04000161 RID: 353
			public const string MixedDativePlural = ".mdp";

			// Token: 0x04000162 RID: 354
			public const string StrongDativePlural = ".sdp";

			// Token: 0x04000163 RID: 355
			public const string WeakGenitivePlural = ".wgp";

			// Token: 0x04000164 RID: 356
			public const string MixedGenitivePlural = ".mgp";

			// Token: 0x04000165 RID: 357
			public const string StrongGenitivePlural = ".sgp";

			// Token: 0x04000166 RID: 358
			public static readonly string[] TokenList = new string[]
			{
				".wn", ".mn", ".sn", ".wa", ".ma", ".sa", ".wd", ".md", ".sd", ".wg",
				".mg", ".sg", ".wnp", ".mnp", ".snp", ".wap", ".map", ".sap", ".wdp", ".mdp",
				".sdp", ".wgp", ".mgp", ".sgp"
			};
		}

		// Token: 0x02000046 RID: 70
		private static class PronounAndArticleTokens
		{
			// Token: 0x04000167 RID: 359
			public const string Nominative = ".pn";

			// Token: 0x04000168 RID: 360
			public const string Accusative = ".pa";

			// Token: 0x04000169 RID: 361
			public const string Genitive = ".pg";

			// Token: 0x0400016A RID: 362
			public const string Dative = ".pd";

			// Token: 0x0400016B RID: 363
			public const string NominativePlural = ".pnp";

			// Token: 0x0400016C RID: 364
			public const string AccusativePlural = ".pap";

			// Token: 0x0400016D RID: 365
			public const string GenitivePlural = ".pgp";

			// Token: 0x0400016E RID: 366
			public const string DativePlural = ".pdp";

			// Token: 0x0400016F RID: 367
			public static readonly string[] TokenList = new string[] { ".pn", ".pa", ".pg", ".pd", ".pnp", ".pap", ".pgp", ".pdp" };
		}

		// Token: 0x02000047 RID: 71
		private static class GenderTokens
		{
			// Token: 0x04000170 RID: 368
			public const string Masculine = ".M";

			// Token: 0x04000171 RID: 369
			public const string Feminine = ".F";

			// Token: 0x04000172 RID: 370
			public const string Neuter = ".N";

			// Token: 0x04000173 RID: 371
			public const string Plural = ".P";

			// Token: 0x04000174 RID: 372
			public static readonly string[] TokenList = new string[] { ".M", ".F", ".N", ".P" };
		}

		// Token: 0x02000048 RID: 72
		private static class WordGroupTokens
		{
			// Token: 0x04000175 RID: 373
			public const string NounNominative = ".nn";

			// Token: 0x04000176 RID: 374
			public const string PronounAndArticleNominative = ".pngroup";

			// Token: 0x04000177 RID: 375
			public const string AdjectiveNominativeWeak = ".ajw";

			// Token: 0x04000178 RID: 376
			public const string AdjectiveNominativeMixed = ".ajm";

			// Token: 0x04000179 RID: 377
			public const string AdjectiveNominativeStrong = ".ajs";

			// Token: 0x0400017A RID: 378
			public const string NounNominativeWithBrackets = "{.nn}";

			// Token: 0x0400017B RID: 379
			public const string PronounAndArticleNominativeWithBrackets = "{.pngroup}";

			// Token: 0x0400017C RID: 380
			public const string AdjectiveNominativeWeakWithBrackets = "{.ajw}";

			// Token: 0x0400017D RID: 381
			public const string AdjectiveNominativeMixedWithBrackets = "{.ajm}";

			// Token: 0x0400017E RID: 382
			public const string AdjectiveNominativeStrongWithBrackets = "{.ajs}";

			// Token: 0x0400017F RID: 383
			public const string NounNominativePlural = ".nnp";

			// Token: 0x04000180 RID: 384
			public const string PronounAndArticleNominativePlural = ".pnpgroup";

			// Token: 0x04000181 RID: 385
			public const string AdjectiveNominativeWeakPlural = ".ajwp";

			// Token: 0x04000182 RID: 386
			public const string AdjectiveNominativeMixedPlural = ".ajmp";

			// Token: 0x04000183 RID: 387
			public const string AdjectiveNominativeStrongPlural = ".ajsp";

			// Token: 0x04000184 RID: 388
			public const string NounNominativePluralWithBrackets = "{.nnp}";

			// Token: 0x04000185 RID: 389
			public const string PronounAndArticleNominativePluralWithBrackets = "{.pnpgroup}";

			// Token: 0x04000186 RID: 390
			public const string AdjectiveNominativeWeakPluralWithBrackets = "{.ajwp}";

			// Token: 0x04000187 RID: 391
			public const string AdjectiveNominativeMixedPluralWithBrackets = "{.ajmp}";

			// Token: 0x04000188 RID: 392
			public const string AdjectiveNominativeStrongPluralWithBrackets = "{.ajsp}";

			// Token: 0x04000189 RID: 393
			public static readonly string[] TokenList = new string[] { ".nn", ".pngroup", ".ajm", ".ajs", ".ajw", ".nnp", ".pnpgroup", ".ajmp", ".ajsp", ".ajwp" };
		}

		// Token: 0x02000049 RID: 73
		private static class OtherTokens
		{
			// Token: 0x0400018A RID: 394
			public const string PossessionToken = ".o";
		}

		// Token: 0x0200004A RID: 74
		private struct DictionaryWord
		{
			// Token: 0x060002B3 RID: 691 RVA: 0x00019E3A File Offset: 0x0001803A
			public DictionaryWord(string nominative, string nominativePlural, string genitive, string genitivePlural, string dative, string dativePlural, string accusative, string accusativePlural)
			{
				this.Nominative = nominative;
				this.NominativePlural = nominativePlural;
				this.Accusative = accusative;
				this.Genitive = genitive;
				this.Dative = dative;
				this.AccusativePlural = accusativePlural;
				this.GenitivePlural = genitivePlural;
				this.DativePlural = dativePlural;
			}

			// Token: 0x0400018B RID: 395
			public readonly string Nominative;

			// Token: 0x0400018C RID: 396
			public readonly string NominativePlural;

			// Token: 0x0400018D RID: 397
			public readonly string Accusative;

			// Token: 0x0400018E RID: 398
			public readonly string Genitive;

			// Token: 0x0400018F RID: 399
			public readonly string Dative;

			// Token: 0x04000190 RID: 400
			public readonly string AccusativePlural;

			// Token: 0x04000191 RID: 401
			public readonly string GenitivePlural;

			// Token: 0x04000192 RID: 402
			public readonly string DativePlural;
		}
	}
}
