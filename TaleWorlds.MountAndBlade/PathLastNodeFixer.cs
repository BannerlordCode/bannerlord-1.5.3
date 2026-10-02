using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000357 RID: 855
	public class PathLastNodeFixer : UsableMissionObjectComponent
	{
		// Token: 0x06003070 RID: 12400 RVA: 0x000BFD58 File Offset: 0x000BDF58
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			Path pathWithName = this._scene.GetPathWithName(this.PathHolder.PathEntity);
			this.Update(pathWithName);
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000BFD8A File Offset: 0x000BDF8A
		protected internal override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			this._scene = scene;
			this.Update();
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000BFDA0 File Offset: 0x000BDFA0
		public void Update()
		{
			Path pathWithName = this._scene.GetPathWithName(this.PathHolder.PathEntity);
			this.Update(pathWithName);
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000BFDCB File Offset: 0x000BDFCB
		private void Update(Path path)
		{
			path != null;
		}

		// Token: 0x040013F0 RID: 5104
		public IPathHolder PathHolder;

		// Token: 0x040013F1 RID: 5105
		private Scene _scene;
	}
}
