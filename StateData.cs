// League Broadcast Connect - League Broadcast (Essence) addon for game information in files
//     Copyright (C) 2021 Lars Eble
//     This file, StateData.cs, is part of League Broadcast Connect
//
//     League Broadcast Connect is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     League Broadcast (Essence) is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY, without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with League Broadcast Connect.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBroadcastConnect
{

    public class HeartbeatEvent 
    {
        public StateData stateData;
        public string eventType;
    }


    public class StateData
    {
        public FrontEndObjective dragon;

        public FrontEndObjective baron;

        public double gameTime;
        public bool gamePaused;

        public float blueGold;
        public float redGold;

        public Dictionary<double, float> goldGraph;
        public InhibitorInfo inhibitors;

        public ScoreboardConfig scoreboard;

        public InfoSidePage infoPage;

        public string blueColor;
        public string redColor;
    }

    public class ScoreboardConfig
    {
        public FrontEndTeam BlueTeam;
        public FrontEndTeam RedTeam;
        public double GameTime;
        public int SeriesGameCount;
    }

    public class FrontEndTeam
    {
        public string Name;
        public string Icon;
        public int Score;

        public int Kills;
        public int Towers;
        public float Gold;
        public List<string> Dragons;
    }

    public class Inhibitor
    {

        public string id;
        public int key;

        public double timeLeft;

        public Inhibitor(int key, string id)
        {
            this.id = id;
            this.key = key;
            this.timeLeft = 0;
        }

    }

    public class InhibitorInfo
    {
        public List<Inhibitor> Inhibitors;

        public Vector2 Location;

    }

    public class InfoSidePage
    {
        public string Title;
        public PlayerOrder Order;
        public List<PlayerTab> Players;
    }

    public enum PlayerOrder
    {
        MaxToMin,
        MinToMax
    }

    public class PlayerTab
    {
        public string PlayerName;
        public string IconPath;
        public ValueBar Values;
        public string[] ExtraInfo;
    }

    public class ValueBar
    {
        public double MinValue;
        public double MaxValue;
        public double CurrentValue;
    }

    public class FrontEndObjective
    {
        public string DurationRemaining { get; set; }

        public ObjectiveType Type { get; set; }

        public float GoldDifference { get; set; }

        public double SpawnTimer { get; set; }

    }

    public enum ObjectiveType
    {
        None = -1,
        Baron = 0,
        Dragon = 1
    }
}
