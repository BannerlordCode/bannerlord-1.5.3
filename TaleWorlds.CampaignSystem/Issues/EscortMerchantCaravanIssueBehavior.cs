using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000378 RID: 888
	public class EscortMerchantCaravanIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x000D9650 File Offset: 0x000D7850
		private static EscortMerchantCaravanIssueBehavior Instance
		{
			get
			{
				return Campaign.Current.GetCampaignBehavior<EscortMerchantCaravanIssueBehavior>();
			}
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000D965C File Offset: 0x000D785C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000D96B0 File Offset: 0x000D78B0
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeOnStart();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("e1.9.1", 0))
			{
				for (int i = MobileParty.All.Count - 1; i >= 0; i--)
				{
					MobileParty mobileParty = MobileParty.All[i];
					if (mobileParty.StringId.Contains("defend_caravan_quest"))
					{
						if (mobileParty.MapEvent != null)
						{
							mobileParty.MapEvent.FinalizeEvent();
						}
						DestroyPartyAction.Apply(null, MobileParty.All[i]);
					}
				}
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000D973C File Offset: 0x000D793C
		private void InitializeOnStart()
		{
			if (MBObjectManager.Instance.GetObject<ItemObject>("hardwood") == null || MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse") == null)
			{
				CampaignEventDispatcher.Instance.RemoveListeners(this);
				using (List<KeyValuePair<Hero, IssueBase>>.Enumerator enumerator = Campaign.Current.IssueManager.Issues.Where<KeyValuePair<Hero, IssueBase>>((KeyValuePair<Hero, IssueBase> x) => x.Value.GetType() == typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue)).ToList<KeyValuePair<Hero, IssueBase>>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Hero, IssueBase> keyValuePair = enumerator.Current;
						keyValuePair.Value.CompleteIssueWithStayAliveConditionsFailed();
					}
					return;
				}
			}
			this.DefaultCaravanItems.Add(DefaultItems.Grain);
			foreach (string text in new string[] { "cotton", "velvet", "oil", "linen", "date_fruit" })
			{
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(text);
				if (@object != null)
				{
					this.DefaultCaravanItems.Add(@object);
				}
			}
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000D986C File Offset: 0x000D7A6C
		private void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeOnStart();
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000D9874 File Offset: 0x000D7A74
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000D9876 File Offset: 0x000D7A76
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsMerchant && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.IsTown && !issueGiver.CurrentSettlement.HasPort && issueGiver.OwnedCaravans.Count <= 2;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x000D98B8 File Offset: 0x000D7AB8
		public void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue), IssueBase.IssueFrequency.VeryCommon, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x000D991C File Offset: 0x000D7B1C
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			return new EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue(issueOwner);
		}

		// Token: 0x04000EF8 RID: 3832
		private const IssueBase.IssueFrequency EscortMerchantCaravanIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000EF9 RID: 3833
		internal readonly List<ItemObject> DefaultCaravanItems = new List<ItemObject>();

		// Token: 0x02000730 RID: 1840
		public class EscortMerchantCaravanIssue : IssueBase
		{
			// Token: 0x06005963 RID: 22883 RVA: 0x001A53E5 File Offset: 0x001A35E5
			internal static void AutoGeneratedStaticCollectObjectsEscortMerchantCaravanIssue(object o, List<object> collectedObjects)
			{
				((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005964 RID: 22884 RVA: 0x001A53F3 File Offset: 0x001A35F3
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005965 RID: 22885 RVA: 0x001A53FC File Offset: 0x001A35FC
			internal static object AutoGeneratedGetMemberValue_companionRewardRandom(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue)o)._companionRewardRandom;
			}

			// Token: 0x170010A9 RID: 4265
			// (get) Token: 0x06005966 RID: 22886 RVA: 0x001A540E File Offset: 0x001A360E
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x170010AA RID: 4266
			// (get) Token: 0x06005967 RID: 22887 RVA: 0x001A5412 File Offset: 0x001A3612
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 10 + MathF.Ceiling(16f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170010AB RID: 4267
			// (get) Token: 0x06005968 RID: 22888 RVA: 0x001A5428 File Offset: 0x001A3628
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 6 + MathF.Ceiling(10f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170010AC RID: 4268
			// (get) Token: 0x06005969 RID: 22889 RVA: 0x001A543D File Offset: 0x001A363D
			protected int DailyQuestRewardGold
			{
				get
				{
					return 250 + MathF.Ceiling(1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170010AD RID: 4269
			// (get) Token: 0x0600596A RID: 22890 RVA: 0x001A5456 File Offset: 0x001A3656
			protected override int RewardGold
			{
				get
				{
					return Math.Min(this.DailyQuestRewardGold * this._companionRewardRandom, 8000);
				}
			}

			// Token: 0x170010AE RID: 4270
			// (get) Token: 0x0600596B RID: 22891 RVA: 0x001A5470 File Offset: 0x001A3670
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=CSqaF7tz}There's been a real surge of banditry around here recently. I don't know if it's because the lords are away fighting or something else, but it's a miracle if a traveler can make three leagues beyond the gates without being set upon by highwaymen.[if:convo_annoyed][ib:hip]", null);
					if (base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaCurt || base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaSoftspoken)
					{
						textObject = new TextObject("{=xwc9mJdC}Things have gotten a lot worse recently with the brigands on the roads around town. My caravans get looted as soon as they're out of sight of the gates.[if:convo_stern][ib:hip]", null);
					}
					return textObject;
				}
			}

			// Token: 0x170010AF RID: 4271
			// (get) Token: 0x0600596C RID: 22892 RVA: 0x001A54C4 File Offset: 0x001A36C4
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=TGYJUUn0}Go on.", null);
				}
			}

			// Token: 0x170010B0 RID: 4272
			// (get) Token: 0x0600596D RID: 22893 RVA: 0x001A54D1 File Offset: 0x001A36D1
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=8ym6UvxE}I'm of a mind to send out a new caravan but I fear it will be plundered before it can turn a profit. So I am looking for some good fighters who can escort it until it finds its footing and visits a couple of settlements.", null);
				}
			}

			// Token: 0x170010B1 RID: 4273
			// (get) Token: 0x0600596E RID: 22894 RVA: 0x001A54E0 File Offset: 0x001A36E0
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=ytdZutjw}I will be willing to pay generously {BASE_REWARD}{GOLD_ICON} for each day the caravan is on the road. It will be more than I usually pay for caravan guards, but you look like the type who send a message to these brigands, that my caravans aren't to be messed with.[if:convo_undecided_closed]", null);
					if (base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaCurt || base.IssueOwner.CharacterObject.GetPersona() == DefaultTraits.PersonaSoftspoken)
					{
						textObject = new TextObject("{=YbbfaHqd}I will be willing to pay generously {BASE_REWARD}{GOLD_ICON} for each day the caravan is on the road. It will be more than I usually pay for guards, but figure maybe you can scare these bandits off. I'm sick of choosing between sending my men to the their deaths or letting them go because I've lost my goods and can't pay their wages.[if:convo_undecided_closed]", null);
					}
					textObject.SetTextVariable("BASE_REWARD", this.DailyQuestRewardGold);
					return textObject;
				}
			}

			// Token: 0x170010B2 RID: 4274
			// (get) Token: 0x0600596F RID: 22895 RVA: 0x001A5546 File Offset: 0x001A3746
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=a7fEPW5Y}Don't worry, I'll escort the caravan myself.", null);
				}
			}

			// Token: 0x170010B3 RID: 4275
			// (get) Token: 0x06005970 RID: 22896 RVA: 0x001A5553 File Offset: 0x001A3753
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=N4p2GCsG}I'll assign one of my companions and {NEEDED_MEN_COUNT} of my men to protect your caravan for {RETURN_DAYS} days.", null);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170010B4 RID: 4276
			// (get) Token: 0x06005971 RID: 22897 RVA: 0x001A5584 File Offset: 0x001A3784
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=hU5j7b3e}I am sure your men are as capable as you are and will look after my caravan. Thanks again for your help, my friend.[if:convo_focused_happy]", null);
				}
			}

			// Token: 0x170010B5 RID: 4277
			// (get) Token: 0x06005972 RID: 22898 RVA: 0x001A5591 File Offset: 0x001A3791
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=iny76Ifh}Thank you, {?PLAYER.GENDER}madam{?}sir{\\?}, I think they will be enough.", null);
				}
			}

			// Token: 0x170010B6 RID: 4278
			// (get) Token: 0x06005973 RID: 22899 RVA: 0x001A559E File Offset: 0x001A379E
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170010B7 RID: 4279
			// (get) Token: 0x06005974 RID: 22900 RVA: 0x001A55A1 File Offset: 0x001A37A1
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170010B8 RID: 4280
			// (get) Token: 0x06005975 RID: 22901 RVA: 0x001A55A4 File Offset: 0x001A37A4
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=6y59FBgL}{ISSUEGIVER.LINK}, a merchant from {SETTLEMENT}, has told you about {?ISSUEGIVER.GENDER}her{?}his{\\?} recent problems with bandits. {?ISSUEGIVER.GENDER}She{?}he{\\?} asked you to guard {?ISSUEGIVER.GENDER}her{?}his{\\?} caravan for a while and deal with any attackers. In return {?ISSUEGIVER.GENDER}she{?}he{\\?} offered you {GOLD}{GOLD_ICON} for each day your troops spend on escort duty.{newline}You agreed to lend {?ISSUEGIVER.GENDER}her{?}him{\\?} {NEEDED_MEN_COUNT} men. They should be enough to turn away most of the bandits. Your troops should return after {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUEGIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.Name);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", this.AlternativeSolutionSentTroops.TotalManCount);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					textObject.SetTextVariable("GOLD", this.DailyQuestRewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x170010B9 RID: 4281
			// (get) Token: 0x06005976 RID: 22902 RVA: 0x001A563E File Offset: 0x001A383E
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=VpLzd69e}Escort Merchant Caravan", null);
				}
			}

			// Token: 0x170010BA RID: 4282
			// (get) Token: 0x06005977 RID: 22903 RVA: 0x001A564B File Offset: 0x001A384B
			public override TextObject Description
			{
				get
				{
					return new TextObject("{=8RNueEmy}A merchant caravan needs an escort for protection against bandits and brigands.", null);
				}
			}

			// Token: 0x170010BB RID: 4283
			// (get) Token: 0x06005978 RID: 22904 RVA: 0x001A5658 File Offset: 0x001A3858
			public override TextObject IssueAlternativeSolutionFailLog
			{
				get
				{
					return new TextObject("{=KLauwaRJ}The caravan was destroyed despite your companion's efforts. Quest failed.", null);
				}
			}

			// Token: 0x170010BC RID: 4284
			// (get) Token: 0x06005979 RID: 22905 RVA: 0x001A5668 File Offset: 0x001A3868
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=3NX8H4TJ}Your companion has protected the caravan that belongs to {ISSUE_GIVER.LINK} from {SETTLEMENT} as promised. {?ISSUE_GIVER.GENDER}She{?}He{\\?} was happy with your work.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x0600597A RID: 22906 RVA: 0x001A56B1 File Offset: 0x001A38B1
			public EscortMerchantCaravanIssue(Hero issueOwner)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._companionRewardRandom = MBRandom.RandomInt(3, 10);
			}

			// Token: 0x0600597B RID: 22907 RVA: 0x001A56D2 File Offset: 0x001A38D2
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return -0.4f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x0600597C RID: 22908 RVA: 0x001A56F5 File Offset: 0x001A38F5
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Scouting) >= hero.GetSkillValue(DefaultSkills.Riding)) ? DefaultSkills.Scouting : DefaultSkills.Riding, 120);
			}

			// Token: 0x0600597D RID: 22909 RVA: 0x001A5722 File Offset: 0x001A3922
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x0600597E RID: 22910 RVA: 0x001A5733 File Offset: 0x001A3933
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x170010BD RID: 4285
			// (get) Token: 0x0600597F RID: 22911 RVA: 0x001A574D File Offset: 0x001A394D
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06005980 RID: 22912 RVA: 0x001A5762 File Offset: 0x001A3962
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06005981 RID: 22913 RVA: 0x001A5770 File Offset: 0x001A3970
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x06005982 RID: 22914 RVA: 0x001A5774 File Offset: 0x001A3974
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				skill = null;
				relationHero = null;
				requiredGold = 0;
				flags = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 20)
				{
					flags |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005983 RID: 22915 RVA: 0x001A57E5 File Offset: 0x001A39E5
			public override bool IssueStayAliveConditions()
			{
				return base.IssueOwner.OwnedCaravans.Count <= 2;
			}

			// Token: 0x06005984 RID: 22916 RVA: 0x001A57FD File Offset: 0x001A39FD
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005985 RID: 22917 RVA: 0x001A57FF File Offset: 0x001A39FF
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005986 RID: 22918 RVA: 0x001A5801 File Offset: 0x001A3A01
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(30f), base.IssueDifficultyMultiplier, this.DailyQuestRewardGold);
			}

			// Token: 0x06005987 RID: 22919 RVA: 0x001A5828 File Offset: 0x001A3A28
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				base.IssueOwner.AddPower(-5f);
				this.RelationshipChangeWithIssueOwner = -5;
				TraitLevelingHelper.OnIssueFailed(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				base.IssueSettlement.Town.Prosperity -= 20f;
			}

			// Token: 0x06005988 RID: 22920 RVA: 0x001A5889 File Offset: 0x001A3A89
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				base.IssueOwner.AddPower(10f);
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueSettlement.Town.Prosperity += 10f;
			}

			// Token: 0x06005989 RID: 22921 RVA: 0x001A58BE File Offset: 0x001A3ABE
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001D8B RID: 7563
			private const int MinimumRequiredMenCount = 20;

			// Token: 0x04001D8C RID: 7564
			private const int AlternativeSolutionTroopTierRequirement = 2;

			// Token: 0x04001D8D RID: 7565
			private const int NeededCompanionSkillAmount = 120;

			// Token: 0x04001D8E RID: 7566
			private const int QuestTimeLimit = 30;

			// Token: 0x04001D8F RID: 7567
			private const int IssueDuration = 30;

			// Token: 0x04001D90 RID: 7568
			[SaveableField(10)]
			private int _companionRewardRandom;
		}

		// Token: 0x02000731 RID: 1841
		public class EscortMerchantCaravanIssueQuest : QuestBase
		{
			// Token: 0x0600598A RID: 22922 RVA: 0x001A58C0 File Offset: 0x001A3AC0
			internal static void AutoGeneratedStaticCollectObjectsEscortMerchantCaravanIssueQuest(object o, List<object> collectedObjects)
			{
				((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600598B RID: 22923 RVA: 0x001A58D0 File Offset: 0x001A3AD0
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._visitedSettlements);
				collectedObjects.Add(this._questCaravanMobileParty);
				collectedObjects.Add(this._questBanditMobileParty);
				collectedObjects.Add(this._otherBanditParty);
				collectedObjects.Add(this._playerStartsQuestLog);
			}

			// Token: 0x0600598C RID: 22924 RVA: 0x001A5920 File Offset: 0x001A3B20
			internal static object AutoGeneratedGetMemberValue_requiredSettlementNumber(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._requiredSettlementNumber;
			}

			// Token: 0x0600598D RID: 22925 RVA: 0x001A5932 File Offset: 0x001A3B32
			internal static object AutoGeneratedGetMemberValue_visitedSettlements(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._visitedSettlements;
			}

			// Token: 0x0600598E RID: 22926 RVA: 0x001A593F File Offset: 0x001A3B3F
			internal static object AutoGeneratedGetMemberValue_questCaravanMobileParty(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questCaravanMobileParty;
			}

			// Token: 0x0600598F RID: 22927 RVA: 0x001A594C File Offset: 0x001A3B4C
			internal static object AutoGeneratedGetMemberValue_questBanditMobileParty(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questBanditMobileParty;
			}

			// Token: 0x06005990 RID: 22928 RVA: 0x001A5959 File Offset: 0x001A3B59
			internal static object AutoGeneratedGetMemberValue_difficultyMultiplier(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._difficultyMultiplier;
			}

			// Token: 0x06005991 RID: 22929 RVA: 0x001A596B File Offset: 0x001A3B6B
			internal static object AutoGeneratedGetMemberValue_isPlayerNotifiedForDanger(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._isPlayerNotifiedForDanger;
			}

			// Token: 0x06005992 RID: 22930 RVA: 0x001A597D File Offset: 0x001A3B7D
			internal static object AutoGeneratedGetMemberValue_otherBanditParty(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._otherBanditParty;
			}

			// Token: 0x06005993 RID: 22931 RVA: 0x001A598A File Offset: 0x001A3B8A
			internal static object AutoGeneratedGetMemberValue_questBanditPartyFollowDuration(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questBanditPartyFollowDuration;
			}

			// Token: 0x06005994 RID: 22932 RVA: 0x001A599C File Offset: 0x001A3B9C
			internal static object AutoGeneratedGetMemberValue_otherBanditPartyFollowDuration(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._otherBanditPartyFollowDuration;
			}

			// Token: 0x06005995 RID: 22933 RVA: 0x001A59AE File Offset: 0x001A3BAE
			internal static object AutoGeneratedGetMemberValue_daysSpentForEscorting(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._daysSpentForEscorting;
			}

			// Token: 0x06005996 RID: 22934 RVA: 0x001A59C0 File Offset: 0x001A3BC0
			internal static object AutoGeneratedGetMemberValue_questBanditPartyAlreadyAttacked(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._questBanditPartyAlreadyAttacked;
			}

			// Token: 0x06005997 RID: 22935 RVA: 0x001A59D2 File Offset: 0x001A3BD2
			internal static object AutoGeneratedGetMemberValue_playerStartsQuestLog(object o)
			{
				return ((EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest)o)._playerStartsQuestLog;
			}

			// Token: 0x170010BE RID: 4286
			// (get) Token: 0x06005998 RID: 22936 RVA: 0x001A59DF File Offset: 0x001A3BDF
			private float BanditPartyAttackRadiusMin
			{
				get
				{
					return Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius * 2.5f;
				}
			}

			// Token: 0x170010BF RID: 4287
			// (get) Token: 0x06005999 RID: 22937 RVA: 0x001A59FB File Offset: 0x001A3BFB
			private float QuestBanditPartySpawnDistance
			{
				get
				{
					return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) * 1.25f;
				}
			}

			// Token: 0x170010C0 RID: 4288
			// (get) Token: 0x0600599A RID: 22938 RVA: 0x001A5A0E File Offset: 0x001A3C0E
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=VpLzd69e}Escort Merchant Caravan", null);
				}
			}

			// Token: 0x170010C1 RID: 4289
			// (get) Token: 0x0600599B RID: 22939 RVA: 0x001A5A1B File Offset: 0x001A3C1B
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170010C2 RID: 4290
			// (get) Token: 0x0600599C RID: 22940 RVA: 0x001A5A1E File Offset: 0x001A3C1E
			private int BanditPartyTroopCount
			{
				get
				{
					return (int)MathF.Min(40f, (float)(MobileParty.MainParty.MemberRoster.TotalHealthyCount + this._questCaravanMobileParty.MemberRoster.TotalHealthyCount) * 0.7f);
				}
			}

			// Token: 0x170010C3 RID: 4291
			// (get) Token: 0x0600599D RID: 22941 RVA: 0x001A5A52 File Offset: 0x001A3C52
			private int CaravanPartyTroopCount
			{
				get
				{
					return (int)(5f * this._difficultyMultiplier) + 10;
				}
			}

			// Token: 0x170010C4 RID: 4292
			// (get) Token: 0x0600599E RID: 22942 RVA: 0x001A5A64 File Offset: 0x001A3C64
			private bool CaravanIsInsideSettlement
			{
				get
				{
					return this._questCaravanMobileParty.CurrentSettlement != null;
				}
			}

			// Token: 0x170010C5 RID: 4293
			// (get) Token: 0x0600599F RID: 22943 RVA: 0x001A5A74 File Offset: 0x001A3C74
			private int TotalRewardGold
			{
				get
				{
					return MathF.Min(8000, this.RewardGold * this._daysSpentForEscorting);
				}
			}

			// Token: 0x170010C6 RID: 4294
			// (get) Token: 0x060059A0 RID: 22944 RVA: 0x001A5A8D File Offset: 0x001A3C8D
			private CustomPartyComponent CaravanCustomPartyComponent
			{
				get
				{
					if (this._customPartyComponent == null)
					{
						this._customPartyComponent = this._questCaravanMobileParty.PartyComponent as CustomPartyComponent;
					}
					return this._customPartyComponent;
				}
			}

			// Token: 0x170010C7 RID: 4295
			// (get) Token: 0x060059A1 RID: 22945 RVA: 0x001A5AB4 File Offset: 0x001A3CB4
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=YXbKXUDu}{ISSUE_GIVER.LINK}, a merchant from {SETTLEMENT}, has told you about {?ISSUE_GIVER.GENDER}her{?}his{\\?} recent problems with bandits. {?ISSUE_GIVER.GENDER}She{?}He{\\?} asked you to guard {?ISSUE_GIVER.GENDER}her{?}his{\\?} caravan for a while and deal with any attackers. In return {?ISSUE_GIVER.GENDER}she{?}he{\\?} offered you {GOLD}{GOLD_ICON} denars for each day you spend on escort duty.{newline}You have agreed to guard it yourself until it visits {NUMBER_OF_SETTLEMENTS} settlements.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
					textObject.SetTextVariable("NUMBER_OF_SETTLEMENTS", this._requiredSettlementNumber);
					textObject.SetTextVariable("GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x170010C8 RID: 4296
			// (get) Token: 0x060059A2 RID: 22946 RVA: 0x001A5B31 File Offset: 0x001A3D31
			private TextObject CaravanDestroyedQuestLogText
			{
				get
				{
					return new TextObject("{=zk9QyKIz}The caravan was destroyed. Quest failed.", null);
				}
			}

			// Token: 0x170010C9 RID: 4297
			// (get) Token: 0x060059A3 RID: 22947 RVA: 0x001A5B40 File Offset: 0x001A3D40
			private TextObject CaravanLostTheTrackLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=y62dyzH6}You have lost the track of caravan. Your agreement with {ISSUE_GIVER.LINK} is failed.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170010CA RID: 4298
			// (get) Token: 0x060059A4 RID: 22948 RVA: 0x001A5B74 File Offset: 0x001A3D74
			private TextObject CaravanDestroyedByBanditsLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=MhvyTcrH}The caravan is destroyed by some bandits. Your agreement with {ISSUE_GIVER.LINK} is failed.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170010CB RID: 4299
			// (get) Token: 0x060059A5 RID: 22949 RVA: 0x001A5BA6 File Offset: 0x001A3DA6
			private TextObject CaravanDestroyedByPlayerQuestLogText
			{
				get
				{
					return new TextObject("{=Rd3m5kyk}You have attacked the caravan.", null);
				}
			}

			// Token: 0x170010CC RID: 4300
			// (get) Token: 0x060059A6 RID: 22950 RVA: 0x001A5BB4 File Offset: 0x001A3DB4
			private TextObject SuccessQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=dKEADOhG}You have protected the caravan belonging to {QUEST_GIVER.LINK} from {SETTLEMENT} as promised. {?QUEST_GIVER.GENDER}She{?}He{\\?} was happy with your work.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170010CD RID: 4301
			// (get) Token: 0x060059A7 RID: 22951 RVA: 0x001A5C04 File Offset: 0x001A3E04
			private TextObject CancelByWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=KhNkBd9O}Your clan is now at war with the {QUEST_GIVER.LINK}’s lord. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x060059A8 RID: 22952 RVA: 0x001A5C38 File Offset: 0x001A3E38
			public EscortMerchantCaravanIssueQuest(string questId, Hero giverHero, CampaignTime duration, float difficultyMultiplier, int rewardGold)
				: base(questId, giverHero, duration, rewardGold)
			{
				this._difficultyMultiplier = difficultyMultiplier;
				this._requiredSettlementNumber = MathF.Round(2f + 4f * this._difficultyMultiplier);
				this._visitedSettlements = new List<Settlement>();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x060059A9 RID: 22953 RVA: 0x001A5C94 File Offset: 0x001A3E94
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=TdwKwExD}Thank you. You can find the caravan just outside the settlement.[if:convo_grateful]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=vtZYmAaR}I feel good knowing that you're looking after my caravan. Safe journeys, my friend![if:convo_grateful]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanPartyDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanGreetingDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanTradeDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanLootDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCaravanFarewellDialogFlow(), this);
			}

			// Token: 0x170010CE RID: 4302
			// (get) Token: 0x060059AA RID: 22954 RVA: 0x001A5D98 File Offset: 0x001A3F98
			private TextObject CaravanNoTargetLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=1FOmvEdf}All profitable trade routes of the caravan are blocked by recent wars. {QUEST_GIVER.LINK} decided to recall the caravan until the situation gets better. {?QUEST_GIVER.GENDER}She{?}He{\\?} was happy with your service and sent you {REWARD}{GOLD_ICON} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.TotalRewardGold);
					return textObject;
				}
			}

			// Token: 0x060059AB RID: 22955 RVA: 0x001A5DDC File Offset: 0x001A3FDC
			private DialogFlow GetCaravanPartyDialogFlow()
			{
				TextObject textObject = new TextObject("{=ZAqEJI9T}About the task {QUEST_GIVER.LINK} gave me.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions(null, false).PlayerOption(textObject, null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.caravan_talk_on_condition))
					.NpcLine("{=heWYa9Oq}I feel safe knowing that you're looking after us. Please continue to follow us my friend!", null, null, null, null)
					.Consequence(delegate
					{
						PlayerEncounter.LeaveEncounter = true;
					})
					.CloseDialog()
					.EndPlayerOptions();
			}

			// Token: 0x060059AC RID: 22956 RVA: 0x001A5E78 File Offset: 0x001A4078
			private bool caravan_talk_on_condition()
			{
				bool flag = this._questCaravanMobileParty.MemberRoster.Contains(CharacterObject.OneToOneConversationCharacter) && this._questCaravanMobileParty == MobileParty.ConversationParty && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCustomParty && !CharacterObject.OneToOneConversationCharacter.IsHero && MobileParty.ConversationParty.Party.Owner != Hero.MainHero;
				if (flag)
				{
					MBTextManager.SetTextVariable("HOMETOWN", MobileParty.ConversationParty.HomeSettlement.EncyclopediaLinkWithName, false);
					StringHelpers.SetCharacterProperties("MERCHANT", MobileParty.ConversationParty.Party.Owner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("PROTECTOR", MobileParty.ConversationParty.HomeSettlement.OwnerClan.Leader.CharacterObject, null, false);
				}
				return flag;
			}

			// Token: 0x060059AD RID: 22957 RVA: 0x001A5F48 File Offset: 0x001A4148
			private DialogFlow GetCaravanFarewellDialogFlow()
			{
				TextObject textObject = new TextObject("{=1IJouNaM}Carry on, then. Farewell.", null);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions(null, false).PlayerOption(textObject, null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.caravan_talk_on_condition))
					.NpcLine("{=heWYa9Oq}I feel safe knowing that you're looking after us. Please continue to follow us my friend!", null, null, null, null)
					.Consequence(delegate
					{
						PlayerEncounter.LeaveEncounter = true;
					})
					.CloseDialog()
					.EndPlayerOptions();
			}

			// Token: 0x060059AE RID: 22958 RVA: 0x001A5FCC File Offset: 0x001A41CC
			private DialogFlow GetCaravanLootDialogFlow()
			{
				TextObject textObject = new TextObject("{=WOBy5UfY}Hand over your goods, or die!", null);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions(null, false).PlayerOption(textObject, null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.caravan_loot_on_condition))
					.NpcLine("{=QNaKmkt9}We're paid to guard this caravan. If you want to rob it, it's going to be over our dead bodies![if:convo_angry][ib:aggressive]", null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=EhxS7NQ4}So be it. Attack!", null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_on_consequence))
					.CloseDialog()
					.PlayerOption("{=bfPsE9M1}You must have misunderstood me. Go in peace.", null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence))
					.CloseDialog()
					.EndPlayerOptions()
					.EndPlayerOptions();
			}

			// Token: 0x060059AF RID: 22959 RVA: 0x001A6077 File Offset: 0x001A4277
			private void conversation_caravan_fight_on_consequence()
			{
				BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
			}

			// Token: 0x060059B0 RID: 22960 RVA: 0x001A608D File Offset: 0x001A428D
			private void caravan_talk_leave_on_consequence()
			{
				if (PlayerEncounter.Current != null)
				{
					PlayerEncounter.LeaveEncounter = true;
				}
			}

			// Token: 0x060059B1 RID: 22961 RVA: 0x001A609C File Offset: 0x001A429C
			private DialogFlow GetCaravanTradeDialogFlow()
			{
				TextObject textObject = new TextObject("{=t0UGXPV4}I'm interested in trading. What kind of products do you have?", null);
				return DialogFlow.CreateDialogFlow("escort_caravan_talk", 125).BeginPlayerOptions(null, false).PlayerOption(textObject, null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.caravan_buy_products_on_condition))
					.NpcLine("{=tlLDHAIu}Very well. A pleasure doing business with you.[if:convo_relaxed_happy][ib:demure]", null, null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.conversation_caravan_player_trade_end_on_condition))
					.NpcLine("{=DQBaaC0e}Is there anything else?", null, null, null, null)
					.GotoDialogState("escort_caravan_talk")
					.EndPlayerOptions();
			}

			// Token: 0x060059B2 RID: 22962 RVA: 0x001A6120 File Offset: 0x001A4320
			private bool caravan_buy_products_on_condition()
			{
				if (MobileParty.ConversationParty != null && MobileParty.ConversationParty == this._questCaravanMobileParty && !MobileParty.ConversationParty.IsCaravan)
				{
					for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
					{
						if (MobileParty.ConversationParty.ItemRoster.GetElementNumber(i) > 0)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060059B3 RID: 22963 RVA: 0x001A617D File Offset: 0x001A437D
			private bool conversation_caravan_player_trade_end_on_condition()
			{
				if (MobileParty.ConversationParty != null && MobileParty.ConversationParty == this._questCaravanMobileParty && !MobileParty.ConversationParty.IsCaravan)
				{
					InventoryScreenHelper.OpenTradeWithCaravanOrAlleyParty(MobileParty.ConversationParty, InventoryScreenHelper.InventoryCategoryType.None);
				}
				return true;
			}

			// Token: 0x060059B4 RID: 22964 RVA: 0x001A61AC File Offset: 0x001A43AC
			private DialogFlow GetCaravanGreetingDialogFlow()
			{
				TextObject textObject = new TextObject("{=FpUybbSk}Greetings. This caravan is owned by {MERCHANT.LINK}. We trade under the protection of {PROTECTOR.LINK}, master of {HOMETOWN}. How may we help you?[if:convo_normal]", null);
				if (MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCurrentlyAtSea)
				{
					textObject = new TextObject("{=yGttYe7g}Greetings. This ship is owned by {MERCHANT.LINK}. We sail under the protection of {PROTECTOR.LINK}, master of {HOMETOWN}. How may we help you?[if:convo_normal]", null);
				}
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.caravan_talk_on_condition))
					.GotoDialogState("escort_caravan_talk");
			}

			// Token: 0x060059B5 RID: 22965 RVA: 0x001A6215 File Offset: 0x001A4415
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this.SpawnCaravan();
				this._playerStartsQuestLog = base.AddDiscreteLog(this.PlayerStartsQuestLogText, new TextObject("{=r2y3n7dR}Visited Settlements", null), this._visitedSettlements.Count, this._requiredSettlementNumber, null, false);
			}

			// Token: 0x060059B6 RID: 22966 RVA: 0x001A6254 File Offset: 0x001A4454
			private bool caravan_loot_on_condition()
			{
				bool flag = MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.MapFaction != Hero.MainHero.MapFaction && !MobileParty.ConversationParty.IsCaravan && MobileParty.ConversationParty == this._questCaravanMobileParty;
				if (flag)
				{
					MBTextManager.SetTextVariable("HOMETOWN", MobileParty.ConversationParty.HomeSettlement.EncyclopediaLinkWithName, false);
					StringHelpers.SetCharacterProperties("MERCHANT", MobileParty.ConversationParty.Party.Owner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("PROTECTOR", MobileParty.ConversationParty.HomeSettlement.OwnerClan.Leader.CharacterObject, null, false);
				}
				return flag;
			}

			// Token: 0x060059B7 RID: 22967 RVA: 0x001A6304 File Offset: 0x001A4504
			private void SpawnCaravan()
			{
				ItemRoster itemRoster = new ItemRoster();
				foreach (ItemObject itemObject in EscortMerchantCaravanIssueBehavior.Instance.DefaultCaravanItems)
				{
					itemRoster.AddToCounts(itemObject, 7);
				}
				string text;
				string text2;
				this.GetAdditionalVisualsForParty(base.QuestGiver.Culture, out text, out text2);
				TextObject textObject = GameTexts.FindText("str_caravan_party_name", null);
				textObject.SetCharacterProperties("OWNER", base.QuestGiver.CharacterObject, false);
				this._questCaravanMobileParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(base.QuestGiver.CurrentSettlement.GatePosition, 0f, base.QuestGiver.CurrentSettlement, textObject, base.QuestGiver.Clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), base.QuestGiver, text, text2, 4f, false);
				this.InitializeCaravanOnCreation(this._questCaravanMobileParty, base.QuestGiver, base.QuestGiver.CurrentSettlement, itemRoster);
				base.AddTrackedObject(this._questCaravanMobileParty);
				this._questCaravanMobileParty.SetPartyUsedByQuest(true);
				this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(true);
				this._questCaravanMobileParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
				this._caravanWaitedInSettlementForHours = 4;
			}

			// Token: 0x060059B8 RID: 22968 RVA: 0x001A644C File Offset: 0x001A464C
			private bool ProperSettlementCondition(Settlement settlement)
			{
				return settlement != Settlement.CurrentSettlement && settlement.IsTown && !settlement.IsUnderSiege && !this._visitedSettlements.Contains(settlement);
			}

			// Token: 0x060059B9 RID: 22969 RVA: 0x001A6478 File Offset: 0x001A4678
			private void InitializeCaravanOnCreation(MobileParty mobileParty, Hero owner, Settlement settlement, ItemRoster caravanItems)
			{
				mobileParty.Aggressiveness = 0f;
				PartyTemplateObject randomCaravanTemplate = CaravanHelper.GetRandomCaravanTemplate(owner.Culture, false, true);
				mobileParty.InitializeMobilePartyAtPosition(TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), settlement.GatePosition, false);
				MobilePartyHelper.FillPartyManuallyAfterCreation(mobileParty, randomCaravanTemplate, this.CaravanPartyTroopCount);
				CharacterObject characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject character) => character.Occupation == Occupation.CaravanGuard && character.IsInfantry && character.Level == 26 && character.Culture == mobileParty.Party.Owner.Culture);
				mobileParty.MemberRoster.AddToCounts(characterObject, 1, true, 0, 0, true, -1);
				mobileParty.Party.SetVisualAsDirty();
				mobileParty.InitializePartyTrade(Campaign.Current.Models.CaravanModel.GetInitialTradeGold(owner, false, false));
				if (caravanItems != null)
				{
					mobileParty.ItemRoster.Add(caravanItems);
					return;
				}
				float num = 10000f;
				ItemObject itemObject = null;
				foreach (ItemObject itemObject2 in Items.All)
				{
					if (itemObject2.ItemCategory == DefaultItemCategories.PackAnimal && !itemObject2.NotMerchandise && (float)itemObject2.Value < num)
					{
						itemObject = itemObject2;
						num = (float)itemObject2.Value;
					}
				}
				if (itemObject != null)
				{
					mobileParty.ItemRoster.Add(new ItemRosterElement(itemObject, (int)((float)mobileParty.MemberRoster.TotalManCount * 0.5f), null));
				}
			}

			// Token: 0x060059BA RID: 22970 RVA: 0x001A6604 File Offset: 0x001A4804
			private void GetAdditionalVisualsForParty(CultureObject culture, out string mountStringId, out string harnessStringId)
			{
				if (culture.StringId == "aserai" || culture.StringId == "khuzait")
				{
					mountStringId = "camel";
					harnessStringId = ((MBRandom.RandomFloat > 0.5f) ? "camel_saddle_a" : "camel_saddle_b");
					return;
				}
				mountStringId = "mule";
				harnessStringId = ((MBRandom.RandomFloat > 0.5f) ? "mule_load_a" : ((MBRandom.RandomFloat > 0.5f) ? "mule_load_b" : "mule_load_c"));
			}

			// Token: 0x060059BB RID: 22971 RVA: 0x001A668C File Offset: 0x001A488C
			protected override void RegisterEvents()
			{
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyHourlyTick));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x060059BC RID: 22972 RVA: 0x001A673A File Offset: 0x001A493A
			private void OnPartyHourlyTick(MobileParty mobileParty)
			{
				this.CheckPartyAndMakeItAttackTheCaravan(mobileParty);
				this.CheckEncounterForBanditParty(this._questBanditMobileParty);
				this.CheckEncounterForBanditParty(this._otherBanditParty);
				this.CheckOtherBanditPartyDistance();
			}

			// Token: 0x060059BD RID: 22973 RVA: 0x001A6764 File Offset: 0x001A4964
			private void CheckOtherBanditPartyDistance()
			{
				if (base.IsOngoing)
				{
					if (this._otherBanditParty != null && this._otherBanditParty.IsActive && this._otherBanditParty.TargetParty == this._questCaravanMobileParty && this._otherBanditPartyFollowDuration < 0)
					{
						if (base.IsTracked(this._otherBanditParty))
						{
							base.RemoveTrackedObject(this._otherBanditParty);
						}
						this._otherBanditParty.SetMoveModeHold();
						this._otherBanditParty.Ai.SetDoNotMakeNewDecisions(false);
						this._otherBanditParty = null;
					}
					if (this._questBanditMobileParty != null && this._questBanditMobileParty.IsActive && this._questBanditMobileParty.MapEvent == null && this._questBanditMobileParty.TargetParty == this._questCaravanMobileParty && this._questBanditPartyFollowDuration < 0 && !this._questBanditMobileParty.IsVisible)
					{
						if (base.IsTracked(this._questBanditMobileParty))
						{
							base.RemoveTrackedObject(this._questBanditMobileParty);
						}
						this._questBanditMobileParty.SetMoveModeHold();
						this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					}
				}
			}

			// Token: 0x060059BE RID: 22974 RVA: 0x001A686C File Offset: 0x001A4A6C
			private void CheckEncounterForBanditParty(MobileParty mobileParty)
			{
				if (base.IsOngoing && mobileParty != null && mobileParty.IsActive && mobileParty.MapEvent == null && this._questCaravanMobileParty.IsActive && this._questCaravanMobileParty.MapEvent == null && this._questCaravanMobileParty.CurrentSettlement == null && mobileParty.Position.DistanceSquared(this._questCaravanMobileParty.Position) <= 1f)
				{
					EncounterManager.StartPartyEncounter(mobileParty.Party, this._questCaravanMobileParty.Party);
					MBInformationManager.AddQuickInformation(new TextObject("{=o8uAzFaJ}The caravan you are protecting is ambushed by raiders!", null), 0, null, null, "");
					this._questCaravanMobileParty.MapEvent.IsInvulnerable = true;
				}
			}

			// Token: 0x060059BF RID: 22975 RVA: 0x001A6928 File Offset: 0x001A4B28
			private void CheckPartyAndMakeItAttackTheCaravan(MobileParty mobileParty)
			{
				if (this._otherBanditParty == null && mobileParty != this._questBanditMobileParty && mobileParty.IsBandit && !mobileParty.IsCurrentlyUsedByAQuest && mobileParty.MapEvent == null && mobileParty.NavigationCapability == MobileParty.NavigationType.Default && mobileParty.Party.NumberOfHealthyMembers > this._questCaravanMobileParty.Party.NumberOfHealthyMembers && (mobileParty.Speed > this._questCaravanMobileParty.Speed || mobileParty.Position.DistanceSquared(this._questCaravanMobileParty.Position) < 9f))
				{
					Settlement settlement = this._visitedSettlements.LastOrDefault<Settlement>() ?? this._questCaravanMobileParty.HomeSettlement;
					Settlement targetSettlement = this._questCaravanMobileParty.TargetSettlement;
					if (targetSettlement == null)
					{
						this.TryToFindAndSetTargetToNextSettlement();
						return;
					}
					float num;
					float num2;
					if (this._questCaravanMobileParty.CurrentSettlement != null)
					{
						num = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty.CurrentSettlement, targetSettlement, false, false, MobileParty.NavigationType.Default);
						num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty.CurrentSettlement, settlement, false, false, MobileParty.NavigationType.Default);
					}
					else
					{
						float num3;
						num = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, targetSettlement, false, MobileParty.NavigationType.Default, out num3);
						num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, settlement, false, MobileParty.NavigationType.Default, out num3);
					}
					float num4 = mobileParty.Position.DistanceSquared(this._questCaravanMobileParty.Position);
					if (num > 5f && num2 > 5f && num4 < this.BanditPartyAttackRadiusMin * this.BanditPartyAttackRadiusMin)
					{
						SetPartyAiAction.GetActionForEngagingParty(mobileParty, this._questCaravanMobileParty, MobileParty.NavigationType.Default, false);
						mobileParty.Ai.SetDoNotMakeNewDecisions(true);
						if (!base.IsTracked(mobileParty))
						{
							base.AddTrackedObject(mobileParty);
						}
						float num5 = mobileParty.Speed + this._questCaravanMobileParty.Speed;
						this._otherBanditPartyFollowDuration = (int)(num4 / num5) + 5;
						this._otherBanditParty = mobileParty;
					}
				}
			}

			// Token: 0x060059C0 RID: 22976 RVA: 0x001A6B2C File Offset: 0x001A4D2C
			private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
			{
				if (party == this._questCaravanMobileParty && settlement != this._questCaravanMobileParty.HomeSettlement && settlement.Position.NearlyEquals(MobileParty.MainParty.Position.ToVec2(), MobileParty.MainParty.SeeingRange + 2f) && settlement == this._questCaravanMobileParty.TargetSettlement)
				{
					this._visitedSettlements.Add(settlement);
					base.UpdateQuestTaskStage(this._playerStartsQuestLog, this._visitedSettlements.Count);
					TextObject textObject = new TextObject("{=0wj3HIbh}Caravan entered {SETTLEMENT_LINK}.", null);
					textObject.SetTextVariable("SETTLEMENT_LINK", settlement.EncyclopediaLinkWithName);
					base.AddLog(textObject, true);
					if (this._questBanditMobileParty != null && this._questBanditMobileParty.IsActive)
					{
						if (base.IsTracked(this._questBanditMobileParty))
						{
							base.RemoveTrackedObject(this._questBanditMobileParty);
						}
						this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
						this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
						if (this._questBanditMobileParty.MapEvent == null)
						{
							SetPartyAiAction.GetActionForPatrollingAroundSettlement(this._questBanditMobileParty, settlement, MobileParty.NavigationType.Default, false, false);
						}
					}
					if (this._otherBanditParty != null)
					{
						if (base.IsTracked(this._otherBanditParty))
						{
							base.RemoveTrackedObject(this._otherBanditParty);
						}
						this._otherBanditParty.SetMoveModeHold();
						this._otherBanditParty.Ai.SetDoNotMakeNewDecisions(false);
						this._otherBanditParty = null;
					}
					if (this._visitedSettlements.Count == this._requiredSettlementNumber)
					{
						this.SuccessConsequences(false);
						return;
					}
					int num = this.CaravanPartyTroopCount - this._questCaravanMobileParty.MemberRoster.TotalManCount;
					if (num > 0)
					{
						this._questCaravanMobileParty.AddElementToMemberRoster(this._questCaravanMobileParty.TargetSettlement.Culture.CaravanGuard, MBRandom.RandomInt(Math.Min(15, num)), false);
					}
				}
			}

			// Token: 0x060059C1 RID: 22977 RVA: 0x001A6CFD File Offset: 0x001A4EFD
			protected override void DailyTick()
			{
				this._daysSpentForEscorting++;
			}

			// Token: 0x060059C2 RID: 22978 RVA: 0x001A6D0D File Offset: 0x001A4F0D
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				this.CheckWarDeclaration();
			}

			// Token: 0x060059C3 RID: 22979 RVA: 0x001A6D15 File Offset: 0x001A4F15
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				this.CheckWarDeclaration();
			}

			// Token: 0x060059C4 RID: 22980 RVA: 0x001A6D1D File Offset: 0x001A4F1D
			private void CheckWarDeclaration()
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.CancelByWarQuestLogText);
				}
			}

			// Token: 0x060059C5 RID: 22981 RVA: 0x001A6D4C File Offset: 0x001A4F4C
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				if (detail == DeclareWarAction.DeclareWarDetail.CausedByPlayerHostility && (faction1 == this._questCaravanMobileParty.MapFaction || faction2 == this._questCaravanMobileParty.MapFaction) && PlayerEncounter.Current != null && PlayerEncounter.PlayerIsAttacker)
				{
					this.FailByPlayerHostileConsequences();
				}
				else
				{
					this.CheckWarDeclaration();
				}
				if (this._questCaravanMobileParty != null && (this._questCaravanMobileParty.TargetSettlement == null || this._questCaravanMobileParty.TargetSettlement.MapFaction.IsAtWarWith(this._questCaravanMobileParty.MapFaction)) && base.IsOngoing)
				{
					this.TryToFindAndSetTargetToNextSettlement();
				}
			}

			// Token: 0x060059C6 RID: 22982 RVA: 0x001A6DDC File Offset: 0x001A4FDC
			protected override void HourlyTick()
			{
				if (base.IsOngoing && this._questCaravanMobileParty.TargetSettlement == null)
				{
					this.TryToFindAndSetTargetToNextSettlement();
				}
				if (base.IsOngoing)
				{
					if (this.CaravanIsInsideSettlement)
					{
						this.SimulateSettlementWaitForCaravan();
					}
					else if (this._questCaravanMobileParty.MapEvent == null)
					{
						this.AdjustCaravansSpeed();
					}
					this.NotifyPlayerOrCancelTheQuestIfCaravanIsFar();
					if (base.IsOngoing)
					{
						this.ThinkAboutSpawningBanditParty();
						this.CheckCaravanMapEvent();
						this._otherBanditPartyFollowDuration--;
						this._questBanditPartyFollowDuration--;
					}
				}
			}

			// Token: 0x060059C7 RID: 22983 RVA: 0x001A6E68 File Offset: 0x001A5068
			private void CheckCaravanMapEvent()
			{
				if (this._questCaravanMobileParty.MapEvent != null && this._questCaravanMobileParty.MapEvent.IsInvulnerable && this._questCaravanMobileParty.MapEvent.BattleStartTime.ElapsedHoursUntilNow > 3f)
				{
					this._questCaravanMobileParty.MapEvent.IsInvulnerable = false;
				}
			}

			// Token: 0x060059C8 RID: 22984 RVA: 0x001A6EC4 File Offset: 0x001A50C4
			private void AdjustCaravansSpeed()
			{
				if (!MobileParty.MainParty.IsActive)
				{
					return;
				}
				float num = MobileParty.MainParty.Speed;
				float num2 = this._questCaravanMobileParty.Speed;
				while (num < num2 || num - num2 > 1f)
				{
					if (num2 >= num)
					{
						this.CaravanCustomPartyComponent.SetBaseSpeed(this.CaravanCustomPartyComponent.BaseSpeed - 0.05f);
					}
					else if (num - num2 > 1f)
					{
						this.CaravanCustomPartyComponent.SetBaseSpeed(this.CaravanCustomPartyComponent.BaseSpeed + 0.05f);
					}
					num = MobileParty.MainParty.Speed;
					num2 = this._questCaravanMobileParty.Speed;
				}
			}

			// Token: 0x060059C9 RID: 22985 RVA: 0x001A6F64 File Offset: 0x001A5164
			private void ThinkAboutSpawningBanditParty()
			{
				if (!this._questBanditPartyAlreadyAttacked && this._questBanditMobileParty == null)
				{
					Settlement targetSettlement = this._questCaravanMobileParty.TargetSettlement;
					if (targetSettlement != null)
					{
						float num2;
						float num = ((this._questCaravanMobileParty.CurrentSettlement != null) ? Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty.CurrentSettlement, targetSettlement, false, false, MobileParty.NavigationType.Default) : Campaign.Current.Models.MapDistanceModel.GetDistance(this._questCaravanMobileParty, targetSettlement, false, MobileParty.NavigationType.Default, out num2));
						if (num > 10f && num < this.QuestBanditPartySpawnDistance)
						{
							this.ActivateBanditParty();
							float num3 = this._questBanditMobileParty.Speed + this._questCaravanMobileParty.Speed;
							this._questBanditPartyFollowDuration = (int)(this.QuestBanditPartySpawnDistance / num3) + 5;
							this._questBanditPartyAlreadyAttacked = true;
						}
					}
				}
			}

			// Token: 0x060059CA RID: 22986 RVA: 0x001A7032 File Offset: 0x001A5232
			private void SimulateSettlementWaitForCaravan()
			{
				this._caravanWaitedInSettlementForHours++;
				if (this._caravanWaitedInSettlementForHours >= 5)
				{
					LeaveSettlementAction.ApplyForParty(this._questCaravanMobileParty);
					this._caravanWaitedInSettlementForHours = 0;
				}
			}

			// Token: 0x060059CB RID: 22987 RVA: 0x001A7060 File Offset: 0x001A5260
			private void NotifyPlayerOrCancelTheQuestIfCaravanIsFar()
			{
				if (this._questCaravanMobileParty.IsActive && !this._questCaravanMobileParty.IsVisible)
				{
					float num = this._questCaravanMobileParty.Position.Distance(MobileParty.MainParty.Position);
					float seeingRange = MobileParty.MainParty.SeeingRange;
					if (!this._isPlayerNotifiedForDanger && num >= seeingRange + 3f)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=2y9DhzCR}You are about to lose sight of the caravan. Find the caravan before they are in danger!", null), 0, null, null, "");
						this._isPlayerNotifiedForDanger = true;
						return;
					}
					if (num >= seeingRange + 20f)
					{
						base.AddLog(this.CaravanLostTheTrackLogText, false);
						this.FailConsequences(false);
					}
				}
			}

			// Token: 0x060059CC RID: 22988 RVA: 0x001A7104 File Offset: 0x001A5304
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party == this._questCaravanMobileParty)
				{
					this.AdjustCaravansSpeed();
					if (party.TargetSettlement == null || party.TargetSettlement == settlement)
					{
						this.TryToFindAndSetTargetToNextSettlement();
					}
					this._caravanWaitedInSettlementForHours = 0;
					this._questBanditPartyAlreadyAttacked = false;
					this._questCaravanMobileParty.Party.SetAsCameraFollowParty();
					if (base.IsTracked(settlement))
					{
						base.RemoveTrackedObject(settlement);
					}
				}
			}

			// Token: 0x060059CD RID: 22989 RVA: 0x001A7168 File Offset: 0x001A5368
			private void TryToFindAndSetTargetToNextSettlement()
			{
				int num = 0;
				int num2 = -1;
				do
				{
					num2 = SettlementHelper.FindNextSettlementAroundMobileParty(this._questCaravanMobileParty, MobileParty.NavigationType.Default, 150f, num2, null);
					if (num2 >= 0)
					{
						Settlement settlement = Settlement.All[num2];
						if (this.ProperSettlementCondition(settlement) && settlement != this._questCaravanMobileParty.HomeSettlement && (this._visitedSettlements.Count == 0 || settlement != this._visitedSettlements[this._visitedSettlements.Count - 1]) && !settlement.MapFaction.IsAtWarWith(this._questCaravanMobileParty.MapFaction))
						{
							num++;
						}
					}
				}
				while (num2 >= 0);
				if (num > 0)
				{
					int num3 = MBRandom.RandomInt(num);
					num2 = -1;
					Settlement settlement2;
					for (;;)
					{
						num2 = SettlementHelper.FindNextSettlementAroundMobileParty(this._questCaravanMobileParty, MobileParty.NavigationType.Default, 150f, num2, null);
						if (num2 >= 0)
						{
							settlement2 = Settlement.All[num2];
							if (this.ProperSettlementCondition(settlement2) && settlement2 != this._questCaravanMobileParty.HomeSettlement && (this._visitedSettlements.Count == 0 || settlement2 != this._visitedSettlements[this._visitedSettlements.Count - 1]) && !settlement2.MapFaction.IsAtWarWith(this._questCaravanMobileParty.MapFaction))
							{
								num3--;
								if (num3 < 0)
								{
									break;
								}
							}
						}
						if (num2 < 0)
						{
							return;
						}
					}
					Settlement settlement3 = settlement2;
					SetPartyAiAction.GetActionForVisitingSettlement(this._questCaravanMobileParty, settlement3, MobileParty.NavigationType.Default, false, false);
					this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(true);
					TextObject textObject = new TextObject("{=OjI8uGFa}We are traveling to {SETTLEMENT_NAME}.", null);
					textObject.SetTextVariable("SETTLEMENT_NAME", settlement3.Name);
					MBInformationManager.AddQuickInformation(textObject, 100, PartyBaseHelper.GetVisualPartyLeader(this._questCaravanMobileParty.Party), null, "");
					TextObject textObject2 = new TextObject("{=QDpfYm4c}The caravan is moving to {SETTLEMENT_NAME}.", null);
					textObject2.SetTextVariable("SETTLEMENT_NAME", settlement3.EncyclopediaLinkWithName);
					base.AddLog(textObject2, true);
					if (!base.IsTracked(settlement3))
					{
						base.AddTrackedObject(settlement3);
					}
					if (this._questBanditMobileParty != null)
					{
						if (this._questBanditMobileParty.IsActive)
						{
							float num4 = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(this._questCaravanMobileParty, this._questBanditMobileParty, MobileParty.NavigationType.Default);
							if (this._questBanditMobileParty.Speed < this._questCaravanMobileParty.Speed || num4 > 10f)
							{
								this._questBanditMobileParty.SetMoveModeHold();
								this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
								this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
								if (base.IsTracked(this._questBanditMobileParty))
								{
									base.RemoveTrackedObject(this._questBanditMobileParty);
								}
							}
						}
						this._questBanditMobileParty = null;
						return;
					}
					return;
				}
				this.CaravanNoTargetQuestSuccess();
			}

			// Token: 0x060059CE RID: 22990 RVA: 0x001A73EF File Offset: 0x001A55EF
			private void CaravanNoTargetQuestSuccess()
			{
				this.SuccessConsequences(true);
			}

			// Token: 0x060059CF RID: 22991 RVA: 0x001A73F8 File Offset: 0x001A55F8
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if (this._questCaravanMobileParty != null && mapEvent.InvolvedParties.Contains(this._questCaravanMobileParty.Party))
				{
					if (mapEvent.HasWinner)
					{
						bool flag = this._questCaravanMobileParty.MapEventSide == MobileParty.MainParty.MapEventSide && mapEvent.IsPlayerMapEvent;
						bool flag2 = mapEvent.Winner == this._questCaravanMobileParty.MapEventSide;
						bool flag3 = mapEvent.InvolvedParties.Contains(PartyBase.MainParty);
						if (!flag2)
						{
							if (!flag3)
							{
								base.AddLog(this.CaravanDestroyedByBanditsLogText, false);
								this.FailConsequences(true);
								return;
							}
							if (flag)
							{
								base.AddLog(this.CaravanDestroyedQuestLogText, false);
								this.FailConsequences(true);
								return;
							}
							this.FailByPlayerHostileConsequences();
							return;
						}
						else
						{
							if (this._questBanditMobileParty != null && this._questBanditMobileParty.IsActive && mapEvent.InvolvedParties.Contains(this._questBanditMobileParty.Party))
							{
								DestroyPartyAction.Apply(MobileParty.MainParty.Party, this._questBanditMobileParty);
							}
							if (this._otherBanditParty != null && this._otherBanditParty.IsActive && mapEvent.InvolvedParties.Contains(this._otherBanditParty.Party))
							{
								DestroyPartyAction.Apply(MobileParty.MainParty.Party, this._otherBanditParty);
							}
							if (this._questCaravanMobileParty.MemberRoster.TotalManCount <= 0)
							{
								this.FailConsequences(true);
							}
							if (this._questCaravanMobileParty.Speed < 2f)
							{
								this._questCaravanMobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse"), 5);
								return;
							}
						}
					}
					else if (this._questCaravanMobileParty.MemberRoster.TotalManCount <= 0)
					{
						this.FailConsequences(true);
					}
				}
			}

			// Token: 0x060059D0 RID: 22992 RVA: 0x001A75A4 File Offset: 0x001A57A4
			private void SuccessConsequences(bool isNoTargetLeftSuccess)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.TotalRewardGold, false);
				base.QuestGiver.AddPower(10f);
				this.RelationshipChangeWithQuestGiver = 5;
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity += 10f;
				if (isNoTargetLeftSuccess)
				{
					base.AddLog(this.CaravanNoTargetLogText, false);
				}
				else
				{
					base.AddLog(this.SuccessQuestLogText, true);
				}
				MobileParty questBanditMobileParty = this._questBanditMobileParty;
				if (questBanditMobileParty != null)
				{
					questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
				}
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060059D1 RID: 22993 RVA: 0x001A765C File Offset: 0x001A585C
			private void FailConsequences(bool banditsWon = false)
			{
				base.QuestGiver.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = -5;
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 10f;
				if (this._questBanditMobileParty != null)
				{
					this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
					if (base.IsTracked(this._questBanditMobileParty))
					{
						base.RemoveTrackedObject(this._questBanditMobileParty);
					}
				}
				if (this._questCaravanMobileParty != null)
				{
					this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					this._questCaravanMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
				if (this._questBanditMobileParty != null && !banditsWon)
				{
					if (base.IsTracked(this._questBanditMobileParty))
					{
						base.RemoveTrackedObject(this._questBanditMobileParty);
					}
					this._questBanditMobileParty.SetPartyUsedByQuest(false);
					this._questBanditMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
					if (this._questBanditMobileParty.IsActive && this._questBanditMobileParty.IsVisible)
					{
						DestroyPartyAction.Apply(null, this._questBanditMobileParty);
					}
				}
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060059D2 RID: 22994 RVA: 0x001A779C File Offset: 0x001A599C
			private void FailByPlayerHostileConsequences()
			{
				base.QuestGiver.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = -10;
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -80)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 20f;
				base.AddLog(this.CaravanDestroyedByPlayerQuestLogText, true);
				MobileParty questBanditMobileParty = this._questBanditMobileParty;
				if (questBanditMobileParty != null)
				{
					questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(false);
				}
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060059D3 RID: 22995 RVA: 0x001A782E File Offset: 0x001A5A2E
			protected override void InitializeQuestOnGameLoad()
			{
				MobileParty questCaravanMobileParty = this._questCaravanMobileParty;
				if (questCaravanMobileParty != null && questCaravanMobileParty.IsCaravan)
				{
					base.CompleteQuestWithCancel(null);
				}
				this.SetDialogs();
			}

			// Token: 0x060059D4 RID: 22996 RVA: 0x001A7854 File Offset: 0x001A5A54
			private void ActivateBanditParty()
			{
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(this._questCaravanMobileParty, this._questCaravanMobileParty.NavigationCapability, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				PartyTemplateObject partyTemplateObject = Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("kingdom_hero_party_caravan_ambushers") ?? clan.DefaultPartyTemplate;
				this._questBanditMobileParty = BanditPartyComponent.CreateBanditParty("escort_caravan_quest_" + base.StringId, clan, closestHideout.Settlement.Hideout, false, partyTemplateObject, this._questCaravanMobileParty.TargetSettlement.GatePosition);
				this._questBanditMobileParty.Party.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
				Campaign.Current.MobilePartyLocator.UpdateLocator(this._questBanditMobileParty);
				this._questBanditMobileParty.ActualClan = clan;
				this._questBanditMobileParty.MemberRoster.Clear();
				for (int i = 0; i < this.BanditPartyTroopCount; i++)
				{
					List<ValueTuple<PartyTemplateStack, float>> list = new List<ValueTuple<PartyTemplateStack, float>>();
					foreach (PartyTemplateStack partyTemplateStack in partyTemplateObject.Stacks)
					{
						list.Add(new ValueTuple<PartyTemplateStack, float>(partyTemplateStack, (float)(64 - partyTemplateStack.Character.Level)));
					}
					PartyTemplateStack partyTemplateStack2 = MBRandom.ChooseWeighted<PartyTemplateStack>(list);
					this._questBanditMobileParty.MemberRoster.AddToCounts(partyTemplateStack2.Character, 1, false, 0, 0, true, -1);
				}
				this._questBanditMobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, this.BanditPartyTroopCount);
				this._questBanditMobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse"), this.BanditPartyTroopCount);
				this._questBanditMobileParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
				SetPartyAiAction.GetActionForEngagingParty(this._questBanditMobileParty, this._questCaravanMobileParty, MobileParty.NavigationType.Default, false);
				this._questBanditMobileParty.Ai.SetDoNotMakeNewDecisions(true);
				base.AddTrackedObject(this._questBanditMobileParty);
				this._questBanditMobileParty.InitializePartyTrade(QuestHelper.CalculateInitialGoldForBanditQuestParty(this._questBanditMobileParty));
			}

			// Token: 0x060059D5 RID: 22997 RVA: 0x001A7AA0 File Offset: 0x001A5CA0
			protected override void OnFinalize()
			{
				if (this._questCaravanMobileParty != null && this._questCaravanMobileParty.IsActive && this._questCaravanMobileParty.IsCustomParty)
				{
					CaravanPartyComponent.ConvertPartyToCaravanParty(this._questCaravanMobileParty, base.QuestGiver, base.QuestGiver.CurrentSettlement, false, null, null, false);
					this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(false);
					this._questCaravanMobileParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
				if (this._questCaravanMobileParty != null)
				{
					base.RemoveTrackedObject(this._questCaravanMobileParty);
				}
				if (this._otherBanditParty != null && this._otherBanditParty.IsActive)
				{
					this._otherBanditParty.Ai.SetDoNotMakeNewDecisions(false);
					this._otherBanditParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
			}

			// Token: 0x060059D6 RID: 22998 RVA: 0x001A7B5C File Offset: 0x001A5D5C
			protected override void OnTimedOut()
			{
				base.QuestGiver.AddPower(-5f);
				this.RelationshipChangeWithQuestGiver = -5;
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 20f;
				base.AddLog(new TextObject("{=pUrSIed8}You have failed to escort the caravan to its destination.", null), false);
			}

			// Token: 0x04001D91 RID: 7569
			private const int BattleFakeSimulationDuration = 3;

			// Token: 0x04001D92 RID: 7570
			private const string CustomPartyComponentTalkId = "escort_caravan_talk";

			// Token: 0x04001D93 RID: 7571
			[SaveableField(2)]
			private readonly int _requiredSettlementNumber;

			// Token: 0x04001D94 RID: 7572
			[SaveableField(3)]
			private List<Settlement> _visitedSettlements;

			// Token: 0x04001D95 RID: 7573
			[SaveableField(4)]
			private MobileParty _questCaravanMobileParty;

			// Token: 0x04001D96 RID: 7574
			[SaveableField(5)]
			private MobileParty _questBanditMobileParty;

			// Token: 0x04001D97 RID: 7575
			[SaveableField(7)]
			private readonly float _difficultyMultiplier;

			// Token: 0x04001D98 RID: 7576
			[SaveableField(12)]
			private bool _isPlayerNotifiedForDanger;

			// Token: 0x04001D99 RID: 7577
			[SaveableField(26)]
			private MobileParty _otherBanditParty;

			// Token: 0x04001D9A RID: 7578
			[SaveableField(30)]
			private int _questBanditPartyFollowDuration;

			// Token: 0x04001D9B RID: 7579
			[SaveableField(31)]
			private int _otherBanditPartyFollowDuration;

			// Token: 0x04001D9C RID: 7580
			[SaveableField(11)]
			private int _daysSpentForEscorting = 1;

			// Token: 0x04001D9D RID: 7581
			private int _caravanWaitedInSettlementForHours;

			// Token: 0x04001D9E RID: 7582
			[SaveableField(23)]
			private bool _questBanditPartyAlreadyAttacked;

			// Token: 0x04001D9F RID: 7583
			private CustomPartyComponent _customPartyComponent;

			// Token: 0x04001DA0 RID: 7584
			[SaveableField(1)]
			private JournalLog _playerStartsQuestLog;
		}

		// Token: 0x02000732 RID: 1842
		public class EscortMerchantCaravanIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060059D9 RID: 23001 RVA: 0x001A7BF3 File Offset: 0x001A5DF3
			public EscortMerchantCaravanIssueTypeDefiner()
				: base(450000)
			{
			}

			// Token: 0x060059DA RID: 23002 RVA: 0x001A7C00 File Offset: 0x001A5E00
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssue), 1, null);
				base.AddClassDefinition(typeof(EscortMerchantCaravanIssueBehavior.EscortMerchantCaravanIssueQuest), 2, null);
			}
		}
	}
}
