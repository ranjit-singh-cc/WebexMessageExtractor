# Webex Chat and Recording Extractor

## Overview

This application, built on .NET 6, is designed to seamlessly extract Webex chat messages and recordings, creating a decent looking HTML file. Whether you need to archive important conversations or create a comprehensive record of your Webex meetings, this tool simplifies the process.

## Features

- **Chat Message Extraction:** Retrieve chat messages from Webex and export them in a readable format.
- **Recording Extraction:** Extract meeting recording links and include them in the final HTML file.
- **HTML File Generation:** Create a decent formatted HTML file with extracted chat messages and recordings.
- **User-Friendly Interface:** Simple command-line interface for easy usage and progress of extraction.

## Getting Started

If you are developer and wants to run the code by building it locally then you can follow installation steps else directly download the executable file [https://github.com/ranjit-singh-cc/WebexMessageExtractor/blob/master/executable/WebexMessageExtractor.zip](https://github.com/ranjit-singh-cc/WebexMessageExtractor/blob/master/executable/WebexMessageExtractor.zip)

## Configuration

appsettings.json file contains all the configuration, in most cases default value would be enough

- **BearerToken** This is PAT(Personal Access Token) which can easily be found at [https://developer.webex.com/docs/getting-your-personal-access-token](https://developer.webex.com/docs/getting-your-personal-access-token) . After login you will be able to see an option to copy the token
  ![image](https://github.com/ranjit-singh-cc/WebexMessageExtractor/assets/4026661/31e623a6-2867-4f8e-a09f-b2d2cad139f8)

  ***Please Note***: The token is valid for 12 hours! Then you have to get a new Personal Access Token.

  ***Additional Note***: If token is not provided in appsettings.json file then console application will ask to input it in runtime

- **MessageCountLimit** Maximum number of message that is possible in a direct/group chat
- **MinimumRecordingDate** "From date" for fetching the recording
- **MaximumRecordingDate** "To date" for fetching the recording
- **MaximumRecordingDataCount** Maximum number of recording data that will be fetched

## Snapshots
***Note***: All the dates are in local timezone

Recording will be visible like below

![image](https://github.com/ranjit-singh-cc/WebexMessageExtractor/assets/4026661/f2c01d9d-ae24-484e-81d8-9d3c1d7eddb2)

Chats will be visible like below

![image](https://github.com/ranjit-singh-cc/WebexMessageExtractor/assets/4026661/5a1c7d61-5577-40be-bdd7-95725096a27c)


## Prerequisites for developer building project locally

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- Your favorite code editor (e.g., Visual Studio, Visual Studio Code)

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/ranjit-singh-cc/WebexMessageExtractor.git
   cd WebexMessageExtractor
2. Build the project

   ```bash
   dotnet build

