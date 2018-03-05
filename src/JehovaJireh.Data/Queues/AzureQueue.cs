using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.Queues;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.AzureStorage;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Queue;

namespace JehovaJireh.Data.Queues
{
	public class AzureQueue : IQueue
	{
		private readonly CloudQueueClient queue;
		private readonly RetryPolicy storageRetryPolicy;
		private readonly ICollection<string> ensuredQueues;

		public AzureQueue(CloudQueueClient queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}

			this.queue = queue;
			this.ensuredQueues = new Collection<string>();
			var retryStrategy = new Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
			var retryPolicy = new RetryPolicy<StorageTransientErrorDetectionStrategy>(retryStrategy);
			this.storageRetryPolicy = retryPolicy;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "we want queue names to be lower case. We control the letters on that queue, hence we will never have culture problems")]
		public static string ResolveQueueName(MemberInfo messageType)
		{
			return messageType.Name.ToLowerInvariant();
		}

		public void AddMessage(QueueMessageBase message)
		{
			var queueName = ResolveQueueName(message.GetType());

			var json = Serialize(message.GetType(), message);
			var cloudQueue = this.storageRetryPolicy.ExecuteAction(() => this.queue.GetQueueReference(queueName));
			this.storageRetryPolicy.ExecuteAction(() => cloudQueue.AddMessage(new CloudQueueMessage(json)));
		}

		
		public void DeleteMessage(QueueMessageBase message)
		{
			var queueName = ResolveQueueName(message.GetType());

			var originalMessage = message.GetContext() as CloudQueueMessage;
			if (originalMessage != null)
			{
				var cloudQueue = this.storageRetryPolicy.ExecuteAction(() => this.queue.GetQueueReference(queueName));
				this.storageRetryPolicy.ExecuteAction(() => cloudQueue.DeleteMessage(originalMessage));
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "we are already providing a non-generic version. Why it is not detecting that?")]
		public T GetMessage<T>() where T : QueueMessageBase
		{
			return (T)this.GetMessage(typeof(T));
		}

		public object GetMessage(Type messageType)
		{
			var queueName = ResolveQueueName(messageType);

			var cloudQueue = this.queue.GetQueueReference(queueName);
			var originalMessage = cloudQueue.GetMessage();
			if (originalMessage != null)
			{
				var message = Deserialize(messageType, originalMessage.AsString) as QueueMessageBase;
				message.SetContext(originalMessage);
				return message;
			}

			return null;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "we are already providing")]
		public T[] GetMultipleMessages<T>(int maxMessages) where T : QueueMessageBase
		{
			return this.GetMultipleMessages(typeof(T), maxMessages).Cast<T>().ToArray();
		}

		public object[] GetMultipleMessages(Type messageType, int maxMessages)
		{
			var queueName = ResolveQueueName(messageType);

			var cloudQueue = this.queue.GetQueueReference(queueName);
			var originalMessages = cloudQueue.GetMessages(maxMessages);
			if (originalMessages != null)
			{
				var messages = new List<object>();
				foreach (var originalMessage in originalMessages)
				{
					var message = Deserialize(messageType, originalMessage.AsString) as QueueMessageBase;
					message.SetContext(originalMessage);
					messages.Add(message);
				}

				return messages.ToArray();
			}

			return null;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "we are already providing a non-generic version")]
		public void Purge<T>() where T : QueueMessageBase
		{
			this.Purge(typeof(T));
		}

		public void Purge(Type messageType)
		{
			var queueName = ResolveQueueName(messageType);
			this.EnsureQueueExists(queueName);
			var cloudQueue = this.queue.GetQueueReference(queueName);
			cloudQueue.Clear();
		}

		private static string Serialize(Type messageType, object obj)
		{
			var serializer = new DataContractJsonSerializer(messageType);
			using (var ms = new MemoryStream())
			{
				serializer.WriteObject(ms, obj);
				return Encoding.Default.GetString(ms.ToArray());
			}
		}

		private static object Deserialize(Type messageType, string json)
		{
			var obj = Activator.CreateInstance(messageType);
			using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
			{
				var serializer = new DataContractJsonSerializer(obj.GetType());
				obj = serializer.ReadObject(ms);
			}

			return obj;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "we are already providing a non-generic version")]
		public void EnsureQueueExists<T>() where T : QueueMessageBase
		{
			this.EnsureQueueExists(typeof(T));
		}

		public void EnsureQueueExists(Type messageType)
		{
			var queueName = ResolveQueueName(messageType);
			this.EnsureQueueExists(queueName);
		}


		private void EnsureQueueExists(string queueName)
		{

            try
            {
                if (!this.ensuredQueues.Contains(queueName))
                {
                    this.storageRetryPolicy.ExecuteAction(() => this.queue.GetQueueReference(queueName).CreateIfNotExists());
                    this.ensuredQueues.Add(queueName);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
		}
	}
}
