//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Makes this object ignore any hovering by the hands
//
//=============================================================================

using UnityEngine;

namespace WorldWizards.SteamVR.InteractionSystem.Core.Scripts
{
	//-------------------------------------------------------------------------
	public class IgnoreHovering : MonoBehaviour
	{
		[Tooltip( "If Hand is not null, only ignore the specified hand" )]
		public Hand onlyIgnoreHand = null;
	}
}
