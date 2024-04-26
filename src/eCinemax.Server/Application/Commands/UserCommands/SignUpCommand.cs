using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Persistence;
using eCinemax.Server.Shared.Constants;
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
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống");
    }
}

public class SignUpCommandHandler(IMongoService mongoService) : IAPIRequestHandler<SignUpCommand>
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

        return APIResponse.IsSuccess("Đăng ký thành công");
    }
}