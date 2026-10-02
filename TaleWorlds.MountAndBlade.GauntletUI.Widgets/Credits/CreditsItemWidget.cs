using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Credits
{
	// Token: 0x02000163 RID: 355
	public class CreditsItemWidget : Widget
	{
		// Token: 0x060012DB RID: 4827 RVA: 0x0003410F File Offset: 0x0003230F
		public CreditsItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00034118 File Offset: 0x00032318
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.RefreshItemWidget();
				this._initialized = true;
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00034138 File Offset: 0x00032338
		private void RefreshItemWidget()
		{
			if (!string.IsNullOrEmpty(this.ItemType))
			{
				if (this.CategoryWidget != null)
				{
					this.CategoryWidget.IsVisible = this.ItemType == "Category";
				}
				if (this.SectionWidget != null)
				{
					this.SectionWidget.IsVisible = this.ItemType == "Section";
				}
				if (this.EntryWidget != null)
				{
					this.EntryWidget.IsVisible = this.ItemType == "Entry";
				}
				if (this.EmptyLineWidget != null)
				{
					this.EmptyLineWidget.IsVisible = this.ItemType == "EmptyLine";
				}
				if (this.ImageWidget != null)
				{
					this.ImageWidget.IsVisible = this.ItemType == "Image";
					if (this.ImageWidget.Sprite != null)
					{
						this.ImageWidget.SuggestedWidth = (float)this.ImageWidget.Sprite.Width;
						this.ImageWidget.SuggestedHeight = (float)this.ImageWidget.Sprite.Height;
					}
				}
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x00034249 File Offset: 0x00032449
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x00034251 File Offset: 0x00032451
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
				}
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00034274 File Offset: 0x00032474
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0003427C File Offset: 0x0003247C
		[Editor(false)]
		public Widget CategoryWidget
		{
			get
			{
				return this._categoryWidget;
			}
			set
			{
				if (this._categoryWidget != value)
				{
					this._categoryWidget = value;
					base.OnPropertyChanged<Widget>(value, "CategoryWidget");
				}
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0003429A File Offset: 0x0003249A
		// (set) Token: 0x060012E3 RID: 4835 RVA: 0x000342A2 File Offset: 0x000324A2
		[Editor(false)]
		public Widget ImageWidget
		{
			get
			{
				return this._imageWidget;
			}
			set
			{
				if (this._imageWidget != value)
				{
					this._imageWidget = value;
					base.OnPropertyChanged<Widget>(value, "ImageWidget");
				}
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x000342C0 File Offset: 0x000324C0
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x000342C8 File Offset: 0x000324C8
		[Editor(false)]
		public Widget SectionWidget
		{
			get
			{
				return this._sectionWidget;
			}
			set
			{
				if (this._sectionWidget != value)
				{
					this._sectionWidget = value;
					base.OnPropertyChanged<Widget>(value, "SectionWidget");
				}
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x000342E6 File Offset: 0x000324E6
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x000342EE File Offset: 0x000324EE
		[Editor(false)]
		public Widget EntryWidget
		{
			get
			{
				return this._entryWidget;
			}
			set
			{
				if (this._entryWidget != value)
				{
					this._entryWidget = value;
					base.OnPropertyChanged<Widget>(value, "EntryWidget");
				}
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0003430C File Offset: 0x0003250C
		// (set) Token: 0x060012E9 RID: 4841 RVA: 0x00034314 File Offset: 0x00032514
		[Editor(false)]
		public Widget EmptyLineWidget
		{
			get
			{
				return this._emptyLineWidget;
			}
			set
			{
				if (this._emptyLineWidget != value)
				{
					this._emptyLineWidget = value;
					base.OnPropertyChanged<Widget>(value, "EmptyLineWidget");
				}
			}
		}

		// Token: 0x04000898 RID: 2200
		private bool _initialized;

		// Token: 0x04000899 RID: 2201
		private string _itemType;

		// Token: 0x0400089A RID: 2202
		private Widget _categoryWidget;

		// Token: 0x0400089B RID: 2203
		private Widget _sectionWidget;

		// Token: 0x0400089C RID: 2204
		private Widget _entryWidget;

		// Token: 0x0400089D RID: 2205
		private Widget _emptyLineWidget;

		// Token: 0x0400089E RID: 2206
		private Widget _imageWidget;
	}
}
