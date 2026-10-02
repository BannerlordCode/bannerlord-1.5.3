using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002B RID: 43
	public class ItemTableauWidget : TextureWidget
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00007FA4 File Offset: 0x000061A4
		// (set) Token: 0x06000233 RID: 563 RVA: 0x00007FAC File Offset: 0x000061AC
		[Editor(false)]
		public string ItemModifierId
		{
			get
			{
				return this._itemModifierId;
			}
			set
			{
				if (value != this._itemModifierId)
				{
					this._itemModifierId = value;
					base.OnPropertyChanged<string>(value, "ItemModifierId");
					base.SetTextureProviderProperty("ItemModifierId", value);
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00007FDB File Offset: 0x000061DB
		// (set) Token: 0x06000235 RID: 565 RVA: 0x00007FE3 File Offset: 0x000061E3
		[Editor(false)]
		public string StringId
		{
			get
			{
				return this._stringId;
			}
			set
			{
				if (value != this._stringId)
				{
					this._stringId = value;
					base.OnPropertyChanged<string>(value, "StringId");
					if (value != null)
					{
						base.SetTextureProviderProperty("StringId", value);
					}
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00008015 File Offset: 0x00006215
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000801D File Offset: 0x0000621D
		[Editor(false)]
		public float InitialTiltRotation
		{
			get
			{
				return this._initialTiltRotation;
			}
			set
			{
				if (value != this._initialTiltRotation)
				{
					this._initialTiltRotation = value;
					base.OnPropertyChanged(value, "InitialTiltRotation");
					base.SetTextureProviderProperty("InitialTiltRotation", value);
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000804C File Offset: 0x0000624C
		// (set) Token: 0x06000239 RID: 569 RVA: 0x00008054 File Offset: 0x00006254
		[Editor(false)]
		public float InitialPanRotation
		{
			get
			{
				return this._initialPanRotation;
			}
			set
			{
				if (value != this._initialPanRotation)
				{
					this._initialPanRotation = value;
					base.OnPropertyChanged(value, "InitialPanRotation");
					base.SetTextureProviderProperty("InitialPanRotation", value);
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00008083 File Offset: 0x00006283
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000808B File Offset: 0x0000628B
		[Editor(false)]
		public string BannerCode
		{
			get
			{
				return this._bannerCode;
			}
			set
			{
				if (value != this._bannerCode)
				{
					this._bannerCode = value;
					base.OnPropertyChanged<string>(value, "BannerCode");
					base.SetTextureProviderProperty("BannerCode", value);
				}
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000080BA File Offset: 0x000062BA
		public ItemTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "ItemTableauTextureProvider";
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000080CE File Offset: 0x000062CE
		protected override bool OnPreviewMouseScroll()
		{
			return true;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000080D1 File Offset: 0x000062D1
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			base.SetTextureProviderProperty("CurrentZoom", Input.DeltaMouseScroll * 0.1f);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x000080F4 File Offset: 0x000062F4
		protected override void OnMousePressed()
		{
			base.SetTextureProviderProperty("CurrentlyRotating", true);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00008107 File Offset: 0x00006307
		protected override void OnRightStickMovement()
		{
			base.OnRightStickMovement();
			base.SetTextureProviderProperty("RotateItemVertical", base.EventManager.RightStickVerticalScrollAmount);
			base.SetTextureProviderProperty("RotateItemHorizontal", base.EventManager.RightStickHorizontalScrollAmount);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00008145 File Offset: 0x00006345
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.SetTextureProviderProperty("CurrentlyRotating", false);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00008158 File Offset: 0x00006358
		protected override bool OnPreviewRightStickMovement()
		{
			return true;
		}

		// Token: 0x04000108 RID: 264
		private string _itemModifierId;

		// Token: 0x04000109 RID: 265
		private string _stringId;

		// Token: 0x0400010A RID: 266
		private float _initialTiltRotation;

		// Token: 0x0400010B RID: 267
		private float _initialPanRotation;

		// Token: 0x0400010C RID: 268
		private string _bannerCode;
	}
}
