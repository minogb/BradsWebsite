using BradsWebsite.Areas.Resource.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BradsWebsite.Areas.Pyramid.Controllers
{
    [Area("Resource")]
    public class ResourceController : Controller
    {
        IConfiguration Configuration;

        public ResourceController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public FileContentResult Index(string id)
        {
            int iid;
            if(int.TryParse(id, System.Globalization.NumberStyles.HexNumber, null, out iid))
            {
                ResourceModel model = new ResourceModel(iid, Configuration);
                return File(model.Data, model.Type);
            }
            return File(new byte[] { },"");
        }
    }
}
