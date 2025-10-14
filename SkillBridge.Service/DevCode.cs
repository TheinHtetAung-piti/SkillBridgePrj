namespace SkillBridge.Service
{
    public static class DevCode
    {
        public static bool IsNullOrEmp(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) || str is null;
        }
    }
}
