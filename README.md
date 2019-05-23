# StationTracker
Windows service application that update remote Azure DB with local MS-Access Data every X minutes

Using the Topshelf framework for hosting services, this console app was created to test and debug the application.

For installation as service, you need to copy the binaries and paste them to the desired installation folder.
In Windows terminal (as administrator), navigate to the folder when you put the binaries and run the next command:
  binarie_name.exe install start

It will display the service name and the description that is defined in the Program.cs 

Then you can go to the Windows Services to check your service is already installed and running.

For more info about Topshelf, you can visit https://topshelf.readthedocs.io/en/latest/index.html
