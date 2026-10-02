using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.General
{
	// Token: 0x0200008D RID: 141
	public class MPGeneralKillNotificationItemVM : ViewModel
	{
		// Token: 0x06000DAB RID: 3499 RVA: 0x0002A006 File Offset: 0x00028206
		public MPGeneralKillNotificationItemVM(Agent affectedAgent, Agent affectorAgent, Agent assistedAgent, Action<MPGeneralKillNotificationItemVM> onRemove, WeaponClass killWeaponClass = WeaponClass.Undefined)
		{
			this._onRemove = onRemove;
			this.InitProperties(affectedAgent, affectorAgent);
			this.InitWeaponProperties(killWeaponClass);
			this.InitDeathProperties(affectedAgent, affectorAgent, assistedAgent);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002A03C File Offset: 0x0002823C
		private void InitWeaponProperties(WeaponClass killWeaponClass)
		{
			string weaponClassSpriteName = MPGeneralKillNotificationItemVM.GetWeaponClassSpriteName(killWeaponClass);
			this.ShowKillWeapon = weaponClassSpriteName != null;
			this.KillWeaponSprite = weaponClassSpriteName ?? string.Empty;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002A06C File Offset: 0x0002826C
		private static string GetWeaponClassSpriteName(WeaponClass weaponClass)
		{
			switch (weaponClass)
			{
			case WeaponClass.Dagger:
			case WeaponClass.OneHandedSword:
			case WeaponClass.OneHandedAxe:
			case WeaponClass.Mace:
			case WeaponClass.Pick:
				return "General\\EquipmentIcons\\equipment_type_one_handed";
			case WeaponClass.TwoHandedSword:
			case WeaponClass.TwoHandedAxe:
			case WeaponClass.TwoHandedMace:
				return "General\\EquipmentIcons\\equipment_type_two_handed";
			case WeaponClass.OneHandedPolearm:
			case WeaponClass.TwoHandedPolearm:
			case WeaponClass.LowGripPolearm:
				return "General\\EquipmentIcons\\equipment_type_polearm";
			case WeaponClass.Arrow:
			case WeaponClass.Bow:
				return "General\\EquipmentIcons\\equipment_type_bow";
			case WeaponClass.Bolt:
			case WeaponClass.Crossbow:
				return "General\\EquipmentIcons\\equipment_type_crossbow";
			case WeaponClass.SlingStone:
			case WeaponClass.Sling:
				return "General\\EquipmentIcons\\equipment_type_sling";
			case WeaponClass.Stone:
			case WeaponClass.ThrowingAxe:
			case WeaponClass.ThrowingKnife:
			case WeaponClass.Javelin:
				return "General\\EquipmentIcons\\equipment_type_throwing";
			}
			return null;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002A10C File Offset: 0x0002830C
		public virtual void InitProperties(Agent affectedAgent, Agent affectorAgent)
		{
			uint num;
			uint num2;
			this.GetAgentColors(affectorAgent, out num, out num2);
			TargetIconType multiplayerAgentType = this.GetMultiplayerAgentType(affectorAgent);
			Banner agentBanner = this.GetAgentBanner(affectorAgent);
			bool? flag;
			if (affectorAgent == null)
			{
				flag = null;
			}
			else
			{
				Team team = affectorAgent.Team;
				flag = ((team != null) ? new bool?(team.IsPlayerAlly) : null);
			}
			bool flag2 = flag ?? false;
			uint num3;
			uint num4;
			this.GetAgentColors(affectedAgent, out num3, out num4);
			TargetIconType multiplayerAgentType2 = this.GetMultiplayerAgentType(affectedAgent);
			Banner agentBanner2 = this.GetAgentBanner(affectedAgent);
			Team team2 = affectedAgent.Team;
			bool flag3 = team2 != null && team2.IsPlayerAlly;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (MultiplayerSpectatorHelper.ShouldShowBothTeamsData())
			{
				BattleSideEnum? battleSideEnum;
				if (affectorAgent == null)
				{
					battleSideEnum = null;
				}
				else
				{
					Team team3 = affectorAgent.Team;
					battleSideEnum = ((team3 != null) ? new BattleSideEnum?(team3.Side) : null);
				}
				BattleSideEnum? battleSideEnum2 = battleSideEnum;
				Team team4 = affectedAgent.Team;
				BattleSideEnum? battleSideEnum3 = ((team4 != null) ? new BattleSideEnum?(team4.Side) : null);
				this.IsMurdererAlly = battleSideEnum2 != null && battleSideEnum2.Value == BattleSideEnum.Attacker;
				this.IsVictimAlly = battleSideEnum3 != null && battleSideEnum3.Value == BattleSideEnum.Defender;
			}
			else if (((missionPeer != null) ? missionPeer.Team : null) != null)
			{
				this.IsMurdererAlly = flag2 && !flag3;
				this.IsVictimAlly = flag3;
			}
			else
			{
				this.IsMurdererAlly = true;
				this.IsVictimAlly = false;
			}
			this.MurdererName = ((affectorAgent != null) ? ((affectorAgent.MissionPeer != null) ? affectorAgent.MissionPeer.DisplayedName : affectorAgent.Name) : "");
			this.MurdererCompassElement = new MPTeammateCompassTargetVM(multiplayerAgentType, num, num2, agentBanner, flag2);
			this.VictimName = ((affectedAgent.MissionPeer != null) ? affectedAgent.MissionPeer.DisplayedName : affectedAgent.Name);
			this.VictimCompassElement = new MPTeammateCompassTargetVM(multiplayerAgentType2, num3, num4, agentBanner2, flag3);
			this.IsRelatedToPlayer = affectedAgent.IsPlayerUnit || (affectorAgent != null && affectorAgent.IsPlayerUnit);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0002A320 File Offset: 0x00028520
		public void InitDeathProperties(Agent affectedAgent, Agent affectorAgent, Agent assistedAgent)
		{
			if (affectorAgent != null && affectorAgent.IsMainAgent)
			{
				MBTextManager.SetTextVariable("TROOP_NAME", affectedAgent.NameTextObject.ToString(), false);
				this.Message = GameTexts.FindText("str_kill_feed_message", null).ToString();
				return;
			}
			if (affectedAgent.IsMainAgent)
			{
				MBTextManager.SetTextVariable("TROOP_NAME", (affectorAgent != null) ? affectorAgent.ToString() : null, false);
				this.Message = GameTexts.FindText("str_death_feed_message", null).ToString();
				return;
			}
			if (assistedAgent != null && assistedAgent.IsMainAgent)
			{
				MBTextManager.SetTextVariable("TROOP_NAME", affectedAgent.NameTextObject.ToString(), false);
				this.Message = GameTexts.FindText("str_assist_feed_message", null).ToString();
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002A3D4 File Offset: 0x000285D4
		protected TargetIconType GetMultiplayerAgentType(Agent agent)
		{
			if (agent == null)
			{
				return TargetIconType.None;
			}
			if (!agent.IsHuman)
			{
				return TargetIconType.Monster;
			}
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character);
			if (mpheroClassForCharacter == null)
			{
				Debug.FailedAssert("Hero class is not set for agent: " + agent.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\KillFeed\\General\\MPGeneralKillNotificationItemVM.cs", "GetMultiplayerAgentType", 142);
				return TargetIconType.None;
			}
			return mpheroClassForCharacter.IconType;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002A42C File Offset: 0x0002862C
		private Banner GetAgentBanner(Agent agent)
		{
			Banner banner = this._defaultBanner;
			if (agent != null)
			{
				MissionPeer missionPeer = agent.MissionPeer;
				MissionPeer missionPeer2 = ((missionPeer != null) ? missionPeer.GetComponent<MissionPeer>() : null);
				if (agent.Team != null && missionPeer2 != null)
				{
					banner = new Banner(missionPeer2.Peer.BannerCode, agent.Team.Color, agent.Team.Color2);
				}
				else if (agent.Team != null && agent.Formation != null && !string.IsNullOrEmpty(agent.Formation.BannerCode))
				{
					banner = new Banner(agent.Formation.BannerCode, agent.Team.Color, agent.Team.Color2);
				}
				else if (agent.Team != null)
				{
					banner = agent.Team.Banner;
				}
			}
			return banner;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002A4ED File Offset: 0x000286ED
		private void GetAgentColors(Agent agent, out uint color1, out uint color2)
		{
			if (((agent != null) ? agent.Team : null) != null)
			{
				color1 = agent.Team.Color;
				color2 = agent.Team.Color2;
				return;
			}
			color1 = 4284111450U;
			color2 = uint.MaxValue;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002A522 File Offset: 0x00028722
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0002A530 File Offset: 0x00028730
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x0002A538 File Offset: 0x00028738
		[DataSourceProperty]
		public string MurdererName
		{
			get
			{
				return this._murdererName;
			}
			set
			{
				if (value != this._murdererName)
				{
					this._murdererName = value;
					base.OnPropertyChangedWithValue<string>(value, "MurdererName");
				}
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0002A55B File Offset: 0x0002875B
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x0002A563 File Offset: 0x00028763
		[DataSourceProperty]
		public string VictimName
		{
			get
			{
				return this._victimName;
			}
			set
			{
				if (value != this._victimName)
				{
					this._victimName = value;
					base.OnPropertyChangedWithValue<string>(value, "VictimName");
				}
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0002A586 File Offset: 0x00028786
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0002A58E File Offset: 0x0002878E
		[DataSourceProperty]
		public MPTeammateCompassTargetVM MurdererCompassElement
		{
			get
			{
				return this._murdererCompassElement;
			}
			set
			{
				if (value != this._murdererCompassElement)
				{
					this._murdererCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "MurdererCompassElement");
				}
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0002A5AC File Offset: 0x000287AC
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0002A5B4 File Offset: 0x000287B4
		[DataSourceProperty]
		public MPTeammateCompassTargetVM VictimCompassElement
		{
			get
			{
				return this._victimCompassElement;
			}
			set
			{
				if (value != this._victimCompassElement)
				{
					this._victimCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "VictimCompassElement");
				}
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0002A5D2 File Offset: 0x000287D2
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x0002A5DA File Offset: 0x000287DA
		[DataSourceProperty]
		public bool IsRelatedToPlayer
		{
			get
			{
				return this._isRelatedToPlayer;
			}
			set
			{
				if (value != this._isRelatedToPlayer)
				{
					this._isRelatedToPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsRelatedToPlayer");
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0002A5F8 File Offset: 0x000287F8
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0002A600 File Offset: 0x00028800
		[DataSourceProperty]
		public string KillWeaponSprite
		{
			get
			{
				return this._killWeaponSprite;
			}
			set
			{
				if (value != this._killWeaponSprite)
				{
					this._killWeaponSprite = value;
					base.OnPropertyChangedWithValue<string>(value, "KillWeaponSprite");
				}
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0002A623 File Offset: 0x00028823
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0002A62B File Offset: 0x0002882B
		[DataSourceProperty]
		public bool ShowKillWeapon
		{
			get
			{
				return this._showKillWeapon;
			}
			set
			{
				if (value != this._showKillWeapon)
				{
					this._showKillWeapon = value;
					base.OnPropertyChangedWithValue(value, "ShowKillWeapon");
				}
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0002A649 File Offset: 0x00028849
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0002A651 File Offset: 0x00028851
		[DataSourceProperty]
		public bool IsMurdererAlly
		{
			get
			{
				return this._isMurdererAlly;
			}
			set
			{
				if (value != this._isMurdererAlly)
				{
					this._isMurdererAlly = value;
					base.OnPropertyChangedWithValue(value, "IsMurdererAlly");
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0002A66F File Offset: 0x0002886F
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0002A677 File Offset: 0x00028877
		[DataSourceProperty]
		public bool IsVictimAlly
		{
			get
			{
				return this._isVictimAlly;
			}
			set
			{
				if (value != this._isVictimAlly)
				{
					this._isVictimAlly = value;
					base.OnPropertyChangedWithValue(value, "IsVictimAlly");
				}
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0002A695 File Offset: 0x00028895
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x0002A69D File Offset: 0x0002889D
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x04000638 RID: 1592
		private readonly Action<MPGeneralKillNotificationItemVM> _onRemove;

		// Token: 0x04000639 RID: 1593
		private readonly Banner _defaultBanner = Banner.CreateOneColoredEmptyBanner(92);

		// Token: 0x0400063A RID: 1594
		private string _murdererName;

		// Token: 0x0400063B RID: 1595
		private string _victimName;

		// Token: 0x0400063C RID: 1596
		private MPTeammateCompassTargetVM _murdererCompassElement;

		// Token: 0x0400063D RID: 1597
		private MPTeammateCompassTargetVM _victimCompassElement;

		// Token: 0x0400063E RID: 1598
		private bool _isRelatedToPlayer;

		// Token: 0x0400063F RID: 1599
		private bool _isMurdererAlly;

		// Token: 0x04000640 RID: 1600
		private bool _isVictimAlly;

		// Token: 0x04000641 RID: 1601
		private string _killWeaponSprite;

		// Token: 0x04000642 RID: 1602
		private bool _showKillWeapon;

		// Token: 0x04000643 RID: 1603
		private string _message;
	}
}
