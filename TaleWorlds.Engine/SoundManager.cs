using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000090 RID: 144
	public static class SoundManager
	{
		// Token: 0x06000CDA RID: 3290 RVA: 0x0000E71D File Offset: 0x0000C91D
		public static void SetListenerFrame(MatrixFrame frame)
		{
			EngineApplicationInterface.ISoundManager.SetListenerFrame(ref frame, ref frame.origin);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0000E732 File Offset: 0x0000C932
		public static void SetListenerFrame(MatrixFrame frame, Vec3 attenuationPosition)
		{
			EngineApplicationInterface.ISoundManager.SetListenerFrame(ref frame, ref attenuationPosition);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0000E744 File Offset: 0x0000C944
		public static MatrixFrame GetListenerFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.ISoundManager.GetListenerFrame(out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0000E760 File Offset: 0x0000C960
		public static Vec3 GetAttenuationPosition()
		{
			Vec3 vec;
			EngineApplicationInterface.ISoundManager.GetAttenuationPosition(out vec);
			return vec;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0000E77A File Offset: 0x0000C97A
		public static void Reset()
		{
			EngineApplicationInterface.ISoundManager.Reset();
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0000E786 File Offset: 0x0000C986
		public static bool StartOneShotEvent(string eventFullName, in Vec3 position, string paramName, float paramValue)
		{
			return EngineApplicationInterface.ISoundManager.StartOneShotEventWithParam(eventFullName, position, paramName, paramValue);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0000E79B File Offset: 0x0000C99B
		public static bool StartOneShotEvent(string eventFullName, in Vec3 position)
		{
			return EngineApplicationInterface.ISoundManager.StartOneShotEvent(eventFullName, position);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0000E7AE File Offset: 0x0000C9AE
		public static bool StartOneShotEventWithIndex(int index, in Vec3 position)
		{
			return EngineApplicationInterface.ISoundManager.StartOneShotEventWithIndex(index, position);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
		public static void SetState(string stateGroup, string state)
		{
			EngineApplicationInterface.ISoundManager.SetState(stateGroup, state);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0000E7CF File Offset: 0x0000C9CF
		public static SoundEvent CreateEvent(string eventFullName, Scene scene)
		{
			return SoundEvent.CreateEventFromString(eventFullName, scene);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		public static void LoadEventFileAux(string soundBank, bool decompressSamples)
		{
			if (!SoundManager._loaded)
			{
				EngineApplicationInterface.ISoundManager.LoadEventFileAux(soundBank, decompressSamples);
				SoundManager._loaded = true;
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0000E7F3 File Offset: 0x0000C9F3
		public static void AddSoundClientWithId(ulong clientId)
		{
			EngineApplicationInterface.ISoundManager.AddSoundClientWithId(clientId);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0000E800 File Offset: 0x0000CA00
		public static void DeleteSoundClientWithId(ulong clientId)
		{
			EngineApplicationInterface.ISoundManager.DeleteSoundClientWithId(clientId);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0000E80D File Offset: 0x0000CA0D
		public static void SetGlobalParameter(string parameterName, float value)
		{
			EngineApplicationInterface.ISoundManager.SetGlobalParameter(parameterName, value);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0000E81B File Offset: 0x0000CA1B
		public static int GetEventGlobalIndex(string eventFullName)
		{
			if (string.IsNullOrEmpty(eventFullName))
			{
				return -1;
			}
			return EngineApplicationInterface.ISoundManager.GetGlobalIndexOfEvent(eventFullName);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0000E832 File Offset: 0x0000CA32
		public static void PauseBus(string busName)
		{
			EngineApplicationInterface.ISoundManager.PauseBus(busName);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0000E83F File Offset: 0x0000CA3F
		public static void UnpauseBus(string busName)
		{
			EngineApplicationInterface.ISoundManager.UnpauseBus(busName);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0000E84C File Offset: 0x0000CA4C
		public static void InitializeVoicePlayEvent()
		{
			EngineApplicationInterface.ISoundManager.InitializeVoicePlayEvent();
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0000E858 File Offset: 0x0000CA58
		public static void CreateVoiceEvent()
		{
			EngineApplicationInterface.ISoundManager.CreateVoiceEvent();
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0000E864 File Offset: 0x0000CA64
		public static void DestroyVoiceEvent(int id)
		{
			EngineApplicationInterface.ISoundManager.DestroyVoiceEvent(id);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0000E871 File Offset: 0x0000CA71
		public static void FinalizeVoicePlayEvent()
		{
			EngineApplicationInterface.ISoundManager.FinalizeVoicePlayEvent();
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0000E87D File Offset: 0x0000CA7D
		public static void StartVoiceRecording()
		{
			EngineApplicationInterface.ISoundManager.StartVoiceRecord();
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0000E889 File Offset: 0x0000CA89
		public static void StopVoiceRecording()
		{
			EngineApplicationInterface.ISoundManager.StopVoiceRecord();
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0000E895 File Offset: 0x0000CA95
		public static void GetVoiceData(byte[] voiceBuffer, int chunkSize, out int readBytesLength)
		{
			readBytesLength = 0;
			EngineApplicationInterface.ISoundManager.GetVoiceData(voiceBuffer, chunkSize, ref readBytesLength);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0000E8A7 File Offset: 0x0000CAA7
		public static void UpdateVoiceToPlay(byte[] voiceBuffer, int length, int index)
		{
			EngineApplicationInterface.ISoundManager.UpdateVoiceToPlay(voiceBuffer, length, index);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0000E8B6 File Offset: 0x0000CAB6
		public static void AddXBOXRemoteUser(ulong XUID, ulong deviceID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			EngineApplicationInterface.ISoundManager.AddXBOXRemoteUser(XUID, deviceID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0000E8CC File Offset: 0x0000CACC
		public static void InitializeXBOXSoundManager()
		{
			EngineApplicationInterface.ISoundManager.InitializeXBOXSoundManager();
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
		public static void ApplyPushToTalk(bool pushed)
		{
			EngineApplicationInterface.ISoundManager.ApplyPushToTalk(pushed);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0000E8E5 File Offset: 0x0000CAE5
		public static void ClearXBOXSoundManager()
		{
			EngineApplicationInterface.ISoundManager.ClearXBOXSoundManager();
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0000E8F1 File Offset: 0x0000CAF1
		public static void UpdateXBOXLocalUser()
		{
			EngineApplicationInterface.ISoundManager.UpdateXBOXLocalUser();
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0000E8FD File Offset: 0x0000CAFD
		public static void UpdateXBOXChatCommunicationFlags(ulong XUID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			EngineApplicationInterface.ISoundManager.UpdateXBOXChatCommunicationFlags(XUID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0000E911 File Offset: 0x0000CB11
		public static void RemoveXBOXRemoteUser(ulong XUID)
		{
			EngineApplicationInterface.ISoundManager.RemoveXBOXRemoteUser(XUID);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0000E91E File Offset: 0x0000CB1E
		public static void ProcessDataToBeReceived(ulong senderDeviceID, byte[] data, uint dataSize)
		{
			EngineApplicationInterface.ISoundManager.ProcessDataToBeReceived(senderDeviceID, data, dataSize);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0000E92D File Offset: 0x0000CB2D
		public static void ProcessDataToBeSent(ref int numData)
		{
			EngineApplicationInterface.ISoundManager.ProcessDataToBeSent(ref numData);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0000E93A File Offset: 0x0000CB3A
		public static void HandleStateChanges()
		{
			EngineApplicationInterface.ISoundManager.HandleStateChanges();
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0000E946 File Offset: 0x0000CB46
		public static void GetSizeOfDataToBeSentAt(int index, ref uint byteCount, ref uint numReceivers)
		{
			EngineApplicationInterface.ISoundManager.GetSizeOfDataToBeSentAt(index, ref byteCount, ref numReceivers);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0000E955 File Offset: 0x0000CB55
		public static bool GetDataToBeSentAt(int index, byte[] buffer, ulong[] receivers, ref bool transportGuaranteed)
		{
			return EngineApplicationInterface.ISoundManager.GetDataToBeSentAt(index, buffer, receivers, ref transportGuaranteed);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0000E965 File Offset: 0x0000CB65
		public static void ClearDataToBeSent()
		{
			EngineApplicationInterface.ISoundManager.ClearDataToBeSent();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0000E971 File Offset: 0x0000CB71
		public static void CompressData(int clientID, byte[] buffer, int length, byte[] compressedBuffer, out int compressedBufferLength)
		{
			compressedBufferLength = 0;
			EngineApplicationInterface.ISoundManager.CompressData((ulong)((long)clientID), buffer, length, compressedBuffer, ref compressedBufferLength);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0000E988 File Offset: 0x0000CB88
		public static void DecompressData(int clientID, byte[] compressedBuffer, int compressedBufferLength, byte[] decompressedBuffer, out int decompressedBufferLength)
		{
			decompressedBufferLength = 0;
			EngineApplicationInterface.ISoundManager.DecompressData((ulong)((long)clientID), compressedBuffer, compressedBufferLength, decompressedBuffer, ref decompressedBufferLength);
		}

		// Token: 0x040001CC RID: 460
		private static bool _loaded;
	}
}
