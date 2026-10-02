using System;
using System.Collections.Generic;
using Helpers;
using StoryMode.Missions;
using Storymode.Missions;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.TutorialPhase
{
	// Token: 0x02000024 RID: 36
	public class VillagersInNeed : StoryModeQuestBase
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000099F9 File Offset: 0x00007BF9
		private static int SettlementBusyPriority
		{
			get
			{
				return 400;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00009A00 File Offset: 0x00007C00
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=Cv2W7aFu}Villagers in Need", null);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009A0D File Offset: 0x00007C0D
		private TextObject _startQuestLogTutorialNotSkipped
		{
			get
			{
				TextObject textObject = new TextObject("{=sbX4fQ0R}A boy came to your camp and told you some of Radagos' men returned to {VILLAGE_LINK} and took the headman hostage.", null);
				textObject.SetTextVariable("VILLAGE_LINK", this._village.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00009A31 File Offset: 0x00007C31
		private TextObject _startQuestLogTutorialSkipped
		{
			get
			{
				TextObject textObject = new TextObject("{=Iu7tpHsO}A boy came to your camp and told you the villagers of {VILLAGE_LINK} need your help rescuing their headman from a group of bandits.", null);
				textObject.SetTextVariable("VILLAGE_LINK", this._village.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009A55 File Offset: 0x00007C55
		private Settlement _village
		{
			get
			{
				return Settlement.Find("village_ES3_2");
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00009A61 File Offset: 0x00007C61
		public CharacterObject Headman
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("tutorial_npc_captive_headman");
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00009A72 File Offset: 0x00007C72
		private CharacterObject _villager
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("tutorial_npc_questgiver_villager");
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009A83 File Offset: 0x00007C83
		public VillagersInNeed()
			: base("talk_to_villagers_in_village_quest", null, CampaignTime.Never)
		{
			base.AddTrackedObject(this._village);
			this.SetDialogs();
			this.AddGameMenus();
			base.InitializeQuestOnCreation();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009AB4 File Offset: 0x00007CB4
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009ABC File Offset: 0x00007CBC
		protected override void OnStartQuest()
		{
			base.AddLog(TutorialPhase.Instance.IsSkipped ? this._startQuestLogTutorialSkipped : this._startQuestLogTutorialNotSkipped, false);
			Hero.AllAliveHeroes.GetRandomElementWithPredicate<Hero>((Hero t) => t.Occupation == Occupation.Headman && t.Culture == this._village.Culture);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009AF8 File Offset: 0x00007CF8
		protected override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.GameMenuOpened));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
			CampaignEvents.IsSettlementBusyEvent.AddNonSerializedListener(this, new ReferenceAction<Settlement, object, int>(this.IsSettlementBusy));
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009B78 File Offset: 0x00007D78
		private void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
		{
			if (settlement == this._village && asker != this)
			{
				priority = Math.Max(priority, VillagersInNeed.SettlementBusyPriority);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009B95 File Offset: 0x00007D95
		private void OnMissionEnded(IMission mission)
		{
			this._isHeadmanFollowing = false;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009BA0 File Offset: 0x00007DA0
		private void GameMenuOpened(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement == this._village && args.MenuContext.GameMenu.StringId == "village")
			{
				if (this._startVillaMission)
				{
					this.StartVillaMission();
					return;
				}
				if (!this._talkedToVillagers && !this._firstConversationWithVillagerOpened && !this._startVillaMission)
				{
					this.OpenConversationWithVillager();
					return;
				}
				if (this._rescuedHeadman)
				{
					this.OpenConversationWithHeadman();
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009C12 File Offset: 0x00007E12
		private void OnGameLoadFinished()
		{
			this.AddGameMenus();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009C1A File Offset: 0x00007E1A
		private void AddGameMenus()
		{
			base.AddGameMenuOption("village", "talk_to_villager", new TextObject("{=Q5jUW8Oa}Talk to the villager", null), new GameMenuOption.OnConditionDelegate(this.village_talk_to_villager_on_condition), new GameMenuOption.OnConsequenceDelegate(this.village_talk_to_villager_on_consequence), false, 4);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009C51 File Offset: 0x00007E51
		private void village_talk_to_villager_on_consequence(MenuCallbackArgs args)
		{
			this.OpenConversationWithVillager();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009C5C File Offset: 0x00007E5C
		private bool village_talk_to_villager_on_condition(MenuCallbackArgs args)
		{
			args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveStoryQuest;
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			if (Hero.MainHero.IsWounded)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=yNMrF2QF}You are wounded", null);
			}
			bool flag = Settlement.CurrentSettlement == this._village && this._talkedToVillagers;
			if (CampaignTime.Now.IsDayTime)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=8RJpNhWh}Come back at night time to talk to the villager.", null);
			}
			return flag;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00009CDC File Offset: 0x00007EDC
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (!CampaignTime.Now.IsDayTime && !this._talkedToVillagers && settlement == this._village && party == MobileParty.MainParty)
			{
				this.OpenConversationWithVillager();
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009D18 File Offset: 0x00007F18
		private void OpenConversationWithVillager()
		{
			CampaignMission.OpenConversationMission(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, false, false, false, false, false), new ConversationCharacterData(this._villager, null, true, true, false, false, false, false), "", "", false);
			this._firstConversationWithVillagerOpened = true;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009D60 File Offset: 0x00007F60
		private void OpenConversationWithHeadman()
		{
			CampaignMission.OpenConversationMission(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, false, false, false, false, false), new ConversationCharacterData(this.Headman, null, true, true, false, false, false, false), "", "", false);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00009DA4 File Offset: 0x00007FA4
		protected override void SetDialogs()
		{
			string text;
			string text2;
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=!}{VILLAGER_DIALOGUE_1}", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_villagers_on_condition))
				.NpcLine(new TextObject("{=!}{VILLAGER_DIALOGUE_2}", null), null, null, null, null)
				.NpcLine(new TextObject("{=vxYaxWwC}They've holed up in a ruined villa a short distance from here, and say that if we try to rescue the headman they'll cut his throat then and there. But surely you could save him? You could sneak in there and get him out?", null), null, null, null, null)
				.GenerateToken(out text)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=3sI7nPbF}Hmm. I guess it's mostly a matter of waiting until their back is turned, then moving quickly from cover to cover.", null), null, null, text)
				.PlayerOption(new TextObject("{=mFTTWH03}I shall pass through them unseen, cloaked in silence and shadow.", null), null, null, text)
				.EndPlayerOptions()
				.NpcLine(new TextObject("{=n5fELaJd}We can give you some things that might help you. We have some special, softer boots, that our hunters use when they go out at night - when you walk, you'll barely make any noise at all. And some darkened clothes. We have some that would fit you.", null), null, null, text, null)
				.NpcLine(new TextObject("{=3ee3I8WX}Also, this dagger... It's probably safest just to get to the headman as stealthily as you can, and then sneak back out. But if there's just no getting around one of them, you can take this and come up behind him, and that would make a lot less noise than a straight-out fight.", null), null, null, null, null)
				.GenerateToken(out text2)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=HpYpZEt3}Right. I'll don those hunting clothes and see what I can do.", null), null, null, text2)
				.PlayerOption(new TextObject("{=q4bjsoKj}Let this dagger be the hand of the angel of death.", null), null, null, text2)
				.EndPlayerOptions()
				.NpcLine(new TextObject("{=4YDqx6uZ}Thank you, my {?PLAYER.GENDER}lady{?}lord{\\?}. I'll take you to the villa so that we arrive at night. I doubt they'll be on their guard given all the wine they have stolen from us.", null), null, null, text2, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.talk_to_villagers_not_skipped_on_consequence))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=glarczej}Lead on.", null), null, null, null)
				.Condition(() => CampaignTime.Now.IsNightTime)
				.Consequence(delegate
				{
					this._startVillaMission = true;
				})
				.CloseDialog()
				.PlayerOption(new TextObject("{=nhSLTzHk}I have to take care of something else, first.", null), null, null, null)
				.Condition(() => CampaignTime.Now.IsNightTime)
				.CloseDialog()
				.PlayerOption(new TextObject("{=OWUiQbHx}I will come back at night.", null), null, null, null)
				.Condition(() => !CampaignTime.Now.IsNightTime)
				.CloseDialog()
				.EndPlayerOptions(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=AZ30Q0nM}Heaven bless you, {?PLAYER.GENDER}madame{?}sir{\\?}. Shall I take you to where the bandits are holding the headman?", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_villagers_later_on_condition))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=glarczej}Lead on.", null), null, null, null)
				.Consequence(delegate
				{
					this._startVillaMission = true;
				})
				.CloseDialog()
				.PlayerOption(new TextObject("{=nhSLTzHk}I have to take care of something else, first.", null), null, null, null)
				.CloseDialog()
				.EndPlayerOptions(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=IGdpap9P}We saw what happened, {?PLAYER.GENDER}madame{?}sir{\\?}, but we think that drunken lot have all gone to sleep again. Maybe you could try again, {?PLAYER.GENDER}madame{?}sir{\\?}? We would be forever in your debt.", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_villagers_failed_on_condition))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=glarczej}Lead on.", null), null, null, null)
				.Consequence(delegate
				{
					this._startVillaMission = true;
					this._failedTheMission = false;
				})
				.NpcLine(new TextObject("{=76acv5m2}Whatever happens, we're forever in your debt.", null), null, null, null, null)
				.CloseDialog()
				.PlayerOption(new TextObject("{=nhSLTzHk}I have to take care of something else, first.", null), null, null, null)
				.NpcLine(new TextObject("{=krsbwYax}Come find us here in Tevea when you're ready, {?PLAYER.GENDER}madame{?}sir{\\?}.", null), null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=7aAFEx7e}You're not one of them! Who are you? What's happening?", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_headman_in_villa_skipped_on_condition))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=bcZaWOZM}I'll find a way out. Follow me as soon as it’s safe.", null), null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.talk_to_headman_in_villa_on_consequence))
				.CloseDialog()
				.PlayerOption(new TextObject("{=nfMWzDbw}Be silent! I shall clear a path past them.", null), null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.talk_to_headman_in_villa_on_consequence))
				.CloseDialog()
				.EndPlayerOptions(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=dykrJl5v}{PLAYER.NAME}! What's happening?", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_headman_in_villa_not_skipped_on_condition))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=bcZaWOZM}I'll find a way out. Follow me as soon as it’s safe.", null), null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.talk_to_headman_in_villa_on_consequence))
				.CloseDialog()
				.PlayerOption(new TextObject("{=nfMWzDbw}Be silent! I shall clear a path past them.", null), null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.talk_to_headman_in_villa_on_consequence))
				.CloseDialog()
				.EndPlayerOptions(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=bN3zJKz5}As soon as you find an escape route, I will follow.", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_headman_in_villa_after_talking_on_condition))
				.CloseDialog(), this);
			string text3;
			string text4;
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=!}{HEADMAN_DIALOGUE_1}", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.talk_to_headman_after_rescue_on_condition))
				.GenerateToken(out text3)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=xJUJmrTb}I'm always glad to help honest folk like yourselves.", null), null, null, text3)
				.PlayerOption(new TextObject("{=y3gl2ada}Perhaps you would like to express a more tangible form of gratitude.", null), null, null, text3)
				.EndPlayerOptions()
				.NpcLine(new TextObject("{=!}{HEADMAN_DIALOGUE_2}", null), null, null, text3, null)
				.GenerateToken(out text4)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=gLVaQeAL}I'll take it. I need whatever I can get.", null), null, null, text4)
				.Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += this.TakeRewards;
				})
				.PlayerOption(new TextObject("{=xj5dlLXa}Keep your money, my good man. You've lost too much already.", null), null, null, text4)
				.EndPlayerOptions()
				.NpcLine(new TextObject("{=x3vZ8iQC}Then thank you again. We here in Tevea won't forget what you've done for us.", null), null, null, text4, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(base.CompleteQuestWithSuccess))
				.CloseDialog(), this);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A38C File Offset: 0x0000858C
		protected override void OnCompleteWithSuccess()
		{
			TextObject textObject = new TextObject("{=LcPHw4m2}You rescued the headman from the bandits, and returned him to the village of {VILLAGE_LINK}.", null);
			textObject.SetTextVariable("VILLAGE_LINK", this._village.EncyclopediaLinkWithName);
			base.AddLog(textObject, false);
			MBEquipmentRoster @object = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("stealth_tutorial_set_player");
			for (int i = 0; i < 12; i++)
			{
				if (!@object.DefaultEquipment[i].IsEmpty)
				{
					MobileParty.MainParty.ItemRoster.AddToCounts(@object.DefaultEquipment[i], 1);
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A415 File Offset: 0x00008615
		private void talk_to_headman_in_villa_on_consequence()
		{
			Mission.Current.GetMissionBehavior<SneakIntoTheVillaMissionController>().OnAfterTalkingToPrisoner();
			this._isHeadmanFollowing = true;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A42D File Offset: 0x0000862D
		private bool talk_to_headman_in_villa_on_condition()
		{
			bool flag = CharacterObject.OneToOneConversationCharacter == this.Headman && this._talkedToVillagers && !this._isHeadmanFollowing && !this._rescuedHeadman;
			if (flag)
			{
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			}
			return flag;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A46D File Offset: 0x0000866D
		private bool talk_to_headman_in_villa_after_talking_on_condition()
		{
			bool flag = CharacterObject.OneToOneConversationCharacter == this.Headman && this._talkedToVillagers && this._isHeadmanFollowing && !this._rescuedHeadman;
			if (flag)
			{
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			}
			return flag;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A4AD File Offset: 0x000086AD
		private bool talk_to_headman_in_villa_skipped_on_condition()
		{
			return this.talk_to_headman_in_villa_on_condition() && TutorialPhase.Instance.IsSkipped;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A4C3 File Offset: 0x000086C3
		private bool talk_to_headman_in_villa_not_skipped_on_condition()
		{
			return this.talk_to_headman_in_villa_on_condition() && !TutorialPhase.Instance.IsSkipped;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A4DC File Offset: 0x000086DC
		private void TakeRewards()
		{
			GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 100, false);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A4EC File Offset: 0x000086EC
		private bool talk_to_headman_after_rescue_on_condition()
		{
			bool flag = CharacterObject.OneToOneConversationCharacter == this.Headman && this._talkedToVillagers && this._rescuedHeadman;
			if (flag)
			{
				if (TutorialPhase.Instance.IsSkipped)
				{
					MBTextManager.SetTextVariable("HEADMAN_DIALOGUE_1", new TextObject("{=sbqpaU64}Thank you, {?PLAYER.GENDER}madame{?}sir{\\?}! You saved my life, and put the fear of Heaven into those villains, I'm sure!  We can handle things from here. There's just a few of them left, and we won't let them take us unaware again. With all our hearts, thank you.", null), false);
				}
				else
				{
					MBTextManager.SetTextVariable("HEADMAN_DIALOGUE_1", new TextObject("{=L5KshziU}{PLAYER.NAME}... Once again, you've helped us fend off those villains. We can handle things from here, I'm sure. There's just a few of them left, and we won't let them take us unaware again. Thank you. With all our hearts, thank you.", null), false);
				}
				if (TutorialPhase.Instance.IsSkipped)
				{
					MBTextManager.SetTextVariable("HEADMAN_DIALOGUE_2", new TextObject("{=9IHnjuXb}We'd heard that you're trying to find your family. Our heart goes out to you, {?PLAYER.GENDER}madame{?}sir{\\?}. That dagger and those hunting clothes - please take them. We pray they can be of use. And I have 100 denars that I'd been saving, but I want you to have it. If it helps you at all, I'd be glad.", null), false);
				}
				else
				{
					MBTextManager.SetTextVariable("HEADMAN_DIALOGUE_2", new TextObject("{=LRkKxcmX}We know you've got a long road ahead of you, trying to find your family. That dagger and those hunting clothes - please take them. We pray they can be of use. And I have 100 denars that I'd been saving, but I want you to have it. If it helps you at all to find your poor brother and sister, I'd be glad.", null), false);
				}
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			}
			return flag;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000A5A4 File Offset: 0x000087A4
		private void talk_to_villagers_not_skipped_on_consequence()
		{
			TextObject textObject = new TextObject("{=4ezrToWI}You agreed to help the villagers and try to save their headman from a nearby villa.", null);
			base.AddLog(textObject, false);
			this._talkedToVillagers = true;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000A5CD File Offset: 0x000087CD
		private void StartVillaMission()
		{
			StoryModeMissions.OpenSneakIntoTheVillaMission("villa_singular_c", CampaignTime.Now, null);
			this._startVillaMission = false;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A5E8 File Offset: 0x000087E8
		private bool talk_to_villagers_later_on_condition()
		{
			bool flag = Mission.Current != null && CharacterObject.OneToOneConversationCharacter != null && Settlement.CurrentSettlement == this._village && !this._failedTheMission && this._talkedToVillagers && CharacterObject.OneToOneConversationCharacter == this._villager;
			if (flag)
			{
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			}
			return flag;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000A648 File Offset: 0x00008848
		private bool talk_to_villagers_failed_on_condition()
		{
			bool flag = Mission.Current != null && CharacterObject.OneToOneConversationCharacter != null && Settlement.CurrentSettlement == this._village && this._talkedToVillagers && this._failedTheMission && CharacterObject.OneToOneConversationCharacter == this._villager;
			if (flag)
			{
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			}
			return flag;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A6A8 File Offset: 0x000088A8
		private bool talk_to_villagers_on_condition()
		{
			bool flag = Mission.Current != null && CharacterObject.OneToOneConversationCharacter != null && Settlement.CurrentSettlement == this._village && !this._talkedToVillagers && CharacterObject.OneToOneConversationCharacter == this._villager;
			if (flag)
			{
				if (TutorialPhase.Instance.IsSkipped)
				{
					MBTextManager.SetTextVariable("VILLAGER_DIALOGUE_1", new TextObject("{=toszqdCj}Thank Heaven our lad found you! Please, {?PLAYER.GENDER}madame{?}sir{\\?}, we'd heard about that terrible affair at the inn, and that a couple of warriors were planning to track those killers down. Are you one of them? We thought you could help us.", null), false);
				}
				else
				{
					MBTextManager.SetTextVariable("VILLAGER_DIALOGUE_1", new TextObject("{=PkoWqYPD}Thank Heaven our lad found you! Please, {?PLAYER.GENDER}madame{?}sir{\\?}, you've done so much for us, but we beg you not to forsake us now.", null), false);
				}
				if (TutorialPhase.Instance.IsSkipped)
				{
					MBTextManager.SetTextVariable("VILLAGER_DIALOGUE_2", new TextObject("{=55avOQ4k}Listen, {?PLAYER.GENDER}madame{?}sir{\\?}... It seems like a small group of bandits broke off from the main group, and now they have our headman. They're demanding a ransom - a half-dozen horses and ten sacks of grain. After all their theft and villainy we have no horses at all, sirs, and the grain would leave us nothing to plant!", null), false);
				}
				else
				{
					MBTextManager.SetTextVariable("VILLAGER_DIALOGUE_2", new TextObject("{=datALLCZ}When our lads came back, and said you'd led them to victory over Radagos and his gang, we thought the danger had passed. But it looks like we rejoiced too soon. A few desperate bandits got away, and now they have our headman. They're demanding a ransom - a half-dozen horses and ten sacks of grain. After all their theft and villainy we have no horses at all, sirs, and the grain would leave us nothing to plant!", null), false);
				}
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			}
			return flag;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A774 File Offset: 0x00008974
		public void OnRescueMissionFailed()
		{
			this._failedTheMission = true;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A77D File Offset: 0x0000897D
		public void OnHeadmanRescued()
		{
			this._rescuedHeadman = true;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A786 File Offset: 0x00008986
		internal static void AutoGeneratedStaticCollectObjectsVillagersInNeed(object o, List<object> collectedObjects)
		{
			((VillagersInNeed)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A794 File Offset: 0x00008994
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A79D File Offset: 0x0000899D
		internal static object AutoGeneratedGetMemberValue_talkedToVillagers(object o)
		{
			return ((VillagersInNeed)o)._talkedToVillagers;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A7AF File Offset: 0x000089AF
		internal static object AutoGeneratedGetMemberValue_failedTheMission(object o)
		{
			return ((VillagersInNeed)o)._failedTheMission;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A7C1 File Offset: 0x000089C1
		internal static object AutoGeneratedGetMemberValue_firstConversationWithVillagerOpened(object o)
		{
			return ((VillagersInNeed)o)._firstConversationWithVillagerOpened;
		}

		// Token: 0x040000A3 RID: 163
		public const string StealthEquipmentId = "stealth_tutorial_set_player";

		// Token: 0x040000A4 RID: 164
		public const string VillaSceneId = "villa_singular_c";

		// Token: 0x040000A5 RID: 165
		public const string HeadmanId = "tutorial_npc_captive_headman";

		// Token: 0x040000A6 RID: 166
		private const string VillagerId = "tutorial_npc_questgiver_villager";

		// Token: 0x040000A7 RID: 167
		[SaveableField(1)]
		private bool _talkedToVillagers;

		// Token: 0x040000A8 RID: 168
		[SaveableField(2)]
		private bool _failedTheMission;

		// Token: 0x040000A9 RID: 169
		[SaveableField(3)]
		private bool _firstConversationWithVillagerOpened;

		// Token: 0x040000AA RID: 170
		private bool _startVillaMission;

		// Token: 0x040000AB RID: 171
		private bool _isHeadmanFollowing;

		// Token: 0x040000AC RID: 172
		private bool _rescuedHeadman;
	}
}
