using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000160 RID: 352
	public class DefaultSiegeLordsHallFightModel : SiegeLordsHallFightModel
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0008A513 File Offset: 0x00088713
		public override float AreaLostRatio
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x0008A51A File Offset: 0x0008871A
		public override float AttackerDefenderTroopCountRatio
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0008A521 File Offset: 0x00088721
		public override float DefenderMaxArcherRatio
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x0008A528 File Offset: 0x00088728
		public override int MaxDefenderSideTroopCount
		{
			get
			{
				return 27;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0008A52C File Offset: 0x0008872C
		public override int MaxDefenderArcherCount
		{
			get
			{
				return MathF.Round((float)this.MaxDefenderSideTroopCount * this.DefenderMaxArcherRatio);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x0008A541 File Offset: 0x00088741
		public override int MaxAttackerSideTroopCount
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001B2F RID: 6959 RVA: 0x0008A545 File Offset: 0x00088745
		public override int DefenderTroopNumberForSuccessfulPullBack
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0008A54C File Offset: 0x0008874C
		public override FlattenedTroopRoster GetPriorityListForLordsHallFightMission(MapEvent playerMapEvent, BattleSideEnum side, int troopCount)
		{
			List<MapEventParty> list = (from x in playerMapEvent.PartiesOnSide(side)
				where x.Party.IsMobile
				select x).ToList<MapEventParty>();
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(list.Sum<MapEventParty>((MapEventParty x) => x.Party.MemberRoster.TotalHealthyCount));
			foreach (MapEventParty mapEventParty in list)
			{
				flattenedTroopRoster.Add(mapEventParty.Party.MemberRoster.GetTroopRoster());
			}
			List<FlattenedTroopRosterElement> list2 = flattenedTroopRoster.Where<FlattenedTroopRosterElement>((FlattenedTroopRosterElement x) => !x.Troop.IsHero && x.Troop.IsRanged && !x.IsWounded).ToList<FlattenedTroopRosterElement>();
			list2.Shuffle<FlattenedTroopRosterElement>();
			List<FlattenedTroopRosterElement> list3 = flattenedTroopRoster.Where<FlattenedTroopRosterElement>((FlattenedTroopRosterElement x) => !x.Troop.IsHero && !x.Troop.IsRanged && !x.IsWounded).ToList<FlattenedTroopRosterElement>();
			list3.Shuffle<FlattenedTroopRosterElement>();
			flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => !x.Troop.IsHero || x.IsWounded);
			int num = troopCount - flattenedTroopRoster.Count<FlattenedTroopRosterElement>();
			if (num > 0)
			{
				int count = list2.Count;
				int count2 = list3.Count;
				int num2 = MathF.Min(count, Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderArcherCount);
				int num3 = 0;
				int num4 = 0;
				while (num > 0 && (num3 < num2 || num4 < count2))
				{
					if (num3 < num2)
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement = list2[num3];
						flattenedTroopRoster.Add(flattenedTroopRosterElement.Troop, false, flattenedTroopRosterElement.Xp);
						num--;
					}
					if (num4 < count2 && num > 0)
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement2 = list3[num4];
						flattenedTroopRoster.Add(flattenedTroopRosterElement2.Troop, false, flattenedTroopRosterElement2.Xp);
						num--;
					}
					num3++;
					num4++;
				}
			}
			return flattenedTroopRoster;
		}
	}
}
