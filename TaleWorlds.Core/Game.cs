using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.SaveSystem.Save;

namespace TaleWorlds.Core
{
	// Token: 0x02000069 RID: 105
	[SaveableRootClass(5000)]
	public sealed class Game : IGameStateManagerOwner
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600074A RID: 1866 RVA: 0x0001923C File Offset: 0x0001743C
		// (remove) Token: 0x0600074B RID: 1867 RVA: 0x00019270 File Offset: 0x00017470
		public static event Action OnGameCreated;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600074C RID: 1868 RVA: 0x000192A4 File Offset: 0x000174A4
		// (remove) Token: 0x0600074D RID: 1869 RVA: 0x000192DC File Offset: 0x000174DC
		public event Action<ItemObject> OnItemDeserializedEvent;

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00019311 File Offset: 0x00017511
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x00019319 File Offset: 0x00017519
		public Game.State CurrentState { get; private set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00019322 File Offset: 0x00017522
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x0001932A File Offset: 0x0001752A
		public IMonsterMissionDataCreator MonsterMissionDataCreator { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00019334 File Offset: 0x00017534
		public Monster DefaultMonster
		{
			get
			{
				Monster monster;
				if ((monster = this._defaultMonster) == null)
				{
					monster = (this._defaultMonster = this.ObjectManager.GetFirstObject<Monster>());
				}
				return monster;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0001935F File Offset: 0x0001755F
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x00019367 File Offset: 0x00017567
		[SaveableProperty(3)]
		public GameType GameType { get; private set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00019370 File Offset: 0x00017570
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x00019378 File Offset: 0x00017578
		public DefaultSiegeEngineTypes DefaultSiegeEngineTypes { get; private set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00019381 File Offset: 0x00017581
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x00019389 File Offset: 0x00017589
		public MBObjectManager ObjectManager { get; private set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00019392 File Offset: 0x00017592
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0001939A File Offset: 0x0001759A
		[SaveableProperty(8)]
		public BasicCharacterObject PlayerTroop { get; set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x000193A3 File Offset: 0x000175A3
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x000193AB File Offset: 0x000175AB
		[SaveableProperty(12)]
		internal MBFastRandom RandomGenerator { get; private set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x000193B4 File Offset: 0x000175B4
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x000193BC File Offset: 0x000175BC
		public BasicGameModels BasicModels { get; private set; }

		// Token: 0x0600075F RID: 1887 RVA: 0x000193C8 File Offset: 0x000175C8
		public T AddGameModelsManager<T>(IEnumerable<GameModel> inputComponents) where T : GameModelsManager
		{
			T t = (T)((object)Activator.CreateInstance(typeof(T), new object[] { inputComponents }));
			this._gameModelManagers.Add(typeof(T), t);
			return t;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00019410 File Offset: 0x00017610
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x00019418 File Offset: 0x00017618
		public GameManagerBase GameManager { get; private set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00019421 File Offset: 0x00017621
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00019429 File Offset: 0x00017629
		public GameTextManager GameTextManager { get; private set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00019432 File Offset: 0x00017632
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0001943A File Offset: 0x0001763A
		public GameStateManager GameStateManager { get; private set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00019443 File Offset: 0x00017643
		public bool CheatMode
		{
			get
			{
				return this.GameManager.CheatMode;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00019450 File Offset: 0x00017650
		public bool IsDevelopmentMode
		{
			get
			{
				return this.GameManager.IsDevelopmentMode;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0001945D File Offset: 0x0001765D
		public bool IsEditModeOn
		{
			get
			{
				return this.GameManager.IsEditModeOn;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001946A File Offset: 0x0001766A
		public UnitSpawnPrioritizations UnitSpawnPrioritization
		{
			get
			{
				return this.GameManager.UnitSpawnPrioritization;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00019477 File Offset: 0x00017677
		public float ApplicationTime
		{
			get
			{
				return this.GameManager.ApplicationTime;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00019484 File Offset: 0x00017684
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x0001948B File Offset: 0x0001768B
		public static Game Current
		{
			get
			{
				return Game._current;
			}
			internal set
			{
				Game._current = value;
				Action onGameCreated = Game.OnGameCreated;
				if (onGameCreated == null)
				{
					return;
				}
				onGameCreated();
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x000194A2 File Offset: 0x000176A2
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x000194AA File Offset: 0x000176AA
		public IBannerVisualCreator BannerVisualCreator { get; set; }

		// Token: 0x0600076F RID: 1903 RVA: 0x000194B3 File Offset: 0x000176B3
		public IBannerVisual CreateBannerVisual(Banner banner)
		{
			IBannerVisualCreator bannerVisualCreator = this.BannerVisualCreator;
			if (bannerVisualCreator == null)
			{
				return null;
			}
			return bannerVisualCreator.CreateBannerVisual(banner);
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x000194C8 File Offset: 0x000176C8
		public int NextUniqueTroopSeed
		{
			get
			{
				int nextUniqueTroopSeed = this._nextUniqueTroopSeed;
				this._nextUniqueTroopSeed = nextUniqueTroopSeed + 1;
				return nextUniqueTroopSeed;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x000194E6 File Offset: 0x000176E6
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x000194EE File Offset: 0x000176EE
		public DefaultCharacterAttributes DefaultCharacterAttributes { get; private set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x000194F7 File Offset: 0x000176F7
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x000194FF File Offset: 0x000176FF
		public DefaultSkills DefaultSkills { get; private set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00019508 File Offset: 0x00017708
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x00019510 File Offset: 0x00017710
		public DefaultBannerEffects DefaultBannerEffects { get; private set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00019519 File Offset: 0x00017719
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x00019521 File Offset: 0x00017721
		public DefaultItemCategories DefaultItemCategories { get; private set; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0001952A File Offset: 0x0001772A
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x00019532 File Offset: 0x00017732
		public EventManager EventManager { get; private set; }

		// Token: 0x0600077B RID: 1915 RVA: 0x0001953C File Offset: 0x0001773C
		public Equipment GetDefaultEquipmentWithName(string equipmentName)
		{
			if (!this._defaultEquipments.ContainsKey(equipmentName))
			{
				Debug.FailedAssert("Equipment with name \"" + equipmentName + "\" could not be found.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\Game.cs", "GetDefaultEquipmentWithName", 125);
				return null;
			}
			return this._defaultEquipments[equipmentName].Clone(false);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001958C File Offset: 0x0001778C
		public void SetDefaultEquipments(IReadOnlyDictionary<string, Equipment> defaultEquipments)
		{
			if (this._defaultEquipments == null)
			{
				this._defaultEquipments = defaultEquipments;
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001959D File Offset: 0x0001779D
		public static Game CreateGame(GameType gameType, GameManagerBase gameManager, uint seed)
		{
			Game game = Game.CreateGame(gameType, gameManager);
			game.RandomGenerator = new MBFastRandom(seed);
			return game;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000195B4 File Offset: 0x000177B4
		private Game(GameType gameType, GameManagerBase gameManager, MBObjectManager objectManager)
		{
			this.GameType = gameType;
			Game.Current = this;
			this.GameType.CurrentGame = this;
			this.GameManager = gameManager;
			this.GameManager.Game = this;
			this.EventManager = new EventManager();
			this.ObjectManager = objectManager;
			this.RandomGenerator = new MBFastRandom();
			this.InitializeParameters();
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00019620 File Offset: 0x00017820
		public static Game CreateGame(GameType gameType, GameManagerBase gameManager)
		{
			MBObjectManager mbobjectManager = MBObjectManager.Init();
			Game.RegisterTypes(gameType, mbobjectManager, gameManager);
			return new Game(gameType, gameManager, mbobjectManager);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00019644 File Offset: 0x00017844
		public static Game LoadSaveGame(LoadResult loadResult, GameManagerBase gameManager)
		{
			MBSaveLoad.OnStartGame(loadResult);
			MBObjectManager mbobjectManager = MBObjectManager.Init();
			Game game = (Game)loadResult.Root;
			Game.RegisterTypes(game.GameType, mbobjectManager, gameManager);
			loadResult.InitializeObjects();
			MBObjectManager.Instance.ReInitialize();
			loadResult.AfterInitializeObjects();
			GC.Collect();
			game.ObjectManager = mbobjectManager;
			game.BeginLoading(gameManager);
			return game;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001969E File Offset: 0x0001789E
		[LoadInitializationCallback]
		private void OnLoad()
		{
			if (this.RandomGenerator == null)
			{
				this.RandomGenerator = new MBFastRandom();
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000196B3 File Offset: 0x000178B3
		private void BeginLoading(GameManagerBase gameManager)
		{
			Game.Current = this;
			this.GameType.CurrentGame = this;
			this.GameManager = gameManager;
			this.GameManager.Game = this;
			this.EventManager = new EventManager();
			this.InitializeParameters();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000196EC File Offset: 0x000178EC
		private void SaveAux(MetaData metaData, string saveName, ISaveDriver driver, Action<SaveResult> onSaveCompleted)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnBeforeSave();
			}
			SaveOutput saveOutput = SaveManager.Save(this, metaData, saveName, driver);
			if (!saveOutput.IsContinuing)
			{
				this.OnSaveCompleted(saveOutput, onSaveCompleted);
				return;
			}
			this._currentActiveSaveData = new Tuple<SaveOutput, Action<SaveResult>>(saveOutput, onSaveCompleted);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001976C File Offset: 0x0001796C
		private void OnSaveCompleted(SaveOutput finishedOutput, Action<SaveResult> onSaveCompleted)
		{
			finishedOutput.PrintStatus();
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnAfterSave();
			}
			Common.MemoryCleanupGC(false);
			if (onSaveCompleted != null)
			{
				onSaveCompleted(finishedOutput.Result);
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000197DC File Offset: 0x000179DC
		public void Save(MetaData metaData, string saveName, ISaveDriver driver, Action<SaveResult> onSaveCompleted)
		{
			using (new PerformanceTestBlock("Save Process"))
			{
				this.SaveAux(metaData, saveName, driver, onSaveCompleted);
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001981C File Offset: 0x00017A1C
		private void InitializeParameters()
		{
			ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_core_parameters"));
			this.GameType.InitializeParameters();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00019842 File Offset: 0x00017A42
		void IGameStateManagerOwner.OnStateStackEmpty()
		{
			this.Destroy();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001984C File Offset: 0x00017A4C
		public void Destroy()
		{
			this.CurrentState = Game.State.Destroying;
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameEnd();
			}
			this.GameManager.OnGameEnd(this);
			this.GameType.OnDestroy();
			this.ObjectManager.Destroy();
			this.EventManager.Clear();
			this.EventManager = null;
			GameStateManager.Current = null;
			this.GameStateManager = null;
			Game.Current = null;
			this.CurrentState = Game.State.Destroyed;
			this._currentActiveSaveData = null;
			Common.MemoryCleanupGC(false);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00019904 File Offset: 0x00017B04
		public void CreateGameManager()
		{
			this.GameStateManager = new GameStateManager(this, GameStateManager.GameStateManagerType.Game);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00019913 File Offset: 0x00017B13
		public void OnStateChanged(GameState oldState)
		{
			this.GameType.OnStateChanged(oldState);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00019921 File Offset: 0x00017B21
		public T AddGameHandler<T>() where T : GameHandler, new()
		{
			return this._gameEntitySystem.AddComponent<T>();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001992E File Offset: 0x00017B2E
		public T GetGameHandler<T>() where T : GameHandler
		{
			return this._gameEntitySystem.GetComponent<T>();
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001993B File Offset: 0x00017B3B
		public void RemoveGameHandler<T>() where T : GameHandler
		{
			this._gameEntitySystem.RemoveComponent<T>();
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00019948 File Offset: 0x00017B48
		public void Initialize()
		{
			if (this._gameEntitySystem == null)
			{
				this._gameEntitySystem = new EntitySystem<GameHandler>();
			}
			this.GameTextManager = new GameTextManager();
			this.GameTextManager.LoadGameTexts();
			this._gameModelManagers = new Dictionary<Type, GameModelsManager>();
			GameTexts.Initialize(this.GameTextManager);
			this.GameType.OnInitialize();
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000199A0 File Offset: 0x00017BA0
		public static void RegisterTypes(GameType gameType, MBObjectManager objectManager, GameManagerBase gameManager)
		{
			if (gameType != null)
			{
				gameType.BeforeRegisterTypes(objectManager);
			}
			objectManager.RegisterType<Monster>("Monster", "Monsters", 2U, true, false);
			objectManager.RegisterType<SkeletonScale>("Scale", "Scales", 3U, true, false);
			objectManager.RegisterType<ItemObject>("Item", "Items", 4U, true, false);
			objectManager.RegisterType<ItemModifier>("ItemModifier", "ItemModifiers", 6U, true, false);
			objectManager.RegisterType<ItemModifierGroup>("ItemModifierGroup", "ItemModifierGroups", 7U, true, false);
			objectManager.RegisterType<CharacterAttribute>("CharacterAttribute", "CharacterAttributes", 8U, true, false);
			objectManager.RegisterType<SkillObject>("Skill", "Skills", 9U, true, false);
			objectManager.RegisterType<ItemCategory>("ItemCategory", "ItemCategories", 10U, true, false);
			objectManager.RegisterType<CraftingPiece>("CraftingPiece", "CraftingPieces", 11U, true, false);
			objectManager.RegisterType<CraftingTemplate>("CraftingTemplate", "CraftingTemplates", 12U, true, false);
			objectManager.RegisterType<SiegeEngineType>("SiegeEngineType", "SiegeEngineTypes", 13U, true, false);
			objectManager.RegisterType<WeaponDescription>("WeaponDescription", "WeaponDescriptions", 14U, true, false);
			objectManager.RegisterType<MBBodyProperty>("BodyProperty", "BodyProperties", 50U, true, false);
			objectManager.RegisterType<MBEquipmentRoster>("EquipmentRoster", "EquipmentRosters", 51U, true, false);
			objectManager.RegisterType<MBCharacterSkills>("SkillSet", "SkillSets", 52U, true, false);
			objectManager.RegisterType<BannerEffect>("BannerEffect", "BannerEffects", 53U, true, false);
			if (gameType != null)
			{
				gameType.OnRegisterTypes(objectManager);
			}
			if (gameManager != null)
			{
				gameManager.RegisterSubModuleTypes();
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00019B04 File Offset: 0x00017D04
		public void SetBasicModels(IEnumerable<GameModel> models)
		{
			this.BasicModels = this.AddGameModelsManager<BasicGameModels>(models);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00019B14 File Offset: 0x00017D14
		internal void OnTick(float dt)
		{
			if (GameStateManager.Current == this.GameStateManager)
			{
				this.GameStateManager.OnTick(dt);
				if (this._gameEntitySystem != null)
				{
					foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
					{
						try
						{
							gameHandler.OnTick(dt);
						}
						catch (Exception ex)
						{
							Debug.Print("Exception on gameHandler tick: " + ex, 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
				}
			}
			Action<float> afterTick = this.AfterTick;
			if (afterTick != null)
			{
				afterTick(dt);
			}
			Tuple<SaveOutput, Action<SaveResult>> currentActiveSaveData = this._currentActiveSaveData;
			if (currentActiveSaveData != null && !currentActiveSaveData.Item1.IsContinuing)
			{
				this.OnSaveCompleted(this._currentActiveSaveData.Item1, this._currentActiveSaveData.Item2);
				this._currentActiveSaveData = null;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00019C0C File Offset: 0x00017E0C
		internal void OnGameNetworkBegin()
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameNetworkBegin();
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00019C64 File Offset: 0x00017E64
		internal void OnGameNetworkEnd()
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameNetworkEnd();
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00019CBC File Offset: 0x00017EBC
		internal void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnEarlyPlayerConnect(peer);
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00019D14 File Offset: 0x00017F14
		internal void OnPlayerConnect(VirtualPlayer peer)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnPlayerConnect(peer);
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00019D6C File Offset: 0x00017F6C
		internal void OnPlayerDisconnect(VirtualPlayer peer)
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnPlayerDisconnect(peer);
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00019DC4 File Offset: 0x00017FC4
		public void OnGameStart()
		{
			foreach (GameHandler gameHandler in this._gameEntitySystem.Components)
			{
				gameHandler.OnGameStart();
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00019E1C File Offset: 0x0001801C
		public bool DoLoading()
		{
			return this.GameType.DoLoadingForGameType();
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00019E29 File Offset: 0x00018029
		public void OnMissionIsStarting(string missionName, MissionInitializerRecord rec)
		{
			this.GameType.OnMissionIsStarting(missionName, rec);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00019E38 File Offset: 0x00018038
		public void OnFinalize()
		{
			this.CurrentState = Game.State.Destroying;
			GameStateManager.Current.CleanStates(0);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00019E4C File Offset: 0x0001804C
		public void InitializeDefaultGameObjects()
		{
			this.DefaultCharacterAttributes = new DefaultCharacterAttributes();
			this.DefaultSkills = new DefaultSkills();
			this.DefaultBannerEffects = new DefaultBannerEffects();
			this.DefaultItemCategories = new DefaultItemCategories();
			this.DefaultSiegeEngineTypes = new DefaultSiegeEngineTypes();
			this.GameManager.InitializeSubModuleGameObjects(Game.Current);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00019EA0 File Offset: 0x000180A0
		public void LoadBasicFiles()
		{
			this.ObjectManager.LoadXML("Monsters", false);
			this.ObjectManager.LoadXML("SkeletonScales", false);
			this.ObjectManager.LoadXML("ItemModifiers", false);
			this.ObjectManager.LoadXML("ItemModifierGroups", false);
			this.ObjectManager.LoadXML("CraftingPieces", false);
			this.ObjectManager.LoadXML("WeaponDescriptions", false);
			this.ObjectManager.LoadXML("CraftingTemplates", false);
			this.ObjectManager.LoadXML("BodyProperties", false);
			this.ObjectManager.LoadXML("SkillSets", false);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00019F46 File Offset: 0x00018146
		public void ItemObjectDeserialized(ItemObject itemObject)
		{
			Action<ItemObject> onItemDeserializedEvent = this.OnItemDeserializedEvent;
			if (onItemDeserializedEvent == null)
			{
				return;
			}
			onItemDeserializedEvent(itemObject);
		}

		// Token: 0x040003ED RID: 1005
		public Action<float> AfterTick;

		// Token: 0x040003F0 RID: 1008
		private EntitySystem<GameHandler> _gameEntitySystem;

		// Token: 0x040003F1 RID: 1009
		private Monster _defaultMonster;

		// Token: 0x040003F8 RID: 1016
		private Dictionary<Type, GameModelsManager> _gameModelManagers;

		// Token: 0x040003FC RID: 1020
		private static Game _current;

		// Token: 0x040003FE RID: 1022
		[SaveableField(11)]
		private int _nextUniqueTroopSeed = 1;

		// Token: 0x04000404 RID: 1028
		private IReadOnlyDictionary<string, Equipment> _defaultEquipments;

		// Token: 0x04000405 RID: 1029
		private Tuple<SaveOutput, Action<SaveResult>> _currentActiveSaveData;

		// Token: 0x02000114 RID: 276
		public enum State
		{
			// Token: 0x0400079B RID: 1947
			Running,
			// Token: 0x0400079C RID: 1948
			Destroying,
			// Token: 0x0400079D RID: 1949
			Destroyed
		}
	}
}
