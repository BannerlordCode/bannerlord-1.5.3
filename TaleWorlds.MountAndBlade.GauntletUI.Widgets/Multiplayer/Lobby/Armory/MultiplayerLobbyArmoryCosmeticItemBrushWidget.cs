using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B7 RID: 183
	public class MultiplayerLobbyArmoryCosmeticItemBrushWidget : BrushWidget
	{
		// Token: 0x060009B6 RID: 2486 RVA: 0x0001B7B5 File Offset: 0x000199B5
		public MultiplayerLobbyArmoryCosmeticItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001B7BE File Offset: 0x000199BE
		public override void SetState(string stateName)
		{
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001B7C0 File Offset: 0x000199C0
		private void OnUsageChanged()
		{
			base.SetState(this.IsUsed ? "Selected" : "Default");
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001B7DC File Offset: 0x000199DC
		private void OnRarityChanged()
		{
			switch (this.Rarity)
			{
			case 0:
			case 1:
				base.Brush = base.Context.GetBrush("MPLobby.Armory.CosmeticButton.Common");
				return;
			case 2:
				base.Brush = base.Context.GetBrush("MPLobby.Armory.CosmeticButton.Rare");
				return;
			case 3:
				base.Brush = base.Context.GetBrush("MPLobby.Armory.CosmeticButton.Unique");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0001B84B File Offset: 0x00019A4B
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0001B853 File Offset: 0x00019A53
		[Editor(false)]
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
					base.OnPropertyChanged(value, "IsUsed");
					this.OnUsageChanged();
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001B877 File Offset: 0x00019A77
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0001B87F File Offset: 0x00019A7F
		[Editor(false)]
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
					base.OnPropertyChanged(value, "Rarity");
					this.OnRarityChanged();
				}
			}
		}

		// Token: 0x04000462 RID: 1122
		private const string BaseBrushName = "MPLobby.Armory.CosmeticButton";

		// Token: 0x04000463 RID: 1123
		private bool _isUsed;

		// Token: 0x04000464 RID: 1124
		private int _rarity;
	}
}
