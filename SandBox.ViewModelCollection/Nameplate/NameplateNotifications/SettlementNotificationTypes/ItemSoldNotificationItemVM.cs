using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000025 RID: 37
	public class ItemSoldNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000EF10 File Offset: 0x0000D110
		public ItemRosterElement Item { get; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000EF18 File Offset: 0x0000D118
		public PartyBase ReceiverParty { get; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000EF20 File Offset: 0x0000D120
		public PartyBase PayerParty { get; }

		// Token: 0x06000368 RID: 872 RVA: 0x0000EF28 File Offset: 0x0000D128
		public ItemSoldNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, PartyBase receiverParty, PartyBase payerParty, ItemRosterElement item, int number, int createdTick)
			: base(onRemove, createdTick)
		{
			this.Item = item;
			this.ReceiverParty = receiverParty;
			this.PayerParty = payerParty;
			this._number = number;
			this._heroParty = (receiverParty.IsSettlement ? payerParty : receiverParty);
			base.Text = SandBoxUIHelper.GetItemSoldNotificationText(this.Item, this._number, this._number < 0);
			base.CharacterName = ((this._heroParty.LeaderHero != null) ? this._heroParty.LeaderHero.Name.ToString() : this._heroParty.Name.ToString());
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this._heroParty);
			base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(visualPartyLeader, false));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (this._heroParty.LeaderHero != null)
			{
				base.RelationType = (this._heroParty.LeaderHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000F02D File Offset: 0x0000D22D
		public void AddNewTransaction(int amount)
		{
			this._number += amount;
			if (this._number == 0)
			{
				base.ExecuteRemove();
				return;
			}
			base.Text = SandBoxUIHelper.GetItemSoldNotificationText(this.Item, this._number, this._number < 0);
		}

		// Token: 0x040001C1 RID: 449
		private int _number;

		// Token: 0x040001C2 RID: 450
		private PartyBase _heroParty;
	}
}
