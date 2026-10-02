using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Tournaments.AgentControllers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Tournaments.MissionLogics
{
	// Token: 0x02000030 RID: 48
	public class TownHorseRaceMissionController : MissionLogic, ITournamentGameBehavior
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000BA50 File Offset: 0x00009C50
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000BA58 File Offset: 0x00009C58
		public List<TownHorseRaceMissionController.CheckPoint> CheckPoints { get; private set; }

		// Token: 0x060001C1 RID: 449 RVA: 0x0000BA61 File Offset: 0x00009C61
		public TownHorseRaceMissionController(CultureObject culture)
		{
			this._culture = culture;
			this._agents = new List<TownHorseRaceAgentController>();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000BA7C File Offset: 0x00009C7C
		public override void AfterStart()
		{
			base.AfterStart();
			this.CollectCheckPointsAndStartPoints();
			foreach (TownHorseRaceAgentController townHorseRaceAgentController in this._agents)
			{
				townHorseRaceAgentController.DisableMovement();
			}
			this._startTimer = new BasicMissionTimer();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._startTimer != null && this._startTimer.ElapsedTime > 3f)
			{
				foreach (TownHorseRaceAgentController townHorseRaceAgentController in this._agents)
				{
					townHorseRaceAgentController.Start();
				}
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000BB58 File Offset: 0x00009D58
		private void CollectCheckPointsAndStartPoints()
		{
			this.CheckPoints = new List<TownHorseRaceMissionController.CheckPoint>();
			foreach (WeakGameEntity weakGameEntity in base.Mission.ActiveMissionObjects.Select<MissionObject, WeakGameEntity>((MissionObject amo) => amo.GameEntity))
			{
				VolumeBox firstScriptOfType = weakGameEntity.GetFirstScriptOfType<VolumeBox>();
				if (firstScriptOfType != null)
				{
					this.CheckPoints.Add(new TownHorseRaceMissionController.CheckPoint(firstScriptOfType));
				}
			}
			this.CheckPoints = this.CheckPoints.OrderBy<TownHorseRaceMissionController.CheckPoint, string>((TownHorseRaceMissionController.CheckPoint x) => x.Name).ToList<TownHorseRaceMissionController.CheckPoint>();
			this._startPoints = base.Mission.Scene.FindEntitiesWithTag("sp_horse_race").ToList<GameEntity>();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000BC44 File Offset: 0x00009E44
		private MatrixFrame GetStartFrame(int index)
		{
			MatrixFrame matrixFrame;
			if (index < this._startPoints.Count)
			{
				matrixFrame = this._startPoints[index].GetGlobalFrame();
			}
			else
			{
				matrixFrame = ((this._startPoints.Count > 0) ? this._startPoints[0].GetGlobalFrame() : MatrixFrame.Identity);
			}
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return matrixFrame;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000BCA8 File Offset: 0x00009EA8
		private void SetItemsAndSpawnCharacter(CharacterObject troop)
		{
			int count = this._agents.Count;
			Equipment equipment = new Equipment();
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ArmorItemEndSlot, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("charger"), null, null, false));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.HorseHarness, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("horse_harness_e"), null, null, false));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("horse_whip"), null, null, false));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Body, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("short_padded_robe"), null, null, false));
			MatrixFrame startFrame = this.GetStartFrame(count);
			AgentBuildData agentBuildData = new AgentBuildData(troop).Team(this._teams[count]).InitialPosition(in startFrame.origin);
			Vec2 vec = startFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).Equipment(equipment).Controller((troop == CharacterObject.PlayerCharacter) ? AgentControllerType.Player : AgentControllerType.AI);
			Agent agent = base.Mission.SpawnAgent(agentBuildData2, false, null, null);
			agent.Health = (float)agent.Monster.HitPoints;
			agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			this._agents.Add(this.AddHorseRaceAgentController(agent));
			if (troop == CharacterObject.PlayerCharacter)
			{
				base.Mission.PlayerTeam = this._teams[count];
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000BE1B File Offset: 0x0000A01B
		private TownHorseRaceAgentController AddHorseRaceAgentController(Agent agent)
		{
			return agent.AddController(typeof(TownHorseRaceAgentController)) as TownHorseRaceAgentController;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000BE34 File Offset: 0x0000A034
		private void InitializeTeams(int count)
		{
			this._teams = new List<Team>();
			for (int i = 0; i < count; i++)
			{
				this._teams.Add(base.Mission.Teams.Add(BattleSideEnum.None, uint.MaxValue, uint.MaxValue, null, true, false, true));
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000BE7A File Offset: 0x0000A07A
		public void StartMatch(TournamentMatch match, bool isLastRound)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BE81 File Offset: 0x0000A081
		public void SkipMatch(TournamentMatch match)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000BE88 File Offset: 0x0000A088
		public bool IsMatchEnded()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000BE8F File Offset: 0x0000A08F
		public void OnMatchEnded()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400009E RID: 158
		public const int TourCount = 2;

		// Token: 0x040000A0 RID: 160
		private readonly List<TownHorseRaceAgentController> _agents;

		// Token: 0x040000A1 RID: 161
		private List<Team> _teams;

		// Token: 0x040000A2 RID: 162
		private List<GameEntity> _startPoints;

		// Token: 0x040000A3 RID: 163
		private BasicMissionTimer _startTimer;

		// Token: 0x040000A4 RID: 164
		private CultureObject _culture;

		// Token: 0x0200014F RID: 335
		public class CheckPoint
		{
			// Token: 0x17000138 RID: 312
			// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00065FAC File Offset: 0x000641AC
			public string Name
			{
				get
				{
					return this._volumeBox.GameEntity.Name;
				}
			}

			// Token: 0x06000E5B RID: 3675 RVA: 0x00065FCC File Offset: 0x000641CC
			public CheckPoint(VolumeBox volumeBox)
			{
				this._volumeBox = volumeBox;
				this._bestTargetList = GameEntity.CreateFromWeakEntity(this._volumeBox.GameEntity).CollectChildrenEntitiesWithTag("best_target_point");
				this._volumeBox.SetIsOccupiedDelegate(new VolumeBox.VolumeBoxDelegate(this.OnAgentsEnterCheckBox));
			}

			// Token: 0x06000E5C RID: 3676 RVA: 0x00066020 File Offset: 0x00064220
			public Vec3 GetBestTargetPosition()
			{
				Vec3 vec;
				if (this._bestTargetList.Count > 0)
				{
					vec = this._bestTargetList[MBRandom.RandomInt(this._bestTargetList.Count)].GetGlobalFrame().origin;
				}
				else
				{
					vec = this._volumeBox.GameEntity.GetGlobalFrame().origin;
				}
				return vec;
			}

			// Token: 0x06000E5D RID: 3677 RVA: 0x0006607D File Offset: 0x0006427D
			public void AddToCheckList(Agent agent)
			{
				this._volumeBox.AddToCheckList(agent);
			}

			// Token: 0x06000E5E RID: 3678 RVA: 0x0006608B File Offset: 0x0006428B
			public void RemoveFromCheckList(Agent agent)
			{
				this._volumeBox.RemoveFromCheckList(agent);
			}

			// Token: 0x06000E5F RID: 3679 RVA: 0x0006609C File Offset: 0x0006429C
			private void OnAgentsEnterCheckBox(VolumeBox volumeBox, List<Agent> agentsInVolume)
			{
				foreach (Agent agent in agentsInVolume)
				{
					agent.GetController<TownHorseRaceAgentController>().OnEnterCheckPoint(volumeBox);
				}
			}

			// Token: 0x0400067C RID: 1660
			private readonly VolumeBox _volumeBox;

			// Token: 0x0400067D RID: 1661
			private readonly List<GameEntity> _bestTargetList;
		}
	}
}
