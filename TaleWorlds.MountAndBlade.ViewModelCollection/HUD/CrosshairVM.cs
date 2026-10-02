using System;
using System.Collections.ObjectModel;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x0200004D RID: 77
	public class CrosshairVM : ViewModel
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x00017569 File Offset: 0x00015769
		public CrosshairVM()
		{
			this.ReloadPhases = new MBBindingList<ReloadPhaseItemVM>();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001757C File Offset: 0x0001577C
		public void SetProperties(double accuracy, double scale)
		{
			this.CrosshairAccuracy = accuracy;
			this.CrosshairScale = scale;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001758C File Offset: 0x0001578C
		public void SetArrowProperties(double topArrowOpacity, double rightArrowOpacity, double bottomArrowOpacity, double leftArrowOpacity)
		{
			this.TopArrowOpacity = topArrowOpacity;
			this.BottomArrowOpacity = bottomArrowOpacity;
			this.RightArrowOpacity = rightArrowOpacity;
			this.LeftArrowOpacity = leftArrowOpacity;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000175AC File Offset: 0x000157AC
		public void SetReloadProperties(in StackArray.StackArray10FloatFloatTuple reloadPhases, int reloadPhaseCount)
		{
			if (reloadPhaseCount == 0)
			{
				this.IsReloadPhasesVisible = false;
			}
			else
			{
				for (int i = 0; i < reloadPhaseCount; i++)
				{
					StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = reloadPhases;
					if (stackArray10FloatFloatTuple[i].Item1 < 1f)
					{
						this.IsReloadPhasesVisible = true;
						break;
					}
				}
			}
			this.PopulateReloadPhases(in reloadPhases, reloadPhaseCount);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00017600 File Offset: 0x00015800
		private void PopulateReloadPhases(in StackArray.StackArray10FloatFloatTuple reloadPhases, int reloadPhaseCount)
		{
			if (reloadPhaseCount != this.ReloadPhases.Count)
			{
				this.ReloadPhases.Clear();
				for (int i = 0; i < reloadPhaseCount; i++)
				{
					Collection<ReloadPhaseItemVM> reloadPhases2 = this.ReloadPhases;
					StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = reloadPhases;
					float item = stackArray10FloatFloatTuple[i].Item1;
					stackArray10FloatFloatTuple = reloadPhases;
					reloadPhases2.Add(new ReloadPhaseItemVM(item, stackArray10FloatFloatTuple[i].Item2));
				}
				return;
			}
			for (int j = 0; j < reloadPhaseCount; j++)
			{
				ReloadPhaseItemVM reloadPhaseItemVM = this.ReloadPhases[j];
				StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = reloadPhases;
				float item2 = stackArray10FloatFloatTuple[j].Item1;
				stackArray10FloatFloatTuple = reloadPhases;
				reloadPhaseItemVM.Update(item2, stackArray10FloatFloatTuple[j].Item2);
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000176B0 File Offset: 0x000158B0
		public void ShowHitMarker(bool isVictimDead, bool isHumanoidHeadShot)
		{
			this.IsVictimDead = isVictimDead;
			this.IsHitMarkerVisible = false;
			this.IsHitMarkerVisible = true;
			this.IsHumanoidHeadshot = false;
			this.IsHumanoidHeadshot = isHumanoidHeadShot;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x000176D5 File Offset: 0x000158D5
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x000176DD File Offset: 0x000158DD
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x000176FB File Offset: 0x000158FB
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00017703 File Offset: 0x00015903
		[DataSourceProperty]
		public bool IsReloadPhasesVisible
		{
			get
			{
				return this._isReloadPhasesVisible;
			}
			set
			{
				if (value != this._isReloadPhasesVisible)
				{
					this._isReloadPhasesVisible = value;
					base.OnPropertyChangedWithValue(value, "IsReloadPhasesVisible");
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00017721 File Offset: 0x00015921
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00017729 File Offset: 0x00015929
		[DataSourceProperty]
		public bool IsHitMarkerVisible
		{
			get
			{
				return this._isHitMarkerVisible;
			}
			set
			{
				if (value != this._isHitMarkerVisible)
				{
					this._isHitMarkerVisible = value;
					base.OnPropertyChangedWithValue(value, "IsHitMarkerVisible");
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00017747 File Offset: 0x00015947
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0001774F File Offset: 0x0001594F
		[DataSourceProperty]
		public bool IsVictimDead
		{
			get
			{
				return this._isVictimDead;
			}
			set
			{
				if (value != this._isVictimDead)
				{
					this._isVictimDead = value;
					base.OnPropertyChangedWithValue(value, "IsVictimDead");
				}
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001776D File Offset: 0x0001596D
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x00017775 File Offset: 0x00015975
		[DataSourceProperty]
		public bool IsHumanoidHeadshot
		{
			get
			{
				return this._isHumanoidHeadshot;
			}
			set
			{
				if (value != this._isHumanoidHeadshot)
				{
					this._isHumanoidHeadshot = value;
					base.OnPropertyChangedWithValue(value, "IsHumanoidHeadshot");
				}
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00017793 File Offset: 0x00015993
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0001779B File Offset: 0x0001599B
		[DataSourceProperty]
		public double TopArrowOpacity
		{
			get
			{
				return this._topArrowOpacity;
			}
			set
			{
				if (value != this._topArrowOpacity)
				{
					this._topArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "TopArrowOpacity");
				}
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x000177B9 File Offset: 0x000159B9
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x000177C1 File Offset: 0x000159C1
		[DataSourceProperty]
		public MBBindingList<ReloadPhaseItemVM> ReloadPhases
		{
			get
			{
				return this._reloadPhases;
			}
			set
			{
				if (value != this._reloadPhases)
				{
					this._reloadPhases = value;
					base.OnPropertyChangedWithValue<MBBindingList<ReloadPhaseItemVM>>(value, "ReloadPhases");
				}
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x000177DF File Offset: 0x000159DF
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x000177E7 File Offset: 0x000159E7
		[DataSourceProperty]
		public double BottomArrowOpacity
		{
			get
			{
				return this._bottomArrowOpacity;
			}
			set
			{
				if (value != this._bottomArrowOpacity)
				{
					this._bottomArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "BottomArrowOpacity");
				}
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00017805 File Offset: 0x00015A05
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x0001780D File Offset: 0x00015A0D
		[DataSourceProperty]
		public double RightArrowOpacity
		{
			get
			{
				return this._rightArrowOpacity;
			}
			set
			{
				if (value != this._rightArrowOpacity)
				{
					this._rightArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "RightArrowOpacity");
				}
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001782B File Offset: 0x00015A2B
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x00017833 File Offset: 0x00015A33
		[DataSourceProperty]
		public double LeftArrowOpacity
		{
			get
			{
				return this._leftArrowOpacity;
			}
			set
			{
				if (value != this._leftArrowOpacity)
				{
					this._leftArrowOpacity = value;
					base.OnPropertyChangedWithValue(value, "LeftArrowOpacity");
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00017851 File Offset: 0x00015A51
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x00017859 File Offset: 0x00015A59
		[DataSourceProperty]
		public bool IsTargetInvalid
		{
			get
			{
				return this._isTargetInvalid;
			}
			set
			{
				if (value != this._isTargetInvalid)
				{
					this._isTargetInvalid = value;
					base.OnPropertyChangedWithValue(value, "IsTargetInvalid");
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00017877 File Offset: 0x00015A77
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x0001787F File Offset: 0x00015A7F
		[DataSourceProperty]
		public double CrosshairAccuracy
		{
			get
			{
				return this._crosshairAccuracy;
			}
			set
			{
				if (value != this._crosshairAccuracy)
				{
					this._crosshairAccuracy = value;
					base.OnPropertyChangedWithValue(value, "CrosshairAccuracy");
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001789D File Offset: 0x00015A9D
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x000178A5 File Offset: 0x00015AA5
		[DataSourceProperty]
		public double CrosshairScale
		{
			get
			{
				return this._crosshairScale;
			}
			set
			{
				if (value != this._crosshairScale)
				{
					this._crosshairScale = value;
					base.OnPropertyChangedWithValue(value, "CrosshairScale");
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x000178C3 File Offset: 0x00015AC3
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x000178CB File Offset: 0x00015ACB
		[DataSourceProperty]
		public int CrosshairType
		{
			get
			{
				return this._crosshairType;
			}
			set
			{
				if (value != this._crosshairType)
				{
					this._crosshairType = value;
					base.OnPropertyChangedWithValue(value, "CrosshairType");
				}
			}
		}

		// Token: 0x040002D6 RID: 726
		private bool _isVisible;

		// Token: 0x040002D7 RID: 727
		private bool _isReloadPhasesVisible;

		// Token: 0x040002D8 RID: 728
		private bool _isHitMarkerVisible;

		// Token: 0x040002D9 RID: 729
		private bool _isVictimDead;

		// Token: 0x040002DA RID: 730
		private bool _isHumanoidHeadshot;

		// Token: 0x040002DB RID: 731
		private bool _isTargetInvalid;

		// Token: 0x040002DC RID: 732
		private MBBindingList<ReloadPhaseItemVM> _reloadPhases;

		// Token: 0x040002DD RID: 733
		private double _crosshairAccuracy;

		// Token: 0x040002DE RID: 734
		private double _crosshairScale;

		// Token: 0x040002DF RID: 735
		private double _topArrowOpacity;

		// Token: 0x040002E0 RID: 736
		private double _bottomArrowOpacity;

		// Token: 0x040002E1 RID: 737
		private double _rightArrowOpacity;

		// Token: 0x040002E2 RID: 738
		private double _leftArrowOpacity;

		// Token: 0x040002E3 RID: 739
		private int _crosshairType;
	}
}
