using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000035 RID: 53
	public class OrderOfBattleVM : ViewModel
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000ECC2 File Offset: 0x0000CEC2
		protected int TotalFormationCount
		{
			get
			{
				return this._mission.PlayerTeam.FormationsIncludingEmpty.Count;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000ECD9 File Offset: 0x0000CED9
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0000ECE1 File Offset: 0x0000CEE1
		public List<MissionOrderVM.FormationConfiguration> CurrentConfiguration { get; private set; }

		// Token: 0x06000423 RID: 1059 RVA: 0x0000ECEC File Offset: 0x0000CEEC
		public OrderOfBattleVM()
		{
			this._allFormations = new List<OrderOfBattleFormationItemVM>();
			this._allHeroes = new List<OrderOfBattleHeroItemVM>();
			this._selectedHeroes = new List<OrderOfBattleHeroItemVM>();
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public override void RefreshValues()
		{
			this.BeginMissionText = new TextObject("{=SYYOSOoa}Ready", null).ToString();
			Mission mission = this._mission;
			if (mission != null && mission.IsSiegeBattle)
			{
				this.AutoDeployText = GameTexts.FindText("str_auto_deploy", null).ToString();
			}
			else
			{
				this.AutoDeployText = new TextObject("{=ADKHovtz}Reset Deployment", null).ToString();
			}
			this.MissingFormationsHint = new HintViewModel(this._missingFormationsHintText, null);
			this.SelectAllHint = new HintViewModel(this._selectAllHintText, null);
			this.ClearSelectionHint = new HintViewModel(this._clearSelectionHintText, null);
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.RefreshValues();
			});
			MBBindingList<OrderOfBattleHeroItemVM> unassignedHeroes = this.UnassignedHeroes;
			if (unassignedHeroes != null)
			{
				unassignedHeroes.ApplyActionOnAllItems(delegate(OrderOfBattleHeroItemVM c)
				{
					c.RefreshValues();
				});
			}
			SiegeDeploymentVM siegeDeployment = this.SiegeDeployment;
			if (siegeDeployment == null)
			{
				return;
			}
			siegeDeployment.RefreshValues();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000EEC0 File Offset: 0x0000D0C0
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.FinalizeFormationCallbacks();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM resetInputKey = this.ResetInputKey;
			if (resetInputKey != null)
			{
				resetInputKey.OnFinalize();
			}
			SiegeDeploymentVM siegeDeployment = this.SiegeDeployment;
			if (siegeDeployment == null)
			{
				return;
			}
			siegeDeployment.OnFinalize();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000EF28 File Offset: 0x0000D128
		private void InitializeFormationCallbacks()
		{
			OrderOfBattleFormationItemVM.OnClassSelectionToggled = new Action<OrderOfBattleFormationItemVM>(this.OnClassSelectionToggled);
			OrderOfBattleFormationItemVM.OnHeroesChanged = new Action(this.OnHeroesChanged);
			OrderOfBattleFormationItemVM.OnFilterUseToggled = new Action<OrderOfBattleFormationItemVM>(this.OnFilterUseToggled);
			OrderOfBattleFormationItemVM.OnSelection = new Action<OrderOfBattleFormationItemVM>(this.SelectFormationItem);
			OrderOfBattleFormationItemVM.OnDeselection = new Action<OrderOfBattleFormationItemVM>(this.DeselectFormationItem);
			OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter = new Func<DeploymentFormationClass, FormationFilterType, int>(this.GetTroopCountWithFilter);
			OrderOfBattleFormationItemVM.GetFormationWithCondition = new Func<Func<OrderOfBattleFormationItemVM, bool>, IEnumerable<OrderOfBattleFormationItemVM>>(this.GetFormationItemsWithCondition);
			OrderOfBattleFormationItemVM.HasAnyTroopWithClass = new Func<FormationClass, bool>(this.HasAnyTroopWithClass);
			OrderOfBattleFormationItemVM.OnAcceptCaptain = new Action<OrderOfBattleFormationItemVM>(this.OnFormationAcceptCaptain);
			OrderOfBattleFormationItemVM.OnAcceptHeroTroops = new Action<OrderOfBattleFormationItemVM>(this.OnFormationAcceptHeroTroops);
			OrderOfBattleFormationItemVM.OnFormationClassChanged = new Action(this.RefreshWeights);
			OrderOfBattleFormationClassVM.OnWeightAdjustedCallback = new Action<OrderOfBattleFormationClassVM>(this.OnWeightAdjusted);
			OrderOfBattleFormationClassVM.OnClassChanged = new Action<OrderOfBattleFormationClassVM, FormationClass>(this.OnFormationClassChanged);
			OrderOfBattleFormationClassVM.CanAdjustWeight = new Func<OrderOfBattleFormationClassVM, bool>(this.CanAdjustWeight);
			OrderOfBattleFormationClassVM.GetTotalCountOfTroopType = new Func<FormationClass, int>(this.GetVisibleTotalTroopCountOfType);
			OrderOfBattleHeroItemVM.OnHeroAssignmentBegin = new Action<OrderOfBattleHeroItemVM>(this.OnHeroAssignmentBegin);
			OrderOfBattleHeroItemVM.OnHeroAssignmentEnd = new Action<OrderOfBattleHeroItemVM>(this.OnHeroAssignmentEnd);
			OrderOfBattleHeroItemVM.GetAgentTooltip = new Func<Agent, List<TooltipProperty>>(this.GetAgentTooltip);
			OrderOfBattleHeroItemVM.OnHeroSelection = new Action<OrderOfBattleHeroItemVM>(this.OnHeroSelection);
			OrderOfBattleHeroItemVM.OnHeroAssignedFormationChanged = new Action<OrderOfBattleHeroItemVM>(this.OnHeroAssignedFormationChanged);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000F08C File Offset: 0x0000D28C
		private void FinalizeFormationCallbacks()
		{
			OrderOfBattleFormationItemVM.OnClassSelectionToggled = null;
			OrderOfBattleFormationItemVM.OnHeroesChanged = null;
			OrderOfBattleFormationItemVM.OnFilterUseToggled = null;
			OrderOfBattleFormationItemVM.OnSelection = null;
			OrderOfBattleFormationItemVM.OnDeselection = null;
			OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter = null;
			OrderOfBattleFormationItemVM.GetFormationWithCondition = null;
			OrderOfBattleFormationItemVM.HasAnyTroopWithClass = null;
			OrderOfBattleFormationItemVM.OnAcceptCaptain = null;
			OrderOfBattleFormationItemVM.OnAcceptHeroTroops = null;
			OrderOfBattleFormationItemVM.OnFormationClassChanged = null;
			OrderOfBattleFormationClassVM.OnWeightAdjustedCallback = null;
			OrderOfBattleFormationClassVM.OnClassChanged = null;
			OrderOfBattleFormationClassVM.CanAdjustWeight = null;
			OrderOfBattleFormationClassVM.GetTotalCountOfTroopType = null;
			OrderOfBattleHeroItemVM.OnHeroAssignmentBegin = null;
			OrderOfBattleHeroItemVM.OnHeroAssignmentEnd = null;
			OrderOfBattleHeroItemVM.GetAgentTooltip = null;
			OrderOfBattleHeroItemVM.OnHeroSelection = null;
			OrderOfBattleHeroItemVM.OnHeroAssignedFormationChanged = null;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000F114 File Offset: 0x0000D314
		public void Tick()
		{
			SiegeDeploymentVM siegeDeployment = this.SiegeDeployment;
			if (siegeDeployment != null)
			{
				siegeDeployment.Update();
			}
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				if (orderOfBattleFormationItemVM != null)
				{
					orderOfBattleFormationItemVM.Tick();
				}
				if (orderOfBattleFormationItemVM != null)
				{
					this.EnsureAllFormationTypesAreSet(orderOfBattleFormationItemVM);
				}
			}
			if (this._isInitialized)
			{
				if (this._isHeroSelectionDirty)
				{
					this.UpdateHeroItemSelection();
					this._isHeroSelectionDirty = false;
				}
				if (this._isTroopCountsDirty)
				{
					this.UpdateTroopTypeLookUpTable();
					this._isTroopCountsDirty = false;
				}
				if (this._isMissingFormationsDirty)
				{
					this.RefreshMissingFormations();
					this._isMissingFormationsDirty = false;
				}
				if (!this._isUnitDeployRefreshed)
				{
					this.OnUnitDeployed();
					this._isUnitDeployRefreshed = true;
				}
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000F1E0 File Offset: 0x0000D3E0
		private void EnsureAllFormationTypesAreSet(OrderOfBattleFormationItemVM f)
		{
			if (this.IsPlayerGeneral && f.OrderOfBattleFormationClassInt == 0 && f.Formation.CountOfUnits > 0)
			{
				bool flag = this._orderController.BackupAndDisableGesturesEnabled();
				for (int i = 0; i < this._allFormations.Count; i++)
				{
					OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations[i];
					if (this._orderController.SelectedFormations.Contains((orderOfBattleFormationItemVM != null) ? orderOfBattleFormationItemVM.Formation : null))
					{
						this._orderController.DeselectFormation((orderOfBattleFormationItemVM != null) ? orderOfBattleFormationItemVM.Formation : null);
					}
				}
				Func<OrderOfBattleFormationClassVM, bool> <>9__2;
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM2 = this._allFormations.Find(delegate(OrderOfBattleFormationItemVM other)
				{
					IEnumerable<OrderOfBattleFormationClassVM> classes = other.Classes;
					Func<OrderOfBattleFormationClassVM, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (OrderOfBattleFormationClassVM fc) => fc.Class == f.Formation.PhysicalClass);
					}
					return classes.Any<OrderOfBattleFormationClassVM>(func);
				});
				if (orderOfBattleFormationItemVM2 == null)
				{
					if (this._mission.IsSiegeBattle && (f.Formation.PhysicalClass == FormationClass.Cavalry || f.Formation.PhysicalClass == FormationClass.HorseArcher))
					{
						FormationClass secondaryFormationClassForMountedFormation = ((f.Formation.PhysicalClass == FormationClass.Cavalry) ? FormationClass.Infantry : FormationClass.Ranged);
						Func<OrderOfBattleFormationClassVM, bool> <>9__4;
						orderOfBattleFormationItemVM2 = this._allFormations.Find(delegate(OrderOfBattleFormationItemVM other)
						{
							IEnumerable<OrderOfBattleFormationClassVM> classes2 = other.Classes;
							Func<OrderOfBattleFormationClassVM, bool> func2;
							if ((func2 = <>9__4) == null)
							{
								func2 = (<>9__4 = (OrderOfBattleFormationClassVM fc) => fc.Class == secondaryFormationClassForMountedFormation);
							}
							return classes2.Any<OrderOfBattleFormationClassVM>(func2);
						});
					}
					if (orderOfBattleFormationItemVM2 == null)
					{
						orderOfBattleFormationItemVM2 = this._allFormations.Find((OrderOfBattleFormationItemVM other) => other.OrderOfBattleFormationClassInt != 0);
					}
				}
				if (orderOfBattleFormationItemVM2 != null)
				{
					Formation formation = orderOfBattleFormationItemVM2.Formation;
					this._orderController.SelectFormation(f.Formation);
					this._orderController.SetOrderWithFormationAndNumber(OrderType.Transfer, formation, f.Formation.CountOfUnits);
					for (int j = 0; j < this._allFormations.Count; j++)
					{
						OrderOfBattleFormationItemVM orderOfBattleFormationItemVM3 = this._allFormations[j];
						if (this._orderController.SelectedFormations.Contains(orderOfBattleFormationItemVM3.Formation))
						{
							this._orderController.DeselectFormation(orderOfBattleFormationItemVM3.Formation);
						}
					}
					orderOfBattleFormationItemVM2.OnSizeChanged();
					f.OnSizeChanged();
					this.RefreshWeights();
					this._orderController.RestoreGesturesEnabled(flag);
				}
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000F40C File Offset: 0x0000D60C
		public void Initialize(Mission mission, Camera missionCamera, Action<int> selectFormationAtIndex, Action<int> deselectFormationAtIndex, Action clearFormationSelection, Action onAutoDeploy, Action onBeginMission, Dictionary<int, Agent> formationIndicesAndSergeants)
		{
			this._mission = mission;
			this._missionCamera = missionCamera;
			this._selectFormationAtIndex = selectFormationAtIndex;
			this._deselectFormationAtIndex = deselectFormationAtIndex;
			this._clearFormationSelection = clearFormationSelection;
			this._onAutoDeploy = onAutoDeploy;
			this._onBeginMission = onBeginMission;
			this._bannerBearerLogic = mission.GetMissionBehavior<BannerBearerLogic>();
			if (this._bannerBearerLogic != null)
			{
				this._bannerBearerLogic.OnBannerBearersUpdated += this.OnBannerBearersUpdated;
				this._bannerBearerLogic.OnBannerBearerAgentUpdated += this.OnBannerAgentUpdated;
			}
			this.InitializeFormationCallbacks();
			this._isInitialized = false;
			this._orderController = Mission.Current.PlayerTeam.PlayerOrderController;
			this._orderController.OnSelectedFormationsChanged += this.OnSelectedFormationsChanged;
			this._orderController.OnOrderIssued += this.OnOrderIssued;
			this.CurrentConfiguration = new List<MissionOrderVM.FormationConfiguration>();
			this._availableTroopTypes = MissionGameModels.Current.BattleInitializationModel.GetAllAvailableTroopTypes();
			this.IsPlayerGeneral = this._mission.PlayerTeam.IsPlayerGeneral;
			this.FormationsFirstHalf = new MBBindingList<OrderOfBattleFormationItemVM>();
			this.FormationsSecondHalf = new MBBindingList<OrderOfBattleFormationItemVM>();
			this.UnassignedHeroes = new MBBindingList<OrderOfBattleHeroItemVM>();
			this._visibleTroopTypeCountLookup = new Dictionary<FormationClass, int>
			{
				{
					FormationClass.Infantry,
					0
				},
				{
					FormationClass.Ranged,
					0
				},
				{
					FormationClass.Cavalry,
					0
				},
				{
					FormationClass.HorseArcher,
					0
				}
			};
			for (int i = 0; i < this.TotalFormationCount; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = new OrderOfBattleFormationItemVM(this._missionCamera);
				if (i < this.TotalFormationCount / 2)
				{
					this.FormationsFirstHalf.Add(orderOfBattleFormationItemVM);
				}
				else
				{
					this.FormationsSecondHalf.Add(orderOfBattleFormationItemVM);
				}
				this._allFormations.Add(orderOfBattleFormationItemVM);
				Formation formation = this._mission.PlayerTeam.FormationsIncludingEmpty.ElementAt<Formation>(i);
				orderOfBattleFormationItemVM.RefreshFormation(formation, DeploymentFormationClass.Unset, false);
			}
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.OnSizeChanged();
			});
			using (List<Agent>.Enumerator enumerator = this._mission.PlayerTeam.GetHeroAgents().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Agent heroAgent = enumerator.Current;
					this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>((OrderOfBattleFormationItemVM f) => heroAgent.Formation == f.Formation);
					OrderOfBattleHeroItemVM orderOfBattleHeroItemVM = new OrderOfBattleHeroItemVM(heroAgent);
					this._allHeroes.Add(orderOfBattleHeroItemVM);
					if (this.IsPlayerGeneral || heroAgent.IsMainAgent)
					{
						this.UnassignedHeroes.Add(orderOfBattleHeroItemVM);
					}
				}
			}
			if (!this.IsPlayerGeneral)
			{
				using (Dictionary<int, Agent>.Enumerator enumerator2 = formationIndicesAndSergeants.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, Agent> preAssignedCaptain = enumerator2.Current;
						this._allHeroes.First<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM h) => h.Agent == preAssignedCaptain.Value).SetIsPreAssigned(true);
						this.AssignCaptain(preAssignedCaptain.Value, this._allFormations[preAssignedCaptain.Key]);
					}
				}
			}
			SiegeDeploymentHandler missionBehavior = mission.GetMissionBehavior<SiegeDeploymentHandler>();
			if (missionBehavior != null)
			{
				this.SiegeDeployment = new SiegeDeploymentVM(missionBehavior, missionCamera, missionBehavior.PlayerDeploymentPoints.ToList<DeploymentPoint>());
			}
			this.IsEnabled = true;
			this.SetAllFormationsLockState(true);
			this.LoadConfiguration();
			this.SetAllFormationsLockState(false);
			this.SetInitialHeroFormations();
			this.DistributeAllTroops();
			this._isInitialized = true;
			this.RefreshWeights();
			this.DeselectAllFormations();
			this.OnUnitDeployed();
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.UpdateAdjustable();
			});
			if (!this.IsPlayerGeneral)
			{
				this.SelectHeroItem(this._allHeroes.FirstOrDefault<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM h) => h.Agent.IsMainAgent));
			}
			this._isMissingFormationsDirty = true;
			this._isTroopCountsDirty = true;
			this.RefreshValues();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000F818 File Offset: 0x0000DA18
		private void UpdateTroopTypeLookUpTable()
		{
			for (FormationClass formationClass = FormationClass.Infantry; formationClass < FormationClass.NumberOfDefaultFormations; formationClass++)
			{
				this._visibleTroopTypeCountLookup[formationClass] = 0;
			}
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				Formation formation = this._allFormations[i].Formation;
				if (formation != null)
				{
					for (FormationClass formationClass2 = FormationClass.Infantry; formationClass2 < FormationClass.NumberOfDefaultFormations; formationClass2++)
					{
						Dictionary<FormationClass, int> visibleTroopTypeCountLookup = this._visibleTroopTypeCountLookup;
						FormationClass formationClass3 = formationClass2;
						visibleTroopTypeCountLookup[formationClass3] += formation.GetCountOfUnitsBelongingToPhysicalClass(formationClass2, false);
					}
				}
			}
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				orderOfBattleFormationItemVM.OnSizeChanged();
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		private void SetAllFormationsLockState(bool isLocked)
		{
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				for (int j = 0; j < this._allFormations[i].Classes.Count; j++)
				{
					this._allFormations[i].Classes[j].SetWeightAdjustmentLock(isLocked);
				}
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000F944 File Offset: 0x0000DB44
		private void OnBannerBearersUpdated(Formation formation)
		{
			if (this._isInitialized)
			{
				foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
				{
					orderOfBattleFormationItemVM.Formation.QuerySystem.Expire();
				}
				this._isTroopCountsDirty = true;
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		private void OnBannerAgentUpdated(Agent agent, bool isBannerBearer)
		{
			if (this._isInitialized && (agent.Team.IsPlayerTeam || agent.Team.IsPlayerAlly) && this._orderController.SelectedFormations.Contains(agent.Formation))
			{
				this._orderController.DeselectFormation(agent.Formation);
				this._orderController.SelectFormation(agent.Formation);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000FA1C File Offset: 0x0000DC1C
		private OrderOfBattleFormationItemVM GetFirstAvailableFormationWithAnyClass(params FormationClass[] classes)
		{
			OrderOfBattleVM.<>c__DisplayClass45_0 CS$<>8__locals1 = new OrderOfBattleVM.<>c__DisplayClass45_0();
			CS$<>8__locals1.classes = classes;
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.classes.Length; i = j + 1)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>((OrderOfBattleFormationItemVM f) => f.HasClass(CS$<>8__locals1.classes[i]));
				if (orderOfBattleFormationItemVM != null)
				{
					return orderOfBattleFormationItemVM;
				}
				j = i;
			}
			return null;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000FA90 File Offset: 0x0000DC90
		private OrderOfBattleFormationItemVM GetInitialHeroFormation(OrderOfBattleHeroItemVM hero)
		{
			FormationClass heroClass = FormationClass.NumberOfAllFormations;
			for (FormationClass formationClass = FormationClass.Infantry; formationClass < FormationClass.NumberOfDefaultFormations; formationClass++)
			{
				if (OrderOfBattleUIHelper.IsAgentInFormationClass(hero.Agent, formationClass))
				{
					heroClass = formationClass;
				}
			}
			if (heroClass == FormationClass.NumberOfAllFormations)
			{
				return null;
			}
			OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = null;
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM2 in this._allFormations)
			{
				if (orderOfBattleFormationItemVM2.Captain.Agent == hero.Agent || orderOfBattleFormationItemVM2.HeroTroops.Contains(hero))
				{
					hero.Agent.Formation = orderOfBattleFormationItemVM2.Formation;
					return orderOfBattleFormationItemVM2;
				}
				if (orderOfBattleFormationItemVM2.Formation == hero.Agent.Formation)
				{
					for (int i = orderOfBattleFormationItemVM2.Classes.Count - 1; i >= 0; i--)
					{
						if (!orderOfBattleFormationItemVM2.Classes[i].IsUnset && orderOfBattleFormationItemVM2.Classes[i].Class == heroClass)
						{
							return orderOfBattleFormationItemVM2;
						}
					}
				}
				if (orderOfBattleFormationItemVM != null)
				{
					break;
				}
			}
			if (!this.UnassignedHeroes.Contains(hero))
			{
				this.UnassignedHeroes.Add(hero);
			}
			Func<OrderOfBattleFormationClassVM, bool> <>9__1;
			OrderOfBattleFormationItemVM orderOfBattleFormationItemVM3 = this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>(delegate(OrderOfBattleFormationItemVM x)
			{
				IEnumerable<OrderOfBattleFormationClassVM> classes = x.Classes;
				Func<OrderOfBattleFormationClassVM, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (OrderOfBattleFormationClassVM c) => c.Class == heroClass);
				}
				return classes.Any<OrderOfBattleFormationClassVM>(func);
			});
			if (orderOfBattleFormationItemVM3 != null)
			{
				hero.Agent.Formation = orderOfBattleFormationItemVM3.Formation;
				return orderOfBattleFormationItemVM3;
			}
			FormationClass[] array = null;
			if (heroClass == FormationClass.HorseArcher)
			{
				FormationClass[] array2 = new FormationClass[3];
				array2[0] = FormationClass.Ranged;
				array2[1] = FormationClass.Cavalry;
				array = array2;
			}
			else if (heroClass == FormationClass.Cavalry)
			{
				array = new FormationClass[]
				{
					FormationClass.Infantry,
					FormationClass.Ranged
				};
			}
			else if (heroClass == FormationClass.Ranged)
			{
				array = new FormationClass[1];
			}
			if (array != null)
			{
				OrderOfBattleFormationItemVM firstAvailableFormationWithAnyClass = this.GetFirstAvailableFormationWithAnyClass(array);
				if (firstAvailableFormationWithAnyClass != null)
				{
					hero.Agent.Formation = firstAvailableFormationWithAnyClass.Formation;
					return firstAvailableFormationWithAnyClass;
				}
			}
			return null;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000FC88 File Offset: 0x0000DE88
		[return: TupleElementNames(new string[] { "Hero", "WasCaptain" })]
		private List<ValueTuple<OrderOfBattleHeroItemVM, bool>> ClearAllHeroAssignments()
		{
			List<ValueTuple<OrderOfBattleHeroItemVM, bool>> list = new List<ValueTuple<OrderOfBattleHeroItemVM, bool>>();
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				if (this._allFormations[i].HasCaptain)
				{
					OrderOfBattleHeroItemVM captain = this._allFormations[i].Captain;
					list.Add(new ValueTuple<OrderOfBattleHeroItemVM, bool>(captain, true));
					this.ClearHeroAssignment(captain);
				}
				for (int j = this._allFormations[i].HeroTroops.Count - 1; j >= 0; j--)
				{
					OrderOfBattleHeroItemVM orderOfBattleHeroItemVM = this._allFormations[i].HeroTroops[j];
					list.Add(new ValueTuple<OrderOfBattleHeroItemVM, bool>(orderOfBattleHeroItemVM, false));
					this.ClearHeroAssignment(orderOfBattleHeroItemVM);
				}
			}
			return list;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000FD44 File Offset: 0x0000DF44
		private void SetInitialHeroFormations()
		{
			for (int i = 0; i < this._allHeroes.Count; i++)
			{
				OrderOfBattleFormationItemVM initialHeroFormation = this.GetInitialHeroFormation(this._allHeroes[i]);
				if (initialHeroFormation != null)
				{
					this._allHeroes[i].SetInitialFormation(initialHeroFormation);
				}
				else
				{
					OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>(delegate(OrderOfBattleFormationItemVM f)
					{
						if (f.HasFormation)
						{
							return f.Classes.Any<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => !c.IsUnset);
						}
						return false;
					});
					if (orderOfBattleFormationItemVM != null)
					{
						this._allHeroes[i].SetInitialFormation(orderOfBattleFormationItemVM);
					}
					else
					{
						Debug.FailedAssert("Failed to find an initial formation for hero: " + this._allHeroes[i].Agent.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleVM.cs", "SetInitialHeroFormations", 623);
					}
				}
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000FE0D File Offset: 0x0000E00D
		protected virtual void LoadConfiguration()
		{
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000FE0F File Offset: 0x0000E00F
		protected virtual void SaveConfiguration()
		{
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000FE14 File Offset: 0x0000E014
		protected virtual List<TooltipProperty> GetAgentTooltip(Agent agent)
		{
			if (agent == null)
			{
				return new List<TooltipProperty>
				{
					new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator)
				};
			}
			List<TooltipProperty> list = new List<TooltipProperty>
			{
				new TooltipProperty(agent.Name, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.Title)
			};
			BannerComponent bannerComponent;
			if (agent.FormationBanner != null && (bannerComponent = agent.FormationBanner.ItemComponent as BannerComponent) != null)
			{
				if (!TextObject.IsNullOrEmpty(agent.FormationBanner.Name))
				{
					list.Add(new TooltipProperty(this._bannerText.ToString(), agent.FormationBanner.Name.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
					GameTexts.SetVariable("RANK", bannerComponent.BannerEffect.Name);
					string text = string.Empty;
					if (bannerComponent.BannerEffect.IncrementType == EffectIncrementType.AddFactor)
					{
						GameTexts.FindText("str_NUMBER_percent", null).SetTextVariable("NUMBER", ((int)Math.Abs(bannerComponent.GetBannerEffectBonus() * 100f)).ToString());
						object obj;
						text = obj.ToString();
					}
					else if (bannerComponent.BannerEffect.IncrementType == EffectIncrementType.Add)
					{
						text = bannerComponent.GetBannerEffectBonus().ToString();
					}
					GameTexts.SetVariable("NUMBER", text);
					list.Add(new TooltipProperty(this._bannerEffectText.ToString(), GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
				else
				{
					Debug.FailedAssert("Agent's formation banner name should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleVM.cs", "GetAgentTooltip", 685);
				}
			}
			else
			{
				list.Add(new TooltipProperty(this._noBannerEquippedText.ToString(), string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		private bool HasAnyTroopWithClass(FormationClass formationClass)
		{
			return this._availableTroopTypes.Contains(formationClass);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		private void RefreshWeights()
		{
			if (this._isSaving || !this._isInitialized)
			{
				return;
			}
			List<OrderOfBattleFormationClassVM> list = new List<OrderOfBattleFormationClassVM>();
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations[i];
				for (int j = 0; j < orderOfBattleFormationItemVM.Classes.Count; j++)
				{
					OrderOfBattleFormationClassVM orderOfBattleFormationClassVM = orderOfBattleFormationItemVM.Classes[j];
					if (orderOfBattleFormationClassVM.Class != FormationClass.NumberOfAllFormations)
					{
						list.Add(orderOfBattleFormationClassVM);
					}
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				OrderOfBattleFormationClassVM orderOfBattleFormationClassVM2 = list[k];
				orderOfBattleFormationClassVM2.SetWeightAdjustmentLock(true);
				float num = (float)OrderOfBattleUIHelper.GetCountOfRealUnitsInClass(orderOfBattleFormationClassVM2);
				float num2 = 0f;
				for (int l = 0; l < list.Count; l++)
				{
					OrderOfBattleFormationClassVM orderOfBattleFormationClassVM3 = list[l];
					if (orderOfBattleFormationClassVM3.Class == orderOfBattleFormationClassVM2.Class)
					{
						int countOfRealUnitsInClass = OrderOfBattleUIHelper.GetCountOfRealUnitsInClass(orderOfBattleFormationClassVM3);
						if (countOfRealUnitsInClass < 0 || countOfRealUnitsInClass > orderOfBattleFormationClassVM3.BelongedFormationItem.Formation.CountOfUnits)
						{
							orderOfBattleFormationClassVM3.SetWeightAdjustmentLock(true);
							orderOfBattleFormationClassVM3.Weight = 0;
							orderOfBattleFormationClassVM3.SetWeightAdjustmentLock(false);
							Debug.FailedAssert("Formation unit count is out of bounds! Skipping...", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleVM.cs", "RefreshWeights", 746);
							Debug.Print("Formation unit count is out of bounds! Skipping...", 0, Debug.DebugColor.White, 17592186044416UL);
						}
						else
						{
							num2 += (float)countOfRealUnitsInClass;
						}
					}
				}
				orderOfBattleFormationClassVM2.Weight = MathF.Round(num / num2 * 100f);
				orderOfBattleFormationClassVM2.IsLocked = !this.IsPlayerGeneral;
				orderOfBattleFormationClassVM2.SetWeightAdjustmentLock(false);
			}
			for (FormationClass formationClass = FormationClass.Infantry; formationClass < FormationClass.NumberOfDefaultFormations; formationClass++)
			{
				List<OrderOfBattleFormationClassVM> list2 = new List<OrderOfBattleFormationClassVM>();
				for (int m = 0; m < list.Count; m++)
				{
					OrderOfBattleFormationClassVM orderOfBattleFormationClassVM4 = list[m];
					if (orderOfBattleFormationClassVM4.Class == formationClass)
					{
						list2.Add(orderOfBattleFormationClassVM4);
					}
				}
				if (list2.Count > 1)
				{
					int num3 = 0;
					for (int n = 0; n < list2.Count; n++)
					{
						OrderOfBattleFormationClassVM orderOfBattleFormationClassVM5 = list2[n];
						if (orderOfBattleFormationClassVM5.Weight < 0 || orderOfBattleFormationClassVM5.Weight > 100)
						{
							orderOfBattleFormationClassVM5.SetWeightAdjustmentLock(true);
							orderOfBattleFormationClassVM5.Weight = 0;
							orderOfBattleFormationClassVM5.SetWeightAdjustmentLock(false);
							Debug.FailedAssert("Item weight is out of bounds! Skipping...", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleVM.cs", "RefreshWeights", 787);
							Debug.Print("Item weight is out of bounds! Skipping...", 0, Debug.DebugColor.White, 17592186044416UL);
						}
						else
						{
							num3 += orderOfBattleFormationClassVM5.Weight;
						}
					}
					for (int num4 = MathF.Abs(num3 - 100); num4 > 0; num4--)
					{
						bool flag = num3 < 100;
						object obj;
						if (!flag)
						{
							obj = list2.MaxBy<OrderOfBattleFormationClassVM, int>((OrderOfBattleFormationClassVM c) => c.Weight);
						}
						else
						{
							obj = list2.MinBy<OrderOfBattleFormationClassVM, int>((OrderOfBattleFormationClassVM c) => c.Weight);
						}
						object obj2 = obj;
						obj2.SetWeightAdjustmentLock(true);
						obj2.Weight += (flag ? 1 : (-1));
						obj2.SetWeightAdjustmentLock(false);
					}
				}
			}
			list.ForEach(delegate(OrderOfBattleFormationClassVM fc)
			{
				fc.UpdateWeightAdjustable();
			});
			list.ForEach(delegate(OrderOfBattleFormationClassVM fc)
			{
				fc.UpdateTroopCountText();
			});
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00010330 File Offset: 0x0000E530
		public void OnAllFormationsAssignedSergeants(Dictionary<int, Agent> preAssignedCaptains)
		{
			foreach (KeyValuePair<int, Agent> keyValuePair in preAssignedCaptains)
			{
				this.AssignCaptain(keyValuePair.Value, this._allFormations[keyValuePair.Key]);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00010398 File Offset: 0x0000E598
		private void OnClassSelectionToggled(OrderOfBattleFormationItemVM formationItem)
		{
			if (formationItem != null && formationItem.IsClassSelectionActive)
			{
				this._lastEnabledClassSelection = formationItem;
				return;
			}
			this._lastEnabledClassSelection = null;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000103B4 File Offset: 0x0000E5B4
		public bool IsAnyClassSelectionEnabled()
		{
			return this._lastEnabledClassSelection != null;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000103BF File Offset: 0x0000E5BF
		public void ExecuteDisableAllClassSelections()
		{
			if (this._lastEnabledClassSelection != null)
			{
				this._lastEnabledClassSelection.IsClassSelectionActive = false;
				this._lastEnabledClassSelection = null;
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000103DC File Offset: 0x0000E5DC
		private void SelectHeroItem(OrderOfBattleHeroItemVM heroItem)
		{
			if (!this._selectedHeroes.Contains(heroItem))
			{
				heroItem.IsSelected = true;
				this._selectedHeroes.Add(heroItem);
				this.UpdateHeroItemSelection();
				this.DeselectAllFormations();
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001040B File Offset: 0x0000E60B
		private void DeselectHeroItem(OrderOfBattleHeroItemVM heroItem)
		{
			heroItem.IsSelected = false;
			this._selectedHeroes.Remove(heroItem);
			this.UpdateHeroItemSelection();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010427 File Offset: 0x0000E627
		private void ToggleHeroItemSelection(OrderOfBattleHeroItemVM heroItem)
		{
			if (this._selectedHeroes.Contains(heroItem))
			{
				this.DeselectHeroItem(heroItem);
			}
			else
			{
				this.SelectHeroItem(heroItem);
			}
			this.UpdateHeroItemSelection();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00010450 File Offset: 0x0000E650
		private void UpdateHeroItemSelection()
		{
			bool flag = this._selectedHeroes.Count > 0;
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				bool flag2 = orderOfBattleFormationItemVM.HeroTroops.Any<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM heroTroop) => this._selectedHeroes.Contains(heroTroop));
				orderOfBattleFormationItemVM.OnHeroSelectionUpdated(this._selectedHeroes.Count, flag2);
			}
			bool flag3;
			if (flag)
			{
				flag3 = this._selectedHeroes.All<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM hero) => hero.IsLeadingAFormation);
			}
			else
			{
				flag3 = false;
			}
			this.IsPoolAcceptingCaptain = flag3;
			bool flag4;
			if (flag && !this.IsPoolAcceptingCaptain)
			{
				flag4 = this._selectedHeroes.All<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM hero) => hero.IsAssignedToAFormation);
			}
			else
			{
				flag4 = false;
			}
			this.IsPoolAcceptingHeroTroops = flag4;
			this.SelectedHeroCount = this._selectedHeroes.Count;
			this.HasSelectedHeroes = flag;
			this.LastSelectedHeroItem = ((this._selectedHeroes.Count > 0) ? this._selectedHeroes[this._selectedHeroes.Count - 1] : null);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00010590 File Offset: 0x0000E790
		private void OnHeroAssignmentBegin(OrderOfBattleHeroItemVM heroItem)
		{
			this.SelectHeroItem(heroItem);
			this._selectedHeroes.ForEach(delegate(OrderOfBattleHeroItemVM hero)
			{
				hero.IsShown = false;
			});
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000105C3 File Offset: 0x0000E7C3
		private void OnHeroAssignmentEnd(OrderOfBattleHeroItemVM heroItem)
		{
			this._selectedHeroes.ForEach(delegate(OrderOfBattleHeroItemVM hero)
			{
				hero.IsShown = true;
			});
			this.UpdateHeroItemSelection();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000105F5 File Offset: 0x0000E7F5
		private void ClearAndSelectHeroItem(OrderOfBattleHeroItemVM heroItem)
		{
			this.ClearHeroItemSelection();
			this.SelectHeroItem(heroItem);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010604 File Offset: 0x0000E804
		private void ClearHeroAssignment(OrderOfBattleHeroItemVM heroItem)
		{
			if (heroItem.IsLeadingAFormation)
			{
				heroItem.CurrentAssignedFormationItem.UnassignCaptain();
				return;
			}
			if (heroItem.IsAssignedToAFormation)
			{
				heroItem.CurrentAssignedFormationItem.RemoveHeroTroop(heroItem);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00010630 File Offset: 0x0000E830
		protected void AssignCaptain(Agent agent, OrderOfBattleFormationItemVM formationItem)
		{
			OrderOfBattleHeroItemVM orderOfBattleHeroItemVM = this._allHeroes.FirstOrDefault<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM h) => h.Agent == agent);
			if (formationItem != null && orderOfBattleHeroItemVM != null && formationItem.Captain != orderOfBattleHeroItemVM)
			{
				if (formationItem.HasCaptain)
				{
					formationItem.Captain.IsSelected = false;
					formationItem.UnassignCaptain();
				}
				formationItem.Captain = orderOfBattleHeroItemVM;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00010692 File Offset: 0x0000E892
		private void ClearHeroItemSelection()
		{
			this._selectedHeroes.ForEach(delegate(OrderOfBattleHeroItemVM hero)
			{
				hero.IsSelected = false;
			});
			this._selectedHeroes.Clear();
			this.UpdateHeroItemSelection();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000106D0 File Offset: 0x0000E8D0
		public void ExecuteAcceptHeroes()
		{
			foreach (OrderOfBattleHeroItemVM orderOfBattleHeroItemVM in this._selectedHeroes)
			{
				this.ClearHeroAssignment(orderOfBattleHeroItemVM);
				orderOfBattleHeroItemVM.IsShown = true;
			}
			this.ClearHeroItemSelection();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00010730 File Offset: 0x0000E930
		public void ExecuteSelectAllHeroes()
		{
			this.ClearHeroItemSelection();
			foreach (OrderOfBattleHeroItemVM orderOfBattleHeroItemVM in this.UnassignedHeroes)
			{
				this.SelectHeroItem(orderOfBattleHeroItemVM);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00010784 File Offset: 0x0000E984
		public void ExecuteClearHeroSelection()
		{
			this.ClearHeroItemSelection();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001078C File Offset: 0x0000E98C
		private void OnFormationAcceptCaptain(OrderOfBattleFormationItemVM formationItem)
		{
			if (this._selectedHeroes.Count != 1)
			{
				this._selectedHeroes.ForEach(delegate(OrderOfBattleHeroItemVM hero)
				{
					hero.IsShown = true;
				});
				this.ClearHeroItemSelection();
				return;
			}
			OrderOfBattleHeroItemVM orderOfBattleHeroItemVM = this._selectedHeroes[0];
			this.ClearHeroAssignment(orderOfBattleHeroItemVM);
			this.AssignCaptain(orderOfBattleHeroItemVM.Agent, formationItem);
			this.ClearHeroItemSelection();
			orderOfBattleHeroItemVM.IsShown = true;
			if (!this.IsPlayerGeneral)
			{
				this._mission.GetMissionBehavior<AssignPlayerRoleInTeamMissionController>().OnPlayerChoiceMade(formationItem.Formation.Index);
			}
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			game.EventManager.TriggerEvent<OrderOfBattleHeroAssignedToFormationEvent>(new OrderOfBattleHeroAssignedToFormationEvent(orderOfBattleHeroItemVM.Agent, formationItem.Formation));
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00010850 File Offset: 0x0000EA50
		private void OnFormationAcceptHeroTroops(OrderOfBattleFormationItemVM formationItem)
		{
			foreach (OrderOfBattleHeroItemVM orderOfBattleHeroItemVM in this._selectedHeroes)
			{
				this.ClearHeroAssignment(orderOfBattleHeroItemVM);
				formationItem.AddHeroTroop(orderOfBattleHeroItemVM);
				orderOfBattleHeroItemVM.IsShown = true;
			}
			this.ClearHeroItemSelection();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000108B8 File Offset: 0x0000EAB8
		private void OnHeroSelection(OrderOfBattleHeroItemVM heroSlotItem)
		{
			if (!this.IsPlayerGeneral)
			{
				this.ToggleHeroItemSelection(heroSlotItem);
				return;
			}
			if (heroSlotItem.IsLeadingAFormation)
			{
				this.ClearAndSelectHeroItem(heroSlotItem);
				return;
			}
			this.ToggleHeroItemSelection(heroSlotItem);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000108E4 File Offset: 0x0000EAE4
		private void OnFilterUseToggled(OrderOfBattleFormationItemVM formationItem)
		{
			foreach (OrderOfBattleFormationClassVM orderOfBattleFormationClassVM in formationItem.Classes)
			{
				if (orderOfBattleFormationClassVM.Class != FormationClass.NumberOfAllFormations)
				{
					this.DistributeTroops(orderOfBattleFormationClassVM);
				}
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001093C File Offset: 0x0000EB3C
		public void OnDeploymentFinalized(bool playerDeployed)
		{
			if (playerDeployed)
			{
				this._isSaving = true;
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>((OrderOfBattleFormationItemVM f) => f.Captain.Agent == this._mission.InitialPlayerAgent);
				if (orderOfBattleFormationItemVM != null)
				{
					AssignPlayerRoleInTeamMissionController missionBehavior = this._mission.GetMissionBehavior<AssignPlayerRoleInTeamMissionController>();
					missionBehavior.OnPlayerChoiceMade(orderOfBattleFormationItemVM.Formation.Index);
					missionBehavior.OnPlayerChoiceFinalized();
				}
				this.SaveConfiguration();
				this._isSaving = false;
				if (this._orderController != null)
				{
					this._orderController.OnSelectedFormationsChanged -= this.OnSelectedFormationsChanged;
					this._orderController.OnOrderIssued -= this.OnOrderIssued;
				}
			}
			SiegeDeploymentVM siegeDeployment = this.SiegeDeployment;
			if (siegeDeployment != null)
			{
				siegeDeployment.OnDeploymentFinalized();
			}
			this.IsEnabled = false;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000109F0 File Offset: 0x0000EBF0
		private void OnHeroAssignedFormationChanged(OrderOfBattleHeroItemVM heroItem)
		{
			if (heroItem.IsAssignedToAFormation)
			{
				this.UnassignedHeroes.Remove(this.UnassignedHeroes.FirstOrDefault<OrderOfBattleHeroItemVM>((OrderOfBattleHeroItemVM h) => h.Agent == heroItem.Agent));
			}
			else if (this.IsPlayerGeneral || heroItem.Agent.IsMainAgent)
			{
				this.UnassignedHeroes.Insert(0, heroItem);
			}
			this._isTroopCountsDirty = true;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00010A6F File Offset: 0x0000EC6F
		private bool CanAdjustWeight(OrderOfBattleFormationClassVM formationClass)
		{
			return this._isInitialized && OrderOfBattleUIHelper.GetMatchingClasses(this._allFormations, formationClass, null).Count > 1;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00010A90 File Offset: 0x0000EC90
		private void OnWeightAdjusted(OrderOfBattleFormationClassVM formationClass)
		{
			if (!this._isInitialized)
			{
				return;
			}
			this.DistributeWeights(formationClass);
			this.DistributeTroops(formationClass);
			EventManager eventManager = Game.Current.EventManager;
			OrderOfBattleFormationItemVM belongedFormationItem = formationClass.BelongedFormationItem;
			eventManager.TriggerEvent<OrderOfBattleFormationWeightChangedEvent>(new OrderOfBattleFormationWeightChangedEvent((belongedFormationItem != null) ? belongedFormationItem.Formation : null));
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		private void DistributeTroops(OrderOfBattleFormationClassVM formationClass)
		{
			List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> massTransferDataForFormation = this.GetMassTransferDataForFormation(formationClass);
			if (massTransferDataForFormation.Count > 0)
			{
				this._orderController.RearrangeFormationsAccordingToFilters(this._mission.PlayerTeam, massTransferDataForFormation);
				this.RefreshFormationsWithClass(formationClass.Class);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00010B14 File Offset: 0x0000ED14
		private void DistributeWeights(OrderOfBattleFormationClassVM formationClass)
		{
			List<OrderOfBattleFormationClassVM> matchingClasses = OrderOfBattleUIHelper.GetMatchingClasses(this._allFormations, formationClass, null);
			List<OrderOfBattleFormationClassVM> matchingClasses2 = OrderOfBattleUIHelper.GetMatchingClasses(this._allFormations, formationClass, (OrderOfBattleFormationClassVM fc) => !fc.IsLocked);
			if (matchingClasses2.Count == 1)
			{
				formationClass.SetWeightAdjustmentLock(true);
				formationClass.Weight = formationClass.PreviousWeight;
				formationClass.SetWeightAdjustmentLock(false);
				return;
			}
			int num = OrderOfBattleUIHelper.GetMatchingClasses(this._allFormations, formationClass, (OrderOfBattleFormationClassVM fc) => fc.IsLocked).Sum<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM fc) => fc.Weight);
			int adjustableWeight = 100 - num;
			if (formationClass.Weight > adjustableWeight)
			{
				formationClass.SetWeightAdjustmentLock(true);
				formationClass.Weight = adjustableWeight;
				formationClass.SetWeightAdjustmentLock(false);
				matchingClasses2.Remove(formationClass);
				matchingClasses2.ForEach(delegate(OrderOfBattleFormationClassVM c)
				{
					c.SetWeightAdjustmentLock(true);
					c.Weight = 0;
					c.SetWeightAdjustmentLock(false);
				});
				return;
			}
			matchingClasses2.Remove(formationClass);
			int changePerClass = MathF.Round((float)(formationClass.PreviousWeight - formationClass.Weight) / (float)matchingClasses2.Count);
			matchingClasses2.ForEach(delegate(OrderOfBattleFormationClassVM formation)
			{
				formation.SetWeightAdjustmentLock(true);
			});
			if (changePerClass != 0)
			{
				matchingClasses2.ForEach(delegate(OrderOfBattleFormationClassVM formation)
				{
					int num5 = MBMath.ClampInt(changePerClass, -formation.Weight, adjustableWeight - formation.Weight);
					formation.Weight += num5;
				});
			}
			int num2 = matchingClasses.Sum<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => c.Weight);
			while (matchingClasses2.Count > 0 && num2 != 100)
			{
				int num3 = num2;
				if (num2 > 100)
				{
					OrderOfBattleFormationClassVM formationClassWithExtremumWeight = OrderOfBattleUIHelper.GetFormationClassWithExtremumWeight(matchingClasses2, false);
					if (formationClassWithExtremumWeight != null)
					{
						OrderOfBattleFormationClassVM orderOfBattleFormationClassVM = formationClassWithExtremumWeight;
						int num4 = orderOfBattleFormationClassVM.Weight;
						orderOfBattleFormationClassVM.Weight = num4 - 1;
						num2--;
					}
				}
				else if (num2 < 100)
				{
					OrderOfBattleFormationClassVM formationClassWithExtremumWeight2 = OrderOfBattleUIHelper.GetFormationClassWithExtremumWeight(matchingClasses2, true);
					if (formationClassWithExtremumWeight2 != null)
					{
						OrderOfBattleFormationClassVM orderOfBattleFormationClassVM2 = formationClassWithExtremumWeight2;
						int num4 = orderOfBattleFormationClassVM2.Weight;
						orderOfBattleFormationClassVM2.Weight = num4 + 1;
						num2++;
					}
				}
				if (num3 == num2)
				{
					Debug.FailedAssert("Failed to sum up all weights to 100", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleVM.cs", "DistributeWeights", 1207);
					break;
				}
			}
			matchingClasses2.ForEach(delegate(OrderOfBattleFormationClassVM formation)
			{
				formation.SetWeightAdjustmentLock(false);
			});
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00010D78 File Offset: 0x0000EF78
		private void DistributeAllTroops()
		{
			if (this._mission.PlayerTeam == null)
			{
				Debug.FailedAssert("Player team should be initialized before distributing troops", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleVM.cs", "DistributeAllTroops", 1219);
				Debug.Print("Player team should be initialized before distributing troops", 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> list = new List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>();
			List<FormationClass> list2 = new List<FormationClass>();
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				for (int j = 0; j < this._allFormations[i].Classes.Count; j++)
				{
					OrderOfBattleFormationClassVM orderOfBattleFormationClassVM = this._allFormations[i].Classes[j];
					if (!orderOfBattleFormationClassVM.IsUnset && !list2.Contains(orderOfBattleFormationClassVM.Class))
					{
						List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> massTransferDataForFormation = this.GetMassTransferDataForFormation(orderOfBattleFormationClassVM);
						list.AddRange(massTransferDataForFormation);
						list2.Add(orderOfBattleFormationClassVM.Class);
					}
				}
				if (list.Count > 0)
				{
					this._orderController.RearrangeFormationsAccordingToFilters(this._mission.PlayerTeam, list);
				}
				list.Clear();
				if (list2.Count == 4)
				{
					break;
				}
			}
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.OnSizeChanged();
			});
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00010EB4 File Offset: 0x0000F0B4
		[return: TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })]
		private List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> GetMassTransferDataForFormationClass(Formation targetFormation, FormationClass formationClass)
		{
			List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> list = new List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>();
			List<OrderOfBattleFormationItemVM> list2 = new List<OrderOfBattleFormationItemVM>();
			List<int> list3 = new List<int>();
			int num = 0;
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				int totalCountOfUnitsInClass = OrderOfBattleUIHelper.GetTotalCountOfUnitsInClass(this._allFormations[i].Formation, formationClass);
				if (totalCountOfUnitsInClass > 0 || this._allFormations[i].Formation == targetFormation)
				{
					list2.Add(this._allFormations[i]);
					list3.Add(totalCountOfUnitsInClass);
					num += totalCountOfUnitsInClass;
				}
			}
			if (list2.Count == 1)
			{
				return list;
			}
			if (num > 0)
			{
				List<int> list4 = new List<int>();
				for (int j = 0; j < list2.Count; j++)
				{
					int num2 = ((list2[j].Formation == targetFormation) ? num : 0);
					list4.Add(num2);
				}
				int num3;
				while (list4.Count > 0 && (num3 = list4.Sum()) != num)
				{
					int num4 = num3 - num;
					int num5;
					if (num4 <= 0)
					{
						num5 = list4.IndexOfMin<int>((int c) => c);
					}
					else
					{
						num5 = list4.IndexOfMax<int>((int c) => c);
					}
					List<int> list5 = list4;
					int num6 = num5;
					list5[num6] -= Math.Sign(num4);
				}
				for (int k = 0; k < list4.Count; k++)
				{
					OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = list2[k];
					TroopTraitsMask troopTraitsMask = TroopFilteringUtilities.GetFilter(new FormationClass[] { formationClass });
					troopTraitsMask |= TroopFilteringUtilities.GetFilter((from f in orderOfBattleFormationItemVM.FilterItems
						where f.IsActive
						select f.FilterType).ToArray<FormationFilterType>());
					if (troopTraitsMask != TroopTraitsMask.None)
					{
						ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> valueTuple = OrderOfBattleUIHelper.CreateMassTransferData(orderOfBattleFormationItemVM, formationClass, troopTraitsMask, list4[k]);
						list.Add(valueTuple);
					}
				}
			}
			return list;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000110E0 File Offset: 0x0000F2E0
		[return: TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })]
		private List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> GetMassTransferDataForFormation(OrderOfBattleFormationClassVM formationClass)
		{
			List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> list = new List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>();
			List<OrderOfBattleFormationClassVM> allFormationClassesWith = this.GetAllFormationClassesWith(formationClass.Class);
			if (allFormationClassesWith.Count == 1)
			{
				return list;
			}
			int num = allFormationClassesWith.Sum<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => OrderOfBattleUIHelper.GetCountOfRealUnitsInClass(c));
			if (num > 0)
			{
				List<int> list2 = new List<int>();
				for (int i = 0; i < allFormationClassesWith.Count; i++)
				{
					int num2 = MathF.Ceiling((float)allFormationClassesWith[i].Weight / 100f * (float)num);
					list2.Add(num2);
				}
				int num3;
				while (list2.Count > 0 && (num3 = list2.Sum()) != num)
				{
					int num4 = num3 - num;
					int num5;
					if (num4 <= 0)
					{
						num5 = list2.IndexOfMin<int>((int c) => c);
					}
					else
					{
						num5 = list2.IndexOfMax<int>((int c) => c);
					}
					List<int> list3 = list2;
					int num6 = num5;
					list3[num6] -= Math.Sign(num4);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					OrderOfBattleFormationItemVM belongedFormationItem = allFormationClassesWith[j].BelongedFormationItem;
					TroopTraitsMask troopTraitsMask = TroopFilteringUtilities.GetFilter((from c in belongedFormationItem.Classes
						where !c.IsUnset
						select c.Class).ToArray<FormationClass>());
					troopTraitsMask |= TroopFilteringUtilities.GetFilter((from f in belongedFormationItem.FilterItems
						where f.IsActive
						select f.FilterType).ToArray<FormationFilterType>());
					if (troopTraitsMask != TroopTraitsMask.None)
					{
						ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> valueTuple = OrderOfBattleUIHelper.CreateMassTransferData(allFormationClassesWith[j], allFormationClassesWith[j].Class, troopTraitsMask, list2[j]);
						list.Add(valueTuple);
					}
				}
			}
			return list;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00011324 File Offset: 0x0000F524
		private List<OrderOfBattleFormationClassVM> GetAllFormationClassesWith(FormationClass formationClass)
		{
			List<OrderOfBattleFormationClassVM> list = new List<OrderOfBattleFormationClassVM>();
			if (formationClass >= FormationClass.NumberOfDefaultFormations)
			{
				return list;
			}
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				for (int j = 0; j < this._allFormations[i].Classes.Count; j++)
				{
					if (this._allFormations[i].Classes[j].Class == formationClass)
					{
						list.Add(this._allFormations[i].Classes[j]);
					}
				}
			}
			return list;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000113B4 File Offset: 0x0000F5B4
		private void RefreshFormationsWithClass(FormationClass formationClass)
		{
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				for (int j = 0; j < this._allFormations[i].Classes.Count; j++)
				{
					if (this._allFormations[i].Classes[j].Class == formationClass)
					{
						this._allFormations[i].OnSizeChanged();
						break;
					}
				}
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001142C File Offset: 0x0000F62C
		private List<Agent> GetLockedAgents()
		{
			List<Agent> list = new List<Agent>();
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				if (orderOfBattleFormationItemVM.Captain.Agent != null)
				{
					list.Add(orderOfBattleFormationItemVM.Captain.Agent);
				}
				foreach (OrderOfBattleHeroItemVM orderOfBattleHeroItemVM in orderOfBattleFormationItemVM.HeroTroops)
				{
					list.Add(orderOfBattleHeroItemVM.Agent);
				}
			}
			return list;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000114E0 File Offset: 0x0000F6E0
		private void OnFormationClassChanged(OrderOfBattleFormationClassVM formationClassItem, FormationClass newFormationClass)
		{
			if (!this._isInitialized)
			{
				return;
			}
			List<OrderOfBattleFormationClassVM> previousFormationClasses = new List<OrderOfBattleFormationClassVM>();
			List<OrderOfBattleFormationClassVM> newFormationClasses = new List<OrderOfBattleFormationClassVM>();
			Func<OrderOfBattleFormationClassVM, bool> <>9__5;
			Func<OrderOfBattleFormationClassVM, bool> <>9__6;
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM formation)
			{
				List<OrderOfBattleFormationClassVM> previousFormationClasses2 = previousFormationClasses;
				IEnumerable<OrderOfBattleFormationClassVM> enumerable = formation.Classes.ToList<OrderOfBattleFormationClassVM>();
				Func<OrderOfBattleFormationClassVM, bool> func3;
				if ((func3 = <>9__5) == null)
				{
					func3 = (<>9__5 = (OrderOfBattleFormationClassVM fc) => fc.Class == formationClassItem.Class);
				}
				previousFormationClasses2.AddRange(enumerable.Where<OrderOfBattleFormationClassVM>(func3));
				List<OrderOfBattleFormationClassVM> newFormationClasses2 = newFormationClasses;
				IEnumerable<OrderOfBattleFormationClassVM> enumerable2 = formation.Classes.ToList<OrderOfBattleFormationClassVM>();
				Func<OrderOfBattleFormationClassVM, bool> func4;
				if ((func4 = <>9__6) == null)
				{
					func4 = (<>9__6 = (OrderOfBattleFormationClassVM fc) => fc.Class == newFormationClass);
				}
				newFormationClasses2.AddRange(enumerable2.Where<OrderOfBattleFormationClassVM>(func4));
			});
			if (newFormationClasses.Count > 0)
			{
				formationClassItem.Weight = 0;
			}
			else
			{
				this.TransferAllAvailableTroopsToFormation(formationClassItem.BelongedFormationItem, newFormationClass);
				formationClassItem.SetWeightAdjustmentLock(true);
				formationClassItem.Weight = 100;
				formationClassItem.SetWeightAdjustmentLock(false);
			}
			newFormationClasses.Add(formationClassItem);
			previousFormationClasses.ForEach(delegate(OrderOfBattleFormationClassVM fc)
			{
				fc.IsAdjustable = formationClassItem.Class != FormationClass.NumberOfAllFormations && previousFormationClasses.Count > 2;
			});
			newFormationClasses.ForEach(delegate(OrderOfBattleFormationClassVM fc)
			{
				fc.IsAdjustable = newFormationClass != FormationClass.NumberOfAllFormations && newFormationClasses.Count > 1;
			});
			List<OrderOfBattleFormationClassVM> allClasses = new List<OrderOfBattleFormationClassVM>();
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM formation)
			{
				allClasses.AddRange(formation.Classes.Where<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM fc) => fc.Class != FormationClass.NumberOfAllFormations));
			});
			if (newFormationClass != FormationClass.NumberOfAllFormations || allClasses.Contains(formationClassItem))
			{
				allClasses.Remove(formationClassItem);
				allClasses.Add(new OrderOfBattleFormationClassVM(formationClassItem.BelongedFormationItem, newFormationClass));
			}
			bool flag = this._orderController.BackupAndDisableGesturesEnabled();
			IEnumerable<OrderOfBattleFormationItemVM> allFormations = this._allFormations;
			Func<OrderOfBattleFormationItemVM, bool> <>9__8;
			Func<OrderOfBattleFormationItemVM, bool> func;
			if ((func = <>9__8) == null)
			{
				Func<OrderOfBattleFormationClassVM, bool> <>9__9;
				func = (<>9__8 = delegate(OrderOfBattleFormationItemVM f)
				{
					IEnumerable<OrderOfBattleFormationClassVM> classes2 = f.Classes;
					Func<OrderOfBattleFormationClassVM, bool> func5;
					if ((func5 = <>9__9) == null)
					{
						func5 = (<>9__9 = (OrderOfBattleFormationClassVM c) => c.Class != newFormationClass);
					}
					return classes2.All<OrderOfBattleFormationClassVM>(func5);
				});
			}
			Func<OrderOfBattleFormationClassVM, bool> <>9__10;
			Action<OrderOfBattleFormationItemVM> <>9__11;
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in allFormations.Where<OrderOfBattleFormationItemVM>(func))
			{
				IEnumerable<OrderOfBattleFormationClassVM> classes = orderOfBattleFormationItemVM.Classes;
				Func<OrderOfBattleFormationClassVM, bool> func2;
				if ((func2 = <>9__10) == null)
				{
					func2 = (<>9__10 = (OrderOfBattleFormationClassVM c) => c.Class == newFormationClass);
				}
				ValueTuple<int, bool, bool> relevantTroopTransferParameters = OrderOfBattleUIHelper.GetRelevantTroopTransferParameters(classes.FirstOrDefault<OrderOfBattleFormationClassVM>(func2));
				if (relevantTroopTransferParameters.Item1 > 0)
				{
					List<OrderOfBattleFormationItemVM> allFormations2 = this._allFormations;
					Action<OrderOfBattleFormationItemVM> action;
					if ((action = <>9__11) == null)
					{
						action = (<>9__11 = delegate(OrderOfBattleFormationItemVM f)
						{
							if (this._orderController.SelectedFormations.Contains(f.Formation))
							{
								this._orderController.DeselectFormation(f.Formation);
							}
						});
					}
					allFormations2.ForEach(action);
					this._orderController.SelectFormation(orderOfBattleFormationItemVM.Formation);
					this._orderController.TransferUnitWithPriorityFunction(formationClassItem.BelongedFormationItem.Formation, relevantTroopTransferParameters.Item1, false, false, false, false, relevantTroopTransferParameters.Item2, relevantTroopTransferParameters.Item3, true, this.GetLockedAgents());
					orderOfBattleFormationItemVM.OnSizeChanged();
					formationClassItem.BelongedFormationItem.OnSizeChanged();
				}
			}
			this._isTroopCountsDirty = true;
			this._isHeroSelectionDirty = true;
			this._isMissingFormationsDirty = true;
			this._orderController.RestoreGesturesEnabled(flag);
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.UpdateAdjustable();
			});
			EventManager eventManager = Game.Current.EventManager;
			OrderOfBattleFormationItemVM belongedFormationItem = formationClassItem.BelongedFormationItem;
			eventManager.TriggerEvent<OrderOfBattleFormationClassChangedEvent>(new OrderOfBattleFormationClassChangedEvent((belongedFormationItem != null) ? belongedFormationItem.Formation : null));
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011800 File Offset: 0x0000FA00
		private void TransferAllAvailableTroopsToFormation(OrderOfBattleFormationItemVM formation, FormationClass formationClass)
		{
			List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> massTransferDataForFormationClass = this.GetMassTransferDataForFormationClass(formation.Formation, formationClass);
			if (massTransferDataForFormationClass.Count > 0)
			{
				this._orderController.RearrangeFormationsAccordingToFilters(this._mission.PlayerTeam, massTransferDataForFormationClass);
				this.RefreshFormationsWithClass(formationClass);
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011844 File Offset: 0x0000FA44
		private void RefreshMissingFormations()
		{
			if (this.IsPlayerGeneral)
			{
				List<OrderOfBattleFormationClassVM> allClasses = new List<OrderOfBattleFormationClassVM>();
				this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM formation)
				{
					allClasses.AddRange(formation.Classes.Where<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM fc) => fc.Class != FormationClass.NumberOfAllFormations));
				});
				bool flag = false;
				using (List<FormationClass>.Enumerator enumerator = this._availableTroopTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FormationClass availableTroopType = enumerator.Current;
						if (allClasses.All<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => c.Class != availableTroopType))
						{
							if (Mission.Current.IsSiegeBattle)
							{
								if (availableTroopType != FormationClass.HorseArcher && availableTroopType != FormationClass.Cavalry)
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
							if (flag)
							{
								this.MissingFormationsHint.HintText.SetTextVariable("FORMATION_CLASS", availableTroopType.GetLocalizedName());
								this.CanStartMission = false;
								break;
							}
						}
					}
				}
				this.CanStartMission = !flag;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011948 File Offset: 0x0000FB48
		private OrderOfBattleFormationItemVM GetFormationItemAtIndex(int index)
		{
			if (index < this.TotalFormationCount / 2)
			{
				return this.FormationsFirstHalf.ElementAt<OrderOfBattleFormationItemVM>(index);
			}
			if (index < this.TotalFormationCount)
			{
				return this.FormationsSecondHalf.ElementAt<OrderOfBattleFormationItemVM>(index - this.TotalFormationCount / 2);
			}
			return null;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011982 File Offset: 0x0000FB82
		private IEnumerable<OrderOfBattleFormationItemVM> GetFormationItemsWithCondition(Func<OrderOfBattleFormationItemVM, bool> condition)
		{
			return this._allFormations.Where<OrderOfBattleFormationItemVM>(condition);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011990 File Offset: 0x0000FB90
		private void OnSelectedFormationsChanged()
		{
			if (!this._isInitialized)
			{
				return;
			}
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations[i];
				orderOfBattleFormationItemVM.IsSelected = this._orderController.IsFormationListening(orderOfBattleFormationItemVM.Formation);
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000119E0 File Offset: 0x0000FBE0
		private void SelectFormationItem(OrderOfBattleFormationItemVM formationItem)
		{
			formationItem.IsSelected = true;
			this._selectFormationAtIndex(formationItem.Formation.Index);
			this.ExecuteClearHeroSelection();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011A08 File Offset: 0x0000FC08
		private void DeselectFormationItem(OrderOfBattleFormationItemVM formationItem)
		{
			Formation formation = formationItem.Formation;
			if (formation != null && formation.Index >= 0)
			{
				Mission.Current.PlayerTeam.PlayerOrderController.DeselectFormation(formationItem.Formation);
				Action<int> deselectFormationAtIndex = this._deselectFormationAtIndex;
				if (deselectFormationAtIndex == null)
				{
					return;
				}
				deselectFormationAtIndex(formationItem.Formation.Index);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00011A64 File Offset: 0x0000FC64
		public void SelectFormationItemAtIndex(int index)
		{
			this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>((OrderOfBattleFormationItemVM f) => f.Formation.Index == index).IsSelected = true;
			this._selectFormationAtIndex(index);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00011AAC File Offset: 0x0000FCAC
		public void FocusFormationItemAtIndex(int index)
		{
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.IsBeingFocused = false;
			});
			this._allFormations.FirstOrDefault<OrderOfBattleFormationItemVM>((OrderOfBattleFormationItemVM f) => f.Formation.Index == index).IsBeingFocused = true;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011B10 File Offset: 0x0000FD10
		public void DeselectAllFormations()
		{
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				orderOfBattleFormationItemVM.IsSelected = false;
			}
			Action clearFormationSelection = this._clearFormationSelection;
			if (clearFormationSelection == null)
			{
				return;
			}
			clearFormationSelection();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00011B74 File Offset: 0x0000FD74
		public void OnUnitDeployed()
		{
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				if (f != null)
				{
					f.MakeMarkerWorldPositionDirty();
				}
			});
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00011BA0 File Offset: 0x0000FDA0
		public bool OnEscape()
		{
			SiegeDeploymentVM siegeDeployment = this.SiegeDeployment;
			if (siegeDeployment != null && siegeDeployment.IsSiegeDeploymentListActive)
			{
				this.SiegeDeployment.ExecuteCancelSelectedDeploymentPoint();
				return true;
			}
			if (this._allFormations.Any<OrderOfBattleFormationItemVM>((OrderOfBattleFormationItemVM f) => f.IsSelected))
			{
				this.DeselectAllFormations();
				return true;
			}
			return false;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00011C04 File Offset: 0x0000FE04
		private int GetTroopCountWithFilter(DeploymentFormationClass orderOfBattleFormationClass, FormationFilterType filterType)
		{
			int num = 0;
			List<FormationClass> formationClasses = orderOfBattleFormationClass.GetFormationClasses();
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				List<FormationClass> list = (from c in orderOfBattleFormationItemVM.Classes
					select c.Class into c
					where c != FormationClass.NumberOfAllFormations
					select c).ToList<FormationClass>();
				if (formationClasses.Intersect<FormationClass>(list).Any<FormationClass>())
				{
					switch (filterType)
					{
					case FormationFilterType.Shield:
						num += orderOfBattleFormationItemVM.Formation.GetCountOfUnitsWithCondition((Agent a) => a.HasShieldCached);
						break;
					case FormationFilterType.Spear:
						num += orderOfBattleFormationItemVM.Formation.GetCountOfUnitsWithCondition((Agent a) => a.HasSpearCached);
						break;
					case FormationFilterType.Thrown:
						num += orderOfBattleFormationItemVM.Formation.GetCountOfUnitsWithCondition((Agent a) => a.HasThrownCached);
						break;
					case FormationFilterType.Heavy:
						num += orderOfBattleFormationItemVM.Formation.GetCountOfUnitsWithCondition((Agent a) => MissionGameModels.Current.AgentStatCalculateModel.HasHeavyArmor(a));
						break;
					case FormationFilterType.HighTier:
						num += orderOfBattleFormationItemVM.Formation.GetCountOfUnitsWithCondition((Agent a) => a.Character.GetBattleTier() >= 4);
						break;
					case FormationFilterType.LowTier:
						num += orderOfBattleFormationItemVM.Formation.GetCountOfUnitsWithCondition((Agent a) => a.Character.GetBattleTier() <= 3);
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00011E1C File Offset: 0x0001001C
		protected void ClearFormationItem(OrderOfBattleFormationItemVM formationItem)
		{
			formationItem.FormationClassSelector.SelectedIndex = 0;
			formationItem.UnassignCaptain();
			foreach (OrderOfBattleFormationClassVM orderOfBattleFormationClassVM in formationItem.Classes)
			{
				orderOfBattleFormationClassVM.IsLocked = false;
				orderOfBattleFormationClassVM.Weight = 0;
				orderOfBattleFormationClassVM.Class = FormationClass.NumberOfAllFormations;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00011E88 File Offset: 0x00010088
		private int GetVisibleTotalTroopCountOfType(FormationClass formationClass)
		{
			return this._visibleTroopTypeCountLookup[formationClass];
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011E96 File Offset: 0x00010096
		private void OnOrderIssued(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM x)
			{
				x.MakeMarkerWorldPositionDirty();
			});
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00011EC2 File Offset: 0x000100C2
		private void OnHeroesChanged()
		{
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.OnSizeChanged();
				f.UpdateAdjustable();
			});
			this.RefreshWeights();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00011EF4 File Offset: 0x000100F4
		public void ExecuteAutoDeploy()
		{
			if (this.IsPlayerGeneral)
			{
				this.BeforeAutoDeploy();
				this._onAutoDeploy();
				this.AfterAutoDeploy();
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00011F15 File Offset: 0x00010115
		private void BeforeAutoDeploy()
		{
			this.ClearHeroItemSelection();
			this.ClearAllHeroAssignments();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00011F24 File Offset: 0x00010124
		private void AfterAutoDeploy()
		{
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				orderOfBattleFormationItemVM.RefreshFormation(orderOfBattleFormationItemVM.Formation, DeploymentFormationClass.Unset, false);
			}
			this.RefreshWeights();
			this.OnUnitDeployed();
			this._allFormations.ForEach(delegate(OrderOfBattleFormationItemVM f)
			{
				f.UpdateAdjustable();
			});
			this._isMissingFormationsDirty = true;
			this._mission.PlayerTeam.TriggerOnFormationsChangedInDeployment();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011FCC File Offset: 0x000101CC
		public void ExecuteBeginMission()
		{
			List<MissionOrderVM.FormationConfiguration> currentConfiguration = this.CurrentConfiguration;
			if (currentConfiguration != null)
			{
				currentConfiguration.Clear();
			}
			foreach (OrderOfBattleFormationItemVM orderOfBattleFormationItemVM in this._allFormations)
			{
				if (orderOfBattleFormationItemVM.Formation.CountOfUnits > 0)
				{
					List<MissionOrderVM.FormationConfiguration> currentConfiguration2 = this.CurrentConfiguration;
					if (currentConfiguration2 != null)
					{
						currentConfiguration2.Add(new MissionOrderVM.FormationConfiguration(orderOfBattleFormationItemVM.Formation.Index, (from f in orderOfBattleFormationItemVM.FilterItems
							where f.IsActive
							select f.FilterType).ToList<FormationFilterType>()));
					}
				}
				else
				{
					List<MissionOrderVM.FormationConfiguration> currentConfiguration3 = this.CurrentConfiguration;
					if (currentConfiguration3 != null)
					{
						currentConfiguration3.Add(new MissionOrderVM.FormationConfiguration(orderOfBattleFormationItemVM.Formation.Index, new List<FormationFilterType>()));
					}
				}
			}
			if (this._bannerBearerLogic != null)
			{
				this._bannerBearerLogic.OnBannerBearersUpdated -= this.OnBannerBearersUpdated;
				this._bannerBearerLogic.OnBannerBearerAgentUpdated -= this.OnBannerAgentUpdated;
			}
			Action onBeginMission = this._onBeginMission;
			if (onBeginMission != null)
			{
				onBeginMission();
			}
			MBInformationManager.HideInformations();
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00012128 File Offset: 0x00010328
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x00012130 File Offset: 0x00010330
		[DataSourceProperty]
		public bool IsPoolAcceptingHeroTroops
		{
			get
			{
				return this._isPoolAcceptingHeroTroops;
			}
			set
			{
				if (value != this._isPoolAcceptingHeroTroops)
				{
					this._isPoolAcceptingHeroTroops = value;
					base.OnPropertyChangedWithValue(value, "IsPoolAcceptingHeroTroops");
					this.IsPoolAcceptingAny = this.IsPoolAcceptingCaptain || this.IsPoolAcceptingHeroTroops;
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00012165 File Offset: 0x00010365
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x0001216D File Offset: 0x0001036D
		[DataSourceProperty]
		public bool CanStartMission
		{
			get
			{
				return this._canStartMission;
			}
			set
			{
				if (value != this._canStartMission)
				{
					this._canStartMission = value;
					base.OnPropertyChangedWithValue(value, "CanStartMission");
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0001218B File Offset: 0x0001038B
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x00012193 File Offset: 0x00010393
		[DataSourceProperty]
		public string BeginMissionText
		{
			get
			{
				return this._beginMissionText;
			}
			set
			{
				if (value != this._beginMissionText)
				{
					this._beginMissionText = value;
					base.OnPropertyChangedWithValue<string>(value, "BeginMissionText");
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x000121B6 File Offset: 0x000103B6
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x000121BE File Offset: 0x000103BE
		[DataSourceProperty]
		public bool HasSelectedHeroes
		{
			get
			{
				return this._hasSelectedHeroes;
			}
			set
			{
				if (value != this._hasSelectedHeroes)
				{
					this._hasSelectedHeroes = value;
					base.OnPropertyChangedWithValue(value, "HasSelectedHeroes");
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x000121DC File Offset: 0x000103DC
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x000121E4 File Offset: 0x000103E4
		[DataSourceProperty]
		public MBBindingList<OrderOfBattleFormationItemVM> FormationsFirstHalf
		{
			get
			{
				return this._formationsFirstHalf;
			}
			set
			{
				if (value != this._formationsFirstHalf)
				{
					this._formationsFirstHalf = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderOfBattleFormationItemVM>>(value, "FormationsFirstHalf");
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00012202 File Offset: 0x00010402
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x0001220A File Offset: 0x0001040A
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00012228 File Offset: 0x00010428
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x00012230 File Offset: 0x00010430
		[DataSourceProperty]
		public bool AreCameraControlsEnabled
		{
			get
			{
				return this._areCameraControlsEnabled;
			}
			set
			{
				if (value != this._areCameraControlsEnabled)
				{
					this._areCameraControlsEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreCameraControlsEnabled");
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0001224E File Offset: 0x0001044E
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x00012256 File Offset: 0x00010456
		[DataSourceProperty]
		public bool IsPlayerGeneral
		{
			get
			{
				return this._isPlayerGeneral;
			}
			set
			{
				if (value != this._isPlayerGeneral)
				{
					this._isPlayerGeneral = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerGeneral");
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00012274 File Offset: 0x00010474
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x0001227C File Offset: 0x0001047C
		[DataSourceProperty]
		public bool IsPoolAcceptingCaptain
		{
			get
			{
				return this._isPoolAcceptingCaptain;
			}
			set
			{
				if (value != this._isPoolAcceptingCaptain)
				{
					this._isPoolAcceptingCaptain = value;
					base.OnPropertyChangedWithValue(value, "IsPoolAcceptingCaptain");
					this.IsPoolAcceptingAny = this.IsPoolAcceptingCaptain || this.IsPoolAcceptingHeroTroops;
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x000122B1 File Offset: 0x000104B1
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x000122B9 File Offset: 0x000104B9
		[DataSourceProperty]
		public bool IsPoolAcceptingAny
		{
			get
			{
				return this._isPoolAcceptingAny;
			}
			set
			{
				if (value != this._isPoolAcceptingAny)
				{
					this._isPoolAcceptingAny = value;
					base.OnPropertyChangedWithValue(value, "IsPoolAcceptingAny");
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x000122D7 File Offset: 0x000104D7
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x000122DF File Offset: 0x000104DF
		[DataSourceProperty]
		public int SelectedHeroCount
		{
			get
			{
				return this._selectedHeroCount;
			}
			set
			{
				if (value != this._selectedHeroCount)
				{
					this._selectedHeroCount = value;
					base.OnPropertyChangedWithValue(value, "SelectedHeroCount");
				}
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x000122FD File Offset: 0x000104FD
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x00012305 File Offset: 0x00010505
		[DataSourceProperty]
		public bool AreHotkeysEnabled
		{
			get
			{
				return this._areHotkeysEnabled;
			}
			set
			{
				if (value != this._areHotkeysEnabled)
				{
					this._areHotkeysEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreHotkeysEnabled");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00012323 File Offset: 0x00010523
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x0001232B File Offset: 0x0001052B
		[DataSourceProperty]
		public HintViewModel ClearSelectionHint
		{
			get
			{
				return this._clearSelectionHint;
			}
			set
			{
				if (value != this._clearSelectionHint)
				{
					this._clearSelectionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ClearSelectionHint");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00012349 File Offset: 0x00010549
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x00012351 File Offset: 0x00010551
		[DataSourceProperty]
		public string AutoDeployText
		{
			get
			{
				return this._autoDeployText;
			}
			set
			{
				if (value != this._autoDeployText)
				{
					this._autoDeployText = value;
					base.OnPropertyChangedWithValue<string>(value, "AutoDeployText");
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00012374 File Offset: 0x00010574
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x0001237C File Offset: 0x0001057C
		[DataSourceProperty]
		public HintViewModel SelectAllHint
		{
			get
			{
				return this._selectAllHint;
			}
			set
			{
				if (value != this._selectAllHint)
				{
					this._selectAllHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SelectAllHint");
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0001239A File Offset: 0x0001059A
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x000123A2 File Offset: 0x000105A2
		[DataSourceProperty]
		public HintViewModel MissingFormationsHint
		{
			get
			{
				return this._missingFormationsHint;
			}
			set
			{
				if (value != this._missingFormationsHint)
				{
					this._missingFormationsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MissingFormationsHint");
				}
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x000123C0 File Offset: 0x000105C0
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x000123C8 File Offset: 0x000105C8
		[DataSourceProperty]
		public OrderOfBattleHeroItemVM LastSelectedHeroItem
		{
			get
			{
				return this._lastSelectedHeroItem;
			}
			set
			{
				if (value != this._lastSelectedHeroItem)
				{
					this._lastSelectedHeroItem = value;
					base.OnPropertyChangedWithValue<OrderOfBattleHeroItemVM>(value, "LastSelectedHeroItem");
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x000123E6 File Offset: 0x000105E6
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x000123EE File Offset: 0x000105EE
		[DataSourceProperty]
		public bool CanToggleHeroSelection
		{
			get
			{
				return this._canToggleHeroSelection;
			}
			set
			{
				if (value != this._canToggleHeroSelection)
				{
					this._canToggleHeroSelection = value;
					base.OnPropertyChangedWithValue(value, "CanToggleHeroSelection");
				}
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001240C File Offset: 0x0001060C
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001241B File Offset: 0x0001061B
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001242A File Offset: 0x0001062A
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00012432 File Offset: 0x00010632
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00012450 File Offset: 0x00010650
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x00012458 File Offset: 0x00010658
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00012476 File Offset: 0x00010676
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0001247E File Offset: 0x0001067E
		[DataSourceProperty]
		public SiegeDeploymentVM SiegeDeployment
		{
			get
			{
				return this._siegeDeployment;
			}
			set
			{
				if (value != this._siegeDeployment)
				{
					this._siegeDeployment = value;
					base.OnPropertyChangedWithValue<SiegeDeploymentVM>(value, "SiegeDeployment");
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001249C File Offset: 0x0001069C
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x000124A4 File Offset: 0x000106A4
		[DataSourceProperty]
		public MBBindingList<OrderOfBattleFormationItemVM> FormationsSecondHalf
		{
			get
			{
				return this._formationsSecondHalf;
			}
			set
			{
				if (value != this._formationsSecondHalf)
				{
					this._formationsSecondHalf = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderOfBattleFormationItemVM>>(value, "FormationsSecondHalf");
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000124C2 File Offset: 0x000106C2
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x000124CA File Offset: 0x000106CA
		[DataSourceProperty]
		public MBBindingList<OrderOfBattleHeroItemVM> UnassignedHeroes
		{
			get
			{
				return this._unassignedHeroes;
			}
			set
			{
				if (value != this._unassignedHeroes)
				{
					this._unassignedHeroes = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderOfBattleHeroItemVM>>(value, "UnassignedHeroes");
				}
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000124E8 File Offset: 0x000106E8
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					if (this._isAssignCaptainHighlightApplied)
					{
						this.SetHighlightEmptyCaptainFormations(false);
						this.SetHighlightMainAgentPortait(false);
						this._isAssignCaptainHighlightApplied = false;
					}
					if (this._isCreateFormationHighlightApplied)
					{
						this.SetHighlightFormationTypeSelection(false);
						this.SetHighlightFormationWeights(false);
						this._isCreateFormationHighlightApplied = false;
					}
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					if (this._latestTutorialElementID == "AssignCaptain" && !this._isAssignCaptainHighlightApplied)
					{
						this.SetHighlightEmptyCaptainFormations(true);
						this.SetHighlightMainAgentPortait(true);
						this._isAssignCaptainHighlightApplied = true;
					}
					if (this._latestTutorialElementID == "CreateFormation" && !this._isCreateFormationHighlightApplied)
					{
						this.SetHighlightFormationTypeSelection(true);
						this.SetHighlightFormationWeights(true);
						this._isCreateFormationHighlightApplied = true;
					}
				}
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000125C0 File Offset: 0x000107C0
		private void SetHighlightMainAgentPortait(bool state)
		{
			for (int i = 0; i < this._allHeroes.Count; i++)
			{
				OrderOfBattleHeroItemVM orderOfBattleHeroItemVM = this._allHeroes[i];
				if (orderOfBattleHeroItemVM.Agent.IsMainAgent)
				{
					orderOfBattleHeroItemVM.IsHighlightActive = state;
					return;
				}
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00012608 File Offset: 0x00010808
		private void SetHighlightEmptyCaptainFormations(bool state)
		{
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations[i];
				if (!state || (!orderOfBattleFormationItemVM.HasCaptain && orderOfBattleFormationItemVM.HasFormation))
				{
					orderOfBattleFormationItemVM.IsCaptainSlotHighlightActive = state;
				}
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00012654 File Offset: 0x00010854
		private void SetHighlightFormationTypeSelection(bool state)
		{
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations[i];
				if (!state || orderOfBattleFormationItemVM.IsAdjustable)
				{
					this._allFormations[i].IsTypeSelectionHighlightActive = state;
				}
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000126A4 File Offset: 0x000108A4
		private void SetHighlightFormationWeights(bool state)
		{
			for (int i = 0; i < this._allFormations.Count; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = this._allFormations[i];
				for (int j = 0; j < orderOfBattleFormationItemVM.Classes.Count; j++)
				{
					orderOfBattleFormationItemVM.Classes[j].IsWeightHighlightActive = state;
				}
			}
		}

		// Token: 0x040001EB RID: 491
		private readonly TextObject _bannerText = new TextObject("{=FvYhaE3z}Banner", null);

		// Token: 0x040001EC RID: 492
		private readonly TextObject _bannerEffectText = new TextObject("{=zjcZZgUY}Banner Effect", null);

		// Token: 0x040001ED RID: 493
		private readonly TextObject _noBannerEquippedText = new TextObject("{=suyl7WWa}No banner equipped", null);

		// Token: 0x040001EE RID: 494
		private readonly TextObject _missingFormationsHintText = new TextObject("{=2AGvFYk9}To start the mission, you need to have at least one formation with {FORMATION_CLASS} class.", null);

		// Token: 0x040001EF RID: 495
		private readonly TextObject _selectAllHintText = new TextObject("{=YwbymaBc}Select all heroes", null);

		// Token: 0x040001F0 RID: 496
		private bool _isSaving;

		// Token: 0x040001F1 RID: 497
		private readonly TextObject _clearSelectionHintText = new TextObject("{=Sbb8YcJM}Deselect all selected heroes", null);

		// Token: 0x040001F2 RID: 498
		private Dictionary<FormationClass, int> _visibleTroopTypeCountLookup;

		// Token: 0x040001F3 RID: 499
		private bool _isUnitDeployRefreshed;

		// Token: 0x040001F4 RID: 500
		private Action<int> _selectFormationAtIndex;

		// Token: 0x040001F5 RID: 501
		private readonly List<OrderOfBattleHeroItemVM> _selectedHeroes;

		// Token: 0x040001F6 RID: 502
		private Action<int> _deselectFormationAtIndex;

		// Token: 0x040001F7 RID: 503
		protected readonly List<OrderOfBattleHeroItemVM> _allHeroes;

		// Token: 0x040001F8 RID: 504
		private List<FormationClass> _availableTroopTypes;

		// Token: 0x040001F9 RID: 505
		private bool _isInitialized;

		// Token: 0x040001FB RID: 507
		protected List<OrderOfBattleFormationItemVM> _allFormations;

		// Token: 0x040001FC RID: 508
		private Action _clearFormationSelection;

		// Token: 0x040001FD RID: 509
		private Action _onAutoDeploy;

		// Token: 0x040001FE RID: 510
		private Action _onBeginMission;

		// Token: 0x040001FF RID: 511
		private Mission _mission;

		// Token: 0x04000200 RID: 512
		private Camera _missionCamera;

		// Token: 0x04000201 RID: 513
		private BannerBearerLogic _bannerBearerLogic;

		// Token: 0x04000202 RID: 514
		private OrderController _orderController;

		// Token: 0x04000203 RID: 515
		private bool _isMissingFormationsDirty;

		// Token: 0x04000204 RID: 516
		private bool _isHeroSelectionDirty;

		// Token: 0x04000205 RID: 517
		private bool _isTroopCountsDirty;

		// Token: 0x04000206 RID: 518
		private OrderOfBattleFormationItemVM _lastEnabledClassSelection;

		// Token: 0x04000207 RID: 519
		private bool _isEnabled;

		// Token: 0x04000208 RID: 520
		private bool _isPlayerGeneral;

		// Token: 0x04000209 RID: 521
		private bool _areCameraControlsEnabled;

		// Token: 0x0400020A RID: 522
		private bool _canStartMission = true;

		// Token: 0x0400020B RID: 523
		private bool _isPoolAcceptingCaptain;

		// Token: 0x0400020C RID: 524
		private bool _isPoolAcceptingHeroTroops;

		// Token: 0x0400020D RID: 525
		private bool _isPoolAcceptingAny;

		// Token: 0x0400020E RID: 526
		private string _beginMissionText;

		// Token: 0x0400020F RID: 527
		private bool _hasSelectedHeroes;

		// Token: 0x04000210 RID: 528
		private int _selectedHeroCount;

		// Token: 0x04000211 RID: 529
		private bool _areHotkeysEnabled = true;

		// Token: 0x04000212 RID: 530
		private SiegeDeploymentVM _siegeDeployment;

		// Token: 0x04000213 RID: 531
		private MBBindingList<OrderOfBattleFormationItemVM> _formationsSecondHalf;

		// Token: 0x04000214 RID: 532
		private HintViewModel _missingFormationsHint;

		// Token: 0x04000215 RID: 533
		private HintViewModel _selectAllHint;

		// Token: 0x04000216 RID: 534
		private HintViewModel _clearSelectionHint;

		// Token: 0x04000217 RID: 535
		private bool _canToggleHeroSelection;

		// Token: 0x04000218 RID: 536
		private string _autoDeployText;

		// Token: 0x04000219 RID: 537
		private MBBindingList<OrderOfBattleHeroItemVM> _unassignedHeroes;

		// Token: 0x0400021A RID: 538
		private OrderOfBattleHeroItemVM _lastSelectedHeroItem;

		// Token: 0x0400021B RID: 539
		private MBBindingList<OrderOfBattleFormationItemVM> _formationsFirstHalf;

		// Token: 0x0400021C RID: 540
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400021D RID: 541
		private InputKeyItemVM _resetInputKey;

		// Token: 0x0400021E RID: 542
		private string _latestTutorialElementID;

		// Token: 0x0400021F RID: 543
		private const string _assignCaptainHighlightID = "AssignCaptain";

		// Token: 0x04000220 RID: 544
		private const string _createFormationHighlightID = "CreateFormation";

		// Token: 0x04000221 RID: 545
		private bool _isAssignCaptainHighlightApplied;

		// Token: 0x04000222 RID: 546
		private bool _isCreateFormationHighlightApplied;
	}
}
