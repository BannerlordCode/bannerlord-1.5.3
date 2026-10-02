using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.MissionRepresentatives
{
	// Token: 0x020003D0 RID: 976
	public class TeamDeathmatchMissionRepresentative : MissionRepresentativeBase
	{
		// Token: 0x060036F6 RID: 14070 RVA: 0x000E36C1 File Offset: 0x000E18C1
		public override void OnAgentSpawned()
		{
			this._currentGoldGains = (GoldGainFlags)0;
			this._killCountOnSpawn = base.MissionPeer.KillCount;
			this._assistCountOnSpawn = base.MissionPeer.AssistCount;
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x000E36EC File Offset: 0x000E18EC
		public int GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.MPPerkHandler killerPerkHandler, MPPerkObject.MPPerkHandler assistingHitterPerkHandler, MultiplayerClassDivisions.MPHeroClass victimClass, bool isAssist, bool isRanged, bool isFriendly)
		{
			int num = 0;
			List<KeyValuePair<ushort, int>> list = new List<KeyValuePair<ushort, int>>();
			if (isAssist)
			{
				int num2 = 1;
				if (!isFriendly)
				{
					int num3 = ((killerPerkHandler != null) ? killerPerkHandler.GetRewardedGoldOnAssist() : 0) + ((assistingHitterPerkHandler != null) ? assistingHitterPerkHandler.GetGoldOnAssist() : 0);
					if (num3 > 0)
					{
						num += num3;
						this._currentGoldGains |= GoldGainFlags.PerkBonus;
						list.Add(new KeyValuePair<ushort, int>(2048, num3));
					}
				}
				switch (base.MissionPeer.AssistCount - this._assistCountOnSpawn)
				{
				case 1:
					num += 10;
					this._currentGoldGains |= GoldGainFlags.FirstAssist;
					list.Add(new KeyValuePair<ushort, int>(4, 10));
					break;
				case 2:
					num += 10;
					this._currentGoldGains |= GoldGainFlags.SecondAssist;
					list.Add(new KeyValuePair<ushort, int>(8, 10));
					break;
				case 3:
					num += 10;
					this._currentGoldGains |= GoldGainFlags.ThirdAssist;
					list.Add(new KeyValuePair<ushort, int>(16, 10));
					break;
				default:
					num += num2;
					list.Add(new KeyValuePair<ushort, int>(256, num2));
					break;
				}
			}
			else
			{
				int num4 = 0;
				if (base.ControlledAgent != null)
				{
					num4 = MultiplayerClassDivisions.GetMPHeroClassForCharacter(base.ControlledAgent.Character).TroopCasualCost;
					int num5 = victimClass.TroopCasualCost - num4;
					int num6 = 2 + MathF.Max(0, num5 / 2);
					num += num6;
					list.Add(new KeyValuePair<ushort, int>(128, num6));
				}
				int num7 = ((killerPerkHandler != null) ? killerPerkHandler.GetGoldOnKill((float)num4, (float)victimClass.TroopCasualCost) : 0);
				if (num7 > 0)
				{
					num += num7;
					this._currentGoldGains |= GoldGainFlags.PerkBonus;
					list.Add(new KeyValuePair<ushort, int>(2048, num7));
				}
				int num8 = base.MissionPeer.KillCount - this._killCountOnSpawn;
				if (num8 != 5)
				{
					if (num8 == 10)
					{
						num += 30;
						this._currentGoldGains |= GoldGainFlags.TenthKill;
						list.Add(new KeyValuePair<ushort, int>(64, 30));
					}
				}
				else
				{
					num += 20;
					this._currentGoldGains |= GoldGainFlags.FifthKill;
					list.Add(new KeyValuePair<ushort, int>(32, 20));
				}
				if (isRanged && !this._currentGoldGains.HasAnyFlag(GoldGainFlags.FirstRangedKill))
				{
					num += 10;
					this._currentGoldGains |= GoldGainFlags.FirstRangedKill;
					list.Add(new KeyValuePair<ushort, int>(1, 10));
				}
				if (!isRanged && !this._currentGoldGains.HasAnyFlag(GoldGainFlags.FirstMeleeKill))
				{
					num += 10;
					this._currentGoldGains |= GoldGainFlags.FirstMeleeKill;
					list.Add(new KeyValuePair<ushort, int>(2, 10));
				}
			}
			int num9 = 0;
			if (base.MissionPeer.Team == Mission.Current.Teams.Attacker)
			{
				num9 = MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			else if (base.MissionPeer.Team == Mission.Current.Teams.Defender)
			{
				num9 = MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			if (num9 != 0 && (num > 0 || list.Count > 0))
			{
				float num10 = 1f + (float)num9 * 0.01f;
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = new KeyValuePair<ushort, int>(list[i].Key, list[i].Value + (int)((float)list[i].Value * num10));
				}
				num += (int)((float)num * num10);
			}
			if (list.Count > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(list));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x000E3A98 File Offset: 0x000E1C98
		public int GetGoldGainsFromAllyDeathReward(int baseAmount)
		{
			if (baseAmount > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(2048, baseAmount)
				}));
				GameNetwork.EndModuleEventAsServer();
			}
			return baseAmount;
		}

		// Token: 0x0400179E RID: 6046
		private const int FirstRangedKillGold = 10;

		// Token: 0x0400179F RID: 6047
		private const int FirstMeleeKillGold = 10;

		// Token: 0x040017A0 RID: 6048
		private const int FirstAssistGold = 10;

		// Token: 0x040017A1 RID: 6049
		private const int SecondAssistGold = 10;

		// Token: 0x040017A2 RID: 6050
		private const int ThirdAssistGold = 10;

		// Token: 0x040017A3 RID: 6051
		private const int FifthKillGold = 20;

		// Token: 0x040017A4 RID: 6052
		private const int TenthKillGold = 30;

		// Token: 0x040017A5 RID: 6053
		private GoldGainFlags _currentGoldGains;

		// Token: 0x040017A6 RID: 6054
		private int _killCountOnSpawn;

		// Token: 0x040017A7 RID: 6055
		private int _assistCountOnSpawn;
	}
}
