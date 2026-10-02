using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001D RID: 29
	public class CircleItemPlacerWidget : Widget
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000BEA6 File Offset: 0x0000A0A6
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000BEAE File Offset: 0x0000A0AE
		public float DistanceFromCenterModifier { get; set; } = 300f;

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000BEB7 File Offset: 0x0000A0B7
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000BEBF File Offset: 0x0000A0BF
		public Widget DirectionWidget { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000BED0 File Offset: 0x0000A0D0
		public float DirectionWidgetDistanceMultiplier { get; set; } = 0.5f;

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000BED9 File Offset: 0x0000A0D9
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000BEE1 File Offset: 0x0000A0E1
		public bool ActivateOnlyWithController { get; set; }

		// Token: 0x06000249 RID: 585 RVA: 0x0000BEEA File Offset: 0x0000A0EA
		public CircleItemPlacerWidget(UIContext context)
			: base(context)
		{
			this._centerDistanceAnimationTimer = -1f;
			this._centerDistanceAnimationDuration = -1f;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000BF1F File Offset: 0x0000A11F
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsRecursivelyVisible())
			{
				this.UpdateItemPlacement();
				this.AnimateDistanceFromCenter(dt);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000BF40 File Offset: 0x0000A140
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

		// Token: 0x0600024C RID: 588 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		public void AnimateDistanceFromCenterTo(float distanceFromCenter, float animationDuration)
		{
			this._centerDistanceAnimationTimer = 0f;
			this._centerDistanceAnimationInitialValue = this.DistanceFromCenterModifier;
			this._centerDistanceAnimationDuration = animationDuration;
			this._centerDistanceAnimationTarget = distanceFromCenter;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C010 File Offset: 0x0000A210
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

		// Token: 0x0600024E RID: 590 RVA: 0x0000C0CC File Offset: 0x0000A2CC
		private float AddAngle(float angle1, float angle2)
		{
			float num = angle1 + angle2;
			if (num < 0f)
			{
				num += 360f;
			}
			return num % 360f;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C0F8 File Offset: 0x0000A2F8
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

		// Token: 0x06000250 RID: 592 RVA: 0x0000C170 File Offset: 0x0000A370
		private float AngleFromDir(Vec2 directionVector)
		{
			if (directionVector.X < 0f)
			{
				return 360f - (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f * -1f;
			}
			return (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
		private Vec2 DirFromAngle(float angle)
		{
			return new Vec2(MathF.Sin(angle), MathF.Cos(angle));
		}

		// Token: 0x0400012F RID: 303
		private float _centerDistanceAnimationTimer;

		// Token: 0x04000130 RID: 304
		private float _centerDistanceAnimationDuration;

		// Token: 0x04000131 RID: 305
		private float _centerDistanceAnimationInitialValue;

		// Token: 0x04000132 RID: 306
		private float _centerDistanceAnimationTarget;
	}
}
