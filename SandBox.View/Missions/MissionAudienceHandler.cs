using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000015 RID: 21
	public class MissionAudienceHandler : MissionView
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00004AB5 File Offset: 0x00002CB5
		public MissionAudienceHandler(float density)
		{
			this._density = density;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004AC4 File Offset: 0x00002CC4
		public override void EarlyStart()
		{
			this._allOneShotSoundEventsAreDisabled = true;
			this._audienceMidPoints = base.Mission.Scene.FindEntitiesWithTag("audience_mid_point").ToList<GameEntity>();
			this._arenaSoundEntity = base.Mission.Scene.FindEntityWithTag("arena_sound");
			this._audienceList = new List<KeyValuePair<GameEntity, float>>();
			if (this._audienceMidPoints.Count > 0)
			{
				this.OnInit();
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004B34 File Offset: 0x00002D34
		public void OnInit()
		{
			this._minChance = MathF.Max(this._density - 0.5f, 0f);
			this._maxChance = this._density;
			this.GetAudienceEntities();
			this.SpawnAudienceAgents();
			this._lastOneShotSoundEventStarted = MissionTime.Zero;
			this._allOneShotSoundEventsAreDisabled = false;
			this._ambientSoundEvent = SoundManager.CreateEvent("event:/mission/ambient/detail/arena/arena", base.Mission.Scene);
			this._ambientSoundEvent.Play();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004BAE File Offset: 0x00002DAE
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman)
			{
				this.Cheer(false);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004BCC File Offset: 0x00002DCC
		private void Cheer(bool onEnd = false)
		{
			if (!this._allOneShotSoundEventsAreDisabled)
			{
				string text = null;
				if (onEnd)
				{
					text = "event:/mission/ambient/detail/arena/cheer_big";
					this._allOneShotSoundEventsAreDisabled = true;
				}
				else if (this._lastOneShotSoundEventStarted.ElapsedSeconds > 4f && this._lastOneShotSoundEventStarted.ElapsedSeconds < 10f)
				{
					text = "event:/mission/ambient/detail/arena/cheer_medium";
				}
				else if (this._lastOneShotSoundEventStarted.ElapsedSeconds > 10f)
				{
					text = "event:/mission/ambient/detail/arena/cheer_small";
				}
				if (text != null)
				{
					Vec3 vec = ((this._arenaSoundEntity != null) ? this._arenaSoundEntity.GlobalPosition : (this._audienceMidPoints.Any<GameEntity>() ? this._audienceMidPoints.GetRandomElement<GameEntity>().GlobalPosition : Vec3.Zero));
					SoundManager.StartOneShotEvent(text, in vec);
					this._lastOneShotSoundEventStarted = MissionTime.Now;
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004C94 File Offset: 0x00002E94
		private void GetAudienceEntities()
		{
			this._maxDist = 0f;
			this._minDist = float.MaxValue;
			this._maxHeight = 0f;
			this._minHeight = float.MaxValue;
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag("audience"))
			{
				float distanceSquareToArena = this.GetDistanceSquareToArena(gameEntity);
				this._maxDist = ((distanceSquareToArena > this._maxDist) ? distanceSquareToArena : this._maxDist);
				this._minDist = ((distanceSquareToArena < this._minDist) ? distanceSquareToArena : this._minDist);
				float z = gameEntity.GetFrame().origin.z;
				this._maxHeight = ((z > this._maxHeight) ? z : this._maxHeight);
				this._minHeight = ((z < this._minHeight) ? z : this._minHeight);
				this._audienceList.Add(new KeyValuePair<GameEntity, float>(gameEntity, distanceSquareToArena));
				gameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004DB0 File Offset: 0x00002FB0
		private float GetDistanceSquareToArena(GameEntity audienceEntity)
		{
			float num = float.MaxValue;
			foreach (GameEntity gameEntity in this._audienceMidPoints)
			{
				float num2 = gameEntity.GlobalPosition.DistanceSquared(audienceEntity.GlobalPosition);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004E1C File Offset: 0x0000301C
		private CharacterObject GetRandomAudienceCharacterToSpawn()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			CharacterObject characterObject = MBRandom.ChooseWeighted<CharacterObject>(new List<ValueTuple<CharacterObject, float>>
			{
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Townswoman, 0.2f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Townsman, 0.2f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Armorer, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Merchant, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Musician, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Weaponsmith, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.RansomBroker, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Barber, 0.05f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.FemaleDancer, 0.05f)
			});
			if (characterObject == null)
			{
				characterObject = ((MBRandom.RandomFloat < 0.65f) ? currentSettlement.Culture.Townsman : currentSettlement.Culture.Townswoman);
			}
			return characterObject;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004F58 File Offset: 0x00003158
		private void SpawnAudienceAgents()
		{
			for (int i = this._audienceList.Count - 1; i >= 0; i--)
			{
				KeyValuePair<GameEntity, float> keyValuePair = this._audienceList[i];
				float num = this._minChance + (1f - (keyValuePair.Value - this._minDist) / (this._maxDist - this._minDist)) * (this._maxChance - this._minChance);
				float num2 = this._minChance + (1f - MathF.Pow((keyValuePair.Key.GetFrame().origin.z - this._minHeight) / (this._maxHeight - this._minHeight), 2f)) * (this._maxChance - this._minChance);
				float num3 = num * 0.4f + num2 * 0.6f;
				if (MBRandom.RandomFloat < num3)
				{
					MatrixFrame globalFrame = keyValuePair.Key.GetGlobalFrame();
					CharacterObject randomAudienceCharacterToSpawn = this.GetRandomAudienceCharacterToSpawn();
					AgentBuildData agentBuildData = new AgentBuildData(randomAudienceCharacterToSpawn).InitialPosition(in globalFrame.origin);
					Vec2 vec = new Vec2(-globalFrame.rotation.f.AsVec2.x, -globalFrame.rotation.f.AsVec2.y);
					AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).TroopOrigin(new SimpleAgentOrigin(randomAudienceCharacterToSpawn, -1, null, default(UniqueTroopDescriptor))).Team(Team.Invalid)
						.ClothingColor1(Settlement.CurrentSettlement.MapFaction.Color)
						.ClothingColor2(Settlement.CurrentSettlement.MapFaction.Color2)
						.NoHorses(true)
						.CanSpawnOutsideOfMissionBoundary(true);
					Agent agent = Mission.Current.SpawnAgent(agentBuildData2, false, null, null);
					MBActionSet actionSetWithIndex = MBActionSet.GetActionSetWithIndex(0);
					AnimationSystemData animationSystemData = agentBuildData2.AgentMonster.FillAnimationSystemData(actionSetWithIndex, randomAudienceCharacterToSpawn.GetStepSize(), false);
					agent.SetActionSet(ref animationSystemData);
					MBAnimation.PrefetchAnimationClip(agent.ActionSet, ActionIndexCache.act_arena_spectator);
					agent.SetActionChannel(0, in ActionIndexCache.act_arena_spectator, true, (AnimFlags)0UL, 0f, MBRandom.RandomFloatRanged(0.75f, 1f), -0.2f, 0.4f, MBRandom.RandomFloatRanged(0.01f, 1f), false, -0.2f, 0, true);
					agent.Controller = AgentControllerType.None;
					agent.ToggleInvulnerable();
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005187 File Offset: 0x00003387
		public override void OnMissionTick(float dt)
		{
			if (this._audienceMidPoints == null)
			{
				return;
			}
			if (base.Mission.MissionEnded)
			{
				this.Cheer(true);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000051A6 File Offset: 0x000033A6
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			if (oldMissionMode == MissionMode.Battle && Mission.Current.Mode == MissionMode.StartUp && Agent.Main != null && Agent.Main.IsActive())
			{
				this.Cheer(true);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000051D2 File Offset: 0x000033D2
		public override void OnMissionScreenFinalize()
		{
			SoundEvent ambientSoundEvent = this._ambientSoundEvent;
			if (ambientSoundEvent == null)
			{
				return;
			}
			ambientSoundEvent.Release();
		}

		// Token: 0x0400001A RID: 26
		private const int GapBetweenCheerSmallInSeconds = 10;

		// Token: 0x0400001B RID: 27
		private const int GapBetweenCheerMedium = 4;

		// Token: 0x0400001C RID: 28
		private float _minChance;

		// Token: 0x0400001D RID: 29
		private float _maxChance;

		// Token: 0x0400001E RID: 30
		private float _minDist;

		// Token: 0x0400001F RID: 31
		private float _maxDist;

		// Token: 0x04000020 RID: 32
		private float _minHeight;

		// Token: 0x04000021 RID: 33
		private float _maxHeight;

		// Token: 0x04000022 RID: 34
		private List<GameEntity> _audienceMidPoints;

		// Token: 0x04000023 RID: 35
		private List<KeyValuePair<GameEntity, float>> _audienceList;

		// Token: 0x04000024 RID: 36
		private readonly float _density;

		// Token: 0x04000025 RID: 37
		private GameEntity _arenaSoundEntity;

		// Token: 0x04000026 RID: 38
		private SoundEvent _ambientSoundEvent;

		// Token: 0x04000027 RID: 39
		private MissionTime _lastOneShotSoundEventStarted;

		// Token: 0x04000028 RID: 40
		private bool _allOneShotSoundEventsAreDisabled;
	}
}
