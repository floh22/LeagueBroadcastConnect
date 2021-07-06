<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![Donate][donate-paypal]](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZSY2FXPUHXVUJ)


<!-- ABOUT THE PROJECT -->
## League Broadcast Connect

This is an addon for [League Broadcast (Essence)](https://github.com/floh22/LeagueBroadcastHub) that allows game information to be saved to file while a game is running.
This is useful for those that wish to use the information in their own overlays when using broadcast software such as vmix.

## Features

This runs as a windows service, meaning that you only have to install once, and thats it. When a game starts with League Broadcast open on the target PC,
this addon will write game information to file. The information includes:

- Game time
- Team gold
- Team kills
- Team towers
- Team dragons

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

* Windows 10 20H1 (May 2020 Update) Build 19041
* Active Internet connection

### Installation

1. Download [latest release](https://github.com/floh22/LeagueBroadcastConnect/releases/latest)
2. Unzip release to desired install folder
3. Run Install.bat and enter desired information
4. Optional: open CMD and run .\LeagueBroadcastConnect to run only once and not install 


### Arguments
- `First argument: Local folder to write file to (Default: local folder)`
- `- u/url         IP of PC running League Broadcast (Default: localhost)` 
- `- p/port        Port used in LB by PC running League Broadcast (Default: 9001)`
- `- m/multifile   Wether to write all information to single file or split into multiple (Default: true)`
- `- f/file        Output file when running in single file mode (Default: Ingame.json)`


<!-- ISSUES -->
## Issues

See the [open issues](https://github.com/floh22/LeagueBroadcastConnect/issues) for a list of proposed features (and known issues).



<!-- LICENSE -->
## License

Distributed under the GPLv3 License. See `LICENSE` for more information.

__This is a standalone project from Lars Eble. Riot Games does not endorse or sponsor this project.__  

<!-- CONTACT -->
## Contact

Twitter - [@larseble](https://twitter.com/@larseble)

Support this project: [![Donate](https://img.shields.io/badge/Paypal-Donate-blueviolet?style=flat-square&logo=paypal)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZSY2FXPUHXVUJ)




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/floh22/LeagueBroadcastConnect.svg?style=for-the-badge
[contributors-url]: https://github.com/floh22/LeagueBroadcastConnect/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/floh22/LeagueBroadcastConnect.svg?style=for-the-badge
[forks-url]: https://github.com/floh22/LeagueBroadcastConnect/network/members
[stars-shield]: https://img.shields.io/github/stars/floh22/LeagueBroadcastConnect.svg?style=for-the-badge
[stars-url]: https://github.com/floh22/LeagueBroadcastConnect/stargazers
[issues-shield]: https://img.shields.io/github/issues/floh22/LeagueBroadcastConnect.svg?style=for-the-badge
[issues-url]: https://github.com/floh22/LeagueBroadcastConnect/issues
[license-shield]: https://img.shields.io/github/license/floh22/LeagueBroadcastConnect.svg?style=for-the-badge
[license-url]: https://github.com/floh22/LeagueBroadcastConnect/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/floh22
[donate-paypal]: https://img.shields.io/badge/Paypal-Donate-blueviolet?style=for-the-badge&logo=paypal
