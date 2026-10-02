using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000069 RID: 105
	public class PartyScreenWidget : Widget
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00010D44 File Offset: 0x0000EF44
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x00010D4C File Offset: 0x0000EF4C
		public ScrollablePanel MainScrollPanel { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00010D55 File Offset: 0x0000EF55
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x00010D5D File Offset: 0x0000EF5D
		public ScrollablePanel OtherScrollPanel { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00010D66 File Offset: 0x0000EF66
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00010D6E File Offset: 0x0000EF6E
		public InputKeyVisualWidget TransferInputKeyVisual { get; set; }

		// Token: 0x0600059E RID: 1438 RVA: 0x00010D77 File Offset: 0x0000EF77
		public PartyScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00010D8B File Offset: 0x0000EF8B
		protected override void OnConnectedToRoot()
		{
			base.Context.EventManager.OnDragStarted += this.OnDragStarted;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00010DA9 File Offset: 0x0000EFA9
		protected override void OnDisconnectedFromRoot()
		{
			base.Context.EventManager.OnDragStarted -= this.OnDragStarted;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00010DC8 File Offset: 0x0000EFC8
		private Widget GetItemWithId(ListPanel charactersListPanel, string id)
		{
			if (charactersListPanel == null)
			{
				return null;
			}
			return charactersListPanel.FindChild((Widget x) => (x as PartyTroopTupleButtonWidget).CharacterID == id);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00010DFC File Offset: 0x0000EFFC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._latestMouseDownWidget != base.EventManager.LatestMouseDownWidget)
			{
				this._latestMouseDownWidget = base.EventManager.LatestMouseDownWidget;
				bool flag;
				if (this._latestMouseDownWidget != null)
				{
					if (!(this._latestMouseDownWidget is PartyTroopTupleButtonWidget))
					{
						flag = this._latestMouseDownWidget.GetAllParents().Any<Widget>((Widget x) => x is PartyTroopTupleButtonWidget);
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (this._latestMouseDownWidget == null || (!flag2 && this.IsWidgetChildOfType<PartyFormationDropdownWidget>(this._latestMouseDownWidget) == null))
				{
					base.EventFired("OnEmptyClick", Array.Empty<object>());
				}
			}
			this.UpdateInputKeyVisualsVisibility();
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00010EB4 File Offset: 0x0000F0B4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.ScrollToCharacter)
			{
				this._scrollToCharacterInSeconds = 0.2f;
				this.ScrollToCharacter = false;
			}
			if (this._scrollToCharacterInSeconds >= 0f)
			{
				this._scrollToCharacterInSeconds -= dt;
				if (this._scrollToCharacterInSeconds <= 0f)
				{
					ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters(100f, 100f, 0f, 0f, -1f, -1f, 0.35f);
					if (this.IsScrollTargetPrisoner)
					{
						ScrollablePanel scrollablePanel = this.OtherPrisonerList.FindParentPanel();
						if (scrollablePanel != null)
						{
							scrollablePanel.ScrollToChild(this.GetItemWithId(this.OtherPrisonerList, this.ScrollCharacterId), autoScrollParameters);
						}
						ScrollablePanel scrollablePanel2 = this.MainPrisonerList.FindParentPanel();
						if (scrollablePanel2 == null)
						{
							return;
						}
						scrollablePanel2.ScrollToChild(this.GetItemWithId(this.MainPrisonerList, this.ScrollCharacterId), autoScrollParameters);
						return;
					}
					else
					{
						ScrollablePanel scrollablePanel3 = this.OtherMemberList.FindParentPanel();
						if (scrollablePanel3 != null)
						{
							scrollablePanel3.ScrollToChild(this.GetItemWithId(this.OtherMemberList, this.ScrollCharacterId), autoScrollParameters);
						}
						ScrollablePanel scrollablePanel4 = this.MainMemberList.FindParentPanel();
						if (scrollablePanel4 == null)
						{
							return;
						}
						scrollablePanel4.ScrollToChild(this.GetItemWithId(this.MainMemberList, this.ScrollCharacterId), autoScrollParameters);
					}
				}
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00010FE4 File Offset: 0x0000F1E4
		private void UpdateInputKeyVisualsVisibility()
		{
			if (base.EventManager.IsControllerActive)
			{
				bool flag = false;
				PartyTroopTupleButtonWidget partyTroopTupleButtonWidget;
				if ((partyTroopTupleButtonWidget = base.EventManager.HoveredWidget as PartyTroopTupleButtonWidget) != null)
				{
					this.TransferInputKeyVisual.IsVisible = partyTroopTupleButtonWidget.IsTransferable;
					flag = true;
					if (partyTroopTupleButtonWidget.IsTupleLeftSide)
					{
						this.TransferInputKeyVisual.KeyID = this._takeAllPrisonersInputKeyVisual.KeyID;
						this.TransferInputKeyVisual.ScaledPositionXOffset = partyTroopTupleButtonWidget.GlobalPosition.X + partyTroopTupleButtonWidget.Size.X - 65f * base._scaleToUse;
						this.TransferInputKeyVisual.ScaledPositionYOffset = partyTroopTupleButtonWidget.GlobalPosition.Y - 13f * base._scaleToUse;
					}
					else
					{
						this.TransferInputKeyVisual.KeyID = this._dismissAllPrisonersInputKeyVisual.KeyID;
						this.TransferInputKeyVisual.ScaledPositionXOffset = partyTroopTupleButtonWidget.GlobalPosition.X + 5f * base._scaleToUse;
						this.TransferInputKeyVisual.ScaledPositionYOffset = partyTroopTupleButtonWidget.GlobalPosition.Y - 13f * base._scaleToUse;
					}
				}
				else
				{
					this.TransferInputKeyVisual.IsVisible = false;
					this.TransferInputKeyVisual.KeyID = "";
				}
				bool flag2 = !this.IsAnyPopupOpen() && !flag && !this.MainScrollPanel.InnerPanel.IsHovered && !this.OtherScrollPanel.InnerPanel.IsHovered && !GauntletGamepadNavigationManager.Instance.IsCursorMovingForNavigation;
				this.TakeAllPrisonersInputKeyVisualParent.IsVisible = flag2;
				this.DismissAllPrisonersInputKeyVisualParent.IsVisible = flag2;
				return;
			}
			this.TransferInputKeyVisual.IsVisible = false;
			this.TakeAllPrisonersInputKeyVisualParent.IsVisible = true;
			this.DismissAllPrisonersInputKeyVisualParent.IsVisible = true;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00011198 File Offset: 0x0000F398
		private void RefreshWarningStatuses()
		{
			TextWidget prisonerLabel = this.PrisonerLabel;
			if (prisonerLabel != null)
			{
				prisonerLabel.SetState(this.IsPrisonerWarningEnabled ? "OverLimit" : "Default");
			}
			TextWidget troopLabel = this.TroopLabel;
			if (troopLabel != null)
			{
				troopLabel.SetState(this.IsTroopWarningEnabled ? "OverLimit" : "Default");
			}
			TextWidget otherTroopLabel = this.OtherTroopLabel;
			if (otherTroopLabel == null)
			{
				return;
			}
			otherTroopLabel.SetState(this.IsOtherTroopWarningEnabled ? "OverLimit" : "Default");
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00011214 File Offset: 0x0000F414
		private PartyTroopTupleButtonWidget FindTupleWithTroopIDInList(string troopID, bool searchMainList, bool isPrisoner)
		{
			IEnumerable<PartyTroopTupleButtonWidget> enumerable;
			if (searchMainList)
			{
				enumerable = (isPrisoner ? this.MainPrisonerList.Children.Cast<PartyTroopTupleButtonWidget>() : this.MainMemberList.Children.Cast<PartyTroopTupleButtonWidget>());
			}
			else
			{
				enumerable = (isPrisoner ? this.OtherPrisonerList.Children.Cast<PartyTroopTupleButtonWidget>() : this.OtherMemberList.Children.Cast<PartyTroopTupleButtonWidget>());
			}
			return enumerable.SingleOrDefault<PartyTroopTupleButtonWidget>((PartyTroopTupleButtonWidget i) => i.CharacterID == troopID);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00011293 File Offset: 0x0000F493
		private void OnDragStarted()
		{
			base.EventFired("OnEmptyClick", Array.Empty<object>());
			this.RemoveZeroCountItems();
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000112AB File Offset: 0x0000F4AB
		private void RemoveZeroCountItems()
		{
			base.EventFired("RemoveZeroCounts", Array.Empty<object>());
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000112BD File Offset: 0x0000F4BD
		private bool IsAnyPopupOpen()
		{
			Widget recruitPopupParent = this.RecruitPopupParent;
			if (recruitPopupParent == null || !recruitPopupParent.IsVisible)
			{
				Widget upgradePopupParent = this.UpgradePopupParent;
				return upgradePopupParent != null && upgradePopupParent.IsVisible;
			}
			return true;
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x000112E6 File Offset: 0x0000F4E6
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x000112EE File Offset: 0x0000F4EE
		public Widget UpgradePopupParent
		{
			get
			{
				return this._upgradePopupParent;
			}
			set
			{
				if (value != this._upgradePopupParent)
				{
					this._upgradePopupParent = value;
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00011300 File Offset: 0x0000F500
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00011308 File Offset: 0x0000F508
		public Widget RecruitPopupParent
		{
			get
			{
				return this._recruitPopupParent;
			}
			set
			{
				if (value != this._recruitPopupParent)
				{
					this._recruitPopupParent = value;
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001131A File Offset: 0x0000F51A
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00011324 File Offset: 0x0000F524
		public Widget TakeAllPrisonersInputKeyVisualParent
		{
			get
			{
				return this._takeAllPrisonersInputKeyVisualParent;
			}
			set
			{
				if (value != this._takeAllPrisonersInputKeyVisualParent)
				{
					this._takeAllPrisonersInputKeyVisualParent = value;
					if (this._takeAllPrisonersInputKeyVisualParent != null)
					{
						this._takeAllPrisonersInputKeyVisual = this._takeAllPrisonersInputKeyVisualParent.Children.FirstOrDefault<Widget>((Widget x) => x is InputKeyVisualWidget) as InputKeyVisualWidget;
					}
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00011383 File Offset: 0x0000F583
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0001138C File Offset: 0x0000F58C
		public Widget DismissAllPrisonersInputKeyVisualParent
		{
			get
			{
				return this._dismissAllPrisonersInputKeyVisualParent;
			}
			set
			{
				if (value != this._dismissAllPrisonersInputKeyVisualParent)
				{
					this._dismissAllPrisonersInputKeyVisualParent = value;
					if (this._dismissAllPrisonersInputKeyVisualParent != null)
					{
						this._dismissAllPrisonersInputKeyVisual = this._dismissAllPrisonersInputKeyVisualParent.Children.FirstOrDefault<Widget>((Widget x) => x is InputKeyVisualWidget) as InputKeyVisualWidget;
					}
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x000113EB File Offset: 0x0000F5EB
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x000113F3 File Offset: 0x0000F5F3
		[Editor(false)]
		public int MainPartyTroopSize
		{
			get
			{
				return this._mainPartyTroopSize;
			}
			set
			{
				if (this._mainPartyTroopSize != value)
				{
					this._mainPartyTroopSize = value;
					base.OnPropertyChanged(value, "MainPartyTroopSize");
				}
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00011411 File Offset: 0x0000F611
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x00011419 File Offset: 0x0000F619
		[Editor(false)]
		public bool IsPrisonerWarningEnabled
		{
			get
			{
				return this._isPrisonerWarningEnabled;
			}
			set
			{
				if (this._isPrisonerWarningEnabled != value)
				{
					this._isPrisonerWarningEnabled = value;
					base.OnPropertyChanged(value, "IsPrisonerWarningEnabled");
					this.RefreshWarningStatuses();
				}
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001143D File Offset: 0x0000F63D
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00011445 File Offset: 0x0000F645
		[Editor(false)]
		public bool IsOtherTroopWarningEnabled
		{
			get
			{
				return this._isOtherTroopWarningEnabled;
			}
			set
			{
				if (this._isOtherTroopWarningEnabled != value)
				{
					this._isOtherTroopWarningEnabled = value;
					base.OnPropertyChanged(value, "IsOtherTroopWarningEnabled");
					this.RefreshWarningStatuses();
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00011469 File Offset: 0x0000F669
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x00011471 File Offset: 0x0000F671
		[Editor(false)]
		public bool IsTroopWarningEnabled
		{
			get
			{
				return this._isTroopWarningEnabled;
			}
			set
			{
				if (this._isTroopWarningEnabled != value)
				{
					this._isTroopWarningEnabled = value;
					base.OnPropertyChanged(value, "IsTroopWarningEnabled");
					this.RefreshWarningStatuses();
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00011495 File Offset: 0x0000F695
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x0001149D File Offset: 0x0000F69D
		[Editor(false)]
		public TextWidget TroopLabel
		{
			get
			{
				return this._troopLabel;
			}
			set
			{
				if (this._troopLabel != value)
				{
					this._troopLabel = value;
					base.OnPropertyChanged<TextWidget>(value, "TroopLabel");
					this.RefreshWarningStatuses();
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000114C1 File Offset: 0x0000F6C1
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x000114C9 File Offset: 0x0000F6C9
		[Editor(false)]
		public TextWidget PrisonerLabel
		{
			get
			{
				return this._prisonerLabel;
			}
			set
			{
				if (this._prisonerLabel != value)
				{
					this._prisonerLabel = value;
					base.OnPropertyChanged<TextWidget>(value, "PrisonerLabel");
					this.RefreshWarningStatuses();
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x000114ED File Offset: 0x0000F6ED
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x000114F5 File Offset: 0x0000F6F5
		[Editor(false)]
		public TextWidget OtherTroopLabel
		{
			get
			{
				return this._otherTroopLabel;
			}
			set
			{
				if (this._otherTroopLabel != value)
				{
					this._otherTroopLabel = value;
					base.OnPropertyChanged<TextWidget>(value, "OtherTroopLabel");
					this.RefreshWarningStatuses();
				}
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00011519 File Offset: 0x0000F719
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x00011521 File Offset: 0x0000F721
		[Editor(false)]
		public ListPanel OtherMemberList
		{
			get
			{
				return this._otherMemberList;
			}
			set
			{
				if (this._otherMemberList != value)
				{
					this._otherMemberList = value;
				}
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00011533 File Offset: 0x0000F733
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x0001153B File Offset: 0x0000F73B
		[Editor(false)]
		public ListPanel OtherPrisonerList
		{
			get
			{
				return this._otherPrisonerList;
			}
			set
			{
				if (this._otherPrisonerList != value)
				{
					this._otherPrisonerList = value;
				}
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001154D File Offset: 0x0000F74D
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00011555 File Offset: 0x0000F755
		[Editor(false)]
		public ListPanel MainMemberList
		{
			get
			{
				return this._mainMemberList;
			}
			set
			{
				if (this._mainMemberList != value)
				{
					this._mainMemberList = value;
				}
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00011567 File Offset: 0x0000F767
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x0001156F File Offset: 0x0000F76F
		[Editor(false)]
		public ListPanel MainPrisonerList
		{
			get
			{
				return this._mainPrisonerList;
			}
			set
			{
				if (this._mainPrisonerList != value)
				{
					this._mainPrisonerList = value;
				}
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00011581 File Offset: 0x0000F781
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x00011589 File Offset: 0x0000F789
		[Editor(false)]
		public bool ScrollToCharacter
		{
			get
			{
				return this._scrollToCharacter;
			}
			set
			{
				if (value != this._scrollToCharacter)
				{
					this._scrollToCharacter = value;
					base.OnPropertyChanged(value, "ScrollToCharacter");
				}
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x000115A7 File Offset: 0x0000F7A7
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x000115AF File Offset: 0x0000F7AF
		[Editor(false)]
		public string ScrollCharacterId
		{
			get
			{
				return this._scrollCharacterId;
			}
			set
			{
				if (value != this._scrollCharacterId)
				{
					this._scrollCharacterId = value;
					base.OnPropertyChanged<string>(value, "ScrollCharacterId");
				}
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x000115D2 File Offset: 0x0000F7D2
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x000115DA File Offset: 0x0000F7DA
		[Editor(false)]
		public bool IsScrollTargetPrisoner
		{
			get
			{
				return this._isScrollTargetPrisoner;
			}
			set
			{
				if (value != this._isScrollTargetPrisoner)
				{
					this._isScrollTargetPrisoner = value;
					base.OnPropertyChanged(value, "IsScrollTargetPrisoner");
				}
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000115F8 File Offset: 0x0000F7F8
		private T IsWidgetChildOfType<T>(Widget currentWidget) where T : Widget
		{
			while (currentWidget != null)
			{
				T t;
				if ((t = currentWidget as T) != null)
				{
					return t;
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return default(T);
		}

		// Token: 0x04000262 RID: 610
		private float _scrollToCharacterInSeconds = -1f;

		// Token: 0x04000263 RID: 611
		private Widget _latestMouseDownWidget;

		// Token: 0x04000266 RID: 614
		private InputKeyVisualWidget _takeAllPrisonersInputKeyVisual;

		// Token: 0x04000267 RID: 615
		private InputKeyVisualWidget _dismissAllPrisonersInputKeyVisual;

		// Token: 0x04000269 RID: 617
		private Widget _upgradePopupParent;

		// Token: 0x0400026A RID: 618
		private Widget _recruitPopupParent;

		// Token: 0x0400026B RID: 619
		private Widget _takeAllPrisonersInputKeyVisualParent;

		// Token: 0x0400026C RID: 620
		private Widget _dismissAllPrisonersInputKeyVisualParent;

		// Token: 0x0400026D RID: 621
		private int _mainPartyTroopSize;

		// Token: 0x0400026E RID: 622
		private bool _isPrisonerWarningEnabled;

		// Token: 0x0400026F RID: 623
		private bool _isTroopWarningEnabled;

		// Token: 0x04000270 RID: 624
		private bool _isOtherTroopWarningEnabled;

		// Token: 0x04000271 RID: 625
		private TextWidget _troopLabel;

		// Token: 0x04000272 RID: 626
		private TextWidget _prisonerLabel;

		// Token: 0x04000273 RID: 627
		private TextWidget _otherTroopLabel;

		// Token: 0x04000274 RID: 628
		private ListPanel _otherMemberList;

		// Token: 0x04000275 RID: 629
		private ListPanel _otherPrisonerList;

		// Token: 0x04000276 RID: 630
		private ListPanel _mainMemberList;

		// Token: 0x04000277 RID: 631
		private ListPanel _mainPrisonerList;

		// Token: 0x04000278 RID: 632
		private bool _scrollToCharacter;

		// Token: 0x04000279 RID: 633
		private bool _isScrollTargetPrisoner;

		// Token: 0x0400027A RID: 634
		private string _scrollCharacterId;
	}
}
