using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AB RID: 427
	public abstract class CombatXpModel : MBGameModel<CombatXpModel>
	{
		// Token: 0x06001D5B RID: 7515
		public abstract SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit);

		// Token: 0x06001D5C RID: 7516
		public abstract ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType);

		// Token: 0x06001D5D RID: 7517
		public abstract float GetXpMultiplierFromShotDifficulty(float shotDifficulty);

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001D5E RID: 7518
		public abstract float CaptainRadius { get; }

		// Token: 0x0200062C RID: 1580
		public enum MissionTypeEnum
		{
			// Token: 0x04001A27 RID: 6695
			Battle,
			// Token: 0x04001A28 RID: 6696
			PracticeFight,
			// Token: 0x04001A29 RID: 6697
			Tournament,
			// Token: 0x04001A2A RID: 6698
			SimulationBattle,
			// Token: 0x04001A2B RID: 6699
			NoXp
		}
	}
}
