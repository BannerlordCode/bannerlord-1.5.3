using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.MissionRepresentatives
{
	// Token: 0x020003CF RID: 975
	public class SiegeMissionRepresentative : MissionRepresentativeBase
	{
		// Token: 0x060036F0 RID: 14064 RVA: 0x000E3178 File Offset: 0x000E1378
		public override void OnAgentSpawned()
		{
			this._currentGoldGains = (GoldGainFlags)0;
			this._killCountOnSpawn = base.MissionPeer.KillCount;
			this._assistCountOnSpawn = base.MissionPeer.AssistCount;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x000E31A4 File Offset: 0x000E13A4
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
				num = 0;
				float num10 = 1f + (float)num9 * 0.01f;
				for (int i = 0; i < list.Count; i++)
				{
					int num11 = (int)((float)list[i].Value * num10);
					list[i] = new KeyValuePair<ushort, int>(list[i].Key, num11);
					num += num11;
				}
			}
			if (list.Count > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(list));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000E353C File Offset: 0x000E173C
		public int GetGoldGainsFromObjectiveAssist(GameEntity objectiveMostParentEntity, float contributionRatio, bool isCompleted)
		{
			int num = (int)(contributionRatio * (float)this.GetTotalGoldDistributionForDestructable(objectiveMostParentEntity));
			if (num > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(isCompleted ? 512 : 1024, num)
				}));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000E35B8 File Offset: 0x000E17B8
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

		// Token: 0x060036F4 RID: 14068 RVA: 0x000E3620 File Offset: 0x000E1820
		private int GetTotalGoldDistributionForDestructable(GameEntity objectiveMostParentEntity)
		{
			string text = null;
			foreach (string text2 in objectiveMostParentEntity.Tags)
			{
				if (text2.StartsWith("mp_siege_objective_"))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				return 20;
			}
			string text3 = text.Replace("mp_siege_objective_", "");
			if (text3 == "wall_breach" || text3 == "castle_gate")
			{
				return 500;
			}
			if (!(text3 == "battering_ram") && !(text3 == "siege_tower"))
			{
				return 20;
			}
			return 600;
		}

		// Token: 0x04001794 RID: 6036
		private const int FirstRangedKillGold = 10;

		// Token: 0x04001795 RID: 6037
		private const int FirstMeleeKillGold = 10;

		// Token: 0x04001796 RID: 6038
		private const int FirstAssistGold = 10;

		// Token: 0x04001797 RID: 6039
		private const int SecondAssistGold = 10;

		// Token: 0x04001798 RID: 6040
		private const int ThirdAssistGold = 10;

		// Token: 0x04001799 RID: 6041
		private const int FifthKillGold = 20;

		// Token: 0x0400179A RID: 6042
		private const int TenthKillGold = 30;

		// Token: 0x0400179B RID: 6043
		private GoldGainFlags _currentGoldGains;

		// Token: 0x0400179C RID: 6044
		private int _killCountOnSpawn;

		// Token: 0x0400179D RID: 6045
		private int _assistCountOnSpawn;
	}
}
