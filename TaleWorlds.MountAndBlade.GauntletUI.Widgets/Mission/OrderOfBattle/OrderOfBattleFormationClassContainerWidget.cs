using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EC RID: 236
	public class OrderOfBattleFormationClassContainerWidget : Widget
	{
		// Token: 0x06000C3C RID: 3132 RVA: 0x00021C21 File Offset: 0x0001FE21
		public OrderOfBattleFormationClassContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00021C2A File Offset: 0x0001FE2A
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x00021C32 File Offset: 0x0001FE32
		[Editor(false)]
		public SliderWidget WeightSlider
		{
			get
			{
				return this._weightSlider;
			}
			set
			{
				if (value != this._weightSlider)
				{
					this._weightSlider = value;
					base.OnPropertyChanged<SliderWidget>(value, "WeightSlider");
				}
			}
		}

		// Token: 0x0400058A RID: 1418
		private SliderWidget _weightSlider;
	}
}
