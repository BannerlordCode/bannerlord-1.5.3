using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000AB RID: 171
	public class MultiplayerLobbyRankItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x00019F49 File Offset: 0x00018149
		public MultiplayerLobbyRankItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00019F54 File Offset: 0x00018154
		private void UpdateSprite()
		{
			string text = "unranked";
			if (this.RankID != string.Empty)
			{
				text = this.RankID;
			}
			base.Brush.DefaultLayer.Sprite = base.Context.SpriteData.GetSprite("MPGeneral\\MPRanks\\" + text);
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00019FAB File Offset: 0x000181AB
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00019FB3 File Offset: 0x000181B3
		[Editor(false)]
		public string RankID
		{
			get
			{
				return this._rankID;
			}
			set
			{
				if (value != this._rankID)
				{
					this._rankID = value;
					base.OnPropertyChanged<string>(value, "RankID");
					this.UpdateSprite();
				}
			}
		}

		// Token: 0x04000417 RID: 1047
		private const string _defaultRankID = "unranked";

		// Token: 0x04000418 RID: 1048
		private string _rankID;
	}
}
