using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000075 RID: 117
	public abstract class GameState : MBObjectBase
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0001A4CF File Offset: 0x000186CF
		public GameState Predecessor
		{
			get
			{
				return this.GameStateManager.FindPredecessor(this);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001A4DD File Offset: 0x000186DD
		public bool IsActive
		{
			get
			{
				return this.GameStateManager != null && this.GameStateManager.ActiveState == this;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0001A4F7 File Offset: 0x000186F7
		public IReadOnlyCollection<IGameStateListener> Listeners
		{
			get
			{
				return this._listeners.AsReadOnly();
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001A504 File Offset: 0x00018704
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0001A50C File Offset: 0x0001870C
		public GameStateManager GameStateManager { get; internal set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001A515 File Offset: 0x00018715
		public virtual bool CanBeDisabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001A518 File Offset: 0x00018718
		public virtual bool IsMusicMenuState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001A51B File Offset: 0x0001871B
		public virtual bool IsMenuState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001A51E File Offset: 0x0001871E
		protected GameState()
		{
			this._listeners = new List<IGameStateListener>();
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001A531 File Offset: 0x00018731
		public bool RegisterListener(IGameStateListener listener)
		{
			if (listener == null)
			{
				Debug.FailedAssert("Can not register null listener to game state.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\GameState.cs", "RegisterListener", 49);
			}
			if (this._listeners.Contains(listener))
			{
				return false;
			}
			this._listeners.Add(listener);
			return true;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001A569 File Offset: 0x00018769
		public bool UnregisterListener(IGameStateListener listener)
		{
			return this._listeners.Remove(listener);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001A578 File Offset: 0x00018778
		public T GetListenerOfType<T>()
		{
			using (List<IGameStateListener>.Enumerator enumerator = this._listeners.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGameStateListener gameStateListener;
					if ((gameStateListener = enumerator.Current) is T)
					{
						return (T)((object)gameStateListener);
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001A5E4 File Offset: 0x000187E4
		internal void HandleInitialize()
		{
			this.OnInitialize();
			foreach (IGameStateListener gameStateListener in this._listeners)
			{
				gameStateListener.OnInitialize();
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001A63C File Offset: 0x0001883C
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001A640 File Offset: 0x00018840
		internal void HandleFinalize()
		{
			this.OnFinalize();
			foreach (IGameStateListener gameStateListener in this._listeners)
			{
				gameStateListener.OnFinalize();
			}
			this._listeners = null;
			this.GameStateManager = null;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001A6A4 File Offset: 0x000188A4
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001A6A8 File Offset: 0x000188A8
		internal void HandleActivate()
		{
			GameState.NumberOfListenerActivations = 0;
			if (this.IsActive)
			{
				this.OnActivate();
				if (this.IsActive && this._listeners.Count != 0 && GameState.NumberOfListenerActivations == 0)
				{
					foreach (IGameStateListener gameStateListener in this._listeners)
					{
						gameStateListener.OnActivate();
					}
					GameState.NumberOfListenerActivations++;
				}
				if (!string.IsNullOrEmpty(GameStateManager.StateActivateCommand))
				{
					bool flag;
					CommandLineFunctionality.CallFunction(GameStateManager.StateActivateCommand, "", out flag);
				}
				Debug.ReportMemoryBookmark("GameState Activated: " + base.GetType().Name);
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0001A770 File Offset: 0x00018970
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x0001A778 File Offset: 0x00018978
		public bool Activated { get; private set; }

		// Token: 0x06000808 RID: 2056 RVA: 0x0001A781 File Offset: 0x00018981
		protected virtual void OnActivate()
		{
			this.Activated = true;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001A78C File Offset: 0x0001898C
		internal void HandleDeactivate()
		{
			this.OnDeactivate();
			foreach (IGameStateListener gameStateListener in this._listeners)
			{
				gameStateListener.OnDeactivate();
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001A7E4 File Offset: 0x000189E4
		protected virtual void OnDeactivate()
		{
			this.Activated = false;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001A7ED File Offset: 0x000189ED
		protected internal virtual void OnTick(float dt)
		{
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001A7EF File Offset: 0x000189EF
		protected internal virtual void OnIdleTick(float dt)
		{
		}

		// Token: 0x04000416 RID: 1046
		public int Level;

		// Token: 0x04000417 RID: 1047
		private List<IGameStateListener> _listeners;

		// Token: 0x04000418 RID: 1048
		public static int NumberOfListenerActivations;
	}
}
