using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013F RID: 319
	public class InventoryEquippedItemSlotWidget : InventoryItemButtonWidget
	{
		// Token: 0x060010AB RID: 4267 RVA: 0x0002DC76 File Offset: 0x0002BE76
		public InventoryEquippedItemSlotWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0002DC80 File Offset: 0x0002BE80
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.ScreenWidget == null || this.Background == null)
			{
				return;
			}
			bool flag = base.ScreenWidget.TargetEquipmentIndex == this.TargetEquipmentIndex;
			bool flag2 = this.TargetEquipmentIndex == 0 && base.ScreenWidget.TargetEquipmentIndex >= 0 && base.ScreenWidget.TargetEquipmentIndex <= 3;
			if (flag || flag2)
			{
				this.Background.SetState("Selected");
				return;
			}
			this.Background.SetState("Default");
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0002DD08 File Offset: 0x0002BF08
		private void ImageIdentifierOnPropertyChanged(PropertyOwnerObject owner, string propertyName, object value)
		{
			if (propertyName == "ImageId")
			{
				base.IsHidden = string.IsNullOrEmpty((string)value);
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0002DD28 File Offset: 0x0002BF28
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x0002DD30 File Offset: 0x0002BF30
		[Editor(false)]
		public ImageIdentifierWidget ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (this._imageIdentifier != value)
				{
					if (this._imageIdentifier != null)
					{
						this._imageIdentifier.PropertyChanged -= this.ImageIdentifierOnPropertyChanged;
					}
					this._imageIdentifier = value;
					if (this._imageIdentifier != null)
					{
						this._imageIdentifier.PropertyChanged += this.ImageIdentifierOnPropertyChanged;
					}
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x0002DD97 File Offset: 0x0002BF97
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x0002DD9F File Offset: 0x0002BF9F
		[Editor(false)]
		public Widget Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (this._background != value)
				{
					this._background = value;
					this._background.AddState("Selected");
					base.OnPropertyChanged<Widget>(value, "Background");
				}
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0002DDCD File Offset: 0x0002BFCD
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x0002DDD5 File Offset: 0x0002BFD5
		[Editor(false)]
		public int TargetEquipmentIndex
		{
			get
			{
				return this._targetEquipmentIndex;
			}
			set
			{
				if (this._targetEquipmentIndex != value)
				{
					this._targetEquipmentIndex = value;
					base.OnPropertyChanged(value, "TargetEquipmentIndex");
				}
			}
		}

		// Token: 0x04000794 RID: 1940
		private ImageIdentifierWidget _imageIdentifier;

		// Token: 0x04000795 RID: 1941
		private Widget _background;

		// Token: 0x04000796 RID: 1942
		private int _targetEquipmentIndex;
	}
}
