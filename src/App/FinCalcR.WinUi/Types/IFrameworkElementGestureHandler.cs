﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StEn.FinCalcR.WinUi.Types
{
	public class IFrameworkElementGestureHandler : IGestureHandler
	{
		private readonly FrameworkElement element;

		public IFrameworkElementGestureHandler(FrameworkElement element)
		{
			this.element = element;
		}

		public Task<bool> IsLongTouch(TimeSpan duration)
		{
			var timer = new DispatcherTimer();
			var taskCompletionSource = new TaskCompletionSource<bool>();
			timer.Interval = duration;

			void TouchUpHandler(object sender, MouseButtonEventArgs e)
			{
				this.element.PreviewMouseUp -= TouchUpHandler;
				timer.Stop();

				// https://devblogs.microsoft.com/pfxteam/the-meaning-of-taskstatus/ --> Tasks from TCS won't be running so cheching for WaitForActivation would be sufficient. Checking all final states is ok too.
				if (taskCompletionSource.Task.Status != TaskStatus.RanToCompletion
				    && taskCompletionSource.Task.Status != TaskStatus.Faulted
				    && taskCompletionSource.Task.Status != TaskStatus.Canceled)
				{
					taskCompletionSource.SetResult(false);
				}
			}

			this.element.PreviewMouseUp += TouchUpHandler;

			timer.Tick += (sender, e) =>
			{
				timer.Stop();
				taskCompletionSource.SetResult(true);
			};

			timer.Start();
			return taskCompletionSource.Task;
		}
	}
}
