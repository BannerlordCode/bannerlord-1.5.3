using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C5 RID: 197
	public class MoraleArrowBrushWidget : BrushWidget
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0001D0C0 File Offset: 0x0001B2C0
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x0001D0C8 File Offset: 0x0001B2C8
		public bool LeftSideArrow { get; set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0001D0D1 File Offset: 0x0001B2D1
		public float BaseHorizontalExtendRange
		{
			get
			{
				return 3.3f;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0001D0D8 File Offset: 0x0001B2D8
		private float BaseSpeedModifier
		{
			get
			{
				return 13f;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0001D0DF File Offset: 0x0001B2DF
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x0001D0E7 File Offset: 0x0001B2E7
		public bool AreMoralesIndependent { get; set; }

		// Token: 0x06000A57 RID: 2647 RVA: 0x0001D0F0 File Offset: 0x0001B2F0
		public MoraleArrowBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001D0FC File Offset: 0x0001B2FC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				base.Brush.GlobalAlphaFactor = 0f;
				this._initialized = true;
			}
			base.IsVisible = this._currentFlow > 0 && !this.AreMoralesIndependent;
			if (base.IsVisible)
			{
				float num = this.BaseSpeedModifier * (float)Math.Sqrt((double)this._currentFlow);
				float num2 = this.BaseHorizontalExtendRange * (float)this._currentFlow;
				if (this._currentAnimState == MoraleArrowBrushWidget.AnimStates.FadeIn)
				{
					if (base.ReadOnlyBrush.GlobalAlphaFactor < 1f)
					{
						this.SetGlobalAlphaRecursively(Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 1f, dt * num));
					}
					if ((double)base.ReadOnlyBrush.GlobalAlphaFactor >= 0.99)
					{
						this._currentAnimState = MoraleArrowBrushWidget.AnimStates.Move;
					}
				}
				else if (this._currentAnimState == MoraleArrowBrushWidget.AnimStates.Move)
				{
					if (Math.Abs(base.PositionXOffset) < num2)
					{
						int num3 = (this.LeftSideArrow ? (-1) : 1);
						base.PositionXOffset = Mathf.Lerp(base.PositionXOffset, num2 * (float)num3, dt * num);
					}
					if ((double)Math.Abs(base.PositionXOffset) >= (double)num2 - 0.01)
					{
						this._currentAnimState = MoraleArrowBrushWidget.AnimStates.FadeOut;
					}
				}
				else if (this._currentAnimState == MoraleArrowBrushWidget.AnimStates.FadeOut)
				{
					if (base.ReadOnlyBrush.GlobalAlphaFactor > 0f)
					{
						this.SetGlobalAlphaRecursively(Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 0f, dt * num));
					}
					if ((double)base.ReadOnlyBrush.GlobalAlphaFactor <= 0.01)
					{
						this._currentAnimState = MoraleArrowBrushWidget.AnimStates.GoToInitPos;
					}
				}
				else
				{
					base.PositionXOffset = 0f;
					this._currentAnimState = MoraleArrowBrushWidget.AnimStates.FadeIn;
				}
			}
			else
			{
				base.PositionXOffset = 0f;
				this._currentAnimState = MoraleArrowBrushWidget.AnimStates.FadeIn;
			}
			this._timeSinceCreation += dt;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0001D2C6 File Offset: 0x0001B4C6
		public void SetFlowLevel(int flow)
		{
			this._currentFlow = flow;
			base.IsVisible = this._currentFlow > 0 && !this.AreMoralesIndependent;
		}

		// Token: 0x040004AB RID: 1195
		private float _timeSinceCreation;

		// Token: 0x040004AC RID: 1196
		private bool _initialized;

		// Token: 0x040004AD RID: 1197
		private int _currentFlow;

		// Token: 0x040004B0 RID: 1200
		private MoraleArrowBrushWidget.AnimStates _currentAnimState;

		// Token: 0x020001BF RID: 447
		private enum AnimStates
		{
			// Token: 0x04000A30 RID: 2608
			FadeIn,
			// Token: 0x04000A31 RID: 2609
			Move,
			// Token: 0x04000A32 RID: 2610
			FadeOut,
			// Token: 0x04000A33 RID: 2611
			GoToInitPos
		}
	}
}
