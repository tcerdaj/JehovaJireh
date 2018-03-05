using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace JehovaJireh.Web.Services.ErrorHandler
{
	public class ErrorResult : IHttpActionResult
	{
		private APIError[] _errors;
		HttpRequestMessage _request;

		public ErrorResult(APIError[] errors, HttpRequestMessage request)
		{
			_errors = errors;
			_request = request;
		}

		public ErrorResult(System.Exception ex, HttpRequestMessage request)
		{
			_errors = new APIError[] { new APIError(ex.Message, ex.StackTrace, getErrorType(ex)) };
			_request = request;
		}

		public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
			{
				Content = new StringContent(SerializeErrors(), Encoding.UTF8, "application/json"),
				RequestMessage = _request
			};
			return Task.FromResult(response);
		}

		protected string SerializeErrors()
		{
			string errorJson = string.Empty;

			if (_errors.Length > 0)
			{
				errorJson = JsonConvert.SerializeObject(_errors);
			}

			if (string.IsNullOrEmpty(errorJson))
			{
				errorJson = string.Empty;
			}

			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);

			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				writer.Formatting = Formatting.Indented;

				writer.WriteStartObject();
				writer.WritePropertyName("Message");
				writer.WriteValue("The request is invalid.");
				writer.WritePropertyName("Errors");
				writer.WriteRawValue(errorJson);
				writer.WriteEndObject();
			}
			return sb.ToString();
		}

		public ErrorType getErrorType(System.Exception ex)
		{
			ErrorType result;

			switch (ex.GetType().Name)
			{
				case "SqlException":
					result = ErrorType.Service;
					break;
				case "UnauthorizedAccessException":
					result = ErrorType.Authorization;
					break;
				case "ServerException":
					result = ErrorType.Internal;
					break;
				case "SecurityException":
					result = ErrorType.Authorization;
					break;
				case "AuthenticationException":
					result = ErrorType.Authentication;
					break;
				case "InvalidCredentialException":
					result = ErrorType.Authentication;
					break;
				case "TimeoutException":
					result = ErrorType.Service;
					break;
				default:
					result = ErrorType.Internal;
					break;
			}

			return result;
		}
	}

	public class APIError
	{
		public APIError(string message, string error, ErrorType type)
		{
			this.message = message;
			this.error = error;
			this.type = type;
		}

		public string message { get; set; }
		public string error { get; set; }
		public ErrorType type { get; set; }
	}

	public enum ErrorType
	{
		None = 0, Validation, Authorization, Authentication, Service, Internal
	}
}