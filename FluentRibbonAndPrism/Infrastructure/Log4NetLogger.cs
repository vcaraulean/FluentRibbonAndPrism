using log4net;
using Microsoft.Practices.Composite.Logging;

namespace FluentRibbonAndPrism.Infrastructure
{
	public class Log4NetLogger : ILoggerFacade
	{
		private static readonly ILog logger = LogManager.GetLogger(typeof(Log4NetLogger));

		public void Log(string message, Category category, Priority priority)
		{
			switch (category)
			{
				case Category.Debug:
					logger.Debug(message);
					break;

				case Category.Exception:
					logger.Error(message);
					break;

				case Category.Warn:
					logger.Warn(message);
					break;

				default:
					logger.Info(message);
					break;
			}
		}
	}
}