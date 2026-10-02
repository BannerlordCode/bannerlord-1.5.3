using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000072 RID: 114
	public class OrderSiegeDeploymentItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x0001213B File Offset: 0x0001033B
		public OrderSiegeDeploymentItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001214C File Offset: 0x0001034C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.IsEnabled = this.IsPlayerGeneral && this.PointType != 2;
			if (this.preSelectedState != base.IsSelected)
			{
				if (base.IsSelected)
				{
					this.ScreenWidget.SetSelectedDeploymentItem(this);
				}
				this.preSelectedState = base.IsSelected;
			}
			if (this._isVisualsDirty)
			{
				this.UpdateTypeVisuals();
				this._isVisualsDirty = false;
			}
			this.UpdateScreenPosition();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000121C8 File Offset: 0x000103C8
		private void UpdateScreenPosition()
		{
			float num = this.Position.X - base.Size.X / 2f;
			float num2 = this.Position.X + base.Size.X / 2f;
			float num3 = this.Position.Y - base.Size.Y / 2f;
			float num4 = this.Position.Y + base.Size.Y / 2f;
			bool flag = this.IsInFront && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
			bool flag2 = this.IsInFront && (num2 > 0f || num < base.Context.EventManager.PageSize.X) && (num4 > 0f || num3 < base.Context.EventManager.PageSize.Y);
			if (!flag && base.IsSelected)
			{
				base.IsVisible = true;
				Vec2 vec = new Vec2(num, num3);
				Vector2 vector = base.Context.EventManager.PageSize - base.Size;
				Vec2 vec2 = vector / 2f;
				vec -= vec2;
				if (!this.IsInFront)
				{
					vec *= -1f;
				}
				float num5 = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
				float num6 = Mathf.Cos(num5);
				float num7 = Mathf.Sin(num5);
				float num8 = num6 / num7;
				Vec2 vec3 = vec2 * 1f;
				vec = ((num6 > 0f) ? new Vec2(-vec3.y / num8, vec2.y) : new Vec2(vec3.y / num8, -vec2.y));
				if (vec.x > vec3.x)
				{
					vec = new Vec2(vec3.x, -vec3.x * num8);
				}
				else if (vec.x < -vec3.x)
				{
					vec = new Vec2(-vec3.x, vec3.x * num8);
				}
				vec += vec2;
				base.ScaledPositionXOffset = Mathf.Clamp(vec.x, 0f, vector.X);
				base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, vector.Y);
				return;
			}
			if (flag || flag2)
			{
				base.IsVisible = true;
				base.ScaledPositionXOffset = num;
				base.ScaledPositionYOffset = num3;
				return;
			}
			base.IsVisible = false;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000124A8 File Offset: 0x000106A8
		private void UpdateTypeVisuals()
		{
			this.TypeIconWidget.RegisterBrushStatesOfWidget();
			this.BreachedTextWidget.IsVisible = this.PointType == 2;
			this.TypeIconWidget.IsVisible = this.PointType != 2;
			if (this.PointType == 0)
			{
				this.TypeIconWidget.SetState("BatteringRam");
				return;
			}
			if (this.PointType == 1)
			{
				this.TypeIconWidget.SetState("TowerLadder");
				return;
			}
			if (this.PointType == 2)
			{
				this.TypeIconWidget.SetState("Breach");
				return;
			}
			if (this.PointType == 3)
			{
				this.TypeIconWidget.SetState("Ranged");
				return;
			}
			this.TypeIconWidget.SetState("Default");
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00012562 File Offset: 0x00010762
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x0001256A File Offset: 0x0001076A
		[Editor(false)]
		public TextWidget BreachedTextWidget
		{
			get
			{
				return this._breachedTextWidget;
			}
			set
			{
				if (this._breachedTextWidget != value)
				{
					this._breachedTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "BreachedTextWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001258F File Offset: 0x0001078F
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00012597 File Offset: 0x00010797
		[Editor(false)]
		public Widget TypeIconWidget
		{
			get
			{
				return this._typeIconWidget;
			}
			set
			{
				if (this._typeIconWidget != value)
				{
					this._typeIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "TypeIconWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x000125BC File Offset: 0x000107BC
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x000125C4 File Offset: 0x000107C4
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x000125E7 File Offset: 0x000107E7
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x000125EF File Offset: 0x000107EF
		public int PointType
		{
			get
			{
				return this._pointType;
			}
			set
			{
				if (this._pointType != value)
				{
					this._pointType = value;
					base.OnPropertyChanged(value, "PointType");
				}
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001260D File Offset: 0x0001080D
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00012615 File Offset: 0x00010815
		public bool IsInsideWindow
		{
			get
			{
				return this._isInsideWindow;
			}
			set
			{
				if (this._isInsideWindow != value)
				{
					this._isInsideWindow = value;
					base.OnPropertyChanged(value, "IsInsideWindow");
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00012633 File Offset: 0x00010833
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0001263B File Offset: 0x0001083B
		public bool IsInFront
		{
			get
			{
				return this._isInFront;
			}
			set
			{
				if (this._isInFront != value)
				{
					this._isInFront = value;
					base.OnPropertyChanged(value, "IsInFront");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00012659 File Offset: 0x00010859
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00012661 File Offset: 0x00010861
		public bool IsPlayerGeneral
		{
			get
			{
				return this._isPlayerGeneral;
			}
			set
			{
				if (this._isPlayerGeneral != value)
				{
					this._isPlayerGeneral = value;
					base.OnPropertyChanged(value, "IsPlayerGeneral");
				}
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001267F File Offset: 0x0001087F
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00012687 File Offset: 0x00010887
		public OrderSiegeDeploymentScreenWidget ScreenWidget
		{
			get
			{
				return this._screenWidget;
			}
			set
			{
				if (this._screenWidget != value)
				{
					this._screenWidget = value;
					base.OnPropertyChanged<OrderSiegeDeploymentScreenWidget>(value, "ScreenWidget");
				}
			}
		}

		// Token: 0x0400029E RID: 670
		private bool preSelectedState;

		// Token: 0x0400029F RID: 671
		private bool _isVisualsDirty = true;

		// Token: 0x040002A0 RID: 672
		private Vec2 _position;

		// Token: 0x040002A1 RID: 673
		private bool _isInsideWindow;

		// Token: 0x040002A2 RID: 674
		private bool _isInFront;

		// Token: 0x040002A3 RID: 675
		private bool _isPlayerGeneral;

		// Token: 0x040002A4 RID: 676
		private OrderSiegeDeploymentScreenWidget _screenWidget;

		// Token: 0x040002A5 RID: 677
		private int _pointType;

		// Token: 0x040002A6 RID: 678
		private Widget _typeIconWidget;

		// Token: 0x040002A7 RID: 679
		private TextWidget _breachedTextWidget;
	}
}
