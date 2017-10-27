using System.Collections.Generic;
using WorldWizards.core.entity.world;

namespace WorldWizards.core.entity.common.user
{
    /// <summary>
    ///     A User Profile contains a user's general global settings, as well as
    ///     their different projects (worlds).
    /// </summary>
    public class UserProfile
    {
        private UserSettings userSettings;
        private List<World> worlds;
    }
}