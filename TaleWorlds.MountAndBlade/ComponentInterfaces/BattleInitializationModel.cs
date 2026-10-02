using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000406 RID: 1030
	public abstract class BattleInitializationModel : MBGameModel<BattleInitializationModel>
	{
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003873 RID: 14451 RVA: 0x000E94DA File Offset: 0x000E76DA
		// (set) Token: 0x06003874 RID: 14452 RVA: 0x000E94E1 File Offset: 0x000E76E1
		public static bool BypassPlayerDeployment { get; private set; }

		// Token: 0x06003875 RID: 14453
		public abstract List<FormationClass> GetAllAvailableTroopTypes();

		// Token: 0x06003876 RID: 14454
		protected abstract bool CanPlayerSideDeployWithOrderOfBattleAux();

		// Token: 0x06003877 RID: 14455 RVA: 0x000E94E9 File Offset: 0x000E76E9
		public bool CanPlayerSideDeployWithOrderOfBattle()
		{
			if (!this._isCanPlayerSideDeployWithOOBCached)
			{
				this._canPlayerSideDeployWithOOB = !BattleInitializationModel.BypassPlayerDeployment && this.CanPlayerSideDeployWithOrderOfBattleAux();
				this._isCanPlayerSideDeployWithOOBCached = true;
			}
			return this._canPlayerSideDeployWithOOB;
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000E9516 File Offset: 0x000E7716
		public void InitializeModel()
		{
			this._isCanPlayerSideDeployWithOOBCached = false;
			this._isInitialized = true;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000E9526 File Offset: 0x000E7726
		public void FinalizeModel()
		{
			this._isInitialized = false;
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x000E9530 File Offset: 0x000E7730
		public static void SetBypassPlayerDeployment(bool value)
		{
			MissionGameModels missionGameModels = MissionGameModels.Current;
			BattleInitializationModel battleInitializationModel = ((missionGameModels != null) ? missionGameModels.BattleInitializationModel : null);
			if (battleInitializationModel != null && BattleInitializationModel.BypassPlayerDeployment != value)
			{
				battleInitializationModel._isCanPlayerSideDeployWithOOBCached = false;
			}
			BattleInitializationModel.BypassPlayerDeployment = value;
		}

		// Token: 0x0400184C RID: 6220
		public const int MinimumTroopCountForPlayerDeployment = 20;

		// Token: 0x0400184E RID: 6222
		private bool _canPlayerSideDeployWithOOB;

		// Token: 0x0400184F RID: 6223
		private bool _isCanPlayerSideDeployWithOOBCached;

		// Token: 0x04001850 RID: 6224
		private bool _isInitialized;
	}
}
