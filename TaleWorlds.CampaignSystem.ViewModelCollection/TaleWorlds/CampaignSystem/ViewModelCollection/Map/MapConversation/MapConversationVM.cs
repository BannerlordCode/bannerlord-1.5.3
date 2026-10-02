using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Conversation;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapConversation
{
	// Token: 0x0200005E RID: 94
	public class MapConversationVM : ViewModel
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x000213C0 File Offset: 0x0001F5C0
		public MapConversationVM(Action onContinue, Func<string> getContinueInputText)
		{
			this._onContinue = onContinue;
			this.DialogController = new MissionConversationVM(getContinueInputText, false);
			this.TableauData = null;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000213E3 File Offset: 0x0001F5E3
		public void ExecuteContinue()
		{
			Action onContinue = this._onContinue;
			if (onContinue == null)
			{
				return;
			}
			onContinue();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000213F5 File Offset: 0x0001F5F5
		public override void OnFinalize()
		{
			base.OnFinalize();
			MissionConversationVM dialogController = this.DialogController;
			if (dialogController != null)
			{
				dialogController.OnFinalize();
			}
			this.DialogController = null;
			this.TableauData = null;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002141C File Offset: 0x0001F61C
		public void Tick(float dt)
		{
			MissionConversationVM dialogController = this.DialogController;
			if (dialogController == null)
			{
				return;
			}
			dialogController.Tick(dt);
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0002142F File Offset: 0x0001F62F
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x00021437 File Offset: 0x0001F637
		[DataSourceProperty]
		public MissionConversationVM DialogController
		{
			get
			{
				return this._dialogController;
			}
			set
			{
				if (value != this._dialogController)
				{
					this._dialogController = value;
					base.OnPropertyChangedWithValue<MissionConversationVM>(value, "DialogController");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x00021455 File Offset: 0x0001F655
		// (set) Token: 0x06000687 RID: 1671 RVA: 0x0002145D File Offset: 0x0001F65D
		[DataSourceProperty]
		public object TableauData
		{
			get
			{
				return this._tableauData;
			}
			set
			{
				if (value != this._tableauData)
				{
					this._tableauData = value;
					base.OnPropertyChangedWithValue<object>(value, "TableauData");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0002147B File Offset: 0x0001F67B
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x00021483 File Offset: 0x0001F683
		[DataSourceProperty]
		public bool IsBarterActive
		{
			get
			{
				return this._isBarterActive;
			}
			set
			{
				if (value != this._isBarterActive)
				{
					this._isBarterActive = value;
					base.OnPropertyChangedWithValue(value, "IsBarterActive");
				}
			}
		}

		// Token: 0x040002BC RID: 700
		private readonly Action _onContinue;

		// Token: 0x040002BD RID: 701
		private MissionConversationVM _dialogController;

		// Token: 0x040002BE RID: 702
		private object _tableauData;

		// Token: 0x040002BF RID: 703
		private bool _isBarterActive;
	}
}
