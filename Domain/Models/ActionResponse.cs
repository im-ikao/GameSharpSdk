using System;

namespace GameSharp.Domain.Models
{
    [Serializable]
    public class ActionResponse<T>
    {
        public string Id;
        public ActionResult Result;
        public T Content;
        public string Error;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Id) == true)
                return false;

            return true;
        }
    }
}