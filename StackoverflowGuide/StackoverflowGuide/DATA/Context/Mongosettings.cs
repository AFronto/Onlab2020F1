using MongoDB.Driver;

namespace StackoverflowGuide.DATA.Context
{
    public class Mongosettings
    {
        public MongoClientSettings Connection { get; internal set; }
        public string DatabaseName { get; internal set; }
    }
}