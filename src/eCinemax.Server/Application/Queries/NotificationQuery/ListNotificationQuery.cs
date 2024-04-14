using eCinemax.Server.Aggregates.NotificationAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.NotificationQuery;

public class ListNotificationQuery : IAPIRequest<ListNotificationResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    internal class ListNotificationQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListNotificationQuery, ListNotificationResponse>
    {
        private readonly IMongoCollection<Notification> _notificationCollection = mongoService.Collection<Notification>();
        
        public async Task<APIResponse<ListNotificationResponse>> Handle(ListNotificationQuery request, CancellationToken cancellationToken)
        {
            var fluent = _notificationCollection.Find(Builders<Notification>.Filter.Empty);

            var totalRecord = await fluent.CountDocumentsAsync(cancellationToken);
            
            var documents = await fluent
                .SortByDescending(x => x.CreatedAt)
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Limit(request.PageSize)
                .Project(x => new NotificationViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    PhotoUrl = x.PhotoUrl,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return APIResponse<ListNotificationResponse>.IsSuccess(new ListNotificationResponse(
                documents,
                request.PageIndex,
                request.PageSize,
                (int)totalRecord));
        }
    }
}

public class ListNotificationQueryValidator : AbstractValidator<ListNotificationQuery>
{
    public ListNotificationQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
    }
}