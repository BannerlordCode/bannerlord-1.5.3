using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000147 RID: 327
	public class DefaultPlayerProgressionModel : PlayerProgressionModel
	{
		// Token: 0x06001A31 RID: 6705 RVA: 0x000841C0 File Offset: 0x000823C0
		public override float GetPlayerProgress()
		{
			return MBMath.ClampFloat((float)Clan.PlayerClan.Fiefs.Count * 0.1f + Clan.PlayerClan.CurrentTotalStrength * 0.0008f + Clan.PlayerClan.Renown * 1.5E-05f + (float)Clan.PlayerClan.AliveLords.Count * 0.002f + (float)Clan.PlayerClan.Companions.Count * 0.01f + (float)Clan.PlayerClan.SupporterNotables.Count * 0.001f + (float)Hero.MainHero.OwnedCaravans.Count * 0.01f + (float)PartyBase.MainParty.NumberOfAllMembers * 0.002f + (float)CharacterObject.PlayerCharacter.Level * 0.002f, 0f, 1f);
		}
	}
}
