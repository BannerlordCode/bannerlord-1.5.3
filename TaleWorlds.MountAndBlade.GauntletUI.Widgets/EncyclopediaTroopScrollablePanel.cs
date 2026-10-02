using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001B RID: 27
	public class EncyclopediaTroopScrollablePanel : ScrollablePanel
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00005E66 File Offset: 0x00004066
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00005E6E File Offset: 0x0000406E
		public bool PanWithMouseEnabled { get; set; }

		// Token: 0x06000165 RID: 357 RVA: 0x00005E77 File Offset: 0x00004077
		public EncyclopediaTroopScrollablePanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005E80 File Offset: 0x00004080
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.PanWithMouseEnabled)
			{
				bool flag = this.IsMouseOverWidget(this);
				if (flag)
				{
					List<Widget> allChildrenAndThisRecursive = base.GetAllChildrenAndThisRecursive();
					for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
					{
						if (this.IsMouseOverWidget(allChildrenAndThisRecursive[i]) && allChildrenAndThisRecursive[i] is ButtonWidget)
						{
							flag = false;
						}
					}
				}
				if (flag && base.HorizontalScrollbar != null && this._canScrollHorizontal)
				{
					base.SetActiveCursor(UIContext.MouseCursors.Move);
					if (Input.IsKeyPressed(InputKey.LeftMouseButton))
					{
						this._isDragging = true;
					}
				}
			}
			if (Input.IsKeyReleased(InputKey.LeftMouseButton))
			{
				this._isDragging = false;
			}
			if (this._isDragging)
			{
				base.HorizontalScrollbar.ValueFloat -= Input.MouseMoveX;
				base.VerticalScrollbar.ValueFloat -= Input.MouseMoveY;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005F54 File Offset: 0x00004154
		private bool IsMouseOverWidget(Widget widget)
		{
			return widget.GlobalPosition.X <= Input.MousePositionPixel.X && Input.MousePositionPixel.X <= widget.GlobalPosition.X + widget.Size.X && widget.GlobalPosition.Y <= Input.MousePositionPixel.Y && Input.MousePositionPixel.Y <= widget.GlobalPosition.Y + widget.Size.Y;
		}

		// Token: 0x040000A4 RID: 164
		private bool _isDragging;
	}
}
