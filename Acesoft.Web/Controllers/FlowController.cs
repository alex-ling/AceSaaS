using System;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Workflow;

namespace Acesoft.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "PLAT")]
    [Route("api/[controller]/[action]")]
    public class FlowController : ApiControllerBase
    {
        private readonly ILogger<FlowController> logger;
        private readonly IProcessService processService;
        private readonly IWorkflowService wfService;

        public FlowController(ILogger<FlowController> logger,
            IProcessService processService,
            IWorkflowService wfService)
        {
            this.logger = logger;
            this.processService = processService;
            this.wfService = wfService;
        }

        #region xml
        [HttpGet, MultiAuthorize, Action("获取XML")]
        public IActionResult GetXml(long processId)
        {
            var process = processService.Get(processId);
            return Ok(process.Xml);
        }

        [HttpPost, MultiAuthorize, Action("提交XML")]
        public IActionResult PostXml([FromBody]JObject data)
        {
            var processId = data.GetValue<long>("processid");
            var xml = data.GetValue("xml", "");
            if (xml.HasValue())
            {
                processService.Save(processId, xml);
            }

            return Ok(null);
        }
        #endregion

        #region wf
        [HttpPost, MultiAuthorize, Action("流程操作")]
        public IActionResult PostWf([FromBody]JObject data)
        {
            var action = (WfActionType)data.GetValue("action", 3);
            var runner = new WfRunner(AppCtx.AC)
            {
                AppInstanceId = App.GetQuery<long>("appinstanceid"),
                TaskId = App.GetQuery<long>("taskid"),
                Audit = (WfAuditState)data.GetValue("audit", 1),
                Opinion = data.GetValue("opinion", ""),
                LoadBackInstanceTask = action == WfActionType.Backward,
                LoadCanWithdraw = action == WfActionType.Withdraw,
                LoadNextTasks = action == WfActionType.Forward,
                LoadPrevInstanceTask = action == WfActionType.Withdraw
            };

            if (action == WfActionType.Forward)
            {
                return Ok(wfService.Forward(runner));
            }
            else if (action == WfActionType.Backward)
            {
                return Ok(wfService.Backward(runner));
            }
            else if (action == WfActionType.Fetch)
            {
                return Ok(wfService.Fetch(runner));
            }
            else if (action == WfActionType.Withdraw)
            {
                return Ok(wfService.Withdraw(runner));
            }
            else
            {
                return Ok(wfService.Start(runner));
            }
        }
        #endregion
    }
}