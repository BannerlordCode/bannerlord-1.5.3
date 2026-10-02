using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000026 RID: 38
	public class IconBrushWidget : ButtonWidget
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x00007618 File Offset: 0x00005818
		public IconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007624 File Offset: 0x00005824
		private void UpdateIcon()
		{
			if (this.IconBrush == null || string.IsNullOrEmpty(this.IconID))
			{
				return;
			}
			BrushLayer layer = this.IconBrush.GetLayer(this.IconID);
			if (base.Brush != null)
			{
				Sprite sprite = ((layer != null) ? layer.Sprite : null);
				base.Brush.Sprite = sprite;
				if (sprite != null && this.UseIconSize)
				{
					base.SuggestedWidth = (float)sprite.Width;
					base.SuggestedHeight = (float)sprite.Height;
				}
				foreach (BrushLayer brushLayer in base.Brush.Layers)
				{
					if (this.UseStylesFromSourceIcon && layer != null)
					{
						brushLayer.FillFrom(layer);
					}
					else
					{
						brushLayer.Sprite = sprite;
					}
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00007704 File Offset: 0x00005904
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000770C File Offset: 0x0000590C
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007730 File Offset: 0x00005930
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00007738 File Offset: 0x00005938
		public string IconID
		{
			get
			{
				return this._iconId;
			}
			set
			{
				if (value != this._iconId)
				{
					this._iconId = value;
					base.OnPropertyChanged<string>(value, "IconID");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00007761 File Offset: 0x00005961
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00007769 File Offset: 0x00005969
		public bool UseStylesFromSourceIcon
		{
			get
			{
				return this._useStylesFromSourceIcon;
			}
			set
			{
				if (value != this._useStylesFromSourceIcon)
				{
					this._useStylesFromSourceIcon = value;
					base.OnPropertyChanged(value, "UseStylesFromSourceIcon");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000778D File Offset: 0x0000598D
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00007795 File Offset: 0x00005995
		public bool UseIconSize
		{
			get
			{
				return this._useIconSize;
			}
			set
			{
				if (value != this._useIconSize)
				{
					this._useIconSize = value;
					base.OnPropertyChanged(value, "UseIconSize");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x040000EC RID: 236
		private Brush _iconBrush;

		// Token: 0x040000ED RID: 237
		private string _iconId;

		// Token: 0x040000EE RID: 238
		private bool _useStylesFromSourceIcon;

		// Token: 0x040000EF RID: 239
		private bool _useIconSize;
	}
}
