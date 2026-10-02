using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000E RID: 14
	public class InputKeyVisualWidget : Widget
	{
		// Token: 0x060000CE RID: 206 RVA: 0x000040F9 File Offset: 0x000022F9
		public InputKeyVisualWidget(UIContext context)
			: base(context)
		{
			base.DoNotAcceptEvents = true;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004120 File Offset: 0x00002320
		private string GetKeyVisualName(string keyID)
		{
			Input.ControllerTypes controllerType = Input.ControllerType;
			string text = "None";
			uint num = <PrivateImplementationDetails>.ComputeStringHash(keyID);
			if (num <= 2144691513U)
			{
				if (num <= 1107541039U)
				{
					if (num <= 265173022U)
					{
						if (num <= 139650141U)
						{
							if (num <= 106449248U)
							{
								if (num <= 97396832U)
								{
									if (num != 75071339U)
									{
										if (num != 97396832U)
										{
											return text;
										}
										if (!(keyID == "D3"))
										{
											return text;
										}
										goto IL_15EB;
									}
									else
									{
										if (!(keyID == "LeftAlt"))
										{
											return text;
										}
										goto IL_1660;
									}
								}
								else if (num != 100894848U)
								{
									if (num != 106449248U)
									{
										return text;
									}
									if (!(keyID == "RightMouseButton"))
									{
										return text;
									}
									goto IL_1598;
								}
								else
								{
									if (!(keyID == "Extended"))
									{
										return text;
									}
									goto IL_1598;
								}
							}
							else if (num <= 115203180U)
							{
								if (num != 114174451U)
								{
									if (num != 115203180U)
									{
										return text;
									}
									if (!(keyID == "BackSpace"))
									{
										return text;
									}
									goto IL_1598;
								}
								else
								{
									if (!(keyID == "D2"))
									{
										return text;
									}
									goto IL_15E0;
								}
							}
							else if (num != 117964108U)
							{
								if (num != 130952070U)
								{
									if (num != 139650141U)
									{
										return text;
									}
									if (!(keyID == "OpenBraces"))
									{
										return text;
									}
									goto IL_1598;
								}
								else
								{
									if (!(keyID == "D1"))
									{
										return text;
									}
									goto IL_15D5;
								}
							}
							else
							{
								if (!(keyID == "NumpadMinus"))
								{
									return text;
								}
								goto IL_1638;
							}
						}
						else if (num <= 198062546U)
						{
							if (num <= 164507308U)
							{
								if (num != 147729689U)
								{
									if (num != 164507308U)
									{
										return text;
									}
									if (!(keyID == "D7"))
									{
										return text;
									}
									goto IL_1617;
								}
								else if (!(keyID == "D0"))
								{
									return text;
								}
							}
							else if (num != 181284927U)
							{
								if (num != 198062546U)
								{
									return text;
								}
								if (!(keyID == "D5"))
								{
									return text;
								}
								goto IL_1601;
							}
							else
							{
								if (!(keyID == "D6"))
								{
									return text;
								}
								goto IL_160C;
							}
						}
						else if (num <= 214840165U)
						{
							if (num != 198356736U)
							{
								if (num != 214840165U)
								{
									return text;
								}
								if (!(keyID == "D4"))
								{
									return text;
								}
								goto IL_15F6;
							}
							else
							{
								if (!(keyID == "F9"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 215134355U)
						{
							if (num != 254900552U)
							{
								if (num != 265173022U)
								{
									return text;
								}
								if (!(keyID == "D9"))
								{
									return text;
								}
								goto IL_162D;
							}
							else
							{
								if (!(keyID == "Insert"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else
						{
							if (!(keyID == "F8"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 416465783U)
					{
						if (num <= 354454743U)
						{
							if (num <= 302657454U)
							{
								if (num != 281950641U)
								{
									if (num != 302657454U)
									{
										return text;
									}
									if (!(keyID == "ControllerRStickUp"))
									{
										return text;
									}
									goto IL_1680;
								}
								else
								{
									if (!(keyID == "D8"))
									{
										return text;
									}
									goto IL_1622;
								}
							}
							else if (num != 332577688U)
							{
								if (num != 354454743U)
								{
									return text;
								}
								if (!(keyID == "ControllerRThumb"))
								{
									return text;
								}
								return "controllerrthumb";
							}
							else
							{
								if (!(keyID == "F1"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num <= 382910545U)
						{
							if (num != 366132926U)
							{
								if (num != 382910545U)
								{
									return text;
								}
								if (!(keyID == "F2"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "F3"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 389828744U)
						{
							if (num != 399688164U)
							{
								if (num != 416465783U)
								{
									return text;
								}
								if (!(keyID == "F4"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "F5"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else
						{
							if (!(keyID == "MouseScrollUp"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 575450500U)
					{
						if (num <= 450021021U)
						{
							if (num != 433243402U)
							{
								if (num != 450021021U)
								{
									return text;
								}
								if (!(keyID == "F6"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "F7"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 513712005U)
						{
							if (num != 575450500U)
							{
								return text;
							}
							if (!(keyID == "Apostrophe"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "Right"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 1044186795U)
					{
						if (num != 1039550435U)
						{
							if (num != 1044186795U)
							{
								return text;
							}
							if (!(keyID == "PageUp"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "ControllerLDown"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num != 1050238388U)
					{
						if (num != 1081442551U)
						{
							if (num != 1107541039U)
							{
								return text;
							}
							if (!(keyID == "ControllerRTrigger"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "CloseBraces"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else
					{
						if (!(keyID == "Equals"))
						{
							return text;
						}
						goto IL_1598;
					}
				}
				else if (num <= 1706424088U)
				{
					if (num <= 1355078617U)
					{
						if (num <= 1231278590U)
						{
							if (num <= 1138704245U)
							{
								if (num != 1123244352U)
								{
									if (num != 1138704245U)
									{
										return text;
									}
									if (!(keyID == "X1MouseButton"))
									{
										return text;
									}
									goto IL_1598;
								}
								else
								{
									if (!(keyID == "Up"))
									{
										return text;
									}
									goto IL_1598;
								}
							}
							else if (num != 1174120482U)
							{
								if (num != 1231278590U)
								{
									return text;
								}
								if (!(keyID == "RightControl"))
								{
									return text;
								}
								goto IL_1658;
							}
							else
							{
								if (!(keyID == "ControllerLUp"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num <= 1304745760U)
						{
							if (num != 1296647161U)
							{
								if (num != 1304745760U)
								{
									return text;
								}
								if (!(keyID == "F21"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "ControllerLTrigger"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 1321523379U)
						{
							if (num != 1338300998U)
							{
								if (num != 1355078617U)
								{
									return text;
								}
								if (!(keyID == "F22"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "F23"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else
						{
							if (!(keyID == "F20"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 1469573738U)
					{
						if (num <= 1391791790U)
						{
							if (num != 1388633855U)
							{
								if (num != 1391791790U)
								{
									return text;
								}
								if (!(keyID == "Home"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "F24"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 1428210068U)
						{
							if (num != 1469573738U)
							{
								return text;
							}
							if (!(keyID == "Delete"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "LeftShift"))
							{
								return text;
							}
							goto IL_1650;
						}
					}
					else if (num <= 1537849368U)
					{
						if (num != 1529719870U)
						{
							if (num != 1537849368U)
							{
								return text;
							}
							if (!(keyID == "ControllerROption"))
							{
								return text;
							}
							goto IL_15A4;
						}
						else
						{
							if (!(keyID == "ControllerLLeft"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num != 1650792303U)
					{
						if (num != 1702612722U)
						{
							if (num != 1706424088U)
							{
								return text;
							}
							if (!(keyID == "Comma"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "ControllerRBumper"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else
					{
						if (!(keyID == "ControllerRStickRight"))
						{
							return text;
						}
						goto IL_1680;
					}
				}
				else if (num <= 1910265404U)
				{
					if (num <= 1859932547U)
					{
						if (num <= 1843154928U)
						{
							if (num != 1806183147U)
							{
								if (num != 1843154928U)
								{
									return text;
								}
								if (!(keyID == "Numpad4"))
								{
									return text;
								}
								goto IL_15F6;
							}
							else
							{
								if (!(keyID == "MiddleMouseButton"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 1852896292U)
						{
							if (num != 1859932547U)
							{
								return text;
							}
							if (!(keyID == "Numpad5"))
							{
								return text;
							}
							goto IL_1601;
						}
						else
						{
							if (!(keyID == "ControllerRLeft"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 1876710166U)
					{
						if (num != 1868010299U)
						{
							if (num != 1876710166U)
							{
								return text;
							}
							if (!(keyID == "Numpad6"))
							{
								return text;
							}
							goto IL_160C;
						}
						else
						{
							if (!(keyID == "ControllerRStick"))
							{
								return text;
							}
							goto IL_1680;
						}
					}
					else if (num != 1893487785U)
					{
						if (num != 1898928778U)
						{
							if (num != 1910265404U)
							{
								return text;
							}
							if (!(keyID == "Numpad0"))
							{
								return text;
							}
						}
						else
						{
							if (!(keyID == "Slash"))
							{
								return text;
							}
							goto IL_16A8;
						}
					}
					else
					{
						if (!(keyID == "Numpad7"))
						{
							return text;
						}
						goto IL_1617;
					}
				}
				else if (num <= 2008406340U)
				{
					if (num <= 1943820642U)
					{
						if (num != 1927043023U)
						{
							if (num != 1943820642U)
							{
								return text;
							}
							if (!(keyID == "Numpad2"))
							{
								return text;
							}
							goto IL_15E0;
						}
						else
						{
							if (!(keyID == "Numpad1"))
							{
								return text;
							}
							goto IL_15D5;
						}
					}
					else if (num != 1960598261U)
					{
						if (num != 2008406340U)
						{
							return text;
						}
						if (!(keyID == "ControllerLStickUp"))
						{
							return text;
						}
						goto IL_1670;
					}
					else
					{
						if (!(keyID == "Numpad3"))
						{
							return text;
						}
						goto IL_15EB;
					}
				}
				else if (num <= 2061263975U)
				{
					if (num != 2044486356U)
					{
						if (num != 2061263975U)
						{
							return text;
						}
						if (!(keyID == "Numpad9"))
						{
							return text;
						}
						goto IL_162D;
					}
					else
					{
						if (!(keyID == "Numpad8"))
						{
							return text;
						}
						goto IL_1622;
					}
				}
				else if (num != 2083773698U)
				{
					if (num != 2112836247U)
					{
						if (num != 2144691513U)
						{
							return text;
						}
						if (!(keyID == "ControllerRDown"))
						{
							return text;
						}
						goto IL_1598;
					}
					else
					{
						if (!(keyID == "ControllerRStickDown"))
						{
							return text;
						}
						goto IL_1680;
					}
				}
				else
				{
					if (!(keyID == "ControllerRStickLeft"))
					{
						return text;
					}
					goto IL_1680;
				}
				return "0";
				IL_15D5:
				return "1";
				IL_15E0:
				return "2";
				IL_15EB:
				return "3";
				IL_15F6:
				return "4";
				IL_1601:
				return "5";
				IL_160C:
				return "6";
				IL_1617:
				return "7";
				IL_1622:
				return "8";
				IL_162D:
				return "9";
				IL_1680:
				return "controllerrstick";
			}
			if (num <= 3406561745U)
			{
				if (num <= 3036628469U)
				{
					if (num <= 2746130317U)
					{
						if (num <= 2365054562U)
						{
							if (num <= 2267317284U)
							{
								if (num != 2157724748U)
								{
									if (num != 2267317284U)
									{
										return text;
									}
									if (!(keyID == "Period"))
									{
										return text;
									}
									goto IL_16B0;
								}
								else
								{
									if (!(keyID == "ControllerLStickLeft"))
									{
										return text;
									}
									goto IL_1670;
								}
							}
							else if (num != 2340347977U)
							{
								if (num != 2365054562U)
								{
									return text;
								}
								if (!(keyID == "NumpadEnter"))
								{
									return text;
								}
							}
							else
							{
								if (!(keyID == "Tilde"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num <= 2457286800U)
						{
							if (num != 2434225852U)
							{
								if (num != 2457286800U)
								{
									return text;
								}
								if (!(keyID == "Left"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "RightAlt"))
								{
									return text;
								}
								goto IL_1660;
							}
						}
						else if (num != 2595691489U)
						{
							if (num != 2728445041U)
							{
								if (num != 2746130317U)
								{
									return text;
								}
								if (!(keyID == "ControllerLThumb"))
								{
									return text;
								}
								return "controllerlthumb";
							}
							else
							{
								if (!(keyID == "ControllerLStickDown"))
								{
									return text;
								}
								goto IL_1670;
							}
						}
						else
						{
							if (!(keyID == "ControllerLStick"))
							{
								return text;
							}
							goto IL_1670;
						}
					}
					else if (num <= 2906557000U)
					{
						if (num <= 2762355378U)
						{
							if (num != 2761510965U)
							{
								if (num != 2762355378U)
								{
									return text;
								}
								if (!(keyID == "X2MouseButton"))
								{
									return text;
								}
								goto IL_1598;
							}
							else
							{
								if (!(keyID == "Down"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 2769091631U)
						{
							if (num != 2906557000U)
							{
								return text;
							}
							if (!(keyID == "ControllerLBumper"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "CapsLock"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 2952291245U)
					{
						if (num != 2913305049U)
						{
							if (num != 2952291245U)
							{
								return text;
							}
							if (!(keyID == "Enter"))
							{
								return text;
							}
						}
						else
						{
							if (!(keyID == "ControllerLStickRight"))
							{
								return text;
							}
							goto IL_1670;
						}
					}
					else if (num != 2979892988U)
					{
						if (num != 3001337907U)
						{
							if (num != 3036628469U)
							{
								return text;
							}
							if (!(keyID == "LeftControl"))
							{
								return text;
							}
							goto IL_1658;
						}
						else
						{
							if (!(keyID == "LeftMouseButton"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else
					{
						if (!(keyID == "NumLock"))
						{
							return text;
						}
						goto IL_1598;
					}
					return "enter";
				}
				if (num <= 3289118412U)
				{
					if (num <= 3238785555U)
					{
						if (num <= 3093862813U)
						{
							if (num != 3082514982U)
							{
								if (num != 3093862813U)
								{
									return text;
								}
								if (!(keyID == "NumpadPeriod"))
								{
									return text;
								}
							}
							else
							{
								if (!(keyID == "Escape"))
								{
									return text;
								}
								goto IL_1598;
							}
						}
						else if (num != 3222007936U)
						{
							if (num != 3238785555U)
							{
								return text;
							}
							if (!(keyID == "D"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "E"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num <= 3250860581U)
					{
						if (num != 3241480638U)
						{
							if (num != 3250860581U)
							{
								return text;
							}
							if (!(keyID == "Space"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "PageDown"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else if (num != 3255563174U)
					{
						if (num != 3272340793U)
						{
							if (num != 3289118412U)
							{
								return text;
							}
							if (!(keyID == "A"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "F"))
							{
								return text;
							}
							goto IL_1598;
						}
					}
					else
					{
						if (!(keyID == "G"))
						{
							return text;
						}
						goto IL_1598;
					}
				}
				else if (num <= 3356228888U)
				{
					if (num <= 3322673650U)
					{
						if (num != 3294917732U)
						{
							if (num != 3322673650U)
							{
								return text;
							}
							if (!(keyID == "C"))
							{
								return text;
							}
							goto IL_1598;
						}
						else
						{
							if (!(keyID == "NumpadPlus"))
							{
								return text;
							}
							return "+";
						}
					}
					else if (num != 3339451269U)
					{
						if (num != 3356228888U)
						{
							return text;
						}
						if (!(keyID == "M"))
						{
							return text;
						}
						goto IL_1598;
					}
					else
					{
						if (!(keyID == "B"))
						{
							return text;
						}
						goto IL_1598;
					}
				}
				else if (num <= 3388260431U)
				{
					if (num != 3373006507U)
					{
						if (num != 3388260431U)
						{
							return text;
						}
						if (!(keyID == "Minus"))
						{
							return text;
						}
						goto IL_1638;
					}
					else
					{
						if (!(keyID == "L"))
						{
							return text;
						}
						goto IL_1598;
					}
				}
				else if (num != 3388411298U)
				{
					if (num != 3389784126U)
					{
						if (num != 3406561745U)
						{
							return text;
						}
						if (!(keyID == "N"))
						{
							return text;
						}
						goto IL_1598;
					}
					else
					{
						if (!(keyID == "O"))
						{
							return text;
						}
						goto IL_1598;
					}
				}
				else
				{
					if (!(keyID == "ControllerLOption"))
					{
						return text;
					}
					goto IL_15A4;
				}
				IL_16B0:
				return "period";
			}
			if (num <= 3691781268U)
			{
				if (num <= 3524005078U)
				{
					if (num <= 3473672221U)
					{
						if (num <= 3440116983U)
						{
							if (num != 3423339364U)
							{
								if (num != 3440116983U)
								{
									return text;
								}
								if (!(keyID == "H"))
								{
									return text;
								}
							}
							else if (!(keyID == "I"))
							{
								return text;
							}
						}
						else if (num != 3456894602U)
						{
							if (num != 3473672221U)
							{
								return text;
							}
							if (!(keyID == "J"))
							{
								return text;
							}
						}
						else if (!(keyID == "K"))
						{
							return text;
						}
					}
					else if (num <= 3485937324U)
					{
						if (num != 3482547786U)
						{
							if (num != 3485937324U)
							{
								return text;
							}
							if (!(keyID == "ControllerRUp"))
							{
								return text;
							}
						}
						else if (!(keyID == "End"))
						{
							return text;
						}
					}
					else if (num != 3490449840U)
					{
						if (num != 3507227459U)
						{
							if (num != 3524005078U)
							{
								return text;
							}
							if (!(keyID == "W"))
							{
								return text;
							}
						}
						else if (!(keyID == "T"))
						{
							return text;
						}
					}
					else if (!(keyID == "U"))
					{
						return text;
					}
				}
				else if (num <= 3574337935U)
				{
					if (num <= 3557560316U)
					{
						if (num != 3540782697U)
						{
							if (num != 3557560316U)
							{
								return text;
							}
							if (!(keyID == "Q"))
							{
								return text;
							}
						}
						else if (!(keyID == "V"))
						{
							return text;
						}
					}
					else if (num != 3569179872U)
					{
						if (num != 3574337935U)
						{
							return text;
						}
						if (!(keyID == "P"))
						{
							return text;
						}
					}
					else if (!(keyID == "F18"))
					{
						return text;
					}
				}
				else if (num <= 3591115554U)
				{
					if (num != 3585957491U)
					{
						if (num != 3591115554U)
						{
							return text;
						}
						if (!(keyID == "S"))
						{
							return text;
						}
					}
					else if (!(keyID == "F19"))
					{
						return text;
					}
				}
				else if (num != 3592460967U)
				{
					if (num != 3607893173U)
					{
						if (num != 3691781268U)
						{
							return text;
						}
						if (!(keyID == "Y"))
						{
							return text;
						}
					}
					else if (!(keyID == "R"))
					{
						return text;
					}
				}
				else
				{
					if (!(keyID == "RightShift"))
					{
						return text;
					}
					goto IL_1650;
				}
			}
			else if (num <= 3770511300U)
			{
				if (num <= 3736956062U)
				{
					if (num <= 3708558887U)
					{
						if (num != 3703400824U)
						{
							if (num != 3708558887U)
							{
								return text;
							}
							if (!(keyID == "X"))
							{
								return text;
							}
						}
						else if (!(keyID == "F10"))
						{
							return text;
						}
					}
					else if (num != 3720178443U)
					{
						if (num != 3736956062U)
						{
							return text;
						}
						if (!(keyID == "F12"))
						{
							return text;
						}
					}
					else if (!(keyID == "F11"))
					{
						return text;
					}
				}
				else if (num <= 3737220883U)
				{
					if (num != 3737177789U)
					{
						if (num != 3737220883U)
						{
							return text;
						}
						if (!(keyID == "ControllerLRight"))
						{
							return text;
						}
					}
					else if (!(keyID == "MouseScrollDown"))
					{
						return text;
					}
				}
				else if (num != 3742114125U)
				{
					if (num != 3753733681U)
					{
						if (num != 3770511300U)
						{
							return text;
						}
						if (!(keyID == "F14"))
						{
							return text;
						}
					}
					else if (!(keyID == "F13"))
					{
						return text;
					}
				}
				else if (!(keyID == "Z"))
				{
					return text;
				}
			}
			else if (num <= 3862950033U)
			{
				if (num <= 3804066538U)
				{
					if (num != 3787288919U)
					{
						if (num != 3804066538U)
						{
							return text;
						}
						if (!(keyID == "F16"))
						{
							return text;
						}
					}
					else if (!(keyID == "F15"))
					{
						return text;
					}
				}
				else if (num != 3820844157U)
				{
					if (num != 3821858654U)
					{
						if (num != 3862950033U)
						{
							return text;
						}
						if (!(keyID == "ControllerRRight"))
						{
							return text;
						}
					}
					else if (!(keyID == "SemiColon"))
					{
						return text;
					}
				}
				else if (!(keyID == "F17"))
				{
					return text;
				}
			}
			else if (num <= 3958280132U)
			{
				if (num != 3890594748U)
				{
					if (num != 3958280132U)
					{
						return text;
					}
					if (!(keyID == "ControllerShare"))
					{
						return text;
					}
					if (controllerType == Input.ControllerTypes.PlayStationDualShock)
					{
						return keyID.ToLower() + "_4";
					}
					return keyID.ToLower();
				}
				else
				{
					if (!(keyID == "NumpadMultiply"))
					{
						return text;
					}
					return "multiply";
				}
			}
			else if (num != 4080261303U)
			{
				if (num != 4149056477U)
				{
					if (num != 4219689196U)
					{
						return text;
					}
					if (!(keyID == "Tab"))
					{
						return text;
					}
				}
				else
				{
					if (!(keyID == "NumpadSlash"))
					{
						return text;
					}
					goto IL_16A8;
				}
			}
			else if (!(keyID == "BackSlash"))
			{
				return text;
			}
			IL_1598:
			return keyID.ToLower();
			IL_15A4:
			if (controllerType == Input.ControllerTypes.PlayStationDualShock)
			{
				return keyID.ToLower() + "_4";
			}
			return keyID.ToLower();
			IL_1638:
			return "-";
			IL_1650:
			return "shift";
			IL_1658:
			return "control";
			IL_1660:
			return "alt";
			IL_1670:
			return "controllerlstick";
			IL_16A8:
			text = "slash";
			return text;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000057EC File Offset: 0x000039EC
		private void SetKeyVisual(string visualName)
		{
			string text = this.IconsPath + "\\" + visualName;
			bool flag = Input.ControllerType.IsPlaystation();
			if (Input.IsGamepadActive && flag)
			{
				base.Sprite = base.Context.SpriteData.GetSprite(text + "_ps");
				return;
			}
			base.Sprite = base.Context.SpriteData.GetSprite(text);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005858 File Offset: 0x00003A58
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00005860 File Offset: 0x00003A60
		public string KeyID
		{
			get
			{
				return this._keyID;
			}
			set
			{
				if (value != this._keyID)
				{
					this._keyID = value;
					this._visualName = this.GetKeyVisualName(value);
					this.SetKeyVisual(this._visualName);
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005890 File Offset: 0x00003A90
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00005898 File Offset: 0x00003A98
		public string IconsPath
		{
			get
			{
				return this._iconsPath;
			}
			set
			{
				if (value != this._iconsPath)
				{
					this._iconsPath = value;
					this.SetKeyVisual(this._visualName);
				}
			}
		}

		// Token: 0x0400005A RID: 90
		private string _visualName = "None";

		// Token: 0x0400005B RID: 91
		private string _keyID;

		// Token: 0x0400005C RID: 92
		private string _iconsPath = "General\\InputKeys";
	}
}
