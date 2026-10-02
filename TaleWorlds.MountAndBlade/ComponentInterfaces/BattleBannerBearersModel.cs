using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000408 RID: 1032
	public abstract class BattleBannerBearersModel : MBGameModel<BattleBannerBearersModel>
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x000E957B File Offset: 0x000E777B
		protected BannerBearerLogic BannerBearerLogic
		{
			get
			{
				return this._bannerBearerLogic;
			}
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000E9583 File Offset: 0x000E7783
		public void InitializeModel(BannerBearerLogic bannerBearerLogic)
		{
			this._bannerBearerLogic = bannerBearerLogic;
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x000E958C File Offset: 0x000E778C
		public void FinalizeModel()
		{
			this._bannerBearerLogic = null;
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000E9598 File Offset: 0x000E7798
		public bool IsFormationBanner(Formation formation, SpawnedItemEntity item)
		{
			if (formation == null)
			{
				return false;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return bannerBearerLogic != null && bannerBearerLogic.IsFormationBanner(formation, item);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000E95C0 File Offset: 0x000E77C0
		public bool IsBannerSearchingAgent(Agent agent)
		{
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return bannerBearerLogic != null && bannerBearerLogic.IsBannerSearchingAgent(agent);
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x000E95E0 File Offset: 0x000E77E0
		public bool IsInteractableFormationBanner(SpawnedItemEntity item, Agent interactingAgent)
		{
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			Formation formation = ((bannerBearerLogic != null) ? bannerBearerLogic.GetFormationFromBanner(item) : null);
			return formation == null || formation.Captain == interactingAgent || interactingAgent.Formation == formation || (interactingAgent.IsPlayerControlled && interactingAgent.Team == formation.Team);
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000E9632 File Offset: 0x000E7832
		public bool HasFormationBanner(Formation formation)
		{
			if (formation == null)
			{
				return false;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return ((bannerBearerLogic != null) ? bannerBearerLogic.GetFormationBanner(formation) : null) != null;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x000E9650 File Offset: 0x000E7850
		public bool HasBannerOnGround(Formation formation)
		{
			if (formation == null)
			{
				return false;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return bannerBearerLogic != null && bannerBearerLogic.HasBannerOnGround(formation);
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x000E9675 File Offset: 0x000E7875
		public ItemObject GetFormationBanner(Formation formation)
		{
			if (formation == null)
			{
				return null;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			if (bannerBearerLogic == null)
			{
				return null;
			}
			return bannerBearerLogic.GetFormationBanner(formation);
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x000E9690 File Offset: 0x000E7890
		public List<Agent> GetFormationBannerBearers(Formation formation)
		{
			if (formation == null)
			{
				return new List<Agent>();
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			if (bannerBearerLogic != null)
			{
				return bannerBearerLogic.GetFormationBannerBearers(formation);
			}
			return new List<Agent>();
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x000E96BD File Offset: 0x000E78BD
		public BannerComponent GetActiveBanner(Formation formation)
		{
			if (formation == null)
			{
				return null;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			if (bannerBearerLogic == null)
			{
				return null;
			}
			return bannerBearerLogic.GetActiveBanner(formation);
		}

		// Token: 0x0600388C RID: 14476
		public abstract int GetMinimumFormationTroopCountToBearBanners();

		// Token: 0x0600388D RID: 14477
		public abstract float GetBannerInteractionDistance(Agent interactingAgent);

		// Token: 0x0600388E RID: 14478
		public abstract bool CanBannerBearerProvideEffectToFormation(Agent agent, Formation formation);

		// Token: 0x0600388F RID: 14479
		public abstract bool CanAgentPickUpAnyBanner(Agent agent);

		// Token: 0x06003890 RID: 14480
		public abstract bool CanAgentBecomeBannerBearer(Agent agent);

		// Token: 0x06003891 RID: 14481
		public abstract int GetAgentBannerBearingPriority(Agent agent);

		// Token: 0x06003892 RID: 14482
		public abstract bool CanFormationDeployBannerBearers(Formation formation);

		// Token: 0x06003893 RID: 14483
		public abstract int GetDesiredNumberOfBannerBearersForFormation(Formation formation);

		// Token: 0x06003894 RID: 14484
		public abstract ItemObject GetBannerBearerReplacementWeapon(BasicCharacterObject agentCharacter);

		// Token: 0x04001851 RID: 6225
		public const float DefaultDetachmentCostMultiplier = 10f;

		// Token: 0x04001852 RID: 6226
		private BannerBearerLogic _bannerBearerLogic;
	}
}
