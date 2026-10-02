using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation
{
	// Token: 0x02000189 RID: 393
	public class CharacterCreationNarrativeStageScreenWidget : Widget
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x00037FD7 File Offset: 0x000361D7
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x00037FDF File Offset: 0x000361DF
		public ButtonWidget NextButton { get; set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x00037FE8 File Offset: 0x000361E8
		// (set) Token: 0x06001481 RID: 5249 RVA: 0x00037FF0 File Offset: 0x000361F0
		public ButtonWidget PreviousButton { get; set; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x00037FF9 File Offset: 0x000361F9
		// (set) Token: 0x06001483 RID: 5251 RVA: 0x00038001 File Offset: 0x00036201
		public ListPanel ItemList { get; set; }

		// Token: 0x06001484 RID: 5252 RVA: 0x0003800A File Offset: 0x0003620A
		public CharacterCreationNarrativeStageScreenWidget(UIContext context)
			: base(context)
		{
		}
	}
}
