using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002A RID: 42
	public class MPLobbyCosmeticSigilItemVM : MPLobbySigilItemVM
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000BEDA File Offset: 0x0000A0DA
		public MPLobbyCosmeticSigilItemVM(int iconID, int rarity, int cost, string cosmeticID)
			: base(iconID, null)
		{
			this.Rarity = rarity;
			this.Cost = cost;
			this.CosmeticID = cosmeticID;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000BEFA File Offset: 0x0000A0FA
		public static void SetOnSelectionCallback(Action<MPLobbyCosmeticSigilItemVM> onSelection)
		{
			MPLobbyCosmeticSigilItemVM._onSelection = onSelection;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000BF02 File Offset: 0x0000A102
		public static void ResetOnSelectionCallback()
		{
			MPLobbyCosmeticSigilItemVM._onSelection = null;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000BF0A File Offset: 0x0000A10A
		public static void SetOnObtainRequestedCallback(Action<MPLobbyCosmeticSigilItemVM> onObtainRequested)
		{
			MPLobbyCosmeticSigilItemVM._onObtainRequested = onObtainRequested;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000BF12 File Offset: 0x0000A112
		public static void ResetOnObtainRequestedCallback()
		{
			MPLobbyCosmeticSigilItemVM._onObtainRequested = null;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000BF1A File Offset: 0x0000A11A
		private void ExecuteSelection()
		{
			if (this.IsUnlocked)
			{
				Action<MPLobbyCosmeticSigilItemVM> onSelection = MPLobbyCosmeticSigilItemVM._onSelection;
				if (onSelection == null)
				{
					return;
				}
				onSelection(this);
				return;
			}
			else
			{
				Action<MPLobbyCosmeticSigilItemVM> onObtainRequested = MPLobbyCosmeticSigilItemVM._onObtainRequested;
				if (onObtainRequested == null)
				{
					return;
				}
				onObtainRequested(this);
				return;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000BF45 File Offset: 0x0000A145
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0000BF4D File Offset: 0x0000A14D
		[DataSourceProperty]
		public bool IsUnlocked
		{
			get
			{
				return this._isUnlocked;
			}
			set
			{
				if (value != this._isUnlocked)
				{
					this._isUnlocked = value;
					base.OnPropertyChangedWithValue(value, "IsUnlocked");
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000BF6B File Offset: 0x0000A16B
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0000BF73 File Offset: 0x0000A173
		[DataSourceProperty]
		public bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				if (value != this._isUsed)
				{
					this._isUsed = value;
					base.OnPropertyChangedWithValue(value, "IsUsed");
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000BF91 File Offset: 0x0000A191
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000BF99 File Offset: 0x0000A199
		[DataSourceProperty]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (value != this._rarity)
				{
					this._rarity = value;
					base.OnPropertyChangedWithValue(value, "Rarity");
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000BFB7 File Offset: 0x0000A1B7
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000BFBF File Offset: 0x0000A1BF
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x04000193 RID: 403
		public readonly string CosmeticID;

		// Token: 0x04000194 RID: 404
		private static Action<MPLobbyCosmeticSigilItemVM> _onSelection;

		// Token: 0x04000195 RID: 405
		private static Action<MPLobbyCosmeticSigilItemVM> _onObtainRequested;

		// Token: 0x04000196 RID: 406
		private bool _isUnlocked;

		// Token: 0x04000197 RID: 407
		private bool _isUsed;

		// Token: 0x04000198 RID: 408
		private int _rarity;

		// Token: 0x04000199 RID: 409
		private int _cost;
	}
}
