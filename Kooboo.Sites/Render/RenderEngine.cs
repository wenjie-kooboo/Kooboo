//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using Kooboo.Sites.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kooboo.Sites.Render
{
    public static class RenderEngine
    {
        public static async Task<string> RenderPageAsync(FrontContext context)
        {
            if (context.Page.Parameters.Count > 0)
            {
                context.RenderContext.DataContext.Push(context.Page.Parameters);
            }

            List<IRenderTask> renderPlan = null;

            if (context.RenderContext.Request.Channel != Data.Context.RequestChannel.InlineDesign)
            {
                renderPlan = Cache.RenderPlan.GetOrAddRenderPlan(context.SiteDb, context.Page.Id, () => RenderEvaluator.Evaluate(context.Page.Body, GetPageOption(context)));
            }
            else
            {
                string html = DomService.ApplyKoobooId(context.Page.Body);
                renderPlan = RenderEvaluator.Evaluate(html, GetPageOption(context));
                renderPlan.Insert(0, new BindingObjectRenderTask() { ObjectType = "page", NameOrId = context.Page.Id.ToString() });
            }

            var result = renderPlan.Render(context.RenderContext);

            if (context.Page.Type == Models.PageType.RichText)
            {
                //special for richtext editor. meta name = "viewport" content = "width=device-width, initial-scale=1"
                var header = new Models.HtmlHeader();
                Dictionary<string, string> content = new Dictionary<string, string>
                {
                    {"", "width=device-width, initial-scale=1"}
                };
                header.Metas.Add(new Models.HtmlMeta() { name = "viewport", content = content });

                result = HtmlHeadService.SetHeaderToHtml(result, header);
            }

            return result;
        }

        private static EvaluatorOption GetPageOption(FrontContext context)
        {
            EvaluatorOption renderoption = new EvaluatorOption();

            if (context.WebSite != null && context.WebSite.EnableSitePath)
            {
                renderoption.RenderUrl = true;
            }
            else
            {
                renderoption.RenderUrl = false;
            }

            if (context.Page.Headers.HasValue())
            {
                renderoption.RenderHeader = !context.Page.HasLayout;
            }
            else
            {
                renderoption.RenderHeader = false;
            }

            //renderoption.RenderHeader = context.Page.Headers.HasValue();

            renderoption.RequireBindingInfo = context.RenderContext.Request.Channel == Data.Context.RequestChannel.InlineDesign;
            renderoption.OwnerObjectId = context.Page.Id;
            return renderoption;
        }
    }
}