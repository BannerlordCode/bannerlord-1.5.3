using System;
using System.IO;
using TaleWorlds.Library;

namespace psai.net
{
	// Token: 0x02000015 RID: 21
	internal class PlatformLayerStandalone : IPlatformLayer
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00008F64 File Offset: 0x00007164
		public PlatformLayerStandalone(Logik logik)
		{
			this.m_logik = logik;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008F73 File Offset: 0x00007173
		public void Initialize()
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008F75 File Offset: 0x00007175
		public void Release()
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008F78 File Offset: 0x00007178
		public string ConvertFilePathForPlatform(string filepath)
		{
			string text = filepath.Replace('\\', '/');
			string text2 = "";
			string text3;
			if (text.Contains("/"))
			{
				text2 = Path.GetDirectoryName(text) + "/";
				text3 = Path.GetFileNameWithoutExtension(text);
			}
			else
			{
				text3 = Path.GetFileNameWithoutExtension(text);
			}
			if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
			{
				return text2 + "PS4/" + text3 + ".fsb";
			}
			if (ApplicationPlatform.CurrentPlatform == Platform.Durango)
			{
				return text2 + "XboxOne/" + text3 + ".fsb";
			}
			return text2 + "PC/" + text3 + ".ogg";
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000900F File Offset: 0x0000720F
		public Stream GetStreamOnPsaiSoundtrackFile(string filepath)
		{
			if (Logik.CheckIfFileExists(filepath))
			{
				return File.OpenRead(filepath);
			}
			return null;
		}

		// Token: 0x040000EC RID: 236
		private Logik m_logik;
	}
}
