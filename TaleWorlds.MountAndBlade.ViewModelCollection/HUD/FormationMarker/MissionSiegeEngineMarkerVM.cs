using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000063 RID: 99
	public class MissionSiegeEngineMarkerVM : ViewModel
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001B54B File Offset: 0x0001974B
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0001B553 File Offset: 0x00019753
		public bool IsInitialized { get; private set; }

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001B55C File Offset: 0x0001975C
		public MissionSiegeEngineMarkerVM(Mission mission, Camera missionCamera)
		{
			this._mission = mission;
			this._missionCamera = missionCamera;
			this._comparer = new MissionSiegeEngineMarkerVM.SiegeEngineMarkerDistanceComparer();
			this.Targets = new MBBindingList<MissionSiegeEngineMarkerTargetVM>();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001B5B4 File Offset: 0x000197B4
		public void InitializeWith(List<SiegeWeapon> siegeEngines)
		{
			this._siegeEngines = siegeEngines;
			for (int i = 0; i < this._siegeEngines.Count; i++)
			{
				SiegeWeapon engine = this._siegeEngines[i];
				BattleSideEnum side = this._mission.PlayerTeam.Side;
				if (!this.Targets.Any<MissionSiegeEngineMarkerTargetVM>((MissionSiegeEngineMarkerTargetVM t) => t.Engine == engine))
				{
					MissionSiegeEngineMarkerTargetVM missionSiegeEngineMarkerTargetVM = new MissionSiegeEngineMarkerTargetVM(engine, engine.Side != side);
					this.Targets.Add(missionSiegeEngineMarkerTargetVM);
					missionSiegeEngineMarkerTargetVM.IsEnabled = this.IsEnabled;
				}
			}
			this.IsInitialized = true;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001B660 File Offset: 0x00019860
		public void Tick(float dt)
		{
			if (this._siegeEngines != null)
			{
				if (this.IsEnabled)
				{
					this.RefreshSiegeEngineList();
					this.RefreshSiegeEnginePositions();
					this.RefreshSiegeEngineItemProperties();
					this.SortMarkersInList();
					this._fadeOutTimerStarted = false;
					this._fadeOutTimer = 0f;
					this._prevIsEnabled = this.IsEnabled;
				}
				else
				{
					if (this._prevIsEnabled)
					{
						this._fadeOutTimerStarted = true;
					}
					if (this._fadeOutTimerStarted)
					{
						this._fadeOutTimer += dt;
					}
					if (this._fadeOutTimer < 2f)
					{
						this.RefreshSiegeEnginePositions();
					}
					else
					{
						this._fadeOutTimerStarted = false;
					}
				}
				this._prevIsEnabled = this.IsEnabled;
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001B708 File Offset: 0x00019908
		private void RefreshSiegeEngineList()
		{
			bool isDefender = this._mission.PlayerTeam.IsDefender;
			for (int i = this._siegeEngines.Count - 1; i >= 0; i--)
			{
				SiegeWeapon engine = this._siegeEngines[i];
				if (engine.DestructionComponent.IsDestroyed)
				{
					this._siegeEngines.RemoveAt(i);
					MissionSiegeEngineMarkerTargetVM missionSiegeEngineMarkerTargetVM = this.Targets.SingleOrDefault<MissionSiegeEngineMarkerTargetVM>((MissionSiegeEngineMarkerTargetVM t) => t.Engine == engine);
					this.Targets.Remove(missionSiegeEngineMarkerTargetVM);
				}
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001B79C File Offset: 0x0001999C
		private void RefreshSiegeEnginePositions()
		{
			foreach (MissionSiegeEngineMarkerTargetVM missionSiegeEngineMarkerTargetVM in this.Targets)
			{
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				Vec3 globalPosition = missionSiegeEngineMarkerTargetVM.Engine.GameEntity.GlobalPosition;
				MBWindowManager.WorldToScreenInsideUsableArea(this._missionCamera, globalPosition + this._heightOffset, ref num, ref num2, ref num3);
				if (num3 < 0f || !MathF.IsValidValue(num) || !MathF.IsValidValue(num2))
				{
					num = -10000f;
					num2 = -10000f;
					num3 = 0f;
				}
				if (this._prevIsEnabled && this.IsEnabled)
				{
					missionSiegeEngineMarkerTargetVM.ScreenPosition = Vec2.Lerp(missionSiegeEngineMarkerTargetVM.ScreenPosition, new Vec2(num, num2), 0.9f);
				}
				else
				{
					missionSiegeEngineMarkerTargetVM.ScreenPosition = new Vec2(num, num2);
				}
				MissionSiegeEngineMarkerTargetVM missionSiegeEngineMarkerTargetVM2 = missionSiegeEngineMarkerTargetVM;
				Agent main = Agent.Main;
				missionSiegeEngineMarkerTargetVM2.Distance = ((main != null && main.IsActive()) ? Agent.Main.Position.Distance(globalPosition) : num3);
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001B8CC File Offset: 0x00019ACC
		private void SortMarkersInList()
		{
			this.Targets.Sort(this._comparer);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001B8E0 File Offset: 0x00019AE0
		private void RefreshSiegeEngineItemProperties()
		{
			foreach (MissionSiegeEngineMarkerTargetVM missionSiegeEngineMarkerTargetVM in this.Targets)
			{
				missionSiegeEngineMarkerTargetVM.Refresh();
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001B92C File Offset: 0x00019B2C
		private void UpdateTargetStates(bool isEnabled)
		{
			foreach (MissionSiegeEngineMarkerTargetVM missionSiegeEngineMarkerTargetVM in this.Targets)
			{
				missionSiegeEngineMarkerTargetVM.IsEnabled = isEnabled;
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001B978 File Offset: 0x00019B78
		public override void OnFinalize()
		{
			base.OnFinalize();
			List<SiegeWeapon> siegeEngines = this._siegeEngines;
			if (siegeEngines != null)
			{
				siegeEngines.Clear();
			}
			this._siegeEngines = null;
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001B998 File Offset: 0x00019B98
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0001B9A0 File Offset: 0x00019BA0
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
					this.UpdateTargetStates(value);
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0001B9C5 File Offset: 0x00019BC5
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0001B9CD File Offset: 0x00019BCD
		[DataSourceProperty]
		public MBBindingList<MissionSiegeEngineMarkerTargetVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionSiegeEngineMarkerTargetVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x0400038D RID: 909
		private Mission _mission;

		// Token: 0x0400038E RID: 910
		private Camera _missionCamera;

		// Token: 0x0400038F RID: 911
		private List<SiegeWeapon> _siegeEngines;

		// Token: 0x04000390 RID: 912
		private Vec3 _heightOffset = new Vec3(0f, 0f, 3f, -1f);

		// Token: 0x04000391 RID: 913
		private bool _prevIsEnabled;

		// Token: 0x04000392 RID: 914
		private MissionSiegeEngineMarkerVM.SiegeEngineMarkerDistanceComparer _comparer;

		// Token: 0x04000393 RID: 915
		private bool _fadeOutTimerStarted;

		// Token: 0x04000394 RID: 916
		private float _fadeOutTimer;

		// Token: 0x04000396 RID: 918
		private bool _isEnabled;

		// Token: 0x04000397 RID: 919
		private MBBindingList<MissionSiegeEngineMarkerTargetVM> _targets;

		// Token: 0x020000F6 RID: 246
		public class SiegeEngineMarkerDistanceComparer : IComparer<MissionSiegeEngineMarkerTargetVM>
		{
			// Token: 0x06000D23 RID: 3363 RVA: 0x00029FD0 File Offset: 0x000281D0
			public int Compare(MissionSiegeEngineMarkerTargetVM x, MissionSiegeEngineMarkerTargetVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
