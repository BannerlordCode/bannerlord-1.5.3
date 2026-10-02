using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000026 RID: 38
	public static class ServerFailureReason
	{
		// Token: 0x0400003D RID: 61
		public const string SessionNotFound = "SessionNotFound";

		// Token: 0x0400003E RID: 62
		public const string InvalidCredentials = "InvalidCredentials";

		// Token: 0x0400003F RID: 63
		public const string InvalidCertificate = "InvalidCertificate";

		// Token: 0x04000040 RID: 64
		public const string UnknownMessageType = "UnknownMessageType";

		// Token: 0x04000041 RID: 65
		public const string FeatureNotSupported = "FeatureNotSupported";

		// Token: 0x04000042 RID: 66
		public const string PeerTypeMismatch = "PeerTypeMismatch";

		// Token: 0x04000043 RID: 67
		public const string ServerError = "ServerError";

		// Token: 0x04000044 RID: 68
		public const string HandlerFailed = "HandlerFailed";
	}
}
