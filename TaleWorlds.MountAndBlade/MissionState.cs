using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000248 RID: 584
	public class MissionState : GameState
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x00077204 File Offset: 0x00075404
		// (set) Token: 0x060021D1 RID: 8657 RVA: 0x0007720C File Offset: 0x0007540C
		public IMissionSystemHandler Handler { get; set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x00077215 File Offset: 0x00075415
		// (set) Token: 0x060021D3 RID: 8659 RVA: 0x0007721C File Offset: 0x0007541C
		public static MissionState Current { get; private set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060021D4 RID: 8660 RVA: 0x00077224 File Offset: 0x00075424
		// (set) Token: 0x060021D5 RID: 8661 RVA: 0x0007722C File Offset: 0x0007542C
		public Mission CurrentMission { get; private set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x00077235 File Offset: 0x00075435
		// (set) Token: 0x060021D7 RID: 8663 RVA: 0x0007723D File Offset: 0x0007543D
		public string MissionName { get; private set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00077246 File Offset: 0x00075446
		// (set) Token: 0x060021D9 RID: 8665 RVA: 0x0007724E File Offset: 0x0007544E
		public bool FirstMissionTickAfterLoading { get; private set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x00077257 File Offset: 0x00075457
		// (set) Token: 0x060021DB RID: 8667 RVA: 0x0007725F File Offset: 0x0007545F
		public bool Paused { get; set; }

		// Token: 0x060021DC RID: 8668 RVA: 0x00077268 File Offset: 0x00075468
		protected override void OnInitialize()
		{
			base.OnInitialize();
			MissionState.Current = this;
			this.FirstMissionTickAfterLoading = true;
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00077282 File Offset: 0x00075482
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.CurrentMission.OnMissionStateFinalize(this.CurrentMission.NeedsMemoryCleanup);
			this.CurrentMission = null;
			MissionState.Current = null;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000772AD File Offset: 0x000754AD
		protected override void OnActivate()
		{
			base.OnActivate();
			this.CurrentMission.OnMissionStateActivate();
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000772C0 File Offset: 0x000754C0
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.CurrentMission.OnMissionStateDeactivate();
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000772D3 File Offset: 0x000754D3
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			if (this.CurrentMission != null && this.CurrentMission.CurrentState == Mission.State.Continuing)
			{
				this.CurrentMission.IdleTick(dt);
			}
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x00077300 File Offset: 0x00075500
		protected override void OnTick(float realDt)
		{
			base.OnTick(realDt);
			if (this._isDelayedDisconnecting && this.CurrentMission != null && this.CurrentMission.CurrentState == Mission.State.Continuing)
			{
				BannerlordNetwork.EndMultiplayerLobbyMission();
			}
			if (this.CurrentMission == null)
			{
				return;
			}
			if (this.CurrentMission.CurrentState == Mission.State.NewlyCreated || this.CurrentMission.CurrentState == Mission.State.Initializing)
			{
				if (this.CurrentMission.CurrentState == Mission.State.NewlyCreated)
				{
					this.CurrentMission.ClearUnreferencedResources(this.CurrentMission.NeedsMemoryCleanup);
				}
				this.TickLoading(realDt);
			}
			else if (this.CurrentMission.CurrentState == Mission.State.Continuing || this.CurrentMission.MissionEnded)
			{
				if (this.MissionReplayStartTime != 0f)
				{
					this.CurrentMission.SkipForwardMissionReplay(this.MissionReplayStartTime, 0.033f);
					this.MissionReplayStartTime = 0f;
				}
				bool flag = false;
				if (this.MissionEndTime != 0f && this.CurrentMission.CurrentTime > this.MissionEndTime)
				{
					this.CurrentMission.EndMission();
					flag = true;
				}
				if (!flag && (this.Handler == null || this.Handler.RenderIsReady()))
				{
					this.TickMission(realDt);
				}
				if (flag && MBEditor._isEditorMissionOn)
				{
					MBEditor.LeaveEditMissionMode();
					this.TickMission(realDt);
				}
			}
			if (this.CurrentMission.CurrentState == Mission.State.Over)
			{
				if (MBGameManager.Current.IsEnding)
				{
					Game.Current.GameStateManager.CleanStates(0);
					return;
				}
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x00077478 File Offset: 0x00075678
		private void TickMission(float realDt)
		{
			if (this.FirstMissionTickAfterLoading && this.CurrentMission != null && this.CurrentMission.CurrentState == Mission.State.Continuing && GameNetwork.IsClient)
			{
				int currentBattleIndex = GameNetwork.GetNetworkComponent<BaseNetworkComponentData>().CurrentBattleIndex;
				MBDebug.Print(string.Format("Client: I finished loading battle with index: {0}. Sending confirmation to server.", currentBattleIndex), 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new FinishedLoading(currentBattleIndex));
				GameNetwork.EndModuleEventAsClient();
				GameNetwork.SyncRelevantGameOptionsToServer();
			}
			IMissionSystemHandler handler = this.Handler;
			if (handler != null)
			{
				handler.BeforeMissionTick(this.CurrentMission, realDt);
			}
			this.CurrentMission.PauseAITick = false;
			if (GameNetwork.IsSessionActive && this.CurrentMission.ClearSceneTimerElapsedTime < 0f)
			{
				this.CurrentMission.PauseAITick = true;
			}
			float num = realDt;
			if (this.Paused || MBCommon.IsPaused)
			{
				num = 0f;
			}
			else if (this.CurrentMission.FixedDeltaTimeMode)
			{
				num = this.CurrentMission.FixedDeltaTime;
			}
			if (!GameNetwork.IsSessionActive)
			{
				this.CurrentMission.UpdateSceneTimeSpeed();
				float timeSpeed = this.CurrentMission.Scene.TimeSpeed;
				num *= timeSpeed;
			}
			if (this.CurrentMission.ClearSceneTimerElapsedTime < -0.3f && !GameNetwork.IsClientOrReplay)
			{
				this.CurrentMission.ClearAgentActions();
			}
			if (this.CurrentMission.CurrentState == Mission.State.Continuing || this.CurrentMission.MissionEnded)
			{
				if (this.CurrentMission.IsFastForward)
				{
					float num2 = num * 9f;
					while (num2 > 1E-06f)
					{
						if (num2 > 0.1f)
						{
							this.TickMissionAux(0.1f, 0.1f, false, false);
							if (this.CurrentMission.CurrentState == Mission.State.Over)
							{
								break;
							}
							num2 -= 0.1f;
						}
						else
						{
							if (num2 > 0.0033333334f)
							{
								this.TickMissionAux(num2, num2, false, false);
							}
							num2 = 0f;
						}
					}
					if (this.CurrentMission.CurrentState != Mission.State.Over)
					{
						this.TickMissionAux(num, realDt, true, false);
					}
				}
				else
				{
					this.TickMissionAux(num, realDt, true, true);
				}
			}
			if (this.Handler != null)
			{
				this.Handler.AfterMissionTick(this.CurrentMission, realDt);
			}
			this.FirstMissionTickAfterLoading = false;
			this._missionTickCount++;
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00077693 File Offset: 0x00075893
		private void TickMissionAux(float dt, float realDt, bool updateCamera, bool asyncAITick)
		{
			this.CurrentMission.Tick(dt);
			if (this._missionTickCount > 2)
			{
				this.CurrentMission.OnTick(dt, realDt, updateCamera, asyncAITick);
			}
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000776BC File Offset: 0x000758BC
		private void TickLoading(float realDt)
		{
			this._tickCountBeforeLoad++;
			if (!this._missionInitializing && this._tickCountBeforeLoad > 0)
			{
				this.LoadMission();
				Utilities.SetLoadingScreenPercentage(0.01f);
				return;
			}
			if (this._missionInitializing && this.CurrentMission.IsLoadingFinished)
			{
				this.FinishMissionLoading();
			}
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x00077714 File Offset: 0x00075914
		private void LoadMission()
		{
			foreach (MissionBehavior missionBehavior in this.CurrentMission.MissionBehaviors)
			{
				missionBehavior.OnMissionScreenPreLoad();
			}
			Utilities.ClearOldResourcesAndObjects();
			this._missionInitializing = true;
			this.CurrentMission.Initialize();
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x00077780 File Offset: 0x00075980
		private void CreateMission(MissionInitializerRecord rec, bool needsMemoryCleanup)
		{
			this.CurrentMission = new Mission(rec, this, needsMemoryCleanup);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00077790 File Offset: 0x00075990
		protected Mission HandleOpenNew(string missionName, MissionInitializerRecord rec, InitializeMissionBehaviorsDelegate handler, bool addDefaultMissionBehaviors, bool needsMemoryCleanup)
		{
			this.MissionName = missionName;
			this.CreateMission(rec, needsMemoryCleanup);
			IEnumerable<MissionBehavior> enumerable = handler(this.CurrentMission);
			enumerable = enumerable.Where<MissionBehavior>((MissionBehavior behavior) => behavior != null);
			if (addDefaultMissionBehaviors)
			{
				enumerable = MissionState.AddDefaultMissionBehaviorsTo(this.CurrentMission, enumerable);
			}
			foreach (MissionBehavior missionBehavior in enumerable)
			{
				missionBehavior.OnAfterMissionCreated();
			}
			this.AddBehaviorsToMission(enumerable);
			if (this.Handler != null)
			{
				enumerable = new MissionBehavior[0];
				enumerable = this.Handler.OnAddBehaviors(enumerable, this.CurrentMission, missionName, addDefaultMissionBehaviors);
				this.AddBehaviorsToMission(enumerable);
			}
			if (GameNetwork.IsDedicatedServer)
			{
				GameNetwork.SetServerFrameRate((double)Module.CurrentModule.StartupInfo.ServerTickRate);
			}
			return this.CurrentMission;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x00077880 File Offset: 0x00075A80
		private void AddBehaviorsToMission(IEnumerable<MissionBehavior> behaviors)
		{
			MissionLogic[] array = (from behavior in behaviors.OfType<MissionLogic>()
				where !(behavior is MissionNetwork)
				select behavior).ToArray<MissionLogic>();
			MissionBehavior[] array2 = behaviors.Where<MissionBehavior>((MissionBehavior behavior) => behavior != null && !(behavior is MissionNetwork) && !(behavior is MissionLogic)).ToArray<MissionBehavior>();
			MissionNetwork[] array3 = behaviors.OfType<MissionNetwork>().ToArray<MissionNetwork>();
			this.CurrentMission.InitializeStartingBehaviors(array, array2, array3);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00077902 File Offset: 0x00075B02
		protected static bool IsRecordingActive()
		{
			if (GameNetwork.IsServer)
			{
				return MultiplayerOptions.OptionType.EnableMissionRecording.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			return MissionState.RecordMission && Game.Current.GameType.IsCoreOnlyGameMode;
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0007792C File Offset: 0x00075B2C
		public static Mission OpenNew(string missionName, MissionInitializerRecord rec, InitializeMissionBehaviorsDelegate handler, bool addDefaultMissionBehaviors = true, bool needsMemoryCleanup = true)
		{
			Debug.Print(string.Concat(new string[] { "Opening new mission ", missionName, " ", rec.SceneLevels, ".\n" }), 0, Debug.DebugColor.White, 17592186044416UL);
			if (!GameNetwork.IsClientOrReplay && !GameNetwork.IsServer)
			{
				MBCommon.CurrentGameType = (MissionState.IsRecordingActive() ? MBCommon.GameType.SingleRecord : MBCommon.GameType.Single);
			}
			Game.Current.OnMissionIsStarting(missionName, rec);
			MissionState missionState = Game.Current.GameStateManager.CreateState<MissionState>();
			Mission mission = missionState.HandleOpenNew(missionName, rec, handler, addDefaultMissionBehaviors, needsMemoryCleanup);
			Game.Current.GameStateManager.PushState(missionState, 0);
			return mission;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000779D4 File Offset: 0x00075BD4
		private static IEnumerable<MissionBehavior> AddDefaultMissionBehaviorsTo(Mission mission, IEnumerable<MissionBehavior> behaviors)
		{
			List<MissionBehavior> list = new List<MissionBehavior>();
			if (GameNetwork.IsSessionActive || GameNetwork.IsReplay)
			{
				list.Add(new MissionNetworkComponent());
			}
			if (MissionState.IsRecordingActive() && !GameNetwork.IsReplay)
			{
				list.Add(new RecordMissionLogic());
			}
			list.Add(new BasicMissionHandler());
			list.Add(new CasualtyHandler());
			list.Add(new AgentCommonAILogic());
			return list.Concat<MissionBehavior>(behaviors);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x00077A44 File Offset: 0x00075C44
		private void FinishMissionLoading()
		{
			this._missionInitializing = false;
			this.CurrentMission.Scene.SetOwnerThread();
			Utilities.SetLoadingScreenPercentage(0.4f);
			for (int i = 0; i < 2; i++)
			{
				this.CurrentMission.Tick(0.001f);
			}
			Utilities.SetLoadingScreenPercentage(0.42f);
			IMissionSystemHandler handler = this.Handler;
			if (handler != null)
			{
				handler.OnMissionAfterStarting(this.CurrentMission);
			}
			Utilities.SetLoadingScreenPercentage(0.48f);
			this.CurrentMission.AfterStart();
			Utilities.SetLoadingScreenPercentage(0.56f);
			IMissionSystemHandler handler2 = this.Handler;
			if (handler2 != null)
			{
				handler2.OnMissionLoadingFinished(this.CurrentMission);
			}
			Utilities.SetLoadingScreenPercentage(0.6f);
			this.CurrentMission.AfterMissionLoadingFinished();
			Utilities.SetLoadingScreenPercentage(0.62f);
			this.CurrentMission.Scene.ResumeLoadingRenderings();
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x00077B14 File Offset: 0x00075D14
		public void BeginDelayedDisconnectFromMission()
		{
			this._isDelayedDisconnecting = true;
		}

		// Token: 0x04000CF8 RID: 3320
		private const int MissionFastForwardSpeedMultiplier = 10;

		// Token: 0x04000CF9 RID: 3321
		private bool _missionInitializing;

		// Token: 0x04000CFA RID: 3322
		private int _tickCountBeforeLoad;

		// Token: 0x04000CFB RID: 3323
		public static bool RecordMission;

		// Token: 0x04000CFD RID: 3325
		public float MissionReplayStartTime;

		// Token: 0x04000CFE RID: 3326
		public float MissionEndTime;

		// Token: 0x04000D04 RID: 3332
		private bool _isDelayedDisconnecting;

		// Token: 0x04000D05 RID: 3333
		private int _missionTickCount;
	}
}
