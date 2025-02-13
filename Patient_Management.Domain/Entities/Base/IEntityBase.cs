using System;

namespace Patient_Management.Domain.Entities.Base
{
    public interface IEntityBase
    {
        string Id { get; }
        DateTime CreatedAt { get; set; }
        void SetNewId();
    }
}
