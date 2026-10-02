using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028B RID: 651
	public class GeneralsAndCaptainsAssignmentLogic : MissionLogic
	{
		// Token: 0x06002471 RID: 9329 RVA: 0x00083271 File Offset: 0x00081471
		public GeneralsAndCaptainsAssignmentLogic(TextObject attackerGeneralName, TextObject defenderGeneralName, TextObject attackerAllyGeneralName = null, TextObject defenderAllyGeneralName = null, bool createBodyguard = true)
		{
			this._attackerGeneralName = attackerGeneralName;
			this._defenderGeneralName = defenderGeneralName;
			this._attackerAllyGeneralName = attackerAllyGeneralName;
			this._defenderAllyGeneralName = defenderAllyGeneralName;
			this._createBodyguard = createBodyguard;
			this._isPlayerTeamGeneralFormationSet = false;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000832AC File Offset: 0x000814AC
		public override void AfterStart()
		{
			this._bannerLogic = base.Mission.GetMissionBehavior<BannerBearerLogic>();
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000832C0 File Offset: 0x000814C0
		public override void OnBattleSideSpawned(BattleSideEnum side)
		{
			foreach (Team team in Mission.GetTeamsOfSide(side))
			{
				this.SetGeneralAgentOfTeam(team);
				if (team.IsPlayerTeam)
				{
					if (!MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
					{
						if (this.CanTeamHaveGeneralsFormation(team))
						{
							this.CreateGeneralFormationForTeam(team);
							this._isPlayerTeamGeneralFormationSet = true;
						}
						this.AssignBestCaptainsForTeam(team);
					}
				}
				else
				{
					if (this.CanTeamHaveGeneralsFormation(team))
					{
						this.CreateGeneralFormationForTeam(team);
					}
					this.AssignBestCaptainsForTeam(team);
				}
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x00083360 File Offset: 0x00081560
		public override void OnDeploymentFinished()
		{
			Team playerTeam = base.Mission.PlayerTeam;
			if (!this._isPlayerTeamGeneralFormationSet && this.CanTeamHaveGeneralsFormation(playerTeam))
			{
				this.CreateGeneralFormationForTeam(playerTeam);
				this._isPlayerTeamGeneralFormationSet = true;
			}
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			if (this._isPlayerTeamGeneralFormationSet && playerTeam.GeneralAgent != initialPlayerAgent && !base.Mission.IsNavalBattle)
			{
				initialPlayerAgent.SetCanLeadFormationsRemotely(true);
				Formation formation = playerTeam.GetFormation(FormationClass.NumberOfRegularFormations);
				initialPlayerAgent.Formation = formation;
				initialPlayerAgent.Team.TriggerOnFormationsChanged(formation);
				formation.QuerySystem.Expire();
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000833F0 File Offset: 0x000815F0
		protected virtual void SortCaptainsByPriority(Team team, ref List<Agent> captains)
		{
			captains = captains.OrderByDescending<Agent, float>(delegate(Agent captain)
			{
				if (team.GeneralAgent != captain)
				{
					return captain.Character.GetPower();
				}
				return float.MaxValue;
			}).ToList<Agent>();
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x00083424 File Offset: 0x00081624
		protected virtual Formation PickBestRegularFormationToLead(Agent agent, List<Formation> candidateFormations)
		{
			Formation formation = null;
			int num = 0;
			foreach (Formation formation2 in candidateFormations)
			{
				if (!(agent.HasMount ^ formation2.CalculateHasSignificantNumberOfMounted))
				{
					int countOfUnits = formation2.CountOfUnits;
					if (countOfUnits > num)
					{
						num = countOfUnits;
						formation = formation2;
					}
				}
			}
			return formation;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00083494 File Offset: 0x00081694
		private bool CanTeamHaveGeneralsFormation(Team team)
		{
			if (base.Mission.IsNavalBattle)
			{
				return false;
			}
			Agent generalAgent = team.GeneralAgent;
			return generalAgent != null && (generalAgent == base.Mission.InitialPlayerAgent || team.QuerySystem.MemberCount >= 50);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000834E0 File Offset: 0x000816E0
		private void AssignBestCaptainsForTeam(Team team)
		{
			List<Agent> list = team.ActiveAgents.Where<Agent>((Agent agent) => agent.IsHero).ToList<Agent>();
			this.SortCaptainsByPriority(team, ref list);
			int numRegularFormations = 8;
			List<Formation> list2 = team.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.FormationIndex < (FormationClass)numRegularFormations).ToList<Formation>();
			List<Agent> list3 = new List<Agent>();
			foreach (Agent agent3 in list)
			{
				Formation formation = null;
				BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
				if (agent3 == team.GeneralAgent && team.BodyGuardFormation != null && team.BodyGuardFormation.CountOfUnits > 0)
				{
					formation = team.BodyGuardFormation;
				}
				if (formation == null)
				{
					formation = this.PickBestRegularFormationToLead(agent3, list2);
					if (formation != null)
					{
						list2.Remove(formation);
					}
				}
				if (formation != null)
				{
					list3.Add(agent3);
					this.OnCaptainAssignedToFormation(agent3, formation);
				}
			}
			foreach (Agent agent2 in list3)
			{
				list.Remove(agent2);
			}
			using (List<Agent>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Agent candidate = enumerator.Current;
					if (list2.IsEmpty<Formation>())
					{
						break;
					}
					Formation formation2 = list2.FirstOrDefault<Formation>((Formation f) => f.CalculateHasSignificantNumberOfMounted == candidate.HasMount);
					if (formation2 != null)
					{
						this.OnCaptainAssignedToFormation(candidate, formation2);
						list2.Remove(formation2);
					}
				}
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000836BC File Offset: 0x000818BC
		private void SetGeneralAgentOfTeam(Team team)
		{
			Agent agent = null;
			if (team.IsPlayerTeam && team.IsPlayerGeneral)
			{
				agent = base.Mission.InitialPlayerAgent;
			}
			else
			{
				List<IFormationUnit> list = team.FormationsIncludingEmpty.SelectMany<Formation, IFormationUnit>((Formation f) => f.UnitsWithoutLooseDetachedOnes).ToList<IFormationUnit>();
				TextObject generalName = ((team == base.Mission.AttackerTeam) ? this._attackerGeneralName : ((team == base.Mission.DefenderTeam) ? this._defenderGeneralName : ((team == base.Mission.AttackerAllyTeam) ? this._attackerAllyGeneralName : ((team == base.Mission.DefenderAllyTeam) ? this._defenderAllyGeneralName : null))));
				if (generalName != null && list.Count<IFormationUnit>((IFormationUnit ta) => ((Agent)ta).Character != null && ((Agent)ta).Character.GetName().Equals(generalName)) == 1)
				{
					agent = (Agent)list.First<IFormationUnit>((IFormationUnit ta) => ((Agent)ta).Character != null && ((Agent)ta).Character.GetName().Equals(generalName));
				}
				else if (list.Any<IFormationUnit>((IFormationUnit u) => !((Agent)u).IsMainAgent && ((Agent)u).IsHero))
				{
					agent = (Agent)list.Where<IFormationUnit>((IFormationUnit u) => !((Agent)u).IsMainAgent && ((Agent)u).IsHero).MaxBy<IFormationUnit, float>((IFormationUnit u) => ((Agent)u).CharacterPowerCached);
				}
			}
			if (agent != null && !base.Mission.IsNavalBattle)
			{
				agent.SetCanLeadFormationsRemotely(true);
			}
			team.GeneralAgent = agent;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00083854 File Offset: 0x00081A54
		private void CreateGeneralFormationForTeam(Team team)
		{
			Agent generalAgent = team.GeneralAgent;
			Formation formation = team.GetFormation(FormationClass.NumberOfRegularFormations);
			base.Mission.SetFormationPositioningFromDeploymentPlan(formation, false);
			WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
			formation.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
			formation.SetControlledByAI(true, false);
			team.GeneralsFormation = formation;
			generalAgent.Formation = formation;
			generalAgent.Team.TriggerOnFormationsChanged(formation);
			formation.QuerySystem.Expire();
			TacticComponent.SetDefaultBehaviorWeights(formation);
			formation.AI.SetBehaviorWeight<BehaviorGeneral>(1f);
			formation.PlayerOwner = null;
			if (this._createBodyguard && generalAgent != base.Mission.InitialPlayerAgent)
			{
				List<IFormationUnit> list = team.FormationsIncludingEmpty.SelectMany<Formation, IFormationUnit>((Formation f) => f.UnitsWithoutLooseDetachedOnes).ToList<IFormationUnit>();
				list.Remove(generalAgent);
				List<IFormationUnit> list2 = list.Where<IFormationUnit>(delegate(IFormationUnit u)
				{
					Agent agent;
					if ((agent = u as Agent) == null || (agent.Character != null && agent.Character.IsHero) || agent.Banner != null)
					{
						return false;
					}
					if (generalAgent.MountAgent == null)
					{
						return !agent.HasMount;
					}
					return agent.HasMount;
				}).ToList<IFormationUnit>();
				int num = MathF.Min((int)((float)list2.Count / 10f), 20);
				if (num != 0)
				{
					Formation formation2 = team.GetFormation(FormationClass.Bodyguard);
					formation2.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
					formation2.SetControlledByAI(true, false);
					List<IFormationUnit> list3 = list2.OrderByDescending<IFormationUnit, float>((IFormationUnit u) => ((Agent)u).CharacterPowerCached).Take<IFormationUnit>(num).ToList<IFormationUnit>();
					IEnumerable<Formation> enumerable = list3.Select<IFormationUnit, Formation>((IFormationUnit bu) => ((Agent)bu).Formation).Distinct<Formation>();
					foreach (IFormationUnit formationUnit in list3)
					{
						((Agent)formationUnit).Formation = formation2;
					}
					foreach (Formation formation3 in enumerable)
					{
						team.TriggerOnFormationsChanged(formation3);
						formation3.QuerySystem.Expire();
					}
					TacticComponent.SetDefaultBehaviorWeights(formation2);
					formation2.AI.SetBehaviorWeight<BehaviorProtectGeneral>(1f);
					formation2.PlayerOwner = null;
					formation2.QuerySystem.Expire();
					team.BodyGuardFormation = formation2;
					team.TriggerOnFormationsChanged(formation2);
				}
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00083AD0 File Offset: 0x00081CD0
		private void OnCaptainAssignedToFormation(Agent captain, Formation formation)
		{
			if (captain.Formation != formation && captain != formation.Team.GeneralAgent)
			{
				captain.Formation = formation;
				formation.Team.TriggerOnFormationsChanged(formation);
				formation.QuerySystem.Expire();
			}
			formation.Captain = captain;
			if (this._bannerLogic != null && captain.FormationBanner != null)
			{
				this._bannerLogic.SetFormationBanner(formation, captain.FormationBanner);
			}
		}

		// Token: 0x04000DFB RID: 3579
		public int MinimumAgentCountToLeadGeneralFormation = 3;

		// Token: 0x04000DFC RID: 3580
		private BannerBearerLogic _bannerLogic;

		// Token: 0x04000DFD RID: 3581
		private readonly TextObject _attackerGeneralName;

		// Token: 0x04000DFE RID: 3582
		private readonly TextObject _defenderGeneralName;

		// Token: 0x04000DFF RID: 3583
		private readonly TextObject _attackerAllyGeneralName;

		// Token: 0x04000E00 RID: 3584
		private readonly TextObject _defenderAllyGeneralName;

		// Token: 0x04000E01 RID: 3585
		private readonly bool _createBodyguard;

		// Token: 0x04000E02 RID: 3586
		private bool _isPlayerTeamGeneralFormationSet;
	}
}
