using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016F RID: 367
	public class CraftingDifficultyBarParentWidget : Widget
	{
		// Token: 0x06001370 RID: 4976 RVA: 0x00035275 File Offset: 0x00033475
		public CraftingDifficultyBarParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00035289 File Offset: 0x00033489
		private void OnWidgetPositionUpdated(PropertyOwnerObject ownerObject, string propertyName, object value)
		{
			if (propertyName == "Text")
			{
				this._areOffsetsDirty = true;
			}
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x000352A0 File Offset: 0x000334A0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.SmithingLevelTextWidget != null && this.OrderDifficultyTextWidget != null)
			{
				if (this._updatePositions)
				{
					TextWidget textWidget = ((this.OrderDifficulty < this.SmithingLevel) ? this.SmithingLevelTextWidget : this.OrderDifficultyTextWidget);
					TextWidget textWidget2 = ((textWidget == this.SmithingLevelTextWidget) ? this.OrderDifficultyTextWidget : this.SmithingLevelTextWidget);
					if (textWidget.GlobalPosition.Y + (textWidget.Size.Y + this._offsetIntolerance) >= textWidget2.GlobalPosition.Y)
					{
						textWidget.PositionYOffset = -textWidget.Size.Y;
						textWidget2.PositionYOffset = 0f;
					}
					else
					{
						textWidget.PositionYOffset = 0f;
						textWidget2.PositionYOffset = 0f;
					}
					this._updatePositions = false;
				}
				if (this._areOffsetsDirty)
				{
					this.SmithingLevelTextWidget.PositionYOffset = 0f;
					this.OrderDifficultyTextWidget.PositionYOffset = 0f;
					this._updatePositions = true;
					this._areOffsetsDirty = false;
				}
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x000353A7 File Offset: 0x000335A7
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x000353AF File Offset: 0x000335AF
		public int OrderDifficulty { get; set; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x000353B8 File Offset: 0x000335B8
		// (set) Token: 0x06001376 RID: 4982 RVA: 0x000353C0 File Offset: 0x000335C0
		public int SmithingLevel { get; set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x000353C9 File Offset: 0x000335C9
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x000353D1 File Offset: 0x000335D1
		public TextWidget SmithingLevelTextWidget
		{
			get
			{
				return this._smithingLevelTextWidget;
			}
			set
			{
				if (value != this._smithingLevelTextWidget)
				{
					this._smithingLevelTextWidget = value;
					this._smithingLevelTextWidget.PropertyChanged += this.OnWidgetPositionUpdated;
				}
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x000353FA File Offset: 0x000335FA
		// (set) Token: 0x0600137A RID: 4986 RVA: 0x00035402 File Offset: 0x00033602
		public TextWidget OrderDifficultyTextWidget
		{
			get
			{
				return this._orderDifficultyTextWidget;
			}
			set
			{
				if (value != this._orderDifficultyTextWidget)
				{
					this._orderDifficultyTextWidget = value;
					this._orderDifficultyTextWidget.PropertyChanged += this.OnWidgetPositionUpdated;
				}
			}
		}

		// Token: 0x040008D9 RID: 2265
		private float _offsetIntolerance = 3f;

		// Token: 0x040008DA RID: 2266
		private bool _areOffsetsDirty;

		// Token: 0x040008DB RID: 2267
		private bool _updatePositions;

		// Token: 0x040008DE RID: 2270
		private TextWidget _smithingLevelTextWidget;

		// Token: 0x040008DF RID: 2271
		private TextWidget _orderDifficultyTextWidget;
	}
}
