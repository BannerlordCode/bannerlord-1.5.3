using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Parley
{
	// Token: 0x02000123 RID: 291
	public class MapParleyAnimationParentBrushWidget : BrushWidget
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x0002ABC4 File Offset: 0x00028DC4
		public MapParleyAnimationParentBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0002ABD4 File Offset: 0x00028DD4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._firstFrame)
			{
				this._firstFrame = false;
				this._targetYOffset = base.PositionYOffset;
				this._minYOffset = this._targetYOffset - 50f;
			}
			this._animationDelta += dt;
			if (this._animationDelta < 0.1f)
			{
				float num = this._animationDelta / 0.1f;
				base.PositionYOffset = MathF.Lerp(this._minYOffset, this._targetYOffset, num, 1E-05f);
				this.SetGlobalAlphaRecursively(MathF.Lerp(0f, 1f, num, 1E-05f));
				return;
			}
			if (this._animationDelta < this.AnimationDuration - 0.1f)
			{
				base.PositionYOffset = this._targetYOffset;
				this.SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this._animationDelta < this.AnimationDuration)
			{
				float num2 = (this._animationDelta - (this.AnimationDuration - 0.1f)) / 0.1f;
				base.PositionYOffset = MathF.Lerp(this._targetYOffset, this._minYOffset, num2, 1E-05f);
				this.SetGlobalAlphaRecursively(MathF.Lerp(1f, 0f, num2, 1E-05f));
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0002AD02 File Offset: 0x00028F02
		// (set) Token: 0x06000F7D RID: 3965 RVA: 0x0002AD0A File Offset: 0x00028F0A
		[Editor(false)]
		public float AnimationDuration
		{
			get
			{
				return this._animationDuration;
			}
			set
			{
				if (this._animationDuration != value)
				{
					this._animationDuration = value;
					base.OnPropertyChanged(value, "AnimationDuration");
				}
			}
		}

		// Token: 0x0400070F RID: 1807
		private bool _firstFrame = true;

		// Token: 0x04000710 RID: 1808
		private const float _fadeInOutDuration = 0.1f;

		// Token: 0x04000711 RID: 1809
		private float _animationDelta;

		// Token: 0x04000712 RID: 1810
		private float _targetYOffset;

		// Token: 0x04000713 RID: 1811
		private float _minYOffset;

		// Token: 0x04000714 RID: 1812
		private const float _fadeInOutYMovement = 50f;

		// Token: 0x04000715 RID: 1813
		private float _animationDuration;
	}
}
