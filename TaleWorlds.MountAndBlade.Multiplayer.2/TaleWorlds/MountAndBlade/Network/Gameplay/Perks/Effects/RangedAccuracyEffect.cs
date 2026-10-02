using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003D RID: 61
	public class RangedAccuracyEffect : MPPerkEffect
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000A020 File Offset: 0x00008220
		protected RangedAccuracyEffect()
		{
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A028 File Offset: 0x00008228
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RangedAccuracyEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000A0CC File Offset: 0x000082CC
		public override void OnUpdate(Agent agent, bool newState)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (agent != null)
			{
				agent.UpdateAgentProperties();
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000A0ED File Offset: 0x000082ED
		public override float GetRangedAccuracy()
		{
			return this._value;
		}

		// Token: 0x040000A7 RID: 167
		protected static string StringType = "RangedAccuracy";

		// Token: 0x040000A8 RID: 168
		private float _value;
	}
}
