# EHP-EditHostProgram

The "Edit Host Program" is a little tool to easily edit the hosts file in windows.

![Program](images/ehp.PNG)


# How does it work?

It gets all information from a "config.csv" file. A example of the csv file is in the config folder. <br>
**The config file and the executable needs to be in the same directory!** <br>
It needs to look something like this:

![Configfile](images/config.png)

The program will skip the first line, so you can use it as a comment line or leave it empty. <br>
The first column of the config file will show in the application combo box. The third column will shown the different systems in the second combo box. <br>
The ip address will show as a description. As soon you press change, the program get the application and the ip and set the entry like "ip_address application_name" in the hosts file.
