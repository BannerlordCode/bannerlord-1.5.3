using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B8 RID: 440
	[ScriptingInterfaceBase]
	internal interface IMBSoundEvent
	{
		// Token: 0x0600191C RID: 6428
		[EngineMethod("create_event_from_external_file", false, null, false)]
		int CreateEventFromExternalFile(string programmerSoundEventName, string filePath, UIntPtr scene, bool is3d, bool isBlocking);

		// Token: 0x0600191D RID: 6429
		[EngineMethod("create_event_from_sound_buffer", false, null, false)]
		int CreateEventFromSoundBuffer(string programmerSoundEventName, byte[] soundBuffer, UIntPtr scene, bool is3d, bool isBlocking);

		// Token: 0x0600191E RID: 6430
		[EngineMethod("play_sound", false, null, false)]
		bool PlaySound(int fmodEventIndex, in Vec3 position);

		// Token: 0x0600191F RID: 6431
		[EngineMethod("play_sound_with_int_param", false, null, false)]
		bool PlaySoundWithIntParam(int fmodEventIndex, int paramIndex, float paramVal, in Vec3 position);

		// Token: 0x06001920 RID: 6432
		[EngineMethod("play_sound_with_str_param", false, null, false)]
		bool PlaySoundWithStrParam(int fmodEventIndex, string paramName, float paramVal, in Vec3 position);

		// Token: 0x06001921 RID: 6433
		[EngineMethod("play_sound_with_param", false, null, false)]
		bool PlaySoundWithParam(int soundCodeId, SoundEventParameter parameter, in Vec3 position);
	}
}
