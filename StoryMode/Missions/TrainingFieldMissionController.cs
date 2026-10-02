using System;
using System.Collections.Generic;
using System.Linq;
using SandBox;
using SandBox.Conversation.MissionLogics;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace StoryMode.Missions
{
	// Token: 0x0200003A RID: 58
	public class TrainingFieldMissionController : MissionLogic
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00013CA5 File Offset: 0x00011EA5
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x00013CAD File Offset: 0x00011EAD
		public TextObject InitialCurrentObjective { get; private set; }

		// Token: 0x060003B2 RID: 946 RVA: 0x00013CB6 File Offset: 0x00011EB6
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00013CCC File Offset: 0x00011ECC
		public override void AfterStart()
		{
			base.AfterStart();
			base.Mission.IsInventoryAccessible = false;
			base.Mission.IsQuestScreenAccessible = false;
			base.Mission.IsCharacterWindowAccessible = false;
			base.Mission.IsPartyWindowAccessible = false;
			base.Mission.IsKingdomWindowAccessible = false;
			base.Mission.IsClanWindowAccessible = false;
			base.Mission.IsEncyclopediaWindowAccessible = false;
			base.Mission.IsBannerWindowAccessible = false;
			this._missionConversationHandler = base.Mission.GetMissionBehavior<MissionConversationLogic>();
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, true, false, false, "");
			this.LoadTutorialScores();
			this.SpawnConversationBrother();
			this.CollectWeaponsAndObjectives();
			this.InitializeMeleeTraining();
			this.InitializeMountedTraining();
			this.InitializeAdvancedMeleeTraining();
			this.InitializeBowTraining();
			TrainingFieldMissionController.MakeAllAgentsImmortal();
			TrainingFieldMissionController.SetHorseMountable(false);
			this.InitialCurrentObjective = new TextObject("{=BTY2aZCt}Enter a training area.", null);
			this._playerCampaignHealth = Agent.Main.Health;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00013DBE File Offset: 0x00011FBE
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00013DCC File Offset: 0x00011FCC
		private void LoadTutorialScores()
		{
			this._tutorialScores = StoryModeManager.Current.MainStoryLine.GetTutorialScores();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00013DE3 File Offset: 0x00011FE3
		protected override void OnEndMission()
		{
			base.OnEndMission();
			Agent.Main.Health = this._playerCampaignHealth;
			StoryModeManager.Current.MainStoryLine.SetTutorialScores(this._tutorialScores);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00013E10 File Offset: 0x00012010
		public override void OnRenderingStarted()
		{
			base.OnRenderingStarted();
			if (this._brotherConversationAgent != null)
			{
				base.Mission.GetMissionBehavior<MissionConversationLogic>().StartConversation(this._brotherConversationAgent, false, true);
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00013E38 File Offset: 0x00012038
		public override void OnMissionTick(float dt)
		{
			this.TrainingAreaUpdate();
			this.UpdateHorseBehavior();
			this.UpdateBowTraining();
			this.UpdateMountedAIBehavior();
			if (TrainingFieldMissionController._updateObjectivesWillBeCalled)
			{
				this.UpdateObjectives();
			}
			for (int i = this._delayedActions.Count - 1; i >= 0; i--)
			{
				if (this._delayedActions[i].Update())
				{
					this._delayedActions.RemoveAt(i);
				}
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00013EA4 File Offset: 0x000120A4
		private void UpdateObjectives()
		{
			if (this._trainingSubTypeIndex == -1 || this._showTutorialObjectivesAnyway)
			{
				Action<List<TrainingFieldMissionController.TutorialObjective>> allObjectivesTick = this.AllObjectivesTick;
				if (allObjectivesTick != null)
				{
					allObjectivesTick(this._tutorialObjectives);
				}
			}
			else
			{
				Action<List<TrainingFieldMissionController.TutorialObjective>> allObjectivesTick2 = this.AllObjectivesTick;
				if (allObjectivesTick2 != null)
				{
					allObjectivesTick2(this._detailedObjectives);
				}
			}
			TrainingFieldMissionController._updateObjectivesWillBeCalled = false;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00013EF8 File Offset: 0x000120F8
		private int GetSelectedTrainingSubTypeIndex()
		{
			TrainingIcon activeTrainingIcon = this._activeTutorialArea.GetActiveTrainingIcon();
			if (activeTrainingIcon != null)
			{
				this.EnableAllTrainingIcons();
				activeTrainingIcon.DisableIcon();
				this._activeTrainingSubTypeTag = activeTrainingIcon.GetTrainingSubTypeTag();
				return this._activeTutorialArea.GetIndexFromTag(activeTrainingIcon.GetTrainingSubTypeTag());
			}
			return -1;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00013F40 File Offset: 0x00012140
		private string GetHighlightedWeaponRack()
		{
			foreach (TrainingIcon trainingIcon in this._activeTutorialArea.TrainingIconsReadOnly)
			{
				if (trainingIcon.Focused)
				{
					return trainingIcon.GetTrainingSubTypeTag();
				}
			}
			return "";
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00013FAC File Offset: 0x000121AC
		private void EnableAllTrainingIcons()
		{
			foreach (TrainingIcon trainingIcon in this._activeTutorialArea.TrainingIconsReadOnly)
			{
				trainingIcon.EnableIcon();
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00014004 File Offset: 0x00012204
		private void TrainingAreaUpdate()
		{
			this.CheckMainAgentEquipment();
			if (this._activeTutorialArea != null)
			{
				string[] array;
				if (this._activeTutorialArea.IsPositionInsideTutorialArea(Agent.Main.Position, out array))
				{
					this.InTrainingArea();
					if (this._trainingSubTypeIndex != -1)
					{
						this._activeTutorialArea.CheckWeapons(this._trainingSubTypeIndex);
					}
				}
				else
				{
					this.OnTrainingAreaExit(true);
					this._activeTutorialArea = null;
				}
			}
			else
			{
				foreach (TutorialArea tutorialArea in this._trainingAreas)
				{
					string[] array;
					if (tutorialArea.IsPositionInsideTutorialArea(Agent.Main.Position, out array))
					{
						this._activeTutorialArea = tutorialArea;
						this.OnTrainingAreaEnter();
						break;
					}
				}
			}
			this.UpdateConversationPermission();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000140D4 File Offset: 0x000122D4
		private void UpdateConversationPermission()
		{
			if (this._brotherConversationAgent == null || Mission.Current.MainAgent == null || (this._brotherConversationAgent.Position - Mission.Current.MainAgent.Position).LengthSquared > 4f)
			{
				this._missionConversationHandler.DisableStartConversation(true);
				return;
			}
			this._missionConversationHandler.DisableStartConversation(false);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001413C File Offset: 0x0001233C
		private void ResetTrainingArea()
		{
			this.OnTrainingAreaExit(true);
			this.OnTrainingAreaEnter();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001414C File Offset: 0x0001234C
		private void OnTrainingAreaExit(bool enableTrainingIcons)
		{
			this._activeTutorialArea.MarkTrainingIcons(false);
			TrainingFieldMissionController.TutorialObjective tutorialObjective = this._tutorialObjectives.Find((TrainingFieldMissionController.TutorialObjective x) => x.Id == this._activeTutorialArea.TypeOfTraining.ToString());
			tutorialObjective.SetActive(false);
			tutorialObjective.SetAllSubTasksInactive();
			TrainingFieldMissionController.DropAllWeaponsOfMainAgent();
			this.SpecialTrainingAreaExit(this._activeTutorialArea.TypeOfTraining);
			this._activeTutorialArea.DeactivateAllWeapons(true);
			this._trainingProgress = 0;
			this._trainingSubTypeIndex = -1;
			this.EnableAllTrainingIcons();
			if (this.CheckAllObjectivesFinished())
			{
				this.CurrentObjectiveTick(new TextObject("{=77TavbOY}You have completed all tutorials. You can always come back to improve your score.", null));
				if (!this._courseFinished)
				{
					this._courseFinished = true;
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/finish_course"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
				}
			}
			else
			{
				this.CurrentObjectiveTick(new TextObject("{=BTY2aZCt}Enter a training area.", null));
			}
			this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.None);
			this.UIEndTimer();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001423C File Offset: 0x0001243C
		private bool CheckAllObjectivesFinished()
		{
			using (List<TrainingFieldMissionController.TutorialObjective>.Enumerator enumerator = this._tutorialObjectives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsFinished)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00014298 File Offset: 0x00012498
		private void OnTrainingAreaEnter()
		{
			this._tutorialObjectives.Find((TrainingFieldMissionController.TutorialObjective x) => x.Id == this._activeTutorialArea.TypeOfTraining.ToString()).SetActive(true);
			TrainingFieldMissionController.DropAllWeaponsOfMainAgent();
			this._trainingProgress = 0;
			this._trainingSubTypeIndex = -1;
			this.SpecialTrainingAreaEnter(this._activeTutorialArea.TypeOfTraining);
			this.CurrentObjectiveTick(new TextObject("{=WIUbM9Hc}Choose a weapon to begin training.", null));
			this._activeTutorialArea.MarkTrainingIcons(true);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001430C File Offset: 0x0001250C
		private void InTrainingArea()
		{
			int selectedTrainingSubTypeIndex = this.GetSelectedTrainingSubTypeIndex();
			if (selectedTrainingSubTypeIndex >= 0)
			{
				this.OnStartTraining(selectedTrainingSubTypeIndex);
			}
			else
			{
				string highlightedWeaponRack = this.GetHighlightedWeaponRack();
				if (highlightedWeaponRack != "")
				{
					using (List<TrainingFieldMissionController.TutorialObjective>.Enumerator enumerator = this._tutorialObjectives.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TrainingFieldMissionController.TutorialObjective tutorialObjective = enumerator.Current;
							if (tutorialObjective.Id == this._activeTutorialArea.TypeOfTraining.ToString())
							{
								using (List<TrainingFieldMissionController.TutorialObjective>.Enumerator enumerator2 = tutorialObjective.SubTasks.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										TrainingFieldMissionController.TutorialObjective tutorialObjective2 = enumerator2.Current;
										if (tutorialObjective2.Id == highlightedWeaponRack)
										{
											tutorialObjective2.SetActive(true);
										}
										else
										{
											tutorialObjective2.SetActive(false);
										}
									}
									break;
								}
							}
						}
						goto IL_00FB;
					}
				}
				this._tutorialObjectives.Find((TrainingFieldMissionController.TutorialObjective x) => x.Id == this._activeTutorialArea.TypeOfTraining.ToString()).SetAllSubTasksInactive();
			}
			IL_00FB:
			this.SpecialInTrainingAreaUpdate(this._activeTutorialArea.TypeOfTraining);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00014444 File Offset: 0x00012644
		private void OnStartTraining(int index)
		{
			this._showTutorialObjectivesAnyway = false;
			this._activeTutorialArea.MarkTrainingIcons(false);
			this.SpecialTrainingStart(this._activeTutorialArea.TypeOfTraining);
			this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.None);
			this.UIEndTimer();
			TrainingFieldMissionController.DropAllWeaponsOfMainAgent();
			this._activeTutorialArea.DeactivateAllWeapons(true);
			this._activeTutorialArea.ActivateTaggedWeapons(index);
			this._activeTutorialArea.EquipWeaponsToPlayer(index);
			this._trainingProgress = 1;
			this._trainingSubTypeIndex = index;
			this.UpdateObjectives();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000144C8 File Offset: 0x000126C8
		private void SuccessfullyFinishTraining(float score)
		{
			this._tutorialObjectives.Find((TrainingFieldMissionController.TutorialObjective x) => x.Id == this._activeTutorialArea.TypeOfTraining.ToString()).FinishSubTask(this._activeTrainingSubTypeTag, score);
			this._tutorialScores[this._activeTrainingSubTypeTag] = score;
			this._activeTutorialArea.MarkTrainingIcons(true);
			Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/finish_task"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
			this._showTutorialObjectivesAnyway = true;
			this.UpdateObjectives();
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00014548 File Offset: 0x00012748
		private static void RefillAmmoOfAgent(Agent agent)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (agent.Equipment[equipmentIndex].IsAnyConsumable() && agent.Equipment[equipmentIndex].Amount <= 1)
				{
					agent.SetWeaponAmountInSlot(equipmentIndex, agent.Equipment[equipmentIndex].ModifiedMaxAmount, true);
				}
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000145AC File Offset: 0x000127AC
		private void SpecialTrainingAreaExit(TutorialArea.TrainingType trainingType)
		{
			if (this._trainingSubTypeIndex != -1)
			{
				this._activeTutorialArea.ResetBreakables(this._trainingSubTypeIndex, true);
			}
			switch (trainingType)
			{
			case TutorialArea.TrainingType.Bow:
				this.OnBowTrainingExit();
				return;
			case TutorialArea.TrainingType.Melee:
				return;
			case TutorialArea.TrainingType.Mounted:
				this.OnMountedTrainingExit();
				return;
			case TutorialArea.TrainingType.AdvancedMelee:
				this.OnAdvancedTrainingExit();
				return;
			default:
				throw new ArgumentOutOfRangeException("trainingType", trainingType, null);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00014613 File Offset: 0x00012813
		private void SpecialTrainingAreaEnter(TutorialArea.TrainingType trainingType)
		{
			if (trainingType <= TutorialArea.TrainingType.Mounted)
			{
				return;
			}
			if (trainingType == TutorialArea.TrainingType.AdvancedMelee)
			{
				this.OnAdvancedTrainingAreaEnter();
				return;
			}
			throw new ArgumentOutOfRangeException("trainingType", trainingType, null);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00014638 File Offset: 0x00012838
		private void SpecialTrainingStart(TutorialArea.TrainingType trainingType)
		{
			if (this._trainingSubTypeIndex != -1)
			{
				this._activeTutorialArea.ResetBreakables(this._trainingSubTypeIndex, true);
			}
			switch (trainingType)
			{
			case TutorialArea.TrainingType.Bow:
				this.OnBowTrainingStart();
				return;
			case TutorialArea.TrainingType.Melee:
				return;
			case TutorialArea.TrainingType.Mounted:
				this.OnMountedTrainingStart();
				return;
			case TutorialArea.TrainingType.AdvancedMelee:
				this.OnAdvancedTrainingStart();
				return;
			default:
				throw new ArgumentOutOfRangeException("trainingType", trainingType, null);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000146A0 File Offset: 0x000128A0
		private void SpecialInTrainingAreaUpdate(TutorialArea.TrainingType trainingType)
		{
			switch (trainingType)
			{
			case TutorialArea.TrainingType.Bow:
				this.BowInTrainingAreaUpdate();
				return;
			case TutorialArea.TrainingType.Melee:
				this.MeleeTrainingUpdate();
				return;
			case TutorialArea.TrainingType.Mounted:
				this.MountedTrainingUpdate();
				return;
			case TutorialArea.TrainingType.AdvancedMelee:
				this.AdvancedMeleeTrainingUpdate();
				return;
			default:
				throw new ArgumentOutOfRangeException("trainingType", trainingType, null);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000146F4 File Offset: 0x000128F4
		private static void DropAllWeaponsOfMainAgent()
		{
			Mission.Current.MainAgent.SetActionChannel(1, in ActionIndexCache.act_none, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex <= EquipmentIndex.Weapon3; equipmentIndex++)
			{
				if (!Mission.Current.MainAgent.Equipment[equipmentIndex].IsEmpty)
				{
					Mission.Current.MainAgent.DropItem(equipmentIndex, WeaponClass.Undefined);
				}
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00014778 File Offset: 0x00012978
		private static void RemoveAllWeaponsFromMainAgent()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex <= EquipmentIndex.Weapon3; equipmentIndex++)
			{
				if (!Mission.Current.MainAgent.Equipment[equipmentIndex].IsEmpty)
				{
					Mission.Current.MainAgent.RemoveEquippedWeapon(equipmentIndex);
				}
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000147C0 File Offset: 0x000129C0
		private void CollectWeaponsAndObjectives()
		{
			List<GameEntity> list = new List<GameEntity>();
			Mission.Current.Scene.GetEntities(ref list);
			foreach (GameEntity gameEntity in list)
			{
				if (gameEntity.HasTag("bow_training_shooting_position"))
				{
					this._shootingPosition = gameEntity;
				}
				if (gameEntity.GetFirstScriptOfType<TutorialArea>() != null)
				{
					this._trainingAreas.Add(gameEntity.GetFirstScriptOfType<TutorialArea>());
					this._tutorialObjectives.Add(new TrainingFieldMissionController.TutorialObjective(this._trainingAreas[this._trainingAreas.Count - 1].TypeOfTraining.ToString(), false, false, false));
					foreach (string text in this._trainingAreas[this._trainingAreas.Count - 1].GetSubTrainingTags())
					{
						this._tutorialObjectives[this._tutorialObjectives.Count - 1].AddSubTask(new TrainingFieldMissionController.TutorialObjective(text, false, false, false));
						float num;
						if (this._tutorialScores.TryGetValue(text, out num))
						{
							this._tutorialObjectives[this._tutorialObjectives.Count - 1].SubTasks.Last<TrainingFieldMissionController.TutorialObjective>().RestoreScoreFromSave(num);
						}
					}
				}
				if (gameEntity.HasTag("mounted_checkpoint") && gameEntity.GetFirstScriptOfType<VolumeBox>() != null)
				{
					bool flag = false;
					for (int i = 0; i < this._checkpoints.Count; i++)
					{
						if (int.Parse(gameEntity.Tags[1]) < int.Parse(this._checkpoints[i].Item1.GameEntity.Tags[1]))
						{
							this._checkpoints.Insert(i, ValueTuple.Create<VolumeBox, bool>(gameEntity.GetFirstScriptOfType<VolumeBox>(), false));
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this._checkpoints.Add(ValueTuple.Create<VolumeBox, bool>(gameEntity.GetFirstScriptOfType<VolumeBox>(), false));
					}
				}
				if (gameEntity.HasScriptOfType<DestructableComponent>())
				{
					if (gameEntity.HasTag("_ranged_npc_target"))
					{
						this._targetsForRangedNpc.Add(gameEntity.GetFirstScriptOfType<DestructableComponent>());
					}
					else if (gameEntity.HasTag("_mounted_ai_target"))
					{
						int j = int.Parse(gameEntity.Tags[1]);
						while (j > this._mountedAITargets.Count - 1)
						{
							this._mountedAITargets.Add(null);
						}
						this._mountedAITargets[j] = gameEntity.GetFirstScriptOfType<DestructableComponent>();
					}
				}
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014A7C File Offset: 0x00012C7C
		private static void MakeAllAgentsImmortal()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				agent.SetMortalityState(Agent.MortalityState.Immortal);
				if (!agent.IsMount)
				{
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				}
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00014AE4 File Offset: 0x00012CE4
		private bool HasAllWeaponsPicked()
		{
			return this._activeTutorialArea.HasMainAgentPickedAll(this._trainingSubTypeIndex);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00014AF7 File Offset: 0x00012CF7
		private void CheckMainAgentEquipment()
		{
			if (this._trainingSubTypeIndex == -1)
			{
				TrainingFieldMissionController.RemoveAllWeaponsFromMainAgent();
				return;
			}
			this._activeTutorialArea.CheckMainAgentEquipment(this._trainingSubTypeIndex);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00014B19 File Offset: 0x00012D19
		private void StartTimer()
		{
			this._beginningTime = base.Mission.CurrentTime;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00014B2C File Offset: 0x00012D2C
		private void EndTimer()
		{
			this._timeScore = base.Mission.CurrentTime - this._beginningTime;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00014B48 File Offset: 0x00012D48
		private void SpawnConversationBrother()
		{
			if (!TutorialPhase.Instance.TalkedWithBrotherForTheFirstTime)
			{
				WorldFrame worldFrame = new WorldFrame(Agent.Main.Frame.rotation, new WorldPosition(base.Mission.Scene, Agent.Main.Position));
				worldFrame.Origin.SetVec2(Agent.Main.GetWorldFrame().Origin.AsVec2 + Vec2.Forward * 3f);
				worldFrame.Rotation.RotateAboutUp(3.1415927f);
				MatrixFrame matrixFrame = worldFrame.ToGroundMatrixFrame();
				CharacterObject characterObject = StoryModeHeroes.ElderBrother.CharacterObject;
				AgentBuildData agentBuildData = new AgentBuildData(characterObject).Team(base.Mission.SpectatorTeam).InitialPosition(in matrixFrame.origin);
				Vec2 vec = matrixFrame.rotation.f.AsVec2;
				vec = vec.Normalized();
				AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).CivilianEquipment(false).NoHorses(true)
					.NoWeapons(true)
					.ClothingColor1(base.Mission.PlayerTeam.Color)
					.ClothingColor2(base.Mission.PlayerTeam.Color2)
					.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, characterObject, -1, default(UniqueTroopDescriptor), false, false))
					.MountKey(MountCreationKey.GetRandomMountKeyString(characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, characterObject.GetMountKeySeed()));
				this._brotherConversationAgent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00014CCC File Offset: 0x00012ECC
		private void InitializeBowTraining()
		{
			this._shootingPosition.SetVisibilityExcludeParents(false);
			this._bowNpc = this.SpawnBowNPC();
			this._rangedNpcSpawnPosition = this._bowNpc.GetWorldPosition();
			this._bowNpc.SetAIBehaviorValues(HumanAIComponent.AISimpleBehaviorKind.Ranged, 0f, 6f, 0f, 66f, 0f);
			this._bowNpc.SetAIBehaviorValues(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 0f, 6f, 0f, 66f, 0f);
			this._bowNpc.SetAIBehaviorValues(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 66666f, 6f, 66666f, 120f, 66666f);
			this.GiveMoveOrderToRangedAgent(this._shootingPosition.GlobalPosition.ToWorldPosition(), this._shootingPosition.GetGlobalFrame().rotation.f.NormalizedCopy());
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00014DA4 File Offset: 0x00012FA4
		private void GiveMoveOrderToRangedAgent(WorldPosition worldPosition, Vec3 rotation)
		{
			if (worldPosition.AsVec2.NearlyEquals(this._rangedTargetPosition.AsVec2, 0.001f))
			{
				Vec3 groundVec = worldPosition.GetGroundVec3();
				Vec3 groundVec2 = this._rangedTargetPosition.GetGroundVec3();
				if (groundVec.NearlyEquals(in groundVec2, 0.001f) && rotation.NearlyEquals(in this._rangedTargetRotation, 1E-05f))
				{
					return;
				}
			}
			this._rangedTargetPosition = worldPosition;
			this._rangedTargetRotation = rotation;
			this._bowNpc.SetWatchState(Agent.WatchState.Patrolling);
			this._targetPositionSet = false;
			this._delayedActions.Add(new TrainingFieldMissionController.DelayedAction(delegate
			{
				this._bowNpc.ClearTargetFrame();
				this._bowNpc.SetScriptedPositionAndDirection(ref worldPosition, this._rangedTargetRotation.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
			}, 2f));
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00014E70 File Offset: 0x00013070
		private WeakGameEntity GetValidTarget()
		{
			foreach (DestructableComponent destructableComponent in this._targetsForRangedNpc)
			{
				if (!destructableComponent.IsDestroyed)
				{
					this._lastTargetGiven = destructableComponent;
					return this._lastTargetGiven.GameEntity;
				}
			}
			foreach (DestructableComponent destructableComponent2 in this._targetsForRangedNpc)
			{
				destructableComponent2.Reset();
			}
			this._lastTargetGiven = this._targetsForRangedNpc[0];
			return this._lastTargetGiven.GameEntity;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00014F38 File Offset: 0x00013138
		private void UpdateBowTraining()
		{
			if ((this._bowNpc.MovementFlags & Agent.MovementControlFlag.MoveMask) == Agent.MovementControlFlag.None && (this._rangedTargetPosition.GetGroundVec3() - this._bowNpc.Position).LengthSquared < 0.16000001f)
			{
				if (!this._targetPositionSet)
				{
					this._bowNpc.DisableScriptedMovement();
					Agent bowNpc = this._bowNpc;
					Vec2 asVec = this._bowNpc.Position.AsVec2;
					bowNpc.SetTargetPositionAndDirection(in asVec, in this._rangedTargetRotation);
					this._targetPositionSet = true;
					if ((this._bowNpc.Position - this._shootingPosition.GlobalPosition).LengthSquared > (this._bowNpc.Position - this._rangedNpcSpawnPosition.GetGroundVec3()).LengthSquared)
					{
						this._atShootingPosition = false;
						return;
					}
					this._bowNpc.SetWatchState(Agent.WatchState.Alarmed);
					this._bowNpc.SetScriptedTargetEntity(this.GetValidTarget(), Agent.AISpecialCombatModeFlags.None, false);
					this._atShootingPosition = true;
					return;
				}
				else if (this._atShootingPosition && this._lastTargetGiven.IsDestroyed)
				{
					this._bowNpc.SetScriptedTargetEntity(this.GetValidTarget(), Agent.AISpecialCombatModeFlags.None, false);
				}
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00015068 File Offset: 0x00013268
		private Agent SpawnBowNPC()
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			this._rangedNpcSpawnPoint = base.Mission.Scene.FindEntityWithTag("spawner_ranged_npc_tag");
			if (this._rangedNpcSpawnPoint != null)
			{
				matrixFrame = this._rangedNpcSpawnPoint.GetGlobalFrame();
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			}
			else
			{
				Debug.FailedAssert("There are no spawn points for bow npc.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Missions\\TrainingFieldMissionController.cs", "SpawnBowNPC", 1096);
			}
			Location locationWithId = LocationComplex.Current.GetLocationWithId("training_field");
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("tutorial_npc_ranged");
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(@object.Race);
			AgentData agentData = new AgentData(new PartyAgentOrigin(PartyBase.MainParty, @object, -1, default(UniqueTroopDescriptor), false, false)).Monster(baseMonsterFromRace).NoHorses(true);
			locationWithId.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), null, true, LocationCharacter.CharacterRelations.Friendly, null, true, false, null, false, true, true, null, false));
			AgentBuildData agentBuildData = new AgentBuildData(@object).Team(base.Mission.PlayerTeam).InitialPosition(in matrixFrame.origin);
			Vec2 asVec = matrixFrame.rotation.f.AsVec2;
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec).CivilianEquipment(false).NoHorses(true)
				.NoWeapons(false)
				.ClothingColor1(base.Mission.PlayerTeam.Color)
				.ClothingColor2(base.Mission.PlayerTeam.Color2)
				.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, @object, -1, default(UniqueTroopDescriptor), false, false))
				.MountKey(MountCreationKey.GetRandomMountKeyString(@object.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, @object.GetMountKeySeed()))
				.Controller(AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.SetTeam(Mission.Current.PlayerAllyTeam, false);
			return agent;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00015244 File Offset: 0x00013444
		private void BowInTrainingAreaUpdate()
		{
			switch (this._trainingProgress)
			{
			case 1:
				if (this.HasAllWeaponsPicked())
				{
					this._rangedLastBrokenTargetCount = 0;
					this.LoadCrossbowForStarting();
					this._trainingProgress++;
					this.CurrentObjectiveTick(new TextObject("{=kwW6v202}Go to shooting position", null));
					this._shootingPosition.SetVisibilityExcludeParents(true);
					this._detailedObjectives = this._rangedObjectives.ConvertAll<TrainingFieldMissionController.TutorialObjective>((TrainingFieldMissionController.TutorialObjective x) => new TrainingFieldMissionController.TutorialObjective(x.Id, x.IsFinished, x.IsActive, x.HasBackground));
					this._detailedObjectives[1].SetTextVariableOfName("HIT", this._activeTutorialArea.GetBrokenBreakableCount(this._trainingSubTypeIndex));
					this._detailedObjectives[1].SetTextVariableOfName("ALL", this._activeTutorialArea.GetBreakablesCount(this._trainingSubTypeIndex));
					this._detailedObjectives[0].SetActive(true);
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/archery/pick_" + this._trainingSubTypeIndex), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
					return;
				}
				break;
			case 2:
				if ((this._shootingPosition.GetGlobalFrame().origin - Agent.Main.Position).LengthSquared < 4f)
				{
					this._trainingProgress++;
					this._shootingPosition.SetVisibilityExcludeParents(false);
					this._activeTutorialArea.MarkAllTargets(this._trainingSubTypeIndex, true);
					this._remainingTargetText.SetTextVariable("REMAINING_TARGET", this._activeTutorialArea.GetUnbrokenBreakableCount(this._trainingSubTypeIndex));
					this.CurrentObjectiveTick(this._remainingTargetText);
					this._detailedObjectives[0].FinishTask();
					this._detailedObjectives[1].SetActive(true);
					return;
				}
				break;
			case 3:
				break;
			case 4:
			{
				int brokenBreakableCount = this._activeTutorialArea.GetBrokenBreakableCount(this._trainingSubTypeIndex);
				this._remainingTargetText.SetTextVariable("REMAINING_TARGET", this._activeTutorialArea.GetUnbrokenBreakableCount(this._trainingSubTypeIndex));
				this.CurrentObjectiveTick(this._remainingTargetText);
				this._detailedObjectives[1].SetTextVariableOfName("HIT", brokenBreakableCount);
				if (brokenBreakableCount != this._rangedLastBrokenTargetCount)
				{
					this._rangedLastBrokenTargetCount = brokenBreakableCount;
					this._activeTutorialArea.ResetMarkingTargetTimers(this._trainingSubTypeIndex);
				}
				if (MBRandom.NondeterministicRandomInt % 4 == 3)
				{
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/archery/hit_target"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
				}
				if (this._activeTutorialArea.AllBreakablesAreBroken(this._trainingSubTypeIndex))
				{
					this._detailedObjectives[1].FinishTask();
					this._trainingProgress++;
					this.BowTrainingEndedSuccessfully();
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00015518 File Offset: 0x00013718
		public void LoadCrossbowForStarting()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				MissionWeapon missionWeapon = Agent.Main.Equipment[equipmentIndex];
				if (!missionWeapon.IsEmpty && missionWeapon.Item.PrimaryWeapon.WeaponClass == WeaponClass.Crossbow && missionWeapon.Ammo == 0)
				{
					int num;
					EquipmentIndex equipmentIndex2;
					Agent.Main.Equipment.GetAmmoCountAndIndexOfType(missionWeapon.Item.Type, out num, out equipmentIndex2, EquipmentIndex.None);
					Agent.Main.SetReloadAmmoInSlot(equipmentIndex, equipmentIndex2, 1);
					Agent.Main.SetWeaponReloadPhaseAsClient(equipmentIndex, missionWeapon.ReloadPhaseCount);
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000155A8 File Offset: 0x000137A8
		public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
		{
			base.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);
			TutorialArea activeTutorialArea = this._activeTutorialArea;
			if (activeTutorialArea != null && activeTutorialArea.TypeOfTraining == TutorialArea.TrainingType.Bow && this._trainingProgress == 3)
			{
				this._trainingProgress++;
				this._activeTutorialArea.MakeDestructible(this._trainingSubTypeIndex);
				this.UIStartTimer();
				this.CurrentObjectiveTick(new TextObject("{=9kGnzjrU}Timer Started.", null));
				this.StartTimer();
				Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/archery/start_training"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
			}
			TrainingFieldMissionController.RefillAmmoOfAgent(shooterAgent);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00015654 File Offset: 0x00013854
		private void BowTrainingEndedSuccessfully()
		{
			this.EndTimer();
			this._activeTutorialArea.HideBoundaries();
			this.CurrentObjectiveTick(this._trainingFinishedText);
			TextObject textObject = new TextObject("{=xVFupnFu}You've successfully hit all of the targets in ({TIME_SCORE}) seconds.", null);
			float num = this.UIEndTimer();
			textObject.SetTextVariable("TIME_SCORE", new TextObject(num.ToString("0.0"), null));
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			this.SuccessfullyFinishTraining(num);
			this._shootingPosition.SetVisibilityExcludeParents(false);
			Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/archery/finish"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000156FC File Offset: 0x000138FC
		private void OnBowTrainingStart()
		{
			this._shootingPosition.SetVisibilityExcludeParents(false);
			this.GiveMoveOrderToRangedAgent(this._rangedNpcSpawnPoint.GlobalPosition.ToWorldPosition(), this._rangedNpcSpawnPoint.GetGlobalFrame().rotation.f.NormalizedCopy());
			foreach (DestructableComponent destructableComponent in this._targetsForRangedNpc)
			{
				destructableComponent.Reset();
				destructableComponent.GameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001579C File Offset: 0x0001399C
		private void OnBowTrainingExit()
		{
			this._shootingPosition.SetVisibilityExcludeParents(false);
			this.GiveMoveOrderToRangedAgent(this._shootingPosition.GlobalPosition.ToWorldPosition(), this._shootingPosition.GetGlobalFrame().rotation.f.NormalizedCopy());
			foreach (DestructableComponent destructableComponent in this._targetsForRangedNpc)
			{
				destructableComponent.Reset();
				destructableComponent.GameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001583C File Offset: 0x00013A3C
		private void InitializeAdvancedMeleeTraining()
		{
			this._advancedMeleeTrainerEasy = this.SpawnAdvancedMeleeTrainerEasy();
			this._advancedMeleeTrainerEasy.SetAgentFlags(this._advancedMeleeTrainerEasy.GetAgentFlags() & ~AgentFlag.CanGetAlarmed);
			this._advancedMeleeTrainerEasyInitialPosition = base.Mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_easy").GetGlobalFrame();
			this._advancedMeleeTrainerEasySecondPosition = base.Mission.Scene.FindEntityWithTag("adv_melee_npc_easy_second_pos").GetGlobalFrame();
			this._advancedMeleeTrainerNormal = this.SpawnAdvancedMeleeTrainerNormal();
			this._advancedMeleeTrainerNormal.SetAgentFlags(this._advancedMeleeTrainerNormal.GetAgentFlags() & ~AgentFlag.CanGetAlarmed);
			this._advancedMeleeTrainerNormalInitialPosition = base.Mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_normal").GetGlobalFrame();
			this._advancedMeleeTrainerNormalSecondPosition = base.Mission.Scene.FindEntityWithTag("adv_melee_npc_normal_second_pos").GetGlobalFrame();
			this.BeginNPCFight();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00015920 File Offset: 0x00013B20
		private Agent SpawnAdvancedMeleeTrainerEasy()
		{
			this._advancedMeleeTrainerEasyInitialPosition = MatrixFrame.Identity;
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_easy");
			if (gameEntity != null)
			{
				this._advancedMeleeTrainerEasyInitialPosition = gameEntity.GetGlobalFrame();
				this._advancedMeleeTrainerEasyInitialPosition.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			}
			else
			{
				Debug.FailedAssert("There are no spawn points for advanced melee trainer.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Missions\\TrainingFieldMissionController.cs", "SpawnAdvancedMeleeTrainerEasy", 1324);
			}
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("tutorial_npc_advanced_melee_easy");
			AgentBuildData agentBuildData = new AgentBuildData(@object).Team(base.Mission.PlayerTeam).InitialPosition(in this._advancedMeleeTrainerEasyInitialPosition.origin);
			Vec2 asVec = this._advancedMeleeTrainerEasyInitialPosition.rotation.f.AsVec2;
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec).CivilianEquipment(false).NoHorses(true)
				.NoWeapons(false)
				.ClothingColor1(base.Mission.PlayerTeam.Color)
				.ClothingColor2(base.Mission.PlayerTeam.Color2)
				.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, @object, -1, default(UniqueTroopDescriptor), false, false))
				.MountKey(MountCreationKey.GetRandomMountKeyString(@object.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, @object.GetMountKeySeed()))
				.Controller(AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.SetTeam(Mission.Current.DefenderTeam, false);
			return agent;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00015A8C File Offset: 0x00013C8C
		private Agent SpawnAdvancedMeleeTrainerNormal()
		{
			this._advancedMeleeTrainerNormalInitialPosition = MatrixFrame.Identity;
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("spawner_adv_melee_npc_normal");
			if (gameEntity != null)
			{
				this._advancedMeleeTrainerNormalInitialPosition = gameEntity.GetGlobalFrame();
				this._advancedMeleeTrainerNormalInitialPosition.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			}
			else
			{
				Debug.FailedAssert("There are no spawn points for advanced melee trainer.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Missions\\TrainingFieldMissionController.cs", "SpawnAdvancedMeleeTrainerNormal", 1356);
			}
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("tutorial_npc_advanced_melee_normal");
			AgentBuildData agentBuildData = new AgentBuildData(@object).Team(base.Mission.PlayerTeam).InitialPosition(in this._advancedMeleeTrainerNormalInitialPosition.origin);
			Vec2 asVec = this._advancedMeleeTrainerNormalInitialPosition.rotation.f.AsVec2;
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec).CivilianEquipment(false).NoHorses(true)
				.NoWeapons(false)
				.ClothingColor1(base.Mission.PlayerTeam.Color)
				.ClothingColor2(base.Mission.PlayerTeam.Color2)
				.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, @object, -1, default(UniqueTroopDescriptor), false, false))
				.MountKey(MountCreationKey.GetRandomMountKeyString(@object.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, @object.GetMountKeySeed()))
				.Controller(AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.SetTeam(Mission.Current.DefenderTeam, false);
			return agent;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00015BF8 File Offset: 0x00013DF8
		private void AdvancedMeleeTrainingUpdate()
		{
			if (this._trainingSubTypeIndex != -1)
			{
				switch (this._trainingProgress)
				{
				case 1:
					if (this.HasAllWeaponsPicked())
					{
						this._playerLeftBattleArea = false;
						this._detailedObjectives = this._advMeleeObjectives.ConvertAll<TrainingFieldMissionController.TutorialObjective>((TrainingFieldMissionController.TutorialObjective x) => new TrainingFieldMissionController.TutorialObjective(x.Id, x.IsFinished, x.IsActive, x.HasBackground));
						this._detailedObjectives[0].SetActive(true);
						this._trainingProgress++;
						this.CurrentObjectiveTick(new TextObject("{=HhuBPfJn}Go to the trainer.", null));
						WorldPosition worldPosition = this._advancedMeleeTrainerNormalSecondPosition.origin.ToWorldPosition();
						this._advancedMeleeTrainerNormal.SetScriptedPositionAndDirection(ref worldPosition, this._advancedMeleeTrainerNormalSecondPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
						this._advancedMeleeTrainerNormal.SetTeam(Mission.Current.PlayerAllyTeam, false);
						this._advancedMeleeTrainerEasy.SetTeam(Mission.Current.PlayerAllyTeam, false);
						return;
					}
					break;
				case 2:
					if ((this._advancedMeleeTrainerEasy.Position - Agent.Main.Position).LengthSquared < 6f)
					{
						this._detailedObjectives[0].FinishTask();
						this._detailedObjectives[1].SetActive(true);
						this._timer = base.Mission.CurrentTime;
						this._trainingProgress++;
						this._fightStartsIn.SetTextVariable("REMAINING_TIME", 3);
						this.CurrentObjectiveTick(this._fightStartsIn);
						return;
					}
					break;
				case 3:
					if (base.Mission.CurrentTime - this._timer > 3f)
					{
						this._playerHealth = Agent.Main.HealthLimit;
						this._advancedMeleeTrainerEasyHealth = this._advancedMeleeTrainerEasy.HealthLimit;
						this._advancedMeleeTrainerNormal.SetTeam(Mission.Current.PlayerEnemyTeam, false);
						this._advancedMeleeTrainerEasy.SetTeam(Mission.Current.PlayerEnemyTeam, false);
						this._advancedMeleeTrainerEasy.SetWatchState(Agent.WatchState.Alarmed);
						this._advancedMeleeTrainerEasy.DisableScriptedMovement();
						this._trainingProgress++;
						this.CurrentObjectiveTick(new TextObject("{=4hdp6SK0}Defeat the trainer!", null));
						return;
					}
					if (base.Mission.CurrentTime - this._timer > 2f)
					{
						this._fightStartsIn.SetTextVariable("REMAINING_TIME", 1);
						this.CurrentObjectiveTick(this._fightStartsIn);
						return;
					}
					if (base.Mission.CurrentTime - this._timer > 1f)
					{
						this._fightStartsIn.SetTextVariable("REMAINING_TIME", 2);
						this.CurrentObjectiveTick(this._fightStartsIn);
						return;
					}
					break;
				case 4:
					if (this._playerHealth <= 1f)
					{
						this._trainingProgress = 9;
						this.CurrentObjectiveTick(new TextObject("{=SvYCz6z6}You've lost. You can restart the training by interacting weapon rack.", null));
						this._timer = base.Mission.CurrentTime;
						Agent.Main.SetActionChannel(0, in ActionIndexCache.act_strike_fall_back_back_rise, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						Agent.Main.Health = 1.1f;
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/player_lose"), this._advancedMeleeTrainerNormal.GetEyeGlobalPosition(), true, false, -1, -1);
						this.OnLost();
						return;
					}
					if (this._advancedMeleeTrainerEasyHealth <= 1f)
					{
						this._detailedObjectives[1].FinishTask();
						this._detailedObjectives[2].SetActive(true);
						this.CurrentObjectiveTick(new TextObject("{=ikhWkw7T}You've successfully defeated rookie trainer. Go to veteran trainer.", null));
						this._timer = base.Mission.CurrentTime;
						this._trainingProgress++;
						this.OnEasyTrainerBeaten();
						this._advancedMeleeTrainerNormal.SetTeam(Mission.Current.PlayerAllyTeam, false);
						this._advancedMeleeTrainerEasy.SetTeam(Mission.Current.PlayerAllyTeam, false);
						this._advancedMeleeTrainerEasy.SetActionChannel(0, in ActionIndexCache.act_strike_fall_back_back_rise, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						return;
					}
					Agent.Main.Health = this._playerHealth;
					this.CheckAndHandlePlayerInsideBattleArea();
					return;
				case 5:
					if ((this._advancedMeleeTrainerNormal.Position - Agent.Main.Position).LengthSquared < 6f && (this._advancedMeleeTrainerNormal.Position - this._advancedMeleeTrainerNormalInitialPosition.origin).LengthSquared < 6f)
					{
						this._timer = base.Mission.CurrentTime;
						this._trainingProgress++;
						this._fightStartsIn.SetTextVariable("REMAINING_TIME", 3);
						this.CurrentObjectiveTick(this._fightStartsIn);
						return;
					}
					break;
				case 6:
					if (base.Mission.CurrentTime - this._timer > 3f)
					{
						this._playerHealth = Agent.Main.HealthLimit;
						this._advancedMeleeTrainerNormalHealth = this._advancedMeleeTrainerNormal.HealthLimit;
						this._advancedMeleeTrainerNormal.SetTeam(Mission.Current.PlayerEnemyTeam, false);
						this._advancedMeleeTrainerEasy.SetTeam(Mission.Current.PlayerEnemyTeam, false);
						this._advancedMeleeTrainerNormal.SetWatchState(Agent.WatchState.Alarmed);
						this._advancedMeleeTrainerNormal.DisableScriptedMovement();
						this._trainingProgress++;
						this.CurrentObjectiveTick(new TextObject("{=4hdp6SK0}Defeat the trainer!", null));
						return;
					}
					if (base.Mission.CurrentTime - this._timer > 2f)
					{
						this._fightStartsIn.SetTextVariable("REMAINING_TIME", 1);
						this.CurrentObjectiveTick(this._fightStartsIn);
						return;
					}
					if (base.Mission.CurrentTime - this._timer > 1f)
					{
						this._fightStartsIn.SetTextVariable("REMAINING_TIME", 2);
						this.CurrentObjectiveTick(this._fightStartsIn);
						return;
					}
					break;
				case 7:
					if (this._playerHealth <= 1f)
					{
						this.ResetTrainingArea();
						this.CurrentObjectiveTick(new TextObject("{=SvYCz6z6}You've lost. You can restart the training by interacting weapon rack.", null));
						this._timer = base.Mission.CurrentTime;
						this._trainingProgress++;
						Agent.Main.SetActionChannel(0, in ActionIndexCache.act_strike_fall_back_back_rise, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						Agent.Main.Health = 1.1f;
						this.OnLost();
						return;
					}
					if (this._advancedMeleeTrainerNormalHealth <= 1f)
					{
						this._detailedObjectives[2].FinishTask();
						this.SuccessfullyFinishTraining(0f);
						this.CurrentObjectiveTick(new TextObject("{=1RaUauBS}You've successfully finished the training.", null));
						this._timer = base.Mission.CurrentTime;
						this._trainingProgress++;
						this.MakeTrainersPatrolling();
						this._advancedMeleeTrainerNormal.SetActionChannel(0, in ActionIndexCache.act_strike_fall_back_back_rise, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/player_win"), this._advancedMeleeTrainerNormal.GetEyeGlobalPosition(), true, false, -1, -1);
						return;
					}
					Agent.Main.Health = this._playerHealth;
					this.CheckAndHandlePlayerInsideBattleArea();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001639C File Offset: 0x0001459C
		private void CheckAndHandlePlayerInsideBattleArea()
		{
			string[] array;
			if (this._activeTutorialArea.IsPositionInsideTutorialArea(Agent.Main.Position, out array))
			{
				if (string.IsNullOrEmpty(array.FirstOrDefault<string>((string x) => x == "battle_area")))
				{
					if (!this._playerLeftBattleArea)
					{
						this._playerLeftBattleArea = true;
						this.OnPlayerLeftBattleArea();
						return;
					}
				}
				else if (this._playerLeftBattleArea)
				{
					this._playerLeftBattleArea = false;
					this.OnPlayerReEnteredBattleArea();
				}
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001641C File Offset: 0x0001461C
		private void OnPlayerLeftBattleArea()
		{
			int trainingProgress = this._trainingProgress;
			if (trainingProgress == 4)
			{
				this._advancedMeleeTrainerEasy.SetWatchState(Agent.WatchState.Patrolling);
				WorldPosition worldPosition = this._advancedMeleeTrainerEasyInitialPosition.origin.ToWorldPosition();
				this._advancedMeleeTrainerEasy.SetScriptedPositionAndDirection(ref worldPosition, this._advancedMeleeTrainerEasySecondPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
				return;
			}
			if (trainingProgress != 7)
			{
				return;
			}
			this._advancedMeleeTrainerNormal.SetWatchState(Agent.WatchState.Patrolling);
			WorldPosition worldPosition2 = this._advancedMeleeTrainerNormalInitialPosition.origin.ToWorldPosition();
			this._advancedMeleeTrainerNormal.SetScriptedPositionAndDirection(ref worldPosition2, this._advancedMeleeTrainerNormalInitialPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000164CC File Offset: 0x000146CC
		private void OnPlayerReEnteredBattleArea()
		{
			int trainingProgress = this._trainingProgress;
			if (trainingProgress == 4)
			{
				this._advancedMeleeTrainerEasy.DisableScriptedMovement();
				this._advancedMeleeTrainerEasy.SetWatchState(Agent.WatchState.Alarmed);
				return;
			}
			if (trainingProgress != 7)
			{
				return;
			}
			this._advancedMeleeTrainerNormal.DisableScriptedMovement();
			this._advancedMeleeTrainerNormal.SetWatchState(Agent.WatchState.Alarmed);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00016518 File Offset: 0x00014718
		private void OnEasyTrainerBeaten()
		{
			this._advancedMeleeTrainerEasy.SetWatchState(Agent.WatchState.Patrolling);
			WorldPosition worldPosition = this._advancedMeleeTrainerEasySecondPosition.origin.ToWorldPosition();
			this._advancedMeleeTrainerEasy.SetScriptedPositionAndDirection(ref worldPosition, this._advancedMeleeTrainerEasySecondPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
			this._advancedMeleeTrainerNormal.SetWatchState(Agent.WatchState.Patrolling);
			WorldPosition worldPosition2 = this._advancedMeleeTrainerNormalInitialPosition.origin.ToWorldPosition();
			this._advancedMeleeTrainerNormal.SetScriptedPositionAndDirection(ref worldPosition2, this._advancedMeleeTrainerNormalInitialPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
			Agent.Main.Health = Agent.Main.HealthLimit;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000165CC File Offset: 0x000147CC
		private void MakeTrainersPatrolling()
		{
			WorldPosition worldPosition = this._advancedMeleeTrainerEasyInitialPosition.origin.ToWorldPosition();
			this._advancedMeleeTrainerEasy.SetWatchState(Agent.WatchState.Patrolling);
			this._advancedMeleeTrainerEasy.SetTeam(Mission.Current.PlayerAllyTeam, false);
			this._advancedMeleeTrainerEasy.SetScriptedPositionAndDirection(ref worldPosition, this._advancedMeleeTrainerEasyInitialPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
			TrainingFieldMissionController.SetAgentDefensiveness(this._advancedMeleeTrainerNormal, 0f);
			WorldPosition worldPosition2 = this._advancedMeleeTrainerNormalInitialPosition.origin.ToWorldPosition();
			this._advancedMeleeTrainerNormal.SetWatchState(Agent.WatchState.Patrolling);
			this._advancedMeleeTrainerNormal.SetTeam(Mission.Current.PlayerAllyTeam, false);
			this._advancedMeleeTrainerNormal.SetScriptedPositionAndDirection(ref worldPosition2, this._advancedMeleeTrainerNormalInitialPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
			TrainingFieldMissionController.SetAgentDefensiveness(this._advancedMeleeTrainerNormal, 0f);
			this._delayedActions.Add(new TrainingFieldMissionController.DelayedAction(delegate
			{
				Agent.Main.Health = Agent.Main.HealthLimit;
			}, 1.5f));
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000166EB File Offset: 0x000148EB
		private void OnLost()
		{
			this.MakeTrainersPatrolling();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000166F4 File Offset: 0x000148F4
		private void BeginNPCFight()
		{
			this._advancedMeleeTrainerEasy.DisableScriptedMovement();
			this._advancedMeleeTrainerEasy.SetWatchState(Agent.WatchState.Alarmed);
			this._advancedMeleeTrainerEasy.SetTeam(Mission.Current.DefenderTeam, false);
			TrainingFieldMissionController.SetAgentDefensiveness(this._advancedMeleeTrainerEasy, 4f);
			this._advancedMeleeTrainerNormal.DisableScriptedMovement();
			this._advancedMeleeTrainerNormal.SetWatchState(Agent.WatchState.Alarmed);
			this._advancedMeleeTrainerNormal.SetTeam(Mission.Current.AttackerTeam, false);
			TrainingFieldMissionController.SetAgentDefensiveness(this._advancedMeleeTrainerNormal, 4f);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001677B File Offset: 0x0001497B
		private void OnAdvancedTrainingStart()
		{
			this.MakeTrainersPatrolling();
			Agent.Main.Health = Agent.Main.HealthLimit;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00016797 File Offset: 0x00014997
		private void OnAdvancedTrainingExit()
		{
			Agent.Main.Health = Agent.Main.HealthLimit;
			this.BeginNPCFight();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x000167B3 File Offset: 0x000149B3
		private void OnAdvancedTrainingAreaEnter()
		{
			this.MakeTrainersPatrolling();
			Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/greet"), this._advancedMeleeTrainerNormal.GetEyeGlobalPosition(), true, false, -1, -1);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000167DE File Offset: 0x000149DE
		private static void SetAgentDefensiveness(Agent agent, float formationOrderDefensivenessFactor)
		{
			agent.Defensiveness = formationOrderDefensivenessFactor;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000167E8 File Offset: 0x000149E8
		private void InitializeMeleeTraining()
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("spawner_melee_npc");
			if (gameEntity != null)
			{
				matrixFrame = gameEntity.GetGlobalFrame();
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			}
			else
			{
				Debug.FailedAssert("There are no spawn points for basic melee trainer.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Missions\\TrainingFieldMissionController.cs", "InitializeMeleeTraining", 1734);
			}
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("tutorial_npc_basic_melee");
			AgentBuildData agentBuildData = new AgentBuildData(@object).Team(base.Mission.PlayerTeam).InitialPosition(in matrixFrame.origin);
			Vec2 asVec = matrixFrame.rotation.f.AsVec2;
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec).CivilianEquipment(false).NoHorses(true)
				.NoWeapons(false)
				.ClothingColor1(base.Mission.PlayerTeam.Color)
				.ClothingColor2(base.Mission.PlayerTeam.Color2)
				.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, @object, -1, default(UniqueTroopDescriptor), false, false))
				.MountKey(MountCreationKey.GetRandomMountKeyString(@object.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, @object.GetMountKeySeed()))
				.Controller(AgentControllerType.None);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.SetTeam(Mission.Current.DefenderTeam, false);
			this._meleeTrainer = agent;
			this._meleeTrainerDefaultPosition = this._meleeTrainer.GetWorldPosition();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001695C File Offset: 0x00014B5C
		private void MeleeTrainingUpdate()
		{
			float lengthSquared = (this._meleeTrainer.Position - this._meleeTrainerDefaultPosition.GetGroundVec3()).LengthSquared;
			if (lengthSquared > 1f)
			{
				if (this._meleeTrainer.MovementFlags == Agent.MovementControlFlag.DefendDown)
				{
					this._meleeTrainer.MovementFlags &= ~Agent.MovementControlFlag.DefendDown;
				}
				else if ((this._meleeTrainer.MovementFlags & Agent.MovementControlFlag.AttackMask) > Agent.MovementControlFlag.None)
				{
					this._meleeTrainer.MovementFlags &= ~(Agent.MovementControlFlag.AttackLeft | Agent.MovementControlFlag.AttackRight | Agent.MovementControlFlag.AttackUp | Agent.MovementControlFlag.AttackDown);
					this._meleeTrainer.MovementFlags |= Agent.MovementControlFlag.DefendDown;
				}
				else
				{
					this._meleeTrainer.SetTargetPosition(this._meleeTrainerDefaultPosition.AsVec2);
				}
				this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.None);
				return;
			}
			if (lengthSquared < 0.1f)
			{
				this.SwordTraining();
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016A34 File Offset: 0x00014C34
		private void SwordTraining()
		{
			if (this._trainingProgress == 1)
			{
				if (this.HasAllWeaponsPicked())
				{
					this._detailedObjectives = this._meleeObjectives.ConvertAll<TrainingFieldMissionController.TutorialObjective>((TrainingFieldMissionController.TutorialObjective x) => new TrainingFieldMissionController.TutorialObjective(x.Id, x.IsFinished, x.IsActive, x.HasBackground));
					this._detailedObjectives[1].SetTextVariableOfName("HIT", 0);
					this._detailedObjectives[1].SetTextVariableOfName("ALL", 4);
					this._detailedObjectives[2].SetTextVariableOfName("HIT", 0);
					this._detailedObjectives[2].SetTextVariableOfName("ALL", 4);
					this._detailedObjectives[0].SetActive(true);
					this._trainingProgress++;
					this.CurrentObjectiveTick(new TextObject("{=Zb1uFhsY}Go to trainer.", null));
				}
				this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.None);
				return;
			}
			Vec3 vec = this._meleeTrainer.Position - Agent.Main.Position;
			if (vec.LengthSquared < 4f)
			{
				Agent meleeTrainer = this._meleeTrainer;
				vec = this._meleeTrainer.Position;
				Vec2 asVec = vec.AsVec2;
				vec = Agent.Main.GetEyeGlobalPosition() - this._meleeTrainer.GetWorldFrame().Rotation.s * 0.1f - this._meleeTrainer.GetEyeGlobalPosition();
				meleeTrainer.SetTargetPositionAndDirection(in asVec, in vec);
				switch (this._trainingProgress)
				{
				case 2:
					this._detailedObjectives[0].FinishTask();
					this._detailedObjectives[1].SetActive(true);
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/block_left"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
					this.CurrentObjectiveTick(new TextObject("{=Db98U6fF}Defend from left.", null));
					this._trainingProgress++;
					return;
				case 3:
					if (base.Mission.CurrentTime - this._timer > 2f && Agent.Main.GetCurrentActionDirection(1) == Agent.UsageDirection.DefendLeft && Agent.Main.GetCurrentActionProgress(1) > 0.1f && Agent.Main.GetCurrentActionType(1) != Agent.ActionCodeType.Guard)
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.None;
						this._timer = base.Mission.CurrentTime;
					}
					else
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.AttackRight;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.DefendLeft);
					return;
				case 4:
					if (base.Mission.CurrentTime - this._timer > 1.5f && Agent.Main.GetCurrentActionDirection(1) == Agent.UsageDirection.DefendRight && Agent.Main.GetCurrentActionProgress(1) > 0.1f && Agent.Main.GetCurrentActionType(1) != Agent.ActionCodeType.Guard)
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.None;
						this._timer = base.Mission.CurrentTime;
					}
					else
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.AttackLeft;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.DefendRight);
					return;
				case 5:
					if (base.Mission.CurrentTime - this._timer > 1.5f && Agent.Main.GetCurrentActionDirection(1) == Agent.UsageDirection.AttackEnd && Agent.Main.GetCurrentActionProgress(1) > 0.1f && Agent.Main.GetCurrentActionType(1) != Agent.ActionCodeType.Guard)
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.None;
						this._timer = base.Mission.CurrentTime;
					}
					else
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.AttackUp;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.DefendUp);
					return;
				case 6:
					if (base.Mission.CurrentTime - this._timer > 1.5f && Agent.Main.GetCurrentActionDirection(1) == Agent.UsageDirection.DefendDown && Agent.Main.GetCurrentActionProgress(1) > 0.1f && Agent.Main.GetCurrentActionType(1) != Agent.ActionCodeType.Guard)
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.None;
						this._timer = base.Mission.CurrentTime;
					}
					else
					{
						this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.AttackDown;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.DefendDown);
					return;
				case 7:
					this._meleeTrainer.MovementFlags |= Agent.MovementControlFlag.DefendRight;
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.AttackLeft);
					return;
				case 8:
					if (base.Mission.CurrentTime - this._timer > 1f)
					{
						this._meleeTrainer.MovementFlags |= Agent.MovementControlFlag.DefendLeft;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.AttackRight);
					return;
				case 9:
					if (base.Mission.CurrentTime - this._timer > 1f)
					{
						this._meleeTrainer.MovementFlags |= Agent.MovementControlFlag.DefendUp;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.AttackUp);
					return;
				case 10:
					if (base.Mission.CurrentTime - this._timer > 1f)
					{
						this._meleeTrainer.MovementFlags |= Agent.MovementControlFlag.DefendDown;
					}
					this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.AttackDown);
					return;
				case 11:
					this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.None;
					this._trainingProgress++;
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/praise"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
					this.SuccessfullyFinishTraining(0f);
					return;
				default:
					return;
				}
			}
			else
			{
				this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.None);
				if (this._meleeTrainer.MovementFlags == Agent.MovementControlFlag.DefendDown)
				{
					this._meleeTrainer.MovementFlags &= ~Agent.MovementControlFlag.DefendDown;
					return;
				}
				if ((this._meleeTrainer.MovementFlags & Agent.MovementControlFlag.AttackMask) > Agent.MovementControlFlag.None)
				{
					this._meleeTrainer.MovementFlags &= ~(Agent.MovementControlFlag.AttackLeft | Agent.MovementControlFlag.AttackRight | Agent.MovementControlFlag.AttackUp | Agent.MovementControlFlag.AttackDown);
					this._meleeTrainer.MovementFlags |= Agent.MovementControlFlag.DefendDown;
				}
				return;
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00016FD4 File Offset: 0x000151D4
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			base.OnScoreHit(affectedAgent, affectorAgent, attackerWeapon, isBlocked, isSiegeEngineHit, in blow, in collisionData, damagedHp, hitDistance, shotDifficulty);
			if (isBlocked)
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex <= EquipmentIndex.Weapon3; equipmentIndex++)
				{
					if (!affectedAgent.Equipment[equipmentIndex].IsEmpty && affectedAgent.Equipment[equipmentIndex].IsShield())
					{
						affectedAgent.ChangeWeaponHitPoints(equipmentIndex, affectedAgent.Equipment[equipmentIndex].ModifiedMaxHitPoints);
					}
				}
			}
			TutorialArea activeTutorialArea = this._activeTutorialArea;
			if (activeTutorialArea != null && activeTutorialArea.TypeOfTraining == TutorialArea.TrainingType.Melee)
			{
				if (affectedAgent.Controller == AgentControllerType.Player)
				{
					if (this._trainingProgress >= 3 && this._trainingProgress <= 6 && isBlocked)
					{
						this._timer = base.Mission.CurrentTime;
						if (this._trainingProgress == 3 && affectedAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.DefendLeft)
						{
							this._detailedObjectives[1].SetTextVariableOfName("HIT", 1);
							Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/block_right"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
							this.CurrentObjectiveTick(new TextObject("{=7wmkPNbI}Defend from right.", null));
							this._trainingProgress++;
						}
						else if (this._trainingProgress == 4 && affectedAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.DefendRight)
						{
							this._detailedObjectives[1].SetTextVariableOfName("HIT", 2);
							Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/block_up"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
							this.CurrentObjectiveTick(new TextObject("{=CEqKkY3m}Defend from up.", null));
							this._trainingProgress++;
						}
						else if (this._trainingProgress == 5 && affectedAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.AttackEnd)
						{
							this._detailedObjectives[1].SetTextVariableOfName("HIT", 3);
							Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/block_down"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
							this.CurrentObjectiveTick(new TextObject("{=Qdz5Hely}Defend from down.", null));
							this._trainingProgress++;
						}
						else if (this._trainingProgress == 6 && affectedAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.DefendDown)
						{
							this._detailedObjectives[1].SetTextVariableOfName("HIT", 4);
							this._detailedObjectives[1].FinishTask();
							this._detailedObjectives[2].SetActive(true);
							Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/attack_left"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
							this.CurrentObjectiveTick(new TextObject("{=8QX1QHAJ}Attack from left.", null));
							this._trainingProgress++;
						}
					}
				}
				else if (affectedAgent == this._meleeTrainer && affectorAgent != null && affectorAgent.Controller == AgentControllerType.Player && (this._trainingProgress >= 7 && this._trainingProgress <= 10 && isBlocked))
				{
					this._meleeTrainer.MovementFlags = Agent.MovementControlFlag.None;
					this._timer = base.Mission.CurrentTime;
					if (this._trainingProgress == 7 && affectorAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.AttackLeft)
					{
						this._detailedObjectives[2].SetTextVariableOfName("HIT", 1);
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/attack_right"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
						this.CurrentObjectiveTick(new TextObject("{=fC60rYwy}Attack from right.", null));
						this._trainingProgress++;
					}
					else if (this._trainingProgress == 8 && affectorAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.AttackRight)
					{
						this._detailedObjectives[2].SetTextVariableOfName("HIT", 2);
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/attack_up"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
						this.CurrentObjectiveTick(new TextObject("{=j2dW9fZt}Attack from up.", null));
						this._trainingProgress++;
					}
					else if (this._trainingProgress == 9 && affectorAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.AttackUp)
					{
						this._detailedObjectives[2].SetTextVariableOfName("HIT", 3);
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/attack_down"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
						this.CurrentObjectiveTick(new TextObject("{=X9Vmjipn}Attack from down.", null));
						this._trainingProgress++;
					}
					else if (this._trainingProgress == 10 && affectorAgent.GetCurrentActionDirection(1) == Agent.UsageDirection.AttackDown)
					{
						this._detailedObjectives[2].SetTextVariableOfName("HIT", 4);
						this._detailedObjectives[2].FinishTask();
						this.CurrentObjectiveTick(this._trainingFinishedText);
						this.TickMouseObjective(TrainingFieldMissionController.MouseObjectives.None);
						MBInformationManager.AddQuickInformation(Agent.Main.Equipment.HasShield() ? new TextObject("{=PiOiQ3u5}You've successfully finished the sword and shield tutorial.", null) : new TextObject("{=GZaYmg95}You've successfully finished the sword tutorial.", null), 0, null, null, "");
						this._trainingProgress++;
					}
					else
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=fBJRdxh2}Try again.", null), 0, null, null, "");
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/parrying/remark"), this._meleeTrainer.GetEyeGlobalPosition(), true, false, -1, -1);
					}
				}
			}
			if (!isBlocked)
			{
				if (affectedAgent.Controller == AgentControllerType.Player)
				{
					this._playerHealth -= (float)blow.InflictedDamage;
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/fighting/warning"), this._advancedMeleeTrainerNormal.GetEyeGlobalPosition(), true, false, -1, -1);
					return;
				}
				if (affectedAgent == this._advancedMeleeTrainerEasy)
				{
					this._advancedMeleeTrainerEasyHealth -= (float)blow.InflictedDamage;
					return;
				}
				if (affectedAgent == this._advancedMeleeTrainerNormal)
				{
					this._advancedMeleeTrainerNormalHealth -= (float)blow.InflictedDamage;
				}
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000175C6 File Offset: 0x000157C6
		private void TickMouseObjective(TrainingFieldMissionController.MouseObjectives objective)
		{
			Action<TrainingFieldMissionController.MouseObjectives, TrainingFieldMissionController.ObjectivePerformingType> currentMouseObjectiveTick = this.CurrentMouseObjectiveTick;
			if (currentMouseObjectiveTick == null)
			{
				return;
			}
			currentMouseObjectiveTick(TrainingFieldMissionController.GetAdjustedMouseObjective(objective), TrainingFieldMissionController.GetObjectivePerformingType(objective));
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000175E4 File Offset: 0x000157E4
		private static bool IsAttackDirection(TrainingFieldMissionController.MouseObjectives objective)
		{
			return objective - TrainingFieldMissionController.MouseObjectives.AttackLeft <= 3;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000175EF File Offset: 0x000157EF
		private static TrainingFieldMissionController.MouseObjectives GetAdjustedMouseObjective(TrainingFieldMissionController.MouseObjectives baseObjective)
		{
			if (!TrainingFieldMissionController.IsAttackDirection(baseObjective))
			{
				return baseObjective;
			}
			if (BannerlordConfig.AttackDirectionControl != 0)
			{
				return baseObjective;
			}
			return TrainingFieldMissionController.GetInverseDirection(baseObjective);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001760C File Offset: 0x0001580C
		private static TrainingFieldMissionController.ObjectivePerformingType GetObjectivePerformingType(TrainingFieldMissionController.MouseObjectives baseObjective)
		{
			if (TrainingFieldMissionController.IsAttackDirection(baseObjective))
			{
				int attackDirectionControl = BannerlordConfig.AttackDirectionControl;
				if (attackDirectionControl == 0)
				{
					return TrainingFieldMissionController.ObjectivePerformingType.ByLookDirection;
				}
				if (attackDirectionControl != 1)
				{
					return TrainingFieldMissionController.ObjectivePerformingType.ByMovement;
				}
				return TrainingFieldMissionController.ObjectivePerformingType.ByLookDirection;
			}
			else
			{
				int defendDirectionControl = BannerlordConfig.DefendDirectionControl;
				if (defendDirectionControl == 0)
				{
					return TrainingFieldMissionController.ObjectivePerformingType.ByLookDirection;
				}
				if (defendDirectionControl != 1)
				{
					return TrainingFieldMissionController.ObjectivePerformingType.AutoBlock;
				}
				return TrainingFieldMissionController.ObjectivePerformingType.ByMovement;
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00017648 File Offset: 0x00015848
		private static TrainingFieldMissionController.MouseObjectives GetInverseDirection(TrainingFieldMissionController.MouseObjectives objective)
		{
			switch (objective)
			{
			case TrainingFieldMissionController.MouseObjectives.None:
				return TrainingFieldMissionController.MouseObjectives.None;
			case TrainingFieldMissionController.MouseObjectives.AttackLeft:
				return TrainingFieldMissionController.MouseObjectives.AttackRight;
			case TrainingFieldMissionController.MouseObjectives.AttackRight:
				return TrainingFieldMissionController.MouseObjectives.AttackLeft;
			case TrainingFieldMissionController.MouseObjectives.AttackUp:
				return TrainingFieldMissionController.MouseObjectives.AttackDown;
			case TrainingFieldMissionController.MouseObjectives.AttackDown:
				return TrainingFieldMissionController.MouseObjectives.AttackUp;
			case TrainingFieldMissionController.MouseObjectives.DefendLeft:
				return TrainingFieldMissionController.MouseObjectives.DefendRight;
			case TrainingFieldMissionController.MouseObjectives.DefendRight:
				return TrainingFieldMissionController.MouseObjectives.DefendLeft;
			case TrainingFieldMissionController.MouseObjectives.DefendUp:
				return TrainingFieldMissionController.MouseObjectives.DefendDown;
			case TrainingFieldMissionController.MouseObjectives.DefendDown:
				return TrainingFieldMissionController.MouseObjectives.DefendUp;
			default:
				Debug.FailedAssert(string.Format("Inverse direction is not defined for: {0}", objective), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Missions\\TrainingFieldMissionController.cs", "GetInverseDirection", 2213);
				return TrainingFieldMissionController.MouseObjectives.None;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000176B8 File Offset: 0x000158B8
		private void InitializeMountedTraining()
		{
			this._horse = this.SpawnHorse();
			this._horse.Controller = AgentControllerType.None;
			this._horseBeginningPosition = this._horse.GetWorldPosition();
			this._finishGateClosed = base.Mission.Scene.FindEntityWithTag("finish_gate_closed");
			this._finishGateOpen = base.Mission.Scene.FindEntityWithTag("finish_gate_open");
			this._mountedAIWaitingPosition = base.Mission.Scene.FindEntityWithTag("_mounted_ai_waiting_position").GetGlobalFrame();
			this._mountedAI = this.SpawnMountedAI();
			this._mountedAI.SetWatchState(Agent.WatchState.Alarmed);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001775C File Offset: 0x0001595C
		private Agent SpawnMountedAI()
		{
			this._mountedAISpawnPosition = MatrixFrame.Identity;
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("_mounted_ai_spawn_position");
			if (gameEntity != null)
			{
				this._mountedAISpawnPosition = gameEntity.GetGlobalFrame();
				this._mountedAISpawnPosition.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			}
			else
			{
				Debug.FailedAssert("There are no spawn points for mounted ai.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Missions\\TrainingFieldMissionController.cs", "SpawnMountedAI", 2258);
			}
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("tutorial_npc_mounted_ai");
			AgentBuildData agentBuildData = new AgentBuildData(@object).Team(base.Mission.PlayerTeam).InitialPosition(in this._mountedAISpawnPosition.origin);
			Vec2 asVec = this._mountedAISpawnPosition.rotation.f.AsVec2;
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in asVec).CivilianEquipment(false).NoHorses(false)
				.NoWeapons(false)
				.ClothingColor1(base.Mission.PlayerTeam.Color)
				.ClothingColor2(base.Mission.PlayerTeam.Color2)
				.TroopOrigin(new PartyAgentOrigin(PartyBase.MainParty, @object, -1, default(UniqueTroopDescriptor), false, false))
				.MountKey(MountCreationKey.GetRandomMountKeyString(@object.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, @object.GetMountKeySeed()))
				.Controller(AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.SetTeam(Mission.Current.PlayerTeam, false);
			return agent;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000178C8 File Offset: 0x00015AC8
		private void UpdateMountedAIBehavior()
		{
			if (this._mountedAICurrentCheckpointTarget == -1)
			{
				if (this._continueLoop && (this._mountedAISpawnPosition.origin - this._mountedAI.Position).LengthSquared < 6.25f)
				{
					this._mountedAICurrentCheckpointTarget++;
					MatrixFrame globalFrame = this._checkpoints[this._mountedAICurrentCheckpointTarget].Item1.GameEntity.GetGlobalFrame();
					WorldPosition worldPosition = globalFrame.origin.ToWorldPosition();
					this._mountedAI.SetScriptedPositionAndDirection(ref worldPosition, globalFrame.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
					this.SetFinishGateStatus(false);
					this._mountedAI.SetWatchState(Agent.WatchState.Alarmed);
					return;
				}
			}
			else
			{
				bool flag = false;
				if ((this._checkpoints[this._mountedAICurrentCheckpointTarget].Item1.GameEntity.GetGlobalFrame().origin.ToWorldPosition().AsVec2 - this._mountedAI.Position.ToWorldPosition().AsVec2).LengthSquared < 25f)
				{
					flag = true;
					this._mountedAICurrentCheckpointTarget++;
					if (this._mountedAICurrentCheckpointTarget > this._checkpoints.Count - 1)
					{
						this._mountedAICurrentCheckpointTarget = -1;
						if (this._continueLoop)
						{
							this.GoToStartingPosition();
						}
						else
						{
							WorldPosition worldPosition2 = this._mountedAIWaitingPosition.origin.ToWorldPosition();
							this._mountedAI.SetScriptedPositionAndDirection(ref worldPosition2, this._mountedAISpawnPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
						}
					}
					else if (this._mountedAICurrentCheckpointTarget == this._checkpoints.Count - 1)
					{
						this.SetFinishGateStatus(true);
						this._mountedAI.SetWatchState(Agent.WatchState.Patrolling);
					}
				}
				else if ((this._mountedAITargets[this._mountedAICurrentHitTarget].GameEntity.GetGlobalFrame().origin.ToWorldPosition().AsVec2 - this._mountedAI.Position.ToWorldPosition().AsVec2).LengthSquared < 169f)
				{
					this._enteredRadiusOfTarget = true;
				}
				else if ((!this._allTargetsDestroyed && this._mountedAITargets[this._mountedAICurrentHitTarget].IsDestroyed) || (this._enteredRadiusOfTarget && (this._mountedAITargets[this._mountedAICurrentHitTarget].GameEntity.GetGlobalFrame().origin.ToWorldPosition().AsVec2 - this._mountedAI.Position.ToWorldPosition().AsVec2).LengthSquared > 169f))
				{
					this._enteredRadiusOfTarget = false;
					flag = true;
					this._mountedAICurrentHitTarget++;
					if (this._mountedAICurrentHitTarget > this._mountedAITargets.Count - 1)
					{
						this._mountedAICurrentHitTarget = 0;
						this._allTargetsDestroyed = true;
					}
				}
				if (flag && this._mountedAICurrentCheckpointTarget != -1)
				{
					MatrixFrame globalFrame2 = this._checkpoints[this._mountedAICurrentCheckpointTarget].Item1.GameEntity.GetGlobalFrame();
					WorldPosition worldPosition3 = globalFrame2.origin.ToWorldPosition();
					this._mountedAI.SetScriptedPositionAndDirection(ref worldPosition3, globalFrame2.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
					if (!this._allTargetsDestroyed)
					{
						this._mountedAI.SetScriptedTargetEntity(this._mountedAITargets[this._mountedAICurrentHitTarget].GameEntity, Agent.AISpecialCombatModeFlags.None, false);
					}
				}
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00017C7C File Offset: 0x00015E7C
		private void GoToStartingPosition()
		{
			WorldPosition worldPosition = this._mountedAISpawnPosition.origin.ToWorldPosition();
			this._mountedAI.SetScriptedPositionAndDirection(ref worldPosition, this._mountedAISpawnPosition.rotation.f.AsVec2.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
			this.RestoreAndShowAllMountedAITargets();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00017CCC File Offset: 0x00015ECC
		private void RestoreAndShowAllMountedAITargets()
		{
			this._allTargetsDestroyed = false;
			foreach (DestructableComponent destructableComponent in this._mountedAITargets)
			{
				destructableComponent.Reset();
				destructableComponent.GameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00017D34 File Offset: 0x00015F34
		private void HideAllMountedAITargets()
		{
			this._allTargetsDestroyed = true;
			foreach (DestructableComponent destructableComponent in this._mountedAITargets)
			{
				destructableComponent.Reset();
				destructableComponent.GameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00017D9C File Offset: 0x00015F9C
		private void UpdateHorseBehavior()
		{
			if (this._horse != null && this._horse.RiderAgent == null)
			{
				if (this._horse.IsAIControlled && this._horse.CommonAIComponent.IsPanicked)
				{
					this._horse.CommonAIComponent.StopRetreating();
				}
				if (this._horseBehaviorMode != TrainingFieldMissionController.HorseReturningSituation.BeginReturn)
				{
					string[] array;
					if (!this._trainingAreas.Find((TutorialArea x) => x.TypeOfTraining == TutorialArea.TrainingType.Mounted).IsPositionInsideTutorialArea(this._horse.Position, out array))
					{
						this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.BeginReturn;
						TutorialArea activeTutorialArea = this._activeTutorialArea;
						if (activeTutorialArea != null && activeTutorialArea.TypeOfTraining == TutorialArea.TrainingType.Mounted && this._trainingProgress > 1)
						{
							this.ResetTrainingArea();
							goto IL_0127;
						}
						goto IL_0127;
					}
				}
				TutorialArea activeTutorialArea2 = this._activeTutorialArea;
				if ((activeTutorialArea2 == null || activeTutorialArea2.TypeOfTraining != TutorialArea.TrainingType.Mounted) && (this._horseBehaviorMode == TrainingFieldMissionController.HorseReturningSituation.NotInPosition || this._horseBehaviorMode == TrainingFieldMissionController.HorseReturningSituation.Following))
				{
					this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.BeginReturn;
				}
				else
				{
					TutorialArea activeTutorialArea3 = this._activeTutorialArea;
					if (activeTutorialArea3 != null && activeTutorialArea3.TypeOfTraining == TutorialArea.TrainingType.Mounted && !Agent.Main.HasMount && this._trainingProgress > 2)
					{
						this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.Following;
					}
				}
				IL_0127:
				switch (this._horseBehaviorMode)
				{
				case TrainingFieldMissionController.HorseReturningSituation.BeginReturn:
					if ((this._horse.Position - this._horseBeginningPosition.GetGroundVec3()).Length > 1f)
					{
						this._horse.Controller = AgentControllerType.AI;
						this._horse.SetScriptedPosition(ref this._horseBeginningPosition, false, Agent.AIScriptedFrameFlags.None);
						this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.Returning;
						return;
					}
					this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.ReturnCompleted;
					return;
				case TrainingFieldMissionController.HorseReturningSituation.Returning:
					if ((this._horse.Position - this._horseBeginningPosition.GetGroundVec3()).Length < 0.5f)
					{
						if (this._horse.GetCurrentVelocity().LengthSquared <= 0f)
						{
							this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.ReturnCompleted;
							return;
						}
						if (this._horse.Controller == AgentControllerType.AI)
						{
							this._horse.Controller = AgentControllerType.None;
							this._horse.MovementFlags &= ~(Agent.MovementControlFlag.Forward | Agent.MovementControlFlag.Backward | Agent.MovementControlFlag.StrafeRight | Agent.MovementControlFlag.StrafeLeft | Agent.MovementControlFlag.TurnRight | Agent.MovementControlFlag.TurnLeft);
							this._horse.MovementInputVector = Vec2.Zero;
							return;
						}
					}
					else if (this._horse.Controller == AgentControllerType.None)
					{
						this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.BeginReturn;
						return;
					}
					break;
				case TrainingFieldMissionController.HorseReturningSituation.ReturnCompleted:
					if ((this._horse.Position - this._horseBeginningPosition.GetGroundVec3()).Length > 1f)
					{
						TutorialArea activeTutorialArea4 = this._activeTutorialArea;
						if (activeTutorialArea4 != null && activeTutorialArea4.TypeOfTraining == TutorialArea.TrainingType.Mounted)
						{
							this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.NotInPosition;
							this._horse.Controller = AgentControllerType.None;
							this._horse.MovementFlags &= ~(Agent.MovementControlFlag.Forward | Agent.MovementControlFlag.Backward | Agent.MovementControlFlag.StrafeRight | Agent.MovementControlFlag.StrafeLeft | Agent.MovementControlFlag.TurnRight | Agent.MovementControlFlag.TurnLeft);
							this._horse.MovementInputVector = Vec2.Zero;
							return;
						}
					}
					break;
				case TrainingFieldMissionController.HorseReturningSituation.Following:
					if ((this._horse.Position - Agent.Main.Position).Length > 3f)
					{
						this._horse.Controller = AgentControllerType.AI;
						Vec3 vec = Agent.Main.Position + (this._horse.Position - Agent.Main.Position).NormalizedCopy() * 3f;
						WorldPosition worldPosition = new WorldPosition(Agent.Main.Mission.Scene, vec);
						this._horse.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
						return;
					}
					break;
				default:
					return;
				}
			}
			else if (this._horse.RiderAgent != null && this._horseBehaviorMode != TrainingFieldMissionController.HorseReturningSituation.NotInPosition)
			{
				this._horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.NotInPosition;
				this._horse.Controller = AgentControllerType.None;
				this._horse.MovementFlags &= ~(Agent.MovementControlFlag.Forward | Agent.MovementControlFlag.Backward | Agent.MovementControlFlag.StrafeRight | Agent.MovementControlFlag.StrafeLeft | Agent.MovementControlFlag.TurnRight | Agent.MovementControlFlag.TurnLeft);
				this._horse.MovementInputVector = Vec2.Zero;
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00018158 File Offset: 0x00016358
		private Agent SpawnHorse()
		{
			MatrixFrame globalFrame = base.Mission.Scene.FindEntityWithTag("spawner_horse").GetGlobalFrame();
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("old_horse");
			ItemRosterElement itemRosterElement = new ItemRosterElement(@object, 1, null);
			ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>("light_harness");
			ItemRosterElement itemRosterElement2 = new ItemRosterElement(object2, 0, null);
			Agent agent = null;
			if (@object.HasHorseComponent)
			{
				Mission mission = Mission.Current;
				ItemRosterElement itemRosterElement3 = itemRosterElement;
				ItemRosterElement itemRosterElement4 = itemRosterElement2;
				Vec2 vec = globalFrame.rotation.f.AsVec2;
				vec = vec.Normalized();
				agent = mission.SpawnMonster(itemRosterElement3, itemRosterElement4, in globalFrame.origin, in vec, -1);
				AnimalSpawnSettings.CheckAndSetAnimalAgentFlags(base.Mission.Scene.FindEntityWithTag("spawner_melee_npc"), agent);
			}
			return agent;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00018214 File Offset: 0x00016414
		private void MountedTrainingUpdate()
		{
			bool flag = false;
			if (this._trainingProgress > 2 && this._trainingProgress < 5)
			{
				flag = this.CheckpointUpdate();
			}
			if (Agent.Main.HasMount)
			{
				this._activeTutorialArea.ActivateBoundaries();
			}
			else
			{
				this._activeTutorialArea.HideBoundaries();
			}
			switch (this._trainingProgress)
			{
			case 1:
				if (this.HasAllWeaponsPicked())
				{
					this._detailedObjectives = this._mountedObjectives.ConvertAll<TrainingFieldMissionController.TutorialObjective>((TrainingFieldMissionController.TutorialObjective x) => new TrainingFieldMissionController.TutorialObjective(x.Id, x.IsFinished, x.IsActive, x.HasBackground));
					this._detailedObjectives[1].SetTextVariableOfName("HIT", this._activeTutorialArea.GetBrokenBreakableCount(this._trainingSubTypeIndex));
					this._detailedObjectives[1].SetTextVariableOfName("ALL", this._activeTutorialArea.GetBreakablesCount(this._trainingSubTypeIndex));
					this._detailedObjectives[0].SetActive(true);
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/riding/pick_" + this._trainingSubTypeIndex), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
					TrainingFieldMissionController.SetHorseMountable(true);
					this._mountedLastBrokenTargetCount = 0;
					this._trainingProgress++;
					this.CurrentObjectiveTick(new TextObject("{=h31YaM4b}Mount the horse.", null));
					return;
				}
				break;
			case 2:
				if (Agent.Main.HasMount)
				{
					this._detailedObjectives[0].FinishTask();
					this._detailedObjectives[1].SetActive(true);
					this._activeTutorialArea.ActivateBoundaries();
					this._trainingProgress++;
					this.CurrentObjectiveTick(new TextObject("{=gJBNUAJd}Finish the track and hit as many targets as you can.", null));
					return;
				}
				break;
			case 3:
				if (this._checkpoints[0].Item2)
				{
					this._activeTutorialArea.MakeDestructible(this._trainingSubTypeIndex);
					this._activeTutorialArea.ResetBreakables(this._trainingSubTypeIndex, false);
					this.ResetCheckpoints();
					ValueTuple<VolumeBox, bool> valueTuple = this._checkpoints[0];
					valueTuple.Item2 = true;
					this._checkpoints[0] = valueTuple;
					this.StartTimer();
					this.UIStartTimer();
					MBInformationManager.AddQuickInformation(new TextObject("{=HvGW2DvS}Track started.", null), 0, null, null, "");
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/riding/start_course"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
					this._trainingProgress++;
					return;
				}
				if (!Agent.Main.HasMount)
				{
					this._trainingProgress = 1;
					return;
				}
				break;
			case 4:
			{
				int brokenBreakableCount = this._activeTutorialArea.GetBrokenBreakableCount(this._trainingSubTypeIndex);
				this._detailedObjectives[1].SetTextVariableOfName("HIT", brokenBreakableCount);
				if (brokenBreakableCount != this._mountedLastBrokenTargetCount)
				{
					Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/hit_target"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
					this._mountedLastBrokenTargetCount = brokenBreakableCount;
				}
				if (flag)
				{
					this._detailedObjectives[1].FinishTask();
					this._trainingProgress++;
					this.MountedTrainingEndedSuccessfully();
					return;
				}
				break;
			}
			case 5:
				if (!Agent.Main.HasMount)
				{
					this._trainingProgress++;
					TrainingFieldMissionController.SetHorseMountable(false);
					this.CurrentObjectiveTick(this._trainingFinishedText);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00018570 File Offset: 0x00016770
		private void ResetCheckpoints()
		{
			for (int i = 0; i < this._checkpoints.Count; i++)
			{
				this._checkpoints[i] = ValueTuple.Create<VolumeBox, bool>(this._checkpoints[i].Item1, false);
			}
			this._currentCheckpointIndex = -1;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000185C0 File Offset: 0x000167C0
		private bool CheckpointUpdate()
		{
			for (int i = 0; i < this._checkpoints.Count; i++)
			{
				if (this._checkpoints[i].Item1.IsPointIn(Agent.Main.Position))
				{
					if (this._currentCheckpointIndex == -1)
					{
						this._enteringDotProduct = Vec3.DotProduct(Agent.Main.Velocity, this._checkpoints[i].Item1.GameEntity.GetFrame().rotation.f);
						this._currentCheckpointIndex = i;
					}
					return false;
				}
			}
			bool flag = false;
			if (this._currentCheckpointIndex != -1)
			{
				float num = Vec3.DotProduct(this._checkpoints[this._currentCheckpointIndex].Item1.GameEntity.GetFrame().rotation.f, Agent.Main.Velocity);
				if (num > 0f == this._enteringDotProduct > 0f)
				{
					if ((this._currentCheckpointIndex == 0 || this._checkpoints[this._currentCheckpointIndex - 1].Item2) && num > 0f)
					{
						this._checkpoints[this._currentCheckpointIndex] = ValueTuple.Create<VolumeBox, bool>(this._checkpoints[this._currentCheckpointIndex].Item1, true);
						int num2 = 0;
						for (int j = 0; j < this._checkpoints.Count; j++)
						{
							if (this._checkpoints[j].Item2)
							{
								num2++;
							}
						}
						if (this._currentCheckpointIndex == this._checkpoints.Count - 1)
						{
							flag = true;
						}
						if (this._currentCheckpointIndex == this._checkpoints.Count - 2)
						{
							this.SetFinishGateStatus(true);
						}
					}
					else if (num < 0f)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=kvTEeUWO}Wrong way!", null), 0, null, null, "");
					}
				}
			}
			this._currentCheckpointIndex = -1;
			return flag;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x000187AD File Offset: 0x000169AD
		private static void SetHorseMountable(bool mountable)
		{
			if (mountable)
			{
				Agent.Main.SetAgentFlags(Agent.Main.GetAgentFlags() | AgentFlag.CanRide);
				return;
			}
			Agent.Main.SetAgentFlags(Agent.Main.GetAgentFlags() & ~AgentFlag.CanRide);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x000187E7 File Offset: 0x000169E7
		private void OnMountedTrainingStart()
		{
			this.ResetCheckpoints();
			this._continueLoop = false;
			this.HideAllMountedAITargets();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000187FC File Offset: 0x000169FC
		private void OnMountedTrainingExit()
		{
			TrainingFieldMissionController.SetHorseMountable(false);
			this.ResetCheckpoints();
			this._continueLoop = true;
			this.GoToStartingPosition();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00018818 File Offset: 0x00016A18
		private void SetFinishGateStatus(bool open)
		{
			if (open)
			{
				this._finishGateStatus++;
				if (this._finishGateStatus == 1)
				{
					this._finishGateClosed.SetVisibilityExcludeParents(false);
					this._finishGateOpen.SetVisibilityExcludeParents(true);
					return;
				}
			}
			else
			{
				this._finishGateStatus = MathF.Max(0, this._finishGateStatus - 1);
				if (this._finishGateStatus == 0)
				{
					this._finishGateClosed.SetVisibilityExcludeParents(true);
					this._finishGateOpen.SetVisibilityExcludeParents(false);
				}
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001888C File Offset: 0x00016A8C
		private void MountedTrainingEndedSuccessfully()
		{
			this.UIEndTimer();
			this.EndTimer();
			int brokenBreakableCount = this._activeTutorialArea.GetBrokenBreakableCount(this._trainingSubTypeIndex);
			int breakablesCount = this._activeTutorialArea.GetBreakablesCount(this._trainingSubTypeIndex);
			float num = this._timeScore + (float)(this._activeTutorialArea.GetBreakablesCount(this._trainingSubTypeIndex) - this._activeTutorialArea.GetBrokenBreakableCount(this._trainingSubTypeIndex));
			TextObject textObject = new TextObject("{=W49eUmpT}You can dismount from horse with {CROUCH_KEY}, or {ACTION_KEY} while looking at the horse.", null);
			textObject.SetTextVariable("CROUCH_KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 15), 1f));
			textObject.SetTextVariable("ACTION_KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			this.CurrentObjectiveTick(textObject);
			if (breakablesCount - brokenBreakableCount == 0)
			{
				Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/riding/course_perfect"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
				TextObject textObject2 = new TextObject("{=veHe94Ec}You've successfully finished the track in ({TIME_SCORE}) seconds without missing any targets!", null);
				textObject2.SetTextVariable("TIME_SCORE", new TextObject(num.ToString("0.0"), null));
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
			}
			else
			{
				Mission.Current.MakeSound(SoundEvent.GetEventIdFromString("event:/mission/tutorial/vo/riding/course_finish"), Agent.Main.GetEyeGlobalPosition(), true, false, -1, -1);
				TextObject textObject3 = new TextObject("{=QLgkR3qN}You've successfully finished the track in ({TIME_SCORE}) seconds. You've received ({PENALTY_SECONDS}) seconds penalty from ({MISSED_TARGETS}) missed targets.", null);
				textObject3.SetTextVariable("TIME_SCORE", new TextObject(num.ToString("0.0"), null));
				textObject3.SetTextVariable("PENALTY_SECONDS", new TextObject((num - this._timeScore).ToString("0.0"), null));
				textObject3.SetTextVariable("MISSED_TARGETS", breakablesCount - brokenBreakableCount);
				MBInformationManager.AddQuickInformation(textObject3, 0, null, null, "");
			}
			this.SetFinishGateStatus(false);
			this.SuccessfullyFinishTraining(num);
		}

		// Token: 0x0400011A RID: 282
		private const string SoundBasicMeleeBlockLeft = "event:/mission/tutorial/vo/parrying/block_left";

		// Token: 0x0400011B RID: 283
		private const string SoundBasicMeleeBlockRight = "event:/mission/tutorial/vo/parrying/block_right";

		// Token: 0x0400011C RID: 284
		private const string SoundBasicMeleeBlockUp = "event:/mission/tutorial/vo/parrying/block_up";

		// Token: 0x0400011D RID: 285
		private const string SoundBasicMeleeBlockDown = "event:/mission/tutorial/vo/parrying/block_down";

		// Token: 0x0400011E RID: 286
		private const string SoundBasicMeleeAttackLeft = "event:/mission/tutorial/vo/parrying/attack_left";

		// Token: 0x0400011F RID: 287
		private const string SoundBasicMeleeAttackRight = "event:/mission/tutorial/vo/parrying/attack_right";

		// Token: 0x04000120 RID: 288
		private const string SoundBasicMeleeAttackUp = "event:/mission/tutorial/vo/parrying/attack_up";

		// Token: 0x04000121 RID: 289
		private const string SoundBasicMeleeAttackDown = "event:/mission/tutorial/vo/parrying/attack_down";

		// Token: 0x04000122 RID: 290
		private const string SoundBasicMeleeRemark = "event:/mission/tutorial/vo/parrying/remark";

		// Token: 0x04000123 RID: 291
		private const string SoundBasicMeleePraise = "event:/mission/tutorial/vo/parrying/praise";

		// Token: 0x04000124 RID: 292
		private const string SoundAdvancedMeleeGreet = "event:/mission/tutorial/vo/fighting/greet";

		// Token: 0x04000125 RID: 293
		private const string SoundAdvancedMeleeWarning = "event:/mission/tutorial/vo/fighting/warning";

		// Token: 0x04000126 RID: 294
		private const string SoundAdvancedMeleePlayerLose = "event:/mission/tutorial/vo/fighting/player_lose";

		// Token: 0x04000127 RID: 295
		private const string SoundAdvancedMeleePlayerWin = "event:/mission/tutorial/vo/fighting/player_win";

		// Token: 0x04000128 RID: 296
		private const string SoundRangedPickPrefix = "event:/mission/tutorial/vo/archery/pick_";

		// Token: 0x04000129 RID: 297
		private const string SoundRangedStartTraining = "event:/mission/tutorial/vo/archery/start_training";

		// Token: 0x0400012A RID: 298
		private const string SoundRangedHitTarget = "event:/mission/tutorial/vo/archery/hit_target";

		// Token: 0x0400012B RID: 299
		private const string SoundRangedFinish = "event:/mission/tutorial/vo/archery/finish";

		// Token: 0x0400012C RID: 300
		private const string SoundMountedPickPrefix = "event:/mission/tutorial/vo/riding/pick_";

		// Token: 0x0400012D RID: 301
		private const string SoundMountedStartCourse = "event:/mission/tutorial/vo/riding/start_course";

		// Token: 0x0400012E RID: 302
		private const string SoundMountedCourseFinish = "event:/mission/tutorial/vo/riding/course_finish";

		// Token: 0x0400012F RID: 303
		private const string SoundMountedCoursePerfect = "event:/mission/tutorial/vo/riding/course_perfect";

		// Token: 0x04000130 RID: 304
		private const string FinishCourseSound = "event:/mission/tutorial/finish_course";

		// Token: 0x04000131 RID: 305
		private const string FinishTaskSound = "event:/mission/tutorial/finish_task";

		// Token: 0x04000132 RID: 306
		private const string HitTargetSound = "event:/mission/tutorial/hit_target";

		// Token: 0x04000133 RID: 307
		private const string RangedNpcCharacter = "tutorial_npc_ranged";

		// Token: 0x04000134 RID: 308
		private const string BowTrainingShootingPositionTag = "bow_training_shooting_position";

		// Token: 0x04000135 RID: 309
		private const string SpawnerRangedNpcTag = "spawner_ranged_npc_tag";

		// Token: 0x04000136 RID: 310
		private const string RangedNpcTargetTag = "_ranged_npc_target";

		// Token: 0x04000137 RID: 311
		private const float ShootingPositionActivationDistance = 2f;

		// Token: 0x04000138 RID: 312
		private const string BasicMeleeNpcSpawnPointTag = "spawner_melee_npc";

		// Token: 0x04000139 RID: 313
		private const string BasicMeleeNpcCharacter = "tutorial_npc_basic_melee";

		// Token: 0x0400013A RID: 314
		private const string AdvancedMeleeNpcSpawnPointTagEasy = "spawner_adv_melee_npc_easy";

		// Token: 0x0400013B RID: 315
		private const string AdvancedMeleeNpcSpawnPointTagNormal = "spawner_adv_melee_npc_normal";

		// Token: 0x0400013C RID: 316
		private const string AdvancedMeleeNpcEasySecondPositionTag = "adv_melee_npc_easy_second_pos";

		// Token: 0x0400013D RID: 317
		private const string AdvancedMeleeNpcNormalSecondPositionTag = "adv_melee_npc_normal_second_pos";

		// Token: 0x0400013E RID: 318
		private const string AdvancedMeleeEasyNpcCharacter = "tutorial_npc_advanced_melee_easy";

		// Token: 0x0400013F RID: 319
		private const string AdvancedMeleeNormalNpcCharacter = "tutorial_npc_advanced_melee_normal";

		// Token: 0x04000140 RID: 320
		private const string AdvancedMeleeBattleAreaTag = "battle_area";

		// Token: 0x04000141 RID: 321
		private const string MountedAISpawnPositionTag = "_mounted_ai_spawn_position";

		// Token: 0x04000142 RID: 322
		private const string MountedAICharacter = "tutorial_npc_mounted_ai";

		// Token: 0x04000143 RID: 323
		private const string MountedAITargetTag = "_mounted_ai_target";

		// Token: 0x04000144 RID: 324
		private const string MountedAIWaitingPositionTag = "_mounted_ai_waiting_position";

		// Token: 0x04000145 RID: 325
		private const string CheckpointTag = "mounted_checkpoint";

		// Token: 0x04000146 RID: 326
		private const string HorseSpawnPositionTag = "spawner_horse";

		// Token: 0x04000147 RID: 327
		private const string FinishGateClosedTag = "finish_gate_closed";

		// Token: 0x04000148 RID: 328
		private const string FinishGateOpenTag = "finish_gate_open";

		// Token: 0x04000149 RID: 329
		private const string NameOfTheHorse = "old_horse";

		// Token: 0x0400014A RID: 330
		private readonly TextObject _trainingFinishedText = new TextObject("{=cRvSuYC8}Choose another weapon or go to another training area.", null);

		// Token: 0x0400014B RID: 331
		private readonly List<TrainingFieldMissionController.DelayedAction> _delayedActions = new List<TrainingFieldMissionController.DelayedAction>();

		// Token: 0x0400014C RID: 332
		private MissionConversationLogic _missionConversationHandler;

		// Token: 0x0400014D RID: 333
		private readonly List<TutorialArea> _trainingAreas = new List<TutorialArea>();

		// Token: 0x0400014E RID: 334
		private TutorialArea _activeTutorialArea;

		// Token: 0x0400014F RID: 335
		private bool _courseFinished;

		// Token: 0x04000150 RID: 336
		private int _trainingProgress;

		// Token: 0x04000151 RID: 337
		private int _trainingSubTypeIndex = -1;

		// Token: 0x04000152 RID: 338
		private string _activeTrainingSubTypeTag = "";

		// Token: 0x04000153 RID: 339
		private float _beginningTime;

		// Token: 0x04000154 RID: 340
		private float _timeScore;

		// Token: 0x04000155 RID: 341
		private bool _showTutorialObjectivesAnyway;

		// Token: 0x04000156 RID: 342
		private Dictionary<string, float> _tutorialScores;

		// Token: 0x04000157 RID: 343
		private GameEntity _shootingPosition;

		// Token: 0x04000158 RID: 344
		private Agent _bowNpc;

		// Token: 0x04000159 RID: 345
		private WorldPosition _rangedNpcSpawnPosition;

		// Token: 0x0400015A RID: 346
		private WorldPosition _rangedTargetPosition;

		// Token: 0x0400015B RID: 347
		private Vec3 _rangedTargetRotation;

		// Token: 0x0400015C RID: 348
		private GameEntity _rangedNpcSpawnPoint;

		// Token: 0x0400015D RID: 349
		private int _rangedLastBrokenTargetCount;

		// Token: 0x0400015E RID: 350
		private readonly List<DestructableComponent> _targetsForRangedNpc = new List<DestructableComponent>();

		// Token: 0x0400015F RID: 351
		private DestructableComponent _lastTargetGiven;

		// Token: 0x04000160 RID: 352
		private bool _atShootingPosition;

		// Token: 0x04000161 RID: 353
		private bool _targetPositionSet;

		// Token: 0x04000162 RID: 354
		private readonly List<TrainingFieldMissionController.TutorialObjective> _rangedObjectives = new List<TrainingFieldMissionController.TutorialObjective>
		{
			new TrainingFieldMissionController.TutorialObjective("ranged_go_to_shooting_position", false, false, false),
			new TrainingFieldMissionController.TutorialObjective("ranged_shoot_targets", false, false, false)
		};

		// Token: 0x04000163 RID: 355
		private readonly TextObject _remainingTargetText = new TextObject("{=gBbm9beO}Hit all of the targets. {REMAINING_TARGET} {?REMAINING_TARGET>1}targets{?}target{\\?} left.", null);

		// Token: 0x04000164 RID: 356
		private Agent _meleeTrainer;

		// Token: 0x04000165 RID: 357
		private WorldPosition _meleeTrainerDefaultPosition;

		// Token: 0x04000166 RID: 358
		private float _timer;

		// Token: 0x04000167 RID: 359
		private readonly List<TrainingFieldMissionController.TutorialObjective> _meleeObjectives = new List<TrainingFieldMissionController.TutorialObjective>
		{
			new TrainingFieldMissionController.TutorialObjective("melee_go_to_trainer", false, false, false),
			new TrainingFieldMissionController.TutorialObjective("melee_defense", false, false, true),
			new TrainingFieldMissionController.TutorialObjective("melee_attack", false, false, true)
		};

		// Token: 0x04000168 RID: 360
		private Agent _advancedMeleeTrainerEasy;

		// Token: 0x04000169 RID: 361
		private Agent _advancedMeleeTrainerNormal;

		// Token: 0x0400016A RID: 362
		private float _playerCampaignHealth;

		// Token: 0x0400016B RID: 363
		private float _playerHealth = 100f;

		// Token: 0x0400016C RID: 364
		private float _advancedMeleeTrainerEasyHealth = 100f;

		// Token: 0x0400016D RID: 365
		private float _advancedMeleeTrainerNormalHealth = 100f;

		// Token: 0x0400016E RID: 366
		private MatrixFrame _advancedMeleeTrainerEasyInitialPosition;

		// Token: 0x0400016F RID: 367
		private MatrixFrame _advancedMeleeTrainerEasySecondPosition;

		// Token: 0x04000170 RID: 368
		private MatrixFrame _advancedMeleeTrainerNormalInitialPosition;

		// Token: 0x04000171 RID: 369
		private MatrixFrame _advancedMeleeTrainerNormalSecondPosition;

		// Token: 0x04000172 RID: 370
		private readonly TextObject _fightStartsIn = new TextObject("{=TNxWBS07}Fight will start in {REMAINING_TIME} {?REMAINING_TIME>1}seconds{?}second{\\?}...", null);

		// Token: 0x04000173 RID: 371
		private readonly List<TrainingFieldMissionController.TutorialObjective> _advMeleeObjectives = new List<TrainingFieldMissionController.TutorialObjective>
		{
			new TrainingFieldMissionController.TutorialObjective("adv_melee_go_to_trainer", false, false, false),
			new TrainingFieldMissionController.TutorialObjective("adv_melee_beat_easy_trainer", false, false, false),
			new TrainingFieldMissionController.TutorialObjective("adv_melee_beat_normal_trainer", false, false, false)
		};

		// Token: 0x04000174 RID: 372
		private bool _playerLeftBattleArea;

		// Token: 0x04000175 RID: 373
		private GameEntity _finishGateClosed;

		// Token: 0x04000176 RID: 374
		private GameEntity _finishGateOpen;

		// Token: 0x04000177 RID: 375
		private int _finishGateStatus;

		// Token: 0x04000178 RID: 376
		private readonly List<ValueTuple<VolumeBox, bool>> _checkpoints = new List<ValueTuple<VolumeBox, bool>>();

		// Token: 0x04000179 RID: 377
		private int _currentCheckpointIndex = -1;

		// Token: 0x0400017A RID: 378
		private int _mountedLastBrokenTargetCount;

		// Token: 0x0400017B RID: 379
		private float _enteringDotProduct;

		// Token: 0x0400017C RID: 380
		private Agent _horse;

		// Token: 0x0400017D RID: 381
		private WorldPosition _horseBeginningPosition;

		// Token: 0x0400017E RID: 382
		private TrainingFieldMissionController.HorseReturningSituation _horseBehaviorMode = TrainingFieldMissionController.HorseReturningSituation.ReturnCompleted;

		// Token: 0x0400017F RID: 383
		private readonly List<TrainingFieldMissionController.TutorialObjective> _mountedObjectives = new List<TrainingFieldMissionController.TutorialObjective>
		{
			new TrainingFieldMissionController.TutorialObjective("mounted_mount_the_horse", false, false, false),
			new TrainingFieldMissionController.TutorialObjective("mounted_hit_targets", false, false, false)
		};

		// Token: 0x04000180 RID: 384
		private Agent _mountedAI;

		// Token: 0x04000181 RID: 385
		private MatrixFrame _mountedAISpawnPosition;

		// Token: 0x04000182 RID: 386
		private MatrixFrame _mountedAIWaitingPosition;

		// Token: 0x04000183 RID: 387
		private int _mountedAICurrentCheckpointTarget = -1;

		// Token: 0x04000184 RID: 388
		private int _mountedAICurrentHitTarget;

		// Token: 0x04000185 RID: 389
		private bool _enteredRadiusOfTarget;

		// Token: 0x04000186 RID: 390
		private bool _allTargetsDestroyed;

		// Token: 0x04000187 RID: 391
		private readonly List<DestructableComponent> _mountedAITargets = new List<DestructableComponent>();

		// Token: 0x04000188 RID: 392
		private bool _continueLoop = true;

		// Token: 0x04000189 RID: 393
		private List<TrainingFieldMissionController.TutorialObjective> _detailedObjectives = new List<TrainingFieldMissionController.TutorialObjective>();

		// Token: 0x0400018A RID: 394
		private readonly List<TrainingFieldMissionController.TutorialObjective> _tutorialObjectives = new List<TrainingFieldMissionController.TutorialObjective>();

		// Token: 0x0400018B RID: 395
		public Action UIStartTimer;

		// Token: 0x0400018C RID: 396
		public Func<float> UIEndTimer;

		// Token: 0x0400018D RID: 397
		public Action<string> TimerTick;

		// Token: 0x0400018E RID: 398
		public Action<TextObject> CurrentObjectiveTick;

		// Token: 0x0400018F RID: 399
		public Action<TrainingFieldMissionController.MouseObjectives, TrainingFieldMissionController.ObjectivePerformingType> CurrentMouseObjectiveTick;

		// Token: 0x04000190 RID: 400
		public Action<List<TrainingFieldMissionController.TutorialObjective>> AllObjectivesTick;

		// Token: 0x04000191 RID: 401
		private static bool _updateObjectivesWillBeCalled;

		// Token: 0x04000193 RID: 403
		private Agent _brotherConversationAgent;

		// Token: 0x02000084 RID: 132
		public class TutorialObjective
		{
			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000694 RID: 1684 RVA: 0x00023E0B File Offset: 0x0002200B
			public string Id { get; }

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x06000695 RID: 1685 RVA: 0x00023E13 File Offset: 0x00022013
			// (set) Token: 0x06000696 RID: 1686 RVA: 0x00023E1B File Offset: 0x0002201B
			public bool IsFinished { get; private set; }

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x06000697 RID: 1687 RVA: 0x00023E24 File Offset: 0x00022024
			public bool HasBackground { get; }

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x06000698 RID: 1688 RVA: 0x00023E2C File Offset: 0x0002202C
			// (set) Token: 0x06000699 RID: 1689 RVA: 0x00023E34 File Offset: 0x00022034
			public bool IsActive { get; private set; }

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x0600069A RID: 1690 RVA: 0x00023E3D File Offset: 0x0002203D
			public List<TrainingFieldMissionController.TutorialObjective> SubTasks { get; }

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x0600069B RID: 1691 RVA: 0x00023E45 File Offset: 0x00022045
			// (set) Token: 0x0600069C RID: 1692 RVA: 0x00023E4D File Offset: 0x0002204D
			public float Score { get; private set; }

			// Token: 0x0600069D RID: 1693 RVA: 0x00023E58 File Offset: 0x00022058
			public TutorialObjective(string id, bool isFinished = false, bool isActive = false, bool hasBackground = false)
			{
				this._name = GameTexts.FindText("str_tutorial_" + id, null);
				this.Id = id;
				this.IsFinished = isFinished;
				this.IsActive = isActive;
				this.SubTasks = new List<TrainingFieldMissionController.TutorialObjective>();
				this.Score = 0f;
				this.HasBackground = hasBackground;
			}

			// Token: 0x0600069E RID: 1694 RVA: 0x00023EB5 File Offset: 0x000220B5
			public void SetTextVariableOfName(string tag, int variable)
			{
				string text = this._name.ToString();
				this._name.SetTextVariable(tag, variable);
				if (text != this._name.ToString())
				{
					TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
				}
			}

			// Token: 0x0600069F RID: 1695 RVA: 0x00023EE8 File Offset: 0x000220E8
			public string GetNameString()
			{
				if (!(this._name == null))
				{
					return this._name.ToString();
				}
				return "";
			}

			// Token: 0x060006A0 RID: 1696 RVA: 0x00023F09 File Offset: 0x00022109
			public bool SetActive(bool isActive)
			{
				if (this.IsActive == isActive)
				{
					return false;
				}
				this.IsActive = isActive;
				TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
				return true;
			}

			// Token: 0x060006A1 RID: 1697 RVA: 0x00023F24 File Offset: 0x00022124
			public bool FinishTask()
			{
				if (this.IsFinished)
				{
					return false;
				}
				this.IsFinished = true;
				TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
				return true;
			}

			// Token: 0x060006A2 RID: 1698 RVA: 0x00023F40 File Offset: 0x00022140
			public void FinishSubTask(string subTaskName, float score)
			{
				TrainingFieldMissionController.TutorialObjective tutorialObjective = this.SubTasks.Find((TrainingFieldMissionController.TutorialObjective x) => x.Id == subTaskName);
				tutorialObjective.FinishTask();
				if (score != 0f && (tutorialObjective.Score > score || tutorialObjective.Score == 0f))
				{
					tutorialObjective.Score = score;
				}
				if (!this.SubTasks.Exists((TrainingFieldMissionController.TutorialObjective x) => !x.IsFinished))
				{
					this.FinishTask();
				}
				TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
			}

			// Token: 0x060006A3 RID: 1699 RVA: 0x00023FD8 File Offset: 0x000221D8
			public bool SetAllSubTasksInactive()
			{
				bool flag = false;
				foreach (TrainingFieldMissionController.TutorialObjective tutorialObjective in this.SubTasks)
				{
					bool flag2 = tutorialObjective.SetActive(false);
					flag = flag || flag2;
					if (tutorialObjective.SubTasks.Count > 0)
					{
						bool flag3 = tutorialObjective.SetAllSubTasksInactive();
						flag = flag || flag3;
					}
				}
				if (flag)
				{
					TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
				}
				return flag;
			}

			// Token: 0x060006A4 RID: 1700 RVA: 0x00024058 File Offset: 0x00022258
			public void AddSubTask(TrainingFieldMissionController.TutorialObjective newSubTask)
			{
				this.SubTasks.Add(newSubTask);
				TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x0002406C File Offset: 0x0002226C
			public void RestoreScoreFromSave(float score)
			{
				this.Score = score;
				TrainingFieldMissionController._updateObjectivesWillBeCalled = true;
			}

			// Token: 0x0400026F RID: 623
			private readonly TextObject _name;
		}

		// Token: 0x02000085 RID: 133
		public readonly struct DelayedAction
		{
			// Token: 0x060006A6 RID: 1702 RVA: 0x0002407B File Offset: 0x0002227B
			public DelayedAction(Action order, float delayTime)
			{
				this._orderGivenTime = Mission.Current.CurrentTime;
				this._delayTime = delayTime;
				this._order = order;
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x0002409B File Offset: 0x0002229B
			public bool Update()
			{
				if (Mission.Current.CurrentTime - this._orderGivenTime > this._delayTime)
				{
					this._order();
					return true;
				}
				return false;
			}

			// Token: 0x04000276 RID: 630
			private readonly float _orderGivenTime;

			// Token: 0x04000277 RID: 631
			private readonly float _delayTime;

			// Token: 0x04000278 RID: 632
			private readonly Action _order;
		}

		// Token: 0x02000086 RID: 134
		public enum MouseObjectives
		{
			// Token: 0x0400027A RID: 634
			None,
			// Token: 0x0400027B RID: 635
			AttackLeft,
			// Token: 0x0400027C RID: 636
			AttackRight,
			// Token: 0x0400027D RID: 637
			AttackUp,
			// Token: 0x0400027E RID: 638
			AttackDown,
			// Token: 0x0400027F RID: 639
			DefendLeft,
			// Token: 0x04000280 RID: 640
			DefendRight,
			// Token: 0x04000281 RID: 641
			DefendUp,
			// Token: 0x04000282 RID: 642
			DefendDown
		}

		// Token: 0x02000087 RID: 135
		public enum ObjectivePerformingType
		{
			// Token: 0x04000284 RID: 644
			None,
			// Token: 0x04000285 RID: 645
			ByLookDirection,
			// Token: 0x04000286 RID: 646
			ByMovement,
			// Token: 0x04000287 RID: 647
			AutoBlock
		}

		// Token: 0x02000088 RID: 136
		private enum HorseReturningSituation
		{
			// Token: 0x04000289 RID: 649
			NotInPosition,
			// Token: 0x0400028A RID: 650
			BeginReturn,
			// Token: 0x0400028B RID: 651
			Returning,
			// Token: 0x0400028C RID: 652
			ReturnCompleted,
			// Token: 0x0400028D RID: 653
			Following
		}
	}
}
