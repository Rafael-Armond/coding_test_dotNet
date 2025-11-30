namespace CreditosConstituidos.Application.Common
{
    public record Result(bool Success, int StatusCode, object? Payload)
    {
        public static Result Accepted() =>
            new(true, 202, new { success = true });

        public static Result BadRequest(List<string> errors) =>
            new(false, 400, new { errors });

        public static Result Conflict(string message) =>
            new(false, 409, new { message, codigo = "CREDITO_DUPLICADO" });
    }
}

