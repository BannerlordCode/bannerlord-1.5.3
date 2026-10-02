using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000097 RID: 151
	public class MultiplayerScoreboardStripedBackgroundWidget : MultiplayerScoreboardStatsListPanel
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x00017FD7 File Offset: 0x000161D7
		public MultiplayerScoreboardStripedBackgroundWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00017FE0 File Offset: 0x000161E0
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			if (base.ChildCount % 2 == 1)
			{
				child.Sprite = base.Context.SpriteData.GetSprite("BlankWhiteSquare_9");
				child.Color = Color.ConvertStringToColor("#000000FF");
				child.AlphaFactor = 0.2f;
			}
		}
	}
}
