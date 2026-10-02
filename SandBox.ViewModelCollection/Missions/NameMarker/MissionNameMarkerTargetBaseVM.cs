using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000033 RID: 51
	public abstract class MissionNameMarkerTargetBaseVM : ViewModel
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x00010BEE File Offset: 0x0000EDEE
		public MissionNameMarkerTargetBaseVM()
		{
			this.Quests = new MBBindingList<QuestMarkerVM>();
		}

		// Token: 0x060003E8 RID: 1000
		public abstract void UpdatePosition(Camera missionCamera);

		// Token: 0x060003E9 RID: 1001
		public abstract bool Equals(MissionNameMarkerTargetBaseVM other);

		// Token: 0x060003EA RID: 1002
		protected abstract TextObject GetName();

		// Token: 0x060003EB RID: 1003 RVA: 0x00010C17 File Offset: 0x0000EE17
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.GetName().ToString();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010C30 File Offset: 0x0000EE30
		protected void UpdatePositionWith(Camera missionCamera, Vec3 worldPosition)
		{
			float num = -100f;
			float num2 = -100f;
			float num3 = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, worldPosition, ref num, ref num2, ref num3);
			if (num3 > 0f)
			{
				this.ScreenPosition = new Vec2(num, num2);
				this.Distance = (int)(worldPosition - missionCamera.Position).Length;
				return;
			}
			this.Distance = -1;
			this.ScreenPosition = new Vec2(-500f, -500f);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010CAA File Offset: 0x0000EEAA
		public void SetEnabledState(bool enabled)
		{
			this.IsEnabled = this.IsPersistent || enabled;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00010CBA File Offset: 0x0000EEBA
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00010CC2 File Offset: 0x0000EEC2
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00010D23 File Offset: 0x0000EF23
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00010D2B File Offset: 0x0000EF2B
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00010D4E File Offset: 0x0000EF4E
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00010D56 File Offset: 0x0000EF56
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00010D79 File Offset: 0x0000EF79
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00010D81 File Offset: 0x0000EF81
		[DataSourceProperty]
		public string NameType
		{
			get
			{
				return this._nameType;
			}
			set
			{
				if (value != this._nameType)
				{
					this._nameType = value;
					base.OnPropertyChangedWithValue<string>(value, "NameType");
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00010DAC File Offset: 0x0000EFAC
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00010DCA File Offset: 0x0000EFCA
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00010DD2 File Offset: 0x0000EFD2
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00010DF0 File Offset: 0x0000EFF0
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		[DataSourceProperty]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChangedWithValue(value, "IsTracked");
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00010E16 File Offset: 0x0000F016
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x00010E1E File Offset: 0x0000F01E
		[DataSourceProperty]
		public bool IsQuestMainStory
		{
			get
			{
				return this._isQuestMainStory;
			}
			set
			{
				if (value != this._isQuestMainStory)
				{
					this._isQuestMainStory = value;
					base.OnPropertyChangedWithValue(value, "IsQuestMainStory");
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00010E3C File Offset: 0x0000F03C
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00010E44 File Offset: 0x0000F044
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (value != this._isEnemy)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00010E62 File Offset: 0x0000F062
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00010E6A File Offset: 0x0000F06A
		[DataSourceProperty]
		public bool IsFriendly
		{
			get
			{
				return this._isFriendly;
			}
			set
			{
				if (value != this._isFriendly)
				{
					this._isFriendly = value;
					base.OnPropertyChangedWithValue(value, "IsFriendly");
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00010E88 File Offset: 0x0000F088
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00010E90 File Offset: 0x0000F090
		[DataSourceProperty]
		public bool IsPersistent
		{
			get
			{
				return this._isPersistent;
			}
			set
			{
				if (value != this._isPersistent)
				{
					this._isPersistent = value;
					base.OnPropertyChangedWithValue(value, "IsPersistent");
					if (this.IsPersistent)
					{
						this.SetEnabledState(true);
						return;
					}
					if (!this.IsEnabled)
					{
						this.SetEnabledState(false);
					}
				}
			}
		}

		// Token: 0x0400020E RID: 526
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x0400020F RID: 527
		private Vec2 _screenPosition;

		// Token: 0x04000210 RID: 528
		private int _distance;

		// Token: 0x04000211 RID: 529
		private string _name;

		// Token: 0x04000212 RID: 530
		private string _iconType = string.Empty;

		// Token: 0x04000213 RID: 531
		private string _nameType = string.Empty;

		// Token: 0x04000214 RID: 532
		private bool _isEnabled;

		// Token: 0x04000215 RID: 533
		private bool _isTracked;

		// Token: 0x04000216 RID: 534
		private bool _isQuestMainStory;

		// Token: 0x04000217 RID: 535
		private bool _isEnemy;

		// Token: 0x04000218 RID: 536
		private bool _isFriendly;

		// Token: 0x04000219 RID: 537
		private bool _isPersistent;
	}
}
