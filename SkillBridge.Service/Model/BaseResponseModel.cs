using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Service.Model.User;

namespace SkillBridge.Service.Model
{
    public class BaseResponseModel
    {
        public BaseResponseModel(bool isSuccess , string message , List<UserResponseModel> data = null)
        {
            IsSuccess = isSuccess ;
            Message = message ;
            Data = data;
        }
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<UserResponseModel> Data;
    }
    public class BaseResponseModel<T>
    {
        public BaseResponseModel(bool isSuccess, string message, List<T>? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<T>? Data { get; set; }
    }

    public class BaseResponseModelGet
    {
        public BaseResponseModelGet(bool isSuccess, string message, List<FullUserModel> data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<FullUserModel> Data;
    }
}
