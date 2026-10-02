using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameMenu
{
	// Token: 0x02000158 RID: 344
	public class SettlementMenuPartyCharacterListsButtonWidget : ButtonWidget
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00033325 File Offset: 0x00031525
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x0003332D File Offset: 0x0003152D
		public Brush PartyListButtonBrush { get; set; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x00033336 File Offset: 0x00031536
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x0003333E File Offset: 0x0003153E
		public Brush CharacterListButtonBrush { get; set; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00033347 File Offset: 0x00031547
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x0003334F File Offset: 0x0003154F
		public ContainerPageControlWidget CharactersList { get; set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x00033358 File Offset: 0x00031558
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x00033360 File Offset: 0x00031560
		public ContainerPageControlWidget PartiesList { get; set; }

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x00033369 File Offset: 0x00031569
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x00033371 File Offset: 0x00031571
		public int MaxNumOfVisuals { get; set; } = 5;

		// Token: 0x0600127C RID: 4732 RVA: 0x0003337A File Offset: 0x0003157A
		public SettlementMenuPartyCharacterListsButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0003338C File Offset: 0x0003158C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.Brush = (this.ChildPartiesList.IsVisible ? this.PartyListButtonBrush : (this.ChildCharactersList.IsVisible ? this.CharacterListButtonBrush : null));
			if (!this._initialized)
			{
				if (this.CharactersList.IsVisible)
				{
					this.SetCharacterListVisible();
				}
				else if (this.PartiesList.IsVisible)
				{
					this.SetPartyListVisible();
				}
				this._initialized = true;
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00033408 File Offset: 0x00031608
		protected override void HandleClick()
		{
			base.HandleClick();
			if (!this.PartiesList.IsVisible && this.CharactersList.IsVisible)
			{
				this.SetPartyListVisible();
				return;
			}
			if (this.PartiesList.IsVisible && !this.CharactersList.IsVisible)
			{
				this.SetCharacterListVisible();
			}
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0003345C File Offset: 0x0003165C
		private void SetCharacterListVisible()
		{
			this.CharactersList.IsVisible = true;
			this.PartiesList.IsVisible = false;
			this.ChildPartiesList.IsVisible = true;
			this.ChildCharactersList.IsVisible = false;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0003348E File Offset: 0x0003168E
		private void SetPartyListVisible()
		{
			this.CharactersList.IsVisible = false;
			this.PartiesList.IsVisible = true;
			this.ChildPartiesList.IsVisible = false;
			this.ChildCharactersList.IsVisible = true;
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x000334C0 File Offset: 0x000316C0
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x000334C8 File Offset: 0x000316C8
		public ListPanel ChildCharactersList
		{
			get
			{
				return this._childCharactersList;
			}
			set
			{
				if (value != this._childCharactersList)
				{
					this._childCharactersList = value;
					this._childCharactersList.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListItemAdded));
				}
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x000334F6 File Offset: 0x000316F6
		// (set) Token: 0x06001284 RID: 4740 RVA: 0x000334FE File Offset: 0x000316FE
		public ListPanel ChildPartiesList
		{
			get
			{
				return this._childPartiesList;
			}
			set
			{
				if (value != this._childPartiesList)
				{
					this._childPartiesList = value;
					this._childPartiesList.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListItemAdded));
				}
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0003352C File Offset: 0x0003172C
		private void OnListItemAdded(Widget parent, Widget child)
		{
			if (parent.ChildCount > this.MaxNumOfVisuals)
			{
				child.IsVisible = false;
			}
		}

		// Token: 0x04000876 RID: 2166
		private bool _initialized;

		// Token: 0x04000877 RID: 2167
		private ListPanel _childCharactersList;

		// Token: 0x04000878 RID: 2168
		private ListPanel _childPartiesList;
	}
}
