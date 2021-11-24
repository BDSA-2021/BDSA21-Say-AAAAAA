namespace SELearning.Core;

public interface ICommentRepository
{
    Task<CommentDTO> GetAsync(int ID);
    Task<CommentDTO> CreateAsync(CommentDTO Comment);
    Task<CommentDTO> UpdateAsync(int ID, CommentDTO Comment);
    Task<CommentDTO> DeleteAsync(int ID);
}
