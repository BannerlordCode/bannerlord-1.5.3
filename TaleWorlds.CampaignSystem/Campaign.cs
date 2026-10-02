using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Handlers;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000033 RID: 51
	public class Campaign : GameType
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00014C6D File Offset: 0x00012E6D
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00014C74 File Offset: 0x00012E74
		public static float MapDiagonal { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00014C7C File Offset: 0x00012E7C
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00014C83 File Offset: 0x00012E83
		public static float MapDiagonalSquared { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00014C8B File Offset: 0x00012E8B
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00014C92 File Offset: 0x00012E92
		public static Vec2 MapMinimumPosition { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00014C9A File Offset: 0x00012E9A
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00014CA1 File Offset: 0x00012EA1
		public static Vec2 MapMaximumPosition { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00014CA9 File Offset: 0x00012EA9
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00014CB0 File Offset: 0x00012EB0
		public static float MapMaximumHeight { get; private set; }

		// Token: 0x06000211 RID: 529 RVA: 0x00014CB8 File Offset: 0x00012EB8
		public float GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType navigationType)
		{
			return this._averageDistanceBetweenClosestTwoTowns[navigationType];
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00014CC6 File Offset: 0x00012EC6
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00014CCE File Offset: 0x00012ECE
		[CachedData]
		public float AverageWage { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00014CD7 File Offset: 0x00012ED7
		public string NewGameVersion
		{
			get
			{
				return this._newGameVersion;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00014CDF File Offset: 0x00012EDF
		public MBReadOnlyList<string> PreviouslyUsedModules
		{
			get
			{
				return this._previouslyUsedModules;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00014CE7 File Offset: 0x00012EE7
		public MBReadOnlyList<string> UsedGameVersions
		{
			get
			{
				return this._usedGameVersions;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00014CEF File Offset: 0x00012EEF
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00014CF7 File Offset: 0x00012EF7
		[SaveableProperty(83)]
		public bool EnabledCheatsBefore { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00014D00 File Offset: 0x00012F00
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00014D08 File Offset: 0x00012F08
		[SaveableProperty(82)]
		public string PlatformID { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00014D11 File Offset: 0x00012F11
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00014D19 File Offset: 0x00012F19
		internal CampaignEventDispatcher CampaignEventDispatcher { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00014D22 File Offset: 0x00012F22
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00014D2A File Offset: 0x00012F2A
		[SaveableProperty(80)]
		public string UniqueGameId { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00014D33 File Offset: 0x00012F33
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00014D3B File Offset: 0x00012F3B
		public SaveHandler SaveHandler { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00014D44 File Offset: 0x00012F44
		public override bool SupportsSaving
		{
			get
			{
				return this.GameMode == CampaignGameMode.Campaign;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00014D4F File Offset: 0x00012F4F
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00014D57 File Offset: 0x00012F57
		[SaveableProperty(211)]
		public CampaignObjectManager CampaignObjectManager { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00014D60 File Offset: 0x00012F60
		public AdvancedStartOptionsData AdvancedStartData
		{
			get
			{
				CampaignOptions options = this.Options;
				if (options == null)
				{
					return null;
				}
				return options.AdvancedStartOptionsData;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00014D73 File Offset: 0x00012F73
		public override bool IsDevelopment
		{
			get
			{
				return this.GameMode == CampaignGameMode.Tutorial;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00014D7E File Offset: 0x00012F7E
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00014D86 File Offset: 0x00012F86
		[SaveableProperty(3)]
		public bool IsCraftingEnabled { get; set; } = true;

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00014D8F File Offset: 0x00012F8F
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00014D97 File Offset: 0x00012F97
		[SaveableProperty(4)]
		public bool IsBannerEditorEnabled { get; set; } = true;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00014DA0 File Offset: 0x00012FA0
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00014DA8 File Offset: 0x00012FA8
		[SaveableProperty(5)]
		public bool IsFaceGenEnabled { get; set; } = true;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00014DB1 File Offset: 0x00012FB1
		public ICampaignBehaviorManager CampaignBehaviorManager
		{
			get
			{
				return this._campaignBehaviorManager;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00014DB9 File Offset: 0x00012FB9
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00014DC1 File Offset: 0x00012FC1
		[SaveableProperty(8)]
		public QuestManager QuestManager { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00014DCA File Offset: 0x00012FCA
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00014DD2 File Offset: 0x00012FD2
		[SaveableProperty(9)]
		public IssueManager IssueManager { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00014DDB File Offset: 0x00012FDB
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00014DE3 File Offset: 0x00012FE3
		[SaveableProperty(345)]
		public IncidentManager IncidentManager { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00014DEC File Offset: 0x00012FEC
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00014DF4 File Offset: 0x00012FF4
		[SaveableProperty(11)]
		public FactionManager FactionManager { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00014DFD File Offset: 0x00012FFD
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00014E05 File Offset: 0x00013005
		[SaveableProperty(12)]
		public CharacterRelationManager CharacterRelationManager { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00014E0E File Offset: 0x0001300E
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00014E16 File Offset: 0x00013016
		[SaveableProperty(14)]
		public Romance Romance { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00014E1F File Offset: 0x0001301F
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00014E27 File Offset: 0x00013027
		[SaveableProperty(16)]
		public PlayerCaptivity PlayerCaptivity { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00014E30 File Offset: 0x00013030
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00014E38 File Offset: 0x00013038
		[SaveableProperty(17)]
		internal Clan PlayerDefaultFaction { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00014E41 File Offset: 0x00013041
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00014E49 File Offset: 0x00013049
		public CampaignMission.ICampaignMissionManager CampaignMissionManager { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00014E52 File Offset: 0x00013052
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00014E5A File Offset: 0x0001305A
		public ISkillLevelingManager SkillLevelingManager { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00014E63 File Offset: 0x00013063
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00014E6B File Offset: 0x0001306B
		public IMapSceneCreator MapSceneCreator { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00014E74 File Offset: 0x00013074
		public override bool IsInventoryAccessibleAtMission
		{
			get
			{
				return this.GameMode == CampaignGameMode.Tutorial;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00014E7F File Offset: 0x0001307F
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00014E87 File Offset: 0x00013087
		public GameMenuCallbackManager GameMenuCallbackManager { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00014E90 File Offset: 0x00013090
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00014E98 File Offset: 0x00013098
		public VisualCreator VisualCreator { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00014EA1 File Offset: 0x000130A1
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00014EA9 File Offset: 0x000130A9
		[SaveableProperty(28)]
		public MapStateData MapStateData { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00014EB2 File Offset: 0x000130B2
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00014EBA File Offset: 0x000130BA
		public DefaultPerks DefaultPerks { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00014EC3 File Offset: 0x000130C3
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00014ECB File Offset: 0x000130CB
		public DefaultTraits DefaultTraits { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00014ED4 File Offset: 0x000130D4
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00014EDC File Offset: 0x000130DC
		public DefaultPolicies DefaultPolicies { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00014EE5 File Offset: 0x000130E5
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00014EED File Offset: 0x000130ED
		public DefaultBuildingTypes DefaultBuildingTypes { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00014EF6 File Offset: 0x000130F6
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00014EFE File Offset: 0x000130FE
		public DefaultIssueEffects DefaultIssueEffects { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00014F07 File Offset: 0x00013107
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00014F0F File Offset: 0x0001310F
		public DefaultItems DefaultItems { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00014F18 File Offset: 0x00013118
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00014F20 File Offset: 0x00013120
		public DefaultFigureheads DefaultFigureheads { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00014F29 File Offset: 0x00013129
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00014F31 File Offset: 0x00013131
		public DefaultSiegeStrategies DefaultSiegeStrategies { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00014F3A File Offset: 0x0001313A
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00014F42 File Offset: 0x00013142
		internal MBReadOnlyList<PerkObject> AllPerks { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00014F4B File Offset: 0x0001314B
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00014F53 File Offset: 0x00013153
		public DefaultSkillEffects DefaultSkillEffects { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00014F5C File Offset: 0x0001315C
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00014F64 File Offset: 0x00013164
		public DefaultVillageTypes DefaultVillageTypes { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00014F6D File Offset: 0x0001316D
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00014F75 File Offset: 0x00013175
		internal MBReadOnlyList<TraitObject> AllTraits { get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00014F7E File Offset: 0x0001317E
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00014F86 File Offset: 0x00013186
		internal MBReadOnlyList<MBEquipmentRoster> AllEquipmentRosters { get; private set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00014F8F File Offset: 0x0001318F
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00014F97 File Offset: 0x00013197
		public DefaultCulturalFeats DefaultFeats { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00014FA0 File Offset: 0x000131A0
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00014FA8 File Offset: 0x000131A8
		public DefaultPersonalityTraitEffects DefaultPersonalityTraitEffects { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00014FB1 File Offset: 0x000131B1
		// (set) Token: 0x06000269 RID: 617 RVA: 0x00014FB9 File Offset: 0x000131B9
		internal MBReadOnlyList<PolicyObject> AllPolicies { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00014FC2 File Offset: 0x000131C2
		// (set) Token: 0x0600026B RID: 619 RVA: 0x00014FCA File Offset: 0x000131CA
		internal MBReadOnlyList<BuildingType> AllBuildingTypes { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00014FD3 File Offset: 0x000131D3
		// (set) Token: 0x0600026D RID: 621 RVA: 0x00014FDB File Offset: 0x000131DB
		internal MBReadOnlyList<IssueEffect> AllIssueEffects { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00014FE4 File Offset: 0x000131E4
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00014FEC File Offset: 0x000131EC
		internal MBReadOnlyList<SiegeStrategy> AllSiegeStrategies { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00014FF5 File Offset: 0x000131F5
		// (set) Token: 0x06000271 RID: 625 RVA: 0x00014FFD File Offset: 0x000131FD
		internal MBReadOnlyList<VillageType> AllVillageTypes { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00015006 File Offset: 0x00013206
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0001500E File Offset: 0x0001320E
		internal MBReadOnlyList<SkillEffect> AllSkillEffects { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00015017 File Offset: 0x00013217
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0001501F File Offset: 0x0001321F
		internal MBReadOnlyList<FeatObject> AllFeats { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00015028 File Offset: 0x00013228
		// (set) Token: 0x06000277 RID: 631 RVA: 0x00015030 File Offset: 0x00013230
		internal MBReadOnlyList<SkillObject> AllSkills { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00015039 File Offset: 0x00013239
		// (set) Token: 0x06000279 RID: 633 RVA: 0x00015041 File Offset: 0x00013241
		internal MBReadOnlyList<SiegeEngineType> AllSiegeEngineTypes { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0001504A File Offset: 0x0001324A
		// (set) Token: 0x0600027B RID: 635 RVA: 0x00015052 File Offset: 0x00013252
		internal MBReadOnlyList<ItemCategory> AllItemCategories { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0001505B File Offset: 0x0001325B
		// (set) Token: 0x0600027D RID: 637 RVA: 0x00015063 File Offset: 0x00013263
		internal MBReadOnlyList<CharacterAttribute> AllCharacterAttributes { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0001506C File Offset: 0x0001326C
		// (set) Token: 0x0600027F RID: 639 RVA: 0x00015074 File Offset: 0x00013274
		internal MBReadOnlyList<ItemObject> AllItems { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0001507D File Offset: 0x0001327D
		// (set) Token: 0x06000281 RID: 641 RVA: 0x00015085 File Offset: 0x00013285
		public float EstimatedMaximumLordPartySpeedExceptPlayer { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0001508E File Offset: 0x0001328E
		// (set) Token: 0x06000283 RID: 643 RVA: 0x00015096 File Offset: 0x00013296
		public float EstimatedAverageLordPartySpeed { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0001509F File Offset: 0x0001329F
		// (set) Token: 0x06000285 RID: 645 RVA: 0x000150A7 File Offset: 0x000132A7
		public float EstimatedAverageCaravanPartySpeed { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000286 RID: 646 RVA: 0x000150B0 File Offset: 0x000132B0
		// (set) Token: 0x06000287 RID: 647 RVA: 0x000150B8 File Offset: 0x000132B8
		public float EstimatedAverageVillagerPartySpeed { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000288 RID: 648 RVA: 0x000150C1 File Offset: 0x000132C1
		// (set) Token: 0x06000289 RID: 649 RVA: 0x000150C9 File Offset: 0x000132C9
		public float EstimatedAverageBanditPartySpeed { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600028A RID: 650 RVA: 0x000150D2 File Offset: 0x000132D2
		// (set) Token: 0x0600028B RID: 651 RVA: 0x000150DA File Offset: 0x000132DA
		public float EstimatedAverageLordPartyNavalSpeed { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600028C RID: 652 RVA: 0x000150E3 File Offset: 0x000132E3
		// (set) Token: 0x0600028D RID: 653 RVA: 0x000150EB File Offset: 0x000132EB
		public float EstimatedAverageCaravanPartyNavalSpeed { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600028E RID: 654 RVA: 0x000150F4 File Offset: 0x000132F4
		// (set) Token: 0x0600028F RID: 655 RVA: 0x000150FC File Offset: 0x000132FC
		public float EstimatedAverageVillagerPartyNavalSpeed { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00015105 File Offset: 0x00013305
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0001510D File Offset: 0x0001330D
		public float EstimatedAverageBanditPartyNavalSpeed { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00015116 File Offset: 0x00013316
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0001511E File Offset: 0x0001331E
		[SaveableProperty(100)]
		internal MapTimeTracker MapTimeTracker { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00015127 File Offset: 0x00013327
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0001512F File Offset: 0x0001332F
		public bool TimeControlModeLock { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00015138 File Offset: 0x00013338
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00015140 File Offset: 0x00013340
		public CampaignTimeControlMode TimeControlMode
		{
			get
			{
				return this._timeControlMode;
			}
			set
			{
				if (!this.TimeControlModeLock && value != this._timeControlMode)
				{
					this._timeControlMode = value;
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0001515A File Offset: 0x0001335A
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00015162 File Offset: 0x00013362
		public bool IsMapTooltipLongForm { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0001516B File Offset: 0x0001336B
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00015173 File Offset: 0x00013373
		public float SpeedUpMultiplier { get; set; } = 4f;

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0001517C File Offset: 0x0001337C
		public float CampaignDt
		{
			get
			{
				return this._dt;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00015184 File Offset: 0x00013384
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0001518C File Offset: 0x0001338C
		public bool TrueSight { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00015195 File Offset: 0x00013395
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0001519C File Offset: 0x0001339C
		public static Campaign Current { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000151A4 File Offset: 0x000133A4
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x000151AC File Offset: 0x000133AC
		[SaveableProperty(37)]
		public CampaignGameMode GameMode { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000151B5 File Offset: 0x000133B5
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x000151BD File Offset: 0x000133BD
		[SaveableProperty(38)]
		public float PlayerProgress { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x000151C6 File Offset: 0x000133C6
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x000151CE File Offset: 0x000133CE
		public GameMenuManager GameMenuManager { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x000151D7 File Offset: 0x000133D7
		public GameModels Models
		{
			get
			{
				return this._gameModels;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x000151DF File Offset: 0x000133DF
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x000151E7 File Offset: 0x000133E7
		public SandBoxManager SandBoxManager { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002AA RID: 682 RVA: 0x000151F0 File Offset: 0x000133F0
		public Campaign.GameLoadingType CampaignGameLoadingType
		{
			get
			{
				return this._gameLoadingType;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000151F8 File Offset: 0x000133F8
		public Campaign(CampaignGameMode gameMode, AdvancedStartOptionsData startOptions)
		{
			this.GameMode = gameMode;
			this.Options = new CampaignOptions(startOptions);
			this.CampaignObjectManager = new CampaignObjectManager();
			this.CurrentConversationContext = ConversationContext.Default;
			this.QuestManager = new QuestManager();
			this.IssueManager = new IssueManager();
			this.IncidentManager = new IncidentManager();
			this.FactionManager = new FactionManager();
			this.CharacterRelationManager = new CharacterRelationManager();
			this.Romance = new Romance();
			this.PlayerCaptivity = new PlayerCaptivity();
			this.BarterManager = new BarterManager();
			this.GameMenuCallbackManager = new GameMenuCallbackManager();
			this._campaignPeriodicEventManager = new CampaignPeriodicEventManager();
			this._tickData = new CampaignTickCacheDataStore();
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002AC RID: 684 RVA: 0x000152F8 File Offset: 0x000134F8
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00015300 File Offset: 0x00013500
		[SaveableProperty(40)]
		public SiegeEventManager SiegeEventManager { get; internal set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00015309 File Offset: 0x00013509
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00015311 File Offset: 0x00013511
		[SaveableProperty(41)]
		public MapEventManager MapEventManager { get; internal set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0001531A File Offset: 0x0001351A
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00015322 File Offset: 0x00013522
		[SaveableProperty(43)]
		public MapTrackerManager MapTrackerManager { get; internal set; }

		// Token: 0x060002B2 RID: 690 RVA: 0x0001532B File Offset: 0x0001352B
		public void AddCustomManager<T>() where T : ICustomSystemManager, new()
		{
			this._customManagers.Add(new T());
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00015344 File Offset: 0x00013544
		public T GetCustomManager<T>() where T : ICustomSystemManager
		{
			foreach (ICustomSystemManager customSystemManager in this._customManagers)
			{
				if (customSystemManager.GetType() == typeof(T))
				{
					return (T)((object)customSystemManager);
				}
			}
			return default(T);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x000153BC File Offset: 0x000135BC
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x000153C4 File Offset: 0x000135C4
		internal CampaignEvents CampaignEvents { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x000153D0 File Offset: 0x000135D0
		public MenuContext CurrentMenuContext
		{
			get
			{
				GameStateManager gameStateManager = base.CurrentGame.GameStateManager;
				TutorialState tutorialState = gameStateManager.ActiveState as TutorialState;
				if (tutorialState != null)
				{
					return tutorialState.MenuContext;
				}
				MapState mapState = gameStateManager.ActiveState as MapState;
				if (mapState != null)
				{
					return mapState.MenuContext;
				}
				GameState activeState = gameStateManager.ActiveState;
				MapState mapState2;
				if (((activeState != null) ? activeState.Predecessor : null) != null && (mapState2 = gameStateManager.ActiveState.Predecessor as MapState) != null)
				{
					return mapState2.MenuContext;
				}
				return null;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00015445 File Offset: 0x00013645
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0001544D File Offset: 0x0001364D
		internal List<MBCampaignEvent> CustomPeriodicCampaignEvents { get; private set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00015456 File Offset: 0x00013656
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0001545E File Offset: 0x0001365E
		public bool IsMainPartyWaiting
		{
			get
			{
				return this._isMainPartyWaiting;
			}
			private set
			{
				this._isMainPartyWaiting = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00015467 File Offset: 0x00013667
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0001546F File Offset: 0x0001366F
		[SaveableProperty(45)]
		private int _curMapFrame { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00015478 File Offset: 0x00013678
		internal LocatorGrid<Settlement> SettlementLocator
		{
			get
			{
				LocatorGrid<Settlement> locatorGrid;
				if ((locatorGrid = this._settlementLocator) == null)
				{
					locatorGrid = (this._settlementLocator = new LocatorGrid<Settlement>(5f, 32, 32));
				}
				return locatorGrid;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000154A8 File Offset: 0x000136A8
		internal LocatorGrid<MobileParty> MobilePartyLocator
		{
			get
			{
				LocatorGrid<MobileParty> locatorGrid;
				if ((locatorGrid = this._mobilePartyLocator) == null)
				{
					locatorGrid = (this._mobilePartyLocator = new LocatorGrid<MobileParty>(5f, 32, 32));
				}
				return locatorGrid;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002BF RID: 703 RVA: 0x000154D6 File Offset: 0x000136D6
		public IMapScene MapSceneWrapper
		{
			get
			{
				return this._mapSceneWrapper;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x000154DE File Offset: 0x000136DE
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x000154E6 File Offset: 0x000136E6
		[SaveableProperty(54)]
		public PlayerEncounter PlayerEncounter { get; internal set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000154EF File Offset: 0x000136EF
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x000154F7 File Offset: 0x000136F7
		[CachedData]
		internal LocationEncounter LocationEncounter { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00015500 File Offset: 0x00013700
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00015508 File Offset: 0x00013708
		internal NameGenerator NameGenerator { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00015511 File Offset: 0x00013711
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00015519 File Offset: 0x00013719
		[SaveableProperty(58)]
		public BarterManager BarterManager { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00015522 File Offset: 0x00013722
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0001552A File Offset: 0x0001372A
		[SaveableProperty(69)]
		public bool IsMainHeroDisguised { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00015533 File Offset: 0x00013733
		// (set) Token: 0x060002CB RID: 715 RVA: 0x0001553B File Offset: 0x0001373B
		public Equipment DeadBattleEquipment { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00015544 File Offset: 0x00013744
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0001554C File Offset: 0x0001374C
		public Equipment DeadCivilianEquipment { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00015555 File Offset: 0x00013755
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0001555D File Offset: 0x0001375D
		public Equipment DefaultStealthEquipment { get; private set; }

		// Token: 0x060002D0 RID: 720 RVA: 0x00015568 File Offset: 0x00013768
		public void InitializeMainParty()
		{
			this.InitializeSinglePlayerReferences();
			CampaignVec2 campaignVec = NavigationHelper.FindReachablePointAroundPosition(this.Settlements.Find((Settlement x) => x.IsTown).GatePosition, MobileParty.MainParty.NavigationCapability, 20f, 0f, false);
			this.MainParty.InitializeMobilePartyAtPosition(base.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template"), campaignVec);
			LordPartyComponent.ConvertPartyToLordParty(this.MainParty, Hero.MainHero, Hero.MainHero);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000155FC File Offset: 0x000137FC
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this._campaignEntitySystem = new EntitySystem<CampaignEntityComponent>();
			this.PlayerFormationPreferences = this._playerFormationPreferences.GetReadOnlyDictionary<CharacterObject, FormationClass>();
			this.SpeedUpMultiplier = 4f;
			if (this.IncidentManager == null)
			{
				this.IncidentManager = new IncidentManager();
			}
			if (this.UniqueGameId == null && MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.2", 0))
			{
				this.UniqueGameId = "oldSave";
			}
			if (MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.3.0", 0))
			{
				if (this._previouslyUsedModules == null)
				{
					this._previouslyUsedModules = new MBList<string>();
				}
				MBList<string> mblist = new MBList<string>(this._previouslyUsedModules);
				this._previouslyUsedModules.Clear();
				if (mblist.Any<string>())
				{
					this._previouslyUsedModules.Add(string.Join(MBSaveLoad.ModuleCodeSeperator.ToString(), mblist.Select<string, string>((string x) => x + MBSaveLoad.ModuleVersionSeperator.ToString() + ApplicationVersion.Empty.ToString())));
				}
			}
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.3.0", 0))
			{
				this.UnlockedFigureheadsByMainHero = new List<Figurehead>();
				this._customManagers = new List<ICustomSystemManager>();
				this.MapTrackerManager = new MapTrackerManager();
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00015740 File Offset: 0x00013940
		private void InitializeForSavedGame()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				settlement.Party.OnFinishLoadState();
			}
			foreach (MobileParty mobileParty in this.MobileParties.ToList<MobileParty>())
			{
				mobileParty.Party.OnFinishLoadState();
			}
			foreach (Settlement settlement2 in Settlement.All)
			{
				settlement2.OnFinishLoadState();
			}
			foreach (BattleWreckage battleWreckage in this.Wreckages)
			{
				battleWreckage.UpdateVisibility();
			}
			this.GameMenuCallbackManager = new GameMenuCallbackManager();
			this.GameMenuCallbackManager.OnGameLoad();
			this.IssueManager.InitializeForSavedGame();
			this.MinSettlementX = float.MaxValue;
			this.MinSettlementY = float.MaxValue;
			this.MaxSettlementX = float.MinValue;
			this.MaxSettlementY = float.MinValue;
			foreach (Settlement settlement3 in Settlement.All)
			{
				if (settlement3.Position.X < this.MinSettlementX)
				{
					this.MinSettlementX = settlement3.Position.X;
				}
				if (settlement3.Position.Y < this.MinSettlementY)
				{
					this.MinSettlementY = settlement3.Position.Y;
				}
				if (settlement3.Position.X > this.MaxSettlementX)
				{
					this.MaxSettlementX = settlement3.Position.X;
				}
				if (settlement3.Position.Y > this.MaxSettlementY)
				{
					this.MaxSettlementY = settlement3.Position.Y;
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00015998 File Offset: 0x00013B98
		private void OnGameLoaded(CampaignGameStarter starter)
		{
			TroopRoster.CalculateCachedStatsOnLoad();
			this._tickData = new CampaignTickCacheDataStore();
			base.ObjectManager.PreAfterLoad();
			this.CampaignObjectManager.PreAfterLoad();
			this.IssueManager.PreAfterLoad();
			this.QuestManager.PreAfterLoad();
			base.ObjectManager.AfterLoad();
			this.CampaignObjectManager.AfterLoad();
			this.CharacterRelationManager.AfterLoad();
			this.FactionManager.AfterLoad();
			CampaignEventDispatcher.Instance.OnGameEarlyLoaded(starter);
			CampaignEventDispatcher.Instance.OnGameLoaded(starter);
			this.InitializeForSavedGame();
			this._tickData.InitializeDataCache();
			MobileParty mainParty = MobileParty.MainParty;
			this._wasPlayerAnchorMovingToPoint = ((mainParty != null) ? mainParty.Anchor : null) != null && MobileParty.MainParty.Anchor.ArrivalTime != CampaignTime.Zero;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00015A6C File Offset: 0x00013C6C
		private void OnDataLoadFinished(CampaignGameStarter starter)
		{
			this._towns = new MBList<Town>();
			this._castles = new MBList<Town>();
			this._villages = new MBList<Village>();
			this._hideouts = new MBList<Hideout>();
			for (int i = 0; i < Settlement.All.Count; i++)
			{
				Settlement settlement = Settlement.All[i];
				if (settlement.IsTown)
				{
					this._towns.Add(settlement.Town);
				}
				else if (settlement.IsCastle)
				{
					this._castles.Add(settlement.Town);
				}
				else if (settlement.IsVillage)
				{
					this._villages.Add(settlement.Village);
				}
				else if (settlement.IsHideout)
				{
					this._hideouts.Add(settlement.Hideout);
				}
			}
			this._campaignPeriodicEventManager.InitializeTickers();
			this.CreateCampaignEvents();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00015B44 File Offset: 0x00013D44
		private void OnSessionStart(CampaignGameStarter starter)
		{
			CampaignEventDispatcher.Instance.OnSessionStart(starter);
			CampaignEventDispatcher.Instance.OnAfterSessionStart(starter);
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			this.ConversationManager.Build();
			foreach (Settlement settlement in this.Settlements)
			{
				settlement.OnSessionStart();
			}
			this.IsCraftingEnabled = true;
			this.IsBannerEditorEnabled = true;
			this.IsFaceGenEnabled = true;
			this.MapEventManager.OnAfterLoad();
			this.SiegeEventManager.OnAfterLoad();
			this.KingdomManager.RegisterEvents();
			this.KingdomManager.OnSessionStart();
			this.CampaignInformationManager.RegisterEvents();
			this.IncidentManager.RegisterEvents();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00015C24 File Offset: 0x00013E24
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsVillage)
			{
				settlement.Village.DailyTick();
				return;
			}
			if (settlement.Town != null)
			{
				settlement.Town.DailyTick();
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00015C4D File Offset: 0x00013E4D
		internal void HourlyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			CampaignEventDispatcher.Instance.HourlyTick();
			MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
			if (mapState == null)
			{
				return;
			}
			mapState.OnHourlyTick();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00015C77 File Offset: 0x00013E77
		internal void QuarterHourlyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			CampaignEventDispatcher.Instance.QuarterHourlyTick();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00015C84 File Offset: 0x00013E84
		internal void DailyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			this.PlayerProgress = (this.PlayerProgress + this.Models.PlayerProgressionModel.GetPlayerProgress()) / 2f;
			Debug.Print("Before Daily Tick: " + CampaignTime.Now.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
			CampaignEventDispatcher.Instance.DailyTick();
			if ((int)this.Models.CampaignTimeModel.CampaignStartTime.ElapsedDaysUntilNow % CampaignTime.DaysInWeek == 0)
			{
				CampaignEventDispatcher.Instance.WeeklyTick();
				this.OnWeeklyTick();
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00015D1D File Offset: 0x00013F1D
		public void WaitAsyncTasks()
		{
			if (this.CampaignLateAITickTask != null)
			{
				this.CampaignLateAITickTask.Wait();
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00015D32 File Offset: 0x00013F32
		private void OnWeeklyTick()
		{
			this.LogEntryHistory.DeleteOutdatedLogs();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00015D40 File Offset: 0x00013F40
		public CampaignTimeControlMode GetSimplifiedTimeControlMode()
		{
			switch (this.TimeControlMode)
			{
			case CampaignTimeControlMode.Stop:
				return CampaignTimeControlMode.Stop;
			case CampaignTimeControlMode.UnstoppablePlay:
				return CampaignTimeControlMode.UnstoppablePlay;
			case CampaignTimeControlMode.UnstoppableFastForward:
			case CampaignTimeControlMode.UnstoppableFastForwardForPartyWaitTime:
				return CampaignTimeControlMode.UnstoppableFastForward;
			case CampaignTimeControlMode.StoppablePlay:
				if (!this.IsMainPartyWaiting)
				{
					return CampaignTimeControlMode.StoppablePlay;
				}
				return CampaignTimeControlMode.Stop;
			case CampaignTimeControlMode.StoppableFastForward:
				if (!this.IsMainPartyWaiting)
				{
					return CampaignTimeControlMode.StoppableFastForward;
				}
				return CampaignTimeControlMode.Stop;
			default:
				return CampaignTimeControlMode.Stop;
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00015D94 File Offset: 0x00013F94
		private void CheckMainPartyNeedsUpdate()
		{
			MobileParty.MainParty.Ai.CheckPartyNeedsUpdate();
			AnchorPoint anchor = MobileParty.MainParty.Anchor;
			bool flag = anchor != null && anchor.IsMovingToPoint;
			if (this._wasPlayerAnchorMovingToPoint != flag)
			{
				this._wasPlayerAnchorMovingToPoint = flag;
				AnchorPoint anchor2 = MobileParty.MainParty.Anchor;
				if (anchor2 == null)
				{
					return;
				}
				anchor2.TryUpdatePlayerAnchorInfo();
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00015DEC File Offset: 0x00013FEC
		private void TickMapTime(float realDt)
		{
			float num = 0f;
			float speedUpMultiplier = this.SpeedUpMultiplier;
			float num2 = 0.25f * realDt;
			this.IsMainPartyWaiting = MobileParty.MainParty.ComputeIsWaiting();
			switch (this.TimeControlMode)
			{
			case CampaignTimeControlMode.Stop:
			case CampaignTimeControlMode.FastForwardStop:
				break;
			case CampaignTimeControlMode.UnstoppablePlay:
				num = num2;
				break;
			case CampaignTimeControlMode.UnstoppableFastForward:
			case CampaignTimeControlMode.UnstoppableFastForwardForPartyWaitTime:
				num = num2 * speedUpMultiplier;
				break;
			case CampaignTimeControlMode.StoppablePlay:
				if (!this.IsMainPartyWaiting)
				{
					num = num2;
				}
				break;
			case CampaignTimeControlMode.StoppableFastForward:
				if (!this.IsMainPartyWaiting)
				{
					num = num2 * speedUpMultiplier;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this._dt = num;
			this.MapTimeTracker.Tick(4320f * num);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00015E8C File Offset: 0x0001408C
		public void OnGameOver()
		{
			if (CampaignOptions.IsIronmanMode)
			{
				this.SaveHandler.QuickSaveCurrentGame();
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00015EA0 File Offset: 0x000140A0
		internal void RealTick(float realDt)
		{
			this.WaitAsyncTasks();
			this.CheckMainPartyNeedsUpdate();
			this.TickMapTime(realDt);
			foreach (CampaignEntityComponent campaignEntityComponent in this._campaignEntitySystem.GetComponents())
			{
				campaignEntityComponent.OnTick(realDt, this._dt);
			}
			if (!this.GameStarted)
			{
				this.GameStarted = true;
				this._tickData.InitializeDataCache();
				this.SiegeEventManager.Tick(this._dt);
			}
			this._tickData.RealTick(this._dt, realDt);
			this.SiegeEventManager.Tick(this._dt);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00015F60 File Offset: 0x00014160
		public static float CurrentTime
		{
			get
			{
				return (float)CampaignTime.Now.ToHours;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00015F7C File Offset: 0x0001417C
		public void SetTimeSpeed(int speed)
		{
			switch (speed)
			{
			case 0:
				if (this.TimeControlMode == CampaignTimeControlMode.UnstoppableFastForward || this.TimeControlMode == CampaignTimeControlMode.StoppableFastForward)
				{
					this.TimeControlMode = CampaignTimeControlMode.FastForwardStop;
					return;
				}
				if (this.TimeControlMode != CampaignTimeControlMode.FastForwardStop && this.TimeControlMode != CampaignTimeControlMode.Stop)
				{
					this.TimeControlMode = CampaignTimeControlMode.Stop;
					return;
				}
				break;
			case 1:
				if (((this.TimeControlMode == CampaignTimeControlMode.Stop || this.TimeControlMode == CampaignTimeControlMode.FastForwardStop) && this.MainParty.DefaultBehavior == AiBehavior.Hold) || this.IsMainPartyWaiting || (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty))
				{
					this.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
					return;
				}
				this.TimeControlMode = CampaignTimeControlMode.StoppablePlay;
				return;
			case 2:
				if (((this.TimeControlMode == CampaignTimeControlMode.Stop || this.TimeControlMode == CampaignTimeControlMode.FastForwardStop) && this.MainParty.DefaultBehavior == AiBehavior.Hold) || this.IsMainPartyWaiting || (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty))
				{
					this.TimeControlMode = CampaignTimeControlMode.UnstoppableFastForward;
					return;
				}
				this.TimeControlMode = CampaignTimeControlMode.StoppableFastForward;
				break;
			default:
				return;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00016084 File Offset: 0x00014284
		public static void LateAITick()
		{
			Campaign.Current.LateAITickAux();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00016090 File Offset: 0x00014290
		internal void LateAITickAux()
		{
			if (this._dt > 0f || this.CurrentTickCount < 3)
			{
				this.PartiesThink(this._dt);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000160B4 File Offset: 0x000142B4
		internal void Tick()
		{
			int curMapFrame = this._curMapFrame;
			this._curMapFrame = curMapFrame + 1;
			this.CurrentTickCount++;
			if (this._dt > 0f || this.CurrentTickCount < 3)
			{
				CampaignEventDispatcher.Instance.Tick(this._dt);
				this._campaignPeriodicEventManager.OnTick(this._dt);
				this.MapEventManager.Tick();
				this._lastNonZeroDtFrame = this._curMapFrame;
				this._campaignPeriodicEventManager.MobilePartyHourlyTick();
			}
			if (this._dt > 0f)
			{
				this._campaignPeriodicEventManager.TickPeriodicEvents();
			}
			this._tickData.Tick();
			Campaign.Current.PlayerCaptivity.Update(this._dt);
			if (this._dt > 0f || (MobileParty.MainParty.MapEvent == null && this._curMapFrame == this._lastNonZeroDtFrame + 1))
			{
				EncounterManager.Tick(this._dt);
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				if (mapState != null && mapState.AtMenu && !mapState.MenuContext.GameMenu.IsWaitActive)
				{
					this._dt = 0f;
				}
			}
			if (this._dt > 0f || this.CurrentTickCount < 3)
			{
				this._campaignPeriodicEventManager.TickPartialHourlyAi();
			}
			MapState mapState2;
			if ((mapState2 = Game.Current.GameStateManager.ActiveState as MapState) != null && mapState2.NextIncident != null)
			{
				if (mapState2.NextIncident.CanIncidentBeInvoked())
				{
					mapState2.StartIncident(mapState2.NextIncident);
				}
				mapState2.NextIncident = null;
			}
			MapState mapState3;
			if ((mapState3 = Game.Current.GameStateManager.ActiveState as MapState) != null && !mapState3.AtMenu)
			{
				string genericStateMenu = this.Models.EncounterGameMenuModel.GetGenericStateMenu();
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.ActivateGameMenu(genericStateMenu);
				}
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00016284 File Offset: 0x00014484
		private void CreateCampaignEvents()
		{
			long numTicks = (CampaignTime.Now - Campaign.Current.Models.CampaignTimeModel.CampaignStartTime).NumTicks;
			CampaignTime campaignTime = CampaignTime.Days(1f);
			if (numTicks % CampaignTime.TimeTicksPerDay != 0L)
			{
				campaignTime = CampaignTime.Days((float)(numTicks % CampaignTime.TimeTicksPerDay) / (float)CampaignTime.TimeTicksPerDay);
			}
			this._dailyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Days(1f), campaignTime);
			this._dailyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.DailyTick));
			CampaignTime campaignTime2 = CampaignTime.Hours(0.5f);
			if (numTicks % CampaignTime.TimeTicksPerHour != 0L)
			{
				campaignTime2 = CampaignTime.Hours((float)(numTicks % CampaignTime.TimeTicksPerHour) / (float)CampaignTime.TimeTicksPerHour);
			}
			this._hourlyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(1f), campaignTime2);
			this._hourlyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.HourlyTick));
			campaignTime2 = CampaignTime.Hours(0.125f);
			if (numTicks % (CampaignTime.TimeTicksPerHour / 4L) != 0L)
			{
				campaignTime2 = CampaignTime.Hours((float)(numTicks % (CampaignTime.TimeTicksPerHour / 4L)) / (float)(CampaignTime.TimeTicksPerHour / 4L));
			}
			this._QuarterHourlyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(0.25f), campaignTime2);
			this._QuarterHourlyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.QuarterHourlyTick));
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000163C4 File Offset: 0x000145C4
		private void PartiesThink(float dt)
		{
			for (int i = 0; i < this.MobileParties.Count; i++)
			{
				this.MobileParties[i].Ai.Tick(dt);
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00016400 File Offset: 0x00014600
		public TComponent GetEntityComponent<TComponent>() where TComponent : CampaignEntityComponent
		{
			EntitySystem<CampaignEntityComponent> campaignEntitySystem = this._campaignEntitySystem;
			if (campaignEntitySystem == null)
			{
				return default(TComponent);
			}
			return campaignEntitySystem.GetComponent<TComponent>();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00016426 File Offset: 0x00014626
		public TComponent AddEntityComponent<TComponent>() where TComponent : CampaignEntityComponent, new()
		{
			return this._campaignEntitySystem.AddComponent<TComponent>();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00016433 File Offset: 0x00014633
		public void RemoveEntityComponent<TComponent>() where TComponent : CampaignEntityComponent
		{
			this._campaignEntitySystem.RemoveComponent<TComponent>();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00016440 File Offset: 0x00014640
		public void RemoveEntityComponent<TComponent>(TComponent component) where TComponent : CampaignEntityComponent
		{
			this._campaignEntitySystem.RemoveComponent(component);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00016453 File Offset: 0x00014653
		public List<TComponent> GetComponents<TComponent>() where TComponent : CampaignEntityComponent
		{
			return this._campaignEntitySystem.GetComponents<TComponent>();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00016460 File Offset: 0x00014660
		public MBReadOnlyList<CampaignEntityComponent> CampaignEntityComponents
		{
			get
			{
				return this._campaignEntitySystem.Components;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001646D File Offset: 0x0001466D
		public T GetCampaignBehavior<T>()
		{
			return this._campaignBehaviorManager.GetBehavior<T>();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0001647A File Offset: 0x0001467A
		public IEnumerable<T> GetCampaignBehaviors<T>()
		{
			return this._campaignBehaviorManager.GetBehaviors<T>();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00016487 File Offset: 0x00014687
		public void AddCampaignBehaviorManager(ICampaignBehaviorManager manager)
		{
			this._campaignBehaviorManager = manager;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00016490 File Offset: 0x00014690
		public MBReadOnlyList<Hero> AliveHeroes
		{
			get
			{
				return this.CampaignObjectManager.AliveHeroes;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0001649D File Offset: 0x0001469D
		public MBReadOnlyList<Hero> DeadOrDisabledHeroes
		{
			get
			{
				return this.CampaignObjectManager.DeadOrDisabledHeroes;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x000164AA File Offset: 0x000146AA
		public MBReadOnlyList<MobileParty> MobileParties
		{
			get
			{
				return this.CampaignObjectManager.MobileParties;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000164B7 File Offset: 0x000146B7
		public MBReadOnlyList<MobileParty> CaravanParties
		{
			get
			{
				return this.CampaignObjectManager.CaravanParties;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x000164C4 File Offset: 0x000146C4
		public MBReadOnlyList<MobileParty> PatrolParties
		{
			get
			{
				return this.CampaignObjectManager.PatrolParties;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000164D1 File Offset: 0x000146D1
		public MBReadOnlyList<MobileParty> VillagerParties
		{
			get
			{
				return this.CampaignObjectManager.VillagerParties;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x000164DE File Offset: 0x000146DE
		public MBReadOnlyList<MobileParty> MilitiaParties
		{
			get
			{
				return this.CampaignObjectManager.MilitiaParties;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000164EB File Offset: 0x000146EB
		public MBReadOnlyList<MobileParty> GarrisonParties
		{
			get
			{
				return this.CampaignObjectManager.GarrisonParties;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000164F8 File Offset: 0x000146F8
		public MBReadOnlyList<MobileParty> CustomParties
		{
			get
			{
				return this.CampaignObjectManager.CustomParties;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00016505 File Offset: 0x00014705
		public MBReadOnlyList<MobileParty> LordParties
		{
			get
			{
				return this.CampaignObjectManager.LordParties;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00016512 File Offset: 0x00014712
		public MBReadOnlyList<MobileParty> BanditParties
		{
			get
			{
				return this.CampaignObjectManager.BanditParties;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0001651F File Offset: 0x0001471F
		public MBReadOnlyList<MobileParty> PartiesWithoutPartyComponent
		{
			get
			{
				return this.CampaignObjectManager.PartiesWithoutPartyComponent;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0001652C File Offset: 0x0001472C
		public MBReadOnlyList<BattleWreckage> Wreckages
		{
			get
			{
				return this.CampaignObjectManager.Wreckages;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00016539 File Offset: 0x00014739
		public MBReadOnlyList<Settlement> Settlements
		{
			get
			{
				return this.CampaignObjectManager.Settlements;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00016546 File Offset: 0x00014746
		public IEnumerable<IFaction> Factions
		{
			get
			{
				return this.CampaignObjectManager.Factions;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00016553 File Offset: 0x00014753
		public MBReadOnlyList<Kingdom> Kingdoms
		{
			get
			{
				return this.CampaignObjectManager.Kingdoms;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00016560 File Offset: 0x00014760
		public MBReadOnlyList<Clan> Clans
		{
			get
			{
				return this.CampaignObjectManager.Clans;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0001656D File Offset: 0x0001476D
		public MBReadOnlyList<CharacterObject> Characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00016575 File Offset: 0x00014775
		public MBReadOnlyList<WorkshopType> Workshops
		{
			get
			{
				return this._workshops;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0001657D File Offset: 0x0001477D
		public MBReadOnlyList<ItemModifier> ItemModifiers
		{
			get
			{
				return this._itemModifiers;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00016585 File Offset: 0x00014785
		public MBReadOnlyList<ItemModifierGroup> ItemModifierGroups
		{
			get
			{
				return this._itemModifierGroups;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0001658D File Offset: 0x0001478D
		public MBReadOnlyList<Concept> Concepts
		{
			get
			{
				return this._concepts;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00016595 File Offset: 0x00014795
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0001659D File Offset: 0x0001479D
		[SaveableProperty(60)]
		public MobileParty MainParty { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000309 RID: 777 RVA: 0x000165A6 File Offset: 0x000147A6
		// (set) Token: 0x0600030A RID: 778 RVA: 0x000165AE File Offset: 0x000147AE
		public PartyBase CameraFollowParty
		{
			get
			{
				return this._cameraFollowParty;
			}
			set
			{
				this._cameraFollowParty = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000165B7 File Offset: 0x000147B7
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000165BF File Offset: 0x000147BF
		[SaveableProperty(62)]
		public CampaignInformationManager CampaignInformationManager { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000165C8 File Offset: 0x000147C8
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000165D0 File Offset: 0x000147D0
		[SaveableProperty(63)]
		public VisualTrackerManager VisualTrackerManager { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000165D9 File Offset: 0x000147D9
		public LogEntryHistory LogEntryHistory
		{
			get
			{
				return this._logEntryHistory;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000310 RID: 784 RVA: 0x000165E1 File Offset: 0x000147E1
		// (set) Token: 0x06000311 RID: 785 RVA: 0x000165E9 File Offset: 0x000147E9
		public EncyclopediaManager EncyclopediaManager { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000165F2 File Offset: 0x000147F2
		// (set) Token: 0x06000313 RID: 787 RVA: 0x000165FA File Offset: 0x000147FA
		public ConversationManager ConversationManager { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00016603 File Offset: 0x00014803
		public bool IsDay
		{
			get
			{
				return !this.IsNight;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00016610 File Offset: 0x00014810
		public bool IsNight
		{
			get
			{
				return CampaignTime.Now.IsNightTime;
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0001662A File Offset: 0x0001482A
		internal int GeneratePartyId(PartyBase party)
		{
			int lastPartyIndex = this._lastPartyIndex;
			this._lastPartyIndex++;
			return lastPartyIndex;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00016640 File Offset: 0x00014840
		private void LoadMapScene()
		{
			this._mapSceneWrapper = this.MapSceneCreator.CreateMapScene();
			this._mapSceneWrapper.SetSceneLevels(new List<string> { "level_1", "level_2", "level_3", "siege", "raid", "burned" });
			this._mapSceneWrapper.Load();
			Vec2 vec;
			Vec2 vec2;
			float num;
			this._mapSceneWrapper.GetMapBorders(out vec, out vec2, out num);
			Campaign.MapMinimumPosition = vec;
			Campaign.MapMaximumPosition = vec2;
			Campaign.MapMaximumHeight = num;
			Campaign.MapDiagonal = Campaign.MapMinimumPosition.Distance(Campaign.MapMaximumPosition);
			Campaign.MapDiagonalSquared = Campaign.MapDiagonal * Campaign.MapDiagonal;
			Campaign.PlayerRegionSwitchCostFromLandToSea = (int)(Campaign.MapDiagonal * (float)this.Models.MapDistanceModel.RegionSwitchCostFromLandToSea * 0.2f);
			Campaign.PathFindingMaxCostLimit = Math.Max(Campaign.PlayerRegionSwitchCostFromLandToSea * 100, (int)(Campaign.MapDiagonal * 500f));
			this._mapSceneWrapper.AfterLoad();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00016754 File Offset: 0x00014954
		private void InitializeCachedLists()
		{
			MBObjectManager objectManager = Game.Current.ObjectManager;
			this._characters = objectManager.GetObjectTypeList<CharacterObject>();
			this._workshops = objectManager.GetObjectTypeList<WorkshopType>();
			this._itemModifiers = objectManager.GetObjectTypeList<ItemModifier>();
			this._itemModifierGroups = objectManager.GetObjectTypeList<ItemModifierGroup>();
			this._concepts = objectManager.GetObjectTypeList<Concept>();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000167A8 File Offset: 0x000149A8
		private void InitializeDefaultEquipments()
		{
			this.DeadBattleEquipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("default_battle_equipment_roster_neutral").DefaultEquipment;
			this.DeadCivilianEquipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("default_civilian_equipment_roster_neutral").DefaultEquipment;
			this.DefaultStealthEquipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("default_stealth_equipment_roster").DefaultEquipment;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00016814 File Offset: 0x00014A14
		public override void OnDestroy()
		{
			this.WaitAsyncTasks();
			GameTexts.ClearInstance();
			IMapScene mapSceneWrapper = this._mapSceneWrapper;
			if (mapSceneWrapper != null)
			{
				mapSceneWrapper.Destroy();
			}
			ConversationManager.Clear();
			MBTextManager.ClearAll();
			GameSceneDataManager.Destroy();
			this.CampaignInformationManager.DeRegisterEvents();
			ICampaignBehaviorManager campaignBehaviorManager = this._campaignBehaviorManager;
			if (campaignBehaviorManager != null)
			{
				campaignBehaviorManager.ClearBehaviors();
			}
			MBSaveLoad.OnGameDestroy();
			Campaign.Current = null;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00016873 File Offset: 0x00014A73
		public void InitializeSinglePlayerReferences()
		{
			this.IsSinglePlayerReferencesInitialized = true;
			this.InitializeGamePlayReferences();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00016884 File Offset: 0x00014A84
		private void CreateLists()
		{
			this.AllPerks = MBObjectManager.Instance.GetObjectTypeList<PerkObject>();
			this.AllTraits = MBObjectManager.Instance.GetObjectTypeList<TraitObject>();
			this.AllEquipmentRosters = MBObjectManager.Instance.GetObjectTypeList<MBEquipmentRoster>();
			this.AllPolicies = MBObjectManager.Instance.GetObjectTypeList<PolicyObject>();
			this.AllBuildingTypes = MBObjectManager.Instance.GetObjectTypeList<BuildingType>();
			this.AllIssueEffects = MBObjectManager.Instance.GetObjectTypeList<IssueEffect>();
			this.AllSiegeStrategies = MBObjectManager.Instance.GetObjectTypeList<SiegeStrategy>();
			this.AllVillageTypes = MBObjectManager.Instance.GetObjectTypeList<VillageType>();
			this.AllSkillEffects = MBObjectManager.Instance.GetObjectTypeList<SkillEffect>();
			this.AllFeats = MBObjectManager.Instance.GetObjectTypeList<FeatObject>();
			this.AllSkills = MBObjectManager.Instance.GetObjectTypeList<SkillObject>();
			this.AllSiegeEngineTypes = MBObjectManager.Instance.GetObjectTypeList<SiegeEngineType>();
			this.AllItemCategories = MBObjectManager.Instance.GetObjectTypeList<ItemCategory>();
			this.AllCharacterAttributes = MBObjectManager.Instance.GetObjectTypeList<CharacterAttribute>();
			this.AllItems = MBObjectManager.Instance.GetObjectTypeList<ItemObject>();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00016984 File Offset: 0x00014B84
		private void CheckMapUpdate()
		{
			uint sceneXmlCrc = this.MapSceneWrapper.GetSceneXmlCrc();
			uint sceneNavigationMeshCrc = this.MapSceneWrapper.GetSceneNavigationMeshCrc();
			if (sceneXmlCrc != this._campaignMapSceneXmlCrc || sceneNavigationMeshCrc != this._campaignMapSceneNavigationMeshCrc)
			{
				this.CalculateCachedValues();
				foreach (Settlement settlement in this.Settlements)
				{
					settlement.CheckPositionsForMapChangeAndUpdateIfNeeded();
				}
				foreach (MapEvent mapEvent in this.MapEventManager.MapEvents)
				{
					mapEvent.CheckPositionsForMapChangeAndUpdateIfNeeded();
				}
				foreach (Kingdom kingdom in this.Kingdoms)
				{
					foreach (Army army in kingdom.Armies)
					{
						army.CheckPositionsForMapChangeAndUpdateIfNeeded();
					}
				}
				foreach (MobileParty mobileParty in this.MobileParties)
				{
					mobileParty.CheckPositionsForMapChangeAndUpdateIfNeeded();
					mobileParty.CheckAiForMapChangeAndUpdateIfNeeded();
				}
				this._campaignMapSceneXmlCrc = sceneXmlCrc;
				this._campaignMapSceneNavigationMeshCrc = sceneNavigationMeshCrc;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00016B1C File Offset: 0x00014D1C
		private void CalculateCachedValues()
		{
			this.EstimatedMaximumLordPartySpeedExceptPlayer = 10f;
			this.EstimatedAverageLordPartySpeed = 3.36f;
			this.EstimatedAverageCaravanPartySpeed = 4.2f;
			this.EstimatedAverageVillagerPartySpeed = 3.43f;
			this.EstimatedAverageBanditPartySpeed = 3.41f;
			this.EstimatedAverageLordPartyNavalSpeed = this.EstimatedAverageLordPartySpeed * 1.2f;
			this.EstimatedAverageCaravanPartyNavalSpeed = 3.53f;
			this.EstimatedAverageVillagerPartyNavalSpeed = 4.01f;
			this.EstimatedAverageBanditPartyNavalSpeed = 3.57f;
			this.CalculateAverageDistanceBetweenTowns();
			this.CalculateAverageWage();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00016BA0 File Offset: 0x00014DA0
		private void CalculateAverageWage()
		{
			float num = 0f;
			float num2 = 0f;
			foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
			{
				if (cultureObject.IsMainCulture)
				{
					foreach (PartyTemplateStack partyTemplateStack in cultureObject.DefaultPartyTemplate.Stacks)
					{
						int troopWage = partyTemplateStack.Character.TroopWage;
						float num3 = (float)(partyTemplateStack.MaxValue + partyTemplateStack.MinValue) * 0.5f;
						num += (float)troopWage * num3;
						num2 += num3;
					}
				}
			}
			if (num2 > 0f)
			{
				this.AverageWage = num / num2;
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00016C90 File Offset: 0x00014E90
		private void CalculateAverageDistanceBetweenTowns()
		{
			this._averageDistanceBetweenClosestTwoTowns = new Dictionary<MobileParty.NavigationType, float>();
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int num4 = 0;
			foreach (Town town in this.AllTowns)
			{
				float num5 = float.MaxValue;
				float num6 = float.MaxValue;
				float num7 = float.MaxValue;
				foreach (Town town2 in this.AllTowns)
				{
					if (town != town2)
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, false, false, MobileParty.NavigationType.Default);
						if (distance < Campaign.MapDiagonal && distance < num6)
						{
							num6 = distance;
						}
						if (town.Settlement.HasPort && town2.Settlement.HasPort)
						{
							float distance2 = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, true, true, MobileParty.NavigationType.Naval);
							if (distance2 < Campaign.MapDiagonal && distance2 < num7)
							{
								num7 = distance2;
							}
						}
						float num8 = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, false, false, MobileParty.NavigationType.All);
						if (town.Settlement.HasPort)
						{
							float distance3 = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, true, false, MobileParty.NavigationType.All);
							if (distance3 < Campaign.MapDiagonal && distance3 < num8)
							{
								num8 = distance3;
							}
						}
						if (town2.Settlement.HasPort)
						{
							float distance4 = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, false, true, MobileParty.NavigationType.All);
							if (distance4 < Campaign.MapDiagonal && distance4 < num8)
							{
								num8 = distance4;
							}
						}
						if (town.Settlement.HasPort && town2.Settlement.HasPort)
						{
							float distance5 = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement, true, true, MobileParty.NavigationType.All);
							if (distance5 < Campaign.MapDiagonal && distance5 < num8)
							{
								num8 = distance5;
							}
						}
						if (num8 < num5)
						{
							num5 = num8;
						}
					}
				}
				if (num5 < Campaign.MapDiagonal)
				{
					num += num5;
				}
				if (num7 < Campaign.MapDiagonal)
				{
					num2 += num7;
				}
				if (num6 < Campaign.MapDiagonal)
				{
					num3 += num6;
				}
				num4++;
			}
			this._averageDistanceBetweenClosestTwoTowns.Add(MobileParty.NavigationType.Default, num3 / (float)num4);
			this._averageDistanceBetweenClosestTwoTowns.Add(MobileParty.NavigationType.Naval, num2 / (float)num4);
			this._averageDistanceBetweenClosestTwoTowns.Add(MobileParty.NavigationType.All, num / (float)num4);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00016F84 File Offset: 0x00015184
		public void InitializeGamePlayReferences()
		{
			base.CurrentGame.PlayerTroop = base.CurrentGame.ObjectManager.GetObject<CharacterObject>("main_hero");
			if (Hero.MainHero.Mother != null)
			{
				Hero.MainHero.Mother.SetHasMet();
			}
			if (Hero.MainHero.Father != null)
			{
				Hero.MainHero.Father.SetHasMet();
			}
			this.PlayerDefaultFaction = this.CampaignObjectManager.Find<Clan>("player_faction");
			Hero.MainHero.ChangeState(Hero.CharacterStates.Active);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00017008 File Offset: 0x00015208
		private void InitializeScenes()
		{
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetActiveModules())
			{
				string text = ModuleHelper.GetModuleFullPath(moduleInfo.Id) + "ModuleData/";
				string text2 = text + "sp_battle_scenes.xml";
				string text3 = text + "conversation_scenes.xml";
				string text4 = text + "meeting_scenes.xml";
				if (File.Exists(text2))
				{
					GameSceneDataManager.Instance.LoadSPBattleScenes(text2);
				}
				if (File.Exists(text3))
				{
					GameSceneDataManager.Instance.LoadConversationScenes(text3);
				}
				if (File.Exists(text4))
				{
					GameSceneDataManager.Instance.LoadMeetingScenes(text4);
				}
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000170C4 File Offset: 0x000152C4
		public void SetLoadingParameters(Campaign.GameLoadingType gameLoadingType)
		{
			Campaign.Current = this;
			this._gameLoadingType = gameLoadingType;
			if (gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				Campaign.Current.GameStarted = true;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000170E2 File Offset: 0x000152E2
		public void AddCampaignEventReceiver(CampaignEventReceiver receiver)
		{
			this.CampaignEventDispatcher.AddCampaignEventReceiver(receiver);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000170F0 File Offset: 0x000152F0
		protected override void OnInitialize()
		{
			this.CampaignEvents = new CampaignEvents();
			this.CustomPeriodicCampaignEvents = new List<MBCampaignEvent>();
			this.CampaignEventDispatcher = new CampaignEventDispatcher(new CampaignEventReceiver[] { this.CampaignEvents, this.IssueManager, this.QuestManager });
			this.SandBoxManager = Game.Current.AddGameHandler<SandBoxManager>();
			this.SaveHandler = new SaveHandler();
			this.VisualCreator = new VisualCreator();
			this.GameMenuManager = new GameMenuManager();
			this._towns = new MBList<Town>();
			this._castles = new MBList<Town>();
			this._villages = new MBList<Village>();
			this._hideouts = new MBList<Hideout>();
			if (this._gameLoadingType != Campaign.GameLoadingType.Editor)
			{
				this.CreateManagers();
			}
			CampaignGameStarter campaignGameStarter = new CampaignGameStarter(this.GameMenuManager, this.ConversationManager);
			this.SandBoxManager.Initialize(campaignGameStarter);
			base.GameManager.InitializeGameStarter(base.CurrentGame, campaignGameStarter);
			GameSceneDataManager.Initialize();
			if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign || this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.InitializeScenes();
			}
			base.GameManager.OnGameStart(base.CurrentGame, campaignGameStarter);
			base.CurrentGame.SetBasicModels(campaignGameStarter.Models);
			this._gameModels = base.CurrentGame.AddGameModelsManager<GameModels>(campaignGameStarter.Models);
			CampaignTime.Initialize();
			base.CurrentGame.CreateGameManager();
			if (this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.InitializeDefaultCampaignObjects();
			}
			else
			{
				this.MapTimeTracker = new MapTimeTracker(this.Models.CampaignTimeModel.CampaignStartTime);
			}
			base.GameManager.BeginGameStart(base.CurrentGame);
			if (this._gameLoadingType != Campaign.GameLoadingType.SavedCampaign)
			{
				this.OnNewCampaignStart();
			}
			this.CreateLists();
			this.InitializeBasicObjectXmls();
			if (this._gameLoadingType != Campaign.GameLoadingType.SavedCampaign)
			{
				base.GameManager.OnNewCampaignStart(base.CurrentGame, campaignGameStarter);
			}
			this.SandBoxManager.OnCampaignStart(campaignGameStarter, base.GameManager, this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign);
			if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign || this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.DetermineSavedStats(this._gameLoadingType);
			}
			if (this._gameLoadingType != Campaign.GameLoadingType.SavedCampaign)
			{
				this.AddCampaignBehaviorManager(new CampaignBehaviorManager(campaignGameStarter.CampaignBehaviors));
				base.GameManager.OnAfterCampaignStart(base.CurrentGame);
			}
			else
			{
				base.GameManager.OnGameLoaded(base.CurrentGame, campaignGameStarter);
				this._campaignBehaviorManager.InitializeCampaignBehaviors(campaignGameStarter.CampaignBehaviors);
				this._campaignBehaviorManager.LoadBehaviorData();
				this._campaignBehaviorManager.RegisterEvents();
			}
			foreach (INonReadyObjectHandler nonReadyObjectHandler in this.GetCampaignBehaviors<INonReadyObjectHandler>())
			{
				nonReadyObjectHandler.OnBeforeNonReadyObjectsDeleted();
			}
			if (this._gameLoadingType != Campaign.GameLoadingType.Tutorial)
			{
				campaignGameStarter.UnregisterNonReadyObjects();
			}
			if (this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.InitializeCampaignObjectsOnAfterLoad();
			}
			else if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign || this._gameLoadingType == Campaign.GameLoadingType.Tutorial)
			{
				this.CampaignObjectManager.InitializeOnNewGame();
			}
			this.InitializeCachedLists();
			this.InitializeDefaultEquipments();
			this.NameGenerator.Initialize();
			base.CurrentGame.OnGameStart();
			base.GameManager.OnGameInitializationFinished(base.CurrentGame);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00017400 File Offset: 0x00015600
		private void CalculateCachedStatsOnLoad()
		{
			ItemRoster.CalculateCachedStatsOnLoad();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00017407 File Offset: 0x00015607
		private void InitializeBasicObjectXmls()
		{
			base.ObjectManager.LoadXML("SPCultures", false);
			base.ObjectManager.LoadXML("Concepts", false);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0001742C File Offset: 0x0001562C
		private void InitializeDefaultCampaignObjects()
		{
			base.CurrentGame.InitializeDefaultGameObjects();
			this.DefaultItems = new DefaultItems();
			base.CurrentGame.LoadBasicFiles();
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("EquipmentRosters", false);
			base.ObjectManager.LoadXML("partyTemplates", false);
			WeaponDescription @object = MBObjectManager.Instance.GetObject<WeaponDescription>("OneHandedBastardSwordAlternative");
			if (@object != null)
			{
				@object.IsHiddenFromUI = true;
			}
			WeaponDescription object2 = MBObjectManager.Instance.GetObject<WeaponDescription>("OneHandedBastardAxeAlternative");
			if (object2 != null)
			{
				object2.IsHiddenFromUI = true;
			}
			this.DefaultIssueEffects = new DefaultIssueEffects();
			this.DefaultTraits = new DefaultTraits();
			this.DefaultPolicies = new DefaultPolicies();
			this.DefaultPerks = new DefaultPerks();
			this.DefaultBuildingTypes = new DefaultBuildingTypes();
			this.DefaultVillageTypes = new DefaultVillageTypes();
			this.DefaultSiegeStrategies = new DefaultSiegeStrategies();
			this.DefaultSkillEffects = new DefaultSkillEffects();
			this.DefaultFeats = new DefaultCulturalFeats();
			this.DefaultPersonalityTraitEffects = new DefaultPersonalityTraitEffects();
			this.DefaultFigureheads = new DefaultFigureheads();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001753A File Offset: 0x0001573A
		private void InitializeManagers()
		{
			this.KingdomManager = new KingdomManager();
			this.CampaignInformationManager = new CampaignInformationManager();
			this.VisualTrackerManager = new VisualTrackerManager();
			this.MapTrackerManager = new MapTrackerManager();
			this.TournamentManager = new TournamentManager();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00017574 File Offset: 0x00015774
		private void InitializeCampaignObjectsOnAfterLoad()
		{
			this.CampaignObjectManager.InitializeOnLoad();
			this.FactionManager.PreAfterLoad();
			List<PerkObject> list = this.AllPerks.Where<PerkObject>((PerkObject x) => !x.IsTrash).ToList<PerkObject>();
			this.AllPerks = new MBReadOnlyList<PerkObject>(list);
			this.LogEntryHistory.OnAfterLoad();
			foreach (Kingdom kingdom in this.Kingdoms)
			{
				foreach (Army army in kingdom.Armies)
				{
					army.OnAfterLoad();
				}
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001765C File Offset: 0x0001585C
		private void OnNewCampaignStart()
		{
			Game.Current.PlayerTroop = null;
			this.MapStateData = new MapStateData();
			this.InitializeDefaultCampaignObjects();
			this.MainParty = MBObjectManager.Instance.CreateObject<MobileParty>("player_party");
			this.InitializeManagers();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00017695 File Offset: 0x00015895
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<FeatObject>("feat", "Feats", 0U, true, false);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000176AC File Offset: 0x000158AC
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<MobileParty>("MobileParty", "MobileParties", 14U, true, true);
			objectManager.RegisterType<CharacterObject>("NPCCharacter", "NPCCharacters", 16U, true, false);
			if (this.GameMode == CampaignGameMode.Tutorial)
			{
				objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "MPCharacters", 43U, true, false);
			}
			objectManager.RegisterType<CultureObject>("Culture", "SPCultures", 17U, true, false);
			objectManager.RegisterType<Clan>("Faction", "Factions", 18U, true, true);
			objectManager.RegisterType<PerkObject>("Perk", "Perks", 19U, true, false);
			objectManager.RegisterType<Kingdom>("Kingdom", "Kingdoms", 20U, true, true);
			objectManager.RegisterType<TraitObject>("Trait", "Traits", 21U, true, false);
			objectManager.RegisterType<VillageType>("VillageType", "VillageTypes", 22U, true, false);
			objectManager.RegisterType<BuildingType>("BuildingType", "BuildingTypes", 23U, true, false);
			objectManager.RegisterType<PartyTemplateObject>("PartyTemplate", "partyTemplates", 24U, true, false);
			objectManager.RegisterType<Settlement>("Settlement", "Settlements", 25U, true, false);
			objectManager.RegisterType<WorkshopType>("WorkshopType", "WorkshopTypes", 26U, true, false);
			objectManager.RegisterType<Village>("Village", "Components", 27U, true, false);
			objectManager.RegisterType<Hideout>("Hideout", "Components", 30U, true, false);
			objectManager.RegisterType<Town>("Town", "Components", 31U, true, false);
			objectManager.RegisterType<Hero>("Hero", "Heroes", 32U, true, true);
			objectManager.RegisterType<MenuContext>("MenuContext", "MenuContexts", 35U, true, false);
			objectManager.RegisterType<PolicyObject>("Policy", "Policies", 36U, true, false);
			objectManager.RegisterType<Concept>("Concept", "Concepts", 37U, true, false);
			objectManager.RegisterType<IssueEffect>("IssueEffect", "IssueEffects", 39U, true, false);
			objectManager.RegisterType<SiegeStrategy>("SiegeStrategy", "SiegeStrategies", 40U, true, false);
			objectManager.RegisterType<SkillEffect>("SkillEffect", "SkillEffects", 53U, true, false);
			objectManager.RegisterType<LocationComplexTemplate>("LocationComplexTemplate", "LocationComplexTemplates", 42U, true, false);
			objectManager.RegisterType<RetirementSettlementComponent>("RetirementSettlementComponent", "Components", 56U, true, false);
			objectManager.RegisterType<MissionShipObject>("MissionShip", "MissionShips", 57U, true, false);
			objectManager.RegisterType<ShipHull>("ShipHull", "ShipHulls", 58U, true, false);
			objectManager.RegisterType<ShipSlot>("ShipSlot", "ShipSlots", 59U, true, false);
			objectManager.RegisterType<ShipUpgradePiece>("ShipUpgradePiece", "ShipUpgradePieces", 60U, true, false);
			objectManager.RegisterType<Incident>("Incident", "Incidents", 62U, true, false);
			objectManager.RegisterType<Figurehead>("Figurehead", "Figureheads", 63U, true, false);
			objectManager.RegisterType<ShipPhysicsReference>("ShipPhysicsReference", "ShipPhysicsReferences", 64U, true, false);
			objectManager.RegisterType<TraitEffectObject>("trait_effect", "TraitEffects", 65U, true, false);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00017956 File Offset: 0x00015B56
		private void CreateManagers()
		{
			this.EncyclopediaManager = new EncyclopediaManager();
			this.ConversationManager = new ConversationManager();
			this.NameGenerator = new NameGenerator();
			this.SkillLevelingManager = new DefaultSkillLevelingManager();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00017984 File Offset: 0x00015B84
		private void OnNewGameCreated(CampaignGameStarter gameStarter)
		{
			this.OnNewGameCreatedInternal();
			GameManagerBase gameManager = base.GameManager;
			if (gameManager != null)
			{
				gameManager.OnNewGameCreated(base.CurrentGame, gameStarter);
			}
			CampaignEventDispatcher.Instance.OnNewGameCreated(gameStarter);
			this.OnAfterNewGameCreatedInternal();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000179B8 File Offset: 0x00015BB8
		private void OnNewGameCreatedInternal()
		{
			this.UniqueGameId = MiscHelper.GenerateCampaignId(12);
			this._newGameVersion = MBSaveLoad.CurrentVersion.ToString();
			this.PlatformID = ApplicationPlatform.CurrentPlatform.ToString();
			this.PlayerTraitDeveloper = new PropertyOwner<PropertyObject>();
			TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
			this.TimeControlMode = CampaignTimeControlMode.Stop;
			this._campaignEntitySystem = new EntitySystem<CampaignEntityComponent>();
			this.SiegeEventManager = new SiegeEventManager();
			this.MapEventManager = new MapEventManager();
			this.MinSettlementX = float.MaxValue;
			this.MinSettlementY = float.MaxValue;
			this.MaxSettlementX = float.MinValue;
			this.MaxSettlementY = float.MinValue;
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.Position.X < this.MinSettlementX)
				{
					this.MinSettlementX = settlement.Position.X;
				}
				if (settlement.Position.Y < this.MinSettlementY)
				{
					this.MinSettlementY = settlement.Position.Y;
				}
				if (settlement.Position.X > this.MaxSettlementX)
				{
					this.MaxSettlementX = settlement.Position.X;
				}
				if (settlement.Position.Y > this.MaxSettlementY)
				{
					this.MaxSettlementY = settlement.Position.Y;
				}
			}
			this.CampaignBehaviorManager.RegisterEvents();
			this.CameraFollowParty = this.MainParty.Party;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00017B78 File Offset: 0x00015D78
		private void OnAfterNewGameCreatedInternal()
		{
			Hero.MainHero.ChangeState(Hero.CharacterStates.Active);
			this._playerFormationPreferences = new Dictionary<CharacterObject, FormationClass>();
			this.PlayerFormationPreferences = this._playerFormationPreferences.GetReadOnlyDictionary<CharacterObject, FormationClass>();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00017BA4 File Offset: 0x00015DA4
		protected override void DoLoadingForGameType(GameTypeLoadingStates gameTypeLoadingState, out GameTypeLoadingStates nextState)
		{
			nextState = GameTypeLoadingStates.None;
			switch (gameTypeLoadingState)
			{
			case GameTypeLoadingStates.InitializeFirstStep:
				base.CurrentGame.Initialize();
				nextState = GameTypeLoadingStates.WaitSecondStep;
				return;
			case GameTypeLoadingStates.WaitSecondStep:
				nextState = GameTypeLoadingStates.LoadVisualsThirdState;
				return;
			case GameTypeLoadingStates.LoadVisualsThirdState:
				if (this.GameMode == CampaignGameMode.Campaign)
				{
					this.LoadMapScene();
				}
				nextState = GameTypeLoadingStates.PostInitializeFourthState;
				return;
			case GameTypeLoadingStates.PostInitializeFourthState:
			{
				CampaignGameStarter gameStarter = this.SandBoxManager.GameStarter;
				if (this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
				{
					this.CheckMapUpdate();
					this.OnDataLoadFinished(gameStarter);
					this.CalculateCachedValues();
					this.CalculateCachedStatsOnLoad();
					base.GameManager.OnAfterGameLoaded(base.CurrentGame);
					this.OnGameLoaded(gameStarter);
					this.OnSessionStart(gameStarter);
					foreach (Hero hero in Hero.AllAliveHeroes)
					{
						hero.CheckInvalidEquipmentsAndReplaceIfNeeded();
					}
					using (List<Hero>.Enumerator enumerator = Hero.DeadOrDisabledHeroes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Hero hero2 = enumerator.Current;
							hero2.CheckInvalidEquipmentsAndReplaceIfNeeded();
						}
						goto IL_019D;
					}
				}
				if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign)
				{
					this._campaignMapSceneXmlCrc = this.MapSceneWrapper.GetSceneXmlCrc();
					this._campaignMapSceneNavigationMeshCrc = this.MapSceneWrapper.GetSceneNavigationMeshCrc();
					this.OnDataLoadFinished(gameStarter);
					this.CalculateCachedValues();
					MBSaveLoad.OnNewGame();
					this.InitializeMainParty();
					foreach (Settlement settlement in Settlement.All)
					{
						settlement.OnGameCreated();
					}
					MBObjectManager.Instance.RemoveTemporaryTypes();
					this.OnNewGameCreated(gameStarter);
					this.OnSessionStart(gameStarter);
					Debug.Print("Finished starting a new game.", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				IL_019D:
				base.GameManager.OnAfterGameInitializationFinished(base.CurrentGame, gameStarter);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00017D88 File Offset: 0x00015F88
		private void DetermineSavedStats(Campaign.GameLoadingType gameLoadingType)
		{
			if (this._previouslyUsedModules == null)
			{
				this._previouslyUsedModules = new MBList<string>();
			}
			if (this._usedGameVersions == null)
			{
				this._usedGameVersions = new MBList<string>();
			}
			string text = MBSaveLoad.CurrentVersion.ToString();
			string text2 = string.Join(MBSaveLoad.ModuleCodeSeperator.ToString(), from x in ModuleHelper.GetActiveModules()
				select x.Id + MBSaveLoad.ModuleVersionSeperator.ToString() + x.Version);
			if (this._usedGameVersions.Count <= 0 || this._usedGameVersions.Last<string>() != text)
			{
				this._usedGameVersions.Add(text);
			}
			if (this._previouslyUsedModules.LastOrDefault<string>() != text2)
			{
				this._previouslyUsedModules.Add(text2);
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00017E57 File Offset: 0x00016057
		public override void OnMissionIsStarting(string missionName, MissionInitializerRecord rec)
		{
			if (rec.PlayingInCampaignMode)
			{
				CampaignEventDispatcher.Instance.BeforeMissionOpened();
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00017E6B File Offset: 0x0001606B
		public override void InitializeParameters()
		{
			ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_campaign_parameters"));
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00017E86 File Offset: 0x00016086
		public void SetTimeControlModeLock(bool isLocked)
		{
			this.TimeControlModeLock = isLocked;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00017E8F File Offset: 0x0001608F
		public override bool IsPartyWindowAccessibleAtMission
		{
			get
			{
				return this.GameMode == CampaignGameMode.Tutorial;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00017E9A File Offset: 0x0001609A
		internal MBReadOnlyList<Town> AllTowns
		{
			get
			{
				return this._towns;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00017EA2 File Offset: 0x000160A2
		internal MBReadOnlyList<Town> AllCastles
		{
			get
			{
				return this._castles;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00017EAA File Offset: 0x000160AA
		internal MBReadOnlyList<Village> AllVillages
		{
			get
			{
				return this._villages;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00017EB2 File Offset: 0x000160B2
		internal MBReadOnlyList<Hideout> AllHideouts
		{
			get
			{
				return this._hideouts;
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00017EBC File Offset: 0x000160BC
		public void OnPlayerCharacterChanged(out bool isMainPartyChanged)
		{
			isMainPartyChanged = false;
			if (MobileParty.MainParty != Hero.MainHero.PartyBelongedTo)
			{
				isMainPartyChanged = true;
			}
			this.MainParty = Hero.MainHero.PartyBelongedTo;
			if (Hero.MainHero.CurrentSettlement != null && !Hero.MainHero.IsPrisoner)
			{
				if (this.MainParty == null)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(Hero.MainHero);
				}
				else
				{
					LeaveSettlementAction.ApplyForParty(this.MainParty);
				}
			}
			if (Hero.MainHero.IsFugitive)
			{
				Hero.MainHero.ChangeState(Hero.CharacterStates.Active);
			}
			this.PlayerTraitDeveloper = new PropertyOwner<PropertyObject>();
			TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
			if (this.MainParty == null)
			{
				this.MainParty = MobileParty.CreateParty("player_party_" + Hero.MainHero.StringId, null);
				LordPartyComponent.ConvertPartyToLordParty(this.MainParty, Hero.MainHero, Hero.MainHero);
				isMainPartyChanged = true;
				CampaignVec2 campaignVec;
				if (Hero.MainHero.IsPrisoner)
				{
					this.MainParty.RemovePartyLeader();
					PartyBase partyBelongedToAsPrisoner = Hero.MainHero.PartyBelongedToAsPrisoner;
					if (partyBelongedToAsPrisoner.IsMobile)
					{
						campaignVec = partyBelongedToAsPrisoner.MobileParty.Position;
					}
					else
					{
						campaignVec = partyBelongedToAsPrisoner.Settlement.GatePosition;
					}
					this.MainParty.IsActive = false;
				}
				else
				{
					CampaignVec2 campaignPosition = Hero.MainHero.GetCampaignPosition();
					campaignVec = ((campaignPosition.IsValid() && campaignPosition != CampaignVec2.Zero) ? campaignPosition : SettlementHelper.GetBestSettlementToSpawnAround(Hero.MainHero).GatePosition);
					this.MainParty.IsActive = true;
					this.MainParty.MemberRoster.AddToCounts(Hero.MainHero.CharacterObject, 1, true, 0, 0, true, -1);
				}
				this.MainParty.InitializeMobilePartyAtPosition(campaignVec);
			}
			PartyBase.MainParty.ItemRoster.UpdateVersion();
			PartyBase.MainParty.MemberRoster.UpdateVersion();
			PartyBase.MainParty.PrisonRoster.UpdateVersion();
			if (MobileParty.MainParty.IsActive)
			{
				PartyBase.MainParty.SetAsCameraFollowParty();
			}
			if (Hero.MainHero.Mother != null)
			{
				Hero.MainHero.Mother.SetHasMet();
			}
			if (Hero.MainHero.Father != null)
			{
				Hero.MainHero.Father.SetHasMet();
			}
			this.MainParty.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000180E8 File Offset: 0x000162E8
		public void SetPlayerFormationPreference(CharacterObject character, FormationClass formation)
		{
			if (!this._playerFormationPreferences.ContainsKey(character))
			{
				this._playerFormationPreferences.Add(character, formation);
				return;
			}
			this._playerFormationPreferences[character] = formation;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00018113 File Offset: 0x00016313
		public override void OnStateChanged(GameState oldState)
		{
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00018115 File Offset: 0x00016315
		public void UnlockFigurehead(Figurehead figurehead)
		{
			this.UnlockedFigureheadsByMainHero.Add(figurehead);
			CampaignEventDispatcher.Instance.OnFigureheadUnlocked(figurehead);
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0001812E File Offset: 0x0001632E
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00018136 File Offset: 0x00016336
		[SaveableProperty(68)]
		public PropertyOwner<PropertyObject> PlayerTraitDeveloper { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0001813F File Offset: 0x0001633F
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00018147 File Offset: 0x00016347
		[SaveableProperty(98)]
		public PlayerDataForNavalAutoTravel PlayerDataForNavalAutoTravel { get; set; }

		// Token: 0x06000344 RID: 836 RVA: 0x00018150 File Offset: 0x00016350
		internal static void AutoGeneratedStaticCollectObjectsCampaign(object o, List<object> collectedObjects)
		{
			((Campaign)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00018160 File Offset: 0x00016360
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Options);
			collectedObjects.Add(this.TournamentManager);
			collectedObjects.Add(this.UnlockedFigureheadsByMainHero);
			collectedObjects.Add(this.KingdomManager);
			collectedObjects.Add(this._campaignPeriodicEventManager);
			collectedObjects.Add(this._previouslyUsedModules);
			collectedObjects.Add(this._usedGameVersions);
			collectedObjects.Add(this._campaignBehaviorManager);
			collectedObjects.Add(this._customManagers);
			collectedObjects.Add(this._cameraFollowParty);
			collectedObjects.Add(this._logEntryHistory);
			collectedObjects.Add(this._playerFormationPreferences);
			collectedObjects.Add(this.CampaignObjectManager);
			collectedObjects.Add(this.QuestManager);
			collectedObjects.Add(this.IssueManager);
			collectedObjects.Add(this.IncidentManager);
			collectedObjects.Add(this.FactionManager);
			collectedObjects.Add(this.CharacterRelationManager);
			collectedObjects.Add(this.Romance);
			collectedObjects.Add(this.PlayerCaptivity);
			collectedObjects.Add(this.PlayerDefaultFaction);
			collectedObjects.Add(this.MapStateData);
			collectedObjects.Add(this.MapTimeTracker);
			collectedObjects.Add(this.SiegeEventManager);
			collectedObjects.Add(this.MapEventManager);
			collectedObjects.Add(this.MapTrackerManager);
			collectedObjects.Add(this.PlayerEncounter);
			collectedObjects.Add(this.BarterManager);
			collectedObjects.Add(this.MainParty);
			collectedObjects.Add(this.CampaignInformationManager);
			collectedObjects.Add(this.VisualTrackerManager);
			collectedObjects.Add(this.PlayerTraitDeveloper);
			collectedObjects.Add(this.PlayerDataForNavalAutoTravel);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00018300 File Offset: 0x00016500
		internal static object AutoGeneratedGetMemberValueEnabledCheatsBefore(object o)
		{
			return ((Campaign)o).EnabledCheatsBefore;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00018312 File Offset: 0x00016512
		internal static object AutoGeneratedGetMemberValuePlatformID(object o)
		{
			return ((Campaign)o).PlatformID;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001831F File Offset: 0x0001651F
		internal static object AutoGeneratedGetMemberValueUniqueGameId(object o)
		{
			return ((Campaign)o).UniqueGameId;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001832C File Offset: 0x0001652C
		internal static object AutoGeneratedGetMemberValueCampaignObjectManager(object o)
		{
			return ((Campaign)o).CampaignObjectManager;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00018339 File Offset: 0x00016539
		internal static object AutoGeneratedGetMemberValueIsCraftingEnabled(object o)
		{
			return ((Campaign)o).IsCraftingEnabled;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0001834B File Offset: 0x0001654B
		internal static object AutoGeneratedGetMemberValueIsBannerEditorEnabled(object o)
		{
			return ((Campaign)o).IsBannerEditorEnabled;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001835D File Offset: 0x0001655D
		internal static object AutoGeneratedGetMemberValueIsFaceGenEnabled(object o)
		{
			return ((Campaign)o).IsFaceGenEnabled;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001836F File Offset: 0x0001656F
		internal static object AutoGeneratedGetMemberValueQuestManager(object o)
		{
			return ((Campaign)o).QuestManager;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001837C File Offset: 0x0001657C
		internal static object AutoGeneratedGetMemberValueIssueManager(object o)
		{
			return ((Campaign)o).IssueManager;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00018389 File Offset: 0x00016589
		internal static object AutoGeneratedGetMemberValueIncidentManager(object o)
		{
			return ((Campaign)o).IncidentManager;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00018396 File Offset: 0x00016596
		internal static object AutoGeneratedGetMemberValueFactionManager(object o)
		{
			return ((Campaign)o).FactionManager;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000183A3 File Offset: 0x000165A3
		internal static object AutoGeneratedGetMemberValueCharacterRelationManager(object o)
		{
			return ((Campaign)o).CharacterRelationManager;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000183B0 File Offset: 0x000165B0
		internal static object AutoGeneratedGetMemberValueRomance(object o)
		{
			return ((Campaign)o).Romance;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000183BD File Offset: 0x000165BD
		internal static object AutoGeneratedGetMemberValuePlayerCaptivity(object o)
		{
			return ((Campaign)o).PlayerCaptivity;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000183CA File Offset: 0x000165CA
		internal static object AutoGeneratedGetMemberValuePlayerDefaultFaction(object o)
		{
			return ((Campaign)o).PlayerDefaultFaction;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000183D7 File Offset: 0x000165D7
		internal static object AutoGeneratedGetMemberValueMapStateData(object o)
		{
			return ((Campaign)o).MapStateData;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000183E4 File Offset: 0x000165E4
		internal static object AutoGeneratedGetMemberValueMapTimeTracker(object o)
		{
			return ((Campaign)o).MapTimeTracker;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000183F1 File Offset: 0x000165F1
		internal static object AutoGeneratedGetMemberValueGameMode(object o)
		{
			return ((Campaign)o).GameMode;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00018403 File Offset: 0x00016603
		internal static object AutoGeneratedGetMemberValuePlayerProgress(object o)
		{
			return ((Campaign)o).PlayerProgress;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00018415 File Offset: 0x00016615
		internal static object AutoGeneratedGetMemberValueSiegeEventManager(object o)
		{
			return ((Campaign)o).SiegeEventManager;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00018422 File Offset: 0x00016622
		internal static object AutoGeneratedGetMemberValueMapEventManager(object o)
		{
			return ((Campaign)o).MapEventManager;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001842F File Offset: 0x0001662F
		internal static object AutoGeneratedGetMemberValueMapTrackerManager(object o)
		{
			return ((Campaign)o).MapTrackerManager;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001843C File Offset: 0x0001663C
		internal static object AutoGeneratedGetMemberValue_curMapFrame(object o)
		{
			return ((Campaign)o)._curMapFrame;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0001844E File Offset: 0x0001664E
		internal static object AutoGeneratedGetMemberValuePlayerEncounter(object o)
		{
			return ((Campaign)o).PlayerEncounter;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001845B File Offset: 0x0001665B
		internal static object AutoGeneratedGetMemberValueBarterManager(object o)
		{
			return ((Campaign)o).BarterManager;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00018468 File Offset: 0x00016668
		internal static object AutoGeneratedGetMemberValueIsMainHeroDisguised(object o)
		{
			return ((Campaign)o).IsMainHeroDisguised;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001847A File Offset: 0x0001667A
		internal static object AutoGeneratedGetMemberValueMainParty(object o)
		{
			return ((Campaign)o).MainParty;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00018487 File Offset: 0x00016687
		internal static object AutoGeneratedGetMemberValueCampaignInformationManager(object o)
		{
			return ((Campaign)o).CampaignInformationManager;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00018494 File Offset: 0x00016694
		internal static object AutoGeneratedGetMemberValueVisualTrackerManager(object o)
		{
			return ((Campaign)o).VisualTrackerManager;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000184A1 File Offset: 0x000166A1
		internal static object AutoGeneratedGetMemberValuePlayerTraitDeveloper(object o)
		{
			return ((Campaign)o).PlayerTraitDeveloper;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000184AE File Offset: 0x000166AE
		internal static object AutoGeneratedGetMemberValuePlayerDataForNavalAutoTravel(object o)
		{
			return ((Campaign)o).PlayerDataForNavalAutoTravel;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000184BB File Offset: 0x000166BB
		internal static object AutoGeneratedGetMemberValueOptions(object o)
		{
			return ((Campaign)o).Options;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000184C8 File Offset: 0x000166C8
		internal static object AutoGeneratedGetMemberValueTournamentManager(object o)
		{
			return ((Campaign)o).TournamentManager;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000184D5 File Offset: 0x000166D5
		internal static object AutoGeneratedGetMemberValueIsSinglePlayerReferencesInitialized(object o)
		{
			return ((Campaign)o).IsSinglePlayerReferencesInitialized;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000184E7 File Offset: 0x000166E7
		internal static object AutoGeneratedGetMemberValueLastTimeControlMode(object o)
		{
			return ((Campaign)o).LastTimeControlMode;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000184F9 File Offset: 0x000166F9
		internal static object AutoGeneratedGetMemberValueMainHeroIllDays(object o)
		{
			return ((Campaign)o).MainHeroIllDays;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001850B File Offset: 0x0001670B
		internal static object AutoGeneratedGetMemberValueUnlockedFigureheadsByMainHero(object o)
		{
			return ((Campaign)o).UnlockedFigureheadsByMainHero;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00018518 File Offset: 0x00016718
		internal static object AutoGeneratedGetMemberValueKingdomManager(object o)
		{
			return ((Campaign)o).KingdomManager;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00018525 File Offset: 0x00016725
		internal static object AutoGeneratedGetMemberValue_campaignPeriodicEventManager(object o)
		{
			return ((Campaign)o)._campaignPeriodicEventManager;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00018532 File Offset: 0x00016732
		internal static object AutoGeneratedGetMemberValue_isMainPartyWaiting(object o)
		{
			return ((Campaign)o)._isMainPartyWaiting;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00018544 File Offset: 0x00016744
		internal static object AutoGeneratedGetMemberValue_newGameVersion(object o)
		{
			return ((Campaign)o)._newGameVersion;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00018551 File Offset: 0x00016751
		internal static object AutoGeneratedGetMemberValue_previouslyUsedModules(object o)
		{
			return ((Campaign)o)._previouslyUsedModules;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0001855E File Offset: 0x0001675E
		internal static object AutoGeneratedGetMemberValue_campaignMapSceneXmlCrc(object o)
		{
			return ((Campaign)o)._campaignMapSceneXmlCrc;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00018570 File Offset: 0x00016770
		internal static object AutoGeneratedGetMemberValue_campaignMapSceneNavigationMeshCrc(object o)
		{
			return ((Campaign)o)._campaignMapSceneNavigationMeshCrc;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00018582 File Offset: 0x00016782
		internal static object AutoGeneratedGetMemberValue_usedGameVersions(object o)
		{
			return ((Campaign)o)._usedGameVersions;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001858F File Offset: 0x0001678F
		internal static object AutoGeneratedGetMemberValue_campaignBehaviorManager(object o)
		{
			return ((Campaign)o)._campaignBehaviorManager;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001859C File Offset: 0x0001679C
		internal static object AutoGeneratedGetMemberValue_customManagers(object o)
		{
			return ((Campaign)o)._customManagers;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000185A9 File Offset: 0x000167A9
		internal static object AutoGeneratedGetMemberValue_lastPartyIndex(object o)
		{
			return ((Campaign)o)._lastPartyIndex;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000185BB File Offset: 0x000167BB
		internal static object AutoGeneratedGetMemberValue_cameraFollowParty(object o)
		{
			return ((Campaign)o)._cameraFollowParty;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000185C8 File Offset: 0x000167C8
		internal static object AutoGeneratedGetMemberValue_logEntryHistory(object o)
		{
			return ((Campaign)o)._logEntryHistory;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000185D5 File Offset: 0x000167D5
		internal static object AutoGeneratedGetMemberValue_playerFormationPreferences(object o)
		{
			return ((Campaign)o)._playerFormationPreferences;
		}

		// Token: 0x0400003A RID: 58
		public const float ConfigTimeMultiplier = 0.25f;

		// Token: 0x0400003B RID: 59
		private EntitySystem<CampaignEntityComponent> _campaignEntitySystem;

		// Token: 0x04000041 RID: 65
		public static int PlayerRegionSwitchCostFromLandToSea;

		// Token: 0x04000042 RID: 66
		public static int PathFindingMaxCostLimit;

		// Token: 0x04000043 RID: 67
		public ITask CampaignLateAITickTask;

		// Token: 0x04000044 RID: 68
		[SaveableField(210)]
		private CampaignPeriodicEventManager _campaignPeriodicEventManager;

		// Token: 0x04000045 RID: 69
		private Dictionary<MobileParty.NavigationType, float> _averageDistanceBetweenClosestTwoTowns;

		// Token: 0x04000047 RID: 71
		[SaveableField(53)]
		private bool _isMainPartyWaiting;

		// Token: 0x04000048 RID: 72
		[SaveableField(344)]
		private string _newGameVersion;

		// Token: 0x04000049 RID: 73
		[SaveableField(78)]
		private MBList<string> _previouslyUsedModules;

		// Token: 0x0400004A RID: 74
		[SaveableField(85)]
		private uint _campaignMapSceneXmlCrc;

		// Token: 0x0400004B RID: 75
		[SaveableField(86)]
		private uint _campaignMapSceneNavigationMeshCrc;

		// Token: 0x0400004C RID: 76
		[SaveableField(81)]
		private MBList<string> _usedGameVersions;

		// Token: 0x04000052 RID: 82
		[SaveableField(7)]
		private ICampaignBehaviorManager _campaignBehaviorManager;

		// Token: 0x04000054 RID: 84
		private CampaignTickCacheDataStore _tickData;

		// Token: 0x04000055 RID: 85
		[SaveableField(2)]
		public readonly CampaignOptions Options;

		// Token: 0x04000056 RID: 86
		public MBReadOnlyDictionary<CharacterObject, FormationClass> PlayerFormationPreferences;

		// Token: 0x04000057 RID: 87
		[SaveableField(13)]
		public ITournamentManager TournamentManager;

		// Token: 0x04000058 RID: 88
		public float MinSettlementX;

		// Token: 0x04000059 RID: 89
		public float MaxSettlementX;

		// Token: 0x0400005A RID: 90
		public float MinSettlementY;

		// Token: 0x0400005B RID: 91
		public float MaxSettlementY;

		// Token: 0x0400005C RID: 92
		[SaveableField(27)]
		public bool IsSinglePlayerReferencesInitialized;

		// Token: 0x0400005D RID: 93
		private LocatorGrid<MobileParty> _mobilePartyLocator;

		// Token: 0x0400005E RID: 94
		private LocatorGrid<Settlement> _settlementLocator;

		// Token: 0x0400005F RID: 95
		private GameModels _gameModels;

		// Token: 0x04000062 RID: 98
		[SaveableField(31)]
		public CampaignTimeControlMode LastTimeControlMode = CampaignTimeControlMode.UnstoppablePlay;

		// Token: 0x04000063 RID: 99
		private IMapScene _mapSceneWrapper;

		// Token: 0x04000064 RID: 100
		public bool GameStarted;

		// Token: 0x04000066 RID: 102
		private Campaign.GameLoadingType _gameLoadingType;

		// Token: 0x04000067 RID: 103
		public ConversationContext CurrentConversationContext;

		// Token: 0x04000068 RID: 104
		[CachedData]
		private float _dt;

		// Token: 0x0400006C RID: 108
		private CampaignTimeControlMode _timeControlMode;

		// Token: 0x0400006D RID: 109
		public int CurrentTickCount;

		// Token: 0x0400009D RID: 157
		[SaveableField(30)]
		public int MainHeroIllDays = -1;

		// Token: 0x040000A8 RID: 168
		private bool _wasPlayerAnchorMovingToPoint;

		// Token: 0x040000AB RID: 171
		[SaveableField(42)]
		private List<ICustomSystemManager> _customManagers = new List<ICustomSystemManager>();

		// Token: 0x040000AF RID: 175
		private MBCampaignEvent _dailyTickEvent;

		// Token: 0x040000B0 RID: 176
		private MBCampaignEvent _hourlyTickEvent;

		// Token: 0x040000B1 RID: 177
		private MBCampaignEvent _QuarterHourlyTickEvent;

		// Token: 0x040000B3 RID: 179
		[CachedData]
		private int _lastNonZeroDtFrame;

		// Token: 0x040000B4 RID: 180
		public int DefaultWeatherNodeDimension;

		// Token: 0x040000BD RID: 189
		[SaveableField(333)]
		public List<Figurehead> UnlockedFigureheadsByMainHero = new List<Figurehead>();

		// Token: 0x040000BE RID: 190
		private MBList<Town> _towns;

		// Token: 0x040000BF RID: 191
		private MBList<Town> _castles;

		// Token: 0x040000C0 RID: 192
		private MBList<Village> _villages;

		// Token: 0x040000C1 RID: 193
		private MBList<Hideout> _hideouts;

		// Token: 0x040000C2 RID: 194
		private MBReadOnlyList<CharacterObject> _characters;

		// Token: 0x040000C3 RID: 195
		private MBReadOnlyList<WorkshopType> _workshops;

		// Token: 0x040000C4 RID: 196
		private MBReadOnlyList<ItemModifier> _itemModifiers;

		// Token: 0x040000C5 RID: 197
		private MBReadOnlyList<Concept> _concepts;

		// Token: 0x040000C6 RID: 198
		private MBReadOnlyList<ItemModifierGroup> _itemModifierGroups;

		// Token: 0x040000C7 RID: 199
		[SaveableField(79)]
		private int _lastPartyIndex;

		// Token: 0x040000C9 RID: 201
		[SaveableField(61)]
		private PartyBase _cameraFollowParty;

		// Token: 0x040000CC RID: 204
		[SaveableField(64)]
		private readonly LogEntryHistory _logEntryHistory = new LogEntryHistory();

		// Token: 0x040000CF RID: 207
		[SaveableField(65)]
		public KingdomManager KingdomManager;

		// Token: 0x040000D1 RID: 209
		[SaveableField(77)]
		private Dictionary<CharacterObject, FormationClass> _playerFormationPreferences;

		// Token: 0x02000523 RID: 1315
		[Flags]
		public enum PartyRestFlags : uint
		{
			// Token: 0x04001694 RID: 5780
			None = 0U,
			// Token: 0x04001695 RID: 5781
			SafeMode = 1U
		}

		// Token: 0x02000524 RID: 1316
		public enum GameLoadingType
		{
			// Token: 0x04001697 RID: 5783
			Tutorial,
			// Token: 0x04001698 RID: 5784
			NewCampaign,
			// Token: 0x04001699 RID: 5785
			SavedCampaign,
			// Token: 0x0400169A RID: 5786
			Editor
		}
	}
}
