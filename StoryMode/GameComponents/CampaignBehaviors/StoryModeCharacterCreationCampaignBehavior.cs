using System;
using System.Collections.Generic;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000053 RID: 83
	public class StoryModeCharacterCreationCampaignBehavior : CampaignBehaviorBase, ICharacterCreationContentHandler
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x0001C647 File Offset: 0x0001A847
		private CharacterCreationManager _characterCreationManager
		{
			get
			{
				CharacterCreationState characterCreationState = GameStateManager.Current.ActiveState as CharacterCreationState;
				if (characterCreationState == null)
				{
					return null;
				}
				return characterCreationState.CharacterCreationManager;
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001C664 File Offset: 0x0001A864
		public override void RegisterEvents()
		{
			CampaignEvents.OnCharacterCreationInitializedEvent.AddNonSerializedListener(this, new Action<CharacterCreationManager>(this.OnCharacterCreationInitialized));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001C6B8 File Offset: 0x0001A8B8
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.1.52060", 0)) && Hero.MainHero.StringId == "main_hero")
			{
				if (Hero.MainHero.Father == null)
				{
					Hero.MainHero.Father = StoryModeHeroes.MainHeroFather;
				}
				if (Hero.MainHero.Mother == null)
				{
					Hero.MainHero.Mother = StoryModeHeroes.MainHeroMother;
				}
				if (!Hero.MainHero.Father.IsDead && !Hero.MainHero.Mother.IsDead)
				{
					if (Hero.MainHero.Father.Spouse == null)
					{
						Hero.MainHero.Father.Spouse = Hero.MainHero.Mother;
					}
					if (Hero.MainHero.Mother.Spouse == null)
					{
						Hero.MainHero.Mother.Spouse = Hero.MainHero.Father;
					}
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001C7A7 File Offset: 0x0001A9A7
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001C7A9 File Offset: 0x0001A9A9
		private void OnCharacterCreationIsOver(int index)
		{
			if (index == 1)
			{
				this.UpdateHomeSettlementsOfFamily();
				this.FinalizeFamilyStory();
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001C7BC File Offset: 0x0001A9BC
		private void UpdateHomeSettlementsOfFamily()
		{
			Settlement homeSettlement = Hero.MainHero.HomeSettlement;
			StoryModeHeroes.MainHeroFather.BornSettlement = homeSettlement;
			StoryModeHeroes.MainHeroFather.UpdateHomeSettlement();
			StoryModeHeroes.MainHeroMother.BornSettlement = homeSettlement;
			StoryModeHeroes.MainHeroMother.UpdateHomeSettlement();
			StoryModeHeroes.LittleBrother.BornSettlement = homeSettlement;
			StoryModeHeroes.LittleBrother.UpdateHomeSettlement();
			StoryModeHeroes.LittleSister.BornSettlement = homeSettlement;
			StoryModeHeroes.LittleSister.UpdateHomeSettlement();
			StoryModeHeroes.ElderBrother.BornSettlement = homeSettlement;
			StoryModeHeroes.ElderBrother.UpdateHomeSettlement();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001C840 File Offset: 0x0001AA40
		private void FinalizeFamilyStory()
		{
			TextObject textObject = new TextObject("{=h68qCoz3}{PLAYER_LITTLE_BROTHER.NAME} is the little brother of {PLAYER.LINK}. He has been abducted by bandits, who intend to sell him into slavery.", null);
			StringHelpers.SetCharacterProperties("PLAYER_LITTLE_BROTHER", StoryModeHeroes.LittleBrother.CharacterObject, textObject, false);
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
			StoryModeHeroes.LittleBrother.EncyclopediaText = textObject;
			TextObject textObject2 = GameTexts.FindText("little_sister_encyclopedia_text", null);
			StringHelpers.SetCharacterProperties("PLAYER_LITTLE_SISTER", StoryModeHeroes.LittleSister.CharacterObject, textObject2, false);
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject2, false);
			StoryModeHeroes.LittleSister.EncyclopediaText = textObject2;
			TextObject textObject3 = new TextObject("{=XmvaRfLM}{PLAYER_FATHER.NAME} was the father of {PLAYER.LINK}. He was slain when raiders attacked the inn at which his family was staying.", null);
			StringHelpers.SetCharacterProperties("PLAYER_FATHER", StoryModeHeroes.MainHeroFather.CharacterObject, textObject3, false);
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject3, false);
			StoryModeHeroes.MainHeroFather.EncyclopediaText = textObject3;
			TextObject textObject4 = new TextObject("{=hrhvEWP8}{PLAYER_MOTHER.NAME} was the mother of {PLAYER.LINK}. She was slain when raiders attacked the inn at which her family was staying.", null);
			StringHelpers.SetCharacterProperties("PLAYER_MOTHER", StoryModeHeroes.MainHeroMother.CharacterObject, textObject4, false);
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject4, false);
			StoryModeHeroes.MainHeroMother.EncyclopediaText = textObject4;
			TextObject textObject5 = new TextObject("{=bsWSecYa}{PLAYER_BROTHER.NAME} is the elder brother of {PLAYER.LINK}. He has gone in search of the family's two youngest siblings, {PLAYER_LITTLE_BROTHER.NAME} and {PLAYER_LITTLE_SISTER.NAME}.", null);
			StringHelpers.SetCharacterProperties("PLAYER_BROTHER", StoryModeHeroes.ElderBrother.CharacterObject, textObject5, false);
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject5, false);
			StringHelpers.SetCharacterProperties("PLAYER_LITTLE_BROTHER", StoryModeHeroes.LittleBrother.CharacterObject, textObject5, false);
			StringHelpers.SetCharacterProperties("PLAYER_LITTLE_SISTER", StoryModeHeroes.LittleSister.CharacterObject, textObject5, false);
			StoryModeHeroes.ElderBrother.EncyclopediaText = textObject5;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
		private void OnCharacterCreationInitialized(CharacterCreationManager characterCreationManager)
		{
			this._focusToAdd = characterCreationManager.CharacterCreationContent.FocusToAdd;
			this._skillLevelToAdd = characterCreationManager.CharacterCreationContent.SkillLevelToAdd;
			this._attributeLevelToAdd = characterCreationManager.CharacterCreationContent.AttributeLevelToAdd;
			characterCreationManager.RegisterCharacterCreationContentHandler(this, 900);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001CA10 File Offset: 0x0001AC10
		public void InitializeCharacterCreationStages(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.RemoveStage<CharacterCreationBannerEditorStage>();
			characterCreationManager.RemoveStage<CharacterCreationClanNamingStage>();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001CA20 File Offset: 0x0001AC20
		public void InitializeData(CharacterCreationManager characterCreationManager)
		{
			Hero.MainHero.Mother = StoryModeHeroes.MainHeroMother;
			Hero.MainHero.Father = StoryModeHeroes.MainHeroFather;
			characterCreationManager.CharacterCreationContent.ChangeReviewPageDescription(new TextObject("{=wbhKgpmr}You prepare to set off with your brother on a mission of vengeance and rescue. Here is your character. Continue if you are ready, or go back to make changes.", null));
			characterCreationManager.DeleteNarrativeMenuWithId("narrative_age_selection_menu");
			this.AddEscapeMenu(characterCreationManager);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001CA73 File Offset: 0x0001AC73
		void ICharacterCreationContentHandler.InitializeContent(CharacterCreationManager characterCreationManager)
		{
			this.InitializeCharacterCreationStages(characterCreationManager);
			this.InitializeData(characterCreationManager);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001CA83 File Offset: 0x0001AC83
		void ICharacterCreationContentHandler.AfterInitializeContent(CharacterCreationManager characterCreationManager)
		{
			this.ModifyParentMenu(characterCreationManager);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001CA8C File Offset: 0x0001AC8C
		void ICharacterCreationContentHandler.OnStageCompleted(CharacterCreationStageBase stage)
		{
			if (stage is CharacterCreationFaceGeneratorStage)
			{
				this.FaceGenUpdated();
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001CA9C File Offset: 0x0001AC9C
		void ICharacterCreationContentHandler.OnCharacterCreationFinalize(CharacterCreationManager characterCreationManager)
		{
			this.ApplyCulture(this._characterCreationManager.CharacterCreationContent.SelectedCulture);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		private void ApplyCulture(CultureObject culture)
		{
			StoryModeHeroes.LittleBrother.Culture = culture;
			StoryModeHeroes.LittleSister.Culture = culture;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001CACC File Offset: 0x0001ACCC
		private void FaceGenUpdated()
		{
			NarrativeMenu narrativeMenuWithId = this._characterCreationManager.GetNarrativeMenuWithId("narrative_parent_menu");
			BodyProperties bodyProperties = BodyProperties.Default;
			BodyProperties bodyProperties2 = BodyProperties.Default;
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in narrativeMenuWithId.Characters)
			{
				if (narrativeMenuCharacter.StringId == "mother_character")
				{
					bodyProperties = narrativeMenuCharacter.BodyProperties;
				}
				if (narrativeMenuCharacter.StringId == "father_character")
				{
					bodyProperties2 = narrativeMenuCharacter.BodyProperties;
				}
			}
			Hero elderBrother = StoryModeHeroes.ElderBrother;
			uint hashCode = (uint)Hero.MainHero.BodyProperties.GetHashCode();
			string text = Hero.MainHero.Culture.ToString().ToLower() + ",";
			string text2 = Hero.MainHero.Culture.ToString().ToLower() + ",";
			int num = Hero.MainHero.RandomIntWithSeed(hashCode, 1, 100);
			this.CreateSibling(StoryModeHeroes.LittleBrother, bodyProperties, bodyProperties2, hashCode + 1U);
			this.CreateSibling(StoryModeHeroes.LittleSister, bodyProperties, bodyProperties2, hashCode + 2U);
			BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(elderBrother.CharacterObject.Race, elderBrother.IsFemale, bodyProperties, bodyProperties2, 1, num, text, text2, Hero.MainHero.Father.CharacterObject.BodyPropertyRange.TattooTags, 0f);
			randomBodyProperties = new BodyProperties(new DynamicBodyProperties(elderBrother.Age, 0.5f, 0.5f), randomBodyProperties.StaticProperties);
			elderBrother.StaticBodyProperties = randomBodyProperties.StaticProperties;
			elderBrother.Weight = randomBodyProperties.Weight;
			elderBrother.Build = randomBodyProperties.Build;
			foreach (NarrativeMenu narrativeMenu in this._characterCreationManager.NarrativeMenus)
			{
				foreach (NarrativeMenuCharacter narrativeMenuCharacter2 in narrativeMenu.Characters)
				{
					if (narrativeMenuCharacter2.StringId.Equals("player_escape_character"))
					{
						narrativeMenuCharacter2.UpdateBodyProperties(CharacterObject.PlayerCharacter.GetBodyProperties(null, -1), CharacterObject.PlayerCharacter.Race, false);
					}
					if (narrativeMenuCharacter2.StringId.Equals("brother_character"))
					{
						narrativeMenuCharacter2.UpdateBodyProperties(elderBrother.BodyProperties, CharacterObject.PlayerCharacter.Race, false);
					}
				}
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001CD64 File Offset: 0x0001AF64
		private void ModifyParentMenu(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuOption narrativeMenuOption in characterCreationManager.GetNarrativeMenuWithId("narrative_parent_menu").CharacterCreationMenuOptions)
			{
				narrativeMenuOption.SetOnConsequence(new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeParentsAndLittleSiblings));
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001CDCC File Offset: 0x0001AFCC
		private List<NarrativeMenuCharacterArgs> GetEscapeMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
			string text = "brother_char_creation_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId;
			list.Add(new NarrativeMenuCharacterArgs("brother_character", (int)StoryModeHeroes.ElderBrother.Age, text, "act_childhood_schooled", "spawnpoint_brother_brother_stage", "", "", null, true, false));
			string selectedTitleType = characterCreationManager.CharacterCreationContent.SelectedTitleType;
			string text2 = "player_char_creation_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId + "_" + selectedTitleType.ToString().ToLower();
			text2 += (Hero.MainHero.IsFemale ? "_f" : "_m");
			list.Add(new NarrativeMenuCharacterArgs("player_escape_character", (int)CharacterObject.PlayerCharacter.Age, text2, "act_childhood_schooled", "spawnpoint_player_brother_stage", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
			return list;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001CEC0 File Offset: 0x0001B0C0
		private void AddEscapeMenu(CharacterCreationManager characterCreationManager)
		{
			MBTextManager.SetTextVariable("EXP_VALUE", this._skillLevelToAdd);
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			NarrativeMenu narrativeMenuWithId = characterCreationManager.GetNarrativeMenuWithId("narrative_parent_menu");
			BodyProperties bodyProperties = BodyProperties.Default;
			BodyProperties bodyProperties2 = BodyProperties.Default;
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in narrativeMenuWithId.Characters)
			{
				if (narrativeMenuCharacter.StringId == "mother_character")
				{
					bodyProperties = narrativeMenuCharacter.BodyProperties;
				}
				if (narrativeMenuCharacter.StringId == "father_character")
				{
					bodyProperties2 = narrativeMenuCharacter.BodyProperties;
				}
			}
			Hero elderBrother = StoryModeHeroes.ElderBrother;
			uint hashCode = (uint)Hero.MainHero.BodyProperties.GetHashCode();
			string text = Hero.MainHero.Culture.ToString().ToLower() + ",";
			string text2 = Hero.MainHero.Culture.ToString().ToLower() + ",";
			int num = Hero.MainHero.RandomIntWithSeed(hashCode, 1, 100);
			this.CreateSibling(StoryModeHeroes.LittleBrother, bodyProperties, bodyProperties2, hashCode + 1U);
			this.CreateSibling(StoryModeHeroes.LittleSister, bodyProperties, bodyProperties2, hashCode + 2U);
			BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(elderBrother.CharacterObject.Race, elderBrother.IsFemale, bodyProperties, bodyProperties2, 1, num, text, text2, Hero.MainHero.Father.CharacterObject.BodyPropertyRange.TattooTags, 0f);
			randomBodyProperties = new BodyProperties(new DynamicBodyProperties(elderBrother.Age, 0.5f, 0.5f), randomBodyProperties.StaticProperties);
			elderBrother.StaticBodyProperties = randomBodyProperties.StaticProperties;
			elderBrother.Weight = randomBodyProperties.Weight;
			elderBrother.Build = randomBodyProperties.Build;
			NarrativeMenuCharacter narrativeMenuCharacter2 = new NarrativeMenuCharacter("brother_character", randomBodyProperties, elderBrother.CharacterObject.Race, elderBrother.CharacterObject.IsFemale);
			list.Add(narrativeMenuCharacter2);
			NarrativeMenuCharacter narrativeMenuCharacter3 = new NarrativeMenuCharacter("player_escape_character", Hero.MainHero.BodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
			list.Add(narrativeMenuCharacter3);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_escape_menu", "narrative_adulthood_menu", "", new TextObject("{=peNBA0WW}Story Background", null), new TextObject("{=jg3T5AyE}Like many families in Calradia, your life was upended by war. Your home was ravaged by the passage of army after army. Eventually, you sold your property and set off with your father, mother, brother, and your two younger siblings to a new town you'd heard was safer. But you did not make it. Along the way, the inn at which you were staying was attacked by raiders. Your parents were slain and your two youngest siblings seized, but you and your brother survived because...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetEscapeMenuNarrativeMenuCharacterArgs));
			this.AddEscapeNarrativeMenuOptions(narrativeMenu);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001D134 File Offset: 0x0001B334
		private void AddEscapeNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("escape_subdued_raider_option", new TextObject("{=6vCHovVH}you subdued a raider.", null), new TextObject("{=CvBoRaFv}You were able to grab a knife in the confusion of the attack. You stabbed a raider blocking your way.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeSubduedRaiderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EscapeSubduedRaiderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeSubduedRaiderNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("escape_arrow_option", new TextObject("{=2XhW49TX}you drove them off with arrows.", null), new TextObject("{=ccf67J3J}You grabbed a bow and sent a few arrows the raiders' way. They took cover, giving you the opportunity to flee with your brother.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeArrowNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EscapeArrowNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeArrowNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("escape_horse_option", new TextObject("{=gOI8lKcl}you rode off on a fast horse.", null), new TextObject("{=cepWNzEA}Jumping on the two remaining horses in the inn's burning stable, you and your brother broke out of the encircling raiders and rode off.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeHorseNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EscapeHorseNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeHorseNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("escape_tricked_option", new TextObject("{=EdUppdLZ}you tricked the raiders.", null), new TextObject("{=ZqOvtLBM}In the confusion of the attack you shouted that someone had found treasure in the back room. You then made your way out of the undefended entrance with your brother.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeTrickedNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EscapeTrickedNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeTrickedNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("escape_breakout_option", new TextObject("{=qhAhPWdp}you organized the travelers to break out.", null), new TextObject("{=Lmfi0cYk}You encouraged the few travellers in the inn to break out in a coordinated fashion. Raiders killed or captured most but you and your brother were able to escape.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeBreakOutNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EscapeBreakOutNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeBreakOutNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("escape_makeshift_fortification_option", new TextObject("{=7AEw4RbK}you threw up makeshift fortifications.", null), new TextObject("{=Lmfi0cYk}You encouraged the few travellers in the inn to break out in a coordinated fashion. Raiders killed or captured most but you and your brother were able to escape.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetMakeshiftFortificationNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.MakeshiftFortificationNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.MakeshiftFortificationNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001D358 File Offset: 0x0001B558
		private void GetEscapeSubduedRaiderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.OneHanded,
				DefaultSkills.Athletics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001D3AC File Offset: 0x0001B5AC
		private bool EscapeSubduedRaiderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001D3B0 File Offset: 0x0001B5B0
		private void EscapeSubduedRaiderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string text = "act_childhood_fierce";
			string text2 = "act_childhood_athlete";
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text);
				}
				if (narrativeMenuCharacter.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text2);
				}
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001D440 File Offset: 0x0001B640
		private void GetEscapeArrowNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Bow,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001D494 File Offset: 0x0001B694
		private bool EscapeArrowNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001D498 File Offset: 0x0001B698
		private void EscapeArrowNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string text = "act_childhood_athlete";
			string text2 = "act_childhood_sharp";
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text);
				}
				if (narrativeMenuCharacter.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text2);
				}
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001D528 File Offset: 0x0001B728
		private void GetEscapeHorseNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Scouting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001D57C File Offset: 0x0001B77C
		private bool EscapeHorseNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001D580 File Offset: 0x0001B780
		private void EscapeHorseNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string text = "act_childhood_tough";
			string text2 = "act_childhood_decisive";
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text);
				}
				if (narrativeMenuCharacter.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text2);
				}
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001D610 File Offset: 0x0001B810
		private void GetEscapeTrickedNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001D664 File Offset: 0x0001B864
		private bool EscapeTrickedNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001D668 File Offset: 0x0001B868
		private void EscapeTrickedNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string text = "act_childhood_ready_handshield";
			string text2 = "act_aserai_aserai_mp_archer_idle";
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text);
				}
				if (narrativeMenuCharacter.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text2);
				}
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001D6F8 File Offset: 0x0001B8F8
		private void GetEscapeBreakOutNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Leadership,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001D74C File Offset: 0x0001B94C
		private bool EscapeBreakOutNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001D750 File Offset: 0x0001B950
		private void EscapeBreakOutNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string text = "act_childhood_manners";
			string text2 = "act_childhood_tough";
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text);
				}
				if (narrativeMenuCharacter.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text2);
				}
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001D7E0 File Offset: 0x0001B9E0
		private void GetMakeshiftFortificationNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Engineering,
				DefaultSkills.TwoHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001D834 File Offset: 0x0001BA34
		private bool MakeshiftFortificationNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001D838 File Offset: 0x0001BA38
		private void MakeshiftFortificationNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string text = "act_childhood_ready_handshield";
			string text2 = "act_khuzait_mp_rabble_idle";
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text);
				}
				if (narrativeMenuCharacter.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter.SetAnimationId(text2);
				}
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
		private void FinalizeParentsAndLittleSiblings(CharacterCreationManager characterCreationManager)
		{
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_mother");
			CharacterObject object2 = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_father");
			CharacterObject characterObject = StoryModeHeroes.ElderBrother.CharacterObject;
			NarrativeMenuCharacter narrativeMenuCharacter = null;
			NarrativeMenuCharacter narrativeMenuCharacter2 = null;
			foreach (NarrativeMenuCharacter narrativeMenuCharacter3 in characterCreationManager.GetNarrativeMenuWithId("narrative_parent_menu").Characters)
			{
				if (narrativeMenuCharacter3.StringId.Equals("mother_character"))
				{
					narrativeMenuCharacter = narrativeMenuCharacter3;
				}
				if (narrativeMenuCharacter3.StringId.Equals("father_character"))
				{
					narrativeMenuCharacter2 = narrativeMenuCharacter3;
				}
			}
			@object.HeroObject.StaticBodyProperties = narrativeMenuCharacter.BodyProperties.StaticProperties;
			object2.HeroObject.StaticBodyProperties = narrativeMenuCharacter2.BodyProperties.StaticProperties;
			@object.HeroObject.Weight = narrativeMenuCharacter.BodyProperties.Weight;
			@object.HeroObject.Build = narrativeMenuCharacter.BodyProperties.Build;
			object2.HeroObject.Weight = narrativeMenuCharacter2.BodyProperties.Weight;
			object2.HeroObject.Build = narrativeMenuCharacter2.BodyProperties.Build;
			if (narrativeMenuCharacter.Equipment != null)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(@object.HeroObject, narrativeMenuCharacter.Equipment.DefaultEquipment);
			}
			if (narrativeMenuCharacter2.Equipment != null)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(object2.HeroObject, narrativeMenuCharacter2.Equipment.DefaultEquipment);
			}
			if (characterObject.Equipment != null)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(characterObject.HeroObject, characterObject.Equipment);
			}
			@object.HeroObject.Culture = Hero.MainHero.Culture;
			object2.HeroObject.Culture = Hero.MainHero.Culture;
			characterObject.HeroObject.Culture = Hero.MainHero.Culture;
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			TextObject textObject = GameTexts.FindText("str_player_little_brother_name", Hero.MainHero.Culture.StringId);
			StoryModeHeroes.LittleBrother.SetName(textObject, textObject);
			StoryModeHeroes.LittleBrother.SetHasMet();
			TextObject textObject2 = GameTexts.FindText("str_player_little_sister_name", Hero.MainHero.Culture.StringId);
			StoryModeHeroes.LittleSister.SetName(textObject2, textObject2);
			StoryModeHeroes.LittleSister.SetHasMet();
			TextObject textObject3 = GameTexts.FindText("str_player_father_name", Hero.MainHero.Culture.StringId);
			object2.HeroObject.SetName(textObject3, textObject3);
			TextObject textObject4 = GameTexts.FindText("str_player_mother_name", Hero.MainHero.Culture.StringId);
			@object.HeroObject.SetName(textObject4, textObject4);
			TextObject textObject5 = GameTexts.FindText("str_player_brother_name", Hero.MainHero.Culture.StringId);
			characterObject.HeroObject.SetName(textObject5, textObject5);
			@object.HeroObject.Spouse = object2.HeroObject;
			object2.HeroObject.Spouse = @object.HeroObject;
			@object.HeroObject.UpdateHomeSettlement();
			object2.HeroObject.UpdateHomeSettlement();
			characterObject.HeroObject.UpdateHomeSettlement();
			@object.HeroObject.SetHasMet();
			object2.HeroObject.SetHasMet();
			characterObject.HeroObject.SetHasMet();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001DC18 File Offset: 0x0001BE18
		private void FinalizeMainHeroAndElderBrother(CharacterCreationManager characterCreationManager)
		{
			NarrativeMenu narrativeMenuWithId = characterCreationManager.GetNarrativeMenuWithId("narrative_escape_menu");
			NarrativeMenuCharacter narrativeMenuCharacter = null;
			NarrativeMenuCharacter narrativeMenuCharacter2 = null;
			foreach (NarrativeMenuCharacter narrativeMenuCharacter3 in narrativeMenuWithId.Characters)
			{
				if (narrativeMenuCharacter3.StringId.Equals("player_escape_character"))
				{
					narrativeMenuCharacter = narrativeMenuCharacter3;
				}
				if (narrativeMenuCharacter3.StringId.Equals("brother_character"))
				{
					narrativeMenuCharacter2 = narrativeMenuCharacter3;
				}
			}
			CharacterObject.PlayerCharacter.Equipment.FillFrom(narrativeMenuCharacter.Equipment.DefaultEquipment, true);
			CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(narrativeMenuCharacter.Equipment.GetRandomCivilianEquipment(), true);
			Hero elderBrother = StoryModeHeroes.ElderBrother;
			elderBrother.CharacterObject.Equipment.FillFrom(narrativeMenuCharacter2.Equipment.DefaultEquipment, true);
			elderBrother.CharacterObject.FirstCivilianEquipment.FillFrom(narrativeMenuCharacter2.Equipment.GetRandomCivilianEquipment(), true);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001DD0C File Offset: 0x0001BF0C
		protected void CreateSibling(Hero hero, BodyProperties motherBodyProperties, BodyProperties fatherBodyProperties, uint seed)
		{
			string text = Hero.MainHero.Culture.ToString().ToLower() + ",";
			string text2 = Hero.MainHero.Culture.ToString().ToLower() + ",";
			int num = Hero.MainHero.RandomIntWithSeed(seed, 1, 100);
			BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(hero.CharacterObject.Race, hero.IsFemale, motherBodyProperties, fatherBodyProperties, 1, num, text, text2, hero.IsFemale ? Hero.MainHero.Mother.CharacterObject.BodyPropertyRange.TattooTags : Hero.MainHero.Father.CharacterObject.BodyPropertyRange.TattooTags, 0f);
			randomBodyProperties = new BodyProperties(new DynamicBodyProperties(hero.Age, 0.5f, 0.5f), randomBodyProperties.StaticProperties);
			hero.StaticBodyProperties = randomBodyProperties.StaticProperties;
			hero.Weight = randomBodyProperties.Weight;
			hero.Build = randomBodyProperties.Build;
		}

		// Token: 0x040001D5 RID: 469
		private const string BrotherNarrativeCharacterStringId = "brother_character";

		// Token: 0x040001D6 RID: 470
		private const string PlayerEscapeNarrativeCharacterStringId = "player_escape_character";

		// Token: 0x040001D7 RID: 471
		private int _focusToAdd = 1;

		// Token: 0x040001D8 RID: 472
		private int _skillLevelToAdd = 10;

		// Token: 0x040001D9 RID: 473
		private int _attributeLevelToAdd = 1;
	}
}
