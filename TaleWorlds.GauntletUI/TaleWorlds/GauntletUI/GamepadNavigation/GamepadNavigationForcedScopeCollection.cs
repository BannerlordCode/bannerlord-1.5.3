using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x0200004B RID: 75
	public class GamepadNavigationForcedScopeCollection
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00011FC5 File Offset: 0x000101C5
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x00011FCD File Offset: 0x000101CD
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
					Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
					if (onAvailabilityChanged == null)
					{
						return;
					}
					onAvailabilityChanged(this);
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00011FF0 File Offset: 0x000101F0
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x00011FFB File Offset: 0x000101FB
		public bool IsDisabled
		{
			get
			{
				return !this.IsEnabled;
			}
			set
			{
				if (value == this.IsEnabled)
				{
					this.IsEnabled = !value;
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00012010 File Offset: 0x00010210
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x00012018 File Offset: 0x00010218
		public string CollectionID { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00012021 File Offset: 0x00010221
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x00012029 File Offset: 0x00010229
		public int CollectionOrder { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00012032 File Offset: 0x00010232
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x0001203C File Offset: 0x0001023C
		public Widget ParentWidget
		{
			get
			{
				return this._parentWidget;
			}
			set
			{
				if (value != this._parentWidget)
				{
					if (this._parentWidget != null)
					{
						this._invisibleParents.Clear();
						for (Widget widget = this._parentWidget; widget != null; widget = widget.ParentWidget)
						{
							widget.OnVisibilityChanged -= this.OnParentVisibilityChanged;
						}
					}
					this._parentWidget = value;
					for (Widget widget2 = this._parentWidget; widget2 != null; widget2 = widget2.ParentWidget)
					{
						if (!widget2.IsVisible)
						{
							this._invisibleParents.Add(widget2);
						}
						widget2.OnVisibilityChanged += this.OnParentVisibilityChanged;
					}
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x000120CA File Offset: 0x000102CA
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x000120D2 File Offset: 0x000102D2
		public List<GamepadNavigationScope> Scopes { get; private set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x000120DB File Offset: 0x000102DB
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x000120E3 File Offset: 0x000102E3
		public GamepadNavigationScope ActiveScope { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x000120EC File Offset: 0x000102EC
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x000120F4 File Offset: 0x000102F4
		public GamepadNavigationScope PreviousScope { get; set; }

		// Token: 0x06000477 RID: 1143 RVA: 0x000120FD File Offset: 0x000102FD
		public GamepadNavigationForcedScopeCollection()
		{
			this.Scopes = new List<GamepadNavigationScope>();
			this._invisibleParents = new List<Widget>();
			this.IsEnabled = true;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00012124 File Offset: 0x00010324
		private void OnParentVisibilityChanged(Widget parent)
		{
			bool flag = this._invisibleParents.Count == 0;
			if (!parent.IsVisible)
			{
				this._invisibleParents.Add(parent);
			}
			else
			{
				this._invisibleParents.Remove(parent);
			}
			bool flag2 = this._invisibleParents.Count == 0;
			if (flag != flag2)
			{
				Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
				if (onAvailabilityChanged == null)
				{
					return;
				}
				onAvailabilityChanged(this);
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00012188 File Offset: 0x00010388
		public bool IsAvailable()
		{
			if (this.IsEnabled && this._invisibleParents.Count == 0)
			{
				if (this.Scopes.Any<GamepadNavigationScope>((GamepadNavigationScope x) => x.IsAvailable()))
				{
					return this.ParentWidget.Context.GamepadNavigation.IsAvailableForNavigation();
				}
			}
			return false;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000121ED File Offset: 0x000103ED
		public void AddScope(GamepadNavigationScope scope)
		{
			if (!this.Scopes.Contains(scope))
			{
				this.Scopes.Add(scope);
			}
			Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
			if (onAvailabilityChanged == null)
			{
				return;
			}
			onAvailabilityChanged(this);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001221A File Offset: 0x0001041A
		public void RemoveScope(GamepadNavigationScope scope)
		{
			if (this.Scopes.Contains(scope))
			{
				this.Scopes.Remove(scope);
			}
			Action<GamepadNavigationForcedScopeCollection> onAvailabilityChanged = this.OnAvailabilityChanged;
			if (onAvailabilityChanged == null)
			{
				return;
			}
			onAvailabilityChanged(this);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00012248 File Offset: 0x00010448
		public void ClearScopes()
		{
			this.Scopes.Clear();
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00012255 File Offset: 0x00010455
		public override string ToString()
		{
			return string.Format("ID:{0} C.C.:{1}", this.CollectionID, this.Scopes.Count);
		}

		// Token: 0x0400022E RID: 558
		public Action<GamepadNavigationForcedScopeCollection> OnAvailabilityChanged;

		// Token: 0x0400022F RID: 559
		private List<Widget> _invisibleParents;

		// Token: 0x04000230 RID: 560
		private bool _isEnabled;

		// Token: 0x04000233 RID: 563
		private Widget _parentWidget;
	}
}
