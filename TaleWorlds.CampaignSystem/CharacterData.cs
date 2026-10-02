using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000081 RID: 129
	public class CharacterData
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x000516BE File Offset: 0x0004F8BE
		private CharacterData()
		{
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x000516C8 File Offset: 0x0004F8C8
		private static CharacterData CreateFrom(Hero hero)
		{
			CharacterData characterData = new CharacterData();
			characterData.Name = hero.Name.ToString();
			characterData.Age = hero.Age;
			characterData.Culture = hero.Culture.StringId;
			characterData.Gold = hero.Gold;
			characterData.Race = hero.CharacterObject.Race;
			characterData.Level = hero.Level;
			characterData.IsFemale = hero.IsFemale;
			characterData.Weight = hero.Weight;
			characterData.Build = hero.Build;
			characterData.CivilianEquipmentCode = hero.CivilianEquipment.CalculateEquipmentCode();
			characterData.StealthEquipmentCode = hero.StealthEquipment.CalculateEquipmentCode();
			characterData.BattleEquipmentCode = hero.BattleEquipment.CalculateEquipmentCode();
			characterData.BodyPropertyKeys = new ulong[]
			{
				hero.StaticBodyProperties.KeyPart1,
				hero.StaticBodyProperties.KeyPart2,
				hero.StaticBodyProperties.KeyPart3,
				hero.StaticBodyProperties.KeyPart4,
				hero.StaticBodyProperties.KeyPart5,
				hero.StaticBodyProperties.KeyPart6,
				hero.StaticBodyProperties.KeyPart7,
				hero.StaticBodyProperties.KeyPart8
			};
			characterData.UnspentAttributePoints = hero.HeroDeveloper.UnspentAttributePoints;
			characterData.UnspentFocusPoints = hero.HeroDeveloper.UnspentFocusPoints;
			characterData.SkillsArray = new CharacterData.SkillObjectData[Skills.All.Count];
			characterData.AttributesArray = new CharacterData.PropertyObjectData[Attributes.All.Count];
			characterData.Traits = new CharacterData.PropertyObjectData[TraitObject.All.Count];
			for (int i = 0; i < Skills.All.Count; i++)
			{
				characterData.SkillsArray[i] = new CharacterData.SkillObjectData(Skills.All[i].StringId, hero.GetSkillValue(Skills.All[i]), hero.HeroDeveloper.GetSkillXpProgress(Skills.All[i]), hero.HeroDeveloper.GetFocus(Skills.All[i]));
			}
			List<string> list = new List<string>();
			for (int j = 0; j < PerkObject.All.Count; j++)
			{
				if (hero.GetPerkValue(PerkObject.All[j]))
				{
					list.Add(PerkObject.All[j].StringId);
				}
			}
			characterData.UnlockedPerks = list.ToArray();
			for (int k = 0; k < Attributes.All.Count; k++)
			{
				characterData.AttributesArray[k] = new CharacterData.PropertyObjectData(Attributes.All[k].StringId, hero.GetAttributeValue(Attributes.All[k]));
			}
			for (int l = 0; l < TraitObject.All.Count; l++)
			{
				characterData.Traits[l] = new CharacterData.PropertyObjectData(TraitObject.All[l].StringId, hero.GetTraitLevel(TraitObject.All[l]));
			}
			return characterData;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000519D8 File Offset: 0x0004FBD8
		private static void InitializeHeroFromCharacterData(Hero target, CharacterData characterData)
		{
			TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
			textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(characterData.Name, null));
			TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
			textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(characterData.Name, null));
			target.Gold = characterData.Gold;
			target.IsFemale = characterData.IsFemale;
			target.CharacterObject.Race = characterData.Race;
			float num = characterData.Age;
			if (num < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				num = (float)Campaign.Current.Models.AgeModel.HeroComesOfAge;
			}
			target.SetBirthDay(CampaignTime.YearsFromNow(-num));
			target.Weight = characterData.Weight;
			target.Build = characterData.Build;
			target.Level = characterData.Level;
			Equipment equipment = Equipment.CreateFromEquipmentCode(characterData.BattleEquipmentCode);
			Equipment equipment2 = Equipment.CreateFromEquipmentCode(characterData.CivilianEquipmentCode);
			Equipment equipment3 = null;
			if (!string.IsNullOrEmpty(characterData.StealthEquipmentCode))
			{
				equipment3 = Equipment.CreateFromEquipmentCode(characterData.StealthEquipmentCode);
			}
			for (int i = 0; i < 12; i++)
			{
				if (target.PartyBelongedTo != null)
				{
					if (!target.BattleEquipment[i].IsEmpty)
					{
						target.PartyBelongedTo.ItemRoster.AddToCounts(target.BattleEquipment[i], 1);
					}
					if (!target.CivilianEquipment[i].IsEmpty)
					{
						target.PartyBelongedTo.ItemRoster.AddToCounts(target.CivilianEquipment[i], 1);
					}
					if (!target.StealthEquipment[i].IsEmpty)
					{
						target.PartyBelongedTo.ItemRoster.AddToCounts(target.StealthEquipment[i], 1);
					}
				}
				target.BattleEquipment[i] = equipment[i];
				target.CivilianEquipment[i] = equipment2[i];
				if (equipment3 != null)
				{
					target.StealthEquipment[i] = equipment3[i];
				}
			}
			CultureObject @object = MBObjectManager.Instance.GetObject<CultureObject>(characterData.Culture);
			if (@object != null)
			{
				target.Culture = @object;
			}
			ulong[] bodyPropertyKeys = characterData.BodyPropertyKeys;
			target.StaticBodyProperties = new StaticBodyProperties(bodyPropertyKeys[0], bodyPropertyKeys[1], bodyPropertyKeys[2], bodyPropertyKeys[3], bodyPropertyKeys[4], bodyPropertyKeys[5], bodyPropertyKeys[6], bodyPropertyKeys[7]);
			target.HeroDeveloper.UnspentFocusPoints = characterData.UnspentFocusPoints;
			target.HeroDeveloper.UnspentAttributePoints = characterData.UnspentAttributePoints;
			for (int j = 0; j < characterData.SkillsArray.Length; j++)
			{
				CharacterData.SkillObjectData skillObjectData = characterData.SkillsArray[j];
				string stringId = skillObjectData.StringId;
				int num2 = skillObjectData.Value;
				int num3 = skillObjectData.Focus;
				int progress = skillObjectData.Progress;
				SkillObject object2 = MBObjectManager.Instance.GetObject<SkillObject>(stringId);
				if (object2 != null)
				{
					int focus = target.HeroDeveloper.GetFocus(object2);
					num2 = Math.Max(0, num2);
					int xpRequiredForSkillLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(num2);
					int xpRequiredForSkillLevel2 = Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(num2 + 1);
					int num4 = Math.Min(progress + xpRequiredForSkillLevel, xpRequiredForSkillLevel2);
					target.HeroDeveloper.SetSkillXp(object2, (float)num4);
					target.SetSkillValue(object2, num2);
					num3 = Math.Max(Math.Min(num3, Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill), 0);
					if (focus < num3)
					{
						target.HeroDeveloper.AddFocus(object2, num3 - focus, false);
					}
					else
					{
						target.HeroDeveloper.RemoveFocus(object2, focus - num3);
					}
				}
			}
			for (int k = 0; k < characterData.Traits.Length; k++)
			{
				CharacterData.PropertyObjectData propertyObjectData = characterData.Traits[k];
				string stringId2 = propertyObjectData.StringId;
				int num5 = propertyObjectData.Value;
				TraitObject object3 = MBObjectManager.Instance.GetObject<TraitObject>(stringId2);
				if (object3 != null)
				{
					num5 = Math.Max(Math.Min(num5, object3.MaxValue), object3.MinValue);
					target.SetTraitLevel(object3, num5);
				}
			}
			for (int l = 0; l < characterData.AttributesArray.Length; l++)
			{
				CharacterData.PropertyObjectData propertyObjectData2 = characterData.AttributesArray[l];
				string stringId3 = propertyObjectData2.StringId;
				int value = propertyObjectData2.Value;
				CharacterAttribute object4 = MBObjectManager.Instance.GetObject<CharacterAttribute>(stringId3);
				if (object4 != null)
				{
					int num6 = ((target.GetAttributeValue(object4) > value) ? (value - target.GetAttributeValue(object4)) : (value - target.GetAttributeValue(object4)));
					target.HeroDeveloper.AddAttribute(object4, num6, false);
				}
			}
			target.ClearPerks();
			for (int m = 0; m < characterData.UnlockedPerks.Length; m++)
			{
				string text = characterData.UnlockedPerks[m];
				PerkObject object5 = MBObjectManager.Instance.GetObject<PerkObject>(text);
				if (object5 != null)
				{
					target.HeroDeveloper.AddPerk(object5);
				}
			}
			target.HeroDeveloper.SetInitialLevel(target.Level);
			target.SetName(textObject2, textObject);
			Hero.SetHeroEncyclopediaTextAndLinks(target);
			if (GameStateManager.Current.ActiveState is MapState && target.PartyBelongedTo != null)
			{
				target.PartyBelongedTo.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00051EFC File Offset: 0x000500FC
		public static void ExportCharacter(Hero hero, string path)
		{
			CharacterData characterData = CharacterData.CreateFrom(hero);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterData));
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				xmlSerializer.Serialize(streamWriter, characterData);
			}
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00051F4C File Offset: 0x0005014C
		public static void ImportCharacter(Hero hero, string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterData));
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				CharacterData characterData = (CharacterData)xmlSerializer.Deserialize(fileStream);
				CharacterData.InitializeHeroFromCharacterData(Hero.MainHero, characterData);
			}
		}

		// Token: 0x040004BD RID: 1213
		public const string CharacterDataExtension = "char";

		// Token: 0x040004BE RID: 1214
		[XmlElement]
		public string Name;

		// Token: 0x040004BF RID: 1215
		[XmlElement]
		public bool IsFemale;

		// Token: 0x040004C0 RID: 1216
		[XmlElement]
		public int Gold;

		// Token: 0x040004C1 RID: 1217
		[XmlElement]
		public int Race;

		// Token: 0x040004C2 RID: 1218
		[XmlElement]
		public int Level;

		// Token: 0x040004C3 RID: 1219
		[XmlElement]
		public string Culture;

		// Token: 0x040004C4 RID: 1220
		[XmlElement]
		public float Age;

		// Token: 0x040004C5 RID: 1221
		[XmlElement]
		public float Weight;

		// Token: 0x040004C6 RID: 1222
		[XmlElement]
		public float Build;

		// Token: 0x040004C7 RID: 1223
		[XmlElement]
		public string CivilianEquipmentCode;

		// Token: 0x040004C8 RID: 1224
		[XmlElement]
		public string BattleEquipmentCode;

		// Token: 0x040004C9 RID: 1225
		[XmlElement]
		public string StealthEquipmentCode;

		// Token: 0x040004CA RID: 1226
		[XmlArray("BodyPropertyKeys")]
		[XmlArrayItem("Key")]
		public ulong[] BodyPropertyKeys;

		// Token: 0x040004CB RID: 1227
		[XmlElement]
		public int UnspentFocusPoints;

		// Token: 0x040004CC RID: 1228
		[XmlElement]
		public int UnspentAttributePoints;

		// Token: 0x040004CD RID: 1229
		[XmlArray("Perks")]
		[XmlArrayItem("Perk")]
		public string[] UnlockedPerks;

		// Token: 0x040004CE RID: 1230
		[XmlArray("Attributes")]
		[XmlArrayItem("Attribute")]
		public CharacterData.PropertyObjectData[] AttributesArray;

		// Token: 0x040004CF RID: 1231
		[XmlArray("Traits")]
		[XmlArrayItem("Trait")]
		public CharacterData.PropertyObjectData[] Traits;

		// Token: 0x040004D0 RID: 1232
		[XmlArray("Skills")]
		[XmlArrayItem("Skill")]
		public CharacterData.SkillObjectData[] SkillsArray;

		// Token: 0x0200056B RID: 1387
		public class PropertyObjectData
		{
			// Token: 0x06005006 RID: 20486 RVA: 0x0018E505 File Offset: 0x0018C705
			public PropertyObjectData(string id, int value)
			{
				this.StringId = id;
				this.Value = value;
			}

			// Token: 0x06005007 RID: 20487 RVA: 0x0018E51B File Offset: 0x0018C71B
			public PropertyObjectData()
			{
			}

			// Token: 0x040017A7 RID: 6055
			[XmlElement]
			public string StringId;

			// Token: 0x040017A8 RID: 6056
			[XmlElement]
			public int Value;
		}

		// Token: 0x0200056C RID: 1388
		public class SkillObjectData : CharacterData.PropertyObjectData
		{
			// Token: 0x06005008 RID: 20488 RVA: 0x0018E523 File Offset: 0x0018C723
			public SkillObjectData(string id, int value, int progress, int focus)
				: base(id, value)
			{
				this.Focus = focus;
				this.Progress = progress;
			}

			// Token: 0x06005009 RID: 20489 RVA: 0x0018E53C File Offset: 0x0018C73C
			public SkillObjectData()
			{
			}

			// Token: 0x040017A9 RID: 6057
			[XmlElement]
			public int Focus;

			// Token: 0x040017AA RID: 6058
			[XmlElement]
			public int Progress;
		}
	}
}
