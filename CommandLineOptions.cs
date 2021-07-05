// League Broadcast Connect - League Broadcast (Essence) addon for game information in files
//     Copyright (C) 2021 Lars Eble
//     This file, CommandLineOptions.cs, is part of League Broadcast Connect
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

using CommandLine;

namespace LeagueBroadcastConnect
{
    public class CommandLineOptions
    {
        [Value(index: 0, Required = false, HelpText = "Folder to write to", Default = "localFolder")]
        public string Path { get; set; }

        [Option(shortName: 'f', longName: "file", Required = false, HelpText = "Text File Name", Default = "Ingame.json")]
        public string FileName { get; set; }

        [Option(shortName: 'u', longName: "url", Required = false, HelpText = "League Broadcast Remote Url", Default = "localhost")]
        public string Url { get; set; }

        [Option(shortName: 'p', longName: "port", Required = false, HelpText = "League Broadcast Remote Port", Default = 9001)]
        public int Port { get; set; }

        [Option(shortName: 'm', longName: "multifile", Required = false, HelpText = "Write to multiple Files", Default = true)]
        public bool Multifile { get; set; }
    }
}
