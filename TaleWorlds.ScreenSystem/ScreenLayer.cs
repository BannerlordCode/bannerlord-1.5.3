using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.ScreenSystem
{
	// Token: 0x02000008 RID: 8
	public abstract class ScreenLayer : IComparable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600005C RID: 92 RVA: 0x00002BC4 File Offset: 0x00000DC4
		// (remove) Token: 0x0600005D RID: 93 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public static event Action<ScreenLayer> OnLayerActiveStateChanged;

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002C2B File Offset: 0x00000E2B
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002C33 File Offset: 0x00000E33
		public string Name { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002C3C File Offset: 0x00000E3C
		public float Scale
		{
			get
			{
				return ScreenManager.Scale;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002C43 File Offset: 0x00000E43
		public Vec2 UsableArea
		{
			get
			{
				return ScreenManager.UsableArea;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002C4A File Offset: 0x00000E4A
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002C52 File Offset: 0x00000E52
		public InputContext Input { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002C5B File Offset: 0x00000E5B
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002C63 File Offset: 0x00000E63
		public InputRestrictions InputRestrictions { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002C6C File Offset: 0x00000E6C
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002C74 File Offset: 0x00000E74
		public bool LastActiveState { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002C7D File Offset: 0x00000E7D
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002C85 File Offset: 0x00000E85
		public bool IsFinalized { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002C8E File Offset: 0x00000E8E
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002C96 File Offset: 0x00000E96
		public bool IsActive { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002C9F File Offset: 0x00000E9F
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002CA7 File Offset: 0x00000EA7
		public bool IsHitThisFrame { get; internal set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002CB0 File Offset: 0x00000EB0
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public bool IsFocusLayer { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002CC1 File Offset: 0x00000EC1
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002CC9 File Offset: 0x00000EC9
		public CursorType ActiveCursor { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002CD2 File Offset: 0x00000ED2
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002CDA File Offset: 0x00000EDA
		protected InputType _usedInputs { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002CE3 File Offset: 0x00000EE3
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002CEB File Offset: 0x00000EEB
		public int ScreenOrderInLastFrame { get; internal set; }

		// Token: 0x06000076 RID: 118 RVA: 0x00002CF4 File Offset: 0x00000EF4
		protected ScreenLayer(string name, int localOrder)
		{
			this.InputRestrictions = new InputRestrictions(localOrder);
			this.Input = new InputContext();
			this.Name = name;
			this.LastActiveState = true;
			this.IsFinalized = false;
			this.IsActive = false;
			this.IsFocusLayer = false;
			this._usedInputs = InputType.None;
			this.ActiveCursor = CursorType.Default;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002D4F File Offset: 0x00000F4F
		protected internal virtual void Tick(float dt)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002D51 File Offset: 0x00000F51
		protected internal virtual void LateUpdate(float dt)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002D53 File Offset: 0x00000F53
		protected internal virtual void RenderTick(float dt)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002D55 File Offset: 0x00000F55
		protected internal virtual void Update(IReadOnlyList<int> lastKeysPressed)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002D57 File Offset: 0x00000F57
		internal void HandleFinalize()
		{
			if (this.IsFinalized)
			{
				Debug.FailedAssert("Screen layer is already finalized", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.ScreenSystem\\ScreenLayer.cs", "HandleFinalize", 74);
				return;
			}
			this.OnFinalize();
			this.IsFinalized = true;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002D85 File Offset: 0x00000F85
		internal void HandleGainFocus()
		{
			this.Input.ResetLastDownKeys();
			this.OnGainFocus();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002D98 File Offset: 0x00000F98
		internal void HandleLoseFocus()
		{
			this.Input.ResetLastDownKeys();
			this.OnLoseFocus();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002DAB File Offset: 0x00000FAB
		protected virtual void OnActivate()
		{
			this.IsFinalized = false;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002DB4 File Offset: 0x00000FB4
		protected virtual void OnDeactivate()
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002DB6 File Offset: 0x00000FB6
		protected internal virtual void OnGainFocus()
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002DB8 File Offset: 0x00000FB8
		protected internal virtual void OnLoseFocus()
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002DBA File Offset: 0x00000FBA
		internal void HandleActivate()
		{
			if (!this.IsActive)
			{
				this.IsActive = true;
				this.OnActivate();
				Action<ScreenLayer> onLayerActiveStateChanged = ScreenLayer.OnLayerActiveStateChanged;
				if (onLayerActiveStateChanged == null)
				{
					return;
				}
				onLayerActiveStateChanged(this);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002DE1 File Offset: 0x00000FE1
		internal void HandleDeactivate()
		{
			if (this.IsActive)
			{
				this.OnDeactivate();
				this.IsActive = false;
				ScreenManager.TryLoseFocus(this);
				Action<ScreenLayer> onLayerActiveStateChanged = ScreenLayer.OnLayerActiveStateChanged;
				if (onLayerActiveStateChanged == null)
				{
					return;
				}
				onLayerActiveStateChanged(this);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002E0E File Offset: 0x0000100E
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002E10 File Offset: 0x00001010
		protected internal virtual void RefreshGlobalOrder(ref int currentOrder)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002E14 File Offset: 0x00001014
		public virtual void DrawDebugInfo()
		{
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Order: {0}", this.InputRestrictions.Order));
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Is Layer Focusable: {0}", this.IsFocusLayer));
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Is FocusedLayer: {0}", this == ScreenManager.FocusedLayer));
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Keys Allowed: {0}", this.Input.IsKeysAllowed));
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Controller Allowed: {0}", this.Input.IsControllerAllowed));
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Mouse Button Allowed: {0}", this.Input.IsMouseButtonAllowed));
			ScreenManager.EngineInterface.DrawDebugText(string.Format("Mouse Wheel Allowed: {0}", this.Input.IsMouseWheelAllowed));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002F15 File Offset: 0x00001115
		public virtual void EarlyProcessEvents(InputType handledInputs)
		{
			this._usedInputs = handledInputs;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002F20 File Offset: 0x00001120
		public virtual void ProcessEvents()
		{
			this.Input.IsKeysAllowed = this._usedInputs.HasAnyFlag(InputType.Key);
			this.Input.IsMouseButtonAllowed = this._usedInputs.HasAnyFlag(InputType.MouseButton);
			this.Input.IsMouseWheelAllowed = this._usedInputs.HasAnyFlag(InputType.MouseWheel);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002F72 File Offset: 0x00001172
		public virtual bool HitTest(Vector2 position)
		{
			return false;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002F75 File Offset: 0x00001175
		public virtual bool HitTest()
		{
			return false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002F78 File Offset: 0x00001178
		public virtual bool FocusTest()
		{
			return false;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002F7B File Offset: 0x0000117B
		public InputUsageMask InputUsageMask
		{
			get
			{
				return this.InputRestrictions.InputUsageMask;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002F88 File Offset: 0x00001188
		public virtual bool IsFocusedOnInput()
		{
			return false;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002F8B File Offset: 0x0000118B
		public virtual void OnOnScreenKeyboardDone(string inputText)
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002F8D File Offset: 0x0000118D
		public virtual void OnOnScreenKeyboardCanceled()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002F90 File Offset: 0x00001190
		public int CompareTo(object obj)
		{
			ScreenLayer screenLayer = obj as ScreenLayer;
			if (screenLayer == null)
			{
				return 1;
			}
			if (screenLayer == this)
			{
				return 0;
			}
			if (this.InputRestrictions.Order == screenLayer.InputRestrictions.Order)
			{
				return this.InputRestrictions.Id.CompareTo(screenLayer.InputRestrictions.Id);
			}
			return this.InputRestrictions.Order.CompareTo(screenLayer.InputRestrictions.Order);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003004 File Offset: 0x00001204
		public virtual void UpdateLayout()
		{
		}
	}
}
