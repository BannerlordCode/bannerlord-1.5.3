using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CustomBattle
{
	// Token: 0x02000162 RID: 354
	public class CustomBattleSliderLockButtonWidget : ButtonWidget
	{
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x0003409F File Offset: 0x0003229F
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x000340A7 File Offset: 0x000322A7
		public Brush LockOpenedBrush { get; set; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x000340B0 File Offset: 0x000322B0
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x000340B8 File Offset: 0x000322B8
		public Brush LockClosedBrush { get; set; }

		// Token: 0x060012D9 RID: 4825 RVA: 0x000340C1 File Offset: 0x000322C1
		public CustomBattleSliderLockButtonWidget(UIContext context)
			: base(context)
		{
			base.boolPropertyChanged += this.CustomBattleSliderLockButtonWidget_PropertyChanged;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x000340DC File Offset: 0x000322DC
		private void CustomBattleSliderLockButtonWidget_PropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsSelected")
			{
				base.Brush = (propertyValue ? this.LockClosedBrush : this.LockOpenedBrush);
			}
		}
	}
}
