using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000115 RID: 277
	public class DefaultDelayedTeleportationModel : DelayedTeleportationModel
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x0007183F File Offset: 0x0006FA3F
		public override float MaximumDistanceForDelayAsDays
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x00071846 File Offset: 0x0006FA46
		public override float DefaultTeleportationSpeed
		{
			get
			{
				return 0.24f;
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00071850 File Offset: 0x0006FA50
		public override ExplainedNumber GetTeleportationDelayAsHours(Hero teleportingHero, PartyBase target)
		{
			float num = this.MaximumDistanceForDelayAsDays * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			float num2 = 0f;
			IMapPoint mapPoint = teleportingHero.GetMapPoint();
			if (mapPoint != null)
			{
				MobileParty.NavigationType navigationType = (teleportingHero.Clan.HasNavalNavigationCapability ? MobileParty.NavigationType.All : MobileParty.NavigationType.Default);
				if (target.IsSettlement)
				{
					if (teleportingHero.CurrentSettlement != null && teleportingHero.CurrentSettlement == target.Settlement)
					{
						num2 = 0f;
					}
					else
					{
						float num3;
						num2 = DistanceHelper.FindClosestDistanceFromMapPointToSettlement(mapPoint, target.Settlement, navigationType, out num3);
					}
				}
				else if (target.IsMobile)
				{
					Settlement settlement;
					MobileParty mobileParty;
					if ((settlement = mapPoint as Settlement) != null)
					{
						num2 = DistanceHelper.FindClosestDistanceFromMobilePartyToSettlement(target.MobileParty, settlement, navigationType);
					}
					else if ((mobileParty = mapPoint as MobileParty) != null)
					{
						float num4 = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(target.MobileParty, mobileParty, navigationType);
						if (num4 < num)
						{
							num2 = num4;
						}
					}
				}
			}
			num2 = MathF.Clamp(num2, 0f, num);
			return new ExplainedNumber(num2 * this.DefaultTeleportationSpeed, false, null);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00071936 File Offset: 0x0006FB36
		public override bool CanPerformImmediateTeleport(Hero hero, MobileParty targetMobileParty, Settlement targetSettlement)
		{
			return (targetSettlement != null && !targetSettlement.IsUnderSiege && !targetSettlement.IsUnderRaid) || (targetMobileParty != null && targetMobileParty.MapEvent == null && !targetMobileParty.IsCurrentlyEngagingParty && (!targetMobileParty.IsCurrentlyAtSea || targetMobileParty.CurrentSettlement != null));
		}
	}
}
