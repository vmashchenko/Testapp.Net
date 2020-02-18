using System;

namespace TestApp.Domain
{
    public interface ICurrentUserProvider
    {
        long GetUserId();
    }
}
