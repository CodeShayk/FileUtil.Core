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

-------------


Using FileUtil
-------------


**Case 1:** Let us see how we can parse a delimiter separted file with no header and footer rows.
------------------------------------------------------------------------

 
**Note:** This file will have rows with no line head as all lines are of same type
*ie. default is type of data.*  

> For the example rows below, we can parse the file with just few lines of code and minimal configuration.
> 
    |Mr|Jack Marias|Male|London|
    |Dr|Bony Stringer|Male|New Jersey|
    |Mrs|Mary Ward|Female||
    |Mr|Robert Webb|||
> 

**Configuration**
-------------

We can configure the parser and provider setting for FileUtil to override the defaults.

The below configuration shows the complete set of attributes required to parse a delimiter separated file with no header or footer rows.

At a minimal you can specify the folder location `{"providerSettings":{ "folderPath":"C:work"}` where the source file will reside. You can override defaults as required.

By default, the file is read using the default file system provider. You can pass in your own implementation of the provider > to custom read file from desired location. We will later show how you can acheive this.


    {
        "configSettings":{
            "parserSettings": { "delimiter":{ "value":"|"} },
            "providerSettings": {
                    "folderPath":"C:work",
                    "fileNameFormat":"File*.txt",
                    "archiveUponRead":"true",
                    "archiveFolder":"Archived"
        }
    }	
}	  


 The  `"fileNameFormat":"File*.txt"` attribute (by default is empty) can
 be used to search the folder location for files with a specific name pattern.
 if not specified then all available files in the folder location will be searched.
 


**Code** 
-------------
To parse a row into a C# class, you need to implement **FileLine** abstract class.
By doing this you create a strongly typed line representation for each row in the file.

Consider the file below -
> 
    |Mr|Jack Marias|Male|London|
    |Dr|Bony Stringer|Male|New Jersey|
    |Mrs|Mary Ward|Female||
    |Mr|Robert Webb|||
> 

Let us create an employee class which will hold data for each row shown in the file above. The properties in the line class should match to the column index and data type of the fields of the row. 

We use the column attribute to specify the column index and can optionally specify a default value for the associated column should it be be empty. As a rule of thumb, the number of properties with column attributes should match the number of columns in the row else the engine will throw an exception. 

FileLine base class has an index property that holds the index of the parsed line relative to the whole file, an array of errors (should there be any column parsing failures) and type property to denote if the file is of type header, data or footer (default is data) 
 

     public class Employee : FileLine
        {
            [Column(0)]
            public string Title { get; set; }
            [Column(1)]
            public string Name { get; set; }
            [Column(2)]
            public string Sex { get; set; }
            [Column(3, "London")]
            public string Location { get; set; }
        } 

Once you have created the line class it is as simple as calling the Engine.GetFile() method as follows

>
   `var files = new Engine(configSettings).GetFiles<Employee>();`

The engine will parse the files found at the specified folder location and return a collection of 
 `File<Employee>` objects ie. one for each file parsed with an array of strongly typed lines (in this case Employee[]).

```
public class File<T> where T: FileLine
    {
        /// <summary>
        /// File meta data.
        /// </summary>
        public FileMeta FileMeta { get; set; }
        /// <summary>
        /// Strongly typed parsed lines.
        /// </summary>
        public T[] Data { get; set; }
       
    }
```

-------------


**Case 2:** Let us see how we can parse a delimiter separted file with header and footer rows.
------------------------------------------------------------------------

**Note:** This file will have rows with line head to determine each row type. By default, the line heads are 'H' for header, 'D' for data and 'F' for footer respectively. all these line heads are configurable via the config. 
> 
    |H|Department|Jun 23 2016  7:01PM|
    |D|Mr|Jack Marias|Male|London|
    |D|Dr|Bony Stringer|Male|New Jersey|
    |D|Mrs|Mary Ward|Female||
    |D|Mr|Robert Webb|||
    |F|4 Records|
> 

**Configuration**
-------------

The configuration is the same as before. We can override the default line heads by specifying the required line head attribute in the parser settings.

```
{
    "configSettings":{
        "parserSettings":{ "delimiter": { "value":"|"} ,
                           "lineHeaders": { 
                                "header":"H", 
                                "footer":"F", 
                                "data":"D" 
                            }
        },
        "providerSettings":{
                        "folderPath":"C:work",
                        "fileNameFormat":"File*.txt",
                        "archiveUponRead":"true",
                        "archiveFolder":"Archived"
        }		
    }	  
}
```

**Code** 
-------------

Like before we need a line class to map to each type of the row in the file
ie one for the header, footer and data line respectively.

We continue by creating two extra classes HeaderLine and FooterLine as follows.

     public class Header : FileLine
        {
            [Column(0)]
            public string Name { get; set; }
            [Column(1)]
            public DateTime Date { get; set; }
        } 

     public class Footer : FileLine
        {
            [Column(0)]
            public string FileRemarks { get; set; }
        } 


To parse the file you call the GetFiles() Method as follows -
> 
`var files = new Engine(configSettings).GetFiles<Header, Employee, Footer>();`

The engine will parse the files and return a collection of `File<Header, Employee, Footer>` objects 
ie. one for each file parsed with strongly typed header, footer and data line arrays.


-------------


**Custom File Provider:** Let us see how we can implement custom file provider for bespoke requirements.
------------------------------------------------------------------------

You can implement your own custom provider and pass it to the engine to provide delimiter separated file lines by implementing your own bespoke logic. This could be reading the contents from the datadase or over http, etc.

To implement a custom provider you need to implement **IFileProvide** interface

An example dummy implementation is as follows

     public class CustomProvider : IFileProvider
    {
        public FileMeta[] GetFiles()
        {
           // custom implementation to return file lines 
            return new[]
            {
                new FileMeta
                {
                    FileName = "Name",
                    FileSize = 100,
                    FilePath = "File Path",
                    Lines = new[] {"H|22-10-2016|Employee Status", "D|John Walsh|456RT4|True", "F|1"}
                }
            };
        }
    }

You can pass the custom provider to the engine as follows -

 `var files = new Engine(configSettings, new CustomProvider()).GetFiles<Employee>();`
 
 `var files = new Engine(configSettings, new CustomProvider()).GetFiles<Header, Employee, Footer>();`

Returns
```
public class File<TH, TD, TF> 
    {
        /// <summary>
        /// File meta data.
        /// </summary>
        public FileMeta FileMeta { get; set; }
        /// <summary>
        /// Parsed header lines.
        /// </summary>
        public TH Header { get; set; }
        /// <summary>
        /// Parsed data lines.
        /// </summary>
        public TD[] Data { get; set; }
        /// <summary>
        /// Parsed footer line.
        /// </summary>
        public TF Footer { get; set; }
    }
```

-------------


**Credits** 
-------------

Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)

