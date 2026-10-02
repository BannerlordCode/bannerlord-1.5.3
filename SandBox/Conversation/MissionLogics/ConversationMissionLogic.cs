using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Conversation.MissionLogics
{
	// Token: 0x020000CD RID: 205
	public class ConversationMissionLogic : MissionLogic
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0003B86E File Offset: 0x00039A6E
		private bool IsReadyForConversation
		{
			get
			{
				return this._isRenderingStarted && Agent.Main != null && Agent.Main.IsActive();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0003B88B File Offset: 0x00039A8B
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0003B893 File Offset: 0x00039A93
		public ConversationCharacterData OtherSideConversationData { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0003B89C File Offset: 0x00039A9C
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0003B8A4 File Offset: 0x00039AA4
		public ConversationCharacterData PlayerConversationData { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0003B8AD File Offset: 0x00039AAD
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0003B8B5 File Offset: 0x00039AB5
		public bool IsMultiAgentConversation { get; private set; }

		// Token: 0x0600085F RID: 2143 RVA: 0x0003B8C0 File Offset: 0x00039AC0
		public ConversationMissionLogic(ConversationCharacterData playerCharacterData, ConversationCharacterData otherCharacterData, bool isMultiAgentConversation)
		{
			this.PlayerConversationData = playerCharacterData;
			this.OtherSideConversationData = otherCharacterData;
			this.IsMultiAgentConversation = isMultiAgentConversation;
			bool flag4;
			if (!isMultiAgentConversation)
			{
				PartyBase party = playerCharacterData.Party;
				bool flag;
				if (party == null)
				{
					flag = false;
				}
				else
				{
					MobileParty mobileParty = party.MobileParty;
					bool? flag2 = ((mobileParty != null) ? new bool?(mobileParty.IsCurrentlyAtSea) : null);
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
				}
				if (!flag)
				{
					PartyBase party2 = otherCharacterData.Party;
					if (party2 == null)
					{
						flag4 = false;
					}
					else
					{
						MobileParty mobileParty2 = party2.MobileParty;
						bool? flag2 = ((mobileParty2 != null) ? new bool?(mobileParty2.IsCurrentlyAtSea) : null);
						bool flag3 = true;
						flag4 = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
				}
				else
				{
					flag4 = true;
				}
			}
			else
			{
				flag4 = false;
			}
			this._isNaval = flag4;
			this._isCivilianEquipmentRequiredForLeader = otherCharacterData.IsCivilianEquipmentRequiredForLeader;
			this._isCivilianEquipmentRequiredForBodyGuards = otherCharacterData.IsCivilianEquipmentRequiredForBodyGuardCharacters;
			this._addBloodToAgents = new List<Agent>();
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0003B9A0 File Offset: 0x00039BA0
		public override void AfterStart()
		{
			base.AfterStart();
			this._realCameraController = base.Mission.CameraIsFirstPerson;
			if (this._isNaval)
			{
				string navalConversationCameraTag = this.GetNavalConversationCameraTag(this.OtherSideConversationData.Party);
				Vec2 vec = Mission.Current.Scene.GetGlobalWindStrengthVector();
				float num = vec.Length * 2f;
				float waterStrength = Mission.Current.Scene.GetWaterStrength();
				this.CustomConversationCameraEntity = base.Mission.Scene.FindEntityWithTag(navalConversationCameraTag);
				Scene scene = Mission.Current.Scene;
				vec = MathF.Clamp(num, 1E-05f, 6f) * Vec2.Side;
				scene.SetGlobalWindStrengthVector(in vec);
				Mission.Current.Scene.SetWaterStrength(MathF.Clamp(waterStrength, 1E-05f, 2.5f));
			}
			else if (this.IsMultiAgentConversation)
			{
				Vec2 vec = Mission.Current.Scene.GetGlobalWindStrengthVector();
				float num2 = vec.Length * 2f;
				float waterStrength2 = Mission.Current.Scene.GetWaterStrength();
				Scene scene2 = Mission.Current.Scene;
				vec = MathF.Clamp(num2, 1E-05f, 6f) * Vec2.Side;
				scene2.SetGlobalWindStrengthVector(in vec);
				Mission.Current.Scene.SetWaterStrength(MathF.Clamp(waterStrength2, 1E-05f, 2.5f));
				base.Mission.CameraIsFirstPerson = true;
			}
			else
			{
				base.Mission.CameraIsFirstPerson = true;
			}
			IEnumerable<GameEntity> enumerable = base.Mission.Scene.FindEntitiesWithTag("binary_conversation_point");
			if (enumerable.Any<GameEntity>())
			{
				this._conversationSet = enumerable.ToMBList<GameEntity>().GetRandomElement<GameEntity>();
			}
			this._usedSpawnPoints = new List<GameEntity>();
			BattleSideEnum battleSideEnum = BattleSideEnum.Attacker;
			if (this._isNaval)
			{
				battleSideEnum = BattleSideEnum.Attacker;
			}
			else if (PlayerSiege.PlayerSiegeEvent != null)
			{
				battleSideEnum = PlayerSiege.PlayerSide;
			}
			else if (PlayerEncounter.Current != null)
			{
				if (PlayerEncounter.InsideSettlement && PlayerEncounter.Current.OpponentSide != BattleSideEnum.Defender)
				{
					battleSideEnum = BattleSideEnum.Defender;
				}
				else
				{
					battleSideEnum = BattleSideEnum.Attacker;
				}
				if (PlayerEncounter.Current.EncounterSettlementAux != null && PlayerEncounter.Current.EncounterSettlementAux.MapFaction == Hero.MainHero.MapFaction)
				{
					if (PlayerEncounter.Current.EncounterSettlementAux.IsUnderSiege)
					{
						battleSideEnum = BattleSideEnum.Defender;
					}
					else
					{
						battleSideEnum = BattleSideEnum.Attacker;
					}
				}
			}
			base.Mission.PlayerTeam = base.Mission.Teams.Add(battleSideEnum, Hero.MainHero.MapFaction.Color, Hero.MainHero.MapFaction.Color2, null, true, false, true);
			bool flag = !this.OtherSideConversationData.NoHorse && this.OtherSideConversationData.Character.Equipment[10].Item != null && this.OtherSideConversationData.Character.Equipment[10].Item.HasHorseComponent && battleSideEnum == BattleSideEnum.Defender;
			MatrixFrame matrixFrame;
			MatrixFrame matrixFrame2;
			if (this._conversationSet != null)
			{
				if (base.Mission.PlayerTeam.IsDefender)
				{
					matrixFrame = this.GetDefenderSideSpawnFrame();
					matrixFrame2 = this.GetAttackerSideSpawnFrame(flag);
				}
				else
				{
					matrixFrame = this.GetAttackerSideSpawnFrame(flag);
					matrixFrame2 = this.GetDefenderSideSpawnFrame();
				}
			}
			else
			{
				matrixFrame = this.GetPlayerSideSpawnFrameInSettlement();
				matrixFrame2 = this.GetOtherSideSpawnFrameInSettlement(matrixFrame);
			}
			if (this._isNaval)
			{
				if (this._navalConversationState != ConversationMissionLogic.NavalConversationCameraState.SameShip)
				{
					GameEntity firstEntityWithName = base.Mission.Scene.GetFirstEntityWithName("Ship");
					if (firstEntityWithName != null)
					{
						WeakGameEntity weakEntity = firstEntityWithName.WeakEntity;
						WeakGameEntity firstChildEntityWithTag = weakEntity.GetFirstChildEntityWithTag("tall_rope");
						if (firstChildEntityWithTag != WeakGameEntity.Invalid)
						{
							this._agentHangPointTall = GameEntity.CreateFromWeakEntity(firstChildEntityWithTag.GetFirstChildEntityWithTagRecursive("rope_hang_point"));
							this._agentHangPointSecondTall = GameEntity.CreateFromWeakEntity(firstChildEntityWithTag.GetFirstChildEntityWithTagRecursive("rope_hang_point2"));
						}
						WeakGameEntity firstChildEntityWithTag2 = weakEntity.GetFirstChildEntityWithTag("short_rope");
						if (firstChildEntityWithTag2 != WeakGameEntity.Invalid)
						{
							this._agentHangPointShort = GameEntity.CreateFromWeakEntity(firstChildEntityWithTag2.GetFirstChildEntityWithTagRecursive("rope_hang_point"));
							this._agentHangPointSecondShort = GameEntity.CreateFromWeakEntity(firstChildEntityWithTag2.GetFirstChildEntityWithTagRecursive("rope_hang_point2"));
						}
					}
				}
				else
				{
					matrixFrame2.Rotate(3.1415927f, in Vec3.Up);
				}
			}
			this.SpawnPlayer(this.PlayerConversationData, matrixFrame);
			this.SpawnOtherSide(this.OtherSideConversationData, matrixFrame2, flag, !base.Mission.PlayerTeam.IsDefender);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0003BDE1 File Offset: 0x00039FE1
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0003BDF0 File Offset: 0x00039FF0
		private void SpawnPlayer(ConversationCharacterData playerConversationData, MatrixFrame initialFrame)
		{
			MatrixFrame matrixFrame = new MatrixFrame(in initialFrame.rotation, in initialFrame.origin);
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			this.SpawnCharacter(CharacterObject.PlayerCharacter, playerConversationData, matrixFrame, in ActionIndexCache.act_conversation_normal_loop);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0003BE30 File Offset: 0x0003A030
		private void SpawnOtherSide(ConversationCharacterData characterData, MatrixFrame initialFrame, bool spawnWithHorse, bool isDefenderSide)
		{
			MatrixFrame matrixFrame = new MatrixFrame(in initialFrame.rotation, in initialFrame.origin);
			if (!this._isNaval && Agent.Main != null)
			{
				matrixFrame.rotation.f = Agent.Main.Position - matrixFrame.origin;
			}
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(characterData.Character.Race, "_settlement");
			AgentBuildData agentBuildData = new AgentBuildData(characterData.Character).TroopOrigin(new SimpleAgentOrigin(characterData.Character, -1, null, default(UniqueTroopDescriptor))).Team(base.Mission.PlayerTeam).Monster(monsterWithSuffix)
				.InitialPosition(in matrixFrame.origin);
			Vec2 asVec = matrixFrame.rotation.f.AsVec2;
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec).NoHorses(!spawnWithHorse).CivilianEquipment(this._isCivilianEquipmentRequiredForLeader)
				.SetPrepareImmediately();
			Hero heroObject = characterData.Character.HeroObject;
			if (((heroObject != null) ? heroObject.MapFaction : null) != null)
			{
				agentBuildData2.Banner(characterData.Character.HeroObject.MapFaction.Banner);
				agentBuildData2.ClothingColor1(characterData.Character.HeroObject.MapFaction.Color).ClothingColor2(characterData.Character.HeroObject.MapFaction.Color2);
			}
			else
			{
				PartyBase party = characterData.Party;
				bool flag;
				if (party == null)
				{
					flag = null != null;
				}
				else
				{
					Hero leaderHero = party.LeaderHero;
					flag = ((leaderHero != null) ? leaderHero.ClanBanner : null) != null;
				}
				if (flag)
				{
					agentBuildData2.Banner(characterData.Party.LeaderHero.ClanBanner);
					agentBuildData2.ClothingColor1(characterData.Party.LeaderHero.MapFaction.Color).ClothingColor2(characterData.Party.LeaderHero.MapFaction.Color2);
				}
				else
				{
					PartyBase party2 = characterData.Party;
					if (((party2 != null) ? party2.MapFaction : null) != null)
					{
						AgentBuildData agentBuildData3 = agentBuildData2;
						PartyBase party3 = characterData.Party;
						Banner banner;
						if (party3 == null)
						{
							banner = null;
						}
						else
						{
							IFaction mapFaction = party3.MapFaction;
							banner = ((mapFaction != null) ? mapFaction.Banner : null);
						}
						agentBuildData3.Banner(banner);
						agentBuildData2.ClothingColor1(characterData.Party.MapFaction.Color).ClothingColor2(characterData.Party.MapFaction.Color2);
					}
				}
			}
			if (spawnWithHorse)
			{
				agentBuildData2.MountKey(MountCreationKey.GetRandomMountKeyString(characterData.Character.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, characterData.Character.GetMountKeySeed()));
			}
			if (characterData.Party != null)
			{
				agentBuildData2.TroopOrigin(new PartyAgentOrigin(characterData.Party, characterData.Character, 0, new UniqueTroopDescriptor(FlattenedTroopRoster.GenerateUniqueNoFromParty(characterData.Party.MobileParty, 0)), false, false));
			}
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			this._otherPartyHeightMultiplier = agent.GetEyeGlobalHeight();
			if (characterData.SpawnedAfterFight)
			{
				this._addBloodToAgents.Add(agent);
			}
			if (agent.MountAgent == null)
			{
				agent.SetActionChannel(0, in ActionIndexCache.act_conversation_normal_loop, false, (AnimFlags)0UL, 0f, 1f, 0f, 0.4f, MBRandom.RandomFloat, false, -0.2f, 0, true);
			}
			else
			{
				agent.MountAgent.AgentVisuals.SetAgentLodZeroOrMax(true);
			}
			agent.AgentVisuals.SetAgentLodZeroOrMax(true);
			this._curConversationPartnerAgent = agent;
			bool flag2 = characterData.Character.HeroObject != null && characterData.Character.HeroObject.IsPlayerCompanion;
			if (!characterData.NoBodyguards && !flag2)
			{
				this.SpawnBodyguards(isDefenderSide);
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0003C198 File Offset: 0x0003A398
		private MatrixFrame GetDefenderSideSpawnFrame()
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			foreach (GameEntity gameEntity in this._conversationSet.GetChildren())
			{
				if (gameEntity.HasTag("opponent_infantry_spawn"))
				{
					matrixFrame = gameEntity.GetGlobalFrame();
					break;
				}
			}
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return matrixFrame;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0003C20C File Offset: 0x0003A40C
		private MatrixFrame GetAttackerSideSpawnFrame(bool hasHorse)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			if (this._isNaval && this.CustomConversationCameraEntity != null)
			{
				matrixFrame = this.CustomConversationCameraEntity.GetGlobalFrame();
			}
			else
			{
				foreach (GameEntity gameEntity in this._conversationSet.GetChildren())
				{
					if (hasHorse && gameEntity.HasTag("player_cavalry_spawn"))
					{
						matrixFrame = gameEntity.GetGlobalFrame();
						break;
					}
					if (gameEntity.HasTag("player_infantry_spawn"))
					{
						matrixFrame = gameEntity.GetGlobalFrame();
						break;
					}
				}
			}
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return matrixFrame;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0003C2C0 File Offset: 0x0003A4C0
		private MatrixFrame GetPlayerSideSpawnFrameInSettlement()
		{
			GameEntity gameEntity;
			if ((gameEntity = base.Mission.Scene.FindEntityWithTag("spawnpoint_player")) == null)
			{
				gameEntity = base.Mission.Scene.FindEntitiesWithTag("sp_player_conversation").FirstOrDefault<GameEntity>() ?? base.Mission.Scene.FindEntityWithTag("spawnpoint_player_outside");
			}
			GameEntity gameEntity2 = gameEntity;
			MatrixFrame matrixFrame = ((gameEntity2 != null) ? gameEntity2.GetFrame() : MatrixFrame.Identity);
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return matrixFrame;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0003C338 File Offset: 0x0003A538
		private MatrixFrame GetOtherSideSpawnFrameInSettlement(MatrixFrame playerFrame)
		{
			MatrixFrame matrixFrame = playerFrame;
			Vec3 vec = new Vec3(playerFrame.rotation.f, -1f);
			vec.Normalize();
			matrixFrame.origin = playerFrame.origin + 4f * vec;
			matrixFrame.rotation.RotateAboutUp(3.1415927f);
			return matrixFrame;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0003C395 File Offset: 0x0003A595
		public override void OnRenderingStarted()
		{
			this._isRenderingStarted = true;
			Debug.Print("\n ConversationMissionLogic::OnRenderingStarted\n", 0, Debug.DebugColor.Cyan, 64UL);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0003C3AD File Offset: 0x0003A5AD
		private void InitializeAfterCreation(Agent conversationPartnerAgent, PartyBase conversationPartnerParty)
		{
			Campaign.Current.ConversationManager.SetupAndStartMapConversation((conversationPartnerParty != null) ? conversationPartnerParty.MobileParty : null, conversationPartnerAgent, Mission.Current.MainAgentServer);
			base.Mission.SetMissionMode(MissionMode.Conversation, true);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0003C3E4 File Offset: 0x0003A5E4
		public override void OnMissionTick(float dt)
		{
			if (this._addBloodToAgents.Count > 0)
			{
				foreach (Agent agent in this._addBloodToAgents)
				{
					ValueTuple<sbyte, sbyte> randomPairOfRealBloodBurstBoneIndices = agent.GetRandomPairOfRealBloodBurstBoneIndices();
					if (randomPairOfRealBloodBurstBoneIndices.Item1 != -1 && randomPairOfRealBloodBurstBoneIndices.Item2 != -1)
					{
						agent.CreateBloodBurstAtLimb(randomPairOfRealBloodBurstBoneIndices.Item1, 0.1f + MBRandom.RandomFloat * 0.1f);
						agent.CreateBloodBurstAtLimb(randomPairOfRealBloodBurstBoneIndices.Item2, 0.2f + MBRandom.RandomFloat * 0.2f);
					}
				}
				this._addBloodToAgents.Clear();
			}
			if (!this._conversationStarted)
			{
				if (!this.IsReadyForConversation)
				{
					return;
				}
				this.InitializeAfterCreation(this._curConversationPartnerAgent, this.OtherSideConversationData.Party);
				this._conversationStarted = true;
			}
			if (base.Mission.InputManager.IsGameKeyPressed(4))
			{
				Campaign.Current.ConversationManager.EndConversation();
			}
			if (this._isNaval && this._curConversationPartnerAgent != null && this._agentHangPointShort != null && this._navalConversationState != ConversationMissionLogic.NavalConversationCameraState.SameShip)
			{
				if (ActionIndexCache.act_conversation_naval_start == this._curConversationPartnerAgent.GetCurrentAction(0) || ActionIndexCache.act_conversation_naval_idle_loop == this._curConversationPartnerAgent.GetCurrentAction(0))
				{
					MatrixFrame globalFrame = ((this._otherPartyHeightMultiplier >= 1.76f) ? this._agentHangPointTall : this._agentHangPointShort).GetGlobalFrame();
					Vec3 vec = ((this._otherPartyHeightMultiplier >= 1.76f) ? this._agentHangPointSecondTall : this._agentHangPointSecondShort).GetGlobalFrame().origin - globalFrame.origin;
					vec.Normalize();
					Vec3 vec2 = globalFrame.rotation.f;
					vec2.Normalize();
					Vec3 vec3 = Vec3.CrossProduct(vec2, vec);
					vec3.Normalize();
					vec2 = Vec3.CrossProduct(vec, vec3);
					vec2.Normalize();
					globalFrame.rotation.f = vec2;
					globalFrame.rotation.u = -vec;
					globalFrame.rotation.s = -vec3;
					Agent curConversationPartnerAgent = this._curConversationPartnerAgent;
					MatrixFrame identity = MatrixFrame.Identity;
					curConversationPartnerAgent.SetHandInverseKinematicsFrame(in globalFrame, in identity);
				}
				else
				{
					this._curConversationPartnerAgent.ClearHandInverseKinematics();
				}
			}
			if (this.IsMultiAgentConversation && (ActionIndexCache.act_conversation_naval_start == this._curConversationPartnerAgent.GetCurrentAction(0) || ActionIndexCache.act_conversation_naval_idle_loop == this._curConversationPartnerAgent.GetCurrentAction(0)))
			{
				this._curConversationPartnerAgent.SetCurrentActionProgress(0, 1f);
				this._curConversationPartnerAgent.SetActionChannel(0, in ActionIndexCache.act_conversation_normal_loop, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
			if (!Campaign.Current.ConversationManager.IsConversationInProgress)
			{
				base.Mission.EndMission();
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0003C6E4 File Offset: 0x0003A8E4
		private void SpawnBodyguards(bool isDefenderSide)
		{
			int num = 2;
			ConversationCharacterData otherSideConversationData = this.OtherSideConversationData;
			if (otherSideConversationData.Party == null)
			{
				return;
			}
			TroopRoster memberRoster = otherSideConversationData.Party.MemberRoster;
			int num2 = memberRoster.TotalManCount;
			if (memberRoster.Contains(CharacterObject.PlayerCharacter))
			{
				num2--;
			}
			if (num2 < num + 1)
			{
				return;
			}
			List<CharacterObject> list = new List<CharacterObject>();
			using (List<TroopRosterElement>.Enumerator enumerator = memberRoster.GetTroopRoster().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TroopRosterElement troopRosterElement = enumerator.Current;
					if (troopRosterElement.Character.IsHero && otherSideConversationData.Character != troopRosterElement.Character && !list.Contains(troopRosterElement.Character) && troopRosterElement.Character.HeroObject.IsWounded && !troopRosterElement.Character.IsPlayerCharacter)
					{
						list.Add(troopRosterElement.Character);
					}
				}
				goto IL_016B;
			}
			IL_00D4:
			foreach (TroopRosterElement troopRosterElement2 in from k in memberRoster.GetTroopRoster()
				orderby k.Character.Level descending
				select k)
			{
				if ((!otherSideConversationData.Character.IsHero || otherSideConversationData.Character != troopRosterElement2.Character) && !troopRosterElement2.Character.IsPlayerCharacter)
				{
					list.Add(troopRosterElement2.Character);
				}
				if (list.Count == num)
				{
					break;
				}
			}
			IL_016B:
			if (list.Count >= num)
			{
				List<ActionIndexCache> list2 = new List<ActionIndexCache>
				{
					ActionIndexCache.act_stand_1,
					ActionIndexCache.act_inventory_idle_start,
					ActionIndexCache.act_inventory_idle,
					ActionIndexCache.act_conversation_normal_loop,
					ActionIndexCache.act_conversation_warrior_loop,
					ActionIndexCache.act_conversation_hip_loop,
					ActionIndexCache.act_conversation_closed_loop,
					ActionIndexCache.act_conversation_demure_loop
				};
				for (int i = 0; i < num; i++)
				{
					int num3 = new Random().Next(0, list.Count);
					int num4 = MBRandom.RandomInt(0, list2.Count);
					CharacterObject characterObject = list[num3];
					ConversationCharacterData conversationCharacterData = otherSideConversationData;
					MatrixFrame bodyguardSpawnFrame = this.GetBodyguardSpawnFrame(list[num3].HasMount(), isDefenderSide);
					ActionIndexCache actionIndexCache = list2[num4];
					this.SpawnCharacter(characterObject, conversationCharacterData, bodyguardSpawnFrame, in actionIndexCache);
					list2.RemoveAt(num4);
					list.RemoveAt(num3);
				}
				return;
			}
			goto IL_00D4;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0003C95C File Offset: 0x0003AB5C
		private void SpawnCharacter(CharacterObject character, ConversationCharacterData characterData, MatrixFrame initialFrame, in ActionIndexCache conversationAction)
		{
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
			AgentBuildData agentBuildData = new AgentBuildData(character).TroopOrigin(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))).Team(base.Mission.PlayerTeam).Monster(monsterWithSuffix)
				.InitialPosition(in initialFrame.origin);
			Vec2 vec = initialFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).NoHorses(character.HasMount()).NoWeapons(characterData.NoWeapon)
				.CivilianEquipment((character == CharacterObject.PlayerCharacter) ? this._isCivilianEquipmentRequiredForLeader : this._isCivilianEquipmentRequiredForBodyGuards)
				.SetPrepareImmediately();
			PartyBase party = characterData.Party;
			bool flag;
			if (party == null)
			{
				flag = null != null;
			}
			else
			{
				Hero leaderHero = party.LeaderHero;
				flag = ((leaderHero != null) ? leaderHero.ClanBanner : null) != null;
			}
			if (flag)
			{
				agentBuildData2.Banner(characterData.Party.LeaderHero.ClanBanner);
			}
			else if (characterData.Party != null)
			{
				PartyBase party2 = characterData.Party;
				if (((party2 != null) ? party2.MapFaction : null) != null)
				{
					agentBuildData2.Banner(characterData.Party.MapFaction.Banner);
				}
			}
			if (characterData.Party != null)
			{
				agentBuildData2.ClothingColor1(characterData.Party.MapFaction.Color).ClothingColor2(characterData.Party.MapFaction.Color2);
			}
			if (characterData.Character == CharacterObject.PlayerCharacter)
			{
				agentBuildData2.Controller(AgentControllerType.Player);
			}
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.AgentVisuals.SetAgentLodZeroOrMax(true);
			agent.SetLookAgent(Agent.Main);
			AnimationSystemData animationSystemData = agentBuildData2.AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSetWithSuffix(agentBuildData2.AgentMonster, agentBuildData2.AgentIsFemale, "_poses"), character.GetStepSize(), false);
			agent.SetActionSet(ref animationSystemData);
			if (characterData.Character == CharacterObject.PlayerCharacter)
			{
				agent.AgentVisuals.GetSkeleton().TickAnimationsAndForceUpdate(0.1f, initialFrame, true);
			}
			if (characterData.SpawnedAfterFight)
			{
				this._addBloodToAgents.Add(agent);
				return;
			}
			if (agent.MountAgent == null)
			{
				agent.SetActionChannel(0, in conversationAction, false, (AnimFlags)0UL, 0f, 1f, 0f, 0.4f, MBRandom.RandomFloat * 0.8f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0003CB98 File Offset: 0x0003AD98
		private MatrixFrame GetBodyguardSpawnFrame(bool spawnWithHorse, bool isDefenderSide)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			foreach (GameEntity gameEntity in this._conversationSet.GetChildren())
			{
				if (!isDefenderSide)
				{
					if (spawnWithHorse && gameEntity.HasTag("player_bodyguard_cavalry_spawn") && !this._usedSpawnPoints.Contains(gameEntity))
					{
						this._usedSpawnPoints.Add(gameEntity);
						matrixFrame = gameEntity.GetGlobalFrame();
						break;
					}
					if (gameEntity.HasTag("player_bodyguard_infantry_spawn") && !this._usedSpawnPoints.Contains(gameEntity))
					{
						this._usedSpawnPoints.Add(gameEntity);
						matrixFrame = gameEntity.GetGlobalFrame();
						break;
					}
				}
				else
				{
					if (spawnWithHorse && gameEntity.HasTag("opponent_bodyguard_cavalry_spawn") && !this._usedSpawnPoints.Contains(gameEntity))
					{
						this._usedSpawnPoints.Add(gameEntity);
						matrixFrame = gameEntity.GetGlobalFrame();
						break;
					}
					if (gameEntity.HasTag("opponent_bodyguard_infantry_spawn") && !this._usedSpawnPoints.Contains(gameEntity))
					{
						this._usedSpawnPoints.Add(gameEntity);
						matrixFrame = gameEntity.GetGlobalFrame();
						break;
					}
				}
			}
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return matrixFrame;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0003CCCC File Offset: 0x0003AECC
		protected override void OnEndMission()
		{
			this._conversationSet = null;
			base.Mission.CameraIsFirstPerson = this._realCameraController;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0003CCE8 File Offset: 0x0003AEE8
		private string GetNavalConversationCameraTag(PartyBase encounteredParty)
		{
			string text;
			if (encounteredParty == null || encounteredParty == PartyBase.MainParty)
			{
				text = "custom_camera_same_ship";
				this._navalConversationState = ConversationMissionLogic.NavalConversationCameraState.SameShip;
			}
			else
			{
				ShipHull.ShipType shipType;
				ShipHull.ShipType shipType2;
				if (MobileParty.MainParty.IsCurrentlyAtSea)
				{
					MobileParty mobileParty = encounteredParty.MobileParty;
					if (mobileParty != null && mobileParty.IsCurrentlyAtSea)
					{
						shipType = ((PartyBase.MainParty.Ships.Count > 0) ? PartyBase.MainParty.FlagShip.ShipHull.Type : ShipHull.ShipType.Medium);
						shipType2 = (encounteredParty.Ships.IsEmpty<Ship>() ? shipType : encounteredParty.FlagShip.ShipHull.Type);
						goto IL_0091;
					}
				}
				shipType = ShipHull.ShipType.Medium;
				shipType2 = ShipHull.ShipType.Medium;
				IL_0091:
				if (shipType < shipType2)
				{
					text = "custom_camera_lookup";
					this._navalConversationState = ConversationMissionLogic.NavalConversationCameraState.LookUp;
				}
				else if (shipType > shipType2)
				{
					text = "custom_camera_lookdown";
					this._navalConversationState = ConversationMissionLogic.NavalConversationCameraState.LookDown;
				}
				else
				{
					text = "custom_camera_level";
					this._navalConversationState = ConversationMissionLogic.NavalConversationCameraState.Level;
				}
			}
			return text;
		}

		// Token: 0x04000430 RID: 1072
		private const float MinimumAgentHeightForRopeAnimation = 1.76f;

		// Token: 0x04000431 RID: 1073
		private const float MaximumWindStrength = 6f;

		// Token: 0x04000432 RID: 1074
		private const float MaximumWaveStrength = 2.5f;

		// Token: 0x04000433 RID: 1075
		private const float WindStrengthAmplifier = 2f;

		// Token: 0x04000434 RID: 1076
		private readonly List<Agent> _addBloodToAgents;

		// Token: 0x04000435 RID: 1077
		private Agent _curConversationPartnerAgent;

		// Token: 0x04000436 RID: 1078
		private bool _isRenderingStarted;

		// Token: 0x04000437 RID: 1079
		private bool _conversationStarted;

		// Token: 0x04000438 RID: 1080
		private bool _isCivilianEquipmentRequiredForLeader;

		// Token: 0x04000439 RID: 1081
		private bool _isCivilianEquipmentRequiredForBodyGuards;

		// Token: 0x0400043A RID: 1082
		private List<GameEntity> _usedSpawnPoints;

		// Token: 0x0400043B RID: 1083
		private GameEntity _agentHangPointShort;

		// Token: 0x0400043C RID: 1084
		private GameEntity _agentHangPointSecondShort;

		// Token: 0x0400043D RID: 1085
		private GameEntity _agentHangPointTall;

		// Token: 0x0400043E RID: 1086
		private GameEntity _agentHangPointSecondTall;

		// Token: 0x0400043F RID: 1087
		private GameEntity _conversationSet;

		// Token: 0x04000440 RID: 1088
		private bool _realCameraController;

		// Token: 0x04000441 RID: 1089
		private readonly bool _isNaval;

		// Token: 0x04000442 RID: 1090
		private float _otherPartyHeightMultiplier;

		// Token: 0x04000443 RID: 1091
		private ConversationMissionLogic.NavalConversationCameraState _navalConversationState;

		// Token: 0x04000444 RID: 1092
		public GameEntity CustomConversationCameraEntity;

		// Token: 0x020001EB RID: 491
		private enum NavalConversationCameraState
		{
			// Token: 0x04000904 RID: 2308
			None,
			// Token: 0x04000905 RID: 2309
			SameShip,
			// Token: 0x04000906 RID: 2310
			Level,
			// Token: 0x04000907 RID: 2311
			LookDown,
			// Token: 0x04000908 RID: 2312
			LookUp
		}
	}
}
