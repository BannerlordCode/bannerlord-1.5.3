using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010D RID: 269
	public class DevelopmentQueueVisualIconWidget : Widget
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x00027EEB File Offset: 0x000260EB
		public DevelopmentQueueVisualIconWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00027EFC File Offset: 0x000260FC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animState == DevelopmentQueueVisualIconWidget.AnimState.Start)
			{
				this._tickCount += 1f;
				if (this._tickCount > 20f)
				{
					this._animState = DevelopmentQueueVisualIconWidget.AnimState.Starting;
					return;
				}
			}
			else if (this._animState == DevelopmentQueueVisualIconWidget.AnimState.Starting)
			{
				BrushWidget inProgressIconWidget = this.InProgressIconWidget;
				if (inProgressIconWidget != null)
				{
					inProgressIconWidget.BrushRenderer.RestartAnimation();
				}
				this._animState = DevelopmentQueueVisualIconWidget.AnimState.Playing;
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00027F68 File Offset: 0x00026168
		private void UpdateVisual(int index)
		{
			if (this.InProgressIconWidget != null && this.QueueIconWidget != null)
			{
				base.IsVisible = index >= 0;
				this.InProgressIconWidget.IsVisible = index == 0;
				this._animState = (this.InProgressIconWidget.IsVisible ? DevelopmentQueueVisualIconWidget.AnimState.Start : DevelopmentQueueVisualIconWidget.AnimState.Idle);
				this._tickCount = 0f;
				this.QueueIconWidget.IsVisible = index > 0;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00027FD2 File Offset: 0x000261D2
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x00027FDA File Offset: 0x000261DA
		[Editor(false)]
		public int QueueIndex
		{
			get
			{
				return this._queueIndex;
			}
			set
			{
				if (this._queueIndex != value)
				{
					this._queueIndex = value;
					base.OnPropertyChanged(value, "QueueIndex");
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00027FFF File Offset: 0x000261FF
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x00028007 File Offset: 0x00026207
		[Editor(false)]
		public Widget QueueIconWidget
		{
			get
			{
				return this._queueIconWidget;
			}
			set
			{
				if (this._queueIconWidget != value)
				{
					this._queueIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "QueueIconWidget");
					this.UpdateVisual(this.QueueIndex);
				}
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00028031 File Offset: 0x00026231
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x00028039 File Offset: 0x00026239
		[Editor(false)]
		public BrushWidget InProgressIconWidget
		{
			get
			{
				return this._inProgressIconWidget;
			}
			set
			{
				if (this._inProgressIconWidget != value)
				{
					this._inProgressIconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "InProgressIconWidget");
					this.UpdateVisual(this.QueueIndex);
				}
			}
		}

		// Token: 0x0400068F RID: 1679
		private DevelopmentQueueVisualIconWidget.AnimState _animState;

		// Token: 0x04000690 RID: 1680
		private float _tickCount;

		// Token: 0x04000691 RID: 1681
		private int _queueIndex = -1;

		// Token: 0x04000692 RID: 1682
		private Widget _queueIconWidget;

		// Token: 0x04000693 RID: 1683
		private BrushWidget _inProgressIconWidget;

		// Token: 0x020001CA RID: 458
		public enum AnimState
		{
			// Token: 0x04000A4F RID: 2639
			Idle,
			// Token: 0x04000A50 RID: 2640
			Start,
			// Token: 0x04000A51 RID: 2641
			Starting,
			// Token: 0x04000A52 RID: 2642
			Playing
		}
	}
}
