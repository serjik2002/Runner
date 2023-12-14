using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Game
{
    public GameStateMachine StateMachine;
    public Game()
    {
        StateMachine = new GameStateMachine();
    }
}

