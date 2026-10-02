using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000030 RID: 48
	[Tutorial("TalkToNotableTutorialStep2")]
	public class TalkToNotableTutorialStep2 : TutorialItemBase
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00003A11 File Offset: 0x00001C11
		public TalkToNotableTutorialStep2()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "OverlayTalkButton";
			base.MouseRequired = true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003A32 File Offset: 0x00001C32
		public override bool IsConditionsMetForCompletion()
		{
			return this._hasTalkedToNotable;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003A3A File Offset: 0x00001C3A
		public override void OnPlayerStartTalkFromMenuOverlay(Hero hero)
		{
			this._hasTalkedToNotable = hero.IsHeadman;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003A48 File Offset: 0x00001C48
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003A4B File Offset: 0x00001C4B
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.IsCharacterPopUpWindowOpen;
		}

		// Token: 0x04000038 RID: 56
		private bool _hasTalkedToNotable;
	}
}
