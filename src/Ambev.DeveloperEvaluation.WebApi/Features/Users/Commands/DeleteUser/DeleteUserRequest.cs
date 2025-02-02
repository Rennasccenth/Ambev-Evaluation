namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;

public sealed class DeleteUserRequest
{
    public Guid Id { get; init; }

    public DeleteUserRequest(Guid id)
    {
        Id = id;
    }
}
