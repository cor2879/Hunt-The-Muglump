namespace OldSchoolGames.HuntTheMuglump.Scripts.Exceptions
{
    using System;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;

    public class UIException : Exception
    {
        public UIException() { }

        public UIException(string message) : base(message) { }

        public UIException(string message, Exception innerException) { }
    }
}
