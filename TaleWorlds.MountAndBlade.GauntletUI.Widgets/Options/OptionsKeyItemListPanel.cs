using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000078 RID: 120
	public class OptionsKeyItemListPanel : ListPanel
	{
		// Token: 0x06000696 RID: 1686 RVA: 0x00013768 File Offset: 0x00011968
		public OptionsKeyItemListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00013771 File Offset: 0x00011971
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this._screenWidget = this.FindScreenWidget(base.ParentWidget);
				this._initialized = true;
			}
			if (!this._eventsRegistered)
			{
				this.RegisterHoverEvents();
				this._eventsRegistered = true;
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000137B0 File Offset: 0x000119B0
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.SetCurrentOption(false, false, -1);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000137C1 File Offset: 0x000119C1
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.ResetCurrentOption();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000137D0 File Offset: 0x000119D0
		private OptionsScreenWidget FindScreenWidget(Widget parent)
		{
			OptionsScreenWidget optionsScreenWidget;
			if ((optionsScreenWidget = parent as OptionsScreenWidget) != null)
			{
				return optionsScreenWidget;
			}
			if (parent == null)
			{
				return null;
			}
			return this.FindScreenWidget(parent.ParentWidget);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000137FA File Offset: 0x000119FA
		private void SetCurrentOption(bool fromHoverOverDropdown, bool fromBooleanSelection, int hoverDropdownItemIndex = -1)
		{
			OptionsScreenWidget screenWidget = this._screenWidget;
			if (screenWidget == null)
			{
				return;
			}
			screenWidget.SetCurrentOption(this, null);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001380E File Offset: 0x00011A0E
		private void ResetCurrentOption()
		{
			OptionsScreenWidget screenWidget = this._screenWidget;
			if (screenWidget == null)
			{
				return;
			}
			screenWidget.SetCurrentOption(null, null);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00013824 File Offset: 0x00011A24
		private void RegisterHoverEvents()
		{
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				allChildrenRecursive[i].boolPropertyChanged += this.Child_PropertyChanged;
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00013862 File Offset: 0x00011A62
		private void Child_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				if (propertyValue)
				{
					this.SetCurrentOption(false, false, -1);
					return;
				}
				this.ResetCurrentOption();
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00013884 File Offset: 0x00011A84
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0001388C File Offset: 0x00011A8C
		public string OptionTitle
		{
			get
			{
				return this._optionTitle;
			}
			set
			{
				if (this._optionTitle != value)
				{
					this._optionTitle = value;
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x000138A3 File Offset: 0x00011AA3
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x000138AB File Offset: 0x00011AAB
		public string OptionDescription
		{
			get
			{
				return this._optionDescription;
			}
			set
			{
				if (this._optionDescription != value)
				{
					this._optionDescription = value;
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x000138C2 File Offset: 0x00011AC2
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x000138CA File Offset: 0x00011ACA
		public string OptionExtraInformation
		{
			get
			{
				return this._optionExtraInformation;
			}
			set
			{
				if (this._optionExtraInformation != value)
				{
					this._optionExtraInformation = value;
				}
			}
		}

		// Token: 0x040002CE RID: 718
		private OptionsScreenWidget _screenWidget;

		// Token: 0x040002CF RID: 719
		private bool _eventsRegistered;

		// Token: 0x040002D0 RID: 720
		private bool _initialized;

		// Token: 0x040002D1 RID: 721
		private string _optionDescription;

		// Token: 0x040002D2 RID: 722
		private string _optionTitle;

		// Token: 0x040002D3 RID: 723
		private string _optionExtraInformation;
	}
}
