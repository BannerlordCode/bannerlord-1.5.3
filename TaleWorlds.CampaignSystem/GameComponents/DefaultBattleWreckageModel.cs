using System;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FD RID: 253
	public class DefaultBattleWreckageModel : BattleWreckageModel
	{
		// Token: 0x06001714 RID: 5908 RVA: 0x0006BADF File Offset: 0x00069CDF
		public override bool CanPlayerInteractWithWreckage(out TextObject explanation)
		{
			explanation = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0006BAE9 File Offset: 0x00069CE9
		public override int GetMaxWreckageCountForMapEventType(MapEvent mapEvent)
		{
			return 50;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0006BAED File Offset: 0x00069CED
		public override int GetWreckageCreationBattleSizeThreshold(MapEvent mapEvent)
		{
			return 15;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0006BAF4 File Offset: 0x00069CF4
		public override BattleWreckage.WreckageType GetWreckageTypeForMapEvent(MapEvent mapEvent)
		{
			int num = mapEvent.AttackerSide.Parties.SumQ<MapEventParty>((MapEventParty x) => x.WoundedInBattle.TotalRegulars + x.DiedInBattle.TotalRegulars) + mapEvent.DefenderSide.Parties.SumQ<MapEventParty>((MapEventParty x) => x.WoundedInBattle.TotalRegulars + x.DiedInBattle.TotalRegulars);
			if (num > 150)
			{
				PartyBase leaderParty = mapEvent.AttackerSide.LeaderParty;
				bool flag;
				if (leaderParty == null)
				{
					flag = false;
				}
				else
				{
					MobileParty mobileParty = leaderParty.MobileParty;
					bool? flag2 = ((mobileParty != null) ? new bool?(mobileParty.IsLordParty) : null);
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
				}
				bool flag4;
				if (flag)
				{
					PartyBase leaderParty2 = mapEvent.DefenderSide.LeaderParty;
					if (leaderParty2 == null)
					{
						flag4 = false;
					}
					else
					{
						MobileParty mobileParty2 = leaderParty2.MobileParty;
						bool? flag2 = ((mobileParty2 != null) ? new bool?(mobileParty2.IsLordParty) : null);
						bool flag3 = true;
						flag4 = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
				}
				else
				{
					flag4 = false;
				}
				if (flag4)
				{
					return BattleWreckage.WreckageType.Epic;
				}
				return BattleWreckage.WreckageType.Normal;
			}
			else
			{
				if (num > 50)
				{
					return BattleWreckage.WreckageType.Normal;
				}
				if (num >= 15)
				{
					return BattleWreckage.WreckageType.Small;
				}
				Debug.FailedAssert("This case for wreckage should not be possible, check this", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultBattleWreckageModel.cs", "GetWreckageTypeForMapEvent", 64);
				return BattleWreckage.WreckageType.Invalid;
			}
		}

		// Token: 0x040007AE RID: 1966
		private const int MaxLandWreckageCount = 50;

		// Token: 0x040007AF RID: 1967
		private const int SmallWreckageBattleSizeThreshold = 15;

		// Token: 0x040007B0 RID: 1968
		private const int NormalWreckageBattleSizeThreshold = 50;

		// Token: 0x040007B1 RID: 1969
		private const int EpicWreckageBattleSizeThreshold = 150;
	}
}
