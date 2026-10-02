using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using SandBox.Objects;
using SandBox.Objects.AnimationPoints;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.Source.Objects;
using TaleWorlds.ObjectSystem;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000073 RID: 115
	public class MissionAgentHandler : MissionLogic
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x0001B528 File Offset: 0x00019728
		public bool HasPassages()
		{
			List<UsableMachine> list;
			return this._usablePoints.TryGetValue("npc_passage", out list) && list.Count > 0;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001B554 File Offset: 0x00019754
		public List<UsableMachine> TownPassageProps
		{
			get
			{
				List<UsableMachine> list;
				this._usablePoints.TryGetValue("npc_passage", out list);
				return list;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001B575 File Offset: 0x00019775
		public List<UsableMachine> DisabledPassages
		{
			get
			{
				return this._disabledPassages;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001B580 File Offset: 0x00019780
		public List<UsableMachine> UsablePoints
		{
			get
			{
				List<UsableMachine> list = new List<UsableMachine>();
				foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
				{
					list.AddRange(keyValuePair.Value);
				}
				foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair2 in this._pairedUsablePoints)
				{
					list.AddRange(keyValuePair2.Value);
				}
				return list;
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001B628 File Offset: 0x00019828
		public MissionAgentHandler()
		{
			this._usablePoints = new Dictionary<string, List<UsableMachine>>();
			this._pairedUsablePoints = new Dictionary<string, List<UsableMachine>>();
			this._usedSpawnPoints = new HashSet<UsableMachine>();
			this._disabledPassages = new List<UsableMachine>();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001B668 File Offset: 0x00019868
		public override void EarlyStart()
		{
			this._passageUsageTime = base.Mission.CurrentTime + 30f;
			this.GetAllProps();
			MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(Settlement.CurrentSettlement.Position.ToVec2());
			if (weatherEventInPosition != MapWeatherModel.WeatherEvent.HeavyRain && weatherEventInPosition != MapWeatherModel.WeatherEvent.Blizzard)
			{
				this.InitializePairedUsableObjects();
			}
			base.Mission.SetReportStuckAgentsMode(true);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001B6D3 File Offset: 0x000198D3
		public override void OnRenderingStarted()
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001B6D8 File Offset: 0x000198D8
		public override void OnMissionTick(float dt)
		{
			float currentTime = base.Mission.CurrentTime;
			if (currentTime > this._passageUsageTime)
			{
				this._passageUsageTime = currentTime + 30f;
				if (PlayerEncounter.LocationEncounter != null && LocationComplex.Current != null)
				{
					LocationComplex.Current.AgentPassageUsageTick();
				}
			}
			for (int i = this._spawnTimers.Count - 1; i >= 0; i--)
			{
				if (this._spawnTimers[i].Item6.Check(currentTime))
				{
					this.SpawnWanderingAgentWithInitialFrame(this._spawnTimers[i].Item1, this._spawnTimers[i].Item2, this._spawnTimers[i].Item3.WeakEntity, this._spawnTimers[i].Item4, this._spawnTimers[i].Item5);
					this._spawnTimers.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001B7C3 File Offset: 0x000199C3
		protected override void OnEndMission()
		{
			this._usablePoints.Clear();
			this._pairedUsablePoints.Clear();
			this._disabledPassages.Clear();
			this._usedSpawnPoints.Clear();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001B7F4 File Offset: 0x000199F4
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			if (!atStart && (base.Mission.Mode == MissionMode.Battle || oldMissionMode == MissionMode.Battle))
			{
				foreach (Agent agent in base.Mission.Agents)
				{
					if (agent.IsHuman && !agent.IsPlayerControlled)
					{
						agent.SetAgentExcludeStateForFaceGroupId(MissionAgentHandler._disabledFaceId, agent.CurrentWatchState != Agent.WatchState.Alarmed);
					}
				}
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001B884 File Offset: 0x00019A84
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				CampaignAgentComponent component = agent.GetComponent<CampaignAgentComponent>();
				if (component != null)
				{
					component.OnAgentRemoved(affectedAgent);
				}
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001B8E8 File Offset: 0x00019AE8
		private void InitializePairedUsableObjects()
		{
			Dictionary<string, List<UsableMachine>> dictionary = new Dictionary<string, List<UsableMachine>>();
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
			{
				foreach (UsableMachine usableMachine in keyValuePair.Value)
				{
					using (List<StandingPoint>.Enumerator enumerator3 = usableMachine.StandingPoints.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							AnimationPoint animationPoint;
							if ((animationPoint = enumerator3.Current as AnimationPoint) != null && animationPoint.PairEntity != null)
							{
								if (this._pairedUsablePoints.ContainsKey(keyValuePair.Key))
								{
									if (!this._pairedUsablePoints[keyValuePair.Key].Contains(usableMachine))
									{
										this._pairedUsablePoints[keyValuePair.Key].Add(usableMachine);
									}
								}
								else
								{
									this._pairedUsablePoints.Add(keyValuePair.Key, new List<UsableMachine> { usableMachine });
								}
								if (dictionary.ContainsKey(keyValuePair.Key))
								{
									dictionary[keyValuePair.Key].Add(usableMachine);
								}
								else
								{
									dictionary.Add(keyValuePair.Key, new List<UsableMachine> { usableMachine });
								}
							}
						}
					}
				}
			}
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair2 in dictionary)
			{
				foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair3 in this._usablePoints)
				{
					foreach (UsableMachine usableMachine2 in dictionary[keyValuePair2.Key])
					{
						keyValuePair3.Value.Remove(usableMachine2);
					}
				}
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001BBA0 File Offset: 0x00019DA0
		private void GetAllProps()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("navigation_mesh_deactivator");
			if (gameEntity != null)
			{
				NavigationMeshDeactivator firstScriptOfType = gameEntity.GetFirstScriptOfType<NavigationMeshDeactivator>();
				MissionAgentHandler._disabledFaceId = firstScriptOfType.DisableFaceWithId;
				MissionAgentHandler._disabledFaceIdForAnimals = firstScriptOfType.DisableFaceWithIdForAnimals;
			}
			this._usablePoints.Clear();
			foreach (UsableMachine usableMachine in base.Mission.MissionObjects.FindAllWithType<UsableMachine>())
			{
				foreach (string text in usableMachine.GameEntity.Tags)
				{
					if (!this._usablePoints.ContainsKey(text))
					{
						this._usablePoints.Add(text, new List<UsableMachine>());
					}
					if (text != "sp_guard" || !usableMachine.GameEntity.HasTag("sp_guard_with_spear"))
					{
						this._usablePoints[text].Add(usableMachine);
					}
				}
			}
			if (Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsTown || Settlement.CurrentSettlement.IsVillage))
			{
				foreach (AreaMarker areaMarker in base.Mission.ActiveMissionObjects.FindAllWithType<AreaMarker>().ToList<AreaMarker>())
				{
					string tag = areaMarker.Tag;
					List<UsableMachine> usableMachinesInRange = areaMarker.GetUsableMachinesInRange(areaMarker.Tag.Contains("workshop") ? "unaffected_by_area" : null);
					if (!this._usablePoints.ContainsKey(tag))
					{
						this._usablePoints.Add(tag, new List<UsableMachine>());
					}
					foreach (UsableMachine usableMachine2 in usableMachinesInRange)
					{
						foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
						{
							if (keyValuePair.Value.Contains(usableMachine2))
							{
								keyValuePair.Value.Remove(usableMachine2);
							}
						}
						if (usableMachine2.GameEntity.HasTag("hold_tag_always"))
						{
							string text2 = usableMachine2.GameEntity.Tags[0] + "_" + areaMarker.Tag;
							usableMachine2.GameEntity.AddTag(text2);
							if (!this._usablePoints.ContainsKey(text2))
							{
								this._usablePoints.Add(text2, new List<UsableMachine>());
								this._usablePoints[text2].Add(usableMachine2);
							}
							else
							{
								this._usablePoints[text2].Add(usableMachine2);
							}
						}
						else
						{
							foreach (UsableMachine usableMachine3 in usableMachinesInRange)
							{
								if (!usableMachine3.GameEntity.HasTag(tag))
								{
									usableMachine3.GameEntity.AddTag(tag);
								}
							}
						}
					}
					if (this._usablePoints.ContainsKey(tag))
					{
						usableMachinesInRange.RemoveAll((UsableMachine x) => this._usablePoints[tag].Contains(x));
						if (usableMachinesInRange.Count > 0)
						{
							this._usablePoints[tag].AddRange(usableMachinesInRange);
						}
					}
					foreach (UsableMachine usableMachine4 in areaMarker.GetUsableMachinesWithTagInRange("unaffected_by_area"))
					{
						string text3 = usableMachine4.GameEntity.Tags[0];
						foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair2 in this._usablePoints)
						{
							if (keyValuePair2.Value.Contains(usableMachine4))
							{
								keyValuePair2.Value.Remove(usableMachine4);
							}
						}
						if (this._usablePoints.ContainsKey(text3))
						{
							this._usablePoints[text3].Add(usableMachine4);
						}
						else
						{
							this._usablePoints.Add(text3, new List<UsableMachine>());
							this._usablePoints[text3].Add(usableMachine4);
						}
					}
				}
			}
			List<GameEntity> list = new List<GameEntity>();
			base.Mission.Scene.GetAllEntitiesWithScriptComponent<DynamicPatrolAreaParent>(ref list);
			foreach (GameEntity gameEntity2 in list)
			{
				foreach (GameEntity gameEntity3 in gameEntity2.GetChildren())
				{
					PatrolPoint firstScriptOfType2 = gameEntity3.GetChild(0).GetFirstScriptOfType<PatrolPoint>();
					if (firstScriptOfType2 != null && !firstScriptOfType2.IsDisabled && !string.IsNullOrEmpty(firstScriptOfType2.SpawnGroupTag))
					{
						if (this._usablePoints.ContainsKey(firstScriptOfType2.SpawnGroupTag))
						{
							this._usablePoints[firstScriptOfType2.SpawnGroupTag].Add(firstScriptOfType2.GameEntity.Parent.GetFirstScriptOfType<UsablePlace>());
						}
						else
						{
							this._usablePoints.Add(firstScriptOfType2.SpawnGroupTag, new List<UsableMachine>());
							this._usablePoints[firstScriptOfType2.SpawnGroupTag].Add(firstScriptOfType2.GameEntity.Parent.GetFirstScriptOfType<UsablePlace>());
						}
					}
				}
			}
			this.DisableUnavailableWaypoints();
			this.RemoveDeactivatedUsablePlacesFromList();
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001C25C File Offset: 0x0001A45C
		[Conditional("DEBUG")]
		public void DetectMissingEntities()
		{
			if (CampaignMission.Current.Location != null && !Utilities.CommandLineArgumentExists("CampaignGameplayTest"))
			{
				IEnumerable<LocationCharacter> characterList = CampaignMission.Current.Location.GetCharacterList();
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (LocationCharacter locationCharacter in characterList)
				{
					if (locationCharacter.SpecialTargetTag != null)
					{
						if (dictionary.ContainsKey(locationCharacter.SpecialTargetTag))
						{
							Dictionary<string, int> dictionary2 = dictionary;
							string specialTargetTag = locationCharacter.SpecialTargetTag;
							int num = dictionary2[specialTargetTag];
							dictionary2[specialTargetTag] = num + 1;
						}
						else
						{
							dictionary.Add(locationCharacter.SpecialTargetTag, 1);
						}
					}
				}
				foreach (KeyValuePair<string, int> keyValuePair in dictionary)
				{
					string key = keyValuePair.Key;
					int value = keyValuePair.Value;
					int num2 = 0;
					List<UsableMachine> list;
					if (this._usablePoints.TryGetValue(key, out list))
					{
						num2 += list.Count;
						foreach (UsableMachine usableMachine in list)
						{
							num2 += MissionAgentHandler.GetPointCountOfUsableMachine(usableMachine, false);
						}
					}
					List<UsableMachine> list2;
					if (this._pairedUsablePoints.TryGetValue(key, out list2))
					{
						num2 += list2.Count;
						foreach (UsableMachine usableMachine2 in list2)
						{
							num2 += MissionAgentHandler.GetPointCountOfUsableMachine(usableMachine2, false);
						}
					}
					if (num2 < value)
					{
						string.Concat(new object[]
						{
							"Trying to spawn ",
							value,
							" npc with \"",
							key,
							"\" but there are ",
							num2,
							" suitable spawn points in scene ",
							base.Mission.SceneName
						});
						if (TestCommonBase.BaseInstance != null)
						{
							bool isTestEnabled = TestCommonBase.BaseInstance.IsTestEnabled;
						}
					}
				}
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
		private void RemoveDeactivatedUsablePlacesFromList()
		{
			Dictionary<string, List<UsableMachine>> dictionary = new Dictionary<string, List<UsableMachine>>();
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
			{
				foreach (UsableMachine usableMachine in keyValuePair.Value)
				{
					if (usableMachine.IsDeactivated)
					{
						if (dictionary.ContainsKey(keyValuePair.Key))
						{
							dictionary[keyValuePair.Key].Add(usableMachine);
						}
						else
						{
							dictionary.Add(keyValuePair.Key, new List<UsableMachine>());
							dictionary[keyValuePair.Key].Add(usableMachine);
						}
					}
				}
			}
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair2 in dictionary)
			{
				foreach (UsableMachine usableMachine2 in keyValuePair2.Value)
				{
					this._usablePoints[keyValuePair2.Key].Remove(usableMachine2);
				}
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001C63C File Offset: 0x0001A83C
		public Dictionary<string, int> FindUnusedUsablePointCount()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
			{
				int num = 0;
				foreach (UsableMachine usableMachine in keyValuePair.Value)
				{
					if (!this._usedSpawnPoints.Contains(usableMachine))
					{
						num += MissionAgentHandler.GetPointCountOfUsableMachine(usableMachine, true);
					}
				}
				if (num > 0)
				{
					dictionary.Add(keyValuePair.Key, num);
				}
			}
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair2 in this._pairedUsablePoints)
			{
				int num2 = 0;
				foreach (UsableMachine usableMachine2 in keyValuePair2.Value)
				{
					if (!this._usedSpawnPoints.Contains(usableMachine2))
					{
						num2 += MissionAgentHandler.GetPointCountOfUsableMachine(usableMachine2, true);
					}
				}
				if (num2 > 0)
				{
					if (!dictionary.ContainsKey(keyValuePair2.Key))
					{
						dictionary.Add(keyValuePair2.Key, num2);
					}
					else
					{
						Dictionary<string, int> dictionary2 = dictionary;
						string key = keyValuePair2.Key;
						dictionary2[key] += num2;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001C7E0 File Offset: 0x0001A9E0
		private void DisableUnavailableWaypoints()
		{
			bool isNight = Campaign.Current.IsNight;
			string text = "";
			int num = 0;
			foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int i = 0;
				while (i < keyValuePair.Value.Count)
				{
					UsableMachine usableMachine = keyValuePair.Value[i];
					if (!Mission.Current.IsPositionInsideBoundaries(usableMachine.GameEntity.GlobalPosition.AsVec2))
					{
						foreach (StandingPoint standingPoint in usableMachine.StandingPoints)
						{
							standingPoint.IsDeactivated = true;
							num++;
						}
					}
					if (usableMachine is Chair)
					{
						using (List<StandingPoint>.Enumerator enumerator2 = usableMachine.StandingPoints.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								StandingPoint standingPoint2 = enumerator2.Current;
								Vec3 origin = standingPoint2.GameEntity.GetGlobalFrame().origin;
								PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
								base.Mission.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, origin, true);
								if (!nullFaceRecord.IsValid() || (MissionAgentHandler._disabledFaceId != -1 && nullFaceRecord.FaceGroupIndex == MissionAgentHandler._disabledFaceId))
								{
									standingPoint2.IsDeactivated = true;
									num2++;
								}
							}
							goto IL_02C6;
						}
						goto IL_014E;
					}
					goto IL_014E;
					IL_02C6:
					i++;
					continue;
					IL_014E:
					if (usableMachine is Passage)
					{
						Passage passage = usableMachine as Passage;
						if (passage.ToLocation == null || !passage.ToLocation.CanPlayerSee())
						{
							foreach (StandingPoint standingPoint3 in passage.StandingPoints)
							{
								standingPoint3.IsDeactivated = true;
							}
							passage.Disable();
							this._disabledPassages.Add(usableMachine);
							Location toLocation = passage.ToLocation;
							keyValuePair.Value.RemoveAt(i);
							i--;
							num3++;
							goto IL_02C6;
						}
						goto IL_02C6;
					}
					else
					{
						if (usableMachine is UsablePlace)
						{
							foreach (StandingPoint standingPoint4 in usableMachine.StandingPoints)
							{
								Vec3 origin2 = standingPoint4.GameEntity.GetGlobalFrame().origin;
								PathFaceRecord nullFaceRecord2 = PathFaceRecord.NullFaceRecord;
								base.Mission.Scene.GetNavMeshFaceIndex(ref nullFaceRecord2, origin2, true);
								if (!nullFaceRecord2.IsValid() || (MissionAgentHandler._disabledFaceId != -1 && nullFaceRecord2.FaceGroupIndex == MissionAgentHandler._disabledFaceId) || (isNight && usableMachine.GameEntity.HasTag("disable_at_night")) || (!isNight && usableMachine.GameEntity.HasTag("enable_at_night")))
								{
									standingPoint4.IsDeactivated = true;
									num4++;
								}
							}
							goto IL_02C6;
						}
						goto IL_02C6;
					}
				}
				if (num4 + num2 + num3 > 0)
				{
					text = text + "_____________________________________________\n\"" + keyValuePair.Key + "\" :\n";
					if (num4 > 0)
					{
						text = string.Concat(new object[] { text, "Disabled standing point : ", num4, "\n" });
					}
					if (num2 > 0)
					{
						text = string.Concat(new object[] { text, "Disabled chair use point : ", num2, "\n" });
					}
					if (num3 > 0)
					{
						text = string.Concat(new object[] { text, "Disabled passage info : ", num3, "\n" });
					}
				}
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001CC1C File Offset: 0x0001AE1C
		public void SpawnLocationCharacters(string overridenTagValue = null)
		{
			CampaignEventDispatcher.Instance.LocationCharactersAreReadyToSpawn(this.FindUnusedUsablePointCount());
			foreach (LocationCharacter locationCharacter in CampaignMission.Current.Location.GetCharacterList())
			{
				if (!this.IsAlreadySpawned(locationCharacter.AgentOrigin))
				{
					if (!string.IsNullOrEmpty(overridenTagValue))
					{
						locationCharacter.SpecialTargetTag = overridenTagValue;
					}
					Agent agent = this.SpawnDefaultLocationCharacter(locationCharacter, false);
					if (agent != null)
					{
						agent.SetAgentExcludeStateForFaceGroupId(MissionAgentHandler._disabledFaceId, true);
					}
				}
			}
			List<Passage> list = new List<Passage>();
			if (this.TownPassageProps != null)
			{
				foreach (UsableMachine usableMachine in this.TownPassageProps)
				{
					Passage passage;
					if ((passage = usableMachine as Passage) != null && !usableMachine.IsDeactivated)
					{
						passage.Deactivate();
						list.Add(passage);
					}
				}
			}
			foreach (Agent agent2 in base.Mission.Agents)
			{
				this.SimulateAgent(agent2);
			}
			foreach (Passage passage2 in list)
			{
				passage2.Activate();
			}
			CampaignEventDispatcher.Instance.LocationCharactersSimulated();
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001CDB0 File Offset: 0x0001AFB0
		private bool IsAlreadySpawned(IAgentOriginBase agentOrigin)
		{
			return Mission.Current != null && Mission.Current.Agents.Any<Agent>((Agent x) => x.Origin == agentOrigin);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001CDF0 File Offset: 0x0001AFF0
		public Agent SpawnDefaultLocationCharacter(LocationCharacter locationCharacter, bool simulateAgentAfterSpawn = false)
		{
			Agent agent = this.SpawnWanderingAgent(locationCharacter);
			if (agent != null)
			{
				if (simulateAgentAfterSpawn)
				{
					this.SimulateAgent(agent);
				}
				if (locationCharacter.IsVisualTracked)
				{
					VisualTrackerMissionBehavior missionBehavior = Mission.Current.GetMissionBehavior<VisualTrackerMissionBehavior>();
					if (missionBehavior != null)
					{
						missionBehavior.RegisterLocalOnlyObject(agent);
					}
				}
			}
			return agent;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001CE34 File Offset: 0x0001B034
		public void SimulateAgent(Agent agent)
		{
			if (agent.IsHuman)
			{
				AgentNavigator agentNavigator = agent.GetComponent<CampaignAgentComponent>().AgentNavigator;
				int num = MBRandom.RandomInt(35, 50);
				agent.PreloadForRendering();
				for (int i = 0; i < num; i++)
				{
					if (agentNavigator != null)
					{
						agentNavigator.Tick(0.1f, true);
					}
					if (agent.IsUsingGameObject)
					{
						agent.CurrentlyUsedGameObject.SimulateTick(0.1f);
					}
				}
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001CE98 File Offset: 0x0001B098
		private void GetFrameForFollowingAgent(Agent followedAgent, out MatrixFrame frame)
		{
			frame = followedAgent.Frame;
			frame.origin += -(frame.rotation.f * 1.5f);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001CED8 File Offset: 0x0001B0D8
		public void FadeoutExitingLocationCharacter(LocationCharacter locationCharacter)
		{
			if (base.Mission.CurrentState != Mission.State.EndingNextFrame && base.Mission.CurrentState != Mission.State.Over)
			{
				foreach (Agent agent in Mission.Current.Agents)
				{
					if ((CharacterObject)agent.Character == locationCharacter.Character)
					{
						agent.FadeOut(false, true);
						break;
					}
				}
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001CF64 File Offset: 0x0001B164
		public void SpawnEnteringLocationCharacter(LocationCharacter locationCharacter, Location fromLocation)
		{
			if (fromLocation != null)
			{
				foreach (UsableMachine usableMachine in this.TownPassageProps)
				{
					Passage passage = usableMachine as Passage;
					if (passage.ToLocation == fromLocation)
					{
						MatrixFrame globalFrame = passage.PilotStandingPoint.GameEntity.GetGlobalFrame();
						globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
						globalFrame.origin.z = base.Mission.Scene.GetGroundHeightAtPosition(globalFrame.origin, BodyFlags.CommonCollisionExcludeFlags);
						Vec3 f = globalFrame.rotation.f;
						f.Normalize();
						globalFrame.origin -= 0.3f * f;
						globalFrame.rotation.RotateAboutUp(3.1415927f);
						bool flag = usableMachine.GameEntity.HasTag("torch");
						Agent agent = this.SpawnWanderingAgentWithInitialFrame(locationCharacter, globalFrame, passage.PilotStandingPoint.GameEntity, true, flag);
						agent.SetAgentExcludeStateForFaceGroupId(MissionAgentHandler._disabledFaceId, true);
						base.Mission.MakeSound(MiscSoundContainer.SoundCodeMovementFoleyDoorClose, globalFrame.origin, true, false, -1, -1);
						agent.FadeIn();
						break;
					}
				}
				return;
			}
			this.SpawnDefaultLocationCharacter(locationCharacter, true);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001D0DC File Offset: 0x0001B2DC
		private void SetUsablePlaceUsed(string spawnTag, GameEntity gameEntity)
		{
			foreach (UsableMachine usableMachine in this.GetAllUsablePointsWithTag(spawnTag))
			{
				if (!this._usedSpawnPoints.Contains(usableMachine) && usableMachine.GameEntity == gameEntity)
				{
					this._usedSpawnPoints.Add(usableMachine);
				}
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001D154 File Offset: 0x0001B354
		private bool GetInitialFrameForSpawnTag(string spawnTag, ref WeakGameEntity spawnedOnGameEntity, ref MatrixFrame frame)
		{
			List<UsableMachine> allUsablePointsWithTag = this.GetAllUsablePointsWithTag(spawnTag);
			if (allUsablePointsWithTag.Count > 0)
			{
				foreach (UsableMachine usableMachine in allUsablePointsWithTag)
				{
					MatrixFrame matrixFrame;
					if (!this._usedSpawnPoints.Contains(usableMachine) && this.GetSpawnFrameFromUsableMachine(usableMachine, out matrixFrame))
					{
						frame = matrixFrame;
						spawnedOnGameEntity = usableMachine.GameEntity;
						this._usedSpawnPoints.Add(usableMachine);
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001D1F0 File Offset: 0x0001B3F0
		public bool HasUsablePointWithTag(string tag)
		{
			return this._usablePoints.ContainsKey(tag) || this._pairedUsablePoints.ContainsKey(tag);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001D20E File Offset: 0x0001B40E
		public IEnumerable<string> GetAllSpawnTags()
		{
			return this._usablePoints.Keys.ToList<string>().Concat<string>(this._pairedUsablePoints.Keys.ToList<string>());
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001D238 File Offset: 0x0001B438
		public List<UsableMachine> GetAllUsablePointsWithTag(string tag)
		{
			List<UsableMachine> list = new List<UsableMachine>();
			List<UsableMachine> list2 = new List<UsableMachine>();
			if (this._usablePoints.TryGetValue(tag, out list2))
			{
				list.AddRange(list2);
			}
			List<UsableMachine> list3 = new List<UsableMachine>();
			if (this._pairedUsablePoints.TryGetValue(tag, out list3))
			{
				list.AddRange(list3);
			}
			return list;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001D288 File Offset: 0x0001B488
		public Agent SpawnWanderingAgent(LocationCharacter locationCharacter)
		{
			WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
			bool flag = false;
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			SandBoxHelpers.MissionHelper.RemapSpecialTagIfNecessary(locationCharacter, this);
			if (locationCharacter.SpecialTargetTag != null)
			{
				flag = this.GetInitialFrameForSpawnTag(locationCharacter.SpecialTargetTag, ref weakGameEntity, ref matrixFrame);
			}
			if (!locationCharacter.ForceSpawnInSpecialTargetTag)
			{
				if (!flag)
				{
					flag = this.GetInitialFrameForSpawnTag("npc_common_limited", ref weakGameEntity, ref matrixFrame);
				}
				if (!flag)
				{
					flag = this.GetInitialFrameForSpawnTag("npc_common", ref weakGameEntity, ref matrixFrame);
				}
				if (!flag && this._usablePoints.Count > 0)
				{
					foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair in this._usablePoints)
					{
						if (keyValuePair.Value.Count > 0)
						{
							foreach (UsableMachine usableMachine in keyValuePair.Value)
							{
								MatrixFrame matrixFrame2;
								if (this.GetSpawnFrameFromUsableMachine(usableMachine, out matrixFrame2))
								{
									matrixFrame = matrixFrame2;
									flag = true;
									weakGameEntity = usableMachine.GameEntity;
									break;
								}
							}
						}
					}
				}
				if (!flag && this._pairedUsablePoints.Count > 0)
				{
					foreach (KeyValuePair<string, List<UsableMachine>> keyValuePair2 in this._pairedUsablePoints)
					{
						if (keyValuePair2.Value.Count > 0)
						{
							foreach (UsableMachine usableMachine2 in keyValuePair2.Value)
							{
								MatrixFrame matrixFrame3;
								if (this.GetSpawnFrameFromUsableMachine(usableMachine2, out matrixFrame3))
								{
									matrixFrame = matrixFrame3;
									flag = true;
									weakGameEntity = usableMachine2.GameEntity;
									break;
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				matrixFrame.rotation.f.z = 0f;
				matrixFrame.rotation.f.Normalize();
				matrixFrame.rotation.u = Vec3.Up;
				matrixFrame.rotation.s = Vec3.CrossProduct(matrixFrame.rotation.f, matrixFrame.rotation.u);
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				bool flag2 = weakGameEntity.HasTag("torch") && !Campaign.Current.IsDay;
				Agent agent = this.SpawnWanderingAgentWithInitialFrame(locationCharacter, matrixFrame, weakGameEntity, true, flag2);
				agent.SetAgentExcludeStateForFaceGroupId(MissionAgentHandler._disabledFaceId, true);
				return agent;
			}
			return null;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001D51C File Offset: 0x0001B71C
		private bool GetSpawnFrameFromUsableMachine(UsableMachine usableMachine, out MatrixFrame frame)
		{
			frame = MatrixFrame.Identity;
			StandingPoint randomElementWithPredicate = usableMachine.StandingPoints.GetRandomElementWithPredicate<StandingPoint>((StandingPoint x) => !x.HasUser && !x.IsDeactivated && !x.IsDisabled);
			if (randomElementWithPredicate != null)
			{
				frame = randomElementWithPredicate.GameEntity.GetGlobalFrame();
				return true;
			}
			return false;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001D57C File Offset: 0x0001B77C
		public void SpawnWanderingAgentWithDelay(LocationCharacter locationCharacter, MatrixFrame matrixFrame, GameEntity spawnEntity, bool noHorses = true, bool hasTorch = false, float delay = 3f)
		{
			if (delay > 0f)
			{
				this._spawnTimers.Add(new ValueTuple<LocationCharacter, MatrixFrame, GameEntity, bool, bool, Timer>(locationCharacter, matrixFrame, spawnEntity, noHorses, hasTorch, new Timer(base.Mission.CurrentTime, delay, false)));
				return;
			}
			Debug.FailedAssert("delay > 0", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\MissionAgentHandler.cs", "SpawnWanderingAgentWithDelay", 1037);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001D5D8 File Offset: 0x0001B7D8
		public Agent SpawnWanderingAgentWithInitialFrame(LocationCharacter locationCharacter, MatrixFrame spawnPointFrame, WeakGameEntity spawnEntity, bool noHorses = true, bool hasTorch = false)
		{
			Team team = Team.Invalid;
			switch (locationCharacter.CharacterRelation)
			{
			case LocationCharacter.CharacterRelations.Neutral:
				team = Team.Invalid;
				break;
			case LocationCharacter.CharacterRelations.Friendly:
				team = base.Mission.PlayerAllyTeam;
				break;
			case LocationCharacter.CharacterRelations.Enemy:
				team = base.Mission.PlayerEnemyTeam;
				break;
			}
			spawnPointFrame.origin.z = base.Mission.Scene.GetGroundHeightAtPosition(spawnPointFrame.origin, BodyFlags.CommonCollisionExcludeFlags);
			ValueTuple<uint, uint> agentSettlementColors = MissionAgentHandler.GetAgentSettlementColors(locationCharacter);
			AgentBuildData agentBuildData = locationCharacter.GetAgentBuildData().Team(team).InitialPosition(in spawnPointFrame.origin);
			Vec2 vec = spawnPointFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).ClothingColor1(agentSettlementColors.Item1).ClothingColor2(agentSettlementColors.Item2)
				.CivilianEquipment(locationCharacter.UseCivilianEquipment)
				.NoHorses(noHorses);
			CharacterObject character = locationCharacter.Character;
			Banner banner;
			if (character == null)
			{
				banner = null;
			}
			else
			{
				Hero heroObject = character.HeroObject;
				if (heroObject == null)
				{
					banner = null;
				}
				else
				{
					Clan clan = heroObject.Clan;
					banner = ((clan != null) ? clan.Banner : null);
				}
			}
			AgentBuildData agentBuildData3 = agentBuildData2.Banner(banner);
			if (hasTorch)
			{
				Equipment equipment = locationCharacter.Character.Equipment.Clone(false);
				equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("torch"), null, null, false);
				agentBuildData3 = agentBuildData3.Equipment(equipment);
			}
			Agent agent = base.Mission.SpawnAgent(agentBuildData3, false, null, null);
			agent.SetAgentExcludeStateForFaceGroupId(MissionAgentHandler._disabledFaceId, true);
			if (hasTorch)
			{
				EquipmentIndex equipmentIndex;
				EquipmentIndex equipmentIndex2;
				bool flag;
				agent.SpawnEquipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
				if (equipmentIndex2 != EquipmentIndex.None)
				{
					agent.TryToWieldWeaponInSlot(equipmentIndex2, Agent.WeaponWieldActionType.InstantAfterPickUp, true);
				}
			}
			AnimationSystemData animationSystemData = agentBuildData3.AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSet(locationCharacter.ActionSetCode), locationCharacter.Character.GetStepSize(), false);
			agent.SetActionSet(ref animationSystemData);
			agent.GetComponent<CampaignAgentComponent>().CreateAgentNavigator(locationCharacter);
			locationCharacter.AddBehaviors(agent);
			LocationCharacter.AfterAgentCreatedDelegate afterAgentCreated = locationCharacter.AfterAgentCreated;
			if (afterAgentCreated != null)
			{
				afterAgentCreated(agent);
			}
			Game.Current.EventManager.TriggerEvent<LocationCharacterAgentSpawnedMissionEvent>(new LocationCharacterAgentSpawnedMissionEvent(locationCharacter, agent, spawnEntity));
			return agent;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001D7DA File Offset: 0x0001B9DA
		public static uint GetRandomTournamentTeamColor(int teamIndex)
		{
			return MissionAgentHandler._tournamentTeamColors[teamIndex % MissionAgentHandler._tournamentTeamColors.Length];
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001D7EC File Offset: 0x0001B9EC
		[return: TupleElementNames(new string[] { "color1", "color2" })]
		public static ValueTuple<uint, uint> GetAgentSettlementColors(LocationCharacter locationCharacter)
		{
			CharacterObject character = locationCharacter.Character;
			if (character.IsHero)
			{
				if (character.HeroObject.Clan == CharacterObject.PlayerCharacter.HeroObject.Clan)
				{
					return new ValueTuple<uint, uint>(Clan.PlayerClan.MapFaction.Color, Clan.PlayerClan.MapFaction.Color2);
				}
				if (!character.HeroObject.IsNotable)
				{
					return new ValueTuple<uint, uint>(locationCharacter.AgentData.AgentClothingColor1, locationCharacter.AgentData.AgentClothingColor2);
				}
				CharacterObject characterObject = character;
				MobileParty partyBelongedTo = character.HeroObject.PartyBelongedTo;
				return CharacterHelper.GetDeterministicColorsForCharacter(characterObject, (partyBelongedTo != null) ? partyBelongedTo.Party : null);
			}
			else
			{
				if (character.IsSoldier)
				{
					return new ValueTuple<uint, uint>(Settlement.CurrentSettlement.MapFaction.Color, Settlement.CurrentSettlement.MapFaction.Color2);
				}
				return new ValueTuple<uint, uint>(MissionAgentHandler._villagerClothColors[MBRandom.RandomInt(MissionAgentHandler._villagerClothColors.Length)], MissionAgentHandler._villagerClothColors[MBRandom.RandomInt(MissionAgentHandler._villagerClothColors.Length)]);
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001D8E8 File Offset: 0x0001BAE8
		public UsableMachine FindUnusedPointWithTagForAgent(Agent agent, string tag)
		{
			UsableMachine usableMachine = this.FindUnusedPointForAgent(agent, this._pairedUsablePoints, tag);
			if (usableMachine == null || usableMachine.StandingPoints.Any<StandingPoint>((StandingPoint x) => x.HasUser && x.UserAgent == agent))
			{
				usableMachine = this.FindUnusedPointForAgent(agent, this._usablePoints, tag);
			}
			return usableMachine;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001D948 File Offset: 0x0001BB48
		public List<UsableMachine> FindUnusedPoints(string tag)
		{
			List<UsableMachine> list;
			if (this._usablePoints.TryGetValue(tag, out list))
			{
				return list;
			}
			return null;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001D968 File Offset: 0x0001BB68
		private UsableMachine FindUnusedPointForAgent(Agent agent, Dictionary<string, List<UsableMachine>> usableMachinesList, string primaryTag)
		{
			List<UsableMachine> list;
			if (usableMachinesList.TryGetValue(primaryTag, out list) && list.Count > 0)
			{
				int num = MBRandom.RandomInt(0, list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					UsableMachine usableMachine = list[(num + i) % list.Count];
					if (!usableMachine.IsDisabled && !usableMachine.IsDestroyed && usableMachine.IsStandingPointAvailableForAgent(agent))
					{
						return usableMachine;
					}
				}
			}
			return null;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001D9D4 File Offset: 0x0001BBD4
		public List<UsableMachine> FindAllUnusedPoints(Agent agent, string primaryTag)
		{
			List<UsableMachine> list = new List<UsableMachine>();
			List<UsableMachine> list2 = new List<UsableMachine>();
			List<UsableMachine> list3;
			this._usablePoints.TryGetValue(primaryTag, out list3);
			List<UsableMachine> list4;
			this._pairedUsablePoints.TryGetValue(primaryTag, out list4);
			list4 = ((list4 != null) ? list4.Distinct<UsableMachine>().ToList<UsableMachine>() : null);
			if (list3 != null && list3.Count > 0)
			{
				list.AddRange(list3);
			}
			if (list4 != null && list4.Count > 0)
			{
				list.AddRange(list4);
			}
			if (list.Count > 0)
			{
				Predicate<StandingPoint> <>9__0;
				foreach (UsableMachine usableMachine in list)
				{
					List<StandingPoint> standingPoints = usableMachine.StandingPoints;
					Predicate<StandingPoint> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (StandingPoint sp) => (sp.IsInstantUse || (!sp.HasUser && !sp.HasAIMovingTo)) && !sp.IsDisabledForAgent(agent));
					}
					if (standingPoints.Exists(predicate))
					{
						list2.Add(usableMachine);
					}
				}
			}
			return list2;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		public void TeleportTargetAgentNearReferenceAgent(Agent referenceAgent, Agent teleportAgent, bool teleportFollowers, bool teleportOpposite)
		{
			Vec3 vec = referenceAgent.Position + referenceAgent.LookDirection.NormalizedCopy() * 4f;
			Vec3 vec2;
			if (teleportOpposite)
			{
				vec2 = vec;
				vec2.z = base.Mission.Scene.GetGroundHeightAtPosition(vec2, BodyFlags.CommonCollisionExcludeFlags);
			}
			else
			{
				vec2 = Mission.Current.GetRandomPositionAroundPoint(referenceAgent.Position, 2f, 4f, true);
				vec2.z = base.Mission.Scene.GetGroundHeightAtPosition(vec2, BodyFlags.CommonCollisionExcludeFlags);
			}
			WorldFrame worldFrame = new WorldFrame(referenceAgent.Frame.rotation, new WorldPosition(base.Mission.Scene, referenceAgent.Frame.origin));
			Vec3 vec3 = new Vec3(worldFrame.Origin.AsVec2 - vec2.AsVec2, 0f, -1f);
			teleportAgent.LookDirection = vec3.NormalizedCopy();
			teleportAgent.TeleportToPosition(vec2);
			if (teleportFollowers && teleportAgent.Controller == AgentControllerType.Player)
			{
				foreach (Agent agent in base.Mission.Agents)
				{
					LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(agent.Origin);
					AccompanyingCharacter accompanyingCharacter = PlayerEncounter.LocationEncounter.GetAccompanyingCharacter(locationCharacter);
					if (agent.GetComponent<CampaignAgentComponent>().AgentNavigator != null && accompanyingCharacter != null && accompanyingCharacter.IsFollowingPlayerAtMissionStart)
					{
						MatrixFrame matrixFrame;
						this.GetFrameForFollowingAgent(teleportAgent, out matrixFrame);
						agent.TeleportToPosition(matrixFrame.origin);
					}
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001DC80 File Offset: 0x0001BE80
		public static int GetPointCountOfUsableMachine(UsableMachine usableMachine, bool checkForUnusedOnes)
		{
			int num = 0;
			List<AnimationPoint> list = new List<AnimationPoint>();
			foreach (StandingPoint standingPoint in usableMachine.StandingPoints)
			{
				if (!standingPoint.IsDeactivated && !standingPoint.IsDisabled && !standingPoint.IsInstantUse && (!checkForUnusedOnes || (!standingPoint.HasUser && !standingPoint.HasAIMovingTo)))
				{
					AnimationPoint animationPoint = standingPoint as AnimationPoint;
					if (animationPoint != null && animationPoint.IsActive)
					{
						List<AnimationPoint> alternatives = animationPoint.GetAlternatives();
						if (alternatives.Count == 0)
						{
							num++;
						}
						else if (!list.Contains(animationPoint))
						{
							if (checkForUnusedOnes)
							{
								if (alternatives.Any<AnimationPoint>((AnimationPoint x) => x.HasUser && x.HasAIMovingTo))
								{
									continue;
								}
							}
							list.AddRange(alternatives);
							num++;
						}
					}
					else
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0400026E RID: 622
		private const float PassageUsageDeltaTime = 30f;

		// Token: 0x0400026F RID: 623
		private static readonly uint[] _tournamentTeamColors = new uint[]
		{
			4294110933U, 4290269521U, 4291535494U, 4286151096U, 4290286497U, 4291600739U, 4291868275U, 4287285710U, 4283204487U, 4287282028U,
			4290300789U
		};

		// Token: 0x04000270 RID: 624
		private static readonly uint[] _villagerClothColors = new uint[]
		{
			4292860590U, 4291351206U, 4289117081U, 4288460959U, 4287541416U, 4288922566U, 4292654718U, 4289243320U, 4290286483U, 4290288531U,
			4290156159U, 4291136871U, 4289233774U, 4291205980U, 4291735684U, 4292722283U, 4293119406U, 4293911751U, 4294110933U, 4291535494U,
			4289955192U, 4289631650U, 4292133587U, 4288785593U, 4286288275U, 4286222496U, 4287601851U, 4286622134U, 4285898909U, 4285638289U,
			4289830302U, 4287593853U, 4289957781U, 4287071646U, 4284445583U
		};

		// Token: 0x04000271 RID: 625
		private static int _disabledFaceId = -1;

		// Token: 0x04000272 RID: 626
		private static int _disabledFaceIdForAnimals = 1;

		// Token: 0x04000273 RID: 627
		private readonly Dictionary<string, List<UsableMachine>> _usablePoints;

		// Token: 0x04000274 RID: 628
		private readonly Dictionary<string, List<UsableMachine>> _pairedUsablePoints;

		// Token: 0x04000275 RID: 629
		private readonly HashSet<UsableMachine> _usedSpawnPoints;

		// Token: 0x04000276 RID: 630
		private List<UsableMachine> _disabledPassages;

		// Token: 0x04000277 RID: 631
		private readonly List<ValueTuple<LocationCharacter, MatrixFrame, GameEntity, bool, bool, Timer>> _spawnTimers = new List<ValueTuple<LocationCharacter, MatrixFrame, GameEntity, bool, bool, Timer>>();

		// Token: 0x04000278 RID: 632
		private float _passageUsageTime;
	}
}
