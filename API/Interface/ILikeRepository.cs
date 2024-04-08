using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interface
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int TargetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}