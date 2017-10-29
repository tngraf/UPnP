Tethys.UPnP
==============

Libraries to explore UPnP network devices.

Did you ever wonder what DLNA and UPnP are? What the logos on your devices stand for?
The UPnP Analyzer application lets you discover the UPnP devices on your network,
shows the supported services and lets you execute the actions that are provided.

Your can browse through the files of your media libraries and download them on
your PC.

## Project Status ##
[![License](https://img.shields.io/badge/license-Apache--2.0-blue.svg)](http://www.apache.org/licenses/LICENSE-2.0)

## Solution Overview ##

* **Tethys.Upnp** - basic UPnP support, device discovery via SSPD, service detection,
  classes for devices, services, actions, and state variables. A basic SOAP implementation
  to execute service actions.
* **Tethys.Upnp.Services** - implementation of UPnP services. At the moment there is only 
  an implementation for the content directory service.
* **UpnpAnalyzer** - an application that uses the libraries to display all UPnP devices
  on the local network, show their services and also allows to execute all service actions.

## Terminology ##
* DIDL = Digital Item Description Language
* DLNA = Digital Living Network Alliance
* NT = Notification Type
* NTS = Notification Sub Type
* SCPD = Service Control Protocol Description
* SOAP = Simple Object Access Protocol
* SSDP = Simple Service Discovery Protocol
* ST = Search Type
* UPnP = Universal Plug & Play
* USN = Unique Service Name


License
-------
Copyright 2017 T. Graf

Licensed under the **Apache License, Version 2.0** (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and limitations under the License.
