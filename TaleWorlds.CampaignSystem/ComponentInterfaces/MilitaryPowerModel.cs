using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EC RID: 492
	public abstract class MilitaryPowerModel : MBGameModel<MilitaryPowerModel>
	{
		// Token: 0x06001F78 RID: 8056
		public abstract float GetTroopPower(CharacterObject troop, BattleSideEnum side, MapEvent.PowerCalculationContext context, float leaderModifier);

		// Token: 0x06001F79 RID: 8057
		public abstract float GetPowerOfParty(PartyBase party, BattleSideEnum side, MapEvent.PowerCalculationContext context);

		// Token: 0x06001F7A RID: 8058
		public abstract float GetContextModifier(CharacterObject troop, BattleSideEnum battleSideEnum, MapEvent.PowerCalculationContext context);

		// Token: 0x06001F7B RID: 8059
		public abstract float GetContextModifier(Ship ship, BattleSideEnum battleSideEnum, MapEvent.PowerCalculationContext context);

		// Token: 0x06001F7C RID: 8060
		public abstract MapEvent.PowerCalculationContext GetContextForPosition(CampaignVec2 position);

		// Token: 0x06001F7D RID: 8061
		public abstract float GetDefaultTroopPower(CharacterObject troop);

		// Token: 0x06001F7E RID: 8062
		public abstract float GetPowerModifierOfHero(Hero leaderHero);
	}
}
