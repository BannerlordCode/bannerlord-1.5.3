using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000034 RID: 52
	public class GoldRecoveryEffect : MPPerkEffect
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00009576 File Offset: 0x00007776
		public override bool IsTickRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009579 File Offset: 0x00007779
		protected GoldRecoveryEffect()
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009584 File Offset: 0x00007784
		protected override void Deserialize(XmlNode node)
		{
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_disabled_in_warmup"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			base.IsDisabledInWarmup = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["value"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null || !int.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\GoldRecoveryEffect.cs", "Deserialize", 29);
			}
			string text5;
			if (node == null)
			{
				text5 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text5 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["period"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			if (text6 == null || !int.TryParse(text6, out this._period) || this._period < 1)
			{
				Debug.FailedAssert("provided 'period' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\GoldRecoveryEffect.cs", "Deserialize", 35);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009684 File Offset: 0x00007884
		public override void OnTick(Agent agent, int tickCount)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			MissionPeer missionPeer = ((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null);
			if (tickCount % this._period == 0 && missionPeer != null)
			{
				Mission mission = Mission.Current;
				MissionMultiplayerGameModeBase missionMultiplayerGameModeBase = ((mission != null) ? mission.GetMissionBehavior<MissionMultiplayerGameModeBase>() : null);
				if (missionMultiplayerGameModeBase == null)
				{
					return;
				}
				missionMultiplayerGameModeBase.ChangeCurrentGoldForPeer(missionPeer, missionPeer.Representative.Gold + this._value);
			}
		}

		// Token: 0x04000092 RID: 146
		protected static string StringType = "GoldRecovery";

		// Token: 0x04000093 RID: 147
		private int _value;

		// Token: 0x04000094 RID: 148
		private int _period;
	}
}
