using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025E RID: 606
	public sealed class MissionGameModels : GameModelsManager
	{
		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x00079F47 File Offset: 0x00078147
		// (set) Token: 0x06002281 RID: 8833 RVA: 0x00079F4E File Offset: 0x0007814E
		public static MissionGameModels Current { get; private set; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x00079F56 File Offset: 0x00078156
		// (set) Token: 0x06002283 RID: 8835 RVA: 0x00079F5E File Offset: 0x0007815E
		public AgentStatCalculateModel AgentStatCalculateModel { get; private set; }

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x00079F67 File Offset: 0x00078167
		// (set) Token: 0x06002285 RID: 8837 RVA: 0x00079F6F File Offset: 0x0007816F
		public ApplyWeatherEffectsModel ApplyWeatherEffectsModel { get; private set; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00079F78 File Offset: 0x00078178
		// (set) Token: 0x06002287 RID: 8839 RVA: 0x00079F80 File Offset: 0x00078180
		public StrikeMagnitudeCalculationModel StrikeMagnitudeModel { get; private set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002288 RID: 8840 RVA: 0x00079F89 File Offset: 0x00078189
		// (set) Token: 0x06002289 RID: 8841 RVA: 0x00079F91 File Offset: 0x00078191
		public AgentApplyDamageModel AgentApplyDamageModel { get; private set; }

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x0600228A RID: 8842 RVA: 0x00079F9A File Offset: 0x0007819A
		// (set) Token: 0x0600228B RID: 8843 RVA: 0x00079FA2 File Offset: 0x000781A2
		public AgentDecideKilledOrUnconsciousModel AgentDecideKilledOrUnconsciousModel { get; private set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x00079FAB File Offset: 0x000781AB
		// (set) Token: 0x0600228D RID: 8845 RVA: 0x00079FB3 File Offset: 0x000781B3
		public MissionDifficultyModel MissionDifficultyModel { get; private set; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x0600228E RID: 8846 RVA: 0x00079FBC File Offset: 0x000781BC
		// (set) Token: 0x0600228F RID: 8847 RVA: 0x00079FC4 File Offset: 0x000781C4
		public BattleMoraleModel BattleMoraleModel { get; private set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x00079FCD File Offset: 0x000781CD
		// (set) Token: 0x06002291 RID: 8849 RVA: 0x00079FD5 File Offset: 0x000781D5
		public BattleInitializationModel BattleInitializationModel { get; private set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x00079FDE File Offset: 0x000781DE
		// (set) Token: 0x06002293 RID: 8851 RVA: 0x00079FE6 File Offset: 0x000781E6
		public BattleSpawnModel BattleSpawnModel { get; private set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x00079FEF File Offset: 0x000781EF
		// (set) Token: 0x06002295 RID: 8853 RVA: 0x00079FF7 File Offset: 0x000781F7
		public BattleBannerBearersModel BattleBannerBearersModel { get; private set; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x0007A000 File Offset: 0x00078200
		// (set) Token: 0x06002297 RID: 8855 RVA: 0x0007A008 File Offset: 0x00078208
		public FormationArrangementModel FormationArrangementsModel { get; private set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x0007A011 File Offset: 0x00078211
		// (set) Token: 0x06002299 RID: 8857 RVA: 0x0007A019 File Offset: 0x00078219
		public AutoBlockModel AutoBlockModel { get; private set; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x0007A022 File Offset: 0x00078222
		// (set) Token: 0x0600229B RID: 8859 RVA: 0x0007A02A File Offset: 0x0007822A
		public DamageParticleModel DamageParticleModel { get; private set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x0007A033 File Offset: 0x00078233
		// (set) Token: 0x0600229D RID: 8861 RVA: 0x0007A03B File Offset: 0x0007823B
		public ItemPickupModel ItemPickupModel { get; private set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x0007A044 File Offset: 0x00078244
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x0007A04C File Offset: 0x0007824C
		public MissionShipParametersModel MissionShipParametersModel { get; private set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x0007A055 File Offset: 0x00078255
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x0007A05D File Offset: 0x0007825D
		public MissionSiegeEngineCalculationModel MissionSiegeEngineCalculationModel { get; private set; }

		// Token: 0x060022A2 RID: 8866 RVA: 0x0007A068 File Offset: 0x00078268
		private void GetSpecificGameBehaviors()
		{
			this.AgentStatCalculateModel = base.GetGameModel<AgentStatCalculateModel>();
			this.ApplyWeatherEffectsModel = base.GetGameModel<ApplyWeatherEffectsModel>();
			this.StrikeMagnitudeModel = base.GetGameModel<StrikeMagnitudeCalculationModel>();
			this.AgentApplyDamageModel = base.GetGameModel<AgentApplyDamageModel>();
			this.AgentDecideKilledOrUnconsciousModel = base.GetGameModel<AgentDecideKilledOrUnconsciousModel>();
			this.MissionDifficultyModel = base.GetGameModel<MissionDifficultyModel>();
			this.BattleMoraleModel = base.GetGameModel<BattleMoraleModel>();
			this.BattleInitializationModel = base.GetGameModel<BattleInitializationModel>();
			this.BattleSpawnModel = base.GetGameModel<BattleSpawnModel>();
			this.BattleBannerBearersModel = base.GetGameModel<BattleBannerBearersModel>();
			this.FormationArrangementsModel = base.GetGameModel<FormationArrangementModel>();
			this.AutoBlockModel = base.GetGameModel<AutoBlockModel>();
			this.DamageParticleModel = base.GetGameModel<DamageParticleModel>();
			this.ItemPickupModel = base.GetGameModel<ItemPickupModel>();
			this.MissionShipParametersModel = base.GetGameModel<MissionShipParametersModel>();
			this.MissionSiegeEngineCalculationModel = base.GetGameModel<MissionSiegeEngineCalculationModel>();
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0007A135 File Offset: 0x00078335
		private void MakeGameComponentBindings()
		{
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0007A137 File Offset: 0x00078337
		public MissionGameModels(IEnumerable<GameModel> inputComponents)
			: base(inputComponents)
		{
			MissionGameModels.Current = this;
			this.GetSpecificGameBehaviors();
			this.MakeGameComponentBindings();
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0007A152 File Offset: 0x00078352
		public static void Clear()
		{
			MissionGameModels.Current = null;
		}
	}
}
