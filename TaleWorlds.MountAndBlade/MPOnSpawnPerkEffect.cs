using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000314 RID: 788
	public abstract class MPOnSpawnPerkEffect : MPOnSpawnPerkEffectBase
	{
		// Token: 0x06002D52 RID: 11602 RVA: 0x000AF4A8 File Offset: 0x000AD6A8
		static MPOnSpawnPerkEffect()
		{
			foreach (Type type in from t in PerkAssemblyCollection.GetPerkAssemblyTypes()
				where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MPOnSpawnPerkEffect))
				select t)
			{
				FieldInfo field = type.GetField("StringType", BindingFlags.Static | BindingFlags.NonPublic);
				string text = (string)((field != null) ? field.GetValue(null) : null);
				MPOnSpawnPerkEffect.Registered.Add(text, type);
			}
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000AF538 File Offset: 0x000AD738
		public static MPOnSpawnPerkEffect CreateFrom(XmlNode node)
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
					XmlAttribute xmlAttribute = attributes["type"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			MPOnSpawnPerkEffect mponSpawnPerkEffect = (MPOnSpawnPerkEffect)Activator.CreateInstance(MPOnSpawnPerkEffect.Registered[text2], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
			mponSpawnPerkEffect.Deserialize(node);
			return mponSpawnPerkEffect;
		}

		// Token: 0x04001201 RID: 4609
		protected static Dictionary<string, Type> Registered = new Dictionary<string, Type>();
	}
}
