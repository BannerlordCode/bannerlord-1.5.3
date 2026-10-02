using System;
using System.Collections.Generic;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace StoryMode
{
	// Token: 0x0200000C RID: 12
	public class MainStoryLine
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002BAC File Offset: 0x00000DAC
		public bool IsPlayerInteractionRestricted
		{
			get
			{
				return !this.TutorialPhase.IsCompleted && !this.IsOnImperialQuestLine && !this.IsOnAntiImperialQuestLine;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public bool IsOnImperialQuestLine
		{
			get
			{
				return this.MainStoryLineSide == MainStoryLineSide.CreateImperialKingdom || this.MainStoryLineSide == MainStoryLineSide.SupportImperialKingdom;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002BE6 File Offset: 0x00000DE6
		public bool IsOnAntiImperialQuestLine
		{
			get
			{
				return this.MainStoryLineSide == MainStoryLineSide.CreateAntiImperialKingdom || this.MainStoryLineSide == MainStoryLineSide.SupportAntiImperialKingdom;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002BFC File Offset: 0x00000DFC
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002C04 File Offset: 0x00000E04
		[SaveableProperty(2)]
		public TutorialPhase TutorialPhase { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002C0D File Offset: 0x00000E0D
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002C15 File Offset: 0x00000E15
		[SaveableProperty(3)]
		public FirstPhase FirstPhase { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002C1E File Offset: 0x00000E1E
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002C26 File Offset: 0x00000E26
		[SaveableProperty(4)]
		public SecondPhase SecondPhase { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002C2F File Offset: 0x00000E2F
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002C37 File Offset: 0x00000E37
		[SaveableProperty(5)]
		public ThirdPhase ThirdPhase { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002C40 File Offset: 0x00000E40
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002C48 File Offset: 0x00000E48
		[SaveableProperty(8)]
		public Kingdom PlayerSupportedKingdom { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002C51 File Offset: 0x00000E51
		public bool IsCompleted
		{
			get
			{
				return StoryModeManager.Current.MainStoryLine.ThirdPhase != null && StoryModeManager.Current.MainStoryLine.ThirdPhase.IsCompleted;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002C7A File Offset: 0x00000E7A
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002C82 File Offset: 0x00000E82
		public ItemObject DragonBanner { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002C8B File Offset: 0x00000E8B
		public bool IsFirstPhaseCompleted
		{
			get
			{
				return this.SecondPhase != null;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002C96 File Offset: 0x00000E96
		public bool IsSecondPhaseCompleted
		{
			get
			{
				return this.ThirdPhase != null;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CA1 File Offset: 0x00000EA1
		public MainStoryLine()
		{
			this.MainStoryLineSide = MainStoryLineSide.None;
			this.TutorialPhase = new TutorialPhase();
			this._tutorialScores = new Dictionary<string, float>();
			this.FamilyRescued = false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002CCD File Offset: 0x00000ECD
		public void OnSessionLaunched()
		{
			this.DragonBanner = Campaign.Current.ObjectManager.GetObject<ItemObject>("dragon_banner");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002CE9 File Offset: 0x00000EE9
		public void SetTutorialScores(Dictionary<string, float> scores)
		{
			this._tutorialScores = new Dictionary<string, float>(scores);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002CF7 File Offset: 0x00000EF7
		public Dictionary<string, float> GetTutorialScores()
		{
			return new Dictionary<string, float>(this._tutorialScores);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D04 File Offset: 0x00000F04
		public void SetStoryLineSide(MainStoryLineSide side)
		{
			this.MainStoryLineSide = side;
			this.PlayerSupportedKingdom = Clan.PlayerClan.Kingdom;
			StoryModeEvents.Instance.OnMainStoryLineSideChosen(this.MainStoryLineSide);
			DisableHeroAction.Apply(StoryModeHeroes.ImperialMentor);
			DisableHeroAction.Apply(StoryModeHeroes.AntiImperialMentor);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D41 File Offset: 0x00000F41
		public void SetMentorSettlements(Settlement imperialMentorSettlement, Settlement antiImperialMentorSettlement)
		{
			this.ImperialMentorSettlement = imperialMentorSettlement;
			this.AntiImperialMentorSettlement = antiImperialMentorSettlement;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002D54 File Offset: 0x00000F54
		public void CompleteTutorialPhase(bool isSkipped)
		{
			this.TutorialPhase.CompleteTutorial(isSkipped);
			this.FirstPhase = new FirstPhase();
			TutorialPhaseCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<TutorialPhaseCampaignBehavior>();
			if (campaignBehavior != null)
			{
				campaignBehavior.FinalizeTutorialPhase();
			}
			StoryModeEvents.Instance.OnStoryModeTutorialEnded();
			StoryModeManager.Current.MainStoryLine.FirstPhase.CollectBannerPiece();
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<TutorialPhaseCampaignBehavior>();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DB9 File Offset: 0x00000FB9
		public void CompleteFirstPhase()
		{
			this.SecondPhase = new SecondPhase();
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<FirstPhaseCampaignBehavior>();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DD5 File Offset: 0x00000FD5
		public void CompleteSecondPhase()
		{
			this.ThirdPhase = new ThirdPhase();
			StoryModeEvents.Instance.OnConspiracyActivated();
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<SecondPhaseCampaignBehavior>();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002DFB File Offset: 0x00000FFB
		public void CancelSecondAndThirdPhase()
		{
			if (this.SecondPhase != null)
			{
				Campaign.Current.CampaignBehaviorManager.RemoveBehavior<SecondPhaseCampaignBehavior>();
			}
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<ThirdPhaseCampaignBehavior>();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E23 File Offset: 0x00001023
		internal static void AutoGeneratedStaticCollectObjectsMainStoryLine(object o, List<object> collectedObjects)
		{
			((MainStoryLine)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E34 File Offset: 0x00001034
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this.ImperialMentorSettlement);
			collectedObjects.Add(this.AntiImperialMentorSettlement);
			collectedObjects.Add(this._tutorialScores);
			collectedObjects.Add(this.TutorialPhase);
			collectedObjects.Add(this.FirstPhase);
			collectedObjects.Add(this.SecondPhase);
			collectedObjects.Add(this.ThirdPhase);
			collectedObjects.Add(this.PlayerSupportedKingdom);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002EA1 File Offset: 0x000010A1
		internal static object AutoGeneratedGetMemberValueTutorialPhase(object o)
		{
			return ((MainStoryLine)o).TutorialPhase;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002EAE File Offset: 0x000010AE
		internal static object AutoGeneratedGetMemberValueFirstPhase(object o)
		{
			return ((MainStoryLine)o).FirstPhase;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002EBB File Offset: 0x000010BB
		internal static object AutoGeneratedGetMemberValueSecondPhase(object o)
		{
			return ((MainStoryLine)o).SecondPhase;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002EC8 File Offset: 0x000010C8
		internal static object AutoGeneratedGetMemberValueThirdPhase(object o)
		{
			return ((MainStoryLine)o).ThirdPhase;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002ED5 File Offset: 0x000010D5
		internal static object AutoGeneratedGetMemberValuePlayerSupportedKingdom(object o)
		{
			return ((MainStoryLine)o).PlayerSupportedKingdom;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002EE2 File Offset: 0x000010E2
		internal static object AutoGeneratedGetMemberValueMainStoryLineSide(object o)
		{
			return ((MainStoryLine)o).MainStoryLineSide;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002EF4 File Offset: 0x000010F4
		internal static object AutoGeneratedGetMemberValueImperialMentorSettlement(object o)
		{
			return ((MainStoryLine)o).ImperialMentorSettlement;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F01 File Offset: 0x00001101
		internal static object AutoGeneratedGetMemberValueAntiImperialMentorSettlement(object o)
		{
			return ((MainStoryLine)o).AntiImperialMentorSettlement;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002F0E File Offset: 0x0000110E
		internal static object AutoGeneratedGetMemberValueFamilyRescued(object o)
		{
			return ((MainStoryLine)o).FamilyRescued;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F20 File Offset: 0x00001120
		internal static object AutoGeneratedGetMemberValue_tutorialScores(object o)
		{
			return ((MainStoryLine)o)._tutorialScores;
		}

		// Token: 0x0400001B RID: 27
		public const int MainStoryLineDialogOptionPriority = 150;

		// Token: 0x0400001C RID: 28
		public const string DragonBannerItemStringId = "dragon_banner";

		// Token: 0x0400001D RID: 29
		public const string DragonBannerPart1ItemStringId = "dragon_banner_center";

		// Token: 0x0400001E RID: 30
		public const string DragonBannerPart2ItemStringId = "dragon_banner_dragonhead";

		// Token: 0x0400001F RID: 31
		public const string DragonBannerPart3ItemStringId = "dragon_banner_handle";

		// Token: 0x04000020 RID: 32
		[SaveableField(1)]
		public MainStoryLineSide MainStoryLineSide;

		// Token: 0x04000025 RID: 37
		[SaveableField(6)]
		public Settlement ImperialMentorSettlement;

		// Token: 0x04000026 RID: 38
		[SaveableField(7)]
		public Settlement AntiImperialMentorSettlement;

		// Token: 0x04000028 RID: 40
		[SaveableField(9)]
		private Dictionary<string, float> _tutorialScores;

		// Token: 0x04000029 RID: 41
		[SaveableField(10)]
		public bool FamilyRescued;
	}
}
