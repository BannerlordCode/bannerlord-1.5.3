using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000089 RID: 137
	public abstract class ScriptComponentBehavior : DotNetObject
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x0000D98F File Offset: 0x0000BB8F
		protected void InvalidateWeakPointersIfValid()
		{
			this._scriptComponent.ManualInvalidate();
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0000D99C File Offset: 0x0000BB9C
		public WeakGameEntity GameEntity
		{
			get
			{
				return this._gameEntity;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x0000D9BD File Offset: 0x0000BBBD
		public ManagedScriptComponent ScriptComponent
		{
			get
			{
				WeakNativeObjectReference scriptComponent = this._scriptComponent;
				return ((scriptComponent != null) ? scriptComponent.GetNativeObject() : null) as ManagedScriptComponent;
			}
			private set
			{
				this._scriptComponent = new WeakNativeObjectReference(value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0000D9CB File Offset: 0x0000BBCB
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x0000D9D3 File Offset: 0x0000BBD3
		private protected ManagedScriptHolder ManagedScriptHolder { protected get; private set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x0000D9F5 File Offset: 0x0000BBF5
		public Scene Scene
		{
			get
			{
				WeakNativeObjectReference scene = this._scene;
				return ((scene != null) ? scene.GetNativeObject() : null) as Scene;
			}
			private set
			{
				this._scene = new WeakNativeObjectReference(value);
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0000DA03 File Offset: 0x0000BC03
		static ScriptComponentBehavior()
		{
			if (ScriptComponentBehavior.CachedFields == null)
			{
				ScriptComponentBehavior.CachedFields = new Dictionary<string, string[]>();
				ScriptComponentBehavior.CacheEditableFieldsForAllScriptComponents();
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0000DA37 File Offset: 0x0000BC37
		internal void Construct(UIntPtr myEntityPtr, ManagedScriptComponent scriptComponent)
		{
			this._gameEntity = new WeakGameEntity(myEntityPtr);
			this.Scene = this._gameEntity.Scene;
			this.ScriptComponent = scriptComponent;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0000DA5D File Offset: 0x0000BC5D
		internal void SetOwnerManagedScriptHolder(ManagedScriptHolder managedScriptHolder)
		{
			this.ManagedScriptHolder = managedScriptHolder;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0000DA66 File Offset: 0x0000BC66
		private void SetScriptComponentToTickAux(ScriptComponentBehavior.TickRequirement value)
		{
			if (this._lastTickRequirement != value)
			{
				this.ManagedScriptHolder.UpdateTickRequirement(this, this._lastTickRequirement, value);
				this._lastTickRequirement = value;
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0000DA8B File Offset: 0x0000BC8B
		public void SetScriptComponentToTick(ScriptComponentBehavior.TickRequirement tickReq)
		{
			if (this.ManagedScriptHolder != null)
			{
				this.SetScriptComponentToTickAux(tickReq);
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		public void SetScriptComponentToTickMT(ScriptComponentBehavior.TickRequirement value)
		{
			if (this.ManagedScriptHolder != null)
			{
				object addRemoveLockObject = this.ManagedScriptHolder.AddRemoveLockObject;
				lock (addRemoveLockObject)
				{
					this.SetScriptComponentToTickAux(value);
				}
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0000DAEC File Offset: 0x0000BCEC
		[EngineCallback(null, false)]
		internal void AddScriptComponentToTick()
		{
			List<ScriptComponentBehavior> prefabScriptComponents = ScriptComponentBehavior._prefabScriptComponents;
			lock (prefabScriptComponents)
			{
				if (!ScriptComponentBehavior._prefabScriptComponents.Contains(this))
				{
					ScriptComponentBehavior._prefabScriptComponents.Add(this);
				}
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0000DB40 File Offset: 0x0000BD40
		[EngineCallback(null, false)]
		internal void RegisterAsPrefabScriptComponent()
		{
			List<ScriptComponentBehavior> prefabScriptComponents = ScriptComponentBehavior._prefabScriptComponents;
			lock (prefabScriptComponents)
			{
				if (!ScriptComponentBehavior._prefabScriptComponents.Contains(this))
				{
					ScriptComponentBehavior._prefabScriptComponents.Add(this);
				}
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0000DB94 File Offset: 0x0000BD94
		[EngineCallback(null, false)]
		internal void DeregisterAsPrefabScriptComponent()
		{
			List<ScriptComponentBehavior> prefabScriptComponents = ScriptComponentBehavior._prefabScriptComponents;
			lock (prefabScriptComponents)
			{
				ScriptComponentBehavior._prefabScriptComponents.Remove(this);
			}
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0000DBDC File Offset: 0x0000BDDC
		[EngineCallback(null, false)]
		internal void RegisterAsUndoStackScriptComponent()
		{
			if (!ScriptComponentBehavior._undoStackScriptComponents.Contains(this))
			{
				ScriptComponentBehavior._undoStackScriptComponents.Add(this);
			}
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0000DBF6 File Offset: 0x0000BDF6
		[EngineCallback(null, false)]
		internal void DeregisterAsUndoStackScriptComponent()
		{
			if (ScriptComponentBehavior._undoStackScriptComponents.Contains(this))
			{
				ScriptComponentBehavior._undoStackScriptComponents.Remove(this);
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0000DC11 File Offset: 0x0000BE11
		[EngineCallback(null, false)]
		protected internal virtual void SetScene(Scene scene)
		{
			this.Scene = scene;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0000DC1A File Offset: 0x0000BE1A
		[EngineCallback(null, false)]
		protected internal virtual void OnInit()
		{
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		[EngineCallback(null, false)]
		protected internal void HandleOnRemoved(int removeReason)
		{
			this.OnRemoved(removeReason);
			this._scene = null;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		protected virtual void OnRemoved(int removeReason)
		{
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0000DC2E File Offset: 0x0000BE2E
		public virtual ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.None;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0000DC31 File Offset: 0x0000BE31
		protected internal virtual bool CanPhysicsCollideBetweenTwoEntities(WeakGameEntity myEntity, BodyFlags myEntityBodyFlags, WeakGameEntity otherEntity, BodyFlags otherEntityBodyFlags)
		{
			return true;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0000DC34 File Offset: 0x0000BE34
		protected internal virtual void OnFixedTick(float fixedDt)
		{
			Debug.FailedAssert("This base function should never be called.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\ScriptComponentBehavior.cs", "OnFixedTick", 253);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0000DC4F File Offset: 0x0000BE4F
		protected internal virtual void OnParallelFixedTick(float fixedDt)
		{
			Debug.FailedAssert("This base function should never be called.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\ScriptComponentBehavior.cs", "OnParallelFixedTick", 259);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0000DC6A File Offset: 0x0000BE6A
		protected internal virtual void OnTick(float dt)
		{
			Debug.FailedAssert("This base function should never be called.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\ScriptComponentBehavior.cs", "OnTick", 265);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0000DC85 File Offset: 0x0000BE85
		protected internal virtual void OnTickParallel(float dt)
		{
			Debug.FailedAssert("This base function should never be called.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\ScriptComponentBehavior.cs", "OnTickParallel", 271);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		protected internal virtual void OnTickParallel2(float dt)
		{
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0000DCA2 File Offset: 0x0000BEA2
		protected internal virtual void OnTickParallel3(float dt)
		{
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0000DCA4 File Offset: 0x0000BEA4
		protected internal virtual void OnTickOccasionally(float currentFrameDeltaTime)
		{
			Debug.FailedAssert("This base function should never be called.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\ScriptComponentBehavior.cs", "OnTickOccasionally", 289);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0000DCBF File Offset: 0x0000BEBF
		[EngineCallback(null, false)]
		protected internal virtual void OnPreInit()
		{
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0000DCC1 File Offset: 0x0000BEC1
		[EngineCallback(null, false)]
		protected internal virtual void OnEditorInit()
		{
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0000DCC3 File Offset: 0x0000BEC3
		[EngineCallback(null, false)]
		protected internal virtual void OnEditorTick(float dt)
		{
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0000DCC5 File Offset: 0x0000BEC5
		[EngineCallback(null, false)]
		protected internal virtual void OnEditorValidate()
		{
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0000DCC7 File Offset: 0x0000BEC7
		[EngineCallback(null, false)]
		protected internal virtual bool IsOnlyVisual()
		{
			return false;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0000DCCA File Offset: 0x0000BECA
		[EngineCallback(null, false)]
		protected internal virtual bool MovesEntity()
		{
			return true;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0000DCCD File Offset: 0x0000BECD
		[EngineCallback(null, false)]
		protected internal virtual bool DisablesOroCreation()
		{
			return true;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0000DCD0 File Offset: 0x0000BED0
		[EngineCallback(null, false)]
		protected internal virtual void OnEditorVariableChanged(string variableName)
		{
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0000DCD2 File Offset: 0x0000BED2
		protected internal virtual bool SkeletonPostIntegrateCallback(AnimResult animResult)
		{
			return false;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		[EngineCallback(null, false)]
		internal static bool SkeletonPostIntegrateCallbackAux(ScriptComponentBehavior script, UIntPtr animResultPointer)
		{
			AnimResult animResult = AnimResult.CreateWithPointer(animResultPointer);
			return script.SkeletonPostIntegrateCallback(animResult);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0000DCF3 File Offset: 0x0000BEF3
		[EngineCallback(null, false)]
		protected internal virtual void OnSceneSave(string saveFolder)
		{
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0000DCF5 File Offset: 0x0000BEF5
		[EngineCallback(null, false)]
		protected internal virtual bool OnCheckForProblems()
		{
			return false;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0000DCF8 File Offset: 0x0000BEF8
		[EngineCallback(null, false)]
		protected internal virtual void OnSaveAsPrefab()
		{
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0000DCFA File Offset: 0x0000BEFA
		[EngineCallback(null, false)]
		protected internal virtual void OnTerrainReload(int step)
		{
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0000DCFC File Offset: 0x0000BEFC
		[EngineCallback(null, false)]
		protected internal void OnPhysicsCollisionAux(ref PhysicsContact contact, UIntPtr entity0, UIntPtr entity1)
		{
			this.OnPhysicsCollision(ref contact, new WeakGameEntity(entity0), new WeakGameEntity(entity1));
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0000DD11 File Offset: 0x0000BF11
		protected internal virtual void OnPhysicsCollision(ref PhysicsContact contact, WeakGameEntity entity0, WeakGameEntity entity1)
		{
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0000DD13 File Offset: 0x0000BF13
		[EngineCallback(null, false)]
		protected internal virtual void OnEditModeVisibilityChanged(bool currentVisibility)
		{
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0000DD15 File Offset: 0x0000BF15
		[EngineCallback(null, false)]
		protected internal virtual void OnBoundingBoxValidate()
		{
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0000DD17 File Offset: 0x0000BF17
		[EngineCallback(null, false)]
		protected internal virtual void OnDynamicNavmeshVertexUpdate()
		{
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		private static void CacheEditableFieldsForAllScriptComponents()
		{
			foreach (KeyValuePair<string, Type> keyValuePair in Managed.ModuleTypes)
			{
				Type value = keyValuePair.Value;
				string text = keyValuePair.Key;
				object[] customAttributesSafe = value.GetCustomAttributesSafe(typeof(ScriptComponentParams), true);
				if (customAttributesSafe.Length != 0)
				{
					ScriptComponentParams scriptComponentParams = (ScriptComponentParams)customAttributesSafe[0];
					if (scriptComponentParams.NameOverride.Length > 0)
					{
						text = scriptComponentParams.NameOverride;
					}
				}
				ScriptComponentBehavior.CachedFields.Add(text, ScriptComponentBehavior.CollectEditableFields(value));
			}
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		private static string[] CollectEditableFields(Type type)
		{
			List<string> list = new List<string>();
			List<FieldInfo> list2 = new List<FieldInfo>();
			while (type != null)
			{
				list2.AddRange(type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
				type = type.BaseType;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				FieldInfo fieldInfo = list2[i];
				string text = list2[i].Name;
				object[] customAttributesSafe = fieldInfo.GetCustomAttributesSafe(typeof(EditableScriptComponentVariable), true);
				bool flag = false;
				if (customAttributesSafe.Length != 0)
				{
					EditableScriptComponentVariable editableScriptComponentVariable = (EditableScriptComponentVariable)customAttributesSafe[0];
					bool isStatic = fieldInfo.IsStatic;
					bool isInitOnly = fieldInfo.IsInitOnly;
					if (editableScriptComponentVariable.OverrideFieldName.Length > 0)
					{
						text = editableScriptComponentVariable.OverrideFieldName;
					}
					flag = editableScriptComponentVariable.Visible;
				}
				else if (!fieldInfo.IsPrivate && !fieldInfo.IsFamily)
				{
					flag = true;
				}
				if (fieldInfo.IsStatic)
				{
					flag = false;
				}
				if (flag)
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
		[EngineCallback(null, false)]
		internal static string[] GetEditableFields(string className)
		{
			string[] array;
			ScriptComponentBehavior.CachedFields.TryGetValue(className, out array);
			return array;
		}

		// Token: 0x040001B8 RID: 440
		private static List<ScriptComponentBehavior> _prefabScriptComponents = new List<ScriptComponentBehavior>();

		// Token: 0x040001B9 RID: 441
		private static List<ScriptComponentBehavior> _undoStackScriptComponents = new List<ScriptComponentBehavior>();

		// Token: 0x040001BA RID: 442
		private WeakGameEntity _gameEntity;

		// Token: 0x040001BB RID: 443
		private WeakNativeObjectReference _scriptComponent;

		// Token: 0x040001BC RID: 444
		private ScriptComponentBehavior.TickRequirement _lastTickRequirement;

		// Token: 0x040001BD RID: 445
		private static readonly Dictionary<string, string[]> CachedFields;

		// Token: 0x040001BF RID: 447
		private WeakNativeObjectReference _scene;

		// Token: 0x020000D2 RID: 210
		[Flags]
		public enum TickRequirement : uint
		{
			// Token: 0x04000440 RID: 1088
			None = 0U,
			// Token: 0x04000441 RID: 1089
			TickOccasionally = 1U,
			// Token: 0x04000442 RID: 1090
			Tick = 2U,
			// Token: 0x04000443 RID: 1091
			TickParallel = 4U,
			// Token: 0x04000444 RID: 1092
			TickParallel2 = 8U,
			// Token: 0x04000445 RID: 1093
			FixedTick = 16U,
			// Token: 0x04000446 RID: 1094
			FixedParallelTick = 32U,
			// Token: 0x04000447 RID: 1095
			TickParallel3 = 64U
		}
	}
}
