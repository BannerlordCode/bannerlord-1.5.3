using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000180 RID: 384
	public abstract class EncyclopediaPage
	{
		// Token: 0x06001C07 RID: 7175
		protected abstract IEnumerable<EncyclopediaListItem> InitializeListItems();

		// Token: 0x06001C08 RID: 7176
		protected abstract IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems();

		// Token: 0x06001C09 RID: 7177
		protected abstract IEnumerable<EncyclopediaSortController> InitializeSortControllers();

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00090E82 File Offset: 0x0008F082
		// (set) Token: 0x06001C0B RID: 7179 RVA: 0x00090E8A File Offset: 0x0008F08A
		public int HomePageOrderIndex { get; protected set; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x00090E93 File Offset: 0x0008F093
		public EncyclopediaPage Parent { get; }

		// Token: 0x06001C0D RID: 7181 RVA: 0x00090E9C File Offset: 0x0008F09C
		public EncyclopediaPage()
		{
			this._filters = this.InitializeFilterItems();
			this._items = this.InitializeListItems();
			this._sortControllers = new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=koX9okuG}None", null), new EncyclopediaListItemNameComparer())
			};
			((List<EncyclopediaSortController>)this._sortControllers).AddRange(this.InitializeSortControllers());
			foreach (object obj in base.GetType().GetCustomAttributesSafe(typeof(EncyclopediaModel), true))
			{
				if (obj is EncyclopediaModel)
				{
					this._identifierTypes = (obj as EncyclopediaModel).PageTargetTypes;
					break;
				}
			}
			this._identifiers = new Dictionary<Type, string>();
			foreach (Type type in this._identifierTypes)
			{
				if (Game.Current.ObjectManager.HasType(type))
				{
					this._identifiers.Add(type, Game.Current.ObjectManager.FindRegisteredClassPrefix(type));
				}
				else
				{
					string text = type.Name.ToString();
					if (text == "Clan")
					{
						text = "Faction";
					}
					this._identifiers.Add(type, text);
				}
			}
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x00090FD1 File Offset: 0x0008F1D1
		public virtual bool IsRelevant()
		{
			return true;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00090FD4 File Offset: 0x0008F1D4
		public bool HasIdentifierType(Type identifierType)
		{
			return this._identifierTypes.Contains(identifierType);
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00090FE2 File Offset: 0x0008F1E2
		internal bool HasIdentifier(string identifier)
		{
			return this._identifiers.ContainsValue(identifier);
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00090FF0 File Offset: 0x0008F1F0
		public string GetIdentifier(Type identifierType)
		{
			if (this._identifiers.ContainsKey(identifierType))
			{
				return this._identifiers[identifierType];
			}
			return "";
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x00091012 File Offset: 0x0008F212
		public string[] GetIdentifierNames()
		{
			return this._identifiers.Values.ToArray<string>();
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00091024 File Offset: 0x0008F224
		public bool IsFiltered(object o)
		{
			using (IEnumerator<EncyclopediaFilterGroup> enumerator = this.GetFilterItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Predicate(o))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x00091080 File Offset: 0x0008F280
		public virtual string GetViewFullyQualifiedName()
		{
			return "";
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00091087 File Offset: 0x0008F287
		public virtual string GetStringID()
		{
			return "";
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0009108E File Offset: 0x0008F28E
		public virtual TextObject GetName()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00091095 File Offset: 0x0008F295
		public virtual MBObjectBase GetObject(string typeName, string stringID)
		{
			return MBObjectManager.Instance.GetObject(typeName, stringID);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000910A3 File Offset: 0x0008F2A3
		public virtual bool IsValidEncyclopediaItem(object o)
		{
			return false;
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000910A6 File Offset: 0x0008F2A6
		public virtual TextObject GetDescriptionText()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000910AD File Offset: 0x0008F2AD
		public IEnumerable<EncyclopediaListItem> GetListItems()
		{
			return this._items;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000910B5 File Offset: 0x0008F2B5
		public IEnumerable<EncyclopediaFilterGroup> GetFilterItems()
		{
			return this._filters;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000910BD File Offset: 0x0008F2BD
		public IEnumerable<EncyclopediaSortController> GetSortControllers()
		{
			return this._sortControllers;
		}

		// Token: 0x0400095E RID: 2398
		private readonly Type[] _identifierTypes;

		// Token: 0x0400095F RID: 2399
		private readonly Dictionary<Type, string> _identifiers;

		// Token: 0x04000960 RID: 2400
		private IEnumerable<EncyclopediaFilterGroup> _filters;

		// Token: 0x04000961 RID: 2401
		private IEnumerable<EncyclopediaListItem> _items;

		// Token: 0x04000962 RID: 2402
		private IEnumerable<EncyclopediaSortController> _sortControllers;
	}
}
