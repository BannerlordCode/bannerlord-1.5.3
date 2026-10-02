using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E4 RID: 484
	public abstract class MBSubModuleBase
	{
		// Token: 0x06001C95 RID: 7317 RVA: 0x000620E6 File Offset: 0x000602E6
		protected internal virtual void OnSubModuleLoad()
		{
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000620E8 File Offset: 0x000602E8
		protected internal virtual void OnSubModuleUnloaded()
		{
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000620EA File Offset: 0x000602EA
		protected internal virtual void OnBeforeInitialModuleScreenSetAsRoot()
		{
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000620EC File Offset: 0x000602EC
		protected internal virtual void RegisterSubModuleTypes()
		{
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000620EE File Offset: 0x000602EE
		protected internal virtual void OnNewModuleLoad()
		{
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000620F0 File Offset: 0x000602F0
		public virtual void OnConfigChanged()
		{
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000620F2 File Offset: 0x000602F2
		protected internal virtual void OnBeforeGameStart(MBGameManager mbGameManager, List<string> disabledModules)
		{
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000620F4 File Offset: 0x000602F4
		protected internal virtual void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000620F6 File Offset: 0x000602F6
		protected internal virtual void OnApplicationTick(float dt)
		{
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000620F8 File Offset: 0x000602F8
		protected internal virtual void AfterAsyncTickTick(float dt)
		{
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000620FA File Offset: 0x000602FA
		protected internal virtual void InitializeGameStarter(Game game, IGameStarter starterObject)
		{
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000620FC File Offset: 0x000602FC
		public virtual void OnGameLoaded(Game game, object initializerObject)
		{
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000620FE File Offset: 0x000602FE
		public virtual void OnAfterGameLoaded(Game game)
		{
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00062100 File Offset: 0x00060300
		public virtual void OnNewGameCreated(Game game, object initializerObject)
		{
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x00062102 File Offset: 0x00060302
		public virtual void BeginGameStart(Game game)
		{
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00062104 File Offset: 0x00060304
		public virtual void OnCampaignStart(Game game, object starterObject)
		{
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00062106 File Offset: 0x00060306
		public virtual void RegisterSubModuleObjects(bool isSavedCampaign)
		{
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00062108 File Offset: 0x00060308
		public virtual void AfterRegisterSubModuleObjects(bool isSavedCampaign)
		{
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0006210A File Offset: 0x0006030A
		public virtual void OnMultiplayerGameStart(Game game, object starterObject)
		{
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0006210C File Offset: 0x0006030C
		public virtual void OnGameInitializationFinished(Game game)
		{
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0006210E File Offset: 0x0006030E
		public virtual void OnAfterGameInitializationFinished(Game game, object starterObject)
		{
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x00062110 File Offset: 0x00060310
		public virtual bool DoLoading(Game game)
		{
			return true;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x00062113 File Offset: 0x00060313
		public virtual void OnGameEnd(Game game)
		{
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x00062115 File Offset: 0x00060315
		public virtual void OnMissionBehaviorInitialize(Mission mission)
		{
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x00062117 File Offset: 0x00060317
		public virtual void OnBeforeMissionBehaviorInitialize(Mission mission)
		{
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x00062119 File Offset: 0x00060319
		public virtual void OnInitialState()
		{
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0006211B File Offset: 0x0006031B
		protected internal virtual void OnNetworkTick(float dt)
		{
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0006211D File Offset: 0x0006031D
		public virtual void OnSubModuleActivated()
		{
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x0006211F File Offset: 0x0006031F
		public virtual void OnSubModuleDeactivated()
		{
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00062121 File Offset: 0x00060321
		public virtual void InitializeSubModuleGameObjects(Game game)
		{
		}
	}
}
