using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets.Graph
{
	// Token: 0x0200001A RID: 26
	public class GraphLinePointWidget : BrushWidget
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000076B7 File Offset: 0x000058B7
		// (set) Token: 0x06000158 RID: 344 RVA: 0x000076BF File Offset: 0x000058BF
		public float HorizontalValue { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000076C8 File Offset: 0x000058C8
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000076D0 File Offset: 0x000058D0
		public float VerticalValue { get; set; }

		// Token: 0x0600015B RID: 347 RVA: 0x000076D9 File Offset: 0x000058D9
		public GraphLinePointWidget(UIContext context)
			: base(context)
		{
		}
	}
}
