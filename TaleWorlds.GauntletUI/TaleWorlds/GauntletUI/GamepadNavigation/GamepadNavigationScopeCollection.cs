using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x0200004E RID: 78
	internal class GamepadNavigationScopeCollection
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00014125 File Offset: 0x00012325
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001412D File Offset: 0x0001232D
		public IGamepadNavigationContext Source { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00014136 File Offset: 0x00012336
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x0001413E File Offset: 0x0001233E
		public ReadOnlyCollection<GamepadNavigationScope> AllScopes { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00014147 File Offset: 0x00012347
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x0001414F File Offset: 0x0001234F
		public ReadOnlyCollection<GamepadNavigationScope> UninitializedScopes { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00014158 File Offset: 0x00012358
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x00014160 File Offset: 0x00012360
		public ReadOnlyCollection<GamepadNavigationScope> VisibleScopes { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00014169 File Offset: 0x00012369
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x00014171 File Offset: 0x00012371
		public ReadOnlyCollection<GamepadNavigationScope> InvisibleScopes { get; private set; }

		// Token: 0x0600051B RID: 1307 RVA: 0x0001417C File Offset: 0x0001237C
		public GamepadNavigationScopeCollection(IGamepadNavigationContext source, Action<GamepadNavigationScope> onScopeNavigatableWidgetsChanged, Action<GamepadNavigationScope, bool> onScopeVisibilityChanged)
		{
			this._onScopeNavigatableWidgetsChanged = onScopeNavigatableWidgetsChanged;
			this._onScopeVisibilityChanged = onScopeVisibilityChanged;
			this.Source = source;
			this._allScopes = new List<GamepadNavigationScope>();
			this.AllScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._allScopes);
			this._uninitializedScopes = new List<GamepadNavigationScope>();
			this.UninitializedScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._uninitializedScopes);
			this._visibleScopes = new List<GamepadNavigationScope>();
			this.VisibleScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._visibleScopes);
			this._invisibleScopes = new List<GamepadNavigationScope>();
			this.InvisibleScopes = new ReadOnlyCollection<GamepadNavigationScope>(this._invisibleScopes);
			this._dirtyScopes = new List<GamepadNavigationScope>();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001421F File Offset: 0x0001241F
		internal void OnFinalize()
		{
			this.ClearAllScopes();
			this._onScopeVisibilityChanged = null;
			this._onScopeNavigatableWidgetsChanged = null;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00014238 File Offset: 0x00012438
		internal void HandleScopeVisibilities()
		{
			List<GamepadNavigationScope> dirtyScopes = this._dirtyScopes;
			lock (dirtyScopes)
			{
				for (int i = 0; i < this._dirtyScopes.Count; i++)
				{
					if (this._dirtyScopes[i] != null)
					{
						for (int j = i + 1; j < this._dirtyScopes.Count; j++)
						{
							if (this._dirtyScopes[i] == this._dirtyScopes[j])
							{
								this._dirtyScopes[j] = null;
							}
						}
					}
				}
				foreach (GamepadNavigationScope gamepadNavigationScope in this._dirtyScopes)
				{
					if (gamepadNavigationScope != null)
					{
						bool flag2 = gamepadNavigationScope.IsVisible();
						this._visibleScopes.Remove(gamepadNavigationScope);
						this._invisibleScopes.Remove(gamepadNavigationScope);
						if (flag2)
						{
							this._visibleScopes.Add(gamepadNavigationScope);
						}
						else
						{
							this._invisibleScopes.Add(gamepadNavigationScope);
						}
						this._onScopeVisibilityChanged(gamepadNavigationScope, flag2);
					}
				}
				this._dirtyScopes.Clear();
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00014390 File Offset: 0x00012590
		private void OnScopeVisibilityChanged(GamepadNavigationScope scope, bool isVisible)
		{
			List<GamepadNavigationScope> dirtyScopes = this._dirtyScopes;
			lock (dirtyScopes)
			{
				this._dirtyScopes.Add(scope);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000143D8 File Offset: 0x000125D8
		private void OnScopeNavigatableWidgetsChanged(GamepadNavigationScope scope)
		{
			this._onScopeNavigatableWidgetsChanged(scope);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000143E6 File Offset: 0x000125E6
		internal int GetTotalNumberOfScopes()
		{
			return this._visibleScopes.Count + this._invisibleScopes.Count + this._uninitializedScopes.Count;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001440B File Offset: 0x0001260B
		internal void AddScope(GamepadNavigationScope scope)
		{
			this._uninitializedScopes.Add(scope);
			this._allScopes.Add(scope);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00014428 File Offset: 0x00012628
		internal void RemoveScope(GamepadNavigationScope scope)
		{
			this._allScopes.Remove(scope);
			this._uninitializedScopes.Remove(scope);
			this._visibleScopes.Remove(scope);
			this._invisibleScopes.Remove(scope);
			scope.OnVisibilityChanged = (Action<GamepadNavigationScope, bool>)Delegate.Remove(scope.OnVisibilityChanged, new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged));
			scope.OnNavigatableWidgetsChanged = (Action<GamepadNavigationScope>)Delegate.Remove(scope.OnNavigatableWidgetsChanged, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged));
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000144AD File Offset: 0x000126AD
		internal bool HasScopeInAnyList(GamepadNavigationScope scope)
		{
			return this._visibleScopes.Contains(scope) || this._invisibleScopes.Contains(scope) || this._uninitializedScopes.Contains(scope);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000144DC File Offset: 0x000126DC
		internal void OnNavigationScopeInitialized(GamepadNavigationScope scope)
		{
			this._uninitializedScopes.Remove(scope);
			if (scope.IsVisible())
			{
				this._visibleScopes.Add(scope);
			}
			else
			{
				this._invisibleScopes.Add(scope);
			}
			scope.OnVisibilityChanged = (Action<GamepadNavigationScope, bool>)Delegate.Combine(scope.OnVisibilityChanged, new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged));
			scope.OnNavigatableWidgetsChanged = (Action<GamepadNavigationScope>)Delegate.Combine(scope.OnNavigatableWidgetsChanged, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged));
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001455C File Offset: 0x0001275C
		internal void OnWidgetDisconnectedFromRoot(Widget widget)
		{
			for (int i = 0; i < this._visibleScopes.Count; i++)
			{
				if (this._visibleScopes[i].FindIndexOfWidget(widget) != -1)
				{
					this._visibleScopes[i].RemoveWidget(widget);
					return;
				}
			}
			for (int j = 0; j < this._invisibleScopes.Count; j++)
			{
				if (this._invisibleScopes[j].FindIndexOfWidget(widget) != -1)
				{
					this._invisibleScopes[j].RemoveWidget(widget);
					return;
				}
			}
			for (int k = 0; k < this._uninitializedScopes.Count; k++)
			{
				if (this._uninitializedScopes[k].FindIndexOfWidget(widget) != -1)
				{
					this._uninitializedScopes[k].RemoveWidget(widget);
					return;
				}
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00014624 File Offset: 0x00012824
		private void ClearAllScopes()
		{
			for (int i = 0; i < this._allScopes.Count; i++)
			{
				this._allScopes[i].ClearNavigatableWidgets();
				GamepadNavigationScope gamepadNavigationScope = this._allScopes[i];
				gamepadNavigationScope.OnNavigatableWidgetsChanged = (Action<GamepadNavigationScope>)Delegate.Remove(gamepadNavigationScope.OnNavigatableWidgetsChanged, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged));
				GamepadNavigationScope gamepadNavigationScope2 = this._allScopes[i];
				gamepadNavigationScope2.OnVisibilityChanged = (Action<GamepadNavigationScope, bool>)Delegate.Remove(gamepadNavigationScope2.OnVisibilityChanged, new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged));
			}
			this._allScopes.Clear();
			this._uninitializedScopes.Clear();
			this._invisibleScopes.Clear();
			this._visibleScopes.Clear();
			this._allScopes = null;
			this._uninitializedScopes = null;
			this._invisibleScopes = null;
			this._visibleScopes = null;
		}

		// Token: 0x04000267 RID: 615
		private Action<GamepadNavigationScope> _onScopeNavigatableWidgetsChanged;

		// Token: 0x04000268 RID: 616
		private Action<GamepadNavigationScope, bool> _onScopeVisibilityChanged;

		// Token: 0x04000269 RID: 617
		private List<GamepadNavigationScope> _allScopes;

		// Token: 0x0400026A RID: 618
		private List<GamepadNavigationScope> _uninitializedScopes;

		// Token: 0x0400026B RID: 619
		private List<GamepadNavigationScope> _visibleScopes;

		// Token: 0x0400026C RID: 620
		private List<GamepadNavigationScope> _invisibleScopes;

		// Token: 0x0400026D RID: 621
		private List<GamepadNavigationScope> _dirtyScopes;
	}
}
