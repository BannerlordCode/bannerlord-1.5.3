using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000035 RID: 53
	public class HealthRecoveryEffect : MPPerkEffect
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000970B File Offset: 0x0000790B
		public override bool IsTickRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000970E File Offset: 0x0000790E
		protected HealthRecoveryEffect()
		{
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009718 File Offset: 0x00007918
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\HealthRecoveryEffect.cs", "Deserialize", 29);
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
				Debug.FailedAssert("provided 'period' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\HealthRecoveryEffect.cs", "Deserialize", 35);
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009818 File Offset: 0x00007A18
		public override void OnTick(Agent agent, int tickCount)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			if (tickCount % this._period == 0 && agent != null && agent.IsActive())
			{
				agent.Health = MathF.Min(agent.HealthLimit, agent.Health + this._value);
			}
		}

		// Token: 0x04000095 RID: 149
		protected static string StringType = "HealthRecovery";

		// Token: 0x04000096 RID: 150
		private float _value;

		// Token: 0x04000097 RID: 151
		private int _period;
	}
}
