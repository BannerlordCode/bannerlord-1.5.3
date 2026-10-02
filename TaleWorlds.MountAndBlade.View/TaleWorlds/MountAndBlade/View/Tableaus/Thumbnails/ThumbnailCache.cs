using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004E RID: 78
	public abstract class ThumbnailCache<T> : IThumbnailCache where T : ThumbnailCreationData
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600028D RID: 653 RVA: 0x000114BD File Offset: 0x0000F6BD
		// (set) Token: 0x0600028E RID: 654 RVA: 0x000114C5 File Offset: 0x0000F6C5
		public int Count { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600028F RID: 655 RVA: 0x000114CE File Offset: 0x0000F6CE
		public int RenderCallbackCount
		{
			get
			{
				return this._renderCallbacks.Count;
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000114DB File Offset: 0x0000F6DB
		public ThumbnailCache(int capacity)
		{
			this._capacity = capacity;
			this._map = new Dictionary<string, ThumbnailCacheNode>(capacity);
			this._renderCallbacks = new Dictionary<string, RenderCallbackCollection>();
			this.Count = 0;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00011513 File Offset: 0x0000F713
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00011515 File Offset: 0x0000F715
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00011517 File Offset: 0x0000F717
		protected virtual void OnTick(float dt)
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00011519 File Offset: 0x0000F719
		protected virtual void OnClear()
		{
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001151B File Offset: 0x0000F71B
		protected virtual void OnImguiTick()
		{
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001151D File Offset: 0x0000F71D
		protected virtual void OnRequestCancelled(string renderId)
		{
		}

		// Token: 0x06000297 RID: 663
		protected abstract TextureCreationInfo OnCreateTexture(T thumbnailCreationData);

		// Token: 0x06000298 RID: 664
		protected abstract bool OnReleaseTexture(T thumbnailCreationData);

		// Token: 0x06000299 RID: 665 RVA: 0x00011520 File Offset: 0x0000F720
		bool IThumbnailCache.OnThumbnailRenderCompleted(string renderId, Texture renderTarget)
		{
			if (this._renderCallbacks.ContainsKey(renderId))
			{
				foreach (Action<Texture> action in this._renderCallbacks[renderId].SetActions)
				{
					if (action != null)
					{
						action(renderTarget);
					}
				}
				this._renderCallbacks.Remove(renderId);
				return true;
			}
			return false;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000115A4 File Offset: 0x0000F7A4
		public TextureCreationInfo CreateTexture(ThumbnailCreationData thumbnailCreationData)
		{
			T t;
			if ((t = thumbnailCreationData as T) != null)
			{
				TextureCreationInfo textureCreationInfo = this.OnCreateTexture(t);
				thumbnailCreationData.IsProcessed = true;
				return textureCreationInfo;
			}
			return default(TextureCreationInfo);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000115E0 File Offset: 0x0000F7E0
		public bool ReleaseTexture(ThumbnailCreationData thumbnailCreationData)
		{
			bool flag = false;
			T t;
			if ((t = thumbnailCreationData as T) != null && this.OnReleaseTexture(t))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0001160F File Offset: 0x0000F80F
		void IThumbnailCache.Initialize(ThumbnailCreatorView thumbnailCreatorView)
		{
			this._thumbnailCreatorView = thumbnailCreatorView;
			this.OnInitialize();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0001161E File Offset: 0x0000F81E
		void IThumbnailCache.Destroy()
		{
			this.OnFinalize();
			this._capacity = 0;
			this.Count = 0;
			this._thumbnailCreatorView = null;
			this._nodeComparer = null;
			this._map.Clear();
			this._map = null;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00011654 File Offset: 0x0000F854
		void IThumbnailCache.Clear(bool releaseImmediately)
		{
			foreach (KeyValuePair<string, ThumbnailCacheNode> keyValuePair in this._map)
			{
				string key = keyValuePair.Key;
				this.RemoveRenderCallbacksForKey(key);
				this._thumbnailCreatorView.CancelRequest(key);
				if (releaseImmediately)
				{
					Texture value = keyValuePair.Value.Value;
					if (value != null)
					{
						value.ReleaseImmediately();
					}
				}
				else
				{
					Texture value2 = keyValuePair.Value.Value;
					if (value2 != null)
					{
						value2.Release();
					}
				}
				keyValuePair.Value.Value = null;
				int count = this.Count;
				this.Count = count - 1;
				this.OnRequestCancelled(key);
			}
			this._map.Clear();
			this._renderCallbacks.Clear();
			this.Count = 0;
			this.OnClear();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00011738 File Offset: 0x0000F938
		bool IThumbnailCache.GetValue(string key, out Texture texture)
		{
			texture = null;
			ThumbnailCacheNode thumbnailCacheNode;
			if (this._map.TryGetValue(key, out thumbnailCacheNode))
			{
				thumbnailCacheNode.FrameNo = Utilities.EngineFrameNo;
				texture = thumbnailCacheNode.Value;
				return true;
			}
			return false;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00011770 File Offset: 0x0000F970
		void IThumbnailCache.Add(string key, Texture value)
		{
			ThumbnailCacheNode thumbnailCacheNode;
			if (!this._map.TryGetValue(key, out thumbnailCacheNode))
			{
				ThumbnailCacheNode thumbnailCacheNode2 = new ThumbnailCacheNode(key, value, Utilities.EngineFrameNo);
				this._map[key] = thumbnailCacheNode2;
				int num = this.Count;
				this.Count = num + 1;
				return;
			}
			if (thumbnailCacheNode.Value != null && thumbnailCacheNode.Value != value)
			{
				if (thumbnailCacheNode.Value != null && !thumbnailCacheNode.Value.IsReleased)
				{
					Debug.FailedAssert("Setting a texture to a node without clearing the old one", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\Thumbnails\\ThumbnailCache.cs", "Add", 266);
				}
				ThumbnailCacheNode thumbnailCacheNode3 = new ThumbnailCacheNode(key, value, Utilities.EngineFrameNo);
				this._map[key] = thumbnailCacheNode3;
				int num = this.Count;
				this.Count = num + 1;
				return;
			}
			thumbnailCacheNode.Value = value;
			thumbnailCacheNode.FrameNo = Utilities.EngineFrameNo;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00011848 File Offset: 0x0000FA48
		bool IThumbnailCache.AddReference(string key)
		{
			ThumbnailCacheNode thumbnailCacheNode;
			if (this._map.TryGetValue(key, out thumbnailCacheNode))
			{
				thumbnailCacheNode.ReferenceCount++;
				return true;
			}
			return false;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00011878 File Offset: 0x0000FA78
		bool IThumbnailCache.RemoveReference(string key)
		{
			ThumbnailCacheNode thumbnailCacheNode;
			if (this._map.TryGetValue(key, out thumbnailCacheNode))
			{
				thumbnailCacheNode.ReferenceCount--;
				if (thumbnailCacheNode.ReferenceCount < 0)
				{
					Debug.FailedAssert("Thumbnail cache reference count is below 0", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\Thumbnails\\ThumbnailCache.cs", "RemoveReference", 304);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000118C8 File Offset: 0x0000FAC8
		void IThumbnailCache.ClearUnusedCache()
		{
			if (this.Count > this._capacity)
			{
				int num = this.Count - Math.Max(this._capacity / 2, 1);
				List<ThumbnailCacheNode> list = new List<ThumbnailCacheNode>();
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, ThumbnailCacheNode> keyValuePair in this._map)
				{
					if (keyValuePair.Value.ReferenceCount <= 0)
					{
						list.Add(keyValuePair.Value);
						list2.Add(keyValuePair.Key);
						num--;
					}
					if (num == 0)
					{
						break;
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					this.RemoveThumbnailCacheNode(list[i], true);
				}
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000119A4 File Offset: 0x0000FBA4
		void IThumbnailCache.Tick(float dt)
		{
			if (this.Count > this._capacity)
			{
				int num = this.Count - Math.Max(this._capacity / 2, 1);
				List<ThumbnailCacheNode> list = new List<ThumbnailCacheNode>();
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, ThumbnailCacheNode> keyValuePair in this._map)
				{
					if (keyValuePair.Value.ReferenceCount <= 0)
					{
						list.Add(keyValuePair.Value);
						list2.Add(keyValuePair.Key);
						num--;
					}
					if (num == 0)
					{
						break;
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					this.RemoveThumbnailCacheNode(list[i], true);
				}
			}
			this.OnTick(dt);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00011A88 File Offset: 0x0000FC88
		protected void RemoveThumbnailCacheNode(ThumbnailCacheNode node, bool releaseTexture = true)
		{
			string key = node.Key;
			this.RemoveRenderCallbacksForKey(key);
			this._map.Remove(key);
			this._thumbnailCreatorView.CancelRequest(key);
			if (releaseTexture && node.Value != null)
			{
				node.Value.Release();
				node.Value = null;
			}
			int count = this.Count;
			this.Count = count - 1;
			this.OnRequestCancelled(key);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00011AF8 File Offset: 0x0000FCF8
		private void RemoveRenderCallbacksForKey(string renderId)
		{
			RenderCallbackCollection renderCallbackCollection;
			if (this._renderCallbacks.TryGetValue(renderId, out renderCallbackCollection))
			{
				for (int i = 0; i < renderCallbackCollection.CancelActions.Count; i++)
				{
					Action action = renderCallbackCollection.CancelActions[i];
					if (action != null)
					{
						action();
					}
				}
				renderCallbackCollection.SetActions.Clear();
				this._renderCallbacks.Remove(renderId);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00011B60 File Offset: 0x0000FD60
		void IThumbnailCache.PrintToImgui()
		{
			int totalMemorySize = this.GetTotalMemorySize();
			Imgui.Text(base.GetType().Name);
			Imgui.NextColumn();
			Imgui.Text(this.Count.ToString());
			Imgui.NextColumn();
			Imgui.Text(ThumbnailCache<T>.ByteWidthToString(totalMemorySize));
			Imgui.NextColumn();
			Imgui.Text(this._renderCallbacks.Count.ToString());
			Imgui.NextColumn();
			this.OnImguiTick();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		protected static Camera CreateCamera(float left, float right, float bottom, float top, float near, float far)
		{
			Camera camera = Camera.CreateCamera();
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin.z = 400f;
			camera.Frame = identity;
			camera.LookAt(new Vec3(0f, 0f, 400f, -1f), new Vec3(0f, 0f, 0f, -1f), new Vec3(0f, 1f, 0f, -1f));
			camera.SetViewVolume(false, left, right, bottom, top, near, far);
			return camera;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00011C64 File Offset: 0x0000FE64
		protected static string CreateDebugIdFrom(string renderId, string typeId, string additionalInfo = "")
		{
			string text = Common.CreateNanoIdFrom(renderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("uit");
			stringBuilder.Append('_');
			stringBuilder.Append(typeId);
			stringBuilder.Append('_');
			stringBuilder.Append(text);
			stringBuilder.Append('_');
			stringBuilder.Append(additionalInfo);
			string text2 = stringBuilder.ToString();
			if (text2.Length > 127)
			{
				text2 = text2.Substring(0, 127);
			}
			return text2;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00011CD8 File Offset: 0x0000FED8
		protected int GetTotalMemorySize()
		{
			int num = 0;
			foreach (ThumbnailCacheNode thumbnailCacheNode in this._map.Values)
			{
				int num2 = num;
				Texture value = thumbnailCacheNode.Value;
				num = num2 + ((value != null) ? value.MemorySize : 0);
			}
			return num;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00011D44 File Offset: 0x0000FF44
		protected static string ByteWidthToString(int bytes)
		{
			double num = Math.Log((double)bytes);
			if (bytes == 0)
			{
				num = 0.0;
			}
			int num2 = (int)(num / Math.Log(1024.0));
			char c = " KMGTPE"[num2];
			return ((double)bytes / Math.Pow(1024.0, (double)num2)).ToString("0.00") + " " + c.ToString() + "      ";
		}

		// Token: 0x04000157 RID: 343
		protected int _capacity;

		// Token: 0x04000158 RID: 344
		protected ThumbnailCreatorView _thumbnailCreatorView;

		// Token: 0x04000159 RID: 345
		protected Dictionary<string, ThumbnailCacheNode> _map;

		// Token: 0x0400015A RID: 346
		protected NodeComparer _nodeComparer = new NodeComparer();

		// Token: 0x0400015B RID: 347
		protected Dictionary<string, RenderCallbackCollection> _renderCallbacks;
	}
}
