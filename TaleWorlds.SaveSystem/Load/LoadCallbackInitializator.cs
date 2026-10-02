using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000039 RID: 57
	internal class LoadCallbackInitializator
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000B89B File Offset: 0x00009A9B
		public LoadCallbackInitializator(LoadData loadData, ObjectHeaderLoadData[] objectHeaderLoadDatas, int objectCount)
		{
			this._loadData = loadData;
			this._objectHeaderLoadDatas = objectHeaderLoadDatas;
			this._objectCount = objectCount;
			this._objectLoadDatas = new Dictionary<int, ObjectLoadData>();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000B8C4 File Offset: 0x00009AC4
		public void InitializeObjects()
		{
			using (new PerformanceTestBlock("LoadContext::Callbacks"))
			{
				for (int i = 0; i < this._objectCount; i++)
				{
					ObjectHeaderLoadData objectHeaderLoadData = this._objectHeaderLoadDatas[i];
					if (objectHeaderLoadData.Target != null)
					{
						TypeDefinition typeDefinition = objectHeaderLoadData.TypeDefinition;
						IEnumerable<MethodInfo> enumerable = ((typeDefinition != null) ? typeDefinition.InitializationCallbacks : null);
						if (enumerable != null)
						{
							foreach (MethodInfo methodInfo in enumerable)
							{
								ParameterInfo[] parameters = methodInfo.GetParameters();
								if (parameters.Length > 1 && parameters[1].ParameterType == typeof(ObjectLoadData))
								{
									ObjectLoadData objectLoadData = this.GetObjectLoadData(objectHeaderLoadData, i);
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[]
									{
										this._loadData.MetaData,
										objectLoadData
									});
								}
								else if (parameters.Length == 1)
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[] { this._loadData.MetaData });
								}
								else
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, null);
								}
							}
						}
					}
				}
			}
			GC.Collect();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000BA2C File Offset: 0x00009C2C
		public void AfterInitializeObjects()
		{
			using (new PerformanceTestBlock("LoadContext::AfterCallbacks"))
			{
				for (int i = 0; i < this._objectCount; i++)
				{
					ObjectHeaderLoadData objectHeaderLoadData = this._objectHeaderLoadDatas[i];
					if (objectHeaderLoadData.Target != null)
					{
						TypeDefinition typeDefinition = objectHeaderLoadData.TypeDefinition;
						IEnumerable<MethodInfo> enumerable = ((typeDefinition != null) ? typeDefinition.LateInitializationCallbacks : null);
						if (enumerable != null)
						{
							foreach (MethodInfo methodInfo in enumerable)
							{
								ParameterInfo[] parameters = methodInfo.GetParameters();
								if (parameters.Length > 1 && parameters[1].ParameterType == typeof(ObjectLoadData))
								{
									ObjectLoadData objectLoadData = this.GetObjectLoadData(objectHeaderLoadData, i);
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[]
									{
										this._loadData.MetaData,
										objectLoadData
									});
								}
								else if (parameters.Length == 1)
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[] { this._loadData.MetaData });
								}
								else
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, null);
								}
							}
						}
					}
				}
			}
			this._objectLoadDatas.Clear();
			GC.Collect();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		private ObjectLoadData GetObjectLoadData(ObjectHeaderLoadData objectHeaderLoadData, int i)
		{
			ObjectLoadData objectLoadData;
			if (!this._objectLoadDatas.TryGetValue(i, out objectLoadData))
			{
				objectLoadData = LoadContext.CreateLoadData(this._loadData, i, objectHeaderLoadData);
				this._objectLoadDatas[i] = objectLoadData;
			}
			return objectLoadData;
		}

		// Token: 0x040000AA RID: 170
		private ObjectHeaderLoadData[] _objectHeaderLoadDatas;

		// Token: 0x040000AB RID: 171
		private int _objectCount;

		// Token: 0x040000AC RID: 172
		private LoadData _loadData;

		// Token: 0x040000AD RID: 173
		private Dictionary<int, ObjectLoadData> _objectLoadDatas;
	}
}
