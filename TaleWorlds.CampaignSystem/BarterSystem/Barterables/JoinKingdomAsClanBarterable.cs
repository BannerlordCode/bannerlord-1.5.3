using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x020004A7 RID: 1191
	public class JoinKingdomAsClanBarterable : Barterable
	{
		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x0018246A File Offset: 0x0018066A
		public override string StringID
		{
			get
			{
				return "join_faction_barterable";
			}
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x00182471 File Offset: 0x00180671
		public JoinKingdomAsClanBarterable(Hero owner, Kingdom targetKingdom, bool isDefecting = false)
			: base(owner, null)
		{
			this.TargetKingdom = targetKingdom;
			this.IsDefecting = isDefecting;
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x00182489 File Offset: 0x00180689
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=8Az4q2wp}Join {FACTION}", null);
				textObject.SetTextVariable("FACTION", this.TargetKingdom.Name);
				return textObject;
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x001824B0 File Offset: 0x001806B0
		public override int GetUnitValueForFaction(IFaction factionForEvaluation)
		{
			float num = -1000000f;
			if (factionForEvaluation == base.OriginalOwner.Clan)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetScoreOfClanToJoinKingdom(base.OriginalOwner.Clan, this.TargetKingdom);
				if (base.OriginalOwner.Clan.Kingdom != null)
				{
					int valueForFaction = new LeaveKingdomAsClanBarterable(base.OriginalOwner, base.OriginalParty).GetValueForFaction(factionForEvaluation);
					if (!this.TargetKingdom.IsAtWarWith(base.OriginalOwner.Clan.Kingdom))
					{
						float num2 = base.OriginalOwner.Clan.CalculateTotalSettlementValueForFaction(base.OriginalOwner.Clan.Kingdom);
						num -= num2 * ((this.TargetKingdom.Leader == Hero.MainHero) ? 0.5f : 1f);
					}
					num += (float)valueForFaction;
				}
			}
			else if (factionForEvaluation.MapFaction == this.TargetKingdom)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetScoreOfKingdomToGetClan(this.TargetKingdom, base.OriginalOwner.Clan);
			}
			if (this.TargetKingdom == Clan.PlayerClan.Kingdom && Hero.MainHero.GetPerkValue(DefaultPerks.Trade.SilverTongue))
			{
				num += num * DefaultPerks.Trade.SilverTongue.PrimaryBonus;
			}
			return (int)num;
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x001825FD File Offset: 0x001807FD
		public override void CheckBarterLink(Barterable linkedBarterable)
		{
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00182600 File Offset: 0x00180800
		public override bool IsCompatible(Barterable barterable)
		{
			LeaveKingdomAsClanBarterable leaveKingdomAsClanBarterable = barterable as LeaveKingdomAsClanBarterable;
			return leaveKingdomAsClanBarterable == null || leaveKingdomAsClanBarterable.OriginalOwner.MapFaction != this.TargetKingdom;
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x0018262F File Offset: 0x0018082F
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new BannerImageIdentifier(this.TargetKingdom.Banner, false);
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x00182642 File Offset: 0x00180842
		public override string GetEncyclopediaLink()
		{
			return this.TargetKingdom.EncyclopediaLink;
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x00182650 File Offset: 0x00180850
		public override void Apply()
		{
			if (this.TargetKingdom != null && this.TargetKingdom != null && this.TargetKingdom.Leader == Hero.MainHero)
			{
				int valueForFaction = base.GetValueForFaction(base.OriginalOwner.Clan);
				int num = ((valueForFaction < 0) ? (20 - valueForFaction / 20000) : 20);
				ChangeRelationAction.ApplyPlayerRelation(base.OriginalOwner.Clan.Leader, num, true, true);
				if (base.OriginalOwner.Clan.MapFaction != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(base.OriginalOwner.Clan.Leader, base.OriginalOwner.Clan.MapFaction.Leader, -100, true);
				}
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker)
			{
				PlayerEncounter.Current.SetPlayerSiegeInterruptedByEnemyDefection();
			}
			bool flag = base.OriginalOwner.Clan.IsMinorFaction && base.OriginalOwner.Clan != Clan.PlayerClan;
			Kingdom kingdom = base.OriginalOwner.Clan.Kingdom;
			if (!this.IsDefecting && base.OriginalOwner.Clan.Kingdom != null)
			{
				if (flag)
				{
					ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(base.OriginalOwner.Clan, true);
				}
				else if (base.OriginalOwner.Clan.Kingdom != null && this.TargetKingdom != null && base.OriginalOwner.Clan.Kingdom.IsAtWarWith(this.TargetKingdom))
				{
					ChangeKingdomAction.ApplyByLeaveWithRebellionAgainstKingdom(base.OriginalOwner.Clan, true);
				}
				else
				{
					ChangeKingdomAction.ApplyByLeaveKingdom(base.OriginalOwner.Clan, true);
				}
			}
			if (flag)
			{
				ChangeKingdomAction.ApplyByJoinFactionAsMercenary(base.OriginalOwner.Clan, this.TargetKingdom, default(CampaignTime), Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(base.OriginalOwner.Clan, this.TargetKingdom, false), true);
				return;
			}
			if (this.IsDefecting)
			{
				ChangeKingdomAction.ApplyByJoinToKingdomByDefection(base.OriginalOwner.Clan, kingdom, this.TargetKingdom, default(CampaignTime), true);
				return;
			}
			ChangeKingdomAction.ApplyByJoinToKingdom(base.OriginalOwner.Clan, this.TargetKingdom, default(CampaignTime), true);
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x0018288E File Offset: 0x00180A8E
		internal static void AutoGeneratedStaticCollectObjectsJoinKingdomAsClanBarterable(object o, List<object> collectedObjects)
		{
			((JoinKingdomAsClanBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x0018289C File Offset: 0x00180A9C
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x04001535 RID: 5429
		public readonly Kingdom TargetKingdom;

		// Token: 0x04001536 RID: 5430
		public readonly bool IsDefecting;
	}
}
