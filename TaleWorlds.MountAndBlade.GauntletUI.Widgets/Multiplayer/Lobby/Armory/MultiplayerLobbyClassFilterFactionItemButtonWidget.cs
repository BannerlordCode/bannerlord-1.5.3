using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BC RID: 188
	public class MultiplayerLobbyClassFilterFactionItemButtonWidget : ButtonWidget
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x0001BFEA File Offset: 0x0001A1EA
		public MultiplayerLobbyClassFilterFactionItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001C000 File Offset: 0x0001A200
		private void OnCultureChanged()
		{
			if (this.Culture == null)
			{
				return;
			}
			string text = this.BaseBrushName + "." + this.Culture[0].ToString().ToUpper() + this.Culture.Substring(1).ToLower();
			base.Brush = base.Context.BrushFactory.GetBrush(text);
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0001C068 File Offset: 0x0001A268
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x0001C070 File Offset: 0x0001A270
		[Editor(false)]
		public string BaseBrushName
		{
			get
			{
				return this._baseBrushName;
			}
			set
			{
				if (value != this._baseBrushName)
				{
					this._baseBrushName = value;
					base.OnPropertyChanged<string>(value, "BaseBrushName");
					this.OnCultureChanged();
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0001C099 File Offset: 0x0001A299
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0001C0A1 File Offset: 0x0001A2A1
		[Editor(false)]
		public string Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (this._culture != value)
				{
					this._culture = value;
					base.OnPropertyChanged<string>(value, "Culture");
					this.OnCultureChanged();
				}
			}
		}

		// Token: 0x0400047C RID: 1148
		private string _baseBrushName = "MPLobby.ClassFilter.FactionButton";

		// Token: 0x0400047D RID: 1149
		private string _culture;
	}
}
