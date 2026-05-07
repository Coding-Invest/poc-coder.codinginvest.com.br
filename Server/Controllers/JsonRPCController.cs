using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    /// <summary>
    /// Controller for handling JSON-RPC requests for various tools.
    /// </summary>
    [Route("api/[controller]")]
    public class JsonRPCController : Controller
    {
        private readonly IFileListService _fileListService;
        private readonly IFileReadService _fileReadService;
        private readonly IFileWriteService _fileWriteService;
        private readonly IFileDeleteService _fileDeleteService;
        private readonly IFileDynamicReadService _fileDynamicReadService;
        private readonly IGitCloneService _gitCloneService;
        private readonly IGitBranchCreateService _gitBranchCreateService;
        private readonly IGitCheckoutService _gitCheckoutService;
        private readonly IGitCommitService _gitCommitService;
        private readonly IGithubAddService _githubAddService;
        private readonly IGitPullRequestService _gitPullRequestService;
        private readonly IGitPullService _gitPullService;
        private readonly IGitPushService _gitPushService;
        private readonly IMemorySaveService _memorySaveService;
        private readonly IMemoryLoadService _memoryLoadService;
        private readonly IMemorySummarizeService _memorySummarizeService;
        private readonly IDotnetBuildService _dotnetBuildService;
        private readonly IEmailSendService _emailSendService;
        private readonly IGoogleSearchService _googleSearchService;
        private readonly IFetchUrlService _fetchUrlService;
        private readonly INgBuildService _ngBuildService;
        private readonly INpmInstallService _npmInstallService;
        private readonly IConfiguration _configuration;

        public JsonRPCController(IGitCloneService gitCloneService, IConfiguration configuration, IFileListService fileListService, IFileReadService fileReadService, IFileWriteService fileWriteService, IFileDeleteService fileDeleteService, IGitBranchCreateService gitBranchCreateService, IGitCheckoutService gitCheckoutService, IGitCommitService gitCommitService, IGithubAddService githubAddService, IGitPullRequestService gitPullRequestService, IGitPullService gitPullService, IGitPushService gitPushService, IMemorySaveService memorySaveService, IMemoryLoadService memoryLoadService, IDotnetBuildService dotnetBuildService, IEmailSendService emailSendService, IGoogleSearchService googleSearchService, IFetchUrlService fetchUrlService, IFileDynamicReadService fileDynamicReadService, IMemorySummarizeService memorySummarizeService, INgBuildService ngBuildService, INpmInstallService npmInstallService)
        {
            _gitCloneService = gitCloneService;
            _configuration = configuration;
            _fileListService = fileListService;
            _fileReadService = fileReadService;
            _fileWriteService = fileWriteService;
            _fileDeleteService = fileDeleteService;
            _fileDynamicReadService = fileDynamicReadService;
            _gitBranchCreateService = gitBranchCreateService;
            _gitCheckoutService = gitCheckoutService;
            _gitCommitService = gitCommitService;
            _githubAddService = githubAddService;
            _gitPullRequestService = gitPullRequestService;
            _gitPullService = gitPullService;
            _gitPushService = gitPushService;
            _memorySaveService = memorySaveService;
            _memoryLoadService = memoryLoadService;
            _memorySummarizeService = memorySummarizeService;
            _dotnetBuildService = dotnetBuildService;
            _emailSendService = emailSendService;
            _googleSearchService = googleSearchService;
            _fetchUrlService = fetchUrlService;
            _ngBuildService = ngBuildService;
            _npmInstallService = npmInstallService;
        }

        /// <summary>
        /// Processes a JSON-RPC request and returns the appropriate response based on the method.
        /// </summary>
        /// <param name="protocol">The JSON-RPC protocol request.</param>
        /// <returns>An IActionResult containing the response.</returns>
        [HttpPost]
        public async Task<IActionResult> Index([FromBody]ProtocolRequest protocol)
        {
            var response = new Response.ProtocolResponse();
            try
            {
                switch (protocol.Method)
                {
                    case Tools.TOOLS_LIST:
                        response = GetToolList(protocol);
                        break;
                    case Tools.SCHEMA_TOOL:
                        response = GetSchema(protocol);
                        break;
                    case Tools.FILE_LIST:
                        response = FileList(protocol);
                        break;
                    case Tools.FILE_READ:
                        response = FileRead(protocol);
                        break;
                    case Tools.FILE_WRITE:
                        response = FileWrite(protocol);
                        break;
                    case Tools.FILE_DELETE:
                        response = FileDelete(protocol);
                        break;
                    case Tools.FILE_DYNAMIC_READ:
                        response = FileDynamicRead(protocol);
                        break;
                    case Tools.GIT_CLONE:
                        response = GitClone(protocol);
                        break;
                    case Tools.GIT_BRANCH_CREATE:
                        response = GitBranchCreate(protocol);
                        break;
                    case Tools.GIT_CHECKOUT:
                        response = GitCheckout(protocol);
                        break;
                    case Tools.GIT_COMMIT:
                        response = GitCommit(protocol);
                        break;
                    case Tools.GIT_ADD:
                        response = GitAdd(protocol);
                        break;
                    case Tools.EMAIL_SEND:
                        response = EmailSend(protocol);
                        break;
                    case Tools.MEMORY_SAVE:
                        response = MemorySave(protocol);
                        break;
                    case Tools.MEMORY_LOAD:
                        response = MemoryLoad(protocol);
                        break;
                    case Tools.MEMORY_SUMMARIZE:
                        response = await MemorySummarize(protocol);
                        break;
                    case Tools.DOTNET_BUILD:
                        response = DotnetBuild(protocol);
                        break;
                    case Tools.NG_BUILD:
                        response = NgBuild(protocol);
                        break;
                    case Tools.NPM_INSTALL:
                        response = NpmInstall(protocol);
                        break;
                    case Tools.GIT_PULL_REQUEST:
                        response = GitPullRequest(protocol);
                        break;
                    case Tools.GIT_PULL:
                        response = GitPull(protocol);
                        break;
                    case Tools.GIT_PUSH:
                        response = GitPush(protocol);
                        break;
                    case Tools.GOOGLE_SEARCH:
                        response = GoogleSearch(protocol);
                        break;
                    case Tools.FETCH_URL:
                        response = await FetchUrl(protocol);
                        break;
                }
            }
            catch (Exception e)
            {
                var errorResponse = new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = "Exception:"+ e.Message
                };
                return Ok(errorResponse);
            }
            return Ok(response);
        }
        /// <summary>
        /// Retrieves the list of available tools.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response with the tool list.</returns>
        private Response.ProtocolResponse GetToolList(ProtocolRequest protocol)
        {
            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = Tools.GetToolsList(),
                Id = protocol.Id
            };
        }
        /// <summary>
        /// Retrieves the schema for a specified tool.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response with the schema.</returns>
        private Response.ProtocolResponse GetSchema(ProtocolRequest protocol)
        {
            var jsonData = protocol.Params[0].ToString();
            var toolName = (string)JsonConvert.DeserializeObject<JObject>(jsonData)["toolName"];
            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = Tools.GetSchema(toolName),
                Id = protocol.Id
            };
        }
        /// <summary>
        /// Lists files and folders in a directory.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response with the file list.</returns>
        private Response.ProtocolResponse FileList(ProtocolRequest protocol)
        {
            var request = new Request.FileList(protocol);
            var response = _fileListService.Handle(request);
            return response;
        }

        /// <summary>
        /// Reads the content of a file.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response with the file content.</returns>
        private Response.ProtocolResponse FileRead(ProtocolRequest protocol)
        {
            var request = new Request.FileRead(protocol);
            var response = _fileReadService.Handle(request);
            return response;
        }

        /// <summary>
        /// Writes content to a file.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse FileWrite(ProtocolRequest protocol)
        {
            var request = new Request.FileWrite(protocol);
            var response = _fileWriteService.Handle(request);
            return response;
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse FileDelete(ProtocolRequest protocol)
        {
            var request = new Request.FileDelete(protocol);
            var response = _fileDeleteService.Handle(request);
            return response;
        }

        /// <summary>
        /// Reads a portion of a file from starting position with specified length.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response with the file portion.</returns>
        private Response.ProtocolResponse FileDynamicRead(ProtocolRequest protocol)
        {
            var request = new Request.FileDynamicRead(protocol);
            var response = _fileDynamicReadService.Handle(request);
            return response;
        }

        /// <summary>
        /// Clones a Git repository.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse GitClone(ProtocolRequest protocol)
        {
            var request = new Request.GitClone(protocol, _configuration);
            var response = _gitCloneService.Handle(request);
            return response;
        }

        /// <summary>
        /// Creates a new Git branch.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse GitBranchCreate(ProtocolRequest protocol)
        {
            var request = new Request.GitBranchCreate(protocol);
            var response = _gitBranchCreateService.Handle(request);
            return response;
        }

        /// <summary>
        /// Checkouts to a Git branch.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse GitCheckout(ProtocolRequest protocol)
        {
            var request = new Request.GitCheckout(protocol);
            var response = _gitCheckoutService.Handle(request);
            return response;
        }

        /// <summary>
        /// Commits changes to Git.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse GitCommit(ProtocolRequest protocol)
        {
            var request = new Request.GitCommit(protocol);
            var response = _gitCommitService.Handle(request);
            return response;
        }

        private Response.ProtocolResponse GitAdd(ProtocolRequest protocol)
        {
            var request = new Request.GitAdd(protocol, _configuration);
            var response = _githubAddService.Handle(request);
            return response;
        }

        private Response.ProtocolResponse EmailSend(ProtocolRequest protocol)
        {
            var request = new Request.EmailSend(protocol);
            var response = _emailSendService.Handle(request);
            return response;
        }

        /// <summary>
        /// Saves conversation history to a file.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse MemorySave(ProtocolRequest protocol)
        {
            var request = new Request.MemorySave(protocol);
            var response = _memorySaveService.Handle(request);
            return response;
        }

        /// <summary>
        /// Loads conversation history from a file.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse MemoryLoad(ProtocolRequest protocol)
        {
            var request = new Request.MemoryLoad(protocol);
            var response = _memoryLoadService.Handle(request);
            return response;
        }

        /// <summary>
        /// Summarizes the current memory.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private async Task<Response.ProtocolResponse> MemorySummarize(ProtocolRequest protocol)
        {
            var request = new Request.MemorySummarize(protocol);
            var response = await _memorySummarizeService.Handle(request);
            return response;
        }

        /// <summary>
        /// Builds the .NET solution.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse DotnetBuild(ProtocolRequest protocol)
        {
            var request = new Request.DotnetBuild(protocol);
            var response = _dotnetBuildService.Handle(request);
            return response;
        }

        /// <summary>
        /// Builds the Angular project.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse NgBuild(ProtocolRequest protocol)
        {
            var request = new Request.NgBuild(protocol);
            var response = _ngBuildService.Handle(request);
            return response;
        }

        /// <summary>
        /// Installs npm dependencies.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse NpmInstall(ProtocolRequest protocol)
        {
            var request = new Request.NpmInstall(protocol);
            var response = _npmInstallService.Handle(request);
            return response;
        }

        /// <summary>
        /// Creates a pull request on GitHub.
        /// </summary>
        /// <param name="protocol">The protocol request.</param>
        /// <returns>The protocol response.</returns>
        private Response.ProtocolResponse GitPullRequest(ProtocolRequest protocol)
        {
            var request = new Request.GitPullRequest(protocol);
            var response = _gitPullRequestService.Handle(request);
            return response;
        }

        private Response.ProtocolResponse GitPull(ProtocolRequest protocol)
        {
            var request = new Request.GitPull(protocol, _configuration);
            var response = _gitPullService.Handle(request);
            return response;
        }

        private Response.ProtocolResponse GitPush(ProtocolRequest protocol)
        {
            var request = new Request.GitPush(protocol, _configuration);
            var response = _gitPushService.Handle(request);
            return response;
        }

        private Response.ProtocolResponse GoogleSearch(ProtocolRequest protocol)
        {
            var request = new Request.GoogleSearch(protocol, _configuration);
            var response = _googleSearchService.Handle(request);
            return response;
        }

        private async Task<Response.ProtocolResponse> FetchUrl(ProtocolRequest protocol)
        {
            var request = new Request.FetchUrl(protocol);
            var response = await _fetchUrlService.Handle(request);
            return response;
        }

    }
}