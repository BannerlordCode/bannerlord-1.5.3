using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Notification
{
	// Token: 0x02000124 RID: 292
	public class MapNotificationContainerWidget : Widget
	{
		// Token: 0x06000F7E RID: 3966 RVA: 0x0002AD28 File Offset: 0x00028F28
		public MapNotificationContainerWidget(UIContext context)
			: base(context)
		{
			this._newChildren = new List<Widget>();
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0002AD44 File Offset: 0x00028F44
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._newChildren.Count > 0)
			{
				foreach (Widget widget in this._newChildren)
				{
					widget.PositionYOffset = this.DetermineChildTargetYOffset(widget, base.GetChildIndex(widget));
				}
				this.DetermineChildrenVisibility();
				this.DetermineMoreTextStatus();
				this.DetermineNavigationIndicies();
				this._newChildren.Clear();
			}
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				if (i < this.MaxAmountOfNotificationsToShow)
				{
					float num = this.DetermineChildTargetYOffset(child, i);
					child.PositionYOffset = this.LocalLerp(child.PositionYOffset, num, dt * 18f);
				}
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0002AE20 File Offset: 0x00029020
		private void DetermineNavigationIndicies()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				MapNotificationItemWidget mapNotificationItemWidget = base.GetChild(i) as MapNotificationItemWidget;
				if (i < this.MaxAmountOfNotificationsToShow)
				{
					mapNotificationItemWidget.NotificationRingWidget.GamepadNavigationIndex = base.ChildCount - 1 - i;
				}
				else
				{
					mapNotificationItemWidget.NotificationRingWidget.GamepadNavigationIndex = -1;
				}
			}
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0002AE77 File Offset: 0x00029077
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this._newChildren.Add(child);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0002AE8C File Offset: 0x0002908C
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			if (this._newChildren.Contains(child))
			{
				this._newChildren.Remove(child);
			}
			this.DetermineChildrenVisibility();
			this.DetermineMoreTextStatus();
			this.DetermineNavigationIndicies();
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0002AEC4 File Offset: 0x000290C4
		private void DetermineChildrenVisibility()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				bool isVisible = child.IsVisible;
				child.IsVisible = i < this.MaxAmountOfNotificationsToShow;
				if (!isVisible)
				{
					child.PositionYOffset = this.DetermineChildTargetYOffset(child, i);
				}
			}
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0002AF10 File Offset: 0x00029110
		private void DetermineMoreTextStatus()
		{
			this.MoreTextWidgetContainer.IsVisible = base.ChildCount > this.MaxAmountOfNotificationsToShow;
			if (this.MoreTextWidgetContainer.IsVisible)
			{
				this.MoreTextWidget.Text = "+" + (base.ChildCount - this.MaxAmountOfNotificationsToShow);
				this.MoreTextWidgetContainer.BrushRenderer.RestartAnimation();
				this.MoreTextWidget.BrushRenderer.RestartAnimation();
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0002AF8A File Offset: 0x0002918A
		private float DetermineChildTargetYOffset(Widget child, int childIndex)
		{
			if (childIndex < this.MaxAmountOfNotificationsToShow)
			{
				return -child.Size.Y * (float)childIndex * base._inverseScaleToUse;
			}
			return -child.Size.Y * (float)this.MaxAmountOfNotificationsToShow * base._inverseScaleToUse;
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0002AFC7 File Offset: 0x000291C7
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0002AFCF File Offset: 0x000291CF
		[Editor(false)]
		public BrushWidget MoreTextWidgetContainer
		{
			get
			{
				return this._moreTextWidgetContainer;
			}
			set
			{
				if (this._moreTextWidgetContainer != value)
				{
					this._moreTextWidgetContainer = value;
					base.OnPropertyChanged<BrushWidget>(value, "MoreTextWidgetContainer");
				}
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0002AFED File Offset: 0x000291ED
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x0002AFF5 File Offset: 0x000291F5
		[Editor(false)]
		public TextWidget MoreTextWidget
		{
			get
			{
				return this._moreTextWidget;
			}
			set
			{
				if (this._moreTextWidget != value)
				{
					this._moreTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "MoreTextWidget");
				}
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0002B013 File Offset: 0x00029213
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x0002B01B File Offset: 0x0002921B
		[Editor(false)]
		public int MaxAmountOfNotificationsToShow
		{
			get
			{
				return this._maxAmountOfNotificationsToShow;
			}
			set
			{
				if (this._maxAmountOfNotificationsToShow != value)
				{
					this._maxAmountOfNotificationsToShow = value;
					base.OnPropertyChanged(value, "MaxAmountOfNotificationsToShow");
				}
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0002B039 File Offset: 0x00029239
		private float LocalLerp(float start, float end, float delta)
		{
			if (MathF.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x04000716 RID: 1814
		private List<Widget> _newChildren;

		// Token: 0x04000717 RID: 1815
		private TextWidget _moreTextWidget;

		// Token: 0x04000718 RID: 1816
		private BrushWidget _moreTextWidgetContainer;

		// Token: 0x04000719 RID: 1817
		private int _maxAmountOfNotificationsToShow = 5;
	}
}
