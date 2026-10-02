using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000117 RID: 279
	public class ListAdvancedStartOption : AdvancedStartOption
	{
		// Token: 0x06000DAC RID: 3500 RVA: 0x00062850 File Offset: 0x00060A50
		public ListAdvancedStartOption(string stringId, string categoryId, [TupleElementNames(new string[] { "Identifier", "Condition" })] IReadOnlyList<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> items, AdvancedStartOption.AdvancedStartOptionCondition onCondition, string defaultValue = "")
			: base(new AdvancedStartData<string>(stringId, categoryId, defaultValue), onCondition)
		{
			this._items = new List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>>(items);
			for (int i = 0; i < this._items.Count; i++)
			{
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00062890 File Offset: 0x00060A90
		[return: TupleElementNames(new string[] { "Identifier", "Condition" })]
		public IReadOnlyList<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> GetItems()
		{
			return this._items;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00062898 File Offset: 0x00060A98
		public void AddItem([TupleElementNames(new string[] { "Identifier", "Condition" })] ValueTuple<string, ListAdvancedStartOption.ListItemCondition> item)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i].Item1 == item.Item1)
				{
					Debug.Print("Overriding start option item id: " + item.Item1, 0, Debug.DebugColor.White, 17592186044416UL);
					this._items[i] = item;
					return;
				}
			}
			this._items.Add(item);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00062914 File Offset: 0x00060B14
		private bool TryGetItem(string identifier, [TupleElementNames(new string[] { "Identifier", "Condition" })] out ValueTuple<string, ListAdvancedStartOption.ListItemCondition> item)
		{
			if (this._items == null)
			{
				item = default(ValueTuple<string, ListAdvancedStartOption.ListItemCondition>);
				return false;
			}
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i].Item1 == identifier)
				{
					item = this._items[i];
					return true;
				}
			}
			item = default(ValueTuple<string, ListAdvancedStartOption.ListItemCondition>);
			return false;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0006297D File Offset: 0x00060B7D
		internal TextObject GetListItemName(string identifier)
		{
			return Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_item_name", identifier);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00062994 File Offset: 0x00060B94
		internal TextObject GetListItemDescription(string identifier)
		{
			TextObject textObject;
			if (Module.CurrentModule.GlobalTextManager.TryGetText("str_campaign_starting_options_item_description", identifier, out textObject))
			{
				return textObject;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000629C4 File Offset: 0x00060BC4
		public bool GetItemCondition(string identifier, AdvancedStartOptions options, out TextObject disabledText)
		{
			bool flag = false;
			disabledText = null;
			ValueTuple<string, ListAdvancedStartOption.ListItemCondition> valueTuple;
			if (this.TryGetItem(identifier, out valueTuple) && valueTuple.Item2 != null)
			{
				flag = valueTuple.Item2(options, out disabledText);
			}
			return flag;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x000629F8 File Offset: 0x00060BF8
		public bool RemoveItem(string identifier)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i].Item1 == identifier)
				{
					this._items.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x040005D2 RID: 1490
		[TupleElementNames(new string[] { "Identifier", "Condition" })]
		private readonly List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> _items;

		// Token: 0x0200023F RID: 575
		// (Invoke) Token: 0x06001487 RID: 5255
		public delegate bool ListItemCondition(AdvancedStartOptions options, out TextObject disabledText);
	}
}
