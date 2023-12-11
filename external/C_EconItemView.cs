using System;
using System.Runtime.InteropServices;

// Token: 0x0200000B RID: 11
[StructLayout(LayoutKind.Explicit)]
public struct C_EconItemView
{
	// Token: 0x0400003F RID: 63
	[FieldOffset(456)]
	public long m_iItemID;

	// Token: 0x04000040 RID: 64
	[FieldOffset(442)]
	public ushort m_iItemDefinitionIndex;
}
