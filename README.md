# Hacker News Scraper
This is a command line application that can be used to scrape posts from Hacker News in JSON format.

## Requirements
This software has currently only been packaged for Windows and therefore can only be run on Windows machines.

## Installation
1. Download the latest hackernews.exe file from the [releases page](https://github.com/tamj0rd2/hacker-news-scraper/releases)
1. Move or copy paste the file to `C:\windows\system32`

### Validating the installation
1. Using the start menu, search for and open `cmd`
1. Type `hackernews` and press enter. If you see the below usage information, the app has been installed successfully.

## Usage
Command usage: `hackernews --posts n`
- `--post` is the flag to set the number of posts to display
-  `n` is the number of posts and must be from 1-100 (inclusive)

Posts will output to stdout in the following format:

```javascript
[
    {
        "title": "Web Scraping in 2016",
        "uri": "https://franciskim.co/2016/08/24/dont-need-no-stinking-api-web-scraping-2016-beyond/",
        "author": "franciskim",
        "points": 133,
        "comments": 80,
        "rank": 1
    },
    {
        "title": "Instapaper is joining Pinterest",
        "uri": "http://blog.instapaper.com/post/149374303661",
        "author": "ropiku",
        "points": 182,
        "comments": 99,
        "rank": 2
    }
]
```
