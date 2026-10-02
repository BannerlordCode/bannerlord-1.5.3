using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C1 RID: 961
	public class DefaultPerks
	{
		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x000E11ED File Offset: 0x000DF3ED
		private static DefaultPerks Instance
		{
			get
			{
				return Campaign.Current.DefaultPerks;
			}
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x000E11F9 File Offset: 0x000DF3F9
		public DefaultPerks()
		{
			this.RegisterAll();
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000E1208 File Offset: 0x000DF408
		private void RegisterAll()
		{
			this._oneHandedWrappedHandles = DefaultPerks.Create("OneHandedWrappedHandles");
			this._oneHandedBasher = DefaultPerks.Create("OneHandedBasher");
			this._oneHandedToBeBlunt = DefaultPerks.Create("OneHandedToBeBlunt");
			this._oneHandedSwiftStrike = DefaultPerks.Create("OneHandedSwiftStrike");
			this._oneHandedCavalry = DefaultPerks.Create("OneHandedCavalry");
			this._oneHandedShieldBearer = DefaultPerks.Create("OneHandedShieldBearer");
			this._oneHandedTrainer = DefaultPerks.Create("OneHandedTrainer");
			this._oneHandedDuelist = DefaultPerks.Create("OneHandedDuelist");
			this._oneHandedShieldWall = DefaultPerks.Create("OneHandedShieldWall");
			this._oneHandedArrowCatcher = DefaultPerks.Create("OneHandedArrowCatcher");
			this._oneHandedMilitaryTradition = DefaultPerks.Create("OneHandedMilitaryTradition");
			this._oneHandedCorpsACorps = DefaultPerks.Create("OneHandedCorpsACorps");
			this._oneHandedStandUnited = DefaultPerks.Create("OneHandedStandUnited");
			this._oneHandedLeadByExample = DefaultPerks.Create("OneHandedLeadByExample");
			this._oneHandedSteelCoreShields = DefaultPerks.Create("OneHandedSteelCoreShields");
			this._oneHandedFleetOfFoot = DefaultPerks.Create("OneHandedFleetOfFoot");
			this._oneHandedDeadlyPurpose = DefaultPerks.Create("OneHandedDeadlyPurpose");
			this._oneHandedUnwaveringDefense = DefaultPerks.Create("OneHandedUnwaveringDefense");
			this._oneHandedPrestige = DefaultPerks.Create("OneHandedPrestige");
			this._oneHandedChinkInTheArmor = DefaultPerks.Create("OneHandedChinkInTheArmor");
			this._oneHandedWayOfTheSword = DefaultPerks.Create("OneHandedWayOfTheSword");
			this._twoHandedStrongGrip = DefaultPerks.Create("TwoHandedStrongGrip");
			this._twoHandedWoodChopper = DefaultPerks.Create("TwoHandedWoodChopper");
			this._twoHandedOnTheEdge = DefaultPerks.Create("TwoHandedOnTheEdge");
			this._twoHandedHeadBasher = DefaultPerks.Create("TwoHandedHeadBasher");
			this._twoHandedShowOfStrength = DefaultPerks.Create("TwoHandedShowOfStrength");
			this._twoHandedBaptisedInBlood = DefaultPerks.Create("TwoHandedBaptisedInBlood");
			this._twoHandedBeastSlayer = DefaultPerks.Create("TwoHandedBeastSlayer");
			this._twoHandedShieldBreaker = DefaultPerks.Create("TwoHandedShieldBreaker");
			this._twoHandedBerserker = DefaultPerks.Create("TwoHandedBerserker");
			this._twoHandedConfidence = DefaultPerks.Create("TwoHandedConfidence");
			this._twoHandedProjectileDeflection = DefaultPerks.Create("TwoHandedProjectileDeflection");
			this._twoHandedTerror = DefaultPerks.Create("TwoHandedTerror");
			this._twoHandedHope = DefaultPerks.Create("TwoHandedHope");
			this._twoHandedRecklessCharge = DefaultPerks.Create("TwoHandedRecklessCharge");
			this._twoHandedThickHides = DefaultPerks.Create("TwoHandedThickHides");
			this._twoHandedBladeMaster = DefaultPerks.Create("TwoHandedBladeMaster");
			this._twoHandedVandal = DefaultPerks.Create("TwoHandedVandal");
			this._twoHandedWayOfTheGreatAxe = DefaultPerks.Create("TwoHandedWayOfTheGreatAxe");
			this._polearmPikeman = DefaultPerks.Create("PolearmPikeman");
			this._polearmCavalry = DefaultPerks.Create("PolearmCavalry");
			this._polearmBraced = DefaultPerks.Create("PolearmBraced");
			this._polearmKeepAtBay = DefaultPerks.Create("PolearmKeepAtBay");
			this._polearmSwiftSwing = DefaultPerks.Create("PolearmSwiftSwing");
			this._polearmCleanThrust = DefaultPerks.Create("PolearmCleanThrust");
			this._polearmFootwork = DefaultPerks.Create("PolearmFootwork");
			this._polearmHardKnock = DefaultPerks.Create("PolearmHardKnock");
			this._polearmSteedKiller = DefaultPerks.Create("PolearmSteadKiller");
			this._polearmLancer = DefaultPerks.Create("PolearmLancer");
			this._polearmSkewer = DefaultPerks.Create("PolearmSkewer");
			this._polearmGuards = DefaultPerks.Create("PolearmGuards");
			this._polearmStandardBearer = DefaultPerks.Create("PolearmStandardBearer");
			this._polearmPhalanx = DefaultPerks.Create("PolearmPhalanx");
			this._polearmHardyFrontline = DefaultPerks.Create("PolearmHardyFrontline");
			this._polearmDrills = DefaultPerks.Create("PolearmDrills");
			this._polearmSureFooted = DefaultPerks.Create("PolearmSureFooted");
			this._polearmUnstoppableForce = DefaultPerks.Create("PolearmUnstoppableForce");
			this._polearmCounterweight = DefaultPerks.Create("PolearmCounterweight");
			this._polearmSharpenTheTip = DefaultPerks.Create("PolearmSharpenTheTip");
			this._polearmWayOfTheSpear = DefaultPerks.Create("PolearmWayOfTheSpear");
			this._bowBowControl = DefaultPerks.Create("BowBowControl");
			this._bowDeadAim = DefaultPerks.Create("BowDeadAim");
			this._bowBodkin = DefaultPerks.Create("BowBodkin");
			this._bowRangersSwiftness = DefaultPerks.Create("BowRangersSwiftness");
			this._bowRapidFire = DefaultPerks.Create("BowRapidFire");
			this._bowQuickAdjustments = DefaultPerks.Create("BowQuickAdjustments");
			this._bowMerryMen = DefaultPerks.Create("BowMerryMen");
			this._bowMountedArchery = DefaultPerks.Create("BowMountedArchery");
			this._bowTrainer = DefaultPerks.Create("BowTrainer");
			this._bowStrongBows = DefaultPerks.Create("BowStrongBows");
			this._bowDiscipline = DefaultPerks.Create("BowDiscipline");
			this._bowHunterClan = DefaultPerks.Create("BowHunterClan");
			this._bowSkirmishPhaseMaster = DefaultPerks.Create("BowSkirmishPhaseMaster");
			this._bowEagleEye = DefaultPerks.Create("BowEagleEye");
			this._bowBullsEye = DefaultPerks.Create("BowBullsEye");
			this._bowRenownedArcher = DefaultPerks.Create("BowRenownedArcher");
			this._bowHorseMaster = DefaultPerks.Create("BowHorseMaster");
			this._bowDeepQuivers = DefaultPerks.Create("BowDeepQuivers");
			this._bowQuickDraw = DefaultPerks.Create("BowQuickDraw");
			this._bowNockingPoint = DefaultPerks.Create("BowNockingPoint");
			this._bowDeadshot = DefaultPerks.Create("BowDeadshot");
			this._crossbowPiercer = DefaultPerks.Create("CrossbowPiercer");
			this._crossbowMarksmen = DefaultPerks.Create("CrossbowMarksmen");
			this._crossbowUnhorser = DefaultPerks.Create("CrossbowUnhorser");
			this._crossbowWindWinder = DefaultPerks.Create("CrossbowWindWinder");
			this._crossbowDonkeysSwiftness = DefaultPerks.Create("CrossbowDonkeysSwiftness");
			this._crossbowSheriff = DefaultPerks.Create("CrossbowSheriff");
			this._crossbowPeasantLeader = DefaultPerks.Create("CrossbowPeasantLeader");
			this._crossbowRenownMarksmen = DefaultPerks.Create("CrossbowRenownMarksmen");
			this._crossbowFletcher = DefaultPerks.Create("CrossbowFletcher");
			this._crossbowPuncture = DefaultPerks.Create("CrossbowPuncture");
			this._crossbowLooseAndMove = DefaultPerks.Create("CrossbowLooseAndMove");
			this._crossbowDeftHands = DefaultPerks.Create("CrossbowDeftHands");
			this._crossbowCounterFire = DefaultPerks.Create("CrossbowCounterFire");
			this._crossbowMountedCrossbowman = DefaultPerks.Create("CrossbowMountedCrossbowman");
			this._crossbowSteady = DefaultPerks.Create("CrossbowSteady");
			this._crossbowLongShots = DefaultPerks.Create("CrossbowLongShots");
			this._crossbowHammerBolts = DefaultPerks.Create("CrossbowHammerBolts");
			this._crossbowPavise = DefaultPerks.Create("CrossbowPavise");
			this._crossbowTerror = DefaultPerks.Create("CrossbowTerror");
			this._crossbowPickedShots = DefaultPerks.Create("CrossbowBoltenGuard");
			this._crossbowMightyPull = DefaultPerks.Create("CrossbowMightyPull");
			this._throwingQuickDraw = DefaultPerks.Create("ThrowingQuickDraw");
			this._throwingShieldBreaker = DefaultPerks.Create("ThrowingShieldBreaker");
			this._throwingHunter = DefaultPerks.Create("ThrowingHunter");
			this._throwingFlexibleFighter = DefaultPerks.Create("ThrowingFlexibleFighter");
			this._throwingMountedSkirmisher = DefaultPerks.Create("ThrowingMountedSkirmisher");
			this._throwingPerfectTechnique = DefaultPerks.Create("ThrowingPerfectTechnique");
			this._throwingRunningThrow = DefaultPerks.Create("ThrowingRunningThrow");
			this._throwingKnockOff = DefaultPerks.Create("ThrowingKnockOff");
			this._throwingSkirmisher = DefaultPerks.Create("ThrowingSkirmisher");
			this._throwingWellPrepared = DefaultPerks.Create("ThrowingWellPrepared");
			this._throwingFocus = DefaultPerks.Create("ThrowingFocus");
			this._throwingLastHit = DefaultPerks.Create("ThrowingLastHit");
			this._throwingHeadHunter = DefaultPerks.Create("ThrowingHeadHunter");
			this._throwingSlingingCompetitions = DefaultPerks.Create("ThrowingSlingingCompetitions");
			this._throwingSaddlebags = DefaultPerks.Create("ThrowingSaddlebags");
			this._throwingSplinters = DefaultPerks.Create("ThrowingSplinters");
			this._throwingResourceful = DefaultPerks.Create("ThrowingResourceful");
			this._throwingLongReach = DefaultPerks.Create("ThrowingLongReach");
			this._throwingWeakSpot = DefaultPerks.Create("ThrowingWeakSpot");
			this._throwingImpale = DefaultPerks.Create("ThrowingImpale");
			this._throwingUnstoppableForce = DefaultPerks.Create("ThrowingUnstoppableForce");
			this._ridingFullSpeed = DefaultPerks.Create("RidingFullSpeed");
			this._ridingNimbleSteed = DefaultPerks.Create("RidingNimbleStead");
			this._ridingWellStraped = DefaultPerks.Create("RidingWellStraped");
			this._ridingVeterinary = DefaultPerks.Create("RidingVeterinary");
			this._ridingNomadicTraditions = DefaultPerks.Create("RidingNomadicTraditions");
			this._ridingDeeperSacks = DefaultPerks.Create("RidingDeeperSacks");
			this._ridingSagittarius = DefaultPerks.Create("RidingSagittarius");
			this._ridingSweepingWind = DefaultPerks.Create("RidingSweepingWind");
			this._ridingReliefForce = DefaultPerks.Create("RidingReliefForce");
			this._ridingMountedWarrior = DefaultPerks.Create("RidingMountedWarrior");
			this._ridingHorseArcher = DefaultPerks.Create("RidingHorseArcher");
			this._ridingShepherd = DefaultPerks.Create("RidingShepherd");
			this._ridingBreeder = DefaultPerks.Create("RidingBreeder");
			this._ridingThunderousCharge = DefaultPerks.Create("RidingThunderousCharge");
			this._ridingAnnoyingBuzz = DefaultPerks.Create("RidingAnnoyingBuzz");
			this._ridingMountedPatrols = DefaultPerks.Create("RidingMountedPatrols");
			this._ridingCavalryTactics = DefaultPerks.Create("RidingCavalryTactics");
			this._ridingDauntlessSteed = DefaultPerks.Create("RidingDauntlessSteed");
			this._ridingToughSteed = DefaultPerks.Create("RidingToughSteed");
			this._ridingTheWayOfTheSaddle = DefaultPerks.Create("RidingTheWayOfTheSaddle");
			this._athleticsMorningExercise = DefaultPerks.Create("AthleticsMorningExercise");
			this._athleticsWellBuilt = DefaultPerks.Create("AthleticsWellBuilt");
			this._athleticsFury = DefaultPerks.Create("AthleticsFury");
			this._athleticsFormFittingArmor = DefaultPerks.Create("AthleticsFormFittingArmor");
			this._athleticsImposingStature = DefaultPerks.Create("AthleticsImposingStature");
			this._athleticsStamina = DefaultPerks.Create("AthleticsStamina");
			this._athleticsSprint = DefaultPerks.Create("AthleticsSprint");
			this._athleticsPowerful = DefaultPerks.Create("AthleticsPowerful");
			this._athleticsSurgingBlow = DefaultPerks.Create("AthleticsSurgingBlow");
			this._athleticsBraced = DefaultPerks.Create("AthleticsBraced");
			this._athleticsWalkItOff = DefaultPerks.Create("AthleticsWalkItOff");
			this._athleticsAGoodDaysRest = DefaultPerks.Create("AthleticsAGoodDaysRest");
			this._athleticsDurable = DefaultPerks.Create("AthleticsDurable");
			this._athleticsEnergetic = DefaultPerks.Create("AthleticsEnergetic");
			this._athleticsSteady = DefaultPerks.Create("AthleticsSteady");
			this._athleticsStrong = DefaultPerks.Create("AthleticsStrong");
			this._athleticsStrongLegs = DefaultPerks.Create("AthleticsStrongLegs");
			this._athleticsStrongArms = DefaultPerks.Create("AthleticsStrongArms");
			this._athleticsSpartan = DefaultPerks.Create("AthleticsSpartan");
			this._athleticsIgnorePain = DefaultPerks.Create("AthleticsIgnorePain");
			this._athleticsMightyBlow = DefaultPerks.Create("AthleticsMightyBlow");
			this._craftingSharpenedEdge = DefaultPerks.Create("CraftingSharpenedEdge");
			this._craftingSharpenedTip = DefaultPerks.Create("CraftingSharpenedTip");
			this._craftingIronMaker = DefaultPerks.Create("IronYield");
			this._craftingCharcoalMaker = DefaultPerks.Create("CharcoalYield");
			this._craftingSteelMaker = DefaultPerks.Create("SteelMaker");
			this._craftingSteelMaker2 = DefaultPerks.Create("SteelMaker2");
			this._craftingSteelMaker3 = DefaultPerks.Create("SteelMaker3");
			this._craftingCuriousSmelter = DefaultPerks.Create("CuriousSmelter");
			this._craftingCuriousSmith = DefaultPerks.Create("CuriousSmith");
			this._craftingPracticalRefiner = DefaultPerks.Create("PracticalRefiner");
			this._craftingPracticalSmelter = DefaultPerks.Create("PracticalSmelter");
			this._craftingPracticalSmith = DefaultPerks.Create("PracticalSmith");
			this._craftingArtisanSmith = DefaultPerks.Create("ArtisanSmith");
			this._craftingExperiencedSmith = DefaultPerks.Create("ExperiencedSmith");
			this._craftingMasterSmith = DefaultPerks.Create("MasterSmith");
			this._craftingLegendarySmith = DefaultPerks.Create("LegendarySmith");
			this._craftingVigorousSmith = DefaultPerks.Create("VigorousSmith");
			this._craftingStrongSmith = DefaultPerks.Create("StrongSmith");
			this._craftingEnduringSmith = DefaultPerks.Create("EnduringSmith");
			this._craftingFencerSmith = DefaultPerks.Create("WeaponMasterSmith");
			this._tacticsTightFormations = DefaultPerks.Create("TacticsTightFormations");
			this._tacticsLooseFormations = DefaultPerks.Create("TacticsLooseFormations");
			this._tacticsExtendedSkirmish = DefaultPerks.Create("TacticsExtendedSkirmish");
			this._tacticsDecisiveBattle = DefaultPerks.Create("TacticsDecisiveBattle");
			this._tacticsSmallUnitTactics = DefaultPerks.Create("TacticsSmallUnitTactics");
			this._tacticsHordeLeader = DefaultPerks.Create("TacticsHordeLeader");
			this._tacticsLawKeeper = DefaultPerks.Create("TacticsLawkeeper");
			this._tacticsCoaching = DefaultPerks.Create("TacticsCoaching");
			this._tacticsSwiftRegroup = DefaultPerks.Create("TacticsSwiftRegroup");
			this._tacticsImproviser = DefaultPerks.Create("TacticsImproviser");
			this._tacticsOnTheMarch = DefaultPerks.Create("TacticsOnTheMarch");
			this._tacticsCallToArms = DefaultPerks.Create("TacticsCallToArms");
			this._tacticsPickThemOfTheWalls = DefaultPerks.Create("TacticsPickThemOfTheWalls");
			this._tacticsMakeThemPay = DefaultPerks.Create("TacticsMakeThemPay");
			this._tacticsEliteReserves = DefaultPerks.Create("TacticsEliteReserves");
			this._tacticsEncirclement = DefaultPerks.Create("TacticsEncirclement");
			this._tacticsPreBattleManeuvers = DefaultPerks.Create("TacticsPreBattleManeuvers");
			this._tacticsBesieged = DefaultPerks.Create("TacticsBesieged");
			this._tacticsCounteroffensive = DefaultPerks.Create("TacticsCounteroffensive");
			this._tacticsGensdarmes = DefaultPerks.Create("TacticsGensdarmes");
			this._tacticsTacticalMastery = DefaultPerks.Create("TacticsTacticalMastery");
			this._scoutingDayTraveler = DefaultPerks.Create("ScoutingDayTraveler");
			this._scoutingNightRunner = DefaultPerks.Create("ScoutingNightRunner");
			this._scoutingPathfinder = DefaultPerks.Create("ScoutingPathfinder");
			this._scoutingWaterDiviner = DefaultPerks.Create("ScoutingWaterDiviner");
			this._scoutingForestKin = DefaultPerks.Create("ScoutingForestKin");
			this._scoutingDesertBorn = DefaultPerks.Create("ScoutingDesertBorn");
			this._scoutingForcedMarch = DefaultPerks.Create("ScoutingForcedMarch");
			this._scoutingUnburdened = DefaultPerks.Create("ScoutingUnburdened");
			this._scoutingTracker = DefaultPerks.Create("ScoutingTracker");
			this._scoutingRanger = DefaultPerks.Create("ScoutingRanger");
			this._scoutingMountedScouts = DefaultPerks.Create("ScoutingMountedScouts");
			this._scoutingPatrols = DefaultPerks.Create("ScoutingPatrols");
			this._scoutingForagers = DefaultPerks.Create("ScoutingForagers");
			this._scoutingBeastWhisperer = DefaultPerks.Create("ScoutingBeastWhisperer");
			this._scoutingVillageNetwork = DefaultPerks.Create("ScoutingVillageNetwork");
			this._scoutingRumourNetwork = DefaultPerks.Create("ScoutingRumourNetwork");
			this._scoutingVantagePoint = DefaultPerks.Create("ScoutingVantagePoint");
			this._scoutingKeenSight = DefaultPerks.Create("ScoutingKeenSight");
			this._scoutingVanguard = DefaultPerks.Create("ScoutingVanguard");
			this._scoutingRearguard = DefaultPerks.Create("ScoutingRearguard");
			this._scoutingUncannyInsight = DefaultPerks.Create("ScoutingUncannyInsight");
			this._rogueryNoRestForTheWicked = DefaultPerks.Create("RogueryNoRestForTheWicked");
			this._roguerySweetTalker = DefaultPerks.Create("RoguerySweetTalker");
			this._rogueryTwoFaced = DefaultPerks.Create("RogueryTwoFaced");
			this._rogueryDeepPockets = DefaultPerks.Create("RogueryDeepPockets");
			this._rogueryInBestLight = DefaultPerks.Create("RogueryInBestLight");
			this._rogueryKnowHow = DefaultPerks.Create("RogueryKnowHow");
			this._rogueryPromises = DefaultPerks.Create("RogueryPromises");
			this._rogueryManhunter = DefaultPerks.Create("RogueryManhunter");
			this._rogueryScarface = DefaultPerks.Create("RogueryScarface");
			this._rogueryWhiteLies = DefaultPerks.Create("RogueryWhiteLies");
			this._roguerySmugglerConnections = DefaultPerks.Create("RoguerySmugglerConnections");
			this._rogueryPartnersInCrime = DefaultPerks.Create("RogueryPartnersInCrime");
			this._rogueryOneOfTheFamily = DefaultPerks.Create("RogueryOneOfTheFamily");
			this._roguerySaltTheEarth = DefaultPerks.Create("RoguerySaltTheEarth");
			this._rogueryCarver = DefaultPerks.Create("RogueryCarver");
			this._rogueryRansomBroker = DefaultPerks.Create("RogueryRansomBroker");
			this._rogueryArmsDealer = DefaultPerks.Create("RogueryArmsDealer");
			this._rogueryDirtyFighting = DefaultPerks.Create("RogueryDirtyFighting");
			this._rogueryDashAndSlash = DefaultPerks.Create("RogueryDashAndSlash");
			this._rogueryFleetFooted = DefaultPerks.Create("RogueryFleetFooted");
			this._rogueryRogueExtraordinaire = DefaultPerks.Create("RogueryRogueExtraordinaire");
			this._leadershipCombatTips = DefaultPerks.Create("LeadershipCombatTips");
			this._leadershipRaiseTheMeek = DefaultPerks.Create("LeadershipRaiseTheMeek");
			this._leadershipFerventAttacker = DefaultPerks.Create("LeadershipFerventAttacker");
			this._leadershipStoutDefender = DefaultPerks.Create("LeadershipStoutDefender");
			this._leadershipAuthority = DefaultPerks.Create("LeadershipAuthority");
			this._leadershipHeroicLeader = DefaultPerks.Create("LeadershipHeroicLeader");
			this._leadershipLoyaltyAndHonor = DefaultPerks.Create("LeadershipLoyaltyAndHonor");
			this._leadershipFamousCommander = DefaultPerks.Create("LeadershipFamousCommander");
			this._leadershipPresence = DefaultPerks.Create("LeadershipPresence");
			this._leadershipLeaderOfTheMasses = DefaultPerks.Create("LeadershipLeaderOfMasses");
			this._leadershipVeteransRespect = DefaultPerks.Create("LeadershipVeteransRespect");
			this._leadershipCitizenMilitia = DefaultPerks.Create("LeadershipCitizenMilitia");
			this._leadershipInspiringLeader = DefaultPerks.Create("LeadershipInspiringLeader");
			this._leadershipUpliftingSpirit = DefaultPerks.Create("LeadershipUpliftingSpirit");
			this._leadershipMakeADifference = DefaultPerks.Create("LeadershipMakeADifference");
			this._leadershipLeadByExample = DefaultPerks.Create("LeadershipLeadByExample");
			this._leadershipTrustedCommander = DefaultPerks.Create("LeadershipTrustedCommander");
			this._leadershipGreatLeader = DefaultPerks.Create("LeadershipGreatLeader");
			this._leadershipWePledgeOurSwords = DefaultPerks.Create("LeadershipWePledgeOurSwords");
			this._leadershipTalentMagnet = DefaultPerks.Create("LeadershipTalentMagnet");
			this._leadershipUltimateLeader = DefaultPerks.Create("LeadershipUltimateLeader");
			this._charmVirile = DefaultPerks.Create("CharmVirile");
			this._charmSelfPromoter = DefaultPerks.Create("CharmSelfPromoter");
			this._charmOratory = DefaultPerks.Create("CharmOratory");
			this._charmWarlord = DefaultPerks.Create("CharmWarlord");
			this._charmForgivableGrievances = DefaultPerks.Create("CharmForgivableGrievances");
			this._charmMeaningfulFavors = DefaultPerks.Create("CharmMeaningfulFavors");
			this._charmInBloom = DefaultPerks.Create("CharmInBloom");
			this._charmYoungAndRespectful = DefaultPerks.Create("CharmYoungAndRespectful");
			this._charmFirebrand = DefaultPerks.Create("CharmFirebrand");
			this._charmFlexibleEthics = DefaultPerks.Create("CharmFlexibleEthics");
			this._charmEffortForThePeople = DefaultPerks.Create("CharmEffortForThePeople");
			this._charmSlickNegotiator = DefaultPerks.Create("CharmSlickNegotiator");
			this._charmGoodNatured = DefaultPerks.Create("CharmGoodNatured");
			this._charmTribute = DefaultPerks.Create("CharmTribute");
			this._charmMoralLeader = DefaultPerks.Create("CharmMoralLeader");
			this._charmNaturalLeader = DefaultPerks.Create("CharmNaturalLeader");
			this._charmPublicSpeaker = DefaultPerks.Create("CharmPublicSpeaker");
			this._charmParade = DefaultPerks.Create("CharmParade");
			this._charmCamaraderie = DefaultPerks.Create("CharmCamaraderie");
			this._charmImmortalCharm = DefaultPerks.Create("CharmImmortalCharm");
			this._tradeAppraiser = DefaultPerks.Create("TradeAppraiser");
			this._tradeWholeSeller = DefaultPerks.Create("TradeWholeSeller");
			this._tradeCaravanMaster = DefaultPerks.Create("TradeCaravanMaster");
			this._tradeMarketDealer = DefaultPerks.Create("TradeMarketDealer");
			this._tradeTravelingRumors = DefaultPerks.Create("TradeTravelingRumors");
			this._tradeLocalConnection = DefaultPerks.Create("TradeLocalConnection");
			this._tradeDistributedGoods = DefaultPerks.Create("TradeDistributedGoods");
			this._tradeTollgates = DefaultPerks.Create("TradeTollgates");
			this._tradeArtisanCommunity = DefaultPerks.Create("TradeArtisanCommunity");
			this._tradeGreatInvestor = DefaultPerks.Create("TradeGreatInvestor");
			this._tradeMercenaryConnections = DefaultPerks.Create("TradeMercenaryConnections");
			this._tradeContentTrades = DefaultPerks.Create("TradeContentTrades");
			this._tradeInsurancePlans = DefaultPerks.Create("TradeInsurancePlans");
			this._tradeRapidDevelopment = DefaultPerks.Create("TradeRapidDevelopment");
			this._tradeGranaryAccountant = DefaultPerks.Create("TradeGranaryAccountant");
			this._tradeTradeyardForeman = DefaultPerks.Create("TradeTradeyardForeman");
			this._tradeSwordForBarter = DefaultPerks.Create("TradeSwordForBarter");
			this._tradeSelfMadeMan = DefaultPerks.Create("TradeSelfMadeMan");
			this._tradeSilverTongue = DefaultPerks.Create("TradeSilverTongue");
			this._tradeSpringOfGold = DefaultPerks.Create("TradeSpringOfGold");
			this._tradeManOfMeans = DefaultPerks.Create("TradeManOfMeans");
			this._tradeTrickleDown = DefaultPerks.Create("TradeTrickleDown");
			this._tradeEverythingHasAPrice = DefaultPerks.Create("TradeEverythingHasAPrice");
			this._stewardWarriorsDiet = DefaultPerks.Create("StewardWarriorsDiet");
			this._stewardFrugal = DefaultPerks.Create("StewardFrugal");
			this._stewardSevenVeterans = DefaultPerks.Create("StewardSevenVeterans");
			this._stewardDrillSergant = DefaultPerks.Create("StewardDrillSergant");
			this._stewardSweatshops = DefaultPerks.Create("StewardSweatshops");
			this._stewardStiffUpperLip = DefaultPerks.Create("StewardStiffUpperLip");
			this._stewardPaidInPromise = DefaultPerks.Create("StewardPaidInPromise");
			this._stewardEfficientCampaigner = DefaultPerks.Create("StewardEfficientCampaigner");
			this._stewardGivingHands = DefaultPerks.Create("StewardForeseeableFuture");
			this._stewardLogistician = DefaultPerks.Create("StewardLogistician");
			this._stewardRelocation = DefaultPerks.Create("StewardRelocation");
			this._stewardAidCorps = DefaultPerks.Create("StewardAidCorps");
			this._stewardGourmet = DefaultPerks.Create("StewardGourmet");
			this._stewardSoundReserves = DefaultPerks.Create("StewardSoundReserves");
			this._stewardForcedLabor = DefaultPerks.Create("StewardForcedLabor");
			this._stewardContractors = DefaultPerks.Create("StewardContractors");
			this._stewardArenicosMules = DefaultPerks.Create("StewardArenicosMules");
			this._stewardArenicosHorses = DefaultPerks.Create("StewardArenicosHorses");
			this._stewardMasterOfPlanning = DefaultPerks.Create("StewardMasterOfPlanning");
			this._stewardMasterOfWarcraft = DefaultPerks.Create("StewardMasterOfWarcraft");
			this._stewardPriceOfLoyalty = DefaultPerks.Create("StewardPriceOfLoyalty");
			this._medicineSelfMedication = DefaultPerks.Create("MedicineSelfMedication");
			this._medicinePreventiveMedicine = DefaultPerks.Create("MedicinePreventiveMedicine");
			this._medicineTriageTent = DefaultPerks.Create("MedicineTriageTent");
			this._medicineWalkItOff = DefaultPerks.Create("MedicineWalkItOff");
			this._medicineSledges = DefaultPerks.Create("MedicineSledges");
			this._medicineDoctorsOath = DefaultPerks.Create("MedicineDoctorsOath");
			this._medicineBestMedicine = DefaultPerks.Create("MedicineBestMedicine");
			this._medicineGoodLodging = DefaultPerks.Create("MedicineGoodLodging");
			this._medicineSiegeMedic = DefaultPerks.Create("MedicineSiegeMedic");
			this._medicineVeterinarian = DefaultPerks.Create("MedicineVeterinarian");
			this._medicinePristineStreets = DefaultPerks.Create("MedicinePristineStreets");
			this._medicineBushDoctor = DefaultPerks.Create("MedicineBushDoctor");
			this._medicinePerfectHealth = DefaultPerks.Create("MedicinePerfectHealth");
			this._medicineHealthAdvise = DefaultPerks.Create("MedicineHealthAdvise");
			this._medicinePhysicianOfPeople = DefaultPerks.Create("MedicinePhysicianOfPeople");
			this._medicineCleanInfrastructure = DefaultPerks.Create("MedicineCleanInfrastructure");
			this._medicineCheatDeath = DefaultPerks.Create("MedicineCheatDeath");
			this._medicineFortitudeTonic = DefaultPerks.Create("MedicineFortitudeTonic");
			this._medicineHelpingHands = DefaultPerks.Create("MedicineHelpingHands");
			this._medicineBattleHardened = DefaultPerks.Create("MedicineBattleHardened");
			this._medicineMinisterOfHealth = DefaultPerks.Create("MedicineMinisterOfHealth");
			this._engineeringScaffolds = DefaultPerks.Create("EngineeringScaffolds");
			this._engineeringTorsionEngines = DefaultPerks.Create("EngineeringTorsionEngines");
			this._engineeringSiegeWorks = DefaultPerks.Create("EngineeringSiegeWorks");
			this._engineeringDungeonArchitect = DefaultPerks.Create("EngineeringDungeonArchitect");
			this._engineeringCarpenters = DefaultPerks.Create("EngineeringCarpenters");
			this._engineeringMilitaryPlanner = DefaultPerks.Create("EngineeringMilitaryPlanner");
			this._engineeringWallBreaker = DefaultPerks.Create("EngineeringWallBreaker");
			this._engineeringDreadfulSieger = DefaultPerks.Create("EngineeringDreadfulSieger");
			this._engineeringSalvager = DefaultPerks.Create("EngineeringSalvager");
			this._engineeringForeman = DefaultPerks.Create("EngineeringForeman");
			this._engineeringStonecutters = DefaultPerks.Create("EngineeringStonecutters");
			this._engineeringSiegeEngineer = DefaultPerks.Create("EngineeringSiegeEngineer");
			this._engineeringCampBuilding = DefaultPerks.Create("EngineeringCampBuilding");
			this._engineeringBattlements = DefaultPerks.Create("EngineeringBattlements");
			this._engineeringEngineeringGuilds = DefaultPerks.Create("EngineeringEngineeringGuilds");
			this._engineeringApprenticeship = DefaultPerks.Create("EngineeringApprenticeship");
			this._engineeringMetallurgy = DefaultPerks.Create("EngineeringMetallurgy");
			this._engineeringImprovedTools = DefaultPerks.Create("EngineeringImprovedTools");
			this._engineeringClockwork = DefaultPerks.Create("EngineeringClockwork");
			this._engineeringArchitecturalCommisions = DefaultPerks.Create("EngineeringArchitecturalCommissions");
			this._engineeringMasterwork = DefaultPerks.Create("EngineeringMasterwork");
			this.InitializeAll();
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x000E297C File Offset: 0x000E0B7C
		private void InitializeAll()
		{
			this._oneHandedWrappedHandles.Initialize("{=looKU9Gl}Wrapped Handles", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(1), this._oneHandedBasher, "{=dY3GOmTN}{VALUE}% handling to one handed weapons.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=0mBHB7mA}{VALUE} one handed skill to infantry troops in your formation.", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.OneHandedUser, TroopUsageFlags.OneHandedUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedBasher.Initialize("{=6yEeYNRu}Basher", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(1), this._oneHandedWrappedHandles, "{=fFNNeqxu}{VALUE}% damage and longer stun duration with shield bashes.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=goOE8oiI}{VALUE}% damage taken by infantry while in shield wall formation.", PartyRole.Captain, -0.04f, EffectIncrementType.AddFactor, TroopUsageFlags.ShieldUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedToBeBlunt.Initialize("{=SJ69EYuI}To Be Blunt", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(2), this._oneHandedSwiftStrike, "{=mzUa3duw}{VALUE}% damage with one handed axes and maces.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=Ib9RYpMO}{VALUE} daily security to governed settlement.", PartyRole.Governor, 0.5f, EffectIncrementType.Add, TroopUsageFlags.OneHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedSwiftStrike.Initialize("{=ciELES5v}Swift Strike", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(2), this._oneHandedToBeBlunt, "{=bW7DT97A}{VALUE}% swing speed with one handed weapons.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, "{=xwA6Om0Y}{VALUE} daily militia recruitment in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.OneHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedCavalry.Initialize("{=YVGtcLHF}Cavalry", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(3), this._oneHandedShieldBearer, "{=D3k7UbmZ}{VALUE}% damage with one handed weapons while mounted.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=aj2R3vnb}{VALUE}% melee damage by cavalry troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.OneHandedUser, TroopUsageFlags.Mounted | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedShieldBearer.Initialize("{=vnG1q18y}Shield Bearer", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(3), this._oneHandedCavalry, "{=hMJVRJdw}Removed movement speed penalty of wielding shields.", PartyRole.Personal, 0f, EffectIncrementType.Invalid, "{=1QsZq9UW}{VALUE}% movement speed to infantry in your formation.", PartyRole.Captain, 0.03f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot | TroopUsageFlags.ShieldUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedTrainer.Initialize("{=UE2X5rAy}Trainer", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(4), this._oneHandedDuelist, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 2f, EffectIncrementType.Add, "{=rXb91Rwi}{VALUE}% experience to melee troops in your party after every battle.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._oneHandedDuelist.Initialize("{=XphY9cNV}Duelist", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(4), this._oneHandedTrainer, "{=uRZgz63l}{VALUE}% damage while wielding a one handed weapon without a shield.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=uKTgBX4S}Double the amount of renown gained from tournaments.", PartyRole.Personal, 2f, EffectIncrementType.AddFactor, TroopUsageFlags.OneHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedShieldWall.Initialize("{=nSwkI97I}Shieldwall", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(5), this._oneHandedArrowCatcher, "{=DiFIyniQ}{VALUE}% damage to your shield while blocking in wrong direction.", PartyRole.Personal, -0.2f, EffectIncrementType.AddFactor, "{=EdDRYFoL}Larger shield protection area against projectiles to troops in your formation while in shield wall formation.", PartyRole.Captain, 0.01f, EffectIncrementType.Add, TroopUsageFlags.ShieldUser, TroopUsageFlags.ShieldUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedArrowCatcher.Initialize("{=a94mkNNk}Arrow Catcher", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(5), this._oneHandedShieldWall, "{=dcsschkC}Larger shield protection area against projectiles.", PartyRole.Personal, 0.01f, EffectIncrementType.Add, "{=uz7KxUlP}Larger shield protection area against projectiles for troops in your formation.", PartyRole.Captain, 0.01f, EffectIncrementType.Add, TroopUsageFlags.ShieldUser, TroopUsageFlags.ShieldUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedMilitaryTradition.Initialize("{=Fc7OsyZ8}Military Tradition", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(6), this._oneHandedCorpsACorps, "{=0A6BUASZ}{VALUE} daily experience to infantry in your party.", PartyRole.PartyLeader, 2f, EffectIncrementType.Add, "{=B2msxAju}{VALUE}% garrison wages in the governed settlement.", PartyRole.Governor, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._oneHandedCorpsACorps.Initialize("{=M3aNEkBJ}Corps-a-corps", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(6), this._oneHandedMilitaryTradition, "{=8jHJeh8z}{VALUE}% of the total experience gained as a bonus to infantry after battles.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=wBgpln4f}{VALUE} garrison limit in the governed settlement.", PartyRole.Governor, 30f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedStandUnited.Initialize("{=d8qjwKza}Stand United", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(7), this._oneHandedLeadByExample, "{=JZ8ihtoa}{VALUE} starting battle morale to troops in your party if you are outnumbered.", PartyRole.PartyLeader, 8f, EffectIncrementType.Add, "{=5aVPqukr}{VALUE}% security provided by troops in the garrison of the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedLeadByExample.Initialize("{=bOhbWapX}Lead by example", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(7), this._oneHandedStandUnited, "{=V97vqbIb}{VALUE}% experience to troops in your party after battle.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, "{=g5nnybjz}{VALUE} starting battle morale to troops in your party.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._oneHandedSteelCoreShields.Initialize("{=rSATpMpq}Steel Core Shields", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(8), this._oneHandedFleetOfFoot, "{=q2gybZYy}{VALUE}% damage to your shields.", PartyRole.Personal, -0.1f, EffectIncrementType.AddFactor, "{=Bb4L971j}{VALUE}% damage to shields of infantry troops in your formation.", PartyRole.Captain, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.ShieldUser, TroopUsageFlags.ShieldUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedFleetOfFoot.Initialize("{=OtdkOGur}Fleet of Foot", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(8), this._oneHandedSteelCoreShields, "{=V53EYEXx}{VALUE}% combat movement speed.", PartyRole.Personal, 0.04f, EffectIncrementType.AddFactor, "{=1QsZq9UW}{VALUE}% movement speed to infantry in your formation.", PartyRole.Captain, 0.04f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedDeadlyPurpose.Initialize("{=xpGoduJq}Deadly Purpose", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(9), this._oneHandedUnwaveringDefense, "{=CTqO5MBf}{VALUE}% damage with one handed weapons.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=fcmt2U5u}{VALUE}% melee weapon damage by infantry in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.OneHandedUser, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedUnwaveringDefense.Initialize("{=yFbEDUyb}Unwavering Defense", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(9), this._oneHandedDeadlyPurpose, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=aeNhsyD7}{VALUE} hit points to infantry in your party.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, TroopUsageFlags.Any, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._oneHandedPrestige.Initialize("{=DSKtsYPi}Prestige", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(10), this._oneHandedChinkInTheArmor, "{=LjeYTgX7}{VALUE}% damage against shields with one handed weapons.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=qxbBsB1a}{VALUE} party limit.", PartyRole.PartyLeader, 15f, EffectIncrementType.Add, TroopUsageFlags.OneHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedChinkInTheArmor.Initialize("{=bBa0LB1D}Chink in the Armor", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(10), this._oneHandedPrestige, "{=KKsCor3D}{VALUE}% armor penetration with melee attacks.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=3a6tmImq}{VALUE}% recruitment cost of infantry.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Melee, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._oneHandedWayOfTheSword.Initialize("{=nThmB3yB}Way of the Sword", DefaultSkills.OneHanded, DefaultPerks.GetTierCost(11), null, "{=jan55Git}{VALUE}% attack speed with one handed weapons for every skill point above 250.", PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, "{=hr0TfX6o}{VALUE}% damage with one handed weapons for every skill point above 250.", PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.Melee, TroopUsageFlags.Melee, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._twoHandedStrongGrip.Initialize("{=xDQTgPf0}Strong Grip", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(1), this._twoHandedWoodChopper, "{=OpVRgL9n}{VALUE}% handling to two handed weapons.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=TmYKKarv}{VALUE} two handed skill to infantry troops in your formation.", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.OnFoot | TroopUsageFlags.TwoHandedUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedWoodChopper.Initialize("{=J7oh7Vin}Wood Chopper", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(1), this._twoHandedStrongGrip, "{=impj5bAM}{VALUE}% damage to shields with two handed weapons.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=4u69jBeE}{VALUE}% damage against shields by troops in your formation.", PartyRole.Captain, 0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Any, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedOnTheEdge.Initialize("{=rkuAgPSA}On the Edge", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(2), this._twoHandedHeadBasher, "{=R8Lnif8l}{VALUE}% swing speed with two handed weapons.", PartyRole.Personal, 0.03f, EffectIncrementType.AddFactor, "{=Tq0E9sSW}{VALUE}% swing speed to infantry in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedHeadBasher.Initialize("{=E5bgLJcs}Head Basher", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(2), this._twoHandedOnTheEdge, "{=qJBhadGi}{VALUE}% damage with two handed axes and maces.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=c86V0dch}{VALUE}% damage by infantry in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedShowOfStrength.Initialize("{=RlMqzqbS}Show of Strength", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(3), this._twoHandedBaptisedInBlood, "{=eaUjIK1D}Two handed weapons that can knockdown ignore {VALUE}% knockdown resistance on swing attacks.", PartyRole.Personal, 0.3f, EffectIncrementType.Add, "{=3a6tmImq}{VALUE}% recruitment cost of infantry.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._twoHandedBaptisedInBlood.Initialize("{=miMZavW3}Baptised in Blood", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(3), this._twoHandedShowOfStrength, "{=TR4ORD1T}{VALUE} experience to infantry in your party for each enemy you kill with a two handed weapon.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=rXb91Rwi}{VALUE}% experience to melee troops in your party after every battle.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._twoHandedBeastSlayer.Initialize("{=NDtlE6PY}Beast Slayer", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(4), this._twoHandedShieldBreaker, "{=fxTRlxBD}{VALUE}% damage to mounts with two handed weapons.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=lekpGqEA}{VALUE}% damage to mounts by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedShieldBreaker.Initialize("{=bM9VX881}Shield breaker", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(4), this._twoHandedBeastSlayer, "{=impj5bAM}{VALUE}% damage to shields with two handed weapons.", PartyRole.Personal, 0.4f, EffectIncrementType.AddFactor, "{=4u69jBeE}{VALUE}% damage against shields by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Any, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._twoHandedBerserker.Initialize("{=RssJTUpL}Berserker", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(5), this._twoHandedConfidence, "{=D5RqqHIm}{VALUE}% damage with two handed weapons while you have less than half of your hit points.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=B2msxAju}{VALUE}% garrison wages in the governed settlement.", PartyRole.Governor, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._twoHandedConfidence.Initialize("{=2jnsxsv5}Confidence", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(5), this._twoHandedBerserker, "{=QUXbhsxb}{VALUE}% damage with two handed weapons while you have more than 90% of your hit points.", PartyRole.Personal, 0.15f, EffectIncrementType.AddFactor, "{=FX0GjiNa}{VALUE}% build speed to military projects in the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._twoHandedProjectileDeflection.Initialize("{=rCCG4mSJ}Projectile Deflection", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(6), null, "{=YP8tN7nl}You can deflect projectiles with two handed swords by blocking.", PartyRole.Personal, 0f, EffectIncrementType.Invalid, "{=FdSPC05Q}{VALUE}% experience to garrison troops in the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._twoHandedTerror.Initialize("{=nAlCj2m0}Terror", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(7), this._twoHandedHope, "{=thGHcZMB}{VALUE}% battle morale effect to enemy troops with your two handed kills.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=POp8DAZD}{VALUE} prisoner limit.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._twoHandedHope.Initialize("{=lPuk6bao}Hope", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(7), this._twoHandedTerror, "{=2zNrVsDj}{VALUE}% battle morale effect to friendly troops with your two handed kills.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=qxbBsB1a}{VALUE} party limit.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._twoHandedRecklessCharge.Initialize("{=ZGovl01w}Reckless Charge", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(8), this._twoHandedThickHides, "{=1PC4D2fx}{VALUE}% damage bonus from speed with two handed weapons while on foot.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=b5l18lo7}{VALUE}% damage and movement speed to infantry in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedThickHides.Initialize("{=j9OIuxxY}Thick Hides", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(8), this._twoHandedRecklessCharge, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=ucHrYWaz}{VALUE} hit points to troops in your party.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Any, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedBladeMaster.Initialize("{=TtwAoHfw}Blade Master", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(9), this._twoHandedVandal, "{=Pq0bhBUL}{VALUE}% damage with two handed weapons.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=l0ZGaUuI}{VALUE}% attack speed to infantry in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._twoHandedVandal.Initialize("{=czCRxHZy}Vandal", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(9), this._twoHandedBladeMaster, "{=u57OuUZR}{VALUE}% armor penetration with your attacks.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=8q4vzfbH}{VALUE}% damage against destructible objects by troops in your formation.", PartyRole.Captain, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._twoHandedWayOfTheGreatAxe.Initialize("{=dbEb7iak}Way Of The Great Axe", DefaultSkills.TwoHanded, DefaultPerks.GetTierCost(10), null, "{=yRvF2Li4}{VALUE}% attack speed with two handed weapons for every skill point above 250.", PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, "{=IM05Jahy}{VALUE}% damage with two handed weapons for every skill point above 250.", PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.TwoHandedUser, TroopUsageFlags.TwoHandedUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._polearmPikeman.Initialize("{=IFqtwpb0}Pikeman", DefaultSkills.Polearm, DefaultPerks.GetTierCost(1), this._polearmCavalry, "{=NtzmIh0g}{VALUE}% damage with polearms on foot.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, "{=Yu5hTuIN}{VALUE}% damage by infantry troops in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot | TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmCavalry.Initialize("{=YVGtcLHF}Cavalry", DefaultSkills.Polearm, DefaultPerks.GetTierCost(1), this._polearmPikeman, "{=IaBTfvfc}{VALUE}% damage with polearms while mounted.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, "{=ywc8frAo}{VALUE}% damage by cavalry troops in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.PolearmUser, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._polearmBraced.Initialize("{=dU7haWkI}Braced", DefaultSkills.Polearm, DefaultPerks.GetTierCost(2), this._polearmKeepAtBay, "{=QFaoD1Ka}Polearms that can dismount ignore {VALUE}% dismount resistance on attacks against cavalry.", PartyRole.Personal, 0.25f, EffectIncrementType.Add, "{=OWXECmbt}{VALUE}% damage by infantry in your formation against cavalry.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._polearmKeepAtBay.Initialize("{=TaucWWCB}Keep at Bay", DefaultSkills.Polearm, DefaultPerks.GetTierCost(2), this._polearmBraced, "{=iqRCAb6f}Polearms ignore {VALUE}% knockback resistance on thrust attacks against footmen.", PartyRole.Personal, 0.3f, EffectIncrementType.Add, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.PolearmUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._polearmSwiftSwing.Initialize("{=xM5aawCj}Swift Swing", DefaultSkills.Polearm, DefaultPerks.GetTierCost(3), this._polearmCleanThrust, "{=7tdcIxYN}{VALUE}% swing speed with polearms.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=Tq0E9sSW}{VALUE}% swing speed to infantry in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmCleanThrust.Initialize("{=PeaiNrSu}Clean Thrust", DefaultSkills.Polearm, DefaultPerks.GetTierCost(3), this._polearmSwiftSwing, "{=xEp10rIa}{VALUE}% thrust damage with polearms.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=apgpk6j1}{VALUE} polearm skill to infantry in your formation.", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot | TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmFootwork.Initialize("{=Yvk8a2tb}Footwork", DefaultSkills.Polearm, DefaultPerks.GetTierCost(4), this._polearmHardKnock, "{=eDzl7r8u}{VALUE}% combat movement speed with polearms.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, "{=1QsZq9UW}{VALUE}% movement speed to infantry in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot | TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmHardKnock.Initialize("{=8DTKXbKw}Hard Knock", DefaultSkills.Polearm, DefaultPerks.GetTierCost(4), this._polearmFootwork, "{=h0HC4a7q}Polearms that can knockdown ignore {VALUE}% knockdown resistance on thrust attacks.", PartyRole.Personal, 0.25f, EffectIncrementType.Add, "{=aeNhsyD7}{VALUE} hit points to infantry in your party.", PartyRole.PartyLeader, 3f, EffectIncrementType.Add, TroopUsageFlags.PolearmUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmSteedKiller.Initialize("{=8POWjrr6}Steed Killer", DefaultSkills.Polearm, DefaultPerks.GetTierCost(5), this._polearmLancer, "{=5aE8sEnj}{VALUE}% damage to mounts with polearms.", PartyRole.Personal, 0.7f, EffectIncrementType.AddFactor, "{=JGGdnIRO}{VALUE}% damage to mounts with polearms by infantry in your formation.", PartyRole.Captain, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot | TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._polearmLancer.Initialize("{=hchDYAKL}Lancer", DefaultSkills.Polearm, DefaultPerks.GetTierCost(5), this._polearmSteedKiller, "{=I0hqrQuD}{VALUE}% damage bonus from speed with polearms while mounted.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=00mulBcs}{VALUE}% damage bonus from speed with polearms by troops in your formation.", PartyRole.Captain, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.PolearmUser, TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._polearmSkewer.Initialize("{=o57z0zB9}Skewer", DefaultSkills.Polearm, DefaultPerks.GetTierCost(6), this._polearmGuards, "{=hMORvBsG}30% chance of your lance staying couched after a kill.", PartyRole.Personal, 0.25f, EffectIncrementType.Add, "{=buFin46y}{VALUE} daily security in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Mounted | TroopUsageFlags.PolearmUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._polearmGuards.Initialize("{=vquApOWo}Guards", DefaultSkills.Polearm, DefaultPerks.GetTierCost(6), this._polearmSkewer, "{=VB0GJijE}{VALUE}% damage when you hit an enemy in the head with a polearm.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=Ux90sIph}{VALUE}% experience gain to garrisoned cavalry in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._polearmStandardBearer.Initialize("{=Vv81gkWN}Standard Bearer", DefaultSkills.Polearm, DefaultPerks.GetTierCost(7), this._polearmPhalanx, "{=RbDAfDsF}{VALUE}% battle morale loss to troops in your formation.", PartyRole.Captain, -0.2f, EffectIncrementType.AddFactor, "{=V2v4ZMDT}{VALUE}% wages to garrisoned infantry in the governed settlement.", PartyRole.Governor, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._polearmPhalanx.Initialize("{=5vs3qlQ8}Phalanx", DefaultSkills.Polearm, DefaultPerks.GetTierCost(7), this._polearmStandardBearer, "{=3zzWzQcO}{VALUE} melee weapon skills to troops in your party while in shield wall formation.", PartyRole.PartyLeader, 30f, EffectIncrementType.Add, "{=yank0KD9}{VALUE}% damage with polearms by troops in your formation.", PartyRole.Captain, 0.03f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._polearmHardyFrontline.Initialize("{=NtMEk0lA}Hardy Frontline", DefaultSkills.Polearm, DefaultPerks.GetTierCost(8), this._polearmDrills, "{=ucHrYWaz}{VALUE} hit points to troops in your party.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, "{=3a6tmImq}{VALUE}% recruitment cost of infantry.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._polearmDrills.Initialize("{=JpiQagYa}Drills", DefaultSkills.Polearm, DefaultPerks.GetTierCost(8), this._polearmHardyFrontline, "{=vMWC5dR8}{VALUE}% rate of militias will spawn as veteran troops in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, "{=x3SJTtDj}{VALUE} bonus daily experience to troops in your party.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._polearmSureFooted.Initialize("{=bdzt4TcN}Sure Footed", DefaultSkills.Polearm, DefaultPerks.GetTierCost(9), this._polearmUnstoppableForce, "{=QqVLsf0N}{VALUE}% charge damage taken.", PartyRole.Personal, -0.4f, EffectIncrementType.AddFactor, "{=Dilnx8Es}{VALUE}% charge damage taken by troops in your formation.", PartyRole.Captain, -0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._polearmUnstoppableForce.Initialize("{=Jat5GFDi}Unstoppable Force", DefaultSkills.Polearm, DefaultPerks.GetTierCost(9), this._polearmSureFooted, "{=cUaUTwx5}Triple couch lance damage against shields.", PartyRole.Personal, 3f, EffectIncrementType.AddFactor, "{=jaLOtaRh}{VALUE}% damage bonus from speed with polearms to cavalry in your formation.", PartyRole.Captain, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.PolearmUser, TroopUsageFlags.Mounted | TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._polearmCounterweight.Initialize("{=BazrgEOj}Counterweight", DefaultSkills.Polearm, DefaultPerks.GetTierCost(10), this._polearmSharpenTheTip, "{=02IdNzbt}{VALUE}% handling of swingable polearms.", PartyRole.Personal, 0.15f, EffectIncrementType.AddFactor, "{=uZPweUQg}{VALUE} polearm skill to troops in your formation.", PartyRole.Captain, 20f, EffectIncrementType.Add, TroopUsageFlags.PolearmUser, TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmSharpenTheTip.Initialize("{=ljduhdzj}Sharpen the Tip", DefaultSkills.Polearm, DefaultPerks.GetTierCost(10), this._polearmCounterweight, "{=sbkrplXi}{VALUE}% damage with thrust attacks made with polearms.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=wLrF0mzr}{VALUE}% damage with thrust attacks by infantry troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._polearmWayOfTheSpear.Initialize("{=YnimoRlg}Way of the Spear", DefaultSkills.Polearm, DefaultPerks.GetTierCost(11), null, "{=x1T8wWNU}{VALUE}% attack speed with polearms for every skill point above 250.", PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, "{=UB67Ye3r}{VALUE}% damage with polearms for every skill point above 250.", PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.PolearmUser, TroopUsageFlags.PolearmUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._bowBowControl.Initialize("{=1zteHA7R}Bow Control", DefaultSkills.Bow, DefaultPerks.GetTierCost(1), this._bowDeadAim, "{=4PdKPMNc}{VALUE}% accuracy penalty while moving.", PartyRole.Personal, -0.3f, EffectIncrementType.AddFactor, "{=0DaxFvnw}{VALUE}% damage with bows by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Ranged, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowDeadAim.Initialize("{=FVLymWqW}Dead Aim", DefaultSkills.Bow, DefaultPerks.GetTierCost(1), this._bowBowControl, "{=hmbeQW4v}{VALUE}% headshot damage with bows.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=QbWK6sWo}{VALUE} Bow skill to troops in your formation.", PartyRole.Captain, 20f, EffectIncrementType.Add, TroopUsageFlags.BowUser, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowBodkin.Initialize("{=PDM8MzCA}Bodkin", DefaultSkills.Bow, DefaultPerks.GetTierCost(2), this._bowNockingPoint, "{=EU3No7XM}{VALUE}% armor penetration with bows.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=KfLZ8Hbw}{VALUE}% armor penetration with bows by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowNockingPoint.Initialize("{=bS8alS24}Nocking Point", DefaultSkills.Bow, DefaultPerks.GetTierCost(2), this._bowBodkin, "{=zady0hI7}{VALUE}% movement speed penalty while reloading.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "{=kaJ6SJeI}{VALUE}% movement speed to archers in your formation.", PartyRole.Captain, 0.03f, EffectIncrementType.AddFactor, TroopUsageFlags.Ranged, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowRapidFire.Initialize("{=Kc9oatmM}Rapid Fire", DefaultSkills.Bow, DefaultPerks.GetTierCost(3), this._bowQuickAdjustments, "{=0vqPUXfr}{VALUE}% reload speed with bows.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=KOlw0Na1}{VALUE}% reload speed to troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.Ranged, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowQuickAdjustments.Initialize("{=nOZerIfl}Quick Adjustments", DefaultSkills.Bow, DefaultPerks.GetTierCost(3), this._bowRapidFire, "{=LAxaYQzv}{VALUE}% accuracy penalty while rotating.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "{=qC298I3g}{VALUE}% accuracy penalty to archers in your formation.", PartyRole.Captain, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Ranged, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowMerryMen.Initialize("{=ssljPTUr}Merry Men", DefaultSkills.Bow, DefaultPerks.GetTierCost(4), this._bowMountedArchery, "{=NouDSrXE}{VALUE} party size.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._bowMountedArchery.Initialize("{=WEUSMkCp}Mounted Archery", DefaultSkills.Bow, DefaultPerks.GetTierCost(4), this._bowMerryMen, "{=XITAY0KG}{VALUE}% accuracy penalty using bows while mounted.", PartyRole.Personal, -0.3f, EffectIncrementType.AddFactor, "{=6XDcZUsb}{VALUE}% security provided by archers in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._bowTrainer.Initialize("{=UE2X5rAy}Trainer", DefaultSkills.Bow, DefaultPerks.GetTierCost(5), this._bowStrongBows, "{=xoVR3Xr3}Daily Bow skill experience bonus to the party member with the lowest bow skill.", PartyRole.PartyLeader, 6f, EffectIncrementType.Add, "{=TcMme3Da}{VALUE} daily experience to archers in your party.", PartyRole.PartyLeader, 3f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._bowStrongBows.Initialize("{=dqbbT5DK}Strong bows", DefaultSkills.Bow, DefaultPerks.GetTierCost(5), this._bowTrainer, "{=FXWn934b}{VALUE}% damage with bows.", PartyRole.Personal, 0.08f, EffectIncrementType.AddFactor, "{=yppPCR1z}{VALUE}% damage with bows by tier 3+ troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowDiscipline.Initialize("{=D867vF71}Discipline", DefaultSkills.Bow, DefaultPerks.GetTierCost(6), this._bowHunterClan, "{=sHiIwnOb}{VALUE}% aiming duration without losing accuracy.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=F7bbkYx4}{VALUE} loyalty per day in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Ranged, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._bowHunterClan.Initialize("{=AAy1oX7z}Hunter Clan", DefaultSkills.Bow, DefaultPerks.GetTierCost(6), this._bowDiscipline, "{=kLVpYR0z}{VALUE}% damage with bows to mounts.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=1FPpHasQ}{VALUE}% garrison wages in the governed castle.", PartyRole.Governor, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._bowSkirmishPhaseMaster.Initialize("{=oVdoauUE}Skirmish Phase Master", DefaultSkills.Bow, DefaultPerks.GetTierCost(7), this._bowEagleEye, "{=R93r6aV7}{VALUE}% damage taken from projectiles.", PartyRole.Personal, -0.1f, EffectIncrementType.AddFactor, "{=pHdIgnYu}{VALUE}% damage taken from projectiles by ranged troops in your formation.", PartyRole.Captain, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Ranged, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowEagleEye.Initialize("{=lq67KjSY}Eagle Eye", DefaultSkills.Bow, DefaultPerks.GetTierCost(7), this._bowSkirmishPhaseMaster, "{=xTDnna2J}{VALUE}% zoom with bows.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=1Z8oWbo7}{VALUE}% visual range on the campaign map.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowBullsEye.Initialize("{=QH77Weyq}Bulls Eye", DefaultSkills.Bow, DefaultPerks.GetTierCost(8), this._bowRenownedArcher, "{=OFMYfDPZ}{VALUE}% bonus experience to ranged troops in your party after every battle.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=mmH70R4H}{VALUE} daily experience to garrison troops in the governed settlement.", PartyRole.Governor, 3f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._bowRenownedArcher.Initialize("{=aIKVpGvE}Renowned Archer", DefaultSkills.Bow, DefaultPerks.GetTierCost(8), this._bowBullsEye, "{=kmdxvkEV}{VALUE}% starting battle morale to ranged troops in your party.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=bnnWpLbk}{VALUE}% recruitment and upgrade cost to ranged troops.", PartyRole.PartyLeader, -0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._bowHorseMaster.Initialize("{=dbUybDTG}Horse Master", DefaultSkills.Bow, DefaultPerks.GetTierCost(9), this._bowDeepQuivers, "{=LiaRnWJZ}You can now use all bows on horseback.", PartyRole.Personal, 0f, EffectIncrementType.Invalid, "{=0J9dgA7j}{VALUE} bow skill to horse archers in your formation", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._bowDeepQuivers.Initialize("{=h83ZU95t}Deep Quivers", DefaultSkills.Bow, DefaultPerks.GetTierCost(9), this._bowHorseMaster, "{=YOQQR1bJ}{VALUE} extra arrows per quiver.", PartyRole.Personal, 3f, EffectIncrementType.Add, "{=CBVfPRRe}{VALUE} extra arrow per quiver to troops in your party.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.BowUser, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._bowQuickDraw.Initialize("{=vnJndBgT}Quick Draw", DefaultSkills.Bow, DefaultPerks.GetTierCost(10), this._bowRangersSwiftness, "{=jU084S5S}{VALUE}% aiming speed with bows.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=tsh4RXNa}{VALUE}% tax gain in the governed settlement.", PartyRole.Governor, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._bowRangersSwiftness.Initialize("{=p12tSfCC}Ranger's Swiftness", DefaultSkills.Bow, DefaultPerks.GetTierCost(10), this._bowQuickDraw, "{=RQd005zy}Equipped bows do not slow you down.", PartyRole.Personal, 0f, EffectIncrementType.Invalid, "{=6XDcZUsb}{VALUE}% security provided by archers in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._bowDeadshot.Initialize("{=rsKhbZpJ}Deadshot", DefaultSkills.Bow, DefaultPerks.GetTierCost(11), null, "{=HCqd0IOt}{VALUE}% reload speed with bows for every skill point above 200.", PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, "{=hiFadSiC}{VALUE}% damage with bows for every skill point above 200.", PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.BowUser, TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._crossbowPiercer.Initialize("{=v8RwzwqD}Piercer", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(1), this._crossbowMarksmen, "{=j3J0Hbax}Your crossbow attacks ignore armors below 20.", PartyRole.Personal, 20f, EffectIncrementType.Add, "{=CLBXxPdh}{VALUE}% recruitment cost of ranged troops.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._crossbowMarksmen.Initialize("{=IUGVdu64}Marksmen", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(1), this._crossbowPiercer, "{=LCsu8rXa}{VALUE}% faster aiming with crossbows.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=kmdxvkEV}{VALUE}% starting battle morale to ranged troops in your party.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._crossbowUnhorser.Initialize("{=75vJc53f}Unhorser", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(2), this._crossbowWindWinder, "{=97nYJcKO}{VALUE}% crossbow damage to mounts.", PartyRole.Personal, 0.4f, EffectIncrementType.AddFactor, "{=laWxqjBP}{VALUE}% damage against mounts to crossbow troops in your formation.", PartyRole.Captain, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowWindWinder.Initialize("{=1bw2uw8H}Wind Winder", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(2), this._crossbowUnhorser, "{=3cBX5x0x}{VALUE}% reload speed with crossbows.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=YHjdf1uO}{VALUE}% crossbow reload speed to troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowDonkeysSwiftness.Initialize("{=uANbQUxg}Donkey's Swiftness", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(3), this._crossbowSheriff, "{=Af7zOV2l}{VALUE}% accuracy loss while moving.", PartyRole.Personal, -0.3f, EffectIncrementType.AddFactor, "{=aIyRxlCf}{VALUE} crossbow skill to troops in your formation.", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.Ranged, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowSheriff.Initialize("{=leaowE4D}Sheriff", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(3), this._crossbowDonkeysSwiftness, "{=W7PaF7Lr}{VALUE}% headshot damage with crossbows.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=HB2wwuj6}{VALUE}% crossbow damage to infantry by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.OnFoot | TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowPeasantLeader.Initialize("{=2rPMYl7Y}Peasant Leader", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(4), this._crossbowRenownMarksmen, "{=4CSaYB8H}{VALUE}% battle morale to tier 1 to 3 troops", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=xuUbaa9f}{VALUE}% garrisoned ranged troop wages in the governed settlement.", PartyRole.Governor, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._crossbowRenownMarksmen.Initialize("{=ICkvftaM}Renowned Marksmen", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(4), this._crossbowPeasantLeader, "{=uj52xbdr}{VALUE} daily experience to ranged troops in your party.", PartyRole.PartyLeader, 2f, EffectIncrementType.Add, "{=i4GboakR}{VALUE}% security provided by ranged troops in the garrison of the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._crossbowFletcher.Initialize("{=FA5QlTvm}Fletcher", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(5), this._crossbowPuncture, "{=wvQbeHpp}{VALUE} bolts per quiver.", PartyRole.Personal, 4f, EffectIncrementType.Add, "{=j3Hcp9RQ}{VALUE} bolts per quiver to troops in your party.", PartyRole.PartyLeader, 2f, EffectIncrementType.Add, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowPuncture.Initialize("{=jjkJyKoy}Puncture", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(5), this._crossbowFletcher, "{=bVUQyN8t}{VALUE}% armor penetration with crossbows.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=KhCU9FZU}{VALUE}% armor penetration with crossbows by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowLooseAndMove.Initialize("{=SKUPHeAw}Loose and Move", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(6), this._crossbowDeftHands, "{=BbaidhT4}Equipped crossbows do not slow you down.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=loVfqss6}{VALUE}% movement speed to ranged troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.Ranged, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowDeftHands.Initialize("{=NYHeygaj}Deft Hands", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(6), this._crossbowLooseAndMove, "{=VY7WQSgu}{VALUE}% resistance to getting staggered while reloading your crossbow.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=wUov3khT}{VALUE}% resistance to getting staggered while reloading crossbows to troops in your formation.", PartyRole.Captain, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowCounterFire.Initialize("{=grgnisK4}Counter Fire", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(7), this._crossbowMountedCrossbowman, "{=8ieLwTyG}{VALUE}% projectile damage taken while equipped with a crossbow.", PartyRole.Personal, -0.1f, EffectIncrementType.AddFactor, "{=zJHhHRBw}{VALUE}% damage taken from projectiles by your troops.", PartyRole.Captain, -0.03f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.Any, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowMountedCrossbowman.Initialize("{=UZHvbYTr}Mounted Crossbowman", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(7), this._crossbowCounterFire, "{=ZTt5Es7q}You can reload any crossbow on horseback.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=i36Gg6mW}{VALUE}% experience gained to ranged troops in your party.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.CrossbowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.NavalReduced);
			this._crossbowSteady.Initialize("{=Ye9lbBr3}Steady", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(8), this._crossbowLongShots, "{=wFWdhNCN}{VALUE}% accuracy penalty with crossbows while mounted.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "{=q5IMLou4}{VALUE}% tariff gain in the governed settlement.", PartyRole.Governor, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.CrossbowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._crossbowLongShots.Initialize("{=Y5KXWHJY}Long Shots", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(8), this._crossbowSteady, "{=Stykk4VR}{VALUE}% more zoom with crossbows.", PartyRole.Personal, 1f, EffectIncrementType.AddFactor, "{=xwA6Om0Y}{VALUE} daily militia recruitment in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.CrossbowUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._crossbowHammerBolts.Initialize("{=FjMS9Mbz}Hammer Bolts", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(9), this._crossbowPavise, "{=Y9Af8mOd}Crossbows can now dismount and ignore {VALUE}% dismount resistance on attacks against cavalry.", PartyRole.Personal, 0.5f, EffectIncrementType.Add, "{=yz8xogMc}{VALUE}% damage with crossbows by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowPavise.Initialize("{=2667CwaK}Pavise", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(9), this._crossbowHammerBolts, "{=pr5vaFNc}{VALUE}% chance of blocking projectiles from behind with a shield on your back.", PartyRole.Personal, 0.75f, EffectIncrementType.AddFactor, "{=Q8LSfvIO}{VALUE}% accuracy to ballistas in the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._crossbowTerror.Initialize("{=nAlCj2m0}Terror", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(10), this._crossbowPickedShots, "{=NGFbn4Qx}{VALUE}% chance of increasing the siege bombardment casualties per hit by 1.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, "{=sUFw8cDt}{VALUE}% morale loss to enemy due to crossbow kills by troops in your formation.", PartyRole.Captain, 0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowPickedShots.Initialize("{=nGWmyZCs}Picked Shots", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(10), this._crossbowTerror, "{=YG7HavAk}{VALUE}% wages of tier 4+ ranged troops.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, "{=Yxchzh3a}{VALUE} hit points to ranged troops in your party.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Ranged, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._crossbowMightyPull.Initialize("{=ZFtyxzT5}Mighty Pull", DefaultSkills.Crossbow, DefaultPerks.GetTierCost(11), null, "{=Jtx8Czql}{VALUE}% reload speed with crossbows for every skill point above 200.", PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, "{=WUaSub02}{VALUE}% damage with crossbows for every skill point above 200.", PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.CrossbowUser, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._throwingQuickDraw.Initialize("{=vnJndBgT}Quick Draw", DefaultSkills.Throwing, DefaultPerks.GetTierCost(1), this._throwingShieldBreaker, "{=Fnbf4ShY}{VALUE}% draw speed with throwing weapons.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=UkADS8nQ}{VALUE}% draw speed with throwing weapons to troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingShieldBreaker.Initialize("{=DeWp2GjP}Shield Breaker", DefaultSkills.Throwing, DefaultPerks.GetTierCost(1), this._throwingQuickDraw, "{=wPwbyBra}{VALUE}% damage to shields with throwing weapons.", PartyRole.Personal, 0.4f, EffectIncrementType.AddFactor, "{=inFSdSiC}{VALUE}% damage to shields with throwing weapons by troops in your formation.", PartyRole.Captain, 0.08f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingHunter.Initialize("{=xnDWqYKW}Hunter", DefaultSkills.Throwing, DefaultPerks.GetTierCost(2), this._throwingFlexibleFighter, "{=FPdjh976}{VALUE}% damage to mounts with throwing weapons.", PartyRole.Personal, 0.4f, EffectIncrementType.AddFactor, "{=ZgvRAR0u}{VALUE}% damage to mounts with throwing weapons by troops in your formation.", PartyRole.Captain, 0.08f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._throwingFlexibleFighter.Initialize("{=mPPWRjCZ}Flexible Fighter", DefaultSkills.Throwing, DefaultPerks.GetTierCost(2), this._throwingHunter, "{=6rEsV6SZ}{VALUE}% damage while using throwing weapons as melee.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=SSm1kkaB}{VALUE} Control skills of infantry, {VALUE} Vigor skills of archers in your formation.", PartyRole.Captain, 15f, EffectIncrementType.Add, TroopUsageFlags.Melee | TroopUsageFlags.ThrownUser, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingMountedSkirmisher.Initialize("{=l1I748Fn}Mounted Skirmisher", DefaultSkills.Throwing, DefaultPerks.GetTierCost(3), this._throwingWellPrepared, "{=JsdkJbDL}{VALUE}% accuracy penalty with throwing weapons while mounted.", PartyRole.Personal, -0.2f, EffectIncrementType.AddFactor, "{=0L96iq1b}{VALUE}% damage with throwing weapons by mounted troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.ThrownUser, TroopUsageFlags.Mounted | TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._throwingWellPrepared.Initialize("{=bloxcikL}Well Prepared", DefaultSkills.Throwing, DefaultPerks.GetTierCost(3), this._throwingMountedSkirmisher, "{=nKw4eb22}{VALUE} ammunition for throwing weapons.", PartyRole.Personal, 1f, EffectIncrementType.Add, "{=1lEckrPh}{VALUE} ammunition for throwing weapons to troops in your party.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingRunningThrow.Initialize("{=OcaW12fJ}Running Throw", DefaultSkills.Throwing, DefaultPerks.GetTierCost(4), this._throwingKnockOff, "{=Z4maWcyl}{VALUE}% damage bonus from speed with throwing weapons.", PartyRole.Personal, 0.25f, EffectIncrementType.Add, "{=a5CWbHsd}{VALUE} throwing skill to troops in your formation.", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingKnockOff.Initialize("{=Gn3KBN8L}Knock Off", DefaultSkills.Throwing, DefaultPerks.GetTierCost(4), this._throwingRunningThrow, "{=WXNOEqwe}Thrown weapons can now dismount and ignore {VALUE}% dismount resistance on attacks against cavalry.", PartyRole.Personal, 0.25f, EffectIncrementType.Add, "{=cJEbenVQ}{VALUE}% throwing weapon damage to cavalry by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.Mounted | TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._throwingSkirmisher.Initialize("{=s9oED1IR}Skirmisher", DefaultSkills.Throwing, DefaultPerks.GetTierCost(5), this._throwingSaddlebags, "{=O6UPQskm}{VALUE}% damage taken by ranged attacks while holding a throwing weapon.", PartyRole.Personal, -0.1f, EffectIncrementType.AddFactor, "{=ZUYOXMFo}{VALUE}% damage taken by ranged attacks to troops in your formation.", PartyRole.Captain, -0.03f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.Any, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingSaddlebags.Initialize("{=VUxFbEiW}Saddlebags", DefaultSkills.Throwing, DefaultPerks.GetTierCost(5), this._throwingSkirmisher, "{=bFNFpd2d}{VALUE} ammunition for throwing weapons when you start a battle mounted.", PartyRole.Personal, 2f, EffectIncrementType.Add, "{=0jbhAPub}{VALUE} daily experience to infantry troops in your party.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Mounted | TroopUsageFlags.ThrownUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._throwingFocus.Initialize("{=throwingskillfocus}Focus", DefaultSkills.Throwing, DefaultPerks.GetTierCost(6), this._throwingLastHit, "{=hJdHb0G7}{VALUE}% zoom with throwing weapons.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=buFin46y}{VALUE} daily security in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.ThrownUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._throwingLastHit.Initialize("{=IsHyjvSq}Last Hit", DefaultSkills.Throwing, DefaultPerks.GetTierCost(6), this._throwingFocus, "{=PleZrXuO}{VALUE}% damage to enemies with less than half of their hit points left.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=g5nnybjz}{VALUE} starting battle morale to troops in your party.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._throwingHeadHunter.Initialize("{=iARYMyuq}Head Hunter", DefaultSkills.Throwing, DefaultPerks.GetTierCost(7), this._throwingSlingingCompetitions, "{=UsD0y74h}{VALUE}% headshot damage with thrown weapons.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=j7inOz04}{VALUE}% recruitment cost of tier 2+ troops.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._throwingSlingingCompetitions.Initialize("{=zQZltzmK}Slinging Competitions", DefaultSkills.Throwing, DefaultPerks.GetTierCost(7), this._throwingHeadHunter, "{=l4bMb2DO}Sling weapons can penetrate head armor.", PartyRole.Personal, -0.2f, EffectIncrementType.AddFactor, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.ThrownUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._throwingResourceful.Initialize("{=w53LSPJ1}Resourceful", DefaultSkills.Throwing, DefaultPerks.GetTierCost(8), this._throwingSplinters, "{=nKw4eb22}{VALUE} ammunition for throwing weapons.", PartyRole.Personal, 2f, EffectIncrementType.Add, "{=P0iCmQAf}{VALUE}% experience from battles to troops in your party equipped with throwing weapons.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._throwingSplinters.Initialize("{=b6W74uyR}Splinters", DefaultSkills.Throwing, DefaultPerks.GetTierCost(8), this._throwingResourceful, "{=ymKzbcfB}Triple damage against shields with throwing axes.", PartyRole.Personal, 3f, EffectIncrementType.AddFactor, "{=inFSdSiC}{VALUE}% damage to shields with throwing weapons by troops in your formation.", PartyRole.Captain, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingPerfectTechnique.Initialize("{=BCoQgZvG}Perfect Technique", DefaultSkills.Throwing, DefaultPerks.GetTierCost(9), this._throwingLongReach, "{=cr1AipGT}{VALUE}% travel speed to your throwing weapons.", PartyRole.Personal, 0.25f, EffectIncrementType.AddFactor, "{=rkHnKmSK}{VALUE}% travel speed to throwing weapons of troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingLongReach.Initialize("{=9iLyu1kp}Long Reach", DefaultSkills.Throwing, DefaultPerks.GetTierCost(9), this._throwingPerfectTechnique, "{=lEi1hIIt}You can pick up items from the ground while mounted.", PartyRole.Personal, 0f, EffectIncrementType.AddFactor, "{=VgkFpMxF}{VALUE}% morale and renown gained from battles won.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._throwingWeakSpot.Initialize("{=cPPLAz8l}Weak Spot", DefaultSkills.Throwing, DefaultPerks.GetTierCost(10), this._throwingImpale, "{=z4zrwc9K}{VALUE}% armor penetration with throwing weapons.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=b97khG1u}{VALUE}% armor penetration with throwing weapons by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._throwingImpale.Initialize("{=tYAYIRjr}Impale", DefaultSkills.Throwing, DefaultPerks.GetTierCost(10), this._throwingWeakSpot, "{=D9coiXFt}Javelins you throw can penetrate shields.", PartyRole.Personal, 0f, EffectIncrementType.AddFactor, "{=xlddWniu}{VALUE}% damage with throwing weapons by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._throwingUnstoppableForce.Initialize("{=Jat5GFDi}Unstoppable Force", DefaultSkills.Throwing, DefaultPerks.GetTierCost(11), null, "{=4MPzgKqE}{VALUE}% travel speed to your throwing weapons for every skill point above 200.", PartyRole.Personal, 0.002f, EffectIncrementType.AddFactor, "{=pDvv90Th}{VALUE}% damage with throwing weapons for every skill point above 200.", PartyRole.Personal, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._ridingFullSpeed.Initialize("{=kzy9Iduz}Full Speed", DefaultSkills.Riding, DefaultPerks.GetTierCost(1), this._ridingNimbleSteed, "{=wKSA8Qob}{VALUE}% charge damage dealt.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=DS8fM8Op}{VALUE}% charge damage dealt by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingNimbleSteed.Initialize("{=cXlnH1Jp}Nimble Steed", DefaultSkills.Riding, DefaultPerks.GetTierCost(1), this._ridingFullSpeed, "{=f8R0Hkxa}{VALUE}% maneuvering.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=zjctOvv8}{VALUE} riding skill to troops in your formation.", PartyRole.Captain, 30f, EffectIncrementType.Add, TroopUsageFlags.Mounted, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingWellStraped.Initialize("{=3lfS4iCZ}Well Strapped", DefaultSkills.Riding, DefaultPerks.GetTierCost(2), this._ridingVeterinary, "{=oKWft2IH}{VALUE}% chance of your mount dying or becoming lame after it falls in battle.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "{=IkhQPr3Z}{VALUE} daily loyalty to the governed settlement.", PartyRole.Governor, 0.5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._ridingVeterinary.Initialize("{=ZaSmz64G}Veterinary", DefaultSkills.Riding, DefaultPerks.GetTierCost(2), this._ridingWellStraped, "{=tvRYz5lr}{VALUE}% hit points to your mount.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=b0w3Fruf}{VALUE}% hit points to mounts of troops in your party.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingNomadicTraditions.Initialize("{=PB5iowxh}Nomadic Traditions", DefaultSkills.Riding, DefaultPerks.GetTierCost(3), this._ridingDeeperSacks, "{=Wrmqdoz8}{VALUE}% party speed bonus from footmen on horses.", PartyRole.PartyLeader, 0.3f, EffectIncrementType.AddFactor, "{=fPB1WdEy}{VALUE}% melee damage bonus from speed to mounted troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingDeeperSacks.Initialize("{=VWYrJCje}Deeper Sacks", DefaultSkills.Riding, DefaultPerks.GetTierCost(3), this._ridingNomadicTraditions, "{=Yp4zv2ib}{VALUE}% carrying capacity for pack animals in your party.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, "{=UC6JdOXk}{VALUE}% trade penalty for mounts.", PartyRole.PartyLeader, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._ridingSagittarius.Initialize("{=jbPZTSP4}Sagittarius", DefaultSkills.Riding, DefaultPerks.GetTierCost(4), this._ridingSweepingWind, "{=nc3carw2}{VALUE}% accuracy penalty while mounted.", PartyRole.Personal, -0.15f, EffectIncrementType.AddFactor, "{=r0epmJJJ}{VALUE}% accuracy penalty to mounted troops in your formation.", PartyRole.Captain, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.Ranged, TroopUsageFlags.Mounted | TroopUsageFlags.Ranged, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingSweepingWind.Initialize("{=gL7Ltjpc}Sweeping Wind", DefaultSkills.Riding, DefaultPerks.GetTierCost(4), this._ridingSagittarius, "{=lTafHBwZ}{VALUE}% top speed to your mount.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=Q74nUiFJ}{VALUE}% party speed.", PartyRole.PartyLeader, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingReliefForce.Initialize("{=g5I4qyjw}Relief Force", DefaultSkills.Riding, DefaultPerks.GetTierCost(5), null, "{=tx37EgiO}{VALUE} starting battle morale when you join an ongoing battle of your allies.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, "{=RVNPXS46}{VALUE}% security provided by mounted troops in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._ridingMountedWarrior.Initialize("{=ixqTFMVA}Mounted Warrior", DefaultSkills.Riding, DefaultPerks.GetTierCost(6), this._ridingHorseArcher, "{=1GwI0hcG}{VALUE}% mounted melee damage.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=f6sgEuS0}{VALUE}% mounted melee damage by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.Melee, TroopUsageFlags.Mounted | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingHorseArcher.Initialize("{=ugJfuabA}Horse Archer", DefaultSkills.Riding, DefaultPerks.GetTierCost(6), this._ridingMountedWarrior, "{=G4xCqSNG}{VALUE}% ranged damage while mounted.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=DFMsbFrB}{VALUE}% damage by mounted archers in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingShepherd.Initialize("{=I5LyCJzj}Shepherd", DefaultSkills.Riding, DefaultPerks.GetTierCost(7), this._ridingBreeder, "{=aiIPozp6}{VALUE}% herding speed penalty.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, "{=YhZj58ut}{VALUE}% chance of producing tier 2 horses in villages bound to the governed settlement.", PartyRole.Governor, 0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._ridingBreeder.Initialize("{=4Pbfs4rV}Breeder", DefaultSkills.Riding, DefaultPerks.GetTierCost(7), this._ridingShepherd, "{=Cpaw42pv}{VALUE}% daily chance of animals in your party reproducing.", PartyRole.PartyLeader, 0.01f, EffectIncrementType.AddFactor, "{=665JbYIC}{VALUE}% production rate to villages bound to the governed settlement.", PartyRole.Governor, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._ridingThunderousCharge.Initialize("{=3MLtqFPt}Thunderous Charge", DefaultSkills.Riding, DefaultPerks.GetTierCost(8), this._ridingAnnoyingBuzz, "{=qvjCYY61}{VALUE}% battle morale penalty to enemies with mounted melee kills.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=fK9GFdM8}{VALUE}% battle morale penalty to enemies with mounted melee kills by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.Melee, TroopUsageFlags.Mounted | TroopUsageFlags.Melee, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingAnnoyingBuzz.Initialize("{=Okibjv5n}Annoying Buzz", DefaultSkills.Riding, DefaultPerks.GetTierCost(8), this._ridingThunderousCharge, "{=nbbQfbli}{VALUE}% battle morale penalty to enemies with mounted ranged kills.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=wtdwqO8i}{VALUE}% battle morale penalty to enemies with mounted ranged kills by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Mounted | TroopUsageFlags.Ranged, TroopUsageFlags.Mounted | TroopUsageFlags.Ranged, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingMountedPatrols.Initialize("{=1z3oRPQu}Mounted Patrols", DefaultSkills.Riding, DefaultPerks.GetTierCost(9), this._ridingCavalryTactics, "{=pAkHwm3k}{VALUE}% escape chance to prisoners in your party.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, "{=mNbAR4uk}{VALUE}% escape chance to prisoners in the governed settlement.", PartyRole.Governor, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._ridingCavalryTactics.Initialize("{=ZMxAGDKU}Cavalry Tactics", DefaultSkills.Riding, DefaultPerks.GetTierCost(9), this._ridingMountedPatrols, "{=oboqflX9}{VALUE}% volunteering rate of cavalry troops in the settlements governed by your clan.", PartyRole.ClanLeader, 0.3f, EffectIncrementType.AddFactor, "{=mXGozqqL}{VALUE}% wages of mounted troops in the governed settlement.", PartyRole.Governor, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._ridingDauntlessSteed.Initialize("{=eYzTvFEH}Dauntless Steed", DefaultSkills.Riding, DefaultPerks.GetTierCost(10), this._ridingToughSteed, "{=7uhottjU}{VALUE}% resistance to getting staggered while mounted.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=MEr2aoeC}{VALUE} armor to all equipped armor pieces of mounted troops in your formation.", PartyRole.Captain, 5f, EffectIncrementType.Add, TroopUsageFlags.Mounted, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingToughSteed.Initialize("{=vDNbHDfq}Tough Steed", DefaultSkills.Riding, DefaultPerks.GetTierCost(10), this._ridingDauntlessSteed, "{=svkQsokb}{VALUE}% armor to your mount.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=Ful5cXFa}{VALUE} armor to mounts of troops in your formation.", PartyRole.Captain, 10f, EffectIncrementType.Add, TroopUsageFlags.Mounted, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._ridingTheWayOfTheSaddle.Initialize("{=HvYgMtXO}The Way Of The Saddle", DefaultSkills.Riding, DefaultPerks.GetTierCost(11), null, "{=nXZktHa6}{VALUE} charge damage and maneuvering for every skill point above 250.", PartyRole.Personal, 0.3f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Mounted, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsMorningExercise.Initialize("{=ipwU1JT3}Morning Exercise", DefaultSkills.Athletics, DefaultPerks.GetTierCost(1), this._athleticsWellBuilt, "{=V53EYEXx}{VALUE}% combat movement speed.", PartyRole.Personal, 0.03f, EffectIncrementType.AddFactor, "{=nRvR1Rpc}{VALUE}% combat movement speed to troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsWellBuilt.Initialize("{=bigS7KHi}Well Built", DefaultSkills.Athletics, DefaultPerks.GetTierCost(1), this._athleticsMorningExercise, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=V4zyUiai}{VALUE} hit points to foot troops in your party.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Any, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsFury.Initialize("{=furyPerk}Fury", DefaultSkills.Athletics, DefaultPerks.GetTierCost(2), this._athleticsFormFittingArmor, "{=Epwmv89M}{VALUE}% weapon handling while on foot.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=LGFsDic7}{VALUE}% weapon handling to foot troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsFormFittingArmor.Initialize("{=tp3p7R8E}Form Fitting Armor", DefaultSkills.Athletics, DefaultPerks.GetTierCost(2), this._athleticsFury, "{=86R9Ttgx}{VALUE}% armor weight.", PartyRole.Personal, -0.15f, EffectIncrementType.AddFactor, "{=WpCx75Pc}{VALUE}% combat movement speed to tier 3+ foot troops in your formation.", PartyRole.Captain, 0.04f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsImposingStature.Initialize("{=3hffzsoK}Imposing Stature", DefaultSkills.Athletics, DefaultPerks.GetTierCost(3), this._athleticsStamina, "{=qCaIau4o}{VALUE}% persuasion chance.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=NouDSrXE}{VALUE} party size.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._athleticsStamina.Initialize("{=2lCLp5eo}Stamina", DefaultSkills.Athletics, DefaultPerks.GetTierCost(3), this._athleticsImposingStature, "{=Lrm17UNm}{VALUE}% crafting stamina recovery rate.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=PNB9bHJd}{VALUE} prisoner limit and -10% escape chance to your prisoners.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsSprint.Initialize("{=864bKdc6}Sprint", DefaultSkills.Athletics, DefaultPerks.GetTierCost(4), this._athleticsPowerful, "{=mWezTaa1}{VALUE}% combat movement speed when you have no shields and no ranged weapons equipped.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=zoNKoZDZ}{VALUE}% combat movement speed to infantry troops in your formation.", PartyRole.Captain, 0.03f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsPowerful.Initialize("{=UCpyo9hw}Powerful", DefaultSkills.Athletics, DefaultPerks.GetTierCost(4), this._athleticsSprint, "{=CglYgfiY}{VALUE}% damage with melee weapons.", PartyRole.Personal, 0.04f, EffectIncrementType.AddFactor, "{=eBmaa49a}{VALUE}% melee damage by troops in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Melee, TroopUsageFlags.Melee, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsSurgingBlow.Initialize("{=zrYFYDfj}Surging Blow", DefaultSkills.Athletics, DefaultPerks.GetTierCost(5), this._athleticsBraced, "{=QiZfTNWJ}{VALUE}% damage bonus from speed while on foot.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=m6RcG1bD}{VALUE}% damage bonus from speed to troops in your formation.", PartyRole.Captain, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsBraced.Initialize("{=dU7haWkI}Braced", DefaultSkills.Athletics, DefaultPerks.GetTierCost(5), this._athleticsSurgingBlow, "{=QqVLsf0N}{VALUE}% charge damage taken.", PartyRole.Personal, -0.4f, EffectIncrementType.AddFactor, "{=Dilnx8Es}{VALUE}% charge damage taken by troops in your formation.", PartyRole.Captain, -0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsWalkItOff.Initialize("{=0pyLfrGZ}Walk It Off", DefaultSkills.Athletics, DefaultPerks.GetTierCost(6), this._athleticsAGoodDaysRest, "{=65b6daHW}{VALUE}% hit point regeneration while traveling.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=9Hv0q2lg}{VALUE} daily experience to foot troops while traveling.", PartyRole.PartyLeader, 3f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsAGoodDaysRest.Initialize("{=B7HwvV6L}A Good Days Rest", DefaultSkills.Athletics, DefaultPerks.GetTierCost(6), this._athleticsWalkItOff, "{=cCXt1jce}{VALUE}% hit point regeneration while waiting in settlements.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=fyibGRUQ}{VALUE} daily experience to foot troops while waiting in settlements.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsDurable.Initialize("{=8AKmJwv7}Durable", DefaultSkills.Athletics, DefaultPerks.GetTierCost(7), this._athleticsEnergetic, "{=4uqDestM}{VALUE} Endurance attribute.", PartyRole.Personal, 1f, EffectIncrementType.Add, "{=m993aVvX}{VALUE} daily loyalty in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._athleticsEnergetic.Initialize("{=1YxFYg3s}Energetic", DefaultSkills.Athletics, DefaultPerks.GetTierCost(7), this._athleticsDurable, "{=qPpN2wW8}{VALUE}% overburdened speed penalty.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, "{=ULY7byYc}{VALUE}% hearth growth in villages bound to the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._athleticsSteady.Initialize("{=Ye9lbBr3}Steady", DefaultSkills.Athletics, DefaultPerks.GetTierCost(8), this._athleticsStrong, "{=C8LhGtUJ}{VALUE} Control attribute.", PartyRole.Personal, 1f, EffectIncrementType.Add, "{=rkQptw1O}{VALUE}% production in farms, mines, lumber camps and clay pits bound to the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._athleticsStrong.Initialize("{=d5aK6Sv0}Strong", DefaultSkills.Athletics, DefaultPerks.GetTierCost(8), this._athleticsSteady, "{=gtlygHIk}{VALUE} Vigor attribute.", PartyRole.Personal, 1f, EffectIncrementType.Add, "{=yXaozMwY}{VALUE}% party speed by foot troops in your party.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsStrongLegs.Initialize("{=guZWnzaV}Strong Legs", DefaultSkills.Athletics, DefaultPerks.GetTierCost(9), this._athleticsStrongArms, "{=QIDr1cTd}{VALUE}% fall damage taken and +100% kick damage dealt.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "{=O3sh2iws}{VALUE}% food consumption in the governed settlement while under siege.", PartyRole.Governor, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._athleticsStrongArms.Initialize("{=qBKmIyYx}Strong Arms", DefaultSkills.Athletics, DefaultPerks.GetTierCost(9), this._athleticsStrongLegs, "{=Ztezot02}{VALUE}% damage with throwing weapons.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=a5CWbHsd}{VALUE} throwing skill to troops in your formation.", PartyRole.Captain, 20f, EffectIncrementType.Add, TroopUsageFlags.ThrownUser, TroopUsageFlags.ThrownUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsSpartan.Initialize("{=PX0Xufmr}Spartan", DefaultSkills.Athletics, DefaultPerks.GetTierCost(10), this._athleticsIgnorePain, "{=NmGcIg3j}{VALUE}% resistance to getting staggered while on foot.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=6NHvsrrx}{VALUE}% food consumption in your party.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._athleticsIgnorePain.Initialize("{=AHtFRv5T}Ignore Pain", DefaultSkills.Athletics, DefaultPerks.GetTierCost(10), this._athleticsSpartan, "{=1be7OEQB}{VALUE}% armor while on foot.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=F2H2lZJ4}{VALUE} armor to all equipped armor pieces of foot troops in your formation.", PartyRole.Captain, 5f, EffectIncrementType.Add, TroopUsageFlags.OnFoot, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._athleticsMightyBlow.Initialize("{=lbGa4ihC}Mighty Blow ", DefaultSkills.Athletics, DefaultPerks.GetTierCost(11), null, "{=cqUXbafi}You stun your enemies longer after they block your attack.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "{=LItNgwiF}{VALUE} hit points for every skill point above 250.", PartyRole.Personal, 1f, EffectIncrementType.Add, TroopUsageFlags.Any, TroopUsageFlags.Any, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._craftingIronMaker.Initialize("{=i3eT3Zjb}Efficient Iron Maker", DefaultSkills.Crafting, DefaultPerks.GetTierCost(1), this._craftingCharcoalMaker, "{=6btajdpT}You can produce crude iron more efficiently by obtaining three units of crude iron from one unit of iron ore.", PartyRole.Personal, 0f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingCharcoalMaker.Initialize("{=u5zNNZKa}Efficient Charcoal Maker", DefaultSkills.Crafting, DefaultPerks.GetTierCost(1), this._craftingIronMaker, "{=wbwoVfSq}You can use a more efficient method of charcoal production that produces three units of charcoal from two units of hardwood.", PartyRole.Personal, 0f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingSteelMaker.Initialize("{=pKquYFTX}Steel Maker", DefaultSkills.Crafting, DefaultPerks.GetTierCost(2), this._craftingCuriousSmelter, "{=qZpIdBib}You can refine two units of iron into one unit of steel, and one unit of crude iron as by-product.", PartyRole.Personal, 0f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingCuriousSmelter.Initialize("{=Tu1Sd2qg}Curious Smelter", DefaultSkills.Crafting, DefaultPerks.GetTierCost(2), this._craftingSteelMaker, "{=1dS5OjLQ}{VALUE}% learning rate of new part designs when smelting.", PartyRole.Personal, 1f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingSteelMaker2.Initialize("{=EerNV0aM}Steel Maker 2", DefaultSkills.Crafting, DefaultPerks.GetTierCost(3), this._craftingCuriousSmith, "{=mm5ZzOOV}You can refine two units of steel into one unit of fine steel, and one unit of crude iron as by-product.", PartyRole.Personal, 0f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingCuriousSmith.Initialize("{=J1GSW0yk}Curious Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(3), this._craftingSteelMaker2, "{=vWt9bvOz}{VALUE}% learning rate of new part designs when smithing.", PartyRole.Personal, 1f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingExperiencedSmith.Initialize("{=dwtIc9AG}Experienced Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(4), this._craftingSteelMaker3, "{=w1K8XDls}{VALUE}% greater chance of creating Fine weapons.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=qPJJnxM1}Successful crafting orders of notables increase your relation by {VALUE} with them.", PartyRole.Personal, 2f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingSteelMaker3.Initialize("{=c5GOJIhU}Steel Maker 3", DefaultSkills.Crafting, DefaultPerks.GetTierCost(4), this._craftingExperiencedSmith, "{=fxGdAlI2}You can refine two units of fine steel into one unit of Thamaskene steel,{newline}and one unit of crude iron as by-product.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=3b4sjuMu}{VALUE} relationships with lords and ladies for successful crafting orders.", PartyRole.Personal, 4f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingPracticalRefiner.Initialize("{=OrcSQyOb}Practical Refiner", DefaultSkills.Crafting, DefaultPerks.GetTierCost(5), this._craftingPracticalSmelter, "{=hmrUcvwz}{VALUE}% stamina spent while refining.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingPracticalSmelter.Initialize("{=KFpnAWwr}Practical Smelter", DefaultSkills.Crafting, DefaultPerks.GetTierCost(5), this._craftingPracticalRefiner, "{=NzlwbSIj}{VALUE}% stamina spent while smelting.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingVigorousSmith.Initialize("{=8hhS659w}Vigorous Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(6), this._craftingStrongSmith, "{=gtlygHIk}{VALUE} Vigor attribute.", PartyRole.Personal, 1f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingStrongSmith.Initialize("{=83iwPPVH}Controlled Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(6), this._craftingVigorousSmith, "{=C8LhGtUJ}{VALUE} Control attribute.", PartyRole.Personal, 1f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingPracticalSmith.Initialize("{=rR8iTDPI}Practical Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(7), this._craftingArtisanSmith, "{=FqmS9wcP}{VALUE}% stamina spent while smithing.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingArtisanSmith.Initialize("{=bnVCX24q}Artisan Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(7), this._craftingPracticalSmith, "{=W9pOfMAE}{VALUE}% trade penalty when selling smithing weapons.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingMasterSmith.Initialize("{=ivH8RWyb}Master Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(8), null, "{=SBTTId7I}{VALUE}% greater chance of creating masterwork weapons.", PartyRole.Personal, 0.075f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingFencerSmith.Initialize("{=SSdYsV4R}Fencer Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(9), this._craftingEnduringSmith, "{=j3QNVqP5}{VALUE} Focus Point to One Handed and Two Handed.", PartyRole.Personal, 1f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingEnduringSmith.Initialize("{=RWMACSag}Enduring Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(9), this._craftingFencerSmith, "{=4uqDestM}{VALUE} Endurance attribute.", PartyRole.Personal, 1f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._craftingSharpenedEdge.Initialize("{=knWgaYdk}Sharpened Edge", DefaultSkills.Crafting, DefaultPerks.GetTierCost(10), this._craftingSharpenedTip, "{=S7BMf2Wa}{VALUE}% swing damage of crafted weapons.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Melee, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._craftingSharpenedTip.Initialize("{=aO2JSbSq}Sharpened Tip", DefaultSkills.Crafting, DefaultPerks.GetTierCost(10), this._craftingSharpenedEdge, "{=KabSHyf0}{VALUE}% thrust damage of crafted weapons.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Melee, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._craftingLegendarySmith.Initialize("{=f4lnEplc}Legendary Smith", DefaultSkills.Crafting, DefaultPerks.GetTierCost(11), null, "{=wc15ZSpO}{VALUE}% greater chance of creating Legendary weapons, chance increases by 1% for every 5 skill points above 275.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._scoutingDayTraveler.Initialize("{=6PSgX2BP}Day Traveler", DefaultSkills.Scouting, DefaultPerks.GetTierCost(1), this._scoutingNightRunner, "{=86nHAJs9}{VALUE}% travel speed during daytime.", PartyRole.Scout, 0.02f, EffectIncrementType.AddFactor, "{=RUEfCZBG}{VALUE}% sight range during daytime in campaign map.", PartyRole.Scout, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingNightRunner.Initialize("{=B8Gq2ylh}Night Runner", DefaultSkills.Scouting, DefaultPerks.GetTierCost(1), this._scoutingDayTraveler, "{=QmaIRD7P}{VALUE}% travel speed during nighttime", PartyRole.Scout, 0.05f, EffectIncrementType.AddFactor, "{=6CteaPKm}{VALUE}% sight range during nighttime in campaign map.", PartyRole.Scout, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingPathfinder.Initialize("{=d2qGHXyx}Pathfinder", DefaultSkills.Scouting, DefaultPerks.GetTierCost(2), this._scoutingWaterDiviner, "{=ETiOGIvT}{VALUE}% travel speed on steppes and plains.", PartyRole.Scout, 0.02f, EffectIncrementType.AddFactor, "{=sAv1co78}{VALUE}% daily chance to increase relation with a notable by 1 when you enter a town.", PartyRole.PartyLeader, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingWaterDiviner.Initialize("{=gsz9DMNq}Water Diviner", DefaultSkills.Scouting, DefaultPerks.GetTierCost(2), this._scoutingPathfinder, "{=8EtK0F1K}{VALUE}% sight range while traveling on steppes and plains.", PartyRole.Scout, 0.1f, EffectIncrementType.AddFactor, "{=aW1qO2dN}{VALUE}% daily chance to increase relation with a notable by 1 when you enter a village.", PartyRole.PartyLeader, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingForestKin.Initialize("{=0XuFh3cX}Forest Kin", DefaultSkills.Scouting, DefaultPerks.GetTierCost(3), this._scoutingDesertBorn, "{=cpbKNtlZ}{VALUE}% travel speed penalty from forests if your party is composed of 75% or more infantry units.", PartyRole.Scout, -0.5f, EffectIncrementType.AddFactor, "{=xq9wJPKI}{VALUE}% tax income from villages bound to the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._scoutingDesertBorn.Initialize("{=TbBmjK8M}Desert Born", DefaultSkills.Scouting, DefaultPerks.GetTierCost(3), this._scoutingForestKin, "{=k9WaJ396}{VALUE}% travel speed on deserts and dunes.", PartyRole.Scout, 0.05f, EffectIncrementType.AddFactor, "{=nUJfb5VX}{VALUE}% tax income from the governed settlement.", PartyRole.Governor, 0.025f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._scoutingForcedMarch.Initialize("{=jhZe9Mfo}Forced March", DefaultSkills.Scouting, DefaultPerks.GetTierCost(4), this._scoutingUnburdened, "{=zky6r5Ax}{VALUE}% travel speed when the party morale is higher than 75.", PartyRole.Scout, 0.025f, EffectIncrementType.AddFactor, "{=hLbn3SBE}{VALUE} experience per day to all troops while traveling with party morale higher than 75.", PartyRole.PartyLeader, 2f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._scoutingUnburdened.Initialize("{=sA2OrT6l}Unburdened", DefaultSkills.Scouting, DefaultPerks.GetTierCost(4), this._scoutingForcedMarch, "{=N5jFSdGR}{VALUE}% overburden penalty.", PartyRole.Scout, -0.2f, EffectIncrementType.AddFactor, "{=OJ9QCJh8}{VALUE} experience per day to all troops when traveling while overburdened.", PartyRole.PartyLeader, 2f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingTracker.Initialize("{=AoaabumE}Tracker", DefaultSkills.Scouting, DefaultPerks.GetTierCost(5), this._scoutingRanger, "{=mTHliJuT}{VALUE}% track visibility duration.", PartyRole.Scout, 0.2f, EffectIncrementType.AddFactor, "{=pnAq0a40}{VALUE}% travel speed while following a hostile party.", PartyRole.Scout, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingRanger.Initialize("{=09gOOa0h}Ranger", DefaultSkills.Scouting, DefaultPerks.GetTierCost(5), this._scoutingTracker, "{=boXP9vkF}{VALUE}% track spotting distance.", PartyRole.Scout, 0.2f, EffectIncrementType.AddFactor, "{=aeK3ykbL}{VALUE}% track detection chance.", PartyRole.Scout, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingMountedScouts.Initialize("{=K9Nb117q}Mounted Scouts", DefaultSkills.Scouting, DefaultPerks.GetTierCost(6), this._scoutingPatrols, "{=DHZxUm6I}{VALUE}% sight range when your party is composed of more than %50 cavalry troops.", PartyRole.Scout, 0.1f, EffectIncrementType.AddFactor, "{=rLs30aPf}{VALUE} party size limit.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._scoutingPatrols.Initialize("{=uKc4le8Q}Patrols", DefaultSkills.Scouting, DefaultPerks.GetTierCost(6), this._scoutingMountedScouts, "{=2ljMER8Z}{VALUE} battle morale against bandit parties.", PartyRole.Scout, 5f, EffectIncrementType.Add, "{=7K0BqbFG}{VALUE}% advantage against bandits when troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingForagers.Initialize("{=LPxEDIk7}Foragers", DefaultSkills.Scouting, DefaultPerks.GetTierCost(7), this._scoutingBeastWhisperer, "{=FepLiMeY}{VALUE}% food consumption while traveling through steppes and forests.", PartyRole.Scout, -0.1f, EffectIncrementType.AddFactor, "{=kjn1D5Td}{VALUE}% disorganized state duration.", PartyRole.PartyLeader, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.NavalReduced);
			this._scoutingBeastWhisperer.Initialize("{=mrtDAhtL}Beast Whisperer", DefaultSkills.Scouting, DefaultPerks.GetTierCost(7), this._scoutingForagers, "{=jGAe89hM}{VALUE}% chance to find a mount when traveling through steppes and plains.", PartyRole.Scout, 0.05f, EffectIncrementType.AddFactor, "{=Yp4zv2ib}{VALUE}% carrying capacity for pack animals in your party.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingVillageNetwork.Initialize("{=lYQAuYaH}Village Network", DefaultSkills.Scouting, DefaultPerks.GetTierCost(8), this._scoutingRumourNetwork, "{=zj4Sz28B}{VALUE}% trade penalty with villages of your own culture.", PartyRole.PartyLeader, -0.1f, EffectIncrementType.AddFactor, "{=j9KDLaDo}{VALUE}% villager party size of villages bound to the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._scoutingRumourNetwork.Initialize("{=LwWyc6ou}Rumor Network", DefaultSkills.Scouting, DefaultPerks.GetTierCost(8), this._scoutingVillageNetwork, "{=c7V0ayuX}{VALUE}% trade penalty within cities of your own kingdom.", PartyRole.PartyLeader, -0.05f, EffectIncrementType.AddFactor, "{=JrTtFFfe}{VALUE}% hideout detection range.", PartyRole.Scout, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._scoutingVantagePoint.Initialize("{=EC2n5DBl}Vantage Point", DefaultSkills.Scouting, DefaultPerks.GetTierCost(9), this._scoutingKeenSight, "{=Y1lC59hw}{VALUE}% sight range when stationary for at least an hour.", PartyRole.Scout, 0.25f, EffectIncrementType.AddFactor, "{=POp8DAZD}{VALUE} prisoner limit.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._scoutingKeenSight.Initialize("{=3yVPrhXt}Keen Sight", DefaultSkills.Scouting, DefaultPerks.GetTierCost(9), this._scoutingVantagePoint, "{=dt1xXqbD}{VALUE}% sight penalty for traveling in forests.", PartyRole.Scout, -0.5f, EffectIncrementType.AddFactor, "{=Lr7TZOFL}{VALUE}% chance of prisoner lords escaping from your party.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingVanguard.Initialize("{=Cp7dI87a}Vanguard", DefaultSkills.Scouting, DefaultPerks.GetTierCost(10), this._scoutingRearguard, "{=9yN8Fpv6}{VALUE}% damage by your troops when they are sent as attackers.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, "{=Bzoxobzn}{VALUE}% damage by your troops when they are sent to sally out.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingRearguard.Initialize("{=e4QAc5A6}Rearguard", DefaultSkills.Scouting, DefaultPerks.GetTierCost(10), this._scoutingVanguard, "{=WlAAsJNK}{VALUE}% wounded troop recovery speed while in an army.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, "{=dlnOcIyj}{VALUE}% damage by your troops when defending at your siege camp.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._scoutingUncannyInsight.Initialize("{=M9vC9mio}Uncanny Insight", DefaultSkills.Scouting, DefaultPerks.GetTierCost(11), null, "{=4Onw6Gxa}{VALUE}% party speed for every skill point above 200 scouting skill.", PartyRole.Scout, 0.001f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsTightFormations.Initialize("{=EX5cZDLH}Tight Formations", DefaultSkills.Tactics, DefaultPerks.GetTierCost(1), this._tacticsLooseFormations, "{=eJ8AN9Au}{VALUE}% damage by your infantry to cavalry when troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=gJJ2F3iL}{VALUE}% morale penalty when troops in your formation use shield wall, square, skein, column formations.", PartyRole.Captain, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsLooseFormations.Initialize("{=9y3X0MQg}Loose Formations", DefaultSkills.Tactics, DefaultPerks.GetTierCost(1), this._tacticsTightFormations, "{=Xykn90RV}{VALUE}% damage to your infantry from ranged troops when troops are sent to confront the enemy.", PartyRole.PartyLeader, -0.1f, EffectIncrementType.AddFactor, "{=jZzVlDlf}{VALUE}% morale penalty when troops in your formation use line, loose, circle or scatter formations.", PartyRole.Captain, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsExtendedSkirmish.Initialize("{=EsYYcvcA}Extended Skirmish", DefaultSkills.Tactics, DefaultPerks.GetTierCost(2), this._tacticsDecisiveBattle, "{=Jm0GA3ak}{VALUE}% damage in snowy and forest terrains when troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=U3B7zaQb}{VALUE}% movement speed to troops in your formation in snowy and forest terrains.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsDecisiveBattle.Initialize("{=4ElA7gRS}Decisive Battle", DefaultSkills.Tactics, DefaultPerks.GetTierCost(2), this._tacticsExtendedSkirmish, "{=CcggbEVk}{VALUE}% damage in plains, steppes and deserts when your troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, "{=7yOCFsG5}{VALUE}% movement speed to troops in your formation in plains, steppes and deserts.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsSmallUnitTactics.Initialize("{=30hNRt3x}Small Unit Tactics", DefaultSkills.Tactics, DefaultPerks.GetTierCost(3), this._tacticsHordeLeader, "{=3mJMAX0Y}{VALUE} troop for the hideout crew", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, "{=GDSQyMaG}{VALUE}% movement speed to troops in your formation when there are less than 15 soldiers.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsHordeLeader.Initialize("{=Vp8Pwou8}Horde Leader", DefaultSkills.Tactics, DefaultPerks.GetTierCost(3), this._tacticsSmallUnitTactics, "{=NouDSrXE}{VALUE} party size.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, "{=y52Zz7U9}{VALUE}% army cohesion loss to commanded armies.", PartyRole.ArmyCommander, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._tacticsLawKeeper.Initialize("{=zUK9JOlb}Law Keeper", DefaultSkills.Tactics, DefaultPerks.GetTierCost(4), this._tacticsCoaching, "{=QOMr1QS7}{VALUE}% damage against bandits when your troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=yfRAX2Qv}{VALUE}% damage against bandits by troops in your formation.", PartyRole.Captain, 0.04f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsCoaching.Initialize("{=afaCdojS}Coaching", DefaultSkills.Tactics, DefaultPerks.GetTierCost(4), this._tacticsLawKeeper, "{=KSWdxKPJ}{VALUE}% damage when your troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.03f, EffectIncrementType.AddFactor, "{=9CjoaJwe}{VALUE}% damage by troops in your formation.", PartyRole.Captain, 0.01f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsSwiftRegroup.Initialize("{=nmJe4wN1}Swift Regroup", DefaultSkills.Tactics, DefaultPerks.GetTierCost(5), this._tacticsImproviser, "{=9f16mDn0}{VALUE}% disorganized state duration when a raid or siege is broken.", PartyRole.PartyMember, -0.15f, EffectIncrementType.AddFactor, "{=0pW4fcjt}{VALUE}% troops left behind when escaping from battles.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsImproviser.Initialize("{=qAn93jVN}Improviser", DefaultSkills.Tactics, DefaultPerks.GetTierCost(5), this._tacticsSwiftRegroup, "{=pFSWDNaF}No morale penalty for disorganized state in battles, in sally out or when being attacked.", PartyRole.PartyMember, 0f, EffectIncrementType.Add, "{=4V8CS018}{VALUE}% loss of troops when breaking into or out of a settlement under siege.", PartyRole.PartyLeader, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.NavalReduced);
			this._tacticsOnTheMarch.Initialize("{=kolBffjD}On The March", DefaultSkills.Tactics, DefaultPerks.GetTierCost(6), this._tacticsCallToArms, "{=C6rYWvrb}{VALUE}% fortification bonus to enemies when troops are sent to confront the enemy.", PartyRole.ArmyCommander, -0.2f, EffectIncrementType.AddFactor, "{=09npcQY0}{VALUE}% fortification bonus to the governed settlement", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsCallToArms.Initialize("{=mUubYb7v}Call To Arms", DefaultSkills.Tactics, DefaultPerks.GetTierCost(6), this._tacticsOnTheMarch, "{=3UB3qhjk}{VALUE}% movement speed to parties called to your army.", PartyRole.ArmyCommander, 0.1f, EffectIncrementType.AddFactor, "{=mAKqS7Rk}{VALUE}% influence required to call parties to your army", PartyRole.ArmyCommander, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsPickThemOfTheWalls.Initialize("{=XQkY7jkL}Pick Them Off The Walls", DefaultSkills.Tactics, DefaultPerks.GetTierCost(7), this._tacticsMakeThemPay, "{=mmRmG5AY}{VALUE}% chance for dealing double damage to siege defender troops in siege bombardment", PartyRole.Engineer, 0.25f, EffectIncrementType.AddFactor, "{=bBRA2jJp}{VALUE}% chance for dealing double damage to besieging troops in siege bombardment of the governed settlement.", PartyRole.Governor, 0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsMakeThemPay.Initialize("{=8xxeNK0o}Make Them Pay", DefaultSkills.Tactics, DefaultPerks.GetTierCost(7), this._tacticsPickThemOfTheWalls, "{=e2N77Ufi}{VALUE}% damage to defender siege engines.", PartyRole.Engineer, 0.25f, EffectIncrementType.AddFactor, "{=hDvZTHbq}{VALUE}% damage to besieging siege engines.", PartyRole.Governor, 0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsEliteReserves.Initialize("{=luDtfSN7}Elite Reserves", DefaultSkills.Tactics, DefaultPerks.GetTierCost(8), this._tacticsEncirclement, "{=zVEvl8WQ}{VALUE}% less damage to tier 3+ units when troops are sent to confront the enemy.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, "{=ldD4sVOi}{VALUE}% damage taken by troops in your formation.", PartyRole.Captain, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsEncirclement.Initialize("{=EhaMPtRX}Encirclement", DefaultSkills.Tactics, DefaultPerks.GetTierCost(8), this._tacticsEliteReserves, "{=seiduCHq}{VALUE}% damage to outnumbered enemies when troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, "{=mtB1tUIb}{VALUE}% influence cost to boost army cohesion.", PartyRole.ArmyCommander, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsPreBattleManeuvers.Initialize("{=cHgLxbbc}Pre Battle Maneuvers", DefaultSkills.Tactics, DefaultPerks.GetTierCost(9), this._tacticsBesieged, "{=dPo5goLo}{VALUE}% influence gain from winning battles.", PartyRole.PartyMember, 0.25f, EffectIncrementType.AddFactor, "{=70PZ5mFx}{VALUE}% damage per 100 skill difference with the enemy when troops are sent to confront the enemy.", PartyRole.PartyLeader, 0.01f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsBesieged.Initialize("{=ALC3Kzv9}Besieged", DefaultSkills.Tactics, DefaultPerks.GetTierCost(9), this._tacticsPreBattleManeuvers, "{=gjkWXuwC}{VALUE}% damage while besieged when troops are sent to confront the enemy.", PartyRole.PartyMember, 0.1f, EffectIncrementType.AddFactor, "{=SIMwGiJF}{VALUE}% influence gain from winning sieges.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsCounteroffensive.Initialize("{=mn5tQhyp}Counter Offensive", DefaultSkills.Tactics, DefaultPerks.GetTierCost(10), this._tacticsGensdarmes, "{=FQppujVl}{VALUE}% damage when troops are sent to confront the attacking enemy in a field battle.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, "{=4Xb1xtbF}{VALUE}% damage when troops are sent to confront the enemy while outnumbered.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._tacticsGensdarmes.Initialize("{=CTEuBfU0}Gens d'armes", DefaultSkills.Tactics, DefaultPerks.GetTierCost(10), this._tacticsCounteroffensive, "{=cPvszBhr}{VALUE}% damage to infantry by cavalry troops in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, "{=buFin46y}{VALUE} daily security in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Mounted, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._tacticsTacticalMastery.Initialize("{=8rvpcb4k}Tactical Mastery", DefaultSkills.Tactics, DefaultPerks.GetTierCost(11), null, "{=ClrLzkvx}{VALUE}% damage for every skill point above 200 tactics skill when troops are sent to confront the enemy.", PartyRole.ArmyCommander, 0.005f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._rogueryNoRestForTheWicked.Initialize("{=RyfFWmDs}No Rest for the Wicked", DefaultSkills.Roguery, DefaultPerks.GetTierCost(1), this._roguerySweetTalker, "{=yZarNiMq}{VALUE}% experience gain for bandits in your party.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, "{=IqRNTls2}{VALUE}% raid speed.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._roguerySweetTalker.Initialize("{=570wiYEe}Sweet Talker", DefaultSkills.Roguery, DefaultPerks.GetTierCost(1), this._rogueryNoRestForTheWicked, "{=P3d4nn88}{VALUE}% chance for convincing bandits to leave in peace with barter.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, "{=icyzOJZf}{VALUE}% prisoner escape chance in the governed settlement.", PartyRole.Governor, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._rogueryTwoFaced.Initialize("{=kg4Mx9j4}Two Faced", DefaultSkills.Roguery, DefaultPerks.GetTierCost(2), this._rogueryDeepPockets, "{=uDRb7FmU}{VALUE}% increased chance for sneaking into towns", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=PznnUlI3}No morale loss from converting bandit prisoners.", PartyRole.PartyLeader, 0f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryDeepPockets.Initialize("{=by1b61pn}Deep Pockets", DefaultSkills.Roguery, DefaultPerks.GetTierCost(2), this._rogueryTwoFaced, "{=ixiL39S4}Double the amount of betting allowed in tournaments.", PartyRole.Personal, 2f, EffectIncrementType.AddFactor, "{=xSKhyecU}{VALUE}% bandit troop wages.", PartyRole.Personal, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryInBestLight.Initialize("{=xoARIHde}In Best Light", DefaultSkills.Roguery, DefaultPerks.GetTierCost(3), this._rogueryKnowHow, "{=fcraUMzb}{VALUE} extra troop from village notables when successfully forced for volunteers.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, "{=UYYntLzb}{VALUE}% faster recovery from raids for your villages.", PartyRole.ClanLeader, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryKnowHow.Initialize("{=tvoN5ynt}Know-How", DefaultSkills.Roguery, DefaultPerks.GetTierCost(3), this._rogueryInBestLight, "{=swgcsLOA}{VALUE}% more loot from defeated villagers and caravans.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, "{=XwmYu5rH}{VALUE} security per day in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryPromises.Initialize("{=XZOtTuxA}Promises", DefaultSkills.Roguery, DefaultPerks.GetTierCost(4), this._rogueryManhunter, "{=jKUmtH7z}{VALUE}% food consumption for bandit units in your party.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, "{=zHQuHeIg}{VALUE}% recruitment rate for bandit prisoners in your party.", PartyRole.PartyLeader, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._rogueryManhunter.Initialize("{=GeB42ygg}Manhunter", DefaultSkills.Roguery, DefaultPerks.GetTierCost(4), this._rogueryPromises, "{=pcys1RSF}{VALUE}% better deals with ransom broker for regular troops.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=POp8DAZD}{VALUE} prisoner limit.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryScarface.Initialize("{=XqSn5Uo0}Scarface", DefaultSkills.Roguery, DefaultPerks.GetTierCost(5), this._rogueryWhiteLies, "{=FaGc9xR4}{VALUE}% chance for bandits, villagers and caravans to surrender.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=1IYP1wHc}{VALUE}% chance per day to increase relation with a notable by 1 in the governed settlement.", PartyRole.Governor, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._rogueryWhiteLies.Initialize("{=F51HfzZj}White Lies", DefaultSkills.Roguery, DefaultPerks.GetTierCost(5), this._rogueryScarface, "{=mseUsbjg}{VALUE}% crime rating decrease rate.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=R8vLC6j0}{VALUE}% chance to get 1 relation per day with a random notable in the governed settlement.", PartyRole.Governor, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._roguerySmugglerConnections.Initialize("{=E8a2joMO}Smuggler Connections", DefaultSkills.Roguery, DefaultPerks.GetTierCost(6), this._rogueryPartnersInCrime, "{=LRa1P7Ha}You can trade in towns while in disguise.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=grbvZM4z}{VALUE}% trade penalty when you are trading with a faction you have crime rating against.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryPartnersInCrime.Initialize("{=2PVm7NON}Partners in Crime", DefaultSkills.Roguery, DefaultPerks.GetTierCost(6), this._roguerySmugglerConnections, "{=1X5mtfDn}Surrendering bandit parties can be recruited.", PartyRole.PartyLeader, 0f, EffectIncrementType.AddFactor, "{=mNleBavO}{VALUE}% damage by bandit troops in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._rogueryOneOfTheFamily.Initialize("{=oumTabhS}One of the Family", DefaultSkills.Roguery, DefaultPerks.GetTierCost(7), this._roguerySaltTheEarth, "{=w0LOgr9e}{VALUE} bonus Vigor and Control skills to bandit units in your party", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, "{=Dn0yNCn8}{VALUE} recruitment slot when recruiting from gang leaders.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._roguerySaltTheEarth.Initialize("{=tuv1O7ig}Salt the Earth", DefaultSkills.Roguery, DefaultPerks.GetTierCost(7), this._rogueryOneOfTheFamily, "{=MesU0nGW}{VALUE}% more loot when villagers comply to your hostile actions.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, "{=YbioVwyr}{VALUE}% tariff revenue in the governed settlement.", PartyRole.Governor, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryCarver.Initialize("{=7gZo2SY4}Carver", DefaultSkills.Roguery, DefaultPerks.GetTierCost(8), this._rogueryRansomBroker, "{=g2Zy1Bso}{VALUE}% damage with civilian weapons.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=hiH9dVhH}{VALUE}% one handed damage by troops under your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.OneHandedUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._rogueryRansomBroker.Initialize("{=W2WXkiAh}Ransom Broker", DefaultSkills.Roguery, DefaultPerks.GetTierCost(8), this._rogueryCarver, "{=7gabTf4P}{VALUE}% better deals for heroes from ransom brokers.", PartyRole.PartyLeader, 0.25f, EffectIncrementType.AddFactor, "{=8aajPkKG}{VALUE}% escape chance for hero prisoners.", PartyRole.PartyLeader, -0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._rogueryArmsDealer.Initialize("{=5bmlZ26b}Arms Dealer", DefaultSkills.Roguery, DefaultPerks.GetTierCost(9), this._rogueryDirtyFighting, "{=o5rp0ViP}{VALUE}% sell price penalty for weapons.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, "{=nmTai1Vw}{VALUE}% militia per day in the besieged governed settlement.", PartyRole.Governor, 2f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._rogueryDirtyFighting.Initialize("{=bb1hS9I4}Dirty Fighting", DefaultSkills.Roguery, DefaultPerks.GetTierCost(9), this._rogueryArmsDealer, "{=bm3eSbBD}{VALUE}% stun duration for kicking.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=iuCYTaMJ}{VALUE} random food item will be smuggled to the besieged governed settlement.", PartyRole.Governor, 2f, EffectIncrementType.Add, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._rogueryDashAndSlash.Initialize("{=w1B71sNj}Dash and Slash", DefaultSkills.Roguery, DefaultPerks.GetTierCost(10), this._rogueryFleetFooted, "{=QiZfTNWJ}{VALUE}% damage bonus from speed while on foot.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=hRCvgbQ5}{VALUE}% two handed weapon damage by troops in your formation.", PartyRole.Captain, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.TwoHandedUser, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._rogueryFleetFooted.Initialize("{=yY5iDvAb}Fleet Footed", DefaultSkills.Roguery, DefaultPerks.GetTierCost(10), this._rogueryDashAndSlash, "{=93Z7G161}{VALUE}% combat movement speed while no weapons or shields are equipped.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=lSebD5Fa}{VALUE}% escape chance when imprisoned by mobile parties.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.OnFoot, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._rogueryRogueExtraordinaire.Initialize("{=U3cgqyUE}Rogue Extraordinaire", DefaultSkills.Roguery, DefaultPerks.GetTierCost(11), null, "{=ClrwacPi}{VALUE}% loot amount for every skill point above 200.", PartyRole.Personal, 0.01f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmVirile.Initialize("{=mbqoZ4WH}Virile", DefaultSkills.Charm, DefaultPerks.GetTierCost(1), this._charmSelfPromoter, "{=pdQbJrr4}{VALUE}% more likely to have children.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=29R5VkXa}{VALUE}% daily chance to get +1 relation with a random notable in the governed settlement while a continuous project is active.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmSelfPromoter.Initialize("{=hkG9ATZy}Self Promoter", DefaultSkills.Charm, DefaultPerks.GetTierCost(1), this._charmVirile, "{=qARDRFqO}{VALUE} renown when a tournament is won.", PartyRole.Personal, 3f, EffectIncrementType.Add, "{=PSvarWWW}{VALUE} morale while defending in a besieged settlement.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmOratory.Initialize("{=OZXEMb2C}Oratory", DefaultSkills.Charm, DefaultPerks.GetTierCost(2), this._charmWarlord, "{=qRoQuHe4}{VALUE} renown and influence for each issue resolved", PartyRole.Personal, 1f, EffectIncrementType.Add, "{=YBmzuIbm}{VALUE} relationship with a random notable of your kingdom when an enemy lord is defeated.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmWarlord.Initialize("{=jiWr5Rlz}Warlord", DefaultSkills.Charm, DefaultPerks.GetTierCost(2), this._charmOratory, "{=IbQlvyY5}{VALUE}% influence gain from battles.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=231BaeH9}{VALUE} relationship with a random lord of your kingdom when an enemy lord is defeated.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._charmForgivableGrievances.Initialize("{=l863hIyN}Forgivable Grievances", DefaultSkills.Charm, DefaultPerks.GetTierCost(3), this._charmMeaningfulFavors, "{=BCB08mNZ}{VALUE}% chance of avoiding critical failure on persuasion.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=bFMDTiLE}{VALUE}% daily chance to increase relations with a random lord or notable with negative relations with you when you are in a settlement.", PartyRole.Personal, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmMeaningfulFavors.Initialize("{=4hUEryJ6}Meaningful Favors", DefaultSkills.Charm, DefaultPerks.GetTierCost(3), this._charmForgivableGrievances, "{=T1N2w4uK}{VALUE}% chance for double persuasion success.", PartyRole.Personal, 0.1f, EffectIncrementType.AddFactor, "{=6WP4OkKt}{VALUE}% daily chance to increase relations with powerful notables in the governed settlement.", PartyRole.Governor, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmInBloom.Initialize("{=ZlXSlx0p}In Bloom", DefaultSkills.Charm, DefaultPerks.GetTierCost(4), this._charmYoungAndRespectful, "{=aVWb6aoQ}{VALUE}% relationship gain with the opposing gender.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=SimMOKbW}{VALUE}% daily chance to increase relations with a random notable of opposed sex in the governed settlement.", PartyRole.Governor, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmYoungAndRespectful.Initialize("{=TpzZgFsA}Young And Respectful", DefaultSkills.Charm, DefaultPerks.GetTierCost(4), this._charmInBloom, "{=3MOJjS7A}{VALUE}% relationship gain with the same gender.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=7e397ieb}{VALUE}% daily chance to increase relations with a random notable of same sex in the governed settlement.", PartyRole.Governor, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmFirebrand.Initialize("{=EbKP7Xx5}Firebrand", DefaultSkills.Charm, DefaultPerks.GetTierCost(5), this._charmFlexibleEthics, "{=vYj0z0zr}{VALUE}% influence cost to initiate kingdom decisions.", PartyRole.ClanLeader, -0.25f, EffectIncrementType.AddFactor, "{=4ajo4jvp}{VALUE} recruitment slot from rural notables.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmFlexibleEthics.Initialize("{=58Imsasy}Flexible Ethics", DefaultSkills.Charm, DefaultPerks.GetTierCost(5), this._charmFirebrand, "{=HkOatwqw}{VALUE}% influence cost when voting for kingdom proposals made by others.", PartyRole.Personal, -0.3f, EffectIncrementType.AddFactor, "{=FbAGhzbI}{VALUE} recruitment slot from urban notables.", PartyRole.Personal, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmEffortForThePeople.Initialize("{=RIiVDdi0}Effort For The People", DefaultSkills.Charm, DefaultPerks.GetTierCost(6), this._charmSlickNegotiator, "{=P2eOw2sQ}{VALUE} relation with the nearest settlement owner clan when you clear a hideout. +1 town loyalty if it is your clan.", PartyRole.Personal, 3f, EffectIncrementType.Add, "{=FpleMw35}{VALUE}% barter penalty with lords of same culture.", PartyRole.Personal, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmSlickNegotiator.Initialize("{=WOqxWM67}Slick Negotiator", DefaultSkills.Charm, DefaultPerks.GetTierCost(6), this._charmEffortForThePeople, "{=AqpEXxNy}{VALUE}% hiring costs of mercenary troops.", PartyRole.Personal, -0.2f, EffectIncrementType.AddFactor, "{=6ex96ekx}{VALUE}% barter penalty with lords of different cultures.", PartyRole.Personal, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmGoodNatured.Initialize("{=2y7gahYi}Good Natured", DefaultSkills.Charm, DefaultPerks.GetTierCost(7), this._charmTribute, "{=aitgGIog}{VALUE}% influence return when a supported proposal fails to pass.", PartyRole.Personal, 1f, EffectIncrementType.AddFactor, "{=fpaeONmG}{VALUE} extra relationship when you increase relationship with merciful lords.", PartyRole.Personal, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmTribute.Initialize("{=dSBbSHkM}Tribute", DefaultSkills.Charm, DefaultPerks.GetTierCost(7), this._charmGoodNatured, "{=nJu03DL9}{VALUE}% relationship bonus when you pay more than minimum amount in barters.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=iqJQd4D8}{VALUE} extra relationship when you increase relationship with cruel lords.", PartyRole.Personal, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmMoralLeader.Initialize("{=zUXUrGWa}Moral Leader", DefaultSkills.Charm, DefaultPerks.GetTierCost(8), this._charmNaturalLeader, "{=9mlBbzLx}{VALUE} persuasion success required against characters of your own culture.", PartyRole.Personal, -1f, EffectIncrementType.Add, "{=Cm0OcbsV}{VALUE} relation with settlement notables when a project is completed in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmNaturalLeader.Initialize("{=qaZDUknZ}Natural Leader", DefaultSkills.Charm, DefaultPerks.GetTierCost(8), this._charmMoralLeader, "{=dyVvsBMs}{VALUE} persuasion success required against characters of different cultures.", PartyRole.Personal, -1f, EffectIncrementType.Add, "{=30eSZeZd}{VALUE}% experience gain for companions.", PartyRole.ClanLeader, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmPublicSpeaker.Initialize("{=16fxd9fN}Public Speaker", DefaultSkills.Charm, DefaultPerks.GetTierCost(9), this._charmParade, "{=z4naITkR}{VALUE}% renown gain from battles.", PartyRole.PartyLeader, 0.3f, EffectIncrementType.AddFactor, "{=J7JaXZm8}{VALUE}% effect from forums, marketplaces and festivals.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmParade.Initialize("{=DTnaWgAv}Parade", DefaultSkills.Charm, DefaultPerks.GetTierCost(9), this._charmPublicSpeaker, "{=yA2P7w9N}{VALUE} loyalty bonus to settlement while waiting in the settlement.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=rHtwp8ag}{VALUE}% daily chance to gain +1 relationship with a random lord in the same army.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmCamaraderie.Initialize("{=p2zZGkZw}Camaraderie", DefaultSkills.Charm, DefaultPerks.GetTierCost(10), null, "{=l2ZKUJQY}Double the relation gain for helping lords in battle.", PartyRole.Personal, 2f, EffectIncrementType.AddFactor, "{=XmwIHIMN}{VALUE} companion limit", PartyRole.ClanLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._charmImmortalCharm.Initialize("{=9XWiXokY}Immortal Charm", DefaultSkills.Charm, DefaultPerks.GetTierCost(11), null, "{=BjLYCHMD}{VALUE} influence per day.", PartyRole.Personal, 5f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._leadershipCombatTips.Initialize("{=Cb5s9HlD}Combat Tips", DefaultSkills.Leadership, DefaultPerks.GetTierCost(1), this._leadershipRaiseTheMeek, "{=76TOkicW}{VALUE} experience per day to all troops in party.", PartyRole.PartyLeader, 2f, EffectIncrementType.Add, "{=z3OU7vrn}{VALUE} to troop tiers when recruiting from same culture.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._leadershipRaiseTheMeek.Initialize("{=JGCtv8om}Raise The Meek", DefaultSkills.Leadership, DefaultPerks.GetTierCost(1), this._leadershipCombatTips, "{=Ra2poaEh}{VALUE} experience per day to tier 1 and 2 troops.", PartyRole.PartyLeader, 4f, EffectIncrementType.Add, "{=CjLuIEgh}{VALUE} experience per day to each troop in garrison in the governed settlement.", PartyRole.Governor, 3f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._leadershipFerventAttacker.Initialize("{=MhRF64eR}Fervent Attacker", DefaultSkills.Leadership, DefaultPerks.GetTierCost(2), this._leadershipStoutDefender, "{=o7xn0ybm}{VALUE} starting battle morale when attacking.", PartyRole.PartyLeader, 4f, EffectIncrementType.Add, "{=AbulTQm9}{VALUE}% recruitment rate of tier 1, 2 and 3 prisoners.", PartyRole.PartyLeader, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._leadershipStoutDefender.Initialize("{=YogcurDJ}Stout Defender", DefaultSkills.Leadership, DefaultPerks.GetTierCost(2), this._leadershipFerventAttacker, "{=qO1cJ3K9}{VALUE} starting battle morale when defending.", PartyRole.PartyLeader, 8f, EffectIncrementType.Add, "{=qItLTWR2}{VALUE}% recruitment rate of tier 4+ prisoners.", PartyRole.PartyLeader, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._leadershipAuthority.Initialize("{=CeCAMvkX}Authority", DefaultSkills.Leadership, DefaultPerks.GetTierCost(3), this._leadershipHeroicLeader, "{=bezXAM92}{VALUE}% security bonus from the town garrison in the governing settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, "{=rLs30aPf}{VALUE} party size limit.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._leadershipHeroicLeader.Initialize("{=7aX2eh5x}Heroic Leader", DefaultSkills.Leadership, DefaultPerks.GetTierCost(3), this._leadershipAuthority, "{=m993aVvX}{VALUE} daily loyalty in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, "{=yvyVugUN}{VALUE}% battle morale penalty to enemies when troops in your formation kill an enemy.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._leadershipLoyaltyAndHonor.Initialize("{=UJYaonYM}Loyalty and Honor", DefaultSkills.Leadership, DefaultPerks.GetTierCost(4), this._leadershipFamousCommander, "{=wBURlfzR}Tier 3+ troops in your party no longer retreat due to low morale", PartyRole.PartyLeader, 3f, EffectIncrementType.Add, "{=kuu7M6aQ}{VALUE}% faster non-bandit prisoner recruitment.", PartyRole.PartyLeader, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._leadershipFamousCommander.Initialize("{=FQkHkMhw}Famous Commander", DefaultSkills.Leadership, DefaultPerks.GetTierCost(4), this._leadershipLoyaltyAndHonor, "{=z4naITkR}{VALUE}% renown gain from battles.", PartyRole.Personal, 0.5f, EffectIncrementType.AddFactor, "{=CkJarFvq}{VALUE} experience to troops on recruitment.", PartyRole.Personal, 200f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._leadershipPresence.Initialize("{=6RckjM4S}Presence", DefaultSkills.Leadership, DefaultPerks.GetTierCost(5), this._leadershipLeaderOfTheMasses, "{=UgRGBWhn}{VALUE} security per day while waiting in a town.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=9JN5bc7f}No morale penalty for recruiting prisoners of your faction.", PartyRole.PartyLeader, 0f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._leadershipLeaderOfTheMasses.Initialize("{=T5rM9XgO}Leader of the Masses", DefaultSkills.Leadership, DefaultPerks.GetTierCost(5), this._leadershipPresence, "{=VUma8oHz}{VALUE} party size for each town you control.", PartyRole.ClanLeader, 5f, EffectIncrementType.Add, "{=ptmYmT6B}{VALUE}% experience from battles shared with the troops in your party.", PartyRole.PartyLeader, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._leadershipVeteransRespect.Initialize("{=vWGQNcu5}Veteran's Respect", DefaultSkills.Leadership, DefaultPerks.GetTierCost(6), this._leadershipCitizenMilitia, "{=wSLO8VgG}{VALUE} garrison size in the governed settlement.", PartyRole.Governor, 20f, EffectIncrementType.Add, "{=lsnrQCB8}Bandits can be converted into regular troops.", PartyRole.PartyLeader, 0f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._leadershipCitizenMilitia.Initialize("{=vZtLm43v}Citizen Militia", DefaultSkills.Leadership, DefaultPerks.GetTierCost(6), this._leadershipVeteransRespect, "{=vMWC5dR8}{VALUE}% rate of militias will spawn as veteran troops in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.Add, "{=QPfj9Dbj}{VALUE}% morale from victories.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._leadershipInspiringLeader.Initialize("{=kaEzJUTW}Inspiring Leader", DefaultSkills.Leadership, DefaultPerks.GetTierCost(7), this._leadershipUpliftingSpirit, "{=M04V0cmt}{VALUE}% influence cost for calling parties to an army.", PartyRole.ArmyCommander, -0.2f, EffectIncrementType.AddFactor, "{=je77ZaN7}{VALUE}% experience to troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._leadershipUpliftingSpirit.Initialize("{=EbROfVJJ}Uplifting Spirit", DefaultSkills.Leadership, DefaultPerks.GetTierCost(7), this._leadershipInspiringLeader, "{=FZ06ALO6}{VALUE} battle morale in siege battles.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, "{=rLs30aPf}{VALUE} party size limit.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._leadershipTrustedCommander.Initialize("{=6ETg3maz}Trusted Commander", DefaultSkills.Leadership, DefaultPerks.GetTierCost(8), this._leadershipLeadByExample, "{=dAS81esi}{VALUE}% recruitment rate for ranged prisoners.", PartyRole.PartyLeader, 0.5f, EffectIncrementType.AddFactor, "{=lutYwHwt}{VALUE}% experience for troops, when they are sent to confront the enemy.", PartyRole.PartyLeader, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._leadershipLeadByExample.Initialize("{=WFFlp3Qi}Lead by Example", DefaultSkills.Leadership, DefaultPerks.GetTierCost(8), this._leadershipTrustedCommander, "{=tEsgNQOZ}{VALUE}% recruitment rate for infantry prisoners.", PartyRole.PartyLeader, 0.5f, EffectIncrementType.AddFactor, "{=aeceOwWb}{VALUE}% shared experience for cavalry troops.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.LandOnly);
			this._leadershipMakeADifference.Initialize("{=5uW9zKTN}Make a Difference", DefaultSkills.Leadership, DefaultPerks.GetTierCost(9), this._leadershipGreatLeader, "{=YaPOTaMJ}{VALUE}% battle morale to troops when you kill an enemy in battle.", PartyRole.Personal, 1f, EffectIncrementType.AddFactor, "{=MMMuSlOW}{VALUE}% shared experience for archers.", PartyRole.PartyLeader, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._leadershipGreatLeader.Initialize("{=3hzSmrMw}Great Leader", DefaultSkills.Leadership, DefaultPerks.GetTierCost(9), this._leadershipMakeADifference, "{=p8pviWlQ}{VALUE} battle morale to troops at the beginning of a battle.", PartyRole.ArmyCommander, 5f, EffectIncrementType.Add, "{=LGH67bOj}{VALUE} battle morale to troops that are of same culture as you.", PartyRole.PartyLeader, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._leadershipWePledgeOurSwords.Initialize("{=3GHIb7YX}We Pledge our Swords", DefaultSkills.Leadership, DefaultPerks.GetTierCost(10), this._leadershipTalentMagnet, "{=0AUYrhGw}{VALUE} companion limit.", PartyRole.Personal, 1f, EffectIncrementType.Add, "{=FkkVHjBP}{VALUE} battle morale at the beginning of the battle for each tier 6 troop in the party up to 10 morale.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._leadershipTalentMagnet.Initialize("{=pFfqWRnf}Talent Magnet", DefaultSkills.Leadership, DefaultPerks.GetTierCost(10), this._leadershipWePledgeOurSwords, "{=rLs30aPf}{VALUE} party size limit.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, "{=gqboke7l}{VALUE} clan party limit.", PartyRole.ClanLeader, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._leadershipUltimateLeader.Initialize("{=FK3W0SKk}Ultimate Leader", DefaultSkills.Leadership, DefaultPerks.GetTierCost(11), null, "{=Q72PJYtf}{VALUE} party size for each leadership point above 250.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeAppraiser.Initialize("{=b3PsxeiB}Appraiser", DefaultSkills.Trade, DefaultPerks.GetTierCost(1), this._tradeWholeSeller, "{=wki8aFec}{VALUE}% price penalty while selling equipment.", PartyRole.PartyLeader, -0.15f, EffectIncrementType.AddFactor, "{=gHUQfWlg}Your profits are marked.", PartyRole.Personal, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeWholeSeller.Initialize("{=lTNpxGoh}Whole Seller", DefaultSkills.Trade, DefaultPerks.GetTierCost(1), this._tradeAppraiser, "{=9Y4rMcYj}{VALUE}% price penalty while selling trade goods.", PartyRole.PartyLeader, -0.15f, EffectIncrementType.AddFactor, "{=gHUQfWlg}Your profits are marked.", PartyRole.Personal, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeCaravanMaster.Initialize("{=5acLha5Q}Caravan Master", DefaultSkills.Trade, DefaultPerks.GetTierCost(2), this._tradeMarketDealer, "{=SPs04fam}{VALUE}% carrying capacity for your party.", PartyRole.Quartermaster, 0.3f, EffectIncrementType.AddFactor, "{=QUYwIYEi}Item prices are marked relative to the average price.", PartyRole.Personal, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeMarketDealer.Initialize("{=InLGoUbB}Market Dealer", DefaultSkills.Trade, DefaultPerks.GetTierCost(2), this._tradeCaravanMaster, "{=Si3QiLW4}{VALUE}% cost of bartering for safe passage.", PartyRole.ClanLeader, -0.5f, EffectIncrementType.AddFactor, "{=QUYwIYEi}Item prices are marked relative to the average price.", PartyRole.Personal, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeDistributedGoods.Initialize("{=nxkNY4YG}Distributed Goods", DefaultSkills.Trade, DefaultPerks.GetTierCost(3), this._tradeLocalConnection, "{=we6jYdRD}Double the relationship gain by resolved issues with artisans.", PartyRole.Personal, 2f, EffectIncrementType.AddFactor, "{=RYkPTHv1}{VALUE}% price penalty while buying from villages.", PartyRole.Quartermaster, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeLocalConnection.Initialize("{=mznjEwjC}Local Connection", DefaultSkills.Trade, DefaultPerks.GetTierCost(3), this._tradeDistributedGoods, "{=ORencCvQ}Double the relationship gain by resolved issues with merchants.", PartyRole.Personal, 2f, EffectIncrementType.Add, "{=AAYplFKi}{VALUE}% price penalty while selling animals.", PartyRole.Quartermaster, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeTravelingRumors.Initialize("{=3j6Ec63l}Traveling Rumors", DefaultSkills.Trade, DefaultPerks.GetTierCost(4), this._tradeTollgates, "{=DV2kW53e}Your caravans gather trade rumors.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=D2nbscmg}{VALUE} gold for each villager party visiting the governed settlement.", PartyRole.Governor, 20f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeTollgates.Initialize("{=JnSh4Fmz}Toll Gates", DefaultSkills.Trade, DefaultPerks.GetTierCost(4), this._tradeTravelingRumors, "{=SOHgkGKy}Your workshops gather trade rumors.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=bteVVFh0}{VALUE} gold for each caravan visiting the governed settlement.", PartyRole.Governor, 30f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeArtisanCommunity.Initialize("{=8f8UGq46}Artisan Community", DefaultSkills.Trade, DefaultPerks.GetTierCost(5), this._tradeGreatInvestor, "{=CBSDuOmp}{VALUE} daily renown from every profiting workshop.", PartyRole.ClanLeader, 1f, EffectIncrementType.Add, "{=amA9OfPU}{VALUE} recruitment slot when recruiting from merchant notables. ", PartyRole.Quartermaster, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeGreatInvestor.Initialize("{=g9qLrEb4}Great Investor", DefaultSkills.Trade, DefaultPerks.GetTierCost(5), this._tradeArtisanCommunity, "{=aYpbyTfA}{VALUE} daily renown from every profiting caravan.", PartyRole.ClanLeader, 1f, EffectIncrementType.Add, "{=m41r7FPw}{VALUE}% companion recruitment cost.", PartyRole.Quartermaster, -0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeMercenaryConnections.Initialize("{=vivNLdHp}Mercenary Connections", DefaultSkills.Trade, DefaultPerks.GetTierCost(6), this._tradeContentTrades, "{=HrTFr1ox}{VALUE}% workshop production rate.", PartyRole.Governor, 0.25f, EffectIncrementType.AddFactor, "{=GNtTFR0j}{VALUE}% mercenary troop wages in your party.", PartyRole.PartyLeader, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeContentTrades.Initialize("{=FV4SWLQx}Content Trades", DefaultSkills.Trade, DefaultPerks.GetTierCost(6), this._tradeMercenaryConnections, "{=Eo958e7R}{VALUE}% tariff income in the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, "{=Oq1K7oDW}{VALUE}% wages paid while waiting in settlements.", PartyRole.PartyLeader, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeInsurancePlans.Initialize("{=aYQybo4E}Insurance Plans", DefaultSkills.Trade, DefaultPerks.GetTierCost(7), this._tradeRapidDevelopment, "{=NMnpGic4}{VALUE} denar return when one of your caravans is destroyed.", PartyRole.ClanLeader, 5000f, EffectIncrementType.Add, "{=xe0dX5QQ}{VALUE}% price penalty while buying food items.", PartyRole.Quartermaster, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeRapidDevelopment.Initialize("{=u9oONz9o}Rapid Development", DefaultSkills.Trade, DefaultPerks.GetTierCost(7), this._tradeInsurancePlans, "{=EdCkK2c4}{VALUE} denar return for each workshop when workshop's town is captured by an enemy.", PartyRole.ClanLeader, 5000f, EffectIncrementType.Add, "{=4ORpHfu2}{VALUE}% price penalty while buying clay, iron, silk and silver.", PartyRole.Quartermaster, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeGranaryAccountant.Initialize("{=TFy2VYtM}Granary Accountant", DefaultSkills.Trade, DefaultPerks.GetTierCost(8), this._tradeTradeyardForeman, "{=SyxQF0tM}{VALUE}% price penalty while selling food items.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, "{=JnQcDyAz}{VALUE}% production rate to grain, olives, fish, date in villages bound to the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeTradeyardForeman.Initialize("{=QqKNxmeF}Tradeyard Foreman", DefaultSkills.Trade, DefaultPerks.GetTierCost(8), this._tradeGranaryAccountant, "{=KgrnmR73}{VALUE}% price penalty while selling pottery, tools, silk and jewelry.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, "{=mN3fLgtx}{VALUE}% production rate to clay, iron, silk and silver in villages bound to the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeSwordForBarter.Initialize("{=AIsDxCeG}Sword For Barter", DefaultSkills.Trade, DefaultPerks.GetTierCost(9), this._tradeSelfMadeMan, "{=AqpEXxNy}{VALUE}% hiring costs of mercenary troops.", PartyRole.Personal, -0.2f, EffectIncrementType.AddFactor, "{=68ye0NpS}{VALUE}% caravan guard wages.", PartyRole.Quartermaster, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeSelfMadeMan.Initialize("{=uHJltZ5D}Self-made Man", DefaultSkills.Trade, DefaultPerks.GetTierCost(9), this._tradeSwordForBarter, "{=rTbVn6sJ}{VALUE}% barter penalty for items.", PartyRole.Personal, -0.5f, EffectIncrementType.AddFactor, "{=Q9VCvUTg}{VALUE}% build speed for marketplace, kiln and aqueduct projects.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeSilverTongue.Initialize("{=5rDdJpJo}Silver Tongue", DefaultSkills.Trade, DefaultPerks.GetTierCost(10), this._tradeSpringOfGold, "{=UzKyyfbF}{VALUE}% gold required while persuading lords to defect to your faction.", PartyRole.Personal, -0.15f, EffectIncrementType.AddFactor, "{=Kb9uC4gQ}{VALUE}% better trade deals from caravans and villagers", PartyRole.Quartermaster, 0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeSpringOfGold.Initialize("{=K0SRwH6E}Spring of Gold", DefaultSkills.Trade, DefaultPerks.GetTierCost(10), this._tradeSilverTongue, "{=gu7EN92A}{VALUE}% denars of interest income per day based on your current denars up to 1000 denars.", PartyRole.ClanLeader, 0.001f, EffectIncrementType.AddFactor, "{=XmqJb7RN}{VALUE}% effect from boosting projects in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeManOfMeans.Initialize("{=Jy2ap8L1}Man of Means", DefaultSkills.Trade, DefaultPerks.GetTierCost(11), this._tradeTrickleDown, "{=7QadTbWs}{VALUE}% costs of recruiting minor faction clans into your clan.", PartyRole.ClanLeader, -0.2f, EffectIncrementType.AddFactor, "{=lA0eEkGP}{VALUE}% ransom cost for your freedom.", PartyRole.Personal, -0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeTrickleDown.Initialize("{=L4fz3Jdr}Trickle Down", DefaultSkills.Trade, DefaultPerks.GetTierCost(11), this._tradeManOfMeans, "{=ANhbaAhL}{VALUE} relationship with merchants if 10.000 or more denars are spent on a single deal.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, "{=REZyGJGH}{VALUE} daily prosperity while building a project in the governed settlement.", PartyRole.Governor, 2f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._tradeEverythingHasAPrice.Initialize("{=cRwNeSzb}Everything Has a Price", DefaultSkills.Trade, DefaultPerks.GetTierCost(12), null, "{=HeefccTC}You can now trade settlements in barter.", PartyRole.Personal, 0f, EffectIncrementType.Invalid, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardWarriorsDiet.Initialize("{=mIDsxe1O}Warrior's Diet", DefaultSkills.Steward, DefaultPerks.GetTierCost(1), this._stewardFrugal, "{=6NHvsrrx}{VALUE}% food consumption in your party.", PartyRole.Quartermaster, -0.1f, EffectIncrementType.AddFactor, "{=mSvfxXVW}No morale penalty from having single type of food.", PartyRole.PartyLeader, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._stewardFrugal.Initialize("{=eJIbMa8P}Frugal", DefaultSkills.Steward, DefaultPerks.GetTierCost(1), this._stewardWarriorsDiet, "{=CJB5HCsI}{VALUE}% wages in your party.", PartyRole.Quartermaster, -0.05f, EffectIncrementType.AddFactor, "{=OTyYJ2Bt}{VALUE}% recruitment costs.", PartyRole.PartyLeader, -0.15f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardSevenVeterans.Initialize("{=2ryLuN2i}Seven Veterans", DefaultSkills.Steward, DefaultPerks.GetTierCost(2), this._stewardDrillSergant, "{=gX0edfpK}{VALUE} daily experience for tier 4+ troops in your party.", PartyRole.Quartermaster, 4f, EffectIncrementType.Add, "{=vMWC5dR8}{VALUE}% rate of militias will spawn as veteran troops in the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardDrillSergant.Initialize("{=L9k4bovO}Drill Sergeant", DefaultSkills.Steward, DefaultPerks.GetTierCost(2), this._stewardSevenVeterans, "{=UYhJZya5}{VALUE} daily experience to troops in your party.", PartyRole.Quartermaster, 2f, EffectIncrementType.Add, "{=B2msxAju}{VALUE}% garrison wages in the governed settlement.", PartyRole.Governor, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardSweatshops.Initialize("{=jbAtOsIy}Sweatshops", DefaultSkills.Steward, DefaultPerks.GetTierCost(3), this._stewardStiffUpperLip, "{=6wqJA77K}{VALUE}% production rate to owned workshops.", PartyRole.Personal, 0.2f, EffectIncrementType.AddFactor, "{=rA9nzrAr}{VALUE}% siege engine build rate in your party.", PartyRole.Quartermaster, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardStiffUpperLip.Initialize("{=QUeJ4gc3}Stiff Upper Lip", DefaultSkills.Steward, DefaultPerks.GetTierCost(3), this._stewardSweatshops, "{=y9AsEMnV}{VALUE}% food consumption in your party while it is part of an army.", PartyRole.Quartermaster, -0.1f, EffectIncrementType.AddFactor, "{=1FPpHasQ}{VALUE}% garrison wages in the governed castle.", PartyRole.Governor, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._stewardPaidInPromise.Initialize("{=CPxbG7Zp}Paid in Promise", DefaultSkills.Steward, DefaultPerks.GetTierCost(4), this._stewardEfficientCampaigner, "{=H9tQfeBr}{VALUE}% companion wages and recruitment fees.", PartyRole.PartyLeader, -0.25f, EffectIncrementType.AddFactor, "{=1eKRHLur}Discarded armors are donated to troops for increased experience.", PartyRole.Quartermaster, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardEfficientCampaigner.Initialize("{=sC53NYcA}Efficient Campaigner", DefaultSkills.Steward, DefaultPerks.GetTierCost(4), this._stewardPaidInPromise, "{=5t6cveXT}{VALUE} extra food for each food taken during village raids for your party.", PartyRole.PartyLeader, 1f, EffectIncrementType.Add, "{=JhFCoWbE}{VALUE}% troop wages in your party while it is part of an army.", PartyRole.Quartermaster, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.NavalReduced);
			this._stewardGivingHands.Initialize("{=VsqyzWYY}Giving Hands", DefaultSkills.Steward, DefaultPerks.GetTierCost(5), this._stewardLogistician, "{=WaGKvsfc}Discarded weapons are donated to troops for increased experience.", PartyRole.Quartermaster, 0f, EffectIncrementType.AddFactor, "{=Eo958e7R}{VALUE}% tariff income in the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardLogistician.Initialize("{=U2buPiec}Logistician", DefaultSkills.Steward, DefaultPerks.GetTierCost(5), this._stewardGivingHands, "{=sG9WGOeN}{VALUE} party morale when number of mounts is greater than number of foot troops in your party.", PartyRole.Quartermaster, 4f, EffectIncrementType.Add, "{=Z1n0w5Kc}{VALUE}% tax income.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardRelocation.Initialize("{=R6dnhblo}Relocation", DefaultSkills.Steward, DefaultPerks.GetTierCost(6), this._stewardAidCorps, "{=urSSNtUD}{VALUE}% influence gain from donating troops.", PartyRole.Quartermaster, 0.25f, EffectIncrementType.AddFactor, "{=XmqJb7RN}{VALUE}% effect from boosting projects in the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardAidCorps.Initialize("{=4FdtVyj1}Aid Corps", DefaultSkills.Steward, DefaultPerks.GetTierCost(6), this._stewardRelocation, "{=ZLbCqt23}Wounded troops in your party are no longer paid wages.", PartyRole.Quartermaster, 0f, EffectIncrementType.AddFactor, "{=ULY7byYc}{VALUE}% hearth growth in villages bound to the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardGourmet.Initialize("{=63lHFDSG}Gourmet", DefaultSkills.Steward, DefaultPerks.GetTierCost(7), this._stewardSoundReserves, "{=KDtcsKUs}Double the morale bonus from having diverse food in your party.", PartyRole.Quartermaster, 1f, EffectIncrementType.AddFactor, "{=q2ZDAm2v}{VALUE}% garrison food consumption during sieges in the governed settlement.", PartyRole.Governor, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._stewardSoundReserves.Initialize("{=O5dgeoss}Sound Reserves", DefaultSkills.Steward, DefaultPerks.GetTierCost(7), this._stewardGourmet, "{=RkYL5eaP}{VALUE}% troop upgrade costs.", PartyRole.Quartermaster, -0.1f, EffectIncrementType.AddFactor, "{=P10E5o9l}{VALUE}% food consumption during sieges in your party.", PartyRole.Quartermaster, -0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardForcedLabor.Initialize("{=cWyqiNrf}Forced Labor", DefaultSkills.Steward, DefaultPerks.GetTierCost(8), this._stewardContractors, "{=HrOTTjgo}Prisoners in your party provide carry capacity as if they are standard troops.", PartyRole.Quartermaster, 0f, EffectIncrementType.AddFactor, "{=T9Viygs8}{VALUE}% construction speed per every 3 prisoners.", PartyRole.Governor, 0.01f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardContractors.Initialize("{=Pg5enC8c}Contractors", DefaultSkills.Steward, DefaultPerks.GetTierCost(8), this._stewardForcedLabor, "{=4220dQ4j}{VALUE}% wages and upgrade costs of the mercenary troops in your party.", PartyRole.Quartermaster, -0.25f, EffectIncrementType.AddFactor, "{=xiTD2qUv}{VALUE}% town project effects in the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardArenicosMules.Initialize("{=qBx8UbUt}Arenicos' Mules", DefaultSkills.Steward, DefaultPerks.GetTierCost(9), this._stewardArenicosHorses, "{=Yp4zv2ib}{VALUE}% carrying capacity for pack animals in your party.", PartyRole.Quartermaster, 0.2f, EffectIncrementType.AddFactor, "{=fswrp38u}{VALUE}% trade penalty for trading pack animals.", PartyRole.Quartermaster, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardArenicosHorses.Initialize("{=tbQ5bUzD}Arenicos' Horses", DefaultSkills.Steward, DefaultPerks.GetTierCost(9), this._stewardArenicosMules, "{=G9OTNRs4}{VALUE}% carrying capacity for troops in your party.", PartyRole.Quartermaster, 0.1f, EffectIncrementType.AddFactor, "{=xm4eEbQY}{VALUE}% trade penalty for trading mounts.", PartyRole.PartyLeader, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._stewardMasterOfPlanning.Initialize("{=n5aT1Y7s}Master of Planning", DefaultSkills.Steward, DefaultPerks.GetTierCost(10), this._stewardMasterOfWarcraft, "{=KMmAG5bk}{VALUE}% food consumption while your party is in a siege camp.", PartyRole.Quartermaster, -0.4f, EffectIncrementType.AddFactor, "{=P5OjioRl}{VALUE}% effectiveness to continuous projects in the governed settlement. ", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardMasterOfWarcraft.Initialize("{=MM0ARhGh}Master of Warcraft", DefaultSkills.Steward, DefaultPerks.GetTierCost(10), this._stewardMasterOfPlanning, "{=StzVsQ2P}{VALUE}% troop wages while your party is in a siege camp.", PartyRole.Quartermaster, -0.25f, EffectIncrementType.AddFactor, "{=ya7alenH}{VALUE}% food consumption of town population in the governed settlement.", PartyRole.Governor, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._stewardPriceOfLoyalty.Initialize("{=eVTnUmSB}Price of Loyalty", DefaultSkills.Steward, DefaultPerks.GetTierCost(11), null, "{=sYrG8rNy}{VALUE}% to food consumption, wages and combat related morale loss for each steward point above 250 in your party.", PartyRole.Quartermaster, -0.005f, EffectIncrementType.AddFactor, "{=lwp50FuF}{VALUE}% tax income for each skill point above 200 in the governed settlement", PartyRole.Governor, 0.005f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._medicineSelfMedication.Initialize("{=TLGvIdJB}Self Medication", DefaultSkills.Medicine, DefaultPerks.GetTierCost(1), this._medicinePreventiveMedicine, "{=bLAw2di4}{VALUE}% healing rate.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, "{=V53EYEXx}{VALUE}% combat movement speed.", PartyRole.Personal, 0.02f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.NavalReduced);
			this._medicinePreventiveMedicine.Initialize("{=wI393cla}Preventive Medicine", DefaultSkills.Medicine, DefaultPerks.GetTierCost(1), this._medicineSelfMedication, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 5f, EffectIncrementType.Add, "{=10cVZTTm}{VALUE}% recovery of lost hit points after each battle.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._medicineTriageTent.Initialize("{=EU4JjLqV}Triage Tent", DefaultSkills.Medicine, DefaultPerks.GetTierCost(2), this._medicineWalkItOff, "{=ZMPhsLdx}{VALUE}% healing rate when stationary on the campaign map.", PartyRole.Surgeon, 0.3f, EffectIncrementType.AddFactor, "{=Mn714dPH}{VALUE}% food consumption for besieged governed settlement.", PartyRole.Governor, -0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._medicineWalkItOff.Initialize("{=0pyLfrGZ}Walk It Off", DefaultSkills.Medicine, DefaultPerks.GetTierCost(2), this._medicineTriageTent, "{=NtCBRiLH}{VALUE}% healing rate when moving on the campaign map.", PartyRole.Surgeon, 0.15f, EffectIncrementType.AddFactor, "{=4YNqWPEu}{VALUE} hit points recovery after each offensive battle.", PartyRole.Personal, 10f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._medicineSledges.Initialize("{=TyB6y5bh}Sledges", DefaultSkills.Medicine, DefaultPerks.GetTierCost(3), this._medicineDoctorsOath, "{=bFOfZmwC}{VALUE}% party speed penalty from the wounded.", PartyRole.Surgeon, -0.5f, EffectIncrementType.AddFactor, "{=dfULyKsz}{VALUE} hit points to mounts in your party.", PartyRole.PartyLeader, 15f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._medicineDoctorsOath.Initialize("{=PAwDV08b}Doctor's Oath", DefaultSkills.Medicine, DefaultPerks.GetTierCost(3), this._medicineSledges, "{=jEAMXTa6}Your medicine skill partially applies to enemy casualties, increasing potential prisoners.", PartyRole.Surgeon, 0f, EffectIncrementType.AddFactor, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._medicineBestMedicine.Initialize("{=ei1JSeco}Best Medicine", DefaultSkills.Medicine, DefaultPerks.GetTierCost(4), this._medicineGoodLodging, "{=L3kTYA2p}{VALUE}% healing rate while party morale is above 60.", PartyRole.Surgeon, 0.15f, EffectIncrementType.AddFactor, "{=At6b9vHF}{VALUE} relationship per day with a random notable over age 40 when party is in a town.", PartyRole.Personal, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._medicineGoodLodging.Initialize("{=RXo3edjn}Good Lodging", DefaultSkills.Medicine, DefaultPerks.GetTierCost(4), this._medicineBestMedicine, "{=NjMR2ypH}{VALUE}% healing rate while resting in settlements.", PartyRole.Surgeon, 0.2f, EffectIncrementType.AddFactor, "{=ZH3U43xW}{VALUE} relationship per day with a random noble over age 40 when party is in a town.", PartyRole.Personal, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._medicineSiegeMedic.Initialize("{=ObwbbEqE}Siege Medic", DefaultSkills.Medicine, DefaultPerks.GetTierCost(5), this._medicineVeterinarian, "{=Gyy4rwnD}{VALUE}% chance of troops getting wounded instead of getting killed during siege bombardment.", PartyRole.Surgeon, 0.5f, EffectIncrementType.AddFactor, "{=Nxh6aX2E}{VALUE}% chance to recover from lethal wounds during siege bombardment.", PartyRole.Surgeon, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._medicineVeterinarian.Initialize("{=DNPbZZPQ}Veterinarian", DefaultSkills.Medicine, DefaultPerks.GetTierCost(5), this._medicineSiegeMedic, "{=PZb8JrMH}{VALUE}% daily chance to recover a lame horse.", PartyRole.Surgeon, 0.3f, EffectIncrementType.AddFactor, "{=GJRcFc0V}{VALUE}% chance to recover mounts of dead cavalry troops in battles.", PartyRole.Surgeon, 0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._medicinePristineStreets.Initialize("{=72tbUfrz}Pristine Streets", DefaultSkills.Medicine, DefaultPerks.GetTierCost(6), this._medicineBushDoctor, "{=JMMVcpA0}{VALUE} settlement prosperity every day in governed settlements.", PartyRole.Governor, 1f, EffectIncrementType.Add, "{=R9O0Y64L}{VALUE}% party healing rate while waiting in towns.", PartyRole.Surgeon, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._medicineBushDoctor.Initialize("{=HGrsb7k2}Bush Doctor", DefaultSkills.Medicine, DefaultPerks.GetTierCost(6), this._medicinePristineStreets, "{=ULY7byYc}{VALUE}% hearth growth in villages bound to the governed settlement.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, "{=UaKTuz1l}{VALUE}% party healing rate while waiting in villages.", PartyRole.Surgeon, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._medicinePerfectHealth.Initialize("{=cGuPMx4p}Perfect Health", DefaultSkills.Medicine, DefaultPerks.GetTierCost(7), this._medicineHealthAdvise, "{=1yqMERf2}{VALUE}% recovery rate for each type of food in party inventory.", PartyRole.Surgeon, 0.05f, EffectIncrementType.AddFactor, "{=QsMEML5E}{VALUE}% animal production rate in villages bound to the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._medicineHealthAdvise.Initialize("{=NxcvQlAk}Health Advice", DefaultSkills.Medicine, DefaultPerks.GetTierCost(7), this._medicinePerfectHealth, "{=uRvym4tq}Chance of recovery from death due to old age for every clan member.", PartyRole.ClanLeader, 0f, EffectIncrementType.AddFactor, "{=ioYR1Grc}Wounded troops do not decrease morale in battles.", PartyRole.Surgeon, 0f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._medicinePhysicianOfPeople.Initialize("{=5o6pSbCx}Physician of People", DefaultSkills.Medicine, DefaultPerks.GetTierCost(8), this._medicineCleanInfrastructure, "{=F7bbkYx4}{VALUE} loyalty per day in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, "{=bNsaUb42}{VALUE}% chance to recover from lethal wounds for tier 1 and 2 troops", PartyRole.Surgeon, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._medicineCleanInfrastructure.Initialize("{=CZ4y5NAf}Clean Infrastructure", DefaultSkills.Medicine, DefaultPerks.GetTierCost(8), this._medicinePhysicianOfPeople, "{=S9XsuYap}{VALUE} prosperity bonus from civilian projects in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, "{=dYyFWmGB}{VALUE}% recovery rate from raids in villages bound to the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._medicineCheatDeath.Initialize("{=cpg0oHZJ}Cheat Death", DefaultSkills.Medicine, DefaultPerks.GetTierCost(9), this._medicineFortitudeTonic, "{=n2xL3okw}Cheat death due to old age once.", PartyRole.Personal, 0f, EffectIncrementType.Add, "{=b1IKTI8t}{VALUE}% chance to die when you fall unconscious in battle.", PartyRole.Surgeon, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._medicineFortitudeTonic.Initialize("{=ib2SMG9b}Fortitude Tonic", DefaultSkills.Medicine, DefaultPerks.GetTierCost(9), this._medicineCheatDeath, "{=v9NohO6l}{VALUE} hit points to other heroes in your party.", PartyRole.PartyLeader, 10f, EffectIncrementType.Add, "{=Ti9auMiO}{VALUE} hit points.", PartyRole.Personal, 5f, EffectIncrementType.Add, TroopUsageFlags.Any, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._medicineHelpingHands.Initialize("{=KavZKNaa}Helping Hands", DefaultSkills.Medicine, DefaultPerks.GetTierCost(10), this._medicineBattleHardened, "{=6NOzUcGN}{VALUE}% troop recovery rate for every 10 troop in your party.", PartyRole.Surgeon, 0.02f, EffectIncrementType.AddFactor, "{=iHuzmdm2}{VALUE}% prosperity loss from starvation.", PartyRole.Governor, -0.5f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._medicineBattleHardened.Initialize("{=oSbRD72H}Battle Hardened", DefaultSkills.Medicine, DefaultPerks.GetTierCost(10), this._medicineHelpingHands, "{=qWpabhp6}{VALUE} experience to wounded units at the end of the battle.", PartyRole.Surgeon, 25f, EffectIncrementType.Add, "{=3tLU4AG7}{VALUE}% siege attrition loss in the governed settlement.", PartyRole.Governor, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.NavalReduced, PerkObject.EffectEnvironment.All);
			this._medicineMinisterOfHealth.Initialize("{=rtTjuJTc}Minister of Health", DefaultSkills.Medicine, DefaultPerks.GetTierCost(11), null, "{=cwFyqrfv}{VALUE} hit point to troops for every skill point above 250.", PartyRole.Personal, 1f, EffectIncrementType.Add, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Any, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._engineeringScaffolds.Initialize("{=ekavTnTp}Scaffolds", DefaultSkills.Engineering, DefaultPerks.GetTierCost(1), this._engineeringTorsionEngines, "{=2WC42D5D}{VALUE}% build speed to non-ranged siege engines.", PartyRole.Engineer, 0.1f, EffectIncrementType.AddFactor, "{=F1CJo2wX}{VALUE}% shield hitpoints.", PartyRole.Personal, 0.3f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ShieldUser, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.NavalReduced);
			this._engineeringTorsionEngines.Initialize("{=57TDG2Ta}Torsion Engines", DefaultSkills.Engineering, DefaultPerks.GetTierCost(1), this._engineeringScaffolds, "{=hv18SprX}{VALUE}% build speed to ranged siege engines.", PartyRole.Engineer, 0.1f, EffectIncrementType.AddFactor, "{=aA8T7AsY}{VALUE} damage to equipped crossbows.", PartyRole.Personal, 3f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.NavalReduced);
			this._engineeringSiegeWorks.Initialize("{=Nr1GPYSr}Siegeworks", DefaultSkills.Engineering, DefaultPerks.GetTierCost(2), this._engineeringDungeonArchitect, "{=oOZH3v9Y}{VALUE}% hit points to ranged siege engines.", PartyRole.Engineer, 0.1f, EffectIncrementType.AddFactor, "{=pIFOcikU}{VALUE} prebuilt catapult to the settlement when a siege starts in the governed settlement.", PartyRole.Governor, 1f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._engineeringDungeonArchitect.Initialize("{=aPbpBJq5}Dungeon Architect", DefaultSkills.Engineering, DefaultPerks.GetTierCost(2), this._engineeringSiegeWorks, "{=KK3DAGej}{VALUE}% chance of ranged siege engines getting hit while under bombardment.", PartyRole.Engineer, -0.25f, EffectIncrementType.AddFactor, "{=ako4Xbvk}{VALUE}% escape chance to prisoners in dungeons of governed settlements.", PartyRole.Governor, -0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringCarpenters.Initialize("{=YwhAlz5n}Carpenters", DefaultSkills.Engineering, DefaultPerks.GetTierCost(3), this._engineeringMilitaryPlanner, "{=cXCbpPqS}{VALUE}% hit points to rams and siege-towers.", PartyRole.Engineer, 0.33f, EffectIncrementType.AddFactor, "{=lVp2bwR9}{VALUE}% build speed for projects in the governed town.", PartyRole.Governor, 0.12f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringMilitaryPlanner.Initialize("{=mzDsT7lV}Military Planner", DefaultSkills.Engineering, DefaultPerks.GetTierCost(3), this._engineeringCarpenters, "{=zU6gKebE}{VALUE}% ammunition to ranged troops when besieging.", PartyRole.Engineer, 0.5f, EffectIncrementType.AddFactor, "{=xZqVL9wN}{VALUE}% build speed for projects in the governed castle.", PartyRole.Governor, 0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringWallBreaker.Initialize("{=0wlWgIeL}Wall Breaker", DefaultSkills.Engineering, DefaultPerks.GetTierCost(4), this._engineeringDreadfulSieger, "{=JBa4DO2u}{VALUE}% damage dealt to walls during siege bombardment.", PartyRole.Engineer, 0.25f, EffectIncrementType.AddFactor, "{=g3SKNcMV}{VALUE}% damage dealt to shields by troops in your formation.", PartyRole.Captain, 0.1f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringDreadfulSieger.Initialize("{=bIS4kqmf}Dreadful Besieger", DefaultSkills.Engineering, DefaultPerks.GetTierCost(4), this._engineeringWallBreaker, "{=zUzfRYzf}{VALUE}% accuracy to your siege engines during siege bombardments in the governed settlement.", PartyRole.Governor, 0.1f, EffectIncrementType.AddFactor, "{=cD8a5zbZ}{VALUE}% crossbow damage by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringSalvager.Initialize("{=AgJAfEEZ}Salvager", DefaultSkills.Engineering, DefaultPerks.GetTierCost(5), this._engineeringForeman, "{=mtb8vJ4o}{VALUE}% accuracy to ballistas during siege bombardment.", PartyRole.Engineer, 0.2f, EffectIncrementType.AddFactor, "{=qfjgKCty}{VALUE}% siege engine build speed increase for each militia.", PartyRole.Governor, 0.001f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringForeman.Initialize("{=3ML4EkWY}Foreman", DefaultSkills.Engineering, DefaultPerks.GetTierCost(5), this._engineeringSalvager, "{=M4IaRQJy}{VALUE}% mangonel and trebuchet accuracy during siege bombardment.", PartyRole.Engineer, 0.1f, EffectIncrementType.AddFactor, "{=ivrmsCFC}{VALUE} prosperity when a project is finished in the governed settlement.", PartyRole.Governor, 100f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringStonecutters.Initialize("{=auIRGa2V}Stonecutters", DefaultSkills.Engineering, DefaultPerks.GetTierCost(6), this._engineeringSiegeEngineer, "{=uohYIaSw}{VALUE}% build speed for fortifications, aqueducts and barrack projects in the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, "{=uakMSJY6}Fire versions of siege engines can be constructed.", PartyRole.Engineer, 0f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringSiegeEngineer.Initialize("{=pFGhJxyN}Siege Engineer", DefaultSkills.Engineering, DefaultPerks.GetTierCost(6), this._engineeringStonecutters, "{=cRfa2IaT}{VALUE}% hit points to defensive siege engines in the governed settlement.", PartyRole.Governor, 0.3f, EffectIncrementType.AddFactor, "{=uakMSJY6}Fire versions of siege engines can be constructed.", PartyRole.Engineer, 0f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringCampBuilding.Initialize("{=Lv2pbg8c}Camp Building", DefaultSkills.Engineering, DefaultPerks.GetTierCost(7), this._engineeringBattlements, "{=fDSyE0eE}{VALUE}% cohesion loss of armies when besieging.", PartyRole.ArmyCommander, -0.5f, EffectIncrementType.AddFactor, "{=0T7AKmVS}{VALUE}% casualty chance from siege bombardments.", PartyRole.Engineer, -0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringBattlements.Initialize("{=hHHEW1HN}Battlements", DefaultSkills.Engineering, DefaultPerks.GetTierCost(7), this._engineeringCampBuilding, "{=Ix98dg08}{VALUE} prebuilt ballista when you set up a siege camp.", PartyRole.Engineer, 1f, EffectIncrementType.Add, "{=hXqSlJM7}{VALUE} maximum food reserve limits in the governed settlement.", PartyRole.Governor, 100f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringEngineeringGuilds.Initialize("{=elKQc0O6}Engineering Guilds", DefaultSkills.Engineering, DefaultPerks.GetTierCost(8), this._engineeringApprenticeship, "{=KAozuVLa}{VALUE} recruitment slot when recruiting from artisan notables.", PartyRole.Engineer, 1f, EffectIncrementType.Add, "{=EIkzYco9}{VALUE}% wall hit points in the governed settlement.", PartyRole.Governor, 0.25f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.All);
			this._engineeringApprenticeship.Initialize("{=yzybG5rl}Apprenticeship", DefaultSkills.Engineering, DefaultPerks.GetTierCost(8), this._engineeringEngineeringGuilds, "{=3m2tQF9F}{VALUE} experience to troops when a siege engine is built.", PartyRole.Engineer, 5f, EffectIncrementType.Add, "{=AeTSNsRu}{VALUE}% prosperity gain for each unique project in the governed settlement.", PartyRole.Governor, 0.01f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringMetallurgy.Initialize("{=qjvDsu8u}Metallurgy", DefaultSkills.Engineering, DefaultPerks.GetTierCost(9), this._engineeringImprovedTools, "{=ZMVo5TTq}{VALUE}% chance to remove negative modifiers on looted items.", PartyRole.Engineer, 0.3f, EffectIncrementType.AddFactor, "{=XWPcZgM9}{VALUE} armor to all equipped armor pieces of troops in your formation.", PartyRole.Captain, 5f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Any, PerkObject.EffectEnvironment.All, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringImprovedTools.Initialize("{=XixNAaD5}Improved Tools", DefaultSkills.Engineering, DefaultPerks.GetTierCost(9), this._engineeringMetallurgy, "{=5ATpHJag}{VALUE}% siege camp preparation speed.", PartyRole.Engineer, 0.2f, EffectIncrementType.AddFactor, "{=eBmaa49a}{VALUE}% melee damage by troops in your formation.", PartyRole.Captain, 0.05f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Melee, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.LandOnly);
			this._engineeringClockwork.Initialize("{=Z9Rey6LC}Clockwork", DefaultSkills.Engineering, DefaultPerks.GetTierCost(10), this._engineeringArchitecturalCommisions, "{=yn9GhVK4}{VALUE}% reload speed to ballistas during siege bombardment.", PartyRole.Engineer, 0.25f, EffectIncrementType.AddFactor, "{=Jlmtufb3}{VALUE}% effect from boosting projects in the governed town.", PartyRole.Governor, 0.2f, EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringArchitecturalCommisions.Initialize("{=KODafKT7}Architectural Commissions", DefaultSkills.Engineering, DefaultPerks.GetTierCost(10), this._engineeringClockwork, "{=0aMHHQL4}{VALUE}% reload speed to mangonels and trebuchets in siege bombardment.", PartyRole.Engineer, 0.25f, EffectIncrementType.AddFactor, "{=e3ykBSpR}{VALUE} gold per day for continuous projects in the governed settlement.", PartyRole.Governor, 20f, EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
			this._engineeringMasterwork.Initialize("{=SNsAlN4R}Masterwork", DefaultSkills.Engineering, DefaultPerks.GetTierCost(11), null, "{=RP2Jn3J4}{VALUE}% damage for each engineering skill point over 250 for siege engines in siege bombardment.", PartyRole.Engineer, 0.01f, EffectIncrementType.AddFactor, "", PartyRole.None, 0f, EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined, PerkObject.EffectEnvironment.LandOnly, PerkObject.EffectEnvironment.All);
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x000E869A File Offset: 0x000E689A
		private static int GetTierCost(int tierIndex)
		{
			return DefaultPerks.TierSkillRequirements[tierIndex - 1];
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x000E86A5 File Offset: 0x000E68A5
		private static PerkObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<PerkObject>(new PerkObject(stringId));
		}

		// Token: 0x04000FBC RID: 4028
		private static readonly int[] TierSkillRequirements = new int[]
		{
			25, 50, 75, 100, 125, 150, 175, 200, 225, 250,
			275, 300
		};

		// Token: 0x04000FBD RID: 4029
		private PerkObject _oneHandedBasher;

		// Token: 0x04000FBE RID: 4030
		private PerkObject _oneHandedToBeBlunt;

		// Token: 0x04000FBF RID: 4031
		private PerkObject _oneHandedSteelCoreShields;

		// Token: 0x04000FC0 RID: 4032
		private PerkObject _oneHandedFleetOfFoot;

		// Token: 0x04000FC1 RID: 4033
		private PerkObject _oneHandedDeadlyPurpose;

		// Token: 0x04000FC2 RID: 4034
		private PerkObject _oneHandedUnwaveringDefense;

		// Token: 0x04000FC3 RID: 4035
		private PerkObject _oneHandedWrappedHandles;

		// Token: 0x04000FC4 RID: 4036
		private PerkObject _oneHandedWayOfTheSword;

		// Token: 0x04000FC5 RID: 4037
		private PerkObject _oneHandedPrestige;

		// Token: 0x04000FC6 RID: 4038
		private PerkObject _oneHandedChinkInTheArmor;

		// Token: 0x04000FC7 RID: 4039
		private PerkObject _oneHandedStandUnited;

		// Token: 0x04000FC8 RID: 4040
		private PerkObject _oneHandedLeadByExample;

		// Token: 0x04000FC9 RID: 4041
		private PerkObject _oneHandedMilitaryTradition;

		// Token: 0x04000FCA RID: 4042
		private PerkObject _oneHandedCorpsACorps;

		// Token: 0x04000FCB RID: 4043
		private PerkObject _oneHandedShieldWall;

		// Token: 0x04000FCC RID: 4044
		private PerkObject _oneHandedArrowCatcher;

		// Token: 0x04000FCD RID: 4045
		private PerkObject _oneHandedShieldBearer;

		// Token: 0x04000FCE RID: 4046
		private PerkObject _oneHandedTrainer;

		// Token: 0x04000FCF RID: 4047
		private PerkObject _oneHandedDuelist;

		// Token: 0x04000FD0 RID: 4048
		private PerkObject _oneHandedSwiftStrike;

		// Token: 0x04000FD1 RID: 4049
		private PerkObject _oneHandedCavalry;

		// Token: 0x04000FD2 RID: 4050
		private PerkObject _twoHandedWoodChopper;

		// Token: 0x04000FD3 RID: 4051
		private PerkObject _twoHandedWayOfTheGreatAxe;

		// Token: 0x04000FD4 RID: 4052
		private PerkObject _twoHandedStrongGrip;

		// Token: 0x04000FD5 RID: 4053
		private PerkObject _twoHandedOnTheEdge;

		// Token: 0x04000FD6 RID: 4054
		private PerkObject _twoHandedHeadBasher;

		// Token: 0x04000FD7 RID: 4055
		private PerkObject _twoHandedShowOfStrength;

		// Token: 0x04000FD8 RID: 4056
		private PerkObject _twoHandedBeastSlayer;

		// Token: 0x04000FD9 RID: 4057
		private PerkObject _twoHandedBaptisedInBlood;

		// Token: 0x04000FDA RID: 4058
		private PerkObject _twoHandedShieldBreaker;

		// Token: 0x04000FDB RID: 4059
		private PerkObject _twoHandedConfidence;

		// Token: 0x04000FDC RID: 4060
		private PerkObject _twoHandedBerserker;

		// Token: 0x04000FDD RID: 4061
		private PerkObject _twoHandedProjectileDeflection;

		// Token: 0x04000FDE RID: 4062
		private PerkObject _twoHandedTerror;

		// Token: 0x04000FDF RID: 4063
		private PerkObject _twoHandedHope;

		// Token: 0x04000FE0 RID: 4064
		private PerkObject _twoHandedThickHides;

		// Token: 0x04000FE1 RID: 4065
		private PerkObject _twoHandedRecklessCharge;

		// Token: 0x04000FE2 RID: 4066
		private PerkObject _twoHandedBladeMaster;

		// Token: 0x04000FE3 RID: 4067
		private PerkObject _twoHandedVandal;

		// Token: 0x04000FE4 RID: 4068
		private PerkObject _polearmPikeman;

		// Token: 0x04000FE5 RID: 4069
		private PerkObject _polearmCavalry;

		// Token: 0x04000FE6 RID: 4070
		private PerkObject _polearmBraced;

		// Token: 0x04000FE7 RID: 4071
		private PerkObject _polearmKeepAtBay;

		// Token: 0x04000FE8 RID: 4072
		private PerkObject _polearmSwiftSwing;

		// Token: 0x04000FE9 RID: 4073
		private PerkObject _polearmCleanThrust;

		// Token: 0x04000FEA RID: 4074
		private PerkObject _polearmFootwork;

		// Token: 0x04000FEB RID: 4075
		private PerkObject _polearmHardKnock;

		// Token: 0x04000FEC RID: 4076
		private PerkObject _polearmSteedKiller;

		// Token: 0x04000FED RID: 4077
		private PerkObject _polearmLancer;

		// Token: 0x04000FEE RID: 4078
		private PerkObject _polearmGuards;

		// Token: 0x04000FEF RID: 4079
		private PerkObject _polearmSkewer;

		// Token: 0x04000FF0 RID: 4080
		private PerkObject _polearmStandardBearer;

		// Token: 0x04000FF1 RID: 4081
		private PerkObject _polearmPhalanx;

		// Token: 0x04000FF2 RID: 4082
		private PerkObject _polearmHardyFrontline;

		// Token: 0x04000FF3 RID: 4083
		private PerkObject _polearmDrills;

		// Token: 0x04000FF4 RID: 4084
		private PerkObject _polearmSureFooted;

		// Token: 0x04000FF5 RID: 4085
		private PerkObject _polearmUnstoppableForce;

		// Token: 0x04000FF6 RID: 4086
		private PerkObject _polearmCounterweight;

		// Token: 0x04000FF7 RID: 4087
		private PerkObject _polearmWayOfTheSpear;

		// Token: 0x04000FF8 RID: 4088
		private PerkObject _polearmSharpenTheTip;

		// Token: 0x04000FF9 RID: 4089
		private PerkObject _bowDeadAim;

		// Token: 0x04000FFA RID: 4090
		private PerkObject _bowBodkin;

		// Token: 0x04000FFB RID: 4091
		private PerkObject _bowRangersSwiftness;

		// Token: 0x04000FFC RID: 4092
		private PerkObject _bowRapidFire;

		// Token: 0x04000FFD RID: 4093
		private PerkObject _bowQuickAdjustments;

		// Token: 0x04000FFE RID: 4094
		private PerkObject _bowMerryMen;

		// Token: 0x04000FFF RID: 4095
		private PerkObject _bowMountedArchery;

		// Token: 0x04001000 RID: 4096
		private PerkObject _bowTrainer;

		// Token: 0x04001001 RID: 4097
		private PerkObject _bowStrongBows;

		// Token: 0x04001002 RID: 4098
		private PerkObject _bowDiscipline;

		// Token: 0x04001003 RID: 4099
		private PerkObject _bowHunterClan;

		// Token: 0x04001004 RID: 4100
		private PerkObject _bowSkirmishPhaseMaster;

		// Token: 0x04001005 RID: 4101
		private PerkObject _bowEagleEye;

		// Token: 0x04001006 RID: 4102
		private PerkObject _bowBullsEye;

		// Token: 0x04001007 RID: 4103
		private PerkObject _bowRenownedArcher;

		// Token: 0x04001008 RID: 4104
		private PerkObject _bowHorseMaster;

		// Token: 0x04001009 RID: 4105
		private PerkObject _bowDeepQuivers;

		// Token: 0x0400100A RID: 4106
		private PerkObject _bowQuickDraw;

		// Token: 0x0400100B RID: 4107
		private PerkObject _bowNockingPoint;

		// Token: 0x0400100C RID: 4108
		private PerkObject _bowBowControl;

		// Token: 0x0400100D RID: 4109
		private PerkObject _bowDeadshot;

		// Token: 0x0400100E RID: 4110
		private PerkObject _crossbowMarksmen;

		// Token: 0x0400100F RID: 4111
		private PerkObject _crossbowUnhorser;

		// Token: 0x04001010 RID: 4112
		private PerkObject _crossbowWindWinder;

		// Token: 0x04001011 RID: 4113
		private PerkObject _crossbowDonkeysSwiftness;

		// Token: 0x04001012 RID: 4114
		private PerkObject _crossbowSheriff;

		// Token: 0x04001013 RID: 4115
		private PerkObject _crossbowPeasantLeader;

		// Token: 0x04001014 RID: 4116
		private PerkObject _crossbowRenownMarksmen;

		// Token: 0x04001015 RID: 4117
		private PerkObject _crossbowFletcher;

		// Token: 0x04001016 RID: 4118
		private PerkObject _crossbowPuncture;

		// Token: 0x04001017 RID: 4119
		private PerkObject _crossbowLooseAndMove;

		// Token: 0x04001018 RID: 4120
		private PerkObject _crossbowDeftHands;

		// Token: 0x04001019 RID: 4121
		private PerkObject _crossbowCounterFire;

		// Token: 0x0400101A RID: 4122
		private PerkObject _crossbowMountedCrossbowman;

		// Token: 0x0400101B RID: 4123
		private PerkObject _crossbowSteady;

		// Token: 0x0400101C RID: 4124
		private PerkObject _crossbowLongShots;

		// Token: 0x0400101D RID: 4125
		private PerkObject _crossbowHammerBolts;

		// Token: 0x0400101E RID: 4126
		private PerkObject _crossbowPavise;

		// Token: 0x0400101F RID: 4127
		private PerkObject _crossbowTerror;

		// Token: 0x04001020 RID: 4128
		private PerkObject _crossbowPickedShots;

		// Token: 0x04001021 RID: 4129
		private PerkObject _crossbowPiercer;

		// Token: 0x04001022 RID: 4130
		private PerkObject _crossbowMightyPull;

		// Token: 0x04001023 RID: 4131
		private PerkObject _throwingShieldBreaker;

		// Token: 0x04001024 RID: 4132
		private PerkObject _throwingHunter;

		// Token: 0x04001025 RID: 4133
		private PerkObject _throwingFlexibleFighter;

		// Token: 0x04001026 RID: 4134
		private PerkObject _throwingMountedSkirmisher;

		// Token: 0x04001027 RID: 4135
		private PerkObject _throwingPerfectTechnique;

		// Token: 0x04001028 RID: 4136
		private PerkObject _throwingRunningThrow;

		// Token: 0x04001029 RID: 4137
		private PerkObject _throwingKnockOff;

		// Token: 0x0400102A RID: 4138
		private PerkObject _throwingWellPrepared;

		// Token: 0x0400102B RID: 4139
		private PerkObject _throwingSkirmisher;

		// Token: 0x0400102C RID: 4140
		private PerkObject _throwingFocus;

		// Token: 0x0400102D RID: 4141
		private PerkObject _throwingLastHit;

		// Token: 0x0400102E RID: 4142
		private PerkObject _throwingHeadHunter;

		// Token: 0x0400102F RID: 4143
		private PerkObject _throwingSlingingCompetitions;

		// Token: 0x04001030 RID: 4144
		private PerkObject _throwingSaddlebags;

		// Token: 0x04001031 RID: 4145
		private PerkObject _throwingSplinters;

		// Token: 0x04001032 RID: 4146
		private PerkObject _throwingResourceful;

		// Token: 0x04001033 RID: 4147
		private PerkObject _throwingLongReach;

		// Token: 0x04001034 RID: 4148
		private PerkObject _throwingWeakSpot;

		// Token: 0x04001035 RID: 4149
		private PerkObject _throwingQuickDraw;

		// Token: 0x04001036 RID: 4150
		private PerkObject _throwingImpale;

		// Token: 0x04001037 RID: 4151
		private PerkObject _throwingUnstoppableForce;

		// Token: 0x04001038 RID: 4152
		private PerkObject _ridingNimbleSteed;

		// Token: 0x04001039 RID: 4153
		private PerkObject _ridingWellStraped;

		// Token: 0x0400103A RID: 4154
		private PerkObject _ridingVeterinary;

		// Token: 0x0400103B RID: 4155
		private PerkObject _ridingNomadicTraditions;

		// Token: 0x0400103C RID: 4156
		private PerkObject _ridingDeeperSacks;

		// Token: 0x0400103D RID: 4157
		private PerkObject _ridingSagittarius;

		// Token: 0x0400103E RID: 4158
		private PerkObject _ridingSweepingWind;

		// Token: 0x0400103F RID: 4159
		private PerkObject _ridingReliefForce;

		// Token: 0x04001040 RID: 4160
		private PerkObject _ridingMountedWarrior;

		// Token: 0x04001041 RID: 4161
		private PerkObject _ridingHorseArcher;

		// Token: 0x04001042 RID: 4162
		private PerkObject _ridingShepherd;

		// Token: 0x04001043 RID: 4163
		private PerkObject _ridingBreeder;

		// Token: 0x04001044 RID: 4164
		private PerkObject _ridingThunderousCharge;

		// Token: 0x04001045 RID: 4165
		private PerkObject _ridingAnnoyingBuzz;

		// Token: 0x04001046 RID: 4166
		private PerkObject _ridingMountedPatrols;

		// Token: 0x04001047 RID: 4167
		private PerkObject _ridingCavalryTactics;

		// Token: 0x04001048 RID: 4168
		private PerkObject _ridingDauntlessSteed;

		// Token: 0x04001049 RID: 4169
		private PerkObject _ridingToughSteed;

		// Token: 0x0400104A RID: 4170
		private PerkObject _ridingFullSpeed;

		// Token: 0x0400104B RID: 4171
		private PerkObject _ridingTheWayOfTheSaddle;

		// Token: 0x0400104C RID: 4172
		private PerkObject _athleticsFormFittingArmor;

		// Token: 0x0400104D RID: 4173
		private PerkObject _athleticsImposingStature;

		// Token: 0x0400104E RID: 4174
		private PerkObject _athleticsStamina;

		// Token: 0x0400104F RID: 4175
		private PerkObject _athleticsSprint;

		// Token: 0x04001050 RID: 4176
		private PerkObject _athleticsPowerful;

		// Token: 0x04001051 RID: 4177
		private PerkObject _athleticsSurgingBlow;

		// Token: 0x04001052 RID: 4178
		private PerkObject _athleticsWellBuilt;

		// Token: 0x04001053 RID: 4179
		private PerkObject _athleticsFury;

		// Token: 0x04001054 RID: 4180
		private PerkObject _athleticsBraced;

		// Token: 0x04001055 RID: 4181
		private PerkObject _athleticsAGoodDaysRest;

		// Token: 0x04001056 RID: 4182
		private PerkObject _athleticsDurable;

		// Token: 0x04001057 RID: 4183
		private PerkObject _athleticsEnergetic;

		// Token: 0x04001058 RID: 4184
		private PerkObject _athleticsSteady;

		// Token: 0x04001059 RID: 4185
		private PerkObject _athleticsStrong;

		// Token: 0x0400105A RID: 4186
		private PerkObject _athleticsStrongLegs;

		// Token: 0x0400105B RID: 4187
		private PerkObject _athleticsStrongArms;

		// Token: 0x0400105C RID: 4188
		private PerkObject _athleticsSpartan;

		// Token: 0x0400105D RID: 4189
		private PerkObject _athleticsMorningExercise;

		// Token: 0x0400105E RID: 4190
		private PerkObject _athleticsIgnorePain;

		// Token: 0x0400105F RID: 4191
		private PerkObject _athleticsWalkItOff;

		// Token: 0x04001060 RID: 4192
		private PerkObject _athleticsMightyBlow;

		// Token: 0x04001061 RID: 4193
		private PerkObject _craftingSteelMaker2;

		// Token: 0x04001062 RID: 4194
		private PerkObject _craftingSteelMaker3;

		// Token: 0x04001063 RID: 4195
		private PerkObject _craftingCharcoalMaker;

		// Token: 0x04001064 RID: 4196
		private PerkObject _craftingSteelMaker;

		// Token: 0x04001065 RID: 4197
		private PerkObject _craftingCuriousSmelter;

		// Token: 0x04001066 RID: 4198
		private PerkObject _craftingCuriousSmith;

		// Token: 0x04001067 RID: 4199
		private PerkObject _craftingPracticalSmelter;

		// Token: 0x04001068 RID: 4200
		private PerkObject _craftingPracticalRefiner;

		// Token: 0x04001069 RID: 4201
		private PerkObject _craftingPracticalSmith;

		// Token: 0x0400106A RID: 4202
		private PerkObject _craftingArtisanSmith;

		// Token: 0x0400106B RID: 4203
		private PerkObject _craftingExperiencedSmith;

		// Token: 0x0400106C RID: 4204
		private PerkObject _craftingMasterSmith;

		// Token: 0x0400106D RID: 4205
		private PerkObject _craftingLegendarySmith;

		// Token: 0x0400106E RID: 4206
		private PerkObject _craftingVigorousSmith;

		// Token: 0x0400106F RID: 4207
		private PerkObject _craftingStrongSmith;

		// Token: 0x04001070 RID: 4208
		private PerkObject _craftingEnduringSmith;

		// Token: 0x04001071 RID: 4209
		private PerkObject _craftingIronMaker;

		// Token: 0x04001072 RID: 4210
		private PerkObject _craftingFencerSmith;

		// Token: 0x04001073 RID: 4211
		private PerkObject _craftingSharpenedEdge;

		// Token: 0x04001074 RID: 4212
		private PerkObject _craftingSharpenedTip;

		// Token: 0x04001075 RID: 4213
		private PerkObject _tacticsSmallUnitTactics;

		// Token: 0x04001076 RID: 4214
		private PerkObject _tacticsHordeLeader;

		// Token: 0x04001077 RID: 4215
		private PerkObject _tacticsLawKeeper;

		// Token: 0x04001078 RID: 4216
		private PerkObject _tacticsLooseFormations;

		// Token: 0x04001079 RID: 4217
		private PerkObject _tacticsSwiftRegroup;

		// Token: 0x0400107A RID: 4218
		private PerkObject _tacticsExtendedSkirmish;

		// Token: 0x0400107B RID: 4219
		private PerkObject _tacticsDecisiveBattle;

		// Token: 0x0400107C RID: 4220
		private PerkObject _tacticsCoaching;

		// Token: 0x0400107D RID: 4221
		private PerkObject _tacticsImproviser;

		// Token: 0x0400107E RID: 4222
		private PerkObject _tacticsOnTheMarch;

		// Token: 0x0400107F RID: 4223
		private PerkObject _tacticsCallToArms;

		// Token: 0x04001080 RID: 4224
		private PerkObject _tacticsPickThemOfTheWalls;

		// Token: 0x04001081 RID: 4225
		private PerkObject _tacticsMakeThemPay;

		// Token: 0x04001082 RID: 4226
		private PerkObject _tacticsEliteReserves;

		// Token: 0x04001083 RID: 4227
		private PerkObject _tacticsEncirclement;

		// Token: 0x04001084 RID: 4228
		private PerkObject _tacticsPreBattleManeuvers;

		// Token: 0x04001085 RID: 4229
		private PerkObject _tacticsBesieged;

		// Token: 0x04001086 RID: 4230
		private PerkObject _tacticsCounteroffensive;

		// Token: 0x04001087 RID: 4231
		private PerkObject _tacticsGensdarmes;

		// Token: 0x04001088 RID: 4232
		private PerkObject _tacticsTightFormations;

		// Token: 0x04001089 RID: 4233
		private PerkObject _tacticsTacticalMastery;

		// Token: 0x0400108A RID: 4234
		private PerkObject _scoutingNightRunner;

		// Token: 0x0400108B RID: 4235
		private PerkObject _scoutingWaterDiviner;

		// Token: 0x0400108C RID: 4236
		private PerkObject _scoutingForestKin;

		// Token: 0x0400108D RID: 4237
		private PerkObject _scoutingForcedMarch;

		// Token: 0x0400108E RID: 4238
		private PerkObject _scoutingDesertBorn;

		// Token: 0x0400108F RID: 4239
		private PerkObject _scoutingPathfinder;

		// Token: 0x04001090 RID: 4240
		private PerkObject _scoutingUnburdened;

		// Token: 0x04001091 RID: 4241
		private PerkObject _scoutingTracker;

		// Token: 0x04001092 RID: 4242
		private PerkObject _scoutingRanger;

		// Token: 0x04001093 RID: 4243
		private PerkObject _scoutingMountedScouts;

		// Token: 0x04001094 RID: 4244
		private PerkObject _scoutingPatrols;

		// Token: 0x04001095 RID: 4245
		private PerkObject _scoutingForagers;

		// Token: 0x04001096 RID: 4246
		private PerkObject _scoutingBeastWhisperer;

		// Token: 0x04001097 RID: 4247
		private PerkObject _scoutingVillageNetwork;

		// Token: 0x04001098 RID: 4248
		private PerkObject _scoutingRumourNetwork;

		// Token: 0x04001099 RID: 4249
		private PerkObject _scoutingVantagePoint;

		// Token: 0x0400109A RID: 4250
		private PerkObject _scoutingKeenSight;

		// Token: 0x0400109B RID: 4251
		private PerkObject _scoutingVanguard;

		// Token: 0x0400109C RID: 4252
		private PerkObject _scoutingRearguard;

		// Token: 0x0400109D RID: 4253
		private PerkObject _scoutingDayTraveler;

		// Token: 0x0400109E RID: 4254
		private PerkObject _scoutingUncannyInsight;

		// Token: 0x0400109F RID: 4255
		private PerkObject _rogueryTwoFaced;

		// Token: 0x040010A0 RID: 4256
		private PerkObject _rogueryDeepPockets;

		// Token: 0x040010A1 RID: 4257
		private PerkObject _rogueryInBestLight;

		// Token: 0x040010A2 RID: 4258
		private PerkObject _roguerySweetTalker;

		// Token: 0x040010A3 RID: 4259
		private PerkObject _rogueryKnowHow;

		// Token: 0x040010A4 RID: 4260
		private PerkObject _rogueryManhunter;

		// Token: 0x040010A5 RID: 4261
		private PerkObject _rogueryPromises;

		// Token: 0x040010A6 RID: 4262
		private PerkObject _rogueryScarface;

		// Token: 0x040010A7 RID: 4263
		private PerkObject _rogueryWhiteLies;

		// Token: 0x040010A8 RID: 4264
		private PerkObject _roguerySmugglerConnections;

		// Token: 0x040010A9 RID: 4265
		private PerkObject _rogueryPartnersInCrime;

		// Token: 0x040010AA RID: 4266
		private PerkObject _rogueryOneOfTheFamily;

		// Token: 0x040010AB RID: 4267
		private PerkObject _roguerySaltTheEarth;

		// Token: 0x040010AC RID: 4268
		private PerkObject _rogueryCarver;

		// Token: 0x040010AD RID: 4269
		private PerkObject _rogueryRansomBroker;

		// Token: 0x040010AE RID: 4270
		private PerkObject _rogueryArmsDealer;

		// Token: 0x040010AF RID: 4271
		private PerkObject _rogueryDirtyFighting;

		// Token: 0x040010B0 RID: 4272
		private PerkObject _rogueryDashAndSlash;

		// Token: 0x040010B1 RID: 4273
		private PerkObject _rogueryFleetFooted;

		// Token: 0x040010B2 RID: 4274
		private PerkObject _rogueryNoRestForTheWicked;

		// Token: 0x040010B3 RID: 4275
		private PerkObject _rogueryRogueExtraordinaire;

		// Token: 0x040010B4 RID: 4276
		private PerkObject _leadershipFerventAttacker;

		// Token: 0x040010B5 RID: 4277
		private PerkObject _leadershipStoutDefender;

		// Token: 0x040010B6 RID: 4278
		private PerkObject _leadershipAuthority;

		// Token: 0x040010B7 RID: 4279
		private PerkObject _leadershipHeroicLeader;

		// Token: 0x040010B8 RID: 4280
		private PerkObject _leadershipLoyaltyAndHonor;

		// Token: 0x040010B9 RID: 4281
		private PerkObject _leadershipFamousCommander;

		// Token: 0x040010BA RID: 4282
		private PerkObject _leadershipRaiseTheMeek;

		// Token: 0x040010BB RID: 4283
		private PerkObject _leadershipPresence;

		// Token: 0x040010BC RID: 4284
		private PerkObject _leadershipVeteransRespect;

		// Token: 0x040010BD RID: 4285
		private PerkObject _leadershipLeaderOfTheMasses;

		// Token: 0x040010BE RID: 4286
		private PerkObject _leadershipInspiringLeader;

		// Token: 0x040010BF RID: 4287
		private PerkObject _leadershipUpliftingSpirit;

		// Token: 0x040010C0 RID: 4288
		private PerkObject _leadershipMakeADifference;

		// Token: 0x040010C1 RID: 4289
		private PerkObject _leadershipLeadByExample;

		// Token: 0x040010C2 RID: 4290
		private PerkObject _leadershipTrustedCommander;

		// Token: 0x040010C3 RID: 4291
		private PerkObject _leadershipGreatLeader;

		// Token: 0x040010C4 RID: 4292
		private PerkObject _leadershipWePledgeOurSwords;

		// Token: 0x040010C5 RID: 4293
		private PerkObject _leadershipUltimateLeader;

		// Token: 0x040010C6 RID: 4294
		private PerkObject _leadershipTalentMagnet;

		// Token: 0x040010C7 RID: 4295
		private PerkObject _leadershipCitizenMilitia;

		// Token: 0x040010C8 RID: 4296
		private PerkObject _leadershipCombatTips;

		// Token: 0x040010C9 RID: 4297
		private PerkObject _charmVirile;

		// Token: 0x040010CA RID: 4298
		private PerkObject _charmSelfPromoter;

		// Token: 0x040010CB RID: 4299
		private PerkObject _charmOratory;

		// Token: 0x040010CC RID: 4300
		private PerkObject _charmWarlord;

		// Token: 0x040010CD RID: 4301
		private PerkObject _charmForgivableGrievances;

		// Token: 0x040010CE RID: 4302
		private PerkObject _charmMeaningfulFavors;

		// Token: 0x040010CF RID: 4303
		private PerkObject _charmInBloom;

		// Token: 0x040010D0 RID: 4304
		private PerkObject _charmYoungAndRespectful;

		// Token: 0x040010D1 RID: 4305
		private PerkObject _charmFirebrand;

		// Token: 0x040010D2 RID: 4306
		private PerkObject _charmFlexibleEthics;

		// Token: 0x040010D3 RID: 4307
		private PerkObject _charmEffortForThePeople;

		// Token: 0x040010D4 RID: 4308
		private PerkObject _charmSlickNegotiator;

		// Token: 0x040010D5 RID: 4309
		private PerkObject _charmGoodNatured;

		// Token: 0x040010D6 RID: 4310
		private PerkObject _charmTribute;

		// Token: 0x040010D7 RID: 4311
		private PerkObject _charmMoralLeader;

		// Token: 0x040010D8 RID: 4312
		private PerkObject _charmNaturalLeader;

		// Token: 0x040010D9 RID: 4313
		private PerkObject _charmPublicSpeaker;

		// Token: 0x040010DA RID: 4314
		private PerkObject _charmParade;

		// Token: 0x040010DB RID: 4315
		private PerkObject _charmCamaraderie;

		// Token: 0x040010DC RID: 4316
		private PerkObject _charmImmortalCharm;

		// Token: 0x040010DD RID: 4317
		private PerkObject _tradeTravelingRumors;

		// Token: 0x040010DE RID: 4318
		private PerkObject _tradeLocalConnection;

		// Token: 0x040010DF RID: 4319
		private PerkObject _tradeDistributedGoods;

		// Token: 0x040010E0 RID: 4320
		private PerkObject _tradeTollgates;

		// Token: 0x040010E1 RID: 4321
		private PerkObject _tradeArtisanCommunity;

		// Token: 0x040010E2 RID: 4322
		private PerkObject _tradeGreatInvestor;

		// Token: 0x040010E3 RID: 4323
		private PerkObject _tradeMercenaryConnections;

		// Token: 0x040010E4 RID: 4324
		private PerkObject _tradeContentTrades;

		// Token: 0x040010E5 RID: 4325
		private PerkObject _tradeInsurancePlans;

		// Token: 0x040010E6 RID: 4326
		private PerkObject _tradeRapidDevelopment;

		// Token: 0x040010E7 RID: 4327
		private PerkObject _tradeGranaryAccountant;

		// Token: 0x040010E8 RID: 4328
		private PerkObject _tradeTradeyardForeman;

		// Token: 0x040010E9 RID: 4329
		private PerkObject _tradeWholeSeller;

		// Token: 0x040010EA RID: 4330
		private PerkObject _tradeCaravanMaster;

		// Token: 0x040010EB RID: 4331
		private PerkObject _tradeMarketDealer;

		// Token: 0x040010EC RID: 4332
		private PerkObject _tradeSwordForBarter;

		// Token: 0x040010ED RID: 4333
		private PerkObject _tradeTrickleDown;

		// Token: 0x040010EE RID: 4334
		private PerkObject _tradeManOfMeans;

		// Token: 0x040010EF RID: 4335
		private PerkObject _tradeSpringOfGold;

		// Token: 0x040010F0 RID: 4336
		private PerkObject _tradeSilverTongue;

		// Token: 0x040010F1 RID: 4337
		private PerkObject _tradeSelfMadeMan;

		// Token: 0x040010F2 RID: 4338
		private PerkObject _tradeAppraiser;

		// Token: 0x040010F3 RID: 4339
		private PerkObject _tradeEverythingHasAPrice;

		// Token: 0x040010F4 RID: 4340
		private PerkObject _medicinePreventiveMedicine;

		// Token: 0x040010F5 RID: 4341
		private PerkObject _medicineTriageTent;

		// Token: 0x040010F6 RID: 4342
		private PerkObject _medicineWalkItOff;

		// Token: 0x040010F7 RID: 4343
		private PerkObject _medicineSledges;

		// Token: 0x040010F8 RID: 4344
		private PerkObject _medicineDoctorsOath;

		// Token: 0x040010F9 RID: 4345
		private PerkObject _medicineBestMedicine;

		// Token: 0x040010FA RID: 4346
		private PerkObject _medicineGoodLodging;

		// Token: 0x040010FB RID: 4347
		private PerkObject _medicineSiegeMedic;

		// Token: 0x040010FC RID: 4348
		private PerkObject _medicineVeterinarian;

		// Token: 0x040010FD RID: 4349
		private PerkObject _medicinePristineStreets;

		// Token: 0x040010FE RID: 4350
		private PerkObject _medicineBushDoctor;

		// Token: 0x040010FF RID: 4351
		private PerkObject _medicinePerfectHealth;

		// Token: 0x04001100 RID: 4352
		private PerkObject _medicineHealthAdvise;

		// Token: 0x04001101 RID: 4353
		private PerkObject _medicinePhysicianOfPeople;

		// Token: 0x04001102 RID: 4354
		private PerkObject _medicineCleanInfrastructure;

		// Token: 0x04001103 RID: 4355
		private PerkObject _medicineCheatDeath;

		// Token: 0x04001104 RID: 4356
		private PerkObject _medicineHelpingHands;

		// Token: 0x04001105 RID: 4357
		private PerkObject _medicineFortitudeTonic;

		// Token: 0x04001106 RID: 4358
		private PerkObject _medicineBattleHardened;

		// Token: 0x04001107 RID: 4359
		private PerkObject _medicineMinisterOfHealth;

		// Token: 0x04001108 RID: 4360
		private PerkObject _medicineSelfMedication;

		// Token: 0x04001109 RID: 4361
		private PerkObject _stewardFrugal;

		// Token: 0x0400110A RID: 4362
		private PerkObject _stewardSevenVeterans;

		// Token: 0x0400110B RID: 4363
		private PerkObject _stewardDrillSergant;

		// Token: 0x0400110C RID: 4364
		private PerkObject _stewardSweatshops;

		// Token: 0x0400110D RID: 4365
		private PerkObject _stewardEfficientCampaigner;

		// Token: 0x0400110E RID: 4366
		private PerkObject _stewardGivingHands;

		// Token: 0x0400110F RID: 4367
		private PerkObject _stewardLogistician;

		// Token: 0x04001110 RID: 4368
		private PerkObject _stewardStiffUpperLip;

		// Token: 0x04001111 RID: 4369
		private PerkObject _stewardPaidInPromise;

		// Token: 0x04001112 RID: 4370
		private PerkObject _stewardRelocation;

		// Token: 0x04001113 RID: 4371
		private PerkObject _stewardAidCorps;

		// Token: 0x04001114 RID: 4372
		private PerkObject _stewardGourmet;

		// Token: 0x04001115 RID: 4373
		private PerkObject _stewardSoundReserves;

		// Token: 0x04001116 RID: 4374
		private PerkObject _stewardArenicosMules;

		// Token: 0x04001117 RID: 4375
		private PerkObject _stewardForcedLabor;

		// Token: 0x04001118 RID: 4376
		private PerkObject _stewardPriceOfLoyalty;

		// Token: 0x04001119 RID: 4377
		private PerkObject _stewardContractors;

		// Token: 0x0400111A RID: 4378
		private PerkObject _stewardMasterOfWarcraft;

		// Token: 0x0400111B RID: 4379
		private PerkObject _stewardMasterOfPlanning;

		// Token: 0x0400111C RID: 4380
		private PerkObject _stewardWarriorsDiet;

		// Token: 0x0400111D RID: 4381
		private PerkObject _stewardArenicosHorses;

		// Token: 0x0400111E RID: 4382
		private PerkObject _engineeringSiegeWorks;

		// Token: 0x0400111F RID: 4383
		private PerkObject _engineeringCarpenters;

		// Token: 0x04001120 RID: 4384
		private PerkObject _engineeringDungeonArchitect;

		// Token: 0x04001121 RID: 4385
		private PerkObject _engineeringMilitaryPlanner;

		// Token: 0x04001122 RID: 4386
		private PerkObject _engineeringDreadfulSieger;

		// Token: 0x04001123 RID: 4387
		private PerkObject _engineeringTorsionEngines;

		// Token: 0x04001124 RID: 4388
		private PerkObject _engineeringSalvager;

		// Token: 0x04001125 RID: 4389
		private PerkObject _engineeringForeman;

		// Token: 0x04001126 RID: 4390
		private PerkObject _engineeringWallBreaker;

		// Token: 0x04001127 RID: 4391
		private PerkObject _engineeringStonecutters;

		// Token: 0x04001128 RID: 4392
		private PerkObject _engineeringSiegeEngineer;

		// Token: 0x04001129 RID: 4393
		private PerkObject _engineeringCampBuilding;

		// Token: 0x0400112A RID: 4394
		private PerkObject _engineeringBattlements;

		// Token: 0x0400112B RID: 4395
		private PerkObject _engineeringEngineeringGuilds;

		// Token: 0x0400112C RID: 4396
		private PerkObject _engineeringApprenticeship;

		// Token: 0x0400112D RID: 4397
		private PerkObject _engineeringMetallurgy;

		// Token: 0x0400112E RID: 4398
		private PerkObject _engineeringImprovedTools;

		// Token: 0x0400112F RID: 4399
		private PerkObject _engineeringClockwork;

		// Token: 0x04001130 RID: 4400
		private PerkObject _engineeringArchitecturalCommisions;

		// Token: 0x04001131 RID: 4401
		private PerkObject _engineeringScaffolds;

		// Token: 0x04001132 RID: 4402
		private PerkObject _engineeringMasterwork;

		// Token: 0x020007A3 RID: 1955
		public static class OneHanded
		{
			// Token: 0x17001411 RID: 5137
			// (get) Token: 0x0600642A RID: 25642 RVA: 0x001CE01E File Offset: 0x001CC21E
			public static PerkObject WrappedHandles
			{
				get
				{
					return DefaultPerks.Instance._oneHandedWrappedHandles;
				}
			}

			// Token: 0x17001412 RID: 5138
			// (get) Token: 0x0600642B RID: 25643 RVA: 0x001CE02A File Offset: 0x001CC22A
			public static PerkObject Basher
			{
				get
				{
					return DefaultPerks.Instance._oneHandedBasher;
				}
			}

			// Token: 0x17001413 RID: 5139
			// (get) Token: 0x0600642C RID: 25644 RVA: 0x001CE036 File Offset: 0x001CC236
			public static PerkObject ToBeBlunt
			{
				get
				{
					return DefaultPerks.Instance._oneHandedToBeBlunt;
				}
			}

			// Token: 0x17001414 RID: 5140
			// (get) Token: 0x0600642D RID: 25645 RVA: 0x001CE042 File Offset: 0x001CC242
			public static PerkObject SwiftStrike
			{
				get
				{
					return DefaultPerks.Instance._oneHandedSwiftStrike;
				}
			}

			// Token: 0x17001415 RID: 5141
			// (get) Token: 0x0600642E RID: 25646 RVA: 0x001CE04E File Offset: 0x001CC24E
			public static PerkObject Cavalry
			{
				get
				{
					return DefaultPerks.Instance._oneHandedCavalry;
				}
			}

			// Token: 0x17001416 RID: 5142
			// (get) Token: 0x0600642F RID: 25647 RVA: 0x001CE05A File Offset: 0x001CC25A
			public static PerkObject ShieldBearer
			{
				get
				{
					return DefaultPerks.Instance._oneHandedShieldBearer;
				}
			}

			// Token: 0x17001417 RID: 5143
			// (get) Token: 0x06006430 RID: 25648 RVA: 0x001CE066 File Offset: 0x001CC266
			public static PerkObject Trainer
			{
				get
				{
					return DefaultPerks.Instance._oneHandedTrainer;
				}
			}

			// Token: 0x17001418 RID: 5144
			// (get) Token: 0x06006431 RID: 25649 RVA: 0x001CE072 File Offset: 0x001CC272
			public static PerkObject Duelist
			{
				get
				{
					return DefaultPerks.Instance._oneHandedDuelist;
				}
			}

			// Token: 0x17001419 RID: 5145
			// (get) Token: 0x06006432 RID: 25650 RVA: 0x001CE07E File Offset: 0x001CC27E
			public static PerkObject ShieldWall
			{
				get
				{
					return DefaultPerks.Instance._oneHandedShieldWall;
				}
			}

			// Token: 0x1700141A RID: 5146
			// (get) Token: 0x06006433 RID: 25651 RVA: 0x001CE08A File Offset: 0x001CC28A
			public static PerkObject ArrowCatcher
			{
				get
				{
					return DefaultPerks.Instance._oneHandedArrowCatcher;
				}
			}

			// Token: 0x1700141B RID: 5147
			// (get) Token: 0x06006434 RID: 25652 RVA: 0x001CE096 File Offset: 0x001CC296
			public static PerkObject MilitaryTradition
			{
				get
				{
					return DefaultPerks.Instance._oneHandedMilitaryTradition;
				}
			}

			// Token: 0x1700141C RID: 5148
			// (get) Token: 0x06006435 RID: 25653 RVA: 0x001CE0A2 File Offset: 0x001CC2A2
			public static PerkObject CorpsACorps
			{
				get
				{
					return DefaultPerks.Instance._oneHandedCorpsACorps;
				}
			}

			// Token: 0x1700141D RID: 5149
			// (get) Token: 0x06006436 RID: 25654 RVA: 0x001CE0AE File Offset: 0x001CC2AE
			public static PerkObject StandUnited
			{
				get
				{
					return DefaultPerks.Instance._oneHandedStandUnited;
				}
			}

			// Token: 0x1700141E RID: 5150
			// (get) Token: 0x06006437 RID: 25655 RVA: 0x001CE0BA File Offset: 0x001CC2BA
			public static PerkObject LeadByExample
			{
				get
				{
					return DefaultPerks.Instance._oneHandedLeadByExample;
				}
			}

			// Token: 0x1700141F RID: 5151
			// (get) Token: 0x06006438 RID: 25656 RVA: 0x001CE0C6 File Offset: 0x001CC2C6
			public static PerkObject SteelCoreShields
			{
				get
				{
					return DefaultPerks.Instance._oneHandedSteelCoreShields;
				}
			}

			// Token: 0x17001420 RID: 5152
			// (get) Token: 0x06006439 RID: 25657 RVA: 0x001CE0D2 File Offset: 0x001CC2D2
			public static PerkObject FleetOfFoot
			{
				get
				{
					return DefaultPerks.Instance._oneHandedFleetOfFoot;
				}
			}

			// Token: 0x17001421 RID: 5153
			// (get) Token: 0x0600643A RID: 25658 RVA: 0x001CE0DE File Offset: 0x001CC2DE
			public static PerkObject DeadlyPurpose
			{
				get
				{
					return DefaultPerks.Instance._oneHandedDeadlyPurpose;
				}
			}

			// Token: 0x17001422 RID: 5154
			// (get) Token: 0x0600643B RID: 25659 RVA: 0x001CE0EA File Offset: 0x001CC2EA
			public static PerkObject UnwaveringDefense
			{
				get
				{
					return DefaultPerks.Instance._oneHandedUnwaveringDefense;
				}
			}

			// Token: 0x17001423 RID: 5155
			// (get) Token: 0x0600643C RID: 25660 RVA: 0x001CE0F6 File Offset: 0x001CC2F6
			public static PerkObject Prestige
			{
				get
				{
					return DefaultPerks.Instance._oneHandedPrestige;
				}
			}

			// Token: 0x17001424 RID: 5156
			// (get) Token: 0x0600643D RID: 25661 RVA: 0x001CE102 File Offset: 0x001CC302
			public static PerkObject WayOfTheSword
			{
				get
				{
					return DefaultPerks.Instance._oneHandedWayOfTheSword;
				}
			}

			// Token: 0x17001425 RID: 5157
			// (get) Token: 0x0600643E RID: 25662 RVA: 0x001CE10E File Offset: 0x001CC30E
			public static PerkObject ChinkInTheArmor
			{
				get
				{
					return DefaultPerks.Instance._oneHandedChinkInTheArmor;
				}
			}
		}

		// Token: 0x020007A4 RID: 1956
		public static class TwoHanded
		{
			// Token: 0x17001426 RID: 5158
			// (get) Token: 0x0600643F RID: 25663 RVA: 0x001CE11A File Offset: 0x001CC31A
			public static PerkObject StrongGrip
			{
				get
				{
					return DefaultPerks.Instance._twoHandedStrongGrip;
				}
			}

			// Token: 0x17001427 RID: 5159
			// (get) Token: 0x06006440 RID: 25664 RVA: 0x001CE126 File Offset: 0x001CC326
			public static PerkObject WoodChopper
			{
				get
				{
					return DefaultPerks.Instance._twoHandedWoodChopper;
				}
			}

			// Token: 0x17001428 RID: 5160
			// (get) Token: 0x06006441 RID: 25665 RVA: 0x001CE132 File Offset: 0x001CC332
			public static PerkObject OnTheEdge
			{
				get
				{
					return DefaultPerks.Instance._twoHandedOnTheEdge;
				}
			}

			// Token: 0x17001429 RID: 5161
			// (get) Token: 0x06006442 RID: 25666 RVA: 0x001CE13E File Offset: 0x001CC33E
			public static PerkObject HeadBasher
			{
				get
				{
					return DefaultPerks.Instance._twoHandedHeadBasher;
				}
			}

			// Token: 0x1700142A RID: 5162
			// (get) Token: 0x06006443 RID: 25667 RVA: 0x001CE14A File Offset: 0x001CC34A
			public static PerkObject ShowOfStrength
			{
				get
				{
					return DefaultPerks.Instance._twoHandedShowOfStrength;
				}
			}

			// Token: 0x1700142B RID: 5163
			// (get) Token: 0x06006444 RID: 25668 RVA: 0x001CE156 File Offset: 0x001CC356
			public static PerkObject BaptisedInBlood
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBaptisedInBlood;
				}
			}

			// Token: 0x1700142C RID: 5164
			// (get) Token: 0x06006445 RID: 25669 RVA: 0x001CE162 File Offset: 0x001CC362
			public static PerkObject BeastSlayer
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBeastSlayer;
				}
			}

			// Token: 0x1700142D RID: 5165
			// (get) Token: 0x06006446 RID: 25670 RVA: 0x001CE16E File Offset: 0x001CC36E
			public static PerkObject ShieldBreaker
			{
				get
				{
					return DefaultPerks.Instance._twoHandedShieldBreaker;
				}
			}

			// Token: 0x1700142E RID: 5166
			// (get) Token: 0x06006447 RID: 25671 RVA: 0x001CE17A File Offset: 0x001CC37A
			public static PerkObject Confidence
			{
				get
				{
					return DefaultPerks.Instance._twoHandedConfidence;
				}
			}

			// Token: 0x1700142F RID: 5167
			// (get) Token: 0x06006448 RID: 25672 RVA: 0x001CE186 File Offset: 0x001CC386
			public static PerkObject Berserker
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBerserker;
				}
			}

			// Token: 0x17001430 RID: 5168
			// (get) Token: 0x06006449 RID: 25673 RVA: 0x001CE192 File Offset: 0x001CC392
			public static PerkObject ProjectileDeflection
			{
				get
				{
					return DefaultPerks.Instance._twoHandedProjectileDeflection;
				}
			}

			// Token: 0x17001431 RID: 5169
			// (get) Token: 0x0600644A RID: 25674 RVA: 0x001CE19E File Offset: 0x001CC39E
			public static PerkObject Terror
			{
				get
				{
					return DefaultPerks.Instance._twoHandedTerror;
				}
			}

			// Token: 0x17001432 RID: 5170
			// (get) Token: 0x0600644B RID: 25675 RVA: 0x001CE1AA File Offset: 0x001CC3AA
			public static PerkObject Hope
			{
				get
				{
					return DefaultPerks.Instance._twoHandedHope;
				}
			}

			// Token: 0x17001433 RID: 5171
			// (get) Token: 0x0600644C RID: 25676 RVA: 0x001CE1B6 File Offset: 0x001CC3B6
			public static PerkObject RecklessCharge
			{
				get
				{
					return DefaultPerks.Instance._twoHandedRecklessCharge;
				}
			}

			// Token: 0x17001434 RID: 5172
			// (get) Token: 0x0600644D RID: 25677 RVA: 0x001CE1C2 File Offset: 0x001CC3C2
			public static PerkObject ThickHides
			{
				get
				{
					return DefaultPerks.Instance._twoHandedThickHides;
				}
			}

			// Token: 0x17001435 RID: 5173
			// (get) Token: 0x0600644E RID: 25678 RVA: 0x001CE1CE File Offset: 0x001CC3CE
			public static PerkObject BladeMaster
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBladeMaster;
				}
			}

			// Token: 0x17001436 RID: 5174
			// (get) Token: 0x0600644F RID: 25679 RVA: 0x001CE1DA File Offset: 0x001CC3DA
			public static PerkObject Vandal
			{
				get
				{
					return DefaultPerks.Instance._twoHandedVandal;
				}
			}

			// Token: 0x17001437 RID: 5175
			// (get) Token: 0x06006450 RID: 25680 RVA: 0x001CE1E6 File Offset: 0x001CC3E6
			public static PerkObject WayOfTheGreatAxe
			{
				get
				{
					return DefaultPerks.Instance._twoHandedWayOfTheGreatAxe;
				}
			}
		}

		// Token: 0x020007A5 RID: 1957
		public static class Polearm
		{
			// Token: 0x17001438 RID: 5176
			// (get) Token: 0x06006451 RID: 25681 RVA: 0x001CE1F2 File Offset: 0x001CC3F2
			public static PerkObject Pikeman
			{
				get
				{
					return DefaultPerks.Instance._polearmPikeman;
				}
			}

			// Token: 0x17001439 RID: 5177
			// (get) Token: 0x06006452 RID: 25682 RVA: 0x001CE1FE File Offset: 0x001CC3FE
			public static PerkObject Cavalry
			{
				get
				{
					return DefaultPerks.Instance._polearmCavalry;
				}
			}

			// Token: 0x1700143A RID: 5178
			// (get) Token: 0x06006453 RID: 25683 RVA: 0x001CE20A File Offset: 0x001CC40A
			public static PerkObject Braced
			{
				get
				{
					return DefaultPerks.Instance._polearmBraced;
				}
			}

			// Token: 0x1700143B RID: 5179
			// (get) Token: 0x06006454 RID: 25684 RVA: 0x001CE216 File Offset: 0x001CC416
			public static PerkObject KeepAtBay
			{
				get
				{
					return DefaultPerks.Instance._polearmKeepAtBay;
				}
			}

			// Token: 0x1700143C RID: 5180
			// (get) Token: 0x06006455 RID: 25685 RVA: 0x001CE222 File Offset: 0x001CC422
			public static PerkObject SwiftSwing
			{
				get
				{
					return DefaultPerks.Instance._polearmSwiftSwing;
				}
			}

			// Token: 0x1700143D RID: 5181
			// (get) Token: 0x06006456 RID: 25686 RVA: 0x001CE22E File Offset: 0x001CC42E
			public static PerkObject CleanThrust
			{
				get
				{
					return DefaultPerks.Instance._polearmCleanThrust;
				}
			}

			// Token: 0x1700143E RID: 5182
			// (get) Token: 0x06006457 RID: 25687 RVA: 0x001CE23A File Offset: 0x001CC43A
			public static PerkObject Footwork
			{
				get
				{
					return DefaultPerks.Instance._polearmFootwork;
				}
			}

			// Token: 0x1700143F RID: 5183
			// (get) Token: 0x06006458 RID: 25688 RVA: 0x001CE246 File Offset: 0x001CC446
			public static PerkObject HardKnock
			{
				get
				{
					return DefaultPerks.Instance._polearmHardKnock;
				}
			}

			// Token: 0x17001440 RID: 5184
			// (get) Token: 0x06006459 RID: 25689 RVA: 0x001CE252 File Offset: 0x001CC452
			public static PerkObject SteedKiller
			{
				get
				{
					return DefaultPerks.Instance._polearmSteedKiller;
				}
			}

			// Token: 0x17001441 RID: 5185
			// (get) Token: 0x0600645A RID: 25690 RVA: 0x001CE25E File Offset: 0x001CC45E
			public static PerkObject Lancer
			{
				get
				{
					return DefaultPerks.Instance._polearmLancer;
				}
			}

			// Token: 0x17001442 RID: 5186
			// (get) Token: 0x0600645B RID: 25691 RVA: 0x001CE26A File Offset: 0x001CC46A
			public static PerkObject Skewer
			{
				get
				{
					return DefaultPerks.Instance._polearmSkewer;
				}
			}

			// Token: 0x17001443 RID: 5187
			// (get) Token: 0x0600645C RID: 25692 RVA: 0x001CE276 File Offset: 0x001CC476
			public static PerkObject Guards
			{
				get
				{
					return DefaultPerks.Instance._polearmGuards;
				}
			}

			// Token: 0x17001444 RID: 5188
			// (get) Token: 0x0600645D RID: 25693 RVA: 0x001CE282 File Offset: 0x001CC482
			public static PerkObject StandardBearer
			{
				get
				{
					return DefaultPerks.Instance._polearmStandardBearer;
				}
			}

			// Token: 0x17001445 RID: 5189
			// (get) Token: 0x0600645E RID: 25694 RVA: 0x001CE28E File Offset: 0x001CC48E
			public static PerkObject Phalanx
			{
				get
				{
					return DefaultPerks.Instance._polearmPhalanx;
				}
			}

			// Token: 0x17001446 RID: 5190
			// (get) Token: 0x0600645F RID: 25695 RVA: 0x001CE29A File Offset: 0x001CC49A
			public static PerkObject HardyFrontline
			{
				get
				{
					return DefaultPerks.Instance._polearmHardyFrontline;
				}
			}

			// Token: 0x17001447 RID: 5191
			// (get) Token: 0x06006460 RID: 25696 RVA: 0x001CE2A6 File Offset: 0x001CC4A6
			public static PerkObject Drills
			{
				get
				{
					return DefaultPerks.Instance._polearmDrills;
				}
			}

			// Token: 0x17001448 RID: 5192
			// (get) Token: 0x06006461 RID: 25697 RVA: 0x001CE2B2 File Offset: 0x001CC4B2
			public static PerkObject SureFooted
			{
				get
				{
					return DefaultPerks.Instance._polearmSureFooted;
				}
			}

			// Token: 0x17001449 RID: 5193
			// (get) Token: 0x06006462 RID: 25698 RVA: 0x001CE2BE File Offset: 0x001CC4BE
			public static PerkObject UnstoppableForce
			{
				get
				{
					return DefaultPerks.Instance._polearmUnstoppableForce;
				}
			}

			// Token: 0x1700144A RID: 5194
			// (get) Token: 0x06006463 RID: 25699 RVA: 0x001CE2CA File Offset: 0x001CC4CA
			public static PerkObject CounterWeight
			{
				get
				{
					return DefaultPerks.Instance._polearmCounterweight;
				}
			}

			// Token: 0x1700144B RID: 5195
			// (get) Token: 0x06006464 RID: 25700 RVA: 0x001CE2D6 File Offset: 0x001CC4D6
			public static PerkObject SharpenTheTip
			{
				get
				{
					return DefaultPerks.Instance._polearmSharpenTheTip;
				}
			}

			// Token: 0x1700144C RID: 5196
			// (get) Token: 0x06006465 RID: 25701 RVA: 0x001CE2E2 File Offset: 0x001CC4E2
			public static PerkObject WayOfTheSpear
			{
				get
				{
					return DefaultPerks.Instance._polearmWayOfTheSpear;
				}
			}
		}

		// Token: 0x020007A6 RID: 1958
		public static class Bow
		{
			// Token: 0x1700144D RID: 5197
			// (get) Token: 0x06006466 RID: 25702 RVA: 0x001CE2EE File Offset: 0x001CC4EE
			public static PerkObject BowControl
			{
				get
				{
					return DefaultPerks.Instance._bowBowControl;
				}
			}

			// Token: 0x1700144E RID: 5198
			// (get) Token: 0x06006467 RID: 25703 RVA: 0x001CE2FA File Offset: 0x001CC4FA
			public static PerkObject DeadAim
			{
				get
				{
					return DefaultPerks.Instance._bowDeadAim;
				}
			}

			// Token: 0x1700144F RID: 5199
			// (get) Token: 0x06006468 RID: 25704 RVA: 0x001CE306 File Offset: 0x001CC506
			public static PerkObject Bodkin
			{
				get
				{
					return DefaultPerks.Instance._bowBodkin;
				}
			}

			// Token: 0x17001450 RID: 5200
			// (get) Token: 0x06006469 RID: 25705 RVA: 0x001CE312 File Offset: 0x001CC512
			public static PerkObject RangersSwiftness
			{
				get
				{
					return DefaultPerks.Instance._bowRangersSwiftness;
				}
			}

			// Token: 0x17001451 RID: 5201
			// (get) Token: 0x0600646A RID: 25706 RVA: 0x001CE31E File Offset: 0x001CC51E
			public static PerkObject RapidFire
			{
				get
				{
					return DefaultPerks.Instance._bowRapidFire;
				}
			}

			// Token: 0x17001452 RID: 5202
			// (get) Token: 0x0600646B RID: 25707 RVA: 0x001CE32A File Offset: 0x001CC52A
			public static PerkObject QuickAdjustments
			{
				get
				{
					return DefaultPerks.Instance._bowQuickAdjustments;
				}
			}

			// Token: 0x17001453 RID: 5203
			// (get) Token: 0x0600646C RID: 25708 RVA: 0x001CE336 File Offset: 0x001CC536
			public static PerkObject MerryMen
			{
				get
				{
					return DefaultPerks.Instance._bowMerryMen;
				}
			}

			// Token: 0x17001454 RID: 5204
			// (get) Token: 0x0600646D RID: 25709 RVA: 0x001CE342 File Offset: 0x001CC542
			public static PerkObject MountedArchery
			{
				get
				{
					return DefaultPerks.Instance._bowMountedArchery;
				}
			}

			// Token: 0x17001455 RID: 5205
			// (get) Token: 0x0600646E RID: 25710 RVA: 0x001CE34E File Offset: 0x001CC54E
			public static PerkObject Trainer
			{
				get
				{
					return DefaultPerks.Instance._bowTrainer;
				}
			}

			// Token: 0x17001456 RID: 5206
			// (get) Token: 0x0600646F RID: 25711 RVA: 0x001CE35A File Offset: 0x001CC55A
			public static PerkObject StrongBows
			{
				get
				{
					return DefaultPerks.Instance._bowStrongBows;
				}
			}

			// Token: 0x17001457 RID: 5207
			// (get) Token: 0x06006470 RID: 25712 RVA: 0x001CE366 File Offset: 0x001CC566
			public static PerkObject Discipline
			{
				get
				{
					return DefaultPerks.Instance._bowDiscipline;
				}
			}

			// Token: 0x17001458 RID: 5208
			// (get) Token: 0x06006471 RID: 25713 RVA: 0x001CE372 File Offset: 0x001CC572
			public static PerkObject HunterClan
			{
				get
				{
					return DefaultPerks.Instance._bowHunterClan;
				}
			}

			// Token: 0x17001459 RID: 5209
			// (get) Token: 0x06006472 RID: 25714 RVA: 0x001CE37E File Offset: 0x001CC57E
			public static PerkObject SkirmishPhaseMaster
			{
				get
				{
					return DefaultPerks.Instance._bowSkirmishPhaseMaster;
				}
			}

			// Token: 0x1700145A RID: 5210
			// (get) Token: 0x06006473 RID: 25715 RVA: 0x001CE38A File Offset: 0x001CC58A
			public static PerkObject EagleEye
			{
				get
				{
					return DefaultPerks.Instance._bowEagleEye;
				}
			}

			// Token: 0x1700145B RID: 5211
			// (get) Token: 0x06006474 RID: 25716 RVA: 0x001CE396 File Offset: 0x001CC596
			public static PerkObject BullsEye
			{
				get
				{
					return DefaultPerks.Instance._bowBullsEye;
				}
			}

			// Token: 0x1700145C RID: 5212
			// (get) Token: 0x06006475 RID: 25717 RVA: 0x001CE3A2 File Offset: 0x001CC5A2
			public static PerkObject RenownedArcher
			{
				get
				{
					return DefaultPerks.Instance._bowRenownedArcher;
				}
			}

			// Token: 0x1700145D RID: 5213
			// (get) Token: 0x06006476 RID: 25718 RVA: 0x001CE3AE File Offset: 0x001CC5AE
			public static PerkObject HorseMaster
			{
				get
				{
					return DefaultPerks.Instance._bowHorseMaster;
				}
			}

			// Token: 0x1700145E RID: 5214
			// (get) Token: 0x06006477 RID: 25719 RVA: 0x001CE3BA File Offset: 0x001CC5BA
			public static PerkObject DeepQuivers
			{
				get
				{
					return DefaultPerks.Instance._bowDeepQuivers;
				}
			}

			// Token: 0x1700145F RID: 5215
			// (get) Token: 0x06006478 RID: 25720 RVA: 0x001CE3C6 File Offset: 0x001CC5C6
			public static PerkObject QuickDraw
			{
				get
				{
					return DefaultPerks.Instance._bowQuickDraw;
				}
			}

			// Token: 0x17001460 RID: 5216
			// (get) Token: 0x06006479 RID: 25721 RVA: 0x001CE3D2 File Offset: 0x001CC5D2
			public static PerkObject NockingPoint
			{
				get
				{
					return DefaultPerks.Instance._bowNockingPoint;
				}
			}

			// Token: 0x17001461 RID: 5217
			// (get) Token: 0x0600647A RID: 25722 RVA: 0x001CE3DE File Offset: 0x001CC5DE
			public static PerkObject Deadshot
			{
				get
				{
					return DefaultPerks.Instance._bowDeadshot;
				}
			}
		}

		// Token: 0x020007A7 RID: 1959
		public static class Crossbow
		{
			// Token: 0x17001462 RID: 5218
			// (get) Token: 0x0600647B RID: 25723 RVA: 0x001CE3EA File Offset: 0x001CC5EA
			public static PerkObject Piercer
			{
				get
				{
					return DefaultPerks.Instance._crossbowPiercer;
				}
			}

			// Token: 0x17001463 RID: 5219
			// (get) Token: 0x0600647C RID: 25724 RVA: 0x001CE3F6 File Offset: 0x001CC5F6
			public static PerkObject Marksmen
			{
				get
				{
					return DefaultPerks.Instance._crossbowMarksmen;
				}
			}

			// Token: 0x17001464 RID: 5220
			// (get) Token: 0x0600647D RID: 25725 RVA: 0x001CE402 File Offset: 0x001CC602
			public static PerkObject Unhorser
			{
				get
				{
					return DefaultPerks.Instance._crossbowUnhorser;
				}
			}

			// Token: 0x17001465 RID: 5221
			// (get) Token: 0x0600647E RID: 25726 RVA: 0x001CE40E File Offset: 0x001CC60E
			public static PerkObject WindWinder
			{
				get
				{
					return DefaultPerks.Instance._crossbowWindWinder;
				}
			}

			// Token: 0x17001466 RID: 5222
			// (get) Token: 0x0600647F RID: 25727 RVA: 0x001CE41A File Offset: 0x001CC61A
			public static PerkObject DonkeysSwiftness
			{
				get
				{
					return DefaultPerks.Instance._crossbowDonkeysSwiftness;
				}
			}

			// Token: 0x17001467 RID: 5223
			// (get) Token: 0x06006480 RID: 25728 RVA: 0x001CE426 File Offset: 0x001CC626
			public static PerkObject Sheriff
			{
				get
				{
					return DefaultPerks.Instance._crossbowSheriff;
				}
			}

			// Token: 0x17001468 RID: 5224
			// (get) Token: 0x06006481 RID: 25729 RVA: 0x001CE432 File Offset: 0x001CC632
			public static PerkObject PeasantLeader
			{
				get
				{
					return DefaultPerks.Instance._crossbowPeasantLeader;
				}
			}

			// Token: 0x17001469 RID: 5225
			// (get) Token: 0x06006482 RID: 25730 RVA: 0x001CE43E File Offset: 0x001CC63E
			public static PerkObject RenownMarksmen
			{
				get
				{
					return DefaultPerks.Instance._crossbowRenownMarksmen;
				}
			}

			// Token: 0x1700146A RID: 5226
			// (get) Token: 0x06006483 RID: 25731 RVA: 0x001CE44A File Offset: 0x001CC64A
			public static PerkObject Fletcher
			{
				get
				{
					return DefaultPerks.Instance._crossbowFletcher;
				}
			}

			// Token: 0x1700146B RID: 5227
			// (get) Token: 0x06006484 RID: 25732 RVA: 0x001CE456 File Offset: 0x001CC656
			public static PerkObject Puncture
			{
				get
				{
					return DefaultPerks.Instance._crossbowPuncture;
				}
			}

			// Token: 0x1700146C RID: 5228
			// (get) Token: 0x06006485 RID: 25733 RVA: 0x001CE462 File Offset: 0x001CC662
			public static PerkObject LooseAndMove
			{
				get
				{
					return DefaultPerks.Instance._crossbowLooseAndMove;
				}
			}

			// Token: 0x1700146D RID: 5229
			// (get) Token: 0x06006486 RID: 25734 RVA: 0x001CE46E File Offset: 0x001CC66E
			public static PerkObject DeftHands
			{
				get
				{
					return DefaultPerks.Instance._crossbowDeftHands;
				}
			}

			// Token: 0x1700146E RID: 5230
			// (get) Token: 0x06006487 RID: 25735 RVA: 0x001CE47A File Offset: 0x001CC67A
			public static PerkObject CounterFire
			{
				get
				{
					return DefaultPerks.Instance._crossbowCounterFire;
				}
			}

			// Token: 0x1700146F RID: 5231
			// (get) Token: 0x06006488 RID: 25736 RVA: 0x001CE486 File Offset: 0x001CC686
			public static PerkObject MountedCrossbowman
			{
				get
				{
					return DefaultPerks.Instance._crossbowMountedCrossbowman;
				}
			}

			// Token: 0x17001470 RID: 5232
			// (get) Token: 0x06006489 RID: 25737 RVA: 0x001CE492 File Offset: 0x001CC692
			public static PerkObject Steady
			{
				get
				{
					return DefaultPerks.Instance._crossbowSteady;
				}
			}

			// Token: 0x17001471 RID: 5233
			// (get) Token: 0x0600648A RID: 25738 RVA: 0x001CE49E File Offset: 0x001CC69E
			public static PerkObject LongShots
			{
				get
				{
					return DefaultPerks.Instance._crossbowLongShots;
				}
			}

			// Token: 0x17001472 RID: 5234
			// (get) Token: 0x0600648B RID: 25739 RVA: 0x001CE4AA File Offset: 0x001CC6AA
			public static PerkObject HammerBolts
			{
				get
				{
					return DefaultPerks.Instance._crossbowHammerBolts;
				}
			}

			// Token: 0x17001473 RID: 5235
			// (get) Token: 0x0600648C RID: 25740 RVA: 0x001CE4B6 File Offset: 0x001CC6B6
			public static PerkObject Pavise
			{
				get
				{
					return DefaultPerks.Instance._crossbowPavise;
				}
			}

			// Token: 0x17001474 RID: 5236
			// (get) Token: 0x0600648D RID: 25741 RVA: 0x001CE4C2 File Offset: 0x001CC6C2
			public static PerkObject Terror
			{
				get
				{
					return DefaultPerks.Instance._crossbowTerror;
				}
			}

			// Token: 0x17001475 RID: 5237
			// (get) Token: 0x0600648E RID: 25742 RVA: 0x001CE4CE File Offset: 0x001CC6CE
			public static PerkObject PickedShots
			{
				get
				{
					return DefaultPerks.Instance._crossbowPickedShots;
				}
			}

			// Token: 0x17001476 RID: 5238
			// (get) Token: 0x0600648F RID: 25743 RVA: 0x001CE4DA File Offset: 0x001CC6DA
			public static PerkObject MightyPull
			{
				get
				{
					return DefaultPerks.Instance._crossbowMightyPull;
				}
			}
		}

		// Token: 0x020007A8 RID: 1960
		public static class Throwing
		{
			// Token: 0x17001477 RID: 5239
			// (get) Token: 0x06006490 RID: 25744 RVA: 0x001CE4E6 File Offset: 0x001CC6E6
			public static PerkObject QuickDraw
			{
				get
				{
					return DefaultPerks.Instance._throwingQuickDraw;
				}
			}

			// Token: 0x17001478 RID: 5240
			// (get) Token: 0x06006491 RID: 25745 RVA: 0x001CE4F2 File Offset: 0x001CC6F2
			public static PerkObject ShieldBreaker
			{
				get
				{
					return DefaultPerks.Instance._throwingShieldBreaker;
				}
			}

			// Token: 0x17001479 RID: 5241
			// (get) Token: 0x06006492 RID: 25746 RVA: 0x001CE4FE File Offset: 0x001CC6FE
			public static PerkObject Hunter
			{
				get
				{
					return DefaultPerks.Instance._throwingHunter;
				}
			}

			// Token: 0x1700147A RID: 5242
			// (get) Token: 0x06006493 RID: 25747 RVA: 0x001CE50A File Offset: 0x001CC70A
			public static PerkObject FlexibleFighter
			{
				get
				{
					return DefaultPerks.Instance._throwingFlexibleFighter;
				}
			}

			// Token: 0x1700147B RID: 5243
			// (get) Token: 0x06006494 RID: 25748 RVA: 0x001CE516 File Offset: 0x001CC716
			public static PerkObject MountedSkirmisher
			{
				get
				{
					return DefaultPerks.Instance._throwingMountedSkirmisher;
				}
			}

			// Token: 0x1700147C RID: 5244
			// (get) Token: 0x06006495 RID: 25749 RVA: 0x001CE522 File Offset: 0x001CC722
			public static PerkObject PerfectTechnique
			{
				get
				{
					return DefaultPerks.Instance._throwingPerfectTechnique;
				}
			}

			// Token: 0x1700147D RID: 5245
			// (get) Token: 0x06006496 RID: 25750 RVA: 0x001CE52E File Offset: 0x001CC72E
			public static PerkObject RunningThrow
			{
				get
				{
					return DefaultPerks.Instance._throwingRunningThrow;
				}
			}

			// Token: 0x1700147E RID: 5246
			// (get) Token: 0x06006497 RID: 25751 RVA: 0x001CE53A File Offset: 0x001CC73A
			public static PerkObject KnockOff
			{
				get
				{
					return DefaultPerks.Instance._throwingKnockOff;
				}
			}

			// Token: 0x1700147F RID: 5247
			// (get) Token: 0x06006498 RID: 25752 RVA: 0x001CE546 File Offset: 0x001CC746
			public static PerkObject WellPrepared
			{
				get
				{
					return DefaultPerks.Instance._throwingWellPrepared;
				}
			}

			// Token: 0x17001480 RID: 5248
			// (get) Token: 0x06006499 RID: 25753 RVA: 0x001CE552 File Offset: 0x001CC752
			public static PerkObject Skirmisher
			{
				get
				{
					return DefaultPerks.Instance._throwingSkirmisher;
				}
			}

			// Token: 0x17001481 RID: 5249
			// (get) Token: 0x0600649A RID: 25754 RVA: 0x001CE55E File Offset: 0x001CC75E
			public static PerkObject Focus
			{
				get
				{
					return DefaultPerks.Instance._throwingFocus;
				}
			}

			// Token: 0x17001482 RID: 5250
			// (get) Token: 0x0600649B RID: 25755 RVA: 0x001CE56A File Offset: 0x001CC76A
			public static PerkObject LastHit
			{
				get
				{
					return DefaultPerks.Instance._throwingLastHit;
				}
			}

			// Token: 0x17001483 RID: 5251
			// (get) Token: 0x0600649C RID: 25756 RVA: 0x001CE576 File Offset: 0x001CC776
			public static PerkObject HeadHunter
			{
				get
				{
					return DefaultPerks.Instance._throwingHeadHunter;
				}
			}

			// Token: 0x17001484 RID: 5252
			// (get) Token: 0x0600649D RID: 25757 RVA: 0x001CE582 File Offset: 0x001CC782
			public static PerkObject SlingingCompetitions
			{
				get
				{
					return DefaultPerks.Instance._throwingSlingingCompetitions;
				}
			}

			// Token: 0x17001485 RID: 5253
			// (get) Token: 0x0600649E RID: 25758 RVA: 0x001CE58E File Offset: 0x001CC78E
			public static PerkObject Saddlebags
			{
				get
				{
					return DefaultPerks.Instance._throwingSaddlebags;
				}
			}

			// Token: 0x17001486 RID: 5254
			// (get) Token: 0x0600649F RID: 25759 RVA: 0x001CE59A File Offset: 0x001CC79A
			public static PerkObject Splinters
			{
				get
				{
					return DefaultPerks.Instance._throwingSplinters;
				}
			}

			// Token: 0x17001487 RID: 5255
			// (get) Token: 0x060064A0 RID: 25760 RVA: 0x001CE5A6 File Offset: 0x001CC7A6
			public static PerkObject Resourceful
			{
				get
				{
					return DefaultPerks.Instance._throwingResourceful;
				}
			}

			// Token: 0x17001488 RID: 5256
			// (get) Token: 0x060064A1 RID: 25761 RVA: 0x001CE5B2 File Offset: 0x001CC7B2
			public static PerkObject LongReach
			{
				get
				{
					return DefaultPerks.Instance._throwingLongReach;
				}
			}

			// Token: 0x17001489 RID: 5257
			// (get) Token: 0x060064A2 RID: 25762 RVA: 0x001CE5BE File Offset: 0x001CC7BE
			public static PerkObject WeakSpot
			{
				get
				{
					return DefaultPerks.Instance._throwingWeakSpot;
				}
			}

			// Token: 0x1700148A RID: 5258
			// (get) Token: 0x060064A3 RID: 25763 RVA: 0x001CE5CA File Offset: 0x001CC7CA
			public static PerkObject Impale
			{
				get
				{
					return DefaultPerks.Instance._throwingImpale;
				}
			}

			// Token: 0x1700148B RID: 5259
			// (get) Token: 0x060064A4 RID: 25764 RVA: 0x001CE5D6 File Offset: 0x001CC7D6
			public static PerkObject UnstoppableForce
			{
				get
				{
					return DefaultPerks.Instance._throwingUnstoppableForce;
				}
			}
		}

		// Token: 0x020007A9 RID: 1961
		public static class Riding
		{
			// Token: 0x1700148C RID: 5260
			// (get) Token: 0x060064A5 RID: 25765 RVA: 0x001CE5E2 File Offset: 0x001CC7E2
			public static PerkObject FullSpeed
			{
				get
				{
					return DefaultPerks.Instance._ridingFullSpeed;
				}
			}

			// Token: 0x1700148D RID: 5261
			// (get) Token: 0x060064A6 RID: 25766 RVA: 0x001CE5EE File Offset: 0x001CC7EE
			public static PerkObject NimbleSteed
			{
				get
				{
					return DefaultPerks.Instance._ridingNimbleSteed;
				}
			}

			// Token: 0x1700148E RID: 5262
			// (get) Token: 0x060064A7 RID: 25767 RVA: 0x001CE5FA File Offset: 0x001CC7FA
			public static PerkObject WellStraped
			{
				get
				{
					return DefaultPerks.Instance._ridingWellStraped;
				}
			}

			// Token: 0x1700148F RID: 5263
			// (get) Token: 0x060064A8 RID: 25768 RVA: 0x001CE606 File Offset: 0x001CC806
			public static PerkObject Veterinary
			{
				get
				{
					return DefaultPerks.Instance._ridingVeterinary;
				}
			}

			// Token: 0x17001490 RID: 5264
			// (get) Token: 0x060064A9 RID: 25769 RVA: 0x001CE612 File Offset: 0x001CC812
			public static PerkObject NomadicTraditions
			{
				get
				{
					return DefaultPerks.Instance._ridingNomadicTraditions;
				}
			}

			// Token: 0x17001491 RID: 5265
			// (get) Token: 0x060064AA RID: 25770 RVA: 0x001CE61E File Offset: 0x001CC81E
			public static PerkObject DeeperSacks
			{
				get
				{
					return DefaultPerks.Instance._ridingDeeperSacks;
				}
			}

			// Token: 0x17001492 RID: 5266
			// (get) Token: 0x060064AB RID: 25771 RVA: 0x001CE62A File Offset: 0x001CC82A
			public static PerkObject Sagittarius
			{
				get
				{
					return DefaultPerks.Instance._ridingSagittarius;
				}
			}

			// Token: 0x17001493 RID: 5267
			// (get) Token: 0x060064AC RID: 25772 RVA: 0x001CE636 File Offset: 0x001CC836
			public static PerkObject SweepingWind
			{
				get
				{
					return DefaultPerks.Instance._ridingSweepingWind;
				}
			}

			// Token: 0x17001494 RID: 5268
			// (get) Token: 0x060064AD RID: 25773 RVA: 0x001CE642 File Offset: 0x001CC842
			public static PerkObject ReliefForce
			{
				get
				{
					return DefaultPerks.Instance._ridingReliefForce;
				}
			}

			// Token: 0x17001495 RID: 5269
			// (get) Token: 0x060064AE RID: 25774 RVA: 0x001CE64E File Offset: 0x001CC84E
			public static PerkObject MountedWarrior
			{
				get
				{
					return DefaultPerks.Instance._ridingMountedWarrior;
				}
			}

			// Token: 0x17001496 RID: 5270
			// (get) Token: 0x060064AF RID: 25775 RVA: 0x001CE65A File Offset: 0x001CC85A
			public static PerkObject HorseArcher
			{
				get
				{
					return DefaultPerks.Instance._ridingHorseArcher;
				}
			}

			// Token: 0x17001497 RID: 5271
			// (get) Token: 0x060064B0 RID: 25776 RVA: 0x001CE666 File Offset: 0x001CC866
			public static PerkObject Shepherd
			{
				get
				{
					return DefaultPerks.Instance._ridingShepherd;
				}
			}

			// Token: 0x17001498 RID: 5272
			// (get) Token: 0x060064B1 RID: 25777 RVA: 0x001CE672 File Offset: 0x001CC872
			public static PerkObject Breeder
			{
				get
				{
					return DefaultPerks.Instance._ridingBreeder;
				}
			}

			// Token: 0x17001499 RID: 5273
			// (get) Token: 0x060064B2 RID: 25778 RVA: 0x001CE67E File Offset: 0x001CC87E
			public static PerkObject ThunderousCharge
			{
				get
				{
					return DefaultPerks.Instance._ridingThunderousCharge;
				}
			}

			// Token: 0x1700149A RID: 5274
			// (get) Token: 0x060064B3 RID: 25779 RVA: 0x001CE68A File Offset: 0x001CC88A
			public static PerkObject AnnoyingBuzz
			{
				get
				{
					return DefaultPerks.Instance._ridingAnnoyingBuzz;
				}
			}

			// Token: 0x1700149B RID: 5275
			// (get) Token: 0x060064B4 RID: 25780 RVA: 0x001CE696 File Offset: 0x001CC896
			public static PerkObject MountedPatrols
			{
				get
				{
					return DefaultPerks.Instance._ridingMountedPatrols;
				}
			}

			// Token: 0x1700149C RID: 5276
			// (get) Token: 0x060064B5 RID: 25781 RVA: 0x001CE6A2 File Offset: 0x001CC8A2
			public static PerkObject CavalryTactics
			{
				get
				{
					return DefaultPerks.Instance._ridingCavalryTactics;
				}
			}

			// Token: 0x1700149D RID: 5277
			// (get) Token: 0x060064B6 RID: 25782 RVA: 0x001CE6AE File Offset: 0x001CC8AE
			public static PerkObject DauntlessSteed
			{
				get
				{
					return DefaultPerks.Instance._ridingDauntlessSteed;
				}
			}

			// Token: 0x1700149E RID: 5278
			// (get) Token: 0x060064B7 RID: 25783 RVA: 0x001CE6BA File Offset: 0x001CC8BA
			public static PerkObject ToughSteed
			{
				get
				{
					return DefaultPerks.Instance._ridingToughSteed;
				}
			}

			// Token: 0x1700149F RID: 5279
			// (get) Token: 0x060064B8 RID: 25784 RVA: 0x001CE6C6 File Offset: 0x001CC8C6
			public static PerkObject TheWayOfTheSaddle
			{
				get
				{
					return DefaultPerks.Instance._ridingTheWayOfTheSaddle;
				}
			}
		}

		// Token: 0x020007AA RID: 1962
		public static class Athletics
		{
			// Token: 0x170014A0 RID: 5280
			// (get) Token: 0x060064B9 RID: 25785 RVA: 0x001CE6D2 File Offset: 0x001CC8D2
			public static PerkObject MorningExercise
			{
				get
				{
					return DefaultPerks.Instance._athleticsMorningExercise;
				}
			}

			// Token: 0x170014A1 RID: 5281
			// (get) Token: 0x060064BA RID: 25786 RVA: 0x001CE6DE File Offset: 0x001CC8DE
			public static PerkObject WellBuilt
			{
				get
				{
					return DefaultPerks.Instance._athleticsWellBuilt;
				}
			}

			// Token: 0x170014A2 RID: 5282
			// (get) Token: 0x060064BB RID: 25787 RVA: 0x001CE6EA File Offset: 0x001CC8EA
			public static PerkObject Fury
			{
				get
				{
					return DefaultPerks.Instance._athleticsFury;
				}
			}

			// Token: 0x170014A3 RID: 5283
			// (get) Token: 0x060064BC RID: 25788 RVA: 0x001CE6F6 File Offset: 0x001CC8F6
			public static PerkObject FormFittingArmor
			{
				get
				{
					return DefaultPerks.Instance._athleticsFormFittingArmor;
				}
			}

			// Token: 0x170014A4 RID: 5284
			// (get) Token: 0x060064BD RID: 25789 RVA: 0x001CE702 File Offset: 0x001CC902
			public static PerkObject ImposingStature
			{
				get
				{
					return DefaultPerks.Instance._athleticsImposingStature;
				}
			}

			// Token: 0x170014A5 RID: 5285
			// (get) Token: 0x060064BE RID: 25790 RVA: 0x001CE70E File Offset: 0x001CC90E
			public static PerkObject Stamina
			{
				get
				{
					return DefaultPerks.Instance._athleticsStamina;
				}
			}

			// Token: 0x170014A6 RID: 5286
			// (get) Token: 0x060064BF RID: 25791 RVA: 0x001CE71A File Offset: 0x001CC91A
			public static PerkObject Sprint
			{
				get
				{
					return DefaultPerks.Instance._athleticsSprint;
				}
			}

			// Token: 0x170014A7 RID: 5287
			// (get) Token: 0x060064C0 RID: 25792 RVA: 0x001CE726 File Offset: 0x001CC926
			public static PerkObject Powerful
			{
				get
				{
					return DefaultPerks.Instance._athleticsPowerful;
				}
			}

			// Token: 0x170014A8 RID: 5288
			// (get) Token: 0x060064C1 RID: 25793 RVA: 0x001CE732 File Offset: 0x001CC932
			public static PerkObject SurgingBlow
			{
				get
				{
					return DefaultPerks.Instance._athleticsSurgingBlow;
				}
			}

			// Token: 0x170014A9 RID: 5289
			// (get) Token: 0x060064C2 RID: 25794 RVA: 0x001CE73E File Offset: 0x001CC93E
			public static PerkObject Braced
			{
				get
				{
					return DefaultPerks.Instance._athleticsBraced;
				}
			}

			// Token: 0x170014AA RID: 5290
			// (get) Token: 0x060064C3 RID: 25795 RVA: 0x001CE74A File Offset: 0x001CC94A
			public static PerkObject WalkItOff
			{
				get
				{
					return DefaultPerks.Instance._athleticsWalkItOff;
				}
			}

			// Token: 0x170014AB RID: 5291
			// (get) Token: 0x060064C4 RID: 25796 RVA: 0x001CE756 File Offset: 0x001CC956
			public static PerkObject AGoodDaysRest
			{
				get
				{
					return DefaultPerks.Instance._athleticsAGoodDaysRest;
				}
			}

			// Token: 0x170014AC RID: 5292
			// (get) Token: 0x060064C5 RID: 25797 RVA: 0x001CE762 File Offset: 0x001CC962
			public static PerkObject Durable
			{
				get
				{
					return DefaultPerks.Instance._athleticsDurable;
				}
			}

			// Token: 0x170014AD RID: 5293
			// (get) Token: 0x060064C6 RID: 25798 RVA: 0x001CE76E File Offset: 0x001CC96E
			public static PerkObject Energetic
			{
				get
				{
					return DefaultPerks.Instance._athleticsEnergetic;
				}
			}

			// Token: 0x170014AE RID: 5294
			// (get) Token: 0x060064C7 RID: 25799 RVA: 0x001CE77A File Offset: 0x001CC97A
			public static PerkObject Steady
			{
				get
				{
					return DefaultPerks.Instance._athleticsSteady;
				}
			}

			// Token: 0x170014AF RID: 5295
			// (get) Token: 0x060064C8 RID: 25800 RVA: 0x001CE786 File Offset: 0x001CC986
			public static PerkObject Strong
			{
				get
				{
					return DefaultPerks.Instance._athleticsStrong;
				}
			}

			// Token: 0x170014B0 RID: 5296
			// (get) Token: 0x060064C9 RID: 25801 RVA: 0x001CE792 File Offset: 0x001CC992
			public static PerkObject StrongLegs
			{
				get
				{
					return DefaultPerks.Instance._athleticsStrongLegs;
				}
			}

			// Token: 0x170014B1 RID: 5297
			// (get) Token: 0x060064CA RID: 25802 RVA: 0x001CE79E File Offset: 0x001CC99E
			public static PerkObject StrongArms
			{
				get
				{
					return DefaultPerks.Instance._athleticsStrongArms;
				}
			}

			// Token: 0x170014B2 RID: 5298
			// (get) Token: 0x060064CB RID: 25803 RVA: 0x001CE7AA File Offset: 0x001CC9AA
			public static PerkObject Spartan
			{
				get
				{
					return DefaultPerks.Instance._athleticsSpartan;
				}
			}

			// Token: 0x170014B3 RID: 5299
			// (get) Token: 0x060064CC RID: 25804 RVA: 0x001CE7B6 File Offset: 0x001CC9B6
			public static PerkObject IgnorePain
			{
				get
				{
					return DefaultPerks.Instance._athleticsIgnorePain;
				}
			}

			// Token: 0x170014B4 RID: 5300
			// (get) Token: 0x060064CD RID: 25805 RVA: 0x001CE7C2 File Offset: 0x001CC9C2
			public static PerkObject MightyBlow
			{
				get
				{
					return DefaultPerks.Instance._athleticsMightyBlow;
				}
			}
		}

		// Token: 0x020007AB RID: 1963
		public static class Crafting
		{
			// Token: 0x170014B5 RID: 5301
			// (get) Token: 0x060064CE RID: 25806 RVA: 0x001CE7CE File Offset: 0x001CC9CE
			public static PerkObject IronMaker
			{
				get
				{
					return DefaultPerks.Instance._craftingIronMaker;
				}
			}

			// Token: 0x170014B6 RID: 5302
			// (get) Token: 0x060064CF RID: 25807 RVA: 0x001CE7DA File Offset: 0x001CC9DA
			public static PerkObject CharcoalMaker
			{
				get
				{
					return DefaultPerks.Instance._craftingCharcoalMaker;
				}
			}

			// Token: 0x170014B7 RID: 5303
			// (get) Token: 0x060064D0 RID: 25808 RVA: 0x001CE7E6 File Offset: 0x001CC9E6
			public static PerkObject SteelMaker
			{
				get
				{
					return DefaultPerks.Instance._craftingSteelMaker;
				}
			}

			// Token: 0x170014B8 RID: 5304
			// (get) Token: 0x060064D1 RID: 25809 RVA: 0x001CE7F2 File Offset: 0x001CC9F2
			public static PerkObject SteelMaker2
			{
				get
				{
					return DefaultPerks.Instance._craftingSteelMaker2;
				}
			}

			// Token: 0x170014B9 RID: 5305
			// (get) Token: 0x060064D2 RID: 25810 RVA: 0x001CE7FE File Offset: 0x001CC9FE
			public static PerkObject SteelMaker3
			{
				get
				{
					return DefaultPerks.Instance._craftingSteelMaker3;
				}
			}

			// Token: 0x170014BA RID: 5306
			// (get) Token: 0x060064D3 RID: 25811 RVA: 0x001CE80A File Offset: 0x001CCA0A
			public static PerkObject CuriousSmelter
			{
				get
				{
					return DefaultPerks.Instance._craftingCuriousSmelter;
				}
			}

			// Token: 0x170014BB RID: 5307
			// (get) Token: 0x060064D4 RID: 25812 RVA: 0x001CE816 File Offset: 0x001CCA16
			public static PerkObject CuriousSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingCuriousSmith;
				}
			}

			// Token: 0x170014BC RID: 5308
			// (get) Token: 0x060064D5 RID: 25813 RVA: 0x001CE822 File Offset: 0x001CCA22
			public static PerkObject PracticalRefiner
			{
				get
				{
					return DefaultPerks.Instance._craftingPracticalRefiner;
				}
			}

			// Token: 0x170014BD RID: 5309
			// (get) Token: 0x060064D6 RID: 25814 RVA: 0x001CE82E File Offset: 0x001CCA2E
			public static PerkObject PracticalSmelter
			{
				get
				{
					return DefaultPerks.Instance._craftingPracticalSmelter;
				}
			}

			// Token: 0x170014BE RID: 5310
			// (get) Token: 0x060064D7 RID: 25815 RVA: 0x001CE83A File Offset: 0x001CCA3A
			public static PerkObject PracticalSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingPracticalSmith;
				}
			}

			// Token: 0x170014BF RID: 5311
			// (get) Token: 0x060064D8 RID: 25816 RVA: 0x001CE846 File Offset: 0x001CCA46
			public static PerkObject ArtisanSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingArtisanSmith;
				}
			}

			// Token: 0x170014C0 RID: 5312
			// (get) Token: 0x060064D9 RID: 25817 RVA: 0x001CE852 File Offset: 0x001CCA52
			public static PerkObject ExperiencedSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingExperiencedSmith;
				}
			}

			// Token: 0x170014C1 RID: 5313
			// (get) Token: 0x060064DA RID: 25818 RVA: 0x001CE85E File Offset: 0x001CCA5E
			public static PerkObject MasterSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingMasterSmith;
				}
			}

			// Token: 0x170014C2 RID: 5314
			// (get) Token: 0x060064DB RID: 25819 RVA: 0x001CE86A File Offset: 0x001CCA6A
			public static PerkObject LegendarySmith
			{
				get
				{
					return DefaultPerks.Instance._craftingLegendarySmith;
				}
			}

			// Token: 0x170014C3 RID: 5315
			// (get) Token: 0x060064DC RID: 25820 RVA: 0x001CE876 File Offset: 0x001CCA76
			public static PerkObject VigorousSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingVigorousSmith;
				}
			}

			// Token: 0x170014C4 RID: 5316
			// (get) Token: 0x060064DD RID: 25821 RVA: 0x001CE882 File Offset: 0x001CCA82
			public static PerkObject StrongSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingStrongSmith;
				}
			}

			// Token: 0x170014C5 RID: 5317
			// (get) Token: 0x060064DE RID: 25822 RVA: 0x001CE88E File Offset: 0x001CCA8E
			public static PerkObject EnduringSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingEnduringSmith;
				}
			}

			// Token: 0x170014C6 RID: 5318
			// (get) Token: 0x060064DF RID: 25823 RVA: 0x001CE89A File Offset: 0x001CCA9A
			public static PerkObject WeaponMasterSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingFencerSmith;
				}
			}

			// Token: 0x170014C7 RID: 5319
			// (get) Token: 0x060064E0 RID: 25824 RVA: 0x001CE8A6 File Offset: 0x001CCAA6
			public static PerkObject SharpenedEdge
			{
				get
				{
					return DefaultPerks.Instance._craftingSharpenedEdge;
				}
			}

			// Token: 0x170014C8 RID: 5320
			// (get) Token: 0x060064E1 RID: 25825 RVA: 0x001CE8B2 File Offset: 0x001CCAB2
			public static PerkObject SharpenedTip
			{
				get
				{
					return DefaultPerks.Instance._craftingSharpenedTip;
				}
			}
		}

		// Token: 0x020007AC RID: 1964
		public static class Scouting
		{
			// Token: 0x170014C9 RID: 5321
			// (get) Token: 0x060064E2 RID: 25826 RVA: 0x001CE8BE File Offset: 0x001CCABE
			public static PerkObject DayTraveler
			{
				get
				{
					return DefaultPerks.Instance._scoutingDayTraveler;
				}
			}

			// Token: 0x170014CA RID: 5322
			// (get) Token: 0x060064E3 RID: 25827 RVA: 0x001CE8CA File Offset: 0x001CCACA
			public static PerkObject Pathfinder
			{
				get
				{
					return DefaultPerks.Instance._scoutingPathfinder;
				}
			}

			// Token: 0x170014CB RID: 5323
			// (get) Token: 0x060064E4 RID: 25828 RVA: 0x001CE8D6 File Offset: 0x001CCAD6
			public static PerkObject NightRunner
			{
				get
				{
					return DefaultPerks.Instance._scoutingNightRunner;
				}
			}

			// Token: 0x170014CC RID: 5324
			// (get) Token: 0x060064E5 RID: 25829 RVA: 0x001CE8E2 File Offset: 0x001CCAE2
			public static PerkObject WaterDiviner
			{
				get
				{
					return DefaultPerks.Instance._scoutingWaterDiviner;
				}
			}

			// Token: 0x170014CD RID: 5325
			// (get) Token: 0x060064E6 RID: 25830 RVA: 0x001CE8EE File Offset: 0x001CCAEE
			public static PerkObject ForestKin
			{
				get
				{
					return DefaultPerks.Instance._scoutingForestKin;
				}
			}

			// Token: 0x170014CE RID: 5326
			// (get) Token: 0x060064E7 RID: 25831 RVA: 0x001CE8FA File Offset: 0x001CCAFA
			public static PerkObject DesertBorn
			{
				get
				{
					return DefaultPerks.Instance._scoutingDesertBorn;
				}
			}

			// Token: 0x170014CF RID: 5327
			// (get) Token: 0x060064E8 RID: 25832 RVA: 0x001CE906 File Offset: 0x001CCB06
			public static PerkObject ForcedMarch
			{
				get
				{
					return DefaultPerks.Instance._scoutingForcedMarch;
				}
			}

			// Token: 0x170014D0 RID: 5328
			// (get) Token: 0x060064E9 RID: 25833 RVA: 0x001CE912 File Offset: 0x001CCB12
			public static PerkObject Unburdened
			{
				get
				{
					return DefaultPerks.Instance._scoutingUnburdened;
				}
			}

			// Token: 0x170014D1 RID: 5329
			// (get) Token: 0x060064EA RID: 25834 RVA: 0x001CE91E File Offset: 0x001CCB1E
			public static PerkObject Tracker
			{
				get
				{
					return DefaultPerks.Instance._scoutingTracker;
				}
			}

			// Token: 0x170014D2 RID: 5330
			// (get) Token: 0x060064EB RID: 25835 RVA: 0x001CE92A File Offset: 0x001CCB2A
			public static PerkObject Ranger
			{
				get
				{
					return DefaultPerks.Instance._scoutingRanger;
				}
			}

			// Token: 0x170014D3 RID: 5331
			// (get) Token: 0x060064EC RID: 25836 RVA: 0x001CE936 File Offset: 0x001CCB36
			public static PerkObject MountedScouts
			{
				get
				{
					return DefaultPerks.Instance._scoutingMountedScouts;
				}
			}

			// Token: 0x170014D4 RID: 5332
			// (get) Token: 0x060064ED RID: 25837 RVA: 0x001CE942 File Offset: 0x001CCB42
			public static PerkObject Patrols
			{
				get
				{
					return DefaultPerks.Instance._scoutingPatrols;
				}
			}

			// Token: 0x170014D5 RID: 5333
			// (get) Token: 0x060064EE RID: 25838 RVA: 0x001CE94E File Offset: 0x001CCB4E
			public static PerkObject Foragers
			{
				get
				{
					return DefaultPerks.Instance._scoutingForagers;
				}
			}

			// Token: 0x170014D6 RID: 5334
			// (get) Token: 0x060064EF RID: 25839 RVA: 0x001CE95A File Offset: 0x001CCB5A
			public static PerkObject BeastWhisperer
			{
				get
				{
					return DefaultPerks.Instance._scoutingBeastWhisperer;
				}
			}

			// Token: 0x170014D7 RID: 5335
			// (get) Token: 0x060064F0 RID: 25840 RVA: 0x001CE966 File Offset: 0x001CCB66
			public static PerkObject VillageNetwork
			{
				get
				{
					return DefaultPerks.Instance._scoutingVillageNetwork;
				}
			}

			// Token: 0x170014D8 RID: 5336
			// (get) Token: 0x060064F1 RID: 25841 RVA: 0x001CE972 File Offset: 0x001CCB72
			public static PerkObject RumourNetwork
			{
				get
				{
					return DefaultPerks.Instance._scoutingRumourNetwork;
				}
			}

			// Token: 0x170014D9 RID: 5337
			// (get) Token: 0x060064F2 RID: 25842 RVA: 0x001CE97E File Offset: 0x001CCB7E
			public static PerkObject VantagePoint
			{
				get
				{
					return DefaultPerks.Instance._scoutingVantagePoint;
				}
			}

			// Token: 0x170014DA RID: 5338
			// (get) Token: 0x060064F3 RID: 25843 RVA: 0x001CE98A File Offset: 0x001CCB8A
			public static PerkObject KeenSight
			{
				get
				{
					return DefaultPerks.Instance._scoutingKeenSight;
				}
			}

			// Token: 0x170014DB RID: 5339
			// (get) Token: 0x060064F4 RID: 25844 RVA: 0x001CE996 File Offset: 0x001CCB96
			public static PerkObject Vanguard
			{
				get
				{
					return DefaultPerks.Instance._scoutingVanguard;
				}
			}

			// Token: 0x170014DC RID: 5340
			// (get) Token: 0x060064F5 RID: 25845 RVA: 0x001CE9A2 File Offset: 0x001CCBA2
			public static PerkObject Rearguard
			{
				get
				{
					return DefaultPerks.Instance._scoutingRearguard;
				}
			}

			// Token: 0x170014DD RID: 5341
			// (get) Token: 0x060064F6 RID: 25846 RVA: 0x001CE9AE File Offset: 0x001CCBAE
			public static PerkObject UncannyInsight
			{
				get
				{
					return DefaultPerks.Instance._scoutingUncannyInsight;
				}
			}
		}

		// Token: 0x020007AD RID: 1965
		public static class Tactics
		{
			// Token: 0x170014DE RID: 5342
			// (get) Token: 0x060064F7 RID: 25847 RVA: 0x001CE9BA File Offset: 0x001CCBBA
			public static PerkObject TightFormations
			{
				get
				{
					return DefaultPerks.Instance._tacticsTightFormations;
				}
			}

			// Token: 0x170014DF RID: 5343
			// (get) Token: 0x060064F8 RID: 25848 RVA: 0x001CE9C6 File Offset: 0x001CCBC6
			public static PerkObject LooseFormations
			{
				get
				{
					return DefaultPerks.Instance._tacticsLooseFormations;
				}
			}

			// Token: 0x170014E0 RID: 5344
			// (get) Token: 0x060064F9 RID: 25849 RVA: 0x001CE9D2 File Offset: 0x001CCBD2
			public static PerkObject ExtendedSkirmish
			{
				get
				{
					return DefaultPerks.Instance._tacticsExtendedSkirmish;
				}
			}

			// Token: 0x170014E1 RID: 5345
			// (get) Token: 0x060064FA RID: 25850 RVA: 0x001CE9DE File Offset: 0x001CCBDE
			public static PerkObject DecisiveBattle
			{
				get
				{
					return DefaultPerks.Instance._tacticsDecisiveBattle;
				}
			}

			// Token: 0x170014E2 RID: 5346
			// (get) Token: 0x060064FB RID: 25851 RVA: 0x001CE9EA File Offset: 0x001CCBEA
			public static PerkObject SmallUnitTactics
			{
				get
				{
					return DefaultPerks.Instance._tacticsSmallUnitTactics;
				}
			}

			// Token: 0x170014E3 RID: 5347
			// (get) Token: 0x060064FC RID: 25852 RVA: 0x001CE9F6 File Offset: 0x001CCBF6
			public static PerkObject HordeLeader
			{
				get
				{
					return DefaultPerks.Instance._tacticsHordeLeader;
				}
			}

			// Token: 0x170014E4 RID: 5348
			// (get) Token: 0x060064FD RID: 25853 RVA: 0x001CEA02 File Offset: 0x001CCC02
			public static PerkObject LawKeeper
			{
				get
				{
					return DefaultPerks.Instance._tacticsLawKeeper;
				}
			}

			// Token: 0x170014E5 RID: 5349
			// (get) Token: 0x060064FE RID: 25854 RVA: 0x001CEA0E File Offset: 0x001CCC0E
			public static PerkObject Coaching
			{
				get
				{
					return DefaultPerks.Instance._tacticsCoaching;
				}
			}

			// Token: 0x170014E6 RID: 5350
			// (get) Token: 0x060064FF RID: 25855 RVA: 0x001CEA1A File Offset: 0x001CCC1A
			public static PerkObject SwiftRegroup
			{
				get
				{
					return DefaultPerks.Instance._tacticsSwiftRegroup;
				}
			}

			// Token: 0x170014E7 RID: 5351
			// (get) Token: 0x06006500 RID: 25856 RVA: 0x001CEA26 File Offset: 0x001CCC26
			public static PerkObject Improviser
			{
				get
				{
					return DefaultPerks.Instance._tacticsImproviser;
				}
			}

			// Token: 0x170014E8 RID: 5352
			// (get) Token: 0x06006501 RID: 25857 RVA: 0x001CEA32 File Offset: 0x001CCC32
			public static PerkObject OnTheMarch
			{
				get
				{
					return DefaultPerks.Instance._tacticsOnTheMarch;
				}
			}

			// Token: 0x170014E9 RID: 5353
			// (get) Token: 0x06006502 RID: 25858 RVA: 0x001CEA3E File Offset: 0x001CCC3E
			public static PerkObject CallToArms
			{
				get
				{
					return DefaultPerks.Instance._tacticsCallToArms;
				}
			}

			// Token: 0x170014EA RID: 5354
			// (get) Token: 0x06006503 RID: 25859 RVA: 0x001CEA4A File Offset: 0x001CCC4A
			public static PerkObject PickThemOfTheWalls
			{
				get
				{
					return DefaultPerks.Instance._tacticsPickThemOfTheWalls;
				}
			}

			// Token: 0x170014EB RID: 5355
			// (get) Token: 0x06006504 RID: 25860 RVA: 0x001CEA56 File Offset: 0x001CCC56
			public static PerkObject MakeThemPay
			{
				get
				{
					return DefaultPerks.Instance._tacticsMakeThemPay;
				}
			}

			// Token: 0x170014EC RID: 5356
			// (get) Token: 0x06006505 RID: 25861 RVA: 0x001CEA62 File Offset: 0x001CCC62
			public static PerkObject EliteReserves
			{
				get
				{
					return DefaultPerks.Instance._tacticsEliteReserves;
				}
			}

			// Token: 0x170014ED RID: 5357
			// (get) Token: 0x06006506 RID: 25862 RVA: 0x001CEA6E File Offset: 0x001CCC6E
			public static PerkObject Encirclement
			{
				get
				{
					return DefaultPerks.Instance._tacticsEncirclement;
				}
			}

			// Token: 0x170014EE RID: 5358
			// (get) Token: 0x06006507 RID: 25863 RVA: 0x001CEA7A File Offset: 0x001CCC7A
			public static PerkObject PreBattleManeuvers
			{
				get
				{
					return DefaultPerks.Instance._tacticsPreBattleManeuvers;
				}
			}

			// Token: 0x170014EF RID: 5359
			// (get) Token: 0x06006508 RID: 25864 RVA: 0x001CEA86 File Offset: 0x001CCC86
			public static PerkObject Besieged
			{
				get
				{
					return DefaultPerks.Instance._tacticsBesieged;
				}
			}

			// Token: 0x170014F0 RID: 5360
			// (get) Token: 0x06006509 RID: 25865 RVA: 0x001CEA92 File Offset: 0x001CCC92
			public static PerkObject Counteroffensive
			{
				get
				{
					return DefaultPerks.Instance._tacticsCounteroffensive;
				}
			}

			// Token: 0x170014F1 RID: 5361
			// (get) Token: 0x0600650A RID: 25866 RVA: 0x001CEA9E File Offset: 0x001CCC9E
			public static PerkObject Gensdarmes
			{
				get
				{
					return DefaultPerks.Instance._tacticsGensdarmes;
				}
			}

			// Token: 0x170014F2 RID: 5362
			// (get) Token: 0x0600650B RID: 25867 RVA: 0x001CEAAA File Offset: 0x001CCCAA
			public static PerkObject TacticalMastery
			{
				get
				{
					return DefaultPerks.Instance._tacticsTacticalMastery;
				}
			}
		}

		// Token: 0x020007AE RID: 1966
		public static class Roguery
		{
			// Token: 0x170014F3 RID: 5363
			// (get) Token: 0x0600650C RID: 25868 RVA: 0x001CEAB6 File Offset: 0x001CCCB6
			public static PerkObject NoRestForTheWicked
			{
				get
				{
					return DefaultPerks.Instance._rogueryNoRestForTheWicked;
				}
			}

			// Token: 0x170014F4 RID: 5364
			// (get) Token: 0x0600650D RID: 25869 RVA: 0x001CEAC2 File Offset: 0x001CCCC2
			public static PerkObject SweetTalker
			{
				get
				{
					return DefaultPerks.Instance._roguerySweetTalker;
				}
			}

			// Token: 0x170014F5 RID: 5365
			// (get) Token: 0x0600650E RID: 25870 RVA: 0x001CEACE File Offset: 0x001CCCCE
			public static PerkObject TwoFaced
			{
				get
				{
					return DefaultPerks.Instance._rogueryTwoFaced;
				}
			}

			// Token: 0x170014F6 RID: 5366
			// (get) Token: 0x0600650F RID: 25871 RVA: 0x001CEADA File Offset: 0x001CCCDA
			public static PerkObject DeepPockets
			{
				get
				{
					return DefaultPerks.Instance._rogueryDeepPockets;
				}
			}

			// Token: 0x170014F7 RID: 5367
			// (get) Token: 0x06006510 RID: 25872 RVA: 0x001CEAE6 File Offset: 0x001CCCE6
			public static PerkObject InBestLight
			{
				get
				{
					return DefaultPerks.Instance._rogueryInBestLight;
				}
			}

			// Token: 0x170014F8 RID: 5368
			// (get) Token: 0x06006511 RID: 25873 RVA: 0x001CEAF2 File Offset: 0x001CCCF2
			public static PerkObject KnowHow
			{
				get
				{
					return DefaultPerks.Instance._rogueryKnowHow;
				}
			}

			// Token: 0x170014F9 RID: 5369
			// (get) Token: 0x06006512 RID: 25874 RVA: 0x001CEAFE File Offset: 0x001CCCFE
			public static PerkObject Promises
			{
				get
				{
					return DefaultPerks.Instance._rogueryPromises;
				}
			}

			// Token: 0x170014FA RID: 5370
			// (get) Token: 0x06006513 RID: 25875 RVA: 0x001CEB0A File Offset: 0x001CCD0A
			public static PerkObject Manhunter
			{
				get
				{
					return DefaultPerks.Instance._rogueryManhunter;
				}
			}

			// Token: 0x170014FB RID: 5371
			// (get) Token: 0x06006514 RID: 25876 RVA: 0x001CEB16 File Offset: 0x001CCD16
			public static PerkObject Scarface
			{
				get
				{
					return DefaultPerks.Instance._rogueryScarface;
				}
			}

			// Token: 0x170014FC RID: 5372
			// (get) Token: 0x06006515 RID: 25877 RVA: 0x001CEB22 File Offset: 0x001CCD22
			public static PerkObject WhiteLies
			{
				get
				{
					return DefaultPerks.Instance._rogueryWhiteLies;
				}
			}

			// Token: 0x170014FD RID: 5373
			// (get) Token: 0x06006516 RID: 25878 RVA: 0x001CEB2E File Offset: 0x001CCD2E
			public static PerkObject SmugglerConnections
			{
				get
				{
					return DefaultPerks.Instance._roguerySmugglerConnections;
				}
			}

			// Token: 0x170014FE RID: 5374
			// (get) Token: 0x06006517 RID: 25879 RVA: 0x001CEB3A File Offset: 0x001CCD3A
			public static PerkObject PartnersInCrime
			{
				get
				{
					return DefaultPerks.Instance._rogueryPartnersInCrime;
				}
			}

			// Token: 0x170014FF RID: 5375
			// (get) Token: 0x06006518 RID: 25880 RVA: 0x001CEB46 File Offset: 0x001CCD46
			public static PerkObject OneOfTheFamily
			{
				get
				{
					return DefaultPerks.Instance._rogueryOneOfTheFamily;
				}
			}

			// Token: 0x17001500 RID: 5376
			// (get) Token: 0x06006519 RID: 25881 RVA: 0x001CEB52 File Offset: 0x001CCD52
			public static PerkObject SaltTheEarth
			{
				get
				{
					return DefaultPerks.Instance._roguerySaltTheEarth;
				}
			}

			// Token: 0x17001501 RID: 5377
			// (get) Token: 0x0600651A RID: 25882 RVA: 0x001CEB5E File Offset: 0x001CCD5E
			public static PerkObject Carver
			{
				get
				{
					return DefaultPerks.Instance._rogueryCarver;
				}
			}

			// Token: 0x17001502 RID: 5378
			// (get) Token: 0x0600651B RID: 25883 RVA: 0x001CEB6A File Offset: 0x001CCD6A
			public static PerkObject RansomBroker
			{
				get
				{
					return DefaultPerks.Instance._rogueryRansomBroker;
				}
			}

			// Token: 0x17001503 RID: 5379
			// (get) Token: 0x0600651C RID: 25884 RVA: 0x001CEB76 File Offset: 0x001CCD76
			public static PerkObject ArmsDealer
			{
				get
				{
					return DefaultPerks.Instance._rogueryArmsDealer;
				}
			}

			// Token: 0x17001504 RID: 5380
			// (get) Token: 0x0600651D RID: 25885 RVA: 0x001CEB82 File Offset: 0x001CCD82
			public static PerkObject DirtyFighting
			{
				get
				{
					return DefaultPerks.Instance._rogueryDirtyFighting;
				}
			}

			// Token: 0x17001505 RID: 5381
			// (get) Token: 0x0600651E RID: 25886 RVA: 0x001CEB8E File Offset: 0x001CCD8E
			public static PerkObject DashAndSlash
			{
				get
				{
					return DefaultPerks.Instance._rogueryDashAndSlash;
				}
			}

			// Token: 0x17001506 RID: 5382
			// (get) Token: 0x0600651F RID: 25887 RVA: 0x001CEB9A File Offset: 0x001CCD9A
			public static PerkObject FleetFooted
			{
				get
				{
					return DefaultPerks.Instance._rogueryFleetFooted;
				}
			}

			// Token: 0x17001507 RID: 5383
			// (get) Token: 0x06006520 RID: 25888 RVA: 0x001CEBA6 File Offset: 0x001CCDA6
			public static PerkObject RogueExtraordinaire
			{
				get
				{
					return DefaultPerks.Instance._rogueryRogueExtraordinaire;
				}
			}
		}

		// Token: 0x020007AF RID: 1967
		public static class Charm
		{
			// Token: 0x17001508 RID: 5384
			// (get) Token: 0x06006521 RID: 25889 RVA: 0x001CEBB2 File Offset: 0x001CCDB2
			public static PerkObject Virile
			{
				get
				{
					return DefaultPerks.Instance._charmVirile;
				}
			}

			// Token: 0x17001509 RID: 5385
			// (get) Token: 0x06006522 RID: 25890 RVA: 0x001CEBBE File Offset: 0x001CCDBE
			public static PerkObject SelfPromoter
			{
				get
				{
					return DefaultPerks.Instance._charmSelfPromoter;
				}
			}

			// Token: 0x1700150A RID: 5386
			// (get) Token: 0x06006523 RID: 25891 RVA: 0x001CEBCA File Offset: 0x001CCDCA
			public static PerkObject Oratory
			{
				get
				{
					return DefaultPerks.Instance._charmOratory;
				}
			}

			// Token: 0x1700150B RID: 5387
			// (get) Token: 0x06006524 RID: 25892 RVA: 0x001CEBD6 File Offset: 0x001CCDD6
			public static PerkObject Warlord
			{
				get
				{
					return DefaultPerks.Instance._charmWarlord;
				}
			}

			// Token: 0x1700150C RID: 5388
			// (get) Token: 0x06006525 RID: 25893 RVA: 0x001CEBE2 File Offset: 0x001CCDE2
			public static PerkObject ForgivableGrievances
			{
				get
				{
					return DefaultPerks.Instance._charmForgivableGrievances;
				}
			}

			// Token: 0x1700150D RID: 5389
			// (get) Token: 0x06006526 RID: 25894 RVA: 0x001CEBEE File Offset: 0x001CCDEE
			public static PerkObject MeaningfulFavors
			{
				get
				{
					return DefaultPerks.Instance._charmMeaningfulFavors;
				}
			}

			// Token: 0x1700150E RID: 5390
			// (get) Token: 0x06006527 RID: 25895 RVA: 0x001CEBFA File Offset: 0x001CCDFA
			public static PerkObject InBloom
			{
				get
				{
					return DefaultPerks.Instance._charmInBloom;
				}
			}

			// Token: 0x1700150F RID: 5391
			// (get) Token: 0x06006528 RID: 25896 RVA: 0x001CEC06 File Offset: 0x001CCE06
			public static PerkObject YoungAndRespectful
			{
				get
				{
					return DefaultPerks.Instance._charmYoungAndRespectful;
				}
			}

			// Token: 0x17001510 RID: 5392
			// (get) Token: 0x06006529 RID: 25897 RVA: 0x001CEC12 File Offset: 0x001CCE12
			public static PerkObject Firebrand
			{
				get
				{
					return DefaultPerks.Instance._charmFirebrand;
				}
			}

			// Token: 0x17001511 RID: 5393
			// (get) Token: 0x0600652A RID: 25898 RVA: 0x001CEC1E File Offset: 0x001CCE1E
			public static PerkObject FlexibleEthics
			{
				get
				{
					return DefaultPerks.Instance._charmFlexibleEthics;
				}
			}

			// Token: 0x17001512 RID: 5394
			// (get) Token: 0x0600652B RID: 25899 RVA: 0x001CEC2A File Offset: 0x001CCE2A
			public static PerkObject EffortForThePeople
			{
				get
				{
					return DefaultPerks.Instance._charmEffortForThePeople;
				}
			}

			// Token: 0x17001513 RID: 5395
			// (get) Token: 0x0600652C RID: 25900 RVA: 0x001CEC36 File Offset: 0x001CCE36
			public static PerkObject SlickNegotiator
			{
				get
				{
					return DefaultPerks.Instance._charmSlickNegotiator;
				}
			}

			// Token: 0x17001514 RID: 5396
			// (get) Token: 0x0600652D RID: 25901 RVA: 0x001CEC42 File Offset: 0x001CCE42
			public static PerkObject GoodNatured
			{
				get
				{
					return DefaultPerks.Instance._charmGoodNatured;
				}
			}

			// Token: 0x17001515 RID: 5397
			// (get) Token: 0x0600652E RID: 25902 RVA: 0x001CEC4E File Offset: 0x001CCE4E
			public static PerkObject Tribute
			{
				get
				{
					return DefaultPerks.Instance._charmTribute;
				}
			}

			// Token: 0x17001516 RID: 5398
			// (get) Token: 0x0600652F RID: 25903 RVA: 0x001CEC5A File Offset: 0x001CCE5A
			public static PerkObject MoralLeader
			{
				get
				{
					return DefaultPerks.Instance._charmMoralLeader;
				}
			}

			// Token: 0x17001517 RID: 5399
			// (get) Token: 0x06006530 RID: 25904 RVA: 0x001CEC66 File Offset: 0x001CCE66
			public static PerkObject NaturalLeader
			{
				get
				{
					return DefaultPerks.Instance._charmNaturalLeader;
				}
			}

			// Token: 0x17001518 RID: 5400
			// (get) Token: 0x06006531 RID: 25905 RVA: 0x001CEC72 File Offset: 0x001CCE72
			public static PerkObject PublicSpeaker
			{
				get
				{
					return DefaultPerks.Instance._charmPublicSpeaker;
				}
			}

			// Token: 0x17001519 RID: 5401
			// (get) Token: 0x06006532 RID: 25906 RVA: 0x001CEC7E File Offset: 0x001CCE7E
			public static PerkObject Parade
			{
				get
				{
					return DefaultPerks.Instance._charmParade;
				}
			}

			// Token: 0x1700151A RID: 5402
			// (get) Token: 0x06006533 RID: 25907 RVA: 0x001CEC8A File Offset: 0x001CCE8A
			public static PerkObject Camaraderie
			{
				get
				{
					return DefaultPerks.Instance._charmCamaraderie;
				}
			}

			// Token: 0x1700151B RID: 5403
			// (get) Token: 0x06006534 RID: 25908 RVA: 0x001CEC96 File Offset: 0x001CCE96
			public static PerkObject ImmortalCharm
			{
				get
				{
					return DefaultPerks.Instance._charmImmortalCharm;
				}
			}
		}

		// Token: 0x020007B0 RID: 1968
		public static class Leadership
		{
			// Token: 0x1700151C RID: 5404
			// (get) Token: 0x06006535 RID: 25909 RVA: 0x001CECA2 File Offset: 0x001CCEA2
			public static PerkObject CombatTips
			{
				get
				{
					return DefaultPerks.Instance._leadershipCombatTips;
				}
			}

			// Token: 0x1700151D RID: 5405
			// (get) Token: 0x06006536 RID: 25910 RVA: 0x001CECAE File Offset: 0x001CCEAE
			public static PerkObject RaiseTheMeek
			{
				get
				{
					return DefaultPerks.Instance._leadershipRaiseTheMeek;
				}
			}

			// Token: 0x1700151E RID: 5406
			// (get) Token: 0x06006537 RID: 25911 RVA: 0x001CECBA File Offset: 0x001CCEBA
			public static PerkObject FerventAttacker
			{
				get
				{
					return DefaultPerks.Instance._leadershipFerventAttacker;
				}
			}

			// Token: 0x1700151F RID: 5407
			// (get) Token: 0x06006538 RID: 25912 RVA: 0x001CECC6 File Offset: 0x001CCEC6
			public static PerkObject StoutDefender
			{
				get
				{
					return DefaultPerks.Instance._leadershipStoutDefender;
				}
			}

			// Token: 0x17001520 RID: 5408
			// (get) Token: 0x06006539 RID: 25913 RVA: 0x001CECD2 File Offset: 0x001CCED2
			public static PerkObject Authority
			{
				get
				{
					return DefaultPerks.Instance._leadershipAuthority;
				}
			}

			// Token: 0x17001521 RID: 5409
			// (get) Token: 0x0600653A RID: 25914 RVA: 0x001CECDE File Offset: 0x001CCEDE
			public static PerkObject HeroicLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipHeroicLeader;
				}
			}

			// Token: 0x17001522 RID: 5410
			// (get) Token: 0x0600653B RID: 25915 RVA: 0x001CECEA File Offset: 0x001CCEEA
			public static PerkObject LoyaltyAndHonor
			{
				get
				{
					return DefaultPerks.Instance._leadershipLoyaltyAndHonor;
				}
			}

			// Token: 0x17001523 RID: 5411
			// (get) Token: 0x0600653C RID: 25916 RVA: 0x001CECF6 File Offset: 0x001CCEF6
			public static PerkObject Presence
			{
				get
				{
					return DefaultPerks.Instance._leadershipPresence;
				}
			}

			// Token: 0x17001524 RID: 5412
			// (get) Token: 0x0600653D RID: 25917 RVA: 0x001CED02 File Offset: 0x001CCF02
			public static PerkObject FamousCommander
			{
				get
				{
					return DefaultPerks.Instance._leadershipFamousCommander;
				}
			}

			// Token: 0x17001525 RID: 5413
			// (get) Token: 0x0600653E RID: 25918 RVA: 0x001CED0E File Offset: 0x001CCF0E
			public static PerkObject LeaderOfMasses
			{
				get
				{
					return DefaultPerks.Instance._leadershipLeaderOfTheMasses;
				}
			}

			// Token: 0x17001526 RID: 5414
			// (get) Token: 0x0600653F RID: 25919 RVA: 0x001CED1A File Offset: 0x001CCF1A
			public static PerkObject VeteransRespect
			{
				get
				{
					return DefaultPerks.Instance._leadershipVeteransRespect;
				}
			}

			// Token: 0x17001527 RID: 5415
			// (get) Token: 0x06006540 RID: 25920 RVA: 0x001CED26 File Offset: 0x001CCF26
			public static PerkObject CitizenMilitia
			{
				get
				{
					return DefaultPerks.Instance._leadershipCitizenMilitia;
				}
			}

			// Token: 0x17001528 RID: 5416
			// (get) Token: 0x06006541 RID: 25921 RVA: 0x001CED32 File Offset: 0x001CCF32
			public static PerkObject InspiringLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipInspiringLeader;
				}
			}

			// Token: 0x17001529 RID: 5417
			// (get) Token: 0x06006542 RID: 25922 RVA: 0x001CED3E File Offset: 0x001CCF3E
			public static PerkObject UpliftingSpirit
			{
				get
				{
					return DefaultPerks.Instance._leadershipUpliftingSpirit;
				}
			}

			// Token: 0x1700152A RID: 5418
			// (get) Token: 0x06006543 RID: 25923 RVA: 0x001CED4A File Offset: 0x001CCF4A
			public static PerkObject MakeADifference
			{
				get
				{
					return DefaultPerks.Instance._leadershipMakeADifference;
				}
			}

			// Token: 0x1700152B RID: 5419
			// (get) Token: 0x06006544 RID: 25924 RVA: 0x001CED56 File Offset: 0x001CCF56
			public static PerkObject LeadByExample
			{
				get
				{
					return DefaultPerks.Instance._leadershipLeadByExample;
				}
			}

			// Token: 0x1700152C RID: 5420
			// (get) Token: 0x06006545 RID: 25925 RVA: 0x001CED62 File Offset: 0x001CCF62
			public static PerkObject TrustedCommander
			{
				get
				{
					return DefaultPerks.Instance._leadershipTrustedCommander;
				}
			}

			// Token: 0x1700152D RID: 5421
			// (get) Token: 0x06006546 RID: 25926 RVA: 0x001CED6E File Offset: 0x001CCF6E
			public static PerkObject GreatLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipGreatLeader;
				}
			}

			// Token: 0x1700152E RID: 5422
			// (get) Token: 0x06006547 RID: 25927 RVA: 0x001CED7A File Offset: 0x001CCF7A
			public static PerkObject WePledgeOurSwords
			{
				get
				{
					return DefaultPerks.Instance._leadershipWePledgeOurSwords;
				}
			}

			// Token: 0x1700152F RID: 5423
			// (get) Token: 0x06006548 RID: 25928 RVA: 0x001CED86 File Offset: 0x001CCF86
			public static PerkObject TalentMagnet
			{
				get
				{
					return DefaultPerks.Instance._leadershipTalentMagnet;
				}
			}

			// Token: 0x17001530 RID: 5424
			// (get) Token: 0x06006549 RID: 25929 RVA: 0x001CED92 File Offset: 0x001CCF92
			public static PerkObject UltimateLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipUltimateLeader;
				}
			}
		}

		// Token: 0x020007B1 RID: 1969
		public static class Trade
		{
			// Token: 0x17001531 RID: 5425
			// (get) Token: 0x0600654A RID: 25930 RVA: 0x001CED9E File Offset: 0x001CCF9E
			public static PerkObject Appraiser
			{
				get
				{
					return DefaultPerks.Instance._tradeAppraiser;
				}
			}

			// Token: 0x17001532 RID: 5426
			// (get) Token: 0x0600654B RID: 25931 RVA: 0x001CEDAA File Offset: 0x001CCFAA
			public static PerkObject WholeSeller
			{
				get
				{
					return DefaultPerks.Instance._tradeWholeSeller;
				}
			}

			// Token: 0x17001533 RID: 5427
			// (get) Token: 0x0600654C RID: 25932 RVA: 0x001CEDB6 File Offset: 0x001CCFB6
			public static PerkObject CaravanMaster
			{
				get
				{
					return DefaultPerks.Instance._tradeCaravanMaster;
				}
			}

			// Token: 0x17001534 RID: 5428
			// (get) Token: 0x0600654D RID: 25933 RVA: 0x001CEDC2 File Offset: 0x001CCFC2
			public static PerkObject MarketDealer
			{
				get
				{
					return DefaultPerks.Instance._tradeMarketDealer;
				}
			}

			// Token: 0x17001535 RID: 5429
			// (get) Token: 0x0600654E RID: 25934 RVA: 0x001CEDCE File Offset: 0x001CCFCE
			public static PerkObject TravelingRumors
			{
				get
				{
					return DefaultPerks.Instance._tradeTravelingRumors;
				}
			}

			// Token: 0x17001536 RID: 5430
			// (get) Token: 0x0600654F RID: 25935 RVA: 0x001CEDDA File Offset: 0x001CCFDA
			public static PerkObject LocalConnection
			{
				get
				{
					return DefaultPerks.Instance._tradeLocalConnection;
				}
			}

			// Token: 0x17001537 RID: 5431
			// (get) Token: 0x06006550 RID: 25936 RVA: 0x001CEDE6 File Offset: 0x001CCFE6
			public static PerkObject DistributedGoods
			{
				get
				{
					return DefaultPerks.Instance._tradeDistributedGoods;
				}
			}

			// Token: 0x17001538 RID: 5432
			// (get) Token: 0x06006551 RID: 25937 RVA: 0x001CEDF2 File Offset: 0x001CCFF2
			public static PerkObject Tollgates
			{
				get
				{
					return DefaultPerks.Instance._tradeTollgates;
				}
			}

			// Token: 0x17001539 RID: 5433
			// (get) Token: 0x06006552 RID: 25938 RVA: 0x001CEDFE File Offset: 0x001CCFFE
			public static PerkObject ArtisanCommunity
			{
				get
				{
					return DefaultPerks.Instance._tradeArtisanCommunity;
				}
			}

			// Token: 0x1700153A RID: 5434
			// (get) Token: 0x06006553 RID: 25939 RVA: 0x001CEE0A File Offset: 0x001CD00A
			public static PerkObject GreatInvestor
			{
				get
				{
					return DefaultPerks.Instance._tradeGreatInvestor;
				}
			}

			// Token: 0x1700153B RID: 5435
			// (get) Token: 0x06006554 RID: 25940 RVA: 0x001CEE16 File Offset: 0x001CD016
			public static PerkObject MercenaryConnections
			{
				get
				{
					return DefaultPerks.Instance._tradeMercenaryConnections;
				}
			}

			// Token: 0x1700153C RID: 5436
			// (get) Token: 0x06006555 RID: 25941 RVA: 0x001CEE22 File Offset: 0x001CD022
			public static PerkObject ContentTrades
			{
				get
				{
					return DefaultPerks.Instance._tradeContentTrades;
				}
			}

			// Token: 0x1700153D RID: 5437
			// (get) Token: 0x06006556 RID: 25942 RVA: 0x001CEE2E File Offset: 0x001CD02E
			public static PerkObject InsurancePlans
			{
				get
				{
					return DefaultPerks.Instance._tradeInsurancePlans;
				}
			}

			// Token: 0x1700153E RID: 5438
			// (get) Token: 0x06006557 RID: 25943 RVA: 0x001CEE3A File Offset: 0x001CD03A
			public static PerkObject RapidDevelopment
			{
				get
				{
					return DefaultPerks.Instance._tradeRapidDevelopment;
				}
			}

			// Token: 0x1700153F RID: 5439
			// (get) Token: 0x06006558 RID: 25944 RVA: 0x001CEE46 File Offset: 0x001CD046
			public static PerkObject GranaryAccountant
			{
				get
				{
					return DefaultPerks.Instance._tradeGranaryAccountant;
				}
			}

			// Token: 0x17001540 RID: 5440
			// (get) Token: 0x06006559 RID: 25945 RVA: 0x001CEE52 File Offset: 0x001CD052
			public static PerkObject TradeyardForeman
			{
				get
				{
					return DefaultPerks.Instance._tradeTradeyardForeman;
				}
			}

			// Token: 0x17001541 RID: 5441
			// (get) Token: 0x0600655A RID: 25946 RVA: 0x001CEE5E File Offset: 0x001CD05E
			public static PerkObject SwordForBarter
			{
				get
				{
					return DefaultPerks.Instance._tradeSwordForBarter;
				}
			}

			// Token: 0x17001542 RID: 5442
			// (get) Token: 0x0600655B RID: 25947 RVA: 0x001CEE6A File Offset: 0x001CD06A
			public static PerkObject SelfMadeMan
			{
				get
				{
					return DefaultPerks.Instance._tradeSelfMadeMan;
				}
			}

			// Token: 0x17001543 RID: 5443
			// (get) Token: 0x0600655C RID: 25948 RVA: 0x001CEE76 File Offset: 0x001CD076
			public static PerkObject SilverTongue
			{
				get
				{
					return DefaultPerks.Instance._tradeSilverTongue;
				}
			}

			// Token: 0x17001544 RID: 5444
			// (get) Token: 0x0600655D RID: 25949 RVA: 0x001CEE82 File Offset: 0x001CD082
			public static PerkObject SpringOfGold
			{
				get
				{
					return DefaultPerks.Instance._tradeSpringOfGold;
				}
			}

			// Token: 0x17001545 RID: 5445
			// (get) Token: 0x0600655E RID: 25950 RVA: 0x001CEE8E File Offset: 0x001CD08E
			public static PerkObject ManOfMeans
			{
				get
				{
					return DefaultPerks.Instance._tradeManOfMeans;
				}
			}

			// Token: 0x17001546 RID: 5446
			// (get) Token: 0x0600655F RID: 25951 RVA: 0x001CEE9A File Offset: 0x001CD09A
			public static PerkObject TrickleDown
			{
				get
				{
					return DefaultPerks.Instance._tradeTrickleDown;
				}
			}

			// Token: 0x17001547 RID: 5447
			// (get) Token: 0x06006560 RID: 25952 RVA: 0x001CEEA6 File Offset: 0x001CD0A6
			public static PerkObject EverythingHasAPrice
			{
				get
				{
					return DefaultPerks.Instance._tradeEverythingHasAPrice;
				}
			}
		}

		// Token: 0x020007B2 RID: 1970
		public static class Steward
		{
			// Token: 0x17001548 RID: 5448
			// (get) Token: 0x06006561 RID: 25953 RVA: 0x001CEEB2 File Offset: 0x001CD0B2
			public static PerkObject WarriorsDiet
			{
				get
				{
					return DefaultPerks.Instance._stewardWarriorsDiet;
				}
			}

			// Token: 0x17001549 RID: 5449
			// (get) Token: 0x06006562 RID: 25954 RVA: 0x001CEEBE File Offset: 0x001CD0BE
			public static PerkObject Frugal
			{
				get
				{
					return DefaultPerks.Instance._stewardFrugal;
				}
			}

			// Token: 0x1700154A RID: 5450
			// (get) Token: 0x06006563 RID: 25955 RVA: 0x001CEECA File Offset: 0x001CD0CA
			public static PerkObject SevenVeterans
			{
				get
				{
					return DefaultPerks.Instance._stewardSevenVeterans;
				}
			}

			// Token: 0x1700154B RID: 5451
			// (get) Token: 0x06006564 RID: 25956 RVA: 0x001CEED6 File Offset: 0x001CD0D6
			public static PerkObject DrillSergant
			{
				get
				{
					return DefaultPerks.Instance._stewardDrillSergant;
				}
			}

			// Token: 0x1700154C RID: 5452
			// (get) Token: 0x06006565 RID: 25957 RVA: 0x001CEEE2 File Offset: 0x001CD0E2
			public static PerkObject Sweatshops
			{
				get
				{
					return DefaultPerks.Instance._stewardSweatshops;
				}
			}

			// Token: 0x1700154D RID: 5453
			// (get) Token: 0x06006566 RID: 25958 RVA: 0x001CEEEE File Offset: 0x001CD0EE
			public static PerkObject StiffUpperLip
			{
				get
				{
					return DefaultPerks.Instance._stewardStiffUpperLip;
				}
			}

			// Token: 0x1700154E RID: 5454
			// (get) Token: 0x06006567 RID: 25959 RVA: 0x001CEEFA File Offset: 0x001CD0FA
			public static PerkObject PaidInPromise
			{
				get
				{
					return DefaultPerks.Instance._stewardPaidInPromise;
				}
			}

			// Token: 0x1700154F RID: 5455
			// (get) Token: 0x06006568 RID: 25960 RVA: 0x001CEF06 File Offset: 0x001CD106
			public static PerkObject EfficientCampaigner
			{
				get
				{
					return DefaultPerks.Instance._stewardEfficientCampaigner;
				}
			}

			// Token: 0x17001550 RID: 5456
			// (get) Token: 0x06006569 RID: 25961 RVA: 0x001CEF12 File Offset: 0x001CD112
			public static PerkObject GivingHands
			{
				get
				{
					return DefaultPerks.Instance._stewardGivingHands;
				}
			}

			// Token: 0x17001551 RID: 5457
			// (get) Token: 0x0600656A RID: 25962 RVA: 0x001CEF1E File Offset: 0x001CD11E
			public static PerkObject Logistician
			{
				get
				{
					return DefaultPerks.Instance._stewardLogistician;
				}
			}

			// Token: 0x17001552 RID: 5458
			// (get) Token: 0x0600656B RID: 25963 RVA: 0x001CEF2A File Offset: 0x001CD12A
			public static PerkObject Relocation
			{
				get
				{
					return DefaultPerks.Instance._stewardRelocation;
				}
			}

			// Token: 0x17001553 RID: 5459
			// (get) Token: 0x0600656C RID: 25964 RVA: 0x001CEF36 File Offset: 0x001CD136
			public static PerkObject AidCorps
			{
				get
				{
					return DefaultPerks.Instance._stewardAidCorps;
				}
			}

			// Token: 0x17001554 RID: 5460
			// (get) Token: 0x0600656D RID: 25965 RVA: 0x001CEF42 File Offset: 0x001CD142
			public static PerkObject Gourmet
			{
				get
				{
					return DefaultPerks.Instance._stewardGourmet;
				}
			}

			// Token: 0x17001555 RID: 5461
			// (get) Token: 0x0600656E RID: 25966 RVA: 0x001CEF4E File Offset: 0x001CD14E
			public static PerkObject SoundReserves
			{
				get
				{
					return DefaultPerks.Instance._stewardSoundReserves;
				}
			}

			// Token: 0x17001556 RID: 5462
			// (get) Token: 0x0600656F RID: 25967 RVA: 0x001CEF5A File Offset: 0x001CD15A
			public static PerkObject ForcedLabor
			{
				get
				{
					return DefaultPerks.Instance._stewardForcedLabor;
				}
			}

			// Token: 0x17001557 RID: 5463
			// (get) Token: 0x06006570 RID: 25968 RVA: 0x001CEF66 File Offset: 0x001CD166
			public static PerkObject Contractors
			{
				get
				{
					return DefaultPerks.Instance._stewardContractors;
				}
			}

			// Token: 0x17001558 RID: 5464
			// (get) Token: 0x06006571 RID: 25969 RVA: 0x001CEF72 File Offset: 0x001CD172
			public static PerkObject ArenicosMules
			{
				get
				{
					return DefaultPerks.Instance._stewardArenicosMules;
				}
			}

			// Token: 0x17001559 RID: 5465
			// (get) Token: 0x06006572 RID: 25970 RVA: 0x001CEF7E File Offset: 0x001CD17E
			public static PerkObject ArenicosHorses
			{
				get
				{
					return DefaultPerks.Instance._stewardArenicosHorses;
				}
			}

			// Token: 0x1700155A RID: 5466
			// (get) Token: 0x06006573 RID: 25971 RVA: 0x001CEF8A File Offset: 0x001CD18A
			public static PerkObject MasterOfPlanning
			{
				get
				{
					return DefaultPerks.Instance._stewardMasterOfPlanning;
				}
			}

			// Token: 0x1700155B RID: 5467
			// (get) Token: 0x06006574 RID: 25972 RVA: 0x001CEF96 File Offset: 0x001CD196
			public static PerkObject MasterOfWarcraft
			{
				get
				{
					return DefaultPerks.Instance._stewardMasterOfWarcraft;
				}
			}

			// Token: 0x1700155C RID: 5468
			// (get) Token: 0x06006575 RID: 25973 RVA: 0x001CEFA2 File Offset: 0x001CD1A2
			public static PerkObject PriceOfLoyalty
			{
				get
				{
					return DefaultPerks.Instance._stewardPriceOfLoyalty;
				}
			}
		}

		// Token: 0x020007B3 RID: 1971
		public static class Medicine
		{
			// Token: 0x1700155D RID: 5469
			// (get) Token: 0x06006576 RID: 25974 RVA: 0x001CEFAE File Offset: 0x001CD1AE
			public static PerkObject SelfMedication
			{
				get
				{
					return DefaultPerks.Instance._medicineSelfMedication;
				}
			}

			// Token: 0x1700155E RID: 5470
			// (get) Token: 0x06006577 RID: 25975 RVA: 0x001CEFBA File Offset: 0x001CD1BA
			public static PerkObject PreventiveMedicine
			{
				get
				{
					return DefaultPerks.Instance._medicinePreventiveMedicine;
				}
			}

			// Token: 0x1700155F RID: 5471
			// (get) Token: 0x06006578 RID: 25976 RVA: 0x001CEFC6 File Offset: 0x001CD1C6
			public static PerkObject TriageTent
			{
				get
				{
					return DefaultPerks.Instance._medicineTriageTent;
				}
			}

			// Token: 0x17001560 RID: 5472
			// (get) Token: 0x06006579 RID: 25977 RVA: 0x001CEFD2 File Offset: 0x001CD1D2
			public static PerkObject WalkItOff
			{
				get
				{
					return DefaultPerks.Instance._medicineWalkItOff;
				}
			}

			// Token: 0x17001561 RID: 5473
			// (get) Token: 0x0600657A RID: 25978 RVA: 0x001CEFDE File Offset: 0x001CD1DE
			public static PerkObject Sledges
			{
				get
				{
					return DefaultPerks.Instance._medicineSledges;
				}
			}

			// Token: 0x17001562 RID: 5474
			// (get) Token: 0x0600657B RID: 25979 RVA: 0x001CEFEA File Offset: 0x001CD1EA
			public static PerkObject DoctorsOath
			{
				get
				{
					return DefaultPerks.Instance._medicineDoctorsOath;
				}
			}

			// Token: 0x17001563 RID: 5475
			// (get) Token: 0x0600657C RID: 25980 RVA: 0x001CEFF6 File Offset: 0x001CD1F6
			public static PerkObject BestMedicine
			{
				get
				{
					return DefaultPerks.Instance._medicineBestMedicine;
				}
			}

			// Token: 0x17001564 RID: 5476
			// (get) Token: 0x0600657D RID: 25981 RVA: 0x001CF002 File Offset: 0x001CD202
			public static PerkObject GoodLogdings
			{
				get
				{
					return DefaultPerks.Instance._medicineGoodLodging;
				}
			}

			// Token: 0x17001565 RID: 5477
			// (get) Token: 0x0600657E RID: 25982 RVA: 0x001CF00E File Offset: 0x001CD20E
			public static PerkObject SiegeMedic
			{
				get
				{
					return DefaultPerks.Instance._medicineSiegeMedic;
				}
			}

			// Token: 0x17001566 RID: 5478
			// (get) Token: 0x0600657F RID: 25983 RVA: 0x001CF01A File Offset: 0x001CD21A
			public static PerkObject Veterinarian
			{
				get
				{
					return DefaultPerks.Instance._medicineVeterinarian;
				}
			}

			// Token: 0x17001567 RID: 5479
			// (get) Token: 0x06006580 RID: 25984 RVA: 0x001CF026 File Offset: 0x001CD226
			public static PerkObject PristineStreets
			{
				get
				{
					return DefaultPerks.Instance._medicinePristineStreets;
				}
			}

			// Token: 0x17001568 RID: 5480
			// (get) Token: 0x06006581 RID: 25985 RVA: 0x001CF032 File Offset: 0x001CD232
			public static PerkObject BushDoctor
			{
				get
				{
					return DefaultPerks.Instance._medicineBushDoctor;
				}
			}

			// Token: 0x17001569 RID: 5481
			// (get) Token: 0x06006582 RID: 25986 RVA: 0x001CF03E File Offset: 0x001CD23E
			public static PerkObject PerfectHealth
			{
				get
				{
					return DefaultPerks.Instance._medicinePerfectHealth;
				}
			}

			// Token: 0x1700156A RID: 5482
			// (get) Token: 0x06006583 RID: 25987 RVA: 0x001CF04A File Offset: 0x001CD24A
			public static PerkObject HealthAdvise
			{
				get
				{
					return DefaultPerks.Instance._medicineHealthAdvise;
				}
			}

			// Token: 0x1700156B RID: 5483
			// (get) Token: 0x06006584 RID: 25988 RVA: 0x001CF056 File Offset: 0x001CD256
			public static PerkObject PhysicianOfPeople
			{
				get
				{
					return DefaultPerks.Instance._medicinePhysicianOfPeople;
				}
			}

			// Token: 0x1700156C RID: 5484
			// (get) Token: 0x06006585 RID: 25989 RVA: 0x001CF062 File Offset: 0x001CD262
			public static PerkObject CleanInfrastructure
			{
				get
				{
					return DefaultPerks.Instance._medicineCleanInfrastructure;
				}
			}

			// Token: 0x1700156D RID: 5485
			// (get) Token: 0x06006586 RID: 25990 RVA: 0x001CF06E File Offset: 0x001CD26E
			public static PerkObject CheatDeath
			{
				get
				{
					return DefaultPerks.Instance._medicineCheatDeath;
				}
			}

			// Token: 0x1700156E RID: 5486
			// (get) Token: 0x06006587 RID: 25991 RVA: 0x001CF07A File Offset: 0x001CD27A
			public static PerkObject FortitudeTonic
			{
				get
				{
					return DefaultPerks.Instance._medicineFortitudeTonic;
				}
			}

			// Token: 0x1700156F RID: 5487
			// (get) Token: 0x06006588 RID: 25992 RVA: 0x001CF086 File Offset: 0x001CD286
			public static PerkObject HelpingHands
			{
				get
				{
					return DefaultPerks.Instance._medicineHelpingHands;
				}
			}

			// Token: 0x17001570 RID: 5488
			// (get) Token: 0x06006589 RID: 25993 RVA: 0x001CF092 File Offset: 0x001CD292
			public static PerkObject BattleHardened
			{
				get
				{
					return DefaultPerks.Instance._medicineBattleHardened;
				}
			}

			// Token: 0x17001571 RID: 5489
			// (get) Token: 0x0600658A RID: 25994 RVA: 0x001CF09E File Offset: 0x001CD29E
			public static PerkObject MinisterOfHealth
			{
				get
				{
					return DefaultPerks.Instance._medicineMinisterOfHealth;
				}
			}
		}

		// Token: 0x020007B4 RID: 1972
		public static class Engineering
		{
			// Token: 0x17001572 RID: 5490
			// (get) Token: 0x0600658B RID: 25995 RVA: 0x001CF0AA File Offset: 0x001CD2AA
			public static PerkObject Scaffolds
			{
				get
				{
					return DefaultPerks.Instance._engineeringScaffolds;
				}
			}

			// Token: 0x17001573 RID: 5491
			// (get) Token: 0x0600658C RID: 25996 RVA: 0x001CF0B6 File Offset: 0x001CD2B6
			public static PerkObject TorsionEngines
			{
				get
				{
					return DefaultPerks.Instance._engineeringTorsionEngines;
				}
			}

			// Token: 0x17001574 RID: 5492
			// (get) Token: 0x0600658D RID: 25997 RVA: 0x001CF0C2 File Offset: 0x001CD2C2
			public static PerkObject SiegeWorks
			{
				get
				{
					return DefaultPerks.Instance._engineeringSiegeWorks;
				}
			}

			// Token: 0x17001575 RID: 5493
			// (get) Token: 0x0600658E RID: 25998 RVA: 0x001CF0CE File Offset: 0x001CD2CE
			public static PerkObject DungeonArchitect
			{
				get
				{
					return DefaultPerks.Instance._engineeringDungeonArchitect;
				}
			}

			// Token: 0x17001576 RID: 5494
			// (get) Token: 0x0600658F RID: 25999 RVA: 0x001CF0DA File Offset: 0x001CD2DA
			public static PerkObject Carpenters
			{
				get
				{
					return DefaultPerks.Instance._engineeringCarpenters;
				}
			}

			// Token: 0x17001577 RID: 5495
			// (get) Token: 0x06006590 RID: 26000 RVA: 0x001CF0E6 File Offset: 0x001CD2E6
			public static PerkObject MilitaryPlanner
			{
				get
				{
					return DefaultPerks.Instance._engineeringMilitaryPlanner;
				}
			}

			// Token: 0x17001578 RID: 5496
			// (get) Token: 0x06006591 RID: 26001 RVA: 0x001CF0F2 File Offset: 0x001CD2F2
			public static PerkObject WallBreaker
			{
				get
				{
					return DefaultPerks.Instance._engineeringWallBreaker;
				}
			}

			// Token: 0x17001579 RID: 5497
			// (get) Token: 0x06006592 RID: 26002 RVA: 0x001CF0FE File Offset: 0x001CD2FE
			public static PerkObject DreadfulSieger
			{
				get
				{
					return DefaultPerks.Instance._engineeringDreadfulSieger;
				}
			}

			// Token: 0x1700157A RID: 5498
			// (get) Token: 0x06006593 RID: 26003 RVA: 0x001CF10A File Offset: 0x001CD30A
			public static PerkObject Salvager
			{
				get
				{
					return DefaultPerks.Instance._engineeringSalvager;
				}
			}

			// Token: 0x1700157B RID: 5499
			// (get) Token: 0x06006594 RID: 26004 RVA: 0x001CF116 File Offset: 0x001CD316
			public static PerkObject Foreman
			{
				get
				{
					return DefaultPerks.Instance._engineeringForeman;
				}
			}

			// Token: 0x1700157C RID: 5500
			// (get) Token: 0x06006595 RID: 26005 RVA: 0x001CF122 File Offset: 0x001CD322
			public static PerkObject Stonecutters
			{
				get
				{
					return DefaultPerks.Instance._engineeringStonecutters;
				}
			}

			// Token: 0x1700157D RID: 5501
			// (get) Token: 0x06006596 RID: 26006 RVA: 0x001CF12E File Offset: 0x001CD32E
			public static PerkObject SiegeEngineer
			{
				get
				{
					return DefaultPerks.Instance._engineeringSiegeEngineer;
				}
			}

			// Token: 0x1700157E RID: 5502
			// (get) Token: 0x06006597 RID: 26007 RVA: 0x001CF13A File Offset: 0x001CD33A
			public static PerkObject CampBuilding
			{
				get
				{
					return DefaultPerks.Instance._engineeringCampBuilding;
				}
			}

			// Token: 0x1700157F RID: 5503
			// (get) Token: 0x06006598 RID: 26008 RVA: 0x001CF146 File Offset: 0x001CD346
			public static PerkObject Battlements
			{
				get
				{
					return DefaultPerks.Instance._engineeringBattlements;
				}
			}

			// Token: 0x17001580 RID: 5504
			// (get) Token: 0x06006599 RID: 26009 RVA: 0x001CF152 File Offset: 0x001CD352
			public static PerkObject EngineeringGuilds
			{
				get
				{
					return DefaultPerks.Instance._engineeringEngineeringGuilds;
				}
			}

			// Token: 0x17001581 RID: 5505
			// (get) Token: 0x0600659A RID: 26010 RVA: 0x001CF15E File Offset: 0x001CD35E
			public static PerkObject Apprenticeship
			{
				get
				{
					return DefaultPerks.Instance._engineeringApprenticeship;
				}
			}

			// Token: 0x17001582 RID: 5506
			// (get) Token: 0x0600659B RID: 26011 RVA: 0x001CF16A File Offset: 0x001CD36A
			public static PerkObject Metallurgy
			{
				get
				{
					return DefaultPerks.Instance._engineeringMetallurgy;
				}
			}

			// Token: 0x17001583 RID: 5507
			// (get) Token: 0x0600659C RID: 26012 RVA: 0x001CF176 File Offset: 0x001CD376
			public static PerkObject ImprovedTools
			{
				get
				{
					return DefaultPerks.Instance._engineeringImprovedTools;
				}
			}

			// Token: 0x17001584 RID: 5508
			// (get) Token: 0x0600659D RID: 26013 RVA: 0x001CF182 File Offset: 0x001CD382
			public static PerkObject Clockwork
			{
				get
				{
					return DefaultPerks.Instance._engineeringClockwork;
				}
			}

			// Token: 0x17001585 RID: 5509
			// (get) Token: 0x0600659E RID: 26014 RVA: 0x001CF18E File Offset: 0x001CD38E
			public static PerkObject ArchitecturalCommisions
			{
				get
				{
					return DefaultPerks.Instance._engineeringArchitecturalCommisions;
				}
			}

			// Token: 0x17001586 RID: 5510
			// (get) Token: 0x0600659F RID: 26015 RVA: 0x001CF19A File Offset: 0x001CD39A
			public static PerkObject Masterwork
			{
				get
				{
					return DefaultPerks.Instance._engineeringMasterwork;
				}
			}
		}
	}
}
