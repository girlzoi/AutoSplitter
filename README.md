# Welcome to AutoSplitter!  



## How to use:  



Select a valid config from the dropdown menu and drag the pdf documents you want to split into the labelled box on the right. The file(s) that you drag into the box will be exported into a folder with the same name as the input file(s), in the same folder as the original document(s), containing \*.pdf files for each individual form specified by the config.  



### Config Files:  



Config Files (\*.cfg) tell the program how to split up your document into its individual forms. The program will not work unless all input documents are in the order specified in your config file and contain all pages, or else the document will be split up with the incorrect offset, or not split at all.  



The default config file, which is generated on program startup if you don't have any configs, is located in your Roaming AppData folder, which is located at C:\\Users\\\[username]\\AppData\\Roaming\\AutoSplitter\\Template.  



The syntax is as follows:  

Form-Name, First-Page, Last-Page  



Note: If your form is only one page long, then First-Page and Last-Page MUST equal each other.



### Buttons:  



#### "Open Config"/"Missing Forms":  



This button opens your config folder unless you have a config selected, otherwise it lets you select which forms are missing from a file you wish to run through the program. This is helpful if you usually process incomplete file-packets. 



#### "Split Every X Page"/"Select Config":  



This button toggles whether or not you would like to split your files(s) per every page, or by a config.  

Splitting every X pages is recommended when you have a stack forms with the same amount of pages, and splitting by config is recommended when you have file-packets with forms of differing page counts.



