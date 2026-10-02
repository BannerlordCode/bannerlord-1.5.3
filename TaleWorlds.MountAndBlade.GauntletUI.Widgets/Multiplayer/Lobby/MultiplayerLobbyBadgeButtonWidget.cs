using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009E RID: 158
	public class MultiplayerLobbyBadgeButtonWidget : ButtonWidget
	{
		// Token: 0x06000896 RID: 2198 RVA: 0x00018D17 File Offset: 0x00016F17
		public MultiplayerLobbyBadgeButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00018D20 File Offset: 0x00016F20
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.EventManager.HoveredWidget == this && Input.IsKeyPressed(InputKey.ControllerRUp))
			{
				this.OnMouseAlternatePressed();
				return;
			}
			if (base.EventManager.HoveredWidget == this && Input.IsKeyReleased(InputKey.ControllerRUp))
			{
				this.OnMouseAlternateReleased(true);
			}
		}
	}
}
