using System.Collections.Generic;
using worldWizards.core.entity.world;

namespace worldWizards.core.entity.user
{
    /// <summary>
    /// A User Profile contains a user's general global settings, as well as
    /// their different projects (worlds).
    /// </summary>
    public class UserProfile
    {
        private UserSettings userSettings;
        private List<World> worlds;
    }
}
