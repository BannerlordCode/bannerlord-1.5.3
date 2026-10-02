using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Credits
{
	// Token: 0x02000165 RID: 357
	public class CreditsWidget : Widget
	{
		// Token: 0x060012EE RID: 4846 RVA: 0x0003441F File Offset: 0x0003261F
		public CreditsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00034454 File Offset: 0x00032654
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.RootItemWidget != null)
			{
				this.RootItemWidget.PositionYOffset = this._currentOffset;
				if (this._doNotScrollTimer > 0f)
				{
					this._doNotScrollTimer -= dt;
				}
				else
				{
					this._targetOffset -= dt * this.ScrollPixelsPerSecond;
				}
				this._currentOffset = MathF.Lerp(this._currentOffset, this._targetOffset, MathF.Min(1f, dt * 10f), 1E-05f);
				if (this._currentOffset < -this.RootItemWidget.Size.Y * base._inverseScaleToUse)
				{
					this._currentOffset = 1080f;
					this._targetOffset = 1080f;
				}
			}
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00034519 File Offset: 0x00032719
		protected override bool OnPreviewMouseScroll()
		{
			return true;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0003451C File Offset: 0x0003271C
		protected override bool OnPreviewRightStickMovement()
		{
			return true;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0003451F File Offset: 0x0003271F
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			this.OnScroll(base.EventManager.DeltaMouseScroll * 0.5f);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0003453E File Offset: 0x0003273E
		protected override void OnRightStickMovement()
		{
			base.OnRightStickMovement();
			this.OnScroll(base.EventManager.RightStickVerticalScrollAmount);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00034558 File Offset: 0x00032758
		private void OnScroll(float scrollAmount)
		{
			if (this._targetOffset <= 0f || scrollAmount <= 0f)
			{
				this._targetOffset += scrollAmount;
				this._targetOffset = MathF.Min(this._targetOffset, 0f);
			}
			this._doNotScrollTimer = this.ManualScrollWaitTimer;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x000345AA File Offset: 0x000327AA
		// (set) Token: 0x060012F6 RID: 4854 RVA: 0x000345B2 File Offset: 0x000327B2
		[Editor(false)]
		public Widget RootItemWidget
		{
			get
			{
				return this._rootItemWidget;
			}
			set
			{
				if (this._rootItemWidget != value)
				{
					this._rootItemWidget = value;
					base.OnPropertyChanged<Widget>(value, "RootItemWidget");
				}
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x000345D0 File Offset: 0x000327D0
		// (set) Token: 0x060012F8 RID: 4856 RVA: 0x000345D8 File Offset: 0x000327D8
		[Editor(false)]
		public float ScrollPixelsPerSecond
		{
			get
			{
				return this._scrollPixelsPerSecond;
			}
			set
			{
				if (this._scrollPixelsPerSecond != value)
				{
					this._scrollPixelsPerSecond = value;
					base.OnPropertyChanged(value, "ScrollPixelsPerSecond");
				}
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x000345F6 File Offset: 0x000327F6
		// (set) Token: 0x060012FA RID: 4858 RVA: 0x000345FE File Offset: 0x000327FE
		[Editor(false)]
		public float ManualScrollWaitTimer
		{
			get
			{
				return this._manualScrollWaitTimer;
			}
			set
			{
				if (this._manualScrollWaitTimer != value)
				{
					this._manualScrollWaitTimer = value;
					base.OnPropertyChanged(value, "ManualScrollWaitTimer");
				}
			}
		}

		// Token: 0x040008A1 RID: 2209
		private float _currentOffset = 1080f;

		// Token: 0x040008A2 RID: 2210
		private float _targetOffset = 1080f;

		// Token: 0x040008A3 RID: 2211
		private float _doNotScrollTimer;

		// Token: 0x040008A4 RID: 2212
		private Widget _rootItemWidget;

		// Token: 0x040008A5 RID: 2213
		private float _scrollPixelsPerSecond = 75f;

		// Token: 0x040008A6 RID: 2214
		private float _manualScrollWaitTimer = 1f;
	}
}
