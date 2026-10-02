using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000209 RID: 521
	public class MultiplayerBattleBannerBearersModel : BattleBannerBearersModel
	{
		// Token: 0x06001E6E RID: 7790 RVA: 0x000689E8 File Offset: 0x00066BE8
		public override int GetMinimumFormationTroopCountToBearBanners()
		{
			return int.MaxValue;
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000689EF File Offset: 0x00066BEF
		public override float GetBannerInteractionDistance(Agent interactingAgent)
		{
			return float.MaxValue;
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000689F6 File Offset: 0x00066BF6
		public override bool CanAgentPickUpAnyBanner(Agent agent)
		{
			return false;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000689F9 File Offset: 0x00066BF9
		public override bool CanBannerBearerProvideEffectToFormation(Agent agent, Formation formation)
		{
			return false;
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000689FC File Offset: 0x00066BFC
		public override bool CanAgentBecomeBannerBearer(Agent agent)
		{
			return false;
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000689FF File Offset: 0x00066BFF
		public override int GetAgentBannerBearingPriority(Agent agent)
		{
			return 0;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x00068A02 File Offset: 0x00066C02
		public override bool CanFormationDeployBannerBearers(Formation formation)
		{
			return false;
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x00068A05 File Offset: 0x00066C05
		public override int GetDesiredNumberOfBannerBearersForFormation(Formation formation)
		{
			return 0;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00068A08 File Offset: 0x00066C08
		public override ItemObject GetBannerBearerReplacementWeapon(BasicCharacterObject agentCharacter)
		{
			return null;
		}
	}
}
