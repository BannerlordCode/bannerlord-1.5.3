using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001C RID: 28
	public class CircleActionSelectorWidget : Widget
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000B598 File Offset: 0x00009798
		public CircleActionSelectorWidget(UIContext context)
			: base(context)
		{
			this._activateOnlyWithController = false;
			this._distanceFromCenterModifier = 300f;
			this._directionWidgetDistanceMultiplier = 0.5f;
			this._centerDistanceAnimationTimer = -1f;
			this._centerDistanceAnimationDuration = -1f;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B5F5 File Offset: 0x000097F5
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.boolPropertyChanged += this.OnChildPropertyChanged;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B610 File Offset: 0x00009810
		private void OnChildPropertyChanged(PropertyOwnerObject widget, string propertyName, bool value)
		{
			if (propertyName == "IsSelected" && base.EventManager.IsControllerActive && !this._isRefreshingSelection)
			{
				this._mouseDirection = Vec2.Zero;
				this._mouseMoveAccumulated = Vec2.Zero;
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B64C File Offset: 0x0000984C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.AllowInvalidSelection)
			{
				this._currentSelectedIndex = -1;
			}
			if (base.IsRecursivelyVisible())
			{
				this.UpdateItemPlacement();
				this.AnimateDistanceFromCenter(dt);
				bool flag = this.IsCircularInputEnabled && (!this.ActivateOnlyWithController || base.EventManager.IsControllerActive);
				if (this.DirectionWidget != null)
				{
					this.DirectionWidget.IsVisible = flag;
				}
				if (flag)
				{
					this.UpdateAverageMouseDirection();
					this.UpdateCircularInput(dt);
					return;
				}
			}
			else
			{
				if (this._mouseDirection.X != 0f || this._mouseDirection.Y != 0f)
				{
					this._mouseDirection = default(Vec2);
				}
				if (this.DirectionWidget != null)
				{
					this.DirectionWidget.IsVisible = false;
					this.DirectionWidget.PositionXOffset = 0f;
					this.DirectionWidget.PositionYOffset = 0f;
				}
				this._mouseMoveAccumulated = Vec2.Zero;
				this._invalidSelectionDuration = 0f;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B748 File Offset: 0x00009948
		private void AnimateDistanceFromCenter(float dt)
		{
			if (this._centerDistanceAnimationTimer == -1f || this._centerDistanceAnimationDuration == -1f || this._centerDistanceAnimationTarget == -1f)
			{
				return;
			}
			if (this._centerDistanceAnimationTimer < this._centerDistanceAnimationDuration)
			{
				this.DistanceFromCenterModifier = MathF.Lerp(this._centerDistanceAnimationInitialValue, this._centerDistanceAnimationTarget, this._centerDistanceAnimationTimer / this._centerDistanceAnimationDuration, 1E-05f);
				this._centerDistanceAnimationTimer += dt;
				return;
			}
			this.DistanceFromCenterModifier = this._centerDistanceAnimationTarget;
			this._centerDistanceAnimationTimer = -1f;
			this._centerDistanceAnimationDuration = -1f;
			this._centerDistanceAnimationTarget = -1f;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B7F0 File Offset: 0x000099F0
		public void AnimateDistanceFromCenterTo(float distanceFromCenter, float animationDuration)
		{
			this._centerDistanceAnimationTimer = 0f;
			this._centerDistanceAnimationInitialValue = this.DistanceFromCenterModifier;
			this._centerDistanceAnimationDuration = animationDuration;
			this._centerDistanceAnimationTarget = distanceFromCenter;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B818 File Offset: 0x00009A18
		private void UpdateAverageMouseDirection()
		{
			bool isMouseActive = base.Context.InputContext.GetIsMouseActive();
			Vector2 vector = (isMouseActive ? base.Context.InputContext.GetMouseMovement() : base.Context.InputContext.GetControllerRightStickState());
			if (isMouseActive)
			{
				this._mouseMoveAccumulated += vector;
				if (this._mouseMoveAccumulated.LengthSquared > 15625f)
				{
					this._mouseMoveAccumulated.Normalize();
					this._mouseMoveAccumulated *= 125f;
				}
				this._mouseDirection = new Vec2(this._mouseMoveAccumulated.X, -this._mouseMoveAccumulated.Y);
				return;
			}
			this._mouseDirection = new Vec2(vector.X, vector.Y);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		private void UpdateItemPlacement()
		{
			if (base.ChildCount > 0)
			{
				int childCount = base.ChildCount;
				float num = 360f / (float)childCount;
				float num2 = -(num / 2f);
				if (num2 < 0f)
				{
					num2 += 360f;
				}
				for (int i = 0; i < base.ChildCount; i++)
				{
					float num3 = num * (float)i;
					float num4 = this.AddAngle(num2, num3);
					num4 = this.AddAngle(num4, num / 2f);
					Vec2 vec = this.DirFromAngle(num4 * 0.017453292f);
					Widget child = base.GetChild(i);
					child.PositionXOffset = vec.X * this.DistanceFromCenterModifier;
					child.PositionYOffset = vec.Y * this.DistanceFromCenterModifier * -1f;
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B99D File Offset: 0x00009B9D
		public bool TrySetSelectedIndex(int index)
		{
			if (index >= 0 && index < base.ChildCount)
			{
				this.OnSelectedIndexChanged(index);
				return true;
			}
			return false;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		protected virtual void OnSelectedIndexChanged(int selectedIndex)
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				ButtonWidget buttonWidget;
				if ((buttonWidget = child as ButtonWidget) != null)
				{
					buttonWidget.IsSelected = !child.IsDisabled && i == selectedIndex;
				}
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000BA00 File Offset: 0x00009C00
		private void UpdateCircularInput(float dt)
		{
			int currentSelectedIndex = this._currentSelectedIndex;
			if (this._mouseDirection.Length > 0.391f)
			{
				this._invalidSelectionDuration = 0f;
				if (base.ChildCount > 0)
				{
					float num = this.AngleFromDir(this._mouseDirection);
					this._currentSelectedIndex = this.GetIndexOfSelectedItemByAngle(num);
				}
			}
			else if (this.AllowInvalidSelection && this._currentSelectedIndex != -1)
			{
				this._invalidSelectionDuration += dt;
				if (this._invalidSelectionDuration >= this.InvalidSelectionDelay)
				{
					this._currentSelectedIndex = -1;
				}
			}
			if (currentSelectedIndex != this._currentSelectedIndex)
			{
				this._isRefreshingSelection = true;
				this.OnSelectedIndexChanged(this._currentSelectedIndex);
				this._isRefreshingSelection = false;
			}
			if (this.DirectionWidget != null)
			{
				float length = this._mouseDirection.Length;
				float num2 = 0f;
				float num3 = 0f;
				if (length > this.DirectionWidgetDeadzone)
				{
					Vec2 vec = this._mouseDirection * (1f / length);
					float num4 = MathF.Min((length - this.DirectionWidgetDeadzone) / (1f - this.DirectionWidgetDeadzone), 1f);
					num4 = AnimationInterpolation.Ease(AnimationInterpolation.Type.EaseOut, AnimationInterpolation.Function.Cubic, num4);
					float num5 = this.DistanceFromCenterModifier * this.DirectionWidgetDistanceMultiplier * num4;
					num2 = vec.X * num5;
					num3 = -vec.Y * num5;
				}
				float num6 = MathF.Min(dt * 30f, 1f);
				this.DirectionWidget.PositionXOffset = MathF.Lerp(this.DirectionWidget.PositionXOffset, num2, num6, 1E-05f);
				this.DirectionWidget.PositionYOffset = MathF.Lerp(this.DirectionWidget.PositionYOffset, num3, num6, 1E-05f);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000BB9C File Offset: 0x00009D9C
		private int GetIndexOfSelectedItemByAngle(float mouseDirectionAngle)
		{
			int childCount = base.ChildCount;
			float num = 360f / (float)childCount;
			float num2 = -(num / 2f);
			if (num2 < 0f)
			{
				num2 += 360f;
			}
			for (int i = 0; i < childCount; i++)
			{
				float num3 = num * (float)i;
				float num4 = num * (float)(i + 1);
				float num5 = this.AddAngle(num2, num3) * 0.017453292f;
				float num6 = this.AddAngle(num2, num4) * 0.017453292f;
				if (this.IsAngleBetweenAngles(mouseDirectionAngle * 0.017453292f, num5, num6))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000BC24 File Offset: 0x00009E24
		private float AddAngle(float angle1, float angle2)
		{
			float num = angle1 + angle2;
			if (num < 0f)
			{
				num += 360f;
			}
			return num % 360f;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000BC50 File Offset: 0x00009E50
		private bool IsAngleBetweenAngles(float angle, float minAngle, float maxAngle)
		{
			float num = angle - 3.1415927f;
			float num2 = minAngle - 3.1415927f;
			float num3 = maxAngle - 3.1415927f;
			if (num2 == num3)
			{
				return true;
			}
			float num4 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(num3, num2));
			if (num4.ApproximatelyEqualsTo(3.1415927f, 1E-05f))
			{
				return num < num3;
			}
			float num5 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(num, num2));
			float num6 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(num, num3));
			return num5 < num4 && num6 < num4;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000BCC8 File Offset: 0x00009EC8
		private float AngleFromDir(Vec2 directionVector)
		{
			if (directionVector.X < 0f)
			{
				return 360f - (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f * -1f;
			}
			return (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000BD28 File Offset: 0x00009F28
		private Vec2 DirFromAngle(float angle)
		{
			return new Vec2(MathF.Sin(angle), MathF.Cos(angle));
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000BD3D File Offset: 0x00009F3D
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000BD45 File Offset: 0x00009F45
		public bool AllowInvalidSelection
		{
			get
			{
				return this._allowInvalidSelection;
			}
			set
			{
				if (value != this._allowInvalidSelection)
				{
					this._allowInvalidSelection = value;
					base.OnPropertyChanged(value, "AllowInvalidSelection");
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000BD63 File Offset: 0x00009F63
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000BD6B File Offset: 0x00009F6B
		public float InvalidSelectionDelay
		{
			get
			{
				return this._invalidSelectionDelay;
			}
			set
			{
				if (value != this._invalidSelectionDelay)
				{
					this._invalidSelectionDelay = value;
					base.OnPropertyChanged(value, "InvalidSelectionDelay");
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000BD89 File Offset: 0x00009F89
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000BD91 File Offset: 0x00009F91
		public float DirectionWidgetDeadzone
		{
			get
			{
				return this._directionWidgetDeadzone;
			}
			set
			{
				if (value != this._directionWidgetDeadzone)
				{
					this._directionWidgetDeadzone = value;
					base.OnPropertyChanged(value, "DirectionWidgetDeadzone");
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000BDAF File Offset: 0x00009FAF
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000BDB7 File Offset: 0x00009FB7
		public bool ActivateOnlyWithController
		{
			get
			{
				return this._activateOnlyWithController;
			}
			set
			{
				if (value != this._activateOnlyWithController)
				{
					this._activateOnlyWithController = value;
					base.OnPropertyChanged(value, "ActivateOnlyWithController");
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000BDD5 File Offset: 0x00009FD5
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		public bool IsCircularInputEnabled
		{
			get
			{
				return !this.IsCircularInputDisabled;
			}
			set
			{
				if (value == this.IsCircularInputDisabled)
				{
					this.IsCircularInputDisabled = !value;
					base.OnPropertyChanged(!value, "IsCircularInputEnabled");
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000BE04 File Offset: 0x0000A004
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000BE0C File Offset: 0x0000A00C
		public bool IsCircularInputDisabled
		{
			get
			{
				return this._isCircularInputDisabled;
			}
			set
			{
				if (value != this._isCircularInputDisabled)
				{
					this._isCircularInputDisabled = value;
					base.OnPropertyChanged(value, "IsCircularInputDisabled");
					if (value)
					{
						this.OnSelectedIndexChanged(-1);
					}
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000BE34 File Offset: 0x0000A034
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000BE3C File Offset: 0x0000A03C
		public float DistanceFromCenterModifier
		{
			get
			{
				return this._distanceFromCenterModifier;
			}
			set
			{
				if (value != this._distanceFromCenterModifier)
				{
					this._distanceFromCenterModifier = value;
					base.OnPropertyChanged(value, "DistanceFromCenterModifier");
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000BE5A File Offset: 0x0000A05A
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000BE62 File Offset: 0x0000A062
		public float DirectionWidgetDistanceMultiplier
		{
			get
			{
				return this._directionWidgetDistanceMultiplier;
			}
			set
			{
				if (value != this._directionWidgetDistanceMultiplier)
				{
					this._directionWidgetDistanceMultiplier = value;
					base.OnPropertyChanged(value, "DirectionWidgetDistanceMultiplier");
				}
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000BE80 File Offset: 0x0000A080
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000BE88 File Offset: 0x0000A088
		public Widget DirectionWidget
		{
			get
			{
				return this._directionWidget;
			}
			set
			{
				if (value != this._directionWidget)
				{
					this._directionWidget = value;
					base.OnPropertyChanged<Widget>(value, "DirectionWidget");
				}
			}
		}

		// Token: 0x04000117 RID: 279
		private int _currentSelectedIndex;

		// Token: 0x04000118 RID: 280
		private const float _mouseMoveMaxDistance = 125f;

		// Token: 0x04000119 RID: 281
		private const float _gamepadDeadzoneLength = 0.391f;

		// Token: 0x0400011A RID: 282
		private const float _mouseMoveMaxDistanceSquared = 15625f;

		// Token: 0x0400011B RID: 283
		private float _centerDistanceAnimationTimer;

		// Token: 0x0400011C RID: 284
		private float _centerDistanceAnimationDuration;

		// Token: 0x0400011D RID: 285
		private float _centerDistanceAnimationInitialValue;

		// Token: 0x0400011E RID: 286
		private float _centerDistanceAnimationTarget;

		// Token: 0x0400011F RID: 287
		private Vec2 _mouseDirection;

		// Token: 0x04000120 RID: 288
		private Vec2 _mouseMoveAccumulated;

		// Token: 0x04000121 RID: 289
		private bool _isRefreshingSelection;

		// Token: 0x04000122 RID: 290
		private float _invalidSelectionDuration;

		// Token: 0x04000123 RID: 291
		private bool _allowInvalidSelection;

		// Token: 0x04000124 RID: 292
		private bool _activateOnlyWithController;

		// Token: 0x04000125 RID: 293
		private bool _isCircularInputDisabled;

		// Token: 0x04000126 RID: 294
		private float _distanceFromCenterModifier;

		// Token: 0x04000127 RID: 295
		private float _directionWidgetDistanceMultiplier;

		// Token: 0x04000128 RID: 296
		private float _invalidSelectionDelay = 0.5f;

		// Token: 0x04000129 RID: 297
		private float _directionWidgetDeadzone = 0.1f;

		// Token: 0x0400012A RID: 298
		private Widget _directionWidget;
	}
}
