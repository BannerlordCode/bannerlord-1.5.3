using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000396 RID: 918
	public class Team : IMissionTeam
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060034D7 RID: 13527 RVA: 0x000DA6F8 File Offset: 0x000D88F8
		// (remove) Token: 0x060034D8 RID: 13528 RVA: 0x000DA730 File Offset: 0x000D8930
		public event Action<Team, Formation> OnFormationsChanged;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x060034D9 RID: 13529 RVA: 0x000DA768 File Offset: 0x000D8968
		// (remove) Token: 0x060034DA RID: 13530 RVA: 0x000DA7A0 File Offset: 0x000D89A0
		public event OnOrderIssuedDelegate OnOrderIssued;

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x060034DB RID: 13531 RVA: 0x000DA7D8 File Offset: 0x000D89D8
		// (remove) Token: 0x060034DC RID: 13532 RVA: 0x000DA810 File Offset: 0x000D8A10
		public event Action<Formation> OnFormationAIActiveBehaviorChanged;

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x060034DD RID: 13533 RVA: 0x000DA848 File Offset: 0x000D8A48
		// (remove) Token: 0x060034DE RID: 13534 RVA: 0x000DA880 File Offset: 0x000D8A80
		public event Action<Team> OnFormationsChangedInDeployment;

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060034DF RID: 13535 RVA: 0x000DA8B5 File Offset: 0x000D8AB5
		public BattleSideEnum Side { get; }

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060034E0 RID: 13536 RVA: 0x000DA8BD File Offset: 0x000D8ABD
		public Mission Mission { get; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060034E1 RID: 13537 RVA: 0x000DA8C5 File Offset: 0x000D8AC5
		// (set) Token: 0x060034E2 RID: 13538 RVA: 0x000DA8CD File Offset: 0x000D8ACD
		public MBList<Formation> FormationsIncludingEmpty { get; private set; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060034E3 RID: 13539 RVA: 0x000DA8D6 File Offset: 0x000D8AD6
		// (set) Token: 0x060034E4 RID: 13540 RVA: 0x000DA8DE File Offset: 0x000D8ADE
		public MBList<Formation> FormationsIncludingSpecialAndEmpty { get; private set; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060034E5 RID: 13541 RVA: 0x000DA8E7 File Offset: 0x000D8AE7
		// (set) Token: 0x060034E6 RID: 13542 RVA: 0x000DA8EF File Offset: 0x000D8AEF
		public TeamAIComponent TeamAI { get; private set; }

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060034E7 RID: 13543 RVA: 0x000DA8F8 File Offset: 0x000D8AF8
		public bool IsPlayerTeam
		{
			get
			{
				return this.Mission != null && this.Mission.PlayerTeam == this;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060034E8 RID: 13544 RVA: 0x000DA912 File Offset: 0x000D8B12
		public bool IsPlayerAlly
		{
			get
			{
				return this.Mission != null && this.Mission.PlayerTeam != null && this.Mission.PlayerTeam.Side == this.Side;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060034E9 RID: 13545 RVA: 0x000DA943 File Offset: 0x000D8B43
		public bool IsPlayerEnemy
		{
			get
			{
				return this.Mission != null && this.Mission.PlayerTeam != null && this.Mission.PlayerTeam.Side != this.Side;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060034EA RID: 13546 RVA: 0x000DA977 File Offset: 0x000D8B77
		public TeamSideEnum TeamSide
		{
			get
			{
				if (this.IsPlayerTeam)
				{
					return TeamSideEnum.PlayerTeam;
				}
				if (!this.IsPlayerAlly)
				{
					return TeamSideEnum.EnemyTeam;
				}
				return TeamSideEnum.PlayerAllyTeam;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060034EB RID: 13547 RVA: 0x000DA98E File Offset: 0x000D8B8E
		public bool IsDefender
		{
			get
			{
				return this.Side == BattleSideEnum.Defender;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x000DA999 File Offset: 0x000D8B99
		public bool IsAttacker
		{
			get
			{
				return this.Side == BattleSideEnum.Attacker;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060034ED RID: 13549 RVA: 0x000DA9A4 File Offset: 0x000D8BA4
		// (set) Token: 0x060034EE RID: 13550 RVA: 0x000DA9AC File Offset: 0x000D8BAC
		public uint Color { get; private set; }

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060034EF RID: 13551 RVA: 0x000DA9B5 File Offset: 0x000D8BB5
		// (set) Token: 0x060034F0 RID: 13552 RVA: 0x000DA9BD File Offset: 0x000D8BBD
		public uint Color2 { get; private set; }

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060034F1 RID: 13553 RVA: 0x000DA9C6 File Offset: 0x000D8BC6
		public Banner Banner { get; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x000DA9CE File Offset: 0x000D8BCE
		public OrderController MasterOrderController
		{
			get
			{
				return this._orderControllers[0];
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060034F3 RID: 13555 RVA: 0x000DA9DC File Offset: 0x000D8BDC
		public OrderController PlayerOrderController
		{
			get
			{
				return this._orderControllers[1];
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060034F4 RID: 13556 RVA: 0x000DA9EA File Offset: 0x000D8BEA
		// (set) Token: 0x060034F5 RID: 13557 RVA: 0x000DA9F2 File Offset: 0x000D8BF2
		public TeamQuerySystem QuerySystem { get; private set; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060034F6 RID: 13558 RVA: 0x000DA9FB File Offset: 0x000D8BFB
		// (set) Token: 0x060034F7 RID: 13559 RVA: 0x000DAA03 File Offset: 0x000D8C03
		public DetachmentManager DetachmentManager { get; private set; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x000DAA0C File Offset: 0x000D8C0C
		// (set) Token: 0x060034F9 RID: 13561 RVA: 0x000DAA14 File Offset: 0x000D8C14
		public bool IsPlayerGeneral { get; private set; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x000DAA1D File Offset: 0x000D8C1D
		// (set) Token: 0x060034FB RID: 13563 RVA: 0x000DAA25 File Offset: 0x000D8C25
		public bool IsPlayerSergeant { get; private set; }

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060034FC RID: 13564 RVA: 0x000DAA2E File Offset: 0x000D8C2E
		public MBReadOnlyList<Agent> ActiveAgents
		{
			get
			{
				return this._activeAgents;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060034FD RID: 13565 RVA: 0x000DAA36 File Offset: 0x000D8C36
		public MBReadOnlyList<Agent> TeamAgents
		{
			get
			{
				return this._teamAgents;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x000DAA3E File Offset: 0x000D8C3E
		public MBReadOnlyList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>> CachedEnemyDataForFleeing
		{
			get
			{
				return this._cachedEnemyDataForFleeing;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x000DAA46 File Offset: 0x000D8C46
		public int TeamIndex
		{
			get
			{
				return this.MBTeam.Index;
			}
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000DAA54 File Offset: 0x000D8C54
		public Team(MBTeam mbTeam, BattleSideEnum side, Mission mission, uint color = 4294967295U, uint color2 = 4294967295U, Banner banner = null)
		{
			this.MBTeam = mbTeam;
			this.Side = side;
			this.Mission = mission;
			this.Color = color;
			this.Color2 = color2;
			this.Banner = banner;
			this.IsPlayerGeneral = true;
			this.IsPlayerSergeant = false;
			if (this != Team._invalid)
			{
				this.Initialize();
			}
			this.MoraleChangeFactor = 1f;
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000DAAC4 File Offset: 0x000D8CC4
		public void SetCustomOrderController(OrderController customMasterOrderController, OrderController customPlayerOrderController)
		{
			this._alreadyHasCustomOrderController = true;
			OrderController orderController = ((this._orderControllers.Count > 0) ? this._orderControllers[0] : null);
			object obj = ((this._orderControllers.Count > 1) ? this._orderControllers[1] : null);
			this._orderControllers.Clear();
			this._orderControllers.Add(customMasterOrderController);
			this._orderControllers.Add(customPlayerOrderController);
			if (orderController != null)
			{
				orderController.AssignDelegatesToController(customMasterOrderController);
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.AssignDelegatesToController(customPlayerOrderController);
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000DAB4C File Offset: 0x000D8D4C
		public void UpdateCachedEnemyDataForFleeing()
		{
			if (this._cachedEnemyDataForFleeing.IsEmpty<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>>())
			{
				foreach (Team team in this.Mission.Teams)
				{
					if (team.IsEnemyOf(this))
					{
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							int countOfUnits = formation.CountOfUnits;
							if (countOfUnits > 0)
							{
								WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
								float movementSpeedMaximum = formation.QuerySystem.MovementSpeedMaximum;
								bool flag = (formation.QuerySystem.IsCavalryFormation || formation.QuerySystem.IsRangedCavalryFormation) && formation.HasAnyMountedUnit;
								if (countOfUnits == 1)
								{
									Vec2 asVec = cachedMedianPosition.AsVec2;
									this._cachedEnemyDataForFleeing.Add(new ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>(movementSpeedMaximum, cachedMedianPosition, countOfUnits, asVec, asVec, flag));
								}
								else
								{
									Vec2 vec = formation.QuerySystem.EstimatedDirection.LeftVec();
									float num = formation.Width / 2f;
									Vec2 vec2 = cachedMedianPosition.AsVec2 - vec * num;
									Vec2 vec3 = cachedMedianPosition.AsVec2 + vec * num;
									this._cachedEnemyDataForFleeing.Add(new ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>(movementSpeedMaximum, cachedMedianPosition, countOfUnits, vec2, vec3, flag));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003503 RID: 13571 RVA: 0x000DACFC File Offset: 0x000D8EFC
		// (set) Token: 0x06003504 RID: 13572 RVA: 0x000DAD04 File Offset: 0x000D8F04
		public float MoraleChangeFactor { get; private set; }

		// Token: 0x06003505 RID: 13573 RVA: 0x000DAD10 File Offset: 0x000D8F10
		private void Initialize()
		{
			this._activeAgents = new MBList<Agent>();
			this._teamAgents = new MBList<Agent>();
			this._cachedEnemyDataForFleeing = new MBList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>>();
			if (!GameNetwork.IsReplay)
			{
				this.FormationsIncludingSpecialAndEmpty = new MBList<Formation>(10);
				this.FormationsIncludingEmpty = new MBList<Formation>(8);
				for (int i = 0; i < 10; i++)
				{
					Formation formation = new Formation(this, i);
					this.FormationsIncludingSpecialAndEmpty.Add(formation);
					if (i < 8)
					{
						this.FormationsIncludingEmpty.Add(formation);
					}
					formation.AI.OnActiveBehaviorChanged += this.FormationAI_OnActiveBehaviorChanged;
				}
				if (this.Mission != null)
				{
					this._orderControllers = new List<OrderController>();
					OrderController orderController = new OrderController(this.Mission, this, null);
					this._orderControllers.Add(orderController);
					orderController.OnOrderIssued += this.OrderController_OnOrderIssued;
					OrderController orderController2 = new OrderController(this.Mission, this, null);
					this._orderControllers.Add(orderController2);
					orderController2.OnOrderIssued += this.OrderController_OnOrderIssued;
				}
				this.QuerySystem = new TeamQuerySystem(this);
				this.DetachmentManager = new DetachmentManager(this);
			}
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000DAE2C File Offset: 0x000D902C
		public void Reset()
		{
			if (!GameNetwork.IsReplay)
			{
				foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
				{
					formation.Reset();
				}
				List<OrderController> orderControllers = this._orderControllers;
				if (orderControllers != null && orderControllers.Count > 2)
				{
					for (int i = this._orderControllers.Count - 1; i >= 2; i--)
					{
						this._orderControllers[i].OnOrderIssued -= this.OrderController_OnOrderIssued;
						this._orderControllers.RemoveAt(i);
					}
				}
				this.QuerySystem = new TeamQuerySystem(this);
			}
			this._teamAgents.Clear();
			this._activeAgents.Clear();
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000DAF00 File Offset: 0x000D9100
		public void Clear()
		{
			if (!GameNetwork.IsReplay)
			{
				foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
				{
					formation.AI.OnActiveBehaviorChanged -= this.FormationAI_OnActiveBehaviorChanged;
				}
			}
			this.Reset();
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000DAF70 File Offset: 0x000D9170
		private void OrderController_OnOrderIssued(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			OnOrderIssuedDelegate onOrderIssued = this.OnOrderIssued;
			if (onOrderIssued == null)
			{
				return;
			}
			onOrderIssued(orderType, appliedFormations, orderController, delegateParams);
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000DAF87 File Offset: 0x000D9187
		public static bool DoesFirstFormationClassContainSecond(FormationClass f1, FormationClass f2)
		{
			return (f1 & f2) == f2;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000DAF8F File Offset: 0x000D918F
		public static FormationClass GetFormationFormationClass(Formation f)
		{
			if (f.QuerySystem.IsRangedCavalryFormation)
			{
				return FormationClass.HorseArcher;
			}
			if (f.QuerySystem.IsCavalryFormation)
			{
				return FormationClass.Cavalry;
			}
			if (!f.QuerySystem.IsRangedFormation)
			{
				return FormationClass.Infantry;
			}
			return FormationClass.Ranged;
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000DAFBF File Offset: 0x000D91BF
		public static FormationClass GetPlayerTeamFormationClass(Agent mainAgent)
		{
			if (mainAgent.IsRangedCached && mainAgent.HasMount)
			{
				return FormationClass.HorseArcher;
			}
			if (mainAgent.IsRangedCached)
			{
				return FormationClass.Ranged;
			}
			if (!mainAgent.HasMount)
			{
				return FormationClass.Infantry;
			}
			return FormationClass.Cavalry;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000DAFE8 File Offset: 0x000D91E8
		public void AssignPlayerAsSergeantOfFormation(MissionPeer peer, FormationClass formationClass)
		{
			Formation formation = this.GetFormation(formationClass);
			formation.PlayerOwner = peer.ControlledAgent;
			formation.BannerCode = peer.Peer.BannerCode;
			if (peer.IsMine)
			{
				this.PlayerOrderController.Owner = peer.ControlledAgent;
			}
			else
			{
				this.GetOrderControllerOf(peer.ControlledAgent).Owner = peer.ControlledAgent;
			}
			formation.SetControlledByAI(false, false);
			foreach (MissionBehavior missionBehavior in this.Mission.MissionBehaviors)
			{
				missionBehavior.OnAssignPlayerAsSergeantOfFormation(peer.ControlledAgent);
			}
			if (peer.IsMine)
			{
				this.PlayerOrderController.SelectAllFormations(false);
			}
			peer.ControlledFormation = formation;
			if (GameNetwork.IsServer)
			{
				peer.ControlledAgent.ForceUpdateCachedAndFormationValues(false, false);
				if (!peer.IsMine)
				{
					GameNetwork.BeginModuleEventAsServer(peer.GetNetworkPeer());
					GameNetwork.WriteMessage(new AssignFormationToPlayer(peer.GetNetworkPeer(), formationClass));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000DB0FC File Offset: 0x000D92FC
		private void FormationAI_OnActiveBehaviorChanged(Formation formation)
		{
			if (formation.CountOfUnits > 0)
			{
				Action<Formation> onFormationAIActiveBehaviorChanged = this.OnFormationAIActiveBehaviorChanged;
				if (onFormationAIActiveBehaviorChanged == null)
				{
					return;
				}
				onFormationAIActiveBehaviorChanged(formation);
			}
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000DB118 File Offset: 0x000D9318
		public void AddTacticOption(TacticComponent tacticOption)
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.AddTacticOption(tacticOption);
			}
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000DB12E File Offset: 0x000D932E
		public void RemoveTacticOption(Type tacticType)
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.RemoveTacticOption(tacticType);
			}
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000DB144 File Offset: 0x000D9344
		public void ClearTacticOptions()
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.ClearTacticOptions();
			}
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x000DB159 File Offset: 0x000D9359
		public void ResetTactic()
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.ResetTactic(true);
			}
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000DB170 File Offset: 0x000D9370
		public void AddTeamAI(TeamAIComponent teamAI, bool forceNotAIControlled = false)
		{
			this.TeamAI = teamAI;
			foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetControlledByAI(!forceNotAIControlled && (this != this.Mission.PlayerTeam || !this.IsPlayerGeneral), false);
			}
			this.TeamAI.InitializeDetachments(this.Mission);
			this.TeamAI.CreateMissionSpecificBehaviors();
			this.TeamAI.ResetTactic(true);
			foreach (Formation formation2 in this.FormationsIncludingSpecialAndEmpty)
			{
				if (formation2.CountOfUnits > 0)
				{
					formation2.AI.Tick();
				}
			}
			this.TeamAI.TickOccasionally();
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000DB26C File Offset: 0x000D946C
		public void DelegateCommandToAI()
		{
			foreach (Formation formation in this.FormationsIncludingEmpty)
			{
				formation.SetControlledByAI(true, false);
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000DB2C0 File Offset: 0x000D94C0
		public void RearrangeFormationsAccordingToFilter([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> MassTransferData)
		{
			List<Formation> list = new List<Formation>();
			foreach (ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> valueTuple in MassTransferData)
			{
				valueTuple.Item1.OnMassUnitTransferStart();
				if (valueTuple.Item1.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderStop && (valueTuple.Item1.CountOfUnits > 0 || valueTuple.Item2 > 0))
				{
					list.Add(valueTuple.Item1);
					valueTuple.Item1.SetMovementOrder(MovementOrder.MovementOrderMove(valueTuple.Item1.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None)));
				}
			}
			List<Agent>[] array = new List<Agent>[MassTransferData.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<Agent>();
			}
			List<FormationPocket> list2 = new List<FormationPocket>();
			for (int j = 0; j < MassTransferData.Count; j++)
			{
				TroopTraitsMask item = MassTransferData[j].Item3;
				Func<Agent, int> func;
				TroopFilteringUtilities.GetPriorityFunction(item, out func);
				int maxPriority = TroopFilteringUtilities.GetMaxPriority(item);
				list2.Add(new FormationPocket(func, maxPriority, MassTransferData[j].Item2, j));
			}
			list2.RemoveAll((FormationPocket pfamv) => pfamv.TroopCount <= 0);
			list2 = list2.OrderBy<FormationPocket, int>((FormationPocket pfamv) => pfamv.TroopCount).ToList<FormationPocket>();
			list2 = list2.OrderByDescending<FormationPocket, int>((FormationPocket pfamv) => pfamv.ScoreToSeek).ToList<FormationPocket>();
			List<IFormationUnit> list3 = new List<IFormationUnit>();
			list3 = MassTransferData.SelectMany<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>, IFormationUnit>(([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> mtd) => mtd.Item1.DetachedUnits.Concat<IFormationUnit>(mtd.Item1.Arrangement.GetAllUnits()).Except<IFormationUnit>(mtd.Item4)).ToList<IFormationUnit>();
			int num = MassTransferData.Sum<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>(([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> mtd) => mtd.Item4.Count);
			int k = MassTransferData.Sum<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>(([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> mtd) => mtd.Item1.CountOfUnits) - num;
			int num2 = list2[0].ScoreToSeek;
			while (k > 0)
			{
				for (int l = 0; l < k; l++)
				{
					Agent agent = list3[l] as Agent;
					for (int m = 0; m < list2.Count; m++)
					{
						FormationPocket formationPocket = list2[m];
						int num3 = formationPocket.PriorityFunction(agent);
						if (num2 <= formationPocket.ScoreToSeek && num3 >= num2)
						{
							array[formationPocket.Index].Add(agent);
							formationPocket.AddTroop();
							if (formationPocket.IsFormationPocketFilled())
							{
								list2.RemoveAt(m);
							}
							k--;
							list3[l] = list3[k];
							l--;
							break;
						}
						if (num3 > formationPocket.BestScoreSoFar)
						{
							formationPocket.SetBestScoreSoFar(num3);
						}
					}
				}
				if (list2.Count == 0)
				{
					break;
				}
				for (int n = 0; n < list2.Count; n++)
				{
					list2[n].UpdateScoreToSeek();
				}
				list2.OrderByDescending<FormationPocket, int>((FormationPocket pfamv) => pfamv.ScoreToSeek);
				num2 = list2[0].ScoreToSeek;
			}
			for (int num4 = 0; num4 < array.Length; num4++)
			{
				foreach (Agent agent2 in array[num4])
				{
					agent2.Formation = MassTransferData[num4].Item1;
				}
			}
			foreach (ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> valueTuple2 in MassTransferData)
			{
				this.TriggerOnFormationsChanged(valueTuple2.Item1);
				valueTuple2.Item1.OnMassUnitTransferEnd();
				if (valueTuple2.Item1.CountOfUnits > 0 && !valueTuple2.Item1.OrderPositionIsValid)
				{
					Vec2 averagePositionOfUnits = valueTuple2.Item1.GetAveragePositionOfUnits(false, false);
					float terrainHeight = this.Mission.Scene.GetTerrainHeight(averagePositionOfUnits, true);
					this.Mission.Scene.GetHeightAtPoint(averagePositionOfUnits, BodyFlags.None, ref terrainHeight);
					Vec3 vec = new Vec3(averagePositionOfUnits, terrainHeight, -1f);
					WorldPosition worldPosition = new WorldPosition(this.Mission.Scene, UIntPtr.Zero, vec, false);
					valueTuple2.Item1.SetPositioning(new WorldPosition?(worldPosition), null, null);
				}
			}
			foreach (Formation formation in list)
			{
				formation.SetMovementOrder(MovementOrder.MovementOrderStop);
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000DB7D4 File Offset: 0x000D99D4
		// (set) Token: 0x06003516 RID: 13590 RVA: 0x000DB7DC File Offset: 0x000D99DC
		public Formation GeneralsFormation { get; set; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x000DB7E5 File Offset: 0x000D99E5
		// (set) Token: 0x06003518 RID: 13592 RVA: 0x000DB7ED File Offset: 0x000D99ED
		public Formation BodyGuardFormation { get; set; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000DB7F6 File Offset: 0x000D99F6
		// (set) Token: 0x0600351A RID: 13594 RVA: 0x000DB7FE File Offset: 0x000D99FE
		public Agent GeneralAgent { get; set; }

		// Token: 0x0600351B RID: 13595 RVA: 0x000DB808 File Offset: 0x000D9A08
		public void Tick(float dt)
		{
			if (!this._cachedEnemyDataForFleeing.IsEmpty<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>>())
			{
				this._cachedEnemyDataForFleeing.Clear();
			}
			if (this.Mission.AllowAiTicking)
			{
				if (this.Mission.RetreatSide != BattleSideEnum.None && this.Side == this.Mission.RetreatSide)
				{
					using (List<Formation>.Enumerator enumerator = this.FormationsIncludingSpecialAndEmpty.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Formation formation = enumerator.Current;
							if (formation.CountOfUnits > 0)
							{
								formation.SetMovementOrder(MovementOrder.MovementOrderRetreat);
							}
						}
						goto IL_00A8;
					}
				}
				if (this.TeamAI != null && this.HasBots)
				{
					this.TeamAI.Tick(dt);
				}
			}
			IL_00A8:
			if (!GameNetwork.IsReplay)
			{
				if (this._tickDetachments)
				{
					this.DetachmentManager.TickDetachments();
				}
				foreach (Formation formation2 in this.FormationsIncludingSpecialAndEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						formation2.Tick(dt);
					}
				}
			}
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000DB934 File Offset: 0x000D9B34
		public Formation GetFormation(FormationClass formationIndex)
		{
			return this.FormationsIncludingSpecialAndEmpty[(int)formationIndex];
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000DB944 File Offset: 0x000D9B44
		public void SetIsEnemyOf(Team otherTeam, bool isEnemyOf)
		{
			this.MBTeam.SetIsEnemyOf(otherTeam.MBTeam, isEnemyOf);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new TeamSetIsEnemyOf(this.TeamIndex, otherTeam.TeamIndex, isEnemyOf));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000DB994 File Offset: 0x000D9B94
		public bool IsEnemyOf(Team otherTeam)
		{
			return this.MBTeam.IsEnemyOf(otherTeam.MBTeam);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x000DB9B8 File Offset: 0x000D9BB8
		public bool IsFriendOf(Team otherTeam)
		{
			return this == otherTeam || !this.MBTeam.IsEnemyOf(otherTeam.MBTeam);
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003520 RID: 13600 RVA: 0x000DB9E2 File Offset: 0x000D9BE2
		public IEnumerable<Agent> Heroes
		{
			get
			{
				Agent main = Agent.Main;
				if (main != null && main.Team == this)
				{
					yield return main;
				}
				yield break;
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x000DB9F2 File Offset: 0x000D9BF2
		public void AddAgentToTeam(Agent unit)
		{
			this._teamAgents.Add(unit);
			this._activeAgents.Add(unit);
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000DBA0C File Offset: 0x000D9C0C
		public void RemoveAgentFromTeam(Agent unit)
		{
			this._teamAgents.Remove(unit);
			this._activeAgents.Remove(unit);
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000DBA28 File Offset: 0x000D9C28
		public void DeactivateAgent(Agent agent)
		{
			this._activeAgents.Remove(agent);
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000DBA38 File Offset: 0x000D9C38
		public void OnAgentRemoved(Agent agent)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
				{
					formation.AI.OnAgentRemoved(agent);
				}
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003525 RID: 13605 RVA: 0x000DBA98 File Offset: 0x000D9C98
		public bool HasBots
		{
			get
			{
				foreach (Agent agent in this.ActiveAgents)
				{
					if (!agent.IsMount && !agent.IsPlayerControlled)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003526 RID: 13606 RVA: 0x000DBAFC File Offset: 0x000D9CFC
		public Agent Leader
		{
			get
			{
				if (Agent.Main != null && Agent.Main.Team == this)
				{
					return Agent.Main;
				}
				Agent agent = null;
				foreach (Agent agent2 in this.ActiveAgents)
				{
					if (agent == null || agent2.IsHero)
					{
						agent = agent2;
						if (agent.IsHero)
						{
							break;
						}
					}
				}
				return agent;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06003527 RID: 13607 RVA: 0x000DBB7C File Offset: 0x000D9D7C
		// (set) Token: 0x06003528 RID: 13608 RVA: 0x000DBB9C File Offset: 0x000D9D9C
		public static Team Invalid
		{
			get
			{
				Team team;
				if ((team = Team._invalid) == null)
				{
					team = (Team._invalid = new Team(MBTeam.InvalidTeam, BattleSideEnum.None, null, uint.MaxValue, uint.MaxValue, null));
				}
				return team;
			}
			internal set
			{
				Team._invalid = value;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06003529 RID: 13609 RVA: 0x000DBBA4 File Offset: 0x000D9DA4
		public bool IsValid
		{
			get
			{
				return this.MBTeam.IsValid;
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000DBBC0 File Offset: 0x000D9DC0
		public override string ToString()
		{
			return this.MBTeam.ToString();
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x0600352B RID: 13611 RVA: 0x000DBBE1 File Offset: 0x000D9DE1
		public bool HasTeamAi
		{
			get
			{
				return this.TeamAI != null;
			}
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x000DBBEC File Offset: 0x000D9DEC
		public void OnMissionEnded()
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.OnMissionEnded();
			}
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000DBC01 File Offset: 0x000D9E01
		public void TriggerOnFormationsChanged(Formation formation)
		{
			Action<Team, Formation> onFormationsChanged = this.OnFormationsChanged;
			if (onFormationsChanged == null)
			{
				return;
			}
			onFormationsChanged(this, formation);
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000DBC15 File Offset: 0x000D9E15
		public void TriggerOnFormationsChangedInDeployment()
		{
			Action<Team> onFormationsChangedInDeployment = this.OnFormationsChangedInDeployment;
			if (onFormationsChangedInDeployment == null)
			{
				return;
			}
			onFormationsChangedInDeployment(this);
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000DBC28 File Offset: 0x000D9E28
		public OrderController GetOrderControllerOf(Agent agent)
		{
			OrderController orderController = this._orderControllers.FirstOrDefault<OrderController>((OrderController oc) => oc.Owner == agent);
			if (orderController == null)
			{
				orderController = new OrderController(this.Mission, this, agent);
				this._orderControllers.Add(orderController);
				orderController.OnOrderIssued += this.OrderController_OnOrderIssued;
			}
			return orderController;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000DBC90 File Offset: 0x000D9E90
		public void SetPlayerRole(bool isPlayerGeneral, bool isPlayerSergeant)
		{
			this.IsPlayerGeneral = isPlayerGeneral;
			this.IsPlayerSergeant = isPlayerSergeant;
			foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetControlledByAI(this != this.Mission.PlayerTeam || !this.IsPlayerGeneral, false);
			}
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000DBD0C File Offset: 0x000D9F0C
		public bool HasAnyEnemyTeamsWithAgents(bool ignoreMountedAgents)
		{
			foreach (Team team in this.Mission.Teams)
			{
				if (team != this && team.IsEnemyOf(this) && team.ActiveAgents.Count > 0)
				{
					if (ignoreMountedAgents)
					{
						using (List<Agent>.Enumerator enumerator2 = team.ActiveAgents.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (!enumerator2.Current.HasMount)
								{
									return true;
								}
							}
							continue;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000DBDC8 File Offset: 0x000D9FC8
		public bool HasAnyFormationsIncludingSpecialThatIsNotEmpty()
		{
			using (List<Formation>.Enumerator enumerator = this.FormationsIncludingSpecialAndEmpty.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CountOfUnits > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x000DBE24 File Offset: 0x000DA024
		public int GetFormationCount()
		{
			int num = 0;
			using (List<Formation>.Enumerator enumerator = this.FormationsIncludingEmpty.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CountOfUnits > 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x000DBE80 File Offset: 0x000DA080
		public int GetAIControlledFormationCount()
		{
			int num = 0;
			foreach (Formation formation in this.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0 && formation.IsAIControlled)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000DBEE4 File Offset: 0x000DA0E4
		public Vec2 GetAveragePositionOfEnemies()
		{
			Vec2 vec = Vec2.Zero;
			int num = 0;
			foreach (Team team in this.Mission.Teams)
			{
				if (team.MBTeam.IsValid && this.IsEnemyOf(team))
				{
					foreach (Agent agent in team.ActiveAgents)
					{
						vec += agent.Position.AsVec2;
						num++;
					}
				}
			}
			if (num > 0)
			{
				vec *= 1f / (float)num;
				return vec;
			}
			return Vec2.Invalid;
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000DBFCC File Offset: 0x000DA1CC
		public Vec2 GetAveragePosition()
		{
			Vec2 vec = Vec2.Zero;
			List<Agent> activeAgents = this.ActiveAgents;
			int num = 0;
			foreach (Agent agent in activeAgents)
			{
				vec += agent.Position.AsVec2;
				num++;
			}
			if (num > 0)
			{
				vec *= 1f / (float)num;
			}
			else
			{
				vec = Vec2.Invalid;
			}
			return vec;
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000DC058 File Offset: 0x000DA258
		public WorldPosition GetMedianPosition(Vec2 averagePosition)
		{
			float num = float.MaxValue;
			Agent agent = null;
			foreach (Agent agent2 in this.ActiveAgents)
			{
				float num2 = agent2.Position.AsVec2.DistanceSquared(averagePosition);
				if (num2 <= num)
				{
					agent = agent2;
					num = num2;
				}
			}
			if (agent == null)
			{
				return WorldPosition.Invalid;
			}
			return agent.GetWorldPosition();
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000DC0E0 File Offset: 0x000DA2E0
		public Vec2 GetWeightedAverageOfEnemies(Vec2 basePoint)
		{
			Vec2 vec = Vec2.Zero;
			float num = 0f;
			foreach (Team team in this.Mission.Teams)
			{
				if (team.MBTeam.IsValid && this.IsEnemyOf(team))
				{
					foreach (Agent agent in team.ActiveAgents)
					{
						Vec2 asVec = agent.Position.AsVec2;
						float lengthSquared = (basePoint - asVec).LengthSquared;
						float num2 = 1f / lengthSquared;
						vec += asVec * num2;
						num += num2;
					}
				}
			}
			if (num > 0f)
			{
				vec *= 1f / num;
				return vec;
			}
			return Vec2.Invalid;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000DC1F8 File Offset: 0x000DA3F8
		public void DisableDetachmentTicking()
		{
			this._tickDetachments = false;
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000DC201 File Offset: 0x000DA401
		[Conditional("DEBUG")]
		private void TickTaskForceDebug()
		{
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000DC203 File Offset: 0x000DA403
		[Conditional("DEBUG")]
		private void TickStandingPointDebug()
		{
		}

		// Token: 0x04001667 RID: 5735
		public readonly MBTeam MBTeam;

		// Token: 0x04001670 RID: 5744
		private List<OrderController> _orderControllers;

		// Token: 0x04001675 RID: 5749
		private MBList<Agent> _activeAgents;

		// Token: 0x04001676 RID: 5750
		private MBList<Agent> _teamAgents;

		// Token: 0x04001677 RID: 5751
		private bool _tickDetachments = true;

		// Token: 0x04001678 RID: 5752
		private MBList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>> _cachedEnemyDataForFleeing;

		// Token: 0x04001679 RID: 5753
		private bool _alreadyHasCustomOrderController;

		// Token: 0x0400167E RID: 5758
		private static Team _invalid;
	}
}
