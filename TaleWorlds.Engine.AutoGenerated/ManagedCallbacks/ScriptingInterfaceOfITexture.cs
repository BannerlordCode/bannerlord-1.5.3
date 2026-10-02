using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200002C RID: 44
	internal class ScriptingInterfaceOfITexture : ITexture
	{
		// Token: 0x0600060F RID: 1551 RVA: 0x00019CA8 File Offset: 0x00017EA8
		public Texture CheckAndGetFromResource(string textureName)
		{
			byte[] array = null;
			if (textureName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(textureName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(textureName, 0, textureName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CheckAndGetFromResourceDelegate(array);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00019D34 File Offset: 0x00017F34
		public Texture CreateDepthTarget(string name, int width, int height)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateDepthTargetDelegate(array, width, height);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00019DC4 File Offset: 0x00017FC4
		public Texture CreateFromByteArray(byte[] data, int width, int height)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (data != null) ? data.Length : 0);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateFromByteArrayDelegate(managedArray, width, height);
			pinnedArrayData.Dispose();
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00019E3C File Offset: 0x0001803C
		public Texture CreateFromMemory(byte[] data)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (data != null) ? data.Length : 0);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateFromMemoryDelegate(managedArray);
			pinnedArrayData.Dispose();
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00019EB4 File Offset: 0x000180B4
		public Texture CreateRenderTarget(string name, int width, int height, bool autoMipmaps, bool isTableau, bool createUninitialized, bool always_valid)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateRenderTargetDelegate(array, width, height, autoMipmaps, isTableau, createUninitialized, always_valid);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00019F4C File Offset: 0x0001814C
		public Texture CreateTextureFromPath(PlatformFilePath filePath)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateTextureFromPathDelegate(filePath);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00019F96 File Offset: 0x00018196
		public void GetCurObject(UIntPtr texturePointer, bool blocking)
		{
			ScriptingInterfaceOfITexture.call_GetCurObjectDelegate(texturePointer, blocking);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00019FA4 File Offset: 0x000181A4
		public Texture GetFromResource(string textureName)
		{
			byte[] array = null;
			if (textureName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(textureName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(textureName, 0, textureName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_GetFromResourceDelegate(array);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001A030 File Offset: 0x00018230
		public int GetHeight(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_GetHeightDelegate(texturePointer);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001A03D File Offset: 0x0001823D
		public int GetMemorySize(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_GetMemorySizeDelegate(texturePointer);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001A04A File Offset: 0x0001824A
		public string GetName(UIntPtr texturePointer)
		{
			if (ScriptingInterfaceOfITexture.call_GetNameDelegate(texturePointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001A064 File Offset: 0x00018264
		public void GetPixelData(UIntPtr texturePointer, byte[] bytes)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(bytes, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (bytes != null) ? bytes.Length : 0);
			ScriptingInterfaceOfITexture.call_GetPixelDataDelegate(texturePointer, managedArray);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001A0A6 File Offset: 0x000182A6
		public RenderTargetComponent GetRenderTargetComponent(UIntPtr texturePointer)
		{
			return DotNetObject.GetManagedObjectWithId(ScriptingInterfaceOfITexture.call_GetRenderTargetComponentDelegate(texturePointer)) as RenderTargetComponent;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001A0BD File Offset: 0x000182BD
		public void GetSDFBoundingBoxData(UIntPtr texturePointer, ref Vec3 min, ref Vec3 max)
		{
			ScriptingInterfaceOfITexture.call_GetSDFBoundingBoxDataDelegate(texturePointer, ref min, ref max);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001A0CC File Offset: 0x000182CC
		public TableauView GetTableauView(UIntPtr texturePointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_GetTableauViewDelegate(texturePointer);
			TableauView tableauView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				tableauView = new TableauView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return tableauView;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001A116 File Offset: 0x00018316
		public int GetWidth(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_GetWidthDelegate(texturePointer);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001A123 File Offset: 0x00018323
		public bool IsLoaded(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_IsLoadedDelegate(texturePointer);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001A130 File Offset: 0x00018330
		public bool IsRenderTarget(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_IsRenderTargetDelegate(texturePointer);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001A140 File Offset: 0x00018340
		public Texture LoadTextureFromPath(string fileName, string folder)
		{
			byte[] array = null;
			if (fileName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(fileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(fileName, 0, fileName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (folder != null)
			{
				int byteCount2 = ScriptingInterfaceOfITexture._utf8.GetByteCount(folder);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(folder, 0, folder.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_LoadTextureFromPathDelegate(array, array2);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001A217 File Offset: 0x00018417
		public void Release(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_ReleaseDelegate(texturePointer);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001A224 File Offset: 0x00018424
		public void ReleaseAfterNumberOfFrames(UIntPtr texturePointer, int numberOfFrames)
		{
			ScriptingInterfaceOfITexture.call_ReleaseAfterNumberOfFramesDelegate(texturePointer, numberOfFrames);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001A232 File Offset: 0x00018432
		public void ReleaseGpuMemories()
		{
			ScriptingInterfaceOfITexture.call_ReleaseGpuMemoriesDelegate();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001A23E File Offset: 0x0001843E
		public void ReleaseNextFrame(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_ReleaseNextFrameDelegate(texturePointer);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001A24B File Offset: 0x0001844B
		public void RemoveContinousTableauTexture(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_RemoveContinousTableauTextureDelegate(texturePointer);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001A258 File Offset: 0x00018458
		public void SaveTextureAsAlwaysValid(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_SaveTextureAsAlwaysValidDelegate(texturePointer);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001A268 File Offset: 0x00018468
		public void SaveToFile(UIntPtr texturePointer, string fileName, bool isRelativePath)
		{
			byte[] array = null;
			if (fileName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(fileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(fileName, 0, fileName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITexture.call_SaveToFileDelegate(texturePointer, array, isRelativePath);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001A2C4 File Offset: 0x000184C4
		public void SetName(UIntPtr texturePointer, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITexture.call_SetNameDelegate(texturePointer, array);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001A31F File Offset: 0x0001851F
		public void SetTableauView(UIntPtr texturePointer, UIntPtr tableauView)
		{
			ScriptingInterfaceOfITexture.call_SetTableauViewDelegate(texturePointer, tableauView);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001A330 File Offset: 0x00018530
		public void TransformRenderTargetToResourceTexture(UIntPtr texturePointer, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITexture.call_TransformRenderTargetToResourceTextureDelegate(texturePointer, array);
		}

		// Token: 0x04000560 RID: 1376
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000561 RID: 1377
		public static ScriptingInterfaceOfITexture.CheckAndGetFromResourceDelegate call_CheckAndGetFromResourceDelegate;

		// Token: 0x04000562 RID: 1378
		public static ScriptingInterfaceOfITexture.CreateDepthTargetDelegate call_CreateDepthTargetDelegate;

		// Token: 0x04000563 RID: 1379
		public static ScriptingInterfaceOfITexture.CreateFromByteArrayDelegate call_CreateFromByteArrayDelegate;

		// Token: 0x04000564 RID: 1380
		public static ScriptingInterfaceOfITexture.CreateFromMemoryDelegate call_CreateFromMemoryDelegate;

		// Token: 0x04000565 RID: 1381
		public static ScriptingInterfaceOfITexture.CreateRenderTargetDelegate call_CreateRenderTargetDelegate;

		// Token: 0x04000566 RID: 1382
		public static ScriptingInterfaceOfITexture.CreateTextureFromPathDelegate call_CreateTextureFromPathDelegate;

		// Token: 0x04000567 RID: 1383
		public static ScriptingInterfaceOfITexture.GetCurObjectDelegate call_GetCurObjectDelegate;

		// Token: 0x04000568 RID: 1384
		public static ScriptingInterfaceOfITexture.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x04000569 RID: 1385
		public static ScriptingInterfaceOfITexture.GetHeightDelegate call_GetHeightDelegate;

		// Token: 0x0400056A RID: 1386
		public static ScriptingInterfaceOfITexture.GetMemorySizeDelegate call_GetMemorySizeDelegate;

		// Token: 0x0400056B RID: 1387
		public static ScriptingInterfaceOfITexture.GetNameDelegate call_GetNameDelegate;

		// Token: 0x0400056C RID: 1388
		public static ScriptingInterfaceOfITexture.GetPixelDataDelegate call_GetPixelDataDelegate;

		// Token: 0x0400056D RID: 1389
		public static ScriptingInterfaceOfITexture.GetRenderTargetComponentDelegate call_GetRenderTargetComponentDelegate;

		// Token: 0x0400056E RID: 1390
		public static ScriptingInterfaceOfITexture.GetSDFBoundingBoxDataDelegate call_GetSDFBoundingBoxDataDelegate;

		// Token: 0x0400056F RID: 1391
		public static ScriptingInterfaceOfITexture.GetTableauViewDelegate call_GetTableauViewDelegate;

		// Token: 0x04000570 RID: 1392
		public static ScriptingInterfaceOfITexture.GetWidthDelegate call_GetWidthDelegate;

		// Token: 0x04000571 RID: 1393
		public static ScriptingInterfaceOfITexture.IsLoadedDelegate call_IsLoadedDelegate;

		// Token: 0x04000572 RID: 1394
		public static ScriptingInterfaceOfITexture.IsRenderTargetDelegate call_IsRenderTargetDelegate;

		// Token: 0x04000573 RID: 1395
		public static ScriptingInterfaceOfITexture.LoadTextureFromPathDelegate call_LoadTextureFromPathDelegate;

		// Token: 0x04000574 RID: 1396
		public static ScriptingInterfaceOfITexture.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x04000575 RID: 1397
		public static ScriptingInterfaceOfITexture.ReleaseAfterNumberOfFramesDelegate call_ReleaseAfterNumberOfFramesDelegate;

		// Token: 0x04000576 RID: 1398
		public static ScriptingInterfaceOfITexture.ReleaseGpuMemoriesDelegate call_ReleaseGpuMemoriesDelegate;

		// Token: 0x04000577 RID: 1399
		public static ScriptingInterfaceOfITexture.ReleaseNextFrameDelegate call_ReleaseNextFrameDelegate;

		// Token: 0x04000578 RID: 1400
		public static ScriptingInterfaceOfITexture.RemoveContinousTableauTextureDelegate call_RemoveContinousTableauTextureDelegate;

		// Token: 0x04000579 RID: 1401
		public static ScriptingInterfaceOfITexture.SaveTextureAsAlwaysValidDelegate call_SaveTextureAsAlwaysValidDelegate;

		// Token: 0x0400057A RID: 1402
		public static ScriptingInterfaceOfITexture.SaveToFileDelegate call_SaveToFileDelegate;

		// Token: 0x0400057B RID: 1403
		public static ScriptingInterfaceOfITexture.SetNameDelegate call_SetNameDelegate;

		// Token: 0x0400057C RID: 1404
		public static ScriptingInterfaceOfITexture.SetTableauViewDelegate call_SetTableauViewDelegate;

		// Token: 0x0400057D RID: 1405
		public static ScriptingInterfaceOfITexture.TransformRenderTargetToResourceTextureDelegate call_TransformRenderTargetToResourceTextureDelegate;

		// Token: 0x020005C4 RID: 1476
		// (Invoke) Token: 0x06001D3B RID: 7483
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CheckAndGetFromResourceDelegate(byte[] textureName);

		// Token: 0x020005C5 RID: 1477
		// (Invoke) Token: 0x06001D3F RID: 7487
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateDepthTargetDelegate(byte[] name, int width, int height);

		// Token: 0x020005C6 RID: 1478
		// (Invoke) Token: 0x06001D43 RID: 7491
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromByteArrayDelegate(ManagedArray data, int width, int height);

		// Token: 0x020005C7 RID: 1479
		// (Invoke) Token: 0x06001D47 RID: 7495
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromMemoryDelegate(ManagedArray data);

		// Token: 0x020005C8 RID: 1480
		// (Invoke) Token: 0x06001D4B RID: 7499
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateRenderTargetDelegate(byte[] name, int width, int height, [MarshalAs(UnmanagedType.U1)] bool autoMipmaps, [MarshalAs(UnmanagedType.U1)] bool isTableau, [MarshalAs(UnmanagedType.U1)] bool createUninitialized, [MarshalAs(UnmanagedType.U1)] bool always_valid);

		// Token: 0x020005C9 RID: 1481
		// (Invoke) Token: 0x06001D4F RID: 7503
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTextureFromPathDelegate(PlatformFilePath filePath);

		// Token: 0x020005CA RID: 1482
		// (Invoke) Token: 0x06001D53 RID: 7507
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCurObjectDelegate(UIntPtr texturePointer, [MarshalAs(UnmanagedType.U1)] bool blocking);

		// Token: 0x020005CB RID: 1483
		// (Invoke) Token: 0x06001D57 RID: 7511
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] textureName);

		// Token: 0x020005CC RID: 1484
		// (Invoke) Token: 0x06001D5B RID: 7515
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetHeightDelegate(UIntPtr texturePointer);

		// Token: 0x020005CD RID: 1485
		// (Invoke) Token: 0x06001D5F RID: 7519
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMemorySizeDelegate(UIntPtr texturePointer);

		// Token: 0x020005CE RID: 1486
		// (Invoke) Token: 0x06001D63 RID: 7523
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr texturePointer);

		// Token: 0x020005CF RID: 1487
		// (Invoke) Token: 0x06001D67 RID: 7527
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetPixelDataDelegate(UIntPtr texturePointer, ManagedArray bytes);

		// Token: 0x020005D0 RID: 1488
		// (Invoke) Token: 0x06001D6B RID: 7531
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRenderTargetComponentDelegate(UIntPtr texturePointer);

		// Token: 0x020005D1 RID: 1489
		// (Invoke) Token: 0x06001D6F RID: 7535
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSDFBoundingBoxDataDelegate(UIntPtr texturePointer, ref Vec3 min, ref Vec3 max);

		// Token: 0x020005D2 RID: 1490
		// (Invoke) Token: 0x06001D73 RID: 7539
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetTableauViewDelegate(UIntPtr texturePointer);

		// Token: 0x020005D3 RID: 1491
		// (Invoke) Token: 0x06001D77 RID: 7543
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetWidthDelegate(UIntPtr texturePointer);

		// Token: 0x020005D4 RID: 1492
		// (Invoke) Token: 0x06001D7B RID: 7547
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsLoadedDelegate(UIntPtr texturePointer);

		// Token: 0x020005D5 RID: 1493
		// (Invoke) Token: 0x06001D7F RID: 7551
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsRenderTargetDelegate(UIntPtr texturePointer);

		// Token: 0x020005D6 RID: 1494
		// (Invoke) Token: 0x06001D83 RID: 7555
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer LoadTextureFromPathDelegate(byte[] fileName, byte[] folder);

		// Token: 0x020005D7 RID: 1495
		// (Invoke) Token: 0x06001D87 RID: 7559
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr texturePointer);

		// Token: 0x020005D8 RID: 1496
		// (Invoke) Token: 0x06001D8B RID: 7563
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseAfterNumberOfFramesDelegate(UIntPtr texturePointer, int numberOfFrames);

		// Token: 0x020005D9 RID: 1497
		// (Invoke) Token: 0x06001D8F RID: 7567
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseGpuMemoriesDelegate();

		// Token: 0x020005DA RID: 1498
		// (Invoke) Token: 0x06001D93 RID: 7571
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseNextFrameDelegate(UIntPtr texturePointer);

		// Token: 0x020005DB RID: 1499
		// (Invoke) Token: 0x06001D97 RID: 7575
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveContinousTableauTextureDelegate(UIntPtr texturePointer);

		// Token: 0x020005DC RID: 1500
		// (Invoke) Token: 0x06001D9B RID: 7579
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SaveTextureAsAlwaysValidDelegate(UIntPtr texturePointer);

		// Token: 0x020005DD RID: 1501
		// (Invoke) Token: 0x06001D9F RID: 7583
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SaveToFileDelegate(UIntPtr texturePointer, byte[] fileName, [MarshalAs(UnmanagedType.U1)] bool isRelativePath);

		// Token: 0x020005DE RID: 1502
		// (Invoke) Token: 0x06001DA3 RID: 7587
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNameDelegate(UIntPtr texturePointer, byte[] name);

		// Token: 0x020005DF RID: 1503
		// (Invoke) Token: 0x06001DA7 RID: 7591
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTableauViewDelegate(UIntPtr texturePointer, UIntPtr tableauView);

		// Token: 0x020005E0 RID: 1504
		// (Invoke) Token: 0x06001DAB RID: 7595
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformRenderTargetToResourceTextureDelegate(UIntPtr texturePointer, byte[] name);
	}
}
