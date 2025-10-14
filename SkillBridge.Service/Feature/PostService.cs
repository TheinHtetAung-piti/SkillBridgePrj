using Microsoft.EntityFrameworkCore;
using SkillBridge.DataAccess.Models;
using SkillBridge.Service.Model;
using SkillBridge.Service.Model.JobPost;

namespace SkillBridge.Service.Feature
{
    public class JobPostService
    {
        private readonly AppDbContext _appDbContext;

        public JobPostService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Create Job Post
        public async Task<BaseResponseModel<JobPostResponseModel>> CreateJobPostAsync(JobPostRequestModel reqModel)
        {
            try
            {
                if (reqModel.PostName.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post name is required.");

                if (reqModel.PostDes.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post description is required.");

                if (reqModel.CompanyId == null || reqModel.CompanyId <= 0)
                    return new BaseResponseModel<JobPostResponseModel>(false, "Company ID is required.");

                if (reqModel.JpImg.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Job post image is required.");

                var newJobPost = new TblJobPost
                {
                    PostId = Guid.NewGuid().ToString(),
                    PostName = reqModel.PostName,
                    PostDes = reqModel.PostDes,
                    JpImg = reqModel.JpImg,
                    CompanyId = reqModel.CompanyId.Value,
                    IsComplete = false,
                    DeleteFlag = false
                };

                await _appDbContext.TblJobPosts.AddAsync(newJobPost);
                await _appDbContext.SaveChangesAsync();

                var company = await _appDbContext.TblCompanies
                    .FirstOrDefaultAsync(c => c.CompanyId == newJobPost.CompanyId);

                var response = new JobPostResponseModel
                {
                    PostId = newJobPost.PostId,
                    PostName = newJobPost.PostName,
                    PostDes = newJobPost.PostDes,
                    JpImg = newJobPost.JpImg,
                    CompanyId = newJobPost.CompanyId,
                    CompanyName = company?.CompanyName,
                    IsComplete = newJobPost.IsComplete,
                    DeleteFlag = newJobPost.DeleteFlag
                };

                return new BaseResponseModel<JobPostResponseModel>(true, "Job post created successfully.", new List<JobPostResponseModel> { response });
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<JobPostResponseModel>(false, $"Error occurred: {ex.Message}", null);
            }
        }

        // Update Job Post
        public async Task<BaseResponseModel<JobPostResponseModel>> UpdateJobPostAsync(string postId, JobPostRequestModel reqModel)
        {
            try
            {
                if (postId.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post ID is required.");

                if (reqModel.PostName.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post name is required.");

                if (reqModel.PostDes.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post description is required.");

                if (reqModel.CompanyId == null || reqModel.CompanyId <= 0)
                    return new BaseResponseModel<JobPostResponseModel>(false, "Company ID is required.");

                var existingPost = await _appDbContext.TblJobPosts.FirstOrDefaultAsync(p => p.PostId == postId);

                if (existingPost == null)
                    return new BaseResponseModel<JobPostResponseModel>(false, "Job post not found.");

                existingPost.PostName = reqModel.PostName;
                existingPost.PostDes = reqModel.PostDes;
                existingPost.JpImg = reqModel.JpImg ?? existingPost.JpImg;
                existingPost.CompanyId = reqModel.CompanyId.Value;
                existingPost.IsComplete = reqModel.IsComplete ?? existingPost.IsComplete;
                existingPost.DeleteFlag = reqModel.DeleteFlag ?? existingPost.DeleteFlag;

                _appDbContext.TblJobPosts.Update(existingPost);
                await _appDbContext.SaveChangesAsync();

                var company = await _appDbContext.TblCompanies.FirstOrDefaultAsync(c => c.CompanyId == existingPost.CompanyId);

                var response = new JobPostResponseModel
                {
                    PostId = existingPost.PostId,
                    PostName = existingPost.PostName,
                    PostDes = existingPost.PostDes,
                    JpImg = existingPost.JpImg,
                    CompanyId = existingPost.CompanyId,
                    CompanyName = company?.CompanyName,
                    IsComplete = existingPost.IsComplete,
                    DeleteFlag = existingPost.DeleteFlag
                };

                return new BaseResponseModel<JobPostResponseModel>(true, "Job post updated successfully.", new List<JobPostResponseModel> { response });
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<JobPostResponseModel>(false, $"Error occurred: {ex.Message}", null);
            }
        }

        // Get All Job Posts with Company Name
        public async Task<BaseResponseModel<JobPostResponseModel>> GetAllJobPostsAsync()
        {
            try
            {
                var posts = await _appDbContext.TblJobPosts
                    .Where(p => p.DeleteFlag == false)
                    .ToListAsync();

                var companies = await _appDbContext.TblCompanies.ToListAsync();

                var response = posts.Select(p =>
                {
                    var company = companies.FirstOrDefault(c => c.CompanyId == p.CompanyId);

                    return new JobPostResponseModel
                    {
                        PostId = p.PostId,
                        PostName = p.PostName,
                        PostDes = p.PostDes,
                        JpImg = p.JpImg,
                        CompanyId = p.CompanyId,
                        CompanyName = company?.CompanyName,
                        IsComplete = p.IsComplete,
                        DeleteFlag = p.DeleteFlag
                    };
                }).ToList();

                return new BaseResponseModel<JobPostResponseModel>(true, "Job posts retrieved successfully.", new List<JobPostResponseModel>(response));
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<JobPostResponseModel>(false, $"Error occurred: {ex.Message}", null);
            }
        }

        // ✅ Get Job Post Detail by Id
        public async Task<BaseResponseModel<JobPostResponseModel>> GetJobPostByIdAsync(string postId)
        {
            try
            {
                if (postId.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post ID is required.");

                var post = await _appDbContext.TblJobPosts
                    .FirstOrDefaultAsync(p => p.PostId == postId && p.DeleteFlag == false);

                if (post == null)
                    return new BaseResponseModel<JobPostResponseModel>(false, "Job post not found.");

                var company = await _appDbContext.TblCompanies
                    .FirstOrDefaultAsync(c => c.CompanyId == post.CompanyId);

                var response = new JobPostResponseModel
                {
                    PostId = post.PostId,
                    PostName = post.PostName,
                    PostDes = post.PostDes,
                    JpImg = post.JpImg,
                    CompanyId = post.CompanyId,
                    CompanyName = company?.CompanyName,
                    IsComplete = post.IsComplete,
                    DeleteFlag = post.DeleteFlag
                };

                return new BaseResponseModel<JobPostResponseModel>(true, "Job post retrieved successfully.", new List<JobPostResponseModel> { response });
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<JobPostResponseModel>(false, $"Error occurred: {ex.Message}", null);
            }
        }

        // ✅ Delete Job Post by Id (Soft Delete)
        public async Task<BaseResponseModel<JobPostResponseModel>> DeleteJobPostByIdAsync(string postId)
        {
            try
            {
                if (postId.IsNullOrEmp())
                    return new BaseResponseModel<JobPostResponseModel>(false, "Post ID is required.");

                var post = await _appDbContext.TblJobPosts.FirstOrDefaultAsync(p => p.PostId == postId);

                if (post == null)
                    return new BaseResponseModel<JobPostResponseModel>(false, "Job post not found.");

                post.DeleteFlag = true;
                _appDbContext.TblJobPosts.Update(post);
                await _appDbContext.SaveChangesAsync();

                return new BaseResponseModel<JobPostResponseModel>(true, "Job post deleted successfully.", null);
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<JobPostResponseModel>(false, $"Error occurred: {ex.Message}", null);
            }
        }
    }
}
