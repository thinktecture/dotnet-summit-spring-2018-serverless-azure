# Sample for building modern cross-platform applications with Angular, Cordova & Electron

This project uses [Angular CLI](https://github.com/angular/angular-cli).


## Notes
The sample application uses the SWAPI (https://swapi.co/) and Pokéapi (https://pokeapi.co/).

## General Setup
* Download and install [node.js](https://nodejs.org/)
* Make sure you have the [git command line tools](https://git-scm.com/downloads) installed
* Optionally: download and install any editor of your choice (e.g. free [Visual Studio Code](https://code.visualstudio.com/); commercial [Sublime Text](https://www.sublimetext.com/), [WebStorm](https://www.jetbrains.com/webstorm/))

---
The steps mentioned in the *Setup for Native Applications* section are optional. Those steps are only required if you want to run the app either on any mobile or desktop operating system as native (or hybrid) applications.

If you're just interested in running the web SPA within the browser, go ahead and follow the *Building & Running - Web* section and focus on the web-related tasks.

---

## Setup for Native Applications
* Download and install the platform SDKs and/or emulators for the mobile platform you want to develop for (this might take quite a while… so do this first!)
  * [XCode](https://developer.apple.com/xcode/download/)
  * [Android SDK](https://developer.android.com/sdk/index.html)
  * [Windows 10 SDK](https://dev.windows.com/en-us/downloads/windows-10-sdk)
* Download and install [ImageMagick](http://www.imagemagick.org/script/binary-releases.php) (base toolkit for image processing, used here for splash screen and icon generation)
* Install Cordova: `npm install -g cordova`
* MacOSX and Linux users might need to install [Wine](https://wiki.winehq.org/) (for executing the Electron Windows build task, if needed)

## Building & Running
The npm scripts will build iOS, Windows UWP, Android apps as well as desktop applications for Mac OSX, Windows and Linux.
To get it working, please do the following:

* After cloning the repo: `npm i --no-progress` within the root folder of this repository

Please have a look at all the available npm scripts in package.json to build various deliverables of the application. The following is just a sample list.

### Web

* Run `npm start` to start a live server based on the Angular CLI, which is best when developing the app

### Cordova

To build the Cordova project use one of the following commands to start:
* `npm run build-mobile-ios`: Builds the iOS version. Requires a Mac and the iOS SDK.
* `npm run build-mobile-android`: Builds the Android version. Requires Android SDK to be installed and at least a simulator.

### Electron

To build the Electron packaged app, do this:
* `npm run build-desktop`: Builds all desktop OS versions of the app (Linux, Windows, macOS)..


### Questions?
Ask Christian Weyer, christian.weyer@thinktecture.com
