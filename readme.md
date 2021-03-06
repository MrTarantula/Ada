# Ada

Ada is a simple static site generator written in ASP.Net Core. It will generate documentation from sets of Markdown files. This project was inspired by [ReadTheDocs](https://readthedocs.org), [Microsoft Docs](https://docs.microsoft.com), and [Sphinx](http://www.sphinx-doc.org/).

## Configuration

* SiteName - Title of your site
* InputPath - path to md files. Supports nested directories (somewhat).
* OutputPath - where you want the site created.
* TemplatePath - HTML templates. Right now the only files supported are `header.html` and `footer.html`.

## Document Template

Use the following as a template for your files. These are the only currently supported fields. It is highly recommended you include at least a `Title` and `Category`.

### YAML
```
Title: 
Description:
Category: 
Subcategory: 
History:
 - Author: Ryan Tauriainen
   ModifiedDate: 3/7/2017
 - Author: Ryan Tauriainen
   ModifiedDate: 3/3/2017
---


```

### JSON *coming soon but it will probably look like this*

```
{
	"Title": "",
	"Description": "",
	"Category": "",
	"Subcategory": "",
	"History": [{
		"Author": "",
		"ModifiedDate": ""
		},
		{
		"Author": "",
		"ModifiedDate": ""
	}]
}
---

```

## Planned Features

- [x] Markdown->HTML pipeline
- [x] Parsing YAML front matter in-document
- [x] Configurable input, output, and template directories
~~ - [x] Basic nav bar~~ (gutted this while working on advanced templates)
~~ - [x] Basic templating~~ (gutted this while working on advanced templates)
- [ ] Copy static assets (images, videos, etc.) *in progress*
- [ ] Advanced nav bar (multiple nesting) *in progress*
- [ ] Multiple templating engines *in progress*
- [ ] Theme packages *in progress*
- [ ] Bundling/minifying/injecting CSS and JS
- [ ] Web creation/submission form
- [ ] Editing from web
- [ ] Backup/export/import md files
- [ ] Logging

## Wishlist (not planned but welcome)

- [ ] Automatic API documentation generation
- [ ] JSON front matter
- [ ] reStructuredText->HTML pipeline
- [ ] Publish to gh-pages or other hosting sites
- [ ] .Net Standard 2.0 library
- [ ] Fluent API

## Bugs and Issues

- [x] Paths
- [x] Markdig advanced extensions