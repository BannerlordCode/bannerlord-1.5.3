using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D0 RID: 464
	public abstract class MBGameManager : GameManagerBase
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x00060AF9 File Offset: 0x0005ECF9
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x00060B01 File Offset: 0x0005ED01
		public bool IsEnding { get; private set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x00060B0A File Offset: 0x0005ED0A
		public new static MBGameManager Current
		{
			get
			{
				return (MBGameManager)GameManagerBase.Current;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x00060B16 File Offset: 0x0005ED16
		// (set) Token: 0x06001BE7 RID: 7143 RVA: 0x00060B1E File Offset: 0x0005ED1E
		public bool IsLoaded { get; protected set; }

		// Token: 0x06001BE8 RID: 7144 RVA: 0x00060B27 File Offset: 0x0005ED27
		protected MBGameManager()
		{
			this.IsEnding = false;
			NativeConfig.OnConfigChanged();
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00060B46 File Offset: 0x0005ED46
		protected static void StartNewGame()
		{
			MBAPI.IMBGame.StartNew();
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x00060B52 File Offset: 0x0005ED52
		protected static void LoadModuleData(bool isLoadGame)
		{
			MBAPI.IMBGame.LoadModuleData(isLoadGame);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x00060B60 File Offset: 0x0005ED60
		public static void StartNewGame(MBGameManager gameLoader)
		{
			Module.CurrentModule.OnBeforeGameStart(gameLoader);
			GameLoadingState gameLoadingState = GameStateManager.Current.CreateState<GameLoadingState>();
			gameLoadingState.SetLoadingParameters(gameLoader);
			GameStateManager.Current.CleanAndPushState(gameLoadingState, 0);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x00060B98 File Offset: 0x0005ED98
		public override void BeginGameStart(Game game)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.BeginGameStart(game);
			}
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x00060BF0 File Offset: 0x0005EDF0
		public override void OnNewCampaignStart(Game game, object starterObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnCampaignStart(game, starterObject);
			}
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00060C48 File Offset: 0x0005EE48
		public override void InitializeSubModuleGameObjects(Game game)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.InitializeSubModuleGameObjects(game);
			}
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x00060CA0 File Offset: 0x0005EEA0
		public override void RegisterSubModuleObjects(bool isSavedCampaign)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.RegisterSubModuleObjects(isSavedCampaign);
			}
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00060CF8 File Offset: 0x0005EEF8
		public override void RegisterSubModuleTypes()
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.RegisterSubModuleTypes();
			}
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x00060D4C File Offset: 0x0005EF4C
		public override void AfterRegisterSubModuleObjects(bool isSavedCampaign)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.AfterRegisterSubModuleObjects(isSavedCampaign);
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00060DA4 File Offset: 0x0005EFA4
		public override void InitializeGameStarter(Game game, IGameStarter starterObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.InitializeGameStarter(game, starterObject);
			}
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00060DFC File Offset: 0x0005EFFC
		public override void OnGameInitializationFinished(Game game)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnGameInitializationFinished(game);
			}
			foreach (SkeletonScale skeletonScale in Game.Current.ObjectManager.GetObjectTypeList<SkeletonScale>())
			{
				sbyte[] array = new sbyte[skeletonScale.BoneNames.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Skeleton.GetBoneIndexFromName(skeletonScale.SkeletonModel, skeletonScale.BoneNames[i]);
				}
				skeletonScale.SetBoneIndices(array);
			}
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x00060EDC File Offset: 0x0005F0DC
		public override void OnAfterGameInitializationFinished(Game game, object initializerObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnAfterGameInitializationFinished(game, initializerObject);
			}
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00060F34 File Offset: 0x0005F134
		public override void OnGameLoaded(Game game, object initializerObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnGameLoaded(game, initializerObject);
			}
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x00060F8C File Offset: 0x0005F18C
		public override void OnAfterGameLoaded(Game game)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnAfterGameLoaded(game);
			}
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x00060FE4 File Offset: 0x0005F1E4
		public override void OnNewGameCreated(Game game, object initializerObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnNewGameCreated(game, initializerObject);
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0006103C File Offset: 0x0005F23C
		public override void OnGameStart(Game game, IGameStarter gameStarter)
		{
			Game.Current.MonsterMissionDataCreator = new MonsterMissionDataCreator();
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnGameStart(game, gameStarter);
			}
			Game.Current.AddGameModelsManager<MissionGameModels>(gameStarter.Models);
			Monster.GetBoneIndexWithId = new Func<string, string, sbyte>(MBActionSet.GetBoneIndexWithId);
			Monster.GetBoneHasParentBone = new Func<string, sbyte, bool>(MBActionSet.GetBoneHasParentBone);
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000610D4 File Offset: 0x0005F2D4
		public override void OnGameEnd(Game game)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnGameEnd(game);
			}
			Module.CurrentModule.OnGameEnd();
			MissionGameModels.Clear();
			base.OnGameEnd(game);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x00061140 File Offset: 0x0005F340
		public static async void EndGame()
		{
			for (;;)
			{
				MBGameManager mbgameManager = MBGameManager.Current;
				if (mbgameManager == null || mbgameManager.IsLoaded)
				{
					break;
				}
				await Task.Delay(100);
			}
			MBGameManager mbgameManager2 = MBGameManager.Current;
			if (mbgameManager2 == null || mbgameManager2.CheckAndSetEnding())
			{
				if (Game.Current.GameStateManager != null)
				{
					while (Mission.Current != null && !(Game.Current.GameStateManager.ActiveState is MissionState))
					{
						Game.Current.GameStateManager.PopState(0);
					}
					if (Game.Current.GameStateManager.ActiveState is MissionState)
					{
						((MissionState)Game.Current.GameStateManager.ActiveState).CurrentMission.EndMission();
						while (Mission.Current != null)
						{
							await Task.Delay(1);
						}
					}
					else
					{
						Game.Current.GameStateManager.CleanStates(0);
					}
				}
			}
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x00061171 File Offset: 0x0005F371
		public override void OnLoadFinished()
		{
			this.IsLoaded = true;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x0006117C File Offset: 0x0005F37C
		public bool CheckAndSetEnding()
		{
			object lockObject = this._lockObject;
			bool flag2;
			lock (lockObject)
			{
				if (this.IsEnding)
				{
					flag2 = false;
				}
				else
				{
					this.IsEnding = true;
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000611CC File Offset: 0x0005F3CC
		public virtual void OnSessionInvitationAccepted(SessionInvitationType targetGameType)
		{
			if (targetGameType != SessionInvitationType.None)
			{
				MBGameManager.EndGame();
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000611D6 File Offset: 0x0005F3D6
		public virtual void OnPlatformRequestedMultiplayer()
		{
			MBGameManager.EndGame();
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000611DD File Offset: 0x0005F3DD
		protected List<MbObjectXmlInformation> GetXmlInformationFromModule()
		{
			return XmlResource.XmlInformationList;
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001C00 RID: 7168 RVA: 0x000611E4 File Offset: 0x0005F3E4
		public override float ApplicationTime
		{
			get
			{
				return MBCommon.GetApplicationTime();
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x000611EB File Offset: 0x0005F3EB
		public override bool CheatMode
		{
			get
			{
				return NativeConfig.CheatMode;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001C02 RID: 7170 RVA: 0x000611F2 File Offset: 0x0005F3F2
		public override bool IsDevelopmentMode
		{
			get
			{
				return NativeConfig.IsDevelopmentMode;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x000611F9 File Offset: 0x0005F3F9
		public override bool IsEditModeOn
		{
			get
			{
				return MBEditor.IsEditModeOn;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x00061200 File Offset: 0x0005F400
		public override UnitSpawnPrioritizations UnitSpawnPrioritization
		{
			get
			{
				return (UnitSpawnPrioritizations)BannerlordConfig.UnitSpawnPrioritization;
			}
		}

		// Token: 0x04000926 RID: 2342
		private readonly object _lockObject = new object();
	}
}
