using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.QueryParameters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Patient_Management.Core.Contract
{
    public interface IUserService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken);
        Task<Response<string>> LogoutAsync();
        Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<UserResponse>> GetUserById(string id, CancellationToken cancellationToken);
        Task<Response<string>> EditUserAsync(EditUserRequest request, CancellationToken cancellationToken);
        //Task<Response<string>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken);
        Task<string> SoftDeleteUserAsync(string userId);
        Task<Response<string>> RegisterUser(RegisterRequest request);
        Task<UserResponse> CreatePassword(CreatePasswordRequest request);
    }
}
