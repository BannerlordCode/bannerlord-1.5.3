using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000185 RID: 389
	public class PerkSelectionBarWidget : Widget
	{
		// Token: 0x06001451 RID: 5201 RVA: 0x0003791B File Offset: 0x00035B1B
		public PerkSelectionBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00037938 File Offset: 0x00035B38
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.PerksList != null)
			{
				for (int i = 0; i < this.PerksList.ChildCount; i++)
				{
					PerkItemButtonWidget perkItemButtonWidget = this.PerksList.GetChild(i) as PerkItemButtonWidget;
					if (this._perkWidgetWidth != perkItemButtonWidget.Size.X)
					{
						this._perkWidgetWidth = perkItemButtonWidget.Size.X;
					}
					perkItemButtonWidget.PositionXOffset = this.GetXPositionOfLevelOnBar((float)perkItemButtonWidget.Level) - this._perkWidgetWidth / 2f * base._inverseScaleToUse;
					if (perkItemButtonWidget.AlternativeType == 0)
					{
						perkItemButtonWidget.PositionYOffset = 45f;
					}
					else if (perkItemButtonWidget.AlternativeType == 1)
					{
						perkItemButtonWidget.PositionYOffset = 5f;
					}
					else if (perkItemButtonWidget.AlternativeType == 2)
					{
						perkItemButtonWidget.PositionYOffset = (float)((int)Mathf.Round(perkItemButtonWidget.Size.Y * base._inverseScaleToUse));
					}
				}
			}
			if (this.PercentageIndicatorWidget != null)
			{
				float xpositionOfLevelOnBar = this.GetXPositionOfLevelOnBar((float)this.Level);
				float num = xpositionOfLevelOnBar - this.PercentageIndicatorWidget.Size.X / 2f * base._inverseScaleToUse;
				this.PercentageIndicatorWidget.PositionXOffset = num;
				if (this.FullLearningRateClip != null)
				{
					float num2 = this.GetXPositionOfLevelOnBar((float)this.FullLearningRateLevel) - xpositionOfLevelOnBar;
					this.FullLearningRateClip.SuggestedWidth = ((num2 >= 0f) ? num2 : 0f);
					this.FullLearningRateClip.PositionXOffset = this.PercentageIndicatorWidget.PositionXOffset + this.PercentageIndicatorWidget.Size.X / 2f * base._inverseScaleToUse;
					this.FullLearningRateClipInnerContent.PositionXOffset = -this.FullLearningRateClip.PositionXOffset;
					if (this.LearningLimitIndicatorWidget != null)
					{
						this.LearningLimitIndicatorWidget.PositionXOffset = this.FullLearningRateClip.PositionXOffset + this.FullLearningRateClip.SuggestedWidth - this.LearningLimitIndicatorWidget.Size.X * base._inverseScaleToUse / 2f;
					}
				}
				this.ProgressClip.SuggestedWidth = num + this.PercentageIndicatorWidget.Size.X / 2f * base._inverseScaleToUse;
			}
			using (List<Widget>.Enumerator enumerator = this.SeperatorContainer.Children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterDeveloperSkillVerticalSeperatorWidget characterDeveloperSkillVerticalSeperatorWidget;
					if ((characterDeveloperSkillVerticalSeperatorWidget = enumerator.Current as CharacterDeveloperSkillVerticalSeperatorWidget) != null)
					{
						characterDeveloperSkillVerticalSeperatorWidget.PositionXOffset = this.GetXPositionOfLevelOnBar((float)characterDeveloperSkillVerticalSeperatorWidget.SkillValue);
					}
				}
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00037BC4 File Offset: 0x00035DC4
		private float GetXPositionOfLevelOnBar(float level)
		{
			return Mathf.Clamp(level / ((float)this.MaxLevel + 25f) * base.Size.X * base._inverseScaleToUse, 0f, base.Size.X * base._inverseScaleToUse);
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x00037C04 File Offset: 0x00035E04
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x00037C0C File Offset: 0x00035E0C
		public Widget ProgressClip
		{
			get
			{
				return this._progressClip;
			}
			set
			{
				if (this._progressClip != value)
				{
					this._progressClip = value;
					base.OnPropertyChanged<Widget>(value, "ProgressClip");
				}
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00037C2A File Offset: 0x00035E2A
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x00037C32 File Offset: 0x00035E32
		public Widget PercentageIndicatorWidget
		{
			get
			{
				return this._percentageIndicatorWidget;
			}
			set
			{
				if (this._percentageIndicatorWidget != value)
				{
					this._percentageIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "PercentageIndicatorWidget");
				}
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x00037C50 File Offset: 0x00035E50
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x00037C58 File Offset: 0x00035E58
		public Widget FullLearningRateClip
		{
			get
			{
				return this._fullLearningRateClip;
			}
			set
			{
				if (this._fullLearningRateClip != value)
				{
					this._fullLearningRateClip = value;
					base.OnPropertyChanged<Widget>(value, "FullLearningRateClip");
				}
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x00037C76 File Offset: 0x00035E76
		// (set) Token: 0x0600145B RID: 5211 RVA: 0x00037C7E File Offset: 0x00035E7E
		public Widget SeperatorContainer
		{
			get
			{
				return this._seperatorContainer;
			}
			set
			{
				if (this._seperatorContainer != value)
				{
					this._seperatorContainer = value;
					base.OnPropertyChanged<Widget>(value, "SeperatorContainer");
				}
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x00037C9C File Offset: 0x00035E9C
		// (set) Token: 0x0600145D RID: 5213 RVA: 0x00037CA4 File Offset: 0x00035EA4
		public Widget LearningLimitIndicatorWidget
		{
			get
			{
				return this._learningLimitIndicatorWidget;
			}
			set
			{
				if (this._learningLimitIndicatorWidget != value)
				{
					this._learningLimitIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "LearningLimitIndicatorWidget");
				}
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x00037CC2 File Offset: 0x00035EC2
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x00037CCA File Offset: 0x00035ECA
		public Widget FullLearningRateClipInnerContent
		{
			get
			{
				return this._fullLearningRateClipInnerContent;
			}
			set
			{
				if (this._fullLearningRateClipInnerContent != value)
				{
					this._fullLearningRateClipInnerContent = value;
					base.OnPropertyChanged<Widget>(value, "FullLearningRateClipInnerContent");
				}
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x00037CE8 File Offset: 0x00035EE8
		// (set) Token: 0x06001461 RID: 5217 RVA: 0x00037CF0 File Offset: 0x00035EF0
		public Widget PerksList
		{
			get
			{
				return this._perksList;
			}
			set
			{
				if (this._perksList != value)
				{
					this._perksList = value;
					base.OnPropertyChanged<Widget>(value, "PerksList");
				}
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x00037D0E File Offset: 0x00035F0E
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x00037D16 File Offset: 0x00035F16
		public TextWidget PercentageIndicatorTextWidget
		{
			get
			{
				return this._percentageIndicatorTextWidget;
			}
			set
			{
				if (this._percentageIndicatorTextWidget != value)
				{
					this._percentageIndicatorTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "PercentageIndicatorTextWidget");
				}
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x00037D34 File Offset: 0x00035F34
		// (set) Token: 0x06001465 RID: 5221 RVA: 0x00037D3C File Offset: 0x00035F3C
		public int MaxLevel
		{
			get
			{
				return this._maxLevel;
			}
			set
			{
				if (this._maxLevel != value)
				{
					this._maxLevel = value;
					base.OnPropertyChanged(value, "MaxLevel");
				}
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x00037D5A File Offset: 0x00035F5A
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x00037D62 File Offset: 0x00035F62
		public int FullLearningRateLevel
		{
			get
			{
				return this._fullLearningRateLevel;
			}
			set
			{
				if (this._fullLearningRateLevel != value)
				{
					this._fullLearningRateLevel = value;
					base.OnPropertyChanged(value, "FullLearningRateLevel");
				}
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x00037D80 File Offset: 0x00035F80
		// (set) Token: 0x06001469 RID: 5225 RVA: 0x00037D88 File Offset: 0x00035F88
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					base.OnPropertyChanged(value, "Level");
				}
			}
		}

		// Token: 0x04000942 RID: 2370
		private float _perkWidgetWidth = -1f;

		// Token: 0x04000943 RID: 2371
		private Widget _perksList;

		// Token: 0x04000944 RID: 2372
		private Widget _progressClip;

		// Token: 0x04000945 RID: 2373
		private Widget _fullLearningRateClip;

		// Token: 0x04000946 RID: 2374
		private Widget _fullLearningRateClipInnerContent;

		// Token: 0x04000947 RID: 2375
		private Widget _percentageIndicatorWidget;

		// Token: 0x04000948 RID: 2376
		private Widget _seperatorContainer;

		// Token: 0x04000949 RID: 2377
		private Widget _learningLimitIndicatorWidget;

		// Token: 0x0400094A RID: 2378
		private TextWidget _percentageIndicatorTextWidget;

		// Token: 0x0400094B RID: 2379
		private int _maxLevel;

		// Token: 0x0400094C RID: 2380
		private int _fullLearningRateLevel;

		// Token: 0x0400094D RID: 2381
		private int _level = -1;
	}
}
