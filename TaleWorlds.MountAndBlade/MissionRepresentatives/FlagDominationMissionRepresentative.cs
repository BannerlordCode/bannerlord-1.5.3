using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.MissionRepresentatives
{
	// Token: 0x020003CE RID: 974
	public class FlagDominationMissionRepresentative : MissionRepresentativeBase
	{
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060036E8 RID: 14056 RVA: 0x000E2F3D File Offset: 0x000E113D
		private bool Forfeited
		{
			get
			{
				return base.Gold < 0;
			}
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000E2F48 File Offset: 0x000E1148
		public int GetGoldAmountForVisual()
		{
			if (base.Gold < 0)
			{
				return 80;
			}
			return base.Gold;
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000E2F5C File Offset: 0x000E115C
		public void UpdateSelectedClassServer(Agent agent)
		{
			this._survivedLastRound = agent != null;
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x000E2F68 File Offset: 0x000E1168
		public bool CheckIfSurvivedLastRoundAndReset()
		{
			bool survivedLastRound = this._survivedLastRound;
			this._survivedLastRound = false;
			return survivedLastRound;
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000E2F78 File Offset: 0x000E1178
		public int GetGoldGainsFromKillData(MPPerkObject.MPPerkHandler killerPerkHandler, MPPerkObject.MPPerkHandler assistingHitterPerkHandler, MultiplayerClassDivisions.MPHeroClass victimClass, bool isAssist, bool isFriendly)
		{
			if (isFriendly || this.Forfeited)
			{
				return 0;
			}
			int num;
			if (isAssist)
			{
				num = ((killerPerkHandler != null) ? killerPerkHandler.GetRewardedGoldOnAssist() : 0) + ((assistingHitterPerkHandler != null) ? assistingHitterPerkHandler.GetGoldOnAssist() : 0);
			}
			else
			{
				int num2 = ((base.ControlledAgent != null) ? MultiplayerClassDivisions.GetMPHeroClassForCharacter(base.ControlledAgent.Character).TroopBattleCost : 0);
				num = ((killerPerkHandler != null) ? killerPerkHandler.GetGoldOnKill((float)num2, (float)victimClass.TroopBattleCost) : 0);
			}
			if (num > 0)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(2048, num)
				}));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000E3024 File Offset: 0x000E1224
		public int GetGoldGainFromKillDataAndUpdateFlags(MultiplayerClassDivisions.MPHeroClass victimClass, bool isAssist)
		{
			int num = 0;
			int num2 = 50;
			List<KeyValuePair<ushort, int>> list = new List<KeyValuePair<ushort, int>>();
			if (base.ControlledAgent != null)
			{
				num2 += victimClass.TroopBattleCost - MultiplayerClassDivisions.GetMPHeroClassForCharacter(base.ControlledAgent.Character).TroopBattleCost / 2;
			}
			if (isAssist)
			{
				int num3 = MathF.Max(5, num2 / 10);
				num += num3;
				list.Add(new KeyValuePair<ushort, int>(256, num3));
			}
			else if (base.ControlledAgent != null)
			{
				int num4 = MathF.Max(10, num2 / 5);
				num += num4;
				list.Add(new KeyValuePair<ushort, int>(128, num4));
			}
			if (list.Count > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(list));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000E3100 File Offset: 0x000E1300
		public int GetGoldGainsFromAllyDeathReward(int baseAmount)
		{
			if (this.Forfeited)
			{
				return 0;
			}
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

		// Token: 0x04001793 RID: 6035
		private bool _survivedLastRound;
	}
}
