using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005B RID: 91
	public class ImageWidget : BrushWidget
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001ADAB File Offset: 0x00018FAB
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x0001ADB3 File Offset: 0x00018FB3
		public bool OverrideDefaultStateSwitchingEnabled { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001ADBC File Offset: 0x00018FBC
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0001ADC7 File Offset: 0x00018FC7
		public bool OverrideDefaultStateSwitchingDisabled
		{
			get
			{
				return !this.OverrideDefaultStateSwitchingEnabled;
			}
			set
			{
				if (value != !this.OverrideDefaultStateSwitchingEnabled)
				{
					this.OverrideDefaultStateSwitchingEnabled = !value;
				}
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001ADDF File Offset: 0x00018FDF
		public ImageWidget(UIContext context)
			: base(context)
		{
			base.AddState("Pressed");
			base.AddState("Hovered");
			base.AddState("Disabled");
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001AE0C File Offset: 0x0001900C
		protected override void RefreshState()
		{
			if (!this.OverrideDefaultStateSwitchingEnabled)
			{
				if (base.IsDisabled)
				{
					this.SetState("Disabled");
				}
				else if (base.IsPressed)
				{
					this.SetState("Pressed");
				}
				else if (base.IsHovered)
				{
					this.SetState("Hovered");
				}
				else
				{
					this.SetState("Default");
				}
			}
			base.RefreshState();
		}
	}
}
