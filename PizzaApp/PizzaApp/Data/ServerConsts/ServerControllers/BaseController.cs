namespace PizzaApp.Data.ServerConsts.ServerControllers
{
    public class BaseController
    {
        public string controllerUrl { get; private set; }
        public BaseController(string controllerUrl)
        {
            this.controllerUrl = controllerUrl;
        }
        public string GetFullUrl(string methodName)
        {
            return string.Format("{0}/{1}/{2}", ServerAddress.Url, controllerUrl, methodName);
        }
    }
}
