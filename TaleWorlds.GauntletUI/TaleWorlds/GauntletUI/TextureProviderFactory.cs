using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000032 RID: 50
	public static class TextureProviderFactory
	{
		// Token: 0x0600036C RID: 876 RVA: 0x0000F230 File Offset: 0x0000D430
		public static TextureProvider CreateInstance(string textureProviderName)
		{
			Type type;
			if (TextureProviderFactory._textureProvidertypes.TryGetValue(textureProviderName, out type))
			{
				try
				{
					TextureProvider textureProvider;
					if ((textureProvider = Activator.CreateInstance(type) as TextureProvider) != null)
					{
						return textureProvider;
					}
				}
				catch
				{
				}
			}
			Debug.FailedAssert("Unable to create instance for texture provider with name: " + textureProviderName, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\TextureProviderFactory.cs", "CreateInstance", 36);
			return null;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000F294 File Offset: 0x0000D494
		public static void RefreshProviderTypes()
		{
			TextureProviderFactory._textureProvidertypes.Clear();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				List<Type> typesSafe = assemblies[i].GetTypesSafe(null);
				for (int j = 0; j < typesSafe.Count; j++)
				{
					Type type = typesSafe[j];
					if (type == null)
					{
						Debug.FailedAssert("(RefreshProviderTypes): Null type while iterating assemblies", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\TextureProviderFactory.cs", "RefreshProviderTypes", 53);
					}
					else if (typeof(TextureProvider).IsAssignableFrom(type) && !type.IsAbstract)
					{
						TextureProviderFactory._textureProvidertypes.Add(type.Name, type);
					}
				}
			}
		}

		// Token: 0x040001AD RID: 429
		private static Dictionary<string, Type> _textureProvidertypes = new Dictionary<string, Type>();
	}
}
