using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.AuxiliaryKeys;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys
{
	// Token: 0x02000078 RID: 120
	public class GameKeyOptionCategoryVM : ViewModel
	{
		// Token: 0x06000989 RID: 2441 RVA: 0x00020600 File Offset: 0x0001E800
		public GameKeyOptionCategoryVM(Action<KeyOptionVM> onKeybindRequest, IEnumerable<string> gameKeyCategories, IEnumerable<int> hiddenGameKeys)
		{
			this._gameKeyCategories = new Dictionary<string, List<GameKey>>();
			foreach (string text in gameKeyCategories)
			{
				this._gameKeyCategories.Add(text, new List<GameKey>());
			}
			this._onKeybindRequest = onKeybindRequest;
			this.GameKeyGroups = new MBBindingList<GameKeyGroupVM>();
			this._auxiliaryKeyCategories = new Dictionary<string, List<HotKey>>();
			this.AuxiliaryKeyGroups = new MBBindingList<AuxiliaryKeyGroupVM>();
			foreach (GameKeyContext gameKeyContext in HotKeyManager.GetAllCategories())
			{
				if (gameKeyContext.Type == GameKeyContext.GameKeyContextType.AuxiliarySerializedAndShownInOptions)
				{
					this._auxiliaryKeyCategories.Add(gameKeyContext.GameKeyCategoryId, new List<HotKey>());
					using (Dictionary<string, HotKey>.ValueCollection.Enumerator enumerator3 = gameKeyContext.RegisteredHotKeys.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							HotKey hotKey = enumerator3.Current;
							List<HotKey> list;
							if (hotKey != null && this._auxiliaryKeyCategories.TryGetValue(hotKey.GroupId, out list) && !list.Contains(hotKey))
							{
								list.Add(hotKey);
							}
						}
						continue;
					}
				}
				if (gameKeyContext.Type == GameKeyContext.GameKeyContextType.Default)
				{
					foreach (GameKey gameKey in gameKeyContext.RegisteredGameKeys)
					{
						List<GameKey> list2;
						if (gameKey != null && !hiddenGameKeys.Contains(gameKey.Id) && this._gameKeyCategories.TryGetValue(gameKey.MainCategoryId, out list2) && !list2.Contains(gameKey))
						{
							list2.Add(gameKey);
						}
					}
				}
			}
			foreach (KeyValuePair<string, List<GameKey>> keyValuePair in this._gameKeyCategories)
			{
				if (keyValuePair.Value.Count > 0)
				{
					this.GameKeyGroups.Add(new GameKeyGroupVM(keyValuePair.Key, keyValuePair.Value, this._onKeybindRequest, new Action<int, InputKey>(this.UpdateKeysOfGamekeysWithID), new Func<KeyOptionVM, string>(this.GetExtraInformationText)));
				}
			}
			foreach (KeyValuePair<string, List<HotKey>> keyValuePair2 in this._auxiliaryKeyCategories)
			{
				if (keyValuePair2.Value.Count > 0)
				{
					this.AuxiliaryKeyGroups.Add(new AuxiliaryKeyGroupVM(keyValuePair2.Key, keyValuePair2.Value, this._onKeybindRequest, new Func<KeyOptionVM, string>(this.GetExtraInformationText)));
				}
			}
			this.RefreshValues();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			this.IsEnabled = !Input.IsGamepadActive;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00020968 File Offset: 0x0001EB68
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = new TextObject("{=qmNeO8FG}Keybindings", null).ToString();
			this.ResetText = new TextObject("{=RVIKFCno}Reset to Defaults", null).ToString();
			this.GameKeyGroups.ApplyActionOnAllItems(delegate(GameKeyGroupVM x)
			{
				x.RefreshValues();
			});
			this.AuxiliaryKeyGroups.ApplyActionOnAllItems(delegate(AuxiliaryKeyGroupVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000209FC File Offset: 0x0001EBFC
		private void OnGamepadActiveStateChanged()
		{
			MBBindingList<GameKeyGroupVM> gameKeyGroups = this.GameKeyGroups;
			if (gameKeyGroups != null)
			{
				gameKeyGroups.ApplyActionOnAllItems(delegate(GameKeyGroupVM g)
				{
					g.OnGamepadActiveStateChanged();
				});
			}
			this.AuxiliaryKeyGroups.ApplyActionOnAllItems(delegate(AuxiliaryKeyGroupVM x)
			{
				x.OnGamepadActiveStateChanged();
			});
			this.IsEnabled = !Input.IsGamepadActive;
			Debug.Print("KEYBINDS TAB ENABLED: " + this.IsEnabled.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00020A9C File Offset: 0x0001EC9C
		public bool IsChanged()
		{
			if (this.GameKeyGroups != null)
			{
				for (int i = 0; i < this.GameKeyGroups.Count; i++)
				{
					if (this.GameKeyGroups[i].IsChanged())
					{
						return true;
					}
				}
			}
			if (this.AuxiliaryKeyGroups != null)
			{
				for (int j = 0; j < this.AuxiliaryKeyGroups.Count; j++)
				{
					if (this.AuxiliaryKeyGroups[j].IsChanged())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00020B10 File Offset: 0x0001ED10
		public void ExecuteResetToDefault()
		{
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=4gCU2ykB}Reset all keys to default", null).ToString(), new TextObject("{=YjbNtFcw}This will reset ALL keys to their default states. You won't be able to undo this action. {newline} {newline}Are you sure?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), delegate
			{
				this.ResetToDefault();
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00020B88 File Offset: 0x0001ED88
		public void OnDone()
		{
			this.GameKeyGroups.ApplyActionOnAllItems(delegate(GameKeyGroupVM x)
			{
				x.OnDone();
			});
			this.AuxiliaryKeyGroups.ApplyActionOnAllItems(delegate(AuxiliaryKeyGroupVM x)
			{
				x.OnDone();
			});
			foreach (KeyValuePair<GameKey, InputKey> keyValuePair in this._keysToChangeOnDone)
			{
				Key key = this.FindValidInputKey(keyValuePair.Key);
				if (key != null)
				{
					key.ChangeKey(keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00020C48 File Offset: 0x0001EE48
		private void ResetToDefault()
		{
			HotKeyManager.Reset();
			this.GameKeyGroups.ApplyActionOnAllItems(delegate(GameKeyGroupVM x)
			{
				x.Update();
			});
			this.AuxiliaryKeyGroups.ApplyActionOnAllItems(delegate(AuxiliaryKeyGroupVM x)
			{
				x.Update();
			});
			this._keysToChangeOnDone.Clear();
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00020CB9 File Offset: 0x0001EEB9
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00020CE1 File Offset: 0x0001EEE1
		private Key FindValidInputKey(GameKey gameKey)
		{
			if (!Input.IsGamepadActive)
			{
				return gameKey.KeyboardKey;
			}
			return gameKey.ControllerKey;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00020CF8 File Offset: 0x0001EEF8
		private void UpdateKeysOfGamekeysWithID(int givenId, InputKey newKey)
		{
			Func<GameKey, bool> <>9__0;
			foreach (GameKeyContext gameKeyContext in HotKeyManager.GetAllCategories())
			{
				if (gameKeyContext.Type == GameKeyContext.GameKeyContextType.Default)
				{
					IEnumerable<GameKey> registeredGameKeys = gameKeyContext.RegisteredGameKeys;
					Func<GameKey, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (GameKey k) => k != null && k.Id == givenId);
					}
					foreach (GameKey gameKey in registeredGameKeys.Where<GameKey>(func))
					{
						if (this._keysToChangeOnDone.ContainsKey(gameKey))
						{
							this._keysToChangeOnDone[gameKey] = newKey;
						}
						else
						{
							this._keysToChangeOnDone.Add(gameKey, newKey);
						}
					}
				}
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00020DF0 File Offset: 0x0001EFF0
		private string GetExtraInformationText(KeyOptionVM keyVM)
		{
			List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
			foreach (GameKeyGroupVM gameKeyGroupVM in this.GameKeyGroups)
			{
				foreach (GameKeyOptionVM gameKeyOptionVM in gameKeyGroupVM.GameKeys)
				{
					if (gameKeyOptionVM != keyVM && gameKeyOptionVM.CurrentKey == keyVM.CurrentKey)
					{
						list.Add(new ValueTuple<string, string>(gameKeyOptionVM.Name, gameKeyGroupVM.Description));
					}
				}
			}
			foreach (AuxiliaryKeyGroupVM auxiliaryKeyGroupVM in this.AuxiliaryKeyGroups)
			{
				foreach (AuxiliaryKeyOptionVM auxiliaryKeyOptionVM in auxiliaryKeyGroupVM.HotKeys)
				{
					if (auxiliaryKeyOptionVM != keyVM && auxiliaryKeyOptionVM.CurrentKey == keyVM.CurrentKey)
					{
						list.Add(new ValueTuple<string, string>(auxiliaryKeyOptionVM.Name, auxiliaryKeyGroupVM.Description));
					}
				}
			}
			if (list.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ValueTuple<string, string> valueTuple in list)
				{
					string item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					stringBuilder.AppendLine(string.Concat(new string[] { "- ", item, " (", item2, ")" }));
				}
				return new TextObject("{=Wp6pM9ea}[{CURRENT_KEY}] also used as:{newline}{KEYS}", null).SetTextVariable("CURRENT_KEY", keyVM.OptionValueText).SetTextVariable("newline", "\n").SetTextVariable("KEYS", stringBuilder.ToString().TrimEnd(Array.Empty<char>()))
					.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002102C File Offset: 0x0001F22C
		public void Cancel()
		{
			this.GameKeyGroups.ApplyActionOnAllItems(delegate(GameKeyGroupVM g)
			{
				g.Cancel();
			});
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00021058 File Offset: 0x0001F258
		public void ApplyValues()
		{
			this.GameKeyGroups.ApplyActionOnAllItems(delegate(GameKeyGroupVM g)
			{
				g.ApplyValues();
			});
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00021084 File Offset: 0x0001F284
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0002108C File Offset: 0x0001F28C
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x000210AF File Offset: 0x0001F2AF
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x000210B7 File Offset: 0x0001F2B7
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x000210D5 File Offset: 0x0001F2D5
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x000210DD File Offset: 0x0001F2DD
		[DataSourceProperty]
		public string ResetText
		{
			get
			{
				return this._resetText;
			}
			set
			{
				if (value != this._resetText)
				{
					this._resetText = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetText");
				}
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00021100 File Offset: 0x0001F300
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x00021108 File Offset: 0x0001F308
		[DataSourceProperty]
		public MBBindingList<GameKeyGroupVM> GameKeyGroups
		{
			get
			{
				return this._gameKeyGroups;
			}
			set
			{
				if (value != this._gameKeyGroups)
				{
					this._gameKeyGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameKeyGroupVM>>(value, "GameKeyGroups");
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00021126 File Offset: 0x0001F326
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0002112E File Offset: 0x0001F32E
		[DataSourceProperty]
		public MBBindingList<AuxiliaryKeyGroupVM> AuxiliaryKeyGroups
		{
			get
			{
				return this._auxiliaryKeyGroups;
			}
			set
			{
				if (value != this._auxiliaryKeyGroups)
				{
					this._auxiliaryKeyGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<AuxiliaryKeyGroupVM>>(value, "AuxiliaryKeyGroups");
				}
			}
		}

		// Token: 0x04000449 RID: 1097
		private readonly Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x0400044A RID: 1098
		private Dictionary<string, List<GameKey>> _gameKeyCategories;

		// Token: 0x0400044B RID: 1099
		private Dictionary<string, List<HotKey>> _auxiliaryKeyCategories;

		// Token: 0x0400044C RID: 1100
		private Dictionary<GameKey, InputKey> _keysToChangeOnDone = new Dictionary<GameKey, InputKey>();

		// Token: 0x0400044D RID: 1101
		private string _name;

		// Token: 0x0400044E RID: 1102
		private string _resetText;

		// Token: 0x0400044F RID: 1103
		private bool _isEnabled;

		// Token: 0x04000450 RID: 1104
		private MBBindingList<GameKeyGroupVM> _gameKeyGroups;

		// Token: 0x04000451 RID: 1105
		private MBBindingList<AuxiliaryKeyGroupVM> _auxiliaryKeyGroups;
	}
}
