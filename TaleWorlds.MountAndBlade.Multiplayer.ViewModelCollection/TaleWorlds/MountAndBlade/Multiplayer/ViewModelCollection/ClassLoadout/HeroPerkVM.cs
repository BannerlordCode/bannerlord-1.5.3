using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AA RID: 170
	public class HeroPerkVM : ViewModel
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x000331F6 File Offset: 0x000313F6
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x000331FE File Offset: 0x000313FE
		public IReadOnlyPerkObject SelectedPerk { get; private set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x00033207 File Offset: 0x00031407
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x0003320F File Offset: 0x0003140F
		public MPPerkVM SelectedPerkItem { get; private set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00033218 File Offset: 0x00031418
		public int PerkIndex { get; }

		// Token: 0x0600106E RID: 4206 RVA: 0x00033220 File Offset: 0x00031420
		public HeroPerkVM(Action<HeroPerkVM, MPPerkVM> onSelectPerk, IReadOnlyPerkObject perk, List<IReadOnlyPerkObject> candidatePerks, int perkIndex)
		{
			HeroPerkVM <>4__this = this;
			this.Hint = new BasicTooltipViewModel(() => <>4__this.SelectedPerkItem.Description);
			this.CandidatePerks = new MBBindingList<MPPerkVM>();
			this.PerkIndex = perkIndex;
			this._onSelectPerk = onSelectPerk;
			for (int i = 0; i < candidatePerks.Count; i++)
			{
				IReadOnlyPerkObject readOnlyPerkObject = candidatePerks[i];
				bool flag = readOnlyPerkObject != perk;
				this.CandidatePerks.Add(new MPPerkVM(new Action<MPPerkVM>(this.OnSelectPerk), readOnlyPerkObject, flag, i));
			}
			this.OnSelectPerk(this.CandidatePerks.SingleOrDefault<MPPerkVM>((MPPerkVM x) => x.Perk == perk));
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000332E8 File Offset: 0x000314E8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.SelectedPerkItem.Name;
			this.CandidatePerks.ApplyActionOnAllItems(delegate(MPPerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00033338 File Offset: 0x00031538
		[UsedImplicitly]
		private void OnSelectPerk(MPPerkVM perkVm)
		{
			this.OnRefreshWithPerk(perkVm);
			foreach (MPPerkVM mpperkVM in this.CandidatePerks)
			{
				mpperkVM.IsSelectable = true;
			}
			perkVm.IsSelectable = false;
			this._onSelectPerk(this, perkVm);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000333A0 File Offset: 0x000315A0
		private void OnRefreshWithPerk(MPPerkVM perk)
		{
			this.SelectedPerkItem = perk;
			MPPerkVM selectedPerkItem = this.SelectedPerkItem;
			this.SelectedPerk = ((selectedPerkItem != null) ? selectedPerkItem.Perk : null);
			if (perk == null)
			{
				this.Name = "";
				this.IconType = "";
				return;
			}
			this.IconType = perk.IconType;
			this.RefreshValues();
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x000333F8 File Offset: 0x000315F8
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x00033400 File Offset: 0x00031600
		[DataSourceProperty]
		public MBBindingList<MPPerkVM> CandidatePerks
		{
			get
			{
				return this._candidatePerks;
			}
			set
			{
				if (value != this._candidatePerks)
				{
					this._candidatePerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPerkVM>>(value, "CandidatePerks");
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0003341E File Offset: 0x0003161E
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x00033426 File Offset: 0x00031626
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x00033449 File Offset: 0x00031649
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x00033451 File Offset: 0x00031651
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x00033474 File Offset: 0x00031674
		// (set) Token: 0x06001079 RID: 4217 RVA: 0x0003347C File Offset: 0x0003167C
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040007B6 RID: 1974
		private readonly Action<HeroPerkVM, MPPerkVM> _onSelectPerk;

		// Token: 0x040007B8 RID: 1976
		private string _name = "";

		// Token: 0x040007B9 RID: 1977
		private string _iconType;

		// Token: 0x040007BA RID: 1978
		private BasicTooltipViewModel _hint;

		// Token: 0x040007BB RID: 1979
		private MBBindingList<MPPerkVM> _candidatePerks;
	}
}
