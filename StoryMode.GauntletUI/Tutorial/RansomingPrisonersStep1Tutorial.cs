using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000012 RID: 18
	[Tutorial("RansomingPrisonersStep1")]
	public class RansomingPrisonersStep1Tutorial : TutorialItemBase
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002A1B File Offset: 0x00000C1B
		public RansomingPrisonersStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "town_backstreet";
			base.MouseRequired = true;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A3C File Offset: 0x00000C3C
		public override bool IsConditionsMetForCompletion()
		{
			return this._wantedGameMenuOpened;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002A44 File Offset: 0x00000C44
		public override void OnGameMenuOpened(MenuCallbackArgs obj)
		{
			this._wantedGameMenuOpened = obj.MenuContext.GameMenu.StringId == "town_backstreet";
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002A66 File Offset: 0x00000C66
		public override bool IsConditionsMetForActivation()
		{
			return !TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.PlayerIsInNonEnemyTown && TutorialHelper.TownMenuIsOpen && !Hero.MainHero.IsPrisoner && MobileParty.MainParty.PrisonRoster.TotalManCount > 0;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002AA5 File Offset: 0x00000CA5
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x04000015 RID: 21
		private bool _wantedGameMenuOpened;
	}
}
