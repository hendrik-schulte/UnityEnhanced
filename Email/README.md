# Email

Allows sending email from WebGL builds using the https://smtpjs.com/ API. See their page for usage and token generation.

``` cs
using UE.Email;

Email.SendEmail(from, to, subject, body, smtp, user, password);

Email.SendEmailToken(from, to, subject, body, token);

```

## HTML

Use this class for easy HTML formating:

``` cs

"This is " + HTML.Bold("bold") + " and this is " + HTML.Italic("italic") + "." + HTML.P + 
"This is a new paragraph."


```

# Knows issues

- Sending mails with attachment does not work.
