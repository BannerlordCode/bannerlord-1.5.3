using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002F RID: 47
	[Tutorial("TalkToNotableTutorialStep1")]
	public class TalkToNotableTutorialStep1 : TutorialItemBase
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00003969 File Offset: 0x00001B69
		public TalkToNotableTutorialStep1()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "ApplicableNotable";
			base.MouseRequired = true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000398A File Offset: 0x00001B8A
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000398D File Offset: 0x00001B8D
		public override bool IsConditionsMetForActivation()
		{
			return !TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.VillageMenuIsOpen && Settlement.CurrentSettlement.StringId == "village_ES3_2";
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000039BB File Offset: 0x00001BBB
		public override bool IsConditionsMetForCompletion()
		{
			return this._wantedCharacterPopupOpened;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000039C4 File Offset: 0x00001BC4
		public override void OnCharacterPortraitPopUpOpened(CharacterObject obj)
		{
			bool flag;
			if (obj == null)
			{
				flag = false;
			}
			else
			{
				Hero heroObject = obj.HeroObject;
				bool? flag2 = ((heroObject != null) ? new bool?(heroObject.IsHeadman) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			this._wantedCharacterPopupOpened = flag;
		}

		// Token: 0x04000037 RID: 55
		private bool _wantedCharacterPopupOpened;
	}
}
