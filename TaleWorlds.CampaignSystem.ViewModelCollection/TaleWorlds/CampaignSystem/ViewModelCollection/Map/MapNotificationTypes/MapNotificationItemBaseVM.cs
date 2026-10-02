using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004C RID: 76
	public class MapNotificationItemBaseVM : ViewModel
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001FFA4 File Offset: 0x0001E1A4
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x0001FFAC File Offset: 0x0001E1AC
		public INavigationHandler NavigationHandler { get; private set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0001FFB5 File Offset: 0x0001E1B5
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001FFBD File Offset: 0x0001E1BD
		private protected Action<CampaignVec2> FastMoveCameraToPosition { protected get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001FFC6 File Offset: 0x0001E1C6
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x0001FFCE File Offset: 0x0001E1CE
		public InformationData Data { get; private set; }

		// Token: 0x0600061D RID: 1565 RVA: 0x0001FFD8 File Offset: 0x0001E1D8
		public MapNotificationItemBaseVM(InformationData data)
		{
			this.Data = data;
			this.ForceInspection = false;
			this.SoundId = data.SoundEventPath;
			this.RefreshValues();
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00020028 File Offset: 0x0001E228
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleText = this.Data.TitleText;
			this.TitleText = ((titleText != null) ? titleText.ToString() : null);
			TextObject descriptionText = this.Data.DescriptionText;
			this.DescriptionText = ((descriptionText != null) ? descriptionText.ToString() : null);
			this._removeHintText = this._removeHintTextObject.ToString();
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00020086 File Offset: 0x0001E286
		public void SetNavigationHandler(INavigationHandler navigationHandler)
		{
			this.NavigationHandler = navigationHandler;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0002008F File Offset: 0x0001E28F
		public void SetFastMoveCameraToPosition(Action<CampaignVec2> fastMoveCameraToPosition)
		{
			this.FastMoveCameraToPosition = fastMoveCameraToPosition;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00020098 File Offset: 0x0001E298
		public void ExecuteAction()
		{
			Action onInspect = this._onInspect;
			if (onInspect == null)
			{
				return;
			}
			onInspect();
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000200AA File Offset: 0x0001E2AA
		public void ExecuteRemove()
		{
			Action<MapNotificationItemBaseVM> onRemove = this.OnRemove;
			if (onRemove != null)
			{
				onRemove(this);
			}
			Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000200CF File Offset: 0x0001E2CF
		public void ExecuteSetFocused()
		{
			this.IsFocused = true;
			Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000200E9 File Offset: 0x0001E2E9
		public void ExecuteSetUnfocused()
		{
			this.IsFocused = false;
			Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00020103 File Offset: 0x0001E303
		public virtual void ManualRefreshRelevantStatus()
		{
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00020105 File Offset: 0x0001E305
		internal void GoToMapPosition(CampaignVec2 position)
		{
			Action<CampaignVec2> fastMoveCameraToPosition = this.FastMoveCameraToPosition;
			if (fastMoveCameraToPosition == null)
			{
				return;
			}
			fastMoveCameraToPosition(position);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00020118 File Offset: 0x0001E318
		public void SetRemoveInputKey(HotKey hotKey)
		{
			this.RemoveInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00020127 File Offset: 0x0001E327
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x0002012F File Offset: 0x0001E32F
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0002014D File Offset: 0x0001E34D
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x00020155 File Offset: 0x0001E355
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
					Action<MapNotificationItemBaseVM> onFocus = this.OnFocus;
					if (onFocus == null)
					{
						return;
					}
					onFocus(this);
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00020184 File Offset: 0x0001E384
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0002018C File Offset: 0x0001E38C
		[DataSourceProperty]
		public string NotificationIdentifier
		{
			get
			{
				return this._notificationIdentifier;
			}
			set
			{
				if (value != this._notificationIdentifier)
				{
					this._notificationIdentifier = value;
					base.OnPropertyChangedWithValue<string>(value, "NotificationIdentifier");
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x000201AF File Offset: 0x0001E3AF
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x000201B7 File Offset: 0x0001E3B7
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000201DA File Offset: 0x0001E3DA
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x000201E2 File Offset: 0x0001E3E2
		[DataSourceProperty]
		public bool ForceInspection
		{
			get
			{
				return this._forceInspection;
			}
			set
			{
				if (value != this._forceInspection)
				{
					Game game = Game.Current;
					if (game != null && !game.IsDevelopmentMode)
					{
						this._forceInspection = value;
						base.OnPropertyChangedWithValue(value, "ForceInspection");
					}
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00020216 File Offset: 0x0001E416
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x0002021E File Offset: 0x0001E41E
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00020241 File Offset: 0x0001E441
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00020249 File Offset: 0x0001E449
		[DataSourceProperty]
		public string SoundId
		{
			get
			{
				return this._soundId;
			}
			set
			{
				if (value != this._soundId)
				{
					this._soundId = value;
					base.OnPropertyChangedWithValue<string>(value, "SoundId");
				}
			}
		}

		// Token: 0x0400028D RID: 653
		internal Action<MapNotificationItemBaseVM> OnRemove;

		// Token: 0x0400028E RID: 654
		internal Action<MapNotificationItemBaseVM> OnFocus;

		// Token: 0x0400028F RID: 655
		protected Action _onInspect;

		// Token: 0x04000291 RID: 657
		private readonly TextObject _removeHintTextObject = new TextObject("{=Bcs9s2tC}Right Click to Remove", null);

		// Token: 0x04000292 RID: 658
		private string _removeHintText;

		// Token: 0x04000293 RID: 659
		private InputKeyItemVM _removeInputKey;

		// Token: 0x04000294 RID: 660
		private bool _isFocused;

		// Token: 0x04000295 RID: 661
		private string _titleText;

		// Token: 0x04000296 RID: 662
		private string _descriptionText;

		// Token: 0x04000297 RID: 663
		private string _soundId;

		// Token: 0x04000298 RID: 664
		private bool _forceInspection;

		// Token: 0x04000299 RID: 665
		private string _notificationIdentifier = "Default";
	}
}
