using System;
using UnityEngine;
using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.Desktop
{
    public class DesktopController : MonoBehaviour
    {
        private DesktopListener left;
        private DesktopListener right;

        private void Awake()
        {
            left = gameObject.AddComponent<DesktopListener>();
            left.Init(KeyCode.E, KeyCode.Q, KeyCode.Alpha2, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D,
                KeyCode.LeftShift, gameObject.AddComponent<StandardTool>());
            
            right = gameObject.AddComponent<DesktopListener>();
            right.Init(KeyCode.U, KeyCode.O, KeyCode.Alpha8, KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L,
                KeyCode.Slash, gameObject.AddComponent<CreateObjectTool>());
        }
        
        public void ChangeTool(Type type)
        {
            Destroy(GetComponent<Tool>());
            right.ChangeTool(gameObject.AddComponent(type) as Tool);
        }
    }
}