using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A6 RID: 166
	public class MultiplayerLobbyGameTypeItemButtonWidget : ButtonWidget
	{
		// Token: 0x060008E7 RID: 2279 RVA: 0x00019B23 File Offset: 0x00017D23
		public MultiplayerLobbyGameTypeItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00019B2C File Offset: 0x00017D2C
		private void UpdateSprite()
		{
			base.Brush.DefaultLayer.Sprite = base.Context.SpriteData.GetSprite("MPLobby\\GameTypes\\" + this.GameTypeID);
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00019B5E File Offset: 0x00017D5E
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x00019B66 File Offset: 0x00017D66
		[Editor(false)]
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
					base.OnPropertyChanged<string>(value, "GameTypeID");
					this.UpdateSprite();
				}
			}
		}

		// Token: 0x04000408 RID: 1032
		private string _gameTypeID;
	}
}
