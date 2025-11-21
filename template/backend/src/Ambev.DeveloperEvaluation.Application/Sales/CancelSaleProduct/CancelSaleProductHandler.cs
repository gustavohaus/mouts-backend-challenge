using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleProduct
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class CancelSaleProductHandler : IRequestHandler<CancelSaleProductCommand, CancelSaleProductResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CancelSaleProductHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<CancelSaleProductResult> Handle(CancelSaleProductCommand command, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

            if(sale == null)
            {

            }

            sale.CancelProduct(command.SaleProduct);

            var update = await _saleRepository.UpdateAsync(sale, cancellationToken);
            return _mapper.Map<CancelSaleProductResult>(update);
        }
    }
}
