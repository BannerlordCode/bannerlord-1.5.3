using System;
using System.Linq;
using SandBox.View.Map.Navigation.NavigationElements;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SandBox.View.Map.Navigation
{
	// Token: 0x0200006A RID: 106
	public class MapNavigationHandler : INavigationHandler
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x00024FAF File Offset: 0x000231AF
		public INavigationElement[] GetElements()
		{
			return this._elements;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00024FB7 File Offset: 0x000231B7
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x00024FBF File Offset: 0x000231BF
		public bool IsNavigationLocked { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00024FC8 File Offset: 0x000231C8
		public bool IsEscapeMenuActive
		{
			get
			{
				return this._elements.Any<INavigationElement>(delegate(INavigationElement e)
				{
					EscapeMenuNavigationElement escapeMenuNavigationElement;
					return (escapeMenuNavigationElement = e as EscapeMenuNavigationElement) != null && escapeMenuNavigationElement.IsActive;
				});
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00024FF4 File Offset: 0x000231F4
		public MapNavigationHandler()
		{
			this._game = Game.Current;
			this._elements = this.OnCreateElements();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00025014 File Offset: 0x00023214
		public bool IsAnyElementActive()
		{
			for (int i = 0; i < this._elements.Length; i++)
			{
				if (this._elements[i].IsActive)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00025048 File Offset: 0x00023248
		protected virtual INavigationElement[] OnCreateElements()
		{
			return new INavigationElement[]
			{
				new EscapeMenuNavigationElement(this),
				new CharacterDeveloperNavigationElement(this),
				new InventoryNavigationElement(this),
				new PartyNavigationElement(this),
				new QuestsNavigationElement(this),
				new ClanNavigationElement(this),
				new KingdomNavigationElement(this)
			};
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0002509C File Offset: 0x0002329C
		public INavigationElement GetElement(string id)
		{
			for (int i = 0; i < this._elements.Length; i++)
			{
				if (this._elements[i].StringId == id)
				{
					return this._elements[i];
				}
			}
			return null;
		}

		// Token: 0x04000239 RID: 569
		protected readonly Game _game;

		// Token: 0x0400023A RID: 570
		private INavigationElement[] _elements;
	}
}
