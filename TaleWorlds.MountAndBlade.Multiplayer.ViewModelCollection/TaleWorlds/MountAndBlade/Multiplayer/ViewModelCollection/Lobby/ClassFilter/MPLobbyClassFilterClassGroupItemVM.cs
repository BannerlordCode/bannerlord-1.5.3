using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter
{
	// Token: 0x02000063 RID: 99
	public class MPLobbyClassFilterClassGroupItemVM : ViewModel
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0001E705 File Offset: 0x0001C905
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0001E70D File Offset: 0x0001C90D
		public MultiplayerClassDivisions.MPHeroClassGroup ClassGroup { get; set; }

		// Token: 0x060009B6 RID: 2486 RVA: 0x0001E716 File Offset: 0x0001C916
		public MPLobbyClassFilterClassGroupItemVM(MultiplayerClassDivisions.MPHeroClassGroup classGroup)
		{
			this.ClassGroup = classGroup;
			this.Classes = new MBBindingList<MPLobbyClassFilterClassItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001E738 File Offset: 0x0001C938
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.ClassGroup.Name.ToString();
			this.Classes.ApplyActionOnAllItems(delegate(MPLobbyClassFilterClassItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001E78B File Offset: 0x0001C98B
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ClassGroup = null;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001E79C File Offset: 0x0001C99C
		public void AddClass(BasicCultureObject culture, MultiplayerClassDivisions.MPHeroClass heroClass, Action<MPLobbyClassFilterClassItemVM> onSelect)
		{
			MPLobbyClassFilterClassItemVM mplobbyClassFilterClassItemVM = new MPLobbyClassFilterClassItemVM(culture, heroClass, onSelect);
			this.Classes.Add(mplobbyClassFilterClassItemVM);
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0001E7BE File Offset: 0x0001C9BE
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0001E7C6 File Offset: 0x0001C9C6
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

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001E7E9 File Offset: 0x0001C9E9
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0001E7F1 File Offset: 0x0001C9F1
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterClassItemVM> Classes
		{
			get
			{
				return this._classes;
			}
			set
			{
				if (value != this._classes)
				{
					this._classes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterClassItemVM>>(value, "Classes");
				}
			}
		}

		// Token: 0x04000478 RID: 1144
		private string _name;

		// Token: 0x04000479 RID: 1145
		private MBBindingList<MPLobbyClassFilterClassItemVM> _classes;
	}
}
