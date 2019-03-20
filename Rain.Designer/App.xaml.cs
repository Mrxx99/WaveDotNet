﻿using Microsoft.Extensions.DependencyInjection;
using Rain.Designer.Modules;
using Rain.Designer.ViewModels.Mesh;
using Rain.Designer.ViewModels.WaveDesigner;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Rain.Designer
{
	public partial class App : Application
	{
		internal WaveDesignerViewModel WaveDesigner { get; private set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			var container = PrepareServiceProvider();

			WaveDesigner = container.GetRequiredService<WaveDesignerViewModel>();

			base.OnStartup(e);
		}

		private ServiceProvider PrepareServiceProvider()
		{
			var serviceCollection = new ServiceCollection();

			MeshModule.Register(serviceCollection);
			WaveDesignerModule.Register(serviceCollection);

			return serviceCollection.BuildServiceProvider();
		}
	}
}
