using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AC RID: 172
	public class SettlementProjectSelectionVM : ViewModel
	{
		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x000427E6 File Offset: 0x000409E6
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x000427EE File Offset: 0x000409EE
		public List<Building> LocalDevelopmentList { get; private set; }

		// Token: 0x06001028 RID: 4136 RVA: 0x000427F7 File Offset: 0x000409F7
		public SettlementProjectSelectionVM(Settlement settlement, Action onAnyChangeInQueue)
		{
			this._settlement = settlement;
			this._town = settlement.Town;
			this._onAnyChangeInQueue = onAnyChangeInQueue;
			this.RefreshValues();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00042820 File Offset: 0x00040A20
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ProjectsText = new TextObject("{=LpsoPtOo}Projects", null).ToString();
			this.DailyDefaultsText = GameTexts.FindText("str_town_management_daily_defaults", null).ToString();
			this.DailyDefaultsExplanationText = GameTexts.FindText("str_town_management_daily_defaults_explanation", null).ToString();
			this.QueueText = GameTexts.FindText("str_town_management_queue", null).ToString();
			this.Refresh();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00042894 File Offset: 0x00040A94
		public void Refresh()
		{
			this.AvailableProjects = new MBBindingList<SettlementBuildingProjectVM>();
			this.DailyDefaultList = new MBBindingList<SettlementDailyProjectVM>();
			this.LocalDevelopmentList = new List<Building>();
			this.CurrentDevelopmentQueue = new MBBindingList<SettlementBuildingProjectVM>();
			this.AvailableProjects.Clear();
			for (int i = 0; i < this._town.Buildings.Count; i++)
			{
				Building building = this._town.Buildings[i];
				if (!building.BuildingType.IsDailyProject)
				{
					SettlementBuildingProjectVM settlementBuildingProjectVM = new SettlementBuildingProjectVM(new Action<SettlementProjectVM, bool>(this.OnCurrentProjectSelection), new Action<SettlementProjectVM>(this.OnCurrentProjectSet), new Action(this.OnResetCurrentProject), building, this._settlement);
					this.AvailableProjects.Add(settlementBuildingProjectVM);
				}
				else
				{
					SettlementDailyProjectVM settlementDailyProjectVM = new SettlementDailyProjectVM(new Action<SettlementProjectVM, bool>(this.OnCurrentProjectSelection), new Action<SettlementProjectVM>(this.OnCurrentProjectSet), new Action(this.OnResetCurrentProject), building, this._settlement);
					this.DailyDefaultList.Add(settlementDailyProjectVM);
					if (settlementDailyProjectVM.Building == this._town.Buildings.FirstOrDefault<Building>((Building k) => k.IsCurrentlyDefault))
					{
						this.CurrentDailyDefault = settlementDailyProjectVM;
					}
				}
			}
			foreach (Building building2 in this._town.BuildingsInProgress)
			{
				this.LocalDevelopmentList.Add(building2);
			}
			this.RefreshDevelopmentsQueueIndex();
			this.RefreshCurrentSelectedProject();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00042A34 File Offset: 0x00040C34
		private void OnCurrentProjectSet(SettlementProjectVM selectedItem)
		{
			this.CurrentSelectedProject = selectedItem;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00042A40 File Offset: 0x00040C40
		private void OnCurrentProjectSelection(SettlementProjectVM selectedItem, bool isSetAsActiveDevelopment)
		{
			if (!selectedItem.IsDaily)
			{
				if (isSetAsActiveDevelopment)
				{
					if (this.LocalDevelopmentList.Exists((Building d) => d == selectedItem.Building))
					{
						int num = this.LocalDevelopmentList.IndexOf(selectedItem.Building) - 1;
						while (0 <= num)
						{
							this.LocalDevelopmentList[num + 1] = this.LocalDevelopmentList[num];
							num--;
						}
						this.LocalDevelopmentList.RemoveAt(0);
					}
					this.LocalDevelopmentList.Insert(0, selectedItem.Building);
				}
				else if (this.LocalDevelopmentList.Exists((Building d) => d == selectedItem.Building))
				{
					this.LocalDevelopmentList.Remove(selectedItem.Building);
				}
				else
				{
					this.LocalDevelopmentList.Add(selectedItem.Building);
				}
			}
			else
			{
				this.CurrentDailyDefault = selectedItem as SettlementDailyProjectVM;
			}
			this.RefreshDevelopmentsQueueIndex();
			this.RefreshCurrentSelectedProject();
			Action onAnyChangeInQueue = this._onAnyChangeInQueue;
			if (onAnyChangeInQueue == null)
			{
				return;
			}
			onAnyChangeInQueue();
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00042B5F File Offset: 0x00040D5F
		private void OnResetCurrentProject()
		{
			this.RefreshCurrentSelectedProject();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00042B68 File Offset: 0x00040D68
		private void RefreshCurrentSelectedProject()
		{
			if (this.LocalDevelopmentList.Count > 0)
			{
				for (int i = 0; i < this.AvailableProjects.Count; i++)
				{
					SettlementBuildingProjectVM settlementBuildingProjectVM = this.AvailableProjects[i];
					if (settlementBuildingProjectVM.Building == this.LocalDevelopmentList[0])
					{
						this.CurrentSelectedProject = settlementBuildingProjectVM;
						return;
					}
				}
				return;
			}
			this.CurrentSelectedProject = this.CurrentDailyDefault;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00042BD0 File Offset: 0x00040DD0
		private void RefreshDevelopmentsQueueIndex()
		{
			this.CurrentDevelopmentQueue = new MBBindingList<SettlementBuildingProjectVM>();
			using (IEnumerator<SettlementBuildingProjectVM> enumerator = this.AvailableProjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SettlementBuildingProjectVM item = enumerator.Current;
					item.DevelopmentQueueIndex = -1;
					item.IsInQueue = this.LocalDevelopmentList.Any<Building>((Building d) => d.BuildingType == item.Building.BuildingType);
					item.IsCurrentActiveProject = false;
					if (item.IsInQueue)
					{
						int num = this.LocalDevelopmentList.IndexOf(item.Building);
						item.DevelopmentQueueIndex = num;
						if (num == 0)
						{
							item.IsCurrentActiveProject = true;
						}
						this.CurrentDevelopmentQueue.Add(item);
					}
					item.RefreshProductionText();
				}
			}
			Comparer<SettlementBuildingProjectVM> comparer = Comparer<SettlementBuildingProjectVM>.Create((SettlementBuildingProjectVM s1, SettlementBuildingProjectVM s2) => s1.DevelopmentQueueIndex.CompareTo(s2.DevelopmentQueueIndex));
			this.CurrentDevelopmentQueue.Sort(comparer);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00042CF8 File Offset: 0x00040EF8
		public void ExecuteChangeQueueOrder(SettlementBuildingProjectVM project, int index, string targetTag)
		{
			if (index == project.DevelopmentQueueIndex || targetTag != "CurrentDevelopmentQueue")
			{
				return;
			}
			this.LocalDevelopmentList.Remove(project.Building);
			if (index > project.DevelopmentQueueIndex)
			{
				this.LocalDevelopmentList.Insert(index - 1, project.Building);
			}
			else
			{
				this.LocalDevelopmentList.Insert(index, project.Building);
			}
			this.RefreshDevelopmentsQueueIndex();
			this.RefreshCurrentSelectedProject();
			Action onAnyChangeInQueue = this._onAnyChangeInQueue;
			if (onAnyChangeInQueue == null)
			{
				return;
			}
			onAnyChangeInQueue();
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x00042D7B File Offset: 0x00040F7B
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x00042D83 File Offset: 0x00040F83
		[DataSourceProperty]
		public string ProjectsText
		{
			get
			{
				return this._projectsText;
			}
			set
			{
				if (value != this._projectsText)
				{
					this._projectsText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProjectsText");
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00042DA6 File Offset: 0x00040FA6
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x00042DAE File Offset: 0x00040FAE
		[DataSourceProperty]
		public string QueueText
		{
			get
			{
				return this._queueText;
			}
			set
			{
				if (value != this._queueText)
				{
					this._queueText = value;
					base.OnPropertyChangedWithValue<string>(value, "QueueText");
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00042DD1 File Offset: 0x00040FD1
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x00042DD9 File Offset: 0x00040FD9
		[DataSourceProperty]
		public string DailyDefaultsText
		{
			get
			{
				return this._dailyDefaultsText;
			}
			set
			{
				if (value != this._dailyDefaultsText)
				{
					this._dailyDefaultsText = value;
					base.OnPropertyChangedWithValue<string>(value, "DailyDefaultsText");
				}
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x00042DFC File Offset: 0x00040FFC
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x00042E04 File Offset: 0x00041004
		[DataSourceProperty]
		public string DailyDefaultsExplanationText
		{
			get
			{
				return this._dailyDefaultsExplanationText;
			}
			set
			{
				if (value != this._dailyDefaultsExplanationText)
				{
					this._dailyDefaultsExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "DailyDefaultsExplanationText");
				}
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x00042E27 File Offset: 0x00041027
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x00042E2F File Offset: 0x0004102F
		[DataSourceProperty]
		public SettlementProjectVM CurrentSelectedProject
		{
			get
			{
				return this._currentSelectedProject;
			}
			set
			{
				if (value != this._currentSelectedProject)
				{
					this._currentSelectedProject = value;
					base.OnPropertyChangedWithValue<SettlementProjectVM>(value, "CurrentSelectedProject");
					if (this._currentSelectedProject != null)
					{
						this._currentSelectedProject.RefreshProductionText();
					}
				}
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00042E60 File Offset: 0x00041060
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x00042E68 File Offset: 0x00041068
		[DataSourceProperty]
		public SettlementDailyProjectVM CurrentDailyDefault
		{
			get
			{
				return this._currentDailyDefault;
			}
			set
			{
				if (value != this._currentDailyDefault)
				{
					if (this._currentDailyDefault != null)
					{
						this._currentDailyDefault.IsDefault = false;
					}
					this._currentDailyDefault = value;
					base.OnPropertyChangedWithValue<SettlementDailyProjectVM>(value, "CurrentDailyDefault");
					if (this._currentDailyDefault != null)
					{
						this._currentDailyDefault.IsDefault = true;
					}
				}
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00042EB9 File Offset: 0x000410B9
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x00042EC1 File Offset: 0x000410C1
		[DataSourceProperty]
		public MBBindingList<SettlementBuildingProjectVM> AvailableProjects
		{
			get
			{
				return this._availableProjects;
			}
			set
			{
				if (value != this._availableProjects)
				{
					this._availableProjects = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementBuildingProjectVM>>(value, "AvailableProjects");
				}
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x00042EDF File Offset: 0x000410DF
		// (set) Token: 0x06001040 RID: 4160 RVA: 0x00042EE7 File Offset: 0x000410E7
		[DataSourceProperty]
		public MBBindingList<SettlementBuildingProjectVM> CurrentDevelopmentQueue
		{
			get
			{
				return this._currentDevelopmentQueue;
			}
			set
			{
				if (value != this._currentDevelopmentQueue)
				{
					this._currentDevelopmentQueue = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementBuildingProjectVM>>(value, "CurrentDevelopmentQueue");
				}
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x00042F05 File Offset: 0x00041105
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x00042F0D File Offset: 0x0004110D
		[DataSourceProperty]
		public MBBindingList<SettlementDailyProjectVM> DailyDefaultList
		{
			get
			{
				return this._dailyDefaultList;
			}
			set
			{
				if (value != this._dailyDefaultList)
				{
					this._dailyDefaultList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementDailyProjectVM>>(value, "DailyDefaultList");
				}
			}
		}

		// Token: 0x04000753 RID: 1875
		private readonly Town _town;

		// Token: 0x04000754 RID: 1876
		private readonly Settlement _settlement;

		// Token: 0x04000755 RID: 1877
		private readonly Action _onAnyChangeInQueue;

		// Token: 0x04000757 RID: 1879
		private SettlementDailyProjectVM _currentDailyDefault;

		// Token: 0x04000758 RID: 1880
		private SettlementProjectVM _currentSelectedProject;

		// Token: 0x04000759 RID: 1881
		private MBBindingList<SettlementDailyProjectVM> _dailyDefaultList;

		// Token: 0x0400075A RID: 1882
		private MBBindingList<SettlementBuildingProjectVM> _currentDevelopmentQueue;

		// Token: 0x0400075B RID: 1883
		private MBBindingList<SettlementBuildingProjectVM> _availableProjects;

		// Token: 0x0400075C RID: 1884
		private string _projectsText;

		// Token: 0x0400075D RID: 1885
		private string _queueText;

		// Token: 0x0400075E RID: 1886
		private string _dailyDefaultsText;

		// Token: 0x0400075F RID: 1887
		private string _dailyDefaultsExplanationText;
	}
}
