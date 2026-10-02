using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EB RID: 235
	public class EncyclopediaFactionVM : ViewModel
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x00056002 File Offset: 0x00054202
		// (set) Token: 0x060015C0 RID: 5568 RVA: 0x0005600A File Offset: 0x0005420A
		public IFaction Faction { get; private set; }

		// Token: 0x060015C1 RID: 5569 RVA: 0x00056014 File Offset: 0x00054214
		public EncyclopediaFactionVM(IFaction faction)
		{
			this.Faction = faction;
			if (faction != null)
			{
				this.ImageIdentifier = new BannerImageIdentifierVM(faction.Banner, true);
				this.IsDestroyed = faction.IsEliminated;
			}
			else
			{
				this.ImageIdentifier = new BannerImageIdentifierVM(null, false);
				this.IsDestroyed = false;
			}
			this.RefreshValues();
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0005606B File Offset: 0x0005426B
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Faction != null)
			{
				this.NameText = this.Faction.Name.ToString();
				return;
			}
			this.NameText = new TextObject("{=2abtb4xu}Independent", null).ToString();
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x000560A8 File Offset: 0x000542A8
		public void ExecuteLink()
		{
			if (this.Faction != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Faction.EncyclopediaLink);
			}
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x000560CC File Offset: 0x000542CC
		public void ExecuteBeginHint()
		{
			if (this.Faction is Clan)
			{
				InformationManager.ShowTooltip(typeof(Clan), new object[] { this.Faction });
				return;
			}
			if (this.Faction is Kingdom)
			{
				InformationManager.ShowTooltip(typeof(Kingdom), new object[] { this.Faction });
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00056130 File Offset: 0x00054330
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x00056137 File Offset: 0x00054337
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x0005613F File Offset: 0x0005433F
		[DataSourceProperty]
		public BannerImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChanged("Banner");
				}
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x0005615C File Offset: 0x0005435C
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00056164 File Offset: 0x00054364
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00056187 File Offset: 0x00054387
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x0005618F File Offset: 0x0005438F
		[DataSourceProperty]
		public bool IsDestroyed
		{
			get
			{
				return this._isDestroyed;
			}
			set
			{
				if (value != this._isDestroyed)
				{
					this._isDestroyed = value;
					base.OnPropertyChangedWithValue(value, "IsDestroyed");
				}
			}
		}

		// Token: 0x040009DB RID: 2523
		private BannerImageIdentifierVM _imageIdentifier;

		// Token: 0x040009DC RID: 2524
		private string _nameText;

		// Token: 0x040009DD RID: 2525
		private bool _isDestroyed;
	}
}
