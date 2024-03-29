using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.UserCommands;

public class UpdateProfileCommand : IAPIRequest
{
    public string FullName { get; set; } = default!;
    
    public string? AvatarUrl { get; set; }
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tên không được bỏ trống");
    }
}

public class UpdateProfileCommandHandler(IMongoService mongoService) : IAPIRequestHandler<UpdateProfileCommand>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, mongoService.UserClaims().Id);
        
        var user = await _userCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<User>.ThrowIfNotFound(user, "Người dùng không tồn tại");

        var update = Builders<User>.Update
            .Set(x => x.FullName, request.FullName)
            .Set(x => x.AvatarUrl, request.AvatarUrl);

        var result = await _userCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (!result.IsAcknowledged) throw new BadRequestException("Vui lòng thử lại");
        return APIResponse.IsSuccess("Cập nhật thành công");
    }
}