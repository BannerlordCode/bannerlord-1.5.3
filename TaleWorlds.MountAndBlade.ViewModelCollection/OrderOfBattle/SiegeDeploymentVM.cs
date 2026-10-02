using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000039 RID: 57
	public class SiegeDeploymentVM : ViewModel
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x0001279C File Offset: 0x0001099C
		public SiegeDeploymentVM(SiegeDeploymentHandler siegeDeploymentHandler, Camera deploymentCamera, List<DeploymentPoint> deploymentPoints)
		{
			this._siegeDeploymentHandler = siegeDeploymentHandler;
			this._deploymentCamera = deploymentCamera;
			this.DeploymentTargets = new MBBindingList<DeploymentSiegeMachineVM>();
			this.SiegeDeploymentList = new MBBindingList<DeploymentSiegeMachineVM>();
			foreach (DeploymentPoint deploymentPoint in deploymentPoints)
			{
				if (deploymentPoint.DeployableWeapons.Any<SynchedMissionObject>((SynchedMissionObject x) => this._siegeDeploymentHandler.GetMaxDeployableWeaponCountOfPlayer(x.GetType()) > 0))
				{
					DeploymentSiegeMachineVM deploymentSiegeMachineVM = new DeploymentSiegeMachineVM(deploymentPoint, null, this._deploymentCamera, new Action<DeploymentSiegeMachineVM>(this.OnRefreshSelectedDeploymentPoint), new Action<DeploymentPoint>(this.OnEntityHover));
					this.DeploymentTargets.Add(deploymentSiegeMachineVM);
				}
			}
			this.RefreshDeployedWeapons();
			this._siegeDeploymentHandler.OnPlayerSideDeploymentReady += this.RefreshDeployedWeapons;
			this._siegeDeploymentHandler.OnEnemySideDeploymentReady += this.RefreshDeployedWeapons;
			this.RefreshValues();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00012894 File Offset: 0x00010A94
		public override void RefreshValues()
		{
			base.RefreshValues();
			MBBindingList<DeploymentSiegeMachineVM> deploymentTargets = this._deploymentTargets;
			if (deploymentTargets != null)
			{
				deploymentTargets.ApplyActionOnAllItems(delegate(DeploymentSiegeMachineVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<DeploymentSiegeMachineVM> siegeDeploymentList = this._siegeDeploymentList;
			if (siegeDeploymentList == null)
			{
				return;
			}
			siegeDeploymentList.ApplyActionOnAllItems(delegate(DeploymentSiegeMachineVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00012908 File Offset: 0x00010B08
		public void Update()
		{
			this.IsSiegeDeploymentDisabled = Mission.Current.IsOrderMenuOpen;
			for (int i = 0; i < this.DeploymentTargets.Count; i++)
			{
				this.DeploymentTargets[i].Update();
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001294C File Offset: 0x00010B4C
		public void AutoDeploySiegeMachines()
		{
			this.IsSiegeDeploymentListActive = false;
			foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
			{
				if (!(deploymentSiegeMachineVM.MachineType != null))
				{
					deploymentSiegeMachineVM.ExecuteAction();
					DeploymentSiegeMachineVM deploymentSiegeMachineVM2 = this.SiegeDeploymentList.FirstOrDefault<DeploymentSiegeMachineVM>((DeploymentSiegeMachineVM d) => d.Machine != null && d.RemainingCount > 0);
					if (deploymentSiegeMachineVM2 != null)
					{
						deploymentSiegeMachineVM2.ExecuteAction();
					}
				}
			}
			this.IsSiegeDeploymentListActive = false;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000129EC File Offset: 0x00010BEC
		public bool HasUndeployedSiegeMachines()
		{
			return this._siegeDeploymentHandler.PlayerDeploymentPoints.Any<DeploymentPoint>((DeploymentPoint d) => !d.IsDeployed && d.DeployableWeaponTypes.Any<Type>((Type type) => this._siegeDeploymentHandler.GetDeployableWeaponCountOfPlayer(type) > 0));
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00012A0C File Offset: 0x00010C0C
		public void OnDeploymentFinalized()
		{
			this.DeploymentTargets.Clear();
			this.SiegeDeploymentList.Clear();
			GameEntity currentSelectedEntity = this._currentSelectedEntity;
			if (currentSelectedEntity != null)
			{
				currentSelectedEntity.SetContourColor(null, true);
			}
			this._currentSelectedEntity = null;
			this._currentHoveredEntity = null;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00012A58 File Offset: 0x00010C58
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._siegeDeploymentHandler.OnPlayerSideDeploymentReady -= this.RefreshDeployedWeapons;
			this._siegeDeploymentHandler.OnEnemySideDeploymentReady -= this.RefreshDeployedWeapons;
			this.SiegeDeploymentList.Clear();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00012AA4 File Offset: 0x00010CA4
		public void OnEntityHover(DeploymentPoint deploymentPoint)
		{
			if (this._currentSelectedEntity != this._currentHoveredEntity)
			{
				GameEntity currentHoveredEntity = this._currentHoveredEntity;
				if (currentHoveredEntity != null)
				{
					currentHoveredEntity.SetContourColor(null, true);
				}
			}
			if (deploymentPoint != null)
			{
				this._currentHoveredEntity = GameEntity.CreateFromWeakEntity(deploymentPoint.IsDeployed ? deploymentPoint.DeployedWeapon.GameEntity : deploymentPoint.GameEntity);
			}
			else
			{
				this._currentHoveredEntity = null;
			}
			if (this._currentSelectedEntity != this._currentHoveredEntity)
			{
				GameEntity currentHoveredEntity2 = this._currentHoveredEntity;
				if (currentHoveredEntity2 == null)
				{
					return;
				}
				currentHoveredEntity2.SetContourColor(new uint?(4289622555U), true);
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00012B3F File Offset: 0x00010D3F
		public void OnRefreshSelectedDeploymentPoint(DeploymentSiegeMachineVM item)
		{
			DeploymentPoint deploymentPoint = item.DeploymentPoint;
			DeploymentSiegeMachineVM selectedDeploymentPoint = this.SelectedDeploymentPoint;
			if (deploymentPoint == ((selectedDeploymentPoint != null) ? selectedDeploymentPoint.DeploymentPoint : null))
			{
				this.ExecuteCancelSelectedDeploymentPoint();
				return;
			}
			this.RefreshSelectedDeploymentPoint(item.DeploymentPoint);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00012B70 File Offset: 0x00010D70
		public void RefreshSelectedDeploymentPoint(DeploymentPoint selectedDeploymentPoint)
		{
			this.IsSiegeDeploymentListActive = false;
			foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
			{
				if (deploymentSiegeMachineVM.DeploymentPoint == selectedDeploymentPoint)
				{
					this.SelectedDeploymentPoint = deploymentSiegeMachineVM;
				}
			}
			if (!this.SelectedDeploymentPoint.IsSelected)
			{
				this.SelectedDeploymentPoint.IsSelected = true;
			}
			this.SiegeDeploymentList.Clear();
			DeploymentSiegeMachineVM deploymentSiegeMachineVM2;
			foreach (SynchedMissionObject synchedMissionObject in selectedDeploymentPoint.DeployableWeapons)
			{
				Type type = synchedMissionObject.GetType();
				if (this._siegeDeploymentHandler.GetMaxDeployableWeaponCountOfPlayer(type) > 0)
				{
					deploymentSiegeMachineVM2 = new DeploymentSiegeMachineVM(selectedDeploymentPoint, synchedMissionObject as SiegeWeapon, this._deploymentCamera, new Action<DeploymentSiegeMachineVM>(this.OnSelectDeploymentSiegeMachine), null);
					this.SiegeDeploymentList.Add(deploymentSiegeMachineVM2);
					deploymentSiegeMachineVM2.RemainingCount = this._siegeDeploymentHandler.GetDeployableWeaponCountOfPlayer(type);
				}
			}
			deploymentSiegeMachineVM2 = new DeploymentSiegeMachineVM(selectedDeploymentPoint, null, this._deploymentCamera, new Action<DeploymentSiegeMachineVM>(this.OnSelectDeploymentSiegeMachine), null);
			this.SiegeDeploymentList.Add(deploymentSiegeMachineVM2);
			selectedDeploymentPoint.GameEntity.SetContourColor(new uint?(4293481743U), true);
			this.IsSiegeDeploymentListActive = true;
			GameEntity currentSelectedEntity = this._currentSelectedEntity;
			if (currentSelectedEntity != null)
			{
				currentSelectedEntity.SetContourColor(null, true);
			}
			this._currentSelectedEntity = GameEntity.CreateFromWeakEntity(selectedDeploymentPoint.GameEntity);
			GameEntity currentSelectedEntity2 = this._currentSelectedEntity;
			if (currentSelectedEntity2 == null)
			{
				return;
			}
			currentSelectedEntity2.SetContourColor(new uint?(4293481743U), true);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00012D10 File Offset: 0x00010F10
		public void ExecuteCancelSelectedDeploymentPoint()
		{
			this.OnSelectDeploymentSiegeMachine(null);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00012D1C File Offset: 0x00010F1C
		private void OnSelectDeploymentSiegeMachine(DeploymentSiegeMachineVM item)
		{
			this.IsSiegeDeploymentListActive = false;
			GameEntity currentSelectedEntity = this._currentSelectedEntity;
			if (currentSelectedEntity != null)
			{
				currentSelectedEntity.SetContourColor(null, true);
			}
			this._currentSelectedEntity = null;
			this.SelectedDeploymentPoint = null;
			this.SiegeDeploymentList.Clear();
			if (item != null && (!(item.MachineType != null) || this._siegeDeploymentHandler.GetDeployableWeaponCountOfPlayer(item.MachineType) != 0) && (item.DeploymentPoint.DeployedWeapon == null || !(item.DeploymentPoint.DeployedWeapon.GetType() == item.MachineType)))
			{
				bool flag = !item.DeploymentPoint.IsDeployed || item.DeploymentPoint.DeployedWeapon != item.SiegeWeapon;
				if (item.DeploymentPoint.IsDeployed)
				{
					if (item.SiegeWeapon == null)
					{
						SoundEvent.PlaySound2D("event:/ui/dropdown");
					}
					item.DeploymentPoint.Disband();
				}
				if (flag && item.SiegeWeapon != null)
				{
					SiegeEngineType machine = item.Machine;
					if (machine == DefaultSiegeEngineTypes.Catapult || machine == DefaultSiegeEngineTypes.FireCatapult || machine == DefaultSiegeEngineTypes.Onager || machine == DefaultSiegeEngineTypes.FireOnager)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/catapult");
					}
					else if (machine == DefaultSiegeEngineTypes.Ram)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/batteringram");
					}
					else if (machine == DefaultSiegeEngineTypes.SiegeTower)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/siegetower");
					}
					else if (machine == DefaultSiegeEngineTypes.Trebuchet || machine == DefaultSiegeEngineTypes.Bricole)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/catapult");
					}
					else if (machine == DefaultSiegeEngineTypes.Ballista || machine == DefaultSiegeEngineTypes.FireBallista)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/ballista");
					}
					item.DeploymentPoint.Deploy(item.SiegeWeapon);
				}
			}
			this.RefreshDeployedWeapons();
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00012ECC File Offset: 0x000110CC
		private void RefreshDeployedWeapons()
		{
			foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
			{
				deploymentSiegeMachineVM.RefreshWithDeployedWeapon();
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00012F18 File Offset: 0x00011118
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00012F20 File Offset: 0x00011120
		[DataSourceProperty]
		public MBBindingList<DeploymentSiegeMachineVM> DeploymentTargets
		{
			get
			{
				return this._deploymentTargets;
			}
			set
			{
				if (value != this._deploymentTargets)
				{
					this._deploymentTargets = value;
					base.OnPropertyChanged("DeploymentTargets");
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00012F3D File Offset: 0x0001113D
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00012F45 File Offset: 0x00011145
		[DataSourceProperty]
		public MBBindingList<DeploymentSiegeMachineVM> SiegeDeploymentList
		{
			get
			{
				return this._siegeDeploymentList;
			}
			set
			{
				if (value != this._siegeDeploymentList)
				{
					this._siegeDeploymentList = value;
					base.OnPropertyChanged("SiegeDeploymentList");
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00012F62 File Offset: 0x00011162
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00012F6A File Offset: 0x0001116A
		[DataSourceProperty]
		public DeploymentSiegeMachineVM SelectedDeploymentPoint
		{
			get
			{
				return this._selectedDeploymentPoint;
			}
			set
			{
				if (value != this._selectedDeploymentPoint)
				{
					this._selectedDeploymentPoint = value;
					base.OnPropertyChanged("SelectedDeploymentPoint");
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00012F87 File Offset: 0x00011187
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00012F8F File Offset: 0x0001118F
		[DataSourceProperty]
		public bool IsSiegeDeploymentDisabled
		{
			get
			{
				return this._isSiegeDeploymentDisabled;
			}
			set
			{
				if (value != this._isSiegeDeploymentDisabled)
				{
					this._isSiegeDeploymentDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsSiegeDeploymentDisabled");
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00012FAD File Offset: 0x000111AD
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00012FB5 File Offset: 0x000111B5
		[DataSourceProperty]
		public bool IsSiegeDeploymentListActive
		{
			get
			{
				return this._isSiegeDeploymentListActive;
			}
			set
			{
				if (value != this._isSiegeDeploymentListActive)
				{
					this._isSiegeDeploymentListActive = value;
					base.OnPropertyChanged("IsSiegeDeploymentListActive");
					if (this.SelectedDeploymentPoint != null)
					{
						this.SelectedDeploymentPoint.IsSelected = value;
					}
				}
			}
		}

		// Token: 0x04000227 RID: 551
		public const uint EntityHighlightColor = 4289622555U;

		// Token: 0x04000228 RID: 552
		public const uint EntitySelectedColor = 4293481743U;

		// Token: 0x04000229 RID: 553
		private GameEntity _currentSelectedEntity;

		// Token: 0x0400022A RID: 554
		private GameEntity _currentHoveredEntity;

		// Token: 0x0400022B RID: 555
		private readonly SiegeDeploymentHandler _siegeDeploymentHandler;

		// Token: 0x0400022C RID: 556
		private readonly Camera _deploymentCamera;

		// Token: 0x0400022D RID: 557
		private MBBindingList<DeploymentSiegeMachineVM> _deploymentTargets;

		// Token: 0x0400022E RID: 558
		private MBBindingList<DeploymentSiegeMachineVM> _siegeDeploymentList;

		// Token: 0x0400022F RID: 559
		private DeploymentSiegeMachineVM _selectedDeploymentPoint;

		// Token: 0x04000230 RID: 560
		private bool _isSiegeDeploymentListActive;

		// Token: 0x04000231 RID: 561
		private bool _isSiegeDeploymentDisabled;
	}
}
