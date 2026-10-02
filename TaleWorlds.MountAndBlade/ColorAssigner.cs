using System;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000335 RID: 821
	[ScriptComponentParams("ship_visual_only", "ShipColorAssigner")]
	public class ColorAssigner : ScriptComponentBehavior
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002E9C RID: 11932 RVA: 0x000B453D File Offset: 0x000B273D
		public Color ShipColor
		{
			get
			{
				return this._color;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002E9D RID: 11933 RVA: 0x000B4545 File Offset: 0x000B2745
		public Color RamDebrisColor
		{
			get
			{
				return this._ramDebrisColor;
			}
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000B4576 File Offset: 0x000B2776
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetColor(base.GameEntity);
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000B458A File Offset: 0x000B278A
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.SetColor(base.GameEntity);
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000B459E File Offset: 0x000B279E
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "Set Colors" || variableName == "Factor Color")
			{
				this.SetColor(base.GameEntity);
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000B45C6 File Offset: 0x000B27C6
		public void SetColor(WeakGameEntity entity)
		{
			entity.SetColorToAllMeshesWithTagRecursive(this._color.ToUnsignedInteger(), "auto_factor_color");
		}

		// Token: 0x04001274 RID: 4724
		[EditableScriptComponentVariable(true, "Factor Color")]
		private Color _color = Color.White;

		// Token: 0x04001275 RID: 4725
		[EditableScriptComponentVariable(true, "Ram Debris Color")]
		private Color _ramDebrisColor = Color.White;

		// Token: 0x04001276 RID: 4726
		[EditableScriptComponentVariable(true, "Set Colors")]
		private SimpleButton _refreshButton = new SimpleButton();
	}
}
