using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000004 RID: 4
	public class DirectXGraphicsContext : IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		internal Dictionary<string, DirectXTexture> LoadedTextures { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002069 File Offset: 0x00000269
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002071 File Offset: 0x00000271
		public MatrixFrame ProjectionMatrix
		{
			get
			{
				return this._projectionMatrix;
			}
			set
			{
				this._projectionMatrix = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000207A File Offset: 0x0000027A
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002082 File Offset: 0x00000282
		public MatrixFrame ViewMatrix
		{
			get
			{
				return this._viewMatrix;
			}
			set
			{
				this._viewMatrix = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000208B File Offset: 0x0000028B
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002093 File Offset: 0x00000293
		public MatrixFrame ModelMatrix
		{
			get
			{
				return this._modelMatrix;
			}
			set
			{
				this._modelMatrix = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000209C File Offset: 0x0000029C
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020A3 File Offset: 0x000002A3
		public static DirectXGraphicsContext Active { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020AB File Offset: 0x000002AB
		public IntPtr DeviceHandle
		{
			get
			{
				return this._device;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020B3 File Offset: 0x000002B3
		public IntPtr DeviceContextHandle
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020BB File Offset: 0x000002BB
		public bool IsDeviceLost
		{
			get
			{
				return this._deviceLost;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020C3 File Offset: 0x000002C3
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000020CB File Offset: 0x000002CB
		public bool IsLayeredWindow { get; set; }

		// Token: 0x06000012 RID: 18 RVA: 0x000020D4 File Offset: 0x000002D4
		public DirectXGraphicsContext()
		{
			this._loadedShaders = new Dictionary<string, DirectXShader>();
			this.LoadedTextures = new Dictionary<string, DirectXTexture>();
			this._stopwatch = new Stopwatch();
			this.MaxTimeToRenderOneFrame = 16;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000214C File Offset: 0x0000034C
		public void CreateContext(IntPtr hwnd, ResourceDepot resourceDepot)
		{
			this._resourceDepot = resourceDepot;
			IntPtr intPtr = DirectXGraphicsContext.SelectBestAdapter();
			int num = ((intPtr != IntPtr.Zero) ? 0 : 1);
			int[] array = new int[] { 45056 };
			int num3;
			if (this.IsLayeredWindow)
			{
				int num2 = D3D11Native.D3D11CreateDevice(intPtr, num, IntPtr.Zero, 32U, array, array.Length, 7, out this._device, out num3, out this._context);
				if (intPtr != IntPtr.Zero)
				{
					ComRelease.Release(intPtr);
				}
				if (num2 < 0)
				{
					StandaloneApplicationUtility.TerminateWithMessageBox("DirectX error", string.Format("D3D11CreateDevice failed (0x{0:X8}).\n", num2) + "Your system may not support DirectX 11. Please update your graphics drivers.");
					return;
				}
				this._screenWidth = 1;
				this._screenHeight = 1;
				this.CreateOffscreenRenderTarget();
			}
			else
			{
				DXGI_SWAP_CHAIN_DESC dxgi_SWAP_CHAIN_DESC = new DXGI_SWAP_CHAIN_DESC
				{
					BufferDesc = new DXGI_MODE_DESC
					{
						Width = 0U,
						Height = 0U,
						Format = 28U,
						RefreshRate = new DXGI_RATIONAL
						{
							Numerator = 0U,
							Denominator = 1U
						}
					},
					SampleDesc = new DXGI_SAMPLE_DESC
					{
						Count = 1U,
						Quality = 0U
					},
					BufferUsage = 32U,
					BufferCount = 1U,
					OutputWindow = hwnd,
					Windowed = 1,
					SwapEffect = 0U,
					Flags = 0U
				};
				int num2 = D3D11Native.D3D11CreateDeviceAndSwapChain(intPtr, num, IntPtr.Zero, 32U, array, array.Length, 7, ref dxgi_SWAP_CHAIN_DESC, out this._swapChain, out this._device, out num3, out this._context);
				if (intPtr != IntPtr.Zero)
				{
					ComRelease.Release(intPtr);
				}
				if (num2 < 0)
				{
					StandaloneApplicationUtility.TerminateWithMessageBox("DirectX error", string.Format("D3D11CreateDeviceAndSwapChain failed (0x{0:X8}).\n", num2) + "Your system may not support DirectX 11. Please update your graphics drivers.");
					return;
				}
				this.CreateRenderTargetView();
			}
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "D3D11FeatureLevel", string.Format("0x{0:X4}", num3));
			DirectXGraphicsContext.Active = this;
			this.CreateRenderStates();
			this.CreateConstantBuffers();
			this._vertexBuffer = DirectXVertexBuffer.Create(this._device);
			if (this._vertexBuffer == null)
			{
				StandaloneApplicationUtility.TerminateWithMessageBox("DirectX error", "Failed to create vertex buffers.");
				return;
			}
			this.ProjectionMatrix = MatrixFrame.Identity.Filled();
			this.ViewMatrix = MatrixFrame.Identity.Filled();
			this.ModelMatrix = MatrixFrame.Identity.Filled();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023B8 File Offset: 0x000005B8
		private void CreateOffscreenRenderTarget()
		{
			if (this._renderTargetView != IntPtr.Zero)
			{
				ComRelease.Release(this._renderTargetView);
				this._renderTargetView = IntPtr.Zero;
			}
			if (this._offscreenRT != IntPtr.Zero)
			{
				ComRelease.Release(this._offscreenRT);
				this._offscreenRT = IntPtr.Zero;
			}
			D3D11_TEXTURE2D_DESC d3D11_TEXTURE2D_DESC = new D3D11_TEXTURE2D_DESC
			{
				Width = (uint)this._screenWidth,
				Height = (uint)this._screenHeight,
				MipLevels = 1U,
				ArraySize = 1U,
				Format = 87U,
				SampleDesc = new DXGI_SAMPLE_DESC
				{
					Count = 1U,
					Quality = 0U
				},
				Usage = 0U,
				BindFlags = 32U,
				CPUAccessFlags = 0U,
				MiscFlags = 0U
			};
			if (D3D11Device.CreateTexture2DEmpty(this._device, ref d3D11_TEXTURE2D_DESC, out this._offscreenRT) < 0)
			{
				return;
			}
			D3D11Device.CreateRenderTargetView(this._device, this._offscreenRT, out this._renderTargetView);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024C4 File Offset: 0x000006C4
		private static IntPtr SelectBestAdapter()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2;
			try
			{
				DXGI.CreateDXGIFactory(ref DXGI.IID_IDXGIFactory, out intPtr);
				if (intPtr == IntPtr.Zero)
				{
					intPtr2 = IntPtr.Zero;
				}
				else
				{
					IntPtr intPtr3 = IntPtr.Zero;
					IntPtr intPtr4 = IntPtr.Zero;
					ulong num = 0UL;
					uint num2 = 0U;
					IntPtr intPtr5;
					while (DXGIFactory.EnumAdapters(intPtr, num2, out intPtr5) == 0)
					{
						DXGI.DXGI_ADAPTER_DESC dxgi_ADAPTER_DESC;
						DXGIAdapter.GetDesc(intPtr5, out dxgi_ADAPTER_DESC);
						ulong num3 = (ulong)dxgi_ADAPTER_DESC.DedicatedVideoMemory;
						bool flag = dxgi_ADAPTER_DESC.VendorId == 5140U && dxgi_ADAPTER_DESC.DeviceId == 140U;
						if (!flag && num3 > num)
						{
							if (intPtr3 != IntPtr.Zero)
							{
								ComRelease.Release(intPtr3);
							}
							intPtr3 = intPtr5;
							num = num3;
							if (intPtr4 != IntPtr.Zero)
							{
								ComRelease.Release(intPtr4);
								intPtr4 = IntPtr.Zero;
							}
							Watchdog.LogProperty("crash_tags.txt", "Runtime", "D3D11SelectedAdapter", dxgi_ADAPTER_DESC.Description);
						}
						else if (!flag && intPtr3 == IntPtr.Zero && intPtr4 == IntPtr.Zero)
						{
							intPtr4 = intPtr5;
						}
						else
						{
							ComRelease.Release(intPtr5);
						}
						num2 += 1U;
					}
					ComRelease.Release(intPtr);
					intPtr = IntPtr.Zero;
					if (intPtr3 != IntPtr.Zero)
					{
						intPtr2 = intPtr3;
					}
					else if (intPtr4 != IntPtr.Zero)
					{
						Watchdog.LogProperty("crash_tags.txt", "Runtime", "D3D11SelectedAdapter", "FirstHardwareFallback");
						intPtr2 = intPtr4;
					}
					else
					{
						intPtr2 = IntPtr.Zero;
					}
				}
			}
			catch (Exception)
			{
				ComRelease.Release(intPtr);
				intPtr2 = IntPtr.Zero;
			}
			return intPtr2;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002664 File Offset: 0x00000864
		private void CreateRenderTargetView()
		{
			if (this._renderTargetView != IntPtr.Zero)
			{
				ComRelease.Release(this._renderTargetView);
				this._renderTargetView = IntPtr.Zero;
			}
			Guid iid_ID3D11Texture2D = DirectXGraphicsContext.IID_ID3D11Texture2D;
			IntPtr intPtr;
			if (DXGISwapChain.GetBuffer(this._swapChain, ref iid_ID3D11Texture2D, out intPtr) < 0 || intPtr == IntPtr.Zero)
			{
				return;
			}
			D3D11Device.CreateRenderTargetView(this._device, intPtr, out this._renderTargetView);
			ComRelease.Release(intPtr);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026DC File Offset: 0x000008DC
		private void CreateRenderStates()
		{
			D3D11_RENDER_TARGET_BLEND_DESC d3D11_RENDER_TARGET_BLEND_DESC = new D3D11_RENDER_TARGET_BLEND_DESC
			{
				BlendEnable = 1,
				SrcBlend = 5U,
				DestBlend = 6U,
				BlendOp = 1U,
				SrcBlendAlpha = 2U,
				DestBlendAlpha = 2U,
				BlendOpAlpha = 1U,
				RenderTargetWriteMask = 15
			};
			D3D11_BLEND_DESC d3D11_BLEND_DESC = new D3D11_BLEND_DESC
			{
				AlphaToCoverageEnable = 0,
				IndependentBlendEnable = 0,
				RT0 = d3D11_RENDER_TARGET_BLEND_DESC
			};
			D3D11Device.CreateBlendState(this._device, ref d3D11_BLEND_DESC, out this._blendStateAlpha);
			D3D11_RENDER_TARGET_BLEND_DESC d3D11_RENDER_TARGET_BLEND_DESC2 = new D3D11_RENDER_TARGET_BLEND_DESC
			{
				BlendEnable = 0,
				SrcBlend = 2U,
				DestBlend = 1U,
				BlendOp = 1U,
				SrcBlendAlpha = 2U,
				DestBlendAlpha = 1U,
				BlendOpAlpha = 1U,
				RenderTargetWriteMask = 15
			};
			D3D11_BLEND_DESC d3D11_BLEND_DESC2 = new D3D11_BLEND_DESC
			{
				AlphaToCoverageEnable = 0,
				IndependentBlendEnable = 0,
				RT0 = d3D11_RENDER_TARGET_BLEND_DESC2
			};
			D3D11Device.CreateBlendState(this._device, ref d3D11_BLEND_DESC2, out this._blendStateOpaque);
			D3D11_RASTERIZER_DESC d3D11_RASTERIZER_DESC = new D3D11_RASTERIZER_DESC
			{
				FillMode = 3U,
				CullMode = 1U,
				FrontCounterClockwise = 0,
				DepthBias = 0,
				DepthBiasClamp = 0f,
				SlopeScaledDepthBias = 0f,
				DepthClipEnable = 0,
				ScissorEnable = 1,
				MultisampleEnable = 0,
				AntialiasedLineEnable = 0
			};
			D3D11Device.CreateRasterizerState(this._device, ref d3D11_RASTERIZER_DESC, out this._rasterizerScissor);
			D3D11_RASTERIZER_DESC d3D11_RASTERIZER_DESC2 = d3D11_RASTERIZER_DESC;
			d3D11_RASTERIZER_DESC2.ScissorEnable = 0;
			D3D11Device.CreateRasterizerState(this._device, ref d3D11_RASTERIZER_DESC2, out this._rasterizerNoScissor);
			D3D11_SAMPLER_DESC d3D11_SAMPLER_DESC = new D3D11_SAMPLER_DESC
			{
				Filter = 21U,
				AddressU = 1U,
				AddressV = 1U,
				AddressW = 1U,
				MipLODBias = 0f,
				MaxAnisotropy = 1U,
				ComparisonFunc = 1U,
				BorderColor0 = 0f,
				BorderColor1 = 0f,
				BorderColor2 = 0f,
				BorderColor3 = 0f,
				MinLOD = 0f,
				MaxLOD = float.MaxValue
			};
			D3D11Device.CreateSamplerState(this._device, ref d3D11_SAMPLER_DESC, out this._samplerLinear);
			D3D11_SAMPLER_DESC d3D11_SAMPLER_DESC2 = d3D11_SAMPLER_DESC;
			d3D11_SAMPLER_DESC2.AddressU = 3U;
			d3D11_SAMPLER_DESC2.AddressV = 3U;
			d3D11_SAMPLER_DESC2.AddressW = 3U;
			D3D11Device.CreateSamplerState(this._device, ref d3D11_SAMPLER_DESC2, out this._samplerLinearClamp);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000295C File Offset: 0x00000B5C
		private void CreateConstantBuffers()
		{
			D3D11_BUFFER_DESC d3D11_BUFFER_DESC = new D3D11_BUFFER_DESC
			{
				ByteWidth = 64U,
				Usage = 2U,
				BindFlags = 4U,
				CPUAccessFlags = 65536U,
				MiscFlags = 0U,
				StructureByteStride = 0U
			};
			int num = D3D11Device.CreateBuffer(this._device, ref d3D11_BUFFER_DESC, out this._cbMVP);
			if (num < 0)
			{
				StandaloneApplicationUtility.TerminateWithMessageBox("DirectX error", string.Format("Failed to create MVP constant buffer (0x{0:X8}).", num));
				return;
			}
			D3D11_BUFFER_DESC d3D11_BUFFER_DESC2 = new D3D11_BUFFER_DESC
			{
				ByteWidth = 256U,
				Usage = 2U,
				BindFlags = 4U,
				CPUAccessFlags = 65536U,
				MiscFlags = 0U,
				StructureByteStride = 0U
			};
			num = D3D11Device.CreateBuffer(this._device, ref d3D11_BUFFER_DESC2, out this._cbMaterial);
			if (num < 0)
			{
				StandaloneApplicationUtility.TerminateWithMessageBox("DirectX error", string.Format("Failed to create material constant buffer (0x{0:X8}).", num));
				return;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A50 File Offset: 0x00000C50
		public void BeginFrame(int width, int height)
		{
			if (this._isShuttingDown)
			{
				return;
			}
			this._anyInvalidMatricesThisFrame = false;
			this._stopwatch.Start();
			if (this._deviceLost)
			{
				this._anyInvalidMatricesThisFrame = true;
				return;
			}
			this.Resize(width, height);
			if (this._renderTargetView == IntPtr.Zero)
			{
				this._anyInvalidMatricesThisFrame = true;
				return;
			}
			D3D11Context.OMSetRenderTargets(this._context, this._renderTargetView);
			D3D11Context.RSSetState(this._context, this._rasterizerNoScissor);
			this._scissorEnabled = false;
			D3D11_VIEWPORT d3D11_VIEWPORT = new D3D11_VIEWPORT
			{
				TopLeftX = 0f,
				TopLeftY = 0f,
				Width = (float)width,
				Height = (float)height,
				MinDepth = 0f,
				MaxDepth = 1f
			};
			D3D11Context.RSSetViewports(this._context, d3D11_VIEWPORT);
			D3D11Context.ClearRenderTargetView(this._context, this._renderTargetView, new float[4]);
			D3D11Context.PSSetSamplers(this._context, this._samplerLinear);
			this.SetBlending(false);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002B5C File Offset: 0x00000D5C
		internal void Resize(int width, int height)
		{
			if (width == this._screenWidth && height == this._screenHeight)
			{
				return;
			}
			if (width <= 0 || height <= 0)
			{
				return;
			}
			this._screenWidth = width;
			this._screenHeight = height;
			if (this.IsLayeredWindow)
			{
				this.CreateOffscreenRenderTarget();
				return;
			}
			if (this._renderTargetView != IntPtr.Zero)
			{
				ComRelease.Release(this._renderTargetView);
				this._renderTargetView = IntPtr.Zero;
			}
			int num = DXGISwapChain.ResizeBuffers(this._swapChain, (uint)width, (uint)height);
			if (num < 0)
			{
				if (num == -2005270523 || num == -2005270521)
				{
					this._deviceLost = true;
				}
				return;
			}
			this.CreateRenderTargetView();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002BFC File Offset: 0x00000DFC
		public void SwapBuffers()
		{
			if (this._isShuttingDown)
			{
				return;
			}
			int num = (int)this._stopwatch.ElapsedMilliseconds;
			int num2 = this.MaxTimeToRenderOneFrame - num;
			if (num2 > 0)
			{
				Thread.Sleep(num2);
			}
			if (!this.IsLayeredWindow)
			{
				int num3 = DXGISwapChain.Present(this._swapChain, 0U, 0U);
				if (num3 == -2005270523 || num3 == -2005270521)
				{
					this._deviceLost = true;
					this._anyInvalidMatricesThisFrame = true;
				}
				else if (num3 < 0)
				{
					this._anyInvalidMatricesThisFrame = true;
				}
			}
			this._stopwatch.Restart();
			if (this._anyInvalidMatricesThisFrame)
			{
				this._failedRenderFrames++;
			}
			else
			{
				this._failedRenderFrames = 0;
			}
			if (this._failedRenderFrames >= 180)
			{
				Watchdog.LogProperty("crash_tags.txt", "Runtime", "LauncherRenderFailure", "ConsecutiveFrameThresholdExceeded");
				StandaloneApplicationUtility.TerminateWithMessageBox("Launcher render error", "The launcher encountered too many consecutive render failures and must close.\nPlease update your graphics drivers and try again.");
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public void DestroyContext()
		{
			this._isShuttingDown = true;
			DirectXGraphicsContext.Active = null;
			foreach (DirectXShader directXShader in this._loadedShaders.Values)
			{
				if (directXShader != null)
				{
					directXShader.Dispose();
				}
			}
			foreach (DirectXTexture directXTexture in this.LoadedTextures.Values)
			{
				if (directXTexture != null)
				{
					directXTexture.Dispose();
				}
			}
			DirectXVertexBuffer vertexBuffer = this._vertexBuffer;
			if (vertexBuffer != null)
			{
				vertexBuffer.Dispose();
			}
			ComRelease.Release(this._cbMVP);
			ComRelease.Release(this._cbMaterial);
			ComRelease.Release(this._samplerLinear);
			ComRelease.Release(this._samplerLinearClamp);
			ComRelease.Release(this._blendStateAlpha);
			ComRelease.Release(this._blendStateOpaque);
			ComRelease.Release(this._rasterizerScissor);
			ComRelease.Release(this._rasterizerNoScissor);
			ComRelease.Release(this._renderTargetView);
			ComRelease.Release(this._offscreenRT);
			ComRelease.Release(this._swapChain);
			if (this._context != IntPtr.Zero)
			{
				D3D11Context.ClearState(this._context);
				D3D11Context.Flush(this._context);
			}
			ComRelease.Release(this._context);
			ComRelease.Release(this._device);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002E50 File Offset: 0x00001050
		public void Dispose()
		{
			this.DestroyContext();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002E58 File Offset: 0x00001058
		public void ReportDeviceLost(int triggerHr)
		{
			if (this._isShuttingDown)
			{
				return;
			}
			int num = ((this._device != IntPtr.Zero) ? D3D11Device.GetDeviceRemovedReason(this._device) : triggerHr);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "D3D11DeviceRemoved", string.Format("trigger=0x{0:X8} reason=0x{1:X8}", triggerHr, num));
			this._deviceLost = true;
			this._anyInvalidMatricesThisFrame = true;
			StandaloneApplicationUtility.TerminateWithMessageBox("Graphics device lost", "The graphics device was lost (driver crash, GPU reset, or power-down).\nPlease close and relaunch the launcher.");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002ED8 File Offset: 0x000010D8
		public IntPtr GetCurrentBackBuffer()
		{
			if (this._isShuttingDown)
			{
				return IntPtr.Zero;
			}
			if (this.IsLayeredWindow)
			{
				if (this._offscreenRT == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}
				ComAddRef.AddRef(this._offscreenRT);
				return this._offscreenRT;
			}
			else
			{
				if (this._swapChain == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}
				Guid iid_ID3D11Texture2D = DirectXGraphicsContext.IID_ID3D11Texture2D;
				IntPtr intPtr;
				if (DXGISwapChain.GetBuffer(this._swapChain, ref iid_ID3D11Texture2D, out intPtr) < 0 || intPtr == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}
				return intPtr;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002F6C File Offset: 0x0000116C
		public void SetScissor(ScissorTestInfo scissorTestInfo)
		{
			SimpleRectangle simpleRectangle = scissorTestInfo.GetSimpleRectangle();
			D3D11_RECT d3D11_RECT = new D3D11_RECT
			{
				left = (int)simpleRectangle.X,
				top = (int)simpleRectangle.Y,
				right = (int)(simpleRectangle.X + simpleRectangle.Width),
				bottom = (int)(simpleRectangle.Y + simpleRectangle.Height)
			};
			D3D11Context.RSSetScissorRects(this._context, d3D11_RECT);
			if (!this._scissorEnabled)
			{
				D3D11Context.RSSetState(this._context, this._rasterizerScissor);
				this._scissorEnabled = true;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002FFF File Offset: 0x000011FF
		public void ResetScissor()
		{
			if (this._scissorEnabled)
			{
				D3D11Context.RSSetState(this._context, this._rasterizerNoScissor);
				this._scissorEnabled = false;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003021 File Offset: 0x00001221
		public void SetBlending(bool enable)
		{
			if (this._blendingEnabled == enable)
			{
				return;
			}
			this._blendingEnabled = enable;
			D3D11Context.OMSetBlendState(this._context, enable ? this._blendStateAlpha : this._blendStateOpaque);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003050 File Offset: 0x00001250
		public DirectXShader GetOrLoadShader(string shaderName)
		{
			if (this._loadedShaders.ContainsKey(shaderName))
			{
				return this._loadedShaders[shaderName];
			}
			DirectXShader directXShader2;
			try
			{
				string text = File.ReadAllText(this._resourceDepot.GetFilePath(shaderName + ".hlsl"));
				DirectXShader directXShader = DirectXShader.CreateShader(this._device, text, shaderName);
				this._loadedShaders[shaderName] = directXShader;
				directXShader2 = directXShader;
			}
			catch (Exception)
			{
				this._loadedShaders[shaderName] = null;
				directXShader2 = null;
			}
			return directXShader2;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000030D8 File Offset: 0x000012D8
		public void DrawImage(SimpleMaterial material, in ImageDrawObject drawObject)
		{
			if (this._isShuttingDown)
			{
				return;
			}
			DirectXShader directXShader = this.PrepareRender(material, in drawObject.Rectangle);
			if (directXShader == null)
			{
				return;
			}
			this.DrawImageAux(directXShader, material, in drawObject);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003109 File Offset: 0x00001309
		public void DrawText(TextMaterial material, in TextDrawObject drawObject)
		{
			if (this._isShuttingDown)
			{
				return;
			}
			if (this.PrepareRender(material, in drawObject.Rectangle) == null)
			{
				return;
			}
			this.DrawTextAux(material, in drawObject);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000312C File Offset: 0x0000132C
		public void DrawPolygon(PrimitivePolygonMaterial material, in ImageDrawObject drawObject)
		{
			if (this._isShuttingDown)
			{
				return;
			}
			if (this.PrepareRender(material, in drawObject.Rectangle) == null)
			{
				return;
			}
			this.DrawPolygonAux(material, in drawObject);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003150 File Offset: 0x00001350
		private DirectXShader PrepareRender(Material material, in Rectangle2D rect)
		{
			DirectXShader orLoadShader = this.GetOrLoadShader(material.GetType().Name);
			if (orLoadShader == null)
			{
				this._anyInvalidMatricesThisFrame = true;
				return null;
			}
			if (this._screenWidth <= 0 || this._screenHeight <= 0)
			{
				this._anyInvalidMatricesThisFrame = true;
				return null;
			}
			Rectangle2D rectangle2D = rect;
			MatrixFrame cachedVisualMatrixFrame = rectangle2D.GetCachedVisualMatrixFrame();
			if (cachedVisualMatrixFrame.AreAllComponentsValid() && !cachedVisualMatrixFrame.IsZero)
			{
				this.ModelMatrix = cachedVisualMatrixFrame;
			}
			else
			{
				this.ModelMatrix = DirectXGraphicsContext.ValidateModelMatrix(cachedVisualMatrixFrame);
				this._anyInvalidMatricesThisFrame = true;
			}
			Matrix4x4 matrix4x = DirectXGraphicsContext.ValidateModelMatrix(this._modelMatrix).ToMatrix4x4();
			Matrix4x4 matrix4x2 = DirectXGraphicsContext.ValidateViewMatrix(in this._viewMatrix).ToMatrix4x4();
			Matrix4x4 matrix4x3 = DirectXGraphicsContext.ValidateProjectionMatrix(in this._projectionMatrix).ToMatrix4x4();
			Matrix4x4 matrix4x4 = matrix4x * matrix4x2 * matrix4x3;
			this.UploadMVP(in matrix4x4);
			orLoadShader.Use(this._context);
			D3D11Context.VSSetConstantBuffers(this._context, 0U, this._cbMVP);
			D3D11Context.PSSetConstantBuffers(this._context, 0U, this._cbMVP);
			return orLoadShader;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000324C File Offset: 0x0000144C
		private void DrawImageAux(DirectXShader shader, SimpleMaterial material, in ImageDrawObject drawObject)
		{
			this.SetBlending(material.Blending);
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			bool flag = false;
			if (material.Texture != null)
			{
				DirectXTexture directXTexture = material.Texture.PlatformTexture as DirectXTexture;
				intPtr = ((directXTexture != null) ? directXTexture.ShaderResourceView : IntPtr.Zero);
				flag = directXTexture != null && directXTexture.ClampToEdge;
			}
			D3D11Context.PSSetSamplers(this._context, flag ? this._samplerLinearClamp : this._samplerLinear);
			D3D11Context.PSSetShaderResources(this._context, 0U, intPtr);
			if (material.OverlayEnabled && material.OverlayTexture != null)
			{
				DirectXTexture directXTexture2 = material.OverlayTexture.PlatformTexture as DirectXTexture;
				intPtr2 = ((directXTexture2 != null) ? directXTexture2.ShaderResourceView : IntPtr.Zero);
			}
			D3D11Context.PSSetShaderResources(this._context, 1U, intPtr2);
			float num = DirectXGraphicsContext.Clamp(material.HueFactor / 360f, -0.5f, 0.5f);
			float num2 = DirectXGraphicsContext.Clamp(material.SaturationFactor / 360f, -0.5f, 0.5f);
			float num3 = DirectXGraphicsContext.Clamp(material.ValueFactor / 360f, -0.5f, 0.5f);
			DirectXGraphicsContext.SimpleMaterialCB simpleMaterialCB = new DirectXGraphicsContext.SimpleMaterialCB
			{
				InputColor = DirectXGraphicsContext.ColorToFloat4(material.Color),
				ColorFactor = material.ColorFactor,
				AlphaFactor = material.AlphaFactor,
				HueFactor = num,
				SaturationFactor = num2,
				ValueFactor = num3,
				OverlayEnabled = (material.OverlayEnabled ? 1 : 0),
				StartCoordX = material.StartCoordinate.X,
				StartCoordY = material.StartCoordinate.Y,
				SizeX = material.Size.X,
				SizeY = material.Size.Y,
				OverlayOffsetX = material.OverlayXOffset,
				OverlayOffsetY = material.OverlayYOffset,
				CircularMaskingEnabled = (material.CircularMaskingEnabled ? 1 : 0),
				MaskingCenterX = material.CircularMaskingCenter.X,
				MaskingCenterY = material.CircularMaskingCenter.Y,
				MaskingRadius = material.CircularMaskingRadius,
				MaskingSmoothingRadius = material.CircularMaskingSmoothingRadius
			};
			this.UploadMaterialCB<DirectXGraphicsContext.SimpleMaterialCB>(ref simpleMaterialCB, Marshal.SizeOf<DirectXGraphicsContext.SimpleMaterialCB>());
			D3D11Context.PSSetConstantBuffers(this._context, 1U, this._cbMaterial);
			Vector2 vector = new Vector2(drawObject.Uvs.x, drawObject.Uvs.y);
			Vector2 vector2 = new Vector2(drawObject.Uvs.z, drawObject.Uvs.w);
			float[] array = new float[]
			{
				0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 1f, 1f,
				0f, 0f, 1f, 0f, 0f, 0f
			};
			array[2] = vector.X;
			array[3] = vector.Y;
			array[6] = vector.X;
			array[7] = vector2.Y;
			array[10] = vector2.X;
			array[11] = vector2.Y;
			array[14] = vector2.X;
			array[15] = vector.Y;
			float[] array2 = array;
			uint[] array3 = new uint[] { 0U, 1U, 2U, 0U, 2U, 3U };
			this._vertexBuffer.LoadVertexData(this._context, array2);
			this._vertexBuffer.LoadIndexData(this._context, array3);
			this._vertexBuffer.Bind(this._context);
			D3D11Context.DrawIndexed(this._context, array3.Length);
			D3D11Context.PSClearShaderResource(this._context, 0U);
			D3D11Context.PSClearShaderResource(this._context, 1U);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000035B4 File Offset: 0x000017B4
		private void DrawTextAux(TextMaterial material, in TextDrawObject drawObject)
		{
			this.SetBlending(material.Blending);
			D3D11Context.PSSetSamplers(this._context, this._samplerLinear);
			IntPtr intPtr = IntPtr.Zero;
			if (material.Texture != null)
			{
				DirectXTexture directXTexture = material.Texture.PlatformTexture as DirectXTexture;
				intPtr = ((directXTexture != null) ? directXTexture.ShaderResourceView : IntPtr.Zero);
			}
			D3D11Context.PSSetShaderResources(this._context, 0U, intPtr);
			DirectXGraphicsContext.TextMaterialCB textMaterialCB = new DirectXGraphicsContext.TextMaterialCB
			{
				InputColor = DirectXGraphicsContext.ColorToFloat4(material.Color),
				GlowColor = DirectXGraphicsContext.ColorToFloat4(material.GlowColor),
				OutlineColor = DirectXGraphicsContext.ColorToFloat4(material.OutlineColor),
				OutlineAmount = material.OutlineAmount,
				ScaleFactor = 1.5f / material.ScaleFactor,
				SmoothingConstant = material.SmoothingConstant,
				GlowRadius = material.GlowRadius,
				Blur = material.Blur,
				ShadowOffset = material.ShadowOffset,
				ShadowAngle = material.ShadowAngle,
				ColorFactor = material.ColorFactor,
				AlphaFactor = material.AlphaFactor
			};
			this.UploadMaterialCB<DirectXGraphicsContext.TextMaterialCB>(ref textMaterialCB, Marshal.SizeOf<DirectXGraphicsContext.TextMaterialCB>());
			D3D11Context.PSSetConstantBuffers(this._context, 1U, this._cbMaterial);
			float[] text_Vertices = drawObject.Text_Vertices;
			float[] text_TextureCoordinates = drawObject.Text_TextureCoordinates;
			uint[] text_Indices = drawObject.Text_Indices;
			int num = text_Vertices.Length / 2;
			float[] array = new float[num * 4];
			for (int i = 0; i < num; i++)
			{
				array[i * 4] = text_Vertices[i * 2];
				array[i * 4 + 1] = text_Vertices[i * 2 + 1];
				array[i * 4 + 2] = text_TextureCoordinates[i * 2];
				array[i * 4 + 3] = text_TextureCoordinates[i * 2 + 1];
			}
			this._vertexBuffer.LoadVertexData(this._context, array);
			this._vertexBuffer.LoadIndexData(this._context, text_Indices);
			this._vertexBuffer.Bind(this._context);
			D3D11Context.DrawIndexed(this._context, text_Indices.Length);
			D3D11Context.PSClearShaderResource(this._context, 0U);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000037C0 File Offset: 0x000019C0
		private void DrawPolygonAux(PrimitivePolygonMaterial material, in ImageDrawObject drawObject)
		{
			this.SetBlending(material.Blending);
			DirectXGraphicsContext.PrimitivePolygonCB primitivePolygonCB = new DirectXGraphicsContext.PrimitivePolygonCB
			{
				Color = DirectXGraphicsContext.ColorToFloat4(material.Color)
			};
			this.UploadMaterialCB<DirectXGraphicsContext.PrimitivePolygonCB>(ref primitivePolygonCB, Marshal.SizeOf<DirectXGraphicsContext.PrimitivePolygonCB>());
			D3D11Context.PSSetConstantBuffers(this._context, 1U, this._cbMaterial);
			float[] array = new float[]
			{
				0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 1f, 1f,
				0f, 0f, 1f, 0f, 0f, 0f
			};
			uint[] array2 = new uint[] { 0U, 1U, 2U, 0U, 2U, 3U };
			this._vertexBuffer.LoadVertexData(this._context, array);
			this._vertexBuffer.LoadIndexData(this._context, array2);
			this._vertexBuffer.Bind(this._context);
			D3D11Context.DrawIndexed(this._context, array2.Length);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000387C File Offset: 0x00001A7C
		private void UploadMVP(in Matrix4x4 mvp)
		{
			D3D11_MAPPED_SUBRESOURCE d3D11_MAPPED_SUBRESOURCE;
			if (D3D11Context.Map(this._context, this._cbMVP, 4U, out d3D11_MAPPED_SUBRESOURCE) < 0)
			{
				return;
			}
			try
			{
				Marshal.Copy(new float[]
				{
					mvp.M11, mvp.M12, mvp.M13, mvp.M14, mvp.M21, mvp.M22, mvp.M23, mvp.M24, mvp.M31, mvp.M32,
					mvp.M33, mvp.M34, mvp.M41, mvp.M42, mvp.M43, mvp.M44
				}, 0, d3D11_MAPPED_SUBRESOURCE.pData, 16);
			}
			finally
			{
				D3D11Context.Unmap(this._context, this._cbMVP);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003974 File Offset: 0x00001B74
		private void UploadMaterialCB<T>(ref T data, int size) where T : struct
		{
			D3D11_MAPPED_SUBRESOURCE d3D11_MAPPED_SUBRESOURCE;
			if (D3D11Context.Map(this._context, this._cbMaterial, 4U, out d3D11_MAPPED_SUBRESOURCE) < 0)
			{
				return;
			}
			try
			{
				IntPtr intPtr = Marshal.AllocHGlobal(size);
				try
				{
					Marshal.StructureToPtr<T>(data, intPtr, false);
					byte[] array = new byte[size];
					Marshal.Copy(intPtr, array, 0, size);
					Marshal.Copy(array, 0, d3D11_MAPPED_SUBRESOURCE.pData, size);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			finally
			{
				D3D11Context.Unmap(this._context, this._cbMaterial);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003A04 File Offset: 0x00001C04
		public void LoadTextureUsing(DirectXTexture texture, ResourceDepot resourceDepot, string name)
		{
			if (!this.LoadedTextures.ContainsKey(name))
			{
				texture.LoadFromFile(this._device, resourceDepot, name);
				this.LoadedTextures.Add(name, texture);
				return;
			}
			texture.CopyFrom(this.LoadedTextures[name]);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003A44 File Offset: 0x00001C44
		public DirectXTexture LoadTexture(ResourceDepot resourceDepot, string name)
		{
			DirectXTexture directXTexture;
			if (this.LoadedTextures.TryGetValue(name, out directXTexture))
			{
				if (directXTexture != null && directXTexture.IsLoaded())
				{
					return directXTexture;
				}
				this.LoadedTextures.Remove(name);
			}
			DirectXTexture directXTexture2 = DirectXTexture.FromFile(this._device, resourceDepot, name);
			if (directXTexture2 == null || !directXTexture2.IsLoaded())
			{
				return null;
			}
			this.LoadedTextures.Add(name, directXTexture2);
			return directXTexture2;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003AA4 File Offset: 0x00001CA4
		public DirectXTexture GetTexture(string textureName)
		{
			DirectXTexture directXTexture;
			this.LoadedTextures.TryGetValue(textureName, out directXTexture);
			return directXTexture;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003AC4 File Offset: 0x00001CC4
		private static MatrixFrame ValidateModelMatrix(MatrixFrame m)
		{
			if (!m.origin.IsValidXYZW)
			{
				m.origin = new Vec3(0f, 0f, 0f, 0f);
			}
			if (!m.rotation.s.IsValidXYZW)
			{
				m.rotation.s = new Vec3(100f, 0f, 0f, 0f);
			}
			if (!m.rotation.f.IsValidXYZW)
			{
				m.rotation.f = new Vec3(0f, 100f, 0f, 0f);
			}
			if (!m.rotation.u.IsValidXYZW)
			{
				m.rotation.u = new Vec3(0f, 0f, 1f, 0f);
			}
			m.Fill();
			return m;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003BAF File Offset: 0x00001DAF
		private static MatrixFrame ValidateViewMatrix(in MatrixFrame m)
		{
			if (!m.AreAllComponentsValid())
			{
				return MatrixFrame.CreateLookAt(in Vec3.Up, in Vec3.Zero, in Vec3.Forward);
			}
			return m;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003BD9 File Offset: 0x00001DD9
		private static MatrixFrame ValidateProjectionMatrix(in MatrixFrame m)
		{
			if (!m.AreAllComponentsValid())
			{
				return MatrixExtensions.CreateOrthographicOffCenter(0f, 900f, 600f, 0f, 0f, 1f);
			}
			return m;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003C12 File Offset: 0x00001E12
		private static float[] ColorToFloat4(Color c)
		{
			return new float[] { c.Red, c.Green, c.Blue, c.Alpha };
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003C3E File Offset: 0x00001E3E
		private static float Clamp(float v, float min, float max)
		{
			if (v < min)
			{
				return min;
			}
			if (v <= max)
			{
				return v;
			}
			return max;
		}

		// Token: 0x04000001 RID: 1
		public const int MaxFrameRate = 60;

		// Token: 0x04000002 RID: 2
		public readonly int MaxTimeToRenderOneFrame;

		// Token: 0x04000003 RID: 3
		private const int FailedRenderFramesFatalThreshold = 180;

		// Token: 0x04000004 RID: 4
		private IntPtr _device;

		// Token: 0x04000005 RID: 5
		private IntPtr _context;

		// Token: 0x04000006 RID: 6
		private IntPtr _swapChain;

		// Token: 0x04000007 RID: 7
		private IntPtr _renderTargetView;

		// Token: 0x04000008 RID: 8
		private IntPtr _offscreenRT;

		// Token: 0x04000009 RID: 9
		private IntPtr _blendStateAlpha;

		// Token: 0x0400000A RID: 10
		private IntPtr _blendStateOpaque;

		// Token: 0x0400000B RID: 11
		private IntPtr _rasterizerScissor;

		// Token: 0x0400000C RID: 12
		private IntPtr _rasterizerNoScissor;

		// Token: 0x0400000D RID: 13
		private IntPtr _samplerLinear;

		// Token: 0x0400000E RID: 14
		private IntPtr _samplerLinearClamp;

		// Token: 0x0400000F RID: 15
		private DirectXVertexBuffer _vertexBuffer;

		// Token: 0x04000010 RID: 16
		private IntPtr _cbMVP;

		// Token: 0x04000011 RID: 17
		private IntPtr _cbMaterial;

		// Token: 0x04000012 RID: 18
		private Dictionary<string, DirectXShader> _loadedShaders;

		// Token: 0x04000014 RID: 20
		private MatrixFrame _modelMatrix = MatrixFrame.Identity.Filled();

		// Token: 0x04000015 RID: 21
		private MatrixFrame _viewMatrix = MatrixFrame.Identity.Filled();

		// Token: 0x04000016 RID: 22
		private MatrixFrame _projectionMatrix = MatrixFrame.Identity.Filled();

		// Token: 0x04000017 RID: 23
		private int _screenWidth;

		// Token: 0x04000018 RID: 24
		private int _screenHeight;

		// Token: 0x04000019 RID: 25
		private bool _scissorEnabled;

		// Token: 0x0400001A RID: 26
		private bool _blendingEnabled;

		// Token: 0x0400001B RID: 27
		private bool _anyInvalidMatricesThisFrame;

		// Token: 0x0400001C RID: 28
		private int _failedRenderFrames;

		// Token: 0x0400001D RID: 29
		private bool _deviceLost;

		// Token: 0x0400001E RID: 30
		private bool _isShuttingDown;

		// Token: 0x0400001F RID: 31
		private Stopwatch _stopwatch;

		// Token: 0x04000020 RID: 32
		private ResourceDepot _resourceDepot;

		// Token: 0x04000023 RID: 35
		private static readonly Guid IID_ID3D11Texture2D = new Guid("6F15AAF2-D208-4E89-9AB4-489535D34F9C");

		// Token: 0x02000049 RID: 73
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct SimpleMaterialCB
		{
			// Token: 0x040001D4 RID: 468
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] InputColor;

			// Token: 0x040001D5 RID: 469
			public float ColorFactor;

			// Token: 0x040001D6 RID: 470
			public float AlphaFactor;

			// Token: 0x040001D7 RID: 471
			public float HueFactor;

			// Token: 0x040001D8 RID: 472
			public float SaturationFactor;

			// Token: 0x040001D9 RID: 473
			public float ValueFactor;

			// Token: 0x040001DA RID: 474
			public int OverlayEnabled;

			// Token: 0x040001DB RID: 475
			public float StartCoordX;

			// Token: 0x040001DC RID: 476
			public float StartCoordY;

			// Token: 0x040001DD RID: 477
			public float SizeX;

			// Token: 0x040001DE RID: 478
			public float SizeY;

			// Token: 0x040001DF RID: 479
			public float OverlayOffsetX;

			// Token: 0x040001E0 RID: 480
			public float OverlayOffsetY;

			// Token: 0x040001E1 RID: 481
			public int CircularMaskingEnabled;

			// Token: 0x040001E2 RID: 482
			public float MaskingCenterX;

			// Token: 0x040001E3 RID: 483
			public float MaskingCenterY;

			// Token: 0x040001E4 RID: 484
			public float MaskingRadius;

			// Token: 0x040001E5 RID: 485
			public float MaskingSmoothingRadius;

			// Token: 0x040001E6 RID: 486
			public float _pad0;

			// Token: 0x040001E7 RID: 487
			public float _pad1;

			// Token: 0x040001E8 RID: 488
			public float _pad2;
		}

		// Token: 0x0200004A RID: 74
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct TextMaterialCB
		{
			// Token: 0x040001E9 RID: 489
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] InputColor;

			// Token: 0x040001EA RID: 490
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] GlowColor;

			// Token: 0x040001EB RID: 491
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] OutlineColor;

			// Token: 0x040001EC RID: 492
			public float OutlineAmount;

			// Token: 0x040001ED RID: 493
			public float ScaleFactor;

			// Token: 0x040001EE RID: 494
			public float SmoothingConstant;

			// Token: 0x040001EF RID: 495
			public float GlowRadius;

			// Token: 0x040001F0 RID: 496
			public float Blur;

			// Token: 0x040001F1 RID: 497
			public float ShadowOffset;

			// Token: 0x040001F2 RID: 498
			public float ShadowAngle;

			// Token: 0x040001F3 RID: 499
			public float ColorFactor;

			// Token: 0x040001F4 RID: 500
			public float AlphaFactor;

			// Token: 0x040001F5 RID: 501
			public float _pad0;

			// Token: 0x040001F6 RID: 502
			public float _pad1;

			// Token: 0x040001F7 RID: 503
			public float _pad2;
		}

		// Token: 0x0200004B RID: 75
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct PrimitivePolygonCB
		{
			// Token: 0x040001F8 RID: 504
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] Color;
		}
	}
}
