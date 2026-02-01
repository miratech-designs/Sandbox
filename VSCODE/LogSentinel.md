# The Silent Log Sentinel (Your Services Should Scream Before They Die)

You know what’s better than debugging production issues?
Not debugging them.

Meet the Log Sentinel — a script that watches logs in real time and notifies you when it sees early signs of failure:
- “timeout”
- “permission denied”
- “reconnecting”
- “deprecated”
- “traceback”
- “retrying in”

```python
import re
import time
import smtplib

PATTERNS = [
    r"timeout",
    r"error",
    r"traceback",
    r"deprecated",
    r"failed",
]
def send_alert(msg):
    with smtplib.SMTP("smtp.gmail.com", 587) as server:
        server.starttls()
        server.login("your_email", "your_password")
        server.sendmail("your_email", "your_email", f"Subject: Log Alert\n\n{msg}")
def monitor(log_file):
    with open(log_file, "r") as f:
        f.seek(0, 2)
        while True:
            line = f.readline()
            if not line:
                time.sleep(0.2)
                continue
            
            for p in PATTERNS:
                if re.search(p, line, re.I):
                    send_alert(f"Critical log event detected:\n{line}")
monitor("your-app.log")
```

## Why this matters:

Most failures give you warnings hours before the actual explosion.
You just never see them.

This script does.