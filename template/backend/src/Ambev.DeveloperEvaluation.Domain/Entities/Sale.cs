using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid BranchId { get; private set; }
        public SaleStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public decimal TotalAmount { get; private set; }
        public User Customer { get; private set; }
        public Branch Branch { get; private set; }
        public IReadOnlyCollection<SaleProduct> SaleProducts => _saleProducts.AsReadOnly();

        private readonly List<SaleProduct> _saleProducts = new();

        protected Sale()
        {
            
        }

        public Sale(
            string saleNumber,
            Branch branch,
            User customer,
            SaleStatus status = SaleStatus.Created)
        {
            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            Branch = branch;
            Customer = customer;
            Status = status;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        
    }
}


