using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistance;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    internal class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVM>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrdersListQueryHandler(IOrderRepository orderRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }


        async Task<List<OrdersVM>> IRequestHandler<GetOrdersListQuery, List<OrdersVM>>.Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList= await _orderRepository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrdersVM>>(orderList);
        }
    }
}
