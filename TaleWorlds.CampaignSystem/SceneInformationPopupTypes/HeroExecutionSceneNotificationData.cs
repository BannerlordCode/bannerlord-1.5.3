using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C6 RID: 198
	public class HeroExecutionSceneNotificationData : SceneNotificationData
	{
		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0006010F File Offset: 0x0005E30F
		public Hero Executer { get; }

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x00060117 File Offset: 0x0005E317
		public Hero Victim { get; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0006011F File Offset: 0x0005E31F
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x00060127 File Offset: 0x0005E327
		public bool IsPlayerExecutionPrompt { get; private set; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x00060130 File Offset: 0x0005E330
		public override bool IsNegativeOptionShown { get; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x00060138 File Offset: 0x0005E338
		public override string SceneID
		{
			get
			{
				return "scn_execution_notification";
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x0006013F File Offset: 0x0005E33F
		public override TextObject NegativeText
		{
			get
			{
				return GameTexts.FindText("str_execution_negative_action", null);
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x0006014C File Offset: 0x0005E34C
		public override bool IsAffirmativeOptionShown
		{
			get
			{
				return !this._shouldAutoConfirm;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x00060157 File Offset: 0x0005E357
		public override bool ShouldAutoConfirm
		{
			get
			{
				return this._shouldAutoConfirm;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0006015F File Offset: 0x0005E35F
		public override TextObject TitleText { get; }

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x00060167 File Offset: 0x0005E367
		public override TextObject DescriptionText { get; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0006016F File Offset: 0x0005E36F
		public override TextObject AffirmativeText { get; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x00060177 File Offset: 0x0005E377
		public override TextObject AffirmativeTitleText { get; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0006017F File Offset: 0x0005E37F
		public override TextObject AffirmativeHintText { get; }

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x00060187 File Offset: 0x0005E387
		public override TextObject AffirmativeHintTextExtended { get; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0006018F File Offset: 0x0005E38F
		public override TextObject AffirmativeDescriptionText { get; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x00060197 File Offset: 0x0005E397
		public override SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x06001489 RID: 5257 RVA: 0x000601A0 File Offset: 0x0005E3A0
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			Equipment equipment = this.Victim.BattleEquipment.Clone(true);
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, default(EquipmentElement));
			ItemObject itemObject = Items.All.FirstOrDefault<ItemObject>((ItemObject i) => i.StringId == "execution_axe");
			Equipment equipment2 = this.Executer.Culture.Executioner.FirstBattleEquipment.Clone(true);
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(itemObject, null, null, false));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, default(EquipmentElement));
			SceneNotificationData.SceneNotificationCharacter sceneNotificationCharacter = CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.Executer, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false);
			if (this._useExecutioner)
			{
				sceneNotificationCharacter = this.CreateExecutorCharacter(this.Executer.Culture.Executioner, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false);
			}
			return new SceneNotificationData.SceneNotificationCharacter[]
			{
				CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.Victim, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false),
				sceneNotificationCharacter
			};
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00060337 File Offset: 0x0005E537
		private SceneNotificationData.SceneNotificationCharacter CreateExecutorCharacter(CharacterObject characterObject, Equipment overridenEquipment = null, bool useCivilian = false, BodyProperties overriddenBodyProperties = default(BodyProperties), uint overriddenColor1 = 4294967295U, uint overriddenColor2 = 4294967295U, bool useHorse = false)
		{
			if (overriddenColor1 == 4294967295U)
			{
				overriddenColor1 = characterObject.Culture.Color;
			}
			if (overriddenColor2 == 4294967295U)
			{
				overriddenColor2 = characterObject.Culture.Color2;
			}
			return new SceneNotificationData.SceneNotificationCharacter(characterObject, overridenEquipment, overriddenBodyProperties, useCivilian, overriddenColor1, overriddenColor2, useHorse);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00060370 File Offset: 0x0005E570
		private HeroExecutionSceneNotificationData(Hero executingHero, Hero dyingHero, TextObject titleText, TextObject descriptionText, TextObject affirmativeTitleText, TextObject affirmativeActionText, TextObject affirmativeActionDescriptionText, TextObject affirmativeActionHintText, TextObject affirmativeActionHintExtendedText, bool isNegativeOptionShown, Action onAffirmativeAction, Action onNegativeAction = null, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any, bool isVisualOnly = false, bool useExecutioner = false, bool shouldAutoConfirm = false)
		{
			this.Executer = executingHero;
			this.Victim = dyingHero;
			this.TitleText = titleText;
			this.DescriptionText = descriptionText;
			this.AffirmativeTitleText = affirmativeTitleText;
			this.AffirmativeText = affirmativeActionText;
			this.AffirmativeDescriptionText = affirmativeActionDescriptionText;
			this.AffirmativeHintText = affirmativeActionHintText;
			this.AffirmativeHintTextExtended = affirmativeActionHintExtendedText;
			this.IsNegativeOptionShown = isNegativeOptionShown;
			this.RelevantContext = relevantContextType;
			this._onAffirmativeAction = onAffirmativeAction;
			this._onNegativeAction = onNegativeAction;
			this._runAffirmativeActionAtClose = false;
			this._isVisualOnly = isVisualOnly;
			this._useExecutioner = useExecutioner;
			this._shouldAutoConfirm = shouldAutoConfirm;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00060407 File Offset: 0x0005E607
		public override void OnCloseAction()
		{
			this.PostponedAffirmativeAction();
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0006040F File Offset: 0x0005E60F
		public override void OnAffirmativeAction()
		{
			base.OnAffirmativeAction();
			this._runAffirmativeActionAtClose = true;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0006041E File Offset: 0x0005E61E
		public override void OnNegativeAction()
		{
			base.OnNegativeAction();
			Action onNegativeAction = this._onNegativeAction;
			if (onNegativeAction != null)
			{
				onNegativeAction();
			}
			this._runAffirmativeActionAtClose = false;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00060440 File Offset: 0x0005E640
		private void PostponedAffirmativeAction()
		{
			if (this._runAffirmativeActionAtClose && !this._isVisualOnly)
			{
				if (this._onAffirmativeAction != null)
				{
					this._onAffirmativeAction();
				}
				else if (this.Victim != Hero.MainHero)
				{
					if (this.Executer.PartyBelongedTo != null && this.Executer.PartyBelongedTo.MapEvent != null)
					{
						KillCharacterAction.ApplyByExecutionAfterMapEvent(this.Victim, this.Executer, true, true);
					}
					else
					{
						KillCharacterAction.ApplyByExecution(this.Victim, this.Executer, true, true);
					}
				}
			}
			this._runAffirmativeActionAtClose = false;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000604D0 File Offset: 0x0005E6D0
		public static HeroExecutionSceneNotificationData CreateForPlayerExecutingHero(Hero dyingHero, Action onAffirmativeAction, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any, bool showNegativeOption = true, Action onNegativeAction = null)
		{
			GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
			GameTexts.SetVariable("YEAR", CampaignTime.Now.GetYear);
			GameTexts.SetVariable("NAME", dyingHero.Name);
			string text = "CLAN_NAME";
			Clan clan = dyingHero.Clan;
			GameTexts.SetVariable(text, (clan != null) ? clan.Name : null);
			TextObject textObject = GameTexts.FindText("str_execution_positive_action", null);
			textObject.SetCharacterProperties("DYING_HERO", dyingHero.CharacterObject, false);
			return new HeroExecutionSceneNotificationData(Hero.MainHero, dyingHero, GameTexts.FindText("str_executing_prisoner", null), HeroExecutionSceneNotificationData.GetExecuteTroopDescriptionText(dyingHero), GameTexts.FindText("str_executed_prisoner", null), textObject, GameTexts.FindText("str_cannot_undo", null), HeroExecutionSceneNotificationData.GetExecuteTroopHintText(dyingHero, false), HeroExecutionSceneNotificationData.GetExecuteTroopHintText(dyingHero, true), showNegativeOption, onAffirmativeAction, onNegativeAction, relevantContextType, false, false, false)
			{
				IsPlayerExecutionPrompt = true
			};
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x000605A4 File Offset: 0x0005E7A4
		public static HeroExecutionSceneNotificationData CreateForInformingPlayer(Hero executingHero, Hero dyingHero, CampaignTime date, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any, Action onClose = null, bool isVisualOnly = false, bool useExecutioner = false, bool shouldAutoConfirm = false, bool showNegativeOption = false, Action onNegativeAction = null)
		{
			GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(date));
			GameTexts.SetVariable("YEAR", date.GetYear);
			GameTexts.SetVariable("NAME", dyingHero.Name);
			TextObject textObject = new TextObject("{=uYjEknNX}{VICTIM.NAME}'s execution by {EXECUTER.NAME}", null);
			textObject.SetCharacterProperties("VICTIM", dyingHero.CharacterObject, false);
			textObject.SetCharacterProperties("EXECUTER", executingHero.CharacterObject, false);
			if (useExecutioner)
			{
				textObject = new TextObject("{=2nOppdq8}{VICTIM.NAME}'s execution by order of {CLAN_NAME}", null);
				textObject.SetCharacterProperties("VICTIM", dyingHero.CharacterObject, false);
				textObject.SetTextVariable("CLAN_NAME", executingHero.Clan.Name);
			}
			return new HeroExecutionSceneNotificationData(executingHero, dyingHero, textObject, null, GameTexts.FindText("str_executed_prisoner", null), GameTexts.FindText("str_proceed", null), null, null, null, showNegativeOption, onClose, onNegativeAction, relevantContextType, isVisualOnly, useExecutioner, shouldAutoConfirm);
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0006067A File Offset: 0x0005E87A
		private static TextObject GetExecuteTroopDescriptionText(Hero dyingHero)
		{
			if (dyingHero.Clan == null)
			{
				return null;
			}
			if (dyingHero.Clan.HasBloodFeudWithPlayer)
			{
				return GameTexts.FindText("str_execute_prisoner_desc_blood_feud", null);
			}
			return GameTexts.FindText("str_execute_prisoner_desc_no_blood_feud", null);
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x000606AC File Offset: 0x0005E8AC
		private static TextObject GetExecuteTroopHintText(Hero dyingHero, bool showAll)
		{
			Dictionary<Clan, int> dictionary = new Dictionary<Clan, int>();
			GameTexts.SetVariable("LEFT", new TextObject("{=jxypVgl2}Relation Changes", null));
			string text = GameTexts.FindText("str_LEFT_colon", null).ToString();
			if (dyingHero.Clan != null && !dyingHero.Clan.HasBloodFeudWithPlayer)
			{
				foreach (Clan clan in Clan.All)
				{
					int bloodFeudStartRelationPenaltyToOtherClan = ExecutionCampaignBehavior.GetBloodFeudStartRelationPenaltyToOtherClan(dyingHero, clan);
					if (bloodFeudStartRelationPenaltyToOtherClan != 0)
					{
						if (dictionary.ContainsKey(clan))
						{
							if (bloodFeudStartRelationPenaltyToOtherClan < dictionary[clan])
							{
								dictionary[clan] = bloodFeudStartRelationPenaltyToOtherClan;
							}
						}
						else
						{
							dictionary.Add(clan, bloodFeudStartRelationPenaltyToOtherClan);
						}
					}
				}
				GameTexts.SetVariable("newline", "\n");
				List<KeyValuePair<Clan, int>> list = dictionary.OrderBy<KeyValuePair<Clan, int>, int>((KeyValuePair<Clan, int> change) => change.Value).ToList<KeyValuePair<Clan, int>>();
				int num = 0;
				foreach (KeyValuePair<Clan, int> keyValuePair in list)
				{
					Clan key = keyValuePair.Key;
					int value = keyValuePair.Value;
					GameTexts.SetVariable("LEFT", key.Name);
					GameTexts.SetVariable("RIGHT", value);
					string text2 = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
					GameTexts.SetVariable("STR1", text);
					GameTexts.SetVariable("STR2", text2);
					text = GameTexts.FindText("str_string_newline_string", null).ToString();
					num++;
					if (!showAll && num == HeroExecutionSceneNotificationData.MaxShownRelationChanges)
					{
						TextObject textObject = new TextObject("{=DPTPuyip}And {NUMBER} more...", null);
						GameTexts.SetVariable("NUMBER", dictionary.Count - num);
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", textObject);
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
						TextObject textObject2 = new TextObject("{=u12ocP9f}Hold '{EXTEND_KEY}' for more info.", null);
						textObject2.SetTextVariable("EXTEND_KEY", GameTexts.FindText("str_game_key_text", "anyalt"));
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", textObject2);
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
						break;
					}
				}
				return new TextObject("{=!}" + text, null);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x040006B4 RID: 1716
		private bool _runAffirmativeActionAtClose;

		// Token: 0x040006B5 RID: 1717
		private readonly Action _onAffirmativeAction;

		// Token: 0x040006B6 RID: 1718
		private readonly Action _onNegativeAction;

		// Token: 0x040006B7 RID: 1719
		protected static int MaxShownRelationChanges = 8;

		// Token: 0x040006B8 RID: 1720
		private bool _isVisualOnly;

		// Token: 0x040006B9 RID: 1721
		private bool _useExecutioner;

		// Token: 0x040006BA RID: 1722
		private readonly bool _shouldAutoConfirm;
	}
}
