using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000469 RID: 1129
	public class TradeAgreementsCampaignBehavior : CampaignBehaviorBase, ITradeAgreementsCampaignBehavior
	{
		// Token: 0x06004911 RID: 18705 RVA: 0x0016DD00 File Offset: 0x0016BF00
		public override void RegisterEvents()
		{
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.WarDeclared));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.SettlementEntered));
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x0016DD54 File Offset: 0x0016BF54
		private void SettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			int num;
			if (party != null && party.IsActive && party.MapFaction != null && party.IsCaravan && settlement.IsTown && party.MapFaction != settlement.MapFaction && settlement.MapFaction.IsKingdomFaction && party.MapFaction.IsKingdomFaction && party.IsPartyTradeActive && !party.IsCurrentlyUsedByAQuest && this.TryGetTradeAgreement((Kingdom)settlement.MapFaction, (Kingdom)party.MapFaction, out num) && !party.IsFleeing())
			{
				Kingdom kingdom = (Kingdom)settlement.MapFaction;
				this._tradeAgreements[num] = this._tradeAgreements[num].AddGainedGoldToKingdom(kingdom, Campaign.Current.Models.TradeAgreementModel.GetProfitPerCaravanVisit(party));
			}
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x0016DE3C File Offset: 0x0016C03C
		public void OnTradeAgreementOfferedToPlayer(Kingdom fromKingdom)
		{
			if (!Clan.PlayerClan.IsUnderMercenaryService)
			{
				KingdomDecision kingdomDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
				{
					TradeAgreementDecision tradeAgreementDecision;
					return (tradeAgreementDecision = s as TradeAgreementDecision) != null && tradeAgreementDecision.TargetKingdom == fromKingdom;
				});
				if (kingdomDecision != null)
				{
					Clan.PlayerClan.Kingdom.RemoveDecision(kingdomDecision);
				}
				Clan.PlayerClan.Kingdom.AddDecision(new TradeAgreementDecision(TradeAgreementDecision.GetProposerClanForPlayerKingdom(fromKingdom), fromKingdom), true);
				return;
			}
			this.AcceptOffer(fromKingdom);
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x0016DEC9 File Offset: 0x0016C0C9
		private void AcceptOffer(Kingdom fromKingdom)
		{
			this.MakeTradeAgreement(fromKingdom, Clan.PlayerClan.Kingdom, Campaign.Current.Models.TradeAgreementModel.GetTradeAgreementDurationInYears(fromKingdom, Clan.PlayerClan.Kingdom));
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x0016DEFC File Offset: 0x0016C0FC
		private void WarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			Kingdom kingdom;
			Kingdom kingdom2;
			if ((kingdom = faction1 as Kingdom) != null && (kingdom2 = faction2 as Kingdom) != null && this.HasTradeAgreement(kingdom, kingdom2))
			{
				this.EndTradeAgreement(kingdom, kingdom2);
				this.ApplyBrokenTradeAgreementPenalty(kingdom, kingdom2, detail);
			}
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x0016DF38 File Offset: 0x0016C138
		private void ApplyBrokenTradeAgreementPenalty(Kingdom kingdom, Kingdom otherKingdom, DeclareWarAction.DeclareWarDetail detail)
		{
			Hero hero = ((detail == DeclareWarAction.DeclareWarDetail.CausedByPlayerHostility) ? Hero.MainHero : kingdom.Leader);
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, otherKingdom.Leader, -50, true);
			if (hero == Hero.MainHero)
			{
				TraitLevelingHelper.OnTradeAgreementBroken();
			}
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x0016DF66 File Offset: 0x0016C166
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			this.EndTradeAgreementsOfKingdom(kingdom);
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x0016DF6F File Offset: 0x0016C16F
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<TradeAgreementsCampaignBehavior.TradeAgreement>>("_tradeAgreements", ref this._tradeAgreements);
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x0016DF84 File Offset: 0x0016C184
		public void MakeTradeAgreement(Kingdom kingdom1, Kingdom kingdom2, CampaignTime duration)
		{
			Debug.Print(string.Format("Trade agreement signed between {0} and {1}", kingdom1.Name, kingdom2.Name), 0, Debug.DebugColor.White, 17592186044416UL);
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement = new TradeAgreementsCampaignBehavior.TradeAgreement(kingdom1, kingdom2, CampaignTime.Now + duration);
			this._tradeAgreements.Add(tradeAgreement);
			CampaignEventDispatcher.Instance.OnTradeAgreementSigned(kingdom1, kingdom2);
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x0016DFE4 File Offset: 0x0016C1E4
		public void EndTradeAgreementsOfKingdom(Kingdom kingdom)
		{
			this._tradeAgreements.RemoveAll((TradeAgreementsCampaignBehavior.TradeAgreement t) => t.Kingdom1 == kingdom || t.Kingdom2 == kingdom);
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x0016E016 File Offset: 0x0016C216
		public void EndTradeAgreement(Kingdom kingdom1, Kingdom kingdom2)
		{
			this.RemoveTradeAgreement(kingdom1, kingdom2);
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x0016E020 File Offset: 0x0016C220
		private bool HasTradeAgreement(Kingdom kingdom1, Kingdom kingdom2)
		{
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			return this.HasTradeAgreement(kingdom1, kingdom2, out tradeAgreement);
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x0016E038 File Offset: 0x0016C238
		public bool HasTradeAgreement(Kingdom kingdom1, Kingdom kingdom2, out TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement)
		{
			bool flag = false;
			tradeAgreement = default(TradeAgreementsCampaignBehavior.TradeAgreement);
			int num;
			if (this.TryGetTradeAgreement(kingdom1, kingdom2, out num))
			{
				tradeAgreement = this._tradeAgreements[num];
				if (tradeAgreement.EndTime.IsPast)
				{
					this.EndTradeAgreement(kingdom1, kingdom2);
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x0016E08C File Offset: 0x0016C28C
		public CampaignTime GetTradeAgreementEndDate(Kingdom kingdom1, Kingdom kingdom2)
		{
			int num;
			if (!this.TryGetTradeAgreement(kingdom1, kingdom2, out num))
			{
				Debug.FailedAssert("Cant find trade agreement", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\TradeAgreementsCampaignBehavior.cs", "GetTradeAgreementEndDate", 236);
				return CampaignTime.Zero;
			}
			return this._tradeAgreements[num].EndTime;
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x0016E0D8 File Offset: 0x0016C2D8
		private bool TryGetTradeAgreement(Kingdom kingdom1, Kingdom kingdom2, out int index)
		{
			index = -1;
			bool flag = false;
			for (int i = 0; i < this._tradeAgreements.Count; i++)
			{
				TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement = this._tradeAgreements[i];
				if ((tradeAgreement.Kingdom1 == kingdom1 && tradeAgreement.Kingdom2 == kingdom2) || (tradeAgreement.Kingdom2 == kingdom1 && tradeAgreement.Kingdom1 == kingdom2))
				{
					flag = true;
					index = i;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x0016E13C File Offset: 0x0016C33C
		private void RemoveTradeAgreement(Kingdom kingdom1, Kingdom kingdom2)
		{
			int num = this._tradeAgreements.Count - 1;
			while (-1 < num)
			{
				TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement = this._tradeAgreements[num];
				if ((tradeAgreement.Kingdom1 == kingdom1 && tradeAgreement.Kingdom2 == kingdom2) || (tradeAgreement.Kingdom2 == kingdom1 && tradeAgreement.Kingdom1 == kingdom2))
				{
					this._tradeAgreements.RemoveAt(num);
					return;
				}
				num--;
			}
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x0016E1A0 File Offset: 0x0016C3A0
		public void OnTradeGoldDistributedInKingdom(Kingdom kingdom1, Kingdom kingdom2, Clan clan, int share)
		{
			int num;
			if (this.TryGetTradeAgreement(kingdom1, kingdom2, out num))
			{
				this._tradeAgreements[num] = this._tradeAgreements[num].OnGoldSharedInKingdom(clan.Kingdom, share);
				return;
			}
			Debug.FailedAssert("cant find agreement", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\TradeAgreementsCampaignBehavior.cs", "OnTradeGoldDistributedInKingdom", 284);
		}

		// Token: 0x04001495 RID: 5269
		private const int BreakingAgreementRelationPenalty = -50;

		// Token: 0x04001496 RID: 5270
		private List<TradeAgreementsCampaignBehavior.TradeAgreement> _tradeAgreements = new List<TradeAgreementsCampaignBehavior.TradeAgreement>();

		// Token: 0x020008A6 RID: 2214
		public class TradeAgreementsCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006BE9 RID: 27625 RVA: 0x001DB666 File Offset: 0x001D9866
			public TradeAgreementsCampaignBehaviorTypeDefiner()
				: base(312260)
			{
			}

			// Token: 0x06006BEA RID: 27626 RVA: 0x001DB673 File Offset: 0x001D9873
			protected override void DefineStructTypes()
			{
				base.AddStructDefinition(typeof(TradeAgreementsCampaignBehavior.TradeAgreement), 1, null);
			}

			// Token: 0x06006BEB RID: 27627 RVA: 0x001DB687 File Offset: 0x001D9887
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<TradeAgreementsCampaignBehavior.TradeAgreement>));
			}
		}

		// Token: 0x020008A7 RID: 2215
		public struct TradeAgreement
		{
			// Token: 0x06006BEC RID: 27628 RVA: 0x001DB699 File Offset: 0x001D9899
			public TradeAgreement(Kingdom kingdom1, Kingdom kingdom2, CampaignTime endTime)
			{
				this = default(TradeAgreementsCampaignBehavior.TradeAgreement);
				this.Kingdom1 = kingdom1;
				this.Kingdom2 = kingdom2;
				this.EndTime = endTime;
				this.Kingdom1GoldGained = 0;
				this.Kingdom2GoldGained = 0;
				this.Kingdom1GoldGainedTotal = 0;
				this.Kingdom2GoldGainedTotal = 0;
			}

			// Token: 0x06006BED RID: 27629 RVA: 0x001DB6D4 File Offset: 0x001D98D4
			public TradeAgreementsCampaignBehavior.TradeAgreement AddGainedGoldToKingdom(Kingdom kingdom, int gold)
			{
				if (kingdom == this.Kingdom1)
				{
					this.Kingdom1GoldGained += gold;
					this.Kingdom1GoldGainedTotal += gold;
				}
				else
				{
					this.Kingdom2GoldGained += gold;
					this.Kingdom2GoldGainedTotal += gold;
				}
				return this;
			}

			// Token: 0x06006BEE RID: 27630 RVA: 0x001DB72A File Offset: 0x001D992A
			public TradeAgreementsCampaignBehavior.TradeAgreement OnGoldSharedInKingdom(Kingdom kingdom, int gold)
			{
				if (kingdom == this.Kingdom1)
				{
					this.Kingdom1GoldGained -= gold;
				}
				else
				{
					this.Kingdom2GoldGained -= gold;
				}
				return this;
			}

			// Token: 0x06006BEF RID: 27631 RVA: 0x001DB75C File Offset: 0x001D995C
			public static void AutoGeneratedStaticCollectObjectsTradeAgreement(object o, List<object> collectedObjects)
			{
				((TradeAgreementsCampaignBehavior.TradeAgreement)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006BF0 RID: 27632 RVA: 0x001DB778 File Offset: 0x001D9978
			private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Kingdom1);
				collectedObjects.Add(this.Kingdom2);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.EndTime, collectedObjects);
			}

			// Token: 0x06006BF1 RID: 27633 RVA: 0x001DB7A3 File Offset: 0x001D99A3
			internal static object AutoGeneratedGetMemberValueKingdom1(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).Kingdom1;
			}

			// Token: 0x06006BF2 RID: 27634 RVA: 0x001DB7B0 File Offset: 0x001D99B0
			internal static object AutoGeneratedGetMemberValueKingdom2(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).Kingdom2;
			}

			// Token: 0x06006BF3 RID: 27635 RVA: 0x001DB7BD File Offset: 0x001D99BD
			internal static object AutoGeneratedGetMemberValueEndTime(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).EndTime;
			}

			// Token: 0x06006BF4 RID: 27636 RVA: 0x001DB7CF File Offset: 0x001D99CF
			internal static object AutoGeneratedGetMemberValueKingdom1GoldGained(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).Kingdom1GoldGained;
			}

			// Token: 0x06006BF5 RID: 27637 RVA: 0x001DB7E1 File Offset: 0x001D99E1
			internal static object AutoGeneratedGetMemberValueKingdom2GoldGained(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).Kingdom2GoldGained;
			}

			// Token: 0x06006BF6 RID: 27638 RVA: 0x001DB7F3 File Offset: 0x001D99F3
			internal static object AutoGeneratedGetMemberValueKingdom1GoldGainedTotal(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).Kingdom1GoldGainedTotal;
			}

			// Token: 0x06006BF7 RID: 27639 RVA: 0x001DB805 File Offset: 0x001D9A05
			internal static object AutoGeneratedGetMemberValueKingdom2GoldGainedTotal(object o)
			{
				return ((TradeAgreementsCampaignBehavior.TradeAgreement)o).Kingdom2GoldGainedTotal;
			}

			// Token: 0x040025AE RID: 9646
			[SaveableField(1)]
			public readonly Kingdom Kingdom1;

			// Token: 0x040025AF RID: 9647
			[SaveableField(2)]
			public readonly Kingdom Kingdom2;

			// Token: 0x040025B0 RID: 9648
			[SaveableField(3)]
			public readonly CampaignTime EndTime;

			// Token: 0x040025B1 RID: 9649
			[SaveableField(4)]
			public int Kingdom1GoldGained;

			// Token: 0x040025B2 RID: 9650
			[SaveableField(5)]
			public int Kingdom2GoldGained;

			// Token: 0x040025B3 RID: 9651
			[SaveableField(6)]
			public int Kingdom1GoldGainedTotal;

			// Token: 0x040025B4 RID: 9652
			[SaveableField(7)]
			public int Kingdom2GoldGainedTotal;
		}
	}
}
