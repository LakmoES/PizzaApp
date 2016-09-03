namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class BaseUrlsController
    {
        public string controllerUrl { get; private set; }
        public BaseUrlsController(string controllerUrl)
        {
            this.controllerUrl = controllerUrl;
        }
        public string GetFullUrl(string methodName) => $"{ServerAddress.Url}/{controllerUrl}/{methodName}";
    }
}
