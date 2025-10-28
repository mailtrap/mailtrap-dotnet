# Contributing to Mailtrap .NET Client
First of all, thank you for considering contributing to Mailtrap .NET Client!  
We are excited to have you here :tada:  
Any contribution is welcome and appreciated, be it a bug report, a feature request, a documentation update, or a code change.


## Code of Conduct
This project and everyone participating in it is governed by the [Code of Conduct](CODE_OF_CONDUCT.md).  
By participating, you are expected to uphold this code.  
Please report unacceptable behavior to [support@mailtrap.io](mailto:support@mailtrap.io).


## License
This project is licensed under the [MIT License](LICENSE.md).  
Please consider that your contributions should meet its terms and conditions.  
Also, by participating, you give your permission to license your contributions into this project under its terms and conditions.


## Report an issue or propose improvement
Kindly consider searching for already existing issue or feature proposal before creating a new one.  
Do not increase the entropy unnecessarily - the Universe will appreciate it.

If it is still the case - please use [Issues](https://github.com/mailtrap/mailtrap-dotnet/issues) section of the repo to file an issue, 
possible improvement or feature proposal.  


## Contribute to the code or documentation

### Prerequisites
To be able to make edits, build and run your changes locally, you will need the following:
- Code editor or IDE of your choice, with C# language and .NET 9.x support.  
This repository uses [EditorConfig](https://aka.ms/editorconfigdocs) settings to enforce consistent code style, 
thus usage of editor which respects them is highly encouraged.  
[Visual Studio 2022](https://visualstudio.microsoft.com/) is one of recommended options.  

- [.NET SDK v9.x](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

- Plain text editor of your choice - for documentation updates.  
Ideally, with [Markdown](https://en.wikipedia.org/wiki/Markdown) support.

- [docfx](https://dotnet.github.io/docfx/docs/) - to build and validate documentation website.


### Pull requests
Please consider that contributions to the repository content are managed through pull requests.  
Thus you will need to follow steps below to create a new one:  

1. Clone the repo
```bat
git clone https://github.com/mailtrap/mailtrap-dotnet.git
```

2. Create new branch
```bat
git checkout -b <branch-name>
```
Please consider using the following pattern for branch naming:  
`[change-type]/[issue-id]-[change-description]`  
where:  
`[change-type]` - one of the following: `feat[ure]`, `fix`, `docs`, `devops`, `test`, `refactor`, etc.  
`[issue-id]` - numeric ID of the issue that you are going to address, if available.  
`[change-description]` - short description of the change, in [kebab-case](https://en.wikipedia.org/wiki/Letter_case#Kebab_case)

3. Make changes, validate them.

4. Create commits with a description of changes made.  
Usage of [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/#summary) is encouraged.

5. Push to the remote.

6. Finally, create a pull request to the [main](https://github.com/mailtrap/mailtrap-dotnet/tree/main) branch, 
using the template provided.  
Please consider providing detailed description what was changed and validating PR checklist to streamline the PR review.


### Code
This SDK is targeting [.NET Standard 2.0](https://dotnet.microsoft.com/platform/dotnet-standard#versions)
thus not all latest C# language features are supported.  
Only C# v7.3 features support is guaranteed.  
Later version features should be used with care and their runtime behavior should be thoroughly tested.

Adding unit/integration tests is encouraged for every added/changed functionality.  
Although we are not targeting 100% coverage, ensuring everything is working as expected is important.


### Documentation
You can improve SDK documentation in several ways.

#### Add or extend [XMLDoc](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/) comments in the code
They are used to show inline help in IDE and to generate SDK API reference section of documentation website,
thus are a crucial part of the SDK.

#### Update markdown files in the [docs](https://github.com/mailtrap/mailtrap-dotnet/tree/main/docs) folder of the repo.  
Considering that [docfx](https://dotnet.github.io/docfx/docs/basic-concepts.html) tool is used to create documentation website
from sources, please ensure that you are using [supported syntax](https://dotnet.github.io/docfx/docs/markdown.html).

To build and verify documentation locally, before committing your changes:
1. Install/update `docfx` as a global tool
```bat
dotnet tool (install|update) -g docfx
```
2. Build and serve documentation website
```bat
cd <repository-path>
docfx docs/docfx.json --serve
```
3. Finally, open <http://localhost:8080> in your browser and navigate to the changed section.
