using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public void UpdateInfo(string saleNumber , SaleStatus status = SaleStatus.Created)
        {
            SaleNumber = saleNumber;
            Status = status;

        }
        public void SyncSaleProducts(IEnumerable<(Guid ProductId, int Quantity)> entries)
        {
            var removeProducts = _saleProducts
                .Where(x => !entries.Any(p => p.ProductId == x.ProductId))
                .Select(x => x.ProductId)
                .ToList();

            var update = entries
                .Where(x => _saleProducts.Any(p => p.ProductId == x.ProductId && p.Quantity != x.Quantity))
                .ToList();

            foreach (var saleProduct in update)
            {
                this.UpdateSaleProduct(saleProduct.ProductId, saleProduct.Quantity);
            }

            foreach (var saleProduct in removeProducts)
            {
                this.RemoveProduct(saleProduct);
            }

            this.CalculateTotalAmount();
            this.Update();

        }
        public void AddProducts(List<SaleProduct> saleProducts)
        {
            foreach (var product in saleProducts)
            {
                this.AddProduct(product);
            }
        }
        public void AddProduct(SaleProduct saleProduct)
        {
            _saleProducts.Add(saleProduct);
            this.CalculateTotalAmount();
            this.Update();
        }

        private void UpdateSaleProduct(Guid ProductId, int quantity)
        {
            var productToUpdate = _saleProducts.FirstOrDefault(p => p.ProductId == ProductId);

            if (productToUpdate == null)
                throw new InvalidOperationException($"Product with ID {ProductId} not found in the sale.");

            productToUpdate.Update(quantity);
        }

        public void RemoveProduct(Guid productId)
        {
            var productToRemove = _saleProducts.FirstOrDefault(p => p.ProductId == productId);

            if (productToRemove == null)
                throw new InvalidOperationException($"Product with ID {productId} not found in the sale.");

            _saleProducts.Remove(productToRemove);


        }

        public void CancelProduct(Guid productId)
        {
            var productToCancel = _saleProducts.FirstOrDefault(p => p.ProductId == productId);

            if (productToCancel == null)
                throw new InvalidOperationException($"Product with ID {productId} not found in the sale.");

            productToCancel.Cancel();
            this.CalculateTotalAmount();
            this.Update();
        }

        public void Cancel()
        {
            foreach (var saleProduct in _saleProducts)
            {
                saleProduct.Cancel();
            }

            if (!_saleProducts.Any(x => !x.IsCancelled))
                Status = SaleStatus.Cancelled;

            this.CalculateTotalAmount();
            this.Update();
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


