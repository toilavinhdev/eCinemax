using System.Security.Claims;
using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Constants;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;
using Todo.NET.Extensions;
using Todo.NET.Security;

namespace eCinemax.Server.Application.Commands.UserCommands;

public class SignInCommand : IAPIRequest<SignInResponse>
{
    public string Email { get; set; } = default!;
    
    public string Password { get; set; } = default!;
}

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được bỏ trống")
            .Matches(RegexConstant.EmailRegex).WithMessage("Email không đúng định dạng");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống");
    }
}

public class SignInCommandHandler(IMongoService mongoService, AppSettings appSettings) : IAPIRequestHandler<SignInCommand, SignInResponse>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userCollection
            .Find(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null) throw new BadRequestException("Không tìm thấy email");
        
        if (!user.PasswordHash.Equals(request.Password.ToSha256()))
            throw new BadRequestException("Mật khẩu không chính xác");

        var claims = new List<Claim>
        {
            new("id", user.Id),
            new("fullName", user.FullName),
            new("email", user.Email)
        };

        var accessToken = JwtBearer.GenerateAccessToken(
            appSettings.JwtConfig.TokenSingingKey, 
            claims,
            DateTime.Now.AddMinutes(appSettings.JwtConfig.AccessTokenDurationInMinutes));

        return APIResponse<SignInResponse>.IsSuccess(
            new SignInResponse { AccessToken = accessToken },
            "Đăng nhập thành công");
    }
}