using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
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

        protected Sale() { }

        public Sale(string saleNumber, Branch branch, User customer, SaleStatus status = SaleStatus.Created)
        {
            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            Branch = branch;
            Customer = customer;
            Status = status;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddProducts(List<SaleProduct> saleProducts)
        {
            foreach (var product in saleProducts)
            {
                AddProduct(product);
            }
        }
        public void UpdateProducts(List<SaleProduct> saleProducts)
        {
            foreach (var product in saleProducts)
            {
                Update(product);
            }
        }

        public void Update(SaleProduct saleProduct)
        {
            saleProduct.Update(saleProduct.Quantity);
            CalculateTotalAmount();
            Update();
        }
        public void AddProduct(SaleProduct saleProduct)
        {
            _saleProducts.Add(saleProduct);
            CalculateTotalAmount();
            Update();
        }

        public void RemoveProduct(Guid productId)
        {
            var productToRemove = _saleProducts.FirstOrDefault(p => p.ProductId == productId);

            if (productToRemove == null)
                throw new InvalidOperationException($"Product with ID {productId} not found in the sale.");

            _saleProducts.Remove(productToRemove);
            CalculateTotalAmount();
            Update();
        }

        public void CancelProduct(Guid productId)
        {
            var productToCancel = _saleProducts.FirstOrDefault(p => p.ProductId == productId);

            if (productToCancel == null)
                throw new InvalidOperationException($"Product with ID {productId} not found in the sale.");

            productToCancel.Cancel();
            CalculateTotalAmount();
            Update();
        }

        public void Cancel()
        {
            foreach (var saleProduct in _saleProducts)
            {
                saleProduct.Cancel();
            }

            Status = SaleStatus.Cancelled;
            CalculateTotalAmount();
            Update();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = _saleProducts.Where(x => !x.IsCancelled).Sum(p => p.TotalAmount);
        }

        private void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}


