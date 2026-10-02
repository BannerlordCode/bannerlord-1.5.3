using System;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000018 RID: 24
	public class MissionConversationCameraView : MissionView
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000553B File Offset: 0x0000373B
		public bool IsCameraOverridden
		{
			get
			{
				return this._customConversationCamera != null;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005549 File Offset: 0x00003749
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._conversationMissionLogic = base.Mission.GetMissionBehavior<ConversationMissionLogic>();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005562 File Offset: 0x00003762
		public override void AfterStart()
		{
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._customMultiAgentConversationCameraEntity = base.Mission.Scene.FindEntityWithTag("custom_camera_multi_agent");
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005590 File Offset: 0x00003790
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._conversationMissionLogic != null && this._conversationMissionLogic.IsMultiAgentConversation)
			{
				ConversationManager conversationManager = Campaign.Current.ConversationManager;
				if (conversationManager.ConversationAgents != null && conversationManager.ConversationAgents.Count > 0)
				{
					Agent agent = (Agent)conversationManager.SpeakerAgent;
					Agent agent2 = (Agent)conversationManager.ListenerAgent;
					if (this._speakerAgent != null && this._listenerAgent != null)
					{
						this._speakerAgent = agent;
						this._listenerAgent = agent2;
						if (this._speakerAgent != Agent.Main && this._listenerAgent != Agent.Main)
						{
							if (base.MissionScreen.CustomCamera == null)
							{
								GameEntity customMultiAgentConversationCameraEntity = this._customMultiAgentConversationCameraEntity;
								Vec3 invalid = Vec3.Invalid;
								Camera camera = Camera.CreateCamera();
								customMultiAgentConversationCameraEntity.GetCameraParamsFromCameraScript(camera, ref invalid);
								camera.SetFovVertical(camera.GetFovVertical(), Screen.AspectRatio, camera.Near, camera.Far);
								base.MissionScreen.CustomCamera = camera;
							}
							this.UpdateAgentLooksForConversation();
							return;
						}
						base.MissionScreen.CustomCamera = null;
					}
				}
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000056AC File Offset: 0x000038AC
		public override bool UpdateOverridenCamera(float dt)
		{
			MissionMode mode = base.Mission.Mode;
			if ((mode == MissionMode.Conversation || mode == MissionMode.Barter) && !base.MissionScreen.IsCheatGhostMode)
			{
				ConversationMissionLogic conversationMissionLogic = this._conversationMissionLogic;
				if (((conversationMissionLogic != null) ? conversationMissionLogic.CustomConversationCameraEntity : null) != null)
				{
					if (this._customConversationCamera == null)
					{
						ConversationMissionLogic conversationMissionLogic2 = this._conversationMissionLogic;
						if (((conversationMissionLogic2 != null) ? conversationMissionLogic2.CustomConversationCameraEntity : null) != null)
						{
							Vec3 invalid = Vec3.Invalid;
							this._customConversationCamera = Camera.CreateCamera();
							ConversationMissionLogic conversationMissionLogic3 = this._conversationMissionLogic;
							if (conversationMissionLogic3 != null)
							{
								conversationMissionLogic3.CustomConversationCameraEntity.GetCameraParamsFromCameraScript(this._customConversationCamera, ref invalid);
							}
							this._customConversationCamera.SetFovVertical(this._customConversationCamera.GetFovVertical(), Screen.AspectRatio, this._customConversationCamera.Near, this._customConversationCamera.Far);
						}
					}
					this.SetConversationLookToPointOfInterest(this._customConversationCamera.Position);
					base.MissionScreen.CustomCamera = this._customConversationCamera;
				}
				else
				{
					this.UpdateAgentLooksForConversation();
				}
			}
			else if (this._missionMainAgentController.CustomLookDir.IsNonZero)
			{
				this._missionMainAgentController.CustomLookDir = Vec3.Zero;
			}
			return false;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000057D8 File Offset: 0x000039D8
		private void UpdateAgentLooksForConversation()
		{
			Agent agent = null;
			ConversationManager conversationManager = Campaign.Current.ConversationManager;
			if (conversationManager.ConversationAgents != null && conversationManager.ConversationAgents.Count > 0)
			{
				this._speakerAgent = (Agent)conversationManager.SpeakerAgent;
				this._listenerAgent = (Agent)conversationManager.ListenerAgent;
				agent = Agent.Main.GetLookAgent();
				if (this._speakerAgent == null)
				{
					return;
				}
				foreach (IAgent agent2 in conversationManager.ConversationAgents)
				{
					if (agent2 != this._speakerAgent)
					{
						this.MakeAgentLookToSpeaker((Agent)agent2);
					}
				}
				this.MakeSpeakerLookToListener();
			}
			this.SetFocusedObjectForCameraFocus();
			if (Agent.Main.GetLookAgent() != agent && this._speakerAgent != null)
			{
				this.SpeakerAgentIsChanged();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000058B8 File Offset: 0x00003AB8
		private void SpeakerAgentIsChanged()
		{
			Mission.Current.ConversationCharacterChanged();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000058C4 File Offset: 0x00003AC4
		private void SetFocusedObjectForCameraFocus()
		{
			if (base.MissionScreen.CustomCamera != null)
			{
				Agent agent = this._speakerAgent;
				Agent agent2 = this._listenerAgent;
				MatrixFrame identity = MatrixFrame.Identity;
				Vec3 vec = agent2.Position - agent.Position;
				vec.RotateAboutZ(-1.0471976f);
				vec += agent.Position;
				Vec3 vec2 = agent.Position - agent2.Position;
				vec2.RotateAboutZ(-1.0471976f);
				vec2 += agent2.Position;
				if (vec.Distance(Agent.Main.Position) > vec2.Distance(Agent.Main.Position))
				{
					agent = this._listenerAgent;
					agent2 = this._speakerAgent;
					vec = vec2;
				}
				vec.z += Agent.Main.GetEyeGlobalHeight();
				Vec3 vec3 = -((agent2.Position - agent.Position) / 2f + agent.Position - vec).NormalizedCopy();
				identity.origin = vec;
				identity.rotation.s = Vec3.Side;
				identity.rotation.f = Vec3.Up;
				identity.rotation.u = vec3;
				identity.rotation.Orthonormalize();
				base.MissionScreen.CustomCamera.SetFovHorizontal(1.5707964f, Screen.AspectRatio, 0.1f, 2000f);
				base.MissionScreen.CustomCamera.Frame = identity;
				Agent.Main.AgentVisuals.SetVisible(false);
				return;
			}
			if (this._speakerAgent == Agent.Main)
			{
				this._missionMainAgentController.InteractionComponent.SetCurrentFocusedObject(this._listenerAgent, null, -1, true);
				this._missionMainAgentController.CustomLookDir = (this._listenerAgent.Position - Agent.Main.Position).NormalizedCopy();
				Agent.Main.SetLookAgent(this._listenerAgent);
				return;
			}
			this._missionMainAgentController.InteractionComponent.SetCurrentFocusedObject(this._speakerAgent, null, -1, true);
			this._missionMainAgentController.CustomLookDir = (this._speakerAgent.Position - Agent.Main.Position).NormalizedCopy();
			Agent.Main.SetLookAgent(this._speakerAgent);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005B20 File Offset: 0x00003D20
		private void MakeAgentLookToSpeaker(Agent agent)
		{
			Vec3 position = agent.Position;
			Vec3 position2 = this._speakerAgent.Position;
			position.z = agent.AgentVisuals.GetGlobalStableEyePoint(true).z;
			position2.z = this._speakerAgent.AgentVisuals.GetGlobalStableEyePoint(true).z;
			agent.SetLookToPointOfInterest(this._speakerAgent.AgentVisuals.GetGlobalStableEyePoint(true));
			agent.AgentVisuals.GetSkeleton().ForceUpdateBoneFrames();
			agent.LookDirection = (position2 - position).NormalizedCopy();
			agent.SetLookAgent(this._speakerAgent);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005BC0 File Offset: 0x00003DC0
		private void MakeSpeakerLookToListener()
		{
			Vec3 position = this._speakerAgent.Position;
			Vec3 position2 = this._listenerAgent.Position;
			position.z = this._speakerAgent.AgentVisuals.GetGlobalStableEyePoint(true).z;
			position2.z = this._listenerAgent.AgentVisuals.GetGlobalStableEyePoint(true).z;
			this._speakerAgent.SetLookToPointOfInterest(this._listenerAgent.AgentVisuals.GetGlobalStableEyePoint(true));
			this._speakerAgent.AgentVisuals.GetSkeleton().ForceUpdateBoneFrames();
			this._speakerAgent.LookDirection = (position2 - position).NormalizedCopy();
			this._speakerAgent.SetLookAgent(this._listenerAgent);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005C7C File Offset: 0x00003E7C
		private void SetConversationLookToPointOfInterest(Vec3 pointOfInterest)
		{
			ConversationManager conversationManager = Campaign.Current.ConversationManager;
			if (conversationManager.ConversationAgents != null && conversationManager.ConversationAgents.Count > 0)
			{
				this._speakerAgent = (Agent)conversationManager.SpeakerAgent;
				this._listenerAgent = (Agent)conversationManager.ListenerAgent;
				this.MakeAgentLookToSpeaker(this._listenerAgent);
				this.MakeSpeakerLookToListener();
				Agent.Main.TeleportToPosition(pointOfInterest);
				Agent.Main.AgentVisuals.SetVisible(false);
				this.MakeAgentLookToSpeaker(Agent.Main);
			}
		}

		// Token: 0x0400002B RID: 43
		private const string CustomCameraMultiAgentTag = "custom_camera_multi_agent";

		// Token: 0x0400002C RID: 44
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x0400002D RID: 45
		private ConversationMissionLogic _conversationMissionLogic;

		// Token: 0x0400002E RID: 46
		private Camera _customConversationCamera;

		// Token: 0x0400002F RID: 47
		private GameEntity _customMultiAgentConversationCameraEntity;

		// Token: 0x04000030 RID: 48
		private Agent _speakerAgent;

		// Token: 0x04000031 RID: 49
		private Agent _listenerAgent;
	}
}
