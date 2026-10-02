using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000070 RID: 112
	public class OrderCircleActionSelectorParentWidget : Widget
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x00012000 File Offset: 0x00010200
		public OrderCircleActionSelectorParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00012009 File Offset: 0x00010209
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00012012 File Offset: 0x00010212
		private void UpdateInputRestrictions()
		{
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x00012014 File Offset: 0x00010214
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x0001201C File Offset: 0x0001021C
		[Editor(false)]
		public bool IsInFreeCameraMode
		{
			get
			{
				return this._isInFreeCameraMode;
			}
			set
			{
				if (value != this._isInFreeCameraMode)
				{
					this._isInFreeCameraMode = value;
					base.OnPropertyChanged(value, "IsInFreeCameraMode");
					this.UpdateInputRestrictions();
				}
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00012040 File Offset: 0x00010240
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00012048 File Offset: 0x00010248
		public CircleActionSelectorWidget CircleActionSelectorWidget
		{
			get
			{
				return this._circleActionSelectorWidget;
			}
			set
			{
				if (value != this._circleActionSelectorWidget)
				{
					this._circleActionSelectorWidget = value;
					base.OnPropertyChanged<CircleActionSelectorWidget>(value, "CircleActionSelectorWidget");
					this.UpdateInputRestrictions();
				}
			}
		}

		// Token: 0x0400029A RID: 666
		private bool _isInFreeCameraMode;

		// Token: 0x0400029B RID: 667
		private CircleActionSelectorWidget _circleActionSelectorWidget;
	}
}
