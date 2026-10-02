using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000031 RID: 49
	public class NavigationForcedScopeCollectionTargeter : Widget
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00008FCE File Offset: 0x000071CE
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00008FD6 File Offset: 0x000071D6
		public bool UseRootAsTarget
		{
			get
			{
				return this._useRootAsTarget;
			}
			set
			{
				if (this._useRootAsTarget != value)
				{
					this._useRootAsTarget = value;
					if (base.Context.Root != null && this._useRootAsTarget)
					{
						this.CollectionParent = base.Context.Root;
					}
				}
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000900E File Offset: 0x0000720E
		public NavigationForcedScopeCollectionTargeter(UIContext context)
			: base(context)
		{
			this._collection = new GamepadNavigationForcedScopeCollection();
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000904D File Offset: 0x0000724D
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.UseRootAsTarget)
			{
				this.CollectionParent = base.Context.Root;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000906E File Offset: 0x0000726E
		protected override void OnDisconnectedFromRoot()
		{
			this.CollectionParent = null;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00009077 File Offset: 0x00007277
		// (set) Token: 0x060002AA RID: 682 RVA: 0x00009084 File Offset: 0x00007284
		public bool IsCollectionEnabled
		{
			get
			{
				return this._collection.IsEnabled;
			}
			set
			{
				if (value != this._collection.IsEnabled)
				{
					this._collection.IsEnabled = value;
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002AB RID: 683 RVA: 0x000090A0 File Offset: 0x000072A0
		// (set) Token: 0x060002AC RID: 684 RVA: 0x000090AD File Offset: 0x000072AD
		public bool IsCollectionDisabled
		{
			get
			{
				return this._collection.IsDisabled;
			}
			set
			{
				if (value != this._collection.IsDisabled)
				{
					this._collection.IsDisabled = value;
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000090C9 File Offset: 0x000072C9
		// (set) Token: 0x060002AE RID: 686 RVA: 0x000090D6 File Offset: 0x000072D6
		public string CollectionID
		{
			get
			{
				return this._collection.CollectionID;
			}
			set
			{
				if (value != this._collection.CollectionID)
				{
					this._collection.CollectionID = value;
				}
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000090F7 File Offset: 0x000072F7
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00009104 File Offset: 0x00007304
		public int CollectionOrder
		{
			get
			{
				return this._collection.CollectionOrder;
			}
			set
			{
				if (value != this._collection.CollectionOrder)
				{
					this._collection.CollectionOrder = value;
				}
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00009120 File Offset: 0x00007320
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00009130 File Offset: 0x00007330
		public Widget CollectionParent
		{
			get
			{
				return this._collection.ParentWidget;
			}
			set
			{
				if (this._collection.ParentWidget != value)
				{
					if (this._collection.ParentWidget != null)
					{
						base.GamepadNavigationContext.RemoveForcedScopeCollection(this._collection);
					}
					if (!this.UseRootAsTarget || value == base.Context.Root)
					{
						this._collection.ParentWidget = value;
					}
					if (this._collection.ParentWidget != null)
					{
						base.GamepadNavigationContext.AddForcedScopeCollection(this._collection);
					}
				}
			}
		}

		// Token: 0x0400012F RID: 303
		private bool _useRootAsTarget;

		// Token: 0x04000130 RID: 304
		private readonly GamepadNavigationForcedScopeCollection _collection;
	}
}
