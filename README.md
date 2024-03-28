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

## Screenshot

# Departure
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/180b45f2-b983-40c2-84f2-dfe092fca95f)
# Seat Selection
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/810709f7-5059-4690-b179-e93e0ccf2b28)
# Passneger information
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/2c8b8576-37cf-4f7a-8743-2552bba91349)
# Summary and payment
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/0f0f012e-5b01-41e2-b4aa-7e1cd5fe2975)
# Booking history
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/a41ff6b4-eac5-4b85-b6d6-3f43c9b2d36b)
# Admin panel
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/3110c5a2-1676-4ed2-a6ba-83b70fb3fcec)
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/5ddfc5cd-3ce2-49d2-b5de-ecbfef63a3c4)
![image](https://github.com/HuyTranHoang/railway-reservation-management/assets/20708669/9090c33f-b583-4f6c-9c95-7678c7514641)


<!-- GETTING STARTED -->

## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites

This is an example of how to list things you need to use the software and how to install them.

- Dotnet: SDK 6.0.417, dotnet tool 6.0.25
  ```sh
  https://dotnet.microsoft.com/en-us/download/dotnet/6.0
  dotnet tool update --global dotnet-ef --version 6.0.25
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
