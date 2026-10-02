using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000019 RID: 25
	public class DropdownButtonWidget : ButtonWidget
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00005AFB File Offset: 0x00003CFB
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00005B04 File Offset: 0x00003D04
		public Widget DisplayedList
		{
			get
			{
				return this._displayedList;
			}
			set
			{
				if (value != this._displayedList)
				{
					if (this._displayedList != null)
					{
						ListPanel listPanel = this._displayedList.GetFirstInChildrenAndThisRecursive((Widget x) => x is ListPanel) as ListPanel;
						if (listPanel != null)
						{
							listPanel.SelectEventHandlers.Remove(new Action<Widget>(this.OnListItemSelected));
						}
					}
					this._displayedList = value;
					this._displayedList.IsVisible = false;
					this._isDisplayingList = false;
					ListPanel listPanel2 = this._displayedList.GetFirstInChildrenAndThisRecursive((Widget x) => x is ListPanel) as ListPanel;
					if (listPanel2 != null)
					{
						listPanel2.SelectEventHandlers.Add(new Action<Widget>(this.OnListItemSelected));
					}
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005BD4 File Offset: 0x00003DD4
		public DropdownButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005BDD File Offset: 0x00003DDD
		private void OnListItemSelected(Widget list)
		{
			this.HideList();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005BE8 File Offset: 0x00003DE8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isDisplayingList)
			{
				this.DisplayedList.ScaledPositionXOffset = Mathf.Clamp(base.GlobalPosition.X, 0f, base.EventManager.Root.Size.X * base._inverseScaleToUse - this.DisplayedList.Size.X);
				this.DisplayedList.ScaledPositionYOffset = Mathf.Clamp(base.GlobalPosition.Y + base.Size.Y, 0f, base.EventManager.Root.Size.Y * base._inverseScaleToUse - this.DisplayedList.Size.Y);
				if (base.EventManager.LatestMouseUpWidget == null)
				{
					this.HideList();
					return;
				}
				if (base.EventManager.LatestMouseUpWidget != this && !this.DisplayedList.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget))
				{
					this.HideList();
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005CF0 File Offset: 0x00003EF0
		private void DisplayList()
		{
			this.DisplayedList.ParentWidget = base.EventManager.Root;
			this.DisplayedList.IsVisible = true;
			this.DisplayedList.HorizontalAlignment = HorizontalAlignment.Left;
			this.DisplayedList.VerticalAlignment = VerticalAlignment.Top;
			this._isDisplayingList = true;
			base.DoNotUseCustomScaleAndChildren = false;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005D48 File Offset: 0x00003F48
		private void HideList()
		{
			this.DisplayedList.ParentWidget = this;
			this.DisplayedList.IsVisible = false;
			this.DisplayedList.PositionXOffset = 0f;
			this.DisplayedList.PositionYOffset = 0f;
			this._isDisplayingList = false;
			base.DoNotUseCustomScaleAndChildren = true;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005D9B File Offset: 0x00003F9B
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.DisplayedList != null)
			{
				if (!this._isDisplayingList)
				{
					this.DisplayList();
					return;
				}
				this.HideList();
			}
		}

		// Token: 0x040000A1 RID: 161
		private Widget _displayedList;

		// Token: 0x040000A2 RID: 162
		private bool _isDisplayingList;
	}
}
