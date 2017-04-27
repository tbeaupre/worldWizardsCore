using UnityEngine;
using UnityEngine.EventSystems;

namespace worldWizards.core.controllers
{
    /// <summary>
    /// The ManipulateObjectController handles translation, rotation, and scaling events
    /// for World Wizards Objects.
    /// </summary>
    public class ManipulateObjectController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData) { }
        public void OnDrag(PointerEventData eventData) { }
        public void OnEndDrag(PointerEventData eventData) { }
    }
}