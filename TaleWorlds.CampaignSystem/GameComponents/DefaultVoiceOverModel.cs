using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Tags;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200016E RID: 366
	public class DefaultVoiceOverModel : VoiceOverModel
	{
		// Token: 0x06001BA7 RID: 7079 RVA: 0x0008F55C File Offset: 0x0008D75C
		public override string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject)
		{
			if (voiceObject == null)
			{
				return "";
			}
			string text = "";
			string text2 = character.StringId + "_" + (CharacterObject.PlayerCharacter.IsFemale ? "female" : "male");
			foreach (string text3 in voiceObject.VoicePaths)
			{
				if (text3.Contains(text2))
				{
					text = text3;
					break;
				}
				if (text3.Contains(character.StringId + "_"))
				{
					text = text3;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				string accentClass = Campaign.Current.Models.VoiceOverModel.GetAccentClass(character.Culture, ConversationTagHelper.UsesHighRegister(character));
				Debug.Print("accentClass: " + accentClass, 0, Debug.DebugColor.White, 17592186044416UL);
				string text4 = (character.IsFemale ? "female" : "male");
				string stringId = character.GetPersona().StringId;
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				list2.Add(string.Concat(new string[] { ".+\\\\", accentClass, "_", text4, "_", stringId, "_.+" }));
				list2.Add(string.Concat(new string[] { ".+\\\\", accentClass, "_", text4, "_generic_.+" }));
				this.CheckPossibleMatches(voiceObject, list2, ref list, false, false);
				if (list.IsEmpty<string>())
				{
					list2.Clear();
					list2.Add(string.Concat(new string[] { ".+\\\\", accentClass, "_", stringId, "_.+" }));
					list2.Add(".+\\\\" + accentClass + "_generic_.+");
					list2.Add(string.Concat(new string[] { ".+\\\\", text4, "_", stringId, "_.+" }));
					list2.Add(".+\\\\" + text4 + "_generic_.+");
					this.CheckPossibleMatches(voiceObject, list2, ref list, false, false);
					if (list.IsEmpty<string>())
					{
						list2.Clear();
						list2.Add(".+\\\\" + stringId + "_.+");
						list2.Add(".+\\\\generic_.+");
						list2.Add(".+" + accentClass + "_.+");
						this.CheckPossibleMatches(voiceObject, list2, ref list, true, character.IsFemale);
					}
				}
				if (!list.IsEmpty<string>())
				{
					if (character.IsHero)
					{
						text = list[character.HeroObject.RandomInt(list.Count)];
					}
					else if (MobileParty.ConversationParty != null)
					{
						text = list[MobileParty.ConversationParty.RandomInt(list.Count)];
					}
					else
					{
						text = list.GetRandomElement<string>();
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text, 0, Debug.DebugColor.White, 17592186044416UL);
			text = text.Replace("$PLATFORM", "PC");
			return text + ".ogg";
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0008F8BC File Offset: 0x0008DABC
		private void CheckPossibleMatches(VoiceObject voiceObject, List<string> possibleMatches, ref List<string> possibleVoicePaths, bool doubleCheckForGender = false, bool isFemale = false)
		{
			foreach (string text in possibleMatches)
			{
				Regex regex = new Regex(text, RegexOptions.IgnoreCase);
				foreach (string text2 in voiceObject.VoicePaths)
				{
					if (regex.Match(text2).Success && !possibleVoicePaths.Contains(text2))
					{
						if (doubleCheckForGender)
						{
							if (text2.Contains("_male") || text2.Contains("_female"))
							{
								string text3 = (isFemale ? "_female" : "_male");
								if (text2.Contains(text3))
								{
									possibleVoicePaths.Add(text2);
								}
							}
						}
						else
						{
							possibleVoicePaths.Add(text2);
						}
					}
				}
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0008F9B0 File Offset: 0x0008DBB0
		public override string GetAccentClass(CultureObject culture, bool isHighClass)
		{
			if (culture.StringId == "empire")
			{
				if (isHighClass)
				{
					return "imperial_high";
				}
				return "imperial_low";
			}
			else
			{
				if (culture.StringId == "vlandia")
				{
					return "vlandian";
				}
				if (culture.StringId == "sturgia")
				{
					return "sturgian";
				}
				if (culture.StringId == "khuzait")
				{
					return "khuzait";
				}
				if (culture.StringId == "aserai")
				{
					return "aserai";
				}
				if (culture.StringId == "battania")
				{
					return "battanian";
				}
				if (culture.StringId == "forest_bandits")
				{
					return "forest_bandits";
				}
				if (culture.StringId == "sea_raiders")
				{
					return "sea_raiders";
				}
				if (culture.StringId == "mountain_bandits")
				{
					return "mountain_bandits";
				}
				if (culture.StringId == "desert_bandits")
				{
					return "desert_bandits";
				}
				if (culture.StringId == "steppe_bandits")
				{
					return "steppe_bandits";
				}
				if (culture.StringId == "looters")
				{
					return "looters";
				}
				return "";
			}
		}

		// Token: 0x0400093B RID: 2363
		private const string ImperialHighClass = "imperial_high";

		// Token: 0x0400093C RID: 2364
		private const string ImperialLowClass = "imperial_low";

		// Token: 0x0400093D RID: 2365
		private const string VlandianClass = "vlandian";

		// Token: 0x0400093E RID: 2366
		private const string SturgianClass = "sturgian";

		// Token: 0x0400093F RID: 2367
		private const string KhuzaitClass = "khuzait";

		// Token: 0x04000940 RID: 2368
		private const string AseraiClass = "aserai";

		// Token: 0x04000941 RID: 2369
		private const string BattanianClass = "battanian";

		// Token: 0x04000942 RID: 2370
		private const string ForestBanditClass = "forest_bandits";

		// Token: 0x04000943 RID: 2371
		private const string SeaBanditClass = "sea_raiders";

		// Token: 0x04000944 RID: 2372
		private const string MountainBanditClass = "mountain_bandits";

		// Token: 0x04000945 RID: 2373
		private const string DesertBanditClass = "desert_bandits";

		// Token: 0x04000946 RID: 2374
		private const string SteppeBanditClass = "steppe_bandits";

		// Token: 0x04000947 RID: 2375
		private const string LootersClass = "looters";

		// Token: 0x04000948 RID: 2376
		private const string Male = "male";

		// Token: 0x04000949 RID: 2377
		private const string Female = "female";

		// Token: 0x0400094A RID: 2378
		private const string GenericPersonaId = "generic";
	}
}
