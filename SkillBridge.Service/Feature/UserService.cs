using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillBridge.DataAccess.Models;
using SkillBridge.Service.Feature;
using SkillBridge.Service.Model;
using SkillBridge.Service.Model.User;

namespace SkillBridge.Service.Feature
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;

		public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #region Create User
        public async Task<BaseResponseModel> CreateUserAsync(CreateUserReqModel reqModel)
        {
			try
			{
				if (reqModel.Username.IsNullOrEmp())
				{
					return new BaseResponseModel(false, "Username is Required.");
				}
				if (reqModel.Email.IsNullOrEmp())
				{
					return new BaseResponseModel(false, "Emain is Required.");
				}
				if (reqModel.Password.IsNullOrEmp())
				{
					return new BaseResponseModel(false, "Passowrd is Required.");
				}

				var model = await _appDbContext.TblUsers.FirstOrDefaultAsync(x => x.Username == reqModel.Username || 
				x.Email == reqModel.Email);

				if(model != null)
				{
					return new BaseResponseModel(false, "Account already exist.");
				}

                TblUser item = new TblUser()
                {
                    Username = reqModel.Username,
                    Email = reqModel.Email,
                    RoleCode = reqModel.RoleCode,
                };

                _appDbContext.TblUsers.Add(item);

                if(item.RoleCode == "R001")
                {
                    TblIndividualPf inidividual = new TblIndividualPf()
                    {
                        IndividualId = item.UserId,
                        RoleCode = item.RoleCode,
                    };
                    _appDbContext.TblIndividualPfs.Add(inidividual);
                }
                if (item.RoleCode == "R002")
                {
                    TblCompany company = new TblCompany()
                    {
                        CompanyId = item.UserId,
                        RoleCode = item.RoleCode,
                    };
                    _appDbContext.TblCompanies.Add(company);
                }

                if (item.RoleCode == "R003")
                {
                    TblMentor mentor = new TblMentor()
                    {
                        MentorId = item.UserId,
                        RoleCode = item.RoleCode,
                    };
                    _appDbContext.TblMentors.Add(mentor);
                }

                UserResponseModel responseModel = new UserResponseModel()
                {
                    Username = reqModel.Username,
                    Email = reqModel.Email,
                };
                List<UserResponseModel> list = new List<UserResponseModel>();

                list.Add(responseModel);

                await _appDbContext.SaveChangesAsync();
                return new BaseResponseModel(true, "Sign Up Success", list);
               
			}
			catch (Exception ex)
			{
				return new BaseResponseModel(false, ex.Message);
			}
        }
        #endregion

        public async Task<LoginBaseResponseModel> IdividualSkillProfile(LoginReqModel reqModel,SkillProfileReqModel sreqModel)
        {
            try
            {
                var model = await _appDbContext.TblUsers.FirstOrDefaultAsync(
                    x => x.Email == reqModel.Email && x.Username == reqModel.Username);
                if(model is null)
                {
                    return new LoginBaseResponseModel(false, "Something went wrong.");
                }
                var SkillProfile = await _appDbContext.TblIndividualPfs.FirstOrDefaultAsync(
                    x => x.IndividualId == model.UserId);
                if (SkillProfile is null)
                {
                    return new LoginBaseResponseModel(false, "Something went wrong.");
                }

                SkillProfile.IndividualName = sreqModel.IndividualName;
                SkillProfile.IsStudent = sreqModel.IsStudent;
                SkillProfile.ProField = sreqModel.ProField;
                SkillProfile.InterestArea = sreqModel.InterestArea;
                SkillProfile.Exp = sreqModel.Exp;
                SkillProfile.IndividualImg = sreqModel.IndividualImg;
                SkillProfile.Address = sreqModel.Address;
                SkillProfile.WorkingHour = sreqModel.WorkingHour;


                await _appDbContext.SaveChangesAsync();

                UserResponseModel resModel = new UserResponseModel()
                {
                    Username = reqModel.Username,
                    Email = reqModel.Email,

                };
                return new LoginBaseResponseModel(true, "Success.", resModel ,sreqModel);

            }
            catch (Exception ex)
            {
                return new LoginBaseResponseModel(false, ex.Message);
            }
        }


        #region Login
        public async Task<LoginBaseResponseModel> LoginAsync(LoginReqModel reqModel)
        {
            try
            {
                if (reqModel.Email.IsNullOrEmp())
                {
                    return new LoginBaseResponseModel(false, "Email is required.");
                }

                if (reqModel.Password.IsNullOrEmp())
                {
                    return new LoginBaseResponseModel(false, "Password is required.");
                }

                if (reqModel.Username.IsNullOrEmp())
                {
                    return new LoginBaseResponseModel(false, "UserName is required.");
                }

                var user = await _appDbContext.TblUsers
                    .FirstOrDefaultAsync(x => x.Email == reqModel.Email && x.Username == reqModel.Username);

                if (user == null)
                {
                    return new LoginBaseResponseModel(false, "Account not found.");
                }

                if (user.Password != reqModel.Password)
                {
                    return new LoginBaseResponseModel(false, "Incorrect password.");
                }

        var userModel = await _appDbContext.TblUsers.FirstOrDefaultAsync(x => x.Email == reqModel.Email);

        var SkillModel = await _appDbContext.TblIndividualPfs.FirstOrDefaultAsync(x => x.IndividualId == userModel.UserId);

        UserResponseModel userResponseModel = new UserResponseModel()
        {
            Username = userModel.Username,
            Email = userModel.Email,
        };

        SkillProfileReqModel skillProfileResModel = new SkillProfileReqModel()
        {
            IndividualName = SkillModel.IndividualName,
            IsStudent = SkillModel.IsStudent,
            ProField = SkillModel.InterestArea,
            InterestArea = SkillModel.InterestArea,
            Exp = SkillModel.Exp,
            IndividualImg = SkillModel.IndividualImg,
            Address = SkillModel.Address,
            WorkingHour = SkillModel.WorkingHour

        };
        return new LoginBaseResponseModel(true, "Sign Up Success.", userResponseModel, skillProfileResModel);
    }
            catch (Exception ex)
            {
                return new LoginBaseResponseModel(false, ex.Message);
            }
        }
        #endregion

        #region Get All User
        public async Task<BaseResponseModel<FullUserModel>> GetAllUsersAsync()
        {
            try
            {
                var userProfiles = await (
                    from user in _appDbContext.TblUsers
                    join profile in _appDbContext.TblIndividualPfs
                        on user.UserId equals profile.IndividualId into userProfileJoin
                    from profile in userProfileJoin.DefaultIfEmpty()
                    where user.DeleteFlag == false
                    select new FullUserModel
                    {
                        Username = user.Username,
                        Email = user.Email,
                        IndividualName = profile.IndividualName,
                        IsStudent = profile.IsStudent,
                        ProField = profile.ProField,
                        InterestArea = profile.InterestArea,
                        Exp = profile.Exp,
                        IndividualImg = profile.IndividualImg,
                        Address = profile.Address,
                        WorkingHour = profile.WorkingHour
                    }
                ).ToListAsync();

                return new BaseResponseModel<FullUserModel>(
                    true,
                    "Users fetched successfully",
                    userProfiles
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<FullUserModel>(
                    false,
                    ex.Message);

            }
        }
        #endregion

        public async Task<BaseResponseModel<FullUserModel>> GetDetailUserAsync(int userId)
        {
            try
            {
                var userProfile = await (
                    from user in _appDbContext.TblUsers
                    join profile in _appDbContext.TblIndividualPfs
                        on user.UserId equals profile.IndividualId into userProfileJoin
                    from profile in userProfileJoin.DefaultIfEmpty()
                    where user.UserId == userId && user.DeleteFlag == false
                    select new FullUserModel
                    {
                        Username = user.Username,
                        Email = user.Email,
                        IndividualName = profile.IndividualName,
                        IsStudent = profile.IsStudent,
                        ProField = profile.ProField,
                        InterestArea = profile.InterestArea,
                        Exp = profile.Exp,
                        IndividualImg = profile.IndividualImg,
                        Address = profile.Address,
                        WorkingHour = profile.WorkingHour
                    }
                ).FirstOrDefaultAsync();

                if (userProfile == null)
                {
                    return new BaseResponseModel<FullUserModel>(
                        false,
                        "User not found or deleted."
                    );
                }

                return new BaseResponseModel<FullUserModel>(
                    true,
                    "User detail fetched successfully",
                    new List<FullUserModel> { userProfile }
                );
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<FullUserModel>(
                    false,
                    $"Error fetching user detail: {ex.Message}"
                );
            }
        }
    }
}
