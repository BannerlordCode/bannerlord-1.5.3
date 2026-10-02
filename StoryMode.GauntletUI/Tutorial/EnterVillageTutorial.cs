using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000023 RID: 35
	[Tutorial("EnterVillageTutorial")]
	public class EnterVillageTutorial : TutorialItemBase
	{
		// Token: 0x060000AD RID: 173 RVA: 0x000034BA File Offset: 0x000016BA
		public EnterVillageTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "storymode_tutorial_village_enter";
			base.MouseRequired = true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000034DB File Offset: 0x000016DB
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000034DE File Offset: 0x000016DE
		public override bool IsConditionsMetForActivation()
		{
			if (!TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow)
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				return ((currentSettlement != null) ? currentSettlement.StringId : null) == "village_ES3_2";
			}
			return false;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000350C File Offset: 0x0000170C
		public override void OnGameMenuOptionSelected(GameMenuOption obj)
		{
			base.OnGameMenuOptionSelected(obj);
			this._isEnterOptionSelected = obj.IdString == "storymode_tutorial_village_enter";
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000352B File Offset: 0x0000172B
		public override bool IsConditionsMetForCompletion()
		{
			return this._isEnterOptionSelected;
		}

		// Token: 0x0400002E RID: 46
		private bool _isEnterOptionSelected;

		// Token: 0x0400002F RID: 47
		private const string _enterGameMenuOptionId = "storymode_tutorial_village_enter";
	}
}
