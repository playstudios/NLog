// 
// Copyright (c) 2004-2016 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 
namespace NLog.Targets
{
    using System;
    using System.Text;
    using System.ComponentModel;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Writes log messages to an ArrayList in memory for programmatic retrieval.
	/// </summary>
	[Target("MemoryQueue")]
	public class MemoryQueueTarget : TargetWithLayout
	{
		public int maxEvents = 1000;

		public int NumErrors { get; private set; }

		public int NumWarnings { get; private set; }

		public int NumInfo { get; private set; }

		public int NumDebug { get; private set; }

		public int NumTrace { get; private set; }

		public event Action<LogEventInfo> NewEvent;

		protected Queue<LogEventInfo> eventsQueue;

		protected List<string> loggerNames;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryQueueTarget" /> class.
		/// </summary>
		/// <remarks>
		/// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
		/// </remarks>
		public MemoryQueueTarget() : base()
		{
			eventsQueue = new Queue<LogEventInfo>();
			loggerNames = new List<string>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryQueueTarget" /> class.
		/// </summary>
		/// <remarks>
		/// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
		/// </remarks>
		/// <param name="name">Name of the target.</param>
		public MemoryQueueTarget(string name) : this()
		{
			this.Name = name;
		}

		/// <summary>
		/// Writes the specified logging event to the UnityEngine.Debug
		/// </summary>
		/// <param name="logEvent">The logging event.</param>
		protected override void Write(LogEventInfo logEvent)
		{

			lock (eventsQueue)
			{
				// Remove oldest events until we are within our maximum
				while (eventsQueue.Count >= maxEvents)
				{
					LogEventInfo e = eventsQueue.Dequeue();
					if (logEvent.Level == LogLevel.Error || logEvent.Level == LogLevel.Fatal)
					{
						NumErrors--;
					}
					else if (logEvent.Level == LogLevel.Warn)
					{
						NumWarnings--;
					}
					else if (logEvent.Level == LogLevel.Info)
					{
						NumInfo--;
					}
					else if (logEvent.Level == LogLevel.Debug)
					{
						NumDebug--;
					}
					else if (logEvent.Level == LogLevel.Trace)
					{
						NumTrace--;
					}

				}

				eventsQueue.Enqueue(logEvent);

				if (!loggerNames.Contains(logEvent.LoggerName))
				{
					loggerNames.Add(logEvent.LoggerName);
				}

				if (logEvent.Level == LogLevel.Error || logEvent.Level == LogLevel.Fatal)
				{
					NumErrors++;
				}
				else if (logEvent.Level == LogLevel.Warn)
				{
					NumWarnings++;
				}
				else if (logEvent.Level == LogLevel.Info)
				{
					NumInfo++;
				}
				else if (logEvent.Level == LogLevel.Debug)
				{
					NumDebug++;
				}
				else if (logEvent.Level == LogLevel.Trace)
				{
					NumTrace++;
				}
			}

			if (NewEvent != null)
			{
				NewEvent(logEvent);
			}

		}

		public LogEventInfo[] GetEvents()
		{
			lock (eventsQueue)
			{
				return (LogEventInfo[]) eventsQueue.ToArray();
			}
		}

		public Queue<LogEventInfo> GetEventsQueue()
		{
			return eventsQueue;
		}

		public List<string> GetLoggerNames()
		{
			return loggerNames;
		}

		public void Clear()
		{
			lock (eventsQueue)
			{
				eventsQueue.Clear();
				loggerNames.Clear();

				NumErrors = 0;
				NumWarnings = 0;
				NumInfo = 0;
				NumDebug = 0;
				NumTrace = 0;
			}
		}
	}
}