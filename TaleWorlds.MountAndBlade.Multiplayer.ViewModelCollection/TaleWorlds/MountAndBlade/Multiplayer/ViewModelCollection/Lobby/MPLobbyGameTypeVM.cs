using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002C RID: 44
	public class MPLobbyGameTypeVM : ViewModel
	{
		// Token: 0x0600033D RID: 829 RVA: 0x0000C522 File Offset: 0x0000A722
		public MPLobbyGameTypeVM(string gameType, bool isCasual, Action<string> onSelection)
		{
			this.GameTypeID = gameType;
			this.IsCasual = isCasual;
			this._onSelection = onSelection;
			this.RefreshValues();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C545 File Offset: 0x0000A745
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Hint = new HintViewModel(GameTexts.FindText("str_multiplayer_game_stats_description", this.GameTypeID), null);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000C569 File Offset: 0x0000A769
		private void OnSelected()
		{
			Action<string> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this.GameTypeID);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000C581 File Offset: 0x0000A781
		// (set) Token: 0x06000341 RID: 833 RVA: 0x0000C589 File Offset: 0x0000A789
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
					if (value)
					{
						this.OnSelected();
					}
				}
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0000C5B8 File Offset: 0x0000A7B8
		[DataSourceProperty]
		public string GameTypeID
		{
			get
			{
				return this._gameTypeID;
			}
			set
			{
				if (value != this._gameTypeID)
				{
					this._gameTypeID = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypeID");
				}
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000C5DB File Offset: 0x0000A7DB
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000C5E3 File Offset: 0x0000A7E3
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040001AA RID: 426
		private readonly Action<string> _onSelection;

		// Token: 0x040001AB RID: 427
		public readonly bool IsCasual;

		// Token: 0x040001AC RID: 428
		private bool _isSelected;

		// Token: 0x040001AD RID: 429
		private string _gameTypeID;

		// Token: 0x040001AE RID: 430
		private HintViewModel _hint;
	}
}
