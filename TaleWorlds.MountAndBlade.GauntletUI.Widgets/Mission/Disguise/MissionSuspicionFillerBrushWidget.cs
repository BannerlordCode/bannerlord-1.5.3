using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Disguise
{
	// Token: 0x02000103 RID: 259
	public class MissionSuspicionFillerBrushWidget : Widget
	{
		// Token: 0x06000DF9 RID: 3577 RVA: 0x000267ED File Offset: 0x000249ED
		public MissionSuspicionFillerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x000267F8 File Offset: 0x000249F8
		private void UpdateBrushState(float suspicionRatio)
		{
			if (suspicionRatio >= 1f)
			{
				BrushWidget circleIcon = this.CircleIcon;
				if (circleIcon != null)
				{
					circleIcon.SetState("Full");
				}
				Widget detectionFillContainer = this.DetectionFillContainer;
				if (detectionFillContainer != null)
				{
					detectionFillContainer.SetState("Full");
				}
				BrushWidget exclamationMark = this.ExclamationMark;
				if (exclamationMark == null)
				{
					return;
				}
				exclamationMark.SetState("Full");
				return;
			}
			else if (suspicionRatio > this._currentSuspicionRatio)
			{
				BrushWidget circleIcon2 = this.CircleIcon;
				if (circleIcon2 != null)
				{
					circleIcon2.SetState("Increasing");
				}
				Widget detectionFillContainer2 = this.DetectionFillContainer;
				if (detectionFillContainer2 != null)
				{
					detectionFillContainer2.SetState("Increasing");
				}
				BrushWidget exclamationMark2 = this.ExclamationMark;
				if (exclamationMark2 == null)
				{
					return;
				}
				exclamationMark2.SetState("Increasing");
				return;
			}
			else
			{
				BrushWidget circleIcon3 = this.CircleIcon;
				if (circleIcon3 != null)
				{
					circleIcon3.SetState("Decreasing");
				}
				Widget detectionFillContainer3 = this.DetectionFillContainer;
				if (detectionFillContainer3 != null)
				{
					detectionFillContainer3.SetState("Decreasing");
				}
				BrushWidget exclamationMark3 = this.ExclamationMark;
				if (exclamationMark3 == null)
				{
					return;
				}
				exclamationMark3.SetState("Decreasing");
				return;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x000268DB File Offset: 0x00024ADB
		// (set) Token: 0x06000DFC RID: 3580 RVA: 0x000268E3 File Offset: 0x00024AE3
		public float CurrentSuspicionRatio
		{
			get
			{
				return this._currentSuspicionRatio;
			}
			set
			{
				if (value != this._currentSuspicionRatio)
				{
					this.UpdateBrushState(value);
					this._currentSuspicionRatio = value;
					base.OnPropertyChanged(value, "CurrentSuspicionRatio");
				}
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00026908 File Offset: 0x00024B08
		// (set) Token: 0x06000DFE RID: 3582 RVA: 0x00026910 File Offset: 0x00024B10
		public BrushWidget ExclamationMark
		{
			get
			{
				return this._exclamationMark;
			}
			set
			{
				if (value != this._exclamationMark)
				{
					this._exclamationMark = value;
					base.OnPropertyChanged<BrushWidget>(value, "ExclamationMark");
				}
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x0002692E File Offset: 0x00024B2E
		// (set) Token: 0x06000E00 RID: 3584 RVA: 0x00026936 File Offset: 0x00024B36
		public Widget DetectionFillContainer
		{
			get
			{
				return this._detectionFillContainer;
			}
			set
			{
				if (value != this._detectionFillContainer)
				{
					this._detectionFillContainer = value;
					base.OnPropertyChanged<Widget>(value, "DetectionFillContainer");
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00026954 File Offset: 0x00024B54
		// (set) Token: 0x06000E02 RID: 3586 RVA: 0x0002695C File Offset: 0x00024B5C
		public BrushWidget CircleIcon
		{
			get
			{
				return this._circleIcon;
			}
			set
			{
				if (value != this._circleIcon)
				{
					this._circleIcon = value;
					base.OnPropertyChanged<BrushWidget>(value, "CircleIcon");
				}
			}
		}

		// Token: 0x0400065A RID: 1626
		private float _currentSuspicionRatio;

		// Token: 0x0400065B RID: 1627
		private BrushWidget _exclamationMark;

		// Token: 0x0400065C RID: 1628
		private Widget _detectionFillContainer;

		// Token: 0x0400065D RID: 1629
		private BrushWidget _circleIcon;
	}
}
