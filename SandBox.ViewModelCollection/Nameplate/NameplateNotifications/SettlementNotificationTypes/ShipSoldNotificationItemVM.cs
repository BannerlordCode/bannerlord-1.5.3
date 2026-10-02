using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000028 RID: 40
	public class ShipSoldNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000F88D File Offset: 0x0000DA8D
		public Ship Ship { get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000F895 File Offset: 0x0000DA95
		public PartyBase SettlementParty { get; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000F89D File Offset: 0x0000DA9D
		public PartyBase HeroParty { get; }

		// Token: 0x06000383 RID: 899 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public ShipSoldNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, Ship ship, PartyBase settlementParty, PartyBase heroParty, int amount, int createdTick)
			: base(onRemove, createdTick)
		{
			this.Ship = ship;
			this.SettlementParty = settlementParty;
			this.HeroParty = heroParty;
			this._amount = amount;
			base.Text = SandBoxUIHelper.GetShipSoldNotificationText(this.Ship, Math.Abs(this._amount), this._amount < 0);
			Hero leaderHero = this.HeroParty.LeaderHero;
			base.CharacterName = ((leaderHero != null) ? leaderHero.Name.ToString() : null) ?? this.HeroParty.Name.ToString();
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.HeroParty);
			if (visualPartyLeader != null)
			{
				base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(visualPartyLeader, false));
			}
			else if (this.HeroParty.Owner != null)
			{
				base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(this.HeroParty.Owner.CharacterObject, false));
			}
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (this.HeroParty.LeaderHero != null)
			{
				base.RelationType = (this.HeroParty.LeaderHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F9D0 File Offset: 0x0000DBD0
		public void AddNewTransaction(int amount)
		{
			this._amount += amount;
			if (this._amount == 0)
			{
				base.ExecuteRemove();
				return;
			}
			base.Text = SandBoxUIHelper.GetShipSoldNotificationText(this.Ship, Math.Abs(this._amount), this._amount < 0);
		}

		// Token: 0x040001CD RID: 461
		private int _amount;
	}
}
