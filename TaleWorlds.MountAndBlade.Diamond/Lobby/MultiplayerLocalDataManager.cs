using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby
{
	// Token: 0x02000171 RID: 369
	public class MultiplayerLocalDataManager
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x000107AD File Offset: 0x0000E9AD
		// (set) Token: 0x06000A4C RID: 2636 RVA: 0x000107B4 File Offset: 0x0000E9B4
		public static MultiplayerLocalDataManager Instance { get; private set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x000107BC File Offset: 0x0000E9BC
		// (set) Token: 0x06000A4E RID: 2638 RVA: 0x000107C4 File Offset: 0x0000E9C4
		public TauntSlotDataContainer TauntSlotData { get; private set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x000107CD File Offset: 0x0000E9CD
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x000107D5 File Offset: 0x0000E9D5
		public MatchHistoryDataContainer MatchHistory { get; private set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x000107DE File Offset: 0x0000E9DE
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x000107E6 File Offset: 0x0000E9E6
		public FavoriteServerDataContainer FavoriteServers { get; private set; }

		// Token: 0x06000A53 RID: 2643 RVA: 0x000107EF File Offset: 0x0000E9EF
		private MultiplayerLocalDataManager()
		{
			this.TauntSlotData = new TauntSlotDataContainer();
			this.MatchHistory = new MatchHistoryDataContainer();
			this.FavoriteServers = new FavoriteServerDataContainer();
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00010818 File Offset: 0x0000EA18
		public static void InitializeManager()
		{
			if (MultiplayerLocalDataManager.Instance != null)
			{
				Debug.FailedAssert("Multiplayer local data manager is already initialized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "InitializeManager", 34);
				return;
			}
			MultiplayerLocalDataManager.Instance = new MultiplayerLocalDataManager();
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00010842 File Offset: 0x0000EA42
		public static void FinalizeManager()
		{
			if (MultiplayerLocalDataManager.Instance == null)
			{
				Debug.FailedAssert("Multiplayer local data manager is not initialized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "FinalizeManager", 45);
				return;
			}
			MultiplayerLocalDataManager.Instance.WaitForAsyncOperations();
			MultiplayerLocalDataManager.Instance = null;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00010874 File Offset: 0x0000EA74
		public async void Tick(float dt)
		{
			if (!this._isBusy)
			{
				this._isBusy = true;
				await this.TauntSlotData.Tick(dt);
				await this.MatchHistory.Tick(dt);
				await this.FavoriteServers.Tick(dt);
				this._isBusy = false;
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000108B5 File Offset: 0x0000EAB5
		private void WaitForAsyncOperations()
		{
			while (this._isBusy)
			{
			}
		}

		// Token: 0x0400052D RID: 1325
		internal const float FileUpdateIntervalInSeconds = 2f;

		// Token: 0x04000531 RID: 1329
		private bool _isBusy;
	}
}
