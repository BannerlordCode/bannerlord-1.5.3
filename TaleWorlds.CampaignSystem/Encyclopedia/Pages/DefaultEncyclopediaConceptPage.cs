using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000188 RID: 392
	[EncyclopediaModel(new Type[] { typeof(Concept) })]
	public class DefaultEncyclopediaConceptPage : EncyclopediaPage
	{
		// Token: 0x06001C3B RID: 7227 RVA: 0x000914F6 File Offset: 0x0008F6F6
		public DefaultEncyclopediaConceptPage()
		{
			base.HomePageOrderIndex = 600;
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00091509 File Offset: 0x0008F709
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			foreach (Concept concept in Concept.All)
			{
				yield return new EncyclopediaListItem(concept, concept.Title.ToString(), concept.Description.ToString(), concept.StringId, base.GetIdentifier(typeof(Concept)), true, null);
			}
			List<Concept>.Enumerator enumerator = default(List<Concept>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0009151C File Offset: 0x0008F71C
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=uauMia0D} Characters", null), (object c) => Concept.IsGroupMember("Characters", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=cwRkqIt4} Kingdoms", null), (object c) => Concept.IsGroupMember("Kingdoms", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=x6knoNnC} Clans", null), (object c) => Concept.IsGroupMember("Clans", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=GYzkb4iB} Parties", null), (object c) => Concept.IsGroupMember("Parties", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=u6GM5Spa} Armies", null), (object c) => Concept.IsGroupMember("Armies", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=zPYRGJtD} Troops", null), (object c) => Concept.IsGroupMember("Troops", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=3PUkH5Zf} Items", null), (object c) => Concept.IsGroupMember("Items", (Concept)c)));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=xKVBAL3m} Campaign Issues", null), (object c) => Concept.IsGroupMember("CampaignIssues", (Concept)c)));
			list.Add(new EncyclopediaFilterGroup(list2, new TextObject("{=tBx7XXps}Types", null)));
			return list;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000916F3 File Offset: 0x0008F8F3
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>();
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000916FA File Offset: 0x0008F8FA
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaConceptPage";
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00091701 File Offset: 0x0008F901
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_concepts", null);
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0009170E File Offset: 0x0008F90E
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_concepts_description", null);
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x0009171B File Offset: 0x0008F91B
		public override string GetStringID()
		{
			return "EncyclopediaConcept";
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00091724 File Offset: 0x0008F924
		public override bool IsValidEncyclopediaItem(object o)
		{
			Concept concept = o as Concept;
			return concept != null && concept.Title != null && concept.Description != null;
		}
	}
}
