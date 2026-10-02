using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens.Scripts
{
	// Token: 0x0200005D RID: 93
	public class MultiThreadedStressTestsScreen : ScreenBase
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0001AACC File Offset: 0x00018CCC
		protected override void OnActivate()
		{
			base.OnActivate();
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.All, "mono_renderscene");
			this._scene.Read("mp_ruins_2");
			this._sceneView = SceneView.CreateSceneView();
			this._sceneView.SetScene(this._scene);
			this._sceneView.SetSceneUsesShadows(true);
			Camera camera = Camera.CreateCamera();
			camera.Frame = this._scene.ReadAndCalculateInitialCamera();
			this._sceneView.SetCamera(camera);
			this._workerThreads = new List<Thread>();
			Thread thread = new Thread(delegate
			{
				MultiThreadedStressTestsScreen.MultiThreadedTestFunctions.MeshMerger(InputLayout.Input_layout_regular);
			});
			thread.Name = "StressTester|Mesh Merger Thread";
			this._workerThreads.Add(thread);
			Thread thread2 = new Thread(delegate
			{
				MultiThreadedStressTestsScreen.MultiThreadedTestFunctions.MeshMerger(InputLayout.Input_layout_normal_map);
			});
			thread2.Name = "StressTester|Mesh Merger Thread";
			this._workerThreads.Add(thread2);
			Thread thread3 = new Thread(delegate
			{
				MultiThreadedStressTestsScreen.MultiThreadedTestFunctions.MeshMerger(InputLayout.Input_layout_skinning);
			});
			thread3.Name = "StressTester|Mesh Merger Thread";
			this._workerThreads.Add(thread3);
			for (int i = 0; i < this._workerThreads.Count; i++)
			{
				this._workerThreads[i].Start();
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001AC38 File Offset: 0x00018E38
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._sceneView = null;
			this._scene = null;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001AC50 File Offset: 0x00018E50
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			bool flag = true;
			for (int i = 0; i < this._workerThreads.Count; i++)
			{
				if (this._workerThreads[i].IsAlive)
				{
					flag = false;
				}
			}
			if (flag)
			{
				ScreenManager.PopScreen();
			}
		}

		// Token: 0x040001E6 RID: 486
		private List<Thread> _workerThreads;

		// Token: 0x040001E7 RID: 487
		private Scene _scene;

		// Token: 0x040001E8 RID: 488
		private SceneView _sceneView;

		// Token: 0x020000D0 RID: 208
		public static class MultiThreadedTestFunctions
		{
			// Token: 0x0600064A RID: 1610 RVA: 0x0002B3D4 File Offset: 0x000295D4
			public static void MeshMerger(InputLayout layout)
			{
				Mesh mesh = Mesh.GetRandomMeshWithVdecl((int)layout);
				mesh = mesh.CreateCopy();
				UIntPtr uintPtr = mesh.LockEditDataWrite();
				Mesh mesh2 = Mesh.GetRandomMeshWithVdecl((int)layout);
				mesh2 = mesh2.CreateCopy();
				Mesh randomMeshWithVdecl = Mesh.GetRandomMeshWithVdecl((int)layout);
				Mesh randomMeshWithVdecl2 = Mesh.GetRandomMeshWithVdecl((int)layout);
				mesh.AddMesh(randomMeshWithVdecl, MatrixFrame.Identity);
				mesh2.AddMesh(randomMeshWithVdecl2, MatrixFrame.Identity);
				mesh.AddMesh(mesh2, MatrixFrame.Identity);
				int num = mesh.AddFaceCorner(new Vec3(0f, 0f, 1f, -1f), new Vec3(0f, 0f, 1f, -1f), new Vec2(0f, 1f), 268435455U, uintPtr);
				int num2 = mesh.AddFaceCorner(new Vec3(0f, 1f, 0f, -1f), new Vec3(0f, 0f, 1f, -1f), new Vec2(1f, 0f), 268435455U, uintPtr);
				int num3 = mesh.AddFaceCorner(new Vec3(0f, 1f, 1f, -1f), new Vec3(0f, 0f, 1f, -1f), new Vec2(1f, 1f), 268435455U, uintPtr);
				mesh.AddFace(num, num2, num3, uintPtr);
				mesh.UnlockEditDataWrite(uintPtr);
			}

			// Token: 0x0600064B RID: 1611 RVA: 0x0002B53C File Offset: 0x0002973C
			public static void SceneHandler(SceneView view)
			{
				int i = 0;
				while (i < 500)
				{
					view.SetSceneUsesShadows(false);
					view.SetRenderWithPostfx(false);
					Thread.Sleep(5000);
					view.SetSceneUsesShadows(true);
					view.SetRenderWithPostfx(true);
					Thread.Sleep(5000);
					view.SetSceneUsesContour(true);
					Thread.Sleep(5000);
				}
			}
		}
	}
}
