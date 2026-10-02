using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000054 RID: 84
	public class MapSiegeProductionMachineVM : ViewModel
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001485F File Offset: 0x00012A5F
		public SiegeEngineType Engine { get; }

		// Token: 0x06000558 RID: 1368 RVA: 0x00014867 File Offset: 0x00012A67
		public MapSiegeProductionMachineVM(SiegeEngineType engineType, int number, Action<MapSiegeProductionMachineVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Engine = engineType;
			this.NumberOfMachines = number;
			this.MachineID = engineType.StringId;
			this.IsReserveOption = false;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00014897 File Offset: 0x00012A97
		public MapSiegeProductionMachineVM(Action<MapSiegeProductionMachineVM> onSelection, bool isCancel)
		{
			this._onSelection = onSelection;
			this.Engine = null;
			this.NumberOfMachines = 0;
			this.MachineID = "reserve";
			this.IsReserveOption = true;
			this._isCancel = isCancel;
			this.RefreshValues();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000148D3 File Offset: 0x00012AD3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ActionText = (this._isCancel ? GameTexts.FindText("str_cancel", null).ToString() : GameTexts.FindText("str_siege_move_to_reserve", null).ToString());
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001490B File Offset: 0x00012B0B
		public void OnSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00014919 File Offset: 0x00012B19
		public void ExecuteShowTooltip()
		{
			if (this.Engine != null)
			{
				InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { SandBoxUIHelper.GetSiegeEngineTooltip(this.Engine) });
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00014946 File Offset: 0x00012B46
		public void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001494D File Offset: 0x00012B4D
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00014955 File Offset: 0x00012B55
		[DataSourceProperty]
		public int MachineType
		{
			get
			{
				return this._machineType;
			}
			set
			{
				if (value != this._machineType)
				{
					this._machineType = value;
					base.OnPropertyChangedWithValue(value, "MachineType");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00014973 File Offset: 0x00012B73
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0001497B File Offset: 0x00012B7B
		[DataSourceProperty]
		public string MachineID
		{
			get
			{
				return this._machineID;
			}
			set
			{
				if (value != this._machineID)
				{
					this._machineID = value;
					base.OnPropertyChangedWithValue<string>(value, "MachineID");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001499E File Offset: 0x00012B9E
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x000149A6 File Offset: 0x00012BA6
		[DataSourceProperty]
		public int NumberOfMachines
		{
			get
			{
				return this._numberOfMachines;
			}
			set
			{
				if (value != this._numberOfMachines)
				{
					this._numberOfMachines = value;
					base.OnPropertyChangedWithValue(value, "NumberOfMachines");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x000149C4 File Offset: 0x00012BC4
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x000149CC File Offset: 0x00012BCC
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x000149EF File Offset: 0x00012BEF
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x000149F7 File Offset: 0x00012BF7
		[DataSourceProperty]
		public bool IsReserveOption
		{
			get
			{
				return this._isReserveOption;
			}
			set
			{
				if (value != this._isReserveOption)
				{
					this._isReserveOption = value;
					base.OnPropertyChangedWithValue(value, "IsReserveOption");
				}
			}
		}

		// Token: 0x040002B2 RID: 690
		private Action<MapSiegeProductionMachineVM> _onSelection;

		// Token: 0x040002B4 RID: 692
		private bool _isCancel;

		// Token: 0x040002B5 RID: 693
		private int _machineType;

		// Token: 0x040002B6 RID: 694
		private int _numberOfMachines;

		// Token: 0x040002B7 RID: 695
		private string _machineID;

		// Token: 0x040002B8 RID: 696
		private bool _isReserveOption;

		// Token: 0x040002B9 RID: 697
		private string _actionText;
	}
}
