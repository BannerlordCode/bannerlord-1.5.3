using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x0200011C RID: 284
	public class ConversationAggressivePartyItemVM : ViewModel
	{
		// Token: 0x060019A8 RID: 6568 RVA: 0x000619F0 File Offset: 0x0005FBF0
		public ConversationAggressivePartyItemVM(MobileParty party, CharacterObject leader = null)
		{
			this.Party = party;
			if (leader != null)
			{
				this.LeaderVisual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(leader, false));
			}
			else if (party != null)
			{
				CharacterObject visualPartyLeader = CampaignUIHelper.GetVisualPartyLeader(party.Party);
				if (visualPartyLeader != null)
				{
					this.LeaderVisual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(visualPartyLeader, false));
				}
			}
			this.HealthyAmount = ((party != null) ? party.Party.NumberOfHealthyMembers : 0);
			this.RefreshQuests();
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x00061A64 File Offset: 0x0005FC64
		private void RefreshQuests()
		{
			this.Quests = new MBBindingList<QuestMarkerVM>();
			if (this.Party != null)
			{
				List<QuestBase> questsRelatedToParty = CampaignUIHelper.GetQuestsRelatedToParty(this.Party);
				CampaignUIHelper.IssueQuestFlags issueQuestFlags = CampaignUIHelper.IssueQuestFlags.None;
				for (int i = 0; i < questsRelatedToParty.Count; i++)
				{
					issueQuestFlags |= CampaignUIHelper.GetQuestType(questsRelatedToParty[i], this.Party.LeaderHero);
				}
				Hero leaderHero = this.Party.LeaderHero;
				if (((leaderHero != null) ? leaderHero.Issue : null) != null)
				{
					issueQuestFlags |= CampaignUIHelper.GetIssueType(this.Party.LeaderHero.Issue);
				}
				if ((issueQuestFlags & CampaignUIHelper.IssueQuestFlags.TrackedIssue) != CampaignUIHelper.IssueQuestFlags.None)
				{
					this.Quests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.TrackedIssue, null, null));
				}
				else if ((issueQuestFlags & CampaignUIHelper.IssueQuestFlags.ActiveIssue) != CampaignUIHelper.IssueQuestFlags.None)
				{
					this.Quests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.ActiveIssue, null, null));
				}
				else if ((issueQuestFlags & CampaignUIHelper.IssueQuestFlags.AvailableIssue) != CampaignUIHelper.IssueQuestFlags.None)
				{
					this.Quests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.AvailableIssue, null, null));
				}
				if ((issueQuestFlags & CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest) != CampaignUIHelper.IssueQuestFlags.None)
				{
					this.Quests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest, null, null));
					return;
				}
				if ((issueQuestFlags & CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest) != CampaignUIHelper.IssueQuestFlags.None)
				{
					this.Quests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest, null, null));
				}
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00061B70 File Offset: 0x0005FD70
		public void ExecuteShowPartyTooltip()
		{
			if (this.Party != null)
			{
				InformationManager.ShowTooltip(typeof(MobileParty), new object[] { this.Party, true, true });
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00061BAA File Offset: 0x0005FDAA
		public void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x00061BB1 File Offset: 0x0005FDB1
		// (set) Token: 0x060019AD RID: 6573 RVA: 0x00061BB9 File Offset: 0x0005FDB9
		[DataSourceProperty]
		public CharacterImageIdentifierVM LeaderVisual
		{
			get
			{
				return this._leaderVisual;
			}
			set
			{
				if (value != this._leaderVisual)
				{
					this._leaderVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "LeaderVisual");
				}
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x00061BD7 File Offset: 0x0005FDD7
		// (set) Token: 0x060019AF RID: 6575 RVA: 0x00061BDF File Offset: 0x0005FDDF
		[DataSourceProperty]
		public int HealthyAmount
		{
			get
			{
				return this._healthyAmount;
			}
			set
			{
				if (value != this._healthyAmount)
				{
					this._healthyAmount = value;
					base.OnPropertyChangedWithValue(value, "HealthyAmount");
				}
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x00061BFD File Offset: 0x0005FDFD
		// (set) Token: 0x060019B1 RID: 6577 RVA: 0x00061C05 File Offset: 0x0005FE05
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x04000BBE RID: 3006
		public readonly MobileParty Party;

		// Token: 0x04000BBF RID: 3007
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000BC0 RID: 3008
		private CharacterImageIdentifierVM _leaderVisual;

		// Token: 0x04000BC1 RID: 3009
		private int _healthyAmount;
	}
}
