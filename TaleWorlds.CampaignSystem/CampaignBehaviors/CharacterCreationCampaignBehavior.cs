using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F9 RID: 1017
	public class CharacterCreationCampaignBehavior : CampaignBehaviorBase, ICharacterCreationContentHandler
	{
		// Token: 0x06003E32 RID: 15922 RVA: 0x00107A68 File Offset: 0x00105C68
		private string GetMotherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
		{
			string text;
			characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out text);
			return "mother_char_creation_" + text + "_" + cultureId;
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00107A98 File Offset: 0x00105C98
		private string GetFatherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
		{
			string text;
			characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out text);
			return "father_char_creation_" + text + "_" + cultureId;
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x00107AC8 File Offset: 0x00105CC8
		private string GetPlayerChildhoodAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
		{
			string text;
			characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out text);
			return string.Concat(new string[]
			{
				"player_char_creation_childhood_age_",
				cultureId,
				"_",
				text,
				"_",
				isFemale ? "f" : "m"
			});
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x00107B24 File Offset: 0x00105D24
		private string GetPlayerEducationAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
		{
			string text;
			characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out text);
			return string.Concat(new string[]
			{
				"player_char_creation_education_age_",
				cultureId,
				"_",
				text,
				"_",
				isFemale ? "f" : "m"
			});
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x00107B80 File Offset: 0x00105D80
		private string GetPlayerEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId, bool isFemale)
		{
			string text;
			characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out text);
			return string.Concat(new string[]
			{
				"player_char_creation_",
				cultureId,
				"_",
				text,
				"_",
				isFemale ? "f" : "m"
			});
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00107BDA File Offset: 0x00105DDA
		public override void RegisterEvents()
		{
			CampaignEvents.OnCharacterCreationInitializedEvent.AddNonSerializedListener(this, new Action<CharacterCreationManager>(this.OnCharacterCreationInitialized));
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00107BF3 File Offset: 0x00105DF3
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00107BF8 File Offset: 0x00105DF8
		private void OnCharacterCreationInitialized(CharacterCreationManager characterCreationManager)
		{
			this._focusToAdd = characterCreationManager.CharacterCreationContent.FocusToAdd;
			this._skillLevelToAdd = characterCreationManager.CharacterCreationContent.SkillLevelToAdd;
			this._attributeLevelToAdd = characterCreationManager.CharacterCreationContent.AttributeLevelToAdd;
			characterCreationManager.CharacterCreationContent.DefaultSelectedTitleType = "guard";
			characterCreationManager.RegisterCharacterCreationContentHandler(this, 800);
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00107C54 File Offset: 0x00105E54
		void ICharacterCreationContentHandler.InitializeContent(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.AddEquipmentToUseGetter(delegate(string occupationId, out string equipmentId)
			{
				return this._occupationToEquipmentMapping.TryGetValue(occupationId, out equipmentId);
			});
			this.InitializeCharacterCreationStages(characterCreationManager);
			this.InitializeCharacterCreationCultures(characterCreationManager);
			this.InitializeData(characterCreationManager);
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00107C82 File Offset: 0x00105E82
		void ICharacterCreationContentHandler.AfterInitializeContent(CharacterCreationManager characterCreationManager)
		{
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00107C84 File Offset: 0x00105E84
		void ICharacterCreationContentHandler.OnStageCompleted(CharacterCreationStageBase stage)
		{
			if (stage is CharacterCreationFaceGeneratorStage)
			{
				this.FaceGenUpdated();
			}
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x00107C94 File Offset: 0x00105E94
		void ICharacterCreationContentHandler.OnCharacterCreationFinalize(CharacterCreationManager characterCreationManager)
		{
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x00107C98 File Offset: 0x00105E98
		public void InitializeCharacterCreationStages(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.AddStage(new CharacterCreationCultureStage());
			characterCreationManager.AddStage(new CharacterCreationFaceGeneratorStage());
			characterCreationManager.AddStage(new CharacterCreationNarrativeStage());
			characterCreationManager.AddStage(new CharacterCreationBannerEditorStage());
			characterCreationManager.AddStage(new CharacterCreationClanNamingStage());
			characterCreationManager.AddStage(new CharacterCreationReviewStage());
			characterCreationManager.AddStage(new CharacterCreationOptionsStage());
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x00107CF4 File Offset: 0x00105EF4
		public void InitializeCharacterCreationCultures(CharacterCreationManager characterCreationManager)
		{
			foreach (CultureObject cultureObject in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>())
			{
				if (cultureObject.StringId == "aserai" || cultureObject.StringId == "battania" || cultureObject.StringId == "empire" || cultureObject.StringId == "khuzait" || cultureObject.StringId == "sturgia" || cultureObject.StringId == "vlandia")
				{
					characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(cultureObject, 1, 10);
				}
			}
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x00107DCC File Offset: 0x00105FCC
		public void InitializeData(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.ChangeReviewPageDescription(new TextObject("{=W6pKpEoT}You prepare to set off for a grand adventure in Calradia! Here is your character. Continue if you are ready, or go back to make changes.", null));
			this.AddParentsMenu(characterCreationManager);
			this.AddChildhoodMenu(characterCreationManager);
			this.AddEducationMenu(characterCreationManager);
			this.AddYouthMenu(characterCreationManager);
			this.AddAdulthoodMenu(characterCreationManager);
			this.AddAgeSelectionMenu(characterCreationManager);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x00107E1C File Offset: 0x0010601C
		public void FaceGenUpdated()
		{
			CharacterCreationManager characterCreationManager = (GameStateManager.Current.ActiveState as CharacterCreationState).CharacterCreationManager;
			BodyProperties bodyProperties2;
			BodyProperties bodyProperties;
			FaceGen.GenerateParentKey(bodyProperties = (bodyProperties2 = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1)), CharacterObject.PlayerCharacter.Race, ref bodyProperties2, ref bodyProperties);
			bodyProperties2 = new BodyProperties(new DynamicBodyProperties(33f, 0.3f, 0.2f), bodyProperties2.StaticProperties);
			bodyProperties = new BodyProperties(new DynamicBodyProperties(33f, 0.5f, 0.5f), bodyProperties.StaticProperties);
			foreach (NarrativeMenu narrativeMenu in characterCreationManager.NarrativeMenus)
			{
				foreach (NarrativeMenuCharacter narrativeMenuCharacter in narrativeMenu.Characters)
				{
					if (narrativeMenuCharacter.StringId.Equals("mother_character"))
					{
						narrativeMenuCharacter.UpdateBodyProperties(bodyProperties2, CharacterObject.PlayerCharacter.Race, true);
					}
					if (narrativeMenuCharacter.StringId.Equals("father_character"))
					{
						narrativeMenuCharacter.UpdateBodyProperties(bodyProperties, CharacterObject.PlayerCharacter.Race, false);
					}
					if (narrativeMenuCharacter.StringId.Equals("player_childhood_character") || narrativeMenuCharacter.StringId.Equals("player_education_character") || narrativeMenuCharacter.StringId.Equals("player_youth_character") || narrativeMenuCharacter.StringId.Equals("player_adulthood_character") || narrativeMenuCharacter.StringId.Equals("player_age_selection_character"))
					{
						narrativeMenuCharacter.UpdateBodyProperties(CharacterObject.PlayerCharacter.GetBodyProperties(null, -1), CharacterObject.PlayerCharacter.Race, false);
					}
				}
			}
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00108014 File Offset: 0x00106214
		private List<NarrativeMenuCharacterArgs> GetParentMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			return new List<NarrativeMenuCharacterArgs>
			{
				new NarrativeMenuCharacterArgs("mother_character", 33, "mother_char_creation_none_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "act_character_creation_female_default_standing", "spawnpoint_player_1", "", "", null, true, true),
				new NarrativeMenuCharacterArgs("father_character", 33, "father_char_creation_none_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "act_character_creation_male_default_standing", "spawnpoint_player_1", "", "", null, true, false)
			};
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x001080AC File Offset: 0x001062AC
		private void AddParentsMenu(CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			BodyProperties bodyProperties2;
			BodyProperties bodyProperties;
			FaceGen.GenerateParentKey(bodyProperties = (bodyProperties2 = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1)), CharacterObject.PlayerCharacter.Race, ref bodyProperties2, ref bodyProperties);
			bodyProperties2 = new BodyProperties(new DynamicBodyProperties(33f, 0.3f, 0.2f), bodyProperties2.StaticProperties);
			bodyProperties = new BodyProperties(new DynamicBodyProperties(33f, 0.5f, 0.5f), bodyProperties.StaticProperties);
			NarrativeMenuCharacter narrativeMenuCharacter = new NarrativeMenuCharacter("mother_character", bodyProperties2, CharacterObject.PlayerCharacter.Race, true);
			list.Add(narrativeMenuCharacter);
			NarrativeMenuCharacter narrativeMenuCharacter2 = new NarrativeMenuCharacter("father_character", bodyProperties, CharacterObject.PlayerCharacter.Race, false);
			list.Add(narrativeMenuCharacter2);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_parent_menu", "start", "narrative_childhood_menu", new TextObject("{=b4lDDcli}Family", null), new TextObject("{=XgFU1pCx}You were born into a family of...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetParentMenuNarrativeMenuCharacterArgs));
			this.AddEmpireParentNarrativeMenuOptions(narrativeMenu);
			this.AddVlandianParentNarrativeMenuOptions(narrativeMenu);
			this.AddSturgianParentNarrativeMenuOptions(narrativeMenu);
			this.AddAseraiParentNarrativeMenuOptions(narrativeMenu);
			this.AddBattaniaNarrativeMenuOptions(narrativeMenu);
			this.AddKhuzaitNarrativeMenuOptions(narrativeMenu);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x001081E0 File Offset: 0x001063E0
		private void AddEmpireParentNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("empire_lanlord_option", new TextObject("{=InN5ZZt3}A landlord's retainers", null), new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireLandlordNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireLandlordNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireLandlordNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("empire_merchant_option", new TextObject("{=651FhzdR}Urban merchants", null), new TextObject("{=FQntPChs}Your family were merchants in one of the main cities of the Empire. They sometimes organized caravans to nearby towns, and discussed issues in the town council.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireUrbanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireUrbanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireUrbanNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("empire_farmer_option", new TextObject("{=sb4gg8Ak}Freeholders", null), new TextObject("{=09z8Q08f}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the imperial rural economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireFarmerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("empire_artisan_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireArtisanNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("empire_hunter_option", new TextObject("{=7eWmU2mF}Foresters", null), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireHunterNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("empire_vagabond_option", new TextObject("{=aEke8dSb}Urban vagabonds", null), new TextObject("{=Jvf6K7TZ}Your family numbered among the many poor migrants living in the slums that grow up outside the walls of imperial cities, making whatever money they could from a variety of odd jobs. Sometimes they did service for one of the Empire's many criminal gangs, and you had an early look at the dark side of life.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireVagabondNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x001083C0 File Offset: 0x001065C0
		private void GetEmpireLandlordNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00108414 File Offset: 0x00106614
		private bool EmpireLandlordNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00108430 File Offset: 0x00106630
		private void EmpireLandlordNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_1";
			string text2 = "act_character_creation_male_default_side_to_side_1";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x001084D0 File Offset: 0x001066D0
		private void GetEmpireUrbanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00108524 File Offset: 0x00106724
		private bool EmpireUrbanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00108540 File Offset: 0x00106740
		private void EmpireUrbanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_mother_front";
			string text2 = "act_character_creation_male_default_mother_front";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x001085E0 File Offset: 0x001067E0
		private void GetEmpireFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x00108634 File Offset: 0x00106834
		private bool EmpireFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x00108650 File Offset: 0x00106850
		private void EmpireFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_father_sitting";
			string text2 = "act_character_creation_male_default_father_sitting";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x001086F0 File Offset: 0x001068F0
		private void GetEmpireArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crafting,
				DefaultSkills.Crossbow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00108744 File Offset: 0x00106944
		private bool EmpireArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x00108760 File Offset: 0x00106960
		private void EmpireArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_2";
			string text2 = "act_character_creation_male_default_side_to_side_2";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x00108800 File Offset: 0x00106A00
		private void GetEmpireHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Scouting,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x00108854 File Offset: 0x00106A54
		private bool EmpireHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x00108870 File Offset: 0x00106A70
		private void EmpireHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_3";
			string text2 = "act_character_creation_male_default_side_to_side_3";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x00108910 File Offset: 0x00106B10
		private void GetEmpireVagabondNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x00108964 File Offset: 0x00106B64
		private bool EmpireVagabondNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x00108980 File Offset: 0x00106B80
		private void EmpireVagabondNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("vagabond_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_hugging";
			string text2 = "act_character_creation_male_default_hugging";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x00108A20 File Offset: 0x00106C20
		public void UpdateParentEquipment(CharacterCreationManager characterCreationManager, MBEquipmentRoster motherEquipment, MBEquipmentRoster fatherEquipment, string motherAnimation, string fatherAnimation)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId.Equals("mother_character"))
				{
					narrativeMenuCharacter.SetEquipment(motherEquipment);
					narrativeMenuCharacter.SetAnimationId(motherAnimation);
				}
				if (narrativeMenuCharacter.StringId.Equals("father_character"))
				{
					narrativeMenuCharacter.SetEquipment(fatherEquipment);
					narrativeMenuCharacter.SetAnimationId(fatherAnimation);
				}
			}
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00108AB4 File Offset: 0x00106CB4
		private void AddVlandianParentNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("vlandia_retainer_option", new TextObject("{=2TptWc4m}A baron's retainers", null), new TextObject("{=0Suu1Q9q}Your father was a bailiff for a local feudal magnate. He looked after his liege's estates, resolved disputes in the village, and helped train the village levy. He rode with the lord's cavalry, fighting as an armored knight.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaRetainerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("vlandia_merchant_option", new TextObject("{=651FhzdR}Urban merchants", null), new TextObject("{=qNZFkxJb}Your family were merchants in one of the main cities of the kingdom. They organized caravans to nearby towns and were active in the local merchant's guild.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMerchantNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("vlandia_farmer_option", new TextObject("{=RDfXuVxT}Yeomen", null), new TextObject("{=BLZ4mdhb}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaFarmerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("vlandia_blacksmith_option", new TextObject("{=p2KIhGbE}Urban blacksmith", null), new TextObject("{=btsMpRcA}Your family owned a smithy in a city. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaBlacksmithNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaBlacksmithNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaBlacksmithNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("vlandia_hunter_option", new TextObject("{=YcnK0Thk}Hunters", null), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaHunterNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("vlandia_mercenary_option", new TextObject("{=ipQP6aVi}Mercenaries", null), new TextObject("{=yYhX6JQC}Your father joined one of Vlandia's many mercenary companies, composed of men who got such a taste for war in their lord's service that they never took well to peace. Their crossbowmen were much valued across Calradia. Your mother was a camp follower, taking you along in the wake of bloody campaigns.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMercenaryNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMercenaryNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMercenaryNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x00108C94 File Offset: 0x00106E94
		private void GetVlandiaRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00108CE8 File Offset: 0x00106EE8
		private bool VlandiaRetainerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00108D04 File Offset: 0x00106F04
		private void VlandiaRetainerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_1";
			string text2 = "act_character_creation_male_default_side_to_side_1";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00108DA4 File Offset: 0x00106FA4
		private void GetVlandiaMerchantNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x00108DF8 File Offset: 0x00106FF8
		private bool VlandiaMerchantNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00108E14 File Offset: 0x00107014
		private void VlandiaMerchantNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_mother_front";
			string text2 = "act_character_creation_male_default_mother_front";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00108EB4 File Offset: 0x001070B4
		private void GetVlandiaFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Polearm,
				DefaultSkills.Crossbow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00108F08 File Offset: 0x00107108
		private bool VlandiaFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x00108F24 File Offset: 0x00107124
		private void VlandiaFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_father_sitting";
			string text2 = "act_character_creation_male_default_father_sitting";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00108FC4 File Offset: 0x001071C4
		private void GetVlandiaBlacksmithNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crafting,
				DefaultSkills.TwoHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00109018 File Offset: 0x00107218
		private bool VlandiaBlacksmithNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x00109034 File Offset: 0x00107234
		private void VlandiaBlacksmithNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_2";
			string text2 = "act_character_creation_male_default_side_to_side_2";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x001090D4 File Offset: 0x001072D4
		private void GetVlandiaHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Scouting,
				DefaultSkills.Crossbow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00109128 File Offset: 0x00107328
		private bool VlandiaHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00109144 File Offset: 0x00107344
		private void VlandiaHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_3";
			string text2 = "act_character_creation_male_default_side_to_side_3";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x001091E4 File Offset: 0x001073E4
		private void GetVlandiaMercenaryNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Crossbow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00109238 File Offset: 0x00107438
		private bool VlandiaMercenaryNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00109254 File Offset: 0x00107454
		private void VlandiaMercenaryNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_hugging";
			string text2 = "act_character_creation_male_default_hugging";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x001092F4 File Offset: 0x001074F4
		private void AddSturgianParentNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("sturgia_companion_option", new TextObject("{=mc78FEbA}A boyar's companions", null), new TextObject("{=hob3WVkU}Your father was a member of a boyar's druzhina, the 'companions' that make up his retinue. He sat at his lord's table in the great hall, oversaw the boyar's estates, and stood by his side in the center of the shield wall in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaCompanionNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaCompanionNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaCompanionNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("sturgia_trader_option", new TextObject("{=HqzVBfpl}Urban traders", null), new TextObject("{=bjVMtW3W}Your family were merchants who lived in one of Sturgia's great river ports, organizing the shipment of the north's bounty of furs, honey and other goods to faraway lands.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaTraderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaTraderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaTraderNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("sturgia_farmer_option", new TextObject("{=zrpqSWSh}Free farmers", null), new TextObject("{=Mcd3ZyKq}Your family had just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaFarmerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("sturgia_artisan_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaArtisanNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("sturgia_hunter_option", new TextObject("{=YcnK0Thk}Hunters", null), new TextObject("{=WyZ2UtFF}Your family had no taste for the authority of the boyars. They made their living deep in the woods, slashing and burning fields which they tended for a year or two before moving on. They hunted and trapped fox, hare, ermine, and other fur-bearing animals.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaHunterNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("sturgia_vagabond_option", new TextObject("{=TPoK3GSj}Vagabonds", null), new TextObject("{=2SDWhGmQ}Your family numbered among the poor migrants living in the slums that grow up outside the walls of the river cities, making whatever money they could from a variety of odd jobs. Sometimes they did services for one of the region's many criminal gangs.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaVagabondNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x001094D4 File Offset: 0x001076D4
		private void GetSturgiaCompanionNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.TwoHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00109528 File Offset: 0x00107728
		private bool SturgiaCompanionNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x00109544 File Offset: 0x00107744
		private void SturgiaCompanionNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_1";
			string text2 = "act_character_creation_male_default_side_to_side_1";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x001095E4 File Offset: 0x001077E4
		private void GetSturgiaTraderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00109638 File Offset: 0x00107838
		private bool SturgiaTraderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x00109654 File Offset: 0x00107854
		private void SturgiaTraderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_mother_front";
			string text2 = "act_character_creation_male_default_mother_front";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x001096F4 File Offset: 0x001078F4
		private void GetSturgiaFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00109748 File Offset: 0x00107948
		private bool SturgiaFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00109764 File Offset: 0x00107964
		private void SturgiaFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_father_sitting";
			string text2 = "act_character_creation_male_default_father_sitting";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00109804 File Offset: 0x00107A04
		private void GetSturgiaArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00109858 File Offset: 0x00107A58
		private bool SturgiaArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00109874 File Offset: 0x00107A74
		private void SturgiaArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_2";
			string text2 = "act_character_creation_male_default_side_to_side_2";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00109914 File Offset: 0x00107B14
		private void GetSturgiaHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Scouting,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00109968 File Offset: 0x00107B68
		private bool SturgiaHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00109984 File Offset: 0x00107B84
		private void SturgiaHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_3";
			string text2 = "act_character_creation_male_default_side_to_side_3";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00109A24 File Offset: 0x00107C24
		private void GetSturgiaVagabondNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00109A78 File Offset: 0x00107C78
		private bool SturgiaVagabondNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00109A94 File Offset: 0x00107C94
		private void SturgiaVagabondNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("vagabond_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_hugging";
			string text2 = "act_character_creation_male_default_hugging";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00109B34 File Offset: 0x00107D34
		private void AddAseraiParentNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("aserai_kinsfolk_option", new TextObject("{=Sw8OxnNr}Kinsfolk of an emir", null), new TextObject("{=MFrIHJZM}Your family was from a smaller offshoot of an emir's tribe. Your father's land gave him enough income to afford a horse but he was not quite wealthy enough to buy the armor needed to join the heavier cavalry. He fought as one of the light horsemen for which the desert is famous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiKinsfolkNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiKinsfolkNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiKinsfolkNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("aserai_slave_option", new TextObject("{=ngFVgwDD}Warrior-slaves", null), new TextObject("{=GsPC2MgU}Your father was part of one of the slave-bodyguards maintained by the Aserai emirs. He fought by his master's side with tribe's armored cavalry, and was freed - perhaps for an act of valor, or perhaps he paid for his freedom with his share of the spoils of battle. He then married your mother.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiSlaveNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiSlaveNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiSlaveNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("aserai_physician_option", new TextObject("{=bgy8LVvY}Physician", null), new TextObject("{=BhQlmQoj}Your family were respected physicians in an oasis town. They set bones and cured the sick, and their skills were in much demand. They were respected in the higher echelons of society too.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiPhysicianNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiPhysicianNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiPhysicianNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("aserai_farmer_option", new TextObject("{=g31pXuqi}Oasis farmers", null), new TextObject("{=5P0KqBAw}Your family tilled the soil in one of the oases of the Nahasa and tended the palm orchards that produced the desert's famous dates. Your father was a member of the main foot levy of his tribe, fighting with his kinsmen under the emir's banner.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiFarmerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("aserai_herder_option", new TextObject("{=EEedqolz}Bedouin", null), new TextObject("{=PKhcPbBX}Your family were part of a nomadic clan, crisscrossing the wastes between wadi beds and wells to feed their herds of goats and camels on the scraggly scrubs of the Nahasa.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiHerderNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("aserai_artisan_option", new TextObject("{=tRIrbTvv}Urban back-alley thugs", null), new TextObject("{=6bUSbsKC}Your father worked for a fitiwi, one of the strongmen who keep order in the poorer quarters of the oasis towns. He resolved disputes over land, dice and insults, imposing his authority with the fitiwi's traditional staff.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiArtisanNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00109D14 File Offset: 0x00107F14
		private void GetAseraiKinsfolkNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00109D68 File Offset: 0x00107F68
		private bool AseraiKinsfolkNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x00109D84 File Offset: 0x00107F84
		private void AseraiKinsfolkNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_1";
			string text2 = "act_character_creation_male_default_side_to_side_1";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x00109E24 File Offset: 0x00108024
		private void GetAseraiSlaveNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x00109E78 File Offset: 0x00108078
		private bool AseraiSlaveNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00109E94 File Offset: 0x00108094
		private void AseraiSlaveNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("mercenary_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_mother_front";
			string text2 = "act_character_creation_male_default_mother_front";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00109F34 File Offset: 0x00108134
		private void GetAseraiPhysicianNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00109F88 File Offset: 0x00108188
		private bool AseraiPhysicianNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x00109FA4 File Offset: 0x001081A4
		private void AseraiPhysicianNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("physician_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_father_sitting";
			string text2 = "act_character_creation_male_default_father_sitting";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x0010A044 File Offset: 0x00108244
		private void GetAseraiFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.OneHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x0010A098 File Offset: 0x00108298
		private bool AseraiFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0010A0B4 File Offset: 0x001082B4
		private void AseraiFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_2";
			string text2 = "act_character_creation_male_default_side_to_side_2";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x0010A154 File Offset: 0x00108354
		private void GetAseraiHerderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Scouting,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x0010A1A8 File Offset: 0x001083A8
		private bool AseraiHerderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x0010A1C4 File Offset: 0x001083C4
		private void AseraiHerderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("herder");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_3";
			string text2 = "act_character_creation_male_default_side_to_side_3";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x0010A264 File Offset: 0x00108464
		private void GetAseraiArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x0010A2B8 File Offset: 0x001084B8
		private bool AseraiArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x0010A2D4 File Offset: 0x001084D4
		private void AseraiArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_hugging";
			string text2 = "act_character_creation_male_default_hugging";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x0010A374 File Offset: 0x00108574
		private void AddBattaniaNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("battania_retainer_option", new TextObject("{=GeNKQlHR}Members of the chieftain's hearthguard", null), new TextObject("{=LpH8SYFL}Your family were the trusted kinfolk of a Battanian chieftain, and sat at his table in his great hall. Your father assisted his chief in running the affairs of the clan and trained with the traditional weapons of the Battanian elite, the two-handed sword or falx and the bow.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaRetainerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("battania_healer_option", new TextObject("{=AeBzTj6w}Healers", null), new TextObject("{=j6py5Rv5}Your parents were healers who gathered herbs and treated the sick. As a living reservoir of Battanian tradition, they were also asked to adjudicate many disputes between the clans.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHealerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("battania_farmer_option", new TextObject("{=tGEStbxb}Tribespeople", null), new TextObject("{=WchH8bS2}Your family were middle-ranking members of a Battanian clan, who tilled their own land. Your father fought with the kern, the main body of his people's warriors, joining in the screaming charges for which the Battanians were famous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaFarmerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("battania_artisan_option", new TextObject("{=BCU6RezA}Smiths", null), new TextObject("{=kg9YtrOg}Your family were smiths, a revered profession among the Battanians. They crafted everything from fine filigree jewelry in geometric designs to the well-balanced longswords favored by the Battanian aristocracy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaArtisanNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("battania_hunter_option", new TextObject("{=7eWmU2mF}Foresters", null), new TextObject("{=7jBroUUQ}Your family had little land of their own, so they earned their living from the woods, hunting and trapping. They taught you from an early age that skills like finding game trails and killing an animal with one shot could make the difference between eating and starvation.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHunterNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("battania_bard_option", new TextObject("{=SpJqhEEh}Bards", null), new TextObject("{=aVzcyhhy}Your father was a bard, drifting from chieftain's hall to chieftain's hall making his living singing the praises of one Battanian aristocrat and mocking his enemies, then going to his enemy's hall and doing the reverse. You learned from him that a clever tongue could spare you  from a life toiling in the fields, if you kept your wits about you.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaBardNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaBardNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaBardNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0010A554 File Offset: 0x00108754
		private void GetBattaniaRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.TwoHanded,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x0010A5A8 File Offset: 0x001087A8
		private bool BattaniaRetainerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0010A5C4 File Offset: 0x001087C4
		private void BattaniaRetainerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_1";
			string text2 = "act_character_creation_male_default_side_to_side_1";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x0010A664 File Offset: 0x00108864
		private void GetBattaniaHealerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x0010A6B8 File Offset: 0x001088B8
		private bool BattaniaHealerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x0010A6D4 File Offset: 0x001088D4
		private void BattaniaHealerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("healer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_mother_front";
			string text2 = "act_character_creation_male_default_mother_front";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0010A774 File Offset: 0x00108974
		private void GetBattaniaFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0010A7C8 File Offset: 0x001089C8
		private bool BattaniaFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0010A7E4 File Offset: 0x001089E4
		private void BattaniaFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_father_sitting";
			string text2 = "act_character_creation_male_default_father_sitting";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x0010A884 File Offset: 0x00108A84
		private void GetBattaniaArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crafting,
				DefaultSkills.TwoHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0010A8D8 File Offset: 0x00108AD8
		private bool BattaniaArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0010A8F4 File Offset: 0x00108AF4
		private void BattaniaArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_2";
			string text2 = "act_character_creation_male_default_side_to_side_2";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0010A994 File Offset: 0x00108B94
		private void GetBattaniaHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Scouting,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0010A9E8 File Offset: 0x00108BE8
		private bool BattaniaHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0010AA04 File Offset: 0x00108C04
		private void BattaniaHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_3";
			string text2 = "act_character_creation_male_default_side_to_side_3";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0010AAA4 File Offset: 0x00108CA4
		private void GetBattaniaBardNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0010AAF8 File Offset: 0x00108CF8
		private bool BattaniaBardNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0010AB14 File Offset: 0x00108D14
		private void BattaniaBardNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("bard_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_hugging";
			string text2 = "act_character_creation_male_default_hugging";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0010ABB4 File Offset: 0x00108DB4
		private void AddKhuzaitNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("khuzait_retainer_option", new TextObject("{=FVaRDe2a}A noyan's kinsfolk", null), new TextObject("{=jAs3kDXh}Your family were the trusted kinsfolk of a Khuzait noyan, and shared his meals in the chieftain's yurt. Your father assisted his chief in running the affairs of the clan and fought in the core of armored lancers in the center of the Khuzait battle line.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitRetainerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("khuzait_merhant_option", new TextObject("{=TkgLEDRM}Merchants", null), new TextObject("{=qPg3IDiq}Your family came from one of the merchant clans that dominated the cities in eastern Calradia before the Khuzait conquest. They adjusted quickly to their new masters, keeping the caravan routes running and ensuring that the tariff revenues that once went into imperial coffers now flowed to the khanate.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitMerchantNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("khuzait_mercenary_option", new TextObject("{=tGEStbxb}Tribespeople", null), new TextObject("{=URgZ4ai4}Your family were middle-ranking members of one of the Khuzait clans. He had some herds of his own, but was not rich. When the Khuzait horde was summoned to battle, he fought with the horse archers, shooting and wheeling and wearing down the enemy before the lancers delivered the final punch.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHerderNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("khuzait_farmer_option", new TextObject("{=gQ2tAvCz}Farmers", null), new TextObject("{=5QSGoRFj}Your family tilled one of the small patches of arable land in the steppes for generations. When the Khuzaits came, they ceased paying taxes to the emperor and providing conscripts for his army, and served the khan instead.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitFarmerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("khuzait_healer_option", new TextObject("{=vfhVveLW}Shamans", null), new TextObject("{=WOKNhaG2}Your family were guardians of the sacred traditions of the Khuzaits, channelling the spirits of the wilderness and of the ancestors. They tended the sick and dispensed wisdom, resolving disputes and providing practical advice.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHealerNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("khuzait_herder_option", new TextObject("{=Xqba1Obq}Nomads", null), new TextObject("{=9aoQYpZs}Your family's clan never pledged its loyalty to the khan and never settled down, preferring to live out in the deep steppe away from his authority. They remain some of the finest trackers and scouts in the grasslands, as the ability to spot an enemy coming and move quickly is often all that protects their herds from their neighbors' predations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitNomadHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitNomadHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitNomadHerderNarrativeOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x0010AD94 File Offset: 0x00108F94
		private void GetKhuzaitRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x0010ADE8 File Offset: 0x00108FE8
		private bool KhuzaitRetainerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0010AE04 File Offset: 0x00109004
		private void KhuzaitRetainerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_1";
			string text2 = "act_character_creation_male_default_side_to_side_1";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0010AEA4 File Offset: 0x001090A4
		private void GetKhuzaitMerchantNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x0010AEF8 File Offset: 0x001090F8
		private bool KhuzaitMerchantNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x0010AF14 File Offset: 0x00109114
		private void KhuzaitMerchantNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_mother_front";
			string text2 = "act_character_creation_male_default_mother_front";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x0010AFB4 File Offset: 0x001091B4
		private void GetKhuzaitHerderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Bow,
				DefaultSkills.Riding
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x0010B008 File Offset: 0x00109208
		private bool KhuzaitHerderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x0010B024 File Offset: 0x00109224
		private void KhuzaitHerderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("herder");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_father_sitting";
			string text2 = "act_character_creation_male_default_father_sitting";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x0010B0C4 File Offset: 0x001092C4
		private void GetKhuzaitFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Polearm,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x0010B118 File Offset: 0x00109318
		private bool KhuzaitFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x0010B134 File Offset: 0x00109334
		private void KhuzaitFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_2";
			string text2 = "act_character_creation_male_default_side_to_side_2";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x0010B1D4 File Offset: 0x001093D4
		private void GetKhuzaitHealerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x0010B228 File Offset: 0x00109428
		private bool KhuzaitHealerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0010B244 File Offset: 0x00109444
		private void KhuzaitHealerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("healer_urban");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_side_to_side_3";
			string text2 = "act_character_creation_male_default_side_to_side_3";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0010B2E4 File Offset: 0x001094E4
		private void GetKhuzaitNomadHerderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Scouting,
				DefaultSkills.Riding
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0010B338 File Offset: 0x00109538
		private bool KhuzaitNomadHerderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0010B354 File Offset: 0x00109554
		private void KhuzaitNomadHerderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SetParentOccupation("herder");
			string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
			string text = "act_character_creation_female_default_hugging";
			string text2 = "act_character_creation_male_default_hugging";
			this.UpdateParentEquipment(characterCreationManager, @object, object2, text, text2);
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0010B3F4 File Offset: 0x001095F4
		private List<NarrativeMenuCharacterArgs> GetChildhoodMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
			string playerChildhoodAgeEquipmentId = this.GetPlayerChildhoodAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			list.Add(new NarrativeMenuCharacterArgs("player_childhood_character", 7, playerChildhoodAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
			return list;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x0010B468 File Offset: 0x00109668
		private void AddChildhoodMenu(CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 7f);
			NarrativeMenuCharacter narrativeMenuCharacter = new NarrativeMenuCharacter("player_childhood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
			list.Add(narrativeMenuCharacter);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_childhood_menu", "narrative_parent_menu", "narrative_education_menu", new TextObject("{=8Yiwt1z6}Early Childhood", null), new TextObject("{=character_creation_content_16}As a child you were noted for...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetChildhoodMenuNarrativeMenuCharacterArgs));
			this.AddChildhoodNarrativeMenuOptions(narrativeMenu);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0010B50C File Offset: 0x0010970C
		private void AddChildhoodNarrativeMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("childhood_leadership_option", new TextObject("{=kmM68Qx4}your leadership skills.", null), new TextObject("{=FfNwXtii}If the wolf pup gang of your early childhood had an alpha, it was definitely you. All the other kids followed your lead as you decided what to play and where to play, and led them in games and mischief.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodLeadershipOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodLeadershipOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodLeadershipOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("childhood_brawn_option", new TextObject("{=5HXS8HEY}your brawn.", null), new TextObject("{=YKzuGc54}You were big, and other children looked to have you around in any scrap with children from a neighboring village. You pushed a plough and threw an axe like an adult.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodBrawnOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodBrawnOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodBrawnOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("childhood_detail_option", new TextObject("{=QrYjPUEf}your attention to detail.", null), new TextObject("{=JUSHAPnu}You were quick on your feet and attentive to what was going on around you. Usually you could run away from trouble, though you could give a good account of yourself in a fight with other children if cornered.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodDetailOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodDetailOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodDetailOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("childhood_smart_option", new TextObject("{=Y3UcaX74}your aptitude for numbers.", null), new TextObject("{=DFidSjIf}Most children around you had only the most rudimentary education, but you lingered after class to study letters and mathematics. You were fascinated by the marketplace - weights and measures, tallies and accounts, the chatter about profits and losses.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodSmartOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodSmartOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodSmartOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("childhood_leader_option", new TextObject("{=GEYzLuwb}your way with people.", null), new TextObject("{=w2TEQq26}You were always attentive to other people, good at guessing their motivations. You studied how individuals were swayed, and tried out what you learned from adults on your friends.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodLeaderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodLeaderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodLeaderOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("childhood_horse_option", new TextObject("{=MEgLE2kj}your skill with horses.", null), new TextObject("{=ngazFofr}You were always drawn to animals, and spent as much time as possible hanging out in the village stables. You could calm horses, and were sometimes called upon to break in new colts. You learned the basics of veterinary arts, much of which is applicable to humans as well.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodHorseOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodHorseOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodHorseOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0010B6EC File Offset: 0x001098EC
		private void GetChildhoodLeadershipOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Leadership,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x0010B740 File Offset: 0x00109940
		private bool ChildhoodLeadershipOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x0010B744 File Offset: 0x00109944
		private void ChildhoodLeadershipOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_childhood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_leader");
				}
			}
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0010B7B4 File Offset: 0x001099B4
		private void GetChildhoodBrawnOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.TwoHanded,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0010B808 File Offset: 0x00109A08
		private bool ChildhoodBrawnOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0010B80C File Offset: 0x00109A0C
		private void ChildhoodBrawnOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_childhood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
				}
			}
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x0010B87C File Offset: 0x00109A7C
		private void GetChildhoodDetailOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0010B8D0 File Offset: 0x00109AD0
		private bool ChildhoodDetailOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0010B8D4 File Offset: 0x00109AD4
		private void ChildhoodDetailOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_childhood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_memory");
				}
			}
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0010B944 File Offset: 0x00109B44
		private void GetChildhoodSmartOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Engineering,
				DefaultSkills.Trade
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0010B998 File Offset: 0x00109B98
		private bool ChildhoodSmartOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0010B99C File Offset: 0x00109B9C
		private void ChildhoodSmartOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_childhood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_numbers");
				}
			}
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0010BA0C File Offset: 0x00109C0C
		private void GetChildhoodLeaderOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Charm,
				DefaultSkills.Leadership
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x0010BA60 File Offset: 0x00109C60
		private bool ChildhoodLeaderOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0010BA64 File Offset: 0x00109C64
		private void ChildhoodLeaderOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_childhood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
				}
			}
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x0010BAD4 File Offset: 0x00109CD4
		private void GetChildhoodHorseOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Medicine
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x0010BB28 File Offset: 0x00109D28
		private bool ChildhoodHorseOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0010BB2C File Offset: 0x00109D2C
		private void ChildhoodHorseOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_childhood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_animals");
				}
			}
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x0010BB9C File Offset: 0x00109D9C
		private List<NarrativeMenuCharacterArgs> GetEducationMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
			string playerEducationAgeEquipmentId = this.GetPlayerEducationAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			list.Add(new NarrativeMenuCharacterArgs("player_education_character", 12, playerEducationAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
			return list;
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0010BC10 File Offset: 0x00109E10
		public void AddEducationMenu(CharacterCreationManager characterCreationManager)
		{
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 12f);
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			NarrativeMenuCharacter narrativeMenuCharacter = new NarrativeMenuCharacter("player_education_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
			list.Add(narrativeMenuCharacter);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_education_menu", "narrative_childhood_menu", "narrative_youth_menu", new TextObject("{=rcoueCmk}Adolescence", null), new TextObject("{=WYvnWcXQ}Like all village children you helped out in the fields. You also...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetEducationMenuNarrativeMenuCharacterArgs));
			this.AddEducationMenuOptions(narrativeMenu);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0010BCB4 File Offset: 0x00109EB4
		private void AddEducationMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("education_herder_option", new TextObject("{=RKVNvimC}herded the sheep.", null), new TextObject("{=KfaqPpbK}You went with other fleet-footed youths to take the villages' sheep, goats or cattle to graze in pastures near the village. You were in charge of chasing down stray beasts, and always kept a big stone on hand to be hurled at lurking predators if necessary.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationHerderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationHerderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationHerderOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("education_smith_option", new TextObject("{=bTKiN0hr}worked in the village smithy.", null), new TextObject("{=y6j1bJTH}You were apprenticed to the local smith. You learned how to heat and forge metal, hammering for hours at a time until your muscles ached.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationSmithOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationSmithOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationSmithOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("education_engineer_option", new TextObject("{=tI8ZLtoA}repaired projects.", null), new TextObject("{=6LFj919J}You helped dig wells, rethatch houses, and fix broken plows. You learned about the basics of construction, as well as what it takes to keep a farming community prosperous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationEngineerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationEngineerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationEngineerOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("education_doctor_option", new TextObject("{=TRwgSLD2}gathered herbs in the wild.", null), new TextObject("{=9ks4u5cH}You were sent by the village healer up into the hills to look for useful medicinal plants. You learned which herbs healed wounds or brought down a fever, and how to find them.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationDoctorOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationDoctorOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationDoctorOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("education_hunter_option", new TextObject("{=T7m7ReTq}hunted small game.", null), new TextObject("{=RuvSk3QT}You accompanied a local hunter as he went into the wilderness, helping him set up traps and catch small animals.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationHunterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationHunterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationHunterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("education_merchant_option", new TextObject("{=qAbMagWq}sold product at the market.", null), new TextObject("{=DIgsfYfz}You took your family's goods to the nearest town to sell your produce and buy supplies. It was hard work, but you enjoyed the hubbub of the marketplace.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationMerchantOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
			NarrativeMenuOption narrativeMenuOption7 = new NarrativeMenuOption("education_watcher_option", new TextObject("{=go7Yu7KS}watched the militia training.", null), new TextObject("{=qnqdEJOv}You watched the town's watch practice shooting and perfect their plans to defend the walls in case of a siege.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationWatcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationWatcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationWatcherOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption7);
			NarrativeMenuOption narrativeMenuOption8 = new NarrativeMenuOption("education_ganger_option", new TextObject("{=gAjvAGTa}hung out with the gangs in the alleys.", null), new TextObject("{=1SUTcF0J}The gang leaders who kept watch over the slums of Calradian cities were always in need of poor youth to run messages and back them up in turf wars, while thrill-seeking merchants' sons and daughters sometimes slummed it in their company as well.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationGangerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationGangerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationGangerOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption8);
			NarrativeMenuOption narrativeMenuOption9 = new NarrativeMenuOption("education_docker_option", new TextObject("{=QVVCgajg}helped at building sites.", null), new TextObject("{=bhdkegZ4}All towns had their share of projects that were constantly in need of both skilled and unskilled labor. You learned how hoists and scaffolds were constructed, how planks and stones were hewn and fitted, and other skills.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationDockerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationDockerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationDockerOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption9);
			NarrativeMenuOption narrativeMenuOption10 = new NarrativeMenuOption("education_marketer_option", new TextObject("{=JTsv6PFe}worked in the markets and caravanserais.", null), new TextObject("{=rmMcwSn8}You helped your family handle their business affairs, going down to the marketplace to make purchases and oversee the arrival of caravans.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationMarketerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationMarketerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationMarketerOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption10);
			NarrativeMenuOption narrativeMenuOption11 = new NarrativeMenuOption("education_tutor_option", new TextObject("{=EMVojYzW}studied with your private tutor.", null), new TextObject("{=hXl25avg}Your family arranged for a private tutor and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationTutorOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationTutorOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationTutorOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption11);
			NarrativeMenuOption narrativeMenuOption12 = new NarrativeMenuOption("education_horser_option", new TextObject("{=hin3iA2D}cared for the horses.", null), new TextObject("{=Ghz90npw}Your family owned a few horses at the town stables and you took charge of their care. Many evenings you would take them out beyond the walls and gallup through the fields, racing other youth.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationPoorHorserOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationPoorHorserOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationPoorHorserOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption12);
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0010C070 File Offset: 0x0010A270
		private void GetEducationHerderOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0010C0C4 File Offset: 0x0010A2C4
		private bool EducationHerderOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0010C0DC File Offset: 0x0010A2DC
		private void EducationHerderOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_streets");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("carry_bostaff_rogue1");
					break;
				}
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0010C164 File Offset: 0x0010A364
		private void GetEducationSmithOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.TwoHanded,
				DefaultSkills.Crafting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0010C1B8 File Offset: 0x0010A3B8
		private bool EducationSmithOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0010C1D0 File Offset: 0x0010A3D0
		private void EducationSmithOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_militia");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("peasant_hammer_1_t1");
					break;
				}
			}
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0010C258 File Offset: 0x0010A458
		private void GetEducationEngineerOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crafting,
				DefaultSkills.Engineering
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x0010C2AC File Offset: 0x0010A4AC
		private bool EducationEngineerOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0010C2C4 File Offset: 0x0010A4C4
		private void EducationEngineerOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_grit");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("carry_hammer");
					break;
				}
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0010C34C File Offset: 0x0010A54C
		private void GetEducationDoctorOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Medicine,
				DefaultSkills.Scouting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0010C3A0 File Offset: 0x0010A5A0
		private bool EducationDoctorOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0010C3B8 File Offset: 0x0010A5B8
		private void EducationDoctorOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_basket_a");
					break;
				}
			}
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0010C440 File Offset: 0x0010A640
		private void GetEducationHunterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Bow,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0010C494 File Offset: 0x0010A694
		private bool EducationHunterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0010C4AC File Offset: 0x0010A6AC
		private void EducationHunterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("composite_bow");
					break;
				}
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0010C534 File Offset: 0x0010A734
		private void GetEducationMerchantOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0010C588 File Offset: 0x0010A788
		private bool EducationMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0010C5A0 File Offset: 0x0010A7A0
		private void EducationMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers_2");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_fabric_c");
					break;
				}
			}
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0010C628 File Offset: 0x0010A828
		private void GetEducationWatcherOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Polearm,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0010C67C File Offset: 0x0010A87C
		private bool EducationWatcherOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0010C690 File Offset: 0x0010A890
		private void EducationWatcherOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_fox");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("");
					break;
				}
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0010C718 File Offset: 0x0010A918
		private void GetEducationGangerOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.OneHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0010C76C File Offset: 0x0010A96C
		private bool EducationGangerOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0010C780 File Offset: 0x0010A980
		private void EducationGangerOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("");
					break;
				}
			}
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0010C808 File Offset: 0x0010AA08
		private void GetEducationDockerOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Crafting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0010C85C File Offset: 0x0010AA5C
		private bool EducationDockerOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x0010C870 File Offset: 0x0010AA70
		private void EducationDockerOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_basket_a");
					break;
				}
			}
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x0010C8F8 File Offset: 0x0010AAF8
		private void GetEducationMarketerOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x0010C94C File Offset: 0x0010AB4C
		private bool EducationMarketerOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0010C960 File Offset: 0x0010AB60
		private void EducationMarketerOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("");
					break;
				}
			}
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0010C9E8 File Offset: 0x0010ABE8
		private void GetEducationTutorOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Engineering,
				DefaultSkills.Leadership
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0010CA3C File Offset: 0x0010AC3C
		private bool EducationTutorOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x0010CA50 File Offset: 0x0010AC50
		private void EducationTutorOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_book");
					narrativeMenuCharacter.SetLeftHandItem("character_creation_notebook");
					narrativeMenuCharacter.SetRightHandItem("");
					break;
				}
			}
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x0010CAD8 File Offset: 0x0010ACD8
		private void GetEducationPoorHorserOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Steward
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0010CB2C File Offset: 0x0010AD2C
		private bool EducationPoorHorserOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0010CB40 File Offset: 0x0010AD40
		private void EducationPoorHorserOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_education_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers_2");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_fabric_c");
					break;
				}
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0010CBC8 File Offset: 0x0010ADC8
		private List<NarrativeMenuCharacterArgs> GetYouthMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			if (string.IsNullOrEmpty(characterCreationManager.CharacterCreationContent.SelectedTitleType))
			{
				characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			}
			List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			list.Add(new NarrativeMenuCharacterArgs("player_youth_character", 17, playerEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
			ItemObject item = @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
			list.Add(new NarrativeMenuCharacterArgs("narrative_character_horse", -1, "", "act_inventory_idle_start", "spawnpoint_mount_1", @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item.StringId, @object.DefaultEquipment[EquipmentIndex.HorseHarness].Item.StringId, MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed()), false, false));
			return list;
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0010CCEC File Offset: 0x0010AEEC
		private void AddYouthMenu(CharacterCreationManager characterCreationManager)
		{
			TextObject textObject = (CharacterObject.PlayerCharacter.IsFemale ? new TextObject("{=5kbeAC7k}In wartorn Calradia, especially in frontier or tribal areas, some women as well as men learn to fight from an early age. You...", null) : new TextObject("{=F7OO5SAa}As a youngster growing up in Calradia, war was never too far away. You...", null));
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 17f);
			NarrativeMenuCharacter narrativeMenuCharacter = new NarrativeMenuCharacter("player_youth_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
			NarrativeMenuCharacter narrativeMenuCharacter2 = new NarrativeMenuCharacter("narrative_character_horse");
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			list.Add(narrativeMenuCharacter);
			list.Add(narrativeMenuCharacter2);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_youth_menu", "narrative_education_menu", "narrative_adulthood_menu", new TextObject("{=ok8lSW6M}Youth", null), textObject, list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetYouthMenuNarrativeMenuCharacterArgs));
			this.AddYouthMenuOptions(narrativeMenu);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0010CDC4 File Offset: 0x0010AFC4
		private void AddYouthMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("youth_staff_first_option", new TextObject("{=CITG915d}joined a commander's staff.", null), new TextObject("{=wNHqFlDL}You were chosen by your superior officer to serve an imperial strategos as a courier. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthStaffOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthStaffOneOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthStaffOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("youth_staff_second_option", new TextObject("{=CITG915d}joined a commander's staff.", null), new TextObject("{=ANbNblaH}You were picked as the courier of the commander of the local forces. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthStaffOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthStaffTwoOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthStaffOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("youth_groom_option", new TextObject("{=bhE2i6OU}served as a baron's groom.", null), new TextObject("{=i3k7YtA8}You were chosen by a knight to accompany a minor baron of the Vlandian kingdom. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGroomOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGroomOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGroomOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("youth_servant_first_option", new TextObject("{=F2bgujPo}were a chieftain's servant.", null), new TextObject("{=AXWO4C69}Your were choosen among others to accompany a chieftain of your people. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthServantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthServantOneOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthServantOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("youth_servant_second_option", new TextObject("{=F2bgujPo}were a chieftain's servant.", null), new TextObject("{=neMCgMZM}Local wise man picked you to become the messenger of a chieftain of your people. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthServantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthServantTwoOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthServantOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("youth_cavalry_option", new TextObject("{=h2KnarLL}trained with the cavalry.", null), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own, but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthCavalryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthCavalryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthCavalryOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
			NarrativeMenuOption narrativeMenuOption7 = new NarrativeMenuOption("youth_hearth_option", new TextObject("{=zsC2t5Hb}trained with the hearth guard.", null), new TextObject("{=RmbWW6Bm}You were a big and imposing enough youth that the chief's guard allowed you to train alongside them, in preparation to join them some day.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthHearthOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthHearthOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthHearthOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption7);
			NarrativeMenuOption narrativeMenuOption8 = new NarrativeMenuOption("youth_guard_high_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardHighRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardHighRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardHighRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption8);
			NarrativeMenuOption narrativeMenuOption9 = new NarrativeMenuOption("youth_guard_low_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=oR58iNDz}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardLowRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardLowRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardLowRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption9);
			NarrativeMenuOption narrativeMenuOption10 = new NarrativeMenuOption("youth_guard_garrisons_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=e6lINjFg}The garrisons spent most of their time guarding the town walls, and their training focused largely on missile weapons.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardGarrisonRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardGarrisonRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardGarrisonRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption10);
			NarrativeMenuOption narrativeMenuOption11 = new NarrativeMenuOption("youth_guard_empire_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=oR58iNDz}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardEmpireRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardEmpireRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardEmpireRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption11);
			NarrativeMenuOption narrativeMenuOption12 = new NarrativeMenuOption("youth_rider_high_register_option", new TextObject("{=VlXOgIX6}rode with the scouts.", null), new TextObject("{=888lmJqs}All of Calradia's kingdoms recognize the value of good light cavalry and horse archers, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a good bow so that you could fulfill their levy obligations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthRiderHighRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthRiderHighRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthRiderHighRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption12);
			NarrativeMenuOption narrativeMenuOption13 = new NarrativeMenuOption("youth_rider_low_register_option", new TextObject("{=VlXOgIX6}rode with the scouts.", null), new TextObject("{=sYuN6hPD}All of Calradia's kingdoms recognize the value of good light cavalry, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a sheaf of javelins so that you could fulfill their levy obligations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthRiderLowRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthRiderLowRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthRiderLowRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption13);
			NarrativeMenuOption narrativeMenuOption14 = new NarrativeMenuOption("youth_infantry_option", new TextObject("{=a8arFSra}trained with the infantry.", null), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Calradia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthInfantryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthInfantryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthInfantryOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption14);
			NarrativeMenuOption narrativeMenuOption15 = new NarrativeMenuOption("youth_skirmisher_option", new TextObject("{=oMbOIPc9}joined the skirmishers.", null), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthSkirmisherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthSkirmisherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthSkirmisherOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption15);
			NarrativeMenuOption narrativeMenuOption16 = new NarrativeMenuOption("youth_kern_option", new TextObject("{=cDWbwBwI}joined the kern.", null), new TextObject("{=tTb28jyU}Many Battanians fight as kern, versatile troops who could both harass the enemy line with their javelins or join in the final screaming charge once it weakened.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthKernOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthKernOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthKernOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption16);
			NarrativeMenuOption narrativeMenuOption17 = new NarrativeMenuOption("youth_camp_option", new TextObject("{=GFUggps8}marched with the camp followers.", null), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthCampOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthCampOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthCampOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption17);
			NarrativeMenuOption narrativeMenuOption18 = new NarrativeMenuOption("youth_envoys_guard_first_option", new TextObject("{=YmPlLGXb}served in an envoy's entourage", null), new TextObject("{=qPamcCkA}Your family arranged for you to accompany an envoy. You were not given major responsibilities - mostly carrying arms and trying to look imposing. - but it did give you a chance to travel a lot and socialise and see the world.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEnvoysGuardFirstOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EnvoysGuardFirstOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EnvoysGuardFirstOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption18);
			NarrativeMenuOption narrativeMenuOption19 = new NarrativeMenuOption("youth_envoys_guard_second_option", new TextObject("{=YmPlLGXb}served in an envoy's entourage", null), new TextObject("{=VYU1nEHP}Your family arranged for you to accompany an envoy. You were not given major responsibilities but it did give you a chance to travel and socialise and see a bit of the world.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEnvoysGuardSecondOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EnvoysGuardSecondOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EnvoysGuardSecondOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption19);
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x0010D3A8 File Offset: 0x0010B5A8
		private void GetYouthStaffOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Steward,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x0010D3FC File Offset: 0x0010B5FC
		private bool YouthStaffOneOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0010D418 File Offset: 0x0010B618
		private bool YouthStaffTwoOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0010D434 File Offset: 0x0010B634
		private void YouthStaffOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "retainer";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0010D4F8 File Offset: 0x0010B6F8
		private void GetYouthGroomOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Charm,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x0010D54C File Offset: 0x0010B74C
		private bool YouthGroomOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x0010D568 File Offset: 0x0010B768
		private void YouthGroomOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "retainer";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0010D62C File Offset: 0x0010B82C
		private void GetYouthServantOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Steward,
				DefaultSkills.Tactics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x0010D680 File Offset: 0x0010B880
		private bool YouthServantOneOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0010D69C File Offset: 0x0010B89C
		private bool YouthServantTwoOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x0010D6B8 File Offset: 0x0010B8B8
		private void YouthServantOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "retainer";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_ready");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0010D77C File Offset: 0x0010B97C
		private void GetYouthCavalryOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x0010D7D0 File Offset: 0x0010B9D0
		private bool YouthCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x0010D7EC File Offset: 0x0010B9EC
		private void YouthCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "mercenary";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_apprentice");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0010D8B0 File Offset: 0x0010BAB0
		private void GetYouthHearthOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x0010D904 File Offset: 0x0010BB04
		private bool YouthHearthOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x0010D940 File Offset: 0x0010BB40
		private void YouthHearthOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "mercenary";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0010DA04 File Offset: 0x0010BC04
		private void GetYouthGuardHighRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crossbow,
				DefaultSkills.Engineering
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0010DA58 File Offset: 0x0010BC58
		private bool YouthGuardHighRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0010DA74 File Offset: 0x0010BC74
		private void YouthGuardHighRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0010DB38 File Offset: 0x0010BD38
		private void GetYouthGuardLowRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Bow,
				DefaultSkills.Engineering
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0010DB8C File Offset: 0x0010BD8C
		private bool YouthGuardLowRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0010DBA8 File Offset: 0x0010BDA8
		private void YouthGuardLowRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x0010DC6C File Offset: 0x0010BE6C
		private void GetYouthGuardGarrisonRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Bow,
				DefaultSkills.Engineering
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0010DCC0 File Offset: 0x0010BEC0
		private bool YouthGuardGarrisonRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0010DD24 File Offset: 0x0010BF24
		private void YouthGuardGarrisonRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x0010DDE8 File Offset: 0x0010BFE8
		private void GetYouthGuardEmpireRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Crossbow,
				DefaultSkills.Engineering
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0010DE3C File Offset: 0x0010C03C
		private bool YouthGuardEmpireRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x0010DE58 File Offset: 0x0010C058
		private void YouthGuardEmpireRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x0010DF1C File Offset: 0x0010C11C
		private void GetYouthRiderHighRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x0010DF70 File Offset: 0x0010C170
		private bool YouthRiderHighRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0010DFAC File Offset: 0x0010C1AC
		private void YouthRiderHighRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "hunter";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_sturgia_mp_warrior_axe");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x0010E070 File Offset: 0x0010C270
		private void GetYouthRiderLowRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Riding,
				DefaultSkills.Bow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x0010E0C4 File Offset: 0x0010C2C4
		private bool YouthRiderLowRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0010E100 File Offset: 0x0010C300
		private void YouthRiderLowRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "hunter";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_sturgia_mp_huskarl_idle");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0010E1C4 File Offset: 0x0010C3C4
		private void GetYouthInfantryOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Polearm,
				DefaultSkills.OneHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0010E218 File Offset: 0x0010C418
		private bool YouthInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0010E2D0 File Offset: 0x0010C4D0
		private void YouthInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_fierce");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0010E394 File Offset: 0x0010C594
		private void GetYouthSkirmisherOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Throwing,
				DefaultSkills.OneHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0010E3E8 File Offset: 0x0010C5E8
		private bool YouthSkirmisherOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0010E484 File Offset: 0x0010C684
		private void YouthSkirmisherOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_fox");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0010E548 File Offset: 0x0010C748
		private void GetYouthKernOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Throwing,
				DefaultSkills.OneHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0010E59C File Offset: 0x0010C79C
		private bool YouthKernOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0010E5B8 File Offset: 0x0010C7B8
		private void YouthKernOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "kern";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_apprentice");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0010E67C File Offset: 0x0010C87C
		private void GetYouthCampOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Roguery,
				DefaultSkills.Throwing
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0010E6D0 File Offset: 0x0010C8D0
		private bool YouthCampOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0010E70C File Offset: 0x0010C90C
		private void YouthCampOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "bard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_militia");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0010E7D0 File Offset: 0x0010C9D0
		private void GetEnvoysGuardFirstOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Charm,
				DefaultSkills.Scouting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0010E824 File Offset: 0x0010CA24
		private void GetEnvoysGuardSecondOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Charm,
				DefaultSkills.Scouting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x0010E878 File Offset: 0x0010CA78
		private bool EnvoysGuardFirstOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0010E8B2 File Offset: 0x0010CAB2
		private bool EnvoysGuardSecondOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0010E8EC File Offset: 0x0010CAEC
		private void EnvoysGuardFirstOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0010E9B0 File Offset: 0x0010CBB0
		private void EnvoysGuardSecondOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_youth_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
				}
			}
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0010EA74 File Offset: 0x0010CC74
		private List<NarrativeMenuCharacterArgs> GetAdultMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			list.Add(new NarrativeMenuCharacterArgs("player_adulthood_character", 20, playerEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
			ItemObject item = @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
			list.Add(new NarrativeMenuCharacterArgs("narrative_character_horse", -1, "", "act_horse_stand_1", "spawnpoint_mount_1", @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item.StringId, @object.DefaultEquipment[EquipmentIndex.HorseHarness].Item.StringId, MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed()), false, false));
			return list;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0010EB74 File Offset: 0x0010CD74
		private void AddAdulthoodMenu(CharacterCreationManager characterCreationManager)
		{
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 20f);
			NarrativeMenuCharacter narrativeMenuCharacter = new NarrativeMenuCharacter("player_adulthood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
			NarrativeMenuCharacter narrativeMenuCharacter2 = new NarrativeMenuCharacter("narrative_character_horse");
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			list.Add(narrativeMenuCharacter);
			list.Add(narrativeMenuCharacter2);
			MBTextManager.SetTextVariable("EXP_VALUE", this._skillLevelToAdd);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_adulthood_menu", "narrative_youth_menu", "narrative_age_selection_menu", new TextObject("{=MafIe9yI}Young Adulthood", null), new TextObject("{=4WYY0X59}Before you set out for a life of adventure, your biggest achievement was...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetAdultMenuNarrativeMenuCharacterArgs));
			this.AddAdulthoodMenuOptions(narrativeMenu);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x0010EC3C File Offset: 0x0010CE3C
		private void AddAdulthoodMenuOptions(NarrativeMenu narrativeMenu)
		{
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("adulthood_defeated_enemy_option", new TextObject("{=8bwpVpgy}you defeated an enemy in battle.", null), new TextObject("{=1IEroJKs}Not everyone who musters for the levy marches to war, and not everyone who goes on campaign sees action. You did both, and you also took down an enemy warrior in direct one-to-one combat, in the full view of your comrades.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodDefeatedEnemyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodDefeatedEnemyOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodDefeatedEnemyOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("adulthood_manhunt_option", new TextObject("{=mP3uFbcq}you led a successful manhunt.", null), new TextObject("{=4f5xwzX0}When your community needed to organize a posse to pursue horse thieves, you were the obvious choice. You hunted down the raiders, surrounded them and forced their surrender, and took back your stolen property.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodManhuntOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodManhuntOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodManhuntOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("adulthood_caravan_leader_option", new TextObject("{=wfbtS71d}you led a caravan.", null), new TextObject("{=joRHKCkm}Your family needed someone trustworthy to take a caravan to a neighboring town. You organized supplies, ensured a constant watch to keep away bandits, and brought it safely to its destination.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodCaravanLeaderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodCaravanLeaderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodCaravanLeaderOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("adulthood_saved_village_option", new TextObject("{=x1HTX5hq}you saved your village from a flood.", null), new TextObject("{=bWlmGDf3}When a sudden storm caused the local stream to rise suddenly, your neighbors needed quick-thinking leadership. You provided it, directing them to build levees to save their homes.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodSavedVillageOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodSavedVillageOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodSavedVillageOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			NarrativeMenuOption narrativeMenuOption5 = new NarrativeMenuOption("adulthood_saved_city_option", new TextObject("{=s8PNllPN}you saved your city quarter from a fire.", null), new TextObject("{=ZAGR6PYc}When a sudden blaze broke out in a back alley, your neighbors needed quick-thinking leadership and you provided it. You organized a bucket line to the nearest well, putting the fire out before any homes were lost.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodSavedCityOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodSavedCityOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodSavedCityOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption5);
			NarrativeMenuOption narrativeMenuOption6 = new NarrativeMenuOption("adulthood_workshop_option", new TextObject("{=xORjDTal}you invested some money in a workshop.", null), new TextObject("{=PyVqDLBu}Your parents didn't give you much money, but they did leave just enough for you to secure a loan against a larger amount to build a small workshop. You paid back what you borrowed, and sold your enterprise for a profit.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodWorkshopOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodWorkshopOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodWorkshopOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption6);
			NarrativeMenuOption narrativeMenuOption7 = new NarrativeMenuOption("adulthood_investor_option", new TextObject("{=xKXcqRJI}you invested some money in land.", null), new TextObject("{=cbF9jdQo}Your parents didn't give you much money, but they did leave just enough for you to purchase a plot of unused land at the edge of the village. You cleared away rocks and dug an irrigation ditch, raised a few seasons of crops, than sold it for a considerable profit.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodInvestorOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodInvestorOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodInvestorOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption7);
			NarrativeMenuOption narrativeMenuOption8 = new NarrativeMenuOption("adulthood_hunter_option", new TextObject("{=TbNRtUjb}you hunted a dangerous animal.", null), new TextObject("{=I3PcdaaL}Wolves, bears are a constant menace to the flocks of northern Calradia, while hyenas and leopards trouble the south. You went with a group of your fellow villagers and fired the missile that brought down the beast.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodHunterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodHunterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodHunterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption8);
			NarrativeMenuOption narrativeMenuOption9 = new NarrativeMenuOption("adulthood_siege_survivor_option", new TextObject("{=WbHfGCbd}you survived a siege.", null), new TextObject("{=FhZPjhli}Your hometown was briefly placed under siege, and you were called to defend the walls. Everyone did their part to repulse the enemy assault, and everyone is justly proud of what they endured.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodSiegeSurvivorOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodSiegeSurvivorOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodSiegeSurvivorOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption9);
			NarrativeMenuOption narrativeMenuOption10 = new NarrativeMenuOption("adulthood_escapade_high_register_option", new TextObject("{=kNXet6Um}you had a famous escapade in town.", null), new TextObject("{=DjeAJtix}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, on one of your trips into town you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodEscapadeHighRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodEscapadeHighRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodEscapadeHighRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption10);
			NarrativeMenuOption narrativeMenuOption11 = new NarrativeMenuOption("adulthood_escapade_low_register_option", new TextObject("{=qlOuiKXj}you had a famous escapade.", null), new TextObject("{=lD5Ob3R4}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodEscapadeLowRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodEscapadeLowRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodEscapadeLowRegisterOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption11);
			NarrativeMenuOption narrativeMenuOption12 = new NarrativeMenuOption("adulthood_nice_person_option", new TextObject("{=Yqm0Dics}you treated people well.", null), new TextObject("{=dDmcqTzb}Yours wasn't the kind of reputation that local legends are made of, but it was the kind that wins you respect among those around you. You were consistently fair and honest in your business dealings and helpful to those in trouble. In doing so, you got a sense of what made people tick.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAdulthoodNicePersonOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AdulthoodNicePersonOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AdulthoodNicePersonOptionOnSelect), null);
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption12);
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0010EFF8 File Offset: 0x0010D1F8
		private void GetAdulthoodDefeatedEnemyOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.OneHanded,
				DefaultSkills.TwoHanded
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Valor };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(20);
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0010F071 File Offset: 0x0010D271
		private bool AdulthoodDefeatedEnemyOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0010F074 File Offset: 0x0010D274
		private void AdulthoodDefeatedEnemyOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
				}
			}
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x0010F0E4 File Offset: 0x0010D2E4
		private void GetAdulthoodManhuntOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Calculating };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(10);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0010F160 File Offset: 0x0010D360
		private bool AdulthoodManhuntOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation) && (characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait");
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0010F210 File Offset: 0x0010D410
		private void AdulthoodManhuntOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_battania_mp_clan_warrior_shieldperk_idle");
				}
			}
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x0010F280 File Offset: 0x0010D480
		private void GetAdulthoodCaravanLeaderOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Leadership
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Calculating };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(10);
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0010F2FC File Offset: 0x0010D4FC
		private bool AdulthoodCaravanLeaderOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation) && (characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord");
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0010F3CC File Offset: 0x0010D5CC
		private void AdulthoodCaravanLeaderOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_ready_handshield");
				}
			}
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x0010F43C File Offset: 0x0010D63C
		private void GetAdulthoodSavedVillageOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Valor };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(10);
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0010F4B8 File Offset: 0x0010D6B8
		private bool AdulthoodSavedVillageOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation) && (characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord");
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0010F514 File Offset: 0x0010D714
		private void AdulthoodSavedVillageOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_drafted_to_war_pose");
				}
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0010F584 File Offset: 0x0010D784
		private void GetAdulthoodSavedCityOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Calculating };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(10);
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0010F5FD File Offset: 0x0010D7FD
		private bool AdulthoodSavedCityOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation) && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0010F630 File Offset: 0x0010D830
		private void AdulthoodSavedCityOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
				}
			}
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0010F6A0 File Offset: 0x0010D8A0
		private void GetAdulthoodWorkshopOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Crafting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Calculating };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(10);
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0010F719 File Offset: 0x0010D919
		private bool AdulthoodWorkshopOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0010F72C File Offset: 0x0010D92C
		private void AdulthoodWorkshopOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
				}
			}
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0010F79C File Offset: 0x0010D99C
		private void GetAdulthoodInvestorOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Trade,
				DefaultSkills.Crafting
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Calculating };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(10);
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x0010F815 File Offset: 0x0010DA15
		private bool AdulthoodInvestorOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x0010F82C File Offset: 0x0010DA2C
		private void AdulthoodInvestorOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
				}
			}
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x0010F89C File Offset: 0x0010DA9C
		private void GetAdulthoodHunterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Polearm,
				DefaultSkills.Athletics
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Valor };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(5);
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x0010F914 File Offset: 0x0010DB14
		private bool AdulthoodHunterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x0010F92C File Offset: 0x0010DB2C
		private void AdulthoodHunterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
				}
			}
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0010F99C File Offset: 0x0010DB9C
		private void GetAdulthoodSiegeSurvivorOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Bow,
				DefaultSkills.Crossbow
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Control, this._attributeLevelToAdd);
			args.SetRenownToAdd(5);
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x0010F9F7 File Offset: 0x0010DBF7
		private bool AdulthoodSiegeSurvivorOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x0010FA0C File Offset: 0x0010DC0C
		private void AdulthoodSiegeSurvivorOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
				}
			}
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0010FA7C File Offset: 0x0010DC7C
		private void GetAdulthoodEscapadeHighRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Roguery
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Valor };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(5);
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0010FAF4 File Offset: 0x0010DCF4
		private bool AdulthoodEscapadeHighRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return !CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x0010FB0C File Offset: 0x0010DD0C
		private void AdulthoodEscapadeHighRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_clever");
				}
			}
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0010FB7C File Offset: 0x0010DD7C
		private void GetAdulthoodEscapadeLowRegisterOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Athletics,
				DefaultSkills.Roguery
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[] { DefaultTraits.Valor };
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(5);
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x0010FBF4 File Offset: 0x0010DDF4
		private bool AdulthoodEscapadeLowRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return CharacterCreationCampaignBehavior.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x0010FC08 File Offset: 0x0010DE08
		private void AdulthoodEscapadeLowRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_clever");
				}
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0010FC78 File Offset: 0x0010DE78
		private void GetAdulthoodNicePersonOptionArgs(NarrativeMenuOptionArgs args)
		{
			SkillObject[] array = new SkillObject[]
			{
				DefaultSkills.Charm,
				DefaultSkills.Steward
			};
			args.SetAffectedSkills(array);
			args.SetFocusToSkills(this._focusToAdd);
			args.SetLevelToSkills(this._skillLevelToAdd);
			args.SetLevelToAttribute(DefaultCharacterAttributes.Social, this._attributeLevelToAdd);
			TraitObject[] array2 = new TraitObject[]
			{
				DefaultTraits.Mercy,
				DefaultTraits.Generosity,
				DefaultTraits.Honor
			};
			args.SetAffectedTraits(array2);
			args.SetLevelToTraits(1);
			args.SetRenownToAdd(5);
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0010FD00 File Offset: 0x0010DF00
		private bool AdulthoodNicePersonOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x0010FD04 File Offset: 0x0010DF04
		private void AdulthoodNicePersonOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_adulthood_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
				}
			}
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0010FD74 File Offset: 0x0010DF74
		private List<NarrativeMenuCharacterArgs> GetAgeSelectionMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
		{
			List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			list.Add(new NarrativeMenuCharacterArgs("player_age_selection_character", characterCreationManager.CharacterCreationContent.StartingAge, playerEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
			ItemObject item = @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
			list.Add(new NarrativeMenuCharacterArgs("narrative_character_horse", -1, "", "act_horse_stand_1", "spawnpoint_mount_1", @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item.StringId, @object.DefaultEquipment[EquipmentIndex.HorseHarness].Item.StringId, MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed()), false, false));
			return list;
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0010FE7C File Offset: 0x0010E07C
		private void AddAgeSelectionMenu(CharacterCreationManager characterCreationManager)
		{
			MBTextManager.SetTextVariable("EXP_VALUE", this._skillLevelToAdd);
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, (float)characterCreationManager.CharacterCreationContent.StartingAge);
			NarrativeMenuCharacter narrativeMenuCharacter = new NarrativeMenuCharacter("player_age_selection_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale);
			NarrativeMenuCharacter narrativeMenuCharacter2 = new NarrativeMenuCharacter("narrative_character_horse");
			List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
			list.Add(narrativeMenuCharacter);
			list.Add(narrativeMenuCharacter2);
			NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_age_selection_menu", "narrative_adulthood_menu", "", new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetAgeSelectionMenuNarrativeMenuCharacterArgs));
			NarrativeMenuOption narrativeMenuOption = new NarrativeMenuOption("age_selection_young_adult_option", new TextObject("{=!}20", null), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionYoungAdultAgeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionYoungAdultAgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionYoungAdultAgeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionYoungAdultAgeOptionOnConsequence));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption);
			NarrativeMenuOption narrativeMenuOption2 = new NarrativeMenuOption("age_selection_adult_option", new TextObject("{=!}30", null), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionAdultOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionAdultOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionAdultOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionAdultOptionOnConsequence));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption2);
			NarrativeMenuOption narrativeMenuOption3 = new NarrativeMenuOption("age_selection_middle_age_option", new TextObject("{=!}40", null), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionMiddleAgeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionMiddleAgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionMiddleAgeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionMiddleAgeOptionOnConsequence));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption3);
			NarrativeMenuOption narrativeMenuOption4 = new NarrativeMenuOption("age_selection_elder_option", new TextObject("{=!}50", null), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionElderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionElderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionElderOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionElderOptionOnConsequence));
			narrativeMenu.AddNarrativeMenuOption(narrativeMenuOption4);
			characterCreationManager.AddNewMenu(narrativeMenu);
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x001100AF File Offset: 0x0010E2AF
		private void GetAgeSelectionYoungAdultAgeOptionArgs(NarrativeMenuOptionArgs args)
		{
			args.SetUnspentFocusToAdd(2);
			args.SetUnspentAttributeToAdd(1);
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x001100BF File Offset: 0x0010E2BF
		private bool AgeSelectionYoungAdultAgeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x001100C4 File Offset: 0x0010E2C4
		private void AgeSelectionYoungAdultAgeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_age_selection_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_focus");
					narrativeMenuCharacter.ChangeAge(20f);
					MBEquipmentRoster mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
					if (mbequipmentRoster == null)
					{
						Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionYoungAdultAgeOptionOnSelect", 4884);
						mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
					}
					narrativeMenuCharacter.SetEquipment(mbequipmentRoster);
					break;
				}
			}
			characterCreationManager.CharacterCreationContent.StartingAge = 20;
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-20f));
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x001101DC File Offset: 0x0010E3DC
		private void AgeSelectionYoungAdultAgeOptionOnConsequence(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.StartingAge = 20;
			this.ApplyMainHeroEquipment(characterCreationManager);
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x001101F2 File Offset: 0x0010E3F2
		private void GetAgeSelectionAdultOptionArgs(NarrativeMenuOptionArgs args)
		{
			args.SetUnspentFocusToAdd(4);
			args.SetUnspentAttributeToAdd(2);
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00110202 File Offset: 0x0010E402
		private bool AgeSelectionAdultOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x00110208 File Offset: 0x0010E408
		private void AgeSelectionAdultOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_age_selection_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
					narrativeMenuCharacter.ChangeAge(30f);
					MBEquipmentRoster mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
					if (mbequipmentRoster == null)
					{
						Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionAdultOptionOnSelect", 4934);
						mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
					}
					narrativeMenuCharacter.SetEquipment(mbequipmentRoster);
					break;
				}
			}
			characterCreationManager.CharacterCreationContent.StartingAge = 30;
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-30f));
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00110320 File Offset: 0x0010E520
		private void AgeSelectionAdultOptionOnConsequence(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.StartingAge = 30;
			this.ApplyMainHeroEquipment(characterCreationManager);
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x00110336 File Offset: 0x0010E536
		private void GetAgeSelectionMiddleAgeOptionArgs(NarrativeMenuOptionArgs args)
		{
			args.SetUnspentFocusToAdd(6);
			args.SetUnspentAttributeToAdd(3);
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00110346 File Offset: 0x0010E546
		private bool AgeSelectionMiddleAgeOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x0011034C File Offset: 0x0010E54C
		private void AgeSelectionMiddleAgeOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_age_selection_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
					narrativeMenuCharacter.ChangeAge(40f);
					MBEquipmentRoster mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
					if (mbequipmentRoster == null)
					{
						Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionMiddleAgeOptionOnSelect", 4984);
						mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
					}
					narrativeMenuCharacter.SetEquipment(mbequipmentRoster);
					break;
				}
			}
			characterCreationManager.CharacterCreationContent.StartingAge = 40;
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-40f));
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x00110464 File Offset: 0x0010E664
		private void AgeSelectionMiddleAgeOptionOnConsequence(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.StartingAge = 40;
			this.ApplyMainHeroEquipment(characterCreationManager);
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0011047A File Offset: 0x0010E67A
		private void GetAgeSelectionElderOptionArgs(NarrativeMenuOptionArgs args)
		{
			args.SetUnspentFocusToAdd(8);
			args.SetUnspentAttributeToAdd(4);
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0011048A File Offset: 0x0010E68A
		private bool AgeSelectionElderOptionOnCondition(CharacterCreationManager characterCreationManager)
		{
			return true;
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x00110490 File Offset: 0x0010E690
		private void AgeSelectionElderOptionOnSelect(CharacterCreationManager characterCreationManager)
		{
			string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.StringId == "player_age_selection_character")
				{
					narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
					narrativeMenuCharacter.ChangeAge(50f);
					MBEquipmentRoster mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
					if (mbequipmentRoster == null)
					{
						Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionElderOptionOnSelect", 5034);
						mbequipmentRoster = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
					}
					narrativeMenuCharacter.SetEquipment(mbequipmentRoster);
					break;
				}
			}
			characterCreationManager.CharacterCreationContent.StartingAge = 50;
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-50f));
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x001105A8 File Offset: 0x0010E7A8
		private void AgeSelectionElderOptionOnConsequence(CharacterCreationManager characterCreationManager)
		{
			characterCreationManager.CharacterCreationContent.StartingAge = 50;
			this.ApplyMainHeroEquipment(characterCreationManager);
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x001105C0 File Offset: 0x0010E7C0
		private void ApplyMainHeroEquipment(CharacterCreationManager characterCreationManager)
		{
			NarrativeMenu narrativeMenuWithId = characterCreationManager.GetNarrativeMenuWithId("narrative_age_selection_menu");
			NarrativeMenuCharacter narrativeMenuCharacter = null;
			foreach (NarrativeMenuCharacter narrativeMenuCharacter2 in narrativeMenuWithId.Characters)
			{
				if (narrativeMenuCharacter2.StringId.Equals("player_age_selection_character"))
				{
					narrativeMenuCharacter = narrativeMenuCharacter2;
					break;
				}
			}
			CharacterObject.PlayerCharacter.Equipment.FillFrom(narrativeMenuCharacter.Equipment.DefaultEquipment, true);
			CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(narrativeMenuCharacter.Equipment.GetRandomCivilianEquipment(), true);
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00110664 File Offset: 0x0010E864
		public void SetHeroAge(float age)
		{
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-age));
		}

		// Token: 0x0400133E RID: 4926
		private readonly IReadOnlyDictionary<string, string> _occupationToEquipmentMapping = new Dictionary<string, string>
		{
			{ "retainer", "retainer" },
			{ "bard", "bard" },
			{ "hunter", "hunter" },
			{ "farmer", "farmer" },
			{ "herder", "herder" },
			{ "healer", "healer" },
			{ "mercenary", "mercenary" },
			{ "infantry", "infantry" },
			{ "skirmisher", "skirmisher" },
			{ "kern", "kern" },
			{ "guard", "guard" },
			{ "retainer_urban", "retainer" },
			{ "mercenary_urban", "mercenary" },
			{ "merchant_urban", "merchant" },
			{ "vagabond_urban", "vagabond" },
			{ "artisan_urban", "artisan" },
			{ "physician_urban", "physician" },
			{ "healer_urban", "healer" },
			{ "bard_urban", "bard" }
		};

		// Token: 0x0400133F RID: 4927
		private const int ChildhoodAge = 7;

		// Token: 0x04001340 RID: 4928
		private const int EducationAge = 12;

		// Token: 0x04001341 RID: 4929
		private const int YouthAge = 17;

		// Token: 0x04001342 RID: 4930
		private const int AccomplishmentAge = 20;

		// Token: 0x04001343 RID: 4931
		private const int ParentAge = 33;

		// Token: 0x04001344 RID: 4932
		private const int YoungAdultAge = 20;

		// Token: 0x04001345 RID: 4933
		private const int AdultAge = 30;

		// Token: 0x04001346 RID: 4934
		private const int MiddleAge = 40;

		// Token: 0x04001347 RID: 4935
		private const int ElderAge = 50;

		// Token: 0x04001348 RID: 4936
		public const int FocusToAddYouthStart = 2;

		// Token: 0x04001349 RID: 4937
		public const int FocusToAddAdultStart = 4;

		// Token: 0x0400134A RID: 4938
		public const int FocusToAddMiddleAgedStart = 6;

		// Token: 0x0400134B RID: 4939
		public const int FocusToAddElderlyStart = 8;

		// Token: 0x0400134C RID: 4940
		public const int AttributeToAddYouthStart = 1;

		// Token: 0x0400134D RID: 4941
		public const int AttributeToAddAdultStart = 2;

		// Token: 0x0400134E RID: 4942
		public const int AttributeToAddMiddleAgedStart = 3;

		// Token: 0x0400134F RID: 4943
		public const int AttributeToAddElderlyStart = 4;

		// Token: 0x04001350 RID: 4944
		public const string MotherNarrativeCharacterStringId = "mother_character";

		// Token: 0x04001351 RID: 4945
		public const string FatherNarrativeCharacterStringId = "father_character";

		// Token: 0x04001352 RID: 4946
		public const string PlayerChildhoodCharacterStringId = "player_childhood_character";

		// Token: 0x04001353 RID: 4947
		public const string PlayerEducationCharacterStringId = "player_education_character";

		// Token: 0x04001354 RID: 4948
		public const string PlayerYouthCharacterStringId = "player_youth_character";

		// Token: 0x04001355 RID: 4949
		public const string PlayerAdulthoodCharacterStringId = "player_adulthood_character";

		// Token: 0x04001356 RID: 4950
		public const string PlayerAgeSelectionCharacterStringId = "player_age_selection_character";

		// Token: 0x04001357 RID: 4951
		public const string HorseNarrativeCharacterStringId = "narrative_character_horse";

		// Token: 0x04001358 RID: 4952
		private int _focusToAdd = 1;

		// Token: 0x04001359 RID: 4953
		private int _skillLevelToAdd = 10;

		// Token: 0x0400135A RID: 4954
		private int _attributeLevelToAdd = 1;

		// Token: 0x02000813 RID: 2067
		private static class CharacterOccupationTypes
		{
			// Token: 0x060066E9 RID: 26345 RVA: 0x001D1308 File Offset: 0x001CF508
			public static bool IsUrbanOccupation(string occupation)
			{
				return occupation == "retainer_urban" || occupation == "mercenary_urban" || occupation == "merchant_urban" || occupation == "vagabond_urban" || occupation == "artisan_urban" || occupation == "physician_urban" || occupation == "healer_urban" || occupation == "bard_urban";
			}

			// Token: 0x040020F9 RID: 8441
			public const string Retainer = "retainer";

			// Token: 0x040020FA RID: 8442
			public const string Bard = "bard";

			// Token: 0x040020FB RID: 8443
			public const string Hunter = "hunter";

			// Token: 0x040020FC RID: 8444
			public const string Farmer = "farmer";

			// Token: 0x040020FD RID: 8445
			public const string Herder = "herder";

			// Token: 0x040020FE RID: 8446
			public const string Healer = "healer";

			// Token: 0x040020FF RID: 8447
			public const string Mercenary = "mercenary";

			// Token: 0x04002100 RID: 8448
			public const string Infantry = "infantry";

			// Token: 0x04002101 RID: 8449
			public const string Skirmisher = "skirmisher";

			// Token: 0x04002102 RID: 8450
			public const string Kern = "kern";

			// Token: 0x04002103 RID: 8451
			public const string Guard = "guard";

			// Token: 0x04002104 RID: 8452
			public const string RetainerUrban = "retainer_urban";

			// Token: 0x04002105 RID: 8453
			public const string MercenaryUrban = "mercenary_urban";

			// Token: 0x04002106 RID: 8454
			public const string MerchantUrban = "merchant_urban";

			// Token: 0x04002107 RID: 8455
			public const string VagabondUrban = "vagabond_urban";

			// Token: 0x04002108 RID: 8456
			public const string ArtisanUrban = "artisan_urban";

			// Token: 0x04002109 RID: 8457
			public const string PhysicianUrban = "physician_urban";

			// Token: 0x0400210A RID: 8458
			public const string HealerUrban = "healer_urban";

			// Token: 0x0400210B RID: 8459
			public const string BardUrban = "bard_urban";
		}
	}
}
