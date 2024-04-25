using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Shared.Constants;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.UserCommands;

public class UpdateProfileCommand : IAPIRequest<GetMeResponse>
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public string? AvatarUrl { get; set; }
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tên không được bỏ trống");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được bỏ trống")
            .Matches(RegexConstant.EmailRegex).WithMessage("Email không đúng định dạng");;
    }
}

public class UpdateProfileCommandHandler(IMongoService mongoService) : IAPIRequestHandler<UpdateProfileCommand, GetMeResponse>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse<GetMeResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, mongoService.UserClaims().Id);
        
        var user = await _userCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<User>.ThrowIfNotFound(user, "Người dùng không tồn tại");

        var existedEmail = await _userCollection.Find(x => x.Email == request.Email).AnyAsync(cancellationToken);
        if (existedEmail && request.Email != user.Email) throw new BadRequestException("Email đã tồn tại");

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.AvatarUrl = request.AvatarUrl;
        user.MarkModified();
        
        var update = Builders<User>.Update
            .Set(x => x.FullName, user.FullName)
            .Set(x => x.Email, user.Email)
            .Set(x => x.AvatarUrl, user.AvatarUrl)
            .Set(x => x.ModifiedAt, user.ModifiedAt);

        var result = await _userCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (!result.IsAcknowledged) throw new BadRequestException("Vui lòng thử lại");
        
        return APIResponse<GetMeResponse>.IsSuccess(
            new GetMeResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            },
            "Cập nhật thành công");
    }
}