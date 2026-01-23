# SKILL File

What is it
Skills are a knowledge base for claude code on how to do certain things. It is a .md file that should always be named SKILL.md.
Each skill is located at /.claude/skills/<skill-name>/SKILL.md
How to create
Here is an example pdf skill from Anthropic. It’s written in natural language along with code on how to read, extract, modify pdf files using python. So whenever claude code has to deal with pdfs it will refer to this skill file and execute the code.
Below is a simpler example pdf skill for reference.

```markdown
    ---
    name: pdf-processing
    description: Extract text, fill forms, merge PDFs. Use when working with PDF files, forms, or document extraction. Requires pypdf and pdfplumber packages.
    allowed-tools: Read, Bash(python:*)
    ---

    # PDF Processing

    ## Quick start

    Extract text:
    ```python
    import pdfplumber
    with pdfplumber.open("doc.pdf") as pdf:
        text = pdf.pages[0].extract_text()
    ```

    For form filling, see [FORMS.md](FORMS.md).
    For detailed API reference, see [REFERENCE.md](REFERENCE.md).

    ## Requirements

    Packages must be installed in your environment:

    ```bash
    pip install pypdf pdfplumber
    ```
```

It is recommended to keep SKILL.md under 500 lines. Add any additional content in other files and reference it in the SKILL.md. For e.g add the below line to extend the pdf skill to manipulate tables instead of bloating the original SKILL.md.

```markdown
For dealing with tables in pdfs, see [tables.md](tables.md)
```

When to use
Use a skill when claude code’s implicit knowledge about something is limited, wrong or doesn’t fit your project’s conventions.