using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000028 RID: 40
	internal class ScriptingInterfaceOfISoundEvent : ISoundEvent
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x00018CBD File Offset: 0x00016EBD
		public int CreateEvent(int fmodEventIndex, UIntPtr scene)
		{
			return ScriptingInterfaceOfISoundEvent.call_CreateEventDelegate(fmodEventIndex, scene);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00018CCC File Offset: 0x00016ECC
		public int CreateEventFromExternalFile(string programmerSoundEventName, string filePath, UIntPtr scene, bool is3d, bool isBlocking)
		{
			byte[] array = null;
			if (programmerSoundEventName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(programmerSoundEventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(programmerSoundEventName, 0, programmerSoundEventName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (filePath != null)
			{
				int byteCount2 = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(filePath);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(filePath, 0, filePath.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfISoundEvent.call_CreateEventFromExternalFileDelegate(array, array2, scene, is3d, isBlocking);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00018D70 File Offset: 0x00016F70
		public int CreateEventFromSoundBuffer(string programmerSoundEventName, byte[] soundBuffer, UIntPtr scene, bool is3d, bool isBlocking)
		{
			byte[] array = null;
			if (programmerSoundEventName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(programmerSoundEventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(programmerSoundEventName, 0, programmerSoundEventName.Length, array, 0);
				array[byteCount] = 0;
			}
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(soundBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (soundBuffer != null) ? soundBuffer.Length : 0);
			int num = ScriptingInterfaceOfISoundEvent.call_CreateEventFromSoundBufferDelegate(array, managedArray, scene, is3d, isBlocking);
			pinnedArrayData.Dispose();
			return num;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00018E00 File Offset: 0x00017000
		public int CreateEventFromString(string eventName, UIntPtr scene)
		{
			byte[] array = null;
			if (eventName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(eventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(eventName, 0, eventName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISoundEvent.call_CreateEventFromStringDelegate(array, scene);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00018E5C File Offset: 0x0001705C
		public int GetEventIdFromString(string eventName)
		{
			byte[] array = null;
			if (eventName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(eventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(eventName, 0, eventName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISoundEvent.call_GetEventIdFromStringDelegate(array);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00018EB6 File Offset: 0x000170B6
		public Vec3 GetEventMinMaxDistance(int eventId)
		{
			return ScriptingInterfaceOfISoundEvent.call_GetEventMinMaxDistanceDelegate(eventId);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00018EC3 File Offset: 0x000170C3
		public int GetTotalEventCount()
		{
			return ScriptingInterfaceOfISoundEvent.call_GetTotalEventCountDelegate();
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00018ECF File Offset: 0x000170CF
		public bool IsPaused(int eventId)
		{
			return ScriptingInterfaceOfISoundEvent.call_IsPausedDelegate(eventId);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00018EDC File Offset: 0x000170DC
		public bool IsPlaying(int eventId)
		{
			return ScriptingInterfaceOfISoundEvent.call_IsPlayingDelegate(eventId);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00018EE9 File Offset: 0x000170E9
		public bool IsStopped(int eventId)
		{
			return ScriptingInterfaceOfISoundEvent.call_IsStoppedDelegate(eventId);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00018EF6 File Offset: 0x000170F6
		public bool IsValid(int eventId)
		{
			return ScriptingInterfaceOfISoundEvent.call_IsValidDelegate(eventId);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00018F03 File Offset: 0x00017103
		public void PauseEvent(int eventId)
		{
			ScriptingInterfaceOfISoundEvent.call_PauseEventDelegate(eventId);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00018F10 File Offset: 0x00017110
		public void PlayExtraEvent(int soundId, string eventName)
		{
			byte[] array = null;
			if (eventName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(eventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(eventName, 0, eventName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundEvent.call_PlayExtraEventDelegate(soundId, array);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00018F6B File Offset: 0x0001716B
		public bool PlaySound2D(int fmodEventIndex)
		{
			return ScriptingInterfaceOfISoundEvent.call_PlaySound2DDelegate(fmodEventIndex);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00018F78 File Offset: 0x00017178
		public void ReleaseEvent(int eventId)
		{
			ScriptingInterfaceOfISoundEvent.call_ReleaseEventDelegate(eventId);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00018F85 File Offset: 0x00017185
		public void ResumeEvent(int eventId)
		{
			ScriptingInterfaceOfISoundEvent.call_ResumeEventDelegate(eventId);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00018F92 File Offset: 0x00017192
		public void SetEventMinMaxDistance(int fmodEventIndex, Vec3 radius)
		{
			ScriptingInterfaceOfISoundEvent.call_SetEventMinMaxDistanceDelegate(fmodEventIndex, radius);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00018FA0 File Offset: 0x000171A0
		public void SetEventParameterAtIndex(int soundId, int parameterIndex, float value)
		{
			ScriptingInterfaceOfISoundEvent.call_SetEventParameterAtIndexDelegate(soundId, parameterIndex, value);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00018FB0 File Offset: 0x000171B0
		public void SetEventParameterFromString(int eventId, string name, float value)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundEvent.call_SetEventParameterFromStringDelegate(eventId, array, value);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001900C File Offset: 0x0001720C
		public void SetEventPosition(int eventId, ref Vec3 position)
		{
			ScriptingInterfaceOfISoundEvent.call_SetEventPositionDelegate(eventId, ref position);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001901A File Offset: 0x0001721A
		public void SetEventVelocity(int eventId, ref Vec3 velocity)
		{
			ScriptingInterfaceOfISoundEvent.call_SetEventVelocityDelegate(eventId, ref velocity);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00019028 File Offset: 0x00017228
		public void SetSwitch(int soundId, string switchGroupName, string newSwitchStateName)
		{
			byte[] array = null;
			if (switchGroupName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(switchGroupName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(switchGroupName, 0, switchGroupName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (newSwitchStateName != null)
			{
				int byteCount2 = ScriptingInterfaceOfISoundEvent._utf8.GetByteCount(newSwitchStateName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISoundEvent._utf8.GetBytes(newSwitchStateName, 0, newSwitchStateName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfISoundEvent.call_SetSwitchDelegate(soundId, array, array2);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000190C6 File Offset: 0x000172C6
		public bool StartEvent(int eventId)
		{
			return ScriptingInterfaceOfISoundEvent.call_StartEventDelegate(eventId);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000190D3 File Offset: 0x000172D3
		public bool StartEventInPosition(int eventId, ref Vec3 position)
		{
			return ScriptingInterfaceOfISoundEvent.call_StartEventInPositionDelegate(eventId, ref position);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000190E1 File Offset: 0x000172E1
		public void StopEvent(int eventId)
		{
			ScriptingInterfaceOfISoundEvent.call_StopEventDelegate(eventId);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000190EE File Offset: 0x000172EE
		public void TriggerCue(int eventId)
		{
			ScriptingInterfaceOfISoundEvent.call_TriggerCueDelegate(eventId);
		}

		// Token: 0x0400050B RID: 1291
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400050C RID: 1292
		public static ScriptingInterfaceOfISoundEvent.CreateEventDelegate call_CreateEventDelegate;

		// Token: 0x0400050D RID: 1293
		public static ScriptingInterfaceOfISoundEvent.CreateEventFromExternalFileDelegate call_CreateEventFromExternalFileDelegate;

		// Token: 0x0400050E RID: 1294
		public static ScriptingInterfaceOfISoundEvent.CreateEventFromSoundBufferDelegate call_CreateEventFromSoundBufferDelegate;

		// Token: 0x0400050F RID: 1295
		public static ScriptingInterfaceOfISoundEvent.CreateEventFromStringDelegate call_CreateEventFromStringDelegate;

		// Token: 0x04000510 RID: 1296
		public static ScriptingInterfaceOfISoundEvent.GetEventIdFromStringDelegate call_GetEventIdFromStringDelegate;

		// Token: 0x04000511 RID: 1297
		public static ScriptingInterfaceOfISoundEvent.GetEventMinMaxDistanceDelegate call_GetEventMinMaxDistanceDelegate;

		// Token: 0x04000512 RID: 1298
		public static ScriptingInterfaceOfISoundEvent.GetTotalEventCountDelegate call_GetTotalEventCountDelegate;

		// Token: 0x04000513 RID: 1299
		public static ScriptingInterfaceOfISoundEvent.IsPausedDelegate call_IsPausedDelegate;

		// Token: 0x04000514 RID: 1300
		public static ScriptingInterfaceOfISoundEvent.IsPlayingDelegate call_IsPlayingDelegate;

		// Token: 0x04000515 RID: 1301
		public static ScriptingInterfaceOfISoundEvent.IsStoppedDelegate call_IsStoppedDelegate;

		// Token: 0x04000516 RID: 1302
		public static ScriptingInterfaceOfISoundEvent.IsValidDelegate call_IsValidDelegate;

		// Token: 0x04000517 RID: 1303
		public static ScriptingInterfaceOfISoundEvent.PauseEventDelegate call_PauseEventDelegate;

		// Token: 0x04000518 RID: 1304
		public static ScriptingInterfaceOfISoundEvent.PlayExtraEventDelegate call_PlayExtraEventDelegate;

		// Token: 0x04000519 RID: 1305
		public static ScriptingInterfaceOfISoundEvent.PlaySound2DDelegate call_PlaySound2DDelegate;

		// Token: 0x0400051A RID: 1306
		public static ScriptingInterfaceOfISoundEvent.ReleaseEventDelegate call_ReleaseEventDelegate;

		// Token: 0x0400051B RID: 1307
		public static ScriptingInterfaceOfISoundEvent.ResumeEventDelegate call_ResumeEventDelegate;

		// Token: 0x0400051C RID: 1308
		public static ScriptingInterfaceOfISoundEvent.SetEventMinMaxDistanceDelegate call_SetEventMinMaxDistanceDelegate;

		// Token: 0x0400051D RID: 1309
		public static ScriptingInterfaceOfISoundEvent.SetEventParameterAtIndexDelegate call_SetEventParameterAtIndexDelegate;

		// Token: 0x0400051E RID: 1310
		public static ScriptingInterfaceOfISoundEvent.SetEventParameterFromStringDelegate call_SetEventParameterFromStringDelegate;

		// Token: 0x0400051F RID: 1311
		public static ScriptingInterfaceOfISoundEvent.SetEventPositionDelegate call_SetEventPositionDelegate;

		// Token: 0x04000520 RID: 1312
		public static ScriptingInterfaceOfISoundEvent.SetEventVelocityDelegate call_SetEventVelocityDelegate;

		// Token: 0x04000521 RID: 1313
		public static ScriptingInterfaceOfISoundEvent.SetSwitchDelegate call_SetSwitchDelegate;

		// Token: 0x04000522 RID: 1314
		public static ScriptingInterfaceOfISoundEvent.StartEventDelegate call_StartEventDelegate;

		// Token: 0x04000523 RID: 1315
		public static ScriptingInterfaceOfISoundEvent.StartEventInPositionDelegate call_StartEventInPositionDelegate;

		// Token: 0x04000524 RID: 1316
		public static ScriptingInterfaceOfISoundEvent.StopEventDelegate call_StopEventDelegate;

		// Token: 0x04000525 RID: 1317
		public static ScriptingInterfaceOfISoundEvent.TriggerCueDelegate call_TriggerCueDelegate;

		// Token: 0x02000573 RID: 1395
		// (Invoke) Token: 0x06001BF7 RID: 7159
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CreateEventDelegate(int fmodEventIndex, UIntPtr scene);

		// Token: 0x02000574 RID: 1396
		// (Invoke) Token: 0x06001BFB RID: 7163
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CreateEventFromExternalFileDelegate(byte[] programmerSoundEventName, byte[] filePath, UIntPtr scene, [MarshalAs(UnmanagedType.U1)] bool is3d, [MarshalAs(UnmanagedType.U1)] bool isBlocking);

		// Token: 0x02000575 RID: 1397
		// (Invoke) Token: 0x06001BFF RID: 7167
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CreateEventFromSoundBufferDelegate(byte[] programmerSoundEventName, ManagedArray soundBuffer, UIntPtr scene, [MarshalAs(UnmanagedType.U1)] bool is3d, [MarshalAs(UnmanagedType.U1)] bool isBlocking);

		// Token: 0x02000576 RID: 1398
		// (Invoke) Token: 0x06001C03 RID: 7171
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CreateEventFromStringDelegate(byte[] eventName, UIntPtr scene);

		// Token: 0x02000577 RID: 1399
		// (Invoke) Token: 0x06001C07 RID: 7175
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetEventIdFromStringDelegate(byte[] eventName);

		// Token: 0x02000578 RID: 1400
		// (Invoke) Token: 0x06001C0B RID: 7179
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetEventMinMaxDistanceDelegate(int eventId);

		// Token: 0x02000579 RID: 1401
		// (Invoke) Token: 0x06001C0F RID: 7183
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetTotalEventCountDelegate();

		// Token: 0x0200057A RID: 1402
		// (Invoke) Token: 0x06001C13 RID: 7187
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsPausedDelegate(int eventId);

		// Token: 0x0200057B RID: 1403
		// (Invoke) Token: 0x06001C17 RID: 7191
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsPlayingDelegate(int eventId);

		// Token: 0x0200057C RID: 1404
		// (Invoke) Token: 0x06001C1B RID: 7195
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsStoppedDelegate(int eventId);

		// Token: 0x0200057D RID: 1405
		// (Invoke) Token: 0x06001C1F RID: 7199
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsValidDelegate(int eventId);

		// Token: 0x0200057E RID: 1406
		// (Invoke) Token: 0x06001C23 RID: 7203
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseEventDelegate(int eventId);

		// Token: 0x0200057F RID: 1407
		// (Invoke) Token: 0x06001C27 RID: 7207
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PlayExtraEventDelegate(int soundId, byte[] eventName);

		// Token: 0x02000580 RID: 1408
		// (Invoke) Token: 0x06001C2B RID: 7211
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool PlaySound2DDelegate(int fmodEventIndex);

		// Token: 0x02000581 RID: 1409
		// (Invoke) Token: 0x06001C2F RID: 7215
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseEventDelegate(int eventId);

		// Token: 0x02000582 RID: 1410
		// (Invoke) Token: 0x06001C33 RID: 7219
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResumeEventDelegate(int eventId);

		// Token: 0x02000583 RID: 1411
		// (Invoke) Token: 0x06001C37 RID: 7223
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEventMinMaxDistanceDelegate(int fmodEventIndex, Vec3 radius);

		// Token: 0x02000584 RID: 1412
		// (Invoke) Token: 0x06001C3B RID: 7227
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEventParameterAtIndexDelegate(int soundId, int parameterIndex, float value);

		// Token: 0x02000585 RID: 1413
		// (Invoke) Token: 0x06001C3F RID: 7231
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEventParameterFromStringDelegate(int eventId, byte[] name, float value);

		// Token: 0x02000586 RID: 1414
		// (Invoke) Token: 0x06001C43 RID: 7235
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEventPositionDelegate(int eventId, ref Vec3 position);

		// Token: 0x02000587 RID: 1415
		// (Invoke) Token: 0x06001C47 RID: 7239
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEventVelocityDelegate(int eventId, ref Vec3 velocity);

		// Token: 0x02000588 RID: 1416
		// (Invoke) Token: 0x06001C4B RID: 7243
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSwitchDelegate(int soundId, byte[] switchGroupName, byte[] newSwitchStateName);

		// Token: 0x02000589 RID: 1417
		// (Invoke) Token: 0x06001C4F RID: 7247
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartEventDelegate(int eventId);

		// Token: 0x0200058A RID: 1418
		// (Invoke) Token: 0x06001C53 RID: 7251
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartEventInPositionDelegate(int eventId, ref Vec3 position);

		// Token: 0x0200058B RID: 1419
		// (Invoke) Token: 0x06001C57 RID: 7255
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StopEventDelegate(int eventId);

		// Token: 0x0200058C RID: 1420
		// (Invoke) Token: 0x06001C5B RID: 7259
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TriggerCueDelegate(int eventId);
	}
}
