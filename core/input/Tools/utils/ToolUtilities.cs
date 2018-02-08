using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

// @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
namespace WorldWizards.core.input.Tools.utils
{
    /// <summary>
    /// Shared functions that multiple tools are using.
    /// </summary>
    public static class ToolUtilities
    {
        public static Vector3 RaycastGridOnly(Vector3 origin, Vector3 direction, Collider gridCollider,
            float rayDistance)
        {
            Vector3 resultingHitpoint = Vector3.zero;
            // first cast against grid
            Ray ray = new Ray(origin, direction);
            RaycastHit raycastHit;
            if (gridCollider.Raycast(ray, out raycastHit, rayDistance))
            {
                resultingHitpoint = raycastHit.point;
                resultingHitpoint.y = 0; // IGNORE Y
            }
            resultingHitpoint.y += CoordinateHelper.GetTileScale() * 0.0001f; // ensure placement in space above ontop of the grid
            return resultingHitpoint;
        }
        
        public static Vector3 RaycastCustom(Vector3 origin, Vector3 direction,
            WWType wwType, float rayDistance)
        {
            Vector3 resultingHitpoint = Vector3.zero;   
            var minDistance = float.MaxValue;
            var colliders = ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().GetAllColliders(wwType);
            Ray ray = new Ray(origin, direction);
            foreach (var c in colliders)
            {
                RaycastHit raycastHit;
                if (c.Raycast(ray, out raycastHit, rayDistance))
                {
                    var dist = Vector3.Distance(origin, raycastHit.point);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        resultingHitpoint = raycastHit.point;
                    }
                }
            }
            return resultingHitpoint;
        }

        public static Vector3 RaycastGridThenCustom(Vector3 origin, Vector3 direction, Collider gridCollider,
            WWType wwType, float rayDistance)
        {
            Vector3 gridHitpoint = RaycastGridOnly(origin, direction, gridCollider, rayDistance);
            Vector3 customHitpoint = RaycastCustom(origin, direction, wwType, rayDistance);
            return customHitpoint != Vector3.zero ? customHitpoint : gridHitpoint;
        }
        
    }
}