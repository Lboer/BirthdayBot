using Newtonsoft.Json;

namespace Berdthday_Bot
{
    public struct ConfigJson
    {
        // get the token from a seperate config.json file, token is used to validate the bot, prefix selects how to call commands.
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}
