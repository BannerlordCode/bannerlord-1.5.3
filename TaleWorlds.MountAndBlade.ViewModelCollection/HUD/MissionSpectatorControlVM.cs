using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000058 RID: 88
	public class MissionSpectatorControlVM : ViewModel
	{
		// Token: 0x0600071C RID: 1820 RVA: 0x00019D0A File Offset: 0x00017F0A
		public MissionSpectatorControlVM(Mission mission)
		{
			this._mission = mission;
			this.RefreshValues();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00019D30 File Offset: 0x00017F30
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PrevCharacterText = new TextObject("{=BANC61K5}Previous Character", null).ToString();
			this.NextCharacterText = new TextObject("{=znKxunbQ}Next Character", null).ToString();
			this.TakeControlText = new TextObject("{=TGpbi44D}Take Control of Character", null).ToString();
			this.UpdateStatusText();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00019D8B File Offset: 0x00017F8B
		public void OnSpectatedAgentFocusIn(Agent followedAgent)
		{
			MissionPeer missionPeer = followedAgent.MissionPeer;
			this.SpectatedAgentName = ((missionPeer != null) ? missionPeer.DisplayedName : null) ?? followedAgent.Name;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00019DAF File Offset: 0x00017FAF
		public void OnSpectatedAgentFocusOut(Agent followedAgent)
		{
			this.SpectatedAgentName = "";
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00019DBC File Offset: 0x00017FBC
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM prevCharacterKey = this.PrevCharacterKey;
			if (prevCharacterKey != null)
			{
				prevCharacterKey.OnFinalize();
			}
			InputKeyItemVM nextCharacterKey = this.NextCharacterKey;
			if (nextCharacterKey != null)
			{
				nextCharacterKey.OnFinalize();
			}
			InputKeyItemVM takeControlKey = this.TakeControlKey;
			if (takeControlKey == null)
			{
				return;
			}
			takeControlKey.OnFinalize();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00019DF6 File Offset: 0x00017FF6
		public void SetMainAgentStatus(bool isDead)
		{
			if (this._isMainHeroDead != isDead)
			{
				this._isMainHeroDead = isDead;
				this.UpdateStatusText();
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00019E0E File Offset: 0x0001800E
		private void UpdateStatusText()
		{
			if (this._isMainHeroDead)
			{
				this.StatusText = this._deadTextObject.ToString();
				return;
			}
			this.StatusText = string.Empty;
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00019E35 File Offset: 0x00018035
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x00019E3D File Offset: 0x0001803D
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00019E5B File Offset: 0x0001805B
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00019E63 File Offset: 0x00018063
		[DataSourceProperty]
		public string PrevCharacterText
		{
			get
			{
				return this._prevCharacterText;
			}
			set
			{
				if (value != this._prevCharacterText)
				{
					this._prevCharacterText = value;
					base.OnPropertyChangedWithValue<string>(value, "PrevCharacterText");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00019E86 File Offset: 0x00018086
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00019E8E File Offset: 0x0001808E
		[DataSourceProperty]
		public string NextCharacterText
		{
			get
			{
				return this._nextCharacterText;
			}
			set
			{
				if (value != this._nextCharacterText)
				{
					this._nextCharacterText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextCharacterText");
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00019EB1 File Offset: 0x000180B1
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00019EB9 File Offset: 0x000180B9
		[DataSourceProperty]
		public string TakeControlText
		{
			get
			{
				return this._takeControlText;
			}
			set
			{
				if (value != this._takeControlText)
				{
					this._takeControlText = value;
					base.OnPropertyChangedWithValue<string>(value, "TakeControlText");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00019EDC File Offset: 0x000180DC
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00019EE4 File Offset: 0x000180E4
		[DataSourceProperty]
		public string StatusText
		{
			get
			{
				return this._statusText;
			}
			set
			{
				if (value != this._statusText)
				{
					this._statusText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatusText");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00019F07 File Offset: 0x00018107
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00019F0F File Offset: 0x0001810F
		[DataSourceProperty]
		public bool IsTakeControlRelevant
		{
			get
			{
				return this._isTakeControlRelevant;
			}
			set
			{
				if (value != this._isTakeControlRelevant)
				{
					this._isTakeControlRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsTakeControlRelevant");
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00019F2D File Offset: 0x0001812D
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00019F35 File Offset: 0x00018135
		[DataSourceProperty]
		public bool IsTakeControlEnabled
		{
			get
			{
				return this._isTakeControlEnabled;
			}
			set
			{
				if (value != this._isTakeControlEnabled)
				{
					this._isTakeControlEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsTakeControlEnabled");
				}
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00019F53 File Offset: 0x00018153
		public void SetPrevCharacterInputKey(GameKey gameKey)
		{
			this.PrevCharacterKey = InputKeyItemVM.CreateFromGameKey(gameKey, false);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00019F62 File Offset: 0x00018162
		public void SetNextCharacterInputKey(GameKey gameKey)
		{
			this.NextCharacterKey = InputKeyItemVM.CreateFromGameKey(gameKey, false);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00019F71 File Offset: 0x00018171
		public void SetTakeControlInputKey(GameKey gameKey)
		{
			this.TakeControlKey = InputKeyItemVM.CreateFromGameKey(gameKey, false);
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x00019F80 File Offset: 0x00018180
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x00019F88 File Offset: 0x00018188
		[DataSourceProperty]
		public string SpectatedAgentName
		{
			get
			{
				return this._spectatedAgentName;
			}
			set
			{
				if (value != this._spectatedAgentName)
				{
					this._spectatedAgentName = value;
					base.OnPropertyChangedWithValue<string>(value, "SpectatedAgentName");
				}
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x00019FAB File Offset: 0x000181AB
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x00019FB3 File Offset: 0x000181B3
		[DataSourceProperty]
		public InputKeyItemVM PrevCharacterKey
		{
			get
			{
				return this._prevCharacterKey;
			}
			set
			{
				if (value != this._prevCharacterKey)
				{
					this._prevCharacterKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PrevCharacterKey");
				}
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x00019FD1 File Offset: 0x000181D1
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x00019FD9 File Offset: 0x000181D9
		[DataSourceProperty]
		public InputKeyItemVM NextCharacterKey
		{
			get
			{
				return this._nextCharacterKey;
			}
			set
			{
				if (value != this._nextCharacterKey)
				{
					this._nextCharacterKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextCharacterKey");
				}
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00019FF7 File Offset: 0x000181F7
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x00019FFF File Offset: 0x000181FF
		[DataSourceProperty]
		public InputKeyItemVM TakeControlKey
		{
			get
			{
				return this._takeControlKey;
			}
			set
			{
				if (value != this._takeControlKey)
				{
					this._takeControlKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "TakeControlKey");
				}
			}
		}

		// Token: 0x0400032C RID: 812
		private readonly Mission _mission;

		// Token: 0x0400032D RID: 813
		private bool _isMainHeroDead;

		// Token: 0x0400032E RID: 814
		private readonly TextObject _deadTextObject = GameTexts.FindText("str_battle_hero_dead", null);

		// Token: 0x0400032F RID: 815
		private bool _isEnabled;

		// Token: 0x04000330 RID: 816
		private string _prevCharacterText;

		// Token: 0x04000331 RID: 817
		private string _nextCharacterText;

		// Token: 0x04000332 RID: 818
		private string _takeControlText;

		// Token: 0x04000333 RID: 819
		private string _statusText;

		// Token: 0x04000334 RID: 820
		private bool _isTakeControlRelevant;

		// Token: 0x04000335 RID: 821
		private bool _isTakeControlEnabled;

		// Token: 0x04000336 RID: 822
		private string _spectatedAgentName;

		// Token: 0x04000337 RID: 823
		private InputKeyItemVM _prevCharacterKey;

		// Token: 0x04000338 RID: 824
		private InputKeyItemVM _nextCharacterKey;

		// Token: 0x04000339 RID: 825
		private InputKeyItemVM _takeControlKey;
	}
}
