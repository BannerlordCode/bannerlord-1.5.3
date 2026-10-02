using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D9 RID: 217
	public class ConversationAnimationToolCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060009CD RID: 2509 RVA: 0x00048EB4 File Offset: 0x000470B4
		public override void RegisterEvents()
		{
			CampaignEvents.TickEvent.AddNonSerializedListener(this, new Action<float>(this.Tick));
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00048ECD File Offset: 0x000470CD
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00048ED0 File Offset: 0x000470D0
		private void Tick(float dt)
		{
			if (ConversationAnimationToolCampaignBehavior._isToolEnabled)
			{
				ConversationAnimationToolCampaignBehavior.StartImGUIWindow("Conversation Animation Test Tool");
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Type:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("0 for noble, 1 for notable, 2 for companion, 3 for troop", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter character type: ", ref ConversationAnimationToolCampaignBehavior._characterType, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character State:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("0 for active, 1 for prisoner", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter character state: ", ref ConversationAnimationToolCampaignBehavior._characterState, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Gender:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("0 for male, 1 for female", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter character gender: ", ref ConversationAnimationToolCampaignBehavior._characterGender, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Age:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Enter a custom age or leave -1 for not changing the age value", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter character age: ", ref ConversationAnimationToolCampaignBehavior._characterAge, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Wounded State:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Change to 1 to change character state to wounded", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter character wounded state: ", ref ConversationAnimationToolCampaignBehavior._characterWoundedState, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Equipment Type:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Change to 1 to change to equipment to civilian, default equipment is battle", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter equipment type: ", ref ConversationAnimationToolCampaignBehavior._equipmentType, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Relation With Main Hero:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Leave -1 for no change, 0 for enemy, 1 for neutral, 2 for friend", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter relation type: ", ref ConversationAnimationToolCampaignBehavior._relationType, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Character Persona Type:", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUITextArea("Leave -1 for no change, 0 for curt, 1 for earnest, 2 for ironic, 3 for softspoken", false, false);
				ConversationAnimationToolCampaignBehavior.ImGUIIntegerField("Enter persona type: ", ref ConversationAnimationToolCampaignBehavior._personaType, false, false);
				ConversationAnimationToolCampaignBehavior.Separator();
				if (ConversationAnimationToolCampaignBehavior.ImGUIButton(" Start Conversation ", true))
				{
					ConversationAnimationToolCampaignBehavior.StartConversation();
				}
				ConversationAnimationToolCampaignBehavior.EndImGUIWindow();
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00049078 File Offset: 0x00047278
		public static void CloseConversationAnimationTool()
		{
			ConversationAnimationToolCampaignBehavior._isToolEnabled = false;
			ConversationAnimationToolCampaignBehavior._characterType = -1;
			ConversationAnimationToolCampaignBehavior._characterState = -1;
			ConversationAnimationToolCampaignBehavior._characterGender = -1;
			ConversationAnimationToolCampaignBehavior._characterAge = -1;
			ConversationAnimationToolCampaignBehavior._characterWoundedState = -1;
			ConversationAnimationToolCampaignBehavior._equipmentType = -1;
			ConversationAnimationToolCampaignBehavior._relationType = -1;
			ConversationAnimationToolCampaignBehavior._personaType = -1;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000490B0 File Offset: 0x000472B0
		private static void StartConversation()
		{
			bool flag = true;
			bool flag2 = true;
			Occupation occupation = Occupation.NotAssigned;
			switch (ConversationAnimationToolCampaignBehavior._characterType)
			{
			case 0:
				occupation = Occupation.Lord;
				break;
			case 1:
				occupation = Occupation.Merchant;
				break;
			case 2:
				occupation = Occupation.Wanderer;
				break;
			case 3:
				occupation = Occupation.Soldier;
				flag2 = false;
				break;
			default:
				flag = false;
				break;
			}
			if (!flag)
			{
				return;
			}
			bool flag3 = false;
			bool flag4 = false;
			if (ConversationAnimationToolCampaignBehavior._characterState == 0)
			{
				flag3 = true;
			}
			else if (ConversationAnimationToolCampaignBehavior._characterState == 1)
			{
				flag4 = true;
			}
			else
			{
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			bool flag5 = false;
			if (ConversationAnimationToolCampaignBehavior._characterGender == 1)
			{
				flag5 = true;
			}
			else if (ConversationAnimationToolCampaignBehavior._characterGender == 0)
			{
				flag5 = false;
			}
			else
			{
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			bool flag6 = false;
			if (ConversationAnimationToolCampaignBehavior._characterAge == -1)
			{
				flag6 = false;
			}
			else if (ConversationAnimationToolCampaignBehavior._characterAge > 0 && ConversationAnimationToolCampaignBehavior._characterAge <= 128)
			{
				flag6 = true;
			}
			else
			{
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			bool flag7 = ConversationAnimationToolCampaignBehavior._characterWoundedState == 1;
			bool flag8 = ConversationAnimationToolCampaignBehavior._equipmentType == 1;
			if (ConversationAnimationToolCampaignBehavior._relationType != 0 && ConversationAnimationToolCampaignBehavior._relationType != 1 && ConversationAnimationToolCampaignBehavior._relationType != 2)
			{
				return;
			}
			CharacterObject characterObject = null;
			if (flag2)
			{
				Hero hero = null;
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (hero2 != Hero.MainHero && hero2.Occupation == occupation && hero2.IsFemale == flag5 && (hero2.PartyBelongedTo == null || hero2.PartyBelongedTo.MapEvent == null))
					{
						hero = hero2;
						break;
					}
				}
				if (hero == null)
				{
					hero = HeroCreator.CreateNotable(occupation, null);
				}
				if (flag6)
				{
					hero.SetBirthDay(HeroHelper.GetRandomBirthDayForAge((float)ConversationAnimationToolCampaignBehavior._characterAge));
				}
				if (flag4)
				{
					TakePrisonerAction.Apply(PartyBase.MainParty, hero);
				}
				if (flag7)
				{
					hero.MakeWounded(null, KillCharacterAction.KillCharacterActionDetail.None);
				}
				if (flag3)
				{
					hero.ChangeState(Hero.CharacterStates.Active);
				}
				hero.IsFemale = flag5;
				characterObject = hero.CharacterObject;
			}
			else
			{
				foreach (CharacterObject characterObject2 in CharacterObject.All)
				{
					if (characterObject2.Occupation == occupation && characterObject2.IsFemale == flag5)
					{
						characterObject = characterObject2;
						break;
					}
				}
				if (characterObject == null)
				{
					characterObject = Campaign.Current.ObjectManager.GetObject<CultureObject>("empire").BasicTroop;
				}
			}
			if (characterObject == null)
			{
				return;
			}
			if (characterObject.IsHero && ConversationAnimationToolCampaignBehavior._relationType != -1)
			{
				Hero heroObject = characterObject.HeroObject;
				float relationWithPlayer = heroObject.GetRelationWithPlayer();
				float num = 0f;
				if (ConversationAnimationToolCampaignBehavior._relationType == 0 && !heroObject.IsEnemy(Hero.MainHero))
				{
					num = -relationWithPlayer - 15f;
				}
				else if (ConversationAnimationToolCampaignBehavior._relationType == 1 && !heroObject.IsNeutral(Hero.MainHero))
				{
					num = -relationWithPlayer;
				}
				else if (ConversationAnimationToolCampaignBehavior._relationType == 2 && !heroObject.IsFriend(Hero.MainHero))
				{
					num = -relationWithPlayer + 15f;
				}
				ChangeRelationAction.ApplyPlayerRelation(heroObject, (int)num, true, true);
			}
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(characterObject, null, false, false, false, flag8, flag8, false));
			ConversationAnimationToolCampaignBehavior.CloseConversationAnimationTool();
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000493C8 File Offset: 0x000475C8
		private static void StartImGUIWindow(string str)
		{
			Imgui.BeginMainThreadScope();
			Imgui.Begin(str);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000493D5 File Offset: 0x000475D5
		private static void ImGUITextArea(string text, bool separatorNeeded, bool onSameLine)
		{
			Imgui.Text(text);
			ConversationAnimationToolCampaignBehavior.ImGUISeparatorSameLineHandler(separatorNeeded, onSameLine);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000493E4 File Offset: 0x000475E4
		private static bool ImGUIButton(string buttonText, bool smallButton)
		{
			if (smallButton)
			{
				return Imgui.SmallButton(buttonText);
			}
			return Imgui.Button(buttonText);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000493F6 File Offset: 0x000475F6
		private static void ImGUIIntegerField(string fieldText, ref int value, bool separatorNeeded, bool onSameLine)
		{
			Imgui.InputInt(fieldText, ref value);
			ConversationAnimationToolCampaignBehavior.ImGUISeparatorSameLineHandler(separatorNeeded, onSameLine);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00049407 File Offset: 0x00047607
		private static void ImGUICheckBox(string text, ref bool is_checked, bool separatorNeeded, bool onSameLine)
		{
			Imgui.Checkbox(text, ref is_checked);
			ConversationAnimationToolCampaignBehavior.ImGUISeparatorSameLineHandler(separatorNeeded, onSameLine);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00049418 File Offset: 0x00047618
		private static void ImGUISeparatorSameLineHandler(bool separatorNeeded, bool onSameLine)
		{
			if (separatorNeeded)
			{
				ConversationAnimationToolCampaignBehavior.Separator();
			}
			if (onSameLine)
			{
				ConversationAnimationToolCampaignBehavior.OnSameLine();
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0004942A File Offset: 0x0004762A
		private static void OnSameLine()
		{
			Imgui.SameLine(0f, 0f);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0004943B File Offset: 0x0004763B
		private static void Separator()
		{
			Imgui.Separator();
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00049442 File Offset: 0x00047642
		private static void EndImGUIWindow()
		{
			Imgui.End();
			Imgui.EndMainThreadScope();
		}

		// Token: 0x0400048E RID: 1166
		private static bool _isToolEnabled = false;

		// Token: 0x0400048F RID: 1167
		private static int _characterType = -1;

		// Token: 0x04000490 RID: 1168
		private static int _characterState = -1;

		// Token: 0x04000491 RID: 1169
		private static int _characterGender = -1;

		// Token: 0x04000492 RID: 1170
		private static int _characterAge = -1;

		// Token: 0x04000493 RID: 1171
		private static int _characterWoundedState = -1;

		// Token: 0x04000494 RID: 1172
		private static int _equipmentType = -1;

		// Token: 0x04000495 RID: 1173
		private static int _relationType = -1;

		// Token: 0x04000496 RID: 1174
		private static int _personaType = -1;
	}
}
