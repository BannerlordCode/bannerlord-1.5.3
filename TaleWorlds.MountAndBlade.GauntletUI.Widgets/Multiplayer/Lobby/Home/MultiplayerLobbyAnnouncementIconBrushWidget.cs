using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Home
{
	// Token: 0x020000AF RID: 175
	public class MultiplayerLobbyAnnouncementIconBrushWidget : BrushWidget
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x0001A7EC File Offset: 0x000189EC
		public MultiplayerLobbyAnnouncementIconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001A7F8 File Offset: 0x000189F8
		private void UpdateIcon()
		{
			if (this.AnnouncementType == null)
			{
				return;
			}
			Brush iconBrush = this.IconBrush;
			Sprite sprite;
			if (iconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = iconBrush.GetLayer(this.AnnouncementType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			Sprite sprite2 = sprite;
			if (base.Brush != null)
			{
				base.Brush.Sprite = sprite2;
				foreach (BrushLayer brushLayer in base.Brush.Layers)
				{
					brushLayer.Sprite = sprite2;
				}
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0001A890 File Offset: 0x00018A90
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0001A898 File Offset: 0x00018A98
		public string AnnouncementType
		{
			get
			{
				return this._announcementType;
			}
			set
			{
				if (value != this._announcementType)
				{
					this._announcementType = value;
					base.OnPropertyChanged<string>(value, "AnnouncementType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0001A8C1 File Offset: 0x00018AC1
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0001A8C9 File Offset: 0x00018AC9
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (value != this._iconBrush)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x04000435 RID: 1077
		private string _announcementType;

		// Token: 0x04000436 RID: 1078
		private Brush _iconBrush;
	}
}
