using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D1 RID: 465
	public abstract class CrimeModel : MBGameModel<CrimeModel>
	{
		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001EB2 RID: 7858
		public abstract float DeclareWarCrimeRatingThreshold { get; }

		// Token: 0x06001EB3 RID: 7859
		public abstract float GetMaxCrimeRating();

		// Token: 0x06001EB4 RID: 7860
		public abstract float GetMinAcceptableCrimeRating(IFaction faction);

		// Token: 0x06001EB5 RID: 7861
		public abstract float GetCrimeRatingAfterPunishment();

		// Token: 0x06001EB6 RID: 7862
		public abstract bool DoesPlayerHaveAnyCrimeRating(IFaction faction);

		// Token: 0x06001EB7 RID: 7863
		public abstract bool IsPlayerCrimeRatingSevere(IFaction faction);

		// Token: 0x06001EB8 RID: 7864
		public abstract bool IsPlayerCrimeRatingModerate(IFaction faction);

		// Token: 0x06001EB9 RID: 7865
		public abstract bool IsPlayerCrimeRatingMild(IFaction faction);

		// Token: 0x06001EBA RID: 7866
		public abstract float GetCost(IFaction faction, CrimeModel.PaymentMethod paymentMethod, float minimumCrimeRating);

		// Token: 0x06001EBB RID: 7867
		public abstract ExplainedNumber GetEffectiveCrimeChange(IFaction faction, float deltaCrimeRating);

		// Token: 0x06001EBC RID: 7868
		public abstract ExplainedNumber GetDailyCrimeRatingChange(IFaction faction, bool includeDescriptions = false);

		// Token: 0x02000630 RID: 1584
		[Flags]
		public enum PaymentMethod : uint
		{
			// Token: 0x04001A3A RID: 6714
			ExMachina = 4096U,
			// Token: 0x04001A3B RID: 6715
			Gold = 1U,
			// Token: 0x04001A3C RID: 6716
			Influence = 2U,
			// Token: 0x04001A3D RID: 6717
			Punishment = 4U,
			// Token: 0x04001A3E RID: 6718
			Execution = 8U
		}
	}
}
