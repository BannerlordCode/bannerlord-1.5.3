using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000066 RID: 102
	public class MPLobbyClanAnnouncementVM : ViewModel
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x0001ED4C File Offset: 0x0001CF4C
		public MPLobbyClanAnnouncementVM(PlayerId senderId, string message, DateTime date, int id, bool canBeDeleted)
		{
			this._id = id;
			this._senderId = senderId;
			this._announcedDate = date;
			this.SenderPlayer = new MPLobbyPlayerBaseVM(senderId, "", null, null);
			this.MessageText = message;
			this.CanBeDeleted = canBeDeleted;
			this.RefreshValues();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
		public override void RefreshValues()
		{
			base.RefreshValues();
			string text = new TextObject("{=oMiNaY1E}Posted By", null).ToString();
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("STR2", this.SenderPlayer.Name);
			string text2 = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			string dateFormattedByLanguage = LocalizedTextManager.GetDateFormattedByLanguage(BannerlordConfig.Language, this._announcedDate);
			GameTexts.SetVariable("STR1", text2);
			GameTexts.SetVariable("STR2", dateFormattedByLanguage);
			this.Details = new TextObject("{=QvDxB57o}{STR1} | {STR2}", null).ToString();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001EE34 File Offset: 0x0001D034
		private void ExecuteDeleteAnnouncement()
		{
			string text = new TextObject("{=P1MybNr7}Delete Announcement", null).ToString();
			string text2 = new TextObject("{=CW2JkWzC}Are you sure want to delete this announcement?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.DeleteAnnouncement), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0001EEAB File Offset: 0x0001D0AB
		private void DeleteAnnouncement()
		{
			NetworkMain.GameClient.RemoveClanAnnouncement(this._id);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0001EEBD File Offset: 0x0001D0BD
		// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0001EEC5 File Offset: 0x0001D0C5
		[DataSourceProperty]
		public bool CanBeDeleted
		{
			get
			{
				return this._canBeDeleted;
			}
			set
			{
				if (value != this._canBeDeleted)
				{
					this._canBeDeleted = value;
					base.OnPropertyChanged("CanBeDeleted");
				}
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0001EEE2 File Offset: 0x0001D0E2
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0001EEEA File Offset: 0x0001D0EA
		[DataSourceProperty]
		public string MessageText
		{
			get
			{
				return this._messageText;
			}
			set
			{
				if (value != this._messageText)
				{
					this._messageText = value;
					base.OnPropertyChanged("MessageText");
				}
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0001EF0C File Offset: 0x0001D10C
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0001EF14 File Offset: 0x0001D114
		[DataSourceProperty]
		public string Details
		{
			get
			{
				return this._details;
			}
			set
			{
				if (value != this._details)
				{
					this._details = value;
					base.OnPropertyChanged("Details");
				}
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0001EF36 File Offset: 0x0001D136
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0001EF3E File Offset: 0x0001D13E
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM SenderPlayer
		{
			get
			{
				return this._senderPlayer;
			}
			set
			{
				if (value != this._senderPlayer)
				{
					this._senderPlayer = value;
					base.OnPropertyChanged("SenderPlayer");
				}
			}
		}

		// Token: 0x0400048B RID: 1163
		private DateTime _announcedDate;

		// Token: 0x0400048C RID: 1164
		private PlayerId _senderId;

		// Token: 0x0400048D RID: 1165
		private int _id;

		// Token: 0x0400048E RID: 1166
		private bool _canBeDeleted;

		// Token: 0x0400048F RID: 1167
		private string _messageText;

		// Token: 0x04000490 RID: 1168
		private string _details;

		// Token: 0x04000491 RID: 1169
		private MPLobbyPlayerBaseVM _senderPlayer;
	}
}
