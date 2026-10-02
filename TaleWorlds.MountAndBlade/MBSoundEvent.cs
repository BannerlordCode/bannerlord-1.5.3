using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E2 RID: 482
	public static class MBSoundEvent
	{
		// Token: 0x06001C8B RID: 7307 RVA: 0x00061FFF File Offset: 0x000601FF
		public static bool PlaySound(int soundCodeId, in Vec3 position)
		{
			return MBAPI.IMBSoundEvent.PlaySound(soundCodeId, in position);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00062010 File Offset: 0x00060210
		public static bool PlaySound(int soundCodeId, Vec3 position)
		{
			Vec3 vec = position;
			return MBAPI.IMBSoundEvent.PlaySound(soundCodeId, in vec);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0006202C File Offset: 0x0006022C
		public static bool PlaySound(int soundCodeId, ref SoundEventParameter parameter, Vec3 position)
		{
			Vec3 vec = position;
			return MBSoundEvent.PlaySound(soundCodeId, ref parameter, in vec);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00062044 File Offset: 0x00060244
		public static bool PlaySound(string soundPath, ref SoundEventParameter parameter, Vec3 position)
		{
			int eventIdFromString = SoundEvent.GetEventIdFromString(soundPath);
			Vec3 vec = position;
			return MBSoundEvent.PlaySound(eventIdFromString, ref parameter, in vec);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00062061 File Offset: 0x00060261
		public static bool PlaySound(int soundCodeId, ref SoundEventParameter parameter, in Vec3 position)
		{
			return MBAPI.IMBSoundEvent.PlaySoundWithParam(soundCodeId, parameter, in position);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00062075 File Offset: 0x00060275
		public static void PlayEventFromSoundBuffer(string eventId, byte[] soundData, Scene scene, bool is3d, bool isBlocking)
		{
			MBAPI.IMBSoundEvent.CreateEventFromSoundBuffer(eventId, soundData, (scene != null) ? scene.Pointer : UIntPtr.Zero, is3d, isBlocking);
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0006209D File Offset: 0x0006029D
		public static void CreateEventFromExternalFile(string programmerEventName, string soundFilePath, Scene scene, bool is3d, bool isBlocking)
		{
			MBAPI.IMBSoundEvent.CreateEventFromExternalFile(programmerEventName, soundFilePath, (scene != null) ? scene.Pointer : UIntPtr.Zero, is3d, isBlocking);
		}
	}
}
