using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000084 RID: 132
	public class DefaultItems
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00051FA8 File Offset: 0x000501A8
		private static DefaultItems Instance
		{
			get
			{
				return Campaign.Current.DefaultItems;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00051FB4 File Offset: 0x000501B4
		public static ItemObject Grain
		{
			get
			{
				return DefaultItems.Instance._itemGrain;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x00051FC0 File Offset: 0x000501C0
		public static ItemObject Planks
		{
			get
			{
				return DefaultItems.Instance._itemPlanks;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00051FCC File Offset: 0x000501CC
		public static ItemObject Felt
		{
			get
			{
				return DefaultItems.Instance._itemFelt;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00051FD8 File Offset: 0x000501D8
		public static ItemObject Meat
		{
			get
			{
				return DefaultItems.Instance._itemMeat;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00051FE4 File Offset: 0x000501E4
		public static ItemObject Hides
		{
			get
			{
				return DefaultItems.Instance._itemHides;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00051FF0 File Offset: 0x000501F0
		public static ItemObject Tools
		{
			get
			{
				return DefaultItems.Instance._itemTools;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00051FFC File Offset: 0x000501FC
		public static ItemObject IronOre
		{
			get
			{
				return DefaultItems.Instance._itemIronOre;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00052008 File Offset: 0x00050208
		public static ItemObject HardWood
		{
			get
			{
				return DefaultItems.Instance._itemHardwood;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00052014 File Offset: 0x00050214
		public static ItemObject Charcoal
		{
			get
			{
				return DefaultItems.Instance._itemCharcoal;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00052020 File Offset: 0x00050220
		public static ItemObject IronIngot1
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot1;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0005202C File Offset: 0x0005022C
		public static ItemObject IronIngot2
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot2;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00052038 File Offset: 0x00050238
		public static ItemObject IronIngot3
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot3;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00052044 File Offset: 0x00050244
		public static ItemObject IronIngot4
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot4;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x00052050 File Offset: 0x00050250
		public static ItemObject IronIngot5
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot5;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0005205C File Offset: 0x0005025C
		public static ItemObject IronIngot6
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot6;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x00052068 File Offset: 0x00050268
		public static ItemObject Trash
		{
			get
			{
				return DefaultItems.Instance._itemTrash;
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00052074 File Offset: 0x00050274
		public DefaultItems()
		{
			this.RegisterAll();
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00052084 File Offset: 0x00050284
		private void RegisterAll()
		{
			this._itemGrain = this.Create("grain");
			this._itemFelt = this.Create("felt");
			this._itemPlanks = this.Create("planks");
			this._itemMeat = this.Create("meat");
			this._itemHides = this.Create("hides");
			this._itemTools = this.Create("tools");
			this._itemIronOre = this.Create("iron");
			this._itemHardwood = this.Create("hardwood");
			this._itemCharcoal = this.Create("charcoal");
			this._itemIronIngot1 = this.Create("ironIngot1");
			this._itemIronIngot2 = this.Create("ironIngot2");
			this._itemIronIngot3 = this.Create("ironIngot3");
			this._itemIronIngot4 = this.Create("ironIngot4");
			this._itemIronIngot5 = this.Create("ironIngot5");
			this._itemIronIngot6 = this.Create("ironIngot6");
			this._itemTrash = this.Create("trash");
			this.InitializeAll();
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000521A7 File Offset: 0x000503A7
		private ItemObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<ItemObject>(new ItemObject(stringId));
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000521C0 File Offset: 0x000503C0
		private void InitializeAll()
		{
			ItemObject.InitializeTradeGood(this._itemGrain, new TextObject("{=Itv3fgJm}Grain{@Plural}loads of grain{\\@}", null), "merchandise_grain", DefaultItemCategories.Grain, 10, 10f, ItemObject.ItemTypeEnum.Goods, true);
			ItemObject.InitializeTradeGood(this._itemMeat, new TextObject("{=LmwhFv5p}Meat{@Plural}loads of meat{\\@}", null), "merchandise_meat", DefaultItemCategories.Meat, 30, 10f, ItemObject.ItemTypeEnum.Goods, true);
			ItemObject.InitializeTradeGood(this._itemPlanks, new TextObject("{=5ac8Boz1}Planks{@Plural}loads of planks{\\@}", null), "bd_planks_a", DefaultItemCategories.Planks, 180, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemFelt, new TextObject("{=hNwjpCVP}Felt{@Plural}rolls of felt{\\@}", null), "merchandise_hides_b", DefaultItemCategories.Felt, 230, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemHides, new TextObject("{=4kvKQuXM}Hides{@Plural}loads of hide{\\@}", null), "merchandise_hides_b", DefaultItemCategories.Hides, 50, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemTools, new TextObject("{=n3cjEB0X}Tools{@Plural}loads of tools{\\@}", null), "bd_pickaxe_b", DefaultItemCategories.Tools, 250, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronOre, new TextObject("{=Kw6BkhIf}Iron Ore{@Plural}loads of iron ore{\\@}", null), "iron_ore", DefaultItemCategories.Iron, 50, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemHardwood, new TextObject("{=ExjMoUiT}Hardwood{@Plural}hardwood logs{\\@}", null), "hardwood", DefaultItemCategories.Wood, 25, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemCharcoal, new TextObject("{=iQadPYNe}Charcoal{@Plural}loads of charcoal{\\@}", null), "charcoal", DefaultItemCategories.Wood, 50, 5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot1, new TextObject("{=gOpodlt1}Crude Iron{@Plural}loads of crude iron{\\@}", null), "crude_iron", DefaultItemCategories.Iron, 20, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot2, new TextObject("{=7HvtT8bm}Wrought Iron{@Plural}loads of wrought iron{\\@}", null), "wrought_iron", DefaultItemCategories.Iron, 30, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot3, new TextObject("{=XHmmbnbB}Iron{@Plural}loads of iron{\\@}", null), "iron_a", DefaultItemCategories.Iron, 60, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot4, new TextObject("{=UfuLKuaI}Steel{@Plural}loads of steel{\\@}", null), "steel", DefaultItemCategories.Iron, 100, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot5, new TextObject("{=azjMBa86}Fine Steel{@Plural}loads of fine steel{\\@}", null), "fine_steel", DefaultItemCategories.Iron, 160, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot6, new TextObject("{=vLVAfcta}Thamaskene Steel{@Plural}loads of thamaskene steel{\\@}", null), "thamaskene_steel", DefaultItemCategories.Iron, 260, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemTrash, new TextObject("{=ZvZN6UkU}Trash Item", null), "iron_ore", DefaultItemCategories.Unassigned, 1, 1f, ItemObject.ItemTypeEnum.Goods, false);
		}

		// Token: 0x040004F8 RID: 1272
		private const float TradeGoodWeight = 10f;

		// Token: 0x040004F9 RID: 1273
		private const float HalfWeight = 5f;

		// Token: 0x040004FA RID: 1274
		private const float IngotWeight = 0.5f;

		// Token: 0x040004FB RID: 1275
		private const float TrashWeight = 1f;

		// Token: 0x040004FC RID: 1276
		private const int IngotValue = 20;

		// Token: 0x040004FD RID: 1277
		private const int TrashValue = 1;

		// Token: 0x040004FE RID: 1278
		private ItemObject _itemGrain;

		// Token: 0x040004FF RID: 1279
		private ItemObject _itemPlanks;

		// Token: 0x04000500 RID: 1280
		private ItemObject _itemFelt;

		// Token: 0x04000501 RID: 1281
		private ItemObject _itemMeat;

		// Token: 0x04000502 RID: 1282
		private ItemObject _itemHides;

		// Token: 0x04000503 RID: 1283
		private ItemObject _itemTools;

		// Token: 0x04000504 RID: 1284
		private ItemObject _itemIronOre;

		// Token: 0x04000505 RID: 1285
		private ItemObject _itemHardwood;

		// Token: 0x04000506 RID: 1286
		private ItemObject _itemCharcoal;

		// Token: 0x04000507 RID: 1287
		private ItemObject _itemIronIngot1;

		// Token: 0x04000508 RID: 1288
		private ItemObject _itemIronIngot2;

		// Token: 0x04000509 RID: 1289
		private ItemObject _itemIronIngot3;

		// Token: 0x0400050A RID: 1290
		private ItemObject _itemIronIngot4;

		// Token: 0x0400050B RID: 1291
		private ItemObject _itemIronIngot5;

		// Token: 0x0400050C RID: 1292
		private ItemObject _itemIronIngot6;

		// Token: 0x0400050D RID: 1293
		private ItemObject _itemTrash;
	}
}
