# Ada

Ada is a simple static site generator written in ASP.Net Core. It will generate documentation from sets of Markdown files. This project was inspired by [ReadTheDocs](https://readthedocs.org), [Microsoft Docs](https://docs.microsoft.com), and [Sphinx](http://www.sphinx-doc.org/).

## Configuration

There are 3 configurable paths:

* SiteName - Title of your site
* InputPath - path to md files. Supports nested directories (somewhat).
* OutputPath - where you want the site created.
* TemplatePath - HTML templates. Right now the only files supported are `header.html` and `footer.html`.

## Document Template

Use the following as a template for your files. These are the only currently supported fields. It is highly recommended you include at least a `Title` and `Category`.

```
Title: 
Description: 
Category: 
Author: 
Published: 
---


```

## Planned Features

- [x] Markdown->HTML pipeline
- [x] Parsing YAML front matter in-document
- [x] Configurable input, output, and template directories
- [x] Basic nav bar
- [x] Basic templating
- [ ] Copy static assets (images, videos, etc.)
- [ ] Advanced nav bar (multiple nesting)
- [ ] Advanced templating/theming
- [ ] Bundling/minifying/injecting CSS and JS
- [ ] Web creation/submission form
- [ ] Editing from web
- [ ] Backup/export/import md files
- [ ] Fluent API
- [ ] Logging

## Wishlist (not planned but welcome)

- [ ] Automatic API documentation generation
- [ ] JSON front matter
- [ ] reStructuredText->HTML pipeline
- [ ] Publish to gh-pages or other hosting sites

## Bugs and Issues

- [ ] Paths
- [x] Markdig advanced extensions