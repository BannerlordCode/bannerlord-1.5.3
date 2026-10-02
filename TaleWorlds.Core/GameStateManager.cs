using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000076 RID: 118
	public class GameStateManager
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0001A7F1 File Offset: 0x000189F1
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0001A7F8 File Offset: 0x000189F8
		public static GameStateManager Current
		{
			get
			{
				return GameStateManager._current;
			}
			set
			{
				GameStateManager current = GameStateManager._current;
				if (current != null)
				{
					current.CleanStates(0);
				}
				GameStateManager._current = value;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001A811 File Offset: 0x00018A11
		public IReadOnlyCollection<IGameStateManagerListener> Listeners
		{
			get
			{
				return this._listeners.AsReadOnly();
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0001A81E File Offset: 0x00018A1E
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0001A826 File Offset: 0x00018A26
		public GameStateManager.GameStateManagerType CurrentType { get; private set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0001A82F File Offset: 0x00018A2F
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0001A837 File Offset: 0x00018A37
		public IGameStateManagerOwner Owner { get; private set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001A840 File Offset: 0x00018A40
		public IEnumerable<GameState> GameStates
		{
			get
			{
				return this._gameStates.AsReadOnly();
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0001A84D File Offset: 0x00018A4D
		public bool ActiveStateDisabledByUser
		{
			get
			{
				if (this._activeStateDisableRequests.Count > 0)
				{
					GameState activeState = this.ActiveState;
					return activeState != null && activeState.CanBeDisabled;
				}
				return false;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0001A870 File Offset: 0x00018A70
		public GameState ActiveState
		{
			get
			{
				if (this._gameStates.Count <= 0)
				{
					return null;
				}
				return this._gameStates[this._gameStates.Count - 1];
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001A89C File Offset: 0x00018A9C
		public GameStateManager(IGameStateManagerOwner owner, GameStateManager.GameStateManagerType gameStateManagerType)
		{
			this.Owner = owner;
			this.CurrentType = gameStateManagerType;
			this._gameStateJobs = new Queue<GameStateManager.GameStateJob>();
			this._gameStates = new List<GameState>();
			this._listeners = new List<IGameStateManagerListener>();
			this._activeStateDisableRequests = new List<WeakReference>();
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001A8EC File Offset: 0x00018AEC
		internal GameState FindPredecessor(GameState gameState)
		{
			GameState gameState2 = null;
			int num = this._gameStates.IndexOf(gameState);
			if (num > 0)
			{
				gameState2 = this._gameStates[num - 1];
			}
			return gameState2;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001A91C File Offset: 0x00018B1C
		public bool RegisterListener(IGameStateManagerListener listener)
		{
			if (this._listeners.Contains(listener))
			{
				return false;
			}
			this._listeners.Add(listener);
			return true;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001A93B File Offset: 0x00018B3B
		public bool UnregisterListener(IGameStateManagerListener listener)
		{
			return this._listeners.Remove(listener);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001A94C File Offset: 0x00018B4C
		public T GetListenerOfType<T>()
		{
			using (List<IGameStateManagerListener>.Enumerator enumerator = this._listeners.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGameStateManagerListener gameStateManagerListener;
					if ((gameStateManagerListener = enumerator.Current) is T)
					{
						return (T)((object)gameStateManagerListener);
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		public void RegisterActiveStateDisableRequest(object requestingInstance)
		{
			if (!this._activeStateDisableRequests.Contains(requestingInstance))
			{
				this._activeStateDisableRequests.Add(new WeakReference(requestingInstance));
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001A9DC File Offset: 0x00018BDC
		public void UnregisterActiveStateDisableRequest(object requestingInstance)
		{
			for (int i = 0; i < this._activeStateDisableRequests.Count; i++)
			{
				WeakReference weakReference = this._activeStateDisableRequests[i];
				if (((weakReference != null) ? weakReference.Target : null) == requestingInstance)
				{
					this._activeStateDisableRequests.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001AA28 File Offset: 0x00018C28
		public void OnSavedGameLoadFinished()
		{
			foreach (IGameStateManagerListener gameStateManagerListener in this._listeners)
			{
				gameStateManagerListener.OnSavedGameLoadFinished();
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001AA78 File Offset: 0x00018C78
		public T LastOrDefault<T>() where T : GameState
		{
			return this._gameStates.LastOrDefault<GameState>((GameState g) => g is T) as T;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001AAB0 File Offset: 0x00018CB0
		public T CreateState<T>() where T : GameState, new()
		{
			T t = new T();
			this.HandleCreateState(t);
			return t;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001AAD0 File Offset: 0x00018CD0
		public T CreateState<T>(params object[] parameters) where T : GameState, new()
		{
			GameState gameState = (GameState)Activator.CreateInstance(typeof(T), parameters);
			this.HandleCreateState(gameState);
			return (T)((object)gameState);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001AB00 File Offset: 0x00018D00
		private void HandleCreateState(GameState state)
		{
			state.GameStateManager = this;
			foreach (IGameStateManagerListener gameStateManagerListener in this._listeners)
			{
				gameStateManagerListener.OnCreateState(state);
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001AB58 File Offset: 0x00018D58
		public void OnTick(float dt)
		{
			this.CleanRequests();
			if (this.ActiveState != null)
			{
				if (this.ActiveStateDisabledByUser)
				{
					this.ActiveState.OnIdleTick(dt);
					return;
				}
				this.ActiveState.OnTick(dt);
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001AB8C File Offset: 0x00018D8C
		private void CleanRequests()
		{
			for (int i = this._activeStateDisableRequests.Count - 1; i >= 0; i--)
			{
				WeakReference weakReference = this._activeStateDisableRequests[i];
				if (weakReference == null || !weakReference.IsAlive)
				{
					this._activeStateDisableRequests.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001ABDC File Offset: 0x00018DDC
		public void PushState(GameState gameState, int level = 0)
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("State should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\GameStateManager.cs", "PushState", 222);
			}
			GameStateManager.GameStateJob gameStateJob = new GameStateManager.GameStateJob(GameStateManager.GameStateJob.JobType.Push, gameState, level);
			this._gameStateJobs.Enqueue(gameStateJob);
			this.DoGameStateJobs();
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001AC28 File Offset: 0x00018E28
		public void PopState(int level = 0)
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("State should be changed from main thread", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\GameStateManager.cs", "PopState", 235);
			}
			GameStateManager.GameStateJob gameStateJob = new GameStateManager.GameStateJob(GameStateManager.GameStateJob.JobType.Pop, null, level);
			this._gameStateJobs.Enqueue(gameStateJob);
			this.DoGameStateJobs();
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001AC74 File Offset: 0x00018E74
		public void CleanAndPushState(GameState gameState, int level = 0)
		{
			GameStateManager.GameStateJob gameStateJob = new GameStateManager.GameStateJob(GameStateManager.GameStateJob.JobType.CleanAndPushState, gameState, level);
			this._gameStateJobs.Enqueue(gameStateJob);
			this.DoGameStateJobs();
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0001ACA0 File Offset: 0x00018EA0
		public void CleanStates(int level = 0)
		{
			GameStateManager.GameStateJob gameStateJob = new GameStateManager.GameStateJob(GameStateManager.GameStateJob.JobType.CleanStates, null, level);
			this._gameStateJobs.Enqueue(gameStateJob);
			this.DoGameStateJobs();
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001ACCC File Offset: 0x00018ECC
		private void OnPushState(GameState gameState)
		{
			GameState activeState = this.ActiveState;
			bool flag = this._gameStates.Count == 0;
			int num = this._gameStates.FindLastIndex((GameState state) => state.Level <= gameState.Level);
			if (num == -1)
			{
				this._gameStates.Add(gameState);
			}
			else
			{
				this._gameStates.Insert(num + 1, gameState);
			}
			GameState activeState2 = this.ActiveState;
			if (activeState2 != activeState)
			{
				if (activeState != null && activeState.Activated)
				{
					activeState.HandleDeactivate();
				}
				foreach (IGameStateManagerListener gameStateManagerListener in this._listeners)
				{
					gameStateManagerListener.OnPushState(activeState2, flag);
				}
				activeState2.HandleInitialize();
				activeState2.HandleActivate();
				this.Owner.OnStateChanged(activeState);
			}
			Common.MemoryCleanupGC(false);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001ADC4 File Offset: 0x00018FC4
		private void OnPopState(int level)
		{
			GameState activeState = this.ActiveState;
			int num = this._gameStates.FindLastIndex((GameState state) => state.Level == level);
			GameState gameState = this._gameStates[num];
			gameState.HandleDeactivate();
			gameState.HandleFinalize();
			this._gameStates.RemoveAt(num);
			GameState activeState2 = this.ActiveState;
			foreach (IGameStateManagerListener gameStateManagerListener in this._listeners)
			{
				gameStateManagerListener.OnPopState(gameState);
			}
			if (activeState2 != activeState)
			{
				if (activeState2 != null)
				{
					activeState2.HandleActivate();
				}
				else if (this._gameStateJobs.Count == 0 || (this._gameStateJobs.Peek().Job != GameStateManager.GameStateJob.JobType.Push && this._gameStateJobs.Peek().Job != GameStateManager.GameStateJob.JobType.CleanAndPushState))
				{
					this.Owner.OnStateStackEmpty();
				}
				this.Owner.OnStateChanged(gameState);
			}
			Common.MemoryCleanupGC(false);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001AED4 File Offset: 0x000190D4
		private void OnCleanAndPushState(GameState gameState)
		{
			int num = -1;
			for (int i = 0; i < this._gameStates.Count; i++)
			{
				if (this._gameStates[i].Level >= gameState.Level)
				{
					num = i - 1;
					break;
				}
			}
			GameState activeState = this.ActiveState;
			for (int j = this._gameStates.Count - 1; j > num; j--)
			{
				GameState gameState2 = this._gameStates[j];
				if (gameState2.Activated)
				{
					gameState2.HandleDeactivate();
				}
				gameState2.HandleFinalize();
				this._gameStates.RemoveAt(j);
			}
			this.OnPushState(gameState);
			this.Owner.OnStateChanged(activeState);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001AF7C File Offset: 0x0001917C
		private void OnCleanStates(int popLevel)
		{
			int num = -1;
			for (int i = 0; i < this._gameStates.Count; i++)
			{
				if (this._gameStates[i].Level >= popLevel)
				{
					num = i - 1;
					break;
				}
			}
			GameState activeState = this.ActiveState;
			for (int j = this._gameStates.Count - 1; j > num; j--)
			{
				GameState gameState = this._gameStates[j];
				if (gameState.Activated)
				{
					gameState.HandleDeactivate();
				}
				gameState.HandleFinalize();
				this._gameStates.RemoveAt(j);
			}
			foreach (IGameStateManagerListener gameStateManagerListener in this._listeners)
			{
				gameStateManagerListener.OnCleanStates();
			}
			GameState activeState2 = this.ActiveState;
			if (activeState != activeState2)
			{
				if (activeState2 != null)
				{
					activeState2.HandleActivate();
				}
				else if (this._gameStateJobs.Count == 0 || (this._gameStateJobs.Peek().Job != GameStateManager.GameStateJob.JobType.Push && this._gameStateJobs.Peek().Job != GameStateManager.GameStateJob.JobType.CleanAndPushState))
				{
					this.Owner.OnStateStackEmpty();
				}
				this.Owner.OnStateChanged(activeState);
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001B0B8 File Offset: 0x000192B8
		private void DoGameStateJobs()
		{
			while (this._gameStateJobs.Count > 0)
			{
				GameStateManager.GameStateJob gameStateJob = this._gameStateJobs.Dequeue();
				switch (gameStateJob.Job)
				{
				case GameStateManager.GameStateJob.JobType.Push:
					this.OnPushState(gameStateJob.GameState);
					break;
				case GameStateManager.GameStateJob.JobType.Pop:
					this.OnPopState(gameStateJob.PopLevel);
					break;
				case GameStateManager.GameStateJob.JobType.CleanAndPushState:
					this.OnCleanAndPushState(gameStateJob.GameState);
					break;
				case GameStateManager.GameStateJob.JobType.CleanStates:
					this.OnCleanStates(gameStateJob.PopLevel);
					break;
				}
			}
		}

		// Token: 0x0400041B RID: 1051
		private static GameStateManager _current;

		// Token: 0x0400041C RID: 1052
		public static string StateActivateCommand;

		// Token: 0x0400041F RID: 1055
		private readonly List<GameState> _gameStates;

		// Token: 0x04000420 RID: 1056
		private readonly List<IGameStateManagerListener> _listeners;

		// Token: 0x04000421 RID: 1057
		private readonly List<WeakReference> _activeStateDisableRequests;

		// Token: 0x04000422 RID: 1058
		private readonly Queue<GameStateManager.GameStateJob> _gameStateJobs;

		// Token: 0x02000115 RID: 277
		public enum GameStateManagerType
		{
			// Token: 0x0400079F RID: 1951
			Game,
			// Token: 0x040007A0 RID: 1952
			Global
		}

		// Token: 0x02000116 RID: 278
		private struct GameStateJob
		{
			// Token: 0x06000BFA RID: 3066 RVA: 0x000266F9 File Offset: 0x000248F9
			public GameStateJob(GameStateManager.GameStateJob.JobType job, GameState gameState, int popLevel)
			{
				this.Job = job;
				this.GameState = gameState;
				this.PopLevel = popLevel;
			}

			// Token: 0x040007A1 RID: 1953
			public readonly GameStateManager.GameStateJob.JobType Job;

			// Token: 0x040007A2 RID: 1954
			public readonly GameState GameState;

			// Token: 0x040007A3 RID: 1955
			public readonly int PopLevel;

			// Token: 0x02000142 RID: 322
			public enum JobType
			{
				// Token: 0x0400083A RID: 2106
				None,
				// Token: 0x0400083B RID: 2107
				Push,
				// Token: 0x0400083C RID: 2108
				Pop,
				// Token: 0x0400083D RID: 2109
				CleanAndPushState,
				// Token: 0x0400083E RID: 2110
				CleanStates
			}
		}
	}
}
