using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D1 RID: 209
	public class BarberCampaignBehavior : CampaignBehaviorBase, IFacegenCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x060008F5 RID: 2293 RVA: 0x000415FE File Offset: 0x0003F7FE
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0004162E File Offset: 0x0003F82E
		public override void SyncData(IDataStore store)
		{
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00041630 File Offset: 0x0003F830
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0004163C File Offset: 0x0003F83C
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("barber_start_talk_beggar", "start", "close_window", "{=pWzdxd7O}May the Heavens bless you, my poor {?PLAYER.GENDER}lady{?}fellow{\\?}, but I can't spare a coin right now.", new ConversationSentence.OnConditionDelegate(this.InDisguiseSpeakingToBarber), new ConversationSentence.OnConsequenceDelegate(this.InitializeBarberConversation), 100, null);
			campaignGameStarter.AddDialogLine("barber_start_talk", "start", "barber_question1", "{=2aXYYNBG}Come to have your hair cut, {?PLAYER.GENDER}my lady{?}my lord{\\?}? A new look for a new day?", new ConversationSentence.OnConditionDelegate(this.IsConversationAgentBarber), new ConversationSentence.OnConsequenceDelegate(this.InitializeBarberConversation), 100, null);
			campaignGameStarter.AddPlayerLine("player_accept_haircut", "barber_question1", "start_cut_token", "{=Q7wBRXtR}Yes, I have. ({GOLD_COST}{GOLD_ICON})", new ConversationSentence.OnConditionDelegate(this.GivePlayerAHaircutCondition), new ConversationSentence.OnConsequenceDelegate(this.GivePlayerAHaircut), 100, new ConversationSentence.OnClickableConditionDelegate(this.DoesPlayerHaveEnoughGold), null);
			campaignGameStarter.AddPlayerLine("player_refuse_haircut", "barber_question1", "no_haircut_conversation_token", "{=xPAAZAaI}My hair is fine as it is, thank you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("barber_ask_if_done", "start_cut_token", "finish_cut_token", "{=M3K8wUOO}So... Does this please you, {?PLAYER.GENDER}my lady{?}my lord{\\?}?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("player_done_with_haircut", "finish_cut_token", "finish_barber", "{=zTF4bJm0}Yes, it's fine.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("player_not_done_with_haircut", "finish_cut_token", "start_cut_token", "{=BnoSOi3r}Actually...", new ConversationSentence.OnConditionDelegate(this.GivePlayerAHaircutCondition), new ConversationSentence.OnConsequenceDelegate(this.GivePlayerAHaircut), 100, new ConversationSentence.OnClickableConditionDelegate(this.DoesPlayerHaveEnoughGold), null);
			campaignGameStarter.AddDialogLine("barber_no_haircut_talk", "no_haircut_conversation_token", "close_window", "{=BusYGTrN}Excellent! Have a good day, then, {?PLAYER.GENDER}my lady{?}my lord{\\?}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("barber_haircut_finished", "finish_barber", "player_had_a_haircut_token", "{=akqJbZpH}Marvellous! You cut a splendid appearance, {?PLAYER.GENDER}my lady{?}my lord{\\?}, if you don't mind my saying. Most splendid.", new ConversationSentence.OnConditionDelegate(this.DidPlayerHaveAHaircut), new ConversationSentence.OnConsequenceDelegate(this.ChargeThePlayer), 100, null);
			campaignGameStarter.AddDialogLine("barber_haircut_no_change", "finish_barber", "player_did_not_cut_token", "{=yLIZlaS1}Very well. Do come back when you're ready, {?PLAYER.GENDER}my lady{?}my lord{\\?}.", new ConversationSentence.OnConditionDelegate(this.DidPlayerNotHaveAHaircut), null, 100, null);
			campaignGameStarter.AddPlayerLine("player_no_haircut_finish_talk", "player_did_not_cut_token", "close_window", "{=oPUVNuhN}I'll keep you in mind", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("player_haircut_finish_talk", "player_had_a_haircut_token", "close_window", "{=F9Xjbchh}Thank you.", null, null, 100, null, null);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0004185E File Offset: 0x0003FA5E
		private bool InDisguiseSpeakingToBarber()
		{
			return this.IsConversationAgentBarber() && Campaign.Current.IsMainHeroDisguised;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00041874 File Offset: 0x0003FA74
		private bool DoesPlayerHaveEnoughGold(out TextObject explanation)
		{
			if (Hero.MainHero.Gold < 100)
			{
				explanation = new TextObject("{=RYJdU43V}Not Enough Gold", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00041897 File Offset: 0x0003FA97
		private void ChargeThePlayer()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 100, false);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000418A7 File Offset: 0x0003FAA7
		private bool DidPlayerNotHaveAHaircut()
		{
			return !this.DidPlayerHaveAHaircut();
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000418B4 File Offset: 0x0003FAB4
		private bool DidPlayerHaveAHaircut()
		{
			return Hero.MainHero.BodyProperties.StaticProperties != this._previousBodyProperties;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x000418DE File Offset: 0x0003FADE
		private bool IsConversationAgentBarber()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return ((currentSettlement != null) ? currentSettlement.Culture.Barber : null) == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000418FD File Offset: 0x0003FAFD
		private bool GivePlayerAHaircutCondition()
		{
			MBTextManager.SetTextVariable("GOLD_COST", 100);
			return true;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0004190C File Offset: 0x0003FB0C
		private void GivePlayerAHaircut()
		{
			this._isOpenedFromBarberDialogue = true;
			BarberState barberState = Game.Current.GameStateManager.CreateState<BarberState>(new object[]
			{
				Hero.MainHero.CharacterObject,
				this.GetFaceGenFilter()
			});
			this._isOpenedFromBarberDialogue = false;
			GameStateManager.Current.PushState(barberState, 0);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00041960 File Offset: 0x0003FB60
		private void InitializeBarberConversation()
		{
			this._previousBodyProperties = Hero.MainHero.BodyProperties.StaticProperties;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00041988 File Offset: 0x0003FB88
		private LocationCharacter CreateBarber(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject barber = culture.Barber;
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(barber, out num, out num2, "Barber");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(barber, -1, null, default(UniqueTroopDescriptor))).Monster(FaceGen.GetMonsterWithSuffix(barber.Race, "_settlement_slow")).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_barber", true, relation, null, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00041A1C File Offset: 0x0003FC1C
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("center");
			int num;
			if (CampaignMission.Current.Location == locationWithId && Campaign.Current.IsDay && unusedUsablePointCount.TryGetValue("sp_merchant_notary", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateBarber), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00041A84 File Offset: 0x0003FC84
		public IFaceGeneratorCustomFilter GetFaceGenFilter()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			if (Settlement.CurrentSettlement != null)
			{
				list.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetHairIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, Settlement.CurrentSettlement.Culture));
				list2.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetBeardIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, Settlement.CurrentSettlement.Culture));
			}
			else
			{
				foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
				{
					list.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetHairIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, cultureObject));
					list2.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetBeardIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, cultureObject));
				}
			}
			return new BarberCampaignBehavior.BarberFaceGeneratorCustomFilter(!this._isOpenedFromBarberDialogue, list.Distinct<int>().ToArray<int>(), list2.Distinct<int>().ToArray<int>());
		}

		// Token: 0x0400046E RID: 1134
		private const int BarberCost = 100;

		// Token: 0x0400046F RID: 1135
		private bool _isOpenedFromBarberDialogue;

		// Token: 0x04000470 RID: 1136
		private StaticBodyProperties _previousBodyProperties;

		// Token: 0x020001FE RID: 510
		private class BarberFaceGeneratorCustomFilter : IFaceGeneratorCustomFilter
		{
			// Token: 0x060013DC RID: 5084 RVA: 0x0007983C File Offset: 0x00077A3C
			public BarberFaceGeneratorCustomFilter(bool useDefaultStages, int[] haircutIndices, int[] faircutIndices)
			{
				this._haircutIndices = haircutIndices;
				this._facialHairIndices = faircutIndices;
				this._defaultStages = useDefaultStages;
			}

			// Token: 0x060013DD RID: 5085 RVA: 0x00079859 File Offset: 0x00077A59
			public int[] GetHaircutIndices(BasicCharacterObject character)
			{
				return this._haircutIndices;
			}

			// Token: 0x060013DE RID: 5086 RVA: 0x00079861 File Offset: 0x00077A61
			public int[] GetFacialHairIndices(BasicCharacterObject character)
			{
				return this._facialHairIndices;
			}

			// Token: 0x060013DF RID: 5087 RVA: 0x00079869 File Offset: 0x00077A69
			public FaceGeneratorStage[] GetAvailableStages()
			{
				if (this._defaultStages)
				{
					return new FaceGeneratorStage[]
					{
						FaceGeneratorStage.Body,
						FaceGeneratorStage.Face,
						FaceGeneratorStage.Eyes,
						FaceGeneratorStage.Nose,
						FaceGeneratorStage.Mouth,
						FaceGeneratorStage.Hair,
						FaceGeneratorStage.Taint
					};
				}
				return new FaceGeneratorStage[] { FaceGeneratorStage.Hair };
			}

			// Token: 0x0400093F RID: 2367
			private readonly int[] _haircutIndices;

			// Token: 0x04000940 RID: 2368
			private readonly int[] _facialHairIndices;

			// Token: 0x04000941 RID: 2369
			private readonly bool _defaultStages;
		}
	}
}
