using System;
using System.Linq;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents
{
	// Token: 0x0200004C RID: 76
	public class StoryModeVoiceOverModel : VoiceOverModel
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x00019B50 File Offset: 0x00017D50
		public override string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject)
		{
			if (voiceObject == null)
			{
				return "";
			}
			if (!TutorialPhase.Instance.IsCompleted && TutorialPhase.Instance.TutorialVillageHeadman.CharacterObject == character)
			{
				string text = voiceObject.VoicePaths.First<string>();
				Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text, 0, Debug.DebugColor.White, 17592186044416UL);
				text = text.Replace("$PLATFORM", "PC");
				return text + ".ogg";
			}
			if (StoryModeHeroes.ElderBrother.CharacterObject != character)
			{
				return base.BaseModel.GetSoundPathForCharacter(character, voiceObject);
			}
			string text2 = "";
			string text3 = character.StringId + "_" + (CharacterObject.PlayerCharacter.IsFemale ? "female" : "male");
			foreach (string text4 in voiceObject.VoicePaths)
			{
				if (text4.Contains(text3))
				{
					text2 = text4;
					break;
				}
				if (text4.Contains(character.StringId + "_"))
				{
					text2 = text4;
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			Debug.Print("[VOICEOVER]Sound path found: " + BasePath.Name + text2, 0, Debug.DebugColor.White, 17592186044416UL);
			text2 = text2.Replace("$PLATFORM", "PC");
			return text2 + ".ogg";
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019CD0 File Offset: 0x00017ED0
		public override string GetAccentClass(CultureObject culture, bool isHighClass)
		{
			return base.BaseModel.GetAccentClass(culture, isHighClass);
		}

		// Token: 0x04000196 RID: 406
		private const string Male = "male";

		// Token: 0x04000197 RID: 407
		private const string Female = "female";
	}
}
