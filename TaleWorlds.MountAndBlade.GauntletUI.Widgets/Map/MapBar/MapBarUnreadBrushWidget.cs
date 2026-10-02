using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012F RID: 303
	public class MapBarUnreadBrushWidget : BrushWidget
	{
		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0002C366 File Offset: 0x0002A566
		// (set) Token: 0x06000FF4 RID: 4084 RVA: 0x0002C36E File Offset: 0x0002A56E
		public bool IsBannerNotification { get; set; }

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0002C377 File Offset: 0x0002A577
		public MapBarUnreadBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0002C380 File Offset: 0x0002A580
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._animState == MapBarUnreadBrushWidget.AnimState.Idle)
			{
				this._animState = MapBarUnreadBrushWidget.AnimState.Start;
			}
			if (this._animState == MapBarUnreadBrushWidget.AnimState.Start)
			{
				this._animState = MapBarUnreadBrushWidget.AnimState.FirstFrame;
			}
			else if (this._animState == MapBarUnreadBrushWidget.AnimState.FirstFrame)
			{
				if (base.BrushRenderer.Brush == null)
				{
					this._animState = MapBarUnreadBrushWidget.AnimState.Start;
				}
				else
				{
					this._animState = MapBarUnreadBrushWidget.AnimState.Playing;
					base.BrushRenderer.RestartAnimation();
				}
			}
			if (this.IsBannerNotification && base.IsVisible && this._animState == MapBarUnreadBrushWidget.AnimState.Idle)
			{
				this._animState = MapBarUnreadBrushWidget.AnimState.Start;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0002C40D File Offset: 0x0002A60D
		// (set) Token: 0x06000FF8 RID: 4088 RVA: 0x0002C415 File Offset: 0x0002A615
		[Editor(false)]
		public TextWidget UnreadTextWidget
		{
			get
			{
				return this._unreadTextWidget;
			}
			set
			{
				if (this._unreadTextWidget != value)
				{
					this._unreadTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "UnreadTextWidget");
					if (value != null)
					{
						value.boolPropertyChanged += this.UnreadTextWidgetOnPropertyChanged;
					}
				}
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0002C448 File Offset: 0x0002A648
		private void UnreadTextWidgetOnPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsVisible")
			{
				base.IsVisible = propertyValue;
				this._animState = (base.IsVisible ? MapBarUnreadBrushWidget.AnimState.Start : MapBarUnreadBrushWidget.AnimState.Idle);
			}
		}

		// Token: 0x04000749 RID: 1865
		private MapBarUnreadBrushWidget.AnimState _animState;

		// Token: 0x0400074A RID: 1866
		private TextWidget _unreadTextWidget;

		// Token: 0x020001CD RID: 461
		public enum AnimState
		{
			// Token: 0x04000A5B RID: 2651
			Idle,
			// Token: 0x04000A5C RID: 2652
			Start,
			// Token: 0x04000A5D RID: 2653
			FirstFrame,
			// Token: 0x04000A5E RID: 2654
			Playing
		}
	}
}
