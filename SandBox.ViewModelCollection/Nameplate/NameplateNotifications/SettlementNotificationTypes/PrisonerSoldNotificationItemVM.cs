using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000026 RID: 38
	public class PrisonerSoldNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000F06C File Offset: 0x0000D26C
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000F074 File Offset: 0x0000D274
		public MobileParty Party { get; private set; }

		// Token: 0x0600036C RID: 876 RVA: 0x0000F080 File Offset: 0x0000D280
		public PrisonerSoldNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, MobileParty party, TroopRoster prisoners, int createdTick)
			: base(onRemove, createdTick)
		{
			this._prisonersAmount = prisoners.TotalManCount;
			base.Text = SandBoxUIHelper.GetPrisonersSoldNotificationText(this._prisonersAmount);
			this.Party = party;
			base.CharacterName = ((party.LeaderHero != null) ? party.LeaderHero.Name.ToString() : party.Name.ToString());
			base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(PartyBaseHelper.GetVisualPartyLeader(party.Party), false));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (party.LeaderHero != null)
			{
				base.RelationType = (party.LeaderHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000F13E File Offset: 0x0000D33E
		public void AddNewPrisoners(TroopRoster newPrisoners)
		{
			this._prisonersAmount += newPrisoners.Count;
			base.Text = SandBoxUIHelper.GetPrisonersSoldNotificationText(this._prisonersAmount);
		}

		// Token: 0x040001C4 RID: 452
		private int _prisonersAmount;
	}
}
