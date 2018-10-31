using System;
using Core.Settings;
using Mongo2Go;
using Repository.Core;

namespace Repository.Test.Core
{
    public class MongoTestRunner : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private const string DatabaseName = "IntegrationTest";

        public MongoTestRunner()
        {
            _runner = MongoDbRunner.Start();

            var settings = new SettingsModel
            {
                ConnectionStrings = _runner.ConnectionString,
                Database = DatabaseName
            };

            Context = new MongoContext(settings);
        }

        public MongoContext Context { get; }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}