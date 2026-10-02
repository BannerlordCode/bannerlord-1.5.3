using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000443 RID: 1091
	public class ParleyCampaignBehavior : CampaignBehaviorBase, IParleyCampaignBehavior
	{
		// Token: 0x0600461B RID: 17947 RVA: 0x001551FD File Offset: 0x001533FD
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00155216 File Offset: 0x00153416
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<PartyBase>("_parleyedParty", ref this._parleyedParty);
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0015522A File Offset: 0x0015342A
		public void StartParley(PartyBase partyBase)
		{
			if (partyBase.IsSettlement)
			{
				this._parleyedParty = partyBase;
				GameMenu.ActivateGameMenu("request_meeting_parley");
				return;
			}
			Debug.FailedAssert("MobileParty parley not implemented yet!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\ParleyCampaignBehavior.cs", "StartParley", 35);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x0015525C File Offset: 0x0015345C
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddMenus(campaignGameStarter);
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x00155268 File Offset: 0x00153468
		private void AddMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("request_meeting_parley", "{=pBAx7jTM}With whom do you want to meet?", new OnInitDelegate(this.game_menu_town_menu_request_meeting_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("request_meeting_parley", "request_meeting_with", "{=!}{HERO_TO_MEET.LINK}", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_with_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_with_on_consequence), false, -1, true, null);
			campaignGameStarter.AddGameMenuOption("request_meeting_parley", "meeting_town_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_meeting_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_town_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenuOption("request_meeting_parley", "meeting_castle_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_meeting_castle_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_castle_leave_on_consequence), true, -1, false, null);
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x00155328 File Offset: 0x00153528
		private void game_menu_town_menu_request_meeting_on_init(MenuCallbackArgs args)
		{
			List<Hero> heroesToMeetInTown = TownHelpers.GetHeroesToMeetInTown(this._parleyedParty.Settlement);
			args.MenuContext.SetRepeatObjectList(heroesToMeetInTown);
			args.MenuContext.SetBackgroundMeshName(this._parleyedParty.Settlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x00155374 File Offset: 0x00153574
		private bool game_menu_request_meeting_with_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			Hero hero = args.MenuContext.GetCurrentRepeatableObject() as Hero;
			if (this._parleyedParty != null && hero != null)
			{
				StringHelpers.SetCharacterProperties("HERO_TO_MEET", hero.CharacterObject, null, false);
				MenuHelper.SetIssueAndQuestDataForHero(args, hero);
				return true;
			}
			return false;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x001553C2 File Offset: 0x001535C2
		private void game_menu_request_meeting_town_leave_on_consequence(MenuCallbackArgs args)
		{
			this.SettlementMenuLeaveConsequenceCommon();
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x001553CA File Offset: 0x001535CA
		private void game_menu_request_meeting_castle_leave_on_consequence(MenuCallbackArgs args)
		{
			this.SettlementMenuLeaveConsequenceCommon();
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x001553D2 File Offset: 0x001535D2
		private void SettlementMenuLeaveConsequenceCommon()
		{
			GameMenu.ExitToLast();
			this._parleyedParty = null;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x001553E0 File Offset: 0x001535E0
		private void game_menu_request_meeting_with_on_consequence(MenuCallbackArgs args)
		{
			string text;
			string meetingScene = this.GetMeetingScene(out text);
			Hero hero = (Hero)args.MenuContext.GetSelectedObject();
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, PartyBase.MainParty, false, false, false, false, false, false);
			CharacterObject characterObject = hero.CharacterObject;
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(characterObject, (partyBelongedTo != null) ? partyBelongedTo.Party : null, true, false, false, false, false, false);
			CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, meetingScene, text, false);
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x00155450 File Offset: 0x00153650
		private string GetMeetingScene(out string sceneLevel)
		{
			string text = GameSceneDataManager.Instance.MeetingScenes.GetRandomElementWithPredicate<MeetingSceneData>((MeetingSceneData x) => x.Culture == this._parleyedParty.Settlement.Culture).SceneID;
			if (string.IsNullOrEmpty(text))
			{
				text = GameSceneDataManager.Instance.MeetingScenes.GetRandomElement<MeetingSceneData>().SceneID;
			}
			sceneLevel = "";
			if (this._parleyedParty.Settlement.IsFortification)
			{
				sceneLevel = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(this._parleyedParty.Settlement.Town.GetWallLevel());
			}
			return text;
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x001554E6 File Offset: 0x001536E6
		private bool game_meeting_town_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return this._parleyedParty.Settlement.IsTown;
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00155500 File Offset: 0x00153700
		private bool game_meeting_castle_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return this._parleyedParty.Settlement.IsCastle;
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0015551A File Offset: 0x0015371A
		public PartyBase GetParleyedParty()
		{
			return this._parleyedParty;
		}

		// Token: 0x0400142D RID: 5165
		private PartyBase _parleyedParty;
	}
}
