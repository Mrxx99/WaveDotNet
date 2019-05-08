﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rain.Designer.Views.Tree
{
	/// <summary>
	/// Interaction logic for ConnectionHandlesControl.xaml
	/// </summary>
	public partial class ConnectionHandlesControl : UserControl
	{
		public ConnectionHandlesControl()
		{
			InitializeComponent();
		}

		private void MouseDownOverPlusButton(object sender, MouseButtonEventArgs e)
		{
			StartConnection?.Invoke(this, e);
		}
		
		public event MouseButtonEventHandler StartConnection;
	}
}
