using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000029 RID: 41
	public class TroopGivenToSettlementNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000FA1F File Offset: 0x0000DC1F
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000FA27 File Offset: 0x0000DC27
		public Hero GiverHero { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0000FA30 File Offset: 0x0000DC30
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0000FA38 File Offset: 0x0000DC38
		public TroopRoster Troops { get; private set; }

		// Token: 0x06000389 RID: 905 RVA: 0x0000FA44 File Offset: 0x0000DC44
		public TroopGivenToSettlementNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, Hero giverHero, TroopRoster troops, int createdTick)
			: base(onRemove, createdTick)
		{
			this.GiverHero = giverHero;
			this.Troops = troops;
			base.Text = SandBoxUIHelper.GetTroopGivenToSettlementNotificationText(this.Troops.TotalManCount);
			base.CharacterName = ((this.GiverHero != null) ? this.GiverHero.Name.ToString() : "null hero");
			base.CharacterVisual = ((this.GiverHero != null) ? new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(this.GiverHero.CharacterObject, false)) : new CharacterImageIdentifierVM(null));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (this.GiverHero != null)
			{
				base.RelationType = (this.GiverHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		public void AddNewAction(TroopRoster newTroops)
		{
			this.Troops.Add(newTroops);
			base.Text = SandBoxUIHelper.GetTroopGivenToSettlementNotificationText(this.Troops.TotalManCount);
		}
	}
}
