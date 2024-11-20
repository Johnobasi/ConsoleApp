# Approach

Set the Startup Project:

Right-click on your project in the Solution Explorer and select "Set as Startup Project".
Add Command-Line Arguments:

Right-click on the project in Solution Explorer and select "Properties".
Go to the Debug tab.
In the Application Arguments or Command-line Arguments field, enter the arguments for your program:
lua
Copy code
input.txt output.txt


Install the Required NuGet Package

Add the System.Text.Encoding.CodePages package to your project. This package provides support for code pages such as windows-1252.
Use the following command in the Package Manager Console or terminal:
bash
Copy code
dotnet add package System.Text.Encoding.CodePages
