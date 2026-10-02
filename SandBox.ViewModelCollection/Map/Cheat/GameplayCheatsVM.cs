using System;
using System.Collections.Generic;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x02000051 RID: 81
	public class GameplayCheatsVM : ViewModel
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x000134CC File Offset: 0x000116CC
		public GameplayCheatsVM(Action onClose, IEnumerable<GameplayCheatBase> cheats)
		{
			this._onClose = onClose;
			this._initialCheatList = cheats;
			this.Cheats = new MBBindingList<CheatItemBaseVM>();
			this._activeCheatGroups = new List<CheatGroupItemVM>();
			this._mainTitleText = new TextObject("{=OYtysXzk}Cheats", null);
			this.FillWithCheats(cheats);
			this.RefreshValues();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00013524 File Offset: 0x00011724
		public override void RefreshValues()
		{
			base.RefreshValues();
			for (int i = 0; i < this.Cheats.Count; i++)
			{
				this.Cheats[i].RefreshValues();
			}
			if (this._activeCheatGroups.Count > 0)
			{
				TextObject textObject = new TextObject("{=1tiF5JhE}{TITLE} > {SUBTITLE}", null);
				for (int j = 0; j < this._activeCheatGroups.Count; j++)
				{
					if (j == 0)
					{
						textObject.SetTextVariable("TITLE", this._mainTitleText.ToString());
					}
					else
					{
						textObject.SetTextVariable("TITLE", textObject.ToString());
					}
					textObject.SetTextVariable("SUBTITLE", this._activeCheatGroups[j].Name.ToString());
				}
				this.Title = textObject.ToString();
				this.ButtonCloseLabel = GameTexts.FindText("str_back", null).ToString();
				return;
			}
			this.Title = this._mainTitleText.ToString();
			this.ButtonCloseLabel = GameTexts.FindText("str_close", null).ToString();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001362B File Offset: 0x0001182B
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM closeInputKey = this.CloseInputKey;
			if (closeInputKey == null)
			{
				return;
			}
			closeInputKey.OnFinalize();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00013644 File Offset: 0x00011844
		private void FillWithCheats(IEnumerable<GameplayCheatBase> cheats)
		{
			this.Cheats.Clear();
			foreach (GameplayCheatBase gameplayCheatBase in cheats)
			{
				GameplayCheatItem gameplayCheatItem;
				GameplayCheatGroup gameplayCheatGroup;
				if ((gameplayCheatItem = gameplayCheatBase as GameplayCheatItem) != null)
				{
					this.Cheats.Add(new CheatActionItemVM(gameplayCheatItem, new Action<CheatActionItemVM>(this.OnCheatActionExecuted)));
				}
				else if ((gameplayCheatGroup = gameplayCheatBase as GameplayCheatGroup) != null)
				{
					this.Cheats.Add(new CheatGroupItemVM(gameplayCheatGroup, new Action<CheatGroupItemVM>(this.OnCheatGroupSelected)));
				}
			}
			this.RefreshValues();
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000136E8 File Offset: 0x000118E8
		private void OnCheatActionExecuted(CheatActionItemVM cheatItem)
		{
			this._activeCheatGroups.Clear();
			this.FillWithCheats(this._initialCheatList);
			TextObject textObject = new TextObject("{=1QAEyN2V}Cheat Used: {CHEAT}", null);
			textObject.SetTextVariable("CHEAT", cheatItem.Name.ToString());
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001373D File Offset: 0x0001193D
		private void OnCheatGroupSelected(CheatGroupItemVM cheatGroup)
		{
			this._activeCheatGroups.Add(cheatGroup);
			IEnumerable<GameplayCheatBase> enumerable;
			if (cheatGroup == null)
			{
				enumerable = null;
			}
			else
			{
				GameplayCheatGroup cheatGroup2 = cheatGroup.CheatGroup;
				enumerable = ((cheatGroup2 != null) ? cheatGroup2.GetCheats() : null);
			}
			this.FillWithCheats(enumerable ?? this._initialCheatList);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00013774 File Offset: 0x00011974
		public void ExecuteClose()
		{
			if (this._activeCheatGroups.Count > 0)
			{
				this._activeCheatGroups.RemoveAt(this._activeCheatGroups.Count - 1);
				if (this._activeCheatGroups.Count > 0)
				{
					this.FillWithCheats(this._activeCheatGroups[this._activeCheatGroups.Count - 1].CheatGroup.GetCheats());
					return;
				}
				this.FillWithCheats(this._initialCheatList);
				return;
			}
			else
			{
				Action onClose = this._onClose;
				if (onClose == null)
				{
					return;
				}
				onClose();
				return;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x000137FB File Offset: 0x000119FB
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00013803 File Offset: 0x00011A03
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00013826 File Offset: 0x00011A26
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x0001382E File Offset: 0x00011A2E
		[DataSourceProperty]
		public string ButtonCloseLabel
		{
			get
			{
				return this._buttonCloseLabel;
			}
			set
			{
				if (value != this._buttonCloseLabel)
				{
					this._buttonCloseLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonCloseLabel");
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00013851 File Offset: 0x00011A51
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x00013859 File Offset: 0x00011A59
		[DataSourceProperty]
		public MBBindingList<CheatItemBaseVM> Cheats
		{
			get
			{
				return this._cheats;
			}
			set
			{
				if (value != this._cheats)
				{
					this._cheats = value;
					base.OnPropertyChangedWithValue<MBBindingList<CheatItemBaseVM>>(value, "Cheats");
				}
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00013877 File Offset: 0x00011A77
		public void SetCloseInputKey(HotKey hotKey)
		{
			this.CloseInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00013886 File Offset: 0x00011A86
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001388E File Offset: 0x00011A8E
		[DataSourceProperty]
		public InputKeyItemVM CloseInputKey
		{
			get
			{
				return this._closeInputKey;
			}
			set
			{
				if (value != this._closeInputKey)
				{
					this._closeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CloseInputKey");
				}
			}
		}

		// Token: 0x04000283 RID: 643
		private readonly Action _onClose;

		// Token: 0x04000284 RID: 644
		private readonly IEnumerable<GameplayCheatBase> _initialCheatList;

		// Token: 0x04000285 RID: 645
		private readonly TextObject _mainTitleText;

		// Token: 0x04000286 RID: 646
		private List<CheatGroupItemVM> _activeCheatGroups;

		// Token: 0x04000287 RID: 647
		private string _title;

		// Token: 0x04000288 RID: 648
		private string _buttonCloseLabel;

		// Token: 0x04000289 RID: 649
		private MBBindingList<CheatItemBaseVM> _cheats;

		// Token: 0x0400028A RID: 650
		private InputKeyItemVM _closeInputKey;
	}
}
