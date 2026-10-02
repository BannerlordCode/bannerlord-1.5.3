using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000027 RID: 39
	public class IconOffsetButtonWidget : IconBrushWidget
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000077B9 File Offset: 0x000059B9
		// (set) Token: 0x06000201 RID: 513 RVA: 0x000077C1 File Offset: 0x000059C1
		public int NormalXOffset { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000077CA File Offset: 0x000059CA
		// (set) Token: 0x06000203 RID: 515 RVA: 0x000077D2 File Offset: 0x000059D2
		public int NormalYOffset { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000077DB File Offset: 0x000059DB
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000077E3 File Offset: 0x000059E3
		public int PressedXOffset { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000077EC File Offset: 0x000059EC
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000077F4 File Offset: 0x000059F4
		public int PressedYOffset { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000077FD File Offset: 0x000059FD
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00007805 File Offset: 0x00005A05
		public Widget ButtonIcon { get; set; }

		// Token: 0x0600020A RID: 522 RVA: 0x0000780E File Offset: 0x00005A0E
		public IconOffsetButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00007818 File Offset: 0x00005A18
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Brush iconBrush = base.IconBrush;
			BrushLayer brushLayer = ((iconBrush != null) ? iconBrush.GetLayer(base.IconID) : null);
			if (((brushLayer != null) ? brushLayer.Sprite : null) != null)
			{
				base.SuggestedWidth = (float)brushLayer.Sprite.Width;
				base.SuggestedHeight = (float)brushLayer.Sprite.Height;
			}
			if (this.ButtonIcon != null)
			{
				if (base.IsPressed || base.IsSelected)
				{
					this.ButtonIcon.PositionYOffset = (float)this.PressedYOffset;
					this.ButtonIcon.PositionXOffset = (float)this.PressedXOffset;
					return;
				}
				this.ButtonIcon.PositionYOffset = (float)this.NormalYOffset;
				this.ButtonIcon.PositionXOffset = (float)this.NormalXOffset;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000078D8 File Offset: 0x00005AD8
		protected override void RefreshState()
		{
			if (base.IsSelected)
			{
				this.SetState("Selected");
				return;
			}
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			if (base.IsPressed)
			{
				this.SetState("Pressed");
				return;
			}
			if (base.IsHovered)
			{
				this.SetState("Hovered");
				return;
			}
			this.SetState("Default");
		}
	}
}
