# <img src="https://github.com/NinjaRocks/FileUtil.Core/blob/master/ninja-icon-16.png" alt="ninja" style="width:30px;"/> FileUtil v3.0.0
[![NuGet version](https://badge.fury.io/nu/FixedWidth.FileParser.svg)](https://badge.fury.io/nu/FixedWidth.FileParser) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/NinjaRocks/FileUtil.Core/blob/master/License.md) [![CI](https://github.com/NinjaRocks/FileUtil.Core/actions/workflows/CI-Build.yml/badge.svg)](https://github.com/NinjaRocks/FileUtil.Core/actions/workflows/CI-Build.yml) [![GitHub Release](https://img.shields.io/github/v/release/ninjarocks/FileUtil.Core?logo=github&sort=semver)](https://github.com/ninjarocks/FileUtil.Core/releases/latest)
[![CodeQL](https://github.com/NinjaRocks/FileUtil.Core/actions/workflows/codeql.yml/badge.svg)](https://github.com/NinjaRocks/FileUtil.Core/actions/workflows/codeql.yml) [![.Net](https://img.shields.io/badge/.Net%20-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8)
-------------
#### .Net Library to read from fixed width or delimiter separated file using strongly typed objects.


**Fixed Width or Delimiter Separated File** 
------------------------------------------------------------------------
> What is Fixed width or Delimiter separated text files?

Fixed width or Delimiter separeted text file is a file that has a specific format which allows for the manipulation of textual information in an organized fashion.  
Each row contains one record of information; each record can contain multiple pieces of data fields or columns. The data columns are separated by any character you specify called the delimiter. All rows in the file follow a consistent format and should be with the same number of data columns. Data columns could be empty with no value.

**CASE 1 :** Simple pipe '|' separated Delimeter File is shown below (this could even be comma ',' separated CSV)

    |Mr|Jack Marias|Male|London|Active|||
    |Dr|Bony Stringer|Male|New Jersey|Active||Paid|
    |Mrs|Mary Ward|Female||Active|||
    |Mr|Robert Webb|||Active|||

**CASE 2:** The above file could have a header and a footer. 
In which case, each row has an identifier called as Line head to determine the type of row in the file. 

    |H|Department|Jun 23 2016  7:01PM|
    |D||Jack Marias|Male|London|Active|||
    |D|Dr|Bony Stringer|Male|New Jersey|Active||Paid|
    |D|Mrs|Mary Ward|Female||Active|||
    |D|Mr|Robert Webb|||Active|||
    |F|4 Records|

**FileUtil** can be used to parse both of the shown formats above. The line heads and data column delimiters (separators) are configurable as required per use case.

## Getting Started?

### i. Installation
Install the latest version of FileUtil nuget package with command below. 

```
NuGet\Install-Package FixedWidth.FileParser 
```

### ii. Developer Guide

Please read [Developer Guide](https://github.com/CodeShayk/FileUtil.Core/blob/master/DeveloperGuide.md) for details on how to implement ApiAggregator in your project.

## Support

If you are having problems, please let me know by [raising a new issue](https://github.com/CodeShayk/FileUtil.Core/issues/new/choose).

## License

This project is licensed with the [MIT license](LICENSE).

## Version History
The main branch is now on .NET 8.0. The following previous versions are available:
| Version  | Release Notes |
| -------- | --------|
| [`v3.0.0`](https://github.com/CodeShayk/FileUtil.Core/tree/v3.0.0) |  [Notes](https://github.com/CodeShayk/FileUtil.Core/releases/tag/v3.0.0) |
| [`v2.0.0`](https://github.com/CodeShayk/FileUtil.Core/tree/v2.0.0) |  [Notes](https://github.com/CodeShayk/FileUtil.Core/releases/tag/v2.0.0) |
| [`v1.0.0`](https://github.com/CodeShayk/FileUtil.Core/tree/v1.0.0) |  [Notes](https://github.com/CodeShayk/FileUtil.Core/releases/tag/v1.0.0) |

## Credits
Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)


