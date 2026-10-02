using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000059 RID: 89
	public static class ManagedExtensions
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x000070C0 File Offset: 0x000052C0
		private static void OnEditorVariableChanged(DotNetObject managedObject, uint classNameHash, uint fieldNameHash)
		{
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000070C4 File Offset: 0x000052C4
		[EngineCallback(null, false)]
		internal static void SetObjectFieldString(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, string value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000070FC File Offset: 0x000052FC
		[EngineCallback(null, false)]
		internal static void SetObjectFieldDouble(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, double value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00007148 File Offset: 0x00005348
		[EngineCallback(null, false)]
		internal static void SetObjectFieldFloat(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, float value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00007194 File Offset: 0x00005394
		[EngineCallback(null, false)]
		internal static void SetObjectFieldBool(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, bool value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000071E0 File Offset: 0x000053E0
		[EngineCallback(null, false)]
		internal static void SetObjectFieldInt(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, int value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000722C File Offset: 0x0000542C
		[EngineCallback(null, false)]
		internal static void SetObjectFieldVec3(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00007278 File Offset: 0x00005478
		[EngineCallback(null, false)]
		internal static void SetObjectFieldEntity(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new GameEntity(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x000072D4 File Offset: 0x000054D4
		[EngineCallback(null, false)]
		internal static void SetObjectFieldTexture(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new Texture(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00007330 File Offset: 0x00005530
		[EngineCallback(null, false)]
		internal static void SetObjectFieldMesh(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new MetaMesh(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0000738C File Offset: 0x0000558C
		[EngineCallback(null, false)]
		internal static void SetObjectFieldMaterial(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new Material(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x000073E8 File Offset: 0x000055E8
		[EngineCallback(null, false)]
		internal static void SetObjectFieldColor(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, new Color
			{
				Red = value.x,
				Green = value.y,
				Blue = value.z,
				Alpha = value.w
			});
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00007470 File Offset: 0x00005670
		[EngineCallback(null, false)]
		internal static void SetObjectFieldMatrixFrame(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, MatrixFrame value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000074BC File Offset: 0x000056BC
		[EngineCallback(null, false)]
		internal static void SetObjectFieldEnum(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, string value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			object obj = Enum.Parse(fieldOfClass.FieldType, value);
			fieldOfClass.SetValue(managedObject, obj);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00007510 File Offset: 0x00005710
		[EngineCallback(null, false)]
		internal static void GetObjectField(DotNetObject managedObject, uint classNameHash, ref ScriptComponentFieldHolder scriptComponentFieldHolder, uint fieldNameHash, RglScriptFieldType type)
		{
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			switch (type)
			{
			case RglScriptFieldType.RglSftString:
				scriptComponentFieldHolder.s = (string)fieldOfClass.GetValue(managedObject);
				return;
			case RglScriptFieldType.RglSftDouble:
				scriptComponentFieldHolder.d = (double)Convert.ChangeType(fieldOfClass.GetValue(managedObject), typeof(double));
				return;
			case RglScriptFieldType.RglSftFloat:
				scriptComponentFieldHolder.f = (float)Convert.ChangeType(fieldOfClass.GetValue(managedObject), typeof(float));
				return;
			case RglScriptFieldType.RglSftBool:
				scriptComponentFieldHolder.b = (((bool)fieldOfClass.GetValue(managedObject)) ? 1 : 0);
				return;
			case RglScriptFieldType.RglSftInt:
				scriptComponentFieldHolder.i = (int)Convert.ChangeType(fieldOfClass.GetValue(managedObject), typeof(int));
				return;
			case RglScriptFieldType.RglSftVec3:
			{
				Vec3 vec = (Vec3)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.v3 = new Vec3(vec, vec.w);
				return;
			}
			case RglScriptFieldType.RglSftEntity:
			{
				GameEntity gameEntity = (GameEntity)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.entityPointer = ((gameEntity != null) ? ((UIntPtr)Convert.ChangeType(gameEntity.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftTexture:
			{
				Texture texture = (Texture)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.texturePointer = ((texture != null) ? ((UIntPtr)Convert.ChangeType(texture.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftMesh:
			{
				MetaMesh metaMesh = (MetaMesh)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.meshPointer = ((metaMesh != null) ? ((UIntPtr)Convert.ChangeType(metaMesh.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftEnum:
				scriptComponentFieldHolder.enumValue = fieldOfClass.GetValue(managedObject).ToString();
				return;
			case RglScriptFieldType.RglSftMaterial:
			{
				Material material = (Material)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.materialPointer = ((material != null) ? ((UIntPtr)Convert.ChangeType(material.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftButton:
				break;
			case RglScriptFieldType.RglSftColor:
			{
				Color color = (Color)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.color.x = color.Red;
				scriptComponentFieldHolder.color.y = color.Green;
				scriptComponentFieldHolder.color.z = color.Blue;
				scriptComponentFieldHolder.color.w = color.Alpha;
				break;
			}
			case RglScriptFieldType.RglSftMatrixFrame:
			{
				MatrixFrame matrixFrame = (MatrixFrame)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.matrixFrame = matrixFrame;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000077BC File Offset: 0x000059BC
		[EngineCallback(null, false)]
		internal static void CopyObjectFieldsFrom(DotNetObject dst, DotNetObject src, string className, int callFieldChangeEventAsInteger)
		{
			foreach (KeyValuePair<uint, FieldInfo> keyValuePair in Managed.GetEditableFieldsOfClass(Managed.GetStringHashCode(className)))
			{
				FieldInfo value = keyValuePair.Value;
				value.SetValue(dst, value.GetValue(src));
			}
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00007824 File Offset: 0x00005A24
		[EngineCallback(null, false)]
		internal static DotNetObject CreateScriptComponentInstance(string className, UIntPtr entityPtr, ManagedScriptComponent managedScriptComponent)
		{
			ScriptComponentBehavior scriptComponentBehavior = null;
			Func<ScriptComponentBehavior> func = (Func<ScriptComponentBehavior>)Managed.GetConstructorDelegateOfClass(className);
			if (func != null)
			{
				scriptComponentBehavior = func();
				if (scriptComponentBehavior != null)
				{
					scriptComponentBehavior.Construct(entityPtr, managedScriptComponent);
				}
			}
			else
			{
				ConstructorInfo constructorOfClass = Managed.GetConstructorOfClass(className);
				if (constructorOfClass != null)
				{
					scriptComponentBehavior = constructorOfClass.Invoke(new object[0]) as ScriptComponentBehavior;
					if (scriptComponentBehavior != null)
					{
						scriptComponentBehavior.Construct(entityPtr, managedScriptComponent);
					}
				}
				else
				{
					MBDebug.ShowWarning("CreateScriptComponentInstance failed: " + className);
				}
			}
			return scriptComponentBehavior;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00007898 File Offset: 0x00005A98
		[EngineCallback(null, false)]
		internal static string GetScriptComponentClassNames()
		{
			List<Type> list = new List<Type>();
			foreach (Type type in Managed.ModuleTypes.Values)
			{
				if (!type.IsAbstract && typeof(ScriptComponentBehavior).IsAssignableFrom(type))
				{
					list.Add(type);
				}
			}
			string text = "";
			for (int i = 0; i < list.Count; i++)
			{
				Type type2 = list[i];
				string text2 = type2.Name;
				string text3 = "!";
				object[] customAttributesSafe = type2.GetCustomAttributesSafe(typeof(ScriptComponentParams), true);
				if (customAttributesSafe.Length != 0)
				{
					ScriptComponentParams scriptComponentParams = (ScriptComponentParams)customAttributesSafe[0];
					if (scriptComponentParams.NameOverride.Length > 0)
					{
						text2 = scriptComponentParams.NameOverride;
					}
					if (scriptComponentParams.Tag.Length > 0)
					{
						text3 = scriptComponentParams.Tag;
					}
				}
				text += text2;
				text += "-";
				text += type2.BaseType.Name;
				text += "-";
				text += text3;
				if (i + 1 != list.Count)
				{
					text += " ";
				}
			}
			return text;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x000079F4 File Offset: 0x00005BF4
		[EngineCallback(null, false)]
		internal static bool GetEditorVisibilityOfField(uint classNameHash, uint fieldNamehash)
		{
			object[] customAttributesSafe = Managed.GetFieldOfClass(classNameHash, fieldNamehash).GetCustomAttributesSafe(typeof(EditorVisibleScriptComponentVariable), true);
			return customAttributesSafe.Length == 0 || (customAttributesSafe[0] as EditorVisibleScriptComponentVariable).Visible;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00007A2C File Offset: 0x00005C2C
		[EngineCallback(null, false)]
		internal static RglScriptFieldType GetTypeOfField(uint classNameHash, uint fieldNameHash)
		{
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return RglScriptFieldType.RglSftInvalid;
			}
			Type fieldType = fieldOfClass.FieldType;
			if (fieldOfClass.FieldType == typeof(string))
			{
				return RglScriptFieldType.RglSftString;
			}
			if (fieldOfClass.FieldType == typeof(double))
			{
				return RglScriptFieldType.RglSftDouble;
			}
			if (fieldOfClass.FieldType.IsEnum)
			{
				return RglScriptFieldType.RglSftEnum;
			}
			if (fieldOfClass.FieldType == typeof(float))
			{
				return RglScriptFieldType.RglSftFloat;
			}
			if (fieldOfClass.FieldType == typeof(bool))
			{
				return RglScriptFieldType.RglSftBool;
			}
			if (fieldType == typeof(byte) || fieldType == typeof(sbyte) || fieldType == typeof(short) || fieldType == typeof(ushort) || fieldType == typeof(int) || fieldType == typeof(uint) || fieldType == typeof(long) || fieldType == typeof(ulong))
			{
				return RglScriptFieldType.RglSftInt;
			}
			if (fieldOfClass.FieldType == typeof(Vec3))
			{
				return RglScriptFieldType.RglSftVec3;
			}
			if (fieldOfClass.FieldType == typeof(GameEntity))
			{
				return RglScriptFieldType.RglSftEntity;
			}
			if (fieldOfClass.FieldType == typeof(Texture))
			{
				return RglScriptFieldType.RglSftTexture;
			}
			if (fieldOfClass.FieldType == typeof(MetaMesh))
			{
				return RglScriptFieldType.RglSftMesh;
			}
			if (fieldOfClass.FieldType == typeof(Material))
			{
				return RglScriptFieldType.RglSftMaterial;
			}
			if (fieldOfClass.FieldType == typeof(SimpleButton))
			{
				return RglScriptFieldType.RglSftButton;
			}
			if (fieldOfClass.FieldType == typeof(MatrixFrame))
			{
				return RglScriptFieldType.RglSftMatrixFrame;
			}
			if (fieldOfClass.FieldType == typeof(Color))
			{
				return RglScriptFieldType.RglSftColor;
			}
			return RglScriptFieldType.RglSftInvalid;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00007C26 File Offset: 0x00005E26
		[EngineCallback(null, false)]
		internal static void ForceGarbageCollect()
		{
			Utilities.FlushManagedObjectsMemory();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00007C30 File Offset: 0x00005E30
		[EngineCallback(null, false)]
		internal static void CollectCommandLineFunctions()
		{
			foreach (string text in CommandLineFunctionality.CollectCommandLineFunctions())
			{
				Utilities.AddCommandLineFunction(text);
			}
		}
	}
}
