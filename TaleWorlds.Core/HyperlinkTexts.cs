using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x0200007B RID: 123
	public static class HyperlinkTexts
	{
		// Token: 0x0600084C RID: 2124 RVA: 0x0001B92B File Offset: 0x00019B2B
		public static TextObject GetSettlementHyperlinkText(string link, TextObject settlementName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Settlement\" href=\"event:{LINK}\"><b>{SETTLEMENT_NAME}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("SETTLEMENT_NAME", settlementName);
			return textObject;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001B952 File Offset: 0x00019B52
		public static TextObject GetKingdomHyperlinkText(string link, TextObject kingdomName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Kingdom\" href=\"event:{LINK}\"><b>{KINGDOM_NAME}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("KINGDOM_NAME", kingdomName);
			return textObject;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001B979 File Offset: 0x00019B79
		public static TextObject GetHeroHyperlinkText(string link, TextObject heroName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Hero\" href=\"event:{LINK}\"><b>{HERO_NAME}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("HERO_NAME", heroName);
			return textObject;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001B9A0 File Offset: 0x00019BA0
		public static TextObject GetConceptHyperlinkText(string link, TextObject conceptName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Concept\" href=\"event:{LINK}\"><b>{CONCEPT_NAME}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("CONCEPT_NAME", conceptName);
			return textObject;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001B9C7 File Offset: 0x00019BC7
		public static TextObject GetClanHyperlinkText(string link, TextObject clanName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Clan\" href=\"event:{LINK}\"><b>{CLAN_NAME}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("CLAN_NAME", clanName);
			return textObject;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001B9EE File Offset: 0x00019BEE
		public static TextObject GetShipHyperlinkText(string link, TextObject shipHullName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Ship\" href=\"event:{LINK}\"><b>{SHIP}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("SHIP", shipHullName);
			return textObject;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001BA15 File Offset: 0x00019C15
		public static TextObject GetUnitHyperlinkText(string link, TextObject unitName)
		{
			TextObject textObject = new TextObject("{=!}{.link}<a style=\"Link.Unit\" href=\"event:{LINK}\"><b>{UNIT_NAME}</b></a>", null);
			textObject.SetTextVariable("LINK", link);
			textObject.SetTextVariable("UNIT_NAME", unitName);
			return textObject;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001BA3C File Offset: 0x00019C3C
		public static string GetGenericHyperlinkText(string link, string name)
		{
			return string.Concat(new string[] { "<a style=\"Link\" href=\"event:", link, "\"><b>", name, "</b></a>" });
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001BA69 File Offset: 0x00019C69
		public static string GetGenericImageText(string meshId, int extend = 0)
		{
			return string.Format("<img src=\"{0}\" extend=\"{1}\">", meshId, extend);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001BA7C File Offset: 0x00019C7C
		public static string GetKeyHyperlinkText(string keyID, float overrideExtendScale = 1f)
		{
			string text = "None";
			int num = 16;
			HyperlinkTexts.ConsoleType consoleType = HyperlinkTexts.ConsoleType.Xbox;
			Func<bool> isPlayStationGamepadActive = HyperlinkTexts.IsPlayStationGamepadActive;
			if (isPlayStationGamepadActive != null && isPlayStationGamepadActive())
			{
				consoleType = HyperlinkTexts.ConsoleType.Ps5;
			}
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(keyID);
			if (num2 <= 2112836247U)
			{
				if (num2 <= 1044186795U)
				{
					if (num2 <= 215134355U)
					{
						if (num2 <= 130952070U)
						{
							if (num2 <= 106449248U)
							{
								if (num2 <= 97396832U)
								{
									if (num2 != 75071339U)
									{
										if (num2 != 97396832U)
										{
											goto IL_15AB;
										}
										if (!(keyID == "D3"))
										{
											goto IL_15AB;
										}
									}
									else
									{
										if (!(keyID == "LeftAlt"))
										{
											goto IL_15AB;
										}
										goto IL_1539;
									}
								}
								else if (num2 != 100894848U)
								{
									if (num2 != 106449248U)
									{
										goto IL_15AB;
									}
									if (!(keyID == "RightMouseButton"))
									{
										goto IL_15AB;
									}
									goto IL_1439;
								}
								else
								{
									if (!(keyID == "Extended"))
									{
										goto IL_15AB;
									}
									goto IL_140C;
								}
							}
							else if (num2 <= 115203180U)
							{
								if (num2 != 114174451U)
								{
									if (num2 != 115203180U)
									{
										goto IL_15AB;
									}
									if (!(keyID == "BackSpace"))
									{
										goto IL_15AB;
									}
									goto IL_141B;
								}
								else if (!(keyID == "D2"))
								{
									goto IL_15AB;
								}
							}
							else if (num2 != 117964108U)
							{
								if (num2 != 130952070U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "D1"))
								{
									goto IL_15AB;
								}
							}
							else
							{
								if (!(keyID == "NumpadMinus"))
								{
									goto IL_15AB;
								}
								goto IL_14F9;
							}
						}
						else if (num2 <= 181284927U)
						{
							if (num2 <= 147729689U)
							{
								if (num2 != 139650141U)
								{
									if (num2 != 147729689U)
									{
										goto IL_15AB;
									}
									if (!(keyID == "D0"))
									{
										goto IL_15AB;
									}
								}
								else
								{
									if (!(keyID == "OpenBraces"))
									{
										goto IL_15AB;
									}
									goto IL_140C;
								}
							}
							else if (num2 != 164507308U)
							{
								if (num2 != 181284927U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "D6"))
								{
									goto IL_15AB;
								}
							}
							else if (!(keyID == "D7"))
							{
								goto IL_15AB;
							}
						}
						else if (num2 <= 198356736U)
						{
							if (num2 != 198062546U)
							{
								if (num2 != 198356736U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "F9"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
							else if (!(keyID == "D5"))
							{
								goto IL_15AB;
							}
						}
						else if (num2 != 214840165U)
						{
							if (num2 != 215134355U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "F8"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else if (!(keyID == "D4"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 <= 382910545U)
					{
						if (num2 <= 302657454U)
						{
							if (num2 <= 265173022U)
							{
								if (num2 != 254900552U)
								{
									if (num2 != 265173022U)
									{
										goto IL_15AB;
									}
									if (!(keyID == "D9"))
									{
										goto IL_15AB;
									}
								}
								else
								{
									if (!(keyID == "Insert"))
									{
										goto IL_15AB;
									}
									goto IL_140C;
								}
							}
							else if (num2 != 281950641U)
							{
								if (num2 != 302657454U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "ControllerRStickUp"))
								{
									goto IL_15AB;
								}
								goto IL_157A;
							}
							else if (!(keyID == "D8"))
							{
								goto IL_15AB;
							}
						}
						else if (num2 <= 354454743U)
						{
							if (num2 != 332577688U)
							{
								if (num2 != 354454743U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "ControllerRThumb"))
								{
									goto IL_15AB;
								}
								num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 12 : 10);
								text = "controllerrthumb";
								goto IL_15AB;
							}
							else
							{
								if (!(keyID == "F1"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
						}
						else if (num2 != 366132926U)
						{
							if (num2 != 382910545U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "F2"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else
						{
							if (!(keyID == "F3"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
					}
					else if (num2 <= 433243402U)
					{
						if (num2 <= 399688164U)
						{
							if (num2 != 389828744U)
							{
								if (num2 != 399688164U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "F5"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
							else
							{
								if (!(keyID == "MouseScrollUp"))
								{
									goto IL_15AB;
								}
								goto IL_1439;
							}
						}
						else if (num2 != 416465783U)
						{
							if (num2 != 433243402U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "F7"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else
						{
							if (!(keyID == "F4"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
					}
					else if (num2 <= 513712005U)
					{
						if (num2 != 450021021U)
						{
							if (num2 != 513712005U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "Right"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else
						{
							if (!(keyID == "F6"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
					}
					else if (num2 != 575450500U)
					{
						if (num2 != 1039550435U)
						{
							if (num2 != 1044186795U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "PageUp"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else
						{
							if (!(keyID == "ControllerLDown"))
							{
								goto IL_15AB;
							}
							goto IL_1448;
						}
					}
					else
					{
						if (!(keyID == "Apostrophe"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
				}
				else if (num2 <= 1706424088U)
				{
					if (num2 <= 1296647161U)
					{
						if (num2 <= 1123244352U)
						{
							if (num2 <= 1081442551U)
							{
								if (num2 != 1050238388U)
								{
									if (num2 != 1081442551U)
									{
										goto IL_15AB;
									}
									if (!(keyID == "CloseBraces"))
									{
										goto IL_15AB;
									}
									goto IL_140C;
								}
								else
								{
									if (!(keyID == "Equals"))
									{
										goto IL_15AB;
									}
									goto IL_140C;
								}
							}
							else if (num2 != 1107541039U)
							{
								if (num2 != 1123244352U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "Up"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
							else if (!(keyID == "ControllerRTrigger"))
							{
								goto IL_15AB;
							}
						}
						else if (num2 <= 1174120482U)
						{
							if (num2 != 1138704245U)
							{
								if (num2 != 1174120482U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "ControllerLUp"))
								{
									goto IL_15AB;
								}
								goto IL_1448;
							}
							else
							{
								if (!(keyID == "X1MouseButton"))
								{
									goto IL_15AB;
								}
								goto IL_1439;
							}
						}
						else if (num2 != 1231278590U)
						{
							if (num2 != 1296647161U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "ControllerLTrigger"))
							{
								goto IL_15AB;
							}
						}
						else
						{
							if (!(keyID == "RightControl"))
							{
								goto IL_15AB;
							}
							goto IL_152E;
						}
						num = 16;
						text = keyID.ToLower();
						goto IL_15AB;
					}
					if (num2 <= 1529719870U)
					{
						if (num2 <= 1428210068U)
						{
							if (num2 != 1391791790U)
							{
								if (num2 != 1428210068U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "LeftShift"))
								{
									goto IL_15AB;
								}
								goto IL_1523;
							}
							else
							{
								if (!(keyID == "Home"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
						}
						else if (num2 != 1469573738U)
						{
							if (num2 != 1529719870U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "ControllerLLeft"))
							{
								goto IL_15AB;
							}
							goto IL_1448;
						}
						else
						{
							if (!(keyID == "Delete"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
					}
					else if (num2 <= 1650792303U)
					{
						if (num2 != 1537849368U)
						{
							if (num2 != 1650792303U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "ControllerRStickRight"))
							{
								goto IL_15AB;
							}
							goto IL_157A;
						}
						else
						{
							if (!(keyID == "ControllerROption"))
							{
								goto IL_15AB;
							}
							num = 16;
							text = ((consoleType == HyperlinkTexts.ConsoleType.Ps4) ? (keyID.ToLower() + "_4") : keyID.ToLower());
							goto IL_15AB;
						}
					}
					else if (num2 != 1702612722U)
					{
						if (num2 != 1706424088U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "Comma"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
					else
					{
						if (!(keyID == "ControllerRBumper"))
						{
							goto IL_15AB;
						}
						goto IL_146D;
					}
				}
				else if (num2 <= 1898928778U)
				{
					if (num2 <= 1859932547U)
					{
						if (num2 <= 1843154928U)
						{
							if (num2 != 1806183147U)
							{
								if (num2 != 1843154928U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "Numpad4"))
								{
									goto IL_15AB;
								}
							}
							else
							{
								if (!(keyID == "MiddleMouseButton"))
								{
									goto IL_15AB;
								}
								goto IL_1439;
							}
						}
						else if (num2 != 1852896292U)
						{
							if (num2 != 1859932547U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "Numpad5"))
							{
								goto IL_15AB;
							}
						}
						else
						{
							if (!(keyID == "ControllerRLeft"))
							{
								goto IL_15AB;
							}
							goto IL_145E;
						}
					}
					else if (num2 <= 1876710166U)
					{
						if (num2 != 1868010299U)
						{
							if (num2 != 1876710166U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "Numpad6"))
							{
								goto IL_15AB;
							}
						}
						else
						{
							if (!(keyID == "ControllerRStick"))
							{
								goto IL_15AB;
							}
							goto IL_157A;
						}
					}
					else if (num2 != 1893487785U)
					{
						if (num2 != 1898928778U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "Slash"))
						{
							goto IL_15AB;
						}
						goto IL_158C;
					}
					else if (!(keyID == "Numpad7"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 <= 1960598261U)
				{
					if (num2 <= 1927043023U)
					{
						if (num2 != 1910265404U)
						{
							if (num2 != 1927043023U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "Numpad1"))
							{
								goto IL_15AB;
							}
						}
						else if (!(keyID == "Numpad0"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 != 1943820642U)
					{
						if (num2 != 1960598261U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "Numpad3"))
						{
							goto IL_15AB;
						}
					}
					else if (!(keyID == "Numpad2"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 <= 2044486356U)
				{
					if (num2 != 2008406340U)
					{
						if (num2 != 2044486356U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "Numpad8"))
						{
							goto IL_15AB;
						}
					}
					else
					{
						if (!(keyID == "ControllerLStickUp"))
						{
							goto IL_15AB;
						}
						goto IL_1556;
					}
				}
				else if (num2 != 2061263975U)
				{
					if (num2 != 2083773698U)
					{
						if (num2 != 2112836247U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "ControllerRStickDown"))
						{
							goto IL_15AB;
						}
						goto IL_157A;
					}
					else
					{
						if (!(keyID == "ControllerRStickLeft"))
						{
							goto IL_15AB;
						}
						goto IL_157A;
					}
				}
				else if (!(keyID == "Numpad9"))
				{
					goto IL_15AB;
				}
				num = 24;
				text = keyID.Substring(keyID.Length - 1);
				goto IL_15AB;
				IL_157A:
				num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 12 : 10);
				text = "controllerrstick";
				goto IL_15AB;
			}
			if (num2 <= 3373006507U)
			{
				if (num2 <= 2952291245U)
				{
					if (num2 <= 2595691489U)
					{
						if (num2 <= 2340347977U)
						{
							if (num2 <= 2157724748U)
							{
								if (num2 != 2144691513U)
								{
									if (num2 != 2157724748U)
									{
										goto IL_15AB;
									}
									if (!(keyID == "ControllerLStickLeft"))
									{
										goto IL_15AB;
									}
									goto IL_1556;
								}
								else
								{
									if (!(keyID == "ControllerRDown"))
									{
										goto IL_15AB;
									}
									goto IL_145E;
								}
							}
							else if (num2 != 2267317284U)
							{
								if (num2 != 2340347977U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "Tilde"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
							else
							{
								if (!(keyID == "Period"))
								{
									goto IL_15AB;
								}
								goto IL_1597;
							}
						}
						else if (num2 <= 2434225852U)
						{
							if (num2 != 2365054562U)
							{
								if (num2 != 2434225852U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "RightAlt"))
								{
									goto IL_15AB;
								}
								goto IL_1539;
							}
							else if (!(keyID == "NumpadEnter"))
							{
								goto IL_15AB;
							}
						}
						else if (num2 != 2457286800U)
						{
							if (num2 != 2595691489U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "ControllerLStick"))
							{
								goto IL_15AB;
							}
							goto IL_1556;
						}
						else
						{
							if (!(keyID == "Left"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
					}
					else if (num2 <= 2762355378U)
					{
						if (num2 <= 2746130317U)
						{
							if (num2 != 2728445041U)
							{
								if (num2 != 2746130317U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "ControllerLThumb"))
								{
									goto IL_15AB;
								}
								num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 12 : 10);
								text = "controllerlthumb";
								goto IL_15AB;
							}
							else
							{
								if (!(keyID == "ControllerLStickDown"))
								{
									goto IL_15AB;
								}
								goto IL_1556;
							}
						}
						else if (num2 != 2761510965U)
						{
							if (num2 != 2762355378U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "X2MouseButton"))
							{
								goto IL_15AB;
							}
							goto IL_1439;
						}
						else
						{
							if (!(keyID == "Down"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
					}
					else if (num2 <= 2906557000U)
					{
						if (num2 != 2769091631U)
						{
							if (num2 != 2906557000U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "ControllerLBumper"))
							{
								goto IL_15AB;
							}
							goto IL_146D;
						}
						else
						{
							if (!(keyID == "CapsLock"))
							{
								goto IL_15AB;
							}
							goto IL_141B;
						}
					}
					else if (num2 != 2913305049U)
					{
						if (num2 != 2952291245U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "Enter"))
						{
							goto IL_15AB;
						}
					}
					else
					{
						if (!(keyID == "ControllerLStickRight"))
						{
							goto IL_15AB;
						}
						goto IL_1556;
					}
					num = 12;
					text = "enter";
					goto IL_15AB;
				}
				if (num2 <= 3241480638U)
				{
					if (num2 <= 3082514982U)
					{
						if (num2 <= 3001337907U)
						{
							if (num2 != 2979892988U)
							{
								if (num2 != 3001337907U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "LeftMouseButton"))
								{
									goto IL_15AB;
								}
								goto IL_1439;
							}
							else
							{
								if (!(keyID == "NumLock"))
								{
									goto IL_15AB;
								}
								goto IL_140C;
							}
						}
						else if (num2 != 3036628469U)
						{
							if (num2 != 3082514982U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "Escape"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else
						{
							if (!(keyID == "LeftControl"))
							{
								goto IL_15AB;
							}
							goto IL_152E;
						}
					}
					else if (num2 <= 3222007936U)
					{
						if (num2 != 3093862813U)
						{
							if (num2 != 3222007936U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "E"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else if (!(keyID == "NumpadPeriod"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 != 3238785555U)
					{
						if (num2 != 3241480638U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "PageDown"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
					else
					{
						if (!(keyID == "D"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
				}
				else if (num2 <= 3289118412U)
				{
					if (num2 <= 3255563174U)
					{
						if (num2 != 3250860581U)
						{
							if (num2 != 3255563174U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "G"))
							{
								goto IL_15AB;
							}
							goto IL_140C;
						}
						else
						{
							if (!(keyID == "Space"))
							{
								goto IL_15AB;
							}
							goto IL_141B;
						}
					}
					else if (num2 != 3272340793U)
					{
						if (num2 != 3289118412U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "A"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
					else
					{
						if (!(keyID == "F"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
				}
				else if (num2 <= 3322673650U)
				{
					if (num2 != 3294917732U)
					{
						if (num2 != 3322673650U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "C"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
					else
					{
						if (!(keyID == "NumpadPlus"))
						{
							goto IL_15AB;
						}
						num = 24;
						text = "+";
						goto IL_15AB;
					}
				}
				else if (num2 != 3339451269U)
				{
					if (num2 != 3356228888U)
					{
						if (num2 != 3373006507U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "L"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
					else
					{
						if (!(keyID == "M"))
						{
							goto IL_15AB;
						}
						goto IL_140C;
					}
				}
				else
				{
					if (!(keyID == "B"))
					{
						goto IL_15AB;
					}
					goto IL_140C;
				}
				IL_1597:
				num = 24;
				text = "period";
				goto IL_15AB;
			}
			if (num2 <= 3574337935U)
			{
				if (num2 <= 3473672221U)
				{
					if (num2 <= 3406561745U)
					{
						if (num2 <= 3388411298U)
						{
							if (num2 != 3388260431U)
							{
								if (num2 != 3388411298U)
								{
									goto IL_15AB;
								}
								if (!(keyID == "ControllerLOption"))
								{
									goto IL_15AB;
								}
								num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 14 : 8);
								text = ((consoleType == HyperlinkTexts.ConsoleType.Ps4) ? (keyID.ToLower() + "_4") : keyID.ToLower());
								goto IL_15AB;
							}
							else
							{
								if (!(keyID == "Minus"))
								{
									goto IL_15AB;
								}
								goto IL_14F9;
							}
						}
						else if (num2 != 3389784126U)
						{
							if (num2 != 3406561745U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "N"))
							{
								goto IL_15AB;
							}
						}
						else if (!(keyID == "O"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 <= 3440116983U)
					{
						if (num2 != 3423339364U)
						{
							if (num2 != 3440116983U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "H"))
							{
								goto IL_15AB;
							}
						}
						else if (!(keyID == "I"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 != 3456894602U)
					{
						if (num2 != 3473672221U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "J"))
						{
							goto IL_15AB;
						}
					}
					else if (!(keyID == "K"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 <= 3507227459U)
				{
					if (num2 <= 3485937324U)
					{
						if (num2 != 3482547786U)
						{
							if (num2 != 3485937324U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "ControllerRUp"))
							{
								goto IL_15AB;
							}
							goto IL_145E;
						}
						else if (!(keyID == "End"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 != 3490449840U)
					{
						if (num2 != 3507227459U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "T"))
						{
							goto IL_15AB;
						}
					}
					else if (!(keyID == "U"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 <= 3540782697U)
				{
					if (num2 != 3524005078U)
					{
						if (num2 != 3540782697U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "V"))
						{
							goto IL_15AB;
						}
					}
					else if (!(keyID == "W"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 != 3557560316U)
				{
					if (num2 != 3574337935U)
					{
						goto IL_15AB;
					}
					if (!(keyID == "P"))
					{
						goto IL_15AB;
					}
				}
				else if (!(keyID == "Q"))
				{
					goto IL_15AB;
				}
			}
			else if (num2 <= 3736956062U)
			{
				if (num2 <= 3691781268U)
				{
					if (num2 <= 3592460967U)
					{
						if (num2 != 3591115554U)
						{
							if (num2 != 3592460967U)
							{
								goto IL_15AB;
							}
							if (!(keyID == "RightShift"))
							{
								goto IL_15AB;
							}
							goto IL_1523;
						}
						else if (!(keyID == "S"))
						{
							goto IL_15AB;
						}
					}
					else if (num2 != 3607893173U)
					{
						if (num2 != 3691781268U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "Y"))
						{
							goto IL_15AB;
						}
					}
					else if (!(keyID == "R"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 <= 3708558887U)
				{
					if (num2 != 3703400824U)
					{
						if (num2 != 3708558887U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "X"))
						{
							goto IL_15AB;
						}
					}
					else if (!(keyID == "F10"))
					{
						goto IL_15AB;
					}
				}
				else if (num2 != 3720178443U)
				{
					if (num2 != 3736956062U)
					{
						goto IL_15AB;
					}
					if (!(keyID == "F12"))
					{
						goto IL_15AB;
					}
				}
				else if (!(keyID == "F11"))
				{
					goto IL_15AB;
				}
			}
			else if (num2 <= 3821858654U)
			{
				if (num2 <= 3737220883U)
				{
					if (num2 != 3737177789U)
					{
						if (num2 != 3737220883U)
						{
							goto IL_15AB;
						}
						if (!(keyID == "ControllerLRight"))
						{
							goto IL_15AB;
						}
						goto IL_1448;
					}
					else
					{
						if (!(keyID == "MouseScrollDown"))
						{
							goto IL_15AB;
						}
						goto IL_1439;
					}
				}
				else if (num2 != 3742114125U)
				{
					if (num2 != 3821858654U)
					{
						goto IL_15AB;
					}
					if (!(keyID == "SemiColon"))
					{
						goto IL_15AB;
					}
				}
				else if (!(keyID == "Z"))
				{
					goto IL_15AB;
				}
			}
			else if (num2 <= 3890594748U)
			{
				if (num2 != 3862950033U)
				{
					if (num2 != 3890594748U)
					{
						goto IL_15AB;
					}
					if (!(keyID == "NumpadMultiply"))
					{
						goto IL_15AB;
					}
					num = 24;
					text = "multiply";
					goto IL_15AB;
				}
				else
				{
					if (!(keyID == "ControllerRRight"))
					{
						goto IL_15AB;
					}
					goto IL_145E;
				}
			}
			else if (num2 != 4080261303U)
			{
				if (num2 != 4149056477U)
				{
					if (num2 != 4219689196U)
					{
						goto IL_15AB;
					}
					if (!(keyID == "Tab"))
					{
						goto IL_15AB;
					}
					num = 12;
					text = keyID.ToLower();
					goto IL_15AB;
				}
				else
				{
					if (!(keyID == "NumpadSlash"))
					{
						goto IL_15AB;
					}
					goto IL_158C;
				}
			}
			else if (!(keyID == "BackSlash"))
			{
				goto IL_15AB;
			}
			IL_140C:
			num = 24;
			text = keyID.ToLower();
			goto IL_15AB;
			IL_141B:
			num = 10;
			text = keyID.ToLower();
			goto IL_15AB;
			IL_1439:
			num = 16;
			text = keyID.ToLower();
			goto IL_15AB;
			IL_1448:
			num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 16 : 10);
			text = keyID.ToLower();
			goto IL_15AB;
			IL_145E:
			num = 14;
			text = keyID.ToLower();
			goto IL_15AB;
			IL_146D:
			num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 14 : 20);
			text = keyID.ToLower();
			goto IL_15AB;
			IL_14F9:
			num = 24;
			text = "-";
			goto IL_15AB;
			IL_1523:
			num = 14;
			text = "shift";
			goto IL_15AB;
			IL_152E:
			num = 12;
			text = "control";
			goto IL_15AB;
			IL_1539:
			num = 24;
			text = "alt";
			goto IL_15AB;
			IL_1556:
			num = ((consoleType == HyperlinkTexts.ConsoleType.Xbox) ? 12 : 10);
			text = "controllerlstick";
			goto IL_15AB;
			IL_158C:
			num = 24;
			text = "slash";
			IL_15AB:
			if (consoleType == HyperlinkTexts.ConsoleType.Ps4 || consoleType == HyperlinkTexts.ConsoleType.Ps5)
			{
				text += "_ps";
			}
			num = (int)((float)num * overrideExtendScale);
			return string.Format("<img src=\"General\\InputKeys\\{0}\" extend=\"{1}\">", text, num);
		}

		// Token: 0x0400042D RID: 1069
		public const string GoldIcon = "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">";

		// Token: 0x0400042E RID: 1070
		public const string MoraleIcon = "{=!}<img src=\"General\\Icons\\Morale@2x\" extend=\"4\">";

		// Token: 0x0400042F RID: 1071
		public const string InfluenceIcon = "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">";

		// Token: 0x04000430 RID: 1072
		public const string IssueAvailableIcon = "{=!}<img src=\"General\\Icons\\icon_issue_available_square\" extend=\"4\">";

		// Token: 0x04000431 RID: 1073
		public const string IssueActiveIcon = "{=!}<img src=\"General\\Icons\\icon_issue_active_square\" extend=\"4\">";

		// Token: 0x04000432 RID: 1074
		public const string TrackedIssueIcon = "{=!}<img src=\"General\\Icons\\issue_target_icon\" extend=\"4\">";

		// Token: 0x04000433 RID: 1075
		public const string QuestAvailableIcon = "{=!}<img src=\"General\\Icons\\icon_quest_available\" extend=\"4\">";

		// Token: 0x04000434 RID: 1076
		public const string QuestActiveIcon = "{=!}<img src=\"General\\Icons\\icon_issue_active_square\" extend=\"4\">";

		// Token: 0x04000435 RID: 1077
		public const string StoryQuestActiveIcon = "{=!}<img src=\"General\\Icons\\icon_story_quest_active_square\" extend=\"4\">";

		// Token: 0x04000436 RID: 1078
		public const string TrackedStoryQuestIcon = "{=!}<img src=\"General\\Icons\\quest_target_icon\" extend=\"4\">";

		// Token: 0x04000437 RID: 1079
		public const string InPrisonIcon = "{=!}<img src=\"SPGeneral\\Clan\\Status\\icon_inprison\" extend=\"4\">";

		// Token: 0x04000438 RID: 1080
		public const string ChildIcon = "{=!}<img src=\"SPGeneral\\Clan\\Status\\icon_ischild\" extend=\"4\">";

		// Token: 0x04000439 RID: 1081
		public const string PregnantIcon = "{=!}<img src=\"SPGeneral\\Clan\\Status\\icon_pregnant\" extend=\"4\">";

		// Token: 0x0400043A RID: 1082
		public const string IllIcon = "{=!}<img src=\"SPGeneral\\Clan\\Status\\icon_terminallyill\" extend=\"4\">";

		// Token: 0x0400043B RID: 1083
		public const string HeirIcon = "{=!}<img src=\"SPGeneral\\Clan\\Status\\icon_heir\" extend=\"4\">";

		// Token: 0x0400043C RID: 1084
		public const string UnreadIcon = "{=!}<img src=\"MapMenuUnread@2x\" extend=\"4\">";

		// Token: 0x0400043D RID: 1085
		public const string UnselectedPerkIcon = "{=!}<img src=\"CharacterDeveloper\\UnselectedPerksIcon\" extend=\"2\">";

		// Token: 0x0400043E RID: 1086
		public const string HorseIcon = "{=!}<img src=\"StdAssets\\ItemIcons\\Mount\" extend=\"14\">";

		// Token: 0x0400043F RID: 1087
		public const string CrimeIcon = "{=!}<img src=\"SPGeneral\\MapOverlay\\Settlement\\icon_crime\" extend=\"12\">";

		// Token: 0x04000440 RID: 1088
		public const string UpgradeAvailableIcon = "{=!}<img src=\"PartyScreen\\upgrade_icon\" extend=\"5\">";

		// Token: 0x04000441 RID: 1089
		public const string FocusIcon = "{=!}<img src=\"CharacterDeveloper\\cp_icon\">";

		// Token: 0x04000442 RID: 1090
		public const string BloodFeudIcon = "{=!}<img src=\"SPGeneral\\blood_feud_icon\" extend=\"3\">";

		// Token: 0x04000443 RID: 1091
		public static Func<bool> IsPlayStationGamepadActive;

		// Token: 0x0200011E RID: 286
		private enum ConsoleType
		{
			// Token: 0x040007B7 RID: 1975
			Xbox,
			// Token: 0x040007B8 RID: 1976
			Ps4,
			// Token: 0x040007B9 RID: 1977
			Ps5
		}
	}
}
