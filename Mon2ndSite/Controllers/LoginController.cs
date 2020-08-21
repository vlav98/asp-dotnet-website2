using Mon2ndSite.Models;
using Mon2ndSite.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace Mon2ndSite.Controllers
{
    public class LogInController : Controller
    {
        private IDal dal;

        public LogInController() : this(new Dal())
        {

        }

        private LogInController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            UserViewModel viewModel = new UserViewModel { Log = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.User = dal.GetUser(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UserViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = dal.LogIn(viewModel.User.Username, viewModel.User.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
                ModelState.AddModelError("User.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                int id = dal.AddUser(user.Username, user.Password);
                FormsAuthentication.SetAuthCookie(id.ToString(), false);
                return Redirect("/");
            }
            return View(user);
        }

        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}