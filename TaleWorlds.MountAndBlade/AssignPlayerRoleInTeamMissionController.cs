using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000279 RID: 633
	public class AssignPlayerRoleInTeamMissionController : MissionLogic
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600238D RID: 9101 RVA: 0x0007E4F4 File Offset: 0x0007C6F4
		// (remove) Token: 0x0600238E RID: 9102 RVA: 0x0007E52C File Offset: 0x0007C72C
		public event PlayerTurnToChooseFormationToLeadEvent OnPlayerTurnToChooseFormationToLead;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600238F RID: 9103 RVA: 0x0007E564 File Offset: 0x0007C764
		// (remove) Token: 0x06002390 RID: 9104 RVA: 0x0007E59C File Offset: 0x0007C79C
		public event AllFormationsAssignedSergeantsEvent OnAllFormationsAssignedSergeants;

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0007E5D1 File Offset: 0x0007C7D1
		public bool IsPlayerInArmy { get; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x0007E5D9 File Offset: 0x0007C7D9
		public bool IsPlayerGeneral { get; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x0007E5E1 File Offset: 0x0007C7E1
		public bool IsPlayerSergeant { get; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x0007E5E9 File Offset: 0x0007C7E9
		// (set) Token: 0x06002395 RID: 9109 RVA: 0x0007E5F1 File Offset: 0x0007C7F1
		public int PlayerChosenIndex { get; protected set; }

		// Token: 0x06002396 RID: 9110 RVA: 0x0007E5FA File Offset: 0x0007C7FA
		public AssignPlayerRoleInTeamMissionController(bool isPlayerGeneral, bool isPlayerSergeant, bool isPlayerInArmy, List<string> charactersInPlayerSideByPriority = null)
		{
			this.IsPlayerGeneral = isPlayerGeneral;
			this.IsPlayerSergeant = isPlayerSergeant;
			this.IsPlayerInArmy = isPlayerInArmy;
			this.PlayerChosenIndex = -1;
			this.CharactersInPlayerSideByPriority = charactersInPlayerSideByPriority;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0007E626 File Offset: 0x0007C826
		public override void AfterStart()
		{
			Mission.Current.PlayerTeam.SetPlayerRole(this.IsPlayerGeneral, this.IsPlayerSergeant);
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0007E644 File Offset: 0x0007C844
		public override void OnBattleSideSpawned(BattleSideEnum side)
		{
			foreach (Team team in Mission.GetTeamsOfSide(side))
			{
				if (team == base.Mission.PlayerTeam)
				{
					team.PlayerOrderController.Owner = base.Mission.InitialPlayerAgent;
					if (team.IsPlayerGeneral)
					{
						foreach (Formation formation in team.FormationsIncludingEmpty)
						{
							formation.PlayerOwner = base.Mission.InitialPlayerAgent;
						}
					}
					team.PlayerOrderController.SelectAllFormations(false);
				}
			}
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0007E710 File Offset: 0x0007C910
		public virtual void OnPlayerTeamDeployed()
		{
			if (MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
			{
				Team playerTeam = Mission.Current.PlayerTeam;
				this.FormationsLockedWithSergeants = new Dictionary<int, Agent>();
				this.FormationsWithLooselyChosenSergeants = new Dictionary<int, Agent>();
				if (playerTeam.IsPlayerGeneral)
				{
					this.CharacterNamesInPlayerSideByPriorityQueue = new Queue<string>();
					this.RemainingFormationsToAssignSergeantsTo = new List<Formation>();
				}
				else
				{
					this.CharacterNamesInPlayerSideByPriorityQueue = ((this.CharactersInPlayerSideByPriority != null) ? new Queue<string>(this.CharactersInPlayerSideByPriority) : new Queue<string>());
					this.RemainingFormationsToAssignSergeantsTo = playerTeam.FormationsIncludingSpecialAndEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0).ToList<Formation>();
					while (this.RemainingFormationsToAssignSergeantsTo.Count > 0 && this.CharacterNamesInPlayerSideByPriorityQueue.Count > 0)
					{
						string nextAgentNameToProcess = this.CharacterNamesInPlayerSideByPriorityQueue.Dequeue();
						Agent agent = playerTeam.ActiveAgents.FirstOrDefault<Agent>((Agent aa) => aa.Character.StringId.Equals(nextAgentNameToProcess));
						if (agent != null)
						{
							if (agent == base.Mission.InitialPlayerAgent)
							{
								break;
							}
							Formation formation = this.ChooseFormationToLead(this.RemainingFormationsToAssignSergeantsTo, agent);
							if (formation != null)
							{
								this.FormationsLockedWithSergeants.Add(formation.Index, agent);
								this.RemainingFormationsToAssignSergeantsTo.Remove(formation);
							}
						}
					}
				}
				PlayerTurnToChooseFormationToLeadEvent onPlayerTurnToChooseFormationToLead = this.OnPlayerTurnToChooseFormationToLead;
				if (onPlayerTurnToChooseFormationToLead == null)
				{
					return;
				}
				onPlayerTurnToChooseFormationToLead(this.FormationsLockedWithSergeants, this.RemainingFormationsToAssignSergeantsTo.Select<Formation, int>((Formation ftcsf) => ftcsf.Index).ToList<int>());
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0007E8A8 File Offset: 0x0007CAA8
		public virtual void OnPlayerChoiceMade(int chosenIndex)
		{
			if (this.PlayerChosenIndex != chosenIndex)
			{
				this.PlayerChosenIndex = chosenIndex;
				this.FormationsWithLooselyChosenSergeants.Clear();
				List<Formation> list = base.Mission.PlayerTeam.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && !this.FormationsLockedWithSergeants.ContainsKey(f.Index)).ToList<Formation>();
				if (chosenIndex != -1)
				{
					Formation formation = list.FirstOrDefault<Formation>((Formation fr) => fr.Index == chosenIndex);
					this.FormationsWithLooselyChosenSergeants.Add(chosenIndex, base.Mission.PlayerTeam.PlayerOrderController.Owner);
					list.Remove(formation);
				}
				Queue<string> queue = new Queue<string>(this.CharacterNamesInPlayerSideByPriorityQueue);
				while (list.Count > 0 && queue.Count > 0)
				{
					string nextAgentNameToProcess = queue.Dequeue();
					Agent agent = base.Mission.PlayerTeam.ActiveAgents.FirstOrDefault<Agent>((Agent aa) => aa.Character.StringId.Equals(nextAgentNameToProcess));
					if (agent != null)
					{
						Formation formation2 = this.ChooseFormationToLead(list, agent);
						if (formation2 != null)
						{
							this.FormationsWithLooselyChosenSergeants.Add(formation2.Index, agent);
							list.Remove(formation2);
						}
					}
				}
				if (this.OnAllFormationsAssignedSergeants != null)
				{
					this.OnAllFormationsAssignedSergeants(this.FormationsWithLooselyChosenSergeants);
				}
			}
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0007EA04 File Offset: 0x0007CC04
		public void OnPlayerChoiceFinalized()
		{
			foreach (KeyValuePair<int, Agent> keyValuePair in this.FormationsLockedWithSergeants)
			{
				this.AssignSergeant(keyValuePair.Value.Team.GetFormation((FormationClass)keyValuePair.Key), keyValuePair.Value);
			}
			foreach (KeyValuePair<int, Agent> keyValuePair2 in this.FormationsWithLooselyChosenSergeants)
			{
				this.AssignSergeant(keyValuePair2.Value.Team.GetFormation((FormationClass)keyValuePair2.Key), keyValuePair2.Value);
			}
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0007EAD8 File Offset: 0x0007CCD8
		protected virtual void AssignSergeant(Formation formationToLead, Agent sergeant)
		{
			sergeant.Formation = formationToLead;
			if (!sergeant.IsAIControlled || sergeant == base.Mission.InitialPlayerAgent)
			{
				formationToLead.PlayerOwner = sergeant;
			}
			formationToLead.Captain = sergeant;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0007EB08 File Offset: 0x0007CD08
		private Formation ChooseFormationToLead(IEnumerable<Formation> formationsToChooseFrom, Agent agent)
		{
			bool hasMount = agent.HasMount;
			bool flag = agent.HasRangedWeapon(false);
			List<Formation> list = formationsToChooseFrom.ToList<Formation>();
			while (list.Count > 0)
			{
				Formation formation = list.MaxBy<Formation, float>((Formation ftcf) => ftcf.QuerySystem.FormationPower);
				list.Remove(formation);
				if ((flag || (!formation.QuerySystem.IsRangedFormation && !formation.QuerySystem.IsRangedCavalryFormation)) && (hasMount || (!formation.QuerySystem.IsCavalryFormation && !formation.QuerySystem.IsRangedCavalryFormation)))
				{
					return formation;
				}
			}
			return null;
		}

		// Token: 0x04000DA6 RID: 3494
		protected readonly List<string> CharactersInPlayerSideByPriority;

		// Token: 0x04000DA7 RID: 3495
		protected Queue<string> CharacterNamesInPlayerSideByPriorityQueue;

		// Token: 0x04000DA8 RID: 3496
		protected List<Formation> RemainingFormationsToAssignSergeantsTo;

		// Token: 0x04000DA9 RID: 3497
		protected Dictionary<int, Agent> FormationsLockedWithSergeants;

		// Token: 0x04000DAA RID: 3498
		protected Dictionary<int, Agent> FormationsWithLooselyChosenSergeants;
	}
}
