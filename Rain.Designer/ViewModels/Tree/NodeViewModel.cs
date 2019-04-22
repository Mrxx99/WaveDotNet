﻿using Rain.Designer.DataStructures;
using Rain.Designer.ViewModels.Common;
using Rain.Designer.ViewModels.Tree.Helpers;
using Rain.Designer.ViewModels.Waves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Rain.Designer.ViewModels.Tree.Helpers.WaveBlockFactoryHelper;

namespace Rain.Designer.ViewModels.Tree
{
	internal class NodeViewModel : ViewModel
	{
		private readonly WaveBlockFactoryHelper _waveBlockFactoryHelper;
		private readonly WaveBuilderHelper _waveBuilderHelper;
		private readonly WavePlayerHelper _wavePlayerHelper;

		public NodeViewModel(
			WaveBlockFactoryHelper waveBlockFactoryHelper,
			WaveBuilderHelper waveBuilderHelper,
			WavePlayerHelper wavePlayerHelper)
		{
			_waveBlockFactoryHelper = waveBlockFactoryHelper;
			_waveBuilderHelper = waveBuilderHelper;
			_wavePlayerHelper = wavePlayerHelper;
		}

		private void InputChanged(object subTree, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(CanPlay):
					this.UpdateCanPlay();
					break;
			}
		}

		private IReadOnlyCollection<NodeViewModel> _inputs = new List<NodeViewModel>();
		public IReadOnlyCollection<NodeViewModel> Inputs
		{
			get => _inputs;
			set => Set(ref _inputs, value)
				.ObserveChildren(InputChanged)
				.Then(UpdateCanPlay);
		}

		public IReadOnlyCollection<WaveBlockFactory> AvailableWavesBlocks
		{
			get => _waveBlockFactoryHelper.AvailableFactories;
		}

		private WaveBlockViewModel _waveBlock;
		public WaveBlockViewModel WaveBlock
		{
			get => _waveBlock;
			set => Set(ref _waveBlock, value)
				.Then(UpdateCanPlay);
		}

		private bool _canPlay;
		public bool CanPlay
		{
			get => _canPlay;
			set => Set(ref _canPlay, value);
		}

		private Position _position;
		public Position Position
		{
			get => _position;
			set => Set(ref _position, value);
		}

		private void AddInput(NodeViewModel input)
		{
			Inputs = Inputs
				.Concat(new[] { input })
				.ToList();
		}

		private void RemoveInput(int index)
		{
			var inputsBefore = Inputs.Take(index);
			var inputsAfter = Inputs.Skip(index + 1);

			Inputs = inputsBefore
				.Concat(inputsAfter)
				.ToList();
		}

		private void ChangeWave(WaveBlockFactory waveBlockFactory)
		{
			WaveBlock = waveBlockFactory.Create();
		}

		private void UpdateCanPlay()
		{
			this.CanPlay =
				this.WaveBlock != null &&
				this.Inputs.All(subTree => subTree.CanPlay) &&
				this.WaveBlock.CanGenerate(this.Inputs.Count);
		}

		private void Play()
		{
			if (!CanPlay)
				return;

			var wave = _waveBuilderHelper.BuildWave(this);

			_wavePlayerHelper.PlayWave(wave);
		}

		public ICommand AddInputCommand => new Command<NodeViewModel>(AddInput);
		public ICommand RemoveInputCommand => new Command<int>(RemoveInput);
		public ICommand ChangeWaveCommand => new Command<WaveBlockFactory>(ChangeWave);
		public ICommand PlayCommand => new Command(Play);
	}
}