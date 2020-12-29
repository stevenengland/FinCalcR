using System;
using FlaUI.Core;
using FlaUI.Core.Tools;
using Application = FlaUI.Core.Application;

namespace FinCalcR.Gui.Interaction.Tests.Framework
{
    public class GuiAutomationFixture : IDisposable
    {
        private bool isInitialized;

        public AutomationBase Automation { get; private set; }

        public Application Application { get; private set; }

        public void Initialize(Func<AutomationBase> getAutomation, Func<Application> getApplication)
        {
            if (this.isInitialized)
            {
                return;
            }

            this.Application = getApplication.Invoke();
            this.Automation = getAutomation.Invoke();

            this.isInitialized = true;
        }

        public void Initialize(Func<AutomationBase> getAutomation)
        {
            if (this.isInitialized)
            {
                return;
            }

            this.Automation = getAutomation.Invoke();

            this.isInitialized = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
            if (this.Application != null)
            {
                this.Application.Close();
                Retry.WhileFalse(() => this.Application.HasExited, TimeSpan.FromSeconds(2), ignoreException: true);
                this.Application.Dispose();
                this.Application = null;
            }

            if (this.Automation != null)
            {
                this.Automation.Dispose();
                this.Automation = null;
            }
        }
    }
}
