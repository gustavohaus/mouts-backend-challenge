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
                var updateProduct = _saleProducts.FirstOrDefault(p => p.ProductId == product.Id);

                if (updateProduct != null)
                    updateProduct.Update(product.Quantity);
            }
        }
        public void RemoveProducts(List<Guid> productIds)
        {
            foreach (var productId in productIds)
            {
                RemoveProduct(productId);
            }
        }

        public void AddProduct(SaleProduct saleProduct)
        {
            _saleProducts.Add(saleProduct);
            this.CalculateTotalAmount();
            this.Update();
        }
        public void CancelProduct(Guid productId)
        {
            var updateProduct = _saleProducts.FirstOrDefault(p => p.ProductId == productId);

            if (updateProduct != null)
                updateProduct.Cancel();

            this.Update();
        }
        public void Cancel()
        {
            foreach (var saleProduct in _saleProducts)
            {
                CancelProduct(saleProduct.ProductId);
            }

            this.Status = SaleStatus.Cancelled;
            this.Update();

        }
        public void RemoveProduct(Guid productId)
        {
            var productToRemove = _saleProducts.FirstOrDefault(p => p.ProductId == productId);

            if (productToRemove != null)
                _saleProducts.Remove(productToRemove);

            this.Update();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = _saleProducts.Where(x => x.IsCancelled == false).Sum(p => p.TotalAmount);
        }

        private void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            foreach (var saleProduct in _saleProducts)
                RemoveProduct(saleProduct.ProductId);

            this.Update();
            this.CalculateTotalAmount();

            Status = SaleStatus.Cancelled;
        }

    }
}


