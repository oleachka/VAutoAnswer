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

