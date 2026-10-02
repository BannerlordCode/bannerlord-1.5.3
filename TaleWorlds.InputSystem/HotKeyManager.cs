using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x02000008 RID: 8
	public static class HotKeyManager
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600009F RID: 159 RVA: 0x00003340 File Offset: 0x00001540
		// (remove) Token: 0x060000A0 RID: 160 RVA: 0x00003374 File Offset: 0x00001574
		public static event HotKeyManager.OnKeybindsChangedEvent OnKeybindsChanged;

		// Token: 0x060000A1 RID: 161 RVA: 0x000033A8 File Offset: 0x000015A8
		public static string GetHotKeyId(string categoryName, string hotKeyId)
		{
			GameKeyContext gameKeyContext;
			if (HotKeyManager._categories.TryGetValue(categoryName, out gameKeyContext))
			{
				return gameKeyContext.GetHotKeyId(hotKeyId);
			}
			Debug.FailedAssert("Key category with id \"" + categoryName + "\" doesn't exsist.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\HotkeyManager.cs", "GetHotKeyId", 36);
			return "";
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000033F4 File Offset: 0x000015F4
		public static string GetHotKeyId(string categoryName, int hotKeyId)
		{
			GameKeyContext gameKeyContext;
			if (HotKeyManager._categories.TryGetValue(categoryName, out gameKeyContext))
			{
				return gameKeyContext.GetHotKeyId(hotKeyId);
			}
			Debug.FailedAssert("Key category with id \"" + categoryName + "\" doesn't exsist.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\HotkeyManager.cs", "GetHotKeyId", 47);
			return "invalid";
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000343E File Offset: 0x0000163E
		public static GameKeyContext GetCategory(string categoryName)
		{
			return HotKeyManager._categories[categoryName];
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000344B File Offset: 0x0000164B
		public static Dictionary<string, GameKeyContext>.ValueCollection GetAllCategories()
		{
			return HotKeyManager._categories.Values;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003457 File Offset: 0x00001657
		public static void Tick(float dt)
		{
			if (!HotKeyManager._isSaveLoadInProgress)
			{
				HotKeyManager.HandleSaveLoad();
			}
			if (!HotKeyManager._isSaveLoadInProgress && HotKeyManager._needsKeybindsChangedEvent)
			{
				HotKeyManager.OnKeybindsChangedEvent onKeybindsChanged = HotKeyManager.OnKeybindsChanged;
				if (onKeybindsChanged != null)
				{
					onKeybindsChanged();
				}
				HotKeyManager._needsKeybindsChangedEvent = false;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000348C File Offset: 0x0000168C
		private static async void HandleSaveLoad()
		{
			if (HotKeyManager._needsLoading || HotKeyManager._needsSaving)
			{
				HotKeyManager._isSaveLoadInProgress = true;
				if (HotKeyManager._needsLoading)
				{
					await HotKeyManager.LoadAsync();
					HotKeyManager._needsLoading = false;
				}
				if (HotKeyManager._needsSaving)
				{
					await HotKeyManager.SaveAsync();
					HotKeyManager._needsSaving = false;
				}
				HotKeyManager._isSaveLoadInProgress = false;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000034BD File Offset: 0x000016BD
		public static void Initialize(PlatformFilePath savePath, bool isRDownSwappedWithRRight)
		{
			GameKeyContext.SetIsRDownSwappedWithRRight(isRDownSwappedWithRRight);
			HotKeyManager._savePath = savePath;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000034CC File Offset: 0x000016CC
		public static void RegisterInitialContexts(IEnumerable<GameKeyContext> contexts)
		{
			HotKeyManager._categories.Clear();
			foreach (GameKeyContext gameKeyContext in contexts)
			{
				HotKeyManager.RegisterContext(gameKeyContext, gameKeyContext.Type == GameKeyContext.GameKeyContextType.AuxiliaryNotSerialized);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003524 File Offset: 0x00001724
		public static void RegisterContext(GameKeyContext context, bool ignoreSerialize = false)
		{
			if (!HotKeyManager._categories.ContainsKey(context.GameKeyCategoryId))
			{
				HotKeyManager._categories.Add(context.GameKeyCategoryId, context);
				HotKeyManager._needsLoading = true;
			}
			if (ignoreSerialize && !HotKeyManager._serializeIgnoredCategories.Contains(context.GameKeyCategoryId))
			{
				HotKeyManager._serializeIgnoredCategories.Add(context.GameKeyCategoryId);
				HotKeyManager._needsLoading = true;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003585 File Offset: 0x00001785
		public static bool ShouldNotifyDocumentVersionDifferent()
		{
			bool notifyDocumentVersionDifferent = HotKeyManager._notifyDocumentVersionDifferent;
			HotKeyManager._notifyDocumentVersionDifferent = false;
			return notifyDocumentVersionDifferent;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003594 File Offset: 0x00001794
		public static void Reset()
		{
			foreach (GameKeyContext gameKeyContext in HotKeyManager._categories.Values)
			{
				foreach (GameKey gameKey in gameKeyContext.RegisteredGameKeys)
				{
					if (gameKey != null)
					{
						Key controllerKey = gameKey.ControllerKey;
						if (controllerKey != null)
						{
							Key defaultControllerKey = gameKey.DefaultControllerKey;
							controllerKey.ChangeKey((defaultControllerKey != null) ? defaultControllerKey.InputKey : InputKey.Invalid);
						}
						Key keyboardKey = gameKey.KeyboardKey;
						if (keyboardKey != null)
						{
							Key defaultKeyboardKey = gameKey.DefaultKeyboardKey;
							keyboardKey.ChangeKey((defaultKeyboardKey != null) ? defaultKeyboardKey.InputKey : InputKey.Invalid);
						}
					}
				}
				foreach (HotKey hotKey in gameKeyContext.RegisteredHotKeys)
				{
					if (hotKey != null)
					{
						hotKey.Keys.Clear();
						foreach (Key key in hotKey.DefaultKeys)
						{
							hotKey.Keys.Add(new Key(key.InputKey));
						}
					}
				}
				foreach (GameAxisKey gameAxisKey in gameKeyContext.RegisteredGameAxisKeys)
				{
					gameAxisKey.AxisKey.ChangeKey(gameAxisKey.DefaultAxisKey.InputKey);
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000037A4 File Offset: 0x000019A4
		private static async Task LoadAsync()
		{
			HotKeyManager._loadedCategories = null;
			if (FileHelper.FileExists(HotKeyManager._savePath))
			{
				try
				{
					XmlDocument document = new XmlDocument();
					await document.LoadAsync(HotKeyManager._savePath);
					XmlElement documentElement = document.DocumentElement;
					float num = 0f;
					if (documentElement.HasAttribute("hotkeyEditEnabled"))
					{
						HotKeyManager._hotkeyEditEnabled = Convert.ToBoolean(documentElement.GetAttribute("hotkeyEditEnabled"));
					}
					float num2;
					if (documentElement.HasAttribute("version") && float.TryParse(documentElement.GetAttribute("version"), out num2))
					{
						num = num2;
					}
					if (num != HotKeyManager._versionOfHotkeys)
					{
						HotKeyManager._notifyDocumentVersionDifferent = true;
						await HotKeyManager.SaveAsync();
					}
					else
					{
						HotKeyManager._loadedCategories = documentElement.ChildNodes;
						foreach (object obj in documentElement.ChildNodes)
						{
							XmlElement xmlElement = (XmlElement)((XmlNode)obj);
							string attribute = xmlElement.GetAttribute("name");
							GameKeyContext gameKeyContext;
							if (HotKeyManager._categories.TryGetValue(attribute, out gameKeyContext))
							{
								foreach (object obj2 in xmlElement.ChildNodes)
								{
									XmlNode xmlNode = (XmlNode)obj2;
									string name = ((XmlElement)xmlNode).Name;
									if (name == "GameKey")
									{
										string innerText = xmlNode["Id"].InnerText;
										GameKey gameKey = gameKeyContext.GetGameKey(innerText);
										if (gameKey != null)
										{
											XmlElement xmlElement2 = xmlNode["Keys"]["KeyboardKey"];
											if (xmlElement2 != null)
											{
												InputKey inputKey;
												if (Enum.TryParse<InputKey>(xmlElement2.InnerText, out inputKey))
												{
													if (gameKey.KeyboardKey != null)
													{
														gameKey.KeyboardKey.ChangeKey(inputKey);
													}
													else
													{
														gameKey.KeyboardKey = new Key(inputKey);
													}
												}
											}
											else if (gameKey.DefaultKeyboardKey != null && gameKey.DefaultKeyboardKey.InputKey != InputKey.Invalid)
											{
												gameKey.KeyboardKey = new Key(gameKey.DefaultKeyboardKey.InputKey);
											}
											else
											{
												gameKey.KeyboardKey = new Key(InputKey.Invalid);
											}
										}
									}
									else if (HotKeyManager._hotkeyEditEnabled || gameKeyContext.Type == GameKeyContext.GameKeyContextType.AuxiliarySerializedAndShownInOptions)
									{
										if (name == "GameAxisKey")
										{
											string innerText2 = xmlNode["Id"].InnerText;
											GameAxisKey gameAxisKey = gameKeyContext.GetGameAxisKey(innerText2);
											if (gameAxisKey != null)
											{
												XmlElement xmlElement3 = xmlNode["Keys"];
												if (!gameAxisKey.IsBinded)
												{
													XmlElement xmlElement4 = xmlElement3["PositiveKey"];
													if (xmlElement4 != null)
													{
														if (xmlElement4.InnerText != "None")
														{
															InputKey inputKey2;
															if (Enum.TryParse<InputKey>(xmlElement4.InnerText, out inputKey2))
															{
																gameAxisKey.PositiveKey = new GameKey(-1, gameAxisKey.Id + "_p", attribute, inputKey2, "");
															}
														}
														else
														{
															gameAxisKey.PositiveKey = null;
														}
													}
													XmlElement xmlElement5 = xmlElement3["NegativeKey"];
													if (xmlElement5 != null)
													{
														if (xmlElement5.InnerText != "None")
														{
															InputKey inputKey3;
															if (Enum.TryParse<InputKey>(xmlElement5.InnerText, out inputKey3))
															{
																gameAxisKey.NegativeKey = new GameKey(-1, gameAxisKey.Id + "_n", attribute, inputKey3, "");
															}
														}
														else
														{
															gameAxisKey.NegativeKey = null;
														}
													}
												}
												XmlElement xmlElement6 = xmlElement3["AxisKey"];
												if (xmlElement6 != null)
												{
													if (xmlElement6.InnerText != "None")
													{
														InputKey inputKey4;
														if (Enum.TryParse<InputKey>(xmlElement6.InnerText, out inputKey4))
														{
															gameAxisKey.AxisKey = new Key(inputKey4);
														}
													}
													else
													{
														gameAxisKey.AxisKey = null;
													}
												}
											}
										}
										else if (name == "HotKey")
										{
											string innerText3 = xmlNode["Id"].InnerText;
											HotKey hotKey = gameKeyContext.GetHotKey(innerText3);
											if (hotKey != null)
											{
												new List<HotKey>();
												XmlElement xmlElement7 = xmlNode["Keys"];
												hotKey.Keys = new List<Key>();
												for (int i = 0; i < xmlElement7.ChildNodes.Count; i++)
												{
													InputKey inputKey5;
													if (Enum.TryParse<InputKey>(xmlElement7.ChildNodes[i].InnerText, out inputKey5))
													{
														hotKey.Keys.Add(new Key(inputKey5));
													}
												}
											}
										}
									}
								}
							}
						}
						document = null;
					}
				}
				catch (Exception ex)
				{
					Debug.FailedAssert("Couldn't load key bindings: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\HotkeyManager.cs", "LoadAsync", 390);
					HotKeyManager._loadedCategories = null;
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000037E1 File Offset: 0x000019E1
		public static void MarkForSave()
		{
			HotKeyManager._needsSaving = true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000037EC File Offset: 0x000019EC
		private static async Task SaveAsync()
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
				XmlElement documentElement = xmlDocument.DocumentElement;
				xmlDocument.InsertBefore(xmlDeclaration, documentElement);
				XmlComment xmlComment = xmlDocument.CreateComment("To override values other than GameKeys, change hotkeyEditEnabled to True.");
				xmlDocument.InsertBefore(xmlComment, documentElement);
				XmlElement xmlElement = xmlDocument.CreateElement("HotKeyCategories");
				xmlElement.SetAttribute("hotkeyEditEnabled", HotKeyManager._hotkeyEditEnabled.ToString());
				xmlElement.SetAttribute("version", HotKeyManager._versionOfHotkeys.ToString());
				xmlDocument.AppendChild(xmlElement);
				foreach (KeyValuePair<string, GameKeyContext> keyValuePair in HotKeyManager._categories)
				{
					if (!HotKeyManager._serializeIgnoredCategories.Contains(keyValuePair.Key))
					{
						XmlElement xmlElement2 = HotKeyManager.CreateGameKeyContextNode(keyValuePair.Key, keyValuePair.Value, ref xmlDocument);
						xmlElement.AppendChild(xmlElement2);
					}
				}
				if (HotKeyManager._loadedCategories != null)
				{
					foreach (object obj in HotKeyManager._loadedCategories)
					{
						XmlElement xmlElement3 = (XmlElement)obj;
						GameKeyContext gameKeyContext;
						if (!HotKeyManager._categories.TryGetValue(xmlElement3.GetAttribute("name"), out gameKeyContext))
						{
							xmlElement.AppendChild(xmlDocument.ImportNode(xmlElement3, true));
						}
					}
				}
				await xmlDocument.SaveAsync(HotKeyManager._savePath);
				HotKeyManager._needsKeybindsChangedEvent = true;
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Couldn't save key bindings: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.InputSystem\\HotkeyManager.cs", "SaveAsync", 447);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000382C File Offset: 0x00001A2C
		private static XmlElement CreateGameKeyContextNode(string name, GameKeyContext context, ref XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("HotKeyCategory");
			xmlElement.SetAttribute("name", name);
			foreach (GameKey gameKey in context.RegisteredGameKeys)
			{
				if (gameKey != null)
				{
					XmlElement xmlElement2 = document.CreateElement("GameKey");
					xmlElement.AppendChild(xmlElement2);
					XmlElement xmlElement3 = document.CreateElement("Id");
					xmlElement2.AppendChild(xmlElement3);
					xmlElement3.InnerText = gameKey.StringId;
					XmlElement xmlElement4 = document.CreateElement("Keys");
					xmlElement2.AppendChild(xmlElement4);
					XmlElement xmlElement5 = document.CreateElement("KeyboardKey");
					xmlElement4.AppendChild(xmlElement5);
					xmlElement5.InnerText = ((gameKey.KeyboardKey != null) ? gameKey.KeyboardKey.InputKey.ToString() : "None");
					XmlElement xmlElement6 = document.CreateElement("ControllerKey");
					xmlElement4.AppendChild(xmlElement6);
					xmlElement6.InnerText = ((gameKey.ControllerKey != null) ? gameKey.ControllerKey.InputKey.ToString() : "None");
				}
			}
			foreach (GameAxisKey gameAxisKey in context.RegisteredGameAxisKeys)
			{
				XmlElement xmlElement7 = document.CreateElement("GameAxisKey");
				xmlElement.AppendChild(xmlElement7);
				XmlElement xmlElement8 = document.CreateElement("Id");
				xmlElement7.AppendChild(xmlElement8);
				xmlElement8.InnerText = gameAxisKey.Id;
				XmlElement xmlElement9 = document.CreateElement("Keys");
				xmlElement7.AppendChild(xmlElement9);
				XmlElement xmlElement10 = document.CreateElement("PositiveKey");
				xmlElement9.AppendChild(xmlElement10);
				xmlElement10.InnerText = ((gameAxisKey.PositiveKey != null) ? gameAxisKey.PositiveKey.KeyboardKey.InputKey.ToString() : "None");
				XmlElement xmlElement11 = document.CreateElement("NegativeKey");
				xmlElement9.AppendChild(xmlElement11);
				xmlElement11.InnerText = ((gameAxisKey.NegativeKey != null) ? gameAxisKey.NegativeKey.KeyboardKey.InputKey.ToString() : "None");
				XmlElement xmlElement12 = document.CreateElement("AxisKey");
				xmlElement9.AppendChild(xmlElement12);
				xmlElement12.InnerText = ((gameAxisKey.AxisKey != null) ? gameAxisKey.AxisKey.InputKey.ToString() : "None");
			}
			foreach (HotKey hotKey in context.RegisteredHotKeys)
			{
				XmlElement xmlElement13 = document.CreateElement("HotKey");
				xmlElement.AppendChild(xmlElement13);
				XmlElement xmlElement14 = document.CreateElement("Id");
				xmlElement13.AppendChild(xmlElement14);
				xmlElement14.InnerText = hotKey.Id;
				XmlElement xmlElement15 = document.CreateElement("Keys");
				xmlElement13.AppendChild(xmlElement15);
				foreach (Key key in hotKey.Keys)
				{
					XmlElement xmlElement16 = document.CreateElement("Key");
					xmlElement15.AppendChild(xmlElement16);
					xmlElement16.InnerText = key.InputKey.ToString();
				}
			}
			return xmlElement;
		}

		// Token: 0x0400001E RID: 30
		private static readonly Dictionary<string, GameKeyContext> _categories = new Dictionary<string, GameKeyContext>();

		// Token: 0x0400001F RID: 31
		private static readonly List<string> _serializeIgnoredCategories = new List<string>();

		// Token: 0x04000020 RID: 32
		private static readonly float _versionOfHotkeys = 5.1f;

		// Token: 0x04000021 RID: 33
		private static XmlNodeList _loadedCategories;

		// Token: 0x04000022 RID: 34
		private static bool _hotkeyEditEnabled = false;

		// Token: 0x04000023 RID: 35
		private static bool _notifyDocumentVersionDifferent = false;

		// Token: 0x04000024 RID: 36
		private static PlatformFilePath _savePath;

		// Token: 0x04000025 RID: 37
		private static bool _needsLoading;

		// Token: 0x04000026 RID: 38
		private static bool _needsSaving;

		// Token: 0x04000027 RID: 39
		private static bool _needsKeybindsChangedEvent;

		// Token: 0x04000028 RID: 40
		private static bool _isSaveLoadInProgress;

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x060001A8 RID: 424
		public delegate void OnKeybindsChangedEvent();
	}
}
