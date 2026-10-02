using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A7 RID: 167
	public class HeroClassGroupVM : ViewModel
	{
		// Token: 0x06001010 RID: 4112 RVA: 0x0003221C File Offset: 0x0003041C
		public HeroClassGroupVM(Action<HeroClassVM> onSelect, Action<HeroPerkVM, MPPerkVM> onPerkSelect, MultiplayerClassDivisions.MPHeroClassGroup heroClassGroup, MultiplayerBattleColors.MultiplayerCultureColorInfo colorInfo)
		{
			this.HeroClassGroup = heroClassGroup;
			this._onPerkSelect = onPerkSelect;
			this.IconType = heroClassGroup.StringId;
			this.SubClasses = new MBBindingList<HeroClassVM>();
			Team team = GameNetwork.MyPeer.GetComponent<MissionPeer>().Team;
			IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses(GameNetwork.MyPeer.GetComponent<MissionPeer>().Culture);
			Func<MultiplayerClassDivisions.MPHeroClass, bool> <>9__0;
			Func<MultiplayerClassDivisions.MPHeroClass, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (MultiplayerClassDivisions.MPHeroClass h) => h.ClassGroup.Equals(heroClassGroup));
			}
			foreach (MultiplayerClassDivisions.MPHeroClass mpheroClass in mpheroClasses.Where<MultiplayerClassDivisions.MPHeroClass>(func))
			{
				this.SubClasses.Add(new HeroClassVM(onSelect, this._onPerkSelect, mpheroClass, colorInfo));
			}
			this.RefreshValues();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00032308 File Offset: 0x00030508
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroClassGroup.Name.ToString();
			this.SubClasses.ApplyActionOnAllItems(delegate(HeroClassVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x0003235B File Offset: 0x0003055B
		public bool IsValid
		{
			get
			{
				return this.SubClasses.Count > 0;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0003236B File Offset: 0x0003056B
		// (set) Token: 0x06001014 RID: 4116 RVA: 0x00032373 File Offset: 0x00030573
		[DataSourceProperty]
		public MBBindingList<HeroClassVM> SubClasses
		{
			get
			{
				return this._subClasses;
			}
			set
			{
				if (value != this._subClasses)
				{
					this._subClasses = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroClassVM>>(value, "SubClasses");
				}
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x00032391 File Offset: 0x00030591
		// (set) Token: 0x06001016 RID: 4118 RVA: 0x00032399 File Offset: 0x00030599
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

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x000323BC File Offset: 0x000305BC
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x000323C4 File Offset: 0x000305C4
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
					this.IconPath = "TroopBanners\\ClassType_" + value;
				}
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x000323F8 File Offset: 0x000305F8
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x00032400 File Offset: 0x00030600
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChangedWithValue<string>(value, "IconPath");
				}
			}
		}

		// Token: 0x04000787 RID: 1927
		public readonly MultiplayerClassDivisions.MPHeroClassGroup HeroClassGroup;

		// Token: 0x04000788 RID: 1928
		private readonly Action<HeroPerkVM, MPPerkVM> _onPerkSelect;

		// Token: 0x04000789 RID: 1929
		private string _name;

		// Token: 0x0400078A RID: 1930
		private string _iconType;

		// Token: 0x0400078B RID: 1931
		private string _iconPath;

		// Token: 0x0400078C RID: 1932
		private MBBindingList<HeroClassVM> _subClasses;
	}
}
