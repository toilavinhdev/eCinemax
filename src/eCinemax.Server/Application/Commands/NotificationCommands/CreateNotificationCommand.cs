using eCinemax.Server.Aggregates.NotificationAggregate;
using eCinemax.Server.Persistence;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.NotificationCommands;

public class CreateNotificationCommand : IAPIRequest<Notification>
{
    public string Title { get; set; } = default!;
    
    public string Content { get; set; } = default!;

    public string PhotoUrl { get; set; } = default!;
    
    internal class CreateNotificationCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CreateNotificationCommand, Notification>
    {
        private readonly IMongoCollection<Notification> _notificationCollection = mongoService.Collection<Notification>();
        
        public async Task<APIResponse<Notification>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var document = new Notification
            {
                Title = request.Title,
                Content = request.Content,
                PhotoUrl = request.PhotoUrl
            };
            document.MarkCreated();

            await _notificationCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

            return APIResponse<Notification>.IsSuccess(document);
        }
    }
}

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.PhotoUrl).NotEmpty();
    }
}