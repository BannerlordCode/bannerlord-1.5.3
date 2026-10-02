using System;
using System.Threading.Tasks;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000054 RID: 84
	public static class InternetAvailabilityChecker
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000BD19 File Offset: 0x00009F19
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000BD20 File Offset: 0x00009F20
		public static bool InternetConnectionAvailable
		{
			get
			{
				return InternetAvailabilityChecker._internetConnectionAvailable;
			}
			private set
			{
				if (value != InternetAvailabilityChecker._internetConnectionAvailable)
				{
					InternetAvailabilityChecker._internetConnectionAvailable = value;
					Action<bool> onInternetConnectionAvailabilityChanged = InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged;
					if (onInternetConnectionAvailabilityChanged == null)
					{
						return;
					}
					onInternetConnectionAvailabilityChanged(value);
				}
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000BD40 File Offset: 0x00009F40
		private static async void CheckInternetConnection()
		{
			if (NetworkMain.GameClient != null)
			{
				InternetAvailabilityChecker.InternetConnectionAvailable = await NetworkMain.GameClient.CheckConnection();
			}
			InternetAvailabilityChecker._lastInternetConnectionCheck = DateTime.Now.Ticks;
			InternetAvailabilityChecker._checkingConnection = false;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000BD74 File Offset: 0x00009F74
		internal static void Tick(float dt)
		{
			long num = (InternetAvailabilityChecker.InternetConnectionAvailable ? 300000000L : 100000000L);
			if (Module.CurrentModule != null && Module.CurrentModule.StartupInfo.StartupType != GameStartupType.Singleplayer && !InternetAvailabilityChecker._checkingConnection && DateTime.Now.Ticks - InternetAvailabilityChecker._lastInternetConnectionCheck > num)
			{
				InternetAvailabilityChecker._checkingConnection = true;
				Task.Run(delegate
				{
					InternetAvailabilityChecker.CheckInternetConnection();
				});
			}
		}

		// Token: 0x040000DF RID: 223
		public static Action<bool> OnInternetConnectionAvailabilityChanged;

		// Token: 0x040000E0 RID: 224
		private static bool _internetConnectionAvailable;

		// Token: 0x040000E1 RID: 225
		private static long _lastInternetConnectionCheck;

		// Token: 0x040000E2 RID: 226
		private static bool _checkingConnection;

		// Token: 0x040000E3 RID: 227
		private const long InternetConnectionCheckIntervalShort = 100000000L;

		// Token: 0x040000E4 RID: 228
		private const long InternetConnectionCheckIntervalLong = 300000000L;
	}
}
