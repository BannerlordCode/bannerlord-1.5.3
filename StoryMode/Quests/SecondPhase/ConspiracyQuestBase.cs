using System;
using System.Collections.Generic;
using System.Linq;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000028 RID: 40
	public abstract class ConspiracyQuestBase : QuestBase
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000205 RID: 517
		public abstract TextObject SideNotificationText { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000206 RID: 518
		public abstract TextObject StartMessageLogFromMentor { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000207 RID: 519
		public abstract TextObject StartLog { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000208 RID: 520
		public abstract float ConspiracyStrengthDecreaseAmount { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000B9E6 File Offset: 0x00009BE6
		public Hero Mentor
		{
			get
			{
				if (!StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
				{
					return StoryModeHeroes.AntiImperialMentor;
				}
				return StoryModeHeroes.ImperialMentor;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000BA04 File Offset: 0x00009C04
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000BA07 File Offset: 0x00009C07
		public override string SpecialQuestType
		{
			get
			{
				return "MainStoryline";
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000BA0E File Offset: 0x00009C0E
		protected ConspiracyQuestBase(string questId, Hero questGiver)
			: base(questId, questGiver, CampaignTime.DaysFromNow(21f), 0)
		{
			base.ChangeQuestDueTime(CampaignTime.DaysFromNow(21f));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000BA33 File Offset: 0x00009C33
		protected override void RegisterEvents()
		{
			StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000BA4C File Offset: 0x00009C4C
		private void OnConspiracyActivated()
		{
			base.CompleteQuestWithFail(null);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000BA55 File Offset: 0x00009C55
		protected override void OnStartQuest()
		{
			base.OnStartQuest();
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ConspiracyQuestMapNotification(this, this.SideNotificationText));
			base.AddLog(this.StartMessageLogFromMentor, false);
			base.AddLog(this.StartLog, false);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000BA94 File Offset: 0x00009C94
		protected override void OnCompleteWithSuccess()
		{
			base.OnCompleteWithSuccess();
			StoryModeManager.Current.MainStoryLine.SecondPhase.DecreaseConspiracyStrength(this.ConspiracyStrengthDecreaseAmount);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000BAB8 File Offset: 0x00009CB8
		protected void DistributeConspiracyRaiderTroopsByLevel(PartyTemplateObject raiderTemplate, PartyBase partyToFill, int troopCountLimit)
		{
			List<KeyValuePair<int, List<CharacterObject>>> list = new List<KeyValuePair<int, List<CharacterObject>>>();
			foreach (PartyTemplateStack partyTemplateStack in raiderTemplate.Stacks.OrderBy<PartyTemplateStack, int>((PartyTemplateStack t) => t.Character.Level))
			{
				int key = partyTemplateStack.Character.Level;
				if (!list.Exists((KeyValuePair<int, List<CharacterObject>> t) => t.Key == key))
				{
					list.Add(new KeyValuePair<int, List<CharacterObject>>(key, new List<CharacterObject>()));
				}
				CharacterObject character = partyTemplateStack.Character;
				KeyValuePair<int, List<CharacterObject>> keyValuePair = list.Find((KeyValuePair<int, List<CharacterObject>> t) => t.Key == key);
				if (keyValuePair.Value != null && !keyValuePair.Value.Contains(character))
				{
					keyValuePair.Value.Add(character);
				}
			}
			int num = list.Sum<KeyValuePair<int, List<CharacterObject>>>((KeyValuePair<int, List<CharacterObject>> t) => t.Key);
			List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
			foreach (KeyValuePair<int, List<CharacterObject>> keyValuePair2 in list)
			{
				list2.Add(new KeyValuePair<int, int>(keyValuePair2.Key, MathF.Floor((float)keyValuePair2.Key / (float)num * (float)troopCountLimit)));
			}
			foreach (PartyTemplateStack partyTemplateStack2 in raiderTemplate.Stacks)
			{
				int level = partyTemplateStack2.Character.Level;
				int num2 = list2.FindIndex((KeyValuePair<int, int> t) => t.Key == level);
				int num3 = list2.Count - 1 - num2;
				int num4 = MathF.Floor((float)list2[num3].Value / (float)list[num2].Value.Count);
				if (num4 != 0)
				{
					partyToFill.MemberRoster.AddToCounts(partyTemplateStack2.Character, num4, false, 0, 0, true, -1);
				}
			}
			if (partyToFill.MemberRoster.TotalManCount < troopCountLimit)
			{
				partyToFill.MemberRoster.AddToCounts(list[0].Value[0], troopCountLimit - partyToFill.MemberRoster.TotalManCount, false, 0, 0, true, -1);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000BD54 File Offset: 0x00009F54
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}
	}
}
