using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Perks
{
	// Token: 0x0200009A RID: 154
	public class MultiplayerPerkPopupWidget : Widget
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00018429 File Offset: 0x00016629
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00018431 File Offset: 0x00016631
		public bool ShowAboveContainer { get; set; }

		// Token: 0x06000861 RID: 2145 RVA: 0x0001843A File Offset: 0x0001663A
		public MultiplayerPerkPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00018443 File Offset: 0x00016643
		public void SetPopupPerksContainer(MultiplayerPerkContainerPanelWidget container)
		{
			this._latestContainer = container;
			base.ApplyActionToAllChildrenRecursive(new Action<Widget>(this.SetContainersOfChildren));
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00018460 File Offset: 0x00016660
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._latestContainer != null)
			{
				float num = this._latestContainer.GlobalPosition.X - (base.Size.X / 2f - this._latestContainer.Size.X / 2f);
				base.ScaledPositionXOffset = Mathf.Clamp(num, 0f, base.Context.EventManager.PageSize.X - base.Size.X);
				if (!this.ShowAboveContainer)
				{
					base.ScaledPositionYOffset = this._latestContainer.GlobalPosition.Y + this._latestContainer.Size.Y - base.EventManager.TopUsableAreaStart;
					return;
				}
				base.ScaledPositionYOffset = this._latestContainer.GlobalPosition.Y - base.Size.Y - base.EventManager.TopUsableAreaStart;
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00018560 File Offset: 0x00016760
		private void SetContainersOfChildren(Widget obj)
		{
			MultiplayerPerkItemToggleWidget multiplayerPerkItemToggleWidget;
			if ((multiplayerPerkItemToggleWidget = obj as MultiplayerPerkItemToggleWidget) != null)
			{
				multiplayerPerkItemToggleWidget.ContainerPanel = this._latestContainer;
			}
		}

		// Token: 0x040003BA RID: 954
		private MultiplayerPerkContainerPanelWidget _latestContainer;
	}
}
