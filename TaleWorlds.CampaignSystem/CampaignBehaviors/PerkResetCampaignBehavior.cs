using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000450 RID: 1104
	public class PerkResetCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x00159A5E File Offset: 0x00157C5E
		public int PerkResetCost
		{
			get
			{
				if (this._selectedSkillForReset == null)
				{
					return 0;
				}
				return this._heroForPerkReset.GetSkillValue(this._selectedSkillForReset) * 40;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x00159A7E File Offset: 0x00157C7E
		public bool HasEnoughSkillValueForReset
		{
			get
			{
				return this._selectedSkillForReset != null && this._heroForPerkReset.GetSkillValue(this._selectedSkillForReset) >= 25;
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x00159AA4 File Offset: 0x00157CA4
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.PerkResetEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerkReset));
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x00159AF6 File Offset: 0x00157CF6
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_warningTime", ref this._warningTime);
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x00159B0A File Offset: 0x00157D0A
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x00159B14 File Offset: 0x00157D14
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("arena_intro_7", "arena_intro_perk_reset", "arena_intro_4", "{=ocIutUyu}Also, here at the arena, we think a lot about the arts of war - and many other related skills as well. Often you pick up certain habits while learning your skills. If you need to change those, to practice one of your skills in a certain way, we can help you.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_player_perk_reset", "arena_master_talk", "arena_master_ask_retrain", "{=Y7tz9D28}These teachers who help people hone their skills and learn new habits... Can you help me find one?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_ask_retrain", "arena_master_ask_retrain", "arena_master_choose_hero", "{=NyWXSHH2}Of course. Was this for you, or someone else?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_player_perk_reset_2", "arena_master_choose_hero", "arena_master_reset_attribute", "{=3VxA6HaZ}This is for me.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_player_for_perk_reset_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_clan_member_perk_reset", "arena_master_choose_hero", "arena_master_reset_attribute", "{=1OKEl18y}This is for {COMPANION.NAME}", new ConversationSentence.OnConditionDelegate(this.conversation_player_has_single_clan_member_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_player_has_single_clan_member_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_clan_member_perk_reset_2", "arena_master_choose_hero", "arena_master_retrain_ask_clan_members", "{=GvcotJmH}I would like you to help hone the skills of a member of my clan.", new ConversationSentence.OnConditionDelegate(this.conversation_player_has_multiple_clan_members_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_list_clan_members_on_condition), 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_retrain_ask_clan_member", "arena_master_retrain_ask_clan_members", "arena_master_select_clan_member", "{=WRwA0VVS}Which one of your clan members did you wish me to retrain?", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("arena_master_select_clan_member", "arena_master_select_clan_member", "arena_master_reset_attribute", "{=!}{CLAN_MEMBER.NAME}", "{=ElG1LnCA}I am thinking of someone else.", "arena_master_retrain_ask_clan_members", new ConversationSentence.OnConditionDelegate(this.conversation_arena_player_select_clan_member_multiple_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_clan_member_for_perk_reset_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_select_clan_member_cancel", "arena_master_select_clan_member", "arena_master_pre_talk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_reset_attribute", "arena_master_reset_attribute", "arena_master_select_attribute", "{=95jXfam8}What kind of skill is this, speaking broadly? What trait would you say it reflects?", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("arena_master_select_attribute", "arena_master_select_attribute", "arena_master_reset_perks", "{=!}{ATTRIBUTE_NAME}", "{=0G8Q3AZv}I am thinking of a different attribute.", "arena_master_reset_attribute", new ConversationSentence.OnConditionDelegate(this.conversation_arena_player_select_attribute_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_attribute_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_select_attribute_cancel", "arena_master_select_attribute", "arena_master_pre_talk", "{=g0JOQQl0}I don't want to do this right now.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_reset_perks", "arena_master_reset_perks", "arena_master_select_skill", "{=pGyO41lb}Yes, I can do that. What skill exactly do you have in mind?", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_set_skills_for_reset_on_consequence), 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("arena_master_select_skill", "arena_master_select_skill", "arena_master_pay_for_reset", "{=8PV1oB9W}I wish to focus on {SKILL_NAME}.", "{=Z9pq58h4}I am thinking of a different skill.", "arena_master_reset_perks", new ConversationSentence.OnConditionDelegate(this.conversation_arena_player_select_skill_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_skill_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_select_skill_cancel", "arena_master_select_skill", "arena_master_reset_attribute", "{=CH7b5LaX}I have changed my mind.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_list_perks_on_condition), 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_pay_for_reset", "arena_master_pay_for_reset", "arena_master_accept_perk_reset", "{=q3J9Wb8N}If you can afford to pay {GOLD_AMOUNT} {GOLD_ICON} for it, I can teach you right now. Are you sure you want to go through with it?", new ConversationSentence.OnConditionDelegate(this.conversation_arena_ask_price_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("arena_master_selected_skill_invalid", "arena_master_pay_for_reset", "arena_master_reset_attribute", "{=!}{NOT_ENOUGH_SKILL_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_arena_skill_not_developed_enough_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_skill_not_developed_enough_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_accept_perk_reset1", "arena_master_accept_perk_reset", "arena_master_perk_reset_closure", "{=Q0UjYw7V}Yes I am sure.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_accept_perk_reset_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.conversation_arena_player_accept_price), null);
			campaignGameStarter.AddPlayerLine("arena_master_reject_perk_reset2", "arena_master_accept_perk_reset", "arena_master_pre_talk", "{=UEbesbKZ}Actually, I have changed my mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_perk_reset_closure", "arena_master_perk_reset_closure", "arena_master_perk_reset_final", "{=IsBVxopm}Excellent! Is there anything else I can help you with?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_perk_reset_final1", "arena_master_perk_reset_final", "arena_master_reset_attribute", "{=aCGgBilx}I would like help fine-tuning another skill.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_train_another_skill_on_condition), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_perk_reset_final2", "arena_master_perk_reset_final", "arena_master_retrain_ask_clan_members", "{=c4tfVgqb}I would like you to help another member of my clan hone their skills.", new ConversationSentence.OnConditionDelegate(this.conversation_player_has_multiple_clan_members_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_train_another_clan_member_on_condition), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_perk_reset_final3", "arena_master_perk_reset_final", "arena_master_pre_talk", "{=Dz7E79QP}You have already helped enough. Thank you.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_finish_perk_reset_dialogs_on_consequence), 100, null, null);
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x00159F1D File Offset: 0x0015811D
		private void OnPerkReset(Hero hero, PerkObject perk)
		{
			if (perk.PrimaryRole == PartyRole.Captain)
			{
				hero.UpdatePowerModifier();
			}
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x00159F2F File Offset: 0x0015812F
		private void conversation_player_has_single_clan_member_on_consequence()
		{
			this._heroForPerkReset = this.GetClanMembersInParty()[0];
			this.SetAttributesForDialog();
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x00159F49 File Offset: 0x00158149
		private void conversation_arena_skill_not_developed_enough_on_consequence()
		{
			this.SetAttributesForDialog();
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x00159F54 File Offset: 0x00158154
		private bool conversation_arena_skill_not_developed_enough_on_condition()
		{
			TextObject textObject;
			if (this._heroForPerkReset == Hero.MainHero)
			{
				textObject = new TextObject("{=FN3xNnd1}You really don't have much experience in this skill, I can't help you much. Maybe we can work on something else?", null);
			}
			else
			{
				textObject = new TextObject("{=wGAmNQGE}{CHARACTER.NAME} does not have much experience in this skill, I can't help {?CHARACTER.GENDER}her{?}him{\\?} much. Maybe we can work on something else?", null);
				textObject.SetCharacterProperties("CHARACTER", this._heroForPerkReset.CharacterObject, false);
			}
			MBTextManager.SetTextVariable("NOT_ENOUGH_SKILL_TEXT", textObject, false);
			return !this.HasEnoughSkillValueForReset;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x00159FB4 File Offset: 0x001581B4
		private void conversation_arena_finish_perk_reset_dialogs_on_consequence()
		{
			this._heroForPerkReset = null;
			this._attributeForPerkReset = null;
			this._selectedSkillForReset = null;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00159FCB File Offset: 0x001581CB
		private void conversation_arena_train_another_skill_on_condition()
		{
			this.SetAttributesForDialog();
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00159FD3 File Offset: 0x001581D3
		private void conversation_arena_train_another_clan_member_on_condition()
		{
			this.SetHeroesForDialog();
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x00159FDB File Offset: 0x001581DB
		private void conversation_arena_player_accept_perk_reset_on_consequence()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, this.PerkResetCost, false);
			this.ResetPerkTreeForHero(this._heroForPerkReset, this._selectedSkillForReset);
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x0015A004 File Offset: 0x00158204
		private bool conversation_arena_player_accept_price(out TextObject explanation)
		{
			if (Hero.MainHero.Gold < this.PerkResetCost)
			{
				explanation = new TextObject("{=QOWyEJrm}You don't have enough denars.", null);
				return false;
			}
			explanation = new TextObject("{=ePmSvu1s}{AMOUNT}{GOLD_ICON}", null);
			explanation.SetTextVariable("AMOUNT", this.PerkResetCost);
			return true;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x0015A053 File Offset: 0x00158253
		private void conversation_arena_player_select_skill_on_consequence()
		{
			this._selectedSkillForReset = ConversationSentence.SelectedRepeatObject as SkillObject;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x0015A065 File Offset: 0x00158265
		private bool conversation_arena_ask_price_on_condition()
		{
			if (this.HasEnoughSkillValueForReset)
			{
				MBTextManager.SetTextVariable("GOLD_AMOUNT", this.PerkResetCost);
				MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
				return true;
			}
			return false;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x0015A094 File Offset: 0x00158294
		private bool conversation_arena_player_select_skill_on_condition()
		{
			SkillObject skillObject = ConversationSentence.CurrentProcessedRepeatObject as SkillObject;
			if (skillObject != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("SKILL_NAME", skillObject.Name);
				return true;
			}
			return false;
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x0015A0C8 File Offset: 0x001582C8
		private void conversation_arena_set_skills_for_reset_on_consequence()
		{
			this.SetSkillsForDialog();
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x0015A0D0 File Offset: 0x001582D0
		private void conversation_arena_list_perks_on_condition()
		{
			this.SetAttributesForDialog();
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x0015A0D8 File Offset: 0x001582D8
		private void conversation_arena_player_select_attribute_on_consequence()
		{
			this._attributeForPerkReset = ConversationSentence.SelectedRepeatObject as CharacterAttribute;
			this.SetSkillsForDialog();
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x0015A0F0 File Offset: 0x001582F0
		private bool conversation_arena_player_select_attribute_on_condition()
		{
			CharacterAttribute characterAttribute = ConversationSentence.CurrentProcessedRepeatObject as CharacterAttribute;
			if (characterAttribute != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("ATTRIBUTE_NAME", characterAttribute.Name);
				return true;
			}
			return false;
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x0015A124 File Offset: 0x00158324
		private void conversation_arena_player_select_clan_member_for_perk_reset_on_consequence()
		{
			this._heroForPerkReset = ConversationSentence.SelectedRepeatObject as Hero;
			this.SetAttributesForDialog();
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x0015A13C File Offset: 0x0015833C
		private void conversation_arena_player_select_player_for_perk_reset_on_consequence()
		{
			this._heroForPerkReset = Hero.MainHero;
			this.SetAttributesForDialog();
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x0015A14F File Offset: 0x0015834F
		private void conversation_arena_list_clan_members_on_condition()
		{
			this.SetHeroesForDialog();
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x0015A158 File Offset: 0x00158358
		private bool conversation_arena_player_select_clan_member_multiple_on_condition()
		{
			Hero hero = ConversationSentence.CurrentProcessedRepeatObject as Hero;
			if (hero != null)
			{
				ConversationSentence.SelectedRepeatLine.SetCharacterProperties("CLAN_MEMBER", hero.CharacterObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x0015A18C File Offset: 0x0015838C
		private bool conversation_player_has_multiple_clan_members_on_condition()
		{
			return this.GetClanMembersInParty().Count > 1;
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x0015A19C File Offset: 0x0015839C
		private bool conversation_player_has_single_clan_member_on_condition()
		{
			List<Hero> clanMembersInParty = this.GetClanMembersInParty();
			if (clanMembersInParty.Count == 1)
			{
				StringHelpers.SetCharacterProperties("COMPANION", clanMembersInParty[0].CharacterObject, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x0015A1D8 File Offset: 0x001583D8
		private void DailyTick()
		{
			if (Clan.PlayerClan.Companions.Count > Clan.PlayerClan.CompanionLimit)
			{
				if (!(this._warningTime != CampaignTime.Zero))
				{
					this.WarnPlayerAboutCompanionLimit();
					return;
				}
				if (this._warningTime.ElapsedDaysUntilNow > 6f)
				{
					this.RemoveACompanionFromPlayerParty();
					return;
				}
			}
			else
			{
				this._warningTime = CampaignTime.Zero;
			}
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x0015A23D File Offset: 0x0015843D
		private void SetHeroesForDialog()
		{
			ConversationSentence.SetObjectsToRepeatOver(this.GetClanMembersInParty(), 5);
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x0015A24B File Offset: 0x0015844B
		private void SetAttributesForDialog()
		{
			ConversationSentence.SetObjectsToRepeatOver(Attributes.All.ToList<CharacterAttribute>(), 5);
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x0015A25D File Offset: 0x0015845D
		private void SetSkillsForDialog()
		{
			ConversationSentence.SetObjectsToRepeatOver(Skills.All.Where<SkillObject>((SkillObject s) => s.Attributes.Contains(this._attributeForPerkReset)).ToList<SkillObject>(), 5);
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x0015A280 File Offset: 0x00158480
		private void ResetPerkTreeForHero(Hero hero, SkillObject skill)
		{
			PerkHelper.ClearPerksForSkill(hero, skill);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x0015A28C File Offset: 0x0015848C
		private void ClearPermanentBonusesIfExists(Hero hero, PerkObject perk)
		{
			if (!hero.GetPerkValue(perk))
			{
				return;
			}
			if (perk == DefaultPerks.Crafting.VigorousSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.StrongSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.EnduringSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.WeaponMasterSmith)
			{
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.OneHanded, 1);
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.TwoHanded, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Durable)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Steady)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Strong)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
			}
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x0015A36C File Offset: 0x0015856C
		private void ClearPerksForSkill(Hero hero, SkillObject skill)
		{
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if (perkObject.Skill == skill)
				{
					this.ClearPermanentBonusesIfExists(hero, perkObject);
					hero.SetPerkValueInternal(perkObject, false);
				}
			}
			PartyBase.MainParty.MemberRoster.UpdateVersion();
			hero.HitPoints = MathF.Min(hero.HitPoints, hero.MaxHitPoints);
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x0015A3F8 File Offset: 0x001585F8
		private void RemoveACompanionFromPlayerParty()
		{
			int count = Clan.PlayerClan.Companions.Count;
			int num = MBRandom.RandomInt(count);
			int i = 0;
			while (i < count)
			{
				int num2 = (i + num) % count;
				Hero hero = Clan.PlayerClan.Companions[num2];
				bool flag = true;
				CampaignEventDispatcher.Instance.CanHeroBeReleased(hero, ref flag);
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) != null)
				{
					goto IL_009A;
				}
				Settlement currentSettlement = hero.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Party.MapEvent : null) != null || Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
				{
					goto IL_009A;
				}
				bool flag2 = hero.PartyBelongedToAsPrisoner == null;
				IL_009B:
				if (flag2 && flag)
				{
					KillCharacterAction.ApplyByRemove(hero, true, true);
					return;
				}
				i++;
				continue;
				IL_009A:
				flag2 = false;
				goto IL_009B;
			}
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x0015A4BA File Offset: 0x001586BA
		private void WarnPlayerAboutCompanionLimit()
		{
			MBInformationManager.AddQuickInformation(new TextObject("{=xDikJxbO}Your party is above your companion limits. Due to that some of the companions might leave soon.", null), 0, null, null, "event:/ui/notification/relation");
			this._warningTime = CampaignTime.Now;
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x0015A4E0 File Offset: 0x001586E0
		private List<Hero> GetClanMembersInParty()
		{
			return (from m in PartyBase.MainParty.MemberRoster.GetTroopRoster()
				where m.Character.IsHero && m.Character.HeroObject.Clan == Clan.PlayerClan && !m.Character.HeroObject.IsHumanPlayerCharacter
				select m into t
				select t.Character.HeroObject).ToList<Hero>();
		}

		// Token: 0x04001447 RID: 5191
		private Hero _heroForPerkReset;

		// Token: 0x04001448 RID: 5192
		private CharacterAttribute _attributeForPerkReset;

		// Token: 0x04001449 RID: 5193
		private SkillObject _selectedSkillForReset;

		// Token: 0x0400144A RID: 5194
		private CampaignTime _warningTime;
	}
}
