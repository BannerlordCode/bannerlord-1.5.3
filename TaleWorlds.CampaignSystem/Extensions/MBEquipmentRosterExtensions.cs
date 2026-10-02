using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200017A RID: 378
	public static class MBEquipmentRosterExtensions
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x00090592 File Offset: 0x0008E792
		public static MBReadOnlyList<MBEquipmentRoster> All
		{
			get
			{
				return Campaign.Current.AllEquipmentRosters;
			}
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0009059E File Offset: 0x0008E79E
		public static IEnumerable<Equipment> GetCivilianEquipments(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.Where<Equipment>((Equipment x) => x.IsCivilian);
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000905CA File Offset: 0x0008E7CA
		public static IEnumerable<Equipment> GetStealthEquipments(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.Where<Equipment>((Equipment x) => x.IsStealth);
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000905F6 File Offset: 0x0008E7F6
		public static IEnumerable<Equipment> GetBattleEquipments(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.Where<Equipment>((Equipment x) => x.IsBattle);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x00090622 File Offset: 0x0008E822
		public static Equipment GetRandomCivilianEquipment(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.GetRandomElementWithPredicate<Equipment>((Equipment x) => x.IsCivilian);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0009064E File Offset: 0x0008E84E
		public static Equipment GetRandomStealthEquipment(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.GetRandomElementWithPredicate<Equipment>((Equipment x) => x.IsStealth);
		}
	}
}
