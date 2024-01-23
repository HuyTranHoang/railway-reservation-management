<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->

<a name="readme-top"></a>

<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/HuyTranHoang/railway-reservation-management">
    <img src="https://i.imgur.com/z7SFTce.png" alt="Logo" height="80">
  </a>

<h3 align="center">Swift Rails</h3>

  <p align="center">
    Railway Reservation Management
    <br />
  </p>
</div>

<!-- ABOUT THE PROJECT -->

## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

Indian Railways is one of the largest rail networks in world. Majority of people used to travel in train which is convenient and affordable means of transport. So keeping this in view, the reservation of railways is a most important task and it must be faster and efficient as the demand (travelers) is very high. In order to meet this demand, manual reservation is cumbersome and it requires an efficient program to implement the online reservation.

This Application enables us to choose the train even there is no necessary to fill a form at the railway reservation counter ,i.e. we can directly select from the choices provided for us with train numbers and their origin, departure time, destination & arrival time at that station and the class to travel in. Application gives us the final output as train ticket with the amount to be paid.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [![.Net][Dotnet.com]][Dotnet-url]
- [![Angular][Angular.io]][Angular-url]
- [![Bootstrap][Bootstrap.com]][Bootstrap-url]
- [![MicrosoftSQLServer][SqlServer.com]][SqlServer-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites

This is an example of how to list things you need to use the software and how to install them.

- Dotnet: SDK 6.0.417
  ```sh
  https://dotnet.microsoft.com/en-us/download/dotnet/6.0
  ```
- Nodejs: v16.20.2
  ```sh
  https://nodejs.org/en/download
  ```
- Npm: 8.19.4
  ```sh
  npm install npm@8.19.4 -g
  ```
- Angular CLI: 15.2.10
  ```sh
  npm install -g @angular/cli@15.2.10
  ```
- Sql Server
  ```sh
  https://www.microsoft.com/en-us/sql-server/sql-server-downloads
  ```

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/HuyTranHoang/railway-reservation-management.git
   ```
2. Rename "appsettings.Development.json.example" to "appsettings.Development.json" in WebApi Project

3. Change "DefaultConnection" with your local Database

4. Install NPM packages in ClientApp and AdminApp
   ```sh
   cd WebApi/ClientApp && npm install
   cd ../AdminApp && npm install --legacy-peer-deps
   ```
5. Run server in WebApi
   ```sh
   dotnet build
   dotnet run
   ```
6. Run client in ClientApp
   ```sh
   ng serve -o
   ```
7. Run admin client in AdminApp
   ```sh
   npm start
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[product-screenshot]: https://i.imgur.com/wkc3RYf.png
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[Dotnet.com]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[Dotnet-url]: https://dotnet.microsoft.com/en-us/download
[SqlServer.com]: https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white
[SqlServer-url]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
