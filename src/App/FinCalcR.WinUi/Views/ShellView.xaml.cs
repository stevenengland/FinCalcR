using System;
using System.Windows;
using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Views
{
	/// <summary>
	/// Interaction logic for ShellView.xaml.
	/// </summary>
	public partial class ShellView : Window
	{
		public ShellView()
		{
			this.InitializeComponent();
		}

		private void ColorZone_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (this.WindowState == System.Windows.WindowState.Maximized)
				{
					var x = e.GetPosition(this).X;
					var y = e.GetPosition(this).Y;
					var widthRatio = x / this.ActualWidth;
					var heightRatio = y / this.TitleBar.ActualHeight;
					this.WindowState = System.Windows.WindowState.Normal;
					this.Left = x - Convert.ToInt32(this.ActualWidth * widthRatio);
					this.Top = y - Convert.ToInt32(this.TitleBar.ActualHeight * heightRatio);
				}

				this.DragMove();
			}
		}
	}
}
