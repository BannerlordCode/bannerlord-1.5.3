using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D0 RID: 720
	public class AgentVisualHolder : IAgentVisual
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x0009D9C8 File Offset: 0x0009BBC8
		public AgentVisualHolder(MatrixFrame frame, Equipment equipment, string name, BodyProperties bodyProperties)
		{
			this.SetFrame(ref frame);
			this._equipment = equipment;
			this._characterObjectStringID = name;
			this._bodyProperties = bodyProperties;
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x0009D9EE File Offset: 0x0009BBEE
		public void SetAction(in ActionIndexCache actionName, float startProgress = 0f, bool forceFaceMorphRestart = true)
		{
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x0009D9F0 File Offset: 0x0009BBF0
		public GameEntity GetEntity()
		{
			return null;
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x0009D9F3 File Offset: 0x0009BBF3
		public MBAgentVisuals GetVisuals()
		{
			return null;
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x0009D9F6 File Offset: 0x0009BBF6
		public void SetFrame(ref MatrixFrame frame)
		{
			this._frame = frame;
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x0009DA04 File Offset: 0x0009BC04
		public MatrixFrame GetFrame()
		{
			return this._frame;
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0009DA0C File Offset: 0x0009BC0C
		public BodyProperties GetBodyProperties()
		{
			return this._bodyProperties;
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x0009DA14 File Offset: 0x0009BC14
		public void SetBodyProperties(BodyProperties bodyProperties)
		{
			this._bodyProperties = bodyProperties;
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0009DA1D File Offset: 0x0009BC1D
		public bool GetIsFemale()
		{
			return false;
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x0009DA20 File Offset: 0x0009BC20
		public string GetCharacterObjectID()
		{
			return this._characterObjectStringID;
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x0009DA28 File Offset: 0x0009BC28
		public void SetCharacterObjectID(string id)
		{
			this._characterObjectStringID = id;
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x0009DA31 File Offset: 0x0009BC31
		public Equipment GetEquipment()
		{
			return this._equipment;
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0009DA39 File Offset: 0x0009BC39
		public void RefreshWithNewEquipment(Equipment equipment)
		{
			this._equipment = equipment;
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0009DA42 File Offset: 0x0009BC42
		public void SetClothingColors(uint color1, uint color2)
		{
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0009DA44 File Offset: 0x0009BC44
		public void GetClothingColors(out uint color1, out uint color2)
		{
			color1 = uint.MaxValue;
			color2 = uint.MaxValue;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0009DA4C File Offset: 0x0009BC4C
		public AgentVisualsData GetCopyAgentVisualsData()
		{
			return null;
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x0009DA4F File Offset: 0x0009BC4F
		public void Refresh(bool needBatchedVersionForWeaponMeshes, AgentVisualsData data, bool forceUseFaceCache = false)
		{
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x0009DA51 File Offset: 0x0009BC51
		void IAgentVisual.SetAction(in ActionIndexCache actionName, float startProgress, bool forceFaceMorphRestart)
		{
			this.SetAction(in actionName, startProgress, forceFaceMorphRestart);
		}

		// Token: 0x04001000 RID: 4096
		private MatrixFrame _frame;

		// Token: 0x04001001 RID: 4097
		private Equipment _equipment;

		// Token: 0x04001002 RID: 4098
		private string _characterObjectStringID;

		// Token: 0x04001003 RID: 4099
		private BodyProperties _bodyProperties;
	}
}
