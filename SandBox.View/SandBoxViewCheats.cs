using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.View
{
	// Token: 0x0200000A RID: 10
	public static class SandBoxViewCheats
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002A64 File Offset: 0x00000C64
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_hero", "campaign")]
		public static string KillHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.kill_hero [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero;
			string text3;
			if (!CampaignCheats.TryGetObject<Hero>(text2, out hero, out text3, (Hero x) => x.IsActive && (x.IsLord || x.IsWanderer)))
			{
				return text3 + "\n" + text;
			}
			if (!hero.IsAlive)
			{
				return "Hero " + text2 + " is already dead.";
			}
			if (hero.DeathMark != KillCharacterAction.KillCharacterActionDetail.None)
			{
				return "Hero already has a death mark.";
			}
			if (hero.CurrentSettlement != null && !hero.IsNotable)
			{
				return "Hero cannot be killed while staying in a settlement.";
			}
			if (MapScreen.Instance.IsHeirSelectionPopupActive)
			{
				return "Hero cannot be killed during the heir selection.";
			}
			if (Campaign.Current.ConversationManager.OneToOneConversationHero != null)
			{
				return "Hero cannot be killed during a conversation.";
			}
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			bool flag;
			if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
			{
				MobileParty partyBelongedTo2 = hero.PartyBelongedTo;
				flag = ((partyBelongedTo2 != null) ? partyBelongedTo2.SiegeEvent : null) != null;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (!hero.CanDie(KillCharacterAction.KillCharacterActionDetail.DiedInBattle))
				{
					return "Hero can't die!";
				}
				hero.AddDeathMark(null, KillCharacterAction.KillCharacterActionDetail.DiedInBattle);
			}
			else
			{
				if (!hero.CanDie(KillCharacterAction.KillCharacterActionDetail.Murdered))
				{
					return "Hero can't die!";
				}
				KillCharacterAction.ApplyByMurder(hero, null, true);
			}
			return "Hero " + text2.ToLower() + " is killed.";
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002BB8 File Offset: 0x00000DB8
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_tournament", "campaign")]
		public static string FocusTournament(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.focus_tournament\".";
			}
			Settlement settlement = Settlement.FindFirst((Settlement x) => x.IsTown && Campaign.Current.TournamentManager.GetTournamentGame(x.Town) != null);
			if (settlement == null)
			{
				return "There isn't any tournament right now.";
			}
			((MapCameraView)typeof(MapCameraView).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)).SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
			settlement.Party.SetAsCameraFollowParty();
			return "Success";
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002C4C File Offset: 0x00000E4C
		[CommandLineFunctionality.CommandLineArgumentFunction("make_clan_mercenary_of_kingdom", "campaign")]
		public static string MakeClanMercenaryOfKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.MakeClanMercenaryOfKingdom [clan] | [kingdom] | [days]\".";
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count < 2)
			{
				return "Format is \"campaign.MakeClanMercenaryOfKingdom [clan] | [kingdom] | [days]\".";
			}
			Clan clan;
			string text;
			CampaignCheats.TryGetObject<Clan>(separatedNames[0], out clan, out text, null);
			if (clan == null)
			{
				return "Cant find the clan\n" + text;
			}
			Kingdom kingdom;
			string text2;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[1], out kingdom, out text2, null);
			if (kingdom == null)
			{
				return text2;
			}
			if (!clan.IsMinorFaction)
			{
				return "Clan is not suitable to be mercenary";
			}
			if (clan == Clan.PlayerClan)
			{
				return "Use join_kingdom or join_kingdom_as_mercenary";
			}
			if (clan.IsUnderMercenaryService)
			{
				ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(clan, true);
			}
			CampaignTime campaignTime = CampaignTime.Zero;
			int num;
			if (separatedNames.Count == 3 && int.TryParse(separatedNames[2], out num))
			{
				campaignTime = CampaignTime.DaysFromNow((float)num);
			}
			ChangeKingdomAction.ApplyByJoinFactionAsMercenary(clan, kingdom, campaignTime, 50, true);
			return "Success";
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D44 File Offset: 0x00000F44
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_hostile_army", "campaign")]
		public static string FocusHostileArmy(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.focus_hostile_army\".";
			}
			Army army = null;
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (kingdom != Clan.PlayerClan.MapFaction && !kingdom.Armies.IsEmpty<Army>() && kingdom.IsAtWarWith(Clan.PlayerClan.MapFaction))
				{
					army = kingdom.Armies.GetRandomElement<Army>();
				}
				if (army != null)
				{
					break;
				}
			}
			if (army == null)
			{
				return "There isn't any hostile army right now.";
			}
			((MapCameraView)typeof(MapCameraView).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)).SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
			army.LeaderParty.Party.SetAsCameraFollowParty();
			return "Success";
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E34 File Offset: 0x00001034
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_mobile_party", "campaign")]
		public static string FocusMobileParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.focus_mobile_party [PartyName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			MobileParty mobileParty;
			string text2;
			if (CampaignCheats.TryGetObject<MobileParty>(CampaignCheats.ConcatenateString(strings), out mobileParty, out text2, null))
			{
				MapCameraView mapCameraView = (MapCameraView)typeof(MapCameraView).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
				if (!mobileParty.IsVisible && mobileParty.CurrentSettlement == null)
				{
					mobileParty.IsVisible = true;
				}
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				mobileParty.Party.SetAsCameraFollowParty();
				return string.Format("Focused party {0}", mobileParty.Name);
			}
			return text2 + " : \n" + text;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002EE4 File Offset: 0x000010E4
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_hero", "campaign")]
		public static string FocusHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.focus_hero [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero;
			string text3;
			if (!CampaignCheats.TryGetObject<Hero>(text2, out hero, out text3, (Hero x) => x != Hero.MainHero && x.IsActive && (x.IsLord || x.IsWanderer)))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			MapCameraView mapCameraView = (MapCameraView)typeof(MapCameraView).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			if (hero.CurrentSettlement != null)
			{
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				hero.CurrentSettlement.Party.SetAsCameraFollowParty();
				return "Success";
			}
			if (hero.PartyBelongedTo != null)
			{
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				hero.PartyBelongedTo.Party.SetAsCameraFollowParty();
				if (hero.PartyBelongedTo.CurrentSettlement == null && !hero.PartyBelongedTo.IsVisible)
				{
					hero.PartyBelongedTo.IsVisible = true;
				}
				return "Success";
			}
			if (hero.PartyBelongedToAsPrisoner != null)
			{
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				hero.PartyBelongedToAsPrisoner.SetAsCameraFollowParty();
				if (hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement == null && !hero.PartyBelongedToAsPrisoner.MobileParty.IsVisible)
				{
					hero.PartyBelongedToAsPrisoner.MobileParty.IsVisible = true;
				}
				return "Success";
			}
			return "Party is not found : " + text2 + "\n" + text;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003078 File Offset: 0x00001278
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_nearest_wreckage", "campaign")]
		public static string FocusNearestWreckage(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.focus_nearest_wreckage [Small|Normal|Epic]\".";
			}
			BattleWreckage.WreckageType wreckageType;
			if (strings == null || strings.Count == 0 || !Enum.TryParse<BattleWreckage.WreckageType>(strings[0], true, out wreckageType) || wreckageType == BattleWreckage.WreckageType.Invalid)
			{
				return "Format is \"campaign.focus_nearest_wreckage [Small|Normal|Epic]\".";
			}
			IEnumerable<BattleWreckage> enumerable = Campaign.Current.Wreckages.Where<BattleWreckage>((BattleWreckage x) => x.WreckageTypeCategory == wreckageType && !x.IsInvestigated);
			if (enumerable.IsEmpty<BattleWreckage>())
			{
				return "There is no " + wreckageType + " wreckage on the map right now.";
			}
			BattleWreckage battleWreckage = enumerable.OrderBy<BattleWreckage, float>((BattleWreckage x) => x.Position.Distance(MobileParty.MainParty.Position)).First<BattleWreckage>();
			MapScreen.Instance.FastMoveCameraToPosition(battleWreckage.Position);
			return "Success";
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003158 File Offset: 0x00001358
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_infested_hideout", "campaign")]
		public static string FocusInfestedHideout(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.focus_infested_hideout [Optional: Number of troops]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			MBList<Settlement> mblist = Settlement.All.Where<Settlement>((Settlement t) => t.IsHideout && t.Parties.Count > 0).ToMBList<Settlement>();
			if (mblist.IsEmpty<Settlement>())
			{
				return "All hideouts are empty!";
			}
			Settlement settlement;
			if (strings.Count > 0)
			{
				int troopCount = -1;
				int.TryParse(strings[0], out troopCount);
				if (troopCount == -1)
				{
					return "Incorrect input.\n" + text;
				}
				MBList<Settlement> mblist2 = mblist.Where<Settlement>((Settlement t) => t.Parties.Sum<MobileParty>((MobileParty p) => p.MemberRoster.TotalManCount) >= troopCount).ToMBList<Settlement>();
				if (mblist2.IsEmpty<Settlement>())
				{
					return "Can't find suitable hideout.";
				}
				settlement = mblist2.GetRandomElement<Settlement>();
			}
			else
			{
				settlement = mblist.GetRandomElement<Settlement>();
			}
			if (settlement != null)
			{
				((MapCameraView)typeof(MapCameraView).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)).SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				settlement.Party.SetAsCameraFollowParty();
				return "Success";
			}
			return "Unable to find such a hideout.";
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003280 File Offset: 0x00001480
		[CommandLineFunctionality.CommandLineArgumentFunction("focus_issue", "campaign")]
		public static string FocusIssues(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.focus_issue [IssueName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			MapCameraView mapCameraView = (MapCameraView)typeof(MapCameraView).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			IssueBase issueBase;
			string text2;
			CampaignCheats.TryGetObject<IssueBase>(CampaignCheats.ConcatenateString(strings), out issueBase, out text2, null);
			if (issueBase == null)
			{
				return text2 + " " + text;
			}
			if (issueBase.IssueSettlement != null)
			{
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				issueBase.IssueSettlement.Party.SetAsCameraFollowParty();
			}
			else if (issueBase.IssueOwner.PartyBelongedTo != null)
			{
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				MobileParty partyBelongedTo = issueBase.IssueOwner.PartyBelongedTo;
				if (partyBelongedTo != null)
				{
					partyBelongedTo.Party.SetAsCameraFollowParty();
				}
			}
			else if (issueBase.IssueOwner.CurrentSettlement != null)
			{
				mapCameraView.SetCameraMode(MapCameraView.CameraFollowMode.FollowParty);
				issueBase.IssueOwner.CurrentSettlement.Party.SetAsCameraFollowParty();
			}
			return "Found issue: " + issueBase.Title.ToString() + ". Issue Owner: " + issueBase.IssueOwner.Name.ToString();
		}
	}
}
