using System;
using System.Runtime.InteropServices;

// Token: 0x02000009 RID: 9
[StructLayout(LayoutKind.Explicit)]
public struct C_BasePlayerWeapon
{
	// Token: 0x0400003A RID: 58
	[FieldOffset(5488)]
	public int m_iClip1;

	// Token: 0x0400003B RID: 59
	[FieldOffset(5492)]
	public int m_iClip2;

	// Token: 0x0400003C RID: 60
	[FieldOffset(5496)]
	public int m_pReserveAmmo;

	// Token: 0x0400003D RID: 61
	[FieldOffset(4160)]
	public C_AttributeContainer m_AttributeManager;
}
