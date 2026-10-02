using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TaleWorlds.Library
{
	// Token: 0x020000A6 RID: 166
	public class VirtualFolders
	{
		// Token: 0x06000661 RID: 1633 RVA: 0x000164EC File Offset: 0x000146EC
		public static string GetFileContent(string filePath, Type type = null)
		{
			if (VirtualFolders._useVirtualFolders)
			{
				if (type == null)
				{
					type = typeof(VirtualFolders);
				}
				return VirtualFolders.GetVirtualFileContent(filePath, type);
			}
			if (filePath.Contains("__MODULE_NAME__"))
			{
				string text = "__MODULE_NAME__";
				string text2 = Regex.Escape(text) + "(.*?)" + Regex.Escape(text);
				string value = Regex.Match(filePath, text2).Groups[1].Value;
				filePath = filePath.Replace(text + value + text, VirtualFolders.PlatformDLCPaths[value]);
			}
			if (!File.Exists(filePath))
			{
				return "";
			}
			return File.ReadAllText(filePath);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00016590 File Offset: 0x00014790
		private static string GetVirtualFileContent(string filePath, Type type)
		{
			string fileName = Path.GetFileName(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			Type type2 = VirtualFolders.GetNestedDirectory(directoryName, type);
			if (type2 == null)
			{
				type2 = type;
				string[] array = directoryName.Split(new char[] { Path.DirectorySeparatorChar });
				int num = 0;
				while (type2 != null && num != array.Length)
				{
					if (!string.IsNullOrEmpty(array[num]))
					{
						type2 = VirtualFolders.GetNestedDirectory(array[num], type2);
					}
					num++;
				}
			}
			if (type2 != null)
			{
				FieldInfo[] fields = type2.GetFields();
				for (int i = 0; i < fields.Length; i++)
				{
					VirtualFileAttribute[] array2 = (VirtualFileAttribute[])fields[i].GetCustomAttributesSafe(typeof(VirtualFileAttribute), false);
					if (array2[0].Name == fileName)
					{
						return array2[0].Content;
					}
				}
			}
			return "";
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00016668 File Offset: 0x00014868
		private static Type GetNestedDirectory(string name, Type type)
		{
			foreach (Type type2 in type.GetNestedTypes())
			{
				if (((VirtualDirectoryAttribute[])type2.GetCustomAttributesSafe(typeof(VirtualDirectoryAttribute), false))[0].Name == name)
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x040001E4 RID: 484
		private static readonly bool _useVirtualFolders = true;

		// Token: 0x040001E5 RID: 485
		public static Dictionary<string, string> PlatformDLCPaths = new Dictionary<string, string>();

		// Token: 0x020000F3 RID: 243
		[VirtualDirectory("..")]
		public class Win64_Shipping_Client
		{
			// Token: 0x020000FE RID: 254
			[VirtualDirectory("..")]
			public class bin
			{
				// Token: 0x020000FF RID: 255
				[VirtualDirectory("Parameters")]
				public class Parameters
				{
					// Token: 0x0400034F RID: 847
					[VirtualFile("Environment", "c8zjpsOgWM5qHxBGAQn2j9A6p2N1am3g_hdoztXRJtVN9oehPh2iv.ICwSKKjM72dgFE1smT5iOdI0JR49dOM3CmKHSLinEat68xq6WvObumXlmkWGJqC_yDUzAsR91CS2PQE9_duxMbDoMRMDSy79SviyHQol6VNkbED_4Yfzc-")]
					public string Environment;

					// Token: 0x04000350 RID: 848
					[VirtualFile("Version.xml", "<Version>\t<Singleplayer Value=\"v1.5.3.122374\"/></Version> ")]
					public string Version;

					// Token: 0x04000351 RID: 849
					[VirtualFile("ClientProfile.xml", "<ClientProfile Value=\"Azure.Discovery\" />")]
					public string ClientProfile;

					// Token: 0x02000100 RID: 256
					[VirtualDirectory("ClientProfiles")]
					public class ClientProfiles
					{
						// Token: 0x02000101 RID: 257
						[VirtualDirectory("Azure.Discovery")]
						public class AzureDiscovery
						{
							// Token: 0x04000352 RID: 850
							[VirtualFile("LobbyClient.xml", "<Configuration>\t<SessionProvider Type=\"ThreadedRest\" />\t<Clients>\t\t<Client Type=\"LobbyClient\" />\t</Clients>\t<Parameters>\t\t<Parameter Name=\"LobbyClient.ServiceDiscovery.Address\" Value=\"https://bannerlord-service-discovery.bannerlord-services-3.net/\" />\t\t<Parameter Name=\"LobbyClient.Address\" Value=\"service://bannerlord.lobby/\" />\t</Parameters></Configuration>")]
							public string LobbyClient;
						}
					}
				}
			}
		}
	}
}
