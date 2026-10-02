using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000032 RID: 50
	public class NavigationScopeTargeter : Widget
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x000091A9 File Offset: 0x000073A9
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x000091B1 File Offset: 0x000073B1
		public GamepadNavigationScope NavigationScope { get; private set; }

		// Token: 0x060002B5 RID: 693 RVA: 0x000091BA File Offset: 0x000073BA
		public NavigationScopeTargeter(UIContext context)
			: base(context)
		{
			this.NavigationScope = new GamepadNavigationScope();
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x000091F9 File Offset: 0x000073F9
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00009206 File Offset: 0x00007406
		public string ScopeID
		{
			get
			{
				return this.NavigationScope.ScopeID;
			}
			set
			{
				if (value != this.NavigationScope.ScopeID)
				{
					this.NavigationScope.ScopeID = value;
				}
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00009227 File Offset: 0x00007427
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x00009234 File Offset: 0x00007434
		public GamepadNavigationTypes ScopeMovements
		{
			get
			{
				return this.NavigationScope.ScopeMovements;
			}
			set
			{
				if (value != this.NavigationScope.ScopeMovements)
				{
					this.NavigationScope.ScopeMovements = value;
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00009250 File Offset: 0x00007450
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000925D File Offset: 0x0000745D
		public GamepadNavigationTypes AlternateScopeMovements
		{
			get
			{
				return this.NavigationScope.AlternateScopeMovements;
			}
			set
			{
				if (value != this.NavigationScope.AlternateScopeMovements)
				{
					this.NavigationScope.AlternateScopeMovements = value;
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00009279 File Offset: 0x00007479
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00009286 File Offset: 0x00007486
		public int AlternateMovementStepSize
		{
			get
			{
				return this.NavigationScope.AlternateMovementStepSize;
			}
			set
			{
				if (value != this.NavigationScope.AlternateMovementStepSize)
				{
					this.NavigationScope.AlternateMovementStepSize = value;
				}
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000092A2 File Offset: 0x000074A2
		// (set) Token: 0x060002BF RID: 703 RVA: 0x000092AF File Offset: 0x000074AF
		public bool HasCircularMovement
		{
			get
			{
				return this.NavigationScope.HasCircularMovement;
			}
			set
			{
				if (value != this.NavigationScope.HasCircularMovement)
				{
					this.NavigationScope.HasCircularMovement = value;
				}
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x000092CB File Offset: 0x000074CB
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x000092D8 File Offset: 0x000074D8
		public bool DoNotAutomaticallyFindChildren
		{
			get
			{
				return this.NavigationScope.DoNotAutomaticallyFindChildren;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutomaticallyFindChildren)
				{
					this.NavigationScope.DoNotAutomaticallyFindChildren = value;
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000092F4 File Offset: 0x000074F4
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00009301 File Offset: 0x00007501
		public bool DoNotAutoGainNavigationOnInit
		{
			get
			{
				return this.NavigationScope.DoNotAutoGainNavigationOnInit;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutoGainNavigationOnInit)
				{
					this.NavigationScope.DoNotAutoGainNavigationOnInit = value;
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000931D File Offset: 0x0000751D
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0000932A File Offset: 0x0000752A
		public bool ForceGainNavigationBasedOnDirection
		{
			get
			{
				return this.NavigationScope.ForceGainNavigationBasedOnDirection;
			}
			set
			{
				if (value != this.NavigationScope.ForceGainNavigationBasedOnDirection)
				{
					this.NavigationScope.ForceGainNavigationBasedOnDirection = value;
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00009346 File Offset: 0x00007546
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00009353 File Offset: 0x00007553
		public bool ForceGainNavigationOnClosestChild
		{
			get
			{
				return this.NavigationScope.ForceGainNavigationOnClosestChild;
			}
			set
			{
				if (value != this.NavigationScope.ForceGainNavigationOnClosestChild)
				{
					this.NavigationScope.ForceGainNavigationOnClosestChild = value;
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000936F File Offset: 0x0000756F
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000937C File Offset: 0x0000757C
		public bool ForceGainNavigationOnFirstChild
		{
			get
			{
				return this.NavigationScope.ForceGainNavigationOnFirstChild;
			}
			set
			{
				if (value != this.NavigationScope.ForceGainNavigationOnFirstChild)
				{
					this.NavigationScope.ForceGainNavigationOnFirstChild = value;
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00009398 File Offset: 0x00007598
		// (set) Token: 0x060002CB RID: 715 RVA: 0x000093A5 File Offset: 0x000075A5
		public bool NavigateFromScopeEdges
		{
			get
			{
				return this.NavigationScope.NavigateFromScopeEdges;
			}
			set
			{
				if (value != this.NavigationScope.NavigateFromScopeEdges)
				{
					this.NavigationScope.NavigateFromScopeEdges = value;
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002CC RID: 716 RVA: 0x000093C1 File Offset: 0x000075C1
		// (set) Token: 0x060002CD RID: 717 RVA: 0x000093CE File Offset: 0x000075CE
		public bool UseDiscoveryAreaAsScopeEdges
		{
			get
			{
				return this.NavigationScope.UseDiscoveryAreaAsScopeEdges;
			}
			set
			{
				if (value != this.NavigationScope.UseDiscoveryAreaAsScopeEdges)
				{
					this.NavigationScope.UseDiscoveryAreaAsScopeEdges = value;
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000093EA File Offset: 0x000075EA
		// (set) Token: 0x060002CF RID: 719 RVA: 0x000093F7 File Offset: 0x000075F7
		public bool DoNotAutoNavigateAfterSort
		{
			get
			{
				return this.NavigationScope.DoNotAutoNavigateAfterSort;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutoNavigateAfterSort)
				{
					this.NavigationScope.DoNotAutoNavigateAfterSort = value;
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00009413 File Offset: 0x00007613
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00009420 File Offset: 0x00007620
		public bool FollowMobileTargets
		{
			get
			{
				return this.NavigationScope.FollowMobileTargets;
			}
			set
			{
				if (value != this.NavigationScope.FollowMobileTargets)
				{
					this.NavigationScope.FollowMobileTargets = value;
				}
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000943C File Offset: 0x0000763C
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00009449 File Offset: 0x00007649
		public bool DoNotAutoCollectChildScopes
		{
			get
			{
				return this.NavigationScope.DoNotAutoCollectChildScopes;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutoCollectChildScopes)
				{
					this.NavigationScope.DoNotAutoCollectChildScopes = value;
				}
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00009465 File Offset: 0x00007665
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00009472 File Offset: 0x00007672
		public bool IsDefaultNavigationScope
		{
			get
			{
				return this.NavigationScope.IsDefaultNavigationScope;
			}
			set
			{
				if (value != this.NavigationScope.IsDefaultNavigationScope)
				{
					this.NavigationScope.IsDefaultNavigationScope = value;
				}
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000948E File Offset: 0x0000768E
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000949B File Offset: 0x0000769B
		public float ExtendDiscoveryAreaTop
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaTop;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaTop)
				{
					this.NavigationScope.ExtendDiscoveryAreaTop = value;
				}
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x000094B7 File Offset: 0x000076B7
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x000094C4 File Offset: 0x000076C4
		public float ExtendDiscoveryAreaRight
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaRight;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaRight)
				{
					this.NavigationScope.ExtendDiscoveryAreaRight = value;
				}
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000094E0 File Offset: 0x000076E0
		// (set) Token: 0x060002DB RID: 731 RVA: 0x000094ED File Offset: 0x000076ED
		public float ExtendDiscoveryAreaBottom
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaBottom;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaBottom)
				{
					this.NavigationScope.ExtendDiscoveryAreaBottom = value;
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00009509 File Offset: 0x00007709
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00009516 File Offset: 0x00007716
		public float ExtendDiscoveryAreaLeft
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaLeft;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaLeft)
				{
					this.NavigationScope.ExtendDiscoveryAreaLeft = value;
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00009532 File Offset: 0x00007732
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000953F File Offset: 0x0000773F
		public float ExtendChildrenCursorAreaLeft
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaLeft;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaLeft)
				{
					this.NavigationScope.ExtendChildrenCursorAreaLeft = value;
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000955B File Offset: 0x0000775B
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00009568 File Offset: 0x00007768
		public float ExtendChildrenCursorAreaRight
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaRight;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaRight)
				{
					this.NavigationScope.ExtendChildrenCursorAreaRight = value;
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00009584 File Offset: 0x00007784
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00009591 File Offset: 0x00007791
		public float ExtendChildrenCursorAreaTop
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaTop;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaTop)
				{
					this.NavigationScope.ExtendChildrenCursorAreaTop = value;
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x000095AD File Offset: 0x000077AD
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x000095BA File Offset: 0x000077BA
		public float ExtendChildrenCursorAreaBottom
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaBottom;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaBottom)
				{
					this.NavigationScope.ExtendChildrenCursorAreaBottom = value;
				}
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000095D6 File Offset: 0x000077D6
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x000095E3 File Offset: 0x000077E3
		public float DiscoveryAreaOffsetX
		{
			get
			{
				return this.NavigationScope.DiscoveryAreaOffsetX;
			}
			set
			{
				if (value != this.NavigationScope.DiscoveryAreaOffsetX)
				{
					this.NavigationScope.DiscoveryAreaOffsetX = value;
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000095FF File Offset: 0x000077FF
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000960C File Offset: 0x0000780C
		public float DiscoveryAreaOffsetY
		{
			get
			{
				return this.NavigationScope.DiscoveryAreaOffsetY;
			}
			set
			{
				if (value != this.NavigationScope.DiscoveryAreaOffsetY)
				{
					this.NavigationScope.DiscoveryAreaOffsetY = value;
				}
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00009628 File Offset: 0x00007828
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00009635 File Offset: 0x00007835
		public bool IsScopeEnabled
		{
			get
			{
				return this.NavigationScope.IsEnabled;
			}
			set
			{
				if (value != this.NavigationScope.IsEnabled)
				{
					this.NavigationScope.IsEnabled = value;
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00009651 File Offset: 0x00007851
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000965E File Offset: 0x0000785E
		public bool IsScopeDisabled
		{
			get
			{
				return this.NavigationScope.IsDisabled;
			}
			set
			{
				if (value != this.NavigationScope.IsDisabled)
				{
					this.NavigationScope.IsDisabled = value;
				}
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000967A File Offset: 0x0000787A
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00009687 File Offset: 0x00007887
		public string UpNavigationScope
		{
			get
			{
				return this.NavigationScope.UpNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.UpNavigationScopeID)
				{
					this.NavigationScope.UpNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x000096A8 File Offset: 0x000078A8
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x000096B5 File Offset: 0x000078B5
		public string RightNavigationScope
		{
			get
			{
				return this.NavigationScope.RightNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.RightNavigationScopeID)
				{
					this.NavigationScope.RightNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000096D6 File Offset: 0x000078D6
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000096E3 File Offset: 0x000078E3
		public string DownNavigationScope
		{
			get
			{
				return this.NavigationScope.DownNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.DownNavigationScopeID)
				{
					this.NavigationScope.DownNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00009704 File Offset: 0x00007904
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00009711 File Offset: 0x00007911
		public string LeftNavigationScope
		{
			get
			{
				return this.NavigationScope.LeftNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.LeftNavigationScopeID)
				{
					this.NavigationScope.LeftNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00009732 File Offset: 0x00007932
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000973A File Offset: 0x0000793A
		public NavigationScopeTargeter UpNavigationScopeTargeter
		{
			get
			{
				return this._upNavigationScopeTargeter;
			}
			set
			{
				if (value != this._upNavigationScopeTargeter)
				{
					this._upNavigationScopeTargeter = value;
					this.NavigationScope.UpNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000975D File Offset: 0x0000795D
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x00009765 File Offset: 0x00007965
		public NavigationScopeTargeter RightNavigationScopeTargeter
		{
			get
			{
				return this._rightNavigationScopeTargeter;
			}
			set
			{
				if (value != this._rightNavigationScopeTargeter)
				{
					this._rightNavigationScopeTargeter = value;
					this.NavigationScope.RightNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00009788 File Offset: 0x00007988
		// (set) Token: 0x060002FB RID: 763 RVA: 0x00009790 File Offset: 0x00007990
		public NavigationScopeTargeter DownNavigationScopeTargeter
		{
			get
			{
				return this._downNavigationScopeTargeter;
			}
			set
			{
				if (value != this._downNavigationScopeTargeter)
				{
					this._downNavigationScopeTargeter = value;
					this.NavigationScope.DownNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002FC RID: 764 RVA: 0x000097B3 File Offset: 0x000079B3
		// (set) Token: 0x060002FD RID: 765 RVA: 0x000097BB File Offset: 0x000079BB
		public NavigationScopeTargeter LeftNavigationScopeTargeter
		{
			get
			{
				return this._leftNavigationScopeTargeter;
			}
			set
			{
				if (value != this._leftNavigationScopeTargeter)
				{
					this._leftNavigationScopeTargeter = value;
					this.NavigationScope.LeftNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002FE RID: 766 RVA: 0x000097DE File Offset: 0x000079DE
		// (set) Token: 0x060002FF RID: 767 RVA: 0x000097EC File Offset: 0x000079EC
		public Widget ScopeParent
		{
			get
			{
				return this.NavigationScope.ParentWidget;
			}
			set
			{
				if (this.NavigationScope.ParentWidget != value)
				{
					if (this.NavigationScope.ParentWidget != null)
					{
						base.GamepadNavigationContext.RemoveNavigationScope(this.NavigationScope);
					}
					this.NavigationScope.ParentWidget = value;
					this.NavigationScope.ParentWidget.EventFire += this.OnParentConnectedToTheRoot;
					base.GamepadNavigationContext.AddNavigationScope(this.NavigationScope, false);
				}
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000985F File Offset: 0x00007A5F
		private void OnParentConnectedToTheRoot(Widget widget, string eventName, object[] arguments)
		{
			if (eventName == "ConnectedToRoot" && !base.GamepadNavigationContext.HasNavigationScope(this.NavigationScope))
			{
				base.GamepadNavigationContext.AddNavigationScope(this.NavigationScope, false);
			}
		}

		// Token: 0x04000132 RID: 306
		private NavigationScopeTargeter _upNavigationScopeTargeter;

		// Token: 0x04000133 RID: 307
		private NavigationScopeTargeter _rightNavigationScopeTargeter;

		// Token: 0x04000134 RID: 308
		private NavigationScopeTargeter _downNavigationScopeTargeter;

		// Token: 0x04000135 RID: 309
		private NavigationScopeTargeter _leftNavigationScopeTargeter;
	}
}
