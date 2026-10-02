using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000077 RID: 119
	public class GameText
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0001B138 File Offset: 0x00019338
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0001B140 File Offset: 0x00019340
		public string Id { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001B149 File Offset: 0x00019349
		public IEnumerable<GameText.GameTextVariation> Variations
		{
			get
			{
				return this._variationList;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001B151 File Offset: 0x00019351
		public TextObject DefaultText
		{
			get
			{
				if (this._variationList != null && this._variationList.Count > 0)
				{
					return this._variationList[0].Text;
				}
				return null;
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001B17C File Offset: 0x0001937C
		internal GameText()
		{
			this._variationList = new List<GameText.GameTextVariation>();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001B18F File Offset: 0x0001938F
		internal GameText(string id)
		{
			this.Id = id;
			this._variationList = new List<GameText.GameTextVariation>();
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001B1AC File Offset: 0x000193AC
		internal TextObject GetVariation(string variationId)
		{
			foreach (GameText.GameTextVariation gameTextVariation in this._variationList)
			{
				if (gameTextVariation.Id.Equals(variationId))
				{
					return gameTextVariation.Text;
				}
			}
			return null;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001B214 File Offset: 0x00019414
		public void AddVariationWithId(string variationId, TextObject text, List<GameTextManager.ChoiceTag> choiceTags)
		{
			foreach (GameText.GameTextVariation gameTextVariation in this._variationList)
			{
				if (gameTextVariation.Id.Equals(variationId) && gameTextVariation.Text.ToString().Equals(text.ToString()))
				{
					return;
				}
			}
			this._variationList.Add(new GameText.GameTextVariation(variationId, text, choiceTags));
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001B29C File Offset: 0x0001949C
		public void SetVariationWithId(string variationId, TextObject text, List<GameTextManager.ChoiceTag> choiceTags)
		{
			for (int i = 0; i < this._variationList.Count; i++)
			{
				if (this._variationList[i].Id.Equals(variationId))
				{
					this._variationList[i] = new GameText.GameTextVariation(variationId, text, choiceTags);
					return;
				}
			}
			this._variationList.Add(new GameText.GameTextVariation(variationId, text, choiceTags));
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001B300 File Offset: 0x00019500
		public void AddVariation(string text, params object[] propertiesAndWeights)
		{
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			for (int i = 0; i < propertiesAndWeights.Length; i += 2)
			{
				string text2 = (string)propertiesAndWeights[i];
				int num = Convert.ToInt32(propertiesAndWeights[i + 1]);
				list.Add(new GameTextManager.ChoiceTag(text2, num));
			}
			this.AddVariationWithId("", new TextObject(text, null), list);
		}

		// Token: 0x04000424 RID: 1060
		private readonly List<GameText.GameTextVariation> _variationList;

		// Token: 0x0200011A RID: 282
		public struct GameTextVariation
		{
			// Token: 0x06000C02 RID: 3074 RVA: 0x00026767 File Offset: 0x00024967
			internal GameTextVariation(string id, TextObject text, List<GameTextManager.ChoiceTag> choiceTags)
			{
				this.Id = id;
				this.Text = text;
				this.Tags = choiceTags.ToArray();
			}

			// Token: 0x040007A8 RID: 1960
			public readonly string Id;

			// Token: 0x040007A9 RID: 1961
			public readonly TextObject Text;

			// Token: 0x040007AA RID: 1962
			public readonly GameTextManager.ChoiceTag[] Tags;
		}
	}
}
