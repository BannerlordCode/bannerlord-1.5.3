using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003A RID: 58
	public class MountManeuverEffect : MPPerkEffect
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00009BBA File Offset: 0x00007DBA
		protected MountManeuverEffect()
		{
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009BC4 File Offset: 0x00007DC4
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
			if (text4 == null || !float.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountManeuverEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009C68 File Offset: 0x00007E68
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && !agent.IsMount) ? agent.MountAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00009C89 File Offset: 0x00007E89
		public override float GetMountManeuver()
		{
			return this._value;
		}

		// Token: 0x040000A1 RID: 161
		protected static string StringType = "MountManeuver";

		// Token: 0x040000A2 RID: 162
		private float _value;
	}
}
