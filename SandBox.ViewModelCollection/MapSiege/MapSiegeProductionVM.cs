using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000053 RID: 83
	public class MapSiegeProductionVM : ViewModel
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x000143B0 File Offset: 0x000125B0
		private SiegeEvent Siege
		{
			get
			{
				return PlayerSiege.PlayerSiegeEvent;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x000143B7 File Offset: 0x000125B7
		private BattleSideEnum PlayerSide
		{
			get
			{
				return PlayerSiege.PlayerSide;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x000143BE File Offset: 0x000125BE
		private Settlement Settlement
		{
			get
			{
				return this.Siege.BesiegedSettlement;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x000143CB File Offset: 0x000125CB
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x000143D3 File Offset: 0x000125D3
		public MapSiegePOIVM LatestSelectedPOI { get; private set; }

		// Token: 0x06000549 RID: 1353 RVA: 0x000143DC File Offset: 0x000125DC
		public MapSiegeProductionVM()
		{
			this.PossibleProductionMachines = new MBBindingList<MapSiegeProductionMachineVM>();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000143EF File Offset: 0x000125EF
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PossibleProductionMachines.ApplyActionOnAllItems(delegate(MapSiegeProductionMachineVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014424 File Offset: 0x00012624
		public void Update()
		{
			if (this.IsEnabled && this.LatestSelectedPOI.Machine == null)
			{
				if (this.PossibleProductionMachines.Any<MapSiegeProductionMachineVM>((MapSiegeProductionMachineVM m) => m.IsReserveOption))
				{
					this.ExecuteDisable();
				}
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00014478 File Offset: 0x00012678
		public void OnMachineSelection(MapSiegePOIVM poi)
		{
			this.PossibleProductionMachines.Clear();
			this.LatestSelectedPOI = poi;
			MapSiegePOIVM latestSelectedPOI = this.LatestSelectedPOI;
			if (((latestSelectedPOI != null) ? latestSelectedPOI.Machine : null) != null)
			{
				this.PossibleProductionMachines.Add(new MapSiegeProductionMachineVM(new Action<MapSiegeProductionMachineVM>(this.OnPossibleMachineSelection), !this.LatestSelectedPOI.Machine.IsActive && !this.LatestSelectedPOI.Machine.IsBeingRedeployed));
			}
			else
			{
				IEnumerable<SiegeEngineType> enumerable;
				switch (poi.Type)
				{
				case MapSiegePOIVM.POIType.DefenderSiegeMachine:
					enumerable = this.GetAllDefenderMachines();
					goto IL_00BE;
				case MapSiegePOIVM.POIType.AttackerRamSiegeMachine:
					enumerable = this.GetAllAttackerRamMachines();
					goto IL_00BE;
				case MapSiegePOIVM.POIType.AttackerTowerSiegeMachine:
					enumerable = this.GetAllAttackerTowerMachines();
					goto IL_00BE;
				case MapSiegePOIVM.POIType.AttackerRangedSiegeMachine:
					enumerable = this.GetAllAttackerRangedMachines();
					goto IL_00BE;
				}
				this.IsEnabled = false;
				return;
				IL_00BE:
				using (IEnumerator<SiegeEngineType> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SiegeEngineType desMachine = enumerator.Current;
						int num = this.Siege.GetSiegeEventSide(this.PlayerSide).SiegeEngines.ReservedSiegeEngines.Count<SiegeEvent.SiegeEngineConstructionProgress>((SiegeEvent.SiegeEngineConstructionProgress m) => m.SiegeEngine == desMachine);
						this.PossibleProductionMachines.Add(new MapSiegeProductionMachineVM(desMachine, num, new Action<MapSiegeProductionMachineVM>(this.OnPossibleMachineSelection)));
					}
				}
			}
			this.IsEnabled = true;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000145DC File Offset: 0x000127DC
		private void OnPossibleMachineSelection(MapSiegeProductionMachineVM machine)
		{
			if (this.LatestSelectedPOI.Machine == null || this.LatestSelectedPOI.Machine.SiegeEngine != machine.Engine)
			{
				ISiegeEventSide siegeEventSide = this.Siege.GetSiegeEventSide(this.PlayerSide);
				if (machine.IsReserveOption && this.LatestSelectedPOI.Machine != null)
				{
					bool flag = this.LatestSelectedPOI.Machine.IsActive || this.LatestSelectedPOI.Machine.IsBeingRedeployed;
					siegeEventSide.SiegeEngines.RemoveDeployedSiegeEngine(this.LatestSelectedPOI.MachineIndex, this.LatestSelectedPOI.Machine.SiegeEngine.IsRanged, flag);
				}
				else
				{
					SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress = siegeEventSide.SiegeEngines.ReservedSiegeEngines.FirstOrDefault<SiegeEvent.SiegeEngineConstructionProgress>((SiegeEvent.SiegeEngineConstructionProgress e) => e.SiegeEngine == machine.Engine);
					if (siegeEngineConstructionProgress == null)
					{
						float siegeEngineHitPoints = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineHitPoints(PlayerSiege.PlayerSiegeEvent, machine.Engine, this.PlayerSide);
						siegeEngineConstructionProgress = new SiegeEvent.SiegeEngineConstructionProgress(machine.Engine, 0f, siegeEngineHitPoints);
					}
					if (siegeEventSide.SiegeStrategy != DefaultSiegeStrategies.Custom && Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(this.Siege, siegeEventSide.BattleSide) == Hero.MainHero)
					{
						siegeEventSide.SetSiegeStrategy(DefaultSiegeStrategies.Custom);
					}
					siegeEventSide.SiegeEngines.DeploySiegeEngineAtIndex(siegeEngineConstructionProgress, this.LatestSelectedPOI.MachineIndex);
				}
				this.Siege.BesiegedSettlement.Party.SetVisualAsDirty();
				Game.Current.EventManager.TriggerEvent<PlayerStartEngineConstructionEvent>(new PlayerStartEngineConstructionEvent(machine.Engine));
			}
			this.IsEnabled = false;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001479E File Offset: 0x0001299E
		public void ExecuteDisable()
		{
			this.IsEnabled = false;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000147A7 File Offset: 0x000129A7
		private IEnumerable<SiegeEngineType> GetAllDefenderMachines()
		{
			return Campaign.Current.Models.SiegeEventModel.GetAvailableDefenderSiegeEngines(PartyBase.MainParty);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000147C2 File Offset: 0x000129C2
		private IEnumerable<SiegeEngineType> GetAllAttackerRangedMachines()
		{
			return Campaign.Current.Models.SiegeEventModel.GetAvailableAttackerRangedSiegeEngines(PartyBase.MainParty);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000147DD File Offset: 0x000129DD
		private IEnumerable<SiegeEngineType> GetAllAttackerRamMachines()
		{
			return Campaign.Current.Models.SiegeEventModel.GetAvailableAttackerRamSiegeEngines(PartyBase.MainParty);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000147F8 File Offset: 0x000129F8
		private IEnumerable<SiegeEngineType> GetAllAttackerTowerMachines()
		{
			return Campaign.Current.Models.SiegeEventModel.GetAvailableAttackerTowerSiegeEngines(PartyBase.MainParty);
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00014813 File Offset: 0x00012A13
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x0001481B File Offset: 0x00012A1B
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00014839 File Offset: 0x00012A39
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00014841 File Offset: 0x00012A41
		[DataSourceProperty]
		public MBBindingList<MapSiegeProductionMachineVM> PossibleProductionMachines
		{
			get
			{
				return this._possibleProductionMachines;
			}
			set
			{
				if (value != this._possibleProductionMachines)
				{
					this._possibleProductionMachines = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapSiegeProductionMachineVM>>(value, "PossibleProductionMachines");
				}
			}
		}

		// Token: 0x040002B0 RID: 688
		private MBBindingList<MapSiegeProductionMachineVM> _possibleProductionMachines;

		// Token: 0x040002B1 RID: 689
		private bool _isEnabled;
	}
}
