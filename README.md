# HomeNet VAuto Program Instructions

To run this project you will have to have [dotnet sdk 2.0](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.3-windows-x64-installer) installed. 

Alternatively you can use a version of Visual Studio that supports dotnet core. You can verify that you have 2.x sdk installed by typing `dotnet --version` in a shell.

## Instructions

1. clone the repository `git clone https://github.com/oleachka/VAutoAnswer.git`
1. `cd` into the root folder.
1. Run the following

    ```powershell
    dotnet restore        # restore nuget packages
    dotnet build          # build solution
    dotnet run -p Answer  # run the console app in project Answer.csproj
    ```

## Architecture

This solution has 2 projects
* A services project called `AutoClient`. This is where all the logic and models live that allow you to communicate with the API
* A console app project called `Answer`. This project consumes the `AutoClient` library to get the answer.

All comunication with the API is done via the `IVAutoService` interface. This way if this was a larger
project we could use dependency injection to inject it into a Controller of some sort. Since this is a tiny console app
we are just creating the instance manually. 

All operations that talk the the server API are `async` and utilize the `HttpClient` and `Newtonsoft.Json` packages to communicate and
serialize/deserialize data. All data is hydrated into objects defined in the `Models` folder of the `AutoClient` project.

### Performance

Performance is optimized by getting all the vehicles in parallel using the `.Net Task library`. This allows us to have all the vehicles retrieved in the same time that it would take to retrieve the longest request (4s). Same technique is used for retrieving dealers.

