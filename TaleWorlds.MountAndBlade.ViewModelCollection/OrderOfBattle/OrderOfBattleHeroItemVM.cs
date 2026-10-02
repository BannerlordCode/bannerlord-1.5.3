using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000033 RID: 51
	public class OrderOfBattleHeroItemVM : ViewModel
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000E347 File Offset: 0x0000C547
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x0000E34F File Offset: 0x0000C54F
		public ItemObject BannerOfHero { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000E358 File Offset: 0x0000C558
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000E360 File Offset: 0x0000C560
		public bool IsAssignedBeforePlayer { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000E369 File Offset: 0x0000C569
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000E371 File Offset: 0x0000C571
		public Formation InitialFormation { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000E37A File Offset: 0x0000C57A
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0000E382 File Offset: 0x0000C582
		public OrderOfBattleFormationItemVM InitialFormationItem { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000E38B File Offset: 0x0000C58B
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0000E394 File Offset: 0x0000C594
		public OrderOfBattleFormationItemVM CurrentAssignedFormationItem
		{
			get
			{
				return this._currentAssignedFormationItem;
			}
			set
			{
				if (value != this._currentAssignedFormationItem)
				{
					this._currentAssignedFormationItem = value;
					if (this._currentAssignedFormationItem == null)
					{
						this.OnAssignmentRemoved();
					}
					this.IsAssignedToAFormation = this._currentAssignedFormationItem != null;
					this.IsLeadingAFormation = this._currentAssignedFormationItem != null && this._currentAssignedFormationItem.Formation.Captain == this.Agent;
					this.OnAssignedFormationChanged();
				}
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000E3FD File Offset: 0x0000C5FD
		public OrderOfBattleHeroItemVM()
		{
			this.IsDisabled = true;
			this.RefreshValues();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000E434 File Offset: 0x0000C634
		public OrderOfBattleHeroItemVM(Agent agent)
		{
			this.Agent = agent;
			this.BannerOfHero = agent.FormationBanner;
			this.IsDisabled = !Mission.Current.PlayerTeam.IsPlayerGeneral && !agent.IsMainAgent;
			this.IsShown = true;
			this.IsMainHero = this.Agent.IsMainAgent;
			this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Agent.Character));
			this.Tooltip = new BasicTooltipViewModel(() => this.GetCaptainTooltip());
			this.RefreshValues();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000E4EF File Offset: 0x0000C6EF
		public void SetInitialFormation(OrderOfBattleFormationItemVM formation)
		{
			if (this.InitialFormationItem != null)
			{
				Debug.FailedAssert("Initial formation for hero is already set", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleHeroItemVM.cs", "SetInitialFormation", 77);
			}
			if (formation != null)
			{
				this.InitialFormationItem = formation;
				this.InitialFormation = formation.Formation;
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000E525 File Offset: 0x0000C725
		public override void RefreshValues()
		{
			Func<Agent, List<TooltipProperty>> getAgentTooltip = OrderOfBattleHeroItemVM.GetAgentTooltip;
			this._cachedTooltipProperties = ((getAgentTooltip != null) ? getAgentTooltip(this.Agent) : null);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000E544 File Offset: 0x0000C744
		private List<TooltipProperty> GetCaptainTooltip()
		{
			return this._cachedTooltipProperties;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000E54C File Offset: 0x0000C74C
		public void OnAssignmentRemoved()
		{
			if (this.CurrentAssignedFormationItem != null)
			{
				this.CurrentAssignedFormationItem.Formation.Refresh();
			}
			if (this.InitialFormation != null)
			{
				this.Agent.Formation = this.InitialFormation;
				this.InitialFormation.Refresh();
				if (this.Agent.IsDetachableFromFormation)
				{
					this.Agent.Team.DetachmentManager.RemoveScoresOfAgentFromDetachments(this.Agent);
				}
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000E5BD File Offset: 0x0000C7BD
		public void RefreshInformation()
		{
			if (this.Agent != null)
			{
				this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Agent.Character));
				return;
			}
			this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateEmpty());
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000E5F3 File Offset: 0x0000C7F3
		private void OnAssignedFormationChanged()
		{
			Action<OrderOfBattleHeroItemVM> onHeroAssignedFormationChanged = OrderOfBattleHeroItemVM.OnHeroAssignedFormationChanged;
			if (onHeroAssignedFormationChanged != null)
			{
				onHeroAssignedFormationChanged(this);
			}
			this.RefreshAssignmentInfo();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E60C File Offset: 0x0000C80C
		public void RefreshAssignmentInfo()
		{
			if (!this.IsLeadingAFormation)
			{
				this.HasMismatchedAssignment = false;
				return;
			}
			DeploymentFormationClass orderOfBattleClass = this.CurrentAssignedFormationItem.GetOrderOfBattleClass();
			if (this.Agent.HasMount)
			{
				if (orderOfBattleClass == DeploymentFormationClass.Infantry || orderOfBattleClass == DeploymentFormationClass.Ranged || orderOfBattleClass == DeploymentFormationClass.InfantryAndRanged)
				{
					this.HasMismatchedAssignment = true;
					this.MismatchedAssignmentDescriptionText = this._mismatchMountedText.ToString();
					return;
				}
			}
			else if (orderOfBattleClass == DeploymentFormationClass.Cavalry || orderOfBattleClass == DeploymentFormationClass.HorseArcher || orderOfBattleClass == DeploymentFormationClass.CavalryAndHorseArcher)
			{
				this.HasMismatchedAssignment = true;
				this.MismatchedAssignmentDescriptionText = this._mismatchDismountedText.ToString();
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E68B File Offset: 0x0000C88B
		public void SetIsPreAssigned(bool isPreAssigned)
		{
			this.IsAssignedBeforePlayer = isPreAssigned;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E694 File Offset: 0x0000C894
		private void ExecuteSelection()
		{
			Action<OrderOfBattleHeroItemVM> onHeroSelection = OrderOfBattleHeroItemVM.OnHeroSelection;
			if (onHeroSelection == null)
			{
				return;
			}
			onHeroSelection(this);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E6A6 File Offset: 0x0000C8A6
		private void ExecuteBeginAssignment()
		{
			Action<OrderOfBattleHeroItemVM> onHeroAssignmentBegin = OrderOfBattleHeroItemVM.OnHeroAssignmentBegin;
			if (onHeroAssignmentBegin == null)
			{
				return;
			}
			onHeroAssignmentBegin(this);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E6B8 File Offset: 0x0000C8B8
		private void ExecuteEndAssignment()
		{
			Action<OrderOfBattleHeroItemVM> onHeroAssignmentEnd = OrderOfBattleHeroItemVM.OnHeroAssignmentEnd;
			if (onHeroAssignmentEnd == null)
			{
				return;
			}
			onHeroAssignmentEnd(this);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0000E6CA File Offset: 0x0000C8CA
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0000E6D2 File Offset: 0x0000C8D2
		[DataSourceProperty]
		public string MismatchedAssignmentDescriptionText
		{
			get
			{
				return this._mismatchedAssignmentDescriptionText;
			}
			set
			{
				if (value != this._mismatchedAssignmentDescriptionText)
				{
					this._mismatchedAssignmentDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "MismatchedAssignmentDescriptionText");
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0000E6F5 File Offset: 0x0000C8F5
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0000E6FD File Offset: 0x0000C8FD
		[DataSourceProperty]
		public bool IsAssignedToAFormation
		{
			get
			{
				return this._isAssignedToAFormation;
			}
			set
			{
				if (value != this._isAssignedToAFormation)
				{
					this._isAssignedToAFormation = value;
					base.OnPropertyChangedWithValue(value, "IsAssignedToAFormation");
				}
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000E71B File Offset: 0x0000C91B
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0000E723 File Offset: 0x0000C923
		[DataSourceProperty]
		public bool IsLeadingAFormation
		{
			get
			{
				return this._isLeadingAFormation;
			}
			set
			{
				if (value != this._isLeadingAFormation)
				{
					this._isLeadingAFormation = value;
					base.OnPropertyChangedWithValue(value, "IsLeadingAFormation");
				}
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000E741 File Offset: 0x0000C941
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0000E749 File Offset: 0x0000C949
		[DataSourceProperty]
		public bool HasMismatchedAssignment
		{
			get
			{
				return this._hasMismatchedAssignment;
			}
			set
			{
				if (value != this._hasMismatchedAssignment)
				{
					this._hasMismatchedAssignment = value;
					base.OnPropertyChangedWithValue(value, "HasMismatchedAssignment");
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000E767 File Offset: 0x0000C967
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0000E76F File Offset: 0x0000C96F
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000E78D File Offset: 0x0000C98D
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000E795 File Offset: 0x0000C995
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000E7B3 File Offset: 0x0000C9B3
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000E7BB File Offset: 0x0000C9BB
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChangedWithValue(value, "IsShown");
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000E7D9 File Offset: 0x0000C9D9
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000E7E1 File Offset: 0x0000C9E1
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000E7FF File Offset: 0x0000C9FF
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0000E807 File Offset: 0x0000CA07
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000E825 File Offset: 0x0000CA25
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0000E82D File Offset: 0x0000CA2D
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000E84B File Offset: 0x0000CA4B
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0000E853 File Offset: 0x0000CA53
		[DataSourceProperty]
		public bool IsHighlightActive
		{
			get
			{
				return this._isHighlightActive;
			}
			set
			{
				if (value != this._isHighlightActive)
				{
					this._isHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightActive");
				}
			}
		}

		// Token: 0x040001D2 RID: 466
		private readonly TextObject _mismatchMountedText = new TextObject("{=J9V9YhkY}Captain is mounted!", null);

		// Token: 0x040001D3 RID: 467
		private readonly TextObject _mismatchDismountedText = new TextObject("{=ufjypmaX}Captain is not mounted!", null);

		// Token: 0x040001D4 RID: 468
		public static Action<OrderOfBattleHeroItemVM> OnHeroSelection;

		// Token: 0x040001D5 RID: 469
		public static Action<OrderOfBattleHeroItemVM> OnHeroAssignedFormationChanged;

		// Token: 0x040001D6 RID: 470
		public static Func<Agent, List<TooltipProperty>> GetAgentTooltip;

		// Token: 0x040001D7 RID: 471
		public static Action<OrderOfBattleHeroItemVM> OnHeroAssignmentBegin;

		// Token: 0x040001D8 RID: 472
		public static Action<OrderOfBattleHeroItemVM> OnHeroAssignmentEnd;

		// Token: 0x040001D9 RID: 473
		public readonly Agent Agent;

		// Token: 0x040001DE RID: 478
		private List<TooltipProperty> _cachedTooltipProperties;

		// Token: 0x040001DF RID: 479
		private OrderOfBattleFormationItemVM _currentAssignedFormationItem;

		// Token: 0x040001E0 RID: 480
		private string _mismatchedAssignmentDescriptionText;

		// Token: 0x040001E1 RID: 481
		private bool _isAssignedToAFormation;

		// Token: 0x040001E2 RID: 482
		private bool _isLeadingAFormation;

		// Token: 0x040001E3 RID: 483
		private bool _hasMismatchedAssignment;

		// Token: 0x040001E4 RID: 484
		private bool _isSelected;

		// Token: 0x040001E5 RID: 485
		private bool _isDisabled;

		// Token: 0x040001E6 RID: 486
		private bool _isShown;

		// Token: 0x040001E7 RID: 487
		private bool _isMainHero;

		// Token: 0x040001E8 RID: 488
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x040001E9 RID: 489
		private BasicTooltipViewModel _tooltip;

		// Token: 0x040001EA RID: 490
		private bool _isHighlightActive;
	}
}
