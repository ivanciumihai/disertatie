using System.Web.Mvc;

public class MyAuthorize : AuthorizeAttribute
{
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        //you can change to any controller or html page.
        filterContext.Result = new RedirectResult("/Home/Index");
    }
}