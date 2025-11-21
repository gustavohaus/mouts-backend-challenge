using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleHandler> _logger;

        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<UpdateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            
            if(sale == null)
            {
                _logger.LogWarning("Sale {Id} not found.", command.Id);
                throw new KeyNotFoundException($"User with ID {command.Id} not found");
            }            

            var adds = command.SaleProducts.Where(x => x.Id == null).ToList();
            AddProducts(sale, adds);

            var removeProducts = command.SaleProducts;
            RemoveProducts(sale, removeProducts);

            var update = command.SaleProducts.Where(x => sale.SaleProducts.Any(p => p.ProductId == x.ProductId 
            && p.Quantity != x.Quantity)).ToList();

            var updateProducts = _mapper.Map<List<SaleProduct>>(update);
            sale.UpdateProducts(updateProducts);

            var updateSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

            return _mapper.Map<GetSaleResult>(updateSale);
        }

        private void AddProducts(Sale sale, List<UpdateSaleProductsCommand> products)
        {
            var addProducts = _mapper.Map<List<SaleProduct>>(products);
            sale.AddProducts(addProducts);
        }

        private void RemoveProducts(Sale sale, List<UpdateSaleProductsCommand> products)
        {
            var removeProducts = sale.SaleProducts
                .Where(x => !products.Any(p => p.ProductId == x.ProductId))
                .Select(x => x.ProductId)
                .ToList();

            foreach (var productId in removeProducts)
                sale.RemoveProduct(productId);
        }
    }
}
