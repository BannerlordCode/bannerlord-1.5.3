using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x0200005E RID: 94
	public class FFAScoreboardData : IScoreboardData
	{
		// Token: 0x060002DA RID: 730 RVA: 0x0000CB80 File Offset: 0x0000AD80
		public MissionScoreboardComponent.ScoreboardHeader[] GetScoreboardHeaders()
		{
			GameNetwork.MyPeer.GetComponent<MissionRepresentativeBase>();
			MissionScoreboardComponent.ScoreboardHeader[] array = new MissionScoreboardComponent.ScoreboardHeader[7];
			array[0] = new MissionScoreboardComponent.ScoreboardHeader("ping", (MissionPeer missionPeer) => MathF.Round(missionPeer.GetNetworkPeer().AveragePingInMilliseconds).ToString(), (BotData bot) => "");
			array[1] = new MissionScoreboardComponent.ScoreboardHeader("avatar", (MissionPeer missionPeer) => "", (BotData bot) => "");
			array[2] = new MissionScoreboardComponent.ScoreboardHeader("name", (MissionPeer missionPeer) => missionPeer.GetComponent<MissionPeer>().DisplayedName, (BotData bot) => new TextObject("{=hvQSOi79}Bot", null).ToString());
			array[3] = new MissionScoreboardComponent.ScoreboardHeader("kill", (MissionPeer missionPeer) => missionPeer.KillCount.ToString(), (BotData bot) => bot.KillCount.ToString());
			array[4] = new MissionScoreboardComponent.ScoreboardHeader("death", (MissionPeer missionPeer) => missionPeer.DeathCount.ToString(), (BotData bot) => bot.DeathCount.ToString());
			array[5] = new MissionScoreboardComponent.ScoreboardHeader("assist", (MissionPeer missionPeer) => missionPeer.AssistCount.ToString(), (BotData bot) => bot.AssistCount.ToString());
			array[6] = new MissionScoreboardComponent.ScoreboardHeader("score", (MissionPeer missionPeer) => missionPeer.Score.ToString(), (BotData bot) => bot.Score.ToString());
			return array;
		}
	}
}
