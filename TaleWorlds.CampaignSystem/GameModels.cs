using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008D RID: 141
	public sealed class GameModels : GameModelsManager
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x000557EE File Offset: 0x000539EE
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x000557F6 File Offset: 0x000539F6
		public MapVisibilityModel MapVisibilityModel { get; private set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x000557FF File Offset: 0x000539FF
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x00055807 File Offset: 0x00053A07
		public InformationRestrictionModel InformationRestrictionModel { get; private set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x00055810 File Offset: 0x00053A10
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x00055818 File Offset: 0x00053A18
		public PartySpeedModel PartySpeedCalculatingModel { get; private set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00055821 File Offset: 0x00053A21
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x00055829 File Offset: 0x00053A29
		public PartyHealingModel PartyHealingModel { get; private set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x00055832 File Offset: 0x00053A32
		// (set) Token: 0x060011A8 RID: 4520 RVA: 0x0005583A File Offset: 0x00053A3A
		public CaravanModel CaravanModel { get; private set; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00055843 File Offset: 0x00053A43
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x0005584B File Offset: 0x00053A4B
		public PartyTrainingModel PartyTrainingModel { get; private set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x00055854 File Offset: 0x00053A54
		// (set) Token: 0x060011AC RID: 4524 RVA: 0x0005585C File Offset: 0x00053A5C
		public BarterModel BarterModel { get; private set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x00055865 File Offset: 0x00053A65
		// (set) Token: 0x060011AE RID: 4526 RVA: 0x0005586D File Offset: 0x00053A6D
		public PersuasionModel PersuasionModel { get; private set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x00055876 File Offset: 0x00053A76
		// (set) Token: 0x060011B0 RID: 4528 RVA: 0x0005587E File Offset: 0x00053A7E
		public DefectionModel DefectionModel { get; private set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00055887 File Offset: 0x00053A87
		// (set) Token: 0x060011B2 RID: 4530 RVA: 0x0005588F File Offset: 0x00053A8F
		public CombatSimulationModel CombatSimulationModel { get; private set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00055898 File Offset: 0x00053A98
		// (set) Token: 0x060011B4 RID: 4532 RVA: 0x000558A0 File Offset: 0x00053AA0
		public CombatXpModel CombatXpModel { get; private set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x000558A9 File Offset: 0x00053AA9
		// (set) Token: 0x060011B6 RID: 4534 RVA: 0x000558B1 File Offset: 0x00053AB1
		public GenericXpModel GenericXpModel { get; private set; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x000558BA File Offset: 0x00053ABA
		// (set) Token: 0x060011B8 RID: 4536 RVA: 0x000558C2 File Offset: 0x00053AC2
		public TradeAgreementModel TradeAgreementModel { get; private set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x000558CB File Offset: 0x00053ACB
		// (set) Token: 0x060011BA RID: 4538 RVA: 0x000558D3 File Offset: 0x00053AD3
		public SmithingModel SmithingModel { get; private set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x000558DC File Offset: 0x00053ADC
		// (set) Token: 0x060011BC RID: 4540 RVA: 0x000558E4 File Offset: 0x00053AE4
		public PartyTradeModel PartyTradeModel { get; private set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x000558ED File Offset: 0x00053AED
		// (set) Token: 0x060011BE RID: 4542 RVA: 0x000558F5 File Offset: 0x00053AF5
		public RansomValueCalculationModel RansomValueCalculationModel { get; private set; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x000558FE File Offset: 0x00053AFE
		// (set) Token: 0x060011C0 RID: 4544 RVA: 0x00055906 File Offset: 0x00053B06
		public RaidModel RaidModel { get; private set; }

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x0005590F File Offset: 0x00053B0F
		// (set) Token: 0x060011C2 RID: 4546 RVA: 0x00055917 File Offset: 0x00053B17
		public MobilePartyFoodConsumptionModel MobilePartyFoodConsumptionModel { get; private set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00055920 File Offset: 0x00053B20
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x00055928 File Offset: 0x00053B28
		public PartyFoodBuyingModel PartyFoodBuyingModel { get; private set; }

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00055931 File Offset: 0x00053B31
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00055939 File Offset: 0x00053B39
		public PartyImpairmentModel PartyImpairmentModel { get; private set; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00055942 File Offset: 0x00053B42
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x0005594A File Offset: 0x00053B4A
		public PartyMoraleModel PartyMoraleModel { get; private set; }

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00055953 File Offset: 0x00053B53
		// (set) Token: 0x060011CA RID: 4554 RVA: 0x0005595B File Offset: 0x00053B5B
		public PartyDesertionModel PartyDesertionModel { get; private set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x00055964 File Offset: 0x00053B64
		// (set) Token: 0x060011CC RID: 4556 RVA: 0x0005596C File Offset: 0x00053B6C
		public PartyTransitionModel PartyTransitionModel { get; private set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00055975 File Offset: 0x00053B75
		// (set) Token: 0x060011CE RID: 4558 RVA: 0x0005597D File Offset: 0x00053B7D
		public DiplomacyModel DiplomacyModel { get; private set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00055986 File Offset: 0x00053B86
		// (set) Token: 0x060011D0 RID: 4560 RVA: 0x0005598E File Offset: 0x00053B8E
		public AllianceModel AllianceModel { get; private set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00055997 File Offset: 0x00053B97
		// (set) Token: 0x060011D2 RID: 4562 RVA: 0x0005599F File Offset: 0x00053B9F
		public MinorFactionsModel MinorFactionsModel { get; private set; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x000559A8 File Offset: 0x00053BA8
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x000559B0 File Offset: 0x00053BB0
		public HideoutModel HideoutModel { get; private set; }

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x000559B9 File Offset: 0x00053BB9
		// (set) Token: 0x060011D6 RID: 4566 RVA: 0x000559C1 File Offset: 0x00053BC1
		public KingdomCreationModel KingdomCreationModel { get; private set; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x000559CA File Offset: 0x00053BCA
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x000559D2 File Offset: 0x00053BD2
		public KingdomDecisionPermissionModel KingdomDecisionPermissionModel { get; private set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x000559DB File Offset: 0x00053BDB
		// (set) Token: 0x060011DA RID: 4570 RVA: 0x000559E3 File Offset: 0x00053BE3
		public EmissaryModel EmissaryModel { get; private set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x000559EC File Offset: 0x00053BEC
		// (set) Token: 0x060011DC RID: 4572 RVA: 0x000559F4 File Offset: 0x00053BF4
		public CharacterDevelopmentModel CharacterDevelopmentModel { get; private set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x000559FD File Offset: 0x00053BFD
		// (set) Token: 0x060011DE RID: 4574 RVA: 0x00055A05 File Offset: 0x00053C05
		public CharacterStatsModel CharacterStatsModel { get; private set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x00055A0E File Offset: 0x00053C0E
		// (set) Token: 0x060011E0 RID: 4576 RVA: 0x00055A16 File Offset: 0x00053C16
		public EncounterModel EncounterModel { get; private set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x00055A1F File Offset: 0x00053C1F
		// (set) Token: 0x060011E2 RID: 4578 RVA: 0x00055A27 File Offset: 0x00053C27
		public SettlementPatrolModel SettlementPatrolModel { get; private set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x00055A30 File Offset: 0x00053C30
		// (set) Token: 0x060011E4 RID: 4580 RVA: 0x00055A38 File Offset: 0x00053C38
		public ItemDiscardModel ItemDiscardModel { get; private set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x00055A41 File Offset: 0x00053C41
		// (set) Token: 0x060011E6 RID: 4582 RVA: 0x00055A49 File Offset: 0x00053C49
		public ValuationModel ValuationModel { get; private set; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00055A52 File Offset: 0x00053C52
		// (set) Token: 0x060011E8 RID: 4584 RVA: 0x00055A5A File Offset: 0x00053C5A
		public PartySizeLimitModel PartySizeLimitModel { get; private set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00055A63 File Offset: 0x00053C63
		// (set) Token: 0x060011EA RID: 4586 RVA: 0x00055A6B File Offset: 0x00053C6B
		public PartyShipLimitModel PartyShipLimitModel { get; private set; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x00055A74 File Offset: 0x00053C74
		// (set) Token: 0x060011EC RID: 4588 RVA: 0x00055A7C File Offset: 0x00053C7C
		public InventoryCapacityModel InventoryCapacityModel { get; private set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00055A85 File Offset: 0x00053C85
		// (set) Token: 0x060011EE RID: 4590 RVA: 0x00055A8D File Offset: 0x00053C8D
		public PartyWageModel PartyWageModel { get; private set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x00055A96 File Offset: 0x00053C96
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x00055A9E File Offset: 0x00053C9E
		public VillageProductionCalculatorModel VillageProductionCalculatorModel { get; private set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x00055AA7 File Offset: 0x00053CA7
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x00055AAF File Offset: 0x00053CAF
		public VolunteerModel VolunteerModel { get; private set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00055AB8 File Offset: 0x00053CB8
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x00055AC0 File Offset: 0x00053CC0
		public RomanceModel RomanceModel { get; private set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00055AC9 File Offset: 0x00053CC9
		// (set) Token: 0x060011F6 RID: 4598 RVA: 0x00055AD1 File Offset: 0x00053CD1
		public MobilePartyAIModel MobilePartyAIModel { get; private set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00055ADA File Offset: 0x00053CDA
		// (set) Token: 0x060011F8 RID: 4600 RVA: 0x00055AE2 File Offset: 0x00053CE2
		public ArmyManagementCalculationModel ArmyManagementCalculationModel { get; private set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060011F9 RID: 4601 RVA: 0x00055AEB File Offset: 0x00053CEB
		// (set) Token: 0x060011FA RID: 4602 RVA: 0x00055AF3 File Offset: 0x00053CF3
		public BanditDensityModel BanditDensityModel { get; private set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x00055AFC File Offset: 0x00053CFC
		// (set) Token: 0x060011FC RID: 4604 RVA: 0x00055B04 File Offset: 0x00053D04
		public EncounterGameMenuModel EncounterGameMenuModel { get; private set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x00055B0D File Offset: 0x00053D0D
		// (set) Token: 0x060011FE RID: 4606 RVA: 0x00055B15 File Offset: 0x00053D15
		public BattleRewardModel BattleRewardModel { get; private set; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x00055B1E File Offset: 0x00053D1E
		// (set) Token: 0x06001200 RID: 4608 RVA: 0x00055B26 File Offset: 0x00053D26
		public MapTrackModel MapTrackModel { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x00055B2F File Offset: 0x00053D2F
		// (set) Token: 0x06001202 RID: 4610 RVA: 0x00055B37 File Offset: 0x00053D37
		public MapDistanceModel MapDistanceModel { get; private set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x00055B40 File Offset: 0x00053D40
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x00055B48 File Offset: 0x00053D48
		public PartyNavigationModel PartyNavigationModel { get; private set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x00055B51 File Offset: 0x00053D51
		// (set) Token: 0x06001206 RID: 4614 RVA: 0x00055B59 File Offset: 0x00053D59
		public MapWeatherModel MapWeatherModel { get; private set; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00055B62 File Offset: 0x00053D62
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x00055B6A File Offset: 0x00053D6A
		public TargetScoreCalculatingModel TargetScoreCalculatingModel { get; private set; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x00055B73 File Offset: 0x00053D73
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x00055B7B File Offset: 0x00053D7B
		public TradeItemPriceFactorModel TradeItemPriceFactorModel { get; private set; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x00055B84 File Offset: 0x00053D84
		// (set) Token: 0x0600120C RID: 4620 RVA: 0x00055B8C File Offset: 0x00053D8C
		public SettlementEconomyModel SettlementEconomyModel { get; private set; }

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x00055B95 File Offset: 0x00053D95
		// (set) Token: 0x0600120E RID: 4622 RVA: 0x00055B9D File Offset: 0x00053D9D
		public SettlementFoodModel SettlementFoodModel { get; private set; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x00055BA6 File Offset: 0x00053DA6
		// (set) Token: 0x06001210 RID: 4624 RVA: 0x00055BAE File Offset: 0x00053DAE
		public SettlementValueModel SettlementValueModel { get; private set; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x00055BB7 File Offset: 0x00053DB7
		// (set) Token: 0x06001212 RID: 4626 RVA: 0x00055BBF File Offset: 0x00053DBF
		public SettlementMilitiaModel SettlementMilitiaModel { get; private set; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x00055BC8 File Offset: 0x00053DC8
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x00055BD0 File Offset: 0x00053DD0
		public SettlementLoyaltyModel SettlementLoyaltyModel { get; private set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x00055BD9 File Offset: 0x00053DD9
		// (set) Token: 0x06001216 RID: 4630 RVA: 0x00055BE1 File Offset: 0x00053DE1
		public SettlementSecurityModel SettlementSecurityModel { get; private set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00055BEA File Offset: 0x00053DEA
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x00055BF2 File Offset: 0x00053DF2
		public SettlementProsperityModel SettlementProsperityModel { get; private set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00055BFB File Offset: 0x00053DFB
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x00055C03 File Offset: 0x00053E03
		public SettlementGarrisonModel SettlementGarrisonModel { get; private set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x00055C0C File Offset: 0x00053E0C
		// (set) Token: 0x0600121C RID: 4636 RVA: 0x00055C14 File Offset: 0x00053E14
		public ClanTierModel ClanTierModel { get; private set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x00055C1D File Offset: 0x00053E1D
		// (set) Token: 0x0600121E RID: 4638 RVA: 0x00055C25 File Offset: 0x00053E25
		public VassalRewardsModel VassalRewardsModel { get; private set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00055C2E File Offset: 0x00053E2E
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x00055C36 File Offset: 0x00053E36
		public ClanPoliticsModel ClanPoliticsModel { get; private set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00055C3F File Offset: 0x00053E3F
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x00055C47 File Offset: 0x00053E47
		public ClanFinanceModel ClanFinanceModel { get; private set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00055C50 File Offset: 0x00053E50
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00055C58 File Offset: 0x00053E58
		public SettlementTaxModel SettlementTaxModel { get; private set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00055C61 File Offset: 0x00053E61
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x00055C69 File Offset: 0x00053E69
		public HeroAgentLocationModel HeroAgentLocationModel { get; private set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x00055C72 File Offset: 0x00053E72
		// (set) Token: 0x06001228 RID: 4648 RVA: 0x00055C7A File Offset: 0x00053E7A
		public HeirSelectionCalculationModel HeirSelectionCalculationModel { get; private set; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x00055C83 File Offset: 0x00053E83
		// (set) Token: 0x0600122A RID: 4650 RVA: 0x00055C8B File Offset: 0x00053E8B
		public HeroDeathProbabilityCalculationModel HeroDeathProbabilityCalculationModel { get; private set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00055C94 File Offset: 0x00053E94
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x00055C9C File Offset: 0x00053E9C
		public BuildingConstructionModel BuildingConstructionModel { get; private set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x00055CA5 File Offset: 0x00053EA5
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x00055CAD File Offset: 0x00053EAD
		public BuildingEffectModel BuildingEffectModel { get; private set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00055CB6 File Offset: 0x00053EB6
		// (set) Token: 0x06001230 RID: 4656 RVA: 0x00055CBE File Offset: 0x00053EBE
		public WallHitPointCalculationModel WallHitPointCalculationModel { get; private set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x00055CC7 File Offset: 0x00053EC7
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x00055CCF File Offset: 0x00053ECF
		public MarriageModel MarriageModel { get; private set; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00055CD8 File Offset: 0x00053ED8
		// (set) Token: 0x06001234 RID: 4660 RVA: 0x00055CE0 File Offset: 0x00053EE0
		public AgeModel AgeModel { get; private set; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x00055CE9 File Offset: 0x00053EE9
		// (set) Token: 0x06001236 RID: 4662 RVA: 0x00055CF1 File Offset: 0x00053EF1
		public PlayerProgressionModel PlayerProgressionModel { get; private set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x00055CFA File Offset: 0x00053EFA
		// (set) Token: 0x06001238 RID: 4664 RVA: 0x00055D02 File Offset: 0x00053F02
		public DailyTroopXpBonusModel DailyTroopXpBonusModel { get; private set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x00055D0B File Offset: 0x00053F0B
		// (set) Token: 0x0600123A RID: 4666 RVA: 0x00055D13 File Offset: 0x00053F13
		public PregnancyModel PregnancyModel { get; private set; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x00055D1C File Offset: 0x00053F1C
		// (set) Token: 0x0600123C RID: 4668 RVA: 0x00055D24 File Offset: 0x00053F24
		public NotablePowerModel NotablePowerModel { get; private set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x00055D2D File Offset: 0x00053F2D
		// (set) Token: 0x0600123E RID: 4670 RVA: 0x00055D35 File Offset: 0x00053F35
		public MilitaryPowerModel MilitaryPowerModel { get; private set; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00055D3E File Offset: 0x00053F3E
		// (set) Token: 0x06001240 RID: 4672 RVA: 0x00055D46 File Offset: 0x00053F46
		public PrisonerDonationModel PrisonerDonationModel { get; private set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00055D4F File Offset: 0x00053F4F
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x00055D57 File Offset: 0x00053F57
		public NotableSpawnModel NotableSpawnModel { get; private set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x00055D60 File Offset: 0x00053F60
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x00055D68 File Offset: 0x00053F68
		public TournamentModel TournamentModel { get; private set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x00055D71 File Offset: 0x00053F71
		// (set) Token: 0x06001246 RID: 4678 RVA: 0x00055D79 File Offset: 0x00053F79
		public CrimeModel CrimeModel { get; private set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x00055D82 File Offset: 0x00053F82
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x00055D8A File Offset: 0x00053F8A
		public DisguiseDetectionModel DisguiseDetectionModel { get; private set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x00055D93 File Offset: 0x00053F93
		// (set) Token: 0x0600124A RID: 4682 RVA: 0x00055D9B File Offset: 0x00053F9B
		public BribeCalculationModel BribeCalculationModel { get; private set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x00055DA4 File Offset: 0x00053FA4
		// (set) Token: 0x0600124C RID: 4684 RVA: 0x00055DAC File Offset: 0x00053FAC
		public TroopSacrificeModel TroopSacrificeModel { get; private set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x00055DB5 File Offset: 0x00053FB5
		// (set) Token: 0x0600124E RID: 4686 RVA: 0x00055DBD File Offset: 0x00053FBD
		public SiegeStrategyActionModel SiegeStrategyActionModel { get; private set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600124F RID: 4687 RVA: 0x00055DC6 File Offset: 0x00053FC6
		// (set) Token: 0x06001250 RID: 4688 RVA: 0x00055DCE File Offset: 0x00053FCE
		public SiegeEventModel SiegeEventModel { get; private set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x00055DD7 File Offset: 0x00053FD7
		// (set) Token: 0x06001252 RID: 4690 RVA: 0x00055DDF File Offset: 0x00053FDF
		public SiegeAftermathModel SiegeAftermathModel { get; private set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001253 RID: 4691 RVA: 0x00055DE8 File Offset: 0x00053FE8
		// (set) Token: 0x06001254 RID: 4692 RVA: 0x00055DF0 File Offset: 0x00053FF0
		public SiegeLordsHallFightModel SiegeLordsHallFightModel { get; private set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x00055DF9 File Offset: 0x00053FF9
		// (set) Token: 0x06001256 RID: 4694 RVA: 0x00055E01 File Offset: 0x00054001
		public CompanionHiringPriceCalculationModel CompanionHiringPriceCalculationModel { get; private set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001257 RID: 4695 RVA: 0x00055E0A File Offset: 0x0005400A
		// (set) Token: 0x06001258 RID: 4696 RVA: 0x00055E12 File Offset: 0x00054012
		public BuildingScoreCalculationModel BuildingScoreCalculationModel { get; private set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x00055E1B File Offset: 0x0005401B
		// (set) Token: 0x0600125A RID: 4698 RVA: 0x00055E23 File Offset: 0x00054023
		public SettlementAccessModel SettlementAccessModel { get; private set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x00055E2C File Offset: 0x0005402C
		// (set) Token: 0x0600125C RID: 4700 RVA: 0x00055E34 File Offset: 0x00054034
		public IssueModel IssueModel { get; private set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x00055E3D File Offset: 0x0005403D
		// (set) Token: 0x0600125E RID: 4702 RVA: 0x00055E45 File Offset: 0x00054045
		public PrisonerRecruitmentCalculationModel PrisonerRecruitmentCalculationModel { get; private set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x00055E4E File Offset: 0x0005404E
		// (set) Token: 0x06001260 RID: 4704 RVA: 0x00055E56 File Offset: 0x00054056
		public PartyTroopUpgradeModel PartyTroopUpgradeModel { get; private set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00055E5F File Offset: 0x0005405F
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x00055E67 File Offset: 0x00054067
		public TavernMercenaryTroopsModel TavernMercenaryTroopsModel { get; private set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x00055E70 File Offset: 0x00054070
		// (set) Token: 0x06001264 RID: 4708 RVA: 0x00055E78 File Offset: 0x00054078
		public WorkshopModel WorkshopModel { get; private set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00055E81 File Offset: 0x00054081
		// (set) Token: 0x06001266 RID: 4710 RVA: 0x00055E89 File Offset: 0x00054089
		public DifficultyModel DifficultyModel { get; private set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x00055E92 File Offset: 0x00054092
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x00055E9A File Offset: 0x0005409A
		public LocationModel LocationModel { get; private set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x00055EA3 File Offset: 0x000540A3
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x00055EAB File Offset: 0x000540AB
		public PrisonBreakModel PrisonBreakModel { get; private set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x00055EB4 File Offset: 0x000540B4
		// (set) Token: 0x0600126C RID: 4716 RVA: 0x00055EBC File Offset: 0x000540BC
		public BattleCaptainModel BattleCaptainModel { get; private set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x00055EC5 File Offset: 0x000540C5
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x00055ECD File Offset: 0x000540CD
		public BannerItemModel BannerItemModel { get; private set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x00055ED6 File Offset: 0x000540D6
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x00055EDE File Offset: 0x000540DE
		public DelayedTeleportationModel DelayedTeleportationModel { get; private set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x00055EE7 File Offset: 0x000540E7
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x00055EEF File Offset: 0x000540EF
		public TroopSupplierProbabilityModel TroopSupplierProbabilityModel { get; private set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x00055EF8 File Offset: 0x000540F8
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x00055F00 File Offset: 0x00054100
		public CutsceneSelectionModel CutsceneSelectionModel { get; private set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00055F09 File Offset: 0x00054109
		// (set) Token: 0x06001276 RID: 4726 RVA: 0x00055F11 File Offset: 0x00054111
		public EquipmentSelectionModel EquipmentSelectionModel { get; private set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x00055F1A File Offset: 0x0005411A
		// (set) Token: 0x06001278 RID: 4728 RVA: 0x00055F22 File Offset: 0x00054122
		public AlleyModel AlleyModel { get; private set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x00055F2B File Offset: 0x0005412B
		// (set) Token: 0x0600127A RID: 4730 RVA: 0x00055F33 File Offset: 0x00054133
		public VoiceOverModel VoiceOverModel { get; private set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00055F3C File Offset: 0x0005413C
		// (set) Token: 0x0600127C RID: 4732 RVA: 0x00055F44 File Offset: 0x00054144
		public CampaignTimeModel CampaignTimeModel { get; private set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00055F4D File Offset: 0x0005414D
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x00055F55 File Offset: 0x00054155
		public VillageTradeModel VillageTradeModel { get; private set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00055F5E File Offset: 0x0005415E
		// (set) Token: 0x06001280 RID: 4736 RVA: 0x00055F66 File Offset: 0x00054166
		public HeroCreationModel HeroCreationModel { get; private set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00055F6F File Offset: 0x0005416F
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x00055F77 File Offset: 0x00054177
		public CampaignShipDamageModel CampaignShipDamageModel { get; private set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00055F80 File Offset: 0x00054180
		// (set) Token: 0x06001284 RID: 4740 RVA: 0x00055F88 File Offset: 0x00054188
		public CampaignShipParametersModel CampaignShipParametersModel { get; private set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001285 RID: 4741 RVA: 0x00055F91 File Offset: 0x00054191
		// (set) Token: 0x06001286 RID: 4742 RVA: 0x00055F99 File Offset: 0x00054199
		public BuildingModel BuildingModel { get; private set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x00055FA2 File Offset: 0x000541A2
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x00055FAA File Offset: 0x000541AA
		public ShipCostModel ShipCostModel { get; private set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x00055FB3 File Offset: 0x000541B3
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x00055FBB File Offset: 0x000541BB
		public ShipStatModel ShipStatModel { get; private set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00055FC4 File Offset: 0x000541C4
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x00055FCC File Offset: 0x000541CC
		public SceneModel SceneModel { get; private set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00055FD5 File Offset: 0x000541D5
		// (set) Token: 0x0600128E RID: 4750 RVA: 0x00055FDD File Offset: 0x000541DD
		public BodyPropertiesModel BodyPropertiesModel { get; private set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600128F RID: 4751 RVA: 0x00055FE6 File Offset: 0x000541E6
		// (set) Token: 0x06001290 RID: 4752 RVA: 0x00055FEE File Offset: 0x000541EE
		public IncidentModel IncidentModel { get; private set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00055FF7 File Offset: 0x000541F7
		// (set) Token: 0x06001292 RID: 4754 RVA: 0x00055FFF File Offset: 0x000541FF
		public FleetManagementModel FleetManagementModel { get; private set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x00056008 File Offset: 0x00054208
		// (set) Token: 0x06001294 RID: 4756 RVA: 0x00056010 File Offset: 0x00054210
		public ClanMemberPartyRoleModel ClanMemberPartyRoleModel { get; private set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x00056019 File Offset: 0x00054219
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x00056021 File Offset: 0x00054221
		public BattleWreckageModel BattleWreckageModel { get; private set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0005602A File Offset: 0x0005422A
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x00056032 File Offset: 0x00054232
		public ShipDistributionModel ShipDistributionModel { get; private set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0005603B File Offset: 0x0005423B
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x00056043 File Offset: 0x00054243
		public FerryModel FerryModel { get; private set; }

		// Token: 0x0600129B RID: 4763 RVA: 0x0005604C File Offset: 0x0005424C
		private void GetSpecificGameBehaviors()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign || Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				this.CharacterDevelopmentModel = base.GetGameModel<CharacterDevelopmentModel>();
				this.CharacterStatsModel = base.GetGameModel<CharacterStatsModel>();
				this.EncounterModel = base.GetGameModel<EncounterModel>();
				this.SettlementPatrolModel = base.GetGameModel<SettlementPatrolModel>();
				this.ItemDiscardModel = base.GetGameModel<ItemDiscardModel>();
				this.ValuationModel = base.GetGameModel<ValuationModel>();
				this.MapVisibilityModel = base.GetGameModel<MapVisibilityModel>();
				this.InformationRestrictionModel = base.GetGameModel<InformationRestrictionModel>();
				this.PartySpeedCalculatingModel = base.GetGameModel<PartySpeedModel>();
				this.PartyHealingModel = base.GetGameModel<PartyHealingModel>();
				this.CaravanModel = base.GetGameModel<CaravanModel>();
				this.PartyTrainingModel = base.GetGameModel<PartyTrainingModel>();
				this.PartyTradeModel = base.GetGameModel<PartyTradeModel>();
				this.RansomValueCalculationModel = base.GetGameModel<RansomValueCalculationModel>();
				this.RaidModel = base.GetGameModel<RaidModel>();
				this.CombatSimulationModel = base.GetGameModel<CombatSimulationModel>();
				this.CombatXpModel = base.GetGameModel<CombatXpModel>();
				this.GenericXpModel = base.GetGameModel<GenericXpModel>();
				this.TradeAgreementModel = base.GetGameModel<TradeAgreementModel>();
				this.SmithingModel = base.GetGameModel<SmithingModel>();
				this.MobilePartyFoodConsumptionModel = base.GetGameModel<MobilePartyFoodConsumptionModel>();
				this.PartyImpairmentModel = base.GetGameModel<PartyImpairmentModel>();
				this.PartyFoodBuyingModel = base.GetGameModel<PartyFoodBuyingModel>();
				this.PartyMoraleModel = base.GetGameModel<PartyMoraleModel>();
				this.PartyDesertionModel = base.GetGameModel<PartyDesertionModel>();
				this.HideoutModel = base.GetGameModel<HideoutModel>();
				this.DiplomacyModel = base.GetGameModel<DiplomacyModel>();
				this.AllianceModel = base.GetGameModel<AllianceModel>();
				this.PartyTransitionModel = base.GetGameModel<PartyTransitionModel>();
				this.MinorFactionsModel = base.GetGameModel<MinorFactionsModel>();
				this.KingdomCreationModel = base.GetGameModel<KingdomCreationModel>();
				this.EmissaryModel = base.GetGameModel<EmissaryModel>();
				this.KingdomDecisionPermissionModel = base.GetGameModel<KingdomDecisionPermissionModel>();
				this.VillageProductionCalculatorModel = base.GetGameModel<VillageProductionCalculatorModel>();
				this.RomanceModel = base.GetGameModel<RomanceModel>();
				this.VolunteerModel = base.GetGameModel<VolunteerModel>();
				this.ArmyManagementCalculationModel = base.GetGameModel<ArmyManagementCalculationModel>();
				this.BanditDensityModel = base.GetGameModel<BanditDensityModel>();
				this.EncounterGameMenuModel = base.GetGameModel<EncounterGameMenuModel>();
				this.BattleRewardModel = base.GetGameModel<BattleRewardModel>();
				this.MapTrackModel = base.GetGameModel<MapTrackModel>();
				this.MapDistanceModel = base.GetGameModel<MapDistanceModel>();
				this.PartyNavigationModel = base.GetGameModel<PartyNavigationModel>();
				this.MapWeatherModel = base.GetGameModel<MapWeatherModel>();
				this.TargetScoreCalculatingModel = base.GetGameModel<TargetScoreCalculatingModel>();
				this.PartySizeLimitModel = base.GetGameModel<PartySizeLimitModel>();
				this.PartyShipLimitModel = base.GetGameModel<PartyShipLimitModel>();
				this.PartyWageModel = base.GetGameModel<PartyWageModel>();
				this.PlayerProgressionModel = base.GetGameModel<PlayerProgressionModel>();
				this.InventoryCapacityModel = base.GetGameModel<InventoryCapacityModel>();
				this.TradeItemPriceFactorModel = base.GetGameModel<TradeItemPriceFactorModel>();
				this.SettlementValueModel = base.GetGameModel<SettlementValueModel>();
				this.SettlementEconomyModel = base.GetGameModel<SettlementEconomyModel>();
				this.SettlementMilitiaModel = base.GetGameModel<SettlementMilitiaModel>();
				this.SettlementFoodModel = base.GetGameModel<SettlementFoodModel>();
				this.SettlementLoyaltyModel = base.GetGameModel<SettlementLoyaltyModel>();
				this.SettlementSecurityModel = base.GetGameModel<SettlementSecurityModel>();
				this.SettlementProsperityModel = base.GetGameModel<SettlementProsperityModel>();
				this.SettlementGarrisonModel = base.GetGameModel<SettlementGarrisonModel>();
				this.SettlementTaxModel = base.GetGameModel<SettlementTaxModel>();
				this.HeroAgentLocationModel = base.GetGameModel<HeroAgentLocationModel>();
				this.BarterModel = base.GetGameModel<BarterModel>();
				this.PersuasionModel = base.GetGameModel<PersuasionModel>();
				this.DefectionModel = base.GetGameModel<DefectionModel>();
				this.ClanTierModel = base.GetGameModel<ClanTierModel>();
				this.VassalRewardsModel = base.GetGameModel<VassalRewardsModel>();
				this.ClanPoliticsModel = base.GetGameModel<ClanPoliticsModel>();
				this.ClanFinanceModel = base.GetGameModel<ClanFinanceModel>();
				this.HeirSelectionCalculationModel = base.GetGameModel<HeirSelectionCalculationModel>();
				this.HeroDeathProbabilityCalculationModel = base.GetGameModel<HeroDeathProbabilityCalculationModel>();
				this.BuildingConstructionModel = base.GetGameModel<BuildingConstructionModel>();
				this.BuildingEffectModel = base.GetGameModel<BuildingEffectModel>();
				this.WallHitPointCalculationModel = base.GetGameModel<WallHitPointCalculationModel>();
				this.MarriageModel = base.GetGameModel<MarriageModel>();
				this.AgeModel = base.GetGameModel<AgeModel>();
				this.DailyTroopXpBonusModel = base.GetGameModel<DailyTroopXpBonusModel>();
				this.PregnancyModel = base.GetGameModel<PregnancyModel>();
				this.NotablePowerModel = base.GetGameModel<NotablePowerModel>();
				this.NotableSpawnModel = base.GetGameModel<NotableSpawnModel>();
				this.TournamentModel = base.GetGameModel<TournamentModel>();
				this.SiegeStrategyActionModel = base.GetGameModel<SiegeStrategyActionModel>();
				this.SiegeEventModel = base.GetGameModel<SiegeEventModel>();
				this.SiegeAftermathModel = base.GetGameModel<SiegeAftermathModel>();
				this.SiegeLordsHallFightModel = base.GetGameModel<SiegeLordsHallFightModel>();
				this.CrimeModel = base.GetGameModel<CrimeModel>();
				this.DisguiseDetectionModel = base.GetGameModel<DisguiseDetectionModel>();
				this.BribeCalculationModel = base.GetGameModel<BribeCalculationModel>();
				this.CompanionHiringPriceCalculationModel = base.GetGameModel<CompanionHiringPriceCalculationModel>();
				this.TroopSacrificeModel = base.GetGameModel<TroopSacrificeModel>();
				this.BuildingScoreCalculationModel = base.GetGameModel<BuildingScoreCalculationModel>();
				this.SettlementAccessModel = base.GetGameModel<SettlementAccessModel>();
				this.IssueModel = base.GetGameModel<IssueModel>();
				this.PrisonerRecruitmentCalculationModel = base.GetGameModel<PrisonerRecruitmentCalculationModel>();
				this.PartyTroopUpgradeModel = base.GetGameModel<PartyTroopUpgradeModel>();
				this.TavernMercenaryTroopsModel = base.GetGameModel<TavernMercenaryTroopsModel>();
				this.WorkshopModel = base.GetGameModel<WorkshopModel>();
				this.DifficultyModel = base.GetGameModel<DifficultyModel>();
				this.LocationModel = base.GetGameModel<LocationModel>();
				this.MilitaryPowerModel = base.GetGameModel<MilitaryPowerModel>();
				this.PrisonerDonationModel = base.GetGameModel<PrisonerDonationModel>();
				this.PrisonBreakModel = base.GetGameModel<PrisonBreakModel>();
				this.BattleCaptainModel = base.GetGameModel<BattleCaptainModel>();
				this.BannerItemModel = base.GetGameModel<BannerItemModel>();
				this.DelayedTeleportationModel = base.GetGameModel<DelayedTeleportationModel>();
				this.TroopSupplierProbabilityModel = base.GetGameModel<TroopSupplierProbabilityModel>();
				this.CutsceneSelectionModel = base.GetGameModel<CutsceneSelectionModel>();
				this.EquipmentSelectionModel = base.GetGameModel<EquipmentSelectionModel>();
				this.AlleyModel = base.GetGameModel<AlleyModel>();
				this.VoiceOverModel = base.GetGameModel<VoiceOverModel>();
				this.CampaignTimeModel = base.GetGameModel<CampaignTimeModel>();
				this.VillageTradeModel = base.GetGameModel<VillageTradeModel>();
				this.PartyNavigationModel = base.GetGameModel<PartyNavigationModel>();
				this.MobilePartyAIModel = base.GetGameModel<MobilePartyAIModel>();
				this.HeroCreationModel = base.GetGameModel<HeroCreationModel>();
				this.CampaignShipDamageModel = base.GetGameModel<CampaignShipDamageModel>();
				this.CampaignShipParametersModel = base.GetGameModel<CampaignShipParametersModel>();
				this.BuildingModel = base.GetGameModel<BuildingModel>();
				this.ShipCostModel = base.GetGameModel<ShipCostModel>();
				this.SceneModel = base.GetGameModel<SceneModel>();
				this.IncidentModel = base.GetGameModel<IncidentModel>();
				this.BodyPropertiesModel = base.GetGameModel<BodyPropertiesModel>();
				this.FleetManagementModel = base.GetGameModel<FleetManagementModel>();
				this.ShipStatModel = base.GetGameModel<ShipStatModel>();
				this.ClanMemberPartyRoleModel = base.GetGameModel<ClanMemberPartyRoleModel>();
				this.BattleWreckageModel = base.GetGameModel<BattleWreckageModel>();
				this.ShipDistributionModel = base.GetGameModel<ShipDistributionModel>();
				this.FerryModel = base.GetGameModel<FerryModel>();
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0005666A File Offset: 0x0005486A
		public GameModels(IEnumerable<GameModel> inputComponents)
			: base(inputComponents)
		{
			this.GetSpecificGameBehaviors();
		}
	}
}
