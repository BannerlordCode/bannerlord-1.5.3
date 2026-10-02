using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005C RID: 92
	public class CampaignInformationManager
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060008F9 RID: 2297 RVA: 0x000280D4 File Offset: 0x000262D4
		// (remove) Token: 0x060008FA RID: 2298 RVA: 0x00028108 File Offset: 0x00026308
		public static event Func<TextObject, int, BasicCharacterObject, Equipment, MBInformationManager.NotificationPriority, string, MBInformationManager.DialogNotificationHandle> OnDisplayDialog;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060008FB RID: 2299 RVA: 0x0002813C File Offset: 0x0002633C
		// (remove) Token: 0x060008FC RID: 2300 RVA: 0x00028170 File Offset: 0x00026370
		public static event Func<MBInformationManager.DialogNotificationHandle, MBInformationManager.NotificationStatus> OnGetStatusOfDialogNotification;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060008FD RID: 2301 RVA: 0x000281A4 File Offset: 0x000263A4
		// (remove) Token: 0x060008FE RID: 2302 RVA: 0x000281D8 File Offset: 0x000263D8
		public static event Action<MBInformationManager.DialogNotificationHandle, bool> OnClearDialogNotification;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060008FF RID: 2303 RVA: 0x0002820C File Offset: 0x0002640C
		// (remove) Token: 0x06000900 RID: 2304 RVA: 0x00028240 File Offset: 0x00026440
		public static event Func<bool> IsAnyDialogNotificationActiveOrQueued;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000901 RID: 2305 RVA: 0x00028274 File Offset: 0x00026474
		// (remove) Token: 0x06000902 RID: 2306 RVA: 0x000282A8 File Offset: 0x000264A8
		public static event Action<bool> OnClearAllDialogNotifications;

		// Token: 0x06000903 RID: 2307 RVA: 0x000282DB File Offset: 0x000264DB
		public CampaignInformationManager()
		{
			this._mapNotices = new List<InformationData>();
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x000282F0 File Offset: 0x000264F0
		private void MapNoticeRemoved(InformationData obj)
		{
			int num = -1;
			for (int i = 0; i < this._mapNotices.Count; i++)
			{
				if (obj == this._mapNotices[i])
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				this._mapNotices.RemoveAt(num);
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00028338 File Offset: 0x00026538
		internal void NewLogEntryAdded(LogEntry log)
		{
			IChatNotification chatNotification;
			if (this._isSessionLaunched && (chatNotification = log as IChatNotification) != null && chatNotification.IsVisibleNotification)
			{
				InformationManager.DisplayMessage(new InformationMessage
				{
					Information = chatNotification.GetNotificationText().ToString(),
					Color = Color.FromUint(Campaign.Current.Models.DiplomacyModel.GetNotificationColor(chatNotification.NotificationType))
				});
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002839F File Offset: 0x0002659F
		private void AddInformationData(InformationData informationData)
		{
			List<InformationData> mapNotices = this._mapNotices;
			if (mapNotices != null)
			{
				mapNotices.Add(informationData);
			}
			MBInformationManager.AddNotice(informationData);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000283B9 File Offset: 0x000265B9
		internal void RegisterEvents()
		{
			this._isSessionLaunched = true;
			MBInformationManager.OnRemoveMapNotice += this.MapNoticeRemoved;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000283D3 File Offset: 0x000265D3
		internal void DeRegisterEvents()
		{
			this._isSessionLaunched = false;
			MBInformationManager.OnRemoveMapNotice -= this.MapNoticeRemoved;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000283F0 File Offset: 0x000265F0
		public void OnGameLoaded()
		{
			this._mapNotices.RemoveAll((InformationData t) => t == null || !t.IsValid());
			foreach (InformationData informationData in this._mapNotices)
			{
				MBInformationManager.AddNotice(informationData);
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002846C File Offset: 0x0002666C
		public void NewMapNoticeAdded(InformationData informationData)
		{
			this.AddInformationData(informationData);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00028478 File Offset: 0x00026678
		public bool InformationDataExists<T>(Func<T, bool> predicate) where T : InformationData
		{
			using (List<InformationData>.Enumerator enumerator = this._mapNotices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null && (predicate == null || predicate(t)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000284EC File Offset: 0x000266EC
		public static MBInformationManager.DialogNotificationHandle AddDialogLine(TextObject text, CharacterObject speakerCharacter, Equipment equipment = null, int extraTimeInMs = 0, MBInformationManager.NotificationPriority priority = MBInformationManager.NotificationPriority.Medium)
		{
			Debug.Print(text.ToString(), 0, Debug.DebugColor.White, 4503599627370496UL);
			Func<TextObject, int, BasicCharacterObject, Equipment, MBInformationManager.NotificationPriority, string, MBInformationManager.DialogNotificationHandle> onDisplayDialog = CampaignInformationManager.OnDisplayDialog;
			return ((onDisplayDialog != null) ? onDisplayDialog(text, extraTimeInMs, speakerCharacter, equipment, priority, CampaignInformationManager.GetSoundPath(text, speakerCharacter)) : null) ?? null;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00028528 File Offset: 0x00026728
		public static MBInformationManager.NotificationStatus GetStatusOfDialogNotification(MBInformationManager.DialogNotificationHandle handle)
		{
			Func<MBInformationManager.DialogNotificationHandle, MBInformationManager.NotificationStatus> onGetStatusOfDialogNotification = CampaignInformationManager.OnGetStatusOfDialogNotification;
			if (onGetStatusOfDialogNotification == null)
			{
				return MBInformationManager.NotificationStatus.Inactive;
			}
			return onGetStatusOfDialogNotification(handle);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002853B File Offset: 0x0002673B
		public static void ClearDialogNotification(MBInformationManager.DialogNotificationHandle handle, bool fadeOut = true)
		{
			Action<MBInformationManager.DialogNotificationHandle, bool> onClearDialogNotification = CampaignInformationManager.OnClearDialogNotification;
			if (onClearDialogNotification == null)
			{
				return;
			}
			onClearDialogNotification(handle, fadeOut);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0002854E File Offset: 0x0002674E
		public static bool GetIsAnyDialogNotificationActiveOrQueued()
		{
			Func<bool> isAnyDialogNotificationActiveOrQueued = CampaignInformationManager.IsAnyDialogNotificationActiveOrQueued;
			return isAnyDialogNotificationActiveOrQueued != null && isAnyDialogNotificationActiveOrQueued();
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00028560 File Offset: 0x00026760
		public static void ClearAllDialogNotifications(bool fadeOut)
		{
			Action<bool> onClearAllDialogNotifications = CampaignInformationManager.OnClearAllDialogNotifications;
			if (onClearAllDialogNotifications == null)
			{
				return;
			}
			onClearAllDialogNotifications(fadeOut);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00028574 File Offset: 0x00026774
		private static string GetSoundPath(TextObject line, CharacterObject characterObject)
		{
			VoiceObject voiceObject;
			string text;
			if (characterObject != null && MBTextManager.TryGetVoiceObject(line, out voiceObject, out text))
			{
				return Campaign.Current.Models.VoiceOverModel.GetSoundPathForCharacter(characterObject, voiceObject);
			}
			Debug.FailedAssert("Sound path for voice line not found! Character: " + ((characterObject != null) ? characterObject.ToString() : null) + ", Line: " + line.ToString(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignInformationManager.cs", "GetSoundPath", 180);
			return null;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x000285DD File Offset: 0x000267DD
		internal static void AutoGeneratedStaticCollectObjectsCampaignInformationManager(object o, List<object> collectedObjects)
		{
			((CampaignInformationManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000285EB File Offset: 0x000267EB
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._mapNotices);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x000285F9 File Offset: 0x000267F9
		internal static object AutoGeneratedGetMemberValue_mapNotices(object o)
		{
			return ((CampaignInformationManager)o)._mapNotices;
		}

		// Token: 0x040002CE RID: 718
		[SaveableField(10)]
		private List<InformationData> _mapNotices;

		// Token: 0x040002CF RID: 719
		[CachedData]
		private bool _isSessionLaunched;

		// Token: 0x0200053E RID: 1342
		public enum NoticeType
		{
			// Token: 0x040016FE RID: 5886
			None,
			// Token: 0x040016FF RID: 5887
			WarAnnouncement,
			// Token: 0x04001700 RID: 5888
			PeaceAnnouncement,
			// Token: 0x04001701 RID: 5889
			ChangeSettlementOwner,
			// Token: 0x04001702 RID: 5890
			FortificationIsCaptured,
			// Token: 0x04001703 RID: 5891
			HeroChangedFaction,
			// Token: 0x04001704 RID: 5892
			BarterAnnouncement
		}
	}
}
