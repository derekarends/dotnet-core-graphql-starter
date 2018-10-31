using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Repository.Core
{
    public static class MongoExtensions
    {
        public static ObjectId ToObjectId(this string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
        
        public static async Task WithTimeout(this Task task, TimeSpan timeout)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, cancellationTokenSource.Token));
                if (completedTask == task)
                {
                    cancellationTokenSource.Cancel();
                    await task;
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }
}