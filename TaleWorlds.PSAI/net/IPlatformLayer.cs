using System;
using System.IO;

namespace psai.net
{
	// Token: 0x0200000F RID: 15
	internal interface IPlatformLayer
	{
		// Token: 0x0600013F RID: 319
		void Initialize();

		// Token: 0x06000140 RID: 320
		void Release();

		// Token: 0x06000141 RID: 321
		Stream GetStreamOnPsaiSoundtrackFile(string filename);

		// Token: 0x06000142 RID: 322
		string ConvertFilePathForPlatform(string filepath);
	}
}
