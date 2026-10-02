using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information.RundownTooltip
{
	// Token: 0x0200014E RID: 334
	public class RundownLineWidget : ListPanel
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00031F46 File Offset: 0x00030146
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00031F4E File Offset: 0x0003014E
		public TextWidget NameTextWidget { get; set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00031F57 File Offset: 0x00030157
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x00031F5F File Offset: 0x0003015F
		public TextWidget ValueTextWidget { get; set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00031F68 File Offset: 0x00030168
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x00031F70 File Offset: 0x00030170
		public float Value { get; set; }

		// Token: 0x060011EE RID: 4590 RVA: 0x00031F79 File Offset: 0x00030179
		public RundownLineWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00031F84 File Offset: 0x00030184
		public void RefreshValueOffset(float columnWidth)
		{
			if (columnWidth >= 0f && this.NameTextWidget.Size.X > 1E-05f && this.ValueTextWidget.Size.X > 1E-05f)
			{
				this.ValueTextWidget.ScaledPositionXOffset = columnWidth - (this.NameTextWidget.Size.X + this.ValueTextWidget.Size.X + base.ScaledMarginLeft + base.ScaledMarginRight);
			}
		}
	}
}
