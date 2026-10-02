using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000141 RID: 321
	public abstract class InventoryItemButtonWidget : ButtonWidget
	{
		// Token: 0x060010B6 RID: 4278 RVA: 0x0002DE28 File Offset: 0x0002C028
		protected InventoryItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0002DE31 File Offset: 0x0002C031
		protected override void OnDragBegin()
		{
			InventoryScreenWidget screenWidget = this.ScreenWidget;
			if (screenWidget != null)
			{
				screenWidget.ItemWidgetDragBegin(this);
			}
			base.OnDragBegin();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0002DE4B File Offset: 0x0002C04B
		protected override bool OnDrop()
		{
			InventoryScreenWidget screenWidget = this.ScreenWidget;
			if (screenWidget != null)
			{
				screenWidget.ItemWidgetDrop(this);
			}
			return base.OnDrop();
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0002DE68 File Offset: 0x0002C068
		private void AssignScreenWidget()
		{
			Widget widget = this;
			while (widget != base.EventManager.Root && this._screenWidget == null)
			{
				if (widget is InventoryScreenWidget)
				{
					this._screenWidget = (InventoryScreenWidget)widget;
				}
				else
				{
					widget = widget.ParentWidget;
				}
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0002DEAC File Offset: 0x0002C0AC
		private void ItemTypeUpdated()
		{
			AudioProperty audioProperty = base.Brush.SoundProperties.GetEventAudioProperty("DragEnd");
			if (audioProperty == null)
			{
				audioProperty = new AudioProperty();
				base.Brush.SoundProperties.AddEventSound("DragEnd", audioProperty);
			}
			audioProperty.AudioName = this.GetSound(this.ItemType);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0002DF00 File Offset: 0x0002C100
		private string GetSound(string typeID)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeID);
			if (num <= 1387635315U)
			{
				if (num <= 778761250U)
				{
					if (num <= 498656566U)
					{
						if (num != 302839205U)
						{
							if (num != 368918302U)
							{
								if (num != 498656566U)
								{
									goto IL_042D;
								}
								if (!(typeID == "Sling"))
								{
									goto IL_042D;
								}
								return "inventory/bow";
							}
							else
							{
								if (!(typeID == "LegArmor"))
								{
									goto IL_042D;
								}
								goto IL_0409;
							}
						}
						else
						{
							if (!(typeID == "ChestArmor"))
							{
								goto IL_042D;
							}
							goto IL_0403;
						}
					}
					else if (num != 678699352U)
					{
						if (num != 731742070U)
						{
							if (num != 778761250U)
							{
								goto IL_042D;
							}
							if (!(typeID == "HeadArmor"))
							{
								goto IL_042D;
							}
							return "inventory/helmet";
						}
						else
						{
							if (!(typeID == "Pistol"))
							{
								goto IL_042D;
							}
							goto IL_0427;
						}
					}
					else
					{
						if (!(typeID == "Book"))
						{
							goto IL_042D;
						}
						return "inventory/book";
					}
				}
				else if (num <= 995063962U)
				{
					if (num != 784896431U)
					{
						if (num != 881552253U)
						{
							if (num != 995063962U)
							{
								goto IL_042D;
							}
							if (!(typeID == "Cape"))
							{
								goto IL_042D;
							}
							goto IL_0403;
						}
						else
						{
							if (!(typeID == "Animal"))
							{
								goto IL_042D;
							}
							return "inventory/animal";
						}
					}
					else
					{
						if (!(typeID == "Banner"))
						{
							goto IL_042D;
						}
						return "inventory/perk";
					}
				}
				else if (num <= 1061154663U)
				{
					if (num != 1048100111U)
					{
						if (num != 1061154663U)
						{
							goto IL_042D;
						}
						if (!(typeID == "OneHandedWeapon"))
						{
							goto IL_042D;
						}
						return "inventory/onehanded";
					}
					else if (!(typeID == "Bolts"))
					{
						goto IL_042D;
					}
				}
				else if (num != 1095128646U)
				{
					if (num != 1387635315U)
					{
						goto IL_042D;
					}
					if (!(typeID == "Thrown"))
					{
						goto IL_042D;
					}
					return "inventory/throwing";
				}
				else
				{
					if (!(typeID == "Horse"))
					{
						goto IL_042D;
					}
					return "inventory/horse";
				}
			}
			else if (num <= 2996768862U)
			{
				if (num <= 1982439889U)
				{
					if (num != 1486204743U)
					{
						if (num != 1721772824U)
						{
							if (num != 1982439889U)
							{
								goto IL_042D;
							}
							if (!(typeID == "Goods"))
							{
								goto IL_042D;
							}
							return "inventory/sack";
						}
						else if (!(typeID == "SlingStones"))
						{
							goto IL_042D;
						}
					}
					else
					{
						if (!(typeID == "HandArmor"))
						{
							goto IL_042D;
						}
						goto IL_0409;
					}
				}
				else if (num != 2039097040U)
				{
					if (num != 2161253412U)
					{
						if (num != 2996768862U)
						{
							goto IL_042D;
						}
						if (!(typeID == "Bullets"))
						{
							goto IL_042D;
						}
						goto IL_0427;
					}
					else
					{
						if (!(typeID == "BodyArmor"))
						{
							goto IL_042D;
						}
						goto IL_0403;
					}
				}
				else
				{
					if (!(typeID == "Shield"))
					{
						goto IL_042D;
					}
					return "inventory/shield";
				}
			}
			else if (num <= 3618788796U)
			{
				if (num != 3083591375U)
				{
					if (num != 3565557811U)
					{
						if (num != 3618788796U)
						{
							goto IL_042D;
						}
						if (!(typeID == "Musket"))
						{
							goto IL_042D;
						}
						goto IL_0427;
					}
					else
					{
						if (!(typeID == "Polearm"))
						{
							goto IL_042D;
						}
						return "inventory/polearm";
					}
				}
				else if (!(typeID == "Arrows"))
				{
					goto IL_042D;
				}
			}
			else if (num <= 3656874833U)
			{
				if (num != 3637216139U)
				{
					if (num != 3656874833U)
					{
						goto IL_042D;
					}
					if (!(typeID == "TwoHandedWeapon"))
					{
						goto IL_042D;
					}
					return "inventory/twohanded";
				}
				else
				{
					if (!(typeID == "Bow"))
					{
						goto IL_042D;
					}
					return "inventory/bow";
				}
			}
			else if (num != 3918828990U)
			{
				if (num != 4282369777U)
				{
					goto IL_042D;
				}
				if (!(typeID == "Crossbow"))
				{
					goto IL_042D;
				}
				return "inventory/crossbow";
			}
			else
			{
				if (!(typeID == "HorseHarness"))
				{
					goto IL_042D;
				}
				return "inventory/horsearmor";
			}
			return "inventory/quiver";
			IL_0403:
			return "inventory/leather";
			IL_0409:
			return "inventory/leather_lite";
			IL_0427:
			return "inventory/leather";
			IL_042D:
			return "inventory/leather";
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0002E33F File Offset: 0x0002C53F
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x0002E347 File Offset: 0x0002C547
		[Editor(false)]
		public bool IsRightSide
		{
			get
			{
				return this._isRightSide;
			}
			set
			{
				if (this._isRightSide != value)
				{
					this._isRightSide = value;
					base.OnPropertyChanged(value, "IsRightSide");
				}
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0002E365 File Offset: 0x0002C565
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x0002E36D File Offset: 0x0002C56D
		[Editor(false)]
		public string ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (this._itemType != value)
				{
					this._itemType = value;
					base.OnPropertyChanged<string>(value, "ItemType");
					this.ItemTypeUpdated();
				}
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0002E396 File Offset: 0x0002C596
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x0002E39E File Offset: 0x0002C59E
		[Editor(false)]
		public int EquipmentIndex
		{
			get
			{
				return this._equipmentIndex;
			}
			set
			{
				if (this._equipmentIndex != value)
				{
					this._equipmentIndex = value;
					base.OnPropertyChanged(value, "EquipmentIndex");
				}
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0002E3BC File Offset: 0x0002C5BC
		public InventoryScreenWidget ScreenWidget
		{
			get
			{
				if (this._screenWidget == null)
				{
					this.AssignScreenWidget();
				}
				return this._screenWidget;
			}
		}

		// Token: 0x04000797 RID: 1943
		private bool _isRightSide;

		// Token: 0x04000798 RID: 1944
		private string _itemType;

		// Token: 0x04000799 RID: 1945
		private int _equipmentIndex;

		// Token: 0x0400079A RID: 1946
		private InventoryScreenWidget _screenWidget;
	}
}
