using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosFromMemberAsync( string memberId);

}
