using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x02000050 RID: 80
	public class GauntletGamepadNavigationManager
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x000146FA File Offset: 0x000128FA
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00014701 File Offset: 0x00012901
		public static GauntletGamepadNavigationManager Instance { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0001470C File Offset: 0x0001290C
		private IGamepadNavigationContext LatestContext
		{
			get
			{
				if (this._latestCachedContext == null)
				{
					for (int i = 0; i < this._sortedNavigationContexts.Count; i++)
					{
						if (this._sortedNavigationContexts[i].IsAvailableForNavigation())
						{
							this._latestCachedContext = this._sortedNavigationContexts[i];
							break;
						}
					}
				}
				return this._latestCachedContext;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00014764 File Offset: 0x00012964
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x0001476C File Offset: 0x0001296C
		public bool IsTouchpadMouseEnabled { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00014775 File Offset: 0x00012975
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0001477D File Offset: 0x0001297D
		public bool IsFollowingMobileTarget { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00014786 File Offset: 0x00012986
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0001478E File Offset: 0x0001298E
		public bool IsHoldingDpadKeysForNavigation { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00014797 File Offset: 0x00012997
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0001479F File Offset: 0x0001299F
		public bool IsCursorMovingForNavigation { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000147A8 File Offset: 0x000129A8
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x000147B0 File Offset: 0x000129B0
		public bool IsInWrapMovement { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x000147B9 File Offset: 0x000129B9
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x000147CA File Offset: 0x000129CA
		private Vector2 MousePosition
		{
			get
			{
				return (Vector2)Input.InputState.MousePositionPixel;
			}
			set
			{
				Input.SetMousePosition((int)value.X, (int)value.Y);
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x000147DF File Offset: 0x000129DF
		private bool IsControllerActive
		{
			get
			{
				return Input.IsGamepadActive;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x000147E6 File Offset: 0x000129E6
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x000147EE File Offset: 0x000129EE
		internal ReadOnlyDictionary<IGamepadNavigationContext, GamepadNavigationScopeCollection> NavigationScopes { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000147F7 File Offset: 0x000129F7
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x000147FF File Offset: 0x000129FF
		internal ReadOnlyDictionary<Widget, List<GamepadNavigationScope>> NavigationScopeParents { get; private set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00014808 File Offset: 0x00012A08
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00014810 File Offset: 0x00012A10
		internal ReadOnlyDictionary<Widget, List<GamepadNavigationForcedScopeCollection>> ForcedNavigationScopeParents { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001481C File Offset: 0x00012A1C
		public Widget LastTargetedWidget
		{
			get
			{
				GamepadNavigationScope activeNavigationScope = this._activeNavigationScope;
				Widget widget = ((activeNavigationScope != null) ? activeNavigationScope.LastNavigatedWidget : null);
				if (widget != null && (this.IsCursorMovingForNavigation || widget.IsPointInsideGamepadCursorArea(this.MousePosition)))
				{
					return widget;
				}
				return null;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00014858 File Offset: 0x00012A58
		public bool TargetedWidgetHasAction
		{
			get
			{
				if (this.LastTargetedWidget != null)
				{
					if (this.LastTargetedWidget.UsedNavigationMovements != GamepadNavigationTypes.None)
					{
						return true;
					}
					List<Widget> allParents = this.LastTargetedWidget.GetAllParents();
					for (int i = 0; i < allParents.Count; i++)
					{
						if (allParents[i].UsedNavigationMovements != GamepadNavigationTypes.None)
						{
							return true;
						}
					}
					if (this.LastTargetedWidget.GetFirstInChildrenAndThisRecursive((Widget child) => child.UsedNavigationMovements > GamepadNavigationTypes.None) != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x000148D8 File Offset: 0x00012AD8
		public bool AnyWidgetUsingNavigation
		{
			get
			{
				return this._navigationBlockingWidgets.Any<Widget>((Widget x) => x.IsUsingNavigation);
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00014904 File Offset: 0x00012B04
		private GauntletGamepadNavigationManager()
		{
			this._cachedNavigationContextComparer = new GauntletGamepadNavigationManager.GamepadNavigationContextComparer();
			this._cachedForcedScopeComparer = new GauntletGamepadNavigationManager.ForcedScopeComparer();
			this._navigationScopes = new Dictionary<IGamepadNavigationContext, GamepadNavigationScopeCollection>();
			this.NavigationScopes = new ReadOnlyDictionary<IGamepadNavigationContext, GamepadNavigationScopeCollection>(this._navigationScopes);
			this._navigationScopeParents = new Dictionary<Widget, List<GamepadNavigationScope>>();
			this._forcedNavigationScopeCollectionParents = new Dictionary<Widget, List<GamepadNavigationForcedScopeCollection>>();
			this.NavigationScopeParents = new ReadOnlyDictionary<Widget, List<GamepadNavigationScope>>(this._navigationScopeParents);
			this.ForcedNavigationScopeParents = new ReadOnlyDictionary<Widget, List<GamepadNavigationForcedScopeCollection>>(this._forcedNavigationScopeCollectionParents);
			this._sortedNavigationContexts = new List<IGamepadNavigationContext>();
			this._availableScopesThisFrame = new List<GamepadNavigationScope>();
			this._unsortedScopes = new List<GamepadNavigationScope>();
			this._forcedScopeCollections = new List<GamepadNavigationForcedScopeCollection>();
			this._layerNavigationScopes = new Dictionary<string, List<GamepadNavigationScope>>();
			this._navigationScopesById = new Dictionary<string, List<GamepadNavigationScope>>();
			this._navigationGainControllers = new Dictionary<IGamepadNavigationContext, GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler>();
			this._navigationBlockingWidgets = new List<Widget>();
			this._isAvailableScopesDirty = false;
			this._isForcedCollectionsDirty = false;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00014A32 File Offset: 0x00012C32
		private void OnGamepadActiveStateChanged()
		{
			if (this.IsControllerActive && Input.MouseMoveX == 0f && Input.MouseMoveY == 0f)
			{
				this._isAvailableScopesDirty = true;
				this._isForcedCollectionsDirty = true;
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00014A62 File Offset: 0x00012C62
		public static void Initialize()
		{
			if (GauntletGamepadNavigationManager.Instance != null)
			{
				Debug.FailedAssert("Gamepad Navigation already initialized", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "Initialize", 224);
				return;
			}
			GauntletGamepadNavigationManager.Instance = new GauntletGamepadNavigationManager();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00014A90 File Offset: 0x00012C90
		public bool TryNavigateTo(Widget widget)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (widget != null && widget.GamepadNavigationIndex != -1 && this._navigationScopes.TryGetValue(widget.GamepadNavigationContext, out gamepadNavigationScopeCollection))
			{
				for (int i = 0; i < gamepadNavigationScopeCollection.VisibleScopes.Count; i++)
				{
					GamepadNavigationScope gamepadNavigationScope = gamepadNavigationScopeCollection.VisibleScopes[i];
					if (gamepadNavigationScope.IsAvailable() && (gamepadNavigationScope.ParentWidget == widget || gamepadNavigationScope.ParentWidget.CheckIsMyChildRecursive(widget)))
					{
						return this.SetCurrentNavigatedWidget(gamepadNavigationScope, widget);
					}
				}
			}
			return false;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00014B0C File Offset: 0x00012D0C
		public bool TryNavigateTo(GamepadNavigationScope scope)
		{
			if (scope != null && scope.IsAvailable())
			{
				Widget approximatelyClosestWidgetToPosition = scope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, GamepadNavigationTypes.None, false);
				if (approximatelyClosestWidgetToPosition != null)
				{
					return this.SetCurrentNavigatedWidget(scope, approximatelyClosestWidgetToPosition);
				}
			}
			return false;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00014B40 File Offset: 0x00012D40
		public void OnFinalize()
		{
			foreach (KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection> keyValuePair in this._navigationScopes)
			{
				keyValuePair.Value.OnFinalize();
			}
			this._navigationScopes.Clear();
			this._navigationScopeParents.Clear();
			GauntletGamepadNavigationManager.Instance = null;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00014BD4 File Offset: 0x00012DD4
		public void Update(float dt)
		{
			this._time += dt;
			if (this._stopCursorNextFrame)
			{
				this.IsCursorMovingForNavigation = false;
				this._stopCursorNextFrame = false;
			}
			if (this.IsControllerActive && Input.MouseMoveX <= 0f && Input.MouseMoveY <= 0f)
			{
				GamepadNavigationScope activeNavigationScope = this._activeNavigationScope;
				if (activeNavigationScope != null && activeNavigationScope.IsAvailable() && this._activeNavigationScope.ParentWidget.Context.GamepadNavigation.IsAvailableForNavigation() && (!Input.IsAnyTouchActive || !this.IsTouchpadMouseEnabled))
				{
					goto IL_008C;
				}
			}
			this.OnDpadNavigationStopped();
			IL_008C:
			foreach (KeyValuePair<IGamepadNavigationContext, GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler> keyValuePair in this._navigationGainControllers)
			{
				keyValuePair.Value.Tick(dt);
			}
			if (this.LastTargetedWidget != null)
			{
				Vector2.Distance(this.LastTargetedWidget.AreaRect.GetCenter(), this.MousePosition);
			}
			if (Input.GetKeyState(InputKey.ControllerRStick).X == 0f)
			{
				bool flag = Input.GetKeyState(InputKey.ControllerRStick).Y != 0f;
			}
			if (this._autoRefreshTimer > -1f)
			{
				this._autoRefreshTimer += dt;
				if (this._autoRefreshTimer > 0.6f)
				{
					this._autoRefreshTimer = -1f;
					this._isAvailableScopesDirty = true;
				}
			}
			if (!this._isAvailableScopesDirty)
			{
				GamepadNavigationScope activeNavigationScope2 = this._activeNavigationScope;
				if (activeNavigationScope2 == null || !activeNavigationScope2.IsAvailable())
				{
					this._isAvailableScopesDirty = true;
				}
			}
			this._sortedNavigationContexts.Clear();
			foreach (KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection> keyValuePair2 in this._navigationScopes)
			{
				this._sortedNavigationContexts.Add(keyValuePair2.Key);
				keyValuePair2.Value.HandleScopeVisibilities();
			}
			this._sortedNavigationContexts.Sort(this._cachedNavigationContextComparer);
			foreach (KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection> keyValuePair3 in this._navigationScopes)
			{
				if (keyValuePair3.Value.UninitializedScopes.Count > 0)
				{
					List<GamepadNavigationScope> list = keyValuePair3.Value.UninitializedScopes.ToList<GamepadNavigationScope>();
					for (int i = 0; i < list.Count; i++)
					{
						this.InitializeScope(keyValuePair3.Key, list[i]);
					}
				}
			}
			if (this._unsortedScopes.Count > 0)
			{
				bool flag2 = false;
				for (int j = 0; j < this._unsortedScopes.Count; j++)
				{
					if (this._unsortedScopes[j] == this._activeNavigationScope)
					{
						flag2 = true;
					}
					this._unsortedScopes[j].SortWidgets();
				}
				this._unsortedScopes.Clear();
				if (flag2 && !this._activeNavigationScope.DoNotAutoNavigateAfterSort && this._activeNavigationScope != null && this._activeNavigationScope.IsAvailable() && (this._wasCursorInsideActiveScopeLastFrame || this._activeNavigationScope.GetRectangle().IsPointInside(this.MousePosition)))
				{
					if (this._activeNavigationScope.ForceGainNavigationOnClosestChild)
					{
						this.MoveCursorToClosestAvailableWidgetInScope(this._activeNavigationScope);
					}
					else
					{
						this.MoveCursorToFirstAvailableWidgetInScope(this._activeNavigationScope);
					}
				}
			}
			if (this._activeForcedScopeCollection != null && !this._activeForcedScopeCollection.IsAvailable())
			{
				this._isAvailableScopesDirty = true;
				this._isForcedCollectionsDirty = true;
			}
			if (this._shouldUpdateAvailableScopes)
			{
				GamepadNavigationForcedScopeCollection activeForcedScopeCollection = this._activeForcedScopeCollection;
				this._activeForcedScopeCollection = this.FindAvailableForcedScope();
				if (this._activeForcedScopeCollection != null && activeForcedScopeCollection == null)
				{
					this._activeForcedScopeCollection.PreviousScope = this._activeNavigationScope;
				}
				this.RefreshAvailableScopes();
				this._shouldUpdateAvailableScopes = false;
				if (activeForcedScopeCollection != null && !activeForcedScopeCollection.IsAvailable())
				{
					this.TryMoveCursorToPreviousScope(activeForcedScopeCollection);
				}
				else if (this._nextScopeToGainNavigation != null)
				{
					this.MoveCursorToFirstAvailableWidgetInScope(this._nextScopeToGainNavigation);
					this._nextScopeToGainNavigation = null;
				}
				else
				{
					GamepadNavigationScope activeNavigationScope3 = this._activeNavigationScope;
					if (activeNavigationScope3 == null || !activeNavigationScope3.IsAvailable() || !this._availableScopesThisFrame.Contains(this._activeNavigationScope))
					{
						this.MoveCursorToBestAvailableScope(false, GamepadNavigationTypes.None);
					}
				}
			}
			if (this._isAvailableScopesDirty)
			{
				this._shouldUpdateAvailableScopes = true;
				this._isAvailableScopesDirty = false;
			}
			this.HandleInput(dt);
			this.HandleCursorMovement();
			GamepadNavigationScope activeNavigationScope4 = this._activeNavigationScope;
			this._wasCursorInsideActiveScopeLastFrame = activeNavigationScope4 != null && activeNavigationScope4.GetRectangle().IsPointInside(this.MousePosition);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001506C File Offset: 0x0001326C
		internal void OnMovieLoaded(IGamepadNavigationContext context, string movieName)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
			{
				List<GamepadNavigationScope> list = gamepadNavigationScopeCollection.UninitializedScopes.ToList<GamepadNavigationScope>();
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].DoNotAutomaticallyFindChildren)
					{
						this.InitializeScope(context, list[i]);
					}
					this.AddItemToDictionaryList<string, GamepadNavigationScope>(this._layerNavigationScopes, movieName, list[i]);
				}
			}
			this._autoRefreshTimer = 0f;
			this._isAvailableScopesDirty = true;
			this._latestCachedContext = null;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000150F0 File Offset: 0x000132F0
		internal void OnMovieReleased(IGamepadNavigationContext context, string movieName)
		{
			List<GamepadNavigationScope> list;
			if (this._layerNavigationScopes.TryGetValue(movieName, out list))
			{
				List<GamepadNavigationScope> list2 = list.ToList<GamepadNavigationScope>();
				for (int i = 0; i < list2.Count; i++)
				{
					this.RemoveItemFromDictionaryList<string, GamepadNavigationScope>(this._layerNavigationScopes, movieName, list2[i]);
					this.RemoveNavigationScope(context, list2[i]);
				}
				this._latestCachedContext = null;
			}
			this._autoRefreshTimer = 0f;
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00015160 File Offset: 0x00013360
		internal void OnContextAdded(IGamepadNavigationContext context)
		{
			this._navigationScopes.Add(context, new GamepadNavigationScopeCollection(context, new Action<GamepadNavigationScope>(this.OnScopeNavigatableWidgetsChanged), new Action<GamepadNavigationScope, bool>(this.OnScopeVisibilityChanged)));
			this._navigationGainControllers.Add(context, new GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler(context));
			this._latestCachedContext = null;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000151B0 File Offset: 0x000133B0
		private void OnContextRemoved(IGamepadNavigationContext context)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
			{
				gamepadNavigationScopeCollection.OnFinalize();
				this._navigationScopes.Remove(context);
			}
			GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler contextGamepadNavigationGainHandler;
			if (this._navigationGainControllers.TryGetValue(context, out contextGamepadNavigationGainHandler))
			{
				contextGamepadNavigationGainHandler.Clear();
				this._navigationGainControllers.Remove(context);
			}
			this._sortedNavigationContexts.Remove(context);
			this._latestCachedContext = null;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00015218 File Offset: 0x00013418
		internal void OnContextFinalized(IGamepadNavigationContext context)
		{
			int count = this._sortedNavigationContexts.Count;
			this.OnContextRemoved(context);
			List<GamepadNavigationForcedScopeCollection> list = new List<GamepadNavigationForcedScopeCollection>();
			foreach (KeyValuePair<Widget, List<GamepadNavigationForcedScopeCollection>> keyValuePair in this._forcedNavigationScopeCollectionParents)
			{
				if (keyValuePair.Key.GamepadNavigationContext == context)
				{
					list.AddRange(keyValuePair.Value);
				}
			}
			foreach (GamepadNavigationForcedScopeCollection gamepadNavigationForcedScopeCollection in list)
			{
				this.RemoveForcedScopeCollection(gamepadNavigationForcedScopeCollection);
			}
			if (count != this._sortedNavigationContexts.Count)
			{
				this._sortedNavigationContexts = this._navigationScopes.Keys.ToList<IGamepadNavigationContext>();
				this._sortedNavigationContexts.Sort(this._cachedNavigationContextComparer);
			}
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00015318 File Offset: 0x00013518
		private Vector2 GetTargetCursorPosition()
		{
			if (this._latestGamepadNavigationWidget != null)
			{
				return this._latestGamepadNavigationWidget.GamepadCursorAreaRect.GetCenter();
			}
			return (Vector2)Vec2.Invalid;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001534C File Offset: 0x0001354C
		private void RefreshAvailableScopes()
		{
			this._availableScopesThisFrame.Clear();
			if (this._activeForcedScopeCollection != null)
			{
				for (int i = 0; i < this._activeForcedScopeCollection.Scopes.Count; i++)
				{
					this._availableScopesThisFrame.Add(this._activeForcedScopeCollection.Scopes[i]);
				}
				return;
			}
			for (int j = 0; j < this._sortedNavigationContexts.Count; j++)
			{
				IGamepadNavigationContext gamepadNavigationContext = this._sortedNavigationContexts[j];
				if (gamepadNavigationContext.IsAvailableForNavigation())
				{
					for (int k = 0; k < this._navigationScopes[gamepadNavigationContext].VisibleScopes.Count; k++)
					{
						GamepadNavigationScope gamepadNavigationScope = this._navigationScopes[gamepadNavigationContext].VisibleScopes[k];
						if (gamepadNavigationScope.IsAvailable())
						{
							Vector2 center = gamepadNavigationScope.ParentWidget.AreaRect.GetCenter();
							if (!gamepadNavigationContext.GetIsBlockedAtPosition(center))
							{
								this._availableScopesThisFrame.Add(gamepadNavigationScope);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00015444 File Offset: 0x00013644
		internal void OnWidgetUsedNavigationMovementsUpdated(Widget widget)
		{
			if (widget.UsedNavigationMovements != GamepadNavigationTypes.None && !this._navigationBlockingWidgets.Contains(widget))
			{
				this._navigationBlockingWidgets.Add(widget);
				return;
			}
			if (widget.UsedNavigationMovements == GamepadNavigationTypes.None && this._navigationBlockingWidgets.Contains(widget))
			{
				this._navigationBlockingWidgets.Remove(widget);
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00015498 File Offset: 0x00013698
		internal void AddForcedScopeCollection(GamepadNavigationForcedScopeCollection forcedCollection)
		{
			if (!this._forcedScopeCollections.Contains(forcedCollection))
			{
				this._forcedScopeCollections.Add(forcedCollection);
				this.AddItemToDictionaryList<Widget, GamepadNavigationForcedScopeCollection>(this._forcedNavigationScopeCollectionParents, forcedCollection.ParentWidget, forcedCollection);
				this.CollectScopesForForcedCollection(forcedCollection);
				forcedCollection.OnAvailabilityChanged = new Action<GamepadNavigationForcedScopeCollection>(this.OnForcedScopeCollectionAvailabilityStateChanged);
				this._isForcedCollectionsDirty = true;
			}
			else
			{
				Debug.FailedAssert("Trying to add a navigation scope collection more than once", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "AddForcedScopeCollection", 620);
			}
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00015514 File Offset: 0x00013714
		internal void RemoveForcedScopeCollection(GamepadNavigationForcedScopeCollection collection)
		{
			if (this._forcedScopeCollections.Contains(collection))
			{
				collection.ClearScopes();
				this._forcedScopeCollections.Remove(collection);
				if (collection.ParentWidget != null && this._forcedNavigationScopeCollectionParents.ContainsKey(collection.ParentWidget))
				{
					this.RemoveItemFromDictionaryList<Widget, GamepadNavigationForcedScopeCollection>(this._forcedNavigationScopeCollectionParents, collection.ParentWidget, collection);
				}
			}
			collection.OnAvailabilityChanged = null;
			collection.ParentWidget = null;
			this._isForcedCollectionsDirty = true;
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001558C File Offset: 0x0001378C
		internal void AddNavigationScope(IGamepadNavigationContext context, GamepadNavigationScope scope, bool initializeScope = false)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
			{
				gamepadNavigationScopeCollection.AddScope(scope);
			}
			else
			{
				this.OnContextAdded(context);
				this._navigationScopes[context].AddScope(scope);
			}
			this.AddItemToDictionaryList<Widget, GamepadNavigationScope>(this._navigationScopeParents, scope.ParentWidget, scope);
			if (initializeScope)
			{
				this.InitializeScope(context, scope);
			}
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000155F0 File Offset: 0x000137F0
		internal void RemoveNavigationScope(IGamepadNavigationContext context, GamepadNavigationScope scope)
		{
			if (scope == null)
			{
				Debug.FailedAssert("Trying to remove null navigation data", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "RemoveNavigationScope", 677);
				return;
			}
			this._availableScopesThisFrame.Remove(scope);
			this._unsortedScopes.Remove(scope);
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
			{
				gamepadNavigationScopeCollection.RemoveScope(scope);
				scope.ClearNavigatableWidgets();
				if (gamepadNavigationScopeCollection.GetTotalNumberOfScopes() == 0)
				{
					this.OnContextRemoved(context);
				}
			}
			else
			{
				foreach (KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection> keyValuePair in this._navigationScopes.Where<KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection>>((KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection> x) => x.Value.AllScopes.Contains(scope)))
				{
					keyValuePair.Value.RemoveScope(scope);
					scope.ClearNavigatableWidgets();
					if (keyValuePair.Value.GetTotalNumberOfScopes() == 0)
					{
						this.OnContextRemoved(context);
					}
				}
			}
			for (int i = 0; i < this._forcedScopeCollections.Count; i++)
			{
				if (this._forcedScopeCollections[i].Scopes.Contains(scope))
				{
					this._forcedScopeCollections[i].RemoveScope(scope);
				}
			}
			if (scope.ParentWidget != null)
			{
				this._navigationScopeParents.Remove(scope.ParentWidget);
			}
			foreach (KeyValuePair<Widget, List<GamepadNavigationScope>> keyValuePair2 in this._navigationScopeParents)
			{
				keyValuePair2.Value.Remove(scope);
			}
			List<GamepadNavigationScope> list;
			if (this._navigationScopesById.TryGetValue(scope.ScopeID, out list) && list.Contains(scope))
			{
				this.RemoveItemFromDictionaryList<string, GamepadNavigationScope>(this._navigationScopesById, scope.ScopeID, scope);
			}
			bool flag = false;
			foreach (KeyValuePair<IGamepadNavigationContext, GamepadNavigationScopeCollection> keyValuePair3 in this._navigationScopes)
			{
				if (keyValuePair3.Value.HasScopeInAnyList(scope))
				{
					keyValuePair3.Value.RemoveScope(scope);
					List<GamepadNavigationScope> list2;
					if (scope.ParentWidget != null && this._navigationScopeParents.TryGetValue(scope.ParentWidget, out list2))
					{
						list2.Remove(scope);
					}
					scope.ClearNavigatableWidgets();
					flag = true;
				}
			}
			if (flag)
			{
				Debug.FailedAssert("Failed to remove scope from all containers: " + scope.ScopeID, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "RemoveNavigationScope", 760);
			}
			scope.ParentWidget = null;
			if (this._activeNavigationScope == scope)
			{
				this._activeNavigationScope = null;
			}
			this._latestCachedContext = null;
			for (int j = 0; j < this._availableScopesThisFrame.Count; j++)
			{
				this._availableScopesThisFrame[j].IsAdditionalMovementsDirty = true;
			}
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00015944 File Offset: 0x00013B44
		internal void OnWidgetNavigationStatusChanged(IGamepadNavigationContext context, Widget widget)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
			{
				for (int i = 0; i < gamepadNavigationScopeCollection.AllScopes.Count; i++)
				{
					GamepadNavigationScope gamepadNavigationScope = gamepadNavigationScopeCollection.AllScopes[i];
					if (gamepadNavigationScope.ParentWidget.CheckIsMyChildRecursive(widget) || widget.CheckIsMyChildRecursive(gamepadNavigationScope.ParentWidget))
					{
						gamepadNavigationScope.RefreshNavigatableChildren();
					}
				}
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000159A8 File Offset: 0x00013BA8
		internal void OnWidgetNavigationIndexUpdated(IGamepadNavigationContext context, Widget widget)
		{
			if (widget != null)
			{
				GamepadNavigationScope gamepadNavigationScope = this.FindClosestParentScopeOfWidget(widget);
				if (gamepadNavigationScope != null && !gamepadNavigationScope.DoNotAutomaticallyFindChildren)
				{
					gamepadNavigationScope.RemoveWidget(widget);
					if (widget.GamepadNavigationIndex != -1)
					{
						gamepadNavigationScope.AddWidget(widget);
					}
				}
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000159E4 File Offset: 0x00013BE4
		internal bool HasNavigationScope(IGamepadNavigationContext context, GamepadNavigationScope scope)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			return this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection) && (gamepadNavigationScopeCollection.VisibleScopes.Contains(scope) || gamepadNavigationScopeCollection.UninitializedScopes.Contains(scope) || gamepadNavigationScopeCollection.InvisibleScopes.Contains(scope));
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00015A30 File Offset: 0x00013C30
		internal bool HasNavigationScope(IGamepadNavigationContext context, Func<GamepadNavigationScope, bool> predicate)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			return this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection) && (gamepadNavigationScopeCollection.VisibleScopes.Any<GamepadNavigationScope>((GamepadNavigationScope x) => predicate(x)) || gamepadNavigationScopeCollection.InvisibleScopes.Any<GamepadNavigationScope>((GamepadNavigationScope x) => predicate(x)));
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00015A8E File Offset: 0x00013C8E
		private void OnActiveScopeParentChanged(GamepadNavigationScope oldParent, GamepadNavigationScope newParent)
		{
			if (oldParent != null && newParent == null && oldParent.LatestNavigationElementIndex != -1 && oldParent.IsAvailable())
			{
				this._isAvailableScopesDirty = true;
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00015AAE File Offset: 0x00013CAE
		private void OnScopeVisibilityChanged(GamepadNavigationScope scope, bool isVisible)
		{
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00015AB7 File Offset: 0x00013CB7
		private void OnForcedScopeCollectionAvailabilityStateChanged(GamepadNavigationForcedScopeCollection scopeCollection)
		{
			this._isAvailableScopesDirty = true;
			this._isForcedCollectionsDirty = true;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00015AC7 File Offset: 0x00013CC7
		private void OnScopeNavigatableWidgetsChanged(GamepadNavigationScope scope)
		{
			if (!this._unsortedScopes.Contains(scope))
			{
				this._unsortedScopes.Add(scope);
			}
			if (scope.IsInitialized)
			{
				this._isAvailableScopesDirty = true;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00015AF2 File Offset: 0x00013CF2
		public void SetAllDirty()
		{
			this._autoRefreshTimer = 0f;
			this._isAvailableScopesDirty = true;
			this._isForcedCollectionsDirty = true;
			this._latestCachedContext = null;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00015B14 File Offset: 0x00013D14
		private void CollectScopesForForcedCollection(GamepadNavigationForcedScopeCollection collection)
		{
			collection.ClearScopes();
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(collection.ParentWidget.GamepadNavigationContext, out gamepadNavigationScopeCollection))
			{
				for (int i = 0; i < gamepadNavigationScopeCollection.AllScopes.Count; i++)
				{
					GamepadNavigationScope gamepadNavigationScope = gamepadNavigationScopeCollection.AllScopes[i];
					if (collection.ParentWidget == gamepadNavigationScope.ParentWidget || collection.ParentWidget.CheckIsMyChildRecursive(gamepadNavigationScope.ParentWidget))
					{
						collection.AddScope(gamepadNavigationScope);
					}
				}
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00015B8C File Offset: 0x00013D8C
		private void InitializeScope(IGamepadNavigationContext context, GamepadNavigationScope scope)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
			{
				gamepadNavigationScopeCollection.OnNavigationScopeInitialized(scope);
			}
			scope.Initialize();
			for (int i = this._forcedScopeCollections.Count - 1; i >= 0; i--)
			{
				if (this._forcedScopeCollections[i].ParentWidget == scope.ParentWidget || this._forcedScopeCollections[i].ParentWidget.CheckIsMyChildRecursive(scope.ParentWidget))
				{
					this._forcedScopeCollections[i].AddScope(scope);
					break;
				}
			}
			for (int j = 0; j < this._availableScopesThisFrame.Count; j++)
			{
				this._availableScopesThisFrame[j].IsAdditionalMovementsDirty = true;
			}
			if (!string.IsNullOrEmpty(scope.ScopeID))
			{
				this.AddItemToDictionaryList<string, GamepadNavigationScope>(this._navigationScopesById, scope.ScopeID, scope);
			}
			if (scope.ParentScope == null)
			{
				List<Widget> allParents = scope.ParentWidget.GetAllParents();
				int k = 0;
				while (k < allParents.Count)
				{
					Widget widget = allParents[k];
					List<GamepadNavigationScope> list;
					if (this.NavigationScopeParents.TryGetValue(widget, out list))
					{
						if (list.Count > 0)
						{
							scope.SetParentScope(list[0]);
							break;
						}
						break;
					}
					else
					{
						k++;
					}
				}
			}
			this._isAvailableScopesDirty = true;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00015CC8 File Offset: 0x00013EC8
		private void AddItemToDictionaryList<TKey, TValue>(Dictionary<TKey, List<TValue>> sourceDict, TKey key, TValue item)
		{
			List<TValue> list;
			if (!sourceDict.TryGetValue(key, out list))
			{
				sourceDict.Add(key, new List<TValue> { item });
				return;
			}
			if (!list.Contains(item))
			{
				list.Add(item);
				return;
			}
			Debug.FailedAssert("Trying to add same item to source dictionary twice", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "AddItemToDictionaryList", 951);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00015D20 File Offset: 0x00013F20
		private void RemoveItemFromDictionaryList<TKey, TValue>(Dictionary<TKey, List<TValue>> sourceDict, TKey key, TValue item)
		{
			List<TValue> list;
			if (sourceDict.TryGetValue(key, out list))
			{
				list.Remove(item);
				if (list.Count == 0)
				{
					sourceDict.Remove(key);
					return;
				}
			}
			else
			{
				Debug.FailedAssert("Trying to remove non-existent item from source dictionary", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "RemoveItemFromDictionaryList", 972);
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00015D6C File Offset: 0x00013F6C
		internal void OnWidgetHoverBegin(Widget widget)
		{
			if (!this.IsCursorMovingForNavigation && !this.IsInWrapMovement && widget.GamepadNavigationIndex != -1 && !this._isAvailableScopesDirty && !this._shouldUpdateAvailableScopes)
			{
				GamepadNavigationForcedScopeCollection activeForcedScopeCollection = this._activeForcedScopeCollection;
				if (activeForcedScopeCollection == null || activeForcedScopeCollection.Scopes.Contains(this._activeNavigationScope))
				{
					int num = this._activeNavigationScope.FindIndexOfWidget(widget);
					if (this._activeNavigationScope != null && num != -1)
					{
						this._activeNavigationScope.LatestNavigationElementIndex = num;
						return;
					}
					int i = 0;
					while (i < this._availableScopesThisFrame.Count)
					{
						GamepadNavigationScope gamepadNavigationScope = this._availableScopesThisFrame[i];
						int num2 = gamepadNavigationScope.FindIndexOfWidget(widget);
						if (!gamepadNavigationScope.DoNotAutoGainNavigationOnInit && num2 != -1)
						{
							if (this._activeNavigationScope != gamepadNavigationScope && gamepadNavigationScope.IsAvailable())
							{
								this.SetActiveNavigationScope(gamepadNavigationScope);
								this._activeNavigationScope.LatestNavigationElementIndex = num2;
								return;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00015E55 File Offset: 0x00014055
		internal void OnWidgetHoverEnd(Widget widget)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00015E58 File Offset: 0x00014058
		internal void OnWidgetDisconnectedFromRoot(Widget widget)
		{
			GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
			if (this._navigationScopes.TryGetValue(widget.GamepadNavigationContext, out gamepadNavigationScopeCollection))
			{
				gamepadNavigationScopeCollection.OnWidgetDisconnectedFromRoot(widget);
			}
			List<GamepadNavigationScope> list;
			if (this._navigationScopeParents.TryGetValue(widget, out list))
			{
				List<GamepadNavigationScope> list2 = list.ToList<GamepadNavigationScope>();
				for (int i = 0; i < list2.Count; i++)
				{
					list2[i].ClearNavigatableWidgets();
					this.RemoveNavigationScope(widget.GamepadNavigationContext, list2[i]);
				}
			}
			List<GamepadNavigationForcedScopeCollection> list3;
			if (this._forcedNavigationScopeCollectionParents.TryGetValue(widget, out list3))
			{
				for (int j = 0; j < list3.Count; j++)
				{
					this.RemoveForcedScopeCollection(list3[j]);
				}
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00015F04 File Offset: 0x00014104
		internal void SetContextNavigationGainAfterTime(IGamepadNavigationContext context, float seconds, Func<bool> predicate)
		{
			GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler contextGamepadNavigationGainHandler;
			if (this._navigationGainControllers.TryGetValue(context, out contextGamepadNavigationGainHandler))
			{
				contextGamepadNavigationGainHandler.GainNavigationAfterTime(seconds, predicate);
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00015F2C File Offset: 0x0001412C
		internal void SetContextNavigationGainAfterFrames(IGamepadNavigationContext context, int frames, Func<bool> predicate)
		{
			GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler contextGamepadNavigationGainHandler;
			if (this._navigationGainControllers.TryGetValue(context, out contextGamepadNavigationGainHandler))
			{
				contextGamepadNavigationGainHandler.GainNavigationAfterFrames(frames, predicate);
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00015F54 File Offset: 0x00014154
		internal void OnContextGainedNavigation(IGamepadNavigationContext context)
		{
			if (this.IsControllerActive && context != null)
			{
				GamepadNavigationScope activeNavigationScope = this._activeNavigationScope;
				IGamepadNavigationContext gamepadNavigationContext;
				if (activeNavigationScope == null)
				{
					gamepadNavigationContext = null;
				}
				else
				{
					Widget parentWidget = activeNavigationScope.ParentWidget;
					gamepadNavigationContext = ((parentWidget != null) ? parentWidget.GamepadNavigationContext : null);
				}
				GamepadNavigationScopeCollection gamepadNavigationScopeCollection;
				if (gamepadNavigationContext != context && context.IsAvailableForNavigation() && this._navigationScopes.TryGetValue(context, out gamepadNavigationScopeCollection))
				{
					this.RefreshAvailableScopes();
					GamepadNavigationScope gamepadNavigationScope = gamepadNavigationScopeCollection.VisibleScopes.FirstOrDefault<GamepadNavigationScope>((GamepadNavigationScope x) => x.IsDefaultNavigationScope && x.IsAvailable());
					if (gamepadNavigationScope != null && this._availableScopesThisFrame.Contains(gamepadNavigationScope))
					{
						if (this._availableScopesThisFrame.Contains(gamepadNavigationScope))
						{
							this.MoveCursorToFirstAvailableWidgetInScope(gamepadNavigationScope);
						}
						return;
					}
					for (int i = 0; i < this._availableScopesThisFrame.Count; i++)
					{
						if (gamepadNavigationScopeCollection.HasScopeInAnyList(this._availableScopesThisFrame[i]))
						{
							this.MoveCursorToFirstAvailableWidgetInScope(this._availableScopesThisFrame[i]);
							return;
						}
					}
					for (int j = 0; j < gamepadNavigationScopeCollection.VisibleScopes.Count; j++)
					{
						if (gamepadNavigationScopeCollection.VisibleScopes[j].IsAvailable() && this._availableScopesThisFrame.Contains(gamepadNavigationScopeCollection.VisibleScopes[j]))
						{
							this._nextScopeToGainNavigation = gamepadNavigationScopeCollection.VisibleScopes[j];
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000160A0 File Offset: 0x000142A0
		private void SetActiveNavigationScope(GamepadNavigationScope scope)
		{
			if (scope != null && scope != this._activeNavigationScope)
			{
				if (this._activeForcedScopeCollection != null && this._activeForcedScopeCollection.Scopes.Contains(scope))
				{
					this._activeForcedScopeCollection.ActiveScope = scope;
				}
				if (this._activeNavigationScope != null)
				{
					GamepadNavigationScope activeNavigationScope = this._activeNavigationScope;
					activeNavigationScope.OnParentScopeChanged = (Action<GamepadNavigationScope, GamepadNavigationScope>)Delegate.Remove(activeNavigationScope.OnParentScopeChanged, new Action<GamepadNavigationScope, GamepadNavigationScope>(this.OnActiveScopeParentChanged));
				}
				GamepadNavigationScope activeNavigationScope2 = this._activeNavigationScope;
				this._activeNavigationScope = scope;
				this._activeNavigationScope.PreviousScope = activeNavigationScope2;
				if (activeNavigationScope2 != null)
				{
					activeNavigationScope2.SetIsActiveScope(false);
				}
				this._activeNavigationScope.SetIsActiveScope(true);
				if (this._activeNavigationScope != null)
				{
					GamepadNavigationScope activeNavigationScope3 = this._activeNavigationScope;
					activeNavigationScope3.OnParentScopeChanged = (Action<GamepadNavigationScope, GamepadNavigationScope>)Delegate.Combine(activeNavigationScope3.OnParentScopeChanged, new Action<GamepadNavigationScope, GamepadNavigationScope>(this.OnActiveScopeParentChanged));
				}
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00016174 File Offset: 0x00014374
		private void OnGamepadNavigation(GamepadNavigationTypes movement)
		{
			if (this._isAvailableScopesDirty || this._isForcedCollectionsDirty || this.LatestContext == null)
			{
				return;
			}
			if (this.AnyWidgetUsingNavigation)
			{
				return;
			}
			GamepadNavigationScope activeNavigationScope = this._activeNavigationScope;
			if (((activeNavigationScope != null) ? activeNavigationScope.ParentWidget : null) != null)
			{
				GamepadNavigationScope activeNavigationScope2 = this._activeNavigationScope;
				if (activeNavigationScope2 != null && activeNavigationScope2.IsAvailable())
				{
					if (this.HandleGamepadNavigation(movement) && this._latestGamepadNavigationWidget != null)
					{
						SimpleRectangle simpleRectangle = new SimpleRectangle(this._latestGamepadNavigationWidget.GlobalPosition.X, this._latestGamepadNavigationWidget.GlobalPosition.Y, this._latestGamepadNavigationWidget.Size.X, this._latestGamepadNavigationWidget.Size.Y);
						GamepadNavigationTypes movementsToReachRectangle = GamepadNavigationHelper.GetMovementsToReachRectangle(this.MousePosition, simpleRectangle);
						if (((movementsToReachRectangle & GamepadNavigationTypes.Left) != GamepadNavigationTypes.None && movement == GamepadNavigationTypes.Right) || ((movementsToReachRectangle & GamepadNavigationTypes.Right) != GamepadNavigationTypes.None && movement == GamepadNavigationTypes.Left) || ((movementsToReachRectangle & GamepadNavigationTypes.Up) != GamepadNavigationTypes.None && movement == GamepadNavigationTypes.Down) || ((movementsToReachRectangle & GamepadNavigationTypes.Down) != GamepadNavigationTypes.None && movement == GamepadNavigationTypes.Up))
						{
							this.IsInWrapMovement = true;
							return;
						}
					}
					else if (!this.IsCursorMovingForNavigation && !this.IsInWrapMovement && (this._activeNavigationScope == null || !this._activeNavigationScope.GetRectangle().IsPointInside(this.MousePosition)))
					{
						this.MoveCursorToBestAvailableScope(true, movement);
					}
					return;
				}
			}
			this.MoveCursorToBestAvailableScope(false, movement);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000162AC File Offset: 0x000144AC
		private bool HandleGamepadNavigation(GamepadNavigationTypes movement)
		{
			GamepadNavigationScope activeNavigationScope = this._activeNavigationScope;
			if (((activeNavigationScope != null) ? activeNavigationScope.ParentWidget : null) == null)
			{
				Debug.FailedAssert("Active navigation scope or it's parent widget shouldn't be null", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\GamepadNavigation\\GauntletGamepadNavigationManager.cs", "HandleGamepadNavigation", 1188);
				this.MoveCursorToBestAvailableScope(true, GamepadNavigationTypes.None);
				return false;
			}
			if (!this.IsInWrapMovement)
			{
				if ((this._activeNavigationScope.ScopeMovements & movement) == GamepadNavigationTypes.None && (this._activeNavigationScope.AlternateScopeMovements & movement) == GamepadNavigationTypes.None)
				{
					bool flag = this.NavigateBetweenScopes(movement, this._activeNavigationScope);
					if (!flag && !this.IsHoldingDpadKeysForNavigation)
					{
						Widget lastNavigatedWidget = this._activeNavigationScope.LastNavigatedWidget;
						if (lastNavigatedWidget == null || !lastNavigatedWidget.IsPointInsideGamepadCursorArea(this.MousePosition))
						{
							this.SetCurrentNavigatedWidget(this._activeNavigationScope, this._activeNavigationScope.LastNavigatedWidget);
							flag = true;
						}
					}
					return flag;
				}
				if (this._activeNavigationScope.IsAvailable())
				{
					bool flag2 = this.NavigateWithinScope(this._activeNavigationScope, movement);
					if (!flag2 && !this.IsHoldingDpadKeysForNavigation)
					{
						GamepadNavigationScope activeNavigationScope2 = this._activeNavigationScope;
						bool flag3;
						if (activeNavigationScope2 == null)
						{
							flag3 = true;
						}
						else
						{
							Widget lastNavigatedWidget2 = activeNavigationScope2.LastNavigatedWidget;
							bool? flag4 = ((lastNavigatedWidget2 != null) ? new bool?(lastNavigatedWidget2.IsPointInsideMeasuredArea(this.MousePosition)) : null);
							bool flag5 = true;
							flag3 = !((flag4.GetValueOrDefault() == flag5) & (flag4 != null));
						}
						if (flag3)
						{
							flag2 = this.MoveCursorToBestAvailableScope(true, movement);
						}
					}
					return flag2;
				}
			}
			return false;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000163F0 File Offset: 0x000145F0
		private bool NavigateBetweenScopes(GamepadNavigationTypes movement, GamepadNavigationScope currentScope)
		{
			this.RefreshExitMovementForScope(currentScope, movement);
			GamepadNavigationScope gamepadNavigationScope = currentScope.InterScopeMovements[movement];
			if (gamepadNavigationScope != null)
			{
				Widget bestWidgetToScope = this.GetBestWidgetToScope(currentScope, gamepadNavigationScope, movement);
				if (bestWidgetToScope != null)
				{
					if (gamepadNavigationScope.ChildScopes.Count > 0)
					{
						float num;
						GamepadNavigationScope closestChildScopeAtDirection = GamepadNavigationHelper.GetClosestChildScopeAtDirection(gamepadNavigationScope, this.MousePosition, movement, false, out num);
						float distanceToClosestWidgetEdge = GamepadNavigationHelper.GetDistanceToClosestWidgetEdge(gamepadNavigationScope.ParentWidget, this.MousePosition, movement);
						if (closestChildScopeAtDirection != null && closestChildScopeAtDirection != currentScope && num < distanceToClosestWidgetEdge)
						{
							Widget bestWidgetToScope2 = this.GetBestWidgetToScope(currentScope, closestChildScopeAtDirection, movement);
							if (bestWidgetToScope2 != null)
							{
								this.SetCurrentNavigatedWidget(closestChildScopeAtDirection, bestWidgetToScope2);
								return true;
							}
						}
					}
					else if (currentScope.ParentScope != null && (currentScope.ParentScope.ScopeMovements & movement) != GamepadNavigationTypes.None)
					{
						Widget bestWidgetToScope3 = this.GetBestWidgetToScope(currentScope, currentScope.ParentScope, movement);
						if (bestWidgetToScope3 != null)
						{
							float distanceToClosestWidgetEdge2 = GamepadNavigationHelper.GetDistanceToClosestWidgetEdge(bestWidgetToScope3, this.MousePosition, movement);
							float distanceToClosestWidgetEdge3 = GamepadNavigationHelper.GetDistanceToClosestWidgetEdge(gamepadNavigationScope.ParentWidget, this.MousePosition, movement);
							if (distanceToClosestWidgetEdge2 < distanceToClosestWidgetEdge3)
							{
								this.SetCurrentNavigatedWidget(currentScope.ParentScope, bestWidgetToScope3);
								return true;
							}
						}
					}
					this.SetCurrentNavigatedWidget(gamepadNavigationScope, bestWidgetToScope);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000164F8 File Offset: 0x000146F8
		private bool NavigateWithinScope(GamepadNavigationScope scope, GamepadNavigationTypes movement)
		{
			if (scope.NavigatableWidgets.Count == 0)
			{
				return false;
			}
			if ((scope.ScopeMovements & movement) == GamepadNavigationTypes.None && (scope.AlternateScopeMovements & movement) == GamepadNavigationTypes.None)
			{
				return false;
			}
			int num = ((movement == GamepadNavigationTypes.Right || movement == GamepadNavigationTypes.Down) ? 1 : (-1));
			if (scope.LatestNavigationElementIndex < 0 || scope.LatestNavigationElementIndex >= scope.NavigatableWidgets.Count)
			{
				scope.LatestNavigationElementIndex = scope.NavigatableWidgets.Count - 1;
			}
			int latestNavigationElementIndex = scope.LatestNavigationElementIndex;
			int num2 = latestNavigationElementIndex;
			if ((movement & scope.AlternateScopeMovements) != GamepadNavigationTypes.None)
			{
				num *= scope.AlternateMovementStepSize;
			}
			ReadOnlyCollection<Widget> navigatableWidgets = scope.NavigatableWidgets;
			bool flag = false;
			for (;;)
			{
				if (!scope.HasCircularMovement)
				{
					bool flag2 = false;
					if (scope.AlternateMovementStepSize > 0)
					{
						if ((movement & scope.ScopeMovements) != GamepadNavigationTypes.None && Math.Abs(num) == 1)
						{
							if (num2 % scope.AlternateMovementStepSize == 0 && num < 0)
							{
								flag2 = true;
							}
							else if (num2 % scope.AlternateMovementStepSize == scope.AlternateMovementStepSize - 1 && num > 0)
							{
								flag2 = true;
							}
							else if (num2 + num < 0 || num2 + num > scope.NavigatableWidgets.Count - 1)
							{
								flag2 = true;
							}
						}
						if (!flag2 && (movement & scope.AlternateScopeMovements) != GamepadNavigationTypes.None && Math.Abs(num) > 1)
						{
							int num3 = scope.NavigatableWidgets.Count % scope.AlternateMovementStepSize;
							if (scope.NavigatableWidgets.Count > 0 && num3 == 0)
							{
								num3 = scope.AlternateMovementStepSize;
							}
							int num4;
							if (num3 > 0)
							{
								num4 = scope.NavigatableWidgets.Count - num3;
								if (scope.NavigatableWidgets.Count != num3)
								{
									if (num2 < num4 && num2 + num >= scope.NavigatableWidgets.Count)
									{
										break;
									}
									if (num2 >= num4 && num2 + num >= scope.NavigatableWidgets.Count)
									{
										flag2 = true;
									}
								}
								else
								{
									flag2 = true;
								}
							}
							else
							{
								num4 = Math.Max(0, scope.NavigatableWidgets.Count - scope.AlternateMovementStepSize - 1);
							}
							if (num2 > num4 && num2 < scope.NavigatableWidgets.Count && num > 1)
							{
								flag2 = true;
							}
							if (num2 >= 0 && num2 < scope.AlternateMovementStepSize && num < 1)
							{
								flag2 = true;
							}
						}
					}
					else if (num2 + num < 0 || num2 + num > scope.NavigatableWidgets.Count - 1)
					{
						flag2 = true;
					}
					if (flag2)
					{
						goto Block_35;
					}
				}
				num2 += num;
				if (num2 > scope.NavigatableWidgets.Count - 1 && !scope.HasCircularMovement)
				{
					return false;
				}
				num2 %= scope.NavigatableWidgets.Count;
				if (num2 < 0)
				{
					num2 = navigatableWidgets.Count - 1;
				}
				if (scope.IsWidgetVisible(navigatableWidgets[num2]))
				{
					goto Block_39;
				}
				if (num2 < 0 || num2 >= navigatableWidgets.Count || num2 == latestNavigationElementIndex)
				{
					goto IL_028D;
				}
			}
			this.SetCurrentNavigatedWidget(scope, scope.GetLastAvailableWidget());
			return true;
			Block_35:
			return this.NavigateBetweenScopes(movement, this._activeNavigationScope);
			Block_39:
			flag = true;
			IL_028D:
			if (num2 >= 0 && flag)
			{
				if (scope.ChildScopes.Count > 0)
				{
					float num5;
					GamepadNavigationScope closestChildScopeAtDirection = GamepadNavigationHelper.GetClosestChildScopeAtDirection(scope, this.MousePosition, movement, false, out num5);
					if (num5 != -1f && closestChildScopeAtDirection != null)
					{
						Vector2 vector;
						GamepadNavigationHelper.GetDistanceToClosestWidgetEdge(navigatableWidgets[num2], this.MousePosition, movement, out vector);
						if (GamepadNavigationHelper.GetDirectionalDistanceBetweenTwoPoints(movement, this.MousePosition, vector) > num5)
						{
							this.SetCurrentNavigatedWidget(closestChildScopeAtDirection, closestChildScopeAtDirection.GetApproximatelyClosestWidgetToPosition(this.MousePosition, movement, false));
							return true;
						}
					}
				}
				this.SetCurrentNavigatedWidget(scope, scope.NavigatableWidgets[num2]);
				return true;
			}
			return false;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00016828 File Offset: 0x00014A28
		private bool SetCurrentNavigatedWidget(GamepadNavigationScope scope, Widget widget)
		{
			if (scope != null && Input.MouseMoveX == 0f && Input.MouseMoveY == 0f)
			{
				int num = scope.FindIndexOfWidget(widget);
				if (num != -1)
				{
					if (this._activeNavigationScope != scope)
					{
						this.SetActiveNavigationScope(scope);
					}
					this._latestGamepadNavigationWidget = widget;
					this._activeNavigationScope.LatestNavigationElementIndex = num;
					if (this.IsControllerActive)
					{
						this._cursorMoveStartTime = this._time;
						this._cursorMoveStartPosition = this.MousePosition;
						this._stopCursorNextFrame = false;
						this.IsCursorMovingForNavigation = true;
						this._latestGamepadNavigationWidget.OnGamepadNavigationFocusGain();
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000168C0 File Offset: 0x00014AC0
		private bool MoveCursorToBestAvailableScope(bool isFromInput, GamepadNavigationTypes preferredMovement = GamepadNavigationTypes.None)
		{
			GamepadNavigationScope gamepadNavigationScope = null;
			if (preferredMovement != GamepadNavigationTypes.None)
			{
				float num;
				gamepadNavigationScope = GamepadNavigationHelper.GetClosestScopeAtDirectionFromList(this._availableScopesThisFrame, this.MousePosition, preferredMovement, isFromInput, false, out num, Array.Empty<GamepadNavigationScope>());
			}
			if (gamepadNavigationScope == null)
			{
				gamepadNavigationScope = GamepadNavigationHelper.GetClosestScopeFromList(this._availableScopesThisFrame, this.MousePosition, true);
			}
			if (gamepadNavigationScope != null)
			{
				bool flag = this._activeForcedScopeCollection != null && this._activeForcedScopeCollection.Scopes.Contains(this._activeNavigationScope) && gamepadNavigationScope.LastNavigatedWidget != null;
				Widget widget;
				if ((this._activeNavigationScope != null && !this._activeNavigationScope.IsAvailable() && this._activeNavigationScope.ParentScope == gamepadNavigationScope) || flag)
				{
					widget = gamepadNavigationScope.LastNavigatedWidget;
				}
				else if (gamepadNavigationScope.ForceGainNavigationOnFirstChild || (!isFromInput && !gamepadNavigationScope.ForceGainNavigationOnClosestChild))
				{
					widget = gamepadNavigationScope.GetFirstAvailableWidget();
				}
				else if (preferredMovement != GamepadNavigationTypes.None)
				{
					widget = gamepadNavigationScope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, preferredMovement, false);
				}
				else
				{
					widget = gamepadNavigationScope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, GamepadNavigationTypes.None, false);
				}
				if (widget != null)
				{
					this.SetCurrentNavigatedWidget(gamepadNavigationScope, widget);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000169B8 File Offset: 0x00014BB8
		private void MoveCursorToFirstAvailableWidgetInScope(GamepadNavigationScope scope)
		{
			this.SetCurrentNavigatedWidget(scope, scope.GetFirstAvailableWidget());
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000169C8 File Offset: 0x00014BC8
		private void MoveCursorToClosestAvailableWidgetInScope(GamepadNavigationScope scope)
		{
			this.SetCurrentNavigatedWidget(scope, scope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, GamepadNavigationTypes.None, false));
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000169E0 File Offset: 0x00014BE0
		private void TryMoveCursorToPreviousScope(GamepadNavigationForcedScopeCollection fromCollection)
		{
			GamepadNavigationScope gamepadNavigationScope = ((fromCollection != null) ? fromCollection.PreviousScope : null);
			if (gamepadNavigationScope != null && this._availableScopesThisFrame.Contains(gamepadNavigationScope))
			{
				if (gamepadNavigationScope.LastNavigatedWidget == null)
				{
					this.SetCurrentNavigatedWidget(gamepadNavigationScope, gamepadNavigationScope.GetFirstAvailableWidget());
					return;
				}
				this.SetCurrentNavigatedWidget(gamepadNavigationScope, gamepadNavigationScope.LastNavigatedWidget);
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00016A34 File Offset: 0x00014C34
		private GamepadNavigationScope GetBestScopeAtDirectionFrom(GamepadNavigationScope fromScope, GamepadNavigationTypes movement)
		{
			if (fromScope.ChildScopes.Count > 0 && fromScope.HasMovement(movement))
			{
				float num;
				GamepadNavigationScope closestChildScopeAtDirection = GamepadNavigationHelper.GetClosestChildScopeAtDirection(fromScope, this.MousePosition, movement, false, out num);
				if (closestChildScopeAtDirection != null && closestChildScopeAtDirection != this._activeNavigationScope && num > 0f)
				{
					return closestChildScopeAtDirection;
				}
			}
			GamepadNavigationScope gamepadNavigationScope = fromScope.ManualScopes[movement];
			if (gamepadNavigationScope == null)
			{
				if (!string.IsNullOrEmpty(fromScope.ManualScopeIDs[movement]))
				{
					gamepadNavigationScope = this.GetManualScopeAtDirection(movement, fromScope);
				}
				else if (fromScope.GetShouldFindScopeByPosition(movement))
				{
					if (fromScope.ParentScope != null && fromScope.ParentScope.HasMovement(movement))
					{
						List<GamepadNavigationScope> list = fromScope.ParentScope.ChildScopes.ToList<GamepadNavigationScope>();
						list.Remove(fromScope);
						if (list.Count > 0)
						{
							float num2;
							gamepadNavigationScope = GamepadNavigationHelper.GetClosestScopeAtDirectionFromList(list, fromScope, this.MousePosition, movement, false, out num2);
						}
						if (gamepadNavigationScope == null && fromScope.ParentScope != null)
						{
							GamepadNavigationForcedScopeCollection activeForcedScopeCollection = this._activeForcedScopeCollection;
							if (activeForcedScopeCollection == null || activeForcedScopeCollection.Scopes.Contains(fromScope.ParentScope))
							{
								if (fromScope.ParentScope.HasMovement(movement))
								{
									gamepadNavigationScope = fromScope.ParentScope;
									if (gamepadNavigationScope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, movement, true) == null)
									{
										return this.GetBestScopeAtDirectionFrom(gamepadNavigationScope, movement);
									}
								}
								else
								{
									bool flag = this._availableScopesThisFrame.Remove(fromScope.ParentScope);
									float num2;
									gamepadNavigationScope = GamepadNavigationHelper.GetClosestScopeAtDirectionFromList(this._availableScopesThisFrame, fromScope, this.MousePosition, movement, false, out num2);
									if (flag)
									{
										this._availableScopesThisFrame.Add(fromScope.ParentScope);
									}
								}
							}
						}
					}
					else
					{
						bool flag2 = fromScope.ChildScopes.Count > 0;
						List<GamepadNavigationScope> list2 = this._availableScopesThisFrame;
						if (flag2)
						{
							list2 = list2.ToList<GamepadNavigationScope>();
							for (int i = 0; i < fromScope.ChildScopes.Count; i++)
							{
								list2.Remove(fromScope.ChildScopes[i]);
							}
						}
						float num3;
						gamepadNavigationScope = GamepadNavigationHelper.GetClosestScopeAtDirectionFromList(list2, fromScope, this.MousePosition, movement, false, out num3);
						if (gamepadNavigationScope != null && gamepadNavigationScope.ChildScopes.Count > 0)
						{
							float num4;
							GamepadNavigationScope closestChildScopeAtDirection2 = GamepadNavigationHelper.GetClosestChildScopeAtDirection(gamepadNavigationScope, this.MousePosition, movement, false, out num4);
							float num5;
							Widget approximatelyClosestWidgetToPosition = gamepadNavigationScope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, out num5, movement, true);
							if (closestChildScopeAtDirection2 != null && closestChildScopeAtDirection2 != this._activeNavigationScope && (num4 < num3 || (approximatelyClosestWidgetToPosition != null && num4 < num5)))
							{
								gamepadNavigationScope = closestChildScopeAtDirection2;
							}
						}
					}
				}
			}
			return gamepadNavigationScope;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00016C70 File Offset: 0x00014E70
		private void RefreshExitMovementForScope(GamepadNavigationScope scope, GamepadNavigationTypes movement)
		{
			scope.InterScopeMovements[movement] = this.GetBestScopeAtDirectionFrom(scope, movement);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00016C86 File Offset: 0x00014E86
		private GamepadNavigationTypes GetMovementForInput(InputKey input)
		{
			if (input == InputKey.ControllerLUp)
			{
				return GamepadNavigationTypes.Up;
			}
			if (input == InputKey.ControllerLRight)
			{
				return GamepadNavigationTypes.Right;
			}
			if (input == InputKey.ControllerLDown)
			{
				return GamepadNavigationTypes.Down;
			}
			if (input == InputKey.ControllerLLeft)
			{
				return GamepadNavigationTypes.Left;
			}
			return GamepadNavigationTypes.None;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00016CB4 File Offset: 0x00014EB4
		private GamepadNavigationScope GetManualScopeAtDirection(GamepadNavigationTypes movement, GamepadNavigationScope fromScope)
		{
			GamepadNavigationScope gamepadNavigationScope = fromScope.ManualScopes[movement];
			string text = fromScope.ManualScopeIDs[movement];
			if (gamepadNavigationScope == null)
			{
				if (string.IsNullOrEmpty(text) || text == "None")
				{
					return null;
				}
				List<GamepadNavigationScope> list;
				if (this._navigationScopesById.TryGetValue(text, out list))
				{
					if (list.Count == 1)
					{
						gamepadNavigationScope = list[0];
					}
					else
					{
						float num;
						gamepadNavigationScope = GamepadNavigationHelper.GetClosestScopeAtDirectionFromList(list, this.MousePosition, movement, false, false, out num, Array.Empty<GamepadNavigationScope>());
					}
					if (gamepadNavigationScope != null && !gamepadNavigationScope.IsAvailable())
					{
						gamepadNavigationScope = this.GetManualScopeAtDirection(movement, gamepadNavigationScope);
					}
				}
			}
			return gamepadNavigationScope;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00016D44 File Offset: 0x00014F44
		private Widget GetBestWidgetToScope(GamepadNavigationScope fromScope, GamepadNavigationScope toScope, GamepadNavigationTypes movement)
		{
			Widget widget;
			if (toScope.ForceGainNavigationOnFirstChild)
			{
				widget = toScope.GetFirstAvailableWidget();
			}
			else if (toScope.ForceGainNavigationBasedOnDirection && (fromScope == null || toScope != fromScope.ParentScope) && ((toScope.ScopeMovements & movement) != GamepadNavigationTypes.None || (toScope.AlternateScopeMovements & movement) != GamepadNavigationTypes.None))
			{
				if ((movement & GamepadNavigationTypes.Up) != GamepadNavigationTypes.None || (movement & GamepadNavigationTypes.Left) != GamepadNavigationTypes.None)
				{
					widget = toScope.GetLastAvailableWidget();
				}
				else
				{
					widget = toScope.GetFirstAvailableWidget();
				}
			}
			else if (fromScope.ParentScope == toScope)
			{
				widget = toScope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, movement, true);
			}
			else
			{
				widget = toScope.GetApproximatelyClosestWidgetToPosition(this.MousePosition, movement, false);
			}
			return widget;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00016DD0 File Offset: 0x00014FD0
		private GamepadNavigationScope FindClosestParentScopeOfWidget(Widget widget)
		{
			Widget widget2 = widget;
			while (widget2 != null && !widget2.DoNotAcceptNavigation)
			{
				List<GamepadNavigationScope> list;
				if (this._navigationScopeParents.TryGetValue(widget2, out list))
				{
					if (list.Count > 0)
					{
						return list[0];
					}
					return null;
				}
				else
				{
					widget2 = widget2.ParentWidget;
				}
			}
			return null;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00016E18 File Offset: 0x00015018
		private GamepadNavigationForcedScopeCollection FindAvailableForcedScope()
		{
			if (this._forcedScopeCollections.Count > 0)
			{
				if (this._isForcedCollectionsDirty)
				{
					this._forcedScopeCollections.Sort(this._cachedForcedScopeComparer);
					this._forcedScopeCollections.ForEach(delegate(GamepadNavigationForcedScopeCollection x)
					{
						this.CollectScopesForForcedCollection(x);
					});
					this._isForcedCollectionsDirty = false;
					this._isAvailableScopesDirty = true;
				}
				for (int i = this._forcedScopeCollections.Count - 1; i >= 0; i--)
				{
					if (this.IsControllerActive && this._forcedScopeCollections[i].IsAvailable())
					{
						return this._forcedScopeCollections[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00016EB4 File Offset: 0x000150B4
		private void HandleInput(float dt)
		{
			if (this.IsControllerActive)
			{
				GamepadNavigationTypes gamepadNavigationTypes = GamepadNavigationTypes.None;
				if (Input.IsKeyPressed(InputKey.ControllerLLeft))
				{
					gamepadNavigationTypes = GamepadNavigationTypes.Left;
				}
				else if (Input.IsKeyPressed(InputKey.ControllerLRight))
				{
					gamepadNavigationTypes = GamepadNavigationTypes.Right;
				}
				else if (Input.IsKeyPressed(InputKey.ControllerLDown))
				{
					gamepadNavigationTypes = GamepadNavigationTypes.Down;
				}
				else if (Input.IsKeyPressed(InputKey.ControllerLUp))
				{
					gamepadNavigationTypes = GamepadNavigationTypes.Up;
				}
				if (gamepadNavigationTypes != GamepadNavigationTypes.None)
				{
					this.OnGamepadNavigation(gamepadNavigationTypes);
				}
				this._navigationHoldTimer += dt;
				if (!this.IsHoldingDpadKeysForNavigation && this._navigationHoldTimer > 0.15f)
				{
					this.IsHoldingDpadKeysForNavigation = true;
					this._navigationHoldTimer = 0f;
				}
				else if (this.IsHoldingDpadKeysForNavigation && this._navigationHoldTimer > 0.08f)
				{
					InputKey inputKey = (InputKey)0;
					if (Input.IsKeyDown(InputKey.ControllerLUp))
					{
						inputKey = InputKey.ControllerLUp;
					}
					else if (Input.IsKeyDown(InputKey.ControllerLRight))
					{
						inputKey = InputKey.ControllerLRight;
					}
					else if (Input.IsKeyDown(InputKey.ControllerLDown))
					{
						inputKey = InputKey.ControllerLDown;
					}
					else if (Input.IsKeyDown(InputKey.ControllerLLeft))
					{
						inputKey = InputKey.ControllerLLeft;
					}
					if (inputKey != (InputKey)0)
					{
						GamepadNavigationTypes movementForInput = this.GetMovementForInput(inputKey);
						this.OnGamepadNavigation(movementForInput);
					}
					this._navigationHoldTimer = 0f;
				}
			}
			if (!Input.IsKeyDown(InputKey.ControllerLUp) && !Input.IsKeyDown(InputKey.ControllerLRight) && !Input.IsKeyDown(InputKey.ControllerLDown) && !Input.IsKeyDown(InputKey.ControllerLLeft))
			{
				this.IsHoldingDpadKeysForNavigation = false;
				this._navigationHoldTimer = 0f;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00017014 File Offset: 0x00015214
		private void HandleCursorMovement()
		{
			Vector2 targetCursorPosition = this.GetTargetCursorPosition();
			if (this._latestGamepadNavigationWidget != null && targetCursorPosition != Vec2.Invalid)
			{
				if (this.IsCursorMovingForNavigation)
				{
					if (this._time - this._cursorMoveStartTime <= this._mouseCursorMoveTime)
					{
						this.MousePosition = (this.IsFollowingMobileTarget ? targetCursorPosition : Vector2.Lerp(this._cursorMoveStartPosition, targetCursorPosition, (this._time - this._cursorMoveStartTime) / this._mouseCursorMoveTime));
						this.IsCursorMovingForNavigation = true;
					}
					else
					{
						bool flag = this._latestGamepadNavigationWidget != null && !this.IsHoldingDpadKeysForNavigation && this.IsControllerActive && (!Input.IsAnyTouchActive || !this.IsTouchpadMouseEnabled) && Vector2.Distance(this.MousePosition, targetCursorPosition) > 1.44f && Input.MouseMoveX == 0f && Input.MouseMoveY == 0f;
						this.MousePosition = targetCursorPosition;
						if (!flag)
						{
							this._latestGamepadNavigationWidget = null;
							this._stopCursorNextFrame = true;
							this.IsInWrapMovement = false;
							this.IsFollowingMobileTarget = false;
						}
					}
				}
				else if (this._latestGamepadNavigationWidget != null)
				{
					this._latestGamepadNavigationWidget = null;
					this._stopCursorNextFrame = true;
					this.IsFollowingMobileTarget = false;
					this.IsInWrapMovement = false;
				}
			}
			if (!this.IsCursorMovingForNavigation && this._activeNavigationScope != null && this._activeNavigationScope.FollowMobileTargets && this._wasCursorInsideActiveScopeLastFrame)
			{
				Widget lastNavigatedWidget = this._activeNavigationScope.LastNavigatedWidget;
				if (lastNavigatedWidget != null)
				{
					Vector2 vector = lastNavigatedWidget.GlobalPosition + lastNavigatedWidget.Size / 2f;
					if (this._lastNavigatedWidgetPosition.X != float.NaN && Vector2.Distance(vector, this._lastNavigatedWidgetPosition) > 1.44f)
					{
						this.SetCurrentNavigatedWidget(this._activeNavigationScope, this._activeNavigationScope.LastNavigatedWidget);
						this._autoRefreshTimer = 0f;
						this.IsFollowingMobileTarget = true;
					}
					this._lastNavigatedWidgetPosition = vector;
				}
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x000171F7 File Offset: 0x000153F7
		private void OnDpadNavigationStopped()
		{
			this._lastNavigatedWidgetPosition = new Vector2(float.NaN, float.NaN);
			this._latestGamepadNavigationWidget = null;
			this._stopCursorNextFrame = true;
			this.IsFollowingMobileTarget = false;
			this.IsInWrapMovement = false;
			this._navigationHoldTimer = 0f;
		}

		// Token: 0x0400027C RID: 636
		private IGamepadNavigationContext _latestCachedContext;

		// Token: 0x04000282 RID: 642
		private float _time;

		// Token: 0x04000283 RID: 643
		private bool _stopCursorNextFrame;

		// Token: 0x04000284 RID: 644
		private bool _isForcedCollectionsDirty;

		// Token: 0x04000285 RID: 645
		private GauntletGamepadNavigationManager.GamepadNavigationContextComparer _cachedNavigationContextComparer;

		// Token: 0x04000286 RID: 646
		private GauntletGamepadNavigationManager.ForcedScopeComparer _cachedForcedScopeComparer;

		// Token: 0x04000287 RID: 647
		private List<IGamepadNavigationContext> _sortedNavigationContexts;

		// Token: 0x04000288 RID: 648
		private Dictionary<IGamepadNavigationContext, GamepadNavigationScopeCollection> _navigationScopes;

		// Token: 0x0400028A RID: 650
		private List<GamepadNavigationScope> _availableScopesThisFrame;

		// Token: 0x0400028B RID: 651
		private List<GamepadNavigationScope> _unsortedScopes;

		// Token: 0x0400028C RID: 652
		private List<GamepadNavigationForcedScopeCollection> _forcedScopeCollections;

		// Token: 0x0400028D RID: 653
		private GamepadNavigationForcedScopeCollection _activeForcedScopeCollection;

		// Token: 0x0400028E RID: 654
		private GamepadNavigationScope _nextScopeToGainNavigation;

		// Token: 0x0400028F RID: 655
		private GamepadNavigationScope _activeNavigationScope;

		// Token: 0x04000290 RID: 656
		private Dictionary<Widget, List<GamepadNavigationScope>> _navigationScopeParents;

		// Token: 0x04000291 RID: 657
		private Dictionary<Widget, List<GamepadNavigationForcedScopeCollection>> _forcedNavigationScopeCollectionParents;

		// Token: 0x04000294 RID: 660
		private Dictionary<string, List<GamepadNavigationScope>> _layerNavigationScopes;

		// Token: 0x04000295 RID: 661
		private Dictionary<string, List<GamepadNavigationScope>> _navigationScopesById;

		// Token: 0x04000296 RID: 662
		private Dictionary<IGamepadNavigationContext, GauntletGamepadNavigationManager.ContextGamepadNavigationGainHandler> _navigationGainControllers;

		// Token: 0x04000297 RID: 663
		private float _navigationHoldTimer;

		// Token: 0x04000298 RID: 664
		private Vector2 _lastNavigatedWidgetPosition;

		// Token: 0x04000299 RID: 665
		private readonly float _mouseCursorMoveTime = 0.09f;

		// Token: 0x0400029A RID: 666
		private Vector2 _cursorMoveStartPosition = new Vector2(float.NaN, float.NaN);

		// Token: 0x0400029B RID: 667
		private float _cursorMoveStartTime = -1f;

		// Token: 0x0400029C RID: 668
		private Widget _latestGamepadNavigationWidget;

		// Token: 0x0400029D RID: 669
		private List<Widget> _navigationBlockingWidgets;

		// Token: 0x0400029E RID: 670
		private bool _isAvailableScopesDirty;

		// Token: 0x0400029F RID: 671
		private bool _shouldUpdateAvailableScopes;

		// Token: 0x040002A0 RID: 672
		private float _autoRefreshTimer;

		// Token: 0x040002A1 RID: 673
		private bool _wasCursorInsideActiveScopeLastFrame;

		// Token: 0x02000089 RID: 137
		private class GamepadNavigationContextComparer : IComparer<IGamepadNavigationContext>
		{
			// Token: 0x06000908 RID: 2312 RVA: 0x00023E1C File Offset: 0x0002201C
			public int Compare(IGamepadNavigationContext x, IGamepadNavigationContext y)
			{
				int lastScreenOrder = x.GetLastScreenOrder();
				int lastScreenOrder2 = y.GetLastScreenOrder();
				return -lastScreenOrder.CompareTo(lastScreenOrder2);
			}
		}

		// Token: 0x0200008A RID: 138
		private class ForcedScopeComparer : IComparer<GamepadNavigationForcedScopeCollection>
		{
			// Token: 0x0600090A RID: 2314 RVA: 0x00023E48 File Offset: 0x00022048
			public int Compare(GamepadNavigationForcedScopeCollection x, GamepadNavigationForcedScopeCollection y)
			{
				return x.CollectionOrder.CompareTo(y.CollectionOrder);
			}
		}

		// Token: 0x0200008B RID: 139
		private class ContextGamepadNavigationGainHandler
		{
			// Token: 0x0600090C RID: 2316 RVA: 0x00023E71 File Offset: 0x00022071
			public ContextGamepadNavigationGainHandler(IGamepadNavigationContext eventManager)
			{
				this._context = eventManager;
				this.Clear();
			}

			// Token: 0x0600090D RID: 2317 RVA: 0x00023E86 File Offset: 0x00022086
			public void GainNavigationAfterFrames(int frameCount, Func<bool> predicate = null)
			{
				this.Clear();
				if (frameCount >= 0)
				{
					this._gainAfterFrames = frameCount;
					this._gainPredicate = predicate;
				}
			}

			// Token: 0x0600090E RID: 2318 RVA: 0x00023EA0 File Offset: 0x000220A0
			public void GainNavigationAfterTime(float seconds, Func<bool> predicate = null)
			{
				this.Clear();
				if (seconds >= 0f)
				{
					this._gainAfterTime = seconds;
					this._gainPredicate = predicate;
				}
			}

			// Token: 0x0600090F RID: 2319 RVA: 0x00023EC0 File Offset: 0x000220C0
			public void Tick(float dt)
			{
				if (this._gainAfterTime != -1f)
				{
					this._gainTimer += dt;
					if (this._gainTimer > this._gainAfterTime)
					{
						Func<bool> gainPredicate = this._gainPredicate;
						if (gainPredicate == null || gainPredicate())
						{
							this._context.OnGainNavigation();
						}
						this.Clear();
						return;
					}
				}
				else if (this._gainAfterFrames != -1)
				{
					this._frameTicker++;
					if (this._frameTicker > this._gainAfterFrames)
					{
						Func<bool> gainPredicate2 = this._gainPredicate;
						if (gainPredicate2 == null || gainPredicate2())
						{
							this._context.OnGainNavigation();
						}
						this.Clear();
					}
				}
			}

			// Token: 0x06000910 RID: 2320 RVA: 0x00023F66 File Offset: 0x00022166
			public void Clear()
			{
				this._gainAfterTime = -1f;
				this._gainAfterFrames = -1;
				this._frameTicker = 0;
				this._gainTimer = 0f;
				this._gainPredicate = null;
			}

			// Token: 0x04000471 RID: 1137
			private readonly IGamepadNavigationContext _context;

			// Token: 0x04000472 RID: 1138
			private float _gainAfterTime;

			// Token: 0x04000473 RID: 1139
			private float _gainTimer;

			// Token: 0x04000474 RID: 1140
			private int _gainAfterFrames;

			// Token: 0x04000475 RID: 1141
			private int _frameTicker;

			// Token: 0x04000476 RID: 1142
			private Func<bool> _gainPredicate;
		}
	}
}
