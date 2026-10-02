using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x02000498 RID: 1176
	public class BarterData
	{
		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x00181291 File Offset: 0x0017F491
		public IFaction OffererMapFaction
		{
			get
			{
				Hero offererHero = this.OffererHero;
				return ((offererHero != null) ? offererHero.MapFaction : null) ?? this.OffererParty.MapFaction;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x001812B4 File Offset: 0x0017F4B4
		public IFaction OtherMapFaction
		{
			get
			{
				Hero otherHero = this.OtherHero;
				return ((otherHero != null) ? otherHero.MapFaction : null) ?? this.OtherParty.MapFaction;
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x001812D7 File Offset: 0x0017F4D7
		public bool IsAiBarter { get; }

		// Token: 0x06004BA8 RID: 19368 RVA: 0x001812E0 File Offset: 0x0017F4E0
		public BarterData(Hero offerer, Hero other, PartyBase offererParty, PartyBase otherParty, BarterManager.BarterContextInitializer contextInitializer = null, int persuasionCostReduction = 0, bool isAiBarter = false)
		{
			this.OffererParty = offererParty;
			this.OtherParty = otherParty;
			this.OffererHero = offerer;
			this.OtherHero = other;
			this.ContextInitializer = contextInitializer;
			this.PersuasionCostReduction = persuasionCostReduction;
			this._barterables = new List<Barterable>(16);
			this._barterGroups = Campaign.Current.Models.DiplomacyModel.GetBarterGroups().ToList<BarterGroup>();
			this.IsAiBarter = isAiBarter;
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x00181354 File Offset: 0x0017F554
		public void AddBarterable<T>(Barterable barterable, bool isContextDependent = false)
		{
			foreach (BarterGroup barterGroup in this._barterGroups)
			{
				if (barterGroup is T)
				{
					barterable.Initialize(barterGroup, isContextDependent);
					this._barterables.Add(barterable);
					break;
				}
			}
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x001813C0 File Offset: 0x0017F5C0
		public void AddBarterGroup(BarterGroup barterGroup)
		{
			this._barterGroups.Add(barterGroup);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x001813CE File Offset: 0x0017F5CE
		public List<BarterGroup> GetBarterGroups()
		{
			return this._barterGroups;
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x001813D6 File Offset: 0x0017F5D6
		public List<Barterable> GetBarterables()
		{
			return this._barterables;
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x001813E0 File Offset: 0x0017F5E0
		public BarterGroup GetBarterGroup<T>()
		{
			IEnumerable<T> enumerable = this._barterGroups.OfType<T>();
			if (enumerable.IsEmpty<T>())
			{
				return null;
			}
			return enumerable.First<T>() as BarterGroup;
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00181413 File Offset: 0x0017F613
		public List<Barterable> GetOfferedBarterables()
		{
			return (from barterable in this.GetBarterables()
				where barterable.IsOffered
				select barterable).ToList<Barterable>();
		}

		// Token: 0x0400150F RID: 5391
		public readonly Hero OffererHero;

		// Token: 0x04001510 RID: 5392
		public readonly Hero OtherHero;

		// Token: 0x04001511 RID: 5393
		public readonly PartyBase OffererParty;

		// Token: 0x04001512 RID: 5394
		public readonly PartyBase OtherParty;

		// Token: 0x04001513 RID: 5395
		private List<Barterable> _barterables;

		// Token: 0x04001514 RID: 5396
		private List<BarterGroup> _barterGroups;

		// Token: 0x04001515 RID: 5397
		public readonly BarterManager.BarterContextInitializer ContextInitializer;

		// Token: 0x04001516 RID: 5398
		public readonly int PersuasionCostReduction;
	}
}
