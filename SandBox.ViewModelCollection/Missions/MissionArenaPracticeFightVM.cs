using System;
using SandBox.Missions.MissionLogics.Arena;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002D RID: 45
	public class MissionArenaPracticeFightVM : ViewModel
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00010451 File Offset: 0x0000E651
		public MissionArenaPracticeFightVM(ArenaPracticeFightMissionController practiceMissionController)
		{
			this._practiceMissionController = practiceMissionController;
			this._mission = practiceMissionController.Mission;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001046C File Offset: 0x0000E66C
		public void Tick()
		{
			this.IsPlayerPracticing = this._practiceMissionController.IsPlayerPracticing;
			Agent mainAgent = this._mission.MainAgent;
			if (mainAgent != null && mainAgent.IsActive())
			{
				int killCount = this._mission.MainAgent.KillCount;
				GameTexts.SetVariable("BEATEN_OPPONENT_COUNT", killCount);
				this.OpponentsBeatenText = GameTexts.FindText("str_beaten_opponent", null).ToString();
			}
			int remainingOpponentCount = this._practiceMissionController.RemainingOpponentCount;
			GameTexts.SetVariable("REMAINING_OPPONENT_COUNT", remainingOpponentCount);
			this.OpponentsRemainingText = GameTexts.FindText("str_remaining_opponent", null).ToString();
			this.UpdatePrizeText();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00010508 File Offset: 0x0000E708
		public void UpdatePrizeText()
		{
			bool remainingOpponentCount = this._practiceMissionController.RemainingOpponentCount != 0;
			int opponentCountBeatenByPlayer = this._practiceMissionController.OpponentCountBeatenByPlayer;
			int num = 0;
			if (!remainingOpponentCount)
			{
				num = 250;
			}
			else if (opponentCountBeatenByPlayer >= 3)
			{
				if (opponentCountBeatenByPlayer < 6)
				{
					num = 5;
				}
				else if (opponentCountBeatenByPlayer < 10)
				{
					num = 10;
				}
				else if (opponentCountBeatenByPlayer < 20)
				{
					num = 25;
				}
				else
				{
					num = 60;
				}
			}
			GameTexts.SetVariable("DENAR_AMOUNT", num);
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.PrizeText = GameTexts.FindText("str_earned_denar", null).ToString();
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0001058B File Offset: 0x0000E78B
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00010593 File Offset: 0x0000E793
		[DataSourceProperty]
		public string OpponentsBeatenText
		{
			get
			{
				return this._opponentsBeatenText;
			}
			set
			{
				if (this._opponentsBeatenText != value)
				{
					this._opponentsBeatenText = value;
					base.OnPropertyChangedWithValue<string>(value, "OpponentsBeatenText");
				}
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003BD RID: 957 RVA: 0x000105B6 File Offset: 0x0000E7B6
		// (set) Token: 0x060003BE RID: 958 RVA: 0x000105BE File Offset: 0x0000E7BE
		[DataSourceProperty]
		public string PrizeText
		{
			get
			{
				return this._prizeText;
			}
			set
			{
				if (this._prizeText != value)
				{
					this._prizeText = value;
					base.OnPropertyChangedWithValue<string>(value, "PrizeText");
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003BF RID: 959 RVA: 0x000105E1 File Offset: 0x0000E7E1
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x000105E9 File Offset: 0x0000E7E9
		[DataSourceProperty]
		public string OpponentsRemainingText
		{
			get
			{
				return this._opponentsRemainingText;
			}
			set
			{
				if (this._opponentsRemainingText != value)
				{
					this._opponentsRemainingText = value;
					base.OnPropertyChangedWithValue<string>(value, "OpponentsRemainingText");
				}
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0001060C File Offset: 0x0000E80C
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00010614 File Offset: 0x0000E814
		public bool IsPlayerPracticing
		{
			get
			{
				return this._isPlayerPracticing;
			}
			set
			{
				if (this._isPlayerPracticing != value)
				{
					this._isPlayerPracticing = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerPracticing");
				}
			}
		}

		// Token: 0x040001E9 RID: 489
		private readonly Mission _mission;

		// Token: 0x040001EA RID: 490
		private readonly ArenaPracticeFightMissionController _practiceMissionController;

		// Token: 0x040001EB RID: 491
		private string _opponentsBeatenText;

		// Token: 0x040001EC RID: 492
		private string _opponentsRemainingText;

		// Token: 0x040001ED RID: 493
		private bool _isPlayerPracticing;

		// Token: 0x040001EE RID: 494
		private string _prizeText;
	}
}
