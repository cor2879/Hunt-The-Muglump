namespace OldSchoolGames.HuntTheMuglump.Scripts.Exceptions
{
    using System;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;

    public class PrefabNotSetException : Exception
    {
        public PrefabNotSetException() { }

        public PrefabNotSetException(string message) : base(message) { }

        public PrefabNotSetException(string message, Exception innerException) { }
    }
}
