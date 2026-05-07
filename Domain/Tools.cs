using System.Text.Json.Nodes;

namespace Domain
{
    public static partial class Tools
    {
        public const string WORKING_DIRECTORY = "./repos";
        public const string SCHEMA_TOOL = "schema.tool";
        public const string TOOLS_LIST = "tools.list";
        public const string FILE_LIST = "file.list";
        public const string FILE_READ = "file.read";
        public const string FILE_WRITE = "file.write";
        public const string FILE_DELETE = "file.delete";
        public const string GIT_CLONE = "git.clone";
        public const string GIT_BRANCH_CREATE = "git.branch.create";
        public const string GIT_CHECKOUT = "git.checkout";
        public const string GIT_COMMIT = "git.commit";
        public const string GIT_ADD = "git.add";
        public const string GIT_PULL_REQUEST = "git.pull.request";
        public const string GIT_PULL = "git.pull";
        public const string GIT_PUSH = "git.push";
        public const string MEMORY_SAVE = "memory.save";
        public const string MEMORY_LOAD = "memory.load";
        public const string DOTNET_BUILD = "dotnet.build";
        public const string EMAIL_SEND = "email.send";
        public const string NG_BUILD = "ng.build";
        public const string NPM_INSTALL = "npm.install";
        public const string GOOGLE_SEARCH = "google.search";
        public const string FETCH_URL = "fetch.url";
        public const string FILE_DYNAMIC_READ = "file.dynamic.read";
        public const string MEMORY_SUMMARIZE = "memory.summarize";

        private static IList<string> TOOLS_LIST_DESC = new List<string>{
            $"{SCHEMA_TOOL} - Show the schema to use the tools",
            $"{TOOLS_LIST} - List the tools available for assistant",
            $"{FILE_LIST} - List files and folders on the directory",
            $"{FILE_READ} - Read the file as an string",
            $"{FILE_WRITE} - Write a file, path: address to file from TargetDirectory, content: file content",
            $"{FILE_DELETE} - Delete a file or a directory",
            $"{GIT_CLONE} - Clone the repository, optionally specify repositoryUrl and directory, defaulting to appsettings",
            $"{GIT_BRANCH_CREATE} - Create a new remote Git branch and push to GitHub",
            $"{GIT_CHECKOUT} - Checkout to a specified Git branch",
            $"{GIT_COMMIT} - Commit changes with a message, optionally specify workDirectory",
            $"{GIT_ADD} - Add files to Git staging area, optionally specify path, default to '.'",
            $"{GIT_PULL_REQUEST} - Create a pull request from current branch to master on GitHub",
            $"{GIT_PULL} - Pull changes from the repository",
            $"{GIT_PUSH} - Push changes to the repository",
            $"{MEMORY_SAVE} - Save conversation history to a file",
            $"{MEMORY_LOAD} - Load conversation history from file",
            $"{DOTNET_BUILD} - Build the .NET solution and report/correct problems",
            $"{EMAIL_SEND} - Send an email using SMTP configuration",
            $"{NG_BUILD} - Build the Angular project and report/correct problems",
            $"{NPM_INSTALL} - Instala dependências npm em um projeto Node.js",
            $"{GOOGLE_SEARCH} - Perform a Google search and return results",
            $"{FETCH_URL} - Fetch content from a URL, and save it as [targetdirectory]/pagesfetched/[urlencodeurl].txt",
            $"{FILE_DYNAMIC_READ} - Read a file from index position with specified length",
            $"{MEMORY_SUMMARIZE} - Summarize the current memory to save space"
            };
        public static string GetToolsList()
        {
            return string.Join(";", TOOLS_LIST_DESC.Select(x => x));
        }
        public static string GetSchema(string ToolName)
        {
            return SCHEMA[ToolName]?.ToString() ?? "error: no tool was found!";
        }

        private static readonly Dictionary<string, JsonNode?> SCHEMA = new()
        {
            [SCHEMA_TOOL] = JsonNode.Parse("{\"method\":\"schema.tool\",\"args\":[{\"toolName\":\"string\"}]}"),
            [TOOLS_LIST] = JsonNode.Parse("{\"method\":\"tools.list\",\"args\":[]}"),
            [FILE_LIST] = JsonNode.Parse("{\"method\":\"file.list\",\"args\":[{\"path\":\"string\"}]}"),
            [FILE_READ] = JsonNode.Parse("{\"method\":\"file.read\",\"args\":[{\"path\":\"string\"}]}"),
            [FILE_WRITE] = JsonNode.Parse("{\"method\":\"file.write\",\"args\":[{\"path\":\"string\",\"content\":\"string\"}]}"),
            [FILE_DELETE] = JsonNode.Parse("{\"method\":\"file.delete\",\"args\":[{\"path\":\"string\"}]}"),
            [GIT_CLONE] = JsonNode.Parse("{\"method\":\"git.clone\",\"args\":[{\"repositoryUrl\":\"string\",\"directory\":\"string\"}]}"),
            [GIT_BRANCH_CREATE] = JsonNode.Parse("{\"method\":\"git.branch.create\",\"args\":[{\"branchName\":\"string\"}]}"),
            [GIT_CHECKOUT] = JsonNode.Parse("{\"method\":\"git.checkout\",\"args\":[{\"branchName\":\"string\"}]}"),
            [GIT_COMMIT] = JsonNode.Parse("{\"method\":\"git.commit\",\"args\":[{\"message\":\"string\",\"workDirectory\":\"string\"}]}"),
            [GIT_ADD] = JsonNode.Parse("{\"method\":\"git.add\",\"args\":[{\"path\":\"string\"}]}"),
            [GIT_PULL_REQUEST] = JsonNode.Parse("{\"method\":\"git.pull.request\",\"args\":[{\"title\":\"string\",\"body\":\"string\",\"head\":\"string\",\"base\":\"string\"}]}"),
            [GIT_PULL] = JsonNode.Parse("{\"method\":\"git.pull\",\"args\":[{\"repositoryUrl\":\"string\",\"directory\":\"string\"}]}"),
            [GIT_PUSH] = JsonNode.Parse("{\"method\":\"git.push\",\"args\":[]}"),
            [MEMORY_SAVE] = JsonNode.Parse("{\"method\":\"memory.save\",\"args\":[{\"content\":\"string\"}]}"),
            [MEMORY_LOAD] = JsonNode.Parse("{\"method\":\"memory.load\",\"args\":[]}"),
            [DOTNET_BUILD] = JsonNode.Parse("{\"method\":\"dotnet.build\",\"args\":[{\"path\":\"string\"}]}"),
            [EMAIL_SEND] = JsonNode.Parse("{\"method\":\"email.send\",\"args\":[{\"to\":\"string\",\"subject\":\"string\",\"body\":\"string\"}]}"),
            [GOOGLE_SEARCH] = JsonNode.Parse("{\"method\":\"google.search\",\"args\":[{\"query\":\"string\",\"numResults\":\"int\"}]}"),
            [FETCH_URL] = JsonNode.Parse("{\"method\":\"fetch.url\",\"args\":[{\"url\":\"string\"}]}"),
            [FILE_DYNAMIC_READ] = JsonNode.Parse("{\"method\":\"file.dynamic.read\",\"args\":[{\"path\":\"string\",\"index\":\"int\",\"length\":\"int\"}]}"),
            [MEMORY_SUMMARIZE] = JsonNode.Parse("{\"method\":\"memory.summarize\",\"args\":[]}"),
            [NG_BUILD] = JsonNode.Parse("{\"method\":\"ng.build\",\"args\":[{\"path\":\"string\"}]}"),
            [NPM_INSTALL] = JsonNode.Parse("{\"method\":\"npm.install\",\"args\":[{\"path\":\"string\"}]}")
        };
    }
}