using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003B RID: 59
	public class MountSpeedEffect : MPPerkEffect
	{
		// Token: 0x0600022B RID: 555 RVA: 0x00009C9D File Offset: 0x00007E9D
		protected MountSpeedEffect()
		{
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009CA8 File Offset: 0x00007EA8
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountSpeedEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00009D4C File Offset: 0x00007F4C
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && !agent.IsMount) ? agent.MountAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009D6D File Offset: 0x00007F6D
		public override float GetMountSpeed()
		{
			return this._value;
		}

		// Token: 0x040000A3 RID: 163
		protected static string StringType = "MountSpeed";

		// Token: 0x040000A4 RID: 164
		private float _value;
	}
}
