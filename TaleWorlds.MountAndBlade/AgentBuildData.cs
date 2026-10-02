using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000100 RID: 256
	public class AgentBuildData
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00016F1C File Offset: 0x0001511C
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x00016F24 File Offset: 0x00015124
		public AgentData AgentData { get; private set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00016F2D File Offset: 0x0001512D
		public BasicCharacterObject AgentCharacter
		{
			get
			{
				return this.AgentData.AgentCharacter;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00016F3A File Offset: 0x0001513A
		public Monster AgentMonster
		{
			get
			{
				return this.AgentData.AgentMonster;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00016F47 File Offset: 0x00015147
		public Equipment AgentOverridenSpawnEquipment
		{
			get
			{
				return this.AgentData.AgentOverridenEquipment;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00016F54 File Offset: 0x00015154
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00016F5C File Offset: 0x0001515C
		public MissionEquipment AgentOverridenSpawnMissionEquipment { get; private set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00016F65 File Offset: 0x00015165
		public int AgentEquipmentSeed
		{
			get
			{
				return this.AgentData.AgentEquipmentSeed;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00016F72 File Offset: 0x00015172
		public bool AgentNoHorses
		{
			get
			{
				return this.AgentData.AgentNoHorses;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00016F7F File Offset: 0x0001517F
		public string AgentMountKey
		{
			get
			{
				return this.AgentData.AgentMountKey;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00016F8C File Offset: 0x0001518C
		public bool AgentNoWeapons
		{
			get
			{
				return this.AgentData.AgentNoWeapons;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00016F99 File Offset: 0x00015199
		public bool AgentNoArmor
		{
			get
			{
				return this.AgentData.AgentNoArmor;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00016FA6 File Offset: 0x000151A6
		public bool AgentFixedEquipment
		{
			get
			{
				return this.AgentData.AgentFixedEquipment;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00016FB3 File Offset: 0x000151B3
		public bool AgentCivilianEquipment
		{
			get
			{
				return this.AgentData.AgentCivilianEquipment;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00016FC0 File Offset: 0x000151C0
		public uint AgentClothingColor1
		{
			get
			{
				return this.AgentData.AgentClothingColor1;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00016FCD File Offset: 0x000151CD
		public uint AgentClothingColor2
		{
			get
			{
				return this.AgentData.AgentClothingColor2;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00016FDA File Offset: 0x000151DA
		public bool BodyPropertiesOverriden
		{
			get
			{
				return this.AgentData.BodyPropertiesOverriden;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x00016FE7 File Offset: 0x000151E7
		public BodyProperties AgentBodyProperties
		{
			get
			{
				return this.AgentData.AgentBodyProperties;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00016FF4 File Offset: 0x000151F4
		public bool AgeOverriden
		{
			get
			{
				return this.AgentData.AgeOverriden;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00017001 File Offset: 0x00015201
		public int AgentAge
		{
			get
			{
				return this.AgentData.AgentAge;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0001700E File Offset: 0x0001520E
		public bool PrepareImmediately
		{
			get
			{
				return this.AgentData.PrepareImmediately;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x0001701B File Offset: 0x0001521B
		public bool GenderOverriden
		{
			get
			{
				return this.AgentData.GenderOverriden;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00017028 File Offset: 0x00015228
		public bool AgentIsFemale
		{
			get
			{
				return this.AgentData.AgentIsFemale;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00017035 File Offset: 0x00015235
		public int AgentRace
		{
			get
			{
				return this.AgentData.AgentRace;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00017042 File Offset: 0x00015242
		public IAgentOriginBase AgentOrigin
		{
			get
			{
				return this.AgentData.AgentOrigin;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x0001704F File Offset: 0x0001524F
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x00017057 File Offset: 0x00015257
		public AgentControllerType AgentController { get; private set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00017060 File Offset: 0x00015260
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00017068 File Offset: 0x00015268
		public Team AgentTeam { get; private set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00017071 File Offset: 0x00015271
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x00017079 File Offset: 0x00015279
		public bool AgentIsReinforcement { get; private set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00017082 File Offset: 0x00015282
		// (set) Token: 0x06000C00 RID: 3072 RVA: 0x0001708A File Offset: 0x0001528A
		public bool AgentSpawnsIntoOwnFormation { get; private set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00017093 File Offset: 0x00015293
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x0001709B File Offset: 0x0001529B
		public bool AgentSpawnsUsingOwnTroopClass { get; private set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x000170A4 File Offset: 0x000152A4
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x000170AC File Offset: 0x000152AC
		public float MakeUnitStandOutDistance { get; private set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x000170B5 File Offset: 0x000152B5
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x000170BD File Offset: 0x000152BD
		public Vec3? AgentInitialPosition { get; private set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x000170C6 File Offset: 0x000152C6
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x000170CE File Offset: 0x000152CE
		public Vec2? AgentInitialDirection { get; private set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000170D7 File Offset: 0x000152D7
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x000170DF File Offset: 0x000152DF
		public Formation AgentFormation { get; private set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x000170E8 File Offset: 0x000152E8
		// (set) Token: 0x06000C0C RID: 3084 RVA: 0x000170F0 File Offset: 0x000152F0
		public int AgentFormationTroopSpawnCount { get; private set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x000170F9 File Offset: 0x000152F9
		// (set) Token: 0x06000C0E RID: 3086 RVA: 0x00017101 File Offset: 0x00015301
		public int AgentFormationTroopSpawnIndex { get; private set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000C0F RID: 3087 RVA: 0x0001710A File Offset: 0x0001530A
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x00017112 File Offset: 0x00015312
		public MissionPeer AgentMissionPeer { get; private set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0001711B File Offset: 0x0001531B
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00017123 File Offset: 0x00015323
		public MissionPeer OwningAgentMissionPeer { get; private set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0001712C File Offset: 0x0001532C
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00017134 File Offset: 0x00015334
		public bool AgentIndexOverriden { get; private set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0001713D File Offset: 0x0001533D
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x00017145 File Offset: 0x00015345
		public int AgentIndex { get; private set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0001714E File Offset: 0x0001534E
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x00017156 File Offset: 0x00015356
		public bool AgentMountIndexOverriden { get; private set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x0001715F File Offset: 0x0001535F
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x00017167 File Offset: 0x00015367
		public int AgentMountIndex { get; private set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x00017170 File Offset: 0x00015370
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x00017178 File Offset: 0x00015378
		public int AgentVisualsIndex { get; private set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00017181 File Offset: 0x00015381
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x00017189 File Offset: 0x00015389
		public Banner AgentBanner { get; private set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00017192 File Offset: 0x00015392
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x0001719A File Offset: 0x0001539A
		public ItemObject AgentBannerItem { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x000171A3 File Offset: 0x000153A3
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x000171AB File Offset: 0x000153AB
		public ItemObject AgentBannerReplacementWeaponItem { get; private set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x000171B4 File Offset: 0x000153B4
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x000171BC File Offset: 0x000153BC
		public bool AgentCanSpawnOutsideOfMissionBoundary { get; private set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x000171C5 File Offset: 0x000153C5
		public bool RandomizeColors
		{
			get
			{
				return this.AgentCharacter != null && !this.AgentCharacter.IsHero && this.AgentMissionPeer == null;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x000171E7 File Offset: 0x000153E7
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x000171EF File Offset: 0x000153EF
		public bool UseFaceCache { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x000171F8 File Offset: 0x000153F8
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x00017200 File Offset: 0x00015400
		public int FaceCacheId { get; set; }

		// Token: 0x06000C2A RID: 3114 RVA: 0x00017209 File Offset: 0x00015409
		private AgentBuildData()
		{
			this.AgentController = AgentControllerType.AI;
			this.AgentTeam = TaleWorlds.MountAndBlade.Team.Invalid;
			this.AgentFormation = null;
			this.AgentMissionPeer = null;
			this.AgentFormationTroopSpawnIndex = -1;
			this.UseFaceCache = false;
			this.FaceCacheId = 0;
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00017246 File Offset: 0x00015446
		public AgentBuildData(AgentData agentData)
			: this()
		{
			this.AgentData = agentData;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00017255 File Offset: 0x00015455
		public AgentBuildData(IAgentOriginBase agentOrigin)
			: this()
		{
			this.AgentData = new AgentData(agentOrigin);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00017269 File Offset: 0x00015469
		public AgentBuildData(BasicCharacterObject characterObject)
			: this()
		{
			this.AgentData = new AgentData(characterObject);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0001727D File Offset: 0x0001547D
		public AgentBuildData Character(BasicCharacterObject characterObject)
		{
			this.AgentData.Character(characterObject);
			return this;
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0001728D File Offset: 0x0001548D
		public AgentBuildData Controller(AgentControllerType controller)
		{
			this.AgentController = controller;
			return this;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00017297 File Offset: 0x00015497
		public AgentBuildData Team(Team team)
		{
			this.AgentTeam = team;
			return this;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000172A1 File Offset: 0x000154A1
		public AgentBuildData IsReinforcement(bool isReinforcement)
		{
			this.AgentIsReinforcement = isReinforcement;
			return this;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000172AB File Offset: 0x000154AB
		public AgentBuildData SpawnsIntoOwnFormation(bool spawnIntoOwnFormation)
		{
			this.AgentSpawnsIntoOwnFormation = spawnIntoOwnFormation;
			return this;
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000172B5 File Offset: 0x000154B5
		public AgentBuildData SpawnsUsingOwnTroopClass(bool spawnUsingOwnTroopClass)
		{
			this.AgentSpawnsUsingOwnTroopClass = spawnUsingOwnTroopClass;
			return this;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x000172BF File Offset: 0x000154BF
		public AgentBuildData MakeUnitStandOutOfFormationDistance(float makeUnitStandOutDistance)
		{
			this.MakeUnitStandOutDistance = makeUnitStandOutDistance;
			return this;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000172C9 File Offset: 0x000154C9
		public AgentBuildData InitialPosition(in Vec3 position)
		{
			this.AgentInitialPosition = new Vec3?(position);
			return this;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000172DD File Offset: 0x000154DD
		public AgentBuildData InitialDirection(in Vec2 direction)
		{
			this.AgentInitialDirection = new Vec2?(direction);
			return this;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000172F4 File Offset: 0x000154F4
		public AgentBuildData InitialFrameFromSpawnPointEntity(GameEntity entity)
		{
			MatrixFrame globalFrame = entity.GetGlobalFrame();
			this.AgentInitialPosition = new Vec3?(globalFrame.origin);
			this.AgentInitialDirection = new Vec2?(globalFrame.rotation.f.AsVec2.Normalized());
			return this;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00017340 File Offset: 0x00015540
		public AgentBuildData InitialFrameFromSpawnPointEntity(WeakGameEntity entity)
		{
			MatrixFrame globalFrame = entity.GetGlobalFrame();
			this.AgentInitialPosition = new Vec3?(globalFrame.origin);
			this.AgentInitialDirection = new Vec2?(globalFrame.rotation.f.AsVec2.Normalized());
			return this;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0001738B File Offset: 0x0001558B
		public AgentBuildData Formation(Formation formation)
		{
			this.AgentFormation = formation;
			return this;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00017395 File Offset: 0x00015595
		public AgentBuildData Monster(Monster monster)
		{
			this.AgentData.Monster(monster);
			return this;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000173A5 File Offset: 0x000155A5
		public AgentBuildData VisualsIndex(int index)
		{
			this.AgentVisualsIndex = index;
			return this;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x000173AF File Offset: 0x000155AF
		public AgentBuildData Equipment(Equipment equipment)
		{
			this.AgentData.Equipment(equipment);
			return this;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x000173BF File Offset: 0x000155BF
		public AgentBuildData MissionEquipment(MissionEquipment missionEquipment)
		{
			this.AgentOverridenSpawnMissionEquipment = missionEquipment;
			return this;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x000173C9 File Offset: 0x000155C9
		public AgentBuildData EquipmentSeed(int seed)
		{
			this.AgentData.EquipmentSeed(seed);
			return this;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000173D9 File Offset: 0x000155D9
		public AgentBuildData NoHorses(bool noHorses)
		{
			this.AgentData.NoHorses(noHorses);
			return this;
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x000173E9 File Offset: 0x000155E9
		public AgentBuildData NoWeapons(bool noWeapons)
		{
			this.AgentData.NoWeapons(noWeapons);
			return this;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000173F9 File Offset: 0x000155F9
		public AgentBuildData NoArmor(bool noArmor)
		{
			this.AgentData.NoArmor(noArmor);
			return this;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00017409 File Offset: 0x00015609
		public AgentBuildData FixedEquipment(bool fixedEquipment)
		{
			this.AgentData.FixedEquipment(fixedEquipment);
			return this;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00017419 File Offset: 0x00015619
		public AgentBuildData CivilianEquipment(bool civilianEquipment)
		{
			this.AgentData.CivilianEquipment(civilianEquipment);
			return this;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00017429 File Offset: 0x00015629
		public AgentBuildData SetPrepareImmediately()
		{
			this.AgentData.SetPrepareImmediately();
			return this;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00017438 File Offset: 0x00015638
		public AgentBuildData ClothingColor1(uint color)
		{
			this.AgentData.ClothingColor1(color);
			return this;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00017448 File Offset: 0x00015648
		public AgentBuildData ClothingColor2(uint color)
		{
			this.AgentData.ClothingColor2(color);
			return this;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00017458 File Offset: 0x00015658
		public AgentBuildData MissionPeer(MissionPeer missionPeer)
		{
			this.AgentMissionPeer = missionPeer;
			return this;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00017462 File Offset: 0x00015662
		public AgentBuildData OwningMissionPeer(MissionPeer missionPeer)
		{
			this.OwningAgentMissionPeer = missionPeer;
			return this;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0001746C File Offset: 0x0001566C
		public AgentBuildData BodyProperties(BodyProperties bodyProperties)
		{
			this.AgentData.BodyProperties(bodyProperties);
			return this;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0001747C File Offset: 0x0001567C
		public AgentBuildData Age(int age)
		{
			this.AgentData.Age(age);
			return this;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0001748C File Offset: 0x0001568C
		public AgentBuildData TroopOrigin(IAgentOriginBase troopOrigin)
		{
			this.AgentData.TroopOrigin(troopOrigin);
			return this;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0001749C File Offset: 0x0001569C
		public AgentBuildData IsFemale(bool isFemale)
		{
			this.AgentData.IsFemale(isFemale);
			return this;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000174AC File Offset: 0x000156AC
		public AgentBuildData Race(int race)
		{
			this.AgentData.Race(race);
			return this;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000174BC File Offset: 0x000156BC
		public AgentBuildData MountKey(string mountKey)
		{
			this.AgentData.MountKey(mountKey);
			return this;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x000174CC File Offset: 0x000156CC
		public AgentBuildData Index(int index)
		{
			this.AgentIndex = index;
			this.AgentIndexOverriden = true;
			return this;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x000174DD File Offset: 0x000156DD
		public AgentBuildData MountIndex(int mountIndex)
		{
			this.AgentMountIndex = mountIndex;
			this.AgentMountIndexOverriden = true;
			return this;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000174EE File Offset: 0x000156EE
		public AgentBuildData Banner(Banner banner)
		{
			this.AgentBanner = banner;
			return this;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000174F8 File Offset: 0x000156F8
		public AgentBuildData BannerItem(ItemObject bannerItem)
		{
			this.AgentBannerItem = bannerItem;
			return this;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00017502 File Offset: 0x00015702
		public AgentBuildData BannerReplacementWeaponItem(ItemObject weaponItem)
		{
			this.AgentBannerReplacementWeaponItem = weaponItem;
			return this;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0001750C File Offset: 0x0001570C
		public AgentBuildData FormationTroopSpawnCount(int formationTroopCount)
		{
			this.AgentFormationTroopSpawnCount = formationTroopCount;
			return this;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00017516 File Offset: 0x00015716
		public AgentBuildData FormationTroopSpawnIndex(int formationTroopIndex)
		{
			this.AgentFormationTroopSpawnIndex = formationTroopIndex;
			return this;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00017520 File Offset: 0x00015720
		public AgentBuildData CanSpawnOutsideOfMissionBoundary(bool canSpawn)
		{
			this.AgentCanSpawnOutsideOfMissionBoundary = canSpawn;
			return this;
		}
	}
}
