using System;

namespace WorkSoftCase.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){}
    }

    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message){}
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message){}
    }
}
