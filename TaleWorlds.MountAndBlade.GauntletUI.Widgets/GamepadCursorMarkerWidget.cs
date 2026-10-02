using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000021 RID: 33
	public class GamepadCursorMarkerWidget : BrushWidget
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00006C62 File Offset: 0x00004E62
		public GamepadCursorMarkerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00006C6B File Offset: 0x00004E6B
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00006C73 File Offset: 0x00004E73
		public bool FlipVisual
		{
			get
			{
				return this._flipVisual;
			}
			set
			{
				if (value != this._flipVisual)
				{
					this._flipVisual = value;
					base.Brush.DefaultLayer.HorizontalFlip = value;
				}
			}
		}

		// Token: 0x040000C8 RID: 200
		private bool _flipVisual;
	}
}
