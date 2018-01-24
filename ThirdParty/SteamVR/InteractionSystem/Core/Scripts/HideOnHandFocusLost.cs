//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Sets this GameObject as inactive when it loses focus from the hand
//
//=============================================================================

using UnityEngine;

namespace WorldWizards.SteamVR.InteractionSystem.Core.Scripts
{
	//-------------------------------------------------------------------------
	public class HideOnHandFocusLost : MonoBehaviour
	{
		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
			gameObject.SetActive( false );
		}
	}
}
