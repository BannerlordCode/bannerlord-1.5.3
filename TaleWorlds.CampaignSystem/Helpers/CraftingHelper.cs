using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000024 RID: 36
	public static class CraftingHelper
	{
		// Token: 0x06000120 RID: 288 RVA: 0x0000E578 File Offset: 0x0000C778
		public static IEnumerable<Hero> GetAvailableHeroesForCrafting()
		{
			return from m in PartyBase.MainParty.MemberRoster.GetTroopRoster()
				where m.Character.IsHero
				select m into t
				select t.Character.HeroObject;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public static void ChangeCurrentCraftingTemplate(CraftingTemplate craftingTemplate)
		{
			CraftingState craftingState = Game.Current.GameStateManager.ActiveState as CraftingState;
			CraftingHelper.OpenCrafting(craftingTemplate, craftingState);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000E608 File Offset: 0x0000C808
		public static void OpenCrafting(CraftingTemplate craftingTemplate, CraftingState oldState = null)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			TextObject textObject = new TextObject("{=uZhHh7pm}Crafted {CURR_TEMPLATE_NAME}", null);
			textObject.SetTextVariable("CURR_TEMPLATE_NAME", craftingTemplate.TemplateName);
			Crafting crafting = new Crafting(craftingTemplate, (currentSettlement != null) ? currentSettlement.Culture : new CultureObject(), textObject);
			crafting.Init();
			crafting.ReIndex(false);
			if (oldState == null)
			{
				CraftingState craftingState = Game.Current.GameStateManager.CreateState<CraftingState>();
				craftingState.InitializeLogic(crafting, false);
				Game.Current.GameStateManager.PushState(craftingState, 0);
				return;
			}
			oldState.InitializeLogic(crafting, true);
		}
	}
}
