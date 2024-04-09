using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Constants;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;
using Todo.NET.Extensions;

namespace eCinemax.Server.Application.Commands.UserCommands;

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
        //     "Bạn đã đăng ký thành công tài khoản eCinemaxx");

        return APIResponse.IsSuccess("Đăng ký thành công");
    }
}