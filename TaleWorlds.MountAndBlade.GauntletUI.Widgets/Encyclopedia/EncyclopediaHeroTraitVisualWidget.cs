using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015D RID: 349
	public class EncyclopediaHeroTraitVisualWidget : Widget
	{
		// Token: 0x0600129C RID: 4764 RVA: 0x000337BB File Offset: 0x000319BB
		public EncyclopediaHeroTraitVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000337C4 File Offset: 0x000319C4
		private void SetVisual(string traitCode, int value)
		{
			if (!string.IsNullOrEmpty(traitCode))
			{
				string text = string.Concat(new object[]
				{
					"SPGeneral\\SPTraits\\",
					traitCode.ToLower(),
					"_",
					value
				});
				base.Sprite = base.Context.SpriteData.GetSprite(text);
				base.Sprite = base.Context.SpriteData.GetSprite(text);
				if (value < 0)
				{
					base.Color = new Color(0.738f, 0.113f, 0.113f, 1f);
					return;
				}
				base.Color = new Color(0.992f, 0.75f, 0.33f, 1f);
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0003387B File Offset: 0x00031A7B
		// (set) Token: 0x0600129F RID: 4767 RVA: 0x00033883 File Offset: 0x00031A83
		[Editor(false)]
		public string TraitId
		{
			get
			{
				return this._traitId;
			}
			set
			{
				if (this._traitId != value)
				{
					this._traitId = value;
					base.OnPropertyChanged<string>(value, "TraitId");
					this.SetVisual(value, this.TraitValue);
				}
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x000338B3 File Offset: 0x00031AB3
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x000338BB File Offset: 0x00031ABB
		[Editor(false)]
		public int TraitValue
		{
			get
			{
				return this._traitValue;
			}
			set
			{
				if (this._traitValue != value)
				{
					this._traitValue = value;
					base.OnPropertyChanged(value, "TraitValue");
					this.SetVisual(this.TraitId, value);
				}
			}
		}

		// Token: 0x0400087E RID: 2174
		private string _traitId;

		// Token: 0x0400087F RID: 2175
		private int _traitValue;
	}
}
