using System;
using System.Windows;
using System.Windows.Input;

namespace TopologyChess
{
	public partial class MainViewModel
	{
        private ICommand _randomTopologyCommand;

        public ICommand RandomTopologyCommand => _randomTopologyCommand ??= new RelayCommand(parameter =>
        {
            Move desiredMove = Move.BoardTransformation.RandomBoardTransformation(Chess);
            if (Chess != null && Chess.canTopologyMove(desiredMove.TopologyChange))
                Chess.Play(Move.BoardTransformation.RandomBoardTransformation(Chess));
            else
                MessageBox.Show("Attempted Illegal Topology");
        });
    }
}