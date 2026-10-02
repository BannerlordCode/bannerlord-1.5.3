using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x020004AB RID: 1195
	public class NoAttackBarterable : Barterable
	{
		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x0018333B File Offset: 0x0018153B
		public override string StringID
		{
			get
			{
				return "no_attack_barterable";
			}
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x00183342 File Offset: 0x00181542
		public NoAttackBarterable(Hero originalOwner, Hero otherHero, PartyBase ownerParty, PartyBase otherParty, CampaignTime duration)
			: base(originalOwner, ownerParty)
		{
			this._otherFaction = otherParty.MapFaction;
			this._duration = duration;
			this._otherHero = otherHero;
			this._otherParty = otherParty;
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x00183370 File Offset: 0x00181570
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=Y3lGJT8H}{PARTY} won't attack {FACTION} for {DURATION} {?DURATION>1}days{?}day{\\?}.", null);
				textObject.SetTextVariable("PARTY", base.OriginalParty.Name);
				textObject.SetTextVariable("FACTION", this._otherFaction.Name);
				textObject.SetTextVariable("DURATION", this._duration.ToDays.ToString());
				return textObject;
			}
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x001833D8 File Offset: 0x001815D8
		public override void Apply()
		{
			if (base.OriginalParty == MobileParty.MainParty.Party)
			{
				if (this._otherFaction.NotAttackableByPlayerUntilTime.IsPast)
				{
					this._otherFaction.NotAttackableByPlayerUntilTime = CampaignTime.Now;
				}
				this._otherFaction.NotAttackableByPlayerUntilTime += this._duration;
			}
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x00183438 File Offset: 0x00181638
		public override int GetUnitValueForFaction(IFaction faction)
		{
			int num = 0;
			float militaryValueOfParty = Campaign.Current.Models.ValuationModel.GetMilitaryValueOfParty(base.OriginalParty.MobileParty);
			if (faction.MapFaction == this._otherFaction.MapFaction && faction.MapFaction.IsAtWarWith(base.OriginalParty.MapFaction))
			{
				num = (int)(militaryValueOfParty * 0.1f);
			}
			else if (faction.MapFaction == base.OriginalParty.MapFaction)
			{
				num = -(int)(militaryValueOfParty * 0.1f);
			}
			return num;
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x001834BB File Offset: 0x001816BB
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}

		// Token: 0x0400153A RID: 5434
		private readonly IFaction _otherFaction;

		// Token: 0x0400153B RID: 5435
		private readonly CampaignTime _duration;

		// Token: 0x0400153C RID: 5436
		private readonly Hero _otherHero;

		// Token: 0x0400153D RID: 5437
		private readonly PartyBase _otherParty;
	}
}
