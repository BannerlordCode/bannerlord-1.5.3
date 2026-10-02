using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x02000094 RID: 148
	public class TestContext
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x00012CE4 File Offset: 0x00010EE4
		public void RunTestAux(string commandLine)
		{
			TestCommonBase.BaseInstance.IsTestEnabled = true;
			Debug.SetTestModeEnabled(true);
			string[] array = commandLine.Split(new char[] { ' ' });
			if (array.Length < 2)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == "/runTest")
				{
					num = i + 1;
				}
			}
			if (num >= array.Length || num == -1)
			{
				return;
			}
			string text = array[num];
			if (text == "OpenSceneOnStartup")
			{
				TestCommonBase.BaseInstance.SceneNameToOpenOnStartup = array[2];
			}
			for (int j = 3; j < array.Length; j++)
			{
				int num2;
				int.TryParse(array[j], out num2);
				TestCommonBase.BaseInstance.TestRandomSeed = num2;
			}
			Debug.Print("commandLine: " + commandLine, 0, Debug.DebugColor.White, 17592186044416UL);
			Debug.Print("p: " + array.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
			Debug.Print("Looking for test: " + text, 0, Debug.DebugColor.Yellow, 17592186044416UL);
			ConstructorInfo asyncRunnerConstructor = this.GetAsyncRunnerConstructor(text);
			object obj = null;
			if (asyncRunnerConstructor != null)
			{
				obj = asyncRunnerConstructor.Invoke(new object[0]);
			}
			this._asyncRunner = obj as AsyncRunner;
			this._awaitableAsyncRunner = obj as AwaitableAsyncRunner;
			if (this._asyncRunner != null)
			{
				this._asyncThread = new Thread(delegate
				{
					this._asyncRunner.Run();
				});
				this._asyncThread.Name = "ManagedAsyncThread";
				this._asyncThread.Start();
			}
			if (this._awaitableAsyncRunner != null)
			{
				this._asyncTask = this._awaitableAsyncRunner.RunAsync();
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00012E80 File Offset: 0x00011080
		private ConstructorInfo GetAsyncRunnerConstructor(string asyncRunner)
		{
			foreach (Assembly assembly in this.GetAsyncRunnerAssemblies())
			{
				Type[] array;
				try
				{
					array = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					array = ex.Types.Where<Type>((Type t) => t != null).ToArray<Type>();
				}
				foreach (Type type in array)
				{
					if (type.Name == asyncRunner && (typeof(AsyncRunner).IsAssignableFrom(type) || typeof(AwaitableAsyncRunner).IsAssignableFrom(type)))
					{
						ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new Type[0], null);
						if (constructor != null)
						{
							return constructor;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00012F70 File Offset: 0x00011170
		private Assembly[] GetAsyncRunnerAssemblies()
		{
			List<Assembly> list = new List<Assembly>();
			Assembly assembly = typeof(AsyncRunner).Assembly;
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				AssemblyName[] referencedAssembliesSafe = assembly2.GetReferencedAssembliesSafe();
				for (int j = 0; j < referencedAssembliesSafe.Length; j++)
				{
					if (referencedAssembliesSafe[j].ToString() == assembly.GetName().ToString())
					{
						list.Add(assembly2);
						break;
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00012FFC File Offset: 0x000111FC
		public void OnApplicationTick(float dt)
		{
			if (this._asyncTask != null && this._asyncTask.Status == TaskStatus.Faulted)
			{
				string text = "ERROR: Mono exception occurred at async Test Run\n";
				Exception innerException = this._asyncTask.Exception.InnerException;
				if (this._asyncTask.Exception.InnerException != null)
				{
					text += this._asyncTask.Exception.InnerException.Message;
					text += "\n";
					text += this._asyncTask.Exception.InnerException.StackTrace;
				}
				this._asyncTask = null;
				Debug.Print(text, 5, Debug.DebugColor.White, 17592186044416UL);
				if (innerException != null)
				{
					ExceptionDispatchInfo.Capture(innerException).Throw();
					return;
				}
				Debug.FailedAssert(text, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\TestContext.cs", "OnApplicationTick", 195);
				Debug.DoDelayedexit(5);
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000130D5 File Offset: 0x000112D5
		public void TickTest(float dt)
		{
			if (this._asyncThread != null && this._asyncThread.IsAlive && this._asyncRunner != null)
			{
				this._asyncRunner.SyncTick();
			}
			if (this._awaitableAsyncRunner != null)
			{
				this._awaitableAsyncRunner.OnTick(dt);
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00013113 File Offset: 0x00011313
		public void FinalizeContext()
		{
			if (this._asyncThread != null)
			{
				this._asyncThread.Join();
			}
			this._asyncThread = null;
			this._asyncRunner = null;
			this._awaitableAsyncRunner = null;
			this._asyncTask = null;
		}

		// Token: 0x040001A8 RID: 424
		private AsyncRunner _asyncRunner;

		// Token: 0x040001A9 RID: 425
		private AwaitableAsyncRunner _awaitableAsyncRunner;

		// Token: 0x040001AA RID: 426
		private Thread _asyncThread;

		// Token: 0x040001AB RID: 427
		private Task _asyncTask;
	}
}
