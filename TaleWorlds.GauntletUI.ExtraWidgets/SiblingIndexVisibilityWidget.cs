using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000012 RID: 18
	public class SiblingIndexVisibilityWidget : Widget
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006723 File Offset: 0x00004923
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000672B File Offset: 0x0000492B
		public SiblingIndexVisibilityWidget.WatchTypes WatchType { get; set; }

		// Token: 0x06000119 RID: 281 RVA: 0x00006734 File Offset: 0x00004934
		public SiblingIndexVisibilityWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000673D File Offset: 0x0000493D
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateVisibility();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000674C File Offset: 0x0000494C
		private void UpdateVisibility()
		{
			Widget widget = this.WidgetToWatch ?? this;
			if (((widget != null) ? widget.ParentWidget : null) != null)
			{
				switch (this.WatchType)
				{
				case SiblingIndexVisibilityWidget.WatchTypes.Equal:
					base.IsVisible = widget.GetSiblingIndex() == this.IndexToBeVisible;
					return;
				case SiblingIndexVisibilityWidget.WatchTypes.BiggerThan:
					base.IsVisible = widget.GetSiblingIndex() > this.IndexToBeVisible;
					return;
				case SiblingIndexVisibilityWidget.WatchTypes.BiggerThanEqual:
					base.IsVisible = widget.GetSiblingIndex() >= this.IndexToBeVisible;
					return;
				case SiblingIndexVisibilityWidget.WatchTypes.LessThan:
					base.IsVisible = widget.GetSiblingIndex() < this.IndexToBeVisible;
					return;
				case SiblingIndexVisibilityWidget.WatchTypes.LessThanEqual:
					base.IsVisible = widget.GetSiblingIndex() <= this.IndexToBeVisible;
					return;
				case SiblingIndexVisibilityWidget.WatchTypes.Odd:
					base.IsVisible = widget.GetSiblingIndex() % 2 == 1;
					return;
				case SiblingIndexVisibilityWidget.WatchTypes.Even:
					base.IsVisible = widget.GetSiblingIndex() % 2 == 0;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006832 File Offset: 0x00004A32
		private void OnWidgetToWatchParentEventFired(Widget arg1, string arg2, object[] arg3)
		{
			if (arg2 == "ItemAdd" || arg2 == "ItemRemove")
			{
				this.UpdateVisibility();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00006854 File Offset: 0x00004A54
		// (set) Token: 0x0600011E RID: 286 RVA: 0x0000685C File Offset: 0x00004A5C
		[Editor(false)]
		public int IndexToBeVisible
		{
			get
			{
				return this._indexToBeVisible;
			}
			set
			{
				if (this._indexToBeVisible != value)
				{
					this._indexToBeVisible = value;
					base.OnPropertyChanged(value, "IndexToBeVisible");
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000687A File Offset: 0x00004A7A
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00006882 File Offset: 0x00004A82
		[Editor(false)]
		public Widget WidgetToWatch
		{
			get
			{
				return this._widgetToWatch;
			}
			set
			{
				if (this._widgetToWatch != value)
				{
					this._widgetToWatch = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToWatch");
					value.ParentWidget.EventFire += this.OnWidgetToWatchParentEventFired;
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x04000085 RID: 133
		private Widget _widgetToWatch;

		// Token: 0x04000086 RID: 134
		private int _indexToBeVisible;

		// Token: 0x02000020 RID: 32
		public enum WatchTypes
		{
			// Token: 0x040000CB RID: 203
			Equal,
			// Token: 0x040000CC RID: 204
			BiggerThan,
			// Token: 0x040000CD RID: 205
			BiggerThanEqual,
			// Token: 0x040000CE RID: 206
			LessThan,
			// Token: 0x040000CF RID: 207
			LessThanEqual,
			// Token: 0x040000D0 RID: 208
			Odd,
			// Token: 0x040000D1 RID: 209
			Even
		}
	}
}
