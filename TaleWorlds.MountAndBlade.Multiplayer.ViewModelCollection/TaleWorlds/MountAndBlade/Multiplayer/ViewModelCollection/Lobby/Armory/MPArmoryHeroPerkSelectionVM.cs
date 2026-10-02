using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007B RID: 123
	public class MPArmoryHeroPerkSelectionVM : ViewModel
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00026357 File Offset: 0x00024557
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x0002635F File Offset: 0x0002455F
		public MultiplayerClassDivisions.MPHeroClass CurrentHeroClass { get; private set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00026368 File Offset: 0x00024568
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x00026370 File Offset: 0x00024570
		public List<IReadOnlyPerkObject> CurrentSelectedPerks { get; private set; }

		// Token: 0x06000C61 RID: 3169 RVA: 0x0002637C File Offset: 0x0002457C
		public MPArmoryHeroPerkSelectionVM(Action<HeroPerkVM, MPPerkVM> onPerkSelection, Action forceRefreshCharacter)
		{
			this._onPerkSelection = onPerkSelection;
			this._forceRefreshCharacter = forceRefreshCharacter;
			this.Perks = new MBBindingList<HeroPerkVM>();
			this.GameModes = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnGameModeSelectionChanged));
			foreach (string text in this._availableGameModes)
			{
				this.GameModes.AddItem(new SelectorItemVM(GameTexts.FindText("str_multiplayer_official_game_type_name", text)));
			}
			this.GameModes.SelectedIndex = 0;
			this.RefreshValues();
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00026470 File Offset: 0x00024670
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.GameModes.RefreshValues();
			this.GameModes.SelectedIndex = 0;
			this.Perks.ApplyActionOnAllItems(delegate(HeroPerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000264C4 File Offset: 0x000246C4
		public void RefreshPerksListWithHero(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			this.Perks = new MBBindingList<HeroPerkVM>();
			this.CurrentHeroClass = heroClass;
			MBBindingList<HeroPerkVM> mbbindingList = new MBBindingList<HeroPerkVM>();
			List<HeroPerkVM> list = new List<HeroPerkVM>();
			List<List<IReadOnlyPerkObject>> allPerksForHeroClass = MultiplayerClassDivisions.GetAllPerksForHeroClass(this.CurrentHeroClass, this._availableGameModes[this.GameModes.SelectedIndex]);
			for (int i = 0; i < allPerksForHeroClass.Count; i++)
			{
				if (allPerksForHeroClass[i].Count > 0)
				{
					IReadOnlyPerkObject readOnlyPerkObject = allPerksForHeroClass[i][0];
					HeroPerkVM heroPerkVM = new HeroPerkVM(new Action<HeroPerkVM, MPPerkVM>(this.OnPerkSelection), readOnlyPerkObject, allPerksForHeroClass[i], i);
					mbbindingList.Add(heroPerkVM);
					list.Add(heroPerkVM);
				}
			}
			this.Perks = mbbindingList;
			if (this.CurrentSelectedPerks == null)
			{
				this.CurrentSelectedPerks = new List<IReadOnlyPerkObject>();
			}
			else
			{
				this.CurrentSelectedPerks.Clear();
			}
			foreach (HeroPerkVM heroPerkVM2 in list)
			{
				this.OnPerkSelection(heroPerkVM2, heroPerkVM2.SelectedPerkItem);
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000265E0 File Offset: 0x000247E0
		private void OnGameModeSelectionChanged(SelectorVM<SelectorItemVM> selector)
		{
			if (this.GameModes.SelectedIndex == -1)
			{
				this.GameModes.SelectedIndex = 0;
			}
			if (this.CurrentHeroClass != null)
			{
				this.RefreshPerksListWithHero(this.CurrentHeroClass);
				Action forceRefreshCharacter = this._forceRefreshCharacter;
				if (forceRefreshCharacter == null)
				{
					return;
				}
				forceRefreshCharacter();
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00026620 File Offset: 0x00024820
		private void OnPerkSelection(HeroPerkVM heroPerk, MPPerkVM candidate)
		{
			this.CurrentSelectedPerks = this.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>();
			Action<HeroPerkVM, MPPerkVM> onPerkSelection = this._onPerkSelection;
			if (onPerkSelection == null)
			{
				return;
			}
			onPerkSelection(heroPerk, candidate);
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00026674 File Offset: 0x00024874
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x0002667C File Offset: 0x0002487C
		[DataSourceProperty]
		public MBBindingList<HeroPerkVM> Perks
		{
			get
			{
				return this._perks;
			}
			set
			{
				if (value != this._perks)
				{
					this._perks = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroPerkVM>>(value, "Perks");
				}
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0002669A File Offset: 0x0002489A
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x000266A2 File Offset: 0x000248A2
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> GameModes
		{
			get
			{
				return this._gameModes;
			}
			set
			{
				if (value != this._gameModes)
				{
					this._gameModes = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "GameModes");
				}
			}
		}

		// Token: 0x0400059D RID: 1437
		private readonly Action<HeroPerkVM, MPPerkVM> _onPerkSelection;

		// Token: 0x0400059E RID: 1438
		private readonly Action _forceRefreshCharacter;

		// Token: 0x0400059F RID: 1439
		private List<string> _availableGameModes = new List<string> { "Skirmish", "Captain", "Siege", "TeamDeathmatch", "Duel" };

		// Token: 0x040005A0 RID: 1440
		private MBBindingList<HeroPerkVM> _perks;

		// Token: 0x040005A1 RID: 1441
		private SelectorVM<SelectorItemVM> _gameModes;
	}
}
