using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Tournament
{
	// Token: 0x0200000E RID: 14
	public class TournamentParticipantVM : ViewModel
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005F4E File Offset: 0x0000414E
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00005F56 File Offset: 0x00004156
		public TournamentParticipant Participant { get; private set; }

		// Token: 0x060000CD RID: 205 RVA: 0x00005F5F File Offset: 0x0000415F
		public TournamentParticipantVM()
		{
			this._visual = new CharacterImageIdentifierVM(null);
			this._character = new CharacterViewModel(CharacterViewModel.StanceTypes.CelebrateVictory);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005F9C File Offset: 0x0000419C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.IsInitialized)
			{
				this.Refresh(this.Participant, this.TeamColor);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005FC0 File Offset: 0x000041C0
		public void Refresh(TournamentParticipant participant, Color teamColor)
		{
			this.Participant = participant;
			this.TeamColor = teamColor;
			this.State = ((participant == null) ? 0 : ((participant.Character == CharacterObject.PlayerCharacter) ? 2 : 1));
			this.IsInitialized = true;
			this._latestParticipant = participant;
			if (participant != null)
			{
				this.Name = participant.Character.Name.ToString();
				CharacterCode characterCode = SandBoxUIHelper.GetCharacterCode(participant.Character, false);
				this.Character = new CharacterViewModel(CharacterViewModel.StanceTypes.CelebrateVictory);
				this.Character.FillFrom(participant.Character, -1, null);
				this.Visual = new CharacterImageIdentifierVM(characterCode);
				this.IsValid = true;
				this.IsMainHero = participant.Character.IsPlayerCharacter;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000606F File Offset: 0x0000426F
		public void ExecuteOpenEncyclopedia()
		{
			TournamentParticipant participant = this.Participant;
			if (((participant != null) ? participant.Character : null) != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Participant.Character.EncyclopediaLink);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000060A4 File Offset: 0x000042A4
		public void Refresh()
		{
			base.OnPropertyChanged("Name");
			base.OnPropertyChanged("Visual");
			base.OnPropertyChanged("Score");
			base.OnPropertyChanged("State");
			base.OnPropertyChanged("TeamColor");
			base.OnPropertyChanged("IsDead");
			TournamentParticipant latestParticipant = this._latestParticipant;
			this.IsMainHero = latestParticipant != null && latestParticipant.Character.IsPlayerCharacter;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006110 File Offset: 0x00004310
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00006118 File Offset: 0x00004318
		[DataSourceProperty]
		public bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
			set
			{
				if (value != this._isInitialized)
				{
					this._isInitialized = value;
					base.OnPropertyChangedWithValue(value, "IsInitialized");
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006136 File Offset: 0x00004336
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000613E File Offset: 0x0000433E
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (value != this._isValid)
				{
					this._isValid = value;
					base.OnPropertyChangedWithValue(value, "IsValid");
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000615C File Offset: 0x0000435C
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00006164 File Offset: 0x00004364
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (value != this._isDead)
				{
					this._isDead = value;
					base.OnPropertyChangedWithValue(value, "IsDead");
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00006182 File Offset: 0x00004382
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x0000618A File Offset: 0x0000438A
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000061A8 File Offset: 0x000043A8
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000061B0 File Offset: 0x000043B0
		[DataSourceProperty]
		public Color TeamColor
		{
			get
			{
				return this._teamColor;
			}
			set
			{
				if (value != this._teamColor)
				{
					this._teamColor = value;
					base.OnPropertyChangedWithValue(value, "TeamColor");
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000061D3 File Offset: 0x000043D3
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000061DB File Offset: 0x000043DB
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000061F9 File Offset: 0x000043F9
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00006201 File Offset: 0x00004401
		[DataSourceProperty]
		public int State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (value != this._state)
				{
					this._state = value;
					base.OnPropertyChangedWithValue(value, "State");
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000621F File Offset: 0x0000441F
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00006227 File Offset: 0x00004427
		[DataSourceProperty]
		public bool IsQualifiedForNextRound
		{
			get
			{
				return this._isQualifiedForNextRound;
			}
			set
			{
				if (value != this._isQualifiedForNextRound)
				{
					this._isQualifiedForNextRound = value;
					base.OnPropertyChangedWithValue(value, "IsQualifiedForNextRound");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006245 File Offset: 0x00004445
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000624D File Offset: 0x0000444D
		[DataSourceProperty]
		public string Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue<string>(value, "Score");
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00006270 File Offset: 0x00004470
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00006278 File Offset: 0x00004478
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000629B File Offset: 0x0000449B
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000062A3 File Offset: 0x000044A3
		[DataSourceProperty]
		public CharacterViewModel Character
		{
			get
			{
				return this._character;
			}
			set
			{
				if (value != this._character)
				{
					this._character = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "Character");
				}
			}
		}

		// Token: 0x0400005B RID: 91
		private TournamentParticipant _latestParticipant;

		// Token: 0x0400005D RID: 93
		private bool _isInitialized;

		// Token: 0x0400005E RID: 94
		private bool _isValid;

		// Token: 0x0400005F RID: 95
		private string _name = "";

		// Token: 0x04000060 RID: 96
		private string _score = "-";

		// Token: 0x04000061 RID: 97
		private bool _isQualifiedForNextRound;

		// Token: 0x04000062 RID: 98
		private int _state = -1;

		// Token: 0x04000063 RID: 99
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000064 RID: 100
		private Color _teamColor;

		// Token: 0x04000065 RID: 101
		private bool _isDead;

		// Token: 0x04000066 RID: 102
		private bool _isMainHero;

		// Token: 0x04000067 RID: 103
		private CharacterViewModel _character;

		// Token: 0x02000076 RID: 118
		public enum TournamentPlayerState
		{
			// Token: 0x04000371 RID: 881
			EmptyPlayer,
			// Token: 0x04000372 RID: 882
			GenericPlayer,
			// Token: 0x04000373 RID: 883
			MainPlayer
		}
	}
}
