//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Destroys this object when it is detached from the hand
//
//=============================================================================

using UnityEngine;

namespace WorldWizards.SteamVR.InteractionSystem.Core.Scripts
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class DestroyOnDetachedFromHand : MonoBehaviour
	{
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
			Destroy( gameObject );
		}
	}
}
