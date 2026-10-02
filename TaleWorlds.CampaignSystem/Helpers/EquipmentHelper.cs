using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Helpers
{
	// Token: 0x0200000B RID: 11
	public static class EquipmentHelper
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00005C84 File Offset: 0x00003E84
		public static void AssignHeroEquipmentFromEquipment(Hero hero, Equipment equipment)
		{
			Equipment equipment2;
			if (equipment.IsStealth)
			{
				equipment2 = hero.StealthEquipment;
			}
			else if (equipment.IsCivilian)
			{
				equipment2 = hero.CivilianEquipment;
			}
			else
			{
				equipment2 = hero.BattleEquipment;
			}
			for (int i = 0; i < 12; i++)
			{
				equipment2[i] = new EquipmentElement(equipment[i].Item, equipment[i].ItemModifier, null, false);
			}
		}
	}
}
