namespace Checkout.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
