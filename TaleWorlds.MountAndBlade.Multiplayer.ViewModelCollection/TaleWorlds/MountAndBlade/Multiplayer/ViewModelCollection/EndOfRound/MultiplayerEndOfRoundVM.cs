using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.EndOfRound
{
	// Token: 0x020000A5 RID: 165
	public class MultiplayerEndOfRoundVM : ViewModel
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x00031968 File Offset: 0x0002FB68
		public MultiplayerEndOfRoundVM(MissionScoreboardComponent scoreboardComponent, MissionLobbyComponent missionLobbyComponent, IRoundComponent multiplayerRoundComponent)
		{
			this._scoreboardComponent = scoreboardComponent;
			this._multiplayerRoundComponent = multiplayerRoundComponent;
			this._missionLobbyComponent = missionLobbyComponent;
			this._victoryText = new TextObject("{=RCuCoVgd}ROUND WON", null).ToString();
			this._defeatText = new TextObject("{=Dbkx4v90}ROUND LOST", null).ToString();
			this._roundEndReasonAllyTeamSideDepletedTextObject = new TextObject("{=9M4G8DDd}Your team was wiped out", null);
			this._roundEndReasonEnemyTeamSideDepletedTextObject = new TextObject("{=jPXglGWT}Enemy team was wiped out", null);
			this._roundEndReasonAllyTeamRoundTimeEndedTextObject = new TextObject("{=x1HZy70i}Your team had the upper hand at timeout", null);
			this._roundEndReasonEnemyTeamRoundTimeEndedTextObject = new TextObject("{=Dc3fFblo}Enemy team had the upper hand at timeout", null);
			this._roundEndReasonRoundTimeEndedWithDrawTextObject = new TextObject("{=i3dJSlD0}No team had the upper hand at timeout", null);
			if (this._missionLobbyComponent.MissionType == MultiplayerGameType.Battle || this._missionLobbyComponent.MissionType == MultiplayerGameType.Captain || this._missionLobbyComponent.MissionType == MultiplayerGameType.Skirmish)
			{
				this._roundEndReasonAllyTeamGameModeSpecificEndedTextObject = new TextObject("{=xxuzZJ3G}Your team ran out of morale", null);
				this._roundEndReasonEnemyTeamGameModeSpecificEndedTextObject = new TextObject("{=c6c9eYrD}Enemy team ran out of morale", null);
			}
			else
			{
				this._roundEndReasonAllyTeamGameModeSpecificEndedTextObject = TextObject.GetEmpty();
				this._roundEndReasonEnemyTeamGameModeSpecificEndedTextObject = TextObject.GetEmpty();
			}
			this.AttackerSide = new MultiplayerEndOfRoundSideVM();
			this.DefenderSide = new MultiplayerEndOfRoundSideVM();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00031A8B File Offset: 0x0002FC8B
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._multiplayerRoundComponent != null)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00031AA4 File Offset: 0x0002FCA4
		public void Refresh()
		{
			BattleSideEnum allyBattleSideEnum = BattleSideEnum.None;
			BattleSideEnum battleSideEnum = BattleSideEnum.None;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null && missionPeer.Team != null)
			{
				allyBattleSideEnum = missionPeer.Team.Side;
				battleSideEnum = ((allyBattleSideEnum == BattleSideEnum.Attacker) ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
			}
			bool flag = allyBattleSideEnum == BattleSideEnum.Attacker;
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = this._scoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == BattleSideEnum.Attacker);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide2 = this._scoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == BattleSideEnum.Defender);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide3 = (flag ? missionScoreboardSide : missionScoreboardSide2);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide4 = (flag ? missionScoreboardSide2 : missionScoreboardSide);
			BasicCultureObject culture = missionScoreboardSide3.GetCulture();
			BasicCultureObject culture2 = missionScoreboardSide4.GetCulture();
			bool flag2 = this._multiplayerRoundComponent.RoundWinner == allyBattleSideEnum;
			bool flag3 = this._multiplayerRoundComponent.RoundWinner == battleSideEnum;
			this.AttackerMVPTitleText = this.GetMVPTitleText(culture);
			this.DefenderMVPTitleText = this.GetMVPTitleText(culture2);
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(culture, culture2);
			this.AttackerSide.SetData(culture, missionScoreboardSide3.SideScore, flag2, multiplayerBattleColors.AttackerColors);
			this.DefenderSide.SetData(culture2, missionScoreboardSide4.SideScore, flag3, multiplayerBattleColors.DefenderColors);
			if (this._scoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == allyBattleSideEnum) != null && this._multiplayerRoundComponent != null)
			{
				bool flag4 = false;
				if (this._multiplayerRoundComponent.RoundWinner == allyBattleSideEnum)
				{
					this.IsRoundWinner = true;
					this.Title = this._victoryText;
				}
				else if (this._multiplayerRoundComponent.RoundWinner == battleSideEnum)
				{
					this.IsRoundWinner = false;
					this.Title = this._defeatText;
				}
				else
				{
					flag4 = true;
				}
				RoundEndReason roundEndReason = this._multiplayerRoundComponent.RoundEndReason;
				if (roundEndReason == RoundEndReason.SideDepleted)
				{
					this.Description = (this.IsRoundWinner ? this._roundEndReasonEnemyTeamSideDepletedTextObject.ToString() : this._roundEndReasonAllyTeamSideDepletedTextObject.ToString());
					return;
				}
				if (roundEndReason == RoundEndReason.GameModeSpecificEnded)
				{
					this.Description = (this.IsRoundWinner ? this._roundEndReasonEnemyTeamGameModeSpecificEndedTextObject.ToString() : this._roundEndReasonAllyTeamGameModeSpecificEndedTextObject.ToString());
					return;
				}
				if (roundEndReason == RoundEndReason.RoundTimeEnded)
				{
					this.Description = (this.IsRoundWinner ? this._roundEndReasonAllyTeamRoundTimeEndedTextObject.ToString() : (flag4 ? this._roundEndReasonRoundTimeEndedWithDrawTextObject.ToString() : this._roundEndReasonEnemyTeamRoundTimeEndedTextObject.ToString()));
				}
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00031D30 File Offset: 0x0002FF30
		public void OnMVPSelected(MissionPeer mvpPeer)
		{
			BasicCharacterObject @object = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			@object.UpdatePlayerCharacterBodyProperties(mvpPeer.Peer.BodyProperties, mvpPeer.Peer.Race, mvpPeer.Peer.IsFemale);
			@object.Age = mvpPeer.Peer.BodyProperties.Age;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			Team team = mvpPeer.Team;
			BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
			Team team2 = missionPeer.Team;
			BattleSideEnum? battleSideEnum2 = ((team2 != null) ? new BattleSideEnum?(team2.Side) : null);
			if ((battleSideEnum.GetValueOrDefault() == battleSideEnum2.GetValueOrDefault()) & (battleSideEnum != null == (battleSideEnum2 != null)))
			{
				this.AttackerMVP = new MPPlayerVM(mvpPeer);
				this.AttackerMVP.RefreshDivision(false);
				this.AttackerMVP.RefreshPreview(@object, mvpPeer.Peer.BodyProperties.DynamicProperties, mvpPeer.Peer.IsFemale);
				this.HasAttackerMVP = true;
				return;
			}
			this.DefenderMVP = new MPPlayerVM(mvpPeer);
			this.DefenderMVP.RefreshDivision(false);
			this.DefenderMVP.RefreshPreview(@object, mvpPeer.Peer.BodyProperties.DynamicProperties, mvpPeer.Peer.IsFemale);
			this.HasDefenderMVP = true;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00031E98 File Offset: 0x00030098
		private string GetMVPTitleText(BasicCultureObject culture)
		{
			if (culture.StringId == "vlandia")
			{
				return new TextObject("{=3VosbFR0}Vlandian Champion", null).ToString();
			}
			if (culture.StringId == "sturgia")
			{
				return new TextObject("{=AGUXiN8u}Voivode", null).ToString();
			}
			if (culture.StringId == "khuzait")
			{
				return new TextObject("{=F2h2cT4q}Khan's Chosen", null).ToString();
			}
			if (culture.StringId == "battania")
			{
				return new TextObject("{=eWPN3HmE}Hero of Battania", null).ToString();
			}
			if (culture.StringId == "aserai")
			{
				return new TextObject("{=5zNfxZ7B}War Prince", null).ToString();
			}
			if (culture.StringId == "empire")
			{
				return new TextObject("{=wwbIcqsq}Conqueror", null).ToString();
			}
			Debug.FailedAssert("Invalid Culture ID for MVP Title", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\EndOfRound\\MultiplayerEndOfRoundVM.cs", "GetMVPTitleText", 205);
			return string.Empty;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00031F95 File Offset: 0x00030195
		private void OnIsShownChanged()
		{
			if (!this.IsShown)
			{
				this.HasAttackerMVP = false;
				this.HasDefenderMVP = false;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00031FAD File Offset: 0x000301AD
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x00031FB5 File Offset: 0x000301B5
		[DataSourceProperty]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChangedWithValue(value, "IsShown");
					this.OnIsShownChanged();
				}
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00031FD9 File Offset: 0x000301D9
		// (set) Token: 0x06000FF5 RID: 4085 RVA: 0x00031FE1 File Offset: 0x000301E1
		[DataSourceProperty]
		public bool HasAttackerMVP
		{
			get
			{
				return this._hasAttackerMVP;
			}
			set
			{
				if (value != this._hasAttackerMVP)
				{
					this._hasAttackerMVP = value;
					base.OnPropertyChangedWithValue(value, "HasAttackerMVP");
				}
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00031FFF File Offset: 0x000301FF
		// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x00032007 File Offset: 0x00030207
		[DataSourceProperty]
		public bool HasDefenderMVP
		{
			get
			{
				return this._hasDefenderMVP;
			}
			set
			{
				if (value != this._hasDefenderMVP)
				{
					this._hasDefenderMVP = value;
					base.OnPropertyChangedWithValue(value, "HasDefenderMVP");
				}
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00032025 File Offset: 0x00030225
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x0003202D File Offset: 0x0003022D
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00032050 File Offset: 0x00030250
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00032058 File Offset: 0x00030258
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0003207B File Offset: 0x0003027B
		// (set) Token: 0x06000FFD RID: 4093 RVA: 0x00032083 File Offset: 0x00030283
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x000320A6 File Offset: 0x000302A6
		// (set) Token: 0x06000FFF RID: 4095 RVA: 0x000320AE File Offset: 0x000302AE
		[DataSourceProperty]
		public bool IsRoundWinner
		{
			get
			{
				return this._isRoundWinner;
			}
			set
			{
				if (value != this._isRoundWinner)
				{
					this._isRoundWinner = value;
					base.OnPropertyChangedWithValue(value, "IsRoundWinner");
				}
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x000320CC File Offset: 0x000302CC
		// (set) Token: 0x06001001 RID: 4097 RVA: 0x000320D4 File Offset: 0x000302D4
		[DataSourceProperty]
		public MultiplayerEndOfRoundSideVM AttackerSide
		{
			get
			{
				return this._attackerSide;
			}
			set
			{
				if (value != this._attackerSide)
				{
					this._attackerSide = value;
					base.OnPropertyChangedWithValue<MultiplayerEndOfRoundSideVM>(value, "AttackerSide");
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x000320F2 File Offset: 0x000302F2
		// (set) Token: 0x06001003 RID: 4099 RVA: 0x000320FA File Offset: 0x000302FA
		[DataSourceProperty]
		public MultiplayerEndOfRoundSideVM DefenderSide
		{
			get
			{
				return this._defenderSide;
			}
			set
			{
				if (value != this._defenderSide)
				{
					this._defenderSide = value;
					base.OnPropertyChangedWithValue<MultiplayerEndOfRoundSideVM>(value, "DefenderSide");
				}
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x00032118 File Offset: 0x00030318
		// (set) Token: 0x06001005 RID: 4101 RVA: 0x00032120 File Offset: 0x00030320
		[DataSourceProperty]
		public MPPlayerVM AttackerMVP
		{
			get
			{
				return this._attackerMVP;
			}
			set
			{
				if (value != this._attackerMVP)
				{
					this._attackerMVP = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "AttackerMVP");
				}
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x0003213E File Offset: 0x0003033E
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x00032146 File Offset: 0x00030346
		[DataSourceProperty]
		public MPPlayerVM DefenderMVP
		{
			get
			{
				return this._defenderMVP;
			}
			set
			{
				if (value != this._defenderMVP)
				{
					this._defenderMVP = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "DefenderMVP");
				}
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x00032164 File Offset: 0x00030364
		// (set) Token: 0x06001009 RID: 4105 RVA: 0x0003216C File Offset: 0x0003036C
		[DataSourceProperty]
		public string AttackerMVPTitleText
		{
			get
			{
				return this._attackerMVPTitleText;
			}
			set
			{
				if (value != this._attackerMVPTitleText)
				{
					this._attackerMVPTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerMVPTitleText");
				}
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x0003218F File Offset: 0x0003038F
		// (set) Token: 0x0600100B RID: 4107 RVA: 0x00032197 File Offset: 0x00030397
		[DataSourceProperty]
		public string DefenderMVPTitleText
		{
			get
			{
				return this._defenderMVPTitleText;
			}
			set
			{
				if (value != this._defenderMVPTitleText)
				{
					this._defenderMVPTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderMVPTitleText");
				}
			}
		}

		// Token: 0x0400076B RID: 1899
		private readonly MissionScoreboardComponent _scoreboardComponent;

		// Token: 0x0400076C RID: 1900
		private readonly MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400076D RID: 1901
		private readonly IRoundComponent _multiplayerRoundComponent;

		// Token: 0x0400076E RID: 1902
		private readonly string _victoryText;

		// Token: 0x0400076F RID: 1903
		private readonly string _defeatText;

		// Token: 0x04000770 RID: 1904
		private readonly TextObject _roundEndReasonAllyTeamSideDepletedTextObject;

		// Token: 0x04000771 RID: 1905
		private readonly TextObject _roundEndReasonEnemyTeamSideDepletedTextObject;

		// Token: 0x04000772 RID: 1906
		private readonly TextObject _roundEndReasonAllyTeamRoundTimeEndedTextObject;

		// Token: 0x04000773 RID: 1907
		private readonly TextObject _roundEndReasonEnemyTeamRoundTimeEndedTextObject;

		// Token: 0x04000774 RID: 1908
		private readonly TextObject _roundEndReasonAllyTeamGameModeSpecificEndedTextObject;

		// Token: 0x04000775 RID: 1909
		private readonly TextObject _roundEndReasonEnemyTeamGameModeSpecificEndedTextObject;

		// Token: 0x04000776 RID: 1910
		private readonly TextObject _roundEndReasonRoundTimeEndedWithDrawTextObject;

		// Token: 0x04000777 RID: 1911
		private bool _isShown;

		// Token: 0x04000778 RID: 1912
		private bool _hasAttackerMVP;

		// Token: 0x04000779 RID: 1913
		private bool _hasDefenderMVP;

		// Token: 0x0400077A RID: 1914
		private string _title;

		// Token: 0x0400077B RID: 1915
		private string _description;

		// Token: 0x0400077C RID: 1916
		private string _cultureId;

		// Token: 0x0400077D RID: 1917
		private bool _isRoundWinner;

		// Token: 0x0400077E RID: 1918
		private MultiplayerEndOfRoundSideVM _attackerSide;

		// Token: 0x0400077F RID: 1919
		private MultiplayerEndOfRoundSideVM _defenderSide;

		// Token: 0x04000780 RID: 1920
		private MPPlayerVM _attackerMVP;

		// Token: 0x04000781 RID: 1921
		private MPPlayerVM _defenderMVP;

		// Token: 0x04000782 RID: 1922
		private string _attackerMVPTitleText;

		// Token: 0x04000783 RID: 1923
		private string _defenderMVPTitleText;
	}
}
