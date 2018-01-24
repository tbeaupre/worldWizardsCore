//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Adding this component to an object will allow the player to 
//			initiate teleporting while that object is attached to their hand
//
//=============================================================================

using UnityEngine;

namespace WorldWizards.SteamVR.InteractionSystem.Teleport.Scripts
{
	//-------------------------------------------------------------------------
	public class AllowTeleportWhileAttachedToHand : MonoBehaviour
	{
		public bool teleportAllowed = true;
		public bool overrideHoverLock = true;
	}
}
