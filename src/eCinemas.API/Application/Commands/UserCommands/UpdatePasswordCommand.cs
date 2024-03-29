using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Constants;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Extensions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.UserCommands;

public class UpdatePasswordCommand : IAPIRequest
{
    public string Email { get; set; } = default!;

    public string CurrentPassword { get; set; } = default!;

    public string NewPassword { get; set; } = default!;
}

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được bỏ trống")
            .Matches(RegexConstant.EmailRegex).WithMessage("Email không đúng định dạng");;
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Mật khẩu hiện tại không được bỏ trống");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Mật khẩu mới không được bỏ trống");
    }
}

public class UpdatePasswordCommandHandler(IMongoService mongoService) : IAPIRequestHandler<UpdatePasswordCommand>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Email, request.Email);
        var user = await _userCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<User>.ThrowIfNotFound(user, "Người dùng không tồn tại");

        if (!user.PasswordHash.Equals(request.CurrentPassword.ToSha256()))
        {
            throw new BadRequestException("Mật khẩu hiện tại không chính xác");
        }

        var update = Builders<User>.Update.Set(x => x.PasswordHash, request.NewPassword.ToSha256());
        var result = await _userCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (!result.IsAcknowledged) throw new BadRequestException("Vui lòng thử lại");
        
        return APIResponse.IsSuccess("Cập nhật thành công");
    }
}