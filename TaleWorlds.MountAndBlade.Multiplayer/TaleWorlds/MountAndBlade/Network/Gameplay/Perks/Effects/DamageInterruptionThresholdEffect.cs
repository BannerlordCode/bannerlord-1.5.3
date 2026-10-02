using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002D RID: 45
	public class DamageInterruptionThresholdEffect : MPPerkEffect
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00008DCD File Offset: 0x00006FCD
		protected DamageInterruptionThresholdEffect()
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008DD8 File Offset: 0x00006FD8
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DamageInterruptionThresholdEffect.cs", "Deserialize", 23);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008E7C File Offset: 0x0000707C
		public override float GetDamageInterruptionThreshold()
		{
			return this._value;
		}

		// Token: 0x0400007E RID: 126
		protected static string StringType = "DamageInterruptionThreshold";

		// Token: 0x0400007F RID: 127
		private float _value;
	}
}
