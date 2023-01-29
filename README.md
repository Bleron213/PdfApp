
<h1 align="center">
  <br>
  <br>
    PdfApp
  <br>
</h1>

<h4 align="center"> A small Web API built for converting HTML to PDF</h4>

<p align="center">

## Key Features

* ASP.NET 6.0 
* Minimal API
* Clean Architecture
* Header Authentication and Authorization
* Cross Platform
* Global Exception Handling
* Serilog integration
* Client SDK 
* Fully compliant with Dependency Injection
* Integration tested
* Unit tested

## How To Use

* Clone the git repo locally

```bash
# Clone this repository
$ git clone https://github.com/Bleron213/PdfApp
```

* Navigate to pdfapp.rest repository

```bash
# Navigate inside pdfapp.rest repository
cd PdfApp/PdfApp.Rest
```

* Initiate dotnet user-secret for development purposes

```bash
# Navigate inside pdfapp.rest repository
dotnet user-secrets set "X-API-KEY" "7a8a7cd837b042b58b56617114f4d3d7"
```

* Open the solution in Visual Studio 2022
* Start PdfApp.Rest project. If prompted for a certificate, use a self-signed development certificate. It is important to have a certificate before running docker-compose.
* After completing the previous step and having a dev certificate, you can shut down PdfApp.Rest and run <b>docker-compose up</b> at the solution level, or alternatively run docker-compose from the Visual Studio interface.

## How To Test

In the solution root, you can find a postman collection. Open it in Postman and you can send test requests.
Alternatively, you can run Integration or Unit tests inside Visual Studio
  
## Credits

This software uses the following open source packages:

- [.NET](https://github.com/dotnet)
- [Fluent Validation](https://github.com/FluentValidation/FluentValidation)
- [XUnit](https://github.com/xunit/xunit)
- [Serilog](https://github.com/serilog/serilog)
- [WkHtmlToPdf-DotNet](https://github.com/HakanL/WkHtmlToPdf-DotNet)
- [wkhtmltopdf](https://wkhtmltopdf.org/) (internally)
- [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer)
- [Moq](https://github.com/moq/moq)
