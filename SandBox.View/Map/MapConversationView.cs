using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x0200004C RID: 76
	public class MapConversationView : MapView
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000280 RID: 640 RVA: 0x000175CF File Offset: 0x000157CF
		// (set) Token: 0x06000281 RID: 641 RVA: 0x000175D7 File Offset: 0x000157D7
		public bool IsConversationActive { get; protected set; }

		// Token: 0x06000282 RID: 642 RVA: 0x000175E0 File Offset: 0x000157E0
		protected internal virtual void InitializeConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000175E2 File Offset: 0x000157E2
		protected internal override void OnFinalize()
		{
			base.OnFinalize();
			this.DestroyConversationMission();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000175F0 File Offset: 0x000157F0
		protected internal virtual void FinalizeConversation()
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000175F4 File Offset: 0x000157F4
		protected void CreateConversationMissionIfMissing()
		{
			MapConversationView.MapConversationMission mapConversationMission;
			if ((mapConversationMission = CampaignMission.Current as MapConversationView.MapConversationMission) != null)
			{
				this.ConversationMission = mapConversationMission;
				return;
			}
			this.ConversationMission = new MapConversationView.MapConversationMission();
			CampaignMission.Current = this.ConversationMission;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0001762D File Offset: 0x0001582D
		protected void DestroyConversationMission()
		{
			MapConversationView.MapConversationMission conversationMission = this.ConversationMission;
			if (conversationMission != null)
			{
				conversationMission.OnFinalize();
			}
			this.ConversationMission = null;
		}

		// Token: 0x0400015F RID: 351
		public MapConversationView.MapConversationMission ConversationMission;

		// Token: 0x020000AC RID: 172
		public class MapConversationMission : ICampaignMission
		{
			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x060005FD RID: 1533 RVA: 0x0002ABED File Offset: 0x00028DED
			GameState ICampaignMission.State
			{
				get
				{
					return GameStateManager.Current.ActiveState;
				}
			}

			// Token: 0x170000B7 RID: 183
			// (get) Token: 0x060005FE RID: 1534 RVA: 0x0002ABF9 File Offset: 0x00028DF9
			IMissionTroopSupplier ICampaignMission.AgentSupplier
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170000B8 RID: 184
			// (get) Token: 0x060005FF RID: 1535 RVA: 0x0002ABFC File Offset: 0x00028DFC
			// (set) Token: 0x06000600 RID: 1536 RVA: 0x0002AC04 File Offset: 0x00028E04
			Location ICampaignMission.Location { get; set; }

			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x06000601 RID: 1537 RVA: 0x0002AC0D File Offset: 0x00028E0D
			// (set) Token: 0x06000602 RID: 1538 RVA: 0x0002AC15 File Offset: 0x00028E15
			Alley ICampaignMission.LastVisitedAlley { get; set; }

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x06000603 RID: 1539 RVA: 0x0002AC1E File Offset: 0x00028E1E
			MissionMode ICampaignMission.Mode
			{
				get
				{
					return MissionMode.Conversation;
				}
			}

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x06000604 RID: 1540 RVA: 0x0002AC21 File Offset: 0x00028E21
			// (set) Token: 0x06000605 RID: 1541 RVA: 0x0002AC29 File Offset: 0x00028E29
			public MapConversationTableau ConversationTableau { get; private set; }

			// Token: 0x06000606 RID: 1542 RVA: 0x0002AC32 File Offset: 0x00028E32
			public MapConversationMission()
			{
				CampaignMission.Current = this;
				this._conversationPlayQueue = new Queue<MapConversationView.MapConversationMission.ConversationPlayArgs>();
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x0002AC4B File Offset: 0x00028E4B
			public void SetConversationTableau(MapConversationTableau tableau)
			{
				this.ConversationTableau = tableau;
				this.PlayCachedConversations();
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x0002AC5A File Offset: 0x00028E5A
			public void Tick(float dt)
			{
				this.PlayCachedConversations();
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x0002AC62 File Offset: 0x00028E62
			public void OnFinalize()
			{
				this.ConversationTableau = null;
				this._conversationPlayQueue = null;
				CampaignMission.Current = null;
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x0002AC78 File Offset: 0x00028E78
			private void PlayCachedConversations()
			{
				if (this.ConversationTableau != null)
				{
					while (this._conversationPlayQueue.Count > 0)
					{
						MapConversationView.MapConversationMission.ConversationPlayArgs conversationPlayArgs = this._conversationPlayQueue.Dequeue();
						this.ConversationTableau.OnConversationPlay(conversationPlayArgs.IdleActionId, conversationPlayArgs.IdleFaceAnimId, conversationPlayArgs.ReactionId, conversationPlayArgs.ReactionFaceAnimId, conversationPlayArgs.SoundPath);
					}
				}
			}

			// Token: 0x0600060B RID: 1547 RVA: 0x0002ACD2 File Offset: 0x00028ED2
			void ICampaignMission.OnConversationPlay(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath)
			{
				if (this.ConversationTableau != null)
				{
					this.ConversationTableau.OnConversationPlay(idleActionId, idleFaceAnimId, reactionId, reactionFaceAnimId, soundPath);
					return;
				}
				this._conversationPlayQueue.Enqueue(new MapConversationView.MapConversationMission.ConversationPlayArgs(idleActionId, idleFaceAnimId, reactionId, reactionFaceAnimId, soundPath));
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x0002AD06 File Offset: 0x00028F06
			void ICampaignMission.AddAgentFollowing(IAgent agent)
			{
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x0002AD08 File Offset: 0x00028F08
			bool ICampaignMission.AgentLookingAtAgent(IAgent agent1, IAgent agent2)
			{
				return false;
			}

			// Token: 0x0600060E RID: 1550 RVA: 0x0002AD0B File Offset: 0x00028F0B
			bool ICampaignMission.CheckIfAgentCanFollow(IAgent agent)
			{
				return false;
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x0002AD0E File Offset: 0x00028F0E
			bool ICampaignMission.CheckIfAgentCanUnFollow(IAgent agent)
			{
				return false;
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x0002AD11 File Offset: 0x00028F11
			void ICampaignMission.EndMission()
			{
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x0002AD13 File Offset: 0x00028F13
			void ICampaignMission.OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation)
			{
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x0002AD15 File Offset: 0x00028F15
			void ICampaignMission.OnCloseEncounterMenu()
			{
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x0002AD17 File Offset: 0x00028F17
			void ICampaignMission.OnConversationContinue()
			{
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x0002AD19 File Offset: 0x00028F19
			void ICampaignMission.OnConversationEnd(IAgent agent)
			{
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x0002AD1B File Offset: 0x00028F1B
			void ICampaignMission.OnConversationStart(IAgent agent, bool setActionsInstantly)
			{
			}

			// Token: 0x06000616 RID: 1558 RVA: 0x0002AD1D File Offset: 0x00028F1D
			void ICampaignMission.OnProcessSentence()
			{
			}

			// Token: 0x06000617 RID: 1559 RVA: 0x0002AD1F File Offset: 0x00028F1F
			void ICampaignMission.RemoveAgentFollowing(IAgent agent)
			{
			}

			// Token: 0x06000618 RID: 1560 RVA: 0x0002AD21 File Offset: 0x00028F21
			void ICampaignMission.SetMissionMode(MissionMode newMode, bool atStart)
			{
			}

			// Token: 0x06000619 RID: 1561 RVA: 0x0002AD23 File Offset: 0x00028F23
			void ICampaignMission.FadeOutCharacter(CharacterObject characterObject)
			{
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x0002AD25 File Offset: 0x00028F25
			void ICampaignMission.OnGameStateChanged()
			{
				MapConversationTableau conversationTableau = this.ConversationTableau;
				if (conversationTableau != null)
				{
					conversationTableau.RemovePreviousAgentsSoundEvent();
				}
				MapConversationTableau conversationTableau2 = this.ConversationTableau;
				if (conversationTableau2 == null)
				{
					return;
				}
				conversationTableau2.StopConversationSoundEvent();
			}

			// Token: 0x0400036D RID: 877
			private Queue<MapConversationView.MapConversationMission.ConversationPlayArgs> _conversationPlayQueue;

			// Token: 0x020000DC RID: 220
			public struct ConversationPlayArgs
			{
				// Token: 0x060006DD RID: 1757 RVA: 0x0002C261 File Offset: 0x0002A461
				public ConversationPlayArgs(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath)
				{
					this.IdleActionId = idleActionId;
					this.IdleFaceAnimId = idleFaceAnimId;
					this.ReactionId = reactionId;
					this.ReactionFaceAnimId = reactionFaceAnimId;
					this.SoundPath = soundPath;
				}

				// Token: 0x0400041E RID: 1054
				public readonly string IdleActionId;

				// Token: 0x0400041F RID: 1055
				public readonly string IdleFaceAnimId;

				// Token: 0x04000420 RID: 1056
				public readonly string ReactionId;

				// Token: 0x04000421 RID: 1057
				public readonly string ReactionFaceAnimId;

				// Token: 0x04000422 RID: 1058
				public readonly string SoundPath;
			}
		}
	}
}
