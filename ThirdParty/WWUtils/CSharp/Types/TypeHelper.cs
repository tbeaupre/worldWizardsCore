using Assembly = System.Reflection.Assembly;
using Type = System.Type;

namespace WorldWizards.WWUtils.CSharp.Types
{
    public class TypeHelper {
    
        public static void ForAllTypes<T>(System.Action<System.Type> action) {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (typeof(T).IsAssignableFrom(t)) { 
                        action(t);
                    }
                }
            }
        }
    }
}
