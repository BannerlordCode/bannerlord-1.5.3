using System;
using System.Collections.Generic;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace StoryMode
{
	// Token: 0x02000012 RID: 18
	public static class StoryModeCheats
	{
		// Token: 0x0600008E RID: 142 RVA: 0x000048FD File Offset: 0x00002AFD
		public static bool CheckCheatUsage(ref string message)
		{
			if (!CampaignCheats.CheckCheatUsage(ref message))
			{
				return false;
			}
			if (StoryModeManager.Current == null)
			{
				message = "Game mode is not correct!";
				return false;
			}
			return true;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000491C File Offset: 0x00002B1C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_family_members", "storymode")]
		public static string AddFamilyMembers(List<string> strings)
		{
			string empty = string.Empty;
			if (!StoryModeCheats.CheckCheatUsage(ref empty))
			{
				return empty;
			}
			foreach (Hero hero in new List<Hero>
			{
				StoryModeHeroes.LittleBrother,
				StoryModeHeroes.ElderBrother,
				StoryModeHeroes.LittleSister
			})
			{
				AddHeroToPartyAction.Apply(hero, MobileParty.MainParty, true);
				hero.Clan = Clan.PlayerClan;
			}
			return "Success";
		}

		// Token: 0x0400002F RID: 47
		public const string NotStoryMode = "Game mode is not correct!";
	}
}
