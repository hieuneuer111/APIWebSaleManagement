using System;

namespace WebAPISalesManagement.ModelResponses
{
    public class ToppingResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
