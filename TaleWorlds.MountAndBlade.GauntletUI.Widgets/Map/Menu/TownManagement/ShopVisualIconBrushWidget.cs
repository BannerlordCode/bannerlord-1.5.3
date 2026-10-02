using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Menu.TownManagement
{
	// Token: 0x02000126 RID: 294
	public class ShopVisualIconBrushWidget : BrushWidget
	{
		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002B666 File Offset: 0x00029866
		public ShopVisualIconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0002B670 File Offset: 0x00029870
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				string shopId = this.ShopId;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(shopId);
				if (num <= 1487947867U)
				{
					if (num <= 480574884U)
					{
						if (num != 53567524U)
						{
							if (num != 413646574U)
							{
								if (num == 480574884U)
								{
									if (shopId == "tannery")
									{
										this.SetState("Tannery");
										goto IL_0314;
									}
								}
							}
							else if (shopId == "empty")
							{
								this.SetState("Default");
								goto IL_0314;
							}
						}
						else if (shopId == "wool_weavery")
						{
							this.SetState("WoolWeavery");
							goto IL_0314;
						}
					}
					else if (num <= 654125368U)
					{
						if (num != 570845286U)
						{
							if (num == 654125368U)
							{
								if (shopId == "olive_press")
								{
									this.SetState("OlivePress");
									goto IL_0314;
								}
							}
						}
						else if (shopId == "silversmithy")
						{
							this.SetState("SilverSmithy");
							goto IL_0314;
						}
					}
					else if (num != 1167966747U)
					{
						if (num == 1487947867U)
						{
							if (shopId == "velvet_weavery")
							{
								this.SetState("VelvetWeavery");
								goto IL_0314;
							}
						}
					}
					else if (shopId == "pottery_shop")
					{
						this.SetState("PotteryShop");
						goto IL_0314;
					}
				}
				else if (num <= 1841523956U)
				{
					if (num != 1527712715U)
					{
						if (num != 1741159131U)
						{
							if (num == 1841523956U)
							{
								if (shopId == "wood_WorkshopType")
								{
									this.SetState("WoodWorkshop");
									goto IL_0314;
								}
							}
						}
						else if (shopId == "linen_weavery")
						{
							this.SetState("LinenWeavery");
							goto IL_0314;
						}
					}
					else if (shopId == "smithy")
					{
						this.SetState("Smithy");
						goto IL_0314;
					}
				}
				else if (num <= 3164387478U)
				{
					if (num != 2207150787U)
					{
						if (num == 3164387478U)
						{
							if (shopId == "stable")
							{
								this.SetState("Stable");
								goto IL_0314;
							}
						}
					}
					else if (shopId == "mill")
					{
						this.SetState("Mill");
						goto IL_0314;
					}
				}
				else if (num != 3470543696U)
				{
					if (num == 4109692093U)
					{
						if (shopId == "brewery")
						{
							this.SetState("Brewery");
							goto IL_0314;
						}
					}
				}
				else if (shopId == "wine_press")
				{
					this.SetState("WinePress");
					goto IL_0314;
				}
				this.SetState("Default");
				Debug.FailedAssert("No workshop visual with this type: " + this.ShopId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Map\\Menu\\TownManagement\\ShopVisualIconBrushWidget.cs", "OnLateUpdate", 68);
				IL_0314:
				this._initialized = true;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0002B998 File Offset: 0x00029B98
		// (set) Token: 0x06000FB7 RID: 4023 RVA: 0x0002B9A0 File Offset: 0x00029BA0
		[Editor(false)]
		public string ShopId
		{
			get
			{
				return this._shopId;
			}
			set
			{
				if (this._shopId != value)
				{
					this._shopId = value;
					base.OnPropertyChanged<string>(value, "ShopId");
				}
			}
		}

		// Token: 0x0400072F RID: 1839
		private bool _initialized;

		// Token: 0x04000730 RID: 1840
		private string _shopId;
	}
}
