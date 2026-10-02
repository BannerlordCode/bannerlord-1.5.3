using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000188 RID: 392
	public class SkillProgressFillBarWidget : FillBarWidget
	{
		// Token: 0x0600147A RID: 5242 RVA: 0x00037F48 File Offset: 0x00036148
		public SkillProgressFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00037F54 File Offset: 0x00036154
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (this.PercentageIndicatorWidget != null)
			{
				base.ScaledPositionXOffset = Mathf.Clamp((this.PercentageIndicatorWidget.ScaledPositionXOffset - base.Size.X / 2f) * base._scaleToUse, 0f, 600f * base._scaleToUse);
			}
			base.OnRender(twoDimensionContext, drawContext);
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x00037FB1 File Offset: 0x000361B1
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x00037FB9 File Offset: 0x000361B9
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

		// Token: 0x04000956 RID: 2390
		private Widget _percentageIndicatorWidget;
	}
}
