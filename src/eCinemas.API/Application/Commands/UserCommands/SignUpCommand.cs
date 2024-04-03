using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Helpers;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Constants;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Extensions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.UserCommands;

public class SignUpCommand : IAPIRequest
{
    public string FullName { get; set; } = default!;
    
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tên không được bỏ trống");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được bỏ trống")
            .Matches(RegexConstant.EmailRegex).WithMessage("Email không đúng định dạng");;
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống")
            .MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự");
    }
}

public class SignUpCommandHandler(IMongoService mongoService, AppSettings appSettings) : IAPIRequestHandler<SignUpCommand>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var isExistedEmail = await _userCollection
            .Find(x => x.Email == request.Email)
            .AnyAsync(cancellationToken);

        if (isExistedEmail) throw new BadRequestException("Email đã tồn tại");
        
        var document = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = request.Password.ToSha256()
        };
        document.MarkCreated();

        await _userCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

        // await EmailHelper.SmptSendAsync(
        //     appSettings.GmailConfig,
        //     request.Email,
        //     "Đăng ký tài khoản người dùng",
        //     "Bạn đã đăng ký thành công tài khoản eCinemas");

        return APIResponse.IsSuccess("Đăng ký thành công");
    }
}