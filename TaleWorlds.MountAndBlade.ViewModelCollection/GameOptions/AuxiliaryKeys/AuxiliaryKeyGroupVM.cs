using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.AuxiliaryKeys
{
	// Token: 0x0200007A RID: 122
	public class AuxiliaryKeyGroupVM : ViewModel
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x00021433 File Offset: 0x0001F633
		public AuxiliaryKeyGroupVM(string categoryId, IEnumerable<HotKey> keys, Action<KeyOptionVM> onKeybindRequest, Func<KeyOptionVM, string> getExtraInformation)
		{
			this._onKeybindRequest = onKeybindRequest;
			this._getExtraInformation = getExtraInformation;
			this._categoryId = categoryId;
			this._hotKeys = new MBBindingList<AuxiliaryKeyOptionVM>();
			this._keys = keys;
			this.PopulateHotKeys();
			this.RefreshValues();
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00021470 File Offset: 0x0001F670
		private void PopulateHotKeys()
		{
			this.HotKeys.Clear();
			foreach (HotKey hotKey in this._keys)
			{
				bool flag;
				if (!Input.IsGamepadActive)
				{
					if (hotKey == null)
					{
						flag = false;
					}
					else
					{
						flag = hotKey.DefaultKeys.Any<Key>((Key x) => x != null && x.IsKeyboardInput && x.InputKey != InputKey.Invalid);
					}
				}
				else if (hotKey == null)
				{
					flag = false;
				}
				else
				{
					flag = hotKey.DefaultKeys.Any<Key>((Key x) => x != null && x.IsControllerInput && x.InputKey != InputKey.Invalid);
				}
				if (flag)
				{
					this.HotKeys.Add(new AuxiliaryKeyOptionVM(hotKey, this._onKeybindRequest, new Action<AuxiliaryKeyOptionVM, InputKey>(this.SetHotKey), this._getExtraInformation));
				}
			}
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002155C File Offset: 0x0001F75C
		public override void RefreshValues()
		{
			base.RefreshValues();
			string text = this._categoryId;
			TextObject textObject;
			if (Module.CurrentModule.GlobalTextManager.TryGetText("str_hotkey_category_name", this._categoryId, out textObject))
			{
				text = textObject.ToString();
			}
			this.Description = text;
			this.HotKeys.ApplyActionOnAllItems(delegate(AuxiliaryKeyOptionVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000215CC File Offset: 0x0001F7CC
		private void SetHotKey(AuxiliaryKeyOptionVM option, InputKey newKey)
		{
			InputKey inputKey = option.CurrentKey.InputKey;
			if (newKey != inputKey)
			{
				option.CurrentKey.ChangeKey(newKey);
				option.OptionValueText = Module.CurrentModule.GlobalTextManager.GetHotKeyGameTextFromKeyID(option.CurrentKey.ToString().ToLower()).ToString();
				option.UpdateIsChanged();
				AuxiliaryKeyOptionVM auxiliaryKeyOptionVM = this.HotKeys.FirstOrDefault<AuxiliaryKeyOptionVM>((AuxiliaryKeyOptionVM k) => k != option && k.CurrentKey.InputKey == option.CurrentKey.InputKey && k.CurrentHotKey.HasSameModifiers(option.CurrentHotKey));
				if (auxiliaryKeyOptionVM != null)
				{
					auxiliaryKeyOptionVM.Set(inputKey);
				}
				if (auxiliaryKeyOptionVM != null)
				{
					MBInformationManager.AddQuickInformation(new TextObject("{=gb2S2aRq}Swapped {FIRST_KEY} and {SECOND_KEY}", null).SetTextVariable("FIRST_KEY", option.Name).SetTextVariable("SECOND_KEY", auxiliaryKeyOptionVM.Name), -1000, null, null, "");
				}
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000216B4 File Offset: 0x0001F8B4
		internal void Update()
		{
			foreach (AuxiliaryKeyOptionVM auxiliaryKeyOptionVM in this.HotKeys)
			{
				auxiliaryKeyOptionVM.Update();
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00021700 File Offset: 0x0001F900
		public void OnDone()
		{
			foreach (AuxiliaryKeyOptionVM auxiliaryKeyOptionVM in this.HotKeys)
			{
				auxiliaryKeyOptionVM.OnDone();
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002174C File Offset: 0x0001F94C
		internal bool IsChanged()
		{
			for (int i = 0; i < this.HotKeys.Count; i++)
			{
				if (this.HotKeys[i].IsChanged)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00021785 File Offset: 0x0001F985
		public void OnGamepadActiveStateChanged()
		{
			this.Update();
			this.OnDone();
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00021793 File Offset: 0x0001F993
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0002179B File Offset: 0x0001F99B
		[DataSourceProperty]
		public MBBindingList<AuxiliaryKeyOptionVM> HotKeys
		{
			get
			{
				return this._hotKeys;
			}
			set
			{
				if (value != this._hotKeys)
				{
					this._hotKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<AuxiliaryKeyOptionVM>>(value, "HotKeys");
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x000217B9 File Offset: 0x0001F9B9
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x000217C1 File Offset: 0x0001F9C1
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x04000456 RID: 1110
		private readonly Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x04000457 RID: 1111
		private readonly Func<KeyOptionVM, string> _getExtraInformation;

		// Token: 0x04000458 RID: 1112
		private readonly string _categoryId;

		// Token: 0x04000459 RID: 1113
		private IEnumerable<HotKey> _keys;

		// Token: 0x0400045A RID: 1114
		private string _description;

		// Token: 0x0400045B RID: 1115
		private MBBindingList<AuxiliaryKeyOptionVM> _hotKeys;
	}
}
