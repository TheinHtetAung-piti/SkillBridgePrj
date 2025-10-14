using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillBridge.DataAccess.Models;
using SkillBridge.Service.Model;
using SkillBridge.Service.Model.Project;
using SkillBridge.Service.Model.Project.SkillBridge.Service.Model.Project;

namespace SkillBridge.Service.Feature
{
    public class ProjectService
    {
        private readonly AppDbContext _appDbContext;

        public ProjectService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BaseResponseModel<object>> CreateProjectAsync(ProjectRequestModel req, int createdBy, string RoleCode)
        {

            int id = 0;
            if (RoleCode == "R001")
            {
                var ini = await _appDbContext.TblIndividualPfs.FirstOrDefaultAsync
                    (x => x.RoleCode == RoleCode && x.IndividualId == createdBy);

                id = ini.IndividualId;
            }
            if (RoleCode == "R001")
            {
                var mentor = await _appDbContext.TblMentors.FirstOrDefaultAsync
                    (x => x.MentorId == createdBy && x.RoleCode == RoleCode);
                id = mentor.MentorId;
            }
            try
            {
                var newProject = new TblProject
                {
                    ProjectName = req.ProjectName,
                    ProjectTitle = req.ProjectTitle,
                    ProjectDes = req.ProjectDes,
                    ProjectSub = req.ProjectSub,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    ProjImg = req.ProjImg,
                    AvaPos = req.AvaPos,
                    CreatedBy = id,
                    IsComplete = false,
                    DeleteFlag = false
                };

                _appDbContext.TblProjects.Add(newProject);
                await _appDbContext.SaveChangesAsync();

                return new BaseResponseModel<object>(
                    true,
                    "Project created successfully."
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<object>(
                    false,
                    ex.Message
                );
            }


        }
        public async Task<BaseResponseModel<ProjectResponseModel>> GetAllProjectsAsync()
        {
            try
            {
                var projects = await _appDbContext.TblProjects
                    .Where(p => p.DeleteFlag == false)
                    .Select(p => new ProjectResponseModel
                    {
                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,
                        ProjectTitle = p.ProjectTitle,
                        ProjectDes = p.ProjectDes,
                        ProjectSub = p.ProjectSub,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        ProjImg = p.ProjImg,
                        AvaPos = p.AvaPos,
                        IsComplete = p.IsComplete,
                        DeleteFlag = p.DeleteFlag,
                        CreatedBy = p.CreatedBy
                    })
                    .ToListAsync();

                return new BaseResponseModel<ProjectResponseModel>(
                    true,
                    "Projects fetched successfully",
                    projects
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<ProjectResponseModel>(
                    false,
                    $"Error fetching projects: {ex.Message}"
                );
            }
        }

        public async Task<BaseResponseModel<ProjectResponseModel>> UpdateProjectAsync(int projectId, ProjectRequestModel reqModel)
        {
            try
            {
                if (reqModel.ProjectName.IsNullOrEmp())
                    return new BaseResponseModel<ProjectResponseModel>(false, "Project Name is required.");

                if (reqModel.ProjectDes.IsNullOrEmp())
                    return new BaseResponseModel<ProjectResponseModel>(false, "Project Description is required.");

                var project = await _appDbContext.TblProjects
                    .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.DeleteFlag == false);

                if (project == null)
                {
                    return new BaseResponseModel<ProjectResponseModel>(false, "Project not found or already deleted.");

                }

                if (!reqModel.ProjectName.IsNullOrEmp())
                    project.ProjectName = reqModel.ProjectName!;

                if (!reqModel.ProjectTitle.IsNullOrEmp())
                    project.ProjectTitle = reqModel.ProjectTitle!;

                if (!reqModel.ProjectDes.IsNullOrEmp())
                    project.ProjectDes = reqModel.ProjectDes!;

                if (!reqModel.ProjectSub.IsNullOrEmp())
                    project.ProjectSub = reqModel.ProjectSub!;

                if (!reqModel.ProjImg.IsNullOrEmp())
                    project.ProjImg = reqModel.ProjImg!;

                if (reqModel.StartDate.HasValue)
                    project.StartDate = reqModel.StartDate;

                if (reqModel.EndDate.HasValue)
                    project.EndDate = reqModel.EndDate;

                if (reqModel.AvaPos.HasValue)
                    project.AvaPos = reqModel.AvaPos;

                if (reqModel.IsComplete.HasValue)
                    project.IsComplete = reqModel.IsComplete;

                if (reqModel.DeleteFlag.HasValue)
                    project.DeleteFlag = reqModel.DeleteFlag;

                if (reqModel.CreatedBy.HasValue)
                    project.CreatedBy = reqModel.CreatedBy;

                _appDbContext.TblProjects.Update(project);
                await _appDbContext.SaveChangesAsync();

                var responseModel = new ProjectResponseModel
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectTitle = project.ProjectTitle,
                    ProjectDes = project.ProjectDes,
                    ProjectSub = project.ProjectSub,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    ProjImg = project.ProjImg,
                    AvaPos = project.AvaPos,
                    IsComplete = project.IsComplete,
                    DeleteFlag = project.DeleteFlag,
                    CreatedBy = project.CreatedBy
                };

                return new BaseResponseModel<ProjectResponseModel>(
                    true,
                    "Project updated successfully.",
                    new List<ProjectResponseModel> { responseModel }
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<ProjectResponseModel>(false, $"Error updating project: {ex.Message}");
            }
        }


        public async Task<BaseResponseModel<ProjectResponseModel>> GetProjectDetailAsync(int projectId,string RoleCode)
        {
            try
            {
                var project = await _appDbContext.TblProjects
                    .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.DeleteFlag == false);

                if (project == null)
                {
                    return new BaseResponseModel<ProjectResponseModel>(false, "Project not found.");
                }

                // --- New Logic: Fetch Created By Details ---

                string creatorName = "Unknown";

                // 1. Try to find the creator in TblIndividualPf (assuming CreatedBy maps to IndividualId)
                var individual = await _appDbContext.TblIndividualPfs
                    .FirstOrDefaultAsync(i => i.IndividualId == project.CreatedBy && i.DeleteFlag == false);

                if (individual != null)
                {
                    creatorName = individual.IndividualName ?? "N/A";
                    // Assuming RoleCode is a string, you might need to parse it or map it to an int ID
                }
                else // 2. If not found, try to find the creator in TblMentor (assuming CreatedBy maps to MentorId)
                {
                    var mentor = await _appDbContext.TblMentors
                        .FirstOrDefaultAsync(m => m.MentorId == project.CreatedBy && m.DeleteFlag == false);

                    if (mentor != null)
                    {
                        creatorName = mentor.MentorName; // MentorName is non-nullable
                    }
                    // If still not found, creatorName remains "Unknown" and role remains 0.
                }

                // --- End New Logic ---

                var result = new ProjectResponseModel
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectTitle = project.ProjectTitle,
                    ProjectDes = project.ProjectDes,
                    ProjectSub = project.ProjectSub,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    ProjImg = project.ProjImg,
                    AvaPos = project.AvaPos,
                    IsComplete = project.IsComplete,
                    DeleteFlag = project.DeleteFlag,
                    CreatedBy = project.CreatedBy,

                    CreatedByName = creatorName,
                    CreatedByRole = RoleCode
                };

                return new BaseResponseModel<ProjectResponseModel>(
                    true,
                    "Project details retrieved successfully.",
                    new List<ProjectResponseModel> { result }
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<ProjectResponseModel>(
                    false,
                    $"Error fetching project details: {ex.Message}"
                );
            }
        }

        public async Task<BaseResponseModel<ProjectResponseModel>> DeleteProjectAsync(int projectId)
        {
            try
            {
                // Find the project that is not already deleted
                var project = await _appDbContext.TblProjects
                    .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.DeleteFlag == false);

                if (project == null)
                {
                    return new BaseResponseModel<ProjectResponseModel>(false, "Project not found or already deleted.");
                }

                // Soft delete
                project.DeleteFlag = true;

                _appDbContext.TblProjects.Update(project);
                await _appDbContext.SaveChangesAsync();

                // Prepare response model
                var response = new ProjectResponseModel
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectTitle = project.ProjectTitle,
                    ProjectDes = project.ProjectDes,
                    ProjectSub = project.ProjectSub,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    ProjImg = project.ProjImg,
                    AvaPos = project.AvaPos,
                    IsComplete = project.IsComplete,
                    DeleteFlag = project.DeleteFlag,
                    CreatedBy = project.CreatedBy
                };

                return new BaseResponseModel<ProjectResponseModel>(
                    true,
                    "Project deleted successfully.",
                    new List<ProjectResponseModel> { response }
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<ProjectResponseModel>(
                    false,
                    $"Error deleting project: {ex.Message}"
                );
            }
        }
    }    
}
