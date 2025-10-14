using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Service.Model.User;

namespace SkillBridge.Service.Feature
{
    public class LoginBaseResponseModel
    {
       public LoginBaseResponseModel(bool isSuccess , string message ,
           UserResponseModel umodel = null , SkillProfileReqModel smodel = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            UserResponse = umodel;
            skillProfileReq = smodel;
        }
        public bool IsSuccess {  get; set; }

        public string Message { get; set; } 

        public UserResponseModel UserResponse { get; set; }

        public SkillProfileReqModel skillProfileReq { get; set; }   
    }
}
