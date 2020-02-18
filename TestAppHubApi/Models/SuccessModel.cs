using Microsoft.AspNetCore.Http;

namespace TestAppApi.Models
{
    public class SuccessModel : ResultModelBase
    {
        public SuccessModel()
            : base(true, StatusCodes.Status200OK)
        {
        }

        public int RequestId { get; set; }
    }
}
