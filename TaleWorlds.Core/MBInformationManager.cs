using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x020000B0 RID: 176
	public static class MBInformationManager
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000937 RID: 2359 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
		// (remove) Token: 0x06000938 RID: 2360 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static event Action<string, int, BasicCharacterObject, Equipment, string> FiringQuickInformation;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000939 RID: 2361 RVA: 0x0001E320 File Offset: 0x0001C520
		// (remove) Token: 0x0600093A RID: 2362 RVA: 0x0001E354 File Offset: 0x0001C554
		public static event Action ClearingQuickInformations;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600093B RID: 2363 RVA: 0x0001E388 File Offset: 0x0001C588
		// (remove) Token: 0x0600093C RID: 2364 RVA: 0x0001E3BC File Offset: 0x0001C5BC
		public static event Action<MultiSelectionInquiryData, bool, bool> OnShowMultiSelectionInquiry;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600093D RID: 2365 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		// (remove) Token: 0x0600093E RID: 2366 RVA: 0x0001E424 File Offset: 0x0001C624
		public static event Action<InformationData> OnAddMapNotice;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600093F RID: 2367 RVA: 0x0001E458 File Offset: 0x0001C658
		// (remove) Token: 0x06000940 RID: 2368 RVA: 0x0001E48C File Offset: 0x0001C68C
		public static event Action<InformationData> OnRemoveMapNotice;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000941 RID: 2369 RVA: 0x0001E4C0 File Offset: 0x0001C6C0
		// (remove) Token: 0x06000942 RID: 2370 RVA: 0x0001E4F4 File Offset: 0x0001C6F4
		public static event Action<SceneNotificationData> OnShowSceneNotification;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000943 RID: 2371 RVA: 0x0001E528 File Offset: 0x0001C728
		// (remove) Token: 0x06000944 RID: 2372 RVA: 0x0001E55C File Offset: 0x0001C75C
		public static event Action OnHideSceneNotification;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000945 RID: 2373 RVA: 0x0001E590 File Offset: 0x0001C790
		// (remove) Token: 0x06000946 RID: 2374 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
		public static event Func<bool> IsAnySceneNotificationActive;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000947 RID: 2375 RVA: 0x0001E5F8 File Offset: 0x0001C7F8
		// (remove) Token: 0x06000948 RID: 2376 RVA: 0x0001E62C File Offset: 0x0001C82C
		public static event Func<SceneNotificationData> ActiveSceneNotificationData;

		// Token: 0x06000949 RID: 2377 RVA: 0x0001E65F File Offset: 0x0001C85F
		public static void AddQuickInformation(TextObject message, int extraTimeInMs = 0, BasicCharacterObject announcerCharacter = null, Equipment equipment = null, string soundEventPath = "")
		{
			Action<string, int, BasicCharacterObject, Equipment, string> firingQuickInformation = MBInformationManager.FiringQuickInformation;
			if (firingQuickInformation != null)
			{
				firingQuickInformation(message.ToString(), extraTimeInMs, announcerCharacter, equipment, soundEventPath);
			}
			Debug.Print(message.ToString(), 0, Debug.DebugColor.White, 1125899906842624UL);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001E693 File Offset: 0x0001C893
		public static void ClearQuickInformations()
		{
			Action clearingQuickInformations = MBInformationManager.ClearingQuickInformations;
			if (clearingQuickInformations == null)
			{
				return;
			}
			clearingQuickInformations();
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
		public static void ShowMultiSelectionInquiry(MultiSelectionInquiryData data, bool pauseGameActiveState = false, bool prioritize = false)
		{
			Action<MultiSelectionInquiryData, bool, bool> onShowMultiSelectionInquiry = MBInformationManager.OnShowMultiSelectionInquiry;
			if (onShowMultiSelectionInquiry == null)
			{
				return;
			}
			onShowMultiSelectionInquiry(data, pauseGameActiveState, prioritize);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001E6B8 File Offset: 0x0001C8B8
		public static void AddNotice(InformationData data)
		{
			Action<InformationData> onAddMapNotice = MBInformationManager.OnAddMapNotice;
			if (onAddMapNotice == null)
			{
				return;
			}
			onAddMapNotice(data);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001E6CA File Offset: 0x0001C8CA
		public static void MapNoticeRemoved(InformationData data)
		{
			Action<InformationData> onRemoveMapNotice = MBInformationManager.OnRemoveMapNotice;
			if (onRemoveMapNotice == null)
			{
				return;
			}
			onRemoveMapNotice(data);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001E6DC File Offset: 0x0001C8DC
		public static void ShowHint(string hint)
		{
			InformationManager.ShowTooltip(typeof(string), new object[] { hint });
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001E6F7 File Offset: 0x0001C8F7
		public static void HideInformations()
		{
			InformationManager.HideTooltip();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001E6FE File Offset: 0x0001C8FE
		public static void ShowSceneNotification(SceneNotificationData data)
		{
			Action<SceneNotificationData> onShowSceneNotification = MBInformationManager.OnShowSceneNotification;
			if (onShowSceneNotification == null)
			{
				return;
			}
			onShowSceneNotification(data);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001E710 File Offset: 0x0001C910
		public static void HideSceneNotification()
		{
			Action onHideSceneNotification = MBInformationManager.OnHideSceneNotification;
			if (onHideSceneNotification == null)
			{
				return;
			}
			onHideSceneNotification();
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001E724 File Offset: 0x0001C924
		public static bool? GetIsAnySceneNotificationActive()
		{
			Func<bool> isAnySceneNotificationActive = MBInformationManager.IsAnySceneNotificationActive;
			if (isAnySceneNotificationActive == null)
			{
				return null;
			}
			return new bool?(isAnySceneNotificationActive());
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001E74E File Offset: 0x0001C94E
		public static SceneNotificationData GetActiveSceneNotificationData()
		{
			Func<SceneNotificationData> activeSceneNotificationData = MBInformationManager.ActiveSceneNotificationData;
			if (activeSceneNotificationData == null)
			{
				return null;
			}
			return activeSceneNotificationData();
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001E760 File Offset: 0x0001C960
		public static void Clear()
		{
			MBInformationManager.FiringQuickInformation = null;
			MBInformationManager.OnShowMultiSelectionInquiry = null;
			MBInformationManager.OnAddMapNotice = null;
			MBInformationManager.OnRemoveMapNotice = null;
			MBInformationManager.OnShowSceneNotification = null;
			MBInformationManager.OnHideSceneNotification = null;
		}

		// Token: 0x02000121 RID: 289
		public enum NotificationPriority
		{
			// Token: 0x040007BF RID: 1983
			Lowest,
			// Token: 0x040007C0 RID: 1984
			Low,
			// Token: 0x040007C1 RID: 1985
			Medium,
			// Token: 0x040007C2 RID: 1986
			High,
			// Token: 0x040007C3 RID: 1987
			Highest
		}

		// Token: 0x02000122 RID: 290
		public enum NotificationStatus
		{
			// Token: 0x040007C5 RID: 1989
			Inactive,
			// Token: 0x040007C6 RID: 1990
			CurrentlyActive,
			// Token: 0x040007C7 RID: 1991
			InQueue
		}

		// Token: 0x02000123 RID: 291
		public class DialogNotificationHandle
		{
		}
	}
}
