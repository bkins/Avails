using System;

namespace Avails.D_Flat.Exceptions
{
    public class EntityRelationAlreadyExistsException : Exception
    {
        public EntityRelationAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
