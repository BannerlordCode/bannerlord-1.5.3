using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.ScreenSystem
{
	// Token: 0x02000006 RID: 6
	public abstract class ScreenBase
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000026 RID: 38 RVA: 0x00002168 File Offset: 0x00000368
		// (remove) Token: 0x06000027 RID: 39 RVA: 0x000021A0 File Offset: 0x000003A0
		public event ScreenBase.OnLayerAddedEvent OnAddLayer;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000028 RID: 40 RVA: 0x000021D8 File Offset: 0x000003D8
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x00002210 File Offset: 0x00000410
		public event ScreenBase.OnLayerRemovedEvent OnRemoveLayer;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002245 File Offset: 0x00000445
		public IInputContext DebugInput
		{
			get
			{
				return Input.DebugInput;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000224C File Offset: 0x0000044C
		public MBReadOnlyList<ScreenLayer> Layers
		{
			get
			{
				return this._layers;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002254 File Offset: 0x00000454
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000225C File Offset: 0x0000045C
		public bool IsActive { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002265 File Offset: 0x00000465
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000226D File Offset: 0x0000046D
		public bool IsPaused { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002276 File Offset: 0x00000476
		// (set) Token: 0x06000031 RID: 49 RVA: 0x0000227E File Offset: 0x0000047E
		public bool IsInitialized { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002287 File Offset: 0x00000487
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000228F File Offset: 0x0000048F
		public bool IsFinalized { get; private set; }

		// Token: 0x06000034 RID: 52 RVA: 0x00002298 File Offset: 0x00000498
		internal void HandleInitialize()
		{
			Debug.Print(this + "::HandleInitialize", 0, Debug.DebugColor.White, 17592186044416UL);
			if (!this.IsInitialized)
			{
				this.IsInitialized = true;
				this.OnInitialize();
				Debug.ReportMemoryBookmark("ScreenBase Initialized: " + base.GetType().Name);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000022F0 File Offset: 0x000004F0
		internal void HandleFinalize()
		{
			if (this.IsFinalized)
			{
				Debug.FailedAssert("Screen is already finalized", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenBase.cs", "HandleFinalize", 66);
				return;
			}
			Debug.Print(this + "::HandleFinalize", 0, Debug.DebugColor.White, 17592186044416UL);
			if (this.IsInitialized)
			{
				this.IsInitialized = false;
				this.OnFinalize();
				for (int i = this._layers.Count - 1; i >= 0; i--)
				{
					this._layers[i].HandleFinalize();
				}
			}
			this.IsActive = false;
			this.OnAddLayer = null;
			this.OnRemoveLayer = null;
			this.IsFinalized = true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002394 File Offset: 0x00000594
		internal void HandleActivate()
		{
			Debug.Print(this + "::HandleActivate", 0, Debug.DebugColor.White, 17592186044416UL);
			if (!this.IsActive)
			{
				this.IsActive = true;
				this._onReadyPending = true;
				for (int i = this._layers.Count - 1; i >= 0; i--)
				{
					ScreenLayer screenLayer = this._layers[i];
					if (!screenLayer.IsActive)
					{
						screenLayer.HandleActivate();
					}
				}
				this.OnActivate();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000240C File Offset: 0x0000060C
		internal void HandleDeactivate()
		{
			Debug.Print(this + "::HandleDeactivate", 0, Debug.DebugColor.White, 17592186044416UL);
			if (this.IsActive)
			{
				this.IsActive = false;
				for (int i = this._layers.Count - 1; i >= 0; i--)
				{
					ScreenLayer screenLayer = this._layers[i];
					if (screenLayer.IsActive)
					{
						screenLayer.HandleDeactivate();
					}
				}
				this.OnDeactivate();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002480 File Offset: 0x00000680
		internal void HandleResume()
		{
			Debug.Print(this + "::HandleResume", 0, Debug.DebugColor.White, 17592186044416UL);
			if (this.IsPaused)
			{
				for (int i = this._layers.Count - 1; i >= 0; i--)
				{
					ScreenLayer screenLayer = this._layers[i];
					if (!screenLayer.IsActive)
					{
						screenLayer.HandleActivate();
					}
				}
				this.IsPaused = false;
				this.OnResume();
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000024F4 File Offset: 0x000006F4
		internal void HandlePause()
		{
			Debug.Print(this + "::HandlePause", 0, Debug.DebugColor.White, 17592186044416UL);
			if (!this.IsPaused)
			{
				for (int i = this._layers.Count - 1; i >= 0; i--)
				{
					ScreenLayer screenLayer = this._layers[i];
					if (screenLayer.IsActive)
					{
						screenLayer.HandleDeactivate();
					}
				}
				this.IsPaused = true;
				this.OnPause();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002568 File Offset: 0x00000768
		internal void FrameTick(float dt)
		{
			if (this.IsActive)
			{
				if (this._onReadyPending)
				{
					this._onReadyPending = false;
					this.OnReady();
				}
				this.OnFrameTick(dt);
			}
			InputContext inputContext;
			if ((inputContext = this.DebugInput as InputContext) != null)
			{
				if (this.IsActive)
				{
					inputContext.RegisterDownKeys();
					return;
				}
				inputContext.ResetLastDownKeys();
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000025BD File Offset: 0x000007BD
		internal void PostFrameTick(float dt)
		{
			if (this.IsActive)
			{
				this.OnPostFrameTick(dt);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000025D0 File Offset: 0x000007D0
		public void ActivateAllLayers()
		{
			foreach (ScreenLayer screenLayer in this._layers)
			{
				if (!screenLayer.IsActive)
				{
					screenLayer.HandleActivate();
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000262C File Offset: 0x0000082C
		public void DeactivateAllLayers()
		{
			foreach (ScreenLayer screenLayer in this._layers)
			{
				if (screenLayer.IsActive)
				{
					screenLayer.HandleDeactivate();
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002688 File Offset: 0x00000888
		public void Deactivate()
		{
			if (this.IsActive)
			{
				this.HandleDeactivate();
				this.IsActive = false;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000269F File Offset: 0x0000089F
		public void Activate()
		{
			if (!this.IsActive)
			{
				this.HandleActivate();
				this.IsActive = true;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000026B8 File Offset: 0x000008B8
		public virtual void UpdateLayout()
		{
			for (int i = 0; i < this._layers.Count; i++)
			{
				if (!this._layers[i].IsFinalized)
				{
					this._layers[i].UpdateLayout();
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000026FF File Offset: 0x000008FF
		internal void IdleTick(float dt)
		{
			this.OnIdleTick(dt);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002708 File Offset: 0x00000908
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000270A File Offset: 0x0000090A
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000270C File Offset: 0x0000090C
		protected virtual void OnPause()
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000270E File Offset: 0x0000090E
		protected virtual void OnResume()
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002710 File Offset: 0x00000910
		protected virtual void OnActivate()
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002712 File Offset: 0x00000912
		protected virtual void OnDeactivate()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002714 File Offset: 0x00000914
		protected virtual void OnFrameTick(float dt)
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002716 File Offset: 0x00000916
		protected virtual void OnReady()
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002718 File Offset: 0x00000918
		protected virtual void OnPostFrameTick(float dt)
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000271A File Offset: 0x0000091A
		protected virtual void OnIdleTick(float dt)
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000271C File Offset: 0x0000091C
		public virtual void OnFocusChangeOnGameWindow(bool focusGained)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000271E File Offset: 0x0000091E
		public void AddComponent(ScreenComponent component)
		{
			this._components.Add(component);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000272C File Offset: 0x0000092C
		public T FindComponent<T>() where T : ScreenComponent
		{
			foreach (ScreenComponent screenComponent in this._components)
			{
				if (screenComponent is T)
				{
					return (T)((object)screenComponent);
				}
			}
			return default(T);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002794 File Offset: 0x00000994
		public void AddLayer(ScreenLayer layer)
		{
			if (layer == null || layer.IsFinalized)
			{
				Debug.FailedAssert("Trying to add a null or finalized layer", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenBase.cs", "AddLayer", 350);
				return;
			}
			if (this._layers.Contains(layer))
			{
				Debug.FailedAssert("Layer is already added to the screen!", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenBase.cs", "AddLayer", 369);
				return;
			}
			this._layers.Add(layer);
			this._layers.Sort();
			if (this.IsActive)
			{
				layer.LastActiveState = true;
				layer.HandleActivate();
			}
			ScreenBase.OnLayerAddedEvent onAddLayer = this.OnAddLayer;
			if (onAddLayer == null)
			{
				return;
			}
			onAddLayer(layer);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000282C File Offset: 0x00000A2C
		public void RemoveLayer(ScreenLayer layer)
		{
			if (this.IsActive)
			{
				layer.LastActiveState = false;
				layer.HandleDeactivate();
			}
			layer.HandleFinalize();
			this._layers.Remove(layer);
			ScreenBase.OnLayerRemovedEvent onRemoveLayer = this.OnRemoveLayer;
			if (onRemoveLayer != null)
			{
				onRemoveLayer(layer);
			}
			ScreenManager.RefreshGlobalOrder();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002878 File Offset: 0x00000A78
		public bool HasLayer(ScreenLayer layer)
		{
			return this._layers.Contains(layer);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002888 File Offset: 0x00000A88
		public T FindLayer<T>() where T : ScreenLayer
		{
			foreach (ScreenLayer screenLayer in this._layers)
			{
				if (screenLayer is T)
				{
					return (T)((object)screenLayer);
				}
			}
			return default(T);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000028F0 File Offset: 0x00000AF0
		public T FindLayer<T>(string name) where T : ScreenLayer
		{
			using (List<ScreenLayer>.Enumerator enumerator = this._layers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null && t.Name == name)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002970 File Offset: 0x00000B70
		public void SetLayerCategoriesState(string[] categoryIds, bool isActive)
		{
			foreach (ScreenLayer screenLayer in this._layers)
			{
				if (categoryIds.IndexOf(screenLayer.Name) >= 0)
				{
					if (isActive && !screenLayer.IsActive)
					{
						screenLayer.HandleActivate();
					}
					else if (!isActive && screenLayer.IsActive)
					{
						screenLayer.HandleDeactivate();
					}
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000029F0 File Offset: 0x00000BF0
		public void SetLayerCategoriesStateAndToggleOthers(string[] categoryIds, bool isActive)
		{
			foreach (ScreenLayer screenLayer in this._layers)
			{
				if (categoryIds.IndexOf(screenLayer.Name) >= 0)
				{
					if (isActive && !screenLayer.IsActive)
					{
						screenLayer.HandleActivate();
					}
					else if (!isActive && screenLayer.IsActive)
					{
						screenLayer.HandleDeactivate();
					}
				}
				else if (screenLayer.IsActive)
				{
					screenLayer.HandleDeactivate();
				}
				else
				{
					screenLayer.HandleActivate();
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A88 File Offset: 0x00000C88
		public void SetLayerCategoriesStateAndDeactivateOthers(string[] categoryIds, bool isActive)
		{
			foreach (ScreenLayer screenLayer in this._layers)
			{
				if (categoryIds.IndexOf(screenLayer.Name) >= 0)
				{
					if (isActive && !screenLayer.IsActive)
					{
						screenLayer.HandleActivate();
					}
					else if (!isActive && screenLayer.IsActive)
					{
						screenLayer.HandleDeactivate();
					}
				}
				else if (screenLayer.IsActive)
				{
					screenLayer.HandleDeactivate();
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002B18 File Offset: 0x00000D18
		protected ScreenBase()
		{
			this.IsPaused = true;
			this.IsActive = false;
			this._components = new List<ScreenComponent>();
			this._layers = new MBList<ScreenLayer>();
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002B44 File Offset: 0x00000D44
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002B4C File Offset: 0x00000D4C
		public virtual bool MouseVisible { get; set; }

		// Token: 0x0600005A RID: 90 RVA: 0x00002B58 File Offset: 0x00000D58
		internal void Update(IReadOnlyList<int> lastKeysPressed)
		{
			if (this.IsActive)
			{
				foreach (ScreenLayer screenLayer in this._layers)
				{
					if (screenLayer.IsActive)
					{
						screenLayer.Update(lastKeysPressed);
					}
				}
			}
		}

		// Token: 0x04000013 RID: 19
		private readonly List<ScreenComponent> _components;

		// Token: 0x04000014 RID: 20
		private readonly MBList<ScreenLayer> _layers;

		// Token: 0x0400001B RID: 27
		private bool _onReadyPending;

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x060000E4 RID: 228
		public delegate void OnLayerAddedEvent(ScreenLayer addedLayer);

		// Token: 0x0200000C RID: 12
		// (Invoke) Token: 0x060000E8 RID: 232
		public delegate void OnLayerRemovedEvent(ScreenLayer removedLayer);
	}
}
