using WorldWizards.core.entity.coordinate;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.gameObject
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// The WWTransform is composed of the coordinate and the rotation.
    /// </summary>
    public class WWTransform
    {
        public Coordinate coordinate { get; set; }
        public int rotation { get; set; }


        public WWTransform(WWTransformJSONBlob b)
        {
            coordinate = new Coordinate(b.coordinateJSONBlob);
            rotation = b.rotation;
        }
        
        public WWTransform(Coordinate coordinate, int rotation)
        {
            this.coordinate = coordinate;
            this.rotation = rotation;
        }
        
        public WWTransform(Coordinate coordinate) : this(coordinate, 0){}
    }
}