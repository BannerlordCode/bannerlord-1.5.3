using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Library;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000055 RID: 85
	public class ThirdPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000543 RID: 1347 RVA: 0x0001E5CC File Offset: 0x0001C7CC
		public override void RegisterEvents()
		{
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.CanKingdomBeDiscontinuedEvent.AddNonSerializedListener(this, new ReferenceAction<Kingdom, bool>(this.CanKingdomBeDiscontinued));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001E635 File Offset: 0x0001C835
		private void OnSessionLaunched(CampaignGameStarter starter)
		{
			this.AddGameMenus(starter);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001E640 File Offset: 0x0001C840
		private void AddGameMenus(CampaignGameStarter starter)
		{
			starter.AddGameMenu("siege_ended_by_last_conspiracy_kingdom_defeat", "{=3pEDvftb} The conspiracy has collapsed. The defenders of their final stronghold send out a delegation under flag of truce and agree to surrender. Your men take possession of their fortress.", new OnInitDelegate(this.game_menu_last_conspiracy_kingdom_defeated_when_player_besiege_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("siege_ended_by_last_conspiracy_kingdom_defeat", "leave_from_besieged_last_conspiracy_settlement", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.siege_ended_by_last_conspiracy_kingdom_defeat_condition), new GameMenuOption.OnConsequenceDelegate(this.siege_ended_by_last_conspiracy_kingdom_defeat_consequence), true, -1, false, null);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			Kingdom kingdom;
			Kingdom kingdom2;
			if ((kingdom = faction1 as Kingdom) != null && (kingdom2 = faction2 as Kingdom) != null && StoryModeManager.Current.MainStoryLine.ThirdPhase != null)
			{
				MBReadOnlyList<Kingdom> oppositionKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms;
				MBReadOnlyList<Kingdom> allyKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.AllyKingdoms;
				if ((oppositionKingdoms.IndexOf(kingdom) >= 0 && oppositionKingdoms.IndexOf(kingdom2) >= 0) || (allyKingdoms.IndexOf(kingdom) >= 0 && allyKingdoms.IndexOf(kingdom2) >= 0))
				{
					this._warsToEnforcePeaceNextWeek.Add(new Tuple<Kingdom, Kingdom>(kingdom, kingdom2));
				}
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001E738 File Offset: 0x0001C938
		private void WeeklyTick()
		{
			foreach (Tuple<Kingdom, Kingdom> tuple in new List<Tuple<Kingdom, Kingdom>>(this._warsToEnforcePeaceNextWeek))
			{
				MakePeaceAction.Apply(tuple.Item1, tuple.Item2);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001E79C File Offset: 0x0001C99C
		private void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null && StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.Contains(kingdom))
			{
				result = false;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001E7CE File Offset: 0x0001C9CE
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Tuple<Kingdom, Kingdom>>>("_warsToEnforcePeaceNextWeek", ref this._warsToEnforcePeaceNextWeek);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001E7E2 File Offset: 0x0001C9E2
		private void siege_ended_by_last_conspiracy_kingdom_defeat_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001E7E9 File Offset: 0x0001C9E9
		private bool siege_ended_by_last_conspiracy_kingdom_defeat_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
		private void game_menu_last_conspiracy_kingdom_defeated_when_player_besiege_menu_on_init(MenuCallbackArgs args)
		{
			Debug.Print("Game loaded when the player siege is left on last conspiracy kingdom is defeated by some other reasons", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x040001DD RID: 477
		private List<Tuple<Kingdom, Kingdom>> _warsToEnforcePeaceNextWeek = new List<Tuple<Kingdom, Kingdom>>();
	}
}
