using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000036 RID: 54
	public class HitpointsEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x06000215 RID: 533 RVA: 0x0000987A File Offset: 0x00007A7A
		protected HitpointsEffect()
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009884 File Offset: 0x00007A84
		protected override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
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
					XmlAttribute xmlAttribute = attributes["value"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null || !float.TryParse(text2, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\HitpointsEffect.cs", "Deserialize", 20);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000098E9 File Offset: 0x00007AE9
		public override float GetHitpoints(bool isPlayer)
		{
			if (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Any || (isPlayer ? (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Player) : (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Troops)))
			{
				return this._value;
			}
			return 0f;
		}

		// Token: 0x04000098 RID: 152
		protected static string StringType = "HitpointsOnSpawn";

		// Token: 0x04000099 RID: 153
		private float _value;
	}
}
