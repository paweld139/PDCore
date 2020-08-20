using System.Web.Optimization;

namespace PDWebCore.Helpers.WebOptimization.BundleTransform
{
    /// <summary>
    /// Transformacja Less na Css
    /// </summary>
    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = dotless.Core.Less.Parse(response.Content);
            response.ContentType = "text/css";
        }
    }
}
