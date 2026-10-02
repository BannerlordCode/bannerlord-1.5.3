using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000033 RID: 51
	public class GoldGainOnKillEffect : MPPerkEffect
	{
		// Token: 0x06000207 RID: 519 RVA: 0x00009414 File Offset: 0x00007614
		protected GoldGainOnKillEffect()
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000941C File Offset: 0x0000761C
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\GoldGainOnKillEffect.cs", "Deserialize", 31);
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
					XmlAttribute xmlAttribute3 = attributes3["enemy_value"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			this._enemyValue = GoldGainOnKillEffect.EnemyValue.Any;
			if (text6 != null && !Enum.TryParse<GoldGainOnKillEffect.EnemyValue>(text6, true, out this._enemyValue))
			{
				this._enemyValue = GoldGainOnKillEffect.EnemyValue.Any;
				Debug.FailedAssert("provided 'enemy_value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\GoldGainOnKillEffect.cs", "Deserialize", 39);
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009520 File Offset: 0x00007720
		public override int GetGoldOnKill(float attackerValue, float victimValue)
		{
			switch (this._enemyValue)
			{
			case GoldGainOnKillEffect.EnemyValue.Any:
				return this._value;
			case GoldGainOnKillEffect.EnemyValue.Higher:
				if (victimValue <= attackerValue)
				{
					return 0;
				}
				return this._value;
			case GoldGainOnKillEffect.EnemyValue.Lower:
				if (victimValue >= attackerValue)
				{
					return 0;
				}
				return this._value;
			default:
				return 0;
			}
		}

		// Token: 0x0400008F RID: 143
		protected static string StringType = "GoldGainOnKill";

		// Token: 0x04000090 RID: 144
		private int _value;

		// Token: 0x04000091 RID: 145
		private GoldGainOnKillEffect.EnemyValue _enemyValue;

		// Token: 0x020000A5 RID: 165
		private enum EnemyValue
		{
			// Token: 0x040001AE RID: 430
			Any,
			// Token: 0x040001AF RID: 431
			Higher,
			// Token: 0x040001B0 RID: 432
			Lower
		}
	}
}
