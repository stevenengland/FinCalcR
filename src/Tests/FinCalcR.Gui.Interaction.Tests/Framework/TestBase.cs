using System;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using Xunit;

namespace FinCalcR.Gui.Interaction.Tests.Framework
{
    public abstract class TestBase : IClassFixture<GuiAutomationFixture>, IDisposable
    {
#if DEBUG
        protected const string Configuration = "Debug";
#else
        protected const string Configuration = "Release";
#endif
        private const int BigWaitTimeout = 5000;
        private readonly GuiAutomationFixture fixture;

        protected TestBase(GuiAutomationFixture fixture)
        {
            switch (this.ApplicationStartMode)
            {
                case ApplicationStartMode.None:
                    break;
                case ApplicationStartMode.OncePerTest:
                    fixture.Initialize(this.GetAutomation); // Will only init once through restriction in the init method.
                    this.Application = this.StartApplication();
                    break;
                case ApplicationStartMode.OncePerFixture:
                    fixture.Initialize(this.GetAutomation, this.StartApplication); // Will only init once through restriction in the init method.
                    this.Application = this.fixture.Application;
                    break;
                default:
                    break;
            }

            this.fixture = fixture;
            this.Automation = fixture.Automation;
        }

        protected AutomationBase Automation { get; }

        protected Application Application { get; private set; }

        protected virtual string AppPath => string.Empty;

        protected virtual ApplicationStartMode ApplicationStartMode => ApplicationStartMode.OncePerTest;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected static T WaitForElement<T>(Func<T> getter)
        {
            var retry = Retry.WhileNull<T>(
                getter,
                TimeSpan.FromMilliseconds(BigWaitTimeout));

            if (!retry.Success)
            {
                throw new TimeoutException($"Failed to get an element within a {BigWaitTimeout}ms");
            }

            return retry.Result;
        }

        // Can be useful in the future so it is kept for the moment
        // ReSharper disable once UnusedMember.Global
#pragma warning disable CA1822 // Mark members as static
        protected void WaitUntilClosed(AutomationElement element)
#pragma warning restore CA1822 // Mark members as static
        {
            var result = Retry.WhileFalse(() => element.IsOffscreen, TimeSpan.FromMilliseconds(BigWaitTimeout));
            if (!result.Success)
            {
                throw new TimeoutException($"Element failed to go offscreen within {BigWaitTimeout}ms");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
            switch (this.ApplicationStartMode)
            {
                case ApplicationStartMode.None:
                    break;
                case ApplicationStartMode.OncePerTest:
                    this.CloseApplication();
                    break;
                case ApplicationStartMode.OncePerFixture:
                    break;
                default:
                    break;
            }
        }

        protected abstract AutomationBase GetAutomation();

        protected abstract Application StartApplication();

        private void CloseApplication()
        {
            if (this.Application != null)
            {
                this.Application.Close();
                Retry.WhileFalse(() => this.Application.HasExited, TimeSpan.FromSeconds(2), ignoreException: true);
                this.Application.Dispose();
                this.Application = null;
            }
        }
    }
}
