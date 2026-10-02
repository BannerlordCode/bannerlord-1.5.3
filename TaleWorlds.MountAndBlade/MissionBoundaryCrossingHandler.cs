using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000292 RID: 658
	public class MissionBoundaryCrossingHandler : MissionLogic
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x060024C1 RID: 9409 RVA: 0x00085CF4 File Offset: 0x00083EF4
		// (remove) Token: 0x060024C2 RID: 9410 RVA: 0x00085D2C File Offset: 0x00083F2C
		public event Action<float, float> StartTime;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x060024C3 RID: 9411 RVA: 0x00085D64 File Offset: 0x00083F64
		// (remove) Token: 0x060024C4 RID: 9412 RVA: 0x00085D9C File Offset: 0x00083F9C
		public event Action StopTime;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x060024C5 RID: 9413 RVA: 0x00085DD4 File Offset: 0x00083FD4
		// (remove) Token: 0x060024C6 RID: 9414 RVA: 0x00085E0C File Offset: 0x0008400C
		public event Action<float> TimeCount;

		// Token: 0x060024C7 RID: 9415 RVA: 0x00085E41 File Offset: 0x00084041
		public MissionBoundaryCrossingHandler(float leewayTime = 10f)
		{
			this._leewayTime = leewayTime;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00085E50 File Offset: 0x00084050
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (GameNetwork.IsSessionActive)
			{
				this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			}
			if (GameNetwork.IsServer)
			{
				this._agentTimers = new Dictionary<Agent, MissionTimer>();
				this._agentsToPunish = new List<Agent>();
			}
			this._vehicleHandler = base.Mission.GetMissionBehavior<IVehicleHandler>();
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00085E9F File Offset: 0x0008409F
		public override void OnRemoveBehavior()
		{
			if (GameNetwork.IsSessionActive)
			{
				this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
			base.OnRemoveBehavior();
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00085EB8 File Offset: 0x000840B8
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (GameNetwork.IsClient)
			{
				networkMessageHandlerRegisterer.Register<SetBoundariesState>(new GameNetworkMessage.ServerMessageHandlerDelegate<SetBoundariesState>(this.HandleServerEventSetPeerBoundariesState));
			}
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00085EE8 File Offset: 0x000840E8
		private void OnAgentWentOut(Agent agent, float startTimeInSeconds)
		{
			MissionTimer missionTimer = (GameNetwork.IsClient ? MissionTimer.CreateSynchedTimerClient(startTimeInSeconds, this._leewayTime) : new MissionTimer(this._leewayTime));
			if (GameNetwork.IsServer)
			{
				this._agentTimers.Add(agent, missionTimer);
				MissionPeer missionPeer = agent.MissionPeer;
				NetworkCommunicator networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
				if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator);
					GameNetwork.WriteMessage(new SetBoundariesState(true, missionTimer.GetStartTime().NumberOfTicks));
					GameNetwork.EndModuleEventAsServer();
				}
			}
			if (base.Mission.MainAgent == agent)
			{
				this._mainAgentLeaveTimer = missionTimer;
				Action<float, float> startTime = this.StartTime;
				if (startTime != null)
				{
					startTime(this._leewayTime, 0f);
				}
				MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
				Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
				if (Mission.Current.Mode == MissionMode.Battle)
				{
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/out_of_map"), vec);
				}
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00085FE0 File Offset: 0x000841E0
		private void OnAgentWentInOrRemoved(Agent agent, bool isAgentRemoved)
		{
			if (GameNetwork.IsServer)
			{
				this._agentTimers.Remove(agent);
				if (!isAgentRemoved)
				{
					MissionPeer missionPeer = agent.MissionPeer;
					NetworkCommunicator networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
					if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkCommunicator);
						GameNetwork.WriteMessage(new SetBoundariesState(false));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
			if (base.Mission.MainAgent == agent)
			{
				this._mainAgentLeaveTimer = null;
				Action stopTime = this.StopTime;
				if (stopTime == null)
				{
					return;
				}
				stopTime();
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00086060 File Offset: 0x00084260
		private void HandleAgentPunishmentsServer()
		{
			foreach (Agent agent in this._agentsToPunish)
			{
				Blow blow = new Blow(agent.Index);
				blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, 0);
				blow.DamageType = DamageTypes.Blunt;
				blow.BaseMagnitude = 10000f;
				blow.WeaponRecord.WeaponClass = WeaponClass.Undefined;
				blow.GlobalPosition = agent.Position;
				blow.DamagedPercentage = 1f;
				agent.Die(blow, Agent.KillInfo.Invalid);
			}
			this._agentsToPunish.Clear();
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00086118 File Offset: 0x00084318
		private void DecideOrHandleAgentPunishment(Agent agent)
		{
			if (GameNetwork.IsSessionActive)
			{
				if (GameNetwork.IsServer)
				{
					this._agentsToPunish.Add(agent);
					if (agent.MountAgent != null)
					{
						this._agentsToPunish.Add(agent.MountAgent);
						return;
					}
				}
			}
			else
			{
				base.Mission.RetreatMission();
			}
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x00086164 File Offset: 0x00084364
		public override void OnClearScene()
		{
			if (GameNetwork.IsServer)
			{
				using (List<Agent>.Enumerator enumerator = this._agentTimers.Keys.ToList<Agent>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						this.OnAgentWentInOrRemoved(agent, true);
					}
					return;
				}
			}
			if (this._mainAgentLeaveTimer != null)
			{
				if (base.Mission.MainAgent != null)
				{
					this.OnAgentWentInOrRemoved(base.Mission.MainAgent, true);
					return;
				}
				this._mainAgentLeaveTimer = null;
			}
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000861F8 File Offset: 0x000843F8
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			this.OnAgentWentInOrRemoved(affectedAgent, true);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00086204 File Offset: 0x00084404
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (GameNetwork.IsServer)
			{
				for (int i = base.Mission.Agents.Count - 1; i >= 0; i--)
				{
					Agent agent = base.Mission.Agents[i];
					if (agent.MissionPeer != null)
					{
						this.TickForAgentAsServer(agent);
					}
				}
				this.HandleAgentPunishmentsServer();
			}
			else if (!GameNetwork.IsSessionActive && Agent.Main != null)
			{
				this.TickForMainAgent();
			}
			if (this._mainAgentLeaveTimer != null)
			{
				this._mainAgentLeaveTimer.Check(false);
				float num = 1f - this._mainAgentLeaveTimer.GetRemainingTimeInSeconds(true) / this._leewayTime;
				Action<float> timeCount = this.TimeCount;
				if (timeCount == null)
				{
					return;
				}
				timeCount(num);
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000862BC File Offset: 0x000844BC
		private void TickForMainAgent()
		{
			WeakGameEntity weakGameEntity;
			bool flag;
			if (this._vehicleHandler != null && this._vehicleHandler.IsAgentInVehicle(Agent.Main, out weakGameEntity))
			{
				flag = !base.Mission.IsPositionInsideBoundaries(weakGameEntity.GlobalPosition.AsVec2);
			}
			else
			{
				flag = !base.Mission.IsPositionInsideBoundaries(Agent.Main.Position.AsVec2);
			}
			bool flag2 = this._mainAgentLeaveTimer != null;
			this.HandleAgentStateChange(Agent.Main, flag, flag2, this._mainAgentLeaveTimer);
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00086344 File Offset: 0x00084544
		private void TickForAgentAsServer(Agent agent)
		{
			bool flag = !base.Mission.IsPositionInsideBoundaries(agent.Position.AsVec2);
			bool flag2 = this._agentTimers.ContainsKey(agent);
			this.HandleAgentStateChange(agent, flag, flag2, flag2 ? this._agentTimers[agent] : null);
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00086396 File Offset: 0x00084596
		private void HandleAgentStateChange(Agent agent, bool isAgentOutside, bool isTimerActiveForAgent, MissionTimer timerInstance)
		{
			if (isAgentOutside && !isTimerActiveForAgent)
			{
				this.OnAgentWentOut(agent, 0f);
				return;
			}
			if (!isAgentOutside && isTimerActiveForAgent)
			{
				this.OnAgentWentInOrRemoved(agent, false);
				return;
			}
			if (isAgentOutside && timerInstance.Check(false))
			{
				this.DecideOrHandleAgentPunishment(agent);
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000863D0 File Offset: 0x000845D0
		private void HandleServerEventSetPeerBoundariesState(SetBoundariesState message)
		{
			if (message.IsOutside)
			{
				this.OnAgentWentOut(base.Mission.MainAgent, message.StateStartTimeInSeconds);
				return;
			}
			this.OnAgentWentInOrRemoved(base.Mission.MainAgent, false);
		}

		// Token: 0x04000E35 RID: 3637
		private float _leewayTime;

		// Token: 0x04000E39 RID: 3641
		private List<Agent> _agentsToPunish;

		// Token: 0x04000E3A RID: 3642
		private Dictionary<Agent, MissionTimer> _agentTimers;

		// Token: 0x04000E3B RID: 3643
		private MissionTimer _mainAgentLeaveTimer;

		// Token: 0x04000E3C RID: 3644
		private IVehicleHandler _vehicleHandler;
	}
}
