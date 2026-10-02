using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x0200002A RID: 42
	public class TroopRecruitmentNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000FB30 File Offset: 0x0000DD30
		// (set) Token: 0x0600038C RID: 908 RVA: 0x0000FB38 File Offset: 0x0000DD38
		public Hero RecruiterHero { get; private set; }

		// Token: 0x0600038D RID: 909 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public TroopRecruitmentNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, Hero recruiterHero, int amount, int createdTick)
			: base(onRemove, createdTick)
		{
			base.Text = SandBoxUIHelper.GetRecruitNotificationText(amount);
			this._recruitAmount = amount;
			this.RecruiterHero = recruiterHero;
			base.CharacterName = ((recruiterHero != null) ? recruiterHero.Name.ToString() : "null hero");
			base.CharacterVisual = ((recruiterHero != null) ? new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(recruiterHero.CharacterObject, false)) : new CharacterImageIdentifierVM(null));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (recruiterHero != null)
			{
				base.RelationType = (recruiterHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000FBE4 File Offset: 0x0000DDE4
		public void AddNewAction(int addedAmount)
		{
			this._recruitAmount += addedAmount;
			base.Text = SandBoxUIHelper.GetRecruitNotificationText(this._recruitAmount);
		}

		// Token: 0x040001D0 RID: 464
		private int _recruitAmount;
	}
}
