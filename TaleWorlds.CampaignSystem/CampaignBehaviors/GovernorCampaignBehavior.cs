using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000413 RID: 1043
	public class GovernorCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600426E RID: 17006 RVA: 0x0012ECA4 File Offset: 0x0012CEA4
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x0012ED24 File Offset: 0x0012CF24
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 0))
			{
				foreach (Town town in Town.AllFiefs)
				{
					if (town.Governor != null && town != town.Governor.GovernorOf)
					{
						town.Governor = null;
					}
				}
			}
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x0012EDA4 File Offset: 0x0012CFA4
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x0012EDB0 File Offset: 0x0012CFB0
		private void DailyTickSettlement(Settlement settlement)
		{
			if ((settlement.IsTown || settlement.IsCastle) && settlement.Town.Governor != null)
			{
				Hero governor = settlement.Town.Governor;
				if (MBRandom.RandomFloat <= DefaultPerks.Charm.InBloom.SecondaryBonus && governor.GetPerkValue(DefaultPerks.Charm.InBloom))
				{
					Hero randomElementWithPredicate = settlement.Notables.GetRandomElementWithPredicate<Hero>((Hero x) => x.IsFemale != governor.IsFemale);
					if (randomElementWithPredicate != null)
					{
						int num = 1;
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(governor.Clan.Leader, randomElementWithPredicate, num, true);
					}
				}
				if (MBRandom.RandomFloat <= DefaultPerks.Charm.YoungAndRespectful.SecondaryBonus && governor.GetPerkValue(DefaultPerks.Charm.YoungAndRespectful))
				{
					Hero randomElementWithPredicate2 = settlement.Notables.GetRandomElementWithPredicate<Hero>((Hero x) => x.IsFemale == governor.IsFemale);
					if (randomElementWithPredicate2 != null)
					{
						int num2 = 1;
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(governor.Clan.Leader, randomElementWithPredicate2, num2, true);
					}
				}
				if (MBRandom.RandomFloat <= DefaultPerks.Charm.MeaningfulFavors.SecondaryBonus && governor.GetPerkValue(DefaultPerks.Charm.MeaningfulFavors))
				{
					foreach (Hero hero in settlement.Notables)
					{
						if (hero.Power >= 200f)
						{
							int num3 = 1;
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero, num3, true);
						}
					}
				}
				SkillLevelingManager.OnSettlementGoverned(governor, settlement);
			}
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x0012EF40 File Offset: 0x0012D140
		private void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			if (hero.GovernorOf != null && hero.GovernorOf.OwnerClan != hero.Clan)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x0012EF63 File Offset: 0x0012D163
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0012EF68 File Offset: 0x0012D168
		private void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddPlayerLine("governor_talk_start", "hero_main_options", "governor_talk_start_reply", "{=zBo78JQb}How are things doing here in {GOVERNOR_SETTLEMENT}?", new ConversationSentence.OnConditionDelegate(this.governor_talk_start_on_condition), null, 100, null, null);
			starter.AddDialogLine("governor_talk_start_reply", "governor_talk_start_reply", "lord_pretalk", "{=!}{SETTLEMENT_DESCRIPTION}", new ConversationSentence.OnConditionDelegate(this.governor_talk_start_reply_on_condition), null, 200, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_start", "hero_main_options", "governor_kingdom_creation_reply", "{=EKuB6Ohf}It is time to take a momentous step... It is time to proclaim a new kingdom.", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_start_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_start_on_consequence), 200, new ConversationSentence.OnClickableConditionDelegate(this.governor_talk_kingdom_creation_start_clickable_condition), null);
			starter.AddDialogLine("governor_talk_kingdom_creation_reply", "governor_kingdom_creation_reply", "governor_kingdom_creation_culture_selection", "{=ZyNjXUHc}I am at your command.", null, null, 100, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_culture_selection", "governor_kingdom_creation_culture_selection", "governor_kingdom_creation_culture_selection_options", "{=jxEVSu98}The language of our documents, and our customary laws... Whose should we use?", null, null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_option", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selected", "{CULTURE_OPTION_0}", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_option_0_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_option_0_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_option_2", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selected", "{CULTURE_OPTION_1}", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_option_1_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_option_1_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_option_3", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selected", "{CULTURE_OPTION_2}", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_option_2_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_option_2_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_other", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selection", "{=kcuNzSvf}I have another people in mind.", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_other_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_other_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_cancel", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_exit", "{=hbzs5tLd}On second thought, perhaps now is not the right time.", null, null, 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_exit_reply", "governor_kingdom_creation_exit", "close_window", "{=ppi6eVos}As you wish.", null, null, 100, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_culture_selected", "governor_kingdom_creation_culture_selected", "governor_kingdom_creation_culture_selected_confirmation", "{=VOtKthQU}Yes. A kingdom using {CULTURE_ADJECTIVE} law would institute the following: {INITIAL_POLICY_NAMES}.", new ConversationSentence.OnConditionDelegate(this.governor_kingdom_creation_culture_selected_on_condition), null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selected_player_reply", "governor_kingdom_creation_culture_selected_confirmation", "governor_kingdom_creation_name_selection", "{=dzXaXKaC}Very well.", null, null, 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selected_player_reply_2", "governor_kingdom_creation_culture_selected_confirmation", "governor_kingdom_creation_culture_selection", "{=kTjsx8gN}Perhaps we should choose another set of laws and customs.", null, null, 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_name_selection", "governor_kingdom_creation_name_selection", "governor_kingdom_creation_name_selection_response", "{=wT1ducZX}Now. What will the kingdom be called?", null, null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_name_selection_player", "governor_kingdom_creation_name_selection_response", "governor_kingdom_creation_name_selection_prompted", "{=XRoG766S}I'll name it...", null, new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_name_selection_on_consequence), 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_name_selection_response", "governor_kingdom_creation_name_selection_prompted", "governor_kingdom_creation_name_selected", "{=shf5aY3l}I'm listening...", null, null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_name_selection_cancel", "governor_kingdom_creation_name_selection_response", "governor_kingdom_creation_exit", "{=7HpfrmIU}On a second thought... Now is not the right time to do this.", null, null, 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_name_selection_final_response", "governor_kingdom_creation_name_selected", "governor_kingdom_creation_finalization", "{=CzJZ5zhT}So it shall be proclaimed throughout your domain. May {KINGDOM_NAME} forever be victorious!", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_finalization_on_condition), null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_finalization", "governor_kingdom_creation_finalization", "close_window", "{=VRbbIWNf}So it shall be.", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_finalization_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_finalization_on_consequence), 100, null, null);
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x0012F2CD File Offset: 0x0012D4CD
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (victim.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(victim);
			}
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0012F2E0 File Offset: 0x0012D4E0
		private bool governor_talk_start_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.GovernorOf != null && Hero.OneToOneConversationHero.CurrentSettlement != null && Hero.OneToOneConversationHero.CurrentSettlement.IsTown && Hero.OneToOneConversationHero.CurrentSettlement.Town == Hero.OneToOneConversationHero.GovernorOf && Hero.OneToOneConversationHero.GovernorOf.Owner.Owner == Hero.MainHero)
			{
				MBTextManager.SetTextVariable("GOVERNOR_SETTLEMENT", Hero.OneToOneConversationHero.CurrentSettlement.Name, false);
				return true;
			}
			return false;
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x0012F370 File Offset: 0x0012D570
		private bool governor_talk_start_reply_on_condition()
		{
			Settlement currentSettlement = Hero.OneToOneConversationHero.CurrentSettlement;
			TextObject textObject = TextObject.GetEmpty();
			switch (currentSettlement.Town.GetProsperityLevel())
			{
			case SettlementComponent.ProsperityLevel.Low:
				textObject = new TextObject("{=rbJEuVKg}Things could certainly be better, my {?HERO.GENDER}lady{?}lord{\\?}. The merchants say business is slow, and the people complain that goods are expensive and in short supply.", null);
				break;
			case SettlementComponent.ProsperityLevel.Mid:
				textObject = new TextObject("{=HgdbSrq9}Things are all right, my {?HERO.GENDER}lady{?}lord{\\?}. The merchants say that they are breaking even, for the most part. Some prices are high, but most of what the people need is available.", null);
				break;
			case SettlementComponent.ProsperityLevel.High:
				textObject = new TextObject("{=8G94SlPD}We are doing well, my {?HERO.GENDER}lady{?}lord{\\?}. The merchants say business is brisk, and everything the people need appears to be in good supply.", null);
				break;
			}
			StringHelpers.SetCharacterProperties("HERO", CharacterObject.PlayerCharacter, textObject, false);
			MBTextManager.SetTextVariable("SETTLEMENT_DESCRIPTION", textObject.ToString(), false);
			return true;
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x0012F3F8 File Offset: 0x0012D5F8
		private bool governor_talk_kingdom_creation_start_on_condition()
		{
			return Clan.PlayerClan.Kingdom == null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.GovernorOf != null && Hero.OneToOneConversationHero.GovernorOf.Settlement.MapFaction == Hero.MainHero.MapFaction;
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x0012F446 File Offset: 0x0012D646
		private void governor_talk_kingdom_creation_start_on_consequence()
		{
			this._availablePlayerKingdomCultures.Clear();
			this._availablePlayerKingdomCultures = Campaign.Current.Models.KingdomCreationModel.GetAvailablePlayerKingdomCultures().ToList<CultureObject>();
			this._kingdomCreationCurrentCulturePageIndex = 0;
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0012F47C File Offset: 0x0012D67C
		private bool governor_talk_kingdom_creation_start_clickable_condition(out TextObject explanation)
		{
			List<TextObject> list;
			bool flag = Campaign.Current.Models.KingdomCreationModel.IsPlayerKingdomCreationPossible(out list);
			string text = "";
			foreach (TextObject textObject in list)
			{
				text += textObject;
				if (textObject != list[list.Count - 1])
				{
					text += "\n";
				}
			}
			explanation = new TextObject(text, null);
			return flag;
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0012F518 File Offset: 0x0012D718
		private bool governor_talk_kingdom_creation_culture_option_0_on_condition()
		{
			return this.HandleAvailableCultureConditionAndText(0);
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0012F521 File Offset: 0x0012D721
		private bool governor_talk_kingdom_creation_culture_option_1_on_condition()
		{
			return this.HandleAvailableCultureConditionAndText(1);
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0012F52A File Offset: 0x0012D72A
		private bool governor_talk_kingdom_creation_culture_option_2_on_condition()
		{
			return this.HandleAvailableCultureConditionAndText(2);
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x0012F534 File Offset: 0x0012D734
		private bool HandleAvailableCultureConditionAndText(int index)
		{
			int cultureIndex = this.GetCultureIndex(index);
			if (this._availablePlayerKingdomCultures.Count > cultureIndex)
			{
				TextObject textObject = new TextObject("{=mY6DbVfc}The language and laws of {CULTURE_NAME}.", null);
				textObject.SetTextVariable("CULTURE_NAME", FactionHelper.GetInformalNameForFactionCulture(this._availablePlayerKingdomCultures[cultureIndex]));
				MBTextManager.SetTextVariable("CULTURE_OPTION_" + index.ToString(), textObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x0012F59B File Offset: 0x0012D79B
		private int GetCultureIndex(int optionIndex)
		{
			return this._kingdomCreationCurrentCulturePageIndex * 3 + optionIndex;
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x0012F5A7 File Offset: 0x0012D7A7
		private void governor_talk_kingdom_creation_culture_option_0_on_consequence()
		{
			this._kingdomCreationChosenCulture = this._availablePlayerKingdomCultures[this.GetCultureIndex(0)];
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x0012F5C1 File Offset: 0x0012D7C1
		private void governor_talk_kingdom_creation_culture_option_1_on_consequence()
		{
			this._kingdomCreationChosenCulture = this._availablePlayerKingdomCultures[this.GetCultureIndex(1)];
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x0012F5DB File Offset: 0x0012D7DB
		private void governor_talk_kingdom_creation_culture_option_2_on_consequence()
		{
			this._kingdomCreationChosenCulture = this._availablePlayerKingdomCultures[this.GetCultureIndex(2)];
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x0012F5F5 File Offset: 0x0012D7F5
		private bool governor_talk_kingdom_creation_culture_other_on_condition()
		{
			return this._availablePlayerKingdomCultures.Count > 3;
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x0012F605 File Offset: 0x0012D805
		private void governor_talk_kingdom_creation_culture_other_on_consequence()
		{
			this._kingdomCreationCurrentCulturePageIndex++;
			if (this._kingdomCreationCurrentCulturePageIndex > MathF.Ceiling((float)this._availablePlayerKingdomCultures.Count / 3f) - 1)
			{
				this._kingdomCreationCurrentCulturePageIndex = 0;
			}
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x0012F640 File Offset: 0x0012D840
		private bool governor_kingdom_creation_culture_selected_on_condition()
		{
			TextObject textObject = GameTexts.GameTextHelper.MergeTextObjectsWithComma(this._kingdomCreationChosenCulture.DefaultPolicyList.Select<PolicyObject, TextObject>((PolicyObject t) => t.Name).ToList<TextObject>(), true);
			MBTextManager.SetTextVariable("INITIAL_POLICY_NAMES", textObject, false);
			MBTextManager.SetTextVariable("CULTURE_ADJECTIVE", FactionHelper.GetAdjectiveForFactionCulture(this._kingdomCreationChosenCulture), false);
			return true;
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x0012F6AC File Offset: 0x0012D8AC
		private void governor_talk_kingdom_creation_name_selection_on_consequence()
		{
			this._kingdomCreationChosenName = TextObject.GetEmpty();
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=RuaA8t97}Kingdom Name", null).ToString(), string.Empty, true, true, GameTexts.FindText("str_done", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action<string>(this.OnKingdomNameSelectionDone), new Action(this.OnKingdomNameSelectionCancel), false, new Func<string, Tuple<bool, string>>(FactionHelper.IsKingdomNameApplicable), "", ""), false, false);
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x0012F736 File Offset: 0x0012D936
		private void OnKingdomNameSelectionDone(string chosenName)
		{
			this._kingdomCreationChosenName = new TextObject(chosenName, null);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x0012F754 File Offset: 0x0012D954
		private void OnKingdomNameSelectionCancel()
		{
			Campaign.Current.ConversationManager.EndConversation();
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x0012F765 File Offset: 0x0012D965
		private bool governor_talk_kingdom_creation_finalization_on_condition()
		{
			MBTextManager.SetTextVariable("KINGDOM_NAME", this._kingdomCreationChosenName, false);
			return true;
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x0012F77C File Offset: 0x0012D97C
		private void governor_talk_kingdom_creation_finalization_on_consequence()
		{
			Campaign.Current.KingdomManager.CreateKingdom(this._kingdomCreationChosenName, this._kingdomCreationChosenName, this._kingdomCreationChosenCulture, Clan.PlayerClan, this._kingdomCreationChosenName, this._kingdomCreationChosenCulture.DefaultPolicyList, null, this._kingdomCreationChosenName, null, "new_kingdom", null, null, null, null, null);
		}

		// Token: 0x040013D9 RID: 5081
		private const int CultureDialogueOptionCount = 3;

		// Token: 0x040013DA RID: 5082
		private List<CultureObject> _availablePlayerKingdomCultures = new List<CultureObject>();

		// Token: 0x040013DB RID: 5083
		private int _kingdomCreationCurrentCulturePageIndex;

		// Token: 0x040013DC RID: 5084
		private CultureObject _kingdomCreationChosenCulture;

		// Token: 0x040013DD RID: 5085
		private TextObject _kingdomCreationChosenName;
	}
}
