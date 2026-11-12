namespace Bookstore.Api.Common;

public abstract class AppException(string message, int status, string code) : Exception(message)
{
    public int Status { get; } = status;
    public string Code { get; } = code;
}

public class ConflictException(string message, string code = "CONFLICT")
    : AppException(message, StatusCodes.Status409Conflict, code);

public class NotFoundException(string message, string code = "NOT_FOUND")
    : AppException(message, StatusCodes.Status404NotFound, code);

public class UnauthorizedException(string message, string code = "UNAUTHORIZED")
    : AppException(message, StatusCodes.Status401Unauthorized, code);

public class ForbiddenException(string message, string code = "FORBIDDEN")
    : AppException(message, StatusCodes.Status403Forbidden, code);

public class BussinessException(string message, string code = "BUSINESS_RULE_VIOLATION")
    : AppException(message, StatusCodes.Status422UnprocessableEntity, code);
