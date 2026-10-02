using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015F RID: 351
	public class EncyclopediaListWidget : Widget
	{
		// Token: 0x060012B0 RID: 4784 RVA: 0x000339DA File Offset: 0x00031BDA
		public EncyclopediaListWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000339E4 File Offset: 0x00031BE4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isListSizeInitialized && this.ItemListScroll != null && this.ItemListScroll.Size.Y != 0f)
			{
				this._isListSizeInitialized = true;
				this._isDirty = true;
			}
			if (this._isDirty)
			{
				this._isDirty = false;
				this.UpdateScrollPosition();
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00033A44 File Offset: 0x00031C44
		private void UpdateScrollPosition()
		{
			if (!string.IsNullOrEmpty(this.LastSelectedItemId) && this.ItemList != null && this.ItemListScroll != null)
			{
				Widget firstInChildrenRecursive = this.ItemList.GetFirstInChildrenRecursive(delegate(Widget x)
				{
					EncyclopediaListItemButtonWidget encyclopediaListItemButtonWidget;
					return (encyclopediaListItemButtonWidget = x as EncyclopediaListItemButtonWidget) != null && encyclopediaListItemButtonWidget.ListItemId == this.LastSelectedItemId;
				});
				if (firstInChildrenRecursive != null && firstInChildrenRecursive.IsVisible)
				{
					float num = firstInChildrenRecursive.ScaledSuggestedHeight + firstInChildrenRecursive.ScaledMarginTop + firstInChildrenRecursive.ScaledMarginBottom - 2f * base._scaleToUse;
					int visibleSiblingIndex = firstInChildrenRecursive.GetVisibleSiblingIndex();
					float num2 = num * (float)visibleSiblingIndex - this.ItemListScroll.Size.Y / 2f;
					this.ItemListScroll.SetValueForced(num2);
				}
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00033AE3 File Offset: 0x00031CE3
		private void OnListItemAdded(Widget widget, string eventName, object[] eventArgs)
		{
			if (eventName == "ItemAdd" && eventArgs.Length != 0 && eventArgs[0] is EncyclopediaListItemButtonWidget)
			{
				this._isDirty = true;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x00033B07 File Offset: 0x00031D07
		// (set) Token: 0x060012B5 RID: 4789 RVA: 0x00033B0F File Offset: 0x00031D0F
		[Editor(false)]
		public string LastSelectedItemId
		{
			get
			{
				return this._lastSelectedItemId;
			}
			set
			{
				if (this._lastSelectedItemId != value)
				{
					this._lastSelectedItemId = value;
					base.OnPropertyChanged<string>(value, "LastSelectedItemId");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x00033B39 File Offset: 0x00031D39
		// (set) Token: 0x060012B7 RID: 4791 RVA: 0x00033B41 File Offset: 0x00031D41
		public ListPanel ItemList
		{
			get
			{
				return this._itemList;
			}
			set
			{
				if (this._itemList != value)
				{
					this._itemList = value;
					this._isDirty = true;
					this._itemList.EventFire += this.OnListItemAdded;
				}
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x00033B71 File Offset: 0x00031D71
		// (set) Token: 0x060012B9 RID: 4793 RVA: 0x00033B79 File Offset: 0x00031D79
		public ScrollbarWidget ItemListScroll
		{
			get
			{
				return this._itemListScroll;
			}
			set
			{
				if (this._itemListScroll != value)
				{
					this._itemListScroll = value;
					this._isDirty = true;
				}
			}
		}

		// Token: 0x04000886 RID: 2182
		private bool _isDirty;

		// Token: 0x04000887 RID: 2183
		private bool _isListSizeInitialized;

		// Token: 0x04000888 RID: 2184
		private string _lastSelectedItemId;

		// Token: 0x04000889 RID: 2185
		private ListPanel _itemList;

		// Token: 0x0400088A RID: 2186
		private ScrollbarWidget _itemListScroll;
	}
}
