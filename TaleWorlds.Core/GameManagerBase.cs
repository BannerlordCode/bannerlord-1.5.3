using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200006D RID: 109
	public abstract class GameManagerBase
	{
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00019F85 File Offset: 0x00018185
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x00019F8C File Offset: 0x0001818C
		public static GameManagerBase Current { get; private set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00019F94 File Offset: 0x00018194
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00019F9C File Offset: 0x0001819C
		public Game Game
		{
			get
			{
				return this._game;
			}
			internal set
			{
				if (value == null)
				{
					this._game = null;
					this._initialized = false;
					return;
				}
				this._game = value;
				this.Initialize();
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00019FBD File Offset: 0x000181BD
		public void Initialize()
		{
			if (!this._initialized)
			{
				this._initialized = true;
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00019FCE File Offset: 0x000181CE
		protected GameManagerBase()
		{
			GameManagerBase.Current = this;
			this._entitySystem = new EntitySystem<GameManagerComponent>();
			this._stepNo = GameManagerLoadingSteps.PreInitializeZerothStep;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00019FEE File Offset: 0x000181EE
		public IEnumerable<GameManagerComponent> Components
		{
			get
			{
				return this._entitySystem.Components;
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00019FFB File Offset: 0x000181FB
		public GameManagerComponent AddComponent(Type componentType)
		{
			GameManagerComponent gameManagerComponent = this._entitySystem.AddComponent(componentType);
			gameManagerComponent.GameManager = this;
			return gameManagerComponent;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001A010 File Offset: 0x00018210
		public T AddComponent<T>() where T : GameManagerComponent, new()
		{
			return (T)((object)this.AddComponent(typeof(T)));
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001A027 File Offset: 0x00018227
		public GameManagerComponent GetComponent(Type componentType)
		{
			return this._entitySystem.GetComponent(componentType);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001A035 File Offset: 0x00018235
		public T GetComponent<T>() where T : GameManagerComponent
		{
			return this._entitySystem.GetComponent<T>();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001A042 File Offset: 0x00018242
		public IEnumerable<T> GetComponents<T>() where T : GameManagerComponent
		{
			return this._entitySystem.GetComponents<T>();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001A050 File Offset: 0x00018250
		public void RemoveComponent<T>() where T : GameManagerComponent
		{
			T component = this._entitySystem.GetComponent<T>();
			this.RemoveComponent(component);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001A075 File Offset: 0x00018275
		public void RemoveComponent(GameManagerComponent component)
		{
			this._entitySystem.RemoveComponent(component);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001A084 File Offset: 0x00018284
		public void OnTick(float dt)
		{
			foreach (GameManagerComponent gameManagerComponent in this._entitySystem.Components)
			{
				gameManagerComponent.OnTick();
			}
			if (this.Game != null)
			{
				this.Game.OnTick(dt);
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001A0F0 File Offset: 0x000182F0
		public void OnGameNetworkBegin()
		{
			foreach (GameManagerComponent gameManagerComponent in this._entitySystem.Components)
			{
				gameManagerComponent.OnGameNetworkBegin();
			}
			if (this.Game != null)
			{
				this.Game.OnGameNetworkBegin();
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001A158 File Offset: 0x00018358
		public void OnGameNetworkEnd()
		{
			foreach (GameManagerComponent gameManagerComponent in this._entitySystem.Components)
			{
				gameManagerComponent.OnGameNetworkEnd();
			}
			if (this.Game != null)
			{
				this.Game.OnGameNetworkEnd();
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001A1C0 File Offset: 0x000183C0
		public void OnPlayerConnect(VirtualPlayer peer)
		{
			foreach (GameManagerComponent gameManagerComponent in this._entitySystem.Components)
			{
				gameManagerComponent.OnEarlyPlayerConnect(peer);
			}
			if (this.Game != null)
			{
				this.Game.OnEarlyPlayerConnect(peer);
			}
			foreach (GameManagerComponent gameManagerComponent2 in this._entitySystem.Components)
			{
				gameManagerComponent2.OnPlayerConnect(peer);
			}
			if (this.Game != null)
			{
				this.Game.OnPlayerConnect(peer);
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001A284 File Offset: 0x00018484
		public void OnPlayerDisconnect(VirtualPlayer peer)
		{
			foreach (GameManagerComponent gameManagerComponent in this._entitySystem.Components)
			{
				gameManagerComponent.OnPlayerDisconnect(peer);
			}
			if (this.Game != null)
			{
				this.Game.OnPlayerDisconnect(peer);
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001A2F0 File Offset: 0x000184F0
		public virtual void OnGameEnd(Game game)
		{
			GameManagerBase.Current = null;
			this.Game = null;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001A2FF File Offset: 0x000184FF
		protected virtual void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001A304 File Offset: 0x00018504
		public bool DoLoadingForGameManager()
		{
			bool flag = false;
			GameManagerLoadingSteps gameManagerLoadingSteps = GameManagerLoadingSteps.None;
			switch (this._stepNo)
			{
			case GameManagerLoadingSteps.PreInitializeZerothStep:
				this.DoLoadingForGameManager(GameManagerLoadingSteps.PreInitializeZerothStep, out gameManagerLoadingSteps);
				if (gameManagerLoadingSteps == GameManagerLoadingSteps.FirstInitializeFirstStep)
				{
					this._stepNo++;
				}
				break;
			case GameManagerLoadingSteps.FirstInitializeFirstStep:
				this.DoLoadingForGameManager(GameManagerLoadingSteps.FirstInitializeFirstStep, out gameManagerLoadingSteps);
				if (gameManagerLoadingSteps == GameManagerLoadingSteps.WaitSecondStep)
				{
					this._stepNo++;
				}
				break;
			case GameManagerLoadingSteps.WaitSecondStep:
				this.DoLoadingForGameManager(GameManagerLoadingSteps.WaitSecondStep, out gameManagerLoadingSteps);
				if (gameManagerLoadingSteps == GameManagerLoadingSteps.SecondInitializeThirdState)
				{
					this._stepNo++;
				}
				break;
			case GameManagerLoadingSteps.SecondInitializeThirdState:
				this.DoLoadingForGameManager(GameManagerLoadingSteps.SecondInitializeThirdState, out gameManagerLoadingSteps);
				if (gameManagerLoadingSteps == GameManagerLoadingSteps.PostInitializeFourthState)
				{
					this._stepNo++;
				}
				break;
			case GameManagerLoadingSteps.PostInitializeFourthState:
				this.DoLoadingForGameManager(GameManagerLoadingSteps.PostInitializeFourthState, out gameManagerLoadingSteps);
				if (gameManagerLoadingSteps == GameManagerLoadingSteps.FinishLoadingFifthStep)
				{
					this._stepNo++;
				}
				break;
			case GameManagerLoadingSteps.FinishLoadingFifthStep:
				this.DoLoadingForGameManager(GameManagerLoadingSteps.FinishLoadingFifthStep, out gameManagerLoadingSteps);
				if (gameManagerLoadingSteps == GameManagerLoadingSteps.None)
				{
					this._stepNo++;
					flag = true;
				}
				break;
			case GameManagerLoadingSteps.LoadingIsOver:
				flag = true;
				break;
			}
			return flag;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001A402 File Offset: 0x00018602
		public virtual void OnLoadFinished()
		{
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001A404 File Offset: 0x00018604
		public virtual void InitializeGameStarter(Game game, IGameStarter starterObject)
		{
		}

		// Token: 0x060007C6 RID: 1990
		public abstract void OnGameStart(Game game, IGameStarter gameStarter);

		// Token: 0x060007C7 RID: 1991
		public abstract void BeginGameStart(Game game);

		// Token: 0x060007C8 RID: 1992
		public abstract void OnNewCampaignStart(Game game, object starterObject);

		// Token: 0x060007C9 RID: 1993
		public abstract void OnAfterCampaignStart(Game game);

		// Token: 0x060007CA RID: 1994
		public abstract void RegisterSubModuleObjects(bool isSavedCampaign);

		// Token: 0x060007CB RID: 1995
		public abstract void AfterRegisterSubModuleObjects(bool isSavedCampaign);

		// Token: 0x060007CC RID: 1996
		public abstract void OnGameInitializationFinished(Game game);

		// Token: 0x060007CD RID: 1997
		public abstract void OnNewGameCreated(Game game, object initializerObject);

		// Token: 0x060007CE RID: 1998
		public abstract void OnGameLoaded(Game game, object initializerObject);

		// Token: 0x060007CF RID: 1999
		public abstract void OnAfterGameLoaded(Game game);

		// Token: 0x060007D0 RID: 2000
		public abstract void OnAfterGameInitializationFinished(Game game, object initializerObject);

		// Token: 0x060007D1 RID: 2001
		public abstract void RegisterSubModuleTypes();

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001A406 File Offset: 0x00018606
		public virtual void InitializeSubModuleGameObjects(Game game)
		{
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060007D3 RID: 2003
		public abstract float ApplicationTime { get; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060007D4 RID: 2004
		public abstract bool CheatMode { get; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060007D5 RID: 2005
		public abstract bool IsDevelopmentMode { get; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060007D6 RID: 2006
		public abstract bool IsEditModeOn { get; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060007D7 RID: 2007
		public abstract UnitSpawnPrioritizations UnitSpawnPrioritization { get; }

		// Token: 0x0400040F RID: 1039
		private EntitySystem<GameManagerComponent> _entitySystem;

		// Token: 0x04000410 RID: 1040
		private GameManagerLoadingSteps _stepNo;

		// Token: 0x04000412 RID: 1042
		private Game _game;

		// Token: 0x04000413 RID: 1043
		private bool _initialized;
	}
}
