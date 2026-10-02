using System;
using SandBox.View.Map.Navigation.NavigationElements;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace StoryMode.View.Permissions
{
	// Token: 0x02000004 RID: 4
	public class StoryModePermissionsSystem
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002368 File Offset: 0x00000568
		private StoryModePermissionsSystem()
		{
			this.RegisterEvents();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002376 File Offset: 0x00000576
		public static void OnInitialize()
		{
			if (StoryModePermissionsSystem.Current == null)
			{
				StoryModePermissionsSystem.Current = new StoryModePermissionsSystem();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002389 File Offset: 0x00000589
		internal static void OnUnload()
		{
			if (StoryModePermissionsSystem.Current != null)
			{
				StoryModePermissionsSystem.Current.UnregisterEvents();
				StoryModePermissionsSystem.Current = null;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023A4 File Offset: 0x000005A4
		private void OnPartyScreenCharacterTalkPermission(PartyScreenCharacterTalkPermissionEvent obj)
		{
			bool flag = StoryModeManager.Current != null;
			StoryModeManager storyModeManager = StoryModeManager.Current;
			bool flag2;
			if (storyModeManager == null)
			{
				flag2 = false;
			}
			else
			{
				MainStoryLine mainStoryLine = storyModeManager.MainStoryLine;
				bool? flag3 = ((mainStoryLine != null) ? new bool?(mainStoryLine.TutorialPhase.IsCompleted) : null);
				bool flag4 = true;
				flag2 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
			}
			bool flag5 = flag2;
			if (flag && !flag5)
			{
				obj.IsTalkAvailable(false, new TextObject("{=epQYhd1A}Cannot talk to hero right now", null));
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000241A File Offset: 0x0000061A
		private void OnClanScreenPermission(ClanScreenPermissionEvent obj)
		{
			StoryModeManager storyModeManager = StoryModeManager.Current;
			if (storyModeManager != null && storyModeManager.MainStoryLine.IsPlayerInteractionRestricted)
			{
				obj.IsClanScreenAvailable(false, new TextObject("{=75nwCTEn}Clan Screen is disabled during Tutorial.", null));
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000244C File Offset: 0x0000064C
		private void OnSettlementOverlayTalkPermission(SettlementOverlayTalkPermissionEvent obj)
		{
			bool flag = StoryModeManager.Current != null;
			TutorialPhase instance = TutorialPhase.Instance;
			bool flag2 = instance != null && instance.TutorialQuestPhase >= TutorialQuestPhase.RecruitAndPurchaseStarted;
			StoryModeManager storyModeManager = StoryModeManager.Current;
			bool flag3;
			if (storyModeManager == null)
			{
				flag3 = false;
			}
			else
			{
				MainStoryLine mainStoryLine = storyModeManager.MainStoryLine;
				bool? flag4 = ((mainStoryLine != null) ? new bool?(mainStoryLine.TutorialPhase.IsCompleted) : null);
				bool flag5 = true;
				flag3 = (flag4.GetValueOrDefault() == flag5) & (flag4 != null);
			}
			bool flag6 = flag3;
			if (flag && !flag2 && !flag6)
			{
				obj.IsTalkAvailable(false, new TextObject("{=UjERCi2F}This feature is disabled.", null));
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024E0 File Offset: 0x000006E0
		private void OnSettlementOverlayQuickTalkPermission(SettlementOverylayQuickTalkPermissionEvent obj)
		{
			bool flag = StoryModeManager.Current != null;
			TutorialPhase instance = TutorialPhase.Instance;
			bool flag2 = instance != null && instance.TutorialQuestPhase >= TutorialQuestPhase.Finalized;
			StoryModeManager storyModeManager = StoryModeManager.Current;
			bool flag3;
			if (storyModeManager == null)
			{
				flag3 = false;
			}
			else
			{
				MainStoryLine mainStoryLine = storyModeManager.MainStoryLine;
				bool? flag4 = ((mainStoryLine != null) ? new bool?(mainStoryLine.TutorialPhase.IsCompleted) : null);
				bool flag5 = true;
				flag3 = (flag4.GetValueOrDefault() == flag5) & (flag4 != null);
			}
			bool flag6 = flag3;
			if (flag && !flag2 && !flag6)
			{
				obj.IsTalkAvailable(false, new TextObject("{=UjERCi2F}This feature is disabled.", null));
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002574 File Offset: 0x00000774
		private void OnSettlementOverlayLeaveMemberPermission(SettlementOverlayLeaveCharacterPermissionEvent obj)
		{
			bool flag = StoryModeManager.Current != null;
			TutorialPhase instance = TutorialPhase.Instance;
			bool flag2 = instance != null && instance.TutorialQuestPhase >= TutorialQuestPhase.RecruitAndPurchaseStarted;
			StoryModeManager storyModeManager = StoryModeManager.Current;
			bool flag3;
			if (storyModeManager == null)
			{
				flag3 = false;
			}
			else
			{
				MainStoryLine mainStoryLine = storyModeManager.MainStoryLine;
				bool? flag4 = ((mainStoryLine != null) ? new bool?(mainStoryLine.TutorialPhase.IsCompleted) : null);
				bool flag5 = true;
				flag3 = (flag4.GetValueOrDefault() == flag5) & (flag4 != null);
			}
			bool flag6 = flag3;
			if (flag && !flag2 && !flag6)
			{
				obj.IsLeaveAvailable(false, new TextObject("{=UjERCi2F}This feature is disabled.", null));
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002608 File Offset: 0x00000808
		private void OnLeaveKingdomPermissionEvent(LeaveKingdomPermissionEvent obj)
		{
			StoryModeManager storyModeManager = StoryModeManager.Current;
			if (((storyModeManager != null) ? storyModeManager.MainStoryLine.PlayerSupportedKingdom : null) != null && Clan.PlayerClan.Kingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
			{
				Action<bool, TextObject> isLeaveKingdomPossbile = obj.IsLeaveKingdomPossbile;
				if (isLeaveKingdomPossbile == null)
				{
					return;
				}
				isLeaveKingdomPossbile(true, new TextObject("{=WFNLizqL}You've supported a kingdom through main story line. Leaving this kingdom will fail your quest.{newline}{newline}Are you sure?", null));
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000266C File Offset: 0x0000086C
		private void RegisterEvents()
		{
			Game.Current.EventManager.RegisterEvent<ClanScreenPermissionEvent>(new Action<ClanScreenPermissionEvent>(this.OnClanScreenPermission));
			Game.Current.EventManager.RegisterEvent<PartyScreenCharacterTalkPermissionEvent>(new Action<PartyScreenCharacterTalkPermissionEvent>(this.OnPartyScreenCharacterTalkPermission));
			Game.Current.EventManager.RegisterEvent<SettlementOverlayTalkPermissionEvent>(new Action<SettlementOverlayTalkPermissionEvent>(this.OnSettlementOverlayTalkPermission));
			Game.Current.EventManager.RegisterEvent<SettlementOverylayQuickTalkPermissionEvent>(new Action<SettlementOverylayQuickTalkPermissionEvent>(this.OnSettlementOverlayQuickTalkPermission));
			Game.Current.EventManager.RegisterEvent<SettlementOverlayLeaveCharacterPermissionEvent>(new Action<SettlementOverlayLeaveCharacterPermissionEvent>(this.OnSettlementOverlayLeaveMemberPermission));
			Game.Current.EventManager.RegisterEvent<LeaveKingdomPermissionEvent>(new Action<LeaveKingdomPermissionEvent>(this.OnLeaveKingdomPermissionEvent));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000271C File Offset: 0x0000091C
		internal void UnregisterEvents()
		{
			Game.Current.EventManager.UnregisterEvent<ClanScreenPermissionEvent>(new Action<ClanScreenPermissionEvent>(this.OnClanScreenPermission));
			Game.Current.EventManager.RegisterEvent<PartyScreenCharacterTalkPermissionEvent>(new Action<PartyScreenCharacterTalkPermissionEvent>(this.OnPartyScreenCharacterTalkPermission));
			Game.Current.EventManager.UnregisterEvent<SettlementOverlayTalkPermissionEvent>(new Action<SettlementOverlayTalkPermissionEvent>(this.OnSettlementOverlayTalkPermission));
			Game.Current.EventManager.UnregisterEvent<SettlementOverylayQuickTalkPermissionEvent>(new Action<SettlementOverylayQuickTalkPermissionEvent>(this.OnSettlementOverlayQuickTalkPermission));
			Game.Current.EventManager.UnregisterEvent<SettlementOverlayLeaveCharacterPermissionEvent>(new Action<SettlementOverlayLeaveCharacterPermissionEvent>(this.OnSettlementOverlayLeaveMemberPermission));
			Game.Current.EventManager.UnregisterEvent<LeaveKingdomPermissionEvent>(new Action<LeaveKingdomPermissionEvent>(this.OnLeaveKingdomPermissionEvent));
		}

		// Token: 0x04000002 RID: 2
		private static StoryModePermissionsSystem Current;
	}
}
