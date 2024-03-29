// using System.Text;
// using eCinemas.API.Aggregates.UserAggregate;
// using eCinemas.API.Helpers;
// using eCinemas.API.Services;
// using eCinemas.API.Shared.Constants;
// using eCinemas.API.Shared.Exceptions;
// using eCinemas.API.Shared.Extensions;
// using eCinemas.API.Shared.Mediator;
// using eCinemas.API.Shared.ValueObjects;
// using FluentValidation;
// using MongoDB.Driver;
//
// namespace eCinemas.API.Application.Commands.UserCommands;
//
// public class ForgotPasswordCommand : IAPIRequest
// {
//     public string Email { get; set; } = default!;
//     
//     public string? Otp { get; set; }
// }
//
// public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
// {
//     public ForgotPasswordCommandValidator()
//     {
//         RuleFor(x => x.Email)
//             .NotEmpty().WithMessage("Email không được bỏ trống")
//             .Matches(RegexConstant.EmailRegex).WithMessage("Email không đúng định dạng");
//     }
// }
//
// public class ForgotPasswordCommandHandler(IMongoService mongoService, AppSettings appSettings) : IAPIRequestHandler<ForgotPasswordCommand>
// {
//     private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
//     private const int OtpDurationInMinutes = 30;
//     private const int OtpLength = 6;
//     private const string OtpPattern = "0123456780";
//     
//     public async Task<APIResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
//     {
//         var filter = Builders<User>.Filter.Eq(x => x.Email, request.Email);
//         var user = await _userCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
//         DocumentNotFoundException<User>.ThrowIfNotFound(user, "Email không tồn tại");
//
//         if (request.Otp is null)
//         {
//             var otpValue = GenerateOtpValue();
//
//             var update = Builders<User>.Update.Set(x => x.OtpHash, otpValue.ToJson());
//             var result = await _userCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
//             if (!result.IsAcknowledged) throw new BadRequestException("Có lỗi xảy ra");
//             
//             await EmailHelper.SmptSendAsync(
//                 appSettings.GmailConfig,
//                 user.Email,
//                 "Reset mật khẩu tài khoản người dùng", 
//                 otpValue.Otp);
//         }
//         else
//         {
//             // var existedOtp = user.OtpHash?.ToObject<OtpValue>();
//             // if (existedOtp is not null && existedOtp.ExpiryTimestamp > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
//             // {
//             //     return API
//             // }
//         }
//         
//
//         return APIResponse.IsSuccess();
//     }
//
//     private OtpValue GenerateOtpValue()
//     {
//         var random = new Random();
//         var otpBuilder = new StringBuilder();
//
//         for (var i = 0; i < OtpLength; i++)
//         {
//             var index = random.Next(OtpLength);
//             otpBuilder.Append(OtpPattern[index]);
//         }
//
//         var otp = otpBuilder.ToString();
//         var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
//
//         return new OtpValue { Otp = otp, ExpiryTimestamp = timestamp };
//     }
// }