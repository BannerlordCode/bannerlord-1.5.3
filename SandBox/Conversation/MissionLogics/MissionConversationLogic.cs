using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Conversation.MissionLogics
{
	// Token: 0x020000CE RID: 206
	public class MissionConversationLogic : MissionLogic
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0003CDBA File Offset: 0x0003AFBA
		public static MissionConversationLogic Current
		{
			get
			{
				return Mission.Current.GetMissionBehavior<MissionConversationLogic>();
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0003CDC6 File Offset: 0x0003AFC6
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0003CDCE File Offset: 0x0003AFCE
		public MissionState State { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0003CDD7 File Offset: 0x0003AFD7
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x0003CDDF File Offset: 0x0003AFDF
		public ConversationManager ConversationManager { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0003CDE8 File Offset: 0x0003AFE8
		public bool IsReadyForConversation
		{
			get
			{
				return this.ConversationAgent != null && this.ConversationManager != null && Agent.Main != null && Agent.Main.IsActive();
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0003CE0F File Offset: 0x0003B00F
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x0003CE17 File Offset: 0x0003B017
		public Agent ConversationAgent { get; private set; }

		// Token: 0x06000878 RID: 2168 RVA: 0x0003CE20 File Offset: 0x0003B020
		public MissionConversationLogic(CharacterObject teleportNearChar)
		{
			this._teleportNearCharacter = teleportNearChar;
			this._conversationPoints = new Dictionary<string, MBList<GameEntity>>();
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0003CE45 File Offset: 0x0003B045
		public MissionConversationLogic()
		{
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0003CE58 File Offset: 0x0003B058
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			CampaignEvents.LocationCharactersSimulatedEvent.AddNonSerializedListener(this, new Action(this.OnLocationCharactersSimulated));
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0003CE77 File Offset: 0x0003B077
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.ClearListeners(this);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0003CE8C File Offset: 0x0003B08C
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (this._teleportNearCharacter != null && agent.Character == this._teleportNearCharacter)
			{
				this.ConversationAgent = agent;
				this._conversationAgentFound = true;
			}
			if (agent.IsHuman)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				if (characterObject != null && characterObject.Culture.FemaleDancer == characterObject)
				{
					this._uninteractableAgents.Add(agent);
				}
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0003CEEF File Offset: 0x0003B0EF
		public void SetSpawnArea(Alley alley)
		{
			this._customSpawnTag = alley.Tag;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0003CEFD File Offset: 0x0003B0FD
		public void SetSpawnArea(Workshop workshop)
		{
			this._customSpawnTag = workshop.Tag;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0003CF0B File Offset: 0x0003B10B
		public void SetSpawnArea(string customTag)
		{
			this._customSpawnTag = customTag;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0003CF14 File Offset: 0x0003B114
		private void OnLocationCharactersSimulated()
		{
			if (this._conversationAgentFound)
			{
				if (this.FillConversationPointList())
				{
					this.DetermineSpawnPoint();
					this._teleported = this.TryToTeleportBothToCertainPoints();
					return;
				}
				MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
				if (missionBehavior == null)
				{
					return;
				}
				missionBehavior.TeleportTargetAgentNearReferenceAgent(this.ConversationAgent, Agent.Main, true, false);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0003CF68 File Offset: 0x0003B168
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this.IsReadyForConversation)
			{
				return;
			}
			if (!this._teleported)
			{
				base.Mission.GetMissionBehavior<MissionAgentHandler>().TeleportTargetAgentNearReferenceAgent(this.ConversationAgent, Agent.Main, true, false);
				this._teleported = true;
			}
			if (this._teleportNearCharacter != null && !this._conversationStarted)
			{
				this.StartConversation(this.ConversationAgent, true, true);
				if (this.ConversationManager.NeedsToActivateForMapConversation && !GameNetwork.IsReplay)
				{
					this.ConversationManager.BeginConversation();
				}
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0003CFF0 File Offset: 0x0003B1F0
		private bool TryToTeleportBothToCertainPoints()
		{
			bool missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>() != null;
			bool flag = Agent.Main.MountAgent != null;
			WorldFrame worldFrame = new WorldFrame(this._selectedConversationPoint.GetGlobalFrame().rotation, new WorldPosition(Agent.Main.Mission.Scene, this._selectedConversationPoint.GetGlobalFrame().origin));
			worldFrame.Origin.SetVec2(worldFrame.Origin.AsVec2 + worldFrame.Rotation.f.AsVec2 * (flag ? 1f : 0.5f));
			WorldFrame worldFrame2 = new WorldFrame(this._selectedConversationPoint.GetGlobalFrame().rotation, new WorldPosition(Agent.Main.Mission.Scene, this._selectedConversationPoint.GetGlobalFrame().origin));
			worldFrame2.Origin.SetVec2(worldFrame2.Origin.AsVec2 - worldFrame2.Rotation.f.AsVec2 * (flag ? 1f : 0.5f));
			Vec3 vec = new Vec3(worldFrame.Origin.AsVec2 - worldFrame2.Origin.AsVec2, 0f, -1f);
			Vec3 vec2 = new Vec3(worldFrame2.Origin.AsVec2 - worldFrame.Origin.AsVec2, 0f, -1f);
			worldFrame.Rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			this.ConversationAgent.LookDirection = vec2.NormalizedCopy();
			this.ConversationAgent.TeleportToPosition(worldFrame.Origin.GetGroundVec3());
			worldFrame2.Rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			if (Agent.Main.MountAgent != null)
			{
				Vec2 vec3 = vec2.AsVec2;
				vec3 = vec3.RightVec();
				Vec3 vec4 = vec3.ToVec3(0f);
				Agent.Main.MountAgent.LookDirection = vec4.NormalizedCopy();
			}
			base.Mission.MainAgent.LookDirection = vec.NormalizedCopy();
			base.Mission.MainAgent.TeleportToPosition(worldFrame2.Origin.GetGroundVec3());
			this.SetConversationAgentAnimations(this.ConversationAgent);
			WorldPosition origin = worldFrame2.Origin;
			origin.SetVec2(origin.AsVec2 - worldFrame2.Rotation.s.AsVec2 * 2f);
			if (missionBehavior)
			{
				foreach (Agent agent in base.Mission.Agents)
				{
					LocationCharacter locationCharacter = LocationComplex.Current.FindCharacter(agent);
					AccompanyingCharacter accompanyingCharacter = PlayerEncounter.LocationEncounter.GetAccompanyingCharacter(locationCharacter);
					if (accompanyingCharacter != null && accompanyingCharacter.IsFollowingPlayerAtMissionStart)
					{
						if (agent.MountAgent != null && Agent.Main.MountAgent != null)
						{
							agent.MountAgent.LookDirection = Agent.Main.MountAgent.LookDirection;
						}
						if (accompanyingCharacter.LocationCharacter.Character == this._teleportNearCharacter)
						{
							agent.LookDirection = vec2.NormalizedCopy();
							Agent agent2 = agent;
							Vec2 vec3 = worldFrame2.Rotation.f.AsVec2;
							agent2.SetMovementDirection(in vec3);
							agent.TeleportToPosition(worldFrame.Origin.GetGroundVec3());
						}
						else
						{
							agent.LookDirection = vec.NormalizedCopy();
							Agent agent3 = agent;
							Vec2 vec3 = worldFrame.Rotation.f.AsVec2;
							agent3.SetMovementDirection(in vec3);
							agent.TeleportToPosition(origin.GetGroundVec3());
						}
					}
				}
			}
			this._teleported = true;
			return true;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0003D3B4 File Offset: 0x0003B5B4
		private void SetConversationAgentAnimations(Agent conversationAgent)
		{
			CampaignAgentComponent component = conversationAgent.GetComponent<CampaignAgentComponent>();
			AgentNavigator agentNavigator = component.AgentNavigator;
			AgentBehavior agentBehavior = ((agentNavigator != null) ? agentNavigator.GetActiveBehavior() : null);
			if (agentBehavior != null)
			{
				agentBehavior.IsActive = false;
				component.AgentNavigator.ForceThink(0f);
				conversationAgent.SetActionChannel(0, in ActionIndexCache.act_none, false, (AnimFlags)((long)conversationAgent.GetCurrentActionPriority(0)), 0f, 1f, 0f, 0.4f, 0f, false, -0.2f, 0, true);
				conversationAgent.SetActionChannel(0, in ActionIndexCache.act_none, false, (AnimFlags)((long)Math.Min(conversationAgent.GetCurrentActionPriority(0), 73)), 0f, 1f, 0f, 0.4f, 0f, false, -0.2f, 0, true);
				conversationAgent.TickActionChannels(0.1f);
				conversationAgent.AgentVisuals.GetSkeleton().TickAnimationsAndForceUpdate(0.1f, conversationAgent.AgentVisuals.GetGlobalFrame(), true);
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0003D49C File Offset: 0x0003B69C
		private void OnConversationEnd()
		{
			foreach (IAgent agent in this.ConversationManager.ConversationAgents)
			{
				Agent agent2 = (Agent)agent;
				agent2.AgentVisuals.SetVisible(true);
				agent2.AgentVisuals.SetClothComponentKeepStateOfAllMeshes(false);
				Agent mountAgent = agent2.MountAgent;
				if (mountAgent != null)
				{
					mountAgent.AgentVisuals.SetVisible(true);
				}
			}
			if (base.Mission.Mode == MissionMode.Conversation && !base.Mission.IsMissionEnding)
			{
				base.Mission.SetMissionMode(this._oldMissionMode, false);
			}
			if (Agent.Main != null)
			{
				Agent.Main.AgentVisuals.SetVisible(true);
				Agent.Main.AgentVisuals.SetClothComponentKeepStateOfAllMeshes(false);
				if (Agent.Main.MountAgent != null)
				{
					Agent.Main.MountAgent.AgentVisuals.SetVisible(true);
				}
			}
			base.Mission.MainAgentServer.Controller = AgentControllerType.Player;
			this.ConversationManager.ConversationEnd -= this.OnConversationEnd;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0003D5B8 File Offset: 0x0003B7B8
		public override void EarlyStart()
		{
			this.State = Game.Current.GameStateManager.ActiveState as MissionState;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0003D5D4 File Offset: 0x0003B7D4
		protected override void OnEndMission()
		{
			if (this.ConversationManager != null && this.ConversationManager.IsConversationInProgress)
			{
				this.ConversationManager.EndConversation();
			}
			this.State = null;
			CampaignEvents.LocationCharactersSimulatedEvent.ClearListeners(this);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0003D608 File Offset: 0x0003B808
		public override void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && this.IsThereAgentAction(userAgent, agent) && !this.ConversationManager.IsConversationInProgress)
			{
				this.StartConversation(agent, false, false);
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0003D638 File Offset: 0x0003B838
		public void StartConversation(Agent agent, bool setActionsInstantly, bool isInitialization = false)
		{
			this._oldMissionMode = base.Mission.Mode;
			this.ConversationManager = Campaign.Current.ConversationManager;
			this.ConversationManager.SetupAndStartMissionConversation(agent, base.Mission.MainAgent, setActionsInstantly);
			this.ConversationManager.ConversationEnd += this.OnConversationEnd;
			this._conversationStarted = true;
			foreach (IAgent agent2 in this.ConversationManager.ConversationAgents)
			{
				Agent agent3 = (Agent)agent2;
				agent3.ForceAiBehaviorSelection();
				agent3.AgentVisuals.SetClothComponentKeepStateOfAllMeshes(true);
			}
			base.Mission.MainAgentServer.AgentVisuals.SetClothComponentKeepStateOfAllMeshes(true);
			base.Mission.SetMissionMode(MissionMode.Conversation, setActionsInstantly);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0003D714 File Offset: 0x0003B914
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return base.Mission.Mode != MissionMode.Battle && base.Mission.Mode != MissionMode.Duel && base.Mission.Mode != MissionMode.Conversation && otherAgent.IsHuman && !this._disableStartConversation && otherAgent.IsActive() && !otherAgent.IsEnemyOf(userAgent) && otherAgent.GetDistanceTo(Agent.Main) > 0.2f && otherAgent.GetDistanceTo(Agent.Main) < 2f && !this._uninteractableAgents.Contains(otherAgent) && (!otherAgent.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_enforce_lowerbody | AnimFlags.anf_enforce_all | AnimFlags.anf_enforce_root_rotation) || Math.Abs(userAgent.LookDirection.AsVec2.AngleBetween(otherAgent.LookDirection.AsVec2) * 57.295776f) > 45f);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0003D804 File Offset: 0x0003BA04
		public override void OnRenderingStarted()
		{
			this.ConversationManager = Campaign.Current.ConversationManager;
			if (this.ConversationManager == null)
			{
				throw new ArgumentNullException("conversationManager");
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0003D829 File Offset: 0x0003BA29
		public void DisableStartConversation(bool isDisabled)
		{
			this._disableStartConversation = isDisabled;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0003D834 File Offset: 0x0003BA34
		private bool FillConversationPointList()
		{
			List<GameEntity> list = base.Mission.Scene.FindEntitiesWithTag("sp_player_conversation").ToList<GameEntity>();
			bool flag = false;
			if (!list.IsEmpty<GameEntity>())
			{
				List<AreaMarker> list2 = base.Mission.ActiveMissionObjects.FindAllWithType<AreaMarker>().ToList<AreaMarker>();
				foreach (GameEntity gameEntity in list)
				{
					bool flag2 = false;
					foreach (AreaMarker areaMarker in list2)
					{
						if (areaMarker.IsPositionInRange(gameEntity.GlobalPosition))
						{
							if (this._conversationPoints.ContainsKey(areaMarker.Tag))
							{
								this._conversationPoints[areaMarker.Tag].Add(gameEntity);
							}
							else
							{
								this._conversationPoints.Add(areaMarker.Tag, new MBList<GameEntity> { gameEntity });
							}
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						if (this._conversationPoints.ContainsKey("CenterConversationPoint"))
						{
							this._conversationPoints["CenterConversationPoint"].Add(gameEntity);
						}
						else
						{
							this._conversationPoints.Add("CenterConversationPoint", new MBList<GameEntity> { gameEntity });
						}
					}
				}
				flag = true;
			}
			else
			{
				Debug.FailedAssert("Scene must have at least one 'sp_player_conversation' game entity. Scene Name: " + Mission.Current.Scene.GetName(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Conversation\\Logics\\MissionConversationLogic.cs", "FillConversationPointList", 404);
			}
			return flag;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0003D9DC File Offset: 0x0003BBDC
		private void DetermineSpawnPoint()
		{
			MBList<GameEntity> mblist;
			if (this._customSpawnTag != null && this._conversationPoints.TryGetValue(this._customSpawnTag, out mblist))
			{
				this._selectedConversationPoint = mblist.GetRandomElement<GameEntity>();
				return;
			}
			string agentsTag = this.ConversationAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag;
			if (agentsTag != null)
			{
				MBList<GameEntity> value = this._conversationPoints.FirstOrDefault<KeyValuePair<string, MBList<GameEntity>>>((KeyValuePair<string, MBList<GameEntity>> x) => agentsTag.Contains(x.Key)).Value;
				this._selectedConversationPoint = ((value != null) ? value.GetRandomElement<GameEntity>() : null);
			}
			if (this._selectedConversationPoint == null)
			{
				if (this._conversationPoints.ContainsKey("CenterConversationPoint"))
				{
					this._selectedConversationPoint = this._conversationPoints["CenterConversationPoint"].GetRandomElement<GameEntity>();
					return;
				}
				this._selectedConversationPoint = this._conversationPoints.GetRandomElementInefficiently<KeyValuePair<string, MBList<GameEntity>>>().Value.GetRandomElement<GameEntity>();
			}
		}

		// Token: 0x04000448 RID: 1096
		private const string CenterConversationPointMappingTag = "CenterConversationPoint";

		// Token: 0x04000449 RID: 1097
		private const int StartingConversationFromBehindAngleThresholdInDegrees = 45;

		// Token: 0x0400044A RID: 1098
		private const float MinDistanceThresholdToOpenConversation = 0.2f;

		// Token: 0x0400044B RID: 1099
		private const float MaxDistanceThresholdToOpenConversation = 2f;

		// Token: 0x0400044E RID: 1102
		private MissionMode _oldMissionMode;

		// Token: 0x0400044F RID: 1103
		private readonly CharacterObject _teleportNearCharacter;

		// Token: 0x04000450 RID: 1104
		private GameEntity _selectedConversationPoint;

		// Token: 0x04000451 RID: 1105
		private bool _conversationStarted;

		// Token: 0x04000452 RID: 1106
		private bool _teleported;

		// Token: 0x04000453 RID: 1107
		private bool _conversationAgentFound;

		// Token: 0x04000454 RID: 1108
		private bool _disableStartConversation;

		// Token: 0x04000455 RID: 1109
		private readonly Dictionary<string, MBList<GameEntity>> _conversationPoints;

		// Token: 0x04000456 RID: 1110
		private string _customSpawnTag;

		// Token: 0x04000457 RID: 1111
		private HashSet<Agent> _uninteractableAgents = new HashSet<Agent>();
	}
}
